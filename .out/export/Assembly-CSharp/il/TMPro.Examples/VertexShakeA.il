.class public auto ansi beforefieldinit TMPro.Examples.VertexShakeA
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested private auto ansi sealed beforefieldinit '<AnimateVertexColors>d__11'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerator`1<object>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private object '<>2__current'
		.field public class TMPro.Examples.VertexShakeA '<>4__this'
		.field private class [Unity.TextMeshPro]TMPro.TMP_TextInfo '<textInfo>5__2'
		.field private valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] '<copyOfVertices>5__3'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x2f04
			// Header size: 1
			// Code size: 16 (0x10)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ldarg.0
			IL_0009: ldarg.1
			IL_000a: stfld int32 TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<>1__state'
			IL_000f: ret
		} // end of method '<AnimateVertexColors>d__11'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			// Method begins at RVA 0x2f15
			// Header size: 1
			// Code size: 2 (0x2)
			.maxstack 8

			IL_0000: nop
			IL_0001: ret
		} // end of method '<AnimateVertexColors>d__11'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			// Method begins at RVA 0x1e038
			// Header size: 12
			// Code size: 2729 (0xaa9)
			.maxstack 5
			.locals init (
				[0] int32,
				[1] class TMPro.Examples.VertexShakeA,
				[2] class TMPro.Examples.VertexShakeA,
				[3] int32,
				[4] class [Unity.TextMeshPro]TMPro.TMP_Text,
				[5] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[6] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][],
				[7] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][],
				[8] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[9] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[10] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[],
				[11] int64,
				[12] int64,
				[13] int64,
				[14] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[15] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[16] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][],
				[17] int32,
				[18] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[19] int32,
				[20] int32,
				[21] valuetype [Unity.TextMeshPro]TMPro.TMP_CharacterInfo[],
				[22] int64,
				[23] int64,
				[24] float32,
				[25] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[26] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
				[27] float32,
				[28] float32,
				[29] float32,
				[30] int64,
				[31] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[],
				[32] int64,
				[33] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][],
				[34] int64,
				[35] int64,
				[36] float32,
				[37] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[38] float32,
				[39] float32,
				[40] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][],
				[41] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[42] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][],
				[43] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[44] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][],
				[45] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[46] float32,
				[47] int32,
				[48] float32,
				[49] float32,
				[50] int64,
				[51] float32,
				[52] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[53] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[54] valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4,
				[55] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][],
				[56] int64,
				[57] int64,
				[58] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[59] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[60] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[61] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][],
				[62] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[63] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[64] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[65] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[66] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[67] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[68] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[69] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[70] float32,
				[71] float32,
				[72] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[73] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[74] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[75] int32,
				[76] int32,
				[77] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[78] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[],
				[79] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][],
				[80] int64,
				[81] int64,
				[82] class [Unity.TextMeshPro]TMPro.TMP_Text,
				[83] int32,
				[84] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[85] int32,
				[86] class [UnityEngine.CoreModule]UnityEngine.WaitForSeconds,
				[87] class [UnityEngine.CoreModule]UnityEngine.WaitForSeconds
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld int32 TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<>1__state'
			IL_0007: stloc.0
			IL_0008: ldarg.0
			IL_0009: ldfld class TMPro.Examples.VertexShakeA TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<>4__this'
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
			IL_002e: br IL_0a82

			IL_0033: nop
			IL_0034: ldarg.0
			IL_0035: ldc.i4.m1
			IL_0036: stfld int32 TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<>1__state'
			IL_003b: br.s IL_0075

			IL_003d: nop
			IL_003e: ldarg.0
			IL_003f: ldc.i4.m1
			IL_0040: stfld int32 TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<>1__state'
			IL_0045: ldloc.2
			IL_0046: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.VertexShakeA::m_TextComponent
			IL_004b: stloc.s 4
			IL_004d: ldloc.s 4
			IL_004f: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo [Unity.TextMeshPro]TMPro.TMP_Text::m_textInfo
			IL_0054: stloc.s 5
			IL_0056: ldarg.0
			IL_0057: ldloc.s 5
			IL_0059: stfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
			IL_005e: ldc.i4.0
			IL_005f: newarr valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
			IL_0064: stloc.s 6
			IL_0066: ldarg.0
			IL_0067: ldloc.s 6
			IL_0069: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<copyOfVertices>5__3'
			IL_006e: ldloc.2
			IL_006f: ldc.i4.1
			IL_0070: stfld bool TMPro.Examples.VertexShakeA::hasTextChanged

			IL_0075: nop
			IL_0076: ldloc.2
			IL_0077: ldfld bool TMPro.Examples.VertexShakeA::hasTextChanged
			IL_007c: ldc.i4.0
			IL_007d: ceq
			IL_007f: brtrue IL_0175

			IL_0084: nop
			IL_0085: ldarg.0
			IL_0086: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<copyOfVertices>5__3'
			IL_008b: ldlen
			IL_008c: conv.i4
			IL_008d: ldarg.0
			IL_008e: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
			IL_0093: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
			IL_0098: ldlen
			IL_0099: conv.i4
			IL_009a: clt
			IL_009c: ldc.i4.0
			IL_009d: ceq
			IL_009f: brtrue.s IL_00be

			IL_00a1: nop
			IL_00a2: ldarg.0
			IL_00a3: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
			IL_00a8: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
			IL_00ad: ldlen
			IL_00ae: conv.i4
			IL_00af: newarr valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
			IL_00b4: stloc.s 7
			IL_00b6: ldarg.0
			IL_00b7: ldloc.s 7
			IL_00b9: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<copyOfVertices>5__3'

			IL_00be: nop
			IL_00bf: ldarg.0
			IL_00c0: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
			IL_00c5: stloc.s 8
			// loop start (head: IL_00c7)
				IL_00c7: nop
				IL_00c8: ldc.i4.0
				IL_00c9: ldloc.s 8
				IL_00cb: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
				IL_00d0: ldlen
				IL_00d1: conv.i4
				IL_00d2: clt
				IL_00d4: ldc.i4.0
				IL_00d5: ceq
				IL_00d7: brtrue IL_016d

				IL_00dc: nop
				IL_00dd: ldarg.0
				IL_00de: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
				IL_00e3: stloc.s 9
				IL_00e5: ldloc.s 9
				IL_00e7: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
				IL_00ec: stloc.s 10
				IL_00ee: ldc.i4.0
				IL_00ef: conv.i8
				IL_00f0: ldc.i4.0
				IL_00f1: conv.i8
				IL_00f2: ldc.i8 4
				IL_00fb: mul
				IL_00fc: add
				IL_00fd: ldc.i4.0
				IL_00fe: conv.i8
				IL_00ff: ldc.i4.0
				IL_0100: conv.i8
				IL_0101: ldc.i8 4
				IL_010a: mul
				IL_010b: add
				IL_010c: add
				IL_010d: stloc.s 11
				IL_010f: ldstr "*(LocalExpr + 0x30)"
				IL_0114: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0119: stloc.s 12
				IL_011b: ldc.i4.0
				IL_011c: conv.i8
				IL_011d: stloc.s 13
				IL_011f: ldstr "*(LocalExpr + 0x18)"
				IL_0124: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0129: conv.i4
				IL_012a: newarr [UnityEngine.CoreModule]UnityEngine.Vector3
				IL_012f: stloc.s 14
				IL_0131: ldloc.s 14
				IL_0133: stloc.s 15
				IL_0135: ldarg.0
				IL_0136: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<copyOfVertices>5__3'
				IL_013b: stloc.s 16
				IL_013d: ldloc.s 16
				IL_013f: ldc.i4.0
				IL_0140: conv.i8
				IL_0141: conv.i4
				IL_0142: ldloc.s 15
				IL_0144: stelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
				IL_0149: ldc.i4.0
				IL_014a: ldc.i4.1
				IL_014b: add
				IL_014c: stloc.s 17
				IL_014e: ldarg.0
				IL_014f: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
				IL_0154: stloc.s 18
				IL_0156: ldc.i4.0
				IL_0157: ldc.i4.1
				IL_0158: add
				IL_0159: stloc.s 19
				IL_015b: ldarg.0
				IL_015c: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
				IL_0161: ldnull
				IL_0162: ceq
				IL_0164: ldc.i4.0
				IL_0165: ceq
				IL_0167: brtrue IL_00c7
			// end loop
			IL_016c: nop

			IL_016d: nop
			IL_016e: ldloc.2
			IL_016f: ldc.i4.0
			IL_0170: stfld bool TMPro.Examples.VertexShakeA::hasTextChanged

			IL_0175: nop
			IL_0176: ldarg.0
			IL_0177: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
			IL_017c: stloc.s 18
			IL_017e: ldloc.s 18
			IL_0180: ldfld int32 [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterCount
			IL_0185: ldc.i4.0
			IL_0186: ceq
			IL_0188: brtrue IL_0a85

			IL_018d: nop
			IL_018e: ldloc.s 18
			IL_0190: ldfld int32 [Unity.TextMeshPro]TMPro.TMP_TextInfo::lineCount
			IL_0195: stloc.s 20
			IL_0197: ldloc.s 20
			IL_0199: ldc.i4.0
			IL_019a: cgt
			IL_019c: ldc.i4.0
			IL_019d: ceq
			IL_019f: brtrue IL_09d9

			IL_01a4: nop
			// loop start (head: IL_01a5)
				IL_01a5: nop
				IL_01a6: ldarg.0
				IL_01a7: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
				IL_01ac: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_CharacterInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterInfo
				IL_01b1: stloc.s 21
				IL_01b3: ldstr "*(FieldExpr + 0x34)"
				IL_01b8: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_01bd: conv.i4
				IL_01be: conv.i8
				IL_01bf: stloc.s 22
				IL_01c1: ldstr "*(BinaryExpr + 0x3C)"
				IL_01c6: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_01cb: conv.i4
				IL_01cc: conv.i8
				IL_01cd: stloc.s 23
				IL_01cf: ldc.r4 -0.25
				IL_01d4: ldc.r4 0.25
				IL_01d9: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
				IL_01de: stloc.s 24
				IL_01e0: ldloca.s 25
				IL_01e2: ldc.r4 0.0
				IL_01e7: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_01ec: ldloca.s 25
				IL_01ee: ldloc.s 24
				IL_01f0: ldloc.2
				IL_01f1: ldfld float32 TMPro.Examples.VertexShakeA::RotationMultiplier
				IL_01f6: mul
				IL_01f7: ldc.r4 0.017453292
				IL_01fc: mul
				IL_01fd: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_0202: ldloc.s 25
				IL_0204: call valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_FromEulerRad(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_0209: stloc.s 26
				IL_020b: ldstr "*(FieldExpr + 0x128)"
				IL_0210: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0215: conv.i4
				IL_0216: conv.r4
				IL_0217: ldstr "*(LocalExpr + 0x11C)"
				IL_021c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0221: conv.i4
				IL_0222: conv.r4
				IL_0223: add
				IL_0224: ldc.r4 0.5
				IL_0229: mul
				IL_022a: stloc.s 27
				IL_022c: ldstr "*(FieldExpr + 0x12C)"
				IL_0231: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0236: conv.i4
				IL_0237: conv.r4
				IL_0238: ldstr "*(LocalExpr + 0x120)"
				IL_023d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0242: conv.i4
				IL_0243: conv.r4
				IL_0244: add
				IL_0245: ldc.r4 0.5
				IL_024a: mul
				IL_024b: stloc.s 28
				IL_024d: ldstr "*(FieldExpr + 0x130)"
				IL_0252: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0257: conv.i4
				IL_0258: conv.r4
				IL_0259: ldstr "*(LocalExpr + 0x124)"
				IL_025e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0263: conv.i4
				IL_0264: conv.r4
				IL_0265: add
				IL_0266: ldc.r4 0.5
				IL_026b: mul
				IL_026c: stloc.s 29
				IL_026e: ldstr "struct_chunk(LocalExpr+0x0, 16)"
				IL_0273: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0278: stloc.s 30
				IL_027a: ldloc.s 22
				IL_027c: conv.i4
				IL_027d: ldloc.s 23
				IL_027f: conv.i4
				IL_0280: cgt
				IL_0282: brtrue IL_09aa

				IL_0287: nop
				// loop start (head: IL_0288)
					IL_0288: nop
					IL_0289: ldstr "*(FieldExpr + 0x194)"
					IL_028e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0293: ldc.i4.0
					IL_0294: conv.i8
					IL_0295: ceq
					IL_0297: brtrue IL_0990

					IL_029c: nop
					IL_029d: ldarg.0
					IL_029e: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
					IL_02a3: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
					IL_02a8: stloc.s 31
					IL_02aa: ldloc.s 22
					IL_02ac: conv.i4
					IL_02ad: conv.i8
					IL_02ae: ldc.i8 376
					IL_02b7: mul
					IL_02b8: stloc.s 11
					IL_02ba: ldstr "*(BinaryExpr + 0x58)"
					IL_02bf: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_02c4: conv.i4
					IL_02c5: conv.i8
					IL_02c6: stloc.s 32
					IL_02c8: ldarg.0
					IL_02c9: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<copyOfVertices>5__3'
					IL_02ce: stloc.s 33
					IL_02d0: ldloc.s 32
					IL_02d2: ldloc.s 32
					IL_02d4: ldc.i8 4
					IL_02dd: mul
					IL_02de: add
					IL_02df: ldloc.s 32
					IL_02e1: ldloc.s 32
					IL_02e3: ldc.i8 4
					IL_02ec: mul
					IL_02ed: add
					IL_02ee: add
					IL_02ef: stloc.s 11
					IL_02f1: ldstr "*(LocalExpr + 0x30)"
					IL_02f6: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_02fb: stloc.s 34
					IL_02fd: ldloc.s 22
					IL_02ff: conv.i4
					IL_0300: conv.i8
					IL_0301: ldc.i8 376
					IL_030a: mul
					IL_030b: stloc.s 11
					IL_030d: ldstr "*(FieldExpr + 0x6C)"
					IL_0312: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0317: conv.i4
					IL_0318: conv.i8
					IL_0319: stloc.s 35
					IL_031b: ldloc.s 35
					IL_031d: ldloc.s 35
					IL_031f: ldc.i8 2
					IL_0328: mul
					IL_0329: add
					IL_032a: stloc.s 11
					IL_032c: ldstr "*(LocalExpr + 0x28)"
					IL_0331: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0336: conv.i4
					IL_0337: conv.r4
					IL_0338: ldloc.s 29
					IL_033a: sub
					IL_033b: stloc.s 36
					IL_033d: ldloc.s 33
					IL_033f: ldloc.s 32
					IL_0341: conv.i4
					IL_0342: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_0347: stloc.s 37
					IL_0349: ldstr "*(LocalExpr + 0x20)"
					IL_034e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0353: conv.i4
					IL_0354: conv.r4
					IL_0355: ldloc.s 27
					IL_0357: sub
					IL_0358: stloc.s 38
					IL_035a: ldstr "*(LocalExpr + 0x24)"
					IL_035f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0364: conv.i4
					IL_0365: conv.r4
					IL_0366: ldloc.s 28
					IL_0368: sub
					IL_0369: stloc.s 39
					IL_036b: ldstr "movss dword ptr [r8+rcx*4+20h],xmm0 ; value = LocalExpr"
					IL_0370: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0375: ldstr "movss dword ptr [r8+rcx*4+24h],xmm1 ; value = LocalExpr"
					IL_037a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_037f: ldstr "movss dword ptr [r8+rcx*4+28h],xmm2 ; value = LocalExpr"
					IL_0384: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0389: ldloc.s 35
					IL_038b: ldloc.s 35
					IL_038d: ldc.i8 2
					IL_0396: mul
					IL_0397: add
					IL_0398: stloc.s 11
					IL_039a: ldarg.0
					IL_039b: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<copyOfVertices>5__3'
					IL_03a0: stloc.s 40
					IL_03a2: ldloc.s 35
					IL_03a4: ldc.i8 1
					IL_03ad: add
					IL_03ae: ldloc.s 35
					IL_03b0: ldc.i8 1
					IL_03b9: add
					IL_03ba: ldc.i8 2
					IL_03c3: mul
					IL_03c4: add
					IL_03c5: stloc.s 11
					IL_03c7: ldstr "*(LocalExpr + 0x28)"
					IL_03cc: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_03d1: conv.i4
					IL_03d2: conv.r4
					IL_03d3: ldloc.s 29
					IL_03d5: sub
					IL_03d6: stloc.s 36
					IL_03d8: ldloc.s 40
					IL_03da: ldloc.s 32
					IL_03dc: conv.i4
					IL_03dd: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_03e2: stloc.s 41
					IL_03e4: ldstr "*(LocalExpr + 0x20)"
					IL_03e9: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_03ee: conv.i4
					IL_03ef: conv.r4
					IL_03f0: ldloc.s 27
					IL_03f2: sub
					IL_03f3: stloc.s 38
					IL_03f5: ldstr "*(LocalExpr + 0x24)"
					IL_03fa: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_03ff: conv.i4
					IL_0400: conv.r4
					IL_0401: ldloc.s 28
					IL_0403: sub
					IL_0404: stloc.s 39
					IL_0406: ldstr "movss dword ptr [r8+rax*4+20h],xmm0 ; value = LocalExpr"
					IL_040b: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0410: ldstr "movss dword ptr [r8+rax*4+24h],xmm1 ; value = LocalExpr"
					IL_0415: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_041a: ldstr "movss dword ptr [r8+rax*4+28h],xmm2 ; value = LocalExpr"
					IL_041f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0424: ldarg.0
					IL_0425: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<copyOfVertices>5__3'
					IL_042a: stloc.s 42
					IL_042c: ldloc.s 35
					IL_042e: ldc.i8 2
					IL_0437: add
					IL_0438: ldloc.s 35
					IL_043a: ldc.i8 2
					IL_0443: add
					IL_0444: ldc.i8 2
					IL_044d: mul
					IL_044e: add
					IL_044f: stloc.s 11
					IL_0451: ldstr "*(LocalExpr + 0x28)"
					IL_0456: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_045b: conv.i4
					IL_045c: conv.r4
					IL_045d: ldloc.s 29
					IL_045f: sub
					IL_0460: stloc.s 36
					IL_0462: ldloc.s 42
					IL_0464: ldloc.s 32
					IL_0466: conv.i4
					IL_0467: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_046c: stloc.s 43
					IL_046e: ldstr "*(LocalExpr + 0x20)"
					IL_0473: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0478: conv.i4
					IL_0479: conv.r4
					IL_047a: ldloc.s 27
					IL_047c: sub
					IL_047d: stloc.s 38
					IL_047f: ldstr "*(LocalExpr + 0x24)"
					IL_0484: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0489: conv.i4
					IL_048a: conv.r4
					IL_048b: ldloc.s 28
					IL_048d: sub
					IL_048e: stloc.s 39
					IL_0490: ldstr "movss dword ptr [r8+rax*4+20h],xmm0 ; value = LocalExpr"
					IL_0495: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_049a: ldstr "movss dword ptr [r8+rax*4+24h],xmm1 ; value = LocalExpr"
					IL_049f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_04a4: ldstr "movss dword ptr [r8+rax*4+28h],xmm2 ; value = LocalExpr"
					IL_04a9: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_04ae: ldarg.0
					IL_04af: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<copyOfVertices>5__3'
					IL_04b4: stloc.s 44
					IL_04b6: ldloc.s 35
					IL_04b8: ldc.i8 3
					IL_04c1: add
					IL_04c2: ldloc.s 35
					IL_04c4: ldc.i8 3
					IL_04cd: add
					IL_04ce: ldc.i8 2
					IL_04d7: mul
					IL_04d8: add
					IL_04d9: stloc.s 11
					IL_04db: ldstr "*(LocalExpr + 0x28)"
					IL_04e0: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_04e5: conv.i4
					IL_04e6: conv.r4
					IL_04e7: ldloc.s 29
					IL_04e9: sub
					IL_04ea: stloc.s 36
					IL_04ec: ldloc.s 44
					IL_04ee: ldloc.s 32
					IL_04f0: conv.i4
					IL_04f1: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_04f6: stloc.s 45
					IL_04f8: ldstr "*(LocalExpr + 0x20)"
					IL_04fd: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0502: conv.i4
					IL_0503: conv.r4
					IL_0504: ldloc.s 27
					IL_0506: sub
					IL_0507: stloc.s 38
					IL_0509: ldstr "*(LocalExpr + 0x24)"
					IL_050e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0513: conv.i4
					IL_0514: conv.r4
					IL_0515: ldloc.s 28
					IL_0517: sub
					IL_0518: stloc.s 39
					IL_051a: ldstr "movss dword ptr [r8+rax*4+20h],xmm0 ; value = LocalExpr"
					IL_051f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0524: ldstr "movss dword ptr [r8+rax*4+24h],xmm1 ; value = LocalExpr"
					IL_0529: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_052e: ldstr "movss dword ptr [r8+rax*4+28h],xmm2 ; value = LocalExpr"
					IL_0533: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0538: ldc.r4 0.995
					IL_053d: ldloc.2
					IL_053e: ldfld float32 TMPro.Examples.VertexShakeA::ScaleMultiplier
					IL_0543: ldc.r4 0.001
					IL_0548: mul
					IL_0549: sub
					IL_054a: ldloc.2
					IL_054b: ldfld float32 TMPro.Examples.VertexShakeA::ScaleMultiplier
					IL_0550: ldc.r4 0.001
					IL_0555: mul
					IL_0556: ldc.r4 1.005
					IL_055b: add
					IL_055c: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
					IL_0561: stloc.s 46
					IL_0563: ldstr "global_19E3BFF"
					IL_0568: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_056d: conv.i4
					IL_056e: stloc.s 47
					IL_0570: ldloc.s 46
					IL_0572: stloc.s 48
					IL_0574: ldloc.s 46
					IL_0576: stloc.s 38
					IL_0578: ldstr "global_19E3BFF"
					IL_057d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0582: conv.i4
					IL_0583: ldc.i4.0
					IL_0584: ceq
					IL_0586: ldc.i4.0
					IL_0587: ceq
					IL_0589: brtrue.s IL_0596

					IL_058b: nop
					IL_058c: ldstr "mov [1819E3BFFh],cl ; value = 0x1"
					IL_0591: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)

					IL_0596: nop
					IL_0597: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::oneVector
					IL_059c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
					IL_05a1: stloc.s 49
					IL_05a3: ldstr "struct_chunk(StaticFieldExpr+0x0, 8)"
					IL_05a8: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_05ad: stloc.s 50
					IL_05af: ldc.i4.1
					IL_05b0: ldc.i4.0
					IL_05b1: ceq
					IL_05b3: ldc.i4.0
					IL_05b4: ceq
					IL_05b6: brtrue.s IL_05c3

					IL_05b8: nop
					IL_05b9: ldstr "mov byte ptr [1819E3BFFh],1 ; value = 0x1"
					IL_05be: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)

					IL_05c3: nop
					IL_05c4: ldloc.s 48
					IL_05c6: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::oneVector
					IL_05cb: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
					IL_05d0: mul
					IL_05d1: stloc.s 51
					IL_05d3: ldloca.s 52
					IL_05d5: ldloc.s 50
					IL_05d7: box [mscorlib]System.Int64
					IL_05dc: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
					IL_05e1: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
					IL_05e6: ldloca.s 52
					IL_05e8: ldloc.s 49
					IL_05ea: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
					IL_05ef: ldloc.s 52
					IL_05f1: ldstr "&stack[0xFFFFFFFFFFFFFEA8]"
					IL_05f6: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_05fb: conv.i8
					IL_05fc: ldc.i8 -32
					IL_0605: add
					IL_0606: box [mscorlib]System.Int64
					IL_060b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion>(object)
					IL_0610: ldloca.s 53
					IL_0612: ldstr "unpcklps xmm1,xmm0"
					IL_0617: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_061c: box [mscorlib]System.Int64
					IL_0621: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
					IL_0626: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
					IL_062b: ldloca.s 53
					IL_062d: ldloc.s 48
					IL_062f: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::oneVector
					IL_0634: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
					IL_0639: mul
					IL_063a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
					IL_063f: ldloc.s 53
					IL_0641: call valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::TRS(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
					IL_0646: stloc.s 54
					IL_0648: ldarg.0
					IL_0649: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<copyOfVertices>5__3'
					IL_064e: stloc.s 55
					IL_0650: ldstr "struct_chunk(LocalExpr+0x20, 16)"
					IL_0655: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_065a: stloc.s 56
					IL_065c: ldstr "struct_chunk(LocalExpr+0x30, 16)"
					IL_0661: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0666: stloc.s 57
					IL_0668: ldloc.s 55
					IL_066a: ldloc.s 32
					IL_066c: conv.i4
					IL_066d: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_0672: stloc.s 58
					IL_0674: ldstr "&stack[0xFFFFFFFFFFFFFEA8]"
					IL_0679: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_067e: conv.i8
					IL_067f: ldc.i8 -96
					IL_0688: add
					IL_0689: ldloca.s 59
					IL_068b: ldstr "*(ArrayElementExpr + 0x20)"
					IL_0690: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0695: box [mscorlib]System.Int64
					IL_069a: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
					IL_069f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
					IL_06a4: ldloca.s 59
					IL_06a6: ldstr "*(ArrayElementExpr + 0x28)"
					IL_06ab: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_06b0: conv.i4
					IL_06b1: box [mscorlib]System.Int32
					IL_06b6: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
					IL_06bb: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
					IL_06c0: ldloc.s 59
					IL_06c2: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
					IL_06c7: stloc.s 60
					IL_06c9: ldstr "movsd qword ptr [r14+rdx*4+20h],xmm0 ; value = StructChunkExpr"
					IL_06ce: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_06d3: ldstr "mov [r14+rdx*4+28h],r8d ; value = FieldExpr"
					IL_06d8: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_06dd: ldarg.0
					IL_06de: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<copyOfVertices>5__3'
					IL_06e3: stloc.s 61
					IL_06e5: ldstr "struct_chunk(LocalExpr+0x0, 8)"
					IL_06ea: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_06ef: stloc.s 56
					IL_06f1: ldloc.s 61
					IL_06f3: ldloc.s 32
					IL_06f5: conv.i4
					IL_06f6: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_06fb: stloc.s 62
					IL_06fd: ldstr "&stack[0xFFFFFFFFFFFFFEA8]"
					IL_0702: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0707: conv.i8
					IL_0708: ldc.i8 -96
					IL_0711: add
					IL_0712: ldloca.s 63
					IL_0714: ldstr "*(ArrayElementExpr + 0x20)"
					IL_0719: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_071e: box [mscorlib]System.Int64
					IL_0723: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
					IL_0728: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
					IL_072d: ldloca.s 63
					IL_072f: ldstr "*(ArrayElementExpr + 0x28)"
					IL_0734: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0739: conv.i4
					IL_073a: box [mscorlib]System.Int32
					IL_073f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
					IL_0744: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
					IL_0749: ldloc.s 63
					IL_074b: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
					IL_0750: stloc.s 64
					IL_0752: ldstr "movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr"
					IL_0757: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_075c: ldstr "mov [r14+rcx*4+28h],edx ; value = FieldExpr"
					IL_0761: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0766: ldarg.0
					IL_0767: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<copyOfVertices>5__3'
					IL_076c: stloc.s 61
					IL_076e: ldstr "struct_chunk(LocalExpr+0x0, 8)"
					IL_0773: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0778: stloc.s 56
					IL_077a: ldloc.s 61
					IL_077c: ldloc.s 32
					IL_077e: conv.i4
					IL_077f: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_0784: stloc.s 65
					IL_0786: ldstr "&stack[0xFFFFFFFFFFFFFEA8]"
					IL_078b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0790: conv.i8
					IL_0791: ldc.i8 -96
					IL_079a: add
					IL_079b: ldstr "&stack[0xFFFFFFFFFFFFFEA8]"
					IL_07a0: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_07a5: conv.i8
					IL_07a6: ldc.i8 -128
					IL_07af: add
					IL_07b0: box [mscorlib]System.Int64
					IL_07b5: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
					IL_07ba: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
					IL_07bf: stloc.s 66
					IL_07c1: ldstr "movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr"
					IL_07c6: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_07cb: ldstr "mov [r14+rcx*4+28h],edx ; value = FieldExpr"
					IL_07d0: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_07d5: ldarg.0
					IL_07d6: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<copyOfVertices>5__3'
					IL_07db: stloc.s 61
					IL_07dd: ldstr "struct_chunk(LocalExpr+0x0, 8)"
					IL_07e2: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_07e7: stloc.s 56
					IL_07e9: ldloc.s 61
					IL_07eb: ldloc.s 32
					IL_07ed: conv.i4
					IL_07ee: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_07f3: stloc.s 67
					IL_07f5: ldstr "&stack[0xFFFFFFFFFFFFFEA8]"
					IL_07fa: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_07ff: conv.i8
					IL_0800: ldc.i8 -96
					IL_0809: add
					IL_080a: ldstr "&stack[0xFFFFFFFFFFFFFEA8]"
					IL_080f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0814: conv.i8
					IL_0815: ldc.i8 -112
					IL_081e: add
					IL_081f: box [mscorlib]System.Int64
					IL_0824: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
					IL_0829: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
					IL_082e: stloc.s 68
					IL_0830: ldstr "movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr"
					IL_0835: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_083a: ldstr "mov [r14+rcx*4+28h],edx ; value = FieldExpr"
					IL_083f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0844: ldstr "struct_chunk(LocalExpr+0x0, 8)"
					IL_0849: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_084e: stloc.s 56
					IL_0850: ldarg.0
					IL_0851: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<copyOfVertices>5__3'
					IL_0856: ldloc.s 32
					IL_0858: conv.i4
					IL_0859: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_085e: stloc.s 69
					IL_0860: ldstr "movss dword ptr [rdx+rcx*4+20h],xmm0 ; value = BinaryExpr"
					IL_0865: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_086a: ldstr "movss dword ptr [rdx+rcx*4+28h],xmm1 ; value = BinaryExpr"
					IL_086f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0874: ldstr "movss dword ptr [rdx+rcx*4+24h],xmm0 ; value = BinaryExpr"
					IL_0879: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_087e: ldloc.s 28
					IL_0880: ldstr "*(LocalExpr + 0x24)"
					IL_0885: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_088a: conv.i4
					IL_088b: conv.r4
					IL_088c: add
					IL_088d: stloc.s 70
					IL_088f: ldloc.s 29
					IL_0891: ldstr "*(LocalExpr + 0x28)"
					IL_0896: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_089b: conv.i4
					IL_089c: conv.r4
					IL_089d: add
					IL_089e: stloc.s 71
					IL_08a0: ldarg.0
					IL_08a1: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<copyOfVertices>5__3'
					IL_08a6: ldloc.s 32
					IL_08a8: conv.i4
					IL_08a9: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_08ae: stloc.s 72
					IL_08b0: ldstr "movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr"
					IL_08b5: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_08ba: ldstr "movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr"
					IL_08bf: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_08c4: ldstr "movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr"
					IL_08c9: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_08ce: ldloc.s 28
					IL_08d0: ldstr "*(LocalExpr + 0x24)"
					IL_08d5: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_08da: conv.i4
					IL_08db: conv.r4
					IL_08dc: add
					IL_08dd: stloc.s 70
					IL_08df: ldloc.s 29
					IL_08e1: ldstr "*(LocalExpr + 0x28)"
					IL_08e6: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_08eb: conv.i4
					IL_08ec: conv.r4
					IL_08ed: add
					IL_08ee: stloc.s 71
					IL_08f0: ldarg.0
					IL_08f1: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<copyOfVertices>5__3'
					IL_08f6: ldloc.s 32
					IL_08f8: conv.i4
					IL_08f9: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_08fe: stloc.s 73
					IL_0900: ldstr "movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr"
					IL_0905: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_090a: ldstr "movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr"
					IL_090f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0914: ldstr "movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr"
					IL_0919: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_091e: ldloc.s 28
					IL_0920: ldstr "*(LocalExpr + 0x24)"
					IL_0925: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_092a: conv.i4
					IL_092b: conv.r4
					IL_092c: add
					IL_092d: stloc.s 70
					IL_092f: ldloc.s 29
					IL_0931: ldstr "*(LocalExpr + 0x28)"
					IL_0936: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_093b: conv.i4
					IL_093c: conv.r4
					IL_093d: add
					IL_093e: stloc.s 71
					IL_0940: ldarg.0
					IL_0941: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<copyOfVertices>5__3'
					IL_0946: ldloc.s 32
					IL_0948: conv.i4
					IL_0949: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_094e: stloc.s 74
					IL_0950: ldstr "movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr"
					IL_0955: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_095a: ldstr "movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr"
					IL_095f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0964: ldstr "movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr"
					IL_0969: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_096e: ldloc.s 28
					IL_0970: ldstr "*(LocalExpr + 0x24)"
					IL_0975: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_097a: conv.i4
					IL_097b: conv.r4
					IL_097c: add
					IL_097d: stloc.s 70
					IL_097f: ldloc.s 29
					IL_0981: ldstr "*(LocalExpr + 0x28)"
					IL_0986: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_098b: conv.i4
					IL_098c: conv.r4
					IL_098d: add
					IL_098e: stloc.s 71

					IL_0990: nop
					IL_0991: ldloc.s 22
					IL_0993: conv.i4
					IL_0994: ldc.i4.1
					IL_0995: add
					IL_0996: stloc.s 75
					IL_0998: ldloc.s 22
					IL_099a: conv.i4
					IL_099b: ldc.i4.1
					IL_099c: add
					IL_099d: ldloc.s 23
					IL_099f: conv.i4
					IL_09a0: cgt
					IL_09a2: ldc.i4.0
					IL_09a3: ceq
					IL_09a5: brtrue IL_0288
				// end loop

				IL_09aa: nop
				IL_09ab: ldstr "stack[0x8]"
				IL_09b0: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_09b5: conv.i4
				IL_09b6: ldc.i4.1
				IL_09b7: add
				IL_09b8: stloc.s 76
				IL_09ba: ldstr "stack[0x8]"
				IL_09bf: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_09c4: conv.i4
				IL_09c5: ldc.i4.1
				IL_09c6: add
				IL_09c7: ldstr "stack[0x18]"
				IL_09cc: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_09d1: conv.i4
				IL_09d2: clt
				IL_09d4: brtrue IL_01a5
			// end loop

			IL_09d9: nop
			IL_09da: ldarg.0
			IL_09db: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
			IL_09e0: stloc.s 18
			// loop start (head: IL_09e2)
				IL_09e2: nop
				IL_09e3: ldc.i4.0
				IL_09e4: ldloc.s 18
				IL_09e6: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
				IL_09eb: ldlen
				IL_09ec: conv.i4
				IL_09ed: clt
				IL_09ef: ldc.i4.0
				IL_09f0: ceq
				IL_09f2: brtrue.s IL_0a65

				IL_09f4: nop
				IL_09f5: ldarg.0
				IL_09f6: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
				IL_09fb: stloc.s 77
				IL_09fd: ldloc.s 77
				IL_09ff: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
				IL_0a04: stloc.s 78
				IL_0a06: ldarg.0
				IL_0a07: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<copyOfVertices>5__3'
				IL_0a0c: stloc.s 79
				IL_0a0e: ldstr "*(LocalExpr + 0x20)"
				IL_0a13: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0a18: stloc.s 80
				IL_0a1a: ldc.i4.0
				IL_0a1b: conv.i8
				IL_0a1c: stloc.s 81
				IL_0a1e: ldloc.s 80
				IL_0a20: box [mscorlib]System.Int64
				IL_0a25: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [UnityEngine.CoreModule]UnityEngine.Mesh>(object)
				IL_0a2a: ldloc.s 79
				IL_0a2c: ldc.i4.0
				IL_0a2d: conv.i8
				IL_0a2e: conv.i4
				IL_0a2f: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
				IL_0a34: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Mesh::set_vertices(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[])
				IL_0a39: ldloc.2
				IL_0a3a: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.VertexShakeA::m_TextComponent
				IL_0a3f: stloc.s 82
				IL_0a41: ldc.i4.0
				IL_0a42: ldc.i4.1
				IL_0a43: add
				IL_0a44: stloc.s 83
				IL_0a46: ldarg.0
				IL_0a47: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
				IL_0a4c: stloc.s 84
				IL_0a4e: ldc.i4.0
				IL_0a4f: ldc.i4.1
				IL_0a50: add
				IL_0a51: stloc.s 85
				IL_0a53: ldarg.0
				IL_0a54: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<textInfo>5__2'
				IL_0a59: ldnull
				IL_0a5a: ceq
				IL_0a5c: ldc.i4.0
				IL_0a5d: ceq
				IL_0a5f: brtrue IL_09e2
			// end loop
			IL_0a64: nop

			IL_0a65: nop
			IL_0a66: ldc.r4 0.1
			IL_0a6b: newobj instance void [UnityEngine.CoreModule]UnityEngine.WaitForSeconds::.ctor(float32)
			IL_0a70: stloc.s 86
			IL_0a72: ldarg.0
			IL_0a73: ldloc.s 86
			IL_0a75: stfld object TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<>2__current'
			IL_0a7a: ldarg.0
			IL_0a7b: ldc.i4.2
			IL_0a7c: stfld int32 TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<>1__state'

			IL_0a81: nop

			IL_0a82: nop
			IL_0a83: ldc.i4.1
			IL_0a84: ret

			IL_0a85: nop
			IL_0a86: ldc.r4 0.25
			IL_0a8b: newobj instance void [UnityEngine.CoreModule]UnityEngine.WaitForSeconds::.ctor(float32)
			IL_0a90: stloc.s 87
			IL_0a92: ldarg.0
			IL_0a93: ldloc.s 87
			IL_0a95: stfld object TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<>2__current'
			IL_0a9a: ldarg.0
			IL_0a9b: ldc.i4.1
			IL_0a9c: stfld int32 TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<>1__state'
			IL_0aa1: br.s IL_0a81

			IL_0aa3: nop
			IL_0aa4: ldnull
			IL_0aa5: throw

			IL_0aa6: nop
			IL_0aa7: ldnull
			IL_0aa8: throw
		} // end of method '<AnimateVertexColors>d__11'::MoveNext

		.method private final hidebysig specialname newslot virtual 
			instance object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () cil managed 
		{
			// Method begins at RVA 0x2f18
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<>2__current'
			IL_0007: ret
		} // end of method '<AnimateVertexColors>d__11'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'

		.method private final hidebysig newslot virtual 
			instance void System.Collections.IEnumerator.Reset () cil managed 
		{
			// Method begins at RVA 0x1eaf0
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
			// Method begins at RVA 0x2f21
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<>2__current'
			IL_0007: ret
		} // end of method '<AnimateVertexColors>d__11'::System.Collections.IEnumerator.get_Current

		// Properties
		.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current'()
		{
			.get instance object TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'()
		}
		.property instance object System.Collections.IEnumerator.Current()
		{
			.get instance object TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::System.Collections.IEnumerator.get_Current()
		}

	} // end of class <AnimateVertexColors>d__11


	// Fields
	.field public float32 AngleMultiplier
	.field public float32 SpeedMultiplier
	.field public float32 ScaleMultiplier
	.field public float32 RotationMultiplier
	.field private class [Unity.TextMeshPro]TMPro.TMP_Text m_TextComponent
	.field private bool hasTextChanged

	// Methods
	.method private hidebysig 
		instance void Awake () cil managed 
	{
		// Method begins at RVA 0x1973c
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
		IL_000a: stfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.VertexShakeA::m_TextComponent
		IL_000f: ret
	} // end of method VertexShakeA::Awake

	.method private hidebysig 
		instance void OnEnable () cil managed 
	{
		// Method begins at RVA 0x19758
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
		IL_000c: ldftn instance void TMPro.Examples.VertexShakeA::ON_TEXT_CHANGED(class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0012: newobj instance void [System]System.IOAsyncCallback::.ctor(object, native int)
		IL_0017: stloc.1
		IL_0018: ldloc.0
		IL_0019: ldloc.1
		IL_001a: callvirt instance void class [Unity.TextMeshPro]TMPro.FastAction`1<class [UnityEngine.CoreModule]UnityEngine.Object>::Add(class [mscorlib]System.Action`1<!0>)
		IL_001f: ret

		IL_0020: nop
		IL_0021: ldnull
		IL_0022: throw
	} // end of method VertexShakeA::OnEnable

	.method private hidebysig 
		instance void OnDisable () cil managed 
	{
		// Method begins at RVA 0x19788
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
		IL_000c: ldftn instance void TMPro.Examples.VertexShakeA::ON_TEXT_CHANGED(class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0012: newobj instance void [System]System.IOAsyncCallback::.ctor(object, native int)
		IL_0017: stloc.1
		IL_0018: ldloc.0
		IL_0019: ldloc.1
		IL_001a: callvirt instance void class [Unity.TextMeshPro]TMPro.FastAction`1<class [UnityEngine.CoreModule]UnityEngine.Object>::Remove(class [mscorlib]System.Action`1<!0>)
		IL_001f: ret

		IL_0020: nop
		IL_0021: ldnull
		IL_0022: throw
	} // end of method VertexShakeA::OnDisable

	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x197b8
		// Header size: 12
		// Code size: 47 (0x2f)
		.maxstack 2
		.locals init (
			[0] class TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11',
			[1] class [UnityEngine.CoreModule]UnityEngine.Coroutine
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(TMPro.Examples.VertexShakeA+<AnimateVertexColors>d__11)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class TMPro.Examples.VertexShakeA TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<>4__this'
		IL_0026: ldarg.0
		IL_0027: ldloc.0
		IL_0028: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Coroutine [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StartCoroutine(class [mscorlib]System.Collections.IEnumerator)
		IL_002d: stloc.1
		IL_002e: ret
	} // end of method VertexShakeA::Start

	.method private hidebysig 
		instance void ON_TEXT_CHANGED (
			class [UnityEngine.CoreModule]UnityEngine.Object obj
		) cil managed 
	{
		// Method begins at RVA 0x197f4
		// Header size: 12
		// Code size: 37 (0x25)
		.maxstack 2
		.locals init (
			[0] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[1] bool,
			[2] bool
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.VertexShakeA::m_TextComponent
		IL_0007: stloc.0
		IL_0008: br.s IL_000b

		IL_000a: nop

		IL_000b: nop
		IL_000c: ldloc.0
		IL_000d: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Implicit(class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0012: stloc.1
		IL_0013: ldloc.1
		IL_0014: stloc.2
		IL_0015: ldloc.1
		IL_0016: ldc.i4.0
		IL_0017: ceq
		IL_0019: brtrue.s IL_0023

		IL_001b: nop
		IL_001c: ldarg.0
		IL_001d: ldc.i4.1
		IL_001e: stfld bool TMPro.Examples.VertexShakeA::hasTextChanged

		IL_0023: nop
		IL_0024: ret
	} // end of method VertexShakeA::ON_TEXT_CHANGED

	.method private hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator AnimateVertexColors () cil managed 
	{
		// Method begins at RVA 0x19828
		// Header size: 12
		// Code size: 40 (0x28)
		.maxstack 2
		.locals init (
			[0] class TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(TMPro.Examples.VertexShakeA+<AnimateVertexColors>d__11)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class TMPro.Examples.VertexShakeA TMPro.Examples.VertexShakeA/'<AnimateVertexColors>d__11'::'<>4__this'
		IL_0026: ldloc.0
		IL_0027: ret
	} // end of method VertexShakeA::AnimateVertexColors

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2aa4
		// Header size: 1
		// Code size: 52 (0x34)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.r4 1
		IL_0007: stfld float32 TMPro.Examples.VertexShakeA::AngleMultiplier
		IL_000c: ldarg.0
		IL_000d: ldc.r4 1
		IL_0012: stfld float32 TMPro.Examples.VertexShakeA::SpeedMultiplier
		IL_0017: ldarg.0
		IL_0018: ldc.r4 1
		IL_001d: stfld float32 TMPro.Examples.VertexShakeA::ScaleMultiplier
		IL_0022: ldarg.0
		IL_0023: ldc.r4 1
		IL_0028: stfld float32 TMPro.Examples.VertexShakeA::RotationMultiplier
		IL_002d: ldarg.0
		IL_002e: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0033: ret
	} // end of method VertexShakeA::.ctor

} // end of class TMPro.Examples.VertexShakeA
