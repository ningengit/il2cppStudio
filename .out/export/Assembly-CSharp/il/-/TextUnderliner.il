.class public auto ansi beforefieldinit TextUnderliner
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class [Unity.TextMeshPro]TMPro.TMP_Text text

	// Methods
	.method public hidebysig 
		instance void Underline () cil managed 
	{
		// Method begins at RVA 0x2714
		// Header size: 1
		// Code size: 27 (0x1b)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TextUnderliner::text
		IL_0007: ldc.i4.4
		IL_0008: box [mscorlib]System.Int32
		IL_000d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [Unity.TextMeshPro]TMPro.FontStyles>(object)
		IL_0012: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_fontStyle(valuetype [Unity.TextMeshPro]TMPro.FontStyles)
		IL_0017: ret

		IL_0018: nop
		IL_0019: ldnull
		IL_001a: throw
	} // end of method TextUnderliner::Underline

	.method public hidebysig 
		instance void Ununderline () cil managed 
	{
		// Method begins at RVA 0x2730
		// Header size: 1
		// Code size: 27 (0x1b)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TextUnderliner::text
		IL_0007: ldc.i4.0
		IL_0008: box [mscorlib]System.Int32
		IL_000d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [Unity.TextMeshPro]TMPro.FontStyles>(object)
		IL_0012: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_fontStyle(valuetype [Unity.TextMeshPro]TMPro.FontStyles)
		IL_0017: ret

		IL_0018: nop
		IL_0019: ldnull
		IL_001a: throw
	} // end of method TextUnderliner::Ununderline

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x274c
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method TextUnderliner::.ctor

} // end of class TextUnderliner
