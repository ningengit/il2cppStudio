.class public auto ansi beforefieldinit SwingingDoorScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class GameControllerScript gc
	.field public class BaldiScript baldi
	.field public class [UnityEngine.PhysicsModule]UnityEngine.MeshCollider barrier
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject obstacle
	.field public class [UnityEngine.PhysicsModule]UnityEngine.MeshCollider trigger
	.field public class [UnityEngine.CoreModule]UnityEngine.MeshRenderer inside
	.field public class [UnityEngine.CoreModule]UnityEngine.MeshRenderer outside
	.field public class [UnityEngine.CoreModule]UnityEngine.Material closed
	.field public class [UnityEngine.CoreModule]UnityEngine.Material open
	.field public class [UnityEngine.CoreModule]UnityEngine.Material locked
	.field public class [UnityEngine.CoreModule]UnityEngine.Material closedOutside
	.field public class [UnityEngine.CoreModule]UnityEngine.Material openOutside
	.field public class [UnityEngine.CoreModule]UnityEngine.Material lockedOutside
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip doorOpen
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip baldiDoor
	.field private float32 openTime
	.field private float32 lockTime
	.field public bool bDoorOpen
	.field public bool bDoorLocked
	.field private bool requirementMet
	.field private class [UnityEngine.AudioModule]UnityEngine.AudioSource myAudio

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x106ac
		// Header size: 12
		// Code size: 23 (0x17)
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
		IL_000a: stfld class [UnityEngine.AudioModule]UnityEngine.AudioSource SwingingDoorScript::myAudio
		IL_000f: ldarg.0
		IL_0010: ldc.i4.1
		IL_0011: stfld bool SwingingDoorScript::bDoorLocked
		IL_0016: ret
	} // end of method SwingingDoorScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x106d0
		// Header size: 12
		// Code size: 278 (0x116)
		.maxstack 4
		.locals init (
			[0] int32,
			[1] float32,
			[2] float32,
			[3] float32,
			[4] int32,
			[5] class [UnityEngine.CoreModule]UnityEngine.MeshRenderer
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class GameControllerScript SwingingDoorScript::gc
		IL_0007: ldfld int32 GameControllerScript::notebooks
		IL_000c: ldc.i4.2
		IL_000d: clt
		IL_000f: ldc.i4.0
		IL_0010: ceq
		IL_0012: stloc.0
		IL_0013: ldarg.0
		IL_0014: ldfld bool SwingingDoorScript::requirementMet
		IL_0019: ldc.i4.0
		IL_001a: ceq
		IL_001c: ldarg.0
		IL_001d: ldfld class GameControllerScript SwingingDoorScript::gc
		IL_0022: ldfld int32 GameControllerScript::notebooks
		IL_0027: ldc.i4.2
		IL_0028: clt
		IL_002a: ldc.i4.0
		IL_002b: ceq
		IL_002d: and
		IL_002e: ldc.i4.0
		IL_002f: ceq
		IL_0031: brtrue.s IL_0041

		IL_0033: nop
		IL_0034: ldarg.0
		IL_0035: ldc.i4.1
		IL_0036: stfld bool SwingingDoorScript::requirementMet
		IL_003b: ldarg.0
		IL_003c: callvirt instance void SwingingDoorScript::UnlockDoor()

		IL_0041: nop
		IL_0042: ldarg.0
		IL_0043: ldfld float32 SwingingDoorScript::openTime
		IL_0048: stloc.1
		IL_0049: ldarg.0
		IL_004a: ldfld float32 SwingingDoorScript::openTime
		IL_004f: ldc.r4 0.0
		IL_0054: cgt
		IL_0056: ldc.i4.0
		IL_0057: ceq
		IL_0059: brtrue.s IL_006b

		IL_005b: nop
		IL_005c: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0061: stloc.2
		IL_0062: ldarg.0
		IL_0063: ldloc.1
		IL_0064: ldloc.2
		IL_0065: sub
		IL_0066: stfld float32 SwingingDoorScript::openTime

		IL_006b: nop
		IL_006c: ldarg.0
		IL_006d: ldfld float32 SwingingDoorScript::lockTime
		IL_0072: stloc.1
		IL_0073: ldarg.0
		IL_0074: ldfld float32 SwingingDoorScript::lockTime
		IL_0079: ldc.r4 0.0
		IL_007e: cgt
		IL_0080: brtrue.s IL_009e

		IL_0082: nop
		IL_0083: ldarg.0
		IL_0084: ldfld bool SwingingDoorScript::bDoorLocked
		IL_0089: ldarg.0
		IL_008a: ldfld bool SwingingDoorScript::requirementMet
		IL_008f: and
		IL_0090: ldc.i4.0
		IL_0091: ceq
		IL_0093: brtrue.s IL_00b0

		IL_0095: nop
		IL_0096: ldarg.0
		IL_0097: callvirt instance void SwingingDoorScript::UnlockDoor()
		IL_009c: br.s IL_00b0

		IL_009e: nop
		IL_009f: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_00a4: stloc.3
		IL_00a5: ldarg.0
		IL_00a6: ldloc.1
		IL_00a7: ldloc.3
		IL_00a8: sub
		IL_00a9: stfld float32 SwingingDoorScript::lockTime
		IL_00ae: ldloc.3
		IL_00af: stloc.1

		IL_00b0: nop
		IL_00b1: ldarg.0
		IL_00b2: ldfld bool SwingingDoorScript::bDoorLocked
		IL_00b7: ldc.i4.0
		IL_00b8: ceq
		IL_00ba: stloc.s 4
		IL_00bc: ldarg.0
		IL_00bd: ldfld bool SwingingDoorScript::bDoorOpen
		IL_00c2: ldc.r4 0.0
		IL_00c7: ldarg.0
		IL_00c8: ldfld float32 SwingingDoorScript::openTime
		IL_00cd: clt
		IL_00cf: ldc.i4.0
		IL_00d0: ceq
		IL_00d2: ldarg.0
		IL_00d3: ldfld bool SwingingDoorScript::bDoorLocked
		IL_00d8: ldc.i4.0
		IL_00d9: ceq
		IL_00db: and
		IL_00dc: and
		IL_00dd: ldc.i4.0
		IL_00de: ceq
		IL_00e0: brtrue.s IL_0111

		IL_00e2: nop
		IL_00e3: ldarg.0
		IL_00e4: ldfld class [UnityEngine.CoreModule]UnityEngine.MeshRenderer SwingingDoorScript::inside
		IL_00e9: stloc.s 5
		IL_00eb: ldarg.0
		IL_00ec: ldc.i4.0
		IL_00ed: stfld bool SwingingDoorScript::bDoorOpen
		IL_00f2: ldloc.s 5
		IL_00f4: ldarg.0
		IL_00f5: ldfld class [UnityEngine.CoreModule]UnityEngine.Material SwingingDoorScript::closed
		IL_00fa: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Renderer::SetMaterial(class [UnityEngine.CoreModule]UnityEngine.Material)
		IL_00ff: ldarg.0
		IL_0100: ldfld class [UnityEngine.CoreModule]UnityEngine.MeshRenderer SwingingDoorScript::outside
		IL_0105: ldarg.0
		IL_0106: ldfld class [UnityEngine.CoreModule]UnityEngine.Material SwingingDoorScript::closedOutside
		IL_010b: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Renderer::SetMaterial(class [UnityEngine.CoreModule]UnityEngine.Material)
		IL_0110: ret

		IL_0111: nop
		IL_0112: ret

		IL_0113: nop
		IL_0114: ldnull
		IL_0115: throw
	} // end of method SwingingDoorScript::Update

	.method private hidebysig 
		instance void OnTriggerStay (
			class [UnityEngine.PhysicsModule]UnityEngine.Collider other
		) cil managed 
	{
		// Method begins at RVA 0x107f4
		// Header size: 12
		// Code size: 73 (0x49)
		.maxstack 2

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld bool SwingingDoorScript::bDoorLocked
		IL_0007: ldc.i4.0
		IL_0008: ceq
		IL_000a: ldc.i4.0
		IL_000b: ceq
		IL_000d: brtrue.s IL_0044

		IL_000f: nop
		IL_0010: ldarg.0
		IL_0011: ldc.i4.1
		IL_0012: stfld bool SwingingDoorScript::bDoorOpen
		IL_0017: ldarg.0
		IL_0018: ldfld class [UnityEngine.CoreModule]UnityEngine.MeshRenderer SwingingDoorScript::inside
		IL_001d: ldarg.0
		IL_001e: ldfld class [UnityEngine.CoreModule]UnityEngine.Material SwingingDoorScript::open
		IL_0023: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Renderer::SetMaterial(class [UnityEngine.CoreModule]UnityEngine.Material)
		IL_0028: ldarg.0
		IL_0029: ldfld class [UnityEngine.CoreModule]UnityEngine.MeshRenderer SwingingDoorScript::outside
		IL_002e: ldarg.0
		IL_002f: ldfld class [UnityEngine.CoreModule]UnityEngine.Material SwingingDoorScript::openOutside
		IL_0034: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Renderer::SetMaterial(class [UnityEngine.CoreModule]UnityEngine.Material)
		IL_0039: ldarg.0
		IL_003a: ldc.r4 2
		IL_003f: stfld float32 SwingingDoorScript::openTime

		IL_0044: nop
		IL_0045: ret

		IL_0046: nop
		IL_0047: ldnull
		IL_0048: throw
	} // end of method SwingingDoorScript::OnTriggerStay

	.method private hidebysig 
		instance void OnTriggerEnter (
			class [UnityEngine.PhysicsModule]UnityEngine.Collider other
		) cil managed 
	{
		// Method begins at RVA 0x1084c
		// Header size: 12
		// Code size: 255 (0xff)
		.maxstack 3
		.locals init (
			[0] int32,
			[1] string,
			[2] bool,
			[3] int32,
			[4] int32,
			[5] int32,
			[6] string,
			[7] bool,
			[8] bool,
			[9] bool,
			[10] class BaldiScript,
			[11] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[12] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[13] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[14] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class GameControllerScript SwingingDoorScript::gc
		IL_0007: ldfld int32 GameControllerScript::notebooks
		IL_000c: stloc.0
		IL_000d: ldarg.1
		IL_000e: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Component::get_tag()
		IL_0013: stloc.1
		IL_0014: ldloc.1
		IL_0015: ldstr "Player"
		IL_001a: call bool [mscorlib]System.String::Equals(string, string)
		IL_001f: stloc.2
		IL_0020: ldloc.0
		IL_0021: ldc.i4.2
		IL_0022: clt
		IL_0024: brfalse.s IL_002a

		IL_0026: ldloc.2
		IL_0027: stloc.3
		IL_0028: br.s IL_002d

		IL_002a: nop
		IL_002b: ldc.i4.0
		IL_002c: stloc.3

		IL_002d: nop
		IL_002e: ldloc.2
		IL_002f: stloc.s 4
		IL_0031: ldloc.3
		IL_0032: stloc.s 5
		IL_0034: ldloc.3
		IL_0035: ldc.i4.0
		IL_0036: ceq
		IL_0038: ldc.i4.0
		IL_0039: ceq
		IL_003b: brtrue IL_00fa

		IL_0040: nop
		IL_0041: ldarg.0
		IL_0042: ldfld bool SwingingDoorScript::bDoorLocked
		IL_0047: ldloc.s 5
		IL_0049: ceq
		IL_004b: ldc.i4.0
		IL_004c: ceq
		IL_004e: brtrue IL_00fa

		IL_0053: nop
		IL_0054: ldarg.0
		IL_0055: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource SwingingDoorScript::myAudio
		IL_005a: ldarg.0
		IL_005b: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip SwingingDoorScript::doorOpen
		IL_0060: ldc.r4 1
		IL_0065: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip, float32)
		IL_006a: ldarg.1
		IL_006b: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Component::get_tag()
		IL_0070: stloc.s 6
		IL_0072: ldloc.s 6
		IL_0074: ldstr "Player"
		IL_0079: call bool [mscorlib]System.String::Equals(string, string)
		IL_007e: stloc.s 7
		IL_0080: ldloc.s 7
		IL_0082: stloc.s 8
		IL_0084: ldloc.s 7
		IL_0086: ldc.i4.0
		IL_0087: ceq
		IL_0089: brtrue.s IL_00fa

		IL_008b: nop
		IL_008c: ldarg.0
		IL_008d: ldfld class BaldiScript SwingingDoorScript::baldi
		IL_0092: callvirt instance bool [UnityEngine.CoreModule]UnityEngine.Behaviour::get_isActiveAndEnabled()
		IL_0097: stloc.s 9
		IL_0099: ldloc.s 9
		IL_009b: stloc.s 8
		IL_009d: ldloc.s 9
		IL_009f: ldc.i4.0
		IL_00a0: ceq
		IL_00a2: brtrue.s IL_00fa

		IL_00a4: nop
		IL_00a5: ldarg.0
		IL_00a6: ldfld class BaldiScript SwingingDoorScript::baldi
		IL_00ab: stloc.s 10
		IL_00ad: ldarg.0
		IL_00ae: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_00b3: stloc.s 11
		IL_00b5: ldloc.s 11
		IL_00b7: stloc.s 12
		IL_00b9: ldloc.s 12
		IL_00bb: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_00c0: stloc.s 13
		IL_00c2: ldloc.s 10
		IL_00c4: ldloca.s 14
		IL_00c6: ldloca.s 13
		IL_00c8: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00cd: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00d2: ldloca.s 14
		IL_00d4: ldloca.s 13
		IL_00d6: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00db: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00e0: ldloca.s 14
		IL_00e2: ldloca.s 13
		IL_00e4: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00e9: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00ee: ldloc.s 14
		IL_00f0: ldc.r4 1
		IL_00f5: callvirt instance void BaldiScript::Hear(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, float32)

		IL_00fa: nop
		IL_00fb: ret

		IL_00fc: nop
		IL_00fd: ldnull
		IL_00fe: throw
	} // end of method SwingingDoorScript::OnTriggerEnter

	.method public hidebysig 
		instance void LockDoor (
			float32 time
		) cil managed 
	{
		// Method begins at RVA 0x10958
		// Header size: 12
		// Code size: 79 (0x4f)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.MeshRenderer
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.PhysicsModule]UnityEngine.MeshCollider SwingingDoorScript::barrier
		IL_0007: ldc.i4.1
		IL_0008: callvirt instance void [UnityEngine.PhysicsModule]UnityEngine.Collider::set_enabled(bool)
		IL_000d: ldarg.0
		IL_000e: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject SwingingDoorScript::obstacle
		IL_0013: ldc.i4.1
		IL_0014: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0019: ldarg.0
		IL_001a: ldfld class [UnityEngine.CoreModule]UnityEngine.MeshRenderer SwingingDoorScript::inside
		IL_001f: stloc.0
		IL_0020: ldarg.0
		IL_0021: ldarg.1
		IL_0022: stfld float32 SwingingDoorScript::lockTime
		IL_0027: ldarg.0
		IL_0028: ldc.i4.1
		IL_0029: stfld bool SwingingDoorScript::bDoorLocked
		IL_002e: ldloc.0
		IL_002f: ldarg.0
		IL_0030: ldfld class [UnityEngine.CoreModule]UnityEngine.Material SwingingDoorScript::locked
		IL_0035: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Renderer::SetMaterial(class [UnityEngine.CoreModule]UnityEngine.Material)
		IL_003a: ldarg.0
		IL_003b: ldfld class [UnityEngine.CoreModule]UnityEngine.MeshRenderer SwingingDoorScript::outside
		IL_0040: ldarg.0
		IL_0041: ldfld class [UnityEngine.CoreModule]UnityEngine.Material SwingingDoorScript::lockedOutside
		IL_0046: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Renderer::SetMaterial(class [UnityEngine.CoreModule]UnityEngine.Material)
		IL_004b: ret

		IL_004c: nop
		IL_004d: ldnull
		IL_004e: throw
	} // end of method SwingingDoorScript::LockDoor

	.method private hidebysig 
		instance void UnlockDoor () cil managed 
	{
		// Method begins at RVA 0x109b4
		// Header size: 12
		// Code size: 72 (0x48)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.MeshRenderer
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.PhysicsModule]UnityEngine.MeshCollider SwingingDoorScript::barrier
		IL_0007: ldc.i4.0
		IL_0008: callvirt instance void [UnityEngine.PhysicsModule]UnityEngine.Collider::set_enabled(bool)
		IL_000d: ldarg.0
		IL_000e: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject SwingingDoorScript::obstacle
		IL_0013: ldc.i4.0
		IL_0014: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0019: ldarg.0
		IL_001a: ldfld class [UnityEngine.CoreModule]UnityEngine.MeshRenderer SwingingDoorScript::inside
		IL_001f: stloc.0
		IL_0020: ldarg.0
		IL_0021: ldc.i4.0
		IL_0022: stfld bool SwingingDoorScript::bDoorLocked
		IL_0027: ldloc.0
		IL_0028: ldarg.0
		IL_0029: ldfld class [UnityEngine.CoreModule]UnityEngine.Material SwingingDoorScript::closed
		IL_002e: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Renderer::SetMaterial(class [UnityEngine.CoreModule]UnityEngine.Material)
		IL_0033: ldarg.0
		IL_0034: ldfld class [UnityEngine.CoreModule]UnityEngine.MeshRenderer SwingingDoorScript::outside
		IL_0039: ldarg.0
		IL_003a: ldfld class [UnityEngine.CoreModule]UnityEngine.Material SwingingDoorScript::closedOutside
		IL_003f: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Renderer::SetMaterial(class [UnityEngine.CoreModule]UnityEngine.Material)
		IL_0044: ret

		IL_0045: nop
		IL_0046: ldnull
		IL_0047: throw
	} // end of method SwingingDoorScript::UnlockDoor

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2618
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method SwingingDoorScript::.ctor

} // end of class SwingingDoorScript
