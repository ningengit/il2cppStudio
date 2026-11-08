.class public sequential ansi sealed serializable beforefieldinit InputBinding
	extends [mscorlib]System.ValueType
{
	// Fields
	.field public valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode primaryKey
	.field public valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode secondaryKey

	// Methods
	.method public hidebysig specialname rtspecialname 
		instance void .ctor (
			valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode primary,
			[opt] valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode secondary
		) cil managed 
	{
		// Method begins at RVA 0x22c9
		// Header size: 1
		// Code size: 56 (0x38)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldobj InputBinding
		IL_0007: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode InputBinding::primaryKey
		IL_000c: ldarg.1
		IL_000d: box [mscorlib]System.Object
		IL_0012: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0017: stfld int32 [UnityEngine.CoreModule]UnityEngine.KeyCode::value__
		IL_001c: ldarg.0
		IL_001d: ldobj InputBinding
		IL_0022: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode InputBinding::secondaryKey
		IL_0027: ldarg.2
		IL_0028: box [mscorlib]System.Object
		IL_002d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0032: stfld int32 [UnityEngine.CoreModule]UnityEngine.KeyCode::value__
		IL_0037: ret
	} // end of method InputBinding::.ctor

} // end of class InputBinding
