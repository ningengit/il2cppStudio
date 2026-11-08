.class public auto ansi beforefieldinit GameOverScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field private class [UnityEngine.UI]UnityEngine.UI.Image image
	.field private float32 delay
	.field public class [UnityEngine.CoreModule]UnityEngine.Sprite[] images
	.field public class [UnityEngine.CoreModule]UnityEngine.Sprite rare
	.field private float32 chance
	.field private class [UnityEngine.AudioModule]UnityEngine.AudioSource audioDevice
	.field public string LoadScene

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0xafb4
		// Header size: 12
		// Code size: 150 (0x96)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.UI]UnityEngine.UI.Image,
			[1] class [UnityEngine.AudioModule]UnityEngine.AudioSource,
			[2] float32,
			[3] float32,
			[4] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.UI]UnityEngine.UI.Image>()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.UI]UnityEngine.UI.Image GameOverScript::image
		IL_000f: ldarg.0
		IL_0010: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.AudioModule]UnityEngine.AudioSource>()
		IL_0015: stloc.1
		IL_0016: ldarg.0
		IL_0017: ldloc.1
		IL_0018: stfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameOverScript::audioDevice
		IL_001d: ldarg.0
		IL_001e: ldc.r4 5
		IL_0023: stfld float32 GameOverScript::delay
		IL_0028: ldc.r4 1
		IL_002d: ldc.r4 99
		IL_0032: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_0037: stloc.2
		IL_0038: ldarg.0
		IL_0039: ldloc.2
		IL_003a: stfld float32 GameOverScript::chance
		IL_003f: ldc.r4 98
		IL_0044: ldloc.2
		IL_0045: cgt
		IL_0047: brtrue.s IL_005c

		IL_0049: nop
		IL_004a: ldarg.0
		IL_004b: ldfld class [UnityEngine.UI]UnityEngine.UI.Image GameOverScript::image
		IL_0050: ldarg.0
		IL_0051: ldfld class [UnityEngine.CoreModule]UnityEngine.Sprite GameOverScript::rare
		IL_0056: callvirt instance void [UnityEngine.UI]UnityEngine.UI.Image::set_sprite(class [UnityEngine.CoreModule]UnityEngine.Sprite)
		IL_005b: ret

		IL_005c: nop
		IL_005d: ldc.r4 0.0
		IL_0062: ldc.r4 4
		IL_0067: call float32 [UnityEngine.CoreModule]UnityEngine.Random::Range(float32, float32)
		IL_006c: stloc.3
		IL_006d: ldloc.3
		IL_006e: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_0073: stloc.s 4
		IL_0075: ldarg.0
		IL_0076: ldfld class [UnityEngine.UI]UnityEngine.UI.Image GameOverScript::image
		IL_007b: ldarg.0
		IL_007c: ldfld class [UnityEngine.CoreModule]UnityEngine.Sprite[] GameOverScript::images
		IL_0081: ldloc.s 4
		IL_0083: conv.i8
		IL_0084: conv.i4
		IL_0085: ldelem [UnityEngine.CoreModule]UnityEngine.Sprite
		IL_008a: callvirt instance void [UnityEngine.UI]UnityEngine.UI.Image::set_sprite(class [UnityEngine.CoreModule]UnityEngine.Sprite)
		IL_008f: ret

		IL_0090: nop
		IL_0091: ldnull
		IL_0092: throw

		IL_0093: nop
		IL_0094: ldnull
		IL_0095: throw
	} // end of method GameOverScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0xb058
		// Header size: 12
		// Code size: 219 (0xdb)
		.maxstack 3
		.locals init (
			[0] float32,
			[1] float32,
			[2] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[3] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[4] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[5] int64,
			[6] bool,
			[7] bool,
			[8] string
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld float32 GameOverScript::delay
		IL_0007: stloc.0
		IL_0008: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_000d: stloc.1
		IL_000e: ldarg.0
		IL_000f: ldloc.0
		IL_0010: ldloc.1
		IL_0011: sub
		IL_0012: stfld float32 GameOverScript::delay
		IL_0017: ldc.r4 0.0
		IL_001c: ldloc.0
		IL_001d: ldloc.1
		IL_001e: sub
		IL_001f: clt
		IL_0021: brtrue IL_00d6

		IL_0026: nop
		IL_0027: ldc.r4 98
		IL_002c: ldarg.0
		IL_002d: ldfld float32 GameOverScript::chance
		IL_0032: cgt
		IL_0034: brtrue IL_00c2

		IL_0039: nop
		IL_003a: ldarg.0
		IL_003b: ldfld class [UnityEngine.UI]UnityEngine.UI.Image GameOverScript::image
		IL_0040: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0045: stloc.2
		IL_0046: ldloc.2
		IL_0047: stloc.3
		IL_0048: ldloc.3
		IL_0049: ldloca.s 4
		IL_004b: ldc.r4 5
		IL_0050: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0055: ldloca.s 4
		IL_0057: ldc.r4 5
		IL_005c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0061: ldloca.s 4
		IL_0063: ldc.r4 1
		IL_0068: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_006d: ldloc.s 4
		IL_006f: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_localScale(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0074: ldstr "global_13EAC10"
		IL_0079: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_007e: stloc.s 5
		IL_0080: ldarg.0
		IL_0081: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameOverScript::audioDevice
		IL_0086: callvirt instance bool [UnityEngine.AudioModule]UnityEngine.AudioSource::get_isPlaying()
		IL_008b: stloc.s 6
		IL_008d: ldloc.s 6
		IL_008f: stloc.s 7
		IL_0091: ldloc.s 6
		IL_0093: ldc.i4.0
		IL_0094: ceq
		IL_0096: ldc.i4.0
		IL_0097: ceq
		IL_0099: brtrue.s IL_00a7

		IL_009b: nop
		IL_009c: ldarg.0
		IL_009d: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameOverScript::audioDevice
		IL_00a2: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Play()

		IL_00a7: nop
		IL_00a8: ldc.r4 -5
		IL_00ad: ldarg.0
		IL_00ae: ldfld float32 GameOverScript::delay
		IL_00b3: clt
		IL_00b5: brtrue.s IL_00d6

		IL_00b7: nop
		IL_00b8: br.s IL_00bb

		IL_00ba: nop

		IL_00bb: nop
		IL_00bc: call void [UnityEngine.CoreModule]UnityEngine.Application::Quit()
		IL_00c1: ret

		IL_00c2: nop
		IL_00c3: ldarg.0
		IL_00c4: ldfld string GameOverScript::LoadScene
		IL_00c9: stloc.s 8
		IL_00cb: br.s IL_00ce

		IL_00cd: nop

		IL_00ce: nop
		IL_00cf: ldloc.s 8
		IL_00d1: call void [UnityEngine.CoreModule]UnityEngine.SceneManagement.SceneManager::LoadScene(string)

		IL_00d6: nop
		IL_00d7: ret

		IL_00d8: nop
		IL_00d9: ldnull
		IL_00da: throw
	} // end of method GameOverScript::Update

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0xb140
		// Header size: 12
		// Code size: 22 (0x16)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Sprite[]
		)

		IL_0000: nop
		IL_0001: ldc.i4.5
		IL_0002: newarr [UnityEngine.CoreModule]UnityEngine.Sprite
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.CoreModule]UnityEngine.Sprite[] GameOverScript::images
		IL_000f: ldarg.0
		IL_0010: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0015: ret
	} // end of method GameOverScript::.ctor

} // end of class GameOverScript
