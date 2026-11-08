.class public auto ansi beforefieldinit Billboard
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field private class [UnityEngine.CoreModule]UnityEngine.Camera m_Camera

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x32b0
		// Header size: 12
		// Code size: 15 (0xf)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Camera
		)

		IL_0000: nop
		IL_0001: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_0006: stloc.0
		IL_0007: ldarg.0
		IL_0008: ldloc.0
		IL_0009: stfld class [UnityEngine.CoreModule]UnityEngine.Camera Billboard::m_Camera
		IL_000e: ret
	} // end of method Billboard::Start

	.method private hidebysig 
		instance void LateUpdate () cil managed 
	{
		// Method begins at RVA 0x32cc
		// Header size: 12
		// Code size: 182 (0xb6)
		.maxstack 4
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[3] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[4] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[5] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[6] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[7] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3
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
		IL_0016: ldarg.0
		IL_0017: ldfld class [UnityEngine.CoreModule]UnityEngine.Camera Billboard::m_Camera
		IL_001c: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0021: stloc.3
		IL_0022: ldloc.3
		IL_0023: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Transform::get_rotation()
		IL_0028: stloc.s 4
		IL_002a: ldloc.s 4
		IL_002c: ldloca.s 5
		IL_002e: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::forwardVector
		IL_0033: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0038: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_003d: ldloca.s 5
		IL_003f: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::forwardVector
		IL_0044: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0049: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_004e: ldloca.s 5
		IL_0050: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::forwardVector
		IL_0055: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_005a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_005f: ldloc.s 5
		IL_0061: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Quaternion::op_Multiply(valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0066: stloc.s 6
		IL_0068: ldloc.0
		IL_0069: ldloca.s 7
		IL_006b: ldloca.s 2
		IL_006d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0072: ldloca.s 6
		IL_0074: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0079: add
		IL_007a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_007f: ldloca.s 7
		IL_0081: ldloca.s 2
		IL_0083: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0088: ldloca.s 6
		IL_008a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_008f: add
		IL_0090: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0095: ldloca.s 7
		IL_0097: ldloca.s 2
		IL_0099: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_009e: ldloca.s 6
		IL_00a0: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00a5: add
		IL_00a6: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00ab: ldloc.s 7
		IL_00ad: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::LookAt(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_00b2: ret

		IL_00b3: nop
		IL_00b4: ldnull
		IL_00b5: throw
	} // end of method Billboard::LateUpdate

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2063
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method Billboard::.ctor

} // end of class Billboard
