.class public auto ansi beforefieldinit EndlessTextScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class [Unity.TextMeshPro]TMPro.TMP_Text text

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0xad30
		// Header size: 12
		// Code size: 362 (0x16a)
		.maxstack 5
		.locals init (
			[0] object[],
			[1] object[],
			[2] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[3] object[],
			[4] object[],
			[5] int64,
			[6] int64,
			[7] int64,
			[8] int32,
			[9] object,
			[10] object,
			[11] object,
			[12] int64,
			[13] int64,
			[14] int64,
			[15] string
		)

		IL_0000: nop
		IL_0001: ldc.i4.4
		IL_0002: newarr [mscorlib]System.Object
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: stloc.1
		IL_000a: ldarg.0
		IL_000b: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text EndlessTextScript::text
		IL_0010: stloc.2
		IL_0011: ldloc.0
		IL_0012: stloc.3
		IL_0013: ldloc.3
		IL_0014: stloc.s 4
		IL_0016: ldloc.3
		IL_0017: ldnull
		IL_0018: ceq
		IL_001a: brtrue.s IL_004b

		IL_001c: nop
		IL_001d: ldstr "sub_1A4130"
		IL_0022: ldloc.3
		IL_0023: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0028: ldstr "*(LocalExpr->klass + 0x40)"
		IL_002d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0032: ldc.i8 0
		IL_003b: ldc.i8 0
		IL_0044: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0049: stloc.s 5

		IL_004b: nop
		IL_004c: ldloc.1
		IL_004d: ldc.i4.0
		IL_004e: ldloc.s 4
		IL_0050: stelem [mscorlib]System.Object
		IL_0055: ldstr "\nHigh Score: "
		IL_005a: ldnull
		IL_005b: ceq
		IL_005d: brtrue.s IL_0096

		IL_005f: nop
		IL_0060: ldstr "sub_1A4130"
		IL_0065: ldstr "\nHigh Score: "
		IL_006a: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_006f: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0074: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0079: ldc.i8 0
		IL_0082: ldc.i8 0
		IL_008b: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0090: stloc.s 6
		IL_0092: ldloc.s 6
		IL_0094: stloc.s 7

		IL_0096: nop
		IL_0097: ldloc.1
		IL_0098: ldc.i4.1
		IL_0099: ldstr "\nHigh Score: "
		IL_009e: stelem [mscorlib]System.Object
		IL_00a3: ldstr "HighBooks"
		IL_00a8: call int32 [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::GetInt(string)
		IL_00ad: stloc.s 8
		IL_00af: ldloc.s 8
		IL_00b1: box [mscorlib]System.Int32
		IL_00b6: stloc.s 9
		IL_00b8: ldloc.s 9
		IL_00ba: stloc.s 10
		IL_00bc: ldloc.s 9
		IL_00be: stloc.s 11
		IL_00c0: ldloc.s 9
		IL_00c2: ldnull
		IL_00c3: ceq
		IL_00c5: brtrue.s IL_00f7

		IL_00c7: nop
		IL_00c8: ldstr "sub_1A4130"
		IL_00cd: ldloc.s 11
		IL_00cf: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_00d4: ldstr "*(LocalExpr->klass + 0x40)"
		IL_00d9: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00de: ldc.i8 0
		IL_00e7: ldc.i8 0
		IL_00f0: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_00f5: stloc.s 12

		IL_00f7: nop
		IL_00f8: ldloc.1
		IL_00f9: ldc.i4.2
		IL_00fa: ldloc.s 10
		IL_00fc: stelem [mscorlib]System.Object
		IL_0101: ldstr " Notebooks"
		IL_0106: ldnull
		IL_0107: ceq
		IL_0109: brtrue.s IL_0142

		IL_010b: nop
		IL_010c: ldstr "sub_1A4130"
		IL_0111: ldstr " Notebooks"
		IL_0116: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_011b: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0120: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0125: ldc.i8 0
		IL_012e: ldc.i8 0
		IL_0137: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_013c: stloc.s 13
		IL_013e: ldloc.s 13
		IL_0140: stloc.s 14

		IL_0142: nop
		IL_0143: ldloc.1
		IL_0144: ldc.i4.3
		IL_0145: ldstr " Notebooks"
		IL_014a: stelem [mscorlib]System.Object
		IL_014f: ldloc.1
		IL_0150: call string [mscorlib]System.String::Concat(object[])
		IL_0155: stloc.s 15
		IL_0157: ret

		IL_0158: nop
		IL_0159: ldnull
		IL_015a: throw

		IL_015b: nop
		IL_015c: ldnull
		IL_015d: throw

		IL_015e: nop
		IL_015f: ldnull
		IL_0160: throw

		IL_0161: nop
		IL_0162: ldnull
		IL_0163: throw

		IL_0164: nop
		IL_0165: ldnull
		IL_0166: throw

		IL_0167: nop
		IL_0168: ldnull
		IL_0169: throw
	} // end of method EndlessTextScript::Start

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2444
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method EndlessTextScript::.ctor

} // end of class EndlessTextScript
