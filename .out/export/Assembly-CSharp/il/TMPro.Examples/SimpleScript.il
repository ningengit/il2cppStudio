.class public auto ansi beforefieldinit TMPro.Examples.SimpleScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field private class [Unity.TextMeshPro]TMPro.TextMeshPro m_textMeshPro
	.field private static literal string label
	.field private float32 m_frame

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x16374
		// Header size: 12
		// Code size: 80 (0x50)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[1] class [Unity.TextMeshPro]TMPro.TextMeshPro
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class [Unity.TextMeshPro]TMPro.TextMeshPro>()
		IL_000e: stloc.1
		IL_000f: ldarg.0
		IL_0010: ldloc.1
		IL_0011: stfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.SimpleScript::m_textMeshPro
		IL_0016: ldarg.0
		IL_0017: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.SimpleScript::m_textMeshPro
		IL_001c: ldc.r4 48
		IL_0021: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_fontSize(float32)
		IL_0026: ldarg.0
		IL_0027: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.SimpleScript::m_textMeshPro
		IL_002c: ldc.i4 514
		IL_0031: box [mscorlib]System.Int32
		IL_0036: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions>(object)
		IL_003b: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_alignment(valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions)
		IL_0040: ldarg.0
		IL_0041: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.SimpleScript::m_textMeshPro
		IL_0046: ldc.i4.0
		IL_0047: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_enableWordWrapping(bool)
		IL_004c: ret

		IL_004d: nop
		IL_004e: ldnull
		IL_004f: throw
	} // end of method SimpleScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x163d0
		// Header size: 12
		// Code size: 107 (0x6b)
		.maxstack 5
		.locals init (
			[0] int64,
			[1] float32,
			[2] float32
		)

		IL_0000: nop
		IL_0001: ldstr "sub_20CB10"
		IL_0006: ldarg.0
		IL_0007: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_000c: ldc.i8 0
		IL_0015: ldc.i8 0
		IL_001e: ldc.i8 0
		IL_0027: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_002c: stloc.0
		IL_002d: ldarg.0
		IL_002e: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.SimpleScript::m_textMeshPro
		IL_0033: ldstr "The <#0050FF>count is: </color>{0:2}"
		IL_0038: ldstr "xmm0"
		IL_003d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0042: box [mscorlib]System.Int64
		IL_0047: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_004c: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::SetText(string, float32)
		IL_0051: ldarg.0
		IL_0052: ldfld float32 TMPro.Examples.SimpleScript::m_frame
		IL_0057: stloc.1
		IL_0058: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_005d: stloc.2
		IL_005e: ldarg.0
		IL_005f: ldloc.2
		IL_0060: ldloc.1
		IL_0061: add
		IL_0062: stfld float32 TMPro.Examples.SimpleScript::m_frame
		IL_0067: ret

		IL_0068: nop
		IL_0069: ldnull
		IL_006a: throw
	} // end of method SimpleScript::Update

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2919
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method SimpleScript::.ctor

} // end of class TMPro.Examples.SimpleScript
