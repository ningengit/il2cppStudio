.class public auto ansi beforefieldinit NeedMoreScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class GameControllerScript gc
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioSource audioDevice
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip baldiDoor

	// Methods
	.method private hidebysig 
		instance void OnTriggerEnter (
			class [UnityEngine.PhysicsModule]UnityEngine.Collider other
		) cil managed 
	{
		// Method begins at RVA 0xb3ac
		// Header size: 12
		// Code size: 83 (0x53)
		.maxstack 3
		.locals init (
			[0] int32,
			[1] string,
			[2] bool,
			[3] int32,
			[4] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class GameControllerScript NeedMoreScript::gc
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
		IL_0021: ldc.i4.2
		IL_0022: clt
		IL_0024: brfalse.s IL_002a

		IL_0026: ldloc.2
		IL_0027: stloc.3
		IL_0028: br.s IL_002d

		IL_002a: nop
		IL_002b: ldc.i4.0
		IL_002c: stloc.3

		IL_002d: nop
		IL_002e: ldloc.2
		IL_002f: stloc.s 4
		IL_0031: ldloc.3
		IL_0032: ldc.i4.0
		IL_0033: ceq
		IL_0035: brtrue.s IL_004e

		IL_0037: nop
		IL_0038: ldarg.0
		IL_0039: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource NeedMoreScript::audioDevice
		IL_003e: ldarg.0
		IL_003f: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip NeedMoreScript::baldiDoor
		IL_0044: ldc.r4 1
		IL_0049: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip, float32)

		IL_004e: nop
		IL_004f: ret

		IL_0050: nop
		IL_0051: ldnull
		IL_0052: throw
	} // end of method NeedMoreScript::OnTriggerEnter

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x24ac
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method NeedMoreScript::.ctor

} // end of class NeedMoreScript
