.class public auto ansi beforefieldinit TMPro.Examples.TMPro_InstructionOverlay
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested public auto ansi sealed FpsCounterAnchorPositions
		extends [mscorlib]System.Enum
	{
		// Fields
		.field public specialname rtspecialname int32 value__
		.field public static literal valuetype TMPro.Examples.TMPro_InstructionOverlay/FpsCounterAnchorPositions TopLeft
		.field public static literal valuetype TMPro.Examples.TMPro_InstructionOverlay/FpsCounterAnchorPositions BottomLeft
		.field public static literal valuetype TMPro.Examples.TMPro_InstructionOverlay/FpsCounterAnchorPositions TopRight
		.field public static literal valuetype TMPro.Examples.TMPro_InstructionOverlay/FpsCounterAnchorPositions BottomRight

	} // end of class FpsCounterAnchorPositions


	// Fields
	.field public valuetype TMPro.Examples.TMPro_InstructionOverlay/FpsCounterAnchorPositions AnchorPosition
	.field private static literal string instructions
	.field private class [Unity.TextMeshPro]TMPro.TextMeshPro m_TextMeshPro
	.field private class [Unity.TextMeshPro]TMPro.TextContainer m_textContainer
	.field private class [UnityEngine.CoreModule]UnityEngine.Transform m_frameCounter_transform
	.field private class [UnityEngine.CoreModule]UnityEngine.Camera m_camera

	// Methods
	.method private hidebysig 
		instance void Awake () cil managed 
	{
		// Method begins at RVA 0x16e94
		// Header size: 12
		// Code size: 263 (0x107)
		.maxstack 3
		.locals init (
			[0] bool,
			[1] bool,
			[2] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[3] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[4] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[5] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[6] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[7] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[8] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[9] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[10] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[11] class [Unity.TextMeshPro]TMPro.TMP_FontAsset,
			[12] class [Unity.TextMeshPro]TMPro.TMP_FontAsset,
			[13] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[14] class [UnityEngine.CoreModule]UnityEngine.Material,
			[15] class [UnityEngine.CoreModule]UnityEngine.Material,
			[16] class [Unity.TextMeshPro]TMPro.TextContainer
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
		IL_000e: brtrue IL_0102

		IL_0013: nop
		IL_0014: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_0019: stloc.2
		IL_001a: ldarg.0
		IL_001b: ldloc.2
		IL_001c: stfld class [UnityEngine.CoreModule]UnityEngine.Camera TMPro.Examples.TMPro_InstructionOverlay::m_camera
		IL_0021: ldstr "Frame Counter"
		IL_0026: newobj instance void [UnityEngine.CoreModule]UnityEngine.GameObject::.ctor(string)
		IL_002b: stloc.3
		IL_002c: ldloc.3
		IL_002d: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
		IL_0032: stloc.s 4
		IL_0034: ldarg.0
		IL_0035: ldloc.s 4
		IL_0037: stfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TMPro_InstructionOverlay::m_frameCounter_transform
		IL_003c: ldarg.0
		IL_003d: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TMPro_InstructionOverlay::m_frameCounter_transform
		IL_0042: stloc.s 5
		IL_0044: ldarg.0
		IL_0045: ldfld class [UnityEngine.CoreModule]UnityEngine.Camera TMPro.Examples.TMPro_InstructionOverlay::m_camera
		IL_004a: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_004f: stloc.s 6
		IL_0051: ldloc.s 6
		IL_0053: stloc.s 7
		IL_0055: ldloc.s 5
		IL_0057: ldloc.s 7
		IL_0059: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_parent(class [UnityEngine.CoreModule]UnityEngine.Transform)
		IL_005e: ldarg.0
		IL_005f: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TMPro_InstructionOverlay::m_frameCounter_transform
		IL_0064: ldloca.s 8
		IL_0066: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Quaternion::identityQuaternion
		IL_006b: box [mscorlib]System.Object
		IL_0070: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0075: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
		IL_007a: ldloc.s 8
		IL_007c: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_localRotation(valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion)
		IL_0081: ldloc.3
		IL_0082: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class [Unity.TextMeshPro]TMPro.TextMeshPro>()
		IL_0087: stloc.s 9
		IL_0089: ldarg.0
		IL_008a: ldloc.s 9
		IL_008c: stfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMPro_InstructionOverlay::m_TextMeshPro
		IL_0091: ldarg.0
		IL_0092: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMPro_InstructionOverlay::m_TextMeshPro
		IL_0097: stloc.s 10
		IL_0099: ldstr "Fonts & Materials/LiberationSans SDF"
		IL_009e: call !!0 [UnityEngine.CoreModule]UnityEngine.Resources::Load<class [Unity.TextMeshPro]TMPro.TMP_FontAsset>(string)
		IL_00a3: stloc.s 11
		IL_00a5: ldloc.s 11
		IL_00a7: stloc.s 12
		IL_00a9: ldloc.s 10
		IL_00ab: ldloc.s 12
		IL_00ad: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_font(class [Unity.TextMeshPro]TMPro.TMP_FontAsset)
		IL_00b2: ldarg.0
		IL_00b3: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMPro_InstructionOverlay::m_TextMeshPro
		IL_00b8: stloc.s 13
		IL_00ba: ldstr "Fonts & Materials/LiberationSans SDF - Overlay"
		IL_00bf: call !!0 [UnityEngine.CoreModule]UnityEngine.Resources::Load<class [UnityEngine.CoreModule]UnityEngine.Material>(string)
		IL_00c4: stloc.s 14
		IL_00c6: ldloc.s 14
		IL_00c8: stloc.s 15
		IL_00ca: ldarg.0
		IL_00cb: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMPro_InstructionOverlay::m_TextMeshPro
		IL_00d0: ldc.r4 30
		IL_00d5: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_fontSize(float32)
		IL_00da: ldarg.0
		IL_00db: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMPro_InstructionOverlay::m_TextMeshPro
		IL_00e0: ldc.i4.1
		IL_00e1: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_isOverlay(bool)
		IL_00e6: ldloc.3
		IL_00e7: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::GetComponent<class [Unity.TextMeshPro]TMPro.TextContainer>()
		IL_00ec: stloc.s 16
		IL_00ee: ldarg.0
		IL_00ef: ldloc.s 16
		IL_00f1: stfld class [Unity.TextMeshPro]TMPro.TextContainer TMPro.Examples.TMPro_InstructionOverlay::m_textContainer
		IL_00f6: ldarg.0
		IL_00f7: ldarg.0
		IL_00f8: ldfld valuetype TMPro.Examples.TMPro_InstructionOverlay/FpsCounterAnchorPositions TMPro.Examples.TMPro_InstructionOverlay::AnchorPosition
		IL_00fd: callvirt instance void TMPro.Examples.TMPro_InstructionOverlay::Set_FrameCounter_Position(valuetype TMPro.Examples.TMPro_InstructionOverlay/FpsCounterAnchorPositions)

		IL_0102: nop
		IL_0103: ret

		IL_0104: nop
		IL_0105: ldnull
		IL_0106: throw
	} // end of method TMPro_InstructionOverlay::Awake

	.method private hidebysig 
		instance void Set_FrameCounter_Position (
			valuetype TMPro.Examples.TMPro_InstructionOverlay/FpsCounterAnchorPositions anchor_position
		) cil managed 
	{
		// Method begins at RVA 0x16fa8
		// Header size: 12
		// Code size: 306 (0x132)
		.maxstack 3
		.locals init (
			[0] int32,
			[1] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[2] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3
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
		IL_001b: brtrue IL_00bb

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
		IL_003f: brtrue.s IL_009b

		IL_0041: nop
		IL_0042: ldloc.0
		IL_0043: ldc.i4.1
		IL_0044: sub
		IL_0045: stloc.0
		IL_0046: ldloc.0
		IL_0047: ldc.i4.1
		IL_0048: sub
		IL_0049: ldc.i4.0
		IL_004a: ceq
		IL_004c: brtrue.s IL_007b

		IL_004e: nop
		IL_004f: ldloc.0
		IL_0050: ldc.i4.1
		IL_0051: ceq
		IL_0053: ldc.i4.0
		IL_0054: ceq
		IL_0056: brtrue IL_012d

		IL_005b: nop
		IL_005c: ldarg.0
		IL_005d: ldfld class [Unity.TextMeshPro]TMPro.TextContainer TMPro.Examples.TMPro_InstructionOverlay::m_textContainer
		IL_0062: ldc.i4.8
		IL_0063: box [mscorlib]System.Int32
		IL_0068: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [Unity.TextMeshPro]TMPro.TextContainerAnchors>(object)
		IL_006d: callvirt instance void [Unity.TextMeshPro]TMPro.TextContainer::set_anchorPosition(valuetype [Unity.TextMeshPro]TMPro.TextContainerAnchors)
		IL_0072: ldarg.0
		IL_0073: ldfld class [UnityEngine.CoreModule]UnityEngine.Camera TMPro.Examples.TMPro_InstructionOverlay::m_camera
		IL_0078: stloc.1
		IL_0079: br.s IL_00da

		IL_007b: nop
		IL_007c: ldarg.0
		IL_007d: ldfld class [Unity.TextMeshPro]TMPro.TextContainer TMPro.Examples.TMPro_InstructionOverlay::m_textContainer
		IL_0082: ldc.i4.2
		IL_0083: box [mscorlib]System.Int32
		IL_0088: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [Unity.TextMeshPro]TMPro.TextContainerAnchors>(object)
		IL_008d: callvirt instance void [Unity.TextMeshPro]TMPro.TextContainer::set_anchorPosition(valuetype [Unity.TextMeshPro]TMPro.TextContainerAnchors)
		IL_0092: ldarg.0
		IL_0093: ldfld class [UnityEngine.CoreModule]UnityEngine.Camera TMPro.Examples.TMPro_InstructionOverlay::m_camera
		IL_0098: stloc.1
		IL_0099: br.s IL_00d9

		IL_009b: nop
		IL_009c: ldarg.0
		IL_009d: ldfld class [Unity.TextMeshPro]TMPro.TextContainer TMPro.Examples.TMPro_InstructionOverlay::m_textContainer
		IL_00a2: ldc.i4.6
		IL_00a3: box [mscorlib]System.Int32
		IL_00a8: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [Unity.TextMeshPro]TMPro.TextContainerAnchors>(object)
		IL_00ad: callvirt instance void [Unity.TextMeshPro]TMPro.TextContainer::set_anchorPosition(valuetype [Unity.TextMeshPro]TMPro.TextContainerAnchors)
		IL_00b2: ldarg.0
		IL_00b3: ldfld class [UnityEngine.CoreModule]UnityEngine.Camera TMPro.Examples.TMPro_InstructionOverlay::m_camera
		IL_00b8: stloc.1
		IL_00b9: br.s IL_00da

		IL_00bb: nop
		IL_00bc: ldarg.0
		IL_00bd: ldfld class [Unity.TextMeshPro]TMPro.TextContainer TMPro.Examples.TMPro_InstructionOverlay::m_textContainer
		IL_00c2: ldc.i4.0
		IL_00c3: box [mscorlib]System.Int32
		IL_00c8: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [Unity.TextMeshPro]TMPro.TextContainerAnchors>(object)
		IL_00cd: callvirt instance void [Unity.TextMeshPro]TMPro.TextContainer::set_anchorPosition(valuetype [Unity.TextMeshPro]TMPro.TextContainerAnchors)
		IL_00d2: ldarg.0
		IL_00d3: ldfld class [UnityEngine.CoreModule]UnityEngine.Camera TMPro.Examples.TMPro_InstructionOverlay::m_camera
		IL_00d8: stloc.1

		IL_00d9: nop

		IL_00da: nop
		IL_00db: ldarg.0
		IL_00dc: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.TMPro_InstructionOverlay::m_frameCounter_transform
		IL_00e1: stloc.2
		IL_00e2: ldloc.1
		IL_00e3: ldstr "&stack[0xFFFFFFFFFFFFFFF8]"
		IL_00e8: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00ed: conv.i8
		IL_00ee: ldc.i8 -32
		IL_00f7: add
		IL_00f8: box [mscorlib]System.Int64
		IL_00fd: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0102: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Camera::ViewportToWorldPoint(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0107: stloc.3
		IL_0108: ldloc.2
		IL_0109: ldstr "&stack[0xFFFFFFFFFFFFFFF8]"
		IL_010e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0113: conv.i8
		IL_0114: ldc.i8 -32
		IL_011d: add
		IL_011e: box [mscorlib]System.Int64
		IL_0123: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0128: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)

		IL_012d: nop
		IL_012e: ret

		IL_012f: nop
		IL_0130: ldnull
		IL_0131: throw
	} // end of method TMPro_InstructionOverlay::Set_FrameCounter_Position

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2968
		// Header size: 1
		// Code size: 25 (0x19)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i4.1
		IL_0003: box [mscorlib]System.Int32
		IL_0008: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype TMPro.Examples.TMPro_InstructionOverlay/FpsCounterAnchorPositions>(object)
		IL_000d: stfld valuetype TMPro.Examples.TMPro_InstructionOverlay/FpsCounterAnchorPositions TMPro.Examples.TMPro_InstructionOverlay::AnchorPosition
		IL_0012: ldarg.0
		IL_0013: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0018: ret
	} // end of method TMPro_InstructionOverlay::.ctor

} // end of class TMPro.Examples.TMPro_InstructionOverlay
