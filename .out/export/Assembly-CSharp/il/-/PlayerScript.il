.class public auto ansi beforefieldinit PlayerScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested private auto ansi sealed beforefieldinit '<BootTimer>d__14'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerator`1<object>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private object '<>2__current'
		.field public class PlayerScript '<>4__this'
		.field private float32 '<time>5__2'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x2cc3
			// Header size: 1
			// Code size: 16 (0x10)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ldarg.0
			IL_0009: ldarg.1
			IL_000a: stfld int32 PlayerScript/'<BootTimer>d__14'::'<>1__state'
			IL_000f: ret
		} // end of method '<BootTimer>d__14'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			// Method begins at RVA 0x2cd4
			// Header size: 1
			// Code size: 2 (0x2)
			.maxstack 8

			IL_0000: nop
			IL_0001: ret
		} // end of method '<BootTimer>d__14'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			// Method begins at RVA 0x1ab30
			// Header size: 12
			// Code size: 132 (0x84)
			.maxstack 3
			.locals init (
				[0] int32,
				[1] float32,
				[2] float32,
				[3] float32
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld int32 PlayerScript/'<BootTimer>d__14'::'<>1__state'
			IL_0007: stloc.0
			IL_0008: ldarg.0
			IL_0009: ldfld int32 PlayerScript/'<BootTimer>d__14'::'<>1__state'
			IL_000e: ldc.i4.0
			IL_000f: ceq
			IL_0011: ldc.i4.0
			IL_0012: ceq
			IL_0014: brtrue.s IL_0024

			IL_0016: nop
			IL_0017: ldarg.0
			IL_0018: ldc.r4 15
			IL_001d: stfld float32 PlayerScript/'<BootTimer>d__14'::'<time>5__2'
			IL_0022: br.s IL_002e

			IL_0024: nop
			IL_0025: ldloc.0
			IL_0026: ldc.i4.1
			IL_0027: ceq
			IL_0029: ldc.i4.0
			IL_002a: ceq
			IL_002c: brtrue.s IL_007e

			IL_002e: nop
			IL_002f: ldarg.0
			IL_0030: ldfld float32 PlayerScript/'<BootTimer>d__14'::'<time>5__2'
			IL_0035: stloc.1
			IL_0036: ldarg.0
			IL_0037: ldc.i4.m1
			IL_0038: stfld int32 PlayerScript/'<BootTimer>d__14'::'<>1__state'
			IL_003d: ldloc.1
			IL_003e: stloc.2
			IL_003f: ldarg.0
			IL_0040: ldfld float32 PlayerScript/'<BootTimer>d__14'::'<time>5__2'
			IL_0045: ldc.r4 0.0
			IL_004a: cgt
			IL_004c: ldc.i4.0
			IL_004d: ceq
			IL_004f: brtrue.s IL_0071

			IL_0051: nop
			IL_0052: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
			IL_0057: stloc.3
			IL_0058: ldarg.0
			IL_0059: ldnull
			IL_005a: stfld object PlayerScript/'<BootTimer>d__14'::'<>2__current'
			IL_005f: ldarg.0
			IL_0060: ldloc.2
			IL_0061: ldloc.3
			IL_0062: sub
			IL_0063: stfld float32 PlayerScript/'<BootTimer>d__14'::'<time>5__2'
			IL_0068: ldarg.0
			IL_0069: ldc.i4.1
			IL_006a: stfld int32 PlayerScript/'<BootTimer>d__14'::'<>1__state'
			IL_006f: ldc.i4.1
			IL_0070: ret

			IL_0071: nop
			IL_0072: ldarg.0
			IL_0073: ldfld class PlayerScript PlayerScript/'<BootTimer>d__14'::'<>4__this'
			IL_0078: ldc.i4.0
			IL_0079: stfld bool PlayerScript::bootsActive

			IL_007e: nop
			IL_007f: ldc.i4.0
			IL_0080: ret

			IL_0081: nop
			IL_0082: ldnull
			IL_0083: throw
		} // end of method '<BootTimer>d__14'::MoveNext

		.method private final hidebysig specialname newslot virtual 
			instance object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () cil managed 
		{
			// Method begins at RVA 0x2cd7
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object PlayerScript/'<BootTimer>d__14'::'<>2__current'
			IL_0007: ret
		} // end of method '<BootTimer>d__14'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'

		.method private final hidebysig newslot virtual 
			instance void System.Collections.IEnumerator.Reset () cil managed 
		{
			// Method begins at RVA 0x1abc0
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
		} // end of method '<BootTimer>d__14'::System.Collections.IEnumerator.Reset

		.method private final hidebysig specialname newslot virtual 
			instance object System.Collections.IEnumerator.get_Current () cil managed 
		{
			// Method begins at RVA 0x2ce0
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object PlayerScript/'<BootTimer>d__14'::'<>2__current'
			IL_0007: ret
		} // end of method '<BootTimer>d__14'::System.Collections.IEnumerator.get_Current

		// Properties
		.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current'()
		{
			.get instance object PlayerScript/'<BootTimer>d__14'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'()
		}
		.property instance object System.Collections.IEnumerator.Current()
		{
			.get instance object PlayerScript/'<BootTimer>d__14'::System.Collections.IEnumerator.get_Current()
		}

	} // end of class <BootTimer>d__14

	.class nested private auto ansi sealed beforefieldinit '<KeepTheHudOff>d__6'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerator`1<object>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private object '<>2__current'
		.field public class PlayerScript '<>4__this'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x2ce9
			// Header size: 1
			// Code size: 16 (0x10)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ldarg.0
			IL_0009: ldarg.1
			IL_000a: stfld int32 PlayerScript/'<KeepTheHudOff>d__6'::'<>1__state'
			IL_000f: ret
		} // end of method '<KeepTheHudOff>d__6'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			// Method begins at RVA 0x2cfa
			// Header size: 1
			// Code size: 2 (0x2)
			.maxstack 8

			IL_0000: nop
			IL_0001: ret
		} // end of method '<KeepTheHudOff>d__6'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			// Method begins at RVA 0x1ac10
			// Header size: 12
			// Code size: 93 (0x5d)
			.maxstack 2
			.locals init (
				[0] class PlayerScript,
				[1] class PlayerScript,
				[2] class [UnityEngine.CoreModule]UnityEngine.WaitForEndOfFrame
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld int32 PlayerScript/'<KeepTheHudOff>d__6'::'<>1__state'
			IL_0007: ldc.i4.1
			IL_0008: cgt.un
			IL_000a: brtrue.s IL_0057

			IL_000c: nop
			IL_000d: ldarg.0
			IL_000e: ldfld class PlayerScript PlayerScript/'<KeepTheHudOff>d__6'::'<>4__this'
			IL_0013: stloc.0
			IL_0014: ldarg.0
			IL_0015: ldc.i4.m1
			IL_0016: stfld int32 PlayerScript/'<KeepTheHudOff>d__6'::'<>1__state'
			IL_001b: ldloc.0
			IL_001c: stloc.1
			IL_001d: ldloc.1
			IL_001e: ldfld bool PlayerScript::gameOver
			IL_0023: ldc.i4.0
			IL_0024: ceq
			IL_0026: brtrue.s IL_0057

			IL_0028: nop
			IL_0029: ldloc.1
			IL_002a: ldfld class [UnityEngine.UIModule]UnityEngine.Canvas PlayerScript::hud
			IL_002f: ldc.i4.0
			IL_0030: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::set_enabled(bool)
			IL_0035: ldloc.1
			IL_0036: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject PlayerScript::jumpRopeScreen
			IL_003b: ldc.i4.0
			IL_003c: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
			IL_0041: newobj instance void [mscorlib]Mono.Globalization.Unicode.ContractionComparer::.ctor()
			IL_0046: stloc.2
			IL_0047: ldarg.0
			IL_0048: ldloc.2
			IL_0049: stfld object PlayerScript/'<KeepTheHudOff>d__6'::'<>2__current'
			IL_004e: ldarg.0
			IL_004f: ldc.i4.1
			IL_0050: stfld int32 PlayerScript/'<KeepTheHudOff>d__6'::'<>1__state'
			IL_0055: ldc.i4.1
			IL_0056: ret

			IL_0057: nop
			IL_0058: ldc.i4.0
			IL_0059: ret

			IL_005a: nop
			IL_005b: ldnull
			IL_005c: throw
		} // end of method '<KeepTheHudOff>d__6'::MoveNext

		.method private final hidebysig specialname newslot virtual 
			instance object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () cil managed 
		{
			// Method begins at RVA 0x2cfd
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object PlayerScript/'<KeepTheHudOff>d__6'::'<>2__current'
			IL_0007: ret
		} // end of method '<KeepTheHudOff>d__6'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'

		.method private final hidebysig newslot virtual 
			instance void System.Collections.IEnumerator.Reset () cil managed 
		{
			// Method begins at RVA 0x1ac7c
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
		} // end of method '<KeepTheHudOff>d__6'::System.Collections.IEnumerator.Reset

		.method private final hidebysig specialname newslot virtual 
			instance object System.Collections.IEnumerator.get_Current () cil managed 
		{
			// Method begins at RVA 0x2d06
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object PlayerScript/'<KeepTheHudOff>d__6'::'<>2__current'
			IL_0007: ret
		} // end of method '<KeepTheHudOff>d__6'::System.Collections.IEnumerator.get_Current

		// Properties
		.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current'()
		{
			.get instance object PlayerScript/'<KeepTheHudOff>d__6'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'()
		}
		.property instance object System.Collections.IEnumerator.Current()
		{
			.get instance object PlayerScript/'<KeepTheHudOff>d__6'::System.Collections.IEnumerator.get_Current()
		}

	} // end of class <KeepTheHudOff>d__6


	// Fields
	.field public class GameControllerScript gc
	.field public class BaldiScript baldi
	.field public class DoorScript door
	.field public class PlaytimeScript playtime
	.field public bool gameOver
	.field public bool jumpRope
	.field public bool sweeping
	.field public bool hugging
	.field public bool bootsActive
	.field public int32 principalBugFixer
	.field public float32 sweepingFailsave
	.field public float32 fliparoo
	.field public float32 flipaturn
	.field private valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion playerRotation
	.field public valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 frozenPosition
	.field private bool sensitivityActive
	.field private float32 sensitivity
	.field public float32 mouseSensitivity
	.field public float32 walkSpeed
	.field public float32 runSpeed
	.field public float32 slowSpeed
	.field public float32 maxStamina
	.field public float32 staminaRate
	.field public float32 guilt
	.field public float32 initGuilt
	.field private valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 moveDirection
	.field private float32 playerSpeed
	.field public float32 stamina
	.field public class [UnityEngine.PhysicsModule]UnityEngine.CharacterController cc
	.field public class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent gottaSweep
	.field public class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent firstPrize
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform firstPrizeTransform
	.field public class [UnityEngine.UI]UnityEngine.UI.Slider staminaBar
	.field public float32 db
	.field public string guiltType
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject jumpRopeScreen
	.field public float32 height
	.field public class [UnityEngine.CoreModule]UnityEngine.Material blackSky
	.field public class [UnityEngine.UIModule]UnityEngine.Canvas hud

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x12a78
		// Header size: 12
		// Code size: 206 (0xce)
		.maxstack 2
		.locals init (
			[0] int32,
			[1] int32,
			[2] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[4] float32,
			[5] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[6] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[7] float32
		)

		IL_0000: nop
		IL_0001: ldstr "AnalogMove"
		IL_0006: call int32 [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::GetInt(string)
		IL_000b: stloc.0
		IL_000c: ldloc.0
		IL_000d: stloc.1
		IL_000e: ldloc.0
		IL_000f: ldc.i4.1
		IL_0010: ceq
		IL_0012: ldc.i4.0
		IL_0013: ceq
		IL_0015: brtrue.s IL_001f

		IL_0017: nop
		IL_0018: ldarg.0
		IL_0019: ldloc.1
		IL_001a: stfld bool PlayerScript::sensitivityActive

		IL_001f: nop
		IL_0020: ldarg.0
		IL_0021: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0026: stloc.2
		IL_0027: ldloc.2
		IL_0028: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_002d: stloc.3
		IL_002e: ldarg.0
		IL_002f: ldfld float32 PlayerScript::maxStamina
		IL_0034: stloc.s 4
		IL_0036: ldarg.0
		IL_0037: ldloca.s 3
		IL_0039: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_003e: stfld float32 PlayerScript::height
		IL_0043: ldarg.0
		IL_0044: ldloc.s 4
		IL_0046: stfld float32 PlayerScript::stamina
		IL_004b: ldarg.0
		IL_004c: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0051: stloc.s 5
		IL_0053: ldloc.s 5
		IL_0055: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Transform::get_rotation()
		IL_005a: stloc.s 6
		IL_005c: ldarg.0
		IL_005d: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion PlayerScript::playerRotation
		IL_0062: ldloca.s 6
		IL_0064: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
		IL_0069: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
		IL_006e: ldarg.0
		IL_006f: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion PlayerScript::playerRotation
		IL_0074: ldloca.s 6
		IL_0076: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::y
		IL_007b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::y
		IL_0080: ldarg.0
		IL_0081: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion PlayerScript::playerRotation
		IL_0086: ldloca.s 6
		IL_0088: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::z
		IL_008d: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::z
		IL_0092: ldarg.0
		IL_0093: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion PlayerScript::playerRotation
		IL_0098: ldloca.s 6
		IL_009a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::w
		IL_009f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::w
		IL_00a4: ldstr "MouseSensitivity"
		IL_00a9: call float32 [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::GetFloat(string)
		IL_00ae: stloc.s 7
		IL_00b0: ldarg.0
		IL_00b1: ldloc.s 7
		IL_00b3: stfld float32 PlayerScript::mouseSensitivity
		IL_00b8: ldarg.0
		IL_00b9: ldc.i4.1
		IL_00ba: stfld int32 PlayerScript::principalBugFixer
		IL_00bf: ldarg.0
		IL_00c0: ldc.r4 1
		IL_00c5: stfld float32 PlayerScript::flipaturn
		IL_00ca: ret

		IL_00cb: nop
		IL_00cc: ldnull
		IL_00cd: throw
	} // end of method PlayerScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x12b54
		// Header size: 12
		// Code size: 819 (0x333)
		.maxstack 4
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[3] float32,
			[4] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[5] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[6] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[7] class [UnityEngine.PhysicsModule]UnityEngine.CharacterController,
			[8] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[9] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[10] float32,
			[11] float32,
			[12] float32,
			[13] float32,
			[14] float32,
			[15] class InputManager,
			[16] bool,
			[17] int32,
			[18] float32,
			[19] float32,
			[20] float32,
			[21] float32,
			[22] float32,
			[23] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[24] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[25] float32,
			[26] int32,
			[27] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[28] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[29] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[30] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[31] float32,
			[32] int32,
			[33] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_000e: stloc.1
		IL_000f: ldloc.1
		IL_0010: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0015: stloc.2
		IL_0016: ldarg.0
		IL_0017: ldfld float32 PlayerScript::height
		IL_001c: stloc.3
		IL_001d: ldarg.0
		IL_001e: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0023: stloc.s 4
		IL_0025: ldloc.s 4
		IL_0027: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_002c: stloc.s 5
		IL_002e: ldloc.0
		IL_002f: ldloca.s 6
		IL_0031: ldloca.s 2
		IL_0033: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0038: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_003d: ldloca.s 6
		IL_003f: ldloc.3
		IL_0040: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0045: ldloca.s 6
		IL_0047: ldloca.s 5
		IL_0049: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_004e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0053: ldloc.s 6
		IL_0055: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_005a: ldarg.0
		IL_005b: callvirt instance void PlayerScript::MouseMove()
		IL_0060: ldarg.0
		IL_0061: callvirt instance void PlayerScript::PlayerMove()
		IL_0066: ldarg.0
		IL_0067: ldfld class [UnityEngine.PhysicsModule]UnityEngine.CharacterController PlayerScript::cc
		IL_006c: stloc.s 7
		IL_006e: ldloc.s 7
		IL_0070: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.PhysicsModule]UnityEngine.CharacterController::get_velocity()
		IL_0075: stloc.s 8
		IL_0077: ldloca.s 9
		IL_0079: ldloca.s 8
		IL_007b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0080: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0085: ldloca.s 9
		IL_0087: ldloca.s 8
		IL_0089: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_008e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0093: ldloca.s 9
		IL_0095: ldloc.3
		IL_0096: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_009b: ldloca.s 9
		IL_009d: ldloca.s 8
		IL_009f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00a4: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00a9: ldloca.s 9
		IL_00ab: call instance float32 [UnityEngine.CoreModule]UnityEngine.Vector3::get_magnitude()
		IL_00b0: stloc.s 10
		IL_00b2: ldloc.s 10
		IL_00b4: stloc.s 11
		IL_00b6: ldloc.s 10
		IL_00b8: ldc.r4 0.1
		IL_00bd: cgt
		IL_00bf: brtrue.s IL_010b

		IL_00c1: nop
		IL_00c2: ldarg.0
		IL_00c3: ldfld float32 PlayerScript::maxStamina
		IL_00c8: stloc.s 11
		IL_00ca: ldarg.0
		IL_00cb: ldfld float32 PlayerScript::maxStamina
		IL_00d0: ldarg.0
		IL_00d1: ldfld float32 PlayerScript::stamina
		IL_00d6: cgt
		IL_00d8: ldc.i4.0
		IL_00d9: ceq
		IL_00db: brtrue IL_019c

		IL_00e0: nop
		IL_00e1: ldarg.0
		IL_00e2: ldfld float32 PlayerScript::stamina
		IL_00e7: stloc.s 12
		IL_00e9: ldarg.0
		IL_00ea: ldfld float32 PlayerScript::staminaRate
		IL_00ef: stloc.s 13
		IL_00f1: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_00f6: stloc.s 14
		IL_00f8: ldarg.0
		IL_00f9: ldloc.s 14
		IL_00fb: ldloc.s 13
		IL_00fd: mul
		IL_00fe: ldloc.s 12
		IL_0100: add
		IL_0101: stfld float32 PlayerScript::stamina
		IL_0106: br IL_019c

		IL_010b: nop
		IL_010c: call !0 class Singleton`1<class InputManager>::get_Instance()
		IL_0111: stloc.s 15
		IL_0113: ldloc.s 15
		IL_0115: ldc.i4.s 9
		IL_0117: box [mscorlib]System.Int32
		IL_011c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
		IL_0121: callvirt instance bool InputManager::GetActionKey(valuetype InputAction)
		IL_0126: stloc.s 16
		IL_0128: ldarg.0
		IL_0129: ldfld float32 PlayerScript::stamina
		IL_012e: ldc.r4 0.0
		IL_0133: cgt
		IL_0135: brfalse.s IL_013d

		IL_0137: ldloc.s 16
		IL_0139: stloc.s 17
		IL_013b: br.s IL_0141

		IL_013d: nop
		IL_013e: ldc.i4.0
		IL_013f: stloc.s 17

		IL_0141: nop
		IL_0142: ldarg.0
		IL_0143: ldfld float32 PlayerScript::stamina
		IL_0148: stloc.s 11
		IL_014a: ldloc.s 17
		IL_014c: ldc.i4.0
		IL_014d: ceq
		IL_014f: brtrue.s IL_016f

		IL_0151: nop
		IL_0152: ldarg.0
		IL_0153: ldfld float32 PlayerScript::staminaRate
		IL_0158: stloc.s 18
		IL_015a: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_015f: stloc.s 19
		IL_0161: ldarg.0
		IL_0162: ldloc.s 11
		IL_0164: ldloc.s 19
		IL_0166: ldloc.s 18
		IL_0168: mul
		IL_0169: sub
		IL_016a: stfld float32 PlayerScript::stamina

		IL_016f: nop
		IL_0170: ldc.r4 0.0
		IL_0175: ldarg.0
		IL_0176: ldfld float32 PlayerScript::stamina
		IL_017b: cgt
		IL_017d: ldarg.0
		IL_017e: ldfld float32 PlayerScript::stamina
		IL_0183: ldc.r4 -5
		IL_0188: cgt
		IL_018a: and
		IL_018b: ldc.i4.0
		IL_018c: ceq
		IL_018e: brtrue.s IL_019c

		IL_0190: nop
		IL_0191: ldarg.0
		IL_0192: ldc.r4 -5
		IL_0197: stfld float32 PlayerScript::stamina

		IL_019c: nop
		IL_019d: ldarg.0
		IL_019e: ldfld float32 PlayerScript::stamina
		IL_01a3: ldarg.0
		IL_01a4: ldfld float32 PlayerScript::maxStamina
		IL_01a9: div
		IL_01aa: ldc.r4 100
		IL_01af: mul
		IL_01b0: stloc.s 20
		IL_01b2: ldarg.0
		IL_01b3: ldfld float32 PlayerScript::guilt
		IL_01b8: stloc.s 11
		IL_01ba: ldloc.s 20
		IL_01bc: stloc.s 21
		IL_01be: ldarg.0
		IL_01bf: ldfld float32 PlayerScript::guilt
		IL_01c4: ldc.r4 0.0
		IL_01c9: cgt
		IL_01cb: ldc.i4.0
		IL_01cc: ceq
		IL_01ce: brtrue.s IL_01e7

		IL_01d0: nop
		IL_01d1: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_01d6: stloc.s 22
		IL_01d8: ldarg.0
		IL_01d9: ldloc.s 11
		IL_01db: ldloc.s 22
		IL_01dd: sub
		IL_01de: stfld float32 PlayerScript::guilt
		IL_01e3: ldloc.s 22
		IL_01e5: stloc.s 11

		IL_01e7: nop
		IL_01e8: ldarg.0
		IL_01e9: ldfld class [UnityEngine.PhysicsModule]UnityEngine.CharacterController PlayerScript::cc
		IL_01ee: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.PhysicsModule]UnityEngine.CharacterController::get_velocity()
		IL_01f3: stloc.s 23
		IL_01f5: ldloca.s 24
		IL_01f7: ldloca.s 23
		IL_01f9: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_01fe: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0203: ldloca.s 24
		IL_0205: ldloca.s 23
		IL_0207: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_020c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0211: ldloca.s 24
		IL_0213: ldloca.s 23
		IL_0215: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_021a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_021f: ldloca.s 24
		IL_0221: call instance float32 [UnityEngine.CoreModule]UnityEngine.Vector3::get_magnitude()
		IL_0226: stloc.s 25
		IL_0228: ldloc.s 25
		IL_022a: stloc.s 11
		IL_022c: ldloc.s 25
		IL_022e: ldc.r4 0.0
		IL_0233: cgt
		IL_0235: ldc.i4.0
		IL_0236: ceq
		IL_0238: brtrue.s IL_0246

		IL_023a: nop
		IL_023b: ldarg.0
		IL_023c: ldfld class GameControllerScript PlayerScript::gc
		IL_0241: callvirt instance void GameControllerScript::LockMouse()

		IL_0246: nop
		IL_0247: ldarg.0
		IL_0248: ldfld bool PlayerScript::jumpRope
		IL_024d: stloc.s 26
		IL_024f: ldarg.0
		IL_0250: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0255: stloc.s 27
		IL_0257: ldloc.s 27
		IL_0259: stloc.s 28
		IL_025b: ldloc.s 28
		IL_025d: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0262: stloc.s 29
		IL_0264: ldloca.s 30
		IL_0266: ldloca.s 29
		IL_0268: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_026d: ldarg.0
		IL_026e: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PlayerScript::frozenPosition
		IL_0273: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0278: sub
		IL_0279: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_027e: ldloca.s 30
		IL_0280: ldloca.s 29
		IL_0282: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0287: ldarg.0
		IL_0288: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PlayerScript::frozenPosition
		IL_028d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0292: sub
		IL_0293: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0298: ldloca.s 30
		IL_029a: ldloca.s 29
		IL_029c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_02a1: ldarg.0
		IL_02a2: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PlayerScript::frozenPosition
		IL_02a7: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_02ac: sub
		IL_02ad: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_02b2: ldloca.s 30
		IL_02b4: call instance float32 [UnityEngine.CoreModule]UnityEngine.Vector3::get_magnitude()
		IL_02b9: stloc.s 31
		IL_02bb: ldloc.s 31
		IL_02bd: ldc.r4 1
		IL_02c2: clt
		IL_02c4: ldc.i4.0
		IL_02c5: ceq
		IL_02c7: stloc.s 32
		IL_02c9: ldloc.s 31
		IL_02cb: stloc.s 11
		IL_02cd: ldloc.s 26
		IL_02cf: ldloc.s 31
		IL_02d1: ldc.r4 1
		IL_02d6: clt
		IL_02d8: ldc.i4.0
		IL_02d9: ceq
		IL_02db: and
		IL_02dc: ldc.i4.0
		IL_02dd: ceq
		IL_02df: brtrue.s IL_02f5

		IL_02e1: nop
		IL_02e2: ldarg.0
		IL_02e3: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject PlayerScript::jumpRopeScreen
		IL_02e8: ldc.i4.0
		IL_02e9: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_02ee: ldarg.0
		IL_02ef: ldc.i4.0
		IL_02f0: stfld bool PlayerScript::jumpRope

		IL_02f5: nop
		IL_02f6: ldarg.0
		IL_02f7: ldfld float32 PlayerScript::sweepingFailsave
		IL_02fc: stloc.s 11
		IL_02fe: ldarg.0
		IL_02ff: ldfld float32 PlayerScript::sweepingFailsave
		IL_0304: ldc.r4 0.0
		IL_0309: cgt
		IL_030b: brtrue.s IL_0317

		IL_030d: nop
		IL_030e: ldarg.0
		IL_030f: ldc.i4.0
		IL_0310: stfld bool PlayerScript::sweeping

		IL_0315: nop
		IL_0316: ret

		IL_0317: nop
		IL_0318: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_031d: stloc.s 33
		IL_031f: ldarg.0
		IL_0320: ldloc.s 11
		IL_0322: ldloc.s 33
		IL_0324: sub
		IL_0325: stfld float32 PlayerScript::sweepingFailsave
		IL_032a: ldloc.s 33
		IL_032c: stloc.s 11
		IL_032e: br.s IL_0315

		IL_0330: nop
		IL_0331: ldnull
		IL_0332: throw
	} // end of method PlayerScript::Update

	.method private hidebysig 
		instance void MouseMove () cil managed 
	{
		// Method begins at RVA 0x12e94
		// Header size: 12
		// Code size: 963 (0x3c3)
		.maxstack 3
		.locals init (
			[0] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[4] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[5] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[6] float32,
			[7] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[8] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[9] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[10] float32,
			[11] float32,
			[12] float32,
			[13] float32,
			[14] float32,
			[15] float32,
			[16] float32,
			[17] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[18] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[19] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion PlayerScript::playerRotation
		IL_0007: stloc.0
		IL_0008: ldstr "movaps [rbp-39h],xmm0 ; value = LocalExpr"
		IL_000d: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0012: ldstr "&stack[0x0]"
		IL_0017: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_001c: conv.i8
		IL_001d: ldc.i8 -95
		IL_0026: add
		IL_0027: ldc.i8 -57
		IL_0030: add
		IL_0031: box [mscorlib]System.Int64
		IL_0036: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion>(object)
		IL_003b: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_ToEulerRad(valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion)
		IL_0040: stloc.1
		IL_0041: ldstr "movss dword ptr [rbp-59h],xmm0 ; value = BinaryExpr"
		IL_0046: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_004b: ldstr "movss dword ptr [rbp-55h],xmm0 ; value = BinaryExpr"
		IL_0050: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0055: ldstr "movss dword ptr [rbp-51h],xmm1 ; value = BinaryExpr"
		IL_005a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_005f: ldstr "&stack[0x0]"
		IL_0064: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0069: conv.i8
		IL_006a: ldc.i8 -95
		IL_0073: add
		IL_0074: ldc.i8 -89
		IL_007d: add
		IL_007e: box [mscorlib]System.Int64
		IL_0083: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0088: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_MakePositive(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_008d: stloc.2
		IL_008e: ldarg.0
		IL_008f: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion PlayerScript::playerRotation
		IL_0094: stloc.3
		IL_0095: ldstr "movaps [rbp-39h],xmm0 ; value = LocalExpr"
		IL_009a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_009f: ldstr "&stack[0x0]"
		IL_00a4: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00a9: conv.i8
		IL_00aa: ldc.i8 -95
		IL_00b3: add
		IL_00b4: ldc.i8 -57
		IL_00bd: add
		IL_00be: box [mscorlib]System.Int64
		IL_00c3: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion>(object)
		IL_00c8: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_ToEulerRad(valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion)
		IL_00cd: stloc.s 4
		IL_00cf: ldstr "movss dword ptr [rbp-59h],xmm0 ; value = BinaryExpr"
		IL_00d4: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_00d9: ldstr "movss dword ptr [rbp-55h],xmm0 ; value = BinaryExpr"
		IL_00de: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_00e3: ldstr "movss dword ptr [rbp-51h],xmm1 ; value = BinaryExpr"
		IL_00e8: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_00ed: ldstr "&stack[0x0]"
		IL_00f2: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00f7: conv.i8
		IL_00f8: ldc.i8 -95
		IL_0101: add
		IL_0102: ldc.i8 -89
		IL_010b: add
		IL_010c: box [mscorlib]System.Int64
		IL_0111: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0116: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_MakePositive(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_011b: stloc.s 5
		IL_011d: ldarg.0
		IL_011e: ldfld float32 PlayerScript::fliparoo
		IL_0123: ldc.r4 0.017453292
		IL_0128: mul
		IL_0129: stloc.s 6
		IL_012b: ldstr "movss dword ptr [rbp-55h],xmm0 ; value = BinaryExpr"
		IL_0130: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0135: ldstr "movss dword ptr [rbp-59h],xmm6 ; value = BinaryExpr"
		IL_013a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_013f: ldstr "movss dword ptr [rbp-51h],xmm1 ; value = LocalExpr"
		IL_0144: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0149: ldstr "&stack[0x0]"
		IL_014e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0153: conv.i8
		IL_0154: ldc.i8 -95
		IL_015d: add
		IL_015e: ldc.i8 -89
		IL_0167: add
		IL_0168: box [mscorlib]System.Int64
		IL_016d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0172: call valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_FromEulerRad(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0177: stloc.s 7
		IL_0179: ldarg.0
		IL_017a: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion PlayerScript::playerRotation
		IL_017f: ldloca.s 7
		IL_0181: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
		IL_0186: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
		IL_018b: ldarg.0
		IL_018c: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion PlayerScript::playerRotation
		IL_0191: ldloca.s 7
		IL_0193: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::y
		IL_0198: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::y
		IL_019d: ldarg.0
		IL_019e: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion PlayerScript::playerRotation
		IL_01a3: ldloca.s 7
		IL_01a5: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::z
		IL_01aa: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::z
		IL_01af: ldarg.0
		IL_01b0: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion PlayerScript::playerRotation
		IL_01b5: ldloca.s 7
		IL_01b7: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::w
		IL_01bc: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::w
		IL_01c1: ldstr "movdqa [rbp-39h],xmm0 ; value = StructChunkExpr"
		IL_01c6: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_01cb: ldstr "&stack[0x0]"
		IL_01d0: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01d5: conv.i8
		IL_01d6: ldc.i8 -95
		IL_01df: add
		IL_01e0: ldc.i8 -57
		IL_01e9: add
		IL_01ea: box [mscorlib]System.Int64
		IL_01ef: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion>(object)
		IL_01f4: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_ToEulerRad(valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion)
		IL_01f9: stloc.s 8
		IL_01fb: ldstr "movss dword ptr [rbp-59h],xmm0 ; value = BinaryExpr"
		IL_0200: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0205: ldstr "movss dword ptr [rbp-55h],xmm0 ; value = BinaryExpr"
		IL_020a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_020f: ldstr "movss dword ptr [rbp-51h],xmm1 ; value = BinaryExpr"
		IL_0214: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0219: ldstr "&stack[0x0]"
		IL_021e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0223: conv.i8
		IL_0224: ldc.i8 -95
		IL_022d: add
		IL_022e: ldc.i8 -89
		IL_0237: add
		IL_0238: box [mscorlib]System.Int64
		IL_023d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0242: call valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_MakePositive(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0247: stloc.s 9
		IL_0249: ldstr "movsd qword ptr [rbp-49h],xmm12 ; value = StructChunkExpr"
		IL_024e: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0253: ldstr "movsd qword ptr [rbp-59h],xmm6 ; value = StructChunkExpr"
		IL_0258: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_025d: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::upVector
		IL_0262: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0267: stloc.s 10
		IL_0269: ldstr "Mouse X"
		IL_026e: call float32 [UnityEngine.InputLegacyModule]UnityEngine.Input::GetAxis(string)
		IL_0273: stloc.s 11
		IL_0275: ldarg.0
		IL_0276: ldfld float32 PlayerScript::mouseSensitivity
		IL_027b: stloc.s 12
		IL_027d: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_timeScale()
		IL_0282: stloc.s 13
		IL_0284: ldloc.s 10
		IL_0286: ldloc.s 11
		IL_0288: mul
		IL_0289: ldloc.s 12
		IL_028b: mul
		IL_028c: ldloc.s 13
		IL_028e: mul
		IL_028f: ldarg.0
		IL_0290: ldfld float32 PlayerScript::flipaturn
		IL_0295: mul
		IL_0296: ldloca.s 9
		IL_0298: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_029d: add
		IL_029e: ldc.r4 0.017453292
		IL_02a3: mul
		IL_02a4: stloc.s 14
		IL_02a6: ldarg.0
		IL_02a7: ldfld float32 PlayerScript::flipaturn
		IL_02ac: stloc.s 15
		IL_02ae: ldstr "*(BinaryExpr + 0xFFFFFFFFFFFFFFAB)"
		IL_02b3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_02b8: conv.i4
		IL_02b9: conv.r4
		IL_02ba: ldloc.s 11
		IL_02bc: mul
		IL_02bd: ldloc.s 12
		IL_02bf: mul
		IL_02c0: ldloc.s 13
		IL_02c2: mul
		IL_02c3: ldarg.0
		IL_02c4: ldfld float32 PlayerScript::flipaturn
		IL_02c9: mul
		IL_02ca: ldstr "*(BinaryExpr + 0xFFFFFFFFFFFFFFBB)"
		IL_02cf: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_02d4: conv.i4
		IL_02d5: conv.r4
		IL_02d6: add
		IL_02d7: ldc.r4 0.017453292
		IL_02dc: mul
		IL_02dd: stloc.s 16
		IL_02df: ldstr "movss dword ptr [rbp-59h],xmm6 ; value = BinaryExpr"
		IL_02e4: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_02e9: ldstr "movss dword ptr [rbp-55h],xmm9 ; value = LocalExpr"
		IL_02ee: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_02f3: ldstr "movss dword ptr [rbp-51h],xmm10 ; value = LocalExpr"
		IL_02f8: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_02fd: ldstr "&stack[0x0]"
		IL_0302: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0307: conv.i8
		IL_0308: ldc.i8 -95
		IL_0311: add
		IL_0312: ldc.i8 -89
		IL_031b: add
		IL_031c: box [mscorlib]System.Int64
		IL_0321: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0326: call valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Quaternion::Internal_FromEulerRad(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_032b: stloc.s 17
		IL_032d: ldarg.0
		IL_032e: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion PlayerScript::playerRotation
		IL_0333: ldloca.s 17
		IL_0335: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
		IL_033a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
		IL_033f: ldarg.0
		IL_0340: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion PlayerScript::playerRotation
		IL_0345: ldloca.s 17
		IL_0347: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::y
		IL_034c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::y
		IL_0351: ldarg.0
		IL_0352: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion PlayerScript::playerRotation
		IL_0357: ldloca.s 17
		IL_0359: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::z
		IL_035e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::z
		IL_0363: ldarg.0
		IL_0364: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion PlayerScript::playerRotation
		IL_0369: ldloca.s 17
		IL_036b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::w
		IL_0370: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::w
		IL_0375: ldarg.0
		IL_0376: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_037b: stloc.s 18
		IL_037d: ldarg.0
		IL_037e: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion PlayerScript::playerRotation
		IL_0383: stloc.s 19
		IL_0385: ldstr "movaps [rbp-39h],xmm0 ; value = LocalExpr"
		IL_038a: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_038f: ldloc.s 18
		IL_0391: ldstr "&stack[0x0]"
		IL_0396: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_039b: conv.i8
		IL_039c: ldc.i8 -95
		IL_03a5: add
		IL_03a6: ldc.i8 -57
		IL_03af: add
		IL_03b0: box [mscorlib]System.Int64
		IL_03b5: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion>(object)
		IL_03ba: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_rotation(valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion)
		IL_03bf: ret

		IL_03c0: nop
		IL_03c1: ldnull
		IL_03c2: throw
	} // end of method PlayerScript::MouseMove

	.method private hidebysig 
		instance void PlayerMove () cil managed 
	{
		// Method begins at RVA 0x13264
		// Header size: 12
		// Code size: 1711 (0x6af)
		.maxstack 3
		.locals init (
			[0] int32,
			[1] float32,
			[2] float32,
			[3] int64,
			[4] float32,
			[5] class InputManager,
			[6] float32,
			[7] int64,
			[8] bool,
			[9] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[10] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[11] float32,
			[12] float32,
			[13] float32,
			[14] int32,
			[15] class InputManager,
			[16] bool,
			[17] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[18] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[19] int64,
			[20] int64,
			[21] int64,
			[22] int64,
			[23] class InputManager,
			[24] bool,
			[25] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[26] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[27] int64,
			[28] int64,
			[29] int32,
			[30] float32,
			[31] class InputManager,
			[32] bool,
			[33] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[34] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[35] float32,
			[36] float32,
			[37] float32,
			[38] float32,
			[39] class InputManager,
			[40] bool,
			[41] float32,
			[42] class [UnityEngine.PhysicsModule]UnityEngine.CharacterController,
			[43] class [UnityEngine.PhysicsModule]UnityEngine.CharacterController,
			[44] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[45] float32,
			[46] int32,
			[47] float32,
			[48] float32,
			[49] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[50] int32,
			[51] float32,
			[52] float32,
			[53] float32,
			[54] int64,
			[55] float32,
			[56] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[57] float32,
			[58] float32,
			[59] float32,
			[60] int64,
			[61] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[62] float32,
			[63] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[64] float32,
			[65] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[66] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[67] int32,
			[68] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[69] int32,
			[70] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[71] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[72] float32,
			[73] class [UnityEngine.PhysicsModule]UnityEngine.CharacterController,
			[74] float32,
			[75] valuetype [UnityEngine.PhysicsModule]UnityEngine.CollisionFlags
		)

		IL_0000: nop
		IL_0001: ldstr "global_19E3BFE"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: conv.i4
		IL_000c: stloc.0
		IL_000d: ldstr "global_19E3BFE"
		IL_0012: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0017: conv.i4
		IL_0018: ldc.i4.0
		IL_0019: ceq
		IL_001b: ldc.i4.0
		IL_001c: ceq
		IL_001e: brtrue.s IL_002b

		IL_0020: nop
		IL_0021: ldstr "mov [1819E3BFEh],cl ; value = 0x1"
		IL_0026: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)

		IL_002b: nop
		IL_002c: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::zeroVector
		IL_0031: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0036: stloc.1
		IL_0037: ldloc.1
		IL_0038: stloc.2
		IL_0039: ldstr "struct_chunk(StaticFieldExpr+0x0, 8)"
		IL_003e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0043: stloc.3
		IL_0044: ldc.i4.1
		IL_0045: ldc.i4.0
		IL_0046: ceq
		IL_0048: ldc.i4.0
		IL_0049: ceq
		IL_004b: brtrue.s IL_0058

		IL_004d: nop
		IL_004e: ldstr "mov byte ptr [1819E3BFEh],1 ; value = 0x1"
		IL_0053: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)

		IL_0058: nop
		IL_0059: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::zeroVector
		IL_005e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0063: stloc.s 4
		IL_0065: call !0 class Singleton`1<class InputManager>::get_Instance()
		IL_006a: stloc.s 5
		IL_006c: ldloc.s 4
		IL_006e: stloc.s 6
		IL_0070: ldstr "struct_chunk(StaticFieldExpr+0x0, 8)"
		IL_0075: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_007a: stloc.s 7
		IL_007c: ldloc.s 5
		IL_007e: ldc.i4.2
		IL_007f: box [mscorlib]System.Int32
		IL_0084: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
		IL_0089: callvirt instance bool InputManager::GetActionKey(valuetype InputAction)
		IL_008e: stloc.s 8
		IL_0090: ldloc.s 8
		IL_0092: ldc.i4.0
		IL_0093: ceq
		IL_0095: brtrue.s IL_00c6

		IL_0097: nop
		IL_0098: ldarg.0
		IL_0099: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_009e: stloc.s 9
		IL_00a0: ldloc.s 9
		IL_00a2: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_forward()
		IL_00a7: stloc.s 10
		IL_00a9: ldloca.s 10
		IL_00ab: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00b0: stloc.s 11
		IL_00b2: ldloca.s 10
		IL_00b4: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00b9: stloc.s 12
		IL_00bb: ldloca.s 10
		IL_00bd: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00c2: stloc.s 13
		IL_00c4: br.s IL_00d7

		IL_00c6: nop
		IL_00c7: ldstr "stack[0xFFFFFFFFFFFFFF2C]"
		IL_00cc: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00d1: conv.i4
		IL_00d2: stloc.s 14
		IL_00d4: ldloc.2
		IL_00d5: stloc.s 13

		IL_00d7: nop
		IL_00d8: call !0 class Singleton`1<class InputManager>::get_Instance()
		IL_00dd: stloc.s 15
		IL_00df: ldloc.s 15
		IL_00e1: ldc.i4.3
		IL_00e2: box [mscorlib]System.Int32
		IL_00e7: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
		IL_00ec: callvirt instance bool InputManager::GetActionKey(valuetype InputAction)
		IL_00f1: stloc.s 16
		IL_00f3: ldloc.s 16
		IL_00f5: ldc.i4.0
		IL_00f6: ceq
		IL_00f8: brtrue.s IL_013c

		IL_00fa: nop
		IL_00fb: ldarg.0
		IL_00fc: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0101: stloc.s 17
		IL_0103: ldloc.s 17
		IL_0105: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_forward()
		IL_010a: stloc.s 18
		IL_010c: ldstr "xorps xmm0,xmm6"
		IL_0111: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0116: stloc.s 19
		IL_0118: ldstr "xorps xmm0,xmm6"
		IL_011d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0122: stloc.s 20
		IL_0124: ldstr "xorps xmm10,xmm6"
		IL_0129: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_012e: stloc.s 21
		IL_0130: ldstr "xorps xmm11,xmm6"
		IL_0135: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_013a: stloc.s 22

		IL_013c: nop
		IL_013d: call !0 class Singleton`1<class InputManager>::get_Instance()
		IL_0142: stloc.s 23
		IL_0144: ldloc.s 23
		IL_0146: ldc.i4.0
		IL_0147: box [mscorlib]System.Int32
		IL_014c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
		IL_0151: callvirt instance bool InputManager::GetActionKey(valuetype InputAction)
		IL_0156: stloc.s 24
		IL_0158: ldloc.s 24
		IL_015a: ldc.i4.0
		IL_015b: ceq
		IL_015d: brtrue.s IL_0197

		IL_015f: nop
		IL_0160: ldarg.0
		IL_0161: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0166: stloc.s 25
		IL_0168: ldloc.s 25
		IL_016a: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_right()
		IL_016f: stloc.s 26
		IL_0171: ldstr "xorps xmm7,xmm6"
		IL_0176: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_017b: stloc.s 7
		IL_017d: ldstr "xorps xmm8,xmm6"
		IL_0182: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0187: stloc.s 27
		IL_0189: ldstr "xorps xmm9,xmm6"
		IL_018e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0193: stloc.s 28
		IL_0195: br.s IL_01a9

		IL_0197: nop
		IL_0198: ldstr "stack[0xFFFFFFFFFFFFFF3C]"
		IL_019d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01a2: conv.i4
		IL_01a3: stloc.s 29
		IL_01a5: ldloc.s 6
		IL_01a7: stloc.s 30

		IL_01a9: nop
		IL_01aa: call !0 class Singleton`1<class InputManager>::get_Instance()
		IL_01af: stloc.s 31
		IL_01b1: ldloc.s 31
		IL_01b3: ldc.i4.1
		IL_01b4: box [mscorlib]System.Int32
		IL_01b9: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
		IL_01be: callvirt instance bool InputManager::GetActionKey(valuetype InputAction)
		IL_01c3: stloc.s 32
		IL_01c5: ldloc.s 32
		IL_01c7: ldc.i4.0
		IL_01c8: ceq
		IL_01ca: brtrue.s IL_01f9

		IL_01cc: nop
		IL_01cd: ldarg.0
		IL_01ce: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_01d3: stloc.s 33
		IL_01d5: ldloc.s 33
		IL_01d7: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_right()
		IL_01dc: stloc.s 34
		IL_01de: ldloca.s 34
		IL_01e0: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_01e5: stloc.s 35
		IL_01e7: ldloca.s 34
		IL_01e9: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_01ee: stloc.s 36
		IL_01f0: ldloca.s 34
		IL_01f2: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_01f7: stloc.s 30

		IL_01f9: nop
		IL_01fa: ldarg.0
		IL_01fb: ldfld float32 PlayerScript::stamina
		IL_0200: stloc.s 37
		IL_0202: ldarg.0
		IL_0203: ldfld float32 PlayerScript::stamina
		IL_0208: ldc.r4 0.0
		IL_020d: cgt
		IL_020f: brtrue.s IL_0250

		IL_0211: nop
		IL_0212: ldarg.0
		IL_0213: ldarg.0
		IL_0214: ldfld float32 PlayerScript::walkSpeed
		IL_0219: stfld float32 PlayerScript::playerSpeed
		IL_021e: ldarg.0
		IL_021f: ldfld bool PlayerScript::sensitivityActive
		IL_0224: ldc.i4.0
		IL_0225: ceq
		IL_0227: ldc.i4.0
		IL_0228: ceq
		IL_022a: brtrue.s IL_023d

		IL_022c: nop
		IL_022d: ldarg.0
		IL_022e: ldc.r4 1
		IL_0233: stfld float32 PlayerScript::sensitivity
		IL_0238: br IL_03b5

		IL_023d: nop
		IL_023e: ldloc.s 21
		IL_0240: conv.r4
		IL_0241: ldloc.s 36
		IL_0243: add
		IL_0244: stloc.s 37
		IL_0246: ldloc.s 22
		IL_0248: conv.r4
		IL_0249: ldloc.s 30
		IL_024b: add
		IL_024c: stloc.s 38
		IL_024e: br.s IL_02b7

		IL_0250: nop
		IL_0251: call !0 class Singleton`1<class InputManager>::get_Instance()
		IL_0256: stloc.s 39
		IL_0258: ldloc.s 39
		IL_025a: ldc.i4.s 9
		IL_025c: box [mscorlib]System.Int32
		IL_0261: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
		IL_0266: callvirt instance bool InputManager::GetActionKey(valuetype InputAction)
		IL_026b: stloc.s 40
		IL_026d: ldloc.s 40
		IL_026f: ldc.i4.0
		IL_0270: ceq
		IL_0272: ldc.i4.0
		IL_0273: ceq
		IL_0275: brtrue IL_0313

		IL_027a: nop
		IL_027b: ldarg.0
		IL_027c: ldarg.0
		IL_027d: ldfld float32 PlayerScript::walkSpeed
		IL_0282: stfld float32 PlayerScript::playerSpeed
		IL_0287: ldarg.0
		IL_0288: ldfld bool PlayerScript::sensitivityActive
		IL_028d: ldc.i4.0
		IL_028e: ceq
		IL_0290: ldc.i4.0
		IL_0291: ceq
		IL_0293: brtrue.s IL_02a6

		IL_0295: nop
		IL_0296: ldarg.0
		IL_0297: ldc.r4 1
		IL_029c: stfld float32 PlayerScript::sensitivity
		IL_02a1: br IL_03b5

		IL_02a6: nop
		IL_02a7: ldloc.s 36
		IL_02a9: ldloc.s 21
		IL_02ab: conv.r4
		IL_02ac: add
		IL_02ad: stloc.s 37
		IL_02af: ldloc.s 30
		IL_02b1: ldloc.s 22
		IL_02b3: conv.r4
		IL_02b4: add
		IL_02b5: stloc.s 38

		IL_02b7: nop
		IL_02b8: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_02bd: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_02c2: conv.i8
		IL_02c3: ldc.i8 -121
		IL_02cc: add
		IL_02cd: call instance float32 [UnityEngine.CoreModule]UnityEngine.Vector3::get_magnitude()
		IL_02d2: stloc.s 41
		IL_02d4: ldloc.s 41
		IL_02d6: stloc.s 37
		IL_02d8: ldc.r4 0.0
		IL_02dd: ldloc.s 41
		IL_02df: cgt
		IL_02e1: brtrue.s IL_0301

		IL_02e3: nop
		IL_02e4: ldloc.s 37
		IL_02e6: ldc.r4 1
		IL_02eb: cgt
		IL_02ed: brtrue.s IL_0302

		IL_02ef: nop
		IL_02f0: ldarg.0
		IL_02f1: ldloc.s 37
		IL_02f3: stfld float32 PlayerScript::sensitivity
		IL_02f8: ldloc.s 37
		IL_02fa: stloc.s 38
		IL_02fc: br IL_03b5

		IL_0301: nop

		IL_0302: nop
		IL_0303: ldarg.0
		IL_0304: ldc.r4 0.0
		IL_0309: stfld float32 PlayerScript::sensitivity
		IL_030e: br IL_03b5

		IL_0313: nop
		IL_0314: ldarg.0
		IL_0315: ldfld class [UnityEngine.PhysicsModule]UnityEngine.CharacterController PlayerScript::cc
		IL_031a: stloc.s 42
		IL_031c: ldarg.0
		IL_031d: ldarg.0
		IL_031e: ldfld float32 PlayerScript::runSpeed
		IL_0323: stfld float32 PlayerScript::playerSpeed
		IL_0328: ldarg.0
		IL_0329: ldc.r4 1
		IL_032e: stfld float32 PlayerScript::sensitivity
		IL_0333: ldloc.s 42
		IL_0335: stloc.s 43
		IL_0337: ldloc.s 43
		IL_0339: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.PhysicsModule]UnityEngine.CharacterController::get_velocity()
		IL_033e: stloc.s 44
		IL_0340: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_0345: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_034a: conv.i8
		IL_034b: ldc.i8 -121
		IL_0354: add
		IL_0355: call instance float32 [UnityEngine.CoreModule]UnityEngine.Vector3::get_magnitude()
		IL_035a: stloc.s 45
		IL_035c: ldloc.s 45
		IL_035e: ldc.r4 0.1
		IL_0363: cgt
		IL_0365: brfalse.s IL_037e

		IL_0367: ldarg.0
		IL_0368: ldfld bool PlayerScript::hugging
		IL_036d: ldc.i4.0
		IL_036e: ceq
		IL_0370: ldarg.0
		IL_0371: ldfld bool PlayerScript::sweeping
		IL_0376: ldc.i4.0
		IL_0377: ceq
		IL_0379: and
		IL_037a: stloc.s 46
		IL_037c: br.s IL_0382

		IL_037e: nop
		IL_037f: ldc.i4.0
		IL_0380: stloc.s 46

		IL_0382: nop
		IL_0383: ldloc.s 45
		IL_0385: stloc.s 37
		IL_0387: ldloc.s 46
		IL_0389: ldc.i4.0
		IL_038a: ceq
		IL_038c: brtrue.s IL_03b5

		IL_038e: nop
		IL_038f: ldc.r4 0.1
		IL_0394: ldarg.0
		IL_0395: ldfld float32 PlayerScript::guilt
		IL_039a: clt
		IL_039c: brtrue.s IL_03b5

		IL_039e: nop
		IL_039f: ldarg.0
		IL_03a0: ldstr "running"
		IL_03a5: stfld string PlayerScript::guiltType
		IL_03aa: ldarg.0
		IL_03ab: ldc.r4 0.1
		IL_03b0: stfld float32 PlayerScript::guilt

		IL_03b5: nop
		IL_03b6: ldarg.0
		IL_03b7: ldfld float32 PlayerScript::playerSpeed
		IL_03bc: stloc.s 47
		IL_03be: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_03c3: stloc.s 48
		IL_03c5: ldarg.0
		IL_03c6: ldloc.s 48
		IL_03c8: ldloc.s 47
		IL_03ca: mul
		IL_03cb: stfld float32 PlayerScript::playerSpeed
		IL_03d0: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_03d5: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_03da: conv.i8
		IL_03db: ldc.i8 -121
		IL_03e4: add
		IL_03e5: call instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::get_normalized()
		IL_03ea: stloc.s 49
		IL_03ec: ldarg.0
		IL_03ed: ldfld bool PlayerScript::jumpRope
		IL_03f2: ldc.i4.0
		IL_03f3: ceq
		IL_03f5: brfalse.s IL_040e

		IL_03f7: ldarg.0
		IL_03f8: ldfld bool PlayerScript::hugging
		IL_03fd: ldc.i4.0
		IL_03fe: ceq
		IL_0400: ldarg.0
		IL_0401: ldfld bool PlayerScript::sweeping
		IL_0406: ldc.i4.0
		IL_0407: ceq
		IL_0409: and
		IL_040a: stloc.s 50
		IL_040c: br.s IL_0412

		IL_040e: nop
		IL_040f: ldc.i4.0
		IL_0410: stloc.s 50

		IL_0412: nop
		IL_0413: ldarg.0
		IL_0414: ldfld float32 PlayerScript::sensitivity
		IL_0419: stloc.s 51
		IL_041b: ldarg.0
		IL_041c: ldfld float32 PlayerScript::playerSpeed
		IL_0421: ldloca.s 49
		IL_0423: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0428: mul
		IL_0429: ldarg.0
		IL_042a: ldfld float32 PlayerScript::sensitivity
		IL_042f: mul
		IL_0430: stloc.s 52
		IL_0432: ldarg.0
		IL_0433: ldfld float32 PlayerScript::playerSpeed
		IL_0438: ldloca.s 49
		IL_043a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_043f: mul
		IL_0440: ldarg.0
		IL_0441: ldfld float32 PlayerScript::sensitivity
		IL_0446: mul
		IL_0447: stloc.s 53
		IL_0449: ldarg.0
		IL_044a: ldstr "unpcklps xmm2,xmm0"
		IL_044f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0454: box [mscorlib]System.Int64
		IL_0459: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_045e: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PlayerScript::moveDirection
		IL_0463: ldarg.0
		IL_0464: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PlayerScript::moveDirection
		IL_0469: ldloc.s 52
		IL_046b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0470: ldloc.s 51
		IL_0472: stloc.s 38
		IL_0474: ldstr "unpcklps xmm2,xmm0"
		IL_0479: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_047e: stloc.s 54
		IL_0480: ldloc.s 52
		IL_0482: stloc.s 55
		IL_0484: ldloc.s 53
		IL_0486: stloc.s 37
		IL_0488: ldloc.s 50
		IL_048a: ldc.i4.0
		IL_048b: ceq
		IL_048d: ldc.i4.0
		IL_048e: ceq
		IL_0490: brtrue IL_066d

		IL_0495: nop
		IL_0496: ldarg.0
		IL_0497: ldfld bool PlayerScript::sweeping
		IL_049c: ldc.i4.0
		IL_049d: ceq
		IL_049f: brtrue IL_0543

		IL_04a4: nop
		IL_04a5: ldarg.0
		IL_04a6: ldfld bool PlayerScript::bootsActive
		IL_04ab: ldc.i4.0
		IL_04ac: ceq
		IL_04ae: ldc.i4.0
		IL_04af: ceq
		IL_04b1: brtrue IL_0543

		IL_04b6: nop
		IL_04b7: ldarg.0
		IL_04b8: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PlayerScript::gottaSweep
		IL_04bd: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::get_velocity()
		IL_04c2: stloc.s 56
		IL_04c4: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_04c9: stloc.s 57
		IL_04cb: ldarg.0
		IL_04cc: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PlayerScript::moveDirection
		IL_04d1: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_04d6: ldc.r4 0.3
		IL_04db: mul
		IL_04dc: ldloca.s 56
		IL_04de: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_04e3: ldloc.s 57
		IL_04e5: mul
		IL_04e6: add
		IL_04e7: stloc.s 58
		IL_04e9: ldarg.0
		IL_04ea: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PlayerScript::moveDirection
		IL_04ef: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_04f4: ldc.r4 0.3
		IL_04f9: mul
		IL_04fa: ldloca.s 56
		IL_04fc: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0501: ldloc.s 57
		IL_0503: mul
		IL_0504: add
		IL_0505: stloc.s 59
		IL_0507: ldarg.0
		IL_0508: ldstr "unpcklps xmm3,xmm1"
		IL_050d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0512: box [mscorlib]System.Int64
		IL_0517: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_051c: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PlayerScript::moveDirection
		IL_0521: ldarg.0
		IL_0522: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PlayerScript::moveDirection
		IL_0527: ldloc.s 58
		IL_0529: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_052e: ldloc.s 59
		IL_0530: stloc.s 38
		IL_0532: ldstr "unpcklps xmm3,xmm1"
		IL_0537: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_053c: stloc.s 60
		IL_053e: br IL_066d

		IL_0543: nop
		IL_0544: ldarg.0
		IL_0545: ldfld bool PlayerScript::hugging
		IL_054a: ldc.i4.0
		IL_054b: ceq
		IL_054d: brtrue IL_063f

		IL_0552: nop
		IL_0553: ldarg.0
		IL_0554: ldfld bool PlayerScript::bootsActive
		IL_0559: ldc.i4.0
		IL_055a: ceq
		IL_055c: ldc.i4.0
		IL_055d: ceq
		IL_055f: brtrue IL_063f

		IL_0564: nop
		IL_0565: ldarg.0
		IL_0566: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PlayerScript::firstPrize
		IL_056b: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::get_velocity()
		IL_0570: stloc.s 61
		IL_0572: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0577: stloc.s 62
		IL_0579: ldarg.0
		IL_057a: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PlayerScript::firstPrizeTransform
		IL_057f: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0584: stloc.s 63
		IL_0586: ldarg.0
		IL_0587: ldfld float32 PlayerScript::height
		IL_058c: stloc.s 64
		IL_058e: ldarg.0
		IL_058f: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PlayerScript::firstPrizeTransform
		IL_0594: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0599: stloc.s 65
		IL_059b: ldarg.0
		IL_059c: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PlayerScript::firstPrizeTransform
		IL_05a1: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_forward()
		IL_05a6: stloc.s 66
		IL_05a8: ldloca.s 66
		IL_05aa: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_05af: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_05b4: stloc.s 67
		IL_05b6: ldarg.0
		IL_05b7: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform PlayerScript::firstPrizeTransform
		IL_05bc: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_forward()
		IL_05c1: stloc.s 68
		IL_05c3: ldloca.s 68
		IL_05c5: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_05ca: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_05cf: stloc.s 69
		IL_05d1: ldarg.0
		IL_05d2: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_05d7: stloc.s 70
		IL_05d9: ldloc.s 70
		IL_05db: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_05e0: stloc.s 71
		IL_05e2: ldloc.s 69
		IL_05e4: conv.r4
		IL_05e5: ldc.r4 3
		IL_05ea: mul
		IL_05eb: ldloca.s 65
		IL_05ed: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_05f2: add
		IL_05f3: ldloca.s 71
		IL_05f5: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_05fa: sub
		IL_05fb: ldloca.s 61
		IL_05fd: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0602: ldc.r4 1.2
		IL_0607: mul
		IL_0608: ldloc.s 62
		IL_060a: mul
		IL_060b: add
		IL_060c: ldarg.0
		IL_060d: ldfld int32 PlayerScript::principalBugFixer
		IL_0612: conv.r4
		IL_0613: mul
		IL_0614: stloc.s 72
		IL_0616: ldarg.0
		IL_0617: ldstr "unpcklps xmm7,xmm9"
		IL_061c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0621: box [mscorlib]System.Int64
		IL_0626: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_062b: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PlayerScript::moveDirection
		IL_0630: ldarg.0
		IL_0631: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PlayerScript::moveDirection
		IL_0636: ldloc.s 72
		IL_0638: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_063d: br.s IL_066d

		IL_063f: nop
		IL_0640: ldarg.0
		IL_0641: ldfld bool PlayerScript::jumpRope
		IL_0646: ldc.i4.0
		IL_0647: ceq
		IL_0649: brtrue.s IL_066d

		IL_064b: nop
		IL_064c: ldarg.0
		IL_064d: ldc.i4.0
		IL_064e: box [mscorlib]System.Int32
		IL_0653: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0658: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PlayerScript::moveDirection
		IL_065d: ldarg.0
		IL_065e: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PlayerScript::moveDirection
		IL_0663: ldc.r4 0.0
		IL_0668: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z

		IL_066d: nop
		IL_066e: ldarg.0
		IL_066f: ldfld class [UnityEngine.PhysicsModule]UnityEngine.CharacterController PlayerScript::cc
		IL_0674: stloc.s 73
		IL_0676: ldarg.0
		IL_0677: ldfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PlayerScript::moveDirection
		IL_067c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0681: stloc.s 74
		IL_0683: ldloc.s 73
		IL_0685: ldstr "&stack[0xFFFFFFFFFFFFFFA1]"
		IL_068a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_068f: conv.i8
		IL_0690: ldc.i8 -121
		IL_0699: add
		IL_069a: box [mscorlib]System.Int64
		IL_069f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_06a4: callvirt instance valuetype [UnityEngine.PhysicsModule]UnityEngine.CollisionFlags [UnityEngine.PhysicsModule]UnityEngine.CharacterController::Move(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_06a9: stloc.s 75
		IL_06ab: ret

		IL_06ac: nop
		IL_06ad: ldnull
		IL_06ae: throw
	} // end of method PlayerScript::PlayerMove

	.method private hidebysig 
		instance void StaminaCheck () cil managed 
	{
		// Method begins at RVA 0x13920
		// Header size: 12
		// Code size: 301 (0x12d)
		.maxstack 4
		.locals init (
			[0] class [UnityEngine.PhysicsModule]UnityEngine.CharacterController,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[3] float32,
			[4] float32,
			[5] float32,
			[6] float32,
			[7] float32,
			[8] class InputManager,
			[9] bool,
			[10] int32,
			[11] float32,
			[12] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.PhysicsModule]UnityEngine.CharacterController PlayerScript::cc
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.PhysicsModule]UnityEngine.CharacterController::get_velocity()
		IL_000e: stloc.1
		IL_000f: ldloca.s 2
		IL_0011: ldloca.s 1
		IL_0013: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0018: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_001d: ldloca.s 2
		IL_001f: ldloca.s 1
		IL_0021: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0026: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_002b: ldloca.s 2
		IL_002d: ldloca.s 1
		IL_002f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0034: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0039: ldloca.s 2
		IL_003b: call instance float32 [UnityEngine.CoreModule]UnityEngine.Vector3::get_magnitude()
		IL_0040: stloc.3
		IL_0041: ldloc.3
		IL_0042: stloc.s 4
		IL_0044: ldloc.3
		IL_0045: ldc.r4 0.1
		IL_004a: cgt
		IL_004c: brtrue.s IL_0098

		IL_004e: nop
		IL_004f: ldarg.0
		IL_0050: ldfld float32 PlayerScript::maxStamina
		IL_0055: stloc.s 4
		IL_0057: ldarg.0
		IL_0058: ldfld float32 PlayerScript::maxStamina
		IL_005d: ldarg.0
		IL_005e: ldfld float32 PlayerScript::stamina
		IL_0063: cgt
		IL_0065: ldc.i4.0
		IL_0066: ceq
		IL_0068: brtrue IL_0129

		IL_006d: nop
		IL_006e: ldarg.0
		IL_006f: ldfld float32 PlayerScript::stamina
		IL_0074: stloc.s 5
		IL_0076: ldarg.0
		IL_0077: ldfld float32 PlayerScript::staminaRate
		IL_007c: stloc.s 6
		IL_007e: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0083: stloc.s 7
		IL_0085: ldarg.0
		IL_0086: ldloc.s 7
		IL_0088: ldloc.s 6
		IL_008a: mul
		IL_008b: ldloc.s 5
		IL_008d: add
		IL_008e: stfld float32 PlayerScript::stamina
		IL_0093: br IL_0129

		IL_0098: nop
		IL_0099: call !0 class Singleton`1<class InputManager>::get_Instance()
		IL_009e: stloc.s 8
		IL_00a0: ldloc.s 8
		IL_00a2: ldc.i4.s 9
		IL_00a4: box [mscorlib]System.Int32
		IL_00a9: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
		IL_00ae: callvirt instance bool InputManager::GetActionKey(valuetype InputAction)
		IL_00b3: stloc.s 9
		IL_00b5: ldarg.0
		IL_00b6: ldfld float32 PlayerScript::stamina
		IL_00bb: ldc.r4 0.0
		IL_00c0: cgt
		IL_00c2: brfalse.s IL_00ca

		IL_00c4: ldloc.s 9
		IL_00c6: stloc.s 10
		IL_00c8: br.s IL_00ce

		IL_00ca: nop
		IL_00cb: ldc.i4.0
		IL_00cc: stloc.s 10

		IL_00ce: nop
		IL_00cf: ldarg.0
		IL_00d0: ldfld float32 PlayerScript::stamina
		IL_00d5: stloc.s 4
		IL_00d7: ldloc.s 10
		IL_00d9: ldc.i4.0
		IL_00da: ceq
		IL_00dc: brtrue.s IL_00fc

		IL_00de: nop
		IL_00df: ldarg.0
		IL_00e0: ldfld float32 PlayerScript::staminaRate
		IL_00e5: stloc.s 11
		IL_00e7: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_00ec: stloc.s 12
		IL_00ee: ldarg.0
		IL_00ef: ldloc.s 4
		IL_00f1: ldloc.s 12
		IL_00f3: ldloc.s 11
		IL_00f5: mul
		IL_00f6: sub
		IL_00f7: stfld float32 PlayerScript::stamina

		IL_00fc: nop
		IL_00fd: ldc.r4 0.0
		IL_0102: ldarg.0
		IL_0103: ldfld float32 PlayerScript::stamina
		IL_0108: cgt
		IL_010a: ldarg.0
		IL_010b: ldfld float32 PlayerScript::stamina
		IL_0110: ldc.r4 -5
		IL_0115: cgt
		IL_0117: and
		IL_0118: ldc.i4.0
		IL_0119: ceq
		IL_011b: brtrue.s IL_0129

		IL_011d: nop
		IL_011e: ldarg.0
		IL_011f: ldc.r4 -5
		IL_0124: stfld float32 PlayerScript::stamina

		IL_0129: nop
		IL_012a: nop
		IL_012b: ldnull
		IL_012c: throw
	} // end of method PlayerScript::StaminaCheck

	.method private hidebysig 
		instance void OnTriggerEnter (
			class [UnityEngine.PhysicsModule]UnityEngine.Collider other
		) cil managed 
	{
		// Method begins at RVA 0x13a5c
		// Header size: 12
		// Code size: 350 (0x15e)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] string,
			[2] bool,
			[3] class GameControllerScript,
			[4] int32,
			[5] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[6] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[7] string,
			[8] bool,
			[9] bool,
			[10] int32,
			[11] int32,
			[12] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[13] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[14] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[15] int64,
			[16] class [UnityEngine.CoreModule]UnityEngine.Material,
			[17] class PlayerScript/'<KeepTheHudOff>d__6',
			[18] class [UnityEngine.CoreModule]UnityEngine.Coroutine
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_000e: stloc.1
		IL_000f: ldloc.1
		IL_0010: ldstr "Baldi"
		IL_0015: call bool [mscorlib]System.String::Equals(string, string)
		IL_001a: stloc.2
		IL_001b: ldarg.0
		IL_001c: ldfld class GameControllerScript PlayerScript::gc
		IL_0021: stloc.3
		IL_0022: ldloc.3
		IL_0023: ldfld bool GameControllerScript::debugMode
		IL_0028: ldc.i4.0
		IL_0029: ceq
		IL_002b: brfalse.s IL_0032

		IL_002d: ldloc.2
		IL_002e: stloc.s 4
		IL_0030: br.s IL_0036

		IL_0032: nop
		IL_0033: ldc.i4.0
		IL_0034: stloc.s 4

		IL_0036: nop
		IL_0037: ldloc.s 4
		IL_0039: ldc.i4.0
		IL_003a: ceq
		IL_003c: ldc.i4.0
		IL_003d: ceq
		IL_003f: brtrue IL_0110

		IL_0044: nop
		IL_0045: ldarg.1
		IL_0046: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_004b: stloc.s 5
		IL_004d: ldloc.s 5
		IL_004f: stloc.s 6
		IL_0051: ldloc.s 6
		IL_0053: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_0058: stloc.s 7
		IL_005a: ldloc.s 7
		IL_005c: ldstr "Playtime"
		IL_0061: call bool [mscorlib]System.String::Equals(string, string)
		IL_0066: stloc.s 8
		IL_0068: ldloc.s 8
		IL_006a: stloc.s 9
		IL_006c: ldarg.0
		IL_006d: ldfld bool PlayerScript::jumpRope
		IL_0072: ldc.i4.0
		IL_0073: ceq
		IL_0075: brfalse.s IL_007d

		IL_0077: ldloc.s 9
		IL_0079: stloc.s 10
		IL_007b: br.s IL_0081

		IL_007d: nop
		IL_007e: ldc.i4.0
		IL_007f: stloc.s 10

		IL_0081: nop
		IL_0082: ldloc.s 9
		IL_0084: stloc.s 11
		IL_0086: ldloc.s 10
		IL_0088: ldc.r4 0.0
		IL_008d: ldarg.0
		IL_008e: ldfld class PlaytimeScript PlayerScript::playtime
		IL_0093: ldfld float32 PlaytimeScript::playCool
		IL_0098: clt
		IL_009a: ldc.i4.0
		IL_009b: ceq
		IL_009d: and
		IL_009e: ldc.i4.0
		IL_009f: ceq
		IL_00a1: brtrue.s IL_010e

		IL_00a3: nop
		IL_00a4: ldarg.0
		IL_00a5: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject PlayerScript::jumpRopeScreen
		IL_00aa: ldc.i4.1
		IL_00ab: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_00b0: ldarg.0
		IL_00b1: ldc.i4.1
		IL_00b2: stfld bool PlayerScript::jumpRope
		IL_00b7: ldarg.0
		IL_00b8: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_00bd: stloc.s 12
		IL_00bf: ldloc.s 12
		IL_00c1: stloc.s 13
		IL_00c3: ldloc.s 13
		IL_00c5: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_00ca: stloc.s 14
		IL_00cc: ldarg.0
		IL_00cd: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PlayerScript::frozenPosition
		IL_00d2: ldloca.s 14
		IL_00d4: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00d9: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_00de: ldarg.0
		IL_00df: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PlayerScript::frozenPosition
		IL_00e4: ldloca.s 14
		IL_00e6: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00eb: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00f0: ldarg.0
		IL_00f1: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PlayerScript::frozenPosition
		IL_00f6: ldloca.s 14
		IL_00f8: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_00fd: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0102: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_0107: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_010c: stloc.s 15

		IL_010e: nop
		IL_010f: ret

		IL_0110: nop
		IL_0111: ldarg.0
		IL_0112: ldfld class [UnityEngine.CoreModule]UnityEngine.Material PlayerScript::blackSky
		IL_0117: stloc.s 16
		IL_0119: ldarg.0
		IL_011a: ldc.i4.1
		IL_011b: stfld bool PlayerScript::gameOver
		IL_0120: ldloc.s 16
		IL_0122: call void [UnityEngine.CoreModule]UnityEngine.RenderSettings::set_skybox(class [UnityEngine.CoreModule]UnityEngine.Material)
		IL_0127: ldstr "il2cpp_codegen_object_new(PlayerScript+<KeepTheHudOff>d__6)"
		IL_012c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0131: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class PlayerScript/'<KeepTheHudOff>d__6'>(object)
		IL_0136: stloc.s 17
		IL_0138: ldloc.s 17
		IL_013a: ldnull
		IL_013b: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0140: ldloc.s 17
		IL_0142: ldc.i4.0
		IL_0143: stfld int32 PlayerScript/'<KeepTheHudOff>d__6'::'<>1__state'
		IL_0148: ldloc.s 17
		IL_014a: ldarg.0
		IL_014b: stfld class PlayerScript PlayerScript/'<KeepTheHudOff>d__6'::'<>4__this'
		IL_0150: ldarg.0
		IL_0151: ldloc.s 17
		IL_0153: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Coroutine [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StartCoroutine(class [mscorlib]System.Collections.IEnumerator)
		IL_0158: stloc.s 18
		IL_015a: ret

		IL_015b: nop
		IL_015c: ldnull
		IL_015d: throw
	} // end of method PlayerScript::OnTriggerEnter

	.method public hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator KeepTheHudOff () cil managed 
	{
		// Method begins at RVA 0x13bc8
		// Header size: 12
		// Code size: 40 (0x28)
		.maxstack 2
		.locals init (
			[0] class PlayerScript/'<KeepTheHudOff>d__6'
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(PlayerScript+<KeepTheHudOff>d__6)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class PlayerScript/'<KeepTheHudOff>d__6'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 PlayerScript/'<KeepTheHudOff>d__6'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class PlayerScript PlayerScript/'<KeepTheHudOff>d__6'::'<>4__this'
		IL_0026: ldloc.0
		IL_0027: ret
	} // end of method PlayerScript::KeepTheHudOff

	.method private hidebysig 
		instance void OnTriggerStay (
			class [UnityEngine.PhysicsModule]UnityEngine.Collider other
		) cil managed 
	{
		// Method begins at RVA 0x13bfc
		// Header size: 12
		// Code size: 225 (0xe1)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[2] string,
			[3] bool,
			[4] bool,
			[5] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[6] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[7] string,
			[8] bool,
			[9] bool,
			[10] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[11] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[12] float32,
			[13] int32,
			[14] float32
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: stloc.1
		IL_000a: ldloc.1
		IL_000b: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_0010: stloc.2
		IL_0011: ldloc.2
		IL_0012: ldstr "Gotta Sweep"
		IL_0017: call bool [mscorlib]System.String::Equals(string, string)
		IL_001c: stloc.3
		IL_001d: ldloc.3
		IL_001e: stloc.s 4
		IL_0020: ldloc.3
		IL_0021: ldc.i4.0
		IL_0022: ceq
		IL_0024: ldc.i4.0
		IL_0025: ceq
		IL_0027: brtrue IL_00ca

		IL_002c: nop
		IL_002d: ldarg.1
		IL_002e: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0033: stloc.s 5
		IL_0035: ldloc.s 5
		IL_0037: stloc.s 6
		IL_0039: ldloc.s 6
		IL_003b: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_0040: stloc.s 7
		IL_0042: ldloc.s 7
		IL_0044: ldstr "1st Prize"
		IL_0049: call bool [mscorlib]System.String::Equals(string, string)
		IL_004e: stloc.s 8
		IL_0050: ldloc.s 8
		IL_0052: stloc.s 9
		IL_0054: ldarg.0
		IL_0055: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent PlayerScript::firstPrize
		IL_005a: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::get_velocity()
		IL_005f: stloc.s 10
		IL_0061: ldloca.s 11
		IL_0063: ldloca.s 10
		IL_0065: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_006a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_006f: ldloca.s 11
		IL_0071: ldloca.s 10
		IL_0073: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0078: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_007d: ldloca.s 11
		IL_007f: ldloca.s 10
		IL_0081: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0086: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_008b: ldloca.s 11
		IL_008d: call instance float32 [UnityEngine.CoreModule]UnityEngine.Vector3::get_magnitude()
		IL_0092: stloc.s 12
		IL_0094: ldloc.s 12
		IL_0096: ldc.r4 5
		IL_009b: cgt
		IL_009d: brfalse.s IL_00a5

		IL_009f: ldloc.s 8
		IL_00a1: stloc.s 13
		IL_00a3: br.s IL_00a9

		IL_00a5: nop
		IL_00a6: ldc.i4.0
		IL_00a7: stloc.s 13

		IL_00a9: nop
		IL_00aa: ldloc.s 12
		IL_00ac: stloc.s 14
		IL_00ae: ldloc.s 13
		IL_00b0: ldc.i4.0
		IL_00b1: ceq
		IL_00b3: brtrue.s IL_00c8

		IL_00b5: nop
		IL_00b6: ldarg.0
		IL_00b7: ldc.i4.1
		IL_00b8: stfld bool PlayerScript::hugging
		IL_00bd: ldarg.0
		IL_00be: ldc.r4 1
		IL_00c3: stfld float32 PlayerScript::sweepingFailsave

		IL_00c8: nop
		IL_00c9: ret

		IL_00ca: nop
		IL_00cb: ldarg.0
		IL_00cc: ldc.i4.1
		IL_00cd: stfld bool PlayerScript::sweeping
		IL_00d2: ldarg.0
		IL_00d3: ldc.r4 1
		IL_00d8: stfld float32 PlayerScript::sweepingFailsave
		IL_00dd: ret

		IL_00de: nop
		IL_00df: ldnull
		IL_00e0: throw
	} // end of method PlayerScript::OnTriggerStay

	.method private hidebysig 
		instance void OnTriggerExit (
			class [UnityEngine.PhysicsModule]UnityEngine.Collider other
		) cil managed 
	{
		// Method begins at RVA 0x13cec
		// Header size: 12
		// Code size: 209 (0xd1)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] string,
			[2] bool,
			[3] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[4] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[5] string,
			[6] bool,
			[7] bool,
			[8] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[9] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[10] string,
			[11] bool,
			[12] bool,
			[13] class DoorScript,
			[14] float32
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_000e: stloc.1
		IL_000f: ldloc.1
		IL_0010: ldstr "Office Trigger"
		IL_0015: call bool [mscorlib]System.String::Equals(string, string)
		IL_001a: stloc.2
		IL_001b: ldloc.2
		IL_001c: ldc.i4.0
		IL_001d: ceq
		IL_001f: ldc.i4.0
		IL_0020: ceq
		IL_0022: brtrue.s IL_0096

		IL_0024: nop
		IL_0025: ldarg.1
		IL_0026: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_002b: stloc.3
		IL_002c: ldloc.3
		IL_002d: stloc.s 4
		IL_002f: ldloc.s 4
		IL_0031: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_0036: stloc.s 5
		IL_0038: ldloc.s 5
		IL_003a: ldstr "Gotta Sweep"
		IL_003f: call bool [mscorlib]System.String::Equals(string, string)
		IL_0044: stloc.s 6
		IL_0046: ldloc.s 6
		IL_0048: stloc.s 7
		IL_004a: ldloc.s 6
		IL_004c: ldc.i4.0
		IL_004d: ceq
		IL_004f: ldc.i4.0
		IL_0050: ceq
		IL_0052: brtrue.s IL_008d

		IL_0054: nop
		IL_0055: ldarg.1
		IL_0056: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_005b: stloc.s 8
		IL_005d: ldloc.s 8
		IL_005f: stloc.s 9
		IL_0061: ldloc.s 9
		IL_0063: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_0068: stloc.s 10
		IL_006a: ldloc.s 10
		IL_006c: ldstr "1st Prize"
		IL_0071: call bool [mscorlib]System.String::Equals(string, string)
		IL_0076: stloc.s 11
		IL_0078: ldloc.s 11
		IL_007a: stloc.s 12
		IL_007c: ldloc.s 11
		IL_007e: ldc.i4.0
		IL_007f: ceq
		IL_0081: brtrue.s IL_008b

		IL_0083: nop
		IL_0084: ldarg.0
		IL_0085: ldc.i4.0
		IL_0086: stfld bool PlayerScript::hugging

		IL_008b: nop
		IL_008c: ret

		IL_008d: nop
		IL_008e: ldarg.0
		IL_008f: ldc.i4.0
		IL_0090: stfld bool PlayerScript::sweeping
		IL_0095: ret

		IL_0096: nop
		IL_0097: ldarg.0
		IL_0098: ldfld class DoorScript PlayerScript::door
		IL_009d: stloc.s 13
		IL_009f: ldloc.s 13
		IL_00a1: ldfld float32 DoorScript::lockTime
		IL_00a6: stloc.s 14
		IL_00a8: ldloc.s 13
		IL_00aa: ldfld float32 DoorScript::lockTime
		IL_00af: ldarg.0
		IL_00b0: ldfld float32 PlayerScript::guilt
		IL_00b5: clt
		IL_00b7: brtrue.s IL_008b

		IL_00b9: nop
		IL_00ba: ldarg.0
		IL_00bb: ldstr "escape"
		IL_00c0: stfld string PlayerScript::guiltType
		IL_00c5: ldarg.0
		IL_00c6: ldloc.s 14
		IL_00c8: stfld float32 PlayerScript::guilt
		IL_00cd: ret

		IL_00ce: nop
		IL_00cf: ldnull
		IL_00d0: throw
	} // end of method PlayerScript::OnTriggerExit

	.method public hidebysig 
		instance void ResetGuilt (
			string 'type',
			float32 amount
		) cil managed 
	{
		// Method begins at RVA 0x26ca
		// Header size: 1
		// Code size: 30 (0x1e)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.2
		IL_0002: ldarg.0
		IL_0003: ldfld float32 PlayerScript::guilt
		IL_0008: clt
		IL_000a: brtrue.s IL_001c

		IL_000c: nop
		IL_000d: ldarg.0
		IL_000e: ldarg.2
		IL_000f: stfld float32 PlayerScript::guilt
		IL_0014: ldarg.0
		IL_0015: ldarg.1
		IL_0016: stfld string PlayerScript::guiltType
		IL_001b: ret

		IL_001c: nop
		IL_001d: ret
	} // end of method PlayerScript::ResetGuilt

	.method private hidebysig 
		instance void GuiltCheck () cil managed 
	{
		// Method begins at RVA 0x13dcc
		// Header size: 12
		// Code size: 46 (0x2e)
		.maxstack 3
		.locals init (
			[0] float32,
			[1] float32,
			[2] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld float32 PlayerScript::guilt
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld float32 PlayerScript::guilt
		IL_000e: ldc.r4 0.0
		IL_0013: cgt
		IL_0015: ldc.i4.0
		IL_0016: ceq
		IL_0018: brtrue.s IL_002c

		IL_001a: nop
		IL_001b: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
		IL_0020: stloc.1
		IL_0021: ldarg.0
		IL_0022: ldloc.0
		IL_0023: ldloc.1
		IL_0024: sub
		IL_0025: stfld float32 PlayerScript::guilt
		IL_002a: ldloc.1
		IL_002b: stloc.2

		IL_002c: nop
		IL_002d: ret
	} // end of method PlayerScript::GuiltCheck

	.method public hidebysig 
		instance void ActivateJumpRope () cil managed 
	{
		// Method begins at RVA 0x13e08
		// Header size: 12
		// Code size: 92 (0x5c)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject PlayerScript::jumpRopeScreen
		IL_0007: ldc.i4.1
		IL_0008: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_000d: ldarg.0
		IL_000e: ldc.i4.1
		IL_000f: stfld bool PlayerScript::jumpRope
		IL_0014: ldarg.0
		IL_0015: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_001a: stloc.0
		IL_001b: ldloc.0
		IL_001c: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0021: stloc.1
		IL_0022: ldarg.0
		IL_0023: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PlayerScript::frozenPosition
		IL_0028: ldloca.s 1
		IL_002a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_002f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0034: ldarg.0
		IL_0035: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PlayerScript::frozenPosition
		IL_003a: ldloca.s 1
		IL_003c: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0041: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0046: ldarg.0
		IL_0047: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 PlayerScript::frozenPosition
		IL_004c: ldloca.s 1
		IL_004e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0053: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0058: ret

		IL_0059: nop
		IL_005a: ldnull
		IL_005b: throw
	} // end of method PlayerScript::ActivateJumpRope

	.method public hidebysig 
		instance void DeactivateJumpRope () cil managed 
	{
		// Method begins at RVA 0x26e9
		// Header size: 1
		// Code size: 24 (0x18)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject PlayerScript::jumpRopeScreen
		IL_0007: ldc.i4.0
		IL_0008: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_000d: ldarg.0
		IL_000e: ldc.i4.0
		IL_000f: stfld bool PlayerScript::jumpRope
		IL_0014: ret

		IL_0015: nop
		IL_0016: ldnull
		IL_0017: throw
	} // end of method PlayerScript::DeactivateJumpRope

	.method public hidebysig 
		instance void ActivateBoots () cil managed 
	{
		// Method begins at RVA 0x13e70
		// Header size: 12
		// Code size: 54 (0x36)
		.maxstack 2
		.locals init (
			[0] class PlayerScript/'<BootTimer>d__14',
			[1] class [UnityEngine.CoreModule]UnityEngine.Coroutine
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i4.1
		IL_0003: stfld bool PlayerScript::bootsActive
		IL_0008: ldstr "il2cpp_codegen_object_new(PlayerScript+<BootTimer>d__14)"
		IL_000d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0012: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class PlayerScript/'<BootTimer>d__14'>(object)
		IL_0017: stloc.0
		IL_0018: ldloc.0
		IL_0019: ldnull
		IL_001a: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_001f: ldloc.0
		IL_0020: ldc.i4.0
		IL_0021: stfld int32 PlayerScript/'<BootTimer>d__14'::'<>1__state'
		IL_0026: ldloc.0
		IL_0027: ldarg.0
		IL_0028: stfld class PlayerScript PlayerScript/'<BootTimer>d__14'::'<>4__this'
		IL_002d: ldarg.0
		IL_002e: ldloc.0
		IL_002f: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Coroutine [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StartCoroutine(class [mscorlib]System.Collections.IEnumerator)
		IL_0034: stloc.1
		IL_0035: ret
	} // end of method PlayerScript::ActivateBoots

	.method private hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator BootTimer () cil managed 
	{
		// Method begins at RVA 0x13eb4
		// Header size: 12
		// Code size: 40 (0x28)
		.maxstack 2
		.locals init (
			[0] class PlayerScript/'<BootTimer>d__14'
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(PlayerScript+<BootTimer>d__14)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class PlayerScript/'<BootTimer>d__14'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 PlayerScript/'<BootTimer>d__14'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class PlayerScript PlayerScript/'<BootTimer>d__14'::'<>4__this'
		IL_0026: ldloc.0
		IL_0027: ret
	} // end of method PlayerScript::BootTimer

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2702
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method PlayerScript::.ctor

} // end of class PlayerScript
