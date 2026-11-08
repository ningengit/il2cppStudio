.class public auto ansi beforefieldinit TMPro.Examples.TextMeshProFloatingText
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested private auto ansi sealed beforefieldinit '<DisplayTextMeshFloatingText>d__13'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerator`1<object>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private object '<>2__current'
		.field public class TMPro.Examples.TextMeshProFloatingText '<>4__this'
		.field private float32 '<CountDuration>5__2'
		.field private float32 '<starting_Count>5__3'
		.field private float32 '<current_Count>5__4'
		.field private valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 '<start_pos>5__5'
		.field private valuetype [UnityEngine.CoreModule]UnityEngine.Color32 '<start_color>5__6'
		.field private float32 '<alpha>5__7'
		.field private float32 '<fadeDuration>5__8'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x2e6c
			// Header size: 1
			// Code size: 16 (0x10)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ldarg.0
			IL_0009: ldarg.1
			IL_000a: stfld int32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<>1__state'
			IL_000f: ret
		} // end of method '<DisplayTextMeshFloatingText>d__13'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			// Method begins at RVA 0x2e7d
			// Header size: 1
			// Code size: 2 (0x2)
			.maxstack 8

			IL_0000: nop
			IL_0001: ret
		} // end of method '<DisplayTextMeshFloatingText>d__13'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			// Method begins at RVA 0x1c6d4
			// Header size: 12
			// Code size: 1503 (0x5df)
			.maxstack 4
			.locals init (
				[0] int32,
				[1] class TMPro.Examples.TextMeshProFloatingText,
				[2] int32,
				[3] class TMPro.Examples.TextMeshProFloatingText,
				[4] class [UnityEngine.CoreModule]UnityEngine.Transform,
				[5] float32,
				[6] class [mscorlib]System.Collections.IEnumerator,
				[7] class [UnityEngine.CoreModule]UnityEngine.Coroutine,
				[8] float32,
				[9] class [UnityEngine.CoreModule]UnityEngine.Transform,
				[10] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[11] float32,
				[12] class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh,
				[13] valuetype [UnityEngine.CoreModule]UnityEngine.Color,
				[14] valuetype [UnityEngine.CoreModule]UnityEngine.Color32,
				[15] float32,
				[16] float32,
				[17] float32,
				[18] float32,
				[19] float32,
				[20] float32,
				[21] float32,
				[22] class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh,
				[23] string,
				[24] class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh,
				[25] float32,
				[26] float32,
				[27] float32,
				[28] class [UnityEngine.CoreModule]UnityEngine.Transform,
				[29] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[30] float32,
				[31] float32,
				[32] float32,
				[33] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[34] bool,
				[35] class [UnityEngine.CoreModule]UnityEngine.Transform,
				[36] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
				[37] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
				[38] bool,
				[39] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[40] class [UnityEngine.CoreModule]UnityEngine.Transform,
				[41] int64,
				[42] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
				[43] class [UnityEngine.CoreModule]UnityEngine.Transform,
				[44] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[45] float32,
				[46] class [UnityEngine.CoreModule]UnityEngine.Transform,
				[47] class [UnityEngine.CoreModule]UnityEngine.WaitForEndOfFrame,
				[48] float32,
				[49] class [UnityEngine.CoreModule]UnityEngine.WaitForSeconds
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld int32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<>1__state'
			IL_0007: stloc.0
			IL_0008: ldarg.0
			IL_0009: ldfld class TMPro.Examples.TextMeshProFloatingText TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<>4__this'
			IL_000e: stloc.1
			IL_000f: ldloc.0
			IL_0010: stloc.2
			IL_0011: ldloc.1
			IL_0012: stloc.3
			IL_0013: ldloc.0
			IL_0014: ldc.i4.0
			IL_0015: ceq
			IL_0017: brtrue.s IL_0098

			IL_0019: nop
			IL_001a: ldloc.2
			IL_001b: ldc.i4.1
			IL_001c: sub
			IL_001d: stloc.2
			IL_001e: ldloc.2
			IL_001f: ldc.i4.1
			IL_0020: sub
			IL_0021: ldc.i4.0
			IL_0022: ceq
			IL_0024: brtrue.s IL_008b

			IL_0026: nop
			IL_0027: ldloc.2
			IL_0028: ldc.i4.1
			IL_0029: ceq
			IL_002b: ldc.i4.0
			IL_002c: ceq
			IL_002e: brtrue.s IL_0085

			IL_0030: nop
			IL_0031: ldarg.0
			IL_0032: ldc.i4.m1
			IL_0033: stfld int32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<>1__state'
			IL_0038: ldloc.3
			IL_0039: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_floatingText_Transform
			IL_003e: stloc.s 4
			IL_0040: ldarg.0
			IL_0041: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<start_pos>5__5'
			IL_0046: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_004b: stloc.s 5
			IL_004d: ldloc.s 4
			IL_004f: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
			IL_0054: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0059: conv.i8
			IL_005a: ldc.i8 -41
			IL_0063: add
			IL_0064: box [mscorlib]System.Int64
			IL_0069: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
			IL_006e: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
			IL_0073: ldloc.3
			IL_0074: callvirt instance class [mscorlib]System.Collections.IEnumerator TMPro.Examples.TextMeshProFloatingText::DisplayTextMeshFloatingText()
			IL_0079: stloc.s 6
			IL_007b: ldloc.3
			IL_007c: ldloc.s 6
			IL_007e: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Coroutine [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StartCoroutine(class [mscorlib]System.Collections.IEnumerator)
			IL_0083: stloc.s 7

			IL_0085: nop
			IL_0086: br IL_05ad

			IL_008b: nop
			IL_008c: ldarg.0
			IL_008d: ldc.i4.m1
			IL_008e: stfld int32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<>1__state'
			IL_0093: br IL_0197

			IL_0098: nop
			IL_0099: ldarg.0
			IL_009a: ldc.i4.m1
			IL_009b: stfld int32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<>1__state'
			IL_00a0: ldarg.0
			IL_00a1: ldc.r4 2
			IL_00a6: stfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<CountDuration>5__2'
			IL_00ab: ldc.r4 5
			IL_00b0: ldc.r4 20
			IL_00b5: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
			IL_00ba: stloc.s 8
			IL_00bc: ldarg.0
			IL_00bd: ldloc.s 8
			IL_00bf: stfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<starting_Count>5__3'
			IL_00c4: ldarg.0
			IL_00c5: ldloc.s 8
			IL_00c7: stfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<current_Count>5__4'
			IL_00cc: ldloc.3
			IL_00cd: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_floatingText_Transform
			IL_00d2: stloc.s 9
			IL_00d4: ldloc.s 9
			IL_00d6: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
			IL_00db: stloc.s 10
			IL_00dd: ldarg.0
			IL_00de: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<start_pos>5__5'
			IL_00e3: ldloca.s 10
			IL_00e5: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_00ea: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_00ef: ldarg.0
			IL_00f0: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<start_pos>5__5'
			IL_00f5: ldloca.s 10
			IL_00f7: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_00fc: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_0101: ldarg.0
			IL_0102: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<start_pos>5__5'
			IL_0107: ldloca.s 10
			IL_0109: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_010e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_0113: ldloca.s 10
			IL_0115: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_011a: stloc.s 11
			IL_011c: ldloc.3
			IL_011d: ldfld class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh TMPro.Examples.TextMeshProFloatingText::m_textMesh
			IL_0122: stloc.s 12
			IL_0124: ldloc.s 12
			IL_0126: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Color [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::get_color()
			IL_012b: stloc.s 13
			IL_012d: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
			IL_0132: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0137: conv.i8
			IL_0138: ldc.i8 -25
			IL_0141: add
			IL_0142: box [mscorlib]System.Int64
			IL_0147: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Color>(object)
			IL_014c: call valuetype [UnityEngine.CoreModule]UnityEngine.Color32 [UnityEngine.CoreModule]UnityEngine.Color32::op_Implicit(valuetype [UnityEngine.CoreModule]UnityEngine.Color)
			IL_0151: stloc.s 14
			IL_0153: ldarg.0
			IL_0154: ldloc.s 14
			IL_0156: box [mscorlib]System.Object
			IL_015b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
			IL_0160: box [mscorlib]System.Int32
			IL_0165: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Color32>(object)
			IL_016a: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Color32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<start_color>5__6'
			IL_016f: ldc.r4 3
			IL_0174: ldarg.0
			IL_0175: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<starting_Count>5__3'
			IL_017a: div
			IL_017b: stloc.s 15
			IL_017d: ldarg.0
			IL_017e: ldc.r4 255
			IL_0183: stfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<alpha>5__7'
			IL_0188: ldarg.0
			IL_0189: ldloc.s 15
			IL_018b: ldarg.0
			IL_018c: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<CountDuration>5__2'
			IL_0191: mul
			IL_0192: stfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<fadeDuration>5__8'

			IL_0197: nop
			IL_0198: ldarg.0
			IL_0199: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<current_Count>5__4'
			IL_019e: stloc.s 16
			IL_01a0: ldarg.0
			IL_01a1: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<current_Count>5__4'
			IL_01a6: ldc.r4 0.0
			IL_01ab: cgt
			IL_01ad: ldc.i4.0
			IL_01ae: ceq
			IL_01b0: brtrue IL_05b0

			IL_01b5: nop
			IL_01b6: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
			IL_01bb: stloc.s 17
			IL_01bd: ldloc.s 17
			IL_01bf: ldarg.0
			IL_01c0: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<CountDuration>5__2'
			IL_01c5: div
			IL_01c6: ldarg.0
			IL_01c7: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<starting_Count>5__3'
			IL_01cc: mul
			IL_01cd: stloc.s 18
			IL_01cf: ldarg.0
			IL_01d0: ldloc.s 16
			IL_01d2: ldloc.s 17
			IL_01d4: ldarg.0
			IL_01d5: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<CountDuration>5__2'
			IL_01da: div
			IL_01db: ldarg.0
			IL_01dc: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<starting_Count>5__3'
			IL_01e1: mul
			IL_01e2: sub
			IL_01e3: stfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<current_Count>5__4'
			IL_01e8: ldloc.s 18
			IL_01ea: stloc.s 16
			IL_01ec: ldc.r4 3
			IL_01f1: ldloc.s 16
			IL_01f3: ldloc.s 17
			IL_01f5: ldarg.0
			IL_01f6: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<CountDuration>5__2'
			IL_01fb: div
			IL_01fc: ldarg.0
			IL_01fd: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<starting_Count>5__3'
			IL_0202: mul
			IL_0203: sub
			IL_0204: clt
			IL_0206: brtrue.s IL_0277

			IL_0208: nop
			IL_0209: ldarg.0
			IL_020a: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<alpha>5__7'
			IL_020f: stloc.s 19
			IL_0211: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
			IL_0216: stloc.s 20
			IL_0218: ldloc.s 20
			IL_021a: ldarg.0
			IL_021b: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<fadeDuration>5__8'
			IL_0220: div
			IL_0221: ldc.r4 255
			IL_0226: mul
			IL_0227: stloc.s 16
			IL_0229: ldloc.s 19
			IL_022b: ldloc.s 20
			IL_022d: ldarg.0
			IL_022e: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<fadeDuration>5__8'
			IL_0233: div
			IL_0234: ldc.r4 255
			IL_0239: mul
			IL_023a: sub
			IL_023b: stloc.s 21
			IL_023d: ldc.r4 0.0
			IL_0242: ldloc.s 19
			IL_0244: ldloc.s 20
			IL_0246: ldarg.0
			IL_0247: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<fadeDuration>5__8'
			IL_024c: div
			IL_024d: ldc.r4 255
			IL_0252: mul
			IL_0253: sub
			IL_0254: cgt
			IL_0256: brtrue.s IL_026a

			IL_0258: nop
			IL_0259: ldloc.s 21
			IL_025b: ldc.r4 255
			IL_0260: cgt
			IL_0262: ldc.i4.0
			IL_0263: ceq
			IL_0265: brtrue.s IL_026b

			IL_0267: nop
			IL_0268: br.s IL_026b

			IL_026a: nop

			IL_026b: nop
			IL_026c: ldarg.0
			IL_026d: ldc.r4 0.0
			IL_0272: stfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<alpha>5__7'

			IL_0277: nop
			IL_0278: ldloc.3
			IL_0279: ldfld class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh TMPro.Examples.TextMeshProFloatingText::m_textMesh
			IL_027e: stloc.s 22
			IL_0280: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
			IL_0285: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_028a: conv.i8
			IL_028b: ldc.i8 103
			IL_0294: add
			IL_0295: call instance string [mscorlib]System.Int32::ToString()
			IL_029a: stloc.s 23
			IL_029c: ldloc.s 22
			IL_029e: ldloc.s 23
			IL_02a0: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_text(string)
			IL_02a5: ldloc.3
			IL_02a6: ldfld class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh TMPro.Examples.TextMeshProFloatingText::m_textMesh
			IL_02ab: stloc.s 24
			IL_02ad: ldarg.0
			IL_02ae: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Color32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<start_color>5__6'
			IL_02b3: ldfld int32 [UnityEngine.CoreModule]UnityEngine.Color32::rgba
			IL_02b8: conv.r4
			IL_02b9: ldc.r4 255
			IL_02be: div
			IL_02bf: stloc.s 16
			IL_02c1: ldarg.0
			IL_02c2: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Color32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<start_color>5__6'
			IL_02c7: ldfld uint8 [UnityEngine.CoreModule]UnityEngine.Color32::g
			IL_02cc: conv.r4
			IL_02cd: ldc.r4 255
			IL_02d2: div
			IL_02d3: stloc.s 25
			IL_02d5: ldarg.0
			IL_02d6: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Color32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<start_color>5__6'
			IL_02db: ldfld uint8 [UnityEngine.CoreModule]UnityEngine.Color32::b
			IL_02e0: conv.r4
			IL_02e1: ldc.r4 255
			IL_02e6: div
			IL_02e7: stloc.s 26
			IL_02e9: ldarg.0
			IL_02ea: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<alpha>5__7'
			IL_02ef: conv.i4
			IL_02f0: conv.r4
			IL_02f1: ldc.r4 255
			IL_02f6: div
			IL_02f7: stloc.s 27
			IL_02f9: ldloc.s 24
			IL_02fb: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
			IL_0300: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0305: conv.i8
			IL_0306: ldc.i8 -25
			IL_030f: add
			IL_0310: box [mscorlib]System.Int64
			IL_0315: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Color>(object)
			IL_031a: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_color(valuetype [UnityEngine.CoreModule]UnityEngine.Color)
			IL_031f: ldloc.3
			IL_0320: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_floatingText_Transform
			IL_0325: stloc.s 28
			IL_0327: ldloc.s 28
			IL_0329: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
			IL_032e: stloc.s 29
			IL_0330: ldarg.0
			IL_0331: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<starting_Count>5__3'
			IL_0336: stloc.s 30
			IL_0338: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
			IL_033d: stloc.s 31
			IL_033f: ldloc.s 28
			IL_0341: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
			IL_0346: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_034b: conv.i8
			IL_034c: ldc.i8 -41
			IL_0355: add
			IL_0356: box [mscorlib]System.Int64
			IL_035b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
			IL_0360: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
			IL_0365: ldloc.3
			IL_0366: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.TextMeshProFloatingText::lastPOS
			IL_036b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_0370: stloc.s 32
			IL_0372: ldloc.3
			IL_0373: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_cameraTransform
			IL_0378: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
			IL_037d: stloc.s 33
			IL_037f: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
			IL_0384: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0389: conv.i8
			IL_038a: ldc.i8 -25
			IL_0393: add
			IL_0394: box [mscorlib]System.Int64
			IL_0399: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
			IL_039e: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
			IL_03a3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_03a8: conv.i8
			IL_03a9: ldc.i8 -41
			IL_03b2: add
			IL_03b3: box [mscorlib]System.Int64
			IL_03b8: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
			IL_03bd: ldc.i4 1000
			IL_03c2: call bool [Unity.TextMeshPro]TMPro.TMPro_ExtensionMethods::Compare(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, int32)
			IL_03c7: stloc.s 34
			IL_03c9: ldloc.s 34
			IL_03cb: ldc.i4.0
			IL_03cc: ceq
			IL_03ce: brtrue.s IL_0441

			IL_03d0: nop
			IL_03d1: ldloc.3
			IL_03d2: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_cameraTransform
			IL_03d7: stloc.s 35
			IL_03d9: ldloc.3
			IL_03da: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion TMPro.Examples.TextMeshProFloatingText::lastRotation
			IL_03df: stloc.s 36
			IL_03e1: ldloc.s 35
			IL_03e3: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Transform::get_rotation()
			IL_03e8: stloc.s 37
			IL_03ea: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
			IL_03ef: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_03f4: conv.i8
			IL_03f5: ldc.i8 -41
			IL_03fe: add
			IL_03ff: box [mscorlib]System.Int64
			IL_0404: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion>(object)
			IL_0409: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
			IL_040e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0413: conv.i8
			IL_0414: ldc.i8 -25
			IL_041d: add
			IL_041e: box [mscorlib]System.Int64
			IL_0423: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion>(object)
			IL_0428: ldc.i4 1000
			IL_042d: call bool [Unity.TextMeshPro]TMPro.TMPro_ExtensionMethods::Compare(valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion, valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion, int32)
			IL_0432: stloc.s 38
			IL_0434: ldloc.s 38
			IL_0436: ldc.i4.0
			IL_0437: ceq
			IL_0439: ldc.i4.0
			IL_043a: ceq
			IL_043c: brtrue IL_0595

			IL_0441: nop
			IL_0442: ldloc.3
			IL_0443: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_cameraTransform
			IL_0448: stloc.s 35
			IL_044a: ldloc.s 35
			IL_044c: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
			IL_0451: stloc.s 39
			IL_0453: ldloc.3
			IL_0454: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_cameraTransform
			IL_0459: stloc.s 40
			IL_045b: ldloc.3
			IL_045c: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.TextMeshProFloatingText::lastPOS
			IL_0461: ldloca.s 39
			IL_0463: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_0468: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_046d: ldloc.3
			IL_046e: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.TextMeshProFloatingText::lastPOS
			IL_0473: ldloca.s 39
			IL_0475: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_047a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_047f: ldloc.3
			IL_0480: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.TextMeshProFloatingText::lastPOS
			IL_0485: ldloca.s 39
			IL_0487: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_048c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_0491: ldloc.s 40
			IL_0493: stloc.s 35
			IL_0495: ldstr "struct_chunk(LocalExpr+0x0, 8)"
			IL_049a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_049f: stloc.s 41
			IL_04a1: ldloc.s 35
			IL_04a3: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Transform::get_rotation()
			IL_04a8: stloc.s 42
			IL_04aa: ldloc.3
			IL_04ab: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_floatingText_Transform
			IL_04b0: stloc.s 43
			IL_04b2: ldloc.3
			IL_04b3: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion TMPro.Examples.TextMeshProFloatingText::lastRotation
			IL_04b8: ldloca.s 42
			IL_04ba: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
			IL_04bf: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
			IL_04c4: ldloc.3
			IL_04c5: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion TMPro.Examples.TextMeshProFloatingText::lastRotation
			IL_04ca: ldloca.s 42
			IL_04cc: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::y
			IL_04d1: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::y
			IL_04d6: ldloc.3
			IL_04d7: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion TMPro.Examples.TextMeshProFloatingText::lastRotation
			IL_04dc: ldloca.s 42
			IL_04de: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::z
			IL_04e3: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::z
			IL_04e8: ldloc.3
			IL_04e9: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion TMPro.Examples.TextMeshProFloatingText::lastRotation
			IL_04ee: ldloca.s 42
			IL_04f0: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::w
			IL_04f5: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::w
			IL_04fa: ldstr "struct_chunk(LocalExpr+0x0, 16)"
			IL_04ff: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0504: stloc.s 41
			IL_0506: ldloc.s 43
			IL_0508: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
			IL_050d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0512: conv.i8
			IL_0513: ldc.i8 -25
			IL_051c: add
			IL_051d: box [mscorlib]System.Int64
			IL_0522: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion>(object)
			IL_0527: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_rotation(valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion)
			IL_052c: ldloc.3
			IL_052d: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_transform
			IL_0532: stloc.s 35
			IL_0534: ldloc.s 35
			IL_0536: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
			IL_053b: stloc.s 44
			IL_053d: ldloca.s 44
			IL_053f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_0544: ldloc.3
			IL_0545: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.TextMeshProFloatingText::lastPOS
			IL_054a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_054f: sub
			IL_0550: stloc.s 45
			IL_0552: ldloca.s 44
			IL_0554: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_0559: ldloc.3
			IL_055a: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.TextMeshProFloatingText::lastPOS
			IL_055f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_0564: sub
			IL_0565: stloc.s 25
			IL_0567: ldloc.3
			IL_0568: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_transform
			IL_056d: stloc.s 46
			IL_056f: ldloc.s 46
			IL_0571: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
			IL_0576: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_057b: conv.i8
			IL_057c: ldc.i8 -41
			IL_0585: add
			IL_0586: box [mscorlib]System.Int64
			IL_058b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
			IL_0590: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_forward(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)

			IL_0595: nop
			IL_0596: newobj instance void [mscorlib]Mono.Globalization.Unicode.ContractionComparer::.ctor()
			IL_059b: stloc.s 47
			IL_059d: ldarg.0
			IL_059e: ldloc.s 47
			IL_05a0: stfld object TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<>2__current'
			IL_05a5: ldarg.0
			IL_05a6: ldc.i4.1
			IL_05a7: stfld int32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<>1__state'

			IL_05ac: nop

			IL_05ad: nop
			IL_05ae: ldc.i4.1
			IL_05af: ret

			IL_05b0: nop
			IL_05b1: ldc.r4 0.1
			IL_05b6: ldc.r4 1
			IL_05bb: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
			IL_05c0: stloc.s 48
			IL_05c2: ldloc.s 48
			IL_05c4: newobj instance void [UnityEngine.CoreModule]UnityEngine.WaitForSeconds::.ctor(float32)
			IL_05c9: stloc.s 49
			IL_05cb: ldarg.0
			IL_05cc: ldloc.s 49
			IL_05ce: stfld object TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<>2__current'
			IL_05d3: ldarg.0
			IL_05d4: ldc.i4.2
			IL_05d5: stfld int32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<>1__state'
			IL_05da: br.s IL_05ac

			IL_05dc: nop
			IL_05dd: ldnull
			IL_05de: throw
		} // end of method '<DisplayTextMeshFloatingText>d__13'::MoveNext

		.method private final hidebysig specialname newslot virtual 
			instance object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () cil managed 
		{
			// Method begins at RVA 0x2e80
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<>2__current'
			IL_0007: ret
		} // end of method '<DisplayTextMeshFloatingText>d__13'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'

		.method private final hidebysig newslot virtual 
			instance void System.Collections.IEnumerator.Reset () cil managed 
		{
			// Method begins at RVA 0x1ccc0
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
		} // end of method '<DisplayTextMeshFloatingText>d__13'::System.Collections.IEnumerator.Reset

		.method private final hidebysig specialname newslot virtual 
			instance object System.Collections.IEnumerator.get_Current () cil managed 
		{
			// Method begins at RVA 0x2e89
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<>2__current'
			IL_0007: ret
		} // end of method '<DisplayTextMeshFloatingText>d__13'::System.Collections.IEnumerator.get_Current

		// Properties
		.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current'()
		{
			.get instance object TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'()
		}
		.property instance object System.Collections.IEnumerator.Current()
		{
			.get instance object TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::System.Collections.IEnumerator.get_Current()
		}

	} // end of class <DisplayTextMeshFloatingText>d__13

	.class nested private auto ansi sealed beforefieldinit '<DisplayTextMeshProFloatingText>d__12'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerator`1<object>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private object '<>2__current'
		.field public class TMPro.Examples.TextMeshProFloatingText '<>4__this'
		.field private float32 '<CountDuration>5__2'
		.field private float32 '<starting_Count>5__3'
		.field private float32 '<current_Count>5__4'
		.field private valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 '<start_pos>5__5'
		.field private valuetype [UnityEngine.CoreModule]UnityEngine.Color32 '<start_color>5__6'
		.field private float32 '<alpha>5__7'
		.field private float32 '<fadeDuration>5__8'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x2e92
			// Header size: 1
			// Code size: 16 (0x10)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ldarg.0
			IL_0009: ldarg.1
			IL_000a: stfld int32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<>1__state'
			IL_000f: ret
		} // end of method '<DisplayTextMeshProFloatingText>d__12'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			// Method begins at RVA 0x2ea3
			// Header size: 1
			// Code size: 2 (0x2)
			.maxstack 8

			IL_0000: nop
			IL_0001: ret
		} // end of method '<DisplayTextMeshProFloatingText>d__12'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			// Method begins at RVA 0x1cd10
			// Header size: 12
			// Code size: 1439 (0x59f)
			.maxstack 4
			.locals init (
				[0] int32,
				[1] class TMPro.Examples.TextMeshProFloatingText,
				[2] int32,
				[3] class TMPro.Examples.TextMeshProFloatingText,
				[4] class [UnityEngine.CoreModule]UnityEngine.Transform,
				[5] float32,
				[6] class [mscorlib]System.Collections.IEnumerator,
				[7] class [UnityEngine.CoreModule]UnityEngine.Coroutine,
				[8] float32,
				[9] class [UnityEngine.CoreModule]UnityEngine.Transform,
				[10] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[11] float32,
				[12] class [Unity.TextMeshPro]TMPro.TextMeshPro,
				[13] valuetype [UnityEngine.CoreModule]UnityEngine.Color32,
				[14] float32,
				[15] float32,
				[16] float32,
				[17] float32,
				[18] float32,
				[19] float32,
				[20] float32,
				[21] class [Unity.TextMeshPro]TMPro.TextMeshPro,
				[22] string,
				[23] float32,
				[24] float32,
				[25] float32,
				[26] class [UnityEngine.CoreModule]UnityEngine.Transform,
				[27] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[28] float32,
				[29] float32,
				[30] float32,
				[31] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[32] bool,
				[33] class [UnityEngine.CoreModule]UnityEngine.Transform,
				[34] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
				[35] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
				[36] bool,
				[37] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[38] class [UnityEngine.CoreModule]UnityEngine.Transform,
				[39] int64,
				[40] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
				[41] class [UnityEngine.CoreModule]UnityEngine.Transform,
				[42] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[43] float32,
				[44] class [UnityEngine.CoreModule]UnityEngine.Transform,
				[45] class [UnityEngine.CoreModule]UnityEngine.WaitForEndOfFrame,
				[46] float32,
				[47] class [UnityEngine.CoreModule]UnityEngine.WaitForSeconds
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld int32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<>1__state'
			IL_0007: stloc.0
			IL_0008: ldarg.0
			IL_0009: ldfld class TMPro.Examples.TextMeshProFloatingText TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<>4__this'
			IL_000e: stloc.1
			IL_000f: ldloc.0
			IL_0010: stloc.2
			IL_0011: ldloc.1
			IL_0012: stloc.3
			IL_0013: ldloc.0
			IL_0014: ldc.i4.0
			IL_0015: ceq
			IL_0017: brtrue.s IL_0098

			IL_0019: nop
			IL_001a: ldloc.2
			IL_001b: ldc.i4.1
			IL_001c: sub
			IL_001d: stloc.2
			IL_001e: ldloc.2
			IL_001f: ldc.i4.1
			IL_0020: sub
			IL_0021: ldc.i4.0
			IL_0022: ceq
			IL_0024: brtrue.s IL_008b

			IL_0026: nop
			IL_0027: ldloc.2
			IL_0028: ldc.i4.1
			IL_0029: ceq
			IL_002b: ldc.i4.0
			IL_002c: ceq
			IL_002e: brtrue.s IL_0085

			IL_0030: nop
			IL_0031: ldarg.0
			IL_0032: ldc.i4.m1
			IL_0033: stfld int32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<>1__state'
			IL_0038: ldloc.3
			IL_0039: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_floatingText_Transform
			IL_003e: stloc.s 4
			IL_0040: ldarg.0
			IL_0041: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<start_pos>5__5'
			IL_0046: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_004b: stloc.s 5
			IL_004d: ldloc.s 4
			IL_004f: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
			IL_0054: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0059: conv.i8
			IL_005a: ldc.i8 -41
			IL_0063: add
			IL_0064: box [mscorlib]System.Int64
			IL_0069: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
			IL_006e: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
			IL_0073: ldloc.3
			IL_0074: callvirt instance class [mscorlib]System.Collections.IEnumerator TMPro.Examples.TextMeshProFloatingText::DisplayTextMeshProFloatingText()
			IL_0079: stloc.s 6
			IL_007b: ldloc.3
			IL_007c: ldloc.s 6
			IL_007e: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Coroutine [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StartCoroutine(class [mscorlib]System.Collections.IEnumerator)
			IL_0083: stloc.s 7

			IL_0085: nop
			IL_0086: br IL_056d

			IL_008b: nop
			IL_008c: ldarg.0
			IL_008d: ldc.i4.m1
			IL_008e: stfld int32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<>1__state'
			IL_0093: br IL_018e

			IL_0098: nop
			IL_0099: ldarg.0
			IL_009a: ldc.i4.m1
			IL_009b: stfld int32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<>1__state'
			IL_00a0: ldarg.0
			IL_00a1: ldc.r4 2
			IL_00a6: stfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<CountDuration>5__2'
			IL_00ab: ldc.r4 5
			IL_00b0: ldc.r4 20
			IL_00b5: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
			IL_00ba: stloc.s 8
			IL_00bc: ldarg.0
			IL_00bd: ldloc.s 8
			IL_00bf: stfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<starting_Count>5__3'
			IL_00c4: ldarg.0
			IL_00c5: ldloc.s 8
			IL_00c7: stfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<current_Count>5__4'
			IL_00cc: ldloc.3
			IL_00cd: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_floatingText_Transform
			IL_00d2: stloc.s 9
			IL_00d4: ldloc.s 9
			IL_00d6: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
			IL_00db: stloc.s 10
			IL_00dd: ldarg.0
			IL_00de: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<start_pos>5__5'
			IL_00e3: ldloca.s 10
			IL_00e5: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_00ea: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_00ef: ldarg.0
			IL_00f0: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<start_pos>5__5'
			IL_00f5: ldloca.s 10
			IL_00f7: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_00fc: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_0101: ldarg.0
			IL_0102: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<start_pos>5__5'
			IL_0107: ldloca.s 10
			IL_0109: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_010e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_0113: ldloca.s 10
			IL_0115: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_011a: stloc.s 11
			IL_011c: ldloc.3
			IL_011d: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TextMeshProFloatingText::m_textMeshPro
			IL_0122: stloc.s 12
			IL_0124: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
			IL_0129: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_012e: conv.i8
			IL_012f: ldc.i8 -25
			IL_0138: add
			IL_0139: box [mscorlib]System.Int64
			IL_013e: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Color>(object)
			IL_0143: call valuetype [UnityEngine.CoreModule]UnityEngine.Color32 [UnityEngine.CoreModule]UnityEngine.Color32::op_Implicit(valuetype [UnityEngine.CoreModule]UnityEngine.Color)
			IL_0148: stloc.s 13
			IL_014a: ldarg.0
			IL_014b: ldloc.s 13
			IL_014d: box [mscorlib]System.Object
			IL_0152: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
			IL_0157: box [mscorlib]System.Int32
			IL_015c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Color32>(object)
			IL_0161: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Color32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<start_color>5__6'
			IL_0166: ldc.r4 3
			IL_016b: ldarg.0
			IL_016c: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<starting_Count>5__3'
			IL_0171: div
			IL_0172: stloc.s 14
			IL_0174: ldarg.0
			IL_0175: ldc.r4 255
			IL_017a: stfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<alpha>5__7'
			IL_017f: ldarg.0
			IL_0180: ldloc.s 14
			IL_0182: ldarg.0
			IL_0183: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<CountDuration>5__2'
			IL_0188: mul
			IL_0189: stfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<fadeDuration>5__8'

			IL_018e: nop
			IL_018f: ldarg.0
			IL_0190: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<current_Count>5__4'
			IL_0195: stloc.s 15
			IL_0197: ldarg.0
			IL_0198: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<current_Count>5__4'
			IL_019d: ldc.r4 0.0
			IL_01a2: cgt
			IL_01a4: ldc.i4.0
			IL_01a5: ceq
			IL_01a7: brtrue IL_0570

			IL_01ac: nop
			IL_01ad: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
			IL_01b2: stloc.s 16
			IL_01b4: ldloc.s 16
			IL_01b6: ldarg.0
			IL_01b7: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<CountDuration>5__2'
			IL_01bc: div
			IL_01bd: ldarg.0
			IL_01be: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<starting_Count>5__3'
			IL_01c3: mul
			IL_01c4: stloc.s 17
			IL_01c6: ldarg.0
			IL_01c7: ldloc.s 15
			IL_01c9: ldloc.s 16
			IL_01cb: ldarg.0
			IL_01cc: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<CountDuration>5__2'
			IL_01d1: div
			IL_01d2: ldarg.0
			IL_01d3: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<starting_Count>5__3'
			IL_01d8: mul
			IL_01d9: sub
			IL_01da: stfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<current_Count>5__4'
			IL_01df: ldloc.s 17
			IL_01e1: stloc.s 15
			IL_01e3: ldc.r4 3
			IL_01e8: ldloc.s 15
			IL_01ea: ldloc.s 16
			IL_01ec: ldarg.0
			IL_01ed: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<CountDuration>5__2'
			IL_01f2: div
			IL_01f3: ldarg.0
			IL_01f4: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<starting_Count>5__3'
			IL_01f9: mul
			IL_01fa: sub
			IL_01fb: clt
			IL_01fd: brtrue.s IL_026e

			IL_01ff: nop
			IL_0200: ldarg.0
			IL_0201: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<alpha>5__7'
			IL_0206: stloc.s 18
			IL_0208: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
			IL_020d: stloc.s 19
			IL_020f: ldloc.s 19
			IL_0211: ldarg.0
			IL_0212: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<fadeDuration>5__8'
			IL_0217: div
			IL_0218: ldc.r4 255
			IL_021d: mul
			IL_021e: stloc.s 15
			IL_0220: ldloc.s 18
			IL_0222: ldloc.s 19
			IL_0224: ldarg.0
			IL_0225: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<fadeDuration>5__8'
			IL_022a: div
			IL_022b: ldc.r4 255
			IL_0230: mul
			IL_0231: sub
			IL_0232: stloc.s 20
			IL_0234: ldc.r4 0.0
			IL_0239: ldloc.s 18
			IL_023b: ldloc.s 19
			IL_023d: ldarg.0
			IL_023e: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<fadeDuration>5__8'
			IL_0243: div
			IL_0244: ldc.r4 255
			IL_0249: mul
			IL_024a: sub
			IL_024b: cgt
			IL_024d: brtrue.s IL_0261

			IL_024f: nop
			IL_0250: ldloc.s 20
			IL_0252: ldc.r4 255
			IL_0257: cgt
			IL_0259: ldc.i4.0
			IL_025a: ceq
			IL_025c: brtrue.s IL_0262

			IL_025e: nop
			IL_025f: br.s IL_0262

			IL_0261: nop

			IL_0262: nop
			IL_0263: ldarg.0
			IL_0264: ldc.r4 0.0
			IL_0269: stfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<alpha>5__7'

			IL_026e: nop
			IL_026f: ldloc.3
			IL_0270: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TextMeshProFloatingText::m_textMeshPro
			IL_0275: stloc.s 21
			IL_0277: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
			IL_027c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0281: conv.i8
			IL_0282: ldc.i8 103
			IL_028b: add
			IL_028c: call instance string [mscorlib]System.Int32::ToString()
			IL_0291: stloc.s 22
			IL_0293: ldarg.0
			IL_0294: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Color32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<start_color>5__6'
			IL_0299: ldfld int32 [UnityEngine.CoreModule]UnityEngine.Color32::rgba
			IL_029e: conv.r4
			IL_029f: ldc.r4 255
			IL_02a4: div
			IL_02a5: stloc.s 15
			IL_02a7: ldarg.0
			IL_02a8: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Color32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<start_color>5__6'
			IL_02ad: ldfld uint8 [UnityEngine.CoreModule]UnityEngine.Color32::g
			IL_02b2: conv.r4
			IL_02b3: ldc.r4 255
			IL_02b8: div
			IL_02b9: stloc.s 23
			IL_02bb: ldarg.0
			IL_02bc: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Color32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<start_color>5__6'
			IL_02c1: ldfld uint8 [UnityEngine.CoreModule]UnityEngine.Color32::b
			IL_02c6: conv.r4
			IL_02c7: ldc.r4 255
			IL_02cc: div
			IL_02cd: stloc.s 24
			IL_02cf: ldarg.0
			IL_02d0: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<alpha>5__7'
			IL_02d5: conv.i4
			IL_02d6: conv.r4
			IL_02d7: ldc.r4 255
			IL_02dc: div
			IL_02dd: stloc.s 25
			IL_02df: ldloc.3
			IL_02e0: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_floatingText_Transform
			IL_02e5: stloc.s 26
			IL_02e7: ldloc.s 26
			IL_02e9: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
			IL_02ee: stloc.s 27
			IL_02f0: ldarg.0
			IL_02f1: ldfld float32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<starting_Count>5__3'
			IL_02f6: stloc.s 28
			IL_02f8: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
			IL_02fd: stloc.s 29
			IL_02ff: ldloc.s 26
			IL_0301: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
			IL_0306: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_030b: conv.i8
			IL_030c: ldc.i8 -41
			IL_0315: add
			IL_0316: box [mscorlib]System.Int64
			IL_031b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
			IL_0320: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
			IL_0325: ldloc.3
			IL_0326: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.TextMeshProFloatingText::lastPOS
			IL_032b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_0330: stloc.s 30
			IL_0332: ldloc.3
			IL_0333: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_cameraTransform
			IL_0338: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
			IL_033d: stloc.s 31
			IL_033f: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
			IL_0344: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0349: conv.i8
			IL_034a: ldc.i8 -25
			IL_0353: add
			IL_0354: box [mscorlib]System.Int64
			IL_0359: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
			IL_035e: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
			IL_0363: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0368: conv.i8
			IL_0369: ldc.i8 -41
			IL_0372: add
			IL_0373: box [mscorlib]System.Int64
			IL_0378: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
			IL_037d: ldc.i4 1000
			IL_0382: call bool [Unity.TextMeshPro]TMPro.TMPro_ExtensionMethods::Compare(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, int32)
			IL_0387: stloc.s 32
			IL_0389: ldloc.s 32
			IL_038b: ldc.i4.0
			IL_038c: ceq
			IL_038e: brtrue.s IL_0401

			IL_0390: nop
			IL_0391: ldloc.3
			IL_0392: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_cameraTransform
			IL_0397: stloc.s 33
			IL_0399: ldloc.3
			IL_039a: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion TMPro.Examples.TextMeshProFloatingText::lastRotation
			IL_039f: stloc.s 34
			IL_03a1: ldloc.s 33
			IL_03a3: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Transform::get_rotation()
			IL_03a8: stloc.s 35
			IL_03aa: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
			IL_03af: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_03b4: conv.i8
			IL_03b5: ldc.i8 -41
			IL_03be: add
			IL_03bf: box [mscorlib]System.Int64
			IL_03c4: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion>(object)
			IL_03c9: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
			IL_03ce: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_03d3: conv.i8
			IL_03d4: ldc.i8 -25
			IL_03dd: add
			IL_03de: box [mscorlib]System.Int64
			IL_03e3: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion>(object)
			IL_03e8: ldc.i4 1000
			IL_03ed: call bool [Unity.TextMeshPro]TMPro.TMPro_ExtensionMethods::Compare(valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion, valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion, int32)
			IL_03f2: stloc.s 36
			IL_03f4: ldloc.s 36
			IL_03f6: ldc.i4.0
			IL_03f7: ceq
			IL_03f9: ldc.i4.0
			IL_03fa: ceq
			IL_03fc: brtrue IL_0555

			IL_0401: nop
			IL_0402: ldloc.3
			IL_0403: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_cameraTransform
			IL_0408: stloc.s 33
			IL_040a: ldloc.s 33
			IL_040c: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
			IL_0411: stloc.s 37
			IL_0413: ldloc.3
			IL_0414: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_cameraTransform
			IL_0419: stloc.s 38
			IL_041b: ldloc.3
			IL_041c: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.TextMeshProFloatingText::lastPOS
			IL_0421: ldloca.s 37
			IL_0423: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_0428: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_042d: ldloc.3
			IL_042e: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.TextMeshProFloatingText::lastPOS
			IL_0433: ldloca.s 37
			IL_0435: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_043a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_043f: ldloc.3
			IL_0440: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.TextMeshProFloatingText::lastPOS
			IL_0445: ldloca.s 37
			IL_0447: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_044c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_0451: ldloc.s 38
			IL_0453: stloc.s 33
			IL_0455: ldstr "struct_chunk(LocalExpr+0x0, 8)"
			IL_045a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_045f: stloc.s 39
			IL_0461: ldloc.s 33
			IL_0463: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Transform::get_rotation()
			IL_0468: stloc.s 40
			IL_046a: ldloc.3
			IL_046b: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_floatingText_Transform
			IL_0470: stloc.s 41
			IL_0472: ldloc.3
			IL_0473: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion TMPro.Examples.TextMeshProFloatingText::lastRotation
			IL_0478: ldloca.s 40
			IL_047a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
			IL_047f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
			IL_0484: ldloc.3
			IL_0485: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion TMPro.Examples.TextMeshProFloatingText::lastRotation
			IL_048a: ldloca.s 40
			IL_048c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::y
			IL_0491: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::y
			IL_0496: ldloc.3
			IL_0497: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion TMPro.Examples.TextMeshProFloatingText::lastRotation
			IL_049c: ldloca.s 40
			IL_049e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::z
			IL_04a3: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::z
			IL_04a8: ldloc.3
			IL_04a9: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion TMPro.Examples.TextMeshProFloatingText::lastRotation
			IL_04ae: ldloca.s 40
			IL_04b0: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::w
			IL_04b5: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::w
			IL_04ba: ldstr "struct_chunk(LocalExpr+0x0, 16)"
			IL_04bf: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_04c4: stloc.s 39
			IL_04c6: ldloc.s 41
			IL_04c8: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
			IL_04cd: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_04d2: conv.i8
			IL_04d3: ldc.i8 -25
			IL_04dc: add
			IL_04dd: box [mscorlib]System.Int64
			IL_04e2: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion>(object)
			IL_04e7: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_rotation(valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion)
			IL_04ec: ldloc.3
			IL_04ed: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_transform
			IL_04f2: stloc.s 33
			IL_04f4: ldloc.s 33
			IL_04f6: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
			IL_04fb: stloc.s 42
			IL_04fd: ldloca.s 42
			IL_04ff: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_0504: ldloc.3
			IL_0505: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.TextMeshProFloatingText::lastPOS
			IL_050a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_050f: sub
			IL_0510: stloc.s 43
			IL_0512: ldloca.s 42
			IL_0514: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_0519: ldloc.3
			IL_051a: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.TextMeshProFloatingText::lastPOS
			IL_051f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_0524: sub
			IL_0525: stloc.s 23
			IL_0527: ldloc.3
			IL_0528: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_transform
			IL_052d: stloc.s 44
			IL_052f: ldloc.s 44
			IL_0531: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
			IL_0536: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_053b: conv.i8
			IL_053c: ldc.i8 -41
			IL_0545: add
			IL_0546: box [mscorlib]System.Int64
			IL_054b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
			IL_0550: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_forward(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)

			IL_0555: nop
			IL_0556: newobj instance void [mscorlib]Mono.Globalization.Unicode.ContractionComparer::.ctor()
			IL_055b: stloc.s 45
			IL_055d: ldarg.0
			IL_055e: ldloc.s 45
			IL_0560: stfld object TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<>2__current'
			IL_0565: ldarg.0
			IL_0566: ldc.i4.1
			IL_0567: stfld int32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<>1__state'

			IL_056c: nop

			IL_056d: nop
			IL_056e: ldc.i4.1
			IL_056f: ret

			IL_0570: nop
			IL_0571: ldc.r4 0.1
			IL_0576: ldc.r4 1
			IL_057b: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
			IL_0580: stloc.s 46
			IL_0582: ldloc.s 46
			IL_0584: newobj instance void [UnityEngine.CoreModule]UnityEngine.WaitForSeconds::.ctor(float32)
			IL_0589: stloc.s 47
			IL_058b: ldarg.0
			IL_058c: ldloc.s 47
			IL_058e: stfld object TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<>2__current'
			IL_0593: ldarg.0
			IL_0594: ldc.i4.2
			IL_0595: stfld int32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<>1__state'
			IL_059a: br.s IL_056c

			IL_059c: nop
			IL_059d: ldnull
			IL_059e: throw
		} // end of method '<DisplayTextMeshProFloatingText>d__12'::MoveNext

		.method private final hidebysig specialname newslot virtual 
			instance object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () cil managed 
		{
			// Method begins at RVA 0x2ea6
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<>2__current'
			IL_0007: ret
		} // end of method '<DisplayTextMeshProFloatingText>d__12'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'

		.method private final hidebysig newslot virtual 
			instance void System.Collections.IEnumerator.Reset () cil managed 
		{
			// Method begins at RVA 0x1d2bc
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
		} // end of method '<DisplayTextMeshProFloatingText>d__12'::System.Collections.IEnumerator.Reset

		.method private final hidebysig specialname newslot virtual 
			instance object System.Collections.IEnumerator.get_Current () cil managed 
		{
			// Method begins at RVA 0x2eaf
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<>2__current'
			IL_0007: ret
		} // end of method '<DisplayTextMeshProFloatingText>d__12'::System.Collections.IEnumerator.get_Current

		// Properties
		.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current'()
		{
			.get instance object TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'()
		}
		.property instance object System.Collections.IEnumerator.Current()
		{
			.get instance object TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::System.Collections.IEnumerator.get_Current()
		}

	} // end of class <DisplayTextMeshProFloatingText>d__12


	// Fields
	.field public class [UnityEngine.TextRenderingModule]UnityEngine.Font TheFont
	.field private class [UnityEngine.CoreModule]UnityEngine.GameObject m_floatingText
	.field private class [Unity.TextMeshPro]TMPro.TextMeshPro m_textMeshPro
	.field private class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh m_textMesh
	.field private class [UnityEngine.CoreModule]UnityEngine.Transform m_transform
	.field private class [UnityEngine.CoreModule]UnityEngine.Transform m_floatingText_Transform
	.field private class [UnityEngine.CoreModule]UnityEngine.Transform m_cameraTransform
	.field private valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 lastPOS
	.field private valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion lastRotation
	.field public int32 SpawnType

	// Methods
	.method private hidebysig 
		instance void Awake () cil managed 
	{
		// Method begins at RVA 0x16864
		// Header size: 12
		// Code size: 76 (0x4c)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] string,
			[2] string,
			[3] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[4] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[5] class [UnityEngine.CoreModule]UnityEngine.Transform
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_transform
		IL_000f: ldarg.0
		IL_0010: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_0015: stloc.1
		IL_0016: ldloc.1
		IL_0017: ldstr " floating text"
		IL_001c: call string [mscorlib]System.String::Concat(string, string)
		IL_0021: stloc.2
		IL_0022: ldloc.2
		IL_0023: newobj instance void [UnityEngine.CoreModule]UnityEngine.GameObject::.ctor(string)
		IL_0028: stloc.3
		IL_0029: ldarg.0
		IL_002a: ldloc.3
		IL_002b: stfld class [UnityEngine.CoreModule]UnityEngine.GameObject TMPro.Examples.TextMeshProFloatingText::m_floatingText
		IL_0030: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_0035: stloc.s 4
		IL_0037: ldloc.s 4
		IL_0039: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_003e: stloc.s 5
		IL_0040: ldarg.0
		IL_0041: ldloc.s 5
		IL_0043: stfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_cameraTransform
		IL_0048: ret

		IL_0049: nop
		IL_004a: ldnull
		IL_004b: throw
	} // end of method TextMeshProFloatingText::Awake

	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x168bc
		// Header size: 12
		// Code size: 790 (0x316)
		.maxstack 4
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[2] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[4] float32,
			[5] float32,
			[6] float32,
			[7] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[8] class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh,
			[9] class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh,
			[10] class [UnityEngine.TextRenderingModule]UnityEngine.Font,
			[11] class [UnityEngine.CoreModule]UnityEngine.Renderer,
			[12] class [UnityEngine.TextRenderingModule]UnityEngine.Font,
			[13] class [UnityEngine.CoreModule]UnityEngine.Material,
			[14] class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh,
			[15] int32,
			[16] int32,
			[17] int32,
			[18] int32,
			[19] valuetype [UnityEngine.CoreModule]UnityEngine.Color,
			[20] class [mscorlib]System.Collections.IEnumerator,
			[21] class [mscorlib]System.Collections.IEnumerator,
			[22] class [UnityEngine.CoreModule]UnityEngine.Coroutine,
			[23] class [UnityEngine.CoreModule]UnityEngine.Coroutine,
			[24] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[25] class [UnityEngine.CoreModule]UnityEngine.RectTransform,
			[26] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[27] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[28] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[29] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[30] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[31] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[32] int32,
			[33] int32,
			[34] int32,
			[35] class TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12',
			[36] class TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld int32 TMPro.Examples.TextMeshProFloatingText::SpawnType
		IL_0007: ldc.i4.0
		IL_0008: ceq
		IL_000a: brtrue IL_01b4

		IL_000f: nop
		IL_0010: ldarg.0
		IL_0011: ldfld int32 TMPro.Examples.TextMeshProFloatingText::SpawnType
		IL_0016: ldc.i4.1
		IL_0017: ceq
		IL_0019: ldc.i4.0
		IL_001a: ceq
		IL_001c: brtrue IL_01b2

		IL_0021: nop
		IL_0022: ldarg.0
		IL_0023: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject TMPro.Examples.TextMeshProFloatingText::m_floatingText
		IL_0028: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
		IL_002d: stloc.0
		IL_002e: ldarg.0
		IL_002f: ldloc.0
		IL_0030: stfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_floatingText_Transform
		IL_0035: ldarg.0
		IL_0036: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_transform
		IL_003b: stloc.1
		IL_003c: ldarg.0
		IL_003d: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_floatingText_Transform
		IL_0042: stloc.2
		IL_0043: ldloc.1
		IL_0044: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0049: stloc.3
		IL_004a: ldc.r4 0.0
		IL_004f: ldloca.s 3
		IL_0051: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0056: add
		IL_0057: stloc.s 4
		IL_0059: ldc.r4 0.0
		IL_005e: ldloca.s 3
		IL_0060: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0065: add
		IL_0066: stloc.s 5
		IL_0068: ldc.r4 15
		IL_006d: ldloca.s 3
		IL_006f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0074: add
		IL_0075: stloc.s 6
		IL_0077: ldloc.2
		IL_0078: ldloca.s 7
		IL_007a: ldloc.s 5
		IL_007c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0081: ldloca.s 7
		IL_0083: ldloc.s 6
		IL_0085: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_008a: ldloca.s 7
		IL_008c: ldloc.s 4
		IL_008e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0093: ldloc.s 7
		IL_0095: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_009a: ldarg.0
		IL_009b: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject TMPro.Examples.TextMeshProFloatingText::m_floatingText
		IL_00a0: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh>()
		IL_00a5: stloc.s 8
		IL_00a7: ldarg.0
		IL_00a8: ldloc.s 8
		IL_00aa: stfld class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh TMPro.Examples.TextMeshProFloatingText::m_textMesh
		IL_00af: ldarg.0
		IL_00b0: ldfld class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh TMPro.Examples.TextMeshProFloatingText::m_textMesh
		IL_00b5: stloc.s 9
		IL_00b7: ldstr "Fonts/ARIAL"
		IL_00bc: call !!0 [UnityEngine.CoreModule]UnityEngine.Resources::Load<class [UnityEngine.TextRenderingModule]UnityEngine.Font>(string)
		IL_00c1: stloc.s 10
		IL_00c3: ldloc.s 9
		IL_00c5: ldloc.s 10
		IL_00c7: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_font(class [UnityEngine.TextRenderingModule]UnityEngine.Font)
		IL_00cc: ldarg.0
		IL_00cd: ldfld class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh TMPro.Examples.TextMeshProFloatingText::m_textMesh
		IL_00d2: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.CoreModule]UnityEngine.Renderer>()
		IL_00d7: stloc.s 11
		IL_00d9: ldarg.0
		IL_00da: ldfld class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh TMPro.Examples.TextMeshProFloatingText::m_textMesh
		IL_00df: callvirt instance class [UnityEngine.TextRenderingModule]UnityEngine.Font [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::get_font()
		IL_00e4: stloc.s 12
		IL_00e6: ldloc.s 12
		IL_00e8: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Material [UnityEngine.TextRenderingModule]UnityEngine.Font::get_material()
		IL_00ed: stloc.s 13
		IL_00ef: ldloc.s 11
		IL_00f1: ldloc.s 13
		IL_00f3: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Renderer::SetMaterial(class [UnityEngine.CoreModule]UnityEngine.Material)
		IL_00f8: ldarg.0
		IL_00f9: ldfld class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh TMPro.Examples.TextMeshProFloatingText::m_textMesh
		IL_00fe: stloc.s 14
		IL_0100: ldc.i4.0
		IL_0101: ldc.i4 255
		IL_0106: call int32 [UnityEngine.CoreModule]UnityEngine.Random::Range(int32, int32)
		IL_010b: stloc.s 15
		IL_010d: ldc.i4.0
		IL_010e: ldc.i4 255
		IL_0113: call int32 [UnityEngine.CoreModule]UnityEngine.Random::Range(int32, int32)
		IL_0118: stloc.s 16
		IL_011a: ldc.i4.0
		IL_011b: ldc.i4 255
		IL_0120: call int32 [UnityEngine.CoreModule]UnityEngine.Random::Range(int32, int32)
		IL_0125: stloc.s 17
		IL_0127: ldloc.s 17
		IL_0129: stloc.s 18
		IL_012b: ldloc.s 17
		IL_012d: conv.r4
		IL_012e: ldc.r4 255
		IL_0133: div
		IL_0134: stloc.s 4
		IL_0136: ldloc.s 14
		IL_0138: ldloca.s 19
		IL_013a: ldloc.s 15
		IL_013c: conv.r4
		IL_013d: ldc.r4 255
		IL_0142: div
		IL_0143: stfld float32 [UnityEngine.CoreModule]UnityEngine.Color::r
		IL_0148: ldloca.s 19
		IL_014a: ldloc.s 16
		IL_014c: conv.r4
		IL_014d: ldc.r4 255
		IL_0152: div
		IL_0153: stfld float32 [UnityEngine.CoreModule]UnityEngine.Color::g
		IL_0158: ldloca.s 19
		IL_015a: ldloc.s 4
		IL_015c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Color::b
		IL_0161: ldloca.s 19
		IL_0163: ldc.r4 1
		IL_0168: stfld float32 [UnityEngine.CoreModule]UnityEngine.Color::a
		IL_016d: ldloc.s 19
		IL_016f: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_color(valuetype [UnityEngine.CoreModule]UnityEngine.Color)
		IL_0174: ldarg.0
		IL_0175: ldfld class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh TMPro.Examples.TextMeshProFloatingText::m_textMesh
		IL_017a: ldc.i4.7
		IL_017b: box [mscorlib]System.Int32
		IL_0180: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.TextRenderingModule]UnityEngine.TextAnchor>(object)
		IL_0185: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_anchor(valuetype [UnityEngine.TextRenderingModule]UnityEngine.TextAnchor)
		IL_018a: ldarg.0
		IL_018b: ldfld class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh TMPro.Examples.TextMeshProFloatingText::m_textMesh
		IL_0190: ldc.i4.s 24
		IL_0192: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_fontSize(int32)
		IL_0197: ldarg.0
		IL_0198: callvirt instance class [mscorlib]System.Collections.IEnumerator TMPro.Examples.TextMeshProFloatingText::DisplayTextMeshFloatingText()
		IL_019d: stloc.s 20
		IL_019f: ldloc.s 20
		IL_01a1: stloc.s 21

		IL_01a3: nop
		IL_01a4: ldarg.0
		IL_01a5: ldloc.s 21
		IL_01a7: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Coroutine [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StartCoroutine(class [mscorlib]System.Collections.IEnumerator)
		IL_01ac: stloc.s 22
		IL_01ae: ldloc.s 22
		IL_01b0: stloc.s 23

		IL_01b2: nop
		IL_01b3: ret

		IL_01b4: nop
		IL_01b5: ldarg.0
		IL_01b6: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject TMPro.Examples.TextMeshProFloatingText::m_floatingText
		IL_01bb: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class [Unity.TextMeshPro]TMPro.TextMeshPro>()
		IL_01c0: stloc.s 24
		IL_01c2: ldarg.0
		IL_01c3: ldloc.s 24
		IL_01c5: stfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TextMeshProFloatingText::m_textMeshPro
		IL_01ca: ldarg.0
		IL_01cb: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TextMeshProFloatingText::m_textMeshPro
		IL_01d0: callvirt instance class [UnityEngine.CoreModule]UnityEngine.RectTransform [Unity.TextMeshPro]TMPro.TMP_Text::get_rectTransform()
		IL_01d5: stloc.s 25
		IL_01d7: ldloc.s 25
		IL_01d9: ldstr "unpcklps xmm0,xmm0"
		IL_01de: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01e3: box [mscorlib]System.Int64
		IL_01e8: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector2>(object)
		IL_01ed: callvirt instance void [UnityEngine.CoreModule]UnityEngine.RectTransform::set_sizeDelta(valuetype [UnityEngine.CoreModule]UnityEngine.Vector2)
		IL_01f2: ldarg.0
		IL_01f3: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject TMPro.Examples.TextMeshProFloatingText::m_floatingText
		IL_01f8: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
		IL_01fd: stloc.s 26
		IL_01ff: ldarg.0
		IL_0200: ldloc.s 26
		IL_0202: stfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_floatingText_Transform
		IL_0207: ldarg.0
		IL_0208: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_transform
		IL_020d: stloc.s 27
		IL_020f: ldarg.0
		IL_0210: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TextMeshProFloatingText::m_floatingText_Transform
		IL_0215: stloc.s 28
		IL_0217: ldloc.s 27
		IL_0219: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_021e: stloc.s 29
		IL_0220: ldc.r4 0.0
		IL_0225: ldloca.s 29
		IL_0227: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_022c: add
		IL_022d: stloc.s 4
		IL_022f: ldc.r4 0.0
		IL_0234: ldloca.s 29
		IL_0236: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_023b: add
		IL_023c: stloc.s 5
		IL_023e: ldc.r4 15
		IL_0243: ldloca.s 29
		IL_0245: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_024a: add
		IL_024b: stloc.s 6
		IL_024d: ldloc.s 28
		IL_024f: ldloca.s 30
		IL_0251: ldloc.s 5
		IL_0253: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0258: ldloca.s 30
		IL_025a: ldloc.s 6
		IL_025c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0261: ldloca.s 30
		IL_0263: ldloc.s 4
		IL_0265: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_026a: ldloc.s 30
		IL_026c: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0271: ldarg.0
		IL_0272: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TextMeshProFloatingText::m_textMeshPro
		IL_0277: ldc.i4 514
		IL_027c: box [mscorlib]System.Int32
		IL_0281: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions>(object)
		IL_0286: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_alignment(valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions)
		IL_028b: ldarg.0
		IL_028c: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TextMeshProFloatingText::m_textMeshPro
		IL_0291: stloc.s 31
		IL_0293: ldc.i4.0
		IL_0294: ldc.i4 255
		IL_0299: call int32 [UnityEngine.CoreModule]UnityEngine.Random::Range(int32, int32)
		IL_029e: stloc.s 32
		IL_02a0: ldc.i4.0
		IL_02a1: ldc.i4 255
		IL_02a6: call int32 [UnityEngine.CoreModule]UnityEngine.Random::Range(int32, int32)
		IL_02ab: stloc.s 33
		IL_02ad: ldc.i4.0
		IL_02ae: ldc.i4 255
		IL_02b3: call int32 [UnityEngine.CoreModule]UnityEngine.Random::Range(int32, int32)
		IL_02b8: stloc.s 34
		IL_02ba: ldloc.s 34
		IL_02bc: conv.r4
		IL_02bd: ldc.r4 255
		IL_02c2: div
		IL_02c3: stloc.s 4
		IL_02c5: ldarg.0
		IL_02c6: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TextMeshProFloatingText::m_textMeshPro
		IL_02cb: ldc.r4 24
		IL_02d0: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_fontSize(float32)
		IL_02d5: ldarg.0
		IL_02d6: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TextMeshProFloatingText::m_textMeshPro
		IL_02db: ldc.i4.0
		IL_02dc: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_enableKerning(bool)
		IL_02e1: ldstr "il2cpp_codegen_object_new(TMPro.Examples.TextMeshProFloatingText+<DisplayTextMeshProFloatingText>d__12)"
		IL_02e6: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_02eb: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'>(object)
		IL_02f0: stloc.s 35
		IL_02f2: ldloc.s 35
		IL_02f4: ldnull
		IL_02f5: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_02fa: ldloc.s 35
		IL_02fc: ldc.i4.0
		IL_02fd: stfld int32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<>1__state'
		IL_0302: ldloc.s 35
		IL_0304: ldarg.0
		IL_0305: stfld class TMPro.Examples.TextMeshProFloatingText TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<>4__this'
		IL_030a: ldloc.s 35
		IL_030c: stloc.s 36
		IL_030e: br IL_01a3

		IL_0313: nop
		IL_0314: ldnull
		IL_0315: throw
	} // end of method TextMeshProFloatingText::Start

	.method public hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator DisplayTextMeshProFloatingText () cil managed 
	{
		// Method begins at RVA 0x16be0
		// Header size: 12
		// Code size: 40 (0x28)
		.maxstack 2
		.locals init (
			[0] class TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(TMPro.Examples.TextMeshProFloatingText+<DisplayTextMeshProFloatingText>d__12)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class TMPro.Examples.TextMeshProFloatingText TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshProFloatingText>d__12'::'<>4__this'
		IL_0026: ldloc.0
		IL_0027: ret
	} // end of method TextMeshProFloatingText::DisplayTextMeshProFloatingText

	.method public hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator DisplayTextMeshFloatingText () cil managed 
	{
		// Method begins at RVA 0x16c14
		// Header size: 12
		// Code size: 40 (0x28)
		.maxstack 2
		.locals init (
			[0] class TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(TMPro.Examples.TextMeshProFloatingText+<DisplayTextMeshFloatingText>d__13)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class TMPro.Examples.TextMeshProFloatingText TMPro.Examples.TextMeshProFloatingText/'<DisplayTextMeshFloatingText>d__13'::'<>4__this'
		IL_0026: ldloc.0
		IL_0027: ret
	} // end of method TextMeshProFloatingText::DisplayTextMeshFloatingText

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x16c48
		// Header size: 12
		// Code size: 84 (0x54)
		.maxstack 2
		.locals init (
			[0] float32
		)

		IL_0000: nop
		IL_0001: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::zeroVector
		IL_0006: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_000b: stloc.0
		IL_000c: ldarg.0
		IL_000d: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.TextMeshProFloatingText::lastPOS
		IL_0012: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::zeroVector
		IL_0017: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_001c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0021: ldarg.0
		IL_0022: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.TextMeshProFloatingText::lastPOS
		IL_0027: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::zeroVector
		IL_002c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0031: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0036: ldarg.0
		IL_0037: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 TMPro.Examples.TextMeshProFloatingText::lastPOS
		IL_003c: ldloc.0
		IL_003d: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0042: ldarg.0
		IL_0043: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Quaternion::identityQuaternion
		IL_0048: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion TMPro.Examples.TextMeshProFloatingText::lastRotation
		IL_004d: ldarg.0
		IL_004e: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0053: ret
	} // end of method TextMeshProFloatingText::.ctor

} // end of class TMPro.Examples.TextMeshProFloatingText
