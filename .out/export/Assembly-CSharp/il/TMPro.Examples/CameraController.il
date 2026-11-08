.class public auto ansi beforefieldinit TMPro.Examples.CameraController
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested public auto ansi sealed CameraModes
		extends [mscorlib]System.Enum
	{
		// Fields
		.field public specialname rtspecialname int32 value__
		.field public static literal valuetype TMPro.Examples.CameraController/CameraModes Follow
		.field public static literal valuetype TMPro.Examples.CameraController/CameraModes Isometric
		.field public static literal valuetype TMPro.Examples.CameraController/CameraModes Free

	} // end of class CameraModes


	// Fields
	.field private class [UnityEngine.CoreModule]UnityEngine.Transform cameraTransform
	.field private class [UnityEngine.CoreModule]UnityEngine.Transform dummyTarget
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform CameraTarget
	.field public float32 FollowDistance
	.field public float32 MaxFollowDistance
	.field public float32 MinFollowDistance
	.field public float32 ElevationAngle
	.field public float32 MaxElevationAngle
	.field public float32 MinElevationAngle
	.field public float32 OrbitalAngle
	.field public valuetype TMPro.Examples.CameraController/CameraModes CameraMode
	.field public bool MovementSmoothing
	.field public bool RotationSmoothing
	.field private bool previousSmoothing
	.field public float32 MovementSmoothingValue
	.field public float32 RotationSmoothingValue
	.field public float32 MoveSensitivity
	.field private valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 currentVelocity
	.field private valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 desiredPosition
	.field private float32 mouseX
	.field private float32 mouseY
	.field private valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 moveVector
	.field private float32 mouseWheel
	.field private static literal string event_SmoothingValue
	.field private static literal string event_FollowDistance

	// Methods
	.method private hidebysig 
		instance void Awake () cil managed 
	{
		// Method begins at RVA 0x1512c
		// Header size: 12
		// Code size: 146 (0x92)
		.maxstack 2
		.locals init (
			[0] int32,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.RuntimePlatform,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.RuntimePlatform,
			[3] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[4] int32
		)

		IL_0000: nop
		IL_0001: call int32 [UnityEngine.CoreModule]UnityEngine.QualitySettings::get_vSyncCount()
		IL_0006: stloc.0
		IL_0007: ldloc.0
		IL_0008: ldc.i4.0
		IL_0009: cgt
		IL_000b: brtrue.s IL_0014

		IL_000d: nop
		IL_000e: br.s IL_0011

		IL_0010: nop

		IL_0011: nop
		IL_0012: br.s IL_002a

		IL_0014: nop
		IL_0015: ldstr "*(0xFFFFFFFFFFFFFFFF + 0xE0)"
		IL_001a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_001f: conv.i4
		IL_0020: ldc.i4.0
		IL_0021: ceq
		IL_0023: ldc.i4.0
		IL_0024: ceq
		IL_0026: brtrue.s IL_0029

		IL_0028: nop

		IL_0029: nop

		IL_002a: nop
		IL_002b: ldc.i4.s 60
		IL_002d: call void [UnityEngine.CoreModule]UnityEngine.Application::set_targetFrameRate(int32)
		IL_0032: br.s IL_0035

		IL_0034: nop

		IL_0035: nop
		IL_0036: call valuetype [UnityEngine.CoreModule]UnityEngine.RuntimePlatform [UnityEngine.CoreModule]UnityEngine.Application::get_platform()
		IL_003b: stloc.1
		IL_003c: ldloc.1
		IL_003d: box [mscorlib]System.Object
		IL_0042: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0047: ldc.i4.8
		IL_0048: ceq
		IL_004a: brtrue.s IL_006b

		IL_004c: nop
		IL_004d: br.s IL_0050

		IL_004f: nop

		IL_0050: nop
		IL_0051: call valuetype [UnityEngine.CoreModule]UnityEngine.RuntimePlatform [UnityEngine.CoreModule]UnityEngine.Application::get_platform()
		IL_0056: stloc.2
		IL_0057: ldloc.2
		IL_0058: box [mscorlib]System.Object
		IL_005d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0062: ldc.i4.s 11
		IL_0064: ceq
		IL_0066: ldc.i4.0
		IL_0067: ceq
		IL_0069: brtrue.s IL_0072

		IL_006b: nop
		IL_006c: ldc.i4.0
		IL_006d: call void [UnityEngine.InputLegacyModule]UnityEngine.Input::set_simulateMouseWithTouches(bool)

		IL_0072: nop
		IL_0073: ldarg.0
		IL_0074: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0079: stloc.3
		IL_007a: ldarg.0
		IL_007b: ldloc.3
		IL_007c: stfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.CameraController::cameraTransform
		IL_0081: ldarg.0
		IL_0082: ldfld bool TMPro.Examples.CameraController::MovementSmoothing
		IL_0087: stloc.s 4
		IL_0089: ldarg.0
		IL_008a: ldloc.s 4
		IL_008c: stfld bool TMPro.Examples.CameraController::previousSmoothing
		IL_0091: ret
	} // end of method CameraController::Awake

	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x151cc
		// Header size: 12
		// Code size: 77 (0x4d)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] bool,
			[2] bool,
			[3] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[4] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[5] class [UnityEngine.CoreModule]UnityEngine.Transform
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.CameraController::CameraTarget
		IL_0007: stloc.0
		IL_0008: br.s IL_000b

		IL_000a: nop

		IL_000b: nop
		IL_000c: ldloc.0
		IL_000d: ldnull
		IL_000e: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Equality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0013: stloc.1
		IL_0014: ldloc.1
		IL_0015: stloc.2
		IL_0016: ldloc.1
		IL_0017: ldc.i4.0
		IL_0018: ceq
		IL_001a: brtrue.s IL_0048

		IL_001c: nop
		IL_001d: ldstr "Camera Target"
		IL_0022: newobj instance void [UnityEngine.CoreModule]UnityEngine.GameObject::.ctor(string)
		IL_0027: stloc.3
		IL_0028: ldloc.3
		IL_0029: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
		IL_002e: stloc.s 4
		IL_0030: ldarg.0
		IL_0031: ldloc.s 4
		IL_0033: stfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.CameraController::dummyTarget
		IL_0038: ldarg.0
		IL_0039: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.CameraController::dummyTarget
		IL_003e: stloc.s 5
		IL_0040: ldarg.0
		IL_0041: ldloc.s 5
		IL_0043: stfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.CameraController::CameraTarget

		IL_0048: nop
		IL_0049: ret

		IL_004a: nop
		IL_004b: ldnull
		IL_004c: throw
	} // end of method CameraController::Start

	.method private hidebysig 
		instance void LateUpdate () cil managed 
	{
		// Method begins at RVA 0x15228
		// Header size: 12
		// Code size: 638 (0x27e)
		.maxstack 5
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] bool,
			[2] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[4] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[5] float32,
			[6] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[7] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[8] float32,
			[9] float32,
			[10] float32,
			[11] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[12] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[13] float32,
			[14] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[15] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[16] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[17] float32,
			[18] int64
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void TMPro.Examples.CameraController::GetPlayerInput()
		IL_0007: ldarg.0
		IL_0008: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.CameraController::CameraTarget
		IL_000d: stloc.0
		IL_000e: br.s IL_0011

		IL_0010: nop

		IL_0011: nop
		IL_0012: ldloc.0
		IL_0013: ldnull
		IL_0014: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Inequality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0019: stloc.1
		IL_001a: nop
		IL_001b: ldarg.0
		IL_001c: ldfld valuetype TMPro.Examples.CameraController/CameraModes TMPro.Examples.CameraController::CameraMode
		IL_0021: box [mscorlib]System.Object
		IL_0026: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_002b: ldc.i4.1
		IL_002c: ceq
		IL_002e: brtrue IL_015a

		IL_0033: nop
		IL_0034: ldarg.0
		IL_0035: ldfld valuetype TMPro.Examples.CameraController/CameraModes TMPro.Examples.CameraController::CameraMode
		IL_003a: box [mscorlib]System.Object
		IL_003f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0044: ldc.i4.0
		IL_0045: ceq
		IL_0047: ldc.i4.0
		IL_0048: ceq
		IL_004a: brtrue IL_0218

		IL_004f: nop
		IL_0050: ldarg.0
		IL_0051: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.CameraController::CameraTarget
		IL_0056: stloc.2
		IL_0057: nop
		IL_0058: ldloc.2
		IL_0059: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_005e: stloc.3
		IL_005f: ldarg.0
		IL_0060: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.CameraController::CameraTarget
		IL_0065: stloc.s 4
		IL_0067: ldarg.0
		IL_0068: ldfld float32 TMPro.Examples.CameraController::OrbitalAngle
		IL_006d: ldc.r4 0.017453292
		IL_0072: mul
		IL_0073: stloc.s 5
		IL_0075: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_007a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_007f: conv.i8
		IL_0080: ldc.i8 -25
		IL_0089: add
		IL_008a: box [mscorlib]System.Int64
		IL_008f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0094: call valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_FromEulerRad(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0099: stloc.s 6
		IL_009b: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_00a0: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00a5: conv.i8
		IL_00a6: ldc.i8 7
		IL_00af: add
		IL_00b0: box [mscorlib]System.Int64
		IL_00b5: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion>(object)
		IL_00ba: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_00bf: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00c4: conv.i8
		IL_00c5: ldc.i8 -25
		IL_00ce: add
		IL_00cf: box [mscorlib]System.Int64
		IL_00d4: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_00d9: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Quaternion::op_Multiply(valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_00de: stloc.s 7
		IL_00e0: ldloc.s 4
		IL_00e2: stloc.0
		IL_00e3: ldloca.s 3
		IL_00e5: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00ea: stloc.s 8
		IL_00ec: ldloca.s 3
		IL_00ee: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00f3: stloc.s 9
		IL_00f5: ldloca.s 3
		IL_00f7: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00fc: stloc.s 10
		IL_00fe: nop
		IL_00ff: ldloc.0
		IL_0100: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0105: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_010a: conv.i8
		IL_010b: ldc.i8 -25
		IL_0114: add
		IL_0115: box [mscorlib]System.Int64
		IL_011a: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_011f: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::TransformDirection(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0124: stloc.s 11
		IL_0126: ldarg.0
		IL_0127: ldstr "unpcklps xmm6,xmm7"
		IL_012c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0131: box [mscorlib]System.Int64
		IL_0136: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_013b: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.CameraController::desiredPosition
		IL_0140: ldarg.0
		IL_0141: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.CameraController::desiredPosition
		IL_0146: ldloc.s 10
		IL_0148: ldloca.s 11
		IL_014a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_014f: add
		IL_0150: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0155: br IL_0218

		IL_015a: nop
		IL_015b: ldarg.0
		IL_015c: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.CameraController::CameraTarget
		IL_0161: stloc.2
		IL_0162: nop
		IL_0163: ldloc.2
		IL_0164: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0169: stloc.s 12
		IL_016b: ldarg.0
		IL_016c: ldfld float32 TMPro.Examples.CameraController::OrbitalAngle
		IL_0171: ldc.r4 0.017453292
		IL_0176: mul
		IL_0177: stloc.s 13
		IL_0179: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_017e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0183: conv.i8
		IL_0184: ldc.i8 -25
		IL_018d: add
		IL_018e: box [mscorlib]System.Int64
		IL_0193: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0198: call valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_FromEulerRad(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_019d: stloc.s 14
		IL_019f: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_01a4: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01a9: conv.i8
		IL_01aa: ldc.i8 7
		IL_01b3: add
		IL_01b4: box [mscorlib]System.Int64
		IL_01b9: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion>(object)
		IL_01be: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_01c3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01c8: conv.i8
		IL_01c9: ldc.i8 -25
		IL_01d2: add
		IL_01d3: box [mscorlib]System.Int64
		IL_01d8: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_01dd: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Quaternion::op_Multiply(valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_01e2: stloc.s 15
		IL_01e4: ldarg.0
		IL_01e5: ldstr "unpcklps xmm8,xmm6"
		IL_01ea: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01ef: box [mscorlib]System.Int64
		IL_01f4: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_01f9: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.CameraController::desiredPosition
		IL_01fe: ldarg.0
		IL_01ff: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.CameraController::desiredPosition
		IL_0204: ldloca.s 12
		IL_0206: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_020b: ldloca.s 15
		IL_020d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0212: add
		IL_0213: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z

		IL_0218: nop
		IL_0219: ldarg.0
		IL_021a: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.CameraController::cameraTransform
		IL_021f: stloc.s 16
		IL_0221: ldarg.0
		IL_0222: ldfld bool TMPro.Examples.CameraController::MovementSmoothing
		IL_0227: ldc.i4.0
		IL_0228: ceq
		IL_022a: ldc.i4.0
		IL_022b: ceq
		IL_022d: brtrue.s IL_027b

		IL_022f: nop
		IL_0230: nop
		IL_0231: ldarg.0
		IL_0232: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.CameraController::desiredPosition
		IL_0237: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_023c: stloc.s 17
		IL_023e: ldstr "sub_25497C"
		IL_0243: ldc.i8 0
		IL_024c: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0251: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0256: conv.i8
		IL_0257: ldc.i8 -25
		IL_0260: add
		IL_0261: ldc.i8 0
		IL_026a: ldc.i8 0
		IL_0273: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0278: stloc.s 18
		IL_027a: ret

		IL_027b: nop
		IL_027c: nop
		IL_027d: ret
	} // end of method CameraController::LateUpdate

	.method private hidebysig 
		instance void GetPlayerInput () cil managed 
	{
		// Method begins at RVA 0x154b4
		// Header size: 12
		// Code size: 2390 (0x956)
		.maxstack 4
		.locals init (
			[0] int64,
			[1] float32,
			[2] float32,
			[3] int32,
			[4] bool,
			[5] float32,
			[6] bool,
			[7] float32,
			[8] bool,
			[9] bool,
			[10] bool,
			[11] bool,
			[12] float32,
			[13] float32,
			[14] float32,
			[15] float32,
			[16] float32,
			[17] float32,
			[18] float32,
			[19] float32,
			[20] float32,
			[21] valuetype [UnityEngine.InputLegacyModule]UnityEngine.Touch,
			[22] valuetype [UnityEngine.InputLegacyModule]UnityEngine.Touch,
			[23] valuetype [UnityEngine.InputLegacyModule]UnityEngine.TouchPhase,
			[24] valuetype [UnityEngine.InputLegacyModule]UnityEngine.Touch,
			[25] valuetype [UnityEngine.InputLegacyModule]UnityEngine.Touch,
			[26] valuetype [UnityEngine.CoreModule]UnityEngine.Vector2,
			[27] int32,
			[28] valuetype [UnityEngine.CoreModule]UnityEngine.Vector2,
			[29] float32,
			[30] float32,
			[31] float32,
			[32] float32,
			[33] bool,
			[34] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[35] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[36] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[37] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[38] valuetype [UnityEngine.CoreModule]UnityEngine.Ray,
			[39] int64,
			[40] int64,
			[41] valuetype [UnityEngine.CoreModule]UnityEngine.Ray,
			[42] bool,
			[43] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[44] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[45] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[46] bool,
			[47] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[48] bool,
			[49] bool,
			[50] bool,
			[51] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[52] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[53] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[54] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[55] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[56] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[57] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[58] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[59] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[60] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[61] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[62] float32,
			[63] float32,
			[64] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[65] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[66] int64,
			[67] int64,
			[68] int64,
			[69] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[70] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3
		)

		IL_0000: nop
		IL_0001: ldstr "movups [rbp+60h],xmm0 ; value = FloatExpr"
		IL_0006: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_000b: ldstr "mov [rbp-60h],eax ; value = 0x0"
		IL_0010: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0015: ldstr "movups [rbp+6Ch],xmm0 ; value = FloatExpr"
		IL_001a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_001f: ldstr "mov [rbp-10h],eax ; value = 0x0"
		IL_0024: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0029: ldstr "mov [rbp+40h],eax ; value = 0x0"
		IL_002e: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0033: ldstr "movups [rbp-80h],xmm0 ; value = FloatExpr"
		IL_0038: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_003d: ldstr "movups [rbp-70h],xmm0 ; value = FloatExpr"
		IL_0042: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0047: ldstr "movups [rbp+50h],xmm0 ; value = FloatExpr"
		IL_004c: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0051: ldstr "movups [rbp-50h],xmm1 ; value = FloatExpr"
		IL_0056: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_005b: ldstr "movups [rbp-40h],xmm1 ; value = FloatExpr"
		IL_0060: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0065: ldstr "movups [rbp-30h],xmm1 ; value = FloatExpr"
		IL_006a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_006f: ldstr "movups [rbp-20h],xmm1 ; value = FloatExpr"
		IL_0074: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0079: ldstr "movups [rbp],xmm0 ; value = FloatExpr"
		IL_007e: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0083: ldstr "movups [rbp+10h],xmm0 ; value = FloatExpr"
		IL_0088: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_008d: ldstr "movups [rbp+20h],xmm0 ; value = FloatExpr"
		IL_0092: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0097: ldstr "movups [rbp+30h],xmm0 ; value = FloatExpr"
		IL_009c: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_00a1: ldstr "&stack[0x0]"
		IL_00a6: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00ab: conv.i8
		IL_00ac: ldc.i8 -360
		IL_00b5: add
		IL_00b6: stloc.0
		IL_00b7: ldstr "global_19E3BFE"
		IL_00bc: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00c1: ldc.i4.0
		IL_00c2: conv.i8
		IL_00c3: ceq
		IL_00c5: ldc.i4.0
		IL_00c6: ceq
		IL_00c8: brtrue.s IL_00d5

		IL_00ca: nop
		IL_00cb: ldstr "mov byte ptr [1819E3BFEh],1 ; value = 0x1"
		IL_00d0: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)

		IL_00d5: nop
		IL_00d6: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::zeroVector
		IL_00db: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00e0: stloc.1
		IL_00e1: ldarg.0
		IL_00e2: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.CameraController::moveVector
		IL_00e7: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::zeroVector
		IL_00ec: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00f1: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00f6: ldarg.0
		IL_00f7: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.CameraController::moveVector
		IL_00fc: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::zeroVector
		IL_0101: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0106: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_010b: ldarg.0
		IL_010c: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.CameraController::moveVector
		IL_0111: ldloc.1
		IL_0112: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0117: ldstr "Mouse ScrollWheel"
		IL_011c: call float32 [UnityEngine.InputLegacyModule]UnityEngine.Input::GetAxis(string)
		IL_0121: stloc.2
		IL_0122: ldarg.0
		IL_0123: ldloc.2
		IL_0124: stfld float32 TMPro.Examples.CameraController::mouseWheel
		IL_0129: call int32 [UnityEngine.InputLegacyModule]UnityEngine.Input::get_touchCount()
		IL_012e: stloc.3
		IL_012f: ldc.i4 304
		IL_0134: box [mscorlib]System.Int32
		IL_0139: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
		IL_013e: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetKeyInt(valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode)
		IL_0143: stloc.s 4
		IL_0145: ldloc.3
		IL_0146: conv.r4
		IL_0147: stloc.s 5
		IL_0149: ldloc.s 4
		IL_014b: ldc.i4.0
		IL_014c: ceq
		IL_014e: ldc.i4.0
		IL_014f: ceq
		IL_0151: brtrue.s IL_0186

		IL_0153: nop
		IL_0154: ldc.i4 303
		IL_0159: box [mscorlib]System.Int32
		IL_015e: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
		IL_0163: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetKeyInt(valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode)
		IL_0168: stloc.s 6
		IL_016a: ldloc.s 6
		IL_016c: ldc.i4.0
		IL_016d: ceq
		IL_016f: ldc.i4.0
		IL_0170: ceq
		IL_0172: brtrue.s IL_0186

		IL_0174: nop
		IL_0175: ldloc.s 5
		IL_0177: ldc.r4 0.0
		IL_017c: cgt
		IL_017e: ldc.i4.0
		IL_017f: ceq
		IL_0181: brtrue IL_0952

		IL_0186: nop
		IL_0187: ldarg.0
		IL_0188: ldfld float32 TMPro.Examples.CameraController::mouseWheel
		IL_018d: ldc.r4 10
		IL_0192: mul
		IL_0193: stloc.s 7
		IL_0195: ldarg.0
		IL_0196: ldloc.s 7
		IL_0198: stfld float32 TMPro.Examples.CameraController::mouseWheel
		IL_019d: ldc.i4.s 105
		IL_019f: box [mscorlib]System.Int32
		IL_01a4: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
		IL_01a9: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetKeyDownInt(valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode)
		IL_01ae: stloc.s 8
		IL_01b0: ldloc.s 8
		IL_01b2: ldc.i4.0
		IL_01b3: ceq
		IL_01b5: brtrue.s IL_01c9

		IL_01b7: nop
		IL_01b8: ldarg.0
		IL_01b9: ldc.i4.1
		IL_01ba: box [mscorlib]System.Int32
		IL_01bf: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype TMPro.Examples.CameraController/CameraModes>(object)
		IL_01c4: stfld valuetype TMPro.Examples.CameraController/CameraModes TMPro.Examples.CameraController::CameraMode

		IL_01c9: nop
		IL_01ca: ldc.i4.s 102
		IL_01cc: box [mscorlib]System.Int32
		IL_01d1: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
		IL_01d6: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetKeyDownInt(valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode)
		IL_01db: stloc.s 9
		IL_01dd: ldloc.s 9
		IL_01df: ldc.i4.0
		IL_01e0: ceq
		IL_01e2: brtrue.s IL_01f6

		IL_01e4: nop
		IL_01e5: ldarg.0
		IL_01e6: ldc.i4.0
		IL_01e7: box [mscorlib]System.Int32
		IL_01ec: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype TMPro.Examples.CameraController/CameraModes>(object)
		IL_01f1: stfld valuetype TMPro.Examples.CameraController/CameraModes TMPro.Examples.CameraController::CameraMode

		IL_01f6: nop
		IL_01f7: ldc.i4.s 115
		IL_01f9: box [mscorlib]System.Int32
		IL_01fe: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
		IL_0203: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetKeyDownInt(valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode)
		IL_0208: stloc.s 10
		IL_020a: ldloc.s 10
		IL_020c: ldc.i4.0
		IL_020d: ceq
		IL_020f: brtrue.s IL_0221

		IL_0211: nop
		IL_0212: ldarg.0
		IL_0213: ldarg.0
		IL_0214: ldfld bool TMPro.Examples.CameraController::MovementSmoothing
		IL_0219: ldc.i4.0
		IL_021a: ceq
		IL_021c: stfld bool TMPro.Examples.CameraController::MovementSmoothing

		IL_0221: nop
		IL_0222: ldc.i4.1
		IL_0223: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetMouseButton(int32)
		IL_0228: stloc.s 11
		IL_022a: ldloc.s 11
		IL_022c: ldc.i4.0
		IL_022d: ceq
		IL_022f: brtrue IL_039a

		IL_0234: nop
		IL_0235: ldstr "Mouse Y"
		IL_023a: call float32 [UnityEngine.InputLegacyModule]UnityEngine.Input::GetAxis(string)
		IL_023f: stloc.s 12
		IL_0241: ldarg.0
		IL_0242: ldloc.s 12
		IL_0244: stfld float32 TMPro.Examples.CameraController::mouseY
		IL_0249: ldstr "Mouse X"
		IL_024e: call float32 [UnityEngine.InputLegacyModule]UnityEngine.Input::GetAxis(string)
		IL_0253: stloc.s 13
		IL_0255: ldarg.0
		IL_0256: ldfld float32 TMPro.Examples.CameraController::mouseY
		IL_025b: stloc.s 14
		IL_025d: ldarg.0
		IL_025e: ldloc.s 13
		IL_0260: stfld float32 TMPro.Examples.CameraController::mouseX
		IL_0265: ldloc.s 13
		IL_0267: stloc.s 15
		IL_0269: ldloc.s 14
		IL_026b: stloc.s 16
		IL_026d: ldarg.0
		IL_026e: ldfld float32 TMPro.Examples.CameraController::mouseY
		IL_0273: ldc.r4 0.01
		IL_0278: cgt
		IL_027a: brtrue.s IL_028b

		IL_027c: nop
		IL_027d: ldc.r4 -0.01
		IL_0282: ldloc.s 16
		IL_0284: cgt
		IL_0286: ldc.i4.0
		IL_0287: ceq
		IL_0289: brtrue.s IL_02fa

		IL_028b: nop
		IL_028c: ldarg.0
		IL_028d: ldfld float32 TMPro.Examples.CameraController::MinElevationAngle
		IL_0292: stloc.s 17
		IL_0294: ldarg.0
		IL_0295: ldfld float32 TMPro.Examples.CameraController::mouseY
		IL_029a: ldarg.0
		IL_029b: ldfld float32 TMPro.Examples.CameraController::MoveSensitivity
		IL_02a0: mul
		IL_02a1: stloc.s 16
		IL_02a3: ldarg.0
		IL_02a4: ldfld float32 TMPro.Examples.CameraController::ElevationAngle
		IL_02a9: ldarg.0
		IL_02aa: ldfld float32 TMPro.Examples.CameraController::mouseY
		IL_02af: ldarg.0
		IL_02b0: ldfld float32 TMPro.Examples.CameraController::MoveSensitivity
		IL_02b5: mul
		IL_02b6: sub
		IL_02b7: stloc.s 18
		IL_02b9: ldarg.0
		IL_02ba: ldfld float32 TMPro.Examples.CameraController::MinElevationAngle
		IL_02bf: ldarg.0
		IL_02c0: ldfld float32 TMPro.Examples.CameraController::ElevationAngle
		IL_02c5: ldarg.0
		IL_02c6: ldfld float32 TMPro.Examples.CameraController::mouseY
		IL_02cb: ldarg.0
		IL_02cc: ldfld float32 TMPro.Examples.CameraController::MoveSensitivity
		IL_02d1: mul
		IL_02d2: sub
		IL_02d3: cgt
		IL_02d5: brtrue.s IL_02f1

		IL_02d7: nop
		IL_02d8: ldarg.0
		IL_02d9: ldfld float32 TMPro.Examples.CameraController::MaxElevationAngle
		IL_02de: stloc.s 17
		IL_02e0: ldloc.s 18
		IL_02e2: ldarg.0
		IL_02e3: ldfld float32 TMPro.Examples.CameraController::MaxElevationAngle
		IL_02e8: cgt
		IL_02ea: brtrue.s IL_02f1

		IL_02ec: nop
		IL_02ed: ldloc.s 18
		IL_02ef: stloc.s 17

		IL_02f1: nop
		IL_02f2: ldarg.0
		IL_02f3: ldloc.s 17
		IL_02f5: stfld float32 TMPro.Examples.CameraController::ElevationAngle

		IL_02fa: nop
		IL_02fb: ldloc.s 15
		IL_02fd: ldc.r4 0.01
		IL_0302: cgt
		IL_0304: brtrue.s IL_0318

		IL_0306: nop
		IL_0307: ldc.r4 -0.01
		IL_030c: ldloc.s 15
		IL_030e: cgt
		IL_0310: ldc.i4.0
		IL_0311: ceq
		IL_0313: brtrue IL_039a

		IL_0318: nop
		IL_0319: ldloc.s 15
		IL_031b: ldarg.0
		IL_031c: ldfld float32 TMPro.Examples.CameraController::MoveSensitivity
		IL_0321: mul
		IL_0322: ldarg.0
		IL_0323: ldfld float32 TMPro.Examples.CameraController::OrbitalAngle
		IL_0328: add
		IL_0329: stloc.s 19
		IL_032b: ldarg.0
		IL_032c: ldloc.s 19
		IL_032e: stfld float32 TMPro.Examples.CameraController::OrbitalAngle
		IL_0333: ldloc.s 19
		IL_0335: stloc.s 15
		IL_0337: ldloc.s 15
		IL_0339: ldarg.0
		IL_033a: ldfld float32 TMPro.Examples.CameraController::MoveSensitivity
		IL_033f: mul
		IL_0340: ldarg.0
		IL_0341: ldfld float32 TMPro.Examples.CameraController::OrbitalAngle
		IL_0346: add
		IL_0347: ldc.r4 360
		IL_034c: cgt
		IL_034e: ldc.i4.0
		IL_034f: ceq
		IL_0351: brtrue.s IL_036c

		IL_0353: nop
		IL_0354: ldarg.0
		IL_0355: ldloc.s 15
		IL_0357: ldc.r4 360
		IL_035c: sub
		IL_035d: stfld float32 TMPro.Examples.CameraController::OrbitalAngle
		IL_0362: ldloc.s 15
		IL_0364: ldc.r4 360
		IL_0369: sub
		IL_036a: stloc.s 15

		IL_036c: nop
		IL_036d: ldc.r4 0.0
		IL_0372: ldarg.0
		IL_0373: ldfld float32 TMPro.Examples.CameraController::OrbitalAngle
		IL_0378: cgt
		IL_037a: ldc.i4.0
		IL_037b: ceq
		IL_037d: brtrue.s IL_039a

		IL_037f: nop
		IL_0380: ldarg.0
		IL_0381: ldfld float32 TMPro.Examples.CameraController::OrbitalAngle
		IL_0386: ldc.r4 360
		IL_038b: add
		IL_038c: stloc.s 20
		IL_038e: ldarg.0
		IL_038f: ldloc.s 20
		IL_0391: stfld float32 TMPro.Examples.CameraController::OrbitalAngle
		IL_0396: ldloc.s 20
		IL_0398: stloc.s 15

		IL_039a: nop
		IL_039b: nop
		IL_039c: ldloc.s 5
		IL_039e: ldc.r4 1
		IL_03a3: ceq
		IL_03a5: ldc.i4.0
		IL_03a6: ceq
		IL_03a8: brtrue IL_0610

		IL_03ad: nop
		IL_03ae: ldc.i4.0
		IL_03af: call valuetype [UnityEngine.InputLegacyModule]UnityEngine.Touch [UnityEngine.InputLegacyModule]UnityEngine.Input::GetTouch(int32)
		IL_03b4: stloc.s 21
		IL_03b6: ldstr "movaps [rbp-80h],xmm0 ; value = StructChunkExpr"
		IL_03bb: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_03c0: ldstr "movaps [rbp-70h],xmm1 ; value = StructChunkExpr"
		IL_03c5: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_03ca: ldstr "mov [rbp-60h],eax ; value = FieldExpr"
		IL_03cf: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_03d4: ldloca.s 22
		IL_03d6: ldloca.s 21
		IL_03d8: ldfld int32 [UnityEngine.InputLegacyModule]UnityEngine.Touch::m_FingerId
		IL_03dd: stfld int32 [UnityEngine.InputLegacyModule]UnityEngine.Touch::m_FingerId
		IL_03e2: ldloca.s 22
		IL_03e4: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector2 [UnityEngine.InputLegacyModule]UnityEngine.Touch::m_Position
		IL_03e9: ldloca.s 21
		IL_03eb: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector2 [UnityEngine.InputLegacyModule]UnityEngine.Touch::m_Position
		IL_03f0: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector2::x
		IL_03f5: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector2::x
		IL_03fa: ldloca.s 22
		IL_03fc: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector2 [UnityEngine.InputLegacyModule]UnityEngine.Touch::m_RawPosition
		IL_0401: ldloca.s 21
		IL_0403: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector2 [UnityEngine.InputLegacyModule]UnityEngine.Touch::m_RawPosition
		IL_0408: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector2::x
		IL_040d: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector2::x
		IL_0412: ldloca.s 22
		IL_0414: call instance valuetype [UnityEngine.InputLegacyModule]UnityEngine.TouchPhase [UnityEngine.InputLegacyModule]UnityEngine.Touch::get_phase()
		IL_0419: stloc.s 23
		IL_041b: ldloc.s 23
		IL_041d: box [mscorlib]System.Object
		IL_0422: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0427: ldc.i4.1
		IL_0428: ceq
		IL_042a: ldc.i4.0
		IL_042b: ceq
		IL_042d: brtrue IL_0610

		IL_0432: nop
		IL_0433: ldc.i4.0
		IL_0434: call valuetype [UnityEngine.InputLegacyModule]UnityEngine.Touch [UnityEngine.InputLegacyModule]UnityEngine.Input::GetTouch(int32)
		IL_0439: stloc.s 24
		IL_043b: ldstr "movaps [rbp-80h],xmm0 ; value = StructChunkExpr"
		IL_0440: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0445: ldstr "movaps [rbp-70h],xmm1 ; value = StructChunkExpr"
		IL_044a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_044f: ldstr "mov [rbp-60h],eax ; value = FieldExpr"
		IL_0454: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0459: ldloca.s 25
		IL_045b: ldloca.s 24
		IL_045d: ldfld int32 [UnityEngine.InputLegacyModule]UnityEngine.Touch::m_FingerId
		IL_0462: stfld int32 [UnityEngine.InputLegacyModule]UnityEngine.Touch::m_FingerId
		IL_0467: ldloca.s 25
		IL_0469: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector2 [UnityEngine.InputLegacyModule]UnityEngine.Touch::m_Position
		IL_046e: ldloca.s 24
		IL_0470: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector2 [UnityEngine.InputLegacyModule]UnityEngine.Touch::m_Position
		IL_0475: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector2::x
		IL_047a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector2::x
		IL_047f: ldloca.s 25
		IL_0481: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector2 [UnityEngine.InputLegacyModule]UnityEngine.Touch::m_RawPosition
		IL_0486: ldloca.s 24
		IL_0488: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector2 [UnityEngine.InputLegacyModule]UnityEngine.Touch::m_RawPosition
		IL_048d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector2::x
		IL_0492: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector2::x
		IL_0497: ldloca.s 25
		IL_0499: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector2 [UnityEngine.InputLegacyModule]UnityEngine.Touch::get_deltaPosition()
		IL_049e: stloc.s 26
		IL_04a0: ldstr "movsd qword ptr [rbp+170h],xmm0 ; value = LocalExpr"
		IL_04a5: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_04aa: ldstr "*(LocalExpr + 0x174)"
		IL_04af: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_04b4: conv.i4
		IL_04b5: stloc.s 27
		IL_04b7: ldloc.s 26
		IL_04b9: stloc.s 28
		IL_04bb: ldstr "*(LocalExpr + 0x174)"
		IL_04c0: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_04c5: conv.i4
		IL_04c6: box [mscorlib]System.Int32
		IL_04cb: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_04d0: ldc.r4 0.01
		IL_04d5: cgt
		IL_04d7: brtrue.s IL_04f2

		IL_04d9: nop
		IL_04da: ldc.r4 -0.01
		IL_04df: ldloc.s 27
		IL_04e1: box [mscorlib]System.Int32
		IL_04e6: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_04eb: cgt
		IL_04ed: ldc.i4.0
		IL_04ee: ceq
		IL_04f0: brtrue.s IL_054a

		IL_04f2: nop
		IL_04f3: ldarg.0
		IL_04f4: ldfld float32 TMPro.Examples.CameraController::MinElevationAngle
		IL_04f9: stloc.s 29
		IL_04fb: ldarg.0
		IL_04fc: ldfld float32 TMPro.Examples.CameraController::ElevationAngle
		IL_0501: ldloc.s 27
		IL_0503: conv.r4
		IL_0504: ldc.r4 0.1
		IL_0509: mul
		IL_050a: sub
		IL_050b: stloc.s 17
		IL_050d: ldarg.0
		IL_050e: ldfld float32 TMPro.Examples.CameraController::MinElevationAngle
		IL_0513: ldarg.0
		IL_0514: ldfld float32 TMPro.Examples.CameraController::ElevationAngle
		IL_0519: ldloc.s 27
		IL_051b: conv.r4
		IL_051c: ldc.r4 0.1
		IL_0521: mul
		IL_0522: sub
		IL_0523: cgt
		IL_0525: brtrue.s IL_0541

		IL_0527: nop
		IL_0528: ldarg.0
		IL_0529: ldfld float32 TMPro.Examples.CameraController::MaxElevationAngle
		IL_052e: stloc.s 29
		IL_0530: ldloc.s 17
		IL_0532: ldarg.0
		IL_0533: ldfld float32 TMPro.Examples.CameraController::MaxElevationAngle
		IL_0538: cgt
		IL_053a: brtrue.s IL_0541

		IL_053c: nop
		IL_053d: ldloc.s 17
		IL_053f: stloc.s 29

		IL_0541: nop
		IL_0542: ldarg.0
		IL_0543: ldloc.s 29
		IL_0545: stfld float32 TMPro.Examples.CameraController::ElevationAngle

		IL_054a: nop
		IL_054b: ldloc.s 28
		IL_054d: box [mscorlib]System.Object
		IL_0552: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0557: ldc.r4 0.01
		IL_055c: cgt
		IL_055e: brtrue.s IL_057c

		IL_0560: nop
		IL_0561: ldc.r4 -0.01
		IL_0566: ldloc.s 28
		IL_0568: box [mscorlib]System.Object
		IL_056d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0572: cgt
		IL_0574: ldc.i4.0
		IL_0575: ceq
		IL_0577: brtrue IL_0610

		IL_057c: nop
		IL_057d: ldloc.s 28
		IL_057f: box [mscorlib]System.Object
		IL_0584: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0589: ldc.r4 0.1
		IL_058e: mul
		IL_058f: ldarg.0
		IL_0590: ldfld float32 TMPro.Examples.CameraController::OrbitalAngle
		IL_0595: add
		IL_0596: stloc.s 30
		IL_0598: ldarg.0
		IL_0599: ldloc.s 30
		IL_059b: stfld float32 TMPro.Examples.CameraController::OrbitalAngle
		IL_05a0: ldloc.s 30
		IL_05a2: stloc.s 31
		IL_05a4: ldloc.s 28
		IL_05a6: box [mscorlib]System.Object
		IL_05ab: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_05b0: ldc.r4 0.1
		IL_05b5: mul
		IL_05b6: ldarg.0
		IL_05b7: ldfld float32 TMPro.Examples.CameraController::OrbitalAngle
		IL_05bc: add
		IL_05bd: ldc.r4 360
		IL_05c2: cgt
		IL_05c4: ldc.i4.0
		IL_05c5: ceq
		IL_05c7: brtrue.s IL_05e2

		IL_05c9: nop
		IL_05ca: ldarg.0
		IL_05cb: ldloc.s 31
		IL_05cd: ldc.r4 360
		IL_05d2: sub
		IL_05d3: stfld float32 TMPro.Examples.CameraController::OrbitalAngle
		IL_05d8: ldloc.s 31
		IL_05da: ldc.r4 360
		IL_05df: sub
		IL_05e0: stloc.s 31

		IL_05e2: nop
		IL_05e3: ldc.r4 0.0
		IL_05e8: ldarg.0
		IL_05e9: ldfld float32 TMPro.Examples.CameraController::OrbitalAngle
		IL_05ee: cgt
		IL_05f0: ldc.i4.0
		IL_05f1: ceq
		IL_05f3: brtrue.s IL_0610

		IL_05f5: nop
		IL_05f6: ldarg.0
		IL_05f7: ldfld float32 TMPro.Examples.CameraController::OrbitalAngle
		IL_05fc: ldc.r4 360
		IL_0601: add
		IL_0602: stloc.s 32
		IL_0604: ldarg.0
		IL_0605: ldloc.s 32
		IL_0607: stfld float32 TMPro.Examples.CameraController::OrbitalAngle
		IL_060c: ldloc.s 32
		IL_060e: stloc.s 31

		IL_0610: nop
		IL_0611: ldc.i4.0
		IL_0612: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetMouseButton(int32)
		IL_0617: stloc.s 33
		IL_0619: ldloc.s 33
		IL_061b: ldc.i4.0
		IL_061c: ceq
		IL_061e: brtrue IL_0730

		IL_0623: nop
		IL_0624: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_0629: stloc.s 34
		IL_062b: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.InputLegacyModule]UnityEngine.Input::get_mousePosition()
		IL_0630: stloc.s 35
		IL_0632: ldloc.s 34
		IL_0634: stloc.s 36
		IL_0636: nop
		IL_0637: ldloc.s 36
		IL_0639: ldloca.s 37
		IL_063b: ldloca.s 35
		IL_063d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0642: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0647: ldloca.s 37
		IL_0649: ldloca.s 35
		IL_064b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0650: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0655: ldloca.s 37
		IL_0657: ldloca.s 35
		IL_0659: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_065e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0663: ldloc.s 37
		IL_0665: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Ray [UnityEngine.CoreModule]UnityEngine.Camera::ScreenPointToRay(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_066a: stloc.s 38
		IL_066c: ldstr "struct_chunk(LocalExpr+0x0, 16)"
		IL_0671: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0676: stloc.s 39
		IL_0678: ldstr "struct_chunk(LocalExpr+0x10, 8)"
		IL_067d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0682: stloc.s 40
		IL_0684: br.s IL_0687

		IL_0686: nop

		IL_0687: nop
		IL_0688: ldloca.s 41
		IL_068a: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Ray::m_Origin
		IL_068f: ldloc.s 39
		IL_0691: box [mscorlib]System.Int64
		IL_0696: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_069b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_06a0: ldloc.s 41
		IL_06a2: ldloc.0
		IL_06a3: ldc.i8 80
		IL_06ac: add
		IL_06ad: ldc.r4 300
		IL_06b2: ldc.i4 23552
		IL_06b7: call bool [UnityEngine.PhysicsModule]UnityEngine.Physics::Raycast(valuetype [UnityEngine.CoreModule]UnityEngine.Ray, valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit&, float32, int32)
		IL_06bc: stloc.s 42
		IL_06be: ldloc.s 42
		IL_06c0: ldc.i4.0
		IL_06c1: ceq
		IL_06c3: brtrue.s IL_0730

		IL_06c5: nop
		IL_06c6: ldloc.0
		IL_06c7: ldc.i8 80
		IL_06d0: add
		IL_06d1: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_06d6: stloc.s 43
		IL_06d8: ldarg.0
		IL_06d9: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.CameraController::CameraTarget
		IL_06de: stloc.s 44
		IL_06e0: ldloc.s 43
		IL_06e2: stloc.s 45
		IL_06e4: br.s IL_06e7

		IL_06e6: nop

		IL_06e7: nop
		IL_06e8: ldloc.s 45
		IL_06ea: ldloc.s 44
		IL_06ec: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Equality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_06f1: stloc.s 46
		IL_06f3: ldloc.s 46
		IL_06f5: ldc.i4.0
		IL_06f6: ceq
		IL_06f8: ldc.i4.0
		IL_06f9: ceq
		IL_06fb: brtrue.s IL_0724

		IL_06fd: nop
		IL_06fe: ldloc.0
		IL_06ff: ldc.i8 80
		IL_0708: add
		IL_0709: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_070e: stloc.s 47
		IL_0710: ldarg.0
		IL_0711: ldloc.s 47
		IL_0713: stfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.CameraController::CameraTarget
		IL_0718: ldarg.0
		IL_0719: ldarg.0
		IL_071a: ldfld bool TMPro.Examples.CameraController::previousSmoothing
		IL_071f: stfld bool TMPro.Examples.CameraController::MovementSmoothing

		IL_0724: nop
		IL_0725: ldarg.0
		IL_0726: ldc.r4 0.0
		IL_072b: stfld float32 TMPro.Examples.CameraController::OrbitalAngle

		IL_0730: nop
		IL_0731: ldc.i4.2
		IL_0732: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetMouseButton(int32)
		IL_0737: stloc.s 48
		IL_0739: ldloc.s 48
		IL_073b: ldc.i4.0
		IL_073c: ceq
		IL_073e: brtrue IL_0952

		IL_0743: nop
		IL_0744: ldarg.0
		IL_0745: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.CameraController::dummyTarget
		IL_074a: stloc.s 45
		IL_074c: br.s IL_074f

		IL_074e: nop

		IL_074f: nop
		IL_0750: ldloc.s 45
		IL_0752: ldnull
		IL_0753: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Equality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0758: stloc.s 49
		IL_075a: ldloc.s 49
		IL_075c: ldc.i4.0
		IL_075d: ceq
		IL_075f: ldc.i4.0
		IL_0760: ceq
		IL_0762: brtrue.s IL_0791

		IL_0764: nop
		IL_0765: ldarg.0
		IL_0766: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.CameraController::CameraTarget
		IL_076b: stloc.s 44
		IL_076d: ldarg.0
		IL_076e: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.CameraController::dummyTarget
		IL_0773: stloc.s 45
		IL_0775: br.s IL_0778

		IL_0777: nop

		IL_0778: nop
		IL_0779: ldloc.s 45
		IL_077b: ldloc.s 44
		IL_077d: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Inequality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0782: stloc.s 50
		IL_0784: ldloc.s 50
		IL_0786: ldc.i4.0
		IL_0787: ceq
		IL_0789: brtrue IL_0852

		IL_078e: nop
		IL_078f: br.s IL_07b4

		IL_0791: nop
		IL_0792: ldstr "Camera Target"
		IL_0797: newobj instance void [UnityEngine.CoreModule]UnityEngine.GameObject::.ctor(string)
		IL_079c: stloc.s 51
		IL_079e: ldloc.s 51
		IL_07a0: stloc.s 52
		IL_07a2: nop
		IL_07a3: ldloc.s 52
		IL_07a5: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
		IL_07aa: stloc.s 53
		IL_07ac: ldarg.0
		IL_07ad: ldloc.s 53
		IL_07af: stfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.CameraController::dummyTarget

		IL_07b4: nop
		IL_07b5: ldarg.0
		IL_07b6: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.CameraController::CameraTarget
		IL_07bb: stloc.s 54
		IL_07bd: nop
		IL_07be: ldarg.0
		IL_07bf: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.CameraController::dummyTarget
		IL_07c4: stloc.s 55
		IL_07c6: ldloc.s 54
		IL_07c8: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_07cd: stloc.s 56
		IL_07cf: ldloc.s 55
		IL_07d1: stloc.s 57
		IL_07d3: nop
		IL_07d4: ldloc.s 57
		IL_07d6: ldloca.s 58
		IL_07d8: ldloca.s 56
		IL_07da: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_07df: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_07e4: ldloca.s 58
		IL_07e6: ldloca.s 56
		IL_07e8: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_07ed: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_07f2: ldloca.s 58
		IL_07f4: ldloca.s 56
		IL_07f6: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_07fb: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0800: ldloc.s 58
		IL_0802: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0807: ldarg.0
		IL_0808: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.CameraController::CameraTarget
		IL_080d: stloc.s 54
		IL_080f: nop
		IL_0810: ldarg.0
		IL_0811: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.CameraController::dummyTarget
		IL_0816: stloc.s 59
		IL_0818: ldloc.s 54
		IL_081a: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Transform::get_rotation()
		IL_081f: stloc.s 60
		IL_0821: ldloc.s 59
		IL_0823: stloc.s 57
		IL_0825: nop
		IL_0826: ldloc.s 57
		IL_0828: ldloc.s 60
		IL_082a: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_rotation(valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion)
		IL_082f: ldarg.0
		IL_0830: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.CameraController::dummyTarget
		IL_0835: stloc.s 61
		IL_0837: ldarg.0
		IL_0838: ldloc.s 61
		IL_083a: stfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.CameraController::CameraTarget
		IL_083f: ldarg.0
		IL_0840: ldarg.0
		IL_0841: ldfld bool TMPro.Examples.CameraController::MovementSmoothing
		IL_0846: stfld bool TMPro.Examples.CameraController::previousSmoothing
		IL_084b: ldarg.0
		IL_084c: ldc.i4.0
		IL_084d: stfld bool TMPro.Examples.CameraController::MovementSmoothing

		IL_0852: nop
		IL_0853: ldstr "Mouse Y"
		IL_0858: call float32 [UnityEngine.InputLegacyModule]UnityEngine.Input::GetAxis(string)
		IL_085d: stloc.s 62
		IL_085f: ldarg.0
		IL_0860: ldloc.s 62
		IL_0862: stfld float32 TMPro.Examples.CameraController::mouseY
		IL_0867: ldstr "Mouse X"
		IL_086c: call float32 [UnityEngine.InputLegacyModule]UnityEngine.Input::GetAxis(string)
		IL_0871: stloc.s 63
		IL_0873: ldarg.0
		IL_0874: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.CameraController::cameraTransform
		IL_0879: stloc.s 64
		IL_087b: ldarg.0
		IL_087c: ldloc.s 63
		IL_087e: stfld float32 TMPro.Examples.CameraController::mouseX
		IL_0883: ldloc.s 63
		IL_0885: stloc.s 31
		IL_0887: ldloc.s 64
		IL_0889: stloc.s 54
		IL_088b: nop
		IL_088c: ldloc.s 54
		IL_088e: ldloc.s 31
		IL_0890: ldarg.0
		IL_0891: ldfld float32 TMPro.Examples.CameraController::mouseY
		IL_0896: ldc.r4 0.0
		IL_089b: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::TransformDirection(float32, float32, float32)
		IL_08a0: stloc.s 65
		IL_08a2: ldarg.0
		IL_08a3: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.CameraController::moveVector
		IL_08a8: ldloca.s 65
		IL_08aa: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_08af: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_08b4: ldarg.0
		IL_08b5: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.CameraController::moveVector
		IL_08ba: ldloca.s 65
		IL_08bc: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_08c1: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_08c6: ldarg.0
		IL_08c7: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.CameraController::moveVector
		IL_08cc: ldloca.s 65
		IL_08ce: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_08d3: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_08d8: ldstr "xorps xmm1,xmm0"
		IL_08dd: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_08e2: stloc.s 66
		IL_08e4: ldstr "xorps xmm2,xmm0"
		IL_08e9: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_08ee: stloc.s 67
		IL_08f0: ldstr "xorps xmm3,xmm0"
		IL_08f5: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_08fa: stloc.s 68
		IL_08fc: ldarg.0
		IL_08fd: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.CameraController::dummyTarget
		IL_0902: stloc.s 69
		IL_0904: nop
		IL_0905: ldloc.s 69
		IL_0907: ldloca.s 70
		IL_0909: ldloc.s 66
		IL_090b: box [mscorlib]System.Int64
		IL_0910: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0915: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_091a: ldloca.s 70
		IL_091c: ldloc.s 67
		IL_091e: box [mscorlib]System.Int64
		IL_0923: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0928: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_092d: ldloca.s 70
		IL_092f: ldloc.s 68
		IL_0931: box [mscorlib]System.Int64
		IL_0936: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_093b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0940: ldloc.s 70
		IL_0942: ldc.i4.0
		IL_0943: box [mscorlib]System.Int32
		IL_0948: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Space>(object)
		IL_094d: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::Translate(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Space)

		IL_0952: nop
		IL_0953: nop
		IL_0954: nop
		IL_0955: ret
	} // end of method CameraController::GetPlayerInput

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x15e18
		// Header size: 12
		// Code size: 168 (0xa8)
		.maxstack 2
		.locals init (
			[0] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.r4 30
		IL_0007: stfld float32 TMPro.Examples.CameraController::FollowDistance
		IL_000c: ldarg.0
		IL_000d: ldc.r4 100
		IL_0012: stfld float32 TMPro.Examples.CameraController::MaxFollowDistance
		IL_0017: ldarg.0
		IL_0018: ldc.r4 2
		IL_001d: stfld float32 TMPro.Examples.CameraController::MinFollowDistance
		IL_0022: ldarg.0
		IL_0023: ldc.r4 30
		IL_0028: stfld float32 TMPro.Examples.CameraController::ElevationAngle
		IL_002d: ldarg.0
		IL_002e: ldc.r4 85
		IL_0033: stfld float32 TMPro.Examples.CameraController::MaxElevationAngle
		IL_0038: ldarg.0
		IL_0039: ldc.i4.1
		IL_003a: stfld bool TMPro.Examples.CameraController::MovementSmoothing
		IL_003f: ldarg.0
		IL_0040: ldc.r4 25
		IL_0045: stfld float32 TMPro.Examples.CameraController::MovementSmoothingValue
		IL_004a: ldarg.0
		IL_004b: ldc.r4 5
		IL_0050: stfld float32 TMPro.Examples.CameraController::RotationSmoothingValue
		IL_0055: ldarg.0
		IL_0056: ldc.r4 2
		IL_005b: stfld float32 TMPro.Examples.CameraController::MoveSensitivity
		IL_0060: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::zeroVector
		IL_0065: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_006a: stloc.0
		IL_006b: ldarg.0
		IL_006c: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.CameraController::currentVelocity
		IL_0071: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::zeroVector
		IL_0076: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_007b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0080: ldarg.0
		IL_0081: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.CameraController::currentVelocity
		IL_0086: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::zeroVector
		IL_008b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0090: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0095: ldarg.0
		IL_0096: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.CameraController::currentVelocity
		IL_009b: ldloc.0
		IL_009c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00a1: ldarg.0
		IL_00a2: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_00a7: ret
	} // end of method CameraController::.ctor

} // end of class TMPro.Examples.CameraController
