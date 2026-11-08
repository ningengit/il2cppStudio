.class public auto ansi beforefieldinit TapePlayerRecordingsScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class [UnityEngine.CoreModule]UnityEngine.Sprite closedSprite
	.field public class [UnityEngine.CoreModule]UnityEngine.SpriteRenderer sprite
	.field private int32 audVal
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip[] recordings
	.field public class BaldiScript baldi
	.field private class [UnityEngine.AudioModule]UnityEngine.AudioSource audioDevice

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x122a0
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
		IL_000a: stfld class [UnityEngine.AudioModule]UnityEngine.AudioSource TapePlayerRecordingsScript::audioDevice
		IL_000f: ret
	} // end of method TapePlayerRecordingsScript::Start

	.method public hidebysig 
		instance void Play () cil managed 
	{
		// Method begins at RVA 0x122bc
		// Header size: 12
		// Code size: 199 (0xc7)
		.maxstack 3
		.locals init (
			[0] float32,
			[1] int32,
			[2] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[3] int32,
			[4] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[5] bool,
			[6] bool,
			[7] class BaldiScript,
			[8] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[9] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[10] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[11] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.SpriteRenderer TapePlayerRecordingsScript::sprite
		IL_0007: ldarg.0
		IL_0008: ldfld class [UnityEngine.CoreModule]UnityEngine.Sprite TapePlayerRecordingsScript::closedSprite
		IL_000d: callvirt instance void [UnityEngine.CoreModule]UnityEngine.SpriteRenderer::set_sprite(class [UnityEngine.CoreModule]UnityEngine.Sprite)
		IL_0012: ldc.r4 0.0
		IL_0017: ldc.r4 4
		IL_001c: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_0021: stloc.0
		IL_0022: ldloc.0
		IL_0023: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_0028: stloc.1
		IL_0029: ldarg.0
		IL_002a: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] TapePlayerRecordingsScript::recordings
		IL_002f: stloc.2
		IL_0030: ldarg.0
		IL_0031: ldloc.1
		IL_0032: stfld int32 TapePlayerRecordingsScript::audVal
		IL_0037: ldloc.1
		IL_0038: stloc.3
		IL_0039: ldloc.2
		IL_003a: stloc.s 4
		IL_003c: ldarg.0
		IL_003d: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource TapePlayerRecordingsScript::audioDevice
		IL_0042: ldloc.s 4
		IL_0044: ldloc.3
		IL_0045: conv.i8
		IL_0046: conv.i4
		IL_0047: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_004c: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_0051: ldarg.0
		IL_0052: ldfld class BaldiScript TapePlayerRecordingsScript::baldi
		IL_0057: callvirt instance bool [UnityEngine.CoreModule]UnityEngine.Behaviour::get_isActiveAndEnabled()
		IL_005c: stloc.s 5
		IL_005e: ldloc.s 5
		IL_0060: stloc.s 6
		IL_0062: ldloc.s 5
		IL_0064: ldc.i4.0
		IL_0065: ceq
		IL_0067: brtrue.s IL_00bf

		IL_0069: nop
		IL_006a: ldarg.0
		IL_006b: ldfld class BaldiScript TapePlayerRecordingsScript::baldi
		IL_0070: stloc.s 7
		IL_0072: ldarg.0
		IL_0073: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0078: stloc.s 8
		IL_007a: ldloc.s 8
		IL_007c: stloc.s 9
		IL_007e: ldloc.s 9
		IL_0080: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0085: stloc.s 10
		IL_0087: ldloc.s 7
		IL_0089: ldloca.s 11
		IL_008b: ldloca.s 10
		IL_008d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0092: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0097: ldloca.s 11
		IL_0099: ldloca.s 10
		IL_009b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00a0: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00a5: ldloca.s 11
		IL_00a7: ldloca.s 10
		IL_00a9: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00ae: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00b3: ldloc.s 11
		IL_00b5: ldc.r4 4
		IL_00ba: callvirt instance void BaldiScript::Hear(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, float32)

		IL_00bf: nop
		IL_00c0: ret

		IL_00c1: nop
		IL_00c2: ldnull
		IL_00c3: throw

		IL_00c4: nop
		IL_00c5: ldnull
		IL_00c6: throw
	} // end of method TapePlayerRecordingsScript::Play

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x12390
		// Header size: 12
		// Code size: 22 (0x16)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.AudioModule]UnityEngine.AudioClip[]
		)

		IL_0000: nop
		IL_0001: ldc.i4.5
		IL_0002: newarr [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] TapePlayerRecordingsScript::recordings
		IL_000f: ldarg.0
		IL_0010: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0015: ret
	} // end of method TapePlayerRecordingsScript::.ctor

} // end of class TapePlayerRecordingsScript
