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
    private readonly Dictionary<int, Il2CppDefaultValue> _paramDefaults;

    public DefaultValueReader(GlobalMetadata md, Il2CppRegistration reg)
    {
        _md = md;
        _reg = reg;
        _fieldDefaults = md.FieldDefaultValues.GroupBy(d => d.OwnerIndex).ToDictionary(g => g.Key, g => g.First());
        _paramDefaults = md.ParameterDefaultValues.GroupBy(d => d.OwnerIndex).ToDictionary(g => g.Key, g => g.First());
    }

    public bool TryGetFieldDefault(int fieldIndex, out object? value)
    {
        value = null;
        return _fieldDefaults.TryGetValue(fieldIndex, out var d) && TryDecode(d.TypeIndex, d.DataIndex, out value);
    }

    public bool TryGetParameterDefault(int paramIndex, out object? value, out bool isNullDefault)
    {
        value = null;
        isNullDefault = false;
        if (!_paramDefaults.TryGetValue(paramIndex, out var d)) return false;
        if (d.DataIndex < 0)
        {
            isNullDefault = true;
            return true;
        }
        if (TryDecode(d.TypeIndex, d.DataIndex, out value)) return true;
        if (value is null && _reg.TryGetType(d.TypeIndex, out var t)
            && t.Type is Il2CppTypeEnum.String or Il2CppTypeEnum.Class or Il2CppTypeEnum.Object or Il2CppTypeEnum.GenericInst or Il2CppTypeEnum.SzArray or Il2CppTypeEnum.Array)
        {
            isNullDefault = true;
            return true;
        }
        return false;
    }

    public bool TryGetFieldRvaBytes(int fieldIndex, int size, out byte[] bytes)
    {
        bytes = [];
        if (size <= 0 || !_fieldDefaults.TryGetValue(fieldIndex, out var d) || d.DataIndex < 0)
            return false;
        var blob = _md.GetSectionBytes("fieldAndParameterDefaultValueData").Span;
        if (d.DataIndex + size > blob.Length)
            return false;
        bytes = blob.Slice(d.DataIndex, size).ToArray();
        return true;
    }

    public bool TryDecode(int typeIndex, int dataIndex, out object? value)
    {
        value = null;
        if (dataIndex < 0 || !_reg.TryGetType(typeIndex, out var t)) return false;
        var kind = t.Type;
        if (kind == Il2CppTypeEnum.ValueType && (uint)t.KlassIndex < (uint)_md.TypeDefinitions.Length)
        {
            var td = _md.TypeDefinitions[t.KlassIndex];
            if (td.IsEnum && td.FieldCount > 0)
            {
                for (var f = 0; f < td.FieldCount; f++)
                {
                    var fd = _md.Fields[td.FieldStart + f];
                    if (_md.GetString(fd.NameIndex) == "value__" && _reg.TryGetType(fd.TypeIndex, out var ut))
                    {
                        kind = ut.Type;
                        break;
                    }
                }
            }
        }
        var blob = _md.GetSectionBytes("fieldAndParameterDefaultValueData").Span;
        if ((uint)dataIndex >= (uint)blob.Length) return false;
        var s = blob[dataIndex..];
        var compressed = _md.Version.Major >= 29;
        value = kind switch
        {
            Il2CppTypeEnum.Boolean when s.Length >= 1 => s[0] != 0,
            Il2CppTypeEnum.U1 when s.Length >= 1 => s[0],
            Il2CppTypeEnum.I1 when s.Length >= 1 => (sbyte)s[0],
            Il2CppTypeEnum.Char when s.Length >= 2 => (char)BinaryPrimitives.ReadUInt16LittleEndian(s),
            Il2CppTypeEnum.U2 when s.Length >= 2 => BinaryPrimitives.ReadUInt16LittleEndian(s),
            Il2CppTypeEnum.I2 when s.Length >= 2 => BinaryPrimitives.ReadInt16LittleEndian(s),
            Il2CppTypeEnum.U4 when compressed && TryReadCompressedUInt32(s, out var u4) => u4,
            Il2CppTypeEnum.U4 when !compressed && s.Length >= 4 => BinaryPrimitives.ReadUInt32LittleEndian(s),
            Il2CppTypeEnum.I4 when compressed && TryReadCompressedInt32(s, out var i4) => i4,
            Il2CppTypeEnum.I4 when !compressed && s.Length >= 4 => BinaryPrimitives.ReadInt32LittleEndian(s),
            Il2CppTypeEnum.U8 when s.Length >= 8 => BinaryPrimitives.ReadUInt64LittleEndian(s),
            Il2CppTypeEnum.I8 when s.Length >= 8 => BinaryPrimitives.ReadInt64LittleEndian(s),
            Il2CppTypeEnum.R4 when s.Length >= 4 => BinaryPrimitives.ReadSingleLittleEndian(s),
            Il2CppTypeEnum.R8 when s.Length >= 8 => BinaryPrimitives.ReadDoubleLittleEndian(s),
            Il2CppTypeEnum.String => ReadString(s, compressed),
            _ => null,
        };
        return value is not null;
    }

    private static bool TryReadCompressedUInt32(ReadOnlySpan<byte> s, out uint value)
    {
        value = 0;
        if (s.IsEmpty) return false;
        var b = s[0];
        if ((b & 0x80) == 0) { value = b; return true; }
        if ((b & 0xC0) == 0x80) { if (s.Length < 2) return false; value = (uint)(b & 0x3F) << 8 | s[1]; return true; }
        if ((b & 0xE0) == 0xC0) { if (s.Length < 4) return false; value = (uint)(b & 0x1F) << 24 | (uint)s[1] << 16 | (uint)s[2] << 8 | s[3]; return true; }
        if (b == 0xF0) { if (s.Length < 5) return false; value = BinaryPrimitives.ReadUInt32LittleEndian(s[1..]); return true; }
        value = b switch { 0xFE => uint.MaxValue - 1, 0xFF => uint.MaxValue, _ => 0u };
        return true;
    }

    private static bool TryReadCompressedInt32(ReadOnlySpan<byte> s, out int value)
    {
        value = 0;
        if (!TryReadCompressedUInt32(s, out var encoded)) return false;
        if (encoded == uint.MaxValue) { value = int.MinValue; return true; }
        var neg = (encoded & 1) != 0;
        encoded >>= 1;
        value = neg ? -(int)(encoded + 1) : (int)encoded;
        return true;
    }

    private static string? ReadString(ReadOnlySpan<byte> s, bool compressed)
    {
        int length, header;
        if (compressed)
        {
            if (!TryReadCompressedInt32(s, out length)) return null;
            header = s.IsEmpty ? 0 : (s[0] & 0x80) == 0 ? 1 : (s[0] & 0xC0) == 0x80 ? 2 : (s[0] & 0xE0) == 0xC0 ? 4 : s[0] == 0xF0 ? 5 : 1;
        }
        else
        {
            if (s.Length < 4) return null;
            length = BinaryPrimitives.ReadInt32LittleEndian(s);
            header = 4;
        }
        if (length < 0 || header + length > s.Length) return null;   // null string default or truncated
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
