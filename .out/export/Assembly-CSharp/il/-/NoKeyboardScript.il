.class public auto ansi beforefieldinit NoKeyboardScript
	extends [UnityEngine.UI]UnityEngine.UI.InputField
{
	// Methods
	.method family hidebysig virtual 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x24b5
		// Header size: 1
		// Code size: 19 (0x13)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i4.m1
		IL_0003: box [mscorlib]System.Int32
		IL_0008: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.TouchScreenKeyboardType>(object)
		IL_000d: callvirt instance void [UnityEngine.UI]UnityEngine.UI.InputField::set_keyboardType(valuetype [UnityEngine.CoreModule]UnityEngine.TouchScreenKeyboardType)
		IL_0012: ret
	} // end of method NoKeyboardScript::Start

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x24c9
		// Header size: 1
		// Code size: 12 (0xc)
		.maxstack 8

		IL_0000: nop
		IL_0001: br.s IL_0004

		IL_0003: nop

		IL_0004: nop
		IL_0005: ldarg.0
		IL_0006: callvirt instance void [UnityEngine.UI]UnityEngine.UI.InputField::.ctor()
		IL_000b: ret
	} // end of method NoKeyboardScript::.ctor

} // end of class NoKeyboardScript
