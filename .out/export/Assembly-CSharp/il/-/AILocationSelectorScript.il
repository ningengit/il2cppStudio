.class public auto ansi beforefieldinit AILocationSelectorScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform[] newLocation
	.field public class AmbienceScript ambience
	.field private int32 id

	// Methods
	.method public hidebysig 
		instance void GetNewTarget () cil managed 
	{
		// Method begins at RVA 0xb74c
		// Header size: 12
		// Code size: 131 (0x83)
		.maxstack 3
		.locals init (
			[0] float32,
			[1] int32,
			[2] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[4] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3
		)

		IL_0000: nop
		IL_0001: ldc.r4 0.0
		IL_0006: ldc.r4 28
		IL_000b: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_0017: stloc.1
		IL_0018: ldarg.0
		IL_0019: ldloc.1
		IL_001a: stfld int32 AILocationSelectorScript::id
		IL_001f: ldarg.0
		IL_0020: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0025: stloc.2
		IL_0026: ldarg.0
		IL_0027: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform[] AILocationSelectorScript::newLocation
		IL_002c: ldarg.0
		IL_002d: ldfld int32 AILocationSelectorScript::id
		IL_0032: conv.i8
		IL_0033: conv.i4
		IL_0034: ldelem [UnityEngine.CoreModule]UnityEngine.Transform
		IL_0039: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_003e: stloc.3
		IL_003f: ldloc.2
		IL_0040: ldloca.s 4
		IL_0042: ldloca.s 3
		IL_0044: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0049: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_004e: ldloca.s 4
		IL_0050: ldloca.s 3
		IL_0052: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0057: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_005c: ldloca.s 4
		IL_005e: ldloca.s 3
		IL_0060: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0065: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_006a: ldloc.s 4
		IL_006c: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0071: ldarg.0
		IL_0072: ldfld class AmbienceScript AILocationSelectorScript::ambience
		IL_0077: callvirt instance void AmbienceScript::PlayAudio()
		IL_007c: ret

		IL_007d: nop
		IL_007e: ldnull
		IL_007f: throw

		IL_0080: nop
		IL_0081: ldnull
		IL_0082: throw
	} // end of method AILocationSelectorScript::GetNewTarget

	.method public hidebysig 
		instance void GetNewTargetHallway () cil managed 
	{
		// Method begins at RVA 0xb7dc
		// Header size: 12
		// Code size: 131 (0x83)
		.maxstack 3
		.locals init (
			[0] float32,
			[1] int32,
			[2] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[4] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3
		)

		IL_0000: nop
		IL_0001: ldc.r4 0.0
		IL_0006: ldc.r4 15
		IL_000b: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_0017: stloc.1
		IL_0018: ldarg.0
		IL_0019: ldloc.1
		IL_001a: stfld int32 AILocationSelectorScript::id
		IL_001f: ldarg.0
		IL_0020: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0025: stloc.2
		IL_0026: ldarg.0
		IL_0027: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform[] AILocationSelectorScript::newLocation
		IL_002c: ldarg.0
		IL_002d: ldfld int32 AILocationSelectorScript::id
		IL_0032: conv.i8
		IL_0033: conv.i4
		IL_0034: ldelem [UnityEngine.CoreModule]UnityEngine.Transform
		IL_0039: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_003e: stloc.3
		IL_003f: ldloc.2
		IL_0040: ldloca.s 4
		IL_0042: ldloca.s 3
		IL_0044: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0049: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_004e: ldloca.s 4
		IL_0050: ldloca.s 3
		IL_0052: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0057: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_005c: ldloca.s 4
		IL_005e: ldloca.s 3
		IL_0060: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0065: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_006a: ldloc.s 4
		IL_006c: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0071: ldarg.0
		IL_0072: ldfld class AmbienceScript AILocationSelectorScript::ambience
		IL_0077: callvirt instance void AmbienceScript::PlayAudio()
		IL_007c: ret

		IL_007d: nop
		IL_007e: ldnull
		IL_007f: throw

		IL_0080: nop
		IL_0081: ldnull
		IL_0082: throw
	} // end of method AILocationSelectorScript::GetNewTargetHallway

	.method public hidebysig 
		instance void QuarterExclusive () cil managed 
	{
		// Method begins at RVA 0xb86c
		// Header size: 12
		// Code size: 120 (0x78)
		.maxstack 3
		.locals init (
			[0] float32,
			[1] int32,
			[2] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[4] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3
		)

		IL_0000: nop
		IL_0001: ldc.r4 1
		IL_0006: ldc.r4 15
		IL_000b: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_0017: stloc.1
		IL_0018: ldarg.0
		IL_0019: ldloc.1
		IL_001a: stfld int32 AILocationSelectorScript::id
		IL_001f: ldarg.0
		IL_0020: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0025: stloc.2
		IL_0026: ldarg.0
		IL_0027: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform[] AILocationSelectorScript::newLocation
		IL_002c: ldarg.0
		IL_002d: ldfld int32 AILocationSelectorScript::id
		IL_0032: conv.i8
		IL_0033: conv.i4
		IL_0034: ldelem [UnityEngine.CoreModule]UnityEngine.Transform
		IL_0039: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_003e: stloc.3
		IL_003f: ldloc.2
		IL_0040: ldloca.s 4
		IL_0042: ldloca.s 3
		IL_0044: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0049: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_004e: ldloca.s 4
		IL_0050: ldloca.s 3
		IL_0052: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0057: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_005c: ldloca.s 4
		IL_005e: ldloca.s 3
		IL_0060: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0065: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_006a: ldloc.s 4
		IL_006c: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0071: ret

		IL_0072: nop
		IL_0073: ldnull
		IL_0074: throw

		IL_0075: nop
		IL_0076: ldnull
		IL_0077: throw
	} // end of method AILocationSelectorScript::QuarterExclusive

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0xb8f0
		// Header size: 12
		// Code size: 23 (0x17)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform[]
		)

		IL_0000: nop
		IL_0001: ldc.i4.s 29
		IL_0003: newarr [UnityEngine.CoreModule]UnityEngine.Transform
		IL_0008: stloc.0
		IL_0009: ldarg.0
		IL_000a: ldloc.0
		IL_000b: stfld class [UnityEngine.CoreModule]UnityEngine.Transform[] AILocationSelectorScript::newLocation
		IL_0010: ldarg.0
		IL_0011: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0016: ret
	} // end of method AILocationSelectorScript::.ctor

} // end of class AILocationSelectorScript
