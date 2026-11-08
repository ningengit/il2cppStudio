using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Metadata;

public sealed record MetadataStatistics(
    string Version, string UnityRange, int FileSize, int Images, int Assemblies, int Types, int Methods,
    int Fields, int Parameters, int Properties, int Events, int GenericContainers, int GenericParameters,
    int StringLiterals, int VTableSlots, int Interfaces, int NestedTypes, int FieldRefs, int CustomAttributeEntries);

public sealed class MethodModel
{
    public required int Index { get; init; }
    public required string Name { get; init; }
    public required uint Token { get; init; }
    public required ushort Flags { get; init; }
    public required ushort ImplFlags { get; init; }
    public required ushort Slot { get; init; }
    public required IReadOnlyList<string> ParameterNames { get; init; }
    public required IReadOnlyList<string> GenericParameters { get; init; }

    /// <summary>Native Il2CppType indices (resolved to names once the binary registration is parsed, Phase 2).</summary>
    public required int ReturnTypeIndex { get; init; }
    public required IReadOnlyList<int> ParameterTypeIndices { get; init; }

    public bool IsStatic => (Flags & 0x0010) != 0;
    public bool IsVirtual => (Flags & 0x0040) != 0;
    public bool IsAbstract => (Flags & 0x0400) != 0;
    public string Access => (Flags & 0x7) switch
    {
        1 => "private", 2 => "private protected", 3 => "internal", 4 => "protected", 5 => "protected internal", 6 => "public", _ => "compilercontrolled",
    };

    public string DisplaySignature =>
        Name + (GenericParameters.Count > 0 ? $"<{string.Join(", ", GenericParameters)}>" : "") + $"({string.Join(", ", ParameterNames)})";
}

public sealed record FieldModel(int Index, string Name, uint Token, int TypeIndex);

public sealed record PropertyModel(int Index, string Name, uint Token, string? GetterName, string? SetterName);

public sealed record EventModel(int Index, string Name, uint Token, int TypeIndex);

public sealed class TypeModel
{
    public required int Index { get; init; }
    public required string Name { get; init; }
    public required string Namespace { get; init; }
    public required uint Token { get; init; }
    public required uint Flags { get; init; }
    public required Il2CppTypeDefinition Raw { get; init; }
    public required IReadOnlyList<string> GenericParameters { get; init; }
    public TypeModel? DeclaringType { get; internal set; }
    public List<TypeModel> NestedTypes { get; } = [];
    public List<MethodModel> Methods { get; } = [];
    public List<FieldModel> Fields { get; } = [];
    public List<PropertyModel> Properties { get; } = [];
    public List<EventModel> Events { get; } = [];
    public required string ImageName { get; init; }

    public string Kind => Raw.IsInterface ? "interface" : Raw.IsEnum ? "enum" : Raw.IsValueType ? "struct" : "class";

    public string DisplayName => GenericParameters.Count == 0 ? Name : $"{StripArity(Name)}<{string.Join(", ", GenericParameters)}>";

    public string FullName => DeclaringType is { } d
        ? $"{d.FullName}+{Name}"
        : string.IsNullOrEmpty(Namespace) ? Name : $"{Namespace}.{Name}";

    private static string StripArity(string n) => n.IndexOf('`') is var i and >= 0 ? n[..i] : n;
}

public sealed record NamespaceModel(string Name, IReadOnlyList<TypeModel> Types);

public sealed record AssemblyModel(
    int Index, string Name, string Version, string Culture, string PublicKeyToken,
    string ImageName, IReadOnlyList<string> References, IReadOnlyList<NamespaceModel> Namespaces, int TypeCount);

/// <summary>
/// Names-only object model built purely from metadata: assemblies, namespaces, types (with nesting),
/// methods, parameters, fields, properties, events and generic parameter names. Member types and method
/// addresses need the binary's registration structures and are attached in Phase 2.
/// </summary>
public sealed class MetadataModel
{
    public IReadOnlyList<AssemblyModel> Assemblies { get; }
    public IReadOnlyList<TypeModel> Types { get; }
    public MetadataStatistics Statistics { get; }

    private MetadataModel(IReadOnlyList<AssemblyModel> assemblies, IReadOnlyList<TypeModel> types, MetadataStatistics stats)
    {
        Assemblies = assemblies;
        Types = types;
        Statistics = stats;
    }

    public static MetadataModel Build(GlobalMetadata md, IDiagnosticSink sink)
    {
        var imageNameOfType = new string[md.TypeDefinitions.Length];
        foreach (var img in md.Images)
        {
            var name = md.GetString(img.NameIndex);
            for (var t = img.TypeStart; t < img.TypeStart + img.TypeCount && t < imageNameOfType.Length; t++)
                if (t >= 0) imageNameOfType[t] = name;
        }

        var types = new TypeModel[md.TypeDefinitions.Length];
        for (var i = 0; i < types.Length; i++)
        {
            var def = md.TypeDefinitions[i];
            var type = new TypeModel
            {
                Index = i,
                Name = md.GetString(def.NameIndex),
                Namespace = md.GetString(def.NamespaceIndex),
                Token = def.Token,
                Flags = def.Flags,
                Raw = def,
                GenericParameters = GenericParameterNames(md, def.GenericContainerIndex),
                ImageName = imageNameOfType[i] ?? "<no image>",
            };

            for (var m = 0; m < def.MethodCount; m++)
            {
                var mi = def.MethodStart + m;
                if ((uint)mi >= (uint)md.Methods.Length) break;
                type.Methods.Add(BuildMethod(md, mi));
            }
            for (var f = 0; f < def.FieldCount; f++)
            {
                var fi = def.FieldStart + f;
                if ((uint)fi >= (uint)md.Fields.Length) break;
                var fd = md.Fields[fi];
                type.Fields.Add(new FieldModel(fi, md.GetString(fd.NameIndex), fd.Token, fd.TypeIndex));
            }
            for (var p = 0; p < def.PropertyCount; p++)
            {
                var pi = def.PropertyStart + p;
                if ((uint)pi >= (uint)md.Properties.Length) break;
                var pd = md.Properties[pi];
                // get/set are indices relative to the declaring type's methodStart; -1 = absent.
                type.Properties.Add(new PropertyModel(pi, md.GetString(pd.NameIndex), pd.Token,
                    RelativeMethodName(md, def, pd.Get), RelativeMethodName(md, def, pd.Set)));
            }
            for (var e = 0; e < def.EventCount; e++)
            {
                var ei = def.EventStart + e;
                if ((uint)ei >= (uint)md.Events.Length) break;
                var ed = md.Events[ei];
                type.Events.Add(new EventModel(ei, md.GetString(ed.NameIndex), ed.Token, ed.TypeIndex));
            }
            types[i] = type;
        }

        // Nesting comes from the nestedTypes table (TypeDefinitionIndex values), which is metadata-only.
        foreach (var parent in types)
        {
            var def = parent.Raw;
            for (var n = 0; n < def.NestedTypeCount; n++)
            {
                var slot = def.NestedTypesStart + n;
                if ((uint)slot >= (uint)md.NestedTypes.Length) break;
                var child = md.NestedTypes[slot];
                if ((uint)child >= (uint)types.Length)
                {
                    sink.Warn(DiagnosticCategory.MetadataIntegrity, DiagnosticCodes.MetadataIndexOutOfRange,
                        $"Nested type index {child} of '{parent.FullName}' is out of range.", new() { Table = "nestedTypes", MetadataIndex = slot });
                    continue;
                }
                types[child].DeclaringType = parent;
                parent.NestedTypes.Add(types[child]);
            }
        }

        var assemblies = new List<AssemblyModel>(md.Assemblies.Length);
        for (var a = 0; a < md.Assemblies.Length; a++)
        {
            var asm = md.Assemblies[a];
            if ((uint)asm.ImageIndex >= (uint)md.Images.Length)
            {
                sink.Warn(DiagnosticCategory.MetadataIntegrity, DiagnosticCodes.MetadataIndexOutOfRange,
                    $"Assembly {a} points to missing image {asm.ImageIndex}.", new() { Table = "assemblies", MetadataIndex = a });
                continue;
            }
            var img = md.Images[asm.ImageIndex];
            var asmTypes = new List<TypeModel>();
            for (var t = img.TypeStart; t < img.TypeStart + img.TypeCount; t++)
                if ((uint)t < (uint)types.Length && types[t].DeclaringType is null)
                    asmTypes.Add(types[t]);

            var namespaces = asmTypes
                .GroupBy(t => t.Namespace)
                .OrderBy(g => g.Key, StringComparer.Ordinal)
                .Select(g => new NamespaceModel(g.Key, [.. g.OrderBy(t => t.Name, StringComparer.Ordinal)]))
                .ToList();

            var refs = new List<string>(asm.ReferencedAssemblyCount);
            for (var r = 0; r < asm.ReferencedAssemblyCount; r++)
            {
                var ri = asm.ReferencedAssemblyStart + r;
                if ((uint)ri < (uint)md.ReferencedAssemblies.Length && (uint)md.ReferencedAssemblies[ri] < (uint)md.Assemblies.Length)
                    refs.Add(md.GetString(md.Assemblies[md.ReferencedAssemblies[ri]].NameIndex));
            }

            assemblies.Add(new AssemblyModel(a, md.GetString(asm.NameIndex), asm.VersionString,
                md.GetString(asm.CultureIndex), asm.PublicKeyTokenHex, md.GetString(img.NameIndex),
                refs, namespaces, (int)img.TypeCount));
        }

        var stats = new MetadataStatistics(
            md.Version.ToString(), md.Version.UnityRange, md.FileSize, md.Images.Length, md.Assemblies.Length,
            md.TypeDefinitions.Length, md.Methods.Length, md.Fields.Length, md.Parameters.Length, md.Properties.Length,
            md.Events.Length, md.GenericContainers.Length, md.GenericParameters.Length, md.StringLiterals.Length,
            md.VTableMethods.Length, md.Interfaces.Length, md.NestedTypes.Length, md.FieldRefs.Length,
            md.Version.HasAttributeDataBlobs ? md.AttributeDataRanges.Length : md.AttributeTypeRanges.Length);

        sink.Info(DiagnosticCategory.Metadata, DiagnosticCodes.MetadataModelBuilt,
            $"Model: {assemblies.Count} assemblies, {types.Length:N0} types, {md.Methods.Length:N0} methods, {md.Fields.Length:N0} fields, {md.StringLiterals.Length:N0} string literals.",
            new() { File = md.Path });

        return new MetadataModel(assemblies, types, stats);
    }

    private static MethodModel BuildMethod(GlobalMetadata md, int index)
    {
        var def = md.Methods[index];
        var names = new List<string>(def.ParameterCount);
        var typeIdx = new List<int>(def.ParameterCount);
        for (var p = 0; p < def.ParameterCount; p++)
        {
            var pi = def.ParameterStart + p;
            if ((uint)pi >= (uint)md.Parameters.Length) break;
            names.Add(md.GetString(md.Parameters[pi].NameIndex));
            typeIdx.Add(md.Parameters[pi].TypeIndex);
        }
        return new MethodModel
        {
            Index = index,
            Name = md.GetString(def.NameIndex),
            Token = def.Token,
            Flags = def.Flags,
            ImplFlags = def.IFlags,
            Slot = def.Slot,
            ParameterNames = names,
            ParameterTypeIndices = typeIdx,
            ReturnTypeIndex = def.ReturnType,
            GenericParameters = GenericParameterNames(md, def.GenericContainerIndex),
        };
    }

    private static string? RelativeMethodName(GlobalMetadata md, Il2CppTypeDefinition def, int relative)
    {
        if (relative < 0) return null;
        var abs = def.MethodStart + relative;
        return (uint)abs < (uint)md.Methods.Length ? md.GetString(md.Methods[abs].NameIndex) : null;
    }

    private static IReadOnlyList<string> GenericParameterNames(GlobalMetadata md, int containerIndex)
    {
        if ((uint)containerIndex >= (uint)md.GenericContainers.Length) return [];
        var c = md.GenericContainers[containerIndex];
        var list = new List<string>(c.TypeArgc);
        for (var i = 0; i < c.TypeArgc; i++)
        {
            var gi = c.GenericParameterStart + i;
            if ((uint)gi >= (uint)md.GenericParameters.Length) break;
            list.Add(md.GetString(md.GenericParameters[gi].NameIndex));
        }
        return list;
    }
}
