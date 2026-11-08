using System.Runtime.InteropServices;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Registration;

/// <summary>
/// Metadata v24.0/24.1 (Unity 2018.x) registration. CodeRegistration has no codeGenModules; instead a global
/// <c>methodPointers</c> table is indexed by Il2CppMethodDefinition.methodIndex. Fields (64-bit):
/// [0] methodPointersCount [1] methodPointers [2..3] reversePInvoke [4] genericMethodPointersCount [5] genericMethodPointers
/// [6..7] invokers [8] customAttributeCount [9] customAttributeGenerators [10..11] unresolvedVirtualCalls [12..13] interopData.
/// The table is found by its count (max methodIndex + 1) and confirmed by customAttributeCount (= attributesInfo rows).
/// Per-image <see cref="CodeGenModuleRecord"/>s indexed by token RID - 1 are synthesized so the rest of the pipeline is unchanged.
/// </summary>
public sealed partial class Il2CppRegistration
{
    private bool LocateCodeRegistrationV24()
    {
        var methodCount = (ulong)(_md.Methods.Max(m => m.MethodIndex) + 1);
        var attrCount = (ulong)_md.AttributeTypeRanges.Length;
        if (methodCount == 0) return false;
        foreach (var (start, end, _) in Reader.DataRanges())
        {
            var words = Words(start, end);
            var pos = 0;
            while (pos < words.Length - 14)
            {
                var hit = words.AsSpan(pos).IndexOf(methodCount);
                if (hit < 0) break;
                var i = pos + hit;
                pos = i + 1;
                if (i + 13 >= words.Length || !Reader.IsMapped(words[i + 1])) continue;
                if (words[i + 8] != attrCount || words[i + 4] > 5_000_000) continue;
                CodeRegistrationVa = start + (ulong)(i * Ptr);
                var pointers = CodePointers(ReadPointerArray(words[i + 1], (int)methodCount));
                GenericMethodPointers = CodePointers(ReadPointerArray(words[i + 5], (int)words[i + 4]));
                CodeGenModules = SynthesizeModules(pointers);
                _sink.Info(DiagnosticCategory.Registration, DiagnosticCodes.RegistrationFound,
                    $"CodeRegistration (v{_md.Version}) at RVA 0x{CodeRegistrationVa - Reader.ImageBase:X}: {pointers.Length:N0} method pointers, " +
                    $"{GenericMethodPointers.Count:N0} generic method pointers.", new() { Rva = CodeRegistrationVa - Reader.ImageBase, VirtualAddress = CodeRegistrationVa });
                return true;
            }
        }
        return false;
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
        var count = At(MetadataRegistrationVa, 14);
        var table = ReadPointerArray(At(MetadataRegistrationVa, 15), (int)Math.Min(count, 5_000_000));
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
