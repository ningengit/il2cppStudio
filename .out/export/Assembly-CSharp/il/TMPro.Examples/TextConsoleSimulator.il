.class public auto ansi beforefieldinit TMPro.Examples.TextConsoleSimulator
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested private auto ansi sealed beforefieldinit '<RevealCharacters>d__7'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerator`1<object>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private object '<>2__current'
		.field public class [Unity.TextMeshPro]TMPro.TMP_Text textComponent
		.field public class TMPro.Examples.TextConsoleSimulator '<>4__this'
		.field private class [Unity.TextMeshPro]TMPro.TMP_TextInfo '<textInfo>5__2'
		.field private int32 '<totalVisibleCharacters>5__3'
		.field private int32 '<visibleCount>5__4'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x2e20
			// Header size: 1
			// Code size: 16 (0x10)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ldarg.0
			IL_0009: ldarg.1
			IL_000a: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<>1__state'
			IL_000f: ret
		} // end of method '<RevealCharacters>d__7'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			// Method begins at RVA 0x2e31
			// Header size: 1
			// Code size: 2 (0x2)
			.maxstack 8

			IL_0000: nop
			IL_0001: ret
		} // end of method '<RevealCharacters>d__7'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			// Method begins at RVA 0x1c398
			// Header size: 12
			// Code size: 273 (0x111)
			.maxstack 3
			.locals init (
				[0] class TMPro.Examples.TextConsoleSimulator,
				[1] int32,
				[2] class [Unity.TextMeshPro]TMPro.TMP_Text,
				[3] class [Unity.TextMeshPro]TMPro.TMP_Text,
				[4] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[5] class [UnityEngine.CoreModule]UnityEngine.WaitForSeconds
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld class TMPro.Examples.TextConsoleSimulator TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<>4__this'
			IL_0007: stloc.0
			IL_0008: ldarg.0
			IL_0009: ldfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<>1__state'
			IL_000e: stloc.1
			IL_000f: ldarg.0
			IL_0010: ldfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<>1__state'
			IL_0015: ldc.i4.0
			IL_0016: ceq
			IL_0018: brtrue.s IL_004c

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
			IL_002c: brtrue.s IL_0031

			IL_002e: nop
			IL_002f: ldc.i4.0
			IL_0030: ret

			IL_0031: nop
			IL_0032: ldarg.0
			IL_0033: ldc.i4.m1
			IL_0034: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<>1__state'
			IL_0039: br.s IL_008a

			IL_003b: nop
			IL_003c: ldarg.0
			IL_003d: ldc.i4.m1
			IL_003e: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<>1__state'
			IL_0043: ldarg.0
			IL_0044: ldc.i4.0
			IL_0045: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<visibleCount>5__4'
			IL_004a: br.s IL_00c0

			IL_004c: nop
			IL_004d: ldarg.0
			IL_004e: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::textComponent
			IL_0053: stloc.2
			IL_0054: ldarg.0
			IL_0055: ldc.i4.m1
			IL_0056: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<>1__state'
			IL_005b: ldarg.0
			IL_005c: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::textComponent
			IL_0061: stloc.3
			IL_0062: ldloc.3
			IL_0063: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo [Unity.TextMeshPro]TMPro.TMP_Text::m_textInfo
			IL_0068: stloc.s 4
			IL_006a: ldarg.0
			IL_006b: ldloc.s 4
			IL_006d: stfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<textInfo>5__2'
			IL_0072: ldarg.0
			IL_0073: ldarg.0
			IL_0074: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<textInfo>5__2'
			IL_0079: ldfld int32 [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterCount
			IL_007e: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<totalVisibleCharacters>5__3'
			IL_0083: ldarg.0
			IL_0084: ldc.i4.0
			IL_0085: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<visibleCount>5__4'

			IL_008a: nop
			IL_008b: ldloc.0
			IL_008c: ldfld bool TMPro.Examples.TextConsoleSimulator::hasTextChanged
			IL_0091: ldc.i4.0
			IL_0092: ceq
			IL_0094: brtrue.s IL_00af

			IL_0096: nop
			IL_0097: ldarg.0
			IL_0098: ldarg.0
			IL_0099: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<textInfo>5__2'
			IL_009e: ldfld int32 [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterCount
			IL_00a3: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<totalVisibleCharacters>5__3'
			IL_00a8: ldloc.0
			IL_00a9: ldc.i4.0
			IL_00aa: stfld bool TMPro.Examples.TextConsoleSimulator::hasTextChanged

			IL_00af: nop
			IL_00b0: ldarg.0
			IL_00b1: ldfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<visibleCount>5__4'
			IL_00b6: ldarg.0
			IL_00b7: ldfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<totalVisibleCharacters>5__3'
			IL_00bc: cgt
			IL_00be: brtrue.s IL_00f0

			IL_00c0: nop
			IL_00c1: ldarg.0
			IL_00c2: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::textComponent
			IL_00c7: ldarg.0
			IL_00c8: ldfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<visibleCount>5__4'
			IL_00cd: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_maxVisibleCharacters(int32)
			IL_00d2: ldarg.0
			IL_00d3: ldarg.0
			IL_00d4: ldfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<visibleCount>5__4'
			IL_00d9: ldc.i4.1
			IL_00da: add
			IL_00db: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<visibleCount>5__4'
			IL_00e0: ldarg.0
			IL_00e1: ldnull
			IL_00e2: stfld object TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<>2__current'
			IL_00e7: ldarg.0
			IL_00e8: ldc.i4.2
			IL_00e9: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<>1__state'
			IL_00ee: ldc.i4.1
			IL_00ef: ret

			IL_00f0: nop
			IL_00f1: ldc.r4 1
			IL_00f6: newobj instance void [UnityEngine.CoreModule]UnityEngine.WaitForSeconds::.ctor(float32)
			IL_00fb: stloc.s 5
			IL_00fd: ldarg.0
			IL_00fe: ldloc.s 5
			IL_0100: stfld object TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<>2__current'
			IL_0105: ldarg.0
			IL_0106: ldc.i4.1
			IL_0107: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<>1__state'
			IL_010c: ldc.i4.1
			IL_010d: ret

			IL_010e: nop
			IL_010f: ldnull
			IL_0110: throw
		} // end of method '<RevealCharacters>d__7'::MoveNext

		.method private final hidebysig specialname newslot virtual 
			instance object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () cil managed 
		{
			// Method begins at RVA 0x2e34
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<>2__current'
			IL_0007: ret
		} // end of method '<RevealCharacters>d__7'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'

		.method private final hidebysig newslot virtual 
			instance void System.Collections.IEnumerator.Reset () cil managed 
		{
			// Method begins at RVA 0x1c4b8
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
		} // end of method '<RevealCharacters>d__7'::System.Collections.IEnumerator.Reset

		.method private final hidebysig specialname newslot virtual 
			instance object System.Collections.IEnumerator.get_Current () cil managed 
		{
			// Method begins at RVA 0x2e3d
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<>2__current'
			IL_0007: ret
		} // end of method '<RevealCharacters>d__7'::System.Collections.IEnumerator.get_Current

		// Properties
		.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current'()
		{
			.get instance object TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'()
		}
		.property instance object System.Collections.IEnumerator.Current()
		{
			.get instance object TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::System.Collections.IEnumerator.get_Current()
		}

	} // end of class <RevealCharacters>d__7

	.class nested private auto ansi sealed beforefieldinit '<RevealWords>d__8'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerator`1<object>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private object '<>2__current'
		.field public class [Unity.TextMeshPro]TMPro.TMP_Text textComponent
		.field private int32 '<totalWordCount>5__2'
		.field private int32 '<totalVisibleCharacters>5__3'
		.field private int32 '<counter>5__4'
		.field private int32 '<visibleCount>5__5'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x2e46
			// Header size: 1
			// Code size: 16 (0x10)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ldarg.0
			IL_0009: ldarg.1
			IL_000a: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<>1__state'
			IL_000f: ret
		} // end of method '<RevealWords>d__8'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			// Method begins at RVA 0x2e57
			// Header size: 1
			// Code size: 2 (0x2)
			.maxstack 8

			IL_0000: nop
			IL_0001: ret
		} // end of method '<RevealWords>d__8'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			// Method begins at RVA 0x1c508
			// Header size: 12
			// Code size: 365 (0x16d)
			.maxstack 3
			.locals init (
				[0] int32,
				[1] class [Unity.TextMeshPro]TMPro.TMP_Text,
				[2] class [UnityEngine.CoreModule]UnityEngine.WaitForSeconds,
				[3] class [UnityEngine.CoreModule]UnityEngine.WaitForSeconds
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<>1__state'
			IL_0007: stloc.0
			IL_0008: ldarg.0
			IL_0009: ldfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<>1__state'
			IL_000e: ldc.i4.0
			IL_000f: ceq
			IL_0011: brtrue.s IL_0041

			IL_0013: nop
			IL_0014: ldloc.0
			IL_0015: ldc.i4.1
			IL_0016: sub
			IL_0017: stloc.0
			IL_0018: ldloc.0
			IL_0019: ldc.i4.1
			IL_001a: sub
			IL_001b: ldc.i4.0
			IL_001c: ceq
			IL_001e: brtrue.s IL_0034

			IL_0020: nop
			IL_0021: ldloc.0
			IL_0022: ldc.i4.1
			IL_0023: ceq
			IL_0025: brtrue.s IL_002a

			IL_0027: nop
			IL_0028: ldc.i4.0
			IL_0029: ret

			IL_002a: nop
			IL_002b: ldarg.0
			IL_002c: ldc.i4.m1
			IL_002d: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<>1__state'
			IL_0032: br.s IL_0083

			IL_0034: nop
			IL_0035: ldarg.0
			IL_0036: ldc.i4.m1
			IL_0037: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<>1__state'
			IL_003c: br IL_0121

			IL_0041: nop
			IL_0042: ldarg.0
			IL_0043: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::textComponent
			IL_0048: stloc.1
			IL_0049: ldarg.0
			IL_004a: ldc.i4.m1
			IL_004b: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<>1__state'
			IL_0050: ldarg.0
			IL_0051: ldarg.0
			IL_0052: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::textComponent
			IL_0057: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo [Unity.TextMeshPro]TMPro.TMP_Text::m_textInfo
			IL_005c: ldfld int32 [Unity.TextMeshPro]TMPro.TMP_TextInfo::wordCount
			IL_0061: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<totalWordCount>5__2'
			IL_0066: ldarg.0
			IL_0067: ldarg.0
			IL_0068: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::textComponent
			IL_006d: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo [Unity.TextMeshPro]TMPro.TMP_Text::m_textInfo
			IL_0072: ldfld int32 [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterCount
			IL_0077: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<totalVisibleCharacters>5__3'
			IL_007c: ldarg.0
			IL_007d: ldc.i4.0
			IL_007e: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<counter>5__4'

			IL_0083: nop
			IL_0084: ldstr "rdx"
			IL_0089: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_008e: conv.i4
			IL_008f: ldstr "rdx"
			IL_0094: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0099: conv.i4
			IL_009a: and
			IL_009b: ldc.i4.0
			IL_009c: ceq
			IL_009e: brtrue.s IL_00f4

			IL_00a0: nop
			IL_00a1: ldstr "method"
			IL_00a6: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_00ab: conv.i4
			IL_00ac: ldarg.0
			IL_00ad: ldfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<totalWordCount>5__2'
			IL_00b2: clt
			IL_00b4: brtrue.s IL_00de

			IL_00b6: nop
			IL_00b7: ldstr "method"
			IL_00bc: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_00c1: conv.i4
			IL_00c2: ldarg.0
			IL_00c3: ldfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<totalWordCount>5__2'
			IL_00c8: ceq
			IL_00ca: ldc.i4.0
			IL_00cb: ceq
			IL_00cd: brtrue.s IL_00fc

			IL_00cf: nop
			IL_00d0: ldarg.0
			IL_00d1: ldarg.0
			IL_00d2: ldfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<totalVisibleCharacters>5__3'
			IL_00d7: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<visibleCount>5__5'
			IL_00dc: br.s IL_00fc

			IL_00de: nop
			IL_00df: ldarg.0
			IL_00e0: ldstr "*(FieldExpr + 0x2C)"
			IL_00e5: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_00ea: conv.i4
			IL_00eb: ldc.i4.1
			IL_00ec: add
			IL_00ed: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<visibleCount>5__5'
			IL_00f2: br.s IL_00fc

			IL_00f4: nop
			IL_00f5: ldarg.0
			IL_00f6: ldc.i4.0
			IL_00f7: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<visibleCount>5__5'

			IL_00fc: nop
			IL_00fd: ldarg.0
			IL_00fe: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::textComponent
			IL_0103: ldarg.0
			IL_0104: ldfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<visibleCount>5__5'
			IL_0109: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_maxVisibleCharacters(int32)
			IL_010e: ldarg.0
			IL_010f: ldfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<visibleCount>5__5'
			IL_0114: ldarg.0
			IL_0115: ldfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<totalVisibleCharacters>5__3'
			IL_011a: clt
			IL_011c: ldc.i4.0
			IL_011d: ceq
			IL_011f: brtrue.s IL_014b

			IL_0121: nop
			IL_0122: ldarg.0
			IL_0123: ldarg.0
			IL_0124: ldfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<counter>5__4'
			IL_0129: ldc.i4.1
			IL_012a: add
			IL_012b: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<counter>5__4'
			IL_0130: ldc.r4 0.1
			IL_0135: newobj instance void [UnityEngine.CoreModule]UnityEngine.WaitForSeconds::.ctor(float32)
			IL_013a: stloc.2
			IL_013b: ldarg.0
			IL_013c: ldloc.2
			IL_013d: stfld object TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<>2__current'
			IL_0142: ldarg.0
			IL_0143: ldc.i4.2
			IL_0144: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<>1__state'
			IL_0149: ldc.i4.1
			IL_014a: ret

			IL_014b: nop
			IL_014c: ldc.r4 1
			IL_0151: newobj instance void [UnityEngine.CoreModule]UnityEngine.WaitForSeconds::.ctor(float32)
			IL_0156: stloc.3
			IL_0157: ldarg.0
			IL_0158: ldloc.3
			IL_0159: stfld object TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<>2__current'
			IL_015e: ldarg.0
			IL_015f: ldc.i4.1
			IL_0160: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<>1__state'
			IL_0165: ldc.i4.1
			IL_0166: ret

			IL_0167: nop
			IL_0168: ldnull
			IL_0169: throw

			IL_016a: nop
			IL_016b: ldnull
			IL_016c: throw
		} // end of method '<RevealWords>d__8'::MoveNext

		.method private final hidebysig specialname newslot virtual 
			instance object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () cil managed 
		{
			// Method begins at RVA 0x2e5a
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<>2__current'
			IL_0007: ret
		} // end of method '<RevealWords>d__8'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'

		.method private final hidebysig newslot virtual 
			instance void System.Collections.IEnumerator.Reset () cil managed 
		{
			// Method begins at RVA 0x1c684
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
		} // end of method '<RevealWords>d__8'::System.Collections.IEnumerator.Reset

		.method private final hidebysig specialname newslot virtual 
			instance object System.Collections.IEnumerator.get_Current () cil managed 
		{
			// Method begins at RVA 0x2e63
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<>2__current'
			IL_0007: ret
		} // end of method '<RevealWords>d__8'::System.Collections.IEnumerator.get_Current

		// Properties
		.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current'()
		{
			.get instance object TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'()
		}
		.property instance object System.Collections.IEnumerator.Current()
		{
			.get instance object TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::System.Collections.IEnumerator.get_Current()
		}

	} // end of class <RevealWords>d__8


	// Fields
	.field private class [Unity.TextMeshPro]TMPro.TMP_Text m_TextComponent
	.field private bool hasTextChanged

	// Methods
	.method private hidebysig 
		instance void Awake () cil managed 
	{
		// Method begins at RVA 0x16720
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
		IL_0011: stfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.TextConsoleSimulator::m_TextComponent
		IL_0016: ret

		IL_0017: nop
		IL_0018: ldnull
		IL_0019: throw
	} // end of method TextConsoleSimulator::Awake

	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x16748
		// Header size: 12
		// Code size: 61 (0x3d)
		.maxstack 2
		.locals init (
			[0] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[1] class TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7',
			[2] class [UnityEngine.CoreModule]UnityEngine.Coroutine
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.TextConsoleSimulator::m_TextComponent
		IL_0007: stloc.0
		IL_0008: ldstr "il2cpp_codegen_object_new(TMPro.Examples.TextConsoleSimulator+<RevealCharacters>d__7)"
		IL_000d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0012: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'>(object)
		IL_0017: stloc.1
		IL_0018: ldloc.1
		IL_0019: ldnull
		IL_001a: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_001f: ldloc.1
		IL_0020: ldc.i4.0
		IL_0021: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<>1__state'
		IL_0026: ldloc.1
		IL_0027: ldarg.0
		IL_0028: stfld class TMPro.Examples.TextConsoleSimulator TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<>4__this'
		IL_002d: ldloc.1
		IL_002e: ldloc.0
		IL_002f: stfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::textComponent
		IL_0034: ldarg.0
		IL_0035: ldloc.1
		IL_0036: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Coroutine [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StartCoroutine(class [mscorlib]System.Collections.IEnumerator)
		IL_003b: stloc.2
		IL_003c: ret
	} // end of method TextConsoleSimulator::Start

	.method private hidebysig 
		instance void OnEnable () cil managed 
	{
		// Method begins at RVA 0x16794
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
		IL_000c: ldftn instance void TMPro.Examples.TextConsoleSimulator::ON_TEXT_CHANGED(class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0012: newobj instance void [System]System.IOAsyncCallback::.ctor(object, native int)
		IL_0017: stloc.1
		IL_0018: ldloc.0
		IL_0019: ldloc.1
		IL_001a: callvirt instance void class [Unity.TextMeshPro]TMPro.FastAction`1<class [UnityEngine.CoreModule]UnityEngine.Object>::Add(class [mscorlib]System.Action`1<!0>)
		IL_001f: ret

		IL_0020: nop
		IL_0021: ldnull
		IL_0022: throw
	} // end of method TextConsoleSimulator::OnEnable

	.method private hidebysig 
		instance void OnDisable () cil managed 
	{
		// Method begins at RVA 0x167c4
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
		IL_000c: ldftn instance void TMPro.Examples.TextConsoleSimulator::ON_TEXT_CHANGED(class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0012: newobj instance void [System]System.IOAsyncCallback::.ctor(object, native int)
		IL_0017: stloc.1
		IL_0018: ldloc.0
		IL_0019: ldloc.1
		IL_001a: callvirt instance void class [Unity.TextMeshPro]TMPro.FastAction`1<class [UnityEngine.CoreModule]UnityEngine.Object>::Remove(class [mscorlib]System.Action`1<!0>)
		IL_001f: ret

		IL_0020: nop
		IL_0021: ldnull
		IL_0022: throw
	} // end of method TextConsoleSimulator::OnDisable

	.method private hidebysig 
		instance void ON_TEXT_CHANGED (
			class [UnityEngine.CoreModule]UnityEngine.Object obj
		) cil managed 
	{
		// Method begins at RVA 0x2941
		// Header size: 1
		// Code size: 9 (0x9)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i4.1
		IL_0003: stfld bool TMPro.Examples.TextConsoleSimulator::hasTextChanged
		IL_0008: ret
	} // end of method TextConsoleSimulator::ON_TEXT_CHANGED

	.method private hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator RevealCharacters (
			class [Unity.TextMeshPro]TMPro.TMP_Text textComponent
		) cil managed 
	{
		// Method begins at RVA 0x167f4
		// Header size: 12
		// Code size: 47 (0x2f)
		.maxstack 2
		.locals init (
			[0] class TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(TMPro.Examples.TextConsoleSimulator+<RevealCharacters>d__7)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class TMPro.Examples.TextConsoleSimulator TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::'<>4__this'
		IL_0026: ldloc.0
		IL_0027: ldarg.1
		IL_0028: stfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.TextConsoleSimulator/'<RevealCharacters>d__7'::textComponent
		IL_002d: ldloc.0
		IL_002e: ret
	} // end of method TextConsoleSimulator::RevealCharacters

	.method private hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator RevealWords (
			class [Unity.TextMeshPro]TMPro.TMP_Text textComponent
		) cil managed 
	{
		// Method begins at RVA 0x16830
		// Header size: 12
		// Code size: 40 (0x28)
		.maxstack 2
		.locals init (
			[0] class TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(TMPro.Examples.TextConsoleSimulator+<RevealWords>d__8)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.1
		IL_0021: stfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.TextConsoleSimulator/'<RevealWords>d__8'::textComponent
		IL_0026: ldloc.0
		IL_0027: ret
	} // end of method TextConsoleSimulator::RevealWords

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x294b
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method TextConsoleSimulator::.ctor

} // end of class TMPro.Examples.TextConsoleSimulator
