.class public auto ansi beforefieldinit OptionsManager
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class [UnityEngine.UI]UnityEngine.UI.Slider slider
	.field public class [UnityEngine.UI]UnityEngine.UI.Toggle rumble
	.field public class [UnityEngine.UI]UnityEngine.UI.Toggle analog

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0xa6bc
		// Header size: 12
		// Code size: 157 (0x9d)
		.maxstack 3
		.locals init (
			[0] bool,
			[1] class [UnityEngine.UI]UnityEngine.UI.Slider,
			[2] float32,
			[3] int32,
			[4] int32
		)

		IL_0000: nop
		IL_0001: ldstr "OptionsSet"
		IL_0006: call bool [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::HasKey(string)
		IL_000b: stloc.0
		IL_000c: ldloc.0
		IL_000d: ldc.i4.0
		IL_000e: ceq
		IL_0010: ldc.i4.0
		IL_0011: ceq
		IL_0013: brtrue.s IL_0022

		IL_0015: nop
		IL_0016: ldstr "OptionsSet"
		IL_001b: ldc.i4.1
		IL_001c: call void [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::SetInt(string, int32)
		IL_0021: ret

		IL_0022: nop
		IL_0023: ldarg.0
		IL_0024: ldfld class [UnityEngine.UI]UnityEngine.UI.Slider OptionsManager::slider
		IL_0029: stloc.1
		IL_002a: ldstr "MouseSensitivity"
		IL_002f: call float32 [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::GetFloat(string)
		IL_0034: stloc.2
		IL_0035: ldstr "Rumble"
		IL_003a: call int32 [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::GetInt(string)
		IL_003f: stloc.3
		IL_0040: ldarg.0
		IL_0041: ldfld class [UnityEngine.UI]UnityEngine.UI.Toggle OptionsManager::rumble
		IL_0046: ldloc.3
		IL_0047: ldc.i4.1
		IL_0048: ceq
		IL_004a: callvirt instance void [UnityEngine.UI]UnityEngine.UI.Toggle::set_isOn(bool)
		IL_004f: ldstr "AnalogMove"
		IL_0054: call int32 [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::GetInt(string)
		IL_0059: stloc.s 4
		IL_005b: ldloc.s 4
		IL_005d: ldc.i4.1
		IL_005e: ceq
		IL_0060: brtrue.s IL_007e

		IL_0062: nop
		IL_0063: ldstr "rcx"
		IL_0068: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_006d: box [mscorlib]System.Int64
		IL_0072: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [UnityEngine.UI]UnityEngine.UI.Toggle>(object)
		IL_0077: ldc.i4.0
		IL_0078: callvirt instance void [UnityEngine.UI]UnityEngine.UI.Toggle::set_isOn(bool)
		IL_007d: ret

		IL_007e: nop
		IL_007f: ldstr "rcx"
		IL_0084: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0089: box [mscorlib]System.Int64
		IL_008e: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [UnityEngine.UI]UnityEngine.UI.Toggle>(object)
		IL_0093: ldc.i4.1
		IL_0094: callvirt instance void [UnityEngine.UI]UnityEngine.UI.Toggle::set_isOn(bool)
		IL_0099: ret

		IL_009a: nop
		IL_009b: ldnull
		IL_009c: throw
	} // end of method OptionsManager::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0xa768
		// Header size: 12
		// Code size: 151 (0x97)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.UI]UnityEngine.UI.Slider
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.UI]UnityEngine.UI.Slider OptionsManager::slider
		IL_0007: stloc.0
		IL_0008: ldstr "MouseSensitivity"
		IL_000d: ldstr "xmm0"
		IL_0012: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0017: box [mscorlib]System.Int64
		IL_001c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0021: call void [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::SetFloat(string, float32)
		IL_0026: ldarg.0
		IL_0027: ldfld class [UnityEngine.UI]UnityEngine.UI.Toggle OptionsManager::rumble
		IL_002c: ldfld bool [UnityEngine.UI]UnityEngine.UI.Toggle::m_IsOn
		IL_0031: ldc.i4.0
		IL_0032: ceq
		IL_0034: ldc.i4.0
		IL_0035: ceq
		IL_0037: brtrue.s IL_003c

		IL_0039: nop
		IL_003a: br.s IL_003d

		IL_003c: nop

		IL_003d: nop
		IL_003e: ldstr "Rumble"
		IL_0043: ldc.i4.1
		IL_0044: call void [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::SetInt(string, int32)
		IL_0049: ldarg.0
		IL_004a: ldfld class [UnityEngine.UI]UnityEngine.UI.Toggle OptionsManager::analog
		IL_004f: ldfld bool [UnityEngine.UI]UnityEngine.UI.Toggle::m_IsOn
		IL_0054: ldc.i4.0
		IL_0055: ceq
		IL_0057: ldc.i4.0
		IL_0058: ceq
		IL_005a: brtrue.s IL_0078

		IL_005c: nop
		IL_005d: ldstr "rcx"
		IL_0062: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0067: box [mscorlib]System.Int64
		IL_006c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<string>(object)
		IL_0071: ldc.i4.0
		IL_0072: call void [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::SetInt(string, int32)
		IL_0077: ret

		IL_0078: nop
		IL_0079: ldstr "rcx"
		IL_007e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0083: box [mscorlib]System.Int64
		IL_0088: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<string>(object)
		IL_008d: ldc.i4.1
		IL_008e: call void [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::SetInt(string, int32)
		IL_0093: ret

		IL_0094: nop
		IL_0095: ldnull
		IL_0096: throw
	} // end of method OptionsManager::Update

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x23ae
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method OptionsManager::.ctor

} // end of class OptionsManager
