using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Text;

namespace Il2CppStudio.Core.Binary;

/// <summary>
/// Zero-copy reader over the mapped binary addressed by virtual address (VA = image base + RVA).
/// Sections are cached as a small sorted array so translation is a branch-predictable linear scan.
/// </summary>
public sealed class VirtualReader
{
    private readonly IBinaryImage _image;
    private readonly ReadOnlyMemory<byte> _data;
    private readonly (ulong VaStart, ulong VaEnd, long FileOffset, bool Exec, bool Write)[] _ranges;

    public ulong ImageBase { get; }
    public int PointerSize { get; }

    public VirtualReader(IBinaryImage image)
    {
        _image = image;
        _data = image.Data;
        ImageBase = image.ImageBase;
        PointerSize = image.Is64Bit ? 8 : 4;
        _ranges = [.. image.Sections.Where(s => s.RawSize > 0)
            .Select(s => (image.ImageBase + s.Rva, image.ImageBase + s.Rva + (ulong)s.RawSize, s.FileOffset, s.IsExecutable, s.IsWritable))
            .OrderBy(r => r.Item1)];
    }

    public ReadOnlySpan<byte> File => _data.Span;
    public ReadOnlyMemory<byte> FileMemory => _data;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryToOffset(ulong va, out long offset)
    {
        foreach (var r in _ranges)
        {
            if (va >= r.VaStart && va < r.VaEnd) { offset = r.FileOffset + (long)(va - r.VaStart); return true; }
        }
        offset = -1;
        return false;
    }

    public bool IsMapped(ulong va) => TryToOffset(va, out _);

    public bool IsExecutable(ulong va)
    {
        foreach (var r in _ranges) if (va >= r.VaStart && va < r.VaEnd) return r.Exec;
        return false;
    }

    public bool IsWritableData(ulong va)
    {
        foreach (var r in _ranges) if (va >= r.VaStart && va < r.VaEnd) return r.Write && !r.Exec;
        return false;
    }

    public ReadOnlySpan<byte> Slice(ulong va, int length)
    {
        if (!TryToOffset(va, out var off) || off + length > _data.Length) return [];
        return _data.Span.Slice((int)off, length);
    }

    public ReadOnlyMemory<byte> SliceMemory(ulong va, int length)
    {
        if (!TryToOffset(va, out var off)) return ReadOnlyMemory<byte>.Empty;
        length = (int)Math.Min(length, _data.Length - off);
        return _data.Slice((int)off, length);
    }

    public bool TryReadU64(ulong va, out ulong value)
    {
        var s = Slice(va, 8);
        if (s.Length < 8) { value = 0; return false; }
        value = BinaryPrimitives.ReadUInt64LittleEndian(s);
        return true;
    }

    public ulong ReadU64(ulong va) => TryReadU64(va, out var v) ? v : 0;

    public uint ReadU32(ulong va)
    {
        var s = Slice(va, 4);
        return s.Length < 4 ? 0 : BinaryPrimitives.ReadUInt32LittleEndian(s);
    }

    public int ReadI32(ulong va) => unchecked((int)ReadU32(va));

    public ulong ReadPointer(ulong va) => PointerSize == 8 ? ReadU64(va) : ReadU32(va);

    public string? ReadCString(ulong va, int max = 512)
    {
        if (!TryToOffset(va, out var off)) return null;
        var span = _data.Span[(int)off..];
        if (span.Length > max) span = span[..max];
        var end = span.IndexOf((byte)0);
        return end < 0 ? null : Encoding.UTF8.GetString(span[..end]);
    }

    /// <summary>Enumerates the [VA start, VA end) ranges of initialized data sections (non-executable).</summary>
    public IEnumerable<(ulong Start, ulong End, bool Writable)> DataRanges() =>
        _ranges.Where(r => !r.Exec).Select(r => (r.VaStart, r.VaEnd, r.Write));

    public IBinaryImage Image => _image;
}
