.class public auto ansi beforefieldinit StartButton
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested public auto ansi sealed Mode
		extends [mscorlib]System.Enum
	{
		// Fields
		.field public specialname rtspecialname int32 value__
		.field public static literal valuetype StartButton/Mode Story
		.field public static literal valuetype StartButton/Mode Endless

	} // end of class Mode


	// Fields
	.field public valuetype StartButton/Mode currentMode
	.field public string LoadScene

	// Methods
	.method public hidebysig 
		instance void StartGame () cil managed 
	{
		// Method begins at RVA 0x5bbc
		// Header size: 12
		// Code size: 61 (0x3d)
		.maxstack 2
		.locals init (
			[0] string,
			[1] class [UnityEngine.CoreModule]UnityEngine.AsyncOperation
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld valuetype StartButton/Mode StartButton::currentMode
		IL_0007: box [mscorlib]System.Object
		IL_000c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0011: ldc.i4.0
		IL_0012: ceq
		IL_0014: ldc.i4.0
		IL_0015: ceq
		IL_0017: brtrue.s IL_001a

		IL_0019: nop

		IL_001a: nop
		IL_001b: ldstr "CurrentMode"
		IL_0020: ldstr "story"
		IL_0025: call void [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::SetString(string, string)
		IL_002a: ldarg.0
		IL_002b: ldfld string StartButton::LoadScene
		IL_0030: stloc.0
		IL_0031: br.s IL_0034

		IL_0033: nop

		IL_0034: nop
		IL_0035: ldloc.0
		IL_0036: call class [UnityEngine.CoreModule]UnityEngine.AsyncOperation [UnityEngine.CoreModule]UnityEngine.SceneManagement.SceneManager::LoadSceneAsync(string)
		IL_003b: stloc.1
		IL_003c: ret
	} // end of method StartButton::StartGame

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2240
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method StartButton::.ctor

} // end of class StartButton
