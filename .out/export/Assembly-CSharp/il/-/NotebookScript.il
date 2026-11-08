.class public auto ansi beforefieldinit NotebookScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public float32 openingDistance
	.field public class GameControllerScript gc
	.field public class BaldiScript bsc
	.field public float32 respawnTime
	.field public bool up
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform player
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject learningGame
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioSource audioDevice

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x2671
		// Header size: 1
		// Code size: 9 (0x9)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i4.1
		IL_0003: stfld bool NotebookScript::up
		IL_0008: ret
	} // end of method NotebookScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x110e0
		// Header size: 12
		// Code size: 1191 (0x4a7)
		.maxstack 5
		.locals init (
			[0] class GameControllerScript,
			[1] bool,
			[2] float32,
			[3] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[4] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[5] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[6] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[7] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[8] class [UnityEngine.AudioModule]UnityEngine.AudioSource,
			[9] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[10] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[11] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[12] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[13] float32,
			[14] float32,
			[15] float32,
			[16] class InputManager,
			[17] class InputManager,
			[18] bool,
			[19] bool,
			[20] float32,
			[21] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[22] int32,
			[23] int32,
			[24] float32,
			[25] valuetype [UnityEngine.CoreModule]UnityEngine.Ray,
			[26] int64,
			[27] int64,
			[28] bool,
			[29] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[30] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[31] string,
			[32] bool,
			[33] int32,
			[34] bool,
			[35] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[36] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[37] float32,
			[38] float32,
			[39] float32,
			[40] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[41] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[42] int64,
			[43] int64,
			[44] int64,
			[45] int64,
			[46] int32,
			[47] float32,
			[48] float32,
			[49] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[50] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[51] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[52] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[53] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[54] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[55] class GameControllerScript,
			[56] class GameControllerScript,
			[57] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[58] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[59] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[60] class MathGameScript,
			[61] class MathGameScript,
			[62] class GameControllerScript,
			[63] class MathGameScript,
			[64] class BaldiScript,
			[65] class MathGameScript,
			[66] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[67] int64
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class GameControllerScript NotebookScript::gc
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: ldfld string GameControllerScript::mode
		IL_000e: ldstr "endless"
		IL_0013: call bool [mscorlib]System.String::Equals(string, string)
		IL_0018: stloc.1
		IL_0019: ldloc.1
		IL_001a: ldc.i4.0
		IL_001b: ceq
		IL_001d: brtrue IL_0120

		IL_0022: nop
		IL_0023: ldarg.0
		IL_0024: ldfld float32 NotebookScript::respawnTime
		IL_0029: stloc.2
		IL_002a: ldarg.0
		IL_002b: ldfld float32 NotebookScript::respawnTime
		IL_0030: ldc.r4 0.0
		IL_0035: cgt
		IL_0037: brtrue.s IL_00b2

		IL_0039: nop
		IL_003a: ldarg.0
		IL_003b: ldfld bool NotebookScript::up
		IL_0040: ldc.i4.0
		IL_0041: ceq
		IL_0043: ldc.i4.0
		IL_0044: ceq
		IL_0046: brtrue IL_0120

		IL_004b: nop
		IL_004c: ldarg.0
		IL_004d: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0052: stloc.3
		IL_0053: ldarg.0
		IL_0054: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0059: stloc.s 4
		IL_005b: ldloc.s 4
		IL_005d: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0062: stloc.s 5
		IL_0064: ldarg.0
		IL_0065: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_006a: stloc.s 6
		IL_006c: ldloc.s 6
		IL_006e: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0073: stloc.s 7
		IL_0075: ldloc.3
		IL_0076: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_007b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0080: conv.i8
		IL_0081: ldc.i8 -105
		IL_008a: add
		IL_008b: box [mscorlib]System.Int64
		IL_0090: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0095: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_009a: ldarg.0
		IL_009b: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource NotebookScript::audioDevice
		IL_00a0: stloc.s 8
		IL_00a2: ldarg.0
		IL_00a3: ldc.i4.1
		IL_00a4: stfld bool NotebookScript::up
		IL_00a9: ldloc.s 8
		IL_00ab: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Play()
		IL_00b0: br.s IL_0120

		IL_00b2: nop
		IL_00b3: ldarg.0
		IL_00b4: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_00b9: stloc.s 9
		IL_00bb: ldloc.s 9
		IL_00bd: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_00c2: stloc.s 10
		IL_00c4: ldarg.0
		IL_00c5: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform NotebookScript::player
		IL_00ca: stloc.s 11
		IL_00cc: ldloc.s 11
		IL_00ce: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_00d3: stloc.s 12
		IL_00d5: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_00da: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00df: conv.i8
		IL_00e0: ldc.i8 -105
		IL_00e9: add
		IL_00ea: call instance float32 [UnityEngine.CoreModule]UnityEngine.Vector3::get_magnitude()
		IL_00ef: stloc.s 13
		IL_00f1: ldloc.s 13
		IL_00f3: stloc.2
		IL_00f4: ldloc.s 13
		IL_00f6: ldc.r4 60
		IL_00fb: cgt
		IL_00fd: ldc.i4.0
		IL_00fe: ceq
		IL_0100: brtrue.s IL_0120

		IL_0102: nop
		IL_0103: ldarg.0
		IL_0104: ldfld float32 NotebookScript::respawnTime
		IL_0109: stloc.s 14
		IL_010b: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0110: stloc.s 15
		IL_0112: ldarg.0
		IL_0113: ldloc.s 14
		IL_0115: ldloc.s 15
		IL_0117: sub
		IL_0118: stfld float32 NotebookScript::respawnTime
		IL_011d: ldloc.s 15
		IL_011f: stloc.2

		IL_0120: nop
		IL_0121: call !0 class Singleton`1<class InputManager>::get_Instance()
		IL_0126: stloc.s 16
		IL_0128: ldloc.s 16
		IL_012a: stloc.s 17
		IL_012c: ldloc.s 17
		IL_012e: ldc.i4.4
		IL_012f: box [mscorlib]System.Int32
		IL_0134: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
		IL_0139: callvirt instance bool InputManager::GetActionKey(valuetype InputAction)
		IL_013e: stloc.s 18
		IL_0140: ldloc.s 18
		IL_0142: stloc.s 19
		IL_0144: ldloc.s 18
		IL_0146: ldc.i4.0
		IL_0147: ceq
		IL_0149: brtrue IL_04a2

		IL_014e: nop
		IL_014f: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_timeScale()
		IL_0154: stloc.s 20
		IL_0156: ldloc.s 20
		IL_0158: stloc.2
		IL_0159: nop
		IL_015a: ldloc.s 20
		IL_015c: ldc.r4 0.0
		IL_0161: ceq
		IL_0163: brtrue IL_04a2

		IL_0168: nop
		IL_0169: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_016e: stloc.s 21
		IL_0170: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_width()
		IL_0175: stloc.s 22
		IL_0177: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_height()
		IL_017c: stloc.s 23
		IL_017e: ldloc.s 22
		IL_0180: ldstr "rdx"
		IL_0185: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_018a: conv.i4
		IL_018b: sub
		IL_018c: ldc.i4.1
		IL_018d: shr
		IL_018e: conv.r4
		IL_018f: stloc.s 24
		IL_0191: ldloc.s 23
		IL_0193: ldstr "rdx"
		IL_0198: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_019d: conv.i4
		IL_019e: sub
		IL_019f: ldc.i4.1
		IL_01a0: shr
		IL_01a1: conv.r4
		IL_01a2: stloc.2
		IL_01a3: ldloc.s 21
		IL_01a5: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_01aa: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01af: conv.i8
		IL_01b0: ldc.i8 -105
		IL_01b9: add
		IL_01ba: box [mscorlib]System.Int64
		IL_01bf: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_01c4: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Ray [UnityEngine.CoreModule]UnityEngine.Camera::ScreenPointToRay(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_01c9: stloc.s 25
		IL_01cb: ldstr "struct_chunk(LocalExpr+0x0, 16)"
		IL_01d0: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01d5: stloc.s 26
		IL_01d7: ldstr "struct_chunk(LocalExpr+0x10, 8)"
		IL_01dc: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01e1: stloc.s 27
		IL_01e3: br.s IL_01e6

		IL_01e5: nop

		IL_01e6: nop
		IL_01e7: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_01ec: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01f1: conv.i8
		IL_01f2: ldc.i8 -89
		IL_01fb: add
		IL_01fc: box [mscorlib]System.Int64
		IL_0201: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Ray>(object)
		IL_0206: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_020b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0210: conv.i8
		IL_0211: ldc.i8 -57
		IL_021a: add
		IL_021b: call bool [UnityEngine.PhysicsModule]UnityEngine.Physics::Raycast(valuetype [UnityEngine.CoreModule]UnityEngine.Ray, valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit&)
		IL_0220: stloc.s 28
		IL_0222: ldloc.s 28
		IL_0224: stloc.s 19
		IL_0226: ldloc.s 28
		IL_0228: ldc.i4.0
		IL_0229: ceq
		IL_022b: brtrue IL_04a2

		IL_0230: nop
		IL_0231: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0236: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_023b: conv.i8
		IL_023c: ldc.i8 -57
		IL_0245: add
		IL_0246: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_024b: stloc.s 29
		IL_024d: ldloc.s 29
		IL_024f: stloc.s 30
		IL_0251: ldloc.s 30
		IL_0253: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Component::get_tag()
		IL_0258: stloc.s 31
		IL_025a: ldloc.s 31
		IL_025c: ldstr "Notebook"
		IL_0261: call bool [mscorlib]System.String::Equals(string, string)
		IL_0266: stloc.s 32
		IL_0268: ldarg.0
		IL_0269: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform NotebookScript::player
		IL_026e: stloc.s 11
		IL_0270: ldloc.s 32
		IL_0272: stloc.s 33
		IL_0274: ldloc.s 32
		IL_0276: stloc.s 34
		IL_0278: ldloc.s 11
		IL_027a: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_027f: stloc.s 35
		IL_0281: ldarg.0
		IL_0282: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0287: stloc.s 36
		IL_0289: ldloca.s 35
		IL_028b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0290: stloc.s 37
		IL_0292: ldloca.s 35
		IL_0294: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0299: stloc.s 38
		IL_029b: ldloca.s 35
		IL_029d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_02a2: stloc.s 39
		IL_02a4: ldloc.s 36
		IL_02a6: stloc.s 40
		IL_02a8: ldloc.s 40
		IL_02aa: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_02af: stloc.s 41
		IL_02b1: ldloc.s 37
		IL_02b3: ldloca.s 41
		IL_02b5: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_02ba: sub
		IL_02bb: stloc.s 37
		IL_02bd: ldloc.s 38
		IL_02bf: ldloca.s 41
		IL_02c1: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_02c6: sub
		IL_02c7: stloc.s 38
		IL_02c9: ldloc.s 39
		IL_02cb: ldloca.s 41
		IL_02cd: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_02d2: sub
		IL_02d3: stloc.s 39
		IL_02d5: br.s IL_02d8

		IL_02d7: nop

		IL_02d8: nop
		IL_02d9: ldstr "cvtps2pd xmm1,xmm6"
		IL_02de: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_02e3: stloc.s 42
		IL_02e5: ldc.r4 0.0
		IL_02ea: ldstr "cvtps2pd xmm1,xmm6"
		IL_02ef: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_02f4: box [mscorlib]System.Int64
		IL_02f9: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_02fe: cgt
		IL_0300: brtrue.s IL_0311

		IL_0302: nop
		IL_0303: ldstr "sqrtsd xmm0,xmm1"
		IL_0308: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_030d: stloc.s 43
		IL_030f: br.s IL_0344

		IL_0311: nop
		IL_0312: ldstr "sub_20C340"
		IL_0317: ldc.i8 0
		IL_0320: ldloc.s 11
		IL_0322: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0327: ldc.i8 0
		IL_0330: ldc.i8 0
		IL_0339: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_033e: stloc.s 44
		IL_0340: ldloc.s 44
		IL_0342: stloc.s 45

		IL_0344: nop
		IL_0345: ldarg.0
		IL_0346: ldfld float32 NotebookScript::openingDistance
		IL_034b: ldloc.s 43
		IL_034d: conv.r4
		IL_034e: cgt
		IL_0350: brfalse.s IL_0358

		IL_0352: ldloc.s 33
		IL_0354: stloc.s 46
		IL_0356: br.s IL_035c

		IL_0358: nop
		IL_0359: ldc.i4.0
		IL_035a: stloc.s 46

		IL_035c: nop
		IL_035d: ldarg.0
		IL_035e: ldfld float32 NotebookScript::openingDistance
		IL_0363: stloc.s 47
		IL_0365: ldloc.s 43
		IL_0367: conv.r4
		IL_0368: stloc.s 48
		IL_036a: ldloc.s 46
		IL_036c: ldc.i4.0
		IL_036d: ceq
		IL_036f: brtrue IL_04a2

		IL_0374: nop
		IL_0375: ldarg.0
		IL_0376: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_037b: stloc.s 49
		IL_037d: ldarg.0
		IL_037e: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0383: stloc.s 50
		IL_0385: ldloc.s 49
		IL_0387: stloc.s 51
		IL_0389: ldloc.s 50
		IL_038b: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0390: stloc.s 52
		IL_0392: ldarg.0
		IL_0393: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0398: stloc.s 53
		IL_039a: ldloc.s 53
		IL_039c: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_03a1: stloc.s 54
		IL_03a3: ldloc.s 51
		IL_03a5: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_03aa: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_03af: conv.i8
		IL_03b0: ldc.i8 -105
		IL_03b9: add
		IL_03ba: box [mscorlib]System.Int64
		IL_03bf: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_03c4: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_03c9: ldarg.0
		IL_03ca: ldfld class GameControllerScript NotebookScript::gc
		IL_03cf: stloc.s 55
		IL_03d1: ldarg.0
		IL_03d2: ldc.i4.0
		IL_03d3: stfld bool NotebookScript::up
		IL_03d8: ldarg.0
		IL_03d9: ldc.r4 120
		IL_03de: stfld float32 NotebookScript::respawnTime
		IL_03e3: ldloc.s 55
		IL_03e5: stloc.s 56
		IL_03e7: ldloc.s 56
		IL_03e9: callvirt instance void GameControllerScript::CollectNotebook()
		IL_03ee: ldarg.0
		IL_03ef: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject NotebookScript::learningGame
		IL_03f4: stloc.s 57
		IL_03f6: br.s IL_03f9

		IL_03f8: nop

		IL_03f9: nop
		IL_03fa: ldloc.s 57
		IL_03fc: call !!0 [UnityEngine.CoreModule]UnityEngine.Object::Instantiate<class [UnityEngine.CoreModule]UnityEngine.GameObject>(!!0)
		IL_0401: stloc.s 58
		IL_0403: ldloc.s 58
		IL_0405: stloc.s 59
		IL_0407: ldloc.s 59
		IL_0409: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::GetComponent<class MathGameScript>()
		IL_040e: stloc.s 60
		IL_0410: ldloc.s 60
		IL_0412: stloc.s 61
		IL_0414: ldarg.0
		IL_0415: ldfld class GameControllerScript NotebookScript::gc
		IL_041a: stloc.s 62
		IL_041c: ldloc.s 61
		IL_041e: ldloc.s 62
		IL_0420: stfld class GameControllerScript MathGameScript::gc
		IL_0425: ldloc.s 58
		IL_0427: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::GetComponent<class MathGameScript>()
		IL_042c: stloc.s 63
		IL_042e: ldloc.s 63
		IL_0430: stloc.s 61
		IL_0432: ldarg.0
		IL_0433: ldfld class BaldiScript NotebookScript::bsc
		IL_0438: stloc.s 64
		IL_043a: ldloc.s 61
		IL_043c: ldloc.s 64
		IL_043e: stfld class BaldiScript MathGameScript::baldiScript
		IL_0443: ldloc.s 58
		IL_0445: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::GetComponent<class MathGameScript>()
		IL_044a: stloc.s 65
		IL_044c: ldloc.s 65
		IL_044e: stloc.s 61
		IL_0450: ldarg.0
		IL_0451: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform NotebookScript::player
		IL_0456: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_045b: stloc.s 66
		IL_045d: ldloc.s 65
		IL_045f: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 MathGameScript::playerPosition
		IL_0464: ldloca.s 66
		IL_0466: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_046b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0470: ldloc.s 65
		IL_0472: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 MathGameScript::playerPosition
		IL_0477: ldloca.s 66
		IL_0479: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_047e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0483: ldloc.s 65
		IL_0485: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 MathGameScript::playerPosition
		IL_048a: ldloca.s 66
		IL_048c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0491: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0496: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_049b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_04a0: stloc.s 67

		IL_04a2: nop
		IL_04a3: ret

		IL_04a4: nop
		IL_04a5: ldnull
		IL_04a6: throw
	} // end of method NotebookScript::Update

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x267b
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method NotebookScript::.ctor

} // end of class NotebookScript
