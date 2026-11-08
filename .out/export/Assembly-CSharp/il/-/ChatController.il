.class public auto ansi beforefieldinit ChatController
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class [Unity.TextMeshPro]TMPro.TMP_InputField TMP_ChatInput
	.field public class [Unity.TextMeshPro]TMPro.TMP_Text TMP_ChatOutput
	.field public class [UnityEngine.UI]UnityEngine.UI.Scrollbar ChatScrollbar

	// Methods
	.method private hidebysig 
		instance void OnEnable () cil managed 
	{
		// Method begins at RVA 0x13fd0
		// Header size: 12
		// Code size: 37 (0x25)
		.maxstack 2
		.locals init (
			[0] class [Unity.TextMeshPro]TMPro.TMP_InputField/SubmitEvent,
			[1] class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`1<string>
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [Unity.TextMeshPro]TMPro.TMP_InputField ChatController::TMP_ChatInput
		IL_0007: ldfld class [Unity.TextMeshPro]TMPro.TMP_InputField/SubmitEvent [Unity.TextMeshPro]TMPro.TMP_InputField::m_OnSubmit
		IL_000c: stloc.0
		IL_000d: ldarg.0
		IL_000e: ldftn instance void ChatController::AddToChatOutput(string)
		IL_0014: newobj instance void [mscorlib]System.AsyncCallback::.ctor(object, native int)
		IL_0019: stloc.1
		IL_001a: ldloc.0
		IL_001b: ldloc.1
		IL_001c: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`1<string>::AddListener(class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`1<!0>)
		IL_0021: ret

		IL_0022: nop
		IL_0023: ldnull
		IL_0024: throw
	} // end of method ChatController::OnEnable

	.method private hidebysig 
		instance void OnDisable () cil managed 
	{
		// Method begins at RVA 0x14004
		// Header size: 12
		// Code size: 37 (0x25)
		.maxstack 2
		.locals init (
			[0] class [Unity.TextMeshPro]TMPro.TMP_InputField/SubmitEvent,
			[1] class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`1<string>
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [Unity.TextMeshPro]TMPro.TMP_InputField ChatController::TMP_ChatInput
		IL_0007: ldfld class [Unity.TextMeshPro]TMPro.TMP_InputField/SubmitEvent [Unity.TextMeshPro]TMPro.TMP_InputField::m_OnSubmit
		IL_000c: stloc.0
		IL_000d: ldarg.0
		IL_000e: ldftn instance void ChatController::AddToChatOutput(string)
		IL_0014: newobj instance void [mscorlib]System.AsyncCallback::.ctor(object, native int)
		IL_0019: stloc.1
		IL_001a: ldloc.0
		IL_001b: ldloc.1
		IL_001c: callvirt instance void class [UnityEngine.CoreModule]UnityEngine.Events.UnityEvent`1<string>::RemoveListener(class [UnityEngine.CoreModule]UnityEngine.Events.UnityAction`1<!0>)
		IL_0021: ret

		IL_0022: nop
		IL_0023: ldnull
		IL_0024: throw
	} // end of method ChatController::OnDisable

	.method private hidebysig 
		instance void AddToChatOutput (
			string newText
		) cil managed 
	{
		// Method begins at RVA 0x14038
		// Header size: 12
		// Code size: 355 (0x163)
		.maxstack 3
		.locals init (
			[0] class [Unity.TextMeshPro]TMPro.TMP_InputField,
			[1] valuetype [mscorlib]System.DateTime,
			[2] string[],
			[3] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[4] valuetype [mscorlib]System.DateTime,
			[5] int32,
			[6] int32,
			[7] string,
			[8] valuetype [mscorlib]System.DateTime,
			[9] int32,
			[10] int32,
			[11] string,
			[12] valuetype [mscorlib]System.DateTime,
			[13] int32,
			[14] int32,
			[15] string,
			[16] string
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [Unity.TextMeshPro]TMPro.TMP_InputField ChatController::TMP_ChatInput
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: ldstr "*(/*typedef#-1*/object.static_fields + 0x0)"
		IL_000e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0013: box [mscorlib]System.Int64
		IL_0018: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<string>(object)
		IL_001d: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_InputField::set_text(string)
		IL_0022: br.s IL_0025

		IL_0024: nop

		IL_0025: nop
		IL_0026: call valuetype [mscorlib]System.DateTime [mscorlib]System.DateTime::get_Now()
		IL_002b: stloc.1
		IL_002c: ldc.i4.s 10
		IL_002e: newarr [mscorlib]System.String
		IL_0033: stloc.2
		IL_0034: ldarg.0
		IL_0035: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text ChatController::TMP_ChatOutput
		IL_003a: stloc.3
		IL_003b: ldloc.2
		IL_003c: ldc.i4.0
		IL_003d: ldloc.2
		IL_003e: stelem [mscorlib]System.String
		IL_0043: ldloc.2
		IL_0044: ldc.i4.1
		IL_0045: ldstr "[<#FFFF80>"
		IL_004a: stelem [mscorlib]System.String
		IL_004f: ldloca.s 4
		IL_0051: ldloc.1
		IL_0052: box [mscorlib]System.Object
		IL_0057: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<uint64>(object)
		IL_005c: stfld uint64 [mscorlib]System.DateTime::_dateData
		IL_0061: ldloca.s 4
		IL_0063: call instance int32 [mscorlib]System.DateTime::get_Hour()
		IL_0068: stloc.s 5
		IL_006a: ldloca.s 6
		IL_006c: ldloc.s 5
		IL_006e: stfld int32 [mscorlib]System.Int32::m_value
		IL_0073: ldloca.s 6
		IL_0075: ldstr "d2"
		IL_007a: call instance string [mscorlib]System.Int32::ToString(string)
		IL_007f: stloc.s 7
		IL_0081: ldloc.2
		IL_0082: ldc.i4.2
		IL_0083: ldloc.s 7
		IL_0085: stelem [mscorlib]System.String
		IL_008a: ldloc.2
		IL_008b: ldc.i4.3
		IL_008c: ldstr ":"
		IL_0091: stelem [mscorlib]System.String
		IL_0096: ldloca.s 8
		IL_0098: ldloc.1
		IL_0099: box [mscorlib]System.Object
		IL_009e: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<uint64>(object)
		IL_00a3: stfld uint64 [mscorlib]System.DateTime::_dateData
		IL_00a8: ldloca.s 8
		IL_00aa: call instance int32 [mscorlib]System.DateTime::get_Minute()
		IL_00af: stloc.s 9
		IL_00b1: ldloca.s 10
		IL_00b3: ldloc.s 9
		IL_00b5: stfld int32 [mscorlib]System.Int32::m_value
		IL_00ba: ldloca.s 10
		IL_00bc: ldstr "d2"
		IL_00c1: call instance string [mscorlib]System.Int32::ToString(string)
		IL_00c6: stloc.s 11
		IL_00c8: ldloc.2
		IL_00c9: ldc.i4.4
		IL_00ca: ldloc.s 11
		IL_00cc: stelem [mscorlib]System.String
		IL_00d1: ldloc.2
		IL_00d2: ldc.i4.5
		IL_00d3: ldstr ":"
		IL_00d8: stelem [mscorlib]System.String
		IL_00dd: ldloca.s 12
		IL_00df: ldloc.1
		IL_00e0: box [mscorlib]System.Object
		IL_00e5: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<uint64>(object)
		IL_00ea: stfld uint64 [mscorlib]System.DateTime::_dateData
		IL_00ef: ldloca.s 12
		IL_00f1: call instance int32 [mscorlib]System.DateTime::get_Second()
		IL_00f6: stloc.s 13
		IL_00f8: ldloca.s 14
		IL_00fa: ldloc.s 13
		IL_00fc: stfld int32 [mscorlib]System.Int32::m_value
		IL_0101: ldloca.s 14
		IL_0103: ldstr "d2"
		IL_0108: call instance string [mscorlib]System.Int32::ToString(string)
		IL_010d: stloc.s 15
		IL_010f: ldloc.2
		IL_0110: ldc.i4.6
		IL_0111: ldloc.s 15
		IL_0113: stelem [mscorlib]System.String
		IL_0118: ldloc.2
		IL_0119: ldc.i4.7
		IL_011a: ldstr "</color>] "
		IL_011f: stelem [mscorlib]System.String
		IL_0124: ldloc.2
		IL_0125: ldc.i4.8
		IL_0126: ldarg.1
		IL_0127: stelem [mscorlib]System.String
		IL_012c: ldloc.2
		IL_012d: ldc.i4.s 9
		IL_012f: ldstr "\n"
		IL_0134: stelem [mscorlib]System.String
		IL_0139: ldloc.2
		IL_013a: call string [mscorlib]System.String::Concat(string[])
		IL_013f: stloc.s 16
		IL_0141: ldarg.0
		IL_0142: ldfld class [Unity.TextMeshPro]TMPro.TMP_InputField ChatController::TMP_ChatInput
		IL_0147: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_InputField::ActivateInputField()
		IL_014c: ldarg.0
		IL_014d: ldfld class [UnityEngine.UI]UnityEngine.UI.Scrollbar ChatController::ChatScrollbar
		IL_0152: ldc.r4 0.0
		IL_0157: callvirt instance void [UnityEngine.UI]UnityEngine.UI.Scrollbar::set_value(float32)
		IL_015c: ret

		IL_015d: nop
		IL_015e: ldnull
		IL_015f: throw

		IL_0160: nop
		IL_0161: ldnull
		IL_0162: throw
	} // end of method ChatController::AddToChatOutput

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2775
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method ChatController::.ctor

} // end of class ChatController
