using System.Collections.Concurrent;
using System.Text;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.IO;

namespace Il2CppStudio.Core.Metadata;

public sealed class MetadataLoadException(string message, Exception? inner = null) : Exception(message, inner);

/// <summary>Validation result for one header section.</summary>
public sealed record MetadataSectionReport(
    string Name, int Offset, int Size, int ElementSize, int ElementCount, bool InBounds, bool Aligned, string Description);

/// <summary>
/// Parser for global-metadata.dat, versions 16 to 31. Tables are decoded eagerly into arrays of small
/// structs; string tables stay in the memory-mapped file and are decoded on demand with a cache.
/// </summary>
public sealed class GlobalMetadata : IDisposable
{
    private readonly struct BackingStore : IDisposable
    {
        private readonly IDisposable? _owner;
        public string Path { get; }
        public int Length { get; }
        public ReadOnlyMemory<byte> Memory { get; }
        public ReadOnlySpan<byte> Span => Memory.Span;

        public BackingStore(MappedFile mapped)
        {
            Path = mapped.Path;
            Length = mapped.Length;
            Memory = mapped.Memory;
            _owner = mapped;
        }

        public BackingStore(VirtualFileBuffer buffer)
        {
            Path = buffer.VirtualPath;
            Length = buffer.Length;
            Memory = buffer.Memory;
            _owner = null; // Owned by the enclosing VirtualArchiveSession.
        }

        public void Dispose() => _owner?.Dispose();
    }

    private readonly BackingStore _file;
    private readonly IDiagnosticSink _sink;
    private readonly ConcurrentDictionary<int, string> _stringCache = new();
    private readonly List<MetadataSectionReport> _sectionReports = [];

    public string Path => _file.Path;
    public int FileSize => _file.Length;
    public MetadataHeader Header { get; }
    public MetadataVersion Version => Header.Version;
    public IReadOnlyList<MetadataSectionReport> SectionReports => _sectionReports;

    public Il2CppStringLiteral[] StringLiterals { get; }
    public Il2CppEventDefinition[] Events { get; }
    public Il2CppPropertyDefinition[] Properties { get; }
    public Il2CppMethodDefinition[] Methods { get; }
    public Il2CppDefaultValue[] ParameterDefaultValues { get; }
    public Il2CppDefaultValue[] FieldDefaultValues { get; }
    public Il2CppFieldMarshaledSize[] FieldMarshaledSizes { get; }
    public Il2CppParameterDefinition[] Parameters { get; }
    public Il2CppFieldDefinition[] Fields { get; }
    public Il2CppGenericParameter[] GenericParameters { get; }
    public int[] GenericParameterConstraints { get; }
    public Il2CppGenericContainer[] GenericContainers { get; }
    public int[] NestedTypes { get; }
    public int[] Interfaces { get; }
    public uint[] VTableMethods { get; }
    public Il2CppInterfaceOffsetPair[] InterfaceOffsets { get; }
    public Il2CppTypeDefinition[] TypeDefinitions { get; }
    public Il2CppImageDefinition[] Images { get; }
    public Il2CppAssemblyDefinition[] Assemblies { get; }
    public Il2CppFieldRef[] FieldRefs { get; }
    public int[] ReferencedAssemblies { get; }
    public Il2CppCustomAttributeTypeRange[] AttributeTypeRanges { get; } = [];   // v27
    public int[] AttributeTypes { get; } = [];                                   // v27
    public Il2CppCustomAttributeDataRange[] AttributeDataRanges { get; } = [];   // v29+
    public int[] UnresolvedIndirectCallParameterTypes { get; }
    public Il2CppMetadataRange[] UnresolvedIndirectCallParameterRanges { get; }
    public Il2CppWindowsRuntimeTypeNamePair[] WindowsRuntimeTypeNames { get; }
    public int[] ExportedTypeDefinitions { get; }

    /// <summary>v24 only: per-method runtime-metadata usage ranges and (destination global, encoded source) pairs.</summary>
    public Il2CppMetadataUsageList[] MetadataUsageLists { get; } = [];
    public Il2CppMetadataUsagePair[] MetadataUsagePairs { get; } = [];

    /// <summary>v108+: methodSpecs and genericMethodTable live directly inside global-metadata.dat.</summary>
    public Registration.Il2CppMethodSpecRecord[] EmbeddedMethodSpecs { get; } = [];
    public Registration.Il2CppGenericMethodFunction[] EmbeddedGenericMethodTable { get; } = [];
    public SerializedIndexSizes IndexSizes { get; }

    private GlobalMetadata(BackingStore file, IDiagnosticSink sink)
    {
        _file = file;
        _sink = sink;
        var data = file.Span;

        if (data.Length < 8)
            throw Fail(DiagnosticCodes.MetadataBadMagic, $"File is only {data.Length} bytes; not a metadata file.");

        var sanity = BitConverter.ToUInt32(data);
        var rawVersion = BitConverter.ToInt32(data[4..]);
        if (sanity != MetadataHeader.ExpectedSanity)
        {
            _sink.Error(DiagnosticCategory.Metadata, DiagnosticCodes.MetadataPossiblyEncrypted,
                $"Magic is 0x{sanity:X8}, expected 0x{MetadataHeader.ExpectedSanity:X8}. The file is either not IL2CPP metadata, " +
                "or it has been encrypted/obfuscated by the game developer. Encrypted metadata is out of scope.", Loc());
            throw new MetadataLoadException("Invalid metadata magic.");
        }

        var version = ResolveVersion(rawVersion);
        if (data.Length < MetadataHeader.SizeFor(version))
            throw Fail(DiagnosticCodes.MetadataSectionOutOfBounds, "File is shorter than the metadata header.");

        Header = MetadataHeader.Read(data, version);
        _sink.Info(DiagnosticCategory.Metadata, DiagnosticCodes.MetadataVersionResolved,
            $"Metadata v{version} ({version.UnityRange}), header {Header.HeaderSize} bytes, file {data.Length:N0} bytes.", Loc());

        ValidateHeaderLayout(data.Length);
        CrossCheckMethodLayout();
        IndexSizes = version.HasCompactIndices ? DeriveIndexSizes(version) : SerializedIndexSizes.Fixed4;
        var sz = IndexSizes;

        StringLiterals = version.HasV39StringLiterals ? ReadV39StringLiterals() : ReadTable<Il2CppStringLiteral>("stringLiteral");
        if (version.HasCompactIndices)
        {
            Events = ReadCompact("events", (ref r) => Il2CppEventDefinition.ReadCompact(ref r, version, sz));
            Properties = ReadCompact("properties", (ref r) => Il2CppPropertyDefinition.ReadCompact(ref r, version, sz));
            Methods = ReadCompact("methods", (ref r) => Il2CppMethodDefinition.ReadCompact(ref r, sz));
            ParameterDefaultValues = ReadCompact("parameterDefaultValues", (ref r) => Il2CppDefaultValue.ReadParamCompact(ref r, version, sz));
            FieldDefaultValues = ReadCompact("fieldDefaultValues", (ref r) => Il2CppDefaultValue.ReadFieldCompact(ref r, version, sz));
            FieldMarshaledSizes = ReadCompact("fieldMarshaledSizes", (ref r) => Il2CppFieldMarshaledSize.ReadCompact(ref r, version, sz));
            Parameters = ReadCompact("parameters", (ref r) => Il2CppParameterDefinition.ReadCompact(ref r, sz));
            Fields = ReadCompact("fields", (ref r) => Il2CppFieldDefinition.ReadCompact(ref r, sz));
            GenericParameters = ReadCompact("genericParameters", (ref r) => Il2CppGenericParameter.ReadCompact(ref r, sz));
            GenericParameterConstraints = ReadCompactInts("genericParameterConstraints", sz.TypeIndex);
            GenericContainers = ReadCompact("genericContainers", (ref r) => Il2CppGenericContainer.ReadCompact(ref r, version, sz));
            NestedTypes = ReadInts("nestedTypes");
            Interfaces = ReadCompactInts("interfaces", sz.TypeIndex);
            VTableMethods = [.. ReadInts("vtableMethods").Select(i => unchecked((uint)i))];
            InterfaceOffsets = ReadCompact("interfaceOffsets", (ref r) => Il2CppInterfaceOffsetPair.ReadCompact(ref r, sz));
            TypeDefinitions = ReadCompact("typeDefinitions", (ref r) => Il2CppTypeDefinition.ReadCompact(ref r, version, sz));
            if (Header.TryGet("typeInlineArrays", out _)) ReportCounted("typeInlineArrays");
            Images = ReadCompact("images", (ref r) => Il2CppImageDefinition.ReadCompact(ref r, version, sz));
            Assemblies = ReadTable<Il2CppAssemblyDefinition>("assemblies");
            FieldRefs = ReadCompact("fieldRefs", (ref r) => Il2CppFieldRef.ReadCompact(ref r, version, sz));
            ReferencedAssemblies = ReadInts("referencedAssemblies");
            AttributeDataRanges = ReadTable<Il2CppCustomAttributeDataRange>("attributeDataRange");
            ReportBlob("attributeData");
            UnresolvedIndirectCallParameterTypes = ReadCompactInts("unresolvedIndirectCallParameterTypes", sz.TypeIndex);
            UnresolvedIndirectCallParameterRanges = ReadTable<Il2CppMetadataRange>("unresolvedIndirectCallParameterRanges");
            WindowsRuntimeTypeNames = ReadCompact("windowsRuntimeTypeNames", (ref r) => Il2CppWindowsRuntimeTypeNamePair.ReadCompact(ref r, sz));
            ExportedTypeDefinitions = ReadInts("exportedTypeDefinitions");
            if (version.HasV108EmbeddedGenerics)
            {
                var onGeneric = ReadCompact("methodSpecsOnGenericType", (ref r) =>
                    new Registration.Il2CppMethodSpecRecord(
                        SerializedIndexSizes.ReadIndex(ref r, sz.MethodIndex),
                        SerializedIndexSizes.ReadIndex(ref r, sz.GenericInstIndex),
                        -1));
                var genericOnType = ReadCompact("genericMethodSpecsOnType", (ref r) =>
                    new Registration.Il2CppMethodSpecRecord(
                        SerializedIndexSizes.ReadIndex(ref r, sz.MethodIndex),
                        -1,
                        SerializedIndexSizes.ReadIndex(ref r, sz.GenericInstIndex)));
                var fullSpecs = ReadCompact("methodSpecs", (ref r) =>
                    new Registration.Il2CppMethodSpecRecord(
                        SerializedIndexSizes.ReadIndex(ref r, sz.MethodIndex),
                        SerializedIndexSizes.ReadIndex(ref r, sz.GenericInstIndex),
                        SerializedIndexSizes.ReadIndex(ref r, sz.GenericInstIndex)));
                EmbeddedMethodSpecs = [.. onGeneric, .. genericOnType, .. fullSpecs];

                var noAdj = ReadCompact("genericMethodFunctionsDefinitions", (ref r) =>
                    new Registration.Il2CppGenericMethodFunction(
                        SerializedIndexSizes.ReadIndex(ref r, sz.GenericMethodIndex),
                        SerializedIndexSizes.ReadIndex(ref r, sz.MethodPointerTableIndex),
                        SerializedIndexSizes.ReadIndex(ref r, sz.InvokerTableIndex),
                        -1));
                var withAdj = ReadCompact("genericMethodFunctionsDefinitionsWithAdjustor", (ref r) =>
                    new Registration.Il2CppGenericMethodFunction(
                        SerializedIndexSizes.ReadIndex(ref r, sz.GenericMethodIndex),
                        SerializedIndexSizes.ReadIndex(ref r, sz.MethodPointerTableIndex),
                        SerializedIndexSizes.ReadIndex(ref r, sz.InvokerTableIndex),
                        SerializedIndexSizes.ReadIndex(ref r, sz.AdjustorThunkIndex)));
                EmbeddedGenericMethodTable = [.. noAdj, .. withAdj];

                ReportCounted("invokerIndices");
                ReportCounted("rgctxRanges");
                ReportCounted("rgctxValues");
                ReportCounted("staticConstructorTypeIndices");
            }
        }
        else
        {
            Events = ReadTable<Il2CppEventDefinition>("events");
            Properties = ReadTable<Il2CppPropertyDefinition>("properties");
            Methods = ReadTable<Il2CppMethodDefinition>("methods");
            ParameterDefaultValues = ReadTable<Il2CppDefaultValue>("parameterDefaultValues");
            FieldDefaultValues = ReadTable<Il2CppDefaultValue>("fieldDefaultValues");
            FieldMarshaledSizes = ReadTable<Il2CppFieldMarshaledSize>("fieldMarshaledSizes");
            Parameters = ReadTable<Il2CppParameterDefinition>("parameters");
            Fields = ReadTable<Il2CppFieldDefinition>("fields");
            GenericParameters = ReadTable<Il2CppGenericParameter>("genericParameters");
            GenericParameterConstraints = ReadInts("genericParameterConstraints");
            GenericContainers = ReadTable<Il2CppGenericContainer>("genericContainers");
            NestedTypes = ReadInts("nestedTypes");
            Interfaces = ReadInts("interfaces");
            VTableMethods = [.. ReadInts("vtableMethods").Select(i => unchecked((uint)i))];
            InterfaceOffsets = ReadTable<Il2CppInterfaceOffsetPair>("interfaceOffsets");
            TypeDefinitions = ReadTable<Il2CppTypeDefinition>("typeDefinitions");
            Images = ReadTable<Il2CppImageDefinition>("images");
            // 24.1 builds from late 2018.4 already dropped hashValueIndex: pick the assembly layout that divides evenly.
            Assemblies = version.IsV24 && Images.Length > 0 && Header["assemblies"].Size / Images.Length == 64 && version.HasHashValueIndex
                ? ReadTable<Il2CppAssemblyDefinition>("assemblies", new MetadataVersion(24, 4))
                : ReadTable<Il2CppAssemblyDefinition>("assemblies");
            FieldRefs = version.HasFieldRefs ? ReadTable<Il2CppFieldRef>("fieldRefs") : [];
            ReferencedAssemblies = version.HasReferencedAssemblies ? ReadInts("referencedAssemblies") : [];
            if (Header.TryGet("rgctxEntries", out _))
                ReportBlob("rgctxEntries");
            if (version.HasMetadataUsageTables)
            {
                MetadataUsageLists = ReadTable<Il2CppMetadataUsageList>("metadataUsageLists");
                MetadataUsagePairs = ReadTable<Il2CppMetadataUsagePair>("metadataUsagePairs");
            }
            if (version.HasAttributeDataBlobs)
            {
                AttributeDataRanges = ReadTable<Il2CppCustomAttributeDataRange>("attributeDataRange");
                ReportBlob("attributeData");
            }
            else if (version.HasCustomAttributeTables)
            {
                AttributeTypeRanges = ReadTable<Il2CppCustomAttributeTypeRange>("attributesInfo");
                AttributeTypes = ReadInts("attributeTypes");
            }
            UnresolvedIndirectCallParameterTypes = version.HasUnresolvedIndirectCalls ? ReadInts("unresolvedIndirectCallParameterTypes") : [];
            UnresolvedIndirectCallParameterRanges = version.HasUnresolvedIndirectCalls ? ReadTable<Il2CppMetadataRange>("unresolvedIndirectCallParameterRanges") : [];
            WindowsRuntimeTypeNames = version.HasWindowsRuntimeTypeNames ? ReadTable<Il2CppWindowsRuntimeTypeNamePair>("windowsRuntimeTypeNames") : [];
            ExportedTypeDefinitions = version.HasExportedTypeDefinitions ? ReadInts("exportedTypeDefinitions") : [];
        }
        ReportBlob("stringLiteralData");
        ReportBlob("string");
        ReportBlob("fieldAndParameterDefaultValueData");
        if (version.HasWindowsRuntimeStrings) ReportBlob("windowsRuntimeStrings");

        ValidateCrossReferences();
    }

    /// <summary>Opens and fully parses a metadata file. Throws <see cref="MetadataLoadException"/> on fatal errors.</summary>
    public static GlobalMetadata Load(string path, IDiagnosticSink sink)
    {
        MappedFile file;
        try { file = MappedFile.Open(path); }
        catch (Exception ex)
        {
            sink.Error(DiagnosticCategory.FileSystem, DiagnosticCodes.FileAccessFailed, $"Cannot open metadata: {ex.Message}", new() { File = path }, ex);
            throw new MetadataLoadException($"Cannot open '{path}'.", ex);
        }

        try { return new GlobalMetadata(new BackingStore(file), sink); }
        catch (MetadataLoadException) { file.Dispose(); throw; }
        catch (Exception ex)
        {
            file.Dispose();
            sink.Fatal(DiagnosticCategory.Metadata, DiagnosticCodes.MetadataSectionOutOfBounds,
                $"Unexpected failure while parsing metadata: {ex.Message}", new() { File = path }, ex);
            throw new MetadataLoadException("Metadata parse failed.", ex);
        }
    }

    /// <summary>Parses a <see cref="VirtualFileBuffer"/> (zero-copy mapped slice or in-memory stream) without disk extraction.</summary>
    public static GlobalMetadata Load(VirtualFileBuffer buffer, IDiagnosticSink sink)
    {
        try { return new GlobalMetadata(new BackingStore(buffer), sink); }
        catch (MetadataLoadException) { throw; }
        catch (Exception ex)
        {
            sink.Fatal(DiagnosticCategory.Metadata, DiagnosticCodes.MetadataSectionOutOfBounds,
                $"Unexpected failure while parsing metadata: {ex.Message}", new() { File = buffer.VirtualPath }, ex);
            throw new MetadataLoadException("Metadata parse failed.", ex);
        }
    }

    // ------------------------------------------------------------------ strings

    /// <summary>Metadata identifier string (type/method/field names). Never throws; returns a marker on bad indices.</summary>
    public string GetString(int index)
    {
        if (_stringCache.TryGetValue(index, out var cached)) return cached;
        var section = Header["string"];
        if (index < 0 || index >= section.Size)
        {
            _sink.Warn(DiagnosticCategory.MetadataIntegrity, DiagnosticCodes.MetadataStringOutOfRange,
                $"String index {index} outside string table (size {section.Size}).", Loc("string", index));
            return $"<bad-string-{index}>";
        }
        var region = _file.Span.Slice(section.Offset + index, section.Size - index);
        var end = region.IndexOf((byte)0);
        var s = Encoding.UTF8.GetString(end < 0 ? region : region[..end]);
        _stringCache.TryAdd(index, s);
        return s;
    }

    /// <summary>A C# string literal used by game code (ldstr).</summary>
    public string GetStringLiteral(int index)
    {
        if ((uint)index >= (uint)StringLiterals.Length) return $"<bad-literal-{index}>";
        var lit = StringLiterals[index];
        var data = Header["stringLiteralData"];
        if (lit.DataIndex < 0 || (long)lit.DataIndex + lit.Length > data.Size)
        {
            _sink.Warn(DiagnosticCategory.MetadataIntegrity, DiagnosticCodes.MetadataStringOutOfRange,
                $"String literal {index} (data {lit.DataIndex}, length {lit.Length}) exceeds literal data.", Loc("stringLiteral", index));
            return $"<bad-literal-{index}>";
        }
        return Encoding.UTF8.GetString(_file.Span.Slice(data.Offset + lit.DataIndex, (int)lit.Length));
    }

    /// <summary>Raw bytes of a header section (for extraction / hex view).</summary>
    public ReadOnlyMemory<byte> GetSectionBytes(string name)
    {
        var s = Header[name];
        return s.Offset >= 0 && s.End <= _file.Length ? _file.Memory.Slice(s.Offset, s.Size) : ReadOnlyMemory<byte>.Empty;
    }

    public ReadOnlySpan<byte> RawFile => _file.Span;

    // ------------------------------------------------------------------ parsing helpers

    private MetadataVersion ResolveVersion(int raw)
    {
        switch (raw)
        {
            case 16 or 19 or 20 or 21 or 22 or 23 or 27 or 29 or 31 or 39 or 108:
                return new MetadataVersion(raw);
            case 24:
                return ResolveV24Minor();
            case 17 or 18 or 25 or 26 or 28 or 30:
                throw Fail(DiagnosticCodes.MetadataVersionUnsupported,
                    $"Metadata v{raw} was never shipped by Unity; the header has likely been altered.");
            case < 16:
                throw Fail(DiagnosticCodes.MetadataVersionUnsupported,
                    $"Metadata v{raw} (pre Unity 5.3) is outside the supported range 16-108.");
            default:
                throw Fail(DiagnosticCodes.MetadataVersionUnsupported,
                    $"Metadata v{raw} is newer than the supported range 16-108. Add a layout for it before parsing.");
        }
    }

    /// <summary>
    /// v24 minor revision from structure alone (the file only stores 24): a 264-byte header (no rgctxEntries pair)
    /// is 24.2+, and 24.4 when assemblies are 64-byte records. Otherwise 24.1 if any image token read with the
    /// 24.0 (32-byte) image layout is not 1, else 24.0.
    /// </summary>
    private MetadataVersion ResolveV24Minor()
    {
        var data = _file.Span;
        var firstTable = BitConverter.ToInt32(data[8..]);
        if (firstTable == MetadataHeader.SizeFor(new MetadataVersion(24, 2)))
        {
            var h = MetadataHeader.Read(data, new MetadataVersion(24, 2));
            var images = h["images"].Size / 40;
            return new MetadataVersion(24, images > 0 && h["assemblies"].Size / 68 < images ? 4 : 2);
        }
        var h1 = MetadataHeader.Read(data, new MetadataVersion(24, 1));
        var img = h1["images"];
        for (var off = img.Offset; off + 32 <= img.End && off + 32 <= data.Length; off += 32)
            if (BitConverter.ToUInt32(data[(off + 28)..]) != 1) return new MetadataVersion(24, 1);
        return new MetadataVersion(24, 0);
    }

    private void ValidateHeaderLayout(int fileLength)
    {
        var first = Header.Sections.Where(s => s.Size > 0).MinBy(s => s.Offset);
        if (first is not null && first.Offset != Header.HeaderSize)
            _sink.Warn(DiagnosticCategory.MetadataIntegrity, DiagnosticCodes.MetadataHeaderSizeMismatch,
                $"First table starts at 0x{first.Offset:X} but the v{Version} header is 0x{Header.HeaderSize:X} bytes. " +
                "The header layout may not match this file.", Loc());

        var ordered = Header.Sections.Where(s => s.Size > 0).OrderBy(s => s.Offset).ToList();
        for (var i = 1; i < ordered.Count; i++)
        {
            if (ordered[i].Offset < ordered[i - 1].End)
                _sink.Warn(DiagnosticCategory.MetadataIntegrity, DiagnosticCodes.MetadataSectionsOverlap,
                    $"Section '{ordered[i].Name}' (0x{ordered[i].Offset:X}) overlaps '{ordered[i - 1].Name}' (ends 0x{ordered[i - 1].End:X}).", Loc());
        }

        foreach (var s in Header.Sections)
        {
            if (s.Offset < 0 || s.Size < 0 || s.End > fileLength)
                _sink.Error(DiagnosticCategory.MetadataIntegrity, DiagnosticCodes.MetadataSectionOutOfBounds,
                    $"Section '{s.Name}' [0x{s.Offset:X}, +{s.Size}] is outside the file (0x{fileLength:X} bytes).", Loc(s.Name));
        }
    }

    /// <summary>
    /// Independent check of the chosen layout: the methods table must divide evenly by the record size.
    /// If the other v27-31 size fits and the chosen one does not, the version field is lying.
    /// </summary>
    private void CrossCheckMethodLayout()
    {
        if (Version.Major < 27 || Version.HasCompactIndices) return;
        var size = Header["methods"].Size;
        var chosen = Il2CppMethodDefinition.SizeOf(Version);
        var alt = chosen == 36 ? 32 : 36;
        if (size % chosen != 0 && size % alt == 0)
            _sink.Error(DiagnosticCategory.MetadataIntegrity, DiagnosticCodes.MetadataLayoutAmbiguous,
                $"Methods table ({size} bytes) fits {alt}-byte records but not the {chosen}-byte v{Version} layout. " +
                "The version number in the header may have been modified.", Loc("methods"));
    }

    private delegate T CompactRowReader<out T>(ref SpanReader reader);

    private SerializedIndexSizes DeriveIndexSizes(MetadataVersion version)
    {
        int Stride(string name, int fallback) =>
            Header.TryGet(name, out var s) && s.Count > 0 ? s.Size / s.Count : fallback;

        var typeIdx = Header["interfaces"].Count > 0 ? Stride("interfaces", 4)
            : Header["fields"].Count > 0 ? Stride("fields", 12) - 8
            : Header["parameters"].Count > 0 ? Stride("parameters", 12) - 8
            : Header["genericParameterConstraints"].Count > 0 ? Stride("genericParameterConstraints", 4)
            : 4;
        var tdIdx = SerializedIndexSizes.GetIndexSize(Header["typeDefinitions"].Count);
        var gcIdx = SerializedIndexSizes.GetIndexSize(Header["genericContainers"].Count);
        var paramIdx = SerializedIndexSizes.GetIndexSize(Header["parameters"].Count);

        if (!version.HasV108EmbeddedGenerics)
        {
            return new SerializedIndexSizes
            {
                TypeIndex = typeIdx, TypeDefinitionIndex = tdIdx, GenericContainerIndex = gcIdx, ParameterIndex = paramIdx,
                GenericParameterIndex = 4, InterfacesIndex = 4, EventIndex = 4, PropertyIndex = 4,
                NestedTypeIndex = 4, FieldIndex = 4, DefaultValueDataIndex = 4, MethodIndex = 4,
                GenericInstIndex = 4, GenericMethodIndex = 4, MethodPointerTableIndex = 4,
                InvokerTableIndex = 4, AdjustorThunkIndex = 4,
            };
        }

        var methodIdx = SerializedIndexSizes.GetIndexSize(Header["methods"].Count);
        var gInstIdx = Header["methodSpecsOnGenericType"].Count > 0 ? Stride("methodSpecsOnGenericType", methodIdx + 4) - methodIdx
            : Header["genericMethodSpecsOnType"].Count > 0 ? Stride("genericMethodSpecsOnType", methodIdx + 4) - methodIdx
            : Header["methodSpecs"].Count > 0 ? (Stride("methodSpecs", methodIdx + 8) - methodIdx) / 2
            : 4;
        var gmCount = Header["methodSpecsOnGenericType"].Count + Header["genericMethodSpecsOnType"].Count + Header["methodSpecs"].Count;
        var gmIdx = SerializedIndexSizes.GetIndexSize(gmCount);
        var invIdx = Header["invokerIndices"].Count > 0 ? Stride("invokerIndices", 4) : 4;
        var adjIdx = SerializedIndexSizes.GetIndexSize(Header["genericMethodFunctionsDefinitionsWithAdjustor"].Count);
        var mpIdx = Header["genericMethodFunctionsDefinitions"].Count > 0
            ? Stride("genericMethodFunctionsDefinitions", gmIdx + invIdx + 4) - gmIdx - invIdx
            : Header["genericMethodFunctionsDefinitionsWithAdjustor"].Count > 0
                ? Stride("genericMethodFunctionsDefinitionsWithAdjustor", gmIdx + invIdx + adjIdx + 4) - gmIdx - invIdx - adjIdx
                : 4;

        return new SerializedIndexSizes
        {
            TypeIndex = typeIdx,
            TypeDefinitionIndex = tdIdx,
            GenericContainerIndex = gcIdx,
            ParameterIndex = paramIdx,
            GenericParameterIndex = SerializedIndexSizes.GetIndexSize(Header["genericParameters"].Count),
            InterfacesIndex = SerializedIndexSizes.GetIndexSize(Header["interfaceOffsets"].Count),
            EventIndex = SerializedIndexSizes.GetIndexSize(Header["events"].Count),
            PropertyIndex = SerializedIndexSizes.GetIndexSize(Header["properties"].Count),
            NestedTypeIndex = SerializedIndexSizes.GetIndexSize(Header["nestedTypes"].Count),
            FieldIndex = SerializedIndexSizes.GetIndexSize(Header["fields"].Count),
            DefaultValueDataIndex = SerializedIndexSizes.GetIndexSize(Header["fieldAndParameterDefaultValueData"].Count),
            MethodIndex = methodIdx,
            GenericInstIndex = gInstIdx,
            GenericMethodIndex = gmIdx,
            MethodPointerTableIndex = mpIdx,
            InvokerTableIndex = invIdx,
            AdjustorThunkIndex = adjIdx,
        };
    }

    private Il2CppStringLiteral[] ReadV39StringLiterals()
    {
        var s = Header["stringLiteral"];
        var count = Math.Max(0, s.Count);
        var inBounds = s.Offset >= 0 && s.Size >= 0 && s.End <= _file.Length;
        var aligned = count == 0 ? s.Size == 0 : s.Size == count * 4;
        _sectionReports.Add(new MetadataSectionReport(s.Name, s.Offset, s.Size, 4, count, inBounds, aligned, s.Description));
        if (!inBounds || count <= 1) return [];

        var r = new SpanReader(_file.Span.Slice(s.Offset, Math.Min(s.Size, count * 4)));
        var literalCount = count - 1;
        var result = new Il2CppStringLiteral[literalCount];
        var prev = r.ReadInt32();
        for (var i = 0; i < literalCount; i++)
        {
            var next = r.ReadInt32();
            var len = next >= prev ? (uint)(next - prev) : 0u;
            result[i] = new Il2CppStringLiteral(len, prev);
            prev = next;
        }
        return result;
    }

    private T[] ReadCompact<T>(string sectionName, CompactRowReader<T> read)
    {
        var s = Header[sectionName];
        var count = Math.Max(0, s.Count);
        var inBounds = s.Offset >= 0 && s.Size >= 0 && s.End <= _file.Length;
        var elementSize = count > 0 ? s.Size / count : 0;
        var aligned = count == 0 ? s.Size == 0 : s.Size % count == 0;
        _sectionReports.Add(new MetadataSectionReport(s.Name, s.Offset, s.Size, elementSize, count, inBounds, aligned, s.Description));
        if (!inBounds || count == 0) return [];

        var result = new T[count];
        var reader = new SpanReader(_file.Span.Slice(s.Offset, s.Size));
        for (var i = 0; i < count; i++)
            result[i] = read(ref reader);

        if (reader.Position != s.Size)
            _sink.Warn(DiagnosticCategory.MetadataIntegrity, DiagnosticCodes.MetadataSectionMisaligned,
                $"Compact section '{s.Name}' read {reader.Position} bytes vs header size {s.Size}.", Loc(s.Name));

        _sink.Verbose(DiagnosticCategory.Metadata, DiagnosticCodes.MetadataTableRead, $"{s.Name}: {count:N0} x {elementSize} bytes.", Loc(s.Name));
        return result;
    }

    private int[] ReadCompactInts(string sectionName, int indexSize) =>
        ReadCompact(sectionName, (ref r) => SerializedIndexSizes.ReadIndex(ref r, indexSize));

    private void ReportCounted(string sectionName)
    {
        var s = Header[sectionName];
        var inBounds = s.Offset >= 0 && s.Size >= 0 && s.End <= _file.Length;
        var count = Math.Max(0, s.Count);
        var elemSize = count > 0 ? s.Size / count : 0;
        var aligned = count == 0 ? s.Size == 0 : s.Size % count == 0;
        _sectionReports.Add(new MetadataSectionReport(s.Name, s.Offset, s.Size, elemSize, count, inBounds, aligned, s.Description));
    }

    private T[] ReadTable<T>(string sectionName, MetadataVersion? layout = null) where T : IMetadataRecord<T>
    {
        var v = layout ?? Version;
        var s = Header[sectionName];
        var elementSize = T.SizeOf(v);
        var inBounds = s.Offset >= 0 && s.Size >= 0 && s.End <= _file.Length;
        var aligned = s.Size % elementSize == 0;
        var count = inBounds ? s.Size / elementSize : 0;
        _sectionReports.Add(new MetadataSectionReport(s.Name, s.Offset, s.Size, elementSize, count, inBounds, aligned, s.Description));

        if (!inBounds) return [];
        if (!aligned)
            _sink.Warn(DiagnosticCategory.MetadataIntegrity, DiagnosticCodes.MetadataSectionMisaligned,
                $"Section '{s.Name}' size {s.Size} is not a multiple of the {elementSize}-byte record; trailing {s.Size % elementSize} byte(s) ignored.",
                Loc(s.Name));

        var result = new T[count];
        var reader = new SpanReader(_file.Span.Slice(s.Offset, count * elementSize));
        for (var i = 0; i < count; i++)
            result[i] = T.Read(ref reader, v);

        _sink.Verbose(DiagnosticCategory.Metadata, DiagnosticCodes.MetadataTableRead, $"{s.Name}: {count:N0} x {elementSize} bytes.", Loc(s.Name));
        return result;
    }

    private int[] ReadInts(string sectionName) => [.. ReadTable<Int32Element>(sectionName).Select(e => e.Value)];

    private void ReportBlob(string sectionName)
    {
        var s = Header[sectionName];
        var inBounds = s.Offset >= 0 && s.Size >= 0 && s.End <= _file.Length;
        _sectionReports.Add(new MetadataSectionReport(s.Name, s.Offset, s.Size, 1, s.Size, inBounds, true, s.Description));
    }

    /// <summary>Checks that every start/count pair points inside its target table.</summary>
    private void ValidateCrossReferences()
    {
        var problems = 0;
        void Check(bool ok, string table, int index, string what)
        {
            if (ok) return;
            if (++problems <= 50)
                _sink.Warn(DiagnosticCategory.MetadataIntegrity, DiagnosticCodes.MetadataIndexOutOfRange, what, Loc(table, index));
        }

        for (var i = 0; i < TypeDefinitions.Length; i++)
        {
            var t = TypeDefinitions[i];
            Check(InRange(t.MethodStart, t.MethodCount, Methods.Length), "typeDefinitions", i, $"Type {i}: methods [{t.MethodStart}, +{t.MethodCount}] out of range.");
            Check(InRange(t.FieldStart, t.FieldCount, Fields.Length), "typeDefinitions", i, $"Type {i}: fields [{t.FieldStart}, +{t.FieldCount}] out of range.");
            Check(InRange(t.PropertyStart, t.PropertyCount, Properties.Length), "typeDefinitions", i, $"Type {i}: properties out of range.");
            Check(InRange(t.EventStart, t.EventCount, Events.Length), "typeDefinitions", i, $"Type {i}: events out of range.");
            Check(InRange(t.NestedTypesStart, t.NestedTypeCount, NestedTypes.Length), "typeDefinitions", i, $"Type {i}: nested types out of range.");
            Check(InRange(t.InterfacesStart, t.InterfacesCount, Interfaces.Length), "typeDefinitions", i, $"Type {i}: interfaces out of range.");
            Check(InRange(t.VtableStart, t.VtableCount, VTableMethods.Length), "typeDefinitions", i, $"Type {i}: vtable out of range.");
        }
        for (var i = 0; i < Methods.Length; i++)
        {
            var m = Methods[i];
            Check(InRange(m.ParameterStart, m.ParameterCount, Parameters.Length), "methods", i, $"Method {i}: parameters out of range.");
            Check((uint)m.DeclaringType < (uint)TypeDefinitions.Length, "methods", i, $"Method {i}: declaring type {m.DeclaringType} out of range.");
        }
        for (var i = 0; i < Images.Length; i++)
        {
            var img = Images[i];
            Check(InRange(img.TypeStart, (int)img.TypeCount, TypeDefinitions.Length), "images", i, $"Image {i}: types out of range.");
        }

        if (problems > 50)
            _sink.Warn(DiagnosticCategory.MetadataIntegrity, DiagnosticCodes.MetadataIndexOutOfRange,
                $"{problems} cross-reference problems in total (first 50 reported individually).", Loc());
    }

    private static bool InRange(int start, int count, int length) =>
        count == 0 || (start >= 0 && (long)start + count <= length);

    private DiagnosticLocation Loc(string? table = null, int? index = null) => new() { File = Path, Table = table, MetadataIndex = index };

    private MetadataLoadException Fail(string code, string message)
    {
        _sink.Error(DiagnosticCategory.Metadata, code, message, Loc());
        return new MetadataLoadException(message);
    }

    public void Dispose() => _file.Dispose();
}
