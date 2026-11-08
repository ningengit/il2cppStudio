.class public auto ansi beforefieldinit BasicButtonScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field private class [UnityEngine.UI]UnityEngine.UI.Button button
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject screen

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x5b7c
		// Header size: 12
		// Code size: 51 (0x33)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.UI]UnityEngine.UI.Button,
			[1] class [UnityEngine.UI]UnityEngine.UI.Button/ButtonClickedEvent,
			[2] class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.UI]UnityEngine.UI.Button>()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.UI]UnityEngine.UI.Button BasicButtonScript::button
		IL_000f: ldarg.0
		IL_0010: ldfld class [UnityEngine.UI]UnityEngine.UI.Button BasicButtonScript::button
		IL_0015: ldfld class [UnityEngine.UI]UnityEngine.UI.Button/ButtonClickedEvent [UnityEngine.UI]UnityEngine.UI.Button::m_OnClick
		IL_001a: stloc.1
		IL_001b: ldarg.0
		IL_001c: ldftn instance void BasicButtonScript::OpenScreen()
		IL_0022: newobj instance void [mscorlib]System.Action::.ctor(object, native int)
		IL_0027: stloc.2
		IL_0028: ldloc.1
		IL_0029: ldloc.2
		IL_002a: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent::AddListener(class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction)
		IL_002f: ret

		IL_0030: nop
		IL_0031: ldnull
		IL_0032: throw
	} // end of method BasicButtonScript::Start

	.method private hidebysig 
		instance void OpenScreen () cil managed 
	{
		// Method begins at RVA 0x21f6
		// Header size: 1
		// Code size: 17 (0x11)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject BasicButtonScript::screen
		IL_0007: ldc.i4.1
		IL_0008: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_000d: ret

		IL_000e: nop
		IL_000f: ldnull
		IL_0010: throw
	} // end of method BasicButtonScript::OpenScreen

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2208
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method BasicButtonScript::.ctor

} // end of class BasicButtonScript
