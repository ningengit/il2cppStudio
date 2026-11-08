.class public auto ansi beforefieldinit PauseMenuScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class GameControllerScript gc

	// Methods
	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x8cf4
		// Header size: 12
		// Code size: 61 (0x3d)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.UI]UnityEngine.EventSystems.EventSystem,
			[1] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[2] bool,
			[3] class GameControllerScript
		)

		IL_0000: nop
		IL_0001: br.s IL_0004

		IL_0003: nop

		IL_0004: nop
		IL_0005: call class [UnityEngine.UI]UnityEngine.EventSystems.EventSystem [UnityEngine.UI]UnityEngine.EventSystems.EventSystem::get_current()
		IL_000a: stloc.0
		IL_000b: ldloc.0
		IL_000c: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.UI]UnityEngine.EventSystems.EventSystem::m_CurrentSelected
		IL_0011: stloc.1
		IL_0012: br.s IL_0015

		IL_0014: nop

		IL_0015: nop
		IL_0016: ldloc.1
		IL_0017: ldnull
		IL_0018: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Equality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_001d: stloc.2
		IL_001e: ldarg.0
		IL_001f: ldfld class GameControllerScript PauseMenuScript::gc
		IL_0024: stloc.3
		IL_0025: ldloc.3
		IL_0026: ldfld bool GameControllerScript::mouseLocked
		IL_002b: ldc.i4.0
		IL_002c: ceq
		IL_002e: brtrue.s IL_0038

		IL_0030: nop
		IL_0031: ldloc.3
		IL_0032: callvirt instance void GameControllerScript::UnlockMouse()
		IL_0037: ret

		IL_0038: nop
		IL_0039: ret

		IL_003a: nop
		IL_003b: ldnull
		IL_003c: throw
	} // end of method PauseMenuScript::Update

	.method private hidebysig specialname 
		instance bool get_usingJoystick () cil managed 
	{
		// Method begins at RVA 0x226e
		// Header size: 1
		// Code size: 3 (0x3)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldc.i4.0
		IL_0002: ret
	} // end of method PauseMenuScript::get_usingJoystick

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2272
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method PauseMenuScript::.ctor

	// Properties
	.property instance bool usingJoystick()
	{
		.get instance bool PauseMenuScript::get_usingJoystick()
	}

} // end of class PauseMenuScript
