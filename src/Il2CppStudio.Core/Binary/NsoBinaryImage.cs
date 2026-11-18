using System.Buffers.Binary;
using System.Text;
using Il2CppStudio.Core.Diagnostics;

namespace Il2CppStudio.Core.Binary;

/// <summary>
/// Nintendo Switch NSO0 executable image (AArch64): parses segment headers (.text/.rodata/.data/.bss),
/// decompresses LZ4-compressed segments via managed raw-block decoding, and applies MOD0 dynamic relocations.
/// </summary>
public sealed class NsoBinaryImage : IBinaryImage
{
    private readonly byte[] _image;
    private readonly List<BinarySection> _sections = [];
    private readonly List<(string Name, ulong Rva)> _symbols = [];

    public string Path { get; }
    public BinaryFormat Format => BinaryFormat.Nso;
    public CpuArchitecture Architecture => CpuArchitecture.Arm64;
    public bool Is64Bit => true;
    public ulong ImageBase => 0;
    public IReadOnlyList<BinarySection> Sections => _sections;
    public ReadOnlyMemory<byte> Data => _image;

    private NsoBinaryImage(string path, byte[] image) { Path = path; _image = image; }

    public static NsoBinaryImage Load(string path, ReadOnlyMemory<byte> rawBytes, IDiagnosticSink sink)
    {
        var span = rawBytes.Span;
        if (span.Length < 0x100 || span[0] != (byte)'N' || span[1] != (byte)'S' || span[2] != (byte)'O' || span[3] != (byte)'0')
            throw new InvalidDataException("Not a valid NSO0 binary (missing NSO0 magic).");

        var flags = BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(0x0C, 4));
        bool textComp = (flags & 1) != 0, roComp = (flags & 2) != 0, dataComp = (flags & 4) != 0;

        uint textFileOff = U32(span, 0x10), textMemOff = U32(span, 0x14), textSize = U32(span, 0x18);
        uint roFileOff = U32(span, 0x20), roMemOff = U32(span, 0x24), roSize = U32(span, 0x28);
        uint dataFileOff = U32(span, 0x30), dataMemOff = U32(span, 0x34), dataSize = U32(span, 0x38);
        uint bssSize = U32(span, 0x3C);

        uint textCompSize = U32(span, 0x60), roCompSize = U32(span, 0x64), dataCompSize = U32(span, 0x68);
        uint dynStrOff = U32(span, 0x90), dynStrSize = U32(span, 0x94);
        uint dynSymOff = U32(span, 0x98), dynSymSize = U32(span, 0x9C);

        ulong totalVirtual = (ulong)dataMemOff + dataSize + bssSize;
        if (totalVirtual == 0 || totalVirtual > 512UL * 1024 * 1024)
            throw new InvalidDataException($"NSO0 image virtual extent out of range: 0x{totalVirtual:X}.");

        var image = new byte[totalVirtual];
        LoadSegment(span, textFileOff, textComp ? textCompSize : textSize, textComp, image.AsSpan((int)textMemOff, (int)textSize));
        LoadSegment(span, roFileOff, roComp ? roCompSize : roSize, roComp, image.AsSpan((int)roMemOff, (int)roSize));
        LoadSegment(span, dataFileOff, dataComp ? dataCompSize : dataSize, dataComp, image.AsSpan((int)dataMemOff, (int)dataSize));

        var nso = new NsoBinaryImage(path, image);
        nso._sections.Add(new BinarySection(".text", textMemOff, textSize, textMemOff, textSize, true, false, true));
        nso._sections.Add(new BinarySection(".rodata", roMemOff, roSize, roMemOff, roSize, false, false, true));
        nso._sections.Add(new BinarySection(".data", dataMemOff, dataSize, dataMemOff, dataSize, false, true, true));
        if (bssSize > 0)
            nso._sections.Add(new BinarySection(".bss", dataMemOff + dataSize, bssSize, dataMemOff + dataSize, bssSize, false, true, true));

        var symsLoaded = nso.LoadDynSymbols(roMemOff, roSize, dynStrOff, dynStrSize, dynSymOff, dynSymSize);
        var relocs = nso.ApplyModRelocations(textMemOff, totalVirtual);

        sink.Info(DiagnosticCategory.Binary, DiagnosticCodes.BinaryFormatDetected,
            $"NSO0 Arm64: 4 segments (compressed flags=0x{flags:X}), {relocs:N0} MOD0 relocations, {symsLoaded:N0} symbols.",
            new() { File = path });
        return nso;
    }

    private static void LoadSegment(ReadOnlySpan<byte> file, uint fileOff, uint srcSize, bool compressed, Span<byte> dst)
    {
        if (srcSize == 0 || dst.IsEmpty) return;
        if ((ulong)fileOff + srcSize > (ulong)file.Length)
            throw new InvalidDataException($"NSO0 segment extends past file end (offset 0x{fileOff:X}, size 0x{srcSize:X}).");
        var src = file.Slice((int)fileOff, (int)srcSize);
        if (compressed) DecompressLz4Block(src, dst);
        else src[..Math.Min(src.Length, dst.Length)].CopyTo(dst);
    }

    /// <summary>Raw LZ4 block decoder used by NSO segments (literals + LE16 match offset + RLE overlap copy).</summary>
    public static void DecompressLz4Block(ReadOnlySpan<byte> src, Span<byte> dst)
    {
        int s = 0, d = 0;
        while (s < src.Length)
        {
            var token = src[s++];
            var litLen = token >> 4;
            if (litLen == 15)
            {
                byte b;
                do { if (s >= src.Length) break; b = src[s++]; litLen += b; } while (b == 255);
            }
            if (litLen > 0)
            {
                var copyLit = Math.Min(litLen, Math.Min(src.Length - s, dst.Length - d));
                src.Slice(s, copyLit).CopyTo(dst.Slice(d));
                s += litLen;
                d += copyLit;
            }
            if (s >= src.Length || d >= dst.Length) break;
            if (s + 2 > src.Length) break;
            var offset = src[s] | (src[s + 1] << 8);
            s += 2;
            if (offset == 0 || offset > d)
                throw new InvalidDataException($"Invalid LZ4 match offset {offset} at out={d}.");
            var matchLen = (token & 0x0F) + 4;
            if ((token & 0x0F) == 15)
            {
                byte b;
                do { if (s >= src.Length) break; b = src[s++]; matchLen += b; } while (b == 255);
            }
            var m = d - offset;
            for (var k = 0; k < matchLen && d < dst.Length; k++)
                dst[d++] = dst[m++];
        }
    }

    private int LoadDynSymbols(uint roMemOff, uint roSize, uint dynStrOff, uint dynStrSize, uint dynSymOff, uint dynSymSize)
    {
        if (dynStrSize == 0 || dynSymSize < 24) return 0;
        var strBase = (ulong)roMemOff + dynStrOff;
        var symBase = (ulong)roMemOff + dynSymOff;
        var limit = (ulong)roMemOff + roSize;
        if (symBase + dynSymSize > limit || strBase + dynStrSize > limit) return 0;

        var count = (int)(dynSymSize / 24);
        var strSpan = _image.AsSpan((int)strBase, (int)dynStrSize);
        for (var i = 0; i < count; i++)
        {
            var p = (int)symBase + i * 24;
            var stName = BinaryPrimitives.ReadUInt32LittleEndian(_image.AsSpan(p, 4));
            var stValue = BinaryPrimitives.ReadUInt64LittleEndian(_image.AsSpan(p + 8, 8));
            if (stValue == 0 || stName >= dynStrSize) continue;
            var nameEnd = strSpan[(int)stName..].IndexOf((byte)0);
            if (nameEnd <= 0) continue;
            var name = Encoding.UTF8.GetString(strSpan.Slice((int)stName, nameEnd));
            _symbols.Add((name, stValue));
        }
        return _symbols.Count;
    }

    private int ApplyModRelocations(uint textMemOff, ulong imageSize)
    {
        if ((ulong)textMemOff + 8 > imageSize) return 0;
        var modRel = BinaryPrimitives.ReadUInt32LittleEndian(_image.AsSpan((int)textMemOff + 4, 4));
        var modOff = (ulong)textMemOff + modRel;
        if (modOff + 28 > imageSize) return 0;
        var span = _image.AsSpan((int)modOff, 28);
        if (span[0] != (byte)'M' || span[1] != (byte)'O' || span[2] != (byte)'D' || span[3] != (byte)'0')
            return 0;

        var dynRel = BinaryPrimitives.ReadInt32LittleEndian(span.Slice(4, 4));
        var dynOff = (long)modOff + dynRel;
        if (dynOff < 0 || (ulong)dynOff + 16 > imageSize) return 0;

        ulong relaOff = 0, relaSz = 0, symTabOff = 0;
        for (var pos = (ulong)dynOff; pos + 16 <= imageSize; pos += 16)
        {
            var tag = BinaryPrimitives.ReadInt64LittleEndian(_image.AsSpan((int)pos, 8));
            var val = BinaryPrimitives.ReadUInt64LittleEndian(_image.AsSpan((int)pos + 8, 8));
            if (tag == 0) break;
            if (tag == 7) relaOff = val;
            else if (tag == 8) relaSz = val;
            else if (tag == 6) symTabOff = val;
        }
        if (relaOff == 0 || relaSz < 24 || relaOff + relaSz > imageSize) return 0;

        var applied = 0;
        for (var pos = relaOff; pos + 24 <= relaOff + relaSz; pos += 24)
        {
            var rOffset = BinaryPrimitives.ReadUInt64LittleEndian(_image.AsSpan((int)pos, 8));
            var rInfo = BinaryPrimitives.ReadUInt64LittleEndian(_image.AsSpan((int)pos + 8, 8));
            var rAddend = BinaryPrimitives.ReadInt64LittleEndian(_image.AsSpan((int)pos + 16, 8));
            if (rOffset + 8 > imageSize) continue;

            var rType = (uint)rInfo;
            var rSym = (uint)(rInfo >> 32);
            if (rType == 1027) // R_AARCH64_RELATIVE
            {
                BinaryPrimitives.WriteUInt64LittleEndian(_image.AsSpan((int)rOffset, 8), unchecked((ulong)rAddend));
                applied++;
            }
            else if (rType == 257 && symTabOff != 0 && symTabOff + (ulong)rSym * 24 + 16 <= imageSize) // R_AARCH64_ABS64
            {
                var stVal = BinaryPrimitives.ReadUInt64LittleEndian(_image.AsSpan((int)(symTabOff + (ulong)rSym * 24 + 8), 8));
                BinaryPrimitives.WriteUInt64LittleEndian(_image.AsSpan((int)rOffset, 8), stVal + unchecked((ulong)rAddend));
                applied++;
            }
        }
        return applied;
    }

    private static uint U32(ReadOnlySpan<byte> s, int off) => BinaryPrimitives.ReadUInt32LittleEndian(s.Slice(off, 4));

    public bool TryRvaToFileOffset(ulong rva, out long fileOffset) => SectionMath.TryRvaToOffset(_sections, rva, out fileOffset);
    public bool TryFileOffsetToRva(long fileOffset, out ulong rva) => SectionMath.TryOffsetToRva(_sections, fileOffset, out rva);
    public IReadOnlyList<(string Name, ulong Rva)> GetSymbols() => _symbols;
    public void Dispose() { }
}
