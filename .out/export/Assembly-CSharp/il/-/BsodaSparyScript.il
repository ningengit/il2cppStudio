.class public auto ansi beforefieldinit BsodaSparyScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public float32 speed
	.field private float32 lifeSpan
	.field private class [UnityEngine.PhysicsModule]UnityEngine.Rigidbody rb

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x10e54
		// Header size: 12
		// Code size: 122 (0x7a)
		.maxstack 4
		.locals init (
			[0] class [UnityEngine.PhysicsModule]UnityEngine.Rigidbody,
			[1] class [UnityEngine.PhysicsModule]UnityEngine.Rigidbody,
			[2] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[4] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.PhysicsModule]UnityEngine.Rigidbody>()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.PhysicsModule]UnityEngine.Rigidbody BsodaSparyScript::rb
		IL_000f: ldarg.0
		IL_0010: ldfld class [UnityEngine.PhysicsModule]UnityEngine.Rigidbody BsodaSparyScript::rb
		IL_0015: stloc.1
		IL_0016: ldarg.0
		IL_0017: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_001c: stloc.2
		IL_001d: ldloc.2
		IL_001e: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_forward()
		IL_0023: stloc.3
		IL_0024: ldloc.1
		IL_0025: ldloca.s 4
		IL_0027: ldarg.0
		IL_0028: ldfld float32 BsodaSparyScript::speed
		IL_002d: ldloca.s 3
		IL_002f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0034: mul
		IL_0035: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_003a: ldloca.s 4
		IL_003c: ldarg.0
		IL_003d: ldfld float32 BsodaSparyScript::speed
		IL_0042: ldloca.s 3
		IL_0044: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0049: mul
		IL_004a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_004f: ldloca.s 4
		IL_0051: ldarg.0
		IL_0052: ldfld float32 BsodaSparyScript::speed
		IL_0057: ldloca.s 3
		IL_0059: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_005e: mul
		IL_005f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0064: ldloc.s 4
		IL_0066: callvirt instance void [UnityEngine.PhysicsModule]UnityEngine.Rigidbody::set_velocity(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_006b: ldarg.0
		IL_006c: ldc.r4 30
		IL_0071: stfld float32 BsodaSparyScript::lifeSpan
		IL_0076: ret

		IL_0077: nop
		IL_0078: ldnull
		IL_0079: throw
	} // end of method BsodaSparyScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x10edc
		// Header size: 12
		// Code size: 188 (0xbc)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.PhysicsModule]UnityEngine.Rigidbody,
			[1] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[2] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[4] float32,
			[5] float32,
			[6] float32,
			[7] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[8] float32,
			[9] float32,
			[10] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[11] class [UnityEngine.CoreModule]UnityEngine.GameObject
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.PhysicsModule]UnityEngine.Rigidbody BsodaSparyScript::rb
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_000e: stloc.1
		IL_000f: ldloc.1
		IL_0010: stloc.2
		IL_0011: ldloc.2
		IL_0012: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_forward()
		IL_0017: stloc.3
		IL_0018: ldarg.0
		IL_0019: ldfld float32 BsodaSparyScript::speed
		IL_001e: ldloca.s 3
		IL_0020: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0025: mul
		IL_0026: stloc.s 4
		IL_0028: ldarg.0
		IL_0029: ldfld float32 BsodaSparyScript::speed
		IL_002e: ldloca.s 3
		IL_0030: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0035: mul
		IL_0036: stloc.s 5
		IL_0038: ldarg.0
		IL_0039: ldfld float32 BsodaSparyScript::speed
		IL_003e: ldloca.s 3
		IL_0040: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0045: mul
		IL_0046: stloc.s 6
		IL_0048: ldloc.0
		IL_0049: ldloca.s 7
		IL_004b: ldloc.s 5
		IL_004d: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0052: ldloca.s 7
		IL_0054: ldloc.s 6
		IL_0056: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_005b: ldloca.s 7
		IL_005d: ldloc.s 4
		IL_005f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0064: ldloc.s 7
		IL_0066: callvirt instance void [UnityEngine.PhysicsModule]UnityEngine.Rigidbody::set_velocity(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_006b: ldarg.0
		IL_006c: ldfld float32 BsodaSparyScript::lifeSpan
		IL_0071: stloc.s 8
		IL_0073: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0078: stloc.s 9
		IL_007a: ldarg.0
		IL_007b: ldloc.s 8
		IL_007d: ldloc.s 9
		IL_007f: sub
		IL_0080: stfld float32 BsodaSparyScript::lifeSpan
		IL_0085: ldloc.s 9
		IL_0087: stloc.s 4
		IL_0089: ldc.r4 0.0
		IL_008e: ldloc.s 8
		IL_0090: ldloc.s 9
		IL_0092: sub
		IL_0093: cgt
		IL_0095: ldc.i4.0
		IL_0096: ceq
		IL_0098: brtrue.s IL_00b7

		IL_009a: nop
		IL_009b: ldarg.0
		IL_009c: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_00a1: stloc.s 10
		IL_00a3: ldloc.s 10
		IL_00a5: stloc.s 11
		IL_00a7: br.s IL_00aa

		IL_00a9: nop

		IL_00aa: nop
		IL_00ab: ldloc.s 11
		IL_00ad: ldc.r4 0.0
		IL_00b2: call void [UnityEngine.CoreModule]UnityEngine.Object::Destroy(class [UnityEngine.CoreModule]UnityEngine.Object, float32)

		IL_00b7: nop
		IL_00b8: ret

		IL_00b9: nop
		IL_00ba: ldnull
		IL_00bb: throw
	} // end of method BsodaSparyScript::Update

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x265f
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method BsodaSparyScript::.ctor

} // end of class BsodaSparyScript
