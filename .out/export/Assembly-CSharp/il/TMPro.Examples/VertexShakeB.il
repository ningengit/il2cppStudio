.class public auto ansi beforefieldinit TMPro.Examples.VertexShakeB
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested private auto ansi sealed beforefieldinit '<AnimateVertexColors>d__10'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerator`1<object>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private object '<>2__current'
		.field public class TMPro.Examples.VertexShakeB '<>4__this'
		.field private class [Unity.TextMeshPro]TMPro.TMP_TextInfo '<textInfo>5__2'
		.field private valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] '<copyOfVertices>5__3'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x2f2a
			// Header size: 1
			// Code size: 16 (0x10)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ldarg.0
			IL_0009: ldarg.1
			IL_000a: stfld int32 TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<>1__state'
			IL_000f: ret
		} // end of method '<AnimateVertexColors>d__10'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			// Method begins at RVA 0x2f3b
			// Header size: 1
			// Code size: 2 (0x2)
			.maxstack 8

			IL_0000: nop
			IL_0001: ret
		} // end of method '<AnimateVertexColors>d__10'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			// Method begins at RVA 0x1eb40
			// Header size: 12
			// Code size: 5773 (0x168d)
			.maxstack 4
			.locals init (
				[0] int32,
				[1] class TMPro.Examples.VertexShakeB,
				[2] class TMPro.Examples.VertexShakeB,
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
				[20] int64,
				[21] int32,
				[22] valuetype [Unity.TextMeshPro]TMPro.TMP_CharacterInfo[],
				[23] int64,
				[24] valuetype [Unity.TextMeshPro]TMPro.TMP_CharacterInfo[],
				[25] int64,
				[26] float32,
				[27] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[28] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
				[29] int64,
				[30] float32,
				[31] float32,
				[32] float32,
				[33] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[],
				[34] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[35] int64,
				[36] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[37] int64,
				[38] float32,
				[39] float32,
				[40] float32,
				[41] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][],
				[42] float32,
				[43] float32,
				[44] float32,
				[45] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[46] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][],
				[47] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[48] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][],
				[49] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[50] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][],
				[51] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[52] float32,
				[53] float32,
				[54] int32,
				[55] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
				[56] float32,
				[57] int64,
				[58] float32,
				[59] float32,
				[60] float32,
				[61] valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4,
				[62] int64,
				[63] int64,
				[64] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[65] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[66] valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4,
				[67] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[68] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][],
				[69] int64,
				[70] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[71] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[72] valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4,
				[73] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[74] float32,
				[75] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[76] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[77] valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4,
				[78] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[79] float32,
				[80] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[81] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[82] valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4,
				[83] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[84] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][],
				[85] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[86] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][],
				[87] float32,
				[88] float32,
				[89] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[90] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][],
				[91] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[92] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][],
				[93] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[94] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][],
				[95] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[96] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][],
				[97] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[98] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][],
				[99] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[100] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][],
				[101] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[102] int32,
				[103] int64,
				[104] float32,
				[105] valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4,
				[106] int64,
				[107] int64,
				[108] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[109] valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4,
				[110] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[111] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[112] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[113] valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4,
				[114] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[115] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[116] valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4,
				[117] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[118] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[119] valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4,
				[120] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[121] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[122] float32,
				[123] float32,
				[124] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[125] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[126] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[],
				[127] int32,
				[128] int32,
				[129] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[130] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[],
				[131] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][],
				[132] int64,
				[133] int64,
				[134] int32,
				[135] int64,
				[136] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[137] int32,
				[138] class [UnityEngine.CoreModule]UnityEngine.WaitForSeconds,
				[139] class [UnityEngine.CoreModule]UnityEngine.WaitForSeconds
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld int32 TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<>1__state'
			IL_0007: stloc.0
			IL_0008: ldarg.0
			IL_0009: ldfld class TMPro.Examples.VertexShakeB TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<>4__this'
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
			IL_002e: br IL_1666

			IL_0033: nop
			IL_0034: ldarg.0
			IL_0035: ldc.i4.m1
			IL_0036: stfld int32 TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<>1__state'
			IL_003b: br.s IL_0075

			IL_003d: nop
			IL_003e: ldarg.0
			IL_003f: ldc.i4.m1
			IL_0040: stfld int32 TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<>1__state'
			IL_0045: ldloc.2
			IL_0046: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.VertexShakeB::m_TextComponent
			IL_004b: stloc.s 4
			IL_004d: ldloc.s 4
			IL_004f: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo [Unity.TextMeshPro]TMPro.TMP_Text::m_textInfo
			IL_0054: stloc.s 5
			IL_0056: ldarg.0
			IL_0057: ldloc.s 5
			IL_0059: stfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
			IL_005e: ldc.i4.0
			IL_005f: newarr valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
			IL_0064: stloc.s 6
			IL_0066: ldarg.0
			IL_0067: ldloc.s 6
			IL_0069: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
			IL_006e: ldloc.2
			IL_006f: ldc.i4.1
			IL_0070: stfld bool TMPro.Examples.VertexShakeB::hasTextChanged

			IL_0075: nop
			IL_0076: ldloc.2
			IL_0077: ldfld bool TMPro.Examples.VertexShakeB::hasTextChanged
			IL_007c: ldc.i4.0
			IL_007d: ceq
			IL_007f: brtrue IL_0184

			IL_0084: nop
			IL_0085: ldarg.0
			IL_0086: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
			IL_008b: ldlen
			IL_008c: conv.i4
			IL_008d: ldarg.0
			IL_008e: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
			IL_0093: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
			IL_0098: ldlen
			IL_0099: conv.i4
			IL_009a: clt
			IL_009c: ldc.i4.0
			IL_009d: ceq
			IL_009f: brtrue.s IL_00be

			IL_00a1: nop
			IL_00a2: ldarg.0
			IL_00a3: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
			IL_00a8: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
			IL_00ad: ldlen
			IL_00ae: conv.i4
			IL_00af: newarr valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
			IL_00b4: stloc.s 7
			IL_00b6: ldarg.0
			IL_00b7: ldloc.s 7
			IL_00b9: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'

			IL_00be: nop
			IL_00bf: ldarg.0
			IL_00c0: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
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
				IL_00de: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
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
				IL_0136: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
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
				IL_014f: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
				IL_0154: stloc.s 18
				IL_0156: ldc.i4.0
				IL_0157: ldc.i4.1
				IL_0158: add
				IL_0159: stloc.s 19
				IL_015b: ldarg.0
				IL_015c: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
				IL_0161: ldnull
				IL_0162: ceq
				IL_0164: ldc.i4.0
				IL_0165: ceq
				IL_0167: brtrue IL_00c7
			// end loop
			IL_016c: nop

			IL_016d: nop
			IL_016e: ldstr "mov byte ptr [rdi+38h],0 ; value = 0x0"
			IL_0173: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
			IL_0178: ldstr "stack[0x18]"
			IL_017d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0182: stloc.s 20

			IL_0184: nop
			IL_0185: ldarg.0
			IL_0186: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
			IL_018b: stloc.s 18
			IL_018d: ldloc.s 18
			IL_018f: ldfld int32 [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterCount
			IL_0194: ldc.i4.0
			IL_0195: ceq
			IL_0197: brtrue IL_1669

			IL_019c: nop
			IL_019d: ldloc.s 18
			IL_019f: ldfld int32 [Unity.TextMeshPro]TMPro.TMP_TextInfo::lineCount
			IL_01a4: stloc.s 21
			IL_01a6: ldloc.s 21
			IL_01a8: ldc.i4.0
			IL_01a9: cgt
			IL_01ab: ldc.i4.0
			IL_01ac: ceq
			IL_01ae: brtrue IL_15aa

			IL_01b3: nop
			// loop start (head: IL_01b4)
				IL_01b4: nop
				IL_01b5: ldarg.0
				IL_01b6: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
				IL_01bb: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_CharacterInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterInfo
				IL_01c0: stloc.s 22
				IL_01c2: ldstr "*(FieldExpr + 0x34)"
				IL_01c7: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_01cc: conv.i4
				IL_01cd: conv.i8
				IL_01ce: stloc.s 23
				IL_01d0: ldarg.0
				IL_01d1: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
				IL_01d6: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_CharacterInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterInfo
				IL_01db: stloc.s 24
				IL_01dd: ldstr "*(BinaryExpr + 0x3C)"
				IL_01e2: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_01e7: conv.i4
				IL_01e8: conv.i8
				IL_01e9: stloc.s 25
				IL_01eb: ldc.r4 -0.25
				IL_01f0: ldc.r4 0.25
				IL_01f5: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
				IL_01fa: stloc.s 26
				IL_01fc: ldloca.s 27
				IL_01fe: ldc.r4 0.0
				IL_0203: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
				IL_0208: ldloca.s 27
				IL_020a: ldloc.s 26
				IL_020c: ldc.r4 0.017453292
				IL_0211: mul
				IL_0212: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
				IL_0217: ldloc.s 27
				IL_0219: call valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_FromEulerRad(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
				IL_021e: stloc.s 28
				IL_0220: ldstr "struct_chunk(LocalExpr+0x0, 16)"
				IL_0225: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_022a: stloc.s 29
				IL_022c: ldstr "*(LocalExpr + 0x128)"
				IL_0231: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0236: conv.i4
				IL_0237: conv.r4
				IL_0238: ldstr "*(LocalExpr + 0x11C)"
				IL_023d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0242: conv.i4
				IL_0243: conv.r4
				IL_0244: add
				IL_0245: ldc.r4 0.5
				IL_024a: mul
				IL_024b: stloc.s 30
				IL_024d: ldstr "*(LocalExpr + 0x130)"
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
				IL_026c: stloc.s 31
				IL_026e: ldstr "*(LocalExpr + 0x12C)"
				IL_0273: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0278: conv.i4
				IL_0279: conv.r4
				IL_027a: ldstr "*(LocalExpr + 0x120)"
				IL_027f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_0284: conv.i4
				IL_0285: conv.r4
				IL_0286: add
				IL_0287: ldc.r4 0.5
				IL_028c: mul
				IL_028d: stloc.s 32
				IL_028f: ldloc.s 23
				IL_0291: conv.i4
				IL_0292: ldloc.s 25
				IL_0294: conv.i4
				IL_0295: cgt
				IL_0297: brtrue IL_1582

				IL_029c: nop
				// loop start (head: IL_029d)
					IL_029d: nop
					IL_029e: ldstr "*(FieldExpr + 0x194)"
					IL_02a3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_02a8: ldc.i4.0
					IL_02a9: conv.i8
					IL_02aa: ceq
					IL_02ac: brtrue IL_1568

					IL_02b1: nop
					IL_02b2: ldarg.0
					IL_02b3: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
					IL_02b8: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
					IL_02bd: stloc.s 33
					IL_02bf: ldarg.0
					IL_02c0: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
					IL_02c5: stloc.s 34
					IL_02c7: ldstr "*(BinaryExpr + 0x58)"
					IL_02cc: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_02d1: conv.i4
					IL_02d2: conv.i8
					IL_02d3: stloc.s 35
					IL_02d5: ldloc.s 34
					IL_02d7: stloc.s 36
					IL_02d9: ldstr "*(LocalExpr + 0x30)"
					IL_02de: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_02e3: stloc.s 37
					IL_02e5: ldstr "*(FieldExpr + 0x6C)"
					IL_02ea: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_02ef: conv.i4
					IL_02f0: conv.i8
					IL_02f1: stloc.s 20
					IL_02f3: ldstr "*(LocalExpr + 0x20)"
					IL_02f8: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_02fd: conv.i4
					IL_02fe: conv.r4
					IL_02ff: ldstr "*(LocalExpr + 0x20)"
					IL_0304: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0309: conv.i4
					IL_030a: conv.r4
					IL_030b: add
					IL_030c: ldc.r4 0.5
					IL_0311: mul
					IL_0312: stloc.s 38
					IL_0314: ldstr "*(LocalExpr + 0x28)"
					IL_0319: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_031e: conv.i4
					IL_031f: conv.r4
					IL_0320: ldstr "*(LocalExpr + 0x28)"
					IL_0325: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_032a: conv.i4
					IL_032b: conv.r4
					IL_032c: add
					IL_032d: ldc.r4 0.5
					IL_0332: mul
					IL_0333: stloc.s 39
					IL_0335: ldstr "*(LocalExpr + 0x24)"
					IL_033a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_033f: conv.i4
					IL_0340: conv.r4
					IL_0341: ldstr "*(LocalExpr + 0x24)"
					IL_0346: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_034b: conv.i4
					IL_034c: conv.r4
					IL_034d: add
					IL_034e: ldc.r4 0.5
					IL_0353: mul
					IL_0354: stloc.s 40
					IL_0356: ldarg.0
					IL_0357: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_035c: stloc.s 41
					IL_035e: ldstr "*(LocalExpr + 0x28)"
					IL_0363: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0368: conv.i4
					IL_0369: conv.r4
					IL_036a: ldloc.s 39
					IL_036c: sub
					IL_036d: stloc.s 42
					IL_036f: ldstr "*(LocalExpr + 0x20)"
					IL_0374: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0379: conv.i4
					IL_037a: conv.r4
					IL_037b: ldloc.s 38
					IL_037d: sub
					IL_037e: stloc.s 43
					IL_0380: ldstr "*(LocalExpr + 0x24)"
					IL_0385: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_038a: conv.i4
					IL_038b: conv.r4
					IL_038c: ldloc.s 40
					IL_038e: sub
					IL_038f: stloc.s 44
					IL_0391: ldloc.s 41
					IL_0393: ldloc.s 35
					IL_0395: conv.i4
					IL_0396: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_039b: stloc.s 45
					IL_039d: ldstr "movss dword ptr [rdx+rcx*4+20h],xmm0 ; value = LocalExpr"
					IL_03a2: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_03a7: ldstr "movss dword ptr [rdx+rcx*4+24h],xmm1 ; value = LocalExpr"
					IL_03ac: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_03b1: ldstr "movss dword ptr [rdx+rcx*4+28h],xmm2 ; value = LocalExpr"
					IL_03b6: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_03bb: ldarg.0
					IL_03bc: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_03c1: stloc.s 46
					IL_03c3: ldstr "*(LocalExpr + 0x28)"
					IL_03c8: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_03cd: conv.i4
					IL_03ce: conv.r4
					IL_03cf: ldloc.s 39
					IL_03d1: sub
					IL_03d2: stloc.s 42
					IL_03d4: ldstr "*(LocalExpr + 0x20)"
					IL_03d9: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_03de: conv.i4
					IL_03df: conv.r4
					IL_03e0: ldloc.s 38
					IL_03e2: sub
					IL_03e3: stloc.s 43
					IL_03e5: ldstr "*(LocalExpr + 0x24)"
					IL_03ea: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_03ef: conv.i4
					IL_03f0: conv.r4
					IL_03f1: ldloc.s 40
					IL_03f3: sub
					IL_03f4: stloc.s 44
					IL_03f6: ldloc.s 46
					IL_03f8: ldloc.s 35
					IL_03fa: conv.i4
					IL_03fb: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_0400: stloc.s 47
					IL_0402: ldstr "movss dword ptr [rdx+rax*4+20h],xmm0 ; value = LocalExpr"
					IL_0407: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_040c: ldstr "movss dword ptr [rdx+rax*4+24h],xmm1 ; value = LocalExpr"
					IL_0411: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0416: ldstr "movss dword ptr [rdx+rax*4+28h],xmm2 ; value = LocalExpr"
					IL_041b: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0420: ldarg.0
					IL_0421: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_0426: stloc.s 48
					IL_0428: ldstr "*(LocalExpr + 0x28)"
					IL_042d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0432: conv.i4
					IL_0433: conv.r4
					IL_0434: ldloc.s 39
					IL_0436: sub
					IL_0437: stloc.s 42
					IL_0439: ldstr "*(LocalExpr + 0x20)"
					IL_043e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0443: conv.i4
					IL_0444: conv.r4
					IL_0445: ldloc.s 38
					IL_0447: sub
					IL_0448: stloc.s 43
					IL_044a: ldstr "*(LocalExpr + 0x24)"
					IL_044f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0454: conv.i4
					IL_0455: conv.r4
					IL_0456: ldloc.s 40
					IL_0458: sub
					IL_0459: stloc.s 44
					IL_045b: ldloc.s 48
					IL_045d: ldloc.s 35
					IL_045f: conv.i4
					IL_0460: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_0465: stloc.s 49
					IL_0467: ldstr "movss dword ptr [rdx+rax*4+20h],xmm0 ; value = LocalExpr"
					IL_046c: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0471: ldstr "movss dword ptr [rdx+rax*4+24h],xmm1 ; value = LocalExpr"
					IL_0476: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_047b: ldstr "movss dword ptr [rdx+rax*4+28h],xmm2 ; value = LocalExpr"
					IL_0480: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0485: ldarg.0
					IL_0486: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_048b: stloc.s 50
					IL_048d: ldstr "*(LocalExpr + 0x28)"
					IL_0492: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0497: conv.i4
					IL_0498: conv.r4
					IL_0499: ldloc.s 39
					IL_049b: sub
					IL_049c: stloc.s 42
					IL_049e: ldstr "*(LocalExpr + 0x20)"
					IL_04a3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_04a8: conv.i4
					IL_04a9: conv.r4
					IL_04aa: ldloc.s 38
					IL_04ac: sub
					IL_04ad: stloc.s 43
					IL_04af: ldstr "*(LocalExpr + 0x24)"
					IL_04b4: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_04b9: conv.i4
					IL_04ba: conv.r4
					IL_04bb: ldloc.s 40
					IL_04bd: sub
					IL_04be: stloc.s 44
					IL_04c0: ldloc.s 50
					IL_04c2: ldloc.s 35
					IL_04c4: conv.i4
					IL_04c5: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_04ca: stloc.s 51
					IL_04cc: ldstr "movss dword ptr [rdx+rax*4+20h],xmm0 ; value = LocalExpr"
					IL_04d1: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_04d6: ldstr "movss dword ptr [rdx+rax*4+24h],xmm1 ; value = LocalExpr"
					IL_04db: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_04e0: ldstr "movss dword ptr [rdx+rax*4+28h],xmm2 ; value = LocalExpr"
					IL_04e5: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_04ea: ldc.r4 0.95
					IL_04ef: ldc.r4 1.05
					IL_04f4: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
					IL_04f9: stloc.s 52
					IL_04fb: ldloc.s 52
					IL_04fd: stloc.s 53
					IL_04ff: ldloc.s 52
					IL_0501: stloc.s 43
					IL_0503: ldstr "global_19E3BFF"
					IL_0508: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_050d: conv.i4
					IL_050e: stloc.s 54
					IL_0510: ldstr "global_19E3BFF"
					IL_0515: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_051a: conv.i4
					IL_051b: ldc.i4.0
					IL_051c: ceq
					IL_051e: ldc.i4.0
					IL_051f: ceq
					IL_0521: brtrue.s IL_052e

					IL_0523: nop
					IL_0524: ldstr "mov [1819E3BFFh],dl ; value = 0x1"
					IL_0529: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)

					IL_052e: nop
					IL_052f: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Quaternion::identityQuaternion
					IL_0534: stloc.s 55
					IL_0536: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::oneVector
					IL_053b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
					IL_0540: stloc.s 56
					IL_0542: ldstr "struct_chunk(StaticFieldExpr+0x0, 8)"
					IL_0547: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_054c: stloc.s 57
					IL_054e: ldstr "global_19E3BFF"
					IL_0553: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0558: conv.i4
					IL_0559: stloc.s 54
					IL_055b: ldstr "global_19E3BFF"
					IL_0560: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0565: conv.i4
					IL_0566: ldc.i4.0
					IL_0567: ceq
					IL_0569: ldc.i4.0
					IL_056a: ceq
					IL_056c: brtrue.s IL_0579

					IL_056e: nop
					IL_056f: ldstr "mov byte ptr [1819E3BFFh],1 ; value = 0x1"
					IL_0574: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)

					IL_0579: nop
					IL_057a: ldloc.s 53
					IL_057c: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::oneVector
					IL_0581: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
					IL_0586: mul
					IL_0587: stloc.s 58
					IL_0589: ldloc.s 53
					IL_058b: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::oneVector
					IL_0590: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
					IL_0595: mul
					IL_0596: stloc.s 59
					IL_0598: ldloc.s 53
					IL_059a: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::oneVector
					IL_059f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
					IL_05a4: mul
					IL_05a5: stloc.s 60
					IL_05a7: ldstr "&stack[0xFFFFFFFFFFFFFDF8]"
					IL_05ac: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_05b1: conv.i8
					IL_05b2: ldc.i8 -112
					IL_05bb: add
					IL_05bc: box [mscorlib]System.Int64
					IL_05c1: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
					IL_05c6: ldstr "&stack[0xFFFFFFFFFFFFFDF8]"
					IL_05cb: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_05d0: conv.i8
					IL_05d1: ldc.i8 64
					IL_05da: add
					IL_05db: box [mscorlib]System.Int64
					IL_05e0: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion>(object)
					IL_05e5: ldstr "&stack[0xFFFFFFFFFFFFFDF8]"
					IL_05ea: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_05ef: conv.i8
					IL_05f0: ldc.i8 -128
					IL_05f9: add
					IL_05fa: box [mscorlib]System.Int64
					IL_05ff: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
					IL_0604: call valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::TRS(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
					IL_0609: stloc.s 61
					IL_060b: ldstr "struct_chunk(LocalExpr+0x30, 16)"
					IL_0610: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0615: stloc.s 62
					IL_0617: ldstr "struct_chunk(LocalExpr+0x20, 16)"
					IL_061c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0621: stloc.s 63
					IL_0623: ldarg.0
					IL_0624: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_0629: ldloc.s 35
					IL_062b: conv.i4
					IL_062c: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_0631: stloc.s 64
					IL_0633: ldarg.0
					IL_0634: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_0639: ldloc.s 35
					IL_063b: conv.i4
					IL_063c: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_0641: stloc.s 65
					IL_0643: ldloca.s 66
					IL_0645: ldloca.s 61
					IL_0647: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m00
					IL_064c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m00
					IL_0651: ldloca.s 66
					IL_0653: ldloca.s 61
					IL_0655: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m10
					IL_065a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m10
					IL_065f: ldloca.s 66
					IL_0661: ldloca.s 61
					IL_0663: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m20
					IL_0668: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m20
					IL_066d: ldloca.s 66
					IL_066f: ldloca.s 61
					IL_0671: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m30
					IL_0676: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m30
					IL_067b: ldloca.s 66
					IL_067d: ldloca.s 61
					IL_067f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m01
					IL_0684: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m01
					IL_0689: ldloca.s 66
					IL_068b: ldloca.s 61
					IL_068d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m11
					IL_0692: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m11
					IL_0697: ldloca.s 66
					IL_0699: ldloca.s 61
					IL_069b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m21
					IL_06a0: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m21
					IL_06a5: ldloca.s 66
					IL_06a7: ldloca.s 61
					IL_06a9: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m31
					IL_06ae: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m31
					IL_06b3: ldloca.s 66
					IL_06b5: ldloca.s 61
					IL_06b7: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m02
					IL_06bc: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m02
					IL_06c1: ldloca.s 66
					IL_06c3: ldloca.s 61
					IL_06c5: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m12
					IL_06ca: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m12
					IL_06cf: ldloca.s 66
					IL_06d1: ldloca.s 61
					IL_06d3: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m22
					IL_06d8: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m22
					IL_06dd: ldloca.s 66
					IL_06df: ldloca.s 61
					IL_06e1: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m32
					IL_06e6: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m32
					IL_06eb: ldloca.s 66
					IL_06ed: ldloca.s 61
					IL_06ef: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m03
					IL_06f4: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m03
					IL_06f9: ldloca.s 66
					IL_06fb: ldloca.s 61
					IL_06fd: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m13
					IL_0702: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m13
					IL_0707: ldloca.s 66
					IL_0709: ldloca.s 61
					IL_070b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m23
					IL_0710: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m23
					IL_0715: ldloca.s 66
					IL_0717: ldloca.s 61
					IL_0719: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m33
					IL_071e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m33
					IL_0723: ldloca.s 66
					IL_0725: ldstr "&stack[0xFFFFFFFFFFFFFDF8]"
					IL_072a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_072f: conv.i8
					IL_0730: ldc.i8 -96
					IL_0739: add
					IL_073a: box [mscorlib]System.Int64
					IL_073f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
					IL_0744: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
					IL_0749: stloc.s 67
					IL_074b: ldstr "movsd qword ptr [r14+rdx*4+20h],xmm0 ; value = StructChunkExpr"
					IL_0750: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0755: ldstr "mov [r14+rdx*4+28h],r8d ; value = FieldExpr"
					IL_075a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_075f: ldarg.0
					IL_0760: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_0765: stloc.s 68
					IL_0767: ldstr "struct_chunk(LocalExpr+0x0, 8)"
					IL_076c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0771: stloc.s 63
					IL_0773: ldloc.s 20
					IL_0775: ldloc.s 20
					IL_0777: ldc.i8 2
					IL_0780: mul
					IL_0781: add
					IL_0782: stloc.s 69
					IL_0784: ldloc.s 68
					IL_0786: ldloc.s 35
					IL_0788: conv.i4
					IL_0789: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_078e: stloc.s 70
					IL_0790: ldloc.s 68
					IL_0792: ldloc.s 35
					IL_0794: conv.i4
					IL_0795: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_079a: stloc.s 71
					IL_079c: ldloca.s 72
					IL_079e: ldloca.s 61
					IL_07a0: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m00
					IL_07a5: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m00
					IL_07aa: ldloca.s 72
					IL_07ac: ldloca.s 61
					IL_07ae: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m10
					IL_07b3: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m10
					IL_07b8: ldloca.s 72
					IL_07ba: ldloca.s 61
					IL_07bc: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m20
					IL_07c1: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m20
					IL_07c6: ldloca.s 72
					IL_07c8: ldloca.s 61
					IL_07ca: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m30
					IL_07cf: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m30
					IL_07d4: ldloca.s 72
					IL_07d6: ldloca.s 61
					IL_07d8: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m01
					IL_07dd: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m01
					IL_07e2: ldloca.s 72
					IL_07e4: ldloca.s 61
					IL_07e6: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m11
					IL_07eb: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m11
					IL_07f0: ldloca.s 72
					IL_07f2: ldloca.s 61
					IL_07f4: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m21
					IL_07f9: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m21
					IL_07fe: ldloca.s 72
					IL_0800: ldloca.s 61
					IL_0802: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m31
					IL_0807: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m31
					IL_080c: ldloca.s 72
					IL_080e: ldloca.s 61
					IL_0810: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m02
					IL_0815: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m02
					IL_081a: ldloca.s 72
					IL_081c: ldloca.s 61
					IL_081e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m12
					IL_0823: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m12
					IL_0828: ldloca.s 72
					IL_082a: ldloca.s 61
					IL_082c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m22
					IL_0831: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m22
					IL_0836: ldloca.s 72
					IL_0838: ldloca.s 61
					IL_083a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m32
					IL_083f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m32
					IL_0844: ldloca.s 72
					IL_0846: ldloca.s 61
					IL_0848: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m03
					IL_084d: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m03
					IL_0852: ldloca.s 72
					IL_0854: ldloca.s 61
					IL_0856: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m13
					IL_085b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m13
					IL_0860: ldloca.s 72
					IL_0862: ldloca.s 61
					IL_0864: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m23
					IL_0869: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m23
					IL_086e: ldloca.s 72
					IL_0870: ldloca.s 61
					IL_0872: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m33
					IL_0877: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m33
					IL_087c: ldloca.s 72
					IL_087e: ldstr "&stack[0xFFFFFFFFFFFFFDF8]"
					IL_0883: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0888: conv.i8
					IL_0889: ldc.i8 -80
					IL_0892: add
					IL_0893: box [mscorlib]System.Int64
					IL_0898: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
					IL_089d: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
					IL_08a2: stloc.s 73
					IL_08a4: ldloc.s 20
					IL_08a6: ldc.i8 1
					IL_08af: add
					IL_08b0: stloc.s 11
					IL_08b2: ldstr "movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr"
					IL_08b7: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_08bc: ldstr "mov [r14+rcx*4+28h],edx ; value = FieldExpr"
					IL_08c1: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_08c6: ldloc.s 11
					IL_08c8: ldloc.s 11
					IL_08ca: ldc.i8 2
					IL_08d3: mul
					IL_08d4: add
					IL_08d5: stloc.s 11
					IL_08d7: ldarg.0
					IL_08d8: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_08dd: stloc.s 68
					IL_08df: ldstr "struct_chunk(LocalExpr+0x0, 8)"
					IL_08e4: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_08e9: stloc.s 63
					IL_08eb: ldloca.s 73
					IL_08ed: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
					IL_08f2: stloc.s 74
					IL_08f4: ldloc.s 68
					IL_08f6: ldloc.s 35
					IL_08f8: conv.i4
					IL_08f9: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_08fe: stloc.s 75
					IL_0900: ldloc.s 68
					IL_0902: ldloc.s 35
					IL_0904: conv.i4
					IL_0905: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_090a: stloc.s 76
					IL_090c: ldloca.s 77
					IL_090e: ldloca.s 61
					IL_0910: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m00
					IL_0915: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m00
					IL_091a: ldloca.s 77
					IL_091c: ldloca.s 61
					IL_091e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m10
					IL_0923: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m10
					IL_0928: ldloca.s 77
					IL_092a: ldloca.s 61
					IL_092c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m20
					IL_0931: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m20
					IL_0936: ldloca.s 77
					IL_0938: ldloca.s 61
					IL_093a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m30
					IL_093f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m30
					IL_0944: ldloca.s 77
					IL_0946: ldloca.s 61
					IL_0948: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m01
					IL_094d: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m01
					IL_0952: ldloca.s 77
					IL_0954: ldloca.s 61
					IL_0956: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m11
					IL_095b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m11
					IL_0960: ldloca.s 77
					IL_0962: ldloca.s 61
					IL_0964: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m21
					IL_0969: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m21
					IL_096e: ldloca.s 77
					IL_0970: ldloca.s 61
					IL_0972: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m31
					IL_0977: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m31
					IL_097c: ldloca.s 77
					IL_097e: ldloca.s 61
					IL_0980: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m02
					IL_0985: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m02
					IL_098a: ldloca.s 77
					IL_098c: ldloca.s 61
					IL_098e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m12
					IL_0993: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m12
					IL_0998: ldloca.s 77
					IL_099a: ldloca.s 61
					IL_099c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m22
					IL_09a1: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m22
					IL_09a6: ldloca.s 77
					IL_09a8: ldloca.s 61
					IL_09aa: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m32
					IL_09af: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m32
					IL_09b4: ldloca.s 77
					IL_09b6: ldloca.s 61
					IL_09b8: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m03
					IL_09bd: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m03
					IL_09c2: ldloca.s 77
					IL_09c4: ldloca.s 61
					IL_09c6: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m13
					IL_09cb: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m13
					IL_09d0: ldloca.s 77
					IL_09d2: ldloca.s 61
					IL_09d4: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m23
					IL_09d9: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m23
					IL_09de: ldloca.s 77
					IL_09e0: ldloca.s 61
					IL_09e2: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m33
					IL_09e7: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m33
					IL_09ec: ldloca.s 77
					IL_09ee: ldstr "&stack[0xFFFFFFFFFFFFFDF8]"
					IL_09f3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_09f8: conv.i8
					IL_09f9: ldc.i8 -64
					IL_0a02: add
					IL_0a03: box [mscorlib]System.Int64
					IL_0a08: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
					IL_0a0d: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
					IL_0a12: stloc.s 78
					IL_0a14: ldloc.s 20
					IL_0a16: ldc.i8 2
					IL_0a1f: add
					IL_0a20: stloc.s 11
					IL_0a22: ldstr "movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr"
					IL_0a27: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0a2c: ldstr "mov [r14+rcx*4+28h],edx ; value = FieldExpr"
					IL_0a31: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0a36: ldloc.s 11
					IL_0a38: ldloc.s 11
					IL_0a3a: ldc.i8 2
					IL_0a43: mul
					IL_0a44: add
					IL_0a45: stloc.s 11
					IL_0a47: ldarg.0
					IL_0a48: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_0a4d: stloc.s 68
					IL_0a4f: ldstr "struct_chunk(LocalExpr+0x0, 8)"
					IL_0a54: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0a59: stloc.s 63
					IL_0a5b: ldloca.s 78
					IL_0a5d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
					IL_0a62: stloc.s 79
					IL_0a64: ldloc.s 68
					IL_0a66: ldloc.s 35
					IL_0a68: conv.i4
					IL_0a69: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_0a6e: stloc.s 80
					IL_0a70: ldloc.s 68
					IL_0a72: ldloc.s 35
					IL_0a74: conv.i4
					IL_0a75: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_0a7a: stloc.s 81
					IL_0a7c: ldloca.s 82
					IL_0a7e: ldloca.s 61
					IL_0a80: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m00
					IL_0a85: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m00
					IL_0a8a: ldloca.s 82
					IL_0a8c: ldloca.s 61
					IL_0a8e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m10
					IL_0a93: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m10
					IL_0a98: ldloca.s 82
					IL_0a9a: ldloca.s 61
					IL_0a9c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m20
					IL_0aa1: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m20
					IL_0aa6: ldloca.s 82
					IL_0aa8: ldloca.s 61
					IL_0aaa: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m30
					IL_0aaf: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m30
					IL_0ab4: ldloca.s 82
					IL_0ab6: ldloca.s 61
					IL_0ab8: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m01
					IL_0abd: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m01
					IL_0ac2: ldloca.s 82
					IL_0ac4: ldloca.s 61
					IL_0ac6: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m11
					IL_0acb: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m11
					IL_0ad0: ldloca.s 82
					IL_0ad2: ldloca.s 61
					IL_0ad4: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m21
					IL_0ad9: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m21
					IL_0ade: ldloca.s 82
					IL_0ae0: ldloca.s 61
					IL_0ae2: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m31
					IL_0ae7: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m31
					IL_0aec: ldloca.s 82
					IL_0aee: ldloca.s 61
					IL_0af0: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m02
					IL_0af5: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m02
					IL_0afa: ldloca.s 82
					IL_0afc: ldloca.s 61
					IL_0afe: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m12
					IL_0b03: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m12
					IL_0b08: ldloca.s 82
					IL_0b0a: ldloca.s 61
					IL_0b0c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m22
					IL_0b11: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m22
					IL_0b16: ldloca.s 82
					IL_0b18: ldloca.s 61
					IL_0b1a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m32
					IL_0b1f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m32
					IL_0b24: ldloca.s 82
					IL_0b26: ldloca.s 61
					IL_0b28: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m03
					IL_0b2d: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m03
					IL_0b32: ldloca.s 82
					IL_0b34: ldloca.s 61
					IL_0b36: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m13
					IL_0b3b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m13
					IL_0b40: ldloca.s 82
					IL_0b42: ldloca.s 61
					IL_0b44: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m23
					IL_0b49: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m23
					IL_0b4e: ldloca.s 82
					IL_0b50: ldloca.s 61
					IL_0b52: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m33
					IL_0b57: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m33
					IL_0b5c: ldloca.s 82
					IL_0b5e: ldstr "&stack[0xFFFFFFFFFFFFFDF8]"
					IL_0b63: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0b68: conv.i8
					IL_0b69: ldc.i8 -48
					IL_0b72: add
					IL_0b73: box [mscorlib]System.Int64
					IL_0b78: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
					IL_0b7d: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
					IL_0b82: stloc.s 83
					IL_0b84: ldloc.s 20
					IL_0b86: ldc.i8 3
					IL_0b8f: add
					IL_0b90: stloc.s 11
					IL_0b92: ldstr "movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr"
					IL_0b97: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0b9c: ldstr "mov [r14+rcx*4+28h],edx ; value = FieldExpr"
					IL_0ba1: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0ba6: ldloc.s 11
					IL_0ba8: ldloc.s 11
					IL_0baa: ldc.i8 2
					IL_0bb3: mul
					IL_0bb4: add
					IL_0bb5: stloc.s 11
					IL_0bb7: ldstr "struct_chunk(LocalExpr+0x0, 8)"
					IL_0bbc: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0bc1: stloc.s 63
					IL_0bc3: ldarg.0
					IL_0bc4: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_0bc9: stloc.s 84
					IL_0bcb: ldloc.s 84
					IL_0bcd: ldloc.s 35
					IL_0bcf: conv.i4
					IL_0bd0: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_0bd5: stloc.s 85
					IL_0bd7: ldstr "movss dword ptr [rdx+rcx*4+20h],xmm0 ; value = BinaryExpr"
					IL_0bdc: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0be1: ldstr "movss dword ptr [rdx+rcx*4+28h],xmm1 ; value = BinaryExpr"
					IL_0be6: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0beb: ldstr "movss dword ptr [rdx+rcx*4+24h],xmm0 ; value = BinaryExpr"
					IL_0bf0: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0bf5: ldarg.0
					IL_0bf6: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_0bfb: stloc.s 86
					IL_0bfd: ldloc.s 39
					IL_0bff: ldstr "*(LocalExpr + 0x28)"
					IL_0c04: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0c09: conv.i4
					IL_0c0a: conv.r4
					IL_0c0b: add
					IL_0c0c: stloc.s 87
					IL_0c0e: ldloc.s 40
					IL_0c10: ldstr "*(LocalExpr + 0x24)"
					IL_0c15: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0c1a: conv.i4
					IL_0c1b: conv.r4
					IL_0c1c: add
					IL_0c1d: stloc.s 88
					IL_0c1f: ldloc.s 86
					IL_0c21: ldloc.s 35
					IL_0c23: conv.i4
					IL_0c24: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_0c29: stloc.s 89
					IL_0c2b: ldstr "movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr"
					IL_0c30: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0c35: ldstr "movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr"
					IL_0c3a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0c3f: ldstr "movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr"
					IL_0c44: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0c49: ldarg.0
					IL_0c4a: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_0c4f: stloc.s 90
					IL_0c51: ldloc.s 39
					IL_0c53: ldstr "*(LocalExpr + 0x28)"
					IL_0c58: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0c5d: conv.i4
					IL_0c5e: conv.r4
					IL_0c5f: add
					IL_0c60: stloc.s 87
					IL_0c62: ldloc.s 40
					IL_0c64: ldstr "*(LocalExpr + 0x24)"
					IL_0c69: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0c6e: conv.i4
					IL_0c6f: conv.r4
					IL_0c70: add
					IL_0c71: stloc.s 88
					IL_0c73: ldloc.s 90
					IL_0c75: ldloc.s 35
					IL_0c77: conv.i4
					IL_0c78: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_0c7d: stloc.s 91
					IL_0c7f: ldstr "movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr"
					IL_0c84: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0c89: ldstr "movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr"
					IL_0c8e: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0c93: ldstr "movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr"
					IL_0c98: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0c9d: ldarg.0
					IL_0c9e: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_0ca3: stloc.s 92
					IL_0ca5: ldloc.s 39
					IL_0ca7: ldstr "*(LocalExpr + 0x28)"
					IL_0cac: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0cb1: conv.i4
					IL_0cb2: conv.r4
					IL_0cb3: add
					IL_0cb4: stloc.s 87
					IL_0cb6: ldloc.s 40
					IL_0cb8: ldstr "*(LocalExpr + 0x24)"
					IL_0cbd: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0cc2: conv.i4
					IL_0cc3: conv.r4
					IL_0cc4: add
					IL_0cc5: stloc.s 88
					IL_0cc7: ldloc.s 92
					IL_0cc9: ldloc.s 35
					IL_0ccb: conv.i4
					IL_0ccc: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_0cd1: stloc.s 93
					IL_0cd3: ldstr "movss dword ptr [rcx+rax*4+20h],xmm6 ; value = BinaryExpr"
					IL_0cd8: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0cdd: ldstr "movss dword ptr [rcx+rax*4+24h],xmm7 ; value = BinaryExpr"
					IL_0ce2: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0ce7: ldstr "movss dword ptr [rcx+rax*4+28h],xmm8 ; value = BinaryExpr"
					IL_0cec: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0cf1: ldarg.0
					IL_0cf2: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_0cf7: stloc.s 94
					IL_0cf9: ldloc.s 94
					IL_0cfb: ldloc.s 35
					IL_0cfd: conv.i4
					IL_0cfe: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_0d03: stloc.s 95
					IL_0d05: ldstr "movss dword ptr [rdx+rcx*4+20h],xmm0 ; value = BinaryExpr"
					IL_0d0a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0d0f: ldstr "movss dword ptr [rdx+rcx*4+28h],xmm1 ; value = BinaryExpr"
					IL_0d14: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0d19: ldstr "movss dword ptr [rdx+rcx*4+24h],xmm0 ; value = BinaryExpr"
					IL_0d1e: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0d23: ldarg.0
					IL_0d24: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_0d29: stloc.s 96
					IL_0d2b: ldstr "*(LocalExpr + 0x28)"
					IL_0d30: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0d35: conv.i4
					IL_0d36: conv.r4
					IL_0d37: ldloc.s 31
					IL_0d39: sub
					IL_0d3a: stloc.s 87
					IL_0d3c: ldstr "*(LocalExpr + 0x24)"
					IL_0d41: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0d46: conv.i4
					IL_0d47: conv.r4
					IL_0d48: ldloc.s 32
					IL_0d4a: sub
					IL_0d4b: stloc.s 88
					IL_0d4d: ldloc.s 96
					IL_0d4f: ldloc.s 35
					IL_0d51: conv.i4
					IL_0d52: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_0d57: stloc.s 97
					IL_0d59: ldstr "movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr"
					IL_0d5e: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0d63: ldstr "movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr"
					IL_0d68: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0d6d: ldstr "movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr"
					IL_0d72: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0d77: ldarg.0
					IL_0d78: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_0d7d: stloc.s 98
					IL_0d7f: ldstr "*(LocalExpr + 0x28)"
					IL_0d84: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0d89: conv.i4
					IL_0d8a: conv.r4
					IL_0d8b: ldloc.s 31
					IL_0d8d: sub
					IL_0d8e: stloc.s 87
					IL_0d90: ldstr "*(LocalExpr + 0x24)"
					IL_0d95: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0d9a: conv.i4
					IL_0d9b: conv.r4
					IL_0d9c: ldloc.s 32
					IL_0d9e: sub
					IL_0d9f: stloc.s 88
					IL_0da1: ldloc.s 98
					IL_0da3: ldloc.s 35
					IL_0da5: conv.i4
					IL_0da6: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_0dab: stloc.s 99
					IL_0dad: ldstr "movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr"
					IL_0db2: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0db7: ldstr "movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr"
					IL_0dbc: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0dc1: ldstr "movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr"
					IL_0dc6: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0dcb: ldarg.0
					IL_0dcc: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_0dd1: stloc.s 100
					IL_0dd3: ldstr "*(LocalExpr + 0x28)"
					IL_0dd8: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0ddd: conv.i4
					IL_0dde: conv.r4
					IL_0ddf: ldloc.s 31
					IL_0de1: sub
					IL_0de2: stloc.s 87
					IL_0de4: ldstr "*(LocalExpr + 0x24)"
					IL_0de9: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0dee: conv.i4
					IL_0def: conv.r4
					IL_0df0: ldloc.s 32
					IL_0df2: sub
					IL_0df3: stloc.s 88
					IL_0df5: ldloc.s 100
					IL_0df7: ldloc.s 35
					IL_0df9: conv.i4
					IL_0dfa: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_0dff: stloc.s 101
					IL_0e01: ldstr "movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr"
					IL_0e06: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0e0b: ldstr "movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr"
					IL_0e10: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0e15: ldstr "movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr"
					IL_0e1a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_0e1f: ldstr "global_19E3BFF"
					IL_0e24: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0e29: conv.i4
					IL_0e2a: stloc.s 102
					IL_0e2c: ldstr "*(LocalExpr + 0x28)"
					IL_0e31: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0e36: conv.i4
					IL_0e37: conv.r4
					IL_0e38: ldloc.s 31
					IL_0e3a: sub
					IL_0e3b: stloc.s 87
					IL_0e3d: ldstr "*(LocalExpr + 0x24)"
					IL_0e42: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0e47: conv.i4
					IL_0e48: conv.r4
					IL_0e49: ldloc.s 32
					IL_0e4b: sub
					IL_0e4c: stloc.s 88
					IL_0e4e: ldstr "global_19E3BFF"
					IL_0e53: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0e58: conv.i4
					IL_0e59: ldc.i4.0
					IL_0e5a: ceq
					IL_0e5c: ldc.i4.0
					IL_0e5d: ceq
					IL_0e5f: brtrue.s IL_0e6c

					IL_0e61: nop
					IL_0e62: ldstr "mov [1819E3BFFh],cl ; value = 0x1"
					IL_0e67: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)

					IL_0e6c: nop
					IL_0e6d: ldstr "struct_chunk(StaticFieldExpr+0x0, 8)"
					IL_0e72: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0e77: stloc.s 103
					IL_0e79: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::oneVector
					IL_0e7e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
					IL_0e83: stloc.s 56
					IL_0e85: ldc.i4.1
					IL_0e86: ldc.i4.0
					IL_0e87: ceq
					IL_0e89: ldc.i4.0
					IL_0e8a: ceq
					IL_0e8c: brtrue.s IL_0e99

					IL_0e8e: nop
					IL_0e8f: ldstr "mov byte ptr [1819E3BFFh],1 ; value = 0x1"
					IL_0e94: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)

					IL_0e99: nop
					IL_0e9a: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::oneVector
					IL_0e9f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
					IL_0ea4: stloc.s 104
					IL_0ea6: ldstr "&stack[0xFFFFFFFFFFFFFDF8]"
					IL_0eab: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0eb0: conv.i8
					IL_0eb1: ldc.i8 -16
					IL_0eba: add
					IL_0ebb: box [mscorlib]System.Int64
					IL_0ec0: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
					IL_0ec5: ldstr "&stack[0xFFFFFFFFFFFFFDF8]"
					IL_0eca: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0ecf: conv.i8
					IL_0ed0: ldc.i8 64
					IL_0ed9: add
					IL_0eda: box [mscorlib]System.Int64
					IL_0edf: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion>(object)
					IL_0ee4: ldstr "&stack[0xFFFFFFFFFFFFFDF8]"
					IL_0ee9: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0eee: conv.i8
					IL_0eef: ldc.i8 -32
					IL_0ef8: add
					IL_0ef9: box [mscorlib]System.Int64
					IL_0efe: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
					IL_0f03: call valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::TRS(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
					IL_0f08: stloc.s 105
					IL_0f0a: ldstr "struct_chunk(LocalExpr+0x20, 16)"
					IL_0f0f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0f14: stloc.s 106
					IL_0f16: ldstr "struct_chunk(LocalExpr+0x30, 16)"
					IL_0f1b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_0f20: stloc.s 107
					IL_0f22: ldarg.0
					IL_0f23: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_0f28: ldloc.s 35
					IL_0f2a: conv.i4
					IL_0f2b: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_0f30: stloc.s 108
					IL_0f32: ldloca.s 109
					IL_0f34: ldloca.s 105
					IL_0f36: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m00
					IL_0f3b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m00
					IL_0f40: ldloca.s 109
					IL_0f42: ldloca.s 105
					IL_0f44: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m10
					IL_0f49: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m10
					IL_0f4e: ldloca.s 109
					IL_0f50: ldloca.s 105
					IL_0f52: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m20
					IL_0f57: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m20
					IL_0f5c: ldloca.s 109
					IL_0f5e: ldloca.s 105
					IL_0f60: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m30
					IL_0f65: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m30
					IL_0f6a: ldloca.s 109
					IL_0f6c: ldloca.s 105
					IL_0f6e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m01
					IL_0f73: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m01
					IL_0f78: ldloca.s 109
					IL_0f7a: ldloca.s 105
					IL_0f7c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m11
					IL_0f81: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m11
					IL_0f86: ldloca.s 109
					IL_0f88: ldloca.s 105
					IL_0f8a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m21
					IL_0f8f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m21
					IL_0f94: ldloca.s 109
					IL_0f96: ldloca.s 105
					IL_0f98: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m31
					IL_0f9d: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m31
					IL_0fa2: ldloca.s 109
					IL_0fa4: ldloca.s 105
					IL_0fa6: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m02
					IL_0fab: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m02
					IL_0fb0: ldloca.s 109
					IL_0fb2: ldloca.s 105
					IL_0fb4: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m12
					IL_0fb9: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m12
					IL_0fbe: ldloca.s 109
					IL_0fc0: ldloca.s 105
					IL_0fc2: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m22
					IL_0fc7: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m22
					IL_0fcc: ldloca.s 109
					IL_0fce: ldloca.s 105
					IL_0fd0: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m32
					IL_0fd5: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m32
					IL_0fda: ldloca.s 109
					IL_0fdc: ldloca.s 105
					IL_0fde: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m03
					IL_0fe3: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m03
					IL_0fe8: ldloca.s 109
					IL_0fea: ldloca.s 105
					IL_0fec: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m13
					IL_0ff1: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m13
					IL_0ff6: ldloca.s 109
					IL_0ff8: ldloca.s 105
					IL_0ffa: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m23
					IL_0fff: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m23
					IL_1004: ldloca.s 109
					IL_1006: ldloca.s 105
					IL_1008: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m33
					IL_100d: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m33
					IL_1012: ldloca.s 109
					IL_1014: ldloca.s 110
					IL_1016: ldstr "*(ArrayElementExpr + 0x20)"
					IL_101b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_1020: box [mscorlib]System.Int64
					IL_1025: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
					IL_102a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
					IL_102f: ldloca.s 110
					IL_1031: ldstr "*(ArrayElementExpr + 0x28)"
					IL_1036: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_103b: conv.i4
					IL_103c: box [mscorlib]System.Int32
					IL_1041: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
					IL_1046: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
					IL_104b: ldloc.s 110
					IL_104d: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
					IL_1052: stloc.s 111
					IL_1054: ldstr "movsd qword ptr [r14+rdx*4+20h],xmm0 ; value = StructChunkExpr"
					IL_1059: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_105e: ldstr "mov [r14+rdx*4+28h],r8d ; value = FieldExpr"
					IL_1063: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_1068: ldarg.0
					IL_1069: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_106e: stloc.s 68
					IL_1070: ldstr "struct_chunk(LocalExpr+0x0, 8)"
					IL_1075: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_107a: stloc.s 106
					IL_107c: ldloc.s 68
					IL_107e: ldloc.s 35
					IL_1080: conv.i4
					IL_1081: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_1086: stloc.s 112
					IL_1088: ldloca.s 113
					IL_108a: ldloca.s 105
					IL_108c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m00
					IL_1091: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m00
					IL_1096: ldloca.s 113
					IL_1098: ldloca.s 105
					IL_109a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m10
					IL_109f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m10
					IL_10a4: ldloca.s 113
					IL_10a6: ldloca.s 105
					IL_10a8: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m20
					IL_10ad: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m20
					IL_10b2: ldloca.s 113
					IL_10b4: ldloca.s 105
					IL_10b6: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m30
					IL_10bb: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m30
					IL_10c0: ldloca.s 113
					IL_10c2: ldloca.s 105
					IL_10c4: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m01
					IL_10c9: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m01
					IL_10ce: ldloca.s 113
					IL_10d0: ldloca.s 105
					IL_10d2: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m11
					IL_10d7: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m11
					IL_10dc: ldloca.s 113
					IL_10de: ldloca.s 105
					IL_10e0: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m21
					IL_10e5: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m21
					IL_10ea: ldloca.s 113
					IL_10ec: ldloca.s 105
					IL_10ee: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m31
					IL_10f3: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m31
					IL_10f8: ldloca.s 113
					IL_10fa: ldloca.s 105
					IL_10fc: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m02
					IL_1101: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m02
					IL_1106: ldloca.s 113
					IL_1108: ldloca.s 105
					IL_110a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m12
					IL_110f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m12
					IL_1114: ldloca.s 113
					IL_1116: ldloca.s 105
					IL_1118: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m22
					IL_111d: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m22
					IL_1122: ldloca.s 113
					IL_1124: ldloca.s 105
					IL_1126: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m32
					IL_112b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m32
					IL_1130: ldloca.s 113
					IL_1132: ldloca.s 105
					IL_1134: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m03
					IL_1139: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m03
					IL_113e: ldloca.s 113
					IL_1140: ldloca.s 105
					IL_1142: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m13
					IL_1147: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m13
					IL_114c: ldloca.s 113
					IL_114e: ldloca.s 105
					IL_1150: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m23
					IL_1155: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m23
					IL_115a: ldloca.s 113
					IL_115c: ldloca.s 105
					IL_115e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m33
					IL_1163: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m33
					IL_1168: ldloca.s 113
					IL_116a: ldstr "&stack[0xFFFFFFFFFFFFFDF8]"
					IL_116f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_1174: conv.i8
					IL_1175: ldc.i8 16
					IL_117e: add
					IL_117f: box [mscorlib]System.Int64
					IL_1184: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
					IL_1189: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
					IL_118e: stloc.s 114
					IL_1190: ldstr "movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr"
					IL_1195: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_119a: ldstr "mov [r14+rcx*4+28h],edx ; value = FieldExpr"
					IL_119f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_11a4: ldarg.0
					IL_11a5: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_11aa: stloc.s 68
					IL_11ac: ldstr "struct_chunk(LocalExpr+0x0, 8)"
					IL_11b1: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_11b6: stloc.s 106
					IL_11b8: ldloc.s 68
					IL_11ba: ldloc.s 35
					IL_11bc: conv.i4
					IL_11bd: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_11c2: stloc.s 115
					IL_11c4: ldloca.s 116
					IL_11c6: ldloca.s 105
					IL_11c8: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m00
					IL_11cd: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m00
					IL_11d2: ldloca.s 116
					IL_11d4: ldloca.s 105
					IL_11d6: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m10
					IL_11db: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m10
					IL_11e0: ldloca.s 116
					IL_11e2: ldloca.s 105
					IL_11e4: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m20
					IL_11e9: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m20
					IL_11ee: ldloca.s 116
					IL_11f0: ldloca.s 105
					IL_11f2: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m30
					IL_11f7: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m30
					IL_11fc: ldloca.s 116
					IL_11fe: ldloca.s 105
					IL_1200: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m01
					IL_1205: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m01
					IL_120a: ldloca.s 116
					IL_120c: ldloca.s 105
					IL_120e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m11
					IL_1213: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m11
					IL_1218: ldloca.s 116
					IL_121a: ldloca.s 105
					IL_121c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m21
					IL_1221: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m21
					IL_1226: ldloca.s 116
					IL_1228: ldloca.s 105
					IL_122a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m31
					IL_122f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m31
					IL_1234: ldloca.s 116
					IL_1236: ldloca.s 105
					IL_1238: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m02
					IL_123d: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m02
					IL_1242: ldloca.s 116
					IL_1244: ldloca.s 105
					IL_1246: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m12
					IL_124b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m12
					IL_1250: ldloca.s 116
					IL_1252: ldloca.s 105
					IL_1254: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m22
					IL_1259: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m22
					IL_125e: ldloca.s 116
					IL_1260: ldloca.s 105
					IL_1262: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m32
					IL_1267: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m32
					IL_126c: ldloca.s 116
					IL_126e: ldloca.s 105
					IL_1270: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m03
					IL_1275: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m03
					IL_127a: ldloca.s 116
					IL_127c: ldloca.s 105
					IL_127e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m13
					IL_1283: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m13
					IL_1288: ldloca.s 116
					IL_128a: ldloca.s 105
					IL_128c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m23
					IL_1291: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m23
					IL_1296: ldloca.s 116
					IL_1298: ldloca.s 105
					IL_129a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m33
					IL_129f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m33
					IL_12a4: ldloca.s 116
					IL_12a6: ldstr "&stack[0xFFFFFFFFFFFFFDF8]"
					IL_12ab: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_12b0: conv.i8
					IL_12b1: ldc.i8 32
					IL_12ba: add
					IL_12bb: box [mscorlib]System.Int64
					IL_12c0: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
					IL_12c5: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
					IL_12ca: stloc.s 117
					IL_12cc: ldstr "movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr"
					IL_12d1: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_12d6: ldstr "mov [r14+rcx*4+28h],edx ; value = FieldExpr"
					IL_12db: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_12e0: ldarg.0
					IL_12e1: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_12e6: stloc.s 68
					IL_12e8: ldstr "struct_chunk(LocalExpr+0x0, 8)"
					IL_12ed: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_12f2: stloc.s 106
					IL_12f4: ldloc.s 68
					IL_12f6: ldloc.s 35
					IL_12f8: conv.i4
					IL_12f9: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_12fe: stloc.s 118
					IL_1300: ldloca.s 119
					IL_1302: ldloca.s 105
					IL_1304: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m00
					IL_1309: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m00
					IL_130e: ldloca.s 119
					IL_1310: ldloca.s 105
					IL_1312: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m10
					IL_1317: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m10
					IL_131c: ldloca.s 119
					IL_131e: ldloca.s 105
					IL_1320: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m20
					IL_1325: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m20
					IL_132a: ldloca.s 119
					IL_132c: ldloca.s 105
					IL_132e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m30
					IL_1333: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m30
					IL_1338: ldloca.s 119
					IL_133a: ldloca.s 105
					IL_133c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m01
					IL_1341: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m01
					IL_1346: ldloca.s 119
					IL_1348: ldloca.s 105
					IL_134a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m11
					IL_134f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m11
					IL_1354: ldloca.s 119
					IL_1356: ldloca.s 105
					IL_1358: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m21
					IL_135d: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m21
					IL_1362: ldloca.s 119
					IL_1364: ldloca.s 105
					IL_1366: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m31
					IL_136b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m31
					IL_1370: ldloca.s 119
					IL_1372: ldloca.s 105
					IL_1374: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m02
					IL_1379: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m02
					IL_137e: ldloca.s 119
					IL_1380: ldloca.s 105
					IL_1382: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m12
					IL_1387: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m12
					IL_138c: ldloca.s 119
					IL_138e: ldloca.s 105
					IL_1390: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m22
					IL_1395: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m22
					IL_139a: ldloca.s 119
					IL_139c: ldloca.s 105
					IL_139e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m32
					IL_13a3: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m32
					IL_13a8: ldloca.s 119
					IL_13aa: ldloca.s 105
					IL_13ac: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m03
					IL_13b1: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m03
					IL_13b6: ldloca.s 119
					IL_13b8: ldloca.s 105
					IL_13ba: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m13
					IL_13bf: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m13
					IL_13c4: ldloca.s 119
					IL_13c6: ldloca.s 105
					IL_13c8: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m23
					IL_13cd: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m23
					IL_13d2: ldloca.s 119
					IL_13d4: ldloca.s 105
					IL_13d6: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m33
					IL_13db: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m33
					IL_13e0: ldloca.s 119
					IL_13e2: ldstr "&stack[0xFFFFFFFFFFFFFDF8]"
					IL_13e7: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_13ec: conv.i8
					IL_13ed: ldc.i8 48
					IL_13f6: add
					IL_13f7: box [mscorlib]System.Int64
					IL_13fc: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
					IL_1401: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::MultiplyPoint3x4(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
					IL_1406: stloc.s 120
					IL_1408: ldstr "movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr"
					IL_140d: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_1412: ldstr "mov [r14+rcx*4+28h],edx ; value = FieldExpr"
					IL_1417: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_141c: ldstr "struct_chunk(LocalExpr+0x0, 8)"
					IL_1421: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_1426: stloc.s 106
					IL_1428: ldarg.0
					IL_1429: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_142e: ldloc.s 35
					IL_1430: conv.i4
					IL_1431: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_1436: stloc.s 121
					IL_1438: ldstr "movss dword ptr [rdx+rcx*4+20h],xmm0 ; value = BinaryExpr"
					IL_143d: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_1442: ldstr "movss dword ptr [rdx+rcx*4+28h],xmm1 ; value = BinaryExpr"
					IL_1447: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_144c: ldstr "movss dword ptr [rdx+rcx*4+24h],xmm0 ; value = BinaryExpr"
					IL_1451: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_1456: ldloc.s 32
					IL_1458: ldstr "*(LocalExpr + 0x24)"
					IL_145d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_1462: conv.i4
					IL_1463: conv.r4
					IL_1464: add
					IL_1465: stloc.s 122
					IL_1467: ldloc.s 31
					IL_1469: ldstr "*(LocalExpr + 0x28)"
					IL_146e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_1473: conv.i4
					IL_1474: conv.r4
					IL_1475: add
					IL_1476: stloc.s 123
					IL_1478: ldarg.0
					IL_1479: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_147e: ldloc.s 35
					IL_1480: conv.i4
					IL_1481: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_1486: stloc.s 124
					IL_1488: ldstr "movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr"
					IL_148d: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_1492: ldstr "movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr"
					IL_1497: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_149c: ldstr "movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr"
					IL_14a1: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_14a6: ldloc.s 32
					IL_14a8: ldstr "*(LocalExpr + 0x24)"
					IL_14ad: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_14b2: conv.i4
					IL_14b3: conv.r4
					IL_14b4: add
					IL_14b5: stloc.s 122
					IL_14b7: ldloc.s 31
					IL_14b9: ldstr "*(LocalExpr + 0x28)"
					IL_14be: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_14c3: conv.i4
					IL_14c4: conv.r4
					IL_14c5: add
					IL_14c6: stloc.s 123
					IL_14c8: ldarg.0
					IL_14c9: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_14ce: ldloc.s 35
					IL_14d0: conv.i4
					IL_14d1: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_14d6: stloc.s 125
					IL_14d8: ldstr "movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr"
					IL_14dd: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_14e2: ldstr "movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr"
					IL_14e7: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_14ec: ldstr "movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr"
					IL_14f1: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_14f6: ldloc.s 32
					IL_14f8: ldstr "*(LocalExpr + 0x24)"
					IL_14fd: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_1502: conv.i4
					IL_1503: conv.r4
					IL_1504: add
					IL_1505: stloc.s 122
					IL_1507: ldloc.s 31
					IL_1509: ldstr "*(LocalExpr + 0x28)"
					IL_150e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_1513: conv.i4
					IL_1514: conv.r4
					IL_1515: add
					IL_1516: stloc.s 123
					IL_1518: ldarg.0
					IL_1519: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
					IL_151e: ldloc.s 35
					IL_1520: conv.i4
					IL_1521: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
					IL_1526: stloc.s 126
					IL_1528: ldstr "movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr"
					IL_152d: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_1532: ldstr "movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr"
					IL_1537: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_153c: ldstr "movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr"
					IL_1541: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
					IL_1546: ldloc.s 32
					IL_1548: ldstr "*(LocalExpr + 0x24)"
					IL_154d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_1552: conv.i4
					IL_1553: conv.r4
					IL_1554: add
					IL_1555: stloc.s 122
					IL_1557: ldloc.s 31
					IL_1559: ldstr "*(LocalExpr + 0x28)"
					IL_155e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
					IL_1563: conv.i4
					IL_1564: conv.r4
					IL_1565: add
					IL_1566: stloc.s 123

					IL_1568: nop
					IL_1569: ldloc.s 23
					IL_156b: conv.i4
					IL_156c: ldc.i4.1
					IL_156d: add
					IL_156e: stloc.s 127
					IL_1570: ldloc.s 23
					IL_1572: conv.i4
					IL_1573: ldc.i4.1
					IL_1574: add
					IL_1575: ldloc.s 25
					IL_1577: conv.i4
					IL_1578: cgt
					IL_157a: ldc.i4.0
					IL_157b: ceq
					IL_157d: brtrue IL_029d
				// end loop

				IL_1582: nop
				IL_1583: ldc.i4.0
				IL_1584: ldc.i4.1
				IL_1585: add
				IL_1586: stloc.s 128
				IL_1588: ldc.i4.0
				IL_1589: ldc.i4.1
				IL_158a: add
				IL_158b: ldstr "stack[0x8]"
				IL_1590: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_1595: conv.i4
				IL_1596: clt
				IL_1598: brtrue IL_01b4
			// end loop
			IL_159d: nop
			IL_159e: ldstr "stack[0x18]"
			IL_15a3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_15a8: stloc.s 20

			IL_15aa: nop
			IL_15ab: ldarg.0
			IL_15ac: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
			IL_15b1: stloc.s 18
			// loop start (head: IL_15b3)
				IL_15b3: nop
				IL_15b4: ldc.i4.0
				IL_15b5: ldloc.s 18
				IL_15b7: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
				IL_15bc: ldlen
				IL_15bd: conv.i4
				IL_15be: clt
				IL_15c0: ldc.i4.0
				IL_15c1: ceq
				IL_15c3: brtrue IL_1649

				IL_15c8: nop
				IL_15c9: ldarg.0
				IL_15ca: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
				IL_15cf: stloc.s 129
				IL_15d1: ldloc.s 129
				IL_15d3: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
				IL_15d8: stloc.s 130
				IL_15da: ldarg.0
				IL_15db: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[][] TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<copyOfVertices>5__3'
				IL_15e0: stloc.s 131
				IL_15e2: ldstr "*(LocalExpr + 0x20)"
				IL_15e7: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_15ec: stloc.s 132
				IL_15ee: ldc.i4.0
				IL_15ef: conv.i8
				IL_15f0: stloc.s 133
				IL_15f2: ldloc.s 132
				IL_15f4: box [mscorlib]System.Int64
				IL_15f9: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [UnityEngine.CoreModule]UnityEngine.Mesh>(object)
				IL_15fe: ldloc.s 131
				IL_1600: ldc.i4.0
				IL_1601: conv.i8
				IL_1602: conv.i4
				IL_1603: ldelem valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[]
				IL_1608: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Mesh::set_vertices(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3[])
				IL_160d: ldstr "*(LocalExpr + 0x30)"
				IL_1612: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_1617: stloc.s 132
				IL_1619: ldc.i4.0
				IL_161a: ldc.i4.1
				IL_161b: add
				IL_161c: stloc.s 134
				IL_161e: ldstr "*(*(LocalExpr + 0x0) + 0x7F0)"
				IL_1623: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
				IL_1628: stloc.s 135
				IL_162a: ldarg.0
				IL_162b: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
				IL_1630: stloc.s 136
				IL_1632: ldc.i4.0
				IL_1633: ldc.i4.1
				IL_1634: add
				IL_1635: stloc.s 137
				IL_1637: ldarg.0
				IL_1638: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<textInfo>5__2'
				IL_163d: ldnull
				IL_163e: ceq
				IL_1640: ldc.i4.0
				IL_1641: ceq
				IL_1643: brtrue IL_15b3
			// end loop
			IL_1648: nop

			IL_1649: nop
			IL_164a: ldc.r4 0.1
			IL_164f: newobj instance void [UnityEngine.CoreModule]UnityEngine.WaitForSeconds::.ctor(float32)
			IL_1654: stloc.s 138
			IL_1656: ldarg.0
			IL_1657: ldloc.s 138
			IL_1659: stfld object TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<>2__current'
			IL_165e: ldarg.0
			IL_165f: ldc.i4.2
			IL_1660: stfld int32 TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<>1__state'

			IL_1665: nop

			IL_1666: nop
			IL_1667: ldc.i4.1
			IL_1668: ret

			IL_1669: nop
			IL_166a: ldc.r4 0.25
			IL_166f: newobj instance void [UnityEngine.CoreModule]UnityEngine.WaitForSeconds::.ctor(float32)
			IL_1674: stloc.s 139
			IL_1676: ldarg.0
			IL_1677: ldloc.s 139
			IL_1679: stfld object TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<>2__current'
			IL_167e: ldarg.0
			IL_167f: ldc.i4.1
			IL_1680: stfld int32 TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<>1__state'
			IL_1685: br.s IL_1665

			IL_1687: nop
			IL_1688: ldnull
			IL_1689: throw

			IL_168a: nop
			IL_168b: ldnull
			IL_168c: throw
		} // end of method '<AnimateVertexColors>d__10'::MoveNext

		.method private final hidebysig specialname newslot virtual 
			instance object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () cil managed 
		{
			// Method begins at RVA 0x2f3e
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<>2__current'
			IL_0007: ret
		} // end of method '<AnimateVertexColors>d__10'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'

		.method private final hidebysig newslot virtual 
			instance void System.Collections.IEnumerator.Reset () cil managed 
		{
			// Method begins at RVA 0x201dc
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
			// Method begins at RVA 0x2f47
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<>2__current'
			IL_0007: ret
		} // end of method '<AnimateVertexColors>d__10'::System.Collections.IEnumerator.get_Current

		// Properties
		.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current'()
		{
			.get instance object TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'()
		}
		.property instance object System.Collections.IEnumerator.Current()
		{
			.get instance object TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::System.Collections.IEnumerator.get_Current()
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
		// Method begins at RVA 0x1985c
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
		IL_000a: stfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.VertexShakeB::m_TextComponent
		IL_000f: ret
	} // end of method VertexShakeB::Awake

	.method private hidebysig 
		instance void OnEnable () cil managed 
	{
		// Method begins at RVA 0x19878
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
		IL_000c: ldftn instance void TMPro.Examples.VertexShakeB::ON_TEXT_CHANGED(class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0012: newobj instance void [System]System.IOAsyncCallback::.ctor(object, native int)
		IL_0017: stloc.1
		IL_0018: ldloc.0
		IL_0019: ldloc.1
		IL_001a: callvirt instance void class [Unity.TextMeshPro]TMPro.FastAction`1<class [UnityEngine.CoreModule]UnityEngine.Object>::Add(class [mscorlib]System.Action`1<!0>)
		IL_001f: ret

		IL_0020: nop
		IL_0021: ldnull
		IL_0022: throw
	} // end of method VertexShakeB::OnEnable

	.method private hidebysig 
		instance void OnDisable () cil managed 
	{
		// Method begins at RVA 0x198a8
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
		IL_000c: ldftn instance void TMPro.Examples.VertexShakeB::ON_TEXT_CHANGED(class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0012: newobj instance void [System]System.IOAsyncCallback::.ctor(object, native int)
		IL_0017: stloc.1
		IL_0018: ldloc.0
		IL_0019: ldloc.1
		IL_001a: callvirt instance void class [Unity.TextMeshPro]TMPro.FastAction`1<class [UnityEngine.CoreModule]UnityEngine.Object>::Remove(class [mscorlib]System.Action`1<!0>)
		IL_001f: ret

		IL_0020: nop
		IL_0021: ldnull
		IL_0022: throw
	} // end of method VertexShakeB::OnDisable

	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x198d8
		// Header size: 12
		// Code size: 47 (0x2f)
		.maxstack 2
		.locals init (
			[0] class TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10',
			[1] class [UnityEngine.CoreModule]UnityEngine.Coroutine
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(TMPro.Examples.VertexShakeB+<AnimateVertexColors>d__10)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class TMPro.Examples.VertexShakeB TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<>4__this'
		IL_0026: ldarg.0
		IL_0027: ldloc.0
		IL_0028: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Coroutine [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StartCoroutine(class [mscorlib]System.Collections.IEnumerator)
		IL_002d: stloc.1
		IL_002e: ret
	} // end of method VertexShakeB::Start

	.method private hidebysig 
		instance void ON_TEXT_CHANGED (
			class [UnityEngine.CoreModule]UnityEngine.Object obj
		) cil managed 
	{
		// Method begins at RVA 0x19914
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
		IL_0002: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.VertexShakeB::m_TextComponent
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
		IL_001e: stfld bool TMPro.Examples.VertexShakeB::hasTextChanged

		IL_0023: nop
		IL_0024: ret
	} // end of method VertexShakeB::ON_TEXT_CHANGED

	.method private hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator AnimateVertexColors () cil managed 
	{
		// Method begins at RVA 0x19948
		// Header size: 12
		// Code size: 40 (0x28)
		.maxstack 2
		.locals init (
			[0] class TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(TMPro.Examples.VertexShakeB+<AnimateVertexColors>d__10)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class TMPro.Examples.VertexShakeB TMPro.Examples.VertexShakeB/'<AnimateVertexColors>d__10'::'<>4__this'
		IL_0026: ldloc.0
		IL_0027: ret
	} // end of method VertexShakeB::AnimateVertexColors

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2ad9
		// Header size: 1
		// Code size: 41 (0x29)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.r4 1
		IL_0007: stfld float32 TMPro.Examples.VertexShakeB::AngleMultiplier
		IL_000c: ldarg.0
		IL_000d: ldc.r4 1
		IL_0012: stfld float32 TMPro.Examples.VertexShakeB::SpeedMultiplier
		IL_0017: ldarg.0
		IL_0018: ldc.r4 1
		IL_001d: stfld float32 TMPro.Examples.VertexShakeB::CurveScale
		IL_0022: ldarg.0
		IL_0023: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0028: ret
	} // end of method VertexShakeB::.ctor

} // end of class TMPro.Examples.VertexShakeB
