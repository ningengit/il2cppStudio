.class public auto ansi beforefieldinit MoveTest
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Methods
	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0xe03c
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
		IL_0019: ldc.r4 0.1
		IL_001e: ldloca.s 2
		IL_0020: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0025: add
		IL_0026: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_002b: ldloca.s 3
		IL_002d: ldc.r4 0.0
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
	} // end of method MoveTest::Update

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2555
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method MoveTest::.ctor

} // end of class MoveTest
