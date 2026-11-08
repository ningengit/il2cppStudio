.class public auto ansi beforefieldinit ExitButtonScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Methods
	.method public hidebysig 
		instance void ExitGame () cil managed 
	{
		// Method begins at RVA 0x2211
		// Header size: 1
		// Code size: 11 (0xb)
		.maxstack 8

		IL_0000: nop
		IL_0001: br.s IL_0004

		IL_0003: nop

		IL_0004: nop
		IL_0005: call void [UnityEngine.CoreModule]UnityEngine.Application::Quit()
		IL_000a: ret
	} // end of method ExitButtonScript::ExitGame

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x221d
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method ExitButtonScript::.ctor

} // end of class ExitButtonScript
