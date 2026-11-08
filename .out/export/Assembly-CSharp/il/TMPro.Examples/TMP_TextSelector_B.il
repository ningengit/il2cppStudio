.class public auto ansi beforefieldinit TMPro.Examples.TMP_TextSelector_B
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
	implements [UnityEngine.UI]UnityEngine.EventSystems.IPointerEnterHandler,
	           [UnityEngine.UI]UnityEngine.EventSystems.IEventSystemHandler,
	           [UnityEngine.UI]UnityEngine.EventSystems.IPointerExitHandler,
	           [UnityEngine.UI]UnityEngine.EventSystems.IPointerClickHandler,
	           [UnityEngine.UI]UnityEngine.EventSystems.IPointerUpHandler
{
	// Fields
	.field public class [UnityEngine.CoreModule]UnityEngine.RectTransform TextPopup_Prefab_01
	.field private class [UnityEngine.CoreModule]UnityEngine.RectTransform m_TextPopup_RectTransform
	.field private class [Unity.TextMeshPro]TMPro.TextMeshProUGUI m_TextPopup_TMPComponent
	.field private static literal string k_LinkText
	.field private static literal string k_WordText
	.field private class [Unity.TextMeshPro]TMPro.TextMeshProUGUI m_TextMeshPro
	.field private class [UnityEngine.UIModule]UnityEngine.Canvas m_Canvas
	.field private class [UnityEngine.CoreModule]UnityEngine.Camera m_Camera
	.field private bool isHoveringObject
	.field private int32 m_selectedWord
	.field private int32 m_selectedLink
	.field private int32 m_lastIndex
	.field private valuetype [UnityEngine.CoreModule]UnityEngine.Matrix4x4 m_matrix
	.field private valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] m_cachedMeshInfoVertexData

	// Methods
	.method private hidebysig 
		instance void Awake () cil managed 
	{
		// Method begins at RVA 0x1821c
		// Header size: 12
		// Code size: 229 (0xe5)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[1] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[2] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[3] class [UnityEngine.UIModule]UnityEngine.Canvas,
			[4] valuetype [UnityEngine.UIModule]UnityEngine.RenderMode,
			[5] class [UnityEngine.UIModule]UnityEngine.Canvas,
			[6] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[7] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[8] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[9] class [UnityEngine.CoreModule]UnityEngine.RectTransform,
			[10] class [UnityEngine.CoreModule]UnityEngine.RectTransform,
			[11] class [UnityEngine.CoreModule]UnityEngine.RectTransform,
			[12] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[13] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[14] class [UnityEngine.CoreModule]UnityEngine.GameObject
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::GetComponent<class [Unity.TextMeshPro]TMPro.TextMeshProUGUI>()
		IL_000e: stloc.1
		IL_000f: ldarg.0
		IL_0010: ldloc.1
		IL_0011: stfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.TMP_TextSelector_B::m_TextMeshPro
		IL_0016: ldarg.0
		IL_0017: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_001c: stloc.2
		IL_001d: ldloc.2
		IL_001e: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::GetComponentInParent<class [UnityEngine.UIModule]UnityEngine.Canvas>()
		IL_0023: stloc.3
		IL_0024: ldarg.0
		IL_0025: ldloc.3
		IL_0026: stfld class [UnityEngine.UIModule]UnityEngine.Canvas TMPro.Examples.TMP_TextSelector_B::m_Canvas
		IL_002b: ldarg.0
		IL_002c: ldfld class [UnityEngine.UIModule]UnityEngine.Canvas TMPro.Examples.TMP_TextSelector_B::m_Canvas
		IL_0031: callvirt instance valuetype [UnityEngine.UIModule]UnityEngine.RenderMode [UnityEngine.UIModule]UnityEngine.Canvas::get_renderMode()
		IL_0036: stloc.s 4
		IL_0038: ldloc.s 4
		IL_003a: box [mscorlib]System.Object
		IL_003f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0044: ldloc.s 4
		IL_0046: box [mscorlib]System.Object
		IL_004b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0050: and
		IL_0051: ldc.i4.0
		IL_0052: ceq
		IL_0054: brtrue.s IL_0072

		IL_0056: nop
		IL_0057: ldarg.0
		IL_0058: ldfld class [UnityEngine.UIModule]UnityEngine.Canvas TMPro.Examples.TMP_TextSelector_B::m_Canvas
		IL_005d: stloc.s 5
		IL_005f: ldloc.s 5
		IL_0061: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.UIModule]UnityEngine.Canvas::get_worldCamera()
		IL_0066: stloc.s 6
		IL_0068: ldloc.s 6
		IL_006a: stloc.s 7
		IL_006c: ldloc.s 6
		IL_006e: stloc.s 8
		IL_0070: br.s IL_0073

		IL_0072: nop

		IL_0073: nop
		IL_0074: ldarg.0
		IL_0075: ldnull
		IL_0076: stfld class [UnityEngine.CoreModule]UnityEngine.Camera TMPro.Examples.TMP_TextSelector_B::m_Camera
		IL_007b: ldarg.0
		IL_007c: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform TMPro.Examples.TMP_TextSelector_B::TextPopup_Prefab_01
		IL_0081: stloc.s 9
		IL_0083: br.s IL_0086

		IL_0085: nop

		IL_0086: nop
		IL_0087: ldloc.s 9
		IL_0089: call !!0 [UnityEngine.CoreModule]UnityEngine.Object::Instantiate<class [UnityEngine.CoreModule]UnityEngine.RectTransform>(!!0)
		IL_008e: stloc.s 10
		IL_0090: ldarg.0
		IL_0091: ldloc.s 10
		IL_0093: stfld class [UnityEngine.CoreModule]UnityEngine.RectTransform TMPro.Examples.TMP_TextSelector_B::m_TextPopup_RectTransform
		IL_0098: ldarg.0
		IL_0099: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform TMPro.Examples.TMP_TextSelector_B::m_TextPopup_RectTransform
		IL_009e: stloc.s 11
		IL_00a0: ldarg.0
		IL_00a1: ldfld class [UnityEngine.UIModule]UnityEngine.Canvas TMPro.Examples.TMP_TextSelector_B::m_Canvas
		IL_00a6: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_00ab: stloc.s 12
		IL_00ad: ldloc.s 11
		IL_00af: ldloc.s 12
		IL_00b1: ldc.i4.0
		IL_00b2: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::SetParent(class [UnityEngine.CoreModule]UnityEngine.Transform, bool)
		IL_00b7: ldarg.0
		IL_00b8: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform TMPro.Examples.TMP_TextSelector_B::m_TextPopup_RectTransform
		IL_00bd: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponentInChildren<class [Unity.TextMeshPro]TMPro.TextMeshProUGUI>()
		IL_00c2: stloc.s 13
		IL_00c4: ldarg.0
		IL_00c5: ldloc.s 13
		IL_00c7: stfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.TMP_TextSelector_B::m_TextPopup_TMPComponent
		IL_00cc: ldarg.0
		IL_00cd: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform TMPro.Examples.TMP_TextSelector_B::m_TextPopup_RectTransform
		IL_00d2: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_00d7: stloc.s 14
		IL_00d9: ldloc.s 14
		IL_00db: ldc.i4.0
		IL_00dc: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_00e1: ret

		IL_00e2: nop
		IL_00e3: ldnull
		IL_00e4: throw
	} // end of method TMP_TextSelector_B::Awake

	.method private hidebysig 
		instance void OnEnable () cil managed 
	{
		// Method begins at RVA 0x18310
		// Header size: 12
		// Code size: 35 (0x23)
		.maxstack 2
		.locals init (
			[0] class [Unity.TextMeshPro]TMPro.FastAction`1<class [UnityEngine.CoreModule]UnityEngine.Object>,
			[1] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Object>
		)

		IL_0000: nop
		IL_0001: br.s IL_0004

		IL_0003: nop

		IL_0004: nop
		IL_0005: ldsfld class [Unity.TextMeshPro]TMPro.FastAction`1<class [UnityEngine.CoreModule]UnityEngine.Object> [Unity.TextMeshPro]TMPro.TMPro_EventManager::TEXT_CHANGED_EVENT
		IL_000a: stloc.0
		IL_000b: ldarg.0
		IL_000c: ldftn instance void TMPro.Examples.TMP_TextSelector_B::ON_TEXT_CHANGED(class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0012: newobj instance void [System]System.IOAsyncCallback::.ctor(object, native int)
		IL_0017: stloc.1
		IL_0018: ldloc.0
		IL_0019: ldloc.1
		IL_001a: callvirt instance void class [Unity.TextMeshPro]TMPro.FastAction`1<class [UnityEngine.CoreModule]UnityEngine.Object>::Add(class [mscorlib]System.Action`1<!0>)
		IL_001f: ret

		IL_0020: nop
		IL_0021: ldnull
		IL_0022: throw
	} // end of method TMP_TextSelector_B::OnEnable

	.method private hidebysig 
		instance void OnDisable () cil managed 
	{
		// Method begins at RVA 0x18340
		// Header size: 12
		// Code size: 35 (0x23)
		.maxstack 2
		.locals init (
			[0] class [Unity.TextMeshPro]TMPro.FastAction`1<class [UnityEngine.CoreModule]UnityEngine.Object>,
			[1] class [mscorlib]System.Action`1<class [UnityEngine.CoreModule]UnityEngine.Object>
		)

		IL_0000: nop
		IL_0001: br.s IL_0004

		IL_0003: nop

		IL_0004: nop
		IL_0005: ldsfld class [Unity.TextMeshPro]TMPro.FastAction`1<class [UnityEngine.CoreModule]UnityEngine.Object> [Unity.TextMeshPro]TMPro.TMPro_EventManager::TEXT_CHANGED_EVENT
		IL_000a: stloc.0
		IL_000b: ldarg.0
		IL_000c: ldftn instance void TMPro.Examples.TMP_TextSelector_B::ON_TEXT_CHANGED(class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0012: newobj instance void [System]System.IOAsyncCallback::.ctor(object, native int)
		IL_0017: stloc.1
		IL_0018: ldloc.0
		IL_0019: ldloc.1
		IL_001a: callvirt instance void class [Unity.TextMeshPro]TMPro.FastAction`1<class [UnityEngine.CoreModule]UnityEngine.Object>::Remove(class [mscorlib]System.Action`1<!0>)
		IL_001f: ret

		IL_0020: nop
		IL_0021: ldnull
		IL_0022: throw
	} // end of method TMP_TextSelector_B::OnDisable

	.method private hidebysig 
		instance void ON_TEXT_CHANGED (
			class [UnityEngine.CoreModule]UnityEngine.Object obj
		) cil managed 
	{
		// Method begins at RVA 0x18370
		// Header size: 12
		// Code size: 62 (0x3e)
		.maxstack 2
		.locals init (
			[0] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[1] bool,
			[2] bool,
			[3] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[4] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[]
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.TMP_TextSelector_B::m_TextMeshPro
		IL_0007: stloc.0
		IL_0008: br.s IL_000b

		IL_000a: nop

		IL_000b: nop
		IL_000c: ldarg.1
		IL_000d: ldloc.0
		IL_000e: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Equality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0013: stloc.1
		IL_0014: ldloc.1
		IL_0015: stloc.2
		IL_0016: ldloc.1
		IL_0017: ldc.i4.0
		IL_0018: ceq
		IL_001a: brtrue.s IL_0039

		IL_001c: nop
		IL_001d: ldarg.0
		IL_001e: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.TMP_TextSelector_B::m_TextMeshPro
		IL_0023: stloc.3
		IL_0024: ldloc.3
		IL_0025: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo [Unity.TextMeshPro]TMPro.TMP_Text::m_textInfo
		IL_002a: callvirt instance valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::CopyMeshInfoVertexData()
		IL_002f: stloc.s 4
		IL_0031: ldarg.0
		IL_0032: ldloc.s 4
		IL_0034: stfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] TMPro.Examples.TMP_TextSelector_B::m_cachedMeshInfoVertexData

		IL_0039: nop
		IL_003a: ret

		IL_003b: nop
		IL_003c: ldnull
		IL_003d: throw
	} // end of method TMP_TextSelector_B::ON_TEXT_CHANGED

	.method private hidebysig 
		instance void LateUpdate () cil managed 
	{
		// Method begins at RVA 0x183bc
		// Header size: 12
		// Code size: 751 (0x2ef)
		.maxstack 4
		.locals init (
			[0] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[2] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[3] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[4] int64,
			[5] float32,
			[6] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[7] int32,
			[8] int64,
			[9] bool,
			[10] bool,
			[11] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[12] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[13] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
			[14] valuetype [Unity.TextMeshPro]TMPro.TMP_CharacterInfo[],
			[15] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[16] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[],
			[17] int64,
			[18] int64,
			[19] float32,
			[20] float32,
			[21] float32,
			[22] float32,
			[23] float32,
			[24] int64
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld bool TMPro.Examples.TMP_TextSelector_B::isHoveringObject
		IL_0007: ldc.i4.0
		IL_0008: ceq
		IL_000a: ldc.i4.0
		IL_000b: ceq
		IL_000d: brtrue.s IL_0025

		IL_000f: nop
		IL_0010: nop
		IL_0011: ldarg.0
		IL_0012: ldarg.0
		IL_0013: ldfld int32 TMPro.Examples.TMP_TextSelector_B::m_lastIndex
		IL_0018: callvirt instance void TMPro.Examples.TMP_TextSelector_B::RestoreCachedVertexAttributes(int32)
		IL_001d: ldarg.0
		IL_001e: ldc.i4.m1
		IL_001f: stfld int32 TMPro.Examples.TMP_TextSelector_B::m_lastIndex
		IL_0024: ret

		IL_0025: nop
		IL_0026: ldarg.0
		IL_0027: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.TMP_TextSelector_B::m_TextMeshPro
		IL_002c: stloc.0
		IL_002d: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.InputLegacyModule]UnityEngine.Input::get_mousePosition()
		IL_0032: stloc.1
		IL_0033: ldarg.0
		IL_0034: ldfld class [UnityEngine.CoreModule]UnityEngine.Camera TMPro.Examples.TMP_TextSelector_B::m_Camera
		IL_0039: stloc.2
		IL_003a: ldloc.0
		IL_003b: stloc.3
		IL_003c: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_0041: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0046: stloc.s 4
		IL_0048: ldloca.s 1
		IL_004a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_004f: stloc.s 5
		IL_0051: br.s IL_0054

		IL_0053: nop

		IL_0054: nop
		IL_0055: ldloc.3
		IL_0056: ldloca.s 6
		IL_0058: ldloc.s 4
		IL_005a: box [mscorlib]System.Int64
		IL_005f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0064: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0069: ldloca.s 6
		IL_006b: ldloc.s 5
		IL_006d: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0072: ldloc.s 6
		IL_0074: ldloc.2
		IL_0075: ldc.i4.1
		IL_0076: call int32 [Unity.TextMeshPro]TMPro.TMP_TextUtilities::FindIntersectingCharacter(class [Unity.TextMeshPro]TMPro.TMP_Text, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, class [UnityEngine.CoreModule]UnityEngine.Camera, bool)
		IL_007b: stloc.s 7
		IL_007d: ldloc.s 7
		IL_007f: conv.i8
		IL_0080: stloc.s 8
		IL_0082: ldloc.s 7
		IL_0084: conv.i8
		IL_0085: conv.i4
		IL_0086: ldc.i4.m1
		IL_0087: ceq
		IL_0089: brtrue.s IL_008c

		IL_008b: nop

		IL_008c: nop
		IL_008d: ldarg.0
		IL_008e: ldarg.0
		IL_008f: ldfld int32 TMPro.Examples.TMP_TextSelector_B::m_lastIndex
		IL_0094: callvirt instance void TMPro.Examples.TMP_TextSelector_B::RestoreCachedVertexAttributes(int32)
		IL_0099: ldarg.0
		IL_009a: ldc.i4.m1
		IL_009b: stfld int32 TMPro.Examples.TMP_TextSelector_B::m_lastIndex
		IL_00a0: nop
		IL_00a1: nop
		IL_00a2: ldc.i4 304
		IL_00a7: box [mscorlib]System.Int32
		IL_00ac: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
		IL_00b1: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetKeyInt(valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode)
		IL_00b6: stloc.s 9
		IL_00b8: ldloc.s 9
		IL_00ba: ldc.i4.0
		IL_00bb: ceq
		IL_00bd: ldc.i4.0
		IL_00be: ceq
		IL_00c0: brtrue.s IL_00d9

		IL_00c2: nop
		IL_00c3: ldc.i4 303
		IL_00c8: box [mscorlib]System.Int32
		IL_00cd: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
		IL_00d2: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetKeyInt(valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode)
		IL_00d7: stloc.s 10

		IL_00d9: nop
		IL_00da: ldarg.0
		IL_00db: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.TMP_TextSelector_B::m_TextMeshPro
		IL_00e0: stloc.s 11
		IL_00e2: ldarg.0
		IL_00e3: ldloc.s 8
		IL_00e5: conv.i4
		IL_00e6: stfld int32 TMPro.Examples.TMP_TextSelector_B::m_lastIndex
		IL_00eb: ldloc.s 11
		IL_00ed: stloc.s 12
		IL_00ef: nop
		IL_00f0: ldloc.s 12
		IL_00f2: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo [Unity.TextMeshPro]TMPro.TMP_Text::m_textInfo
		IL_00f7: stloc.s 13
		IL_00f9: nop
		IL_00fa: ldloc.s 13
		IL_00fc: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_CharacterInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterInfo
		IL_0101: stloc.s 14
		IL_0103: nop
		IL_0104: nop
		IL_0105: ldloc.s 12
		IL_0107: stloc.s 15
		IL_0109: nop
		IL_010a: ldloc.s 12
		IL_010c: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo [Unity.TextMeshPro]TMPro.TMP_Text::m_textInfo
		IL_0111: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
		IL_0116: stloc.s 16
		IL_0118: nop
		IL_0119: ldstr "*(BinaryExpr + 0x58)"
		IL_011e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0123: conv.i4
		IL_0124: conv.i8
		IL_0125: stloc.s 17
		IL_0127: nop
		IL_0128: ldstr "*(LocalExpr + 0x30)"
		IL_012d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0132: stloc.s 18
		IL_0134: nop
		IL_0135: ldstr "*(BinaryExpr + 0x6C)"
		IL_013a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_013f: conv.i4
		IL_0140: conv.i8
		IL_0141: stloc.s 8
		IL_0143: nop
		IL_0144: nop
		IL_0145: ldstr "*(LocalExpr + 0x20)"
		IL_014a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_014f: conv.i4
		IL_0150: conv.r4
		IL_0151: ldstr "*(LocalExpr + 0x20)"
		IL_0156: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_015b: conv.i4
		IL_015c: conv.r4
		IL_015d: ldstr "*(LocalExpr + 0x20)"
		IL_0162: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0167: conv.i4
		IL_0168: conv.r4
		IL_0169: add
		IL_016a: ldc.r4 0.5
		IL_016f: mul
		IL_0170: sub
		IL_0171: stloc.s 19
		IL_0173: ldstr "*(LocalExpr + 0x24)"
		IL_0178: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_017d: conv.i4
		IL_017e: conv.r4
		IL_017f: ldstr "*(LocalExpr + 0x24)"
		IL_0184: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0189: conv.i4
		IL_018a: conv.r4
		IL_018b: ldstr "*(LocalExpr + 0x24)"
		IL_0190: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0195: conv.i4
		IL_0196: conv.r4
		IL_0197: add
		IL_0198: ldc.r4 0.5
		IL_019d: mul
		IL_019e: sub
		IL_019f: stloc.s 20
		IL_01a1: ldstr "*(LocalExpr + 0x28)"
		IL_01a6: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01ab: conv.i4
		IL_01ac: conv.r4
		IL_01ad: ldc.r4 0.0
		IL_01b2: sub
		IL_01b3: stloc.s 21
		IL_01b5: ldstr "*(LocalExpr + 0x20)"
		IL_01ba: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01bf: conv.i4
		IL_01c0: conv.r4
		IL_01c1: ldstr "*(LocalExpr + 0x20)"
		IL_01c6: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01cb: conv.i4
		IL_01cc: conv.r4
		IL_01cd: add
		IL_01ce: ldc.r4 0.5
		IL_01d3: mul
		IL_01d4: stloc.s 22
		IL_01d6: ldstr "*(LocalExpr + 0x24)"
		IL_01db: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01e0: conv.i4
		IL_01e1: conv.r4
		IL_01e2: ldstr "*(LocalExpr + 0x24)"
		IL_01e7: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01ec: conv.i4
		IL_01ed: conv.r4
		IL_01ee: add
		IL_01ef: ldc.r4 0.5
		IL_01f4: mul
		IL_01f5: stloc.s 23
		IL_01f7: nop
		IL_01f8: ldstr "movss dword ptr [rdi+rcx*4+20h],xmm0 ; value = LocalExpr"
		IL_01fd: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0202: ldstr "movss dword ptr [rdi+rcx*4+24h],xmm1 ; value = LocalExpr"
		IL_0207: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_020c: ldstr "movss dword ptr [rdi+rcx*4+28h],xmm2 ; value = LocalExpr"
		IL_0211: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0216: nop
		IL_0217: ldloc.s 8
		IL_0219: ldc.i8 1
		IL_0222: add
		IL_0223: stloc.s 24
		IL_0225: ldstr "*(LocalExpr + 0x20)"
		IL_022a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_022f: conv.i4
		IL_0230: conv.r4
		IL_0231: ldloc.s 22
		IL_0233: sub
		IL_0234: stloc.s 19
		IL_0236: ldstr "*(LocalExpr + 0x24)"
		IL_023b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0240: conv.i4
		IL_0241: conv.r4
		IL_0242: ldloc.s 23
		IL_0244: sub
		IL_0245: stloc.s 20
		IL_0247: ldstr "*(LocalExpr + 0x28)"
		IL_024c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0251: conv.i4
		IL_0252: conv.r4
		IL_0253: ldc.r4 0.0
		IL_0258: sub
		IL_0259: stloc.s 21
		IL_025b: nop
		IL_025c: ldstr "movss dword ptr [rdi+rax*4+20h],xmm0 ; value = LocalExpr"
		IL_0261: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0266: ldstr "movss dword ptr [rdi+rax*4+24h],xmm1 ; value = LocalExpr"
		IL_026b: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0270: ldstr "movss dword ptr [rdi+rax*4+28h],xmm2 ; value = LocalExpr"
		IL_0275: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_027a: nop
		IL_027b: ldloc.s 8
		IL_027d: ldc.i8 2
		IL_0286: add
		IL_0287: stloc.s 24
		IL_0289: ldstr "*(LocalExpr + 0x20)"
		IL_028e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0293: conv.i4
		IL_0294: conv.r4
		IL_0295: ldloc.s 22
		IL_0297: sub
		IL_0298: stloc.s 19
		IL_029a: ldstr "*(LocalExpr + 0x24)"
		IL_029f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_02a4: conv.i4
		IL_02a5: conv.r4
		IL_02a6: ldloc.s 23
		IL_02a8: sub
		IL_02a9: stloc.s 20
		IL_02ab: ldstr "*(LocalExpr + 0x28)"
		IL_02b0: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_02b5: conv.i4
		IL_02b6: conv.r4
		IL_02b7: ldc.r4 0.0
		IL_02bc: sub
		IL_02bd: stloc.s 21
		IL_02bf: nop
		IL_02c0: ldstr "movss dword ptr [rdi+rax*4+20h],xmm0 ; value = LocalExpr"
		IL_02c5: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_02ca: ldstr "movss dword ptr [rdi+rax*4+24h],xmm1 ; value = LocalExpr"
		IL_02cf: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_02d4: ldstr "movss dword ptr [rdi+rax*4+28h],xmm2 ; value = LocalExpr"
		IL_02d9: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_02de: nop
		IL_02df: ldloc.s 8
		IL_02e1: ldc.i8 3
		IL_02ea: add
		IL_02eb: stloc.s 24
		IL_02ed: nop
		IL_02ee: ret
	} // end of method TMP_TextSelector_B::LateUpdate

	.method public final hidebysig newslot virtual 
		instance void OnPointerEnter (
			class [UnityEngine.UI]UnityEngine.EventSystems.PointerEventData eventData
		) cil managed 
	{
		// Method begins at RVA 0x2a12
		// Header size: 1
		// Code size: 9 (0x9)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i4.1
		IL_0003: stfld bool TMPro.Examples.TMP_TextSelector_B::isHoveringObject
		IL_0008: ret
	} // end of method TMP_TextSelector_B::OnPointerEnter

	.method public final hidebysig newslot virtual 
		instance void OnPointerExit (
			class [UnityEngine.UI]UnityEngine.EventSystems.PointerEventData eventData
		) cil managed 
	{
		// Method begins at RVA 0x2a1c
		// Header size: 1
		// Code size: 9 (0x9)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i4.0
		IL_0003: stfld bool TMPro.Examples.TMP_TextSelector_B::isHoveringObject
		IL_0008: ret
	} // end of method TMP_TextSelector_B::OnPointerExit

	.method public final hidebysig newslot virtual 
		instance void OnPointerClick (
			class [UnityEngine.UI]UnityEngine.EventSystems.PointerEventData eventData
		) cil managed 
	{
		// Method begins at RVA 0x2a26
		// Header size: 1
		// Code size: 2 (0x2)
		.maxstack 8

		IL_0000: nop
		IL_0001: ret
	} // end of method TMP_TextSelector_B::OnPointerClick

	.method public final hidebysig newslot virtual 
		instance void OnPointerUp (
			class [UnityEngine.UI]UnityEngine.EventSystems.PointerEventData eventData
		) cil managed 
	{
		// Method begins at RVA 0x2a29
		// Header size: 1
		// Code size: 2 (0x2)
		.maxstack 8

		IL_0000: nop
		IL_0001: ret
	} // end of method TMP_TextSelector_B::OnPointerUp

	.method private hidebysig 
		instance void RestoreCachedVertexAttributes (
			int32 index
		) cil managed 
	{
		// Method begins at RVA 0x186b8
		// Header size: 12
		// Code size: 2517 (0x9d5)
		.maxstack 5
		.locals init (
			[0] int64,
			[1] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[2] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
			[3] int32,
			[4] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[5] valuetype [Unity.TextMeshPro]TMPro.TMP_CharacterInfo[],
			[6] int64,
			[7] int64,
			[8] int32,
			[9] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[10] int32,
			[11] int64,
			[12] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[13] int64,
			[14] int32,
			[15] int64,
			[16] int64,
			[17] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[18] int32,
			[19] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[20] int64,
			[21] int32,
			[22] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[23] int64
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: conv.i8
		IL_0003: stloc.0
		IL_0004: ldarg.1
		IL_0005: conv.i8
		IL_0006: conv.i4
		IL_0007: ldc.i4.m1
		IL_0008: ceq
		IL_000a: brtrue IL_09cd

		IL_000f: nop
		IL_0010: ldarg.0
		IL_0011: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.TMP_TextSelector_B::m_TextMeshPro
		IL_0016: stloc.1
		IL_0017: ldloc.1
		IL_0018: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo [Unity.TextMeshPro]TMPro.TMP_Text::m_textInfo
		IL_001d: stloc.2
		IL_001e: ldloc.2
		IL_001f: ldfld int32 [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterCount
		IL_0024: ldc.i4.1
		IL_0025: sub
		IL_0026: stloc.3
		IL_0027: ldloc.0
		IL_0028: conv.i4
		IL_0029: ldloc.2
		IL_002a: ldfld int32 [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterCount
		IL_002f: ldc.i4.1
		IL_0030: sub
		IL_0031: cgt
		IL_0033: brtrue IL_09cc

		IL_0038: nop
		IL_0039: ldarg.0
		IL_003a: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.TMP_TextSelector_B::m_TextMeshPro
		IL_003f: stloc.s 4
		IL_0041: ldloc.s 4
		IL_0043: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo [Unity.TextMeshPro]TMPro.TMP_Text::m_textInfo
		IL_0048: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_CharacterInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::characterInfo
		IL_004d: stloc.s 5
		IL_004f: ldstr "movsd qword ptr [r11+rdx*4+20h],xmm0 ; value = *(*(FieldExpr + 0x30) + 0x20)"
		IL_0054: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0059: ldstr "mov [r11+rdx*4+28h],r8d ; value = *(*(FieldExpr + 0x30) + 0x28)"
		IL_005e: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0063: ldstr "*(*(FieldExpr + 0x30) + 0x20)"
		IL_0068: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_006d: stloc.s 6
		IL_006f: ldstr "movsd qword ptr [r11+rcx*4+20h],xmm0 ; value = *(*(FieldExpr + 0x30) + 0x20)"
		IL_0074: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0079: ldstr "mov [r11+rcx*4+28h],edx ; value = *(*(FieldExpr + 0x30) + 0x28)"
		IL_007e: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0083: ldstr "*(BinaryExpr + 0x6C)"
		IL_0088: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_008d: conv.i4
		IL_008e: conv.i8
		IL_008f: ldc.i8 1
		IL_0098: add
		IL_0099: ldstr "*(BinaryExpr + 0x6C)"
		IL_009e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00a3: conv.i4
		IL_00a4: conv.i8
		IL_00a5: ldc.i8 1
		IL_00ae: add
		IL_00af: ldc.i8 2
		IL_00b8: mul
		IL_00b9: add
		IL_00ba: stloc.s 7
		IL_00bc: ldstr "*(*(FieldExpr + 0x30) + 0x20)"
		IL_00c1: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00c6: stloc.s 6
		IL_00c8: ldstr "movsd qword ptr [r11+rcx*4+20h],xmm0 ; value = *(*(FieldExpr + 0x30) + 0x20)"
		IL_00cd: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_00d2: ldstr "mov [r11+rcx*4+28h],edx ; value = *(*(FieldExpr + 0x30) + 0x28)"
		IL_00d7: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_00dc: ldstr "*(BinaryExpr + 0x6C)"
		IL_00e1: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00e6: conv.i4
		IL_00e7: conv.i8
		IL_00e8: ldc.i8 2
		IL_00f1: add
		IL_00f2: ldstr "*(BinaryExpr + 0x6C)"
		IL_00f7: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00fc: conv.i4
		IL_00fd: conv.i8
		IL_00fe: ldc.i8 2
		IL_0107: add
		IL_0108: ldc.i8 2
		IL_0111: mul
		IL_0112: add
		IL_0113: stloc.s 7
		IL_0115: ldstr "*(*(FieldExpr + 0x30) + 0x20)"
		IL_011a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_011f: stloc.s 6
		IL_0121: ldstr "movsd qword ptr [r11+rcx*4+20h],xmm0 ; value = *(*(FieldExpr + 0x30) + 0x20)"
		IL_0126: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_012b: ldstr "mov [r11+rcx*4+28h],edx ; value = *(*(FieldExpr + 0x30) + 0x28)"
		IL_0130: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0135: ldstr "*(BinaryExpr + 0x6C)"
		IL_013a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_013f: conv.i4
		IL_0140: conv.i8
		IL_0141: ldc.i8 3
		IL_014a: add
		IL_014b: ldstr "*(BinaryExpr + 0x6C)"
		IL_0150: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0155: conv.i4
		IL_0156: conv.i8
		IL_0157: ldc.i8 3
		IL_0160: add
		IL_0161: ldc.i8 2
		IL_016a: mul
		IL_016b: add
		IL_016c: stloc.s 7
		IL_016e: ldstr "*(*(FieldExpr + 0x30) + 0x20)"
		IL_0173: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0178: stloc.s 6
		IL_017a: ldstr "mov [rdx+r9*4+20h],eax ; value = *(*(FieldExpr + 0x58) + 0x20)"
		IL_017f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0184: ldstr "*(*(FieldExpr + 0x58) + 0x20)"
		IL_0189: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_018e: conv.i4
		IL_018f: stloc.s 8
		IL_0191: ldstr "mov [rdx+rcx*4+20h],eax ; value = *(*(FieldExpr + 0x58) + 0x20)"
		IL_0196: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_019b: ldstr "*(*(FieldExpr + 0x58) + 0x20)"
		IL_01a0: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01a5: conv.i4
		IL_01a6: stloc.s 8
		IL_01a8: ldstr "mov [rdx+rcx*4+20h],eax ; value = *(*(FieldExpr + 0x58) + 0x20)"
		IL_01ad: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_01b2: ldstr "*(*(FieldExpr + 0x58) + 0x20)"
		IL_01b7: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01bc: conv.i4
		IL_01bd: stloc.s 8
		IL_01bf: ldstr "mov [rdx+rcx*4+20h],eax ; value = *(*(FieldExpr + 0x58) + 0x20)"
		IL_01c4: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_01c9: ldstr "*(*(FieldExpr + 0x58) + 0x20)"
		IL_01ce: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01d3: conv.i4
		IL_01d4: stloc.s 8
		IL_01d6: ldarg.0
		IL_01d7: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.TMP_TextSelector_B::m_TextMeshPro
		IL_01dc: stloc.s 9
		IL_01de: ldstr "movss dword ptr [rdx+r9*8+20h],xmm0 ; value = *(*(FieldExpr + 0x48) + 0x20)"
		IL_01e3: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_01e8: ldstr "movss dword ptr [rdx+r9*8+24h],xmm1 ; value = *(*(FieldExpr + 0x48) + 0x24)"
		IL_01ed: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_01f2: ldstr "*(*(FieldExpr + 0x48) + 0x20)"
		IL_01f7: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01fc: conv.i4
		IL_01fd: stloc.s 10
		IL_01ff: ldstr "*(BinaryExpr + 0x6C)"
		IL_0204: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0209: conv.i4
		IL_020a: conv.i8
		IL_020b: ldc.i8 1
		IL_0214: add
		IL_0215: stloc.s 11
		IL_0217: ldstr "movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x48) + 0x20)"
		IL_021c: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0221: ldstr "movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x48) + 0x24)"
		IL_0226: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_022b: ldstr "*(*(FieldExpr + 0x48) + 0x20)"
		IL_0230: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0235: conv.i4
		IL_0236: stloc.s 10
		IL_0238: ldstr "*(BinaryExpr + 0x6C)"
		IL_023d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0242: conv.i4
		IL_0243: conv.i8
		IL_0244: ldc.i8 2
		IL_024d: add
		IL_024e: stloc.s 11
		IL_0250: ldstr "movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x48) + 0x20)"
		IL_0255: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_025a: ldstr "movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x48) + 0x24)"
		IL_025f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0264: ldstr "*(*(FieldExpr + 0x48) + 0x20)"
		IL_0269: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_026e: conv.i4
		IL_026f: stloc.s 10
		IL_0271: ldstr "*(BinaryExpr + 0x6C)"
		IL_0276: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_027b: conv.i4
		IL_027c: conv.i8
		IL_027d: ldc.i8 3
		IL_0286: add
		IL_0287: stloc.s 11
		IL_0289: ldstr "movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x48) + 0x20)"
		IL_028e: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0293: ldstr "movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x48) + 0x24)"
		IL_0298: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_029d: ldstr "*(*(FieldExpr + 0x48) + 0x20)"
		IL_02a2: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_02a7: conv.i4
		IL_02a8: stloc.s 10
		IL_02aa: ldarg.0
		IL_02ab: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.TMP_TextSelector_B::m_TextMeshPro
		IL_02b0: stloc.s 12
		IL_02b2: ldstr "movss dword ptr [rdx+r9*8+20h],xmm0 ; value = *(*(FieldExpr + 0x50) + 0x20)"
		IL_02b7: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_02bc: ldstr "movss dword ptr [rdx+r9*8+24h],xmm1 ; value = *(*(FieldExpr + 0x50) + 0x24)"
		IL_02c1: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_02c6: ldstr "*(*(FieldExpr + 0x50) + 0x20)"
		IL_02cb: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_02d0: conv.i4
		IL_02d1: stloc.s 10
		IL_02d3: ldstr "*(BinaryExpr + 0x6C)"
		IL_02d8: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_02dd: conv.i4
		IL_02de: conv.i8
		IL_02df: ldc.i8 1
		IL_02e8: add
		IL_02e9: stloc.s 13
		IL_02eb: ldstr "movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x50) + 0x20)"
		IL_02f0: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_02f5: ldstr "movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x50) + 0x24)"
		IL_02fa: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_02ff: ldstr "*(*(FieldExpr + 0x50) + 0x20)"
		IL_0304: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0309: conv.i4
		IL_030a: stloc.s 10
		IL_030c: ldstr "*(BinaryExpr + 0x6C)"
		IL_0311: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0316: conv.i4
		IL_0317: conv.i8
		IL_0318: ldc.i8 2
		IL_0321: add
		IL_0322: stloc.s 13
		IL_0324: ldstr "movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x50) + 0x20)"
		IL_0329: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_032e: ldstr "movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x50) + 0x24)"
		IL_0333: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0338: ldstr "*(*(FieldExpr + 0x50) + 0x20)"
		IL_033d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0342: conv.i4
		IL_0343: stloc.s 10
		IL_0345: ldstr "*(BinaryExpr + 0x6C)"
		IL_034a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_034f: conv.i4
		IL_0350: conv.i8
		IL_0351: ldc.i8 3
		IL_035a: add
		IL_035b: stloc.s 13
		IL_035d: ldstr "movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x50) + 0x20)"
		IL_0362: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0367: ldstr "movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x50) + 0x24)"
		IL_036c: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0371: ldstr "*(*(FieldExpr + 0x30) + 0x18)"
		IL_0376: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_037b: conv.i4
		IL_037c: ldstr "*(FieldExpr + 0x50)"
		IL_0381: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0386: conv.i4
		IL_0387: ldc.i4.3
		IL_0388: and
		IL_0389: add
		IL_038a: ldc.i4.2
		IL_038b: shr
		IL_038c: stloc.s 14
		IL_038e: ldstr "*(*(FieldExpr + 0x50) + 0x20)"
		IL_0393: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0398: conv.i4
		IL_0399: stloc.s 10
		IL_039b: ldstr "movsd qword ptr [r11+rdx*4+20h],xmm0 ; value = *(*(FieldExpr + 0x30) + 0x20)"
		IL_03a0: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_03a5: ldstr "mov [r11+rdx*4+28h],r8d ; value = *(*(FieldExpr + 0x30) + 0x28)"
		IL_03aa: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_03af: ldc.i8 0
		IL_03b8: ldstr "*(*(FieldExpr + 0x30) + 0x18)"
		IL_03bd: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_03c2: conv.i4
		IL_03c3: ldstr "*(FieldExpr + 0x50)"
		IL_03c8: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_03cd: conv.i4
		IL_03ce: ldc.i4.3
		IL_03cf: and
		IL_03d0: add
		IL_03d1: ldc.i4.2
		IL_03d2: shr
		IL_03d3: conv.i8
		IL_03d4: ldc.i8 4
		IL_03dd: mul
		IL_03de: add
		IL_03df: ldc.i8 -4
		IL_03e8: add
		IL_03e9: conv.i4
		IL_03ea: conv.i8
		IL_03eb: stloc.s 15
		IL_03ed: ldstr "*(*(FieldExpr + 0x30) + 0x20)"
		IL_03f2: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_03f7: stloc.s 16
		IL_03f9: ldstr "movsd qword ptr [r11+rcx*4+20h],xmm0 ; value = *(*(FieldExpr + 0x30) + 0x20)"
		IL_03fe: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0403: ldstr "mov [r11+rcx*4+28h],edx ; value = *(*(FieldExpr + 0x30) + 0x28)"
		IL_0408: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_040d: ldc.i8 0
		IL_0416: ldstr "*(*(FieldExpr + 0x30) + 0x18)"
		IL_041b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0420: conv.i4
		IL_0421: ldstr "*(FieldExpr + 0x50)"
		IL_0426: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_042b: conv.i4
		IL_042c: ldc.i4.3
		IL_042d: and
		IL_042e: add
		IL_042f: ldc.i4.2
		IL_0430: shr
		IL_0431: conv.i8
		IL_0432: ldc.i8 4
		IL_043b: mul
		IL_043c: add
		IL_043d: ldc.i8 -4
		IL_0446: add
		IL_0447: conv.i4
		IL_0448: conv.i8
		IL_0449: ldc.i8 1
		IL_0452: add
		IL_0453: ldc.i8 0
		IL_045c: ldstr "*(*(FieldExpr + 0x30) + 0x18)"
		IL_0461: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0466: conv.i4
		IL_0467: ldstr "*(FieldExpr + 0x50)"
		IL_046c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0471: conv.i4
		IL_0472: ldc.i4.3
		IL_0473: and
		IL_0474: add
		IL_0475: ldc.i4.2
		IL_0476: shr
		IL_0477: conv.i8
		IL_0478: ldc.i8 4
		IL_0481: mul
		IL_0482: add
		IL_0483: ldc.i8 -4
		IL_048c: add
		IL_048d: conv.i4
		IL_048e: conv.i8
		IL_048f: ldc.i8 1
		IL_0498: add
		IL_0499: ldc.i8 2
		IL_04a2: mul
		IL_04a3: add
		IL_04a4: stloc.s 15
		IL_04a6: ldstr "*(*(FieldExpr + 0x30) + 0x20)"
		IL_04ab: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_04b0: stloc.s 16
		IL_04b2: ldstr "movsd qword ptr [r11+rcx*4+20h],xmm0 ; value = *(*(FieldExpr + 0x30) + 0x20)"
		IL_04b7: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_04bc: ldstr "mov [r11+rcx*4+28h],edx ; value = *(*(FieldExpr + 0x30) + 0x28)"
		IL_04c1: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_04c6: ldc.i8 0
		IL_04cf: ldstr "*(*(FieldExpr + 0x30) + 0x18)"
		IL_04d4: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_04d9: conv.i4
		IL_04da: ldstr "*(FieldExpr + 0x50)"
		IL_04df: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_04e4: conv.i4
		IL_04e5: ldc.i4.3
		IL_04e6: and
		IL_04e7: add
		IL_04e8: ldc.i4.2
		IL_04e9: shr
		IL_04ea: conv.i8
		IL_04eb: ldc.i8 4
		IL_04f4: mul
		IL_04f5: add
		IL_04f6: ldc.i8 -4
		IL_04ff: add
		IL_0500: conv.i4
		IL_0501: conv.i8
		IL_0502: ldc.i8 2
		IL_050b: add
		IL_050c: ldc.i8 0
		IL_0515: ldstr "*(*(FieldExpr + 0x30) + 0x18)"
		IL_051a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_051f: conv.i4
		IL_0520: ldstr "*(FieldExpr + 0x50)"
		IL_0525: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_052a: conv.i4
		IL_052b: ldc.i4.3
		IL_052c: and
		IL_052d: add
		IL_052e: ldc.i4.2
		IL_052f: shr
		IL_0530: conv.i8
		IL_0531: ldc.i8 4
		IL_053a: mul
		IL_053b: add
		IL_053c: ldc.i8 -4
		IL_0545: add
		IL_0546: conv.i4
		IL_0547: conv.i8
		IL_0548: ldc.i8 2
		IL_0551: add
		IL_0552: ldc.i8 2
		IL_055b: mul
		IL_055c: add
		IL_055d: stloc.s 15
		IL_055f: ldstr "*(*(FieldExpr + 0x30) + 0x20)"
		IL_0564: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0569: stloc.s 16
		IL_056b: ldstr "movsd qword ptr [r11+rcx*4+20h],xmm0 ; value = *(*(FieldExpr + 0x30) + 0x20)"
		IL_0570: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0575: ldstr "mov [r11+rcx*4+28h],edx ; value = *(*(FieldExpr + 0x30) + 0x28)"
		IL_057a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_057f: ldc.i8 0
		IL_0588: ldstr "*(*(FieldExpr + 0x30) + 0x18)"
		IL_058d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0592: conv.i4
		IL_0593: ldstr "*(FieldExpr + 0x50)"
		IL_0598: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_059d: conv.i4
		IL_059e: ldc.i4.3
		IL_059f: and
		IL_05a0: add
		IL_05a1: ldc.i4.2
		IL_05a2: shr
		IL_05a3: conv.i8
		IL_05a4: ldc.i8 4
		IL_05ad: mul
		IL_05ae: add
		IL_05af: ldc.i8 -4
		IL_05b8: add
		IL_05b9: conv.i4
		IL_05ba: conv.i8
		IL_05bb: ldc.i8 3
		IL_05c4: add
		IL_05c5: ldc.i8 0
		IL_05ce: ldstr "*(*(FieldExpr + 0x30) + 0x18)"
		IL_05d3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_05d8: conv.i4
		IL_05d9: ldstr "*(FieldExpr + 0x50)"
		IL_05de: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_05e3: conv.i4
		IL_05e4: ldc.i4.3
		IL_05e5: and
		IL_05e6: add
		IL_05e7: ldc.i4.2
		IL_05e8: shr
		IL_05e9: conv.i8
		IL_05ea: ldc.i8 4
		IL_05f3: mul
		IL_05f4: add
		IL_05f5: ldc.i8 -4
		IL_05fe: add
		IL_05ff: conv.i4
		IL_0600: conv.i8
		IL_0601: ldc.i8 3
		IL_060a: add
		IL_060b: ldc.i8 2
		IL_0614: mul
		IL_0615: add
		IL_0616: stloc.s 15
		IL_0618: ldstr "*(*(FieldExpr + 0x30) + 0x20)"
		IL_061d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0622: stloc.s 16
		IL_0624: ldarg.0
		IL_0625: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.TMP_TextSelector_B::m_TextMeshPro
		IL_062a: stloc.s 17
		IL_062c: ldstr "mov [rdx+rcx*4+20h],eax ; value = *(*(FieldExpr + 0x58) + 0x20)"
		IL_0631: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0636: ldstr "*(*(FieldExpr + 0x58) + 0x20)"
		IL_063b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0640: conv.i4
		IL_0641: stloc.s 18
		IL_0643: ldstr "mov [rdx+rcx*4+20h],eax ; value = *(*(FieldExpr + 0x58) + 0x20)"
		IL_0648: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_064d: ldstr "*(*(FieldExpr + 0x58) + 0x20)"
		IL_0652: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0657: conv.i4
		IL_0658: stloc.s 18
		IL_065a: ldstr "mov [rdx+rcx*4+20h],eax ; value = *(*(FieldExpr + 0x58) + 0x20)"
		IL_065f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0664: ldstr "*(*(FieldExpr + 0x58) + 0x20)"
		IL_0669: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_066e: conv.i4
		IL_066f: stloc.s 18
		IL_0671: ldstr "mov [rdx+rcx*4+20h],eax ; value = *(*(FieldExpr + 0x58) + 0x20)"
		IL_0676: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_067b: ldstr "*(*(FieldExpr + 0x58) + 0x20)"
		IL_0680: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0685: conv.i4
		IL_0686: stloc.s 18
		IL_0688: ldarg.0
		IL_0689: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.TMP_TextSelector_B::m_TextMeshPro
		IL_068e: stloc.s 19
		IL_0690: ldstr "movss dword ptr [rdx+rcx*8+20h],xmm0 ; value = *(*(FieldExpr + 0x48) + 0x20)"
		IL_0695: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_069a: ldstr "movss dword ptr [rdx+rcx*8+24h],xmm1 ; value = *(*(FieldExpr + 0x48) + 0x24)"
		IL_069f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_06a4: ldc.i8 0
		IL_06ad: ldstr "*(*(FieldExpr + 0x30) + 0x18)"
		IL_06b2: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_06b7: conv.i4
		IL_06b8: ldstr "*(FieldExpr + 0x50)"
		IL_06bd: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_06c2: conv.i4
		IL_06c3: ldc.i4.3
		IL_06c4: and
		IL_06c5: add
		IL_06c6: ldc.i4.2
		IL_06c7: shr
		IL_06c8: conv.i8
		IL_06c9: ldc.i8 4
		IL_06d2: mul
		IL_06d3: add
		IL_06d4: ldc.i8 -4
		IL_06dd: add
		IL_06de: conv.i4
		IL_06df: conv.i8
		IL_06e0: stloc.s 20
		IL_06e2: ldstr "*(*(FieldExpr + 0x48) + 0x20)"
		IL_06e7: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_06ec: conv.i4
		IL_06ed: stloc.s 21
		IL_06ef: ldc.i8 0
		IL_06f8: ldstr "*(*(FieldExpr + 0x30) + 0x18)"
		IL_06fd: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0702: conv.i4
		IL_0703: ldstr "*(FieldExpr + 0x50)"
		IL_0708: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_070d: conv.i4
		IL_070e: ldc.i4.3
		IL_070f: and
		IL_0710: add
		IL_0711: ldc.i4.2
		IL_0712: shr
		IL_0713: conv.i8
		IL_0714: ldc.i8 4
		IL_071d: mul
		IL_071e: add
		IL_071f: ldc.i8 -4
		IL_0728: add
		IL_0729: conv.i4
		IL_072a: conv.i8
		IL_072b: ldc.i8 1
		IL_0734: add
		IL_0735: stloc.s 20
		IL_0737: ldstr "movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x48) + 0x20)"
		IL_073c: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0741: ldstr "movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x48) + 0x24)"
		IL_0746: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_074b: ldstr "*(*(FieldExpr + 0x48) + 0x20)"
		IL_0750: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0755: conv.i4
		IL_0756: stloc.s 21
		IL_0758: ldc.i8 0
		IL_0761: ldstr "*(*(FieldExpr + 0x30) + 0x18)"
		IL_0766: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_076b: conv.i4
		IL_076c: ldstr "*(FieldExpr + 0x50)"
		IL_0771: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0776: conv.i4
		IL_0777: ldc.i4.3
		IL_0778: and
		IL_0779: add
		IL_077a: ldc.i4.2
		IL_077b: shr
		IL_077c: conv.i8
		IL_077d: ldc.i8 4
		IL_0786: mul
		IL_0787: add
		IL_0788: ldc.i8 -4
		IL_0791: add
		IL_0792: conv.i4
		IL_0793: conv.i8
		IL_0794: ldc.i8 2
		IL_079d: add
		IL_079e: stloc.s 20
		IL_07a0: ldstr "movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x48) + 0x20)"
		IL_07a5: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_07aa: ldstr "movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x48) + 0x24)"
		IL_07af: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_07b4: ldstr "*(*(FieldExpr + 0x48) + 0x20)"
		IL_07b9: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_07be: conv.i4
		IL_07bf: stloc.s 21
		IL_07c1: ldc.i8 0
		IL_07ca: ldstr "*(*(FieldExpr + 0x30) + 0x18)"
		IL_07cf: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_07d4: conv.i4
		IL_07d5: ldstr "*(FieldExpr + 0x50)"
		IL_07da: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_07df: conv.i4
		IL_07e0: ldc.i4.3
		IL_07e1: and
		IL_07e2: add
		IL_07e3: ldc.i4.2
		IL_07e4: shr
		IL_07e5: conv.i8
		IL_07e6: ldc.i8 4
		IL_07ef: mul
		IL_07f0: add
		IL_07f1: ldc.i8 -4
		IL_07fa: add
		IL_07fb: conv.i4
		IL_07fc: conv.i8
		IL_07fd: ldc.i8 3
		IL_0806: add
		IL_0807: stloc.s 20
		IL_0809: ldstr "movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x48) + 0x20)"
		IL_080e: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0813: ldstr "movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x48) + 0x24)"
		IL_0818: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_081d: ldstr "*(*(FieldExpr + 0x48) + 0x20)"
		IL_0822: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0827: conv.i4
		IL_0828: stloc.s 21
		IL_082a: ldarg.0
		IL_082b: ldfld class [Unity.TextMeshPro]TMPro.TextMeshProUGUI TMPro.Examples.TMP_TextSelector_B::m_TextMeshPro
		IL_0830: stloc.s 22
		IL_0832: ldstr "movss dword ptr [rdx+rcx*8+20h],xmm0 ; value = *(*(FieldExpr + 0x50) + 0x20)"
		IL_0837: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_083c: ldstr "movss dword ptr [rdx+rcx*8+24h],xmm1 ; value = *(*(FieldExpr + 0x50) + 0x24)"
		IL_0841: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0846: ldc.i8 0
		IL_084f: ldstr "*(*(FieldExpr + 0x30) + 0x18)"
		IL_0854: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0859: conv.i4
		IL_085a: ldstr "*(FieldExpr + 0x50)"
		IL_085f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0864: conv.i4
		IL_0865: ldc.i4.3
		IL_0866: and
		IL_0867: add
		IL_0868: ldc.i4.2
		IL_0869: shr
		IL_086a: conv.i8
		IL_086b: ldc.i8 4
		IL_0874: mul
		IL_0875: add
		IL_0876: ldc.i8 -4
		IL_087f: add
		IL_0880: conv.i4
		IL_0881: conv.i8
		IL_0882: stloc.s 23
		IL_0884: ldstr "*(*(FieldExpr + 0x50) + 0x20)"
		IL_0889: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_088e: conv.i4
		IL_088f: stloc.s 21
		IL_0891: ldc.i8 0
		IL_089a: ldstr "*(*(FieldExpr + 0x30) + 0x18)"
		IL_089f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_08a4: conv.i4
		IL_08a5: ldstr "*(FieldExpr + 0x50)"
		IL_08aa: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_08af: conv.i4
		IL_08b0: ldc.i4.3
		IL_08b1: and
		IL_08b2: add
		IL_08b3: ldc.i4.2
		IL_08b4: shr
		IL_08b5: conv.i8
		IL_08b6: ldc.i8 4
		IL_08bf: mul
		IL_08c0: add
		IL_08c1: ldc.i8 -4
		IL_08ca: add
		IL_08cb: conv.i4
		IL_08cc: conv.i8
		IL_08cd: ldc.i8 1
		IL_08d6: add
		IL_08d7: stloc.s 23
		IL_08d9: ldstr "movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x50) + 0x20)"
		IL_08de: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_08e3: ldstr "movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x50) + 0x24)"
		IL_08e8: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_08ed: ldstr "*(*(FieldExpr + 0x50) + 0x20)"
		IL_08f2: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_08f7: conv.i4
		IL_08f8: stloc.s 21
		IL_08fa: ldc.i8 0
		IL_0903: ldstr "*(*(FieldExpr + 0x30) + 0x18)"
		IL_0908: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_090d: conv.i4
		IL_090e: ldstr "*(FieldExpr + 0x50)"
		IL_0913: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0918: conv.i4
		IL_0919: ldc.i4.3
		IL_091a: and
		IL_091b: add
		IL_091c: ldc.i4.2
		IL_091d: shr
		IL_091e: conv.i8
		IL_091f: ldc.i8 4
		IL_0928: mul
		IL_0929: add
		IL_092a: ldc.i8 -4
		IL_0933: add
		IL_0934: conv.i4
		IL_0935: conv.i8
		IL_0936: ldc.i8 2
		IL_093f: add
		IL_0940: stloc.s 23
		IL_0942: ldstr "movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x50) + 0x20)"
		IL_0947: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_094c: ldstr "movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x50) + 0x24)"
		IL_0951: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0956: ldstr "*(*(FieldExpr + 0x50) + 0x20)"
		IL_095b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0960: conv.i4
		IL_0961: stloc.s 21
		IL_0963: ldc.i8 0
		IL_096c: ldstr "*(*(FieldExpr + 0x30) + 0x18)"
		IL_0971: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0976: conv.i4
		IL_0977: ldstr "*(FieldExpr + 0x50)"
		IL_097c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0981: conv.i4
		IL_0982: ldc.i4.3
		IL_0983: and
		IL_0984: add
		IL_0985: ldc.i4.2
		IL_0986: shr
		IL_0987: conv.i8
		IL_0988: ldc.i8 4
		IL_0991: mul
		IL_0992: add
		IL_0993: ldc.i8 -4
		IL_099c: add
		IL_099d: conv.i4
		IL_099e: conv.i8
		IL_099f: ldc.i8 3
		IL_09a8: add
		IL_09a9: stloc.s 23
		IL_09ab: ldstr "movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x50) + 0x20)"
		IL_09b0: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_09b5: ldstr "movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x50) + 0x24)"
		IL_09ba: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_09bf: ldstr "*(*(FieldExpr + 0x50) + 0x20)"
		IL_09c4: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_09c9: conv.i4
		IL_09ca: stloc.s 21

		IL_09cc: nop

		IL_09cd: nop
		IL_09ce: ret

		IL_09cf: nop
		IL_09d0: ldnull
		IL_09d1: throw

		IL_09d2: nop
		IL_09d3: ldnull
		IL_09d4: throw
	} // end of method TMP_TextSelector_B::RestoreCachedVertexAttributes

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2a2c
		// Header size: 1
		// Code size: 31 (0x1f)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i8 -1
		IL_000b: conv.i4
		IL_000c: stfld int32 TMPro.Examples.TMP_TextSelector_B::m_selectedWord
		IL_0011: ldarg.0
		IL_0012: ldc.i4.m1
		IL_0013: stfld int32 TMPro.Examples.TMP_TextSelector_B::m_lastIndex
		IL_0018: ldarg.0
		IL_0019: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_001e: ret
	} // end of method TMP_TextSelector_B::.ctor

} // end of class TMPro.Examples.TMP_TextSelector_B
