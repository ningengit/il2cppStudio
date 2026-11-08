.class public auto ansi beforefieldinit AgentTest
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public bool db
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform player
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform wanderTarget
	.field public class AILocationSelectorScript wanderer
	.field public float32 coolDown
	.field private class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent agent

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0xb40c
		// Header size: 12
		// Code size: 22 (0x16)
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
		IL_000a: stfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent AgentTest::agent
		IL_000f: ldarg.0
		IL_0010: callvirt instance void AgentTest::Wander()
		IL_0015: ret
	} // end of method AgentTest::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0xb430
		// Header size: 12
		// Code size: 46 (0x2e)
		.maxstack 3
		.locals init (
			[0] float32,
			[1] float32,
			[2] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld float32 AgentTest::coolDown
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld float32 AgentTest::coolDown
		IL_000e: ldc.r4 0.0
		IL_0013: cgt
		IL_0015: ldc.i4.0
		IL_0016: ceq
		IL_0018: brtrue.s IL_002c

		IL_001a: nop
		IL_001b: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0020: stloc.1
		IL_0021: ldarg.0
		IL_0022: ldloc.0
		IL_0023: ldloc.1
		IL_0024: sub
		IL_0025: stfld float32 AgentTest::coolDown
		IL_002a: ldloc.1
		IL_002b: stloc.2

		IL_002c: nop
		IL_002d: ret
	} // end of method AgentTest::Update

	.method private hidebysig 
		instance void FixedUpdate () cil managed 
	{
		// Method begins at RVA 0xb46c
		// Header size: 12
		// Code size: 513 (0x201)
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
			[9] int64,
			[10] float32,
			[11] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[12] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[13] valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit,
			[14] bool,
			[15] valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit,
			[16] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[17] int32,
			[18] string,
			[19] bool,
			[20] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[21] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[22] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[23] float32,
			[24] int32,
			[25] float32,
			[26] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[27] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[28] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[29] bool,
			[30] bool
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform AgentTest::player
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
		IL_006b: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_0070: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0075: stloc.s 9
		IL_0077: ldloca.s 8
		IL_0079: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_007e: stloc.s 10
		IL_0080: br.s IL_0083

		IL_0082: nop

		IL_0083: nop
		IL_0084: ldloca.s 11
		IL_0086: ldloc.s 9
		IL_0088: box [mscorlib]System.Int64
		IL_008d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0092: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0097: ldloca.s 11
		IL_0099: ldloc.s 10
		IL_009b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00a0: ldloc.s 11
		IL_00a2: ldloca.s 12
		IL_00a4: ldstr "unpcklps xmm6,xmm7"
		IL_00a9: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00ae: box [mscorlib]System.Int64
		IL_00b3: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_00b8: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00bd: ldloca.s 12
		IL_00bf: ldloc.s 5
		IL_00c1: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00c6: ldloc.s 12
		IL_00c8: ldloca.s 13
		IL_00ca: ldc.r4 (00 00 80 7F)
		IL_00cf: ldc.i4.3
		IL_00d0: ldc.i4.1
		IL_00d1: box [mscorlib]System.Int32
		IL_00d6: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.PhysicsModule]UnityEngine.QueryTriggerInteraction>(object)
		IL_00db: call bool [UnityEngine.PhysicsModule]UnityEngine.Physics::Raycast(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit&, float32, int32, valuetype [UnityEngine.PhysicsModule]UnityEngine.QueryTriggerInteraction)
		IL_00e0: stloc.s 14
		IL_00e2: ldloca.s 15
		IL_00e4: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_00e9: stloc.s 16
		IL_00eb: ldloc.s 14
		IL_00ed: stloc.s 17
		IL_00ef: ldloc.s 16
		IL_00f1: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Component::get_tag()
		IL_00f6: stloc.s 18
		IL_00f8: ldloc.s 18
		IL_00fa: ldstr "Player"
		IL_00ff: call bool [mscorlib]System.String::Equals(string, string)
		IL_0104: stloc.s 19
		IL_0106: ldarg.0
		IL_0107: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent AgentTest::agent
		IL_010c: stloc.s 20
		IL_010e: ldloc.s 17
		IL_0110: ldloc.s 19
		IL_0112: and
		IL_0113: ldc.i4.0
		IL_0114: ceq
		IL_0116: ldc.i4.0
		IL_0117: ceq
		IL_0119: brtrue IL_019f

		IL_011e: nop
		IL_011f: ldarg.0
		IL_0120: ldc.i4.0
		IL_0121: stfld bool AgentTest::db
		IL_0126: ldloc.s 20
		IL_0128: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::get_velocity()
		IL_012d: stloc.s 21
		IL_012f: ldloca.s 22
		IL_0131: ldloca.s 21
		IL_0133: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0138: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_013d: ldloca.s 22
		IL_013f: ldloca.s 21
		IL_0141: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0146: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_014b: ldloca.s 22
		IL_014d: ldloca.s 21
		IL_014f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0154: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0159: ldloca.s 22
		IL_015b: call instance float32 [UnityEngine.CoreModule]UnityEngine.Vector3::get_magnitude()
		IL_0160: stloc.s 23
		IL_0162: ldc.r4 1
		IL_0167: ldloc.s 23
		IL_0169: clt
		IL_016b: ldc.i4.0
		IL_016c: ceq
		IL_016e: stloc.s 24
		IL_0170: ldloc.s 23
		IL_0172: stloc.s 25
		IL_0174: ldc.r4 1
		IL_0179: ldloc.s 23
		IL_017b: clt
		IL_017d: ldc.i4.0
		IL_017e: ceq
		IL_0180: ldc.r4 0.0
		IL_0185: ldarg.0
		IL_0186: ldfld float32 AgentTest::coolDown
		IL_018b: clt
		IL_018d: ldc.i4.0
		IL_018e: ceq
		IL_0190: and
		IL_0191: ldc.i4.0
		IL_0192: ceq
		IL_0194: brtrue.s IL_019d

		IL_0196: nop
		IL_0197: ldarg.0
		IL_0198: callvirt instance void AgentTest::Wander()

		IL_019d: nop
		IL_019e: ret

		IL_019f: nop
		IL_01a0: ldarg.0
		IL_01a1: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform AgentTest::player
		IL_01a6: stloc.s 26
		IL_01a8: ldarg.0
		IL_01a9: ldc.i4.1
		IL_01aa: stfld bool AgentTest::db
		IL_01af: ldloc.s 26
		IL_01b1: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_01b6: stloc.s 27
		IL_01b8: ldloc.s 20
		IL_01ba: ldloca.s 28
		IL_01bc: ldloca.s 27
		IL_01be: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_01c3: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_01c8: ldloca.s 28
		IL_01ca: ldloca.s 27
		IL_01cc: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_01d1: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_01d6: ldloca.s 28
		IL_01d8: ldloca.s 27
		IL_01da: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_01df: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_01e4: ldloc.s 28
		IL_01e6: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::SetDestination(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_01eb: stloc.s 29
		IL_01ed: ldarg.0
		IL_01ee: ldc.r4 1
		IL_01f3: stfld float32 AgentTest::coolDown
		IL_01f8: ldloc.s 29
		IL_01fa: stloc.s 30
		IL_01fc: br.s IL_019d

		IL_01fe: nop
		IL_01ff: ldnull
		IL_0200: throw
	} // end of method AgentTest::FixedUpdate

	.method private hidebysig 
		instance void Wander () cil managed 
	{
		// Method begins at RVA 0xb67c
		// Header size: 12
		// Code size: 96 (0x60)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[3] bool
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class AILocationSelectorScript AgentTest::wanderer
		IL_0007: callvirt instance void AILocationSelectorScript::GetNewTarget()
		IL_000c: ldarg.0
		IL_000d: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent AgentTest::agent
		IL_0012: stloc.0
		IL_0013: ldarg.0
		IL_0014: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform AgentTest::wanderTarget
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
		IL_0051: ldarg.0
		IL_0052: ldc.r4 1
		IL_0057: stfld float32 AgentTest::coolDown
		IL_005c: ret

		IL_005d: nop
		IL_005e: ldnull
		IL_005f: throw
	} // end of method AgentTest::Wander

	.method private hidebysig 
		instance void TargetPlayer () cil managed 
	{
		// Method begins at RVA 0xb6e8
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
		IL_0002: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent AgentTest::agent
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform AgentTest::player
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
		IL_0047: ldc.r4 1
		IL_004c: stfld float32 AgentTest::coolDown
		IL_0051: ret

		IL_0052: nop
		IL_0053: ldnull
		IL_0054: throw
	} // end of method AgentTest::TargetPlayer

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x24d6
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method AgentTest::.ctor

} // end of class AgentTest
