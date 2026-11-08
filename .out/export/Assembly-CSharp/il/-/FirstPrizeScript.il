.class public auto ansi beforefieldinit FirstPrizeScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public float32 debug
	.field public float32 turnSpeed
	.field public float32 str
	.field public float32 angleDiff
	.field public float32 normSpeed
	.field public float32 runSpeed
	.field public float32 currentSpeed
	.field public float32 acceleration
	.field public float32 speed
	.field public float32 autoBrakeCool
	.field public float32 crazyTime
	.field public valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion targetRotation
	.field public float32 coolDown
	.field private float32 prevSpeed
	.field public bool playerSeen
	.field public bool hugAnnounced
	.field public class AILocationSelectorScript wanderer
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform player
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform wanderTarget
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip[] aud_Found
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip[] aud_Lost
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip[] aud_Hug
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip[] aud_Random
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip audBang
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioSource audioDevice
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioSource motorAudio
	.field private class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent agent

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0xd258
		// Header size: 12
		// Code size: 33 (0x21)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent>()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent FirstPrizeScript::agent
		IL_000f: ldarg.0
		IL_0010: ldc.r4 1
		IL_0015: stfld float32 FirstPrizeScript::coolDown
		IL_001a: ldarg.0
		IL_001b: callvirt instance void FirstPrizeScript::Wander()
		IL_0020: ret
	} // end of method FirstPrizeScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0xd288
		// Header size: 12
		// Code size: 1054 (0x41e)
		.maxstack 5
		.locals init (
			[0] float32,
			[1] int64,
			[2] float32,
			[3] float32,
			[4] float32,
			[5] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[6] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[7] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[8] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[9] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[10] float32,
			[11] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[12] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[13] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[14] float32,
			[15] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[16] int64,
			[17] int64,
			[18] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[19] float32,
			[20] float32,
			[21] float32,
			[22] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[23] float32,
			[24] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[25] float32,
			[26] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[27] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[28] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[29] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[30] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[31] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[32] float32,
			[33] class [UnityEngine.AudioModule]UnityEngine.AudioSource,
			[34] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[35] float32,
			[36] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld float32 FirstPrizeScript::coolDown
		IL_0007: stloc.0
		IL_0008: ldstr "&stack[0x0]"
		IL_000d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0012: conv.i8
		IL_0013: ldc.i8 -95
		IL_001c: add
		IL_001d: stloc.1
		IL_001e: ldloc.0
		IL_001f: stloc.2
		IL_0020: ldloc.0
		IL_0021: ldc.r4 0.0
		IL_0026: cgt
		IL_0028: ldc.i4.0
		IL_0029: ceq
		IL_002b: brtrue.s IL_003d

		IL_002d: nop
		IL_002e: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0033: stloc.3
		IL_0034: ldarg.0
		IL_0035: ldloc.2
		IL_0036: ldloc.3
		IL_0037: sub
		IL_0038: stfld float32 FirstPrizeScript::coolDown

		IL_003d: nop
		IL_003e: ldarg.0
		IL_003f: ldfld float32 FirstPrizeScript::autoBrakeCool
		IL_0044: stloc.2
		IL_0045: ldarg.0
		IL_0046: ldfld float32 FirstPrizeScript::autoBrakeCool
		IL_004b: ldc.r4 0.0
		IL_0050: cgt
		IL_0052: brtrue.s IL_0063

		IL_0054: nop
		IL_0055: ldarg.0
		IL_0056: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent FirstPrizeScript::agent
		IL_005b: ldc.i4.1
		IL_005c: callvirt instance void [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::set_autoBraking(bool)
		IL_0061: br.s IL_0078

		IL_0063: nop
		IL_0064: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0069: stloc.s 4
		IL_006b: ldarg.0
		IL_006c: ldloc.2
		IL_006d: ldloc.s 4
		IL_006f: sub
		IL_0070: stfld float32 FirstPrizeScript::autoBrakeCool
		IL_0075: ldloc.s 4
		IL_0077: stloc.2

		IL_0078: nop
		IL_0079: ldarg.0
		IL_007a: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_007f: stloc.s 5
		IL_0081: ldloc.s 5
		IL_0083: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_eulerAngles()
		IL_0088: stloc.s 6
		IL_008a: ldarg.0
		IL_008b: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent FirstPrizeScript::agent
		IL_0090: stloc.s 7
		IL_0092: ldloc.s 7
		IL_0094: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::get_steeringTarget()
		IL_0099: stloc.s 8
		IL_009b: ldarg.0
		IL_009c: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_00a1: stloc.s 9
		IL_00a3: ldloca.s 8
		IL_00a5: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00aa: stloc.s 10
		IL_00ac: ldloc.s 9
		IL_00ae: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_00b3: stloc.s 11
		IL_00b5: ldarg.0
		IL_00b6: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent FirstPrizeScript::agent
		IL_00bb: stloc.s 7
		IL_00bd: ldloc.s 7
		IL_00bf: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::get_steeringTarget()
		IL_00c4: stloc.s 12
		IL_00c6: ldarg.0
		IL_00c7: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_00cc: stloc.s 13
		IL_00ce: ldloca.s 12
		IL_00d0: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00d5: stloc.s 14
		IL_00d7: ldloc.s 13
		IL_00d9: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_00de: stloc.s 15
		IL_00e0: ldstr "sub_20BC10"
		IL_00e5: ldc.i8 0
		IL_00ee: ldc.i8 0
		IL_00f7: ldc.i8 0
		IL_0100: ldc.i8 0
		IL_0109: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_010e: stloc.s 16
		IL_0110: ldstr "sub_20C2B0"
		IL_0115: ldc.i8 0
		IL_011e: ldc.i8 0
		IL_0127: ldc.i8 0
		IL_0130: ldc.i8 0
		IL_0139: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_013e: stloc.s 17
		IL_0140: ldstr "xmm0"
		IL_0145: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_014a: conv.r4
		IL_014b: ldc.r4 360
		IL_0150: mul
		IL_0151: stloc.2
		IL_0152: ldstr "xmm0"
		IL_0157: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_015c: conv.r4
		IL_015d: ldc.r4 57.29578
		IL_0162: mul
		IL_0163: ldloca.s 6
		IL_0165: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_016a: sub
		IL_016b: ldstr "xmm0"
		IL_0170: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0175: conv.r4
		IL_0176: ldc.r4 360
		IL_017b: mul
		IL_017c: sub
		IL_017d: stloc.s 14
		IL_017f: ldc.r4 0.0
		IL_0184: ldstr "xmm0"
		IL_0189: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_018e: conv.r4
		IL_018f: ldc.r4 57.29578
		IL_0194: mul
		IL_0195: ldloca.s 6
		IL_0197: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_019c: sub
		IL_019d: ldstr "xmm0"
		IL_01a2: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01a7: conv.r4
		IL_01a8: ldc.r4 360
		IL_01ad: mul
		IL_01ae: sub
		IL_01af: cgt
		IL_01b1: brtrue.s IL_01c5

		IL_01b3: nop
		IL_01b4: ldloc.s 14
		IL_01b6: ldc.r4 360
		IL_01bb: cgt
		IL_01bd: ldc.i4.0
		IL_01be: ceq
		IL_01c0: brtrue.s IL_01c6

		IL_01c2: nop
		IL_01c3: br.s IL_01c6

		IL_01c5: nop

		IL_01c6: nop
		IL_01c7: ldc.r4 0.0
		IL_01cc: ldc.r4 180
		IL_01d1: cgt
		IL_01d3: ldc.i4.0
		IL_01d4: ceq
		IL_01d6: brtrue.s IL_01e6

		IL_01d8: nop
		IL_01d9: ldc.r4 0.0
		IL_01de: ldc.r4 360
		IL_01e3: sub
		IL_01e4: stloc.s 14

		IL_01e6: nop
		IL_01e7: ldarg.0
		IL_01e8: ldloc.s 14
		IL_01ea: stfld float32 FirstPrizeScript::angleDiff
		IL_01ef: ldc.r4 0.0
		IL_01f4: ldarg.0
		IL_01f5: ldfld float32 FirstPrizeScript::crazyTime
		IL_01fa: clt
		IL_01fc: ldc.i4.0
		IL_01fd: ceq
		IL_01ff: brtrue IL_0289

		IL_0204: nop
		IL_0205: ldarg.0
		IL_0206: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent FirstPrizeScript::agent
		IL_020b: ldc.r4 0.0
		IL_0210: callvirt instance void [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::set_speed(float32)
		IL_0215: ldarg.0
		IL_0216: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_021b: stloc.s 18
		IL_021d: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0222: stloc.s 19
		IL_0224: ldloc.s 19
		IL_0226: ldc.r4 180
		IL_022b: mul
		IL_022c: stloc.2
		IL_022d: ldstr "movss dword ptr [rbp-15h],xmm0 ; value = LocalExpr"
		IL_0232: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0237: ldstr "mov dword ptr [rbp-19h],0 ; value = 0x0"
		IL_023c: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0241: ldstr "mov dword ptr [rbp-11h],0 ; value = 0x0"
		IL_0246: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_024b: ldloc.s 18
		IL_024d: ldloc.1
		IL_024e: ldc.i8 -25
		IL_0257: add
		IL_0258: box [mscorlib]System.Int64
		IL_025d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0262: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::Rotate(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0267: ldarg.0
		IL_0268: ldfld float32 FirstPrizeScript::crazyTime
		IL_026d: stloc.s 20
		IL_026f: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0274: stloc.s 21
		IL_0276: ldarg.0
		IL_0277: ldloc.s 20
		IL_0279: ldloc.s 21
		IL_027b: sub
		IL_027c: stfld float32 FirstPrizeScript::crazyTime
		IL_0281: ldloc.s 21
		IL_0283: stloc.2
		IL_0284: br IL_03c5

		IL_0289: nop
		IL_028a: ldc.r4 5
		IL_028f: ldstr "andps xmm6,[1813EAC20h]"
		IL_0294: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0299: box [mscorlib]System.Int64
		IL_029e: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_02a3: cgt
		IL_02a5: brtrue IL_0334

		IL_02aa: nop
		IL_02ab: ldarg.0
		IL_02ac: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_02b1: stloc.s 22
		IL_02b3: ldarg.0
		IL_02b4: ldfld float32 FirstPrizeScript::angleDiff
		IL_02b9: stloc.2
		IL_02ba: ldarg.0
		IL_02bb: ldfld float32 FirstPrizeScript::turnSpeed
		IL_02c0: stloc.s 23
		IL_02c2: ldloc.s 22
		IL_02c4: stloc.s 24
		IL_02c6: ldarg.0
		IL_02c7: ldfld float32 FirstPrizeScript::angleDiff
		IL_02cc: ldc.r4 0.0
		IL_02d1: clt
		IL_02d3: brtrue.s IL_02d8

		IL_02d5: nop
		IL_02d6: br.s IL_02d9

		IL_02d8: nop

		IL_02d9: nop
		IL_02da: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_02df: stloc.s 25
		IL_02e1: ldloc.s 25
		IL_02e3: ldc.r4 -1
		IL_02e8: ldloc.s 23
		IL_02ea: mul
		IL_02eb: mul
		IL_02ec: stloc.2
		IL_02ed: ldstr "movss dword ptr [rbp-15h],xmm0 ; value = LocalExpr"
		IL_02f2: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_02f7: ldstr "mov dword ptr [rbp-19h],0 ; value = 0x0"
		IL_02fc: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0301: ldstr "mov dword ptr [rbp-11h],0 ; value = 0x0"
		IL_0306: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_030b: ldloc.s 24
		IL_030d: ldloc.1
		IL_030e: ldc.i8 -25
		IL_0317: add
		IL_0318: box [mscorlib]System.Int64
		IL_031d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0322: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::Rotate(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0327: ldarg.0
		IL_0328: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent FirstPrizeScript::agent
		IL_032d: stloc.s 26
		IL_032f: br IL_03bb

		IL_0334: nop
		IL_0335: ldloc.s 26
		IL_0337: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_033c: stloc.s 27
		IL_033e: ldarg.0
		IL_033f: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent FirstPrizeScript::agent
		IL_0344: stloc.s 7
		IL_0346: ldloc.s 7
		IL_0348: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::get_steeringTarget()
		IL_034d: stloc.s 28
		IL_034f: ldarg.0
		IL_0350: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0355: stloc.s 29
		IL_0357: ldloc.s 29
		IL_0359: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_035e: stloc.s 30
		IL_0360: ldarg.0
		IL_0361: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent FirstPrizeScript::agent
		IL_0366: stloc.s 7
		IL_0368: ldloc.s 7
		IL_036a: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::get_steeringTarget()
		IL_036f: stloc.s 31
		IL_0371: ldstr "movss dword ptr [rbp-11h],xmm0 ; value = FieldExpr"
		IL_0376: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_037b: ldstr "movss dword ptr [rbp-19h],xmm7 ; value = FieldExpr"
		IL_0380: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0385: ldstr "movss dword ptr [rbp-15h],xmm6 ; value = FieldExpr"
		IL_038a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_038f: ldloc.s 27
		IL_0391: ldloc.1
		IL_0392: ldc.i8 -25
		IL_039b: add
		IL_039c: box [mscorlib]System.Int64
		IL_03a1: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_03a6: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::LookAt(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_03ab: ldarg.0
		IL_03ac: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent FirstPrizeScript::agent
		IL_03b1: stloc.s 26
		IL_03b3: ldarg.0
		IL_03b4: ldfld float32 FirstPrizeScript::currentSpeed
		IL_03b9: stloc.s 32

		IL_03bb: nop
		IL_03bc: ldloc.s 26
		IL_03be: ldloc.s 32
		IL_03c0: callvirt instance void [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::set_speed(float32)

		IL_03c5: nop
		IL_03c6: ldarg.0
		IL_03c7: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource FirstPrizeScript::motorAudio
		IL_03cc: stloc.s 33
		IL_03ce: ldarg.0
		IL_03cf: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent FirstPrizeScript::agent
		IL_03d4: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::get_velocity()
		IL_03d9: stloc.s 34
		IL_03db: ldstr "movsd qword ptr [rbp-19h],xmm0 ; value = StructChunkExpr"
		IL_03e0: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_03e5: ldstr "mov [rbp-11h],eax ; value = FieldExpr"
		IL_03ea: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_03ef: ldloc.1
		IL_03f0: ldc.i8 -25
		IL_03f9: add
		IL_03fa: call instance float32 [UnityEngine.CoreModule]UnityEngine.Vector3::get_magnitude()
		IL_03ff: stloc.s 35
		IL_0401: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_timeScale()
		IL_0406: stloc.s 36
		IL_0408: ldloc.s 33
		IL_040a: ldloc.s 35
		IL_040c: ldc.r4 1
		IL_0411: add
		IL_0412: ldloc.s 36
		IL_0414: mul
		IL_0415: call void [UnityEngine.AudioModule]UnityEngine.AudioSource::SetPitch(class [UnityEngine.AudioModule]UnityEngine.AudioSource, float32)
		IL_041a: ret

		IL_041b: nop
		IL_041c: ldnull
		IL_041d: throw
	} // end of method FirstPrizeScript::Update

	.method private hidebysig 
		instance void FixedUpdate () cil managed 
	{
		// Method begins at RVA 0xd6b4
		// Header size: 12
		// Code size: 842 (0x34a)
		.maxstack 6
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[3] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[4] float32,
			[5] float32,
			[6] float32,
			[7] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[8] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[9] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[10] int64,
			[11] float32,
			[12] bool,
			[13] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[14] string,
			[15] bool,
			[16] int32,
			[17] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[18] float32,
			[19] float32,
			[20] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[21] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[22] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[23] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[24] float32,
			[25] int32,
			[26] float32,
			[27] class [UnityEngine.AudioModule]UnityEngine.AudioSource,
			[28] bool,
			[29] float32,
			[30] int32,
			[31] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[32] bool,
			[33] float32,
			[34] int32,
			[35] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[36] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[37] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[38] bool,
			[39] float32,
			[40] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform FirstPrizeScript::player
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: stloc.1
		IL_000a: ldloc.1
		IL_000b: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0010: stloc.2
		IL_0011: ldarg.0
		IL_0012: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0017: stloc.3
		IL_0018: ldloca.s 2
		IL_001a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_001f: stloc.s 4
		IL_0021: ldloca.s 2
		IL_0023: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0028: stloc.s 5
		IL_002a: ldloca.s 2
		IL_002c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0031: stloc.s 6
		IL_0033: ldloc.3
		IL_0034: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0039: stloc.s 7
		IL_003b: ldarg.0
		IL_003c: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0041: stloc.s 8
		IL_0043: ldloc.s 4
		IL_0045: ldloca.s 7
		IL_0047: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_004c: sub
		IL_004d: stloc.s 4
		IL_004f: ldloc.s 5
		IL_0051: ldloca.s 7
		IL_0053: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0058: sub
		IL_0059: stloc.s 5
		IL_005b: ldloc.s 6
		IL_005d: ldloca.s 7
		IL_005f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0064: sub
		IL_0065: stloc.s 6
		IL_0067: ldloc.s 8
		IL_0069: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_006e: stloc.s 9
		IL_0070: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_0075: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_007a: stloc.s 10
		IL_007c: ldloca.s 9
		IL_007e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0083: stloc.s 11
		IL_0085: br.s IL_0088

		IL_0087: nop

		IL_0088: nop
		IL_0089: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_008e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0093: conv.i8
		IL_0094: ldc.i8 -73
		IL_009d: add
		IL_009e: box [mscorlib]System.Int64
		IL_00a3: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_00a8: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_00ad: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00b2: conv.i8
		IL_00b3: ldc.i8 -89
		IL_00bc: add
		IL_00bd: box [mscorlib]System.Int64
		IL_00c2: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_00c7: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_00cc: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00d1: conv.i8
		IL_00d2: ldc.i8 -57
		IL_00db: add
		IL_00dc: ldc.r4 (00 00 80 7F)
		IL_00e1: ldc.i4 769
		IL_00e6: ldc.i4.1
		IL_00e7: box [mscorlib]System.Int32
		IL_00ec: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.PhysicsModule]UnityEngine.QueryTriggerInteraction>(object)
		IL_00f1: call bool [UnityEngine.PhysicsModule]UnityEngine.Physics::Raycast(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit&, float32, int32, valuetype [UnityEngine.PhysicsModule]UnityEngine.QueryTriggerInteraction)
		IL_00f6: stloc.s 12
		IL_00f8: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_00fd: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0102: conv.i8
		IL_0103: ldc.i8 -57
		IL_010c: add
		IL_010d: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_0112: stloc.s 13
		IL_0114: ldloc.s 13
		IL_0116: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Component::get_tag()
		IL_011b: stloc.s 14
		IL_011d: ldloc.s 14
		IL_011f: ldstr "Player"
		IL_0124: call bool [mscorlib]System.String::Equals(string, string)
		IL_0129: stloc.s 15
		IL_012b: ldarg.0
		IL_012c: ldfld bool FirstPrizeScript::playerSeen
		IL_0131: stloc.s 16
		IL_0133: ldloc.s 12
		IL_0135: ldloc.s 15
		IL_0137: and
		IL_0138: ldc.i4.0
		IL_0139: ceq
		IL_013b: ldc.i4.0
		IL_013c: ceq
		IL_013e: brtrue IL_027b

		IL_0143: nop
		IL_0144: ldarg.0
		IL_0145: ldarg.0
		IL_0146: ldfld float32 FirstPrizeScript::normSpeed
		IL_014b: stfld float32 FirstPrizeScript::currentSpeed
		IL_0150: ldloc.s 16
		IL_0152: ldc.r4 0.0
		IL_0157: ldarg.0
		IL_0158: ldfld float32 FirstPrizeScript::coolDown
		IL_015d: clt
		IL_015f: ldc.i4.0
		IL_0160: ceq
		IL_0162: and
		IL_0163: ldc.i4.0
		IL_0164: ceq
		IL_0166: ldc.i4.0
		IL_0167: ceq
		IL_0169: brtrue IL_021c

		IL_016e: nop
		IL_016f: ldarg.0
		IL_0170: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent FirstPrizeScript::agent
		IL_0175: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::get_velocity()
		IL_017a: stloc.s 17
		IL_017c: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0181: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0186: conv.i8
		IL_0187: ldc.i8 -89
		IL_0190: add
		IL_0191: call instance float32 [UnityEngine.CoreModule]UnityEngine.Vector3::get_magnitude()
		IL_0196: stloc.s 18
		IL_0198: ldarg.0
		IL_0199: ldfld float32 FirstPrizeScript::coolDown
		IL_019e: stloc.s 19
		IL_01a0: ldarg.0
		IL_01a1: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_01a6: stloc.s 20
		IL_01a8: ldloc.s 20
		IL_01aa: stloc.s 21
		IL_01ac: ldloc.s 21
		IL_01ae: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_01b3: stloc.s 22
		IL_01b5: ldarg.0
		IL_01b6: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent FirstPrizeScript::agent
		IL_01bb: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::get_destination()
		IL_01c0: stloc.s 23
		IL_01c2: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_01c7: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01cc: conv.i8
		IL_01cd: ldc.i8 -89
		IL_01d6: add
		IL_01d7: call instance float32 [UnityEngine.CoreModule]UnityEngine.Vector3::get_magnitude()
		IL_01dc: stloc.s 24
		IL_01de: ldc.r4 1
		IL_01e3: ldloc.s 18
		IL_01e5: clt
		IL_01e7: brfalse.s IL_01ee

		IL_01e9: ldc.i4.0
		IL_01ea: stloc.s 25
		IL_01ec: br.s IL_0207

		IL_01ee: nop
		IL_01ef: ldc.r4 5
		IL_01f4: ldloc.s 24
		IL_01f6: cgt
		IL_01f8: ldc.r4 0.0
		IL_01fd: ldloc.s 19
		IL_01ff: clt
		IL_0201: ldc.i4.0
		IL_0202: ceq
		IL_0204: and
		IL_0205: stloc.s 25

		IL_0207: nop
		IL_0208: ldloc.s 24
		IL_020a: stloc.s 26
		IL_020c: ldloc.s 25
		IL_020e: ldc.i4.0
		IL_020f: ceq
		IL_0211: brtrue.s IL_021a

		IL_0213: nop
		IL_0214: ldarg.0
		IL_0215: callvirt instance void FirstPrizeScript::Wander()

		IL_021a: nop
		IL_021b: ret

		IL_021c: nop
		IL_021d: ldarg.0
		IL_021e: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource FirstPrizeScript::audioDevice
		IL_0223: stloc.s 27
		IL_0225: ldloc.s 27
		IL_0227: callvirt instance bool [UnityEngine.AudioModule]UnityEngine.AudioSource::get_isPlaying()
		IL_022c: stloc.s 28
		IL_022e: ldloc.s 28
		IL_0230: ldc.i4.0
		IL_0231: ceq
		IL_0233: ldc.i4.0
		IL_0234: ceq
		IL_0236: brtrue.s IL_0271

		IL_0238: nop
		IL_0239: ldc.r4 0.0
		IL_023e: ldc.r4 1
		IL_0243: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_0248: stloc.s 29
		IL_024a: ldloc.s 29
		IL_024c: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_0251: stloc.s 30
		IL_0253: ldarg.0
		IL_0254: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] FirstPrizeScript::aud_Lost
		IL_0259: stloc.s 31
		IL_025b: ldarg.0
		IL_025c: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource FirstPrizeScript::audioDevice
		IL_0261: ldloc.s 31
		IL_0263: ldloc.s 30
		IL_0265: conv.i8
		IL_0266: conv.i4
		IL_0267: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_026c: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)

		IL_0271: nop
		IL_0272: ldarg.0
		IL_0273: ldc.i4.0
		IL_0274: stfld bool FirstPrizeScript::playerSeen
		IL_0279: br.s IL_0213

		IL_027b: nop
		IL_027c: ldloc.s 27
		IL_027e: ldnull
		IL_027f: ceq
		IL_0281: ldc.i4.0
		IL_0282: ceq
		IL_0284: brtrue.s IL_02d7

		IL_0286: nop
		IL_0287: ldarg.0
		IL_0288: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource FirstPrizeScript::audioDevice
		IL_028d: stloc.s 27
		IL_028f: ldloc.s 27
		IL_0291: callvirt instance bool [UnityEngine.AudioModule]UnityEngine.AudioSource::get_isPlaying()
		IL_0296: stloc.s 32
		IL_0298: ldloc.s 32
		IL_029a: ldc.i4.0
		IL_029b: ceq
		IL_029d: ldc.i4.0
		IL_029e: ceq
		IL_02a0: brtrue.s IL_02d7

		IL_02a2: nop
		IL_02a3: ldc.r4 0.0
		IL_02a8: ldc.r4 1
		IL_02ad: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_02b2: stloc.s 33
		IL_02b4: ldloc.s 33
		IL_02b6: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_02bb: stloc.s 34
		IL_02bd: ldarg.0
		IL_02be: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource FirstPrizeScript::audioDevice
		IL_02c3: ldarg.0
		IL_02c4: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] FirstPrizeScript::aud_Found
		IL_02c9: ldloc.s 34
		IL_02cb: conv.i8
		IL_02cc: conv.i4
		IL_02cd: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_02d2: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)

		IL_02d7: nop
		IL_02d8: ldarg.0
		IL_02d9: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform FirstPrizeScript::player
		IL_02de: stloc.s 35
		IL_02e0: ldarg.0
		IL_02e1: ldc.i4.1
		IL_02e2: stfld bool FirstPrizeScript::playerSeen
		IL_02e7: ldarg.0
		IL_02e8: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent FirstPrizeScript::agent
		IL_02ed: stloc.s 36
		IL_02ef: ldloc.s 35
		IL_02f1: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_02f6: stloc.s 37
		IL_02f8: ldloc.s 36
		IL_02fa: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_02ff: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0304: conv.i8
		IL_0305: ldc.i8 -73
		IL_030e: add
		IL_030f: box [mscorlib]System.Int64
		IL_0314: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0319: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::SetDestination(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_031e: stloc.s 38
		IL_0320: ldarg.0
		IL_0321: ldfld float32 FirstPrizeScript::runSpeed
		IL_0326: stloc.s 39
		IL_0328: ldarg.0
		IL_0329: ldloc.s 39
		IL_032b: stfld float32 FirstPrizeScript::currentSpeed
		IL_0330: ldarg.0
		IL_0331: ldc.r4 0.5
		IL_0336: stfld float32 FirstPrizeScript::coolDown
		IL_033b: ldloc.s 39
		IL_033d: stloc.s 40
		IL_033f: br IL_021a

		IL_0344: nop
		IL_0345: ldnull
		IL_0346: throw

		IL_0347: nop
		IL_0348: ldnull
		IL_0349: throw
	} // end of method FirstPrizeScript::FixedUpdate

	.method private hidebysig 
		instance void Wander () cil managed 
	{
		// Method begins at RVA 0xda0c
		// Header size: 12
		// Code size: 273 (0x111)
		.maxstack 3
		.locals init (
			[0] class AILocationSelectorScript,
			[1] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[2] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[4] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[5] bool,
			[6] float32,
			[7] int32,
			[8] int32,
			[9] class [UnityEngine.AudioModule]UnityEngine.AudioSource,
			[10] bool,
			[11] int32,
			[12] int32,
			[13] float32,
			[14] int32,
			[15] class [UnityEngine.AudioModule]UnityEngine.AudioSource
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class AILocationSelectorScript FirstPrizeScript::wanderer
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: callvirt instance void AILocationSelectorScript::GetNewTargetHallway()
		IL_000e: ldarg.0
		IL_000f: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform FirstPrizeScript::wanderTarget
		IL_0014: stloc.1
		IL_0015: ldarg.0
		IL_0016: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent FirstPrizeScript::agent
		IL_001b: stloc.2
		IL_001c: ldloc.1
		IL_001d: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0022: stloc.3
		IL_0023: ldloc.2
		IL_0024: ldloca.s 4
		IL_0026: ldloca.s 3
		IL_0028: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_002d: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0032: ldloca.s 4
		IL_0034: ldloca.s 3
		IL_0036: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_003b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0040: ldloca.s 4
		IL_0042: ldloca.s 3
		IL_0044: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0049: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_004e: ldloc.s 4
		IL_0050: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::SetDestination(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0055: stloc.s 5
		IL_0057: ldarg.0
		IL_0058: ldc.i4.0
		IL_0059: stfld bool FirstPrizeScript::hugAnnounced
		IL_005e: ldc.r4 0.0
		IL_0063: ldc.r4 9
		IL_0068: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_006d: stloc.s 6
		IL_006f: ldloc.s 6
		IL_0071: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_0076: stloc.s 7
		IL_0078: ldloc.s 7
		IL_007a: stloc.s 8
		IL_007c: ldarg.0
		IL_007d: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource FirstPrizeScript::audioDevice
		IL_0082: stloc.s 9
		IL_0084: ldloc.s 9
		IL_0086: callvirt instance bool [UnityEngine.AudioModule]UnityEngine.AudioSource::get_isPlaying()
		IL_008b: stloc.s 10
		IL_008d: ldc.r4 0.0
		IL_0092: ldarg.0
		IL_0093: ldfld float32 FirstPrizeScript::coolDown
		IL_0098: clt
		IL_009a: brfalse.s IL_00a1

		IL_009c: ldc.i4.0
		IL_009d: stloc.s 11
		IL_009f: br.s IL_00a8

		IL_00a1: nop
		IL_00a2: ldloc.s 10
		IL_00a4: ldc.i4.1
		IL_00a5: xor
		IL_00a6: stloc.s 11

		IL_00a8: nop
		IL_00a9: ldloc.s 7
		IL_00ab: ldc.i4.0
		IL_00ac: ceq
		IL_00ae: stloc.s 8
		IL_00b0: ldloc.s 11
		IL_00b2: stloc.s 12
		IL_00b4: ldloc.s 7
		IL_00b6: ldc.i4.0
		IL_00b7: ceq
		IL_00b9: ldloc.s 11
		IL_00bb: and
		IL_00bc: ldc.i4.0
		IL_00bd: ceq
		IL_00bf: brtrue.s IL_00fe

		IL_00c1: nop
		IL_00c2: ldc.r4 0.0
		IL_00c7: ldc.r4 1
		IL_00cc: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_00d1: stloc.s 13
		IL_00d3: ldloc.s 13
		IL_00d5: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_00da: stloc.s 14
		IL_00dc: ldloc.s 14
		IL_00de: stloc.s 8
		IL_00e0: ldarg.0
		IL_00e1: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource FirstPrizeScript::audioDevice
		IL_00e6: stloc.s 15
		IL_00e8: ldloc.s 15
		IL_00ea: ldarg.0
		IL_00eb: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] FirstPrizeScript::aud_Random
		IL_00f0: ldloc.s 14
		IL_00f2: conv.i8
		IL_00f3: conv.i4
		IL_00f4: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_00f9: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)

		IL_00fe: nop
		IL_00ff: ldarg.0
		IL_0100: ldc.r4 1
		IL_0105: stfld float32 FirstPrizeScript::coolDown
		IL_010a: ret

		IL_010b: nop
		IL_010c: ldnull
		IL_010d: throw

		IL_010e: nop
		IL_010f: ldnull
		IL_0110: throw
	} // end of method FirstPrizeScript::Wander

	.method private hidebysig 
		instance void TargetPlayer () cil managed 
	{
		// Method begins at RVA 0xdb2c
		// Header size: 12
		// Code size: 85 (0x55)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[3] bool
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent FirstPrizeScript::agent
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform FirstPrizeScript::player
		IL_000e: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0013: stloc.1
		IL_0014: ldloc.0
		IL_0015: ldloca.s 2
		IL_0017: ldloca.s 1
		IL_0019: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_001e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0023: ldloca.s 2
		IL_0025: ldloca.s 1
		IL_0027: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_002c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0031: ldloca.s 2
		IL_0033: ldloca.s 1
		IL_0035: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_003a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_003f: ldloc.2
		IL_0040: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::SetDestination(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0045: stloc.3
		IL_0046: ldarg.0
		IL_0047: ldc.r4 0.5
		IL_004c: stfld float32 FirstPrizeScript::coolDown
		IL_0051: ret

		IL_0052: nop
		IL_0053: ldnull
		IL_0054: throw
	} // end of method FirstPrizeScript::TargetPlayer

	.method private hidebysig 
		instance void OnTriggerEnter (
			class [UnityEngine.PhysicsModule]UnityEngine.Collider other
		) cil managed 
	{
		// Method begins at RVA 0xdb90
		// Header size: 12
		// Code size: 169 (0xa9)
		.maxstack 3
		.locals init (
			[0] string,
			[1] bool,
			[2] bool,
			[3] class [UnityEngine.AudioModule]UnityEngine.AudioSource,
			[4] bool,
			[5] int32,
			[6] int32,
			[7] float32,
			[8] int32,
			[9] class [UnityEngine.AudioModule]UnityEngine.AudioClip[]
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Component::get_tag()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: ldstr "Player"
		IL_000e: call bool [mscorlib]System.String::Equals(string, string)
		IL_0013: stloc.1
		IL_0014: ldloc.1
		IL_0015: stloc.2
		IL_0016: ldloc.1
		IL_0017: ldc.i4.0
		IL_0018: ceq
		IL_001a: brtrue IL_00a1

		IL_001f: nop
		IL_0020: ldarg.0
		IL_0021: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource FirstPrizeScript::audioDevice
		IL_0026: stloc.3
		IL_0027: ldloc.3
		IL_0028: callvirt instance bool [UnityEngine.AudioModule]UnityEngine.AudioSource::get_isPlaying()
		IL_002d: stloc.s 4
		IL_002f: ldarg.0
		IL_0030: ldfld bool FirstPrizeScript::hugAnnounced
		IL_0035: ldc.i4.0
		IL_0036: ceq
		IL_0038: brfalse.s IL_0042

		IL_003a: ldloc.s 4
		IL_003c: ldc.i4.1
		IL_003d: xor
		IL_003e: stloc.s 5
		IL_0040: br.s IL_0046

		IL_0042: nop
		IL_0043: ldc.i4.0
		IL_0044: stloc.s 5

		IL_0046: nop
		IL_0047: ldloc.s 4
		IL_0049: ldc.i4.1
		IL_004a: xor
		IL_004b: stloc.s 6
		IL_004d: ldloc.s 5
		IL_004f: ldc.i4.0
		IL_0050: ceq
		IL_0052: brtrue.s IL_0094

		IL_0054: nop
		IL_0055: ldc.r4 0.0
		IL_005a: ldc.r4 1
		IL_005f: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_0064: stloc.s 7
		IL_0066: ldloc.s 7
		IL_0068: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_006d: stloc.s 8
		IL_006f: ldarg.0
		IL_0070: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] FirstPrizeScript::aud_Hug
		IL_0075: stloc.s 9
		IL_0077: ldarg.0
		IL_0078: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource FirstPrizeScript::audioDevice
		IL_007d: ldloc.s 9
		IL_007f: ldloc.s 8
		IL_0081: conv.i8
		IL_0082: conv.i4
		IL_0083: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0088: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_008d: ldarg.0
		IL_008e: ldc.i4.1
		IL_008f: stfld bool FirstPrizeScript::hugAnnounced

		IL_0094: nop
		IL_0095: ldarg.0
		IL_0096: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent FirstPrizeScript::agent
		IL_009b: ldc.i4.0
		IL_009c: callvirt instance void [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::set_autoBraking(bool)

		IL_00a1: nop
		IL_00a2: ret

		IL_00a3: nop
		IL_00a4: ldnull
		IL_00a5: throw

		IL_00a6: nop
		IL_00a7: ldnull
		IL_00a8: throw
	} // end of method FirstPrizeScript::OnTriggerEnter

	.method private hidebysig 
		instance void OnTriggerExit (
			class [UnityEngine.PhysicsModule]UnityEngine.Collider other
		) cil managed 
	{
		// Method begins at RVA 0xdc48
		// Header size: 12
		// Code size: 45 (0x2d)
		.maxstack 2
		.locals init (
			[0] string,
			[1] bool,
			[2] bool
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Component::get_tag()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: ldstr "Player"
		IL_000e: call bool [mscorlib]System.String::Equals(string, string)
		IL_0013: stloc.1
		IL_0014: ldloc.1
		IL_0015: stloc.2
		IL_0016: ldloc.1
		IL_0017: ldc.i4.0
		IL_0018: ceq
		IL_001a: brtrue.s IL_0028

		IL_001c: nop
		IL_001d: ldarg.0
		IL_001e: ldc.r4 1
		IL_0023: stfld float32 FirstPrizeScript::autoBrakeCool

		IL_0028: nop
		IL_0029: ret

		IL_002a: nop
		IL_002b: ldnull
		IL_002c: throw
	} // end of method FirstPrizeScript::OnTriggerExit

	.method public hidebysig 
		instance void GoCrazy () cil managed 
	{
		// Method begins at RVA 0x2534
		// Header size: 1
		// Code size: 13 (0xd)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.r4 15
		IL_0007: stfld float32 FirstPrizeScript::crazyTime
		IL_000c: ret
	} // end of method FirstPrizeScript::GoCrazy

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0xdc84
		// Header size: 12
		// Code size: 64 (0x40)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[1] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[2] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[3] class [UnityEngine.AudioModule]UnityEngine.AudioClip[]
		)

		IL_0000: nop
		IL_0001: ldc.i4.2
		IL_0002: newarr [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] FirstPrizeScript::aud_Found
		IL_000f: ldc.i4.2
		IL_0010: newarr [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0015: stloc.1
		IL_0016: ldarg.0
		IL_0017: ldloc.1
		IL_0018: stfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] FirstPrizeScript::aud_Lost
		IL_001d: ldc.i4.2
		IL_001e: newarr [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0023: stloc.2
		IL_0024: ldarg.0
		IL_0025: ldloc.2
		IL_0026: stfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] FirstPrizeScript::aud_Hug
		IL_002b: ldc.i4.2
		IL_002c: newarr [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0031: stloc.3
		IL_0032: ldarg.0
		IL_0033: ldloc.3
		IL_0034: stfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] FirstPrizeScript::aud_Random
		IL_0039: ldarg.0
		IL_003a: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_003f: ret
	} // end of method FirstPrizeScript::.ctor

} // end of class FirstPrizeScript
