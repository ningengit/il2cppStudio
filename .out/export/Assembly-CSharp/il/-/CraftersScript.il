.class public auto ansi beforefieldinit CraftersScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public bool db
	.field public bool angry
	.field public bool gettingAngry
	.field public float32 anger
	.field private float32 forceShowTime
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform player
	.field public class [UnityEngine.PhysicsModule]UnityEngine.CharacterController cc
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform playerCamera
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform baldiTeleportPoint
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform playerTeleportPoint
	.field public class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent baldiAgent
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject sprite
	.field public class GameControllerScript gc
	.field private class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent agent
	.field public class [UnityEngine.CoreModule]UnityEngine.Renderer craftersRenderer
	.field public class [UnityEngine.CoreModule]UnityEngine.SpriteRenderer spriteImage
	.field public class [UnityEngine.CoreModule]UnityEngine.Sprite angrySprite
	.field private class [UnityEngine.AudioModule]UnityEngine.AudioSource audioDevice
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_Intro
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_Loop

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0xcaf0
		// Header size: 12
		// Code size: 45 (0x2d)
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
		IL_000a: stfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent CraftersScript::agent
		IL_000f: ldarg.0
		IL_0010: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.AudioModule]UnityEngine.AudioSource>()
		IL_0015: stloc.1
		IL_0016: ldarg.0
		IL_0017: ldloc.1
		IL_0018: stfld class [UnityEngine.AudioModule]UnityEngine.AudioSource CraftersScript::audioDevice
		IL_001d: ldarg.0
		IL_001e: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject CraftersScript::sprite
		IL_0023: ldc.i4.0
		IL_0024: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0029: ret

		IL_002a: nop
		IL_002b: ldnull
		IL_002c: throw
	} // end of method CraftersScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0xcb2c
		// Header size: 12
		// Code size: 688 (0x2b0)
		.maxstack 3
		.locals init (
			[0] float32,
			[1] float32,
			[2] float32,
			[3] float32,
			[4] float32,
			[5] class [UnityEngine.AudioModule]UnityEngine.AudioSource,
			[6] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[7] float32,
			[8] float32,
			[9] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[10] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[11] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[12] bool,
			[13] bool,
			[14] bool,
			[15] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[16] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[17] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[18] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[19] float32,
			[20] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[21] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[22] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[23] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[24] float32,
			[25] class [UnityEngine.CoreModule]UnityEngine.GameObject
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld float32 CraftersScript::forceShowTime
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld float32 CraftersScript::forceShowTime
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
		IL_0025: stfld float32 CraftersScript::forceShowTime
		IL_002a: ldloc.1
		IL_002b: stloc.0

		IL_002c: nop
		IL_002d: ldarg.0
		IL_002e: ldfld float32 CraftersScript::anger
		IL_0033: stloc.2
		IL_0034: ldarg.0
		IL_0035: ldfld bool CraftersScript::gettingAngry
		IL_003a: ldc.i4.0
		IL_003b: ceq
		IL_003d: ldc.i4.0
		IL_003e: ceq
		IL_0040: brtrue.s IL_0064

		IL_0042: nop
		IL_0043: ldloc.2
		IL_0044: ldc.r4 0.0
		IL_0049: cgt
		IL_004b: ldc.i4.0
		IL_004c: ceq
		IL_004e: brtrue.s IL_00c6

		IL_0050: nop
		IL_0051: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0056: stloc.3
		IL_0057: ldarg.0
		IL_0058: ldloc.2
		IL_0059: ldloc.3
		IL_005a: sub
		IL_005b: stfld float32 CraftersScript::anger
		IL_0060: ldloc.3
		IL_0061: stloc.0
		IL_0062: br.s IL_00c6

		IL_0064: nop
		IL_0065: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_006a: stloc.s 4
		IL_006c: ldarg.0
		IL_006d: ldloc.s 4
		IL_006f: ldloc.2
		IL_0070: add
		IL_0071: stfld float32 CraftersScript::anger
		IL_0076: ldloc.s 4
		IL_0078: ldloc.2
		IL_0079: add
		IL_007a: stloc.0
		IL_007b: ldarg.0
		IL_007c: ldfld bool CraftersScript::angry
		IL_0081: ldc.i4.0
		IL_0082: ceq
		IL_0084: ldloc.s 4
		IL_0086: ldloc.2
		IL_0087: add
		IL_0088: ldc.r4 1
		IL_008d: clt
		IL_008f: ldc.i4.0
		IL_0090: ceq
		IL_0092: and
		IL_0093: ldc.i4.0
		IL_0094: ceq
		IL_0096: brtrue.s IL_00c6

		IL_0098: nop
		IL_0099: ldarg.0
		IL_009a: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource CraftersScript::audioDevice
		IL_009f: stloc.s 5
		IL_00a1: ldarg.0
		IL_00a2: ldc.i4.1
		IL_00a3: stfld bool CraftersScript::angry
		IL_00a8: ldloc.s 5
		IL_00aa: ldarg.0
		IL_00ab: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip CraftersScript::aud_Intro
		IL_00b0: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_00b5: ldarg.0
		IL_00b6: ldfld class [UnityEngine.CoreModule]UnityEngine.SpriteRenderer CraftersScript::spriteImage
		IL_00bb: ldarg.0
		IL_00bc: ldfld class [UnityEngine.CoreModule]UnityEngine.Sprite CraftersScript::angrySprite
		IL_00c1: callvirt instance void [UnityEngine.CoreModule]UnityEngine.SpriteRenderer::set_sprite(class [UnityEngine.CoreModule]UnityEngine.Sprite)

		IL_00c6: nop
		IL_00c7: ldarg.0
		IL_00c8: ldfld bool CraftersScript::angry
		IL_00cd: ldc.i4.0
		IL_00ce: ceq
		IL_00d0: brtrue IL_0183

		IL_00d5: nop
		IL_00d6: ldarg.0
		IL_00d7: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent CraftersScript::agent
		IL_00dc: stloc.s 6
		IL_00de: ldloc.s 6
		IL_00e0: callvirt instance float32 [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::get_speed()
		IL_00e5: stloc.s 7
		IL_00e7: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_00ec: stloc.s 8
		IL_00ee: ldloc.s 6
		IL_00f0: ldloc.s 8
		IL_00f2: ldc.r4 60
		IL_00f7: mul
		IL_00f8: ldloc.s 7
		IL_00fa: add
		IL_00fb: callvirt instance void [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::set_speed(float32)
		IL_0100: ldarg.0
		IL_0101: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent CraftersScript::agent
		IL_0106: stloc.s 9
		IL_0108: ldarg.0
		IL_0109: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform CraftersScript::player
		IL_010e: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0113: stloc.s 10
		IL_0115: ldloc.s 9
		IL_0117: ldloca.s 11
		IL_0119: ldloca.s 10
		IL_011b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0120: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0125: ldloca.s 11
		IL_0127: ldloca.s 10
		IL_0129: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_012e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0133: ldloca.s 11
		IL_0135: ldloca.s 10
		IL_0137: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_013c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0141: ldloc.s 11
		IL_0143: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::SetDestination(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0148: stloc.s 12
		IL_014a: ldloc.s 12
		IL_014c: stloc.s 13
		IL_014e: ldarg.0
		IL_014f: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource CraftersScript::audioDevice
		IL_0154: callvirt instance bool [UnityEngine.AudioModule]UnityEngine.AudioSource::get_isPlaying()
		IL_0159: stloc.s 14
		IL_015b: ldloc.s 14
		IL_015d: stloc.s 13
		IL_015f: ldloc.s 14
		IL_0161: ldc.i4.0
		IL_0162: ceq
		IL_0164: ldc.i4.0
		IL_0165: ceq
		IL_0167: brtrue IL_02a0

		IL_016c: nop
		IL_016d: ldarg.0
		IL_016e: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource CraftersScript::audioDevice
		IL_0173: ldarg.0
		IL_0174: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip CraftersScript::aud_Loop
		IL_0179: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_017e: br IL_02a0

		IL_0183: nop
		IL_0184: ldarg.0
		IL_0185: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_018a: stloc.s 15
		IL_018c: ldloc.s 15
		IL_018e: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0193: stloc.s 16
		IL_0195: ldarg.0
		IL_0196: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent CraftersScript::agent
		IL_019b: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::get_destination()
		IL_01a0: stloc.s 17
		IL_01a2: ldloca.s 18
		IL_01a4: ldloca.s 16
		IL_01a6: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_01ab: ldloca.s 17
		IL_01ad: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_01b2: sub
		IL_01b3: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_01b8: ldloca.s 18
		IL_01ba: ldloca.s 16
		IL_01bc: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_01c1: ldloca.s 17
		IL_01c3: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_01c8: sub
		IL_01c9: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_01ce: ldloca.s 18
		IL_01d0: ldloca.s 16
		IL_01d2: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_01d7: ldloca.s 17
		IL_01d9: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_01de: sub
		IL_01df: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_01e4: ldloca.s 18
		IL_01e6: call instance float32 [UnityEngine.CoreModule]UnityEngine.Vector3::get_magnitude()
		IL_01eb: stloc.s 19
		IL_01ed: ldarg.0
		IL_01ee: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_01f3: stloc.s 20
		IL_01f5: ldloc.s 20
		IL_01f7: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_01fc: stloc.s 21
		IL_01fe: ldarg.0
		IL_01ff: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform CraftersScript::player
		IL_0204: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0209: stloc.s 22
		IL_020b: ldloca.s 23
		IL_020d: ldloca.s 21
		IL_020f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0214: ldloca.s 22
		IL_0216: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_021b: sub
		IL_021c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0221: ldloca.s 23
		IL_0223: ldloca.s 21
		IL_0225: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_022a: ldloca.s 22
		IL_022c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0231: sub
		IL_0232: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0237: ldloca.s 23
		IL_0239: ldloca.s 21
		IL_023b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0240: ldloca.s 22
		IL_0242: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0247: sub
		IL_0248: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_024d: ldloca.s 23
		IL_024f: call instance float32 [UnityEngine.CoreModule]UnityEngine.Vector3::get_magnitude()
		IL_0254: stloc.s 24
		IL_0256: ldc.r4 20
		IL_025b: ldloc.s 19
		IL_025d: clt
		IL_025f: ldc.i4.0
		IL_0260: ceq
		IL_0262: ldloc.s 24
		IL_0264: ldc.r4 60
		IL_0269: clt
		IL_026b: ldc.i4.0
		IL_026c: ceq
		IL_026e: and
		IL_026f: ldc.i4.0
		IL_0270: ceq
		IL_0272: ldc.i4.0
		IL_0273: ceq
		IL_0275: brtrue.s IL_02a2

		IL_0277: nop
		IL_0278: ldarg.0
		IL_0279: ldfld float32 CraftersScript::forceShowTime
		IL_027e: stloc.0
		IL_027f: ldarg.0
		IL_0280: ldfld float32 CraftersScript::forceShowTime
		IL_0285: ldc.r4 0.0
		IL_028a: cgt
		IL_028c: brtrue.s IL_02a2

		IL_028e: nop
		IL_028f: ldarg.0
		IL_0290: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject CraftersScript::sprite
		IL_0295: stloc.s 25

		IL_0297: nop
		IL_0298: ldloc.s 25
		IL_029a: ldc.i4.0
		IL_029b: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)

		IL_02a0: nop
		IL_02a1: ret

		IL_02a2: nop
		IL_02a3: ldarg.0
		IL_02a4: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject CraftersScript::sprite
		IL_02a9: stloc.s 25
		IL_02ab: br.s IL_0297

		IL_02ad: nop
		IL_02ae: ldnull
		IL_02af: throw
	} // end of method CraftersScript::Update

	.method private hidebysig 
		instance void FixedUpdate () cil managed 
	{
		// Method begins at RVA 0xcde8
		// Header size: 12
		// Code size: 86 (0x56)
		.maxstack 1
		.locals init (
			[0] class GameControllerScript,
			[1] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[3] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[4] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[5] float32,
			[6] float32,
			[7] float32,
			[8] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[9] class [UnityEngine.CoreModule]UnityEngine.Transform
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class GameControllerScript CraftersScript::gc
		IL_0007: stloc.0
		IL_0008: nop
		IL_0009: nop
		IL_000a: ldarg.0
		IL_000b: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform CraftersScript::player
		IL_0010: stloc.1
		IL_0011: nop
		IL_0012: ldloc.1
		IL_0013: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0018: stloc.2
		IL_0019: ldarg.0
		IL_001a: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_001f: stloc.3
		IL_0020: ldloc.3
		IL_0021: stloc.s 4
		IL_0023: ldloca.s 2
		IL_0025: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_002a: stloc.s 5
		IL_002c: ldloca.s 2
		IL_002e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0033: stloc.s 6
		IL_0035: ldloca.s 2
		IL_0037: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_003c: stloc.s 7
		IL_003e: nop
		IL_003f: ldloc.s 4
		IL_0041: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0046: stloc.s 8
		IL_0048: ldarg.0
		IL_0049: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_004e: stloc.s 9
		IL_0050: ldloc.s 9
		IL_0052: stloc.s 4
		IL_0054: nop
		IL_0055: ret
	} // end of method CraftersScript::FixedUpdate

	.method public hidebysig 
		instance void GiveLocation (
			valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 location,
			bool flee
		) cil managed 
	{
		// Method begins at RVA 0xce4c
		// Header size: 12
		// Code size: 118 (0x76)
		.maxstack 3
		.locals init (
			[0] int32,
			[1] bool,
			[2] bool,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[4] bool
		)

		IL_0000: nop
		IL_0001: ldarg.2
		IL_0002: stloc.0
		IL_0003: ldarg.0
		IL_0004: ldfld bool CraftersScript::angry
		IL_0009: ldc.i4.0
		IL_000a: ceq
		IL_000c: ldc.i4.0
		IL_000d: ceq
		IL_000f: brtrue.s IL_0071

		IL_0011: nop
		IL_0012: ldarg.0
		IL_0013: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent CraftersScript::agent
		IL_0018: callvirt instance bool [UnityEngine.CoreModule]UnityEngine.Behaviour::get_isActiveAndEnabled()
		IL_001d: stloc.1
		IL_001e: ldloc.1
		IL_001f: stloc.2
		IL_0020: ldloc.1
		IL_0021: ldc.i4.0
		IL_0022: ceq
		IL_0024: brtrue.s IL_0071

		IL_0026: nop
		IL_0027: ldarg.0
		IL_0028: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent CraftersScript::agent
		IL_002d: ldloca.s 3
		IL_002f: ldarg.1
		IL_0030: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0035: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_003a: ldloca.s 3
		IL_003c: ldarg.1
		IL_003d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0042: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0047: ldloca.s 3
		IL_0049: ldarg.1
		IL_004a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_004f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0054: ldloc.3
		IL_0055: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::SetDestination(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_005a: stloc.s 4
		IL_005c: ldloc.s 4
		IL_005e: stloc.2
		IL_005f: ldloc.0
		IL_0060: ldc.i4.0
		IL_0061: ceq
		IL_0063: brtrue.s IL_0071

		IL_0065: nop
		IL_0066: ldarg.0
		IL_0067: ldc.r4 3
		IL_006c: stfld float32 CraftersScript::forceShowTime

		IL_0071: nop
		IL_0072: ret

		IL_0073: nop
		IL_0074: ldnull
		IL_0075: throw
	} // end of method CraftersScript::GiveLocation

	.method private hidebysig 
		instance void TargetPlayer () cil managed 
	{
		// Method begins at RVA 0xced0
		// Header size: 12
		// Code size: 74 (0x4a)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[3] bool
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent CraftersScript::agent
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform CraftersScript::player
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
		IL_0046: ret

		IL_0047: nop
		IL_0048: ldnull
		IL_0049: throw
	} // end of method CraftersScript::TargetPlayer

	.method private hidebysig 
		instance void OnTriggerEnter (
			class [UnityEngine.PhysicsModule]UnityEngine.Collider other
		) cil managed 
	{
		// Method begins at RVA 0xcf28
		// Header size: 12
		// Code size: 258 (0x102)
		.maxstack 3
		.locals init (
			[0] string,
			[1] bool,
			[2] bool,
			[3] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[4] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[5] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[6] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[7] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[8] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[9] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[10] bool,
			[11] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[12] class [UnityEngine.CoreModule]UnityEngine.GameObject
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
		IL_0016: ldarg.0
		IL_0017: ldfld bool CraftersScript::angry
		IL_001c: ldloc.1
		IL_001d: and
		IL_001e: ldc.i4.0
		IL_001f: ceq
		IL_0021: brtrue IL_00fd

		IL_0026: nop
		IL_0027: ldarg.0
		IL_0028: ldfld class [UnityEngine.PhysicsModule]UnityEngine.CharacterController CraftersScript::cc
		IL_002d: ldc.i4.0
		IL_002e: callvirt instance void [UnityEngine.PhysicsModule]UnityEngine.Collider::set_enabled(bool)
		IL_0033: ldarg.0
		IL_0034: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform CraftersScript::player
		IL_0039: stloc.3
		IL_003a: ldarg.0
		IL_003b: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform CraftersScript::playerTeleportPoint
		IL_0040: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0045: stloc.s 4
		IL_0047: ldarg.0
		IL_0048: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform CraftersScript::playerTeleportPoint
		IL_004d: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0052: stloc.s 5
		IL_0054: ldloc.3
		IL_0055: ldloca.s 6
		IL_0057: ldloca.s 4
		IL_0059: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_005e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0063: ldloca.s 6
		IL_0065: ldc.r4 4
		IL_006a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_006f: ldloca.s 6
		IL_0071: ldloca.s 5
		IL_0073: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0078: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_007d: ldloc.s 6
		IL_007f: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0084: ldarg.0
		IL_0085: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent CraftersScript::baldiAgent
		IL_008a: stloc.s 7
		IL_008c: ldarg.0
		IL_008d: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform CraftersScript::baldiTeleportPoint
		IL_0092: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0097: stloc.s 8
		IL_0099: ldloc.s 7
		IL_009b: ldloca.s 9
		IL_009d: ldloca.s 8
		IL_009f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00a4: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00a9: ldloca.s 9
		IL_00ab: ldloca.s 8
		IL_00ad: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00b2: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00b7: ldloca.s 9
		IL_00b9: ldc.r4 4
		IL_00be: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00c3: ldloca.s 9
		IL_00c5: ldloca.s 8
		IL_00c7: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00cc: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00d1: ldloc.s 9
		IL_00d3: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::Warp(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_00d8: stloc.s 10
		IL_00da: ldloc.s 10
		IL_00dc: stloc.2
		IL_00dd: ldarg.0
		IL_00de: ldfld class [UnityEngine.PhysicsModule]UnityEngine.CharacterController CraftersScript::cc
		IL_00e3: ldc.i4.1
		IL_00e4: callvirt instance void [UnityEngine.PhysicsModule]UnityEngine.Collider::set_enabled(bool)
		IL_00e9: ldarg.0
		IL_00ea: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_00ef: stloc.s 11
		IL_00f1: ldloc.s 11
		IL_00f3: stloc.s 12
		IL_00f5: ldloc.s 12
		IL_00f7: ldc.i4.0
		IL_00f8: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)

		IL_00fd: nop
		IL_00fe: ret

		IL_00ff: nop
		IL_0100: ldnull
		IL_0101: throw
	} // end of method CraftersScript::OnTriggerEnter

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2519
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method CraftersScript::.ctor

} // end of class CraftersScript
