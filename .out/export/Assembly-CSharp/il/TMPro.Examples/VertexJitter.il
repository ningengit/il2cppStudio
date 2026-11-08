.class public auto ansi beforefieldinit TMPro.Examples.VertexJitter
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested private sequential ansi sealed beforefieldinit VertexAnim
		extends [mscorlib]System.ValueType
	{
		// Fields
		.field public float32 angleRange
		.field public float32 angle
		.field public float32 speed

	} // end of class VertexAnim

	.class nested private auto ansi sealed beforefieldinit '<AnimateVertexColors>d__11'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerator`1<object>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private object '<>2__current'
		.field public class TMPro.Examples.VertexJitter '<>4__this'
		.field private class [Unity.TextMeshPro]TMPro.TMP_TextInfo '<textInfo>5__2'
		.field private int32 '<loopCount>5__3'
		.field private valuetype TMPro.Examples.VertexJitter/VertexAnim[] '<vertexAnim>5__4'
		.field private valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] '<cachedMeshInfo>5__5'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x2ede
			// Header size: 1
			// Code size: 16 (0x10)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ldarg.0
			IL_0009: ldarg.1
			IL_000a: stfld int32 TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<>1__state'
			IL_000f: ret
		} // end of method '<AnimateVertexColors>d__11'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			// Method begins at RVA 0x2eef
			// Header size: 1
			// Code size: 2 (0x2)
			.maxstack 8

			IL_0000: nop
			IL_0001: ret
		} // end of method '<AnimateVertexColors>d__11'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			// Method begins at RVA 0x1d4e8
			// Header size: 12
			// Code size: 2801 (0xaf1)
			.maxstack 5
			.locals init (
				[0] int32,
				[1] class TMPro.Examples.VertexJitter,
				[2] class TMPro.Examples.VertexJitter,
				[3] int64,
				[4] int32,
				[5] class [Unity.TextMeshPro]TMPro.TMP_Text,
				[6] class [Unity.TextMeshPro]TMPro.TMP_Text,
				[7] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[8] valuetype TMPro.Examples.VertexJitter/VertexAnim[],
				[9] valuetype TMPro.Examples.VertexJitter/VertexAnim[],
				[10] float32,
				[11] float32,
				[12] int64,
				[13] valuetype TMPro.Examples.VertexJitter/VertexAnim[],
				[14] float32,
				[15] int32,
				[16] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[],
				[17] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[],
				[18] int32,
				[19] int64,
				[20] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[21] valuetype [Unity.TextMeshPro]TMPro.TMP_CharacterInfo[],
				[22] int64,
				[23] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[24] int64,
				[25] int64,
				[26] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[],
				[27] float32,
				[28] float32,
				[29] float32,
				[30] int64,
				[31] int32,
				[32] int32,
				[33] float32,
				[34] int64,
				[35] float32,
				[36] float32,
				[37] float32,
				[38] float32,
				[39] float32,
				[40] float32,
				[41] float32,
				[42] float32,
				[43] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[44] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
				[45] float32,
				[46] float32,
				[47] int64,
				[48] float32,
				[49] float32,
				[50] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[51] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[52] valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4,
				[53] int64,
				[54] int64,
				[55] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[56] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[57] float32,
				[58] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[59] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[60] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[61] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[62] float32,
				[63] float32,
				[64] valuetype TMPro.Examples.VertexJitter/VertexAnim[],
				[65] int32,
				[66] int32,
				[67] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[68] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[69] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[],
				[70] int64,
				[71] class [Unity.TextMeshPro]TMPro.TMP_Text,
				[72] int32,
				[73] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[74] int32,
				[75] class [UnityEngine.CoreModule]UnityEngine.WaitForSeconds,
				[76] class [UnityEngine.CoreModule]UnityEngine.WaitForSeconds
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld int32 TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<>1__state'
			IL_0007: stloc.0
			IL_0008: ldarg.0
			IL_0009: ldfld class TMPro.Examples.VertexJitter TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<>4__this'
			IL_000e: stloc.1
			IL_000f: ldstr "movups [rbp-60h],xmm0 ; value = FloatExpr"
			IL_0014: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
			IL_0019: ldstr "movups [rbp-50h],xmm0 ; value = FloatExpr"
			IL_001e: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
			IL_0023: ldstr "movups [rbp-40h],xmm0 ; value = FloatExpr"
			IL_0028: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
			IL_002d: ldstr "movups [rbp-30h],xmm0 ; value = FloatExpr"
			IL_0032: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
			IL_0037: ldloc.1
			IL_0038: stloc.2
			IL_0039: ldstr "&stack[0x0]"
			IL_003e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0043: conv.i8
			IL_0044: ldc.i8 -728
			IL_004d: add
			IL_004e: stloc.3
			IL_004f: ldloc.0
			IL_0050: stloc.s 4
			IL_0052: ldloc.0
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
			IL_0071: br IL_0aca

			IL_0076: nop
			IL_0077: ldarg.0
			IL_0078: ldc.i4.m1
			IL_0079: stfld int32 TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<>1__state'
			IL_007e: br IL_0169

			IL_0083: nop
			IL_0084: ldarg.0
			IL_0085: ldc.i4.m1
			IL_0086: stfld int32 TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<>1__state'
			IL_008b: ldloc.2
			IL_008c: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.VertexJitter::m_TextComponent
			IL_0091: stloc.s 5
			IL_0093: ldloc.2
			IL_0094: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.VertexJitter::m_TextComponent
			IL_0099: stloc.s 6
			IL_009b: ldloc.s 6
			IL_009d: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo [Unity.TextMeshPro]TMPro.TMP_Text::m_textInfo
			IL_00a2: stloc.s 7
			IL_00a4: ldarg.0
			IL_00a5: ldloc.s 7
			IL_00a7: stfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
			IL_00ac: ldarg.0
			IL_00ad: ldc.i4.0
			IL_00ae: stfld int32 TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<loopCount>5__3'
			IL_00b3: ldloc.2
			IL_00b4: ldc.i4.1
			IL_00b5: stfld bool TMPro.Examples.VertexJitter::hasTextChanged
			IL_00ba: ldc.i4 1024
			IL_00bf: newarr TMPro.Examples.VertexJitter/VertexAnim
			IL_00c4: stloc.s 8
			IL_00c6: ldarg.0
			IL_00c7: ldloc.s 8
			IL_00c9: stfld valuetype TMPro.Examples.VertexJitter/VertexAnim[] TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<vertexAnim>5__4'
			// loop start (head: IL_00ce)
				IL_00ce: nop
				IL_00cf: ldarg.0
				IL_00d0: ldfld valuetype TMPro.Examples.VertexJitter/VertexAnim[] TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<vertexAnim>5__4'
				IL_00d5: stloc.s 9
				IL_00d7: ldc.r4 10
				IL_00dc: ldc.r4 25
				IL_00e1: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
				IL_00e6: stloc.s 10
				IL_00e8: ldloc.s 10
				IL_00ea: stloc.s 11
				IL_00ec: ldstr "movss dword ptr [rdi+rcx*4+20h],xmm0 ; value = LocalExpr"
				IL_00f1: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_00f6: ldc.i4.0
				IL_00f7: conv.i8
				IL_00f8: ldc.i4.0
				IL_00f9: conv.i8
				IL_00fa: ldc.i8 2
				IL_0103: mul
				IL_0104: add
				IL_0105: stloc.s 12
				IL_0107: ldarg.0
				IL_0108: ldfld valuetype TMPro.Examples.VertexJitter/VertexAnim[] TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<vertexAnim>5__4'
				IL_010d: stloc.s 13
				IL_010f: ldc.r4 1
				IL_0114: ldc.r4 3
				IL_0119: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
				IL_011e: stloc.s 14
				IL_0120: ldloc.s 14
				IL_0122: stloc.s 11
				IL_0124: ldstr "movss dword ptr [rdi+rcx*4+28h],xmm0 ; value = LocalExpr"
				IL_0129: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_012e: ldc.i4.0
				IL_012f: conv.i8
				IL_0130: ldc.i4.0
				IL_0131: conv.i8
				IL_0132: ldc.i8 2
				IL_013b: mul
				IL_013c: add
				IL_013d: stloc.s 12
				IL_013f: ldc.i4.0
				IL_0140: ldc.i4.1
				IL_0141: add
				IL_0142: stloc.s 15
				IL_0144: ldc.i4.0
				IL_0145: ldc.i4.1
				IL_0146: add
				IL_0147: ldc.i4 1024
				IL_014c: clt
				IL_014e: brtrue IL_00ce
			// end loop
			IL_0153: nop
			IL_0154: ldarg.0
			IL_0155: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
			IL_015a: callvirt instance valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::CopyMeshInfoVertexData()
			IL_015f: stloc.s 16
			IL_0161: ldarg.0
			IL_0162: ldloc.s 16
			IL_0164: stfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<cachedMeshInfo>5__5'

			IL_0169: nop
			IL_016a: ldloc.2
			IL_016b: ldfld bool TMPro.Examples.VertexJitter::hasTextChanged
			IL_0170: ldc.i4.0
			IL_0171: ceq
			IL_0173: brtrue.s IL_0192

			IL_0175: nop
			IL_0176: ldarg.0
			IL_0177: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
			IL_017c: callvirt instance valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::CopyMeshInfoVertexData()
			IL_0181: stloc.s 17
			IL_0183: ldarg.0
			IL_0184: ldloc.s 17
			IL_0186: stfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<cachedMeshInfo>5__5'
			IL_018b: ldloc.2
			IL_018c: ldc.i4.0
			IL_018d: stfld bool TMPro.Examples.VertexJitter::hasTextChanged

			IL_0192: nop
			IL_0193: ldarg.0
			IL_0194: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
			IL_0199: ldfld int32 [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterCount
			IL_019e: stloc.s 18
			IL_01a0: ldarg.0
			IL_01a1: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
			IL_01a6: ldfld int32 [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterCount
			IL_01ab: ldc.i4.0
			IL_01ac: ceq
			IL_01ae: brtrue IL_0acd

			IL_01b3: nop
			IL_01b4: ldarg.0
			IL_01b5: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
			IL_01ba: ldfld int32 [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterCount
			IL_01bf: ldc.i4.0
			IL_01c0: cgt
			IL_01c2: ldc.i4.0
			IL_01c3: ceq
			IL_01c5: brtrue IL_0a11

			IL_01ca: nop
			// loop start (head: IL_01cb)
				IL_01cb: nop
				IL_01cc: ldstr "sub_32D0"
				IL_01d1: ldarg.0
				IL_01d2: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
				IL_01d7: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_CharacterInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterInfo
				IL_01dc: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
				IL_01e1: ldloc.3
				IL_01e2: ldc.i8 128
				IL_01eb: add
				IL_01ec: ldc.i4.0
				IL_01ed: conv.i8
				IL_01ee: ldc.i8 0
				IL_01f7: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
				IL_01fc: stloc.s 19
				IL_01fe: ldstr "*(LocalExpr + 0x1F4)"
				IL_0203: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0208: ldc.i4.0
				IL_0209: conv.i8
				IL_020a: ceq
				IL_020c: brtrue IL_09ff

				IL_0211: nop
				IL_0212: ldarg.0
				IL_0213: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
				IL_0218: stloc.s 20
				IL_021a: ldloc.s 20
				IL_021c: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_CharacterInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterInfo
				IL_0221: stloc.s 21
				IL_0223: ldc.i4.0
				IL_0224: conv.i8
				IL_0225: ldc.i8 376
				IL_022e: mul
				IL_022f: stloc.s 12
				IL_0231: ldstr "*(LocalExpr + 0x58)"
				IL_0236: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_023b: conv.i4
				IL_023c: conv.i8
				IL_023d: stloc.s 22
				IL_023f: ldloc.s 22
				IL_0241: ldloc.s 22
				IL_0243: ldc.i8 4
				IL_024c: mul
				IL_024d: add
				IL_024e: ldloc.s 22
				IL_0250: ldloc.s 22
				IL_0252: ldc.i8 4
				IL_025b: mul
				IL_025c: add
				IL_025d: add
				IL_025e: stloc.s 12
				IL_0260: ldc.i4.0
				IL_0261: conv.i8
				IL_0262: ldc.i8 376
				IL_026b: mul
				IL_026c: stloc.s 12
				IL_026e: ldarg.0
				IL_026f: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
				IL_0274: stloc.s 23
				IL_0276: ldstr "*(BinaryExpr + 0x6C)"
				IL_027b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0280: conv.i4
				IL_0281: conv.i8
				IL_0282: stloc.s 24
				IL_0284: ldloc.s 24
				IL_0286: ldc.i8 2
				IL_028f: add
				IL_0290: ldloc.s 24
				IL_0292: ldc.i8 2
				IL_029b: add
				IL_029c: ldc.i8 2
				IL_02a5: mul
				IL_02a6: add
				IL_02a7: stloc.s 12
				IL_02a9: ldloc.s 24
				IL_02ab: ldloc.s 24
				IL_02ad: ldc.i8 2
				IL_02b6: mul
				IL_02b7: add
				IL_02b8: stloc.s 25
				IL_02ba: ldarg.0
				IL_02bb: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
				IL_02c0: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
				IL_02c5: stloc.s 26
				IL_02c7: ldstr "*(*(FieldExpr + 0x30) + 0x20)"
				IL_02cc: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_02d1: conv.i4
				IL_02d2: conv.r4
				IL_02d3: ldstr "*(*(FieldExpr + 0x30) + 0x20)"
				IL_02d8: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_02dd: conv.i4
				IL_02de: conv.r4
				IL_02df: add
				IL_02e0: ldc.r4 0.5
				IL_02e5: mul
				IL_02e6: stloc.s 27
				IL_02e8: ldstr "*(*(FieldExpr + 0x30) + 0x24)"
				IL_02ed: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_02f2: conv.i4
				IL_02f3: conv.r4
				IL_02f4: ldstr "*(*(FieldExpr + 0x30) + 0x24)"
				IL_02f9: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_02fe: conv.i4
				IL_02ff: conv.r4
				IL_0300: add
				IL_0301: ldc.r4 0.5
				IL_0306: mul
				IL_0307: stloc.s 28
				IL_0309: ldloc.s 24
				IL_030b: ldloc.s 24
				IL_030d: ldc.i8 2
				IL_0316: mul
				IL_0317: add
				IL_0318: stloc.s 12
				IL_031a: ldstr "*(*(FieldExpr + 0x30) + 0x28)"
				IL_031f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0324: conv.i4
				IL_0325: conv.r4
				IL_0326: ldc.r4 0.0
				IL_032b: sub
				IL_032c: stloc.s 29
				IL_032e: ldstr "*(LocalExpr + 0x30)"
				IL_0333: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0338: stloc.s 30
				IL_033a: ldstr "movss dword ptr [rbx+rcx*4+20h],xmm0 ; value = BinaryExpr"
				IL_033f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0344: ldstr "movss dword ptr [rbx+rcx*4+24h],xmm1 ; value = BinaryExpr"
				IL_0349: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_034e: ldstr "movss dword ptr [rbx+rcx*4+28h],xmm2 ; value = LocalExpr"
				IL_0353: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0358: ldstr "movss dword ptr [rbp+2E0h],xmm12 ; value = *(FieldExpr + 0x20)"
				IL_035d: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0362: ldstr "movss dword ptr [rbp+2F0h],xmm3 ; value = *(FieldExpr + 0x28)"
				IL_0367: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_036c: ldstr "*(FieldExpr + 0x20)"
				IL_0371: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0376: conv.i4
				IL_0377: stloc.s 31
				IL_0379: ldloc.s 24
				IL_037b: ldloc.s 24
				IL_037d: ldc.i8 2
				IL_0386: mul
				IL_0387: add
				IL_0388: stloc.s 12
				IL_038a: ldstr "*(FieldExpr + 0x28)"
				IL_038f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0394: conv.i4
				IL_0395: stloc.s 32
				IL_0397: ldloc.s 24
				IL_0399: ldc.i8 1
				IL_03a2: add
				IL_03a3: ldloc.s 24
				IL_03a5: ldc.i8 1
				IL_03ae: add
				IL_03af: ldc.i8 2
				IL_03b8: mul
				IL_03b9: add
				IL_03ba: stloc.s 12
				IL_03bc: ldstr "*(*(FieldExpr + 0x30) + 0x28)"
				IL_03c1: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_03c6: conv.i4
				IL_03c7: conv.r4
				IL_03c8: ldc.r4 0.0
				IL_03cd: sub
				IL_03ce: stloc.s 29
				IL_03d0: ldstr "movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr"
				IL_03d5: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_03da: ldstr "movss dword ptr [rbx+rax*4+24h],xmm1 ; value = BinaryExpr"
				IL_03df: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_03e4: ldstr "movss dword ptr [rbx+rax*4+28h],xmm2 ; value = LocalExpr"
				IL_03e9: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_03ee: ldloc.s 24
				IL_03f0: ldc.i8 2
				IL_03f9: add
				IL_03fa: ldloc.s 24
				IL_03fc: ldc.i8 2
				IL_0405: add
				IL_0406: ldc.i8 2
				IL_040f: mul
				IL_0410: add
				IL_0411: stloc.s 12
				IL_0413: ldstr "*(*(FieldExpr + 0x30) + 0x28)"
				IL_0418: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_041d: conv.i4
				IL_041e: conv.r4
				IL_041f: ldc.r4 0.0
				IL_0424: sub
				IL_0425: stloc.s 29
				IL_0427: ldstr "movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr"
				IL_042c: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0431: ldstr "movss dword ptr [rbx+rax*4+24h],xmm1 ; value = BinaryExpr"
				IL_0436: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_043b: ldstr "movss dword ptr [rbx+rax*4+28h],xmm2 ; value = LocalExpr"
				IL_0440: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0445: ldloc.s 24
				IL_0447: ldc.i8 3
				IL_0450: add
				IL_0451: ldloc.s 24
				IL_0453: ldc.i8 3
				IL_045c: add
				IL_045d: ldc.i8 2
				IL_0466: mul
				IL_0467: add
				IL_0468: stloc.s 12
				IL_046a: ldstr "*(*(FieldExpr + 0x30) + 0x28)"
				IL_046f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0474: conv.i4
				IL_0475: conv.r4
				IL_0476: ldc.r4 0.0
				IL_047b: sub
				IL_047c: stloc.s 29
				IL_047e: ldstr "movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr"
				IL_0483: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0488: ldstr "movss dword ptr [rbx+rax*4+24h],xmm1 ; value = BinaryExpr"
				IL_048d: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0492: ldstr "movss dword ptr [rbx+rax*4+28h],xmm2 ; value = LocalExpr"
				IL_0497: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_049c: ldarg.0
				IL_049d: ldfld int32 TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<loopCount>5__3'
				IL_04a2: conv.r4
				IL_04a3: ldc.r4 25
				IL_04a8: div
				IL_04a9: ldloc.s 32
				IL_04ab: conv.r4
				IL_04ac: mul
				IL_04ad: stloc.s 33
				IL_04af: ldstr "sub_20C2B0"
				IL_04b4: ldloc.s 12
				IL_04b6: ldloc.s 25
				IL_04b8: ldstr "*(FieldExpr + 0x30)"
				IL_04bd: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_04c2: ldloc.s 22
				IL_04c4: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
				IL_04c9: stloc.s 34
				IL_04cb: ldloc.s 33
				IL_04cd: ldstr "xmm0"
				IL_04d2: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_04d7: conv.r4
				IL_04d8: ldstr "xmm0"
				IL_04dd: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_04e2: conv.r4
				IL_04e3: add
				IL_04e4: sub
				IL_04e5: stloc.s 35
				IL_04e7: ldc.r4 0.0
				IL_04ec: ldloc.s 33
				IL_04ee: ldstr "xmm0"
				IL_04f3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_04f8: conv.r4
				IL_04f9: ldstr "xmm0"
				IL_04fe: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0503: conv.r4
				IL_0504: add
				IL_0505: sub
				IL_0506: cgt
				IL_0508: brtrue.s IL_051c

				IL_050a: nop
				IL_050b: ldloc.s 35
				IL_050d: ldc.r4 2
				IL_0512: cgt
				IL_0514: ldc.i4.0
				IL_0515: ceq
				IL_0517: brtrue.s IL_051d

				IL_0519: nop
				IL_051a: br.s IL_051d

				IL_051c: nop

				IL_051d: nop
				IL_051e: ldc.r4 1
				IL_0523: ldstr "andps xmm6,[1813EAC20h]"
				IL_0528: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_052d: conv.r4
				IL_052e: sub
				IL_052f: stloc.s 36
				IL_0531: ldc.r4 0.0
				IL_0536: ldc.r4 1
				IL_053b: ldstr "andps xmm6,[1813EAC20h]"
				IL_0540: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0545: conv.r4
				IL_0546: sub
				IL_0547: cgt
				IL_0549: brtrue.s IL_055d

				IL_054b: nop
				IL_054c: ldloc.s 36
				IL_054e: ldc.r4 1
				IL_0553: cgt
				IL_0555: ldc.i4.0
				IL_0556: ceq
				IL_0558: brtrue.s IL_055e

				IL_055a: nop
				IL_055b: br.s IL_055e

				IL_055d: nop

				IL_055e: nop
				IL_055f: ldc.r4 -0.25
				IL_0564: ldc.r4 0.25
				IL_0569: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
				IL_056e: stloc.s 37
				IL_0570: ldc.r4 -0.25
				IL_0575: ldc.r4 0.25
				IL_057a: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
				IL_057f: stloc.s 38
				IL_0581: ldloc.2
				IL_0582: ldfld float32 TMPro.Examples.VertexJitter::CurveScale
				IL_0587: ldc.r4 0.0
				IL_058c: mul
				IL_058d: stloc.s 39
				IL_058f: ldloc.2
				IL_0590: ldfld float32 TMPro.Examples.VertexJitter::CurveScale
				IL_0595: ldloc.s 38
				IL_0597: mul
				IL_0598: stloc.s 40
				IL_059a: ldloc.2
				IL_059b: ldfld float32 TMPro.Examples.VertexJitter::CurveScale
				IL_05a0: ldloc.s 37
				IL_05a2: mul
				IL_05a3: stloc.s 41
				IL_05a5: ldc.r4 -5
				IL_05aa: ldc.r4 5
				IL_05af: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
				IL_05b4: stloc.s 42
				IL_05b6: ldloca.s 43
				IL_05b8: ldc.r4 0.0
				IL_05bd: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_05c2: ldloca.s 43
				IL_05c4: ldloc.s 42
				IL_05c6: ldloc.2
				IL_05c7: ldfld float32 TMPro.Examples.VertexJitter::AngleMultiplier
				IL_05cc: mul
				IL_05cd: ldc.r4 0.017453292
				IL_05d2: mul
				IL_05d3: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_05d8: ldloc.s 43
				IL_05da: call valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_FromEulerRad(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_05df: stloc.s 44
				IL_05e1: ldloc.s 41
				IL_05e3: stloc.s 45
				IL_05e5: ldloc.s 40
				IL_05e7: stloc.s 35
				IL_05e9: ldloc.s 39
				IL_05eb: stloc.s 46
				IL_05ed: ldstr "struct_chunk(LocalExpr+0x0, 16)"
				IL_05f2: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_05f7: stloc.s 47
				IL_05f9: ldc.r4 1
				IL_05fe: ldc.r4 0.0
				IL_0603: ldc.r4 -2
				IL_0608: mul
				IL_0609: ldc.r4 0.0
				IL_060e: mul
				IL_060f: ldc.r4 0.0
				IL_0614: mul
				IL_0615: ldc.r4 0.0
				IL_061a: ldc.r4 3
				IL_061f: mul
				IL_0620: ldc.r4 0.0
				IL_0625: mul
				IL_0626: add
				IL_0627: sub
				IL_0628: ldstr "xorps xmm0,[1813EACB0h]"
				IL_062d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0632: conv.r4
				IL_0633: mul
				IL_0634: stloc.s 48
				IL_0636: ldc.r4 0.0
				IL_063b: ldc.r4 -2
				IL_0640: mul
				IL_0641: ldc.r4 0.0
				IL_0646: mul
				IL_0647: ldc.r4 0.0
				IL_064c: mul
				IL_064d: ldc.r4 0.0
				IL_0652: ldc.r4 3
				IL_0657: mul
				IL_0658: ldc.r4 0.0
				IL_065d: mul
				IL_065e: add
				IL_065f: ldloc.s 31
				IL_0661: conv.r4
				IL_0662: mul
				IL_0663: stloc.s 49
				IL_0665: ldstr "global_19E3BFF"
				IL_066a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_066f: ldc.i4.0
				IL_0670: conv.i8
				IL_0671: ceq
				IL_0673: ldc.i4.0
				IL_0674: ceq
				IL_0676: brtrue.s IL_0683

				IL_0678: nop
				IL_0679: ldstr "mov byte ptr [1819E3BFFh],1 ; value = 0x1"
				IL_067e: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)

				IL_0683: nop
				IL_0684: ldstr "movdqa [rbp-20h],xmm14 ; value = LocalExpr"
				IL_0689: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_068e: ldloca.s 50
				IL_0690: ldloc.s 45
				IL_0692: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_0697: ldloca.s 50
				IL_0699: ldloc.s 35
				IL_069b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
				IL_06a0: ldloca.s 50
				IL_06a2: ldloc.s 46
				IL_06a4: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_06a9: ldloc.s 50
				IL_06ab: ldloc.3
				IL_06ac: ldc.i8 -32
				IL_06b5: add
				IL_06b6: box [mscorlib]System.Int64
				IL_06bb: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion>(object)
				IL_06c0: ldloca.s 51
				IL_06c2: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::oneVector
				IL_06c7: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_06cc: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_06d1: ldloca.s 51
				IL_06d3: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::oneVector
				IL_06d8: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
				IL_06dd: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
				IL_06e2: ldloca.s 51
				IL_06e4: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::oneVector
				IL_06e9: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_06ee: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_06f3: ldloc.s 51
				IL_06f5: call valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::TRS(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_06fa: stloc.s 52
				IL_06fc: ldstr "movaps [rbp-60h],xmm0 ; value = StructChunkExpr"
				IL_0701: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0706: ldstr "movaps [rbp-50h],xmm1 ; value = StructChunkExpr"
				IL_070b: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0710: ldstr "movaps [rbp-40h],xmm0 ; value = StructChunkExpr"
				IL_0715: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_071a: ldstr "movaps [rbp-30h],xmm1 ; value = StructChunkExpr"
				IL_071f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0724: ldstr "struct_chunk(LocalExpr+0x20, 16)"
				IL_0729: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_072e: stloc.s 53
				IL_0730: ldstr "struct_chunk(LocalExpr+0x30, 16)"
				IL_0735: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_073a: stloc.s 54
				IL_073c: ldloc.3
				IL_073d: ldc.i8 -96
				IL_0746: add
				IL_0747: ldloca.s 55
				IL_0749: ldstr "*(LocalExpr + 0x20)"
				IL_074e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0753: box [mscorlib]System.Int64
				IL_0758: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
				IL_075d: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_0762: ldloca.s 55
				IL_0764: ldstr "*(LocalExpr + 0x28)"
				IL_0769: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_076e: conv.i4
				IL_076f: box [mscorlib]System.Int32
				IL_0774: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
				IL_0779: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_077e: ldloc.s 55
				IL_0780: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_0785: stloc.s 56
				IL_0787: ldstr "movsd qword ptr [rbx+rdx*4+20h],xmm0 ; value = StructChunkExpr"
				IL_078c: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0791: ldstr "mov [rbx+rdx*4+28h],r8d ; value = FieldExpr"
				IL_0796: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_079b: ldloca.s 56
				IL_079d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_07a2: stloc.s 57
				IL_07a4: ldstr "struct_chunk(LocalExpr+0x0, 8)"
				IL_07a9: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_07ae: stloc.s 53
				IL_07b0: ldloc.3
				IL_07b1: ldc.i8 -96
				IL_07ba: add
				IL_07bb: ldloca.s 58
				IL_07bd: ldstr "*(LocalExpr + 0x20)"
				IL_07c2: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_07c7: box [mscorlib]System.Int64
				IL_07cc: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
				IL_07d1: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_07d6: ldloca.s 58
				IL_07d8: ldstr "*(LocalExpr + 0x28)"
				IL_07dd: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_07e2: conv.i4
				IL_07e3: box [mscorlib]System.Int32
				IL_07e8: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
				IL_07ed: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_07f2: ldloc.s 58
				IL_07f4: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_07f9: stloc.s 59
				IL_07fb: ldstr "movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr"
				IL_0800: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0805: ldstr "mov [rbx+rcx*4+28h],edx ; value = FieldExpr"
				IL_080a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_080f: ldstr "struct_chunk(LocalExpr+0x0, 8)"
				IL_0814: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0819: stloc.s 53
				IL_081b: ldstr "movsd qword ptr [rbp-80h],xmm0 ; value = *(LocalExpr + 0x20)"
				IL_0820: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0825: ldstr "mov [rbp-78h],eax ; value = *(LocalExpr + 0x28)"
				IL_082a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_082f: ldloc.3
				IL_0830: ldc.i8 -96
				IL_0839: add
				IL_083a: ldloc.3
				IL_083b: ldc.i8 -128
				IL_0844: add
				IL_0845: box [mscorlib]System.Int64
				IL_084a: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
				IL_084f: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_0854: stloc.s 60
				IL_0856: ldstr "movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr"
				IL_085b: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0860: ldstr "mov [rbx+rcx*4+28h],edx ; value = FieldExpr"
				IL_0865: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_086a: ldstr "struct_chunk(LocalExpr+0x0, 8)"
				IL_086f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0874: stloc.s 53
				IL_0876: ldstr "movsd qword ptr [rbp-70h],xmm0 ; value = *(LocalExpr + 0x20)"
				IL_087b: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0880: ldstr "mov [rbp-68h],eax ; value = *(LocalExpr + 0x28)"
				IL_0885: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_088a: ldloc.3
				IL_088b: ldc.i8 -96
				IL_0894: add
				IL_0895: ldloc.3
				IL_0896: ldc.i8 -112
				IL_089f: add
				IL_08a0: box [mscorlib]System.Int64
				IL_08a5: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
				IL_08aa: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_08af: stloc.s 61
				IL_08b1: ldstr "movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr"
				IL_08b6: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_08bb: ldstr "mov [rbx+rcx*4+28h],edx ; value = FieldExpr"
				IL_08c0: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_08c5: ldstr "struct_chunk(LocalExpr+0x0, 8)"
				IL_08ca: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_08cf: stloc.s 53
				IL_08d1: ldstr "movss dword ptr [rbx+rcx*4+20h],xmm0 ; value = BinaryExpr"
				IL_08d6: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_08db: ldstr "movss dword ptr [rbx+rcx*4+24h],xmm0 ; value = BinaryExpr"
				IL_08e0: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_08e5: ldstr "movss dword ptr [rbx+rcx*4+28h],xmm1 ; value = BinaryExpr"
				IL_08ea: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_08ef: ldloc.s 28
				IL_08f1: ldstr "*(LocalExpr + 0x24)"
				IL_08f6: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_08fb: conv.i4
				IL_08fc: conv.r4
				IL_08fd: add
				IL_08fe: stloc.s 62
				IL_0900: ldc.r4 0.0
				IL_0905: ldstr "*(LocalExpr + 0x28)"
				IL_090a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_090f: conv.i4
				IL_0910: conv.r4
				IL_0911: add
				IL_0912: stloc.s 63
				IL_0914: ldstr "movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr"
				IL_0919: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_091e: ldstr "movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr"
				IL_0923: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0928: ldstr "movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr"
				IL_092d: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0932: ldloc.s 28
				IL_0934: ldstr "*(LocalExpr + 0x24)"
				IL_0939: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_093e: conv.i4
				IL_093f: conv.r4
				IL_0940: add
				IL_0941: stloc.s 62
				IL_0943: ldc.r4 0.0
				IL_0948: ldstr "*(LocalExpr + 0x28)"
				IL_094d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0952: conv.i4
				IL_0953: conv.r4
				IL_0954: add
				IL_0955: stloc.s 63
				IL_0957: ldstr "movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr"
				IL_095c: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0961: ldstr "movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr"
				IL_0966: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_096b: ldstr "movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr"
				IL_0970: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0975: ldloc.s 28
				IL_0977: ldstr "*(LocalExpr + 0x24)"
				IL_097c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0981: conv.i4
				IL_0982: conv.r4
				IL_0983: add
				IL_0984: stloc.s 62
				IL_0986: ldc.r4 0.0
				IL_098b: ldstr "*(LocalExpr + 0x28)"
				IL_0990: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0995: conv.i4
				IL_0996: conv.r4
				IL_0997: add
				IL_0998: stloc.s 63
				IL_099a: ldstr "movss dword ptr [rbx+rax*4+20h],xmm7 ; value = BinaryExpr"
				IL_099f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_09a4: ldstr "movss dword ptr [rbx+rax*4+24h],xmm8 ; value = BinaryExpr"
				IL_09a9: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_09ae: ldstr "movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr"
				IL_09b3: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_09b8: ldc.r4 0.0
				IL_09bd: ldstr "*(LocalExpr + 0x28)"
				IL_09c2: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_09c7: conv.i4
				IL_09c8: conv.r4
				IL_09c9: add
				IL_09ca: stloc.s 62
				IL_09cc: ldarg.0
				IL_09cd: ldfld valuetype TMPro.Examples.VertexJitter/VertexAnim[] TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<vertexAnim>5__4'
				IL_09d2: stloc.s 64
				IL_09d4: ldstr "movss dword ptr [rdx+rcx*4+20h],xmm0 ; value = *(LocalExpr + 0x2E0)"
				IL_09d9: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_09de: ldstr "movss dword ptr [rdx+rcx*4+24h],xmm10 ; value = BinaryExpr"
				IL_09e3: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_09e8: ldstr "movss dword ptr [rdx+rcx*4+28h],xmm0 ; value = *(LocalExpr + 0x2F0)"
				IL_09ed: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_09f2: ldstr "*(LocalExpr + 0x2F0)"
				IL_09f7: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_09fc: conv.i4
				IL_09fd: stloc.s 65

				IL_09ff: nop
				IL_0a00: ldc.i4.0
				IL_0a01: ldc.i4.1
				IL_0a02: add
				IL_0a03: stloc.s 66
				IL_0a05: ldc.i4.0
				IL_0a06: ldc.i4.1
				IL_0a07: add
				IL_0a08: ldloc.s 18
				IL_0a0a: clt
				IL_0a0c: brtrue IL_01cb
			// end loop

			IL_0a11: nop
			IL_0a12: ldarg.0
			IL_0a13: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
			IL_0a18: stloc.s 67
			// loop start (head: IL_0a1a)
				IL_0a1a: nop
				IL_0a1b: ldc.i4.0
				IL_0a1c: ldloc.s 67
				IL_0a1e: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
				IL_0a23: ldlen
				IL_0a24: conv.i4
				IL_0a25: clt
				IL_0a27: ldc.i4.0
				IL_0a28: ceq
				IL_0a2a: brtrue.s IL_0a9f

				IL_0a2c: nop
				IL_0a2d: ldarg.0
				IL_0a2e: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
				IL_0a33: stloc.s 68
				IL_0a35: ldloc.s 68
				IL_0a37: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
				IL_0a3c: stloc.s 69
				IL_0a3e: ldstr "*(LocalExpr + 0x20)"
				IL_0a43: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0a48: stloc.s 12
				IL_0a4a: ldc.i4.0
				IL_0a4b: conv.i8
				IL_0a4c: stloc.s 70
				IL_0a4e: ldloc.s 12
				IL_0a50: box [mscorlib]System.Int64
				IL_0a55: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [UnityEngine.CoreModule]UnityEngine.Mesh>(object)
				IL_0a5a: ldstr "*(FieldExpr + 0x30)"
				IL_0a5f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0a64: box [mscorlib]System.Int64
				IL_0a69: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]>(object)
				IL_0a6e: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Mesh::set_vertices(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[])
				IL_0a73: ldloc.2
				IL_0a74: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.VertexJitter::m_TextComponent
				IL_0a79: stloc.s 71
				IL_0a7b: ldc.i4.0
				IL_0a7c: ldc.i4.1
				IL_0a7d: add
				IL_0a7e: stloc.s 72
				IL_0a80: ldarg.0
				IL_0a81: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
				IL_0a86: stloc.s 73
				IL_0a88: ldc.i4.0
				IL_0a89: ldc.i4.1
				IL_0a8a: add
				IL_0a8b: stloc.s 74
				IL_0a8d: ldarg.0
				IL_0a8e: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
				IL_0a93: ldnull
				IL_0a94: ceq
				IL_0a96: ldc.i4.0
				IL_0a97: ceq
				IL_0a99: brtrue IL_0a1a
			// end loop
			IL_0a9e: nop

			IL_0a9f: nop
			IL_0aa0: ldarg.0
			IL_0aa1: ldarg.0
			IL_0aa2: ldfld int32 TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<loopCount>5__3'
			IL_0aa7: ldc.i4.1
			IL_0aa8: add
			IL_0aa9: stfld int32 TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<loopCount>5__3'
			IL_0aae: ldc.r4 0.1
			IL_0ab3: newobj instance void [UnityEngine.CoreModule]UnityEngine.WaitForSeconds::.ctor(float32)
			IL_0ab8: stloc.s 75
			IL_0aba: ldarg.0
			IL_0abb: ldloc.s 75
			IL_0abd: stfld object TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<>2__current'
			IL_0ac2: ldarg.0
			IL_0ac3: ldc.i4.2
			IL_0ac4: stfld int32 TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<>1__state'

			IL_0ac9: nop

			IL_0aca: nop
			IL_0acb: ldc.i4.1
			IL_0acc: ret

			IL_0acd: nop
			IL_0ace: ldc.r4 0.25
			IL_0ad3: newobj instance void [UnityEngine.CoreModule]UnityEngine.WaitForSeconds::.ctor(float32)
			IL_0ad8: stloc.s 76
			IL_0ada: ldarg.0
			IL_0adb: ldloc.s 76
			IL_0add: stfld object TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<>2__current'
			IL_0ae2: ldarg.0
			IL_0ae3: ldc.i4.1
			IL_0ae4: stfld int32 TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<>1__state'
			IL_0ae9: br.s IL_0ac9

			IL_0aeb: nop
			IL_0aec: ldnull
			IL_0aed: throw

			IL_0aee: nop
			IL_0aef: ldnull
			IL_0af0: throw
		} // end of method '<AnimateVertexColors>d__11'::MoveNext

		.method private final hidebysig specialname newslot virtual 
			instance object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () cil managed 
		{
			// Method begins at RVA 0x2ef2
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<>2__current'
			IL_0007: ret
		} // end of method '<AnimateVertexColors>d__11'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'

		.method private final hidebysig newslot virtual 
			instance void System.Collections.IEnumerator.Reset () cil managed 
		{
			// Method begins at RVA 0x1dfe8
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
		} // end of method '<AnimateVertexColors>d__11'::System.Collections.IEnumerator.Reset

		.method private final hidebysig specialname newslot virtual 
			instance object System.Collections.IEnumerator.get_Current () cil managed 
		{
			// Method begins at RVA 0x2efb
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<>2__current'
			IL_0007: ret
		} // end of method '<AnimateVertexColors>d__11'::System.Collections.IEnumerator.get_Current

		// Properties
		.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current'()
		{
			.get instance object TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'()
		}
		.property instance object System.Collections.IEnumerator.Current()
		{
			.get instance object TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::System.Collections.IEnumerator.get_Current()
		}

	} // end of class <AnimateVertexColors>d__11


	// Fields
	.field public float32 AngleMultiplier
	.field public float32 SpeedMultiplier
	.field public float32 CurveScale
	.field private class [Unity.TextMeshPro]TMPro.TMP_Text m_TextComponent
	.field private bool hasTextChanged

	// Methods
	.method private hidebysig 
		instance void Awake () cil managed 
	{
		// Method begins at RVA 0x1961c
		// Header size: 12
		// Code size: 16 (0x10)
		.maxstack 2
		.locals init (
			[0] class [Unity.TextMeshPro]TMPro.TMP_Text
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [Unity.TextMeshPro]TMPro.TMP_Text>()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.VertexJitter::m_TextComponent
		IL_000f: ret
	} // end of method VertexJitter::Awake

	.method private hidebysig 
		instance void OnEnable () cil managed 
	{
		// Method begins at RVA 0x19638
		// Header size: 12
		// Code size: 35 (0x23)
		.maxstack 2
		.locals init (
			[0] class [Unity.TextMeshPro]TMPro.FastAction`1<class [UnityEngine.CoreModule]UnityEngine.Object>,
			[1] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Object>
		)

		IL_0000: nop
		IL_0001: br.s IL_0004

		IL_0003: nop

		IL_0004: nop
		IL_0005: ldsfld class [Unity.TextMeshPro]TMPro.FastAction`1<class [UnityEngine.CoreModule]UnityEngine.Object> [Unity.TextMeshPro]TMPro.TMPro_EventManager::TEXT_CHANGED_EVENT
		IL_000a: stloc.0
		IL_000b: ldarg.0
		IL_000c: ldftn instance void TMPro.Examples.VertexJitter::ON_TEXT_CHANGED(class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0012: newobj instance void [System]System.IOAsyncCallback::.ctor(object, native int)
		IL_0017: stloc.1
		IL_0018: ldloc.0
		IL_0019: ldloc.1
		IL_001a: callvirt instance void class [Unity.TextMeshPro]TMPro.FastAction`1<class [UnityEngine.CoreModule]UnityEngine.Object>::Add(class [mscorlib]System.Action`1<!0>)
		IL_001f: ret

		IL_0020: nop
		IL_0021: ldnull
		IL_0022: throw
	} // end of method VertexJitter::OnEnable

	.method private hidebysig 
		instance void OnDisable () cil managed 
	{
		// Method begins at RVA 0x19668
		// Header size: 12
		// Code size: 35 (0x23)
		.maxstack 2
		.locals init (
			[0] class [Unity.TextMeshPro]TMPro.FastAction`1<class [UnityEngine.CoreModule]UnityEngine.Object>,
			[1] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Object>
		)

		IL_0000: nop
		IL_0001: br.s IL_0004

		IL_0003: nop

		IL_0004: nop
		IL_0005: ldsfld class [Unity.TextMeshPro]TMPro.FastAction`1<class [UnityEngine.CoreModule]UnityEngine.Object> [Unity.TextMeshPro]TMPro.TMPro_EventManager::TEXT_CHANGED_EVENT
		IL_000a: stloc.0
		IL_000b: ldarg.0
		IL_000c: ldftn instance void TMPro.Examples.VertexJitter::ON_TEXT_CHANGED(class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0012: newobj instance void [System]System.IOAsyncCallback::.ctor(object, native int)
		IL_0017: stloc.1
		IL_0018: ldloc.0
		IL_0019: ldloc.1
		IL_001a: callvirt instance void class [Unity.TextMeshPro]TMPro.FastAction`1<class [UnityEngine.CoreModule]UnityEngine.Object>::Remove(class [mscorlib]System.Action`1<!0>)
		IL_001f: ret

		IL_0020: nop
		IL_0021: ldnull
		IL_0022: throw
	} // end of method VertexJitter::OnDisable

	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x19698
		// Header size: 12
		// Code size: 47 (0x2f)
		.maxstack 2
		.locals init (
			[0] class TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11',
			[1] class [UnityEngine.CoreModule]UnityEngine.Coroutine
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(TMPro.Examples.VertexJitter+<AnimateVertexColors>d__11)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class TMPro.Examples.VertexJitter TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<>4__this'
		IL_0026: ldarg.0
		IL_0027: ldloc.0
		IL_0028: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Coroutine [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StartCoroutine(class [mscorlib]System.Collections.IEnumerator)
		IL_002d: stloc.1
		IL_002e: ret
	} // end of method VertexJitter::Start

	.method private hidebysig 
		instance void ON_TEXT_CHANGED (
			class [UnityEngine.CoreModule]UnityEngine.Object obj
		) cil managed 
	{
		// Method begins at RVA 0x196d4
		// Header size: 12
		// Code size: 38 (0x26)
		.maxstack 2
		.locals init (
			[0] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[1] bool,
			[2] bool
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.VertexJitter::m_TextComponent
		IL_0007: stloc.0
		IL_0008: br.s IL_000b

		IL_000a: nop

		IL_000b: nop
		IL_000c: ldarg.1
		IL_000d: ldloc.0
		IL_000e: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Equality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0013: stloc.1
		IL_0014: ldloc.1
		IL_0015: stloc.2
		IL_0016: ldloc.1
		IL_0017: ldc.i4.0
		IL_0018: ceq
		IL_001a: brtrue.s IL_0024

		IL_001c: nop
		IL_001d: ldarg.0
		IL_001e: ldc.i4.1
		IL_001f: stfld bool TMPro.Examples.VertexJitter::hasTextChanged

		IL_0024: nop
		IL_0025: ret
	} // end of method VertexJitter::ON_TEXT_CHANGED

	.method private hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator AnimateVertexColors () cil managed 
	{
		// Method begins at RVA 0x19708
		// Header size: 12
		// Code size: 40 (0x28)
		.maxstack 2
		.locals init (
			[0] class TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(TMPro.Examples.VertexJitter+<AnimateVertexColors>d__11)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class TMPro.Examples.VertexJitter TMPro.Examples.VertexJitter/'<AnimateVertexColors>d__11'::'<>4__this'
		IL_0026: ldloc.0
		IL_0027: ret
	} // end of method VertexJitter::AnimateVertexColors

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2a7a
		// Header size: 1
		// Code size: 41 (0x29)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.r4 1
		IL_0007: stfld float32 TMPro.Examples.VertexJitter::AngleMultiplier
		IL_000c: ldarg.0
		IL_000d: ldc.r4 1
		IL_0012: stfld float32 TMPro.Examples.VertexJitter::SpeedMultiplier
		IL_0017: ldarg.0
		IL_0018: ldc.r4 1
		IL_001d: stfld float32 TMPro.Examples.VertexJitter::CurveScale
		IL_0022: ldarg.0
		IL_0023: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0028: ret
	} // end of method VertexJitter::.ctor

} // end of class TMPro.Examples.VertexJitter
