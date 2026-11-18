using System.Buffers.Binary;
using System.IO.Compression;

namespace Il2CppStudio.Core.Binary;

/// <summary>
/// Reconstructs an ELF64 executable/shared-object image from an unencrypted PlayStation 4 / PlayStation 5
/// SELF container (<c>.prx</c>, <c>eboot.bin</c>; magic <c>4F 15 3D 1D</c> or <c>54 14 F5 EE</c>).
/// Supports both raw and zlib-compressed SELF segments and strips invalid post-dump section tables.
/// </summary>
public static class Ps4SelfUnpacker
{
    private const uint Ps4SelfMagic = 0x1D3D154F;
    private const uint Ps5SelfMagic = 0xEEF51454;
    private const uint ElfMagic = 0x464C457F;

    private readonly record struct ElfPhdr(uint Type, uint Flags, ulong Offset, ulong VAddr, ulong FileSize, ulong MemSize);
    private readonly record struct SelfEntry(ulong Flags, ulong Offset, ulong StoredSize, ulong UnpackedSize);

    public static bool IsSelf(ReadOnlySpan<byte> head)
    {
        if (head.Length < 4) return false;
        var magic = BinaryPrimitives.ReadUInt32LittleEndian(head);
        return magic is Ps4SelfMagic or Ps5SelfMagic;
    }

    public static byte[] UnpackToElf(ReadOnlySpan<byte> data)
    {
        if (!IsSelf(data) || data.Length < 32)
            throw new InvalidDataException("Not a valid PlayStation SELF container.");

        var headerSize = BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(12, 2));
        var entryCount = BinaryPrimitives.ReadUInt16LittleEndian(data.Slice(24, 2));
        var elfOffset = 32 + entryCount * 32;
        if (entryCount == 0 || elfOffset + 64 > headerSize || headerSize > data.Length)
            throw new InvalidDataException("Invalid SELF header or segment table bounds.");

        var elf = data.Slice(elfOffset);
        if (BinaryPrimitives.ReadUInt32LittleEndian(elf) != ElfMagic || elf[4] != 2 || elf[5] != 1)
            throw new InvalidDataException("SELF container does not embed a little-endian ELF64 image.");

        var phOffset = BinaryPrimitives.ReadUInt64LittleEndian(elf.Slice(32, 8));
        var phEntSize = BinaryPrimitives.ReadUInt16LittleEndian(elf.Slice(54, 2));
        var phCount = BinaryPrimitives.ReadUInt16LittleEndian(elf.Slice(56, 2));
        if (phEntSize != 56 || phCount == 0 || phOffset < 64 || (int)phOffset + phCount * 56 > headerSize - elfOffset)
            throw new InvalidDataException("Invalid embedded ELF64 program-header table in SELF.");

        var phdrs = new ElfPhdr[phCount];
        for (var i = 0; i < phCount; i++)
        {
            var p = elf.Slice((int)phOffset + i * 56, 56);
            phdrs[i] = new ElfPhdr(
                Type: BinaryPrimitives.ReadUInt32LittleEndian(p),
                Flags: BinaryPrimitives.ReadUInt32LittleEndian(p.Slice(4, 4)),
                Offset: BinaryPrimitives.ReadUInt64LittleEndian(p.Slice(8, 8)),
                VAddr: BinaryPrimitives.ReadUInt64LittleEndian(p.Slice(16, 8)),
                FileSize: BinaryPrimitives.ReadUInt64LittleEndian(p.Slice(32, 8)),
                MemSize: BinaryPrimitives.ReadUInt64LittleEndian(p.Slice(40, 8)));
        }

        var entries = new SelfEntry[entryCount];
        for (var i = 0; i < entryCount; i++)
        {
            var s = data.Slice(32 + i * 32, 32);
            entries[i] = new SelfEntry(
                Flags: BinaryPrimitives.ReadUInt64LittleEndian(s),
                Offset: BinaryPrimitives.ReadUInt64LittleEndian(s.Slice(8, 8)),
                StoredSize: BinaryPrimitives.ReadUInt64LittleEndian(s.Slice(16, 8)),
                UnpackedSize: BinaryPrimitives.ReadUInt64LittleEndian(s.Slice(24, 8)));
        }

        var elfHeaderSize = (int)phOffset + phCount * 56;
        ulong maxEnd = (ulong)elfHeaderSize;
        foreach (var e in entries)
        {
            if ((e.Flags & 2) != 0)
                throw new InvalidDataException("Encrypted PS4/PS5 SELF binary is not supported; provide a decrypted FSELF/SELF dump.");
            if ((e.Flags & 0x30000) != 0) continue; // Digest / block-table metadata entry
            var phIdx = (int)((e.Flags >> 20) & 0xFFF);
            if ((uint)phIdx >= (uint)phCount)
                throw new InvalidDataException($"SELF segment references out-of-range program header {phIdx}.");
            if (e.UnpackedSize > 0)
            {
                var end = phdrs[phIdx].Offset + Math.Max(e.UnpackedSize, phdrs[phIdx].FileSize);
                if (end > maxEnd) maxEnd = end;
            }
        }

        if (maxEnd > 1024UL * 1024 * 1024)
            throw new InvalidDataException($"Reconstructed ELF size out of range: 0x{maxEnd:X}.");

        var output = new byte[maxEnd];
        elf.Slice(0, elfHeaderSize).CopyTo(output);

        foreach (var e in entries)
        {
            if ((e.Flags & 0x30000) != 0 || e.UnpackedSize == 0) continue;
            var phIdx = (int)((e.Flags >> 20) & 0xFFF);
            var dstOff = (int)phdrs[phIdx].Offset;
            var dstLen = (int)e.UnpackedSize;
            if (e.Offset > (ulong)data.Length || e.StoredSize > (ulong)data.Length - e.Offset)
                throw new InvalidDataException($"SELF segment payload at 0x{e.Offset:X} exceeds file length.");

            var src = data.Slice((int)e.Offset, (int)e.StoredSize);
            if ((e.Flags & 8) != 0)
            {
                using var ms = new MemoryStream(src.ToArray());
                using var zs = new ZLibStream(ms, CompressionMode.Decompress);
                zs.ReadExactly(output.AsSpan(dstOff, dstLen));
            }
            else
            {
                src.Slice(0, Math.Min(src.Length, dstLen)).CopyTo(output.AsSpan(dstOff, dstLen));
            }
        }

        // Section headers are stripped during SELF packing; clear stale e_shoff/e_shnum fields.
        BinaryPrimitives.WriteUInt64LittleEndian(output.AsSpan(40, 8), 0);
        BinaryPrimitives.WriteUInt16LittleEndian(output.AsSpan(58, 2), 0);
        BinaryPrimitives.WriteUInt16LittleEndian(output.AsSpan(60, 2), 0);
        BinaryPrimitives.WriteUInt16LittleEndian(output.AsSpan(62, 2), 0);

        // Reset out-of-range empty segment offsets so downstream bounds checks stay consistent.
        for (var i = 0; i < phCount; i++)
        {
            if (phdrs[i].FileSize == 0 && phdrs[i].Offset > (ulong)output.Length)
                BinaryPrimitives.WriteUInt64LittleEndian(output.AsSpan((int)phOffset + i * 56 + 8, 8), 0);
        }

        return output;
    }
}
