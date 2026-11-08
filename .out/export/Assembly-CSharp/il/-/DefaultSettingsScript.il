.class public auto ansi beforefieldinit DefaultSettingsScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested private auto ansi sealed beforefieldinit '<CloseOptions>d__1'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerator`1<object>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private object '<>2__current'
		.field public class DefaultSettingsScript '<>4__this'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x2c9d
			// Header size: 1
			// Code size: 16 (0x10)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ldarg.0
			IL_0009: ldarg.1
			IL_000a: stfld int32 DefaultSettingsScript/'<CloseOptions>d__1'::'<>1__state'
			IL_000f: ret
		} // end of method '<CloseOptions>d__1'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			// Method begins at RVA 0x2cae
			// Header size: 1
			// Code size: 2 (0x2)
			.maxstack 8

			IL_0000: nop
			IL_0001: ret
		} // end of method '<CloseOptions>d__1'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			// Method begins at RVA 0x1aa68
			// Header size: 12
			// Code size: 107 (0x6b)
			.maxstack 2
			.locals init (
				[0] int32,
				[1] class [UnityEngine.CoreModule]UnityEngine.WaitForEndOfFrame,
				[2] class DefaultSettingsScript
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld int32 DefaultSettingsScript/'<CloseOptions>d__1'::'<>1__state'
			IL_0007: stloc.0
			IL_0008: ldarg.0
			IL_0009: ldfld int32 DefaultSettingsScript/'<CloseOptions>d__1'::'<>1__state'
			IL_000e: ldc.i4.0
			IL_000f: ceq
			IL_0011: ldc.i4.0
			IL_0012: ceq
			IL_0014: brtrue.s IL_0034

			IL_0016: nop
			IL_0017: ldarg.0
			IL_0018: ldc.i4.m1
			IL_0019: stfld int32 DefaultSettingsScript/'<CloseOptions>d__1'::'<>1__state'
			IL_001e: newobj instance void [mscorlib]Mono.Globalization.Unicode.ContractionComparer::.ctor()
			IL_0023: stloc.1
			IL_0024: ldarg.0
			IL_0025: ldloc.1
			IL_0026: stfld object DefaultSettingsScript/'<CloseOptions>d__1'::'<>2__current'
			IL_002b: ldarg.0
			IL_002c: ldc.i4.1
			IL_002d: stfld int32 DefaultSettingsScript/'<CloseOptions>d__1'::'<>1__state'
			IL_0032: ldc.i4.1
			IL_0033: ret

			IL_0034: nop
			IL_0035: ldloc.0
			IL_0036: ldc.i4.1
			IL_0037: ceq
			IL_0039: ldc.i4.0
			IL_003a: ceq
			IL_003c: brtrue.s IL_0065

			IL_003e: nop
			IL_003f: ldarg.0
			IL_0040: ldfld class DefaultSettingsScript DefaultSettingsScript/'<CloseOptions>d__1'::'<>4__this'
			IL_0045: stloc.2
			IL_0046: ldarg.0
			IL_0047: ldc.i4.m1
			IL_0048: stfld int32 DefaultSettingsScript/'<CloseOptions>d__1'::'<>1__state'
			IL_004d: ldloc.2
			IL_004e: ldfld class [UnityEngine.UIModule]UnityEngine.Canvas DefaultSettingsScript::canvas
			IL_0053: ldloc.0
			IL_0054: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::set_enabled(bool)
			IL_0059: ldloc.2
			IL_005a: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject DefaultSettingsScript::options
			IL_005f: ldc.i4.0
			IL_0060: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)

			IL_0065: nop
			IL_0066: ldc.i4.0
			IL_0067: ret

			IL_0068: nop
			IL_0069: ldnull
			IL_006a: throw
		} // end of method '<CloseOptions>d__1'::MoveNext

		.method private final hidebysig specialname newslot virtual 
			instance object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () cil managed 
		{
			// Method begins at RVA 0x2cb1
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object DefaultSettingsScript/'<CloseOptions>d__1'::'<>2__current'
			IL_0007: ret
		} // end of method '<CloseOptions>d__1'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'

		.method private final hidebysig newslot virtual 
			instance void System.Collections.IEnumerator.Reset () cil managed 
		{
			// Method begins at RVA 0x1aae0
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
		} // end of method '<CloseOptions>d__1'::System.Collections.IEnumerator.Reset

		.method private final hidebysig specialname newslot virtual 
			instance object System.Collections.IEnumerator.get_Current () cil managed 
		{
			// Method begins at RVA 0x2cba
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object DefaultSettingsScript/'<CloseOptions>d__1'::'<>2__current'
			IL_0007: ret
		} // end of method '<CloseOptions>d__1'::System.Collections.IEnumerator.get_Current

		// Properties
		.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current'()
		{
			.get instance object DefaultSettingsScript/'<CloseOptions>d__1'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'()
		}
		.property instance object System.Collections.IEnumerator.Current()
		{
			.get instance object DefaultSettingsScript/'<CloseOptions>d__1'::System.Collections.IEnumerator.get_Current()
		}

	} // end of class <CloseOptions>d__1


	// Fields
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject options
	.field public class [UnityEngine.UIModule]UnityEngine.Canvas canvas

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0xa5c8
		// Header size: 12
		// Code size: 99 (0x63)
		.maxstack 2
		.locals init (
			[0] bool,
			[1] bool,
			[2] class DefaultSettingsScript/'<CloseOptions>d__1',
			[3] class [UnityEngine.CoreModule]UnityEngine.Coroutine
		)

		IL_0000: nop
		IL_0001: ldstr "OptionsSet"
		IL_0006: call bool [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::HasKey(string)
		IL_000b: stloc.0
		IL_000c: ldloc.0
		IL_000d: stloc.1
		IL_000e: ldloc.0
		IL_000f: ldc.i4.0
		IL_0010: ceq
		IL_0012: ldc.i4.0
		IL_0013: ceq
		IL_0015: brtrue.s IL_005e

		IL_0017: nop
		IL_0018: ldarg.0
		IL_0019: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject DefaultSettingsScript::options
		IL_001e: ldc.i4.1
		IL_001f: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0024: ldstr "il2cpp_codegen_object_new(DefaultSettingsScript+<CloseOptions>d__1)"
		IL_0029: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_002e: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class DefaultSettingsScript/'<CloseOptions>d__1'>(object)
		IL_0033: stloc.2
		IL_0034: ldloc.2
		IL_0035: ldnull
		IL_0036: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_003b: ldloc.2
		IL_003c: ldc.i4.0
		IL_003d: stfld int32 DefaultSettingsScript/'<CloseOptions>d__1'::'<>1__state'
		IL_0042: ldloc.2
		IL_0043: ldarg.0
		IL_0044: stfld class DefaultSettingsScript DefaultSettingsScript/'<CloseOptions>d__1'::'<>4__this'
		IL_0049: ldarg.0
		IL_004a: ldloc.2
		IL_004b: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Coroutine [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StartCoroutine(class [mscorlib]System.Collections.IEnumerator)
		IL_0050: stloc.3
		IL_0051: ldarg.0
		IL_0052: ldfld class [UnityEngine.UIModule]UnityEngine.Canvas DefaultSettingsScript::canvas
		IL_0057: ldc.i4.0
		IL_0058: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::set_enabled(bool)
		IL_005d: ret

		IL_005e: nop
		IL_005f: ret

		IL_0060: nop
		IL_0061: ldnull
		IL_0062: throw
	} // end of method DefaultSettingsScript::Start

	.method public hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator CloseOptions () cil managed 
	{
		// Method begins at RVA 0xa638
		// Header size: 12
		// Code size: 40 (0x28)
		.maxstack 2
		.locals init (
			[0] class DefaultSettingsScript/'<CloseOptions>d__1'
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(DefaultSettingsScript+<CloseOptions>d__1)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class DefaultSettingsScript/'<CloseOptions>d__1'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 DefaultSettingsScript/'<CloseOptions>d__1'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class DefaultSettingsScript DefaultSettingsScript/'<CloseOptions>d__1'::'<>4__this'
		IL_0026: ldloc.0
		IL_0027: ret
	} // end of method DefaultSettingsScript::CloseOptions

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2378
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method DefaultSettingsScript::.ctor

} // end of class DefaultSettingsScript
