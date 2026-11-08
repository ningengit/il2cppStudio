.class public auto ansi beforefieldinit ControlsMenu
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested private auto ansi sealed beforefieldinit '<RebindCoroutine>d__15'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerator`1<object>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private object '<>2__current'
		.field public class ControlsMenu '<>4__this'
		.field public float32 duration
		.field private string '<actionName>5__2'
		.field private float32 '<remainingTime>5__3'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x2c77
			// Header size: 1
			// Code size: 16 (0x10)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ldarg.0
			IL_0009: ldarg.1
			IL_000a: stfld int32 ControlsMenu/'<RebindCoroutine>d__15'::'<>1__state'
			IL_000f: ret
		} // end of method '<RebindCoroutine>d__15'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			// Method begins at RVA 0x2c88
			// Header size: 1
			// Code size: 2 (0x2)
			.maxstack 8

			IL_0000: nop
			IL_0001: ret
		} // end of method '<RebindCoroutine>d__15'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			// Method begins at RVA 0x1a690
			// Header size: 12
			// Code size: 892 (0x37c)
			.maxstack 5
			.locals init (
				[0] int32,
				[1] class ControlsMenu,
				[2] int64,
				[3] float32,
				[4] float32,
				[5] class [Unity.TextMeshPro]TMPro.TMP_Text,
				[6] string,
				[7] int32,
				[8] object,
				[9] string,
				[10] class [Unity.TextMeshPro]TMPro.TMP_Text,
				[11] string,
				[12] string,
				[13] bool,
				[14] valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode[],
				[15] valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode,
				[16] bool,
				[17] valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode,
				[18] int32,
				[19] int32,
				[20] bool,
				[21] int64,
				[22] int64,
				[23] class InputManager,
				[24] int32,
				[25] class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>,
				[26] bool,
				[27] int32,
				[28] int64,
				[29] int64,
				[30] bool,
				[31] int32
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld int32 ControlsMenu/'<RebindCoroutine>d__15'::'<>1__state'
			IL_0007: stloc.0
			IL_0008: ldarg.0
			IL_0009: ldfld class ControlsMenu ControlsMenu/'<RebindCoroutine>d__15'::'<>4__this'
			IL_000e: stloc.1
			IL_000f: nop
			IL_0010: ldloc.0
			IL_0011: ldc.i4.1
			IL_0012: sub
			IL_0013: stloc.0
			IL_0014: nop
			IL_0015: ldloc.0
			IL_0016: ldc.i4.1
			IL_0017: sub
			IL_0018: stloc.0
			IL_0019: ldloc.0
			IL_001a: ldc.i4.1
			IL_001b: sub
			IL_001c: ldc.i4.0
			IL_001d: ceq
			IL_001f: brtrue.s IL_005b

			IL_0021: nop
			IL_0022: ldloc.0
			IL_0023: ldc.i4.1
			IL_0024: ceq
			IL_0026: brtrue.s IL_005b

			IL_0028: nop
			IL_0029: ldstr "sub_251368"
			IL_002e: ldc.i8 0
			IL_0037: ldc.i8 0
			IL_0040: ldc.i8 0
			IL_0049: ldc.i8 0
			IL_0052: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
			IL_0057: stloc.2
			IL_0058: ldloc.2
			IL_0059: conv.i4
			IL_005a: ret

			IL_005b: nop
			IL_005c: ldarg.0
			IL_005d: ldfld float32 ControlsMenu/'<RebindCoroutine>d__15'::'<remainingTime>5__3'
			IL_0062: stloc.3
			IL_0063: ldarg.0
			IL_0064: ldc.i4.m1
			IL_0065: stfld int32 ControlsMenu/'<RebindCoroutine>d__15'::'<>1__state'
			IL_006a: ldloc.3
			IL_006b: stloc.s 4
			IL_006d: nop
			IL_006e: nop
			IL_006f: ldloc.1
			IL_0070: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text ControlsMenu::counterText
			IL_0075: stloc.s 5
			IL_0077: ldarg.0
			IL_0078: ldfld string ControlsMenu/'<RebindCoroutine>d__15'::'<actionName>5__2'
			IL_007d: stloc.s 6
			IL_007f: ldloc.s 4
			IL_0081: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::CeilToInt(float32)
			IL_0086: stloc.s 7
			IL_0088: ldloc.s 7
			IL_008a: box [mscorlib]System.Int32
			IL_008f: stloc.s 8
			IL_0091: ldstr "<size=16>{0}<size=21>\nPress a key to assign it to {1}\n\n\n\n\n\n{2}"
			IL_0096: ldloc.s 6
			IL_0098: ldloc.s 6
			IL_009a: ldloc.s 8
			IL_009c: call string [mscorlib]System.String::Format(string, object, object, object)
			IL_00a1: stloc.s 9
			IL_00a3: ldloc.s 5
			IL_00a5: stloc.s 10
			IL_00a7: ldloc.s 9
			IL_00a9: stloc.s 11
			IL_00ab: nop
			IL_00ac: ldloc.s 11
			IL_00ae: stloc.s 12
			IL_00b0: ldloc.1
			IL_00b1: ldfld bool ControlsMenu::isBindingMouseInput
			IL_00b6: ldc.i4.0
			IL_00b7: ceq
			IL_00b9: ldc.i4.0
			IL_00ba: ceq
			IL_00bc: brtrue.s IL_0139

			IL_00be: nop
			IL_00bf: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::get_anyKeyDown()
			IL_00c4: stloc.s 13
			IL_00c6: nop
			IL_00c7: ldloc.1
			IL_00c8: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode[] ControlsMenu::allKeyCodes
			IL_00cd: stloc.s 14
			IL_00cf: nop
			// loop start (head: IL_00d0)
				IL_00d0: nop
				IL_00d1: nop
				IL_00d2: nop
				IL_00d3: ldloc.s 14
				IL_00d5: ldc.i4.0
				IL_00d6: conv.i8
				IL_00d7: conv.i4
				IL_00d8: ldelem [UnityEngine.CoreModule]UnityEngine.KeyCode
				IL_00dd: stloc.s 15
				IL_00df: ldloc.s 14
				IL_00e1: ldc.i4.0
				IL_00e2: conv.i8
				IL_00e3: conv.i4
				IL_00e4: ldelem [UnityEngine.CoreModule]UnityEngine.KeyCode
				IL_00e9: box [mscorlib]System.Object
				IL_00ee: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
				IL_00f3: box [mscorlib]System.Int32
				IL_00f8: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
				IL_00fd: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetKeyDownInt(valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode)
				IL_0102: stloc.s 16
				IL_0104: ldloc.s 15
				IL_0106: stloc.s 17
				IL_0108: ldloc.s 16
				IL_010a: ldc.i4.0
				IL_010b: ceq
				IL_010d: brtrue.s IL_012c

				IL_010f: nop
				IL_0110: ldloc.s 17
				IL_0112: box [mscorlib]System.Object
				IL_0117: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
				IL_011c: ldc.i8 -323
				IL_0125: add
				IL_0126: conv.i4
				IL_0127: ldc.i4.4
				IL_0128: cgt.un
				IL_012a: brtrue.s IL_01a0

				IL_012c: nop
				IL_012d: ldc.i4.0
				IL_012e: ldc.i4.1
				IL_012f: add
				IL_0130: stloc.s 18
				IL_0132: ldc.i4.0
				IL_0133: ldc.i4.1
				IL_0134: add
				IL_0135: stloc.s 19
				IL_0137: br.s IL_00d0
			// end loop

			IL_0139: nop
			// loop start (head: IL_013a)
				IL_013a: nop
				IL_013b: ldc.i4.0
				IL_013c: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetMouseButtonDown(int32)
				IL_0141: stloc.s 20
				IL_0143: ldloc.s 20
				IL_0145: ldc.i4.0
				IL_0146: ceq
				IL_0148: ldc.i4.0
				IL_0149: ceq
				IL_014b: brtrue.s IL_0190

				IL_014d: nop
				IL_014e: ldc.i4.0
				IL_014f: ldc.i4.1
				IL_0150: add
				IL_0151: stloc.s 19
				IL_0153: ldc.i4.0
				IL_0154: ldc.i4.1
				IL_0155: add
				IL_0156: ldc.i4.7
				IL_0157: clt
				IL_0159: brtrue.s IL_013a
			// end loop
			IL_015b: nop
			IL_015c: ldstr "sub_25132C"
			IL_0161: ldc.i8 0
			IL_016a: ldc.i8 0
			IL_0173: ldc.i8 0
			IL_017c: ldc.i8 0
			IL_0185: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
			IL_018a: stloc.s 21
			IL_018c: ldloc.s 21
			IL_018e: conv.i4
			IL_018f: ret

			IL_0190: nop
			IL_0191: ldloc.s 19
			IL_0193: conv.i8
			IL_0194: ldc.i8 323
			IL_019d: add
			IL_019e: stloc.s 22

			IL_01a0: nop
			IL_01a1: ldloc.1
			IL_01a2: ldfld bool ControlsMenu::isBindingMouseInput
			IL_01a7: ldc.i4.0
			IL_01a8: ceq
			IL_01aa: brtrue IL_0276

			IL_01af: nop
			IL_01b0: ldloc.s 22
			IL_01b2: ldc.i8 -323
			IL_01bb: add
			IL_01bc: conv.i4
			IL_01bd: ldc.i4.4
			IL_01be: cgt.un
			IL_01c0: brtrue IL_025f

			IL_01c5: nop
			IL_01c6: ldloc.1
			IL_01c7: ldfld class InputManager ControlsMenu::inputMan
			IL_01cc: stloc.s 23
			IL_01ce: nop
			IL_01cf: ldloc.1
			IL_01d0: ldfld int32 ControlsMenu::currentActionIndex
			IL_01d5: stloc.s 24
			IL_01d7: ldstr "global_19E3B38"
			IL_01dc: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_01e1: ldc.i4.0
			IL_01e2: conv.i8
			IL_01e3: ceq
			IL_01e5: ldc.i4.0
			IL_01e6: ceq
			IL_01e8: brtrue.s IL_01f5

			IL_01ea: nop
			IL_01eb: ldstr "mov byte ptr [1819E3B38h],1 ; value = 0x1"
			IL_01f0: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)

			IL_01f5: nop
			IL_01f6: ldloc.s 23
			IL_01f8: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
			IL_01fd: stloc.s 25
			IL_01ff: nop
			IL_0200: ldloc.s 25
			IL_0202: ldloc.s 24
			IL_0204: box [mscorlib]System.Int32
			IL_0209: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
			IL_020e: ldstr "&stack[0x8]"
			IL_0213: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0218: callvirt instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::TryGetValue(!0, !1&)
			IL_021d: stloc.s 26
			IL_021f: ldloc.s 26
			IL_0221: ldc.i4.0
			IL_0222: ceq
			IL_0224: ldc.i4.0
			IL_0225: ceq
			IL_0227: brtrue.s IL_022a

			IL_0229: nop

			IL_022a: nop
			IL_022b: ldloc.s 22
			IL_022d: conv.i4
			IL_022e: ldloc.s 22
			IL_0230: conv.i4
			IL_0231: and
			IL_0232: ldc.i4.0
			IL_0233: ceq
			IL_0235: ldc.i4.0
			IL_0236: ceq
			IL_0238: brfalse.s IL_0241

			IL_023a: ldloc.s 22
			IL_023c: conv.i4
			IL_023d: stloc.s 27
			IL_023f: br.s IL_024f

			IL_0241: nop
			IL_0242: ldstr "stack[0xC]"
			IL_0247: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_024c: conv.i4
			IL_024d: stloc.s 27

			IL_024f: nop
			IL_0250: ldloc.s 23
			IL_0252: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
			IL_0257: stloc.s 25
			IL_0259: nop
			IL_025a: br IL_033c

			IL_025f: nop
			IL_0260: ldstr "*(LocalExpr + 0x80)"
			IL_0265: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_026a: ldc.i4.0
			IL_026b: conv.i8
			IL_026c: ceq
			IL_026e: ldc.i4.0
			IL_026f: ceq
			IL_0271: brtrue IL_036e

			IL_0276: nop
			IL_0277: ldloc.s 22
			IL_0279: ldc.i8 -323
			IL_0282: add
			IL_0283: conv.i4
			IL_0284: ldc.i4.4
			IL_0285: cgt.un
			IL_0287: ldc.i4.0
			IL_0288: ceq
			IL_028a: brtrue IL_036e

			IL_028f: nop
			IL_0290: ldstr "*(LocalExpr + 0x88)"
			IL_0295: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_029a: stloc.s 28
			IL_029c: nop
			IL_029d: ldstr "*(LocalExpr + 0x68)"
			IL_02a2: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_02a7: conv.i4
			IL_02a8: stloc.s 24
			IL_02aa: ldstr "global_19E3B38"
			IL_02af: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_02b4: ldc.i4.0
			IL_02b5: conv.i8
			IL_02b6: ceq
			IL_02b8: ldc.i4.0
			IL_02b9: ceq
			IL_02bb: brtrue.s IL_02c8

			IL_02bd: nop
			IL_02be: ldstr "mov byte ptr [1819E3B38h],1 ; value = 0x1"
			IL_02c3: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)

			IL_02c8: nop
			IL_02c9: ldstr "*(LocalExpr + 0x20)"
			IL_02ce: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_02d3: stloc.s 29
			IL_02d5: nop
			IL_02d6: ldloc.s 29
			IL_02d8: box [mscorlib]System.Int64
			IL_02dd: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [mscorlib]System.Collections.Generic.Dictionary`2>(object)
			IL_02e2: ldloc.s 24
			IL_02e4: box [mscorlib]System.Int32
			IL_02e9: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
			IL_02ee: ldstr "&stack[0x8]"
			IL_02f3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_02f8: callvirt instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::TryGetValue(!0, !1&)
			IL_02fd: stloc.s 30
			IL_02ff: ldloc.s 30
			IL_0301: ldc.i4.0
			IL_0302: ceq
			IL_0304: ldc.i4.0
			IL_0305: ceq
			IL_0307: brtrue.s IL_030a

			IL_0309: nop

			IL_030a: nop
			IL_030b: ldloc.s 22
			IL_030d: conv.i4
			IL_030e: ldloc.s 22
			IL_0310: conv.i4
			IL_0311: and
			IL_0312: ldc.i4.0
			IL_0313: ceq
			IL_0315: ldc.i4.0
			IL_0316: ceq
			IL_0318: brfalse.s IL_0321

			IL_031a: ldloc.s 22
			IL_031c: conv.i4
			IL_031d: stloc.s 31
			IL_031f: br.s IL_032f

			IL_0321: nop
			IL_0322: ldstr "stack[0x8]"
			IL_0327: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_032c: conv.i4
			IL_032d: stloc.s 31

			IL_032f: nop
			IL_0330: ldstr "*(LocalExpr + 0x20)"
			IL_0335: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_033a: stloc.s 29

			IL_033c: nop
			IL_033d: ldloc.s 29
			IL_033f: box [mscorlib]System.Int64
			IL_0344: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [mscorlib]System.Collections.Generic.Dictionary`2>(object)
			IL_0349: ldloc.s 24
			IL_034b: box [mscorlib]System.Int32
			IL_0350: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
			IL_0355: ldstr "stack[0x8]"
			IL_035a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_035f: box [mscorlib]System.Int64
			IL_0364: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputBinding>(object)
			IL_0369: callvirt instance void class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::set_Item(!0, !1)

			IL_036e: nop
			IL_036f: ldarg.0
			IL_0370: ldc.r4 0.01
			IL_0375: stfld float32 ControlsMenu/'<RebindCoroutine>d__15'::'<remainingTime>5__3'
			IL_037a: ldc.i4.0
			IL_037b: ret
		} // end of method '<RebindCoroutine>d__15'::MoveNext

		.method private final hidebysig specialname newslot virtual 
			instance object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () cil managed 
		{
			// Method begins at RVA 0x2c8b
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object ControlsMenu/'<RebindCoroutine>d__15'::'<>2__current'
			IL_0007: ret
		} // end of method '<RebindCoroutine>d__15'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'

		.method private final hidebysig newslot virtual 
			instance void System.Collections.IEnumerator.Reset () cil managed 
		{
			// Method begins at RVA 0x1aa18
			// Header size: 12
			// Code size: 67 (0x43)
			.maxstack 5
			.locals init (
				[0] int64
			)

			IL_0000: nop
			IL_0001: ldstr "sub_184BE0"
			IL_0006: ldstr "rax"
			IL_000b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0010: ldc.i8 0
			IL_0019: ldc.i8 0
			IL_0022: ldc.i8 0
			IL_002b: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
			IL_0030: stloc.0
			IL_0031: ldloc.0
			IL_0032: box [mscorlib]System.Int64
			IL_0037: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [mscorlib]System.NotSupportedException>(object)
			IL_003c: callvirt instance void [mscorlib]System.NotSupportedException::.ctor()
			IL_0041: ldnull
			IL_0042: throw
		} // end of method '<RebindCoroutine>d__15'::System.Collections.IEnumerator.Reset

		.method private final hidebysig specialname newslot virtual 
			instance object System.Collections.IEnumerator.get_Current () cil managed 
		{
			// Method begins at RVA 0x2c94
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object ControlsMenu/'<RebindCoroutine>d__15'::'<>2__current'
			IL_0007: ret
		} // end of method '<RebindCoroutine>d__15'::System.Collections.IEnumerator.get_Current

		// Properties
		.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current'()
		{
			.get instance object ControlsMenu/'<RebindCoroutine>d__15'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'()
		}
		.property instance object System.Collections.IEnumerator.Current()
		{
			.get instance object ControlsMenu/'<RebindCoroutine>d__15'::System.Collections.IEnumerator.get_Current()
		}

	} // end of class <RebindCoroutine>d__15


	// Fields
	.field private class [UnityEngine.UI]UnityEngine.UI.Button[] allButtons
	.field private class [Unity.TextMeshPro]TMPro.TMP_Text[] buttonTexts
	.field private class [Unity.TextMeshPro]TMPro.TMP_Text[] secondaryButtonTexts
	.field private class [Unity.TextMeshPro]TMPro.TMP_Text[] labelTexts
	.field private class [UnityEngine.CoreModule]UnityEngine.GameObject panel
	.field private class [UnityEngine.CoreModule]UnityEngine.GameObject confirmationPanel
	.field private class [UnityEngine.CoreModule]UnityEngine.GameObject resetConfirmationPanel
	.field private class [Unity.TextMeshPro]TMPro.TMP_Text counterText
	.field private class [Unity.TextMeshPro]TMPro.TMP_Text confirmText
	.field private int32 currentActionIndex
	.field private class [UnityEngine.CoreModule]UnityEngine.Coroutine rebindCoroutine
	.field private valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode[] allKeyCodes
	.field private bool isBindingMouseInput
	.field private class InputManager inputMan

	// Methods
	.method private hidebysig 
		instance void Awake () cil managed 
	{
		// Method begins at RVA 0x8d40
		// Header size: 12
		// Code size: 115 (0x73)
		.maxstack 2
		.locals init (
			[0] class [mscorlib]System.Type,
			[1] class [mscorlib]System.Type,
			[2] class [mscorlib]System.Array,
			[3] class [mscorlib]System.Array,
			[4] class [mscorlib]System.Array,
			[5] valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode[],
			[6] class [mscorlib]System.Array,
			[7] valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode[],
			[8] valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode[]
		)

		IL_0000: nop
		IL_0001: br.s IL_0004

		IL_0003: nop

		IL_0004: nop
		IL_0005: ldstr "typeof(KeyCode)"
		IL_000a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000f: box [mscorlib]System.IntPtr
		IL_0014: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [mscorlib]System.RuntimeTypeHandle>(object)
		IL_0019: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		IL_001e: stloc.0
		IL_001f: ldloc.0
		IL_0020: stloc.1
		IL_0021: br.s IL_0024

		IL_0023: nop

		IL_0024: nop
		IL_0025: ldloc.1
		IL_0026: call class [mscorlib]System.Array [mscorlib]System.Enum::GetValues(class [mscorlib]System.Type)
		IL_002b: stloc.2
		IL_002c: ldloc.2
		IL_002d: stloc.3
		IL_002e: ldloc.2
		IL_002f: stloc.s 4
		IL_0031: ldloc.2
		IL_0032: ldnull
		IL_0033: ceq
		IL_0035: ldc.i4.0
		IL_0036: ceq
		IL_0038: brtrue.s IL_0045

		IL_003a: nop
		IL_003b: ldarg.0
		IL_003c: ldloc.s 4
		IL_003e: stfld valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode[] ControlsMenu::allKeyCodes
		IL_0043: br.s IL_006b

		IL_0045: nop
		IL_0046: ldloc.3
		IL_0047: isinst valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode[]
		IL_004c: stloc.s 5
		IL_004e: ldloc.3
		IL_004f: stloc.s 6
		IL_0051: ldloc.s 5
		IL_0053: stloc.s 7
		IL_0055: nop
		IL_0056: ldarg.0
		IL_0057: ldloc.s 7
		IL_0059: stfld valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode[] ControlsMenu::allKeyCodes
		IL_005e: ldloc.s 6
		IL_0060: isinst valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode[]
		IL_0065: stloc.s 8
		IL_0067: ldloc.s 8
		IL_0069: stloc.s 7

		IL_006b: nop
		IL_006c: ret

		IL_006d: nop
		IL_006e: ldnull
		IL_006f: throw

		IL_0070: nop
		IL_0071: ldnull
		IL_0072: throw
	} // end of method ControlsMenu::Awake

	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x8dc0
		// Header size: 12
		// Code size: 50 (0x32)
		.maxstack 2
		.locals init (
			[0] class InputManager,
			[1] class InputManager,
			[2] bool,
			[3] bool
		)

		IL_0000: nop
		IL_0001: call !0 class Singleton`1<class InputManager>::get_Instance()
		IL_0006: stloc.0
		IL_0007: ldarg.0
		IL_0008: ldloc.0
		IL_0009: stfld class InputManager ControlsMenu::inputMan
		IL_000e: ldarg.0
		IL_000f: ldfld class InputManager ControlsMenu::inputMan
		IL_0014: stloc.1
		IL_0015: br.s IL_0018

		IL_0017: nop

		IL_0018: nop
		IL_0019: ldloc.1
		IL_001a: ldnull
		IL_001b: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Inequality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0020: stloc.2
		IL_0021: ldloc.2
		IL_0022: stloc.3
		IL_0023: ldloc.2
		IL_0024: ldc.i4.0
		IL_0025: ceq
		IL_0027: brtrue.s IL_0030

		IL_0029: nop
		IL_002a: ldarg.0
		IL_002b: callvirt instance void ControlsMenu::UpdateAllButtonTexts()

		IL_0030: nop
		IL_0031: ret
	} // end of method ControlsMenu::Start

	.method private hidebysig 
		instance void OnEnable () cil managed 
	{
		// Method begins at RVA 0x8e00
		// Header size: 12
		// Code size: 156 (0x9c)
		.maxstack 2
		.locals init (
			[0] class InputManager,
			[1] bool,
			[2] class InputManager,
			[3] bool,
			[4] class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>,
			[5] int32,
			[6] object,
			[7] string,
			[8] string
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class InputManager ControlsMenu::inputMan
		IL_0007: stloc.0
		IL_0008: br.s IL_000b

		IL_000a: nop

		IL_000b: nop
		IL_000c: ldloc.0
		IL_000d: ldnull
		IL_000e: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Equality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0013: stloc.1
		IL_0014: ldloc.1
		IL_0015: ldc.i4.0
		IL_0016: ceq
		IL_0018: brtrue.s IL_0028

		IL_001a: nop
		IL_001b: call !0 class Singleton`1<class InputManager>::get_Instance()
		IL_0020: stloc.2
		IL_0021: ldarg.0
		IL_0022: ldloc.2
		IL_0023: stfld class InputManager ControlsMenu::inputMan

		IL_0028: nop
		IL_0029: ldarg.0
		IL_002a: ldfld class InputManager ControlsMenu::inputMan
		IL_002f: stloc.0
		IL_0030: br.s IL_0033

		IL_0032: nop

		IL_0033: nop
		IL_0034: ldloc.0
		IL_0035: ldnull
		IL_0036: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Equality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_003b: stloc.3
		IL_003c: ldloc.3
		IL_003d: ldc.i4.0
		IL_003e: ceq
		IL_0040: ldc.i4.0
		IL_0041: ceq
		IL_0043: brtrue.s IL_0089

		IL_0045: nop
		IL_0046: ldarg.0
		IL_0047: ldfld class InputManager ControlsMenu::inputMan
		IL_004c: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
		IL_0051: stloc.s 4
		IL_0053: ldloc.s 4
		IL_0055: callvirt instance int32 class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::get_Count()
		IL_005a: stloc.s 5
		IL_005c: ldloc.s 5
		IL_005e: box [mscorlib]System.Int32
		IL_0063: stloc.s 6
		IL_0065: ldstr "ControlsMenu: InputManager found. Mappings count: {0}"
		IL_006a: ldloc.s 6
		IL_006c: call string [mscorlib]System.String::Format(string, object)
		IL_0071: stloc.s 7
		IL_0073: ldloc.s 7
		IL_0075: stloc.s 8
		IL_0077: br.s IL_007a

		IL_0079: nop

		IL_007a: nop
		IL_007b: ldloc.s 8
		IL_007d: call void [UnityEngine.CoreModule]UnityEngine.Debug::Log(object)
		IL_0082: ldarg.0
		IL_0083: callvirt instance void ControlsMenu::UpdateAllButtonTexts()
		IL_0088: ret

		IL_0089: nop
		IL_008a: br.s IL_008d

		IL_008c: nop

		IL_008d: nop
		IL_008e: ldstr "ControlsMenu: InputManager not found! Is it in the scene?"
		IL_0093: call void [UnityEngine.CoreModule]UnityEngine.Debug::LogError(object)
		IL_0098: ret

		IL_0099: nop
		IL_009a: ldnull
		IL_009b: throw
	} // end of method ControlsMenu::OnEnable

	.method private hidebysig 
		instance void OnDisable () cil managed 
	{
		// Method begins at RVA 0x227b
		// Header size: 1
		// Code size: 16 (0x10)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class InputManager ControlsMenu::inputMan
		IL_0007: callvirt instance void InputManager::Save()
		IL_000c: ret

		IL_000d: nop
		IL_000e: ldnull
		IL_000f: throw
	} // end of method ControlsMenu::OnDisable

	.method public hidebysig 
		instance void UpdateAllButtonTexts () cil managed 
	{
		// Method begins at RVA 0x8ea8
		// Header size: 12
		// Code size: 410 (0x19a)
		.maxstack 3
		.locals init (
			[0] class [Unity.TextMeshPro]TMPro.TMP_Text[],
			[1] class [mscorlib]System.Type,
			[2] class [mscorlib]System.Type,
			[3] class [mscorlib]System.Array,
			[4] int32,
			[5] bool,
			[6] class [Unity.TextMeshPro]TMPro.TMP_Text[],
			[7] int64,
			[8] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[9] string,
			[10] string,
			[11] string,
			[12] string,
			[13] int32,
			[14] string,
			[15] string,
			[16] string,
			[17] int32,
			[18] class [Unity.TextMeshPro]TMPro.TMP_Text[],
			[19] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text[] ControlsMenu::labelTexts
		IL_0007: stloc.0
		// loop start (head: IL_0008)
			IL_0008: nop
			IL_0009: ldc.i4.0
			IL_000a: ldloc.0
			IL_000b: ldlen
			IL_000c: conv.i4
			IL_000d: clt
			IL_000f: ldc.i4.0
			IL_0010: ceq
			IL_0012: brtrue IL_0192

			IL_0017: nop
			IL_0018: br.s IL_001b

			IL_001a: nop

			IL_001b: nop
			IL_001c: ldstr "typeof(InputAction)"
			IL_0021: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0026: box [mscorlib]System.IntPtr
			IL_002b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [mscorlib]System.RuntimeTypeHandle>(object)
			IL_0030: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
			IL_0035: stloc.1
			IL_0036: ldloc.1
			IL_0037: stloc.2
			IL_0038: br.s IL_003b

			IL_003a: nop

			IL_003b: nop
			IL_003c: ldloc.2
			IL_003d: call class [mscorlib]System.Array [mscorlib]System.Enum::GetValues(class [mscorlib]System.Type)
			IL_0042: stloc.3
			IL_0043: ldloc.3
			IL_0044: callvirt instance int32 [mscorlib]System.Array::System.Collections.ICollection.get_Count()
			IL_0049: stloc.s 4
			IL_004b: ldc.i4.0
			IL_004c: ldloc.s 4
			IL_004e: clt
			IL_0050: ldc.i4.0
			IL_0051: ceq
			IL_0053: brtrue IL_017a

			IL_0058: nop
			IL_0059: ldarg.0
			IL_005a: ldfld class InputManager ControlsMenu::inputMan
			IL_005f: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
			IL_0064: ldc.i4.0
			IL_0065: box [mscorlib]System.Int32
			IL_006a: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
			IL_006f: ldstr "&stack[0x8]"
			IL_0074: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0079: callvirt instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::TryGetValue(!0, !1&)
			IL_007e: stloc.s 5
			IL_0080: ldloc.s 5
			IL_0082: ldc.i4.0
			IL_0083: ceq
			IL_0085: brtrue IL_017a

			IL_008a: nop
			IL_008b: ldarg.0
			IL_008c: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text[] ControlsMenu::buttonTexts
			IL_0091: stloc.s 6
			IL_0093: ldstr "stack[0x8]"
			IL_0098: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_009d: stloc.s 7
			IL_009f: ldloc.s 6
			IL_00a1: ldc.i4.0
			IL_00a2: conv.i8
			IL_00a3: conv.i4
			IL_00a4: ldelem [Unity.TextMeshPro]TMPro.TMP_Text
			IL_00a9: stloc.s 8
			IL_00ab: ldstr "stack[0x8]"
			IL_00b0: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_00b5: ldc.i8 -323
			IL_00be: add
			IL_00bf: conv.i4
			IL_00c0: ldc.i4.4
			IL_00c1: cgt.un
			IL_00c3: ldc.i4.0
			IL_00c4: ceq
			IL_00c6: brtrue.s IL_00e3

			IL_00c8: nop
			IL_00c9: ldarg.0
			IL_00ca: ldloc.s 7
			IL_00cc: box [mscorlib]System.Int64
			IL_00d1: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
			IL_00d6: callvirt instance string ControlsMenu::FormatKeyText(valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode)
			IL_00db: stloc.s 9
			IL_00dd: ldloc.s 9
			IL_00df: stloc.s 10
			IL_00e1: br.s IL_00fc

			IL_00e3: nop
			IL_00e4: ldarg.0
			IL_00e5: ldloc.s 7
			IL_00e7: box [mscorlib]System.Int64
			IL_00ec: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
			IL_00f1: callvirt instance string ControlsMenu::FormatMouseText(valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode)
			IL_00f6: stloc.s 11
			IL_00f8: ldloc.s 11
			IL_00fa: stloc.s 10

			IL_00fc: nop
			IL_00fd: ldloc.s 10
			IL_00ff: stloc.s 12
			IL_0101: ldarg.0
			IL_0102: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text[] ControlsMenu::secondaryButtonTexts
			IL_0107: stloc.s 6
			IL_0109: ldstr "stack[0xC]"
			IL_010e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0113: conv.i4
			IL_0114: stloc.s 13
			IL_0116: ldloc.s 6
			IL_0118: ldc.i4.0
			IL_0119: conv.i8
			IL_011a: conv.i4
			IL_011b: ldelem [Unity.TextMeshPro]TMPro.TMP_Text
			IL_0120: stloc.s 8
			IL_0122: ldstr "stack[0xC]"
			IL_0127: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_012c: conv.i4
			IL_012d: conv.i8
			IL_012e: ldc.i8 -323
			IL_0137: add
			IL_0138: conv.i4
			IL_0139: ldc.i4.4
			IL_013a: cgt.un
			IL_013c: ldc.i4.0
			IL_013d: ceq
			IL_013f: brtrue.s IL_015c

			IL_0141: nop
			IL_0142: ldarg.0
			IL_0143: ldloc.s 13
			IL_0145: box [mscorlib]System.Int32
			IL_014a: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
			IL_014f: callvirt instance string ControlsMenu::FormatKeyText(valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode)
			IL_0154: stloc.s 14
			IL_0156: ldloc.s 14
			IL_0158: stloc.s 10
			IL_015a: br.s IL_0175

			IL_015c: nop
			IL_015d: ldarg.0
			IL_015e: ldloc.s 13
			IL_0160: box [mscorlib]System.Int32
			IL_0165: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
			IL_016a: callvirt instance string ControlsMenu::FormatMouseText(valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode)
			IL_016f: stloc.s 15
			IL_0171: ldloc.s 15
			IL_0173: stloc.s 10

			IL_0175: nop
			IL_0176: ldloc.s 10
			IL_0178: stloc.s 16

			IL_017a: nop
			IL_017b: ldc.i4.0
			IL_017c: ldc.i4.1
			IL_017d: add
			IL_017e: stloc.s 17
			IL_0180: ldarg.0
			IL_0181: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text[] ControlsMenu::labelTexts
			IL_0186: stloc.s 18
			IL_0188: ldc.i4.0
			IL_0189: ldc.i4.1
			IL_018a: add
			IL_018b: stloc.s 19
			IL_018d: br IL_0008
		// end loop

		IL_0192: nop
		IL_0193: ret

		IL_0194: nop
		IL_0195: ldnull
		IL_0196: throw

		IL_0197: nop
		IL_0198: ldnull
		IL_0199: throw
	} // end of method ControlsMenu::UpdateAllButtonTexts

	.method private hidebysig 
		instance string FormatKeyText (
			valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode key
		) cil managed 
	{
		// Method begins at RVA 0x9050
		// Header size: 12
		// Code size: 180 (0xb4)
		.maxstack 2
		.locals init (
			[0] int32,
			[1] string
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: box [mscorlib]System.Object
		IL_0007: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_000c: stloc.0
		IL_000d: ldarg.1
		IL_000e: box [mscorlib]System.Object
		IL_0013: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0018: ldc.i4.0
		IL_0019: ceq
		IL_001b: ldc.i4.0
		IL_001c: ceq
		IL_001e: brtrue.s IL_0027

		IL_0020: nop
		IL_0021: ldstr ""
		IL_0026: ret

		IL_0027: nop
		IL_0028: ldloc.0
		IL_0029: ldc.i4.s 27
		IL_002b: ceq
		IL_002d: ldc.i4.0
		IL_002e: ceq
		IL_0030: brtrue.s IL_0039

		IL_0032: nop
		IL_0033: ldstr "ESC"
		IL_0038: ret

		IL_0039: nop
		IL_003a: ldloc.0
		IL_003b: conv.i8
		IL_003c: ldc.i8 -48
		IL_0045: add
		IL_0046: conv.i4
		IL_0047: ldc.i4.s 9
		IL_0049: cgt.un
		IL_004b: brtrue.s IL_0094

		IL_004d: nop
		IL_004e: nop
		IL_004f: ldstr "0"
		IL_0054: ret

		IL_0055: nop
		IL_0056: ldstr "1"
		IL_005b: ret

		IL_005c: nop
		IL_005d: ldstr "2"
		IL_0062: ret

		IL_0063: nop
		IL_0064: ldstr "3"
		IL_0069: ret

		IL_006a: nop
		IL_006b: ldstr "4"
		IL_0070: ret

		IL_0071: nop
		IL_0072: ldstr "5"
		IL_0077: ret

		IL_0078: nop
		IL_0079: ldstr "6"
		IL_007e: ret

		IL_007f: nop
		IL_0080: ldstr "7"
		IL_0085: ret

		IL_0086: nop
		IL_0087: ldstr "8"
		IL_008c: ret

		IL_008d: nop
		IL_008e: ldstr "9"
		IL_0093: ret

		IL_0094: nop
		IL_0095: ldstr "&stack[0x338]"
		IL_009a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_009f: box [mscorlib]System.IntPtr
		IL_00a4: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [mscorlib]System.Enum>(object)
		IL_00a9: callvirt instance string [mscorlib]System.Enum::ToString()
		IL_00ae: stloc.1
		IL_00af: ldloc.1
		IL_00b0: ret

		IL_00b1: nop
		IL_00b2: ldnull
		IL_00b3: ret
	} // end of method ControlsMenu::FormatKeyText

	.method private hidebysig 
		instance string FormatMouseText (
			valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode mouseKey
		) cil managed 
	{
		// Method begins at RVA 0x9110
		// Header size: 12
		// Code size: 160 (0xa0)
		.maxstack 3
		.locals init (
			[0] int64,
			[1] int32,
			[2] int32,
			[3] string
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: box [mscorlib]System.Object
		IL_0007: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_000c: conv.i8
		IL_000d: ldc.i8 -323
		IL_0016: add
		IL_0017: stloc.0
		IL_0018: ldarg.1
		IL_0019: box [mscorlib]System.Object
		IL_001e: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0023: stloc.1
		IL_0024: ldarg.1
		IL_0025: box [mscorlib]System.Object
		IL_002a: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_002f: conv.i8
		IL_0030: ldc.i8 -323
		IL_0039: add
		IL_003a: conv.i4
		IL_003b: ldarg.1
		IL_003c: box [mscorlib]System.Object
		IL_0041: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0046: conv.i8
		IL_0047: ldc.i8 -323
		IL_0050: add
		IL_0051: conv.i4
		IL_0052: and
		IL_0053: ldc.i4.0
		IL_0054: ceq
		IL_0056: brtrue.s IL_0099

		IL_0058: nop
		IL_0059: ldloc.0
		IL_005a: conv.i4
		IL_005b: ldc.i4.1
		IL_005c: sub
		IL_005d: stloc.2
		IL_005e: ldloc.0
		IL_005f: conv.i4
		IL_0060: ldc.i4.1
		IL_0061: sub
		IL_0062: ldc.i4.0
		IL_0063: ceq
		IL_0065: brtrue.s IL_0092

		IL_0067: nop
		IL_0068: ldloc.2
		IL_0069: ldc.i4.1
		IL_006a: ceq
		IL_006c: brtrue.s IL_008b

		IL_006e: nop
		IL_006f: ldstr "&stack[0xFFFFFFFFFFFFFFD8]"
		IL_0074: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0079: box [mscorlib]System.IntPtr
		IL_007e: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [mscorlib]System.Enum>(object)
		IL_0083: callvirt instance string [mscorlib]System.Enum::ToString()
		IL_0088: stloc.3
		IL_0089: ldloc.3
		IL_008a: ret

		IL_008b: nop
		IL_008c: ldstr "Middle Mouse Button"
		IL_0091: ret

		IL_0092: nop
		IL_0093: ldstr "Right Mouse Button"
		IL_0098: ret

		IL_0099: nop
		IL_009a: ldstr "Left Mouse Button"
		IL_009f: ret
	} // end of method ControlsMenu::FormatMouseText

	.method public hidebysig 
		instance void StartRebindKeyboard (
			int32 actionIndex
		) cil managed 
	{
		// Method begins at RVA 0x91bc
		// Header size: 12
		// Code size: 247 (0xf7)
		.maxstack 3
		.locals init (
			[0] bool,
			[1] bool,
			[2] bool,
			[3] class [mscorlib]System.Type,
			[4] class [mscorlib]System.Type,
			[5] class [mscorlib]System.Array,
			[6] class [mscorlib]System.Array,
			[7] int32,
			[8] int32,
			[9] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[10] int64,
			[11] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[12] string,
			[13] string
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject ControlsMenu::panel
		IL_0007: callvirt instance bool [UnityEngine.CoreModule]UnityEngine.GameObject::get_activeSelf()
		IL_000c: stloc.0
		IL_000d: ldloc.0
		IL_000e: stloc.1
		IL_000f: ldloc.0
		IL_0010: ldc.i4.0
		IL_0011: ceq
		IL_0013: ldc.i4.0
		IL_0014: ceq
		IL_0016: brtrue IL_00ef

		IL_001b: nop
		IL_001c: ldarg.0
		IL_001d: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject ControlsMenu::confirmationPanel
		IL_0022: callvirt instance bool [UnityEngine.CoreModule]UnityEngine.GameObject::get_activeSelf()
		IL_0027: stloc.2
		IL_0028: ldloc.2
		IL_0029: stloc.1
		IL_002a: ldloc.2
		IL_002b: ldc.i4.0
		IL_002c: ceq
		IL_002e: ldc.i4.0
		IL_002f: ceq
		IL_0031: brtrue IL_00ef

		IL_0036: nop
		IL_0037: br.s IL_003a

		IL_0039: nop

		IL_003a: nop
		IL_003b: ldstr "typeof(InputAction)"
		IL_0040: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0045: box [mscorlib]System.IntPtr
		IL_004a: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [mscorlib]System.RuntimeTypeHandle>(object)
		IL_004f: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		IL_0054: stloc.3
		IL_0055: ldloc.3
		IL_0056: stloc.s 4
		IL_0058: br.s IL_005b

		IL_005a: nop

		IL_005b: nop
		IL_005c: ldloc.s 4
		IL_005e: call class [mscorlib]System.Array [mscorlib]System.Enum::GetValues(class [mscorlib]System.Type)
		IL_0063: stloc.s 5
		IL_0065: ldloc.s 5
		IL_0067: stloc.s 6
		IL_0069: ldloc.s 6
		IL_006b: callvirt instance int32 [mscorlib]System.Array::System.Collections.ICollection.get_Count()
		IL_0070: stloc.s 7
		IL_0072: ldloc.s 7
		IL_0074: stloc.s 8
		IL_0076: ldarg.1
		IL_0077: ldloc.s 7
		IL_0079: clt
		IL_007b: ldc.i4.0
		IL_007c: ceq
		IL_007e: brtrue.s IL_00ef

		IL_0080: nop
		IL_0081: ldarg.0
		IL_0082: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject ControlsMenu::confirmationPanel
		IL_0087: stloc.s 9
		IL_0089: ldarg.0
		IL_008a: ldarg.1
		IL_008b: stfld int32 ControlsMenu::currentActionIndex
		IL_0090: ldarg.0
		IL_0091: ldc.i4.0
		IL_0092: stfld bool ControlsMenu::isBindingMouseInput
		IL_0097: ldloc.s 9
		IL_0099: ldc.i4.1
		IL_009a: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_009f: ldarg.0
		IL_00a0: ldc.i4.0
		IL_00a1: callvirt instance void ControlsMenu::SetButtonsInteractable(bool)
		IL_00a6: ldarg.0
		IL_00a7: ldfld int32 ControlsMenu::currentActionIndex
		IL_00ac: conv.i8
		IL_00ad: stloc.s 10
		IL_00af: ldarg.0
		IL_00b0: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text ControlsMenu::confirmText
		IL_00b5: stloc.s 11
		IL_00b7: ldstr "ArrayElementExpr->klass"
		IL_00bc: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00c1: box [mscorlib]System.IntPtr
		IL_00c6: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<string>(object)
		IL_00cb: ldstr "\n\n"
		IL_00d0: ldstr "ArrayElementExpr->klass"
		IL_00d5: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00da: box [mscorlib]System.IntPtr
		IL_00df: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<string>(object)
		IL_00e4: call string [mscorlib]System.String::Concat(string, string, string)
		IL_00e9: stloc.s 12
		IL_00eb: ldloc.s 12
		IL_00ed: stloc.s 13

		IL_00ef: nop
		IL_00f0: ret

		IL_00f1: nop
		IL_00f2: ldnull
		IL_00f3: throw

		IL_00f4: nop
		IL_00f5: ldnull
		IL_00f6: throw
	} // end of method ControlsMenu::StartRebindKeyboard

	.method public hidebysig 
		instance void StartRebindMouse (
			int32 actionIndex
		) cil managed 
	{
		// Method begins at RVA 0x92c0
		// Header size: 12
		// Code size: 247 (0xf7)
		.maxstack 3
		.locals init (
			[0] bool,
			[1] bool,
			[2] bool,
			[3] class [mscorlib]System.Type,
			[4] class [mscorlib]System.Type,
			[5] class [mscorlib]System.Array,
			[6] class [mscorlib]System.Array,
			[7] int32,
			[8] int32,
			[9] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[10] int64,
			[11] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[12] string,
			[13] string
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject ControlsMenu::panel
		IL_0007: callvirt instance bool [UnityEngine.CoreModule]UnityEngine.GameObject::get_activeSelf()
		IL_000c: stloc.0
		IL_000d: ldloc.0
		IL_000e: stloc.1
		IL_000f: ldloc.0
		IL_0010: ldc.i4.0
		IL_0011: ceq
		IL_0013: ldc.i4.0
		IL_0014: ceq
		IL_0016: brtrue IL_00ef

		IL_001b: nop
		IL_001c: ldarg.0
		IL_001d: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject ControlsMenu::confirmationPanel
		IL_0022: callvirt instance bool [UnityEngine.CoreModule]UnityEngine.GameObject::get_activeSelf()
		IL_0027: stloc.2
		IL_0028: ldloc.2
		IL_0029: stloc.1
		IL_002a: ldloc.2
		IL_002b: ldc.i4.0
		IL_002c: ceq
		IL_002e: ldc.i4.0
		IL_002f: ceq
		IL_0031: brtrue IL_00ef

		IL_0036: nop
		IL_0037: br.s IL_003a

		IL_0039: nop

		IL_003a: nop
		IL_003b: ldstr "typeof(InputAction)"
		IL_0040: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0045: box [mscorlib]System.IntPtr
		IL_004a: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [mscorlib]System.RuntimeTypeHandle>(object)
		IL_004f: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		IL_0054: stloc.3
		IL_0055: ldloc.3
		IL_0056: stloc.s 4
		IL_0058: br.s IL_005b

		IL_005a: nop

		IL_005b: nop
		IL_005c: ldloc.s 4
		IL_005e: call class [mscorlib]System.Array [mscorlib]System.Enum::GetValues(class [mscorlib]System.Type)
		IL_0063: stloc.s 5
		IL_0065: ldloc.s 5
		IL_0067: stloc.s 6
		IL_0069: ldloc.s 6
		IL_006b: callvirt instance int32 [mscorlib]System.Array::System.Collections.ICollection.get_Count()
		IL_0070: stloc.s 7
		IL_0072: ldloc.s 7
		IL_0074: stloc.s 8
		IL_0076: ldarg.1
		IL_0077: ldloc.s 7
		IL_0079: clt
		IL_007b: ldc.i4.0
		IL_007c: ceq
		IL_007e: brtrue.s IL_00ef

		IL_0080: nop
		IL_0081: ldarg.0
		IL_0082: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject ControlsMenu::confirmationPanel
		IL_0087: stloc.s 9
		IL_0089: ldarg.0
		IL_008a: ldarg.1
		IL_008b: stfld int32 ControlsMenu::currentActionIndex
		IL_0090: ldarg.0
		IL_0091: ldc.i4.1
		IL_0092: stfld bool ControlsMenu::isBindingMouseInput
		IL_0097: ldloc.s 9
		IL_0099: ldc.i4.1
		IL_009a: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_009f: ldarg.0
		IL_00a0: ldc.i4.0
		IL_00a1: callvirt instance void ControlsMenu::SetButtonsInteractable(bool)
		IL_00a6: ldarg.0
		IL_00a7: ldfld int32 ControlsMenu::currentActionIndex
		IL_00ac: conv.i8
		IL_00ad: stloc.s 10
		IL_00af: ldarg.0
		IL_00b0: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text ControlsMenu::confirmText
		IL_00b5: stloc.s 11
		IL_00b7: ldstr "ArrayElementExpr->klass"
		IL_00bc: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00c1: box [mscorlib]System.IntPtr
		IL_00c6: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<string>(object)
		IL_00cb: ldstr "\n\n"
		IL_00d0: ldstr "ArrayElementExpr->klass"
		IL_00d5: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00da: box [mscorlib]System.IntPtr
		IL_00df: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<string>(object)
		IL_00e4: call string [mscorlib]System.String::Concat(string, string, string)
		IL_00e9: stloc.s 12
		IL_00eb: ldloc.s 12
		IL_00ed: stloc.s 13

		IL_00ef: nop
		IL_00f0: ret

		IL_00f1: nop
		IL_00f2: ldnull
		IL_00f3: throw

		IL_00f4: nop
		IL_00f5: ldnull
		IL_00f6: throw
	} // end of method ControlsMenu::StartRebindMouse

	.method public hidebysig 
		instance void ShowResetConfirmation () cil managed 
	{
		// Method begins at RVA 0x93c4
		// Header size: 12
		// Code size: 69 (0x45)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.UI]UnityEngine.UI.Button[],
			[1] int64,
			[2] int32,
			[3] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject ControlsMenu::resetConfirmationPanel
		IL_0007: ldc.i4.1
		IL_0008: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_000d: ldarg.0
		IL_000e: ldfld class [UnityEngine.UI]UnityEngine.UI.Button[] ControlsMenu::allButtons
		IL_0013: stloc.0
		// loop start (head: IL_0014)
			IL_0014: nop
			IL_0015: ldc.i4.0
			IL_0016: ldloc.0
			IL_0017: ldlen
			IL_0018: conv.i4
			IL_0019: clt
			IL_001b: ldc.i4.0
			IL_001c: ceq
			IL_001e: brtrue.s IL_003d

			IL_0020: nop
			IL_0021: ldc.i4.0
			IL_0022: conv.i8
			IL_0023: stloc.1
			IL_0024: ldloc.0
			IL_0025: ldc.i4.0
			IL_0026: conv.i8
			IL_0027: conv.i4
			IL_0028: ldelem [UnityEngine.UI]UnityEngine.UI.Button
			IL_002d: ldc.i4.0
			IL_002e: callvirt instance void [UnityEngine.UI]UnityEngine.UI.Selectable::set_interactable(bool)
			IL_0033: ldc.i4.0
			IL_0034: ldc.i4.1
			IL_0035: add
			IL_0036: stloc.2
			IL_0037: ldc.i4.0
			IL_0038: ldc.i4.1
			IL_0039: add
			IL_003a: stloc.3
			IL_003b: br.s IL_0014
		// end loop

		IL_003d: nop
		IL_003e: ret

		IL_003f: nop
		IL_0040: ldnull
		IL_0041: throw

		IL_0042: nop
		IL_0043: ldnull
		IL_0044: throw
	} // end of method ControlsMenu::ShowResetConfirmation

	.method public hidebysig 
		instance void ResetKey () cil managed 
	{
		// Method begins at RVA 0x9418
		// Header size: 12
		// Code size: 286 (0x11e)
		.maxstack 3
		.locals init (
			[0] class InputManager,
			[1] int32,
			[2] bool,
			[3] class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>,
			[4] bool,
			[5] class InputManager,
			[6] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[7] class [UnityEngine.UI]UnityEngine.UI.Button[],
			[8] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class InputManager ControlsMenu::inputMan
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld int32 ControlsMenu::currentActionIndex
		IL_000e: stloc.1
		IL_000f: ldarg.0
		IL_0010: ldfld bool ControlsMenu::isBindingMouseInput
		IL_0015: ldc.i4.0
		IL_0016: ceq
		IL_0018: brtrue.s IL_004c

		IL_001a: nop
		IL_001b: ldloc.0
		IL_001c: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
		IL_0021: ldloc.1
		IL_0022: box [mscorlib]System.Int32
		IL_0027: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
		IL_002c: ldstr "&stack[0x8]"
		IL_0031: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0036: callvirt instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::TryGetValue(!0, !1&)
		IL_003b: stloc.2
		IL_003c: ldloc.2
		IL_003d: ldc.i4.0
		IL_003e: ceq
		IL_0040: brtrue.s IL_00a4

		IL_0042: nop
		IL_0043: ldloc.0
		IL_0044: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
		IL_0049: stloc.3
		IL_004a: br.s IL_007e

		IL_004c: nop
		IL_004d: ldloc.0
		IL_004e: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
		IL_0053: ldloc.1
		IL_0054: box [mscorlib]System.Int32
		IL_0059: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
		IL_005e: ldstr "&stack[0x8]"
		IL_0063: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0068: callvirt instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::TryGetValue(!0, !1&)
		IL_006d: stloc.s 4
		IL_006f: ldloc.s 4
		IL_0071: ldc.i4.0
		IL_0072: ceq
		IL_0074: brtrue.s IL_00a4

		IL_0076: nop
		IL_0077: ldloc.0
		IL_0078: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
		IL_007d: stloc.3

		IL_007e: nop
		IL_007f: ldloc.3
		IL_0080: ldloc.1
		IL_0081: box [mscorlib]System.Int32
		IL_0086: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
		IL_008b: ldstr "stack[0x8]"
		IL_0090: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0095: box [mscorlib]System.Int64
		IL_009a: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputBinding>(object)
		IL_009f: callvirt instance void class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::set_Item(!0, !1)

		IL_00a4: nop
		IL_00a5: ldarg.0
		IL_00a6: ldfld class InputManager ControlsMenu::inputMan
		IL_00ab: stloc.s 5
		IL_00ad: ldloc.s 5
		IL_00af: callvirt instance void InputManager::Save()
		IL_00b4: ldarg.0
		IL_00b5: callvirt instance void ControlsMenu::UpdateAllButtonTexts()
		IL_00ba: ldarg.0
		IL_00bb: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject ControlsMenu::confirmationPanel
		IL_00c0: stloc.s 6
		IL_00c2: ldloc.s 6
		IL_00c4: ldc.i4.0
		IL_00c5: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_00ca: ldarg.0
		IL_00cb: ldfld class [UnityEngine.CoreModule]UnityEngine.Coroutine ControlsMenu::rebindCoroutine
		IL_00d0: ldnull
		IL_00d1: ceq
		IL_00d3: brtrue.s IL_00e2

		IL_00d5: nop
		IL_00d6: ldarg.0
		IL_00d7: ldarg.0
		IL_00d8: ldfld class [UnityEngine.CoreModule]UnityEngine.Coroutine ControlsMenu::rebindCoroutine
		IL_00dd: callvirt instance void [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StopCoroutine(class [UnityEngine.CoreModule]UnityEngine.Coroutine)

		IL_00e2: nop
		IL_00e3: ldarg.0
		IL_00e4: ldfld class [UnityEngine.UI]UnityEngine.UI.Button[] ControlsMenu::allButtons
		IL_00e9: stloc.s 7
		// loop start (head: IL_00eb)
			IL_00eb: nop
			IL_00ec: ldc.i4.0
			IL_00ed: ldloc.s 7
			IL_00ef: ldlen
			IL_00f0: conv.i4
			IL_00f1: clt
			IL_00f3: ldc.i4.0
			IL_00f4: ceq
			IL_00f6: brtrue.s IL_0110

			IL_00f8: nop
			IL_00f9: ldloc.s 7
			IL_00fb: ldc.i4.0
			IL_00fc: conv.i8
			IL_00fd: conv.i4
			IL_00fe: ldelem [UnityEngine.UI]UnityEngine.UI.Button
			IL_0103: ldc.i4.1
			IL_0104: callvirt instance void [UnityEngine.UI]UnityEngine.UI.Selectable::set_interactable(bool)
			IL_0109: ldc.i4.0
			IL_010a: ldc.i4.1
			IL_010b: add
			IL_010c: stloc.s 8
			IL_010e: br.s IL_00eb
		// end loop

		IL_0110: nop
		IL_0111: ret

		IL_0112: nop
		IL_0113: ldnull
		IL_0114: throw

		IL_0115: nop
		IL_0116: ldnull
		IL_0117: throw

		IL_0118: nop
		IL_0119: ldnull
		IL_011a: throw

		IL_011b: nop
		IL_011c: ldnull
		IL_011d: throw
	} // end of method ControlsMenu::ResetKey

	.method public hidebysig 
		instance void ConfirmRebind () cil managed 
	{
		// Method begins at RVA 0x9544
		// Header size: 12
		// Code size: 117 (0x75)
		.maxstack 2
		.locals init (
			[0] class ControlsMenu/'<RebindCoroutine>d__15',
			[1] class [UnityEngine.CoreModule]UnityEngine.Coroutine
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject ControlsMenu::confirmationPanel
		IL_0007: ldc.i4.0
		IL_0008: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_000d: ldarg.0
		IL_000e: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject ControlsMenu::panel
		IL_0013: ldc.i4.1
		IL_0014: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0019: ldarg.0
		IL_001a: ldfld class [UnityEngine.CoreModule]UnityEngine.Coroutine ControlsMenu::rebindCoroutine
		IL_001f: ldnull
		IL_0020: ceq
		IL_0022: brtrue.s IL_0031

		IL_0024: nop
		IL_0025: ldarg.0
		IL_0026: ldarg.0
		IL_0027: ldfld class [UnityEngine.CoreModule]UnityEngine.Coroutine ControlsMenu::rebindCoroutine
		IL_002c: callvirt instance void [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StopCoroutine(class [UnityEngine.CoreModule]UnityEngine.Coroutine)

		IL_0031: nop
		IL_0032: ldstr "il2cpp_codegen_object_new(ControlsMenu+<RebindCoroutine>d__15)"
		IL_0037: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_003c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class ControlsMenu/'<RebindCoroutine>d__15'>(object)
		IL_0041: stloc.0
		IL_0042: ldloc.0
		IL_0043: ldnull
		IL_0044: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0049: ldloc.0
		IL_004a: ldc.i4.0
		IL_004b: stfld int32 ControlsMenu/'<RebindCoroutine>d__15'::'<>1__state'
		IL_0050: ldloc.0
		IL_0051: ldarg.0
		IL_0052: stfld class ControlsMenu ControlsMenu/'<RebindCoroutine>d__15'::'<>4__this'
		IL_0057: ldloc.0
		IL_0058: ldc.r4 5
		IL_005d: stfld float32 ControlsMenu/'<RebindCoroutine>d__15'::duration
		IL_0062: ldarg.0
		IL_0063: ldloc.0
		IL_0064: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Coroutine [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StartCoroutine(class [mscorlib]System.Collections.IEnumerator)
		IL_0069: stloc.1
		IL_006a: ldarg.0
		IL_006b: ldloc.1
		IL_006c: stfld class [UnityEngine.CoreModule]UnityEngine.Coroutine ControlsMenu::rebindCoroutine
		IL_0071: ret

		IL_0072: nop
		IL_0073: ldnull
		IL_0074: throw
	} // end of method ControlsMenu::ConfirmRebind

	.method public hidebysig 
		instance void ConfirmReset () cil managed 
	{
		// Method begins at RVA 0x95c8
		// Header size: 12
		// Code size: 220 (0xdc)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.UI]UnityEngine.UI.Button[],
			[1] int64,
			[2] int32,
			[3] int32,
			[4] class InputManager,
			[5] class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>,
			[6] valuetype InputBinding,
			[7] class [UnityEngine.UI]UnityEngine.UI.Button[],
			[8] int64,
			[9] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject ControlsMenu::resetConfirmationPanel
		IL_0007: ldc.i4.0
		IL_0008: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_000d: ldarg.0
		IL_000e: ldfld class [UnityEngine.UI]UnityEngine.UI.Button[] ControlsMenu::allButtons
		IL_0013: stloc.0
		// loop start (head: IL_0014)
			IL_0014: nop
			IL_0015: ldc.i4.0
			IL_0016: ldloc.0
			IL_0017: ldlen
			IL_0018: conv.i4
			IL_0019: clt
			IL_001b: ldc.i4.0
			IL_001c: ceq
			IL_001e: brtrue.s IL_003d

			IL_0020: nop
			IL_0021: ldc.i4.0
			IL_0022: conv.i8
			IL_0023: stloc.1
			IL_0024: ldloc.0
			IL_0025: ldc.i4.0
			IL_0026: conv.i8
			IL_0027: conv.i4
			IL_0028: ldelem [UnityEngine.UI]UnityEngine.UI.Button
			IL_002d: ldc.i4.1
			IL_002e: callvirt instance void [UnityEngine.UI]UnityEngine.UI.Selectable::set_interactable(bool)
			IL_0033: ldc.i4.0
			IL_0034: ldc.i4.1
			IL_0035: add
			IL_0036: stloc.2
			IL_0037: ldc.i4.0
			IL_0038: ldc.i4.1
			IL_0039: add
			IL_003a: stloc.3
			IL_003b: br.s IL_0014
		// end loop

		IL_003d: nop
		IL_003e: ldarg.0
		IL_003f: ldfld class InputManager ControlsMenu::inputMan
		IL_0044: stloc.s 4
		IL_0046: ldloc.s 4
		IL_0048: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
		IL_004d: callvirt instance void class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::Clear()
		// loop start (head: IL_0052)
			IL_0052: nop
			IL_0053: ldloc.s 4
			IL_0055: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
			IL_005a: stloc.s 5
			IL_005c: ldloc.s 4
			IL_005e: ldc.i4.0
			IL_005f: box [mscorlib]System.Int32
			IL_0064: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
			IL_0069: callvirt instance valuetype InputBinding InputManager::GetDefaultBinding(valuetype InputAction)
			IL_006e: stloc.s 6
			IL_0070: ldloc.s 5
			IL_0072: ldc.i4.0
			IL_0073: box [mscorlib]System.Int32
			IL_0078: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
			IL_007d: ldloc.s 6
			IL_007f: callvirt instance void class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::set_Item(!0, !1)
			IL_0084: ldc.i4.0
			IL_0085: ldc.i4.1
			IL_0086: add
			IL_0087: stloc.3
			IL_0088: ldc.i4.0
			IL_0089: ldc.i4.1
			IL_008a: add
			IL_008b: ldc.i4.s 13
			IL_008d: clt
			IL_008f: brtrue.s IL_0052
		// end loop
		IL_0091: nop
		IL_0092: ldloc.s 4
		IL_0094: callvirt instance void InputManager::Save()
		IL_0099: ldarg.0
		IL_009a: callvirt instance void ControlsMenu::UpdateAllButtonTexts()
		IL_009f: ldarg.0
		IL_00a0: ldfld class [UnityEngine.UI]UnityEngine.UI.Button[] ControlsMenu::allButtons
		IL_00a5: stloc.s 7
		// loop start (head: IL_00a7)
			IL_00a7: nop
			IL_00a8: ldc.i4.0
			IL_00a9: ldloc.s 7
			IL_00ab: ldlen
			IL_00ac: conv.i4
			IL_00ad: clt
			IL_00af: ldc.i4.0
			IL_00b0: ceq
			IL_00b2: brtrue.s IL_00d4

			IL_00b4: nop
			IL_00b5: ldc.i4.0
			IL_00b6: conv.i8
			IL_00b7: stloc.s 8
			IL_00b9: ldloc.s 7
			IL_00bb: ldc.i4.0
			IL_00bc: conv.i8
			IL_00bd: conv.i4
			IL_00be: ldelem [UnityEngine.UI]UnityEngine.UI.Button
			IL_00c3: ldc.i4.1
			IL_00c4: callvirt instance void [UnityEngine.UI]UnityEngine.UI.Selectable::set_interactable(bool)
			IL_00c9: ldc.i4.0
			IL_00ca: ldc.i4.1
			IL_00cb: add
			IL_00cc: stloc.s 9
			IL_00ce: ldc.i4.0
			IL_00cf: ldc.i4.1
			IL_00d0: add
			IL_00d1: stloc.3
			IL_00d2: br.s IL_00a7
		// end loop

		IL_00d4: nop
		IL_00d5: ret

		IL_00d6: nop
		IL_00d7: ldnull
		IL_00d8: throw

		IL_00d9: nop
		IL_00da: ldnull
		IL_00db: throw
	} // end of method ControlsMenu::ConfirmReset

	.method public hidebysig 
		instance void CancelRebind () cil managed 
	{
		// Method begins at RVA 0x96b0
		// Header size: 12
		// Code size: 94 (0x5e)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.UI]UnityEngine.UI.Button[],
			[1] int64,
			[2] int32,
			[3] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject ControlsMenu::confirmationPanel
		IL_0007: ldc.i4.0
		IL_0008: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_000d: ldarg.0
		IL_000e: ldfld class [UnityEngine.CoreModule]UnityEngine.Coroutine ControlsMenu::rebindCoroutine
		IL_0013: ldnull
		IL_0014: ceq
		IL_0016: brtrue.s IL_0025

		IL_0018: nop
		IL_0019: ldarg.0
		IL_001a: ldarg.0
		IL_001b: ldfld class [UnityEngine.CoreModule]UnityEngine.Coroutine ControlsMenu::rebindCoroutine
		IL_0020: callvirt instance void [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StopCoroutine(class [UnityEngine.CoreModule]UnityEngine.Coroutine)

		IL_0025: nop
		IL_0026: ldarg.0
		IL_0027: ldfld class [UnityEngine.UI]UnityEngine.UI.Button[] ControlsMenu::allButtons
		IL_002c: stloc.0
		// loop start (head: IL_002d)
			IL_002d: nop
			IL_002e: ldc.i4.0
			IL_002f: ldloc.0
			IL_0030: ldlen
			IL_0031: conv.i4
			IL_0032: clt
			IL_0034: ldc.i4.0
			IL_0035: ceq
			IL_0037: brtrue.s IL_0056

			IL_0039: nop
			IL_003a: ldc.i4.0
			IL_003b: conv.i8
			IL_003c: stloc.1
			IL_003d: ldloc.0
			IL_003e: ldc.i4.0
			IL_003f: conv.i8
			IL_0040: conv.i4
			IL_0041: ldelem [UnityEngine.UI]UnityEngine.UI.Button
			IL_0046: ldc.i4.1
			IL_0047: callvirt instance void [UnityEngine.UI]UnityEngine.UI.Selectable::set_interactable(bool)
			IL_004c: ldc.i4.0
			IL_004d: ldc.i4.1
			IL_004e: add
			IL_004f: stloc.2
			IL_0050: ldc.i4.0
			IL_0051: ldc.i4.1
			IL_0052: add
			IL_0053: stloc.3
			IL_0054: br.s IL_002d
		// end loop

		IL_0056: nop
		IL_0057: ret

		IL_0058: nop
		IL_0059: ldnull
		IL_005a: throw

		IL_005b: nop
		IL_005c: ldnull
		IL_005d: throw
	} // end of method ControlsMenu::CancelRebind

	.method public hidebysig 
		instance void CancelReset () cil managed 
	{
		// Method begins at RVA 0x971c
		// Header size: 12
		// Code size: 69 (0x45)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.UI]UnityEngine.UI.Button[],
			[1] int64,
			[2] int32,
			[3] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject ControlsMenu::resetConfirmationPanel
		IL_0007: ldc.i4.0
		IL_0008: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_000d: ldarg.0
		IL_000e: ldfld class [UnityEngine.UI]UnityEngine.UI.Button[] ControlsMenu::allButtons
		IL_0013: stloc.0
		// loop start (head: IL_0014)
			IL_0014: nop
			IL_0015: ldc.i4.0
			IL_0016: ldloc.0
			IL_0017: ldlen
			IL_0018: conv.i4
			IL_0019: clt
			IL_001b: ldc.i4.0
			IL_001c: ceq
			IL_001e: brtrue.s IL_003d

			IL_0020: nop
			IL_0021: ldc.i4.0
			IL_0022: conv.i8
			IL_0023: stloc.1
			IL_0024: ldloc.0
			IL_0025: ldc.i4.0
			IL_0026: conv.i8
			IL_0027: conv.i4
			IL_0028: ldelem [UnityEngine.UI]UnityEngine.UI.Button
			IL_002d: ldc.i4.1
			IL_002e: callvirt instance void [UnityEngine.UI]UnityEngine.UI.Selectable::set_interactable(bool)
			IL_0033: ldc.i4.0
			IL_0034: ldc.i4.1
			IL_0035: add
			IL_0036: stloc.2
			IL_0037: ldc.i4.0
			IL_0038: ldc.i4.1
			IL_0039: add
			IL_003a: stloc.3
			IL_003b: br.s IL_0014
		// end loop

		IL_003d: nop
		IL_003e: ret

		IL_003f: nop
		IL_0040: ldnull
		IL_0041: throw

		IL_0042: nop
		IL_0043: ldnull
		IL_0044: throw
	} // end of method ControlsMenu::CancelReset

	.method private hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator RebindCoroutine (
			float32 duration
		) cil managed 
	{
		// Method begins at RVA 0x9770
		// Header size: 12
		// Code size: 47 (0x2f)
		.maxstack 2
		.locals init (
			[0] class ControlsMenu/'<RebindCoroutine>d__15'
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(ControlsMenu+<RebindCoroutine>d__15)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class ControlsMenu/'<RebindCoroutine>d__15'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 ControlsMenu/'<RebindCoroutine>d__15'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class ControlsMenu ControlsMenu/'<RebindCoroutine>d__15'::'<>4__this'
		IL_0026: ldloc.0
		IL_0027: ldarg.1
		IL_0028: stfld float32 ControlsMenu/'<RebindCoroutine>d__15'::duration
		IL_002d: ldloc.0
		IL_002e: ret
	} // end of method ControlsMenu::RebindCoroutine

	.method private hidebysig 
		instance bool GetPressedInput (
			[out] valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode& pressedKey
		) cil managed 
	{
		// Method begins at RVA 0x97ac
		// Header size: 12
		// Code size: 233 (0xe9)
		.maxstack 2
		.locals init (
			[0] bool,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode[],
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode,
			[3] bool,
			[4] valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode,
			[5] int32,
			[6] int32,
			[7] bool
		)

		IL_0000: nop
		IL_0001: ldstr "mov dword ptr [rdx],0 ; value = 0x0"
		IL_0006: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_000b: ldarg.0
		IL_000c: ldfld bool ControlsMenu::isBindingMouseInput
		IL_0011: ldc.i4.0
		IL_0012: ceq
		IL_0014: ldc.i4.0
		IL_0015: ceq
		IL_0017: brtrue IL_00b1

		IL_001c: nop
		IL_001d: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::get_anyKeyDown()
		IL_0022: stloc.0
		IL_0023: ldloc.0
		IL_0024: ldc.i4.0
		IL_0025: ceq
		IL_0027: brtrue IL_00d3

		IL_002c: nop
		IL_002d: ldarg.0
		IL_002e: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode[] ControlsMenu::allKeyCodes
		IL_0033: stloc.1
		// loop start (head: IL_0034)
			IL_0034: nop
			IL_0035: ldc.i4.0
			IL_0036: ldloc.1
			IL_0037: ldlen
			IL_0038: conv.i4
			IL_0039: clt
			IL_003b: ldc.i4.0
			IL_003c: ceq
			IL_003e: brtrue IL_00d3

			IL_0043: nop
			IL_0044: ldloc.1
			IL_0045: ldc.i4.0
			IL_0046: conv.i8
			IL_0047: conv.i4
			IL_0048: ldelem [UnityEngine.CoreModule]UnityEngine.KeyCode
			IL_004d: stloc.2
			IL_004e: ldloc.1
			IL_004f: ldc.i4.0
			IL_0050: conv.i8
			IL_0051: conv.i4
			IL_0052: ldelem [UnityEngine.CoreModule]UnityEngine.KeyCode
			IL_0057: box [mscorlib]System.Object
			IL_005c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
			IL_0061: box [mscorlib]System.Int32
			IL_0066: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
			IL_006b: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetKeyDownInt(valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode)
			IL_0070: stloc.3
			IL_0071: ldloc.2
			IL_0072: stloc.s 4
			IL_0074: ldloc.3
			IL_0075: ldc.i4.0
			IL_0076: ceq
			IL_0078: brtrue.s IL_0097

			IL_007a: nop
			IL_007b: ldloc.s 4
			IL_007d: box [mscorlib]System.Object
			IL_0082: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
			IL_0087: ldc.i8 -323
			IL_0090: add
			IL_0091: conv.i4
			IL_0092: ldc.i4.4
			IL_0093: cgt.un
			IL_0095: brtrue.s IL_00a4

			IL_0097: nop
			IL_0098: ldc.i4.0
			IL_0099: ldc.i4.1
			IL_009a: add
			IL_009b: stloc.s 5
			IL_009d: ldc.i4.0
			IL_009e: ldc.i4.1
			IL_009f: add
			IL_00a0: stloc.s 6
			IL_00a2: br.s IL_0034
		// end loop

		IL_00a4: nop
		IL_00a5: ldstr "mov [r14],esi ; value = ConvExpr"
		IL_00aa: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_00af: ldc.i4.1
		IL_00b0: ret

		IL_00b1: nop
		// loop start (head: IL_00b2)
			IL_00b2: nop
			IL_00b3: ldc.i4.0
			IL_00b4: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetMouseButtonDown(int32)
			IL_00b9: stloc.s 7
			IL_00bb: ldloc.s 7
			IL_00bd: ldc.i4.0
			IL_00be: ceq
			IL_00c0: ldc.i4.0
			IL_00c1: ceq
			IL_00c3: brtrue.s IL_00d6

			IL_00c5: nop
			IL_00c6: ldc.i4.0
			IL_00c7: ldc.i4.1
			IL_00c8: add
			IL_00c9: stloc.s 6
			IL_00cb: ldc.i4.0
			IL_00cc: ldc.i4.1
			IL_00cd: add
			IL_00ce: ldc.i4.7
			IL_00cf: clt
			IL_00d1: brtrue.s IL_00b2
		// end loop

		IL_00d3: nop
		IL_00d4: ldc.i4.0
		IL_00d5: ret

		IL_00d6: nop
		IL_00d7: ldstr "mov [r14],ebx ; value = BinaryExpr"
		IL_00dc: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_00e1: ldc.i4.1
		IL_00e2: ret

		IL_00e3: nop
		IL_00e4: ldnull
		IL_00e5: throw

		IL_00e6: nop
		IL_00e7: ldnull
		IL_00e8: throw
	} // end of method ControlsMenu::GetPressedInput

	.method private hidebysig 
		instance bool IsMouseKey (
			valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode key
		) cil managed 
	{
		// Method begins at RVA 0x228c
		// Header size: 1
		// Code size: 51 (0x33)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: box [mscorlib]System.Object
		IL_0007: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_000c: ldc.i8 -323
		IL_0015: add
		IL_0016: conv.i4
		IL_0017: ldc.i4.3
		IL_0018: cgt.un
		IL_001a: brtrue.s IL_001f

		IL_001c: nop
		IL_001d: ldc.i4.1
		IL_001e: ret

		IL_001f: nop
		IL_0020: ldarg.1
		IL_0021: box [mscorlib]System.Object
		IL_0026: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_002b: ldc.i4 327
		IL_0030: ceq
		IL_0032: ret
	} // end of method ControlsMenu::IsMouseKey

	.method private hidebysig 
		instance void SetButtonsInteractable (
			bool state
		) cil managed 
	{
		// Method begins at RVA 0x98a4
		// Header size: 12
		// Code size: 60 (0x3c)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.UI]UnityEngine.UI.Button[],
			[1] int32,
			[2] int64,
			[3] int32,
			[4] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.UI]UnityEngine.UI.Button[] ControlsMenu::allButtons
		IL_0007: stloc.0
		IL_0008: ldarg.1
		IL_0009: stloc.1
		// loop start (head: IL_000a)
			IL_000a: nop
			IL_000b: ldc.i4.0
			IL_000c: ldloc.0
			IL_000d: ldlen
			IL_000e: conv.i4
			IL_000f: clt
			IL_0011: ldc.i4.0
			IL_0012: ceq
			IL_0014: brtrue.s IL_0034

			IL_0016: nop
			IL_0017: ldc.i4.0
			IL_0018: conv.i8
			IL_0019: stloc.2
			IL_001a: ldloc.0
			IL_001b: ldc.i4.0
			IL_001c: conv.i8
			IL_001d: conv.i4
			IL_001e: ldelem [UnityEngine.UI]UnityEngine.UI.Button
			IL_0023: ldloc.1
			IL_0024: callvirt instance void [UnityEngine.UI]UnityEngine.UI.Selectable::set_interactable(bool)
			IL_0029: ldc.i4.0
			IL_002a: ldc.i4.1
			IL_002b: add
			IL_002c: stloc.3
			IL_002d: ldc.i4.0
			IL_002e: ldc.i4.1
			IL_002f: add
			IL_0030: stloc.s 4
			IL_0032: br.s IL_000a
		// end loop

		IL_0034: nop
		IL_0035: ret

		IL_0036: nop
		IL_0037: ldnull
		IL_0038: throw

		IL_0039: nop
		IL_003a: ldnull
		IL_003b: throw
	} // end of method ControlsMenu::SetButtonsInteractable

	.method public hidebysig 
		instance void ResetToDefaults () cil managed 
	{
		// Method begins at RVA 0x98ec
		// Header size: 12
		// Code size: 154 (0x9a)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.UI]UnityEngine.UI.Button[],
			[1] int64,
			[2] int32,
			[3] int32,
			[4] class InputManager,
			[5] class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>,
			[6] valuetype InputBinding
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.UI]UnityEngine.UI.Button[] ControlsMenu::allButtons
		IL_0007: stloc.0
		// loop start (head: IL_0008)
			IL_0008: nop
			IL_0009: ldc.i4.0
			IL_000a: ldloc.0
			IL_000b: ldlen
			IL_000c: conv.i4
			IL_000d: clt
			IL_000f: ldc.i4.0
			IL_0010: ceq
			IL_0012: brtrue.s IL_0031

			IL_0014: nop
			IL_0015: ldc.i4.0
			IL_0016: conv.i8
			IL_0017: stloc.1
			IL_0018: ldloc.0
			IL_0019: ldc.i4.0
			IL_001a: conv.i8
			IL_001b: conv.i4
			IL_001c: ldelem [UnityEngine.UI]UnityEngine.UI.Button
			IL_0021: ldc.i4.1
			IL_0022: callvirt instance void [UnityEngine.UI]UnityEngine.UI.Selectable::set_interactable(bool)
			IL_0027: ldc.i4.0
			IL_0028: ldc.i4.1
			IL_0029: add
			IL_002a: stloc.2
			IL_002b: ldc.i4.0
			IL_002c: ldc.i4.1
			IL_002d: add
			IL_002e: stloc.3
			IL_002f: br.s IL_0008
		// end loop

		IL_0031: nop
		IL_0032: ldarg.0
		IL_0033: ldfld class InputManager ControlsMenu::inputMan
		IL_0038: stloc.s 4
		IL_003a: ldloc.s 4
		IL_003c: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
		IL_0041: callvirt instance void class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::Clear()
		// loop start (head: IL_0046)
			IL_0046: nop
			IL_0047: ldloc.s 4
			IL_0049: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
			IL_004e: stloc.s 5
			IL_0050: ldloc.s 4
			IL_0052: ldc.i4.0
			IL_0053: box [mscorlib]System.Int32
			IL_0058: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
			IL_005d: callvirt instance valuetype InputBinding InputManager::GetDefaultBinding(valuetype InputAction)
			IL_0062: stloc.s 6
			IL_0064: ldloc.s 5
			IL_0066: ldc.i4.0
			IL_0067: box [mscorlib]System.Int32
			IL_006c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
			IL_0071: ldloc.s 6
			IL_0073: callvirt instance void class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::set_Item(!0, !1)
			IL_0078: ldc.i4.0
			IL_0079: ldc.i4.1
			IL_007a: add
			IL_007b: stloc.3
			IL_007c: ldc.i4.0
			IL_007d: ldc.i4.1
			IL_007e: add
			IL_007f: ldc.i4.s 13
			IL_0081: clt
			IL_0083: brtrue.s IL_0046
		// end loop
		IL_0085: nop
		IL_0086: ldloc.s 4
		IL_0088: callvirt instance void InputManager::Save()
		IL_008d: ldarg.0
		IL_008e: callvirt instance void ControlsMenu::UpdateAllButtonTexts()
		IL_0093: ret

		IL_0094: nop
		IL_0095: ldnull
		IL_0096: throw

		IL_0097: nop
		IL_0098: ldnull
		IL_0099: throw
	} // end of method ControlsMenu::ResetToDefaults

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x22c0
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method ControlsMenu::.ctor

} // end of class ControlsMenu
