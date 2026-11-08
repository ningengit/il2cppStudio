.class public auto ansi beforefieldinit EndlessNotebookScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public float32 openingDistance
	.field public class GameControllerScript gc
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform player
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject learningGame

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x10fa4
		// Header size: 12
		// Code size: 55 (0x37)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[1] class GameControllerScript,
			[2] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[3] class [UnityEngine.CoreModule]UnityEngine.Transform
		)

		IL_0000: nop
		IL_0001: ldstr "Game Controller"
		IL_0006: call class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.GameObject::Find(string)
		IL_000b: stloc.0
		IL_000c: ldloc.0
		IL_000d: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::GetComponent<class GameControllerScript>()
		IL_0012: stloc.1
		IL_0013: ldarg.0
		IL_0014: ldloc.1
		IL_0015: stfld class GameControllerScript EndlessNotebookScript::gc
		IL_001a: ldstr "Player"
		IL_001f: call class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.GameObject::Find(string)
		IL_0024: stloc.2
		IL_0025: ldloc.2
		IL_0026: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::GetComponent<class [UnityEngine.CoreModule]UnityEngine.Transform>()
		IL_002b: stloc.3
		IL_002c: ldarg.0
		IL_002d: ldloc.3
		IL_002e: stfld class [UnityEngine.CoreModule]UnityEngine.Transform EndlessNotebookScript::player
		IL_0033: ret

		IL_0034: nop
		IL_0035: ldnull
		IL_0036: throw
	} // end of method EndlessNotebookScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x10fe8
		// Header size: 12
		// Code size: 236 (0xec)
		.maxstack 3
		.locals init (
			[0] bool,
			[1] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[2] int32,
			[3] int32,
			[4] float32,
			[5] float32,
			[6] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[7] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[8] valuetype [UnityEngine.CoreModule]UnityEngine.Ray,
			[9] int64,
			[10] int64,
			[11] valuetype [UnityEngine.CoreModule]UnityEngine.Ray,
			[12] valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit,
			[13] bool,
			[14] valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit,
			[15] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[16] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[17] string,
			[18] bool,
			[19] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[20] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[21] class [UnityEngine.CoreModule]UnityEngine.Transform
		)

		IL_0000: nop
		IL_0001: ldc.i4.0
		IL_0002: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetMouseButtonDown(int32)
		IL_0007: stloc.0
		IL_0008: nop
		IL_0009: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_000e: stloc.1
		IL_000f: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_width()
		IL_0014: stloc.2
		IL_0015: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_height()
		IL_001a: stloc.3
		IL_001b: ldloc.2
		IL_001c: ldstr "rdx"
		IL_0021: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0026: conv.i4
		IL_0027: sub
		IL_0028: ldc.i4.1
		IL_0029: shr
		IL_002a: conv.r4
		IL_002b: stloc.s 4
		IL_002d: ldloc.3
		IL_002e: ldstr "rdx"
		IL_0033: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0038: conv.i4
		IL_0039: sub
		IL_003a: ldc.i4.1
		IL_003b: shr
		IL_003c: conv.r4
		IL_003d: stloc.s 5
		IL_003f: ldloc.1
		IL_0040: stloc.s 6
		IL_0042: nop
		IL_0043: ldloc.s 6
		IL_0045: ldloca.s 7
		IL_0047: ldloc.s 4
		IL_0049: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_004e: ldloca.s 7
		IL_0050: ldloc.s 5
		IL_0052: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0057: ldloca.s 7
		IL_0059: ldc.r4 0.0
		IL_005e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0063: ldloc.s 7
		IL_0065: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Ray [UnityEngine.CoreModule]UnityEngine.Camera::ScreenPointToRay(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_006a: stloc.s 8
		IL_006c: ldstr "struct_chunk(LocalExpr+0x0, 16)"
		IL_0071: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0076: stloc.s 9
		IL_0078: ldstr "struct_chunk(LocalExpr+0x10, 8)"
		IL_007d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0082: stloc.s 10
		IL_0084: br.s IL_0087

		IL_0086: nop

		IL_0087: nop
		IL_0088: ldloca.s 11
		IL_008a: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Ray::m_Origin
		IL_008f: ldloc.s 9
		IL_0091: box [mscorlib]System.Int64
		IL_0096: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_009b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00a0: ldloc.s 11
		IL_00a2: ldloca.s 12
		IL_00a4: call bool [UnityEngine.PhysicsModule]UnityEngine.Physics::Raycast(valuetype [UnityEngine.CoreModule]UnityEngine.Ray, valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit&)
		IL_00a9: stloc.s 13
		IL_00ab: nop
		IL_00ac: ldloca.s 14
		IL_00ae: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_00b3: stloc.s 15
		IL_00b5: ldloc.s 15
		IL_00b7: stloc.s 16
		IL_00b9: nop
		IL_00ba: ldloc.s 16
		IL_00bc: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Component::get_tag()
		IL_00c1: stloc.s 17
		IL_00c3: ldloc.s 17
		IL_00c5: ldstr "Notebook"
		IL_00ca: call bool [mscorlib]System.String::Equals(string, string)
		IL_00cf: stloc.s 18
		IL_00d1: ldarg.0
		IL_00d2: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform EndlessNotebookScript::player
		IL_00d7: stloc.s 19
		IL_00d9: nop
		IL_00da: ldloc.s 19
		IL_00dc: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_00e1: stloc.s 20
		IL_00e3: ldarg.0
		IL_00e4: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_00e9: stloc.s 21
		IL_00eb: ret
	} // end of method EndlessNotebookScript::Update

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2668
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method EndlessNotebookScript::.ctor

} // end of class EndlessNotebookScript
