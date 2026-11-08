.class public auto ansi beforefieldinit TMPro.Examples.Benchmark01
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
		.field public class TMPro.Examples.Benchmark01 '<>4__this'
		.field private int32 '<i>5__2'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x2d62
			// Header size: 1
			// Code size: 16 (0x10)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ldarg.0
			IL_0009: ldarg.1
			IL_000a: stfld int32 TMPro.Examples.Benchmark01/'<Start>d__10'::'<>1__state'
			IL_000f: ret
		} // end of method '<Start>d__10'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			// Method begins at RVA 0x2d73
			// Header size: 1
			// Code size: 2 (0x2)
			.maxstack 8

			IL_0000: nop
			IL_0001: ret
		} // end of method '<Start>d__10'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			// Method begins at RVA 0x1ada8
			// Header size: 12
			// Code size: 1035 (0x40b)
			.maxstack 6
			.locals init (
				[0] class TMPro.Examples.Benchmark01,
				[1] int32,
				[2] int32,
				[3] int64,
				[4] class [UnityEngine.CoreModule]UnityEngine.GameObject,
				[5] class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh,
				[6] class [UnityEngine.TextRenderingModule]UnityEngine.Font,
				[7] bool,
				[8] class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh,
				[9] class [mscorlib]System.Type,
				[10] class [UnityEngine.CoreModule]UnityEngine.Object,
				[11] class [UnityEngine.CoreModule]UnityEngine.Object,
				[12] class [UnityEngine.CoreModule]UnityEngine.Object,
				[13] class [UnityEngine.CoreModule]UnityEngine.Object,
				[14] class [UnityEngine.TextRenderingModule]UnityEngine.Font,
				[15] class [UnityEngine.CoreModule]UnityEngine.Renderer,
				[16] class [UnityEngine.TextRenderingModule]UnityEngine.Font,
				[17] class [UnityEngine.CoreModule]UnityEngine.Material,
				[18] class [UnityEngine.CoreModule]UnityEngine.GameObject,
				[19] class [Unity.TextMeshPro]TMPro.TextMeshPro,
				[20] class [Unity.TextMeshPro]TMPro.TextMeshPro,
				[21] class [Unity.TextMeshPro]TMPro.TMP_FontAsset,
				[22] bool,
				[23] class [UnityEngine.CoreModule]UnityEngine.Material,
				[24] class [UnityEngine.CoreModule]UnityEngine.Material,
				[25] class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh,
				[26] int32,
				[27] string,
				[28] string,
				[29] class [Unity.TextMeshPro]TMPro.TextMeshPro,
				[30] class [UnityEngine.CoreModule]UnityEngine.Material,
				[31] class [Unity.TextMeshPro]TMPro.TextMeshPro,
				[32] bool,
				[33] bool,
				[34] class [UnityEngine.CoreModule]UnityEngine.Material
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld class TMPro.Examples.Benchmark01 TMPro.Examples.Benchmark01/'<Start>d__10'::'<>4__this'
			IL_0007: stloc.0
			IL_0008: ldarg.0
			IL_0009: ldfld int32 TMPro.Examples.Benchmark01/'<Start>d__10'::'<>1__state'
			IL_000e: stloc.1
			IL_000f: ldarg.0
			IL_0010: ldfld int32 TMPro.Examples.Benchmark01/'<Start>d__10'::'<>1__state'
			IL_0015: ldc.i4.0
			IL_0016: ceq
			IL_0018: brtrue.s IL_0099

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
			IL_002f: brtrue.s IL_0096

			IL_0031: nop
			IL_0032: ldarg.0
			IL_0033: ldc.i4.m1
			IL_0034: stfld int32 TMPro.Examples.Benchmark01/'<Start>d__10'::'<>1__state'
			IL_0039: br.s IL_0096

			IL_003b: nop
			IL_003c: ldarg.0
			IL_003d: ldfld int32 TMPro.Examples.Benchmark01/'<Start>d__10'::'<i>5__2'
			IL_0042: stloc.2
			IL_0043: ldarg.0
			IL_0044: ldc.i4.m1
			IL_0045: stfld int32 TMPro.Examples.Benchmark01/'<Start>d__10'::'<>1__state'
			IL_004a: ldarg.0
			IL_004b: ldloc.2
			IL_004c: conv.i8
			IL_004d: ldc.i8 1
			IL_0056: add
			IL_0057: conv.i4
			IL_0058: stfld int32 TMPro.Examples.Benchmark01/'<Start>d__10'::'<i>5__2'
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
			IL_0081: br IL_0299

			IL_0086: nop
			IL_0087: ldarg.0
			IL_0088: ldnull
			IL_0089: stfld object TMPro.Examples.Benchmark01/'<Start>d__10'::'<>2__current'
			IL_008e: ldarg.0
			IL_008f: ldc.i4.2
			IL_0090: stfld int32 TMPro.Examples.Benchmark01/'<Start>d__10'::'<>1__state'

			IL_0095: nop

			IL_0096: nop
			IL_0097: ldc.i4.1
			IL_0098: ret

			IL_0099: nop
			IL_009a: ldarg.0
			IL_009b: ldc.i4.m1
			IL_009c: stfld int32 TMPro.Examples.Benchmark01/'<Start>d__10'::'<>1__state'
			IL_00a1: ldloc.0
			IL_00a2: ldfld int32 TMPro.Examples.Benchmark01::BenchmarkType
			IL_00a7: ldc.i4.0
			IL_00a8: ceq
			IL_00aa: brtrue IL_01cf

			IL_00af: nop
			IL_00b0: ldloc.0
			IL_00b1: ldfld int32 TMPro.Examples.Benchmark01::BenchmarkType
			IL_00b6: ldc.i4.1
			IL_00b7: ceq
			IL_00b9: ldc.i4.0
			IL_00ba: ceq
			IL_00bc: brtrue IL_0291

			IL_00c1: nop
			IL_00c2: ldloc.0
			IL_00c3: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
			IL_00c8: stloc.s 4
			IL_00ca: ldloc.s 4
			IL_00cc: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh>()
			IL_00d1: stloc.s 5
			IL_00d3: ldloc.0
			IL_00d4: ldloc.s 5
			IL_00d6: stfld class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh TMPro.Examples.Benchmark01::m_textMesh
			IL_00db: ldloc.0
			IL_00dc: ldfld class [UnityEngine.TextRenderingModule]UnityEngine.Font TMPro.Examples.Benchmark01::TextMeshFont
			IL_00e1: stloc.s 6
			IL_00e3: br.s IL_00e6

			IL_00e5: nop

			IL_00e6: nop
			IL_00e7: ldloc.s 6
			IL_00e9: ldnull
			IL_00ea: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Inequality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
			IL_00ef: stloc.s 7
			IL_00f1: ldloc.0
			IL_00f2: ldfld class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh TMPro.Examples.Benchmark01::m_textMesh
			IL_00f7: stloc.s 8
			IL_00f9: ldloc.s 7
			IL_00fb: ldc.i4.0
			IL_00fc: ceq
			IL_00fe: ldc.i4.0
			IL_00ff: ceq
			IL_0101: brtrue.s IL_0168

			IL_0103: nop
			IL_0104: br.s IL_0107

			IL_0106: nop

			IL_0107: nop
			IL_0108: ldstr "typeof(Font)"
			IL_010d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0112: box [mscorlib]System.IntPtr
			IL_0117: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [mscorlib]System.RuntimeTypeHandle>(object)
			IL_011c: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
			IL_0121: stloc.s 9
			IL_0123: ldstr "Fonts/ARIAL"
			IL_0128: ldloc.s 9
			IL_012a: call class [UnityEngine.CoreModule]UnityEngine.Object [UnityEngine.CoreModule]UnityEngine.Resources::Load(string, class [mscorlib]System.Type)
			IL_012f: stloc.s 10
			IL_0131: ldloc.s 10
			IL_0133: stloc.s 11
			IL_0135: ldloc.s 10
			IL_0137: ldnull
			IL_0138: ceq
			IL_013a: brtrue.s IL_0171

			IL_013c: nop
			IL_013d: ldstr "LocalExpr->klass"
			IL_0142: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0147: conv.i8
			IL_0148: ldstr "Font_TypeInfo"
			IL_014d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0152: conv.i8
			IL_0153: ceq
			IL_0155: brfalse.s IL_015d

			IL_0157: ldloc.s 11
			IL_0159: stloc.s 12
			IL_015b: br.s IL_0161

			IL_015d: nop
			IL_015e: ldnull
			IL_015f: stloc.s 12

			IL_0161: nop
			IL_0162: ldloc.s 12
			IL_0164: stloc.s 13
			IL_0166: br.s IL_0171

			IL_0168: nop
			IL_0169: ldloc.0
			IL_016a: ldfld class [UnityEngine.TextRenderingModule]UnityEngine.Font TMPro.Examples.Benchmark01::TextMeshFont
			IL_016f: stloc.s 14

			IL_0171: nop
			IL_0172: ldloc.s 8
			IL_0174: ldloc.s 14
			IL_0176: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_font(class [UnityEngine.TextRenderingModule]UnityEngine.Font)
			IL_017b: ldloc.0
			IL_017c: ldfld class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh TMPro.Examples.Benchmark01::m_textMesh
			IL_0181: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.CoreModule]UnityEngine.Renderer>()
			IL_0186: stloc.s 15
			IL_0188: ldloc.0
			IL_0189: ldfld class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh TMPro.Examples.Benchmark01::m_textMesh
			IL_018e: callvirt instance class [UnityEngine.TextRenderingModule]UnityEngine.Font [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::get_font()
			IL_0193: stloc.s 16
			IL_0195: ldloc.s 16
			IL_0197: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Material [UnityEngine.TextRenderingModule]UnityEngine.Font::get_material()
			IL_019c: stloc.s 17
			IL_019e: ldloc.s 15
			IL_01a0: ldloc.s 17
			IL_01a2: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Renderer::SetMaterial(class [UnityEngine.CoreModule]UnityEngine.Material)
			IL_01a7: ldloc.0
			IL_01a8: ldfld class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh TMPro.Examples.Benchmark01::m_textMesh
			IL_01ad: ldc.i4.s 48
			IL_01af: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_fontSize(int32)
			IL_01b4: ldloc.0
			IL_01b5: ldfld class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh TMPro.Examples.Benchmark01::m_textMesh
			IL_01ba: ldc.i4.4
			IL_01bb: box [mscorlib]System.Int32
			IL_01c0: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.TextRenderingModule]UnityEngine.TextAnchor>(object)
			IL_01c5: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_anchor(valuetype [UnityEngine.TextRenderingModule]UnityEngine.TextAnchor)
			IL_01ca: br IL_0291

			IL_01cf: nop
			IL_01d0: ldloc.0
			IL_01d1: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
			IL_01d6: stloc.s 18
			IL_01d8: ldloc.s 18
			IL_01da: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class [Unity.TextMeshPro]TMPro.TextMeshPro>()
			IL_01df: stloc.s 19
			IL_01e1: ldloc.0
			IL_01e2: ldloc.s 19
			IL_01e4: stfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.Benchmark01::m_textMeshPro
			IL_01e9: ldloc.0
			IL_01ea: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.Benchmark01::m_textMeshPro
			IL_01ef: stloc.s 20
			IL_01f1: ldloc.0
			IL_01f2: ldfld class [Unity.TextMeshPro]TMPro.TMP_FontAsset TMPro.Examples.Benchmark01::TMProFont
			IL_01f7: stloc.s 21
			IL_01f9: br.s IL_01fc

			IL_01fb: nop

			IL_01fc: nop
			IL_01fd: ldloc.s 21
			IL_01ff: ldnull
			IL_0200: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Inequality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
			IL_0205: stloc.s 22
			IL_0207: ldloc.s 22
			IL_0209: ldc.i4.0
			IL_020a: ceq
			IL_020c: brtrue.s IL_0220

			IL_020e: nop
			IL_020f: ldloc.0
			IL_0210: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.Benchmark01::m_textMeshPro
			IL_0215: ldloc.0
			IL_0216: ldfld class [Unity.TextMeshPro]TMPro.TMP_FontAsset TMPro.Examples.Benchmark01::TMProFont
			IL_021b: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_font(class [Unity.TextMeshPro]TMPro.TMP_FontAsset)

			IL_0220: nop
			IL_0221: ldloc.0
			IL_0222: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.Benchmark01::m_textMeshPro
			IL_0227: ldc.r4 48
			IL_022c: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_fontSize(float32)
			IL_0231: ldloc.0
			IL_0232: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.Benchmark01::m_textMeshPro
			IL_0237: ldc.i4 514
			IL_023c: box [mscorlib]System.Int32
			IL_0241: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions>(object)
			IL_0246: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_alignment(valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions)
			IL_024b: ldloc.0
			IL_024c: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.Benchmark01::m_textMeshPro
			IL_0251: ldc.i4.1
			IL_0252: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_extraPadding(bool)
			IL_0257: ldloc.0
			IL_0258: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.Benchmark01::m_textMeshPro
			IL_025d: ldc.i4.0
			IL_025e: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_enableWordWrapping(bool)
			IL_0263: ldloc.0
			IL_0264: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.Benchmark01::m_textMeshPro
			IL_0269: ldfld class [Unity.TextMeshPro]TMPro.TMP_FontAsset [Unity.TextMeshPro]TMPro.TMP_Text::m_fontAsset
			IL_026e: ldfld class [UnityEngine.CoreModule]UnityEngine.Material [Unity.TextMeshPro]TMPro.TMP_Asset::material
			IL_0273: stloc.s 23
			IL_0275: ldloc.0
			IL_0276: ldloc.s 23
			IL_0278: stfld class [UnityEngine.CoreModule]UnityEngine.Material TMPro.Examples.Benchmark01::m_material01
			IL_027d: ldstr "Fonts & Materials/LiberationSans SDF - Drop Shadow"
			IL_0282: call !!0 [UnityEngine.CoreModule]UnityEngine.Resources::Load<class [UnityEngine.CoreModule]UnityEngine.Material>(string)
			IL_0287: stloc.s 24
			IL_0289: ldloc.0
			IL_028a: ldloc.s 24
			IL_028c: stfld class [UnityEngine.CoreModule]UnityEngine.Material TMPro.Examples.Benchmark01::m_material02

			IL_0291: nop
			IL_0292: ldarg.0
			IL_0293: ldc.i4.0
			IL_0294: stfld int32 TMPro.Examples.Benchmark01/'<Start>d__10'::'<i>5__2'

			IL_0299: nop
			IL_029a: ldloc.0
			IL_029b: ldfld int32 TMPro.Examples.Benchmark01::BenchmarkType
			IL_02a0: ldc.i4.0
			IL_02a1: ceq
			IL_02a3: brtrue.s IL_031c

			IL_02a5: nop
			IL_02a6: ldloc.0
			IL_02a7: ldfld int32 TMPro.Examples.Benchmark01::BenchmarkType
			IL_02ac: ldc.i4.1
			IL_02ad: ceq
			IL_02af: ldc.i4.0
			IL_02b0: ceq
			IL_02b2: brtrue IL_03f4

			IL_02b7: nop
			IL_02b8: ldloc.0
			IL_02b9: ldfld class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh TMPro.Examples.Benchmark01::m_textMesh
			IL_02be: stloc.s 25
			IL_02c0: ldloca.s 26
			IL_02c2: ldstr "imul ecx"
			IL_02c7: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_02cc: conv.i4
			IL_02cd: ldstr "rdx"
			IL_02d2: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_02d7: conv.i4
			IL_02d8: ldc.i4.6
			IL_02d9: shr
			IL_02da: ldstr "rdx"
			IL_02df: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_02e4: conv.i4
			IL_02e5: ldc.i4.6
			IL_02e6: shr
			IL_02e7: ldc.i4.s 31
			IL_02e9: shr.un
			IL_02ea: add
			IL_02eb: ldc.i4 1000
			IL_02f0: mul
			IL_02f1: sub
			IL_02f2: stfld int32 [mscorlib]System.Int32::m_value
			IL_02f7: ldloca.s 26
			IL_02f9: call instance string [mscorlib]System.Int32::ToString()
			IL_02fe: stloc.s 27
			IL_0300: ldstr "The <color=#0050FF>count is: </color>"
			IL_0305: ldloc.s 27
			IL_0307: call string [mscorlib]System.String::Concat(string, string)
			IL_030c: stloc.s 28
			IL_030e: ldloc.s 25
			IL_0310: ldloc.s 28
			IL_0312: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_text(string)
			IL_0317: br IL_03f4

			IL_031c: nop
			IL_031d: ldloc.0
			IL_031e: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.Benchmark01::m_textMeshPro
			IL_0323: stloc.s 29
			IL_0325: ldloc.s 29
			IL_0327: ldstr "The <#0050FF>count is: </color>{0}"
			IL_032c: ldstr "imul ecx"
			IL_0331: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0336: conv.i4
			IL_0337: ldstr "rdx"
			IL_033c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0341: conv.i4
			IL_0342: ldc.i4.6
			IL_0343: shr
			IL_0344: ldstr "rdx"
			IL_0349: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_034e: conv.i4
			IL_034f: ldc.i4.6
			IL_0350: shr
			IL_0351: ldc.i4.s 31
			IL_0353: shr.un
			IL_0354: add
			IL_0355: ldc.i4 1000
			IL_035a: mul
			IL_035b: sub
			IL_035c: conv.r4
			IL_035d: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::SetText(string, float32)
			IL_0362: ldstr "imul ecx"
			IL_0367: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_036c: conv.i4
			IL_036d: ldstr "rdx"
			IL_0372: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0377: conv.i4
			IL_0378: ldc.i4.6
			IL_0379: shr
			IL_037a: ldstr "rdx"
			IL_037f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0384: conv.i4
			IL_0385: ldc.i4.6
			IL_0386: shr
			IL_0387: ldc.i4.s 31
			IL_0389: shr.un
			IL_038a: add
			IL_038b: ldc.i4 1000
			IL_0390: mul
			IL_0391: sub
			IL_0392: ldc.i4 999
			IL_0397: ceq
			IL_0399: ldc.i4.0
			IL_039a: ceq
			IL_039c: brtrue.s IL_03f4

			IL_039e: nop
			IL_039f: ldloc.0
			IL_03a0: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.Benchmark01::m_textMeshPro
			IL_03a5: stloc.s 20
			IL_03a7: ldloc.0
			IL_03a8: ldfld class [UnityEngine.CoreModule]UnityEngine.Material TMPro.Examples.Benchmark01::m_material01
			IL_03ad: stloc.s 30
			IL_03af: ldloc.s 20
			IL_03b1: stloc.s 31
			IL_03b3: br.s IL_03b6

			IL_03b5: nop

			IL_03b6: nop
			IL_03b7: ldstr "LocalExpr->klass"
			IL_03bc: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_03c1: box [mscorlib]System.IntPtr
			IL_03c6: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [UnityEngine.CoreModule]UnityEngine.Object>(object)
			IL_03cb: ldloc.s 30
			IL_03cd: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Equality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
			IL_03d2: stloc.s 32
			IL_03d4: ldloc.s 32
			IL_03d6: stloc.s 33
			IL_03d8: ldloc.s 33
			IL_03da: ldc.i4.0
			IL_03db: ceq
			IL_03dd: brtrue.s IL_03ea

			IL_03df: nop
			IL_03e0: ldloc.0
			IL_03e1: ldfld class [UnityEngine.CoreModule]UnityEngine.Material TMPro.Examples.Benchmark01::m_material02
			IL_03e6: stloc.s 34
			IL_03e8: br.s IL_03f3

			IL_03ea: nop
			IL_03eb: ldloc.0
			IL_03ec: ldfld class [UnityEngine.CoreModule]UnityEngine.Material TMPro.Examples.Benchmark01::m_material01
			IL_03f1: stloc.s 34

			IL_03f3: nop

			IL_03f4: nop
			IL_03f5: ldarg.0
			IL_03f6: ldnull
			IL_03f7: stfld object TMPro.Examples.Benchmark01/'<Start>d__10'::'<>2__current'
			IL_03fc: ldarg.0
			IL_03fd: ldc.i4.1
			IL_03fe: stfld int32 TMPro.Examples.Benchmark01/'<Start>d__10'::'<>1__state'
			IL_0403: br IL_0095

			IL_0408: nop
			IL_0409: ldnull
			IL_040a: throw
		} // end of method '<Start>d__10'::MoveNext

		.method private final hidebysig specialname newslot virtual 
			instance object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () cil managed 
		{
			// Method begins at RVA 0x2d76
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.Benchmark01/'<Start>d__10'::'<>2__current'
			IL_0007: ret
		} // end of method '<Start>d__10'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'

		.method private final hidebysig newslot virtual 
			instance void System.Collections.IEnumerator.Reset () cil managed 
		{
			// Method begins at RVA 0x1b1c0
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
			// Method begins at RVA 0x2d7f
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.Benchmark01/'<Start>d__10'::'<>2__current'
			IL_0007: ret
		} // end of method '<Start>d__10'::System.Collections.IEnumerator.get_Current

		// Properties
		.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current'()
		{
			.get instance object TMPro.Examples.Benchmark01/'<Start>d__10'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'()
		}
		.property instance object System.Collections.IEnumerator.Current()
		{
			.get instance object TMPro.Examples.Benchmark01/'<Start>d__10'::System.Collections.IEnumerator.get_Current()
		}

	} // end of class <Start>d__10


	// Fields
	.field public int32 BenchmarkType
	.field public class [Unity.TextMeshPro]TMPro.TMP_FontAsset TMProFont
	.field public class [UnityEngine.TextRenderingModule]UnityEngine.Font TextMeshFont
	.field private class [Unity.TextMeshPro]TMPro.TextMeshPro m_textMeshPro
	.field private class [Unity.TextMeshPro]TMPro.TextContainer m_textContainer
	.field private class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh m_textMesh
	.field private static literal string label01
	.field private static literal string label02
	.field private class [UnityEngine.CoreModule]UnityEngine.Material m_material01
	.field private class [UnityEngine.CoreModule]UnityEngine.Material m_material02

	// Methods
	.method private hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator Start () cil managed 
	{
		// Method begins at RVA 0x149c8
		// Header size: 12
		// Code size: 40 (0x28)
		.maxstack 2
		.locals init (
			[0] class TMPro.Examples.Benchmark01/'<Start>d__10'
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(TMPro.Examples.Benchmark01+<Start>d__10)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.Benchmark01/'<Start>d__10'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 TMPro.Examples.Benchmark01/'<Start>d__10'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class TMPro.Examples.Benchmark01 TMPro.Examples.Benchmark01/'<Start>d__10'::'<>4__this'
		IL_0026: ldloc.0
		IL_0027: ret
	} // end of method Benchmark01::Start

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x289d
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method Benchmark01::.ctor

} // end of class TMPro.Examples.Benchmark01
