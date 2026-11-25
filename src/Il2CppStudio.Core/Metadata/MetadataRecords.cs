using Il2CppStudio.Core.IO;

namespace Il2CppStudio.Core.Metadata;

/// <summary>Compact serialized index byte widths used by v39+ metadata tables.</summary>
public readonly struct SerializedIndexSizes
{
    public int TypeIndex { get; init; }
    public int TypeDefinitionIndex { get; init; }
    public int GenericContainerIndex { get; init; }
    public int ParameterIndex { get; init; }
    public int GenericParameterIndex { get; init; }
    public int InterfacesIndex { get; init; }
    public int EventIndex { get; init; }
    public int PropertyIndex { get; init; }
    public int NestedTypeIndex { get; init; }
    public int FieldIndex { get; init; }
    public int DefaultValueDataIndex { get; init; }
    public int MethodIndex { get; init; }
    public int GenericInstIndex { get; init; }
    public int GenericMethodIndex { get; init; }
    public int MethodPointerTableIndex { get; init; }
    public int InvokerTableIndex { get; init; }
    public int AdjustorThunkIndex { get; init; }

    public static readonly SerializedIndexSizes Fixed4 = new()
    {
        TypeIndex = 4, TypeDefinitionIndex = 4, GenericContainerIndex = 4, ParameterIndex = 4,
        GenericParameterIndex = 4, InterfacesIndex = 4, EventIndex = 4, PropertyIndex = 4,
        NestedTypeIndex = 4, FieldIndex = 4, DefaultValueDataIndex = 4, MethodIndex = 4,
        GenericInstIndex = 4, GenericMethodIndex = 4, MethodPointerTableIndex = 4,
        InvokerTableIndex = 4, AdjustorThunkIndex = 4,
    };

    public static int GetIndexSize(int count) => count <= byte.MaxValue ? 1 : count <= ushort.MaxValue ? 2 : 4;

    public static int ReadIndex(ref SpanReader r, int size) => size switch
    {
        1 => r.ReadByte() is var b && b == byte.MaxValue ? -1 : b,
        2 => r.ReadUInt16() is var u && u == ushort.MaxValue ? -1 : u,
        _ => r.ReadInt32(),
    };
}

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

    public static int SizeOf(MetadataVersion v) => 16 * 4 + 8 * 2 + (v.HasTokens ? 8 : 4)   // 88 (v27+) or 84 without token
        + (v.HasCustomAttributeIndex ? 4 : 0)
        + (v.HasByrefTypeIndex ? 4 : 0)
        + (v.HasCodegenIndices ? 8 : 0)
        + (v.HasTypeDelegateAndMarshalingIndices ? 8 : 0)
        + (v.HasTypeCcwAndGuidIndices ? 8 : 0);

    public static Il2CppTypeDefinition Read(ref SpanReader r, MetadataVersion v)
    {
        int name = r.ReadInt32(), ns = r.ReadInt32();
        if (v.HasCustomAttributeIndex) r.ReadInt32();
        var byval = r.ReadInt32();
        if (v.HasByrefTypeIndex) r.ReadInt32();
        int declaring = r.ReadInt32(), parent = r.ReadInt32(), element = v.HasElementTypeIndex ? r.ReadInt32() : -1;
        if (v.HasCodegenIndices) { r.ReadInt32(); r.ReadInt32(); }   // rgctxStartIndex, rgctxCount
        var genericContainer = r.ReadInt32();
        if (v.HasTypeDelegateAndMarshalingIndices) { r.ReadInt32(); r.ReadInt32(); }   // delegateWrapperFromManagedToNativeIndex, marshalingFunctionsIndex
        if (v.HasTypeCcwAndGuidIndices) { r.ReadInt32(); r.ReadInt32(); }              // ccwFunctionIndex, guidIndex
        return new(name, ns, byval, declaring, parent, element, genericContainer, r.ReadUInt32(),
            r.ReadInt32(), r.ReadInt32(), r.ReadInt32(), r.ReadInt32(), r.ReadInt32(),
            r.ReadInt32(), r.ReadInt32(), r.ReadInt32(),
            r.ReadUInt16(), r.ReadUInt16(), r.ReadUInt16(), r.ReadUInt16(),
            r.ReadUInt16(), r.ReadUInt16(), r.ReadUInt16(), r.ReadUInt16(),
            r.ReadUInt32(), v.HasTokens ? r.ReadUInt32() : 0u);
    }

    public static Il2CppTypeDefinition ReadCompact(ref SpanReader r, MetadataVersion v, in SerializedIndexSizes s)
    {
        var name = r.ReadInt32();
        var ns = r.ReadInt32();
        var byval = SerializedIndexSizes.ReadIndex(ref r, s.TypeIndex);
        var declaring = SerializedIndexSizes.ReadIndex(ref r, s.TypeIndex);
        var parent = SerializedIndexSizes.ReadIndex(ref r, s.TypeIndex);
        var gc = SerializedIndexSizes.ReadIndex(ref r, s.GenericContainerIndex);
        var flags = r.ReadUInt32();
        var v108 = v.HasV108EmbeddedGenerics;
        var fieldStart = v108 ? SerializedIndexSizes.ReadIndex(ref r, s.FieldIndex) : r.ReadInt32();
        var methodStart = v108 ? SerializedIndexSizes.ReadIndex(ref r, s.MethodIndex) : r.ReadInt32();
        var eventStart = v108 ? SerializedIndexSizes.ReadIndex(ref r, s.EventIndex) : r.ReadInt32();
        var propStart = v108 ? SerializedIndexSizes.ReadIndex(ref r, s.PropertyIndex) : r.ReadInt32();
        var nestedStart = v108 ? SerializedIndexSizes.ReadIndex(ref r, s.NestedTypeIndex) : r.ReadInt32();
        var ifStart = v108 ? SerializedIndexSizes.ReadIndex(ref r, s.InterfacesIndex) : r.ReadInt32();
        var vtableStart = r.ReadInt32();
        var ifOffStart = v108 ? SerializedIndexSizes.ReadIndex(ref r, s.InterfacesIndex) : r.ReadInt32();
        return new(name, ns, byval, declaring, parent, -1, gc, flags,
            fieldStart, methodStart, eventStart, propStart, nestedStart, ifStart, vtableStart, ifOffStart,
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
        return new(name, declaring, ret, retToken, paramStart, container,
            NormalizeToken(r.ReadUInt32(), 0x06000000u, v),
            r.ReadUInt16(), r.ReadUInt16(), r.ReadUInt16(), r.ReadUInt16(), methodIndex, invokerIndex);
    }

    public static Il2CppMethodDefinition ReadCompact(ref SpanReader r, in SerializedIndexSizes s) =>
        new(r.ReadInt32(),
            SerializedIndexSizes.ReadIndex(ref r, s.TypeDefinitionIndex),
            SerializedIndexSizes.ReadIndex(ref r, s.TypeIndex),
            r.ReadUInt32(),
            SerializedIndexSizes.ReadIndex(ref r, s.ParameterIndex),
            SerializedIndexSizes.ReadIndex(ref r, s.GenericContainerIndex),
            r.ReadUInt32(),
            r.ReadUInt16(), r.ReadUInt16(), r.ReadUInt16(), r.ReadUInt16());

    private static uint NormalizeToken(uint token, uint tableBit, MetadataVersion v) =>
        !v.HasTokens && token != 0 && (token & 0xFF000000u) == 0 ? token | tableBit : token;
}

public readonly record struct Il2CppParameterDefinition(int NameIndex, uint Token, int TypeIndex)
    : IMetadataRecord<Il2CppParameterDefinition>
{
    public static int SizeOf(MetadataVersion v) => v.HasCustomAttributeIndex ? 16 : 12;
    public static Il2CppParameterDefinition Read(ref SpanReader r, MetadataVersion v)
    {
        int name = r.ReadInt32(); var token = r.ReadUInt32();
        if (!v.HasTokens && token != 0 && (token & 0xFF000000u) == 0)
            token |= 0x08000000u;
        if (v.HasCustomAttributeIndex) r.ReadInt32();
        return new(name, token, r.ReadInt32());
    }

    public static Il2CppParameterDefinition ReadCompact(ref SpanReader r, in SerializedIndexSizes s) =>
        new(r.ReadInt32(), r.ReadUInt32(), SerializedIndexSizes.ReadIndex(ref r, s.TypeIndex));
}

public readonly record struct Il2CppFieldDefinition(int NameIndex, int TypeIndex, uint Token)
    : IMetadataRecord<Il2CppFieldDefinition>
{
    public static int SizeOf(MetadataVersion v) => 8 + (v.HasCustomAttributeIndex ? 4 : 0) + (v.HasTokens ? 4 : 0);
    public static Il2CppFieldDefinition Read(ref SpanReader r, MetadataVersion v)
    {
        int name = r.ReadInt32(), type = r.ReadInt32();
        if (v.HasCustomAttributeIndex) r.ReadInt32();
        return new(name, type, v.HasTokens ? r.ReadUInt32() : 0u);
    }

    public static Il2CppFieldDefinition ReadCompact(ref SpanReader r, in SerializedIndexSizes s) =>
        new(r.ReadInt32(), SerializedIndexSizes.ReadIndex(ref r, s.TypeIndex), r.ReadUInt32());
}

/// <summary>Used for both field and parameter default values (identical layout).</summary>
public readonly record struct Il2CppDefaultValue(int OwnerIndex, int TypeIndex, int DataIndex)
    : IMetadataRecord<Il2CppDefaultValue>
{
    public static int SizeOf(MetadataVersion v) => 12;
    public static Il2CppDefaultValue Read(ref SpanReader r, MetadataVersion v) => new(r.ReadInt32(), r.ReadInt32(), r.ReadInt32());

    public static Il2CppDefaultValue ReadParamCompact(ref SpanReader r, MetadataVersion v, in SerializedIndexSizes s) =>
        new(SerializedIndexSizes.ReadIndex(ref r, s.ParameterIndex),
            SerializedIndexSizes.ReadIndex(ref r, s.TypeIndex),
            v.HasV108EmbeddedGenerics ? SerializedIndexSizes.ReadIndex(ref r, s.DefaultValueDataIndex) : r.ReadInt32());

    public static Il2CppDefaultValue ReadFieldCompact(ref SpanReader r, MetadataVersion v, in SerializedIndexSizes s) =>
        new(v.HasV108EmbeddedGenerics ? SerializedIndexSizes.ReadIndex(ref r, s.FieldIndex) : r.ReadInt32(),
            SerializedIndexSizes.ReadIndex(ref r, s.TypeIndex),
            v.HasV108EmbeddedGenerics ? SerializedIndexSizes.ReadIndex(ref r, s.DefaultValueDataIndex) : r.ReadInt32());
}

public readonly record struct Il2CppFieldMarshaledSize(int FieldIndex, int TypeIndex, int Size)
    : IMetadataRecord<Il2CppFieldMarshaledSize>
{
    public static int SizeOf(MetadataVersion v) => 12;
    public static Il2CppFieldMarshaledSize Read(ref SpanReader r, MetadataVersion v) => new(r.ReadInt32(), r.ReadInt32(), r.ReadInt32());

    public static Il2CppFieldMarshaledSize ReadCompact(ref SpanReader r, MetadataVersion v, in SerializedIndexSizes s) =>
        new(v.HasV108EmbeddedGenerics ? SerializedIndexSizes.ReadIndex(ref r, s.FieldIndex) : r.ReadInt32(),
            SerializedIndexSizes.ReadIndex(ref r, s.TypeIndex),
            r.ReadInt32());
}

public readonly record struct Il2CppPropertyDefinition(int NameIndex, int Get, int Set, uint Attrs, uint Token)
    : IMetadataRecord<Il2CppPropertyDefinition>
{
    public static int SizeOf(MetadataVersion v) => 16 + (v.HasCustomAttributeIndex ? 4 : 0) + (v.HasTokens ? 4 : 0);
    public static Il2CppPropertyDefinition Read(ref SpanReader r, MetadataVersion v)
    {
        int name = r.ReadInt32(), get = r.ReadInt32(), set = r.ReadInt32(); var attrs = r.ReadUInt32();
        if (v.HasCustomAttributeIndex) r.ReadInt32();
        return new(name, get, set, attrs, v.HasTokens ? r.ReadUInt32() : 0u);
    }

    public static Il2CppPropertyDefinition ReadCompact(ref SpanReader r, MetadataVersion v, in SerializedIndexSizes s)
    {
        var name = r.ReadInt32();
        var mSize = v.HasV108EmbeddedGenerics ? s.MethodIndex : 4;
        return new(name, SerializedIndexSizes.ReadIndex(ref r, mSize), SerializedIndexSizes.ReadIndex(ref r, mSize), r.ReadUInt32(), r.ReadUInt32());
    }
}

public readonly record struct Il2CppEventDefinition(int NameIndex, int TypeIndex, int Add, int Remove, int Raise, uint Token)
    : IMetadataRecord<Il2CppEventDefinition>
{
    public static int SizeOf(MetadataVersion v) => 20 + (v.HasCustomAttributeIndex ? 4 : 0) + (v.HasTokens ? 4 : 0);
    public static Il2CppEventDefinition Read(ref SpanReader r, MetadataVersion v)
    {
        int name = r.ReadInt32(), type = r.ReadInt32(), add = r.ReadInt32(), remove = r.ReadInt32(), raise = r.ReadInt32();
        if (v.HasCustomAttributeIndex) r.ReadInt32();
        return new(name, type, add, remove, raise, v.HasTokens ? r.ReadUInt32() : 0u);
    }

    public static Il2CppEventDefinition ReadCompact(ref SpanReader r, MetadataVersion v, in SerializedIndexSizes s)
    {
        var name = r.ReadInt32();
        var type = SerializedIndexSizes.ReadIndex(ref r, s.TypeIndex);
        var mSize = v.HasV108EmbeddedGenerics ? s.MethodIndex : 4;
        return new(name, type, SerializedIndexSizes.ReadIndex(ref r, mSize), SerializedIndexSizes.ReadIndex(ref r, mSize), SerializedIndexSizes.ReadIndex(ref r, mSize), r.ReadUInt32());
    }
}

public readonly record struct Il2CppGenericContainer(int OwnerIndex, int TypeArgc, int IsMethod, int GenericParameterStart)
    : IMetadataRecord<Il2CppGenericContainer>
{
    public static int SizeOf(MetadataVersion v) => 16;
    public static Il2CppGenericContainer Read(ref SpanReader r, MetadataVersion v) =>
        new(r.ReadInt32(), r.ReadInt32(), r.ReadInt32(), r.ReadInt32());

    public static Il2CppGenericContainer ReadCompact(ref SpanReader r, MetadataVersion v, in SerializedIndexSizes s) =>
        v.HasV108EmbeddedGenerics
            ? new(r.ReadInt32(), r.ReadUInt16(), r.ReadByte(), SerializedIndexSizes.ReadIndex(ref r, s.GenericParameterIndex))
            : Read(ref r, v);
}

public readonly record struct Il2CppGenericParameter(int OwnerIndex, int NameIndex, short ConstraintsStart, short ConstraintsCount, ushort Num, ushort Flags)
    : IMetadataRecord<Il2CppGenericParameter>
{
    public static int SizeOf(MetadataVersion v) => 16;
    public static Il2CppGenericParameter Read(ref SpanReader r, MetadataVersion v) =>
        new(r.ReadInt32(), r.ReadInt32(), r.ReadInt16(), r.ReadInt16(), r.ReadUInt16(), r.ReadUInt16());

    public static Il2CppGenericParameter ReadCompact(ref SpanReader r, in SerializedIndexSizes s) =>
        new(SerializedIndexSizes.ReadIndex(ref r, s.GenericContainerIndex), r.ReadInt32(), r.ReadInt16(), r.ReadInt16(), r.ReadUInt16(), r.ReadUInt16());
}

public readonly record struct Il2CppImageDefinition(
    int NameIndex, int AssemblyIndex, int TypeStart, uint TypeCount, int ExportedTypeStart, uint ExportedTypeCount,
    int EntryPointIndex, uint Token, int CustomAttributeStart, uint CustomAttributeCount)
    : IMetadataRecord<Il2CppImageDefinition>
{
    public static int SizeOf(MetadataVersion v) => 20
        + (v.HasExportedTypeDefinitions ? 8 : 0)
        + (v.HasTokens ? 4 : 0)
        + (v.HasImageAttributeRanges ? 8 : 0);

    public static Il2CppImageDefinition Read(ref SpanReader r, MetadataVersion v) => new(
        r.ReadInt32(), r.ReadInt32(), r.ReadInt32(), r.ReadUInt32(),
        v.HasExportedTypeDefinitions ? r.ReadInt32() : 0,
        v.HasExportedTypeDefinitions ? r.ReadUInt32() : 0u,
        r.ReadInt32(),
        v.HasTokens ? r.ReadUInt32() : 1u,
        v.HasImageAttributeRanges ? r.ReadInt32() : 0,
        v.HasImageAttributeRanges ? r.ReadUInt32() : 0u);

    public static Il2CppImageDefinition ReadCompact(ref SpanReader r, MetadataVersion v, in SerializedIndexSizes s)
    {
        var name = r.ReadInt32();
        var asm = r.ReadInt32();
        var typeStart = SerializedIndexSizes.ReadIndex(ref r, s.TypeDefinitionIndex);
        var typeCount = r.ReadUInt32();
        var expStart = SerializedIndexSizes.ReadIndex(ref r, s.TypeDefinitionIndex);
        var expCount = r.ReadUInt32();
        var entry = v.HasV108EmbeddedGenerics ? SerializedIndexSizes.ReadIndex(ref r, s.MethodIndex) : r.ReadInt32();
        var tok = r.ReadUInt32();
        var caStart = r.ReadInt32();
        var caCount = r.ReadUInt32();
        if (v.HasV108EmbeddedGenerics)
        {
            r.ReadUInt32(); // invokerIndicesStart
            r.ReadUInt32(); // rgctxRangesStart
            r.ReadUInt32(); // rgctxRangesCount
            SerializedIndexSizes.ReadIndex(ref r, s.TypeDefinitionIndex); // staticConstructorStart
            r.ReadUInt32(); // staticConstructorCount
        }
        return new(name, asm, typeStart, typeCount, expStart, expCount, entry, tok, caStart, caCount);
    }
}

public readonly record struct Il2CppAssemblyDefinition(
    int ImageIndex, uint Token, int ReferencedAssemblyStart, int ReferencedAssemblyCount,
    int NameIndex, int CultureIndex, int PublicKeyIndex, uint HashAlg, int HashLen, uint Flags,
    int Major, int Minor, int Build, int Revision, ulong PublicKeyToken)
    : IMetadataRecord<Il2CppAssemblyDefinition>
{
    public string VersionString => $"{Major}.{Minor}.{Build}.{Revision}";
    public string PublicKeyTokenHex => PublicKeyToken == 0 ? "null" : Convert.ToHexStringLower(BitConverter.GetBytes(PublicKeyToken));

    /// <summary>60 bytes for v16-19, 68 bytes for v20-24.3, 64 bytes for v24.4-v31, 68 bytes for v39+ (moduleToken).</summary>
    public static int SizeOf(MetadataVersion v) => 8
        + (v.HasAssemblyModuleToken ? 4 : 0)
        + (v.HasReferencedAssemblies ? 8 : 0)
        + 10 * 4 + 8
        + (v.HasHashValueIndex ? 4 : 0);

    public static Il2CppAssemblyDefinition Read(ref SpanReader r, MetadataVersion v)
    {
        var image = r.ReadInt32();
        var tokenOrAttr = r.ReadUInt32();   // token (24.1+) or customAttributeIndex (<= 24.0)
        if (v.HasAssemblyModuleToken) r.ReadUInt32();   // moduleToken
        int refStart = v.HasReferencedAssemblies ? r.ReadInt32() : 0;
        int refCount = v.HasReferencedAssemblies ? r.ReadInt32() : 0;
        int name = r.ReadInt32(), culture = r.ReadInt32();
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

    public static Il2CppFieldRef ReadCompact(ref SpanReader r, MetadataVersion v, in SerializedIndexSizes s) =>
        new(SerializedIndexSizes.ReadIndex(ref r, s.TypeIndex),
            v.HasV108EmbeddedGenerics ? SerializedIndexSizes.ReadIndex(ref r, s.FieldIndex) : r.ReadInt32());
}

public readonly record struct Il2CppInterfaceOffsetPair(int InterfaceTypeIndex, int Offset) : IMetadataRecord<Il2CppInterfaceOffsetPair>
{
    public static int SizeOf(MetadataVersion v) => 8;
    public static Il2CppInterfaceOffsetPair Read(ref SpanReader r, MetadataVersion v) => new(r.ReadInt32(), r.ReadInt32());

    public static Il2CppInterfaceOffsetPair ReadCompact(ref SpanReader r, in SerializedIndexSizes s) =>
        new(SerializedIndexSizes.ReadIndex(ref r, s.TypeIndex), r.ReadInt32());
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

    public static Il2CppWindowsRuntimeTypeNamePair ReadCompact(ref SpanReader r, in SerializedIndexSizes s) =>
        new(r.ReadInt32(), SerializedIndexSizes.ReadIndex(ref r, s.TypeIndex));
}

/// <summary>Plain int32 table element (nested types, interfaces, constraints, vtable slots, ...).</summary>
public readonly record struct Int32Element(int Value) : IMetadataRecord<Int32Element>
{
    public static int SizeOf(MetadataVersion v) => 4;
    public static Int32Element Read(ref SpanReader r, MetadataVersion v) => new(r.ReadInt32());
}
