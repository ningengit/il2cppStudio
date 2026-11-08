.class public auto ansi beforefieldinit UIController
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class CursorControllerScript cc
	.field private bool joystickEnabled
	.field public bool controlMouse
	.field public bool unlockOnStart
	.field public bool uiControlEnabled
	.field public class [UnityEngine.UI]UnityEngine.UI.Selectable firstButton
	.field public class [UnityEngine.UI]UnityEngine.UI.Selectable dummyButtonPC
	.field public class [UnityEngine.UI]UnityEngine.UI.Selectable dummyButtonElse
	.field public string buttonTag

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x23c0
		// Header size: 1
		// Code size: 51 (0x33)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld bool UIController::unlockOnStart
		IL_0007: ldarg.0
		IL_0008: ldfld bool UIController::joystickEnabled
		IL_000d: ldc.i4.0
		IL_000e: ceq
		IL_0010: and
		IL_0011: ldc.i4.0
		IL_0012: ceq
		IL_0014: brtrue.s IL_002e

		IL_0016: nop
		IL_0017: ldc.i4.0
		IL_0018: box [mscorlib]System.Int32
		IL_001d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode>(object)
		IL_0022: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_lockState(valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode)
		IL_0027: ldc.i4.1
		IL_0028: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_visible(bool)
		IL_002d: ret

		IL_002e: nop
		IL_002f: ret

		IL_0030: nop
		IL_0031: ldnull
		IL_0032: throw
	} // end of method UIController::Start

	.method private hidebysig 
		instance void OnEnable () cil managed 
	{
		// Method begins at RVA 0x23f4
		// Header size: 1
		// Code size: 11 (0xb)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void UIController::UpdateControllerType()
		IL_0007: ret

		IL_0008: nop
		IL_0009: ldnull
		IL_000a: throw
	} // end of method UIController::OnEnable

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x2400
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void UIController::UpdateControllerType()
		IL_0007: ret
	} // end of method UIController::Update

	.method public hidebysig 
		instance void SwitchMenu () cil managed 
	{
		// Method begins at RVA 0x2409
		// Header size: 1
		// Code size: 11 (0xb)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void UIController::UpdateControllerType()
		IL_0007: ret

		IL_0008: nop
		IL_0009: ldnull
		IL_000a: throw
	} // end of method UIController::SwitchMenu

	.method private hidebysig 
		instance void UpdateControllerType () cil managed 
	{
		// Method begins at RVA 0xaa14
		// Header size: 12
		// Code size: 167 (0xa7)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.UI]UnityEngine.EventSystems.EventSystem,
			[1] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[2] string,
			[3] bool,
			[4] class [UnityEngine.UI]UnityEngine.UI.Selectable,
			[5] int32,
			[6] bool,
			[7] bool
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld bool UIController::joystickEnabled
		IL_0007: ldc.i4.1
		IL_0008: and
		IL_0009: ldc.i4.0
		IL_000a: ceq
		IL_000c: brtrue.s IL_0038

		IL_000e: nop
		IL_000f: ldarg.0
		IL_0010: ldc.i4.0
		IL_0011: stfld bool UIController::joystickEnabled
		IL_0016: ldarg.0
		IL_0017: ldfld bool UIController::controlMouse
		IL_001c: ldc.i4.0
		IL_001d: ceq
		IL_001f: brtrue.s IL_0038

		IL_0021: nop
		IL_0022: ldc.i4.0
		IL_0023: box [mscorlib]System.Int32
		IL_0028: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode>(object)
		IL_002d: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_lockState(valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode)
		IL_0032: ldc.i4.1
		IL_0033: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_visible(bool)

		IL_0038: nop
		IL_0039: ldarg.0
		IL_003a: ldfld bool UIController::uiControlEnabled
		IL_003f: ldc.i4.0
		IL_0040: ceq
		IL_0042: brtrue.s IL_0054

		IL_0044: nop
		IL_0045: ldarg.0
		IL_0046: ldfld bool UIController::joystickEnabled
		IL_004b: ldc.i4.0
		IL_004c: ceq
		IL_004e: ldc.i4.0
		IL_004f: ceq
		IL_0051: brtrue.s IL_0056

		IL_0053: nop

		IL_0054: nop
		IL_0055: ret

		IL_0056: nop
		IL_0057: br.s IL_005a

		IL_0059: nop

		IL_005a: nop
		IL_005b: call class [UnityEngine.UI]UnityEngine.EventSystems.EventSystem [UnityEngine.UI]UnityEngine.EventSystems.EventSystem::get_current()
		IL_0060: stloc.0
		IL_0061: ldloc.0
		IL_0062: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.UI]UnityEngine.EventSystems.EventSystem::m_CurrentSelected
		IL_0067: stloc.1
		IL_0068: ldloc.1
		IL_0069: callvirt instance string [UnityEngine.CoreModule]UnityEngine.GameObject::get_tag()
		IL_006e: stloc.2
		IL_006f: ldloc.2
		IL_0070: ldarg.0
		IL_0071: ldfld string UIController::buttonTag
		IL_0076: call bool [mscorlib]System.String::op_Inequality(string, string)
		IL_007b: stloc.3
		IL_007c: ldarg.0
		IL_007d: ldfld class [UnityEngine.UI]UnityEngine.UI.Selectable UIController::firstButton
		IL_0082: stloc.s 4
		IL_0084: ldloc.3
		IL_0085: stloc.s 5
		IL_0087: br.s IL_008a

		IL_0089: nop

		IL_008a: nop
		IL_008b: ldloc.s 4
		IL_008d: ldnull
		IL_008e: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Inequality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0093: stloc.s 6
		IL_0095: ldloc.s 6
		IL_0097: stloc.s 7
		IL_0099: ldloc.s 5
		IL_009b: ldloc.s 6
		IL_009d: and
		IL_009e: ldc.i4.0
		IL_009f: ceq
		IL_00a1: brtrue.s IL_0054

		IL_00a3: nop
		IL_00a4: nop
		IL_00a5: ldnull
		IL_00a6: throw
	} // end of method UIController::UpdateControllerType

	.method private hidebysig 
		instance void UIUpdate () cil managed 
	{
		// Method begins at RVA 0xaac8
		// Header size: 12
		// Code size: 111 (0x6f)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.UI]UnityEngine.EventSystems.EventSystem,
			[1] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[2] string,
			[3] bool,
			[4] class [UnityEngine.UI]UnityEngine.UI.Selectable,
			[5] int32,
			[6] bool,
			[7] bool
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld bool UIController::uiControlEnabled
		IL_0007: ldc.i4.0
		IL_0008: ceq
		IL_000a: brtrue.s IL_001c

		IL_000c: nop
		IL_000d: ldarg.0
		IL_000e: ldfld bool UIController::joystickEnabled
		IL_0013: ldc.i4.0
		IL_0014: ceq
		IL_0016: ldc.i4.0
		IL_0017: ceq
		IL_0019: brtrue.s IL_001e

		IL_001b: nop

		IL_001c: nop
		IL_001d: ret

		IL_001e: nop
		IL_001f: br.s IL_0022

		IL_0021: nop

		IL_0022: nop
		IL_0023: call class [UnityEngine.UI]UnityEngine.EventSystems.EventSystem [UnityEngine.UI]UnityEngine.EventSystems.EventSystem::get_current()
		IL_0028: stloc.0
		IL_0029: ldloc.0
		IL_002a: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.UI]UnityEngine.EventSystems.EventSystem::m_CurrentSelected
		IL_002f: stloc.1
		IL_0030: ldloc.1
		IL_0031: callvirt instance string [UnityEngine.CoreModule]UnityEngine.GameObject::get_tag()
		IL_0036: stloc.2
		IL_0037: ldloc.2
		IL_0038: ldarg.0
		IL_0039: ldfld string UIController::buttonTag
		IL_003e: call bool [mscorlib]System.String::op_Inequality(string, string)
		IL_0043: stloc.3
		IL_0044: ldarg.0
		IL_0045: ldfld class [UnityEngine.UI]UnityEngine.UI.Selectable UIController::firstButton
		IL_004a: stloc.s 4
		IL_004c: ldloc.3
		IL_004d: stloc.s 5
		IL_004f: br.s IL_0052

		IL_0051: nop

		IL_0052: nop
		IL_0053: ldloc.s 4
		IL_0055: ldnull
		IL_0056: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Inequality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_005b: stloc.s 6
		IL_005d: ldloc.s 6
		IL_005f: stloc.s 7
		IL_0061: ldloc.s 5
		IL_0063: ldloc.s 6
		IL_0065: and
		IL_0066: ldc.i4.0
		IL_0067: ceq
		IL_0069: brtrue.s IL_001c

		IL_006b: nop
		IL_006c: nop
		IL_006d: ldnull
		IL_006e: throw
	} // end of method UIController::UIUpdate

	.method public hidebysig 
		instance void EnableControl () cil managed 
	{
		// Method begins at RVA 0x2415
		// Header size: 1
		// Code size: 9 (0x9)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i4.1
		IL_0003: stfld bool UIController::uiControlEnabled
		IL_0008: ret
	} // end of method UIController::EnableControl

	.method public hidebysig 
		instance void DisableControl () cil managed 
	{
		// Method begins at RVA 0x241f
		// Header size: 1
		// Code size: 9 (0x9)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i4.0
		IL_0003: stfld bool UIController::uiControlEnabled
		IL_0008: ret
	} // end of method UIController::DisableControl

	.method private hidebysig 
		instance void SelectDummy () cil managed 
	{
		// Method begins at RVA 0x2429
		// Header size: 1
		// Code size: 4 (0x4)
		.maxstack 8

		IL_0000: nop
		IL_0001: nop
		IL_0002: ldnull
		IL_0003: throw
	} // end of method UIController::SelectDummy

	.method public hidebysig specialname 
		instance bool get_usingJoystick () cil managed 
	{
		// Method begins at RVA 0x242e
		// Header size: 1
		// Code size: 3 (0x3)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldc.i4.0
		IL_0002: ret
	} // end of method UIController::get_usingJoystick

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2432
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method UIController::.ctor

	// Properties
	.property instance bool usingJoystick()
	{
		.get instance bool UIController::get_usingJoystick()
	}

} // end of class UIController
