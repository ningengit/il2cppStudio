.class public auto ansi beforefieldinit CameraScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject player
	.field public class PlayerScript ps
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform baldi
	.field public valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BaldiOffset
	.field public float32 initVelocity
	.field public float32 velocity
	.field public float32 gravity
	.field private int32 lookBehind
	.field public valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 offset
	.field public float32 jumpHeight
	.field public valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 jumpHeightV3

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0xf954
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
		IL_0010: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject CameraScript::player
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
		IL_0037: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::offset
		IL_003c: ldarg.0
		IL_003d: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::offset
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
	} // end of method CameraScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0xf9bc
		// Header size: 12
		// Code size: 281 (0x119)
		.maxstack 5
		.locals init (
			[0] class InputManager,
			[1] int32,
			[2] float32,
			[3] float32,
			[4] float32,
			[5] float32,
			[6] float32,
			[7] float32,
			[8] class InputManager,
			[9] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class PlayerScript CameraScript::ps
		IL_0007: ldfld bool PlayerScript::jumpRope
		IL_000c: ldc.i4.0
		IL_000d: ceq
		IL_000f: ldc.i4.0
		IL_0010: ceq
		IL_0012: brtrue.s IL_0042

		IL_0014: nop
		IL_0015: call !0 class Singleton`1<class InputManager>::get_Instance()
		IL_001a: stloc.0
		IL_001b: ldloc.0
		IL_001c: ldfld bool[] InputManager::currentKeyStates
		IL_0021: ldc.i4.s 10
		IL_0023: ldelem [mscorlib]System.Boolean
		IL_0028: neg
		IL_0029: stloc.1
		IL_002a: ldarg.0
		IL_002b: ldstr "sbb ecx,ecx"
		IL_0030: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0035: conv.i4
		IL_0036: ldc.i4 180
		IL_003b: and
		IL_003c: stfld int32 CameraScript::lookBehind
		IL_0041: ret

		IL_0042: nop
		IL_0043: ldarg.0
		IL_0044: ldfld float32 CameraScript::gravity
		IL_0049: stloc.2
		IL_004a: ldarg.0
		IL_004b: ldfld float32 CameraScript::velocity
		IL_0050: stloc.3
		IL_0051: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0056: stloc.s 4
		IL_0058: ldarg.0
		IL_0059: ldfld float32 CameraScript::jumpHeight
		IL_005e: stloc.s 5
		IL_0060: ldarg.0
		IL_0061: ldloc.3
		IL_0062: ldloc.s 4
		IL_0064: ldloc.2
		IL_0065: mul
		IL_0066: sub
		IL_0067: stfld float32 CameraScript::velocity
		IL_006c: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0071: stloc.s 6
		IL_0073: ldarg.0
		IL_0074: ldloc.s 6
		IL_0076: ldloc.3
		IL_0077: ldloc.s 4
		IL_0079: ldloc.2
		IL_007a: mul
		IL_007b: sub
		IL_007c: mul
		IL_007d: ldloc.s 5
		IL_007f: add
		IL_0080: stfld float32 CameraScript::jumpHeight
		IL_0085: ldloc.s 6
		IL_0087: ldloc.3
		IL_0088: ldloc.s 4
		IL_008a: ldloc.2
		IL_008b: mul
		IL_008c: sub
		IL_008d: mul
		IL_008e: ldloc.s 5
		IL_0090: add
		IL_0091: stloc.s 7
		IL_0093: ldc.r4 0.0
		IL_0098: ldloc.s 6
		IL_009a: ldloc.3
		IL_009b: ldloc.s 4
		IL_009d: ldloc.2
		IL_009e: mul
		IL_009f: sub
		IL_00a0: mul
		IL_00a1: ldloc.s 5
		IL_00a3: add
		IL_00a4: clt
		IL_00a6: brtrue.s IL_00db

		IL_00a8: nop
		IL_00a9: ldarg.0
		IL_00aa: ldc.r4 0.0
		IL_00af: stfld float32 CameraScript::jumpHeight
		IL_00b4: call !0 class Singleton`1<class InputManager>::get_Instance()
		IL_00b9: stloc.s 8
		IL_00bb: ldloc.s 8
		IL_00bd: ldfld bool[] InputManager::currentKeyStates
		IL_00c2: ldc.i4.s 11
		IL_00c4: ldelem [mscorlib]System.Boolean
		IL_00c9: ldc.i4.0
		IL_00ca: ceq
		IL_00cc: brtrue.s IL_00db

		IL_00ce: nop
		IL_00cf: ldarg.0
		IL_00d0: ldarg.0
		IL_00d1: ldfld float32 CameraScript::initVelocity
		IL_00d6: stfld float32 CameraScript::velocity

		IL_00db: nop
		IL_00dc: ldarg.0
		IL_00dd: ldfld float32 CameraScript::jumpHeight
		IL_00e2: stloc.s 9
		IL_00e4: ldarg.0
		IL_00e5: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::jumpHeightV3
		IL_00ea: ldloc.s 9
		IL_00ec: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00f1: ldarg.0
		IL_00f2: ldc.i4.0
		IL_00f3: box [mscorlib]System.Int32
		IL_00f8: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_00fd: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::jumpHeightV3
		IL_0102: ldarg.0
		IL_0103: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::jumpHeightV3
		IL_0108: ldc.r4 0.0
		IL_010d: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0112: ret

		IL_0113: nop
		IL_0114: ldnull
		IL_0115: throw

		IL_0116: nop
		IL_0117: ldnull
		IL_0118: throw
	} // end of method CameraScript::Update

	.method private hidebysig 
		instance void LateUpdate () cil managed 
	{
		// Method begins at RVA 0xfae4
		// Header size: 12
		// Code size: 1282 (0x502)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] int64,
			[2] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[3] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[4] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[5] float32,
			[6] float32,
			[7] float32,
			[8] class PlayerScript,
			[9] int32,
			[10] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[11] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[12] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[13] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[14] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[15] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[16] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[17] int64,
			[18] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[19] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[20] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[21] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[22] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[23] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[24] float32,
			[25] float32,
			[26] float32,
			[27] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[28] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[29] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[30] float32,
			[31] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[32] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[33] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[34] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[35] float32,
			[36] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[37] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[38] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[39] float32,
			[40] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[41] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0007: stloc.0
		IL_0008: ldstr "&stack[0x0]"
		IL_000d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0012: conv.i8
		IL_0013: ldc.i8 -95
		IL_001c: add
		IL_001d: stloc.1
		IL_001e: ldloc.0
		IL_001f: stloc.2
		IL_0020: ldarg.0
		IL_0021: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject CameraScript::player
		IL_0026: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
		IL_002b: stloc.3
		IL_002c: ldloc.3
		IL_002d: stloc.2
		IL_002e: ldloc.2
		IL_002f: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0034: stloc.s 4
		IL_0036: ldarg.0
		IL_0037: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::offset
		IL_003c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0041: ldloca.s 4
		IL_0043: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0048: add
		IL_0049: stloc.s 5
		IL_004b: ldarg.0
		IL_004c: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::offset
		IL_0051: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0056: ldloca.s 4
		IL_0058: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_005d: add
		IL_005e: stloc.s 6
		IL_0060: ldarg.0
		IL_0061: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::offset
		IL_0066: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_006b: ldloca.s 4
		IL_006d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0072: add
		IL_0073: stloc.s 7
		IL_0075: ldstr "movss dword ptr [rbp-39h],xmm0 ; value = LocalExpr"
		IL_007a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_007f: ldstr "movss dword ptr [rbp-35h],xmm1 ; value = LocalExpr"
		IL_0084: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0089: ldstr "movss dword ptr [rbp-31h],xmm2 ; value = LocalExpr"
		IL_008e: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0093: ldloc.0
		IL_0094: ldloc.1
		IL_0095: ldc.i8 -57
		IL_009e: add
		IL_009f: box [mscorlib]System.Int64
		IL_00a4: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_00a9: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_00ae: ldarg.0
		IL_00af: ldfld class PlayerScript CameraScript::ps
		IL_00b4: stloc.s 8
		IL_00b6: ldloc.s 8
		IL_00b8: ldfld bool PlayerScript::gameOver
		IL_00bd: ldc.i4.0
		IL_00be: ceq
		IL_00c0: stloc.s 9
		IL_00c2: ldloc.s 8
		IL_00c4: ldfld bool PlayerScript::gameOver
		IL_00c9: ldc.i4.0
		IL_00ca: ceq
		IL_00cc: ldloc.s 8
		IL_00ce: ldfld bool PlayerScript::jumpRope
		IL_00d3: ldc.i4.0
		IL_00d4: ceq
		IL_00d6: and
		IL_00d7: ldc.i4.0
		IL_00d8: ceq
		IL_00da: ldc.i4.0
		IL_00db: ceq
		IL_00dd: brtrue IL_038d

		IL_00e2: nop
		IL_00e3: ldloc.s 8
		IL_00e5: ldfld bool PlayerScript::gameOver
		IL_00ea: ldc.i4.0
		IL_00eb: ceq
		IL_00ed: ldc.i4.0
		IL_00ee: ceq
		IL_00f0: brtrue IL_021c

		IL_00f5: nop
		IL_00f6: ldloc.s 8
		IL_00f8: ldfld bool PlayerScript::jumpRope
		IL_00fd: ldc.i4.0
		IL_00fe: ceq
		IL_0100: brtrue IL_021a

		IL_0105: nop
		IL_0106: ldarg.0
		IL_0107: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_010c: stloc.s 10
		IL_010e: ldloc.s 10
		IL_0110: stloc.s 11
		IL_0112: ldarg.0
		IL_0113: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject CameraScript::player
		IL_0118: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
		IL_011d: stloc.s 12
		IL_011f: ldloc.s 12
		IL_0121: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0126: stloc.s 13
		IL_0128: ldarg.0
		IL_0129: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::offset
		IL_012e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0133: ldloca.s 13
		IL_0135: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_013a: add
		IL_013b: ldarg.0
		IL_013c: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::jumpHeightV3
		IL_0141: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0146: add
		IL_0147: stloc.s 5
		IL_0149: ldarg.0
		IL_014a: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::offset
		IL_014f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0154: ldloca.s 13
		IL_0156: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_015b: add
		IL_015c: ldarg.0
		IL_015d: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::jumpHeightV3
		IL_0162: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0167: add
		IL_0168: stloc.s 6
		IL_016a: ldarg.0
		IL_016b: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::offset
		IL_0170: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0175: ldloca.s 13
		IL_0177: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_017c: add
		IL_017d: ldarg.0
		IL_017e: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::jumpHeightV3
		IL_0183: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0188: add
		IL_0189: stloc.s 7
		IL_018b: ldstr "movss dword ptr [rbp-39h],xmm0 ; value = LocalExpr"
		IL_0190: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0195: ldstr "movss dword ptr [rbp-35h],xmm1 ; value = LocalExpr"
		IL_019a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_019f: ldstr "movss dword ptr [rbp-31h],xmm2 ; value = LocalExpr"
		IL_01a4: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_01a9: ldloc.s 11
		IL_01ab: ldloc.1
		IL_01ac: ldc.i8 -57
		IL_01b5: add
		IL_01b6: box [mscorlib]System.Int64
		IL_01bb: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_01c0: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_01c5: ldarg.0
		IL_01c6: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_01cb: stloc.s 14
		IL_01cd: ldloc.s 14
		IL_01cf: stloc.s 11
		IL_01d1: ldarg.0
		IL_01d2: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject CameraScript::player
		IL_01d7: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
		IL_01dc: stloc.s 15
		IL_01de: ldloc.s 15
		IL_01e0: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Transform::get_rotation()
		IL_01e5: stloc.s 16
		IL_01e7: ldstr "movaps [rbp-29h],xmm0 ; value = StructChunkExpr"
		IL_01ec: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_01f1: ldstr "struct_chunk(LocalExpr+0x0, 16)"
		IL_01f6: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01fb: stloc.s 17

		IL_01fd: nop
		IL_01fe: ldloc.s 11
		IL_0200: ldloc.1
		IL_0201: ldc.i8 -41
		IL_020a: add
		IL_020b: box [mscorlib]System.Int64
		IL_0210: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion>(object)
		IL_0215: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_rotation(valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion)

		IL_021a: nop
		IL_021b: ret

		IL_021c: nop
		IL_021d: ldarg.0
		IL_021e: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0223: stloc.s 18
		IL_0225: ldloc.s 18
		IL_0227: stloc.s 19
		IL_0229: ldarg.0
		IL_022a: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform CameraScript::baldi
		IL_022f: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0234: stloc.s 20
		IL_0236: ldloc.s 20
		IL_0238: stloc.s 19
		IL_023a: ldloc.s 19
		IL_023c: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0241: stloc.s 21
		IL_0243: ldarg.0
		IL_0244: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform CameraScript::baldi
		IL_0249: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_024e: stloc.s 22
		IL_0250: ldloc.s 22
		IL_0252: stloc.s 19
		IL_0254: ldloc.s 19
		IL_0256: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_forward()
		IL_025b: stloc.s 23
		IL_025d: ldarg.0
		IL_025e: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::BaldiOffset
		IL_0263: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0268: ldloca.s 23
		IL_026a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_026f: mul
		IL_0270: ldloca.s 21
		IL_0272: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0277: add
		IL_0278: ldc.r4 0.0
		IL_027d: add
		IL_027e: stloc.s 24
		IL_0280: ldarg.0
		IL_0281: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::BaldiOffset
		IL_0286: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_028b: ldloca.s 23
		IL_028d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0292: mul
		IL_0293: ldloca.s 21
		IL_0295: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_029a: add
		IL_029b: ldarg.0
		IL_029c: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::BaldiOffset
		IL_02a1: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_02a6: add
		IL_02a7: stloc.s 25
		IL_02a9: ldarg.0
		IL_02aa: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::BaldiOffset
		IL_02af: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_02b4: ldloca.s 23
		IL_02b6: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_02bb: mul
		IL_02bc: ldloca.s 21
		IL_02be: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_02c3: add
		IL_02c4: ldc.r4 0.0
		IL_02c9: add
		IL_02ca: stloc.s 26
		IL_02cc: ldstr "movss dword ptr [rbp-39h],xmm2 ; value = LocalExpr"
		IL_02d1: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_02d6: ldstr "movss dword ptr [rbp-35h],xmm3 ; value = LocalExpr"
		IL_02db: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_02e0: ldstr "movss dword ptr [rbp-31h],xmm1 ; value = LocalExpr"
		IL_02e5: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_02ea: ldloc.s 18
		IL_02ec: ldloc.1
		IL_02ed: ldc.i8 -57
		IL_02f6: add
		IL_02f7: box [mscorlib]System.Int64
		IL_02fc: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0301: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0306: ldarg.0
		IL_0307: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_030c: stloc.s 27
		IL_030e: ldloc.s 27
		IL_0310: stloc.s 19
		IL_0312: ldarg.0
		IL_0313: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform CameraScript::baldi
		IL_0318: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_031d: stloc.s 28
		IL_031f: ldarg.0
		IL_0320: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform CameraScript::baldi
		IL_0325: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_032a: stloc.s 29
		IL_032c: ldarg.0
		IL_032d: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::BaldiOffset
		IL_0332: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0337: ldloca.s 29
		IL_0339: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_033e: add
		IL_033f: stloc.s 30
		IL_0341: ldarg.0
		IL_0342: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform CameraScript::baldi
		IL_0347: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_034c: stloc.s 31
		IL_034e: ldstr "movss dword ptr [rbp-31h],xmm0 ; value = FieldExpr"
		IL_0353: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0358: ldstr "movss dword ptr [rbp-39h],xmm7 ; value = FieldExpr"
		IL_035d: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0362: ldstr "movss dword ptr [rbp-35h],xmm6 ; value = LocalExpr"
		IL_0367: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_036c: ldloc.s 27
		IL_036e: ldloc.1
		IL_036f: ldc.i8 -57
		IL_0378: add
		IL_0379: box [mscorlib]System.Int64
		IL_037e: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0383: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::LookAt(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0388: br IL_021a

		IL_038d: nop
		IL_038e: ldarg.0
		IL_038f: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0394: stloc.s 32
		IL_0396: ldloc.s 32
		IL_0398: stloc.s 11
		IL_039a: ldarg.0
		IL_039b: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject CameraScript::player
		IL_03a0: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
		IL_03a5: stloc.s 33
		IL_03a7: ldloc.s 33
		IL_03a9: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_03ae: stloc.s 34
		IL_03b0: ldarg.0
		IL_03b1: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::offset
		IL_03b6: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_03bb: ldloca.s 34
		IL_03bd: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_03c2: add
		IL_03c3: stloc.s 35
		IL_03c5: ldarg.0
		IL_03c6: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::offset
		IL_03cb: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_03d0: ldloca.s 34
		IL_03d2: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_03d7: add
		IL_03d8: stloc.s 6
		IL_03da: ldarg.0
		IL_03db: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::offset
		IL_03e0: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_03e5: ldloca.s 34
		IL_03e7: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_03ec: add
		IL_03ed: stloc.s 7
		IL_03ef: ldstr "movss dword ptr [rbp-39h],xmm0 ; value = LocalExpr"
		IL_03f4: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_03f9: ldstr "movss dword ptr [rbp-35h],xmm1 ; value = LocalExpr"
		IL_03fe: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0403: ldstr "movss dword ptr [rbp-31h],xmm2 ; value = LocalExpr"
		IL_0408: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_040d: ldloc.s 11
		IL_040f: ldloc.1
		IL_0410: ldc.i8 -57
		IL_0419: add
		IL_041a: box [mscorlib]System.Int64
		IL_041f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0424: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0429: ldarg.0
		IL_042a: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_042f: stloc.s 36
		IL_0431: ldloc.s 36
		IL_0433: stloc.s 11
		IL_0435: ldarg.0
		IL_0436: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject CameraScript::player
		IL_043b: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
		IL_0440: stloc.s 37
		IL_0442: ldloc.s 37
		IL_0444: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Transform::get_rotation()
		IL_0449: stloc.s 38
		IL_044b: ldarg.0
		IL_044c: ldfld int32 CameraScript::lookBehind
		IL_0451: conv.r4
		IL_0452: stloc.s 39
		IL_0454: ldstr "mov dword ptr [rbp-39h],0 ; value = 0x0"
		IL_0459: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_045e: ldstr "mov dword ptr [rbp-31h],0 ; value = 0x0"
		IL_0463: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0468: ldstr "movss dword ptr [rbp-35h],xmm1 ; value = BinaryExpr"
		IL_046d: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0472: ldloc.1
		IL_0473: ldc.i8 -57
		IL_047c: add
		IL_047d: box [mscorlib]System.Int64
		IL_0482: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0487: call valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_FromEulerRad(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_048c: stloc.s 40
		IL_048e: ldloca.s 40
		IL_0490: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
		IL_0495: ldloca.s 38
		IL_0497: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::y
		IL_049c: mul
		IL_049d: stloc.s 35
		IL_049f: ldloca.s 40
		IL_04a1: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::y
		IL_04a6: ldloca.s 38
		IL_04a8: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
		IL_04ad: mul
		IL_04ae: stloc.s 6
		IL_04b0: ldloca.s 40
		IL_04b2: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
		IL_04b7: ldloca.s 38
		IL_04b9: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
		IL_04be: mul
		IL_04bf: stloc.s 7
		IL_04c1: ldloca.s 40
		IL_04c3: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::y
		IL_04c8: ldloca.s 38
		IL_04ca: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::y
		IL_04cf: mul
		IL_04d0: stloc.s 41
		IL_04d2: ldstr "movss dword ptr [rbp-29h],xmm10 ; value = BinaryExpr"
		IL_04d7: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_04dc: ldstr "movss dword ptr [rbp-25h],xmm11 ; value = BinaryExpr"
		IL_04e1: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_04e6: ldstr "movss dword ptr [rbp-21h],xmm12 ; value = BinaryExpr"
		IL_04eb: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_04f0: ldstr "movss dword ptr [rbp-1Dh],xmm5 ; value = BinaryExpr"
		IL_04f5: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_04fa: br IL_01fd

		IL_04ff: nop
		IL_0500: ldnull
		IL_0501: throw
	} // end of method CameraScript::LateUpdate

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x25bd
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method CameraScript::.ctor

} // end of class CameraScript
