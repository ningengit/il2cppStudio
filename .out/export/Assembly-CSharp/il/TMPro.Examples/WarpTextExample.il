.class public auto ansi beforefieldinit TMPro.Examples.WarpTextExample
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested private auto ansi sealed beforefieldinit '<WarpText>d__8'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerator`1<object>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private object '<>2__current'
		.field public class TMPro.Examples.WarpTextExample '<>4__this'
		.field private float32 '<old_CurveScale>5__2'
		.field private class [UnityEngine.CoreModule]UnityEngine.AnimationCurve '<old_curve>5__3'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x2f80
			// Header size: 1
			// Code size: 16 (0x10)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ldarg.0
			IL_0009: ldarg.1
			IL_000a: stfld int32 TMPro.Examples.WarpTextExample/'<WarpText>d__8'::'<>1__state'
			IL_000f: ret
		} // end of method '<WarpText>d__8'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			// Method begins at RVA 0x2f91
			// Header size: 1
			// Code size: 2 (0x2)
			.maxstack 8

			IL_0000: nop
			IL_0001: ret
		} // end of method '<WarpText>d__8'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			// Method begins at RVA 0x20e2c
			// Header size: 12
			// Code size: 2105 (0x839)
			.maxstack 5
			.locals init (
				[0] int32,
				[1] class TMPro.Examples.WarpTextExample,
				[2] class TMPro.Examples.WarpTextExample,
				[3] int32,
				[4] class [UnityEngine.CoreModule]UnityEngine.AnimationCurve,
				[5] class [Unity.TextMeshPro]TMPro.TMP_Text,
				[6] float32,
				[7] class [UnityEngine.CoreModule]UnityEngine.AnimationCurve,
				[8] class [UnityEngine.CoreModule]UnityEngine.AnimationCurve,
				[9] valuetype [UnityEngine.CoreModule]UnityEngine.Keyframe[],
				[10] float32,
				[11] valuetype [UnityEngine.CoreModule]UnityEngine.Keyframe[],
				[12] float32,
				[13] valuetype [UnityEngine.CoreModule]UnityEngine.Keyframe[],
				[14] float32,
				[15] class [UnityEngine.CoreModule]UnityEngine.AnimationCurve,
				[16] class [Unity.TextMeshPro]TMPro.TMP_Text,
				[17] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[18] int32,
				[19] valuetype [UnityEngine.CoreModule]UnityEngine.Bounds,
				[20] int64,
				[21] int64,
				[22] float32,
				[23] valuetype [UnityEngine.CoreModule]UnityEngine.Bounds,
				[24] float32,
				[25] valuetype [Unity.TextMeshPro]TMPro.TMP_CharacterInfo[],
				[26] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[],
				[27] int64,
				[28] int64,
				[29] float32,
				[30] float32,
				[31] int32,
				[32] float32,
				[33] float32,
				[34] float32,
				[35] float32,
				[36] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[37] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[38] int64,
				[39] float32,
				[40] float32,
				[41] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[42] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
				[43] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[44] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[45] valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4,
				[46] int64,
				[47] int64,
				[48] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[49] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[50] float32,
				[51] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[52] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[53] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[54] float32,
				[55] float32,
				[56] int64,
				[57] int32,
				[58] class [UnityEngine.CoreModule]UnityEngine.WaitForSeconds
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld int32 TMPro.Examples.WarpTextExample/'<WarpText>d__8'::'<>1__state'
			IL_0007: stloc.0
			IL_0008: ldarg.0
			IL_0009: ldfld class TMPro.Examples.WarpTextExample TMPro.Examples.WarpTextExample/'<WarpText>d__8'::'<>4__this'
			IL_000e: stloc.1
			IL_000f: ldloc.1
			IL_0010: stloc.2
			IL_0011: ldloc.0
			IL_0012: stloc.3
			IL_0013: ldloc.0
			IL_0014: ldc.i4.0
			IL_0015: ceq
			IL_0017: brtrue.s IL_0040

			IL_0019: nop
			IL_001a: ldloc.3
			IL_001b: ldc.i4.1
			IL_001c: sub
			IL_001d: stloc.3
			IL_001e: ldloc.3
			IL_001f: ldc.i4.1
			IL_0020: sub
			IL_0021: ldc.i4.0
			IL_0022: ceq
			IL_0024: brtrue.s IL_0033

			IL_0026: nop
			IL_0027: ldloc.3
			IL_0028: ldc.i4.1
			IL_0029: ceq
			IL_002b: brtrue.s IL_0033

			IL_002d: nop
			IL_002e: br IL_081f

			IL_0033: nop
			IL_0034: ldarg.0
			IL_0035: ldc.i4.m1
			IL_0036: stfld int32 TMPro.Examples.WarpTextExample/'<WarpText>d__8'::'<>1__state'
			IL_003b: br IL_00d3

			IL_0040: nop
			IL_0041: ldarg.0
			IL_0042: ldc.i4.m1
			IL_0043: stfld int32 TMPro.Examples.WarpTextExample/'<WarpText>d__8'::'<>1__state'
			IL_0048: ldloc.2
			IL_0049: ldfld class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.WarpTextExample::VertexCurve
			IL_004e: stloc.s 4
			IL_0050: ldloc.s 4
			IL_0052: ldc.i4.1
			IL_0053: box [mscorlib]System.Int32
			IL_0058: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.WrapMode>(object)
			IL_005d: callvirt instance void [UnityEngine.CoreModule]UnityEngine.AnimationCurve::set_preWrapMode(valuetype [UnityEngine.CoreModule]UnityEngine.WrapMode)
			IL_0062: ldloc.2
			IL_0063: ldfld class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.WarpTextExample::VertexCurve
			IL_0068: stloc.s 4
			IL_006a: ldloc.s 4
			IL_006c: ldc.i4.1
			IL_006d: box [mscorlib]System.Int32
			IL_0072: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.WrapMode>(object)
			IL_0077: callvirt instance void [UnityEngine.CoreModule]UnityEngine.AnimationCurve::set_postWrapMode(valuetype [UnityEngine.CoreModule]UnityEngine.WrapMode)
			IL_007c: ldloc.2
			IL_007d: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.WarpTextExample::m_TextComponent
			IL_0082: stloc.s 5
			IL_0084: ldloc.s 5
			IL_0086: ldc.i4.1
			IL_0087: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_havePropertiesChanged(bool)
			IL_008c: ldloc.2
			IL_008d: ldfld float32 TMPro.Examples.WarpTextExample::CurveScale
			IL_0092: ldc.r4 10
			IL_0097: mul
			IL_0098: stloc.s 6
			IL_009a: ldloc.2
			IL_009b: ldloc.s 6
			IL_009d: stfld float32 TMPro.Examples.WarpTextExample::CurveScale
			IL_00a2: ldarg.0
			IL_00a3: ldloc.s 6
			IL_00a5: stfld float32 TMPro.Examples.WarpTextExample/'<WarpText>d__8'::'<old_CurveScale>5__2'
			IL_00aa: ldloc.2
			IL_00ab: ldfld class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.WarpTextExample::VertexCurve
			IL_00b0: stloc.s 7
			IL_00b2: newobj instance void [UnityEngine.CoreModule]UnityEngine.AnimationCurve::.ctor()
			IL_00b7: stloc.s 8
			IL_00b9: ldloc.s 7
			IL_00bb: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Keyframe[] [UnityEngine.CoreModule]UnityEngine.AnimationCurve::get_keys()
			IL_00c0: stloc.s 9
			IL_00c2: ldloc.s 8
			IL_00c4: ldloc.s 9
			IL_00c6: callvirt instance void [UnityEngine.CoreModule]UnityEngine.AnimationCurve::set_keys(valuetype [UnityEngine.CoreModule]UnityEngine.Keyframe[])
			IL_00cb: ldarg.0
			IL_00cc: ldloc.s 8
			IL_00ce: stfld class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.WarpTextExample/'<WarpText>d__8'::'<old_curve>5__3'

			IL_00d3: nop
			// loop start (head: IL_00d4)
				IL_00d4: nop
				IL_00d5: ldloc.2
				IL_00d6: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.WarpTextExample::m_TextComponent
				IL_00db: ldfld bool [Unity.TextMeshPro]TMPro.TMP_Text::m_havePropertiesChanged
				IL_00e0: ldc.i4.0
				IL_00e1: ceq
				IL_00e3: ldc.i4.0
				IL_00e4: ceq
				IL_00e6: brtrue.s IL_0164

				IL_00e8: nop
				IL_00e9: ldarg.0
				IL_00ea: ldfld float32 TMPro.Examples.WarpTextExample/'<WarpText>d__8'::'<old_CurveScale>5__2'
				IL_00ef: stloc.s 10
				IL_00f1: nop
				IL_00f2: ldarg.0
				IL_00f3: ldfld float32 TMPro.Examples.WarpTextExample/'<WarpText>d__8'::'<old_CurveScale>5__2'
				IL_00f8: ldloc.2
				IL_00f9: ldfld float32 TMPro.Examples.WarpTextExample::CurveScale
				IL_00fe: ceq
				IL_0100: ldc.i4.0
				IL_0101: ceq
				IL_0103: brtrue.s IL_0164

				IL_0105: nop
				IL_0106: ldarg.0
				IL_0107: ldfld class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.WarpTextExample/'<WarpText>d__8'::'<old_curve>5__3'
				IL_010c: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Keyframe[] [UnityEngine.CoreModule]UnityEngine.AnimationCurve::get_keys()
				IL_0111: stloc.s 11
				IL_0113: ldloc.s 11
				IL_0115: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
				IL_011a: ldc.i8 60
				IL_0123: add
				IL_0124: call instance float32 [UnityEngine.CoreModule]UnityEngine.Keyframe::get_value()
				IL_0129: stloc.s 12
				IL_012b: ldloc.s 12
				IL_012d: stloc.s 10
				IL_012f: ldloc.2
				IL_0130: ldfld class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.WarpTextExample::VertexCurve
				IL_0135: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Keyframe[] [UnityEngine.CoreModule]UnityEngine.AnimationCurve::get_keys()
				IL_013a: stloc.s 13
				IL_013c: ldloc.s 13
				IL_013e: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
				IL_0143: ldc.i8 60
				IL_014c: add
				IL_014d: call instance float32 [UnityEngine.CoreModule]UnityEngine.Keyframe::get_value()
				IL_0152: stloc.s 14
				IL_0154: ldloc.s 14
				IL_0156: stloc.s 10
				IL_0158: nop
				IL_0159: ldloc.s 12
				IL_015b: ldloc.s 14
				IL_015d: ceq
				IL_015f: brtrue IL_0822

				IL_0164: nop
				IL_0165: ldarg.0
				IL_0166: ldloc.2
				IL_0167: ldfld float32 TMPro.Examples.WarpTextExample::CurveScale
				IL_016c: stfld float32 TMPro.Examples.WarpTextExample/'<WarpText>d__8'::'<old_CurveScale>5__2'
				IL_0171: ldloc.2
				IL_0172: ldloc.2
				IL_0173: ldfld class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.WarpTextExample::VertexCurve
				IL_0178: callvirt instance class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.WarpTextExample::CopyAnimationCurve(class [UnityEngine.CoreModule]UnityEngine.AnimationCurve)
				IL_017d: stloc.s 15
				IL_017f: ldarg.0
				IL_0180: ldloc.s 15
				IL_0182: stfld class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.WarpTextExample/'<WarpText>d__8'::'<old_curve>5__3'
				IL_0187: ldloc.2
				IL_0188: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.WarpTextExample::m_TextComponent
				IL_018d: stloc.s 16
				IL_018f: ldloc.s 16
				IL_0191: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo [Unity.TextMeshPro]TMPro.TMP_Text::m_textInfo
				IL_0196: stloc.s 17
				IL_0198: ldloc.s 17
				IL_019a: ldfld int32 [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterCount
				IL_019f: stloc.s 18
				IL_01a1: ldloc.s 17
				IL_01a3: ldfld int32 [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterCount
				IL_01a8: ldc.i4.0
				IL_01a9: ceq
				IL_01ab: brtrue IL_00d4
			// end loop
			IL_01b0: nop
			IL_01b1: ldloc.2
			IL_01b2: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.WarpTextExample::m_TextComponent
			IL_01b7: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Bounds [Unity.TextMeshPro]TMPro.TMP_Text::get_bounds()
			IL_01bc: stloc.s 19
			IL_01be: ldstr "struct_chunk(LocalExpr+0x10, 8)"
			IL_01c3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_01c8: stloc.s 20
			IL_01ca: ldstr "shufps xmm1,xmm15,0FFh"
			IL_01cf: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_01d4: stloc.s 21
			IL_01d6: ldstr "struct_chunk(LocalExpr+0x0, 16)"
			IL_01db: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_01e0: conv.r4
			IL_01e1: ldstr "shufps xmm1,xmm15,0FFh"
			IL_01e6: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_01eb: conv.r4
			IL_01ec: sub
			IL_01ed: stloc.s 22
			IL_01ef: ldloc.2
			IL_01f0: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.WarpTextExample::m_TextComponent
			IL_01f5: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Bounds [Unity.TextMeshPro]TMPro.TMP_Text::get_bounds()
			IL_01fa: stloc.s 23
			IL_01fc: ldstr "struct_chunk(LocalExpr+0x10, 8)"
			IL_0201: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0206: stloc.s 20
			IL_0208: ldstr "struct_chunk(LocalExpr+0x0, 16)"
			IL_020d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0212: stloc.s 21
			IL_0214: ldstr "shufps xmm2,xmm1,0FFh"
			IL_0219: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_021e: conv.r4
			IL_021f: ldstr "struct_chunk(LocalExpr+0x0, 16)"
			IL_0224: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0229: conv.r4
			IL_022a: add
			IL_022b: stloc.s 24
			IL_022d: ldloc.s 18
			IL_022f: ldc.i4.0
			IL_0230: cgt
			IL_0232: ldc.i4.0
			IL_0233: ceq
			IL_0235: brtrue IL_0802

			IL_023a: nop
			// loop start (head: IL_023b)
				IL_023b: nop
				IL_023c: ldloc.s 17
				IL_023e: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_CharacterInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterInfo
				IL_0243: stloc.s 25
				IL_0245: ldstr "*(LocalExpr + 0x194)"
				IL_024a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_024f: ldc.i4.0
				IL_0250: conv.i8
				IL_0251: ceq
				IL_0253: brtrue IL_07f0

				IL_0258: nop
				IL_0259: ldloc.s 17
				IL_025b: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
				IL_0260: stloc.s 26
				IL_0262: ldstr "*(LocalExpr + 0x30)"
				IL_0267: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_026c: stloc.s 27
				IL_026e: ldstr "*(FieldExpr + 0x6C)"
				IL_0273: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0278: conv.i4
				IL_0279: conv.i8
				IL_027a: stloc.s 28
				IL_027c: ldstr "movss dword ptr [rbx+rcx*4+20h],xmm0 ; value = BinaryExpr"
				IL_0281: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0286: ldstr "movss dword ptr [rbx+rcx*4+24h],xmm0 ; value = BinaryExpr"
				IL_028b: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0290: ldstr "movss dword ptr [rbx+rcx*4+28h],xmm0 ; value = BinaryExpr"
				IL_0295: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_029a: ldc.r4 -0.0
				IL_029f: ldstr "*(LocalExpr + 0x28)"
				IL_02a4: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_02a9: conv.i4
				IL_02aa: conv.r4
				IL_02ab: add
				IL_02ac: stloc.s 29
				IL_02ae: ldstr "*(LocalExpr + 0x20)"
				IL_02b3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_02b8: conv.i4
				IL_02b9: conv.r4
				IL_02ba: ldstr "*(LocalExpr + 0x20)"
				IL_02bf: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_02c4: conv.i4
				IL_02c5: conv.r4
				IL_02c6: add
				IL_02c7: ldc.r4 0.5
				IL_02cc: mul
				IL_02cd: stloc.s 30
				IL_02cf: ldstr "*(FieldExpr + 0x14C)"
				IL_02d4: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_02d9: conv.i4
				IL_02da: stloc.s 31
				IL_02dc: ldstr "movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr"
				IL_02e1: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_02e6: ldstr "movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr"
				IL_02eb: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_02f0: ldstr "movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr"
				IL_02f5: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_02fa: ldc.r4 -0.0
				IL_02ff: ldstr "*(LocalExpr + 0x28)"
				IL_0304: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0309: conv.i4
				IL_030a: conv.r4
				IL_030b: add
				IL_030c: stloc.s 29
				IL_030e: ldstr "movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr"
				IL_0313: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0318: ldstr "movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr"
				IL_031d: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0322: ldstr "movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr"
				IL_0327: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_032c: ldc.r4 -0.0
				IL_0331: ldstr "*(LocalExpr + 0x28)"
				IL_0336: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_033b: conv.i4
				IL_033c: conv.r4
				IL_033d: add
				IL_033e: stloc.s 29
				IL_0340: ldstr "movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr"
				IL_0345: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_034a: ldstr "movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr"
				IL_034f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0354: ldstr "movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr"
				IL_0359: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_035e: ldloc.s 24
				IL_0360: ldloc.s 22
				IL_0362: sub
				IL_0363: stloc.s 29
				IL_0365: ldloc.2
				IL_0366: ldfld class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.WarpTextExample::VertexCurve
				IL_036b: ldloc.s 30
				IL_036d: ldloc.s 22
				IL_036f: sub
				IL_0370: ldloc.s 24
				IL_0372: ldloc.s 22
				IL_0374: sub
				IL_0375: div
				IL_0376: callvirt instance float32 [UnityEngine.CoreModule]UnityEngine.AnimationCurve::Evaluate(float32)
				IL_037b: stloc.s 32
				IL_037d: ldloc.s 32
				IL_037f: stloc.s 29
				IL_0381: ldloc.2
				IL_0382: ldfld float32 TMPro.Examples.WarpTextExample::CurveScale
				IL_0387: stloc.s 33
				IL_0389: ldloc.2
				IL_038a: ldfld class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.WarpTextExample::VertexCurve
				IL_038f: ldloc.s 30
				IL_0391: ldloc.s 22
				IL_0393: sub
				IL_0394: ldloc.s 24
				IL_0396: ldloc.s 22
				IL_0398: sub
				IL_0399: div
				IL_039a: ldc.r4 0.0001
				IL_039f: add
				IL_03a0: callvirt instance float32 [UnityEngine.CoreModule]UnityEngine.AnimationCurve::Evaluate(float32)
				IL_03a5: stloc.s 34
				IL_03a7: ldloc.s 34
				IL_03a9: ldloc.2
				IL_03aa: ldfld float32 TMPro.Examples.WarpTextExample::CurveScale
				IL_03af: mul
				IL_03b0: ldloc.s 33
				IL_03b2: ldloc.s 32
				IL_03b4: mul
				IL_03b5: sub
				IL_03b6: stloc.s 35
				IL_03b8: ldloca.s 36
				IL_03ba: ldstr "stack[0xFFFFFFFFFFFFFE48]"
				IL_03bf: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_03c4: conv.i4
				IL_03c5: conv.r4
				IL_03c6: ldloc.s 22
				IL_03c8: sub
				IL_03c9: ldloc.s 30
				IL_03cb: ldloc.s 22
				IL_03cd: sub
				IL_03ce: ldloc.s 24
				IL_03d0: ldloc.s 22
				IL_03d2: sub
				IL_03d3: div
				IL_03d4: ldc.r4 0.0001
				IL_03d9: add
				IL_03da: mul
				IL_03db: ldloc.s 22
				IL_03dd: add
				IL_03de: ldloc.s 30
				IL_03e0: sub
				IL_03e1: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_03e6: ldloca.s 36
				IL_03e8: ldloc.s 35
				IL_03ea: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
				IL_03ef: ldloca.s 36
				IL_03f1: ldc.r4 0.0
				IL_03f6: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_03fb: ldloca.s 36
				IL_03fd: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::get_normalized()
				IL_0402: stloc.s 37
				IL_0404: ldstr "sub_20C3F0"
				IL_0409: ldc.i8 0
				IL_0412: ldc.i8 0
				IL_041b: ldc.i8 0
				IL_0424: ldc.i8 0
				IL_042d: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
				IL_0432: stloc.s 38
				IL_0434: ldstr "xmm0"
				IL_0439: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_043e: conv.r4
				IL_043f: ldc.r4 57.29578
				IL_0444: mul
				IL_0445: stloc.s 39
				IL_0447: ldloc.s 33
				IL_0449: ldloc.s 32
				IL_044b: mul
				IL_044c: stloc.s 40
				IL_044e: ldloc.s 35
				IL_0450: ldstr "stack[0xFFFFFFFFFFFFFE48]"
				IL_0455: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_045a: conv.i4
				IL_045b: conv.r4
				IL_045c: ldloc.s 22
				IL_045e: sub
				IL_045f: ldloc.s 30
				IL_0461: ldloc.s 22
				IL_0463: sub
				IL_0464: ldloc.s 24
				IL_0466: ldloc.s 22
				IL_0468: sub
				IL_0469: div
				IL_046a: ldc.r4 0.0001
				IL_046f: add
				IL_0470: mul
				IL_0471: ldloc.s 22
				IL_0473: add
				IL_0474: ldloc.s 30
				IL_0476: sub
				IL_0477: ldc.r4 0.0
				IL_047c: mul
				IL_047d: sub
				IL_047e: ldc.r4 0.0
				IL_0483: cgt
				IL_0485: brtrue.s IL_049c

				IL_0487: nop
				IL_0488: ldc.r4 360
				IL_048d: ldloc.s 39
				IL_048f: sub
				IL_0490: stloc.s 39
				IL_0492: ldc.r4 360
				IL_0497: ldloc.s 39
				IL_0499: sub
				IL_049a: stloc.s 29

				IL_049c: nop
				IL_049d: ldloca.s 41
				IL_049f: ldc.r4 0.0
				IL_04a4: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_04a9: ldloca.s 41
				IL_04ab: ldloc.s 39
				IL_04ad: ldc.r4 0.017453292
				IL_04b2: mul
				IL_04b3: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_04b8: ldloc.s 41
				IL_04ba: call valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_FromEulerRad(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_04bf: stloc.s 42
				IL_04c1: ldloca.s 43
				IL_04c3: ldc.r4 0.0
				IL_04c8: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_04cd: ldloca.s 43
				IL_04cf: ldloc.s 40
				IL_04d1: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
				IL_04d6: ldloca.s 43
				IL_04d8: ldc.r4 0.0
				IL_04dd: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_04e2: ldloc.s 43
				IL_04e4: ldstr "&stack[0xFFFFFFFFFFFFFE78]"
				IL_04e9: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_04ee: conv.i8
				IL_04ef: ldc.i8 32
				IL_04f8: add
				IL_04f9: box [mscorlib]System.Int64
				IL_04fe: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion>(object)
				IL_0503: ldloca.s 44
				IL_0505: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::oneVector
				IL_050a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_050f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_0514: ldloca.s 44
				IL_0516: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::oneVector
				IL_051b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
				IL_0520: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
				IL_0525: ldloca.s 44
				IL_0527: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::oneVector
				IL_052c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_0531: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_0536: ldloc.s 44
				IL_0538: call valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::TRS(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_053d: stloc.s 45
				IL_053f: ldstr "struct_chunk(LocalExpr+0x20, 16)"
				IL_0544: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0549: stloc.s 46
				IL_054b: ldstr "struct_chunk(LocalExpr+0x30, 16)"
				IL_0550: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0555: stloc.s 47
				IL_0557: ldstr "&stack[0xFFFFFFFFFFFFFE78]"
				IL_055c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0561: conv.i8
				IL_0562: ldc.i8 -32
				IL_056b: add
				IL_056c: ldloca.s 48
				IL_056e: ldstr "*(LocalExpr + 0x20)"
				IL_0573: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0578: box [mscorlib]System.Int64
				IL_057d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
				IL_0582: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_0587: ldloca.s 48
				IL_0589: ldstr "*(LocalExpr + 0x28)"
				IL_058e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0593: conv.i4
				IL_0594: box [mscorlib]System.Int32
				IL_0599: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
				IL_059e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_05a3: ldloc.s 48
				IL_05a5: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_05aa: stloc.s 49
				IL_05ac: ldstr "movsd qword ptr [rbx+rdx*4+20h],xmm0 ; value = StructChunkExpr"
				IL_05b1: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_05b6: ldstr "mov [rbx+rdx*4+28h],r8d ; value = FieldExpr"
				IL_05bb: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_05c0: ldloca.s 49
				IL_05c2: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_05c7: stloc.s 50
				IL_05c9: ldstr "struct_chunk(LocalExpr+0x0, 8)"
				IL_05ce: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_05d3: stloc.s 46
				IL_05d5: ldstr "&stack[0xFFFFFFFFFFFFFE78]"
				IL_05da: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_05df: conv.i8
				IL_05e0: ldc.i8 -32
				IL_05e9: add
				IL_05ea: ldstr "&stack[0xFFFFFFFFFFFFFE78]"
				IL_05ef: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_05f4: conv.i8
				IL_05f5: ldc.i8 -128
				IL_05fe: add
				IL_05ff: box [mscorlib]System.Int64
				IL_0604: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
				IL_0609: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_060e: stloc.s 51
				IL_0610: ldstr "movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr"
				IL_0615: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_061a: ldstr "mov [rbx+rcx*4+28h],edx ; value = FieldExpr"
				IL_061f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0624: ldstr "struct_chunk(LocalExpr+0x0, 8)"
				IL_0629: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_062e: stloc.s 46
				IL_0630: ldstr "&stack[0xFFFFFFFFFFFFFE78]"
				IL_0635: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_063a: conv.i8
				IL_063b: ldc.i8 -32
				IL_0644: add
				IL_0645: ldstr "&stack[0xFFFFFFFFFFFFFE78]"
				IL_064a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_064f: conv.i8
				IL_0650: ldc.i8 -112
				IL_0659: add
				IL_065a: box [mscorlib]System.Int64
				IL_065f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
				IL_0664: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_0669: stloc.s 52
				IL_066b: ldstr "movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr"
				IL_0670: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0675: ldstr "mov [rbx+rcx*4+28h],edx ; value = FieldExpr"
				IL_067a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_067f: ldstr "struct_chunk(LocalExpr+0x0, 8)"
				IL_0684: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0689: stloc.s 46
				IL_068b: ldstr "&stack[0xFFFFFFFFFFFFFE78]"
				IL_0690: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0695: conv.i8
				IL_0696: ldc.i8 -32
				IL_069f: add
				IL_06a0: ldstr "&stack[0xFFFFFFFFFFFFFE78]"
				IL_06a5: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_06aa: conv.i8
				IL_06ab: ldc.i8 -96
				IL_06b4: add
				IL_06b5: box [mscorlib]System.Int64
				IL_06ba: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
				IL_06bf: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_06c4: stloc.s 53
				IL_06c6: ldstr "movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr"
				IL_06cb: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_06d0: ldstr "mov [rbx+rcx*4+28h],edx ; value = FieldExpr"
				IL_06d5: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_06da: ldstr "struct_chunk(LocalExpr+0x0, 8)"
				IL_06df: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_06e4: stloc.s 46
				IL_06e6: ldstr "movss dword ptr [rbx+rcx*4+20h],xmm0 ; value = BinaryExpr"
				IL_06eb: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_06f0: ldstr "movss dword ptr [rbx+rcx*4+24h],xmm0 ; value = BinaryExpr"
				IL_06f5: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_06fa: ldstr "movss dword ptr [rbx+rcx*4+28h],xmm1 ; value = BinaryExpr"
				IL_06ff: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0704: ldloc.s 31
				IL_0706: conv.r4
				IL_0707: ldstr "*(LocalExpr + 0x24)"
				IL_070c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0711: conv.i4
				IL_0712: conv.r4
				IL_0713: add
				IL_0714: stloc.s 54
				IL_0716: ldc.r4 0.0
				IL_071b: ldstr "*(LocalExpr + 0x28)"
				IL_0720: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0725: conv.i4
				IL_0726: conv.r4
				IL_0727: add
				IL_0728: stloc.s 55
				IL_072a: ldstr "movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr"
				IL_072f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0734: ldstr "movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr"
				IL_0739: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_073e: ldstr "movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr"
				IL_0743: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0748: ldloc.s 31
				IL_074a: conv.r4
				IL_074b: ldstr "*(LocalExpr + 0x24)"
				IL_0750: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0755: conv.i4
				IL_0756: conv.r4
				IL_0757: add
				IL_0758: stloc.s 54
				IL_075a: ldc.r4 0.0
				IL_075f: ldstr "*(LocalExpr + 0x28)"
				IL_0764: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0769: conv.i4
				IL_076a: conv.r4
				IL_076b: add
				IL_076c: stloc.s 55
				IL_076e: ldstr "movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr"
				IL_0773: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0778: ldstr "movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr"
				IL_077d: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0782: ldstr "movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr"
				IL_0787: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_078c: ldloc.s 31
				IL_078e: conv.r4
				IL_078f: ldstr "*(LocalExpr + 0x24)"
				IL_0794: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0799: conv.i4
				IL_079a: conv.r4
				IL_079b: add
				IL_079c: stloc.s 54
				IL_079e: ldc.r4 0.0
				IL_07a3: ldstr "*(LocalExpr + 0x28)"
				IL_07a8: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_07ad: conv.i4
				IL_07ae: conv.r4
				IL_07af: add
				IL_07b0: stloc.s 55
				IL_07b2: ldstr "movss dword ptr [rbx+rax*4+20h],xmm8 ; value = BinaryExpr"
				IL_07b7: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_07bc: ldstr "movss dword ptr [rbx+rax*4+24h],xmm9 ; value = BinaryExpr"
				IL_07c1: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_07c6: ldstr "movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr"
				IL_07cb: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_07d0: ldc.r4 0.0
				IL_07d5: ldstr "*(LocalExpr + 0x28)"
				IL_07da: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_07df: conv.i4
				IL_07e0: conv.r4
				IL_07e1: add
				IL_07e2: stloc.s 54
				IL_07e4: ldstr "stack[0xFFFFFFFFFFFFFE48]"
				IL_07e9: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_07ee: stloc.s 56

				IL_07f0: nop
				IL_07f1: ldc.i4.0
				IL_07f2: ldc.i4.1
				IL_07f3: add
				IL_07f4: stloc.s 57
				IL_07f6: ldc.i4.0
				IL_07f7: ldc.i4.1
				IL_07f8: add
				IL_07f9: ldloc.s 18
				IL_07fb: clt
				IL_07fd: brtrue IL_023b
			// end loop

			IL_0802: nop
			IL_0803: ldc.r4 0.025
			IL_0808: newobj instance void [UnityEngine.CoreModule]UnityEngine.WaitForSeconds::.ctor(float32)
			IL_080d: stloc.s 58
			IL_080f: ldarg.0
			IL_0810: ldloc.s 58
			IL_0812: stfld object TMPro.Examples.WarpTextExample/'<WarpText>d__8'::'<>2__current'
			IL_0817: ldarg.0
			IL_0818: ldc.i4.2
			IL_0819: stfld int32 TMPro.Examples.WarpTextExample/'<WarpText>d__8'::'<>1__state'

			IL_081e: nop

			IL_081f: nop
			IL_0820: ldc.i4.1
			IL_0821: ret

			IL_0822: nop
			IL_0823: ldarg.0
			IL_0824: ldnull
			IL_0825: stfld object TMPro.Examples.WarpTextExample/'<WarpText>d__8'::'<>2__current'
			IL_082a: ldarg.0
			IL_082b: ldc.i4.1
			IL_082c: stfld int32 TMPro.Examples.WarpTextExample/'<WarpText>d__8'::'<>1__state'
			IL_0831: br.s IL_081e

			IL_0833: nop
			IL_0834: ldnull
			IL_0835: throw

			IL_0836: nop
			IL_0837: ldnull
			IL_0838: throw
		} // end of method '<WarpText>d__8'::MoveNext

		.method private final hidebysig specialname newslot virtual 
			instance object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () cil managed 
		{
			// Method begins at RVA 0x2f94
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.WarpTextExample/'<WarpText>d__8'::'<>2__current'
			IL_0007: ret
		} // end of method '<WarpText>d__8'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'

		.method private final hidebysig newslot virtual 
			instance void System.Collections.IEnumerator.Reset () cil managed 
		{
			// Method begins at RVA 0x21674
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
		} // end of method '<WarpText>d__8'::System.Collections.IEnumerator.Reset

		.method private final hidebysig specialname newslot virtual 
			instance object System.Collections.IEnumerator.get_Current () cil managed 
		{
			// Method begins at RVA 0x2f9d
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.WarpTextExample/'<WarpText>d__8'::'<>2__current'
			IL_0007: ret
		} // end of method '<WarpText>d__8'::System.Collections.IEnumerator.get_Current

		// Properties
		.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current'()
		{
			.get instance object TMPro.Examples.WarpTextExample/'<WarpText>d__8'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'()
		}
		.property instance object System.Collections.IEnumerator.Current()
		{
			.get instance object TMPro.Examples.WarpTextExample/'<WarpText>d__8'::System.Collections.IEnumerator.get_Current()
		}

	} // end of class <WarpText>d__8


	// Fields
	.field private class [Unity.TextMeshPro]TMPro.TMP_Text m_TextComponent
	.field public class [UnityEngine.CoreModule]UnityEngine.AnimationCurve VertexCurve
	.field public float32 AngleMultiplier
	.field public float32 SpeedMultiplier
	.field public float32 CurveScale

	// Methods
	.method private hidebysig 
		instance void Awake () cil managed 
	{
		// Method begins at RVA 0x19a9c
		// Header size: 12
		// Code size: 26 (0x1a)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[1] class [Unity.TextMeshPro]TMPro.TMP_Text
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::GetComponent<class [Unity.TextMeshPro]TMPro.TMP_Text>()
		IL_000e: stloc.1
		IL_000f: ldarg.0
		IL_0010: ldloc.1
		IL_0011: stfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.WarpTextExample::m_TextComponent
		IL_0016: ret

		IL_0017: nop
		IL_0018: ldnull
		IL_0019: throw
	} // end of method WarpTextExample::Awake

	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x19ac4
		// Header size: 12
		// Code size: 47 (0x2f)
		.maxstack 2
		.locals init (
			[0] class TMPro.Examples.WarpTextExample/'<WarpText>d__8',
			[1] class [UnityEngine.CoreModule]UnityEngine.Coroutine
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(TMPro.Examples.WarpTextExample+<WarpText>d__8)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.WarpTextExample/'<WarpText>d__8'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 TMPro.Examples.WarpTextExample/'<WarpText>d__8'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class TMPro.Examples.WarpTextExample TMPro.Examples.WarpTextExample/'<WarpText>d__8'::'<>4__this'
		IL_0026: ldarg.0
		IL_0027: ldloc.0
		IL_0028: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Coroutine [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StartCoroutine(class [mscorlib]System.Collections.IEnumerator)
		IL_002d: stloc.1
		IL_002e: ret
	} // end of method WarpTextExample::Start

	.method private hidebysig 
		instance class [UnityEngine.CoreModule]UnityEngine.AnimationCurve CopyAnimationCurve (
			class [UnityEngine.CoreModule]UnityEngine.AnimationCurve curve
		) cil managed 
	{
		// Method begins at RVA 0x19b00
		// Header size: 12
		// Code size: 26 (0x1a)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.AnimationCurve,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.Keyframe[]
		)

		IL_0000: nop
		IL_0001: newobj instance void [UnityEngine.CoreModule]UnityEngine.AnimationCurve::.ctor()
		IL_0006: stloc.0
		IL_0007: ldarg.1
		IL_0008: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Keyframe[] [UnityEngine.CoreModule]UnityEngine.AnimationCurve::get_keys()
		IL_000d: stloc.1
		IL_000e: ldloc.0
		IL_000f: ldloc.1
		IL_0010: callvirt instance void [UnityEngine.CoreModule]UnityEngine.AnimationCurve::set_keys(valuetype [UnityEngine.CoreModule]UnityEngine.Keyframe[])
		IL_0015: ldloc.0
		IL_0016: ret

		IL_0017: nop
		IL_0018: ldnull
		IL_0019: throw
	} // end of method WarpTextExample::CopyAnimationCurve

	.method private hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator WarpText () cil managed 
	{
		// Method begins at RVA 0x19b28
		// Header size: 12
		// Code size: 40 (0x28)
		.maxstack 2
		.locals init (
			[0] class TMPro.Examples.WarpTextExample/'<WarpText>d__8'
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(TMPro.Examples.WarpTextExample+<WarpText>d__8)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.WarpTextExample/'<WarpText>d__8'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 TMPro.Examples.WarpTextExample/'<WarpText>d__8'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class TMPro.Examples.WarpTextExample TMPro.Examples.WarpTextExample/'<WarpText>d__8'::'<>4__this'
		IL_0026: ldloc.0
		IL_0027: ret
	} // end of method WarpTextExample::WarpText

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x19b5c
		// Header size: 12
		// Code size: 408 (0x198)
		.maxstack 3
		.locals init (
			[0] valuetype [UnityEngine.CoreModule]UnityEngine.Keyframe[],
			[1] class [UnityEngine.CoreModule]UnityEngine.AnimationCurve
		)

		IL_0000: nop
		IL_0001: ldc.i4.5
		IL_0002: newarr [UnityEngine.CoreModule]UnityEngine.Keyframe
		IL_0007: stloc.0
		IL_0008: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_000d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0012: conv.i8
		IL_0013: ldc.i8 -73
		IL_001c: add
		IL_001d: ldc.r4 0.0
		IL_0022: ldc.r4 0.0
		IL_0027: call instance void [UnityEngine.CoreModule]UnityEngine.Keyframe::.ctor(float32, float32)
		IL_002c: ldloc.0
		IL_002d: ldc.i4.0
		IL_002e: ldc.r4 0.0
		IL_0033: box [mscorlib]System.Single
		IL_0038: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Keyframe>(object)
		IL_003d: stelem [UnityEngine.CoreModule]UnityEngine.Keyframe
		IL_0042: ldstr "movups [rdi+2Ch],xmm1 ; value = FloatExpr"
		IL_0047: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_004c: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0051: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0056: conv.i8
		IL_0057: ldc.i8 -41
		IL_0060: add
		IL_0061: ldc.r4 0.25
		IL_0066: ldc.r4 2
		IL_006b: call instance void [UnityEngine.CoreModule]UnityEngine.Keyframe::.ctor(float32, float32)
		IL_0070: ldloc.0
		IL_0071: ldc.i4.1
		IL_0072: ldc.r4 0.0
		IL_0077: box [mscorlib]System.Single
		IL_007c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Keyframe>(object)
		IL_0081: stelem [UnityEngine.CoreModule]UnityEngine.Keyframe
		IL_0086: ldstr "movups [rdi+48h],xmm1 ; value = FloatExpr"
		IL_008b: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0090: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0095: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_009a: conv.i8
		IL_009b: ldc.i8 -9
		IL_00a4: add
		IL_00a5: ldc.r4 0.5
		IL_00aa: ldc.r4 0.0
		IL_00af: call instance void [UnityEngine.CoreModule]UnityEngine.Keyframe::.ctor(float32, float32)
		IL_00b4: ldloc.0
		IL_00b5: ldc.i4.2
		IL_00b6: ldc.r4 0.0
		IL_00bb: box [mscorlib]System.Single
		IL_00c0: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Keyframe>(object)
		IL_00c5: stelem [UnityEngine.CoreModule]UnityEngine.Keyframe
		IL_00ca: ldstr "movups [rdi+64h],xmm1 ; value = FloatExpr"
		IL_00cf: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_00d4: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_00d9: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00de: conv.i8
		IL_00df: ldc.i8 23
		IL_00e8: add
		IL_00e9: ldc.r4 0.75
		IL_00ee: ldc.r4 2
		IL_00f3: call instance void [UnityEngine.CoreModule]UnityEngine.Keyframe::.ctor(float32, float32)
		IL_00f8: ldloc.0
		IL_00f9: ldc.i4.3
		IL_00fa: ldc.r4 0.0
		IL_00ff: box [mscorlib]System.Single
		IL_0104: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Keyframe>(object)
		IL_0109: stelem [UnityEngine.CoreModule]UnityEngine.Keyframe
		IL_010e: ldstr "movups [rdi+80h],xmm1 ; value = FloatExpr"
		IL_0113: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0118: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_011d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0122: conv.i8
		IL_0123: ldc.i8 55
		IL_012c: add
		IL_012d: ldc.r4 1
		IL_0132: ldc.r4 0.0
		IL_0137: call instance void [UnityEngine.CoreModule]UnityEngine.Keyframe::.ctor(float32, float32)
		IL_013c: ldloc.0
		IL_013d: ldc.i4.4
		IL_013e: ldc.r4 0.0
		IL_0143: box [mscorlib]System.Single
		IL_0148: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Keyframe>(object)
		IL_014d: stelem [UnityEngine.CoreModule]UnityEngine.Keyframe
		IL_0152: ldstr "movups [rdi+9Ch],xmm1 ; value = FloatExpr"
		IL_0157: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_015c: ldloc.0
		IL_015d: newobj instance void [UnityEngine.CoreModule]UnityEngine.AnimationCurve::.ctor(valuetype [UnityEngine.CoreModule]UnityEngine.Keyframe[])
		IL_0162: stloc.1
		IL_0163: ldarg.0
		IL_0164: ldloc.1
		IL_0165: stfld class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.WarpTextExample::VertexCurve
		IL_016a: ldarg.0
		IL_016b: ldc.r4 1
		IL_0170: stfld float32 TMPro.Examples.WarpTextExample::AngleMultiplier
		IL_0175: ldarg.0
		IL_0176: ldc.r4 1
		IL_017b: stfld float32 TMPro.Examples.WarpTextExample::SpeedMultiplier
		IL_0180: ldarg.0
		IL_0181: ldc.r4 1
		IL_0186: stfld float32 TMPro.Examples.WarpTextExample::CurveScale
		IL_018b: ldarg.0
		IL_018c: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0191: ret

		IL_0192: nop
		IL_0193: ldnull
		IL_0194: throw

		IL_0195: nop
		IL_0196: ldnull
		IL_0197: throw
	} // end of method WarpTextExample::.ctor

} // end of class TMPro.Examples.WarpTextExample
