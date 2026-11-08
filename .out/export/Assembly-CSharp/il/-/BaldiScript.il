.class public auto ansi beforefieldinit BaldiScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public bool db
	.field public float32 baseTime
	.field public float32 speed
	.field public float32 timeToMove
	.field public float32 baldiAnger
	.field public float32 baldiTempAnger
	.field public float32 baldiWait
	.field public float32 baldiSpeedScale
	.field private float32 moveFrames
	.field private float32 currentPriority
	.field public bool antiHearing
	.field public float32 antiHearingTime
	.field public float32 vibrationDistance
	.field public float32 angerRate
	.field public float32 angerRateRate
	.field public float32 angerFrequency
	.field public float32 timeToAnger
	.field public bool endless
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform player
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform wanderTarget
	.field public class AILocationSelectorScript wanderer
	.field private class [UnityEngine.AudioModule]UnityEngine.AudioSource baldiAudio
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip slap
	.field public class [UnityEngine.AnimationModule]UnityEngine.Animator baldiAnimator
	.field public float32 coolDown
	.field private valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 previous
	.field private bool rumble
	.field private class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent agent

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0xb914
		// Header size: 12
		// Code size: 79 (0x4f)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.AudioModule]UnityEngine.AudioSource,
			[1] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[2] int32,
			[3] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.AudioModule]UnityEngine.AudioSource>()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.AudioModule]UnityEngine.AudioSource BaldiScript::baldiAudio
		IL_000f: ldarg.0
		IL_0010: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent>()
		IL_0015: stloc.1
		IL_0016: ldarg.0
		IL_0017: ldloc.1
		IL_0018: stfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent BaldiScript::agent
		IL_001d: ldarg.0
		IL_001e: ldarg.0
		IL_001f: ldfld float32 BaldiScript::baseTime
		IL_0024: stfld float32 BaldiScript::timeToMove
		IL_0029: ldarg.0
		IL_002a: callvirt instance void BaldiScript::Wander()
		IL_002f: ldstr "Rumble"
		IL_0034: call int32 [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::GetInt(string)
		IL_0039: stloc.2
		IL_003a: ldloc.2
		IL_003b: stloc.3
		IL_003c: ldloc.2
		IL_003d: ldc.i4.1
		IL_003e: ceq
		IL_0040: ldc.i4.0
		IL_0041: ceq
		IL_0043: brtrue.s IL_004d

		IL_0045: nop
		IL_0046: ldarg.0
		IL_0047: ldloc.3
		IL_0048: stfld bool BaldiScript::rumble

		IL_004d: nop
		IL_004e: ret
	} // end of method BaldiScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0xb970
		// Header size: 12
		// Code size: 406 (0x196)
		.maxstack 4
		.locals init (
			[0] float32,
			[1] float32,
			[2] float32,
			[3] float32,
			[4] float32,
			[5] float32,
			[6] float32,
			[7] float32,
			[8] float32,
			[9] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld float32 BaldiScript::timeToMove
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld float32 BaldiScript::timeToMove
		IL_000e: ldc.r4 0.0
		IL_0013: cgt
		IL_0015: brtrue.s IL_0020

		IL_0017: nop
		IL_0018: ldarg.0
		IL_0019: callvirt instance void BaldiScript::Move()
		IL_001e: br.s IL_0030

		IL_0020: nop
		IL_0021: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0026: stloc.1
		IL_0027: ldarg.0
		IL_0028: ldloc.0
		IL_0029: ldloc.1
		IL_002a: sub
		IL_002b: stfld float32 BaldiScript::timeToMove

		IL_0030: nop
		IL_0031: ldarg.0
		IL_0032: ldfld float32 BaldiScript::coolDown
		IL_0037: stloc.0
		IL_0038: ldarg.0
		IL_0039: ldfld float32 BaldiScript::coolDown
		IL_003e: ldc.r4 0.0
		IL_0043: cgt
		IL_0045: ldc.i4.0
		IL_0046: ceq
		IL_0048: brtrue.s IL_005a

		IL_004a: nop
		IL_004b: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0050: stloc.2
		IL_0051: ldarg.0
		IL_0052: ldloc.0
		IL_0053: ldloc.2
		IL_0054: sub
		IL_0055: stfld float32 BaldiScript::coolDown

		IL_005a: nop
		IL_005b: ldarg.0
		IL_005c: ldfld float32 BaldiScript::baldiTempAnger
		IL_0061: stloc.0
		IL_0062: ldarg.0
		IL_0063: ldfld float32 BaldiScript::baldiTempAnger
		IL_0068: ldc.r4 0.0
		IL_006d: cgt
		IL_006f: brtrue.s IL_007f

		IL_0071: nop
		IL_0072: ldarg.0
		IL_0073: ldc.r4 0.0
		IL_0078: stfld float32 BaldiScript::baldiTempAnger
		IL_007d: br.s IL_0095

		IL_007f: nop
		IL_0080: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0085: stloc.3
		IL_0086: ldarg.0
		IL_0087: ldloc.0
		IL_0088: ldloc.3
		IL_0089: ldc.r4 0.02
		IL_008e: mul
		IL_008f: sub
		IL_0090: stfld float32 BaldiScript::baldiTempAnger

		IL_0095: nop
		IL_0096: ldarg.0
		IL_0097: ldfld float32 BaldiScript::antiHearingTime
		IL_009c: stloc.0
		IL_009d: ldarg.0
		IL_009e: ldfld float32 BaldiScript::antiHearingTime
		IL_00a3: ldc.r4 0.0
		IL_00a8: cgt
		IL_00aa: brtrue.s IL_00b6

		IL_00ac: nop
		IL_00ad: ldarg.0
		IL_00ae: ldc.i4.0
		IL_00af: stfld bool BaldiScript::antiHearing
		IL_00b4: br.s IL_00cb

		IL_00b6: nop
		IL_00b7: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_00bc: stloc.s 4
		IL_00be: ldarg.0
		IL_00bf: ldloc.0
		IL_00c0: ldloc.s 4
		IL_00c2: sub
		IL_00c3: stfld float32 BaldiScript::antiHearingTime
		IL_00c8: ldloc.s 4
		IL_00ca: stloc.0

		IL_00cb: nop
		IL_00cc: ldarg.0
		IL_00cd: ldfld bool BaldiScript::endless
		IL_00d2: ldc.i4.0
		IL_00d3: ceq
		IL_00d5: brtrue IL_0194

		IL_00da: nop
		IL_00db: ldarg.0
		IL_00dc: ldfld float32 BaldiScript::timeToAnger
		IL_00e1: stloc.0
		IL_00e2: ldarg.0
		IL_00e3: ldfld float32 BaldiScript::timeToAnger
		IL_00e8: ldc.r4 0.0
		IL_00ed: cgt
		IL_00ef: brtrue IL_017f

		IL_00f4: nop
		IL_00f5: ldarg.0
		IL_00f6: ldfld float32 BaldiScript::angerRate
		IL_00fb: ldarg.0
		IL_00fc: ldfld float32 BaldiScript::baldiAnger
		IL_0101: add
		IL_0102: stloc.s 5
		IL_0104: ldarg.0
		IL_0105: ldfld float32 BaldiScript::angerFrequency
		IL_010a: stloc.s 6
		IL_010c: ldarg.0
		IL_010d: ldloc.s 6
		IL_010f: stfld float32 BaldiScript::timeToAnger
		IL_0114: ldarg.0
		IL_0115: ldloc.s 5
		IL_0117: stfld float32 BaldiScript::baldiAnger
		IL_011c: ldloc.s 6
		IL_011e: stloc.s 7
		IL_0120: ldc.r4 0.5
		IL_0125: ldloc.s 5
		IL_0127: cgt
		IL_0129: ldc.i4.0
		IL_012a: ceq
		IL_012c: brtrue.s IL_013a

		IL_012e: nop
		IL_012f: ldarg.0
		IL_0130: ldc.r4 0.5
		IL_0135: stfld float32 BaldiScript::baldiAnger

		IL_013a: nop
		IL_013b: ldarg.0
		IL_013c: ldfld float32 BaldiScript::angerRateRate
		IL_0141: ldarg.0
		IL_0142: ldfld float32 BaldiScript::angerRate
		IL_0147: add
		IL_0148: stloc.s 8
		IL_014a: ldarg.0
		IL_014b: ldarg.0
		IL_014c: ldfld float32 BaldiScript::baldiAnger
		IL_0151: ldc.r4 -3
		IL_0156: mul
		IL_0157: ldc.r4 2
		IL_015c: ldarg.0
		IL_015d: ldfld float32 BaldiScript::baldiSpeedScale
		IL_0162: div
		IL_0163: ldarg.0
		IL_0164: ldfld float32 BaldiScript::baldiAnger
		IL_0169: add
		IL_016a: div
		IL_016b: ldc.r4 3
		IL_0170: add
		IL_0171: stfld float32 BaldiScript::baldiWait
		IL_0176: ldarg.0
		IL_0177: ldloc.s 8
		IL_0179: stfld float32 BaldiScript::angerRate
		IL_017e: ret

		IL_017f: nop
		IL_0180: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0185: stloc.s 9
		IL_0187: ldarg.0
		IL_0188: ldloc.0
		IL_0189: ldloc.s 9
		IL_018b: sub
		IL_018c: stfld float32 BaldiScript::timeToAnger
		IL_0191: ldloc.s 9
		IL_0193: stloc.0

		IL_0194: nop
		IL_0195: ret
	} // end of method BaldiScript::Update

	.method private hidebysig 
		instance void FixedUpdate () cil managed 
	{
		// Method begins at RVA 0xbb14
		// Header size: 12
		// Code size: 151 (0x97)
		.maxstack 3
		.locals init (
			[0] float32,
			[1] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[2] float32,
			[3] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[4] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[5] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[6] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[7] float32,
			[8] float32,
			[9] float32,
			[10] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[11] class [UnityEngine.CoreModule]UnityEngine.Transform
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld float32 BaldiScript::moveFrames
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent BaldiScript::agent
		IL_000e: stloc.1
		IL_000f: ldarg.0
		IL_0010: ldfld float32 BaldiScript::moveFrames
		IL_0015: ldc.r4 0.0
		IL_001a: cgt
		IL_001c: brtrue.s IL_0022

		IL_001e: nop
		IL_001f: nop
		IL_0020: br.s IL_0040

		IL_0022: nop
		IL_0023: ldarg.0
		IL_0024: ldloc.0
		IL_0025: ldc.r4 1
		IL_002a: sub
		IL_002b: stfld float32 BaldiScript::moveFrames
		IL_0030: ldloc.0
		IL_0031: ldc.r4 1
		IL_0036: sub
		IL_0037: stloc.0
		IL_0038: nop
		IL_0039: ldarg.0
		IL_003a: ldfld float32 BaldiScript::speed
		IL_003f: stloc.2

		IL_0040: nop
		IL_0041: ldloc.1
		IL_0042: ldloc.2
		IL_0043: callvirt instance void [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::set_speed(float32)
		IL_0048: ldarg.0
		IL_0049: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform BaldiScript::player
		IL_004e: stloc.3
		IL_004f: nop
		IL_0050: ldloc.3
		IL_0051: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0056: stloc.s 4
		IL_0058: ldarg.0
		IL_0059: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_005e: stloc.s 5
		IL_0060: ldloc.s 5
		IL_0062: stloc.s 6
		IL_0064: ldloca.s 4
		IL_0066: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_006b: stloc.s 7
		IL_006d: ldloca.s 4
		IL_006f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0074: stloc.s 8
		IL_0076: ldloca.s 4
		IL_0078: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_007d: stloc.s 9
		IL_007f: nop
		IL_0080: ldloc.s 6
		IL_0082: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0087: stloc.s 10
		IL_0089: ldarg.0
		IL_008a: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_008f: stloc.s 11
		IL_0091: ldloc.s 11
		IL_0093: stloc.s 6
		IL_0095: nop
		IL_0096: ret
	} // end of method BaldiScript::FixedUpdate

	.method private hidebysig 
		instance void Wander () cil managed 
	{
		// Method begins at RVA 0xbbb8
		// Header size: 12
		// Code size: 107 (0x6b)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[3] bool
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class AILocationSelectorScript BaldiScript::wanderer
		IL_0007: callvirt instance void AILocationSelectorScript::GetNewTarget()
		IL_000c: ldarg.0
		IL_000d: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent BaldiScript::agent
		IL_0012: stloc.0
		IL_0013: ldarg.0
		IL_0014: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform BaldiScript::wanderTarget
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
		IL_0057: stfld float32 BaldiScript::coolDown
		IL_005c: ldarg.0
		IL_005d: ldc.r4 0.0
		IL_0062: stfld float32 BaldiScript::currentPriority
		IL_0067: ret

		IL_0068: nop
		IL_0069: ldnull
		IL_006a: throw
	} // end of method BaldiScript::Wander

	.method public hidebysig 
		instance void TargetPlayer () cil managed 
	{
		// Method begins at RVA 0xbc30
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
		IL_0002: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent BaldiScript::agent
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform BaldiScript::player
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
		IL_004c: stfld float32 BaldiScript::coolDown
		IL_0051: ldarg.0
		IL_0052: ldc.r4 0.0
		IL_0057: stfld float32 BaldiScript::currentPriority
		IL_005c: ret

		IL_005d: nop
		IL_005e: ldnull
		IL_005f: throw
	} // end of method BaldiScript::TargetPlayer

	.method private hidebysig 
		instance void Move () cil managed 
	{
		// Method begins at RVA 0xbc9c
		// Header size: 12
		// Code size: 540 (0x21c)
		.maxstack 6
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[2] float32,
			[3] float32,
			[4] float32,
			[5] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[6] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[7] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[8] int64,
			[9] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[10] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[11] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[12] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_000e: stloc.1
		IL_000f: ldloca.s 1
		IL_0011: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0016: ldarg.0
		IL_0017: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BaldiScript::previous
		IL_001c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0021: sub
		IL_0022: ldloca.s 1
		IL_0024: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0029: ldarg.0
		IL_002a: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BaldiScript::previous
		IL_002f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0034: sub
		IL_0035: mul
		IL_0036: stloc.2
		IL_0037: ldloca.s 1
		IL_0039: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_003e: ldarg.0
		IL_003f: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BaldiScript::previous
		IL_0044: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0049: sub
		IL_004a: ldloca.s 1
		IL_004c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0051: ldarg.0
		IL_0052: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BaldiScript::previous
		IL_0057: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_005c: sub
		IL_005d: mul
		IL_005e: ldloca.s 1
		IL_0060: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0065: ldarg.0
		IL_0066: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BaldiScript::previous
		IL_006b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0070: sub
		IL_0071: ldloca.s 1
		IL_0073: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0078: ldarg.0
		IL_0079: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BaldiScript::previous
		IL_007e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0083: sub
		IL_0084: mul
		IL_0085: add
		IL_0086: ldloca.s 1
		IL_0088: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_008d: ldarg.0
		IL_008e: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BaldiScript::previous
		IL_0093: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0098: sub
		IL_0099: ldloca.s 1
		IL_009b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00a0: ldarg.0
		IL_00a1: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BaldiScript::previous
		IL_00a6: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00ab: sub
		IL_00ac: mul
		IL_00ad: add
		IL_00ae: stloc.3
		IL_00af: ldc.r4 0.0
		IL_00b4: ldarg.0
		IL_00b5: ldfld float32 BaldiScript::coolDown
		IL_00ba: cgt
		IL_00bc: ldc.r4 9.9999994E-11
		IL_00c1: ldloca.s 1
		IL_00c3: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00c8: ldarg.0
		IL_00c9: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BaldiScript::previous
		IL_00ce: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00d3: sub
		IL_00d4: ldloca.s 1
		IL_00d6: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00db: ldarg.0
		IL_00dc: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BaldiScript::previous
		IL_00e1: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00e6: sub
		IL_00e7: mul
		IL_00e8: ldloca.s 1
		IL_00ea: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00ef: ldarg.0
		IL_00f0: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BaldiScript::previous
		IL_00f5: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00fa: sub
		IL_00fb: ldloca.s 1
		IL_00fd: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0102: ldarg.0
		IL_0103: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BaldiScript::previous
		IL_0108: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_010d: sub
		IL_010e: mul
		IL_010f: add
		IL_0110: ldloca.s 1
		IL_0112: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0117: ldarg.0
		IL_0118: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BaldiScript::previous
		IL_011d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0122: sub
		IL_0123: ldloca.s 1
		IL_0125: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_012a: ldarg.0
		IL_012b: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BaldiScript::previous
		IL_0130: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0135: sub
		IL_0136: mul
		IL_0137: add
		IL_0138: cgt
		IL_013a: and
		IL_013b: ldc.i4.0
		IL_013c: ceq
		IL_013e: brtrue.s IL_0147

		IL_0140: nop
		IL_0141: ldarg.0
		IL_0142: callvirt instance void BaldiScript::Wander()

		IL_0147: nop
		IL_0148: ldarg.0
		IL_0149: ldfld float32 BaldiScript::baldiWait
		IL_014e: ldarg.0
		IL_014f: ldfld float32 BaldiScript::baldiTempAnger
		IL_0154: sub
		IL_0155: stloc.s 4
		IL_0157: ldarg.0
		IL_0158: ldc.r4 10
		IL_015d: stfld float32 BaldiScript::moveFrames
		IL_0162: ldarg.0
		IL_0163: ldloc.s 4
		IL_0165: stfld float32 BaldiScript::timeToMove
		IL_016a: ldarg.0
		IL_016b: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0170: stloc.s 5
		IL_0172: ldloc.s 5
		IL_0174: stloc.s 6
		IL_0176: ldloc.s 6
		IL_0178: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_017d: stloc.s 7
		IL_017f: ldarg.0
		IL_0180: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BaldiScript::previous
		IL_0185: ldloca.s 7
		IL_0187: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_018c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0191: ldarg.0
		IL_0192: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BaldiScript::previous
		IL_0197: ldloca.s 7
		IL_0199: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_019e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_01a3: ldarg.0
		IL_01a4: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BaldiScript::previous
		IL_01a9: ldloca.s 7
		IL_01ab: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_01b0: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_01b5: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_01ba: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01bf: stloc.s 8
		IL_01c1: ldarg.0
		IL_01c2: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource BaldiScript::baldiAudio
		IL_01c7: ldarg.0
		IL_01c8: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip BaldiScript::slap
		IL_01cd: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_01d2: ldarg.0
		IL_01d3: ldfld class [UnityEngine.AnimationModule]UnityEngine.Animator BaldiScript::baldiAnimator
		IL_01d8: ldstr "slap"
		IL_01dd: callvirt instance void [UnityEngine.AnimationModule]UnityEngine.Animator::SetTrigger(string)
		IL_01e2: ldarg.0
		IL_01e3: ldfld bool BaldiScript::rumble
		IL_01e8: ldc.i4.0
		IL_01e9: ceq
		IL_01eb: brtrue.s IL_0217

		IL_01ed: nop
		IL_01ee: ldarg.0
		IL_01ef: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_01f4: stloc.s 9
		IL_01f6: ldloc.s 9
		IL_01f8: stloc.s 6
		IL_01fa: ldloc.s 6
		IL_01fc: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0201: stloc.s 10
		IL_0203: ldarg.0
		IL_0204: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform BaldiScript::player
		IL_0209: stloc.s 11
		IL_020b: ldloc.s 11
		IL_020d: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0212: stloc.s 12
		IL_0214: br.s IL_0217

		IL_0216: nop

		IL_0217: nop
		IL_0218: ret

		IL_0219: nop
		IL_021a: ldnull
		IL_021b: throw
	} // end of method BaldiScript::Move

	.method public hidebysig 
		instance void GetAngry (
			float32 'value'
		) cil managed 
	{
		// Method begins at RVA 0xbec4
		// Header size: 12
		// Code size: 113 (0x71)
		.maxstack 4
		.locals init (
			[0] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldarg.1
		IL_0003: ldarg.0
		IL_0004: ldfld float32 BaldiScript::baldiAnger
		IL_0009: add
		IL_000a: stfld float32 BaldiScript::baldiAnger
		IL_000f: ldc.r4 0.5
		IL_0014: ldarg.1
		IL_0015: ldarg.0
		IL_0016: ldfld float32 BaldiScript::baldiAnger
		IL_001b: add
		IL_001c: cgt
		IL_001e: ldc.i4.0
		IL_001f: ceq
		IL_0021: brtrue.s IL_002f

		IL_0023: nop
		IL_0024: ldarg.0
		IL_0025: ldc.r4 0.5
		IL_002a: stfld float32 BaldiScript::baldiAnger

		IL_002f: nop
		IL_0030: ldc.r4 2
		IL_0035: ldarg.0
		IL_0036: ldfld float32 BaldiScript::baldiSpeedScale
		IL_003b: div
		IL_003c: ldarg.0
		IL_003d: ldfld float32 BaldiScript::baldiAnger
		IL_0042: add
		IL_0043: stloc.0
		IL_0044: ldarg.0
		IL_0045: ldarg.0
		IL_0046: ldfld float32 BaldiScript::baldiAnger
		IL_004b: ldc.r4 -3
		IL_0050: mul
		IL_0051: ldc.r4 2
		IL_0056: ldarg.0
		IL_0057: ldfld float32 BaldiScript::baldiSpeedScale
		IL_005c: div
		IL_005d: ldarg.0
		IL_005e: ldfld float32 BaldiScript::baldiAnger
		IL_0063: add
		IL_0064: div
		IL_0065: ldc.r4 3
		IL_006a: add
		IL_006b: stfld float32 BaldiScript::baldiWait
		IL_0070: ret
	} // end of method BaldiScript::GetAngry

	.method public hidebysig 
		instance void GetTempAngry (
			float32 'value'
		) cil managed 
	{
		// Method begins at RVA 0x24df
		// Header size: 1
		// Code size: 16 (0x10)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldarg.1
		IL_0003: ldarg.0
		IL_0004: ldfld float32 BaldiScript::baldiTempAnger
		IL_0009: add
		IL_000a: stfld float32 BaldiScript::baldiTempAnger
		IL_000f: ret
	} // end of method BaldiScript::GetTempAngry

	.method public hidebysig 
		instance void Hear (
			valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 soundLocation,
			float32 priority
		) cil managed 
	{
		// Method begins at RVA 0xbf44
		// Header size: 12
		// Code size: 94 (0x5e)
		.maxstack 3
		.locals init (
			[0] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[1] bool,
			[2] bool
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld bool BaldiScript::antiHearing
		IL_0007: ldc.i4.0
		IL_0008: ceq
		IL_000a: ldc.i4.0
		IL_000b: ceq
		IL_000d: brtrue.s IL_0059

		IL_000f: nop
		IL_0010: ldarg.2
		IL_0011: ldarg.0
		IL_0012: ldfld float32 BaldiScript::currentPriority
		IL_0017: clt
		IL_0019: brtrue.s IL_0059

		IL_001b: nop
		IL_001c: ldarg.0
		IL_001d: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent BaldiScript::agent
		IL_0022: ldloca.s 0
		IL_0024: ldarg.1
		IL_0025: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_002a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_002f: ldloca.s 0
		IL_0031: ldarg.1
		IL_0032: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0037: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_003c: ldloca.s 0
		IL_003e: ldarg.1
		IL_003f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0044: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0049: ldloc.0
		IL_004a: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::SetDestination(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_004f: stloc.1
		IL_0050: ldarg.0
		IL_0051: ldarg.2
		IL_0052: stfld float32 BaldiScript::currentPriority
		IL_0057: ldloc.1
		IL_0058: stloc.2

		IL_0059: nop
		IL_005a: ret

		IL_005b: nop
		IL_005c: ldnull
		IL_005d: throw
	} // end of method BaldiScript::Hear

	.method public hidebysig 
		instance void ActivateAntiHearing (
			float32 t
		) cil managed 
	{
		// Method begins at RVA 0x24f0
		// Header size: 1
		// Code size: 22 (0x16)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void BaldiScript::Wander()
		IL_0007: ldarg.0
		IL_0008: ldarg.1
		IL_0009: stfld float32 BaldiScript::antiHearingTime
		IL_000e: ldarg.0
		IL_000f: ldc.i4.1
		IL_0010: stfld bool BaldiScript::antiHearing
		IL_0015: ret
	} // end of method BaldiScript::ActivateAntiHearing

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2507
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method BaldiScript::.ctor

} // end of class BaldiScript
