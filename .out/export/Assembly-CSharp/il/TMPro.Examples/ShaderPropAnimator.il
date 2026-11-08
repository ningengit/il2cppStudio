.class public auto ansi beforefieldinit TMPro.Examples.ShaderPropAnimator
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested private auto ansi sealed beforefieldinit '<AnimateProperties>d__6'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerator`1<object>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private object '<>2__current'
		.field public class TMPro.Examples.ShaderPropAnimator '<>4__this'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x2dae
			// Header size: 1
			// Code size: 16 (0x10)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ldarg.0
			IL_0009: ldarg.1
			IL_000a: stfld int32 TMPro.Examples.ShaderPropAnimator/'<AnimateProperties>d__6'::'<>1__state'
			IL_000f: ret
		} // end of method '<AnimateProperties>d__6'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			// Method begins at RVA 0x2dbf
			// Header size: 1
			// Code size: 2 (0x2)
			.maxstack 8

			IL_0000: nop
			IL_0001: ret
		} // end of method '<AnimateProperties>d__6'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			// Method begins at RVA 0x1b5c8
			// Header size: 12
			// Code size: 215 (0xd7)
			.maxstack 3
			.locals init (
				[0] int32,
				[1] class TMPro.Examples.ShaderPropAnimator,
				[2] float32,
				[3] float32,
				[4] float32,
				[5] class [UnityEngine.CoreModule]UnityEngine.Material,
				[6] float32,
				[7] float32,
				[8] float32,
				[9] float32,
				[10] class [UnityEngine.CoreModule]UnityEngine.WaitForEndOfFrame
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld int32 TMPro.Examples.ShaderPropAnimator/'<AnimateProperties>d__6'::'<>1__state'
			IL_0007: stloc.0
			IL_0008: ldarg.0
			IL_0009: ldfld class TMPro.Examples.ShaderPropAnimator TMPro.Examples.ShaderPropAnimator/'<AnimateProperties>d__6'::'<>4__this'
			IL_000e: stloc.1
			IL_000f: ldarg.0
			IL_0010: ldfld int32 TMPro.Examples.ShaderPropAnimator/'<AnimateProperties>d__6'::'<>1__state'
			IL_0015: ldc.i4.0
			IL_0016: ceq
			IL_0018: ldc.i4.0
			IL_0019: ceq
			IL_001b: brtrue.s IL_0040

			IL_001d: nop
			IL_001e: ldarg.0
			IL_001f: ldc.i4.m1
			IL_0020: stfld int32 TMPro.Examples.ShaderPropAnimator/'<AnimateProperties>d__6'::'<>1__state'
			IL_0025: ldc.r4 0.0
			IL_002a: ldc.r4 1
			IL_002f: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
			IL_0034: stloc.2
			IL_0035: ldloc.2
			IL_0036: stloc.3
			IL_0037: ldloc.1
			IL_0038: ldloc.3
			IL_0039: stfld float32 TMPro.Examples.ShaderPropAnimator::m_frame
			IL_003e: br.s IL_0055

			IL_0040: nop
			IL_0041: ldloc.0
			IL_0042: ldc.i4.1
			IL_0043: ceq
			IL_0045: ldc.i4.0
			IL_0046: ceq
			IL_0048: brtrue IL_00d0

			IL_004d: nop
			IL_004e: ldarg.0
			IL_004f: ldc.i4.m1
			IL_0050: stfld int32 TMPro.Examples.ShaderPropAnimator/'<AnimateProperties>d__6'::'<>1__state'

			IL_0055: nop
			IL_0056: ldloc.1
			IL_0057: ldfld class [UnityEngine.CoreModule]UnityEngine.AnimationCurve TMPro.Examples.ShaderPropAnimator::GlowCurve
			IL_005c: ldloc.1
			IL_005d: ldfld float32 TMPro.Examples.ShaderPropAnimator::m_frame
			IL_0062: callvirt instance float32 [UnityEngine.CoreModule]UnityEngine.AnimationCurve::Evaluate(float32)
			IL_0067: stloc.s 4
			IL_0069: ldloc.1
			IL_006a: ldfld class [UnityEngine.CoreModule]UnityEngine.Material TMPro.Examples.ShaderPropAnimator::m_Material
			IL_006f: stloc.s 5
			IL_0071: ldloc.s 4
			IL_0073: stloc.s 6
			IL_0075: ldloc.s 4
			IL_0077: stloc.3
			IL_0078: br.s IL_007b

			IL_007a: nop

			IL_007b: nop
			IL_007c: ldloc.s 5
			IL_007e: ldsfld int32 [Unity.TextMeshPro]TMPro.ShaderUtilities::ID_GlowPower
			IL_0083: ldloc.s 6
			IL_0085: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Material::SetFloatImpl(int32, float32)
			IL_008a: ldloc.1
			IL_008b: ldfld float32 TMPro.Examples.ShaderPropAnimator::m_frame
			IL_0090: stloc.s 7
			IL_0092: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
			IL_0097: stloc.s 8
			IL_0099: ldc.r4 0.2
			IL_009e: ldc.r4 0.3
			IL_00a3: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
			IL_00a8: stloc.s 9
			IL_00aa: ldloc.1
			IL_00ab: ldloc.s 9
			IL_00ad: ldloc.s 8
			IL_00af: mul
			IL_00b0: ldloc.s 7
			IL_00b2: add
			IL_00b3: stfld float32 TMPro.Examples.ShaderPropAnimator::m_frame
			IL_00b8: newobj instance void [mscorlib]Mono.Globalization.Unicode.ContractionComparer::.ctor()
			IL_00bd: stloc.s 10
			IL_00bf: ldarg.0
			IL_00c0: ldloc.s 10
			IL_00c2: stfld object TMPro.Examples.ShaderPropAnimator/'<AnimateProperties>d__6'::'<>2__current'
			IL_00c7: ldarg.0
			IL_00c8: ldc.i4.1
			IL_00c9: stfld int32 TMPro.Examples.ShaderPropAnimator/'<AnimateProperties>d__6'::'<>1__state'
			IL_00ce: br.s IL_00d1

			IL_00d0: nop

			IL_00d1: nop
			IL_00d2: ldc.i4.0
			IL_00d3: ret

			IL_00d4: nop
			IL_00d5: ldnull
			IL_00d6: throw
		} // end of method '<AnimateProperties>d__6'::MoveNext

		.method private final hidebysig specialname newslot virtual 
			instance object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () cil managed 
		{
			// Method begins at RVA 0x2dc2
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.ShaderPropAnimator/'<AnimateProperties>d__6'::'<>2__current'
			IL_0007: ret
		} // end of method '<AnimateProperties>d__6'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'

		.method private final hidebysig newslot virtual 
			instance void System.Collections.IEnumerator.Reset () cil managed 
		{
			// Method begins at RVA 0x1b6ac
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
		} // end of method '<AnimateProperties>d__6'::System.Collections.IEnumerator.Reset

		.method private final hidebysig specialname newslot virtual 
			instance object System.Collections.IEnumerator.get_Current () cil managed 
		{
			// Method begins at RVA 0x2dcb
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object TMPro.Examples.ShaderPropAnimator/'<AnimateProperties>d__6'::'<>2__current'
			IL_0007: ret
		} // end of method '<AnimateProperties>d__6'::System.Collections.IEnumerator.get_Current

		// Properties
		.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current'()
		{
			.get instance object TMPro.Examples.ShaderPropAnimator/'<AnimateProperties>d__6'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'()
		}
		.property instance object System.Collections.IEnumerator.Current()
		{
			.get instance object TMPro.Examples.ShaderPropAnimator/'<AnimateProperties>d__6'::System.Collections.IEnumerator.get_Current()
		}

	} // end of class <AnimateProperties>d__6


	// Fields
	.field private class [UnityEngine.CoreModule]UnityEngine.Renderer m_Renderer
	.field private class [UnityEngine.CoreModule]UnityEngine.Material m_Material
	.field public class [UnityEngine.CoreModule]UnityEngine.AnimationCurve GlowCurve
	.field public float32 m_frame

	// Methods
	.method private hidebysig 
		instance void Awake () cil managed 
	{
		// Method begins at RVA 0x162d0
		// Header size: 12
		// Code size: 38 (0x26)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Renderer,
			[1] class [UnityEngine.CoreModule]UnityEngine.Material
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.CoreModule]UnityEngine.Renderer>()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.CoreModule]UnityEngine.Renderer TMPro.Examples.ShaderPropAnimator::m_Renderer
		IL_000f: ldarg.0
		IL_0010: ldfld class [UnityEngine.CoreModule]UnityEngine.Renderer TMPro.Examples.ShaderPropAnimator::m_Renderer
		IL_0015: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Material [UnityEngine.CoreModule]UnityEngine.Renderer::GetMaterial()
		IL_001a: stloc.1
		IL_001b: ldarg.0
		IL_001c: ldloc.1
		IL_001d: stfld class [UnityEngine.CoreModule]UnityEngine.Material TMPro.Examples.ShaderPropAnimator::m_Material
		IL_0022: ret

		IL_0023: nop
		IL_0024: ldnull
		IL_0025: throw
	} // end of method ShaderPropAnimator::Awake

	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x16304
		// Header size: 12
		// Code size: 47 (0x2f)
		.maxstack 2
		.locals init (
			[0] class TMPro.Examples.ShaderPropAnimator/'<AnimateProperties>d__6',
			[1] class [UnityEngine.CoreModule]UnityEngine.Coroutine
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(TMPro.Examples.ShaderPropAnimator+<AnimateProperties>d__6)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.ShaderPropAnimator/'<AnimateProperties>d__6'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 TMPro.Examples.ShaderPropAnimator/'<AnimateProperties>d__6'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class TMPro.Examples.ShaderPropAnimator TMPro.Examples.ShaderPropAnimator/'<AnimateProperties>d__6'::'<>4__this'
		IL_0026: ldarg.0
		IL_0027: ldloc.0
		IL_0028: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Coroutine [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StartCoroutine(class [mscorlib]System.Collections.IEnumerator)
		IL_002d: stloc.1
		IL_002e: ret
	} // end of method ShaderPropAnimator::Start

	.method private hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator AnimateProperties () cil managed 
	{
		// Method begins at RVA 0x16340
		// Header size: 12
		// Code size: 40 (0x28)
		.maxstack 2
		.locals init (
			[0] class TMPro.Examples.ShaderPropAnimator/'<AnimateProperties>d__6'
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(TMPro.Examples.ShaderPropAnimator+<AnimateProperties>d__6)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class TMPro.Examples.ShaderPropAnimator/'<AnimateProperties>d__6'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 TMPro.Examples.ShaderPropAnimator/'<AnimateProperties>d__6'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class TMPro.Examples.ShaderPropAnimator TMPro.Examples.ShaderPropAnimator/'<AnimateProperties>d__6'::'<>4__this'
		IL_0026: ldloc.0
		IL_0027: ret
	} // end of method ShaderPropAnimator::AnimateProperties

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2910
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method ShaderPropAnimator::.ctor

} // end of class TMPro.Examples.ShaderPropAnimator
