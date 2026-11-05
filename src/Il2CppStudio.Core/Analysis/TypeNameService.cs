using System.Collections.Concurrent;
using System.Text;
using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.Analysis;

/// <summary>
/// Turns Il2CppType records into C# display names and Unity-convention C++ type names. C++ naming follows
/// il2cpp's NamingComponent: identifier characters are kept, '`' and '.' become '_', anything else is
/// escaped as U+hex (e.g. "[]" → "U5BU5D"), and types get the "_t" suffix (Unity appends a SHA1 hash
/// of the full name after "_t"; that hash is only reproduced when the build backup provides the real names).
/// </summary>
public sealed class TypeNameService(GlobalMetadata md, MetadataModel model, Il2CppRegistration? reg)
{
    private readonly ConcurrentDictionary<ulong, string> _csCache = new();

    public static readonly IReadOnlyDictionary<Il2CppTypeEnum, string> CSharpPrimitives = new Dictionary<Il2CppTypeEnum, string>
    {
        [Il2CppTypeEnum.Void] = "void", [Il2CppTypeEnum.Boolean] = "bool", [Il2CppTypeEnum.Char] = "char",
        [Il2CppTypeEnum.I1] = "sbyte", [Il2CppTypeEnum.U1] = "byte", [Il2CppTypeEnum.I2] = "short", [Il2CppTypeEnum.U2] = "ushort",
        [Il2CppTypeEnum.I4] = "int", [Il2CppTypeEnum.U4] = "uint", [Il2CppTypeEnum.I8] = "long", [Il2CppTypeEnum.U8] = "ulong",
        [Il2CppTypeEnum.R4] = "float", [Il2CppTypeEnum.R8] = "double", [Il2CppTypeEnum.String] = "string",
        [Il2CppTypeEnum.I] = "nint", [Il2CppTypeEnum.U] = "nuint", [Il2CppTypeEnum.Object] = "object", [Il2CppTypeEnum.TypedByRef] = "TypedReference",
    };

    public static readonly IReadOnlyDictionary<Il2CppTypeEnum, string> CppPrimitives = new Dictionary<Il2CppTypeEnum, string>
    {
        [Il2CppTypeEnum.Void] = "void", [Il2CppTypeEnum.Boolean] = "bool", [Il2CppTypeEnum.Char] = "Il2CppChar",
        [Il2CppTypeEnum.I1] = "int8_t", [Il2CppTypeEnum.U1] = "uint8_t", [Il2CppTypeEnum.I2] = "int16_t", [Il2CppTypeEnum.U2] = "uint16_t",
        [Il2CppTypeEnum.I4] = "int32_t", [Il2CppTypeEnum.U4] = "uint32_t", [Il2CppTypeEnum.I8] = "int64_t", [Il2CppTypeEnum.U8] = "uint64_t",
        [Il2CppTypeEnum.R4] = "float", [Il2CppTypeEnum.R8] = "double", [Il2CppTypeEnum.String] = "String_t*",
        [Il2CppTypeEnum.I] = "intptr_t", [Il2CppTypeEnum.U] = "uintptr_t", [Il2CppTypeEnum.Object] = "RuntimeObject*", [Il2CppTypeEnum.TypedByRef] = "TypedReference_t",
    };

    public Il2CppRegistration? Registration => reg;

    // ------------------------------------------------------------------ C#

    public string CSharpName(int typeIndex)
    {
        if (reg is null || !reg.TryGetType(typeIndex, out var t)) return $"/*type#{typeIndex}*/object";
        return CSharpName(t);
    }

    public string CSharpName(Il2CppTypeRecord t)
    {
        if (_csCache.TryGetValue(t.Va, out var cached)) return cached;
        var name = (t.ByRef ? "ref " : "") + CSharpNameCore(t, 0);
        _csCache[t.Va] = name;
        return name;
    }

    private string CSharpNameCore(Il2CppTypeRecord t, int depth)
    {
        if (depth > 16 || reg is null) return "?";
        if (CSharpPrimitives.TryGetValue(t.Type, out var p)) return p;
        switch (t.Type)
        {
            case Il2CppTypeEnum.Class or Il2CppTypeEnum.ValueType:
                return TypeDefName(t.KlassIndex, includeNamespace: false);
            case Il2CppTypeEnum.Var or Il2CppTypeEnum.MVar:
                return GenericParameterName(t.GenericParameterIndex);
            case Il2CppTypeEnum.SzArray:
                return CSharpNameCore(reg.GetTypeAt(t.Data), depth + 1) + "[]";
            case Il2CppTypeEnum.Ptr:
                return CSharpNameCore(reg.GetTypeAt(t.Data), depth + 1) + "*";
            case Il2CppTypeEnum.Array:
            {
                var a = reg.ReadArrayType(t.Data);
                return CSharpNameCore(reg.GetTypeAt(a.ElementTypeVa), depth + 1) + "[" + new string(',', Math.Max(0, a.Rank - 1)) + "]";
            }
            case Il2CppTypeEnum.GenericInst:
            {
                var gc = reg.ReadGenericClass(t.Data);
                var def = reg.GetTypeAt(gc.TypeVa);
                var baseName = StripArity(CSharpNameCore(def, depth + 1));
                var inst = reg.ReadGenericInst(gc.ClassInstVa);
                return $"{baseName}<{string.Join(", ", inst.TypeArgVas.Select(a => CSharpNameCore(reg.GetTypeAt(a), depth + 1)))}>";
            }
            default:
                return $"/*{t.Type}*/object";
        }
    }

    public string TypeDefName(int typeDefIndex, bool includeNamespace)
    {
        if ((uint)typeDefIndex >= (uint)model.Types.Count) return $"/*typedef#{typeDefIndex}*/object";
        var type = model.Types[typeDefIndex];
        var simple = StripArity(type.Name);
        if (type.DeclaringType is not null) return TypeDefName(type.DeclaringType.Index, includeNamespace) + "." + simple;
        return includeNamespace && type.Namespace.Length > 0 ? $"{type.Namespace}.{simple}" : simple;
    }

    public string GenericParameterName(int index) =>
        (uint)index < (uint)md.GenericParameters.Length ? md.GetString(md.GenericParameters[index].NameIndex) : $"T{index}";

    /// <summary>The typedef index a type ultimately refers to (generic instances → their definition), or -1.</summary>
    public int ResolveTypeDefIndex(Il2CppTypeRecord t)
    {
        if (reg is null) return -1;
        return t.Type switch
        {
            Il2CppTypeEnum.Class or Il2CppTypeEnum.ValueType => t.KlassIndex,
            Il2CppTypeEnum.GenericInst => ResolveTypeDefIndex(reg.GetTypeAt(reg.ReadGenericClass(t.Data).TypeVa)),
            _ => -1,
        };
    }

    // ------------------------------------------------------------------ C++

    public string CppTypeName(int typeIndex)
    {
        if (reg is null || !reg.TryGetType(typeIndex, out var t)) return "RuntimeObject*";
        return CppTypeName(t);
    }

    public string CppTypeName(Il2CppTypeRecord t)
    {
        var core = CppTypeNameCore(t, 0);
        return t.ByRef ? core + "*" : core;
    }

    private string CppTypeNameCore(Il2CppTypeRecord t, int depth)
    {
        if (depth > 16 || reg is null) return "RuntimeObject*";
        if (CppPrimitives.TryGetValue(t.Type, out var p)) return p;
        switch (t.Type)
        {
            case Il2CppTypeEnum.Class:
                return CppStructName(t.KlassIndex) + "*";
            case Il2CppTypeEnum.ValueType:
                return (uint)t.KlassIndex < (uint)model.Types.Count && model.Types[t.KlassIndex].Raw.IsEnum
                    ? CppStructName(t.KlassIndex)
                    : CppStructName(t.KlassIndex);
            case Il2CppTypeEnum.Var or Il2CppTypeEnum.MVar:
                return "Il2CppFullySharedGenericAny /*" + GenericParameterName(t.GenericParameterIndex) + "*/";
            case Il2CppTypeEnum.SzArray or Il2CppTypeEnum.Array:
                return Mangle(CSharpNameCore(t, 0)) + "_t*";
            case Il2CppTypeEnum.Ptr:
                return CppTypeNameCore(reg.GetTypeAt(t.Data), depth + 1) + "*";
            case Il2CppTypeEnum.GenericInst:
            {
                var gc = reg.ReadGenericClass(t.Data);
                var def = reg.GetTypeAt(gc.TypeVa);
                var isValue = def.Type == Il2CppTypeEnum.ValueType;
                var defIndex = ResolveTypeDefIndex(def);
                var raw = defIndex >= 0 ? model.Types[defIndex].Name : "Generic";
                return Mangle(raw) + "_t" + (isValue ? "" : "*") + " /*" + CSharpNameCore(t, 0) + "*/";
            }
            default:
                return "RuntimeObject*";
        }
    }

    /// <summary>Unity-style struct name for a type definition, e.g. "List_1_t" or "Player_t".</summary>
    public string CppStructName(int typeDefIndex)
    {
        if ((uint)typeDefIndex >= (uint)model.Types.Count) return "RuntimeObject";
        var t = model.Types[typeDefIndex];
        var prefix = t.DeclaringType is null ? "" : Mangle(t.DeclaringType.Name) + "_";
        return prefix + Mangle(t.Name) + "_t";
    }

    public static string Mangle(string name)
    {
        var sb = new StringBuilder(name.Length + 8);
        foreach (var c in name)
        {
            if (char.IsAsciiLetterOrDigit(c) || c == '_') sb.Append(c);
            else if (c is '`' or '.') sb.Append('_');
            else sb.Append('U').Append(((int)c).ToString("X"));
        }
        if (sb.Length > 0 && char.IsAsciiDigit(sb[0])) sb.Insert(0, '_');
        return sb.ToString();
    }

    public static string StripArity(string n) => n.IndexOf('`') is var i and >= 0 ? n[..i] : n;
}
