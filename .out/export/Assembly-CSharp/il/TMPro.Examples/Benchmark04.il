.class public auto ansi beforefieldinit TMPro.Examples.Benchmark04
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public int32 SpawnType
	.field public int32 MinPointSize
	.field public int32 MaxPointSize
	.field public int32 Steps
	.field private class [UnityEngine.CoreModule]UnityEngine.Transform m_Transform

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x14f00
		// Header size: 12
		// Code size: 544 (0x220)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[2] int32,
			[3] int32,
			[4] int32,
			[5] int32,
			[6] float32,
			[7] float32,
			[8] float32,
			[9] int64,
			[10] int32,
			[11] string,
			[12] string,
			[13] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[14] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[15] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[16] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[17] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[18] float32,
			[19] float32,
			[20] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[21] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[22] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[23] class [UnityEngine.CoreModule]UnityEngine.RectTransform,
			[24] class [UnityEngine.CoreModule]UnityEngine.RectTransform,
			[25] int32,
			[26] string,
			[27] string,
			[28] int32,
			[29] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.Benchmark04::m_Transform
		IL_000f: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_0014: stloc.1
		IL_0015: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_height()
		IL_001a: stloc.2
		IL_001b: ldloc.2
		IL_001c: stloc.3
		IL_001d: ldloc.1
		IL_001e: ldloc.3
		IL_001f: ldstr "rdx"
		IL_0024: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0029: conv.i4
		IL_002a: sub
		IL_002b: ldc.i4.1
		IL_002c: shr
		IL_002d: conv.r4
		IL_002e: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Camera::set_orthographicSize(float32)
		IL_0033: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_width()
		IL_0038: stloc.s 4
		IL_003a: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_height()
		IL_003f: stloc.s 5
		IL_0041: ldarg.0
		IL_0042: ldfld int32 TMPro.Examples.Benchmark04::MinPointSize
		IL_0047: stloc.3
		IL_0048: ldloc.s 5
		IL_004a: conv.r4
		IL_004b: stloc.s 6
		IL_004d: ldloc.3
		IL_004e: ldstr "rdx"
		IL_0053: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0058: conv.i4
		IL_0059: sub
		IL_005a: ldc.i4.1
		IL_005b: shr
		IL_005c: conv.r4
		IL_005d: stloc.s 7
		IL_005f: ldloc.s 4
		IL_0061: conv.r4
		IL_0062: ldloc.s 5
		IL_0064: conv.r4
		IL_0065: div
		IL_0066: stloc.s 8
		IL_0068: ldarg.0
		IL_0069: ldfld int32 TMPro.Examples.Benchmark04::MinPointSize
		IL_006e: ldarg.0
		IL_006f: ldfld int32 TMPro.Examples.Benchmark04::MaxPointSize
		IL_0074: cgt
		IL_0076: brtrue IL_021b

		IL_007b: nop
		IL_007c: ldstr "global_13EACA0"
		IL_0081: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0086: stloc.s 9
		// loop start (head: IL_0088)
			IL_0088: nop
			IL_0089: ldarg.0
			IL_008a: ldfld int32 TMPro.Examples.Benchmark04::SpawnType
			IL_008f: ldc.i4.0
			IL_0090: ceq
			IL_0092: ldc.i4.0
			IL_0093: ceq
			IL_0095: brtrue IL_01f6

			IL_009a: nop
			IL_009b: ldloca.s 10
			IL_009d: call instance string [mscorlib]System.Int32::ToString()
			IL_00a2: stloc.s 11
			IL_00a4: ldstr "Text - "
			IL_00a9: ldloc.s 11
			IL_00ab: ldstr " Pts"
			IL_00b0: call string [mscorlib]System.String::Concat(string, string, string)
			IL_00b5: stloc.s 12
			IL_00b7: ldloc.s 12
			IL_00b9: newobj instance void [UnityEngine.CoreModule]UnityEngine.GameObject::.ctor(string)
			IL_00be: stloc.s 13
			IL_00c0: ldloc.s 7
			IL_00c2: ldloc.s 7
			IL_00c4: add
			IL_00c5: stloc.s 6
			IL_00c7: ldloc.s 13
			IL_00c9: stloc.s 14
			IL_00cb: ldc.r4 0.0
			IL_00d0: ldloc.s 7
			IL_00d2: ldloc.s 7
			IL_00d4: add
			IL_00d5: cgt
			IL_00d7: brtrue IL_021b

			IL_00dc: nop
			IL_00dd: ldloc.s 14
			IL_00df: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.GameObject::get_transform()
			IL_00e4: stloc.s 15
			IL_00e6: ldloc.s 15
			IL_00e8: stloc.s 16
			IL_00ea: ldarg.0
			IL_00eb: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform TMPro.Examples.Benchmark04::m_Transform
			IL_00f0: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
			IL_00f5: stloc.s 17
			IL_00f7: ldstr "xorps xmm0,xmm10"
			IL_00fc: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0101: conv.r4
			IL_0102: ldloc.s 8
			IL_0104: mul
			IL_0105: ldc.r4 0.975
			IL_010a: mul
			IL_010b: ldloca.s 17
			IL_010d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_0112: add
			IL_0113: stloc.s 6
			IL_0115: ldloc.s 7
			IL_0117: ldc.r4 0.975
			IL_011c: mul
			IL_011d: ldc.r4 0.0
			IL_0122: sub
			IL_0123: ldloca.s 17
			IL_0125: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_012a: add
			IL_012b: stloc.s 18
			IL_012d: ldc.r4 0.0
			IL_0132: ldloca.s 17
			IL_0134: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_0139: add
			IL_013a: stloc.s 19
			IL_013c: ldloc.s 15
			IL_013e: ldloca.s 20
			IL_0140: ldloc.s 6
			IL_0142: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_0147: ldloca.s 20
			IL_0149: ldloc.s 18
			IL_014b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_0150: ldloca.s 20
			IL_0152: ldloc.s 19
			IL_0154: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_0159: ldloc.s 20
			IL_015b: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
			IL_0160: ldloc.s 14
			IL_0162: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::AddComponent<class [Unity.TextMeshPro]TMPro.TextMeshPro>()
			IL_0167: stloc.s 21
			IL_0169: ldloc.s 21
			IL_016b: stloc.s 22
			IL_016d: ldloc.s 22
			IL_016f: callvirt instance class [UnityEngine.CoreModule]UnityEngine.RectTransform [Unity.TextMeshPro]TMPro.TMP_Text::get_rectTransform()
			IL_0174: stloc.s 23
			IL_0176: ldloc.s 23
			IL_0178: stloc.s 24
			IL_017a: ldloc.s 24
			IL_017c: ldstr "unpcklps xmm0,xmm12"
			IL_0181: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0186: box [mscorlib]System.Int64
			IL_018b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector2>(object)
			IL_0190: callvirt instance void [UnityEngine.CoreModule]UnityEngine.RectTransform::set_pivot(valuetype [UnityEngine.CoreModule]UnityEngine.Vector2)
			IL_0195: ldloc.s 21
			IL_0197: ldc.i4.0
			IL_0198: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_enableWordWrapping(bool)
			IL_019d: ldloc.s 21
			IL_019f: ldc.i4.1
			IL_01a0: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_extraPadding(bool)
			IL_01a5: ldloc.s 21
			IL_01a7: ldc.i4.1
			IL_01a8: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_isOrthographic(bool)
			IL_01ad: ldloc.s 21
			IL_01af: ldloc.s 10
			IL_01b1: ldfld int32 [mscorlib]System.Int32::m_value
			IL_01b6: conv.r4
			IL_01b7: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Text::set_fontSize(float32)
			IL_01bc: ldloca.s 25
			IL_01be: call instance string [mscorlib]System.Int32::ToString()
			IL_01c3: stloc.s 26
			IL_01c5: ldloc.s 26
			IL_01c7: ldstr " pts - Lorem ipsum dolor sit..."
			IL_01cc: call string [mscorlib]System.String::Concat(string, string)
			IL_01d1: stloc.s 27
			IL_01d3: ldloc.s 25
			IL_01d5: ldfld int32 [mscorlib]System.Int32::m_value
			IL_01da: stloc.s 28
			IL_01dc: ldloc.s 25
			IL_01de: ldfld int32 [mscorlib]System.Int32::m_value
			IL_01e3: conv.r4
			IL_01e4: stloc.s 6
			IL_01e6: ldc.r4 0.0
			IL_01eb: ldloc.s 25
			IL_01ed: ldfld int32 [mscorlib]System.Int32::m_value
			IL_01f2: conv.r4
			IL_01f3: add
			IL_01f4: stloc.s 29

			IL_01f6: nop
			IL_01f7: ldloc.s 28
			IL_01f9: ldarg.0
			IL_01fa: ldfld int32 TMPro.Examples.Benchmark04::Steps
			IL_01ff: add
			IL_0200: stloc.s 28
			IL_0202: ldloc.s 28
			IL_0204: ldarg.0
			IL_0205: ldfld int32 TMPro.Examples.Benchmark04::Steps
			IL_020a: add
			IL_020b: ldarg.0
			IL_020c: ldfld int32 TMPro.Examples.Benchmark04::MaxPointSize
			IL_0211: cgt
			IL_0213: ldc.i4.0
			IL_0214: ceq
			IL_0216: brtrue IL_0088
		// end loop

		IL_021b: nop
		IL_021c: ret

		IL_021d: nop
		IL_021e: ldnull
		IL_021f: throw
	} // end of method Benchmark04::Start

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x28d4
		// Header size: 1
		// Code size: 31 (0x1f)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i4.s 12
		IL_0004: stfld int32 TMPro.Examples.Benchmark04::MinPointSize
		IL_0009: ldarg.0
		IL_000a: ldc.i4.s 64
		IL_000c: stfld int32 TMPro.Examples.Benchmark04::MaxPointSize
		IL_0011: ldarg.0
		IL_0012: ldc.i4.4
		IL_0013: stfld int32 TMPro.Examples.Benchmark04::Steps
		IL_0018: ldarg.0
		IL_0019: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_001e: ret
	} // end of method Benchmark04::.ctor

} // end of class TMPro.Examples.Benchmark04
