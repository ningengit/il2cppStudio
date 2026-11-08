.class public auto ansi serializable beforefieldinit TMPro.TMP_PhoneNumberValidator
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
		// Method begins at RVA 0x27c2
		// Header size: 1
		// Code size: 18 (0x12)
		.maxstack 8

		IL_0000: nop
		IL_0001: br.s IL_0004

		IL_0003: nop

		IL_0004: nop
		IL_0005: ldstr "Trying to validate..."
		IL_000a: call void [UnityEngine.CoreModule]UnityEngine.Debug::Log(object)
		IL_000f: nop
		IL_0010: ldc.i4.0
		IL_0011: ret
	} // end of method TMP_PhoneNumberValidator::Validate

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x27d5
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.UIElementsModule]UnityEngine.UIElements.StyleSheet::.ctor()
		IL_0007: ret
	} // end of method TMP_PhoneNumberValidator::.ctor

} // end of class TMPro.TMP_PhoneNumberValidator
