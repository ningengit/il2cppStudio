.class public auto ansi beforefieldinit SweepScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform wanderTarget
	.field public class AILocationSelectorScript wanderer
	.field public float32 coolDown
	.field public float32 waitTime
	.field public int32 wanders
	.field public bool active
	.field private valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 origin
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_Sweep
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_Intro
	.field private class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent agent
	.field private class [UnityEngine.AudioModule]UnityEngine.AudioSource audioDevice

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0xf5a4
		// Header size: 12
		// Code size: 126 (0x7e)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[1] class [UnityEngine.AudioModule]UnityEngine.AudioSource,
			[2] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[4] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent>()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent SweepScript::agent
		IL_000f: ldarg.0
		IL_0010: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.AudioModule]UnityEngine.AudioSource>()
		IL_0015: stloc.1
		IL_0016: ldarg.0
		IL_0017: ldloc.1
		IL_0018: stfld class [UnityEngine.AudioModule]UnityEngine.AudioSource SweepScript::audioDevice
		IL_001d: ldarg.0
		IL_001e: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0023: stloc.2
		IL_0024: ldloc.2
		IL_0025: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_002a: stloc.3
		IL_002b: ldarg.0
		IL_002c: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 SweepScript::origin
		IL_0031: ldloca.s 3
		IL_0033: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0038: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_003d: ldarg.0
		IL_003e: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 SweepScript::origin
		IL_0043: ldloca.s 3
		IL_0045: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_004a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_004f: ldarg.0
		IL_0050: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 SweepScript::origin
		IL_0055: ldloca.s 3
		IL_0057: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_005c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0061: ldc.r4 120
		IL_0066: ldc.r4 180
		IL_006b: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_0070: stloc.s 4
		IL_0072: ldarg.0
		IL_0073: ldloc.s 4
		IL_0075: stfld float32 SweepScript::waitTime
		IL_007a: ret

		IL_007b: nop
		IL_007c: ldnull
		IL_007d: throw
	} // end of method SweepScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0xf630
		// Header size: 12
		// Code size: 142 (0x8e)
		.maxstack 3
		.locals init (
			[0] float32,
			[1] float32,
			[2] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld float32 SweepScript::coolDown
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld float32 SweepScript::coolDown
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
		IL_0025: stfld float32 SweepScript::coolDown

		IL_002a: nop
		IL_002b: ldarg.0
		IL_002c: ldfld float32 SweepScript::waitTime
		IL_0031: stloc.0
		IL_0032: ldarg.0
		IL_0033: ldfld float32 SweepScript::waitTime
		IL_0038: ldc.r4 0.0
		IL_003d: cgt
		IL_003f: brtrue.s IL_0077

		IL_0041: nop
		IL_0042: ldarg.0
		IL_0043: ldfld bool SweepScript::active
		IL_0048: ldc.i4.0
		IL_0049: ceq
		IL_004b: ldc.i4.0
		IL_004c: ceq
		IL_004e: brtrue.s IL_0089

		IL_0050: nop
		IL_0051: ldarg.0
		IL_0052: ldc.i4.1
		IL_0053: stfld bool SweepScript::active
		IL_0058: ldarg.0
		IL_0059: ldc.i4.0
		IL_005a: stfld int32 SweepScript::wanders
		IL_005f: ldarg.0
		IL_0060: callvirt instance void SweepScript::Wander()
		IL_0065: ldarg.0
		IL_0066: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource SweepScript::audioDevice
		IL_006b: ldarg.0
		IL_006c: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip SweepScript::aud_Intro
		IL_0071: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_0076: ret

		IL_0077: nop
		IL_0078: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_007d: stloc.2
		IL_007e: ldarg.0
		IL_007f: ldloc.0
		IL_0080: ldloc.2
		IL_0081: sub
		IL_0082: stfld float32 SweepScript::waitTime
		IL_0087: ldloc.2
		IL_0088: stloc.0

		IL_0089: nop
		IL_008a: ret

		IL_008b: nop
		IL_008c: ldnull
		IL_008d: throw
	} // end of method SweepScript::Update

	.method private hidebysig 
		instance void FixedUpdate () cil managed 
	{
		// Method begins at RVA 0xf6cc
		// Header size: 12
		// Code size: 290 (0x122)
		.maxstack 3
		.locals init (
			[0] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[2] float32,
			[3] int32,
			[4] int32,
			[5] float64,
			[6] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[7] bool,
			[8] float32,
			[9] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent SweepScript::agent
		IL_0007: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::get_velocity()
		IL_000c: stloc.0
		IL_000d: ldloca.s 1
		IL_000f: ldloca.s 0
		IL_0011: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0016: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_001b: ldloca.s 1
		IL_001d: ldloca.s 0
		IL_001f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0024: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0029: ldloca.s 1
		IL_002b: ldloca.s 0
		IL_002d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0032: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0037: ldloca.s 1
		IL_0039: call instance float32 [UnityEngine.CoreModule]UnityEngine.Vector3::get_magnitude()
		IL_003e: stloc.2
		IL_003f: ldarg.0
		IL_0040: ldfld int32 SweepScript::wanders
		IL_0045: ldc.i4.5
		IL_0046: clt
		IL_0048: brfalse.s IL_006e

		IL_004a: ldc.r8 0.1
		IL_0053: ldloc.2
		IL_0054: conv.r8
		IL_0055: clt
		IL_0057: ldc.i4.0
		IL_0058: ceq
		IL_005a: ldc.r4 0.0
		IL_005f: ldarg.0
		IL_0060: ldfld float32 SweepScript::coolDown
		IL_0065: clt
		IL_0067: ldc.i4.0
		IL_0068: ceq
		IL_006a: and
		IL_006b: stloc.3
		IL_006c: br.s IL_0071

		IL_006e: nop
		IL_006f: ldc.i4.0
		IL_0070: stloc.3

		IL_0071: nop
		IL_0072: ldc.r4 0.0
		IL_0077: ldarg.0
		IL_0078: ldfld float32 SweepScript::coolDown
		IL_007d: clt
		IL_007f: ldc.i4.0
		IL_0080: ceq
		IL_0082: stloc.s 4
		IL_0084: ldloc.2
		IL_0085: conv.r8
		IL_0086: stloc.s 5
		IL_0088: ldarg.0
		IL_0089: ldfld bool SweepScript::active
		IL_008e: ldloc.3
		IL_008f: and
		IL_0090: ldc.i4.0
		IL_0091: ceq
		IL_0093: ldc.i4.0
		IL_0094: ceq
		IL_0096: brtrue.s IL_0117

		IL_0098: nop
		IL_0099: ldarg.0
		IL_009a: ldfld int32 SweepScript::wanders
		IL_009f: ldc.i4.5
		IL_00a0: clt
		IL_00a2: brtrue.s IL_0115

		IL_00a4: nop
		IL_00a5: ldarg.0
		IL_00a6: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent SweepScript::agent
		IL_00ab: ldloca.s 6
		IL_00ad: ldarg.0
		IL_00ae: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 SweepScript::origin
		IL_00b3: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00b8: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00bd: ldloca.s 6
		IL_00bf: ldarg.0
		IL_00c0: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 SweepScript::origin
		IL_00c5: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00ca: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00cf: ldloca.s 6
		IL_00d1: ldarg.0
		IL_00d2: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 SweepScript::origin
		IL_00d7: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00dc: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00e1: ldloc.s 6
		IL_00e3: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::SetDestination(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_00e8: stloc.s 7
		IL_00ea: ldc.r4 120
		IL_00ef: ldc.r4 180
		IL_00f4: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_00f9: stloc.s 8
		IL_00fb: ldarg.0
		IL_00fc: ldloc.s 8
		IL_00fe: stfld float32 SweepScript::waitTime
		IL_0103: ldarg.0
		IL_0104: ldc.i4.0
		IL_0105: stfld int32 SweepScript::wanders
		IL_010a: ldarg.0
		IL_010b: ldc.i4.0
		IL_010c: stfld bool SweepScript::active
		IL_0111: ldloc.s 8
		IL_0113: stloc.s 9

		IL_0115: nop
		IL_0116: ret

		IL_0117: nop
		IL_0118: ldarg.0
		IL_0119: callvirt instance void SweepScript::Wander()
		IL_011e: ret

		IL_011f: nop
		IL_0120: ldnull
		IL_0121: throw
	} // end of method SweepScript::FixedUpdate

	.method private hidebysig 
		instance void Wander () cil managed 
	{
		// Method begins at RVA 0xf7fc
		// Header size: 12
		// Code size: 110 (0x6e)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[3] bool
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class AILocationSelectorScript SweepScript::wanderer
		IL_0007: callvirt instance void AILocationSelectorScript::GetNewTargetHallway()
		IL_000c: ldarg.0
		IL_000d: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent SweepScript::agent
		IL_0012: stloc.0
		IL_0013: ldarg.0
		IL_0014: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform SweepScript::wanderTarget
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
		IL_0052: ldarg.0
		IL_0053: ldfld int32 SweepScript::wanders
		IL_0058: ldc.i4.1
		IL_0059: add
		IL_005a: stfld int32 SweepScript::wanders
		IL_005f: ldarg.0
		IL_0060: ldc.r4 1
		IL_0065: stfld float32 SweepScript::coolDown
		IL_006a: ret

		IL_006b: nop
		IL_006c: ldnull
		IL_006d: throw
	} // end of method SweepScript::Wander

	.method private hidebysig 
		instance void GoHome () cil managed 
	{
		// Method begins at RVA 0xf878
		// Header size: 12
		// Code size: 109 (0x6d)
		.maxstack 3
		.locals init (
			[0] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[1] bool,
			[2] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent SweepScript::agent
		IL_0007: ldloca.s 0
		IL_0009: ldarg.0
		IL_000a: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 SweepScript::origin
		IL_000f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0014: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0019: ldloca.s 0
		IL_001b: ldarg.0
		IL_001c: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 SweepScript::origin
		IL_0021: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0026: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_002b: ldloca.s 0
		IL_002d: ldarg.0
		IL_002e: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 SweepScript::origin
		IL_0033: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0038: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_003d: ldloc.0
		IL_003e: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::SetDestination(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0043: stloc.1
		IL_0044: ldc.r4 120
		IL_0049: ldc.r4 180
		IL_004e: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_0053: stloc.2
		IL_0054: ldarg.0
		IL_0055: ldloc.2
		IL_0056: stfld float32 SweepScript::waitTime
		IL_005b: ldarg.0
		IL_005c: ldc.i4.0
		IL_005d: stfld int32 SweepScript::wanders
		IL_0062: ldarg.0
		IL_0063: ldc.i4.0
		IL_0064: stfld bool SweepScript::active
		IL_0069: ret

		IL_006a: nop
		IL_006b: ldnull
		IL_006c: throw
	} // end of method SweepScript::GoHome

	.method private hidebysig 
		instance void OnTriggerEnter (
			class [UnityEngine.PhysicsModule]UnityEngine.Collider other
		) cil managed 
	{
		// Method begins at RVA 0xf8f4
		// Header size: 12
		// Code size: 81 (0x51)
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
		IL_0002: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Component::get_tag()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: ldstr "NPC"
		IL_000e: call bool [mscorlib]System.String::Equals(string, string)
		IL_0013: stloc.1
		IL_0014: ldloc.1
		IL_0015: ldc.i4.0
		IL_0016: ceq
		IL_0018: ldc.i4.0
		IL_0019: ceq
		IL_001b: brtrue.s IL_003a

		IL_001d: nop
		IL_001e: ldarg.1
		IL_001f: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Component::get_tag()
		IL_0024: stloc.2
		IL_0025: ldloc.2
		IL_0026: ldstr "Player"
		IL_002b: call bool [mscorlib]System.String::Equals(string, string)
		IL_0030: stloc.3
		IL_0031: ldloc.3
		IL_0032: stloc.s 4
		IL_0034: ldloc.3
		IL_0035: ldc.i4.0
		IL_0036: ceq
		IL_0038: brtrue.s IL_004c

		IL_003a: nop
		IL_003b: ldarg.0
		IL_003c: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource SweepScript::audioDevice
		IL_0041: ldarg.0
		IL_0042: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip SweepScript::aud_Sweep
		IL_0047: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)

		IL_004c: nop
		IL_004d: ret

		IL_004e: nop
		IL_004f: ldnull
		IL_0050: throw
	} // end of method SweepScript::OnTriggerEnter

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x259a
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method SweepScript::.ctor

} // end of class SweepScript
