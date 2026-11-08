.class public auto ansi beforefieldinit AmbienceScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform aiLocation
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip[] sounds
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioSource audioDevice

	// Methods
	.method public hidebysig 
		instance void PlayAudio () cil managed 
	{
		// Method begins at RVA 0x2fa8
		// Header size: 12
		// Code size: 220 (0xdc)
		.maxstack 3
		.locals init (
			[0] float32,
			[1] int32,
			[2] int32,
			[3] bool,
			[4] int32,
			[5] bool,
			[6] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[7] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[8] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[9] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[10] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[11] float32,
			[12] int32,
			[13] int32
		)

		IL_0000: nop
		IL_0001: ldc.r4 0.0
		IL_0006: ldc.r4 49
		IL_000b: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_0017: stloc.1
		IL_0018: ldloc.1
		IL_0019: stloc.2
		IL_001a: ldarg.0
		IL_001b: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource AmbienceScript::audioDevice
		IL_0020: callvirt instance bool [UnityEngine.AudioModule]UnityEngine.AudioSource::get_isPlaying()
		IL_0025: stloc.3
		IL_0026: ldloc.1
		IL_0027: ldc.i4.0
		IL_0028: ceq
		IL_002a: brfalse.s IL_0033

		IL_002c: ldloc.3
		IL_002d: ldc.i4.1
		IL_002e: xor
		IL_002f: stloc.s 4
		IL_0031: br.s IL_0037

		IL_0033: nop
		IL_0034: ldc.i4.0
		IL_0035: stloc.s 4

		IL_0037: nop
		IL_0038: ldloc.3
		IL_0039: stloc.s 5
		IL_003b: ldloc.s 4
		IL_003d: ldc.i4.0
		IL_003e: ceq
		IL_0040: brtrue IL_00d4

		IL_0045: nop
		IL_0046: ldarg.0
		IL_0047: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_004c: stloc.s 6
		IL_004e: ldloc.s 6
		IL_0050: stloc.s 7
		IL_0052: ldarg.0
		IL_0053: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform AmbienceScript::aiLocation
		IL_0058: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_005d: stloc.s 8
		IL_005f: ldloc.s 6
		IL_0061: ldloca.s 9
		IL_0063: ldloca.s 8
		IL_0065: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_006a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_006f: ldloca.s 9
		IL_0071: ldloca.s 8
		IL_0073: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0078: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_007d: ldloca.s 9
		IL_007f: ldloca.s 8
		IL_0081: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0086: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_008b: ldloc.s 9
		IL_008d: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0092: ldarg.0
		IL_0093: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] AmbienceScript::sounds
		IL_0098: stloc.s 10
		IL_009a: ldc.r4 0.0
		IL_009f: ldloc.s 10
		IL_00a1: ldlen
		IL_00a2: conv.i4
		IL_00a3: ldc.i4.1
		IL_00a4: sub
		IL_00a5: conv.r4
		IL_00a6: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_00ab: stloc.s 11
		IL_00ad: ldloc.s 11
		IL_00af: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_00b4: stloc.s 12
		IL_00b6: ldloc.s 12
		IL_00b8: stloc.s 13
		IL_00ba: ldarg.0
		IL_00bb: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource AmbienceScript::audioDevice
		IL_00c0: ldarg.0
		IL_00c1: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] AmbienceScript::sounds
		IL_00c6: ldloc.s 13
		IL_00c8: conv.i8
		IL_00c9: conv.i4
		IL_00ca: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_00cf: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)

		IL_00d4: nop
		IL_00d5: ret

		IL_00d6: nop
		IL_00d7: ldnull
		IL_00d8: throw

		IL_00d9: nop
		IL_00da: ldnull
		IL_00db: throw
	} // end of method AmbienceScript::PlayAudio

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2050
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method AmbienceScript::.ctor

} // end of class AmbienceScript
