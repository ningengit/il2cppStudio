.class public auto ansi beforefieldinit PlaytimeScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public bool db
	.field public bool playerSeen
	.field public bool disappointed
	.field public int32 audVal
	.field public class [UnityEngine.AnimationModule]UnityEngine.Animator animator
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform player
	.field public class PlayerScript ps
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform wanderTarget
	.field public class AILocationSelectorScript wanderer
	.field public float32 coolDown
	.field public float32 playCool
	.field public bool playerSpotted
	.field public bool jumpRopeStarted
	.field private class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent agent
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip[] aud_Numbers
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip[] aud_Random
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_Instrcutions
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_Oops
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_LetsPlay
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_Congrats
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_ReadyGo
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_Sad
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioSource audioDevice

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0xe0a8
		// Header size: 12
		// Code size: 36 (0x24)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[1] class [UnityEngine.AudioModule]UnityEngine.AudioSource
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent>()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PlaytimeScript::agent
		IL_000f: ldarg.0
		IL_0010: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.AudioModule]UnityEngine.AudioSource>()
		IL_0015: stloc.1
		IL_0016: ldarg.0
		IL_0017: ldloc.1
		IL_0018: stfld class [UnityEngine.AudioModule]UnityEngine.AudioSource PlaytimeScript::audioDevice
		IL_001d: ldarg.0
		IL_001e: callvirt instance void PlaytimeScript::Wander()
		IL_0023: ret
	} // end of method PlaytimeScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0xe0d8
		// Header size: 12
		// Code size: 154 (0x9a)
		.maxstack 3
		.locals init (
			[0] float32,
			[1] float32,
			[2] bool,
			[3] bool,
			[4] class [UnityEngine.AnimationModule]UnityEngine.Animator,
			[5] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld float32 PlaytimeScript::coolDown
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld float32 PlaytimeScript::coolDown
		IL_000e: ldc.r4 0.0
		IL_0013: cgt
		IL_0015: ldc.i4.0
		IL_0016: ceq
		IL_0018: brtrue.s IL_002a

		IL_001a: nop
		IL_001b: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0020: stloc.1
		IL_0021: ldarg.0
		IL_0022: ldloc.0
		IL_0023: ldloc.1
		IL_0024: sub
		IL_0025: stfld float32 PlaytimeScript::coolDown

		IL_002a: nop
		IL_002b: ldarg.0
		IL_002c: ldfld float32 PlaytimeScript::playCool
		IL_0031: stloc.0
		IL_0032: ldarg.0
		IL_0033: ldfld float32 PlaytimeScript::playCool
		IL_0038: ldc.r4 0.0
		IL_003d: clt
		IL_003f: ldc.i4.0
		IL_0040: ceq
		IL_0042: brtrue.s IL_0080

		IL_0044: nop
		IL_0045: ldarg.0
		IL_0046: ldfld class [UnityEngine.AnimationModule]UnityEngine.Animator PlaytimeScript::animator
		IL_004b: ldstr "disappointed"
		IL_0050: callvirt instance bool [UnityEngine.AnimationModule]UnityEngine.Animator::GetBool(string)
		IL_0055: stloc.2
		IL_0056: ldloc.2
		IL_0057: stloc.3
		IL_0058: ldloc.2
		IL_0059: ldc.i4.0
		IL_005a: ceq
		IL_005c: brtrue.s IL_0095

		IL_005e: nop
		IL_005f: ldarg.0
		IL_0060: ldfld class [UnityEngine.AnimationModule]UnityEngine.Animator PlaytimeScript::animator
		IL_0065: stloc.s 4
		IL_0067: ldarg.0
		IL_0068: ldc.r4 0.0
		IL_006d: stfld float32 PlaytimeScript::playCool
		IL_0072: ldloc.s 4
		IL_0074: ldstr "disappointed"
		IL_0079: ldc.i4.0
		IL_007a: callvirt instance void [UnityEngine.AnimationModule]UnityEngine.Animator::SetBool(string, bool)
		IL_007f: ret

		IL_0080: nop
		IL_0081: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0086: stloc.s 5
		IL_0088: ldarg.0
		IL_0089: ldloc.0
		IL_008a: ldloc.s 5
		IL_008c: sub
		IL_008d: stfld float32 PlaytimeScript::playCool
		IL_0092: ldloc.s 5
		IL_0094: stloc.0

		IL_0095: nop
		IL_0096: ret

		IL_0097: nop
		IL_0098: ldnull
		IL_0099: throw
	} // end of method PlaytimeScript::Update

	.method private hidebysig 
		instance void FixedUpdate () cil managed 
	{
		// Method begins at RVA 0xe180
		// Header size: 12
		// Code size: 1015 (0x3f7)
		.maxstack 6
		.locals init (
			[0] class PlayerScript,
			[1] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[2] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[4] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[5] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[6] float32,
			[7] bool,
			[8] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[9] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[10] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[11] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[12] float32,
			[13] float32,
			[14] float32,
			[15] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[16] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[17] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[18] int64,
			[19] float32,
			[20] bool,
			[21] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[22] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[23] int64,
			[24] string,
			[25] bool,
			[26] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[27] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[28] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[29] float32,
			[30] int32,
			[31] int32,
			[32] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[33] float32,
			[34] int32,
			[35] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[36] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[37] bool,
			[38] bool,
			[39] class [UnityEngine.AudioModule]UnityEngine.AudioSource
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class PlayerScript PlaytimeScript::ps
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: ldfld bool PlayerScript::jumpRope
		IL_000e: ldc.i4.0
		IL_000f: ceq
		IL_0011: brtrue IL_00ab

		IL_0016: nop
		IL_0017: ldarg.0
		IL_0018: ldfld bool PlaytimeScript::jumpRopeStarted
		IL_001d: ldc.i4.0
		IL_001e: ceq
		IL_0020: ldc.i4.0
		IL_0021: ceq
		IL_0023: brtrue.s IL_0082

		IL_0025: nop
		IL_0026: ldarg.0
		IL_0027: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PlaytimeScript::agent
		IL_002c: stloc.1
		IL_002d: ldarg.0
		IL_002e: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0033: stloc.2
		IL_0034: ldloc.2
		IL_0035: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_003a: stloc.3
		IL_003b: ldarg.0
		IL_003c: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0041: stloc.s 4
		IL_0043: ldloc.s 4
		IL_0045: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_forward()
		IL_004a: stloc.s 5
		IL_004c: ldloca.s 5
		IL_004e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0053: ldc.r4 10
		IL_0058: mul
		IL_0059: stloc.s 6
		IL_005b: ldloc.1
		IL_005c: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0061: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0066: conv.i8
		IL_0067: ldc.i8 -57
		IL_0070: add
		IL_0071: box [mscorlib]System.Int64
		IL_0076: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_007b: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::Warp(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0080: stloc.s 7

		IL_0082: nop
		IL_0083: ldarg.0
		IL_0084: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PlaytimeScript::agent
		IL_0089: stloc.s 8
		IL_008b: ldarg.0
		IL_008c: ldc.i4.1
		IL_008d: stfld bool PlaytimeScript::jumpRopeStarted
		IL_0092: ldloc.s 8
		IL_0094: ldc.r4 0.0
		IL_0099: callvirt instance void [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::set_speed(float32)
		IL_009e: ldarg.0
		IL_009f: ldc.r4 15
		IL_00a4: stfld float32 PlaytimeScript::playCool

		IL_00a9: nop
		IL_00aa: ret

		IL_00ab: nop
		IL_00ac: ldarg.0
		IL_00ad: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PlaytimeScript::player
		IL_00b2: stloc.s 9
		IL_00b4: ldloc.s 9
		IL_00b6: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_00bb: stloc.s 10
		IL_00bd: ldarg.0
		IL_00be: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_00c3: stloc.s 11
		IL_00c5: ldloca.s 10
		IL_00c7: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00cc: stloc.s 12
		IL_00ce: ldloca.s 10
		IL_00d0: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00d5: stloc.s 13
		IL_00d7: ldloca.s 10
		IL_00d9: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00de: stloc.s 14
		IL_00e0: ldloc.s 11
		IL_00e2: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_00e7: stloc.s 15
		IL_00e9: ldarg.0
		IL_00ea: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_00ef: stloc.s 16
		IL_00f1: ldloc.s 12
		IL_00f3: ldloca.s 15
		IL_00f5: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00fa: sub
		IL_00fb: stloc.s 12
		IL_00fd: ldloc.s 13
		IL_00ff: ldloca.s 15
		IL_0101: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0106: sub
		IL_0107: stloc.s 13
		IL_0109: ldloc.s 14
		IL_010b: ldloca.s 15
		IL_010d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0112: sub
		IL_0113: stloc.s 14
		IL_0115: ldloc.s 16
		IL_0117: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_011c: stloc.s 17
		IL_011e: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_0123: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0128: stloc.s 18
		IL_012a: ldloca.s 17
		IL_012c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0131: stloc.s 19
		IL_0133: br.s IL_0136

		IL_0135: nop

		IL_0136: nop
		IL_0137: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_013c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0141: conv.i8
		IL_0142: ldc.i8 -41
		IL_014b: add
		IL_014c: box [mscorlib]System.Int64
		IL_0151: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0156: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_015b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0160: conv.i8
		IL_0161: ldc.i8 -57
		IL_016a: add
		IL_016b: box [mscorlib]System.Int64
		IL_0170: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0175: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_017a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_017f: conv.i8
		IL_0180: ldc.i8 -25
		IL_0189: add
		IL_018a: ldc.r4 (00 00 80 7F)
		IL_018f: ldc.i4 769
		IL_0194: ldc.i4.1
		IL_0195: box [mscorlib]System.Int32
		IL_019a: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.PhysicsModule]UnityEngine.QueryTriggerInteraction>(object)
		IL_019f: call bool [UnityEngine.PhysicsModule]UnityEngine.Physics::Raycast(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit&, float32, int32, valuetype [UnityEngine.PhysicsModule]UnityEngine.QueryTriggerInteraction)
		IL_01a4: stloc.s 20
		IL_01a6: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_01ab: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01b0: conv.i8
		IL_01b1: ldc.i8 -25
		IL_01ba: add
		IL_01bb: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_01c0: stloc.s 21
		IL_01c2: ldloc.s 21
		IL_01c4: stloc.s 22
		IL_01c6: ldstr "unpcklps xmm6,xmm7"
		IL_01cb: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01d0: stloc.s 23
		IL_01d2: ldloc.s 22
		IL_01d4: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Component::get_tag()
		IL_01d9: stloc.s 24
		IL_01db: ldloc.s 24
		IL_01dd: ldstr "Player"
		IL_01e2: call bool [mscorlib]System.String::Equals(string, string)
		IL_01e7: stloc.s 25
		IL_01e9: ldarg.0
		IL_01ea: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_01ef: stloc.s 26
		IL_01f1: ldloc.s 26
		IL_01f3: stloc.s 22
		IL_01f5: ldloc.s 22
		IL_01f7: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_01fc: stloc.s 27
		IL_01fe: ldarg.0
		IL_01ff: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PlaytimeScript::player
		IL_0204: stloc.s 9
		IL_0206: ldloc.s 9
		IL_0208: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_020d: stloc.s 28
		IL_020f: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0214: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0219: conv.i8
		IL_021a: ldc.i8 -57
		IL_0223: add
		IL_0224: call instance float32 [UnityEngine.CoreModule]UnityEngine.Vector3::get_magnitude()
		IL_0229: stloc.s 29
		IL_022b: ldc.r4 80
		IL_0230: ldloc.s 29
		IL_0232: clt
		IL_0234: brfalse.s IL_023b

		IL_0236: ldc.i4.0
		IL_0237: stloc.s 30
		IL_0239: br.s IL_0243

		IL_023b: nop
		IL_023c: ldloc.s 25
		IL_023e: ldloc.s 20
		IL_0240: and
		IL_0241: stloc.s 30

		IL_0243: nop
		IL_0244: ldloc.s 29
		IL_0246: stloc.s 6
		IL_0248: ldc.r4 0.0
		IL_024d: ldarg.0
		IL_024e: ldfld float32 PlaytimeScript::playCool
		IL_0253: clt
		IL_0255: ldc.i4.0
		IL_0256: ceq
		IL_0258: stloc.s 31
		IL_025a: ldloc.s 30
		IL_025c: ldc.r4 0.0
		IL_0261: ldarg.0
		IL_0262: ldfld float32 PlaytimeScript::playCool
		IL_0267: clt
		IL_0269: ldc.i4.0
		IL_026a: ceq
		IL_026c: and
		IL_026d: ldc.i4.0
		IL_026e: ceq
		IL_0270: ldc.i4.0
		IL_0271: ceq
		IL_0273: brtrue IL_0327

		IL_0278: nop
		IL_0279: ldarg.0
		IL_027a: ldfld bool PlaytimeScript::playerSeen
		IL_027f: ldc.r4 0.0
		IL_0284: ldarg.0
		IL_0285: ldfld float32 PlaytimeScript::coolDown
		IL_028a: clt
		IL_028c: ldc.i4.0
		IL_028d: ceq
		IL_028f: and
		IL_0290: ldc.i4.0
		IL_0291: ceq
		IL_0293: ldc.i4.0
		IL_0294: ceq
		IL_0296: brtrue.s IL_030c

		IL_0298: nop
		IL_0299: ldarg.0
		IL_029a: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PlaytimeScript::agent
		IL_029f: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::get_velocity()
		IL_02a4: stloc.s 32
		IL_02a6: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_02ab: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_02b0: conv.i8
		IL_02b1: ldc.i8 -57
		IL_02ba: add
		IL_02bb: call instance float32 [UnityEngine.CoreModule]UnityEngine.Vector3::get_magnitude()
		IL_02c0: stloc.s 33
		IL_02c2: ldloc.s 33
		IL_02c4: stloc.s 6
		IL_02c6: ldc.r4 1
		IL_02cb: ldloc.s 33
		IL_02cd: clt
		IL_02cf: ldc.i4.0
		IL_02d0: ceq
		IL_02d2: stloc.s 34
		IL_02d4: ldc.r4 1
		IL_02d9: ldloc.s 33
		IL_02db: clt
		IL_02dd: ldc.i4.0
		IL_02de: ceq
		IL_02e0: ldc.r4 0.0
		IL_02e5: ldarg.0
		IL_02e6: ldfld float32 PlaytimeScript::coolDown
		IL_02eb: clt
		IL_02ed: ldc.i4.0
		IL_02ee: ceq
		IL_02f0: and
		IL_02f1: ldc.i4.0
		IL_02f2: ceq
		IL_02f4: brtrue IL_03e7

		IL_02f9: nop
		IL_02fa: ldarg.0
		IL_02fb: callvirt instance void PlaytimeScript::Wander()
		IL_0300: ldarg.0
		IL_0301: ldc.i4.0
		IL_0302: stfld bool PlaytimeScript::jumpRopeStarted
		IL_0307: br IL_00a9

		IL_030c: nop
		IL_030d: ldarg.0
		IL_030e: ldloc.s 34
		IL_0310: stfld bool PlaytimeScript::playerSeen
		IL_0315: ldarg.0
		IL_0316: callvirt instance void PlaytimeScript::Wander()
		IL_031b: ldarg.0
		IL_031c: ldc.i4.0
		IL_031d: stfld bool PlaytimeScript::jumpRopeStarted
		IL_0322: br IL_00a9

		IL_0327: nop
		IL_0328: ldarg.0
		IL_0329: ldc.i4.1
		IL_032a: stfld bool PlaytimeScript::playerSeen
		IL_032f: ldstr "global_19E3B6E"
		IL_0334: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0339: ldloc.s 34
		IL_033b: conv.i8
		IL_033c: ceq
		IL_033e: ldc.i4.0
		IL_033f: ceq
		IL_0341: brtrue.s IL_034e

		IL_0343: nop
		IL_0344: ldstr "mov byte ptr [1819E3B6Eh],1 ; value = 0x1"
		IL_0349: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)

		IL_034e: nop
		IL_034f: ldarg.0
		IL_0350: ldfld class [UnityEngine.AnimationModule]UnityEngine.Animator PlaytimeScript::animator
		IL_0355: ldstr "disappointed"
		IL_035a: ldc.i4.0
		IL_035b: callvirt instance void [UnityEngine.AnimationModule]UnityEngine.Animator::SetBool(string, bool)
		IL_0360: ldarg.0
		IL_0361: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PlaytimeScript::agent
		IL_0366: stloc.s 35
		IL_0368: ldarg.0
		IL_0369: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PlaytimeScript::player
		IL_036e: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0373: stloc.s 36
		IL_0375: ldloc.s 35
		IL_0377: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_037c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0381: conv.i8
		IL_0382: ldc.i8 -41
		IL_038b: add
		IL_038c: box [mscorlib]System.Int64
		IL_0391: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0396: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::SetDestination(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_039b: stloc.s 37
		IL_039d: ldloc.s 37
		IL_039f: stloc.s 38
		IL_03a1: ldarg.0
		IL_03a2: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PlaytimeScript::agent
		IL_03a7: ldc.r4 20
		IL_03ac: callvirt instance void [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::set_speed(float32)
		IL_03b1: ldarg.0
		IL_03b2: ldc.r4 0.2
		IL_03b7: stfld float32 PlaytimeScript::coolDown
		IL_03bc: ldarg.0
		IL_03bd: ldfld bool PlaytimeScript::playerSpotted
		IL_03c2: ldc.i4.0
		IL_03c3: ceq
		IL_03c5: ldc.i4.0
		IL_03c6: ceq
		IL_03c8: brtrue.s IL_03e7

		IL_03ca: nop
		IL_03cb: ldarg.0
		IL_03cc: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource PlaytimeScript::audioDevice
		IL_03d1: stloc.s 39
		IL_03d3: ldarg.0
		IL_03d4: ldc.i4.1
		IL_03d5: stfld bool PlaytimeScript::playerSpotted
		IL_03da: ldloc.s 39
		IL_03dc: ldarg.0
		IL_03dd: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip PlaytimeScript::aud_LetsPlay
		IL_03e2: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)

		IL_03e7: nop
		IL_03e8: ldarg.0
		IL_03e9: ldc.i4.0
		IL_03ea: stfld bool PlaytimeScript::jumpRopeStarted
		IL_03ef: br IL_00a9

		IL_03f4: nop
		IL_03f5: ldnull
		IL_03f6: throw
	} // end of method PlaytimeScript::FixedUpdate

	.method private hidebysig 
		instance void Wander () cil managed 
	{
		// Method begins at RVA 0xe584
		// Header size: 12
		// Code size: 230 (0xe6)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[3] bool,
			[4] bool,
			[5] float32,
			[6] int32,
			[7] class [UnityEngine.AudioModule]UnityEngine.AudioSource,
			[8] int32,
			[9] bool,
			[10] bool,
			[11] class [UnityEngine.AudioModule]UnityEngine.AudioClip[]
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class AILocationSelectorScript PlaytimeScript::wanderer
		IL_0007: callvirt instance void AILocationSelectorScript::GetNewTargetHallway()
		IL_000c: ldarg.0
		IL_000d: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PlaytimeScript::agent
		IL_0012: stloc.0
		IL_0013: ldarg.0
		IL_0014: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PlaytimeScript::wanderTarget
		IL_0019: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_001e: stloc.1
		IL_001f: ldloc.0
		IL_0020: ldloca.s 2
		IL_0022: ldloca.s 1
		IL_0024: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0029: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_002e: ldloca.s 2
		IL_0030: ldloca.s 1
		IL_0032: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0037: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_003c: ldloca.s 2
		IL_003e: ldloca.s 1
		IL_0040: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0045: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_004a: ldloc.2
		IL_004b: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::SetDestination(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0050: stloc.3
		IL_0051: ldloc.3
		IL_0052: stloc.s 4
		IL_0054: ldarg.0
		IL_0055: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PlaytimeScript::agent
		IL_005a: ldc.r4 15
		IL_005f: callvirt instance void [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::set_speed(float32)
		IL_0064: ldarg.0
		IL_0065: ldc.i4.0
		IL_0066: stfld bool PlaytimeScript::playerSpotted
		IL_006b: ldc.r4 0.0
		IL_0070: ldc.r4 1
		IL_0075: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_007a: stloc.s 5
		IL_007c: ldloc.s 5
		IL_007e: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_0083: stloc.s 6
		IL_0085: ldarg.0
		IL_0086: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource PlaytimeScript::audioDevice
		IL_008b: stloc.s 7
		IL_008d: ldarg.0
		IL_008e: ldloc.s 6
		IL_0090: stfld int32 PlaytimeScript::audVal
		IL_0095: ldloc.s 6
		IL_0097: stloc.s 8
		IL_0099: ldloc.s 7
		IL_009b: callvirt instance bool [UnityEngine.AudioModule]UnityEngine.AudioSource::get_isPlaying()
		IL_00a0: stloc.s 9
		IL_00a2: ldloc.s 9
		IL_00a4: stloc.s 10
		IL_00a6: ldloc.s 9
		IL_00a8: ldc.i4.0
		IL_00a9: ceq
		IL_00ab: ldc.i4.0
		IL_00ac: ceq
		IL_00ae: brtrue.s IL_00d3

		IL_00b0: nop
		IL_00b1: ldarg.0
		IL_00b2: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] PlaytimeScript::aud_Random
		IL_00b7: stloc.s 11
		IL_00b9: ldarg.0
		IL_00ba: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource PlaytimeScript::audioDevice
		IL_00bf: ldloc.s 11
		IL_00c1: ldarg.0
		IL_00c2: ldfld int32 PlaytimeScript::audVal
		IL_00c7: conv.i8
		IL_00c8: conv.i4
		IL_00c9: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_00ce: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)

		IL_00d3: nop
		IL_00d4: ldarg.0
		IL_00d5: ldc.r4 1
		IL_00da: stfld float32 PlaytimeScript::coolDown
		IL_00df: ret

		IL_00e0: nop
		IL_00e1: ldnull
		IL_00e2: throw

		IL_00e3: nop
		IL_00e4: ldnull
		IL_00e5: throw
	} // end of method PlaytimeScript::Wander

	.method private hidebysig 
		instance void TargetPlayer () cil managed 
	{
		// Method begins at RVA 0xe678
		// Header size: 12
		// Code size: 165 (0xa5)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[3] bool,
			[4] bool,
			[5] class [UnityEngine.AudioModule]UnityEngine.AudioSource
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.AnimationModule]UnityEngine.Animator PlaytimeScript::animator
		IL_0007: ldstr "disappointed"
		IL_000c: ldc.i4.0
		IL_000d: callvirt instance void [UnityEngine.AnimationModule]UnityEngine.Animator::SetBool(string, bool)
		IL_0012: ldarg.0
		IL_0013: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PlaytimeScript::agent
		IL_0018: stloc.0
		IL_0019: ldarg.0
		IL_001a: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PlaytimeScript::player
		IL_001f: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0024: stloc.1
		IL_0025: ldloc.0
		IL_0026: ldloca.s 2
		IL_0028: ldloca.s 1
		IL_002a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_002f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0034: ldloca.s 2
		IL_0036: ldloca.s 1
		IL_0038: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_003d: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0042: ldloca.s 2
		IL_0044: ldloca.s 1
		IL_0046: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_004b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0050: ldloc.2
		IL_0051: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::SetDestination(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0056: stloc.3
		IL_0057: ldloc.3
		IL_0058: stloc.s 4
		IL_005a: ldarg.0
		IL_005b: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PlaytimeScript::agent
		IL_0060: ldc.r4 20
		IL_0065: callvirt instance void [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::set_speed(float32)
		IL_006a: ldarg.0
		IL_006b: ldc.r4 0.2
		IL_0070: stfld float32 PlaytimeScript::coolDown
		IL_0075: ldarg.0
		IL_0076: ldfld bool PlaytimeScript::playerSpotted
		IL_007b: ldc.i4.0
		IL_007c: ceq
		IL_007e: ldc.i4.0
		IL_007f: ceq
		IL_0081: brtrue.s IL_00a0

		IL_0083: nop
		IL_0084: ldarg.0
		IL_0085: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource PlaytimeScript::audioDevice
		IL_008a: stloc.s 5
		IL_008c: ldarg.0
		IL_008d: ldc.i4.1
		IL_008e: stfld bool PlaytimeScript::playerSpotted
		IL_0093: ldloc.s 5
		IL_0095: ldarg.0
		IL_0096: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip PlaytimeScript::aud_LetsPlay
		IL_009b: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)

		IL_00a0: nop
		IL_00a1: ret

		IL_00a2: nop
		IL_00a3: ldnull
		IL_00a4: throw
	} // end of method PlaytimeScript::TargetPlayer

	.method public hidebysig 
		instance void Disappoint () cil managed 
	{
		// Method begins at RVA 0x255e
		// Header size: 1
		// Code size: 50 (0x32)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.AnimationModule]UnityEngine.Animator PlaytimeScript::animator
		IL_0007: ldstr "disappointed"
		IL_000c: ldc.i4.1
		IL_000d: callvirt instance void [UnityEngine.AnimationModule]UnityEngine.Animator::SetBool(string, bool)
		IL_0012: ldarg.0
		IL_0013: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource PlaytimeScript::audioDevice
		IL_0018: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Stop()
		IL_001d: ldarg.0
		IL_001e: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource PlaytimeScript::audioDevice
		IL_0023: ldarg.0
		IL_0024: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip PlaytimeScript::aud_Sad
		IL_0029: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_002e: ret

		IL_002f: nop
		IL_0030: ldnull
		IL_0031: throw
	} // end of method PlaytimeScript::Disappoint

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0xe72c
		// Header size: 12
		// Code size: 37 (0x25)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[1] class [UnityEngine.AudioModule]UnityEngine.AudioClip[]
		)

		IL_0000: nop
		IL_0001: ldc.i4.s 10
		IL_0003: newarr [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0008: stloc.0
		IL_0009: ldarg.0
		IL_000a: ldloc.0
		IL_000b: stfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] PlaytimeScript::aud_Numbers
		IL_0010: ldc.i4.2
		IL_0011: newarr [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0016: stloc.1
		IL_0017: ldarg.0
		IL_0018: ldloc.1
		IL_0019: stfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] PlaytimeScript::aud_Random
		IL_001e: ldarg.0
		IL_001f: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0024: ret
	} // end of method PlaytimeScript::.ctor

} // end of class PlaytimeScript
