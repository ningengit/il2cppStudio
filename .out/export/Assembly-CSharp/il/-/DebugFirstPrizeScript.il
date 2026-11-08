.class public auto ansi beforefieldinit DebugFirstPrizeScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform player
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform first

	// Methods
	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0xd1b4
		// Header size: 12
		// Code size: 151 (0x97)
		.maxstack 4
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[3] int32,
			[4] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[5] int32,
			[6] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform DebugFirstPrizeScript::first
		IL_000e: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0013: stloc.1
		IL_0014: ldarg.0
		IL_0015: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform DebugFirstPrizeScript::first
		IL_001a: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_forward()
		IL_001f: stloc.2
		IL_0020: ldloca.s 2
		IL_0022: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0027: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_002c: stloc.3
		IL_002d: ldarg.0
		IL_002e: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform DebugFirstPrizeScript::first
		IL_0033: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_forward()
		IL_0038: stloc.s 4
		IL_003a: ldloca.s 4
		IL_003c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0041: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_0046: stloc.s 5
		IL_0048: ldloc.0
		IL_0049: ldloca.s 6
		IL_004b: ldloc.3
		IL_004c: conv.r4
		IL_004d: ldc.r4 3
		IL_0052: mul
		IL_0053: ldloca.s 1
		IL_0055: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_005a: add
		IL_005b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0060: ldloca.s 6
		IL_0062: ldloca.s 1
		IL_0064: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0069: ldc.r4 0.0
		IL_006e: add
		IL_006f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0074: ldloca.s 6
		IL_0076: ldloc.s 5
		IL_0078: conv.r4
		IL_0079: ldc.r4 3
		IL_007e: mul
		IL_007f: ldloca.s 1
		IL_0081: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0086: add
		IL_0087: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_008c: ldloc.s 6
		IL_008e: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0093: ret

		IL_0094: nop
		IL_0095: ldnull
		IL_0096: throw
	} // end of method DebugFirstPrizeScript::Update

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x252b
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method DebugFirstPrizeScript::.ctor

} // end of class DebugFirstPrizeScript
