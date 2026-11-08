using System.Buffers.Binary;
using System.Text;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Binary;

/// <summary>
/// Applies an ELF shared object's dynamic relocations for a load base of 0, in memory. RELATIVE writes the addend
/// (RELA) or keeps the in-place value (REL / RELR, already correct at base 0); absolute/GLOB_DAT relocations against
/// defined dynamic symbols write S + A. Android packed relocations (APS2) are reported, not decoded.
/// </summary>
internal sealed class ElfRelocator(ElfBinaryImage image, byte[] data)
{
    private const long DtNull = 0, DtStrTab = 5, DtSymTab = 6, DtRela = 7, DtRelaSz = 8, DtRel = 17, DtRelSz = 18, DtJmpRel = 23, DtPltRelSz = 2,
        DtPltRel = 20, DtRelrSz = 35, DtRelr = 36, DtAndroidRela = 0x60000011, DtAndroidRel = 0x6000000F, DtGnuHash = 0x6FFFFEF5, DtHash = 4;

    private readonly Dictionary<long, ulong> _dyn = [];
    private bool W64 => image.Is64Bit;
    private int Ptr => W64 ? 8 : 4;

    public int Apply(IDiagnosticSink sink)
    {
        if (image.Dynamic is not { } d) return 0;
        for (var p = d.Offset; p + 2 * Ptr <= d.Offset + (long)d.Size; p += 2 * Ptr)
        {
            var tag = (long)image.Word(p);
            if (tag == DtNull) break;
            _dyn.TryAdd(tag, image.Word(p + Ptr));
        }
        var n = 0;
        var relaLike = W64;   // ELF64 uses RELA on x86_64/AArch64; ELF32 (ARM/x86) uses REL
        if (_dyn.TryGetValue(DtRela, out var rela) && _dyn.TryGetValue(DtRelaSz, out var relaSz)) n += Table(rela, relaSz, true);
        if (_dyn.TryGetValue(DtRel, out var rel) && _dyn.TryGetValue(DtRelSz, out var relSz)) n += Table(rel, relSz, false);
        if (_dyn.TryGetValue(DtJmpRel, out var jmp) && _dyn.TryGetValue(DtPltRelSz, out var jmpSz))
            n += Table(jmp, jmpSz, _dyn.TryGetValue(DtPltRel, out var kind) ? kind == DtRela : relaLike);
        if (_dyn.TryGetValue(DtRelr, out var relr) && _dyn.TryGetValue(DtRelrSz, out var relrSz)) n += Relr(relr, relrSz);
        if (_dyn.ContainsKey(DtAndroidRela) || _dyn.ContainsKey(DtAndroidRel))
            sink.Warn(DiagnosticCategory.Binary, DiagnosticCodes.BinaryFormatUnsupported,
                "Android packed relocations (APS2) are present and not decoded; some data pointers may read as 0.", new() { File = image.Path });
        return n;
    }

    private bool ToOffset(ulong va, out long off)
    {
        foreach (var s in image.Segments)
            if (va >= s.VAddr && va < s.VAddr + s.FileSize) { off = s.Offset + (long)(va - s.VAddr); return true; }
        off = -1;
        return false;
    }

    private int Table(ulong va, ulong size, bool withAddend)
    {
        if (!ToOffset(va, out var off)) return 0;
        var ent = withAddend ? 3 * Ptr : 2 * Ptr;
        var applied = 0;
        for (var p = off; p + ent <= off + (long)size && p + ent <= data.Length; p += ent)
        {
            var where = image.Word(p);
            var info = image.Word(p + Ptr);
            var type = W64 ? (uint)(info & 0xFFFFFFFF) : (uint)(info & 0xFF);
            var sym = W64 ? (int)(info >> 32) : (int)(info >> 8);
            if (!ToOffset(where, out var target) || target + Ptr > data.Length) continue;
            long addend = withAddend ? (long)image.Word(p + 2 * Ptr) : (long)image.Word(target);
            ulong value;
            if (IsRelative(type)) value = (ulong)addend;
            else if (IsAbsolute(type) && SymbolValue(sym) is { } s and not 0) value = s + (ulong)(withAddend ? addend : 0);
            else continue;
            if (W64) BinaryPrimitives.WriteUInt64LittleEndian(data.AsSpan((int)target), value);
            else BinaryPrimitives.WriteUInt32LittleEndian(data.AsSpan((int)target), (uint)value);
            applied++;
        }
        return applied;
    }

    // RELATIVE: x86_64 8, AArch64 1027, ARM 23, i386 8.  ABS/GLOB_DAT/JUMP_SLOT: x86_64 1/6/7, AArch64 257/1025/1026, ARM 2/21/22, i386 1/6/7.
    private bool IsRelative(uint t) => image.Architecture switch { CpuArchitecture.Arm64 => t == 1027, CpuArchitecture.Arm => t == 23, _ => t == 8 };
    private bool IsAbsolute(uint t) => image.Architecture switch
    {
        CpuArchitecture.Arm64 => t is 257 or 1025 or 1026, CpuArchitecture.Arm => t is 2 or 21 or 22, _ => t is 1 or 6 or 7,
    };

    /// <summary>RELR: implicit-addend relative relocations; at load base 0 the stored values are already final.</summary>
    private int Relr(ulong va, ulong size) => ToOffset(va, out _) ? (int)(size / (ulong)Ptr) : 0;

    private ulong? SymbolValue(int index)
    {
        if (index == 0 || !_dyn.TryGetValue(DtSymTab, out var symtab) || !ToOffset(symtab, out var off)) return null;
        var p = off + (long)index * (W64 ? 24 : 16);
        if (p + (W64 ? 24 : 16) > data.Length) return null;
        return W64 ? image.Word(p + 8) : image.Word(p + 4);
    }

    /// <summary>Defined STT_FUNC dynamic symbols (exports such as il2cpp_object_new).</summary>
    public IEnumerable<(string Name, ulong Rva)> DynamicFunctionSymbols()
    {
        if (!_dyn.TryGetValue(DtSymTab, out var symtab) || !_dyn.TryGetValue(DtStrTab, out var strtab)
            || !ToOffset(symtab, out var symOff) || !ToOffset(strtab, out var strOff)) yield break;
        var entSize = W64 ? 24 : 16;
        // Prefer the .dynsym section size; otherwise the table usually ends where the string table starts.
        var dynsym = image.Sections.FirstOrDefault(s => s.Name == ".dynsym");
        var dynstr = image.Sections.FirstOrDefault(s => s.Name == ".dynstr");
        var end = dynsym is not null ? symOff + dynsym.RawSize : strOff > symOff ? strOff : symOff;
        var strEnd = dynstr is not null ? strOff + dynstr.RawSize : data.Length;
        for (var p = symOff + entSize; p + entSize <= end && p + entSize <= data.Length; p += entSize)
        {
            var name = BinaryPrimitives.ReadUInt32LittleEndian(data.AsSpan((int)p));
            byte info = data[p + (W64 ? 4 : 12)];
            ulong value = W64 ? image.Word(p + 8) : image.Word(p + 4);
            if ((info & 0xF) != 2 || value == 0) continue;   // STT_FUNC, defined
            var s = strOff + name;
            if (s < 0 || s >= strEnd || s >= data.Length) continue;
            var e = Array.IndexOf(data, (byte)0, (int)s);
            if (e > s) yield return (Encoding.ASCII.GetString(data, (int)s, e - (int)s), value & ~1UL);   // clear ARM Thumb bit
        }
    }
}
