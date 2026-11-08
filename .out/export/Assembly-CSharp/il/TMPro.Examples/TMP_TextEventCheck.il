.class public auto ansi beforefieldinit TMPro.Examples.TMP_TextEventCheck
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class TMPro.TMP_TextEventHandler TextEventHandler

	// Methods
	.method private hidebysig 
		instance void OnEnable () cil managed 
	{
		// Method begins at RVA 0x1770c
		// Header size: 12
		// Code size: 227 (0xe3)
		.maxstack 2
		.locals init (
			[0] class TMPro.TMP_TextEventHandler,
			[1] bool,
			[2] bool,
			[3] class TMPro.TMP_TextEventHandler,
			[4] class TMPro.TMP_TextEventHandler/CharacterSelectionEvent,
			[5] class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`2<char, int32>,
			[6] class TMPro.TMP_TextEventHandler/SpriteSelectionEvent,
			[7] class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`2<char, int32>,
			[8] class TMPro.TMP_TextEventHandler/WordSelectionEvent,
			[9] class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`3<string, int32, int32>,
			[10] class TMPro.TMP_TextEventHandler/LineSelectionEvent,
			[11] class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`3<string, int32, int32>,
			[12] class TMPro.TMP_TextEventHandler/LinkSelectionEvent,
			[13] class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`3<string, string, int32>
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class TMPro.TMP_TextEventHandler TMPro.Examples.TMP_TextEventCheck::TextEventHandler
		IL_0007: stloc.0
		IL_0008: br.s IL_000b

		IL_000a: nop

		IL_000b: nop
		IL_000c: ldloc.0
		IL_000d: ldnull
		IL_000e: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Inequality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0013: stloc.1
		IL_0014: ldloc.1
		IL_0015: stloc.2
		IL_0016: ldloc.1
		IL_0017: ldc.i4.0
		IL_0018: ceq
		IL_001a: brtrue IL_00de

		IL_001f: nop
		IL_0020: ldarg.0
		IL_0021: ldfld class TMPro.TMP_TextEventHandler TMPro.Examples.TMP_TextEventCheck::TextEventHandler
		IL_0026: stloc.3
		IL_0027: ldloc.3
		IL_0028: ldfld class TMPro.TMP_TextEventHandler/CharacterSelectionEvent TMPro.TMP_TextEventHandler::m_OnCharacterSelection
		IL_002d: stloc.s 4
		IL_002f: ldarg.0
		IL_0030: ldftn instance void TMPro.Examples.TMP_TextEventCheck::OnCharacterSelection(char, int32)
		IL_0036: newobj instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`2<char, int32>::.ctor(object, native int)
		IL_003b: stloc.s 5
		IL_003d: ldloc.s 4
		IL_003f: ldloc.s 5
		IL_0041: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`2<char, int32>::AddListener(class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`2<!0, !1>)
		IL_0046: ldarg.0
		IL_0047: ldfld class TMPro.TMP_TextEventHandler TMPro.Examples.TMP_TextEventCheck::TextEventHandler
		IL_004c: stloc.3
		IL_004d: ldloc.3
		IL_004e: ldfld class TMPro.TMP_TextEventHandler/SpriteSelectionEvent TMPro.TMP_TextEventHandler::m_OnSpriteSelection
		IL_0053: stloc.s 6
		IL_0055: ldarg.0
		IL_0056: ldftn instance void TMPro.Examples.TMP_TextEventCheck::OnSpriteSelection(char, int32)
		IL_005c: newobj instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`2<char, int32>::.ctor(object, native int)
		IL_0061: stloc.s 7
		IL_0063: ldloc.s 6
		IL_0065: ldloc.s 7
		IL_0067: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`2<char, int32>::AddListener(class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`2<!0, !1>)
		IL_006c: ldarg.0
		IL_006d: ldfld class TMPro.TMP_TextEventHandler TMPro.Examples.TMP_TextEventCheck::TextEventHandler
		IL_0072: stloc.3
		IL_0073: ldloc.3
		IL_0074: ldfld class TMPro.TMP_TextEventHandler/WordSelectionEvent TMPro.TMP_TextEventHandler::m_OnWordSelection
		IL_0079: stloc.s 8
		IL_007b: ldarg.0
		IL_007c: ldftn instance void TMPro.Examples.TMP_TextEventCheck::OnWordSelection(string, int32, int32)
		IL_0082: newobj instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`3<object, int32, int32>::.ctor(object, native int)
		IL_0087: stloc.s 9
		IL_0089: ldloc.s 8
		IL_008b: ldloc.s 9
		IL_008d: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`3<string, int32, int32>::AddListener(class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`3<!0, !1, !2>)
		IL_0092: ldarg.0
		IL_0093: ldfld class TMPro.TMP_TextEventHandler TMPro.Examples.TMP_TextEventCheck::TextEventHandler
		IL_0098: stloc.3
		IL_0099: ldloc.3
		IL_009a: ldfld class TMPro.TMP_TextEventHandler/LineSelectionEvent TMPro.TMP_TextEventHandler::m_OnLineSelection
		IL_009f: stloc.s 10
		IL_00a1: ldarg.0
		IL_00a2: ldftn instance void TMPro.Examples.TMP_TextEventCheck::OnLineSelection(string, int32, int32)
		IL_00a8: newobj instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`3<object, int32, int32>::.ctor(object, native int)
		IL_00ad: stloc.s 11
		IL_00af: ldloc.s 10
		IL_00b1: ldloc.s 11
		IL_00b3: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`3<string, int32, int32>::AddListener(class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`3<!0, !1, !2>)
		IL_00b8: ldarg.0
		IL_00b9: ldfld class TMPro.TMP_TextEventHandler TMPro.Examples.TMP_TextEventCheck::TextEventHandler
		IL_00be: stloc.3
		IL_00bf: ldloc.3
		IL_00c0: ldfld class TMPro.TMP_TextEventHandler/LinkSelectionEvent TMPro.TMP_TextEventHandler::m_OnLinkSelection
		IL_00c5: stloc.s 12
		IL_00c7: ldarg.0
		IL_00c8: ldftn instance void TMPro.Examples.TMP_TextEventCheck::OnLinkSelection(string, string, int32)
		IL_00ce: newobj instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`3<object, object, int32>::.ctor(object, native int)
		IL_00d3: stloc.s 13
		IL_00d5: ldloc.s 12
		IL_00d7: ldloc.s 13
		IL_00d9: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`3<string, string, int32>::AddListener(class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`3<!0, !1, !2>)

		IL_00de: nop
		IL_00df: ret

		IL_00e0: nop
		IL_00e1: ldnull
		IL_00e2: throw
	} // end of method TMP_TextEventCheck::OnEnable

	.method private hidebysig 
		instance void OnDisable () cil managed 
	{
		// Method begins at RVA 0x177fc
		// Header size: 12
		// Code size: 227 (0xe3)
		.maxstack 2
		.locals init (
			[0] class TMPro.TMP_TextEventHandler,
			[1] bool,
			[2] bool,
			[3] class TMPro.TMP_TextEventHandler,
			[4] class TMPro.TMP_TextEventHandler/CharacterSelectionEvent,
			[5] class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`2<char, int32>,
			[6] class TMPro.TMP_TextEventHandler/SpriteSelectionEvent,
			[7] class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`2<char, int32>,
			[8] class TMPro.TMP_TextEventHandler/WordSelectionEvent,
			[9] class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`3<string, int32, int32>,
			[10] class TMPro.TMP_TextEventHandler/LineSelectionEvent,
			[11] class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`3<string, int32, int32>,
			[12] class TMPro.TMP_TextEventHandler/LinkSelectionEvent,
			[13] class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`3<string, string, int32>
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class TMPro.TMP_TextEventHandler TMPro.Examples.TMP_TextEventCheck::TextEventHandler
		IL_0007: stloc.0
		IL_0008: br.s IL_000b

		IL_000a: nop

		IL_000b: nop
		IL_000c: ldloc.0
		IL_000d: ldnull
		IL_000e: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Inequality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0013: stloc.1
		IL_0014: ldloc.1
		IL_0015: stloc.2
		IL_0016: ldloc.1
		IL_0017: ldc.i4.0
		IL_0018: ceq
		IL_001a: brtrue IL_00de

		IL_001f: nop
		IL_0020: ldarg.0
		IL_0021: ldfld class TMPro.TMP_TextEventHandler TMPro.Examples.TMP_TextEventCheck::TextEventHandler
		IL_0026: stloc.3
		IL_0027: ldloc.3
		IL_0028: ldfld class TMPro.TMP_TextEventHandler/CharacterSelectionEvent TMPro.TMP_TextEventHandler::m_OnCharacterSelection
		IL_002d: stloc.s 4
		IL_002f: ldarg.0
		IL_0030: ldftn instance void TMPro.Examples.TMP_TextEventCheck::OnCharacterSelection(char, int32)
		IL_0036: newobj instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`2<char, int32>::.ctor(object, native int)
		IL_003b: stloc.s 5
		IL_003d: ldloc.s 4
		IL_003f: ldloc.s 5
		IL_0041: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`1<bool>::RemoveListener(class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`1<!0>)
		IL_0046: ldarg.0
		IL_0047: ldfld class TMPro.TMP_TextEventHandler TMPro.Examples.TMP_TextEventCheck::TextEventHandler
		IL_004c: stloc.3
		IL_004d: ldloc.3
		IL_004e: ldfld class TMPro.TMP_TextEventHandler/SpriteSelectionEvent TMPro.TMP_TextEventHandler::m_OnSpriteSelection
		IL_0053: stloc.s 6
		IL_0055: ldarg.0
		IL_0056: ldftn instance void TMPro.Examples.TMP_TextEventCheck::OnSpriteSelection(char, int32)
		IL_005c: newobj instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`2<char, int32>::.ctor(object, native int)
		IL_0061: stloc.s 7
		IL_0063: ldloc.s 6
		IL_0065: ldloc.s 7
		IL_0067: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`1<bool>::RemoveListener(class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`1<!0>)
		IL_006c: ldarg.0
		IL_006d: ldfld class TMPro.TMP_TextEventHandler TMPro.Examples.TMP_TextEventCheck::TextEventHandler
		IL_0072: stloc.3
		IL_0073: ldloc.3
		IL_0074: ldfld class TMPro.TMP_TextEventHandler/WordSelectionEvent TMPro.TMP_TextEventHandler::m_OnWordSelection
		IL_0079: stloc.s 8
		IL_007b: ldarg.0
		IL_007c: ldftn instance void TMPro.Examples.TMP_TextEventCheck::OnWordSelection(string, int32, int32)
		IL_0082: newobj instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`3<object, int32, int32>::.ctor(object, native int)
		IL_0087: stloc.s 9
		IL_0089: ldloc.s 8
		IL_008b: ldloc.s 9
		IL_008d: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`1<bool>::RemoveListener(class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`1<!0>)
		IL_0092: ldarg.0
		IL_0093: ldfld class TMPro.TMP_TextEventHandler TMPro.Examples.TMP_TextEventCheck::TextEventHandler
		IL_0098: stloc.3
		IL_0099: ldloc.3
		IL_009a: ldfld class TMPro.TMP_TextEventHandler/LineSelectionEvent TMPro.TMP_TextEventHandler::m_OnLineSelection
		IL_009f: stloc.s 10
		IL_00a1: ldarg.0
		IL_00a2: ldftn instance void TMPro.Examples.TMP_TextEventCheck::OnLineSelection(string, int32, int32)
		IL_00a8: newobj instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`3<object, int32, int32>::.ctor(object, native int)
		IL_00ad: stloc.s 11
		IL_00af: ldloc.s 10
		IL_00b1: ldloc.s 11
		IL_00b3: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`1<bool>::RemoveListener(class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`1<!0>)
		IL_00b8: ldarg.0
		IL_00b9: ldfld class TMPro.TMP_TextEventHandler TMPro.Examples.TMP_TextEventCheck::TextEventHandler
		IL_00be: stloc.3
		IL_00bf: ldloc.3
		IL_00c0: ldfld class TMPro.TMP_TextEventHandler/LinkSelectionEvent TMPro.TMP_TextEventHandler::m_OnLinkSelection
		IL_00c5: stloc.s 12
		IL_00c7: ldarg.0
		IL_00c8: ldftn instance void TMPro.Examples.TMP_TextEventCheck::OnLinkSelection(string, string, int32)
		IL_00ce: newobj instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`3<object, object, int32>::.ctor(object, native int)
		IL_00d3: stloc.s 13
		IL_00d5: ldloc.s 12
		IL_00d7: ldloc.s 13
		IL_00d9: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`1<bool>::RemoveListener(class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`1<!0>)

		IL_00de: nop
		IL_00df: ret

		IL_00e0: nop
		IL_00e1: ldnull
		IL_00e2: throw
	} // end of method TMP_TextEventCheck::OnDisable

	.method private hidebysig 
		instance void OnCharacterSelection (
			char c,
			int32 index
		) cil managed 
	{
		// Method begins at RVA 0x178ec
		// Header size: 12
		// Code size: 114 (0x72)
		.maxstack 3
		.locals init (
			[0] string[],
			[1] string[],
			[2] char,
			[3] string,
			[4] int32,
			[5] string,
			[6] string,
			[7] string
		)

		IL_0000: nop
		IL_0001: ldc.i4.5
		IL_0002: newarr [mscorlib]System.String
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: stloc.1
		IL_000a: ldloc.0
		IL_000b: ldc.i4.0
		IL_000c: ldstr "Character ["
		IL_0011: stelem [mscorlib]System.String
		IL_0016: br.s IL_0019

		IL_0018: nop

		IL_0019: nop
		IL_001a: ldloca.s 2
		IL_001c: call instance string [mscorlib]System.Char::ToString()
		IL_0021: stloc.3
		IL_0022: ldloc.1
		IL_0023: ldc.i4.1
		IL_0024: ldloc.3
		IL_0025: stelem [mscorlib]System.String
		IL_002a: ldloc.1
		IL_002b: ldc.i4.2
		IL_002c: ldstr "] at Index: "
		IL_0031: stelem [mscorlib]System.String
		IL_0036: ldloca.s 4
		IL_0038: call instance string [mscorlib]System.Int32::ToString()
		IL_003d: stloc.s 5
		IL_003f: ldloc.1
		IL_0040: ldc.i4.3
		IL_0041: ldloc.s 5
		IL_0043: stelem [mscorlib]System.String
		IL_0048: ldloc.1
		IL_0049: ldc.i4.4
		IL_004a: ldstr " has been selected."
		IL_004f: stelem [mscorlib]System.String
		IL_0054: ldloc.1
		IL_0055: call string [mscorlib]System.String::Concat(string[])
		IL_005a: stloc.s 6
		IL_005c: ldloc.s 6
		IL_005e: stloc.s 7
		IL_0060: br.s IL_0063

		IL_0062: nop

		IL_0063: nop
		IL_0064: ldloc.s 7
		IL_0066: call void [UnityEngine.CoreModule]UnityEngine.Debug::Log(object)
		IL_006b: ret

		IL_006c: nop
		IL_006d: ldnull
		IL_006e: throw

		IL_006f: nop
		IL_0070: ldnull
		IL_0071: throw
	} // end of method TMP_TextEventCheck::OnCharacterSelection

	.method private hidebysig 
		instance void OnSpriteSelection (
			char c,
			int32 index
		) cil managed 
	{
		// Method begins at RVA 0x1796c
		// Header size: 12
		// Code size: 114 (0x72)
		.maxstack 3
		.locals init (
			[0] string[],
			[1] string[],
			[2] char,
			[3] string,
			[4] int32,
			[5] string,
			[6] string,
			[7] string
		)

		IL_0000: nop
		IL_0001: ldc.i4.5
		IL_0002: newarr [mscorlib]System.String
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: stloc.1
		IL_000a: ldloc.0
		IL_000b: ldc.i4.0
		IL_000c: ldstr "Sprite ["
		IL_0011: stelem [mscorlib]System.String
		IL_0016: br.s IL_0019

		IL_0018: nop

		IL_0019: nop
		IL_001a: ldloca.s 2
		IL_001c: call instance string [mscorlib]System.Char::ToString()
		IL_0021: stloc.3
		IL_0022: ldloc.1
		IL_0023: ldc.i4.1
		IL_0024: ldloc.3
		IL_0025: stelem [mscorlib]System.String
		IL_002a: ldloc.1
		IL_002b: ldc.i4.2
		IL_002c: ldstr "] at Index: "
		IL_0031: stelem [mscorlib]System.String
		IL_0036: ldloca.s 4
		IL_0038: call instance string [mscorlib]System.Int32::ToString()
		IL_003d: stloc.s 5
		IL_003f: ldloc.1
		IL_0040: ldc.i4.3
		IL_0041: ldloc.s 5
		IL_0043: stelem [mscorlib]System.String
		IL_0048: ldloc.1
		IL_0049: ldc.i4.4
		IL_004a: ldstr " has been selected."
		IL_004f: stelem [mscorlib]System.String
		IL_0054: ldloc.1
		IL_0055: call string [mscorlib]System.String::Concat(string[])
		IL_005a: stloc.s 6
		IL_005c: ldloc.s 6
		IL_005e: stloc.s 7
		IL_0060: br.s IL_0063

		IL_0062: nop

		IL_0063: nop
		IL_0064: ldloc.s 7
		IL_0066: call void [UnityEngine.CoreModule]UnityEngine.Debug::Log(object)
		IL_006b: ret

		IL_006c: nop
		IL_006d: ldnull
		IL_006e: throw

		IL_006f: nop
		IL_0070: ldnull
		IL_0071: throw
	} // end of method TMP_TextEventCheck::OnSpriteSelection

	.method private hidebysig 
		instance void OnWordSelection (
			string word,
			int32 firstCharacterIndex,
			int32 length
		) cil managed 
	{
		// Method begins at RVA 0x179ec
		// Header size: 12
		// Code size: 146 (0x92)
		.maxstack 3
		.locals init (
			[0] string[],
			[1] string[],
			[2] int32,
			[3] string,
			[4] int32,
			[5] string,
			[6] string,
			[7] string
		)

		IL_0000: nop
		IL_0001: ldc.i4.7
		IL_0002: newarr [mscorlib]System.String
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: stloc.1
		IL_000a: ldloc.0
		IL_000b: ldc.i4.0
		IL_000c: ldstr "Word ["
		IL_0011: stelem [mscorlib]System.String
		IL_0016: ldloc.1
		IL_0017: ldc.i4.1
		IL_0018: ldarg.1
		IL_0019: stelem [mscorlib]System.String
		IL_001e: ldloc.1
		IL_001f: ldc.i4.2
		IL_0020: ldstr "] with first character index of "
		IL_0025: stelem [mscorlib]System.String
		IL_002a: ldloca.s 2
		IL_002c: ldarg.2
		IL_002d: stfld int32 [mscorlib]System.Int32::m_value
		IL_0032: ldloca.s 2
		IL_0034: call instance string [mscorlib]System.Int32::ToString()
		IL_0039: stloc.3
		IL_003a: ldloc.1
		IL_003b: ldc.i4.3
		IL_003c: ldloc.3
		IL_003d: stelem [mscorlib]System.String
		IL_0042: ldloc.1
		IL_0043: ldc.i4.4
		IL_0044: ldstr " and length of "
		IL_0049: stelem [mscorlib]System.String
		IL_004e: ldloca.s 4
		IL_0050: ldarg.3
		IL_0051: stfld int32 [mscorlib]System.Int32::m_value
		IL_0056: ldloca.s 4
		IL_0058: call instance string [mscorlib]System.Int32::ToString()
		IL_005d: stloc.s 5
		IL_005f: ldloc.1
		IL_0060: ldc.i4.5
		IL_0061: ldloc.s 5
		IL_0063: stelem [mscorlib]System.String
		IL_0068: ldloc.1
		IL_0069: ldc.i4.6
		IL_006a: ldstr " has been selected."
		IL_006f: stelem [mscorlib]System.String
		IL_0074: ldloc.1
		IL_0075: call string [mscorlib]System.String::Concat(string[])
		IL_007a: stloc.s 6
		IL_007c: ldloc.s 6
		IL_007e: stloc.s 7
		IL_0080: br.s IL_0083

		IL_0082: nop

		IL_0083: nop
		IL_0084: ldloc.s 7
		IL_0086: call void [UnityEngine.CoreModule]UnityEngine.Debug::Log(object)
		IL_008b: ret

		IL_008c: nop
		IL_008d: ldnull
		IL_008e: throw

		IL_008f: nop
		IL_0090: ldnull
		IL_0091: throw
	} // end of method TMP_TextEventCheck::OnWordSelection

	.method private hidebysig 
		instance void OnLineSelection (
			string lineText,
			int32 firstCharacterIndex,
			int32 length
		) cil managed 
	{
		// Method begins at RVA 0x17a8c
		// Header size: 12
		// Code size: 146 (0x92)
		.maxstack 3
		.locals init (
			[0] string[],
			[1] string[],
			[2] int32,
			[3] string,
			[4] int32,
			[5] string,
			[6] string,
			[7] string
		)

		IL_0000: nop
		IL_0001: ldc.i4.7
		IL_0002: newarr [mscorlib]System.String
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: stloc.1
		IL_000a: ldloc.0
		IL_000b: ldc.i4.0
		IL_000c: ldstr "Line ["
		IL_0011: stelem [mscorlib]System.String
		IL_0016: ldloc.1
		IL_0017: ldc.i4.1
		IL_0018: ldarg.1
		IL_0019: stelem [mscorlib]System.String
		IL_001e: ldloc.1
		IL_001f: ldc.i4.2
		IL_0020: ldstr "] with first character index of "
		IL_0025: stelem [mscorlib]System.String
		IL_002a: ldloca.s 2
		IL_002c: ldarg.2
		IL_002d: stfld int32 [mscorlib]System.Int32::m_value
		IL_0032: ldloca.s 2
		IL_0034: call instance string [mscorlib]System.Int32::ToString()
		IL_0039: stloc.3
		IL_003a: ldloc.1
		IL_003b: ldc.i4.3
		IL_003c: ldloc.3
		IL_003d: stelem [mscorlib]System.String
		IL_0042: ldloc.1
		IL_0043: ldc.i4.4
		IL_0044: ldstr " and length of "
		IL_0049: stelem [mscorlib]System.String
		IL_004e: ldloca.s 4
		IL_0050: ldarg.3
		IL_0051: stfld int32 [mscorlib]System.Int32::m_value
		IL_0056: ldloca.s 4
		IL_0058: call instance string [mscorlib]System.Int32::ToString()
		IL_005d: stloc.s 5
		IL_005f: ldloc.1
		IL_0060: ldc.i4.5
		IL_0061: ldloc.s 5
		IL_0063: stelem [mscorlib]System.String
		IL_0068: ldloc.1
		IL_0069: ldc.i4.6
		IL_006a: ldstr " has been selected."
		IL_006f: stelem [mscorlib]System.String
		IL_0074: ldloc.1
		IL_0075: call string [mscorlib]System.String::Concat(string[])
		IL_007a: stloc.s 6
		IL_007c: ldloc.s 6
		IL_007e: stloc.s 7
		IL_0080: br.s IL_0083

		IL_0082: nop

		IL_0083: nop
		IL_0084: ldloc.s 7
		IL_0086: call void [UnityEngine.CoreModule]UnityEngine.Debug::Log(object)
		IL_008b: ret

		IL_008c: nop
		IL_008d: ldnull
		IL_008e: throw

		IL_008f: nop
		IL_0090: ldnull
		IL_0091: throw
	} // end of method TMP_TextEventCheck::OnLineSelection

	.method private hidebysig 
		instance void OnLinkSelection (
			string linkID,
			string linkText,
			int32 linkIndex
		) cil managed 
	{
		// Method begins at RVA 0x17b2c
		// Header size: 12
		// Code size: 128 (0x80)
		.maxstack 3
		.locals init (
			[0] string[],
			[1] string[],
			[2] int32,
			[3] string,
			[4] string,
			[5] string
		)

		IL_0000: nop
		IL_0001: ldc.i4.7
		IL_0002: newarr [mscorlib]System.String
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: stloc.1
		IL_000a: ldloc.0
		IL_000b: ldc.i4.0
		IL_000c: ldstr "Link Index: "
		IL_0011: stelem [mscorlib]System.String
		IL_0016: ldloca.s 2
		IL_0018: ldarg.3
		IL_0019: stfld int32 [mscorlib]System.Int32::m_value
		IL_001e: ldloca.s 2
		IL_0020: call instance string [mscorlib]System.Int32::ToString()
		IL_0025: stloc.3
		IL_0026: ldloc.1
		IL_0027: ldc.i4.1
		IL_0028: ldloc.3
		IL_0029: stelem [mscorlib]System.String
		IL_002e: ldloc.1
		IL_002f: ldc.i4.2
		IL_0030: ldstr " with ID ["
		IL_0035: stelem [mscorlib]System.String
		IL_003a: ldloc.1
		IL_003b: ldc.i4.3
		IL_003c: ldarg.1
		IL_003d: stelem [mscorlib]System.String
		IL_0042: ldloc.1
		IL_0043: ldc.i4.4
		IL_0044: ldstr "] and Text \""
		IL_0049: stelem [mscorlib]System.String
		IL_004e: ldloc.1
		IL_004f: ldc.i4.5
		IL_0050: ldarg.2
		IL_0051: stelem [mscorlib]System.String
		IL_0056: ldloc.1
		IL_0057: ldc.i4.6
		IL_0058: ldstr "\" has been selected."
		IL_005d: stelem [mscorlib]System.String
		IL_0062: ldloc.1
		IL_0063: call string [mscorlib]System.String::Concat(string[])
		IL_0068: stloc.s 4
		IL_006a: ldloc.s 4
		IL_006c: stloc.s 5
		IL_006e: br.s IL_0071

		IL_0070: nop

		IL_0071: nop
		IL_0072: ldloc.s 5
		IL_0074: call void [UnityEngine.CoreModule]UnityEngine.Debug::Log(object)
		IL_0079: ret

		IL_007a: nop
		IL_007b: ldnull
		IL_007c: throw

		IL_007d: nop
		IL_007e: ldnull
		IL_007f: throw
	} // end of method TMP_TextEventCheck::OnLinkSelection

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x29b0
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method TMP_TextEventCheck::.ctor

} // end of class TMPro.Examples.TMP_TextEventCheck
