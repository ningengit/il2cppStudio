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
public sealed class Il2CppRegistration
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
        if (!image.Is64Bit)
        {
            sink.Error(DiagnosticCategory.Registration, DiagnosticCodes.RegistrationNotFound, "Only 64-bit binaries are supported for registration parsing.", new() { File = image.Path });
            return reg;
        }
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
        t = new Il2CppTypeRecord(va, Reader.ReadU64(va), Reader.ReadU32(va + 8));
        _typeCache[va] = t;
        return t;
    }

    public int TypeIndexOfVa(ulong va) => _typeIndexByVa.GetValueOrDefault(va, -1);

    public Il2CppGenericClassRecord ReadGenericClass(ulong va) =>
        new(va, Reader.ReadU64(va), Reader.ReadU64(va + 8), Reader.ReadU64(va + 16));

    public Il2CppGenericInstRecord ReadGenericInst(ulong va)
    {
        var argc = Reader.ReadU64(va);
        var argv = Reader.ReadU64(va + 8);
        if (argc > 64) return new(va, []);
        var list = new ulong[argc];
        for (var i = 0; i < (int)argc; i++) list[i] = Reader.ReadU64(argv + (ulong)i * 8);
        return new(va, list);
    }

    public Il2CppArrayTypeRecord ReadArrayType(ulong va) => new(va, Reader.ReadU64(va), Reader.Slice(va + 8, 1) is { Length: 1 } r ? r[0] : (byte)1);

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
            var span = Reader.Slice(start, (int)(end - start));
            var words = MemoryMarshal.Cast<byte, ulong>(span[..(span.Length & ~7)]);
            var pos = 0;
            while (pos < words.Length - 4)
            {
                var hit = words[pos..].IndexOf(typeDefCount);
                if (hit < 0) break;
                var i = pos + hit;
                pos = i + 1;
                if (i + 3 >= words.Length || i < 10) continue;
                if (words[i + 2] != typeDefCount) continue;
                if (!Reader.IsMapped(words[i + 1]) || !Reader.IsMapped(words[i + 3])) continue;
                var typesCount = words[i - 4];
                if (typesCount == 0 || typesCount > 2_000_000 || !Reader.IsMapped(words[i - 3])) continue;
                candidates.Add(start + (ulong)(i - 10) * 8);
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
        ulong F(int i) => Reader.ReadU64(va + (ulong)i * 8);

        GenericInstVas = ReadPointerArray(F(3), (int)Math.Min(F(2), 1_000_000));
        GenericMethodTable = ReadArray(F(5), (int)Math.Min(F(4), 2_000_000), 16,
            s => new Il2CppGenericMethodFunction(BitConverter.ToInt32(s), BitConverter.ToInt32(s[4..]), BitConverter.ToInt32(s[8..]), BitConverter.ToInt32(s[12..])));
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
        var imageCount = (ulong)_md.Images.Length;
        var imageNames = _md.Images.Select(i => _md.GetString(i.NameIndex)).ToHashSet(StringComparer.Ordinal);
        foreach (var (start, end, _) in Reader.DataRanges())
        {
            var span = Reader.Slice(start, (int)(end - start));
            var words = MemoryMarshal.Cast<byte, ulong>(span[..(span.Length & ~7)]);
            var pos = 0;
            while (pos < words.Length - 2)
            {
                var hit = words[pos..].IndexOf(imageCount);
                if (hit < 0) break;
                var i = pos + hit;
                pos = i + 1;
                var arrayVa = words[i + 1];
                if (!Reader.IsMapped(arrayVa)) continue;
                var firstModule = Reader.ReadU64(arrayVa);
                if (!Reader.IsMapped(firstModule)) continue;
                var name = Reader.ReadCString(Reader.ReadU64(firstModule));
                if (name is null || !imageNames.Contains(name)) continue;

                var fieldVa = start + (ulong)i * 8;
                // codeGenModulesCount is field 15 in v29.1+/v31 (unresolvedInstance/StaticCallPointers present), 13 before.
                var fieldIndex = _md.Version.Major >= 31 ? 15 : 13;
                CodeRegistrationVa = fieldVa - (ulong)fieldIndex * 8;
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
            var m = Reader.ReadU64(modulesArrayVa + (ulong)i * 8);
            var name = Reader.ReadCString(Reader.ReadU64(m)) ?? $"<module-{i}>";
            var ptrCount = Reader.ReadU64(m + 8);
            var ptrs = ReadPointerArray(Reader.ReadU64(m + 16), (int)Math.Min(ptrCount, 1_000_000));
            modules.Add(new CodeGenModuleRecord(m, name, ptrs));
        }
        CodeGenModules = modules;

        var cr = CodeRegistrationVa;
        var gmpCount = Reader.ReadU64(cr + 16);
        if (gmpCount < 5_000_000)
            GenericMethodPointers = ReadPointerArray(Reader.ReadU64(cr + 24), (int)gmpCount);

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
            var span = Reader.Slice(start, (int)(end - start));
            var words = MemoryMarshal.Cast<byte, ulong>(span[..(span.Length & ~7)]);
            for (var i = 0; i < words.Length; i++)
            {
                var v = words[i];
                if ((v & 1) == 0 || v > uint.MaxValue) continue;
                var kind = (int)((v >> 29) & 7);
                if (kind == 0) continue;
                var index = (int)((v & 0x1FFFFFFE) >> 1);
                if (index >= limits[kind]) continue;
                var va = start + (ulong)i * 8;
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
        var span = Reader.Slice(va, count * 8);
        if (span.Length < count * 8) return [];
        return MemoryMarshal.Cast<byte, ulong>(span).ToArray();
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
