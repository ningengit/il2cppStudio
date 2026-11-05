using System.Text;
using Il2CppStudio.Core.Analysis;
using Il2CppStudio.Core.Backup;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.CodeGen;

/// <summary>
/// Generates synthetic C/C++ that follows Unity IL2CPP's code generation conventions (as emitted by
/// Unity.IL2CPP's CppDeclarationsWriter / MethodWriter):
/// <list type="bullet">
/// <item>classes as <c>struct Name_t : public Parent_t</c> (root: <c>RuntimeObject</c>), value types as plain structs;</item>
/// <item>instance fields named <c>___name</c> at their exact native offsets (from Il2CppMetadataRegistration.fieldOffsets);</item>
/// <item>statics in a separate <c>Name_StaticFields</c> struct reached through <c>klass->static_fields</c>;</item>
/// <item>methods as <c>IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR ret Type_Method_mXXXX (Type_t* __this, …, const RuntimeMethod* method)</c>.</item>
/// </list>
/// In Enhanced mode the real symbol names (from *_CodeGen.c) and the original generated bodies are used; otherwise
/// bodies are pseudo-C++ annotated disassembly.
/// </summary>
public sealed class CppHeaderGenerator(
    GlobalMetadata md, MetadataModel model, Il2CppRegistration? reg, TypeNameService names, MethodAddressMap? map,
    Func<int, string?>? disassemble, BackupSourceIndex? backupSources, BuildBackupInfo? backup)
{
    private readonly Dictionary<string, CodeGenModule> _backupModules =
        backup?.CodeGenModules.ToDictionary(m => m.AssemblyName, StringComparer.OrdinalIgnoreCase) ?? [];

    // ------------------------------------------------------------------ types

    public string GenerateTypeHeader(TypeModel type, bool includeMethods = true)
    {
        var sb = new StringBuilder();
        var def = type.Raw;
        var structName = names.CppStructName(type.Index);
        sb.AppendLine($"// {type.Kind} {type.FullName}  [{type.ImageName}]  token 0x{type.Token:X8}  typedef #{type.Index}");
        if (reg is not null && reg.GetInstanceSize(type.Index) is var size and > 0)
            sb.AppendLine($"// instance_size 0x{size:X}{(def.IsValueType ? $" (unboxed 0x{size - 16:X})" : "")}");

        var parent = ParentTypeDef(def);
        var inherit = def.IsValueType || def.IsInterface ? "" : parent >= 0 ? $" : public {names.CppStructName(parent)}" : " : public RuntimeObject";
        if (def.IsEnum)
        {
            var underlying = type.Fields.FirstOrDefault(f => f.Name == "value__");
            sb.AppendLine($"struct {structName}");
            sb.AppendLine("{");
            if (underlying is not null) sb.AppendLine($"\t{names.CppTypeName(underlying.TypeIndex)} ___value__;");
            sb.AppendLine("};");
            AppendEnumConstants(sb, type);
        }
        else
        {
            sb.AppendLine($"struct {structName}{inherit}");
            sb.AppendLine("{");
            var instance = FieldLayout(type, statics: false);
            var expected = def.IsValueType ? 0 : -1;
            foreach (var (f, off) in instance)
            {
                if (expected >= 0 && off > expected) sb.AppendLine($"\tuint8_t ___padding_0x{expected:X}[{off - expected}];");
                sb.AppendLine($"\t{names.CppTypeName(f.TypeIndex)} ___{f.Name}; // 0x{off:X}");
                expected = -1;
            }
            if (instance.Count == 0) sb.AppendLine(def.IsValueType ? "\tunion { struct { }; uint8_t U1__padding[1]; };" : "");
            sb.AppendLine("};");

            var statics = FieldLayout(type, statics: true);
            if (statics.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine($"struct {TypeNameService.StripArity(structName)[..^2]}_StaticFields");
                sb.AppendLine("{");
                foreach (var (f, off) in statics) sb.AppendLine($"\t{names.CppTypeName(f.TypeIndex)} ___{f.Name}; // 0x{off:X}");
                sb.AppendLine("};");
            }
        }

        if (includeMethods && type.Methods.Count > 0)
        {
            sb.AppendLine();
            foreach (var m in type.Methods)
            {
                var rva = map?.GetMethodRva(m.Index) ?? 0;
                sb.AppendLine($"{MethodPrototype(type, m)};{(rva != 0 ? $" // RVA 0x{rva:X}" : " // no native code")}");
            }
        }
        return sb.ToString();
    }

    private void AppendEnumConstants(StringBuilder sb, TypeModel type)
    {
        foreach (var f in type.Fields.Where(f => f.Name != "value__"))
            sb.AppendLine($"// {names.CppStructName(type.Index)}::{f.Name}");
    }

    private int ParentTypeDef(Il2CppTypeDefinition def) =>
        def.ParentIndex >= 0 && reg is not null && reg.TryGetType(def.ParentIndex, out var p) ? names.ResolveTypeDefIndex(p) : -1;

    /// <summary>Declared fields with native offsets (instance or static), ordered by offset.</summary>
    public List<(FieldModel Field, int Offset)> FieldLayout(TypeModel type, bool statics)
    {
        var list = new List<(FieldModel, int)>();
        for (var k = 0; k < type.Fields.Count; k++)
        {
            var f = type.Fields[k];
            var attrs = reg is not null && reg.TryGetType(f.TypeIndex, out var t) ? t.Attrs : (ushort)0;
            var isStatic = (attrs & 0x10) != 0;
            if ((attrs & 0x40) != 0 || isStatic != statics) continue;   // skip literal constants
            var off = reg?.GetFieldOffset(type.Index, k) ?? -1;
            if (off < 0) continue;
            if (!statics && type.Raw.IsValueType) off -= 16;
            list.Add((f, off));
        }
        list.Sort((a, b) => a.Item2.CompareTo(b.Item2));
        return list;
    }

    // ------------------------------------------------------------------ methods

    /// <summary>The generated C symbol: the real one from the build backup, else Type_Method_m{token}.</summary>
    public string MethodSymbol(TypeModel type, MethodModel m)
    {
        if (_backupModules.TryGetValue(type.ImageName, out var mod) && mod.SymbolForToken(m.Token) is { } s && s is not ("NULL" or "0"))
            return s;
        return $"{TypeNameService.StripArity(names.CppStructName(type.Index))[..^2]}_{TypeNameService.Mangle(m.Name)}_m{m.Token:X8}";
    }

    public string MethodPrototype(TypeModel type, MethodModel m)
    {
        var ret = names.CppTypeName(m.ReturnTypeIndex);
        var ps = new List<string>();
        if (!m.IsStatic) ps.Add($"{names.CppStructName(type.Index)}{(type.Raw.IsValueType ? "*" : "*")} __this");
        for (var i = 0; i < m.ParameterTypeIndices.Count; i++)
            ps.Add($"{names.CppTypeName(m.ParameterTypeIndices[i])} ___{m.ParameterNames[i]}");
        ps.Add("const RuntimeMethod* method");
        return $"IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR {ret} {MethodSymbol(type, m)} ({string.Join(", ", ps)})";
    }

    /// <summary>Full C++ view of one method: original generated source (Enhanced) or annotated pseudo-C++.</summary>
    public string GenerateMethod(TypeModel type, MethodModel m)
    {
        var sb = new StringBuilder();
        var rva = map?.GetMethodRva(m.Index) ?? 0;
        sb.AppendLine($"// {type.FullName}::{m.DisplaySignature}");
        sb.AppendLine($"// token 0x{m.Token:X8}  method #{m.Index}  {(rva != 0 ? $"RVA 0x{rva:X}" : "no native code (abstract, stripped or generic definition)")}");

        var symbol = MethodSymbol(type, m);
        if (backupSources?.GetDefinition(symbol) is { } original)
        {
            sb.AppendLine("// Source: build backup il2cppOutput (original Unity-generated C++)");
            sb.Append(original);
            return sb.ToString();
        }

        sb.AppendLine("// Source: native heuristic recovery (pseudo-C++ over annotated x86-64)");
        sb.AppendLine(MethodPrototype(type, m));
        sb.AppendLine("{");
        if (rva != 0 && disassemble?.Invoke(m.Index) is { } asm)
        {
            foreach (var line in asm.Split('\n', StringSplitOptions.RemoveEmptyEntries))
                sb.Append("\t// ").AppendLine(line.TrimEnd('\r'));
        }
        else sb.AppendLine("\tIL2CPP_UNREACHABLE;");
        sb.AppendLine("}");
        return sb.ToString();
    }

    /// <summary>All types of one image as a single header (Export C++ for an assembly).</summary>
    public string GenerateImageHeader(AssemblyModel asm)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"// {asm.Name} v{asm.Version} — generated by Il2CppStudio");
        sb.AppendLine("#pragma once");
        sb.AppendLine("#include \"il2cpp-codegen.h\"");
        sb.AppendLine();
        foreach (var ns in asm.Namespaces)
            foreach (var t in ns.Types)
                AppendRecursive(sb, t);
        return sb.ToString();
    }

    private void AppendRecursive(StringBuilder sb, TypeModel t)
    {
        sb.AppendLine(GenerateTypeHeader(t));
        foreach (var n in t.NestedTypes) AppendRecursive(sb, n);
    }
}
