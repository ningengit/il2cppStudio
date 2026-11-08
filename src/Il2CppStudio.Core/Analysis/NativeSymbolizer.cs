using Il2CppStudio.Core.Metadata;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.Analysis;

/// <summary>Describes native addresses: method symbols, runtime metadata globals (string literals, TypeInfo, ...).</summary>
public sealed class NativeSymbolizer(GlobalMetadata md, MetadataModel model, Il2CppRegistration reg, TypeNameService names, MethodAddressMap map)
{
    public GlobalMetadata Metadata => md;
    public MetadataModel Model => model;
    public Il2CppRegistration Registration => reg;
    public TypeNameService Names => names;
    public MethodAddressMap Map => map;

    public bool TryGetUsage(ulong va, out MetadataUsage usage) => reg.MetadataUsages.TryGetValue(va, out usage);

    public string? Describe(ulong va)
    {
        if (map.SymbolAt(va) is { } sym) return sym.CppSymbol ?? sym.DisplayName;
        if (reg.MetadataUsages.TryGetValue(va, out var u)) return DescribeUsage(u);
        return null;
    }

    public string DescribeUsage(MetadataUsage u) => u.Kind switch
    {
        MetadataUsageKind.StringLiteral => $"\"{Escape(md.GetStringLiteral(u.Index))}\"",
        MetadataUsageKind.TypeInfo => $"{names.CSharpName(u.Index)}_TypeInfo",
        MetadataUsageKind.Il2CppType => $"typeof({names.CSharpName(u.Index)})",
        MetadataUsageKind.MethodDef => $"{MethodName(u.Index)}_RuntimeMethod",
        MetadataUsageKind.MethodRef => (uint)u.Index < (uint)reg.MethodSpecs.Count
            ? MethodAddressMap.SpecDisplayName(md, model, reg, names, reg.MethodSpecs[u.Index]) + "_RuntimeMethod"
            : $"methodspec#{u.Index}",
        MetadataUsageKind.FieldInfo or MetadataUsageKind.FieldRva => FieldRefName(u.Index) + "_FieldInfo",
        _ => $"usage#{u.Index}",
    };

    public string MethodName(int methodIndex)
    {
        if ((uint)methodIndex >= (uint)md.Methods.Length) return $"method#{methodIndex}";
        var m = md.Methods[methodIndex];
        var owner = (uint)m.DeclaringType < (uint)model.Types.Count ? model.Types[m.DeclaringType].FullName : "?";
        return $"{owner}::{md.GetString(m.NameIndex)}";
    }

    public string FieldRefName(int fieldRefIndex)
    {
        if ((uint)fieldRefIndex >= (uint)md.FieldRefs.Length) return $"fieldref#{fieldRefIndex}";
        var fr = md.FieldRefs[fieldRefIndex];
        if (!reg.TryGetType(fr.TypeIndex, out var t)) return $"fieldref#{fieldRefIndex}";
        var td = names.ResolveTypeDefIndex(t);
        if (td < 0) return $"fieldref#{fieldRefIndex}";
        var def = md.TypeDefinitions[td];
        var fi = def.FieldStart + fr.FieldIndex;
        return (uint)fi < (uint)md.Fields.Length ? $"{model.Types[td].FullName}::{md.GetString(md.Fields[fi].NameIndex)}" : $"fieldref#{fieldRefIndex}";
    }

    public static string Escape(string s)
    {
        var e = s.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t");
        return e.Length > 120 ? e[..120] + "…" : e;
    }
}
