.class public auto ansi beforefieldinit PrincipalScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public bool seesRuleBreak
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform player
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform bully
	.field public bool bullySeen
	.field public class PlayerScript playerScript
	.field public class BullyScript bullyScript
	.field public class BaldiScript baldiScript
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform wanderTarget
	.field public class AILocationSelectorScript wanderer
	.field public class DoorScript officeDoor
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform principalDetentionPoint
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform playerDetentionPoint
	.field public float32 coolDown
	.field public float32 timeSeenRuleBreak
	.field public bool angry
	.field public bool inOffice
	.field private int32 detentions
	.field private int32[] lockTime
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip[] audTimes
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip[] audScolds
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip audDetention
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip audNoDrinking
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip audNoEating
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip audNoBullying
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip audNoFaculty
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip audNoLockers
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip audNoRunning
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip audNoStabbing
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip audNoEscaping
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_Whistle
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_Delay
	.field private class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent agent
	.field private class AudioQueueScript audioQueue
	.field private class [UnityEngine.AudioModule]UnityEngine.AudioSource audioDevice
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioSource quietAudioDevice
	.field private valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit hit
	.field private valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 aim
	.field public class [UnityEngine.PhysicsModule]UnityEngine.CharacterController cc

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0xe760
		// Header size: 12
		// Code size: 44 (0x2c)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[1] class AudioQueueScript,
			[2] class [UnityEngine.AudioModule]UnityEngine.AudioSource
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent>()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PrincipalScript::agent
		IL_000f: ldarg.0
		IL_0010: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class AudioQueueScript>()
		IL_0015: stloc.1
		IL_0016: ldarg.0
		IL_0017: ldloc.1
		IL_0018: stfld class AudioQueueScript PrincipalScript::audioQueue
		IL_001d: ldarg.0
		IL_001e: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.AudioModule]UnityEngine.AudioSource>()
		IL_0023: stloc.2
		IL_0024: ldarg.0
		IL_0025: ldloc.2
		IL_0026: stfld class [UnityEngine.AudioModule]UnityEngine.AudioSource PrincipalScript::audioDevice
		IL_002b: ret
	} // end of method PrincipalScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0xe798
		// Header size: 12
		// Code size: 183 (0xb7)
		.maxstack 3
		.locals init (
			[0] float32,
			[1] float32,
			[2] int32,
			[3] float32,
			[4] float32,
			[5] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld bool PrincipalScript::seesRuleBreak
		IL_0007: ldc.i4.0
		IL_0008: ceq
		IL_000a: ldc.i4.0
		IL_000b: ceq
		IL_000d: brtrue.s IL_001d

		IL_000f: nop
		IL_0010: ldarg.0
		IL_0011: ldc.r4 0.0
		IL_0016: stfld float32 PrincipalScript::timeSeenRuleBreak
		IL_001b: br.s IL_0084

		IL_001d: nop
		IL_001e: ldarg.0
		IL_001f: ldfld float32 PrincipalScript::timeSeenRuleBreak
		IL_0024: stloc.0
		IL_0025: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_002a: stloc.1
		IL_002b: ldarg.0
		IL_002c: ldloc.1
		IL_002d: ldloc.0
		IL_002e: add
		IL_002f: stfld float32 PrincipalScript::timeSeenRuleBreak
		IL_0034: ldarg.0
		IL_0035: ldfld bool PrincipalScript::angry
		IL_003a: ldc.i4.0
		IL_003b: ceq
		IL_003d: stloc.2
		IL_003e: ldloc.1
		IL_003f: ldloc.0
		IL_0040: add
		IL_0041: stloc.3
		IL_0042: ldarg.0
		IL_0043: ldfld bool PrincipalScript::angry
		IL_0048: ldc.i4.0
		IL_0049: ceq
		IL_004b: ldloc.1
		IL_004c: ldloc.0
		IL_004d: add
		IL_004e: ldc.r4 0.5
		IL_0053: clt
		IL_0055: ldc.i4.0
		IL_0056: ceq
		IL_0058: and
		IL_0059: ldc.i4.0
		IL_005a: ceq
		IL_005c: brtrue.s IL_0084

		IL_005e: nop
		IL_005f: ldarg.0
		IL_0060: ldc.i4.1
		IL_0061: stfld bool PrincipalScript::angry
		IL_0066: ldarg.0
		IL_0067: ldc.i4.0
		IL_0068: stfld bool PrincipalScript::seesRuleBreak
		IL_006d: ldarg.0
		IL_006e: ldc.r4 0.0
		IL_0073: stfld float32 PrincipalScript::timeSeenRuleBreak
		IL_0078: ldarg.0
		IL_0079: callvirt instance void PrincipalScript::TargetPlayer()
		IL_007e: ldarg.0
		IL_007f: callvirt instance void PrincipalScript::CorrectPlayer()

		IL_0084: nop
		IL_0085: ldarg.0
		IL_0086: ldfld float32 PrincipalScript::coolDown
		IL_008b: stloc.s 4
		IL_008d: ldarg.0
		IL_008e: ldfld float32 PrincipalScript::coolDown
		IL_0093: ldc.r4 0.0
		IL_0098: cgt
		IL_009a: ldc.i4.0
		IL_009b: ceq
		IL_009d: brtrue.s IL_00b5

		IL_009f: nop
		IL_00a0: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_00a5: stloc.s 5
		IL_00a7: ldarg.0
		IL_00a8: ldloc.s 4
		IL_00aa: ldloc.s 5
		IL_00ac: sub
		IL_00ad: stfld float32 PrincipalScript::coolDown
		IL_00b2: ldloc.s 5
		IL_00b4: stloc.3

		IL_00b5: nop
		IL_00b6: ret
	} // end of method PrincipalScript::Update

	.method private hidebysig 
		instance void FixedUpdate () cil managed 
	{
		// Method begins at RVA 0xe85c
		// Header size: 12
		// Code size: 1328 (0x530)
		.maxstack 6
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[2] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[3] float32,
			[4] float32,
			[5] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[6] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[7] int64,
			[8] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[9] int64,
			[10] float32,
			[11] float32,
			[12] bool,
			[13] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[14] string,
			[15] bool,
			[16] int32,
			[17] float32,
			[18] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[19] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[20] float32,
			[21] class AILocationSelectorScript,
			[22] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[23] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[24] bool,
			[25] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[26] bool,
			[27] float32,
			[28] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[29] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[30] float32,
			[31] float32,
			[32] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[33] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[34] int64,
			[35] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[36] int64,
			[37] bool,
			[38] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[39] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[40] string,
			[41] bool,
			[42] class BullyScript,
			[43] int32,
			[44] int32,
			[45] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[46] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[47] bool,
			[48] bool,
			[49] int64,
			[50] class AudioQueueScript,
			[51] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[52] class [UnityEngine.AudioModule]UnityEngine.AudioClip
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld bool PrincipalScript::angry
		IL_0007: ldc.i4.0
		IL_0008: ceq
		IL_000a: brtrue.s IL_0014

		IL_000c: nop
		IL_000d: ldarg.0
		IL_000e: callvirt instance void PrincipalScript::TargetPlayer()
		IL_0013: ret

		IL_0014: nop
		IL_0015: ldarg.0
		IL_0016: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PrincipalScript::player
		IL_001b: stloc.0
		IL_001c: ldloc.0
		IL_001d: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0022: stloc.1
		IL_0023: ldarg.0
		IL_0024: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0029: stloc.2
		IL_002a: ldloca.s 1
		IL_002c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0031: stloc.3
		IL_0032: ldloca.s 1
		IL_0034: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0039: stloc.s 4
		IL_003b: ldloc.2
		IL_003c: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0041: stloc.s 5
		IL_0043: ldarg.0
		IL_0044: ldstr "unpcklps xmm6,xmm7"
		IL_0049: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_004e: box [mscorlib]System.Int64
		IL_0053: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0058: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PrincipalScript::aim
		IL_005d: ldarg.0
		IL_005e: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PrincipalScript::aim
		IL_0063: ldloca.s 1
		IL_0065: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_006a: ldloca.s 5
		IL_006c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0071: sub
		IL_0072: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0077: ldarg.0
		IL_0078: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_007d: stloc.s 6
		IL_007f: ldloc.3
		IL_0080: ldloca.s 5
		IL_0082: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0087: sub
		IL_0088: stloc.3
		IL_0089: ldstr "unpcklps xmm6,xmm7"
		IL_008e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0093: stloc.s 7
		IL_0095: ldloc.s 6
		IL_0097: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_009c: stloc.s 8
		IL_009e: ldstr "struct_chunk(FieldExpr+0x0, 8)"
		IL_00a3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00a8: stloc.s 9
		IL_00aa: ldarg.0
		IL_00ab: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PrincipalScript::aim
		IL_00b0: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00b5: stloc.s 10
		IL_00b7: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_00bc: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00c1: stloc.s 7
		IL_00c3: ldloca.s 8
		IL_00c5: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00ca: stloc.s 11
		IL_00cc: br.s IL_00cf

		IL_00ce: nop

		IL_00cf: nop
		IL_00d0: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_00d5: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00da: conv.i8
		IL_00db: ldc.i8 7
		IL_00e4: add
		IL_00e5: box [mscorlib]System.Int64
		IL_00ea: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_00ef: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_00f4: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00f9: conv.i8
		IL_00fa: ldc.i8 -9
		IL_0103: add
		IL_0104: box [mscorlib]System.Int64
		IL_0109: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_010e: ldarg.0
		IL_010f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0114: ldc.i8 288
		IL_011d: add
		IL_011e: ldc.r4 (00 00 80 7F)
		IL_0123: ldc.i4 769
		IL_0128: ldc.i4.1
		IL_0129: box [mscorlib]System.Int32
		IL_012e: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.PhysicsModule]UnityEngine.QueryTriggerInteraction>(object)
		IL_0133: call bool [UnityEngine.PhysicsModule]UnityEngine.Physics::Raycast(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit&, float32, int32, valuetype [UnityEngine.PhysicsModule]UnityEngine.QueryTriggerInteraction)
		IL_0138: stloc.s 12
		IL_013a: ldarg.0
		IL_013b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0140: ldc.i8 288
		IL_0149: add
		IL_014a: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_014f: stloc.s 13
		IL_0151: ldloc.s 13
		IL_0153: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Component::get_tag()
		IL_0158: stloc.s 14
		IL_015a: ldloc.s 14
		IL_015c: ldstr "Player"
		IL_0161: call bool [mscorlib]System.String::Equals(string, string)
		IL_0166: stloc.s 15
		IL_0168: ldarg.0
		IL_0169: ldfld bool PrincipalScript::angry
		IL_016e: ldc.i4.0
		IL_016f: ceq
		IL_0171: brfalse.s IL_0193

		IL_0173: ldarg.0
		IL_0174: ldfld class PlayerScript PrincipalScript::playerScript
		IL_0179: ldfld float32 PlayerScript::guilt
		IL_017e: ldc.r4 0.0
		IL_0183: cgt
		IL_0185: ldarg.0
		IL_0186: ldfld bool PrincipalScript::inOffice
		IL_018b: ldc.i4.0
		IL_018c: ceq
		IL_018e: and
		IL_018f: stloc.s 16
		IL_0191: br.s IL_0197

		IL_0193: nop
		IL_0194: ldc.i4.0
		IL_0195: stloc.s 16

		IL_0197: nop
		IL_0198: ldarg.0
		IL_0199: ldfld class PlayerScript PrincipalScript::playerScript
		IL_019e: ldfld float32 PlayerScript::guilt
		IL_01a3: stloc.s 17
		IL_01a5: ldloc.s 15
		IL_01a7: ldloc.s 12
		IL_01a9: and
		IL_01aa: ldloc.s 16
		IL_01ac: and
		IL_01ad: ldc.i4.0
		IL_01ae: ceq
		IL_01b0: ldc.i4.0
		IL_01b1: ceq
		IL_01b3: brtrue IL_02da

		IL_01b8: nop
		IL_01b9: ldarg.0
		IL_01ba: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PrincipalScript::agent
		IL_01bf: stloc.s 18
		IL_01c1: ldarg.0
		IL_01c2: ldc.i4.0
		IL_01c3: stfld bool PrincipalScript::seesRuleBreak
		IL_01c8: ldloc.s 18
		IL_01ca: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::get_velocity()
		IL_01cf: stloc.s 19
		IL_01d1: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_01d6: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01db: conv.i8
		IL_01dc: ldc.i8 7
		IL_01e5: add
		IL_01e6: call instance float32 [UnityEngine.CoreModule]UnityEngine.Vector3::get_magnitude()
		IL_01eb: stloc.s 20
		IL_01ed: ldloc.s 20
		IL_01ef: stloc.s 17
		IL_01f1: ldc.r4 1
		IL_01f6: ldloc.s 20
		IL_01f8: clt
		IL_01fa: ldc.i4.0
		IL_01fb: ceq
		IL_01fd: ldc.r4 0.0
		IL_0202: ldarg.0
		IL_0203: ldfld float32 PrincipalScript::coolDown
		IL_0208: clt
		IL_020a: ldc.i4.0
		IL_020b: ceq
		IL_020d: and
		IL_020e: ldc.i4.0
		IL_020f: ceq
		IL_0211: brtrue IL_02e2

		IL_0216: nop
		IL_0217: ldarg.0
		IL_0218: ldfld class PlayerScript PrincipalScript::playerScript
		IL_021d: ldc.i4.1
		IL_021e: stfld int32 PlayerScript::principalBugFixer
		IL_0223: ldarg.0
		IL_0224: ldfld class AILocationSelectorScript PrincipalScript::wanderer
		IL_0229: stloc.s 21
		IL_022b: ldloc.s 21
		IL_022d: callvirt instance void AILocationSelectorScript::GetNewTarget()
		IL_0232: ldarg.0
		IL_0233: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PrincipalScript::wanderTarget
		IL_0238: stloc.0
		IL_0239: ldarg.0
		IL_023a: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PrincipalScript::agent
		IL_023f: stloc.s 22
		IL_0241: ldloc.0
		IL_0242: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0247: stloc.s 23
		IL_0249: ldloc.s 22
		IL_024b: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0250: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0255: conv.i8
		IL_0256: ldc.i8 7
		IL_025f: add
		IL_0260: box [mscorlib]System.Int64
		IL_0265: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_026a: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::SetDestination(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_026f: stloc.s 24
		IL_0271: ldarg.0
		IL_0272: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PrincipalScript::agent
		IL_0277: stloc.s 25
		IL_0279: ldloc.s 25
		IL_027b: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::get_isStopped()
		IL_0280: stloc.s 26
		IL_0282: ldloc.s 26
		IL_0284: ldc.i4.0
		IL_0285: ceq
		IL_0287: brtrue.s IL_029a

		IL_0289: nop
		IL_028a: ldarg.0
		IL_028b: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PrincipalScript::agent
		IL_0290: stloc.s 25
		IL_0292: ldloc.s 25
		IL_0294: ldc.i4.0
		IL_0295: callvirt instance void [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::set_isStopped(bool)

		IL_029a: nop
		IL_029b: ldarg.0
		IL_029c: ldc.r4 1
		IL_02a1: stfld float32 PrincipalScript::coolDown
		IL_02a6: ldc.r4 0.0
		IL_02ab: ldc.r4 10
		IL_02b0: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_02b5: stloc.s 27
		IL_02b7: ldloc.s 27
		IL_02b9: stloc.s 17
		IL_02bb: ldc.r4 1
		IL_02c0: ldloc.s 27
		IL_02c2: clt
		IL_02c4: brtrue.s IL_02e2

		IL_02c6: nop
		IL_02c7: ldarg.0
		IL_02c8: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource PrincipalScript::quietAudioDevice
		IL_02cd: ldarg.0
		IL_02ce: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip PrincipalScript::aud_Whistle
		IL_02d3: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_02d8: br.s IL_02e2

		IL_02da: nop
		IL_02db: ldarg.0
		IL_02dc: ldc.i4.1
		IL_02dd: stfld bool PrincipalScript::seesRuleBreak

		IL_02e2: nop
		IL_02e3: ldarg.0
		IL_02e4: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PrincipalScript::bully
		IL_02e9: stloc.0
		IL_02ea: ldloc.0
		IL_02eb: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_02f0: stloc.s 28
		IL_02f2: ldarg.0
		IL_02f3: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_02f8: stloc.s 29
		IL_02fa: ldloca.s 28
		IL_02fc: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0301: stloc.s 30
		IL_0303: ldloca.s 28
		IL_0305: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_030a: stloc.s 31
		IL_030c: ldloc.s 29
		IL_030e: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0313: stloc.s 32
		IL_0315: ldarg.0
		IL_0316: ldstr "unpcklps xmm7,xmm8"
		IL_031b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0320: box [mscorlib]System.Int64
		IL_0325: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_032a: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PrincipalScript::aim
		IL_032f: ldarg.0
		IL_0330: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PrincipalScript::aim
		IL_0335: ldloca.s 28
		IL_0337: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_033c: ldloca.s 32
		IL_033e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0343: sub
		IL_0344: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0349: ldarg.0
		IL_034a: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_034f: stloc.s 33
		IL_0351: ldloc.s 30
		IL_0353: ldloca.s 32
		IL_0355: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_035a: sub
		IL_035b: stloc.s 30
		IL_035d: ldstr "unpcklps xmm7,xmm8"
		IL_0362: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0367: stloc.s 34
		IL_0369: ldloc.s 33
		IL_036b: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0370: stloc.s 35
		IL_0372: ldstr "struct_chunk(FieldExpr+0x0, 8)"
		IL_0377: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_037c: stloc.s 36
		IL_037e: ldarg.0
		IL_037f: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PrincipalScript::aim
		IL_0384: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0389: stloc.s 10
		IL_038b: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_0390: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0395: stloc.s 34
		IL_0397: ldloca.s 35
		IL_0399: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_039e: stloc.s 11
		IL_03a0: br.s IL_03a3

		IL_03a2: nop

		IL_03a3: nop
		IL_03a4: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_03a9: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_03ae: conv.i8
		IL_03af: ldc.i8 -9
		IL_03b8: add
		IL_03b9: box [mscorlib]System.Int64
		IL_03be: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_03c3: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_03c8: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_03cd: conv.i8
		IL_03ce: ldc.i8 7
		IL_03d7: add
		IL_03d8: box [mscorlib]System.Int64
		IL_03dd: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_03e2: ldarg.0
		IL_03e3: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_03e8: ldc.i8 288
		IL_03f1: add
		IL_03f2: ldc.r4 (00 00 80 7F)
		IL_03f7: ldc.i4 769
		IL_03fc: call bool [UnityEngine.PhysicsModule]UnityEngine.Physics::Raycast(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit&, float32, int32)
		IL_0401: stloc.s 37
		IL_0403: ldarg.0
		IL_0404: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0409: ldc.i8 288
		IL_0412: add
		IL_0413: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_0418: stloc.s 38
		IL_041a: ldloc.s 38
		IL_041c: stloc.s 39
		IL_041e: ldloc.s 39
		IL_0420: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_0425: stloc.s 40
		IL_0427: ldloc.s 40
		IL_0429: ldstr "Its a Bully"
		IL_042e: call bool [mscorlib]System.String::Equals(string, string)
		IL_0433: stloc.s 41
		IL_0435: ldarg.0
		IL_0436: ldfld class BullyScript PrincipalScript::bullyScript
		IL_043b: stloc.s 42
		IL_043d: ldarg.0
		IL_043e: ldfld bool PrincipalScript::angry
		IL_0443: ldc.i4.0
		IL_0444: ceq
		IL_0446: brfalse.s IL_0464

		IL_0448: ldloc.s 42
		IL_044a: ldfld float32 BullyScript::guilt
		IL_044f: ldc.r4 0.0
		IL_0454: cgt
		IL_0456: ldarg.0
		IL_0457: ldfld bool PrincipalScript::inOffice
		IL_045c: ldc.i4.0
		IL_045d: ceq
		IL_045f: and
		IL_0460: stloc.s 43
		IL_0462: br.s IL_0468

		IL_0464: nop
		IL_0465: ldc.i4.0
		IL_0466: stloc.s 43

		IL_0468: nop
		IL_0469: ldloc.s 43
		IL_046b: stloc.s 44
		IL_046d: ldloc.s 42
		IL_046f: ldfld float32 BullyScript::guilt
		IL_0474: stloc.s 17
		IL_0476: ldloc.s 41
		IL_0478: ldloc.s 37
		IL_047a: and
		IL_047b: ldloc.s 43
		IL_047d: and
		IL_047e: ldc.i4.0
		IL_047f: ceq
		IL_0481: brtrue IL_0528

		IL_0486: nop
		IL_0487: ldarg.0
		IL_0488: ldfld bool PrincipalScript::bullySeen
		IL_048d: ldc.i4.0
		IL_048e: ceq
		IL_0490: ldc.i4.0
		IL_0491: ceq
		IL_0493: brtrue IL_0528

		IL_0498: nop
		IL_0499: ldarg.0
		IL_049a: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PrincipalScript::agent
		IL_049f: stloc.s 45
		IL_04a1: ldarg.0
		IL_04a2: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PrincipalScript::bully
		IL_04a7: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_04ac: stloc.s 46
		IL_04ae: ldloc.s 45
		IL_04b0: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_04b5: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_04ba: conv.i8
		IL_04bb: ldc.i8 7
		IL_04c4: add
		IL_04c5: box [mscorlib]System.Int64
		IL_04ca: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_04cf: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::SetDestination(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_04d4: stloc.s 47
		IL_04d6: ldloc.s 47
		IL_04d8: stloc.s 48
		IL_04da: ldarg.0
		IL_04db: ldfld class AudioQueueScript PrincipalScript::audioQueue
		IL_04e0: ldfld int32 AudioQueueScript::audioInQueue
		IL_04e5: conv.i8
		IL_04e6: stloc.s 49
		IL_04e8: ldarg.0
		IL_04e9: ldfld class AudioQueueScript PrincipalScript::audioQueue
		IL_04ee: stloc.s 50
		IL_04f0: ldarg.0
		IL_04f1: ldfld class AudioQueueScript PrincipalScript::audioQueue
		IL_04f6: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] AudioQueueScript::audioQueue
		IL_04fb: stloc.s 51
		IL_04fd: ldarg.0
		IL_04fe: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip PrincipalScript::audNoBullying
		IL_0503: stloc.s 52
		IL_0505: ldloc.s 51
		IL_0507: ldloc.s 49
		IL_0509: conv.i4
		IL_050a: ldloc.s 52
		IL_050c: stelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0511: ldloc.s 50
		IL_0513: ldloc.s 50
		IL_0515: ldfld int32 AudioQueueScript::audioInQueue
		IL_051a: ldc.i4.1
		IL_051b: add
		IL_051c: stfld int32 AudioQueueScript::audioInQueue
		IL_0521: ldarg.0
		IL_0522: ldc.i4.1
		IL_0523: stfld bool PrincipalScript::bullySeen

		IL_0528: nop
		IL_0529: ret

		IL_052a: nop
		IL_052b: ldnull
		IL_052c: throw

		IL_052d: nop
		IL_052e: ldnull
		IL_052f: throw
	} // end of method PrincipalScript::FixedUpdate

	.method private hidebysig 
		instance void Wander () cil managed 
	{
		// Method begins at RVA 0xed98
		// Header size: 12
		// Code size: 207 (0xcf)
		.maxstack 3
		.locals init (
			[0] class AILocationSelectorScript,
			[1] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[2] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[4] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[5] bool,
			[6] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[7] bool,
			[8] float32,
			[9] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class PlayerScript PrincipalScript::playerScript
		IL_0007: ldc.i4.1
		IL_0008: stfld int32 PlayerScript::principalBugFixer
		IL_000d: ldarg.0
		IL_000e: ldfld class AILocationSelectorScript PrincipalScript::wanderer
		IL_0013: stloc.0
		IL_0014: ldloc.0
		IL_0015: callvirt instance void AILocationSelectorScript::GetNewTarget()
		IL_001a: ldarg.0
		IL_001b: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PrincipalScript::wanderTarget
		IL_0020: stloc.1
		IL_0021: ldarg.0
		IL_0022: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PrincipalScript::agent
		IL_0027: stloc.2
		IL_0028: ldloc.1
		IL_0029: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_002e: stloc.3
		IL_002f: ldloc.2
		IL_0030: ldloca.s 4
		IL_0032: ldloca.s 3
		IL_0034: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0039: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_003e: ldloca.s 4
		IL_0040: ldloca.s 3
		IL_0042: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0047: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_004c: ldloca.s 4
		IL_004e: ldloca.s 3
		IL_0050: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0055: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_005a: ldloc.s 4
		IL_005c: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::SetDestination(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0061: stloc.s 5
		IL_0063: ldarg.0
		IL_0064: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PrincipalScript::agent
		IL_0069: stloc.s 6
		IL_006b: ldloc.s 6
		IL_006d: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::get_isStopped()
		IL_0072: stloc.s 7
		IL_0074: ldloc.s 7
		IL_0076: ldc.i4.0
		IL_0077: ceq
		IL_0079: brtrue.s IL_008c

		IL_007b: nop
		IL_007c: ldarg.0
		IL_007d: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PrincipalScript::agent
		IL_0082: stloc.s 6
		IL_0084: ldloc.s 6
		IL_0086: ldc.i4.0
		IL_0087: callvirt instance void [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::set_isStopped(bool)

		IL_008c: nop
		IL_008d: ldarg.0
		IL_008e: ldc.r4 1
		IL_0093: stfld float32 PrincipalScript::coolDown
		IL_0098: ldc.r4 0.0
		IL_009d: ldc.r4 10
		IL_00a2: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_00a7: stloc.s 8
		IL_00a9: ldloc.s 8
		IL_00ab: stloc.s 9
		IL_00ad: ldc.r4 1
		IL_00b2: ldloc.s 8
		IL_00b4: clt
		IL_00b6: brtrue.s IL_00ca

		IL_00b8: nop
		IL_00b9: ldarg.0
		IL_00ba: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource PrincipalScript::quietAudioDevice
		IL_00bf: ldarg.0
		IL_00c0: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip PrincipalScript::aud_Whistle
		IL_00c5: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)

		IL_00ca: nop
		IL_00cb: ret

		IL_00cc: nop
		IL_00cd: ldnull
		IL_00ce: throw
	} // end of method PrincipalScript::Wander

	.method private hidebysig 
		instance void TargetPlayer () cil managed 
	{
		// Method begins at RVA 0xee74
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
		IL_0002: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PrincipalScript::agent
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PrincipalScript::player
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
		IL_004c: stfld float32 PrincipalScript::coolDown
		IL_0051: ret

		IL_0052: nop
		IL_0053: ldnull
		IL_0054: throw
	} // end of method PrincipalScript::TargetPlayer

	.method private hidebysig 
		instance void TargetBully () cil managed 
	{
		// Method begins at RVA 0xeed8
		// Header size: 12
		// Code size: 177 (0xb1)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[3] bool,
			[4] bool,
			[5] int64,
			[6] class AudioQueueScript,
			[7] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[8] class [UnityEngine.AudioModule]UnityEngine.AudioClip
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld bool PrincipalScript::bullySeen
		IL_0007: ldc.i4.0
		IL_0008: ceq
		IL_000a: ldc.i4.0
		IL_000b: ceq
		IL_000d: brtrue IL_00a9

		IL_0012: nop
		IL_0013: ldarg.0
		IL_0014: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PrincipalScript::agent
		IL_0019: stloc.0
		IL_001a: ldarg.0
		IL_001b: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PrincipalScript::bully
		IL_0020: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0025: stloc.1
		IL_0026: ldloc.0
		IL_0027: ldloca.s 2
		IL_0029: ldloca.s 1
		IL_002b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0030: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0035: ldloca.s 2
		IL_0037: ldloca.s 1
		IL_0039: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_003e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0043: ldloca.s 2
		IL_0045: ldloca.s 1
		IL_0047: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_004c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0051: ldloc.2
		IL_0052: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::SetDestination(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0057: stloc.3
		IL_0058: ldloc.3
		IL_0059: stloc.s 4
		IL_005b: ldarg.0
		IL_005c: ldfld class AudioQueueScript PrincipalScript::audioQueue
		IL_0061: ldfld int32 AudioQueueScript::audioInQueue
		IL_0066: conv.i8
		IL_0067: stloc.s 5
		IL_0069: ldarg.0
		IL_006a: ldfld class AudioQueueScript PrincipalScript::audioQueue
		IL_006f: stloc.s 6
		IL_0071: ldarg.0
		IL_0072: ldfld class AudioQueueScript PrincipalScript::audioQueue
		IL_0077: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] AudioQueueScript::audioQueue
		IL_007c: stloc.s 7
		IL_007e: ldarg.0
		IL_007f: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip PrincipalScript::audNoBullying
		IL_0084: stloc.s 8
		IL_0086: ldloc.s 7
		IL_0088: ldloc.s 5
		IL_008a: conv.i4
		IL_008b: ldloc.s 8
		IL_008d: stelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0092: ldloc.s 6
		IL_0094: ldloc.s 6
		IL_0096: ldfld int32 AudioQueueScript::audioInQueue
		IL_009b: ldc.i4.1
		IL_009c: add
		IL_009d: stfld int32 AudioQueueScript::audioInQueue
		IL_00a2: ldarg.0
		IL_00a3: ldc.i4.1
		IL_00a4: stfld bool PrincipalScript::bullySeen

		IL_00a9: nop
		IL_00aa: ret

		IL_00ab: nop
		IL_00ac: ldnull
		IL_00ad: throw

		IL_00ae: nop
		IL_00af: ldnull
		IL_00b0: throw
	} // end of method PrincipalScript::TargetBully

	.method private hidebysig 
		instance void CorrectPlayer () cil managed 
	{
		// Method begins at RVA 0xef98
		// Header size: 12
		// Code size: 311 (0x137)
		.maxstack 3
		.locals init (
			[0] bool,
			[1] bool,
			[2] bool,
			[3] bool,
			[4] bool,
			[5] class AudioQueueScript,
			[6] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[7] int64,
			[8] class [UnityEngine.AudioModule]UnityEngine.AudioClip
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class AudioQueueScript PrincipalScript::audioQueue
		IL_0007: ldc.i4.0
		IL_0008: stfld int32 AudioQueueScript::audioInQueue
		IL_000d: ldarg.0
		IL_000e: ldfld class PlayerScript PrincipalScript::playerScript
		IL_0013: ldfld string PlayerScript::guiltType
		IL_0018: ldstr "faculty"
		IL_001d: call bool [mscorlib]System.String::Equals(string, string)
		IL_0022: stloc.0
		IL_0023: ldloc.0
		IL_0024: ldc.i4.0
		IL_0025: ceq
		IL_0027: ldc.i4.0
		IL_0028: ceq
		IL_002a: brtrue IL_00ef

		IL_002f: nop
		IL_0030: ldarg.0
		IL_0031: ldfld class PlayerScript PrincipalScript::playerScript
		IL_0036: ldfld string PlayerScript::guiltType
		IL_003b: ldstr "running"
		IL_0040: call bool [mscorlib]System.String::Equals(string, string)
		IL_0045: stloc.1
		IL_0046: ldloc.1
		IL_0047: ldc.i4.0
		IL_0048: ceq
		IL_004a: ldc.i4.0
		IL_004b: ceq
		IL_004d: brtrue.s IL_00c9

		IL_004f: nop
		IL_0050: ldarg.0
		IL_0051: ldfld class PlayerScript PrincipalScript::playerScript
		IL_0056: ldfld string PlayerScript::guiltType
		IL_005b: ldstr "drink"
		IL_0060: call bool [mscorlib]System.String::Equals(string, string)
		IL_0065: stloc.2
		IL_0066: ldloc.2
		IL_0067: ldc.i4.0
		IL_0068: ceq
		IL_006a: ldc.i4.0
		IL_006b: ceq
		IL_006d: brtrue.s IL_00a3

		IL_006f: nop
		IL_0070: ldarg.0
		IL_0071: ldfld class PlayerScript PrincipalScript::playerScript
		IL_0076: ldfld string PlayerScript::guiltType
		IL_007b: ldstr "escape"
		IL_0080: call bool [mscorlib]System.String::Equals(string, string)
		IL_0085: stloc.3
		IL_0086: ldloc.3
		IL_0087: stloc.s 4
		IL_0089: ldloc.3
		IL_008a: ldc.i4.0
		IL_008b: ceq
		IL_008d: brtrue.s IL_00a1

		IL_008f: nop
		IL_0090: ldarg.0
		IL_0091: ldfld class AudioQueueScript PrincipalScript::audioQueue
		IL_0096: ldarg.0
		IL_0097: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip PrincipalScript::audNoEscaping
		IL_009c: callvirt instance void AudioQueueScript::QueueAudio(class [UnityEngine.AudioModule]UnityEngine.AudioClip)

		IL_00a1: nop
		IL_00a2: ret

		IL_00a3: nop
		IL_00a4: ldarg.0
		IL_00a5: ldfld class AudioQueueScript PrincipalScript::audioQueue
		IL_00aa: stloc.s 5
		IL_00ac: ldloc.s 5
		IL_00ae: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] AudioQueueScript::audioQueue
		IL_00b3: stloc.s 6
		IL_00b5: ldloc.s 5
		IL_00b7: ldfld int32 AudioQueueScript::audioInQueue
		IL_00bc: conv.i8
		IL_00bd: stloc.s 7
		IL_00bf: ldarg.0
		IL_00c0: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip PrincipalScript::audNoDrinking
		IL_00c5: stloc.s 8
		IL_00c7: br.s IL_0113

		IL_00c9: nop
		IL_00ca: ldarg.0
		IL_00cb: ldfld class AudioQueueScript PrincipalScript::audioQueue
		IL_00d0: stloc.s 5
		IL_00d2: ldloc.s 5
		IL_00d4: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] AudioQueueScript::audioQueue
		IL_00d9: stloc.s 6
		IL_00db: ldloc.s 5
		IL_00dd: ldfld int32 AudioQueueScript::audioInQueue
		IL_00e2: conv.i8
		IL_00e3: stloc.s 7
		IL_00e5: ldarg.0
		IL_00e6: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip PrincipalScript::audNoRunning
		IL_00eb: stloc.s 8
		IL_00ed: br.s IL_0113

		IL_00ef: nop
		IL_00f0: ldarg.0
		IL_00f1: ldfld class AudioQueueScript PrincipalScript::audioQueue
		IL_00f6: stloc.s 5
		IL_00f8: ldloc.s 5
		IL_00fa: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] AudioQueueScript::audioQueue
		IL_00ff: stloc.s 6
		IL_0101: ldloc.s 5
		IL_0103: ldfld int32 AudioQueueScript::audioInQueue
		IL_0108: conv.i8
		IL_0109: stloc.s 7
		IL_010b: ldarg.0
		IL_010c: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip PrincipalScript::audNoFaculty
		IL_0111: stloc.s 8

		IL_0113: nop
		IL_0114: ldloc.s 6
		IL_0116: ldloc.s 7
		IL_0118: conv.i4
		IL_0119: ldloc.s 8
		IL_011b: stelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0120: ldloc.s 5
		IL_0122: ldloc.s 5
		IL_0124: ldfld int32 AudioQueueScript::audioInQueue
		IL_0129: ldc.i4.1
		IL_012a: add
		IL_012b: stfld int32 AudioQueueScript::audioInQueue
		IL_0130: ret

		IL_0131: nop
		IL_0132: ldnull
		IL_0133: throw

		IL_0134: nop
		IL_0135: ldnull
		IL_0136: throw
	} // end of method PrincipalScript::CorrectPlayer

	.method private hidebysig 
		instance void OnTriggerStay (
			class [UnityEngine.PhysicsModule]UnityEngine.Collider other
		) cil managed 
	{
		// Method begins at RVA 0xf0dc
		// Header size: 12
		// Code size: 875 (0x36b)
		.maxstack 3
		.locals init (
			[0] string,
			[1] bool,
			[2] string,
			[3] bool,
			[4] bool,
			[5] class PlayerScript,
			[6] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[7] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[8] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[9] bool,
			[10] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[11] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[12] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[13] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[14] int64,
			[15] class AudioQueueScript,
			[16] class [UnityEngine.AudioModule]UnityEngine.AudioClip,
			[17] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[18] class AudioQueueScript,
			[19] class [UnityEngine.AudioModule]UnityEngine.AudioClip,
			[20] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[21] class AudioQueueScript,
			[22] class [UnityEngine.AudioModule]UnityEngine.AudioClip,
			[23] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[24] float32,
			[25] int32,
			[26] class AudioQueueScript,
			[27] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[28] class [UnityEngine.AudioModule]UnityEngine.AudioClip,
			[29] int32,
			[30] class DoorScript,
			[31] float32,
			[32] bool,
			[33] class BaldiScript,
			[34] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[35] class BaldiScript,
			[36] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[37] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[38] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[39] bool,
			[40] int32,
			[41] int32
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: ldstr "Office Trigger"
		IL_000e: call bool [mscorlib]System.String::Equals(string, string)
		IL_0013: stloc.1
		IL_0014: ldloc.1
		IL_0015: ldc.i4.0
		IL_0016: ceq
		IL_0018: brtrue.s IL_0022

		IL_001a: nop
		IL_001b: ldarg.0
		IL_001c: ldc.i4.1
		IL_001d: stfld bool PrincipalScript::inOffice

		IL_0022: nop
		IL_0023: ldarg.1
		IL_0024: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Component::get_tag()
		IL_0029: stloc.2
		IL_002a: ldloc.2
		IL_002b: ldstr "Player"
		IL_0030: call bool [mscorlib]System.String::Equals(string, string)
		IL_0035: stloc.3
		IL_0036: ldloc.3
		IL_0037: stloc.s 4
		IL_0039: ldarg.0
		IL_003a: ldfld bool PrincipalScript::angry
		IL_003f: ldloc.3
		IL_0040: and
		IL_0041: ldc.i4.0
		IL_0042: ceq
		IL_0044: brtrue IL_0363

		IL_0049: nop
		IL_004a: ldarg.0
		IL_004b: ldfld class PlayerScript PrincipalScript::playerScript
		IL_0050: stloc.s 5
		IL_0052: ldarg.0
		IL_0053: ldc.i4.1
		IL_0054: stfld bool PrincipalScript::inOffice
		IL_0059: ldloc.s 5
		IL_005b: ldc.i4.0
		IL_005c: stfld int32 PlayerScript::principalBugFixer
		IL_0061: ldarg.0
		IL_0062: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PrincipalScript::agent
		IL_0067: stloc.s 6
		IL_0069: ldarg.0
		IL_006a: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PrincipalScript::principalDetentionPoint
		IL_006f: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0074: stloc.s 7
		IL_0076: ldloc.s 6
		IL_0078: ldloca.s 8
		IL_007a: ldloca.s 7
		IL_007c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0081: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0086: ldloca.s 8
		IL_0088: ldloca.s 7
		IL_008a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_008f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0094: ldloca.s 8
		IL_0096: ldloca.s 7
		IL_0098: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_009d: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00a2: ldloc.s 8
		IL_00a4: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::Warp(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_00a9: stloc.s 9
		IL_00ab: ldarg.0
		IL_00ac: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PrincipalScript::agent
		IL_00b1: ldc.i4.1
		IL_00b2: callvirt instance void [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::set_isStopped(bool)
		IL_00b7: ldarg.1
		IL_00b8: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_00bd: stloc.s 10
		IL_00bf: ldarg.0
		IL_00c0: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PrincipalScript::playerDetentionPoint
		IL_00c5: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_00ca: stloc.s 11
		IL_00cc: ldarg.0
		IL_00cd: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PrincipalScript::playerDetentionPoint
		IL_00d2: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_00d7: stloc.s 12
		IL_00d9: ldloc.s 10
		IL_00db: ldloca.s 13
		IL_00dd: ldloca.s 11
		IL_00df: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00e4: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00e9: ldloca.s 13
		IL_00eb: ldc.r4 4
		IL_00f0: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00f5: ldloca.s 13
		IL_00f7: ldloca.s 12
		IL_00f9: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00fe: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0103: ldloc.s 13
		IL_0105: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_010a: ldarg.0
		IL_010b: ldfld class AudioQueueScript PrincipalScript::audioQueue
		IL_0110: ldfld int32 AudioQueueScript::audioInQueue
		IL_0115: conv.i8
		IL_0116: stloc.s 14
		IL_0118: ldarg.0
		IL_0119: ldfld class AudioQueueScript PrincipalScript::audioQueue
		IL_011e: stloc.s 15
		IL_0120: ldarg.0
		IL_0121: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip PrincipalScript::aud_Delay
		IL_0126: stloc.s 16
		IL_0128: ldarg.0
		IL_0129: ldfld class AudioQueueScript PrincipalScript::audioQueue
		IL_012e: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] AudioQueueScript::audioQueue
		IL_0133: stloc.s 17
		IL_0135: ldloc.s 17
		IL_0137: ldloc.s 14
		IL_0139: conv.i4
		IL_013a: ldloc.s 16
		IL_013c: stelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0141: ldloc.s 15
		IL_0143: ldloc.s 15
		IL_0145: ldfld int32 AudioQueueScript::audioInQueue
		IL_014a: ldc.i4.1
		IL_014b: add
		IL_014c: stfld int32 AudioQueueScript::audioInQueue
		IL_0151: ldarg.0
		IL_0152: ldfld class AudioQueueScript PrincipalScript::audioQueue
		IL_0157: ldfld int32 AudioQueueScript::audioInQueue
		IL_015c: conv.i8
		IL_015d: stloc.s 14
		IL_015f: ldarg.0
		IL_0160: ldfld class AudioQueueScript PrincipalScript::audioQueue
		IL_0165: stloc.s 18
		IL_0167: ldarg.0
		IL_0168: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] PrincipalScript::audTimes
		IL_016d: ldarg.0
		IL_016e: ldfld int32 PrincipalScript::detentions
		IL_0173: conv.i8
		IL_0174: conv.i4
		IL_0175: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_017a: stloc.s 19
		IL_017c: ldarg.0
		IL_017d: ldfld class AudioQueueScript PrincipalScript::audioQueue
		IL_0182: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] AudioQueueScript::audioQueue
		IL_0187: stloc.s 20
		IL_0189: ldloc.s 20
		IL_018b: ldloc.s 14
		IL_018d: conv.i4
		IL_018e: ldloc.s 19
		IL_0190: stelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0195: ldloc.s 18
		IL_0197: ldloc.s 18
		IL_0199: ldfld int32 AudioQueueScript::audioInQueue
		IL_019e: ldc.i4.1
		IL_019f: add
		IL_01a0: stfld int32 AudioQueueScript::audioInQueue
		IL_01a5: ldarg.0
		IL_01a6: ldfld class AudioQueueScript PrincipalScript::audioQueue
		IL_01ab: ldfld int32 AudioQueueScript::audioInQueue
		IL_01b0: conv.i8
		IL_01b1: stloc.s 14
		IL_01b3: ldarg.0
		IL_01b4: ldfld class AudioQueueScript PrincipalScript::audioQueue
		IL_01b9: stloc.s 21
		IL_01bb: ldarg.0
		IL_01bc: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip PrincipalScript::audDetention
		IL_01c1: stloc.s 22
		IL_01c3: ldarg.0
		IL_01c4: ldfld class AudioQueueScript PrincipalScript::audioQueue
		IL_01c9: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] AudioQueueScript::audioQueue
		IL_01ce: stloc.s 23
		IL_01d0: ldloc.s 23
		IL_01d2: ldloc.s 14
		IL_01d4: conv.i4
		IL_01d5: ldloc.s 22
		IL_01d7: stelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_01dc: ldloc.s 21
		IL_01de: ldloc.s 21
		IL_01e0: ldfld int32 AudioQueueScript::audioInQueue
		IL_01e5: ldc.i4.1
		IL_01e6: add
		IL_01e7: stfld int32 AudioQueueScript::audioInQueue
		IL_01ec: ldc.r4 0.0
		IL_01f1: ldc.r4 2
		IL_01f6: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_01fb: stloc.s 24
		IL_01fd: ldloc.s 24
		IL_01ff: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_0204: stloc.s 25
		IL_0206: ldarg.0
		IL_0207: ldfld class AudioQueueScript PrincipalScript::audioQueue
		IL_020c: ldfld int32 AudioQueueScript::audioInQueue
		IL_0211: conv.i8
		IL_0212: stloc.s 14
		IL_0214: ldarg.0
		IL_0215: ldfld class AudioQueueScript PrincipalScript::audioQueue
		IL_021a: stloc.s 26
		IL_021c: ldarg.0
		IL_021d: ldfld class AudioQueueScript PrincipalScript::audioQueue
		IL_0222: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] AudioQueueScript::audioQueue
		IL_0227: stloc.s 27
		IL_0229: ldarg.0
		IL_022a: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] PrincipalScript::audScolds
		IL_022f: ldloc.s 25
		IL_0231: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0236: stloc.s 28
		IL_0238: ldloc.s 27
		IL_023a: ldloc.s 14
		IL_023c: conv.i4
		IL_023d: ldloc.s 28
		IL_023f: stelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0244: ldloc.s 26
		IL_0246: ldloc.s 26
		IL_0248: ldfld int32 AudioQueueScript::audioInQueue
		IL_024d: ldc.i4.1
		IL_024e: add
		IL_024f: stfld int32 AudioQueueScript::audioInQueue
		IL_0254: ldarg.0
		IL_0255: ldfld int32[] PrincipalScript::lockTime
		IL_025a: ldarg.0
		IL_025b: ldfld int32 PrincipalScript::detentions
		IL_0260: conv.i8
		IL_0261: conv.i4
		IL_0262: ldelem [mscorlib]System.Int32
		IL_0267: stloc.s 29
		IL_0269: ldarg.0
		IL_026a: ldfld class DoorScript PrincipalScript::officeDoor
		IL_026f: stloc.s 30
		IL_0271: ldloc.s 30
		IL_0273: ldc.i4.1
		IL_0274: stfld bool DoorScript::bDoorLocked
		IL_0279: ldloc.s 30
		IL_027b: ldloc.s 29
		IL_027d: conv.r4
		IL_027e: stfld float32 DoorScript::lockTime
		IL_0283: ldloc.s 29
		IL_0285: conv.r4
		IL_0286: stloc.s 31
		IL_0288: ldarg.0
		IL_0289: ldfld class BaldiScript PrincipalScript::baldiScript
		IL_028e: callvirt instance bool [UnityEngine.CoreModule]UnityEngine.Behaviour::get_isActiveAndEnabled()
		IL_0293: stloc.s 32
		IL_0295: ldloc.s 32
		IL_0297: ldc.i4.0
		IL_0298: ceq
		IL_029a: brtrue IL_0328

		IL_029f: nop
		IL_02a0: ldarg.0
		IL_02a1: ldfld class BaldiScript PrincipalScript::baldiScript
		IL_02a6: stloc.s 33
		IL_02a8: ldarg.0
		IL_02a9: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_02ae: stloc.s 34
		IL_02b0: ldloc.s 33
		IL_02b2: stloc.s 35
		IL_02b4: ldloc.s 34
		IL_02b6: stloc.s 36
		IL_02b8: ldloc.s 36
		IL_02ba: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_02bf: stloc.s 37
		IL_02c1: ldloc.s 35
		IL_02c3: ldfld bool BaldiScript::antiHearing
		IL_02c8: ldc.i4.0
		IL_02c9: ceq
		IL_02cb: ldc.i4.0
		IL_02cc: ceq
		IL_02ce: brtrue.s IL_0328

		IL_02d0: nop
		IL_02d1: ldc.r4 8
		IL_02d6: ldloc.s 35
		IL_02d8: ldfld float32 BaldiScript::currentPriority
		IL_02dd: clt
		IL_02df: brtrue.s IL_0328

		IL_02e1: nop
		IL_02e2: ldloc.s 35
		IL_02e4: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent BaldiScript::agent
		IL_02e9: ldloca.s 38
		IL_02eb: ldloca.s 37
		IL_02ed: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_02f2: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_02f7: ldloca.s 38
		IL_02f9: ldloca.s 37
		IL_02fb: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0300: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0305: ldloca.s 38
		IL_0307: ldloca.s 37
		IL_0309: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_030e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0313: ldloc.s 38
		IL_0315: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::SetDestination(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_031a: stloc.s 39
		IL_031c: ldloc.s 35
		IL_031e: ldc.r4 8
		IL_0323: stfld float32 BaldiScript::currentPriority

		IL_0328: nop
		IL_0329: ldarg.0
		IL_032a: ldfld int32 PrincipalScript::detentions
		IL_032f: ldc.i4.1
		IL_0330: add
		IL_0331: stloc.s 40
		IL_0333: ldarg.0
		IL_0334: ldc.r4 5
		IL_0339: stfld float32 PrincipalScript::coolDown
		IL_033e: ldarg.0
		IL_033f: ldc.i4.0
		IL_0340: stfld bool PrincipalScript::angry
		IL_0345: ldarg.0
		IL_0346: ldloc.s 40
		IL_0348: stfld int32 PrincipalScript::detentions
		IL_034d: ldloc.s 40
		IL_034f: stloc.s 41
		IL_0351: ldloc.s 40
		IL_0353: ldc.i4.4
		IL_0354: cgt
		IL_0356: ldc.i4.0
		IL_0357: ceq
		IL_0359: brtrue.s IL_0363

		IL_035b: nop
		IL_035c: ldarg.0
		IL_035d: ldc.i4.4
		IL_035e: stfld int32 PrincipalScript::detentions

		IL_0363: nop
		IL_0364: ret

		IL_0365: nop
		IL_0366: ldnull
		IL_0367: throw

		IL_0368: nop
		IL_0369: ldnull
		IL_036a: throw
	} // end of method PrincipalScript::OnTriggerStay

	.method private hidebysig 
		instance void OnTriggerExit (
			class [UnityEngine.PhysicsModule]UnityEngine.Collider other
		) cil managed 
	{
		// Method begins at RVA 0xf454
		// Header size: 12
		// Code size: 76 (0x4c)
		.maxstack 2
		.locals init (
			[0] string,
			[1] bool,
			[2] string,
			[3] bool,
			[4] bool
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: ldstr "Office Trigger"
		IL_000e: call bool [mscorlib]System.String::Equals(string, string)
		IL_0013: stloc.1
		IL_0014: ldloc.1
		IL_0015: ldc.i4.0
		IL_0016: ceq
		IL_0018: brtrue.s IL_0022

		IL_001a: nop
		IL_001b: ldarg.0
		IL_001c: ldc.i4.0
		IL_001d: stfld bool PrincipalScript::inOffice

		IL_0022: nop
		IL_0023: ldarg.1
		IL_0024: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_0029: stloc.2
		IL_002a: ldloc.2
		IL_002b: ldstr "Its a Bully"
		IL_0030: call bool [mscorlib]System.String::Equals(string, string)
		IL_0035: stloc.3
		IL_0036: ldloc.3
		IL_0037: stloc.s 4
		IL_0039: ldloc.3
		IL_003a: ldc.i4.0
		IL_003b: ceq
		IL_003d: brtrue.s IL_0047

		IL_003f: nop
		IL_0040: ldarg.0
		IL_0041: ldc.i4.0
		IL_0042: stfld bool PrincipalScript::bullySeen

		IL_0047: nop
		IL_0048: ret

		IL_0049: nop
		IL_004a: ldnull
		IL_004b: throw
	} // end of method PrincipalScript::OnTriggerExit

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0xf4ac
		// Header size: 12
		// Code size: 76 (0x4c)
		.maxstack 2
		.locals init (
			[0] int32[],
			[1] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[2] class [UnityEngine.AudioModule]UnityEngine.AudioClip[]
		)

		IL_0000: nop
		IL_0001: ldc.i4.5
		IL_0002: newarr [mscorlib]System.Int32
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: ldstr "<PrivateImplementationDetails>::6BE9D134EF6D584CC78B34C0D3A332D6443042F2AA77BB4B36BD6E2F86CA7069_FieldInfo"
		IL_000e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0013: box [mscorlib]System.IntPtr
		IL_0018: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [mscorlib]System.RuntimeFieldHandle>(object)
		IL_001d: call void [mscorlib]System.Runtime.CompilerServices.RuntimeHelpers::InitializeArray(class [mscorlib]System.Array, valuetype [mscorlib]System.RuntimeFieldHandle)
		IL_0022: ldarg.0
		IL_0023: ldloc.0
		IL_0024: stfld int32[] PrincipalScript::lockTime
		IL_0029: ldc.i4.5
		IL_002a: newarr [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_002f: stloc.1
		IL_0030: ldarg.0
		IL_0031: ldloc.1
		IL_0032: stfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] PrincipalScript::audTimes
		IL_0037: ldc.i4.3
		IL_0038: newarr [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_003d: stloc.2
		IL_003e: ldarg.0
		IL_003f: ldloc.2
		IL_0040: stfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] PrincipalScript::audScolds
		IL_0045: ldarg.0
		IL_0046: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_004b: ret
	} // end of method PrincipalScript::.ctor

} // end of class PrincipalScript
