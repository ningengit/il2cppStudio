.class public auto ansi beforefieldinit TMPro.Examples.ObjectSpin
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested public auto ansi sealed MotionType
		extends [mscorlib]System.Enum
	{
		// Fields
		.field public specialname rtspecialname int32 value__
		.field public static literal valuetype TMPro.Examples.ObjectSpin/MotionType Rotation
		.field public static literal valuetype TMPro.Examples.ObjectSpin/MotionType BackAndForth
		.field public static literal valuetype TMPro.Examples.ObjectSpin/MotionType Translation

	} // end of class MotionType


	// Fields
	.field public float32 SpinSpeed
	.field public int32 RotationRange
	.field private class [UnityEngine.CoreModule]UnityEngine.Transform m_transform
	.field private float32 m_time
	.field private valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 m_prevPOS
	.field private valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 m_initial_Rotation
	.field private valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 m_initial_Position
	.field private valuetype [UnityEngine.CoreModule]UnityEngine.Color32 m_lightColor
	.field private int32 frames
	.field public valuetype TMPro.Examples.ObjectSpin/MotionType Motion

	// Methods
	.method private hidebysig 
		instance void Awake () cil managed 
	{
		// Method begins at RVA 0x15ecc
		// Header size: 12
		// Code size: 425 (0x1a9)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[4] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[5] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[6] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[7] int64,
			[8] float32,
			[9] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[10] class [UnityEngine.CoreModule]UnityEngine.Light,
			[11] class [UnityEngine.CoreModule]UnityEngine.Light,
			[12] bool,
			[13] valuetype [UnityEngine.CoreModule]UnityEngine.Color,
			[14] float32,
			[15] float32,
			[16] float32,
			[17] float32,
			[18] valuetype [UnityEngine.CoreModule]UnityEngine.Color,
			[19] valuetype [UnityEngine.CoreModule]UnityEngine.Color32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.ObjectSpin::m_transform
		IL_000f: ldarg.0
		IL_0010: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.ObjectSpin::m_transform
		IL_0015: stloc.1
		IL_0016: ldloc.1
		IL_0017: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Transform::get_rotation()
		IL_001c: stloc.2
		IL_001d: ldloc.2
		IL_001e: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_ToEulerRad(valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion)
		IL_0023: stloc.3
		IL_0024: ldloca.s 4
		IL_0026: ldloca.s 3
		IL_0028: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_002d: ldc.r4 57.29578
		IL_0032: mul
		IL_0033: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0038: ldloca.s 4
		IL_003a: ldloca.s 3
		IL_003c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0041: ldc.r4 57.29578
		IL_0046: mul
		IL_0047: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_004c: ldloca.s 4
		IL_004e: ldloca.s 3
		IL_0050: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0055: ldc.r4 57.29578
		IL_005a: mul
		IL_005b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0060: ldloc.s 4
		IL_0062: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_MakePositive(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0067: stloc.s 5
		IL_0069: ldarg.0
		IL_006a: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.ObjectSpin::m_transform
		IL_006f: stloc.s 6
		IL_0071: ldarg.0
		IL_0072: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.ObjectSpin::m_initial_Rotation
		IL_0077: ldloca.s 5
		IL_0079: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_007e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0083: ldarg.0
		IL_0084: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.ObjectSpin::m_initial_Rotation
		IL_0089: ldloca.s 5
		IL_008b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0090: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0095: ldarg.0
		IL_0096: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.ObjectSpin::m_initial_Rotation
		IL_009b: ldloca.s 5
		IL_009d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00a2: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00a7: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_00ac: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00b1: stloc.s 7
		IL_00b3: ldloca.s 5
		IL_00b5: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00ba: stloc.s 8
		IL_00bc: ldloc.s 6
		IL_00be: stloc.1
		IL_00bf: ldloc.1
		IL_00c0: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_00c5: stloc.s 9
		IL_00c7: ldarg.0
		IL_00c8: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.ObjectSpin::m_initial_Position
		IL_00cd: ldloca.s 9
		IL_00cf: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00d4: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00d9: ldarg.0
		IL_00da: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.ObjectSpin::m_initial_Position
		IL_00df: ldloca.s 9
		IL_00e1: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00e6: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00eb: ldarg.0
		IL_00ec: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.ObjectSpin::m_initial_Position
		IL_00f1: ldloca.s 9
		IL_00f3: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00f8: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00fd: ldarg.0
		IL_00fe: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.CoreModule]UnityEngine.Light>()
		IL_0103: stloc.s 10
		IL_0105: ldloc.s 10
		IL_0107: stloc.s 11
		IL_0109: br.s IL_010c

		IL_010b: nop

		IL_010c: nop
		IL_010d: ldloc.s 11
		IL_010f: ldnull
		IL_0110: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Inequality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0115: stloc.s 12
		IL_0117: ldloc.s 12
		IL_0119: ldc.i4.0
		IL_011a: ceq
		IL_011c: brtrue.s IL_014e

		IL_011e: nop
		IL_011f: ldloc.s 11
		IL_0121: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Color [UnityEngine.CoreModule]UnityEngine.Light::get_color()
		IL_0126: stloc.s 13
		IL_0128: ldloca.s 13
		IL_012a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Color::r
		IL_012f: stloc.s 14
		IL_0131: ldloca.s 13
		IL_0133: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Color::g
		IL_0138: stloc.s 15
		IL_013a: ldloca.s 13
		IL_013c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Color::b
		IL_0141: stloc.s 16
		IL_0143: ldloca.s 13
		IL_0145: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Color::a
		IL_014a: stloc.s 17
		IL_014c: br.s IL_014f

		IL_014e: nop

		IL_014f: nop
		IL_0150: ldloca.s 18
		IL_0152: ldc.r4 0.0
		IL_0157: stfld float32 [UnityEngine.CoreModule]UnityEngine.Color::r
		IL_015c: ldloca.s 18
		IL_015e: ldc.r4 0.0
		IL_0163: stfld float32 [UnityEngine.CoreModule]UnityEngine.Color::g
		IL_0168: ldloca.s 18
		IL_016a: ldc.r4 0.0
		IL_016f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Color::b
		IL_0174: ldloca.s 18
		IL_0176: ldc.r4 1
		IL_017b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Color::a
		IL_0180: ldloc.s 18
		IL_0182: call valuetype [UnityEngine.CoreModule]UnityEngine.Color32 [UnityEngine.CoreModule]UnityEngine.Color32::op_Implicit(valuetype [UnityEngine.CoreModule]UnityEngine.Color)
		IL_0187: stloc.s 19
		IL_0189: ldarg.0
		IL_018a: ldloc.s 19
		IL_018c: box [mscorlib]System.Object
		IL_0191: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0196: box [mscorlib]System.Int32
		IL_019b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Color32>(object)
		IL_01a0: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Color32 TMPro.Examples.ObjectSpin::m_lightColor
		IL_01a5: ret

		IL_01a6: nop
		IL_01a7: ldnull
		IL_01a8: throw
	} // end of method ObjectSpin::Awake

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x16084
		// Header size: 12
		// Code size: 576 (0x240)
		.maxstack 5
		.locals init (
			[0] float32,
			[1] float32,
			[2] float32,
			[3] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[4] int64,
			[5] int64,
			[6] float32,
			[7] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[8] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[9] int64,
			[10] float32,
			[11] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[12] float32,
			[13] int64,
			[14] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[15] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[16] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[17] float32,
			[18] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld float32 TMPro.Examples.ObjectSpin::SpinSpeed
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld valuetype TMPro.Examples.ObjectSpin/MotionType TMPro.Examples.ObjectSpin::Motion
		IL_000e: box [mscorlib]System.Object
		IL_0013: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0018: ldc.i4.0
		IL_0019: ceq
		IL_001b: brtrue IL_020f

		IL_0020: nop
		IL_0021: ldarg.0
		IL_0022: ldfld float32 TMPro.Examples.ObjectSpin::m_time
		IL_0027: stloc.1
		IL_0028: ldarg.0
		IL_0029: ldfld valuetype TMPro.Examples.ObjectSpin/MotionType TMPro.Examples.ObjectSpin::Motion
		IL_002e: box [mscorlib]System.Object
		IL_0033: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0038: ldc.i4.1
		IL_0039: ceq
		IL_003b: brtrue IL_014c

		IL_0040: nop
		IL_0041: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0046: stloc.2
		IL_0047: ldarg.0
		IL_0048: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.ObjectSpin::m_transform
		IL_004d: stloc.3
		IL_004e: ldarg.0
		IL_004f: ldloc.2
		IL_0050: ldloc.0
		IL_0051: mul
		IL_0052: ldloc.1
		IL_0053: add
		IL_0054: stfld float32 TMPro.Examples.ObjectSpin::m_time
		IL_0059: ldstr "sub_20C8F0"
		IL_005e: ldc.i8 0
		IL_0067: ldc.i8 0
		IL_0070: ldc.i8 0
		IL_0079: ldc.i8 0
		IL_0082: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0087: stloc.s 4
		IL_0089: ldloc.s 4
		IL_008b: stloc.s 5
		IL_008d: ldstr "xmm0"
		IL_0092: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0097: conv.r4
		IL_0098: ldc.r4 15
		IL_009d: mul
		IL_009e: ldarg.0
		IL_009f: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.ObjectSpin::m_initial_Position
		IL_00a4: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00a9: add
		IL_00aa: stloc.s 6
		IL_00ac: ldloc.3
		IL_00ad: ldloca.s 7
		IL_00af: ldloc.s 6
		IL_00b1: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00b6: ldloca.s 7
		IL_00b8: ldc.r4 0.0
		IL_00bd: ldarg.0
		IL_00be: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.ObjectSpin::m_initial_Position
		IL_00c3: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00c8: add
		IL_00c9: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00ce: ldloca.s 7
		IL_00d0: ldc.r4 10
		IL_00d5: ldarg.0
		IL_00d6: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.ObjectSpin::m_initial_Position
		IL_00db: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00e0: add
		IL_00e1: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00e6: ldloc.s 7
		IL_00e8: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_00ed: ldarg.0
		IL_00ee: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.ObjectSpin::m_transform
		IL_00f3: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_00f8: stloc.s 8
		IL_00fa: ldarg.0
		IL_00fb: ldarg.0
		IL_00fc: ldfld int32 TMPro.Examples.ObjectSpin::frames
		IL_0101: ldc.i4.1
		IL_0102: add
		IL_0103: stfld int32 TMPro.Examples.ObjectSpin::frames
		IL_0108: ldarg.0
		IL_0109: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.ObjectSpin::m_prevPOS
		IL_010e: ldloca.s 8
		IL_0110: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0115: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_011a: ldarg.0
		IL_011b: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.ObjectSpin::m_prevPOS
		IL_0120: ldloca.s 8
		IL_0122: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0127: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_012c: ldarg.0
		IL_012d: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.ObjectSpin::m_prevPOS
		IL_0132: ldloca.s 8
		IL_0134: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0139: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_013e: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_0143: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0148: stloc.s 9

		IL_014a: nop
		IL_014b: ret

		IL_014c: nop
		IL_014d: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0152: stloc.s 10
		IL_0154: ldarg.0
		IL_0155: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.ObjectSpin::m_transform
		IL_015a: stloc.s 11
		IL_015c: ldarg.0
		IL_015d: ldloc.s 10
		IL_015f: ldloc.0
		IL_0160: mul
		IL_0161: ldloc.1
		IL_0162: add
		IL_0163: stfld float32 TMPro.Examples.ObjectSpin::m_time
		IL_0168: ldarg.0
		IL_0169: ldfld int32 TMPro.Examples.ObjectSpin::RotationRange
		IL_016e: conv.r4
		IL_016f: stloc.s 12
		IL_0171: ldstr "sub_20D1D0"
		IL_0176: ldc.i8 0
		IL_017f: ldc.i8 0
		IL_0188: ldc.i8 0
		IL_0191: ldc.i8 0
		IL_019a: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_019f: stloc.s 13
		IL_01a1: ldloca.s 14
		IL_01a3: ldarg.0
		IL_01a4: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.ObjectSpin::m_initial_Rotation
		IL_01a9: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_01ae: ldc.r4 0.017453292
		IL_01b3: mul
		IL_01b4: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_01b9: ldloca.s 14
		IL_01bb: ldstr "xmm0"
		IL_01c0: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01c5: conv.r4
		IL_01c6: ldloc.s 12
		IL_01c8: mul
		IL_01c9: ldarg.0
		IL_01ca: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.ObjectSpin::m_initial_Rotation
		IL_01cf: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_01d4: add
		IL_01d5: ldc.r4 0.017453292
		IL_01da: mul
		IL_01db: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_01e0: ldloca.s 14
		IL_01e2: ldarg.0
		IL_01e3: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.ObjectSpin::m_initial_Rotation
		IL_01e8: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_01ed: ldc.r4 0.017453292
		IL_01f2: mul
		IL_01f3: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_01f8: ldloc.s 14
		IL_01fa: call valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_FromEulerRad(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_01ff: stloc.s 15
		IL_0201: ldloc.s 11
		IL_0203: ldloc.s 15
		IL_0205: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_rotation(valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion)
		IL_020a: br IL_014a

		IL_020f: nop
		IL_0210: ldarg.0
		IL_0211: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.ObjectSpin::m_transform
		IL_0216: stloc.s 16
		IL_0218: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_021d: stloc.s 17
		IL_021f: ldloc.s 17
		IL_0221: stloc.s 18
		IL_0223: ldloc.s 16
		IL_0225: ldc.r4 0.0
		IL_022a: ldloc.s 18
		IL_022c: ldloc.0
		IL_022d: mul
		IL_022e: ldc.r4 0.0
		IL_0233: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::Rotate(float32, float32, float32)
		IL_0238: br IL_014a

		IL_023d: nop
		IL_023e: ldnull
		IL_023f: throw
	} // end of method ObjectSpin::Update

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x28f4
		// Header size: 1
		// Code size: 27 (0x1b)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.r4 5
		IL_0007: stfld float32 TMPro.Examples.ObjectSpin::SpinSpeed
		IL_000c: ldarg.0
		IL_000d: ldc.i4.s 15
		IL_000f: stfld int32 TMPro.Examples.ObjectSpin::RotationRange
		IL_0014: ldarg.0
		IL_0015: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_001a: ret
	} // end of method ObjectSpin::.ctor

} // end of class TMPro.Examples.ObjectSpin
