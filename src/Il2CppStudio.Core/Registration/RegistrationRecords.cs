namespace Il2CppStudio.Core.Registration;

/// <summary>Il2CppTypeEnum (il2cpp-blob.h).</summary>
public enum Il2CppTypeEnum : byte
{
    End = 0x00, Void = 0x01, Boolean = 0x02, Char = 0x03, I1 = 0x04, U1 = 0x05, I2 = 0x06, U2 = 0x07,
    I4 = 0x08, U4 = 0x09, I8 = 0x0A, U8 = 0x0B, R4 = 0x0C, R8 = 0x0D, String = 0x0E, Ptr = 0x0F,
    ByRef = 0x10, ValueType = 0x11, Class = 0x12, Var = 0x13, Array = 0x14, GenericInst = 0x15,
    TypedByRef = 0x16, I = 0x18, U = 0x19, FnPtr = 0x1B, Object = 0x1C, SzArray = 0x1D, MVar = 0x1E,
    CModReqd = 0x1F, CModOpt = 0x20, Internal = 0x21, Modifier = 0x40, Sentinel = 0x41, Pinned = 0x45, Enum = 0x55,
    Il2CppTypeIndex = 0xFF,
}

/// <summary>
/// One Il2CppType as stored in the binary (16 bytes on 64-bit). <see cref="Data"/> is a TypeDefinitionIndex
/// for CLASS/VALUETYPE, a GenericParameterIndex for VAR/MVAR, or a VA for PTR/SZARRAY/ARRAY/GENERICINST.
/// </summary>
public readonly record struct Il2CppTypeRecord(ulong Va, ulong Data, uint Bits)
{
    public ushort Attrs => (ushort)(Bits & 0xFFFF);
    public Il2CppTypeEnum Type => (Il2CppTypeEnum)((Bits >> 16) & 0xFF);
    public int NumMods => (int)((Bits >> 24) & 0x1F);
    public bool ByRef => ((Bits >> 29) & 1) != 0;
    public bool Pinned => ((Bits >> 30) & 1) != 0;
    public bool IsValueTypeFlag => ((Bits >> 31) & 1) != 0;
    public int KlassIndex => unchecked((int)Data);
    public int GenericParameterIndex => unchecked((int)Data);
}

/// <summary>Il2CppGenericClass: generic type definition + class instantiation.</summary>
public sealed record Il2CppGenericClassRecord(ulong Va, ulong TypeVa, ulong ClassInstVa, ulong MethodInstVa);

/// <summary>Il2CppGenericInst: type argument list (as Il2CppType VAs).</summary>
public sealed record Il2CppGenericInstRecord(ulong Va, IReadOnlyList<ulong> TypeArgVas);

public sealed record Il2CppArrayTypeRecord(ulong Va, ulong ElementTypeVa, byte Rank);

public readonly record struct Il2CppMethodSpecRecord(int MethodDefinitionIndex, int ClassInstIndex, int MethodInstIndex);

public readonly record struct Il2CppGenericMethodFunction(int GenericMethodIndex, int MethodIndex, int InvokerIndex, int AdjustorThunkIndex);

/// <summary>One Il2CppCodeGenModule: the method pointer table of one assembly (indexed by token RID - 1).</summary>
public sealed record CodeGenModuleRecord(ulong Va, string Name, IReadOnlyList<ulong> MethodPointers);

/// <summary>Metadata usage kinds encoded in runtime-metadata globals (v27+): (kind &lt;&lt; 29) | (index &lt;&lt; 1) | 1.</summary>
public enum MetadataUsageKind : byte
{
    Invalid = 0, TypeInfo = 1, Il2CppType = 2, MethodDef = 3, FieldInfo = 4, StringLiteral = 5, MethodRef = 6, FieldRva = 7,
}

public readonly record struct MetadataUsage(ulong Va, MetadataUsageKind Kind, int Index);
