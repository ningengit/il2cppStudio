.class public auto ansi beforefieldinit TMPro.Examples.Benchmark01_UGUI
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested private auto ansi sealed beforefieldinit '<Start>d__10'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerator`1<object>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private object '<>2__current'
		.field public class TMPro.Examples.Benchmark01_UGUI '<>4__this'
		.field private int32 '<i>5__2'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x2d88
			// Header size: 1
			// Code size: 16 (0x10)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ldarg.0
			IL_0009: ldarg.1
			IL_000a: stfld int32 TMPro.Examples.Benchmark01_UGUI/'<Start>d__10'::'<>1__state'
			IL_000f: ret
		} // end of method '<Start>d__10'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			// Method begins at RVA 0x2d99
			// Header size: 1
			// Code size: 2 (0x2)
			.maxstack 8

			IL_0000: nop
			IL_0001: ret
		} // end of method '<Start>d__10'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			// Method begins at RVA 0x1b210
			// Header size: 12
			// Code size: 859 (0x35b)
			.maxstack 5
			.locals init (
				[0] class TMPro.Examples.Benchmark01_UGUI,
				[1] int32,
				[2] int32,
				[3] int64,
				[4] class [UnityEngine.CoreModule]UnityEngine.GameObject,
				[5] class [UnityEngine.UI]UnityEngine.UI.Text,
				[6] class [UnityEngine.TextRenderingModule]UnityEngine.Font,
				[7] bool,
				[8] class [UnityEngine.CoreModule]UnityEngine.GameObject,
				[9] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
				[10] class [Unity.TextMeshPro]TMPro.TMP_FontAsset,
				[11] bool,
				[12] class [UnityEngine.CoreModule]UnityEngine.Material,
				[13] class [UnityEngine.CoreModule]UnityEngine.Material,
				[14] class [UnityEngine.UI]UnityEngine.UI.Text,
				[15] int32,
				[16] string,
				[17] string,
				[18] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
				[19] int32,
				[20] string,
				[21] string,
				[22] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
				[23] class [UnityEngine.CoreModule]UnityEngine.Material,
				[24] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
				[25] bool,
				[26] bool,
				[27] class [UnityEngine.CoreModule]UnityEngine.Material,
				[28] int64
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld class TMPro.Examples.Benchmark01_UGUI TMPro.Examples.Benchmark01_UGUI/'<Start>d__10'::'<>4__this'
			IL_0007: stloc.0
			IL_0008: ldarg.0
			IL_0009: ldfld int32 TMPro.Examples.Benchmark01_UGUI/'<Start>d__10'::'<>1__state'
			IL_000e: stloc.1
			IL_000f: ldarg.0
			IL_0010: ldfld int32 TMPro.Examples.Benchmark01_UGUI/'<Start>d__10'::'<>1__state'
			IL_0015: ldc.i4.0
			IL_0016: ceq
			IL_0018: brtrue.s IL_0098

			IL_001a: nop
			IL_001b: ldloc.1
			IL_001c: ldc.i4.1
			IL_001d: sub
			IL_001e: stloc.1
			IL_001f: ldloc.1
			IL_0020: ldc.i4.1
			IL_0021: sub
			IL_0022: ldc.i4.0
			IL_0023: ceq
			IL_0025: brtrue.s IL_003b

			IL_0027: nop
			IL_0028: ldloc.1
			IL_0029: ldc.i4.1
			IL_002a: ceq
			IL_002c: ldc.i4.0
			IL_002d: ceq
			IL_002f: brtrue.s IL_0095

			IL_0031: nop
			IL_0032: ldarg.0
			IL_0033: ldc.i4.m1
			IL_0034: stfld int32 TMPro.Examples.Benchmark01_UGUI/'<Start>d__10'::'<>1__state'
			IL_0039: ldc.i4.0
			IL_003a: ret

			IL_003b: nop
			IL_003c: ldarg.0
			IL_003d: ldfld int32 TMPro.Examples.Benchmark01_UGUI/'<Start>d__10'::'<i>5__2'
			IL_0042: stloc.2
			IL_0043: ldarg.0
			IL_0044: ldc.i4.m1
			IL_0045: stfld int32 TMPro.Examples.Benchmark01_UGUI/'<Start>d__10'::'<>1__state'
			IL_004a: ldarg.0
			IL_004b: ldloc.2
			IL_004c: conv.i8
			IL_004d: ldc.i8 1
			IL_0056: add
			IL_0057: conv.i4
			IL_0058: stfld int32 TMPro.Examples.Benchmark01_UGUI/'<Start>d__10'::'<i>5__2'
			IL_005d: ldloc.2
			IL_005e: conv.i8
			IL_005f: ldc.i8 1
			IL_0068: add
			IL_0069: stloc.3
			IL_006a: ldloc.2
			IL_006b: conv.i8
			IL_006c: ldc.i8 1
			IL_0075: add
			IL_0076: conv.i4
			IL_0077: ldc.i4 1000000
			IL_007c: cgt
			IL_007e: brtrue.s IL_0086

			IL_0080: nop
			IL_0081: br IL_01e8

			IL_0086: nop
			IL_0087: ldarg.0
			IL_0088: ldnull
			IL_0089: stfld object TMPro.Examples.Benchmark01_UGUI/'<Start>d__10'::'<>2__current'
			IL_008e: ldarg.0
			IL_008f: ldc.i4.2
			IL_0090: stfld int32 TMPro.Examples.Benchmark01_UGUI/'<Start>d__10'::'<>1__state'

			IL_0095: nop
			IL_0096: ldc.i4.1
			IL_0097: ret

			IL_0098: nop
			IL_0099: ldarg.0
			IL_009a: ldc.i4.m1
			IL_009b: stfld int32 TMPro.Examples.Benchmark01_UGUI/'<Start>d__10'::'<>1__state'
			IL_00a0: ldloc.0
			IL_00a1: ldfld int32 TMPro.Examples.Benchmark01_UGUI::BenchmarkType
			IL_00a6: ldc.i4.0
			IL_00a7: ceq
			IL_00a9: brtrue IL_0132

			IL_00ae: nop
			IL_00af: ldloc.0
			IL_00b0: ldfld int32 TMPro.Examples.Benchmark01_UGUI::BenchmarkType
			IL_00b5: ldc.i4.1
			IL_00b6: ceq
			IL_00b8: ldc.i4.0
			IL_00b9: ceq
			IL_00bb: brtrue IL_01e0

			IL_00c0: nop
			IL_00c1: ldloc.0
			IL_00c2: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
			IL_00c7: stloc.s 4
			IL_00c9: ldloc.s 4
			IL_00cb: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class [UnityEngine.UI]UnityEngine.UI.Text>()
			IL_00d0: stloc.s 5
			IL_00d2: ldloc.0
			IL_00d3: ldloc.s 5
			IL_00d5: stfld class [UnityEngine.UI]UnityEngine.UI.Text TMPro.Examples.Benchmark01_UGUI::m_textMesh
			IL_00da: ldloc.0
			IL_00db: ldfld class [UnityEngine.TextRenderingModule]UnityEngine.Font TMPro.Examples.Benchmark01_UGUI::TextMeshFont
			IL_00e0: stloc.s 6
			IL_00e2: br.s IL_00e5

			IL_00e4: nop

			IL_00e5: nop
			IL_00e6: ldloc.s 6
			IL_00e8: ldnull
			IL_00e9: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Inequality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
			IL_00ee: stloc.s 7
			IL_00f0: ldloc.s 7
			IL_00f2: ldc.i4.0
			IL_00f3: ceq
			IL_00f5: brtrue.s IL_0109

			IL_00f7: nop
			IL_00f8: ldloc.0
			IL_00f9: ldfld class [UnityEngine.UI]UnityEngine.UI.Text TMPro.Examples.Benchmark01_UGUI::m_textMesh
			IL_00fe: ldloc.0
			IL_00ff: ldfld class [UnityEngine.TextRenderingModule]UnityEngine.Font TMPro.Examples.Benchmark01_UGUI::TextMeshFont
			IL_0104: callvirt instance void [UnityEngine.UI]UnityEngine.UI.Text::set_font(class [UnityEngine.TextRenderingModule]UnityEngine.Font)

			IL_0109: nop
			IL_010a: ldloc.0
			IL_010b: ldfld class [UnityEngine.UI]UnityEngine.UI.Text TMPro.Examples.Benchmark01_UGUI::m_textMesh
			IL_0110: ldc.i4.s 48
			IL_0112: callvirt instance void [UnityEngine.UI]UnityEngine.UI.Text::set_fontSize(int32)
			IL_0117: ldloc.0
			IL_0118: ldfld class [UnityEngine.UI]UnityEngine.UI.Text TMPro.Examples.Benchmark01_UGUI::m_textMesh
			IL_011d: ldc.i4.4
			IL_011e: box [mscorlib]System.Int32
			IL_0123: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.TextRenderingModule]UnityEngine.TextAnchor>(object)
			IL_0128: callvirt instance void [UnityEngine.UI]UnityEngine.UI.Text::set_alignment(valuetype [UnityEngine.TextRenderingModule]UnityEngine.TextAnchor)
			IL_012d: br IL_01e0

			IL_0132: nop
			IL_0133: ldloc.0
			IL_0134: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
			IL_0139: stloc.s 8
			IL_013b: ldloc.s 8
			IL_013d: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class [Unity.TextMeshPro]TMPro.TextMeshProUGUI>()
			IL_0142: stloc.s 9
			IL_0144: ldloc.0
			IL_0145: ldloc.s 9
			IL_0147: stfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.Benchmark01_UGUI::m_textMeshPro
			IL_014c: ldloc.0
			IL_014d: ldfld class [Unity.TextMeshPro]TMPro.TMP_FontAsset TMPro.Examples.Benchmark01_UGUI::TMProFont
			IL_0152: stloc.s 10
			IL_0154: br.s IL_0157

			IL_0156: nop

			IL_0157: nop
			IL_0158: ldloc.s 10
			IL_015a: ldnull
			IL_015b: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Inequality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
			IL_0160: stloc.s 11
			IL_0162: ldloc.s 11
			IL_0164: ldc.i4.0
			IL_0165: ceq
			IL_0167: brtrue.s IL_017b

			IL_0169: nop
			IL_016a: ldloc.0
			IL_016b: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.Benchmark01_UGUI::m_textMeshPro
			IL_0170: ldloc.0
			IL_0171: ldfld class [Unity.TextMeshPro]TMPro.TMP_FontAsset TMPro.Examples.Benchmark01_UGUI::TMProFont
			IL_0176: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_font(class [Unity.TextMeshPro]TMPro.TMP_FontAsset)

			IL_017b: nop
			IL_017c: ldloc.0
			IL_017d: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.Benchmark01_UGUI::m_textMeshPro
			IL_0182: ldc.r4 48
			IL_0187: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_fontSize(float32)
			IL_018c: ldloc.0
			IL_018d: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.Benchmark01_UGUI::m_textMeshPro
			IL_0192: ldc.i4 514
			IL_0197: box [mscorlib]System.Int32
			IL_019c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions>(object)
			IL_01a1: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_alignment(valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions)
			IL_01a6: ldloc.0
			IL_01a7: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.Benchmark01_UGUI::m_textMeshPro
			IL_01ac: ldc.i4.1
			IL_01ad: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_extraPadding(bool)
			IL_01b2: ldloc.0
			IL_01b3: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.Benchmark01_UGUI::m_textMeshPro
			IL_01b8: ldfld class [Unity.TextMeshPro]TMPro.TMP_FontAsset [Unity.TextMeshPro]TMPro.TMP_Text::m_fontAsset
			IL_01bd: ldfld class [UnityEngine.CoreModule]UnityEngine.Material [Unity.TextMeshPro]TMPro.TMP_Asset::material
			IL_01c2: stloc.s 12
			IL_01c4: ldloc.0
			IL_01c5: ldloc.s 12
			IL_01c7: stfld class [UnityEngine.CoreModule]UnityEngine.Material TMPro.Examples.Benchmark01_UGUI::m_material01
			IL_01cc: ldstr "Fonts & Materials/LiberationSans SDF - BEVEL"
			IL_01d1: call !!0 [UnityEngine.CoreModule]UnityEngine.Resources::Load<class [UnityEngine.CoreModule]UnityEngine.Material>(string)
			IL_01d6: stloc.s 13
			IL_01d8: ldloc.0
			IL_01d9: ldloc.s 13
			IL_01db: stfld class [UnityEngine.CoreModule]UnityEngine.Material TMPro.Examples.Benchmark01_UGUI::m_material02

			IL_01e0: nop
			IL_01e1: ldarg.0
			IL_01e2: ldc.i4.0
			IL_01e3: stfld int32 TMPro.Examples.Benchmark01_UGUI/'<Start>d__10'::'<i>5__2'

			IL_01e8: nop
			IL_01e9: ldloc.0
			IL_01ea: ldfld int32 TMPro.Examples.Benchmark01_UGUI::BenchmarkType
			IL_01ef: ldc.i4.0
			IL_01f0: ceq
			IL_01f2: brtrue.s IL_0262

			IL_01f4: nop
			IL_01f5: ldloc.0
			IL_01f6: ldfld int32 TMPro.Examples.Benchmark01_UGUI::BenchmarkType
			IL_01fb: ldc.i4.1
			IL_01fc: ceq
			IL_01fe: ldc.i4.0
			IL_01ff: ceq
			IL_0201: brtrue IL_0347

			IL_0206: nop
			IL_0207: ldloc.0
			IL_0208: ldfld class [UnityEngine.UI]UnityEngine.UI.Text TMPro.Examples.Benchmark01_UGUI::m_textMesh
			IL_020d: stloc.s 14
			IL_020f: ldloca.s 15
			IL_0211: ldstr "imul ecx"
			IL_0216: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_021b: conv.i4
			IL_021c: ldstr "rdx"
			IL_0221: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0226: conv.i4
			IL_0227: ldc.i4.6
			IL_0228: shr
			IL_0229: ldstr "rdx"
			IL_022e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0233: conv.i4
			IL_0234: ldc.i4.6
			IL_0235: shr
			IL_0236: ldc.i4.s 31
			IL_0238: shr.un
			IL_0239: add
			IL_023a: ldc.i4 1000
			IL_023f: mul
			IL_0240: sub
			IL_0241: stfld int32 [mscorlib]System.Int32::m_value
			IL_0246: ldloca.s 15
			IL_0248: call instance string [mscorlib]System.Int32::ToString()
			IL_024d: stloc.s 16
			IL_024f: ldstr "The <color=#0050FF>count is: </color>"
			IL_0254: ldloc.s 16
			IL_0256: call string [mscorlib]System.String::Concat(string, string)
			IL_025b: stloc.s 17
			IL_025d: br IL_0347

			IL_0262: nop
			IL_0263: ldloc.0
			IL_0264: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.Benchmark01_UGUI::m_textMeshPro
			IL_0269: stloc.s 18
			IL_026b: ldloca.s 19
			IL_026d: ldstr "imul ecx"
			IL_0272: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0277: conv.i4
			IL_0278: ldstr "rdx"
			IL_027d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0282: conv.i4
			IL_0283: ldc.i4.6
			IL_0284: shr
			IL_0285: ldstr "rdx"
			IL_028a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_028f: conv.i4
			IL_0290: ldc.i4.6
			IL_0291: shr
			IL_0292: ldc.i4.s 31
			IL_0294: shr.un
			IL_0295: add
			IL_0296: ldc.i4 1000
			IL_029b: mul
			IL_029c: sub
			IL_029d: stfld int32 [mscorlib]System.Int32::m_value
			IL_02a2: ldloca.s 19
			IL_02a4: call instance string [mscorlib]System.Int32::ToString()
			IL_02a9: stloc.s 20
			IL_02ab: ldstr "The <#0050FF>count is: </color>"
			IL_02b0: ldloc.s 20
			IL_02b2: call string [mscorlib]System.String::Concat(string, string)
			IL_02b7: stloc.s 21
			IL_02b9: ldstr "imul ecx"
			IL_02be: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_02c3: conv.i4
			IL_02c4: ldloc.s 21
			IL_02c6: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
			IL_02cb: ldc.i4.6
			IL_02cc: shr
			IL_02cd: ldloc.s 21
			IL_02cf: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
			IL_02d4: ldc.i4.6
			IL_02d5: shr
			IL_02d6: ldc.i4.s 31
			IL_02d8: shr.un
			IL_02d9: add
			IL_02da: ldc.i4 1000
			IL_02df: mul
			IL_02e0: sub
			IL_02e1: ldc.i4 999
			IL_02e6: ceq
			IL_02e8: ldc.i4.0
			IL_02e9: ceq
			IL_02eb: brtrue.s IL_0347

			IL_02ed: nop
			IL_02ee: ldloc.0
			IL_02ef: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.Benchmark01_UGUI::m_textMeshPro
			IL_02f4: stloc.s 22
			IL_02f6: ldloc.0
			IL_02f7: ldfld class [UnityEngine.CoreModule]UnityEngine.Material TMPro.Examples.Benchmark01_UGUI::m_material01
			IL_02fc: stloc.s 23
			IL_02fe: ldloc.s 22
			IL_0300: stloc.s 24
			IL_0302: br.s IL_0305

			IL_0304: nop

			IL_0305: nop
			IL_0306: ldstr "LocalExpr->klass"
			IL_030b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0310: box [mscorlib]System.IntPtr
			IL_0315: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [UnityEngine.CoreModule]UnityEngine.Object>(object)
			IL_031a: ldloc.s 23
			IL_031c: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Equality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
			IL_0321: stloc.s 25
			IL_0323: ldloc.s 25
			IL_0325: stloc.s 26
			IL_0327: ldloc.s 26
			IL_0329: ldc.i4.0
			IL_032a: ceq
			IL_032c: brtrue.s IL_0339

			IL_032e: nop
			IL_032f: ldloc.0
			IL_0330: ldfld class [UnityEngine.CoreModule]UnityEngine.Material TMPro.Examples.Benchmark01_UGUI::m_material02
			IL_0335: stloc.s 27
			IL_0337: br.s IL_0346

			IL_0339: nop
			IL_033a: ldstr "*(LocalExpr + 0x50)"
			IL_033f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0344: stloc.s 28

			IL_0346: nop

			IL_0347: nop
			IL_0348: ldarg.0
			IL_0349: ldnull
			IL_034a: stfld object TMPro.Examples.Benchmark01_UGUI/'<Start>d__10'::'<>2__current'
			IL_034f: ldarg.0
			IL_0350: ldc.i4.1
			IL_0351: stfld int32 TMPro.Examples.Benchmark01_UGUI/'<Start>d__10'::'<>1__state'
			IL_0356: ldc.i4.1
			IL_0357: ret

			IL_0358: nop
			IL_0359: ldnull
			IL_035a: throw
		} // end of method '<Start>d__10'::MoveNext

		.method private final hidebysig specialname newslot virtual 
			instance object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () cil managed 
		{
			// Method begins at RVA 0x2d9c
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.Benchmark01_UGUI/'<Start>d__10'::'<>2__current'
			IL_0007: ret
		} // end of method '<Start>d__10'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'

		.method private final hidebysig newslot virtual 
			instance void System.Collections.IEnumerator.Reset () cil managed 
		{
			// Method begins at RVA 0x1b578
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
		} // end of method '<Start>d__10'::System.Collections.IEnumerator.Reset

		.method private final hidebysig specialname newslot virtual 
			instance object System.Collections.IEnumerator.get_Current () cil managed 
		{
			// Method begins at RVA 0x2da5
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.Benchmark01_UGUI/'<Start>d__10'::'<>2__current'
			IL_0007: ret
		} // end of method '<Start>d__10'::System.Collections.IEnumerator.get_Current

		// Properties
		.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current'()
		{
			.get instance object TMPro.Examples.Benchmark01_UGUI/'<Start>d__10'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'()
		}
		.property instance object System.Collections.IEnumerator.Current()
		{
			.get instance object TMPro.Examples.Benchmark01_UGUI/'<Start>d__10'::System.Collections.IEnumerator.get_Current()
		}

	} // end of class <Start>d__10


	// Fields
	.field public int32 BenchmarkType
	.field public class [UnityEngine.UIModule]UnityEngine.Canvas canvas
	.field public class [Unity.TextMeshPro]TMPro.TMP_FontAsset TMProFont
	.field public class [UnityEngine.TextRenderingModule]UnityEngine.Font TextMeshFont
	.field private class [Unity.TextMeshPro]TMPro.TextMeshProUGUI m_textMeshPro
	.field private class [UnityEngine.UI]UnityEngine.UI.Text m_textMesh
	.field private static literal string label01
	.field private static literal string label02
	.field private class [UnityEngine.CoreModule]UnityEngine.Material m_material01
	.field private class [UnityEngine.CoreModule]UnityEngine.Material m_material02

	// Methods
	.method private hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator Start () cil managed 
	{
		// Method begins at RVA 0x149fc
		// Header size: 12
		// Code size: 40 (0x28)
		.maxstack 2
		.locals init (
			[0] class TMPro.Examples.Benchmark01_UGUI/'<Start>d__10'
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(TMPro.Examples.Benchmark01_UGUI+<Start>d__10)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.Benchmark01_UGUI/'<Start>d__10'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 TMPro.Examples.Benchmark01_UGUI/'<Start>d__10'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class TMPro.Examples.Benchmark01_UGUI TMPro.Examples.Benchmark01_UGUI/'<Start>d__10'::'<>4__this'
		IL_0026: ldloc.0
		IL_0027: ret
	} // end of method Benchmark01_UGUI::Start

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x28a6
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method Benchmark01_UGUI::.ctor

} // end of class TMPro.Examples.Benchmark01_UGUI
