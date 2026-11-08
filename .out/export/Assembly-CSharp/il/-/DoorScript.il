.class public auto ansi beforefieldinit DoorScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public float32 openingDistance
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform player
	.field public class BaldiScript baldi
	.field public class [UnityEngine.PhysicsModule]UnityEngine.MeshCollider barrier
	.field public class [UnityEngine.PhysicsModule]UnityEngine.MeshCollider trigger
	.field public class [UnityEngine.PhysicsModule]UnityEngine.MeshCollider invisibleBarrier
	.field public class [UnityEngine.CoreModule]UnityEngine.MeshRenderer inside
	.field public class [UnityEngine.CoreModule]UnityEngine.MeshRenderer outside
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip doorOpen
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip doorClose
	.field public class [UnityEngine.CoreModule]UnityEngine.Material closed
	.field public class [UnityEngine.CoreModule]UnityEngine.Material open
	.field public class [UnityEngine.CoreModule]UnityEngine.Material closedOutside
	.field public class [UnityEngine.CoreModule]UnityEngine.Material openOutside
	.field private bool bDoorOpen
	.field private bool bDoorLocked
	.field public int32 silentOpens
	.field private float32 openTime
	.field public float32 lockTime
	.field private class [UnityEngine.AudioModule]UnityEngine.AudioSource myAudio

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x10310
		// Header size: 12
		// Code size: 16 (0x10)
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
		IL_000a: stfld class [UnityEngine.AudioModule]UnityEngine.AudioSource DoorScript::myAudio
		IL_000f: ret
	} // end of method DoorScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x1032c
		// Header size: 12
		// Code size: 538 (0x21a)
		.maxstack 3
		.locals init (
			[0] float32,
			[1] float32,
			[2] float32,
			[3] class [UnityEngine.PhysicsModule]UnityEngine.MeshCollider,
			[4] class [UnityEngine.CoreModule]UnityEngine.MeshRenderer,
			[5] class [UnityEngine.CoreModule]UnityEngine.MeshRenderer,
			[6] class [UnityEngine.AudioModule]UnityEngine.AudioSource,
			[7] class InputManager,
			[8] class InputManager,
			[9] bool[],
			[10] bool[],
			[11] float32,
			[12] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[13] int32,
			[14] int32,
			[15] float32,
			[16] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[17] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[18] valuetype [UnityEngine.CoreModule]UnityEngine.Ray,
			[19] int64,
			[20] int64,
			[21] valuetype [UnityEngine.CoreModule]UnityEngine.Ray,
			[22] valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit,
			[23] bool,
			[24] valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit,
			[25] class [UnityEngine.PhysicsModule]UnityEngine.Collider,
			[26] class [UnityEngine.PhysicsModule]UnityEngine.MeshCollider,
			[27] class [UnityEngine.PhysicsModule]UnityEngine.Collider,
			[28] bool,
			[29] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[30] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[31] class [UnityEngine.CoreModule]UnityEngine.Transform
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld float32 DoorScript::lockTime
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld float32 DoorScript::lockTime
		IL_000e: ldc.r4 0.0
		IL_0013: cgt
		IL_0015: brtrue.s IL_002d

		IL_0017: nop
		IL_0018: ldarg.0
		IL_0019: ldfld bool DoorScript::bDoorLocked
		IL_001e: ldc.i4.0
		IL_001f: ceq
		IL_0021: brtrue.s IL_003d

		IL_0023: nop
		IL_0024: ldarg.0
		IL_0025: ldc.i4.0
		IL_0026: stfld bool DoorScript::bDoorLocked
		IL_002b: br.s IL_003d

		IL_002d: nop
		IL_002e: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0033: stloc.1
		IL_0034: ldarg.0
		IL_0035: ldloc.0
		IL_0036: ldloc.1
		IL_0037: sub
		IL_0038: stfld float32 DoorScript::lockTime

		IL_003d: nop
		IL_003e: ldarg.0
		IL_003f: ldfld float32 DoorScript::openTime
		IL_0044: stloc.0
		IL_0045: ldarg.0
		IL_0046: ldfld float32 DoorScript::openTime
		IL_004b: ldc.r4 0.0
		IL_0050: cgt
		IL_0052: ldc.i4.0
		IL_0053: ceq
		IL_0055: brtrue.s IL_0069

		IL_0057: nop
		IL_0058: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_005d: stloc.2
		IL_005e: ldarg.0
		IL_005f: ldloc.0
		IL_0060: ldloc.2
		IL_0061: sub
		IL_0062: stfld float32 DoorScript::openTime
		IL_0067: ldloc.2
		IL_0068: stloc.0

		IL_0069: nop
		IL_006a: ldarg.0
		IL_006b: ldfld bool DoorScript::bDoorOpen
		IL_0070: ldc.r4 0.0
		IL_0075: ldarg.0
		IL_0076: ldfld float32 DoorScript::openTime
		IL_007b: clt
		IL_007d: ldc.i4.0
		IL_007e: ceq
		IL_0080: and
		IL_0081: ldc.i4.0
		IL_0082: ceq
		IL_0084: brtrue.s IL_0103

		IL_0086: nop
		IL_0087: ldarg.0
		IL_0088: ldfld class [UnityEngine.PhysicsModule]UnityEngine.MeshCollider DoorScript::barrier
		IL_008d: stloc.3
		IL_008e: nop
		IL_008f: ldloc.3
		IL_0090: ldc.i4.1
		IL_0091: callvirt instance void [UnityEngine.PhysicsModule]UnityEngine.Collider::set_enabled(bool)
		IL_0096: ldarg.0
		IL_0097: ldfld class [UnityEngine.PhysicsModule]UnityEngine.MeshCollider DoorScript::invisibleBarrier
		IL_009c: stloc.3
		IL_009d: nop
		IL_009e: ldloc.3
		IL_009f: ldc.i4.1
		IL_00a0: callvirt instance void [UnityEngine.PhysicsModule]UnityEngine.Collider::set_enabled(bool)
		IL_00a5: ldarg.0
		IL_00a6: ldfld class [UnityEngine.CoreModule]UnityEngine.MeshRenderer DoorScript::inside
		IL_00ab: stloc.s 4
		IL_00ad: ldarg.0
		IL_00ae: ldc.i4.0
		IL_00af: stfld bool DoorScript::bDoorOpen
		IL_00b4: ldloc.s 4
		IL_00b6: stloc.s 5
		IL_00b8: nop
		IL_00b9: ldloc.s 5
		IL_00bb: ldarg.0
		IL_00bc: ldfld class [UnityEngine.CoreModule]UnityEngine.Material DoorScript::closed
		IL_00c1: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Renderer::SetMaterial(class [UnityEngine.CoreModule]UnityEngine.Material)
		IL_00c6: ldarg.0
		IL_00c7: ldfld class [UnityEngine.CoreModule]UnityEngine.MeshRenderer DoorScript::outside
		IL_00cc: stloc.s 5
		IL_00ce: nop
		IL_00cf: ldloc.s 5
		IL_00d1: ldarg.0
		IL_00d2: ldfld class [UnityEngine.CoreModule]UnityEngine.Material DoorScript::closedOutside
		IL_00d7: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Renderer::SetMaterial(class [UnityEngine.CoreModule]UnityEngine.Material)
		IL_00dc: ldarg.0
		IL_00dd: ldfld int32 DoorScript::silentOpens
		IL_00e2: ldc.i4.0
		IL_00e3: cgt
		IL_00e5: brtrue.s IL_0103

		IL_00e7: nop
		IL_00e8: ldarg.0
		IL_00e9: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource DoorScript::myAudio
		IL_00ee: stloc.s 6
		IL_00f0: nop
		IL_00f1: ldloc.s 6
		IL_00f3: ldarg.0
		IL_00f4: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip DoorScript::doorClose
		IL_00f9: ldc.r4 1
		IL_00fe: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip, float32)

		IL_0103: nop
		IL_0104: call !0 class Singleton`1<class InputManager>::get_Instance()
		IL_0109: stloc.s 7
		IL_010b: ldloc.s 7
		IL_010d: stloc.s 8
		IL_010f: nop
		IL_0110: ldloc.s 8
		IL_0112: ldfld bool[] InputManager::currentKeyStates
		IL_0117: stloc.s 9
		IL_0119: nop
		IL_011a: nop
		IL_011b: nop
		IL_011c: ldloc.s 8
		IL_011e: ldfld bool[] InputManager::previousKeyStates
		IL_0123: stloc.s 10
		IL_0125: nop
		IL_0126: nop
		IL_0127: nop
		IL_0128: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_timeScale()
		IL_012d: stloc.s 11
		IL_012f: ldloc.s 11
		IL_0131: stloc.0
		IL_0132: nop
		IL_0133: nop
		IL_0134: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_0139: stloc.s 12
		IL_013b: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_width()
		IL_0140: stloc.s 13
		IL_0142: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_height()
		IL_0147: stloc.s 14
		IL_0149: ldloc.s 13
		IL_014b: ldstr "rdx"
		IL_0150: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0155: conv.i4
		IL_0156: sub
		IL_0157: ldc.i4.1
		IL_0158: shr
		IL_0159: conv.r4
		IL_015a: stloc.s 15
		IL_015c: ldloc.s 14
		IL_015e: ldstr "rdx"
		IL_0163: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0168: conv.i4
		IL_0169: sub
		IL_016a: ldc.i4.1
		IL_016b: shr
		IL_016c: conv.r4
		IL_016d: stloc.0
		IL_016e: ldloc.s 12
		IL_0170: stloc.s 16
		IL_0172: nop
		IL_0173: ldloc.s 16
		IL_0175: ldloca.s 17
		IL_0177: ldloc.s 15
		IL_0179: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_017e: ldloca.s 17
		IL_0180: ldloc.0
		IL_0181: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0186: ldloca.s 17
		IL_0188: ldc.r4 0.0
		IL_018d: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0192: ldloc.s 17
		IL_0194: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Ray [UnityEngine.CoreModule]UnityEngine.Camera::ScreenPointToRay(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0199: stloc.s 18
		IL_019b: ldstr "struct_chunk(LocalExpr+0x0, 16)"
		IL_01a0: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01a5: stloc.s 19
		IL_01a7: ldstr "struct_chunk(LocalExpr+0x10, 8)"
		IL_01ac: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01b1: stloc.s 20
		IL_01b3: br.s IL_01b6

		IL_01b5: nop

		IL_01b6: nop
		IL_01b7: ldloca.s 21
		IL_01b9: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Ray::m_Origin
		IL_01be: ldloc.s 19
		IL_01c0: box [mscorlib]System.Int64
		IL_01c5: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_01ca: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_01cf: ldloc.s 21
		IL_01d1: ldloca.s 22
		IL_01d3: call bool [UnityEngine.PhysicsModule]UnityEngine.Physics::Raycast(valuetype [UnityEngine.CoreModule]UnityEngine.Ray, valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit&)
		IL_01d8: stloc.s 23
		IL_01da: nop
		IL_01db: ldloca.s 24
		IL_01dd: call instance class [UnityEngine.PhysicsModule]UnityEngine.Collider [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_collider()
		IL_01e2: stloc.s 25
		IL_01e4: ldarg.0
		IL_01e5: ldfld class [UnityEngine.PhysicsModule]UnityEngine.MeshCollider DoorScript::trigger
		IL_01ea: stloc.s 26
		IL_01ec: ldloc.s 25
		IL_01ee: stloc.s 27
		IL_01f0: br.s IL_01f3

		IL_01f2: nop

		IL_01f3: nop
		IL_01f4: ldloc.s 27
		IL_01f6: ldloc.s 26
		IL_01f8: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Equality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_01fd: stloc.s 28
		IL_01ff: ldarg.0
		IL_0200: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform DoorScript::player
		IL_0205: stloc.s 29
		IL_0207: nop
		IL_0208: ldloc.s 29
		IL_020a: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_020f: stloc.s 30
		IL_0211: ldarg.0
		IL_0212: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0217: stloc.s 31
		IL_0219: ret
	} // end of method DoorScript::Update

	.method public hidebysig 
		instance void OpenDoor () cil managed 
	{
		// Method begins at RVA 0x10554
		// Header size: 12
		// Code size: 133 (0x85)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.MeshRenderer
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld int32 DoorScript::silentOpens
		IL_0007: ldc.i4.0
		IL_0008: cgt
		IL_000a: brtrue.s IL_0032

		IL_000c: nop
		IL_000d: ldarg.0
		IL_000e: ldfld bool DoorScript::bDoorOpen
		IL_0013: ldc.i4.0
		IL_0014: ceq
		IL_0016: ldc.i4.0
		IL_0017: ceq
		IL_0019: brtrue.s IL_0032

		IL_001b: nop
		IL_001c: ldarg.0
		IL_001d: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource DoorScript::myAudio
		IL_0022: ldarg.0
		IL_0023: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip DoorScript::doorOpen
		IL_0028: ldc.r4 1
		IL_002d: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip, float32)

		IL_0032: nop
		IL_0033: ldarg.0
		IL_0034: ldfld class [UnityEngine.PhysicsModule]UnityEngine.MeshCollider DoorScript::barrier
		IL_0039: ldc.i4.0
		IL_003a: callvirt instance void [UnityEngine.PhysicsModule]UnityEngine.Collider::set_enabled(bool)
		IL_003f: ldarg.0
		IL_0040: ldfld class [UnityEngine.PhysicsModule]UnityEngine.MeshCollider DoorScript::invisibleBarrier
		IL_0045: ldc.i4.0
		IL_0046: callvirt instance void [UnityEngine.PhysicsModule]UnityEngine.Collider::set_enabled(bool)
		IL_004b: ldarg.0
		IL_004c: ldfld class [UnityEngine.CoreModule]UnityEngine.MeshRenderer DoorScript::inside
		IL_0051: stloc.0
		IL_0052: ldarg.0
		IL_0053: ldc.i4.1
		IL_0054: stfld bool DoorScript::bDoorOpen
		IL_0059: ldloc.0
		IL_005a: ldarg.0
		IL_005b: ldfld class [UnityEngine.CoreModule]UnityEngine.Material DoorScript::open
		IL_0060: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Renderer::SetMaterial(class [UnityEngine.CoreModule]UnityEngine.Material)
		IL_0065: ldarg.0
		IL_0066: ldfld class [UnityEngine.CoreModule]UnityEngine.MeshRenderer DoorScript::outside
		IL_006b: ldarg.0
		IL_006c: ldfld class [UnityEngine.CoreModule]UnityEngine.Material DoorScript::openOutside
		IL_0071: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Renderer::SetMaterial(class [UnityEngine.CoreModule]UnityEngine.Material)
		IL_0076: ldarg.0
		IL_0077: ldc.r4 3
		IL_007c: stfld float32 DoorScript::openTime
		IL_0081: ret

		IL_0082: nop
		IL_0083: ldnull
		IL_0084: throw
	} // end of method DoorScript::OpenDoor

	.method private hidebysig 
		instance void OnTriggerStay (
			class [UnityEngine.PhysicsModule]UnityEngine.Collider other
		) cil managed 
	{
		// Method begins at RVA 0x105e8
		// Header size: 12
		// Code size: 54 (0x36)
		.maxstack 2
		.locals init (
			[0] int32,
			[1] bool,
			[2] int32,
			[3] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld bool DoorScript::bDoorLocked
		IL_0007: stloc.0
		IL_0008: ldarg.1
		IL_0009: ldstr "NPC"
		IL_000e: callvirt instance bool [UnityEngine.CoreModule]UnityEngine.Component::CompareTag(string)
		IL_0013: stloc.1
		IL_0014: ldloc.0
		IL_0015: ldc.i4.0
		IL_0016: ceq
		IL_0018: brfalse.s IL_001e

		IL_001a: ldloc.1
		IL_001b: stloc.2
		IL_001c: br.s IL_0021

		IL_001e: nop
		IL_001f: ldc.i4.0
		IL_0020: stloc.2

		IL_0021: nop
		IL_0022: ldloc.1
		IL_0023: stloc.3
		IL_0024: ldloc.2
		IL_0025: ldc.i4.0
		IL_0026: ceq
		IL_0028: brtrue.s IL_0031

		IL_002a: nop
		IL_002b: ldarg.0
		IL_002c: callvirt instance void DoorScript::OpenDoor()

		IL_0031: nop
		IL_0032: ret

		IL_0033: nop
		IL_0034: ldnull
		IL_0035: throw
	} // end of method DoorScript::OnTriggerStay

	.method public hidebysig 
		instance void LockDoor (
			float32 time
		) cil managed 
	{
		// Method begins at RVA 0x25d8
		// Header size: 1
		// Code size: 16 (0x10)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldarg.1
		IL_0003: stfld float32 DoorScript::lockTime
		IL_0008: ldarg.0
		IL_0009: ldc.i4.1
		IL_000a: stfld bool DoorScript::bDoorLocked
		IL_000f: ret
	} // end of method DoorScript::LockDoor

	.method public hidebysig 
		instance void UnlockDoor () cil managed 
	{
		// Method begins at RVA 0x25e9
		// Header size: 1
		// Code size: 9 (0x9)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i4.0
		IL_0003: stfld bool DoorScript::bDoorLocked
		IL_0008: ret
	} // end of method DoorScript::UnlockDoor

	.method public hidebysig specialname 
		instance bool get_DoorLocked () cil managed 
	{
		// Method begins at RVA 0x25f3
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld bool DoorScript::bDoorLocked
		IL_0007: ret
	} // end of method DoorScript::get_DoorLocked

	.method public hidebysig 
		instance void SilenceDoor () cil managed 
	{
		// Method begins at RVA 0x25fc
		// Header size: 1
		// Code size: 9 (0x9)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i4.4
		IL_0003: stfld int32 DoorScript::silentOpens
		IL_0008: ret
	} // end of method DoorScript::SilenceDoor

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2606
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method DoorScript::.ctor

	// Properties
	.property instance bool DoorLocked()
	{
		.get instance bool DoorScript::get_DoorLocked()
	}

} // end of class DoorScript
