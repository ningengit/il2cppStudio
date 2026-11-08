.class public auto ansi beforefieldinit MouseoverScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
	implements [UnityEngine.UI]UnityEngine.EventSystems.ISelectHandler,
	           [UnityEngine.UI]UnityEngine.EventSystems.IEventSystemHandler,
	           [UnityEngine.UI]UnityEngine.EventSystems.IDeselectHandler,
	           [UnityEngine.UI]UnityEngine.EventSystems.IPointerEnterHandler,
	           [UnityEngine.UI]UnityEngine.EventSystems.IPointerExitHandler
{
	// Fields
	.field public class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent mouseOver
	.field public class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent mouseLeave

	// Methods
	.method public final hidebysig newslot virtual 
		instance void OnSelect (
			class [UnityEngine.UI]UnityEngine.EventSystems.BaseEventData eventData
		) cil managed 
	{
		// Method begins at RVA 0x2456
		// Header size: 1
		// Code size: 16 (0x10)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent MouseoverScript::mouseOver
		IL_0007: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent::Invoke()
		IL_000c: ret

		IL_000d: nop
		IL_000e: ldnull
		IL_000f: throw
	} // end of method MouseoverScript::OnSelect

	.method public final hidebysig newslot virtual 
		instance void OnPointerEnter (
			class [UnityEngine.UI]UnityEngine.EventSystems.PointerEventData eventData
		) cil managed 
	{
		// Method begins at RVA 0x2467
		// Header size: 1
		// Code size: 16 (0x10)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent MouseoverScript::mouseOver
		IL_0007: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent::Invoke()
		IL_000c: ret

		IL_000d: nop
		IL_000e: ldnull
		IL_000f: throw
	} // end of method MouseoverScript::OnPointerEnter

	.method public final hidebysig newslot virtual 
		instance void OnDeselect (
			class [UnityEngine.UI]UnityEngine.EventSystems.BaseEventData eventData
		) cil managed 
	{
		// Method begins at RVA 0x2478
		// Header size: 1
		// Code size: 16 (0x10)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent MouseoverScript::mouseLeave
		IL_0007: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent::Invoke()
		IL_000c: ret

		IL_000d: nop
		IL_000e: ldnull
		IL_000f: throw
	} // end of method MouseoverScript::OnDeselect

	.method public final hidebysig newslot virtual 
		instance void OnPointerExit (
			class [UnityEngine.UI]UnityEngine.EventSystems.PointerEventData eventData
		) cil managed 
	{
		// Method begins at RVA 0x2489
		// Header size: 1
		// Code size: 16 (0x10)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent MouseoverScript::mouseLeave
		IL_0007: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent::Invoke()
		IL_000c: ret

		IL_000d: nop
		IL_000e: ldnull
		IL_000f: throw
	} // end of method MouseoverScript::OnPointerExit

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x249a
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method MouseoverScript::.ctor

} // end of class MouseoverScript
