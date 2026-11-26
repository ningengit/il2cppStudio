using System.Buffers.Binary;
using System.Text;
using Il2CppStudio.Core.Registration;

namespace Il2CppStudio.Core.Metadata;

public readonly record struct DecodedAttrValue(
    Il2CppTypeEnum TypeCode,
    int TypeIndex,
    Il2CppTypeEnum ArrayElementTypeCode,
    int ArrayElementTypeIndex,
    object? Value);

public readonly record struct DecodedAttrNamedArg(
    bool IsField,
    int DeclaringTypeDef,
    int MemberOffsetInType,
    DecodedAttrValue Value);

public readonly record struct DecodedCustomAttribute(
    int CtorMethodIndex,
    IReadOnlyList<DecodedAttrValue> FixedArgs,
    IReadOnlyList<DecodedAttrNamedArg> NamedArgs);

/// <summary>
/// Decodes IL2CPP v29+ <c>attributeData</c> blobs (and v27 <c>attributesInfo</c> type lists) using the
/// exact serialization layout in Unity's <c>CustomAttributeDataReader.cpp</c> / <c>BlobReader.cpp</c>.
/// </summary>
public sealed class CustomAttributeReader
{
    private readonly GlobalMetadata _md;
    private readonly Il2CppRegistration? _reg;
    private readonly Dictionary<(int ImageIndex, uint Token), (uint Start, uint End)> _dataRanges = [];
    private readonly Dictionary<(int ImageIndex, uint Token), (int Start, int Count)> _typeRanges = [];
    private readonly Dictionary<int, int> _byvalToTypeDef = [];

    public CustomAttributeReader(GlobalMetadata md, Il2CppRegistration? reg)
    {
        _md = md;
        _reg = reg;
        for (var i = 0; i < md.TypeDefinitions.Length; i++)
        {
            var bt = md.TypeDefinitions[i].ByvalTypeIndex;
            if (bt >= 0) _byvalToTypeDef.TryAdd(bt, i);
        }

        var blobLen = md.Header.TryGet("attributeData", out _) ? (uint)md.GetSectionBytes("attributeData").Length : 0u;
        if (md.AttributeDataRanges.Length > 0 && blobLen > 0)
        {
            for (var imgIdx = 0; imgIdx < md.Images.Length; imgIdx++)
            {
                var img = md.Images[imgIdx];
                var start = img.CustomAttributeStart;
                var count = (int)img.CustomAttributeCount;
                for (var i = 0; i < count && start + i < md.AttributeDataRanges.Length; i++)
                {
                    var r = md.AttributeDataRanges[start + i];
                    var nextOff = start + i + 1 < md.AttributeDataRanges.Length
                        ? md.AttributeDataRanges[start + i + 1].StartOffset
                        : blobLen;
                    if (nextOff < r.StartOffset || nextOff > blobLen) nextOff = blobLen;
                    if (r.Token != 0 && nextOff > r.StartOffset)
                        _dataRanges[(imgIdx, r.Token)] = (r.StartOffset, nextOff);
                }
            }
        }
        else if (md.AttributeTypeRanges.Length > 0 && md.Version.HasImageAttributeRanges)
        {
            for (var imgIdx = 0; imgIdx < md.Images.Length; imgIdx++)
            {
                var img = md.Images[imgIdx];
                var start = img.CustomAttributeStart;
                var count = (int)img.CustomAttributeCount;
                for (var i = 0; i < count && start + i < md.AttributeTypeRanges.Length; i++)
                {
                    var r = md.AttributeTypeRanges[start + i];
                    if (r.Token != 0 && r.Count > 0)
                        _typeRanges[(imgIdx, r.Token)] = (r.Start, r.Count);
                }
            }
        }
    }

    public IReadOnlyList<DecodedCustomAttribute> ReadForToken(int imageIndex, uint token)
    {
        if (token == 0) return [];
        if (_dataRanges.TryGetValue((imageIndex, token), out var range))
            return DecodeBlob(range.Start, range.End);

        if (_typeRanges.TryGetValue((imageIndex, token), out var tr))
        {
            var list = new List<DecodedCustomAttribute>(tr.Count);
            for (var i = 0; i < tr.Count && tr.Start + i < _md.AttributeTypes.Length; i++)
            {
                var typeIdx = _md.AttributeTypes[tr.Start + i];
                var td = ResolveTypeDef(typeIdx);
                if ((uint)td >= (uint)_md.TypeDefinitions.Length) continue;
                var def = _md.TypeDefinitions[td];
                for (var m = 0; m < def.MethodCount; m++)
                {
                    var mi = def.MethodStart + m;
                    var meth = _md.Methods[mi];
                    if (meth.ParameterCount == 0 && _md.GetString(meth.NameIndex) == ".ctor")
                    {
                        list.Add(new DecodedCustomAttribute(mi, [], []));
                        break;
                    }
                }
            }
            return list;
        }
        return [];
    }

    private IReadOnlyList<DecodedCustomAttribute> DecodeBlob(uint startOff, uint endOff)
    {
        if (!_md.Header.TryGet("attributeData", out _)) return [];
        var all = _md.GetSectionBytes("attributeData").Span;
        if (endOff > (uint)all.Length || startOff >= endOff) return [];
        var span = all[(int)startOff..(int)endOff];
        var pos = 0;
        var count = (int)ReadCompressedUInt32(span, ref pos);
        if (count <= 0 || count > 256 || pos + count * 4 > span.Length) return [];

        var ctorIndices = new int[count];
        for (var i = 0; i < count; i++)
        {
            ctorIndices[i] = BinaryPrimitives.ReadInt32LittleEndian(span.Slice(pos, 4));
            pos += 4;
        }

        var result = new List<DecodedCustomAttribute>(count);
        for (var i = 0; i < count; i++)
        {
            var ctorIdx = ctorIndices[i];
            if ((uint)ctorIdx >= (uint)_md.Methods.Length) return result;
            var ctorDeclTd = _md.Methods[ctorIdx].DeclaringType;

            var argCount = (int)ReadCompressedUInt32(span, ref pos);
            var fieldCount = (int)ReadCompressedUInt32(span, ref pos);
            var propCount = (int)ReadCompressedUInt32(span, ref pos);
            if (argCount < 0 || fieldCount < 0 || propCount < 0 || argCount > 64 || fieldCount > 64 || propCount > 64)
                return result;

            var fixedArgs = new List<DecodedAttrValue>(argCount);
            for (var a = 0; a < argCount; a++)
                fixedArgs.Add(ReadAttrValue(span, ref pos));

            var namedArgs = new List<DecodedAttrNamedArg>(fieldCount + propCount);
            for (var f = 0; f < fieldCount; f++)
            {
                var val = ReadAttrValue(span, ref pos);
                var (declTd, memberOff) = ReadNamedMember(span, ref pos, ctorDeclTd);
                namedArgs.Add(new DecodedAttrNamedArg(true, declTd, memberOff, val));
            }
            for (var p = 0; p < propCount; p++)
            {
                var val = ReadAttrValue(span, ref pos);
                var (declTd, memberOff) = ReadNamedMember(span, ref pos, ctorDeclTd);
                namedArgs.Add(new DecodedAttrNamedArg(false, declTd, memberOff, val));
            }

            result.Add(new DecodedCustomAttribute(ctorIdx, fixedArgs, namedArgs));
        }
        return result;
    }

    private (int DeclTd, int MemberOff) ReadNamedMember(ReadOnlySpan<byte> s, ref int pos, int attrDeclTd)
    {
        var idx = ReadCompressedInt32(s, ref pos);
        if (idx >= 0) return (attrDeclTd, idx);
        var actualIdx = -(idx + 1);
        var declTd = (int)ReadCompressedUInt32(s, ref pos);
        return (declTd, actualIdx);
    }

    private DecodedAttrValue ReadAttrValue(ReadOnlySpan<byte> s, ref int pos)
    {
        var (encodedType, typeIndex, underlyingType) = ReadEncodedTypeEnum(s, ref pos);
        if (encodedType == Il2CppTypeEnum.SzArray)
        {
            var len = ReadCompressedInt32(s, ref pos);
            if (len < 0)
                return new DecodedAttrValue(Il2CppTypeEnum.SzArray, -1, Il2CppTypeEnum.Object, -1, null);
            var (elemType, elemTypeIdx, elemUnderlying) = ReadEncodedTypeEnum(s, ref pos);
            var diffElements = s[pos++];
            var items = new DecodedAttrValue[len];
            for (var i = 0; i < len; i++)
            {
                var curType = elemType;
                var curIdx = elemTypeIdx;
                var curUnderlying = elemUnderlying;
                if (diffElements != 0)
                    (curType, curIdx, curUnderlying) = ReadEncodedTypeEnum(s, ref pos);
                var itemVal = ReadConstantScalar(curType, curUnderlying, s, ref pos);
                items[i] = new DecodedAttrValue(curType, curIdx, Il2CppTypeEnum.End, -1, itemVal);
            }
            return new DecodedAttrValue(Il2CppTypeEnum.SzArray, -1, elemType, elemTypeIdx, items);
        }

        var scalar = ReadConstantScalar(encodedType, underlyingType, s, ref pos);
        if (encodedType == Il2CppTypeEnum.Il2CppTypeIndex && scalar is int resolvedTypeIdx)
            return new DecodedAttrValue(Il2CppTypeEnum.Il2CppTypeIndex, resolvedTypeIdx, Il2CppTypeEnum.End, -1, null);
        return new DecodedAttrValue(encodedType, typeIndex, Il2CppTypeEnum.End, -1, scalar);
    }

    private (Il2CppTypeEnum RawType, int TypeIndex, Il2CppTypeEnum Underlying) ReadEncodedTypeEnum(ReadOnlySpan<byte> s, ref int pos)
    {
        var raw = (Il2CppTypeEnum)s[pos++];
        if (raw == Il2CppTypeEnum.Enum)
        {
            var enumTypeIdx = ReadCompressedInt32(s, ref pos);
            return (Il2CppTypeEnum.Enum, enumTypeIdx, EnumUnderlyingType(enumTypeIdx));
        }
        return (raw, -1, raw);
    }

    private object? ReadConstantScalar(Il2CppTypeEnum rawType, Il2CppTypeEnum underlying, ReadOnlySpan<byte> s, ref int pos)
    {
        switch (underlying)
        {
            case Il2CppTypeEnum.Boolean: return s[pos++] != 0;
            case Il2CppTypeEnum.U1: return s[pos++];
            case Il2CppTypeEnum.I1: return unchecked((sbyte)s[pos++]);
            case Il2CppTypeEnum.Char:
            {
                var v = (char)BinaryPrimitives.ReadUInt16LittleEndian(s.Slice(pos, 2));
                pos += 2;
                return v;
            }
            case Il2CppTypeEnum.U2:
            {
                var v = BinaryPrimitives.ReadUInt16LittleEndian(s.Slice(pos, 2));
                pos += 2;
                return v;
            }
            case Il2CppTypeEnum.I2:
            {
                var v = BinaryPrimitives.ReadInt16LittleEndian(s.Slice(pos, 2));
                pos += 2;
                return v;
            }
            case Il2CppTypeEnum.U4: return ReadCompressedUInt32(s, ref pos);
            case Il2CppTypeEnum.I4: return ReadCompressedInt32(s, ref pos);
            case Il2CppTypeEnum.U8:
            {
                var v = BinaryPrimitives.ReadUInt64LittleEndian(s.Slice(pos, 8));
                pos += 8;
                return v;
            }
            case Il2CppTypeEnum.I8:
            {
                var v = BinaryPrimitives.ReadInt64LittleEndian(s.Slice(pos, 8));
                pos += 8;
                return v;
            }
            case Il2CppTypeEnum.R4:
            {
                var v = BinaryPrimitives.ReadSingleLittleEndian(s.Slice(pos, 4));
                pos += 4;
                return v;
            }
            case Il2CppTypeEnum.R8:
            {
                var v = BinaryPrimitives.ReadDoubleLittleEndian(s.Slice(pos, 8));
                pos += 8;
                return v;
            }
            case Il2CppTypeEnum.String:
            {
                var len = ReadCompressedInt32(s, ref pos);
                if (len < 0) return null;
                var str = Encoding.UTF8.GetString(s.Slice(pos, len));
                pos += len;
                return str;
            }
            case Il2CppTypeEnum.Il2CppTypeIndex:
                return ReadCompressedInt32(s, ref pos);
            case Il2CppTypeEnum.Class or Il2CppTypeEnum.Object or Il2CppTypeEnum.GenericInst:
                return null;
            default:
                _ = rawType;
                return null;
        }
    }

    private Il2CppTypeEnum EnumUnderlyingType(int typeIndex)
    {
        var td = ResolveTypeDef(typeIndex);
        if ((uint)td < (uint)_md.TypeDefinitions.Length)
        {
            var def = _md.TypeDefinitions[td];
            for (var f = 0; f < def.FieldCount; f++)
            {
                var fd = _md.Fields[def.FieldStart + f];
                if (_md.GetString(fd.NameIndex) == "value__" && _reg is not null && _reg.TryGetType(fd.TypeIndex, out var ut))
                    return ut.Type;
            }
        }
        return Il2CppTypeEnum.I4;
    }

    private int ResolveTypeDef(int typeIndex)
    {
        if (typeIndex < 0) return -1;
        if (_reg is not null && _reg.TryGetType(typeIndex, out var t)) return t.KlassIndex;
        return _byvalToTypeDef.GetValueOrDefault(typeIndex, -1);
    }

    private static uint ReadCompressedUInt32(ReadOnlySpan<byte> s, ref int pos)
    {
        var v = DefaultValueReader.ReadCompressedUInt32(s[pos..], out var sz);
        pos += sz;
        return v;
    }

    private static int ReadCompressedInt32(ReadOnlySpan<byte> s, ref int pos)
    {
        var v = DefaultValueReader.ReadCompressedInt32(s[pos..], out var sz);
        pos += sz;
        return v;
    }
}
