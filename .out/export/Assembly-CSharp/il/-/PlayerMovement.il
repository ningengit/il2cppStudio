.class public auto ansi beforefieldinit PlayerMovement
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class [UnityEngine.PhysicsModule]UnityEngine.CharacterController cc
	.field public float32 walkSpeed
	.field public float32 runSpeed
	.field public float32 stamina
	.field public float32 staminaDrop
	.field public float32 staminaRise
	.field public float32 staminaMax
	.field private float32 sensitivity
	.field private float32 mouseSensitivity
	.field private bool running

	// Methods
	.method private hidebysig 
		instance void Awake () cil managed 
	{
		// Method begins at RVA 0x124c8
		// Header size: 12
		// Code size: 20 (0x14)
		.maxstack 2
		.locals init (
			[0] float32
		)

		IL_0000: nop
		IL_0001: ldstr "MouseSensitivity"
		IL_0006: call float32 [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::GetFloat(string)
		IL_000b: stloc.0
		IL_000c: ldarg.0
		IL_000d: ldloc.0
		IL_000e: stfld float32 PlayerMovement::mouseSensitivity
		IL_0013: ret
	} // end of method PlayerMovement::Awake

	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x26a8
		// Header size: 1
		// Code size: 24 (0x18)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldarg.0
		IL_0003: ldfld float32 PlayerMovement::staminaMax
		IL_0008: stfld float32 PlayerMovement::stamina
		IL_000d: ldc.r4 1
		IL_0012: call void [UnityEngine.CoreModule]UnityEngine.Time::set_timeScale(float32)
		IL_0017: ret
	} // end of method PlayerMovement::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x124e8
		// Header size: 12
		// Code size: 559 (0x22f)
		.maxstack 4
		.locals init (
			[0] bool,
			[1] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[2] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[4] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[5] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[6] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[7] float32,
			[8] float32,
			[9] float32,
			[10] float32,
			[11] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[12] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[13] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[14] class [UnityEngine.PhysicsModule]UnityEngine.CharacterController,
			[15] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[16] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[17] float32,
			[18] float32,
			[19] float32,
			[20] float32,
			[21] float32,
			[22] float32,
			[23] float32,
			[24] float32,
			[25] float32
		)

		IL_0000: nop
		IL_0001: ldstr "Run"
		IL_0006: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetButton(string)
		IL_000b: stloc.0
		IL_000c: ldarg.0
		IL_000d: ldloc.0
		IL_000e: stfld bool PlayerMovement::running
		IL_0013: ldarg.0
		IL_0014: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0019: stloc.1
		IL_001a: ldloc.1
		IL_001b: stloc.2
		IL_001c: ldloc.2
		IL_001d: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Transform::get_rotation()
		IL_0022: stloc.3
		IL_0023: ldloc.3
		IL_0024: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_ToEulerRad(valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion)
		IL_0029: stloc.s 4
		IL_002b: ldloca.s 5
		IL_002d: ldloca.s 4
		IL_002f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0034: ldc.r4 57.29578
		IL_0039: mul
		IL_003a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_003f: ldloca.s 5
		IL_0041: ldloca.s 4
		IL_0043: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0048: ldc.r4 57.29578
		IL_004d: mul
		IL_004e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0053: ldloca.s 5
		IL_0055: ldloca.s 4
		IL_0057: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_005c: ldc.r4 57.29578
		IL_0061: mul
		IL_0062: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0067: ldloc.s 5
		IL_0069: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_MakePositive(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_006e: stloc.s 6
		IL_0070: ldstr "Mouse X"
		IL_0075: call float32 [UnityEngine.InputLegacyModule]UnityEngine.Input::GetAxis(string)
		IL_007a: stloc.s 7
		IL_007c: ldarg.0
		IL_007d: ldfld float32 PlayerMovement::mouseSensitivity
		IL_0082: stloc.s 8
		IL_0084: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0089: stloc.s 9
		IL_008b: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_timeScale()
		IL_0090: stloc.s 10
		IL_0092: ldloca.s 11
		IL_0094: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_0099: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_009e: conv.r4
		IL_009f: ldc.r4 0.0
		IL_00a4: add
		IL_00a5: ldc.r4 0.017453292
		IL_00aa: mul
		IL_00ab: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00b0: ldloca.s 11
		IL_00b2: ldloc.s 10
		IL_00b4: ldloc.s 8
		IL_00b6: ldloc.s 7
		IL_00b8: mul
		IL_00b9: ldloc.s 9
		IL_00bb: mul
		IL_00bc: mul
		IL_00bd: ldstr "stack[0xFFFFFFFFFFFFFF7C]"
		IL_00c2: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00c7: conv.i4
		IL_00c8: conv.r4
		IL_00c9: add
		IL_00ca: ldc.r4 0.017453292
		IL_00cf: mul
		IL_00d0: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00d5: ldloca.s 11
		IL_00d7: ldloca.s 6
		IL_00d9: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00de: ldc.r4 0.0
		IL_00e3: add
		IL_00e4: ldc.r4 0.017453292
		IL_00e9: mul
		IL_00ea: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00ef: ldloc.s 11
		IL_00f1: call valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_FromEulerRad(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_00f6: stloc.s 12
		IL_00f8: ldarg.0
		IL_00f9: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_00fe: stloc.s 13
		IL_0100: ldloc.s 13
		IL_0102: stloc.2
		IL_0103: ldloc.2
		IL_0104: ldloc.s 12
		IL_0106: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_rotation(valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion)
		IL_010b: ldarg.0
		IL_010c: callvirt instance void PlayerMovement::PlayerMove()
		IL_0111: ldarg.0
		IL_0112: ldfld class [UnityEngine.PhysicsModule]UnityEngine.CharacterController PlayerMovement::cc
		IL_0117: stloc.s 14
		IL_0119: ldloc.s 14
		IL_011b: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.PhysicsModule]UnityEngine.CharacterController::get_velocity()
		IL_0120: stloc.s 15
		IL_0122: ldloca.s 16
		IL_0124: ldloca.s 15
		IL_0126: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_012b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0130: ldloca.s 16
		IL_0132: ldloca.s 15
		IL_0134: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0139: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_013e: ldloca.s 16
		IL_0140: ldloc.s 10
		IL_0142: ldloc.s 8
		IL_0144: ldloc.s 7
		IL_0146: mul
		IL_0147: ldloc.s 9
		IL_0149: mul
		IL_014a: mul
		IL_014b: ldstr "stack[0xFFFFFFFFFFFFFF7C]"
		IL_0150: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0155: conv.i4
		IL_0156: conv.r4
		IL_0157: add
		IL_0158: ldc.r4 0.017453292
		IL_015d: mul
		IL_015e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0163: ldloca.s 16
		IL_0165: ldloca.s 15
		IL_0167: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_016c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0171: ldloca.s 16
		IL_0173: call instance float32 [UnityEngine.CoreModule]UnityEngine.Vector3::get_magnitude()
		IL_0178: stloc.s 17
		IL_017a: ldloc.s 17
		IL_017c: stloc.s 18
		IL_017e: ldarg.0
		IL_017f: ldfld class [UnityEngine.PhysicsModule]UnityEngine.CharacterController PlayerMovement::cc
		IL_0184: callvirt instance float32 [UnityEngine.PhysicsModule]UnityEngine.CharacterController::get_minMoveDistance()
		IL_0189: stloc.s 19
		IL_018b: ldloc.s 19
		IL_018d: stloc.s 18
		IL_018f: ldloc.s 17
		IL_0191: ldloc.s 19
		IL_0193: cgt
		IL_0195: brtrue.s IL_01e5

		IL_0197: nop
		IL_0198: ldarg.0
		IL_0199: ldfld float32 PlayerMovement::staminaMax
		IL_019e: stloc.s 18
		IL_01a0: ldarg.0
		IL_01a1: ldfld float32 PlayerMovement::staminaMax
		IL_01a6: ldarg.0
		IL_01a7: ldfld float32 PlayerMovement::stamina
		IL_01ac: cgt
		IL_01ae: ldc.i4.0
		IL_01af: ceq
		IL_01b1: brtrue.s IL_01e3

		IL_01b3: nop
		IL_01b4: ldarg.0
		IL_01b5: ldfld float32 PlayerMovement::stamina
		IL_01ba: stloc.s 20
		IL_01bc: ldarg.0
		IL_01bd: ldfld float32 PlayerMovement::staminaRise
		IL_01c2: stloc.s 21
		IL_01c4: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_01c9: stloc.s 22
		IL_01cb: ldarg.0
		IL_01cc: ldloc.s 22
		IL_01ce: ldloc.s 21
		IL_01d0: mul
		IL_01d1: ldloc.s 20
		IL_01d3: add
		IL_01d4: stfld float32 PlayerMovement::stamina
		IL_01d9: ldloc.s 22
		IL_01db: ldloc.s 21
		IL_01dd: mul
		IL_01de: ldloc.s 20
		IL_01e0: add
		IL_01e1: stloc.s 18

		IL_01e3: nop
		IL_01e4: ret

		IL_01e5: nop
		IL_01e6: ldarg.0
		IL_01e7: ldfld bool PlayerMovement::running
		IL_01ec: ldc.i4.0
		IL_01ed: ceq
		IL_01ef: brtrue.s IL_01e3

		IL_01f1: nop
		IL_01f2: ldarg.0
		IL_01f3: ldfld float32 PlayerMovement::stamina
		IL_01f8: stloc.s 23
		IL_01fa: ldarg.0
		IL_01fb: ldfld float32 PlayerMovement::staminaDrop
		IL_0200: stloc.s 24
		IL_0202: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0207: stloc.s 25
		IL_0209: ldarg.0
		IL_020a: ldstr "maxss xmm7,xmm11"
		IL_020f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0214: box [mscorlib]System.Int64
		IL_0219: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_021e: stfld float32 PlayerMovement::stamina
		IL_0223: ldloc.s 25
		IL_0225: ldloc.s 24
		IL_0227: mul
		IL_0228: stloc.s 18
		IL_022a: br.s IL_01e3

		IL_022c: nop
		IL_022d: ldnull
		IL_022e: throw
	} // end of method PlayerMovement::Update

	.method private hidebysig 
		instance void MouseMove () cil managed 
	{
		// Method begins at RVA 0x12724
		// Header size: 12
		// Code size: 247 (0xf7)
		.maxstack 4
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[4] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[5] float32,
			[6] float32,
			[7] float32,
			[8] float32,
			[9] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[10] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[11] class [UnityEngine.CoreModule]UnityEngine.Transform
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Transform::get_rotation()
		IL_000e: stloc.1
		IL_000f: ldloc.1
		IL_0010: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_ToEulerRad(valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion)
		IL_0015: stloc.2
		IL_0016: ldloca.s 3
		IL_0018: ldloca.s 2
		IL_001a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_001f: ldc.r4 57.29578
		IL_0024: mul
		IL_0025: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_002a: ldloca.s 3
		IL_002c: ldloca.s 2
		IL_002e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0033: ldc.r4 57.29578
		IL_0038: mul
		IL_0039: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_003e: ldloca.s 3
		IL_0040: ldloca.s 2
		IL_0042: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0047: ldc.r4 57.29578
		IL_004c: mul
		IL_004d: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0052: ldloc.3
		IL_0053: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_MakePositive(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0058: stloc.s 4
		IL_005a: ldstr "Mouse X"
		IL_005f: call float32 [UnityEngine.InputLegacyModule]UnityEngine.Input::GetAxis(string)
		IL_0064: stloc.s 5
		IL_0066: ldarg.0
		IL_0067: ldfld float32 PlayerMovement::mouseSensitivity
		IL_006c: stloc.s 6
		IL_006e: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0073: stloc.s 7
		IL_0075: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_timeScale()
		IL_007a: stloc.s 8
		IL_007c: ldloca.s 9
		IL_007e: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_0083: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0088: conv.r4
		IL_0089: ldc.r4 0.0
		IL_008e: add
		IL_008f: ldc.r4 0.017453292
		IL_0094: mul
		IL_0095: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_009a: ldloca.s 9
		IL_009c: ldloc.s 8
		IL_009e: ldloc.s 6
		IL_00a0: ldloc.s 5
		IL_00a2: mul
		IL_00a3: ldloc.s 7
		IL_00a5: mul
		IL_00a6: mul
		IL_00a7: ldstr "stack[0xFFFFFFFFFFFFFF8C]"
		IL_00ac: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00b1: conv.i4
		IL_00b2: conv.r4
		IL_00b3: add
		IL_00b4: ldc.r4 0.017453292
		IL_00b9: mul
		IL_00ba: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00bf: ldloca.s 9
		IL_00c1: ldloca.s 4
		IL_00c3: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00c8: ldc.r4 0.0
		IL_00cd: add
		IL_00ce: ldc.r4 0.017453292
		IL_00d3: mul
		IL_00d4: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00d9: ldloc.s 9
		IL_00db: call valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_FromEulerRad(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_00e0: stloc.s 10
		IL_00e2: ldarg.0
		IL_00e3: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_00e8: stloc.s 11
		IL_00ea: ldloc.s 11
		IL_00ec: ldloc.s 10
		IL_00ee: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_rotation(valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion)
		IL_00f3: ret

		IL_00f4: nop
		IL_00f5: ldnull
		IL_00f6: throw
	} // end of method PlayerMovement::MouseMove

	.method private hidebysig 
		instance void PlayerMove () cil managed 
	{
		// Method begins at RVA 0x12828
		// Header size: 12
		// Code size: 330 (0x14a)
		.maxstack 4
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] int32,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[3] float32,
			[4] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[5] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[6] float32,
			[7] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[8] float32,
			[9] float32,
			[10] float32,
			[11] class [UnityEngine.PhysicsModule]UnityEngine.CharacterController,
			[12] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[13] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[14] float32,
			[15] float32,
			[16] float32,
			[17] float32,
			[18] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[19] valuetype [UnityEngine.PhysicsModule]UnityEngine.CollisionFlags
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0007: stloc.0
		IL_0008: ldstr "*(A_0 + 0x28)"
		IL_000d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0012: conv.i4
		IL_0013: stloc.1
		IL_0014: ldloc.0
		IL_0015: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_right()
		IL_001a: stloc.2
		IL_001b: ldstr "Strafe"
		IL_0020: call float32 [UnityEngine.InputLegacyModule]UnityEngine.Input::GetAxis(string)
		IL_0025: stloc.3
		IL_0026: ldarg.0
		IL_0027: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_002c: stloc.s 4
		IL_002e: ldloc.s 4
		IL_0030: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_forward()
		IL_0035: stloc.s 5
		IL_0037: ldstr "Forward"
		IL_003c: call float32 [UnityEngine.InputLegacyModule]UnityEngine.Input::GetAxis(string)
		IL_0041: stloc.s 6
		IL_0043: ldloca.s 7
		IL_0045: ldloca.s 5
		IL_0047: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_004c: ldloc.s 6
		IL_004e: mul
		IL_004f: ldloca.s 2
		IL_0051: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0056: ldloc.3
		IL_0057: mul
		IL_0058: add
		IL_0059: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_005e: ldloca.s 7
		IL_0060: ldloca.s 5
		IL_0062: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0067: ldloc.s 6
		IL_0069: mul
		IL_006a: ldloca.s 2
		IL_006c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0071: ldloc.3
		IL_0072: mul
		IL_0073: add
		IL_0074: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0079: ldloca.s 7
		IL_007b: ldloca.s 5
		IL_007d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0082: ldloc.s 6
		IL_0084: mul
		IL_0085: ldloca.s 2
		IL_0087: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_008c: ldloc.3
		IL_008d: mul
		IL_008e: add
		IL_008f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0094: ldloca.s 7
		IL_0096: call instance float32 [UnityEngine.CoreModule]UnityEngine.Vector3::get_magnitude()
		IL_009b: stloc.s 8
		IL_009d: ldloc.s 8
		IL_009f: stloc.s 9
		IL_00a1: ldc.r4 0.0
		IL_00a6: ldloc.s 8
		IL_00a8: cgt
		IL_00aa: brtrue.s IL_00bd

		IL_00ac: nop
		IL_00ad: ldloc.s 9
		IL_00af: ldc.r4 1
		IL_00b4: cgt
		IL_00b6: brtrue.s IL_00bd

		IL_00b8: nop
		IL_00b9: ldloc.s 9
		IL_00bb: stloc.s 10

		IL_00bd: nop
		IL_00be: ldarg.0
		IL_00bf: ldfld class [UnityEngine.PhysicsModule]UnityEngine.CharacterController PlayerMovement::cc
		IL_00c4: stloc.s 11
		IL_00c6: ldarg.0
		IL_00c7: ldloc.s 10
		IL_00c9: stfld float32 PlayerMovement::sensitivity
		IL_00ce: ldloca.s 12
		IL_00d0: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::get_normalized()
		IL_00d5: stloc.s 13
		IL_00d7: ldloc.1
		IL_00d8: conv.r4
		IL_00d9: ldloca.s 13
		IL_00db: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00e0: mul
		IL_00e1: ldarg.0
		IL_00e2: ldfld float32 PlayerMovement::sensitivity
		IL_00e7: mul
		IL_00e8: stloc.s 14
		IL_00ea: ldloc.1
		IL_00eb: conv.r4
		IL_00ec: ldloca.s 13
		IL_00ee: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00f3: mul
		IL_00f4: ldarg.0
		IL_00f5: ldfld float32 PlayerMovement::sensitivity
		IL_00fa: mul
		IL_00fb: stloc.s 15
		IL_00fd: ldloc.1
		IL_00fe: conv.r4
		IL_00ff: ldloca.s 13
		IL_0101: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0106: mul
		IL_0107: ldarg.0
		IL_0108: ldfld float32 PlayerMovement::sensitivity
		IL_010d: mul
		IL_010e: stloc.s 16
		IL_0110: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0115: stloc.s 17
		IL_0117: ldloc.s 11
		IL_0119: ldloca.s 18
		IL_011b: ldloc.s 15
		IL_011d: ldloc.s 17
		IL_011f: mul
		IL_0120: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0125: ldloca.s 18
		IL_0127: ldloc.s 16
		IL_0129: ldloc.s 17
		IL_012b: mul
		IL_012c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0131: ldloca.s 18
		IL_0133: ldloc.s 14
		IL_0135: ldloc.s 17
		IL_0137: mul
		IL_0138: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_013d: ldloc.s 18
		IL_013f: callvirt instance valuetype [UnityEngine.PhysicsModule]UnityEngine.CollisionFlags [UnityEngine.PhysicsModule]UnityEngine.CharacterController::Move(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0144: stloc.s 19
		IL_0146: ret

		IL_0147: nop
		IL_0148: ldnull
		IL_0149: throw
	} // end of method PlayerMovement::PlayerMove

	.method public hidebysig 
		instance void StaminaUpdate () cil managed 
	{
		// Method begins at RVA 0x12980
		// Header size: 12
		// Code size: 236 (0xec)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.PhysicsModule]UnityEngine.CharacterController,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[3] float32,
			[4] float32,
			[5] float32,
			[6] float32,
			[7] float32,
			[8] float32,
			[9] float32,
			[10] float32,
			[11] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.PhysicsModule]UnityEngine.CharacterController PlayerMovement::cc
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.PhysicsModule]UnityEngine.CharacterController::get_velocity()
		IL_000e: stloc.1
		IL_000f: ldloca.s 2
		IL_0011: ldloca.s 1
		IL_0013: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0018: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_001d: ldloca.s 2
		IL_001f: ldloca.s 1
		IL_0021: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0026: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_002b: ldloca.s 2
		IL_002d: ldloca.s 1
		IL_002f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0034: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0039: ldloca.s 2
		IL_003b: call instance float32 [UnityEngine.CoreModule]UnityEngine.Vector3::get_magnitude()
		IL_0040: stloc.3
		IL_0041: ldloc.3
		IL_0042: stloc.s 4
		IL_0044: ldarg.0
		IL_0045: ldfld class [UnityEngine.PhysicsModule]UnityEngine.CharacterController PlayerMovement::cc
		IL_004a: callvirt instance float32 [UnityEngine.PhysicsModule]UnityEngine.CharacterController::get_minMoveDistance()
		IL_004f: stloc.s 5
		IL_0051: ldloc.s 5
		IL_0053: stloc.s 4
		IL_0055: ldloc.3
		IL_0056: ldloc.s 5
		IL_0058: cgt
		IL_005a: brtrue.s IL_00aa

		IL_005c: nop
		IL_005d: ldarg.0
		IL_005e: ldfld float32 PlayerMovement::staminaMax
		IL_0063: stloc.s 4
		IL_0065: ldarg.0
		IL_0066: ldfld float32 PlayerMovement::staminaMax
		IL_006b: ldarg.0
		IL_006c: ldfld float32 PlayerMovement::stamina
		IL_0071: cgt
		IL_0073: ldc.i4.0
		IL_0074: ceq
		IL_0076: brtrue.s IL_00a8

		IL_0078: nop
		IL_0079: ldarg.0
		IL_007a: ldfld float32 PlayerMovement::stamina
		IL_007f: stloc.s 6
		IL_0081: ldarg.0
		IL_0082: ldfld float32 PlayerMovement::staminaRise
		IL_0087: stloc.s 7
		IL_0089: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_008e: stloc.s 8
		IL_0090: ldarg.0
		IL_0091: ldloc.s 8
		IL_0093: ldloc.s 7
		IL_0095: mul
		IL_0096: ldloc.s 6
		IL_0098: add
		IL_0099: stfld float32 PlayerMovement::stamina
		IL_009e: ldloc.s 8
		IL_00a0: ldloc.s 7
		IL_00a2: mul
		IL_00a3: ldloc.s 6
		IL_00a5: add
		IL_00a6: stloc.s 4

		IL_00a8: nop
		IL_00a9: ret

		IL_00aa: nop
		IL_00ab: ldarg.0
		IL_00ac: ldfld bool PlayerMovement::running
		IL_00b1: ldc.i4.0
		IL_00b2: ceq
		IL_00b4: brtrue.s IL_00a8

		IL_00b6: nop
		IL_00b7: ldarg.0
		IL_00b8: ldfld float32 PlayerMovement::stamina
		IL_00bd: stloc.s 9
		IL_00bf: ldarg.0
		IL_00c0: ldfld float32 PlayerMovement::staminaDrop
		IL_00c5: stloc.s 10
		IL_00c7: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_00cc: stloc.s 11
		IL_00ce: ldarg.0
		IL_00cf: ldstr "maxss xmm7,xmm0"
		IL_00d4: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00d9: box [mscorlib]System.Int64
		IL_00de: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_00e3: stfld float32 PlayerMovement::stamina
		IL_00e8: ret

		IL_00e9: nop
		IL_00ea: ldnull
		IL_00eb: throw
	} // end of method PlayerMovement::StaminaUpdate

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x26c1
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method PlayerMovement::.ctor

} // end of class PlayerMovement
