.class public auto ansi beforefieldinit DebugScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested private auto ansi sealed beforefieldinit '<LogDissappear>d__3'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerator`1<object>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private object '<>2__current'
		.field public class DebugScript '<>4__this'
		.field private float32 '<elapsed>5__2'
		.field private valuetype [UnityEngine.CoreModule]UnityEngine.Color '<color>5__3'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x2bd6
			// Header size: 1
			// Code size: 16 (0x10)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ldarg.0
			IL_0009: ldarg.1
			IL_000a: stfld int32 DebugScript/'<LogDissappear>d__3'::'<>1__state'
			IL_000f: ret
		} // end of method '<LogDissappear>d__3'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			// Method begins at RVA 0x2be7
			// Header size: 1
			// Code size: 2 (0x2)
			.maxstack 8

			IL_0000: nop
			IL_0001: ret
		} // end of method '<LogDissappear>d__3'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			// Method begins at RVA 0x19f60
			// Header size: 12
			// Code size: 123 (0x7b)
			.maxstack 2
			.locals init (
				[0] int32,
				[1] class DebugScript,
				[2] class [Unity.TextMeshPro]TMPro.TMP_Text,
				[3] class [Unity.TextMeshPro]TMPro.TMP_Text,
				[4] class [Unity.TextMeshPro]TMPro.TMP_Text
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld int32 DebugScript/'<LogDissappear>d__3'::'<>1__state'
			IL_0007: stloc.0
			IL_0008: ldarg.0
			IL_0009: ldfld class DebugScript DebugScript/'<LogDissappear>d__3'::'<>4__this'
			IL_000e: stloc.1
			IL_000f: nop
			IL_0010: ldloc.0
			IL_0011: ldc.i4.1
			IL_0012: sub
			IL_0013: stloc.0
			IL_0014: ldloc.0
			IL_0015: ldc.i4.1
			IL_0016: sub
			IL_0017: ldc.i4.0
			IL_0018: ceq
			IL_001a: brtrue.s IL_0027

			IL_001c: nop
			IL_001d: nop
			IL_001e: ldarg.0
			IL_001f: ldc.i4.m1
			IL_0020: stfld int32 DebugScript/'<LogDissappear>d__3'::'<>1__state'
			IL_0025: br.s IL_0078

			IL_0027: nop
			IL_0028: ldarg.0
			IL_0029: ldc.i4.m1
			IL_002a: stfld int32 DebugScript/'<LogDissappear>d__3'::'<>1__state'
			IL_002f: ldarg.0
			IL_0030: ldc.r4 0.0
			IL_0035: stfld float32 DebugScript/'<LogDissappear>d__3'::'<elapsed>5__2'
			IL_003a: nop
			IL_003b: ldloc.1
			IL_003c: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text DebugScript::errorText
			IL_0041: stloc.2
			IL_0042: nop
			IL_0043: ldarg.0
			IL_0044: ldstr "*(LocalExpr->klass + 0x0)"
			IL_0049: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_004e: box [mscorlib]System.Int64
			IL_0053: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Color>(object)
			IL_0058: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Color DebugScript/'<LogDissappear>d__3'::'<color>5__3'
			IL_005d: ldloc.1
			IL_005e: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text DebugScript::errorText
			IL_0063: stloc.3
			IL_0064: ldarg.0
			IL_0065: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Color DebugScript/'<LogDissappear>d__3'::'<color>5__3'
			IL_006a: ldc.r4 1
			IL_006f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Color::a
			IL_0074: ldloc.3
			IL_0075: stloc.s 4
			IL_0077: nop

			IL_0078: nop
			IL_0079: ldc.i4.0
			IL_007a: ret
		} // end of method '<LogDissappear>d__3'::MoveNext

		.method private final hidebysig specialname newslot virtual 
			instance object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () cil managed 
		{
			// Method begins at RVA 0x2bea
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object DebugScript/'<LogDissappear>d__3'::'<>2__current'
			IL_0007: ret
		} // end of method '<LogDissappear>d__3'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'

		.method private final hidebysig newslot virtual 
			instance void System.Collections.IEnumerator.Reset () cil managed 
		{
			// Method begins at RVA 0x19fe8
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
		} // end of method '<LogDissappear>d__3'::System.Collections.IEnumerator.Reset

		.method private final hidebysig specialname newslot virtual 
			instance object System.Collections.IEnumerator.get_Current () cil managed 
		{
			// Method begins at RVA 0x2bf3
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object DebugScript/'<LogDissappear>d__3'::'<>2__current'
			IL_0007: ret
		} // end of method '<LogDissappear>d__3'::System.Collections.IEnumerator.get_Current

		// Properties
		.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current'()
		{
			.get instance object DebugScript/'<LogDissappear>d__3'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'()
		}
		.property instance object System.Collections.IEnumerator.Current()
		{
			.get instance object DebugScript/'<LogDissappear>d__3'::System.Collections.IEnumerator.get_Current()
		}

	} // end of class <LogDissappear>d__3


	// Fields
	.field public bool limitFramerate
	.field public int32 framerate
	.field public class [Unity.TextMeshPro]TMPro.TextMeshProUGUI fpsText
	.field public float32 deltaTime
	.field private class [Unity.TextMeshPro]TMPro.TextMeshProUGUI keyText
	.field private class [Unity.TextMeshPro]TMPro.TMP_Text errorText
	.field private class [UnityEngine.AudioModule]UnityEngine.AudioSource errorSource
	.field private class [UnityEngine.CoreModule]UnityEngine.Coroutine logDissappearCoroutine

	// Methods
	.method private hidebysig 
		instance void OnEnable () cil managed 
	{
		// Method begins at RVA 0x3390
		// Header size: 12
		// Code size: 27 (0x1b)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Application/LogCallback,
			[1] class [UnityEngine.CoreModule]UnityEngine.Application/LogCallback
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldftn instance void DebugScript::LogError(string, string, valuetype [UnityEngine.CoreModule]UnityEngine.LogType)
		IL_0008: newobj instance void [UnityEngine.CoreModule]UnityEngine.Application/LogCallback::.ctor(object, native int)
		IL_000d: stloc.0
		IL_000e: ldloc.0
		IL_000f: stloc.1
		IL_0010: br.s IL_0013

		IL_0012: nop

		IL_0013: nop
		IL_0014: ldloc.1
		IL_0015: call void [UnityEngine.CoreModule]UnityEngine.Application::add_logMessageReceived(class [UnityEngine.CoreModule]UnityEngine.Application/LogCallback)
		IL_001a: ret
	} // end of method DebugScript::OnEnable

	.method private hidebysig 
		instance void OnDisable () cil managed 
	{
		// Method begins at RVA 0x33b8
		// Header size: 12
		// Code size: 27 (0x1b)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Application/LogCallback,
			[1] class [UnityEngine.CoreModule]UnityEngine.Application/LogCallback
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldftn instance void DebugScript::LogError(string, string, valuetype [UnityEngine.CoreModule]UnityEngine.LogType)
		IL_0008: newobj instance void [UnityEngine.CoreModule]UnityEngine.Application/LogCallback::.ctor(object, native int)
		IL_000d: stloc.0
		IL_000e: ldloc.0
		IL_000f: stloc.1
		IL_0010: br.s IL_0013

		IL_0012: nop

		IL_0013: nop
		IL_0014: ldloc.1
		IL_0015: call void [UnityEngine.CoreModule]UnityEngine.Application::remove_logMessageReceived(class [UnityEngine.CoreModule]UnityEngine.Application/LogCallback)
		IL_001a: ret
	} // end of method DebugScript::OnDisable

	.method private hidebysig 
		instance void LogError (
			string logString,
			string stackTrace,
			valuetype [UnityEngine.CoreModule]UnityEngine.LogType 'type'
		) cil managed 
	{
		// Method begins at RVA 0x33e0
		// Header size: 12
		// Code size: 195 (0xc3)
		.maxstack 4
		.locals init (
			[0] int32,
			[1] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[2] string,
			[3] string,
			[4] bool,
			[5] class DebugScript/'<LogDissappear>d__3',
			[6] class [UnityEngine.CoreModule]UnityEngine.Coroutine
		)

		IL_0000: nop
		IL_0001: ldarg.3
		IL_0002: box [mscorlib]System.Object
		IL_0007: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_000c: stloc.0
		IL_000d: ldarg.3
		IL_000e: box [mscorlib]System.Object
		IL_0013: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0018: ldc.i4.0
		IL_0019: ceq
		IL_001b: brtrue.s IL_002a

		IL_001d: nop
		IL_001e: ldloc.0
		IL_001f: ldc.i4.4
		IL_0020: ceq
		IL_0022: ldc.i4.0
		IL_0023: ceq
		IL_0025: brtrue IL_00be

		IL_002a: nop
		IL_002b: ldarg.0
		IL_002c: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text DebugScript::errorText
		IL_0031: stloc.1
		IL_0032: ldarg.1
		IL_0033: ldstr "\n"
		IL_0038: ldarg.2
		IL_0039: ldstr "\n"
		IL_003e: call string [mscorlib]System.String::Concat(string, string, string, string)
		IL_0043: stloc.2
		IL_0044: ldloc.2
		IL_0045: stloc.3
		IL_0046: ldarg.0
		IL_0047: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource DebugScript::errorSource
		IL_004c: callvirt instance bool [UnityEngine.AudioModule]UnityEngine.AudioSource::get_isPlaying()
		IL_0051: stloc.s 4
		IL_0053: ldloc.s 4
		IL_0055: ldc.i4.0
		IL_0056: ceq
		IL_0058: ldc.i4.0
		IL_0059: ceq
		IL_005b: brtrue.s IL_0069

		IL_005d: nop
		IL_005e: ldarg.0
		IL_005f: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource DebugScript::errorSource
		IL_0064: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Play()

		IL_0069: nop
		IL_006a: ldarg.0
		IL_006b: ldfld class [UnityEngine.CoreModule]UnityEngine.Coroutine DebugScript::logDissappearCoroutine
		IL_0070: ldnull
		IL_0071: ceq
		IL_0073: brtrue.s IL_0082

		IL_0075: nop
		IL_0076: ldarg.0
		IL_0077: ldarg.0
		IL_0078: ldfld class [UnityEngine.CoreModule]UnityEngine.Coroutine DebugScript::logDissappearCoroutine
		IL_007d: callvirt instance void [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StopCoroutine(class [UnityEngine.CoreModule]UnityEngine.Coroutine)

		IL_0082: nop
		IL_0083: ldstr "il2cpp_codegen_object_new(DebugScript+<LogDissappear>d__3)"
		IL_0088: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_008d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class DebugScript/'<LogDissappear>d__3'>(object)
		IL_0092: stloc.s 5
		IL_0094: ldloc.s 5
		IL_0096: ldnull
		IL_0097: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_009c: ldloc.s 5
		IL_009e: ldc.i4.0
		IL_009f: stfld int32 DebugScript/'<LogDissappear>d__3'::'<>1__state'
		IL_00a4: ldloc.s 5
		IL_00a6: ldarg.0
		IL_00a7: stfld class DebugScript DebugScript/'<LogDissappear>d__3'::'<>4__this'
		IL_00ac: ldarg.0
		IL_00ad: ldloc.s 5
		IL_00af: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Coroutine [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StartCoroutine(class [mscorlib]System.Collections.IEnumerator)
		IL_00b4: stloc.s 6
		IL_00b6: ldarg.0
		IL_00b7: ldloc.s 6
		IL_00b9: stfld class [UnityEngine.CoreModule]UnityEngine.Coroutine DebugScript::logDissappearCoroutine

		IL_00be: nop
		IL_00bf: ret

		IL_00c0: nop
		IL_00c1: ldnull
		IL_00c2: throw
	} // end of method DebugScript::LogError

	.method private hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator LogDissappear () cil managed 
	{
		// Method begins at RVA 0x34b0
		// Header size: 12
		// Code size: 40 (0x28)
		.maxstack 2
		.locals init (
			[0] class DebugScript/'<LogDissappear>d__3'
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(DebugScript+<LogDissappear>d__3)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class DebugScript/'<LogDissappear>d__3'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 DebugScript/'<LogDissappear>d__3'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class DebugScript DebugScript/'<LogDissappear>d__3'::'<>4__this'
		IL_0026: ldloc.0
		IL_0027: ret
	} // end of method DebugScript::LogDissappear

	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x34e4
		// Header size: 12
		// Code size: 39 (0x27)
		.maxstack 2
		.locals init (
			[0] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld bool DebugScript::limitFramerate
		IL_0007: ldc.i4.0
		IL_0008: ceq
		IL_000a: brtrue.s IL_0025

		IL_000c: nop
		IL_000d: ldc.i4.0
		IL_000e: call void [UnityEngine.CoreModule]UnityEngine.QualitySettings::set_vSyncCount(int32)
		IL_0013: ldarg.0
		IL_0014: ldfld int32 DebugScript::framerate
		IL_0019: stloc.0
		IL_001a: br.s IL_001d

		IL_001c: nop

		IL_001d: nop
		IL_001e: ldloc.0
		IL_001f: call void [UnityEngine.CoreModule]UnityEngine.Application::set_targetFrameRate(int32)
		IL_0024: ret

		IL_0025: nop
		IL_0026: ret
	} // end of method DebugScript::Start

	.method public hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x3518
		// Header size: 12
		// Code size: 981 (0x3d5)
		.maxstack 5
		.locals init (
			[0] float32,
			[1] float32,
			[2] float32,
			[3] float32,
			[4] float32,
			[5] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[6] int64,
			[7] float32,
			[8] string,
			[9] string,
			[10] string,
			[11] class [mscorlib]System.Type,
			[12] class [mscorlib]System.Type,
			[13] class [mscorlib]System.Array,
			[14] class [mscorlib]System.Collections.IEnumerator,
			[15] int64,
			[16] int64,
			[17] int32,
			[18] int64,
			[19] int32,
			[20] int64,
			[21] int64,
			[22] int64,
			[23] int32,
			[24] int64,
			[25] int64,
			[26] int64,
			[27] int64,
			[28] bool,
			[29] int32,
			[30] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[31] string,
			[32] string,
			[33] string,
			[34] int64,
			[35] int64,
			[36] int64,
			[37] int64,
			[38] int64
		)

		IL_0000: nop
		IL_0001: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0006: stloc.0
		IL_0007: ldloc.0
		IL_0008: stloc.1
		IL_0009: nop
		IL_000a: ldloc.0
		IL_000b: ldc.r4 0.0
		IL_0010: ceq
		IL_0012: brtrue IL_00ad

		IL_0017: nop
		IL_0018: ldarg.0
		IL_0019: ldfld float32 DebugScript::deltaTime
		IL_001e: stloc.2
		IL_001f: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0024: stloc.3
		IL_0025: ldloc.3
		IL_0026: ldarg.0
		IL_0027: ldfld float32 DebugScript::deltaTime
		IL_002c: sub
		IL_002d: ldc.r4 0.1
		IL_0032: mul
		IL_0033: ldloc.2
		IL_0034: add
		IL_0035: stloc.s 4
		IL_0037: ldarg.0
		IL_0038: ldloc.s 4
		IL_003a: stfld float32 DebugScript::deltaTime
		IL_003f: ldarg.0
		IL_0040: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI DebugScript::fpsText
		IL_0045: stloc.s 5
		IL_0047: ldstr "sub_20C150"
		IL_004c: ldc.i8 0
		IL_0055: ldc.i8 0
		IL_005e: ldc.i8 0
		IL_0067: ldc.i8 0
		IL_0070: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0075: stloc.s 6
		IL_0077: ldloca.s 7
		IL_0079: ldstr "xmm0"
		IL_007e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0083: box [mscorlib]System.Int64
		IL_0088: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_008d: stfld float32 [mscorlib]System.Single::m_value
		IL_0092: ldloca.s 7
		IL_0094: call instance string [mscorlib]System.Single::ToString()
		IL_0099: stloc.s 8
		IL_009b: ldloc.s 8
		IL_009d: ldstr " :FPS"
		IL_00a2: call string [mscorlib]System.String::Concat(string, string)
		IL_00a7: stloc.s 9
		IL_00a9: ldloc.s 9
		IL_00ab: stloc.s 10

		IL_00ad: nop
		IL_00ae: br.s IL_00b1

		IL_00b0: nop

		IL_00b1: nop
		IL_00b2: ldstr "typeof(KeyCode)"
		IL_00b7: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00bc: box [mscorlib]System.IntPtr
		IL_00c1: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [mscorlib]System.RuntimeTypeHandle>(object)
		IL_00c6: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		IL_00cb: stloc.s 11
		IL_00cd: ldloc.s 11
		IL_00cf: stloc.s 12
		IL_00d1: br.s IL_00d4

		IL_00d3: nop

		IL_00d4: nop
		IL_00d5: ldloc.s 12
		IL_00d7: call class [mscorlib]System.Array [mscorlib]System.Enum::GetValues(class [mscorlib]System.Type)
		IL_00dc: stloc.s 13
		IL_00de: ldloc.s 13
		IL_00e0: callvirt instance class [mscorlib]System.Collections.IEnumerator [mscorlib]System.Array::GetEnumerator()
		IL_00e5: stloc.s 14

		IL_00e7: nop
		IL_00e8: ldstr "stack[0x20]"
		IL_00ed: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00f2: stloc.s 15
		IL_00f4: ldstr "*(LocalExpr + 0x0)"
		IL_00f9: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00fe: stloc.s 16
		IL_0100: ldstr "*(*(LocalExpr + 0x0) + 0x12E)"
		IL_0105: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_010a: conv.i4
		IL_010b: stloc.s 17
		IL_010d: ldc.i4.0
		IL_010e: ldstr "*(*(LocalExpr + 0x0) + 0x12E)"
		IL_0113: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0118: conv.i4
		IL_0119: clt.un
		IL_011b: ldc.i4.0
		IL_011c: ceq
		IL_011e: brtrue.s IL_013d

		IL_0120: nop
		IL_0121: ldstr "*(LocalExpr + 0xB0)"
		IL_0126: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_012b: stloc.s 18
		// loop start (head: IL_012d)
			IL_012d: nop
			IL_012e: nop
			IL_012f: ldc.i4.0
			IL_0130: ldc.i4.1
			IL_0131: add
			IL_0132: stloc.s 19
			IL_0134: ldc.i4.0
			IL_0135: ldc.i4.1
			IL_0136: add
			IL_0137: ldloc.s 17
			IL_0139: clt.un
			IL_013b: brtrue.s IL_012d
		// end loop

		IL_013d: nop
		IL_013e: ldstr "sub_17A5F0"
		IL_0143: ldloc.s 15
		IL_0145: ldc.i8 0
		IL_014e: ldc.i4.0
		IL_014f: conv.i8
		IL_0150: ldc.i8 0
		IL_0159: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_015e: stloc.s 20
		IL_0160: ldloc.s 20
		IL_0162: stloc.s 21

		IL_0164: nop
		IL_0165: ldstr "*(LocalExpr + 0x8)"
		IL_016a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_016f: stloc.s 22
		IL_0171: ldloc.s 21
		IL_0173: conv.i4
		IL_0174: ldloc.s 21
		IL_0176: conv.i4
		IL_0177: and
		IL_0178: ldc.i4.0
		IL_0179: ceq
		IL_017b: brtrue IL_033b

		IL_0180: nop
		IL_0181: ldstr "stack[0x20]"
		IL_0186: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_018b: stloc.s 15
		IL_018d: ldstr "*(LocalExpr + 0x0)"
		IL_0192: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0197: stloc.s 16
		IL_0199: ldstr "*(*(LocalExpr + 0x0) + 0x12E)"
		IL_019e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01a3: conv.i4
		IL_01a4: stloc.s 23
		IL_01a6: ldc.i4.0
		IL_01a7: ldstr "*(*(LocalExpr + 0x0) + 0x12E)"
		IL_01ac: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01b1: conv.i4
		IL_01b2: clt.un
		IL_01b4: ldc.i4.0
		IL_01b5: ceq
		IL_01b7: brtrue.s IL_01d6

		IL_01b9: nop
		IL_01ba: ldstr "*(LocalExpr + 0xB0)"
		IL_01bf: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01c4: stloc.s 18
		// loop start (head: IL_01c6)
			IL_01c6: nop
			IL_01c7: nop
			IL_01c8: ldc.i4.0
			IL_01c9: ldc.i4.1
			IL_01ca: add
			IL_01cb: stloc.s 19
			IL_01cd: ldc.i4.0
			IL_01ce: ldc.i4.1
			IL_01cf: add
			IL_01d0: ldloc.s 23
			IL_01d2: clt.un
			IL_01d4: brtrue.s IL_01c6
		// end loop

		IL_01d6: nop
		IL_01d7: ldstr "sub_17A5F0"
		IL_01dc: ldloc.s 15
		IL_01de: ldc.i8 0
		IL_01e7: ldc.i4.1
		IL_01e8: conv.i8
		IL_01e9: ldc.i8 0
		IL_01f2: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_01f7: stloc.s 24
		IL_01f9: ldloc.s 24
		IL_01fb: stloc.s 21
		// loop start (head: IL_01fd)
			IL_01fd: nop
			IL_01fe: ldloc.s 15
			IL_0200: stloc.s 25
			IL_0202: ldstr "*(LocalExpr + 0x8)"
			IL_0207: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_020c: stloc.s 26
			IL_020e: ldloc.s 21
			IL_0210: stloc.s 25
			IL_0212: ldstr "*(LocalExpr + 0x0)"
			IL_0217: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_021c: stloc.s 26
			IL_021e: nop
			IL_021f: ldstr "il2cpp_object_unbox"
			IL_0224: ldloc.s 25
			IL_0226: ldloc.s 26
			IL_0228: ldc.i8 0
			IL_0231: ldc.i8 0
			IL_023a: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
			IL_023f: stloc.s 27
			IL_0241: ldstr "*(LocalExpr + 0x0)"
			IL_0246: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_024b: conv.i4
			IL_024c: box [mscorlib]System.Int32
			IL_0251: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
			IL_0256: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetKeyDownInt(valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode)
			IL_025b: stloc.s 28
			IL_025d: ldstr "*(LocalExpr + 0x0)"
			IL_0262: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0267: conv.i4
			IL_0268: stloc.s 29
			IL_026a: ldloc.s 28
			IL_026c: ldc.i4.0
			IL_026d: ceq
			IL_026f: brtrue IL_00e7

			IL_0274: nop
			IL_0275: ldarg.0
			IL_0276: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI DebugScript::keyText
			IL_027b: stloc.s 30
			IL_027d: ldstr "&stack[0xFFFFFFFFFFFFFFD0]"
			IL_0282: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0287: box [mscorlib]System.IntPtr
			IL_028c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [mscorlib]System.Enum>(object)
			IL_0291: callvirt instance string [mscorlib]System.Enum::ToString()
			IL_0296: stloc.s 31
			IL_0298: ldloc.s 31
			IL_029a: ldstr " :Input"
			IL_029f: call string [mscorlib]System.String::Concat(string, string)
			IL_02a4: stloc.s 32
			IL_02a6: ldloc.s 32
			IL_02a8: stloc.s 33
			IL_02aa: ldstr "sub_23E000"
			IL_02af: ldstr "&stack[0xFFFFFFFFFFFFFFC0]"
			IL_02b4: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_02b9: conv.i8
			IL_02ba: ldloc.s 33
			IL_02bc: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
			IL_02c1: ldc.i8 0
			IL_02ca: ldc.i8 0
			IL_02d3: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
			IL_02d8: stloc.s 34
			IL_02da: ldloc.s 34
			IL_02dc: stloc.s 35
			IL_02de: br IL_039d

			IL_02e3: nop
			IL_02e4: ldstr "*(KeyCode_TypeInfo + 0x8)"
			IL_02e9: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_02ee: conv.i4
			IL_02ef: conv.i8
			IL_02f0: ldc.i8 4
			IL_02f9: shl
			IL_02fa: ldc.i8 312
			IL_0303: add
			IL_0304: ldloc.s 16
			IL_0306: add
			IL_0307: stloc.s 35
			IL_0309: br IL_0164

			IL_030e: nop
			IL_030f: ldstr "*(KeyCode_TypeInfo + 0x8)"
			IL_0314: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0319: conv.i4
			IL_031a: ldc.i4.1
			IL_031b: add
			IL_031c: conv.i8
			IL_031d: ldc.i8 4
			IL_0326: shl
			IL_0327: ldc.i8 312
			IL_0330: add
			IL_0331: ldloc.s 16
			IL_0333: add
			IL_0334: stloc.s 35
			IL_0336: br IL_01fd
		// end loop

		IL_033b: nop
		IL_033c: ldstr "sub_23E000"
		IL_0341: ldstr "&stack[0xFFFFFFFFFFFFFFC0]"
		IL_0346: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_034b: conv.i8
		IL_034c: ldloc.s 26
		IL_034e: ldc.i8 0
		IL_0357: ldc.i8 0
		IL_0360: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0365: stloc.s 36
		IL_0367: ldloc.s 36
		IL_0369: stloc.s 35
		IL_036b: br.s IL_039d

		IL_036d: nop
		IL_036e: ldstr "sub_23E000"
		IL_0373: ldstr "&stack[0xFFFFFFFFFFFFFFC0]"
		IL_0378: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_037d: conv.i8
		IL_037e: ldloc.s 26
		IL_0380: ldc.i8 0
		IL_0389: ldc.i8 0
		IL_0392: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0397: stloc.s 37
		IL_0399: ldloc.s 37
		IL_039b: stloc.s 35

		IL_039d: nop
		IL_039e: ret

		IL_039f: nop
		IL_03a0: ldnull
		IL_03a1: throw

		IL_03a2: nop
		IL_03a3: nop
		IL_03a4: ldnull
		IL_03a5: throw

		IL_03a6: nop
		IL_03a7: ldnull
		IL_03a8: throw

		IL_03a9: nop
		IL_03aa: ldnull
		IL_03ab: throw

		IL_03ac: nop
		IL_03ad: ldnull
		IL_03ae: throw

		IL_03af: nop
		IL_03b0: ldstr "sub_167180"
		IL_03b5: ldloc.s 25
		IL_03b7: ldloc.s 26
		IL_03b9: ldc.i8 0
		IL_03c2: ldc.i8 0
		IL_03cb: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_03d0: stloc.s 38
		IL_03d2: nop
		IL_03d3: ldnull
		IL_03d4: throw
	} // end of method DebugScript::Update

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x20a7
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method DebugScript::.ctor

} // end of class DebugScript
