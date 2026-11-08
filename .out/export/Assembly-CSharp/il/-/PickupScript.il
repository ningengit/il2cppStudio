.class public auto ansi beforefieldinit PickupScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class GameControllerScript gc
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform player

	// Methods
	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x1163c
		// Header size: 12
		// Code size: 3009 (0xbc1)
		.maxstack 5
		.locals init (
			[0] class InputManager,
			[1] class InputManager,
			[2] bool,
			[3] bool,
			[4] float32,
			[5] float32,
			[6] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[7] int32,
			[8] int32,
			[9] float32,
			[10] valuetype [UnityEngine.CoreModule]UnityEngine.Ray,
			[11] int64,
			[12] int64,
			[13] bool,
			[14] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[15] string,
			[16] bool,
			[17] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[18] int32,
			[19] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[20] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[21] float32,
			[22] float32,
			[23] float32,
			[24] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[25] int64,
			[26] int64,
			[27] int64,
			[28] int32,
			[29] int64,
			[30] float32,
			[31] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[32] string,
			[33] bool,
			[34] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[35] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[36] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[37] float32,
			[38] int32,
			[39] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[40] string,
			[41] bool,
			[42] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[43] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[44] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[45] float32,
			[46] int32,
			[47] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[48] string,
			[49] bool,
			[50] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[51] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[52] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[53] float32,
			[54] int32,
			[55] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[56] string,
			[57] bool,
			[58] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[59] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[60] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[61] float32,
			[62] int32,
			[63] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[64] string,
			[65] bool,
			[66] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[67] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[68] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[69] float32,
			[70] int32,
			[71] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[72] string,
			[73] bool,
			[74] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[75] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[76] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[77] float32,
			[78] int32,
			[79] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[80] string,
			[81] bool,
			[82] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[83] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[84] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[85] float32,
			[86] int32,
			[87] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[88] string,
			[89] bool,
			[90] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[91] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[92] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[93] float32,
			[94] int32,
			[95] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[96] string,
			[97] bool,
			[98] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[99] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[100] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[101] float32,
			[102] int32,
			[103] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[104] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[105] string,
			[106] bool,
			[107] bool,
			[108] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[109] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[110] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[111] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[112] float32,
			[113] int32,
			[114] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[115] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[116] class GameControllerScript,
			[117] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[118] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[119] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[120] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[121] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[122] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[123] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[124] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[125] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[126] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[127] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[128] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[129] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[130] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[131] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[132] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[133] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[134] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[135] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[136] class [UnityEngine.CoreModule]UnityEngine.GameObject
		)

		IL_0000: nop
		IL_0001: call !0 class Singleton`1<class InputManager>::get_Instance()
		IL_0006: stloc.0
		IL_0007: ldloc.0
		IL_0008: stloc.1
		IL_0009: ldloc.1
		IL_000a: ldc.i4.4
		IL_000b: box [mscorlib]System.Int32
		IL_0010: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
		IL_0015: callvirt instance bool InputManager::GetActionKeyDown(valuetype InputAction)
		IL_001a: stloc.2
		IL_001b: ldloc.2
		IL_001c: stloc.3
		IL_001d: ldloc.2
		IL_001e: ldc.i4.0
		IL_001f: ceq
		IL_0021: brtrue IL_0a35

		IL_0026: nop
		IL_0027: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_timeScale()
		IL_002c: stloc.s 4
		IL_002e: ldloc.s 4
		IL_0030: stloc.s 5
		IL_0032: nop
		IL_0033: ldloc.s 4
		IL_0035: ldc.r4 0.0
		IL_003a: ceq
		IL_003c: brtrue IL_0a35

		IL_0041: nop
		IL_0042: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_0047: stloc.s 6
		IL_0049: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_width()
		IL_004e: stloc.s 7
		IL_0050: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_height()
		IL_0055: stloc.s 8
		IL_0057: ldloc.s 7
		IL_0059: ldstr "rdx"
		IL_005e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0063: conv.i4
		IL_0064: sub
		IL_0065: ldc.i4.1
		IL_0066: shr
		IL_0067: conv.r4
		IL_0068: stloc.s 9
		IL_006a: ldloc.s 8
		IL_006c: ldstr "rdx"
		IL_0071: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0076: conv.i4
		IL_0077: sub
		IL_0078: ldc.i4.1
		IL_0079: shr
		IL_007a: conv.r4
		IL_007b: stloc.s 5
		IL_007d: ldloc.s 6
		IL_007f: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0084: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0089: conv.i8
		IL_008a: ldc.i8 -105
		IL_0093: add
		IL_0094: box [mscorlib]System.Int64
		IL_0099: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_009e: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Ray [UnityEngine.CoreModule]UnityEngine.Camera::ScreenPointToRay(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_00a3: stloc.s 10
		IL_00a5: ldstr "struct_chunk(LocalExpr+0x0, 16)"
		IL_00aa: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00af: stloc.s 11
		IL_00b1: ldstr "struct_chunk(LocalExpr+0x10, 8)"
		IL_00b6: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00bb: stloc.s 12
		IL_00bd: br.s IL_00c0

		IL_00bf: nop

		IL_00c0: nop
		IL_00c1: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_00c6: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00cb: conv.i8
		IL_00cc: ldc.i8 -89
		IL_00d5: add
		IL_00d6: box [mscorlib]System.Int64
		IL_00db: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Ray>(object)
		IL_00e0: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_00e5: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00ea: conv.i8
		IL_00eb: ldc.i8 -57
		IL_00f4: add
		IL_00f5: call bool [UnityEngine.PhysicsModule]UnityEngine.Physics::Raycast(valuetype [UnityEngine.CoreModule]UnityEngine.Ray, valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit&)
		IL_00fa: stloc.s 13
		IL_00fc: ldloc.s 13
		IL_00fe: stloc.3
		IL_00ff: ldloc.s 13
		IL_0101: ldc.i4.0
		IL_0102: ceq
		IL_0104: brtrue IL_0a35

		IL_0109: nop
		IL_010a: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_010f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0114: conv.i8
		IL_0115: ldc.i8 -57
		IL_011e: add
		IL_011f: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_0124: stloc.s 14
		IL_0126: ldloc.s 14
		IL_0128: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_012d: stloc.s 15
		IL_012f: ldloc.s 15
		IL_0131: ldstr "Pickup_EnergyFlavoredZestyBar"
		IL_0136: call bool [mscorlib]System.String::Equals(string, string)
		IL_013b: stloc.s 16
		IL_013d: ldarg.0
		IL_013e: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PickupScript::player
		IL_0143: stloc.s 17
		IL_0145: ldloc.s 16
		IL_0147: stloc.s 18
		IL_0149: ldloc.s 17
		IL_014b: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0150: stloc.s 19
		IL_0152: ldarg.0
		IL_0153: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0158: stloc.s 20
		IL_015a: ldloca.s 19
		IL_015c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0161: stloc.s 21
		IL_0163: ldloca.s 19
		IL_0165: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_016a: stloc.s 22
		IL_016c: ldloca.s 19
		IL_016e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0173: stloc.s 23
		IL_0175: ldloc.s 20
		IL_0177: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_017c: stloc.s 24
		IL_017e: ldloc.s 21
		IL_0180: ldloca.s 24
		IL_0182: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0187: sub
		IL_0188: stloc.s 21
		IL_018a: ldloc.s 22
		IL_018c: ldloca.s 24
		IL_018e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0193: sub
		IL_0194: stloc.s 22
		IL_0196: ldloc.s 23
		IL_0198: ldloca.s 24
		IL_019a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_019f: sub
		IL_01a0: stloc.s 23
		IL_01a2: br.s IL_01a5

		IL_01a4: nop

		IL_01a5: nop
		IL_01a6: ldstr "cvtps2pd xmm1,xmm6"
		IL_01ab: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01b0: stloc.s 25
		IL_01b2: ldc.r4 0.0
		IL_01b7: ldstr "cvtps2pd xmm1,xmm6"
		IL_01bc: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01c1: box [mscorlib]System.Int64
		IL_01c6: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_01cb: cgt
		IL_01cd: brtrue.s IL_01de

		IL_01cf: nop
		IL_01d0: ldstr "sqrtsd xmm0,xmm1"
		IL_01d5: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01da: stloc.s 26
		IL_01dc: br.s IL_020d

		IL_01de: nop
		IL_01df: ldstr "sub_20C340"
		IL_01e4: ldc.i8 0
		IL_01ed: ldloc.s 17
		IL_01ef: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_01f4: ldc.i8 0
		IL_01fd: ldc.i8 0
		IL_0206: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_020b: stloc.s 27

		IL_020d: nop
		IL_020e: ldc.r4 10
		IL_0213: ldloc.s 26
		IL_0215: conv.r4
		IL_0216: cgt
		IL_0218: brfalse.s IL_0220

		IL_021a: ldloc.s 18
		IL_021c: stloc.s 28
		IL_021e: br.s IL_0224

		IL_0220: nop
		IL_0221: ldc.i4.0
		IL_0222: stloc.s 28

		IL_0224: nop
		IL_0225: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_022a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_022f: conv.i8
		IL_0230: ldc.i8 -57
		IL_0239: add
		IL_023a: stloc.s 29
		IL_023c: ldloc.s 26
		IL_023e: conv.r4
		IL_023f: stloc.s 30
		IL_0241: ldloc.s 28
		IL_0243: ldc.i4.0
		IL_0244: ceq
		IL_0246: ldc.i4.0
		IL_0247: ceq
		IL_0249: brtrue IL_0b96

		IL_024e: nop
		IL_024f: ldloc.s 29
		IL_0251: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_0256: stloc.s 31
		IL_0258: ldloc.s 31
		IL_025a: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_025f: stloc.s 32
		IL_0261: ldloc.s 32
		IL_0263: ldstr "Pickup_YellowDoorLock"
		IL_0268: call bool [mscorlib]System.String::Equals(string, string)
		IL_026d: stloc.s 33
		IL_026f: ldarg.0
		IL_0270: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PickupScript::player
		IL_0275: stloc.s 17
		IL_0277: ldloc.s 17
		IL_0279: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_027e: stloc.s 34
		IL_0280: ldarg.0
		IL_0281: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0286: stloc.s 35
		IL_0288: ldloc.s 35
		IL_028a: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_028f: stloc.s 36
		IL_0291: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0296: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_029b: conv.i8
		IL_029c: ldc.i8 -89
		IL_02a5: add
		IL_02a6: box [mscorlib]System.Int64
		IL_02ab: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_02b0: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_02b5: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_02ba: conv.i8
		IL_02bb: ldc.i8 -105
		IL_02c4: add
		IL_02c5: box [mscorlib]System.Int64
		IL_02ca: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_02cf: call float32 [UnityEngine.CoreModule]UnityEngine.Vector3::Distance(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_02d4: stloc.s 37
		IL_02d6: ldc.r4 10
		IL_02db: ldloc.s 37
		IL_02dd: cgt
		IL_02df: brfalse.s IL_02e7

		IL_02e1: ldloc.s 33
		IL_02e3: stloc.s 38
		IL_02e5: br.s IL_02eb

		IL_02e7: nop
		IL_02e8: ldc.i4.0
		IL_02e9: stloc.s 38

		IL_02eb: nop
		IL_02ec: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_02f1: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_02f6: conv.i8
		IL_02f7: ldc.i8 -57
		IL_0300: add
		IL_0301: stloc.s 29
		IL_0303: ldloc.s 37
		IL_0305: stloc.s 30
		IL_0307: ldloc.s 38
		IL_0309: ldc.i4.0
		IL_030a: ceq
		IL_030c: ldc.i4.0
		IL_030d: ceq
		IL_030f: brtrue IL_0b6e

		IL_0314: nop
		IL_0315: ldloc.s 29
		IL_0317: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_031c: stloc.s 39
		IL_031e: ldloc.s 39
		IL_0320: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_0325: stloc.s 40
		IL_0327: ldloc.s 40
		IL_0329: ldstr "Pickup_Key"
		IL_032e: call bool [mscorlib]System.String::Equals(string, string)
		IL_0333: stloc.s 41
		IL_0335: ldarg.0
		IL_0336: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PickupScript::player
		IL_033b: stloc.s 17
		IL_033d: ldloc.s 17
		IL_033f: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0344: stloc.s 42
		IL_0346: ldarg.0
		IL_0347: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_034c: stloc.s 43
		IL_034e: ldloc.s 43
		IL_0350: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0355: stloc.s 44
		IL_0357: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_035c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0361: conv.i8
		IL_0362: ldc.i8 -105
		IL_036b: add
		IL_036c: box [mscorlib]System.Int64
		IL_0371: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0376: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_037b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0380: conv.i8
		IL_0381: ldc.i8 -89
		IL_038a: add
		IL_038b: box [mscorlib]System.Int64
		IL_0390: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0395: call float32 [UnityEngine.CoreModule]UnityEngine.Vector3::Distance(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_039a: stloc.s 45
		IL_039c: ldc.r4 10
		IL_03a1: ldloc.s 45
		IL_03a3: cgt
		IL_03a5: brfalse.s IL_03ad

		IL_03a7: ldloc.s 41
		IL_03a9: stloc.s 46
		IL_03ab: br.s IL_03b1

		IL_03ad: nop
		IL_03ae: ldc.i4.0
		IL_03af: stloc.s 46

		IL_03b1: nop
		IL_03b2: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_03b7: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_03bc: conv.i8
		IL_03bd: ldc.i8 -57
		IL_03c6: add
		IL_03c7: stloc.s 29
		IL_03c9: ldloc.s 45
		IL_03cb: stloc.s 30
		IL_03cd: ldloc.s 46
		IL_03cf: ldc.i4.0
		IL_03d0: ceq
		IL_03d2: ldc.i4.0
		IL_03d3: ceq
		IL_03d5: brtrue IL_0b46

		IL_03da: nop
		IL_03db: ldloc.s 29
		IL_03dd: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_03e2: stloc.s 47
		IL_03e4: ldloc.s 47
		IL_03e6: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_03eb: stloc.s 48
		IL_03ed: ldloc.s 48
		IL_03ef: ldstr "Pickup_BSODA"
		IL_03f4: call bool [mscorlib]System.String::Equals(string, string)
		IL_03f9: stloc.s 49
		IL_03fb: ldarg.0
		IL_03fc: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PickupScript::player
		IL_0401: stloc.s 17
		IL_0403: ldloc.s 17
		IL_0405: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_040a: stloc.s 50
		IL_040c: ldarg.0
		IL_040d: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0412: stloc.s 51
		IL_0414: ldloc.s 51
		IL_0416: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_041b: stloc.s 52
		IL_041d: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0422: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0427: conv.i8
		IL_0428: ldc.i8 -105
		IL_0431: add
		IL_0432: box [mscorlib]System.Int64
		IL_0437: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_043c: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0441: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0446: conv.i8
		IL_0447: ldc.i8 -89
		IL_0450: add
		IL_0451: box [mscorlib]System.Int64
		IL_0456: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_045b: call float32 [UnityEngine.CoreModule]UnityEngine.Vector3::Distance(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0460: stloc.s 53
		IL_0462: ldc.r4 10
		IL_0467: ldloc.s 53
		IL_0469: cgt
		IL_046b: brfalse.s IL_0473

		IL_046d: ldloc.s 49
		IL_046f: stloc.s 54
		IL_0471: br.s IL_0477

		IL_0473: nop
		IL_0474: ldc.i4.0
		IL_0475: stloc.s 54

		IL_0477: nop
		IL_0478: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_047d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0482: conv.i8
		IL_0483: ldc.i8 -57
		IL_048c: add
		IL_048d: stloc.s 29
		IL_048f: ldloc.s 53
		IL_0491: stloc.s 30
		IL_0493: ldloc.s 54
		IL_0495: ldc.i4.0
		IL_0496: ceq
		IL_0498: ldc.i4.0
		IL_0499: ceq
		IL_049b: brtrue IL_0b1e

		IL_04a0: nop
		IL_04a1: ldloc.s 29
		IL_04a3: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_04a8: stloc.s 55
		IL_04aa: ldloc.s 55
		IL_04ac: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_04b1: stloc.s 56
		IL_04b3: ldloc.s 56
		IL_04b5: ldstr "Pickup_Quarter"
		IL_04ba: call bool [mscorlib]System.String::Equals(string, string)
		IL_04bf: stloc.s 57
		IL_04c1: ldarg.0
		IL_04c2: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PickupScript::player
		IL_04c7: stloc.s 17
		IL_04c9: ldloc.s 17
		IL_04cb: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_04d0: stloc.s 58
		IL_04d2: ldarg.0
		IL_04d3: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_04d8: stloc.s 59
		IL_04da: ldloc.s 59
		IL_04dc: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_04e1: stloc.s 60
		IL_04e3: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_04e8: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_04ed: conv.i8
		IL_04ee: ldc.i8 -105
		IL_04f7: add
		IL_04f8: box [mscorlib]System.Int64
		IL_04fd: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0502: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0507: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_050c: conv.i8
		IL_050d: ldc.i8 -89
		IL_0516: add
		IL_0517: box [mscorlib]System.Int64
		IL_051c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0521: call float32 [UnityEngine.CoreModule]UnityEngine.Vector3::Distance(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0526: stloc.s 61
		IL_0528: ldc.r4 10
		IL_052d: ldloc.s 61
		IL_052f: cgt
		IL_0531: brfalse.s IL_0539

		IL_0533: ldloc.s 57
		IL_0535: stloc.s 62
		IL_0537: br.s IL_053d

		IL_0539: nop
		IL_053a: ldc.i4.0
		IL_053b: stloc.s 62

		IL_053d: nop
		IL_053e: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0543: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0548: conv.i8
		IL_0549: ldc.i8 -57
		IL_0552: add
		IL_0553: stloc.s 29
		IL_0555: ldloc.s 61
		IL_0557: stloc.s 30
		IL_0559: ldloc.s 62
		IL_055b: ldc.i4.0
		IL_055c: ceq
		IL_055e: ldc.i4.0
		IL_055f: ceq
		IL_0561: brtrue IL_0af6

		IL_0566: nop
		IL_0567: ldloc.s 29
		IL_0569: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_056e: stloc.s 63
		IL_0570: ldloc.s 63
		IL_0572: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_0577: stloc.s 64
		IL_0579: ldloc.s 64
		IL_057b: ldstr "Pickup_Tape"
		IL_0580: call bool [mscorlib]System.String::Equals(string, string)
		IL_0585: stloc.s 65
		IL_0587: ldarg.0
		IL_0588: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PickupScript::player
		IL_058d: stloc.s 17
		IL_058f: ldloc.s 17
		IL_0591: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0596: stloc.s 66
		IL_0598: ldarg.0
		IL_0599: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_059e: stloc.s 67
		IL_05a0: ldloc.s 67
		IL_05a2: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_05a7: stloc.s 68
		IL_05a9: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_05ae: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_05b3: conv.i8
		IL_05b4: ldc.i8 -105
		IL_05bd: add
		IL_05be: box [mscorlib]System.Int64
		IL_05c3: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_05c8: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_05cd: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_05d2: conv.i8
		IL_05d3: ldc.i8 -89
		IL_05dc: add
		IL_05dd: box [mscorlib]System.Int64
		IL_05e2: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_05e7: call float32 [UnityEngine.CoreModule]UnityEngine.Vector3::Distance(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_05ec: stloc.s 69
		IL_05ee: ldc.r4 10
		IL_05f3: ldloc.s 69
		IL_05f5: cgt
		IL_05f7: brfalse.s IL_05ff

		IL_05f9: ldloc.s 65
		IL_05fb: stloc.s 70
		IL_05fd: br.s IL_0603

		IL_05ff: nop
		IL_0600: ldc.i4.0
		IL_0601: stloc.s 70

		IL_0603: nop
		IL_0604: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0609: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_060e: conv.i8
		IL_060f: ldc.i8 -57
		IL_0618: add
		IL_0619: stloc.s 29
		IL_061b: ldloc.s 69
		IL_061d: stloc.s 30
		IL_061f: ldloc.s 70
		IL_0621: ldc.i4.0
		IL_0622: ceq
		IL_0624: ldc.i4.0
		IL_0625: ceq
		IL_0627: brtrue IL_0ace

		IL_062c: nop
		IL_062d: ldloc.s 29
		IL_062f: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_0634: stloc.s 71
		IL_0636: ldloc.s 71
		IL_0638: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_063d: stloc.s 72
		IL_063f: ldloc.s 72
		IL_0641: ldstr "Pickup_AlarmClock"
		IL_0646: call bool [mscorlib]System.String::Equals(string, string)
		IL_064b: stloc.s 73
		IL_064d: ldarg.0
		IL_064e: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PickupScript::player
		IL_0653: stloc.s 17
		IL_0655: ldloc.s 17
		IL_0657: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_065c: stloc.s 74
		IL_065e: ldarg.0
		IL_065f: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0664: stloc.s 75
		IL_0666: ldloc.s 75
		IL_0668: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_066d: stloc.s 76
		IL_066f: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0674: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0679: conv.i8
		IL_067a: ldc.i8 -105
		IL_0683: add
		IL_0684: box [mscorlib]System.Int64
		IL_0689: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_068e: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0693: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0698: conv.i8
		IL_0699: ldc.i8 -89
		IL_06a2: add
		IL_06a3: box [mscorlib]System.Int64
		IL_06a8: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_06ad: call float32 [UnityEngine.CoreModule]UnityEngine.Vector3::Distance(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_06b2: stloc.s 77
		IL_06b4: ldc.r4 10
		IL_06b9: ldloc.s 77
		IL_06bb: cgt
		IL_06bd: brfalse.s IL_06c5

		IL_06bf: ldloc.s 73
		IL_06c1: stloc.s 78
		IL_06c3: br.s IL_06c9

		IL_06c5: nop
		IL_06c6: ldc.i4.0
		IL_06c7: stloc.s 78

		IL_06c9: nop
		IL_06ca: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_06cf: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_06d4: conv.i8
		IL_06d5: ldc.i8 -57
		IL_06de: add
		IL_06df: stloc.s 29
		IL_06e1: ldloc.s 77
		IL_06e3: stloc.s 30
		IL_06e5: ldloc.s 78
		IL_06e7: ldc.i4.0
		IL_06e8: ceq
		IL_06ea: ldc.i4.0
		IL_06eb: ceq
		IL_06ed: brtrue IL_0aa6

		IL_06f2: nop
		IL_06f3: ldloc.s 29
		IL_06f5: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_06fa: stloc.s 79
		IL_06fc: ldloc.s 79
		IL_06fe: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_0703: stloc.s 80
		IL_0705: ldloc.s 80
		IL_0707: ldstr "Pickup_WD-3D"
		IL_070c: call bool [mscorlib]System.String::Equals(string, string)
		IL_0711: stloc.s 81
		IL_0713: ldarg.0
		IL_0714: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PickupScript::player
		IL_0719: stloc.s 17
		IL_071b: ldloc.s 17
		IL_071d: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0722: stloc.s 82
		IL_0724: ldarg.0
		IL_0725: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_072a: stloc.s 83
		IL_072c: ldloc.s 83
		IL_072e: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0733: stloc.s 84
		IL_0735: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_073a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_073f: conv.i8
		IL_0740: ldc.i8 -105
		IL_0749: add
		IL_074a: box [mscorlib]System.Int64
		IL_074f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0754: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0759: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_075e: conv.i8
		IL_075f: ldc.i8 -89
		IL_0768: add
		IL_0769: box [mscorlib]System.Int64
		IL_076e: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0773: call float32 [UnityEngine.CoreModule]UnityEngine.Vector3::Distance(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0778: stloc.s 85
		IL_077a: ldc.r4 10
		IL_077f: ldloc.s 85
		IL_0781: cgt
		IL_0783: brfalse.s IL_078b

		IL_0785: ldloc.s 81
		IL_0787: stloc.s 86
		IL_0789: br.s IL_078f

		IL_078b: nop
		IL_078c: ldc.i4.0
		IL_078d: stloc.s 86

		IL_078f: nop
		IL_0790: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0795: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_079a: conv.i8
		IL_079b: ldc.i8 -57
		IL_07a4: add
		IL_07a5: stloc.s 29
		IL_07a7: ldloc.s 85
		IL_07a9: stloc.s 30
		IL_07ab: ldloc.s 86
		IL_07ad: ldc.i4.0
		IL_07ae: ceq
		IL_07b0: ldc.i4.0
		IL_07b1: ceq
		IL_07b3: brtrue IL_0a81

		IL_07b8: nop
		IL_07b9: ldloc.s 29
		IL_07bb: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_07c0: stloc.s 87
		IL_07c2: ldloc.s 87
		IL_07c4: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_07c9: stloc.s 88
		IL_07cb: ldloc.s 88
		IL_07cd: ldstr "Pickup_SafetyScissors"
		IL_07d2: call bool [mscorlib]System.String::Equals(string, string)
		IL_07d7: stloc.s 89
		IL_07d9: ldarg.0
		IL_07da: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PickupScript::player
		IL_07df: stloc.s 17
		IL_07e1: ldloc.s 17
		IL_07e3: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_07e8: stloc.s 90
		IL_07ea: ldarg.0
		IL_07eb: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_07f0: stloc.s 91
		IL_07f2: ldloc.s 91
		IL_07f4: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_07f9: stloc.s 92
		IL_07fb: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0800: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0805: conv.i8
		IL_0806: ldc.i8 -105
		IL_080f: add
		IL_0810: box [mscorlib]System.Int64
		IL_0815: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_081a: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_081f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0824: conv.i8
		IL_0825: ldc.i8 -89
		IL_082e: add
		IL_082f: box [mscorlib]System.Int64
		IL_0834: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0839: call float32 [UnityEngine.CoreModule]UnityEngine.Vector3::Distance(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_083e: stloc.s 93
		IL_0840: ldc.r4 10
		IL_0845: ldloc.s 93
		IL_0847: cgt
		IL_0849: brfalse.s IL_0851

		IL_084b: ldloc.s 89
		IL_084d: stloc.s 94
		IL_084f: br.s IL_0855

		IL_0851: nop
		IL_0852: ldc.i4.0
		IL_0853: stloc.s 94

		IL_0855: nop
		IL_0856: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_085b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0860: conv.i8
		IL_0861: ldc.i8 -57
		IL_086a: add
		IL_086b: stloc.s 29
		IL_086d: ldloc.s 93
		IL_086f: stloc.s 30
		IL_0871: ldloc.s 94
		IL_0873: ldc.i4.0
		IL_0874: ceq
		IL_0876: ldc.i4.0
		IL_0877: ceq
		IL_0879: brtrue IL_0a5c

		IL_087e: nop
		IL_087f: ldloc.s 29
		IL_0881: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_0886: stloc.s 95
		IL_0888: ldloc.s 95
		IL_088a: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_088f: stloc.s 96
		IL_0891: ldloc.s 96
		IL_0893: ldstr "Pickup_BigBoots"
		IL_0898: call bool [mscorlib]System.String::Equals(string, string)
		IL_089d: stloc.s 97
		IL_089f: ldarg.0
		IL_08a0: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PickupScript::player
		IL_08a5: stloc.s 17
		IL_08a7: ldloc.s 17
		IL_08a9: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_08ae: stloc.s 98
		IL_08b0: ldarg.0
		IL_08b1: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_08b6: stloc.s 99
		IL_08b8: ldloc.s 99
		IL_08ba: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_08bf: stloc.s 100
		IL_08c1: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_08c6: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_08cb: conv.i8
		IL_08cc: ldc.i8 -105
		IL_08d5: add
		IL_08d6: box [mscorlib]System.Int64
		IL_08db: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_08e0: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_08e5: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_08ea: conv.i8
		IL_08eb: ldc.i8 -89
		IL_08f4: add
		IL_08f5: box [mscorlib]System.Int64
		IL_08fa: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_08ff: call float32 [UnityEngine.CoreModule]UnityEngine.Vector3::Distance(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0904: stloc.s 101
		IL_0906: ldc.r4 10
		IL_090b: ldloc.s 101
		IL_090d: cgt
		IL_090f: brfalse.s IL_0917

		IL_0911: ldloc.s 97
		IL_0913: stloc.s 102
		IL_0915: br.s IL_091b

		IL_0917: nop
		IL_0918: ldc.i4.0
		IL_0919: stloc.s 102

		IL_091b: nop
		IL_091c: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0921: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0926: conv.i8
		IL_0927: ldc.i8 -57
		IL_0930: add
		IL_0931: stloc.s 29
		IL_0933: ldloc.s 101
		IL_0935: stloc.s 30
		IL_0937: ldloc.s 102
		IL_0939: ldc.i4.0
		IL_093a: ceq
		IL_093c: ldc.i4.0
		IL_093d: ceq
		IL_093f: brtrue IL_0a37

		IL_0944: nop
		IL_0945: ldloc.s 29
		IL_0947: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_094c: stloc.s 103
		IL_094e: ldloc.s 103
		IL_0950: stloc.s 104
		IL_0952: ldloc.s 104
		IL_0954: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_0959: stloc.s 105
		IL_095b: ldloc.s 105
		IL_095d: ldstr "Pickup_Teleporter"
		IL_0962: call bool [mscorlib]System.String::Equals(string, string)
		IL_0967: stloc.s 106
		IL_0969: ldloc.s 106
		IL_096b: stloc.s 107
		IL_096d: ldarg.0
		IL_096e: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PickupScript::player
		IL_0973: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0978: stloc.s 108
		IL_097a: ldarg.0
		IL_097b: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0980: stloc.s 109
		IL_0982: ldloc.s 109
		IL_0984: stloc.s 110
		IL_0986: ldloc.s 110
		IL_0988: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_098d: stloc.s 111
		IL_098f: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0994: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0999: conv.i8
		IL_099a: ldc.i8 -105
		IL_09a3: add
		IL_09a4: box [mscorlib]System.Int64
		IL_09a9: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_09ae: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_09b3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_09b8: conv.i8
		IL_09b9: ldc.i8 -89
		IL_09c2: add
		IL_09c3: box [mscorlib]System.Int64
		IL_09c8: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_09cd: call float32 [UnityEngine.CoreModule]UnityEngine.Vector3::Distance(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_09d2: stloc.s 112
		IL_09d4: ldc.r4 10
		IL_09d9: ldloc.s 112
		IL_09db: cgt
		IL_09dd: brfalse.s IL_09e5

		IL_09df: ldloc.s 106
		IL_09e1: stloc.s 113
		IL_09e3: br.s IL_09e9

		IL_09e5: nop
		IL_09e6: ldc.i4.0
		IL_09e7: stloc.s 113

		IL_09e9: nop
		IL_09ea: ldloc.s 112
		IL_09ec: stloc.s 30
		IL_09ee: ldloc.s 113
		IL_09f0: ldc.i4.0
		IL_09f1: ceq
		IL_09f3: brtrue.s IL_0a35

		IL_09f5: nop
		IL_09f6: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_09fb: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0a00: conv.i8
		IL_0a01: ldc.i8 -57
		IL_0a0a: add
		IL_0a0b: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_0a10: stloc.s 114
		IL_0a12: ldloc.s 114
		IL_0a14: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_0a19: stloc.s 115
		IL_0a1b: ldloc.s 115
		IL_0a1d: ldc.i4.0
		IL_0a1e: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0a23: ldarg.0
		IL_0a24: ldfld class GameControllerScript PickupScript::gc
		IL_0a29: stloc.s 116

		IL_0a2b: nop
		IL_0a2c: ldloc.s 116
		IL_0a2e: ldc.i4.s 11
		IL_0a30: callvirt instance void GameControllerScript::CollectItem(int32)

		IL_0a35: nop
		IL_0a36: ret

		IL_0a37: nop
		IL_0a38: ldloc.s 116
		IL_0a3a: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_0a3f: stloc.s 117
		IL_0a41: ldloc.s 117
		IL_0a43: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_0a48: stloc.s 118
		IL_0a4a: ldloc.s 118
		IL_0a4c: ldc.i4.0
		IL_0a4d: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0a52: ldarg.0
		IL_0a53: ldfld class GameControllerScript PickupScript::gc
		IL_0a58: stloc.s 116
		IL_0a5a: br.s IL_0a2b

		IL_0a5c: nop
		IL_0a5d: ldloc.s 116
		IL_0a5f: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_0a64: stloc.s 119
		IL_0a66: ldloc.s 119
		IL_0a68: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_0a6d: stloc.s 120
		IL_0a6f: ldloc.s 120
		IL_0a71: ldc.i4.0
		IL_0a72: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0a77: ldarg.0
		IL_0a78: ldfld class GameControllerScript PickupScript::gc
		IL_0a7d: stloc.s 116
		IL_0a7f: br.s IL_0a2b

		IL_0a81: nop
		IL_0a82: ldloc.s 116
		IL_0a84: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_0a89: stloc.s 121
		IL_0a8b: ldloc.s 121
		IL_0a8d: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_0a92: stloc.s 122
		IL_0a94: ldloc.s 122
		IL_0a96: ldc.i4.0
		IL_0a97: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0a9c: ldarg.0
		IL_0a9d: ldfld class GameControllerScript PickupScript::gc
		IL_0aa2: stloc.s 116
		IL_0aa4: br.s IL_0a2b

		IL_0aa6: nop
		IL_0aa7: ldloc.s 116
		IL_0aa9: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_0aae: stloc.s 123
		IL_0ab0: ldloc.s 123
		IL_0ab2: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_0ab7: stloc.s 124
		IL_0ab9: ldloc.s 124
		IL_0abb: ldc.i4.0
		IL_0abc: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0ac1: ldarg.0
		IL_0ac2: ldfld class GameControllerScript PickupScript::gc
		IL_0ac7: stloc.s 116
		IL_0ac9: br IL_0a2b

		IL_0ace: nop
		IL_0acf: ldloc.s 116
		IL_0ad1: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_0ad6: stloc.s 125
		IL_0ad8: ldloc.s 125
		IL_0ada: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_0adf: stloc.s 126
		IL_0ae1: ldloc.s 126
		IL_0ae3: ldc.i4.0
		IL_0ae4: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0ae9: ldarg.0
		IL_0aea: ldfld class GameControllerScript PickupScript::gc
		IL_0aef: stloc.s 116
		IL_0af1: br IL_0a2b

		IL_0af6: nop
		IL_0af7: ldloc.s 116
		IL_0af9: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_0afe: stloc.s 127
		IL_0b00: ldloc.s 127
		IL_0b02: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_0b07: stloc.s 128
		IL_0b09: ldloc.s 128
		IL_0b0b: ldc.i4.0
		IL_0b0c: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0b11: ldarg.0
		IL_0b12: ldfld class GameControllerScript PickupScript::gc
		IL_0b17: stloc.s 116
		IL_0b19: br IL_0a2b

		IL_0b1e: nop
		IL_0b1f: ldloc.s 116
		IL_0b21: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_0b26: stloc.s 129
		IL_0b28: ldloc.s 129
		IL_0b2a: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_0b2f: stloc.s 130
		IL_0b31: ldloc.s 130
		IL_0b33: ldc.i4.0
		IL_0b34: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0b39: ldarg.0
		IL_0b3a: ldfld class GameControllerScript PickupScript::gc
		IL_0b3f: stloc.s 116
		IL_0b41: br IL_0a2b

		IL_0b46: nop
		IL_0b47: ldloc.s 116
		IL_0b49: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_0b4e: stloc.s 131
		IL_0b50: ldloc.s 131
		IL_0b52: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_0b57: stloc.s 132
		IL_0b59: ldloc.s 132
		IL_0b5b: ldc.i4.0
		IL_0b5c: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0b61: ldarg.0
		IL_0b62: ldfld class GameControllerScript PickupScript::gc
		IL_0b67: stloc.s 116
		IL_0b69: br IL_0a2b

		IL_0b6e: nop
		IL_0b6f: ldloc.s 116
		IL_0b71: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_0b76: stloc.s 133
		IL_0b78: ldloc.s 133
		IL_0b7a: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_0b7f: stloc.s 134
		IL_0b81: ldloc.s 134
		IL_0b83: ldc.i4.0
		IL_0b84: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0b89: ldarg.0
		IL_0b8a: ldfld class GameControllerScript PickupScript::gc
		IL_0b8f: stloc.s 116
		IL_0b91: br IL_0a2b

		IL_0b96: nop
		IL_0b97: ldloc.s 116
		IL_0b99: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_0b9e: stloc.s 135
		IL_0ba0: ldloc.s 135
		IL_0ba2: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_0ba7: stloc.s 136
		IL_0ba9: ldloc.s 136
		IL_0bab: ldc.i4.0
		IL_0bac: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0bb1: ldarg.0
		IL_0bb2: ldfld class GameControllerScript PickupScript::gc
		IL_0bb7: stloc.s 116
		IL_0bb9: br IL_0a2b

		IL_0bbe: nop
		IL_0bbf: ldnull
		IL_0bc0: throw
	} // end of method PickupScript::Update

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x268d
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method PickupScript::.ctor

} // end of class PickupScript
