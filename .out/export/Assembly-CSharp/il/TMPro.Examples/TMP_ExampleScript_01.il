.class public auto ansi beforefieldinit TMPro.Examples.TMP_ExampleScript_01
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested public auto ansi sealed objectType
		extends [mscorlib]System.Enum
	{
		// Fields
		.field public specialname rtspecialname int32 value__
		.field public static literal valuetype TMPro.Examples.TMP_ExampleScript_01/objectType TextMeshPro
		.field public static literal valuetype TMPro.Examples.TMP_ExampleScript_01/objectType TextMeshProUGUI

	} // end of class objectType


	// Fields
	.field public valuetype TMPro.Examples.TMP_ExampleScript_01/objectType ObjectType
	.field public bool isStatic
	.field private class [Unity.TextMeshPro]TMPro.TMP_Text m_text
	.field private static literal string k_label
	.field private int32 count

	// Methods
	.method private hidebysig 
		instance void Awake () cil managed 
	{
		// Method begins at RVA 0x170e8
		// Header size: 12
		// Code size: 244 (0xf4)
		.maxstack 3
		.locals init (
			[0] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[1] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[2] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[3] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[4] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[5] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[6] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[7] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[8] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[9] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[10] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[11] class [Unity.TextMeshPro]TMPro.TMP_FontAsset,
			[12] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[13] class [UnityEngine.CoreModule]UnityEngine.Material,
			[14] valuetype [UnityEngine.CoreModule]UnityEngine.Vector2,
			[15] class [UnityEngine.CoreModule]UnityEngine.RectTransform
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld valuetype TMPro.Examples.TMP_ExampleScript_01/objectType TMPro.Examples.TMP_ExampleScript_01::ObjectType
		IL_0007: box [mscorlib]System.Object
		IL_000c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0011: ldc.i4.0
		IL_0012: ceq
		IL_0014: brtrue.s IL_0035

		IL_0016: nop
		IL_0017: ldarg.0
		IL_0018: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [Unity.TextMeshPro]TMPro.TextMeshProUGUI>()
		IL_001d: stloc.0
		IL_001e: ldloc.0
		IL_001f: stloc.1
		IL_0020: ldloc.0
		IL_0021: ldnull
		IL_0022: ceq
		IL_0024: ldc.i4.0
		IL_0025: ceq
		IL_0027: brtrue.s IL_0067

		IL_0029: nop
		IL_002a: ldarg.0
		IL_002b: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_0030: stloc.2
		IL_0031: ldloc.2
		IL_0032: stloc.3
		IL_0033: br.s IL_0059

		IL_0035: nop
		IL_0036: ldarg.0
		IL_0037: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [Unity.TextMeshPro]TMPro.TextMeshPro>()
		IL_003c: stloc.s 4
		IL_003e: ldloc.s 4
		IL_0040: stloc.s 5
		IL_0042: ldloc.s 4
		IL_0044: ldnull
		IL_0045: ceq
		IL_0047: ldc.i4.0
		IL_0048: ceq
		IL_004a: brtrue.s IL_0067

		IL_004c: nop
		IL_004d: ldarg.0
		IL_004e: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_0053: stloc.s 6
		IL_0055: ldloc.s 6
		IL_0057: stloc.s 7

		IL_0059: nop
		IL_005a: ldloc.s 7
		IL_005c: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class [Unity.TextMeshPro]TMPro.TextMeshPro>()
		IL_0061: stloc.s 8
		IL_0063: ldloc.s 8
		IL_0065: stloc.s 9

		IL_0067: nop
		IL_0068: ldarg.0
		IL_0069: ldloc.s 9
		IL_006b: stfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.TMP_ExampleScript_01::m_text
		IL_0070: ldarg.0
		IL_0071: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.TMP_ExampleScript_01::m_text
		IL_0076: stloc.s 10
		IL_0078: ldstr "Fonts & Materials/Anton SDF"
		IL_007d: call !!0 [UnityEngine.CoreModule]UnityEngine.Resources::Load<class [Unity.TextMeshPro]TMPro.TMP_FontAsset>(string)
		IL_0082: stloc.s 11
		IL_0084: ldloc.s 10
		IL_0086: ldloc.s 11
		IL_0088: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_font(class [Unity.TextMeshPro]TMPro.TMP_FontAsset)
		IL_008d: ldarg.0
		IL_008e: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.TMP_ExampleScript_01::m_text
		IL_0093: stloc.s 12
		IL_0095: ldstr "Fonts & Materials/Anton SDF - Drop Shadow"
		IL_009a: call !!0 [UnityEngine.CoreModule]UnityEngine.Resources::Load<class [UnityEngine.CoreModule]UnityEngine.Material>(string)
		IL_009f: stloc.s 13
		IL_00a1: ldarg.0
		IL_00a2: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.TMP_ExampleScript_01::m_text
		IL_00a7: ldc.r4 120
		IL_00ac: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_fontSize(float32)
		IL_00b1: ldarg.0
		IL_00b2: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.TMP_ExampleScript_01::m_text
		IL_00b7: ldc.r4 (00 00 80 7F)
		IL_00bc: ldc.r4 (00 00 80 7F)
		IL_00c1: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector2 [Unity.TextMeshPro]TMPro.TMP_Text::GetPreferredValues(float32, float32)
		IL_00c6: stloc.s 14
		IL_00c8: ldarg.0
		IL_00c9: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.TMP_ExampleScript_01::m_text
		IL_00ce: callvirt instance class [UnityEngine.CoreModule]UnityEngine.RectTransform [Unity.TextMeshPro]TMPro.TMP_Text::get_rectTransform()
		IL_00d3: stloc.s 15
		IL_00d5: ldloc.s 15
		IL_00d7: ldstr "unpcklps xmm6,xmm7"
		IL_00dc: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00e1: box [mscorlib]System.Int64
		IL_00e6: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector2>(object)
		IL_00eb: callvirt instance void [UnityEngine.CoreModule]UnityEngine.RectTransform::set_sizeDelta(valuetype [UnityEngine.CoreModule]UnityEngine.Vector2)
		IL_00f0: ret

		IL_00f1: nop
		IL_00f2: ldnull
		IL_00f3: throw
	} // end of method TMP_ExampleScript_01::Awake

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x171e8
		// Header size: 12
		// Code size: 100 (0x64)
		.maxstack 6

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld bool TMPro.Examples.TMP_ExampleScript_01::isStatic
		IL_0007: ldc.i4.0
		IL_0008: ceq
		IL_000a: ldc.i4.0
		IL_000b: ceq
		IL_000d: brtrue.s IL_005f

		IL_000f: nop
		IL_0010: ldarg.0
		IL_0011: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.Examples.TMP_ExampleScript_01::m_text
		IL_0016: ldstr "The count is <#0080ff>{0}</color>"
		IL_001b: ldstr "imul r8d"
		IL_0020: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0025: conv.i4
		IL_0026: ldstr "rdx"
		IL_002b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0030: conv.i4
		IL_0031: ldc.i4.6
		IL_0032: shr
		IL_0033: ldstr "rdx"
		IL_0038: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_003d: conv.i4
		IL_003e: ldc.i4.6
		IL_003f: shr
		IL_0040: ldc.i4.s 31
		IL_0042: shr.un
		IL_0043: add
		IL_0044: ldc.i4 1000
		IL_0049: mul
		IL_004a: sub
		IL_004b: conv.r4
		IL_004c: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::SetText(string, float32)
		IL_0051: ldarg.0
		IL_0052: ldarg.0
		IL_0053: ldfld int32 TMPro.Examples.TMP_ExampleScript_01::count
		IL_0058: ldc.i4.1
		IL_0059: add
		IL_005a: stfld int32 TMPro.Examples.TMP_ExampleScript_01::count

		IL_005f: nop
		IL_0060: ret

		IL_0061: nop
		IL_0062: ldnull
		IL_0063: throw
	} // end of method TMP_ExampleScript_01::Update

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2982
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method TMP_ExampleScript_01::.ctor

} // end of class TMPro.Examples.TMP_ExampleScript_01
