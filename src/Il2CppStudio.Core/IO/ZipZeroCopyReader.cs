using System.Buffers.Binary;
using System.IO.Compression;
using System.Text;

namespace Il2CppStudio.Core.IO;

/// <summary>
/// Entry descriptor parsed directly from a ZIP central directory without extracting any payload to disk.
/// </summary>
public readonly record struct ZipZeroCopyEntry(
    string FullName,
    ushort CompressionMethod,
    long CompressedSize,
    long UncompressedSize,
    long LocalHeaderOffset);

/// <summary>
/// Pure-C# span reader over a memory-mapped ZIP archive (.apk / .xapk / .ipa).
/// Uncompressed (<c>Stored</c>, method 0) entries are returned as zero-copy <see cref="MappedSlice"/> views directly
/// backed by the OS page cache; <c>Deflate</c> (method 8) entries are decompressed directly in memory.
/// </summary>
public sealed class ZipZeroCopyReader : IDisposable
{
    private readonly SharedMappedFile _shared;
    private readonly int _sliceOffset;
    private readonly int _sliceLength;
    private readonly List<ZipZeroCopyEntry> _entries;

    public IReadOnlyList<ZipZeroCopyEntry> Entries => _entries;

    private ZipZeroCopyReader(SharedMappedFile shared, int sliceOffset, int sliceLength, List<ZipZeroCopyEntry> entries)
    {
        if (!shared.TryAddRef())
            throw new ObjectDisposedException(nameof(SharedMappedFile));
        _shared = shared;
        _sliceOffset = sliceOffset;
        _sliceLength = sliceLength;
        _entries = entries;
    }

    public static ZipZeroCopyReader Open(string path)
    {
        var mapped = MappedFile.Open(path);
        var shared = new SharedMappedFile(mapped);
        try
        {
            var entries = ParseCentralDirectory(shared.Span);
            return new ZipZeroCopyReader(shared, 0, shared.Length, entries);
        }
        finally
        {
            shared.Dispose();
        }
    }

    public static ZipZeroCopyReader OpenSlice(SharedMappedFile shared, int offset, int length)
    {
        var entries = ParseCentralDirectory(shared.Span.Slice(offset, length));
        return new ZipZeroCopyReader(shared, offset, length, entries);
    }

    public ZipZeroCopyReader? TryOpenNestedStored(ZipZeroCopyEntry entry)
    {
        if (entry.CompressionMethod != 0) return null;
        if (!TryResolveDataExtent(entry, out var relOff, out var len)) return null;
        var entries = ParseCentralDirectory(_shared.Span.Slice(_sliceOffset + relOff, len));
        return new ZipZeroCopyReader(_shared, _sliceOffset + relOff, len, entries);
    }

    public VirtualFileBuffer OpenBuffer(ZipZeroCopyEntry entry)
    {
        if (!TryResolveDataExtent(entry, out var relOff, out var dataLen))
            throw new InvalidDataException($"ZIP entry '{entry.FullName}' exceeds archive bounds.");

        var absOffset = _sliceOffset + relOff;
        if (entry.CompressionMethod == 0)
            return VirtualFileBuffer.FromMappedSlice(entry.FullName, _shared, absOffset, dataLen);

        if (entry.CompressionMethod == 8)
        {
            if (entry.UncompressedSize > int.MaxValue)
                throw new InvalidDataException($"ZIP entry '{entry.FullName}' exceeds 2 GiB.");
            var expected = (int)entry.UncompressedSize;
            var raw = _shared.Span.Slice(absOffset, dataLen);
            unsafe
            {
                fixed (byte* ptr = raw)
                {
                    using var ums = new UnmanagedMemoryStream(ptr, dataLen);
                    using var ds = new DeflateStream(ums, CompressionMode.Decompress);
                    if (expected > 0)
                    {
                        var dst = GC.AllocateUninitializedArray<byte>(expected);
                        var total = 0;
                        while (total < expected)
                        {
                            var read = ds.Read(dst, total, expected - total);
                            if (read == 0) break;
                            total += read;
                        }
                        if (total == expected)
                            return VirtualFileBuffer.FromArray(entry.FullName, dst);
                        var trimmed = new byte[total];
                        Buffer.BlockCopy(dst, 0, trimmed, 0, total);
                        return VirtualFileBuffer.FromArray(entry.FullName, trimmed);
                    }
                    using var ms = new MemoryStream();
                    ds.CopyTo(ms);
                    return VirtualFileBuffer.FromArray(entry.FullName, ms.ToArray());
                }
            }
        }

        throw new NotSupportedException($"Unsupported ZIP compression method {entry.CompressionMethod} for '{entry.FullName}'.");
    }

    private bool TryResolveDataExtent(ZipZeroCopyEntry entry, out int dataOffset, out int dataLength)
    {
        dataOffset = 0;
        dataLength = 0;
        var span = _shared.Span.Slice(_sliceOffset, _sliceLength);
        if (entry.LocalHeaderOffset < 0 || entry.LocalHeaderOffset + 30 > span.Length)
            return false;
        var lfh = (int)entry.LocalHeaderOffset;
        if (BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(lfh, 4)) != 0x04034B50)
            return false;
        var nameLen = BinaryPrimitives.ReadUInt16LittleEndian(span.Slice(lfh + 26, 2));
        var extraLen = BinaryPrimitives.ReadUInt16LittleEndian(span.Slice(lfh + 28, 2));
        var start = (long)lfh + 30 + nameLen + extraLen;
        var size = entry.CompressionMethod == 0 ? entry.UncompressedSize : entry.CompressedSize;
        if (start < 0 || size < 0 || start + size > span.Length || size > int.MaxValue)
            return false;
        dataOffset = (int)start;
        dataLength = (int)size;
        return true;
    }

    private static List<ZipZeroCopyEntry> ParseCentralDirectory(ReadOnlySpan<byte> span)
    {
        if (span.Length < 22)
            throw new InvalidDataException("Archive too small to be a ZIP file.");

        var minSearch = Math.Max(0, span.Length - (22 + 65535));
        var eocdOffset = -1;
        for (var pos = span.Length - 22; pos >= minSearch; pos--)
        {
            if (span[pos] == 0x50 && span[pos + 1] == 0x4B && span[pos + 2] == 0x05 && span[pos + 3] == 0x06)
            {
                var commentLen = BinaryPrimitives.ReadUInt16LittleEndian(span.Slice(pos + 20, 2));
                if (pos + 22 + commentLen == span.Length)
                {
                    eocdOffset = pos;
                    break;
                }
            }
        }
        if (eocdOffset < 0)
            throw new InvalidDataException("ZIP End of Central Directory record not found.");

        long totalEntries = BinaryPrimitives.ReadUInt16LittleEndian(span.Slice(eocdOffset + 10, 2));
        long cdSize = BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(eocdOffset + 12, 4));
        long cdOffset = BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(eocdOffset + 16, 4));

        // Check ZIP64 EOCD locator right before EOCD (20 bytes)
        if (eocdOffset >= 20 && BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(eocdOffset - 20, 4)) == 0x07064B50)
        {
            var eocd64Off = BinaryPrimitives.ReadInt64LittleEndian(span.Slice(eocdOffset - 12, 8));
            if (eocd64Off >= 0 && eocd64Off + 56 <= eocdOffset
                && BinaryPrimitives.ReadUInt32LittleEndian(span.Slice((int)eocd64Off, 4)) == 0x06064B50)
            {
                var off = (int)eocd64Off;
                if (totalEntries == 0xFFFF) totalEntries = BinaryPrimitives.ReadInt64LittleEndian(span.Slice(off + 32, 8));
                if (cdSize == 0xFFFFFFFF) cdSize = BinaryPrimitives.ReadInt64LittleEndian(span.Slice(off + 40, 8));
                if (cdOffset == 0xFFFFFFFF) cdOffset = BinaryPrimitives.ReadInt64LittleEndian(span.Slice(off + 48, 8));
            }
        }

        if (cdOffset < 0 || cdSize < 0 || cdOffset + cdSize > eocdOffset)
            throw new InvalidDataException("ZIP Central Directory is out of archive bounds.");

        var list = new List<ZipZeroCopyEntry>((int)Math.Min(totalEntries, 65536));
        var cursor = (int)cdOffset;
        var cdEnd = (int)(cdOffset + cdSize);

        while (cursor + 46 <= cdEnd)
        {
            if (BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(cursor, 4)) != 0x02014B50)
                break;

            var flags = BinaryPrimitives.ReadUInt16LittleEndian(span.Slice(cursor + 8, 2));
            var method = BinaryPrimitives.ReadUInt16LittleEndian(span.Slice(cursor + 10, 2));
            long compSize = BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(cursor + 20, 4));
            long uncompSize = BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(cursor + 24, 4));
            var nameLen = BinaryPrimitives.ReadUInt16LittleEndian(span.Slice(cursor + 28, 2));
            var extraLen = BinaryPrimitives.ReadUInt16LittleEndian(span.Slice(cursor + 30, 2));
            var commentLen = BinaryPrimitives.ReadUInt16LittleEndian(span.Slice(cursor + 32, 2));
            long localOffset = BinaryPrimitives.ReadUInt32LittleEndian(span.Slice(cursor + 42, 4));

            var totalRecLen = 46 + nameLen + extraLen + commentLen;
            if (cursor + totalRecLen > cdEnd) break;

            var nameSpan = span.Slice(cursor + 46, nameLen);
            var utf8 = (flags & 0x0800) != 0;
            var name = utf8 ? Encoding.UTF8.GetString(nameSpan) : Encoding.Latin1.GetString(nameSpan);

            if ((compSize == 0xFFFFFFFF || uncompSize == 0xFFFFFFFF || localOffset == 0xFFFFFFFF) && extraLen >= 4)
            {
                var extraSpan = span.Slice(cursor + 46 + nameLen, extraLen);
                ReadZip64Extra(extraSpan, ref uncompSize, ref compSize, ref localOffset);
            }

            list.Add(new ZipZeroCopyEntry(name, method, compSize, uncompSize, localOffset));
            cursor += totalRecLen;
        }

        return list;
    }

    private static void ReadZip64Extra(ReadOnlySpan<byte> extra, ref long uncompSize, ref long compSize, ref long localOffset)
    {
        var p = 0;
        while (p + 4 <= extra.Length)
        {
            var id = BinaryPrimitives.ReadUInt16LittleEndian(extra.Slice(p, 2));
            var size = BinaryPrimitives.ReadUInt16LittleEndian(extra.Slice(p + 2, 2));
            p += 4;
            if (p + size > extra.Length) break;
            if (id == 0x0001)
            {
                var field = extra.Slice(p, size);
                var fp = 0;
                if (uncompSize == 0xFFFFFFFF && fp + 8 <= field.Length)
                {
                    uncompSize = BinaryPrimitives.ReadInt64LittleEndian(field.Slice(fp, 8));
                    fp += 8;
                }
                if (compSize == 0xFFFFFFFF && fp + 8 <= field.Length)
                {
                    compSize = BinaryPrimitives.ReadInt64LittleEndian(field.Slice(fp, 8));
                    fp += 8;
                }
                if (localOffset == 0xFFFFFFFF && fp + 8 <= field.Length)
                    localOffset = BinaryPrimitives.ReadInt64LittleEndian(field.Slice(fp, 8));
                break;
            }
            p += size;
        }
    }

    public void Dispose() => _shared.Dispose();
}
