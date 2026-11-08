.class public auto ansi beforefieldinit CursorControllerScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Methods
	.method public hidebysig 
		instance void LockCursor () cil managed 
	{
		// Method begins at RVA 0x206c
		// Header size: 1
		// Code size: 24 (0x18)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldc.i4.1
		IL_0002: box [mscorlib]System.Int32
		IL_0007: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode>(object)
		IL_000c: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_lockState(valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode)
		IL_0011: ldc.i4.0
		IL_0012: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_visible(bool)
		IL_0017: ret
	} // end of method CursorControllerScript::LockCursor

	.method public hidebysig 
		instance void UnlockCursor () cil managed 
	{
		// Method begins at RVA 0x2085
		// Header size: 1
		// Code size: 24 (0x18)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldc.i4.0
		IL_0002: box [mscorlib]System.Int32
		IL_0007: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode>(object)
		IL_000c: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_lockState(valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode)
		IL_0011: ldc.i4.1
		IL_0012: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_visible(bool)
		IL_0017: ret
	} // end of method CursorControllerScript::UnlockCursor

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x209e
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method CursorControllerScript::.ctor

} // end of class CursorControllerScript
