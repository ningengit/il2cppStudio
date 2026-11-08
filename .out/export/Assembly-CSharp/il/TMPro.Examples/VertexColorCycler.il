.class public auto ansi beforefieldinit TMPro.Examples.VertexColorCycler
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested private auto ansi sealed beforefieldinit '<AnimateVertexColors>d__3'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerator`1<object>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private object '<>2__current'
		.field public class TMPro.Examples.VertexColorCycler '<>4__this'
		.field private class [Unity.TextMeshPro]TMPro.TMP_TextInfo '<textInfo>5__2'
		.field private int32 '<currentCharacter>5__3'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x2eb8
			// Header size: 1
			// Code size: 16 (0x10)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ldarg.0
			IL_0009: ldarg.1
			IL_000a: stfld int32 TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'::'<>1__state'
			IL_000f: ret
		} // end of method '<AnimateVertexColors>d__3'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			// Method begins at RVA 0x2ec9
			// Header size: 1
			// Code size: 2 (0x2)
			.maxstack 8

			IL_0000: nop
			IL_0001: ret
		} // end of method '<AnimateVertexColors>d__3'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			// Method begins at RVA 0x1d30c
			// Header size: 12
			// Code size: 383 (0x17f)
			.maxstack 2
			.locals init (
				[0] class TMPro.Examples.VertexColorCycler,
				[1] int32,
				[2] class [Unity.TextMeshPro]TMPro.TMP_Text,
				[3] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[4] valuetype [UnityEngine.CoreModule]UnityEngine.Color,
				[5] valuetype [UnityEngine.CoreModule]UnityEngine.Color32,
				[6] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[7] int32,
				[8] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[],
				[9] int64,
				[10] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
				[11] int32,
				[12] int32,
				[13] int32,
				[14] class [UnityEngine.CoreModule]UnityEngine.WaitForSeconds,
				[15] class [UnityEngine.CoreModule]UnityEngine.WaitForSeconds
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld class TMPro.Examples.VertexColorCycler TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'::'<>4__this'
			IL_0007: stloc.0
			IL_0008: ldarg.0
			IL_0009: ldfld int32 TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'::'<>1__state'
			IL_000e: stloc.1
			IL_000f: ldarg.0
			IL_0010: ldfld int32 TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'::'<>1__state'
			IL_0015: ldc.i4.0
			IL_0016: ceq
			IL_0018: brtrue.s IL_003e

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
			IL_0025: brtrue.s IL_0034

			IL_0027: nop
			IL_0028: ldloc.1
			IL_0029: ldc.i4.1
			IL_002a: ceq
			IL_002c: brtrue.s IL_0034

			IL_002e: nop
			IL_002f: br IL_0158

			IL_0034: nop
			IL_0035: ldarg.0
			IL_0036: ldc.i4.m1
			IL_0037: stfld int32 TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'::'<>1__state'
			IL_003c: br.s IL_008d

			IL_003e: nop
			IL_003f: ldarg.0
			IL_0040: ldc.i4.m1
			IL_0041: stfld int32 TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'::'<>1__state'
			IL_0046: ldloc.0
			IL_0047: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.VertexColorCycler::m_TextComponent
			IL_004c: stloc.2
			IL_004d: ldloc.2
			IL_004e: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo [Unity.TextMeshPro]TMPro.TMP_Text::m_textInfo
			IL_0053: stloc.3
			IL_0054: ldarg.0
			IL_0055: ldloc.3
			IL_0056: stfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'::'<textInfo>5__2'
			IL_005b: ldarg.0
			IL_005c: ldc.i4.0
			IL_005d: stfld int32 TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'::'<currentCharacter>5__3'
			IL_0062: ldloc.0
			IL_0063: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.VertexColorCycler::m_TextComponent
			IL_0068: stloc.2
			IL_0069: ldloca.s 4
			IL_006b: ldstr "*(LocalExpr->klass + 0x0)"
			IL_0070: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0075: box [mscorlib]System.Int64
			IL_007a: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
			IL_007f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Color::r
			IL_0084: ldloc.s 4
			IL_0086: call valuetype [UnityEngine.CoreModule]UnityEngine.Color32 [UnityEngine.CoreModule]UnityEngine.Color32::op_Implicit(valuetype [UnityEngine.CoreModule]UnityEngine.Color)
			IL_008b: stloc.s 5

			IL_008d: nop
			IL_008e: ldarg.0
			IL_008f: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'::'<textInfo>5__2'
			IL_0094: stloc.s 6
			IL_0096: ldloc.s 6
			IL_0098: ldfld int32 [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterCount
			IL_009d: stloc.s 7
			IL_009f: ldloc.s 6
			IL_00a1: ldfld int32 [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterCount
			IL_00a6: ldc.i4.0
			IL_00a7: ceq
			IL_00a9: brtrue IL_015b

			IL_00ae: nop
			IL_00af: ldloc.s 6
			IL_00b1: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
			IL_00b6: stloc.s 8
			IL_00b8: ldstr "*(BinaryExpr + 0x58)"
			IL_00bd: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_00c2: conv.i4
			IL_00c3: conv.i8
			IL_00c4: stloc.s 9
			IL_00c6: ldloc.s 6
			IL_00c8: stloc.s 10
			IL_00ca: ldstr "*(BinaryExpr + 0x194)"
			IL_00cf: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_00d4: ldc.i4.0
			IL_00d5: conv.i8
			IL_00d6: ceq
			IL_00d8: brtrue.s IL_012a

			IL_00da: nop
			IL_00db: ldc.i4.0
			IL_00dc: ldc.i4 255
			IL_00e1: call int32 [UnityEngine.CoreModule]UnityEngine.Random::Range(int32, int32)
			IL_00e6: stloc.s 11
			IL_00e8: ldc.i4.0
			IL_00e9: ldc.i4 255
			IL_00ee: call int32 [UnityEngine.CoreModule]UnityEngine.Random::Range(int32, int32)
			IL_00f3: stloc.s 12
			IL_00f5: ldc.i4.0
			IL_00f6: ldc.i4 255
			IL_00fb: call int32 [UnityEngine.CoreModule]UnityEngine.Random::Range(int32, int32)
			IL_0100: stloc.s 13
			IL_0102: ldstr "mov [rbx+rbp*4+20h],eax ; value = LocalExpr"
			IL_0107: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
			IL_010c: ldstr "mov [rbx+rcx*4+20h],eax ; value = LocalExpr"
			IL_0111: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
			IL_0116: ldstr "mov [rbx+rcx*4+20h],eax ; value = LocalExpr"
			IL_011b: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
			IL_0120: ldstr "mov [rbx+rcx*4+20h],eax ; value = LocalExpr"
			IL_0125: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)

			IL_012a: nop
			IL_012b: ldarg.0
			IL_012c: ldstr "rdx"
			IL_0131: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0136: conv.i4
			IL_0137: stfld int32 TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'::'<currentCharacter>5__3'
			IL_013c: ldc.r4 0.05
			IL_0141: newobj instance void [UnityEngine.CoreModule]UnityEngine.WaitForSeconds::.ctor(float32)
			IL_0146: stloc.s 14
			IL_0148: ldarg.0
			IL_0149: ldloc.s 14
			IL_014b: stfld object TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'::'<>2__current'
			IL_0150: ldarg.0
			IL_0151: ldc.i4.2
			IL_0152: stfld int32 TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'::'<>1__state'

			IL_0157: nop

			IL_0158: nop
			IL_0159: ldc.i4.1
			IL_015a: ret

			IL_015b: nop
			IL_015c: ldc.r4 0.25
			IL_0161: newobj instance void [UnityEngine.CoreModule]UnityEngine.WaitForSeconds::.ctor(float32)
			IL_0166: stloc.s 15
			IL_0168: ldarg.0
			IL_0169: ldloc.s 15
			IL_016b: stfld object TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'::'<>2__current'
			IL_0170: ldarg.0
			IL_0171: ldc.i4.1
			IL_0172: stfld int32 TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'::'<>1__state'
			IL_0177: br.s IL_0157

			IL_0179: nop
			IL_017a: ldnull
			IL_017b: throw

			IL_017c: nop
			IL_017d: ldnull
			IL_017e: throw
		} // end of method '<AnimateVertexColors>d__3'::MoveNext

		.method private final hidebysig specialname newslot virtual 
			instance object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () cil managed 
		{
			// Method begins at RVA 0x2ecc
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'::'<>2__current'
			IL_0007: ret
		} // end of method '<AnimateVertexColors>d__3'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'

		.method private final hidebysig newslot virtual 
			instance void System.Collections.IEnumerator.Reset () cil managed 
		{
			// Method begins at RVA 0x1d498
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
		} // end of method '<AnimateVertexColors>d__3'::System.Collections.IEnumerator.Reset

		.method private final hidebysig specialname newslot virtual 
			instance object System.Collections.IEnumerator.get_Current () cil managed 
		{
			// Method begins at RVA 0x2ed5
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'::'<>2__current'
			IL_0007: ret
		} // end of method '<AnimateVertexColors>d__3'::System.Collections.IEnumerator.get_Current

		// Properties
		.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current'()
		{
			.get instance object TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'()
		}
		.property instance object System.Collections.IEnumerator.Current()
		{
			.get instance object TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'::System.Collections.IEnumerator.get_Current()
		}

	} // end of class <AnimateVertexColors>d__3


	// Fields
	.field private class [Unity.TextMeshPro]TMPro.TMP_Text m_TextComponent

	// Methods
	.method private hidebysig 
		instance void Awake () cil managed 
	{
		// Method begins at RVA 0x19590
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
		IL_000a: stfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.VertexColorCycler::m_TextComponent
		IL_000f: ret
	} // end of method VertexColorCycler::Awake

	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x195ac
		// Header size: 12
		// Code size: 47 (0x2f)
		.maxstack 2
		.locals init (
			[0] class TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3',
			[1] class [UnityEngine.CoreModule]UnityEngine.Coroutine
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(TMPro.Examples.VertexColorCycler+<AnimateVertexColors>d__3)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class TMPro.Examples.VertexColorCycler TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'::'<>4__this'
		IL_0026: ldarg.0
		IL_0027: ldloc.0
		IL_0028: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Coroutine [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StartCoroutine(class [mscorlib]System.Collections.IEnumerator)
		IL_002d: stloc.1
		IL_002e: ret
	} // end of method VertexColorCycler::Start

	.method private hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator AnimateVertexColors () cil managed 
	{
		// Method begins at RVA 0x195e8
		// Header size: 12
		// Code size: 40 (0x28)
		.maxstack 2
		.locals init (
			[0] class TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(TMPro.Examples.VertexColorCycler+<AnimateVertexColors>d__3)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class TMPro.Examples.VertexColorCycler TMPro.Examples.VertexColorCycler/'<AnimateVertexColors>d__3'::'<>4__this'
		IL_0026: ldloc.0
		IL_0027: ret
	} // end of method VertexColorCycler::AnimateVertexColors

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2a71
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method VertexColorCycler::.ctor

} // end of class TMPro.Examples.VertexColorCycler
