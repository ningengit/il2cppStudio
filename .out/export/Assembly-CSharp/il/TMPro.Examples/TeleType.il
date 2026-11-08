.class public auto ansi beforefieldinit TMPro.Examples.TeleType
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested private auto ansi sealed beforefieldinit '<Start>d__4'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerator`1<object>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private object '<>2__current'
		.field public class TMPro.Examples.TeleType '<>4__this'
		.field private int32 '<totalVisibleCharacters>5__2'
		.field private int32 '<counter>5__3'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x2dfa
			// Header size: 1
			// Code size: 16 (0x10)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ldarg.0
			IL_0009: ldarg.1
			IL_000a: stfld int32 TMPro.Examples.TeleType/'<Start>d__4'::'<>1__state'
			IL_000f: ret
		} // end of method '<Start>d__4'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			// Method begins at RVA 0x2e0b
			// Header size: 1
			// Code size: 2 (0x2)
			.maxstack 8

			IL_0000: nop
			IL_0001: ret
		} // end of method '<Start>d__4'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			// Method begins at RVA 0x1c1dc
			// Header size: 12
			// Code size: 352 (0x160)
			.maxstack 3
			.locals init (
				[0] class TMPro.Examples.TeleType,
				[1] int32,
				[2] string,
				[3] class [UnityEngine.CoreModule]UnityEngine.WaitForSeconds,
				[4] string,
				[5] class [UnityEngine.CoreModule]UnityEngine.WaitForSeconds,
				[6] class [UnityEngine.CoreModule]UnityEngine.WaitForSeconds,
				[7] class [UnityEngine.CoreModule]UnityEngine.WaitForSeconds
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld class TMPro.Examples.TeleType TMPro.Examples.TeleType/'<Start>d__4'::'<>4__this'
			IL_0007: stloc.0
			IL_0008: ldarg.0
			IL_0009: ldfld int32 TMPro.Examples.TeleType/'<Start>d__4'::'<>1__state'
			IL_000e: stloc.1
			IL_000f: ldarg.0
			IL_0010: ldfld int32 TMPro.Examples.TeleType/'<Start>d__4'::'<>1__state'
			IL_0015: ldc.i4.0
			IL_0016: ceq
			IL_0018: brtrue IL_00bf

			IL_001d: nop
			IL_001e: ldloc.1
			IL_001f: ldc.i4.1
			IL_0020: sub
			IL_0021: stloc.1
			IL_0022: ldloc.1
			IL_0023: ldc.i4.1
			IL_0024: sub
			IL_0025: ldc.i4.0
			IL_0026: ceq
			IL_0028: brtrue.s IL_0092

			IL_002a: nop
			IL_002b: ldloc.1
			IL_002c: ldc.i4.1
			IL_002d: sub
			IL_002e: stloc.1
			IL_002f: ldloc.1
			IL_0030: ldc.i4.1
			IL_0031: sub
			IL_0032: ldc.i4.0
			IL_0033: ceq
			IL_0035: brtrue.s IL_0068

			IL_0037: nop
			IL_0038: ldloc.1
			IL_0039: ldc.i4.1
			IL_003a: sub
			IL_003b: stloc.1
			IL_003c: ldloc.1
			IL_003d: ldc.i4.1
			IL_003e: sub
			IL_003f: ldc.i4.0
			IL_0040: ceq
			IL_0042: brtrue.s IL_005b

			IL_0044: nop
			IL_0045: ldloc.1
			IL_0046: ldc.i4.1
			IL_0047: ceq
			IL_0049: brtrue.s IL_004e

			IL_004b: nop
			IL_004c: ldc.i4.0
			IL_004d: ret

			IL_004e: nop
			IL_004f: ldarg.0
			IL_0050: ldc.i4.m1
			IL_0051: stfld int32 TMPro.Examples.TeleType/'<Start>d__4'::'<>1__state'
			IL_0056: br IL_00e4

			IL_005b: nop
			IL_005c: ldarg.0
			IL_005d: ldc.i4.m1
			IL_005e: stfld int32 TMPro.Examples.TeleType/'<Start>d__4'::'<>1__state'
			IL_0063: br IL_0113

			IL_0068: nop
			IL_0069: ldarg.0
			IL_006a: ldc.i4.m1
			IL_006b: stfld int32 TMPro.Examples.TeleType/'<Start>d__4'::'<>1__state'
			IL_0070: ldloc.0
			IL_0071: ldfld string TMPro.Examples.TeleType::label01
			IL_0076: stloc.2
			IL_0077: ldc.r4 1
			IL_007c: newobj instance void [UnityEngine.CoreModule]UnityEngine.WaitForSeconds::.ctor(float32)
			IL_0081: stloc.3
			IL_0082: ldarg.0
			IL_0083: ldloc.3
			IL_0084: stfld object TMPro.Examples.TeleType/'<Start>d__4'::'<>2__current'
			IL_0089: ldarg.0
			IL_008a: ldc.i4.3
			IL_008b: stfld int32 TMPro.Examples.TeleType/'<Start>d__4'::'<>1__state'
			IL_0090: ldc.i4.1
			IL_0091: ret

			IL_0092: nop
			IL_0093: ldarg.0
			IL_0094: ldc.i4.m1
			IL_0095: stfld int32 TMPro.Examples.TeleType/'<Start>d__4'::'<>1__state'
			IL_009a: ldloc.0
			IL_009b: ldfld string TMPro.Examples.TeleType::label02
			IL_00a0: stloc.s 4
			IL_00a2: ldc.r4 1
			IL_00a7: newobj instance void [UnityEngine.CoreModule]UnityEngine.WaitForSeconds::.ctor(float32)
			IL_00ac: stloc.s 5
			IL_00ae: ldarg.0
			IL_00af: ldloc.s 5
			IL_00b1: stfld object TMPro.Examples.TeleType/'<Start>d__4'::'<>2__current'
			IL_00b6: ldarg.0
			IL_00b7: ldc.i4.2
			IL_00b8: stfld int32 TMPro.Examples.TeleType/'<Start>d__4'::'<>1__state'
			IL_00bd: ldc.i4.1
			IL_00be: ret

			IL_00bf: nop
			IL_00c0: ldarg.0
			IL_00c1: ldc.i4.m1
			IL_00c2: stfld int32 TMPro.Examples.TeleType/'<Start>d__4'::'<>1__state'
			IL_00c7: ldarg.0
			IL_00c8: ldloc.0
			IL_00c9: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.TeleType::m_textMeshPro
			IL_00ce: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo [Unity.TextMeshPro]TMPro.TMP_Text::m_textInfo
			IL_00d3: ldfld int32 [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterCount
			IL_00d8: stfld int32 TMPro.Examples.TeleType/'<Start>d__4'::'<totalVisibleCharacters>5__2'
			IL_00dd: ldarg.0
			IL_00de: ldc.i4.0
			IL_00df: stfld int32 TMPro.Examples.TeleType/'<Start>d__4'::'<counter>5__3'

			IL_00e4: nop
			IL_00e5: ldloc.0
			IL_00e6: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.TeleType::m_textMeshPro
			IL_00eb: ldstr "rdx"
			IL_00f0: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_00f5: conv.i4
			IL_00f6: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_maxVisibleCharacters(int32)
			IL_00fb: ldstr "rdx"
			IL_0100: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0105: conv.i4
			IL_0106: ldarg.0
			IL_0107: ldfld int32 TMPro.Examples.TeleType/'<Start>d__4'::'<totalVisibleCharacters>5__2'
			IL_010c: clt
			IL_010e: ldc.i4.0
			IL_010f: ceq
			IL_0111: brtrue.s IL_013f

			IL_0113: nop
			IL_0114: ldarg.0
			IL_0115: ldarg.0
			IL_0116: ldfld int32 TMPro.Examples.TeleType/'<Start>d__4'::'<counter>5__3'
			IL_011b: ldc.i4.1
			IL_011c: add
			IL_011d: stfld int32 TMPro.Examples.TeleType/'<Start>d__4'::'<counter>5__3'
			IL_0122: ldc.r4 0.05
			IL_0127: newobj instance void [UnityEngine.CoreModule]UnityEngine.WaitForSeconds::.ctor(float32)
			IL_012c: stloc.s 6
			IL_012e: ldarg.0
			IL_012f: ldloc.s 6
			IL_0131: stfld object TMPro.Examples.TeleType/'<Start>d__4'::'<>2__current'
			IL_0136: ldarg.0
			IL_0137: ldc.i4.4
			IL_0138: stfld int32 TMPro.Examples.TeleType/'<Start>d__4'::'<>1__state'
			IL_013d: ldc.i4.1
			IL_013e: ret

			IL_013f: nop
			IL_0140: ldc.r4 1
			IL_0145: newobj instance void [UnityEngine.CoreModule]UnityEngine.WaitForSeconds::.ctor(float32)
			IL_014a: stloc.s 7
			IL_014c: ldarg.0
			IL_014d: ldloc.s 7
			IL_014f: stfld object TMPro.Examples.TeleType/'<Start>d__4'::'<>2__current'
			IL_0154: ldarg.0
			IL_0155: ldc.i4.1
			IL_0156: stfld int32 TMPro.Examples.TeleType/'<Start>d__4'::'<>1__state'
			IL_015b: ldc.i4.1
			IL_015c: ret

			IL_015d: nop
			IL_015e: ldnull
			IL_015f: throw
		} // end of method '<Start>d__4'::MoveNext

		.method private final hidebysig specialname newslot virtual 
			instance object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () cil managed 
		{
			// Method begins at RVA 0x2e0e
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.TeleType/'<Start>d__4'::'<>2__current'
			IL_0007: ret
		} // end of method '<Start>d__4'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'

		.method private final hidebysig newslot virtual 
			instance void System.Collections.IEnumerator.Reset () cil managed 
		{
			// Method begins at RVA 0x1c348
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
		} // end of method '<Start>d__4'::System.Collections.IEnumerator.Reset

		.method private final hidebysig specialname newslot virtual 
			instance object System.Collections.IEnumerator.get_Current () cil managed 
		{
			// Method begins at RVA 0x2e17
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.TeleType/'<Start>d__4'::'<>2__current'
			IL_0007: ret
		} // end of method '<Start>d__4'::System.Collections.IEnumerator.get_Current

		// Properties
		.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current'()
		{
			.get instance object TMPro.Examples.TeleType/'<Start>d__4'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'()
		}
		.property instance object System.Collections.IEnumerator.Current()
		{
			.get instance object TMPro.Examples.TeleType/'<Start>d__4'::System.Collections.IEnumerator.get_Current()
		}

	} // end of class <Start>d__4


	// Fields
	.field private string label01
	.field private string label02
	.field private class [Unity.TextMeshPro]TMPro.TMP_Text m_textMeshPro

	// Methods
	.method private hidebysig 
		instance void Awake () cil managed 
	{
		// Method begins at RVA 0x166a4
		// Header size: 12
		// Code size: 57 (0x39)
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
		IL_000a: stfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.TeleType::m_textMeshPro
		IL_000f: ldarg.0
		IL_0010: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.TeleType::m_textMeshPro
		IL_0015: ldc.i4.1
		IL_0016: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_enableWordWrapping(bool)
		IL_001b: ldarg.0
		IL_001c: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.TeleType::m_textMeshPro
		IL_0021: ldc.i4 258
		IL_0026: box [mscorlib]System.Int32
		IL_002b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions>(object)
		IL_0030: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_alignment(valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions)
		IL_0035: ret

		IL_0036: nop
		IL_0037: ldnull
		IL_0038: throw
	} // end of method TeleType::Awake

	.method private hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator Start () cil managed 
	{
		// Method begins at RVA 0x166ec
		// Header size: 12
		// Code size: 40 (0x28)
		.maxstack 2
		.locals init (
			[0] class TMPro.Examples.TeleType/'<Start>d__4'
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(TMPro.Examples.TeleType+<Start>d__4)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.TeleType/'<Start>d__4'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 TMPro.Examples.TeleType/'<Start>d__4'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class TMPro.Examples.TeleType TMPro.Examples.TeleType/'<Start>d__4'::'<>4__this'
		IL_0026: ldloc.0
		IL_0027: ret
	} // end of method TeleType::Start

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2922
		// Header size: 1
		// Code size: 30 (0x1e)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldstr "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=1>"
		IL_0007: stfld string TMPro.Examples.TeleType::label01
		IL_000c: ldarg.0
		IL_000d: ldstr "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=2>"
		IL_0012: stfld string TMPro.Examples.TeleType::label02
		IL_0017: ldarg.0
		IL_0018: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_001d: ret
	} // end of method TeleType::.ctor

} // end of class TMPro.Examples.TeleType
