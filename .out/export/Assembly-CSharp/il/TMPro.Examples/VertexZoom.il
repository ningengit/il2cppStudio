.class public auto ansi beforefieldinit TMPro.Examples.VertexZoom
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested private auto ansi sealed beforefieldinit '<>c__DisplayClass10_0'
		extends [mscorlib]System.Object
	{
		// Fields
		.field public class [mscorlib]System.Collections.Generic.List`1<float32> modifiedCharScale
		.field public class [mscorlib]System.Comparison`1<int32> '<>9__0'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor () cil managed 
		{
			// Method begins at RVA 0x2f50
			// Header size: 1
			// Code size: 9 (0x9)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ret
		} // end of method '<>c__DisplayClass10_0'::.ctor

		.method assembly hidebysig 
			instance int32 '<AnimateVertexColors>b__0' (
				int32 a,
				int32 b
			) cil managed 
		{
			// Method begins at RVA 0x2022c
			// Header size: 12
			// Code size: 49 (0x31)
			.maxstack 2
			.locals init (
				[0] float32,
				[1] float32,
				[2] float32,
				[3] int32
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld class [mscorlib]System.Collections.Generic.List`1<float32> TMPro.Examples.VertexZoom/'<>c__DisplayClass10_0'::modifiedCharScale
			IL_0007: ldarg.1
			IL_0008: callvirt instance !0 class [mscorlib]System.Collections.Generic.List`1<float32>::get_Item(int32)
			IL_000d: stloc.0
			IL_000e: ldarg.0
			IL_000f: ldfld class [mscorlib]System.Collections.Generic.List`1<float32> TMPro.Examples.VertexZoom/'<>c__DisplayClass10_0'::modifiedCharScale
			IL_0014: ldarg.2
			IL_0015: callvirt instance !0 class [mscorlib]System.Collections.Generic.List`1<float32>::get_Item(int32)
			IL_001a: stloc.1
			IL_001b: ldloca.s 2
			IL_001d: ldloc.0
			IL_001e: stfld float32 [mscorlib]System.Single::m_value
			IL_0023: ldloca.s 2
			IL_0025: ldloc.1
			IL_0026: call instance int32 [mscorlib]System.Single::CompareTo(float32)
			IL_002b: stloc.3
			IL_002c: ldloc.3
			IL_002d: ret

			IL_002e: nop
			IL_002f: ldnull
			IL_0030: throw
		} // end of method '<>c__DisplayClass10_0'::'<AnimateVertexColors>b__0'

	} // end of class <>c__DisplayClass10_0

	.class nested private auto ansi sealed beforefieldinit '<AnimateVertexColors>d__10'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerator`1<object>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private object '<>2__current'
		.field public class TMPro.Examples.VertexZoom '<>4__this'
		.field private class TMPro.Examples.VertexZoom/'<>c__DisplayClass10_0' '<>8__1'
		.field private class [Unity.TextMeshPro]TMPro.TMP_TextInfo '<textInfo>5__2'
		.field private valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] '<cachedMeshInfoVertexData>5__3'
		.field private class [mscorlib]System.Collections.Generic.List`1<int32> '<scaleSortingOrder>5__4'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x2f5a
			// Header size: 1
			// Code size: 16 (0x10)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ldarg.0
			IL_0009: ldarg.1
			IL_000a: stfld int32 TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<>1__state'
			IL_000f: ret
		} // end of method '<AnimateVertexColors>d__10'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			// Method begins at RVA 0x2f6b
			// Header size: 1
			// Code size: 2 (0x2)
			.maxstack 8

			IL_0000: nop
			IL_0001: ret
		} // end of method '<AnimateVertexColors>d__10'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			// Method begins at RVA 0x2026c
			// Header size: 12
			// Code size: 2913 (0xb61)
			.maxstack 5
			.locals init (
				[0] int32,
				[1] class TMPro.Examples.VertexZoom,
				[2] class TMPro.Examples.VertexZoom,
				[3] int32,
				[4] class TMPro.Examples.VertexZoom/'<>c__DisplayClass10_0',
				[5] class [Unity.TextMeshPro]TMPro.TMP_Text,
				[6] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[7] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[],
				[8] class TMPro.Examples.VertexZoom/'<>c__DisplayClass10_0',
				[9] class [mscorlib]System.Collections.Generic.List`1<float32>,
				[10] class [mscorlib]System.Collections.Generic.List`1<int32>,
				[11] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[],
				[12] int32,
				[13] class [mscorlib]System.Collections.Generic.List`1<float32>,
				[14] class [mscorlib]System.Collections.Generic.List`1<int32>,
				[15] int64,
				[16] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[],
				[17] int64,
				[18] float32,
				[19] float32,
				[20] float32,
				[21] float32,
				[22] float32,
				[23] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[24] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[25] valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4,
				[26] int64,
				[27] int64,
				[28] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[29] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[30] float32,
				[31] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[32] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[33] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[34] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[35] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[36] float32,
				[37] float32,
				[38] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[],
				[39] int64,
				[40] int64,
				[41] int32,
				[42] int32,
				[43] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[],
				[44] int64,
				[45] int32,
				[46] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[47] class TMPro.Examples.VertexZoom/'<>c__DisplayClass10_0',
				[48] class [mscorlib]System.Collections.Generic.List`1<int32>,
				[49] class [mscorlib]System.Comparison`1<int32>,
				[50] class TMPro.Examples.VertexZoom/'<>c__DisplayClass10_0',
				[51] class [mscorlib]System.Comparison`1<int32>,
				[52] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[53] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[],
				[54] class [mscorlib]System.Collections.Generic.List`1<int32>,
				[55] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[56] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[],
				[57] int64,
				[58] int64,
				[59] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[60] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[],
				[61] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[62] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[],
				[63] class [Unity.TextMeshPro]TMPro.TMP_Text,
				[64] int32,
				[65] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[66] int32,
				[67] class [UnityEngine.CoreModule]UnityEngine.WaitForSeconds,
				[68] class [UnityEngine.CoreModule]UnityEngine.WaitForSeconds
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld int32 TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<>1__state'
			IL_0007: stloc.0
			IL_0008: ldarg.0
			IL_0009: ldfld class TMPro.Examples.VertexZoom TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<>4__this'
			IL_000e: stloc.1
			IL_000f: ldloc.1
			IL_0010: stloc.2
			IL_0011: ldloc.0
			IL_0012: stloc.3
			IL_0013: ldloc.0
			IL_0014: ldc.i4.0
			IL_0015: ceq
			IL_0017: brtrue.s IL_003d

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
			IL_002e: br IL_0b3a

			IL_0033: nop
			IL_0034: ldarg.0
			IL_0035: ldc.i4.m1
			IL_0036: stfld int32 TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<>1__state'
			IL_003b: br.s IL_00b8

			IL_003d: nop
			IL_003e: ldarg.0
			IL_003f: ldc.i4.m1
			IL_0040: stfld int32 TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<>1__state'
			IL_0045: newobj instance void TMPro.Examples.VertexZoom/'<>c__DisplayClass10_0'::.ctor()
			IL_004a: stloc.s 4
			IL_004c: ldloc.s 4
			IL_004e: ldnull
			IL_004f: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0054: ldarg.0
			IL_0055: ldloc.s 4
			IL_0057: stfld class TMPro.Examples.VertexZoom/'<>c__DisplayClass10_0' TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<>8__1'
			IL_005c: ldloc.2
			IL_005d: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.VertexZoom::m_TextComponent
			IL_0062: stloc.s 5
			IL_0064: ldloc.s 5
			IL_0066: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo [Unity.TextMeshPro]TMPro.TMP_Text::m_textInfo
			IL_006b: stloc.s 6
			IL_006d: ldarg.0
			IL_006e: ldloc.s 6
			IL_0070: stfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
			IL_0075: ldarg.0
			IL_0076: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
			IL_007b: callvirt instance valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::CopyMeshInfoVertexData()
			IL_0080: stloc.s 7
			IL_0082: ldarg.0
			IL_0083: ldloc.s 7
			IL_0085: stfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<cachedMeshInfoVertexData>5__3'
			IL_008a: ldarg.0
			IL_008b: ldfld class TMPro.Examples.VertexZoom/'<>c__DisplayClass10_0' TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<>8__1'
			IL_0090: stloc.s 8
			IL_0092: newobj instance void class [mscorlib]System.Collections.Generic.List`1<float32>::.ctor()
			IL_0097: stloc.s 9
			IL_0099: ldloc.s 8
			IL_009b: ldloc.s 9
			IL_009d: stfld class [mscorlib]System.Collections.Generic.List`1<float32> TMPro.Examples.VertexZoom/'<>c__DisplayClass10_0'::modifiedCharScale
			IL_00a2: newobj instance void class [mscorlib]System.Collections.Generic.List`1<int32>::.ctor()
			IL_00a7: stloc.s 10
			IL_00a9: ldarg.0
			IL_00aa: ldloc.s 10
			IL_00ac: stfld class [mscorlib]System.Collections.Generic.List`1<int32> TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<scaleSortingOrder>5__4'
			IL_00b1: ldloc.2
			IL_00b2: ldc.i4.1
			IL_00b3: stfld bool TMPro.Examples.VertexZoom::hasTextChanged

			IL_00b8: nop
			IL_00b9: ldloc.2
			IL_00ba: ldfld bool TMPro.Examples.VertexZoom::hasTextChanged
			IL_00bf: ldc.i4.0
			IL_00c0: ceq
			IL_00c2: brtrue.s IL_00e1

			IL_00c4: nop
			IL_00c5: ldarg.0
			IL_00c6: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
			IL_00cb: callvirt instance valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::CopyMeshInfoVertexData()
			IL_00d0: stloc.s 11
			IL_00d2: ldarg.0
			IL_00d3: ldloc.s 11
			IL_00d5: stfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<cachedMeshInfoVertexData>5__3'
			IL_00da: ldloc.2
			IL_00db: ldc.i4.0
			IL_00dc: stfld bool TMPro.Examples.VertexZoom::hasTextChanged

			IL_00e1: nop
			IL_00e2: ldarg.0
			IL_00e3: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
			IL_00e8: ldfld int32 [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterCount
			IL_00ed: stloc.s 12
			IL_00ef: ldarg.0
			IL_00f0: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
			IL_00f5: ldfld int32 [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterCount
			IL_00fa: ldc.i4.0
			IL_00fb: ceq
			IL_00fd: brtrue IL_0b3d

			IL_0102: nop
			IL_0103: ldarg.0
			IL_0104: ldfld class TMPro.Examples.VertexZoom/'<>c__DisplayClass10_0' TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<>8__1'
			IL_0109: ldfld class [mscorlib]System.Collections.Generic.List`1<float32> TMPro.Examples.VertexZoom/'<>c__DisplayClass10_0'::modifiedCharScale
			IL_010e: stloc.s 13
			IL_0110: ldstr "inc dword ptr [rcx+1Ch] ; value = BinaryExpr"
			IL_0115: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
			IL_011a: ldstr "mov dword ptr [rcx+18h],0 ; value = 0x0"
			IL_011f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
			IL_0124: ldarg.0
			IL_0125: ldfld class [mscorlib]System.Collections.Generic.List`1<int32> TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<scaleSortingOrder>5__4'
			IL_012a: stloc.s 14
			IL_012c: ldstr "inc dword ptr [rcx+1Ch] ; value = BinaryExpr"
			IL_0131: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
			IL_0136: ldstr "mov dword ptr [rcx+18h],0 ; value = 0x0"
			IL_013b: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
			IL_0140: ldloc.s 12
			IL_0142: ldc.i4.0
			IL_0143: cgt
			IL_0145: ldc.i4.0
			IL_0146: ceq
			IL_0148: brtrue IL_095a

			IL_014d: nop
			// loop start (head: IL_014e)
				IL_014e: nop
				IL_014f: ldstr "sub_32D0"
				IL_0154: ldarg.0
				IL_0155: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
				IL_015a: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_CharacterInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterInfo
				IL_015f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
				IL_0164: ldstr "&stack[0xFFFFFFFFFFFFFD78]"
				IL_0169: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_016e: conv.i8
				IL_016f: ldc.i8 96
				IL_0178: add
				IL_0179: ldc.i4.0
				IL_017a: conv.i8
				IL_017b: ldc.i8 0
				IL_0184: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
				IL_0189: stloc.s 15
				IL_018b: ldstr "stack[0xFFFFFFFFFFFFFF4C]"
				IL_0190: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0195: ldc.i4.0
				IL_0196: conv.i8
				IL_0197: ceq
				IL_0199: brtrue IL_0948

				IL_019e: nop
				IL_019f: ldarg.0
				IL_01a0: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<cachedMeshInfoVertexData>5__3'
				IL_01a5: stloc.s 16
				IL_01a7: ldstr "*(LocalExpr + 0x30)"
				IL_01ac: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_01b1: stloc.s 17
				IL_01b3: ldstr "*(LocalExpr + 0x20)"
				IL_01b8: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_01bd: conv.i4
				IL_01be: conv.r4
				IL_01bf: ldstr "*(LocalExpr + 0x20)"
				IL_01c4: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_01c9: conv.i4
				IL_01ca: conv.r4
				IL_01cb: ldstr "*(LocalExpr + 0x20)"
				IL_01d0: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_01d5: conv.i4
				IL_01d6: conv.r4
				IL_01d7: add
				IL_01d8: ldc.r4 0.5
				IL_01dd: mul
				IL_01de: sub
				IL_01df: stloc.s 18
				IL_01e1: ldstr "*(LocalExpr + 0x24)"
				IL_01e6: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_01eb: conv.i4
				IL_01ec: conv.r4
				IL_01ed: ldstr "*(LocalExpr + 0x24)"
				IL_01f2: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_01f7: conv.i4
				IL_01f8: conv.r4
				IL_01f9: ldstr "*(LocalExpr + 0x24)"
				IL_01fe: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0203: conv.i4
				IL_0204: conv.r4
				IL_0205: add
				IL_0206: ldc.r4 0.5
				IL_020b: mul
				IL_020c: sub
				IL_020d: stloc.s 19
				IL_020f: ldstr "*(LocalExpr + 0x28)"
				IL_0214: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0219: conv.i4
				IL_021a: conv.r4
				IL_021b: ldc.r4 0.0
				IL_0220: sub
				IL_0221: stloc.s 20
				IL_0223: ldstr "movss dword ptr [rbx+rcx*4+20h],xmm0 ; value = LocalExpr"
				IL_0228: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_022d: ldstr "movss dword ptr [rbx+rcx*4+24h],xmm1 ; value = LocalExpr"
				IL_0232: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0237: ldstr "movss dword ptr [rbx+rcx*4+28h],xmm2 ; value = LocalExpr"
				IL_023c: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0241: ldstr "*(LocalExpr + 0x20)"
				IL_0246: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_024b: conv.i4
				IL_024c: conv.r4
				IL_024d: ldstr "*(LocalExpr + 0x20)"
				IL_0252: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0257: conv.i4
				IL_0258: conv.r4
				IL_0259: ldstr "*(LocalExpr + 0x20)"
				IL_025e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0263: conv.i4
				IL_0264: conv.r4
				IL_0265: add
				IL_0266: ldc.r4 0.5
				IL_026b: mul
				IL_026c: sub
				IL_026d: stloc.s 18
				IL_026f: ldstr "*(LocalExpr + 0x24)"
				IL_0274: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0279: conv.i4
				IL_027a: conv.r4
				IL_027b: ldstr "*(LocalExpr + 0x24)"
				IL_0280: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0285: conv.i4
				IL_0286: conv.r4
				IL_0287: ldstr "*(LocalExpr + 0x24)"
				IL_028c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0291: conv.i4
				IL_0292: conv.r4
				IL_0293: add
				IL_0294: ldc.r4 0.5
				IL_0299: mul
				IL_029a: sub
				IL_029b: stloc.s 19
				IL_029d: ldstr "*(LocalExpr + 0x28)"
				IL_02a2: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_02a7: conv.i4
				IL_02a8: conv.r4
				IL_02a9: ldc.r4 0.0
				IL_02ae: sub
				IL_02af: stloc.s 20
				IL_02b1: ldstr "movss dword ptr [rbx+rax*4+20h],xmm0 ; value = LocalExpr"
				IL_02b6: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_02bb: ldstr "movss dword ptr [rbx+rax*4+24h],xmm1 ; value = LocalExpr"
				IL_02c0: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_02c5: ldstr "movss dword ptr [rbx+rax*4+28h],xmm2 ; value = LocalExpr"
				IL_02ca: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_02cf: ldstr "*(LocalExpr + 0x20)"
				IL_02d4: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_02d9: conv.i4
				IL_02da: conv.r4
				IL_02db: ldstr "*(LocalExpr + 0x20)"
				IL_02e0: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_02e5: conv.i4
				IL_02e6: conv.r4
				IL_02e7: ldstr "*(LocalExpr + 0x20)"
				IL_02ec: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_02f1: conv.i4
				IL_02f2: conv.r4
				IL_02f3: add
				IL_02f4: ldc.r4 0.5
				IL_02f9: mul
				IL_02fa: sub
				IL_02fb: stloc.s 18
				IL_02fd: ldstr "*(LocalExpr + 0x24)"
				IL_0302: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0307: conv.i4
				IL_0308: conv.r4
				IL_0309: ldstr "*(LocalExpr + 0x24)"
				IL_030e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0313: conv.i4
				IL_0314: conv.r4
				IL_0315: ldstr "*(LocalExpr + 0x24)"
				IL_031a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_031f: conv.i4
				IL_0320: conv.r4
				IL_0321: add
				IL_0322: ldc.r4 0.5
				IL_0327: mul
				IL_0328: sub
				IL_0329: stloc.s 19
				IL_032b: ldstr "*(LocalExpr + 0x28)"
				IL_0330: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0335: conv.i4
				IL_0336: conv.r4
				IL_0337: ldc.r4 0.0
				IL_033c: sub
				IL_033d: stloc.s 20
				IL_033f: ldstr "movss dword ptr [rbx+rax*4+20h],xmm0 ; value = LocalExpr"
				IL_0344: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0349: ldstr "movss dword ptr [rbx+rax*4+24h],xmm1 ; value = LocalExpr"
				IL_034e: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0353: ldstr "movss dword ptr [rbx+rax*4+28h],xmm2 ; value = LocalExpr"
				IL_0358: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_035d: ldstr "*(LocalExpr + 0x20)"
				IL_0362: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0367: conv.i4
				IL_0368: conv.r4
				IL_0369: ldstr "*(LocalExpr + 0x20)"
				IL_036e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0373: conv.i4
				IL_0374: conv.r4
				IL_0375: ldstr "*(LocalExpr + 0x20)"
				IL_037a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_037f: conv.i4
				IL_0380: conv.r4
				IL_0381: add
				IL_0382: ldc.r4 0.5
				IL_0387: mul
				IL_0388: sub
				IL_0389: stloc.s 18
				IL_038b: ldstr "*(LocalExpr + 0x24)"
				IL_0390: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0395: conv.i4
				IL_0396: conv.r4
				IL_0397: ldstr "*(LocalExpr + 0x24)"
				IL_039c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_03a1: conv.i4
				IL_03a2: conv.r4
				IL_03a3: ldstr "*(LocalExpr + 0x24)"
				IL_03a8: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_03ad: conv.i4
				IL_03ae: conv.r4
				IL_03af: add
				IL_03b0: ldc.r4 0.5
				IL_03b5: mul
				IL_03b6: sub
				IL_03b7: stloc.s 19
				IL_03b9: ldstr "*(LocalExpr + 0x28)"
				IL_03be: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_03c3: conv.i4
				IL_03c4: conv.r4
				IL_03c5: ldc.r4 0.0
				IL_03ca: sub
				IL_03cb: stloc.s 20
				IL_03cd: ldstr "movss dword ptr [rbx+rax*4+20h],xmm0 ; value = LocalExpr"
				IL_03d2: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_03d7: ldstr "movss dword ptr [rbx+rax*4+24h],xmm1 ; value = LocalExpr"
				IL_03dc: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_03e1: ldstr "movss dword ptr [rbx+rax*4+28h],xmm2 ; value = LocalExpr"
				IL_03e6: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_03eb: ldc.r4 1
				IL_03f0: ldc.r4 1.5
				IL_03f5: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
				IL_03fa: stloc.s 21
				IL_03fc: ldloc.s 21
				IL_03fe: stloc.s 18
				IL_0400: ldarg.0
				IL_0401: ldfld class TMPro.Examples.VertexZoom/'<>c__DisplayClass10_0' TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<>8__1'
				IL_0406: ldfld class [mscorlib]System.Collections.Generic.List`1<float32> TMPro.Examples.VertexZoom/'<>c__DisplayClass10_0'::modifiedCharScale
				IL_040b: ldloc.s 18
				IL_040d: callvirt instance void class [mscorlib]System.Collections.Generic.List`1<float32>::Add(!0)
				IL_0412: ldarg.0
				IL_0413: ldfld class [mscorlib]System.Collections.Generic.List`1<int32> TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<scaleSortingOrder>5__4'
				IL_0418: ldstr "*(FieldExpr + 0x18)"
				IL_041d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0422: conv.i4
				IL_0423: ldc.i4.1
				IL_0424: sub
				IL_0425: callvirt instance void class [mscorlib]System.Collections.Generic.List`1<int32>::Add(!0)
				IL_042a: ldloc.s 21
				IL_042c: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::oneVector
				IL_0431: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_0436: mul
				IL_0437: stloc.s 22
				IL_0439: ldloca.s 23
				IL_043b: ldc.r4 0.0
				IL_0440: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_0445: ldloca.s 23
				IL_0447: ldc.r4 0.0
				IL_044c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_0451: ldloc.s 23
				IL_0453: ldstr "&stack[0xFFFFFFFFFFFFFD78]"
				IL_0458: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_045d: conv.i8
				IL_045e: ldc.i8 -48
				IL_0467: add
				IL_0468: box [mscorlib]System.Int64
				IL_046d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion>(object)
				IL_0472: ldloca.s 24
				IL_0474: ldstr "unpcklps xmm1,xmm0"
				IL_0479: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_047e: box [mscorlib]System.Int64
				IL_0483: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
				IL_0488: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_048d: ldloca.s 24
				IL_048f: ldloc.s 21
				IL_0491: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::oneVector
				IL_0496: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_049b: mul
				IL_049c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_04a1: ldloc.s 24
				IL_04a3: call valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::TRS(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_04a8: stloc.s 25
				IL_04aa: ldstr "struct_chunk(LocalExpr+0x20, 16)"
				IL_04af: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_04b4: stloc.s 26
				IL_04b6: ldstr "struct_chunk(LocalExpr+0x30, 16)"
				IL_04bb: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_04c0: stloc.s 27
				IL_04c2: ldstr "&stack[0xFFFFFFFFFFFFFD78]"
				IL_04c7: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_04cc: conv.i8
				IL_04cd: ldc.i8 -112
				IL_04d6: add
				IL_04d7: ldloca.s 28
				IL_04d9: ldstr "*(*(FieldExpr + 0x30) + 0x20)"
				IL_04de: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_04e3: box [mscorlib]System.Int64
				IL_04e8: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
				IL_04ed: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_04f2: ldloca.s 28
				IL_04f4: ldstr "*(*(FieldExpr + 0x30) + 0x28)"
				IL_04f9: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_04fe: conv.i4
				IL_04ff: box [mscorlib]System.Int32
				IL_0504: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
				IL_0509: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_050e: ldloc.s 28
				IL_0510: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_0515: stloc.s 29
				IL_0517: ldstr "movsd qword ptr [rbx+rdx*4+20h],xmm0 ; value = StructChunkExpr"
				IL_051c: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0521: ldstr "mov [rbx+rdx*4+28h],r8d ; value = FieldExpr"
				IL_0526: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_052b: ldloca.s 29
				IL_052d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_0532: stloc.s 30
				IL_0534: ldstr "struct_chunk(LocalExpr+0x0, 8)"
				IL_0539: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_053e: stloc.s 26
				IL_0540: ldstr "&stack[0xFFFFFFFFFFFFFD78]"
				IL_0545: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_054a: conv.i8
				IL_054b: ldc.i8 -112
				IL_0554: add
				IL_0555: ldloca.s 31
				IL_0557: ldstr "*(*(FieldExpr + 0x30) + 0x20)"
				IL_055c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0561: box [mscorlib]System.Int64
				IL_0566: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
				IL_056b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_0570: ldloca.s 31
				IL_0572: ldstr "*(*(FieldExpr + 0x30) + 0x28)"
				IL_0577: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_057c: conv.i4
				IL_057d: box [mscorlib]System.Int32
				IL_0582: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
				IL_0587: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_058c: ldloc.s 31
				IL_058e: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_0593: stloc.s 32
				IL_0595: ldstr "movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr"
				IL_059a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_059f: ldstr "mov [rbx+rcx*4+28h],edx ; value = FieldExpr"
				IL_05a4: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_05a9: ldstr "struct_chunk(LocalExpr+0x0, 8)"
				IL_05ae: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_05b3: stloc.s 26
				IL_05b5: ldstr "&stack[0xFFFFFFFFFFFFFD78]"
				IL_05ba: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_05bf: conv.i8
				IL_05c0: ldc.i8 -112
				IL_05c9: add
				IL_05ca: ldloca.s 33
				IL_05cc: ldstr "*(*(FieldExpr + 0x30) + 0x20)"
				IL_05d1: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_05d6: box [mscorlib]System.Int64
				IL_05db: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
				IL_05e0: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_05e5: ldloca.s 33
				IL_05e7: ldstr "*(*(FieldExpr + 0x30) + 0x28)"
				IL_05ec: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_05f1: conv.i4
				IL_05f2: box [mscorlib]System.Int32
				IL_05f7: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
				IL_05fc: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_0601: ldloc.s 33
				IL_0603: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_0608: stloc.s 34
				IL_060a: ldstr "movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr"
				IL_060f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0614: ldstr "mov [rbx+rcx*4+28h],edx ; value = FieldExpr"
				IL_0619: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_061e: ldstr "struct_chunk(LocalExpr+0x0, 8)"
				IL_0623: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0628: stloc.s 26
				IL_062a: ldstr "&stack[0xFFFFFFFFFFFFFD78]"
				IL_062f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0634: conv.i8
				IL_0635: ldc.i8 -112
				IL_063e: add
				IL_063f: ldstr "&stack[0xFFFFFFFFFFFFFD78]"
				IL_0644: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0649: conv.i8
				IL_064a: ldc.i8 -128
				IL_0653: add
				IL_0654: box [mscorlib]System.Int64
				IL_0659: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
				IL_065e: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_0663: stloc.s 35
				IL_0665: ldstr "movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr"
				IL_066a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_066f: ldstr "mov [rbx+rcx*4+28h],edx ; value = FieldExpr"
				IL_0674: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0679: ldstr "struct_chunk(LocalExpr+0x0, 8)"
				IL_067e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0683: stloc.s 26
				IL_0685: ldstr "movss dword ptr [rbx+rcx*4+20h],xmm0 ; value = BinaryExpr"
				IL_068a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_068f: ldstr "movss dword ptr [rbx+rcx*4+24h],xmm0 ; value = BinaryExpr"
				IL_0694: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0699: ldstr "movss dword ptr [rbx+rcx*4+28h],xmm1 ; value = BinaryExpr"
				IL_069e: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_06a3: ldstr "*(LocalExpr + 0x24)"
				IL_06a8: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_06ad: conv.i4
				IL_06ae: conv.r4
				IL_06af: ldstr "*(LocalExpr + 0x24)"
				IL_06b4: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_06b9: conv.i4
				IL_06ba: conv.r4
				IL_06bb: add
				IL_06bc: ldc.r4 0.5
				IL_06c1: mul
				IL_06c2: ldstr "*(*(FieldExpr + 0x30) + 0x24)"
				IL_06c7: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_06cc: conv.i4
				IL_06cd: conv.r4
				IL_06ce: add
				IL_06cf: stloc.s 36
				IL_06d1: ldc.r4 0.0
				IL_06d6: ldstr "*(*(FieldExpr + 0x30) + 0x28)"
				IL_06db: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_06e0: conv.i4
				IL_06e1: conv.r4
				IL_06e2: add
				IL_06e3: stloc.s 37
				IL_06e5: ldstr "movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr"
				IL_06ea: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_06ef: ldstr "movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr"
				IL_06f4: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_06f9: ldstr "movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr"
				IL_06fe: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0703: ldstr "*(LocalExpr + 0x24)"
				IL_0708: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_070d: conv.i4
				IL_070e: conv.r4
				IL_070f: ldstr "*(LocalExpr + 0x24)"
				IL_0714: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0719: conv.i4
				IL_071a: conv.r4
				IL_071b: add
				IL_071c: ldc.r4 0.5
				IL_0721: mul
				IL_0722: ldstr "*(*(FieldExpr + 0x30) + 0x24)"
				IL_0727: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_072c: conv.i4
				IL_072d: conv.r4
				IL_072e: add
				IL_072f: stloc.s 36
				IL_0731: ldc.r4 0.0
				IL_0736: ldstr "*(*(FieldExpr + 0x30) + 0x28)"
				IL_073b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0740: conv.i4
				IL_0741: conv.r4
				IL_0742: add
				IL_0743: stloc.s 37
				IL_0745: ldstr "movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr"
				IL_074a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_074f: ldstr "movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr"
				IL_0754: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0759: ldstr "movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr"
				IL_075e: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0763: ldstr "*(LocalExpr + 0x24)"
				IL_0768: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_076d: conv.i4
				IL_076e: conv.r4
				IL_076f: ldstr "*(LocalExpr + 0x24)"
				IL_0774: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0779: conv.i4
				IL_077a: conv.r4
				IL_077b: add
				IL_077c: ldc.r4 0.5
				IL_0781: mul
				IL_0782: ldstr "*(*(FieldExpr + 0x30) + 0x24)"
				IL_0787: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_078c: conv.i4
				IL_078d: conv.r4
				IL_078e: add
				IL_078f: stloc.s 36
				IL_0791: ldc.r4 0.0
				IL_0796: ldstr "*(*(FieldExpr + 0x30) + 0x28)"
				IL_079b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_07a0: conv.i4
				IL_07a1: conv.r4
				IL_07a2: add
				IL_07a3: stloc.s 37
				IL_07a5: ldstr "movss dword ptr [rbx+rax*4+20h],xmm6 ; value = BinaryExpr"
				IL_07aa: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_07af: ldstr "movss dword ptr [rbx+rax*4+24h],xmm7 ; value = BinaryExpr"
				IL_07b4: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_07b9: ldstr "movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr"
				IL_07be: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_07c3: ldarg.0
				IL_07c4: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<cachedMeshInfoVertexData>5__3'
				IL_07c9: stloc.s 38
				IL_07cb: ldc.r4 0.0
				IL_07d0: ldstr "*(*(FieldExpr + 0x30) + 0x28)"
				IL_07d5: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_07da: conv.i4
				IL_07db: conv.r4
				IL_07dc: add
				IL_07dd: stloc.s 36
				IL_07df: ldstr "*(LocalExpr + 0x48)"
				IL_07e4: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_07e9: stloc.s 39
				IL_07eb: ldstr "movss dword ptr [rdx+rdi*8+20h],xmm0 ; value = *(LocalExpr + 0x20)"
				IL_07f0: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_07f5: ldstr "movss dword ptr [rdx+rdi*8+24h],xmm1 ; value = *(LocalExpr + 0x24)"
				IL_07fa: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_07ff: ldstr "*(BinaryExpr + 0x6C)"
				IL_0804: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0809: conv.i4
				IL_080a: conv.i8
				IL_080b: ldc.i8 1
				IL_0814: add
				IL_0815: stloc.s 40
				IL_0817: ldstr "*(LocalExpr + 0x20)"
				IL_081c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0821: conv.i4
				IL_0822: stloc.s 41
				IL_0824: ldstr "*(LocalExpr + 0x24)"
				IL_0829: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_082e: conv.i4
				IL_082f: stloc.s 42
				IL_0831: ldstr "movss dword ptr [rdx+rcx*8+24h],xmm0 ; value = *(LocalExpr + 0x24)"
				IL_0836: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_083b: ldstr "mov [rdx+rcx*8+20h],eax ; value = *(LocalExpr + 0x20)"
				IL_0840: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0845: ldstr "*(BinaryExpr + 0x6C)"
				IL_084a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_084f: conv.i4
				IL_0850: conv.i8
				IL_0851: ldc.i8 2
				IL_085a: add
				IL_085b: stloc.s 40
				IL_085d: ldstr "*(LocalExpr + 0x24)"
				IL_0862: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0867: conv.i4
				IL_0868: stloc.s 41
				IL_086a: ldstr "movss dword ptr [rdx+rcx*8+24h],xmm0 ; value = *(LocalExpr + 0x24)"
				IL_086f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0874: ldstr "mov [rdx+rcx*8+20h],eax ; value = *(LocalExpr + 0x20)"
				IL_0879: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_087e: ldstr "*(BinaryExpr + 0x6C)"
				IL_0883: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0888: conv.i4
				IL_0889: conv.i8
				IL_088a: ldc.i8 3
				IL_0893: add
				IL_0894: stloc.s 40
				IL_0896: ldstr "*(LocalExpr + 0x24)"
				IL_089b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_08a0: conv.i4
				IL_08a1: stloc.s 41
				IL_08a3: ldstr "movss dword ptr [rdx+rcx*8+24h],xmm0 ; value = *(LocalExpr + 0x24)"
				IL_08a8: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_08ad: ldstr "mov [rdx+rcx*8+20h],eax ; value = *(LocalExpr + 0x20)"
				IL_08b2: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_08b7: ldarg.0
				IL_08b8: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<cachedMeshInfoVertexData>5__3'
				IL_08bd: stloc.s 43
				IL_08bf: ldstr "*(LocalExpr + 0x24)"
				IL_08c4: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_08c9: conv.i4
				IL_08ca: stloc.s 41
				IL_08cc: ldstr "*(LocalExpr + 0x58)"
				IL_08d1: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_08d6: stloc.s 44
				IL_08d8: ldstr "mov [rdx+rdi*4+20h],eax ; value = *(LocalExpr + 0x20)"
				IL_08dd: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_08e2: ldstr "*(BinaryExpr + 0x6C)"
				IL_08e7: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_08ec: conv.i4
				IL_08ed: conv.i8
				IL_08ee: ldc.i8 1
				IL_08f7: add
				IL_08f8: stloc.s 40
				IL_08fa: ldstr "mov [rdx+rcx*4+20h],eax ; value = *(LocalExpr + 0x20)"
				IL_08ff: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0904: ldstr "*(BinaryExpr + 0x6C)"
				IL_0909: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_090e: conv.i4
				IL_090f: conv.i8
				IL_0910: ldc.i8 2
				IL_0919: add
				IL_091a: stloc.s 40
				IL_091c: ldstr "mov [rdx+rcx*4+20h],eax ; value = *(LocalExpr + 0x20)"
				IL_0921: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
				IL_0926: ldstr "*(BinaryExpr + 0x6C)"
				IL_092b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0930: conv.i4
				IL_0931: conv.i8
				IL_0932: ldc.i8 3
				IL_093b: add
				IL_093c: stloc.s 40
				IL_093e: ldstr "mov [rdx+rcx*4+20h],eax ; value = *(LocalExpr + 0x20)"
				IL_0943: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)

				IL_0948: nop
				IL_0949: ldc.i4.0
				IL_094a: ldc.i4.1
				IL_094b: add
				IL_094c: stloc.s 45
				IL_094e: ldc.i4.0
				IL_094f: ldc.i4.1
				IL_0950: add
				IL_0951: ldloc.s 12
				IL_0953: clt
				IL_0955: brtrue IL_014e
			// end loop

			IL_095a: nop
			IL_095b: ldarg.0
			IL_095c: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
			IL_0961: stloc.s 46
			// loop start (head: IL_0963)
				IL_0963: nop
				IL_0964: ldc.i4.0
				IL_0965: ldloc.s 46
				IL_0967: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
				IL_096c: ldlen
				IL_096d: conv.i4
				IL_096e: clt
				IL_0970: ldc.i4.0
				IL_0971: ceq
				IL_0973: brtrue IL_0b1d

				IL_0978: nop
				IL_0979: ldarg.0
				IL_097a: ldfld class TMPro.Examples.VertexZoom/'<>c__DisplayClass10_0' TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<>8__1'
				IL_097f: stloc.s 47
				IL_0981: ldarg.0
				IL_0982: ldfld class [mscorlib]System.Collections.Generic.List`1<int32> TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<scaleSortingOrder>5__4'
				IL_0987: stloc.s 48
				IL_0989: ldloc.s 47
				IL_098b: ldfld class [mscorlib]System.Comparison`1<int32> TMPro.Examples.VertexZoom/'<>c__DisplayClass10_0'::'<>9__0'
				IL_0990: stloc.s 49
				IL_0992: ldloc.s 47
				IL_0994: ldfld class [mscorlib]System.Comparison`1<int32> TMPro.Examples.VertexZoom/'<>c__DisplayClass10_0'::'<>9__0'
				IL_0999: ldnull
				IL_099a: ceq
				IL_099c: ldc.i4.0
				IL_099d: ceq
				IL_099f: brtrue.s IL_09c6

				IL_09a1: nop
				IL_09a2: ldarg.0
				IL_09a3: ldfld class TMPro.Examples.VertexZoom/'<>c__DisplayClass10_0' TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<>8__1'
				IL_09a8: stloc.s 50
				IL_09aa: ldloc.s 50
				IL_09ac: ldftn instance int32 TMPro.Examples.VertexZoom/'<>c__DisplayClass10_0'::'<AnimateVertexColors>b__0'(int32, int32)
				IL_09b2: newobj instance void class [mscorlib]System.Comparison`1<int32>::.ctor(object, native int)
				IL_09b7: stloc.s 51
				IL_09b9: ldloc.s 50
				IL_09bb: ldloc.s 51
				IL_09bd: stfld class [mscorlib]System.Comparison`1<int32> TMPro.Examples.VertexZoom/'<>c__DisplayClass10_0'::'<>9__0'
				IL_09c2: ldloc.s 51
				IL_09c4: stloc.s 49

				IL_09c6: nop
				IL_09c7: ldloc.s 48
				IL_09c9: ldloc.s 49
				IL_09cb: callvirt instance void class [mscorlib]System.Collections.Generic.List`1<int32>::Sort(class [mscorlib]System.Comparison`1<!0>)
				IL_09d0: ldarg.0
				IL_09d1: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
				IL_09d6: stloc.s 52
				IL_09d8: ldloc.s 52
				IL_09da: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
				IL_09df: stloc.s 53
				IL_09e1: ldarg.0
				IL_09e2: ldfld class [mscorlib]System.Collections.Generic.List`1<int32> TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<scaleSortingOrder>5__4'
				IL_09e7: stloc.s 54
				IL_09e9: br.s IL_09ec

				IL_09eb: nop

				IL_09ec: nop
				IL_09ed: ldc.i4.0
				IL_09ee: conv.i8
				IL_09ef: ldc.i4.0
				IL_09f0: conv.i8
				IL_09f1: ldc.i8 4
				IL_09fa: mul
				IL_09fb: add
				IL_09fc: ldc.i8 4
				IL_0a05: shl
				IL_0a06: ldc.i8 32
				IL_0a0f: add
				IL_0a10: ldloc.s 53
				IL_0a12: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
				IL_0a17: add
				IL_0a18: ldloc.s 54
				IL_0a1a: call instance void [Unity.TextMeshPro]TMPro.TMP_MeshInfo::SortGeometry(class [mscorlib]System.Collections.Generic.IList`1<int32>)
				IL_0a1f: ldarg.0
				IL_0a20: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
				IL_0a25: stloc.s 55
				IL_0a27: ldloc.s 55
				IL_0a29: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
				IL_0a2e: stloc.s 56
				IL_0a30: ldstr "*(LocalExpr + 0x20)"
				IL_0a35: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0a3a: stloc.s 57
				IL_0a3c: ldc.i4.0
				IL_0a3d: conv.i8
				IL_0a3e: stloc.s 58
				IL_0a40: ldloc.s 57
				IL_0a42: box [mscorlib]System.Int64
				IL_0a47: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [UnityEngine.CoreModule]UnityEngine.Mesh>(object)
				IL_0a4c: ldstr "*(FieldExpr + 0x30)"
				IL_0a51: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0a56: box [mscorlib]System.Int64
				IL_0a5b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]>(object)
				IL_0a60: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Mesh::set_vertices(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[])
				IL_0a65: ldarg.0
				IL_0a66: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
				IL_0a6b: stloc.s 59
				IL_0a6d: ldloc.s 59
				IL_0a6f: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
				IL_0a74: stloc.s 60
				IL_0a76: ldstr "*(LocalExpr + 0x20)"
				IL_0a7b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0a80: stloc.s 57
				IL_0a82: ldc.i4.0
				IL_0a83: conv.i8
				IL_0a84: stloc.s 58
				IL_0a86: ldloc.s 57
				IL_0a88: box [mscorlib]System.Int64
				IL_0a8d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [UnityEngine.CoreModule]UnityEngine.Mesh>(object)
				IL_0a92: ldstr "*(FieldExpr + 0x48)"
				IL_0a97: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0a9c: box [mscorlib]System.Int64
				IL_0aa1: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector2[]>(object)
				IL_0aa6: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Mesh::set_uv(valuetype [UnityEngine.CoreModule]UnityEngine.Vector2[])
				IL_0aab: ldarg.0
				IL_0aac: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
				IL_0ab1: stloc.s 61
				IL_0ab3: ldloc.s 61
				IL_0ab5: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
				IL_0aba: stloc.s 62
				IL_0abc: ldstr "*(LocalExpr + 0x20)"
				IL_0ac1: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0ac6: stloc.s 57
				IL_0ac8: ldc.i4.0
				IL_0ac9: conv.i8
				IL_0aca: stloc.s 58
				IL_0acc: ldloc.s 57
				IL_0ace: box [mscorlib]System.Int64
				IL_0ad3: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [UnityEngine.CoreModule]UnityEngine.Mesh>(object)
				IL_0ad8: ldstr "*(FieldExpr + 0x58)"
				IL_0add: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0ae2: box [mscorlib]System.Int64
				IL_0ae7: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Color32[]>(object)
				IL_0aec: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Mesh::set_colors32(valuetype [UnityEngine.CoreModule]UnityEngine.Color32[])
				IL_0af1: ldloc.2
				IL_0af2: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.VertexZoom::m_TextComponent
				IL_0af7: stloc.s 63
				IL_0af9: ldc.i4.0
				IL_0afa: ldc.i4.1
				IL_0afb: add
				IL_0afc: stloc.s 64
				IL_0afe: ldarg.0
				IL_0aff: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
				IL_0b04: stloc.s 65
				IL_0b06: ldc.i4.0
				IL_0b07: ldc.i4.1
				IL_0b08: add
				IL_0b09: stloc.s 66
				IL_0b0b: ldarg.0
				IL_0b0c: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
				IL_0b11: ldnull
				IL_0b12: ceq
				IL_0b14: ldc.i4.0
				IL_0b15: ceq
				IL_0b17: brtrue IL_0963
			// end loop
			IL_0b1c: nop

			IL_0b1d: nop
			IL_0b1e: ldc.r4 0.1
			IL_0b23: newobj instance void [UnityEngine.CoreModule]UnityEngine.WaitForSeconds::.ctor(float32)
			IL_0b28: stloc.s 67
			IL_0b2a: ldarg.0
			IL_0b2b: ldloc.s 67
			IL_0b2d: stfld object TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<>2__current'
			IL_0b32: ldarg.0
			IL_0b33: ldc.i4.2
			IL_0b34: stfld int32 TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<>1__state'

			IL_0b39: nop

			IL_0b3a: nop
			IL_0b3b: ldc.i4.1
			IL_0b3c: ret

			IL_0b3d: nop
			IL_0b3e: ldc.r4 0.25
			IL_0b43: newobj instance void [UnityEngine.CoreModule]UnityEngine.WaitForSeconds::.ctor(float32)
			IL_0b48: stloc.s 68
			IL_0b4a: ldarg.0
			IL_0b4b: ldloc.s 68
			IL_0b4d: stfld object TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<>2__current'
			IL_0b52: ldarg.0
			IL_0b53: ldc.i4.1
			IL_0b54: stfld int32 TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<>1__state'
			IL_0b59: br.s IL_0b39

			IL_0b5b: nop
			IL_0b5c: ldnull
			IL_0b5d: throw

			IL_0b5e: nop
			IL_0b5f: ldnull
			IL_0b60: throw
		} // end of method '<AnimateVertexColors>d__10'::MoveNext

		.method private final hidebysig specialname newslot virtual 
			instance object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () cil managed 
		{
			// Method begins at RVA 0x2f6e
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<>2__current'
			IL_0007: ret
		} // end of method '<AnimateVertexColors>d__10'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'

		.method private final hidebysig newslot virtual 
			instance void System.Collections.IEnumerator.Reset () cil managed 
		{
			// Method begins at RVA 0x20ddc
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
		} // end of method '<AnimateVertexColors>d__10'::System.Collections.IEnumerator.Reset

		.method private final hidebysig specialname newslot virtual 
			instance object System.Collections.IEnumerator.get_Current () cil managed 
		{
			// Method begins at RVA 0x2f77
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<>2__current'
			IL_0007: ret
		} // end of method '<AnimateVertexColors>d__10'::System.Collections.IEnumerator.get_Current

		// Properties
		.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current'()
		{
			.get instance object TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'()
		}
		.property instance object System.Collections.IEnumerator.Current()
		{
			.get instance object TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::System.Collections.IEnumerator.get_Current()
		}

	} // end of class <AnimateVertexColors>d__10


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
		// Method begins at RVA 0x1997c
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
		IL_000a: stfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.VertexZoom::m_TextComponent
		IL_000f: ret
	} // end of method VertexZoom::Awake

	.method private hidebysig 
		instance void OnEnable () cil managed 
	{
		// Method begins at RVA 0x19998
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
		IL_000c: ldftn instance void TMPro.Examples.VertexZoom::ON_TEXT_CHANGED(class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0012: newobj instance void [System]System.IOAsyncCallback::.ctor(object, native int)
		IL_0017: stloc.1
		IL_0018: ldloc.0
		IL_0019: ldloc.1
		IL_001a: callvirt instance void class [Unity.TextMeshPro]TMPro.FastAction`1<class [UnityEngine.CoreModule]UnityEngine.Object>::Add(class [mscorlib]System.Action`1<!0>)
		IL_001f: ret

		IL_0020: nop
		IL_0021: ldnull
		IL_0022: throw
	} // end of method VertexZoom::OnEnable

	.method private hidebysig 
		instance void OnDisable () cil managed 
	{
		// Method begins at RVA 0x199c8
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
		IL_000c: ldftn instance void TMPro.Examples.VertexZoom::ON_TEXT_CHANGED(class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0012: newobj instance void [System]System.IOAsyncCallback::.ctor(object, native int)
		IL_0017: stloc.1
		IL_0018: ldloc.0
		IL_0019: ldloc.1
		IL_001a: callvirt instance void class [Unity.TextMeshPro]TMPro.FastAction`1<class [UnityEngine.CoreModule]UnityEngine.Object>::Remove(class [mscorlib]System.Action`1<!0>)
		IL_001f: ret

		IL_0020: nop
		IL_0021: ldnull
		IL_0022: throw
	} // end of method VertexZoom::OnDisable

	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x199f8
		// Header size: 12
		// Code size: 47 (0x2f)
		.maxstack 2
		.locals init (
			[0] class TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10',
			[1] class [UnityEngine.CoreModule]UnityEngine.Coroutine
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(TMPro.Examples.VertexZoom+<AnimateVertexColors>d__10)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class TMPro.Examples.VertexZoom TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<>4__this'
		IL_0026: ldarg.0
		IL_0027: ldloc.0
		IL_0028: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Coroutine [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StartCoroutine(class [mscorlib]System.Collections.IEnumerator)
		IL_002d: stloc.1
		IL_002e: ret
	} // end of method VertexZoom::Start

	.method private hidebysig 
		instance void ON_TEXT_CHANGED (
			class [UnityEngine.CoreModule]UnityEngine.Object obj
		) cil managed 
	{
		// Method begins at RVA 0x19a34
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
		IL_0002: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.VertexZoom::m_TextComponent
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
		IL_001f: stfld bool TMPro.Examples.VertexZoom::hasTextChanged

		IL_0024: nop
		IL_0025: ret
	} // end of method VertexZoom::ON_TEXT_CHANGED

	.method private hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator AnimateVertexColors () cil managed 
	{
		// Method begins at RVA 0x19a68
		// Header size: 12
		// Code size: 40 (0x28)
		.maxstack 2
		.locals init (
			[0] class TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(TMPro.Examples.VertexZoom+<AnimateVertexColors>d__10)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class TMPro.Examples.VertexZoom TMPro.Examples.VertexZoom/'<AnimateVertexColors>d__10'::'<>4__this'
		IL_0026: ldloc.0
		IL_0027: ret
	} // end of method VertexZoom::AnimateVertexColors

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2b03
		// Header size: 1
		// Code size: 41 (0x29)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.r4 1
		IL_0007: stfld float32 TMPro.Examples.VertexZoom::AngleMultiplier
		IL_000c: ldarg.0
		IL_000d: ldc.r4 1
		IL_0012: stfld float32 TMPro.Examples.VertexZoom::SpeedMultiplier
		IL_0017: ldarg.0
		IL_0018: ldc.r4 1
		IL_001d: stfld float32 TMPro.Examples.VertexZoom::CurveScale
		IL_0022: ldarg.0
		IL_0023: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0028: ret
	} // end of method VertexZoom::.ctor

} // end of class TMPro.Examples.VertexZoom
