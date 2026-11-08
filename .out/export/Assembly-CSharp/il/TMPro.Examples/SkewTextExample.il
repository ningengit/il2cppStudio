.class public auto ansi beforefieldinit TMPro.Examples.SkewTextExample
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested private auto ansi sealed beforefieldinit '<WarpText>d__7'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerator`1<object>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private object '<>2__current'
		.field public class TMPro.Examples.SkewTextExample '<>4__this'
		.field private float32 '<old_CurveScale>5__2'
		.field private float32 '<old_ShearValue>5__3'
		.field private class [UnityEngine.CoreModule]UnityEngine.AnimationCurve '<old_curve>5__4'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x2dd4
			// Header size: 1
			// Code size: 16 (0x10)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ldarg.0
			IL_0009: ldarg.1
			IL_000a: stfld int32 TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<>1__state'
			IL_000f: ret
		} // end of method '<WarpText>d__7'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			// Method begins at RVA 0x2de5
			// Header size: 1
			// Code size: 2 (0x2)
			.maxstack 8

			IL_0000: nop
			IL_0001: ret
		} // end of method '<WarpText>d__7'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			// Method begins at RVA 0x1b6fc
			// Header size: 12
			// Code size: 2689 (0xa81)
			.maxstack 5
			.locals init (
				[0] class TMPro.Examples.SkewTextExample,
				[1] int32,
				[2] class TMPro.Examples.SkewTextExample,
				[3] int64,
				[4] int32,
				[5] float32,
				[6] class [UnityEngine.CoreModule]UnityEngine.AnimationCurve,
				[7] class [UnityEngine.CoreModule]UnityEngine.AnimationCurve,
				[8] valuetype [UnityEngine.CoreModule]UnityEngine.Keyframe[],
				[9] float32,
				[10] valuetype [UnityEngine.CoreModule]UnityEngine.Keyframe[],
				[11] float32,
				[12] valuetype [UnityEngine.CoreModule]UnityEngine.Keyframe[],
				[13] float32,
				[14] class [UnityEngine.CoreModule]UnityEngine.AnimationCurve,
				[15] class [Unity.TextMeshPro]TMPro.TMP_Text,
				[16] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[17] int32,
				[18] valuetype [UnityEngine.CoreModule]UnityEngine.Bounds,
				[19] int64,
				[20] int64,
				[21] float32,
				[22] valuetype [UnityEngine.CoreModule]UnityEngine.Bounds,
				[23] float32,
				[24] valuetype [Unity.TextMeshPro]TMPro.TMP_CharacterInfo[],
				[25] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[],
				[26] int64,
				[27] int64,
				[28] float32,
				[29] float32,
				[30] int32,
				[31] valuetype [Unity.TextMeshPro]TMPro.TMP_CharacterInfo[],
				[32] float32,
				[33] float32,
				[34] float32,
				[35] float32,
				[36] float32,
				[37] float32,
				[38] float32,
				[39] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[40] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[41] int64,
				[42] float32,
				[43] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[44] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
				[45] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[46] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[47] valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4,
				[48] int64,
				[49] int64,
				[50] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[51] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[52] float32,
				[53] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[54] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[55] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[56] float32,
				[57] float32,
				[58] int64,
				[59] int32
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld class TMPro.Examples.SkewTextExample TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<>4__this'
			IL_0007: stloc.0
			IL_0008: ldarg.0
			IL_0009: ldfld int32 TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<>1__state'
			IL_000e: stloc.1
			IL_000f: ldstr "movups [rbp-20h],xmm0 ; value = FloatExpr"
			IL_0014: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
			IL_0019: ldstr "movups [rbp-10h],xmm0 ; value = FloatExpr"
			IL_001e: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
			IL_0023: ldstr "movups [rbp],xmm0 ; value = FloatExpr"
			IL_0028: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
			IL_002d: ldstr "movups [rbp+10h],xmm0 ; value = FloatExpr"
			IL_0032: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
			IL_0037: ldloc.0
			IL_0038: stloc.2
			IL_0039: ldstr "&stack[0x0]"
			IL_003e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0043: conv.i8
			IL_0044: ldc.i8 -424
			IL_004d: add
			IL_004e: stloc.3
			IL_004f: ldloc.1
			IL_0050: stloc.s 4
			IL_0052: ldloc.1
			IL_0053: ldc.i4.0
			IL_0054: ceq
			IL_0056: brtrue.s IL_0083

			IL_0058: nop
			IL_0059: ldloc.s 4
			IL_005b: ldc.i4.1
			IL_005c: sub
			IL_005d: stloc.s 4
			IL_005f: ldloc.s 4
			IL_0061: ldc.i4.1
			IL_0062: sub
			IL_0063: ldc.i4.0
			IL_0064: ceq
			IL_0066: brtrue.s IL_0076

			IL_0068: nop
			IL_0069: ldloc.s 4
			IL_006b: ldc.i4.1
			IL_006c: ceq
			IL_006e: brtrue.s IL_0076

			IL_0070: nop
			IL_0071: br IL_0a67

			IL_0076: nop
			IL_0077: ldarg.0
			IL_0078: ldc.i4.m1
			IL_0079: stfld int32 TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<>1__state'
			IL_007e: br IL_0116

			IL_0083: nop
			IL_0084: ldarg.0
			IL_0085: ldc.i4.m1
			IL_0086: stfld int32 TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<>1__state'
			IL_008b: ldloc.2
			IL_008c: ldfld class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.SkewTextExample::VertexCurve
			IL_0091: ldc.i4.1
			IL_0092: box [mscorlib]System.Int32
			IL_0097: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.WrapMode>(object)
			IL_009c: callvirt instance void [UnityEngine.CoreModule]UnityEngine.AnimationCurve::set_preWrapMode(valuetype [UnityEngine.CoreModule]UnityEngine.WrapMode)
			IL_00a1: ldloc.2
			IL_00a2: ldfld class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.SkewTextExample::VertexCurve
			IL_00a7: ldc.i4.1
			IL_00a8: box [mscorlib]System.Int32
			IL_00ad: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.WrapMode>(object)
			IL_00b2: callvirt instance void [UnityEngine.CoreModule]UnityEngine.AnimationCurve::set_postWrapMode(valuetype [UnityEngine.CoreModule]UnityEngine.WrapMode)
			IL_00b7: ldloc.2
			IL_00b8: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.SkewTextExample::m_TextComponent
			IL_00bd: ldc.i4.1
			IL_00be: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_havePropertiesChanged(bool)
			IL_00c3: ldloc.2
			IL_00c4: ldfld float32 TMPro.Examples.SkewTextExample::CurveScale
			IL_00c9: ldc.r4 10
			IL_00ce: mul
			IL_00cf: stloc.s 5
			IL_00d1: ldloc.2
			IL_00d2: ldloc.s 5
			IL_00d4: stfld float32 TMPro.Examples.SkewTextExample::CurveScale
			IL_00d9: ldarg.0
			IL_00da: ldloc.s 5
			IL_00dc: stfld float32 TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<old_CurveScale>5__2'
			IL_00e1: ldarg.0
			IL_00e2: ldloc.2
			IL_00e3: ldfld float32 TMPro.Examples.SkewTextExample::ShearAmount
			IL_00e8: stfld float32 TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<old_ShearValue>5__3'
			IL_00ed: ldloc.2
			IL_00ee: ldfld class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.SkewTextExample::VertexCurve
			IL_00f3: stloc.s 6
			IL_00f5: newobj instance void [UnityEngine.CoreModule]UnityEngine.AnimationCurve::.ctor()
			IL_00fa: stloc.s 7
			IL_00fc: ldloc.s 6
			IL_00fe: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Keyframe[] [UnityEngine.CoreModule]UnityEngine.AnimationCurve::get_keys()
			IL_0103: stloc.s 8
			IL_0105: ldloc.s 7
			IL_0107: ldloc.s 8
			IL_0109: callvirt instance void [UnityEngine.CoreModule]UnityEngine.AnimationCurve::set_keys(valuetype [UnityEngine.CoreModule]UnityEngine.Keyframe[])
			IL_010e: ldarg.0
			IL_010f: ldloc.s 7
			IL_0111: stfld class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<old_curve>5__4'

			IL_0116: nop
			// loop start (head: IL_0117)
				IL_0117: nop
				IL_0118: ldloc.2
				IL_0119: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.SkewTextExample::m_TextComponent
				IL_011e: ldfld bool [Unity.TextMeshPro]TMPro.TMP_Text::m_havePropertiesChanged
				IL_0123: ldc.i4.0
				IL_0124: ceq
				IL_0126: ldc.i4.0
				IL_0127: ceq
				IL_0129: brtrue IL_01c7

				IL_012e: nop
				IL_012f: ldarg.0
				IL_0130: ldfld float32 TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<old_CurveScale>5__2'
				IL_0135: stloc.s 9
				IL_0137: nop
				IL_0138: ldarg.0
				IL_0139: ldfld float32 TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<old_CurveScale>5__2'
				IL_013e: ldloc.2
				IL_013f: ldfld float32 TMPro.Examples.SkewTextExample::CurveScale
				IL_0144: ceq
				IL_0146: ldc.i4.0
				IL_0147: ceq
				IL_0149: brtrue.s IL_01c7

				IL_014b: nop
				IL_014c: ldarg.0
				IL_014d: ldfld class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<old_curve>5__4'
				IL_0152: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Keyframe[] [UnityEngine.CoreModule]UnityEngine.AnimationCurve::get_keys()
				IL_0157: stloc.s 10
				IL_0159: ldloc.s 10
				IL_015b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
				IL_0160: ldc.i8 60
				IL_0169: add
				IL_016a: call instance float32 [UnityEngine.CoreModule]UnityEngine.Keyframe::get_value()
				IL_016f: stloc.s 11
				IL_0171: ldloc.s 11
				IL_0173: stloc.s 9
				IL_0175: ldloc.2
				IL_0176: ldfld class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.SkewTextExample::VertexCurve
				IL_017b: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Keyframe[] [UnityEngine.CoreModule]UnityEngine.AnimationCurve::get_keys()
				IL_0180: stloc.s 12
				IL_0182: ldloc.s 12
				IL_0184: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
				IL_0189: ldc.i8 60
				IL_0192: add
				IL_0193: call instance float32 [UnityEngine.CoreModule]UnityEngine.Keyframe::get_value()
				IL_0198: stloc.s 13
				IL_019a: ldloc.s 13
				IL_019c: stloc.s 9
				IL_019e: nop
				IL_019f: ldloc.s 11
				IL_01a1: ldloc.s 13
				IL_01a3: ceq
				IL_01a5: ldc.i4.0
				IL_01a6: ceq
				IL_01a8: brtrue.s IL_01c7

				IL_01aa: nop
				IL_01ab: ldarg.0
				IL_01ac: ldfld float32 TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<old_ShearValue>5__3'
				IL_01b1: stloc.s 9
				IL_01b3: nop
				IL_01b4: ldarg.0
				IL_01b5: ldfld float32 TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<old_ShearValue>5__3'
				IL_01ba: ldloc.2
				IL_01bb: ldfld float32 TMPro.Examples.SkewTextExample::ShearAmount
				IL_01c0: ceq
				IL_01c2: brtrue IL_0a6a

				IL_01c7: nop
				IL_01c8: ldarg.0
				IL_01c9: ldloc.2
				IL_01ca: ldfld float32 TMPro.Examples.SkewTextExample::CurveScale
				IL_01cf: stfld float32 TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<old_CurveScale>5__2'
				IL_01d4: ldloc.2
				IL_01d5: ldloc.2
				IL_01d6: ldfld class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.SkewTextExample::VertexCurve
				IL_01db: callvirt instance class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.SkewTextExample::CopyAnimationCurve(class [UnityEngine.CoreModule]UnityEngine.AnimationCurve)
				IL_01e0: stloc.s 14
				IL_01e2: ldarg.0
				IL_01e3: ldloc.s 14
				IL_01e5: stfld class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<old_curve>5__4'
				IL_01ea: ldarg.0
				IL_01eb: ldloc.2
				IL_01ec: ldfld float32 TMPro.Examples.SkewTextExample::ShearAmount
				IL_01f1: stfld float32 TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<old_ShearValue>5__3'
				IL_01f6: ldloc.2
				IL_01f7: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.SkewTextExample::m_TextComponent
				IL_01fc: stloc.s 15
				IL_01fe: ldloc.s 15
				IL_0200: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo [Unity.TextMeshPro]TMPro.TMP_Text::m_textInfo
				IL_0205: stloc.s 16
				IL_0207: ldloc.s 16
				IL_0209: ldfld int32 [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterCount
				IL_020e: stloc.s 17
				IL_0210: ldloc.s 16
				IL_0212: ldfld int32 [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterCount
				IL_0217: ldc.i4.0
				IL_0218: ceq
				IL_021a: brtrue IL_0117
			// end loop
			IL_021f: nop
			IL_0220: ldloc.2
			IL_0221: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.SkewTextExample::m_TextComponent
			IL_0226: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Bounds [Unity.TextMeshPro]TMPro.TMP_Text::get_bounds()
			IL_022b: stloc.s 18
			IL_022d: ldstr "movsd qword ptr [rbp-40h],xmm0 ; value = StructChunkExpr"
			IL_0232: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
			IL_0237: ldstr "struct_chunk(LocalExpr+0x10, 8)"
			IL_023c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0241: stloc.s 19
			IL_0243: ldstr "shufps xmm1,xmm15,0FFh"
			IL_0248: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_024d: stloc.s 20
			IL_024f: ldstr "struct_chunk(LocalExpr+0x0, 16)"
			IL_0254: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0259: conv.r4
			IL_025a: ldstr "shufps xmm1,xmm15,0FFh"
			IL_025f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0264: conv.r4
			IL_0265: sub
			IL_0266: stloc.s 21
			IL_0268: ldloc.2
			IL_0269: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.SkewTextExample::m_TextComponent
			IL_026e: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Bounds [Unity.TextMeshPro]TMPro.TMP_Text::get_bounds()
			IL_0273: stloc.s 22
			IL_0275: ldstr "movsd qword ptr [rbp-40h],xmm0 ; value = StructChunkExpr"
			IL_027a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
			IL_027f: ldstr "movaps [rbp-30h],xmm4 ; value = BinaryExpr"
			IL_0284: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
			IL_0289: ldstr "struct_chunk(LocalExpr+0x10, 8)"
			IL_028e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0293: stloc.s 19
			IL_0295: ldstr "struct_chunk(LocalExpr+0x0, 16)"
			IL_029a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_029f: stloc.s 20
			IL_02a1: ldstr "shufps xmm4,xmm1,0FFh"
			IL_02a6: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_02ab: conv.r4
			IL_02ac: ldstr "struct_chunk(LocalExpr+0x0, 16)"
			IL_02b1: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_02b6: conv.r4
			IL_02b7: add
			IL_02b8: stloc.s 23
			IL_02ba: ldloc.s 17
			IL_02bc: ldc.i4.0
			IL_02bd: cgt
			IL_02bf: ldc.i4.0
			IL_02c0: ceq
			IL_02c2: brtrue IL_0a57

			IL_02c7: nop
			// loop start (head: IL_02c8)
				IL_02c8: nop
				IL_02c9: ldloc.s 16
				IL_02cb: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_CharacterInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterInfo
				IL_02d0: stloc.s 24
				IL_02d2: ldstr "*(LocalExpr + 0x194)"
				IL_02d7: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_02dc: ldc.i4.0
				IL_02dd: conv.i8
				IL_02de: ceq
				IL_02e0: brtrue IL_0a45

				IL_02e5: nop
				IL_02e6: ldloc.s 16
				IL_02e8: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
				IL_02ed: stloc.s 25
				IL_02ef: ldstr "*(LocalExpr + 0x30)"
				IL_02f4: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_02f9: stloc.s 26
				IL_02fb: ldstr "*(FieldExpr + 0x6C)"
				IL_0300: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0305: conv.i4
				IL_0306: conv.i8
				IL_0307: stloc.s 27
				IL_0309: ldstr "movss dword ptr [rbx+rcx*4+20h],xmm0 ; value = BinaryExpr"
				IL_030e: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0313: ldstr "movss dword ptr [rbx+rcx*4+24h],xmm0 ; value = BinaryExpr"
				IL_0318: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_031d: ldstr "movss dword ptr [rbx+rcx*4+28h],xmm0 ; value = BinaryExpr"
				IL_0322: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0327: ldc.r4 -0.0
				IL_032c: ldstr "*(LocalExpr + 0x28)"
				IL_0331: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0336: conv.i4
				IL_0337: conv.r4
				IL_0338: add
				IL_0339: stloc.s 28
				IL_033b: ldstr "*(LocalExpr + 0x20)"
				IL_0340: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0345: conv.i4
				IL_0346: conv.r4
				IL_0347: ldstr "*(LocalExpr + 0x20)"
				IL_034c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0351: conv.i4
				IL_0352: conv.r4
				IL_0353: add
				IL_0354: ldc.r4 0.5
				IL_0359: mul
				IL_035a: stloc.s 29
				IL_035c: ldstr "*(FieldExpr + 0x14C)"
				IL_0361: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0366: conv.i4
				IL_0367: stloc.s 30
				IL_0369: ldstr "movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr"
				IL_036e: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0373: ldstr "movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr"
				IL_0378: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_037d: ldstr "movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr"
				IL_0382: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0387: ldc.r4 -0.0
				IL_038c: ldstr "*(LocalExpr + 0x28)"
				IL_0391: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0396: conv.i4
				IL_0397: conv.r4
				IL_0398: add
				IL_0399: stloc.s 28
				IL_039b: ldstr "movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr"
				IL_03a0: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_03a5: ldstr "movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr"
				IL_03aa: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_03af: ldstr "movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr"
				IL_03b4: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_03b9: ldc.r4 -0.0
				IL_03be: ldstr "*(LocalExpr + 0x28)"
				IL_03c3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_03c8: conv.i4
				IL_03c9: conv.r4
				IL_03ca: add
				IL_03cb: stloc.s 28
				IL_03cd: ldstr "movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr"
				IL_03d2: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_03d7: ldstr "movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr"
				IL_03dc: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_03e1: ldstr "movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr"
				IL_03e6: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_03eb: ldloc.s 16
				IL_03ed: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_CharacterInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterInfo
				IL_03f2: stloc.s 31
				IL_03f4: ldc.r4 -0.0
				IL_03f9: ldstr "*(LocalExpr + 0x28)"
				IL_03fe: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0403: conv.i4
				IL_0404: conv.r4
				IL_0405: add
				IL_0406: stloc.s 28
				IL_0408: ldloc.2
				IL_0409: ldfld float32 TMPro.Examples.SkewTextExample::ShearAmount
				IL_040e: ldc.r4 0.01
				IL_0413: mul
				IL_0414: stloc.s 28
				IL_0416: ldstr "*(LocalExpr + 0x14C)"
				IL_041b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0420: conv.i4
				IL_0421: conv.r4
				IL_0422: ldstr "*(LocalExpr + 0x138)"
				IL_0427: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_042c: conv.i4
				IL_042d: conv.r4
				IL_042e: sub
				IL_042f: ldloc.2
				IL_0430: ldfld float32 TMPro.Examples.SkewTextExample::ShearAmount
				IL_0435: ldc.r4 0.01
				IL_043a: mul
				IL_043b: mul
				IL_043c: stloc.s 32
				IL_043e: ldstr "*(LocalExpr + 0x12C)"
				IL_0443: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0448: conv.i4
				IL_0449: conv.r4
				IL_044a: ldstr "*(LocalExpr + 0x14C)"
				IL_044f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0454: conv.i4
				IL_0455: conv.r4
				IL_0456: sub
				IL_0457: ldloc.2
				IL_0458: ldfld float32 TMPro.Examples.SkewTextExample::ShearAmount
				IL_045d: ldc.r4 0.01
				IL_0462: mul
				IL_0463: mul
				IL_0464: stloc.s 33
				IL_0466: ldstr "movss dword ptr [rbx+rcx*4+20h],xmm0 ; value = BinaryExpr"
				IL_046b: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0470: ldstr "movss dword ptr [rbx+rcx*4+24h],xmm0 ; value = BinaryExpr"
				IL_0475: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_047a: ldstr "movss dword ptr [rbx+rcx*4+28h],xmm1 ; value = BinaryExpr"
				IL_047f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0484: ldc.r4 -0.0
				IL_0489: ldstr "*(LocalExpr + 0x28)"
				IL_048e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0493: conv.i4
				IL_0494: conv.r4
				IL_0495: add
				IL_0496: stloc.s 34
				IL_0498: ldc.r4 -0.0
				IL_049d: ldstr "*(LocalExpr + 0x24)"
				IL_04a2: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_04a7: conv.i4
				IL_04a8: conv.r4
				IL_04a9: add
				IL_04aa: stloc.s 28
				IL_04ac: ldstr "movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr"
				IL_04b1: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_04b6: ldstr "movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr"
				IL_04bb: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_04c0: ldstr "movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr"
				IL_04c5: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_04ca: ldc.r4 0.0
				IL_04cf: ldstr "*(LocalExpr + 0x28)"
				IL_04d4: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_04d9: conv.i4
				IL_04da: conv.r4
				IL_04db: add
				IL_04dc: stloc.s 34
				IL_04de: ldc.r4 0.0
				IL_04e3: ldstr "*(LocalExpr + 0x24)"
				IL_04e8: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_04ed: conv.i4
				IL_04ee: conv.r4
				IL_04ef: add
				IL_04f0: stloc.s 28
				IL_04f2: ldstr "movss dword ptr [rbx+rax*4+20h],xmm3 ; value = BinaryExpr"
				IL_04f7: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_04fc: ldstr "movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr"
				IL_0501: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0506: ldstr "movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr"
				IL_050b: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0510: ldc.r4 0.0
				IL_0515: ldstr "*(LocalExpr + 0x28)"
				IL_051a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_051f: conv.i4
				IL_0520: conv.r4
				IL_0521: add
				IL_0522: stloc.s 34
				IL_0524: ldc.r4 0.0
				IL_0529: ldstr "*(LocalExpr + 0x24)"
				IL_052e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0533: conv.i4
				IL_0534: conv.r4
				IL_0535: add
				IL_0536: stloc.s 28
				IL_0538: ldloc.s 33
				IL_053a: ldstr "*(LocalExpr + 0x20)"
				IL_053f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0544: conv.i4
				IL_0545: conv.r4
				IL_0546: add
				IL_0547: stloc.s 33
				IL_0549: ldstr "movss dword ptr [rbx+rax*4+20h],xmm2 ; value = BinaryExpr"
				IL_054e: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0553: ldstr "movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr"
				IL_0558: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_055d: ldstr "movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr"
				IL_0562: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0567: ldc.r4 -0.0
				IL_056c: ldstr "*(LocalExpr + 0x28)"
				IL_0571: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0576: conv.i4
				IL_0577: conv.r4
				IL_0578: add
				IL_0579: stloc.s 34
				IL_057b: ldloc.s 23
				IL_057d: ldloc.s 21
				IL_057f: sub
				IL_0580: stloc.s 28
				IL_0582: ldstr "xorps xmm2,xmm11"
				IL_0587: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_058c: conv.r4
				IL_058d: ldstr "*(LocalExpr + 0x20)"
				IL_0592: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0597: conv.i4
				IL_0598: conv.r4
				IL_0599: add
				IL_059a: stloc.s 32
				IL_059c: ldloc.2
				IL_059d: ldfld class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.SkewTextExample::VertexCurve
				IL_05a2: ldloc.s 29
				IL_05a4: ldloc.s 21
				IL_05a6: sub
				IL_05a7: ldloc.s 23
				IL_05a9: ldloc.s 21
				IL_05ab: sub
				IL_05ac: div
				IL_05ad: callvirt instance float32 [UnityEngine.CoreModule]UnityEngine.AnimationCurve::Evaluate(float32)
				IL_05b2: stloc.s 35
				IL_05b4: ldloc.s 35
				IL_05b6: stloc.s 28
				IL_05b8: ldloc.2
				IL_05b9: ldfld float32 TMPro.Examples.SkewTextExample::CurveScale
				IL_05be: stloc.s 36
				IL_05c0: ldloc.2
				IL_05c1: ldfld class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.SkewTextExample::VertexCurve
				IL_05c6: ldloc.s 29
				IL_05c8: ldloc.s 21
				IL_05ca: sub
				IL_05cb: ldloc.s 23
				IL_05cd: ldloc.s 21
				IL_05cf: sub
				IL_05d0: div
				IL_05d1: ldc.r4 0.0001
				IL_05d6: add
				IL_05d7: callvirt instance float32 [UnityEngine.CoreModule]UnityEngine.AnimationCurve::Evaluate(float32)
				IL_05dc: stloc.s 37
				IL_05de: ldloc.s 37
				IL_05e0: ldloc.2
				IL_05e1: ldfld float32 TMPro.Examples.SkewTextExample::CurveScale
				IL_05e6: mul
				IL_05e7: ldloc.s 36
				IL_05e9: ldloc.s 35
				IL_05eb: mul
				IL_05ec: sub
				IL_05ed: stloc.s 38
				IL_05ef: ldloca.s 39
				IL_05f1: ldstr "*(LocalExpr + 0xFFFFFFFFFFFFFFD0)"
				IL_05f6: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_05fb: conv.i4
				IL_05fc: conv.r4
				IL_05fd: ldloc.s 21
				IL_05ff: sub
				IL_0600: ldloc.s 29
				IL_0602: ldloc.s 21
				IL_0604: sub
				IL_0605: ldloc.s 23
				IL_0607: ldloc.s 21
				IL_0609: sub
				IL_060a: div
				IL_060b: ldc.r4 0.0001
				IL_0610: add
				IL_0611: mul
				IL_0612: ldloc.s 21
				IL_0614: add
				IL_0615: ldloc.s 29
				IL_0617: sub
				IL_0618: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_061d: ldloca.s 39
				IL_061f: ldloc.s 38
				IL_0621: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
				IL_0626: ldloca.s 39
				IL_0628: ldc.r4 0.0
				IL_062d: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_0632: ldloca.s 39
				IL_0634: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::get_normalized()
				IL_0639: stloc.s 40
				IL_063b: ldstr "sub_20C3F0"
				IL_0640: ldc.i8 0
				IL_0649: ldc.i8 0
				IL_0652: ldc.i8 0
				IL_065b: ldc.i8 0
				IL_0664: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
				IL_0669: stloc.s 41
				IL_066b: ldstr "xmm0"
				IL_0670: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0675: conv.r4
				IL_0676: ldc.r4 57.29578
				IL_067b: mul
				IL_067c: stloc.s 34
				IL_067e: ldloc.s 36
				IL_0680: ldloc.s 35
				IL_0682: mul
				IL_0683: stloc.s 42
				IL_0685: ldloc.s 38
				IL_0687: ldstr "*(LocalExpr + 0xFFFFFFFFFFFFFFD0)"
				IL_068c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0691: conv.i4
				IL_0692: conv.r4
				IL_0693: ldloc.s 21
				IL_0695: sub
				IL_0696: ldloc.s 29
				IL_0698: ldloc.s 21
				IL_069a: sub
				IL_069b: ldloc.s 23
				IL_069d: ldloc.s 21
				IL_069f: sub
				IL_06a0: div
				IL_06a1: ldc.r4 0.0001
				IL_06a6: add
				IL_06a7: mul
				IL_06a8: ldloc.s 21
				IL_06aa: add
				IL_06ab: ldloc.s 29
				IL_06ad: sub
				IL_06ae: ldc.r4 0.0
				IL_06b3: mul
				IL_06b4: sub
				IL_06b5: ldc.r4 0.0
				IL_06ba: cgt
				IL_06bc: brtrue.s IL_06d3

				IL_06be: nop
				IL_06bf: ldc.r4 360
				IL_06c4: ldloc.s 34
				IL_06c6: sub
				IL_06c7: stloc.s 34
				IL_06c9: ldc.r4 360
				IL_06ce: ldloc.s 34
				IL_06d0: sub
				IL_06d1: stloc.s 28

				IL_06d3: nop
				IL_06d4: ldloca.s 43
				IL_06d6: ldc.r4 0.0
				IL_06db: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_06e0: ldloca.s 43
				IL_06e2: ldloc.s 34
				IL_06e4: ldc.r4 0.017453292
				IL_06e9: mul
				IL_06ea: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_06ef: ldloc.s 43
				IL_06f1: call valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_FromEulerRad(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_06f6: stloc.s 44
				IL_06f8: ldstr "movdqa [rbp+20h],xmm6 ; value = StructChunkExpr"
				IL_06fd: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0702: ldloca.s 45
				IL_0704: ldc.r4 0.0
				IL_0709: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_070e: ldloca.s 45
				IL_0710: ldloc.s 42
				IL_0712: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
				IL_0717: ldloca.s 45
				IL_0719: ldc.r4 0.0
				IL_071e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_0723: ldloc.s 45
				IL_0725: ldloc.3
				IL_0726: ldc.i8 32
				IL_072f: add
				IL_0730: box [mscorlib]System.Int64
				IL_0735: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion>(object)
				IL_073a: ldloca.s 46
				IL_073c: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::oneVector
				IL_0741: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_0746: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_074b: ldloca.s 46
				IL_074d: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::oneVector
				IL_0752: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
				IL_0757: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
				IL_075c: ldloca.s 46
				IL_075e: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::oneVector
				IL_0763: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_0768: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_076d: ldloc.s 46
				IL_076f: call valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::TRS(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_0774: stloc.s 47
				IL_0776: ldstr "movaps [rbp-20h],xmm0 ; value = StructChunkExpr"
				IL_077b: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0780: ldstr "movaps [rbp-10h],xmm1 ; value = StructChunkExpr"
				IL_0785: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_078a: ldstr "movaps [rbp],xmm0 ; value = StructChunkExpr"
				IL_078f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0794: ldstr "movaps [rbp+10h],xmm1 ; value = StructChunkExpr"
				IL_0799: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_079e: ldstr "struct_chunk(LocalExpr+0x20, 16)"
				IL_07a3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_07a8: stloc.s 48
				IL_07aa: ldstr "struct_chunk(LocalExpr+0x30, 16)"
				IL_07af: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_07b4: stloc.s 49
				IL_07b6: ldloc.3
				IL_07b7: ldc.i8 -32
				IL_07c0: add
				IL_07c1: ldloca.s 50
				IL_07c3: ldstr "*(LocalExpr + 0x20)"
				IL_07c8: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_07cd: box [mscorlib]System.Int64
				IL_07d2: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
				IL_07d7: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_07dc: ldloca.s 50
				IL_07de: ldstr "*(LocalExpr + 0x28)"
				IL_07e3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_07e8: conv.i4
				IL_07e9: box [mscorlib]System.Int32
				IL_07ee: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
				IL_07f3: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_07f8: ldloc.s 50
				IL_07fa: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_07ff: stloc.s 51
				IL_0801: ldstr "movsd qword ptr [rbx+rdx*4+20h],xmm0 ; value = StructChunkExpr"
				IL_0806: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_080b: ldstr "mov [rbx+rdx*4+28h],r8d ; value = FieldExpr"
				IL_0810: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0815: ldloca.s 51
				IL_0817: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_081c: stloc.s 52
				IL_081e: ldstr "struct_chunk(LocalExpr+0x0, 8)"
				IL_0823: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0828: stloc.s 48
				IL_082a: ldstr "movsd qword ptr [rbp-80h],xmm0 ; value = *(LocalExpr + 0x20)"
				IL_082f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0834: ldstr "mov [rbp-78h],eax ; value = *(LocalExpr + 0x28)"
				IL_0839: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_083e: ldloc.3
				IL_083f: ldc.i8 -32
				IL_0848: add
				IL_0849: ldloc.3
				IL_084a: ldc.i8 -128
				IL_0853: add
				IL_0854: box [mscorlib]System.Int64
				IL_0859: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
				IL_085e: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_0863: stloc.s 53
				IL_0865: ldstr "movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr"
				IL_086a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_086f: ldstr "mov [rbx+rcx*4+28h],edx ; value = FieldExpr"
				IL_0874: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0879: ldstr "struct_chunk(LocalExpr+0x0, 8)"
				IL_087e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0883: stloc.s 48
				IL_0885: ldstr "movsd qword ptr [rbp-70h],xmm0 ; value = *(LocalExpr + 0x20)"
				IL_088a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_088f: ldstr "mov [rbp-68h],eax ; value = *(LocalExpr + 0x28)"
				IL_0894: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0899: ldloc.3
				IL_089a: ldc.i8 -32
				IL_08a3: add
				IL_08a4: ldloc.3
				IL_08a5: ldc.i8 -112
				IL_08ae: add
				IL_08af: box [mscorlib]System.Int64
				IL_08b4: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
				IL_08b9: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_08be: stloc.s 54
				IL_08c0: ldstr "movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr"
				IL_08c5: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_08ca: ldstr "mov [rbx+rcx*4+28h],edx ; value = FieldExpr"
				IL_08cf: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_08d4: ldstr "struct_chunk(LocalExpr+0x0, 8)"
				IL_08d9: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_08de: stloc.s 48
				IL_08e0: ldstr "movsd qword ptr [rbp-60h],xmm0 ; value = *(LocalExpr + 0x20)"
				IL_08e5: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_08ea: ldstr "mov [rbp-58h],eax ; value = *(LocalExpr + 0x28)"
				IL_08ef: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_08f4: ldloc.3
				IL_08f5: ldc.i8 -32
				IL_08fe: add
				IL_08ff: ldloc.3
				IL_0900: ldc.i8 -96
				IL_0909: add
				IL_090a: box [mscorlib]System.Int64
				IL_090f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
				IL_0914: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_0919: stloc.s 55
				IL_091b: ldstr "movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr"
				IL_0920: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0925: ldstr "mov [rbx+rcx*4+28h],edx ; value = FieldExpr"
				IL_092a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_092f: ldstr "struct_chunk(LocalExpr+0x0, 8)"
				IL_0934: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0939: stloc.s 48
				IL_093b: ldstr "movss dword ptr [rbx+rcx*4+20h],xmm0 ; value = BinaryExpr"
				IL_0940: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0945: ldstr "movss dword ptr [rbx+rcx*4+24h],xmm0 ; value = BinaryExpr"
				IL_094a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_094f: ldstr "movss dword ptr [rbx+rcx*4+28h],xmm1 ; value = BinaryExpr"
				IL_0954: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0959: ldloc.s 30
				IL_095b: conv.r4
				IL_095c: ldstr "*(LocalExpr + 0x24)"
				IL_0961: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0966: conv.i4
				IL_0967: conv.r4
				IL_0968: add
				IL_0969: stloc.s 56
				IL_096b: ldc.r4 0.0
				IL_0970: ldstr "*(LocalExpr + 0x28)"
				IL_0975: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_097a: conv.i4
				IL_097b: conv.r4
				IL_097c: add
				IL_097d: stloc.s 57
				IL_097f: ldstr "movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr"
				IL_0984: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0989: ldstr "movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr"
				IL_098e: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0993: ldstr "movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr"
				IL_0998: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_099d: ldloc.s 30
				IL_099f: conv.r4
				IL_09a0: ldstr "*(LocalExpr + 0x24)"
				IL_09a5: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_09aa: conv.i4
				IL_09ab: conv.r4
				IL_09ac: add
				IL_09ad: stloc.s 56
				IL_09af: ldc.r4 0.0
				IL_09b4: ldstr "*(LocalExpr + 0x28)"
				IL_09b9: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_09be: conv.i4
				IL_09bf: conv.r4
				IL_09c0: add
				IL_09c1: stloc.s 57
				IL_09c3: ldstr "movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr"
				IL_09c8: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_09cd: ldstr "movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr"
				IL_09d2: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_09d7: ldstr "movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr"
				IL_09dc: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_09e1: ldloc.s 30
				IL_09e3: conv.r4
				IL_09e4: ldstr "*(LocalExpr + 0x24)"
				IL_09e9: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_09ee: conv.i4
				IL_09ef: conv.r4
				IL_09f0: add
				IL_09f1: stloc.s 56
				IL_09f3: ldc.r4 0.0
				IL_09f8: ldstr "*(LocalExpr + 0x28)"
				IL_09fd: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0a02: conv.i4
				IL_0a03: conv.r4
				IL_0a04: add
				IL_0a05: stloc.s 57
				IL_0a07: ldstr "movss dword ptr [rbx+rax*4+20h],xmm8 ; value = BinaryExpr"
				IL_0a0c: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0a11: ldstr "movss dword ptr [rbx+rax*4+24h],xmm9 ; value = BinaryExpr"
				IL_0a16: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0a1b: ldstr "movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr"
				IL_0a20: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0a25: ldc.r4 0.0
				IL_0a2a: ldstr "*(LocalExpr + 0x28)"
				IL_0a2f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0a34: conv.i4
				IL_0a35: conv.r4
				IL_0a36: add
				IL_0a37: stloc.s 56
				IL_0a39: ldstr "*(LocalExpr + 0xFFFFFFFFFFFFFFD0)"
				IL_0a3e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0a43: stloc.s 58

				IL_0a45: nop
				IL_0a46: ldc.i4.0
				IL_0a47: ldc.i4.1
				IL_0a48: add
				IL_0a49: stloc.s 59
				IL_0a4b: ldc.i4.0
				IL_0a4c: ldc.i4.1
				IL_0a4d: add
				IL_0a4e: ldloc.s 17
				IL_0a50: clt
				IL_0a52: brtrue IL_02c8
			// end loop

			IL_0a57: nop
			IL_0a58: ldarg.0
			IL_0a59: ldnull
			IL_0a5a: stfld object TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<>2__current'
			IL_0a5f: ldarg.0
			IL_0a60: ldc.i4.2
			IL_0a61: stfld int32 TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<>1__state'

			IL_0a66: nop

			IL_0a67: nop
			IL_0a68: ldc.i4.1
			IL_0a69: ret

			IL_0a6a: nop
			IL_0a6b: ldarg.0
			IL_0a6c: ldnull
			IL_0a6d: stfld object TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<>2__current'
			IL_0a72: ldarg.0
			IL_0a73: ldc.i4.1
			IL_0a74: stfld int32 TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<>1__state'
			IL_0a79: br.s IL_0a66

			IL_0a7b: nop
			IL_0a7c: ldnull
			IL_0a7d: throw

			IL_0a7e: nop
			IL_0a7f: ldnull
			IL_0a80: throw
		} // end of method '<WarpText>d__7'::MoveNext

		.method private final hidebysig specialname newslot virtual 
			instance object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () cil managed 
		{
			// Method begins at RVA 0x2de8
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<>2__current'
			IL_0007: ret
		} // end of method '<WarpText>d__7'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'

		.method private final hidebysig newslot virtual 
			instance void System.Collections.IEnumerator.Reset () cil managed 
		{
			// Method begins at RVA 0x1c18c
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
		} // end of method '<WarpText>d__7'::System.Collections.IEnumerator.Reset

		.method private final hidebysig specialname newslot virtual 
			instance object System.Collections.IEnumerator.get_Current () cil managed 
		{
			// Method begins at RVA 0x2df1
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<>2__current'
			IL_0007: ret
		} // end of method '<WarpText>d__7'::System.Collections.IEnumerator.get_Current

		// Properties
		.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current'()
		{
			.get instance object TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'()
		}
		.property instance object System.Collections.IEnumerator.Current()
		{
			.get instance object TMPro.Examples.SkewTextExample/'<WarpText>d__7'::System.Collections.IEnumerator.get_Current()
		}

	} // end of class <WarpText>d__7


	// Fields
	.field private class [Unity.TextMeshPro]TMPro.TMP_Text m_TextComponent
	.field public class [UnityEngine.CoreModule]UnityEngine.AnimationCurve VertexCurve
	.field public float32 CurveScale
	.field public float32 ShearAmount

	// Methods
	.method private hidebysig 
		instance void Awake () cil managed 
	{
		// Method begins at RVA 0x16448
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
		IL_0011: stfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.SkewTextExample::m_TextComponent
		IL_0016: ret

		IL_0017: nop
		IL_0018: ldnull
		IL_0019: throw
	} // end of method SkewTextExample::Awake

	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x16470
		// Header size: 12
		// Code size: 47 (0x2f)
		.maxstack 2
		.locals init (
			[0] class TMPro.Examples.SkewTextExample/'<WarpText>d__7',
			[1] class [UnityEngine.CoreModule]UnityEngine.Coroutine
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(TMPro.Examples.SkewTextExample+<WarpText>d__7)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.SkewTextExample/'<WarpText>d__7'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class TMPro.Examples.SkewTextExample TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<>4__this'
		IL_0026: ldarg.0
		IL_0027: ldloc.0
		IL_0028: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Coroutine [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StartCoroutine(class [mscorlib]System.Collections.IEnumerator)
		IL_002d: stloc.1
		IL_002e: ret
	} // end of method SkewTextExample::Start

	.method private hidebysig 
		instance class [UnityEngine.CoreModule]UnityEngine.AnimationCurve CopyAnimationCurve (
			class [UnityEngine.CoreModule]UnityEngine.AnimationCurve curve
		) cil managed 
	{
		// Method begins at RVA 0x164ac
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
	} // end of method SkewTextExample::CopyAnimationCurve

	.method private hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator WarpText () cil managed 
	{
		// Method begins at RVA 0x164d4
		// Header size: 12
		// Code size: 40 (0x28)
		.maxstack 2
		.locals init (
			[0] class TMPro.Examples.SkewTextExample/'<WarpText>d__7'
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(TMPro.Examples.SkewTextExample+<WarpText>d__7)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.SkewTextExample/'<WarpText>d__7'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class TMPro.Examples.SkewTextExample TMPro.Examples.SkewTextExample/'<WarpText>d__7'::'<>4__this'
		IL_0026: ldloc.0
		IL_0027: ret
	} // end of method SkewTextExample::WarpText

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x16508
		// Header size: 12
		// Code size: 397 (0x18d)
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
		IL_0165: stfld class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.SkewTextExample::VertexCurve
		IL_016a: ldarg.0
		IL_016b: ldc.r4 1
		IL_0170: stfld float32 TMPro.Examples.SkewTextExample::CurveScale
		IL_0175: ldarg.0
		IL_0176: ldc.r4 1
		IL_017b: stfld float32 TMPro.Examples.SkewTextExample::ShearAmount
		IL_0180: ldarg.0
		IL_0181: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0186: ret

		IL_0187: nop
		IL_0188: ldnull
		IL_0189: throw

		IL_018a: nop
		IL_018b: ldnull
		IL_018c: throw
	} // end of method SkewTextExample::.ctor

} // end of class TMPro.Examples.SkewTextExample
