.class public auto ansi beforefieldinit CraftersTriggerScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform goTarget
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform fleeTarget
	.field public class CraftersScript crafters

	// Methods
	.method private hidebysig 
		instance void OnTriggerEnter (
			class [UnityEngine.PhysicsModule]UnityEngine.Collider other
		) cil managed 
	{
		// Method begins at RVA 0xd038
		// Header size: 12
		// Code size: 169 (0xa9)
		.maxstack 3
		.locals init (
			[0] string,
			[1] bool,
			[2] bool,
			[3] class CraftersScript,
			[4] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[5] class CraftersScript,
			[6] bool,
			[7] int64,
			[8] float32,
			[9] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[10] bool
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Component::get_tag()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: ldstr "Player"
		IL_000e: call bool [mscorlib]System.String::Equals(string, string)
		IL_0013: stloc.1
		IL_0014: ldloc.1
		IL_0015: stloc.2
		IL_0016: ldloc.1
		IL_0017: ldc.i4.0
		IL_0018: ceq
		IL_001a: brtrue IL_00a4

		IL_001f: nop
		IL_0020: ldarg.0
		IL_0021: ldfld class CraftersScript CraftersTriggerScript::crafters
		IL_0026: stloc.3
		IL_0027: ldarg.0
		IL_0028: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform CraftersTriggerScript::goTarget
		IL_002d: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0032: stloc.s 4
		IL_0034: ldloc.3
		IL_0035: stloc.s 5
		IL_0037: ldloc.s 5
		IL_0039: ldfld bool CraftersScript::angry
		IL_003e: ldc.i4.0
		IL_003f: ceq
		IL_0041: ldc.i4.0
		IL_0042: ceq
		IL_0044: brtrue.s IL_00a4

		IL_0046: nop
		IL_0047: ldloc.s 5
		IL_0049: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent CraftersScript::agent
		IL_004e: callvirt instance bool [UnityEngine.CoreModule]UnityEngine.Behaviour::get_isActiveAndEnabled()
		IL_0053: stloc.s 6
		IL_0055: ldloc.s 6
		IL_0057: stloc.2
		IL_0058: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_005d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0062: stloc.s 7
		IL_0064: ldloca.s 4
		IL_0066: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_006b: stloc.s 8
		IL_006d: ldloc.s 6
		IL_006f: ldc.i4.0
		IL_0070: ceq
		IL_0072: brtrue.s IL_00a4

		IL_0074: nop
		IL_0075: ldloc.s 5
		IL_0077: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent CraftersScript::agent
		IL_007c: ldloca.s 9
		IL_007e: ldloc.s 7
		IL_0080: box [mscorlib]System.Int64
		IL_0085: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_008a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_008f: ldloca.s 9
		IL_0091: ldloc.s 8
		IL_0093: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0098: ldloc.s 9
		IL_009a: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::SetDestination(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_009f: stloc.s 10
		IL_00a1: ldloc.s 10
		IL_00a3: stloc.2

		IL_00a4: nop
		IL_00a5: ret

		IL_00a6: nop
		IL_00a7: ldnull
		IL_00a8: throw
	} // end of method CraftersTriggerScript::OnTriggerEnter

	.method private hidebysig 
		instance void OnTriggerExit (
			class [UnityEngine.PhysicsModule]UnityEngine.Collider other
		) cil managed 
	{
		// Method begins at RVA 0xd0f0
		// Header size: 12
		// Code size: 181 (0xb5)
		.maxstack 3
		.locals init (
			[0] string,
			[1] bool,
			[2] bool,
			[3] class CraftersScript,
			[4] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[5] class CraftersScript,
			[6] bool,
			[7] int64,
			[8] float32,
			[9] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[10] bool
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Component::get_tag()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: ldstr "Player"
		IL_000e: call bool [mscorlib]System.String::Equals(string, string)
		IL_0013: stloc.1
		IL_0014: ldloc.1
		IL_0015: stloc.2
		IL_0016: ldloc.1
		IL_0017: ldc.i4.0
		IL_0018: ceq
		IL_001a: brtrue IL_00b0

		IL_001f: nop
		IL_0020: ldarg.0
		IL_0021: ldfld class CraftersScript CraftersTriggerScript::crafters
		IL_0026: stloc.3
		IL_0027: ldarg.0
		IL_0028: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform CraftersTriggerScript::fleeTarget
		IL_002d: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0032: stloc.s 4
		IL_0034: ldloc.3
		IL_0035: stloc.s 5
		IL_0037: ldloc.s 5
		IL_0039: ldfld bool CraftersScript::angry
		IL_003e: ldc.i4.0
		IL_003f: ceq
		IL_0041: ldc.i4.0
		IL_0042: ceq
		IL_0044: brtrue.s IL_00b0

		IL_0046: nop
		IL_0047: ldloc.s 5
		IL_0049: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent CraftersScript::agent
		IL_004e: callvirt instance bool [UnityEngine.CoreModule]UnityEngine.Behaviour::get_isActiveAndEnabled()
		IL_0053: stloc.s 6
		IL_0055: ldloc.s 6
		IL_0057: stloc.2
		IL_0058: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_005d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0062: stloc.s 7
		IL_0064: ldloca.s 4
		IL_0066: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_006b: stloc.s 8
		IL_006d: ldloc.s 6
		IL_006f: ldc.i4.0
		IL_0070: ceq
		IL_0072: brtrue.s IL_00b0

		IL_0074: nop
		IL_0075: ldloc.s 5
		IL_0077: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent CraftersScript::agent
		IL_007c: ldloca.s 9
		IL_007e: ldloc.s 7
		IL_0080: box [mscorlib]System.Int64
		IL_0085: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_008a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_008f: ldloca.s 9
		IL_0091: ldloc.s 8
		IL_0093: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0098: ldloc.s 9
		IL_009a: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::SetDestination(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_009f: stloc.s 10
		IL_00a1: ldloc.s 5
		IL_00a3: ldc.r4 3
		IL_00a8: stfld float32 CraftersScript::forceShowTime
		IL_00ad: ldloc.s 10
		IL_00af: stloc.2

		IL_00b0: nop
		IL_00b1: ret

		IL_00b2: nop
		IL_00b3: ldnull
		IL_00b4: throw
	} // end of method CraftersTriggerScript::OnTriggerExit

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2522
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method CraftersTriggerScript::.ctor

} // end of class CraftersTriggerScript
