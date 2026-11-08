.class public auto ansi beforefieldinit MenuController
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class UIController uc
	.field public class [UnityEngine.UI]UnityEngine.UI.Selectable firstButton
	.field public class [UnityEngine.UI]UnityEngine.UI.Selectable dummyButtonPC
	.field public class [UnityEngine.UI]UnityEngine.UI.Selectable dummyButtonElse
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject back

	// Methods
	.method public hidebysig 
		instance void OnEnable () cil managed 
	{
		// Method begins at RVA 0x8c90
		// Header size: 12
		// Code size: 87 (0x57)
		.maxstack 2
		.locals init (
			[0] class UIController,
			[1] class [UnityEngine.UI]UnityEngine.UI.Selectable,
			[2] class [UnityEngine.UI]UnityEngine.UI.Selectable,
			[3] class UIController,
			[4] class UIController,
			[5] class [UnityEngine.UI]UnityEngine.UI.Selectable,
			[6] class UIController
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class UIController MenuController::uc
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld class [UnityEngine.UI]UnityEngine.UI.Selectable MenuController::firstButton
		IL_000e: stloc.1
		IL_000f: ldloc.0
		IL_0010: ldloc.1
		IL_0011: stfld class [UnityEngine.UI]UnityEngine.UI.Selectable UIController::firstButton
		IL_0016: ldarg.0
		IL_0017: ldfld class [UnityEngine.UI]UnityEngine.UI.Selectable MenuController::dummyButtonPC
		IL_001c: stloc.2
		IL_001d: ldarg.0
		IL_001e: ldfld class UIController MenuController::uc
		IL_0023: stloc.3
		IL_0024: ldloc.3
		IL_0025: ldloc.2
		IL_0026: stfld class [UnityEngine.UI]UnityEngine.UI.Selectable UIController::dummyButtonPC
		IL_002b: ldarg.0
		IL_002c: ldfld class UIController MenuController::uc
		IL_0031: stloc.s 4
		IL_0033: ldarg.0
		IL_0034: ldfld class [UnityEngine.UI]UnityEngine.UI.Selectable MenuController::dummyButtonElse
		IL_0039: stloc.s 5
		IL_003b: ldloc.s 4
		IL_003d: ldloc.s 5
		IL_003f: stfld class [UnityEngine.UI]UnityEngine.UI.Selectable UIController::dummyButtonElse
		IL_0044: ldarg.0
		IL_0045: ldfld class UIController MenuController::uc
		IL_004a: stloc.s 6
		IL_004c: ldloc.s 6
		IL_004e: callvirt instance void UIController::UpdateControllerType()
		IL_0053: ret

		IL_0054: nop
		IL_0055: ldnull
		IL_0056: throw
	} // end of method MenuController::OnEnable

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2265
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method MenuController::.ctor

} // end of class MenuController
