using System.Collections.Concurrent;
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
    public IReadOnlyList<ulong> GlobalMethodPointers { get; private set; } = [];
    public IReadOnlyList<ulong> GenericMethodPointers { get; private set; } = [];
    public IReadOnlyList<ulong> TypeVas { get; private set; } = [];
    public IReadOnlyList<Il2CppMethodSpecRecord> MethodSpecs { get; private set; } = [];
    public IReadOnlyList<Il2CppGenericMethodFunction> GenericMethodTable { get; private set; } = [];
    public IReadOnlyList<ulong> GenericInstVas { get; private set; } = [];
    public IReadOnlyList<ulong> FieldOffsetTableVas { get; private set; } = [];
    public bool FieldOffsetsArePointers { get; private set; } = true;
    public IReadOnlyList<ulong> TypeSizeTableVas { get; private set; } = [];
    public bool TypeSizesArePointers { get; private set; } = true;
    public int[] MethodReferences { get; private set; } = [];

    private int[] _flatFieldOffsets = [];
    private int[] _flatTypeInstanceSizes = [];

    /// <summary>Il2CppTypeDefinitionSizes.instance_size (includes the 16-byte object header, also for value types), or 0.</summary>
    public int GetInstanceSize(int typeDefIndex) => TypeSizesArePointers
        ? ((uint)typeDefIndex < (uint)TypeSizeTableVas.Count && TypeSizeTableVas[typeDefIndex] != 0 ? Reader.ReadI32(TypeSizeTableVas[typeDefIndex]) : 0)
        : ((uint)typeDefIndex < (uint)_flatTypeInstanceSizes.Length ? _flatTypeInstanceSizes[typeDefIndex] : 0);

    /// <summary>Unboxed size of a value type (instance_size minus the object header), or 0 if unknown.</summary>
    public int GetValueTypeSize(int typeDefIndex) => Math.Max(0, GetInstanceSize(typeDefIndex) - 2 * Ptr);
    public IReadOnlyDictionary<ulong, MetadataUsage> MetadataUsages { get; private set; } = new Dictionary<ulong, MetadataUsage>();

    private readonly Dictionary<ulong, int> _typeIndexByVa = [];
    private readonly ConcurrentDictionary<ulong, Il2CppTypeRecord> _typeCache = new();

    private ulong _codeGenModulesArrayVa;

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
        var reader = new VirtualReader(image);
        if (RegistrationCache.TryRead(image, md, out var cached))
        {
            var fast = new Il2CppRegistration(reader, md, sink);
            if (fast.TryLoadFromCache(in cached))
                return fast;
        }

        var reg = new Il2CppRegistration(reader, md, sink);
        reg.LocateMetadataRegistration();
        reg.LocateCodeRegistration();
        reg.ScanMetadataUsages();
        if (reg.IsComplete)
        {
            var usages = reg.MetadataUsages as Dictionary<ulong, MetadataUsage>
                ?? new Dictionary<ulong, MetadataUsage>(reg.MetadataUsages);
            RegistrationCache.TryWrite(image, md,
                new CachedRegistration(reg.MetadataRegistrationVa, reg.CodeRegistrationVa, reg._codeGenModulesArrayVa, usages));
        }
        return reg;
    }

    private bool TryLoadFromCache(in CachedRegistration cached)
    {
        if (!Reader.IsMapped(cached.MetadataRegistrationVa) || !Reader.IsMapped(cached.CodeRegistrationVa))
            return false;

        var typeDefCount = (ulong)_md.TypeDefinitions.Length;
        var fieldOffIdx = MetadataFieldOffsetsWordIndex;
        var typeSizeIdx = fieldOffIdx + 2;
        var fieldOffCount = At(cached.MetadataRegistrationVa, fieldOffIdx);
        if ((fieldOffCount != typeDefCount && fieldOffCount != (ulong)_md.Fields.Length)
            || At(cached.MetadataRegistrationVa, typeSizeIdx) != typeDefCount)
            return false;
        var typesWord = _md.Version.HasV108EmbeddedGenerics ? 4 : 6;
        var typesCount = At(cached.MetadataRegistrationVa, typesWord);
        if (typesCount == 0 || typesCount > 2_000_000 || !Reader.IsMapped(At(cached.MetadataRegistrationVa, typesWord + 1)))
            return false;

        if (_md.Version.HasCodegenIndices)
        {
            var (_, _, attrIndex, _) = GetPreV242CodeRegLayout(_md.Version);
            var methodCount = _md.Methods.Length == 0 ? 0UL : (ulong)(_md.Methods.Max(m => m.MethodIndex) + 1);
            if (methodCount == 0
                || At(cached.CodeRegistrationVa, 0) != methodCount
                || (_md.Version.HasCustomAttributeTables && At(cached.CodeRegistrationVa, attrIndex) != (ulong)_md.AttributeTypeRanges.Length)
                || !Reader.IsMapped(At(cached.CodeRegistrationVa, 1)))
                return false;

            ParseMetadataRegistration(cached.MetadataRegistrationVa);
            if (!ParseCodeRegistrationV24(cached.CodeRegistrationVa, methodCount))
                return false;
        }
        else
        {
            if (cached.CodeGenModulesArrayVa == 0 || !Reader.IsMapped(cached.CodeGenModulesArrayVa))
                return false;
            var firstModule = P(cached.CodeGenModulesArrayVa);
            if (!Reader.IsMapped(firstModule) || Reader.ReadCString(P(firstModule)) is null)
                return false;

            ParseMetadataRegistration(cached.MetadataRegistrationVa);
            CodeRegistrationVa = cached.CodeRegistrationVa;
            _codeGenModulesArrayVa = cached.CodeGenModulesArrayVa;
            ParseCodeRegistration(cached.CodeGenModulesArrayVa, _md.Images.Length);
        }

        MetadataUsages = cached.MetadataUsages;
        EmitMetadataUsagesDiagnostic(cached.MetadataUsages, _md.Version.HasMetadataUsageTables);
        return IsComplete;
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
        t = new Il2CppTypeRecord(va, P(va), _md.Version.IsPreV27 ? NormalizeTypeBitsV24(Reader.ReadU32(va + (ulong)Ptr)) : Reader.ReadU32(va + (ulong)Ptr));
        _typeCache[va] = t;
        return t;
    }

    public int TypeIndexOfVa(ulong va) => _typeIndexByVa.GetValueOrDefault(va, -1);

    /// <summary>
    /// Il2CppGenericClass. v27+: { Il2CppType* type; context; cached_class }. Pre-v27: the first field is the generic
    /// TypeDefinitionIndex instead; it is mapped to that definition's byval Il2CppType so callers see one layout.
    /// </summary>
    public Il2CppGenericClassRecord ReadGenericClass(ulong va)
    {
        var first = P(va);
        if (_md.Version.IsPreV27)
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
        if (!FieldOffsetsArePointers)
        {
            if ((uint)typeDefIndex >= (uint)_md.TypeDefinitions.Length) return -1;
            var fieldIndex = _md.TypeDefinitions[typeDefIndex].FieldStart + fieldOrdinal;
            return (uint)fieldIndex < (uint)_flatFieldOffsets.Length ? _flatFieldOffsets[fieldIndex] : -1;
        }
        if ((uint)typeDefIndex >= (uint)FieldOffsetTableVas.Count) return -1;
        var table = FieldOffsetTableVas[typeDefIndex];
        return table == 0 ? -1 : Reader.ReadI32(table + (ulong)fieldOrdinal * 4);
    }

    /// <summary>Resolves a v16 encoded method reference index (<c>MetadataCache::GetMethodInfoFromIndex</c>).</summary>
    public bool TryResolveV16EncodedMethodIndex(uint encodedIndex, out MetadataUsageKind kind, out int targetIndex)
    {
        var decoded = (int)(encodedIndex & 0x7FFFFFFFu);
        if (decoded <= 0 || decoded >= MethodReferences.Length)
        {
            kind = MetadataUsageKind.Invalid;
            targetIndex = -1;
            return false;
        }
        targetIndex = MethodReferences[decoded];
        var isGeneric = (encodedIndex & 0x80000000u) != 0;
        kind = isGeneric ? MetadataUsageKind.MethodRef : MetadataUsageKind.MethodDef;
        return isGeneric ? (uint)targetIndex < (uint)MethodSpecs.Count : (uint)targetIndex < (uint)_md.Methods.Length;
    }

    // ------------------------------------------------------------------ locating

    private int MetadataFieldOffsetsWordIndex =>
        _md.Version.HasV108EmbeddedGenerics ? 6
        : _md.Version.HasMethodReferencesInMetadataReg ? 12
        : 10;

    private void LocateMetadataRegistration()
    {
        var typeDefCount = (ulong)_md.TypeDefinitions.Length;
        var candidates = new List<ulong>();
        foreach (var (start, end, _) in Reader.DataRanges())
        {
            var aligned = AlignUp(start);
            if (aligned >= end) continue;
            if (Ptr == 8) ScanMetadataCandidates(Words64(aligned, end), aligned, typeDefCount, candidates);
            else ScanMetadataCandidates(Words32(aligned, end), aligned, (uint)typeDefCount, candidates);
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

        ParseMetadataRegistration(candidates[0]);
    }

    private void ScanMetadataCandidates<TWord>(ReadOnlySpan<TWord> words, ulong start, TWord target, List<ulong> candidates)
        where TWord : unmanaged, IEquatable<TWord>
    {
        var typeDefCount = WordToU64(target);
        var fieldOffIdx = MetadataFieldOffsetsWordIndex;
        var typeSizeIdx = fieldOffIdx + 2;
        var fieldsCount = (ulong)_md.Fields.Length;

        var pos = 0;
        while (pos < words.Length - 2)
        {
            var hit = words[pos..].IndexOf(target);
            if (hit < 0) break;
            var i = pos + hit; // candidate index for typeDefinitionsSizesCount (word typeSizeIdx)
            pos = i + 1;
            if (i < typeSizeIdx || i + 1 >= words.Length) continue;

            var typeSizesPtr = WordToU64(words[i + 1]);
            if (typeDefCount > 0 && !Reader.IsMapped(typeSizesPtr)) continue;

            var fieldOffCount = WordToU64(words[i - 2]);
            var fieldOffPtr = WordToU64(words[i - 1]);
            if (fieldOffCount != typeDefCount && (_md.Version.Major > 21 || fieldOffCount != fieldsCount)) continue;
            if (fieldOffCount > 0 && !Reader.IsMapped(fieldOffPtr)) continue;

            var baseIdx = i - typeSizeIdx;
            var typesWord = _md.Version.HasV108EmbeddedGenerics ? 4 : 6;
            var typesCount = WordToU64(words[baseIdx + typesWord]);
            var typesPtr = WordToU64(words[baseIdx + typesWord + 1]);
            if (typesCount == 0 || typesCount > 2_000_000 || !Reader.IsMapped(typesPtr)) continue;

            var gInstCount = WordToU64(words[baseIdx + 2]);
            if (gInstCount > 1_000_000) continue;
            if (!_md.Version.HasV108EmbeddedGenerics)
            {
                var gmCount = WordToU64(words[baseIdx + 4]);
                var mSpecCount = WordToU64(words[baseIdx + 8]);
                if (gmCount > 2_000_000 || mSpecCount > 2_000_000) continue;
            }
            if (!Reader.IsMapped(P(typesPtr))) continue;

            candidates.Add(start + (ulong)(baseIdx * Ptr));
        }
    }

    private void ParseMetadataRegistration(ulong va)
    {
        MetadataRegistrationVa = va;
        ulong F(int i) => At(va, i);

        var fieldOffIdx = MetadataFieldOffsetsWordIndex;
        var typeSizeIdx = fieldOffIdx + 2;

        GenericInstVas = ReadPointerArray(F(3), (int)Math.Min(F(2), 1_000_000));
        if (_md.Version.HasV108EmbeddedGenerics)
        {
            GenericMethodTable = _md.EmbeddedGenericMethodTable;
            TypeVas = ReadPointerArray(F(5), (int)Math.Min(F(4), 2_000_000));
            MethodSpecs = _md.EmbeddedMethodSpecs;
            MethodReferences = [];
        }
        else
        {
            // Il2CppGenericMethodIndices: {methodIndex, invokerIndex} in pre-v27 (v16-v24.4), + adjustorThunkIndex from 24.5/27.1.
            var gmStride = _md.Version.IsPreV27 ? 12 : 16;
            GenericMethodTable = ReadArray(F(5), (int)Math.Min(F(4), 2_000_000), gmStride,
                s => new Il2CppGenericMethodFunction(BitConverter.ToInt32(s), BitConverter.ToInt32(s[4..]), BitConverter.ToInt32(s[8..]), s.Length >= 16 ? BitConverter.ToInt32(s[12..]) : -1));
            TypeVas = ReadPointerArray(F(7), (int)Math.Min(F(6), 2_000_000));
            MethodSpecs = ReadArray(F(9), (int)Math.Min(F(8), 2_000_000), 12,
                s => new Il2CppMethodSpecRecord(BitConverter.ToInt32(s), BitConverter.ToInt32(s[4..]), BitConverter.ToInt32(s[8..])));
            MethodReferences = _md.Version.HasMethodReferencesInMetadataReg
                ? ReadInt32Array(F(11), (int)Math.Min(F(10), 2_000_000))
                : [];
        }

        var fieldOffCount = (int)Math.Min(F(fieldOffIdx), 2_000_000);
        var fieldOffVa = F(fieldOffIdx + 1);
        FieldOffsetsArePointers = DetermineFieldOffsetsArePointers(fieldOffCount, fieldOffVa);
        if (FieldOffsetsArePointers)
        {
            FieldOffsetTableVas = ReadPointerArray(fieldOffVa, fieldOffCount);
            _flatFieldOffsets = [];
        }
        else
        {
            FieldOffsetTableVas = [];
            _flatFieldOffsets = ReadInt32Array(fieldOffVa, fieldOffCount);
        }

        var typeSizeCount = (int)Math.Min(F(typeSizeIdx), 2_000_000);
        var typeSizeVa = F(typeSizeIdx + 1);
        TypeSizesArePointers = DetermineTypeSizesArePointers(typeSizeCount, typeSizeVa);
        if (TypeSizesArePointers)
        {
            TypeSizeTableVas = ReadPointerArray(typeSizeVa, typeSizeCount);
            _flatTypeInstanceSizes = [];
        }
        else
        {
            TypeSizeTableVas = [];
            _flatTypeInstanceSizes = ReadArray(typeSizeVa, typeSizeCount, 16, s => BitConverter.ToInt32(s));
        }
        for (var i = 0; i < TypeVas.Count; i++) _typeIndexByVa.TryAdd(TypeVas[i], i);

        _sink.Info(DiagnosticCategory.Registration, DiagnosticCodes.RegistrationFound,
            $"MetadataRegistration at RVA 0x{va - Reader.ImageBase:X}: {TypeVas.Count:N0} types, {GenericInstVas.Count:N0} generic insts, " +
            $"{MethodSpecs.Count:N0} method specs, {GenericMethodTable.Count:N0} generic method functions.",
            new() { Rva = va - Reader.ImageBase, VirtualAddress = va });
    }

    private bool DetermineTypeSizesArePointers(int count, ulong va)
    {
        if (_md.Version.Major >= 22) return true;
        if (_md.Version.Major <= 20) return false;
        if (count <= 0 || !Reader.IsMapped(va)) return true;

        var sample = ReadPointerArray(va, Math.Min(count, 64));
        var nonZero = 0;
        foreach (var entry in sample)
        {
            if (entry == 0) continue;
            nonZero++;
            if (!Reader.IsMapped(entry)) return false;
        }
        return nonZero > 0;
    }

    private bool DetermineFieldOffsetsArePointers(int count, ulong va)
    {
        if (_md.Version.Major >= 22) return true;
        if (_md.Version.Major <= 20) return false;
        // v21 transitioned between flat int32_t[] (fieldOffsetsCount == Fields.Length) and pointer tables.
        if (count == _md.Fields.Length && count != _md.TypeDefinitions.Length) return false;
        if (count <= 0 || !Reader.IsMapped(va)) return true;

        var sample = ReadPointerArray(va, Math.Min(count, 64));
        var nonZero = 0;
        foreach (var entry in sample)
        {
            if (entry == 0) continue;
            nonZero++;
            if (!Reader.IsMapped(entry)) return false;
        }
        return nonZero > 0 || count == _md.TypeDefinitions.Length;
    }

    private int[] ReadInt32Array(ulong va, int count)
    {
        if (count <= 0 || !Reader.IsMapped(va)) return [];
        var span = Reader.Slice(va, count * 4);
        return span.Length < count * 4 ? [] : MemoryMarshal.Cast<byte, int>(span).ToArray();
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
            var aligned = AlignUp(start);
            if (aligned >= end) continue;
            var found = Ptr == 8
                ? TryFindCodeRegistration(Words64(aligned, end), aligned, imageCount, imageNames, out var fieldVa, out var arrayVa)
                : TryFindCodeRegistration(Words32(aligned, end), aligned, (uint)imageCount, imageNames, out fieldVa, out arrayVa);
            if (!found) continue;

            // codeGenModulesCount index: v31+ = 15, v27-v29 = 13, v24.3-24.4 (genericAdjustorThunks) = 15, v24.2 = 14.
            var fieldIndex = _md.Version.Major >= 31 ? 15
                : _md.Version.IsV24 ? PickV24CodeRegFieldIndex(fieldVa)
                : 13;
            CodeRegistrationVa = fieldVa - (ulong)(fieldIndex * Ptr);
            _codeGenModulesArrayVa = arrayVa;
            ParseCodeRegistration(arrayVa, (int)imageCount);
            return;
        }
        _sink.Error(DiagnosticCategory.Registration, DiagnosticCodes.RegistrationNotFound,
            "Il2CppCodeRegistration not found; method addresses are unavailable.", new() { File = Reader.Image.Path });
    }

    private bool TryFindCodeRegistration<TWord>(
        ReadOnlySpan<TWord> words, ulong start, TWord target, HashSet<string> imageNames, out ulong fieldVa, out ulong arrayVa)
        where TWord : unmanaged, IEquatable<TWord>
    {
        var pos = 0;
        while (pos < words.Length - 2)
        {
            var hit = words[pos..].IndexOf(target);
            if (hit < 0) break;
            var i = pos + hit;
            pos = i + 1;
            if (i + 1 >= words.Length) break;
            var arr = WordToU64(words[i + 1]);
            if (!Reader.IsMapped(arr)) continue;
            var firstModule = P(arr);
            if (!Reader.IsMapped(firstModule)) continue;
            var name = Reader.ReadCString(P(firstModule));
            if (name is null || !imageNames.Contains(name)) continue;

            fieldVa = start + (ulong)(i * Ptr);
            arrayVa = arr;
            return true;
        }
        fieldVa = 0;
        arrayVa = 0;
        return false;
    }

    private int PickV24CodeRegFieldIndex(ulong fieldVa)
    {
        var cr14 = fieldVa - (ulong)(14 * Ptr);
        var cr15 = fieldVa - (ulong)(15 * Ptr);
        var valid15 = IsValidGmpLayout(cr15);
        var valid14 = IsValidGmpLayout(cr14);
        if (valid15 && !valid14) return 15;
        if (valid14 && !valid15) return 14;
        return _md.Version.Minor >= 3 ? 15 : 14;
    }

    private bool IsValidGmpLayout(ulong cr)
    {
        var cnt = At(cr, 2);
        var ptr = At(cr, 3);
        if (cnt == 0 || cnt >= 5_000_000 || !Reader.IsMapped(ptr)) return false;
        var need = cnt * (ulong)Ptr;
        return need <= int.MaxValue && Reader.Slice(ptr, (int)need).Length == (int)need;
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
            var v24Map = ReadMetadataUsagesV24();
            MetadataUsages = v24Map;
            EmitMetadataUsagesDiagnostic(v24Map, fromV24Table: true);
            return;
        }
        if (_md.Version.Major < 19)
        {
            MetadataUsages = new Dictionary<ulong, MetadataUsage>();
            EmitMetadataUsagesDiagnostic(MetadataUsages, fromV24Table: false);
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
            var aligned = AlignUp(start);
            if (aligned >= end) continue;
            if (Ptr == 8) ScanUsageWords(Words64(aligned, end), aligned, limits, map);
            else ScanUsageWords(Words32(aligned, end), aligned, limits, map);
        }
        MetadataUsages = map;
        EmitMetadataUsagesDiagnostic(map, fromV24Table: false);
    }

    private void ScanUsageWords<TWord>(ReadOnlySpan<TWord> words, ulong start, int[] limits, Dictionary<ulong, MetadataUsage> map)
        where TWord : unmanaged
    {
        var v108 = _md.Version.HasV108EmbeddedGenerics;
        for (var i = 0; i < words.Length; i++)
        {
            var v = WordToU64(words[i]);
            if ((v & 1) == 0 || v > uint.MaxValue) continue;
            var rawKind = (int)((v >> 29) & 7);
            if (rawKind == 0) continue;
            var kind = v108
                ? rawKind switch
                {
                    1 => MetadataUsageKind.TypeInfo,
                    2 => MetadataUsageKind.MethodDef,
                    3 => MetadataUsageKind.FieldInfo,
                    4 => MetadataUsageKind.StringLiteral,
                    5 => MetadataUsageKind.MethodRef,
                    6 => MetadataUsageKind.FieldRva,
                    _ => MetadataUsageKind.Invalid,
                }
                : (MetadataUsageKind)rawKind;
            if (kind == MetadataUsageKind.Invalid) continue;
            var index = (int)((v & 0x1FFFFFFE) >> 1);
            if (index >= limits[(int)kind]) continue;
            var va = start + (ulong)(i * Ptr);
            map[va] = new MetadataUsage(va, kind, index);
        }
    }

    private void EmitMetadataUsagesDiagnostic(IReadOnlyDictionary<ulong, MetadataUsage> map, bool fromV24Table)
    {
        if (fromV24Table)
        {
            _sink.Info(DiagnosticCategory.Registration, DiagnosticCodes.MetadataUsagesScanned,
                $"{map.Count:N0} runtime metadata globals recovered from v24 metadataUsages " +
                $"({map.Values.Count(u => u.Kind == MetadataUsageKind.StringLiteral):N0} string literals, {map.Values.Count(u => u.Kind == MetadataUsageKind.TypeInfo):N0} TypeInfo).");
            return;
        }
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
