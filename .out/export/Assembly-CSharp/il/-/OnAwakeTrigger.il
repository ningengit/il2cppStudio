.class public auto ansi beforefieldinit OnAwakeTrigger
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent OnEnableEvent

	// Methods
	.method private hidebysig 
		instance void OnEnable () cil managed 
	{
		// Method begins at RVA 0x25a3
		// Header size: 1
		// Code size: 16 (0x10)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent OnAwakeTrigger::OnEnableEvent
		IL_0007: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent::Invoke()
		IL_000c: ret

		IL_000d: nop
		IL_000e: ldnull
		IL_000f: throw
	} // end of method OnAwakeTrigger::OnEnable

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x25b4
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method OnAwakeTrigger::.ctor

} // end of class OnAwakeTrigger
