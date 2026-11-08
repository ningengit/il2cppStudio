.class public auto ansi beforefieldinit TMPro.Examples.TMP_UiFrameRateCounter
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested public auto ansi sealed FpsCounterAnchorPositions
		extends [mscorlib]System.Enum
	{
		// Fields
		.field public specialname rtspecialname int32 value__
		.field public static literal valuetype TMPro.Examples.TMP_UiFrameRateCounter/FpsCounterAnchorPositions TopLeft
		.field public static literal valuetype TMPro.Examples.TMP_UiFrameRateCounter/FpsCounterAnchorPositions BottomLeft
		.field public static literal valuetype TMPro.Examples.TMP_UiFrameRateCounter/FpsCounterAnchorPositions TopRight
		.field public static literal valuetype TMPro.Examples.TMP_UiFrameRateCounter/FpsCounterAnchorPositions BottomRight

	} // end of class FpsCounterAnchorPositions


	// Fields
	.field public float32 UpdateInterval
	.field private float32 m_LastInterval
	.field private int32 m_Frames
	.field public valuetype TMPro.Examples.TMP_UiFrameRateCounter/FpsCounterAnchorPositions AnchorPosition
	.field private string htmlColorTag
	.field private static literal string fpsLabel
	.field private class [Unity.TextMeshPro]TMPro.TextMeshProUGUI m_TextMeshPro
	.field private class [UnityEngine.CoreModule]UnityEngine.RectTransform m_frameCounter_transform
	.field private valuetype TMPro.Examples.TMP_UiFrameRateCounter/FpsCounterAnchorPositions last_AnchorPosition

	// Methods
	.method private hidebysig 
		instance void Awake () cil managed 
	{
		// Method begins at RVA 0x1909c
		// Header size: 12
		// Code size: 255 (0xff)
		.maxstack 3
		.locals init (
			[0] bool,
			[1] bool,
			[2] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[3] class [UnityEngine.CoreModule]UnityEngine.RectTransform,
			[4] class [UnityEngine.CoreModule]UnityEngine.RectTransform,
			[5] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[6] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[7] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[8] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[9] class [Unity.TextMeshPro]TMPro.TMP_FontAsset,
			[10] class [Unity.TextMeshPro]TMPro.TMP_FontAsset,
			[11] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[12] class [UnityEngine.CoreModule]UnityEngine.Material,
			[13] class [UnityEngine.CoreModule]UnityEngine.Material,
			[14] valuetype TMPro.Examples.TMP_UiFrameRateCounter/FpsCounterAnchorPositions,
			[15] valuetype TMPro.Examples.TMP_UiFrameRateCounter/FpsCounterAnchorPositions
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance bool [UnityEngine.CoreModule]UnityEngine.Behaviour::get_enabled()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: stloc.1
		IL_000a: ldloc.0
		IL_000b: ldc.i4.0
		IL_000c: ceq
		IL_000e: brtrue IL_00fa

		IL_0013: nop
		IL_0014: br.s IL_0017

		IL_0016: nop

		IL_0017: nop
		IL_0018: ldc.i4.m1
		IL_0019: call void [UnityEngine.CoreModule]UnityEngine.Application::set_targetFrameRate(int32)
		IL_001e: ldstr "Frame Counter"
		IL_0023: newobj instance void [UnityEngine.CoreModule]UnityEngine.GameObject::.ctor(string)
		IL_0028: stloc.2
		IL_0029: ldloc.2
		IL_002a: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class [UnityEngine.CoreModule]UnityEngine.RectTransform>()
		IL_002f: stloc.3
		IL_0030: ldarg.0
		IL_0031: ldloc.3
		IL_0032: stfld class [UnityEngine.CoreModule]UnityEngine.RectTransform TMPro.Examples.TMP_UiFrameRateCounter::m_frameCounter_transform
		IL_0037: ldarg.0
		IL_0038: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform TMPro.Examples.TMP_UiFrameRateCounter::m_frameCounter_transform
		IL_003d: stloc.s 4
		IL_003f: ldarg.0
		IL_0040: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0045: stloc.s 5
		IL_0047: ldloc.s 5
		IL_0049: stloc.s 6
		IL_004b: ldloc.s 4
		IL_004d: ldloc.s 6
		IL_004f: ldc.i4.0
		IL_0050: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::SetParent(class [UnityEngine.CoreModule]UnityEngine.Transform, bool)
		IL_0055: ldloc.2
		IL_0056: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class [Unity.TextMeshPro]TMPro.TextMeshProUGUI>()
		IL_005b: stloc.s 7
		IL_005d: ldarg.0
		IL_005e: ldloc.s 7
		IL_0060: stfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.TMP_UiFrameRateCounter::m_TextMeshPro
		IL_0065: ldarg.0
		IL_0066: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.TMP_UiFrameRateCounter::m_TextMeshPro
		IL_006b: stloc.s 8
		IL_006d: ldstr "Fonts & Materials/LiberationSans SDF"
		IL_0072: call !!0 [UnityEngine.CoreModule]UnityEngine.Resources::Load<class [Unity.TextMeshPro]TMPro.TMP_FontAsset>(string)
		IL_0077: stloc.s 9
		IL_0079: ldloc.s 9
		IL_007b: stloc.s 10
		IL_007d: ldloc.s 8
		IL_007f: ldloc.s 10
		IL_0081: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_font(class [Unity.TextMeshPro]TMPro.TMP_FontAsset)
		IL_0086: ldarg.0
		IL_0087: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.TMP_UiFrameRateCounter::m_TextMeshPro
		IL_008c: stloc.s 11
		IL_008e: ldstr "Fonts & Materials/LiberationSans SDF - Overlay"
		IL_0093: call !!0 [UnityEngine.CoreModule]UnityEngine.Resources::Load<class [UnityEngine.CoreModule]UnityEngine.Material>(string)
		IL_0098: stloc.s 12
		IL_009a: ldloc.s 12
		IL_009c: stloc.s 13
		IL_009e: ldarg.0
		IL_009f: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.TMP_UiFrameRateCounter::m_TextMeshPro
		IL_00a4: ldc.i4.0
		IL_00a5: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_enableWordWrapping(bool)
		IL_00aa: ldarg.0
		IL_00ab: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.TMP_UiFrameRateCounter::m_TextMeshPro
		IL_00b0: ldc.r4 36
		IL_00b5: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_fontSize(float32)
		IL_00ba: ldarg.0
		IL_00bb: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.TMP_UiFrameRateCounter::m_TextMeshPro
		IL_00c0: ldc.i4.1
		IL_00c1: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_isOverlay(bool)
		IL_00c6: ldarg.0
		IL_00c7: ldarg.0
		IL_00c8: ldfld valuetype TMPro.Examples.TMP_UiFrameRateCounter/FpsCounterAnchorPositions TMPro.Examples.TMP_UiFrameRateCounter::AnchorPosition
		IL_00cd: callvirt instance void TMPro.Examples.TMP_UiFrameRateCounter::Set_FrameCounter_Position(valuetype TMPro.Examples.TMP_UiFrameRateCounter/FpsCounterAnchorPositions)
		IL_00d2: ldarg.0
		IL_00d3: ldfld valuetype TMPro.Examples.TMP_UiFrameRateCounter/FpsCounterAnchorPositions TMPro.Examples.TMP_UiFrameRateCounter::AnchorPosition
		IL_00d8: stloc.s 14
		IL_00da: ldarg.0
		IL_00db: ldloc.s 14
		IL_00dd: box [mscorlib]System.Object
		IL_00e2: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_00e7: box [mscorlib]System.Int32
		IL_00ec: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype TMPro.Examples.TMP_UiFrameRateCounter/FpsCounterAnchorPositions>(object)
		IL_00f1: stfld valuetype TMPro.Examples.TMP_UiFrameRateCounter/FpsCounterAnchorPositions TMPro.Examples.TMP_UiFrameRateCounter::last_AnchorPosition
		IL_00f6: ldloc.s 14
		IL_00f8: stloc.s 15

		IL_00fa: nop
		IL_00fb: ret

		IL_00fc: nop
		IL_00fd: ldnull
		IL_00fe: throw
	} // end of method TMP_UiFrameRateCounter::Awake

	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x191a8
		// Header size: 12
		// Code size: 22 (0x16)
		.maxstack 2
		.locals init (
			[0] float32
		)

		IL_0000: nop
		IL_0001: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_realtimeSinceStartup()
		IL_0006: stloc.0
		IL_0007: ldarg.0
		IL_0008: ldloc.0
		IL_0009: stfld float32 TMPro.Examples.TMP_UiFrameRateCounter::m_LastInterval
		IL_000e: ldarg.0
		IL_000f: ldc.i4.0
		IL_0010: stfld int32 TMPro.Examples.TMP_UiFrameRateCounter::m_Frames
		IL_0015: ret
	} // end of method TMP_UiFrameRateCounter::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x191cc
		// Header size: 12
		// Code size: 341 (0x155)
		.maxstack 4
		.locals init (
			[0] valuetype TMPro.Examples.TMP_UiFrameRateCounter/FpsCounterAnchorPositions,
			[1] float32,
			[2] float32,
			[3] float32,
			[4] int64,
			[5] int64,
			[6] float32,
			[7] float32,
			[8] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[9] string,
			[10] string
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld valuetype TMPro.Examples.TMP_UiFrameRateCounter/FpsCounterAnchorPositions TMPro.Examples.TMP_UiFrameRateCounter::AnchorPosition
		IL_0007: box [mscorlib]System.Object
		IL_000c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0011: ldarg.0
		IL_0012: ldfld valuetype TMPro.Examples.TMP_UiFrameRateCounter/FpsCounterAnchorPositions TMPro.Examples.TMP_UiFrameRateCounter::last_AnchorPosition
		IL_0017: box [mscorlib]System.Object
		IL_001c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0021: ceq
		IL_0023: brtrue.s IL_0032

		IL_0025: nop
		IL_0026: ldarg.0
		IL_0027: ldarg.0
		IL_0028: ldfld valuetype TMPro.Examples.TMP_UiFrameRateCounter/FpsCounterAnchorPositions TMPro.Examples.TMP_UiFrameRateCounter::AnchorPosition
		IL_002d: callvirt instance void TMPro.Examples.TMP_UiFrameRateCounter::Set_FrameCounter_Position(valuetype TMPro.Examples.TMP_UiFrameRateCounter/FpsCounterAnchorPositions)

		IL_0032: nop
		IL_0033: ldarg.0
		IL_0034: ldfld valuetype TMPro.Examples.TMP_UiFrameRateCounter/FpsCounterAnchorPositions TMPro.Examples.TMP_UiFrameRateCounter::AnchorPosition
		IL_0039: stloc.0
		IL_003a: ldarg.0
		IL_003b: ldarg.0
		IL_003c: ldfld int32 TMPro.Examples.TMP_UiFrameRateCounter::m_Frames
		IL_0041: ldc.i4.1
		IL_0042: add
		IL_0043: stfld int32 TMPro.Examples.TMP_UiFrameRateCounter::m_Frames
		IL_0048: ldarg.0
		IL_0049: ldloc.0
		IL_004a: box [mscorlib]System.Object
		IL_004f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0054: box [mscorlib]System.Int32
		IL_0059: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype TMPro.Examples.TMP_UiFrameRateCounter/FpsCounterAnchorPositions>(object)
		IL_005e: stfld valuetype TMPro.Examples.TMP_UiFrameRateCounter/FpsCounterAnchorPositions TMPro.Examples.TMP_UiFrameRateCounter::last_AnchorPosition
		IL_0063: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_realtimeSinceStartup()
		IL_0068: stloc.1
		IL_0069: ldloc.1
		IL_006a: stloc.2
		IL_006b: ldloc.1
		IL_006c: stloc.3
		IL_006d: ldloc.1
		IL_006e: ldarg.0
		IL_006f: ldfld float32 TMPro.Examples.TMP_UiFrameRateCounter::m_LastInterval
		IL_0074: ldarg.0
		IL_0075: ldfld float32 TMPro.Examples.TMP_UiFrameRateCounter::UpdateInterval
		IL_007a: add
		IL_007b: cgt
		IL_007d: ldc.i4.0
		IL_007e: ceq
		IL_0080: brtrue IL_0150

		IL_0085: nop
		IL_0086: ldarg.0
		IL_0087: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_008c: ldc.i8 48
		IL_0095: add
		IL_0096: stloc.s 4
		IL_0098: ldstr "maxss xmm1,dword ptr [1813EAC30h]"
		IL_009d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00a2: stloc.s 5
		IL_00a4: ldc.r4 1000
		IL_00a9: ldstr "maxss xmm1,dword ptr [1813EAC30h]"
		IL_00ae: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00b3: conv.r4
		IL_00b4: div
		IL_00b5: stloc.s 6
		IL_00b7: ldarg.0
		IL_00b8: ldfld int32 TMPro.Examples.TMP_UiFrameRateCounter::m_Frames
		IL_00bd: conv.r4
		IL_00be: ldloc.2
		IL_00bf: ldarg.0
		IL_00c0: ldfld float32 TMPro.Examples.TMP_UiFrameRateCounter::m_LastInterval
		IL_00c5: sub
		IL_00c6: div
		IL_00c7: stloc.s 7
		IL_00c9: ldc.r4 30
		IL_00ce: ldarg.0
		IL_00cf: ldfld int32 TMPro.Examples.TMP_UiFrameRateCounter::m_Frames
		IL_00d4: conv.r4
		IL_00d5: ldloc.2
		IL_00d6: ldarg.0
		IL_00d7: ldfld float32 TMPro.Examples.TMP_UiFrameRateCounter::m_LastInterval
		IL_00dc: sub
		IL_00dd: div
		IL_00de: cgt
		IL_00e0: brtrue.s IL_010a

		IL_00e2: nop
		IL_00e3: ldc.r4 10
		IL_00e8: ldloc.s 7
		IL_00ea: cgt
		IL_00ec: brtrue.s IL_00fc

		IL_00ee: nop
		IL_00ef: ldarg.0
		IL_00f0: ldstr "<color=green>"
		IL_00f5: stfld string TMPro.Examples.TMP_UiFrameRateCounter::htmlColorTag
		IL_00fa: br.s IL_0116

		IL_00fc: nop
		IL_00fd: ldarg.0
		IL_00fe: ldstr "<color=red>"
		IL_0103: stfld string TMPro.Examples.TMP_UiFrameRateCounter::htmlColorTag
		IL_0108: br.s IL_0116

		IL_010a: nop
		IL_010b: ldarg.0
		IL_010c: ldstr "<color=yellow>"
		IL_0111: stfld string TMPro.Examples.TMP_UiFrameRateCounter::htmlColorTag

		IL_0116: nop
		IL_0117: ldarg.0
		IL_0118: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.TMP_UiFrameRateCounter::m_TextMeshPro
		IL_011d: stloc.s 8
		IL_011f: ldarg.0
		IL_0120: ldfld string TMPro.Examples.TMP_UiFrameRateCounter::htmlColorTag
		IL_0125: ldstr "{0:2}</color> <#8080ff>FPS \n<#FF8000>{1:2} <#8080ff>MS"
		IL_012a: call string [mscorlib]System.String::Concat(string, string)
		IL_012f: stloc.s 9
		IL_0131: ldloc.s 9
		IL_0133: stloc.s 10
		IL_0135: ldloc.s 8
		IL_0137: ldloc.s 10
		IL_0139: ldloc.s 7
		IL_013b: ldloc.s 6
		IL_013d: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::SetText(string, float32, float32)
		IL_0142: ldarg.0
		IL_0143: ldloc.3
		IL_0144: stfld float32 TMPro.Examples.TMP_UiFrameRateCounter::m_LastInterval
		IL_0149: ldarg.0
		IL_014a: ldc.i4.0
		IL_014b: stfld int32 TMPro.Examples.TMP_UiFrameRateCounter::m_Frames

		IL_0150: nop
		IL_0151: ret

		IL_0152: nop
		IL_0153: ldnull
		IL_0154: throw
	} // end of method TMP_UiFrameRateCounter::Update

	.method private hidebysig 
		instance void Set_FrameCounter_Position (
			valuetype TMPro.Examples.TMP_UiFrameRateCounter/FpsCounterAnchorPositions anchor_position
		) cil managed 
	{
		// Method begins at RVA 0x19330
		// Header size: 12
		// Code size: 596 (0x254)
		.maxstack 2
		.locals init (
			[0] int32,
			[1] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[2] class [UnityEngine.CoreModule]UnityEngine.RectTransform,
			[3] int64,
			[4] int64,
			[5] class [UnityEngine.CoreModule]UnityEngine.RectTransform
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: box [mscorlib]System.Object
		IL_0007: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_000c: ldarg.1
		IL_000d: box [mscorlib]System.Object
		IL_0012: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0017: and
		IL_0018: ldc.i4.0
		IL_0019: ceq
		IL_001b: brtrue IL_01f4

		IL_0020: nop
		IL_0021: ldarg.1
		IL_0022: box [mscorlib]System.Object
		IL_0027: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_002c: ldc.i4.1
		IL_002d: sub
		IL_002e: stloc.0
		IL_002f: ldarg.1
		IL_0030: box [mscorlib]System.Object
		IL_0035: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_003a: ldc.i4.1
		IL_003b: sub
		IL_003c: ldc.i4.0
		IL_003d: ceq
		IL_003f: brtrue IL_0152

		IL_0044: nop
		IL_0045: ldloc.0
		IL_0046: ldc.i4.1
		IL_0047: sub
		IL_0048: stloc.0
		IL_0049: ldloc.0
		IL_004a: ldc.i4.1
		IL_004b: sub
		IL_004c: ldc.i4.0
		IL_004d: ceq
		IL_004f: brtrue IL_0108

		IL_0054: nop
		IL_0055: ldloc.0
		IL_0056: ldc.i4.1
		IL_0057: ceq
		IL_0059: ldc.i4.0
		IL_005a: ceq
		IL_005c: brtrue IL_0106

		IL_0061: nop
		IL_0062: ldarg.0
		IL_0063: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.TMP_UiFrameRateCounter::m_TextMeshPro
		IL_0068: stloc.1
		IL_0069: ldloc.1
		IL_006a: ldc.i4 1028
		IL_006f: box [mscorlib]System.Int32
		IL_0074: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions>(object)
		IL_0079: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_alignment(valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions)
		IL_007e: ldarg.0
		IL_007f: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform TMPro.Examples.TMP_UiFrameRateCounter::m_frameCounter_transform
		IL_0084: stloc.2
		IL_0085: ldloc.2
		IL_0086: ldstr "unpcklps xmm0,xmm8"
		IL_008b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0090: box [mscorlib]System.Int64
		IL_0095: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector2>(object)
		IL_009a: callvirt instance void [UnityEngine.CoreModule]UnityEngine.RectTransform::set_pivot(valuetype [UnityEngine.CoreModule]UnityEngine.Vector2)
		IL_009f: ldarg.0
		IL_00a0: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform TMPro.Examples.TMP_UiFrameRateCounter::m_frameCounter_transform
		IL_00a5: stloc.2

		IL_00a6: nop
		IL_00a7: ldloc.2
		IL_00a8: ldstr "unpcklps xmm0,xmm9"
		IL_00ad: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00b2: box [mscorlib]System.Int64
		IL_00b7: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector2>(object)
		IL_00bc: callvirt instance void [UnityEngine.CoreModule]UnityEngine.RectTransform::set_anchorMin(valuetype [UnityEngine.CoreModule]UnityEngine.Vector2)
		IL_00c1: ldarg.0
		IL_00c2: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform TMPro.Examples.TMP_UiFrameRateCounter::m_frameCounter_transform
		IL_00c7: stloc.2
		IL_00c8: ldloc.2
		IL_00c9: ldstr "unpcklps xmm7,xmm9"
		IL_00ce: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00d3: box [mscorlib]System.Int64
		IL_00d8: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector2>(object)
		IL_00dd: callvirt instance void [UnityEngine.CoreModule]UnityEngine.RectTransform::set_anchorMax(valuetype [UnityEngine.CoreModule]UnityEngine.Vector2)
		IL_00e2: ldarg.0
		IL_00e3: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform TMPro.Examples.TMP_UiFrameRateCounter::m_frameCounter_transform
		IL_00e8: stloc.2
		IL_00e9: ldstr "unpcklps xmm6,xmm8"
		IL_00ee: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00f3: stloc.3

		IL_00f4: nop
		IL_00f5: ldloc.2
		IL_00f6: ldloc.3
		IL_00f7: box [mscorlib]System.Int64
		IL_00fc: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector2>(object)
		IL_0101: callvirt instance void [UnityEngine.CoreModule]UnityEngine.RectTransform::set_anchoredPosition(valuetype [UnityEngine.CoreModule]UnityEngine.Vector2)

		IL_0106: nop
		IL_0107: ret

		IL_0108: nop
		IL_0109: ldstr "*(LocalExpr + 0x38)"
		IL_010e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0113: box [mscorlib]System.Int64
		IL_0118: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [Unity.TextMeshPro]TMPro.TMP_Text>(object)
		IL_011d: ldc.i4 260
		IL_0122: box [mscorlib]System.Int32
		IL_0127: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions>(object)
		IL_012c: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_alignment(valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions)
		IL_0131: ldarg.0
		IL_0132: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform TMPro.Examples.TMP_UiFrameRateCounter::m_frameCounter_transform
		IL_0137: ldstr "unpcklps xmm0,xmm6"
		IL_013c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0141: box [mscorlib]System.Int64
		IL_0146: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector2>(object)
		IL_014b: callvirt instance void [UnityEngine.CoreModule]UnityEngine.RectTransform::set_pivot(valuetype [UnityEngine.CoreModule]UnityEngine.Vector2)
		IL_0150: br.s IL_019a

		IL_0152: nop
		IL_0153: ldstr "*(LocalExpr + 0x38)"
		IL_0158: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_015d: box [mscorlib]System.Int64
		IL_0162: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [Unity.TextMeshPro]TMPro.TMP_Text>(object)
		IL_0167: ldc.i4 1025
		IL_016c: box [mscorlib]System.Int32
		IL_0171: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions>(object)
		IL_0176: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_alignment(valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions)
		IL_017b: ldarg.0
		IL_017c: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform TMPro.Examples.TMP_UiFrameRateCounter::m_frameCounter_transform
		IL_0181: ldstr "unpcklps xmm0,xmm6"
		IL_0186: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_018b: box [mscorlib]System.Int64
		IL_0190: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector2>(object)
		IL_0195: callvirt instance void [UnityEngine.CoreModule]UnityEngine.RectTransform::set_pivot(valuetype [UnityEngine.CoreModule]UnityEngine.Vector2)

		IL_019a: nop
		IL_019b: ldarg.0
		IL_019c: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform TMPro.Examples.TMP_UiFrameRateCounter::m_frameCounter_transform
		IL_01a1: stloc.2
		IL_01a2: ldloc.2
		IL_01a3: ldstr "unpcklps xmm0,xmm7"
		IL_01a8: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01ad: box [mscorlib]System.Int64
		IL_01b2: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector2>(object)
		IL_01b7: callvirt instance void [UnityEngine.CoreModule]UnityEngine.RectTransform::set_anchorMin(valuetype [UnityEngine.CoreModule]UnityEngine.Vector2)
		IL_01bc: ldarg.0
		IL_01bd: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform TMPro.Examples.TMP_UiFrameRateCounter::m_frameCounter_transform
		IL_01c2: stloc.2
		IL_01c3: ldloc.2
		IL_01c4: ldstr "unpcklps xmm7,xmm7"
		IL_01c9: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01ce: box [mscorlib]System.Int64
		IL_01d3: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector2>(object)
		IL_01d8: callvirt instance void [UnityEngine.CoreModule]UnityEngine.RectTransform::set_anchorMax(valuetype [UnityEngine.CoreModule]UnityEngine.Vector2)
		IL_01dd: ldarg.0
		IL_01de: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform TMPro.Examples.TMP_UiFrameRateCounter::m_frameCounter_transform
		IL_01e3: stloc.2
		IL_01e4: ldstr "unpcklps xmm6,xmm6"
		IL_01e9: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01ee: stloc.3
		IL_01ef: br IL_00f4

		IL_01f4: nop
		IL_01f5: ldstr "*(LocalExpr + 0x38)"
		IL_01fa: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01ff: stloc.s 4
		IL_0201: ldloc.s 4
		IL_0203: box [mscorlib]System.Int64
		IL_0208: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [Unity.TextMeshPro]TMPro.TMP_Text>(object)
		IL_020d: ldc.i4 257
		IL_0212: box [mscorlib]System.Int32
		IL_0217: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions>(object)
		IL_021c: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_alignment(valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions)
		IL_0221: ldarg.0
		IL_0222: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform TMPro.Examples.TMP_UiFrameRateCounter::m_frameCounter_transform
		IL_0227: stloc.s 5
		IL_0229: ldloc.s 5
		IL_022b: ldstr "unpcklps xmm0,xmm8"
		IL_0230: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0235: box [mscorlib]System.Int64
		IL_023a: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector2>(object)
		IL_023f: callvirt instance void [UnityEngine.CoreModule]UnityEngine.RectTransform::set_pivot(valuetype [UnityEngine.CoreModule]UnityEngine.Vector2)
		IL_0244: ldarg.0
		IL_0245: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform TMPro.Examples.TMP_UiFrameRateCounter::m_frameCounter_transform
		IL_024a: stloc.s 5
		IL_024c: br IL_00a6

		IL_0251: nop
		IL_0252: ldnull
		IL_0253: throw
	} // end of method TMP_UiFrameRateCounter::Set_FrameCounter_Position

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2a4c
		// Header size: 1
		// Code size: 36 (0x24)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.r4 5
		IL_0007: stfld float32 TMPro.Examples.TMP_UiFrameRateCounter::UpdateInterval
		IL_000c: ldarg.0
		IL_000d: ldc.i4.2
		IL_000e: box [mscorlib]System.Int32
		IL_0013: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype TMPro.Examples.TMP_UiFrameRateCounter/FpsCounterAnchorPositions>(object)
		IL_0018: stfld valuetype TMPro.Examples.TMP_UiFrameRateCounter/FpsCounterAnchorPositions TMPro.Examples.TMP_UiFrameRateCounter::AnchorPosition
		IL_001d: ldarg.0
		IL_001e: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0023: ret
	} // end of method TMP_UiFrameRateCounter::.ctor

} // end of class TMPro.Examples.TMP_UiFrameRateCounter
