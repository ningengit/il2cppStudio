.class public auto ansi beforefieldinit TMPro.Examples.TMP_FrameRateCounter
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested public auto ansi sealed FpsCounterAnchorPositions
		extends [mscorlib]System.Enum
	{
		// Fields
		.field public specialname rtspecialname int32 value__
		.field public static literal valuetype TMPro.Examples.TMP_FrameRateCounter/FpsCounterAnchorPositions TopLeft
		.field public static literal valuetype TMPro.Examples.TMP_FrameRateCounter/FpsCounterAnchorPositions BottomLeft
		.field public static literal valuetype TMPro.Examples.TMP_FrameRateCounter/FpsCounterAnchorPositions TopRight
		.field public static literal valuetype TMPro.Examples.TMP_FrameRateCounter/FpsCounterAnchorPositions BottomRight

	} // end of class FpsCounterAnchorPositions


	// Fields
	.field public float32 UpdateInterval
	.field private float32 m_LastInterval
	.field private int32 m_Frames
	.field public valuetype TMPro.Examples.TMP_FrameRateCounter/FpsCounterAnchorPositions AnchorPosition
	.field private string htmlColorTag
	.field private static literal string fpsLabel
	.field private class [Unity.TextMeshPro]TMPro.TextMeshPro m_TextMeshPro
	.field private class [UnityEngine.CoreModule]UnityEngine.Transform m_frameCounter_transform
	.field private class [UnityEngine.CoreModule]UnityEngine.Camera m_camera
	.field private valuetype TMPro.Examples.TMP_FrameRateCounter/FpsCounterAnchorPositions last_AnchorPosition

	// Methods
	.method private hidebysig 
		instance void Awake () cil managed 
	{
		// Method begins at RVA 0x17258
		// Header size: 12
		// Code size: 309 (0x135)
		.maxstack 3
		.locals init (
			[0] bool,
			[1] bool,
			[2] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[3] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[4] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[5] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[6] class [Unity.TextMeshPro]TMPro.TMP_FontAsset,
			[7] class [Unity.TextMeshPro]TMPro.TMP_FontAsset,
			[8] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[9] class [UnityEngine.CoreModule]UnityEngine.Material,
			[10] class [UnityEngine.CoreModule]UnityEngine.Material,
			[11] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[12] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[13] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[14] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[15] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[16] valuetype TMPro.Examples.TMP_FrameRateCounter/FpsCounterAnchorPositions,
			[17] valuetype TMPro.Examples.TMP_FrameRateCounter/FpsCounterAnchorPositions
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
		IL_000e: brtrue IL_0130

		IL_0013: nop
		IL_0014: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_0019: stloc.2
		IL_001a: ldarg.0
		IL_001b: ldloc.2
		IL_001c: stfld class [UnityEngine.CoreModule]UnityEngine.Camera TMPro.Examples.TMP_FrameRateCounter::m_camera
		IL_0021: br.s IL_0024

		IL_0023: nop

		IL_0024: nop
		IL_0025: ldc.i4.m1
		IL_0026: call void [UnityEngine.CoreModule]UnityEngine.Application::set_targetFrameRate(int32)
		IL_002b: ldstr "Frame Counter"
		IL_0030: newobj instance void [UnityEngine.CoreModule]UnityEngine.GameObject::.ctor(string)
		IL_0035: stloc.3
		IL_0036: ldloc.3
		IL_0037: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class [Unity.TextMeshPro]TMPro.TextMeshPro>()
		IL_003c: stloc.s 4
		IL_003e: ldarg.0
		IL_003f: ldloc.s 4
		IL_0041: stfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_FrameRateCounter::m_TextMeshPro
		IL_0046: ldarg.0
		IL_0047: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_FrameRateCounter::m_TextMeshPro
		IL_004c: stloc.s 5
		IL_004e: ldstr "Fonts & Materials/LiberationSans SDF"
		IL_0053: call !!0 [UnityEngine.CoreModule]UnityEngine.Resources::Load<class [Unity.TextMeshPro]TMPro.TMP_FontAsset>(string)
		IL_0058: stloc.s 6
		IL_005a: ldloc.s 6
		IL_005c: stloc.s 7
		IL_005e: ldloc.s 5
		IL_0060: ldloc.s 7
		IL_0062: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_font(class [Unity.TextMeshPro]TMPro.TMP_FontAsset)
		IL_0067: ldarg.0
		IL_0068: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_FrameRateCounter::m_TextMeshPro
		IL_006d: stloc.s 8
		IL_006f: ldstr "Fonts & Materials/LiberationSans SDF - Overlay"
		IL_0074: call !!0 [UnityEngine.CoreModule]UnityEngine.Resources::Load<class [UnityEngine.CoreModule]UnityEngine.Material>(string)
		IL_0079: stloc.s 9
		IL_007b: ldloc.s 9
		IL_007d: stloc.s 10
		IL_007f: ldloc.3
		IL_0080: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
		IL_0085: stloc.s 11
		IL_0087: ldarg.0
		IL_0088: ldloc.s 11
		IL_008a: stfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TMP_FrameRateCounter::m_frameCounter_transform
		IL_008f: ldarg.0
		IL_0090: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TMP_FrameRateCounter::m_frameCounter_transform
		IL_0095: stloc.s 12
		IL_0097: ldarg.0
		IL_0098: ldfld class [UnityEngine.CoreModule]UnityEngine.Camera TMPro.Examples.TMP_FrameRateCounter::m_camera
		IL_009d: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_00a2: stloc.s 13
		IL_00a4: ldloc.s 13
		IL_00a6: stloc.s 14
		IL_00a8: ldloc.s 12
		IL_00aa: ldloc.s 14
		IL_00ac: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_parentInternal(class [UnityEngine.CoreModule]UnityEngine.Transform)
		IL_00b1: ldarg.0
		IL_00b2: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TMP_FrameRateCounter::m_frameCounter_transform
		IL_00b7: ldloca.s 15
		IL_00b9: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Quaternion::identityQuaternion
		IL_00be: box [mscorlib]System.Object
		IL_00c3: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_00c8: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
		IL_00cd: ldloc.s 15
		IL_00cf: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_localRotation(valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion)
		IL_00d4: ldarg.0
		IL_00d5: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_FrameRateCounter::m_TextMeshPro
		IL_00da: ldc.i4.0
		IL_00db: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_enableWordWrapping(bool)
		IL_00e0: ldarg.0
		IL_00e1: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_FrameRateCounter::m_TextMeshPro
		IL_00e6: ldc.r4 24
		IL_00eb: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_fontSize(float32)
		IL_00f0: ldarg.0
		IL_00f1: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_FrameRateCounter::m_TextMeshPro
		IL_00f6: ldc.i4.1
		IL_00f7: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_isOverlay(bool)
		IL_00fc: ldarg.0
		IL_00fd: ldarg.0
		IL_00fe: ldfld valuetype TMPro.Examples.TMP_FrameRateCounter/FpsCounterAnchorPositions TMPro.Examples.TMP_FrameRateCounter::AnchorPosition
		IL_0103: callvirt instance void TMPro.Examples.TMP_FrameRateCounter::Set_FrameCounter_Position(valuetype TMPro.Examples.TMP_FrameRateCounter/FpsCounterAnchorPositions)
		IL_0108: ldarg.0
		IL_0109: ldfld valuetype TMPro.Examples.TMP_FrameRateCounter/FpsCounterAnchorPositions TMPro.Examples.TMP_FrameRateCounter::AnchorPosition
		IL_010e: stloc.s 16
		IL_0110: ldarg.0
		IL_0111: ldloc.s 16
		IL_0113: box [mscorlib]System.Object
		IL_0118: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_011d: box [mscorlib]System.Int32
		IL_0122: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype TMPro.Examples.TMP_FrameRateCounter/FpsCounterAnchorPositions>(object)
		IL_0127: stfld valuetype TMPro.Examples.TMP_FrameRateCounter/FpsCounterAnchorPositions TMPro.Examples.TMP_FrameRateCounter::last_AnchorPosition
		IL_012c: ldloc.s 16
		IL_012e: stloc.s 17

		IL_0130: nop
		IL_0131: ret

		IL_0132: nop
		IL_0133: ldnull
		IL_0134: throw
	} // end of method TMP_FrameRateCounter::Awake

	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x1739c
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
		IL_0009: stfld float32 TMPro.Examples.TMP_FrameRateCounter::m_LastInterval
		IL_000e: ldarg.0
		IL_000f: ldc.i4.0
		IL_0010: stfld int32 TMPro.Examples.TMP_FrameRateCounter::m_Frames
		IL_0015: ret
	} // end of method TMP_FrameRateCounter::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x173c0
		// Header size: 12
		// Code size: 341 (0x155)
		.maxstack 4
		.locals init (
			[0] valuetype TMPro.Examples.TMP_FrameRateCounter/FpsCounterAnchorPositions,
			[1] float32,
			[2] float32,
			[3] float32,
			[4] int64,
			[5] int64,
			[6] float32,
			[7] float32,
			[8] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[9] string,
			[10] string
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld valuetype TMPro.Examples.TMP_FrameRateCounter/FpsCounterAnchorPositions TMPro.Examples.TMP_FrameRateCounter::AnchorPosition
		IL_0007: box [mscorlib]System.Object
		IL_000c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0011: ldarg.0
		IL_0012: ldfld valuetype TMPro.Examples.TMP_FrameRateCounter/FpsCounterAnchorPositions TMPro.Examples.TMP_FrameRateCounter::last_AnchorPosition
		IL_0017: box [mscorlib]System.Object
		IL_001c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0021: ceq
		IL_0023: brtrue.s IL_0032

		IL_0025: nop
		IL_0026: ldarg.0
		IL_0027: ldarg.0
		IL_0028: ldfld valuetype TMPro.Examples.TMP_FrameRateCounter/FpsCounterAnchorPositions TMPro.Examples.TMP_FrameRateCounter::AnchorPosition
		IL_002d: callvirt instance void TMPro.Examples.TMP_FrameRateCounter::Set_FrameCounter_Position(valuetype TMPro.Examples.TMP_FrameRateCounter/FpsCounterAnchorPositions)

		IL_0032: nop
		IL_0033: ldarg.0
		IL_0034: ldfld valuetype TMPro.Examples.TMP_FrameRateCounter/FpsCounterAnchorPositions TMPro.Examples.TMP_FrameRateCounter::AnchorPosition
		IL_0039: stloc.0
		IL_003a: ldarg.0
		IL_003b: ldarg.0
		IL_003c: ldfld int32 TMPro.Examples.TMP_FrameRateCounter::m_Frames
		IL_0041: ldc.i4.1
		IL_0042: add
		IL_0043: stfld int32 TMPro.Examples.TMP_FrameRateCounter::m_Frames
		IL_0048: ldarg.0
		IL_0049: ldloc.0
		IL_004a: box [mscorlib]System.Object
		IL_004f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0054: box [mscorlib]System.Int32
		IL_0059: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype TMPro.Examples.TMP_FrameRateCounter/FpsCounterAnchorPositions>(object)
		IL_005e: stfld valuetype TMPro.Examples.TMP_FrameRateCounter/FpsCounterAnchorPositions TMPro.Examples.TMP_FrameRateCounter::last_AnchorPosition
		IL_0063: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_realtimeSinceStartup()
		IL_0068: stloc.1
		IL_0069: ldloc.1
		IL_006a: stloc.2
		IL_006b: ldloc.1
		IL_006c: stloc.3
		IL_006d: ldloc.1
		IL_006e: ldarg.0
		IL_006f: ldfld float32 TMPro.Examples.TMP_FrameRateCounter::m_LastInterval
		IL_0074: ldarg.0
		IL_0075: ldfld float32 TMPro.Examples.TMP_FrameRateCounter::UpdateInterval
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
		IL_00b8: ldfld int32 TMPro.Examples.TMP_FrameRateCounter::m_Frames
		IL_00bd: conv.r4
		IL_00be: ldloc.2
		IL_00bf: ldarg.0
		IL_00c0: ldfld float32 TMPro.Examples.TMP_FrameRateCounter::m_LastInterval
		IL_00c5: sub
		IL_00c6: div
		IL_00c7: stloc.s 7
		IL_00c9: ldc.r4 30
		IL_00ce: ldarg.0
		IL_00cf: ldfld int32 TMPro.Examples.TMP_FrameRateCounter::m_Frames
		IL_00d4: conv.r4
		IL_00d5: ldloc.2
		IL_00d6: ldarg.0
		IL_00d7: ldfld float32 TMPro.Examples.TMP_FrameRateCounter::m_LastInterval
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
		IL_00f5: stfld string TMPro.Examples.TMP_FrameRateCounter::htmlColorTag
		IL_00fa: br.s IL_0116

		IL_00fc: nop
		IL_00fd: ldarg.0
		IL_00fe: ldstr "<color=red>"
		IL_0103: stfld string TMPro.Examples.TMP_FrameRateCounter::htmlColorTag
		IL_0108: br.s IL_0116

		IL_010a: nop
		IL_010b: ldarg.0
		IL_010c: ldstr "<color=yellow>"
		IL_0111: stfld string TMPro.Examples.TMP_FrameRateCounter::htmlColorTag

		IL_0116: nop
		IL_0117: ldarg.0
		IL_0118: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_FrameRateCounter::m_TextMeshPro
		IL_011d: stloc.s 8
		IL_011f: ldarg.0
		IL_0120: ldfld string TMPro.Examples.TMP_FrameRateCounter::htmlColorTag
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
		IL_0144: stfld float32 TMPro.Examples.TMP_FrameRateCounter::m_LastInterval
		IL_0149: ldarg.0
		IL_014a: ldc.i4.0
		IL_014b: stfld int32 TMPro.Examples.TMP_FrameRateCounter::m_Frames

		IL_0150: nop
		IL_0151: ret

		IL_0152: nop
		IL_0153: ldnull
		IL_0154: throw
	} // end of method TMP_FrameRateCounter::Update

	.method private hidebysig 
		instance void Set_FrameCounter_Position (
			valuetype TMPro.Examples.TMP_FrameRateCounter/FpsCounterAnchorPositions anchor_position
		) cil managed 
	{
		// Method begins at RVA 0x17524
		// Header size: 12
		// Code size: 473 (0x1d9)
		.maxstack 3
		.locals init (
			[0] int32,
			[1] int64,
			[2] class [UnityEngine.CoreModule]UnityEngine.RectTransform,
			[3] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[4] class [UnityEngine.CoreModule]UnityEngine.RectTransform,
			[5] class [UnityEngine.CoreModule]UnityEngine.RectTransform,
			[6] class [UnityEngine.CoreModule]UnityEngine.RectTransform,
			[7] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[8] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: box [mscorlib]System.Object
		IL_0007: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_000c: stloc.0
		IL_000d: ldstr "global_13EACA0"
		IL_0012: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0017: stloc.1
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: ceq
		IL_001c: brtrue IL_0133

		IL_0021: nop
		IL_0022: ldloc.0
		IL_0023: ldc.i4.1
		IL_0024: sub
		IL_0025: stloc.0
		IL_0026: ldloc.0
		IL_0027: ldc.i4.1
		IL_0028: sub
		IL_0029: ldc.i4.0
		IL_002a: ceq
		IL_002c: brtrue IL_00e7

		IL_0031: nop
		IL_0032: ldloc.0
		IL_0033: ldc.i4.1
		IL_0034: sub
		IL_0035: stloc.0
		IL_0036: ldloc.0
		IL_0037: ldc.i4.1
		IL_0038: sub
		IL_0039: ldc.i4.0
		IL_003a: ceq
		IL_003c: brtrue.s IL_0098

		IL_003e: nop
		IL_003f: ldloc.0
		IL_0040: ldc.i4.1
		IL_0041: ceq
		IL_0043: ldc.i4.0
		IL_0044: ceq
		IL_0046: brtrue IL_01d4

		IL_004b: nop
		IL_004c: ldarg.0
		IL_004d: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_FrameRateCounter::m_TextMeshPro
		IL_0052: ldc.i4 1028
		IL_0057: box [mscorlib]System.Int32
		IL_005c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions>(object)
		IL_0061: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_alignment(valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions)
		IL_0066: ldarg.0
		IL_0067: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_FrameRateCounter::m_TextMeshPro
		IL_006c: callvirt instance class [UnityEngine.CoreModule]UnityEngine.RectTransform [Unity.TextMeshPro]TMPro.TMP_Text::get_rectTransform()
		IL_0071: stloc.2
		IL_0072: ldloc.2
		IL_0073: ldstr "unpcklps xmm0,xmm1"
		IL_0078: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_007d: box [mscorlib]System.Int64
		IL_0082: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector2>(object)
		IL_0087: callvirt instance void [UnityEngine.CoreModule]UnityEngine.RectTransform::set_pivot(valuetype [UnityEngine.CoreModule]UnityEngine.Vector2)
		IL_008c: ldarg.0
		IL_008d: ldfld class [UnityEngine.CoreModule]UnityEngine.Camera TMPro.Examples.TMP_FrameRateCounter::m_camera
		IL_0092: stloc.3
		IL_0093: br IL_017e

		IL_0098: nop
		IL_0099: ldarg.0
		IL_009a: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_FrameRateCounter::m_TextMeshPro
		IL_009f: ldc.i4 260
		IL_00a4: box [mscorlib]System.Int32
		IL_00a9: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions>(object)
		IL_00ae: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_alignment(valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions)
		IL_00b3: ldarg.0
		IL_00b4: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_FrameRateCounter::m_TextMeshPro
		IL_00b9: callvirt instance class [UnityEngine.CoreModule]UnityEngine.RectTransform [Unity.TextMeshPro]TMPro.TMP_Text::get_rectTransform()
		IL_00be: stloc.s 4
		IL_00c0: ldloc.s 4
		IL_00c2: ldstr "unpcklps xmm0,xmm0"
		IL_00c7: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00cc: box [mscorlib]System.Int64
		IL_00d1: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector2>(object)
		IL_00d6: callvirt instance void [UnityEngine.CoreModule]UnityEngine.RectTransform::set_pivot(valuetype [UnityEngine.CoreModule]UnityEngine.Vector2)
		IL_00db: ldarg.0
		IL_00dc: ldfld class [UnityEngine.CoreModule]UnityEngine.Camera TMPro.Examples.TMP_FrameRateCounter::m_camera
		IL_00e1: stloc.3
		IL_00e2: br IL_017d

		IL_00e7: nop
		IL_00e8: ldarg.0
		IL_00e9: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_FrameRateCounter::m_TextMeshPro
		IL_00ee: ldc.i4 1025
		IL_00f3: box [mscorlib]System.Int32
		IL_00f8: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions>(object)
		IL_00fd: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_alignment(valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions)
		IL_0102: ldarg.0
		IL_0103: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_FrameRateCounter::m_TextMeshPro
		IL_0108: callvirt instance class [UnityEngine.CoreModule]UnityEngine.RectTransform [Unity.TextMeshPro]TMPro.TMP_Text::get_rectTransform()
		IL_010d: stloc.s 5
		IL_010f: ldloc.s 5
		IL_0111: ldstr "unpcklps xmm0,xmm0"
		IL_0116: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_011b: box [mscorlib]System.Int64
		IL_0120: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector2>(object)
		IL_0125: callvirt instance void [UnityEngine.CoreModule]UnityEngine.RectTransform::set_pivot(valuetype [UnityEngine.CoreModule]UnityEngine.Vector2)
		IL_012a: ldarg.0
		IL_012b: ldfld class [UnityEngine.CoreModule]UnityEngine.Camera TMPro.Examples.TMP_FrameRateCounter::m_camera
		IL_0130: stloc.3
		IL_0131: br.s IL_017e

		IL_0133: nop
		IL_0134: ldarg.0
		IL_0135: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_FrameRateCounter::m_TextMeshPro
		IL_013a: ldc.i4 257
		IL_013f: box [mscorlib]System.Int32
		IL_0144: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions>(object)
		IL_0149: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_alignment(valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions)
		IL_014e: ldarg.0
		IL_014f: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_FrameRateCounter::m_TextMeshPro
		IL_0154: callvirt instance class [UnityEngine.CoreModule]UnityEngine.RectTransform [Unity.TextMeshPro]TMPro.TMP_Text::get_rectTransform()
		IL_0159: stloc.s 6
		IL_015b: ldloc.s 6
		IL_015d: ldstr "unpcklps xmm0,xmm1"
		IL_0162: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0167: box [mscorlib]System.Int64
		IL_016c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector2>(object)
		IL_0171: callvirt instance void [UnityEngine.CoreModule]UnityEngine.RectTransform::set_pivot(valuetype [UnityEngine.CoreModule]UnityEngine.Vector2)
		IL_0176: ldarg.0
		IL_0177: ldfld class [UnityEngine.CoreModule]UnityEngine.Camera TMPro.Examples.TMP_FrameRateCounter::m_camera
		IL_017c: stloc.3

		IL_017d: nop

		IL_017e: nop
		IL_017f: ldarg.0
		IL_0180: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TMP_FrameRateCounter::m_frameCounter_transform
		IL_0185: stloc.s 7
		IL_0187: ldloc.3
		IL_0188: ldstr "&stack[0xFFFFFFFFFFFFFFF8]"
		IL_018d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0192: conv.i8
		IL_0193: ldc.i8 -32
		IL_019c: add
		IL_019d: box [mscorlib]System.Int64
		IL_01a2: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_01a7: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Camera::ViewportToWorldPoint(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_01ac: stloc.s 8
		IL_01ae: ldloc.s 7
		IL_01b0: ldstr "&stack[0xFFFFFFFFFFFFFFF8]"
		IL_01b5: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01ba: conv.i8
		IL_01bb: ldc.i8 -32
		IL_01c4: add
		IL_01c5: box [mscorlib]System.Int64
		IL_01ca: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_01cf: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)

		IL_01d4: nop
		IL_01d5: ret

		IL_01d6: nop
		IL_01d7: ldnull
		IL_01d8: throw
	} // end of method TMP_FrameRateCounter::Set_FrameCounter_Position

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x298b
		// Header size: 1
		// Code size: 36 (0x24)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.r4 5
		IL_0007: stfld float32 TMPro.Examples.TMP_FrameRateCounter::UpdateInterval
		IL_000c: ldarg.0
		IL_000d: ldc.i4.2
		IL_000e: box [mscorlib]System.Int32
		IL_0013: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype TMPro.Examples.TMP_FrameRateCounter/FpsCounterAnchorPositions>(object)
		IL_0018: stfld valuetype TMPro.Examples.TMP_FrameRateCounter/FpsCounterAnchorPositions TMPro.Examples.TMP_FrameRateCounter::AnchorPosition
		IL_001d: ldarg.0
		IL_001e: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0023: ret
	} // end of method TMP_FrameRateCounter::.ctor

} // end of class TMPro.Examples.TMP_FrameRateCounter
