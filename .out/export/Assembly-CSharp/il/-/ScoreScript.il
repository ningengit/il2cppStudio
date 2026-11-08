.class public auto ansi beforefieldinit ScoreScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject scoreText
	.field public class [Unity.TextMeshPro]TMPro.TMP_Text text

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x13ee8
		// Header size: 12
		// Code size: 110 (0x6e)
		.maxstack 3
		.locals init (
			[0] string,
			[1] bool,
			[2] bool,
			[3] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[4] int32,
			[5] int32,
			[6] string,
			[7] string,
			[8] string
		)

		IL_0000: nop
		IL_0001: ldstr "CurrentMode"
		IL_0006: call string [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::GetString(string)
		IL_000b: stloc.0
		IL_000c: ldloc.0
		IL_000d: ldstr "endless"
		IL_0012: call bool [mscorlib]System.String::Equals(string, string)
		IL_0017: stloc.1
		IL_0018: ldloc.1
		IL_0019: stloc.2
		IL_001a: ldloc.1
		IL_001b: ldc.i4.0
		IL_001c: ceq
		IL_001e: brtrue.s IL_0069

		IL_0020: nop
		IL_0021: ldarg.0
		IL_0022: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject ScoreScript::scoreText
		IL_0027: ldc.i4.1
		IL_0028: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_002d: ldarg.0
		IL_002e: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text ScoreScript::text
		IL_0033: stloc.3
		IL_0034: ldstr "CurrentBooks"
		IL_0039: call int32 [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::GetInt(string)
		IL_003e: stloc.s 4
		IL_0040: ldloca.s 5
		IL_0042: ldloc.s 4
		IL_0044: stfld int32 [mscorlib]System.Int32::m_value
		IL_0049: ldloca.s 5
		IL_004b: call instance string [mscorlib]System.Int32::ToString()
		IL_0050: stloc.s 6
		IL_0052: ldstr "Score:\n"
		IL_0057: ldloc.s 6
		IL_0059: ldstr " Notebooks"
		IL_005e: call string [mscorlib]System.String::Concat(string, string, string)
		IL_0063: stloc.s 7
		IL_0065: ldloc.s 7
		IL_0067: stloc.s 8

		IL_0069: nop
		IL_006a: ret

		IL_006b: nop
		IL_006c: ldnull
		IL_006d: throw
	} // end of method ScoreScript::Start

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x270b
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method ScoreScript::.ctor

} // end of class ScoreScript
