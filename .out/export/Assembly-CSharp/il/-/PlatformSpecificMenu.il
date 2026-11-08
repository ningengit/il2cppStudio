.class public auto ansi beforefieldinit PlatformSpecificMenu
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject pC
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject mobile

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x21a5
		// Header size: 1
		// Code size: 17 (0x11)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject PlatformSpecificMenu::pC
		IL_0007: ldc.i4.1
		IL_0008: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_000d: ret

		IL_000e: nop
		IL_000f: ldnull
		IL_0010: throw
	} // end of method PlatformSpecificMenu::Start

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x21b7
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method PlatformSpecificMenu::.ctor

} // end of class PlatformSpecificMenu
