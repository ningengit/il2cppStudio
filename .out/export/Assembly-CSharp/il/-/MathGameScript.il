.class public auto ansi beforefieldinit MathGameScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested private auto ansi sealed beforefieldinit '<CheatText>d__11'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerator`1<object>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private object '<>2__current'
		.field public class MathGameScript '<>4__this'
		.field public string text

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x2c51
			// Header size: 1
			// Code size: 16 (0x10)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ldarg.0
			IL_0009: ldarg.1
			IL_000a: stfld int32 MathGameScript/'<CheatText>d__11'::'<>1__state'
			IL_000f: ret
		} // end of method '<CheatText>d__11'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			// Method begins at RVA 0x2c62
			// Header size: 1
			// Code size: 2 (0x2)
			.maxstack 8

			IL_0000: nop
			IL_0001: ret
		} // end of method '<CheatText>d__11'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			// Method begins at RVA 0x1a5fc
			// Header size: 12
			// Code size: 55 (0x37)
			.maxstack 2
			.locals init (
				[0] class MathGameScript,
				[1] class [UnityEngine.CoreModule]UnityEngine.WaitForEndOfFrame
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld int32 MathGameScript/'<CheatText>d__11'::'<>1__state'
			IL_0007: ldc.i4.1
			IL_0008: cgt.un
			IL_000a: brtrue.s IL_0031

			IL_000c: nop
			IL_000d: ldarg.0
			IL_000e: ldfld class MathGameScript MathGameScript/'<CheatText>d__11'::'<>4__this'
			IL_0013: stloc.0
			IL_0014: ldarg.0
			IL_0015: ldc.i4.m1
			IL_0016: stfld int32 MathGameScript/'<CheatText>d__11'::'<>1__state'
			IL_001b: newobj instance void [mscorlib]Mono.Globalization.Unicode.ContractionComparer::.ctor()
			IL_0020: stloc.1
			IL_0021: ldarg.0
			IL_0022: ldloc.1
			IL_0023: stfld object MathGameScript/'<CheatText>d__11'::'<>2__current'
			IL_0028: ldarg.0
			IL_0029: ldc.i4.1
			IL_002a: stfld int32 MathGameScript/'<CheatText>d__11'::'<>1__state'
			IL_002f: ldc.i4.1
			IL_0030: ret

			IL_0031: nop
			IL_0032: ldc.i4.0
			IL_0033: ret

			IL_0034: nop
			IL_0035: ldnull
			IL_0036: throw
		} // end of method '<CheatText>d__11'::MoveNext

		.method private final hidebysig specialname newslot virtual 
			instance object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () cil managed 
		{
			// Method begins at RVA 0x2c65
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object MathGameScript/'<CheatText>d__11'::'<>2__current'
			IL_0007: ret
		} // end of method '<CheatText>d__11'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'

		.method private final hidebysig newslot virtual 
			instance void System.Collections.IEnumerator.Reset () cil managed 
		{
			// Method begins at RVA 0x1a640
			// Header size: 12
			// Code size: 67 (0x43)
			.maxstack 5
			.locals init (
				[0] int64
			)

			IL_0000: nop
			IL_0001: ldstr "sub_184BE0"
			IL_0006: ldstr "rax"
			IL_000b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0010: ldc.i8 0
			IL_0019: ldc.i8 0
			IL_0022: ldc.i8 0
			IL_002b: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
			IL_0030: stloc.0
			IL_0031: ldloc.0
			IL_0032: box [mscorlib]System.Int64
			IL_0037: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [mscorlib]System.NotSupportedException>(object)
			IL_003c: callvirt instance void [mscorlib]System.NotSupportedException::.ctor()
			IL_0041: ldnull
			IL_0042: throw
		} // end of method '<CheatText>d__11'::System.Collections.IEnumerator.Reset

		.method private final hidebysig specialname newslot virtual 
			instance object System.Collections.IEnumerator.get_Current () cil managed 
		{
			// Method begins at RVA 0x2c6e
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object MathGameScript/'<CheatText>d__11'::'<>2__current'
			IL_0007: ret
		} // end of method '<CheatText>d__11'::System.Collections.IEnumerator.get_Current

		// Properties
		.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current'()
		{
			.get instance object MathGameScript/'<CheatText>d__11'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'()
		}
		.property instance object System.Collections.IEnumerator.Current()
		{
			.get instance object MathGameScript/'<CheatText>d__11'::System.Collections.IEnumerator.get_Current()
		}

	} // end of class <CheatText>d__11


	// Fields
	.field public class GameControllerScript gc
	.field public class BaldiScript baldiScript
	.field public valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 playerPosition
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject mathGame
	.field public class [UnityEngine.UI]UnityEngine.UI.RawImage[] results
	.field public class [UnityEngine.CoreModule]UnityEngine.Texture correct
	.field public class [UnityEngine.CoreModule]UnityEngine.Texture incorrect
	.field public class [Unity.TextMeshPro]TMPro.TMP_InputField playerAnswer
	.field public class [Unity.TextMeshPro]TMPro.TMP_Text questionText
	.field public class [Unity.TextMeshPro]TMPro.TMP_Text questionText2
	.field public class [Unity.TextMeshPro]TMPro.TMP_Text questionText3
	.field public class [UnityEngine.AnimationModule]UnityEngine.Animator baldiFeed
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform baldiFeedTransform
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip bal_plus
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip bal_minus
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip bal_times
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip bal_divided
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip bal_equals
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip bal_howto
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip bal_intro
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip bal_screech
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip[] bal_numbers
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip[] bal_praises
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip[] bal_problems
	.field public class [UnityEngine.UI]UnityEngine.UI.Button firstButton
	.field private float32 endDelay
	.field private int32 problem
	.field private int32 audioInQueue
	.field private float32 num1
	.field private float32 num2
	.field private float32 num3
	.field private int32 sign
	.field private float32 solution
	.field private string[] hintText
	.field private string[] endlessHintText
	.field private bool questionInProgress
	.field private bool impossibleMode
	.field private int32 problemsWrong
	.field private class [UnityEngine.AudioModule]UnityEngine.AudioClip[] audioQueue
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioSource baldiAudio

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x6000
		// Header size: 12
		// Code size: 325 (0x145)
		.maxstack 3
		.locals init (
			[0] class GameControllerScript,
			[1] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[2] int64,
			[3] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[4] class [UnityEngine.AudioModule]UnityEngine.AudioClip,
			[5] int64,
			[6] int64,
			[7] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[8] class [UnityEngine.AudioModule]UnityEngine.AudioClip,
			[9] class GameControllerScript,
			[10] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class GameControllerScript MathGameScript::gc
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: ldc.i4.1
		IL_000a: stfld bool GameControllerScript::learningActive
		IL_000f: ldc.i4.0
		IL_0010: box [mscorlib]System.Int32
		IL_0015: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode>(object)
		IL_001a: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_lockState(valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode)
		IL_001f: ldc.i4.1
		IL_0020: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_visible(bool)
		IL_0025: ldloc.0
		IL_0026: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::reticle
		IL_002b: stloc.1
		IL_002c: ldloc.0
		IL_002d: ldc.i4.0
		IL_002e: stfld bool GameControllerScript::mouseLocked
		IL_0033: ldloc.1
		IL_0034: ldc.i4.0
		IL_0035: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_003a: ldloc.0
		IL_003b: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::tutorBaldi
		IL_0040: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Stop()
		IL_0045: ldloc.0
		IL_0046: ldfld bool GameControllerScript::spoopMode
		IL_004b: ldc.i4.0
		IL_004c: ceq
		IL_004e: ldc.i4.0
		IL_004f: ceq
		IL_0051: brtrue.s IL_006a

		IL_0053: nop
		IL_0054: ldloc.0
		IL_0055: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::schoolMusic
		IL_005a: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Stop()
		IL_005f: ldloc.0
		IL_0060: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::learnMusic
		IL_0065: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Play()

		IL_006a: nop
		IL_006b: ldarg.0
		IL_006c: ldfld class GameControllerScript MathGameScript::gc
		IL_0071: ldfld int32 GameControllerScript::notebooks
		IL_0076: ldc.i4.1
		IL_0077: ceq
		IL_0079: ldc.i4.0
		IL_007a: ceq
		IL_007c: brtrue.s IL_00f0

		IL_007e: nop
		IL_007f: ldarg.0
		IL_0080: ldfld int32 MathGameScript::audioInQueue
		IL_0085: conv.i8
		IL_0086: stloc.2
		IL_0087: ldarg.0
		IL_0088: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] MathGameScript::audioQueue
		IL_008d: stloc.3
		IL_008e: ldarg.0
		IL_008f: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip MathGameScript::bal_intro
		IL_0094: stloc.s 4
		IL_0096: ldloc.3
		IL_0097: ldloc.2
		IL_0098: conv.i4
		IL_0099: ldloc.s 4
		IL_009b: stelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_00a0: ldarg.0
		IL_00a1: ldfld int32 MathGameScript::audioInQueue
		IL_00a6: conv.i8
		IL_00a7: ldc.i8 1
		IL_00b0: add
		IL_00b1: stloc.s 5
		IL_00b3: ldarg.0
		IL_00b4: ldfld int32 MathGameScript::audioInQueue
		IL_00b9: conv.i8
		IL_00ba: stloc.s 6
		IL_00bc: ldarg.0
		IL_00bd: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] MathGameScript::audioQueue
		IL_00c2: stloc.s 7
		IL_00c4: ldarg.0
		IL_00c5: ldloc.s 5
		IL_00c7: conv.i4
		IL_00c8: stfld int32 MathGameScript::audioInQueue
		IL_00cd: ldloc.s 5
		IL_00cf: stloc.2
		IL_00d0: ldarg.0
		IL_00d1: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip MathGameScript::bal_howto
		IL_00d6: stloc.s 8
		IL_00d8: ldstr "mov [rcx+rax*8+28h],rdx ; value = LocalExpr"
		IL_00dd: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_00e2: ldarg.0
		IL_00e3: ldarg.0
		IL_00e4: ldfld int32 MathGameScript::audioInQueue
		IL_00e9: ldc.i4.1
		IL_00ea: add
		IL_00eb: stfld int32 MathGameScript::audioInQueue

		IL_00f0: nop
		IL_00f1: ldarg.0
		IL_00f2: callvirt instance void MathGameScript::NewProblem()
		IL_00f7: ldarg.0
		IL_00f8: ldfld class GameControllerScript MathGameScript::gc
		IL_00fd: stloc.s 9
		IL_00ff: ldloc.s 9
		IL_0101: ldfld bool GameControllerScript::spoopMode
		IL_0106: ldc.i4.0
		IL_0107: ceq
		IL_0109: brtrue.s IL_013d

		IL_010b: nop
		IL_010c: ldarg.0
		IL_010d: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform MathGameScript::baldiFeedTransform
		IL_0112: ldloca.s 10
		IL_0114: ldc.r4 -1000
		IL_0119: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_011e: ldloca.s 10
		IL_0120: ldc.r4 -1000
		IL_0125: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_012a: ldloca.s 10
		IL_012c: ldc.r4 0.0
		IL_0131: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0136: ldloc.s 10
		IL_0138: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)

		IL_013d: nop
		IL_013e: ret

		IL_013f: nop
		IL_0140: ldnull
		IL_0141: throw

		IL_0142: nop
		IL_0143: ldnull
		IL_0144: throw
	} // end of method MathGameScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x6154
		// Header size: 12
		// Code size: 601 (0x259)
		.maxstack 3
		.locals init (
			[0] bool,
			[1] class [UnityEngine.AnimationModule]UnityEngine.Animator,
			[2] int32,
			[3] class [UnityEngine.AudioModule]UnityEngine.AudioClip,
			[4] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[5] int32,
			[6] bool,
			[7] bool,
			[8] bool,
			[9] int32,
			[10] float32,
			[11] float32,
			[12] int32,
			[13] bool,
			[14] int32,
			[15] class BaldiScript,
			[16] float32,
			[17] float32,
			[18] float32,
			[19] float32,
			[20] class GameControllerScript,
			[21] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[22] class [UnityEngine.CoreModule]UnityEngine.GameObject
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource MathGameScript::baldiAudio
		IL_0007: callvirt instance bool [UnityEngine.AudioModule]UnityEngine.AudioSource::get_isPlaying()
		IL_000c: stloc.0
		IL_000d: ldloc.0
		IL_000e: ldc.i4.0
		IL_000f: ceq
		IL_0011: brtrue.s IL_0022

		IL_0013: nop
		IL_0014: ldarg.0
		IL_0015: ldfld class [UnityEngine.AnimationModule]UnityEngine.Animator MathGameScript::baldiFeed
		IL_001a: stloc.1
		IL_001b: ldc.i4.1
		IL_001c: stloc.2
		IL_001d: br IL_00bd

		IL_0022: nop
		IL_0023: ldarg.0
		IL_0024: ldfld int32 MathGameScript::audioInQueue
		IL_0029: ldc.i4.0
		IL_002a: cgt
		IL_002c: ldarg.0
		IL_002d: ldfld class GameControllerScript MathGameScript::gc
		IL_0032: ldfld bool GameControllerScript::spoopMode
		IL_0037: ldc.i4.0
		IL_0038: ceq
		IL_003a: and
		IL_003b: ldc.i4.0
		IL_003c: ceq
		IL_003e: brtrue.s IL_00b5

		IL_0040: nop
		IL_0041: ldarg.0
		IL_0042: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource MathGameScript::baldiAudio
		IL_0047: ldarg.0
		IL_0048: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] MathGameScript::audioQueue
		IL_004d: ldc.i4.0
		IL_004e: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0053: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_0058: ldarg.0
		IL_0059: ldfld int32 MathGameScript::audioInQueue
		IL_005e: ldc.i4.1
		IL_005f: cgt
		IL_0061: ldc.i4.0
		IL_0062: ceq
		IL_0064: brtrue.s IL_00a6

		IL_0066: nop
		// loop start (head: IL_0067)
			IL_0067: nop
			IL_0068: ldarg.0
			IL_0069: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] MathGameScript::audioQueue
			IL_006e: ldc.i4.1
			IL_006f: conv.i8
			IL_0070: conv.i4
			IL_0071: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
			IL_0076: stloc.3
			IL_0077: ldarg.0
			IL_0078: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] MathGameScript::audioQueue
			IL_007d: stloc.s 4
			IL_007f: ldloc.s 4
			IL_0081: ldc.i4.1
			IL_0082: conv.i8
			IL_0083: ldc.i8 1
			IL_008c: sub
			IL_008d: conv.i4
			IL_008e: ldloc.3
			IL_008f: stelem [UnityEngine.AudioModule]UnityEngine.AudioClip
			IL_0094: ldc.i4.1
			IL_0095: ldc.i4.1
			IL_0096: add
			IL_0097: stloc.s 5
			IL_0099: ldc.i4.1
			IL_009a: ldc.i4.1
			IL_009b: add
			IL_009c: ldarg.0
			IL_009d: ldfld int32 MathGameScript::audioInQueue
			IL_00a2: clt
			IL_00a4: brtrue.s IL_0067
		// end loop

		IL_00a6: nop
		IL_00a7: ldarg.0
		IL_00a8: ldarg.0
		IL_00a9: ldfld int32 MathGameScript::audioInQueue
		IL_00ae: ldc.i4.1
		IL_00af: sub
		IL_00b0: stfld int32 MathGameScript::audioInQueue

		IL_00b5: nop
		IL_00b6: ldarg.0
		IL_00b7: ldfld class [UnityEngine.AnimationModule]UnityEngine.Animator MathGameScript::baldiFeed
		IL_00bc: stloc.1

		IL_00bd: nop
		IL_00be: ldloc.1
		IL_00bf: ldstr "talking"
		IL_00c4: ldc.i4.0
		IL_00c5: callvirt instance void [UnityEngine.AnimationModule]UnityEngine.Animator::SetBool(string, bool)
		IL_00ca: ldstr "return"
		IL_00cf: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetKeyDown(string)
		IL_00d4: stloc.s 6
		IL_00d6: ldloc.s 6
		IL_00d8: stloc.s 7
		IL_00da: ldloc.s 6
		IL_00dc: ldc.i4.0
		IL_00dd: ceq
		IL_00df: ldc.i4.0
		IL_00e0: ceq
		IL_00e2: brtrue.s IL_00f9

		IL_00e4: nop
		IL_00e5: ldstr "enter"
		IL_00ea: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetKeyDown(string)
		IL_00ef: stloc.s 8
		IL_00f1: ldloc.s 8
		IL_00f3: stloc.s 9
		IL_00f5: ldloc.s 8
		IL_00f7: stloc.s 7

		IL_00f9: nop
		IL_00fa: ldarg.0
		IL_00fb: ldfld bool MathGameScript::questionInProgress
		IL_0100: ldloc.s 9
		IL_0102: and
		IL_0103: ldc.i4.0
		IL_0104: ceq
		IL_0106: brtrue.s IL_0116

		IL_0108: nop
		IL_0109: ldarg.0
		IL_010a: ldc.i4.0
		IL_010b: stfld bool MathGameScript::questionInProgress
		IL_0110: ldarg.0
		IL_0111: callvirt instance void MathGameScript::CheckAnswer()

		IL_0116: nop
		IL_0117: ldarg.0
		IL_0118: ldfld int32 MathGameScript::problem
		IL_011d: ldc.i4.3
		IL_011e: cgt
		IL_0120: ldc.i4.0
		IL_0121: ceq
		IL_0123: brtrue IL_0251

		IL_0128: nop
		IL_0129: ldarg.0
		IL_012a: ldfld float32 MathGameScript::endDelay
		IL_012f: stloc.s 10
		IL_0131: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_unscaledDeltaTime()
		IL_0136: stloc.s 11
		IL_0138: ldarg.0
		IL_0139: ldloc.s 10
		IL_013b: ldloc.s 11
		IL_013d: sub
		IL_013e: stfld float32 MathGameScript::endDelay
		IL_0143: ldc.r4 0.0
		IL_0148: ldloc.s 10
		IL_014a: ldloc.s 11
		IL_014c: sub
		IL_014d: clt
		IL_014f: brtrue IL_0251

		IL_0154: nop
		IL_0155: br.s IL_0158

		IL_0157: nop

		IL_0158: nop
		IL_0159: call void [mscorlib]System.GC::Collect()
		IL_015e: ldarg.0
		IL_015f: ldfld int32 MathGameScript::problemsWrong
		IL_0164: stloc.s 12
		IL_0166: ldarg.0
		IL_0167: ldfld class GameControllerScript MathGameScript::gc
		IL_016c: ldfld string GameControllerScript::mode
		IL_0171: ldstr "endless"
		IL_0176: call bool [mscorlib]System.String::Equals(string, string)
		IL_017b: stloc.s 13
		IL_017d: ldloc.s 12
		IL_017f: ldc.i4.0
		IL_0180: cgt
		IL_0182: ldc.i4.0
		IL_0183: ceq
		IL_0185: brfalse.s IL_018d

		IL_0187: ldloc.s 13
		IL_0189: stloc.s 14
		IL_018b: br.s IL_0191

		IL_018d: nop
		IL_018e: ldc.i4.0
		IL_018f: stloc.s 14

		IL_0191: nop
		IL_0192: ldloc.s 14
		IL_0194: ldc.i4.0
		IL_0195: ceq
		IL_0197: brtrue IL_0233

		IL_019c: nop
		IL_019d: ldarg.0
		IL_019e: ldfld class BaldiScript MathGameScript::baldiScript
		IL_01a3: stloc.s 15
		IL_01a5: ldloc.s 15
		IL_01a7: ldloc.s 15
		IL_01a9: ldfld float32 BaldiScript::baldiAnger
		IL_01ae: ldc.r4 1
		IL_01b3: sub
		IL_01b4: stfld float32 BaldiScript::baldiAnger
		IL_01b9: ldc.r4 0.5
		IL_01be: ldloc.s 15
		IL_01c0: ldfld float32 BaldiScript::baldiAnger
		IL_01c5: ldc.r4 1
		IL_01ca: sub
		IL_01cb: cgt
		IL_01cd: ldc.i4.0
		IL_01ce: ceq
		IL_01d0: brtrue.s IL_01df

		IL_01d2: nop
		IL_01d3: ldloc.s 15
		IL_01d5: ldc.r4 0.5
		IL_01da: stfld float32 BaldiScript::baldiAnger

		IL_01df: nop
		IL_01e0: ldc.r4 2
		IL_01e5: ldloc.s 15
		IL_01e7: ldfld float32 BaldiScript::baldiSpeedScale
		IL_01ec: div
		IL_01ed: ldloc.s 15
		IL_01ef: ldfld float32 BaldiScript::baldiAnger
		IL_01f4: add
		IL_01f5: stloc.s 16
		IL_01f7: ldloc.s 15
		IL_01f9: ldfld float32 BaldiScript::baldiAnger
		IL_01fe: ldc.r4 -3
		IL_0203: mul
		IL_0204: ldc.r4 2
		IL_0209: ldloc.s 15
		IL_020b: ldfld float32 BaldiScript::baldiSpeedScale
		IL_0210: div
		IL_0211: ldloc.s 15
		IL_0213: ldfld float32 BaldiScript::baldiAnger
		IL_0218: add
		IL_0219: div
		IL_021a: ldc.r4 3
		IL_021f: add
		IL_0220: stloc.s 17
		IL_0222: ldloc.s 15
		IL_0224: ldloc.s 17
		IL_0226: stfld float32 BaldiScript::baldiWait
		IL_022b: ldloc.s 16
		IL_022d: stloc.s 18
		IL_022f: ldloc.s 17
		IL_0231: stloc.s 19

		IL_0233: nop
		IL_0234: ldarg.0
		IL_0235: ldfld class GameControllerScript MathGameScript::gc
		IL_023a: stloc.s 20
		IL_023c: ldarg.0
		IL_023d: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_0242: stloc.s 21
		IL_0244: ldloc.s 21
		IL_0246: stloc.s 22
		IL_0248: ldloc.s 20
		IL_024a: ldloc.s 22
		IL_024c: callvirt instance void GameControllerScript::DeactivateLearningGame(class [UnityEngine.CoreModule]UnityEngine.GameObject)

		IL_0251: nop
		IL_0252: ret

		IL_0253: nop
		IL_0254: ldnull
		IL_0255: throw

		IL_0256: nop
		IL_0257: ldnull
		IL_0258: throw
	} // end of method MathGameScript::Update

	.method private hidebysig 
		instance void NewProblem () cil managed 
	{
		// Method begins at RVA 0x63bc
		// Header size: 12
		// Code size: 8532 (0x2154)
		.maxstack 5
		.locals init (
			[0] class [Unity.TextMeshPro]TMPro.TMP_InputField,
			[1] class GameControllerScript,
			[2] class GameControllerScript,
			[3] bool,
			[4] int32,
			[5] int32,
			[6] class GameControllerScript,
			[7] bool,
			[8] int32,
			[9] float32,
			[10] int32,
			[11] int32,
			[12] bool,
			[13] bool,
			[14] class BaldiScript,
			[15] class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent,
			[16] float32,
			[17] bool,
			[18] float32,
			[19] int32,
			[20] int32,
			[21] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[22] class [UnityEngine.AudioModule]UnityEngine.AudioClip,
			[23] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[24] int64,
			[25] class GameControllerScript,
			[26] bool,
			[27] int32,
			[28] int32,
			[29] class GameControllerScript,
			[30] bool,
			[31] int32,
			[32] int32,
			[33] float32,
			[34] float32,
			[35] float32,
			[36] int32,
			[37] int32,
			[38] class [UnityEngine.AudioModule]UnityEngine.AudioClip,
			[39] object[],
			[40] object[],
			[41] object[],
			[42] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[43] int64,
			[44] int64,
			[45] object,
			[46] object,
			[47] object,
			[48] int64,
			[49] int64,
			[50] int64,
			[51] float32,
			[52] object,
			[53] object,
			[54] int64,
			[55] int64,
			[56] int64,
			[57] float32,
			[58] object,
			[59] object,
			[60] int64,
			[61] int64,
			[62] int64,
			[63] float32,
			[64] object,
			[65] object,
			[66] int64,
			[67] int64,
			[68] int64,
			[69] string,
			[70] class [UnityEngine.AudioModule]UnityEngine.AudioClip,
			[71] object[],
			[72] object[],
			[73] int64,
			[74] int64,
			[75] object,
			[76] object,
			[77] int64,
			[78] int64,
			[79] int64,
			[80] float32,
			[81] object,
			[82] object,
			[83] int64,
			[84] int64,
			[85] int64,
			[86] float32,
			[87] object,
			[88] object,
			[89] int64,
			[90] int64,
			[91] int64,
			[92] float32,
			[93] object,
			[94] object,
			[95] int64,
			[96] int64,
			[97] int64,
			[98] string,
			[99] float32,
			[100] float32,
			[101] float32,
			[102] int32,
			[103] int32,
			[104] int32,
			[105] object[],
			[106] object[],
			[107] int64,
			[108] int64,
			[109] object,
			[110] object,
			[111] int64,
			[112] int64,
			[113] int64,
			[114] float32,
			[115] object,
			[116] object,
			[117] int64,
			[118] int64,
			[119] int64,
			[120] float32,
			[121] object,
			[122] object,
			[123] int64,
			[124] int64,
			[125] int64,
			[126] float32,
			[127] object,
			[128] object,
			[129] int64,
			[130] int64,
			[131] int64,
			[132] string,
			[133] string,
			[134] object[],
			[135] object[],
			[136] int64,
			[137] int64,
			[138] object,
			[139] object,
			[140] int64,
			[141] int64,
			[142] int64,
			[143] float32,
			[144] object,
			[145] object,
			[146] int64,
			[147] int64,
			[148] int64,
			[149] float32,
			[150] object,
			[151] object,
			[152] int64,
			[153] int64,
			[154] int64,
			[155] float32,
			[156] object,
			[157] object,
			[158] int64,
			[159] int64,
			[160] int64,
			[161] string,
			[162] string,
			[163] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[164] string,
			[165] float32,
			[166] float32,
			[167] float32,
			[168] int32,
			[169] int32,
			[170] int32,
			[171] object[],
			[172] object[],
			[173] int64,
			[174] int64,
			[175] object,
			[176] object,
			[177] int64,
			[178] int64,
			[179] int64,
			[180] float32,
			[181] object,
			[182] object,
			[183] int64,
			[184] int64,
			[185] int64,
			[186] float32,
			[187] object,
			[188] object,
			[189] int64,
			[190] int64,
			[191] int64,
			[192] float32,
			[193] object,
			[194] object,
			[195] int64,
			[196] int64,
			[197] int64,
			[198] string,
			[199] string,
			[200] object[],
			[201] object[],
			[202] int64,
			[203] int64,
			[204] object,
			[205] object,
			[206] int64,
			[207] int64,
			[208] int64,
			[209] float32,
			[210] object,
			[211] object,
			[212] int64,
			[213] int64,
			[214] int64,
			[215] float32,
			[216] object,
			[217] object,
			[218] int64,
			[219] int64,
			[220] int64,
			[221] float32,
			[222] object,
			[223] object,
			[224] int64,
			[225] int64,
			[226] int64,
			[227] string,
			[228] string,
			[229] float32,
			[230] int32,
			[231] float32,
			[232] int32,
			[233] float32,
			[234] int32,
			[235] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[236] float32,
			[237] int32,
			[238] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[239] int64,
			[240] class [UnityEngine.AudioModule]UnityEngine.AudioClip,
			[241] float32,
			[242] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[243] object[],
			[244] object[],
			[245] int64,
			[246] int64,
			[247] object,
			[248] object,
			[249] int64,
			[250] int64,
			[251] int64,
			[252] float32,
			[253] object,
			[254] object,
			[255] int64,
			[256] int64,
			[257] int64,
			[258] float32,
			[259] object,
			[260] object,
			[261] int64,
			[262] int64,
			[263] int64,
			[264] string,
			[265] float32,
			[266] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[267] object[],
			[268] object[],
			[269] int64,
			[270] int64,
			[271] object,
			[272] object,
			[273] int64,
			[274] int64,
			[275] int64,
			[276] float32,
			[277] object,
			[278] object,
			[279] int64,
			[280] int64,
			[281] int64,
			[282] float32,
			[283] object,
			[284] object,
			[285] int64,
			[286] int64,
			[287] int64,
			[288] string,
			[289] class [UnityEngine.AudioModule]UnityEngine.AudioClip,
			[290] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[291] int32,
			[292] int32,
			[293] int64,
			[294] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[295] class [UnityEngine.AudioModule]UnityEngine.AudioClip,
			[296] int64,
			[297] int64,
			[298] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[299] int64,
			[300] class [UnityEngine.AudioModule]UnityEngine.AudioClip
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [Unity.TextMeshPro]TMPro.TMP_InputField MathGameScript::playerAnswer
		IL_0007: ldstr "*(/*typedef#-1*/object.static_fields + 0x0)"
		IL_000c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0011: box [mscorlib]System.Int64
		IL_0016: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<string>(object)
		IL_001b: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_InputField::set_text(string)
		IL_0020: ldarg.0
		IL_0021: ldfld class [Unity.TextMeshPro]TMPro.TMP_InputField MathGameScript::playerAnswer
		IL_0026: stloc.0
		IL_0027: ldarg.0
		IL_0028: ldarg.0
		IL_0029: ldfld int32 MathGameScript::problem
		IL_002e: ldc.i4.1
		IL_002f: add
		IL_0030: stfld int32 MathGameScript::problem
		IL_0035: ldloc.0
		IL_0036: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_InputField::ActivateInputField()
		IL_003b: ldarg.0
		IL_003c: ldfld int32 MathGameScript::problem
		IL_0041: ldc.i4.3
		IL_0042: cgt
		IL_0044: ldc.i4.0
		IL_0045: ceq
		IL_0047: brtrue IL_01e3

		IL_004c: nop
		IL_004d: ldarg.0
		IL_004e: ldfld class GameControllerScript MathGameScript::gc
		IL_0053: stloc.1
		IL_0054: ldarg.0
		IL_0055: ldc.r4 5
		IL_005a: stfld float32 MathGameScript::endDelay
		IL_005f: ldloc.1
		IL_0060: stloc.2
		IL_0061: ldloc.2
		IL_0062: ldfld bool GameControllerScript::spoopMode
		IL_0067: ldc.i4.0
		IL_0068: ceq
		IL_006a: brtrue IL_01dd

		IL_006f: nop
		IL_0070: ldloc.2
		IL_0071: ldfld string GameControllerScript::mode
		IL_0076: ldstr "endless"
		IL_007b: call bool [mscorlib]System.String::Equals(string, string)
		IL_0080: stloc.3
		IL_0081: ldarg.0
		IL_0082: ldfld int32 MathGameScript::problemsWrong
		IL_0087: ldc.i4.0
		IL_0088: cgt
		IL_008a: ldc.i4.0
		IL_008b: ceq
		IL_008d: brfalse.s IL_0094

		IL_008f: ldloc.3
		IL_0090: stloc.s 4
		IL_0092: br.s IL_0098

		IL_0094: nop
		IL_0095: ldc.i4.0
		IL_0096: stloc.s 4

		IL_0098: nop
		IL_0099: ldloc.3
		IL_009a: stloc.s 5
		IL_009c: ldloc.s 4
		IL_009e: ldc.i4.0
		IL_009f: ceq
		IL_00a1: ldc.i4.0
		IL_00a2: ceq
		IL_00a4: brtrue IL_01b9

		IL_00a9: nop
		IL_00aa: ldarg.0
		IL_00ab: ldfld class GameControllerScript MathGameScript::gc
		IL_00b0: stloc.s 6
		IL_00b2: ldloc.s 6
		IL_00b4: ldfld string GameControllerScript::mode
		IL_00b9: ldstr "story"
		IL_00be: call bool [mscorlib]System.String::Equals(string, string)
		IL_00c3: stloc.s 7
		IL_00c5: ldarg.0
		IL_00c6: ldfld int32 MathGameScript::problemsWrong
		IL_00cb: ldc.i4.3
		IL_00cc: clt
		IL_00ce: ldc.i4.0
		IL_00cf: ceq
		IL_00d1: brfalse.s IL_00d9

		IL_00d3: ldloc.s 7
		IL_00d5: stloc.s 8
		IL_00d7: br.s IL_00dd

		IL_00d9: nop
		IL_00da: ldc.i4.0
		IL_00db: stloc.s 8

		IL_00dd: nop
		IL_00de: ldloc.s 8
		IL_00e0: ldc.i4.0
		IL_00e1: ceq
		IL_00e3: ldc.i4.0
		IL_00e4: ceq
		IL_00e6: brtrue.s IL_010a

		IL_00e8: nop
		IL_00e9: ldc.r4 0.0
		IL_00ee: ldc.r4 1
		IL_00f3: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_00f8: stloc.s 9
		IL_00fa: ldloc.s 9
		IL_00fc: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_0101: stloc.s 10
		IL_0103: ldloc.s 10
		IL_0105: stloc.s 11

		IL_0107: nop

		IL_0108: nop
		IL_0109: ret

		IL_010a: nop
		IL_010b: ldarg.0
		IL_010c: ldfld class BaldiScript MathGameScript::baldiScript
		IL_0111: callvirt instance bool [UnityEngine.CoreModule]UnityEngine.Behaviour::get_isActiveAndEnabled()
		IL_0116: stloc.s 12
		IL_0118: ldloc.s 12
		IL_011a: stloc.s 13
		IL_011c: ldloc.s 12
		IL_011e: ldc.i4.0
		IL_011f: ceq
		IL_0121: brtrue.s IL_019b

		IL_0123: nop
		IL_0124: ldarg.0
		IL_0125: ldfld class BaldiScript MathGameScript::baldiScript
		IL_012a: stloc.s 14
		IL_012c: ldloc.s 14
		IL_012e: ldfld bool BaldiScript::antiHearing
		IL_0133: ldc.i4.0
		IL_0134: ceq
		IL_0136: ldc.i4.0
		IL_0137: ceq
		IL_0139: brtrue.s IL_019b

		IL_013b: nop
		IL_013c: ldc.r4 7
		IL_0141: ldloc.s 14
		IL_0143: ldfld float32 BaldiScript::currentPriority
		IL_0148: clt
		IL_014a: brtrue.s IL_019b

		IL_014c: nop
		IL_014d: ldloc.s 14
		IL_014f: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent BaldiScript::agent
		IL_0154: stloc.s 15
		IL_0156: ldarg.0
		IL_0157: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 MathGameScript::playerPosition
		IL_015c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0161: stloc.s 16
		IL_0163: ldloc.s 15
		IL_0165: ldstr "&stack[0xFFFFFFFFFFFFFFF0]"
		IL_016a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_016f: conv.i8
		IL_0170: ldc.i8 -72
		IL_0179: add
		IL_017a: box [mscorlib]System.Int64
		IL_017f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0184: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::SetDestination(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0189: stloc.s 17
		IL_018b: ldloc.s 14
		IL_018d: ldc.r4 7
		IL_0192: stfld float32 BaldiScript::currentPriority
		IL_0197: ldloc.s 17
		IL_0199: stloc.s 13

		IL_019b: nop
		IL_019c: ldarg.0
		IL_019d: ldfld class GameControllerScript MathGameScript::gc
		IL_01a2: ldarg.0
		IL_01a3: ldfld class GameControllerScript MathGameScript::gc
		IL_01a8: ldfld int32 GameControllerScript::failedNotebooks
		IL_01ad: ldc.i4.1
		IL_01ae: add
		IL_01af: stfld int32 GameControllerScript::failedNotebooks
		IL_01b4: br IL_0108

		IL_01b9: nop
		IL_01ba: ldc.r4 0.0
		IL_01bf: ldc.r4 1
		IL_01c4: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_01c9: stloc.s 18
		IL_01cb: ldloc.s 18
		IL_01cd: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_01d2: stloc.s 19
		IL_01d4: ldloc.s 19
		IL_01d6: stloc.s 20
		IL_01d8: br IL_0107

		IL_01dd: nop
		IL_01de: br IL_0108

		IL_01e3: nop
		IL_01e4: ldarg.0
		IL_01e5: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] MathGameScript::bal_problems
		IL_01ea: stloc.s 21
		IL_01ec: ldloc.s 21
		IL_01ee: ldarg.0
		IL_01ef: ldfld int32 MathGameScript::problem
		IL_01f4: conv.i8
		IL_01f5: ldc.i8 1
		IL_01fe: sub
		IL_01ff: conv.i4
		IL_0200: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0205: stloc.s 22
		IL_0207: ldarg.0
		IL_0208: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] MathGameScript::audioQueue
		IL_020d: stloc.s 23
		IL_020f: ldarg.0
		IL_0210: ldfld int32 MathGameScript::audioInQueue
		IL_0215: conv.i8
		IL_0216: stloc.s 24
		IL_0218: ldloc.s 23
		IL_021a: ldloc.s 24
		IL_021c: conv.i4
		IL_021d: ldloc.s 22
		IL_021f: stelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0224: ldarg.0
		IL_0225: ldfld class GameControllerScript MathGameScript::gc
		IL_022a: stloc.s 25
		IL_022c: ldarg.0
		IL_022d: ldarg.0
		IL_022e: ldfld int32 MathGameScript::audioInQueue
		IL_0233: ldc.i4.1
		IL_0234: add
		IL_0235: stfld int32 MathGameScript::audioInQueue
		IL_023a: ldloc.s 25
		IL_023c: ldfld string GameControllerScript::mode
		IL_0241: ldstr "story"
		IL_0246: call bool [mscorlib]System.String::Equals(string, string)
		IL_024b: stloc.s 26
		IL_024d: ldloc.s 26
		IL_024f: stloc.s 27
		IL_0251: ldarg.0
		IL_0252: ldfld int32 MathGameScript::problem
		IL_0257: ldc.i4.2
		IL_0258: cgt
		IL_025a: brtrue.s IL_025f

		IL_025c: nop
		IL_025d: br.s IL_0273

		IL_025f: nop
		IL_0260: ldarg.0
		IL_0261: ldfld class GameControllerScript MathGameScript::gc
		IL_0266: ldfld int32 GameControllerScript::notebooks
		IL_026b: ldc.i4.1
		IL_026c: cgt
		IL_026e: ldc.i4.0
		IL_026f: ceq
		IL_0271: stloc.s 28

		IL_0273: nop
		IL_0274: ldloc.s 28
		IL_0276: ldloc.s 27
		IL_0278: and
		IL_0279: ldc.i4.0
		IL_027a: ceq
		IL_027c: ldc.i4.0
		IL_027d: ceq
		IL_027f: brtrue IL_18f8

		IL_0284: nop
		IL_0285: ldarg.0
		IL_0286: ldfld class GameControllerScript MathGameScript::gc
		IL_028b: stloc.s 29
		IL_028d: ldloc.s 29
		IL_028f: ldfld string GameControllerScript::mode
		IL_0294: ldstr "endless"
		IL_0299: call bool [mscorlib]System.String::Equals(string, string)
		IL_029e: stloc.s 30
		IL_02a0: ldloc.s 30
		IL_02a2: stloc.s 31
		IL_02a4: ldarg.0
		IL_02a5: ldfld int32 MathGameScript::problem
		IL_02aa: ldc.i4.2
		IL_02ab: cgt
		IL_02ad: brtrue.s IL_02b2

		IL_02af: nop
		IL_02b0: br.s IL_02c6

		IL_02b2: nop
		IL_02b3: ldarg.0
		IL_02b4: ldfld class GameControllerScript MathGameScript::gc
		IL_02b9: ldfld int32 GameControllerScript::notebooks
		IL_02be: ldc.i4.2
		IL_02bf: ceq
		IL_02c1: ldc.i4.0
		IL_02c2: ceq
		IL_02c4: stloc.s 32

		IL_02c6: nop
		IL_02c7: ldloc.s 32
		IL_02c9: ldloc.s 31
		IL_02cb: and
		IL_02cc: ldc.i4.0
		IL_02cd: ceq
		IL_02cf: ldc.i4.0
		IL_02d0: ceq
		IL_02d2: brtrue IL_18f8

		IL_02d7: nop
		IL_02d8: ldarg.0
		IL_02d9: ldc.i4.1
		IL_02da: stfld bool MathGameScript::impossibleMode
		IL_02df: ldc.r4 1
		IL_02e4: ldc.r4 9999
		IL_02e9: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_02ee: stloc.s 33
		IL_02f0: ldarg.0
		IL_02f1: ldloc.s 33
		IL_02f3: stfld float32 MathGameScript::num1
		IL_02f8: ldc.r4 1
		IL_02fd: ldc.r4 9999
		IL_0302: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_0307: stloc.s 34
		IL_0309: ldarg.0
		IL_030a: ldloc.s 34
		IL_030c: stfld float32 MathGameScript::num2
		IL_0311: ldc.r4 1
		IL_0316: ldc.r4 9999
		IL_031b: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_0320: stloc.s 35
		IL_0322: ldarg.0
		IL_0323: ldloc.s 35
		IL_0325: stfld float32 MathGameScript::num3
		IL_032a: ldc.i4.0
		IL_032b: ldc.i4.1
		IL_032c: call int32 [UnityEngine.CoreModule]UnityEngine.Random::Range(int32, int32)
		IL_0331: stloc.s 36
		IL_0333: ldloc.s 36
		IL_0335: conv.r4
		IL_0336: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_033b: stloc.s 37
		IL_033d: ldarg.0
		IL_033e: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip MathGameScript::bal_screech
		IL_0343: stloc.s 38
		IL_0345: ldarg.0
		IL_0346: ldloc.s 37
		IL_0348: stfld int32 MathGameScript::sign
		IL_034d: ldarg.0
		IL_034e: ldloc.s 38
		IL_0350: callvirt instance void MathGameScript::QueueAudio(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_0355: ldarg.0
		IL_0356: ldfld int32 MathGameScript::sign
		IL_035b: ldc.i4.0
		IL_035c: ceq
		IL_035e: brtrue IL_06eb

		IL_0363: nop
		IL_0364: ldarg.0
		IL_0365: ldfld int32 MathGameScript::sign
		IL_036a: ldc.i4.1
		IL_036b: ceq
		IL_036d: ldc.i4.0
		IL_036e: ceq
		IL_0370: brtrue IL_0a71

		IL_0375: nop
		IL_0376: ldc.i4.s 9
		IL_0378: newarr [mscorlib]System.Object
		IL_037d: stloc.s 39
		IL_037f: ldloc.s 39
		IL_0381: stloc.s 40
		IL_0383: ldloc.s 39
		IL_0385: stloc.s 41
		IL_0387: ldarg.0
		IL_0388: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text MathGameScript::questionText
		IL_038d: stloc.s 42
		IL_038f: ldstr "SOLVE MATH Q"
		IL_0394: ldnull
		IL_0395: ceq
		IL_0397: brtrue.s IL_03d0

		IL_0399: nop
		IL_039a: ldstr "sub_1A4130"
		IL_039f: ldstr "SOLVE MATH Q"
		IL_03a4: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_03a9: ldstr "*(LocalExpr->klass + 0x40)"
		IL_03ae: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_03b3: ldc.i8 0
		IL_03bc: ldc.i8 0
		IL_03c5: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_03ca: stloc.s 43
		IL_03cc: ldloc.s 43
		IL_03ce: stloc.s 44

		IL_03d0: nop
		IL_03d1: ldloc.s 40
		IL_03d3: ldc.i4.0
		IL_03d4: ldstr "SOLVE MATH Q"
		IL_03d9: stelem [mscorlib]System.Object
		IL_03de: ldstr "*(BinaryExpr)"
		IL_03e3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_03e8: conv.i4
		IL_03e9: box [mscorlib]System.Int32
		IL_03ee: stloc.s 45
		IL_03f0: ldloc.s 45
		IL_03f2: stloc.s 46
		IL_03f4: ldloc.s 45
		IL_03f6: stloc.s 47
		IL_03f8: ldloc.s 45
		IL_03fa: ldnull
		IL_03fb: ceq
		IL_03fd: brtrue.s IL_042f

		IL_03ff: nop
		IL_0400: ldstr "sub_1A4130"
		IL_0405: ldloc.s 47
		IL_0407: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_040c: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0411: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0416: ldc.i8 0
		IL_041f: ldc.i8 0
		IL_0428: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_042d: stloc.s 48

		IL_042f: nop
		IL_0430: ldloc.s 40
		IL_0432: ldc.i4.1
		IL_0433: ldloc.s 46
		IL_0435: stelem [mscorlib]System.Object
		IL_043a: ldstr ": \n ("
		IL_043f: ldnull
		IL_0440: ceq
		IL_0442: brtrue.s IL_047b

		IL_0444: nop
		IL_0445: ldstr "sub_1A4130"
		IL_044a: ldstr ": \n ("
		IL_044f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0454: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0459: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_045e: ldc.i8 0
		IL_0467: ldc.i8 0
		IL_0470: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0475: stloc.s 49
		IL_0477: ldloc.s 49
		IL_0479: stloc.s 50

		IL_047b: nop
		IL_047c: ldloc.s 40
		IL_047e: ldc.i4.2
		IL_047f: ldstr ": \n ("
		IL_0484: stelem [mscorlib]System.Object
		IL_0489: ldarg.0
		IL_048a: ldfld float32 MathGameScript::num1
		IL_048f: stloc.s 51
		IL_0491: ldstr "*(BinaryExpr)"
		IL_0496: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_049b: box [mscorlib]System.Int64
		IL_04a0: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_04a5: box [mscorlib]System.Single
		IL_04aa: stloc.s 52
		IL_04ac: ldloc.s 52
		IL_04ae: stloc.s 46
		IL_04b0: ldloc.s 52
		IL_04b2: stloc.s 53
		IL_04b4: ldloc.s 52
		IL_04b6: ldnull
		IL_04b7: ceq
		IL_04b9: brtrue.s IL_04eb

		IL_04bb: nop
		IL_04bc: ldstr "sub_1A4130"
		IL_04c1: ldloc.s 53
		IL_04c3: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_04c8: ldstr "*(LocalExpr->klass + 0x40)"
		IL_04cd: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_04d2: ldc.i8 0
		IL_04db: ldc.i8 0
		IL_04e4: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_04e9: stloc.s 54

		IL_04eb: nop
		IL_04ec: ldloc.s 40
		IL_04ee: ldc.i4.3
		IL_04ef: ldloc.s 46
		IL_04f1: stelem [mscorlib]System.Object
		IL_04f6: ldstr "/"
		IL_04fb: ldnull
		IL_04fc: ceq
		IL_04fe: brtrue.s IL_0537

		IL_0500: nop
		IL_0501: ldstr "sub_1A4130"
		IL_0506: ldstr "/"
		IL_050b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0510: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0515: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_051a: ldc.i8 0
		IL_0523: ldc.i8 0
		IL_052c: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0531: stloc.s 55
		IL_0533: ldloc.s 55
		IL_0535: stloc.s 56

		IL_0537: nop
		IL_0538: ldloc.s 40
		IL_053a: ldc.i4.4
		IL_053b: ldstr "/"
		IL_0540: stelem [mscorlib]System.Object
		IL_0545: ldarg.0
		IL_0546: ldfld float32 MathGameScript::num2
		IL_054b: stloc.s 57
		IL_054d: ldstr "*(BinaryExpr)"
		IL_0552: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0557: box [mscorlib]System.Int64
		IL_055c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0561: box [mscorlib]System.Single
		IL_0566: stloc.s 58
		IL_0568: ldloc.s 58
		IL_056a: stloc.s 46
		IL_056c: ldloc.s 58
		IL_056e: stloc.s 59
		IL_0570: ldloc.s 58
		IL_0572: ldnull
		IL_0573: ceq
		IL_0575: brtrue.s IL_05a7

		IL_0577: nop
		IL_0578: ldstr "sub_1A4130"
		IL_057d: ldloc.s 59
		IL_057f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0584: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0589: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_058e: ldc.i8 0
		IL_0597: ldc.i8 0
		IL_05a0: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_05a5: stloc.s 60

		IL_05a7: nop
		IL_05a8: ldloc.s 40
		IL_05aa: ldc.i4.5
		IL_05ab: ldloc.s 46
		IL_05ad: stelem [mscorlib]System.Object
		IL_05b2: ldstr ")+"
		IL_05b7: ldnull
		IL_05b8: ceq
		IL_05ba: brtrue.s IL_05f3

		IL_05bc: nop
		IL_05bd: ldstr "sub_1A4130"
		IL_05c2: ldstr ")+"
		IL_05c7: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_05cc: ldstr "*(LocalExpr->klass + 0x40)"
		IL_05d1: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_05d6: ldc.i8 0
		IL_05df: ldc.i8 0
		IL_05e8: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_05ed: stloc.s 61
		IL_05ef: ldloc.s 61
		IL_05f1: stloc.s 62

		IL_05f3: nop
		IL_05f4: ldloc.s 40
		IL_05f6: ldc.i4.6
		IL_05f7: ldstr ")+"
		IL_05fc: stelem [mscorlib]System.Object
		IL_0601: ldarg.0
		IL_0602: ldfld float32 MathGameScript::num3
		IL_0607: stloc.s 63
		IL_0609: ldstr "*(BinaryExpr)"
		IL_060e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0613: box [mscorlib]System.Int64
		IL_0618: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_061d: box [mscorlib]System.Single
		IL_0622: stloc.s 64
		IL_0624: ldloc.s 64
		IL_0626: stloc.s 46
		IL_0628: ldloc.s 64
		IL_062a: stloc.s 65
		IL_062c: ldloc.s 64
		IL_062e: ldnull
		IL_062f: ceq
		IL_0631: brtrue.s IL_0663

		IL_0633: nop
		IL_0634: ldstr "sub_1A4130"
		IL_0639: ldloc.s 65
		IL_063b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0640: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0645: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_064a: ldc.i8 0
		IL_0653: ldc.i8 0
		IL_065c: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0661: stloc.s 66

		IL_0663: nop
		IL_0664: ldloc.s 40
		IL_0666: ldc.i4.7
		IL_0667: ldloc.s 46
		IL_0669: stelem [mscorlib]System.Object
		IL_066e: ldstr "="
		IL_0673: ldnull
		IL_0674: ceq
		IL_0676: brtrue.s IL_06af

		IL_0678: nop
		IL_0679: ldstr "sub_1A4130"
		IL_067e: ldstr "="
		IL_0683: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0688: ldstr "*(LocalExpr->klass + 0x40)"
		IL_068d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0692: ldc.i8 0
		IL_069b: ldc.i8 0
		IL_06a4: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_06a9: stloc.s 67
		IL_06ab: ldloc.s 67
		IL_06ad: stloc.s 68

		IL_06af: nop
		IL_06b0: ldloc.s 40
		IL_06b2: ldc.i4.8
		IL_06b3: ldstr "="
		IL_06b8: stelem [mscorlib]System.Object
		IL_06bd: ldloc.s 40
		IL_06bf: call string [mscorlib]System.String::Concat(object[])
		IL_06c4: stloc.s 69
		IL_06c6: ldarg.0
		IL_06c7: ldarg.0
		IL_06c8: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip MathGameScript::bal_divided
		IL_06cd: callvirt instance void MathGameScript::QueueAudio(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_06d2: ldarg.0
		IL_06d3: ldarg.0
		IL_06d4: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip MathGameScript::bal_screech
		IL_06d9: callvirt instance void MathGameScript::QueueAudio(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_06de: ldarg.0
		IL_06df: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip MathGameScript::bal_plus
		IL_06e4: stloc.s 70
		IL_06e6: br IL_0a5c

		IL_06eb: nop
		IL_06ec: ldc.i4.s 9
		IL_06ee: newarr [mscorlib]System.Object
		IL_06f3: stloc.s 71
		IL_06f5: ldloc.s 71
		IL_06f7: stloc.s 40
		IL_06f9: ldloc.s 71
		IL_06fb: stloc.s 72
		IL_06fd: ldarg.0
		IL_06fe: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text MathGameScript::questionText
		IL_0703: stloc.s 42
		IL_0705: ldstr "SOLVE MATH Q"
		IL_070a: ldnull
		IL_070b: ceq
		IL_070d: brtrue.s IL_0746

		IL_070f: nop
		IL_0710: ldstr "sub_1A4130"
		IL_0715: ldstr "SOLVE MATH Q"
		IL_071a: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_071f: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0724: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0729: ldc.i8 0
		IL_0732: ldc.i8 0
		IL_073b: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0740: stloc.s 73
		IL_0742: ldloc.s 73
		IL_0744: stloc.s 74

		IL_0746: nop
		IL_0747: ldloc.s 40
		IL_0749: ldc.i4.0
		IL_074a: ldstr "SOLVE MATH Q"
		IL_074f: stelem [mscorlib]System.Object
		IL_0754: ldstr "*(BinaryExpr)"
		IL_0759: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_075e: conv.i4
		IL_075f: box [mscorlib]System.Int32
		IL_0764: stloc.s 75
		IL_0766: ldloc.s 75
		IL_0768: stloc.s 46
		IL_076a: ldloc.s 75
		IL_076c: stloc.s 76
		IL_076e: ldloc.s 75
		IL_0770: ldnull
		IL_0771: ceq
		IL_0773: brtrue.s IL_07a5

		IL_0775: nop
		IL_0776: ldstr "sub_1A4130"
		IL_077b: ldloc.s 76
		IL_077d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0782: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0787: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_078c: ldc.i8 0
		IL_0795: ldc.i8 0
		IL_079e: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_07a3: stloc.s 77

		IL_07a5: nop
		IL_07a6: ldloc.s 40
		IL_07a8: ldc.i4.1
		IL_07a9: ldloc.s 46
		IL_07ab: stelem [mscorlib]System.Object
		IL_07b0: ldstr ": \n"
		IL_07b5: ldnull
		IL_07b6: ceq
		IL_07b8: brtrue.s IL_07f1

		IL_07ba: nop
		IL_07bb: ldstr "sub_1A4130"
		IL_07c0: ldstr ": \n"
		IL_07c5: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_07ca: ldstr "*(LocalExpr->klass + 0x40)"
		IL_07cf: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_07d4: ldc.i8 0
		IL_07dd: ldc.i8 0
		IL_07e6: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_07eb: stloc.s 78
		IL_07ed: ldloc.s 78
		IL_07ef: stloc.s 79

		IL_07f1: nop
		IL_07f2: ldloc.s 40
		IL_07f4: ldc.i4.2
		IL_07f5: ldstr ": \n"
		IL_07fa: stelem [mscorlib]System.Object
		IL_07ff: ldarg.0
		IL_0800: ldfld float32 MathGameScript::num1
		IL_0805: stloc.s 80
		IL_0807: ldstr "*(BinaryExpr)"
		IL_080c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0811: box [mscorlib]System.Int64
		IL_0816: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_081b: box [mscorlib]System.Single
		IL_0820: stloc.s 81
		IL_0822: ldloc.s 81
		IL_0824: stloc.s 46
		IL_0826: ldloc.s 81
		IL_0828: stloc.s 82
		IL_082a: ldloc.s 81
		IL_082c: ldnull
		IL_082d: ceq
		IL_082f: brtrue.s IL_0861

		IL_0831: nop
		IL_0832: ldstr "sub_1A4130"
		IL_0837: ldloc.s 82
		IL_0839: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_083e: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0843: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0848: ldc.i8 0
		IL_0851: ldc.i8 0
		IL_085a: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_085f: stloc.s 83

		IL_0861: nop
		IL_0862: ldloc.s 40
		IL_0864: ldc.i4.3
		IL_0865: ldloc.s 46
		IL_0867: stelem [mscorlib]System.Object
		IL_086c: ldstr "+("
		IL_0871: ldnull
		IL_0872: ceq
		IL_0874: brtrue.s IL_08ad

		IL_0876: nop
		IL_0877: ldstr "sub_1A4130"
		IL_087c: ldstr "+("
		IL_0881: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0886: ldstr "*(LocalExpr->klass + 0x40)"
		IL_088b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0890: ldc.i8 0
		IL_0899: ldc.i8 0
		IL_08a2: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_08a7: stloc.s 84
		IL_08a9: ldloc.s 84
		IL_08ab: stloc.s 85

		IL_08ad: nop
		IL_08ae: ldloc.s 40
		IL_08b0: ldc.i4.4
		IL_08b1: ldstr "+("
		IL_08b6: stelem [mscorlib]System.Object
		IL_08bb: ldarg.0
		IL_08bc: ldfld float32 MathGameScript::num2
		IL_08c1: stloc.s 86
		IL_08c3: ldstr "*(BinaryExpr)"
		IL_08c8: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_08cd: box [mscorlib]System.Int64
		IL_08d2: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_08d7: box [mscorlib]System.Single
		IL_08dc: stloc.s 87
		IL_08de: ldloc.s 87
		IL_08e0: stloc.s 46
		IL_08e2: ldloc.s 87
		IL_08e4: stloc.s 88
		IL_08e6: ldloc.s 87
		IL_08e8: ldnull
		IL_08e9: ceq
		IL_08eb: brtrue.s IL_091d

		IL_08ed: nop
		IL_08ee: ldstr "sub_1A4130"
		IL_08f3: ldloc.s 88
		IL_08f5: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_08fa: ldstr "*(LocalExpr->klass + 0x40)"
		IL_08ff: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0904: ldc.i8 0
		IL_090d: ldc.i8 0
		IL_0916: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_091b: stloc.s 89

		IL_091d: nop
		IL_091e: ldloc.s 40
		IL_0920: ldc.i4.5
		IL_0921: ldloc.s 46
		IL_0923: stelem [mscorlib]System.Object
		IL_0928: ldstr "X"
		IL_092d: ldnull
		IL_092e: ceq
		IL_0930: brtrue.s IL_0969

		IL_0932: nop
		IL_0933: ldstr "sub_1A4130"
		IL_0938: ldstr "X"
		IL_093d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0942: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0947: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_094c: ldc.i8 0
		IL_0955: ldc.i8 0
		IL_095e: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0963: stloc.s 90
		IL_0965: ldloc.s 90
		IL_0967: stloc.s 91

		IL_0969: nop
		IL_096a: ldloc.s 40
		IL_096c: ldc.i4.6
		IL_096d: ldstr "X"
		IL_0972: stelem [mscorlib]System.Object
		IL_0977: ldarg.0
		IL_0978: ldfld float32 MathGameScript::num3
		IL_097d: stloc.s 92
		IL_097f: ldstr "*(BinaryExpr)"
		IL_0984: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0989: box [mscorlib]System.Int64
		IL_098e: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0993: box [mscorlib]System.Single
		IL_0998: stloc.s 93
		IL_099a: ldloc.s 93
		IL_099c: stloc.s 46
		IL_099e: ldloc.s 93
		IL_09a0: stloc.s 94
		IL_09a2: ldloc.s 93
		IL_09a4: ldnull
		IL_09a5: ceq
		IL_09a7: brtrue.s IL_09d9

		IL_09a9: nop
		IL_09aa: ldstr "sub_1A4130"
		IL_09af: ldloc.s 94
		IL_09b1: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_09b6: ldstr "*(LocalExpr->klass + 0x40)"
		IL_09bb: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_09c0: ldc.i8 0
		IL_09c9: ldc.i8 0
		IL_09d2: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_09d7: stloc.s 95

		IL_09d9: nop
		IL_09da: ldloc.s 40
		IL_09dc: ldc.i4.7
		IL_09dd: ldloc.s 46
		IL_09df: stelem [mscorlib]System.Object
		IL_09e4: ldstr "="
		IL_09e9: ldnull
		IL_09ea: ceq
		IL_09ec: brtrue.s IL_0a25

		IL_09ee: nop
		IL_09ef: ldstr "sub_1A4130"
		IL_09f4: ldstr "="
		IL_09f9: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_09fe: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0a03: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0a08: ldc.i8 0
		IL_0a11: ldc.i8 0
		IL_0a1a: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0a1f: stloc.s 96
		IL_0a21: ldloc.s 96
		IL_0a23: stloc.s 97

		IL_0a25: nop
		IL_0a26: ldloc.s 40
		IL_0a28: ldc.i4.8
		IL_0a29: ldstr "="
		IL_0a2e: stelem [mscorlib]System.Object
		IL_0a33: ldloc.s 40
		IL_0a35: call string [mscorlib]System.String::Concat(object[])
		IL_0a3a: stloc.s 98
		IL_0a3c: ldarg.0
		IL_0a3d: ldarg.0
		IL_0a3e: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip MathGameScript::bal_plus
		IL_0a43: callvirt instance void MathGameScript::QueueAudio(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_0a48: ldarg.0
		IL_0a49: ldarg.0
		IL_0a4a: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip MathGameScript::bal_screech
		IL_0a4f: callvirt instance void MathGameScript::QueueAudio(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_0a54: ldarg.0
		IL_0a55: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip MathGameScript::bal_times
		IL_0a5a: stloc.s 70

		IL_0a5c: nop
		IL_0a5d: ldarg.0
		IL_0a5e: ldloc.s 70
		IL_0a60: callvirt instance void MathGameScript::QueueAudio(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_0a65: ldarg.0
		IL_0a66: ldarg.0
		IL_0a67: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip MathGameScript::bal_screech
		IL_0a6c: callvirt instance void MathGameScript::QueueAudio(class [UnityEngine.AudioModule]UnityEngine.AudioClip)

		IL_0a71: nop
		IL_0a72: ldc.r4 1
		IL_0a77: ldc.r4 9999
		IL_0a7c: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_0a81: stloc.s 99
		IL_0a83: ldarg.0
		IL_0a84: ldloc.s 99
		IL_0a86: stfld float32 MathGameScript::num1
		IL_0a8b: ldc.r4 1
		IL_0a90: ldc.r4 9999
		IL_0a95: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_0a9a: stloc.s 100
		IL_0a9c: ldarg.0
		IL_0a9d: ldloc.s 100
		IL_0a9f: stfld float32 MathGameScript::num2
		IL_0aa4: ldc.r4 1
		IL_0aa9: ldc.r4 9999
		IL_0aae: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_0ab3: stloc.s 101
		IL_0ab5: ldarg.0
		IL_0ab6: ldloc.s 101
		IL_0ab8: stfld float32 MathGameScript::num3
		IL_0abd: ldc.i4.0
		IL_0abe: ldc.i4.1
		IL_0abf: call int32 [UnityEngine.CoreModule]UnityEngine.Random::Range(int32, int32)
		IL_0ac4: stloc.s 102
		IL_0ac6: ldloc.s 102
		IL_0ac8: conv.r4
		IL_0ac9: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_0ace: stloc.s 103
		IL_0ad0: ldarg.0
		IL_0ad1: ldloc.s 103
		IL_0ad3: stfld int32 MathGameScript::sign
		IL_0ad8: ldloc.s 103
		IL_0ada: stloc.s 104
		IL_0adc: ldloc.s 103
		IL_0ade: ldc.i4.0
		IL_0adf: ceq
		IL_0ae1: brtrue IL_0e4e

		IL_0ae6: nop
		IL_0ae7: ldloc.s 104
		IL_0ae9: ldc.i4.1
		IL_0aea: ceq
		IL_0aec: ldc.i4.0
		IL_0aed: ceq
		IL_0aef: brtrue IL_11ac

		IL_0af4: nop
		IL_0af5: ldc.i4.s 9
		IL_0af7: newarr [mscorlib]System.Object
		IL_0afc: stloc.s 105
		IL_0afe: ldloc.s 105
		IL_0b00: stloc.s 40
		IL_0b02: ldloc.s 105
		IL_0b04: stloc.s 106
		IL_0b06: ldarg.0
		IL_0b07: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text MathGameScript::questionText2
		IL_0b0c: stloc.s 42
		IL_0b0e: ldstr "SOLVE MATH Q"
		IL_0b13: ldnull
		IL_0b14: ceq
		IL_0b16: brtrue.s IL_0b4f

		IL_0b18: nop
		IL_0b19: ldstr "sub_1A4130"
		IL_0b1e: ldstr "SOLVE MATH Q"
		IL_0b23: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0b28: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0b2d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0b32: ldc.i8 0
		IL_0b3b: ldc.i8 0
		IL_0b44: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0b49: stloc.s 107
		IL_0b4b: ldloc.s 107
		IL_0b4d: stloc.s 108

		IL_0b4f: nop
		IL_0b50: ldloc.s 40
		IL_0b52: ldc.i4.0
		IL_0b53: ldstr "SOLVE MATH Q"
		IL_0b58: stelem [mscorlib]System.Object
		IL_0b5d: ldstr "*(BinaryExpr)"
		IL_0b62: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0b67: conv.i4
		IL_0b68: box [mscorlib]System.Int32
		IL_0b6d: stloc.s 109
		IL_0b6f: ldloc.s 109
		IL_0b71: stloc.s 46
		IL_0b73: ldloc.s 109
		IL_0b75: stloc.s 110
		IL_0b77: ldloc.s 109
		IL_0b79: ldnull
		IL_0b7a: ceq
		IL_0b7c: brtrue.s IL_0bae

		IL_0b7e: nop
		IL_0b7f: ldstr "sub_1A4130"
		IL_0b84: ldloc.s 110
		IL_0b86: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0b8b: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0b90: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0b95: ldc.i8 0
		IL_0b9e: ldc.i8 0
		IL_0ba7: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0bac: stloc.s 111

		IL_0bae: nop
		IL_0baf: ldloc.s 40
		IL_0bb1: ldc.i4.1
		IL_0bb2: ldloc.s 46
		IL_0bb4: stelem [mscorlib]System.Object
		IL_0bb9: ldstr ": \n ("
		IL_0bbe: ldnull
		IL_0bbf: ceq
		IL_0bc1: brtrue.s IL_0bfa

		IL_0bc3: nop
		IL_0bc4: ldstr "sub_1A4130"
		IL_0bc9: ldstr ": \n ("
		IL_0bce: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0bd3: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0bd8: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0bdd: ldc.i8 0
		IL_0be6: ldc.i8 0
		IL_0bef: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0bf4: stloc.s 112
		IL_0bf6: ldloc.s 112
		IL_0bf8: stloc.s 113

		IL_0bfa: nop
		IL_0bfb: ldloc.s 40
		IL_0bfd: ldc.i4.2
		IL_0bfe: ldstr ": \n ("
		IL_0c03: stelem [mscorlib]System.Object
		IL_0c08: ldarg.0
		IL_0c09: ldfld float32 MathGameScript::num1
		IL_0c0e: stloc.s 114
		IL_0c10: ldstr "*(BinaryExpr)"
		IL_0c15: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0c1a: box [mscorlib]System.Int64
		IL_0c1f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0c24: box [mscorlib]System.Single
		IL_0c29: stloc.s 115
		IL_0c2b: ldloc.s 115
		IL_0c2d: stloc.s 46
		IL_0c2f: ldloc.s 115
		IL_0c31: stloc.s 116
		IL_0c33: ldloc.s 115
		IL_0c35: ldnull
		IL_0c36: ceq
		IL_0c38: brtrue.s IL_0c6a

		IL_0c3a: nop
		IL_0c3b: ldstr "sub_1A4130"
		IL_0c40: ldloc.s 116
		IL_0c42: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0c47: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0c4c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0c51: ldc.i8 0
		IL_0c5a: ldc.i8 0
		IL_0c63: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0c68: stloc.s 117

		IL_0c6a: nop
		IL_0c6b: ldloc.s 40
		IL_0c6d: ldc.i4.3
		IL_0c6e: ldloc.s 46
		IL_0c70: stelem [mscorlib]System.Object
		IL_0c75: ldstr "/"
		IL_0c7a: ldnull
		IL_0c7b: ceq
		IL_0c7d: brtrue.s IL_0cb6

		IL_0c7f: nop
		IL_0c80: ldstr "sub_1A4130"
		IL_0c85: ldstr "/"
		IL_0c8a: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0c8f: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0c94: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0c99: ldc.i8 0
		IL_0ca2: ldc.i8 0
		IL_0cab: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0cb0: stloc.s 118
		IL_0cb2: ldloc.s 118
		IL_0cb4: stloc.s 119

		IL_0cb6: nop
		IL_0cb7: ldloc.s 40
		IL_0cb9: ldc.i4.4
		IL_0cba: ldstr "/"
		IL_0cbf: stelem [mscorlib]System.Object
		IL_0cc4: ldarg.0
		IL_0cc5: ldfld float32 MathGameScript::num2
		IL_0cca: stloc.s 120
		IL_0ccc: ldstr "*(BinaryExpr)"
		IL_0cd1: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0cd6: box [mscorlib]System.Int64
		IL_0cdb: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0ce0: box [mscorlib]System.Single
		IL_0ce5: stloc.s 121
		IL_0ce7: ldloc.s 121
		IL_0ce9: stloc.s 46
		IL_0ceb: ldloc.s 121
		IL_0ced: stloc.s 122
		IL_0cef: ldloc.s 121
		IL_0cf1: ldnull
		IL_0cf2: ceq
		IL_0cf4: brtrue.s IL_0d26

		IL_0cf6: nop
		IL_0cf7: ldstr "sub_1A4130"
		IL_0cfc: ldloc.s 122
		IL_0cfe: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0d03: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0d08: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0d0d: ldc.i8 0
		IL_0d16: ldc.i8 0
		IL_0d1f: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0d24: stloc.s 123

		IL_0d26: nop
		IL_0d27: ldloc.s 40
		IL_0d29: ldc.i4.5
		IL_0d2a: ldloc.s 46
		IL_0d2c: stelem [mscorlib]System.Object
		IL_0d31: ldstr ")+"
		IL_0d36: ldnull
		IL_0d37: ceq
		IL_0d39: brtrue.s IL_0d72

		IL_0d3b: nop
		IL_0d3c: ldstr "sub_1A4130"
		IL_0d41: ldstr ")+"
		IL_0d46: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0d4b: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0d50: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0d55: ldc.i8 0
		IL_0d5e: ldc.i8 0
		IL_0d67: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0d6c: stloc.s 124
		IL_0d6e: ldloc.s 124
		IL_0d70: stloc.s 125

		IL_0d72: nop
		IL_0d73: ldloc.s 40
		IL_0d75: ldc.i4.6
		IL_0d76: ldstr ")+"
		IL_0d7b: stelem [mscorlib]System.Object
		IL_0d80: ldarg.0
		IL_0d81: ldfld float32 MathGameScript::num3
		IL_0d86: stloc.s 126
		IL_0d88: ldstr "*(BinaryExpr)"
		IL_0d8d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0d92: box [mscorlib]System.Int64
		IL_0d97: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0d9c: box [mscorlib]System.Single
		IL_0da1: stloc.s 127
		IL_0da3: ldloc.s 127
		IL_0da5: stloc.s 46
		IL_0da7: ldloc.s 127
		IL_0da9: stloc.s 128
		IL_0dab: ldloc.s 127
		IL_0dad: ldnull
		IL_0dae: ceq
		IL_0db0: brtrue.s IL_0de2

		IL_0db2: nop
		IL_0db3: ldstr "sub_1A4130"
		IL_0db8: ldloc.s 128
		IL_0dba: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0dbf: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0dc4: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0dc9: ldc.i8 0
		IL_0dd2: ldc.i8 0
		IL_0ddb: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0de0: stloc.s 129

		IL_0de2: nop
		IL_0de3: ldloc.s 40
		IL_0de5: ldc.i4.7
		IL_0de6: ldloc.s 46
		IL_0de8: stelem [mscorlib]System.Object
		IL_0ded: ldstr "="
		IL_0df2: ldnull
		IL_0df3: ceq
		IL_0df5: brtrue.s IL_0e2e

		IL_0df7: nop
		IL_0df8: ldstr "sub_1A4130"
		IL_0dfd: ldstr "="
		IL_0e02: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0e07: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0e0c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0e11: ldc.i8 0
		IL_0e1a: ldc.i8 0
		IL_0e23: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0e28: stloc.s 130
		IL_0e2a: ldloc.s 130
		IL_0e2c: stloc.s 131

		IL_0e2e: nop
		IL_0e2f: ldloc.s 40
		IL_0e31: ldc.i4.8
		IL_0e32: ldstr "="
		IL_0e37: stelem [mscorlib]System.Object
		IL_0e3c: ldloc.s 40
		IL_0e3e: call string [mscorlib]System.String::Concat(object[])
		IL_0e43: stloc.s 132
		IL_0e45: ldloc.s 132
		IL_0e47: stloc.s 133
		IL_0e49: br IL_11a3

		IL_0e4e: nop
		IL_0e4f: ldc.i4.s 9
		IL_0e51: newarr [mscorlib]System.Object
		IL_0e56: stloc.s 134
		IL_0e58: ldloc.s 134
		IL_0e5a: stloc.s 40
		IL_0e5c: ldloc.s 134
		IL_0e5e: stloc.s 135
		IL_0e60: ldarg.0
		IL_0e61: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text MathGameScript::questionText2
		IL_0e66: stloc.s 42
		IL_0e68: ldstr "SOLVE MATH Q"
		IL_0e6d: ldnull
		IL_0e6e: ceq
		IL_0e70: brtrue.s IL_0ea9

		IL_0e72: nop
		IL_0e73: ldstr "sub_1A4130"
		IL_0e78: ldstr "SOLVE MATH Q"
		IL_0e7d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0e82: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0e87: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0e8c: ldc.i8 0
		IL_0e95: ldc.i8 0
		IL_0e9e: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0ea3: stloc.s 136
		IL_0ea5: ldloc.s 136
		IL_0ea7: stloc.s 137

		IL_0ea9: nop
		IL_0eaa: ldloc.s 40
		IL_0eac: ldc.i4.0
		IL_0ead: ldstr "SOLVE MATH Q"
		IL_0eb2: stelem [mscorlib]System.Object
		IL_0eb7: ldstr "*(BinaryExpr)"
		IL_0ebc: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0ec1: conv.i4
		IL_0ec2: box [mscorlib]System.Int32
		IL_0ec7: stloc.s 138
		IL_0ec9: ldloc.s 138
		IL_0ecb: stloc.s 46
		IL_0ecd: ldloc.s 138
		IL_0ecf: stloc.s 139
		IL_0ed1: ldloc.s 138
		IL_0ed3: ldnull
		IL_0ed4: ceq
		IL_0ed6: brtrue.s IL_0f08

		IL_0ed8: nop
		IL_0ed9: ldstr "sub_1A4130"
		IL_0ede: ldloc.s 139
		IL_0ee0: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0ee5: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0eea: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0eef: ldc.i8 0
		IL_0ef8: ldc.i8 0
		IL_0f01: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0f06: stloc.s 140

		IL_0f08: nop
		IL_0f09: ldloc.s 40
		IL_0f0b: ldc.i4.1
		IL_0f0c: ldloc.s 46
		IL_0f0e: stelem [mscorlib]System.Object
		IL_0f13: ldstr ": \n"
		IL_0f18: ldnull
		IL_0f19: ceq
		IL_0f1b: brtrue.s IL_0f54

		IL_0f1d: nop
		IL_0f1e: ldstr "sub_1A4130"
		IL_0f23: ldstr ": \n"
		IL_0f28: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0f2d: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0f32: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0f37: ldc.i8 0
		IL_0f40: ldc.i8 0
		IL_0f49: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0f4e: stloc.s 141
		IL_0f50: ldloc.s 141
		IL_0f52: stloc.s 142

		IL_0f54: nop
		IL_0f55: ldloc.s 40
		IL_0f57: ldc.i4.2
		IL_0f58: ldstr ": \n"
		IL_0f5d: stelem [mscorlib]System.Object
		IL_0f62: ldarg.0
		IL_0f63: ldfld float32 MathGameScript::num1
		IL_0f68: stloc.s 143
		IL_0f6a: ldstr "*(BinaryExpr)"
		IL_0f6f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0f74: box [mscorlib]System.Int64
		IL_0f79: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0f7e: box [mscorlib]System.Single
		IL_0f83: stloc.s 144
		IL_0f85: ldloc.s 144
		IL_0f87: stloc.s 46
		IL_0f89: ldloc.s 144
		IL_0f8b: stloc.s 145
		IL_0f8d: ldloc.s 144
		IL_0f8f: ldnull
		IL_0f90: ceq
		IL_0f92: brtrue.s IL_0fc4

		IL_0f94: nop
		IL_0f95: ldstr "sub_1A4130"
		IL_0f9a: ldloc.s 145
		IL_0f9c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0fa1: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0fa6: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0fab: ldc.i8 0
		IL_0fb4: ldc.i8 0
		IL_0fbd: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0fc2: stloc.s 146

		IL_0fc4: nop
		IL_0fc5: ldloc.s 40
		IL_0fc7: ldc.i4.3
		IL_0fc8: ldloc.s 46
		IL_0fca: stelem [mscorlib]System.Object
		IL_0fcf: ldstr "+("
		IL_0fd4: ldnull
		IL_0fd5: ceq
		IL_0fd7: brtrue.s IL_1010

		IL_0fd9: nop
		IL_0fda: ldstr "sub_1A4130"
		IL_0fdf: ldstr "+("
		IL_0fe4: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0fe9: ldstr "*(LocalExpr->klass + 0x40)"
		IL_0fee: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0ff3: ldc.i8 0
		IL_0ffc: ldc.i8 0
		IL_1005: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_100a: stloc.s 147
		IL_100c: ldloc.s 147
		IL_100e: stloc.s 148

		IL_1010: nop
		IL_1011: ldloc.s 40
		IL_1013: ldc.i4.4
		IL_1014: ldstr "+("
		IL_1019: stelem [mscorlib]System.Object
		IL_101e: ldarg.0
		IL_101f: ldfld float32 MathGameScript::num2
		IL_1024: stloc.s 149
		IL_1026: ldstr "*(BinaryExpr)"
		IL_102b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1030: box [mscorlib]System.Int64
		IL_1035: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_103a: box [mscorlib]System.Single
		IL_103f: stloc.s 150
		IL_1041: ldloc.s 150
		IL_1043: stloc.s 46
		IL_1045: ldloc.s 150
		IL_1047: stloc.s 151
		IL_1049: ldloc.s 150
		IL_104b: ldnull
		IL_104c: ceq
		IL_104e: brtrue.s IL_1080

		IL_1050: nop
		IL_1051: ldstr "sub_1A4130"
		IL_1056: ldloc.s 151
		IL_1058: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_105d: ldstr "*(LocalExpr->klass + 0x40)"
		IL_1062: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1067: ldc.i8 0
		IL_1070: ldc.i8 0
		IL_1079: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_107e: stloc.s 152

		IL_1080: nop
		IL_1081: ldloc.s 40
		IL_1083: ldc.i4.5
		IL_1084: ldloc.s 46
		IL_1086: stelem [mscorlib]System.Object
		IL_108b: ldstr "X"
		IL_1090: ldnull
		IL_1091: ceq
		IL_1093: brtrue.s IL_10cc

		IL_1095: nop
		IL_1096: ldstr "sub_1A4130"
		IL_109b: ldstr "X"
		IL_10a0: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_10a5: ldstr "*(LocalExpr->klass + 0x40)"
		IL_10aa: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_10af: ldc.i8 0
		IL_10b8: ldc.i8 0
		IL_10c1: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_10c6: stloc.s 153
		IL_10c8: ldloc.s 153
		IL_10ca: stloc.s 154

		IL_10cc: nop
		IL_10cd: ldloc.s 40
		IL_10cf: ldc.i4.6
		IL_10d0: ldstr "X"
		IL_10d5: stelem [mscorlib]System.Object
		IL_10da: ldarg.0
		IL_10db: ldfld float32 MathGameScript::num3
		IL_10e0: stloc.s 155
		IL_10e2: ldstr "*(BinaryExpr)"
		IL_10e7: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_10ec: box [mscorlib]System.Int64
		IL_10f1: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_10f6: box [mscorlib]System.Single
		IL_10fb: stloc.s 156
		IL_10fd: ldloc.s 156
		IL_10ff: stloc.s 46
		IL_1101: ldloc.s 156
		IL_1103: stloc.s 157
		IL_1105: ldloc.s 156
		IL_1107: ldnull
		IL_1108: ceq
		IL_110a: brtrue.s IL_113c

		IL_110c: nop
		IL_110d: ldstr "sub_1A4130"
		IL_1112: ldloc.s 157
		IL_1114: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1119: ldstr "*(LocalExpr->klass + 0x40)"
		IL_111e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1123: ldc.i8 0
		IL_112c: ldc.i8 0
		IL_1135: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_113a: stloc.s 158

		IL_113c: nop
		IL_113d: ldloc.s 40
		IL_113f: ldc.i4.7
		IL_1140: ldloc.s 46
		IL_1142: stelem [mscorlib]System.Object
		IL_1147: ldstr "="
		IL_114c: ldnull
		IL_114d: ceq
		IL_114f: brtrue.s IL_1188

		IL_1151: nop
		IL_1152: ldstr "sub_1A4130"
		IL_1157: ldstr "="
		IL_115c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1161: ldstr "*(LocalExpr->klass + 0x40)"
		IL_1166: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_116b: ldc.i8 0
		IL_1174: ldc.i8 0
		IL_117d: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_1182: stloc.s 159
		IL_1184: ldloc.s 159
		IL_1186: stloc.s 160

		IL_1188: nop
		IL_1189: ldloc.s 40
		IL_118b: ldc.i4.8
		IL_118c: ldstr "="
		IL_1191: stelem [mscorlib]System.Object
		IL_1196: ldloc.s 40
		IL_1198: call string [mscorlib]System.String::Concat(object[])
		IL_119d: stloc.s 161
		IL_119f: ldloc.s 161
		IL_11a1: stloc.s 162

		IL_11a3: nop
		IL_11a4: ldloc.s 42
		IL_11a6: stloc.s 163
		IL_11a8: ldloc.s 162
		IL_11aa: stloc.s 164

		IL_11ac: nop
		IL_11ad: ldc.r4 1
		IL_11b2: ldc.r4 9999
		IL_11b7: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_11bc: stloc.s 165
		IL_11be: ldarg.0
		IL_11bf: ldloc.s 165
		IL_11c1: stfld float32 MathGameScript::num1
		IL_11c6: ldc.r4 1
		IL_11cb: ldc.r4 9999
		IL_11d0: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_11d5: stloc.s 166
		IL_11d7: ldarg.0
		IL_11d8: ldloc.s 166
		IL_11da: stfld float32 MathGameScript::num2
		IL_11df: ldc.r4 1
		IL_11e4: ldc.r4 9999
		IL_11e9: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_11ee: stloc.s 167
		IL_11f0: ldarg.0
		IL_11f1: ldloc.s 167
		IL_11f3: stfld float32 MathGameScript::num3
		IL_11f8: ldc.i4.0
		IL_11f9: ldc.i4.1
		IL_11fa: call int32 [UnityEngine.CoreModule]UnityEngine.Random::Range(int32, int32)
		IL_11ff: stloc.s 168
		IL_1201: ldloc.s 168
		IL_1203: conv.r4
		IL_1204: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_1209: stloc.s 169
		IL_120b: ldarg.0
		IL_120c: ldloc.s 169
		IL_120e: stfld int32 MathGameScript::sign
		IL_1213: ldloc.s 169
		IL_1215: stloc.s 170
		IL_1217: ldloc.s 169
		IL_1219: ldc.i4.0
		IL_121a: ceq
		IL_121c: brtrue IL_1589

		IL_1221: nop
		IL_1222: ldloc.s 170
		IL_1224: ldc.i4.1
		IL_1225: ceq
		IL_1227: ldc.i4.0
		IL_1228: ceq
		IL_122a: brtrue IL_18df

		IL_122f: nop
		IL_1230: ldc.i4.s 9
		IL_1232: newarr [mscorlib]System.Object
		IL_1237: stloc.s 171
		IL_1239: ldloc.s 171
		IL_123b: stloc.s 40
		IL_123d: ldloc.s 171
		IL_123f: stloc.s 172
		IL_1241: ldarg.0
		IL_1242: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text MathGameScript::questionText3
		IL_1247: stloc.s 42
		IL_1249: ldstr "SOLVE MATH Q"
		IL_124e: ldnull
		IL_124f: ceq
		IL_1251: brtrue.s IL_128a

		IL_1253: nop
		IL_1254: ldstr "sub_1A4130"
		IL_1259: ldstr "SOLVE MATH Q"
		IL_125e: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1263: ldstr "*(LocalExpr->klass + 0x40)"
		IL_1268: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_126d: ldc.i8 0
		IL_1276: ldc.i8 0
		IL_127f: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_1284: stloc.s 173
		IL_1286: ldloc.s 173
		IL_1288: stloc.s 174

		IL_128a: nop
		IL_128b: ldloc.s 40
		IL_128d: ldc.i4.0
		IL_128e: ldstr "SOLVE MATH Q"
		IL_1293: stelem [mscorlib]System.Object
		IL_1298: ldstr "*(BinaryExpr)"
		IL_129d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_12a2: conv.i4
		IL_12a3: box [mscorlib]System.Int32
		IL_12a8: stloc.s 175
		IL_12aa: ldloc.s 175
		IL_12ac: stloc.s 46
		IL_12ae: ldloc.s 175
		IL_12b0: stloc.s 176
		IL_12b2: ldloc.s 175
		IL_12b4: ldnull
		IL_12b5: ceq
		IL_12b7: brtrue.s IL_12e9

		IL_12b9: nop
		IL_12ba: ldstr "sub_1A4130"
		IL_12bf: ldloc.s 176
		IL_12c1: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_12c6: ldstr "*(LocalExpr->klass + 0x40)"
		IL_12cb: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_12d0: ldc.i8 0
		IL_12d9: ldc.i8 0
		IL_12e2: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_12e7: stloc.s 177

		IL_12e9: nop
		IL_12ea: ldloc.s 40
		IL_12ec: ldc.i4.1
		IL_12ed: ldloc.s 46
		IL_12ef: stelem [mscorlib]System.Object
		IL_12f4: ldstr ": \n ("
		IL_12f9: ldnull
		IL_12fa: ceq
		IL_12fc: brtrue.s IL_1335

		IL_12fe: nop
		IL_12ff: ldstr "sub_1A4130"
		IL_1304: ldstr ": \n ("
		IL_1309: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_130e: ldstr "*(LocalExpr->klass + 0x40)"
		IL_1313: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1318: ldc.i8 0
		IL_1321: ldc.i8 0
		IL_132a: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_132f: stloc.s 178
		IL_1331: ldloc.s 178
		IL_1333: stloc.s 179

		IL_1335: nop
		IL_1336: ldloc.s 40
		IL_1338: ldc.i4.2
		IL_1339: ldstr ": \n ("
		IL_133e: stelem [mscorlib]System.Object
		IL_1343: ldarg.0
		IL_1344: ldfld float32 MathGameScript::num1
		IL_1349: stloc.s 180
		IL_134b: ldstr "*(BinaryExpr)"
		IL_1350: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1355: box [mscorlib]System.Int64
		IL_135a: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_135f: box [mscorlib]System.Single
		IL_1364: stloc.s 181
		IL_1366: ldloc.s 181
		IL_1368: stloc.s 46
		IL_136a: ldloc.s 181
		IL_136c: stloc.s 182
		IL_136e: ldloc.s 181
		IL_1370: ldnull
		IL_1371: ceq
		IL_1373: brtrue.s IL_13a5

		IL_1375: nop
		IL_1376: ldstr "sub_1A4130"
		IL_137b: ldloc.s 182
		IL_137d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1382: ldstr "*(LocalExpr->klass + 0x40)"
		IL_1387: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_138c: ldc.i8 0
		IL_1395: ldc.i8 0
		IL_139e: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_13a3: stloc.s 183

		IL_13a5: nop
		IL_13a6: ldloc.s 40
		IL_13a8: ldc.i4.3
		IL_13a9: ldloc.s 46
		IL_13ab: stelem [mscorlib]System.Object
		IL_13b0: ldstr "/"
		IL_13b5: ldnull
		IL_13b6: ceq
		IL_13b8: brtrue.s IL_13f1

		IL_13ba: nop
		IL_13bb: ldstr "sub_1A4130"
		IL_13c0: ldstr "/"
		IL_13c5: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_13ca: ldstr "*(LocalExpr->klass + 0x40)"
		IL_13cf: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_13d4: ldc.i8 0
		IL_13dd: ldc.i8 0
		IL_13e6: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_13eb: stloc.s 184
		IL_13ed: ldloc.s 184
		IL_13ef: stloc.s 185

		IL_13f1: nop
		IL_13f2: ldloc.s 40
		IL_13f4: ldc.i4.4
		IL_13f5: ldstr "/"
		IL_13fa: stelem [mscorlib]System.Object
		IL_13ff: ldarg.0
		IL_1400: ldfld float32 MathGameScript::num2
		IL_1405: stloc.s 186
		IL_1407: ldstr "*(BinaryExpr)"
		IL_140c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1411: box [mscorlib]System.Int64
		IL_1416: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_141b: box [mscorlib]System.Single
		IL_1420: stloc.s 187
		IL_1422: ldloc.s 187
		IL_1424: stloc.s 46
		IL_1426: ldloc.s 187
		IL_1428: stloc.s 188
		IL_142a: ldloc.s 187
		IL_142c: ldnull
		IL_142d: ceq
		IL_142f: brtrue.s IL_1461

		IL_1431: nop
		IL_1432: ldstr "sub_1A4130"
		IL_1437: ldloc.s 188
		IL_1439: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_143e: ldstr "*(LocalExpr->klass + 0x40)"
		IL_1443: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1448: ldc.i8 0
		IL_1451: ldc.i8 0
		IL_145a: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_145f: stloc.s 189

		IL_1461: nop
		IL_1462: ldloc.s 40
		IL_1464: ldc.i4.5
		IL_1465: ldloc.s 46
		IL_1467: stelem [mscorlib]System.Object
		IL_146c: ldstr ")+"
		IL_1471: ldnull
		IL_1472: ceq
		IL_1474: brtrue.s IL_14ad

		IL_1476: nop
		IL_1477: ldstr "sub_1A4130"
		IL_147c: ldstr ")+"
		IL_1481: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1486: ldstr "*(LocalExpr->klass + 0x40)"
		IL_148b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1490: ldc.i8 0
		IL_1499: ldc.i8 0
		IL_14a2: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_14a7: stloc.s 190
		IL_14a9: ldloc.s 190
		IL_14ab: stloc.s 191

		IL_14ad: nop
		IL_14ae: ldloc.s 40
		IL_14b0: ldc.i4.6
		IL_14b1: ldstr ")+"
		IL_14b6: stelem [mscorlib]System.Object
		IL_14bb: ldarg.0
		IL_14bc: ldfld float32 MathGameScript::num3
		IL_14c1: stloc.s 192
		IL_14c3: ldstr "*(BinaryExpr)"
		IL_14c8: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_14cd: box [mscorlib]System.Int64
		IL_14d2: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_14d7: box [mscorlib]System.Single
		IL_14dc: stloc.s 193
		IL_14de: ldloc.s 193
		IL_14e0: stloc.s 46
		IL_14e2: ldloc.s 193
		IL_14e4: stloc.s 194
		IL_14e6: ldloc.s 193
		IL_14e8: ldnull
		IL_14e9: ceq
		IL_14eb: brtrue.s IL_151d

		IL_14ed: nop
		IL_14ee: ldstr "sub_1A4130"
		IL_14f3: ldloc.s 194
		IL_14f5: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_14fa: ldstr "*(LocalExpr->klass + 0x40)"
		IL_14ff: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1504: ldc.i8 0
		IL_150d: ldc.i8 0
		IL_1516: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_151b: stloc.s 195

		IL_151d: nop
		IL_151e: ldloc.s 40
		IL_1520: ldc.i4.7
		IL_1521: ldloc.s 46
		IL_1523: stelem [mscorlib]System.Object
		IL_1528: ldstr "="
		IL_152d: ldnull
		IL_152e: ceq
		IL_1530: brtrue.s IL_1569

		IL_1532: nop
		IL_1533: ldstr "sub_1A4130"
		IL_1538: ldstr "="
		IL_153d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1542: ldstr "*(LocalExpr->klass + 0x40)"
		IL_1547: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_154c: ldc.i8 0
		IL_1555: ldc.i8 0
		IL_155e: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_1563: stloc.s 196
		IL_1565: ldloc.s 196
		IL_1567: stloc.s 197

		IL_1569: nop
		IL_156a: ldloc.s 40
		IL_156c: ldc.i4.8
		IL_156d: ldstr "="
		IL_1572: stelem [mscorlib]System.Object
		IL_1577: ldloc.s 40
		IL_1579: call string [mscorlib]System.String::Concat(object[])
		IL_157e: stloc.s 198
		IL_1580: ldloc.s 198
		IL_1582: stloc.s 199
		IL_1584: br IL_18de

		IL_1589: nop
		IL_158a: ldc.i4.s 9
		IL_158c: newarr [mscorlib]System.Object
		IL_1591: stloc.s 200
		IL_1593: ldloc.s 200
		IL_1595: stloc.s 40
		IL_1597: ldloc.s 200
		IL_1599: stloc.s 201
		IL_159b: ldarg.0
		IL_159c: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text MathGameScript::questionText3
		IL_15a1: stloc.s 42
		IL_15a3: ldstr "SOLVE MATH Q"
		IL_15a8: ldnull
		IL_15a9: ceq
		IL_15ab: brtrue.s IL_15e4

		IL_15ad: nop
		IL_15ae: ldstr "sub_1A4130"
		IL_15b3: ldstr "SOLVE MATH Q"
		IL_15b8: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_15bd: ldstr "*(LocalExpr->klass + 0x40)"
		IL_15c2: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_15c7: ldc.i8 0
		IL_15d0: ldc.i8 0
		IL_15d9: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_15de: stloc.s 202
		IL_15e0: ldloc.s 202
		IL_15e2: stloc.s 203

		IL_15e4: nop
		IL_15e5: ldloc.s 40
		IL_15e7: ldc.i4.0
		IL_15e8: ldstr "SOLVE MATH Q"
		IL_15ed: stelem [mscorlib]System.Object
		IL_15f2: ldstr "*(BinaryExpr)"
		IL_15f7: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_15fc: conv.i4
		IL_15fd: box [mscorlib]System.Int32
		IL_1602: stloc.s 204
		IL_1604: ldloc.s 204
		IL_1606: stloc.s 46
		IL_1608: ldloc.s 204
		IL_160a: stloc.s 205
		IL_160c: ldloc.s 204
		IL_160e: ldnull
		IL_160f: ceq
		IL_1611: brtrue.s IL_1643

		IL_1613: nop
		IL_1614: ldstr "sub_1A4130"
		IL_1619: ldloc.s 205
		IL_161b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1620: ldstr "*(LocalExpr->klass + 0x40)"
		IL_1625: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_162a: ldc.i8 0
		IL_1633: ldc.i8 0
		IL_163c: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_1641: stloc.s 206

		IL_1643: nop
		IL_1644: ldloc.s 40
		IL_1646: ldc.i4.1
		IL_1647: ldloc.s 46
		IL_1649: stelem [mscorlib]System.Object
		IL_164e: ldstr ": \n"
		IL_1653: ldnull
		IL_1654: ceq
		IL_1656: brtrue.s IL_168f

		IL_1658: nop
		IL_1659: ldstr "sub_1A4130"
		IL_165e: ldstr ": \n"
		IL_1663: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1668: ldstr "*(LocalExpr->klass + 0x40)"
		IL_166d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1672: ldc.i8 0
		IL_167b: ldc.i8 0
		IL_1684: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_1689: stloc.s 207
		IL_168b: ldloc.s 207
		IL_168d: stloc.s 208

		IL_168f: nop
		IL_1690: ldloc.s 40
		IL_1692: ldc.i4.2
		IL_1693: ldstr ": \n"
		IL_1698: stelem [mscorlib]System.Object
		IL_169d: ldarg.0
		IL_169e: ldfld float32 MathGameScript::num1
		IL_16a3: stloc.s 209
		IL_16a5: ldstr "*(BinaryExpr)"
		IL_16aa: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_16af: box [mscorlib]System.Int64
		IL_16b4: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_16b9: box [mscorlib]System.Single
		IL_16be: stloc.s 210
		IL_16c0: ldloc.s 210
		IL_16c2: stloc.s 46
		IL_16c4: ldloc.s 210
		IL_16c6: stloc.s 211
		IL_16c8: ldloc.s 210
		IL_16ca: ldnull
		IL_16cb: ceq
		IL_16cd: brtrue.s IL_16ff

		IL_16cf: nop
		IL_16d0: ldstr "sub_1A4130"
		IL_16d5: ldloc.s 211
		IL_16d7: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_16dc: ldstr "*(LocalExpr->klass + 0x40)"
		IL_16e1: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_16e6: ldc.i8 0
		IL_16ef: ldc.i8 0
		IL_16f8: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_16fd: stloc.s 212

		IL_16ff: nop
		IL_1700: ldloc.s 40
		IL_1702: ldc.i4.3
		IL_1703: ldloc.s 46
		IL_1705: stelem [mscorlib]System.Object
		IL_170a: ldstr "+("
		IL_170f: ldnull
		IL_1710: ceq
		IL_1712: brtrue.s IL_174b

		IL_1714: nop
		IL_1715: ldstr "sub_1A4130"
		IL_171a: ldstr "+("
		IL_171f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1724: ldstr "*(LocalExpr->klass + 0x40)"
		IL_1729: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_172e: ldc.i8 0
		IL_1737: ldc.i8 0
		IL_1740: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_1745: stloc.s 213
		IL_1747: ldloc.s 213
		IL_1749: stloc.s 214

		IL_174b: nop
		IL_174c: ldloc.s 40
		IL_174e: ldc.i4.4
		IL_174f: ldstr "+("
		IL_1754: stelem [mscorlib]System.Object
		IL_1759: ldarg.0
		IL_175a: ldfld float32 MathGameScript::num2
		IL_175f: stloc.s 215
		IL_1761: ldstr "*(BinaryExpr)"
		IL_1766: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_176b: box [mscorlib]System.Int64
		IL_1770: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_1775: box [mscorlib]System.Single
		IL_177a: stloc.s 216
		IL_177c: ldloc.s 216
		IL_177e: stloc.s 46
		IL_1780: ldloc.s 216
		IL_1782: stloc.s 217
		IL_1784: ldloc.s 216
		IL_1786: ldnull
		IL_1787: ceq
		IL_1789: brtrue.s IL_17bb

		IL_178b: nop
		IL_178c: ldstr "sub_1A4130"
		IL_1791: ldloc.s 217
		IL_1793: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1798: ldstr "*(LocalExpr->klass + 0x40)"
		IL_179d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_17a2: ldc.i8 0
		IL_17ab: ldc.i8 0
		IL_17b4: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_17b9: stloc.s 218

		IL_17bb: nop
		IL_17bc: ldloc.s 40
		IL_17be: ldc.i4.5
		IL_17bf: ldloc.s 46
		IL_17c1: stelem [mscorlib]System.Object
		IL_17c6: ldstr "X"
		IL_17cb: ldnull
		IL_17cc: ceq
		IL_17ce: brtrue.s IL_1807

		IL_17d0: nop
		IL_17d1: ldstr "sub_1A4130"
		IL_17d6: ldstr "X"
		IL_17db: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_17e0: ldstr "*(LocalExpr->klass + 0x40)"
		IL_17e5: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_17ea: ldc.i8 0
		IL_17f3: ldc.i8 0
		IL_17fc: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_1801: stloc.s 219
		IL_1803: ldloc.s 219
		IL_1805: stloc.s 220

		IL_1807: nop
		IL_1808: ldloc.s 40
		IL_180a: ldc.i4.6
		IL_180b: ldstr "X"
		IL_1810: stelem [mscorlib]System.Object
		IL_1815: ldarg.0
		IL_1816: ldfld float32 MathGameScript::num3
		IL_181b: stloc.s 221
		IL_181d: ldstr "*(BinaryExpr)"
		IL_1822: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1827: box [mscorlib]System.Int64
		IL_182c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_1831: box [mscorlib]System.Single
		IL_1836: stloc.s 222
		IL_1838: ldloc.s 222
		IL_183a: stloc.s 46
		IL_183c: ldloc.s 222
		IL_183e: stloc.s 223
		IL_1840: ldloc.s 222
		IL_1842: ldnull
		IL_1843: ceq
		IL_1845: brtrue.s IL_1877

		IL_1847: nop
		IL_1848: ldstr "sub_1A4130"
		IL_184d: ldloc.s 223
		IL_184f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1854: ldstr "*(LocalExpr->klass + 0x40)"
		IL_1859: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_185e: ldc.i8 0
		IL_1867: ldc.i8 0
		IL_1870: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_1875: stloc.s 224

		IL_1877: nop
		IL_1878: ldloc.s 40
		IL_187a: ldc.i4.7
		IL_187b: ldloc.s 46
		IL_187d: stelem [mscorlib]System.Object
		IL_1882: ldstr "="
		IL_1887: ldnull
		IL_1888: ceq
		IL_188a: brtrue.s IL_18c3

		IL_188c: nop
		IL_188d: ldstr "sub_1A4130"
		IL_1892: ldstr "="
		IL_1897: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_189c: ldstr "*(LocalExpr->klass + 0x40)"
		IL_18a1: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_18a6: ldc.i8 0
		IL_18af: ldc.i8 0
		IL_18b8: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_18bd: stloc.s 225
		IL_18bf: ldloc.s 225
		IL_18c1: stloc.s 226

		IL_18c3: nop
		IL_18c4: ldloc.s 40
		IL_18c6: ldc.i4.8
		IL_18c7: ldstr "="
		IL_18cc: stelem [mscorlib]System.Object
		IL_18d1: ldloc.s 40
		IL_18d3: call string [mscorlib]System.String::Concat(object[])
		IL_18d8: stloc.s 227
		IL_18da: ldloc.s 227
		IL_18dc: stloc.s 228

		IL_18de: nop

		IL_18df: nop
		IL_18e0: ldarg.0
		IL_18e1: ldarg.0
		IL_18e2: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip MathGameScript::bal_equals
		IL_18e7: callvirt instance void MathGameScript::QueueAudio(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_18ec: ldarg.0
		IL_18ed: ldc.i4.1
		IL_18ee: stfld bool MathGameScript::questionInProgress
		IL_18f3: br IL_0108

		IL_18f8: nop
		IL_18f9: ldc.r4 0.0
		IL_18fe: ldc.r4 9
		IL_1903: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_1908: stloc.s 229
		IL_190a: ldloc.s 229
		IL_190c: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_1911: stloc.s 230
		IL_1913: ldarg.0
		IL_1914: ldloc.s 230
		IL_1916: conv.r4
		IL_1917: stfld float32 MathGameScript::num1
		IL_191c: ldc.r4 0.0
		IL_1921: ldc.r4 9
		IL_1926: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_192b: stloc.s 231
		IL_192d: ldloc.s 231
		IL_192f: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_1934: stloc.s 232
		IL_1936: ldarg.0
		IL_1937: ldloc.s 232
		IL_1939: conv.r4
		IL_193a: stfld float32 MathGameScript::num2
		IL_193f: ldc.r4 0.0
		IL_1944: ldc.r4 1
		IL_1949: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_194e: stloc.s 233
		IL_1950: ldloc.s 233
		IL_1952: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_1957: stloc.s 234
		IL_1959: ldarg.0
		IL_195a: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] MathGameScript::bal_numbers
		IL_195f: stloc.s 235
		IL_1961: ldarg.0
		IL_1962: ldfld float32 MathGameScript::num1
		IL_1967: stloc.s 236
		IL_1969: ldarg.0
		IL_196a: ldloc.s 234
		IL_196c: stfld int32 MathGameScript::sign
		IL_1971: ldloc.s 236
		IL_1973: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_1978: stloc.s 237
		IL_197a: ldarg.0
		IL_197b: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] MathGameScript::audioQueue
		IL_1980: stloc.s 238
		IL_1982: ldarg.0
		IL_1983: ldfld int32 MathGameScript::audioInQueue
		IL_1988: conv.i8
		IL_1989: stloc.s 239
		IL_198b: ldloc.s 235
		IL_198d: ldloc.s 237
		IL_198f: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_1994: stloc.s 240
		IL_1996: ldloc.s 238
		IL_1998: ldloc.s 239
		IL_199a: conv.i4
		IL_199b: ldloc.s 240
		IL_199d: stelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_19a2: ldarg.0
		IL_19a3: ldarg.0
		IL_19a4: ldfld int32 MathGameScript::audioInQueue
		IL_19a9: ldc.i4.1
		IL_19aa: add
		IL_19ab: stfld int32 MathGameScript::audioInQueue
		IL_19b0: ldarg.0
		IL_19b1: ldfld int32 MathGameScript::sign
		IL_19b6: ldc.i4.0
		IL_19b7: ceq
		IL_19b9: brtrue IL_1c7d

		IL_19be: nop
		IL_19bf: ldarg.0
		IL_19c0: ldfld int32 MathGameScript::sign
		IL_19c5: ldc.i4.1
		IL_19c6: ceq
		IL_19c8: ldc.i4.0
		IL_19c9: ceq
		IL_19cb: brtrue IL_1f8a

		IL_19d0: nop
		IL_19d1: ldarg.0
		IL_19d2: ldfld float32 MathGameScript::num1
		IL_19d7: ldarg.0
		IL_19d8: ldfld float32 MathGameScript::num2
		IL_19dd: sub
		IL_19de: stloc.s 241
		IL_19e0: ldarg.0
		IL_19e1: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text MathGameScript::questionText
		IL_19e6: stloc.s 242
		IL_19e8: ldarg.0
		IL_19e9: ldloc.s 241
		IL_19eb: stfld float32 MathGameScript::solution
		IL_19f0: ldc.i4.7
		IL_19f1: newarr [mscorlib]System.Object
		IL_19f6: stloc.s 243
		IL_19f8: ldloc.s 243
		IL_19fa: stloc.s 40
		IL_19fc: ldloc.s 243
		IL_19fe: stloc.s 244
		IL_1a00: ldloc.s 242
		IL_1a02: stloc.s 42
		IL_1a04: ldstr "SOLVE MATH Q"
		IL_1a09: ldnull
		IL_1a0a: ceq
		IL_1a0c: brtrue.s IL_1a3e

		IL_1a0e: nop
		IL_1a0f: ldstr "sub_1A4130"
		IL_1a14: ldstr "SOLVE MATH Q"
		IL_1a19: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1a1e: ldstr "*(LocalExpr->klass + 0x40)"
		IL_1a23: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1a28: ldloc.s 239
		IL_1a2a: ldc.i8 0
		IL_1a33: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_1a38: stloc.s 245
		IL_1a3a: ldloc.s 245
		IL_1a3c: stloc.s 246

		IL_1a3e: nop
		IL_1a3f: ldloc.s 40
		IL_1a41: ldc.i4.0
		IL_1a42: ldstr "SOLVE MATH Q"
		IL_1a47: stelem [mscorlib]System.Object
		IL_1a4c: ldstr "*(BinaryExpr)"
		IL_1a51: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1a56: conv.i4
		IL_1a57: box [mscorlib]System.Int32
		IL_1a5c: stloc.s 247
		IL_1a5e: ldloc.s 247
		IL_1a60: stloc.s 46
		IL_1a62: ldloc.s 247
		IL_1a64: stloc.s 248
		IL_1a66: ldloc.s 247
		IL_1a68: ldnull
		IL_1a69: ceq
		IL_1a6b: brtrue.s IL_1a96

		IL_1a6d: nop
		IL_1a6e: ldstr "sub_1A4130"
		IL_1a73: ldloc.s 248
		IL_1a75: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1a7a: ldstr "*(LocalExpr->klass + 0x40)"
		IL_1a7f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1a84: ldloc.s 239
		IL_1a86: ldc.i8 0
		IL_1a8f: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_1a94: stloc.s 249

		IL_1a96: nop
		IL_1a97: ldloc.s 40
		IL_1a99: ldc.i4.1
		IL_1a9a: ldloc.s 46
		IL_1a9c: stelem [mscorlib]System.Object
		IL_1aa1: ldstr ": \n \n"
		IL_1aa6: ldnull
		IL_1aa7: ceq
		IL_1aa9: brtrue.s IL_1adb

		IL_1aab: nop
		IL_1aac: ldstr "sub_1A4130"
		IL_1ab1: ldstr ": \n \n"
		IL_1ab6: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1abb: ldstr "*(LocalExpr->klass + 0x40)"
		IL_1ac0: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1ac5: ldloc.s 239
		IL_1ac7: ldc.i8 0
		IL_1ad0: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_1ad5: stloc.s 250
		IL_1ad7: ldloc.s 250
		IL_1ad9: stloc.s 251

		IL_1adb: nop
		IL_1adc: ldloc.s 40
		IL_1ade: ldc.i4.2
		IL_1adf: ldstr ": \n \n"
		IL_1ae4: stelem [mscorlib]System.Object
		IL_1ae9: ldarg.0
		IL_1aea: ldfld float32 MathGameScript::num1
		IL_1aef: stloc.s 252
		IL_1af1: ldstr "*(BinaryExpr)"
		IL_1af6: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1afb: box [mscorlib]System.Int64
		IL_1b00: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_1b05: box [mscorlib]System.Single
		IL_1b0a: stloc.s 253
		IL_1b0c: ldloc.s 253
		IL_1b0e: stloc.s 46
		IL_1b10: ldloc.s 253
		IL_1b12: stloc.s 254
		IL_1b14: ldloc.s 253
		IL_1b16: ldnull
		IL_1b17: ceq
		IL_1b19: brtrue.s IL_1b44

		IL_1b1b: nop
		IL_1b1c: ldstr "sub_1A4130"
		IL_1b21: ldloc.s 254
		IL_1b23: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1b28: ldstr "*(LocalExpr->klass + 0x40)"
		IL_1b2d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1b32: ldloc.s 239
		IL_1b34: ldc.i8 0
		IL_1b3d: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_1b42: stloc.s 255

		IL_1b44: nop
		IL_1b45: ldloc.s 40
		IL_1b47: ldc.i4.3
		IL_1b48: ldloc.s 46
		IL_1b4a: stelem [mscorlib]System.Object
		IL_1b4f: ldstr "-"
		IL_1b54: ldnull
		IL_1b55: ceq
		IL_1b57: brtrue.s IL_1b8f

		IL_1b59: nop
		IL_1b5a: ldstr "sub_1A4130"
		IL_1b5f: ldstr "-"
		IL_1b64: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1b69: ldstr "*(LocalExpr->klass + 0x40)"
		IL_1b6e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1b73: ldloc.s 239
		IL_1b75: ldc.i8 0
		IL_1b7e: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_1b83: stloc 256
		IL_1b87: ldloc 256
		IL_1b8b: stloc 257

		IL_1b8f: nop
		IL_1b90: ldloc.s 40
		IL_1b92: ldc.i4.4
		IL_1b93: ldstr "-"
		IL_1b98: stelem [mscorlib]System.Object
		IL_1b9d: ldarg.0
		IL_1b9e: ldfld float32 MathGameScript::num2
		IL_1ba3: stloc 258
		IL_1ba7: ldstr "*(BinaryExpr)"
		IL_1bac: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1bb1: box [mscorlib]System.Int64
		IL_1bb6: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_1bbb: box [mscorlib]System.Single
		IL_1bc0: stloc 259
		IL_1bc4: ldloc 259
		IL_1bc8: stloc.s 46
		IL_1bca: ldloc 259
		IL_1bce: stloc 260
		IL_1bd2: ldloc 259
		IL_1bd6: ldnull
		IL_1bd7: ceq
		IL_1bd9: brtrue.s IL_1c08

		IL_1bdb: nop
		IL_1bdc: ldstr "sub_1A4130"
		IL_1be1: ldloc 260
		IL_1be5: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1bea: ldstr "*(LocalExpr->klass + 0x40)"
		IL_1bef: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1bf4: ldloc.s 239
		IL_1bf6: ldc.i8 0
		IL_1bff: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_1c04: stloc 261

		IL_1c08: nop
		IL_1c09: ldloc.s 40
		IL_1c0b: ldc.i4.5
		IL_1c0c: ldloc.s 46
		IL_1c0e: stelem [mscorlib]System.Object
		IL_1c13: ldstr "="
		IL_1c18: ldnull
		IL_1c19: ceq
		IL_1c1b: brtrue.s IL_1c53

		IL_1c1d: nop
		IL_1c1e: ldstr "sub_1A4130"
		IL_1c23: ldstr "="
		IL_1c28: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1c2d: ldstr "*(LocalExpr->klass + 0x40)"
		IL_1c32: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1c37: ldloc.s 239
		IL_1c39: ldc.i8 0
		IL_1c42: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_1c47: stloc 262
		IL_1c4b: ldloc 262
		IL_1c4f: stloc 263

		IL_1c53: nop
		IL_1c54: ldloc.s 40
		IL_1c56: ldc.i4.6
		IL_1c57: ldstr "="
		IL_1c5c: stelem [mscorlib]System.Object
		IL_1c61: ldloc.s 40
		IL_1c63: call string [mscorlib]System.String::Concat(object[])
		IL_1c68: stloc 264
		IL_1c6c: ldarg.0
		IL_1c6d: ldarg.0
		IL_1c6e: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip MathGameScript::bal_minus
		IL_1c73: callvirt instance void MathGameScript::QueueAudio(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_1c78: br IL_1f8a

		IL_1c7d: nop
		IL_1c7e: ldarg.0
		IL_1c7f: ldfld float32 MathGameScript::num2
		IL_1c84: ldarg.0
		IL_1c85: ldfld float32 MathGameScript::num1
		IL_1c8a: add
		IL_1c8b: stloc 265
		IL_1c8f: ldarg.0
		IL_1c90: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text MathGameScript::questionText
		IL_1c95: stloc 266
		IL_1c99: ldarg.0
		IL_1c9a: ldloc 265
		IL_1c9e: stfld float32 MathGameScript::solution
		IL_1ca3: ldc.i4.7
		IL_1ca4: newarr [mscorlib]System.Object
		IL_1ca9: stloc 267
		IL_1cad: ldloc 267
		IL_1cb1: stloc.s 40
		IL_1cb3: ldloc 267
		IL_1cb7: stloc 268
		IL_1cbb: ldloc 266
		IL_1cbf: stloc.s 42
		IL_1cc1: ldstr "SOLVE MATH Q"
		IL_1cc6: ldnull
		IL_1cc7: ceq
		IL_1cc9: brtrue.s IL_1d01

		IL_1ccb: nop
		IL_1ccc: ldstr "sub_1A4130"
		IL_1cd1: ldstr "SOLVE MATH Q"
		IL_1cd6: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1cdb: ldstr "*(LocalExpr->klass + 0x40)"
		IL_1ce0: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1ce5: ldloc.s 239
		IL_1ce7: ldc.i8 0
		IL_1cf0: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_1cf5: stloc 269
		IL_1cf9: ldloc 269
		IL_1cfd: stloc 270

		IL_1d01: nop
		IL_1d02: ldloc.s 40
		IL_1d04: ldc.i4.0
		IL_1d05: ldstr "SOLVE MATH Q"
		IL_1d0a: stelem [mscorlib]System.Object
		IL_1d0f: ldstr "*(BinaryExpr)"
		IL_1d14: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1d19: conv.i4
		IL_1d1a: box [mscorlib]System.Int32
		IL_1d1f: stloc 271
		IL_1d23: ldloc 271
		IL_1d27: stloc.s 46
		IL_1d29: ldloc 271
		IL_1d2d: stloc 272
		IL_1d31: ldloc 271
		IL_1d35: ldnull
		IL_1d36: ceq
		IL_1d38: brtrue.s IL_1d67

		IL_1d3a: nop
		IL_1d3b: ldstr "sub_1A4130"
		IL_1d40: ldloc 272
		IL_1d44: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1d49: ldstr "*(LocalExpr->klass + 0x40)"
		IL_1d4e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1d53: ldloc.s 239
		IL_1d55: ldc.i8 0
		IL_1d5e: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_1d63: stloc 273

		IL_1d67: nop
		IL_1d68: ldloc.s 40
		IL_1d6a: ldc.i4.1
		IL_1d6b: ldloc.s 46
		IL_1d6d: stelem [mscorlib]System.Object
		IL_1d72: ldstr ": \n \n"
		IL_1d77: ldnull
		IL_1d78: ceq
		IL_1d7a: brtrue.s IL_1db2

		IL_1d7c: nop
		IL_1d7d: ldstr "sub_1A4130"
		IL_1d82: ldstr ": \n \n"
		IL_1d87: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1d8c: ldstr "*(LocalExpr->klass + 0x40)"
		IL_1d91: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1d96: ldloc.s 239
		IL_1d98: ldc.i8 0
		IL_1da1: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_1da6: stloc 274
		IL_1daa: ldloc 274
		IL_1dae: stloc 275

		IL_1db2: nop
		IL_1db3: ldloc.s 40
		IL_1db5: ldc.i4.2
		IL_1db6: ldstr ": \n \n"
		IL_1dbb: stelem [mscorlib]System.Object
		IL_1dc0: ldarg.0
		IL_1dc1: ldfld float32 MathGameScript::num1
		IL_1dc6: stloc 276
		IL_1dca: ldstr "*(BinaryExpr)"
		IL_1dcf: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1dd4: box [mscorlib]System.Int64
		IL_1dd9: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_1dde: box [mscorlib]System.Single
		IL_1de3: stloc 277
		IL_1de7: ldloc 277
		IL_1deb: stloc.s 46
		IL_1ded: ldloc 277
		IL_1df1: stloc 278
		IL_1df5: ldloc 277
		IL_1df9: ldnull
		IL_1dfa: ceq
		IL_1dfc: brtrue.s IL_1e2b

		IL_1dfe: nop
		IL_1dff: ldstr "sub_1A4130"
		IL_1e04: ldloc 278
		IL_1e08: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1e0d: ldstr "*(LocalExpr->klass + 0x40)"
		IL_1e12: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1e17: ldloc.s 239
		IL_1e19: ldc.i8 0
		IL_1e22: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_1e27: stloc 279

		IL_1e2b: nop
		IL_1e2c: ldloc.s 40
		IL_1e2e: ldc.i4.3
		IL_1e2f: ldloc.s 46
		IL_1e31: stelem [mscorlib]System.Object
		IL_1e36: ldstr "+"
		IL_1e3b: ldnull
		IL_1e3c: ceq
		IL_1e3e: brtrue.s IL_1e76

		IL_1e40: nop
		IL_1e41: ldstr "sub_1A4130"
		IL_1e46: ldstr "+"
		IL_1e4b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1e50: ldstr "*(LocalExpr->klass + 0x40)"
		IL_1e55: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1e5a: ldloc.s 239
		IL_1e5c: ldc.i8 0
		IL_1e65: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_1e6a: stloc 280
		IL_1e6e: ldloc 280
		IL_1e72: stloc 281

		IL_1e76: nop
		IL_1e77: ldloc.s 40
		IL_1e79: ldc.i4.4
		IL_1e7a: ldstr "+"
		IL_1e7f: stelem [mscorlib]System.Object
		IL_1e84: ldarg.0
		IL_1e85: ldfld float32 MathGameScript::num2
		IL_1e8a: stloc 282
		IL_1e8e: ldstr "*(BinaryExpr)"
		IL_1e93: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1e98: box [mscorlib]System.Int64
		IL_1e9d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_1ea2: box [mscorlib]System.Single
		IL_1ea7: stloc 283
		IL_1eab: ldloc 283
		IL_1eaf: stloc.s 46
		IL_1eb1: ldloc 283
		IL_1eb5: stloc 284
		IL_1eb9: ldloc 283
		IL_1ebd: ldnull
		IL_1ebe: ceq
		IL_1ec0: brtrue.s IL_1eef

		IL_1ec2: nop
		IL_1ec3: ldstr "sub_1A4130"
		IL_1ec8: ldloc 284
		IL_1ecc: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1ed1: ldstr "*(LocalExpr->klass + 0x40)"
		IL_1ed6: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1edb: ldloc.s 239
		IL_1edd: ldc.i8 0
		IL_1ee6: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_1eeb: stloc 285

		IL_1eef: nop
		IL_1ef0: ldloc.s 40
		IL_1ef2: ldc.i4.5
		IL_1ef3: ldloc.s 46
		IL_1ef5: stelem [mscorlib]System.Object
		IL_1efa: ldstr "="
		IL_1eff: ldnull
		IL_1f00: ceq
		IL_1f02: brtrue.s IL_1f3a

		IL_1f04: nop
		IL_1f05: ldstr "sub_1A4130"
		IL_1f0a: ldstr "="
		IL_1f0f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_1f14: ldstr "*(LocalExpr->klass + 0x40)"
		IL_1f19: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_1f1e: ldloc.s 239
		IL_1f20: ldc.i8 0
		IL_1f29: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_1f2e: stloc 286
		IL_1f32: ldloc 286
		IL_1f36: stloc 287

		IL_1f3a: nop
		IL_1f3b: ldloc.s 40
		IL_1f3d: ldc.i4.6
		IL_1f3e: ldstr "="
		IL_1f43: stelem [mscorlib]System.Object
		IL_1f48: ldloc.s 40
		IL_1f4a: call string [mscorlib]System.String::Concat(object[])
		IL_1f4f: stloc 288
		IL_1f53: ldarg.0
		IL_1f54: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] MathGameScript::audioQueue
		IL_1f59: stloc.s 238
		IL_1f5b: ldarg.0
		IL_1f5c: ldfld int32 MathGameScript::audioInQueue
		IL_1f61: conv.i8
		IL_1f62: stloc.s 239
		IL_1f64: ldarg.0
		IL_1f65: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip MathGameScript::bal_plus
		IL_1f6a: stloc 289
		IL_1f6e: ldloc.s 238
		IL_1f70: ldloc.s 239
		IL_1f72: conv.i4
		IL_1f73: ldloc 289
		IL_1f77: stelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_1f7c: ldarg.0
		IL_1f7d: ldarg.0
		IL_1f7e: ldfld int32 MathGameScript::audioInQueue
		IL_1f83: ldc.i4.1
		IL_1f84: add
		IL_1f85: stfld int32 MathGameScript::audioInQueue

		IL_1f8a: nop
		IL_1f8b: ldarg.0
		IL_1f8c: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] MathGameScript::bal_numbers
		IL_1f91: stloc 290
		IL_1f95: ldarg.0
		IL_1f96: ldfld float32 MathGameScript::num2
		IL_1f9b: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_1fa0: stloc 291
		IL_1fa4: ldloc 291
		IL_1fa8: stloc 292
		IL_1fac: ldarg.0
		IL_1fad: ldfld int32 MathGameScript::audioInQueue
		IL_1fb2: conv.i8
		IL_1fb3: stloc 293
		IL_1fb7: ldarg.0
		IL_1fb8: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] MathGameScript::audioQueue
		IL_1fbd: stloc 294
		IL_1fc1: ldloc 290
		IL_1fc5: ldloc 292
		IL_1fc9: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_1fce: stloc 295
		IL_1fd2: ldloc 294
		IL_1fd6: ldloc 293
		IL_1fda: conv.i4
		IL_1fdb: ldloc 295
		IL_1fdf: stelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_1fe4: ldarg.0
		IL_1fe5: ldfld int32 MathGameScript::audioInQueue
		IL_1fea: conv.i8
		IL_1feb: stloc 296
		IL_1fef: ldarg.0
		IL_1ff0: ldfld int32 MathGameScript::audioInQueue
		IL_1ff5: conv.i8
		IL_1ff6: ldc.i8 1
		IL_1fff: add
		IL_2000: stloc 297
		IL_2004: ldarg.0
		IL_2005: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] MathGameScript::audioQueue
		IL_200a: stloc 298
		IL_200e: ldarg.0
		IL_200f: ldloc 297
		IL_2013: conv.i4
		IL_2014: stfld int32 MathGameScript::audioInQueue
		IL_2019: ldloc 296
		IL_201d: stloc 299
		IL_2021: ldarg.0
		IL_2022: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip MathGameScript::bal_equals
		IL_2027: stloc 300
		IL_202b: ldstr "mov [rcx+rax*8+28h],rdx ; value = LocalExpr"
		IL_2030: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_2035: ldarg.0
		IL_2036: ldarg.0
		IL_2037: ldfld int32 MathGameScript::audioInQueue
		IL_203c: ldc.i4.1
		IL_203d: add
		IL_203e: stfld int32 MathGameScript::audioInQueue
		IL_2043: ldarg.0
		IL_2044: ldc.i4.1
		IL_2045: stfld bool MathGameScript::questionInProgress
		IL_204a: br IL_0108

		IL_204f: nop
		IL_2050: ldnull
		IL_2051: throw

		IL_2052: nop
		IL_2053: ldnull
		IL_2054: throw

		IL_2055: nop
		IL_2056: ldnull
		IL_2057: throw

		IL_2058: nop
		IL_2059: ldnull
		IL_205a: throw

		IL_205b: nop
		IL_205c: ldnull
		IL_205d: throw

		IL_205e: nop
		IL_205f: ldnull
		IL_2060: throw

		IL_2061: nop
		IL_2062: ldnull
		IL_2063: throw

		IL_2064: nop
		IL_2065: ldnull
		IL_2066: throw

		IL_2067: nop
		IL_2068: ldnull
		IL_2069: throw

		IL_206a: nop
		IL_206b: ldnull
		IL_206c: throw

		IL_206d: nop
		IL_206e: ldnull
		IL_206f: throw

		IL_2070: nop
		IL_2071: ldnull
		IL_2072: throw

		IL_2073: nop
		IL_2074: ldnull
		IL_2075: throw

		IL_2076: nop
		IL_2077: ldnull
		IL_2078: throw

		IL_2079: nop
		IL_207a: ldnull
		IL_207b: throw

		IL_207c: nop
		IL_207d: ldnull
		IL_207e: throw

		IL_207f: nop
		IL_2080: ldnull
		IL_2081: throw

		IL_2082: nop
		IL_2083: ldnull
		IL_2084: throw

		IL_2085: nop
		IL_2086: ldnull
		IL_2087: throw

		IL_2088: nop
		IL_2089: ldnull
		IL_208a: throw

		IL_208b: nop
		IL_208c: ldnull
		IL_208d: throw

		IL_208e: nop
		IL_208f: ldnull
		IL_2090: throw

		IL_2091: nop
		IL_2092: ldnull
		IL_2093: throw

		IL_2094: nop
		IL_2095: ldnull
		IL_2096: throw

		IL_2097: nop
		IL_2098: ldnull
		IL_2099: throw

		IL_209a: nop
		IL_209b: ldnull
		IL_209c: throw

		IL_209d: nop
		IL_209e: ldnull
		IL_209f: throw

		IL_20a0: nop
		IL_20a1: ldnull
		IL_20a2: throw

		IL_20a3: nop
		IL_20a4: ldnull
		IL_20a5: throw

		IL_20a6: nop
		IL_20a7: ldnull
		IL_20a8: throw

		IL_20a9: nop
		IL_20aa: ldnull
		IL_20ab: throw

		IL_20ac: nop
		IL_20ad: ldnull
		IL_20ae: throw

		IL_20af: nop
		IL_20b0: ldnull
		IL_20b1: throw

		IL_20b2: nop
		IL_20b3: ldnull
		IL_20b4: throw

		IL_20b5: nop
		IL_20b6: ldnull
		IL_20b7: throw

		IL_20b8: nop
		IL_20b9: ldnull
		IL_20ba: throw

		IL_20bb: nop
		IL_20bc: ldnull
		IL_20bd: throw

		IL_20be: nop
		IL_20bf: ldnull
		IL_20c0: throw

		IL_20c1: nop
		IL_20c2: ldnull
		IL_20c3: throw

		IL_20c4: nop
		IL_20c5: ldnull
		IL_20c6: throw

		IL_20c7: nop
		IL_20c8: ldnull
		IL_20c9: throw

		IL_20ca: nop
		IL_20cb: ldnull
		IL_20cc: throw

		IL_20cd: nop
		IL_20ce: ldnull
		IL_20cf: throw

		IL_20d0: nop
		IL_20d1: ldnull
		IL_20d2: throw

		IL_20d3: nop
		IL_20d4: ldnull
		IL_20d5: throw

		IL_20d6: nop
		IL_20d7: ldnull
		IL_20d8: throw

		IL_20d9: nop
		IL_20da: ldnull
		IL_20db: throw

		IL_20dc: nop
		IL_20dd: ldnull
		IL_20de: throw

		IL_20df: nop
		IL_20e0: ldnull
		IL_20e1: throw

		IL_20e2: nop
		IL_20e3: ldnull
		IL_20e4: throw

		IL_20e5: nop
		IL_20e6: ldnull
		IL_20e7: throw

		IL_20e8: nop
		IL_20e9: ldnull
		IL_20ea: throw

		IL_20eb: nop
		IL_20ec: ldnull
		IL_20ed: throw

		IL_20ee: nop
		IL_20ef: ldnull
		IL_20f0: throw

		IL_20f1: nop
		IL_20f2: ldnull
		IL_20f3: throw

		IL_20f4: nop
		IL_20f5: ldnull
		IL_20f6: throw

		IL_20f7: nop
		IL_20f8: ldnull
		IL_20f9: throw

		IL_20fa: nop
		IL_20fb: ldnull
		IL_20fc: throw

		IL_20fd: nop
		IL_20fe: ldnull
		IL_20ff: throw

		IL_2100: nop
		IL_2101: ldnull
		IL_2102: throw

		IL_2103: nop
		IL_2104: ldnull
		IL_2105: throw

		IL_2106: nop
		IL_2107: ldnull
		IL_2108: throw

		IL_2109: nop
		IL_210a: ldnull
		IL_210b: throw

		IL_210c: nop
		IL_210d: ldnull
		IL_210e: throw

		IL_210f: nop
		IL_2110: ldnull
		IL_2111: throw

		IL_2112: nop
		IL_2113: ldnull
		IL_2114: throw

		IL_2115: nop
		IL_2116: ldnull
		IL_2117: throw

		IL_2118: nop
		IL_2119: ldnull
		IL_211a: throw

		IL_211b: nop
		IL_211c: ldnull
		IL_211d: throw

		IL_211e: nop
		IL_211f: ldnull
		IL_2120: throw

		IL_2121: nop
		IL_2122: ldnull
		IL_2123: throw

		IL_2124: nop
		IL_2125: ldnull
		IL_2126: throw

		IL_2127: nop
		IL_2128: ldnull
		IL_2129: throw

		IL_212a: nop
		IL_212b: ldnull
		IL_212c: throw

		IL_212d: nop
		IL_212e: ldnull
		IL_212f: throw

		IL_2130: nop
		IL_2131: ldnull
		IL_2132: throw

		IL_2133: nop
		IL_2134: ldnull
		IL_2135: throw

		IL_2136: nop
		IL_2137: ldnull
		IL_2138: throw

		IL_2139: nop
		IL_213a: ldnull
		IL_213b: throw

		IL_213c: nop
		IL_213d: ldnull
		IL_213e: throw

		IL_213f: nop
		IL_2140: ldnull
		IL_2141: throw

		IL_2142: nop
		IL_2143: ldnull
		IL_2144: throw

		IL_2145: nop
		IL_2146: ldnull
		IL_2147: throw

		IL_2148: nop
		IL_2149: ldnull
		IL_214a: throw

		IL_214b: nop
		IL_214c: ldnull
		IL_214d: throw

		IL_214e: nop
		IL_214f: ldnull
		IL_2150: throw

		IL_2151: nop
		IL_2152: ldnull
		IL_2153: throw
	} // end of method MathGameScript::NewProblem

	.method public hidebysig 
		instance void OKButton () cil managed 
	{
		// Method begins at RVA 0x2252
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void MathGameScript::CheckAnswer()
		IL_0007: ret
	} // end of method MathGameScript::OKButton

	.method public hidebysig 
		instance void CheckAnswer () cil managed 
	{
		// Method begins at RVA 0x851c
		// Header size: 12
		// Code size: 963 (0x3c3)
		.maxstack 3
		.locals init (
			[0] bool,
			[1] bool,
			[2] bool,
			[3] class [mscorlib]System.Collections.IEnumerator,
			[4] class [UnityEngine.CoreModule]UnityEngine.Coroutine,
			[5] class GameControllerScript,
			[6] class PlayerScript,
			[7] class PlayerScript,
			[8] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[9] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[10] class CameraScript,
			[11] class CameraScript,
			[12] class [mscorlib]System.Collections.IEnumerator,
			[13] class [UnityEngine.CoreModule]UnityEngine.Coroutine,
			[14] class [UnityEngine.CoreModule]UnityEngine.AsyncOperation,
			[15] class [UnityEngine.CoreModule]UnityEngine.AsyncOperation,
			[16] string,
			[17] string,
			[18] bool,
			[19] int32,
			[20] class [UnityEngine.UI]UnityEngine.UI.RawImage[],
			[21] class GameControllerScript,
			[22] bool,
			[23] class BaldiScript,
			[24] float32,
			[25] float32,
			[26] float32,
			[27] float32,
			[28] float32,
			[29] class [UnityEngine.AudioModule]UnityEngine.AudioSource,
			[30] float32,
			[31] int32,
			[32] int64,
			[33] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[34] class [UnityEngine.AudioModule]UnityEngine.AudioClip
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [Unity.TextMeshPro]TMPro.TMP_InputField MathGameScript::playerAnswer
		IL_0007: ldfld string [Unity.TextMeshPro]TMPro.TMP_InputField::m_Text
		IL_000c: ldstr "31718"
		IL_0011: call bool [mscorlib]System.String::Equals(string, string)
		IL_0016: stloc.0
		IL_0017: ldloc.0
		IL_0018: ldc.i4.0
		IL_0019: ceq
		IL_001b: ldc.i4.0
		IL_001c: ceq
		IL_001e: brtrue IL_011b

		IL_0023: nop
		IL_0024: ldarg.0
		IL_0025: ldfld class [Unity.TextMeshPro]TMPro.TMP_InputField MathGameScript::playerAnswer
		IL_002a: ldfld string [Unity.TextMeshPro]TMPro.TMP_InputField::m_Text
		IL_002f: ldstr "53045009"
		IL_0034: call bool [mscorlib]System.String::Equals(string, string)
		IL_0039: stloc.1
		IL_003a: ldloc.1
		IL_003b: stloc.2
		IL_003c: ldloc.1
		IL_003d: ldc.i4.0
		IL_003e: ceq
		IL_0040: brtrue IL_0147

		IL_0045: nop
		IL_0046: ldarg.0
		IL_0047: ldstr "USE THESE TO STICK TO THE CEILING!"
		IL_004c: callvirt instance class [mscorlib]System.Collections.IEnumerator MathGameScript::CheatText(string)
		IL_0051: stloc.3
		IL_0052: ldarg.0
		IL_0053: ldloc.3
		IL_0054: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Coroutine [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StartCoroutine(class [mscorlib]System.Collections.IEnumerator)
		IL_0059: stloc.s 4
		IL_005b: ldarg.0
		IL_005c: ldfld class GameControllerScript MathGameScript::gc
		IL_0061: stloc.s 5
		IL_0063: ldstr "global_19E3B11"
		IL_0068: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_006d: ldc.i4.0
		IL_006e: conv.i8
		IL_006f: ceq
		IL_0071: ldc.i4.0
		IL_0072: ceq
		IL_0074: brtrue.s IL_0081

		IL_0076: nop
		IL_0077: ldstr "mov byte ptr [1819E3B11h],1 ; value = 0x1"
		IL_007c: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)

		IL_0081: nop
		IL_0082: ldloc.s 5
		IL_0084: ldfld class PlayerScript GameControllerScript::player
		IL_0089: stloc.s 6
		IL_008b: ldloc.s 5
		IL_008d: ldc.i4.1
		IL_008e: stfld bool GameControllerScript::flipped
		IL_0093: ldloc.s 6
		IL_0095: stloc.s 7
		IL_0097: ldloc.s 7
		IL_0099: ldc.r4 6
		IL_009e: stfld float32 PlayerScript::height
		IL_00a3: ldloc.s 5
		IL_00a5: ldfld class PlayerScript GameControllerScript::player
		IL_00aa: stloc.s 7
		IL_00ac: ldloc.s 7
		IL_00ae: ldc.r4 180
		IL_00b3: stfld float32 PlayerScript::fliparoo
		IL_00b8: ldloc.s 5
		IL_00ba: ldfld class PlayerScript GameControllerScript::player
		IL_00bf: stloc.s 7
		IL_00c1: ldloc.s 7
		IL_00c3: ldc.r4 -1
		IL_00c8: stfld float32 PlayerScript::flipaturn
		IL_00cd: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_00d2: stloc.s 8
		IL_00d4: ldloc.s 8
		IL_00d6: stloc.s 9
		IL_00d8: ldloc.s 9
		IL_00da: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class CameraScript>()
		IL_00df: stloc.s 10
		IL_00e1: ldloc.s 10
		IL_00e3: stloc.s 11
		IL_00e5: ldloc.s 11
		IL_00e7: ldc.i4.0
		IL_00e8: box [mscorlib]System.Int32
		IL_00ed: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_00f2: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::offset
		IL_00f7: ldloc.s 11
		IL_00f9: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::offset
		IL_00fe: ldc.r4 -1
		IL_0103: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0108: ldloc.s 11
		IL_010a: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::offset
		IL_010f: ldc.r4 0.0
		IL_0114: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0119: br.s IL_0147

		IL_011b: nop
		IL_011c: ldarg.0
		IL_011d: ldstr "THIS IS WHERE IT ALL BEGAN"
		IL_0122: callvirt instance class [mscorlib]System.Collections.IEnumerator MathGameScript::CheatText(string)
		IL_0127: stloc.s 12
		IL_0129: ldarg.0
		IL_012a: ldloc.s 12
		IL_012c: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Coroutine [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StartCoroutine(class [mscorlib]System.Collections.IEnumerator)
		IL_0131: stloc.s 13
		IL_0133: br.s IL_0136

		IL_0135: nop

		IL_0136: nop
		IL_0137: ldstr "TestRoom"
		IL_013c: call class [UnityEngine.CoreModule]UnityEngine.AsyncOperation [UnityEngine.CoreModule]UnityEngine.SceneManagement.SceneManager::LoadSceneAsync(string)
		IL_0141: stloc.s 14
		IL_0143: ldloc.s 14
		IL_0145: stloc.s 15

		IL_0147: nop
		IL_0148: ldarg.0
		IL_0149: ldfld int32 MathGameScript::problem
		IL_014e: ldc.i4.3
		IL_014f: cgt
		IL_0151: brtrue IL_032b

		IL_0156: nop
		IL_0157: ldarg.0
		IL_0158: ldfld class [Unity.TextMeshPro]TMPro.TMP_InputField MathGameScript::playerAnswer
		IL_015d: ldfld string [Unity.TextMeshPro]TMPro.TMP_InputField::m_Text
		IL_0162: stloc.s 16
		IL_0164: ldarg.0
		IL_0165: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_016a: ldc.i8 268
		IL_0173: add
		IL_0174: call instance string [mscorlib]System.Single::ToString()
		IL_0179: stloc.s 17
		IL_017b: ldloc.s 16
		IL_017d: ldloc.s 17
		IL_017f: call bool [mscorlib]System.String::Equals(string, string)
		IL_0184: stloc.s 18
		IL_0186: ldarg.0
		IL_0187: ldfld bool MathGameScript::impossibleMode
		IL_018c: ldc.i4.0
		IL_018d: ceq
		IL_018f: brfalse.s IL_0197

		IL_0191: ldloc.s 18
		IL_0193: stloc.s 19
		IL_0195: br.s IL_019b

		IL_0197: nop
		IL_0198: ldc.i4.0
		IL_0199: stloc.s 19

		IL_019b: nop
		IL_019c: ldarg.0
		IL_019d: ldfld class [UnityEngine.UI]UnityEngine.UI.RawImage[] MathGameScript::results
		IL_01a2: stloc.s 20
		IL_01a4: ldloc.s 19
		IL_01a6: ldc.i4.0
		IL_01a7: ceq
		IL_01a9: ldc.i4.0
		IL_01aa: ceq
		IL_01ac: brtrue IL_032d

		IL_01b1: nop
		IL_01b2: ldarg.0
		IL_01b3: ldarg.0
		IL_01b4: ldfld int32 MathGameScript::problemsWrong
		IL_01b9: ldc.i4.1
		IL_01ba: add
		IL_01bb: stfld int32 MathGameScript::problemsWrong
		IL_01c0: ldloc.s 20
		IL_01c2: ldarg.0
		IL_01c3: ldfld int32 MathGameScript::problem
		IL_01c8: conv.i8
		IL_01c9: ldc.i8 1
		IL_01d2: sub
		IL_01d3: conv.i4
		IL_01d4: ldelem [UnityEngine.UI]UnityEngine.UI.RawImage
		IL_01d9: ldarg.0
		IL_01da: ldfld class [UnityEngine.CoreModule]UnityEngine.Texture MathGameScript::incorrect
		IL_01df: callvirt instance void [UnityEngine.UI]UnityEngine.UI.RawImage::set_texture(class [UnityEngine.CoreModule]UnityEngine.Texture)
		IL_01e4: ldarg.0
		IL_01e5: ldfld class GameControllerScript MathGameScript::gc
		IL_01ea: ldfld bool GameControllerScript::spoopMode
		IL_01ef: ldc.i4.0
		IL_01f0: ceq
		IL_01f2: ldc.i4.0
		IL_01f3: ceq
		IL_01f5: brtrue.s IL_0213

		IL_01f7: nop
		IL_01f8: ldarg.0
		IL_01f9: ldfld class [UnityEngine.AnimationModule]UnityEngine.Animator MathGameScript::baldiFeed
		IL_01fe: ldstr "angry"
		IL_0203: callvirt instance void [UnityEngine.AnimationModule]UnityEngine.Animator::SetTrigger(string)
		IL_0208: ldarg.0
		IL_0209: ldfld class GameControllerScript MathGameScript::gc
		IL_020e: callvirt instance void GameControllerScript::ActivateSpoopMode()

		IL_0213: nop
		IL_0214: ldarg.0
		IL_0215: ldfld class GameControllerScript MathGameScript::gc
		IL_021a: stloc.s 21
		IL_021c: ldloc.s 21
		IL_021e: ldfld string GameControllerScript::mode
		IL_0223: ldstr "story"
		IL_0228: call bool [mscorlib]System.String::Equals(string, string)
		IL_022d: stloc.s 22
		IL_022f: ldarg.0
		IL_0230: ldfld class BaldiScript MathGameScript::baldiScript
		IL_0235: stloc.s 23
		IL_0237: ldloc.s 22
		IL_0239: ldc.i4.0
		IL_023a: ceq
		IL_023c: ldc.i4.0
		IL_023d: ceq
		IL_023f: brtrue IL_02d5

		IL_0244: nop
		IL_0245: ldloc.s 23
		IL_0247: ldloc.s 23
		IL_0249: ldfld float32 BaldiScript::baldiAnger
		IL_024e: ldc.r4 1
		IL_0253: add
		IL_0254: stfld float32 BaldiScript::baldiAnger
		IL_0259: ldc.r4 0.5
		IL_025e: ldloc.s 23
		IL_0260: ldfld float32 BaldiScript::baldiAnger
		IL_0265: ldc.r4 1
		IL_026a: add
		IL_026b: cgt
		IL_026d: ldc.i4.0
		IL_026e: ceq
		IL_0270: brtrue.s IL_027f

		IL_0272: nop
		IL_0273: ldloc.s 23
		IL_0275: ldc.r4 0.5
		IL_027a: stfld float32 BaldiScript::baldiAnger

		IL_027f: nop
		IL_0280: ldc.r4 2
		IL_0285: ldloc.s 23
		IL_0287: ldfld float32 BaldiScript::baldiSpeedScale
		IL_028c: div
		IL_028d: ldloc.s 23
		IL_028f: ldfld float32 BaldiScript::baldiAnger
		IL_0294: add
		IL_0295: stloc.s 24
		IL_0297: ldloc.s 23
		IL_0299: ldfld float32 BaldiScript::baldiAnger
		IL_029e: ldc.r4 -3
		IL_02a3: mul
		IL_02a4: ldc.r4 2
		IL_02a9: ldloc.s 23
		IL_02ab: ldfld float32 BaldiScript::baldiSpeedScale
		IL_02b0: div
		IL_02b1: ldloc.s 23
		IL_02b3: ldfld float32 BaldiScript::baldiAnger
		IL_02b8: add
		IL_02b9: div
		IL_02ba: ldc.r4 3
		IL_02bf: add
		IL_02c0: stloc.s 25
		IL_02c2: ldloc.s 23
		IL_02c4: ldloc.s 25
		IL_02c6: stfld float32 BaldiScript::baldiWait
		IL_02cb: ldloc.s 24
		IL_02cd: stloc.s 26
		IL_02cf: ldloc.s 25
		IL_02d1: stloc.s 27
		IL_02d3: br.s IL_030d

		IL_02d5: nop
		IL_02d6: ldarg.0
		IL_02d7: ldfld int32 MathGameScript::problem
		IL_02dc: ldc.i4.3
		IL_02dd: ceq
		IL_02df: brtrue.s IL_0300

		IL_02e1: nop
		IL_02e2: ldloc.s 23
		IL_02e4: ldfld float32 BaldiScript::baldiTempAnger
		IL_02e9: ldc.r4 0.25
		IL_02ee: add
		IL_02ef: stloc.s 28
		IL_02f1: ldloc.s 23
		IL_02f3: ldloc.s 28
		IL_02f5: stfld float32 BaldiScript::baldiTempAnger
		IL_02fa: ldloc.s 28
		IL_02fc: stloc.s 26
		IL_02fe: br.s IL_030d

		IL_0300: nop
		IL_0301: ldloc.s 23
		IL_0303: ldc.r4 1
		IL_0308: callvirt instance void BaldiScript::GetAngry(float32)

		IL_030d: nop
		IL_030e: ldarg.0
		IL_030f: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource MathGameScript::baldiAudio
		IL_0314: stloc.s 29
		IL_0316: ldarg.0
		IL_0317: ldc.i4.0
		IL_0318: stfld int32 MathGameScript::audioInQueue
		IL_031d: ldloc.s 29
		IL_031f: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Stop()

		IL_0324: nop
		IL_0325: ldarg.0
		IL_0326: callvirt instance void MathGameScript::NewProblem()

		IL_032b: nop
		IL_032c: ret

		IL_032d: nop
		IL_032e: ldloc.s 20
		IL_0330: ldarg.0
		IL_0331: ldfld int32 MathGameScript::problem
		IL_0336: conv.i8
		IL_0337: ldc.i8 1
		IL_0340: sub
		IL_0341: conv.i4
		IL_0342: ldelem [UnityEngine.UI]UnityEngine.UI.RawImage
		IL_0347: ldarg.0
		IL_0348: ldfld class [UnityEngine.CoreModule]UnityEngine.Texture MathGameScript::correct
		IL_034d: callvirt instance void [UnityEngine.UI]UnityEngine.UI.RawImage::set_texture(class [UnityEngine.CoreModule]UnityEngine.Texture)
		IL_0352: ldarg.0
		IL_0353: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource MathGameScript::baldiAudio
		IL_0358: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Stop()
		IL_035d: ldarg.0
		IL_035e: ldc.i4.0
		IL_035f: stfld int32 MathGameScript::audioInQueue
		IL_0364: ldc.r4 0.0
		IL_0369: ldc.r4 4
		IL_036e: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_0373: stloc.s 30
		IL_0375: ldloc.s 30
		IL_0377: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_037c: stloc.s 31
		IL_037e: ldarg.0
		IL_037f: ldfld int32 MathGameScript::audioInQueue
		IL_0384: conv.i8
		IL_0385: stloc.s 32
		IL_0387: ldarg.0
		IL_0388: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] MathGameScript::audioQueue
		IL_038d: stloc.s 33
		IL_038f: ldarg.0
		IL_0390: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] MathGameScript::bal_praises
		IL_0395: ldloc.s 31
		IL_0397: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_039c: stloc.s 34
		IL_039e: ldloc.s 33
		IL_03a0: ldloc.s 32
		IL_03a2: conv.i4
		IL_03a3: ldloc.s 34
		IL_03a5: stelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_03aa: ldarg.0
		IL_03ab: ldarg.0
		IL_03ac: ldfld int32 MathGameScript::audioInQueue
		IL_03b1: ldc.i4.1
		IL_03b2: add
		IL_03b3: stfld int32 MathGameScript::audioInQueue
		IL_03b8: br IL_0324

		IL_03bd: nop
		IL_03be: ldnull
		IL_03bf: throw

		IL_03c0: nop
		IL_03c1: ldnull
		IL_03c2: throw
	} // end of method MathGameScript::CheckAnswer

	.method private hidebysig 
		instance void QueueAudio (
			class [UnityEngine.AudioModule]UnityEngine.AudioClip sound
		) cil managed 
	{
		// Method begins at RVA 0x88ec
		// Header size: 12
		// Code size: 46 (0x2e)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[1] int64
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] MathGameScript::audioQueue
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld int32 MathGameScript::audioInQueue
		IL_000e: conv.i8
		IL_000f: stloc.1
		IL_0010: ldloc.0
		IL_0011: ldloc.1
		IL_0012: conv.i4
		IL_0013: ldarg.1
		IL_0014: stelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0019: ldarg.0
		IL_001a: ldarg.0
		IL_001b: ldfld int32 MathGameScript::audioInQueue
		IL_0020: ldc.i4.1
		IL_0021: add
		IL_0022: stfld int32 MathGameScript::audioInQueue
		IL_0027: ret

		IL_0028: nop
		IL_0029: ldnull
		IL_002a: throw

		IL_002b: nop
		IL_002c: ldnull
		IL_002d: throw
	} // end of method MathGameScript::QueueAudio

	.method private hidebysig 
		instance void PlayQueue () cil managed 
	{
		// Method begins at RVA 0x8928
		// Header size: 12
		// Code size: 121 (0x79)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.AudioModule]UnityEngine.AudioClip,
			[1] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[2] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource MathGameScript::baldiAudio
		IL_0007: ldarg.0
		IL_0008: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] MathGameScript::audioQueue
		IL_000d: ldc.i4.0
		IL_000e: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0013: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_0018: ldarg.0
		IL_0019: ldfld int32 MathGameScript::audioInQueue
		IL_001e: ldc.i4.1
		IL_001f: cgt
		IL_0021: ldc.i4.0
		IL_0022: ceq
		IL_0024: brtrue.s IL_0063

		IL_0026: nop
		// loop start (head: IL_0027)
			IL_0027: nop
			IL_0028: ldarg.0
			IL_0029: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] MathGameScript::audioQueue
			IL_002e: ldc.i4.1
			IL_002f: conv.i8
			IL_0030: conv.i4
			IL_0031: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
			IL_0036: stloc.0
			IL_0037: ldarg.0
			IL_0038: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] MathGameScript::audioQueue
			IL_003d: stloc.1
			IL_003e: ldloc.1
			IL_003f: ldc.i4.1
			IL_0040: conv.i8
			IL_0041: ldc.i8 1
			IL_004a: sub
			IL_004b: conv.i4
			IL_004c: ldloc.0
			IL_004d: stelem [UnityEngine.AudioModule]UnityEngine.AudioClip
			IL_0052: ldc.i4.1
			IL_0053: ldc.i4.1
			IL_0054: add
			IL_0055: stloc.2
			IL_0056: ldc.i4.1
			IL_0057: ldc.i4.1
			IL_0058: add
			IL_0059: ldarg.0
			IL_005a: ldfld int32 MathGameScript::audioInQueue
			IL_005f: clt
			IL_0061: brtrue.s IL_0027
		// end loop

		IL_0063: nop
		IL_0064: ldarg.0
		IL_0065: ldarg.0
		IL_0066: ldfld int32 MathGameScript::audioInQueue
		IL_006b: ldc.i4.1
		IL_006c: sub
		IL_006d: stfld int32 MathGameScript::audioInQueue
		IL_0072: ret

		IL_0073: nop
		IL_0074: ldnull
		IL_0075: throw

		IL_0076: nop
		IL_0077: ldnull
		IL_0078: throw
	} // end of method MathGameScript::PlayQueue

	.method private hidebysig 
		instance void UnqueueAudio () cil managed 
	{
		// Method begins at RVA 0x89b0
		// Header size: 12
		// Code size: 93 (0x5d)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[1] class [UnityEngine.AudioModule]UnityEngine.AudioClip,
			[2] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld int32 MathGameScript::audioInQueue
		IL_0007: ldc.i4.1
		IL_0008: cgt
		IL_000a: ldc.i4.0
		IL_000b: ceq
		IL_000d: brtrue.s IL_0047

		IL_000f: nop
		// loop start (head: IL_0010)
			IL_0010: nop
			IL_0011: ldarg.0
			IL_0012: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] MathGameScript::audioQueue
			IL_0017: stloc.0
			IL_0018: ldloc.0
			IL_0019: ldc.i4.1
			IL_001a: conv.i8
			IL_001b: conv.i4
			IL_001c: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
			IL_0021: stloc.1
			IL_0022: ldloc.0
			IL_0023: ldc.i4.1
			IL_0024: conv.i8
			IL_0025: ldc.i8 1
			IL_002e: sub
			IL_002f: conv.i4
			IL_0030: ldloc.1
			IL_0031: stelem [UnityEngine.AudioModule]UnityEngine.AudioClip
			IL_0036: ldc.i4.1
			IL_0037: ldc.i4.1
			IL_0038: add
			IL_0039: stloc.2
			IL_003a: ldc.i4.1
			IL_003b: ldc.i4.1
			IL_003c: add
			IL_003d: ldarg.0
			IL_003e: ldfld int32 MathGameScript::audioInQueue
			IL_0043: clt
			IL_0045: brtrue.s IL_0010
		// end loop

		IL_0047: nop
		IL_0048: ldarg.0
		IL_0049: ldarg.0
		IL_004a: ldfld int32 MathGameScript::audioInQueue
		IL_004f: ldc.i4.1
		IL_0050: sub
		IL_0051: stfld int32 MathGameScript::audioInQueue
		IL_0056: ret

		IL_0057: nop
		IL_0058: ldnull
		IL_0059: throw

		IL_005a: nop
		IL_005b: ldnull
		IL_005c: throw
	} // end of method MathGameScript::UnqueueAudio

	.method private hidebysig 
		instance void ClearAudioQueue () cil managed 
	{
		// Method begins at RVA 0x225b
		// Header size: 1
		// Code size: 9 (0x9)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i4.0
		IL_0003: stfld int32 MathGameScript::audioInQueue
		IL_0008: ret
	} // end of method MathGameScript::ClearAudioQueue

	.method private hidebysig 
		instance void ExitGame () cil managed 
	{
		// Method begins at RVA 0x8a1c
		// Header size: 12
		// Code size: 226 (0xe2)
		.maxstack 3
		.locals init (
			[0] int32,
			[1] bool,
			[2] int32,
			[3] class BaldiScript,
			[4] float32,
			[5] float32,
			[6] float32,
			[7] float32,
			[8] class GameControllerScript,
			[9] class [UnityEngine.CoreModule]UnityEngine.GameObject
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld int32 MathGameScript::problemsWrong
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld class GameControllerScript MathGameScript::gc
		IL_000e: ldfld string GameControllerScript::mode
		IL_0013: ldstr "endless"
		IL_0018: call bool [mscorlib]System.String::Equals(string, string)
		IL_001d: stloc.1
		IL_001e: ldloc.0
		IL_001f: ldc.i4.0
		IL_0020: cgt
		IL_0022: ldc.i4.0
		IL_0023: ceq
		IL_0025: brfalse.s IL_002b

		IL_0027: ldloc.1
		IL_0028: stloc.2
		IL_0029: br.s IL_002e

		IL_002b: nop
		IL_002c: ldc.i4.0
		IL_002d: stloc.2

		IL_002e: nop
		IL_002f: ldloc.2
		IL_0030: ldc.i4.0
		IL_0031: ceq
		IL_0033: brtrue IL_00c4

		IL_0038: nop
		IL_0039: ldarg.0
		IL_003a: ldfld class BaldiScript MathGameScript::baldiScript
		IL_003f: stloc.3
		IL_0040: ldloc.3
		IL_0041: ldloc.3
		IL_0042: ldfld float32 BaldiScript::baldiAnger
		IL_0047: ldc.r4 1
		IL_004c: sub
		IL_004d: stfld float32 BaldiScript::baldiAnger
		IL_0052: ldc.r4 0.5
		IL_0057: ldloc.3
		IL_0058: ldfld float32 BaldiScript::baldiAnger
		IL_005d: ldc.r4 1
		IL_0062: sub
		IL_0063: cgt
		IL_0065: ldc.i4.0
		IL_0066: ceq
		IL_0068: brtrue.s IL_0076

		IL_006a: nop
		IL_006b: ldloc.3
		IL_006c: ldc.r4 0.5
		IL_0071: stfld float32 BaldiScript::baldiAnger

		IL_0076: nop
		IL_0077: ldc.r4 2
		IL_007c: ldloc.3
		IL_007d: ldfld float32 BaldiScript::baldiSpeedScale
		IL_0082: div
		IL_0083: ldloc.3
		IL_0084: ldfld float32 BaldiScript::baldiAnger
		IL_0089: add
		IL_008a: stloc.s 4
		IL_008c: ldloc.3
		IL_008d: ldfld float32 BaldiScript::baldiAnger
		IL_0092: ldc.r4 -3
		IL_0097: mul
		IL_0098: ldc.r4 2
		IL_009d: ldloc.3
		IL_009e: ldfld float32 BaldiScript::baldiSpeedScale
		IL_00a3: div
		IL_00a4: ldloc.3
		IL_00a5: ldfld float32 BaldiScript::baldiAnger
		IL_00aa: add
		IL_00ab: div
		IL_00ac: ldc.r4 3
		IL_00b1: add
		IL_00b2: stloc.s 5
		IL_00b4: ldloc.3
		IL_00b5: ldloc.s 5
		IL_00b7: stfld float32 BaldiScript::baldiWait
		IL_00bc: ldloc.s 4
		IL_00be: stloc.s 6
		IL_00c0: ldloc.s 5
		IL_00c2: stloc.s 7

		IL_00c4: nop
		IL_00c5: ldarg.0
		IL_00c6: ldfld class GameControllerScript MathGameScript::gc
		IL_00cb: stloc.s 8
		IL_00cd: ldarg.0
		IL_00ce: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_00d3: stloc.s 9
		IL_00d5: ldloc.s 8
		IL_00d7: ldloc.s 9
		IL_00d9: callvirt instance void GameControllerScript::DeactivateLearningGame(class [UnityEngine.CoreModule]UnityEngine.GameObject)
		IL_00de: ret

		IL_00df: nop
		IL_00e0: ldnull
		IL_00e1: throw
	} // end of method MathGameScript::ExitGame

	.method public hidebysig 
		instance void ButtonPress (
			int32 'value'
		) cil managed 
	{
		// Method begins at RVA 0x8b0c
		// Header size: 12
		// Code size: 132 (0x84)
		.maxstack 3
		.locals init (
			[0] class [Unity.TextMeshPro]TMPro.TMP_InputField,
			[1] string,
			[2] string,
			[3] int32,
			[4] string,
			[5] string
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [Unity.TextMeshPro]TMPro.TMP_InputField MathGameScript::playerAnswer
		IL_0007: stloc.0
		IL_0008: ldarg.1
		IL_0009: ldc.i4.0
		IL_000a: clt
		IL_000c: ldc.i4.0
		IL_000d: ceq
		IL_000f: ldarg.1
		IL_0010: ldc.i4.s 9
		IL_0012: cgt
		IL_0014: ldc.i4.0
		IL_0015: ceq
		IL_0017: and
		IL_0018: ldc.i4.0
		IL_0019: ceq
		IL_001b: ldc.i4.0
		IL_001c: ceq
		IL_001e: brtrue.s IL_005d

		IL_0020: nop
		IL_0021: ldarg.1
		IL_0022: ldc.i4.m1
		IL_0023: ceq
		IL_0025: brtrue.s IL_0043

		IL_0027: nop
		IL_0028: ldloc.0
		IL_0029: ldstr "*(/*typedef#-1*/object.static_fields + 0x0)"
		IL_002e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0033: box [mscorlib]System.Int64
		IL_0038: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<string>(object)
		IL_003d: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_InputField::set_text(string)
		IL_0042: ret

		IL_0043: nop
		IL_0044: ldloc.0
		IL_0045: ldfld string [Unity.TextMeshPro]TMPro.TMP_InputField::m_Text
		IL_004a: ldstr "-"
		IL_004f: call string [mscorlib]System.String::Concat(string, string)
		IL_0054: stloc.1
		IL_0055: ldloc.0
		IL_0056: ldloc.1
		IL_0057: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_InputField::set_text(string)
		IL_005c: ret

		IL_005d: nop
		IL_005e: ldloc.0
		IL_005f: ldfld string [Unity.TextMeshPro]TMPro.TMP_InputField::m_Text
		IL_0064: stloc.2
		IL_0065: ldloca.s 3
		IL_0067: call instance string [mscorlib]System.Int32::ToString()
		IL_006c: stloc.s 4
		IL_006e: ldloc.2
		IL_006f: ldloc.s 4
		IL_0071: call string [mscorlib]System.String::Concat(string, string)
		IL_0076: stloc.s 5
		IL_0078: ldloc.0
		IL_0079: ldloc.s 5
		IL_007b: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_InputField::set_text(string)
		IL_0080: ret

		IL_0081: nop
		IL_0082: ldnull
		IL_0083: throw
	} // end of method MathGameScript::ButtonPress

	.method private hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator CheatText (
			string text
		) cil managed 
	{
		// Method begins at RVA 0x8b9c
		// Header size: 12
		// Code size: 47 (0x2f)
		.maxstack 2
		.locals init (
			[0] class MathGameScript/'<CheatText>d__11'
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(MathGameScript+<CheatText>d__11)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class MathGameScript/'<CheatText>d__11'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 MathGameScript/'<CheatText>d__11'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class MathGameScript MathGameScript/'<CheatText>d__11'::'<>4__this'
		IL_0026: ldloc.0
		IL_0027: ldarg.1
		IL_0028: stfld string MathGameScript/'<CheatText>d__11'::text
		IL_002d: ldloc.0
		IL_002e: ret
	} // end of method MathGameScript::CheatText

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x8bd8
		// Header size: 12
		// Code size: 172 (0xac)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.UI]UnityEngine.UI.RawImage[],
			[1] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[2] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[3] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[4] string[],
			[5] string[],
			[6] class [UnityEngine.AudioModule]UnityEngine.AudioClip[]
		)

		IL_0000: nop
		IL_0001: ldc.i4.3
		IL_0002: newarr [UnityEngine.UI]UnityEngine.UI.RawImage
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.UI]UnityEngine.UI.RawImage[] MathGameScript::results
		IL_000f: ldc.i4.s 10
		IL_0011: newarr [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0016: stloc.1
		IL_0017: ldarg.0
		IL_0018: ldloc.1
		IL_0019: stfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] MathGameScript::bal_numbers
		IL_001e: ldc.i4.5
		IL_001f: newarr [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0024: stloc.2
		IL_0025: ldarg.0
		IL_0026: ldloc.2
		IL_0027: stfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] MathGameScript::bal_praises
		IL_002c: ldc.i4.3
		IL_002d: newarr [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0032: stloc.3
		IL_0033: ldarg.0
		IL_0034: ldloc.3
		IL_0035: stfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] MathGameScript::bal_problems
		IL_003a: ldc.i4.2
		IL_003b: newarr [mscorlib]System.String
		IL_0040: stloc.s 4
		IL_0042: ldloc.s 4
		IL_0044: ldc.i4.0
		IL_0045: ldstr "I GET ANGRIER FOR EVERY PROBLEM YOU GET WRONG"
		IL_004a: stelem [mscorlib]System.String
		IL_004f: ldloc.s 4
		IL_0051: ldc.i4.1
		IL_0052: ldstr "I HEAR EVERY DOOR YOU OPEN"
		IL_0057: stelem [mscorlib]System.String
		IL_005c: ldarg.0
		IL_005d: ldloc.s 4
		IL_005f: stfld string[] MathGameScript::hintText
		IL_0064: ldc.i4.2
		IL_0065: newarr [mscorlib]System.String
		IL_006a: stloc.s 5
		IL_006c: ldloc.s 5
		IL_006e: ldc.i4.0
		IL_006f: ldstr "That's more like it..."
		IL_0074: stelem [mscorlib]System.String
		IL_0079: ldloc.s 5
		IL_007b: ldc.i4.1
		IL_007c: ldstr "Keep up the good work or see me after class..."
		IL_0081: stelem [mscorlib]System.String
		IL_0086: ldarg.0
		IL_0087: ldloc.s 5
		IL_0089: stfld string[] MathGameScript::endlessHintText
		IL_008e: ldc.i4.s 20
		IL_0090: newarr [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0095: stloc.s 6
		IL_0097: ldarg.0
		IL_0098: ldloc.s 6
		IL_009a: stfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] MathGameScript::audioQueue
		IL_009f: ldarg.0
		IL_00a0: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_00a5: ret

		IL_00a6: nop
		IL_00a7: ldnull
		IL_00a8: throw

		IL_00a9: nop
		IL_00aa: ldnull
		IL_00ab: throw
	} // end of method MathGameScript::.ctor

} // end of class MathGameScript
