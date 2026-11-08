.class public auto ansi beforefieldinit Script
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioSource audioDevice
	.field private bool played

	// Methods
	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0xf504
		// Header size: 12
		// Code size: 62 (0x3e)
		.maxstack 3
		.locals init (
			[0] bool,
			[1] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource Script::audioDevice
		IL_0007: callvirt instance bool [UnityEngine.AudioModule]UnityEngine.AudioSource::get_isPlaying()
		IL_000c: stloc.0
		IL_000d: ldloc.0
		IL_000e: ldc.i4.1
		IL_000f: xor
		IL_0010: stloc.1
		IL_0011: ldarg.0
		IL_0012: ldfld bool Script::played
		IL_0017: ldloc.0
		IL_0018: ldc.i4.1
		IL_0019: xor
		IL_001a: and
		IL_001b: ldc.i4.0
		IL_001c: ceq
		IL_001e: brtrue.s IL_0039

		IL_0020: nop
		IL_0021: br.s IL_0024

		IL_0023: nop

		IL_0024: nop
		IL_0025: call void [UnityEngine.CoreModule]UnityEngine.Application::Quit()
		IL_002a: br.s IL_002d

		IL_002c: nop

		IL_002d: nop
		IL_002e: ldstr "The game is closed."
		IL_0033: call void [UnityEngine.CoreModule]UnityEngine.Debug::Log(object)
		IL_0038: ret

		IL_0039: nop
		IL_003a: ret

		IL_003b: nop
		IL_003c: ldnull
		IL_003d: throw
	} // end of method Script::Update

	.method private hidebysig 
		instance void OnTriggerEnter (
			class [UnityEngine.PhysicsModule]UnityEngine.Collider other
		) cil managed 
	{
		// Method begins at RVA 0xf550
		// Header size: 12
		// Code size: 71 (0x47)
		.maxstack 2
		.locals init (
			[0] string,
			[1] bool,
			[2] int32,
			[3] int32
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: ldstr "Player"
		IL_000e: call bool [mscorlib]System.String::Equals(string, string)
		IL_0013: stloc.1
		IL_0014: ldarg.0
		IL_0015: ldfld bool Script::played
		IL_001a: ldc.i4.0
		IL_001b: ceq
		IL_001d: brfalse.s IL_0023

		IL_001f: ldloc.1
		IL_0020: stloc.2
		IL_0021: br.s IL_0026

		IL_0023: nop
		IL_0024: ldc.i4.0
		IL_0025: stloc.2

		IL_0026: nop
		IL_0027: ldloc.2
		IL_0028: stloc.3
		IL_0029: ldloc.2
		IL_002a: ldc.i4.0
		IL_002b: ceq
		IL_002d: brtrue.s IL_0042

		IL_002f: nop
		IL_0030: ldarg.0
		IL_0031: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource Script::audioDevice
		IL_0036: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Play()
		IL_003b: ldarg.0
		IL_003c: ldc.i4.1
		IL_003d: stfld bool Script::played

		IL_0042: nop
		IL_0043: ret

		IL_0044: nop
		IL_0045: ldnull
		IL_0046: throw
	} // end of method Script::OnTriggerEnter

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2591
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method Script::.ctor

} // end of class Script
