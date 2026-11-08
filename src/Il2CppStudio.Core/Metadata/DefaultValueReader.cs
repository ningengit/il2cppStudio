using System.Buffers.Binary;
using System.Text;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.Metadata;

/// <summary>
/// Decodes field / parameter default values (enum members, const fields, optional parameters) from
/// <c>fieldAndParameterDefaultValueData</c>. From v29 on, 32-bit integers and string lengths use IL2CPP's
/// compressed encoding (1/2/4/5 bytes, ZigZag-style sign bit for signed values).
/// </summary>
public sealed class DefaultValueReader
{
    private readonly GlobalMetadata _md;
    private readonly Il2CppRegistration _reg;
    private readonly Dictionary<int, Il2CppDefaultValue> _fieldDefaults;

    public DefaultValueReader(GlobalMetadata md, Il2CppRegistration reg)
    {
        _md = md;
        _reg = reg;
        _fieldDefaults = md.FieldDefaultValues.GroupBy(d => d.OwnerIndex).ToDictionary(g => g.Key, g => g.First());
    }

    public bool TryGetFieldDefault(int fieldIndex, out object? value)
    {
        value = null;
        return _fieldDefaults.TryGetValue(fieldIndex, out var d) && TryDecode(d.TypeIndex, d.DataIndex, out value);
    }

    public bool TryDecode(int typeIndex, int dataIndex, out object? value)
    {
        value = null;
        if (dataIndex < 0 || !_reg.TryGetType(typeIndex, out var t)) return false;
        var blob = _md.GetSectionBytes("fieldAndParameterDefaultValueData").Span;
        if (dataIndex >= blob.Length) return false;
        var s = blob[dataIndex..];
        var compressed = _md.Version.Major >= 29;
        try
        {
            value = t.Type switch
            {
                Il2CppTypeEnum.Boolean => s[0] != 0,
                Il2CppTypeEnum.U1 => s[0],
                Il2CppTypeEnum.I1 => (sbyte)s[0],
                Il2CppTypeEnum.Char => (char)BinaryPrimitives.ReadUInt16LittleEndian(s),
                Il2CppTypeEnum.U2 => BinaryPrimitives.ReadUInt16LittleEndian(s),
                Il2CppTypeEnum.I2 => BinaryPrimitives.ReadInt16LittleEndian(s),
                Il2CppTypeEnum.U4 => compressed ? ReadCompressedUInt32(s, out _) : BinaryPrimitives.ReadUInt32LittleEndian(s),
                Il2CppTypeEnum.I4 => compressed ? ReadCompressedInt32(s) : BinaryPrimitives.ReadInt32LittleEndian(s),
                Il2CppTypeEnum.U8 => BinaryPrimitives.ReadUInt64LittleEndian(s),
                Il2CppTypeEnum.I8 => BinaryPrimitives.ReadInt64LittleEndian(s),
                Il2CppTypeEnum.R4 => BinaryPrimitives.ReadSingleLittleEndian(s),
                Il2CppTypeEnum.R8 => BinaryPrimitives.ReadDoubleLittleEndian(s),
                Il2CppTypeEnum.String => ReadString(s, compressed),
                _ => null,
            };
        }
        catch (ArgumentOutOfRangeException) { value = null; }
        return value is not null;
    }

    private static string? ReadString(ReadOnlySpan<byte> s, bool compressed)
    {
        int length, header;
        if (compressed) { length = ReadCompressedInt32(s, out header); }
        else { length = BinaryPrimitives.ReadInt32LittleEndian(s); header = 4; }
        if (length == -1) return null;   // null string default
        return Encoding.UTF8.GetString(s.Slice(header, length));
    }

    public static uint ReadCompressedUInt32(ReadOnlySpan<byte> s, out int size)
    {
        var b = s[0];
        if ((b & 0x80) == 0) { size = 1; return b; }
        if ((b & 0xC0) == 0x80) { size = 2; return (uint)(b & 0x3F) << 8 | s[1]; }
        if ((b & 0xE0) == 0xC0) { size = 4; return (uint)(b & 0x1F) << 24 | (uint)s[1] << 16 | (uint)s[2] << 8 | s[3]; }
        size = b == 0xF0 ? 5 : 1;
        return b switch
        {
            0xF0 => BinaryPrimitives.ReadUInt32LittleEndian(s[1..]),
            0xFE => uint.MaxValue - 1,
            0xFF => uint.MaxValue,
            _ => throw new InvalidDataException($"Invalid compressed integer prefix 0x{b:X2}."),
        };
    }

    public static int ReadCompressedInt32(ReadOnlySpan<byte> s) => ReadCompressedInt32(s, out _);

    public static int ReadCompressedInt32(ReadOnlySpan<byte> s, out int size)
    {
        var encoded = ReadCompressedUInt32(s, out size);
        if (encoded == uint.MaxValue) return int.MinValue;
        var negative = (encoded & 1) != 0;
        encoded >>= 1;
        return negative ? -(int)(encoded + 1) : (int)encoded;
    }
}
