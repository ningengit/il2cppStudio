.class public auto ansi beforefieldinit PickupAnimationScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field private class [UnityEngine.CoreModule]UnityEngine.Transform itemPosition

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x11594
		// Header size: 12
		// Code size: 16 (0x10)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.CoreModule]UnityEngine.Transform>()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.CoreModule]UnityEngine.Transform PickupAnimationScript::itemPosition
		IL_000f: ret
	} // end of method PickupAnimationScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x115b0
		// Header size: 12
		// Code size: 126 (0x7e)
		.maxstack 5
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] int32,
			[2] int64,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PickupAnimationScript::itemPosition
		IL_0007: stloc.0
		IL_0008: call int32 [UnityEngine.CoreModule]UnityEngine.Time::get_frameCount()
		IL_000d: stloc.1
		IL_000e: ldstr "sub_20D1D0"
		IL_0013: ldc.i8 0
		IL_001c: ldc.i8 0
		IL_0025: ldc.i8 0
		IL_002e: ldc.i8 0
		IL_0037: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_003c: stloc.2
		IL_003d: ldloc.0
		IL_003e: ldloca.s 3
		IL_0040: ldc.r4 0.0
		IL_0045: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_004a: ldloca.s 3
		IL_004c: ldstr "xmm0"
		IL_0051: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0056: conv.r4
		IL_0057: ldc.r4 0.5
		IL_005c: mul
		IL_005d: ldc.r4 1
		IL_0062: add
		IL_0063: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0068: ldloca.s 3
		IL_006a: ldc.r4 0.0
		IL_006f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0074: ldloc.3
		IL_0075: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_localPosition(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_007a: ret

		IL_007b: nop
		IL_007c: ldnull
		IL_007d: throw
	} // end of method PickupAnimationScript::Update

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2684
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method PickupAnimationScript::.ctor

} // end of class PickupAnimationScript
