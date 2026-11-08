.class public auto ansi beforefieldinit QuarterSpawnScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class AILocationSelectorScript wanderer
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform location

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x1220c
		// Header size: 12
		// Code size: 135 (0x87)
		.maxstack 4
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class AILocationSelectorScript QuarterSpawnScript::wanderer
		IL_0007: callvirt instance void AILocationSelectorScript::QuarterExclusive()
		IL_000c: ldarg.0
		IL_000d: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0012: stloc.0
		IL_0013: ldarg.0
		IL_0014: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform QuarterSpawnScript::location
		IL_0019: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_001e: stloc.1
		IL_001f: ldloc.0
		IL_0020: ldloca.s 2
		IL_0022: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::upVector
		IL_0027: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_002c: ldc.r4 4
		IL_0031: mul
		IL_0032: ldloca.s 1
		IL_0034: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0039: add
		IL_003a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_003f: ldloca.s 2
		IL_0041: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::upVector
		IL_0046: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_004b: ldc.r4 4
		IL_0050: mul
		IL_0051: ldloca.s 1
		IL_0053: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0058: add
		IL_0059: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_005e: ldloca.s 2
		IL_0060: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::upVector
		IL_0065: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_006a: ldc.r4 4
		IL_006f: mul
		IL_0070: ldloca.s 1
		IL_0072: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0077: add
		IL_0078: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_007d: ldloc.2
		IL_007e: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0083: ret

		IL_0084: nop
		IL_0085: ldnull
		IL_0086: throw
	} // end of method QuarterSpawnScript::Start

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2696
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method QuarterSpawnScript::.ctor

} // end of class QuarterSpawnScript
