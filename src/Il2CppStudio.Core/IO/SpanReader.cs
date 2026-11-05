using System.Buffers.Binary;
using System.Text;

namespace Il2CppStudio.Core.IO;

/// <summary>Thrown when a read would cross the end of the underlying data.</summary>
public sealed class DataBoundsException(long position, int requested, long length)
    : Exception($"Read of {requested} byte(s) at 0x{position:X} exceeds data length 0x{length:X}.")
{
    public long Position { get; } = position;
    public int Requested { get; } = requested;
    public long DataLength { get; } = length;
}

/// <summary>
/// Little-endian, bounds-checked, allocation-free reader over a span. Every read is checked; there is
/// no way to read outside the span, so corrupt or hostile files produce a <see cref="DataBoundsException"/>
/// that callers turn into a diagnostic instead of undefined behaviour.
/// </summary>
public ref struct SpanReader
{
    private readonly ReadOnlySpan<byte> _data;

    public int Position;

    public SpanReader(ReadOnlySpan<byte> data, int position = 0)
    {
        _data = data;
        Position = position;
    }

    public readonly int Length => _data.Length;
    public readonly int Remaining => _data.Length - Position;

    public ReadOnlySpan<byte> ReadBytes(int count)
    {
        if (count < 0 || (uint)count > (uint)Remaining) throw new DataBoundsException(Position, count, _data.Length);
        var slice = _data.Slice(Position, count);
        Position += count;
        return slice;
    }

    public void Skip(int count) => ReadBytes(count);

    public byte ReadByte() => ReadBytes(1)[0];
    public short ReadInt16() => BinaryPrimitives.ReadInt16LittleEndian(ReadBytes(2));
    public ushort ReadUInt16() => BinaryPrimitives.ReadUInt16LittleEndian(ReadBytes(2));
    public int ReadInt32() => BinaryPrimitives.ReadInt32LittleEndian(ReadBytes(4));
    public uint ReadUInt32() => BinaryPrimitives.ReadUInt32LittleEndian(ReadBytes(4));
    public long ReadInt64() => BinaryPrimitives.ReadInt64LittleEndian(ReadBytes(8));
    public ulong ReadUInt64() => BinaryPrimitives.ReadUInt64LittleEndian(ReadBytes(8));

    /// <summary>Reads a NUL-terminated UTF-8 string (terminator consumed).</summary>
    public string ReadNullTerminatedUtf8(int maxLength = 1 << 20)
    {
        var rest = _data[Position..];
        var end = rest.IndexOf((byte)0);
        if (end < 0 || end > maxLength) throw new DataBoundsException(Position, Math.Min(rest.Length + 1, maxLength), _data.Length);
        var s = Encoding.UTF8.GetString(rest[..end]);
        Position += end + 1;
        return s;
    }

    /// <summary>Reads a .NET 7-bit encoded length prefixed UTF-8 string (BinaryWriter format).</summary>
    public string Read7BitPrefixedUtf8()
    {
        var len = Read7BitEncodedInt();
        return Encoding.UTF8.GetString(ReadBytes(len));
    }

    public int Read7BitEncodedInt()
    {
        uint result = 0;
        for (var shift = 0; shift < 35; shift += 7)
        {
            var b = ReadByte();
            result |= (uint)(b & 0x7F) << shift;
            if ((b & 0x80) == 0) return (int)result;
        }
        throw new FormatException($"Malformed 7-bit encoded integer at 0x{Position:X}.");
    }

    public void Align(int alignment)
    {
        var mis = Position % alignment;
        if (mis != 0) Skip(alignment - mis);
    }
}

public static class SpanExtensions
{
    /// <summary>Bounds-checked slice that returns false instead of throwing.</summary>
    public static bool TrySlice(this ReadOnlySpan<byte> span, long offset, long length, out ReadOnlySpan<byte> slice)
    {
        if (offset < 0 || length < 0 || offset > span.Length || length > span.Length - offset)
        {
            slice = default;
            return false;
        }
        slice = span.Slice((int)offset, (int)length);
        return true;
    }

    public static string ReadNullTerminatedAscii(this ReadOnlySpan<byte> span, int offset)
    {
        if ((uint)offset >= (uint)span.Length) return string.Empty;
        var rest = span[offset..];
        var end = rest.IndexOf((byte)0);
        return Encoding.ASCII.GetString(end < 0 ? rest : rest[..end]);
    }
}
