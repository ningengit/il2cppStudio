.class public auto ansi beforefieldinit BullyScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform player
	.field public class GameControllerScript gc
	.field public class [UnityEngine.CoreModule]UnityEngine.Renderer bullyRenderer
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform wanderTarget
	.field public class AILocationSelectorScript wanderer
	.field public float32 waitTime
	.field public float32 activeTime
	.field public float32 guilt
	.field public bool active
	.field public bool spoken
	.field private class [UnityEngine.AudioModule]UnityEngine.AudioSource audioDevice
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip[] aud_Taunts
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip[] aud_Thanks
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_Denied

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0xbfb0
		// Header size: 12
		// Code size: 39 (0x27)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.AudioModule]UnityEngine.AudioSource,
			[1] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.AudioModule]UnityEngine.AudioSource>()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.AudioModule]UnityEngine.AudioSource BullyScript::audioDevice
		IL_000f: ldc.r4 60
		IL_0014: ldc.r4 120
		IL_0019: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_001e: stloc.1
		IL_001f: ldarg.0
		IL_0020: ldloc.1
		IL_0021: stfld float32 BullyScript::waitTime
		IL_0026: ret
	} // end of method BullyScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0xbfe4
		// Header size: 12
		// Code size: 324 (0x144)
		.maxstack 3
		.locals init (
			[0] float32,
			[1] float32,
			[2] float32,
			[3] float32,
			[4] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[5] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[6] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[7] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[8] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[9] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[10] float32,
			[11] int32,
			[12] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld float32 BullyScript::waitTime
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld float32 BullyScript::waitTime
		IL_000e: ldc.r4 0.0
		IL_0013: cgt
		IL_0015: brtrue.s IL_002f

		IL_0017: nop
		IL_0018: ldarg.0
		IL_0019: ldfld bool BullyScript::active
		IL_001e: ldc.i4.0
		IL_001f: ceq
		IL_0021: ldc.i4.0
		IL_0022: ceq
		IL_0024: brtrue.s IL_0041

		IL_0026: nop
		IL_0027: ldarg.0
		IL_0028: callvirt instance void BullyScript::Activate()
		IL_002d: br.s IL_0041

		IL_002f: nop
		IL_0030: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0035: stloc.1
		IL_0036: ldarg.0
		IL_0037: ldloc.0
		IL_0038: ldloc.1
		IL_0039: sub
		IL_003a: stfld float32 BullyScript::waitTime
		IL_003f: ldloc.1
		IL_0040: stloc.0

		IL_0041: nop
		IL_0042: ldarg.0
		IL_0043: ldfld bool BullyScript::active
		IL_0048: ldc.i4.0
		IL_0049: ceq
		IL_004b: brtrue IL_0110

		IL_0050: nop
		IL_0051: ldarg.0
		IL_0052: ldfld float32 BullyScript::activeTime
		IL_0057: stloc.2
		IL_0058: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_005d: stloc.3
		IL_005e: ldarg.0
		IL_005f: ldloc.3
		IL_0060: ldloc.2
		IL_0061: add
		IL_0062: stfld float32 BullyScript::activeTime
		IL_0067: ldarg.0
		IL_0068: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_006d: stloc.s 4
		IL_006f: ldloc.s 4
		IL_0071: stloc.s 5
		IL_0073: ldloc.s 5
		IL_0075: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_007a: stloc.s 6
		IL_007c: ldarg.0
		IL_007d: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform BullyScript::player
		IL_0082: stloc.s 7
		IL_0084: ldloc.s 7
		IL_0086: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_008b: stloc.s 8
		IL_008d: ldloca.s 9
		IL_008f: ldloca.s 6
		IL_0091: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0096: ldloca.s 8
		IL_0098: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_009d: sub
		IL_009e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00a3: ldloca.s 9
		IL_00a5: ldloca.s 6
		IL_00a7: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00ac: ldloca.s 8
		IL_00ae: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00b3: sub
		IL_00b4: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00b9: ldloca.s 9
		IL_00bb: ldloca.s 6
		IL_00bd: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00c2: ldloca.s 8
		IL_00c4: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00c9: sub
		IL_00ca: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00cf: ldloca.s 9
		IL_00d1: call instance float32 [UnityEngine.CoreModule]UnityEngine.Vector3::get_magnitude()
		IL_00d6: stloc.s 10
		IL_00d8: ldloc.3
		IL_00d9: ldloc.2
		IL_00da: add
		IL_00db: ldc.r4 180
		IL_00e0: clt
		IL_00e2: ldc.i4.0
		IL_00e3: ceq
		IL_00e5: stloc.s 11
		IL_00e7: ldloc.s 10
		IL_00e9: stloc.0
		IL_00ea: ldloc.3
		IL_00eb: ldloc.2
		IL_00ec: add
		IL_00ed: ldc.r4 180
		IL_00f2: clt
		IL_00f4: ldc.i4.0
		IL_00f5: ceq
		IL_00f7: ldloc.s 10
		IL_00f9: ldc.r4 120
		IL_00fe: clt
		IL_0100: ldc.i4.0
		IL_0101: ceq
		IL_0103: and
		IL_0104: ldc.i4.0
		IL_0105: ceq
		IL_0107: brtrue.s IL_0110

		IL_0109: nop
		IL_010a: ldarg.0
		IL_010b: callvirt instance void BullyScript::Reset()

		IL_0110: nop
		IL_0111: ldarg.0
		IL_0112: ldfld float32 BullyScript::guilt
		IL_0117: stloc.0
		IL_0118: ldarg.0
		IL_0119: ldfld float32 BullyScript::guilt
		IL_011e: ldc.r4 0.0
		IL_0123: cgt
		IL_0125: ldc.i4.0
		IL_0126: ceq
		IL_0128: brtrue.s IL_013f

		IL_012a: nop
		IL_012b: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0130: stloc.s 12
		IL_0132: ldarg.0
		IL_0133: ldloc.0
		IL_0134: ldloc.s 12
		IL_0136: sub
		IL_0137: stfld float32 BullyScript::guilt
		IL_013c: ldloc.s 12
		IL_013e: stloc.0

		IL_013f: nop
		IL_0140: ret

		IL_0141: nop
		IL_0142: ldnull
		IL_0143: throw
	} // end of method BullyScript::Update

	.method private hidebysig 
		instance void FixedUpdate () cil managed 
	{
		// Method begins at RVA 0xc134
		// Header size: 12
		// Code size: 559 (0x22f)
		.maxstack 6
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[2] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[3] float32,
			[4] float32,
			[5] float32,
			[6] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[7] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[8] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[9] float32,
			[10] float32,
			[11] float32,
			[12] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[13] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[14] valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit,
			[15] bool,
			[16] valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit,
			[17] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[18] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[19] string,
			[20] bool,
			[21] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[22] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[23] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[24] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[25] float32,
			[26] int32,
			[27] float32,
			[28] int32,
			[29] float32,
			[30] int32,
			[31] int32,
			[32] class [UnityEngine.AudioModule]UnityEngine.AudioSource
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform BullyScript::player
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_000e: stloc.1
		IL_000f: ldarg.0
		IL_0010: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0015: stloc.2
		IL_0016: ldloca.s 1
		IL_0018: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_001d: stloc.3
		IL_001e: ldloca.s 1
		IL_0020: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0025: stloc.s 4
		IL_0027: ldloca.s 1
		IL_0029: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_002e: stloc.s 5
		IL_0030: ldloc.2
		IL_0031: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0036: stloc.s 6
		IL_0038: ldarg.0
		IL_0039: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_003e: stloc.s 7
		IL_0040: ldloc.3
		IL_0041: ldloca.s 6
		IL_0043: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0048: sub
		IL_0049: stloc.3
		IL_004a: ldloc.s 4
		IL_004c: ldloca.s 6
		IL_004e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0053: sub
		IL_0054: stloc.s 4
		IL_0056: ldloc.s 5
		IL_0058: ldloca.s 6
		IL_005a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_005f: sub
		IL_0060: stloc.s 5
		IL_0062: ldloc.s 7
		IL_0064: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0069: stloc.s 8
		IL_006b: ldc.r4 0.0
		IL_0070: ldloca.s 8
		IL_0072: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0077: add
		IL_0078: stloc.s 9
		IL_007a: ldc.r4 4
		IL_007f: ldloca.s 8
		IL_0081: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0086: add
		IL_0087: stloc.s 10
		IL_0089: ldc.r4 0.0
		IL_008e: ldloca.s 8
		IL_0090: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0095: add
		IL_0096: stloc.s 11
		IL_0098: br.s IL_009b

		IL_009a: nop

		IL_009b: nop
		IL_009c: ldloca.s 12
		IL_009e: ldstr "unpcklps xmm9,xmm10"
		IL_00a3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00a8: box [mscorlib]System.Int64
		IL_00ad: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_00b2: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00b7: ldloca.s 12
		IL_00b9: ldloc.s 11
		IL_00bb: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00c0: ldloc.s 12
		IL_00c2: ldloca.s 13
		IL_00c4: ldstr "unpcklps xmm6,xmm7"
		IL_00c9: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00ce: box [mscorlib]System.Int64
		IL_00d3: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_00d8: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00dd: ldloca.s 13
		IL_00df: ldloc.s 5
		IL_00e1: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00e6: ldloc.s 13
		IL_00e8: ldloca.s 14
		IL_00ea: ldc.r4 (00 00 80 7F)
		IL_00ef: ldc.i4 769
		IL_00f4: ldc.i4.1
		IL_00f5: box [mscorlib]System.Int32
		IL_00fa: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.PhysicsModule]UnityEngine.QueryTriggerInteraction>(object)
		IL_00ff: call bool [UnityEngine.PhysicsModule]UnityEngine.Physics::Raycast(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit&, float32, int32, valuetype [UnityEngine.PhysicsModule]UnityEngine.QueryTriggerInteraction)
		IL_0104: stloc.s 15
		IL_0106: ldloca.s 16
		IL_0108: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_010d: stloc.s 17
		IL_010f: ldloc.s 17
		IL_0111: stloc.s 18
		IL_0113: ldloc.s 18
		IL_0115: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Component::get_tag()
		IL_011a: stloc.s 19
		IL_011c: ldloc.s 19
		IL_011e: ldstr "Player"
		IL_0123: call bool [mscorlib]System.String::Equals(string, string)
		IL_0128: stloc.s 20
		IL_012a: ldarg.0
		IL_012b: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0130: stloc.s 21
		IL_0132: ldloc.s 21
		IL_0134: stloc.s 18
		IL_0136: ldloc.s 18
		IL_0138: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_013d: stloc.s 22
		IL_013f: ldarg.0
		IL_0140: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform BullyScript::player
		IL_0145: stloc.0
		IL_0146: ldloc.0
		IL_0147: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_014c: stloc.s 23
		IL_014e: ldloca.s 24
		IL_0150: ldloca.s 22
		IL_0152: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0157: ldloca.s 23
		IL_0159: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_015e: sub
		IL_015f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0164: ldloca.s 24
		IL_0166: ldloca.s 22
		IL_0168: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_016d: ldloca.s 23
		IL_016f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0174: sub
		IL_0175: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_017a: ldloca.s 24
		IL_017c: ldloca.s 22
		IL_017e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0183: ldloca.s 23
		IL_0185: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_018a: sub
		IL_018b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0190: ldloca.s 24
		IL_0192: call instance float32 [UnityEngine.CoreModule]UnityEngine.Vector3::get_magnitude()
		IL_0197: stloc.s 25
		IL_0199: ldc.r4 30
		IL_019e: ldloc.s 25
		IL_01a0: clt
		IL_01a2: brfalse.s IL_01a9

		IL_01a4: ldc.i4.0
		IL_01a5: stloc.s 26
		IL_01a7: br.s IL_01b8

		IL_01a9: nop
		IL_01aa: ldarg.0
		IL_01ab: ldfld bool BullyScript::active
		IL_01b0: ldloc.s 20
		IL_01b2: and
		IL_01b3: ldloc.s 15
		IL_01b5: and
		IL_01b6: stloc.s 26

		IL_01b8: nop
		IL_01b9: ldloc.s 25
		IL_01bb: stloc.s 27
		IL_01bd: ldloc.s 26
		IL_01bf: stloc.s 28
		IL_01c1: ldloc.s 26
		IL_01c3: ldc.i4.0
		IL_01c4: ceq
		IL_01c6: brtrue.s IL_0227

		IL_01c8: nop
		IL_01c9: ldarg.0
		IL_01ca: ldfld bool BullyScript::spoken
		IL_01cf: ldc.i4.0
		IL_01d0: ceq
		IL_01d2: ldc.i4.0
		IL_01d3: ceq
		IL_01d5: brtrue.s IL_021b

		IL_01d7: nop
		IL_01d8: ldc.r4 0.0
		IL_01dd: ldc.r4 1
		IL_01e2: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_01e7: stloc.s 29
		IL_01e9: ldloc.s 29
		IL_01eb: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_01f0: stloc.s 30
		IL_01f2: ldloc.s 30
		IL_01f4: stloc.s 31
		IL_01f6: ldarg.0
		IL_01f7: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource BullyScript::audioDevice
		IL_01fc: stloc.s 32
		IL_01fe: ldloc.s 32
		IL_0200: ldarg.0
		IL_0201: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] BullyScript::aud_Taunts
		IL_0206: ldloc.s 30
		IL_0208: conv.i8
		IL_0209: conv.i4
		IL_020a: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_020f: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_0214: ldarg.0
		IL_0215: ldc.i4.1
		IL_0216: stfld bool BullyScript::spoken

		IL_021b: nop
		IL_021c: ldarg.0
		IL_021d: ldc.r4 10
		IL_0222: stfld float32 BullyScript::guilt

		IL_0227: nop
		IL_0228: ret

		IL_0229: nop
		IL_022a: ldnull
		IL_022b: throw

		IL_022c: nop
		IL_022d: ldnull
		IL_022e: throw
	} // end of method BullyScript::FixedUpdate

	.method private hidebysig 
		instance void Activate () cil managed 
	{
		// Method begins at RVA 0xc370
		// Header size: 12
		// Code size: 485 (0x1e5)
		.maxstack 5
		.locals init (
			[0] class AILocationSelectorScript,
			[1] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[2] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[4] float32,
			[5] float32,
			[6] float32,
			[7] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[8] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[9] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[10] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[11] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[12] float32,
			[13] float32,
			[14] float32,
			[15] int64,
			[16] int64,
			[17] int64,
			[18] int64,
			[19] float32,
			[20] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[21] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[22] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[23] float32,
			[24] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[25] class [UnityEngine.CoreModule]UnityEngine.Transform
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class AILocationSelectorScript BullyScript::wanderer
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: callvirt instance void AILocationSelectorScript::GetNewTargetHallway()
		IL_000e: ldarg.0
		IL_000f: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0014: stloc.1
		IL_0015: ldloc.1
		IL_0016: stloc.2
		IL_0017: ldarg.0
		IL_0018: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform BullyScript::wanderTarget
		IL_001d: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0022: stloc.3
		IL_0023: ldc.r4 0.0
		IL_0028: ldloca.s 3
		IL_002a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_002f: add
		IL_0030: stloc.s 4
		IL_0032: ldc.r4 5
		IL_0037: ldloca.s 3
		IL_0039: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_003e: add
		IL_003f: stloc.s 5
		IL_0041: ldc.r4 0.0
		IL_0046: ldloca.s 3
		IL_0048: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_004d: add
		IL_004e: stloc.s 6
		IL_0050: ldloc.1
		IL_0051: ldloca.s 7
		IL_0053: ldloc.s 4
		IL_0055: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_005a: ldloca.s 7
		IL_005c: ldloc.s 5
		IL_005e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0063: ldloca.s 7
		IL_0065: ldloc.s 6
		IL_0067: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_006c: ldloc.s 7
		IL_006e: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0073: ldarg.0
		IL_0074: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0079: stloc.s 8
		IL_007b: ldloc.s 8
		IL_007d: stloc.2
		// loop start (head: IL_007e)
			IL_007e: nop
			IL_007f: ldloc.2
			IL_0080: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
			IL_0085: stloc.s 9
			IL_0087: ldarg.0
			IL_0088: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform BullyScript::player
			IL_008d: stloc.s 10
			IL_008f: ldloc.s 10
			IL_0091: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
			IL_0096: stloc.s 11
			IL_0098: ldloca.s 9
			IL_009a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_009f: ldloca.s 11
			IL_00a1: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_00a6: sub
			IL_00a7: stloc.s 12
			IL_00a9: ldloca.s 9
			IL_00ab: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_00b0: ldloca.s 11
			IL_00b2: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_00b7: sub
			IL_00b8: stloc.s 13
			IL_00ba: ldloca.s 9
			IL_00bc: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_00c1: ldloca.s 11
			IL_00c3: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_00c8: sub
			IL_00c9: stloc.s 14
			IL_00cb: br.s IL_00ce

			IL_00cd: nop

			IL_00ce: nop
			IL_00cf: ldstr "cvtps2pd xmm1,xmm6"
			IL_00d4: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_00d9: stloc.s 15
			IL_00db: ldc.r4 0.0
			IL_00e0: ldstr "cvtps2pd xmm1,xmm6"
			IL_00e5: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_00ea: box [mscorlib]System.Int64
			IL_00ef: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
			IL_00f4: cgt
			IL_00f6: brtrue.s IL_0107

			IL_00f8: nop
			IL_00f9: ldstr "sqrtsd xmm0,xmm1"
			IL_00fe: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0103: stloc.s 16
			IL_0105: br.s IL_013a

			IL_0107: nop
			IL_0108: ldstr "sub_20C340"
			IL_010d: ldc.i8 0
			IL_0116: ldloc.s 10
			IL_0118: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
			IL_011d: ldc.i8 0
			IL_0126: ldc.i8 0
			IL_012f: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
			IL_0134: stloc.s 17
			IL_0136: ldloc.s 17
			IL_0138: stloc.s 18

			IL_013a: nop
			IL_013b: ldloc.s 16
			IL_013d: conv.r4
			IL_013e: stloc.s 19
			IL_0140: ldc.r4 20
			IL_0145: ldloc.s 16
			IL_0147: conv.r4
			IL_0148: cgt
			IL_014a: ldc.i4.0
			IL_014b: ceq
			IL_014d: brtrue IL_01d9

			IL_0152: nop
			IL_0153: ldarg.0
			IL_0154: ldfld class AILocationSelectorScript BullyScript::wanderer
			IL_0159: callvirt instance void AILocationSelectorScript::GetNewTargetHallway()
			IL_015e: ldarg.0
			IL_015f: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
			IL_0164: stloc.s 20
			IL_0166: ldloc.s 20
			IL_0168: stloc.s 21
			IL_016a: ldarg.0
			IL_016b: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform BullyScript::wanderTarget
			IL_0170: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
			IL_0175: stloc.s 22
			IL_0177: ldc.r4 0.0
			IL_017c: ldloca.s 22
			IL_017e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_0183: add
			IL_0184: stloc.s 19
			IL_0186: ldc.r4 5
			IL_018b: ldloca.s 22
			IL_018d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_0192: add
			IL_0193: stloc.s 23
			IL_0195: ldc.r4 0.0
			IL_019a: ldloca.s 22
			IL_019c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_01a1: add
			IL_01a2: stloc.s 6
			IL_01a4: ldloc.s 20
			IL_01a6: ldloca.s 24
			IL_01a8: ldloc.s 19
			IL_01aa: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_01af: ldloca.s 24
			IL_01b1: ldloc.s 23
			IL_01b3: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_01b8: ldloca.s 24
			IL_01ba: ldloc.s 6
			IL_01bc: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_01c1: ldloc.s 24
			IL_01c3: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
			IL_01c8: ldarg.0
			IL_01c9: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
			IL_01ce: stloc.s 25
			IL_01d0: ldloc.s 25
			IL_01d2: stloc.s 21
			IL_01d4: br IL_007e
		// end loop

		IL_01d9: nop
		IL_01da: ldarg.0
		IL_01db: ldc.i4.1
		IL_01dc: stfld bool BullyScript::active
		IL_01e1: ret

		IL_01e2: nop
		IL_01e3: ldnull
		IL_01e4: throw
	} // end of method BullyScript::Activate

	.method private hidebysig 
		instance void OnTriggerEnter (
			class [UnityEngine.PhysicsModule]UnityEngine.Collider other
		) cil managed 
	{
		// Method begins at RVA 0xc564
		// Header size: 12
		// Code size: 828 (0x33c)
		.maxstack 5
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[2] string,
			[3] bool,
			[4] bool,
			[5] class GameControllerScript,
			[6] int32[],
			[7] int32,
			[8] int32,
			[9] float32,
			[10] int32,
			[11] class GameControllerScript,
			[12] float32,
			[13] float32,
			[14] float32,
			[15] int64,
			[16] float64,
			[17] int64,
			[18] float64,
			[19] int64,
			[20] int64,
			[21] float64,
			[22] int64,
			[23] class GameControllerScript,
			[24] int64,
			[25] string,
			[26] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[27] string[],
			[28] float32,
			[29] int32,
			[30] int32
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: stloc.1
		IL_000a: ldloc.1
		IL_000b: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Component::get_tag()
		IL_0010: stloc.2
		IL_0011: ldloc.2
		IL_0012: ldstr "Player"
		IL_0017: call bool [mscorlib]System.String::Equals(string, string)
		IL_001c: stloc.3
		IL_001d: ldloc.3
		IL_001e: stloc.s 4
		IL_0020: ldloc.3
		IL_0021: ldc.i4.0
		IL_0022: ceq
		IL_0024: brtrue IL_0320

		IL_0029: nop
		IL_002a: ldarg.0
		IL_002b: ldfld class GameControllerScript BullyScript::gc
		IL_0030: stloc.s 5
		IL_0032: ldloc.s 5
		IL_0034: ldfld int32[] GameControllerScript::item
		IL_0039: stloc.s 6
		IL_003b: ldloc.s 6
		IL_003d: ldc.i4.0
		IL_003e: ldelem [mscorlib]System.Int32
		IL_0043: ldc.i4.0
		IL_0044: ceq
		IL_0046: brfalse.s IL_0075

		IL_0048: ldarg.0
		IL_0049: ldfld class GameControllerScript BullyScript::gc
		IL_004e: ldfld int32[] GameControllerScript::item
		IL_0053: ldc.i4.2
		IL_0054: ldelem [mscorlib]System.Int32
		IL_0059: ldc.i4.0
		IL_005a: ceq
		IL_005c: ldarg.0
		IL_005d: ldfld class GameControllerScript BullyScript::gc
		IL_0062: ldfld int32[] GameControllerScript::item
		IL_0067: ldc.i4.1
		IL_0068: ldelem [mscorlib]System.Int32
		IL_006d: ldc.i4.0
		IL_006e: ceq
		IL_0070: and
		IL_0071: stloc.s 7
		IL_0073: br.s IL_0079

		IL_0075: nop
		IL_0076: ldc.i4.0
		IL_0077: stloc.s 7

		IL_0079: nop
		IL_007a: ldarg.0
		IL_007b: ldfld class GameControllerScript BullyScript::gc
		IL_0080: ldfld int32[] GameControllerScript::item
		IL_0085: ldc.i4.2
		IL_0086: ldelem [mscorlib]System.Int32
		IL_008b: ldc.i4.0
		IL_008c: ceq
		IL_008e: ldarg.0
		IL_008f: ldfld class GameControllerScript BullyScript::gc
		IL_0094: ldfld int32[] GameControllerScript::item
		IL_0099: ldc.i4.1
		IL_009a: ldelem [mscorlib]System.Int32
		IL_009f: ldc.i4.0
		IL_00a0: ceq
		IL_00a2: and
		IL_00a3: stloc.s 8
		IL_00a5: ldloc.s 7
		IL_00a7: ldc.i4.0
		IL_00a8: ceq
		IL_00aa: ldc.i4.0
		IL_00ab: ceq
		IL_00ad: brtrue IL_0322

		IL_00b2: nop
		IL_00b3: ldc.r4 0.0
		IL_00b8: ldc.r4 2
		IL_00bd: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_00c2: stloc.s 9
		IL_00c4: ldloc.s 9
		IL_00c6: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_00cb: stloc.s 10
		IL_00cd: ldarg.0
		IL_00ce: ldfld class GameControllerScript BullyScript::gc
		IL_00d3: stloc.s 11
		IL_00d5: ldloc.s 10
		IL_00d7: stloc.s 8
		// loop start (head: IL_00d9)
			IL_00d9: nop
			IL_00da: ldloc.s 11
			IL_00dc: ldfld int32[] GameControllerScript::item
			IL_00e1: stloc.s 6
			IL_00e3: ldloc.s 6
			IL_00e5: ldloc.s 8
			IL_00e7: conv.i8
			IL_00e8: conv.i4
			IL_00e9: ldelem [mscorlib]System.Int32
			IL_00ee: ldc.i4.0
			IL_00ef: ceq
			IL_00f1: ldc.i4.0
			IL_00f2: ceq
			IL_00f4: brtrue IL_025d

			IL_00f9: nop
			IL_00fa: ldc.r4 0.0
			IL_00ff: ldc.r4 2
			IL_0104: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
			IL_0109: stloc.s 12
			IL_010b: ldloc.s 12
			IL_010d: stloc.s 13
			IL_010f: ldloc.s 12
			IL_0111: stloc.s 14
			IL_0113: nop
			IL_0114: nop
			IL_0115: ldstr "sub_20BB40"
			IL_011a: ldc.i8 0
			IL_0123: ldstr "&stack[0x10]"
			IL_0128: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_012d: conv.i8
			IL_012e: ldc.i8 0
			IL_0137: ldc.i8 0
			IL_0140: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
			IL_0145: stloc.s 15
			IL_0147: ldloc.s 13
			IL_0149: conv.r8
			IL_014a: stloc.s 16
			IL_014c: ldloc.s 13
			IL_014e: conv.r8
			IL_014f: ldc.r4 0.0
			IL_0154: conv.r8
			IL_0155: clt
			IL_0157: brtrue.s IL_01d6

			IL_0159: nop
			IL_015a: nop
			IL_015b: ldloc.s 14
			IL_015d: conv.r8
			IL_015e: ldc.r8 0.5
			IL_0167: ceq
			IL_0169: ldc.i4.0
			IL_016a: ceq
			IL_016c: brtrue.s IL_01a5

			IL_016e: nop
			IL_016f: ldstr "stack[0x10]"
			IL_0174: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0179: stloc.s 17
			IL_017b: ldstr "stack[0x10]"
			IL_0180: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0185: conv.i4
			IL_0186: ldc.i4.1
			IL_0187: and
			IL_0188: ldc.i4.0
			IL_0189: ceq
			IL_018b: brtrue IL_024a

			IL_0190: nop
			IL_0191: ldloc.s 17
			IL_0193: conv.r8
			IL_0194: ldc.r8 1
			IL_019d: add
			IL_019e: stloc.s 18
			IL_01a0: br IL_024a

			IL_01a5: nop
			IL_01a6: ldstr "sub_20C1F0"
			IL_01ab: ldc.i8 0
			IL_01b4: ldloc.s 6
			IL_01b6: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
			IL_01bb: ldc.i8 0
			IL_01c4: ldc.i8 0
			IL_01cd: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
			IL_01d2: stloc.s 19
			IL_01d4: br.s IL_024a

			IL_01d6: nop
			IL_01d7: nop
			IL_01d8: ldloc.s 18
			IL_01da: ldc.r8 -0.5
			IL_01e3: ceq
			IL_01e5: ldc.i4.0
			IL_01e6: ceq
			IL_01e8: brtrue.s IL_021b

			IL_01ea: nop
			IL_01eb: ldstr "stack[0x10]"
			IL_01f0: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_01f5: stloc.s 20
			IL_01f7: ldstr "stack[0x10]"
			IL_01fc: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0201: conv.i4
			IL_0202: ldc.i4.1
			IL_0203: and
			IL_0204: ldc.i4.0
			IL_0205: ceq
			IL_0207: brtrue.s IL_024a

			IL_0209: nop
			IL_020a: ldloc.s 20
			IL_020c: conv.r8
			IL_020d: ldc.r8 1
			IL_0216: sub
			IL_0217: stloc.s 21
			IL_0219: br.s IL_024a

			IL_021b: nop
			IL_021c: ldstr "sub_20C080"
			IL_0221: ldc.i8 0
			IL_022a: ldloc.s 6
			IL_022c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
			IL_0231: ldc.i8 0
			IL_023a: ldc.i8 0
			IL_0243: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
			IL_0248: stloc.s 22

			IL_024a: nop
			IL_024b: ldarg.0
			IL_024c: ldfld class GameControllerScript BullyScript::gc
			IL_0251: stloc.s 11
			IL_0253: ldloc.s 21
			IL_0255: conv.i4
			IL_0256: stloc.s 8
			IL_0258: br IL_00d9
		// end loop

		IL_025d: nop
		IL_025e: ldarg.0
		IL_025f: ldfld class GameControllerScript BullyScript::gc
		IL_0264: stloc.s 23
		IL_0266: ldarg.0
		IL_0267: ldfld class GameControllerScript BullyScript::gc
		IL_026c: ldfld int32[] GameControllerScript::item
		IL_0271: ldloc.s 8
		IL_0273: conv.i8
		IL_0274: conv.i4
		IL_0275: ldc.i4.0
		IL_0276: stelem [mscorlib]System.Int32
		IL_027b: ldloc.s 8
		IL_027d: conv.i8
		IL_027e: stloc.s 24
		IL_0280: ldloc.s 8
		IL_0282: conv.i8
		IL_0283: stloc.s 24
		IL_0285: ldloc.s 23
		IL_0287: ldfld class [UnityEngine.UI]UnityEngine.UI.RawImage[] GameControllerScript::itemSlot
		IL_028c: ldloc.s 8
		IL_028e: conv.i8
		IL_028f: conv.i4
		IL_0290: ldelem [UnityEngine.UI]UnityEngine.UI.RawImage
		IL_0295: ldloc.s 23
		IL_0297: ldfld class [UnityEngine.CoreModule]UnityEngine.Texture[] GameControllerScript::itemTextures
		IL_029c: ldc.i4.0
		IL_029d: ldelem [UnityEngine.CoreModule]UnityEngine.Texture
		IL_02a2: callvirt instance void [UnityEngine.UI]UnityEngine.UI.RawImage::set_texture(class [UnityEngine.CoreModule]UnityEngine.Texture)
		IL_02a7: ldloc.s 23
		IL_02a9: ldfld int32 GameControllerScript::itemSelected
		IL_02ae: conv.i8
		IL_02af: stloc.s 24
		IL_02b1: ldloc.s 23
		IL_02b3: ldfld string[] GameControllerScript::itemNames
		IL_02b8: ldloc.s 23
		IL_02ba: ldfld int32[] GameControllerScript::item
		IL_02bf: ldloc.s 24
		IL_02c1: conv.i4
		IL_02c2: ldelem [mscorlib]System.Int32
		IL_02c7: conv.i8
		IL_02c8: conv.i4
		IL_02c9: ldelem [mscorlib]System.String
		IL_02ce: stloc.s 25
		IL_02d0: ldloc.s 23
		IL_02d2: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text GameControllerScript::itemText
		IL_02d7: stloc.s 26
		IL_02d9: ldloc.s 23
		IL_02db: ldfld string[] GameControllerScript::itemNames
		IL_02e0: stloc.s 27
		IL_02e2: ldc.r4 0.0
		IL_02e7: ldc.r4 1
		IL_02ec: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_02f1: stloc.s 28
		IL_02f3: ldloc.s 28
		IL_02f5: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_02fa: stloc.s 29
		IL_02fc: ldloc.s 29
		IL_02fe: stloc.s 30
		IL_0300: ldarg.0
		IL_0301: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource BullyScript::audioDevice
		IL_0306: ldarg.0
		IL_0307: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] BullyScript::aud_Thanks
		IL_030c: ldloc.s 30
		IL_030e: conv.i8
		IL_030f: conv.i4
		IL_0310: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0315: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_031a: ldarg.0
		IL_031b: callvirt instance void BullyScript::Reset()

		IL_0320: nop
		IL_0321: ret

		IL_0322: nop
		IL_0323: ldarg.0
		IL_0324: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource BullyScript::audioDevice
		IL_0329: ldarg.0
		IL_032a: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip BullyScript::aud_Denied
		IL_032f: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_0334: br.s IL_0320

		IL_0336: nop
		IL_0337: ldnull
		IL_0338: throw

		IL_0339: nop
		IL_033a: ldnull
		IL_033b: throw
	} // end of method BullyScript::OnTriggerEnter

	.method private hidebysig 
		instance void OnTriggerStay (
			class [UnityEngine.PhysicsModule]UnityEngine.Collider other
		) cil managed 
	{
		// Method begins at RVA 0xc8ac
		// Header size: 12
		// Code size: 85 (0x55)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[2] string,
			[3] bool,
			[4] int32,
			[5] int32,
			[6] float32
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: stloc.1
		IL_000a: ldloc.1
		IL_000b: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_0010: stloc.2
		IL_0011: ldloc.2
		IL_0012: ldstr "Principal of the Thing"
		IL_0017: call bool [mscorlib]System.String::Equals(string, string)
		IL_001c: stloc.3
		IL_001d: ldarg.0
		IL_001e: ldfld float32 BullyScript::guilt
		IL_0023: ldc.r4 0.0
		IL_0028: cgt
		IL_002a: brfalse.s IL_0031

		IL_002c: ldloc.3
		IL_002d: stloc.s 4
		IL_002f: br.s IL_0035

		IL_0031: nop
		IL_0032: ldc.i4.0
		IL_0033: stloc.s 4

		IL_0035: nop
		IL_0036: ldloc.s 4
		IL_0038: stloc.s 5
		IL_003a: ldarg.0
		IL_003b: ldfld float32 BullyScript::guilt
		IL_0040: stloc.s 6
		IL_0042: ldloc.s 4
		IL_0044: ldc.i4.0
		IL_0045: ceq
		IL_0047: brtrue.s IL_0050

		IL_0049: nop
		IL_004a: ldarg.0
		IL_004b: callvirt instance void BullyScript::Reset()

		IL_0050: nop
		IL_0051: ret

		IL_0052: nop
		IL_0053: ldnull
		IL_0054: throw
	} // end of method BullyScript::OnTriggerStay

	.method private hidebysig 
		instance void Reset () cil managed 
	{
		// Method begins at RVA 0xc910
		// Header size: 12
		// Code size: 136 (0x88)
		.maxstack 4
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[4] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_000e: stloc.1
		IL_000f: ldloc.1
		IL_0010: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0015: stloc.2
		IL_0016: ldloc.0
		IL_0017: ldloca.s 3
		IL_0019: ldloca.s 2
		IL_001b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0020: ldc.r4 0.0
		IL_0025: sub
		IL_0026: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_002b: ldloca.s 3
		IL_002d: ldloca.s 2
		IL_002f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0034: ldc.r4 20
		IL_0039: sub
		IL_003a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_003f: ldloca.s 3
		IL_0041: ldloca.s 2
		IL_0043: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0048: ldc.r4 0.0
		IL_004d: sub
		IL_004e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0053: ldloc.3
		IL_0054: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0059: ldc.r4 60
		IL_005e: ldc.r4 120
		IL_0063: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_0068: stloc.s 4
		IL_006a: ldarg.0
		IL_006b: ldloc.s 4
		IL_006d: stfld float32 BullyScript::waitTime
		IL_0072: ldarg.0
		IL_0073: ldc.i4.0
		IL_0074: stfld bool BullyScript::active
		IL_0079: ldarg.0
		IL_007a: ldc.r4 0.0
		IL_007f: stfld float32 BullyScript::activeTime
		IL_0084: ret

		IL_0085: nop
		IL_0086: ldnull
		IL_0087: throw
	} // end of method BullyScript::Reset

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0xc9a4
		// Header size: 12
		// Code size: 36 (0x24)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[1] class [UnityEngine.AudioModule]UnityEngine.AudioClip[]
		)

		IL_0000: nop
		IL_0001: ldc.i4.2
		IL_0002: newarr [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] BullyScript::aud_Taunts
		IL_000f: ldc.i4.2
		IL_0010: newarr [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0015: stloc.1
		IL_0016: ldarg.0
		IL_0017: ldloc.1
		IL_0018: stfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] BullyScript::aud_Thanks
		IL_001d: ldarg.0
		IL_001e: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0023: ret
	} // end of method BullyScript::.ctor

} // end of class BullyScript
