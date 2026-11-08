.class public auto ansi beforefieldinit ExitTriggerScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class GameControllerScript gc
	.field public string ResultsScene
	.field public string SecretScene

	// Methods
	.method private hidebysig 
		instance void OnTriggerEnter (
			class [UnityEngine.PhysicsModule]UnityEngine.Collider other
		) cil managed 
	{
		// Method begins at RVA 0x1062c
		// Header size: 12
		// Code size: 115 (0x73)
		.maxstack 2
		.locals init (
			[0] int32,
			[1] string,
			[2] bool,
			[3] int32,
			[4] int32,
			[5] string
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class GameControllerScript ExitTriggerScript::gc
		IL_0007: ldfld int32 GameControllerScript::notebooks
		IL_000c: stloc.0
		IL_000d: ldarg.1
		IL_000e: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Component::get_tag()
		IL_0013: stloc.1
		IL_0014: ldloc.1
		IL_0015: ldstr "Player"
		IL_001a: call bool [mscorlib]System.String::Equals(string, string)
		IL_001f: stloc.2
		IL_0020: ldloc.0
		IL_0021: ldc.i4.7
		IL_0022: clt
		IL_0024: ldc.i4.0
		IL_0025: ceq
		IL_0027: brfalse.s IL_002d

		IL_0029: ldloc.2
		IL_002a: stloc.3
		IL_002b: br.s IL_0030

		IL_002d: nop
		IL_002e: ldc.i4.0
		IL_002f: stloc.3

		IL_0030: nop
		IL_0031: ldloc.2
		IL_0032: stloc.s 4
		IL_0034: ldloc.3
		IL_0035: ldc.i4.0
		IL_0036: ceq
		IL_0038: brtrue.s IL_0063

		IL_003a: nop
		IL_003b: ldarg.0
		IL_003c: ldfld class GameControllerScript ExitTriggerScript::gc
		IL_0041: ldfld int32 GameControllerScript::failedNotebooks
		IL_0046: ldc.i4.7
		IL_0047: clt
		IL_0049: ldc.i4.0
		IL_004a: ceq
		IL_004c: brtrue.s IL_0065

		IL_004e: nop
		IL_004f: ldarg.0
		IL_0050: ldfld string ExitTriggerScript::ResultsScene
		IL_0055: stloc.s 5

		IL_0057: nop
		IL_0058: br.s IL_005b

		IL_005a: nop

		IL_005b: nop
		IL_005c: ldloc.s 5
		IL_005e: call void [UnityEngine.CoreModule]UnityEngine.SceneManagement.SceneManager::LoadScene(string)

		IL_0063: nop
		IL_0064: ret

		IL_0065: nop
		IL_0066: ldarg.0
		IL_0067: ldfld string ExitTriggerScript::SecretScene
		IL_006c: stloc.s 5
		IL_006e: br.s IL_0057

		IL_0070: nop
		IL_0071: ldnull
		IL_0072: throw
	} // end of method ExitTriggerScript::OnTriggerEnter

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x260f
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method ExitTriggerScript::.ctor

} // end of class ExitTriggerScript
