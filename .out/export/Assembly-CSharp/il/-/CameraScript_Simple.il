.class public auto ansi beforefieldinit CameraScript_Simple
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject player
	.field private int32 lookBehind
	.field private valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 offset

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0xfff4
		// Header size: 12
		// Code size: 90 (0x5a)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[2] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_000e: stloc.1
		IL_000f: ldarg.0
		IL_0010: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject CameraScript_Simple::player
		IL_0015: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
		IL_001a: stloc.2
		IL_001b: ldloc.2
		IL_001c: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0021: stloc.3
		IL_0022: ldarg.0
		IL_0023: ldstr "unpcklps xmm6,xmm7"
		IL_0028: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_002d: box [mscorlib]System.Int64
		IL_0032: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0037: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript_Simple::offset
		IL_003c: ldarg.0
		IL_003d: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript_Simple::offset
		IL_0042: ldloca.s 1
		IL_0044: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0049: ldloca.s 3
		IL_004b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0050: sub
		IL_0051: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0056: ret

		IL_0057: nop
		IL_0058: ldnull
		IL_0059: throw
	} // end of method CameraScript_Simple::Start

	.method private hidebysig 
		instance void LateUpdate () cil managed 
	{
		// Method begins at RVA 0x1005c
		// Header size: 12
		// Code size: 488 (0x1e8)
		.maxstack 5
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[4] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[5] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[6] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[7] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[8] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[9] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject CameraScript_Simple::player
		IL_000e: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
		IL_0013: stloc.1
		IL_0014: ldloc.1
		IL_0015: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_001a: stloc.2
		IL_001b: ldloc.0
		IL_001c: ldloca.s 3
		IL_001e: ldarg.0
		IL_001f: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript_Simple::offset
		IL_0024: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0029: ldloca.s 2
		IL_002b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0030: add
		IL_0031: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0036: ldloca.s 3
		IL_0038: ldarg.0
		IL_0039: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript_Simple::offset
		IL_003e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0043: ldloca.s 2
		IL_0045: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_004a: add
		IL_004b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0050: ldloca.s 3
		IL_0052: ldarg.0
		IL_0053: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript_Simple::offset
		IL_0058: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_005d: ldloca.s 2
		IL_005f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0064: add
		IL_0065: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_006a: ldloc.3
		IL_006b: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0070: ldarg.0
		IL_0071: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0076: stloc.s 4
		IL_0078: ldarg.0
		IL_0079: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject CameraScript_Simple::player
		IL_007e: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
		IL_0083: stloc.s 5
		IL_0085: ldloc.s 5
		IL_0087: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Transform::get_rotation()
		IL_008c: stloc.s 6
		IL_008e: ldloca.s 7
		IL_0090: ldc.r4 0.0
		IL_0095: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_009a: ldloca.s 7
		IL_009c: ldarg.0
		IL_009d: ldfld int32 CameraScript_Simple::lookBehind
		IL_00a2: conv.r4
		IL_00a3: ldc.r4 0.017453292
		IL_00a8: mul
		IL_00a9: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00ae: ldloca.s 7
		IL_00b0: ldc.r4 0.0
		IL_00b5: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00ba: ldloc.s 7
		IL_00bc: call valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_FromEulerRad(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_00c1: stloc.s 8
		IL_00c3: ldloc.s 4
		IL_00c5: ldloca.s 9
		IL_00c7: ldloca.s 8
		IL_00c9: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
		IL_00ce: ldloca.s 6
		IL_00d0: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::w
		IL_00d5: mul
		IL_00d6: ldloca.s 8
		IL_00d8: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::w
		IL_00dd: ldloca.s 6
		IL_00df: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
		IL_00e4: mul
		IL_00e5: add
		IL_00e6: ldloca.s 8
		IL_00e8: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::z
		IL_00ed: ldloca.s 6
		IL_00ef: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::y
		IL_00f4: mul
		IL_00f5: add
		IL_00f6: ldloca.s 8
		IL_00f8: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::y
		IL_00fd: ldloca.s 6
		IL_00ff: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::z
		IL_0104: mul
		IL_0105: sub
		IL_0106: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
		IL_010b: ldloca.s 9
		IL_010d: ldloca.s 8
		IL_010f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::w
		IL_0114: ldloca.s 6
		IL_0116: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::y
		IL_011b: mul
		IL_011c: ldloca.s 8
		IL_011e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::y
		IL_0123: ldloca.s 6
		IL_0125: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::w
		IL_012a: mul
		IL_012b: add
		IL_012c: ldloca.s 8
		IL_012e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
		IL_0133: ldloca.s 6
		IL_0135: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::z
		IL_013a: mul
		IL_013b: add
		IL_013c: ldloca.s 8
		IL_013e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::z
		IL_0143: ldloca.s 6
		IL_0145: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
		IL_014a: mul
		IL_014b: sub
		IL_014c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::y
		IL_0151: ldloca.s 9
		IL_0153: ldloca.s 8
		IL_0155: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::w
		IL_015a: ldloca.s 6
		IL_015c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::z
		IL_0161: mul
		IL_0162: ldloca.s 8
		IL_0164: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::z
		IL_0169: ldloca.s 6
		IL_016b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::w
		IL_0170: mul
		IL_0171: add
		IL_0172: ldloca.s 8
		IL_0174: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::y
		IL_0179: ldloca.s 6
		IL_017b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
		IL_0180: mul
		IL_0181: add
		IL_0182: ldloca.s 8
		IL_0184: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
		IL_0189: ldloca.s 6
		IL_018b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::y
		IL_0190: mul
		IL_0191: sub
		IL_0192: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::z
		IL_0197: ldloca.s 9
		IL_0199: ldloca.s 8
		IL_019b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::w
		IL_01a0: ldloca.s 6
		IL_01a2: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::w
		IL_01a7: mul
		IL_01a8: ldloca.s 8
		IL_01aa: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
		IL_01af: ldloca.s 6
		IL_01b1: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
		IL_01b6: mul
		IL_01b7: sub
		IL_01b8: ldloca.s 8
		IL_01ba: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::y
		IL_01bf: ldloca.s 6
		IL_01c1: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::y
		IL_01c6: mul
		IL_01c7: sub
		IL_01c8: ldloca.s 8
		IL_01ca: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::z
		IL_01cf: ldloca.s 6
		IL_01d1: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::z
		IL_01d6: mul
		IL_01d7: sub
		IL_01d8: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::w
		IL_01dd: ldloc.s 9
		IL_01df: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_rotation(valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion)
		IL_01e4: ret

		IL_01e5: nop
		IL_01e6: ldnull
		IL_01e7: throw
	} // end of method CameraScript_Simple::LateUpdate

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x25c6
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method CameraScript_Simple::.ctor

} // end of class CameraScript_Simple
