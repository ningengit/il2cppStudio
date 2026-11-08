.class public auto ansi beforefieldinit DetentionTextScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class DoorScript door
	.field private class [Unity.TextMeshPro]TMPro.TMP_Text text

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x10250
		// Header size: 12
		// Code size: 16 (0x10)
		.maxstack 2
		.locals init (
			[0] class [Unity.TextMeshPro]TMPro.TMP_Text
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [Unity.TextMeshPro]TMPro.TMP_Text>()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [Unity.TextMeshPro]TMPro.TMP_Text DetentionTextScript::text
		IL_000f: ret
	} // end of method DetentionTextScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x1026c
		// Header size: 12
		// Code size: 150 (0x96)
		.maxstack 5
		.locals init (
			[0] class DoorScript,
			[1] float32,
			[2] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[3] float32,
			[4] int64,
			[5] int32,
			[6] string,
			[7] string,
			[8] string
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class DoorScript DetentionTextScript::door
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: ldfld float32 DoorScript::lockTime
		IL_000e: stloc.1
		IL_000f: ldarg.0
		IL_0010: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text DetentionTextScript::text
		IL_0015: stloc.2
		IL_0016: ldloc.0
		IL_0017: ldfld float32 DoorScript::lockTime
		IL_001c: ldc.r4 0.0
		IL_0021: cgt
		IL_0023: brtrue.s IL_0028

		IL_0025: nop

		IL_0026: nop
		IL_0027: ret

		IL_0028: nop
		IL_0029: ldloc.1
		IL_002a: stloc.3
		IL_002b: br.s IL_002e

		IL_002d: nop

		IL_002e: nop
		IL_002f: ldstr "sub_20C080"
		IL_0034: ldc.i8 0
		IL_003d: ldc.i8 0
		IL_0046: ldc.i8 0
		IL_004f: ldc.i8 0
		IL_0058: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_005d: stloc.s 4
		IL_005f: ldloca.s 5
		IL_0061: ldstr "xmm0"
		IL_0066: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_006b: conv.i4
		IL_006c: stfld int32 [mscorlib]System.Int32::m_value
		IL_0071: ldloca.s 5
		IL_0073: call instance string [mscorlib]System.Int32::ToString()
		IL_0078: stloc.s 6
		IL_007a: ldstr "You have detention! \n"
		IL_007f: ldloc.s 6
		IL_0081: ldstr " seconds remain!"
		IL_0086: call string [mscorlib]System.String::Concat(string, string, string)
		IL_008b: stloc.s 7
		IL_008d: ldloc.s 7
		IL_008f: stloc.s 8
		IL_0091: br.s IL_0026

		IL_0093: nop
		IL_0094: ldnull
		IL_0095: throw
	} // end of method DetentionTextScript::Update

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x25cf
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method DetentionTextScript::.ctor

} // end of class DetentionTextScript
