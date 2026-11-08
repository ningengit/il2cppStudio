.class public auto ansi beforefieldinit MouseSliderScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class [UnityEngine.UI]UnityEngine.UI.Slider slider

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0xa66c
		// Header size: 12
		// Code size: 65 (0x41)
		.maxstack 2
		.locals init (
			[0] float32,
			[1] float32,
			[2] class [UnityEngine.UI]UnityEngine.UI.Slider,
			[3] float32
		)

		IL_0000: nop
		IL_0001: ldstr "MouseSensitivity"
		IL_0006: call float32 [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::GetFloat(string)
		IL_000b: stloc.0
		IL_000c: ldloc.0
		IL_000d: stloc.1
		IL_000e: ldc.r4 100
		IL_0013: ldloc.0
		IL_0014: cgt
		IL_0016: ldc.i4.0
		IL_0017: ceq
		IL_0019: brtrue.s IL_002b

		IL_001b: nop
		IL_001c: ldstr "MouseSensitivity"
		IL_0021: ldc.r4 200
		IL_0026: call void [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::SetFloat(string, float32)

		IL_002b: nop
		IL_002c: ldarg.0
		IL_002d: ldfld class [UnityEngine.UI]UnityEngine.UI.Slider MouseSliderScript::slider
		IL_0032: stloc.2
		IL_0033: ldstr "MouseSensitivity"
		IL_0038: call float32 [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::GetFloat(string)
		IL_003d: stloc.3
		IL_003e: nop
		IL_003f: ldnull
		IL_0040: throw
	} // end of method MouseSliderScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x2381
		// Header size: 1
		// Code size: 35 (0x23)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldstr "MouseSensitivity"
		IL_0006: ldstr "xmm0"
		IL_000b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0010: box [mscorlib]System.Int64
		IL_0015: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_001a: call void [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::SetFloat(string, float32)
		IL_001f: ret

		IL_0020: nop
		IL_0021: ldnull
		IL_0022: throw
	} // end of method MouseSliderScript::Update

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x23a5
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method MouseSliderScript::.ctor

} // end of class MouseSliderScript
