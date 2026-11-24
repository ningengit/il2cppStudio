using Il2CppStudio.Core.Diagnostics;
using Il2CppStudio.Core.Metadata;

namespace Il2CppStudio.Core.Registration;

/// <summary>
/// Pre-v24.2 metadata registration (v16–v24.1, Unity 5.3–2018.4). CodeRegistration has no codeGenModules; instead a
/// global <c>methodPointers</c> table is indexed by Il2CppMethodDefinition.methodIndex. Word slots vary by version:
/// v16-20 (14 words): gmp@8, invokers@10, attr@12;
/// v21 (18 words): gmp@10, invokers@12, attr@14;
/// v22 (20 words): gmp@10, invokers@12, attr@14;
/// v23-24.1 (14 words): gmp@4, invokers@6, attr@8.
/// Per-image <see cref="CodeGenModuleRecord"/>s indexed by token RID - 1 are synthesized so the rest of the pipeline is unchanged.
/// </summary>
public sealed partial class Il2CppRegistration
{
    private static (int GmpIndex, int InvokerIndex, int AttrIndex, int TotalWords) GetPreV242CodeRegLayout(MetadataVersion v) =>
        v.Major switch
        {
            <= 20 => (8, 10, 12, 14),
            21 => (10, 12, 14, 18),
            22 => (10, 12, 14, 20),
            _ => (4, 6, 8, 14),
        };

    private bool LocateCodeRegistrationV24()
    {
        var methodCount = _md.Methods.Length == 0 ? 0UL : (ulong)(_md.Methods.Max(m => m.MethodIndex) + 1);
        var attrCount = (ulong)_md.AttributeTypeRanges.Length;
        if (methodCount == 0) return false;
        foreach (var (start, end, _) in Reader.DataRanges())
        {
            var aligned = AlignUp(start);
            if (aligned >= end) continue;
            var va = Ptr == 8
                ? FindCodeRegistrationV24(Words64(aligned, end), aligned, methodCount, attrCount)
                : FindCodeRegistrationV24(Words32(aligned, end), aligned, (uint)methodCount, attrCount);
            if (va != 0)
                return ParseCodeRegistrationV24(va, methodCount);
        }
        return false;
    }

    private ulong FindCodeRegistrationV24<TWord>(ReadOnlySpan<TWord> words, ulong start, TWord target, ulong attrCount)
        where TWord : unmanaged, IEquatable<TWord>
    {
        var (gmpIdx, invIdx, attrIdx, totalWords) = GetPreV242CodeRegLayout(_md.Version);
        var methodCount = WordToU64(target);
        var metaRegBytes = (ulong)((MetadataFieldOffsetsWordIndex + (_md.Version.HasMetadataUsageTables ? 6 : 4)) * Ptr);
        var pos = 0;
        while (pos <= words.Length - totalWords)
        {
            var hit = words[pos..].IndexOf(target);
            if (hit < 0) break;
            var i = pos + hit;
            pos = i + 1;
            if (i + totalWords > words.Length) break;

            var candidateVa = start + (ulong)(i * Ptr);
            if (MetadataRegistrationVa != 0 && candidateVa >= MetadataRegistrationVa && candidateVa < MetadataRegistrationVa + metaRegBytes)
                continue;

            var ptrsVa = WordToU64(words[i + 1]);
            if (!Reader.IsMapped(ptrsVa)) continue;

            var gmpCount = WordToU64(words[i + gmpIdx]);
            if (gmpCount > 5_000_000 || (gmpCount > 0 && !Reader.IsMapped(WordToU64(words[i + gmpIdx + 1])))) continue;

            var invCount = WordToU64(words[i + invIdx]);
            if (invCount > 5_000_000 || (invCount > 0 && !Reader.IsMapped(WordToU64(words[i + invIdx + 1])))) continue;

            var candAttrCount = WordToU64(words[i + attrIdx]);
            if (_md.Version.HasCustomAttributeTables)
            {
                if (candAttrCount != attrCount) continue;
            }
            else if (candAttrCount > 5_000_000)
            {
                continue;
            }

            if (!HasValidMethodPointerSample(ptrsVa, methodCount)) continue;
            return start + (ulong)(i * Ptr);
        }
        return 0;
    }

    private bool HasValidMethodPointerSample(ulong ptrsVa, ulong methodCount)
    {
        var sampleCount = (int)Math.Min(methodCount, 256UL);
        var ptrs = ReadPointerArray(ptrsVa, sampleCount);
        if (ptrs.Length != sampleCount) return false;

        if (Reader.Image is Binary.AsmJsBinaryImage asmJs)
        {
            var checkedSlots = 0;
            foreach (var raw in ptrs)
            {
                if (raw == 0) continue;
                if (raw > (ulong)asmJs.MaxTableSlot) return false;
                if (++checkedSlots >= 8) break;
            }
            return checkedSlots > 0;
        }

        var requireExec = Reader.Image.Architecture != Binary.CpuArchitecture.Wasm32
            && Reader.Image.Sections.Any(s => s.IsExecutable);
        var checkedNonZero = 0;
        foreach (var raw in ptrs)
        {
            if (raw == 0) continue;
            var addr = Reader.Image.Architecture == Binary.CpuArchitecture.Arm ? (raw & ~1UL) : raw;
            if (!Reader.IsMapped(addr)) return false;
            if (requireExec)
            {
                var rva = addr - Reader.ImageBase;
                if (Reader.Image.FindSectionByRva(rva) is not { IsExecutable: true })
                    return false;
            }
            if (++checkedNonZero >= 8) break;
        }
        return checkedNonZero > 0 || methodCount == 0;
    }

    private bool ParseCodeRegistrationV24(ulong codeRegVa, ulong methodCount)
    {
        var (gmpIdx, _, _, _) = GetPreV242CodeRegLayout(_md.Version);
        var ptrsVa = At(codeRegVa, 1);
        var gmpCount = At(codeRegVa, gmpIdx);
        var gmpVa = At(codeRegVa, gmpIdx + 1);
        if (!Reader.IsMapped(ptrsVa) || gmpCount > 5_000_000) return false;

        CodeRegistrationVa = codeRegVa;
        var pointers = CodePointers(ReadPointerArray(ptrsVa, (int)methodCount));
        GlobalMethodPointers = pointers;
        GenericMethodPointers = gmpCount > 0 ? CodePointers(ReadPointerArray(gmpVa, (int)gmpCount)) : [];
        CodeGenModules = SynthesizeModules(pointers);
        _sink.Info(DiagnosticCategory.Registration, DiagnosticCodes.RegistrationFound,
            $"CodeRegistration (v{_md.Version}) at RVA 0x{CodeRegistrationVa - Reader.ImageBase:X}: {pointers.Length:N0} method pointers, " +
            $"{GenericMethodPointers.Count:N0} generic method pointers.", new() { Rva = CodeRegistrationVa - Reader.ImageBase, VirtualAddress = CodeRegistrationVa });
        return true;
    }

    /// <summary>One module per image; MethodPointers[rid - 1] = methodPointers[methodDef.MethodIndex].</summary>
    private List<CodeGenModuleRecord> SynthesizeModules(ulong[] pointers)
    {
        var modules = new List<CodeGenModuleRecord>(_md.Images.Length);
        foreach (var img in _md.Images)
        {
            var slots = new List<ulong>();
            for (var t = img.TypeStart; t < img.TypeStart + img.TypeCount && t < _md.TypeDefinitions.Length; t++)
            {
                var td = _md.TypeDefinitions[t];
                for (var m = td.MethodStart; m < td.MethodStart + td.MethodCount && m < _md.Methods.Length; m++)
                {
                    var md = _md.Methods[m];
                    var rid = (int)(md.Token & 0x00FFFFFF);
                    while (slots.Count < rid) slots.Add(0);
                    if (rid > 0 && (uint)md.MethodIndex < (uint)pointers.Length) slots[rid - 1] = pointers[md.MethodIndex];
                }
            }
            modules.Add(new CodeGenModuleRecord(0, _md.GetString(img.NameIndex), slots));
        }
        return modules;
    }

    /// <summary>
    /// v24 runtime-metadata globals: MetadataRegistration.metadataUsages[dest] is the address of the global for each
    /// (dest, encodedSource) pair; source = (kind &lt;&lt; 29) | index (no low tag bit before v27).
    /// </summary>
    private Dictionary<ulong, MetadataUsage> ReadMetadataUsagesV24()
    {
        var map = new Dictionary<ulong, MetadataUsage>();
        if (MetadataRegistrationVa == 0) return map;
        var tableVa = At(MetadataRegistrationVa, 15);
        var needed = At(MetadataRegistrationVa, 14);
        foreach (var pair in _md.MetadataUsagePairs)
            if (pair.DestinationIndex + 1UL > needed) needed = pair.DestinationIndex + 1UL;
        var maxCount = (int)Math.Min(needed, 5_000_000UL);
        var avail = Reader.IsMapped(tableVa) ? Reader.Slice(tableVa, maxCount * Ptr).Length / Ptr : 0;
        var table = ReadPointerArray(tableVa, Math.Min(maxCount, avail));
        foreach (var pair in _md.MetadataUsagePairs)
        {
            if (pair.DestinationIndex >= table.Length) continue;
            var kind = (MetadataUsageKind)((pair.EncodedSourceIndex & 0xE0000000) >> 29);
            var index = (int)(pair.EncodedSourceIndex & 0x1FFFFFFF);
            var va = table[pair.DestinationIndex];
            if (kind != MetadataUsageKind.Invalid && va != 0) map[va] = new MetadataUsage(va, kind, index);
        }
        return map;
    }

    /// <summary>v24 Il2CppType bits (num_mods:6, byref:1, pinned:1) re-encoded into the v27+ layout used by <see cref="Il2CppTypeRecord"/>.</summary>
    private static uint NormalizeTypeBitsV24(uint bits)
    {
        var numMods = (bits >> 24) & 0x3F;
        var byref = (bits >> 30) & 1;
        var pinned = (bits >> 31) & 1;
        return (bits & 0x00FFFFFF) | ((numMods & 0x1F) << 24) | (byref << 29) | (pinned << 30);
    }
}
