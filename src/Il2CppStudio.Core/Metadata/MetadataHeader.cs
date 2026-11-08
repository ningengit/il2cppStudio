using Il2CppStudio.Core.IO;

namespace Il2CppStudio.Core.Metadata;

/// <summary>One (offset, size) pair from the metadata header. Size is in bytes.</summary>
public sealed record MetadataSection(string Name, int Offset, int Size, string Description)
{
    public long End => (long)Offset + Size;
}

/// <summary>
/// Il2CppGlobalMetadataHeader. For versions 27 to 31 the header is 2 x int32 followed by 31 (offset, size)
/// pairs = 256 bytes. The only layout difference is the meaning of two pairs: v27 stores
/// attributesInfo/attributeTypes (type ranges), v29+ stores attributeData/attributeDataRange (blobs).
/// Order verified against libil2cpp/vm/GlobalMetadataFileInternals.h (Unity 2022.3.62f3).
/// </summary>
public sealed class MetadataHeader
{
    public const uint ExpectedSanity = 0xFAB11BAF;

    private static readonly (string Name, string Description)[] CommonPrefix =
    [
        ("stringLiteral", "Il2CppStringLiteral"),
        ("stringLiteralData", "UTF-8 string literal bytes"),
        ("string", "NUL-terminated metadata identifier strings"),
        ("events", "Il2CppEventDefinition"),
        ("properties", "Il2CppPropertyDefinition"),
        ("methods", "Il2CppMethodDefinition"),
        ("parameterDefaultValues", "Il2CppParameterDefaultValue"),
        ("fieldDefaultValues", "Il2CppFieldDefaultValue"),
        ("fieldAndParameterDefaultValueData", "Default value blob (uint8)"),
        ("fieldMarshaledSizes", "Il2CppFieldMarshaledSize"),
        ("parameters", "Il2CppParameterDefinition"),
        ("fields", "Il2CppFieldDefinition"),
        ("genericParameters", "Il2CppGenericParameter"),
        ("genericParameterConstraints", "TypeIndex"),
        ("genericContainers", "Il2CppGenericContainer"),
        ("nestedTypes", "TypeDefinitionIndex"),
        ("interfaces", "TypeIndex"),
        ("vtableMethods", "EncodedMethodIndex"),
        ("interfaceOffsets", "Il2CppInterfaceOffsetPair"),
        ("typeDefinitions", "Il2CppTypeDefinition"),
        ("images", "Il2CppImageDefinition"),
        ("assemblies", "Il2CppAssemblyDefinition"),
        ("fieldRefs", "Il2CppFieldRef"),
        ("referencedAssemblies", "int32 assembly index"),
    ];

    private static readonly (string Name, string Description)[] V27Attributes =
    [
        ("attributesInfo", "Il2CppCustomAttributeTypeRange"),
        ("attributeTypes", "TypeIndex"),
    ];

    private static readonly (string Name, string Description)[] V29Attributes =
    [
        ("attributeData", "Encoded custom attribute blob"),
        ("attributeDataRange", "Il2CppCustomAttributeDataRange"),
    ];

    private static readonly (string Name, string Description)[] CommonSuffix =
    [
        ("unresolvedIndirectCallParameterTypes", "TypeIndex"),
        ("unresolvedIndirectCallParameterRanges", "Il2CppMetadataRange"),
        ("windowsRuntimeTypeNames", "Il2CppWindowsRuntimeTypeNamePair"),
        ("windowsRuntimeStrings", "const char*"),
        ("exportedTypeDefinitions", "TypeDefinitionIndex"),
    ];

    public uint Sanity { get; }
    public int RawVersion { get; }
    public MetadataVersion Version { get; }
    public int HeaderSize { get; }
    public IReadOnlyList<MetadataSection> Sections { get; }
    private readonly Dictionary<string, MetadataSection> _byName;

    private MetadataHeader(uint sanity, int rawVersion, MetadataVersion version, int headerSize, List<MetadataSection> sections)
    {
        Sanity = sanity;
        RawVersion = rawVersion;
        Version = version;
        HeaderSize = headerSize;
        Sections = sections;
        _byName = sections.ToDictionary(s => s.Name, StringComparer.Ordinal);
    }

    public MetadataSection this[string name] =>
        _byName.TryGetValue(name, out var s) ? s : throw new KeyNotFoundException($"Metadata section '{name}' does not exist in v{Version}.");

    public bool TryGet(string name, out MetadataSection section) => _byName.TryGetValue(name, out section!);

    public static int SectionCount => CommonPrefix.Length + 2 + CommonSuffix.Length;

    public static int SizeFor(MetadataVersion v) => 8 + SectionNames(v).Count * 8;

    /// <summary>
    /// Section order for a version. v27+: CommonPrefix, attribute pair, CommonSuffix. v24 inserts rgctxEntries (24.0-24.1)
    /// before images and metadataUsageLists/Pairs before fieldRefs, and has no windowsRuntimeStrings.
    /// </summary>
    public static List<(string Name, string Description)> SectionNames(MetadataVersion v)
    {
        var names = new List<(string, string)>(36);
        foreach (var entry in CommonPrefix)
        {
            if (v.IsV24 && entry.Name == "images" && v.HasCodegenIndices) names.Add(("rgctxEntries", "Il2CppRGCTXDefinition"));
            if (v.IsV24 && entry.Name == "fieldRefs")
            {
                names.Add(("metadataUsageLists", "Il2CppMetadataUsageList"));
                names.Add(("metadataUsagePairs", "Il2CppMetadataUsagePair"));
            }
            names.Add(entry);
        }
        names.AddRange(v.HasAttributeDataBlobs ? V29Attributes : V27Attributes);
        names.AddRange(CommonSuffix.Where(s => v.HasWindowsRuntimeStrings || s.Name != "windowsRuntimeStrings"));
        return names;
    }

    /// <summary>Reads the header. Throws <see cref="InvalidDataException"/> on bad magic or truncation.</summary>
    public static MetadataHeader Read(ReadOnlySpan<byte> data, MetadataVersion version)
    {
        var r = new SpanReader(data);
        var sanity = r.ReadUInt32();
        var raw = r.ReadInt32();

        var names = SectionNames(version);

        var sections = new List<MetadataSection>(names.Count);
        foreach (var (name, desc) in names)
            sections.Add(new MetadataSection(name, r.ReadInt32(), r.ReadInt32(), desc));

        return new MetadataHeader(sanity, raw, version, r.Position, sections);
    }
}
