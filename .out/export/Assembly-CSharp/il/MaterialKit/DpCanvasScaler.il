.class public auto ansi beforefieldinit MaterialKit.DpCanvasScaler
	extends [UnityEngine.UI]UnityEngine.EventSystems.UIBehaviour
{
	// Fields
	.field family float32 m_ReferencePixelsPerUnit
	.field private static literal float32 kLogBase
	.field family float32 m_FallbackScreenDPI
	.field family float32 m_DefaultSpriteDPI
	.field family float32 m_DynamicPixelsPerUnit
	.field private class [UnityEngine.UIModule]UnityEngine.Canvas m_Canvas
	.field private notserialized float32 m_PrevScaleFactor
	.field private notserialized float32 m_PrevReferencePixelsPerUnit

	// Methods
	.method family hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x19d00
		// Header size: 12
		// Code size: 74 (0x4a)
		.maxstack 2

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.r4 100
		IL_0007: stfld float32 MaterialKit.DpCanvasScaler::m_ReferencePixelsPerUnit
		IL_000c: ldarg.0
		IL_000d: ldc.r4 96
		IL_0012: stfld float32 MaterialKit.DpCanvasScaler::m_FallbackScreenDPI
		IL_0017: ldarg.0
		IL_0018: ldc.r4 96
		IL_001d: stfld float32 MaterialKit.DpCanvasScaler::m_DefaultSpriteDPI
		IL_0022: ldarg.0
		IL_0023: ldc.r4 1
		IL_0028: stfld float32 MaterialKit.DpCanvasScaler::m_DynamicPixelsPerUnit
		IL_002d: ldarg.0
		IL_002e: ldc.r4 1
		IL_0033: stfld float32 MaterialKit.DpCanvasScaler::m_PrevScaleFactor
		IL_0038: ldarg.0
		IL_0039: ldc.r4 100
		IL_003e: stfld float32 MaterialKit.DpCanvasScaler::m_PrevReferencePixelsPerUnit
		IL_0043: ldarg.0
		IL_0044: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Dropdown/DropdownItem::.ctor()
		IL_0049: ret
	} // end of method DpCanvasScaler::.ctor

	.method public hidebysig specialname 
		instance float32 get_referencePixelsPerUnit () cil managed 
	{
		// Method begins at RVA 0x2b2d
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld float32 MaterialKit.DpCanvasScaler::m_ReferencePixelsPerUnit
		IL_0007: ret
	} // end of method DpCanvasScaler::get_referencePixelsPerUnit

	.method public hidebysig specialname 
		instance void set_referencePixelsPerUnit (
			float32 'value'
		) cil managed 
	{
		// Method begins at RVA 0x2b36
		// Header size: 1
		// Code size: 9 (0x9)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldarg.1
		IL_0003: stfld float32 MaterialKit.DpCanvasScaler::m_ReferencePixelsPerUnit
		IL_0008: ret
	} // end of method DpCanvasScaler::set_referencePixelsPerUnit

	.method public hidebysig specialname 
		instance float32 get_fallbackScreenDPI () cil managed 
	{
		// Method begins at RVA 0x2b40
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld float32 MaterialKit.DpCanvasScaler::m_FallbackScreenDPI
		IL_0007: ret
	} // end of method DpCanvasScaler::get_fallbackScreenDPI

	.method public hidebysig specialname 
		instance void set_fallbackScreenDPI (
			float32 'value'
		) cil managed 
	{
		// Method begins at RVA 0x2b49
		// Header size: 1
		// Code size: 9 (0x9)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldarg.1
		IL_0003: stfld float32 MaterialKit.DpCanvasScaler::m_FallbackScreenDPI
		IL_0008: ret
	} // end of method DpCanvasScaler::set_fallbackScreenDPI

	.method public hidebysig specialname 
		instance float32 get_defaultSpriteDPI () cil managed 
	{
		// Method begins at RVA 0x2b53
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld float32 MaterialKit.DpCanvasScaler::m_DefaultSpriteDPI
		IL_0007: ret
	} // end of method DpCanvasScaler::get_defaultSpriteDPI

	.method public hidebysig specialname 
		instance void set_defaultSpriteDPI (
			float32 'value'
		) cil managed 
	{
		// Method begins at RVA 0x2b5c
		// Header size: 1
		// Code size: 9 (0x9)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldarg.1
		IL_0003: stfld float32 MaterialKit.DpCanvasScaler::m_DefaultSpriteDPI
		IL_0008: ret
	} // end of method DpCanvasScaler::set_defaultSpriteDPI

	.method public hidebysig specialname 
		instance float32 get_dynamicPixelsPerUnit () cil managed 
	{
		// Method begins at RVA 0x2b66
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld float32 MaterialKit.DpCanvasScaler::m_DynamicPixelsPerUnit
		IL_0007: ret
	} // end of method DpCanvasScaler::get_dynamicPixelsPerUnit

	.method public hidebysig specialname 
		instance void set_dynamicPixelsPerUnit (
			float32 'value'
		) cil managed 
	{
		// Method begins at RVA 0x2b6f
		// Header size: 1
		// Code size: 9 (0x9)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldarg.1
		IL_0003: stfld float32 MaterialKit.DpCanvasScaler::m_DynamicPixelsPerUnit
		IL_0008: ret
	} // end of method DpCanvasScaler::set_dynamicPixelsPerUnit

	.method family hidebysig virtual 
		instance void OnEnable () cil managed 
	{
		// Method begins at RVA 0x19d58
		// Header size: 12
		// Code size: 16 (0x10)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.UIModule]UnityEngine.Canvas
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.UIModule]UnityEngine.Canvas>()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.UIModule]UnityEngine.Canvas MaterialKit.DpCanvasScaler::m_Canvas
		IL_000f: ret
	} // end of method DpCanvasScaler::OnEnable

	.method family hidebysig virtual 
		instance void OnDisable () cil managed 
	{
		// Method begins at RVA 0x19d74
		// Header size: 12
		// Code size: 94 (0x5e)
		.maxstack 2

		IL_0000: nop
		IL_0001: nop
		IL_0002: ldc.r4 1
		IL_0007: ldarg.0
		IL_0008: ldfld float32 MaterialKit.DpCanvasScaler::m_PrevScaleFactor
		IL_000d: ceq
		IL_000f: brtrue.s IL_002d

		IL_0011: nop
		IL_0012: ldarg.0
		IL_0013: ldfld class [UnityEngine.UIModule]UnityEngine.Canvas MaterialKit.DpCanvasScaler::m_Canvas
		IL_0018: ldc.r4 1
		IL_001d: callvirt instance void [UnityEngine.UIModule]UnityEngine.Canvas::set_scaleFactor(float32)
		IL_0022: ldarg.0
		IL_0023: ldc.r4 1
		IL_0028: stfld float32 MaterialKit.DpCanvasScaler::m_PrevScaleFactor

		IL_002d: nop
		IL_002e: nop
		IL_002f: ldc.r4 100
		IL_0034: ldarg.0
		IL_0035: ldfld float32 MaterialKit.DpCanvasScaler::m_PrevReferencePixelsPerUnit
		IL_003a: ceq
		IL_003c: brtrue.s IL_005a

		IL_003e: nop
		IL_003f: ldarg.0
		IL_0040: ldfld class [UnityEngine.UIModule]UnityEngine.Canvas MaterialKit.DpCanvasScaler::m_Canvas
		IL_0045: ldc.r4 100
		IL_004a: callvirt instance void [UnityEngine.UIModule]UnityEngine.Canvas::set_referencePixelsPerUnit(float32)
		IL_004f: ldarg.0
		IL_0050: ldc.r4 100
		IL_0055: stfld float32 MaterialKit.DpCanvasScaler::m_PrevReferencePixelsPerUnit

		IL_005a: nop
		IL_005b: nop
		IL_005c: ldnull
		IL_005d: throw
	} // end of method DpCanvasScaler::OnDisable

	.method family hidebysig newslot virtual 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x2b79
		// Header size: 1
		// Code size: 2 (0x2)
		.maxstack 8

		IL_0000: nop
		IL_0001: ret
	} // end of method DpCanvasScaler::Update

	.method family hidebysig newslot virtual 
		instance void Handle () cil managed 
	{
		// Method begins at RVA 0x19de0
		// Header size: 12
		// Code size: 94 (0x5e)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.UIModule]UnityEngine.Canvas,
			[1] bool,
			[2] bool,
			[3] bool,
			[4] valuetype [UnityEngine.UIModule]UnityEngine.RenderMode,
			[5] valuetype [UnityEngine.UIModule]UnityEngine.RenderMode
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.UIModule]UnityEngine.Canvas MaterialKit.DpCanvasScaler::m_Canvas
		IL_0007: stloc.0
		IL_0008: br.s IL_000b

		IL_000a: nop

		IL_000b: nop
		IL_000c: ldloc.0
		IL_000d: ldnull
		IL_000e: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Equality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0013: stloc.1
		IL_0014: ldloc.1
		IL_0015: stloc.2
		IL_0016: ldloc.1
		IL_0017: ldc.i4.0
		IL_0018: ceq
		IL_001a: ldc.i4.0
		IL_001b: ceq
		IL_001d: brtrue.s IL_0058

		IL_001f: nop
		IL_0020: ldarg.0
		IL_0021: ldfld class [UnityEngine.UIModule]UnityEngine.Canvas MaterialKit.DpCanvasScaler::m_Canvas
		IL_0026: callvirt instance bool [UnityEngine.UIModule]UnityEngine.Canvas::get_isRootCanvas()
		IL_002b: stloc.3
		IL_002c: ldloc.3
		IL_002d: stloc.2
		IL_002e: ldloc.3
		IL_002f: ldc.i4.0
		IL_0030: ceq
		IL_0032: brtrue.s IL_0058

		IL_0034: nop
		IL_0035: ldarg.0
		IL_0036: ldfld class [UnityEngine.UIModule]UnityEngine.Canvas MaterialKit.DpCanvasScaler::m_Canvas
		IL_003b: callvirt instance valuetype [UnityEngine.UIModule]UnityEngine.RenderMode [UnityEngine.UIModule]UnityEngine.Canvas::get_renderMode()
		IL_0040: stloc.s 4
		IL_0042: ldloc.s 4
		IL_0044: stloc.s 5
		IL_0046: ldloc.s 4
		IL_0048: box [mscorlib]System.Object
		IL_004d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0052: ldc.i4.2
		IL_0053: ceq
		IL_0055: brtrue.s IL_005a

		IL_0057: nop

		IL_0058: nop
		IL_0059: ret

		IL_005a: nop
		IL_005b: nop
		IL_005c: ldnull
		IL_005d: throw
	} // end of method DpCanvasScaler::Handle

	.method family hidebysig newslot virtual 
		instance void HandleWorldCanvas () cil managed 
	{
		// Method begins at RVA 0x19e4c
		// Header size: 12
		// Code size: 95 (0x5f)
		.maxstack 2
		.locals init (
			[0] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld float32 MaterialKit.DpCanvasScaler::m_DynamicPixelsPerUnit
		IL_0007: stloc.0
		IL_0008: nop
		IL_0009: ldarg.0
		IL_000a: ldfld float32 MaterialKit.DpCanvasScaler::m_DynamicPixelsPerUnit
		IL_000f: ldarg.0
		IL_0010: ldfld float32 MaterialKit.DpCanvasScaler::m_PrevScaleFactor
		IL_0015: ceq
		IL_0017: brtrue.s IL_002d

		IL_0019: nop
		IL_001a: ldarg.0
		IL_001b: ldfld class [UnityEngine.UIModule]UnityEngine.Canvas MaterialKit.DpCanvasScaler::m_Canvas
		IL_0020: ldloc.0
		IL_0021: callvirt instance void [UnityEngine.UIModule]UnityEngine.Canvas::set_scaleFactor(float32)
		IL_0026: ldarg.0
		IL_0027: ldloc.0
		IL_0028: stfld float32 MaterialKit.DpCanvasScaler::m_PrevScaleFactor

		IL_002d: nop
		IL_002e: ldarg.0
		IL_002f: ldfld float32 MaterialKit.DpCanvasScaler::m_ReferencePixelsPerUnit
		IL_0034: stloc.0
		IL_0035: nop
		IL_0036: ldarg.0
		IL_0037: ldfld float32 MaterialKit.DpCanvasScaler::m_ReferencePixelsPerUnit
		IL_003c: ldarg.0
		IL_003d: ldfld float32 MaterialKit.DpCanvasScaler::m_PrevReferencePixelsPerUnit
		IL_0042: ceq
		IL_0044: brtrue.s IL_005a

		IL_0046: nop
		IL_0047: ldarg.0
		IL_0048: ldfld class [UnityEngine.UIModule]UnityEngine.Canvas MaterialKit.DpCanvasScaler::m_Canvas
		IL_004d: ldloc.0
		IL_004e: callvirt instance void [UnityEngine.UIModule]UnityEngine.Canvas::set_referencePixelsPerUnit(float32)
		IL_0053: ldarg.0
		IL_0054: ldloc.0
		IL_0055: stfld float32 MaterialKit.DpCanvasScaler::m_PrevReferencePixelsPerUnit

		IL_005a: nop
		IL_005b: ret

		IL_005c: nop
		IL_005d: ldnull
		IL_005e: throw
	} // end of method DpCanvasScaler::HandleWorldCanvas

	.method family hidebysig newslot virtual 
		instance void HandleConstantPhysicalSize () cil managed 
	{
		// Method begins at RVA 0x19eb8
		// Header size: 12
		// Code size: 156 (0x9c)
		.maxstack 2
		.locals init (
			[0] float32,
			[1] float32,
			[2] float32
		)

		IL_0000: nop
		IL_0001: call float32 [UnityEngine.CoreModule]UnityEngine.Screen::get_dpi()
		IL_0006: stloc.0
		IL_0007: ldloc.0
		IL_0008: stloc.1
		IL_0009: ldloc.0
		IL_000a: stloc.2
		IL_000b: nop
		IL_000c: ldloc.0
		IL_000d: ldc.r4 0.0
		IL_0012: ceq
		IL_0014: ldc.i4.0
		IL_0015: ceq
		IL_0017: brtrue.s IL_0021

		IL_0019: nop
		IL_001a: ldarg.0
		IL_001b: ldfld float32 MaterialKit.DpCanvasScaler::m_FallbackScreenDPI
		IL_0020: stloc.1

		IL_0021: nop
		IL_0022: ldloc.1
		IL_0023: ldc.r4 160
		IL_0028: div
		IL_0029: stloc.1
		IL_002a: nop
		IL_002b: ldloc.1
		IL_002c: ldc.r4 160
		IL_0031: div
		IL_0032: ldarg.0
		IL_0033: ldfld float32 MaterialKit.DpCanvasScaler::m_PrevScaleFactor
		IL_0038: ceq
		IL_003a: brtrue.s IL_0050

		IL_003c: nop
		IL_003d: ldarg.0
		IL_003e: ldfld class [UnityEngine.UIModule]UnityEngine.Canvas MaterialKit.DpCanvasScaler::m_Canvas
		IL_0043: ldloc.1
		IL_0044: callvirt instance void [UnityEngine.UIModule]UnityEngine.Canvas::set_scaleFactor(float32)
		IL_0049: ldarg.0
		IL_004a: ldloc.1
		IL_004b: stfld float32 MaterialKit.DpCanvasScaler::m_PrevScaleFactor

		IL_0050: nop
		IL_0051: ldarg.0
		IL_0052: ldfld float32 MaterialKit.DpCanvasScaler::m_ReferencePixelsPerUnit
		IL_0057: ldc.r4 160
		IL_005c: mul
		IL_005d: ldarg.0
		IL_005e: ldfld float32 MaterialKit.DpCanvasScaler::m_DefaultSpriteDPI
		IL_0063: div
		IL_0064: stloc.1
		IL_0065: nop
		IL_0066: ldarg.0
		IL_0067: ldfld float32 MaterialKit.DpCanvasScaler::m_ReferencePixelsPerUnit
		IL_006c: ldc.r4 160
		IL_0071: mul
		IL_0072: ldarg.0
		IL_0073: ldfld float32 MaterialKit.DpCanvasScaler::m_DefaultSpriteDPI
		IL_0078: div
		IL_0079: ldarg.0
		IL_007a: ldfld float32 MaterialKit.DpCanvasScaler::m_PrevReferencePixelsPerUnit
		IL_007f: ceq
		IL_0081: brtrue.s IL_0097

		IL_0083: nop
		IL_0084: ldarg.0
		IL_0085: ldfld class [UnityEngine.UIModule]UnityEngine.Canvas MaterialKit.DpCanvasScaler::m_Canvas
		IL_008a: ldloc.1
		IL_008b: callvirt instance void [UnityEngine.UIModule]UnityEngine.Canvas::set_referencePixelsPerUnit(float32)
		IL_0090: ldarg.0
		IL_0091: ldloc.1
		IL_0092: stfld float32 MaterialKit.DpCanvasScaler::m_PrevReferencePixelsPerUnit

		IL_0097: nop
		IL_0098: ret

		IL_0099: nop
		IL_009a: ldnull
		IL_009b: throw
	} // end of method DpCanvasScaler::HandleConstantPhysicalSize

	.method family hidebysig 
		instance void SetScaleFactor (
			float32 scaleFactor
		) cil managed 
	{
		// Method begins at RVA 0x2b7c
		// Header size: 1
		// Code size: 38 (0x26)
		.maxstack 8

		IL_0000: nop
		IL_0001: nop
		IL_0002: ldarg.1
		IL_0003: ldarg.0
		IL_0004: ldfld float32 MaterialKit.DpCanvasScaler::m_PrevScaleFactor
		IL_0009: ceq
		IL_000b: brtrue.s IL_0021

		IL_000d: nop
		IL_000e: ldarg.0
		IL_000f: ldfld class [UnityEngine.UIModule]UnityEngine.Canvas MaterialKit.DpCanvasScaler::m_Canvas
		IL_0014: ldarg.1
		IL_0015: callvirt instance void [UnityEngine.UIModule]UnityEngine.Canvas::set_scaleFactor(float32)
		IL_001a: ldarg.0
		IL_001b: ldarg.1
		IL_001c: stfld float32 MaterialKit.DpCanvasScaler::m_PrevScaleFactor

		IL_0021: nop
		IL_0022: ret

		IL_0023: nop
		IL_0024: ldnull
		IL_0025: throw
	} // end of method DpCanvasScaler::SetScaleFactor

	.method family hidebysig 
		instance void SetReferencePixelsPerUnit (
			float32 referencePixelsPerUnit
		) cil managed 
	{
		// Method begins at RVA 0x2ba3
		// Header size: 1
		// Code size: 38 (0x26)
		.maxstack 8

		IL_0000: nop
		IL_0001: nop
		IL_0002: ldarg.1
		IL_0003: ldarg.0
		IL_0004: ldfld float32 MaterialKit.DpCanvasScaler::m_PrevReferencePixelsPerUnit
		IL_0009: ceq
		IL_000b: brtrue.s IL_0021

		IL_000d: nop
		IL_000e: ldarg.0
		IL_000f: ldfld class [UnityEngine.UIModule]UnityEngine.Canvas MaterialKit.DpCanvasScaler::m_Canvas
		IL_0014: ldarg.1
		IL_0015: callvirt instance void [UnityEngine.UIModule]UnityEngine.Canvas::set_referencePixelsPerUnit(float32)
		IL_001a: ldarg.0
		IL_001b: ldarg.1
		IL_001c: stfld float32 MaterialKit.DpCanvasScaler::m_PrevReferencePixelsPerUnit

		IL_0021: nop
		IL_0022: ret

		IL_0023: nop
		IL_0024: ldnull
		IL_0025: throw
	} // end of method DpCanvasScaler::SetReferencePixelsPerUnit

	// Properties
	.property instance float32 referencePixelsPerUnit()
	{
		.get instance float32 MaterialKit.DpCanvasScaler::get_referencePixelsPerUnit()
		.set instance void MaterialKit.DpCanvasScaler::set_referencePixelsPerUnit(float32)
	}
	.property instance float32 fallbackScreenDPI()
	{
		.get instance float32 MaterialKit.DpCanvasScaler::get_fallbackScreenDPI()
		.set instance void MaterialKit.DpCanvasScaler::set_fallbackScreenDPI(float32)
	}
	.property instance float32 defaultSpriteDPI()
	{
		.get instance float32 MaterialKit.DpCanvasScaler::get_defaultSpriteDPI()
		.set instance void MaterialKit.DpCanvasScaler::set_defaultSpriteDPI(float32)
	}
	.property instance float32 dynamicPixelsPerUnit()
	{
		.get instance float32 MaterialKit.DpCanvasScaler::get_dynamicPixelsPerUnit()
		.set instance void MaterialKit.DpCanvasScaler::set_dynamicPixelsPerUnit(float32)
	}

} // end of class MaterialKit.DpCanvasScaler
