.class public auto ansi beforefieldinit WarningScreenScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public string LoadScene

	// Methods
	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x13f64
		// Header size: 12
		// Code size: 36 (0x24)
		.maxstack 2
		.locals init (
			[0] bool,
			[1] bool,
			[2] string
		)

		IL_0000: nop
		IL_0001: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::get_anyKeyDown()
		IL_0006: stloc.0
		IL_0007: ldloc.0
		IL_0008: stloc.1
		IL_0009: ldloc.0
		IL_000a: ldc.i4.0
		IL_000b: ceq
		IL_000d: brtrue.s IL_0022

		IL_000f: nop
		IL_0010: ldarg.0
		IL_0011: ldfld string WarningScreenScript::LoadScene
		IL_0016: stloc.2
		IL_0017: br.s IL_001a

		IL_0019: nop

		IL_001a: nop
		IL_001b: ldloc.2
		IL_001c: call void [UnityEngine.CoreModule]UnityEngine.SceneManagement.SceneManager::LoadScene(string)
		IL_0021: ret

		IL_0022: nop
		IL_0023: ret
	} // end of method WarningScreenScript::Update

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2755
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method WarningScreenScript::.ctor

} // end of class WarningScreenScript
