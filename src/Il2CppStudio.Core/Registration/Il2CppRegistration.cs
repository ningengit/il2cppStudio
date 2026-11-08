using System.Runtime.InteropServices;
using Il2CppStudio.Core.Binary;
using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Metadata;

namespace Il2CppStudio.Core.Registration;

/// <summary>
/// Locates and parses Il2CppCodeRegistration / Il2CppMetadataRegistration (libil2cpp il2cpp-class-internals.h,
/// Unity 2022.3 layout) in a 64-bit binary, using the signature-scan strategy of Il2CppDumper adapted to v27+:
/// <list type="bullet">
/// <item>MetadataRegistration: fieldOffsetsCount and typeDefinitionsSizesCount both equal the metadata
/// typeDefinitions count and are each followed by a valid data pointer.</item>
/// <item>CodeRegistration: codeGenModulesCount equals the image count and is followed by a pointer to an array
/// of Il2CppCodeGenModule* whose moduleName strings are the image names.</item>
/// </list>
/// Scanning is done over 8-byte aligned <see cref="ulong"/> views with vectorized <c>IndexOf</c>.
/// </summary>
public sealed partial class Il2CppRegistration
{
    public ulong CodeRegistrationVa { get; private set; }
    public ulong MetadataRegistrationVa { get; private set; }

    public IReadOnlyList<CodeGenModuleRecord> CodeGenModules { get; private set; } = [];
    public IReadOnlyList<ulong> GenericMethodPointers { get; private set; } = [];
    public IReadOnlyList<ulong> TypeVas { get; private set; } = [];
    public IReadOnlyList<Il2CppMethodSpecRecord> MethodSpecs { get; private set; } = [];
    public IReadOnlyList<Il2CppGenericMethodFunction> GenericMethodTable { get; private set; } = [];
    public IReadOnlyList<ulong> GenericInstVas { get; private set; } = [];
    public IReadOnlyList<ulong> FieldOffsetTableVas { get; private set; } = [];
    public IReadOnlyList<ulong> TypeSizeTableVas { get; private set; } = [];

    /// <summary>Il2CppTypeDefinitionSizes.instance_size (includes the 16-byte object header, also for value types), or 0.</summary>
    public int GetInstanceSize(int typeDefIndex) =>
        (uint)typeDefIndex < (uint)TypeSizeTableVas.Count && TypeSizeTableVas[typeDefIndex] != 0 ? Reader.ReadI32(TypeSizeTableVas[typeDefIndex]) : 0;

    /// <summary>Unboxed size of a value type (instance_size minus the object header), or 0 if unknown.</summary>
    public int GetValueTypeSize(int typeDefIndex) => Math.Max(0, GetInstanceSize(typeDefIndex) - 16);
    public IReadOnlyDictionary<ulong, MetadataUsage> MetadataUsages { get; private set; } = new Dictionary<ulong, MetadataUsage>();

    private readonly Dictionary<ulong, int> _typeIndexByVa = [];
    private readonly Dictionary<ulong, Il2CppTypeRecord> _typeCache = [];

    public VirtualReader Reader { get; }
    private readonly GlobalMetadata _md;
    private readonly IDiagnosticSink _sink;

    private Il2CppRegistration(VirtualReader reader, GlobalMetadata md, IDiagnosticSink sink)
    {
        Reader = reader; _md = md; _sink = sink;
    }

    public bool IsComplete => CodeRegistrationVa != 0 && MetadataRegistrationVa != 0;

    public static Il2CppRegistration Load(IBinaryImage image, GlobalMetadata md, IDiagnosticSink sink)
    {
        var reg = new Il2CppRegistration(new VirtualReader(image), md, sink);
        reg.LocateMetadataRegistration();
        reg.LocateCodeRegistration();
        reg.ScanMetadataUsages();
        return reg;
    }

    // ------------------------------------------------------------------ type access

    public int TypeCount => TypeVas.Count;

    public bool TryGetType(int typeIndex, out Il2CppTypeRecord type)
    {
        if ((uint)typeIndex >= (uint)TypeVas.Count) { type = default; return false; }
        type = GetTypeAt(TypeVas[typeIndex]);
        return true;
    }

    public Il2CppTypeRecord GetTypeAt(ulong va)
    {
        if (_typeCache.TryGetValue(va, out var t)) return t;
        t = new Il2CppTypeRecord(va, P(va), _md.Version.IsV24 ? NormalizeTypeBitsV24(Reader.ReadU32(va + (ulong)Ptr)) : Reader.ReadU32(va + (ulong)Ptr));
        _typeCache[va] = t;
        return t;
    }

    public int TypeIndexOfVa(ulong va) => _typeIndexByVa.GetValueOrDefault(va, -1);

    /// <summary>
    /// Il2CppGenericClass. v27+: { Il2CppType* type; context; cached_class }. v24: the first field is the generic
    /// TypeDefinitionIndex instead; it is mapped to that definition's byval Il2CppType so callers see one layout.
    /// </summary>
    public Il2CppGenericClassRecord ReadGenericClass(ulong va)
    {
        var first = P(va);
        if (_md.Version.IsV24)
        {
            var td = unchecked((int)first);
            first = (uint)td < (uint)_md.TypeDefinitions.Length && (uint)_md.TypeDefinitions[td].ByvalTypeIndex < (uint)TypeVas.Count
                ? TypeVas[_md.TypeDefinitions[td].ByvalTypeIndex] : 0;
        }
        return new(va, first, At(va, 1), At(va, 2));
    }

    public Il2CppGenericInstRecord ReadGenericInst(ulong va)
    {
        var argc = P(va);
        var argv = At(va, 1);
        if (argc > 64) return new(va, []);
        var list = new ulong[argc];
        for (var i = 0; i < (int)argc; i++) list[i] = At(argv, i);
        return new(va, list);
    }

    public Il2CppArrayTypeRecord ReadArrayType(ulong va) => new(va, P(va), Reader.Slice(va + (ulong)Ptr, 1) is { Length: 1 } r ? r[0] : (byte)1);

    /// <summary>Field offset of field <paramref name="fieldOrdinal"/> (0-based within its type), or -1.</summary>
    public int GetFieldOffset(int typeDefIndex, int fieldOrdinal)
    {
        if ((uint)typeDefIndex >= (uint)FieldOffsetTableVas.Count) return -1;
        var table = FieldOffsetTableVas[typeDefIndex];
        return table == 0 ? -1 : Reader.ReadI32(table + (ulong)fieldOrdinal * 4);
    }

    // ------------------------------------------------------------------ locating

    private void LocateMetadataRegistration()
    {
        var typeDefCount = (ulong)_md.TypeDefinitions.Length;
        var candidates = new List<ulong>();
        foreach (var (start, end, _) in Reader.DataRanges())
        {
            var words = Words(start, end);
            var pos = 0;
            while (pos < words.Length - 4)
            {
                var hit = words.AsSpan(pos).IndexOf(typeDefCount);
                if (hit < 0) break;
                var i = pos + hit;
                pos = i + 1;
                if (i + 3 >= words.Length || i < 10) continue;
                if (words[i + 2] != typeDefCount) continue;
                if (!Reader.IsMapped(words[i + 1]) || !Reader.IsMapped(words[i + 3])) continue;
                var typesCount = words[i - 4];
                if (typesCount == 0 || typesCount > 2_000_000 || !Reader.IsMapped(words[i - 3])) continue;
                candidates.Add(start + (ulong)((i - 10) * Ptr));
            }
        }

        if (candidates.Count == 0)
        {
            _sink.Error(DiagnosticCategory.Registration, DiagnosticCodes.RegistrationNotFound,
                "Il2CppMetadataRegistration not found; type signatures and field offsets are unavailable.", new() { File = Reader.Image.Path });
            return;
        }
        if (candidates.Count > 1)
            _sink.Warn(DiagnosticCategory.Registration, DiagnosticCodes.RegistrationAmbiguous,
                $"{candidates.Count} MetadataRegistration candidates; using the first at 0x{candidates[0]:X}.", new() { VirtualAddress = candidates[0] });

        var va = MetadataRegistrationVa = candidates[0];
        ulong F(int i) => At(va, i);

        GenericInstVas = ReadPointerArray(F(3), (int)Math.Min(F(2), 1_000_000));
        // Il2CppGenericMethodIndices: {methodIndex, invokerIndex} in v24.0-24.4, + adjustorThunkIndex from 24.5/27.1.
        var gmStride = _md.Version.IsV24 ? 12 : 16;
        GenericMethodTable = ReadArray(F(5), (int)Math.Min(F(4), 2_000_000), gmStride,
            s => new Il2CppGenericMethodFunction(BitConverter.ToInt32(s), BitConverter.ToInt32(s[4..]), BitConverter.ToInt32(s[8..]), s.Length >= 16 ? BitConverter.ToInt32(s[12..]) : -1));
        TypeVas = ReadPointerArray(F(7), (int)Math.Min(F(6), 2_000_000));
        MethodSpecs = ReadArray(F(9), (int)Math.Min(F(8), 2_000_000), 12,
            s => new Il2CppMethodSpecRecord(BitConverter.ToInt32(s), BitConverter.ToInt32(s[4..]), BitConverter.ToInt32(s[8..])));
        FieldOffsetTableVas = ReadPointerArray(F(11), (int)Math.Min(F(10), 2_000_000));
        TypeSizeTableVas = ReadPointerArray(F(13), (int)Math.Min(F(12), 2_000_000));
        for (var i = 0; i < TypeVas.Count; i++) _typeIndexByVa.TryAdd(TypeVas[i], i);

        _sink.Info(DiagnosticCategory.Registration, DiagnosticCodes.RegistrationFound,
            $"MetadataRegistration at RVA 0x{va - Reader.ImageBase:X}: {TypeVas.Count:N0} types, {GenericInstVas.Count:N0} generic insts, " +
            $"{MethodSpecs.Count:N0} method specs, {GenericMethodTable.Count:N0} generic method functions.",
            new() { Rva = va - Reader.ImageBase, VirtualAddress = va });
    }

    private void LocateCodeRegistration()
    {
        if (_md.Version.HasCodegenIndices)
        {
            if (!LocateCodeRegistrationV24())
                _sink.Error(DiagnosticCategory.Registration, DiagnosticCodes.RegistrationNotFound,
                    $"Il2CppCodeRegistration (v{_md.Version} layout) not found; method addresses are unavailable.", new() { File = Reader.Image.Path });
            return;
        }
        var imageCount = (ulong)_md.Images.Length;
        var imageNames = _md.Images.Select(i => _md.GetString(i.NameIndex)).ToHashSet(StringComparer.Ordinal);
        foreach (var (start, end, _) in Reader.DataRanges())
        {
            var words = Words(start, end);
            var pos = 0;
            while (pos < words.Length - 2)
            {
                var hit = words.AsSpan(pos).IndexOf(imageCount);
                if (hit < 0) break;
                var i = pos + hit;
                pos = i + 1;
                if (i + 1 >= words.Length) break;
                var arrayVa = words[i + 1];
                if (!Reader.IsMapped(arrayVa)) continue;
                var firstModule = P(arrayVa);
                if (!Reader.IsMapped(firstModule)) continue;
                var name = Reader.ReadCString(P(firstModule));
                if (name is null || !imageNames.Contains(name)) continue;

                var fieldVa = start + (ulong)(i * Ptr);
                // codeGenModulesCount is field 15 in v29.1+/v31 (unresolvedInstance/StaticCallPointers present), 13 before.
                var fieldIndex = _md.Version.Major >= 31 ? 15 : 13;
                CodeRegistrationVa = fieldVa - (ulong)(fieldIndex * Ptr);
                ParseCodeRegistration(arrayVa, (int)imageCount);
                return;
            }
        }
        _sink.Error(DiagnosticCategory.Registration, DiagnosticCodes.RegistrationNotFound,
            "Il2CppCodeRegistration not found; method addresses are unavailable.", new() { File = Reader.Image.Path });
    }

    private void ParseCodeRegistration(ulong modulesArrayVa, int count)
    {
        var modules = new List<CodeGenModuleRecord>(count);
        for (var i = 0; i < count; i++)
        {
            var m = At(modulesArrayVa, i);
            var name = Reader.ReadCString(P(m)) ?? $"<module-{i}>";
            var ptrCount = Reader.ReadU32(m + (ulong)Ptr);   // uint32_t methodPointerCount (padded on 64-bit)
            var ptrs = CodePointers(ReadPointerArray(At(m, 2), (int)Math.Min(ptrCount, 1_000_000)));
            modules.Add(new CodeGenModuleRecord(m, name, ptrs));
        }
        CodeGenModules = modules;

        var cr = CodeRegistrationVa;
        var gmpCount = At(cr, 2);
        if (gmpCount < 5_000_000)
            GenericMethodPointers = CodePointers(ReadPointerArray(At(cr, 3), (int)gmpCount));

        _sink.Info(DiagnosticCategory.Registration, DiagnosticCodes.RegistrationFound,
            $"CodeRegistration at RVA 0x{cr - Reader.ImageBase:X}: {modules.Count} code gen modules, " +
            $"{modules.Sum(m => m.MethodPointers.Count):N0} method pointers, {GenericMethodPointers.Count:N0} generic method pointers.",
            new() { Rva = cr - Reader.ImageBase, VirtualAddress = cr });
    }

    /// <summary>
    /// v27+ runtime-metadata globals (TypeInfo / RuntimeMethod / string literal vars) are emitted into writable data
    /// holding an encoded token until il2cpp_codegen_initialize_runtime_metadata resolves them. Scanning .data for
    /// that encoding recovers the VA → metadata mapping the lifter uses to resolve RIP-relative loads.
    /// </summary>
    private void ScanMetadataUsages()
    {
        if (_md.Version.HasMetadataUsageTables)
        {
            MetadataUsages = ReadMetadataUsagesV24();
            _sink.Info(DiagnosticCategory.Registration, DiagnosticCodes.MetadataUsagesScanned,
                $"{MetadataUsages.Count:N0} runtime metadata globals recovered from v24 metadataUsages " +
                $"({MetadataUsages.Values.Count(u => u.Kind == MetadataUsageKind.StringLiteral):N0} string literals, {MetadataUsages.Values.Count(u => u.Kind == MetadataUsageKind.TypeInfo):N0} TypeInfo).");
            return;
        }
        var map = new Dictionary<ulong, MetadataUsage>();
        var limits = new int[8];
        limits[(int)MetadataUsageKind.TypeInfo] = TypeVas.Count;
        limits[(int)MetadataUsageKind.Il2CppType] = TypeVas.Count;
        limits[(int)MetadataUsageKind.MethodDef] = _md.Methods.Length;
        limits[(int)MetadataUsageKind.FieldInfo] = _md.FieldRefs.Length;
        limits[(int)MetadataUsageKind.StringLiteral] = _md.StringLiterals.Length;
        limits[(int)MetadataUsageKind.MethodRef] = MethodSpecs.Count;
        limits[(int)MetadataUsageKind.FieldRva] = _md.FieldRefs.Length;

        foreach (var (start, end, writable) in Reader.DataRanges())
        {
            if (!writable) continue;
            var words = Words(start, end);
            for (var i = 0; i < words.Length; i++)
            {
                var v = words[i];
                if ((v & 1) == 0 || v > uint.MaxValue) continue;
                var kind = (int)((v >> 29) & 7);
                if (kind == 0) continue;
                var index = (int)((v & 0x1FFFFFFE) >> 1);
                if (index >= limits[kind]) continue;
                var va = start + (ulong)(i * Ptr);
                map[va] = new MetadataUsage(va, (MetadataUsageKind)kind, index);
            }
        }
        MetadataUsages = map;
        _sink.Info(DiagnosticCategory.Registration, DiagnosticCodes.MetadataUsagesScanned,
            $"{map.Count:N0} runtime metadata globals recovered ({map.Values.Count(u => u.Kind == MetadataUsageKind.StringLiteral):N0} string literals, " +
            $"{map.Values.Count(u => u.Kind == MetadataUsageKind.TypeInfo):N0} TypeInfo, {map.Values.Count(u => u.Kind is MetadataUsageKind.MethodDef or MetadataUsageKind.MethodRef):N0} methods).");
    }

    // ------------------------------------------------------------------ helpers

    private ulong[] ReadPointerArray(ulong va, int count)
    {
        if (count <= 0 || !Reader.IsMapped(va)) return [];
        var span = Reader.Slice(va, count * Ptr);
        if (span.Length < count * Ptr) return [];
        if (Ptr == 8) return MemoryMarshal.Cast<byte, ulong>(span).ToArray();
        var narrow = MemoryMarshal.Cast<byte, uint>(span);
        var wide = new ulong[count];
        for (var i = 0; i < count; i++) wide[i] = narrow[i];
        return wide;
    }

    private T[] ReadArray<T>(ulong va, int count, int stride, SpanParser<T> parse)
    {
        if (count <= 0 || !Reader.IsMapped(va)) return [];
        var span = Reader.Slice(va, count * stride);
        if (span.Length < count * stride) return [];
        var result = new T[count];
        for (var i = 0; i < count; i++) result[i] = parse(span.Slice(i * stride, stride));
        return result;
    }

    private delegate T SpanParser<out T>(ReadOnlySpan<byte> s);
}
