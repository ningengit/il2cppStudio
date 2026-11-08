.class private auto ansi beforefieldinit UnitySourceGeneratedAssemblyMonoScriptTypes_v1
	extends [mscorlib]System.Object
{
	// Nested Types
	.class nested private sequential ansi sealed beforefieldinit MonoScriptData
		extends [mscorlib]System.ValueType
	{
		// Fields
		.field public uint8[] FilePathsData
		.field public uint8[] TypesData
		.field public int32 TotalTypes
		.field public int32 TotalFiles
		.field public bool IsEditorOnly

	} // end of class MonoScriptData


	// Methods
	.method private hidebysig static 
		valuetype UnitySourceGeneratedAssemblyMonoScriptTypes_v1/MonoScriptData Get () cil managed aggressiveinlining 
	{
		// Method begins at RVA 0x14218
		// Header size: 12
		// Code size: 149 (0x95)
		.maxstack 2
		.locals init (
			[0] valuetype UnitySourceGeneratedAssemblyMonoScriptTypes_v1/MonoScriptData,
			[1] uint8[],
			[2] uint8[]
		)

		IL_0000: nop
		IL_0001: ldloca.s 0
		IL_0003: ldc.r4 0.0
		IL_0008: box [mscorlib]System.Single
		IL_000d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<uint8[]>(object)
		IL_0012: stfld uint8[] [Unity.TextMeshPro]UnitySourceGeneratedAssemblyMonoScriptTypes_v1/MonoScriptData::FilePathsData
		IL_0017: ldloca.s 0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 [Unity.TextMeshPro]UnitySourceGeneratedAssemblyMonoScriptTypes_v1/MonoScriptData::TotalTypes
		IL_001f: ldc.i4 6521
		IL_0024: newarr [mscorlib]System.Byte
		IL_0029: stloc.1
		IL_002a: ldloc.1
		IL_002b: ldstr "<PrivateImplementationDetails>::4FD1BA63F59EF28DCC0AE3B817277254C4FED9EACF574BC730F7F8947CB170F4_FieldInfo"
		IL_0030: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0035: box [mscorlib]System.IntPtr
		IL_003a: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [mscorlib]System.RuntimeFieldHandle>(object)
		IL_003f: call void [mscorlib]System.Runtime.CompilerServices.RuntimeHelpers::InitializeArray(class [mscorlib]System.Array, valuetype [mscorlib]System.RuntimeFieldHandle)
		IL_0044: ldloca.s 0
		IL_0046: ldloc.1
		IL_0047: stfld uint8[] [Unity.TextMeshPro]UnitySourceGeneratedAssemblyMonoScriptTypes_v1/MonoScriptData::FilePathsData
		IL_004c: ldc.i4 3020
		IL_0051: newarr [mscorlib]System.Byte
		IL_0056: stloc.2
		IL_0057: ldloc.2
		IL_0058: ldstr "<PrivateImplementationDetails>::EAA1865ADC35520EC9A936D69C0A170C00A36B478E3737217DBC0E517A8297CC_FieldInfo"
		IL_005d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0062: box [mscorlib]System.IntPtr
		IL_0067: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [mscorlib]System.RuntimeFieldHandle>(object)
		IL_006c: call void [mscorlib]System.Runtime.CompilerServices.RuntimeHelpers::InitializeArray(class [mscorlib]System.Array, valuetype [mscorlib]System.RuntimeFieldHandle)
		IL_0071: ldloca.s 0
		IL_0073: ldloc.2
		IL_0074: stfld uint8[] [Unity.TextMeshPro]UnitySourceGeneratedAssemblyMonoScriptTypes_v1/MonoScriptData::TypesData
		IL_0079: ldloca.s 0
		IL_007b: ldc.i4.s 106
		IL_007d: stfld int32 [Unity.TextMeshPro]UnitySourceGeneratedAssemblyMonoScriptTypes_v1/MonoScriptData::TotalFiles
		IL_0082: ldloca.s 0
		IL_0084: ldc.i4.s 114
		IL_0086: stfld int32 [Unity.TextMeshPro]UnitySourceGeneratedAssemblyMonoScriptTypes_v1/MonoScriptData::TotalTypes
		IL_008b: ldloca.s 0
		IL_008d: ldc.i4.0
		IL_008e: stfld bool [Unity.TextMeshPro]UnitySourceGeneratedAssemblyMonoScriptTypes_v1/MonoScriptData::IsEditorOnly
		IL_0093: ldloc.0
		IL_0094: ret
	} // end of method UnitySourceGeneratedAssemblyMonoScriptTypes_v1::Get

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2787
		// Header size: 1
		// Code size: 9 (0x9)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldnull
		IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0008: ret
	} // end of method UnitySourceGeneratedAssemblyMonoScriptTypes_v1::.ctor

} // end of class UnitySourceGeneratedAssemblyMonoScriptTypes_v1
