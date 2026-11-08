.class public auto ansi beforefieldinit AudioQueueScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field private class [UnityEngine.AudioModule]UnityEngine.AudioSource audioDevice
	.field private int32 audioInQueue
	.field private class [UnityEngine.AudioModule]UnityEngine.AudioClip[] audioQueue

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x3090
		// Header size: 12
		// Code size: 16 (0x10)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.AudioModule]UnityEngine.AudioSource
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.AudioModule]UnityEngine.AudioSource>()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.AudioModule]UnityEngine.AudioSource AudioQueueScript::audioDevice
		IL_000f: ret
	} // end of method AudioQueueScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x30ac
		// Header size: 12
		// Code size: 165 (0xa5)
		.maxstack 3
		.locals init (
			[0] bool,
			[1] bool,
			[2] class [UnityEngine.AudioModule]UnityEngine.AudioClip,
			[3] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[4] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource AudioQueueScript::audioDevice
		IL_0007: callvirt instance bool [UnityEngine.AudioModule]UnityEngine.AudioSource::get_isPlaying()
		IL_000c: stloc.0
		IL_000d: ldloc.0
		IL_000e: stloc.1
		IL_000f: ldloc.0
		IL_0010: ldc.i4.0
		IL_0011: ceq
		IL_0013: ldc.i4.0
		IL_0014: ceq
		IL_0016: brtrue IL_009d

		IL_001b: nop
		IL_001c: ldarg.0
		IL_001d: ldfld int32 AudioQueueScript::audioInQueue
		IL_0022: ldc.i4.0
		IL_0023: cgt
		IL_0025: ldc.i4.0
		IL_0026: ceq
		IL_0028: brtrue.s IL_009d

		IL_002a: nop
		IL_002b: ldarg.0
		IL_002c: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource AudioQueueScript::audioDevice
		IL_0031: ldarg.0
		IL_0032: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] AudioQueueScript::audioQueue
		IL_0037: ldc.i4.0
		IL_0038: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_003d: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_0042: ldarg.0
		IL_0043: ldfld int32 AudioQueueScript::audioInQueue
		IL_0048: ldc.i4.1
		IL_0049: cgt
		IL_004b: ldc.i4.0
		IL_004c: ceq
		IL_004e: brtrue.s IL_008e

		IL_0050: nop
		// loop start (head: IL_0051)
			IL_0051: nop
			IL_0052: ldarg.0
			IL_0053: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] AudioQueueScript::audioQueue
			IL_0058: ldc.i4.1
			IL_0059: conv.i8
			IL_005a: conv.i4
			IL_005b: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
			IL_0060: stloc.2
			IL_0061: ldarg.0
			IL_0062: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] AudioQueueScript::audioQueue
			IL_0067: stloc.3
			IL_0068: ldloc.3
			IL_0069: ldc.i4.1
			IL_006a: conv.i8
			IL_006b: ldc.i8 1
			IL_0074: sub
			IL_0075: conv.i4
			IL_0076: ldloc.2
			IL_0077: stelem [UnityEngine.AudioModule]UnityEngine.AudioClip
			IL_007c: ldc.i4.1
			IL_007d: ldc.i4.1
			IL_007e: add
			IL_007f: stloc.s 4
			IL_0081: ldc.i4.1
			IL_0082: ldc.i4.1
			IL_0083: add
			IL_0084: ldarg.0
			IL_0085: ldfld int32 AudioQueueScript::audioInQueue
			IL_008a: clt
			IL_008c: brtrue.s IL_0051
		// end loop

		IL_008e: nop
		IL_008f: ldarg.0
		IL_0090: ldarg.0
		IL_0091: ldfld int32 AudioQueueScript::audioInQueue
		IL_0096: ldc.i4.1
		IL_0097: sub
		IL_0098: stfld int32 AudioQueueScript::audioInQueue

		IL_009d: nop
		IL_009e: ret

		IL_009f: nop
		IL_00a0: ldnull
		IL_00a1: throw

		IL_00a2: nop
		IL_00a3: ldnull
		IL_00a4: throw
	} // end of method AudioQueueScript::Update

	.method public hidebysig 
		instance void QueueAudio (
			class [UnityEngine.AudioModule]UnityEngine.AudioClip sound
		) cil managed 
	{
		// Method begins at RVA 0x3160
		// Header size: 12
		// Code size: 46 (0x2e)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[1] int64
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] AudioQueueScript::audioQueue
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld int32 AudioQueueScript::audioInQueue
		IL_000e: conv.i8
		IL_000f: stloc.1
		IL_0010: ldloc.0
		IL_0011: ldloc.1
		IL_0012: conv.i4
		IL_0013: ldarg.1
		IL_0014: stelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0019: ldarg.0
		IL_001a: ldarg.0
		IL_001b: ldfld int32 AudioQueueScript::audioInQueue
		IL_0020: ldc.i4.1
		IL_0021: add
		IL_0022: stfld int32 AudioQueueScript::audioInQueue
		IL_0027: ret

		IL_0028: nop
		IL_0029: ldnull
		IL_002a: throw

		IL_002b: nop
		IL_002c: ldnull
		IL_002d: throw
	} // end of method AudioQueueScript::QueueAudio

	.method private hidebysig 
		instance void PlayQueue () cil managed 
	{
		// Method begins at RVA 0x319c
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
		IL_0002: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource AudioQueueScript::audioDevice
		IL_0007: ldarg.0
		IL_0008: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] AudioQueueScript::audioQueue
		IL_000d: ldc.i4.0
		IL_000e: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0013: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_0018: ldarg.0
		IL_0019: ldfld int32 AudioQueueScript::audioInQueue
		IL_001e: ldc.i4.1
		IL_001f: cgt
		IL_0021: ldc.i4.0
		IL_0022: ceq
		IL_0024: brtrue.s IL_0063

		IL_0026: nop
		// loop start (head: IL_0027)
			IL_0027: nop
			IL_0028: ldarg.0
			IL_0029: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] AudioQueueScript::audioQueue
			IL_002e: ldc.i4.1
			IL_002f: conv.i8
			IL_0030: conv.i4
			IL_0031: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
			IL_0036: stloc.0
			IL_0037: ldarg.0
			IL_0038: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] AudioQueueScript::audioQueue
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
			IL_005a: ldfld int32 AudioQueueScript::audioInQueue
			IL_005f: clt
			IL_0061: brtrue.s IL_0027
		// end loop

		IL_0063: nop
		IL_0064: ldarg.0
		IL_0065: ldarg.0
		IL_0066: ldfld int32 AudioQueueScript::audioInQueue
		IL_006b: ldc.i4.1
		IL_006c: sub
		IL_006d: stfld int32 AudioQueueScript::audioInQueue
		IL_0072: ret

		IL_0073: nop
		IL_0074: ldnull
		IL_0075: throw

		IL_0076: nop
		IL_0077: ldnull
		IL_0078: throw
	} // end of method AudioQueueScript::PlayQueue

	.method private hidebysig 
		instance void UnqueueAudio () cil managed 
	{
		// Method begins at RVA 0x3224
		// Header size: 12
		// Code size: 92 (0x5c)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.AudioModule]UnityEngine.AudioClip[],
			[1] class [UnityEngine.AudioModule]UnityEngine.AudioClip,
			[2] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld int32 AudioQueueScript::audioInQueue
		IL_0007: ldc.i4.1
		IL_0008: cgt
		IL_000a: ldc.i4.0
		IL_000b: ceq
		IL_000d: brtrue.s IL_0046
		// loop start (head: IL_000f)
			IL_000f: nop
			IL_0010: ldarg.0
			IL_0011: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] AudioQueueScript::audioQueue
			IL_0016: stloc.0
			IL_0017: ldloc.0
			IL_0018: ldc.i4.1
			IL_0019: conv.i8
			IL_001a: conv.i4
			IL_001b: ldelem [UnityEngine.AudioModule]UnityEngine.AudioClip
			IL_0020: stloc.1
			IL_0021: ldloc.0
			IL_0022: ldc.i4.1
			IL_0023: conv.i8
			IL_0024: ldc.i8 1
			IL_002d: sub
			IL_002e: conv.i4
			IL_002f: ldloc.1
			IL_0030: stelem [UnityEngine.AudioModule]UnityEngine.AudioClip
			IL_0035: ldc.i4.1
			IL_0036: ldc.i4.1
			IL_0037: add
			IL_0038: stloc.2
			IL_0039: ldc.i4.1
			IL_003a: ldc.i4.1
			IL_003b: add
			IL_003c: ldarg.0
			IL_003d: ldfld int32 AudioQueueScript::audioInQueue
			IL_0042: clt
			IL_0044: brtrue.s IL_000f
		// end loop

		IL_0046: nop
		IL_0047: ldarg.0
		IL_0048: ldarg.0
		IL_0049: ldfld int32 AudioQueueScript::audioInQueue
		IL_004e: ldc.i4.1
		IL_004f: sub
		IL_0050: stfld int32 AudioQueueScript::audioInQueue
		IL_0055: ret

		IL_0056: nop
		IL_0057: ldnull
		IL_0058: throw

		IL_0059: nop
		IL_005a: ldnull
		IL_005b: throw
	} // end of method AudioQueueScript::UnqueueAudio

	.method public hidebysig 
		instance void ClearAudioQueue () cil managed 
	{
		// Method begins at RVA 0x2059
		// Header size: 1
		// Code size: 9 (0x9)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i4.0
		IL_0003: stfld int32 AudioQueueScript::audioInQueue
		IL_0008: ret
	} // end of method AudioQueueScript::ClearAudioQueue

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x328c
		// Header size: 12
		// Code size: 23 (0x17)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.AudioModule]UnityEngine.AudioClip[]
		)

		IL_0000: nop
		IL_0001: ldc.i4.s 100
		IL_0003: newarr [UnityEngine.AudioModule]UnityEngine.AudioClip
		IL_0008: stloc.0
		IL_0009: ldarg.0
		IL_000a: ldloc.0
		IL_000b: stfld class [UnityEngine.AudioModule]UnityEngine.AudioClip[] AudioQueueScript::audioQueue
		IL_0010: ldarg.0
		IL_0011: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0016: ret
	} // end of method AudioQueueScript::.ctor

} // end of class AudioQueueScript
