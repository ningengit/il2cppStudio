.class public auto ansi beforefieldinit BsodaEffectScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field private class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent agent
	.field private valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 otherVelocity
	.field private bool inBsoda
	.field private float32 failSave

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x10c60
		// Header size: 12
		// Code size: 16 (0x10)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent>()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent BsodaEffectScript::agent
		IL_000f: ret
	} // end of method BsodaEffectScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x10c7c
		// Header size: 12
		// Code size: 131 (0x83)
		.maxstack 3
		.locals init (
			[0] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[1] float32,
			[2] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld bool BsodaEffectScript::inBsoda
		IL_0007: ldc.i4.0
		IL_0008: ceq
		IL_000a: brtrue.s IL_004f

		IL_000c: nop
		IL_000d: ldarg.0
		IL_000e: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent BsodaEffectScript::agent
		IL_0013: ldloca.s 0
		IL_0015: ldarg.0
		IL_0016: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BsodaEffectScript::otherVelocity
		IL_001b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0020: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0025: ldloca.s 0
		IL_0027: ldarg.0
		IL_0028: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BsodaEffectScript::otherVelocity
		IL_002d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0032: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0037: ldloca.s 0
		IL_0039: ldarg.0
		IL_003a: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BsodaEffectScript::otherVelocity
		IL_003f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0044: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0049: ldloc.0
		IL_004a: callvirt instance void [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::set_velocity(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)

		IL_004f: nop
		IL_0050: ldarg.0
		IL_0051: ldfld float32 BsodaEffectScript::failSave
		IL_0056: stloc.1
		IL_0057: ldarg.0
		IL_0058: ldfld float32 BsodaEffectScript::failSave
		IL_005d: ldc.r4 0.0
		IL_0062: cgt
		IL_0064: brtrue.s IL_006f

		IL_0066: nop
		IL_0067: ldarg.0
		IL_0068: ldc.i4.0
		IL_0069: stfld bool BsodaEffectScript::inBsoda
		IL_006e: ret

		IL_006f: nop
		IL_0070: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0075: stloc.2
		IL_0076: ldarg.0
		IL_0077: ldloc.1
		IL_0078: ldloc.2
		IL_0079: sub
		IL_007a: stfld float32 BsodaEffectScript::failSave
		IL_007f: ret

		IL_0080: nop
		IL_0081: ldnull
		IL_0082: throw
	} // end of method BsodaEffectScript::Update

	.method private hidebysig 
		instance void OnTriggerStay (
			class [UnityEngine.PhysicsModule]UnityEngine.Collider other
		) cil managed 
	{
		// Method begins at RVA 0x10d0c
		// Header size: 12
		// Code size: 316 (0x13c)
		.maxstack 3
		.locals init (
			[0] string,
			[1] bool,
			[2] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[3] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[4] string,
			[5] bool,
			[6] bool,
			[7] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[8] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[9] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[10] float32,
			[11] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[12] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[13] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[14] class [UnityEngine.PhysicsModule]UnityEngine.Rigidbody,
			[15] class [UnityEngine.PhysicsModule]UnityEngine.Rigidbody,
			[16] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[17] int64
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Component::get_tag()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: ldstr "BSODA"
		IL_000e: call bool [mscorlib]System.String::Equals(string, string)
		IL_0013: stloc.1
		IL_0014: ldloc.1
		IL_0015: ldc.i4.0
		IL_0016: ceq
		IL_0018: ldc.i4.0
		IL_0019: ceq
		IL_001b: brtrue IL_00d8

		IL_0020: nop
		IL_0021: ldarg.1
		IL_0022: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0027: stloc.2
		IL_0028: ldloc.2
		IL_0029: stloc.3
		IL_002a: ldloc.3
		IL_002b: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_0030: stloc.s 4
		IL_0032: ldloc.s 4
		IL_0034: ldstr "Gotta Sweep"
		IL_0039: call bool [mscorlib]System.String::Equals(string, string)
		IL_003e: stloc.s 5
		IL_0040: ldloc.s 5
		IL_0042: stloc.s 6
		IL_0044: ldloc.s 5
		IL_0046: ldc.i4.0
		IL_0047: ceq
		IL_0049: brtrue IL_00d6

		IL_004e: nop
		IL_004f: ldarg.0
		IL_0050: ldc.i4.1
		IL_0051: stfld bool BsodaEffectScript::inBsoda
		IL_0056: ldarg.0
		IL_0057: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_005c: stloc.s 7
		IL_005e: ldloc.s 7
		IL_0060: stloc.s 8
		IL_0062: ldloc.s 8
		IL_0064: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_forward()
		IL_0069: stloc.s 9
		IL_006b: ldarg.0
		IL_006c: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent BsodaEffectScript::agent
		IL_0071: callvirt instance float32 [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::get_speed()
		IL_0076: stloc.s 10
		IL_0078: ldarg.1
		IL_0079: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent>()
		IL_007e: stloc.s 11
		IL_0080: ldloc.s 11
		IL_0082: stloc.s 12
		IL_0084: ldloc.s 12
		IL_0086: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::get_velocity()
		IL_008b: stloc.s 13
		IL_008d: ldarg.0
		IL_008e: ldstr "unpcklps xmm6,xmm7"
		IL_0093: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0098: box [mscorlib]System.Int64
		IL_009d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_00a2: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BsodaEffectScript::otherVelocity
		IL_00a7: ldarg.0
		IL_00a8: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BsodaEffectScript::otherVelocity
		IL_00ad: ldloca.s 9
		IL_00af: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00b4: ldloc.s 10
		IL_00b6: mul
		IL_00b7: ldc.r4 0.1
		IL_00bc: mul
		IL_00bd: ldloca.s 13
		IL_00bf: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00c4: add
		IL_00c5: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z

		IL_00ca: nop
		IL_00cb: ldarg.0
		IL_00cc: ldc.r4 1
		IL_00d1: stfld float32 BsodaEffectScript::failSave

		IL_00d6: nop
		IL_00d7: ret

		IL_00d8: nop
		IL_00d9: ldarg.0
		IL_00da: ldc.i4.1
		IL_00db: stfld bool BsodaEffectScript::inBsoda
		IL_00e0: ldarg.1
		IL_00e1: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.PhysicsModule]UnityEngine.Rigidbody>()
		IL_00e6: stloc.s 14
		IL_00e8: ldloc.s 14
		IL_00ea: stloc.s 15
		IL_00ec: ldloc.s 15
		IL_00ee: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.PhysicsModule]UnityEngine.Rigidbody::get_velocity()
		IL_00f3: stloc.s 16
		IL_00f5: ldarg.0
		IL_00f6: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BsodaEffectScript::otherVelocity
		IL_00fb: ldloca.s 16
		IL_00fd: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0102: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0107: ldarg.0
		IL_0108: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BsodaEffectScript::otherVelocity
		IL_010d: ldloca.s 16
		IL_010f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0114: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0119: ldarg.0
		IL_011a: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 BsodaEffectScript::otherVelocity
		IL_011f: ldloca.s 16
		IL_0121: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0126: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_012b: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_0130: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0135: stloc.s 17
		IL_0137: br.s IL_00ca

		IL_0139: nop
		IL_013a: ldnull
		IL_013b: throw
	} // end of method BsodaEffectScript::OnTriggerStay

	.method private hidebysig 
		instance void OnTriggerExit () cil managed 
	{
		// Method begins at RVA 0x264c
		// Header size: 1
		// Code size: 9 (0x9)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i4.0
		IL_0003: stfld bool BsodaEffectScript::inBsoda
		IL_0008: ret
	} // end of method BsodaEffectScript::OnTriggerExit

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2656
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method BsodaEffectScript::.ctor

} // end of class BsodaEffectScript
