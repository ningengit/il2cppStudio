.class public auto ansi beforefieldinit EntranceScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class GameControllerScript gc
	.field public class [UnityEngine.CoreModule]UnityEngine.Material map
	.field public class [UnityEngine.CoreModule]UnityEngine.MeshRenderer wall

	// Methods
	.method public hidebysig 
		instance void Lower () cil managed 
	{
		// Method begins at RVA 0xaea8
		// Header size: 12
		// Code size: 147 (0x93)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[2] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[4] float32,
			[5] float32,
			[6] float32,
			[7] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[8] class GameControllerScript
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_000e: stloc.1
		IL_000f: ldloc.1
		IL_0010: stloc.2
		IL_0011: ldloc.2
		IL_0012: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0017: stloc.3
		IL_0018: ldloca.s 3
		IL_001a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_001f: ldc.r4 0.0
		IL_0024: sub
		IL_0025: stloc.s 4
		IL_0027: ldloca.s 3
		IL_0029: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_002e: ldc.r4 10
		IL_0033: sub
		IL_0034: stloc.s 5
		IL_0036: ldloca.s 3
		IL_0038: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_003d: ldc.r4 0.0
		IL_0042: sub
		IL_0043: stloc.s 6
		IL_0045: ldloc.0
		IL_0046: ldloca.s 7
		IL_0048: ldloc.s 4
		IL_004a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_004f: ldloca.s 7
		IL_0051: ldloc.s 5
		IL_0053: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0058: ldloca.s 7
		IL_005a: ldloc.s 6
		IL_005c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0061: ldloc.s 7
		IL_0063: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0068: ldarg.0
		IL_0069: ldfld class GameControllerScript EntranceScript::gc
		IL_006e: stloc.s 8
		IL_0070: ldloc.s 8
		IL_0072: ldfld bool GameControllerScript::finaleMode
		IL_0077: ldc.i4.0
		IL_0078: ceq
		IL_007a: brtrue.s IL_008e

		IL_007c: nop
		IL_007d: ldarg.0
		IL_007e: ldfld class [UnityEngine.CoreModule]UnityEngine.MeshRenderer EntranceScript::wall
		IL_0083: ldarg.0
		IL_0084: ldfld class [UnityEngine.CoreModule]UnityEngine.Material EntranceScript::map
		IL_0089: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Renderer::SetMaterial(class [UnityEngine.CoreModule]UnityEngine.Material)

		IL_008e: nop
		IL_008f: ret

		IL_0090: nop
		IL_0091: ldnull
		IL_0092: throw
	} // end of method EntranceScript::Lower

	.method public hidebysig 
		instance void Raise () cil managed 
	{
		// Method begins at RVA 0xaf48
		// Header size: 12
		// Code size: 93 (0x5d)
		.maxstack 4
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_000e: stloc.1
		IL_000f: ldloc.1
		IL_0010: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0015: stloc.2
		IL_0016: ldloc.0
		IL_0017: ldloca.s 3
		IL_0019: ldc.r4 0.0
		IL_001e: ldloca.s 2
		IL_0020: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0025: add
		IL_0026: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_002b: ldloca.s 3
		IL_002d: ldc.r4 10
		IL_0032: ldloca.s 2
		IL_0034: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0039: add
		IL_003a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_003f: ldloca.s 3
		IL_0041: ldc.r4 0.0
		IL_0046: ldloca.s 2
		IL_0048: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_004d: add
		IL_004e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0053: ldloc.3
		IL_0054: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0059: ret

		IL_005a: nop
		IL_005b: ldnull
		IL_005c: throw
	} // end of method EntranceScript::Raise

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x244d
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method EntranceScript::.ctor

} // end of class EntranceScript
