.class public auto ansi beforefieldinit UiManager
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class [UnityEngine.UI]UnityEngine.UI.CanvasScaler normScaler
	.field public class MaterialKit.DpCanvasScaler dpiScaler
	.field public class [UnityEngine.CoreModule]UnityEngine.RectTransform[] transforms

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0xa80c
		// Header size: 12
		// Code size: 506 (0x1fa)
		.maxstack 5
		.locals init (
			[0] int32,
			[1] int32,
			[2] int32,
			[3] int32,
			[4] int32,
			[5] class [UnityEngine.CoreModule]UnityEngine.RectTransform[],
			[6] class [UnityEngine.CoreModule]UnityEngine.RectTransform,
			[7] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[8] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[9] int32,
			[10] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[11] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[12] class [UnityEngine.CoreModule]UnityEngine.RectTransform,
			[13] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[14] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[15] int32,
			[16] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[17] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3
		)

		IL_0000: nop
		IL_0001: ldstr "UiSize"
		IL_0006: call int32 [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::GetInt(string)
		IL_000b: stloc.0
		IL_000c: ldstr "UiHeight"
		IL_0011: call int32 [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::GetInt(string)
		IL_0016: stloc.1
		IL_0017: ldloc.1
		IL_0018: stloc.2
		IL_0019: ldloc.0
		IL_001a: stloc.3
		IL_001b: ldloc.1
		IL_001c: stloc.s 4
		IL_001e: ldloc.0
		IL_001f: ldc.i4.1
		IL_0020: ceq
		IL_0022: brtrue.s IL_0045

		IL_0024: nop
		IL_0025: ldloc.3
		IL_0026: ldc.i4.2
		IL_0027: ceq
		IL_0029: brtrue.s IL_0042

		IL_002b: nop
		IL_002c: ldloc.3
		IL_002d: ldc.i4.3
		IL_002e: ceq
		IL_0030: brtrue.s IL_003f

		IL_0032: nop
		IL_0033: ldloc.3
		IL_0034: ldc.i4.4
		IL_0035: ceq
		IL_0037: ldc.i4.0
		IL_0038: ceq
		IL_003a: brtrue.s IL_0066

		IL_003c: nop
		IL_003d: br.s IL_0046

		IL_003f: nop
		IL_0040: br.s IL_0046

		IL_0042: nop
		IL_0043: br.s IL_0046

		IL_0045: nop

		IL_0046: nop
		IL_0047: ldarg.0
		IL_0048: ldfld class [UnityEngine.UI]UnityEngine.UI.CanvasScaler UiManager::normScaler
		IL_004d: ldstr "unpcklps xmm0,xmm1"
		IL_0052: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0057: box [mscorlib]System.Int64
		IL_005c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector2>(object)
		IL_0061: callvirt instance void [UnityEngine.UI]UnityEngine.UI.CanvasScaler::set_referenceResolution(valuetype [UnityEngine.CoreModule]UnityEngine.Vector2)

		IL_0066: nop
		IL_0067: ldloc.2
		IL_0068: ldc.i4.1
		IL_0069: ceq
		IL_006b: brtrue IL_0115

		IL_0070: nop
		IL_0071: ldloc.2
		IL_0072: ldc.i4.2
		IL_0073: ceq
		IL_0075: ldc.i4.0
		IL_0076: ceq
		IL_0078: brtrue IL_01f2

		IL_007d: nop
		IL_007e: ldarg.0
		IL_007f: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform[] UiManager::transforms
		IL_0084: stloc.s 5
		// loop start (head: IL_0086)
			IL_0086: nop
			IL_0087: ldc.i4.0
			IL_0088: ldloc.s 5
			IL_008a: ldlen
			IL_008b: conv.i4
			IL_008c: clt
			IL_008e: ldc.i4.0
			IL_008f: ceq
			IL_0091: brtrue IL_01f2

			IL_0096: nop
			IL_0097: ldloc.s 5
			IL_0099: ldc.i4.0
			IL_009a: conv.i8
			IL_009b: conv.i4
			IL_009c: ldelem [UnityEngine.CoreModule]UnityEngine.RectTransform
			IL_00a1: stloc.s 6
			IL_00a3: ldloc.s 6
			IL_00a5: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
			IL_00aa: stloc.s 7
			IL_00ac: ldloc.s 6
			IL_00ae: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
			IL_00b3: stloc.s 8
			IL_00b5: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_height()
			IL_00ba: stloc.s 9
			IL_00bc: ldloc.s 6
			IL_00be: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
			IL_00c3: stloc.s 10
			IL_00c5: ldloc.s 6
			IL_00c7: ldloca.s 11
			IL_00c9: ldloca.s 7
			IL_00cb: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_00d0: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_00d5: ldloca.s 11
			IL_00d7: ldloc.s 9
			IL_00d9: ldstr "rdx"
			IL_00de: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_00e3: conv.i4
			IL_00e4: ldc.i4.3
			IL_00e5: and
			IL_00e6: add
			IL_00e7: ldc.i4.2
			IL_00e8: shr
			IL_00e9: conv.r4
			IL_00ea: ldloca.s 8
			IL_00ec: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_00f1: add
			IL_00f2: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_00f7: ldloca.s 11
			IL_00f9: ldloca.s 10
			IL_00fb: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_0100: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_0105: ldloc.s 11
			IL_0107: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
			IL_010c: ldc.i4.0
			IL_010d: ldc.i4.1
			IL_010e: add
			IL_010f: stloc.3
			IL_0110: br IL_0086
		// end loop

		IL_0115: nop
		IL_0116: ldloc.s 5
		IL_0118: ldc.i4.2
		IL_0119: ldelem [UnityEngine.CoreModule]UnityEngine.RectTransform
		IL_011e: stloc.s 12
		// loop start (head: IL_0120)
			IL_0120: nop
			IL_0121: ldc.i4.0
			IL_0122: ldstr "*(LocalExpr + 0x18)"
			IL_0127: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_012c: conv.i4
			IL_012d: clt
			IL_012f: ldc.i4.0
			IL_0130: ceq
			IL_0132: brtrue IL_01f2

			IL_0137: nop
			IL_0138: ldstr "*(LocalExpr + 0x20)"
			IL_013d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0142: box [mscorlib]System.Int64
			IL_0147: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [UnityEngine.CoreModule]UnityEngine.Transform>(object)
			IL_014c: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
			IL_0151: stloc.s 13
			IL_0153: ldstr "*(LocalExpr + 0x20)"
			IL_0158: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_015d: box [mscorlib]System.Int64
			IL_0162: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [UnityEngine.CoreModule]UnityEngine.Transform>(object)
			IL_0167: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
			IL_016c: stloc.s 14
			IL_016e: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_height()
			IL_0173: stloc.s 15
			IL_0175: ldstr "*(LocalExpr + 0x20)"
			IL_017a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_017f: box [mscorlib]System.Int64
			IL_0184: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [UnityEngine.CoreModule]UnityEngine.Transform>(object)
			IL_0189: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
			IL_018e: stloc.s 16
			IL_0190: ldstr "*(LocalExpr + 0x20)"
			IL_0195: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_019a: box [mscorlib]System.Int64
			IL_019f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [UnityEngine.CoreModule]UnityEngine.Transform>(object)
			IL_01a4: ldloca.s 17
			IL_01a6: ldloca.s 13
			IL_01a8: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_01ad: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_01b2: ldloca.s 17
			IL_01b4: ldloc.s 15
			IL_01b6: ldstr "rdx"
			IL_01bb: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_01c0: conv.i4
			IL_01c1: ldc.i4.7
			IL_01c2: and
			IL_01c3: add
			IL_01c4: ldc.i4.3
			IL_01c5: shr
			IL_01c6: conv.r4
			IL_01c7: ldloca.s 14
			IL_01c9: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_01ce: add
			IL_01cf: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_01d4: ldloca.s 17
			IL_01d6: ldloca.s 16
			IL_01d8: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_01dd: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_01e2: ldloc.s 17
			IL_01e4: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
			IL_01e9: ldc.i4.0
			IL_01ea: ldc.i4.1
			IL_01eb: add
			IL_01ec: stloc.3
			IL_01ed: br IL_0120
		// end loop

		IL_01f2: nop
		IL_01f3: ret

		IL_01f4: nop
		IL_01f5: ldnull
		IL_01f6: throw

		IL_01f7: nop
		IL_01f8: ldnull
		IL_01f9: throw
	} // end of method UiManager::Start

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x23b7
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method UiManager::.ctor

} // end of class UiManager
