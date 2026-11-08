.class public auto ansi beforefieldinit FacultyTriggerScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class PlayerScript ps
	.field private class [UnityEngine.PhysicsModule]UnityEngine.BoxCollider hitBox

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x10a08
		// Header size: 12
		// Code size: 16 (0x10)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.PhysicsModule]UnityEngine.BoxCollider
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.PhysicsModule]UnityEngine.BoxCollider>()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.PhysicsModule]UnityEngine.BoxCollider FacultyTriggerScript::hitBox
		IL_000f: ret
	} // end of method FacultyTriggerScript::Start

	.method private hidebysig 
		instance void OnTriggerStay (
			class [UnityEngine.PhysicsModule]UnityEngine.Collider other
		) cil managed 
	{
		// Method begins at RVA 0x10a24
		// Header size: 12
		// Code size: 85 (0x55)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[1] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[2] bool,
			[3] bool,
			[4] class PlayerScript
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: stloc.1
		IL_000a: ldloc.1
		IL_000b: ldstr "Player"
		IL_0010: callvirt instance bool [UnityEngine.CoreModule]UnityEngine.GameObject::CompareTag(string)
		IL_0015: stloc.2
		IL_0016: ldloc.2
		IL_0017: stloc.3
		IL_0018: ldloc.2
		IL_0019: ldc.i4.0
		IL_001a: ceq
		IL_001c: brtrue.s IL_0050

		IL_001e: nop
		IL_001f: ldarg.0
		IL_0020: ldfld class PlayerScript FacultyTriggerScript::ps
		IL_0025: stloc.s 4
		IL_0027: ldc.r4 1
		IL_002c: ldloc.s 4
		IL_002e: ldfld float32 PlayerScript::guilt
		IL_0033: clt
		IL_0035: brtrue.s IL_0050

		IL_0037: nop
		IL_0038: ldloc.s 4
		IL_003a: ldstr "faculty"
		IL_003f: stfld string PlayerScript::guiltType
		IL_0044: ldloc.s 4
		IL_0046: ldc.r4 1
		IL_004b: stfld float32 PlayerScript::guilt

		IL_0050: nop
		IL_0051: ret

		IL_0052: nop
		IL_0053: ldnull
		IL_0054: throw
	} // end of method FacultyTriggerScript::OnTriggerStay

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2621
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method FacultyTriggerScript::.ctor

} // end of class FacultyTriggerScript
