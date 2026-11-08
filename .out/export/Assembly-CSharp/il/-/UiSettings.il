.class public auto ansi beforefieldinit UiSettings
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class [UnityEngine.UI]UnityEngine.UI.Toggle sAuto
	.field public class [UnityEngine.UI]UnityEngine.UI.Toggle sXLarge
	.field public class [UnityEngine.UI]UnityEngine.UI.Toggle sLarge
	.field public class [UnityEngine.UI]UnityEngine.UI.Toggle sMed
	.field public class [UnityEngine.UI]UnityEngine.UI.Toggle sSmall
	.field public class [UnityEngine.UI]UnityEngine.UI.Toggle hLow
	.field public class [UnityEngine.UI]UnityEngine.UI.Toggle hMed
	.field public class [UnityEngine.UI]UnityEngine.UI.Toggle hHigh
	.field private int32 size
	.field private int32 height

	// Methods
	.method public hidebysig 
		instance void UpdateState () cil managed 
	{
		// Method begins at RVA 0xab44
		// Header size: 12
		// Code size: 225 (0xe1)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.UI]UnityEngine.UI.Toggle
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.UI]UnityEngine.UI.Toggle UiSettings::sAuto
		IL_0007: ldfld bool [UnityEngine.UI]UnityEngine.UI.Toggle::m_IsOn
		IL_000c: ldc.i4.0
		IL_000d: ceq
		IL_000f: ldc.i4.0
		IL_0010: ceq
		IL_0012: brtrue.s IL_006d

		IL_0014: nop
		IL_0015: ldarg.0
		IL_0016: ldfld class [UnityEngine.UI]UnityEngine.UI.Toggle UiSettings::sXLarge
		IL_001b: ldfld bool [UnityEngine.UI]UnityEngine.UI.Toggle::m_IsOn
		IL_0020: ldc.i4.0
		IL_0021: ceq
		IL_0023: ldc.i4.0
		IL_0024: ceq
		IL_0026: brtrue.s IL_006a

		IL_0028: nop
		IL_0029: ldarg.0
		IL_002a: ldfld class [UnityEngine.UI]UnityEngine.UI.Toggle UiSettings::sLarge
		IL_002f: ldfld bool [UnityEngine.UI]UnityEngine.UI.Toggle::m_IsOn
		IL_0034: ldc.i4.0
		IL_0035: ceq
		IL_0037: ldc.i4.0
		IL_0038: ceq
		IL_003a: brtrue.s IL_0067

		IL_003c: nop
		IL_003d: ldarg.0
		IL_003e: ldfld class [UnityEngine.UI]UnityEngine.UI.Toggle UiSettings::sMed
		IL_0043: ldfld bool [UnityEngine.UI]UnityEngine.UI.Toggle::m_IsOn
		IL_0048: ldc.i4.0
		IL_0049: ceq
		IL_004b: ldc.i4.0
		IL_004c: ceq
		IL_004e: brtrue.s IL_0064

		IL_0050: nop
		IL_0051: ldarg.0
		IL_0052: ldfld class [UnityEngine.UI]UnityEngine.UI.Toggle UiSettings::sSmall
		IL_0057: ldfld bool [UnityEngine.UI]UnityEngine.UI.Toggle::m_IsOn
		IL_005c: ldc.i4.0
		IL_005d: ceq
		IL_005f: brtrue.s IL_007a

		IL_0061: nop
		IL_0062: br.s IL_006e

		IL_0064: nop
		IL_0065: br.s IL_006e

		IL_0067: nop
		IL_0068: br.s IL_006e

		IL_006a: nop
		IL_006b: br.s IL_006e

		IL_006d: nop

		IL_006e: nop
		IL_006f: ldstr "UiSize"
		IL_0074: ldc.i4.0
		IL_0075: call void [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::SetInt(string, int32)

		IL_007a: nop
		IL_007b: ldarg.0
		IL_007c: ldfld class [UnityEngine.UI]UnityEngine.UI.Toggle UiSettings::hLow
		IL_0081: ldfld bool [UnityEngine.UI]UnityEngine.UI.Toggle::m_IsOn
		IL_0086: ldc.i4.0
		IL_0087: ceq
		IL_0089: ldc.i4.0
		IL_008a: ceq
		IL_008c: brtrue.s IL_00d1

		IL_008e: nop
		IL_008f: ldarg.0
		IL_0090: ldfld class [UnityEngine.UI]UnityEngine.UI.Toggle UiSettings::hMed
		IL_0095: ldfld bool [UnityEngine.UI]UnityEngine.UI.Toggle::m_IsOn
		IL_009a: ldc.i4.0
		IL_009b: ceq
		IL_009d: ldc.i4.0
		IL_009e: ceq
		IL_00a0: brtrue.s IL_00c4

		IL_00a2: nop
		IL_00a3: ldarg.0
		IL_00a4: ldfld class [UnityEngine.UI]UnityEngine.UI.Toggle UiSettings::hHigh
		IL_00a9: stloc.0
		IL_00aa: ldloc.0
		IL_00ab: ldfld bool [UnityEngine.UI]UnityEngine.UI.Toggle::m_IsOn
		IL_00b0: ldc.i4.0
		IL_00b1: ceq
		IL_00b3: brtrue.s IL_00c2

		IL_00b5: nop
		IL_00b6: ldstr "UiHeight"
		IL_00bb: ldc.i4.2
		IL_00bc: call void [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::SetInt(string, int32)
		IL_00c1: ret

		IL_00c2: nop
		IL_00c3: ret

		IL_00c4: nop
		IL_00c5: ldstr "UiHeight"
		IL_00ca: ldc.i4.1
		IL_00cb: call void [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::SetInt(string, int32)
		IL_00d0: ret

		IL_00d1: nop
		IL_00d2: ldstr "UiHeight"
		IL_00d7: ldc.i4.0
		IL_00d8: call void [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::SetInt(string, int32)
		IL_00dd: ret

		IL_00de: nop
		IL_00df: ldnull
		IL_00e0: throw
	} // end of method UiSettings::UpdateState

	.method public hidebysig 
		instance void RestoreState () cil managed 
	{
		// Method begins at RVA 0xac34
		// Header size: 12
		// Code size: 238 (0xee)
		.maxstack 2
		.locals init (
			[0] int32,
			[1] int32,
			[2] int32,
			[3] class [UnityEngine.UI]UnityEngine.UI.Toggle
		)

		IL_0000: nop
		IL_0001: ldstr "UiSize"
		IL_0006: call int32 [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::GetInt(string)
		IL_000b: stloc.0
		IL_000c: ldarg.0
		IL_000d: ldloc.0
		IL_000e: stfld int32 UiSettings::size
		IL_0013: ldstr "UiHeight"
		IL_0018: call int32 [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::GetInt(string)
		IL_001d: stloc.1
		IL_001e: ldarg.0
		IL_001f: ldloc.1
		IL_0020: stfld int32 UiSettings::height
		IL_0025: ldloc.1
		IL_0026: stloc.2
		IL_0027: ldarg.0
		IL_0028: ldfld int32 UiSettings::size
		IL_002d: ldc.i4.0
		IL_002e: ceq
		IL_0030: brtrue.s IL_008d

		IL_0032: nop
		IL_0033: ldarg.0
		IL_0034: ldfld int32 UiSettings::size
		IL_0039: ldc.i4.1
		IL_003a: ceq
		IL_003c: brtrue.s IL_0083

		IL_003e: nop
		IL_003f: ldarg.0
		IL_0040: ldfld int32 UiSettings::size
		IL_0045: ldc.i4.2
		IL_0046: ceq
		IL_0048: brtrue.s IL_0079

		IL_004a: nop
		IL_004b: ldarg.0
		IL_004c: ldfld int32 UiSettings::size
		IL_0051: ldc.i4.3
		IL_0052: ceq
		IL_0054: brtrue.s IL_006f

		IL_0056: nop
		IL_0057: ldarg.0
		IL_0058: ldfld int32 UiSettings::size
		IL_005d: ldc.i4.4
		IL_005e: ceq
		IL_0060: ldc.i4.0
		IL_0061: ceq
		IL_0063: brtrue.s IL_009d

		IL_0065: nop
		IL_0066: ldarg.0
		IL_0067: ldfld class [UnityEngine.UI]UnityEngine.UI.Toggle UiSettings::sSmall
		IL_006c: stloc.3
		IL_006d: br.s IL_0095

		IL_006f: nop
		IL_0070: ldarg.0
		IL_0071: ldfld class [UnityEngine.UI]UnityEngine.UI.Toggle UiSettings::sMed
		IL_0076: stloc.3
		IL_0077: br.s IL_0095

		IL_0079: nop
		IL_007a: ldarg.0
		IL_007b: ldfld class [UnityEngine.UI]UnityEngine.UI.Toggle UiSettings::sLarge
		IL_0080: stloc.3
		IL_0081: br.s IL_0095

		IL_0083: nop
		IL_0084: ldarg.0
		IL_0085: ldfld class [UnityEngine.UI]UnityEngine.UI.Toggle UiSettings::sXLarge
		IL_008a: stloc.3
		IL_008b: br.s IL_0095

		IL_008d: nop
		IL_008e: ldarg.0
		IL_008f: ldfld class [UnityEngine.UI]UnityEngine.UI.Toggle UiSettings::sAuto
		IL_0094: stloc.3

		IL_0095: nop
		IL_0096: ldloc.3
		IL_0097: ldc.i4.1
		IL_0098: callvirt instance void [UnityEngine.UI]UnityEngine.UI.Toggle::set_isOn(bool)

		IL_009d: nop
		IL_009e: ldarg.0
		IL_009f: ldfld int32 UiSettings::height
		IL_00a4: ldc.i4.0
		IL_00a5: ceq
		IL_00a7: brtrue.s IL_00e1

		IL_00a9: nop
		IL_00aa: ldarg.0
		IL_00ab: ldfld int32 UiSettings::height
		IL_00b0: ldc.i4.1
		IL_00b1: ceq
		IL_00b3: brtrue.s IL_00d7

		IL_00b5: nop
		IL_00b6: ldarg.0
		IL_00b7: ldfld int32 UiSettings::height
		IL_00bc: ldc.i4.2
		IL_00bd: ceq
		IL_00bf: ldc.i4.0
		IL_00c0: ceq
		IL_00c2: brtrue.s IL_00d5

		IL_00c4: nop
		IL_00c5: ldarg.0
		IL_00c6: ldfld class [UnityEngine.UI]UnityEngine.UI.Toggle UiSettings::hHigh
		IL_00cb: stloc.3

		IL_00cc: nop
		IL_00cd: ldloc.3
		IL_00ce: ldc.i4.1
		IL_00cf: callvirt instance void [UnityEngine.UI]UnityEngine.UI.Toggle::set_isOn(bool)
		IL_00d4: ret

		IL_00d5: nop
		IL_00d6: ret

		IL_00d7: nop
		IL_00d8: ldarg.0
		IL_00d9: ldfld class [UnityEngine.UI]UnityEngine.UI.Toggle UiSettings::hMed
		IL_00de: stloc.3
		IL_00df: br.s IL_00cc

		IL_00e1: nop
		IL_00e2: ldarg.0
		IL_00e3: ldfld class [UnityEngine.UI]UnityEngine.UI.Toggle UiSettings::hLow
		IL_00e8: stloc.3
		IL_00e9: br.s IL_00cc

		IL_00eb: nop
		IL_00ec: ldnull
		IL_00ed: throw
	} // end of method UiSettings::RestoreState

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x243b
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method UiSettings::.ctor

} // end of class UiSettings
