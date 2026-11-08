.class public auto ansi serializable beforefieldinit TMPro.TMP_DigitValidator
	extends [Unity.TextMeshPro]TMPro.TMP_InputValidator
{
	// Methods
	.method public hidebysig virtual 
		instance char Validate (
			string& text,
			int32& pos,
			char ch
		) cil managed 
	{
		// Method begins at RVA 0x2791
		// Header size: 1
		// Code size: 39 (0x27)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.3
		IL_0002: conv.i8
		IL_0003: ldc.i8 -48
		IL_000c: add
		IL_000d: conv.i4
		IL_000e: ldc.i4.s 9
		IL_0010: cgt.un
		IL_0012: ldc.i4.0
		IL_0013: ceq
		IL_0015: brtrue.s IL_001a

		IL_0017: nop
		IL_0018: ldc.i4.0
		IL_0019: ret

		IL_001a: nop
		IL_001b: ldstr "inc dword ptr [r8] ; value = BinaryExpr"
		IL_0020: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0025: ldarg.3
		IL_0026: ret
	} // end of method TMP_DigitValidator::Validate

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x27b9
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.UIElementsModule]UnityEngine.UIElements.StyleSheet::.ctor()
		IL_0007: ret
	} // end of method TMP_DigitValidator::.ctor

} // end of class TMPro.TMP_DigitValidator
