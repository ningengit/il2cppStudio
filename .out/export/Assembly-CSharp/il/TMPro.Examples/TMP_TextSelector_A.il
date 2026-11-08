.class public auto ansi beforefieldinit TMPro.Examples.TMP_TextSelector_A
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
	implements [UnityEngine.UI]UnityEngine.EventSystems.IPointerEnterHandler,
	           [UnityEngine.UI]UnityEngine.EventSystems.IEventSystemHandler,
	           [UnityEngine.UI]UnityEngine.EventSystems.IPointerExitHandler
{
	// Fields
	.field private class [Unity.TextMeshPro]TMPro.TextMeshPro m_TextMeshPro
	.field private class [UnityEngine.CoreModule]UnityEngine.Camera m_Camera
	.field private bool m_isHoveringObject
	.field private int32 m_selectedLink
	.field private int32 m_lastCharIndex
	.field private int32 m_lastWordIndex

	// Methods
	.method private hidebysig 
		instance void Awake () cil managed 
	{
		// Method begins at RVA 0x17bb8
		// Header size: 12
		// Code size: 38 (0x26)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[1] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[2] class [UnityEngine.CoreModule]UnityEngine.Camera
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::GetComponent<class [Unity.TextMeshPro]TMPro.TextMeshPro>()
		IL_000e: stloc.1
		IL_000f: ldarg.0
		IL_0010: ldloc.1
		IL_0011: stfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_TextSelector_A::m_TextMeshPro
		IL_0016: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_001b: stloc.2
		IL_001c: ldarg.0
		IL_001d: ldloc.2
		IL_001e: stfld class [UnityEngine.CoreModule]UnityEngine.Camera TMPro.Examples.TMP_TextSelector_A::m_Camera
		IL_0023: nop
		IL_0024: ldnull
		IL_0025: throw
	} // end of method TMP_TextSelector_A::Awake

	.method private hidebysig 
		instance void LateUpdate () cil managed 
	{
		// Method begins at RVA 0x17bec
		// Header size: 12
		// Code size: 1571 (0x623)
		.maxstack 5
		.locals init (
			[0] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[1] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[2] class [UnityEngine.CoreModule]UnityEngine.RectTransform,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[4] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[5] int64,
			[6] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[7] float32,
			[8] class [UnityEngine.CoreModule]UnityEngine.RectTransform,
			[9] bool,
			[10] bool,
			[11] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[12] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[13] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[14] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[15] int32,
			[16] int64,
			[17] bool,
			[18] bool,
			[19] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[20] int32,
			[21] int32,
			[22] int32,
			[23] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[24] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[25] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
			[26] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[],
			[27] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[28] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[29] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[30] int32,
			[31] int32,
			[32] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[33] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
			[34] valuetype [Unity.TextMeshPro]TMPro.TMP_LinkInfo[],
			[35] string[],
			[36] string[],
			[37] string,
			[38] string,
			[39] string,
			[40] string,
			[41] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[42] float32,
			[43] int64,
			[44] class [UnityEngine.CoreModule]UnityEngine.RectTransform,
			[45] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[46] float32,
			[47] float32,
			[48] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[49] class [UnityEngine.CoreModule]UnityEngine.RectTransform,
			[50] bool,
			[51] string,
			[52] bool,
			[53] string,
			[54] bool,
			[55] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[56] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[57] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[58] int64,
			[59] float32,
			[60] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[61] int32,
			[62] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[63] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[64] int64,
			[65] int32,
			[66] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[67] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[68] float32,
			[69] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[70] class [Unity.TextMeshPro]TMPro.TextMeshPro,
			[71] class [Unity.TextMeshPro]TMPro.TMP_TextInfo,
			[72] valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[],
			[73] int32,
			[74] int32,
			[75] int32,
			[76] int64,
			[77] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_TextSelector_A::m_TextMeshPro
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldc.i4.0
		IL_000a: stfld bool TMPro.Examples.TMP_TextSelector_A::m_isHoveringObject
		IL_000f: ldloc.0
		IL_0010: stloc.1
		IL_0011: ldloc.1
		IL_0012: callvirt instance class [UnityEngine.CoreModule]UnityEngine.RectTransform [Unity.TextMeshPro]TMPro.TMP_Text::get_rectTransform()
		IL_0017: stloc.2
		IL_0018: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.InputLegacyModule]UnityEngine.Input::get_mousePosition()
		IL_001d: stloc.3
		IL_001e: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_0023: stloc.s 4
		IL_0025: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_002a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_002f: stloc.s 5
		IL_0031: ldloc.s 4
		IL_0033: stloc.s 6
		IL_0035: ldloca.s 3
		IL_0037: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_003c: stloc.s 7
		IL_003e: ldloc.2
		IL_003f: stloc.s 8
		IL_0041: br.s IL_0044

		IL_0043: nop

		IL_0044: nop
		IL_0045: ldloc.s 8
		IL_0047: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_004c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0051: conv.i8
		IL_0052: ldc.i8 -41
		IL_005b: add
		IL_005c: box [mscorlib]System.Int64
		IL_0061: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0066: ldloc.s 6
		IL_0068: call bool [Unity.TextMeshPro]TMPro.TMP_TextUtilities::IsIntersectingRectTransform(class [UnityEngine.CoreModule]UnityEngine.RectTransform, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, class [UnityEngine.CoreModule]UnityEngine.Camera)
		IL_006d: stloc.s 9
		IL_006f: ldloc.s 9
		IL_0071: stloc.s 10
		IL_0073: ldloc.s 9
		IL_0075: ldc.i4.0
		IL_0076: ceq
		IL_0078: brtrue.s IL_0082

		IL_007a: nop
		IL_007b: ldarg.0
		IL_007c: ldc.i4.1
		IL_007d: stfld bool TMPro.Examples.TMP_TextSelector_A::m_isHoveringObject

		IL_0082: nop
		IL_0083: ldarg.0
		IL_0084: ldfld bool TMPro.Examples.TMP_TextSelector_A::m_isHoveringObject
		IL_0089: ldc.i4.0
		IL_008a: ceq
		IL_008c: brtrue IL_061b

		IL_0091: nop
		IL_0092: ldarg.0
		IL_0093: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_TextSelector_A::m_TextMeshPro
		IL_0098: stloc.s 11
		IL_009a: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.InputLegacyModule]UnityEngine.Input::get_mousePosition()
		IL_009f: stloc.s 12
		IL_00a1: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_00a6: stloc.s 13
		IL_00a8: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_00ad: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00b2: stloc.s 5
		IL_00b4: ldloc.s 13
		IL_00b6: stloc.s 6
		IL_00b8: ldloca.s 12
		IL_00ba: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00bf: stloc.s 7
		IL_00c1: ldloc.s 11
		IL_00c3: stloc.s 14
		IL_00c5: br.s IL_00c8

		IL_00c7: nop

		IL_00c8: nop
		IL_00c9: ldloc.s 14
		IL_00cb: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_00d0: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00d5: conv.i8
		IL_00d6: ldc.i8 -41
		IL_00df: add
		IL_00e0: box [mscorlib]System.Int64
		IL_00e5: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_00ea: ldloc.s 6
		IL_00ec: ldc.i4.1
		IL_00ed: call int32 [Unity.TextMeshPro]TMPro.TMP_TextUtilities::FindIntersectingCharacter(class [Unity.TextMeshPro]TMPro.TMP_Text, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, class [UnityEngine.CoreModule]UnityEngine.Camera, bool)
		IL_00f2: stloc.s 15
		IL_00f4: ldloc.s 15
		IL_00f6: conv.i8
		IL_00f7: stloc.s 16
		IL_00f9: ldloc.s 15
		IL_00fb: conv.i8
		IL_00fc: conv.i4
		IL_00fd: ldc.i4.m1
		IL_00fe: ceq
		IL_0100: brtrue IL_0208

		IL_0105: nop
		IL_0106: ldloc.s 16
		IL_0108: conv.i4
		IL_0109: ldarg.0
		IL_010a: ldfld int32 TMPro.Examples.TMP_TextSelector_A::m_lastCharIndex
		IL_010f: ceq
		IL_0111: brtrue IL_0208

		IL_0116: nop
		IL_0117: ldc.i4 304
		IL_011c: box [mscorlib]System.Int32
		IL_0121: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
		IL_0126: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetKeyInt(valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode)
		IL_012b: stloc.s 17
		IL_012d: ldloc.s 17
		IL_012f: ldc.i4.0
		IL_0130: ceq
		IL_0132: ldc.i4.0
		IL_0133: ceq
		IL_0135: brtrue.s IL_0158

		IL_0137: nop
		IL_0138: ldc.i4 303
		IL_013d: box [mscorlib]System.Int32
		IL_0142: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
		IL_0147: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetKeyInt(valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode)
		IL_014c: stloc.s 18
		IL_014e: ldloc.s 18
		IL_0150: ldc.i4.0
		IL_0151: ceq
		IL_0153: brtrue IL_0208

		IL_0158: nop
		IL_0159: ldarg.0
		IL_015a: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_TextSelector_A::m_TextMeshPro
		IL_015f: stloc.s 19
		IL_0161: ldarg.0
		IL_0162: ldloc.s 16
		IL_0164: conv.i4
		IL_0165: stfld int32 TMPro.Examples.TMP_TextSelector_A::m_lastCharIndex
		IL_016a: ldc.i4.0
		IL_016b: ldc.i4 255
		IL_0170: call int32 [UnityEngine.CoreModule]UnityEngine.Random::Range(int32, int32)
		IL_0175: stloc.s 20
		IL_0177: ldc.i4.0
		IL_0178: ldc.i4 255
		IL_017d: call int32 [UnityEngine.CoreModule]UnityEngine.Random::Range(int32, int32)
		IL_0182: stloc.s 21
		IL_0184: ldc.i4.0
		IL_0185: ldc.i4 255
		IL_018a: call int32 [UnityEngine.CoreModule]UnityEngine.Random::Range(int32, int32)
		IL_018f: stloc.s 22
		IL_0191: ldarg.0
		IL_0192: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_TextSelector_A::m_TextMeshPro
		IL_0197: stloc.s 23
		IL_0199: ldstr "mov [rdx+rbx*4+20h],eax ; value = LocalExpr"
		IL_019e: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_01a3: ldstr "mov [rdx+rcx*4+20h],eax ; value = LocalExpr"
		IL_01a8: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_01ad: ldstr "mov [rdx+rcx*4+20h],eax ; value = LocalExpr"
		IL_01b2: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_01b7: ldstr "mov [rdx+rcx*4+20h],eax ; value = LocalExpr"
		IL_01bc: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_01c1: ldarg.0
		IL_01c2: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_TextSelector_A::m_TextMeshPro
		IL_01c7: stloc.s 24
		IL_01c9: ldloc.s 24
		IL_01cb: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo [Unity.TextMeshPro]TMPro.TMP_Text::m_textInfo
		IL_01d0: stloc.s 25
		IL_01d2: ldloc.s 25
		IL_01d4: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
		IL_01d9: stloc.s 26
		IL_01db: ldstr "*(LocalExpr + 0x20)"
		IL_01e0: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01e5: box [mscorlib]System.Int64
		IL_01ea: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [UnityEngine.CoreModule]UnityEngine.Mesh>(object)
		IL_01ef: ldstr "*(FieldExpr + 0x58)"
		IL_01f4: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01f9: box [mscorlib]System.Int64
		IL_01fe: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Color32[]>(object)
		IL_0203: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Mesh::set_colors32(valuetype [UnityEngine.CoreModule]UnityEngine.Color32[])

		IL_0208: nop
		IL_0209: ldarg.0
		IL_020a: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_TextSelector_A::m_TextMeshPro
		IL_020f: stloc.s 27
		IL_0211: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.InputLegacyModule]UnityEngine.Input::get_mousePosition()
		IL_0216: stloc.s 28
		IL_0218: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_021d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0222: stloc.s 5
		IL_0224: ldarg.0
		IL_0225: ldfld class [UnityEngine.CoreModule]UnityEngine.Camera TMPro.Examples.TMP_TextSelector_A::m_Camera
		IL_022a: stloc.s 6
		IL_022c: ldloca.s 28
		IL_022e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0233: stloc.s 7
		IL_0235: ldloc.s 27
		IL_0237: stloc.s 29
		IL_0239: br.s IL_023c

		IL_023b: nop

		IL_023c: nop
		IL_023d: ldloc.s 29
		IL_023f: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0244: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0249: conv.i8
		IL_024a: ldc.i8 -41
		IL_0253: add
		IL_0254: box [mscorlib]System.Int64
		IL_0259: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_025e: ldloc.s 6
		IL_0260: call int32 [Unity.TextMeshPro]TMPro.TMP_TextUtilities::FindIntersectingLink(class [Unity.TextMeshPro]TMPro.TMP_Text, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, class [UnityEngine.CoreModule]UnityEngine.Camera)
		IL_0265: stloc.s 30
		IL_0267: ldloc.s 30
		IL_0269: stloc.s 31
		IL_026b: ldloc.s 30
		IL_026d: ldc.i4.m1
		IL_026e: ceq
		IL_0270: ldc.i4.0
		IL_0271: ceq
		IL_0273: brtrue.s IL_0285

		IL_0275: nop
		IL_0276: ldarg.0
		IL_0277: ldfld int32 TMPro.Examples.TMP_TextSelector_A::m_selectedLink
		IL_027c: ldloc.s 31
		IL_027e: ceq
		IL_0280: ldc.i4.0
		IL_0281: ceq
		IL_0283: brtrue.s IL_0292

		IL_0285: nop
		IL_0286: ldloc.s 31
		IL_0288: ldarg.0
		IL_0289: ldfld int32 TMPro.Examples.TMP_TextSelector_A::m_selectedLink
		IL_028e: ceq
		IL_0290: brtrue.s IL_029a

		IL_0292: nop
		IL_0293: ldarg.0
		IL_0294: ldc.i4.m1
		IL_0295: stfld int32 TMPro.Examples.TMP_TextSelector_A::m_selectedLink

		IL_029a: nop
		IL_029b: ldloc.s 31
		IL_029d: ldc.i4.m1
		IL_029e: ceq
		IL_02a0: brtrue IL_0440

		IL_02a5: nop
		IL_02a6: ldloc.s 31
		IL_02a8: ldarg.0
		IL_02a9: ldfld int32 TMPro.Examples.TMP_TextSelector_A::m_selectedLink
		IL_02ae: ceq
		IL_02b0: brtrue IL_0440

		IL_02b5: nop
		IL_02b6: ldarg.0
		IL_02b7: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_TextSelector_A::m_TextMeshPro
		IL_02bc: stloc.s 32
		IL_02be: ldarg.0
		IL_02bf: ldloc.s 31
		IL_02c1: stfld int32 TMPro.Examples.TMP_TextSelector_A::m_selectedLink
		IL_02c6: ldloc.s 32
		IL_02c8: stloc.1
		IL_02c9: ldloc.1
		IL_02ca: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo [Unity.TextMeshPro]TMPro.TMP_Text::m_textInfo
		IL_02cf: stloc.s 33
		IL_02d1: ldloc.s 33
		IL_02d3: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_LinkInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::linkInfo
		IL_02d8: stloc.s 34
		IL_02da: ldc.i4.5
		IL_02db: newarr [mscorlib]System.String
		IL_02e0: stloc.s 35
		IL_02e2: ldloc.s 35
		IL_02e4: stloc.s 36
		IL_02e6: ldloc.s 35
		IL_02e8: ldc.i4.0
		IL_02e9: ldstr "Link ID: \""
		IL_02ee: stelem [mscorlib]System.String
		IL_02f3: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_02f8: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_02fd: conv.i8
		IL_02fe: ldc.i8 -9
		IL_0307: add
		IL_0308: call instance string [Unity.TextMeshPro]TMPro.TMP_LinkInfo::GetLinkID()
		IL_030d: stloc.s 37
		IL_030f: ldloc.s 36
		IL_0311: ldc.i4.1
		IL_0312: ldloc.s 37
		IL_0314: stelem [mscorlib]System.String
		IL_0319: ldloc.s 36
		IL_031b: ldc.i4.2
		IL_031c: ldstr "\"   Link Text: \""
		IL_0321: stelem [mscorlib]System.String
		IL_0326: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_032b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0330: conv.i8
		IL_0331: ldc.i8 -9
		IL_033a: add
		IL_033b: call instance string [Unity.TextMeshPro]TMPro.TMP_LinkInfo::GetLinkText()
		IL_0340: stloc.s 38
		IL_0342: ldloc.s 36
		IL_0344: ldc.i4.3
		IL_0345: ldloc.s 38
		IL_0347: stelem [mscorlib]System.String
		IL_034c: ldloc.s 36
		IL_034e: ldc.i4.4
		IL_034f: ldstr "\""
		IL_0354: stelem [mscorlib]System.String
		IL_0359: ldloc.s 36
		IL_035b: call string [mscorlib]System.String::Concat(string[])
		IL_0360: stloc.s 39
		IL_0362: ldloc.s 39
		IL_0364: stloc.s 40
		IL_0366: br.s IL_0369

		IL_0368: nop

		IL_0369: nop
		IL_036a: ldloc.s 40
		IL_036c: call void [UnityEngine.CoreModule]UnityEngine.Debug::Log(object)
		IL_0371: ldarg.0
		IL_0372: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_TextSelector_A::m_TextMeshPro
		IL_0377: stloc.s 41
		IL_0379: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::zeroVector
		IL_037e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0383: stloc.s 42
		IL_0385: ldstr "struct_chunk(StaticFieldExpr+0x0, 8)"
		IL_038a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_038f: stloc.s 43
		IL_0391: ldloc.s 41
		IL_0393: stloc.1
		IL_0394: ldloc.1
		IL_0395: callvirt instance class [UnityEngine.CoreModule]UnityEngine.RectTransform [Unity.TextMeshPro]TMPro.TMP_Text::get_rectTransform()
		IL_039a: stloc.s 44
		IL_039c: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.InputLegacyModule]UnityEngine.Input::get_mousePosition()
		IL_03a1: stloc.s 45
		IL_03a3: ldloca.s 45
		IL_03a5: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_03aa: stloc.s 46
		IL_03ac: ldloca.s 45
		IL_03ae: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_03b3: stloc.s 47
		IL_03b5: ldarg.0
		IL_03b6: ldfld class [UnityEngine.CoreModule]UnityEngine.Camera TMPro.Examples.TMP_TextSelector_A::m_Camera
		IL_03bb: stloc.s 48
		IL_03bd: ldloc.s 44
		IL_03bf: stloc.s 49
		IL_03c1: br.s IL_03c4

		IL_03c3: nop

		IL_03c4: nop
		IL_03c5: ldloc.s 49
		IL_03c7: ldstr "unpcklps xmm6,xmm7"
		IL_03cc: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_03d1: box [mscorlib]System.Int64
		IL_03d6: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector2>(object)
		IL_03db: ldloc.s 48
		IL_03dd: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_03e2: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_03e7: conv.i8
		IL_03e8: ldc.i8 -57
		IL_03f1: add
		IL_03f2: call bool [UnityEngine.UIModule]UnityEngine.RectTransformUtility::ScreenPointToWorldPointInRectangle(class [UnityEngine.CoreModule]UnityEngine.RectTransform, valuetype [UnityEngine.CoreModule]UnityEngine.Vector2, class [UnityEngine.CoreModule]UnityEngine.Camera, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3&)
		IL_03f7: stloc.s 50
		IL_03f9: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_03fe: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0403: conv.i8
		IL_0404: ldc.i8 -9
		IL_040d: add
		IL_040e: call instance string [Unity.TextMeshPro]TMPro.TMP_LinkInfo::GetLinkID()
		IL_0413: stloc.s 51
		IL_0415: ldloc.s 51
		IL_0417: ldstr "id_01"
		IL_041c: call bool [mscorlib]System.String::Equals(string, string)
		IL_0421: stloc.s 52
		IL_0423: ldloc.s 51
		IL_0425: stloc.s 53
		IL_0427: ldloc.s 52
		IL_0429: ldc.i4.0
		IL_042a: ceq
		IL_042c: ldc.i4.0
		IL_042d: ceq
		IL_042f: brtrue.s IL_0440

		IL_0431: nop
		IL_0432: ldloc.s 53
		IL_0434: ldstr "id_02"
		IL_0439: call bool [mscorlib]System.String::Equals(string, string)
		IL_043e: stloc.s 54

		IL_0440: nop
		IL_0441: ldarg.0
		IL_0442: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_TextSelector_A::m_TextMeshPro
		IL_0447: stloc.s 55
		IL_0449: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.InputLegacyModule]UnityEngine.Input::get_mousePosition()
		IL_044e: stloc.s 56
		IL_0450: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_0455: stloc.s 57
		IL_0457: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_045c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0461: stloc.s 58
		IL_0463: ldloc.s 57
		IL_0465: stloc.s 6
		IL_0467: ldloca.s 56
		IL_0469: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_046e: stloc.s 59
		IL_0470: ldloc.s 55
		IL_0472: stloc.s 60
		IL_0474: br.s IL_0477

		IL_0476: nop

		IL_0477: nop
		IL_0478: ldloc.s 60
		IL_047a: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_047f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0484: conv.i8
		IL_0485: ldc.i8 -41
		IL_048e: add
		IL_048f: box [mscorlib]System.Int64
		IL_0494: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0499: ldloc.s 6
		IL_049b: call int32 [Unity.TextMeshPro]TMPro.TMP_TextUtilities::FindIntersectingWord(class [Unity.TextMeshPro]TMPro.TMP_Text, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, class [UnityEngine.CoreModule]UnityEngine.Camera)
		IL_04a0: stloc.s 61
		IL_04a2: ldloc.s 61
		IL_04a4: stloc.s 31
		IL_04a6: ldloc.s 61
		IL_04a8: ldc.i4.m1
		IL_04a9: ceq
		IL_04ab: brtrue IL_061b

		IL_04b0: nop
		IL_04b1: ldloc.s 31
		IL_04b3: ldarg.0
		IL_04b4: ldfld int32 TMPro.Examples.TMP_TextSelector_A::m_lastWordIndex
		IL_04b9: ceq
		IL_04bb: brtrue IL_061b

		IL_04c0: nop
		IL_04c1: ldarg.0
		IL_04c2: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_TextSelector_A::m_TextMeshPro
		IL_04c7: stloc.s 62
		IL_04c9: ldarg.0
		IL_04ca: ldloc.s 31
		IL_04cc: stfld int32 TMPro.Examples.TMP_TextSelector_A::m_lastWordIndex
		IL_04d1: ldarg.0
		IL_04d2: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_TextSelector_A::m_TextMeshPro
		IL_04d7: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [Unity.TextMeshPro]TMPro.TextMeshPro::get_transform()
		IL_04dc: stloc.s 63
		IL_04de: ldstr "*(FieldExpr + 0x28)"
		IL_04e3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_04e8: conv.i4
		IL_04e9: conv.i8
		IL_04ea: stloc.s 64
		IL_04ec: ldstr "*(FieldExpr + 0x30)"
		IL_04f1: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_04f6: conv.i4
		IL_04f7: stloc.s 65
		IL_04f9: ldloc.s 63
		IL_04fb: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0500: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0505: conv.i8
		IL_0506: ldc.i8 -41
		IL_050f: add
		IL_0510: box [mscorlib]System.Int64
		IL_0515: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_051a: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::TransformPoint(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_051f: stloc.s 66
		IL_0521: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_0526: stloc.s 67
		IL_0528: ldloca.s 66
		IL_052a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_052f: stloc.s 68
		IL_0531: ldloc.s 67
		IL_0533: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0538: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_053d: conv.i8
		IL_053e: ldc.i8 -41
		IL_0547: add
		IL_0548: box [mscorlib]System.Int64
		IL_054d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0552: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Camera::WorldToScreenPoint(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0557: stloc.s 69
		IL_0559: ldarg.0
		IL_055a: ldfld class [Unity.TextMeshPro]TMPro.TextMeshPro TMPro.Examples.TMP_TextSelector_A::m_TextMeshPro
		IL_055f: stloc.s 70
		IL_0561: ldloc.s 70
		IL_0563: ldfld class [Unity.TextMeshPro]TMPro.TMP_TextInfo [Unity.TextMeshPro]TMPro.TMP_Text::m_textInfo
		IL_0568: stloc.s 71
		IL_056a: ldloc.s 71
		IL_056c: ldfld valuetype [Unity.TextMeshPro]TMPro.TMP_MeshInfo[] [Unity.TextMeshPro]TMPro.TMP_TextInfo::meshInfo
		IL_0571: stloc.s 72
		IL_0573: ldc.i4.0
		IL_0574: ldc.i4 255
		IL_0579: call int32 [UnityEngine.CoreModule]UnityEngine.Random::Range(int32, int32)
		IL_057e: stloc.s 73
		IL_0580: ldc.i4.0
		IL_0581: ldc.i4 255
		IL_0586: call int32 [UnityEngine.CoreModule]UnityEngine.Random::Range(int32, int32)
		IL_058b: stloc.s 74
		IL_058d: ldc.i4.0
		IL_058e: ldc.i4 255
		IL_0593: call int32 [UnityEngine.CoreModule]UnityEngine.Random::Range(int32, int32)
		IL_0598: stloc.s 75
		IL_059a: ldstr "*(LocalExpr + 0x58)"
		IL_059f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_05a4: stloc.s 76
		IL_05a6: ldloc.s 65
		IL_05a8: ldc.i4.0
		IL_05a9: cgt
		IL_05ab: ldc.i4.0
		IL_05ac: ceq
		IL_05ae: brtrue.s IL_05f5

		IL_05b0: nop
		IL_05b1: ldstr "stack[0x8]"
		IL_05b6: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_05bb: conv.i4
		IL_05bc: stloc.s 31
		// loop start (head: IL_05be)
			IL_05be: nop
			IL_05bf: ldstr "mov [rbx+r8*4+20h],eax ; value = LocalExpr"
			IL_05c4: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
			IL_05c9: ldstr "mov [rbx+rcx*4+20h],eax ; value = LocalExpr"
			IL_05ce: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
			IL_05d3: ldstr "mov [rbx+rcx*4+20h],eax ; value = LocalExpr"
			IL_05d8: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
			IL_05dd: ldstr "mov [rbx+rcx*4+20h],eax ; value = LocalExpr"
			IL_05e2: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
			IL_05e7: ldc.i4.0
			IL_05e8: ldc.i4.1
			IL_05e9: add
			IL_05ea: stloc.s 77
			IL_05ec: ldc.i4.0
			IL_05ed: ldc.i4.1
			IL_05ee: add
			IL_05ef: ldloc.s 65
			IL_05f1: clt
			IL_05f3: brtrue.s IL_05be
		// end loop

		IL_05f5: nop
		IL_05f6: ldstr "FieldExpr->klass"
		IL_05fb: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0600: box [mscorlib]System.IntPtr
		IL_0605: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [UnityEngine.CoreModule]UnityEngine.Mesh>(object)
		IL_060a: ldloc.s 76
		IL_060c: box [mscorlib]System.Int64
		IL_0611: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Color32[]>(object)
		IL_0616: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Mesh::set_colors32(valuetype [UnityEngine.CoreModule]UnityEngine.Color32[])

		IL_061b: nop
		IL_061c: ret

		IL_061d: nop
		IL_061e: ldnull
		IL_061f: throw

		IL_0620: nop
		IL_0621: ldnull
		IL_0622: throw
	} // end of method TMP_TextSelector_A::LateUpdate

	.method public final hidebysig newslot virtual 
		instance void OnPointerEnter (
			class [UnityEngine.UI]UnityEngine.EventSystems.PointerEventData eventData
		) cil managed 
	{
		// Method begins at RVA 0x29c2
		// Header size: 1
		// Code size: 23 (0x17)
		.maxstack 8

		IL_0000: nop
		IL_0001: br.s IL_0004

		IL_0003: nop

		IL_0004: nop
		IL_0005: ldstr "OnPointerEnter()"
		IL_000a: call void [UnityEngine.CoreModule]UnityEngine.Debug::Log(object)
		IL_000f: ldarg.0
		IL_0010: ldc.i4.1
		IL_0011: stfld bool TMPro.Examples.TMP_TextSelector_A::m_isHoveringObject
		IL_0016: ret
	} // end of method TMP_TextSelector_A::OnPointerEnter

	.method public final hidebysig newslot virtual 
		instance void OnPointerExit (
			class [UnityEngine.UI]UnityEngine.EventSystems.PointerEventData eventData
		) cil managed 
	{
		// Method begins at RVA 0x29da
		// Header size: 1
		// Code size: 23 (0x17)
		.maxstack 8

		IL_0000: nop
		IL_0001: br.s IL_0004

		IL_0003: nop

		IL_0004: nop
		IL_0005: ldstr "OnPointerExit()"
		IL_000a: call void [UnityEngine.CoreModule]UnityEngine.Debug::Log(object)
		IL_000f: ldarg.0
		IL_0010: ldc.i4.0
		IL_0011: stfld bool TMPro.Examples.TMP_TextSelector_A::m_isHoveringObject
		IL_0016: ret
	} // end of method TMP_TextSelector_A::OnPointerExit

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x29f2
		// Header size: 1
		// Code size: 31 (0x1f)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i8 -1
		IL_000b: conv.i4
		IL_000c: stfld int32 TMPro.Examples.TMP_TextSelector_A::m_selectedLink
		IL_0011: ldarg.0
		IL_0012: ldc.i4.m1
		IL_0013: stfld int32 TMPro.Examples.TMP_TextSelector_A::m_lastWordIndex
		IL_0018: ldarg.0
		IL_0019: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_001e: ret
	} // end of method TMP_TextSelector_A::.ctor

} // end of class TMPro.Examples.TMP_TextSelector_A
