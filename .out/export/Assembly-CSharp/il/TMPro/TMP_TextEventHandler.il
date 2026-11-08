.class public auto ansi beforefieldinit TMPro.TMP_TextEventHandler
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
	implements [UnityEngine.UI]UnityEngine.EventSystems.IPointerEnterHandler,
	           [UnityEngine.UI]UnityEngine.EventSystems.IEventSystemHandler,
	           [UnityEngine.UI]UnityEngine.EventSystems.IPointerExitHandler
{
	// Nested Types
	.class nested public auto ansi serializable beforefieldinit CharacterSelectionEvent
		extends class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`2<char, int32>
	{
		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor () cil managed 
		{
			// Method begins at RVA 0x2d35
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`1<bool>::.ctor()
			IL_0007: ret
		} // end of method CharacterSelectionEvent::.ctor

	} // end of class CharacterSelectionEvent

	.class nested public auto ansi serializable beforefieldinit SpriteSelectionEvent
		extends class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`2<char, int32>
	{
		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor () cil managed 
		{
			// Method begins at RVA 0x2d3e
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`1<bool>::.ctor()
			IL_0007: ret
		} // end of method SpriteSelectionEvent::.ctor

	} // end of class SpriteSelectionEvent

	.class nested public auto ansi serializable beforefieldinit WordSelectionEvent
		extends class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`3<string, int32, int32>
	{
		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor () cil managed 
		{
			// Method begins at RVA 0x2d47
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`1<bool>::.ctor()
			IL_0007: ret
		} // end of method WordSelectionEvent::.ctor

	} // end of class WordSelectionEvent

	.class nested public auto ansi serializable beforefieldinit LineSelectionEvent
		extends class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`3<string, int32, int32>
	{
		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor () cil managed 
		{
			// Method begins at RVA 0x2d50
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`1<bool>::.ctor()
			IL_0007: ret
		} // end of method LineSelectionEvent::.ctor

	} // end of class LineSelectionEvent

	.class nested public auto ansi serializable beforefieldinit LinkSelectionEvent
		extends class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`3<string, string, int32>
	{
		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor () cil managed 
		{
			// Method begins at RVA 0x2d59
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`1<bool>::.ctor()
			IL_0007: ret
		} // end of method LinkSelectionEvent::.ctor

	} // end of class LinkSelectionEvent


	// Fields
	.field private class TMPro.TMP_TextEventHandler/CharacterSelectionEvent m_OnCharacterSelection
	.field private class TMPro.TMP_TextEventHandler/SpriteSelectionEvent m_OnSpriteSelection
	.field private class TMPro.TMP_TextEventHandler/WordSelectionEvent m_OnWordSelection
	.field private class TMPro.TMP_TextEventHandler/LineSelectionEvent m_OnLineSelection
	.field private class TMPro.TMP_TextEventHandler/LinkSelectionEvent m_OnLinkSelection
	.field private class [Unity.TextMeshPro]TMPro.TMP_Text m_TextComponent
	.field private class [UnityEngine.CoreModule]UnityEngine.Camera m_Camera
	.field private class [UnityEngine.UIModule]UnityEngine.Canvas m_Canvas
	.field private int32 m_selectedLink
	.field private int32 m_lastCharIndex
	.field private int32 m_lastWordIndex
	.field private int32 m_lastLineIndex

	// Methods
	.method public hidebysig specialname 
		instance class TMPro.TMP_TextEventHandler/CharacterSelectionEvent get_onCharacterSelection () cil managed 
	{
		// Method begins at RVA 0x27de
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class TMPro.TMP_TextEventHandler/CharacterSelectionEvent TMPro.TMP_TextEventHandler::m_OnCharacterSelection
		IL_0007: ret
	} // end of method TMP_TextEventHandler::get_onCharacterSelection

	.method public hidebysig specialname 
		instance void set_onCharacterSelection (
			class TMPro.TMP_TextEventHandler/CharacterSelectionEvent 'value'
		) cil managed 
	{
		// Method begins at RVA 0x27e7
		// Header size: 1
		// Code size: 9 (0x9)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldarg.1
		IL_0003: stfld class TMPro.TMP_TextEventHandler/CharacterSelectionEvent TMPro.TMP_TextEventHandler::m_OnCharacterSelection
		IL_0008: ret
	} // end of method TMP_TextEventHandler::set_onCharacterSelection

	.method public hidebysig specialname 
		instance class TMPro.TMP_TextEventHandler/SpriteSelectionEvent get_onSpriteSelection () cil managed 
	{
		// Method begins at RVA 0x27f1
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class TMPro.TMP_TextEventHandler/SpriteSelectionEvent TMPro.TMP_TextEventHandler::m_OnSpriteSelection
		IL_0007: ret
	} // end of method TMP_TextEventHandler::get_onSpriteSelection

	.method public hidebysig specialname 
		instance void set_onSpriteSelection (
			class TMPro.TMP_TextEventHandler/SpriteSelectionEvent 'value'
		) cil managed 
	{
		// Method begins at RVA 0x27fa
		// Header size: 1
		// Code size: 9 (0x9)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldarg.1
		IL_0003: stfld class TMPro.TMP_TextEventHandler/SpriteSelectionEvent TMPro.TMP_TextEventHandler::m_OnSpriteSelection
		IL_0008: ret
	} // end of method TMP_TextEventHandler::set_onSpriteSelection

	.method public hidebysig specialname 
		instance class TMPro.TMP_TextEventHandler/WordSelectionEvent get_onWordSelection () cil managed 
	{
		// Method begins at RVA 0x2804
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class TMPro.TMP_TextEventHandler/WordSelectionEvent TMPro.TMP_TextEventHandler::m_OnWordSelection
		IL_0007: ret
	} // end of method TMP_TextEventHandler::get_onWordSelection

	.method public hidebysig specialname 
		instance void set_onWordSelection (
			class TMPro.TMP_TextEventHandler/WordSelectionEvent 'value'
		) cil managed 
	{
		// Method begins at RVA 0x280d
		// Header size: 1
		// Code size: 9 (0x9)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldarg.1
		IL_0003: stfld class TMPro.TMP_TextEventHandler/WordSelectionEvent TMPro.TMP_TextEventHandler::m_OnWordSelection
		IL_0008: ret
	} // end of method TMP_TextEventHandler::set_onWordSelection

	.method public hidebysig specialname 
		instance class TMPro.TMP_TextEventHandler/LineSelectionEvent get_onLineSelection () cil managed 
	{
		// Method begins at RVA 0x2817
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class TMPro.TMP_TextEventHandler/LineSelectionEvent TMPro.TMP_TextEventHandler::m_OnLineSelection
		IL_0007: ret
	} // end of method TMP_TextEventHandler::get_onLineSelection

	.method public hidebysig specialname 
		instance void set_onLineSelection (
			class TMPro.TMP_TextEventHandler/LineSelectionEvent 'value'
		) cil managed 
	{
		// Method begins at RVA 0x2820
		// Header size: 1
		// Code size: 9 (0x9)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldarg.1
		IL_0003: stfld class TMPro.TMP_TextEventHandler/LineSelectionEvent TMPro.TMP_TextEventHandler::m_OnLineSelection
		IL_0008: ret
	} // end of method TMP_TextEventHandler::set_onLineSelection

	.method public hidebysig specialname 
		instance class TMPro.TMP_TextEventHandler/LinkSelectionEvent get_onLinkSelection () cil managed 
	{
		// Method begins at RVA 0x282a
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class TMPro.TMP_TextEventHandler/LinkSelectionEvent TMPro.TMP_TextEventHandler::m_OnLinkSelection
		IL_0007: ret
	} // end of method TMP_TextEventHandler::get_onLinkSelection

	.method public hidebysig specialname 
		instance void set_onLinkSelection (
			class TMPro.TMP_TextEventHandler/LinkSelectionEvent 'value'
		) cil managed 
	{
		// Method begins at RVA 0x2833
		// Header size: 1
		// Code size: 9 (0x9)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldarg.1
		IL_0003: stfld class TMPro.TMP_TextEventHandler/LinkSelectionEvent TMPro.TMP_TextEventHandler::m_OnLinkSelection
		IL_0008: ret
	} // end of method TMP_TextEventHandler::set_onLinkSelection

	.method private hidebysig 
		instance void Awake () cil managed 
	{
		// Method begins at RVA 0x142bc
		// Header size: 12
		// Code size: 262 (0x106)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[1] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[2] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[3] class [mscorlib]System.Type,
			[4] class [mscorlib]System.Type,
			[5] class [mscorlib]System.Type,
			[6] bool,
			[7] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[8] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[9] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[10] class [UnityEngine.UIModule]UnityEngine.Canvas,
			[11] class [UnityEngine.UIModule]UnityEngine.Canvas,
			[12] bool,
			[13] bool,
			[14] valuetype [UnityEngine.UIModule]UnityEngine.RenderMode,
			[15] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[16] class [UnityEngine.CoreModule]UnityEngine.Camera
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::GetComponent<class [Unity.TextMeshPro]TMPro.TMP_Text>()
		IL_000e: stloc.1
		IL_000f: ldarg.0
		IL_0010: ldloc.1
		IL_0011: stfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.TMP_TextEventHandler::m_TextComponent
		IL_0016: ldarg.0
		IL_0017: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.TMP_TextEventHandler::m_TextComponent
		IL_001c: stloc.2
		IL_001d: ldloc.2
		IL_001e: callvirt instance class [mscorlib]System.Type [mscorlib]System.Object::GetType()
		IL_0023: stloc.3
		IL_0024: ldloc.3
		IL_0025: stloc.s 4
		IL_0027: br.s IL_002a

		IL_0029: nop

		IL_002a: nop
		IL_002b: ldstr "typeof(TextMeshProUGUI)"
		IL_0030: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0035: box [mscorlib]System.IntPtr
		IL_003a: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [mscorlib]System.RuntimeTypeHandle>(object)
		IL_003f: call class [mscorlib]System.Type [mscorlib]System.Type::GetTypeFromHandle(valuetype [mscorlib]System.RuntimeTypeHandle)
		IL_0044: stloc.s 5
		IL_0046: ldloc.s 4
		IL_0048: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [mscorlib]System.TimeSpan>(object)
		IL_004d: ldloc.s 5
		IL_004f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [mscorlib]System.TimeSpan>(object)
		IL_0054: call bool [mscorlib]System.TimeSpan::op_Equality(valuetype [mscorlib]System.TimeSpan, valuetype [mscorlib]System.TimeSpan)
		IL_0059: stloc.s 6
		IL_005b: ldloc.s 6
		IL_005d: ldc.i4.0
		IL_005e: ceq
		IL_0060: ldc.i4.0
		IL_0061: ceq
		IL_0063: brtrue.s IL_007c

		IL_0065: nop
		IL_0066: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_006b: stloc.s 7
		IL_006d: ldloc.s 7
		IL_006f: stloc.s 8

		IL_0071: nop
		IL_0072: ldarg.0
		IL_0073: ldloc.s 8
		IL_0075: stfld class [UnityEngine.CoreModule]UnityEngine.Camera TMPro.TMP_TextEventHandler::m_Camera

		IL_007a: nop
		IL_007b: ret

		IL_007c: nop
		IL_007d: ldarg.0
		IL_007e: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_0083: stloc.s 9
		IL_0085: ldloc.s 9
		IL_0087: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::GetComponentInParent<class [UnityEngine.UIModule]UnityEngine.Canvas>()
		IL_008c: stloc.s 10
		IL_008e: ldarg.0
		IL_008f: ldloc.s 10
		IL_0091: stfld class [UnityEngine.UIModule]UnityEngine.Canvas TMPro.TMP_TextEventHandler::m_Canvas
		IL_0096: ldarg.0
		IL_0097: ldfld class [UnityEngine.UIModule]UnityEngine.Canvas TMPro.TMP_TextEventHandler::m_Canvas
		IL_009c: stloc.s 11
		IL_009e: br.s IL_00a1

		IL_00a0: nop

		IL_00a1: nop
		IL_00a2: ldloc.s 11
		IL_00a4: ldnull
		IL_00a5: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Inequality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_00aa: stloc.s 12
		IL_00ac: ldloc.s 12
		IL_00ae: stloc.s 13
		IL_00b0: ldloc.s 12
		IL_00b2: ldc.i4.0
		IL_00b3: ceq
		IL_00b5: brtrue.s IL_007a

		IL_00b7: nop
		IL_00b8: ldarg.0
		IL_00b9: ldfld class [UnityEngine.UIModule]UnityEngine.Canvas TMPro.TMP_TextEventHandler::m_Canvas
		IL_00be: callvirt instance valuetype [UnityEngine.UIModule]UnityEngine.RenderMode [UnityEngine.UIModule]UnityEngine.Canvas::get_renderMode()
		IL_00c3: stloc.s 14
		IL_00c5: ldloc.s 14
		IL_00c7: box [mscorlib]System.Object
		IL_00cc: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_00d1: ldloc.s 14
		IL_00d3: box [mscorlib]System.Object
		IL_00d8: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_00dd: and
		IL_00de: ldc.i4.0
		IL_00df: ceq
		IL_00e1: brtrue.s IL_00fa

		IL_00e3: nop
		IL_00e4: ldarg.0
		IL_00e5: ldfld class [UnityEngine.UIModule]UnityEngine.Canvas TMPro.TMP_TextEventHandler::m_Canvas
		IL_00ea: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.UIModule]UnityEngine.Canvas::get_worldCamera()
		IL_00ef: stloc.s 15
		IL_00f1: ldloc.s 15
		IL_00f3: stloc.s 16
		IL_00f5: br IL_0071

		IL_00fa: nop
		IL_00fb: ldarg.0
		IL_00fc: ldnull
		IL_00fd: stfld class [UnityEngine.CoreModule]UnityEngine.Camera TMPro.TMP_TextEventHandler::m_Camera
		IL_0102: ret

		IL_0103: nop
		IL_0104: ldnull
		IL_0105: throw
	} // end of method TMP_TextEventHandler::Awake

	.method private hidebysig 
		instance void LateUpdate () cil managed 
	{
		// Method begins at RVA 0x143d0
		// Header size: 12
		// Code size: 1308 (0x51c)
		.maxstack 4
		.locals init (
			[0] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[1] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[2] class [UnityEngine.CoreModule]UnityEngine.RectTransform,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[4] int64,
			[5] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[6] float32,
			[7] class [UnityEngine.CoreModule]UnityEngine.RectTransform,
			[8] bool,
			[9] bool,
			[10] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[11] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[12] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[13] int32,
			[14] int64,
			[15] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[16] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[17] int32,
			[18] int32,
			[19] class TMPro.TMP_TextEventHandler/SpriteSelectionEvent,
			[20] class TMPro.TMP_TextEventHandler/CharacterSelectionEvent,
			[21] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[22] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[23] float32,
			[24] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[25] int32,
			[26] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[27] valuetype [Unity.TextMeshPro]TMPro.TMP_WordInfo[],
			[28] string,
			[29] int64,
			[30] int32,
			[31] string,
			[32] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[33] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[34] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[35] float32,
			[36] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[37] int32,
			[38] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[39] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
			[40] valuetype [Unity.TextMeshPro]TMPro.TMP_LineInfo[],
			[41] char[],
			[42] char[],
			[43] int64,
			[44] int32,
			[45] string,
			[46] string,
			[47] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[48] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[49] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[50] int32,
			[51] int64,
			[52] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[53] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[54] valuetype [Unity.TextMeshPro]TMPro.TMP_LinkInfo[],
			[55] string,
			[56] string,
			[57] string,
			[58] string,
			[59] string
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.TMP_TextEventHandler::m_TextComponent
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: stloc.1
		IL_000a: ldloc.1
		IL_000b: callvirt instance class [UnityEngine.CoreModule]UnityEngine.RectTransform [Unity.TextMeshPro]TMPro.TMP_Text::get_rectTransform()
		IL_0010: stloc.2
		IL_0011: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.InputLegacyModule]UnityEngine.Input::get_mousePosition()
		IL_0016: stloc.3
		IL_0017: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_001c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0021: stloc.s 4
		IL_0023: ldarg.0
		IL_0024: ldfld class [UnityEngine.CoreModule]UnityEngine.Camera TMPro.TMP_TextEventHandler::m_Camera
		IL_0029: stloc.s 5
		IL_002b: ldloca.s 3
		IL_002d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0032: stloc.s 6
		IL_0034: ldloc.2
		IL_0035: stloc.s 7
		IL_0037: br.s IL_003a

		IL_0039: nop

		IL_003a: nop
		IL_003b: ldloc.s 7
		IL_003d: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0042: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0047: conv.i8
		IL_0048: ldc.i8 -121
		IL_0051: add
		IL_0052: box [mscorlib]System.Int64
		IL_0057: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_005c: ldloc.s 5
		IL_005e: call bool [Unity.TextMeshPro]TMPro.TMP_TextUtilities::IsIntersectingRectTransform(class [UnityEngine.CoreModule]UnityEngine.RectTransform, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, class [UnityEngine.CoreModule]UnityEngine.Camera)
		IL_0063: stloc.s 8
		IL_0065: ldloc.s 8
		IL_0067: stloc.s 9
		IL_0069: ldloc.s 8
		IL_006b: ldc.i4.0
		IL_006c: ceq
		IL_006e: brtrue IL_0514

		IL_0073: nop
		IL_0074: ldarg.0
		IL_0075: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.TMP_TextEventHandler::m_TextComponent
		IL_007a: stloc.s 10
		IL_007c: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.InputLegacyModule]UnityEngine.Input::get_mousePosition()
		IL_0081: stloc.s 11
		IL_0083: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_0088: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_008d: stloc.s 4
		IL_008f: ldarg.0
		IL_0090: ldfld class [UnityEngine.CoreModule]UnityEngine.Camera TMPro.TMP_TextEventHandler::m_Camera
		IL_0095: stloc.s 5
		IL_0097: ldloca.s 11
		IL_0099: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_009e: stloc.s 6
		IL_00a0: ldloc.s 10
		IL_00a2: stloc.s 12
		IL_00a4: br.s IL_00a7

		IL_00a6: nop

		IL_00a7: nop
		IL_00a8: ldloc.s 12
		IL_00aa: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_00af: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00b4: conv.i8
		IL_00b5: ldc.i8 -121
		IL_00be: add
		IL_00bf: box [mscorlib]System.Int64
		IL_00c4: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_00c9: ldloc.s 5
		IL_00cb: ldc.i4.1
		IL_00cc: call int32 [Unity.TextMeshPro]TMPro.TMP_TextUtilities::FindIntersectingCharacter(class [Unity.TextMeshPro]TMPro.TMP_Text, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, class [UnityEngine.CoreModule]UnityEngine.Camera, bool)
		IL_00d1: stloc.s 13
		IL_00d3: ldloc.s 13
		IL_00d5: conv.i8
		IL_00d6: stloc.s 14
		IL_00d8: ldloc.s 13
		IL_00da: conv.i8
		IL_00db: conv.i4
		IL_00dc: ldc.i4.m1
		IL_00dd: ceq
		IL_00df: brtrue IL_0186

		IL_00e4: nop
		IL_00e5: ldloc.s 14
		IL_00e7: conv.i4
		IL_00e8: ldarg.0
		IL_00e9: ldfld int32 TMPro.TMP_TextEventHandler::m_lastCharIndex
		IL_00ee: ceq
		IL_00f0: brtrue IL_0186

		IL_00f5: nop
		IL_00f6: ldarg.0
		IL_00f7: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.TMP_TextEventHandler::m_TextComponent
		IL_00fc: stloc.s 15
		IL_00fe: ldarg.0
		IL_00ff: ldloc.s 14
		IL_0101: conv.i4
		IL_0102: stfld int32 TMPro.TMP_TextEventHandler::m_lastCharIndex
		IL_0107: ldloc.s 15
		IL_0109: stloc.s 16
		IL_010b: ldstr "*(BinaryExpr + 0x2C)"
		IL_0110: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0115: conv.i4
		IL_0116: stloc.s 17
		IL_0118: ldstr "*(BinaryExpr + 0x2C)"
		IL_011d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0122: conv.i4
		IL_0123: ldc.i4.0
		IL_0124: ceq
		IL_0126: brtrue.s IL_0157

		IL_0128: nop
		IL_0129: ldloc.s 17
		IL_012b: ldc.i4.1
		IL_012c: ceq
		IL_012e: ldc.i4.0
		IL_012f: ceq
		IL_0131: brtrue.s IL_0186

		IL_0133: nop
		IL_0134: ldstr "*(BinaryExpr + 0x20)"
		IL_0139: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_013e: conv.i4
		IL_013f: stloc.s 18
		IL_0141: ldarg.0
		IL_0142: ldfld class TMPro.TMP_TextEventHandler/SpriteSelectionEvent TMPro.TMP_TextEventHandler::m_OnSpriteSelection
		IL_0147: ldnull
		IL_0148: ceq
		IL_014a: brtrue.s IL_0186

		IL_014c: nop
		IL_014d: ldarg.0
		IL_014e: ldfld class TMPro.TMP_TextEventHandler/SpriteSelectionEvent TMPro.TMP_TextEventHandler::m_OnSpriteSelection
		IL_0153: stloc.s 19
		IL_0155: br.s IL_0179

		IL_0157: nop
		IL_0158: ldstr "*(BinaryExpr + 0x20)"
		IL_015d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0162: conv.i4
		IL_0163: stloc.s 18
		IL_0165: ldarg.0
		IL_0166: ldfld class TMPro.TMP_TextEventHandler/CharacterSelectionEvent TMPro.TMP_TextEventHandler::m_OnCharacterSelection
		IL_016b: ldnull
		IL_016c: ceq
		IL_016e: brtrue.s IL_0186

		IL_0170: nop
		IL_0171: ldarg.0
		IL_0172: ldfld class TMPro.TMP_TextEventHandler/CharacterSelectionEvent TMPro.TMP_TextEventHandler::m_OnCharacterSelection
		IL_0177: stloc.s 20

		IL_0179: nop
		IL_017a: ldloc.s 20
		IL_017c: ldloc.s 18
		IL_017e: ldloc.s 14
		IL_0180: conv.i4
		IL_0181: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`2<char, int32>::Invoke(!0, !1)

		IL_0186: nop
		IL_0187: ldarg.0
		IL_0188: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.TMP_TextEventHandler::m_TextComponent
		IL_018d: stloc.s 21
		IL_018f: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.InputLegacyModule]UnityEngine.Input::get_mousePosition()
		IL_0194: stloc.s 22
		IL_0196: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_019b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01a0: stloc.s 4
		IL_01a2: ldarg.0
		IL_01a3: ldfld class [UnityEngine.CoreModule]UnityEngine.Camera TMPro.TMP_TextEventHandler::m_Camera
		IL_01a8: stloc.s 5
		IL_01aa: ldloca.s 22
		IL_01ac: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_01b1: stloc.s 23
		IL_01b3: ldloc.s 21
		IL_01b5: stloc.s 24
		IL_01b7: br.s IL_01ba

		IL_01b9: nop

		IL_01ba: nop
		IL_01bb: ldloc.s 24
		IL_01bd: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_01c2: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01c7: conv.i8
		IL_01c8: ldc.i8 -121
		IL_01d1: add
		IL_01d2: box [mscorlib]System.Int64
		IL_01d7: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_01dc: ldloc.s 5
		IL_01de: call int32 [Unity.TextMeshPro]TMPro.TMP_TextUtilities::FindIntersectingWord(class [Unity.TextMeshPro]TMPro.TMP_Text, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, class [UnityEngine.CoreModule]UnityEngine.Camera)
		IL_01e3: stloc.s 25
		IL_01e5: ldloc.s 25
		IL_01e7: stloc.s 17
		IL_01e9: ldloc.s 25
		IL_01eb: ldc.i4.m1
		IL_01ec: ceq
		IL_01ee: brtrue IL_0276

		IL_01f3: nop
		IL_01f4: ldloc.s 17
		IL_01f6: ldarg.0
		IL_01f7: ldfld int32 TMPro.TMP_TextEventHandler::m_lastWordIndex
		IL_01fc: ceq
		IL_01fe: brtrue.s IL_0276

		IL_0200: nop
		IL_0201: ldarg.0
		IL_0202: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.TMP_TextEventHandler::m_TextComponent
		IL_0207: stloc.s 26
		IL_0209: ldarg.0
		IL_020a: ldloc.s 17
		IL_020c: stfld int32 TMPro.TMP_TextEventHandler::m_lastWordIndex
		IL_0211: ldloc.s 26
		IL_0213: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo [Unity.TextMeshPro]TMPro.TMP_Text::m_textInfo
		IL_0218: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_WordInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::wordInfo
		IL_021d: stloc.s 27
		IL_021f: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0224: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0229: conv.i8
		IL_022a: ldc.i8 -105
		IL_0233: add
		IL_0234: call instance string [Unity.TextMeshPro]TMPro.TMP_WordInfo::GetWord()
		IL_0239: stloc.s 28
		IL_023b: ldstr "*(LocalExpr + 0x30)"
		IL_0240: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0245: stloc.s 29
		IL_0247: ldstr "stack[0xFFFFFFFFFFFFFF40]"
		IL_024c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0251: conv.i4
		IL_0252: stloc.s 30
		IL_0254: ldloc.s 28
		IL_0256: stloc.s 31
		IL_0258: ldarg.0
		IL_0259: ldfld class TMPro.TMP_TextEventHandler/WordSelectionEvent TMPro.TMP_TextEventHandler::m_OnWordSelection
		IL_025e: ldnull
		IL_025f: ceq
		IL_0261: brtrue.s IL_0276

		IL_0263: nop
		IL_0264: ldarg.0
		IL_0265: ldfld class TMPro.TMP_TextEventHandler/WordSelectionEvent TMPro.TMP_TextEventHandler::m_OnWordSelection
		IL_026a: ldloc.s 31
		IL_026c: ldloc.s 30
		IL_026e: ldloc.s 29
		IL_0270: conv.i4
		IL_0271: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`3<string, int32, int32>::Invoke(!0, !1, !2)

		IL_0276: nop
		IL_0277: ldarg.0
		IL_0278: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.TMP_TextEventHandler::m_TextComponent
		IL_027d: stloc.s 32
		IL_027f: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.InputLegacyModule]UnityEngine.Input::get_mousePosition()
		IL_0284: stloc.s 33
		IL_0286: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_028b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0290: stloc.s 4
		IL_0292: ldarg.0
		IL_0293: ldfld class [UnityEngine.CoreModule]UnityEngine.Camera TMPro.TMP_TextEventHandler::m_Camera
		IL_0298: stloc.s 34
		IL_029a: ldloca.s 33
		IL_029c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_02a1: stloc.s 35
		IL_02a3: ldloc.s 32
		IL_02a5: stloc.s 36
		IL_02a7: br.s IL_02aa

		IL_02a9: nop

		IL_02aa: nop
		IL_02ab: ldloc.s 36
		IL_02ad: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_02b2: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_02b7: conv.i8
		IL_02b8: ldc.i8 -121
		IL_02c1: add
		IL_02c2: box [mscorlib]System.Int64
		IL_02c7: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_02cc: ldloc.s 34
		IL_02ce: call int32 [Unity.TextMeshPro]TMPro.TMP_TextUtilities::FindIntersectingLine(class [Unity.TextMeshPro]TMPro.TMP_Text, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, class [UnityEngine.CoreModule]UnityEngine.Camera)
		IL_02d3: stloc.s 37
		IL_02d5: ldloc.s 37
		IL_02d7: stloc.s 17
		IL_02d9: ldloc.s 37
		IL_02db: ldc.i4.m1
		IL_02dc: ceq
		IL_02de: brtrue IL_0409

		IL_02e3: nop
		IL_02e4: ldloc.s 17
		IL_02e6: ldarg.0
		IL_02e7: ldfld int32 TMPro.TMP_TextEventHandler::m_lastLineIndex
		IL_02ec: ceq
		IL_02ee: brtrue IL_0409

		IL_02f3: nop
		IL_02f4: ldarg.0
		IL_02f5: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.TMP_TextEventHandler::m_TextComponent
		IL_02fa: stloc.s 38
		IL_02fc: ldarg.0
		IL_02fd: ldloc.s 17
		IL_02ff: stfld int32 TMPro.TMP_TextEventHandler::m_lastLineIndex
		IL_0304: ldloc.s 38
		IL_0306: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo [Unity.TextMeshPro]TMPro.TMP_Text::m_textInfo
		IL_030b: stloc.s 39
		IL_030d: ldloc.s 39
		IL_030f: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_LineInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::lineInfo
		IL_0314: stloc.s 40
		IL_0316: ldstr "*(BinaryExpr + 0x20)"
		IL_031b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0320: ldc.i8 32
		IL_0329: shr.un
		IL_032a: conv.i4
		IL_032b: newarr [mscorlib]System.Char
		IL_0330: stloc.s 41
		IL_0332: ldloc.s 41
		IL_0334: stloc.s 42
		IL_0336: ldstr "*(BinaryExpr + 0x20)"
		IL_033b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0340: stloc.s 43
		IL_0342: ldstr "*(BinaryExpr + 0x30)"
		IL_0347: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_034c: stloc.s 4
		IL_034e: ldstr "*(BinaryExpr + 0x20)"
		IL_0353: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0358: ldc.i8 32
		IL_0361: shr.un
		IL_0362: conv.i4
		IL_0363: ldstr "*(BinaryExpr + 0x20)"
		IL_0368: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_036d: ldc.i8 32
		IL_0376: shr.un
		IL_0377: conv.i4
		IL_0378: and
		IL_0379: ldc.i4.0
		IL_037a: cgt
		IL_037c: ldc.i4.0
		IL_037d: ceq
		IL_037f: brtrue.s IL_03cb
		// loop start (head: IL_0381)
			IL_0381: nop
			IL_0382: ldc.i4.0
			IL_0383: ldarg.0
			IL_0384: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.TMP_TextEventHandler::m_TextComponent
			IL_0389: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo [Unity.TextMeshPro]TMPro.TMP_Text::m_textInfo
			IL_038e: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_CharacterInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterInfo
			IL_0393: ldlen
			IL_0394: conv.i4
			IL_0395: clt
			IL_0397: ldc.i4.0
			IL_0398: ceq
			IL_039a: brtrue.s IL_03cb

			IL_039c: nop
			IL_039d: ldloc.s 42
			IL_039f: ldc.i4.0
			IL_03a0: conv.i8
			IL_03a1: conv.i4
			IL_03a2: ldstr "*(FieldExpr + 0x20)"
			IL_03a7: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_03ac: conv.i4
			IL_03ad: stelem [mscorlib]System.Char
			IL_03b2: ldc.i4.0
			IL_03b3: ldc.i4.1
			IL_03b4: add
			IL_03b5: stloc.s 44
			IL_03b7: ldc.i4.0
			IL_03b8: ldc.i4.1
			IL_03b9: add
			IL_03ba: ldloc.s 43
			IL_03bc: ldc.i8 32
			IL_03c5: shr.un
			IL_03c6: conv.i4
			IL_03c7: clt
			IL_03c9: brtrue.s IL_0381
		// end loop

		IL_03cb: nop
		IL_03cc: ldnull
		IL_03cd: ldloc.s 42
		IL_03cf: callvirt instance string [mscorlib]System.String::CreateString(char[])
		IL_03d4: stloc.s 45
		IL_03d6: ldloc.s 45
		IL_03d8: stloc.s 46
		IL_03da: ldarg.0
		IL_03db: ldfld class TMPro.TMP_TextEventHandler/LineSelectionEvent TMPro.TMP_TextEventHandler::m_OnLineSelection
		IL_03e0: ldnull
		IL_03e1: ceq
		IL_03e3: brtrue.s IL_0409

		IL_03e5: nop
		IL_03e6: ldarg.0
		IL_03e7: ldfld class TMPro.TMP_TextEventHandler/LineSelectionEvent TMPro.TMP_TextEventHandler::m_OnLineSelection
		IL_03ec: ldloc.s 46
		IL_03ee: ldstr "psrldq xmm6,4"
		IL_03f3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_03f8: conv.i4
		IL_03f9: ldstr "psrldq xmm7,4"
		IL_03fe: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0403: conv.i4
		IL_0404: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`3<string, int32, int32>::Invoke(!0, !1, !2)

		IL_0409: nop
		IL_040a: ldarg.0
		IL_040b: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.TMP_TextEventHandler::m_TextComponent
		IL_0410: stloc.s 47
		IL_0412: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.InputLegacyModule]UnityEngine.Input::get_mousePosition()
		IL_0417: stloc.s 48
		IL_0419: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_041e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0423: stloc.s 4
		IL_0425: ldarg.0
		IL_0426: ldfld class [UnityEngine.CoreModule]UnityEngine.Camera TMPro.TMP_TextEventHandler::m_Camera
		IL_042b: stloc.s 34
		IL_042d: ldloca.s 48
		IL_042f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0434: stloc.s 35
		IL_0436: ldloc.s 47
		IL_0438: stloc.s 49
		IL_043a: br.s IL_043d

		IL_043c: nop

		IL_043d: nop
		IL_043e: ldloc.s 49
		IL_0440: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0445: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_044a: conv.i8
		IL_044b: ldc.i8 -121
		IL_0454: add
		IL_0455: box [mscorlib]System.Int64
		IL_045a: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_045f: ldloc.s 34
		IL_0461: call int32 [Unity.TextMeshPro]TMPro.TMP_TextUtilities::FindIntersectingLink(class [Unity.TextMeshPro]TMPro.TMP_Text, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, class [UnityEngine.CoreModule]UnityEngine.Camera)
		IL_0466: stloc.s 50
		IL_0468: ldloc.s 50
		IL_046a: stloc.s 17
		IL_046c: ldloc.s 50
		IL_046e: conv.i8
		IL_046f: stloc.s 51
		IL_0471: ldloc.s 50
		IL_0473: conv.i8
		IL_0474: conv.i4
		IL_0475: ldc.i4.m1
		IL_0476: ceq
		IL_0478: brtrue IL_0514

		IL_047d: nop
		IL_047e: ldloc.s 51
		IL_0480: conv.i4
		IL_0481: ldarg.0
		IL_0482: ldfld int32 TMPro.TMP_TextEventHandler::m_selectedLink
		IL_0487: ceq
		IL_0489: brtrue IL_0514

		IL_048e: nop
		IL_048f: ldarg.0
		IL_0490: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text TMPro.TMP_TextEventHandler::m_TextComponent
		IL_0495: stloc.s 52
		IL_0497: ldarg.0
		IL_0498: ldloc.s 51
		IL_049a: conv.i4
		IL_049b: stfld int32 TMPro.TMP_TextEventHandler::m_selectedLink
		IL_04a0: ldloc.s 52
		IL_04a2: stloc.s 53
		IL_04a4: ldloc.s 53
		IL_04a6: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo [Unity.TextMeshPro]TMPro.TMP_Text::m_textInfo
		IL_04ab: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_LinkInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::linkInfo
		IL_04b0: stloc.s 54
		IL_04b2: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_04b7: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_04bc: conv.i8
		IL_04bd: ldc.i8 -81
		IL_04c6: add
		IL_04c7: call instance string [Unity.TextMeshPro]TMPro.TMP_LinkInfo::GetLinkID()
		IL_04cc: stloc.s 55
		IL_04ce: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_04d3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_04d8: conv.i8
		IL_04d9: ldc.i8 -81
		IL_04e2: add
		IL_04e3: call instance string [Unity.TextMeshPro]TMPro.TMP_LinkInfo::GetLinkText()
		IL_04e8: stloc.s 56
		IL_04ea: ldloc.s 56
		IL_04ec: stloc.s 57
		IL_04ee: ldloc.s 56
		IL_04f0: stloc.s 58
		IL_04f2: ldloc.s 55
		IL_04f4: stloc.s 59
		IL_04f6: ldarg.0
		IL_04f7: ldfld class TMPro.TMP_TextEventHandler/LinkSelectionEvent TMPro.TMP_TextEventHandler::m_OnLinkSelection
		IL_04fc: ldnull
		IL_04fd: ceq
		IL_04ff: brtrue.s IL_0514

		IL_0501: nop
		IL_0502: ldarg.0
		IL_0503: ldfld class TMPro.TMP_TextEventHandler/LinkSelectionEvent TMPro.TMP_TextEventHandler::m_OnLinkSelection
		IL_0508: ldloc.s 59
		IL_050a: ldloc.s 58
		IL_050c: ldloc.s 51
		IL_050e: conv.i4
		IL_050f: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`3<string, string, int32>::Invoke(!0, !1, !2)

		IL_0514: nop
		IL_0515: ret

		IL_0516: nop
		IL_0517: ldnull
		IL_0518: throw

		IL_0519: nop
		IL_051a: ldnull
		IL_051b: throw
	} // end of method TMP_TextEventHandler::LateUpdate

	.method public final hidebysig newslot virtual 
		instance void OnPointerEnter (
			class [UnityEngine.UI]UnityEngine.EventSystems.PointerEventData eventData
		) cil managed 
	{
		// Method begins at RVA 0x283d
		// Header size: 1
		// Code size: 2 (0x2)
		.maxstack 8

		IL_0000: nop
		IL_0001: ret
	} // end of method TMP_TextEventHandler::OnPointerEnter

	.method public final hidebysig newslot virtual 
		instance void OnPointerExit (
			class [UnityEngine.UI]UnityEngine.EventSystems.PointerEventData eventData
		) cil managed 
	{
		// Method begins at RVA 0x2840
		// Header size: 1
		// Code size: 2 (0x2)
		.maxstack 8

		IL_0000: nop
		IL_0001: ret
	} // end of method TMP_TextEventHandler::OnPointerExit

	.method private hidebysig 
		instance void SendOnCharacterSelection (
			char character,
			int32 characterIndex
		) cil managed 
	{
		// Method begins at RVA 0x148f8
		// Header size: 12
		// Code size: 30 (0x1e)
		.maxstack 3
		.locals init (
			[0] int32
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: stloc.0
		IL_0003: ldarg.0
		IL_0004: ldfld class TMPro.TMP_TextEventHandler/CharacterSelectionEvent TMPro.TMP_TextEventHandler::m_OnCharacterSelection
		IL_0009: ldnull
		IL_000a: ceq
		IL_000c: brtrue.s IL_001c

		IL_000e: nop
		IL_000f: ldarg.0
		IL_0010: ldfld class TMPro.TMP_TextEventHandler/CharacterSelectionEvent TMPro.TMP_TextEventHandler::m_OnCharacterSelection
		IL_0015: ldloc.0
		IL_0016: ldarg.2
		IL_0017: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`2<char, int32>::Invoke(!0, !1)

		IL_001c: nop
		IL_001d: ret
	} // end of method TMP_TextEventHandler::SendOnCharacterSelection

	.method private hidebysig 
		instance void SendOnSpriteSelection (
			char character,
			int32 characterIndex
		) cil managed 
	{
		// Method begins at RVA 0x14924
		// Header size: 12
		// Code size: 30 (0x1e)
		.maxstack 3
		.locals init (
			[0] int32
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: stloc.0
		IL_0003: ldarg.0
		IL_0004: ldfld class TMPro.TMP_TextEventHandler/SpriteSelectionEvent TMPro.TMP_TextEventHandler::m_OnSpriteSelection
		IL_0009: ldnull
		IL_000a: ceq
		IL_000c: brtrue.s IL_001c

		IL_000e: nop
		IL_000f: ldarg.0
		IL_0010: ldfld class TMPro.TMP_TextEventHandler/SpriteSelectionEvent TMPro.TMP_TextEventHandler::m_OnSpriteSelection
		IL_0015: ldloc.0
		IL_0016: ldarg.2
		IL_0017: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`2<char, int32>::Invoke(!0, !1)

		IL_001c: nop
		IL_001d: ret
	} // end of method TMP_TextEventHandler::SendOnSpriteSelection

	.method private hidebysig 
		instance void SendOnWordSelection (
			string word,
			int32 charIndex,
			int32 length
		) cil managed 
	{
		// Method begins at RVA 0x2843
		// Header size: 1
		// Code size: 29 (0x1d)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class TMPro.TMP_TextEventHandler/WordSelectionEvent TMPro.TMP_TextEventHandler::m_OnWordSelection
		IL_0007: ldnull
		IL_0008: ceq
		IL_000a: brtrue.s IL_001b

		IL_000c: nop
		IL_000d: ldarg.0
		IL_000e: ldfld class TMPro.TMP_TextEventHandler/WordSelectionEvent TMPro.TMP_TextEventHandler::m_OnWordSelection
		IL_0013: ldarg.1
		IL_0014: ldarg.2
		IL_0015: ldarg.3
		IL_0016: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`3<string, int32, int32>::Invoke(!0, !1, !2)

		IL_001b: nop
		IL_001c: ret
	} // end of method TMP_TextEventHandler::SendOnWordSelection

	.method private hidebysig 
		instance void SendOnLineSelection (
			string line,
			int32 charIndex,
			int32 length
		) cil managed 
	{
		// Method begins at RVA 0x2861
		// Header size: 1
		// Code size: 29 (0x1d)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class TMPro.TMP_TextEventHandler/LineSelectionEvent TMPro.TMP_TextEventHandler::m_OnLineSelection
		IL_0007: ldnull
		IL_0008: ceq
		IL_000a: brtrue.s IL_001b

		IL_000c: nop
		IL_000d: ldarg.0
		IL_000e: ldfld class TMPro.TMP_TextEventHandler/LineSelectionEvent TMPro.TMP_TextEventHandler::m_OnLineSelection
		IL_0013: ldarg.1
		IL_0014: ldarg.2
		IL_0015: ldarg.3
		IL_0016: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`3<string, int32, int32>::Invoke(!0, !1, !2)

		IL_001b: nop
		IL_001c: ret
	} // end of method TMP_TextEventHandler::SendOnLineSelection

	.method private hidebysig 
		instance void SendOnLinkSelection (
			string linkID,
			string linkText,
			int32 linkIndex
		) cil managed 
	{
		// Method begins at RVA 0x287f
		// Header size: 1
		// Code size: 29 (0x1d)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class TMPro.TMP_TextEventHandler/LinkSelectionEvent TMPro.TMP_TextEventHandler::m_OnLinkSelection
		IL_0007: ldnull
		IL_0008: ceq
		IL_000a: brtrue.s IL_001b

		IL_000c: nop
		IL_000d: ldarg.0
		IL_000e: ldfld class TMPro.TMP_TextEventHandler/LinkSelectionEvent TMPro.TMP_TextEventHandler::m_OnLinkSelection
		IL_0013: ldarg.1
		IL_0014: ldarg.2
		IL_0015: ldarg.3
		IL_0016: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`3<string, string, int32>::Invoke(!0, !1, !2)

		IL_001b: nop
		IL_001c: ret
	} // end of method TMP_TextEventHandler::SendOnLinkSelection

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x14950
		// Header size: 12
		// Code size: 107 (0x6b)
		.maxstack 2
		.locals init (
			[0] class TMPro.TMP_TextEventHandler/CharacterSelectionEvent,
			[1] class TMPro.TMP_TextEventHandler/SpriteSelectionEvent,
			[2] class TMPro.TMP_TextEventHandler/WordSelectionEvent,
			[3] class TMPro.TMP_TextEventHandler/LineSelectionEvent,
			[4] class TMPro.TMP_TextEventHandler/LinkSelectionEvent
		)

		IL_0000: nop
		IL_0001: newobj instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`1<bool>::.ctor()
		IL_0006: stloc.0
		IL_0007: ldarg.0
		IL_0008: ldloc.0
		IL_0009: stfld class TMPro.TMP_TextEventHandler/CharacterSelectionEvent TMPro.TMP_TextEventHandler::m_OnCharacterSelection
		IL_000e: newobj instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`1<bool>::.ctor()
		IL_0013: stloc.1
		IL_0014: ldarg.0
		IL_0015: ldloc.1
		IL_0016: stfld class TMPro.TMP_TextEventHandler/SpriteSelectionEvent TMPro.TMP_TextEventHandler::m_OnSpriteSelection
		IL_001b: newobj instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`1<bool>::.ctor()
		IL_0020: stloc.2
		IL_0021: ldarg.0
		IL_0022: ldloc.2
		IL_0023: stfld class TMPro.TMP_TextEventHandler/WordSelectionEvent TMPro.TMP_TextEventHandler::m_OnWordSelection
		IL_0028: newobj instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`1<bool>::.ctor()
		IL_002d: stloc.3
		IL_002e: ldarg.0
		IL_002f: ldloc.3
		IL_0030: stfld class TMPro.TMP_TextEventHandler/LineSelectionEvent TMPro.TMP_TextEventHandler::m_OnLineSelection
		IL_0035: newobj instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`1<bool>::.ctor()
		IL_003a: stloc.s 4
		IL_003c: ldarg.0
		IL_003d: ldloc.s 4
		IL_003f: stfld class TMPro.TMP_TextEventHandler/LinkSelectionEvent TMPro.TMP_TextEventHandler::m_OnLinkSelection
		IL_0044: ldarg.0
		IL_0045: ldc.i8 -1
		IL_004e: conv.i4
		IL_004f: stfld int32 TMPro.TMP_TextEventHandler::m_selectedLink
		IL_0054: ldarg.0
		IL_0055: ldc.i8 -1
		IL_005e: conv.i4
		IL_005f: stfld int32 TMPro.TMP_TextEventHandler::m_lastWordIndex
		IL_0064: ldarg.0
		IL_0065: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_006a: ret
	} // end of method TMP_TextEventHandler::.ctor

	// Properties
	.property instance class TMPro.TMP_TextEventHandler/CharacterSelectionEvent onCharacterSelection()
	{
		.get instance class TMPro.TMP_TextEventHandler/CharacterSelectionEvent TMPro.TMP_TextEventHandler::get_onCharacterSelection()
		.set instance void TMPro.TMP_TextEventHandler::set_onCharacterSelection(class TMPro.TMP_TextEventHandler/CharacterSelectionEvent)
	}
	.property instance class TMPro.TMP_TextEventHandler/SpriteSelectionEvent onSpriteSelection()
	{
		.get instance class TMPro.TMP_TextEventHandler/SpriteSelectionEvent TMPro.TMP_TextEventHandler::get_onSpriteSelection()
		.set instance void TMPro.TMP_TextEventHandler::set_onSpriteSelection(class TMPro.TMP_TextEventHandler/SpriteSelectionEvent)
	}
	.property instance class TMPro.TMP_TextEventHandler/WordSelectionEvent onWordSelection()
	{
		.get instance class TMPro.TMP_TextEventHandler/WordSelectionEvent TMPro.TMP_TextEventHandler::get_onWordSelection()
		.set instance void TMPro.TMP_TextEventHandler::set_onWordSelection(class TMPro.TMP_TextEventHandler/WordSelectionEvent)
	}
	.property instance class TMPro.TMP_TextEventHandler/LineSelectionEvent onLineSelection()
	{
		.get instance class TMPro.TMP_TextEventHandler/LineSelectionEvent TMPro.TMP_TextEventHandler::get_onLineSelection()
		.set instance void TMPro.TMP_TextEventHandler::set_onLineSelection(class TMPro.TMP_TextEventHandler/LineSelectionEvent)
	}
	.property instance class TMPro.TMP_TextEventHandler/LinkSelectionEvent onLinkSelection()
	{
		.get instance class TMPro.TMP_TextEventHandler/LinkSelectionEvent TMPro.TMP_TextEventHandler::get_onLinkSelection()
		.set instance void TMPro.TMP_TextEventHandler::set_onLinkSelection(class TMPro.TMP_TextEventHandler/LinkSelectionEvent)
	}

} // end of class TMPro.TMP_TextEventHandler
