.class public auto ansi beforefieldinit ClickableTest
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field private class [UnityEngine.CoreModule]UnityEngine.Transform player
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioSource test
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip testHit

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0xc9d4
		// Header size: 12
		// Code size: 56 (0x38)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[1] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[2] bool,
			[3] bool,
			[4] class [UnityEngine.CoreModule]UnityEngine.Transform
		)

		IL_0000: nop
		IL_0001: ldstr "Player"
		IL_0006: call class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.GameObject::FindGameObjectWithTag(string)
		IL_000b: stloc.0
		IL_000c: ldloc.0
		IL_000d: stloc.1
		IL_000e: br.s IL_0011

		IL_0010: nop

		IL_0011: nop
		IL_0012: ldloc.1
		IL_0013: ldnull
		IL_0014: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Inequality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0019: stloc.2
		IL_001a: ldloc.2
		IL_001b: stloc.3
		IL_001c: ldloc.2
		IL_001d: ldc.i4.0
		IL_001e: ceq
		IL_0020: brtrue.s IL_0033

		IL_0022: nop
		IL_0023: ldloc.1
		IL_0024: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
		IL_0029: stloc.s 4
		IL_002b: ldarg.0
		IL_002c: ldloc.s 4
		IL_002e: stfld class [UnityEngine.CoreModule]UnityEngine.Transform ClickableTest::player

		IL_0033: nop
		IL_0034: ret

		IL_0035: nop
		IL_0036: ldnull
		IL_0037: throw
	} // end of method ClickableTest::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0xca18
		// Header size: 12
		// Code size: 204 (0xcc)
		.maxstack 3
		.locals init (
			[0] bool,
			[1] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[3] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[4] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[5] valuetype [UnityEngine.CoreModule]UnityEngine.Ray,
			[6] int64,
			[7] int64,
			[8] valuetype [UnityEngine.CoreModule]UnityEngine.Ray,
			[9] valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit,
			[10] bool,
			[11] valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit,
			[12] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[13] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[14] string,
			[15] bool,
			[16] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[17] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[18] class [UnityEngine.CoreModule]UnityEngine.Transform
		)

		IL_0000: nop
		IL_0001: ldc.i4.0
		IL_0002: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetMouseButtonDown(int32)
		IL_0007: stloc.0
		IL_0008: nop
		IL_0009: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_000e: stloc.1
		IL_000f: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.InputLegacyModule]UnityEngine.Input::get_mousePosition()
		IL_0014: stloc.2
		IL_0015: ldloc.1
		IL_0016: stloc.3
		IL_0017: nop
		IL_0018: ldloc.3
		IL_0019: ldloca.s 4
		IL_001b: ldloca.s 2
		IL_001d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0022: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0027: ldloca.s 4
		IL_0029: ldloca.s 2
		IL_002b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0030: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0035: ldloca.s 4
		IL_0037: ldloca.s 2
		IL_0039: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_003e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0043: ldloc.s 4
		IL_0045: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Ray [UnityEngine.CoreModule]UnityEngine.Camera::ScreenPointToRay(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_004a: stloc.s 5
		IL_004c: ldstr "struct_chunk(LocalExpr+0x0, 16)"
		IL_0051: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0056: stloc.s 6
		IL_0058: ldstr "struct_chunk(LocalExpr+0x10, 8)"
		IL_005d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0062: stloc.s 7
		IL_0064: br.s IL_0067

		IL_0066: nop

		IL_0067: nop
		IL_0068: ldloca.s 8
		IL_006a: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Ray::m_Origin
		IL_006f: ldloc.s 6
		IL_0071: box [mscorlib]System.Int64
		IL_0076: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_007b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0080: ldloc.s 8
		IL_0082: ldloca.s 9
		IL_0084: call bool [UnityEngine.PhysicsModule]UnityEngine.Physics::Raycast(valuetype [UnityEngine.CoreModule]UnityEngine.Ray, valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit&)
		IL_0089: stloc.s 10
		IL_008b: nop
		IL_008c: ldloca.s 11
		IL_008e: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_0093: stloc.s 12
		IL_0095: ldloc.s 12
		IL_0097: stloc.s 13
		IL_0099: nop
		IL_009a: ldloc.s 13
		IL_009c: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Component::get_tag()
		IL_00a1: stloc.s 14
		IL_00a3: ldloc.s 14
		IL_00a5: ldstr "Notebook"
		IL_00aa: call bool [mscorlib]System.String::Equals(string, string)
		IL_00af: stloc.s 15
		IL_00b1: ldarg.0
		IL_00b2: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform ClickableTest::player
		IL_00b7: stloc.s 16
		IL_00b9: nop
		IL_00ba: ldloc.s 16
		IL_00bc: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_00c1: stloc.s 17
		IL_00c3: ldarg.0
		IL_00c4: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_00c9: stloc.s 18
		IL_00cb: ret
	} // end of method ClickableTest::Update

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2510
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method ClickableTest::.ctor

} // end of class ClickableTest
