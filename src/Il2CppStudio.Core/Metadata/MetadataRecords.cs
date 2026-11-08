using Il2CppStudio.Core.IO;

namespace Il2CppStudio.Core.Metadata;

/// <summary>A fixed-size metadata table row whose layout may depend on the metadata version.</summary>
public interface IMetadataRecord<TSelf> where TSelf : IMetadataRecord<TSelf>
{
    static abstract int SizeOf(MetadataVersion version);
    static abstract TSelf Read(ref SpanReader reader, MetadataVersion version);
}

public readonly record struct Il2CppTypeDefinition(
    int NameIndex, int NamespaceIndex, int ByvalTypeIndex, int DeclaringTypeIndex, int ParentIndex,
    int ElementTypeIndex, int GenericContainerIndex, uint Flags,
    int FieldStart, int MethodStart, int EventStart, int PropertyStart, int NestedTypesStart,
    int InterfacesStart, int VtableStart, int InterfaceOffsetsStart,
    ushort MethodCount, ushort PropertyCount, ushort FieldCount, ushort EventCount,
    ushort NestedTypeCount, ushort VtableCount, ushort InterfacesCount, ushort InterfaceOffsetsCount,
    uint Bitfield, uint Token) : IMetadataRecord<Il2CppTypeDefinition>
{
    public bool IsValueType => (Bitfield & 0x1) != 0;
    public bool IsEnum => ((Bitfield >> 1) & 0x1) != 0;
    public bool HasFinalizer => ((Bitfield >> 2) & 0x1) != 0;
    public bool HasStaticConstructor => ((Bitfield >> 3) & 0x1) != 0;
    public bool IsBlittable => ((Bitfield >> 4) & 0x1) != 0;
    public bool IsInterface => (Flags & 0x20) != 0; // TypeAttributes.Interface

    public static int SizeOf(MetadataVersion v) => 16 * 4 + 8 * 2 + 2 * 4   // 88
        + (v.HasCustomAttributeIndex ? 4 : 0) + (v.HasByrefTypeIndex ? 4 : 0) + (v.HasCodegenIndices ? 8 : 0);

    public static Il2CppTypeDefinition Read(ref SpanReader r, MetadataVersion v)
    {
        int name = r.ReadInt32(), ns = r.ReadInt32();
        if (v.HasCustomAttributeIndex) r.ReadInt32();
        var byval = r.ReadInt32();
        if (v.HasByrefTypeIndex) r.ReadInt32();
        int declaring = r.ReadInt32(), parent = r.ReadInt32(), element = r.ReadInt32();
        if (v.HasCodegenIndices) { r.ReadInt32(); r.ReadInt32(); }   // rgctxStartIndex, rgctxCount
        return new(name, ns, byval, declaring, parent, element, r.ReadInt32(), r.ReadUInt32(),
            r.ReadInt32(), r.ReadInt32(), r.ReadInt32(), r.ReadInt32(), r.ReadInt32(),
            r.ReadInt32(), r.ReadInt32(), r.ReadInt32(),
            r.ReadUInt16(), r.ReadUInt16(), r.ReadUInt16(), r.ReadUInt16(),
            r.ReadUInt16(), r.ReadUInt16(), r.ReadUInt16(), r.ReadUInt16(),
            r.ReadUInt32(), r.ReadUInt32());
    }
}

public readonly record struct Il2CppMethodDefinition(
    int NameIndex, int DeclaringType, int ReturnType, uint ReturnParameterToken, int ParameterStart,
    int GenericContainerIndex, uint Token, ushort Flags, ushort IFlags, ushort Slot, ushort ParameterCount,
    int MethodIndex = -1, int InvokerIndex = -1)
    : IMetadataRecord<Il2CppMethodDefinition>
{
    /// <summary>
    /// 6 x int32 (+ returnParameterToken in v31) + 4 x uint16. v24.0 adds customAttributeIndex; v24.0-24.1 add
    /// methodIndex (index into CodeRegistration.methodPointers), invokerIndex, delegateWrapperIndex and the rgctx pair.
    /// </summary>
    public static int SizeOf(MetadataVersion v) => (v.HasReturnParameterToken ? 36 : 32)
        + (v.HasCustomAttributeIndex ? 4 : 0) + (v.HasCodegenIndices ? 20 : 0);

    public static Il2CppMethodDefinition Read(ref SpanReader r, MetadataVersion v)
    {
        var name = r.ReadInt32();
        var declaring = r.ReadInt32();
        var ret = r.ReadInt32();
        var retToken = v.HasReturnParameterToken ? r.ReadUInt32() : 0u;
        var paramStart = r.ReadInt32();
        if (v.HasCustomAttributeIndex) r.ReadInt32();
        var container = r.ReadInt32();
        int methodIndex = -1, invokerIndex = -1;
        if (v.HasCodegenIndices)
        {
            methodIndex = r.ReadInt32();
            invokerIndex = r.ReadInt32();
            r.ReadInt32(); r.ReadInt32(); r.ReadInt32();   // delegateWrapperIndex, rgctxStartIndex, rgctxCount
        }
        return new(name, declaring, ret, retToken, paramStart, container, r.ReadUInt32(),
            r.ReadUInt16(), r.ReadUInt16(), r.ReadUInt16(), r.ReadUInt16(), methodIndex, invokerIndex);
    }
}

public readonly record struct Il2CppParameterDefinition(int NameIndex, uint Token, int TypeIndex)
    : IMetadataRecord<Il2CppParameterDefinition>
{
    public static int SizeOf(MetadataVersion v) => v.HasCustomAttributeIndex ? 16 : 12;
    public static Il2CppParameterDefinition Read(ref SpanReader r, MetadataVersion v)
    {
        int name = r.ReadInt32(); var token = r.ReadUInt32();
        if (v.HasCustomAttributeIndex) r.ReadInt32();
        return new(name, token, r.ReadInt32());
    }
}

public readonly record struct Il2CppFieldDefinition(int NameIndex, int TypeIndex, uint Token)
    : IMetadataRecord<Il2CppFieldDefinition>
{
    public static int SizeOf(MetadataVersion v) => v.HasCustomAttributeIndex ? 16 : 12;
    public static Il2CppFieldDefinition Read(ref SpanReader r, MetadataVersion v)
    {
        int name = r.ReadInt32(), type = r.ReadInt32();
        if (v.HasCustomAttributeIndex) r.ReadInt32();
        return new(name, type, r.ReadUInt32());
    }
}

/// <summary>Used for both field and parameter default values (identical layout).</summary>
public readonly record struct Il2CppDefaultValue(int OwnerIndex, int TypeIndex, int DataIndex)
    : IMetadataRecord<Il2CppDefaultValue>
{
    public static int SizeOf(MetadataVersion v) => 12;
    public static Il2CppDefaultValue Read(ref SpanReader r, MetadataVersion v) => new(r.ReadInt32(), r.ReadInt32(), r.ReadInt32());
}

public readonly record struct Il2CppFieldMarshaledSize(int FieldIndex, int TypeIndex, int Size)
    : IMetadataRecord<Il2CppFieldMarshaledSize>
{
    public static int SizeOf(MetadataVersion v) => 12;
    public static Il2CppFieldMarshaledSize Read(ref SpanReader r, MetadataVersion v) => new(r.ReadInt32(), r.ReadInt32(), r.ReadInt32());
}

public readonly record struct Il2CppPropertyDefinition(int NameIndex, int Get, int Set, uint Attrs, uint Token)
    : IMetadataRecord<Il2CppPropertyDefinition>
{
    public static int SizeOf(MetadataVersion v) => v.HasCustomAttributeIndex ? 24 : 20;
    public static Il2CppPropertyDefinition Read(ref SpanReader r, MetadataVersion v)
    {
        int name = r.ReadInt32(), get = r.ReadInt32(), set = r.ReadInt32(); var attrs = r.ReadUInt32();
        if (v.HasCustomAttributeIndex) r.ReadInt32();
        return new(name, get, set, attrs, r.ReadUInt32());
    }
}

public readonly record struct Il2CppEventDefinition(int NameIndex, int TypeIndex, int Add, int Remove, int Raise, uint Token)
    : IMetadataRecord<Il2CppEventDefinition>
{
    public static int SizeOf(MetadataVersion v) => v.HasCustomAttributeIndex ? 28 : 24;
    public static Il2CppEventDefinition Read(ref SpanReader r, MetadataVersion v)
    {
        int name = r.ReadInt32(), type = r.ReadInt32(), add = r.ReadInt32(), remove = r.ReadInt32(), raise = r.ReadInt32();
        if (v.HasCustomAttributeIndex) r.ReadInt32();
        return new(name, type, add, remove, raise, r.ReadUInt32());
    }
}

public readonly record struct Il2CppGenericContainer(int OwnerIndex, int TypeArgc, int IsMethod, int GenericParameterStart)
    : IMetadataRecord<Il2CppGenericContainer>
{
    public static int SizeOf(MetadataVersion v) => 16;
    public static Il2CppGenericContainer Read(ref SpanReader r, MetadataVersion v) =>
        new(r.ReadInt32(), r.ReadInt32(), r.ReadInt32(), r.ReadInt32());
}

public readonly record struct Il2CppGenericParameter(int OwnerIndex, int NameIndex, short ConstraintsStart, short ConstraintsCount, ushort Num, ushort Flags)
    : IMetadataRecord<Il2CppGenericParameter>
{
    public static int SizeOf(MetadataVersion v) => 16;
    public static Il2CppGenericParameter Read(ref SpanReader r, MetadataVersion v) =>
        new(r.ReadInt32(), r.ReadInt32(), r.ReadInt16(), r.ReadInt16(), r.ReadUInt16(), r.ReadUInt16());
}

public readonly record struct Il2CppImageDefinition(
    int NameIndex, int AssemblyIndex, int TypeStart, uint TypeCount, int ExportedTypeStart, uint ExportedTypeCount,
    int EntryPointIndex, uint Token, int CustomAttributeStart, uint CustomAttributeCount)
    : IMetadataRecord<Il2CppImageDefinition>
{
    public static int SizeOf(MetadataVersion v) => v.HasImageAttributeRanges ? 40 : 32;
    public static Il2CppImageDefinition Read(ref SpanReader r, MetadataVersion v) => new(
        r.ReadInt32(), r.ReadInt32(), r.ReadInt32(), r.ReadUInt32(), r.ReadInt32(), r.ReadUInt32(),
        r.ReadInt32(), r.ReadUInt32(),
        v.HasImageAttributeRanges ? r.ReadInt32() : 0, v.HasImageAttributeRanges ? r.ReadUInt32() : 0u);
}

public readonly record struct Il2CppAssemblyDefinition(
    int ImageIndex, uint Token, int ReferencedAssemblyStart, int ReferencedAssemblyCount,
    int NameIndex, int CultureIndex, int PublicKeyIndex, uint HashAlg, int HashLen, uint Flags,
    int Major, int Minor, int Build, int Revision, ulong PublicKeyToken)
    : IMetadataRecord<Il2CppAssemblyDefinition>
{
    public string VersionString => $"{Major}.{Minor}.{Build}.{Revision}";
    public string PublicKeyTokenHex => PublicKeyToken == 0 ? "null" : Convert.ToHexStringLower(BitConverter.GetBytes(PublicKeyToken));

    /// <summary>64 bytes for v24.4+; v24.0 has customAttributeIndex instead of token; v24.0-24.3 add hashValueIndex.</summary>
    public static int SizeOf(MetadataVersion v) => 4 * 4 + 10 * 4 + 8 + (v.HasHashValueIndex ? 4 : 0);

    public static Il2CppAssemblyDefinition Read(ref SpanReader r, MetadataVersion v)
    {
        var image = r.ReadInt32();
        var tokenOrAttr = r.ReadUInt32();   // token (24.1+) or customAttributeIndex (24.0)
        int refStart = r.ReadInt32(), refCount = r.ReadInt32(), name = r.ReadInt32(), culture = r.ReadInt32();
        if (v.HasHashValueIndex) r.ReadInt32();
        return new(image, v.HasImageAttributeRanges ? tokenOrAttr : 0x20000001u, refStart, refCount, name, culture,
            r.ReadInt32(), r.ReadUInt32(), r.ReadInt32(), r.ReadUInt32(),
            r.ReadInt32(), r.ReadInt32(), r.ReadInt32(), r.ReadInt32(), r.ReadUInt64());
    }
}

public readonly record struct Il2CppStringLiteral(uint Length, int DataIndex) : IMetadataRecord<Il2CppStringLiteral>
{
    public static int SizeOf(MetadataVersion v) => 8;
    public static Il2CppStringLiteral Read(ref SpanReader r, MetadataVersion v) => new(r.ReadUInt32(), r.ReadInt32());
}

public readonly record struct Il2CppFieldRef(int TypeIndex, int FieldIndex) : IMetadataRecord<Il2CppFieldRef>
{
    public static int SizeOf(MetadataVersion v) => 8;
    public static Il2CppFieldRef Read(ref SpanReader r, MetadataVersion v) => new(r.ReadInt32(), r.ReadInt32());
}

public readonly record struct Il2CppInterfaceOffsetPair(int InterfaceTypeIndex, int Offset) : IMetadataRecord<Il2CppInterfaceOffsetPair>
{
    public static int SizeOf(MetadataVersion v) => 8;
    public static Il2CppInterfaceOffsetPair Read(ref SpanReader r, MetadataVersion v) => new(r.ReadInt32(), r.ReadInt32());
}

public readonly record struct Il2CppMetadataRange(int Start, int Length) : IMetadataRecord<Il2CppMetadataRange>
{
    public static int SizeOf(MetadataVersion v) => 8;
    public static Il2CppMetadataRange Read(ref SpanReader r, MetadataVersion v) => new(r.ReadInt32(), r.ReadInt32());
}

/// <summary>v27 custom attribute index: attribute types for a token are attributeTypes[Start..Start+Count].</summary>
public readonly record struct Il2CppCustomAttributeTypeRange(uint Token, int Start, int Count) : IMetadataRecord<Il2CppCustomAttributeTypeRange>
{
    public static int SizeOf(MetadataVersion v) => v.HasImageAttributeRanges ? 12 : 8;
    public static Il2CppCustomAttributeTypeRange Read(ref SpanReader r, MetadataVersion v) =>
        new(v.HasImageAttributeRanges ? r.ReadUInt32() : 0u, r.ReadInt32(), r.ReadInt32());
}

/// <summary>v24: a method's runtime-metadata usages are metadataUsagePairs[Start..Start+Count].</summary>
public readonly record struct Il2CppMetadataUsageList(uint Start, uint Count) : IMetadataRecord<Il2CppMetadataUsageList>
{
    public static int SizeOf(MetadataVersion v) => 8;
    public static Il2CppMetadataUsageList Read(ref SpanReader r, MetadataVersion v) => new(r.ReadUInt32(), r.ReadUInt32());
}

/// <summary>v24: metadataUsages[DestinationIndex] (a global in the binary) holds the item encoded by EncodedSourceIndex.</summary>
public readonly record struct Il2CppMetadataUsagePair(uint DestinationIndex, uint EncodedSourceIndex) : IMetadataRecord<Il2CppMetadataUsagePair>
{
    public static int SizeOf(MetadataVersion v) => 8;
    public static Il2CppMetadataUsagePair Read(ref SpanReader r, MetadataVersion v) => new(r.ReadUInt32(), r.ReadUInt32());
}

/// <summary>v29+ custom attribute index: blob for a token starts at attributeData + StartOffset.</summary>
public readonly record struct Il2CppCustomAttributeDataRange(uint Token, uint StartOffset) : IMetadataRecord<Il2CppCustomAttributeDataRange>
{
    public static int SizeOf(MetadataVersion v) => 8;
    public static Il2CppCustomAttributeDataRange Read(ref SpanReader r, MetadataVersion v) => new(r.ReadUInt32(), r.ReadUInt32());
}

public readonly record struct Il2CppWindowsRuntimeTypeNamePair(int NameIndex, int TypeIndex) : IMetadataRecord<Il2CppWindowsRuntimeTypeNamePair>
{
    public static int SizeOf(MetadataVersion v) => 8;
    public static Il2CppWindowsRuntimeTypeNamePair Read(ref SpanReader r, MetadataVersion v) => new(r.ReadInt32(), r.ReadInt32());
}

/// <summary>Plain int32 table element (nested types, interfaces, constraints, vtable slots, ...).</summary>
public readonly record struct Int32Element(int Value) : IMetadataRecord<Int32Element>
{
    public static int SizeOf(MetadataVersion v) => 4;
    public static Int32Element Read(ref SpanReader r, MetadataVersion v) => new(r.ReadInt32());
}
