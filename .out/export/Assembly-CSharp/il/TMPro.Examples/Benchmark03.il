.class public auto ansi beforefieldinit TMPro.Examples.Benchmark03
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public int32 SpawnType
	.field public int32 NumberOfNPC
	.field public class [UnityEngine.TextRenderingModule]UnityEngine.Font TheFont

	// Methods
	.method private hidebysig 
		instance void Awake () cil managed 
	{
		// Method begins at RVA 0x28c0
		// Header size: 1
		// Code size: 2 (0x2)
		.maxstack 8

		IL_0000: nop
		IL_0001: ret
	} // end of method Benchmark03::Awake

	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x14d90
		// Header size: 12
		// Code size: 354 (0x162)
		.maxstack 3
		.locals init (
			[0] int64,
			[1] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[2] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[3] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[4] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[5] class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh,
			[6] class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh,
			[7] class [UnityEngine.CoreModule]UnityEngine.Renderer,
			[8] class [UnityEngine.CoreModule]UnityEngine.Renderer,
			[9] class [UnityEngine.CoreModule]UnityEngine.Material,
			[10] class [UnityEngine.CoreModule]UnityEngine.Material,
			[11] valuetype [UnityEngine.CoreModule]UnityEngine.Color,
			[12] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[13] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[14] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[15] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[16] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[17] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[18] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld int32 TMPro.Examples.Benchmark03::NumberOfNPC
		IL_0007: ldc.i4.0
		IL_0008: cgt
		IL_000a: ldc.i4.0
		IL_000b: ceq
		IL_000d: brtrue IL_015d

		IL_0012: nop
		IL_0013: ldstr "global_13EAC90"
		IL_0018: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_001d: stloc.0
		// loop start (head: IL_001e)
			IL_001e: nop
			IL_001f: ldarg.0
			IL_0020: ldfld int32 TMPro.Examples.Benchmark03::SpawnType
			IL_0025: ldc.i4.0
			IL_0026: ceq
			IL_0028: brtrue IL_00e1

			IL_002d: nop
			IL_002e: newobj instance void [UnityEngine.CoreModule]UnityEngine.GameObject::.ctor()
			IL_0033: stloc.1
			IL_0034: ldloc.1
			IL_0035: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
			IL_003a: stloc.2
			IL_003b: ldloc.2
			IL_003c: stloc.3
			IL_003d: ldloc.3
			IL_003e: ldloca.s 4
			IL_0040: ldc.r4 0.0
			IL_0045: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_004a: ldloca.s 4
			IL_004c: ldc.r4 0.0
			IL_0051: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_0056: ldloc.s 4
			IL_0058: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
			IL_005d: ldloc.1
			IL_005e: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh>()
			IL_0063: stloc.s 5
			IL_0065: ldloc.s 5
			IL_0067: stloc.s 6
			IL_0069: ldloc.s 6
			IL_006b: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.CoreModule]UnityEngine.Renderer>()
			IL_0070: stloc.s 7
			IL_0072: ldloc.s 7
			IL_0074: stloc.s 8
			IL_0076: ldarg.0
			IL_0077: ldfld class [UnityEngine.TextRenderingModule]UnityEngine.Font TMPro.Examples.Benchmark03::TheFont
			IL_007c: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Material [UnityEngine.TextRenderingModule]UnityEngine.Font::get_material()
			IL_0081: stloc.s 9
			IL_0083: ldloc.s 9
			IL_0085: stloc.s 10
			IL_0087: ldloc.s 7
			IL_0089: ldloc.s 10
			IL_008b: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Renderer::SetMaterial(class [UnityEngine.CoreModule]UnityEngine.Material)
			IL_0090: ldloc.s 5
			IL_0092: ldarg.0
			IL_0093: ldfld class [UnityEngine.TextRenderingModule]UnityEngine.Font TMPro.Examples.Benchmark03::TheFont
			IL_0098: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_font(class [UnityEngine.TextRenderingModule]UnityEngine.Font)
			IL_009d: ldloc.s 5
			IL_009f: ldc.i4.4
			IL_00a0: box [mscorlib]System.Int32
			IL_00a5: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.TextRenderingModule]UnityEngine.TextAnchor>(object)
			IL_00aa: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_anchor(valuetype [UnityEngine.TextRenderingModule]UnityEngine.TextAnchor)
			IL_00af: ldloc.s 5
			IL_00b1: ldc.i4.s 96
			IL_00b3: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_fontSize(int32)
			IL_00b8: ldloc.s 5
			IL_00ba: ldloca.s 11
			IL_00bc: ldloc.0
			IL_00bd: box [mscorlib]System.Int64
			IL_00c2: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
			IL_00c7: stfld float32 [UnityEngine.CoreModule]UnityEngine.Color::r
			IL_00cc: ldloc.s 11
			IL_00ce: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_color(valuetype [UnityEngine.CoreModule]UnityEngine.Color)
			IL_00d3: ldloc.s 5
			IL_00d5: ldstr "@"
			IL_00da: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_text(string)
			IL_00df: br.s IL_0146

			IL_00e1: nop
			IL_00e2: newobj instance void [UnityEngine.CoreModule]UnityEngine.GameObject::.ctor()
			IL_00e7: stloc.s 12
			IL_00e9: ldloc.s 12
			IL_00eb: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
			IL_00f0: stloc.s 13
			IL_00f2: ldloc.s 13
			IL_00f4: stloc.s 14
			IL_00f6: ldloc.s 14
			IL_00f8: ldloca.s 15
			IL_00fa: ldc.r4 0.0
			IL_00ff: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_0104: ldloca.s 15
			IL_0106: ldc.r4 0.0
			IL_010b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_0110: ldloc.s 15
			IL_0112: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
			IL_0117: ldloc.s 12
			IL_0119: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class [Unity.TextMeshPro]TMPro.TextMeshPro>()
			IL_011e: stloc.s 16
			IL_0120: ldloc.s 16
			IL_0122: stloc.s 17
			IL_0124: ldloc.s 17
			IL_0126: ldc.i4 514
			IL_012b: box [mscorlib]System.Int32
			IL_0130: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions>(object)
			IL_0135: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_alignment(valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions)
			IL_013a: ldloc.s 16
			IL_013c: ldc.r4 96
			IL_0141: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_fontSize(float32)

			IL_0146: nop
			IL_0147: ldc.i4.0
			IL_0148: ldc.i4.1
			IL_0149: add
			IL_014a: stloc.s 18
			IL_014c: ldc.i4.0
			IL_014d: ldc.i4.1
			IL_014e: add
			IL_014f: ldarg.0
			IL_0150: ldfld int32 TMPro.Examples.Benchmark03::NumberOfNPC
			IL_0155: clt
			IL_0157: brtrue IL_001e
		// end loop
		IL_015c: nop

		IL_015d: nop
		IL_015e: ret

		IL_015f: nop
		IL_0160: ldnull
		IL_0161: throw
	} // end of method Benchmark03::Start

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x28c3
		// Header size: 1
		// Code size: 16 (0x10)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i4.s 12
		IL_0004: stfld int32 TMPro.Examples.Benchmark03::NumberOfNPC
		IL_0009: ldarg.0
		IL_000a: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_000f: ret
	} // end of method Benchmark03::.ctor

} // end of class TMPro.Examples.Benchmark03
