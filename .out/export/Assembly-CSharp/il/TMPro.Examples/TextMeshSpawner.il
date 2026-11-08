.class public auto ansi beforefieldinit TMPro.Examples.TextMeshSpawner
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public int32 SpawnType
	.field public int32 NumberOfNPC
	.field public class [UnityEngine.TextRenderingModule]UnityEngine.Font TheFont
	.field private class TMPro.Examples.TextMeshProFloatingText floatingText_Script

	// Methods
	.method private hidebysig 
		instance void Awake () cil managed 
	{
		// Method begins at RVA 0x2954
		// Header size: 1
		// Code size: 2 (0x2)
		.maxstack 8

		IL_0000: nop
		IL_0001: ret
	} // end of method TextMeshSpawner::Awake

	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x16ca8
		// Header size: 12
		// Code size: 480 (0x1e0)
		.maxstack 3
		.locals init (
			[0] int64,
			[1] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[2] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[3] float32,
			[4] float32,
			[5] float32,
			[6] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[7] class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh,
			[8] class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh,
			[9] class [UnityEngine.CoreModule]UnityEngine.Renderer,
			[10] class [UnityEngine.CoreModule]UnityEngine.Renderer,
			[11] class [UnityEngine.CoreModule]UnityEngine.Material,
			[12] class [UnityEngine.CoreModule]UnityEngine.Material,
			[13] valuetype [UnityEngine.CoreModule]UnityEngine.Color,
			[14] class TMPro.Examples.TextMeshProFloatingText,
			[15] class TMPro.Examples.TextMeshProFloatingText,
			[16] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[17] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[18] float32,
			[19] float32,
			[20] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[21] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[22] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[23] class TMPro.Examples.TextMeshProFloatingText,
			[24] class TMPro.Examples.TextMeshProFloatingText,
			[25] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld int32 TMPro.Examples.TextMeshSpawner::NumberOfNPC
		IL_0007: ldc.i4.0
		IL_0008: cgt
		IL_000a: ldc.i4.0
		IL_000b: ceq
		IL_000d: brtrue IL_01db

		IL_0012: nop
		IL_0013: ldstr "global_13EAC90"
		IL_0018: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_001d: stloc.0
		// loop start (head: IL_001e)
			IL_001e: nop
			IL_001f: ldarg.0
			IL_0020: ldfld int32 TMPro.Examples.TextMeshSpawner::SpawnType
			IL_0025: ldc.i4.0
			IL_0026: ceq
			IL_0028: brtrue IL_012c

			IL_002d: nop
			IL_002e: newobj instance void [UnityEngine.CoreModule]UnityEngine.GameObject::.ctor()
			IL_0033: stloc.1
			IL_0034: ldloc.1
			IL_0035: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
			IL_003a: stloc.2
			IL_003b: ldc.r4 -95
			IL_0040: ldc.r4 95
			IL_0045: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
			IL_004a: stloc.3
			IL_004b: ldc.r4 -95
			IL_0050: ldc.r4 95
			IL_0055: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
			IL_005a: stloc.s 4
			IL_005c: ldloc.s 4
			IL_005e: stloc.s 5
			IL_0060: ldloc.2
			IL_0061: ldloca.s 6
			IL_0063: ldloc.3
			IL_0064: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_0069: ldloca.s 6
			IL_006b: ldc.r4 0.5
			IL_0070: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_0075: ldloca.s 6
			IL_0077: ldloc.s 5
			IL_0079: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_007e: ldloc.s 6
			IL_0080: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
			IL_0085: ldloc.1
			IL_0086: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh>()
			IL_008b: stloc.s 7
			IL_008d: ldloc.s 7
			IL_008f: stloc.s 8
			IL_0091: ldloc.s 8
			IL_0093: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.CoreModule]UnityEngine.Renderer>()
			IL_0098: stloc.s 9
			IL_009a: ldloc.s 9
			IL_009c: stloc.s 10
			IL_009e: ldarg.0
			IL_009f: ldfld class [UnityEngine.TextRenderingModule]UnityEngine.Font TMPro.Examples.TextMeshSpawner::TheFont
			IL_00a4: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Material [UnityEngine.TextRenderingModule]UnityEngine.Font::get_material()
			IL_00a9: stloc.s 11
			IL_00ab: ldloc.s 11
			IL_00ad: stloc.s 12
			IL_00af: ldloc.s 9
			IL_00b1: ldloc.s 12
			IL_00b3: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Renderer::SetMaterial(class [UnityEngine.CoreModule]UnityEngine.Material)
			IL_00b8: ldloc.s 7
			IL_00ba: ldarg.0
			IL_00bb: ldfld class [UnityEngine.TextRenderingModule]UnityEngine.Font TMPro.Examples.TextMeshSpawner::TheFont
			IL_00c0: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_font(class [UnityEngine.TextRenderingModule]UnityEngine.Font)
			IL_00c5: ldloc.s 7
			IL_00c7: ldc.i4.7
			IL_00c8: box [mscorlib]System.Int32
			IL_00cd: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.TextRenderingModule]UnityEngine.TextAnchor>(object)
			IL_00d2: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_anchor(valuetype [UnityEngine.TextRenderingModule]UnityEngine.TextAnchor)
			IL_00d7: ldloc.s 7
			IL_00d9: ldc.i4.s 96
			IL_00db: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_fontSize(int32)
			IL_00e0: ldloc.s 7
			IL_00e2: ldloca.s 13
			IL_00e4: ldloc.0
			IL_00e5: box [mscorlib]System.Int64
			IL_00ea: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
			IL_00ef: stfld float32 [UnityEngine.CoreModule]UnityEngine.Color::r
			IL_00f4: ldloc.s 13
			IL_00f6: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_color(valuetype [UnityEngine.CoreModule]UnityEngine.Color)
			IL_00fb: ldloc.s 7
			IL_00fd: ldstr "!"
			IL_0102: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_text(string)
			IL_0107: ldloc.1
			IL_0108: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class TMPro.Examples.TextMeshProFloatingText>()
			IL_010d: stloc.s 14
			IL_010f: ldarg.0
			IL_0110: ldloc.s 14
			IL_0112: stfld class TMPro.Examples.TextMeshProFloatingText TMPro.Examples.TextMeshSpawner::floatingText_Script
			IL_0117: ldarg.0
			IL_0118: ldfld class TMPro.Examples.TextMeshProFloatingText TMPro.Examples.TextMeshSpawner::floatingText_Script
			IL_011d: stloc.s 15
			IL_011f: ldloc.s 15
			IL_0121: ldc.i4.1
			IL_0122: stfld int32 TMPro.Examples.TextMeshProFloatingText::SpawnType
			IL_0127: br IL_01c4

			IL_012c: nop
			IL_012d: newobj instance void [UnityEngine.CoreModule]UnityEngine.GameObject::.ctor()
			IL_0132: stloc.s 16
			IL_0134: ldloc.s 16
			IL_0136: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
			IL_013b: stloc.s 17
			IL_013d: ldc.r4 -95
			IL_0142: ldc.r4 95
			IL_0147: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
			IL_014c: stloc.s 18
			IL_014e: ldc.r4 -95
			IL_0153: ldc.r4 95
			IL_0158: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
			IL_015d: stloc.s 19
			IL_015f: ldloc.s 19
			IL_0161: stloc.s 5
			IL_0163: ldloc.s 17
			IL_0165: ldloca.s 20
			IL_0167: ldloc.s 18
			IL_0169: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_016e: ldloca.s 20
			IL_0170: ldc.r4 0.5
			IL_0175: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_017a: ldloca.s 20
			IL_017c: ldloc.s 5
			IL_017e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_0183: ldloc.s 20
			IL_0185: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
			IL_018a: ldloc.s 16
			IL_018c: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class [Unity.TextMeshPro]TMPro.TextMeshPro>()
			IL_0191: stloc.s 21
			IL_0193: ldloc.s 21
			IL_0195: stloc.s 22
			IL_0197: ldloc.s 22
			IL_0199: ldc.r4 96
			IL_019e: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_fontSize(float32)
			IL_01a3: ldloc.s 16
			IL_01a5: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class TMPro.Examples.TextMeshProFloatingText>()
			IL_01aa: stloc.s 23
			IL_01ac: ldarg.0
			IL_01ad: ldloc.s 23
			IL_01af: stfld class TMPro.Examples.TextMeshProFloatingText TMPro.Examples.TextMeshSpawner::floatingText_Script
			IL_01b4: ldarg.0
			IL_01b5: ldfld class TMPro.Examples.TextMeshProFloatingText TMPro.Examples.TextMeshSpawner::floatingText_Script
			IL_01ba: stloc.s 24
			IL_01bc: ldloc.s 24
			IL_01be: ldc.i4.0
			IL_01bf: stfld int32 TMPro.Examples.TextMeshProFloatingText::SpawnType

			IL_01c4: nop
			IL_01c5: ldc.i4.0
			IL_01c6: ldc.i4.1
			IL_01c7: add
			IL_01c8: stloc.s 25
			IL_01ca: ldc.i4.0
			IL_01cb: ldc.i4.1
			IL_01cc: add
			IL_01cd: ldarg.0
			IL_01ce: ldfld int32 TMPro.Examples.TextMeshSpawner::NumberOfNPC
			IL_01d3: clt
			IL_01d5: brtrue IL_001e
		// end loop
		IL_01da: nop

		IL_01db: nop
		IL_01dc: ret

		IL_01dd: nop
		IL_01de: ldnull
		IL_01df: throw
	} // end of method TextMeshSpawner::Start

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2957
		// Header size: 1
		// Code size: 16 (0x10)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i4.s 12
		IL_0004: stfld int32 TMPro.Examples.TextMeshSpawner::NumberOfNPC
		IL_0009: ldarg.0
		IL_000a: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_000f: ret
	} // end of method TextMeshSpawner::.ctor

} // end of class TMPro.Examples.TextMeshSpawner
