.class public auto ansi beforefieldinit EnvMapAnimator
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
		.field public class EnvMapAnimator '<>4__this'
		.field private valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4 '<matrix>5__2'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x2d0f
			// Header size: 1
			// Code size: 16 (0x10)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ldarg.0
			IL_0009: ldarg.1
			IL_000a: stfld int32 EnvMapAnimator/'<Start>d__4'::'<>1__state'
			IL_000f: ret
		} // end of method '<Start>d__4'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			// Method begins at RVA 0x2d20
			// Header size: 1
			// Code size: 2 (0x2)
			.maxstack 8

			IL_0000: nop
			IL_0001: ret
		} // end of method '<Start>d__4'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			// Method begins at RVA 0x1accc
			// Header size: 12
			// Code size: 126 (0x7e)
			.maxstack 2
			.locals init (
				[0] int32,
				[1] class EnvMapAnimator,
				[2] float32
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld int32 EnvMapAnimator/'<Start>d__4'::'<>1__state'
			IL_0007: stloc.0
			IL_0008: ldarg.0
			IL_0009: ldfld class EnvMapAnimator EnvMapAnimator/'<Start>d__4'::'<>4__this'
			IL_000e: stloc.1
			IL_000f: ldarg.0
			IL_0010: ldfld int32 EnvMapAnimator/'<Start>d__4'::'<>1__state'
			IL_0015: ldc.i4.0
			IL_0016: ceq
			IL_0018: ldc.i4.0
			IL_0019: ceq
			IL_001b: brtrue.s IL_006c

			IL_001d: nop
			IL_001e: ldarg.0
			IL_001f: ldc.i4.m1
			IL_0020: stfld int32 EnvMapAnimator/'<Start>d__4'::'<>1__state'
			IL_0025: ldarg.0
			IL_0026: ldc.r4 0.0
			IL_002b: box [mscorlib]System.Single
			IL_0030: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4>(object)
			IL_0035: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4 EnvMapAnimator/'<Start>d__4'::'<matrix>5__2'
			IL_003a: ldarg.0
			IL_003b: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4 EnvMapAnimator/'<Start>d__4'::'<matrix>5__2'
			IL_0040: ldc.r4 0.0
			IL_0045: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m01
			IL_004a: ldarg.0
			IL_004b: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4 EnvMapAnimator/'<Start>d__4'::'<matrix>5__2'
			IL_0050: ldc.r4 0.0
			IL_0055: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m02
			IL_005a: ldarg.0
			IL_005b: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4 EnvMapAnimator/'<Start>d__4'::'<matrix>5__2'
			IL_0060: ldc.r4 0.0
			IL_0065: stfld float32 [UnityEngine.CoreModule]UnityEngine.Matrix4x4::m03
			IL_006a: br.s IL_0075

			IL_006c: nop
			IL_006d: nop
			IL_006e: ldarg.0
			IL_006f: ldc.i4.m1
			IL_0070: stfld int32 EnvMapAnimator/'<Start>d__4'::'<>1__state'

			IL_0075: nop
			IL_0076: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_time()
			IL_007b: stloc.2
			IL_007c: ldc.i4.0
			IL_007d: ret
		} // end of method '<Start>d__4'::MoveNext

		.method private final hidebysig specialname newslot virtual 
			instance object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () cil managed 
		{
			// Method begins at RVA 0x2d23
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object EnvMapAnimator/'<Start>d__4'::'<>2__current'
			IL_0007: ret
		} // end of method '<Start>d__4'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'

		.method private final hidebysig newslot virtual 
			instance void System.Collections.IEnumerator.Reset () cil managed 
		{
			// Method begins at RVA 0x1ad58
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
			// Method begins at RVA 0x2d2c
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object EnvMapAnimator/'<Start>d__4'::'<>2__current'
			IL_0007: ret
		} // end of method '<Start>d__4'::System.Collections.IEnumerator.get_Current

		// Properties
		.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current'()
		{
			.get instance object EnvMapAnimator/'<Start>d__4'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'()
		}
		.property instance object System.Collections.IEnumerator.Current()
		{
			.get instance object EnvMapAnimator/'<Start>d__4'::System.Collections.IEnumerator.get_Current()
		}

	} // end of class <Start>d__4


	// Fields
	.field public valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 RotationSpeeds
	.field private class [Unity.TextMeshPro]TMPro.TMP_Text m_textMeshPro
	.field private class [UnityEngine.CoreModule]UnityEngine.Material m_material

	// Methods
	.method private hidebysig 
		instance void Awake () cil managed 
	{
		// Method begins at RVA 0x141a8
		// Header size: 12
		// Code size: 45 (0x2d)
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
		IL_000a: stfld class [Unity.TextMeshPro]TMPro.TMP_Text EnvMapAnimator::m_textMeshPro
		IL_000f: ldarg.0
		IL_0010: ldstr "FieldExpr->klass"
		IL_0015: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_001a: box [mscorlib]System.IntPtr
		IL_001f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [UnityEngine.CoreModule]UnityEngine.Material>(object)
		IL_0024: stfld class [UnityEngine.CoreModule]UnityEngine.Material EnvMapAnimator::m_material
		IL_0029: ret

		IL_002a: nop
		IL_002b: ldnull
		IL_002c: throw
	} // end of method EnvMapAnimator::Awake

	.method private hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator Start () cil managed 
	{
		// Method begins at RVA 0x141e4
		// Header size: 12
		// Code size: 40 (0x28)
		.maxstack 2
		.locals init (
			[0] class EnvMapAnimator/'<Start>d__4'
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(EnvMapAnimator+<Start>d__4)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class EnvMapAnimator/'<Start>d__4'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 EnvMapAnimator/'<Start>d__4'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class EnvMapAnimator EnvMapAnimator/'<Start>d__4'::'<>4__this'
		IL_0026: ldloc.0
		IL_0027: ret
	} // end of method EnvMapAnimator::Start

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x277e
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method EnvMapAnimator::.ctor

} // end of class EnvMapAnimator
