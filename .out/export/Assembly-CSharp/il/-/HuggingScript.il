.class public auto ansi beforefieldinit HuggingScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field private class [UnityEngine.PhysicsModule]UnityEngine.Rigidbody rb
	.field private valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 otherVelocity
	.field public bool inBsoda
	.field private float32 failSave

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0xded4
		// Header size: 12
		// Code size: 16 (0x10)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.PhysicsModule]UnityEngine.Rigidbody
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.PhysicsModule]UnityEngine.Rigidbody>()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.PhysicsModule]UnityEngine.Rigidbody HuggingScript::rb
		IL_000f: ret
	} // end of method HuggingScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0xdef0
		// Header size: 12
		// Code size: 49 (0x31)
		.maxstack 3
		.locals init (
			[0] float32,
			[1] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld float32 HuggingScript::failSave
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld float32 HuggingScript::failSave
		IL_000e: ldc.r4 0.0
		IL_0013: cgt
		IL_0015: brtrue.s IL_0020

		IL_0017: nop
		IL_0018: ldarg.0
		IL_0019: ldc.i4.0
		IL_001a: stfld bool HuggingScript::inBsoda
		IL_001f: ret

		IL_0020: nop
		IL_0021: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0026: stloc.1
		IL_0027: ldarg.0
		IL_0028: ldloc.0
		IL_0029: ldloc.1
		IL_002a: sub
		IL_002b: stfld float32 HuggingScript::failSave
		IL_0030: ret
	} // end of method HuggingScript::Update

	.method private hidebysig 
		instance void FixedUpdate () cil managed 
	{
		// Method begins at RVA 0xdf30
		// Header size: 12
		// Code size: 84 (0x54)
		.maxstack 3
		.locals init (
			[0] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld bool HuggingScript::inBsoda
		IL_0007: ldc.i4.0
		IL_0008: ceq
		IL_000a: brtrue.s IL_004f

		IL_000c: nop
		IL_000d: ldarg.0
		IL_000e: ldfld class [UnityEngine.PhysicsModule]UnityEngine.Rigidbody HuggingScript::rb
		IL_0013: ldloca.s 0
		IL_0015: ldarg.0
		IL_0016: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 HuggingScript::otherVelocity
		IL_001b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0020: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0025: ldloca.s 0
		IL_0027: ldarg.0
		IL_0028: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 HuggingScript::otherVelocity
		IL_002d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0032: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0037: ldloca.s 0
		IL_0039: ldarg.0
		IL_003a: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 HuggingScript::otherVelocity
		IL_003f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0044: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0049: ldloc.0
		IL_004a: callvirt instance void [UnityEngine.PhysicsModule]UnityEngine.Rigidbody::set_velocity(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)

		IL_004f: nop
		IL_0050: ret

		IL_0051: nop
		IL_0052: ldnull
		IL_0053: throw
	} // end of method HuggingScript::FixedUpdate

	.method private hidebysig 
		instance void OnTriggerStay (
			class [UnityEngine.PhysicsModule]UnityEngine.Collider other
		) cil managed 
	{
		// Method begins at RVA 0xdf90
		// Header size: 12
		// Code size: 158 (0x9e)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[2] string,
			[3] bool,
			[4] bool,
			[5] class [UnityEngine.PhysicsModule]UnityEngine.Rigidbody,
			[6] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[7] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[8] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[9] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: stloc.1
		IL_000a: ldloc.1
		IL_000b: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_0010: stloc.2
		IL_0011: ldloc.2
		IL_0012: ldstr "1st Prize"
		IL_0017: call bool [mscorlib]System.String::Equals(string, string)
		IL_001c: stloc.3
		IL_001d: ldloc.3
		IL_001e: stloc.s 4
		IL_0020: ldloc.3
		IL_0021: ldc.i4.0
		IL_0022: ceq
		IL_0024: brtrue.s IL_0099

		IL_0026: nop
		IL_0027: ldarg.0
		IL_0028: ldfld class [UnityEngine.PhysicsModule]UnityEngine.Rigidbody HuggingScript::rb
		IL_002d: stloc.s 5
		IL_002f: ldarg.0
		IL_0030: ldc.i4.1
		IL_0031: stfld bool HuggingScript::inBsoda
		IL_0036: ldloc.s 5
		IL_0038: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.PhysicsModule]UnityEngine.Rigidbody::get_velocity()
		IL_003d: stloc.s 6
		IL_003f: ldarg.1
		IL_0040: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent>()
		IL_0045: stloc.s 7
		IL_0047: ldloc.s 7
		IL_0049: stloc.s 8
		IL_004b: ldloc.s 8
		IL_004d: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::get_velocity()
		IL_0052: stloc.s 9
		IL_0054: ldarg.0
		IL_0055: ldc.r4 1
		IL_005a: stfld float32 HuggingScript::failSave
		IL_005f: ldarg.0
		IL_0060: ldstr "unpcklps xmm6,xmm7"
		IL_0065: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_006a: box [mscorlib]System.Int64
		IL_006f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0074: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 HuggingScript::otherVelocity
		IL_0079: ldarg.0
		IL_007a: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 HuggingScript::otherVelocity
		IL_007f: ldloca.s 6
		IL_0081: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0086: ldc.r4 0.1
		IL_008b: mul
		IL_008c: ldloca.s 9
		IL_008e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0093: add
		IL_0094: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z

		IL_0099: nop
		IL_009a: ret

		IL_009b: nop
		IL_009c: ldnull
		IL_009d: throw
	} // end of method HuggingScript::OnTriggerStay

	.method private hidebysig 
		instance void OnTriggerExit () cil managed 
	{
		// Method begins at RVA 0x2542
		// Header size: 1
		// Code size: 9 (0x9)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i4.0
		IL_0003: stfld bool HuggingScript::inBsoda
		IL_0008: ret
	} // end of method HuggingScript::OnTriggerExit

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x254c
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method HuggingScript::.ctor

} // end of class HuggingScript
