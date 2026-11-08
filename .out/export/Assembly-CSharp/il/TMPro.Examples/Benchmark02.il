.class public auto ansi beforefieldinit TMPro.Examples.Benchmark02
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public int32 SpawnType
	.field public int32 NumberOfNPC
	.field private class TMPro.Examples.TextMeshProFloatingText floatingText_Script

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x14a30
		// Header size: 12
		// Code size: 851 (0x353)
		.maxstack 3
		.locals init (
			[0] int64,
			[1] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[2] class [UnityEngine.UIModule]UnityEngine.Canvas,
			[3] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[4] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[5] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[6] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[7] float32,
			[8] float32,
			[9] float32,
			[10] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[11] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[12] class [Unity.TextMeshPro]TMPro.TextMeshProUGUI,
			[13] class [UnityEngine.CoreModule]UnityEngine.RectTransform,
			[14] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[15] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[16] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[17] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[18] float32,
			[19] float32,
			[20] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[21] class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh,
			[22] class [UnityEngine.TextRenderingModule]UnityEngine.Font,
			[23] class [UnityEngine.TextRenderingModule]UnityEngine.Font,
			[24] class [UnityEngine.CoreModule]UnityEngine.Renderer,
			[25] class [UnityEngine.TextRenderingModule]UnityEngine.Font,
			[26] class [UnityEngine.CoreModule]UnityEngine.Material,
			[27] class [UnityEngine.CoreModule]UnityEngine.Material,
			[28] valuetype [UnityEngine.CoreModule]UnityEngine.Color,
			[29] class TMPro.Examples.TextMeshProFloatingText,
			[30] class TMPro.Examples.TextMeshProFloatingText,
			[31] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[32] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[33] float32,
			[34] float32,
			[35] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[36] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[37] class [UnityEngine.CoreModule]UnityEngine.RectTransform,
			[38] class TMPro.Examples.TextMeshProFloatingText,
			[39] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld int32 TMPro.Examples.Benchmark02::NumberOfNPC
		IL_0007: ldc.i4.0
		IL_0008: cgt
		IL_000a: ldc.i4.0
		IL_000b: ceq
		IL_000d: brtrue IL_034e

		IL_0012: nop
		IL_0013: ldstr "global_13EAC90"
		IL_0018: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_001d: stloc.0
		// loop start (head: IL_001e)
			IL_001e: nop
			IL_001f: ldarg.0
			IL_0020: ldfld int32 TMPro.Examples.Benchmark02::SpawnType
			IL_0025: ldc.i4.0
			IL_0026: ceq
			IL_0028: brtrue IL_025c

			IL_002d: nop
			IL_002e: ldarg.0
			IL_002f: ldfld int32 TMPro.Examples.Benchmark02::SpawnType
			IL_0034: ldc.i4.1
			IL_0035: ceq
			IL_0037: brtrue IL_0148

			IL_003c: nop
			IL_003d: ldarg.0
			IL_003e: ldfld int32 TMPro.Examples.Benchmark02::SpawnType
			IL_0043: ldc.i4.2
			IL_0044: ceq
			IL_0046: ldc.i4.0
			IL_0047: ceq
			IL_0049: brtrue IL_0337

			IL_004e: nop
			IL_004f: newobj instance void [UnityEngine.CoreModule]UnityEngine.GameObject::.ctor()
			IL_0054: stloc.1
			IL_0055: ldloc.1
			IL_0056: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class [UnityEngine.UIModule]UnityEngine.Canvas>()
			IL_005b: stloc.2
			IL_005c: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
			IL_0061: stloc.3
			IL_0062: ldloc.2
			IL_0063: ldloc.3
			IL_0064: callvirt instance void [UnityEngine.UIModule]UnityEngine.Canvas::set_worldCamera(class [UnityEngine.CoreModule]UnityEngine.Camera)
			IL_0069: ldloc.1
			IL_006a: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
			IL_006f: stloc.s 4
			IL_0071: ldloc.s 4
			IL_0073: ldloca.s 5
			IL_0075: ldc.r4 0.1
			IL_007a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_007f: ldloca.s 5
			IL_0081: ldc.r4 0.1
			IL_0086: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_008b: ldloca.s 5
			IL_008d: ldc.r4 0.1
			IL_0092: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_0097: ldloc.s 5
			IL_0099: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_localScale(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
			IL_009e: ldloc.1
			IL_009f: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
			IL_00a4: stloc.s 6
			IL_00a6: ldc.r4 -95
			IL_00ab: ldc.r4 95
			IL_00b0: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
			IL_00b5: stloc.s 7
			IL_00b7: ldc.r4 -95
			IL_00bc: ldc.r4 95
			IL_00c1: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
			IL_00c6: stloc.s 8
			IL_00c8: ldloc.s 8
			IL_00ca: stloc.s 9
			IL_00cc: ldloc.s 6
			IL_00ce: ldloca.s 10
			IL_00d0: ldloc.s 7
			IL_00d2: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_00d7: ldloca.s 10
			IL_00d9: ldc.r4 5
			IL_00de: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_00e3: ldloca.s 10
			IL_00e5: ldloc.s 9
			IL_00e7: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_00ec: ldloc.s 10
			IL_00ee: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
			IL_00f3: newobj instance void [UnityEngine.CoreModule]UnityEngine.GameObject::.ctor()
			IL_00f8: stloc.s 11
			IL_00fa: ldloc.s 11
			IL_00fc: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class [Unity.TextMeshPro]TMPro.TextMeshProUGUI>()
			IL_0101: stloc.s 12
			IL_0103: ldloc.s 12
			IL_0105: callvirt instance class [UnityEngine.CoreModule]UnityEngine.RectTransform [Unity.TextMeshPro]TMPro.TMP_Text::get_rectTransform()
			IL_010a: stloc.s 13
			IL_010c: ldloc.1
			IL_010d: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
			IL_0112: stloc.s 14
			IL_0114: ldloc.s 13
			IL_0116: ldloc.s 14
			IL_0118: ldc.i4.0
			IL_0119: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::SetParent(class [UnityEngine.CoreModule]UnityEngine.Transform, bool)
			IL_011e: ldloc.s 12
			IL_0120: ldc.i4 1026
			IL_0125: box [mscorlib]System.Int32
			IL_012a: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions>(object)
			IL_012f: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_alignment(valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions)
			IL_0134: ldloc.s 12
			IL_0136: ldc.r4 96
			IL_013b: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_fontSize(float32)
			IL_0140: ldloc.1
			IL_0141: stloc.s 15
			IL_0143: br IL_0315

			IL_0148: nop
			IL_0149: newobj instance void [UnityEngine.CoreModule]UnityEngine.GameObject::.ctor()
			IL_014e: stloc.s 16
			IL_0150: ldloc.s 16
			IL_0152: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
			IL_0157: stloc.s 17
			IL_0159: ldc.r4 -95
			IL_015e: ldc.r4 95
			IL_0163: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
			IL_0168: stloc.s 18
			IL_016a: ldc.r4 -95
			IL_016f: ldc.r4 95
			IL_0174: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
			IL_0179: stloc.s 19
			IL_017b: ldloc.s 19
			IL_017d: stloc.s 9
			IL_017f: ldloc.s 17
			IL_0181: ldloca.s 20
			IL_0183: ldloc.s 18
			IL_0185: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_018a: ldloca.s 20
			IL_018c: ldc.r4 0.25
			IL_0191: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_0196: ldloca.s 20
			IL_0198: ldloc.s 9
			IL_019a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_019f: ldloc.s 20
			IL_01a1: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
			IL_01a6: ldloc.s 16
			IL_01a8: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class [UnityEngine.TextRenderingModule]UnityEngine.TextMesh>()
			IL_01ad: stloc.s 21
			IL_01af: ldstr "Fonts/ARIAL"
			IL_01b4: call !!0 [UnityEngine.CoreModule]UnityEngine.Resources::Load<class [UnityEngine.TextRenderingModule]UnityEngine.Font>(string)
			IL_01b9: stloc.s 22
			IL_01bb: ldloc.s 22
			IL_01bd: stloc.s 23
			IL_01bf: ldloc.s 21
			IL_01c1: ldloc.s 23
			IL_01c3: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_font(class [UnityEngine.TextRenderingModule]UnityEngine.Font)
			IL_01c8: ldloc.s 21
			IL_01ca: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.CoreModule]UnityEngine.Renderer>()
			IL_01cf: stloc.s 24
			IL_01d1: ldloc.s 21
			IL_01d3: callvirt instance class [UnityEngine.TextRenderingModule]UnityEngine.Font [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::get_font()
			IL_01d8: stloc.s 25
			IL_01da: ldloc.s 25
			IL_01dc: stloc.s 23
			IL_01de: ldloc.s 23
			IL_01e0: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Material [UnityEngine.TextRenderingModule]UnityEngine.Font::get_material()
			IL_01e5: stloc.s 26
			IL_01e7: ldloc.s 26
			IL_01e9: stloc.s 27
			IL_01eb: ldloc.s 24
			IL_01ed: ldloc.s 27
			IL_01ef: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Renderer::SetMaterial(class [UnityEngine.CoreModule]UnityEngine.Material)
			IL_01f4: ldloc.s 21
			IL_01f6: ldc.i4.7
			IL_01f7: box [mscorlib]System.Int32
			IL_01fc: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.TextRenderingModule]UnityEngine.TextAnchor>(object)
			IL_0201: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_anchor(valuetype [UnityEngine.TextRenderingModule]UnityEngine.TextAnchor)
			IL_0206: ldloc.s 21
			IL_0208: ldc.i4.s 96
			IL_020a: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_fontSize(int32)
			IL_020f: ldloc.s 21
			IL_0211: ldloca.s 28
			IL_0213: ldloc.0
			IL_0214: box [mscorlib]System.Int64
			IL_0219: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
			IL_021e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Color::r
			IL_0223: ldloc.s 28
			IL_0225: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_color(valuetype [UnityEngine.CoreModule]UnityEngine.Color)
			IL_022a: ldloc.s 21
			IL_022c: ldstr "!"
			IL_0231: callvirt instance void [UnityEngine.TextRenderingModule]UnityEngine.TextMesh::set_text(string)
			IL_0236: ldloc.s 16
			IL_0238: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class TMPro.Examples.TextMeshProFloatingText>()
			IL_023d: stloc.s 29
			IL_023f: ldarg.0
			IL_0240: ldloc.s 29
			IL_0242: stfld class TMPro.Examples.TextMeshProFloatingText TMPro.Examples.Benchmark02::floatingText_Script
			IL_0247: ldarg.0
			IL_0248: ldfld class TMPro.Examples.TextMeshProFloatingText TMPro.Examples.Benchmark02::floatingText_Script
			IL_024d: stloc.s 30
			IL_024f: ldloc.s 30
			IL_0251: ldc.i4.1
			IL_0252: stfld int32 TMPro.Examples.TextMeshProFloatingText::SpawnType
			IL_0257: br IL_0337

			IL_025c: nop
			IL_025d: newobj instance void [UnityEngine.CoreModule]UnityEngine.GameObject::.ctor()
			IL_0262: stloc.s 31
			IL_0264: ldloc.s 31
			IL_0266: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
			IL_026b: stloc.s 32
			IL_026d: ldc.r4 -95
			IL_0272: ldc.r4 95
			IL_0277: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
			IL_027c: stloc.s 33
			IL_027e: ldc.r4 -95
			IL_0283: ldc.r4 95
			IL_0288: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
			IL_028d: stloc.s 34
			IL_028f: ldloc.s 34
			IL_0291: stloc.s 9
			IL_0293: ldloc.s 32
			IL_0295: ldloca.s 35
			IL_0297: ldloc.s 33
			IL_0299: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_029e: ldloca.s 35
			IL_02a0: ldc.r4 0.25
			IL_02a5: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_02aa: ldloca.s 35
			IL_02ac: ldloc.s 9
			IL_02ae: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_02b3: ldloc.s 35
			IL_02b5: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
			IL_02ba: ldloc.s 31
			IL_02bc: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class [Unity.TextMeshPro]TMPro.TextMeshPro>()
			IL_02c1: stloc.s 36
			IL_02c3: ldloc.s 36
			IL_02c5: callvirt instance class [UnityEngine.CoreModule]UnityEngine.RectTransform [Unity.TextMeshPro]TMPro.TMP_Text::get_rectTransform()
			IL_02ca: stloc.s 37
			IL_02cc: ldloc.s 37
			IL_02ce: ldstr "unpcklps xmm0,xmm12"
			IL_02d3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_02d8: box [mscorlib]System.Int64
			IL_02dd: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector2>(object)
			IL_02e2: callvirt instance void [UnityEngine.CoreModule]UnityEngine.RectTransform::set_pivot(valuetype [UnityEngine.CoreModule]UnityEngine.Vector2)
			IL_02e7: ldloc.s 36
			IL_02e9: ldc.i4 1026
			IL_02ee: box [mscorlib]System.Int32
			IL_02f3: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions>(object)
			IL_02f8: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_alignment(valuetype [Unity.TextMeshPro]TMPro.TextAlignmentOptions)
			IL_02fd: ldloc.s 36
			IL_02ff: ldc.r4 96
			IL_0304: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_fontSize(float32)
			IL_0309: ldloc.s 36
			IL_030b: ldc.i4.0
			IL_030c: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_enableKerning(bool)
			IL_0311: ldloc.s 31
			IL_0313: stloc.s 15

			IL_0315: nop
			IL_0316: ldloc.s 15
			IL_0318: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class TMPro.Examples.TextMeshProFloatingText>()
			IL_031d: stloc.s 38
			IL_031f: ldarg.0
			IL_0320: ldloc.s 38
			IL_0322: stfld class TMPro.Examples.TextMeshProFloatingText TMPro.Examples.Benchmark02::floatingText_Script
			IL_0327: ldarg.0
			IL_0328: ldfld class TMPro.Examples.TextMeshProFloatingText TMPro.Examples.Benchmark02::floatingText_Script
			IL_032d: stloc.s 30
			IL_032f: ldloc.s 30
			IL_0331: ldc.i4.0
			IL_0332: stfld int32 TMPro.Examples.TextMeshProFloatingText::SpawnType

			IL_0337: nop
			IL_0338: ldc.i4.0
			IL_0339: ldc.i4.1
			IL_033a: add
			IL_033b: stloc.s 39
			IL_033d: ldc.i4.0
			IL_033e: ldc.i4.1
			IL_033f: add
			IL_0340: ldarg.0
			IL_0341: ldfld int32 TMPro.Examples.Benchmark02::NumberOfNPC
			IL_0346: clt
			IL_0348: brtrue IL_001e
		// end loop
		IL_034d: nop

		IL_034e: nop
		IL_034f: ret

		IL_0350: nop
		IL_0351: ldnull
		IL_0352: throw
	} // end of method Benchmark02::Start

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x28af
		// Header size: 1
		// Code size: 16 (0x10)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i4.s 12
		IL_0004: stfld int32 TMPro.Examples.Benchmark02::NumberOfNPC
		IL_0009: ldarg.0
		IL_000a: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_000f: ret
	} // end of method Benchmark02::.ctor

} // end of class TMPro.Examples.Benchmark02
