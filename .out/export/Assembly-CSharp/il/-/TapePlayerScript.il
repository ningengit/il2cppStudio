.class public auto ansi beforefieldinit TapePlayerScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class [UnityEngine.CoreModule]UnityEngine.Sprite closedSprite
	.field public class [UnityEngine.CoreModule]UnityEngine.SpriteRenderer sprite
	.field public class BaldiScript baldi
	.field private class [UnityEngine.AudioModule]UnityEngine.AudioSource audioDevice

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x123b4
		// Header size: 12
		// Code size: 16 (0x10)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.AudioModule]UnityEngine.AudioSource
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.AudioModule]UnityEngine.AudioSource>()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.AudioModule]UnityEngine.AudioSource TapePlayerScript::audioDevice
		IL_000f: ret
	} // end of method TapePlayerScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x123d0
		// Header size: 12
		// Code size: 149 (0x95)
		.maxstack 3
		.locals init (
			[0] bool,
			[1] float32,
			[2] int32,
			[3] float32,
			[4] float32,
			[5] class BaldiScript,
			[6] int32,
			[7] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource TapePlayerScript::audioDevice
		IL_0007: callvirt instance bool [UnityEngine.AudioModule]UnityEngine.AudioSource::get_isPlaying()
		IL_000c: stloc.0
		IL_000d: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_timeScale()
		IL_0012: stloc.1
		IL_0013: ldloc.0
		IL_0014: stloc.2
		IL_0015: ldloc.1
		IL_0016: stloc.3
		IL_0017: nop
		IL_0018: ldloc.1
		IL_0019: ldc.r4 0.0
		IL_001e: ceq
		IL_0020: ldc.i4.0
		IL_0021: ceq
		IL_0023: brtrue.s IL_0028

		IL_0025: nop
		IL_0026: br.s IL_0029

		IL_0028: nop

		IL_0029: nop
		IL_002a: ldc.i4.0
		IL_002b: ldloc.2
		IL_002c: and
		IL_002d: ldc.i4.0
		IL_002e: ceq
		IL_0030: ldc.i4.0
		IL_0031: ceq
		IL_0033: brtrue.s IL_0085

		IL_0035: nop
		IL_0036: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_timeScale()
		IL_003b: stloc.s 4
		IL_003d: ldarg.0
		IL_003e: ldfld class BaldiScript TapePlayerScript::baldi
		IL_0043: stloc.s 5
		IL_0045: ldloc.s 4
		IL_0047: stloc.3
		IL_0048: ldloc.3
		IL_0049: ldc.r4 0.0
		IL_004e: cgt
		IL_0050: stloc.s 6
		IL_0052: ldloc.s 5
		IL_0054: ldfld float32 BaldiScript::antiHearingTime
		IL_0059: stloc.s 7
		IL_005b: ldloc.3
		IL_005c: ldc.r4 0.0
		IL_0061: cgt
		IL_0063: ldloc.s 5
		IL_0065: ldfld float32 BaldiScript::antiHearingTime
		IL_006a: ldc.r4 0.0
		IL_006f: cgt
		IL_0071: and
		IL_0072: ldc.i4.0
		IL_0073: ceq
		IL_0075: brtrue.s IL_0083

		IL_0077: nop
		IL_0078: ldarg.0
		IL_0079: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource TapePlayerScript::audioDevice
		IL_007e: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::UnPause()

		IL_0083: nop
		IL_0084: ret

		IL_0085: nop
		IL_0086: ldarg.0
		IL_0087: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource TapePlayerScript::audioDevice
		IL_008c: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Pause()
		IL_0091: ret

		IL_0092: nop
		IL_0093: ldnull
		IL_0094: throw
	} // end of method TapePlayerScript::Update

	.method public hidebysig 
		instance void Play () cil managed 
	{
		// Method begins at RVA 0x12474
		// Header size: 12
		// Code size: 72 (0x48)
		.maxstack 2
		.locals init (
			[0] bool,
			[1] bool
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.SpriteRenderer TapePlayerScript::sprite
		IL_0007: ldarg.0
		IL_0008: ldfld class [UnityEngine.CoreModule]UnityEngine.Sprite TapePlayerScript::closedSprite
		IL_000d: callvirt instance void [UnityEngine.CoreModule]UnityEngine.SpriteRenderer::set_sprite(class [UnityEngine.CoreModule]UnityEngine.Sprite)
		IL_0012: ldarg.0
		IL_0013: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource TapePlayerScript::audioDevice
		IL_0018: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Play()
		IL_001d: ldarg.0
		IL_001e: ldfld class BaldiScript TapePlayerScript::baldi
		IL_0023: callvirt instance bool [UnityEngine.CoreModule]UnityEngine.Behaviour::get_isActiveAndEnabled()
		IL_0028: stloc.0
		IL_0029: ldloc.0
		IL_002a: stloc.1
		IL_002b: ldloc.0
		IL_002c: ldc.i4.0
		IL_002d: ceq
		IL_002f: brtrue.s IL_0043

		IL_0031: nop
		IL_0032: ldarg.0
		IL_0033: ldfld class BaldiScript TapePlayerScript::baldi
		IL_0038: ldc.r4 30
		IL_003d: callvirt instance void BaldiScript::ActivateAntiHearing(float32)
		IL_0042: ret

		IL_0043: nop
		IL_0044: ret

		IL_0045: nop
		IL_0046: ldnull
		IL_0047: throw
	} // end of method TapePlayerScript::Play

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x269f
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method TapePlayerScript::.ctor

} // end of class TapePlayerScript
