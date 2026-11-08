.class public auto ansi beforefieldinit YouWonScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field private float32 delay

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x275e
		// Header size: 1
		// Code size: 13 (0xd)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.r4 10
		IL_0007: stfld float32 YouWonScript::delay
		IL_000c: ret
	} // end of method YouWonScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x13f94
		// Header size: 12
		// Code size: 47 (0x2f)
		.maxstack 3
		.locals init (
			[0] float32,
			[1] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld float32 YouWonScript::delay
		IL_0007: stloc.0
		IL_0008: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_000d: stloc.1
		IL_000e: ldarg.0
		IL_000f: ldloc.0
		IL_0010: ldloc.1
		IL_0011: sub
		IL_0012: stfld float32 YouWonScript::delay
		IL_0017: ldc.r4 0.0
		IL_001c: ldloc.0
		IL_001d: ldloc.1
		IL_001e: sub
		IL_001f: clt
		IL_0021: brtrue.s IL_002d

		IL_0023: nop
		IL_0024: br.s IL_0027

		IL_0026: nop

		IL_0027: nop
		IL_0028: call void [UnityEngine.CoreModule]UnityEngine.Application::Quit()

		IL_002d: nop
		IL_002e: ret
	} // end of method YouWonScript::Update

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x276c
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method YouWonScript::.ctor

} // end of class YouWonScript
