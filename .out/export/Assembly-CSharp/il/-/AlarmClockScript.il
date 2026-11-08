.class public auto ansi beforefieldinit AlarmClockScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public float32 timeLeft
	.field private float32 lifeSpan
	.field private bool rang
	.field public class BaldiScript baldi
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip ring
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioSource audioDevice

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x262a
		// Header size: 1
		// Code size: 24 (0x18)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.r4 30
		IL_0007: stfld float32 AlarmClockScript::timeLeft
		IL_000c: ldarg.0
		IL_000d: ldc.r4 35
		IL_0012: stfld float32 AlarmClockScript::lifeSpan
		IL_0017: ret
	} // end of method AlarmClockScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x10a88
		// Header size: 12
		// Code size: 299 (0x12b)
		.maxstack 3
		.locals init (
			[0] float32,
			[1] class BaldiScript,
			[2] bool,
			[3] class BaldiScript,
			[4] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[5] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[6] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[7] float32,
			[8] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[9] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[10] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld float32 AlarmClockScript::timeLeft
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld float32 AlarmClockScript::timeLeft
		IL_000e: ldc.r4 0.0
		IL_0013: clt
		IL_0015: ldc.i4.0
		IL_0016: ceq
		IL_0018: brtrue IL_00c6

		IL_001d: nop
		IL_001e: ldarg.0
		IL_001f: ldfld bool AlarmClockScript::rang
		IL_0024: ldc.i4.0
		IL_0025: ceq
		IL_0027: ldc.i4.0
		IL_0028: ceq
		IL_002a: brtrue IL_00d8

		IL_002f: nop
		IL_0030: ldarg.0
		IL_0031: ldfld class BaldiScript AlarmClockScript::baldi
		IL_0036: stloc.1
		IL_0037: ldarg.0
		IL_0038: ldc.i4.1
		IL_0039: stfld bool AlarmClockScript::rang
		IL_003e: ldloc.1
		IL_003f: callvirt instance bool [UnityEngine.CoreModule]UnityEngine.Behaviour::get_isActiveAndEnabled()
		IL_0044: stloc.2
		IL_0045: ldloc.2
		IL_0046: ldc.i4.0
		IL_0047: ceq
		IL_0049: brtrue.s IL_009b

		IL_004b: nop
		IL_004c: ldarg.0
		IL_004d: ldfld class BaldiScript AlarmClockScript::baldi
		IL_0052: stloc.3
		IL_0053: ldarg.0
		IL_0054: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0059: stloc.s 4
		IL_005b: ldloc.s 4
		IL_005d: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0062: stloc.s 5
		IL_0064: ldloc.3
		IL_0065: ldloca.s 6
		IL_0067: ldloca.s 5
		IL_0069: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_006e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0073: ldloca.s 6
		IL_0075: ldloca.s 5
		IL_0077: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_007c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0081: ldloca.s 6
		IL_0083: ldloca.s 5
		IL_0085: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_008a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_008f: ldloc.s 6
		IL_0091: ldc.r4 8
		IL_0096: callvirt instance void BaldiScript::Hear(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, float32)

		IL_009b: nop
		IL_009c: ldarg.0
		IL_009d: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource AlarmClockScript::audioDevice
		IL_00a2: ldarg.0
		IL_00a3: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip AlarmClockScript::ring
		IL_00a8: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_clip(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_00ad: ldarg.0
		IL_00ae: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource AlarmClockScript::audioDevice
		IL_00b3: ldc.i4.0
		IL_00b4: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_loop(bool)
		IL_00b9: ldarg.0
		IL_00ba: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource AlarmClockScript::audioDevice
		IL_00bf: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Play()
		IL_00c4: br.s IL_00d8

		IL_00c6: nop
		IL_00c7: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_00cc: stloc.s 7
		IL_00ce: ldarg.0
		IL_00cf: ldloc.0
		IL_00d0: ldloc.s 7
		IL_00d2: sub
		IL_00d3: stfld float32 AlarmClockScript::timeLeft

		IL_00d8: nop
		IL_00d9: ldarg.0
		IL_00da: ldfld float32 AlarmClockScript::lifeSpan
		IL_00df: stloc.0
		IL_00e0: ldarg.0
		IL_00e1: ldfld float32 AlarmClockScript::lifeSpan
		IL_00e6: ldc.r4 0.0
		IL_00eb: clt
		IL_00ed: ldc.i4.0
		IL_00ee: ceq
		IL_00f0: brtrue.s IL_0111

		IL_00f2: nop
		IL_00f3: ldarg.0
		IL_00f4: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_00f9: stloc.s 8
		IL_00fb: ldloc.s 8
		IL_00fd: stloc.s 9
		IL_00ff: br.s IL_0102

		IL_0101: nop

		IL_0102: nop
		IL_0103: ldloc.s 9
		IL_0105: ldc.r4 0.0
		IL_010a: call void [UnityEngine.CoreModule]UnityEngine.Object::Destroy(class [UnityEngine.CoreModule]UnityEngine.Object, float32)
		IL_010f: br.s IL_0126

		IL_0111: nop
		IL_0112: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0117: stloc.s 10
		IL_0119: ldstr "movss dword ptr [rbx+24h],xmm6 ; value = BinaryExpr"
		IL_011e: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0123: ldloc.s 10
		IL_0125: stloc.0

		IL_0126: nop
		IL_0127: ret

		IL_0128: nop
		IL_0129: ldnull
		IL_012a: throw
	} // end of method AlarmClockScript::Update

	.method private hidebysig 
		instance void Alarm () cil managed 
	{
		// Method begins at RVA 0x10bc0
		// Header size: 12
		// Code size: 148 (0x94)
		.maxstack 3
		.locals init (
			[0] bool,
			[1] class BaldiScript,
			[2] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[4] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i4.1
		IL_0003: stfld bool AlarmClockScript::rang
		IL_0008: ldarg.0
		IL_0009: ldfld class BaldiScript AlarmClockScript::baldi
		IL_000e: callvirt instance bool [UnityEngine.CoreModule]UnityEngine.Behaviour::get_isActiveAndEnabled()
		IL_0013: stloc.0
		IL_0014: ldloc.0
		IL_0015: ldc.i4.0
		IL_0016: ceq
		IL_0018: brtrue.s IL_0067

		IL_001a: nop
		IL_001b: ldarg.0
		IL_001c: ldfld class BaldiScript AlarmClockScript::baldi
		IL_0021: stloc.1
		IL_0022: ldarg.0
		IL_0023: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0028: stloc.2
		IL_0029: ldloc.2
		IL_002a: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_002f: stloc.3
		IL_0030: ldloc.1
		IL_0031: ldloca.s 4
		IL_0033: ldloca.s 3
		IL_0035: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_003a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_003f: ldloca.s 4
		IL_0041: ldloca.s 3
		IL_0043: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0048: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_004d: ldloca.s 4
		IL_004f: ldloca.s 3
		IL_0051: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0056: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_005b: ldloc.s 4
		IL_005d: ldc.r4 8
		IL_0062: callvirt instance void BaldiScript::Hear(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, float32)

		IL_0067: nop
		IL_0068: ldarg.0
		IL_0069: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource AlarmClockScript::audioDevice
		IL_006e: ldarg.0
		IL_006f: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip AlarmClockScript::ring
		IL_0074: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_clip(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_0079: ldarg.0
		IL_007a: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource AlarmClockScript::audioDevice
		IL_007f: ldc.i4.0
		IL_0080: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_loop(bool)
		IL_0085: ldarg.0
		IL_0086: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource AlarmClockScript::audioDevice
		IL_008b: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Play()
		IL_0090: ret

		IL_0091: nop
		IL_0092: ldnull
		IL_0093: throw
	} // end of method AlarmClockScript::Alarm

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2643
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method AlarmClockScript::.ctor

} // end of class AlarmClockScript
