.class public auto ansi beforefieldinit JumpRopeScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class [Unity.TextMeshPro]TMPro.TMP_Text jumpCount
	.field public class [UnityEngine.AnimationModule]UnityEngine.Animator rope
	.field public class CameraScript cs
	.field public class PlayerScript ps
	.field public class PlaytimeScript playtime
	.field public int32 jumps
	.field public float32 jumpDelay
	.field public float32 ropePosition
	.field public bool ropeHit
	.field public bool jumpStarted

	// Methods
	.method private hidebysig 
		instance void OnEnable () cil managed 
	{
		// Method begins at RVA 0x5c08
		// Header size: 12
		// Code size: 108 (0x6c)
		.maxstack 2
		.locals init (
			[0] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[1] int32,
			[2] string,
			[3] string
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text JumpRopeScript::jumpCount
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldc.r4 1
		IL_000e: stfld float32 JumpRopeScript::jumpDelay
		IL_0013: ldarg.0
		IL_0014: ldc.i4.1
		IL_0015: stfld bool JumpRopeScript::ropeHit
		IL_001a: ldarg.0
		IL_001b: ldc.i4.0
		IL_001c: stfld int32 JumpRopeScript::jumps
		IL_0021: ldloca.s 1
		IL_0023: ldc.i4.0
		IL_0024: stfld int32 [mscorlib]System.Int32::m_value
		IL_0029: ldloca.s 1
		IL_002b: call instance string [mscorlib]System.Int32::ToString()
		IL_0030: stloc.2
		IL_0031: ldloc.2
		IL_0032: ldstr "/5"
		IL_0037: call string [mscorlib]System.String::Concat(string, string)
		IL_003c: stloc.3
		IL_003d: ldarg.0
		IL_003e: ldfld class CameraScript JumpRopeScript::cs
		IL_0043: ldc.r4 0.0
		IL_0048: stfld float32 CameraScript::jumpHeight
		IL_004d: ldarg.0
		IL_004e: ldfld class PlaytimeScript JumpRopeScript::playtime
		IL_0053: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource PlaytimeScript::audioDevice
		IL_0058: ldarg.0
		IL_0059: ldfld class PlaytimeScript JumpRopeScript::playtime
		IL_005e: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip PlaytimeScript::aud_ReadyGo
		IL_0063: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_0068: ret

		IL_0069: nop
		IL_006a: ldnull
		IL_006b: throw
	} // end of method JumpRopeScript::OnEnable

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x5c80
		// Header size: 12
		// Code size: 169 (0xa9)
		.maxstack 3
		.locals init (
			[0] float32,
			[1] class [UnityEngine.AnimationModule]UnityEngine.Animator,
			[2] float32,
			[3] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld float32 JumpRopeScript::jumpDelay
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld float32 JumpRopeScript::jumpDelay
		IL_000e: ldc.r4 0.0
		IL_0013: cgt
		IL_0015: brtrue.s IL_0054

		IL_0017: nop
		IL_0018: ldarg.0
		IL_0019: ldfld bool JumpRopeScript::jumpStarted
		IL_001e: ldc.i4.0
		IL_001f: ceq
		IL_0021: ldc.i4.0
		IL_0022: ceq
		IL_0024: brtrue.s IL_0064

		IL_0026: nop
		IL_0027: ldarg.0
		IL_0028: ldfld class [UnityEngine.AnimationModule]UnityEngine.Animator JumpRopeScript::rope
		IL_002d: stloc.1
		IL_002e: ldarg.0
		IL_002f: ldc.i4.1
		IL_0030: stfld bool JumpRopeScript::jumpStarted
		IL_0035: ldarg.0
		IL_0036: ldc.r4 1
		IL_003b: stfld float32 JumpRopeScript::ropePosition
		IL_0040: ldloc.1
		IL_0041: ldstr "ActivateJumpRope"
		IL_0046: callvirt instance void [UnityEngine.AnimationModule]UnityEngine.Animator::SetTrigger(string)
		IL_004b: ldarg.0
		IL_004c: ldc.i4.0
		IL_004d: stfld bool JumpRopeScript::ropeHit
		IL_0052: br.s IL_0064

		IL_0054: nop
		IL_0055: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_005a: stloc.2
		IL_005b: ldarg.0
		IL_005c: ldloc.0
		IL_005d: ldloc.2
		IL_005e: sub
		IL_005f: stfld float32 JumpRopeScript::jumpDelay

		IL_0064: nop
		IL_0065: ldarg.0
		IL_0066: ldfld float32 JumpRopeScript::ropePosition
		IL_006b: stloc.0
		IL_006c: ldarg.0
		IL_006d: ldfld float32 JumpRopeScript::ropePosition
		IL_0072: ldc.r4 0.0
		IL_0077: cgt
		IL_0079: brtrue.s IL_0092

		IL_007b: nop
		IL_007c: ldarg.0
		IL_007d: ldfld bool JumpRopeScript::ropeHit
		IL_0082: ldc.i4.0
		IL_0083: ceq
		IL_0085: ldc.i4.0
		IL_0086: ceq
		IL_0088: brtrue.s IL_00a4

		IL_008a: nop
		IL_008b: ldarg.0
		IL_008c: callvirt instance void JumpRopeScript::RopeHit()
		IL_0091: ret

		IL_0092: nop
		IL_0093: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0098: stloc.3
		IL_0099: ldarg.0
		IL_009a: ldloc.0
		IL_009b: ldloc.3
		IL_009c: sub
		IL_009d: stfld float32 JumpRopeScript::ropePosition
		IL_00a2: ldloc.3
		IL_00a3: stloc.0

		IL_00a4: nop
		IL_00a5: ret

		IL_00a6: nop
		IL_00a7: ldnull
		IL_00a8: throw
	} // end of method JumpRopeScript::Update

	.method private hidebysig 
		instance void RopeHit () cil managed 
	{
		// Method begins at RVA 0x5d38
		// Header size: 12
		// Code size: 364 (0x16c)
		.maxstack 3
		.locals init (
			[0] class CameraScript,
			[1] class PlaytimeScript,
			[2] int64,
			[3] class [UnityEngine.AudioModule]UnityEngine.AudioSource,
			[4] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[5] string,
			[6] string,
			[7] string,
			[8] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[9] string,
			[10] string,
			[11] class PlaytimeScript
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class CameraScript JumpRopeScript::cs
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldc.i4.1
		IL_000a: stfld bool JumpRopeScript::ropeHit
		IL_000f: ldc.r4 0.2
		IL_0014: ldloc.0
		IL_0015: ldfld float32 CameraScript::jumpHeight
		IL_001a: clt
		IL_001c: ldc.i4.0
		IL_001d: ceq
		IL_001f: brtrue IL_00fa

		IL_0024: nop
		IL_0025: ldarg.0
		IL_0026: ldfld class PlaytimeScript JumpRopeScript::playtime
		IL_002b: stloc.1
		IL_002c: ldloc.1
		IL_002d: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource PlaytimeScript::audioDevice
		IL_0032: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Stop()
		IL_0037: ldarg.0
		IL_0038: ldfld int32 JumpRopeScript::jumps
		IL_003d: conv.i8
		IL_003e: stloc.2
		IL_003f: ldarg.0
		IL_0040: ldfld class PlaytimeScript JumpRopeScript::playtime
		IL_0045: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource PlaytimeScript::audioDevice
		IL_004a: stloc.3
		IL_004b: ldloc.3
		IL_004c: ldarg.0
		IL_004d: ldfld class PlaytimeScript JumpRopeScript::playtime
		IL_0052: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] PlaytimeScript::aud_Numbers
		IL_0057: ldloc.2
		IL_0058: conv.i4
		IL_0059: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_005e: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_0063: ldarg.0
		IL_0064: ldarg.0
		IL_0065: ldfld int32 JumpRopeScript::jumps
		IL_006a: ldc.i4.1
		IL_006b: add
		IL_006c: stfld int32 JumpRopeScript::jumps
		IL_0071: ldarg.0
		IL_0072: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text JumpRopeScript::jumpCount
		IL_0077: stloc.s 4
		IL_0079: ldarg.0
		IL_007a: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_007f: ldc.i8 72
		IL_0088: add
		IL_0089: call instance string [mscorlib]System.Int32::ToString()
		IL_008e: stloc.s 5
		IL_0090: ldloc.s 5
		IL_0092: ldstr "/5"
		IL_0097: call string [mscorlib]System.String::Concat(string, string)
		IL_009c: stloc.s 6
		IL_009e: ldloc.s 6
		IL_00a0: stloc.s 7
		IL_00a2: ldarg.0
		IL_00a3: ldc.r4 0.5
		IL_00a8: stfld float32 JumpRopeScript::jumpDelay
		IL_00ad: ldarg.0
		IL_00ae: ldfld int32 JumpRopeScript::jumps
		IL_00b3: ldc.i4.5
		IL_00b4: clt
		IL_00b6: brtrue IL_015d

		IL_00bb: nop
		IL_00bc: ldarg.0
		IL_00bd: ldfld class PlaytimeScript JumpRopeScript::playtime
		IL_00c2: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource PlaytimeScript::audioDevice
		IL_00c7: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Stop()
		IL_00cc: ldarg.0
		IL_00cd: ldfld class PlaytimeScript JumpRopeScript::playtime
		IL_00d2: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource PlaytimeScript::audioDevice
		IL_00d7: ldarg.0
		IL_00d8: ldfld class PlaytimeScript JumpRopeScript::playtime
		IL_00dd: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip PlaytimeScript::aud_Congrats
		IL_00e2: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_00e7: ldarg.0
		IL_00e8: ldfld class PlayerScript JumpRopeScript::ps
		IL_00ed: callvirt instance void PlayerScript::DeactivateJumpRope()
		IL_00f2: ldarg.0
		IL_00f3: ldc.i4.0
		IL_00f4: stfld bool JumpRopeScript::jumpStarted
		IL_00f9: ret

		IL_00fa: nop
		IL_00fb: ldarg.0
		IL_00fc: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text JumpRopeScript::jumpCount
		IL_0101: stloc.s 8
		IL_0103: ldarg.0
		IL_0104: ldc.i4.0
		IL_0105: stfld int32 JumpRopeScript::jumps
		IL_010a: ldarg.0
		IL_010b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0110: ldc.i8 72
		IL_0119: add
		IL_011a: call instance string [mscorlib]System.Int32::ToString()
		IL_011f: stloc.s 9
		IL_0121: ldloc.s 9
		IL_0123: ldstr "/5"
		IL_0128: call string [mscorlib]System.String::Concat(string, string)
		IL_012d: stloc.s 10
		IL_012f: ldloc.s 10
		IL_0131: stloc.s 7
		IL_0133: ldarg.0
		IL_0134: ldfld class PlaytimeScript JumpRopeScript::playtime
		IL_0139: stloc.s 11
		IL_013b: ldarg.0
		IL_013c: ldc.r4 2
		IL_0141: stfld float32 JumpRopeScript::jumpDelay
		IL_0146: ldloc.s 11
		IL_0148: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource PlaytimeScript::audioDevice
		IL_014d: ldarg.0
		IL_014e: ldfld class PlaytimeScript JumpRopeScript::playtime
		IL_0153: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip PlaytimeScript::aud_Oops
		IL_0158: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)

		IL_015d: nop
		IL_015e: ldarg.0
		IL_015f: ldc.i4.0
		IL_0160: stfld bool JumpRopeScript::jumpStarted
		IL_0165: ret

		IL_0166: nop
		IL_0167: ldnull
		IL_0168: throw

		IL_0169: nop
		IL_016a: ldnull
		IL_016b: throw
	} // end of method JumpRopeScript::RopeHit

	.method private hidebysig 
		instance void Success () cil managed 
	{
		// Method begins at RVA 0x5eb0
		// Header size: 12
		// Code size: 214 (0xd6)
		.maxstack 3
		.locals init (
			[0] class PlaytimeScript,
			[1] int64,
			[2] class [UnityEngine.AudioModule]UnityEngine.AudioSource,
			[3] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[4] string,
			[5] string,
			[6] string,
			[7] class PlaytimeScript
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class PlaytimeScript JumpRopeScript::playtime
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource PlaytimeScript::audioDevice
		IL_000e: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Stop()
		IL_0013: ldarg.0
		IL_0014: ldfld int32 JumpRopeScript::jumps
		IL_0019: conv.i8
		IL_001a: stloc.1
		IL_001b: ldarg.0
		IL_001c: ldfld class PlaytimeScript JumpRopeScript::playtime
		IL_0021: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource PlaytimeScript::audioDevice
		IL_0026: stloc.2
		IL_0027: ldloc.2
		IL_0028: ldarg.0
		IL_0029: ldfld class PlaytimeScript JumpRopeScript::playtime
		IL_002e: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] PlaytimeScript::aud_Numbers
		IL_0033: ldloc.1
		IL_0034: conv.i4
		IL_0035: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_003a: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_003f: ldarg.0
		IL_0040: ldarg.0
		IL_0041: ldfld int32 JumpRopeScript::jumps
		IL_0046: ldc.i4.1
		IL_0047: add
		IL_0048: stfld int32 JumpRopeScript::jumps
		IL_004d: ldarg.0
		IL_004e: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text JumpRopeScript::jumpCount
		IL_0053: stloc.3
		IL_0054: ldarg.0
		IL_0055: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_005a: ldc.i8 72
		IL_0063: add
		IL_0064: call instance string [mscorlib]System.Int32::ToString()
		IL_0069: stloc.s 4
		IL_006b: ldloc.s 4
		IL_006d: ldstr "/5"
		IL_0072: call string [mscorlib]System.String::Concat(string, string)
		IL_0077: stloc.s 5
		IL_0079: ldloc.s 5
		IL_007b: stloc.s 6
		IL_007d: ldarg.0
		IL_007e: ldc.r4 0.5
		IL_0083: stfld float32 JumpRopeScript::jumpDelay
		IL_0088: ldarg.0
		IL_0089: ldfld int32 JumpRopeScript::jumps
		IL_008e: ldc.i4.5
		IL_008f: clt
		IL_0091: brtrue.s IL_00ce

		IL_0093: nop
		IL_0094: ldarg.0
		IL_0095: ldfld class PlaytimeScript JumpRopeScript::playtime
		IL_009a: stloc.s 7
		IL_009c: ldloc.s 7
		IL_009e: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource PlaytimeScript::audioDevice
		IL_00a3: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Stop()
		IL_00a8: ldarg.0
		IL_00a9: ldfld class PlaytimeScript JumpRopeScript::playtime
		IL_00ae: stloc.s 7
		IL_00b0: ldloc.s 7
		IL_00b2: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource PlaytimeScript::audioDevice
		IL_00b7: ldloc.s 7
		IL_00b9: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip PlaytimeScript::aud_Congrats
		IL_00be: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_00c3: ldarg.0
		IL_00c4: ldfld class PlayerScript JumpRopeScript::ps
		IL_00c9: callvirt instance void PlayerScript::DeactivateJumpRope()

		IL_00ce: nop
		IL_00cf: ret

		IL_00d0: nop
		IL_00d1: ldnull
		IL_00d2: throw

		IL_00d3: nop
		IL_00d4: ldnull
		IL_00d5: throw
	} // end of method JumpRopeScript::Success

	.method private hidebysig 
		instance void Fail () cil managed 
	{
		// Method begins at RVA 0x5f94
		// Header size: 12
		// Code size: 93 (0x5d)
		.maxstack 2
		.locals init (
			[0] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[1] string,
			[2] string,
			[3] class PlaytimeScript
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text JumpRopeScript::jumpCount
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldc.i4.0
		IL_000a: stfld int32 JumpRopeScript::jumps
		IL_000f: ldarg.0
		IL_0010: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0015: ldc.i8 72
		IL_001e: add
		IL_001f: call instance string [mscorlib]System.Int32::ToString()
		IL_0024: stloc.1
		IL_0025: ldloc.1
		IL_0026: ldstr "/5"
		IL_002b: call string [mscorlib]System.String::Concat(string, string)
		IL_0030: stloc.2
		IL_0031: ldarg.0
		IL_0032: ldfld class PlaytimeScript JumpRopeScript::playtime
		IL_0037: stloc.3
		IL_0038: ldarg.0
		IL_0039: ldc.r4 2
		IL_003e: stfld float32 JumpRopeScript::jumpDelay
		IL_0043: ldloc.3
		IL_0044: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource PlaytimeScript::audioDevice
		IL_0049: ldarg.0
		IL_004a: ldfld class PlaytimeScript JumpRopeScript::playtime
		IL_004f: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip PlaytimeScript::aud_Oops
		IL_0054: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_0059: ret

		IL_005a: nop
		IL_005b: ldnull
		IL_005c: throw
	} // end of method JumpRopeScript::Fail

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2249
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method JumpRopeScript::.ctor

} // end of class JumpRopeScript
