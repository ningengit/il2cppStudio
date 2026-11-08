.class public auto ansi beforefieldinit GameControllerScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Nested Types
	.class nested private auto ansi sealed beforefieldinit '<BootAnimation>d__19'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerator`1<object>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private object '<>2__current'
		.field public class GameControllerScript '<>4__this'
		.field private float32 '<time>5__2'
		.field private float32 '<height>5__3'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x2bfc
			// Header size: 1
			// Code size: 16 (0x10)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ldarg.0
			IL_0009: ldarg.1
			IL_000a: stfld int32 GameControllerScript/'<BootAnimation>d__19'::'<>1__state'
			IL_000f: ret
		} // end of method '<BootAnimation>d__19'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			// Method begins at RVA 0x2c0d
			// Header size: 1
			// Code size: 2 (0x2)
			.maxstack 8

			IL_0000: nop
			IL_0001: ret
		} // end of method '<BootAnimation>d__19'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			// Method begins at RVA 0x1a038
			// Header size: 12
			// Code size: 750 (0x2ee)
			.maxstack 4
			.locals init (
				[0] class GameControllerScript,
				[1] int32,
				[2] class [UnityEngine.CoreModule]UnityEngine.GameObject,
				[3] float32,
				[4] float32,
				[5] float32,
				[6] float32,
				[7] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[8] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[9] class [UnityEngine.CoreModule]UnityEngine.RectTransform,
				[10] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[11] float32,
				[12] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[13] class [UnityEngine.CoreModule]UnityEngine.GameObject,
				[14] float32,
				[15] float32,
				[16] class [UnityEngine.CoreModule]UnityEngine.GameObject,
				[17] float32,
				[18] float32,
				[19] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[20] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[21] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[22] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[23] class [UnityEngine.CoreModule]UnityEngine.GameObject
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld class GameControllerScript GameControllerScript/'<BootAnimation>d__19'::'<>4__this'
			IL_0007: stloc.0
			IL_0008: ldarg.0
			IL_0009: ldfld int32 GameControllerScript/'<BootAnimation>d__19'::'<>1__state'
			IL_000e: stloc.1
			IL_000f: ldarg.0
			IL_0010: ldfld int32 GameControllerScript/'<BootAnimation>d__19'::'<>1__state'
			IL_0015: ldc.i4.0
			IL_0016: ceq
			IL_0018: brtrue.s IL_0065

			IL_001a: nop
			IL_001b: ldloc.1
			IL_001c: ldc.i4.1
			IL_001d: sub
			IL_001e: stloc.1
			IL_001f: ldloc.1
			IL_0020: ldc.i4.1
			IL_0021: sub
			IL_0022: ldc.i4.0
			IL_0023: ceq
			IL_0025: brtrue.s IL_005b

			IL_0027: nop
			IL_0028: ldloc.1
			IL_0029: ldc.i4.1
			IL_002a: sub
			IL_002b: stloc.1
			IL_002c: ldloc.1
			IL_002d: ldc.i4.1
			IL_002e: sub
			IL_002f: ldc.i4.0
			IL_0030: ceq
			IL_0032: brtrue.s IL_004e

			IL_0034: nop
			IL_0035: ldloc.1
			IL_0036: ldc.i4.1
			IL_0037: ceq
			IL_0039: ldc.i4.0
			IL_003a: ceq
			IL_003c: brtrue IL_02e5

			IL_0041: nop
			IL_0042: ldarg.0
			IL_0043: ldc.i4.m1
			IL_0044: stfld int32 GameControllerScript/'<BootAnimation>d__19'::'<>1__state'
			IL_0049: br IL_01f4

			IL_004e: nop
			IL_004f: ldarg.0
			IL_0050: ldc.i4.m1
			IL_0051: stfld int32 GameControllerScript/'<BootAnimation>d__19'::'<>1__state'
			IL_0056: br IL_019d

			IL_005b: nop
			IL_005c: ldarg.0
			IL_005d: ldc.i4.m1
			IL_005e: stfld int32 GameControllerScript/'<BootAnimation>d__19'::'<>1__state'
			IL_0063: br.s IL_0096

			IL_0065: nop
			IL_0066: ldarg.0
			IL_0067: ldc.i4.m1
			IL_0068: stfld int32 GameControllerScript/'<BootAnimation>d__19'::'<>1__state'
			IL_006d: ldarg.0
			IL_006e: ldc.r4 15
			IL_0073: stfld float32 GameControllerScript/'<BootAnimation>d__19'::'<time>5__2'
			IL_0078: ldarg.0
			IL_0079: ldc.r4 375
			IL_007e: stfld float32 GameControllerScript/'<BootAnimation>d__19'::'<height>5__3'
			IL_0083: ldloc.0
			IL_0084: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform GameControllerScript::boots
			IL_0089: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
			IL_008e: stloc.2
			IL_008f: ldloc.2
			IL_0090: ldc.i4.1
			IL_0091: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)

			IL_0096: nop
			IL_0097: ldarg.0
			IL_0098: ldfld float32 GameControllerScript/'<BootAnimation>d__19'::'<height>5__3'
			IL_009d: stloc.3
			IL_009e: ldarg.0
			IL_009f: ldfld float32 GameControllerScript/'<BootAnimation>d__19'::'<height>5__3'
			IL_00a4: ldc.r4 -375
			IL_00a9: cgt
			IL_00ab: ldc.i4.0
			IL_00ac: ceq
			IL_00ae: brtrue IL_013d

			IL_00b3: nop
			IL_00b4: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
			IL_00b9: stloc.s 4
			IL_00bb: ldarg.0
			IL_00bc: ldloc.3
			IL_00bd: ldloc.s 4
			IL_00bf: ldc.r4 375
			IL_00c4: mul
			IL_00c5: sub
			IL_00c6: stfld float32 GameControllerScript/'<BootAnimation>d__19'::'<height>5__3'
			IL_00cb: ldarg.0
			IL_00cc: ldfld float32 GameControllerScript/'<BootAnimation>d__19'::'<time>5__2'
			IL_00d1: stloc.s 5
			IL_00d3: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
			IL_00d8: stloc.s 6
			IL_00da: ldarg.0
			IL_00db: ldloc.s 5
			IL_00dd: ldloc.s 6
			IL_00df: sub
			IL_00e0: stfld float32 GameControllerScript/'<BootAnimation>d__19'::'<time>5__2'
			IL_00e5: ldloc.s 6
			IL_00e7: stloc.3
			IL_00e8: ldloc.0
			IL_00e9: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform GameControllerScript::boots
			IL_00ee: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_localPosition()
			IL_00f3: stloc.s 7
			IL_00f5: ldloc.0
			IL_00f6: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform GameControllerScript::boots
			IL_00fb: ldloca.s 8
			IL_00fd: ldloca.s 7
			IL_00ff: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_0104: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_0109: ldloca.s 8
			IL_010b: ldarg.0
			IL_010c: ldfld float32 GameControllerScript/'<BootAnimation>d__19'::'<height>5__3'
			IL_0111: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_0116: ldloca.s 8
			IL_0118: ldloca.s 7
			IL_011a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_011f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_0124: ldloc.s 8
			IL_0126: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_localPosition(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
			IL_012b: ldarg.0
			IL_012c: ldnull
			IL_012d: stfld object GameControllerScript/'<BootAnimation>d__19'::'<>2__current'
			IL_0132: ldarg.0
			IL_0133: ldc.i4.1
			IL_0134: stfld int32 GameControllerScript/'<BootAnimation>d__19'::'<>1__state'

			IL_0139: nop

			IL_013a: nop
			IL_013b: ldc.i4.1
			IL_013c: ret

			IL_013d: nop
			IL_013e: ldloc.0
			IL_013f: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform GameControllerScript::boots
			IL_0144: stloc.s 9
			IL_0146: ldloc.s 9
			IL_0148: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_localPosition()
			IL_014d: stloc.s 10
			IL_014f: ldloca.s 10
			IL_0151: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_0156: stloc.s 11
			IL_0158: ldloc.0
			IL_0159: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform GameControllerScript::boots
			IL_015e: ldloca.s 12
			IL_0160: ldloc.s 11
			IL_0162: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_0167: ldloca.s 12
			IL_0169: ldc.r4 -375
			IL_016e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_0173: ldloca.s 12
			IL_0175: ldloca.s 10
			IL_0177: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_017c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_0181: ldloc.s 12
			IL_0183: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_localPosition(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
			IL_0188: ldloc.0
			IL_0189: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform GameControllerScript::boots
			IL_018e: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
			IL_0193: stloc.s 13
			IL_0195: ldloc.s 13
			IL_0197: ldc.i4.0
			IL_0198: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)

			IL_019d: nop
			IL_019e: ldarg.0
			IL_019f: ldfld float32 GameControllerScript/'<BootAnimation>d__19'::'<time>5__2'
			IL_01a4: stloc.s 14
			IL_01a6: ldarg.0
			IL_01a7: ldfld float32 GameControllerScript/'<BootAnimation>d__19'::'<time>5__2'
			IL_01ac: ldc.r4 0.0
			IL_01b1: cgt
			IL_01b3: ldc.i4.0
			IL_01b4: ceq
			IL_01b6: brtrue.s IL_01de

			IL_01b8: nop
			IL_01b9: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
			IL_01be: stloc.s 15
			IL_01c0: ldarg.0
			IL_01c1: ldnull
			IL_01c2: stfld object GameControllerScript/'<BootAnimation>d__19'::'<>2__current'
			IL_01c7: ldarg.0
			IL_01c8: ldloc.s 14
			IL_01ca: ldloc.s 15
			IL_01cc: sub
			IL_01cd: stfld float32 GameControllerScript/'<BootAnimation>d__19'::'<time>5__2'
			IL_01d2: ldarg.0
			IL_01d3: ldc.i4.2
			IL_01d4: stfld int32 GameControllerScript/'<BootAnimation>d__19'::'<>1__state'
			IL_01d9: br IL_0139

			IL_01de: nop
			IL_01df: ldloc.0
			IL_01e0: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform GameControllerScript::boots
			IL_01e5: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
			IL_01ea: stloc.s 16
			IL_01ec: ldloc.s 16
			IL_01ee: ldc.i4.1
			IL_01ef: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)

			IL_01f4: nop
			IL_01f5: ldc.r4 375
			IL_01fa: ldarg.0
			IL_01fb: ldfld float32 GameControllerScript/'<BootAnimation>d__19'::'<height>5__3'
			IL_0200: cgt
			IL_0202: ldc.i4.0
			IL_0203: ceq
			IL_0205: brtrue IL_028d

			IL_020a: nop
			IL_020b: ldarg.0
			IL_020c: ldfld float32 GameControllerScript/'<BootAnimation>d__19'::'<height>5__3'
			IL_0211: stloc.s 17
			IL_0213: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
			IL_0218: stloc.s 18
			IL_021a: ldarg.0
			IL_021b: ldloc.s 18
			IL_021d: ldc.r4 375
			IL_0222: mul
			IL_0223: ldloc.s 17
			IL_0225: add
			IL_0226: stfld float32 GameControllerScript/'<BootAnimation>d__19'::'<height>5__3'
			IL_022b: ldloc.s 18
			IL_022d: ldc.r4 375
			IL_0232: mul
			IL_0233: ldloc.s 17
			IL_0235: add
			IL_0236: stloc.3
			IL_0237: ldloc.0
			IL_0238: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform GameControllerScript::boots
			IL_023d: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_localPosition()
			IL_0242: stloc.s 19
			IL_0244: ldloc.0
			IL_0245: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform GameControllerScript::boots
			IL_024a: ldloca.s 20
			IL_024c: ldloca.s 19
			IL_024e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_0253: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_0258: ldloca.s 20
			IL_025a: ldarg.0
			IL_025b: ldfld float32 GameControllerScript/'<BootAnimation>d__19'::'<height>5__3'
			IL_0260: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_0265: ldloca.s 20
			IL_0267: ldloca.s 19
			IL_0269: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_026e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_0273: ldloc.s 20
			IL_0275: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_localPosition(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
			IL_027a: ldarg.0
			IL_027b: ldnull
			IL_027c: stfld object GameControllerScript/'<BootAnimation>d__19'::'<>2__current'
			IL_0281: ldarg.0
			IL_0282: ldc.i4.3
			IL_0283: stfld int32 GameControllerScript/'<BootAnimation>d__19'::'<>1__state'
			IL_0288: br IL_0139

			IL_028d: nop
			IL_028e: ldloc.0
			IL_028f: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform GameControllerScript::boots
			IL_0294: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_localPosition()
			IL_0299: stloc.s 21
			IL_029b: ldloc.0
			IL_029c: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform GameControllerScript::boots
			IL_02a1: ldloca.s 22
			IL_02a3: ldloca.s 21
			IL_02a5: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_02aa: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_02af: ldloca.s 22
			IL_02b1: ldc.r4 375
			IL_02b6: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_02bb: ldloca.s 22
			IL_02bd: ldloca.s 21
			IL_02bf: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_02c4: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_02c9: ldloc.s 22
			IL_02cb: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_localPosition(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
			IL_02d0: ldloc.0
			IL_02d1: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform GameControllerScript::boots
			IL_02d6: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
			IL_02db: stloc.s 23
			IL_02dd: ldloc.s 23
			IL_02df: ldc.i4.0
			IL_02e0: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)

			IL_02e5: nop
			IL_02e6: br IL_013a

			IL_02eb: nop
			IL_02ec: ldnull
			IL_02ed: throw
		} // end of method '<BootAnimation>d__19'::MoveNext

		.method private final hidebysig specialname newslot virtual 
			instance object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () cil managed 
		{
			// Method begins at RVA 0x2c10
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object GameControllerScript/'<BootAnimation>d__19'::'<>2__current'
			IL_0007: ret
		} // end of method '<BootAnimation>d__19'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'

		.method private final hidebysig newslot virtual 
			instance void System.Collections.IEnumerator.Reset () cil managed 
		{
			// Method begins at RVA 0x1a334
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
		} // end of method '<BootAnimation>d__19'::System.Collections.IEnumerator.Reset

		.method private final hidebysig specialname newslot virtual 
			instance object System.Collections.IEnumerator.get_Current () cil managed 
		{
			// Method begins at RVA 0x2c19
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object GameControllerScript/'<BootAnimation>d__19'::'<>2__current'
			IL_0007: ret
		} // end of method '<BootAnimation>d__19'::System.Collections.IEnumerator.get_Current

		// Properties
		.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current'()
		{
			.get instance object GameControllerScript/'<BootAnimation>d__19'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'()
		}
		.property instance object System.Collections.IEnumerator.Current()
		{
			.get instance object GameControllerScript/'<BootAnimation>d__19'::System.Collections.IEnumerator.get_Current()
		}

	} // end of class <BootAnimation>d__19

	.class nested private auto ansi sealed beforefieldinit '<Teleporter>d__20'
		extends [mscorlib]System.Object
		implements class [mscorlib]System.Collections.Generic.IEnumerator`1<object>,
		           [mscorlib]System.Collections.IEnumerator,
		           [mscorlib]System.IDisposable
	{
		// Fields
		.field private int32 '<>1__state'
		.field private object '<>2__current'
		.field public class GameControllerScript '<>4__this'
		.field private int32 '<teleports>5__2'
		.field private int32 '<teleportCount>5__3'
		.field private float32 '<baseTime>5__4'
		.field private float32 '<currentTime>5__5'
		.field private float32 '<increaseFactor>5__6'

		// Methods
		.method public hidebysig specialname rtspecialname 
			instance void .ctor (
				int32 '<>1__state'
			) cil managed 
		{
			// Method begins at RVA 0x2c22
			// Header size: 1
			// Code size: 16 (0x10)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldnull
			IL_0003: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
			IL_0008: ldarg.0
			IL_0009: ldarg.1
			IL_000a: stfld int32 GameControllerScript/'<Teleporter>d__20'::'<>1__state'
			IL_000f: ret
		} // end of method '<Teleporter>d__20'::.ctor

		.method private final hidebysig newslot virtual 
			instance void System.IDisposable.Dispose () cil managed 
		{
			// Method begins at RVA 0x2c33
			// Header size: 1
			// Code size: 2 (0x2)
			.maxstack 8

			IL_0000: nop
			IL_0001: ret
		} // end of method '<Teleporter>d__20'::System.IDisposable.Dispose

		.method private final hidebysig newslot virtual 
			instance bool MoveNext () cil managed 
		{
			// Method begins at RVA 0x1a384
			// Header size: 12
			// Code size: 537 (0x219)
			.maxstack 4
			.locals init (
				[0] int32,
				[1] class GameControllerScript,
				[2] int32,
				[3] float32,
				[4] float32,
				[5] float32,
				[6] class [UnityEngine.CoreModule]UnityEngine.Transform,
				[7] class [UnityEngine.CoreModule]UnityEngine.Transform,
				[8] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[9] float32,
				[10] float32,
				[11] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
				[12] float32,
				[13] class PlayerScript
			)

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld int32 GameControllerScript/'<Teleporter>d__20'::'<>1__state'
			IL_0007: stloc.0
			IL_0008: ldarg.0
			IL_0009: ldfld class GameControllerScript GameControllerScript/'<Teleporter>d__20'::'<>4__this'
			IL_000e: stloc.1
			IL_000f: ldarg.0
			IL_0010: ldfld int32 GameControllerScript/'<Teleporter>d__20'::'<>1__state'
			IL_0015: ldc.i4.0
			IL_0016: ceq
			IL_0018: ldc.i4.0
			IL_0019: ceq
			IL_001b: brtrue.s IL_0078

			IL_001d: nop
			IL_001e: ldarg.0
			IL_001f: ldc.i4.m1
			IL_0020: stfld int32 GameControllerScript/'<Teleporter>d__20'::'<>1__state'
			IL_0025: ldloc.1
			IL_0026: ldfld class [UnityEngine.PhysicsModule]UnityEngine.CharacterController GameControllerScript::playerCharacter
			IL_002b: ldc.i4.0
			IL_002c: callvirt instance void [UnityEngine.PhysicsModule]UnityEngine.Collider::set_enabled(bool)
			IL_0031: ldloc.1
			IL_0032: ldfld class [UnityEngine.PhysicsModule]UnityEngine.Collider GameControllerScript::playerCollider
			IL_0037: ldc.i4.0
			IL_0038: callvirt instance void [UnityEngine.PhysicsModule]UnityEngine.Collider::set_enabled(bool)
			IL_003d: ldc.i4.s 12
			IL_003f: ldc.i4.s 16
			IL_0041: call int32 [UnityEngine.CoreModule]UnityEngine.Random::Range(int32, int32)
			IL_0046: stloc.2
			IL_0047: ldarg.0
			IL_0048: ldloc.2
			IL_0049: stfld int32 GameControllerScript/'<Teleporter>d__20'::'<teleports>5__2'
			IL_004e: ldarg.0
			IL_004f: ldc.i4.0
			IL_0050: stfld int32 GameControllerScript/'<Teleporter>d__20'::'<teleportCount>5__3'
			IL_0055: ldarg.0
			IL_0056: ldc.r4 0.2
			IL_005b: stfld float32 GameControllerScript/'<Teleporter>d__20'::'<baseTime>5__4'
			IL_0060: ldarg.0
			IL_0061: ldc.r4 0.2
			IL_0066: stfld float32 GameControllerScript/'<Teleporter>d__20'::'<currentTime>5__5'
			IL_006b: ldarg.0
			IL_006c: ldc.r4 1.1
			IL_0071: stfld float32 GameControllerScript/'<Teleporter>d__20'::'<increaseFactor>5__6'
			IL_0076: br.s IL_008d

			IL_0078: nop
			IL_0079: ldloc.0
			IL_007a: ldc.i4.1
			IL_007b: ceq
			IL_007d: ldc.i4.0
			IL_007e: ceq
			IL_0080: brtrue IL_0212

			IL_0085: nop
			IL_0086: ldarg.0
			IL_0087: ldc.i4.m1
			IL_0088: stfld int32 GameControllerScript/'<Teleporter>d__20'::'<>1__state'

			IL_008d: nop
			IL_008e: ldarg.0
			IL_008f: ldfld int32 GameControllerScript/'<Teleporter>d__20'::'<teleportCount>5__3'
			IL_0094: ldarg.0
			IL_0095: ldfld int32 GameControllerScript/'<Teleporter>d__20'::'<teleports>5__2'
			IL_009a: clt
			IL_009c: ldc.i4.0
			IL_009d: ceq
			IL_009f: brtrue IL_01f9

			IL_00a4: nop
			IL_00a5: ldarg.0
			IL_00a6: ldfld float32 GameControllerScript/'<Teleporter>d__20'::'<currentTime>5__5'
			IL_00ab: stloc.3
			IL_00ac: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_deltaTime()
			IL_00b1: stloc.s 4
			IL_00b3: ldarg.0
			IL_00b4: ldloc.3
			IL_00b5: ldloc.s 4
			IL_00b7: sub
			IL_00b8: stfld float32 GameControllerScript/'<Teleporter>d__20'::'<currentTime>5__5'
			IL_00bd: ldloc.s 4
			IL_00bf: stloc.s 5
			IL_00c1: ldc.r4 0.0
			IL_00c6: ldloc.3
			IL_00c7: ldloc.s 4
			IL_00c9: sub
			IL_00ca: cgt
			IL_00cc: ldc.i4.0
			IL_00cd: ceq
			IL_00cf: brtrue IL_01c1

			IL_00d4: nop
			IL_00d5: ldloc.1
			IL_00d6: ldfld class AILocationSelectorScript GameControllerScript::AILocationSelector
			IL_00db: callvirt instance void AILocationSelectorScript::GetNewTarget()
			IL_00e0: ldloc.1
			IL_00e1: ldfld class PlayerScript GameControllerScript::player
			IL_00e6: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
			IL_00eb: stloc.s 6
			IL_00ed: ldloc.1
			IL_00ee: ldfld class AILocationSelectorScript GameControllerScript::AILocationSelector
			IL_00f3: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
			IL_00f8: stloc.s 7
			IL_00fa: ldloc.s 7
			IL_00fc: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
			IL_0101: stloc.s 8
			IL_0103: ldloc.1
			IL_0104: ldfld class PlayerScript GameControllerScript::player
			IL_0109: ldfld float32 PlayerScript::height
			IL_010e: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::upVector
			IL_0113: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_0118: mul
			IL_0119: ldloca.s 8
			IL_011b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_0120: add
			IL_0121: stloc.s 9
			IL_0123: ldloc.1
			IL_0124: ldfld class PlayerScript GameControllerScript::player
			IL_0129: ldfld float32 PlayerScript::height
			IL_012e: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::upVector
			IL_0133: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_0138: mul
			IL_0139: ldloca.s 8
			IL_013b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_0140: add
			IL_0141: stloc.s 10
			IL_0143: ldloc.s 6
			IL_0145: ldloca.s 11
			IL_0147: ldloc.s 9
			IL_0149: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
			IL_014e: ldloca.s 11
			IL_0150: ldloc.s 10
			IL_0152: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
			IL_0157: ldloca.s 11
			IL_0159: ldloc.1
			IL_015a: ldfld class PlayerScript GameControllerScript::player
			IL_015f: ldfld float32 PlayerScript::height
			IL_0164: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::upVector
			IL_0169: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_016e: mul
			IL_016f: ldloca.s 8
			IL_0171: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_0176: add
			IL_0177: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
			IL_017c: ldloc.s 11
			IL_017e: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
			IL_0183: ldloc.1
			IL_0184: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
			IL_0189: ldloc.1
			IL_018a: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip GameControllerScript::aud_Teleport
			IL_018f: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
			IL_0194: ldarg.0
			IL_0195: ldfld float32 GameControllerScript/'<Teleporter>d__20'::'<increaseFactor>5__6'
			IL_019a: ldarg.0
			IL_019b: ldfld float32 GameControllerScript/'<Teleporter>d__20'::'<baseTime>5__4'
			IL_01a0: mul
			IL_01a1: stloc.s 12
			IL_01a3: ldarg.0
			IL_01a4: ldarg.0
			IL_01a5: ldfld int32 GameControllerScript/'<Teleporter>d__20'::'<teleportCount>5__3'
			IL_01aa: ldc.i4.1
			IL_01ab: add
			IL_01ac: stfld int32 GameControllerScript/'<Teleporter>d__20'::'<teleportCount>5__3'
			IL_01b1: ldarg.0
			IL_01b2: ldloc.s 12
			IL_01b4: stfld float32 GameControllerScript/'<Teleporter>d__20'::'<baseTime>5__4'
			IL_01b9: ldarg.0
			IL_01ba: ldloc.s 12
			IL_01bc: stfld float32 GameControllerScript/'<Teleporter>d__20'::'<currentTime>5__5'

			IL_01c1: nop
			IL_01c2: ldloc.1
			IL_01c3: ldfld class PlayerScript GameControllerScript::player
			IL_01c8: stloc.s 13
			IL_01ca: ldloc.1
			IL_01cb: ldfld bool GameControllerScript::flipped
			IL_01d0: ldc.i4.0
			IL_01d1: ceq
			IL_01d3: ldc.i4.0
			IL_01d4: ceq
			IL_01d6: brtrue.s IL_01db

			IL_01d8: nop
			IL_01d9: br.s IL_01dc

			IL_01db: nop

			IL_01dc: nop
			IL_01dd: ldloc.s 13
			IL_01df: ldc.r4 6
			IL_01e4: stfld float32 PlayerScript::height
			IL_01e9: ldarg.0
			IL_01ea: ldnull
			IL_01eb: stfld object GameControllerScript/'<Teleporter>d__20'::'<>2__current'
			IL_01f0: ldarg.0
			IL_01f1: ldc.i4.1
			IL_01f2: stfld int32 GameControllerScript/'<Teleporter>d__20'::'<>1__state'
			IL_01f7: br.s IL_0213

			IL_01f9: nop
			IL_01fa: ldloc.1
			IL_01fb: ldfld class [UnityEngine.PhysicsModule]UnityEngine.CharacterController GameControllerScript::playerCharacter
			IL_0200: ldc.i4.1
			IL_0201: callvirt instance void [UnityEngine.PhysicsModule]UnityEngine.Collider::set_enabled(bool)
			IL_0206: ldloc.1
			IL_0207: ldfld class [UnityEngine.PhysicsModule]UnityEngine.Collider GameControllerScript::playerCollider
			IL_020c: ldc.i4.1
			IL_020d: callvirt instance void [UnityEngine.PhysicsModule]UnityEngine.Collider::set_enabled(bool)

			IL_0212: nop

			IL_0213: nop
			IL_0214: ldc.i4.0
			IL_0215: ret

			IL_0216: nop
			IL_0217: ldnull
			IL_0218: throw
		} // end of method '<Teleporter>d__20'::MoveNext

		.method private final hidebysig specialname newslot virtual 
			instance object 'System.Collections.Generic.IEnumerator<System.Object>.get_Current' () cil managed 
		{
			// Method begins at RVA 0x2c36
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object GameControllerScript/'<Teleporter>d__20'::'<>2__current'
			IL_0007: ret
		} // end of method '<Teleporter>d__20'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'

		.method private final hidebysig newslot virtual 
			instance void System.Collections.IEnumerator.Reset () cil managed 
		{
			// Method begins at RVA 0x1a5ac
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
		} // end of method '<Teleporter>d__20'::System.Collections.IEnumerator.Reset

		.method private final hidebysig specialname newslot virtual 
			instance object System.Collections.IEnumerator.get_Current () cil managed 
		{
			// Method begins at RVA 0x2c3f
			// Header size: 1
			// Code size: 8 (0x8)
			.maxstack 8

			IL_0000: nop
			IL_0001: ldarg.0
			IL_0002: ldfld object GameControllerScript/'<Teleporter>d__20'::'<>2__current'
			IL_0007: ret
		} // end of method '<Teleporter>d__20'::System.Collections.IEnumerator.get_Current

		// Properties
		.property instance object 'System.Collections.Generic.IEnumerator<System.Object>.Current'()
		{
			.get instance object GameControllerScript/'<Teleporter>d__20'::'System.Collections.Generic.IEnumerator<System.Object>.get_Current'()
		}
		.property instance object System.Collections.IEnumerator.Current()
		{
			.get instance object GameControllerScript/'<Teleporter>d__20'::System.Collections.IEnumerator.get_Current()
		}

	} // end of class <Teleporter>d__20


	// Fields
	.field public class CursorControllerScript cursorController
	.field public class PlayerScript player
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform playerTransform
	.field public class [UnityEngine.PhysicsModule]UnityEngine.CharacterController playerCharacter
	.field public class [UnityEngine.PhysicsModule]UnityEngine.Collider playerCollider
	.field public class AILocationSelectorScript AILocationSelector
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform cameraTransform
	.field public class [UnityEngine.CoreModule]UnityEngine.Camera playerCamera
	.field private int32 cullingMask
	.field public class EntranceScript entrance_0
	.field public class EntranceScript entrance_1
	.field public class EntranceScript entrance_2
	.field public class EntranceScript entrance_3
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject baldiTutor
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject baldi
	.field public class BaldiScript baldiScrpt
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_Prize
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_PrizeMobile
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_AllNotebooks
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_Teleport
	.field private bool flipped
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject principal
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject crafters
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject playtime
	.field public class PlaytimeScript playtimeScript
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject gottaSweep
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject bully
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject firstPrize
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject TestEnemy
	.field public class FirstPrizeScript firstPrizeScript
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject quarter
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioSource tutorBaldi
	.field public class [UnityEngine.CoreModule]UnityEngine.RectTransform boots
	.field public string mode
	.field public int32 notebooks
	.field public int32 failedNotebooks
	.field public bool spoopMode
	.field public bool finaleMode
	.field public bool debugMode
	.field public bool mouseLocked
	.field public int32 exitsReached
	.field public int32 itemSelected
	.field public int32[] item
	.field public class [UnityEngine.UI]UnityEngine.UI.RawImage[] itemSlot
	.field private string[] itemNames
	.field public class [Unity.TextMeshPro]TMPro.TMP_Text itemText
	.field public class [UnityEngine.CoreModule]UnityEngine.Texture[] itemTextures
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject bsodaSpray
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject alarmClock
	.field public class [Unity.TextMeshPro]TMPro.TMP_Text notebookCount
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject pauseMenu
	.field public string ExitGameScene
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject highScoreText
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject warning
	.field public class [UnityEngine.CoreModule]UnityEngine.GameObject reticle
	.field public class [UnityEngine.CoreModule]UnityEngine.RectTransform itemSelect
	.field public int32[] itemSelectOffset
	.field public int32 itemSelectPosition
	.field private bool gamePaused
	.field private bool learningActive
	.field private float32 gameOverDelay
	.field public string GameOverScene
	.field private class [UnityEngine.AudioModule]UnityEngine.AudioSource audioDevice
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_Soda
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_Spray
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_buzz
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_Hang
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_MachineQuiet
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_MachineStart
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_MachineRev
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_MachineLoop
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioClip aud_Switch
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioSource schoolMusic
	.field public class [UnityEngine.AudioModule]UnityEngine.AudioSource learnMusic

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0x38fc
		// Header size: 12
		// Code size: 189 (0xbd)
		.maxstack 2
		.locals init (
			[0] int32,
			[1] class [UnityEngine.AudioModule]UnityEngine.AudioSource,
			[2] string,
			[3] bool,
			[4] class [UnityEngine.CoreModule]UnityEngine.GameObject
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.Camera GameControllerScript::playerCamera
		IL_0007: callvirt instance int32 [UnityEngine.CoreModule]UnityEngine.Camera::get_cullingMask()
		IL_000c: stloc.0
		IL_000d: ldarg.0
		IL_000e: ldloc.0
		IL_000f: stfld int32 GameControllerScript::cullingMask
		IL_0014: ldarg.0
		IL_0015: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.AudioModule]UnityEngine.AudioSource>()
		IL_001a: stloc.1
		IL_001b: ldarg.0
		IL_001c: ldloc.1
		IL_001d: stfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_0022: ldstr "CurrentMode"
		IL_0027: call string [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::GetString(string)
		IL_002c: stloc.2
		IL_002d: ldarg.0
		IL_002e: ldloc.2
		IL_002f: stfld string GameControllerScript::mode
		IL_0034: ldarg.0
		IL_0035: ldfld string GameControllerScript::mode
		IL_003a: ldstr "endless"
		IL_003f: call bool [mscorlib]System.String::Equals(string, string)
		IL_0044: stloc.3
		IL_0045: ldloc.3
		IL_0046: ldc.i4.0
		IL_0047: ceq
		IL_0049: brtrue.s IL_0058

		IL_004b: nop
		IL_004c: ldarg.0
		IL_004d: ldfld class BaldiScript GameControllerScript::baldiScrpt
		IL_0052: ldc.i4.1
		IL_0053: stfld bool BaldiScript::endless

		IL_0058: nop
		IL_0059: ldarg.0
		IL_005a: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::schoolMusic
		IL_005f: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Play()
		IL_0064: ldarg.0
		IL_0065: ldfld bool GameControllerScript::learningActive
		IL_006a: ldc.i4.0
		IL_006b: ceq
		IL_006d: ldc.i4.0
		IL_006e: ceq
		IL_0070: brtrue.s IL_00a0

		IL_0072: nop
		IL_0073: ldc.i4.1
		IL_0074: box [mscorlib]System.Int32
		IL_0079: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode>(object)
		IL_007e: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_lockState(valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode)
		IL_0083: ldc.i4.0
		IL_0084: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_visible(bool)
		IL_0089: ldarg.0
		IL_008a: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::reticle
		IL_008f: stloc.s 4
		IL_0091: ldarg.0
		IL_0092: ldc.i4.1
		IL_0093: stfld bool GameControllerScript::mouseLocked
		IL_0098: ldloc.s 4
		IL_009a: ldc.i4.1
		IL_009b: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)

		IL_00a0: nop
		IL_00a1: ldarg.0
		IL_00a2: callvirt instance void GameControllerScript::UpdateNotebookCount()
		IL_00a7: ldarg.0
		IL_00a8: ldc.i4.0
		IL_00a9: stfld int32 GameControllerScript::itemSelected
		IL_00ae: ldarg.0
		IL_00af: ldc.r4 0.5
		IL_00b4: stfld float32 GameControllerScript::gameOverDelay
		IL_00b9: ret

		IL_00ba: nop
		IL_00bb: ldnull
		IL_00bc: throw
	} // end of method GameControllerScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x39c8
		// Header size: 12
		// Code size: 1546 (0x60a)
		.maxstack 4
		.locals init (
			[0] float32,
			[1] float32,
			[2] class InputManager,
			[3] class InputManager,
			[4] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[5] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[6] bool,
			[7] bool,
			[8] string,
			[9] int32,
			[10] float32,
			[11] int32,
			[12] class InputManager,
			[13] float32,
			[14] float32,
			[15] float32,
			[16] float32,
			[17] float32,
			[18] int32,
			[19] int32[],
			[20] float32,
			[21] string[],
			[22] int64,
			[23] string,
			[24] string,
			[25] float32,
			[26] class InputManager,
			[27] class InputManager,
			[28] class InputManager,
			[29] float32,
			[30] bool,
			[31] int32,
			[32] float32,
			[33] bool,
			[34] int32,
			[35] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[36] class PlayerScript,
			[37] bool,
			[38] int32,
			[39] int32,
			[40] bool,
			[41] float32,
			[42] float32,
			[43] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[44] bool,
			[45] int32,
			[46] int32,
			[47] string,
			[48] bool,
			[49] bool
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld bool GameControllerScript::learningActive
		IL_0007: ldc.i4.0
		IL_0008: ceq
		IL_000a: brtrue.s IL_0033

		IL_000c: nop
		IL_000d: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_timeScale()
		IL_0012: stloc.0
		IL_0013: ldloc.0
		IL_0014: stloc.1
		IL_0015: nop
		IL_0016: ldloc.0
		IL_0017: ldc.r4 0.0
		IL_001c: ceq
		IL_001e: brtrue IL_03a3

		IL_0023: nop
		IL_0024: ldc.r4 0.0
		IL_0029: call void [UnityEngine.CoreModule]UnityEngine.Time::set_timeScale(float32)
		IL_002e: br IL_03a3

		IL_0033: nop
		IL_0034: call !0 class Singleton`1<class InputManager>::get_Instance()
		IL_0039: stloc.2
		IL_003a: ldloc.2
		IL_003b: stloc.3
		IL_003c: ldloc.3
		IL_003d: ldfld bool[] InputManager::currentKeyStates
		IL_0042: ldc.i4.s 12
		IL_0044: ldelem [mscorlib]System.Boolean
		IL_0049: ldc.i4.0
		IL_004a: ceq
		IL_004c: brtrue IL_00f1

		IL_0051: nop
		IL_0052: ldloc.3
		IL_0053: ldfld bool[] InputManager::previousKeyStates
		IL_0058: ldc.i4.s 12
		IL_005a: ldelem [mscorlib]System.Boolean
		IL_005f: ldc.i4.0
		IL_0060: ceq
		IL_0062: ldc.i4.0
		IL_0063: ceq
		IL_0065: brtrue IL_00f1

		IL_006a: nop
		IL_006b: ldarg.0
		IL_006c: ldfld class PlayerScript GameControllerScript::player
		IL_0071: ldfld bool PlayerScript::gameOver
		IL_0076: ldc.i4.0
		IL_0077: ceq
		IL_0079: ldc.i4.0
		IL_007a: ceq
		IL_007c: brtrue.s IL_00f1

		IL_007e: nop
		IL_007f: ldarg.0
		IL_0080: ldfld bool GameControllerScript::gamePaused
		IL_0085: ldc.i4.0
		IL_0086: ceq
		IL_0088: brtrue.s IL_0093

		IL_008a: nop
		IL_008b: ldarg.0
		IL_008c: callvirt instance void GameControllerScript::UnpauseGame()
		IL_0091: br.s IL_00f1

		IL_0093: nop
		IL_0094: ldarg.0
		IL_0095: ldfld bool GameControllerScript::learningActive
		IL_009a: ldc.i4.0
		IL_009b: ceq
		IL_009d: ldc.i4.0
		IL_009e: ceq
		IL_00a0: brtrue.s IL_00f1

		IL_00a2: nop
		IL_00a3: ldc.i4.0
		IL_00a4: box [mscorlib]System.Int32
		IL_00a9: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode>(object)
		IL_00ae: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_lockState(valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode)
		IL_00b3: ldc.i4.1
		IL_00b4: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_visible(bool)
		IL_00b9: ldarg.0
		IL_00ba: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::reticle
		IL_00bf: stloc.s 4
		IL_00c1: ldarg.0
		IL_00c2: ldc.i4.0
		IL_00c3: stfld bool GameControllerScript::mouseLocked
		IL_00c8: ldloc.s 4
		IL_00ca: ldc.i4.0
		IL_00cb: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_00d0: ldc.r4 0.0
		IL_00d5: call void [UnityEngine.CoreModule]UnityEngine.Time::set_timeScale(float32)
		IL_00da: ldarg.0
		IL_00db: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::pauseMenu
		IL_00e0: stloc.s 5
		IL_00e2: ldarg.0
		IL_00e3: ldc.i4.1
		IL_00e4: stfld bool GameControllerScript::gamePaused
		IL_00e9: ldloc.s 5
		IL_00eb: ldc.i4.1
		IL_00ec: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)

		IL_00f1: nop
		IL_00f2: ldc.i4.s 121
		IL_00f4: box [mscorlib]System.Int32
		IL_00f9: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
		IL_00fe: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetKeyDownInt(valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode)
		IL_0103: stloc.s 6
		IL_0105: ldarg.0
		IL_0106: ldfld bool GameControllerScript::gamePaused
		IL_010b: ldloc.s 6
		IL_010d: and
		IL_010e: ldc.i4.0
		IL_010f: ceq
		IL_0111: ldc.i4.0
		IL_0112: ceq
		IL_0114: brtrue.s IL_0141

		IL_0116: nop
		IL_0117: ldc.i4.s 110
		IL_0119: box [mscorlib]System.Int32
		IL_011e: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
		IL_0123: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetKeyDownInt(valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode)
		IL_0128: stloc.s 7
		IL_012a: ldarg.0
		IL_012b: ldfld bool GameControllerScript::gamePaused
		IL_0130: ldloc.s 7
		IL_0132: and
		IL_0133: ldc.i4.0
		IL_0134: ceq
		IL_0136: brtrue.s IL_015f

		IL_0138: nop
		IL_0139: ldarg.0
		IL_013a: callvirt instance void GameControllerScript::UnpauseGame()
		IL_013f: br.s IL_015f

		IL_0141: nop
		IL_0142: ldc.r4 1
		IL_0147: call void [UnityEngine.CoreModule]UnityEngine.Time::set_timeScale(float32)
		IL_014c: ldarg.0
		IL_014d: ldfld string GameControllerScript::ExitGameScene
		IL_0152: stloc.s 8
		IL_0154: br.s IL_0157

		IL_0156: nop

		IL_0157: nop
		IL_0158: ldloc.s 8
		IL_015a: call void [UnityEngine.CoreModule]UnityEngine.SceneManagement.SceneManager::LoadScene(string)

		IL_015f: nop
		IL_0160: ldarg.0
		IL_0161: ldfld bool GameControllerScript::gamePaused
		IL_0166: stloc.s 9
		IL_0168: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_timeScale()
		IL_016d: stloc.s 10
		IL_016f: ldloc.s 9
		IL_0171: stloc.s 11
		IL_0173: ldloc.s 10
		IL_0175: stloc.1
		IL_0176: nop
		IL_0177: ldloc.s 10
		IL_0179: ldc.r4 1
		IL_017e: ceq
		IL_0180: brtrue.s IL_0198

		IL_0182: nop
		IL_0183: ldloc.s 11
		IL_0185: ldc.i4.0
		IL_0186: ceq
		IL_0188: ldc.i4.0
		IL_0189: ceq
		IL_018b: brtrue.s IL_0198

		IL_018d: nop
		IL_018e: ldc.r4 1
		IL_0193: call void [UnityEngine.CoreModule]UnityEngine.Time::set_timeScale(float32)

		IL_0198: nop
		IL_0199: call !0 class Singleton`1<class InputManager>::get_Instance()
		IL_019e: stloc.s 12
		IL_01a0: ldloc.s 12
		IL_01a2: stloc.3
		IL_01a3: ldloc.3
		IL_01a4: ldfld bool[] InputManager::currentKeyStates
		IL_01a9: ldc.i4.5
		IL_01aa: ldelem [mscorlib]System.Boolean
		IL_01af: ldc.i4.0
		IL_01b0: ceq
		IL_01b2: brtrue.s IL_01e7

		IL_01b4: nop
		IL_01b5: ldloc.3
		IL_01b6: ldfld bool[] InputManager::previousKeyStates
		IL_01bb: ldc.i4.5
		IL_01bc: ldelem [mscorlib]System.Boolean
		IL_01c1: ldc.i4.0
		IL_01c2: ceq
		IL_01c4: ldc.i4.0
		IL_01c5: ceq
		IL_01c7: brtrue.s IL_01e7

		IL_01c9: nop
		IL_01ca: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_timeScale()
		IL_01cf: stloc.s 13
		IL_01d1: ldloc.s 13
		IL_01d3: stloc.1
		IL_01d4: nop
		IL_01d5: ldloc.s 13
		IL_01d7: ldc.r4 0.0
		IL_01dc: ceq
		IL_01de: brtrue.s IL_01e7

		IL_01e0: nop
		IL_01e1: ldarg.0
		IL_01e2: callvirt instance void GameControllerScript::UseItem()

		IL_01e7: nop
		IL_01e8: ldstr "Mouse ScrollWheel"
		IL_01ed: call float32 [UnityEngine.InputLegacyModule]UnityEngine.Input::GetAxis(string)
		IL_01f2: stloc.s 14
		IL_01f4: ldloc.s 14
		IL_01f6: stloc.1
		IL_01f7: ldloc.s 14
		IL_01f9: ldc.r4 0.0
		IL_01fe: cgt
		IL_0200: ldc.i4.0
		IL_0201: ceq
		IL_0203: brtrue.s IL_0245

		IL_0205: nop
		IL_0206: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_timeScale()
		IL_020b: stloc.s 15
		IL_020d: ldloc.s 15
		IL_020f: stloc.1
		IL_0210: nop
		IL_0211: ldloc.s 15
		IL_0213: ldc.r4 0.0
		IL_0218: ceq
		IL_021a: brtrue.s IL_0245

		IL_021c: nop
		IL_021d: ldarg.0
		IL_021e: ldarg.0
		IL_021f: ldfld int32 GameControllerScript::itemSelected
		IL_0224: ldc.i4.1
		IL_0225: sub
		IL_0226: stfld int32 GameControllerScript::itemSelected
		IL_022b: ldarg.0
		IL_022c: ldfld int32 GameControllerScript::itemSelected
		IL_0231: ldc.i4.1
		IL_0232: sub
		IL_0233: ldc.i4.0
		IL_0234: clt
		IL_0236: ldc.i4.0
		IL_0237: ceq
		IL_0239: brtrue.s IL_02a5

		IL_023b: nop
		IL_023c: ldarg.0
		IL_023d: ldc.i4.2
		IL_023e: stfld int32 GameControllerScript::itemSelected
		IL_0243: br.s IL_02a5

		IL_0245: nop
		IL_0246: ldstr "Mouse ScrollWheel"
		IL_024b: call float32 [UnityEngine.InputLegacyModule]UnityEngine.Input::GetAxis(string)
		IL_0250: stloc.s 16
		IL_0252: ldloc.s 16
		IL_0254: stloc.1
		IL_0255: ldc.r4 0.0
		IL_025a: ldloc.s 16
		IL_025c: cgt
		IL_025e: ldc.i4.0
		IL_025f: ceq
		IL_0261: brtrue IL_0315

		IL_0266: nop
		IL_0267: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_timeScale()
		IL_026c: stloc.s 17
		IL_026e: ldloc.s 17
		IL_0270: stloc.1
		IL_0271: nop
		IL_0272: ldloc.s 17
		IL_0274: ldc.r4 0.0
		IL_0279: ceq
		IL_027b: brtrue IL_0315

		IL_0280: nop
		IL_0281: ldarg.0
		IL_0282: ldfld int32 GameControllerScript::itemSelected
		IL_0287: ldc.i4.1
		IL_0288: add
		IL_0289: stloc.s 18
		IL_028b: ldarg.0
		IL_028c: ldloc.s 18
		IL_028e: stfld int32 GameControllerScript::itemSelected
		IL_0293: ldloc.s 18
		IL_0295: ldc.i4.2
		IL_0296: cgt
		IL_0298: ldc.i4.0
		IL_0299: ceq
		IL_029b: brtrue.s IL_02a5

		IL_029d: nop
		IL_029e: ldarg.0
		IL_029f: ldc.i4.0
		IL_02a0: stfld int32 GameControllerScript::itemSelected

		IL_02a5: nop
		IL_02a6: ldarg.0
		IL_02a7: ldfld int32[] GameControllerScript::itemSelectOffset
		IL_02ac: stloc.s 19
		IL_02ae: ldloc.s 19
		IL_02b0: ldarg.0
		IL_02b1: ldfld int32 GameControllerScript::itemSelected
		IL_02b6: conv.i8
		IL_02b7: conv.i4
		IL_02b8: ldelem [mscorlib]System.Int32
		IL_02bd: conv.r4
		IL_02be: stloc.1
		IL_02bf: ldarg.0
		IL_02c0: ldfld int32 GameControllerScript::itemSelectPosition
		IL_02c5: conv.r4
		IL_02c6: stloc.s 20
		IL_02c8: ldarg.0
		IL_02c9: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform GameControllerScript::itemSelect
		IL_02ce: ldstr "unpcklps xmm0,xmm1"
		IL_02d3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_02d8: box [mscorlib]System.Int64
		IL_02dd: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector2>(object)
		IL_02e2: callvirt instance void [UnityEngine.CoreModule]UnityEngine.RectTransform::set_anchoredPosition(valuetype [UnityEngine.CoreModule]UnityEngine.Vector2)
		IL_02e7: ldarg.0
		IL_02e8: ldfld string[] GameControllerScript::itemNames
		IL_02ed: stloc.s 21
		IL_02ef: ldarg.0
		IL_02f0: ldfld int32[] GameControllerScript::item
		IL_02f5: ldarg.0
		IL_02f6: ldfld int32 GameControllerScript::itemSelected
		IL_02fb: conv.i8
		IL_02fc: conv.i4
		IL_02fd: ldelem [mscorlib]System.Int32
		IL_0302: conv.i8
		IL_0303: stloc.s 22
		IL_0305: ldloc.s 21
		IL_0307: ldloc.s 22
		IL_0309: conv.i4
		IL_030a: ldelem [mscorlib]System.String
		IL_030f: stloc.s 23
		IL_0311: ldloc.s 23
		IL_0313: stloc.s 24

		IL_0315: nop
		IL_0316: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_timeScale()
		IL_031b: stloc.s 25
		IL_031d: ldloc.s 25
		IL_031f: stloc.1
		IL_0320: nop
		IL_0321: ldloc.s 25
		IL_0323: ldc.r4 0.0
		IL_0328: ceq
		IL_032a: brtrue.s IL_03a3

		IL_032c: nop
		IL_032d: call !0 class Singleton`1<class InputManager>::get_Instance()
		IL_0332: stloc.s 26
		IL_0334: ldloc.s 26
		IL_0336: ldfld bool[] InputManager::currentKeyStates
		IL_033b: ldc.i4.6
		IL_033c: ldelem [mscorlib]System.Boolean
		IL_0341: ldc.i4.0
		IL_0342: ceq
		IL_0344: ldc.i4.0
		IL_0345: ceq
		IL_0347: brtrue.s IL_0394

		IL_0349: nop
		IL_034a: call !0 class Singleton`1<class InputManager>::get_Instance()
		IL_034f: stloc.s 27
		IL_0351: ldloc.s 27
		IL_0353: ldfld bool[] InputManager::currentKeyStates
		IL_0358: ldc.i4.7
		IL_0359: ldelem [mscorlib]System.Boolean
		IL_035e: ldc.i4.0
		IL_035f: ceq
		IL_0361: ldc.i4.0
		IL_0362: ceq
		IL_0364: brtrue.s IL_038a

		IL_0366: nop
		IL_0367: call !0 class Singleton`1<class InputManager>::get_Instance()
		IL_036c: stloc.s 28
		IL_036e: ldloc.s 28
		IL_0370: ldfld bool[] InputManager::currentKeyStates
		IL_0375: ldc.i4.8
		IL_0376: ldelem [mscorlib]System.Boolean
		IL_037b: ldc.i4.0
		IL_037c: ceq
		IL_037e: brtrue.s IL_03a3

		IL_0380: nop
		IL_0381: ldarg.0
		IL_0382: ldc.i4.2
		IL_0383: stfld int32 GameControllerScript::itemSelected
		IL_0388: br.s IL_039c

		IL_038a: nop
		IL_038b: ldarg.0
		IL_038c: ldc.i4.1
		IL_038d: stfld int32 GameControllerScript::itemSelected
		IL_0392: br.s IL_039c

		IL_0394: nop
		IL_0395: ldarg.0
		IL_0396: ldc.i4.0
		IL_0397: stfld int32 GameControllerScript::itemSelected

		IL_039c: nop
		IL_039d: ldarg.0
		IL_039e: callvirt instance void GameControllerScript::UpdateItemSelection()

		IL_03a3: nop
		IL_03a4: ldarg.0
		IL_03a5: ldfld class PlayerScript GameControllerScript::player
		IL_03aa: ldfld float32 PlayerScript::stamina
		IL_03af: stloc.s 29
		IL_03b1: ldarg.0
		IL_03b2: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::warning
		IL_03b7: callvirt instance bool [UnityEngine.CoreModule]UnityEngine.GameObject::get_activeSelf()
		IL_03bc: stloc.s 30
		IL_03be: ldc.r4 0.0
		IL_03c3: ldloc.s 29
		IL_03c5: cgt
		IL_03c7: brfalse.s IL_03d1

		IL_03c9: ldloc.s 30
		IL_03cb: ldc.i4.1
		IL_03cc: xor
		IL_03cd: stloc.s 31
		IL_03cf: br.s IL_03d5

		IL_03d1: nop
		IL_03d2: ldc.i4.0
		IL_03d3: stloc.s 31

		IL_03d5: nop
		IL_03d6: ldloc.s 31
		IL_03d8: ldc.i4.0
		IL_03d9: ceq
		IL_03db: ldc.i4.0
		IL_03dc: ceq
		IL_03de: brtrue.s IL_0423

		IL_03e0: nop
		IL_03e1: ldarg.0
		IL_03e2: ldfld class PlayerScript GameControllerScript::player
		IL_03e7: ldfld float32 PlayerScript::stamina
		IL_03ec: stloc.s 32
		IL_03ee: ldarg.0
		IL_03ef: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::warning
		IL_03f4: callvirt instance bool [UnityEngine.CoreModule]UnityEngine.GameObject::get_activeSelf()
		IL_03f9: stloc.s 33
		IL_03fb: ldloc.s 32
		IL_03fd: ldc.r4 0.0
		IL_0402: cgt
		IL_0404: brfalse.s IL_040c

		IL_0406: ldloc.s 33
		IL_0408: stloc.s 34
		IL_040a: br.s IL_0410

		IL_040c: nop
		IL_040d: ldc.i4.0
		IL_040e: stloc.s 34

		IL_0410: nop
		IL_0411: ldloc.s 34
		IL_0413: ldc.i4.0
		IL_0414: ceq
		IL_0416: brtrue.s IL_0435

		IL_0418: nop
		IL_0419: ldarg.0
		IL_041a: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::warning
		IL_041f: stloc.s 35
		IL_0421: br.s IL_042c

		IL_0423: nop
		IL_0424: ldarg.0
		IL_0425: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::warning
		IL_042a: stloc.s 35

		IL_042c: nop
		IL_042d: ldloc.s 35
		IL_042f: ldc.i4.1
		IL_0430: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)

		IL_0435: nop
		IL_0436: ldarg.0
		IL_0437: ldfld class PlayerScript GameControllerScript::player
		IL_043c: stloc.s 36
		IL_043e: ldloc.s 36
		IL_0440: ldfld bool PlayerScript::gameOver
		IL_0445: ldc.i4.0
		IL_0446: ceq
		IL_0448: brtrue IL_05a2

		IL_044d: nop
		IL_044e: ldarg.0
		IL_044f: ldfld string GameControllerScript::mode
		IL_0454: ldstr "endless"
		IL_0459: call bool [mscorlib]System.String::Equals(string, string)
		IL_045e: stloc.s 37
		IL_0460: ldloc.s 37
		IL_0462: ldc.i4.0
		IL_0463: ceq
		IL_0465: brtrue.s IL_04b4

		IL_0467: nop
		IL_0468: ldarg.0
		IL_0469: ldfld int32 GameControllerScript::notebooks
		IL_046e: stloc.s 38
		IL_0470: ldstr "HighBooks"
		IL_0475: call int32 [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::GetInt(string)
		IL_047a: stloc.s 39
		IL_047c: ldloc.s 38
		IL_047e: ldloc.s 39
		IL_0480: cgt
		IL_0482: ldc.i4.0
		IL_0483: ceq
		IL_0485: brtrue.s IL_04b4

		IL_0487: nop
		IL_0488: ldarg.0
		IL_0489: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::highScoreText
		IL_048e: stloc.s 35
		IL_0490: ldloc.s 35
		IL_0492: callvirt instance bool [UnityEngine.CoreModule]UnityEngine.GameObject::get_activeSelf()
		IL_0497: stloc.s 40
		IL_0499: ldloc.s 40
		IL_049b: ldc.i4.0
		IL_049c: ceq
		IL_049e: ldc.i4.0
		IL_049f: ceq
		IL_04a1: brtrue.s IL_04b4

		IL_04a3: nop
		IL_04a4: ldarg.0
		IL_04a5: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::highScoreText
		IL_04aa: stloc.s 35
		IL_04ac: ldloc.s 35
		IL_04ae: ldc.i4.1
		IL_04af: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)

		IL_04b4: nop
		IL_04b5: ldc.r4 0.0
		IL_04ba: call void [UnityEngine.CoreModule]UnityEngine.Time::set_timeScale(float32)
		IL_04bf: ldarg.0
		IL_04c0: ldfld float32 GameControllerScript::gameOverDelay
		IL_04c5: stloc.s 41
		IL_04c7: call float32 [UnityEngine.CoreModule]UnityEngine.Time::get_unscaledDeltaTime()
		IL_04cc: stloc.s 42
		IL_04ce: ldarg.0
		IL_04cf: ldfld class [UnityEngine.CoreModule]UnityEngine.Camera GameControllerScript::playerCamera
		IL_04d4: stloc.s 43
		IL_04d6: ldarg.0
		IL_04d7: ldloc.s 41
		IL_04d9: ldloc.s 42
		IL_04db: ldc.r4 0.5
		IL_04e0: mul
		IL_04e1: sub
		IL_04e2: stfld float32 GameControllerScript::gameOverDelay
		IL_04e7: ldloc.s 42
		IL_04e9: ldc.r4 0.5
		IL_04ee: mul
		IL_04ef: stloc.1
		IL_04f0: ldloc.s 43
		IL_04f2: ldloc.s 41
		IL_04f4: ldloc.s 42
		IL_04f6: ldc.r4 0.5
		IL_04fb: mul
		IL_04fc: sub
		IL_04fd: ldc.r4 400
		IL_0502: mul
		IL_0503: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Camera::set_farClipPlane(float32)
		IL_0508: ldarg.0
		IL_0509: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_050e: ldarg.0
		IL_050f: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip GameControllerScript::aud_buzz
		IL_0514: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_0519: ldc.r4 0.0
		IL_051e: ldarg.0
		IL_051f: ldfld float32 GameControllerScript::gameOverDelay
		IL_0524: clt
		IL_0526: brtrue.s IL_05a2

		IL_0528: nop
		IL_0529: ldarg.0
		IL_052a: ldfld string GameControllerScript::mode
		IL_052f: ldstr "endless"
		IL_0534: call bool [mscorlib]System.String::Equals(string, string)
		IL_0539: stloc.s 44
		IL_053b: ldloc.s 44
		IL_053d: ldc.i4.0
		IL_053e: ceq
		IL_0540: brtrue.s IL_0584

		IL_0542: nop
		IL_0543: ldarg.0
		IL_0544: ldfld int32 GameControllerScript::notebooks
		IL_0549: stloc.s 45
		IL_054b: ldstr "HighBooks"
		IL_0550: call int32 [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::GetInt(string)
		IL_0555: stloc.s 46
		IL_0557: ldloc.s 45
		IL_0559: ldloc.s 46
		IL_055b: cgt
		IL_055d: ldc.i4.0
		IL_055e: ceq
		IL_0560: brtrue.s IL_0573

		IL_0562: nop
		IL_0563: ldstr "HighBooks"
		IL_0568: ldarg.0
		IL_0569: ldfld int32 GameControllerScript::notebooks
		IL_056e: call void [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::SetInt(string, int32)

		IL_0573: nop
		IL_0574: ldstr "CurrentBooks"
		IL_0579: ldarg.0
		IL_057a: ldfld int32 GameControllerScript::notebooks
		IL_057f: call void [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::SetInt(string, int32)

		IL_0584: nop
		IL_0585: ldc.r4 1
		IL_058a: call void [UnityEngine.CoreModule]UnityEngine.Time::set_timeScale(float32)
		IL_058f: ldarg.0
		IL_0590: ldfld string GameControllerScript::GameOverScene
		IL_0595: stloc.s 47
		IL_0597: br.s IL_059a

		IL_0599: nop

		IL_059a: nop
		IL_059b: ldloc.s 47
		IL_059d: call void [UnityEngine.CoreModule]UnityEngine.SceneManagement.SceneManager::LoadScene(string)

		IL_05a2: nop
		IL_05a3: ldarg.0
		IL_05a4: ldfld bool GameControllerScript::finaleMode
		IL_05a9: ldc.i4.0
		IL_05aa: ceq
		IL_05ac: brtrue.s IL_0602

		IL_05ae: nop
		IL_05af: ldarg.0
		IL_05b0: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_05b5: callvirt instance bool [UnityEngine.AudioModule]UnityEngine.AudioSource::get_isPlaying()
		IL_05ba: stloc.s 48
		IL_05bc: ldloc.s 48
		IL_05be: stloc.s 49
		IL_05c0: ldloc.s 48
		IL_05c2: ldc.i4.0
		IL_05c3: ceq
		IL_05c5: ldc.i4.0
		IL_05c6: ceq
		IL_05c8: brtrue.s IL_0602

		IL_05ca: nop
		IL_05cb: ldarg.0
		IL_05cc: ldfld int32 GameControllerScript::exitsReached
		IL_05d1: ldc.i4.3
		IL_05d2: ceq
		IL_05d4: ldc.i4.0
		IL_05d5: ceq
		IL_05d7: brtrue.s IL_0602

		IL_05d9: nop
		IL_05da: ldarg.0
		IL_05db: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_05e0: ldarg.0
		IL_05e1: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip GameControllerScript::aud_MachineLoop
		IL_05e6: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_clip(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_05eb: ldarg.0
		IL_05ec: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_05f1: ldc.i4.1
		IL_05f2: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_loop(bool)
		IL_05f7: ldarg.0
		IL_05f8: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_05fd: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Play()

		IL_0602: nop
		IL_0603: ret

		IL_0604: nop
		IL_0605: ldnull
		IL_0606: throw

		IL_0607: nop
		IL_0608: ldnull
		IL_0609: throw
	} // end of method GameControllerScript::Update

	.method private hidebysig 
		instance void UpdateNotebookCount () cil managed 
	{
		// Method begins at RVA 0x3fe0
		// Header size: 12
		// Code size: 212 (0xd4)
		.maxstack 2
		.locals init (
			[0] bool,
			[1] int64,
			[2] class [Unity.TextMeshPro]TMPro.TMP_Text,
			[3] string,
			[4] string,
			[5] string,
			[6] string,
			[7] int32,
			[8] bool,
			[9] int32,
			[10] int32,
			[11] class EntranceScript
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld string GameControllerScript::mode
		IL_0007: ldstr "story"
		IL_000c: call bool [mscorlib]System.String::Equals(string, string)
		IL_0011: stloc.0
		IL_0012: ldarg.0
		IL_0013: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int64>(object)
		IL_0018: ldc.i8 304
		IL_0021: add
		IL_0022: stloc.1
		IL_0023: ldarg.0
		IL_0024: ldfld class [Unity.TextMeshPro]TMPro.TMP_Text GameControllerScript::notebookCount
		IL_0029: stloc.2
		IL_002a: ldloc.0
		IL_002b: ldc.i4.0
		IL_002c: ceq
		IL_002e: ldc.i4.0
		IL_002f: ceq
		IL_0031: brtrue.s IL_0040

		IL_0033: nop
		IL_0034: ldloc.1
		IL_0035: call instance string [mscorlib]System.Int32::ToString()
		IL_003a: stloc.3
		IL_003b: ldloc.3
		IL_003c: stloc.s 4
		IL_003e: br.s IL_004d

		IL_0040: nop
		IL_0041: ldloc.1
		IL_0042: call instance string [mscorlib]System.Int32::ToString()
		IL_0047: stloc.s 5
		IL_0049: ldloc.s 5
		IL_004b: stloc.s 4

		IL_004d: nop
		IL_004e: ldloc.s 4
		IL_0050: ldstr "/7 Notebooks"
		IL_0055: call string [mscorlib]System.String::Concat(string, string)
		IL_005a: stloc.s 6
		IL_005c: ldloc.s 6
		IL_005e: stloc.s 4
		IL_0060: ldarg.0
		IL_0061: ldfld int32 GameControllerScript::notebooks
		IL_0066: stloc.s 7
		IL_0068: ldarg.0
		IL_0069: ldfld string GameControllerScript::mode
		IL_006e: ldstr "story"
		IL_0073: call bool [mscorlib]System.String::Equals(string, string)
		IL_0078: stloc.s 8
		IL_007a: ldloc.s 7
		IL_007c: ldc.i4.7
		IL_007d: ceq
		IL_007f: brfalse.s IL_0087

		IL_0081: ldloc.s 8
		IL_0083: stloc.s 9
		IL_0085: br.s IL_008b

		IL_0087: nop
		IL_0088: ldc.i4.0
		IL_0089: stloc.s 9

		IL_008b: nop
		IL_008c: ldloc.s 8
		IL_008e: stloc.s 10
		IL_0090: ldloc.s 9
		IL_0092: ldc.i4.0
		IL_0093: ceq
		IL_0095: brtrue.s IL_00cf

		IL_0097: nop
		IL_0098: ldarg.0
		IL_0099: ldfld class EntranceScript GameControllerScript::entrance_0
		IL_009e: stloc.s 11
		IL_00a0: ldarg.0
		IL_00a1: ldc.i4.1
		IL_00a2: stfld bool GameControllerScript::finaleMode
		IL_00a7: ldloc.s 11
		IL_00a9: callvirt instance void EntranceScript::Raise()
		IL_00ae: ldarg.0
		IL_00af: ldfld class EntranceScript GameControllerScript::entrance_1
		IL_00b4: callvirt instance void EntranceScript::Raise()
		IL_00b9: ldarg.0
		IL_00ba: ldfld class EntranceScript GameControllerScript::entrance_2
		IL_00bf: callvirt instance void EntranceScript::Raise()
		IL_00c4: ldarg.0
		IL_00c5: ldfld class EntranceScript GameControllerScript::entrance_3
		IL_00ca: callvirt instance void EntranceScript::Raise()

		IL_00cf: nop
		IL_00d0: ret

		IL_00d1: nop
		IL_00d2: ldnull
		IL_00d3: throw
	} // end of method GameControllerScript::UpdateNotebookCount

	.method public hidebysig 
		instance void CollectNotebook () cil managed 
	{
		// Method begins at RVA 0x20b0
		// Header size: 1
		// Code size: 22 (0x16)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldarg.0
		IL_0003: ldfld int32 GameControllerScript::notebooks
		IL_0008: ldc.i4.1
		IL_0009: add
		IL_000a: stfld int32 GameControllerScript::notebooks
		IL_000f: ldarg.0
		IL_0010: callvirt instance void GameControllerScript::UpdateNotebookCount()
		IL_0015: ret
	} // end of method GameControllerScript::CollectNotebook

	.method public hidebysig 
		instance void LockMouse () cil managed 
	{
		// Method begins at RVA 0x40c0
		// Header size: 12
		// Code size: 65 (0x41)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.GameObject
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld bool GameControllerScript::learningActive
		IL_0007: ldc.i4.0
		IL_0008: ceq
		IL_000a: ldc.i4.0
		IL_000b: ceq
		IL_000d: brtrue.s IL_003c

		IL_000f: nop
		IL_0010: ldc.i4.1
		IL_0011: box [mscorlib]System.Int32
		IL_0016: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode>(object)
		IL_001b: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_lockState(valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode)
		IL_0020: ldc.i4.0
		IL_0021: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_visible(bool)
		IL_0026: ldarg.0
		IL_0027: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::reticle
		IL_002c: stloc.0
		IL_002d: ldarg.0
		IL_002e: ldc.i4.1
		IL_002f: stfld bool GameControllerScript::mouseLocked
		IL_0034: ldloc.0
		IL_0035: ldc.i4.1
		IL_0036: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_003b: ret

		IL_003c: nop
		IL_003d: ret

		IL_003e: nop
		IL_003f: ldnull
		IL_0040: throw
	} // end of method GameControllerScript::LockMouse

	.method public hidebysig 
		instance void UnlockMouse () cil managed 
	{
		// Method begins at RVA 0x4110
		// Header size: 12
		// Code size: 48 (0x30)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.GameObject
		)

		IL_0000: nop
		IL_0001: ldc.i4.0
		IL_0002: box [mscorlib]System.Int32
		IL_0007: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode>(object)
		IL_000c: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_lockState(valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode)
		IL_0011: ldc.i4.1
		IL_0012: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_visible(bool)
		IL_0017: ldarg.0
		IL_0018: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::reticle
		IL_001d: stloc.0
		IL_001e: ldarg.0
		IL_001f: ldc.i4.0
		IL_0020: stfld bool GameControllerScript::mouseLocked
		IL_0025: ldloc.0
		IL_0026: ldc.i4.0
		IL_0027: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_002c: ret

		IL_002d: nop
		IL_002e: ldnull
		IL_002f: throw
	} // end of method GameControllerScript::UnlockMouse

	.method public hidebysig 
		instance void PauseGame () cil managed 
	{
		// Method begins at RVA 0x414c
		// Header size: 12
		// Code size: 96 (0x60)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[1] class [UnityEngine.CoreModule]UnityEngine.GameObject
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld bool GameControllerScript::learningActive
		IL_0007: ldc.i4.0
		IL_0008: ceq
		IL_000a: ldc.i4.0
		IL_000b: ceq
		IL_000d: brtrue.s IL_005b

		IL_000f: nop
		IL_0010: ldc.i4.0
		IL_0011: box [mscorlib]System.Int32
		IL_0016: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode>(object)
		IL_001b: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_lockState(valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode)
		IL_0020: ldc.i4.1
		IL_0021: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_visible(bool)
		IL_0026: ldarg.0
		IL_0027: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::reticle
		IL_002c: stloc.0
		IL_002d: ldarg.0
		IL_002e: ldc.i4.0
		IL_002f: stfld bool GameControllerScript::mouseLocked
		IL_0034: ldloc.0
		IL_0035: ldc.i4.0
		IL_0036: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_003b: ldc.r4 0.0
		IL_0040: call void [UnityEngine.CoreModule]UnityEngine.Time::set_timeScale(float32)
		IL_0045: ldarg.0
		IL_0046: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::pauseMenu
		IL_004b: stloc.1
		IL_004c: ldarg.0
		IL_004d: ldc.i4.1
		IL_004e: stfld bool GameControllerScript::gamePaused
		IL_0053: ldloc.1
		IL_0054: ldc.i4.1
		IL_0055: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_005a: ret

		IL_005b: nop
		IL_005c: ret

		IL_005d: nop
		IL_005e: ldnull
		IL_005f: throw
	} // end of method GameControllerScript::PauseGame

	.method public hidebysig 
		instance void ExitGame () cil managed 
	{
		// Method begins at RVA 0x41b8
		// Header size: 12
		// Code size: 29 (0x1d)
		.maxstack 1
		.locals init (
			[0] string
		)

		IL_0000: nop
		IL_0001: ldc.r4 1
		IL_0006: call void [UnityEngine.CoreModule]UnityEngine.Time::set_timeScale(float32)
		IL_000b: ldarg.0
		IL_000c: ldfld string GameControllerScript::ExitGameScene
		IL_0011: stloc.0
		IL_0012: br.s IL_0015

		IL_0014: nop

		IL_0015: nop
		IL_0016: ldloc.0
		IL_0017: call void [UnityEngine.CoreModule]UnityEngine.SceneManagement.SceneManager::LoadScene(string)
		IL_001c: ret
	} // end of method GameControllerScript::ExitGame

	.method public hidebysig 
		instance void UnpauseGame () cil managed 
	{
		// Method begins at RVA 0x41e4
		// Header size: 12
		// Code size: 96 (0x60)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[1] class [UnityEngine.CoreModule]UnityEngine.GameObject
		)

		IL_0000: nop
		IL_0001: ldc.r4 1
		IL_0006: call void [UnityEngine.CoreModule]UnityEngine.Time::set_timeScale(float32)
		IL_000b: ldarg.0
		IL_000c: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::pauseMenu
		IL_0011: stloc.0
		IL_0012: ldarg.0
		IL_0013: ldc.i4.0
		IL_0014: stfld bool GameControllerScript::gamePaused
		IL_0019: ldloc.0
		IL_001a: ldc.i4.0
		IL_001b: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0020: ldarg.0
		IL_0021: ldfld bool GameControllerScript::learningActive
		IL_0026: ldc.i4.0
		IL_0027: ceq
		IL_0029: ldc.i4.0
		IL_002a: ceq
		IL_002c: brtrue.s IL_005b

		IL_002e: nop
		IL_002f: ldc.i4.1
		IL_0030: box [mscorlib]System.Int32
		IL_0035: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode>(object)
		IL_003a: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_lockState(valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode)
		IL_003f: ldc.i4.0
		IL_0040: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_visible(bool)
		IL_0045: ldarg.0
		IL_0046: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::reticle
		IL_004b: stloc.1
		IL_004c: ldarg.0
		IL_004d: ldc.i4.1
		IL_004e: stfld bool GameControllerScript::mouseLocked
		IL_0053: ldloc.1
		IL_0054: ldc.i4.1
		IL_0055: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_005a: ret

		IL_005b: nop
		IL_005c: ret

		IL_005d: nop
		IL_005e: ldnull
		IL_005f: throw
	} // end of method GameControllerScript::UnpauseGame

	.method public hidebysig 
		instance void ActivateSpoopMode () cil managed 
	{
		// Method begins at RVA 0x4250
		// Header size: 12
		// Code size: 191 (0xbf)
		.maxstack 2

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i4.1
		IL_0003: stfld bool GameControllerScript::spoopMode
		IL_0008: ldarg.0
		IL_0009: ldfld class EntranceScript GameControllerScript::entrance_0
		IL_000e: callvirt instance void EntranceScript::Lower()
		IL_0013: ldarg.0
		IL_0014: ldfld class EntranceScript GameControllerScript::entrance_1
		IL_0019: callvirt instance void EntranceScript::Lower()
		IL_001e: ldarg.0
		IL_001f: ldfld class EntranceScript GameControllerScript::entrance_2
		IL_0024: callvirt instance void EntranceScript::Lower()
		IL_0029: ldarg.0
		IL_002a: ldfld class EntranceScript GameControllerScript::entrance_3
		IL_002f: callvirt instance void EntranceScript::Lower()
		IL_0034: ldarg.0
		IL_0035: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::baldiTutor
		IL_003a: ldc.i4.0
		IL_003b: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0040: ldarg.0
		IL_0041: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::baldi
		IL_0046: ldc.i4.1
		IL_0047: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_004c: ldarg.0
		IL_004d: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::principal
		IL_0052: ldc.i4.1
		IL_0053: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0058: ldarg.0
		IL_0059: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::crafters
		IL_005e: ldc.i4.1
		IL_005f: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0064: ldarg.0
		IL_0065: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::playtime
		IL_006a: ldc.i4.1
		IL_006b: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0070: ldarg.0
		IL_0071: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::gottaSweep
		IL_0076: ldc.i4.1
		IL_0077: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_007c: ldarg.0
		IL_007d: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::bully
		IL_0082: ldc.i4.1
		IL_0083: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0088: ldarg.0
		IL_0089: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::firstPrize
		IL_008e: ldc.i4.1
		IL_008f: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0094: ldarg.0
		IL_0095: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_009a: ldarg.0
		IL_009b: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip GameControllerScript::aud_Hang
		IL_00a0: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_00a5: ldarg.0
		IL_00a6: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::learnMusic
		IL_00ab: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Stop()
		IL_00b0: ldarg.0
		IL_00b1: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::schoolMusic
		IL_00b6: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Stop()
		IL_00bb: ret

		IL_00bc: nop
		IL_00bd: ldnull
		IL_00be: throw
	} // end of method GameControllerScript::ActivateSpoopMode

	.method private hidebysig 
		instance void ActivateFinaleMode () cil managed 
	{
		// Method begins at RVA 0x20c7
		// Header size: 1
		// Code size: 56 (0x38)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i4.1
		IL_0003: stfld bool GameControllerScript::finaleMode
		IL_0008: ldarg.0
		IL_0009: ldfld class EntranceScript GameControllerScript::entrance_0
		IL_000e: callvirt instance void EntranceScript::Raise()
		IL_0013: ldarg.0
		IL_0014: ldfld class EntranceScript GameControllerScript::entrance_1
		IL_0019: callvirt instance void EntranceScript::Raise()
		IL_001e: ldarg.0
		IL_001f: ldfld class EntranceScript GameControllerScript::entrance_2
		IL_0024: callvirt instance void EntranceScript::Raise()
		IL_0029: ldarg.0
		IL_002a: ldfld class EntranceScript GameControllerScript::entrance_3
		IL_002f: callvirt instance void EntranceScript::Raise()
		IL_0034: ret

		IL_0035: nop
		IL_0036: ldnull
		IL_0037: throw
	} // end of method GameControllerScript::ActivateFinaleMode

	.method public hidebysig 
		instance void GetAngry (
			float32 'value'
		) cil managed 
	{
		// Method begins at RVA 0x431c
		// Header size: 12
		// Code size: 145 (0x91)
		.maxstack 4
		.locals init (
			[0] class BaldiScript,
			[1] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld bool GameControllerScript::spoopMode
		IL_0007: ldc.i4.0
		IL_0008: ceq
		IL_000a: ldc.i4.0
		IL_000b: ceq
		IL_000d: brtrue.s IL_0016

		IL_000f: nop
		IL_0010: ldarg.0
		IL_0011: callvirt instance void GameControllerScript::ActivateSpoopMode()

		IL_0016: nop
		IL_0017: ldarg.0
		IL_0018: ldfld class BaldiScript GameControllerScript::baldiScrpt
		IL_001d: stloc.0
		IL_001e: ldloc.0
		IL_001f: ldarg.1
		IL_0020: ldloc.0
		IL_0021: ldfld float32 BaldiScript::baldiAnger
		IL_0026: add
		IL_0027: stfld float32 BaldiScript::baldiAnger
		IL_002c: ldc.r4 0.5
		IL_0031: ldarg.1
		IL_0032: ldloc.0
		IL_0033: ldfld float32 BaldiScript::baldiAnger
		IL_0038: add
		IL_0039: cgt
		IL_003b: ldc.i4.0
		IL_003c: ceq
		IL_003e: brtrue.s IL_004c

		IL_0040: nop
		IL_0041: ldloc.0
		IL_0042: ldc.r4 0.5
		IL_0047: stfld float32 BaldiScript::baldiAnger

		IL_004c: nop
		IL_004d: ldc.r4 2
		IL_0052: ldloc.0
		IL_0053: ldfld float32 BaldiScript::baldiSpeedScale
		IL_0058: div
		IL_0059: ldloc.0
		IL_005a: ldfld float32 BaldiScript::baldiAnger
		IL_005f: add
		IL_0060: stloc.1
		IL_0061: ldloc.0
		IL_0062: ldloc.0
		IL_0063: ldfld float32 BaldiScript::baldiAnger
		IL_0068: ldc.r4 -3
		IL_006d: mul
		IL_006e: ldc.r4 2
		IL_0073: ldloc.0
		IL_0074: ldfld float32 BaldiScript::baldiSpeedScale
		IL_0079: div
		IL_007a: ldloc.0
		IL_007b: ldfld float32 BaldiScript::baldiAnger
		IL_0080: add
		IL_0081: div
		IL_0082: ldc.r4 3
		IL_0087: add
		IL_0088: stfld float32 BaldiScript::baldiWait
		IL_008d: ret

		IL_008e: nop
		IL_008f: ldnull
		IL_0090: throw
	} // end of method GameControllerScript::GetAngry

	.method public hidebysig 
		instance void ActivateLearningGame () cil managed 
	{
		// Method begins at RVA 0x43bc
		// Header size: 12
		// Code size: 105 (0x69)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.GameObject
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldc.i4.1
		IL_0003: stfld bool GameControllerScript::learningActive
		IL_0008: ldc.i4.0
		IL_0009: box [mscorlib]System.Int32
		IL_000e: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode>(object)
		IL_0013: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_lockState(valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode)
		IL_0018: ldc.i4.1
		IL_0019: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_visible(bool)
		IL_001e: ldarg.0
		IL_001f: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::reticle
		IL_0024: stloc.0
		IL_0025: ldarg.0
		IL_0026: ldc.i4.0
		IL_0027: stfld bool GameControllerScript::mouseLocked
		IL_002c: ldloc.0
		IL_002d: ldc.i4.0
		IL_002e: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0033: ldarg.0
		IL_0034: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::tutorBaldi
		IL_0039: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Stop()
		IL_003e: ldarg.0
		IL_003f: ldfld bool GameControllerScript::spoopMode
		IL_0044: ldc.i4.0
		IL_0045: ceq
		IL_0047: ldc.i4.0
		IL_0048: ceq
		IL_004a: brtrue.s IL_0064

		IL_004c: nop
		IL_004d: ldarg.0
		IL_004e: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::schoolMusic
		IL_0053: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Stop()
		IL_0058: ldarg.0
		IL_0059: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::learnMusic
		IL_005e: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Play()
		IL_0063: ret

		IL_0064: nop
		IL_0065: ret

		IL_0066: nop
		IL_0067: ldnull
		IL_0068: throw
	} // end of method GameControllerScript::ActivateLearningGame

	.method public hidebysig 
		instance void DeactivateLearningGame (
			class [UnityEngine.CoreModule]UnityEngine.GameObject subject
		) cil managed 
	{
		// Method begins at RVA 0x4434
		// Header size: 12
		// Code size: 312 (0x138)
		.maxstack 3
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[1] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[2] class PlayerScript,
			[3] int32,
			[4] bool,
			[5] int32,
			[6] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.Camera GameControllerScript::playerCamera
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: ldarg.0
		IL_000a: ldfld int32 GameControllerScript::cullingMask
		IL_000f: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Camera::set_cullingMask(int32)
		IL_0014: ldarg.0
		IL_0015: ldc.i4.0
		IL_0016: stfld bool GameControllerScript::learningActive
		IL_001b: br.s IL_001e

		IL_001d: nop

		IL_001e: nop
		IL_001f: ldarg.1
		IL_0020: call void [UnityEngine.CoreModule]UnityEngine.Object::Destroy(class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0025: ldarg.0
		IL_0026: ldfld bool GameControllerScript::learningActive
		IL_002b: ldc.i4.0
		IL_002c: ceq
		IL_002e: ldc.i4.0
		IL_002f: ceq
		IL_0031: brtrue.s IL_005f

		IL_0033: nop
		IL_0034: ldc.i4.1
		IL_0035: box [mscorlib]System.Int32
		IL_003a: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode>(object)
		IL_003f: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_lockState(valuetype [UnityEngine.CoreModule]UnityEngine.CursorLockMode)
		IL_0044: ldc.i4.0
		IL_0045: call void [UnityEngine.CoreModule]UnityEngine.Cursor::set_visible(bool)
		IL_004a: ldarg.0
		IL_004b: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::reticle
		IL_0050: stloc.1
		IL_0051: ldarg.0
		IL_0052: ldc.i4.1
		IL_0053: stfld bool GameControllerScript::mouseLocked
		IL_0058: ldloc.1
		IL_0059: ldc.i4.1
		IL_005a: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)

		IL_005f: nop
		IL_0060: ldarg.0
		IL_0061: ldfld class PlayerScript GameControllerScript::player
		IL_0066: stloc.2
		IL_0067: ldc.r4 100
		IL_006c: ldloc.2
		IL_006d: ldfld float32 PlayerScript::stamina
		IL_0072: cgt
		IL_0074: ldc.i4.0
		IL_0075: ceq
		IL_0077: brtrue.s IL_0085

		IL_0079: nop
		IL_007a: ldloc.2
		IL_007b: ldc.r4 100
		IL_0080: stfld float32 PlayerScript::stamina

		IL_0085: nop
		IL_0086: ldarg.0
		IL_0087: ldfld bool GameControllerScript::spoopMode
		IL_008c: ldc.i4.0
		IL_008d: ceq
		IL_008f: ldc.i4.0
		IL_0090: ceq
		IL_0092: brtrue.s IL_00ab

		IL_0094: nop
		IL_0095: ldarg.0
		IL_0096: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::schoolMusic
		IL_009b: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Play()
		IL_00a0: ldarg.0
		IL_00a1: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::learnMusic
		IL_00a6: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Stop()

		IL_00ab: nop
		IL_00ac: ldarg.0
		IL_00ad: ldfld int32 GameControllerScript::notebooks
		IL_00b2: ldc.i4.1
		IL_00b3: ceq
		IL_00b5: ldarg.0
		IL_00b6: ldfld bool GameControllerScript::spoopMode
		IL_00bb: ldc.i4.0
		IL_00bc: ceq
		IL_00be: and
		IL_00bf: ldc.i4.0
		IL_00c0: ceq
		IL_00c2: ldc.i4.0
		IL_00c3: ceq
		IL_00c5: brtrue.s IL_0116

		IL_00c7: nop
		IL_00c8: ldarg.0
		IL_00c9: ldfld int32 GameControllerScript::notebooks
		IL_00ce: stloc.3
		IL_00cf: ldarg.0
		IL_00d0: ldfld string GameControllerScript::mode
		IL_00d5: ldstr "story"
		IL_00da: call bool [mscorlib]System.String::Equals(string, string)
		IL_00df: stloc.s 4
		IL_00e1: ldloc.3
		IL_00e2: ldc.i4.7
		IL_00e3: ceq
		IL_00e5: brfalse.s IL_00ed

		IL_00e7: ldloc.s 4
		IL_00e9: stloc.s 5
		IL_00eb: br.s IL_00f1

		IL_00ed: nop
		IL_00ee: ldc.i4.0
		IL_00ef: stloc.s 5

		IL_00f1: nop
		IL_00f2: ldloc.s 4
		IL_00f4: stloc.s 6
		IL_00f6: ldloc.s 5
		IL_00f8: ldc.i4.0
		IL_00f9: ceq
		IL_00fb: brtrue.s IL_0114

		IL_00fd: nop
		IL_00fe: ldarg.0
		IL_00ff: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_0104: ldarg.0
		IL_0105: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip GameControllerScript::aud_AllNotebooks
		IL_010a: ldc.r4 0.8
		IL_010f: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip, float32)

		IL_0114: nop
		IL_0115: ret

		IL_0116: nop
		IL_0117: ldarg.0
		IL_0118: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::quarter
		IL_011d: ldc.i4.1
		IL_011e: callvirt instance void [UnityEngine.CoreModule]UnityEngine.GameObject::SetActive(bool)
		IL_0123: ldarg.0
		IL_0124: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::tutorBaldi
		IL_0129: ldarg.0
		IL_012a: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip GameControllerScript::aud_Prize
		IL_012f: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_0134: ret

		IL_0135: nop
		IL_0136: ldnull
		IL_0137: throw
	} // end of method GameControllerScript::DeactivateLearningGame

	.method private hidebysig 
		instance void IncreaseItemSelection () cil managed 
	{
		// Method begins at RVA 0x4578
		// Header size: 12
		// Code size: 72 (0x48)
		.maxstack 2
		.locals init (
			[0] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld int32 GameControllerScript::itemSelected
		IL_0007: ldc.i4.1
		IL_0008: add
		IL_0009: stloc.0
		IL_000a: ldarg.0
		IL_000b: ldloc.0
		IL_000c: stfld int32 GameControllerScript::itemSelected
		IL_0011: ldloc.0
		IL_0012: ldc.i4.2
		IL_0013: cgt
		IL_0015: ldc.i4.0
		IL_0016: ceq
		IL_0018: brtrue.s IL_0022

		IL_001a: nop
		IL_001b: ldarg.0
		IL_001c: ldc.i4.0
		IL_001d: stfld int32 GameControllerScript::itemSelected

		IL_0022: nop
		IL_0023: ldarg.0
		IL_0024: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform GameControllerScript::itemSelect
		IL_0029: ldstr "unpcklps xmm0,xmm1"
		IL_002e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0033: box [mscorlib]System.Int64
		IL_0038: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector2>(object)
		IL_003d: callvirt instance void [UnityEngine.CoreModule]UnityEngine.RectTransform::set_anchoredPosition(valuetype [UnityEngine.CoreModule]UnityEngine.Vector2)
		IL_0042: nop
		IL_0043: ldnull
		IL_0044: throw

		IL_0045: nop
		IL_0046: ldnull
		IL_0047: throw
	} // end of method GameControllerScript::IncreaseItemSelection

	.method private hidebysig 
		instance void DecreaseItemSelection () cil managed 
	{
		// Method begins at RVA 0x45cc
		// Header size: 12
		// Code size: 77 (0x4d)
		.maxstack 3

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldarg.0
		IL_0003: ldfld int32 GameControllerScript::itemSelected
		IL_0008: ldc.i4.1
		IL_0009: sub
		IL_000a: stfld int32 GameControllerScript::itemSelected
		IL_000f: ldarg.0
		IL_0010: ldfld int32 GameControllerScript::itemSelected
		IL_0015: ldc.i4.1
		IL_0016: sub
		IL_0017: ldc.i4.0
		IL_0018: clt
		IL_001a: ldc.i4.0
		IL_001b: ceq
		IL_001d: brtrue.s IL_0027

		IL_001f: nop
		IL_0020: ldarg.0
		IL_0021: ldc.i4.2
		IL_0022: stfld int32 GameControllerScript::itemSelected

		IL_0027: nop
		IL_0028: ldarg.0
		IL_0029: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform GameControllerScript::itemSelect
		IL_002e: ldstr "unpcklps xmm0,xmm1"
		IL_0033: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0038: box [mscorlib]System.Int64
		IL_003d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector2>(object)
		IL_0042: callvirt instance void [UnityEngine.CoreModule]UnityEngine.RectTransform::set_anchoredPosition(valuetype [UnityEngine.CoreModule]UnityEngine.Vector2)
		IL_0047: nop
		IL_0048: ldnull
		IL_0049: throw

		IL_004a: nop
		IL_004b: ldnull
		IL_004c: throw
	} // end of method GameControllerScript::DecreaseItemSelection

	.method private hidebysig 
		instance void UpdateItemSelection () cil managed 
	{
		// Method begins at RVA 0x2100
		// Header size: 1
		// Code size: 38 (0x26)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.RectTransform GameControllerScript::itemSelect
		IL_0007: ldstr "unpcklps xmm0,xmm1"
		IL_000c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0011: box [mscorlib]System.Int64
		IL_0016: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector2>(object)
		IL_001b: callvirt instance void [UnityEngine.CoreModule]UnityEngine.RectTransform::set_anchoredPosition(valuetype [UnityEngine.CoreModule]UnityEngine.Vector2)
		IL_0020: nop
		IL_0021: ldnull
		IL_0022: throw

		IL_0023: nop
		IL_0024: ldnull
		IL_0025: throw
	} // end of method GameControllerScript::UpdateItemSelection

	.method public hidebysig 
		instance void CollectItem (
			int32 item_ID
		) cil managed 
	{
		// Method begins at RVA 0x4628
		// Header size: 12
		// Code size: 251 (0xfb)
		.maxstack 3
		.locals init (
			[0] int32[],
			[1] int32,
			[2] int32[],
			[3] class [UnityEngine.UI]UnityEngine.UI.RawImage[],
			[4] class [UnityEngine.CoreModule]UnityEngine.Texture[],
			[5] class [UnityEngine.UI]UnityEngine.UI.RawImage,
			[6] class [UnityEngine.UI]UnityEngine.UI.RawImage[],
			[7] class [UnityEngine.UI]UnityEngine.UI.RawImage,
			[8] class [UnityEngine.UI]UnityEngine.UI.RawImage[],
			[9] class [UnityEngine.UI]UnityEngine.UI.RawImage,
			[10] class [UnityEngine.UI]UnityEngine.UI.RawImage[],
			[11] class [UnityEngine.UI]UnityEngine.UI.RawImage
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld int32[] GameControllerScript::item
		IL_0007: stloc.0
		IL_0008: ldloc.0
		IL_0009: ldlen
		IL_000a: conv.i4
		IL_000b: stloc.1
		IL_000c: ldloc.0
		IL_000d: stloc.2
		IL_000e: ldloc.0
		IL_000f: ldc.i4.0
		IL_0010: ldelem [mscorlib]System.Int32
		IL_0015: ldc.i4.0
		IL_0016: ceq
		IL_0018: brtrue IL_00b6

		IL_001d: nop
		IL_001e: ldloc.0
		IL_001f: ldlen
		IL_0020: conv.i4
		IL_0021: stloc.1
		IL_0022: ldloc.0
		IL_0023: ldc.i4.1
		IL_0024: ldelem [mscorlib]System.Int32
		IL_0029: ldc.i4.0
		IL_002a: ceq
		IL_002c: brtrue.s IL_0091

		IL_002e: nop
		IL_002f: ldloc.0
		IL_0030: ldc.i4.2
		IL_0031: ldelem [mscorlib]System.Int32
		IL_0036: ldc.i4.0
		IL_0037: ceq
		IL_0039: brtrue.s IL_006c

		IL_003b: nop
		IL_003c: ldloc.2
		IL_003d: ldarg.0
		IL_003e: ldfld int32 GameControllerScript::itemSelected
		IL_0043: conv.i8
		IL_0044: conv.i4
		IL_0045: ldarg.1
		IL_0046: stelem [mscorlib]System.Int32
		IL_004b: ldarg.0
		IL_004c: ldfld class [UnityEngine.UI]UnityEngine.UI.RawImage[] GameControllerScript::itemSlot
		IL_0051: stloc.3
		IL_0052: ldarg.0
		IL_0053: ldfld class [UnityEngine.CoreModule]UnityEngine.Texture[] GameControllerScript::itemTextures
		IL_0058: stloc.s 4
		IL_005a: ldloc.3
		IL_005b: ldarg.0
		IL_005c: ldfld int32 GameControllerScript::itemSelected
		IL_0061: conv.i8
		IL_0062: conv.i4
		IL_0063: ldelem [UnityEngine.UI]UnityEngine.UI.RawImage
		IL_0068: stloc.s 5
		IL_006a: br.s IL_00d9

		IL_006c: nop
		IL_006d: ldloc.0
		IL_006e: ldc.i4.2
		IL_006f: ldarg.1
		IL_0070: stelem [mscorlib]System.Int32
		IL_0075: ldarg.0
		IL_0076: ldfld class [UnityEngine.UI]UnityEngine.UI.RawImage[] GameControllerScript::itemSlot
		IL_007b: stloc.s 6
		IL_007d: ldarg.0
		IL_007e: ldfld class [UnityEngine.CoreModule]UnityEngine.Texture[] GameControllerScript::itemTextures
		IL_0083: stloc.s 4
		IL_0085: ldloc.s 6
		IL_0087: ldc.i4.2
		IL_0088: ldelem [UnityEngine.UI]UnityEngine.UI.RawImage
		IL_008d: stloc.s 7
		IL_008f: br.s IL_00d9

		IL_0091: nop
		IL_0092: ldloc.0
		IL_0093: ldc.i4.1
		IL_0094: ldarg.1
		IL_0095: stelem [mscorlib]System.Int32
		IL_009a: ldarg.0
		IL_009b: ldfld class [UnityEngine.UI]UnityEngine.UI.RawImage[] GameControllerScript::itemSlot
		IL_00a0: stloc.s 8
		IL_00a2: ldarg.0
		IL_00a3: ldfld class [UnityEngine.CoreModule]UnityEngine.Texture[] GameControllerScript::itemTextures
		IL_00a8: stloc.s 4
		IL_00aa: ldloc.s 8
		IL_00ac: ldc.i4.1
		IL_00ad: ldelem [UnityEngine.UI]UnityEngine.UI.RawImage
		IL_00b2: stloc.s 9
		IL_00b4: br.s IL_00d9

		IL_00b6: nop
		IL_00b7: ldloc.0
		IL_00b8: ldc.i4.0
		IL_00b9: ldarg.1
		IL_00ba: stelem [mscorlib]System.Int32
		IL_00bf: ldarg.0
		IL_00c0: ldfld class [UnityEngine.UI]UnityEngine.UI.RawImage[] GameControllerScript::itemSlot
		IL_00c5: stloc.s 10
		IL_00c7: ldarg.0
		IL_00c8: ldfld class [UnityEngine.CoreModule]UnityEngine.Texture[] GameControllerScript::itemTextures
		IL_00cd: stloc.s 4
		IL_00cf: ldloc.s 10
		IL_00d1: ldc.i4.0
		IL_00d2: ldelem [UnityEngine.UI]UnityEngine.UI.RawImage
		IL_00d7: stloc.s 11

		IL_00d9: nop
		IL_00da: ldloc.s 11
		IL_00dc: ldstr "*(BinaryExpr + 0x0)"
		IL_00e1: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00e6: box [mscorlib]System.Int64
		IL_00eb: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [UnityEngine.CoreModule]UnityEngine.Texture>(object)
		IL_00f0: callvirt instance void [UnityEngine.UI]UnityEngine.UI.RawImage::set_texture(class [UnityEngine.CoreModule]UnityEngine.Texture)
		IL_00f5: nop
		IL_00f6: ldnull
		IL_00f7: throw

		IL_00f8: nop
		IL_00f9: ldnull
		IL_00fa: throw
	} // end of method GameControllerScript::CollectItem

	.method private hidebysig 
		instance void UseItem () cil managed 
	{
		// Method begins at RVA 0x4730
		// Header size: 12
		// Code size: 4158 (0x103e)
		.maxstack 4
		.locals init (
			[0] int32[],
			[1] int64,
			[2] int32[],
			[3] int64,
			[4] class GameControllerScript/'<Teleporter>d__20',
			[5] class [UnityEngine.CoreModule]UnityEngine.Coroutine,
			[6] class PlayerScript,
			[7] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[8] int32,
			[9] int32,
			[10] float32,
			[11] int32,
			[12] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[13] valuetype [UnityEngine.CoreModule]UnityEngine.Ray,
			[14] int64,
			[15] int64,
			[16] valuetype [UnityEngine.CoreModule]UnityEngine.Ray,
			[17] bool,
			[18] bool,
			[19] class [UnityEngine.PhysicsModule]UnityEngine.Collider,
			[20] class [UnityEngine.PhysicsModule]UnityEngine.Collider,
			[21] string,
			[22] bool,
			[23] bool,
			[24] class PlaytimeScript,
			[25] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[26] int32,
			[27] int32,
			[28] float32,
			[29] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[30] valuetype [UnityEngine.CoreModule]UnityEngine.Ray,
			[31] int64,
			[32] valuetype [UnityEngine.CoreModule]UnityEngine.Ray,
			[33] bool,
			[34] class [UnityEngine.PhysicsModule]UnityEngine.Collider,
			[35] class [UnityEngine.PhysicsModule]UnityEngine.Collider,
			[36] string,
			[37] bool,
			[38] bool,
			[39] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[40] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[41] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[42] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[43] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[44] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[45] float32,
			[46] int32,
			[47] int32,
			[48] class [UnityEngine.PhysicsModule]UnityEngine.Collider,
			[49] class [UnityEngine.PhysicsModule]UnityEngine.Collider,
			[50] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[51] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[52] class DoorScript,
			[53] class DoorScript,
			[54] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[55] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[56] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[57] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[58] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[59] float32,
			[60] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[61] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[62] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[63] class AlarmClockScript,
			[64] class BaldiScript,
			[65] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[66] int32,
			[67] int32,
			[68] float32,
			[69] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[70] valuetype [UnityEngine.CoreModule]UnityEngine.Ray,
			[71] int64,
			[72] valuetype [UnityEngine.CoreModule]UnityEngine.Ray,
			[73] bool,
			[74] bool,
			[75] class [UnityEngine.PhysicsModule]UnityEngine.Collider,
			[76] class [UnityEngine.PhysicsModule]UnityEngine.Collider,
			[77] string,
			[78] bool,
			[79] bool,
			[80] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[81] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[82] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[83] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[84] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[85] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[86] float32,
			[87] int32,
			[88] int32,
			[89] int64,
			[90] class [UnityEngine.PhysicsModule]UnityEngine.Collider,
			[91] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[92] class TapePlayerScript,
			[93] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[94] int32,
			[95] int32,
			[96] float32,
			[97] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[98] valuetype [UnityEngine.CoreModule]UnityEngine.Ray,
			[99] int64,
			[100] valuetype [UnityEngine.CoreModule]UnityEngine.Ray,
			[101] valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit,
			[102] bool,
			[103] bool,
			[104] valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit,
			[105] class [UnityEngine.PhysicsModule]UnityEngine.Collider,
			[106] string,
			[107] bool,
			[108] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[109] valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit,
			[110] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[111] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[112] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[113] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[114] float32,
			[115] int32,
			[116] valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit,
			[117] class [UnityEngine.PhysicsModule]UnityEngine.Collider,
			[118] string,
			[119] bool,
			[120] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[121] valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit,
			[122] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[123] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[124] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[125] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[126] float32,
			[127] int32,
			[128] valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit,
			[129] class [UnityEngine.PhysicsModule]UnityEngine.Collider,
			[130] class [UnityEngine.PhysicsModule]UnityEngine.Collider,
			[131] string,
			[132] bool,
			[133] bool,
			[134] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[135] valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit,
			[136] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[137] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[138] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[139] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[140] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[141] float32,
			[142] int32,
			[143] int32,
			[144] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[145] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[146] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[147] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[148] valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion,
			[149] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[150] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[151] int32,
			[152] int32,
			[153] float32,
			[154] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[155] valuetype [UnityEngine.CoreModule]UnityEngine.Ray,
			[156] int64,
			[157] valuetype [UnityEngine.CoreModule]UnityEngine.Ray,
			[158] bool,
			[159] bool,
			[160] class [UnityEngine.PhysicsModule]UnityEngine.Collider,
			[161] class [UnityEngine.PhysicsModule]UnityEngine.Collider,
			[162] string,
			[163] bool,
			[164] bool,
			[165] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[166] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[167] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[168] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[169] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[170] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[171] float32,
			[172] int32,
			[173] int32,
			[174] class [UnityEngine.PhysicsModule]UnityEngine.Collider,
			[175] class [UnityEngine.PhysicsModule]UnityEngine.Collider,
			[176] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[177] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[178] class DoorScript,
			[179] class DoorScript,
			[180] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[181] int32,
			[182] int32,
			[183] float32,
			[184] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[185] valuetype [UnityEngine.CoreModule]UnityEngine.Ray,
			[186] int64,
			[187] valuetype [UnityEngine.CoreModule]UnityEngine.Ray,
			[188] bool,
			[189] bool,
			[190] class [UnityEngine.PhysicsModule]UnityEngine.Collider,
			[191] class [UnityEngine.PhysicsModule]UnityEngine.Collider,
			[192] string,
			[193] bool,
			[194] bool,
			[195] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[196] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[197] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[198] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[199] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[200] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[201] float32,
			[202] int32,
			[203] int32,
			[204] class [UnityEngine.PhysicsModule]UnityEngine.Collider,
			[205] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[206] class SwingingDoorScript,
			[207] float32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld int32[] GameControllerScript::item
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld int32 GameControllerScript::itemSelected
		IL_000e: conv.i8
		IL_000f: stloc.1
		IL_0010: ldloc.0
		IL_0011: ldloc.1
		IL_0012: conv.i4
		IL_0013: ldelem [mscorlib]System.Int32
		IL_0018: ldc.i4.0
		IL_0019: ceq
		IL_001b: brtrue IL_07dd

		IL_0020: nop
		IL_0021: ldarg.0
		IL_0022: ldfld int32[] GameControllerScript::item
		IL_0027: stloc.2
		IL_0028: ldarg.0
		IL_0029: ldfld int32[] GameControllerScript::item
		IL_002e: ldarg.0
		IL_002f: ldfld int32 GameControllerScript::itemSelected
		IL_0034: conv.i8
		IL_0035: conv.i4
		IL_0036: ldelem [mscorlib]System.Int32
		IL_003b: ldc.i4.1
		IL_003c: ceq
		IL_003e: brtrue IL_1008

		IL_0043: nop
		IL_0044: ldloc.2
		IL_0045: ldarg.0
		IL_0046: ldfld int32 GameControllerScript::itemSelected
		IL_004b: conv.i8
		IL_004c: conv.i4
		IL_004d: ldelem [mscorlib]System.Int32
		IL_0052: ldc.i4.2
		IL_0053: ceq
		IL_0055: brtrue IL_0e0a

		IL_005a: nop
		IL_005b: ldloc.2
		IL_005c: ldarg.0
		IL_005d: ldfld int32 GameControllerScript::itemSelected
		IL_0062: conv.i8
		IL_0063: conv.i4
		IL_0064: ldelem [mscorlib]System.Int32
		IL_0069: ldc.i4.3
		IL_006a: ceq
		IL_006c: brtrue IL_0be9

		IL_0071: nop
		IL_0072: ldloc.2
		IL_0073: ldarg.0
		IL_0074: ldfld int32 GameControllerScript::itemSelected
		IL_0079: conv.i8
		IL_007a: conv.i4
		IL_007b: ldelem [mscorlib]System.Int32
		IL_0080: ldc.i4.4
		IL_0081: ceq
		IL_0083: brtrue IL_0b28

		IL_0088: nop
		IL_0089: ldloc.2
		IL_008a: ldarg.0
		IL_008b: ldfld int32 GameControllerScript::itemSelected
		IL_0090: conv.i8
		IL_0091: conv.i4
		IL_0092: ldelem [mscorlib]System.Int32
		IL_0097: ldc.i4.5
		IL_0098: ceq
		IL_009a: brtrue IL_07df

		IL_009f: nop
		IL_00a0: ldloc.2
		IL_00a1: ldarg.0
		IL_00a2: ldfld int32 GameControllerScript::itemSelected
		IL_00a7: conv.i8
		IL_00a8: conv.i4
		IL_00a9: ldelem [mscorlib]System.Int32
		IL_00ae: ldc.i4.6
		IL_00af: ceq
		IL_00b1: brtrue IL_05e0

		IL_00b6: nop
		IL_00b7: ldloc.2
		IL_00b8: ldarg.0
		IL_00b9: ldfld int32 GameControllerScript::itemSelected
		IL_00be: conv.i8
		IL_00bf: conv.i4
		IL_00c0: ldelem [mscorlib]System.Int32
		IL_00c5: ldc.i4.7
		IL_00c6: ceq
		IL_00c8: brtrue IL_0531

		IL_00cd: nop
		IL_00ce: ldloc.2
		IL_00cf: ldarg.0
		IL_00d0: ldfld int32 GameControllerScript::itemSelected
		IL_00d5: conv.i8
		IL_00d6: conv.i4
		IL_00d7: ldelem [mscorlib]System.Int32
		IL_00dc: ldc.i4.8
		IL_00dd: ceq
		IL_00df: brtrue IL_0314

		IL_00e4: nop
		IL_00e5: ldloc.2
		IL_00e6: ldarg.0
		IL_00e7: ldfld int32 GameControllerScript::itemSelected
		IL_00ec: conv.i8
		IL_00ed: conv.i4
		IL_00ee: ldelem [mscorlib]System.Int32
		IL_00f3: ldc.i4.s 9
		IL_00f5: ceq
		IL_00f7: brtrue IL_0180

		IL_00fc: nop
		IL_00fd: ldloc.2
		IL_00fe: ldarg.0
		IL_00ff: ldfld int32 GameControllerScript::itemSelected
		IL_0104: conv.i8
		IL_0105: conv.i4
		IL_0106: ldelem [mscorlib]System.Int32
		IL_010b: ldc.i4.s 10
		IL_010d: ceq
		IL_010f: brtrue.s IL_016e

		IL_0111: nop
		IL_0112: ldarg.0
		IL_0113: ldfld int32 GameControllerScript::itemSelected
		IL_0118: conv.i8
		IL_0119: stloc.3
		IL_011a: ldloc.2
		IL_011b: ldarg.0
		IL_011c: ldfld int32 GameControllerScript::itemSelected
		IL_0121: conv.i8
		IL_0122: conv.i4
		IL_0123: ldelem [mscorlib]System.Int32
		IL_0128: ldc.i4.s 11
		IL_012a: ceq
		IL_012c: ldc.i4.0
		IL_012d: ceq
		IL_012f: brtrue IL_07dd

		IL_0134: nop

		IL_0135: nop
		IL_0136: ldstr "il2cpp_codegen_object_new(GameControllerScript+<Teleporter>d__20)"
		IL_013b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0140: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class GameControllerScript/'<Teleporter>d__20'>(object)
		IL_0145: stloc.s 4
		IL_0147: ldloc.s 4
		IL_0149: ldnull
		IL_014a: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_014f: ldloc.s 4
		IL_0151: ldc.i4.0
		IL_0152: stfld int32 GameControllerScript/'<Teleporter>d__20'::'<>1__state'
		IL_0157: ldloc.s 4
		IL_0159: ldarg.0
		IL_015a: stfld class GameControllerScript GameControllerScript/'<Teleporter>d__20'::'<>4__this'
		IL_015f: ldarg.0
		IL_0160: ldloc.s 4
		IL_0162: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Coroutine [UnityEngine.CoreModule]UnityEngine.MonoBehaviour::StartCoroutine(class [mscorlib]System.Collections.IEnumerator)
		IL_0167: stloc.s 5
		IL_0169: br IL_07d6

		IL_016e: nop
		IL_016f: ldarg.0
		IL_0170: ldfld class PlayerScript GameControllerScript::player
		IL_0175: stloc.s 6
		IL_0177: ldloc.s 6
		IL_0179: callvirt instance void PlayerScript::ActivateBoots()
		IL_017e: br.s IL_0135

		IL_0180: nop
		IL_0181: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_0186: stloc.s 7
		IL_0188: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_width()
		IL_018d: stloc.s 8
		IL_018f: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_height()
		IL_0194: stloc.s 9
		IL_0196: ldloc.s 9
		IL_0198: ldstr "rdx"
		IL_019d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01a2: conv.i4
		IL_01a3: sub
		IL_01a4: ldc.i4.1
		IL_01a5: shr
		IL_01a6: conv.r4
		IL_01a7: stloc.s 10
		IL_01a9: ldloc.s 9
		IL_01ab: ldstr "rdx"
		IL_01b0: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01b5: conv.i4
		IL_01b6: sub
		IL_01b7: ldc.i4.1
		IL_01b8: shr
		IL_01b9: stloc.s 11
		IL_01bb: ldloc.s 7
		IL_01bd: ldloca.s 12
		IL_01bf: ldloc.s 8
		IL_01c1: ldstr "rdx"
		IL_01c6: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_01cb: conv.i4
		IL_01cc: sub
		IL_01cd: ldc.i4.1
		IL_01ce: shr
		IL_01cf: conv.r4
		IL_01d0: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_01d5: ldloca.s 12
		IL_01d7: ldloc.s 10
		IL_01d9: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_01de: ldloca.s 12
		IL_01e0: ldc.r4 0.0
		IL_01e5: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_01ea: ldloc.s 12
		IL_01ec: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Ray [UnityEngine.CoreModule]UnityEngine.Camera::ScreenPointToRay(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_01f1: stloc.s 13
		IL_01f3: ldarg.0
		IL_01f4: ldfld class PlayerScript GameControllerScript::player
		IL_01f9: ldfld bool PlayerScript::jumpRope
		IL_01fe: ldc.i4.0
		IL_01ff: ceq
		IL_0201: ldc.i4.0
		IL_0202: ceq
		IL_0204: brtrue IL_02c6

		IL_0209: nop
		IL_020a: ldstr "struct_chunk(LocalExpr+0x0, 16)"
		IL_020f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0214: stloc.s 14
		IL_0216: ldstr "struct_chunk(LocalExpr+0x10, 8)"
		IL_021b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0220: stloc.s 15
		IL_0222: br.s IL_0225

		IL_0224: nop

		IL_0225: nop
		IL_0226: ldloca.s 16
		IL_0228: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Ray::m_Origin
		IL_022d: ldloc.s 14
		IL_022f: box [mscorlib]System.Int64
		IL_0234: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0239: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_023e: ldloc.s 16
		IL_0240: ldstr "&stack[0xFFFFFFFFFFFFFF68]"
		IL_0245: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_024a: conv.i8
		IL_024b: ldc.i8 64
		IL_0254: add
		IL_0255: call bool [UnityEngine.PhysicsModule]UnityEngine.Physics::Raycast(valuetype [UnityEngine.CoreModule]UnityEngine.Ray, valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit&)
		IL_025a: stloc.s 17
		IL_025c: ldloc.s 17
		IL_025e: stloc.s 18
		IL_0260: ldloc.s 17
		IL_0262: ldc.i4.0
		IL_0263: ceq
		IL_0265: brtrue IL_07dd

		IL_026a: nop
		IL_026b: ldstr "&stack[0xFFFFFFFFFFFFFF68]"
		IL_0270: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0275: conv.i8
		IL_0276: ldc.i8 64
		IL_027f: add
		IL_0280: call instance class [UnityEngine.PhysicsModule]UnityEngine.Collider [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_collider()
		IL_0285: stloc.s 19
		IL_0287: ldloc.s 19
		IL_0289: stloc.s 20
		IL_028b: ldloc.s 20
		IL_028d: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_0292: stloc.s 21
		IL_0294: ldloc.s 21
		IL_0296: ldstr "1st Prize"
		IL_029b: call bool [mscorlib]System.String::Equals(string, string)
		IL_02a0: stloc.s 22
		IL_02a2: ldloc.s 22
		IL_02a4: stloc.s 23
		IL_02a6: ldloc.s 22
		IL_02a8: ldc.i4.0
		IL_02a9: ceq
		IL_02ab: brtrue IL_07dd

		IL_02b0: nop
		IL_02b1: ldarg.0
		IL_02b2: ldfld class FirstPrizeScript GameControllerScript::firstPrizeScript
		IL_02b7: ldc.r4 15
		IL_02bc: stfld float32 FirstPrizeScript::crazyTime
		IL_02c1: br IL_07d6

		IL_02c6: nop
		IL_02c7: ldarg.0
		IL_02c8: ldfld class PlayerScript GameControllerScript::player
		IL_02cd: callvirt instance void PlayerScript::DeactivateJumpRope()
		IL_02d2: ldarg.0
		IL_02d3: ldfld class PlaytimeScript GameControllerScript::playtimeScript
		IL_02d8: stloc.s 24
		IL_02da: ldarg.0
		IL_02db: ldfld class PlaytimeScript GameControllerScript::playtimeScript
		IL_02e0: ldfld class [UnityEngine.AnimationModule]UnityEngine.Animator PlaytimeScript::animator
		IL_02e5: ldstr "disappointed"
		IL_02ea: ldc.i4.1
		IL_02eb: callvirt instance void [UnityEngine.AnimationModule]UnityEngine.Animator::SetBool(string, bool)
		IL_02f0: ldloc.s 24
		IL_02f2: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource PlaytimeScript::audioDevice
		IL_02f7: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Stop()
		IL_02fc: ldloc.s 24
		IL_02fe: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource PlaytimeScript::audioDevice
		IL_0303: ldloc.s 24
		IL_0305: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip PlaytimeScript::aud_Sad
		IL_030a: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_030f: br IL_07d6

		IL_0314: nop
		IL_0315: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_031a: stloc.s 25
		IL_031c: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_width()
		IL_0321: stloc.s 26
		IL_0323: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_height()
		IL_0328: stloc.s 27
		IL_032a: ldloc.s 26
		IL_032c: ldstr "rdx"
		IL_0331: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0336: conv.i4
		IL_0337: sub
		IL_0338: ldc.i4.1
		IL_0339: shr
		IL_033a: conv.r4
		IL_033b: stloc.s 28
		IL_033d: ldloc.s 27
		IL_033f: ldstr "rdx"
		IL_0344: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0349: conv.i4
		IL_034a: sub
		IL_034b: ldc.i4.1
		IL_034c: shr
		IL_034d: conv.r4
		IL_034e: stloc.s 10
		IL_0350: ldloc.s 25
		IL_0352: ldloca.s 29
		IL_0354: ldloc.s 28
		IL_0356: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_035b: ldloca.s 29
		IL_035d: ldloc.s 10
		IL_035f: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0364: ldloca.s 29
		IL_0366: ldc.r4 0.0
		IL_036b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0370: ldloc.s 29
		IL_0372: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Ray [UnityEngine.CoreModule]UnityEngine.Camera::ScreenPointToRay(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0377: stloc.s 30
		IL_0379: ldstr "struct_chunk(LocalExpr+0x0, 16)"
		IL_037e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0383: stloc.s 31
		IL_0385: ldstr "struct_chunk(LocalExpr+0x10, 8)"
		IL_038a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_038f: stloc.s 15
		IL_0391: br.s IL_0394

		IL_0393: nop

		IL_0394: nop
		IL_0395: ldloca.s 32
		IL_0397: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Ray::m_Origin
		IL_039c: ldloc.s 31
		IL_039e: box [mscorlib]System.Int64
		IL_03a3: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_03a8: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_03ad: ldloc.s 32
		IL_03af: ldstr "&stack[0xFFFFFFFFFFFFFF68]"
		IL_03b4: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_03b9: conv.i8
		IL_03ba: ldc.i8 -128
		IL_03c3: add
		IL_03c4: call bool [UnityEngine.PhysicsModule]UnityEngine.Physics::Raycast(valuetype [UnityEngine.CoreModule]UnityEngine.Ray, valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit&)
		IL_03c9: stloc.s 33
		IL_03cb: ldloc.s 33
		IL_03cd: stloc.s 23
		IL_03cf: ldloc.s 33
		IL_03d1: ldc.i4.0
		IL_03d2: ceq
		IL_03d4: brtrue IL_07dd

		IL_03d9: nop
		IL_03da: ldstr "&stack[0xFFFFFFFFFFFFFF68]"
		IL_03df: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_03e4: conv.i8
		IL_03e5: ldc.i8 -128
		IL_03ee: add
		IL_03ef: call instance class [UnityEngine.PhysicsModule]UnityEngine.Collider [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_collider()
		IL_03f4: stloc.s 34
		IL_03f6: ldloc.s 34
		IL_03f8: stloc.s 35
		IL_03fa: ldloc.s 35
		IL_03fc: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Component::get_tag()
		IL_0401: stloc.s 36
		IL_0403: ldloc.s 36
		IL_0405: ldstr "Door"
		IL_040a: call bool [mscorlib]System.String::Equals(string, string)
		IL_040f: stloc.s 37
		IL_0411: ldloc.s 37
		IL_0413: stloc.s 38
		IL_0415: ldarg.0
		IL_0416: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform GameControllerScript::playerTransform
		IL_041b: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0420: stloc.s 39
		IL_0422: ldstr "&stack[0xFFFFFFFFFFFFFF68]"
		IL_0427: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_042c: conv.i8
		IL_042d: ldc.i8 -128
		IL_0436: add
		IL_0437: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_043c: stloc.s 40
		IL_043e: ldloc.s 40
		IL_0440: stloc.s 41
		IL_0442: ldloc.s 41
		IL_0444: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0449: stloc.s 42
		IL_044b: ldloca.s 43
		IL_044d: ldloca.s 39
		IL_044f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0454: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0459: ldloca.s 43
		IL_045b: ldloca.s 39
		IL_045d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0462: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0467: ldloca.s 43
		IL_0469: ldloca.s 39
		IL_046b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0470: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0475: ldloc.s 43
		IL_0477: ldloca.s 44
		IL_0479: ldloca.s 42
		IL_047b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0480: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0485: ldloca.s 44
		IL_0487: ldloca.s 42
		IL_0489: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_048e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0493: ldloca.s 44
		IL_0495: ldloca.s 42
		IL_0497: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_049c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_04a1: ldloc.s 44
		IL_04a3: call float32 [UnityEngine.CoreModule]UnityEngine.Vector3::Distance(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_04a8: stloc.s 45
		IL_04aa: ldc.r4 10
		IL_04af: ldloc.s 45
		IL_04b1: clt
		IL_04b3: brfalse.s IL_04ba

		IL_04b5: ldc.i4.0
		IL_04b6: stloc.s 46
		IL_04b8: br.s IL_04bf

		IL_04ba: nop
		IL_04bb: ldloc.s 37
		IL_04bd: stloc.s 46

		IL_04bf: nop
		IL_04c0: ldloc.s 46
		IL_04c2: stloc.s 47
		IL_04c4: ldloc.s 45
		IL_04c6: stloc.s 10
		IL_04c8: ldloc.s 46
		IL_04ca: ldc.i4.0
		IL_04cb: ceq
		IL_04cd: brtrue IL_07dd

		IL_04d2: nop
		IL_04d3: ldstr "&stack[0xFFFFFFFFFFFFFF68]"
		IL_04d8: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_04dd: conv.i8
		IL_04de: ldc.i8 -128
		IL_04e7: add
		IL_04e8: call instance class [UnityEngine.PhysicsModule]UnityEngine.Collider [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_collider()
		IL_04ed: stloc.s 48
		IL_04ef: ldloc.s 48
		IL_04f1: stloc.s 49
		IL_04f3: ldloc.s 49
		IL_04f5: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_04fa: stloc.s 50
		IL_04fc: ldloc.s 50
		IL_04fe: stloc.s 51
		IL_0500: ldloc.s 51
		IL_0502: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::GetComponent<class DoorScript>()
		IL_0507: stloc.s 52
		IL_0509: ldloc.s 52
		IL_050b: stloc.s 53
		IL_050d: ldloc.s 53
		IL_050f: ldc.i4.4
		IL_0510: stfld int32 DoorScript::silentOpens
		IL_0515: ldarg.0
		IL_0516: callvirt instance void GameControllerScript::ResetItem()
		IL_051b: ldarg.0
		IL_051c: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_0521: ldarg.0
		IL_0522: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip GameControllerScript::aud_Spray
		IL_0527: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_052c: br IL_07dd

		IL_0531: nop
		IL_0532: ldarg.0
		IL_0533: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform GameControllerScript::playerTransform
		IL_0538: stloc.s 54
		IL_053a: ldarg.0
		IL_053b: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::alarmClock
		IL_0540: stloc.s 55
		IL_0542: ldloc.s 54
		IL_0544: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0549: stloc.s 56
		IL_054b: ldloc.s 55
		IL_054d: stloc.s 57
		IL_054f: ldarg.0
		IL_0550: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform GameControllerScript::cameraTransform
		IL_0555: stloc.s 54
		IL_0557: ldloc.s 54
		IL_0559: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Transform::get_rotation()
		IL_055e: stloc.s 58
		IL_0560: ldstr "struct_chunk(LocalExpr+0x0, 16)"
		IL_0565: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_056a: stloc.s 15
		IL_056c: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_0571: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0576: stloc.s 31
		IL_0578: ldloca.s 56
		IL_057a: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_057f: stloc.s 59
		IL_0581: br.s IL_0584

		IL_0583: nop

		IL_0584: nop
		IL_0585: ldloc.s 57
		IL_0587: ldloca.s 60
		IL_0589: ldloc.s 31
		IL_058b: box [mscorlib]System.Int64
		IL_0590: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0595: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_059a: ldloca.s 60
		IL_059c: ldloc.s 59
		IL_059e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_05a3: ldloc.s 60
		IL_05a5: ldloca.s 61
		IL_05a7: ldloc.s 15
		IL_05a9: box [mscorlib]System.Int64
		IL_05ae: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_05b3: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
		IL_05b8: ldloc.s 61
		IL_05ba: call !!0 [UnityEngine.CoreModule]UnityEngine.Object::Instantiate<class [UnityEngine.CoreModule]UnityEngine.GameObject>(!!0, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion)
		IL_05bf: stloc.s 62
		IL_05c1: ldloc.s 62
		IL_05c3: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::GetComponent<class AlarmClockScript>()
		IL_05c8: stloc.s 63
		IL_05ca: ldarg.0
		IL_05cb: ldfld class BaldiScript GameControllerScript::baldiScrpt
		IL_05d0: stloc.s 64
		IL_05d2: ldloc.s 63
		IL_05d4: ldloc.s 64
		IL_05d6: stfld class BaldiScript AlarmClockScript::baldi
		IL_05db: br IL_07d6

		IL_05e0: nop
		IL_05e1: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_05e6: stloc.s 65
		IL_05e8: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_width()
		IL_05ed: stloc.s 66
		IL_05ef: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_height()
		IL_05f4: stloc.s 67
		IL_05f6: ldloc.s 66
		IL_05f8: ldstr "rdx"
		IL_05fd: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0602: conv.i4
		IL_0603: sub
		IL_0604: ldc.i4.1
		IL_0605: shr
		IL_0606: conv.r4
		IL_0607: stloc.s 68
		IL_0609: ldloc.s 67
		IL_060b: ldstr "rdx"
		IL_0610: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0615: conv.i4
		IL_0616: sub
		IL_0617: ldc.i4.1
		IL_0618: shr
		IL_0619: conv.r4
		IL_061a: stloc.s 10
		IL_061c: ldloc.s 65
		IL_061e: ldloca.s 69
		IL_0620: ldloc.s 68
		IL_0622: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0627: ldloca.s 69
		IL_0629: ldloc.s 10
		IL_062b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0630: ldloca.s 69
		IL_0632: ldc.r4 0.0
		IL_0637: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_063c: ldloc.s 69
		IL_063e: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Ray [UnityEngine.CoreModule]UnityEngine.Camera::ScreenPointToRay(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0643: stloc.s 70
		IL_0645: ldstr "struct_chunk(LocalExpr+0x0, 16)"
		IL_064a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_064f: stloc.s 71
		IL_0651: ldstr "struct_chunk(LocalExpr+0x10, 8)"
		IL_0656: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_065b: stloc.s 15
		IL_065d: br.s IL_0660

		IL_065f: nop

		IL_0660: nop
		IL_0661: ldloca.s 72
		IL_0663: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Ray::m_Origin
		IL_0668: ldloc.s 71
		IL_066a: box [mscorlib]System.Int64
		IL_066f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0674: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0679: ldloc.s 72
		IL_067b: ldstr "&stack[0xFFFFFFFFFFFFFF68]"
		IL_0680: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0685: conv.i8
		IL_0686: ldc.i8 -80
		IL_068f: add
		IL_0690: call bool [UnityEngine.PhysicsModule]UnityEngine.Physics::Raycast(valuetype [UnityEngine.CoreModule]UnityEngine.Ray, valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit&)
		IL_0695: stloc.s 73
		IL_0697: ldloc.s 73
		IL_0699: stloc.s 74
		IL_069b: ldloc.s 73
		IL_069d: ldc.i4.0
		IL_069e: ceq
		IL_06a0: brtrue IL_07dd

		IL_06a5: nop
		IL_06a6: ldstr "&stack[0xFFFFFFFFFFFFFF68]"
		IL_06ab: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_06b0: conv.i8
		IL_06b1: ldc.i8 -80
		IL_06ba: add
		IL_06bb: call instance class [UnityEngine.PhysicsModule]UnityEngine.Collider [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_collider()
		IL_06c0: stloc.s 75
		IL_06c2: ldloc.s 75
		IL_06c4: stloc.s 76
		IL_06c6: ldloc.s 76
		IL_06c8: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_06cd: stloc.s 77
		IL_06cf: ldloc.s 77
		IL_06d1: ldstr "TapePlayer"
		IL_06d6: call bool [mscorlib]System.String::Equals(string, string)
		IL_06db: stloc.s 78
		IL_06dd: ldloc.s 78
		IL_06df: stloc.s 79
		IL_06e1: ldarg.0
		IL_06e2: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform GameControllerScript::playerTransform
		IL_06e7: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_06ec: stloc.s 80
		IL_06ee: ldstr "&stack[0xFFFFFFFFFFFFFF68]"
		IL_06f3: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_06f8: conv.i8
		IL_06f9: ldc.i8 -80
		IL_0702: add
		IL_0703: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_0708: stloc.s 81
		IL_070a: ldloc.s 81
		IL_070c: stloc.s 82
		IL_070e: ldloc.s 82
		IL_0710: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0715: stloc.s 83
		IL_0717: ldloca.s 84
		IL_0719: ldloca.s 80
		IL_071b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0720: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0725: ldloca.s 84
		IL_0727: ldloca.s 80
		IL_0729: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_072e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0733: ldloca.s 84
		IL_0735: ldloca.s 80
		IL_0737: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_073c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0741: ldloc.s 84
		IL_0743: ldloca.s 85
		IL_0745: ldloca.s 83
		IL_0747: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_074c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0751: ldloca.s 85
		IL_0753: ldloca.s 83
		IL_0755: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_075a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_075f: ldloca.s 85
		IL_0761: ldloca.s 83
		IL_0763: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0768: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_076d: ldloc.s 85
		IL_076f: call float32 [UnityEngine.CoreModule]UnityEngine.Vector3::Distance(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0774: stloc.s 86
		IL_0776: ldc.r4 10
		IL_077b: ldloc.s 86
		IL_077d: clt
		IL_077f: brfalse.s IL_0786

		IL_0781: ldc.i4.0
		IL_0782: stloc.s 87
		IL_0784: br.s IL_078b

		IL_0786: nop
		IL_0787: ldloc.s 78
		IL_0789: stloc.s 87

		IL_078b: nop
		IL_078c: ldloc.s 87
		IL_078e: stloc.s 88
		IL_0790: ldloc.s 86
		IL_0792: stloc.s 10
		IL_0794: ldloc.s 87
		IL_0796: ldc.i4.0
		IL_0797: ceq
		IL_0799: brtrue.s IL_07dd

		IL_079b: nop
		IL_079c: ldstr "&stack[0xFFFFFFFFFFFFFF68]"
		IL_07a1: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_07a6: conv.i8
		IL_07a7: ldc.i8 -80
		IL_07b0: add
		IL_07b1: stloc.s 89

		IL_07b3: nop
		IL_07b4: ldloc.s 89
		IL_07b6: call instance class [UnityEngine.PhysicsModule]UnityEngine.Collider [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_collider()
		IL_07bb: stloc.s 90
		IL_07bd: ldloc.s 90
		IL_07bf: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_07c4: stloc.s 91
		IL_07c6: ldloc.s 91
		IL_07c8: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::GetComponent<class TapePlayerScript>()
		IL_07cd: stloc.s 92
		IL_07cf: ldloc.s 92
		IL_07d1: callvirt instance void TapePlayerScript::Play()

		IL_07d6: nop
		IL_07d7: ldarg.0
		IL_07d8: callvirt instance void GameControllerScript::ResetItem()

		IL_07dd: nop
		IL_07de: ret

		IL_07df: nop
		IL_07e0: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_07e5: stloc.s 93
		IL_07e7: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_width()
		IL_07ec: stloc.s 94
		IL_07ee: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_height()
		IL_07f3: stloc.s 95
		IL_07f5: ldloc.s 94
		IL_07f7: ldstr "rdx"
		IL_07fc: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0801: conv.i4
		IL_0802: sub
		IL_0803: ldc.i4.1
		IL_0804: shr
		IL_0805: conv.r4
		IL_0806: stloc.s 96
		IL_0808: ldloc.s 95
		IL_080a: ldstr "rdx"
		IL_080f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0814: conv.i4
		IL_0815: sub
		IL_0816: ldc.i4.1
		IL_0817: shr
		IL_0818: conv.r4
		IL_0819: stloc.s 10
		IL_081b: ldloc.s 93
		IL_081d: ldloca.s 97
		IL_081f: ldloc.s 96
		IL_0821: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0826: ldloca.s 97
		IL_0828: ldloc.s 10
		IL_082a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_082f: ldloca.s 97
		IL_0831: ldc.r4 0.0
		IL_0836: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_083b: ldloc.s 97
		IL_083d: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Ray [UnityEngine.CoreModule]UnityEngine.Camera::ScreenPointToRay(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0842: stloc.s 98
		IL_0844: ldstr "struct_chunk(LocalExpr+0x0, 16)"
		IL_0849: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_084e: stloc.s 99
		IL_0850: ldstr "struct_chunk(LocalExpr+0x10, 8)"
		IL_0855: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_085a: stloc.s 15
		IL_085c: br.s IL_085f

		IL_085e: nop

		IL_085f: nop
		IL_0860: ldloca.s 100
		IL_0862: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Ray::m_Origin
		IL_0867: ldloc.s 99
		IL_0869: box [mscorlib]System.Int64
		IL_086e: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0873: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0878: ldloc.s 100
		IL_087a: ldloca.s 101
		IL_087c: call bool [UnityEngine.PhysicsModule]UnityEngine.Physics::Raycast(valuetype [UnityEngine.CoreModule]UnityEngine.Ray, valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit&)
		IL_0881: stloc.s 102
		IL_0883: ldloc.s 102
		IL_0885: stloc.s 103
		IL_0887: ldloc.s 102
		IL_0889: ldc.i4.0
		IL_088a: ceq
		IL_088c: brtrue IL_07dd

		IL_0891: nop
		IL_0892: ldloca.s 104
		IL_0894: call instance class [UnityEngine.PhysicsModule]UnityEngine.Collider [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_collider()
		IL_0899: stloc.s 105
		IL_089b: ldloc.s 105
		IL_089d: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_08a2: stloc.s 106
		IL_08a4: ldloc.s 106
		IL_08a6: ldstr "BSODAMachine"
		IL_08ab: call bool [mscorlib]System.String::Equals(string, string)
		IL_08b0: stloc.s 107
		IL_08b2: ldarg.0
		IL_08b3: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform GameControllerScript::playerTransform
		IL_08b8: stloc.s 54
		IL_08ba: ldloc.s 54
		IL_08bc: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_08c1: stloc.s 108
		IL_08c3: ldloca.s 109
		IL_08c5: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_08ca: stloc.s 110
		IL_08cc: ldloca.s 108
		IL_08ce: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_08d3: stloc.s 59
		IL_08d5: ldloc.s 110
		IL_08d7: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_08dc: stloc.s 111
		IL_08de: ldloca.s 112
		IL_08e0: ldloca.s 108
		IL_08e2: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_08e7: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_08ec: ldloca.s 112
		IL_08ee: ldloca.s 108
		IL_08f0: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_08f5: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_08fa: ldloca.s 112
		IL_08fc: ldloc.s 59
		IL_08fe: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0903: ldloc.s 112
		IL_0905: ldloca.s 113
		IL_0907: ldloca.s 111
		IL_0909: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_090e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0913: ldloca.s 113
		IL_0915: ldloca.s 111
		IL_0917: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_091c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0921: ldloca.s 113
		IL_0923: ldloca.s 111
		IL_0925: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_092a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_092f: ldloc.s 113
		IL_0931: call float32 [UnityEngine.CoreModule]UnityEngine.Vector3::Distance(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0936: stloc.s 114
		IL_0938: ldc.r4 10
		IL_093d: ldloc.s 114
		IL_093f: clt
		IL_0941: brfalse.s IL_0948

		IL_0943: ldc.i4.0
		IL_0944: stloc.s 115
		IL_0946: br.s IL_094d

		IL_0948: nop
		IL_0949: ldloc.s 107
		IL_094b: stloc.s 115

		IL_094d: nop
		IL_094e: ldloc.s 114
		IL_0950: stloc.s 10
		IL_0952: ldloc.s 115
		IL_0954: ldc.i4.0
		IL_0955: ceq
		IL_0957: ldc.i4.0
		IL_0958: ceq
		IL_095a: brtrue IL_0b15

		IL_095f: nop
		IL_0960: ldloca.s 116
		IL_0962: call instance class [UnityEngine.PhysicsModule]UnityEngine.Collider [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_collider()
		IL_0967: stloc.s 117
		IL_0969: ldloc.s 117
		IL_096b: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_0970: stloc.s 118
		IL_0972: ldloc.s 118
		IL_0974: ldstr "ZestyMachine"
		IL_0979: call bool [mscorlib]System.String::Equals(string, string)
		IL_097e: stloc.s 119
		IL_0980: ldarg.0
		IL_0981: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform GameControllerScript::playerTransform
		IL_0986: stloc.s 54
		IL_0988: ldloc.s 54
		IL_098a: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_098f: stloc.s 120
		IL_0991: ldloca.s 121
		IL_0993: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_0998: stloc.s 122
		IL_099a: ldloc.s 122
		IL_099c: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_09a1: stloc.s 123
		IL_09a3: ldloca.s 124
		IL_09a5: ldloca.s 120
		IL_09a7: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_09ac: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_09b1: ldloca.s 124
		IL_09b3: ldloca.s 120
		IL_09b5: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_09ba: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_09bf: ldloca.s 124
		IL_09c1: ldloca.s 120
		IL_09c3: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_09c8: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_09cd: ldloc.s 124
		IL_09cf: ldloca.s 125
		IL_09d1: ldloca.s 123
		IL_09d3: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_09d8: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_09dd: ldloca.s 125
		IL_09df: ldloca.s 123
		IL_09e1: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_09e6: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_09eb: ldloca.s 125
		IL_09ed: ldloca.s 123
		IL_09ef: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_09f4: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_09f9: ldloc.s 125
		IL_09fb: call float32 [UnityEngine.CoreModule]UnityEngine.Vector3::Distance(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0a00: stloc.s 126
		IL_0a02: ldc.r4 10
		IL_0a07: ldloc.s 126
		IL_0a09: clt
		IL_0a0b: brfalse.s IL_0a12

		IL_0a0d: ldc.i4.0
		IL_0a0e: stloc.s 127
		IL_0a10: br.s IL_0a17

		IL_0a12: nop
		IL_0a13: ldloc.s 119
		IL_0a15: stloc.s 127

		IL_0a17: nop
		IL_0a18: ldloc.s 126
		IL_0a1a: stloc.s 10
		IL_0a1c: ldloc.s 127
		IL_0a1e: ldc.i4.0
		IL_0a1f: ceq
		IL_0a21: ldc.i4.0
		IL_0a22: ceq
		IL_0a24: brtrue IL_0b02

		IL_0a29: nop
		IL_0a2a: ldloca.s 128
		IL_0a2c: call instance class [UnityEngine.PhysicsModule]UnityEngine.Collider [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_collider()
		IL_0a31: stloc.s 129
		IL_0a33: ldloc.s 129
		IL_0a35: stloc.s 130
		IL_0a37: ldloc.s 130
		IL_0a39: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Object::get_name()
		IL_0a3e: stloc.s 131
		IL_0a40: ldloc.s 131
		IL_0a42: ldstr "PayPhone"
		IL_0a47: call bool [mscorlib]System.String::Equals(string, string)
		IL_0a4c: stloc.s 132
		IL_0a4e: ldloc.s 132
		IL_0a50: stloc.s 133
		IL_0a52: ldarg.0
		IL_0a53: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform GameControllerScript::playerTransform
		IL_0a58: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0a5d: stloc.s 134
		IL_0a5f: ldloca.s 135
		IL_0a61: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_0a66: stloc.s 136
		IL_0a68: ldloc.s 136
		IL_0a6a: stloc.s 137
		IL_0a6c: ldloc.s 137
		IL_0a6e: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0a73: stloc.s 138
		IL_0a75: ldloca.s 139
		IL_0a77: ldloca.s 134
		IL_0a79: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0a7e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0a83: ldloca.s 139
		IL_0a85: ldloca.s 134
		IL_0a87: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0a8c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0a91: ldloca.s 139
		IL_0a93: ldloca.s 134
		IL_0a95: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0a9a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0a9f: ldloc.s 139
		IL_0aa1: ldloca.s 140
		IL_0aa3: ldloca.s 138
		IL_0aa5: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0aaa: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0aaf: ldloca.s 140
		IL_0ab1: ldloca.s 138
		IL_0ab3: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0ab8: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0abd: ldloca.s 140
		IL_0abf: ldloca.s 138
		IL_0ac1: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0ac6: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0acb: ldloc.s 140
		IL_0acd: call float32 [UnityEngine.CoreModule]UnityEngine.Vector3::Distance(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0ad2: stloc.s 141
		IL_0ad4: ldc.r4 10
		IL_0ad9: ldloc.s 141
		IL_0adb: clt
		IL_0add: brfalse.s IL_0ae4

		IL_0adf: ldc.i4.0
		IL_0ae0: stloc.s 142
		IL_0ae2: br.s IL_0ae9

		IL_0ae4: nop
		IL_0ae5: ldloc.s 132
		IL_0ae7: stloc.s 142

		IL_0ae9: nop
		IL_0aea: ldloc.s 142
		IL_0aec: stloc.s 143
		IL_0aee: ldloc.s 141
		IL_0af0: stloc.s 10
		IL_0af2: ldloc.s 142
		IL_0af4: ldc.i4.0
		IL_0af5: ceq
		IL_0af7: brtrue IL_07dd

		IL_0afc: nop
		IL_0afd: br IL_07b3

		IL_0b02: nop
		IL_0b03: ldarg.0
		IL_0b04: callvirt instance void GameControllerScript::ResetItem()
		IL_0b09: ldarg.0
		IL_0b0a: ldc.i4.1
		IL_0b0b: callvirt instance void GameControllerScript::CollectItem(int32)
		IL_0b10: br IL_07dd

		IL_0b15: nop
		IL_0b16: ldarg.0
		IL_0b17: callvirt instance void GameControllerScript::ResetItem()
		IL_0b1c: ldarg.0
		IL_0b1d: ldc.i4.4
		IL_0b1e: callvirt instance void GameControllerScript::CollectItem(int32)
		IL_0b23: br IL_07dd

		IL_0b28: nop
		IL_0b29: ldarg.0
		IL_0b2a: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform GameControllerScript::playerTransform
		IL_0b2f: stloc.s 54
		IL_0b31: ldarg.0
		IL_0b32: ldfld class [UnityEngine.CoreModule]UnityEngine.GameObject GameControllerScript::bsodaSpray
		IL_0b37: stloc.s 144
		IL_0b39: ldloc.s 54
		IL_0b3b: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0b40: stloc.s 145
		IL_0b42: ldloc.s 144
		IL_0b44: stloc.s 57
		IL_0b46: ldarg.0
		IL_0b47: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform GameControllerScript::cameraTransform
		IL_0b4c: stloc.s 54
		IL_0b4e: ldloc.s 54
		IL_0b50: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion [UnityEngine.CoreModule]UnityEngine.Transform::get_rotation()
		IL_0b55: stloc.s 146
		IL_0b57: ldstr "struct_chunk(LocalExpr+0x0, 16)"
		IL_0b5c: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0b61: stloc.s 15
		IL_0b63: ldstr "struct_chunk(LocalExpr+0x0, 8)"
		IL_0b68: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0b6d: stloc.s 99
		IL_0b6f: ldloca.s 145
		IL_0b71: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0b76: stloc.s 59
		IL_0b78: br.s IL_0b7b

		IL_0b7a: nop

		IL_0b7b: nop
		IL_0b7c: ldloc.s 57
		IL_0b7e: ldloca.s 147
		IL_0b80: ldloc.s 99
		IL_0b82: box [mscorlib]System.Int64
		IL_0b87: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0b8c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0b91: ldloca.s 147
		IL_0b93: ldloc.s 59
		IL_0b95: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0b9a: ldloc.s 147
		IL_0b9c: ldloca.s 148
		IL_0b9e: ldloc.s 15
		IL_0ba0: box [mscorlib]System.Int64
		IL_0ba5: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0baa: stfld float32 [UnityEngine.CoreModule]UnityEngine.Quaternion::x
		IL_0baf: ldloc.s 148
		IL_0bb1: call !!0 [UnityEngine.CoreModule]UnityEngine.Object::Instantiate<class [UnityEngine.CoreModule]UnityEngine.GameObject>(!!0, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Quaternion)
		IL_0bb6: stloc.s 149
		IL_0bb8: ldarg.0
		IL_0bb9: callvirt instance void GameControllerScript::ResetItem()
		IL_0bbe: ldarg.0
		IL_0bbf: ldfld class PlayerScript GameControllerScript::player
		IL_0bc4: ldstr "drink"
		IL_0bc9: ldc.r4 1
		IL_0bce: callvirt instance void PlayerScript::ResetGuilt(string, float32)
		IL_0bd3: ldarg.0
		IL_0bd4: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_0bd9: ldarg.0
		IL_0bda: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip GameControllerScript::aud_Soda
		IL_0bdf: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_0be4: br IL_07dd

		IL_0be9: nop
		IL_0bea: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_0bef: stloc.s 150
		IL_0bf1: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_width()
		IL_0bf6: stloc.s 151
		IL_0bf8: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_height()
		IL_0bfd: stloc.s 152
		IL_0bff: ldloc.s 151
		IL_0c01: ldstr "rdx"
		IL_0c06: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0c0b: conv.i4
		IL_0c0c: sub
		IL_0c0d: ldc.i4.1
		IL_0c0e: shr
		IL_0c0f: conv.r4
		IL_0c10: stloc.s 153
		IL_0c12: ldloc.s 152
		IL_0c14: ldstr "rdx"
		IL_0c19: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0c1e: conv.i4
		IL_0c1f: sub
		IL_0c20: ldc.i4.1
		IL_0c21: shr
		IL_0c22: conv.r4
		IL_0c23: stloc.s 10
		IL_0c25: ldloc.s 150
		IL_0c27: ldloca.s 154
		IL_0c29: ldloc.s 153
		IL_0c2b: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0c30: ldloca.s 154
		IL_0c32: ldloc.s 10
		IL_0c34: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0c39: ldloca.s 154
		IL_0c3b: ldc.r4 0.0
		IL_0c40: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0c45: ldloc.s 154
		IL_0c47: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Ray [UnityEngine.CoreModule]UnityEngine.Camera::ScreenPointToRay(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0c4c: stloc.s 155
		IL_0c4e: ldstr "struct_chunk(LocalExpr+0x0, 16)"
		IL_0c53: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0c58: stloc.s 156
		IL_0c5a: ldstr "struct_chunk(LocalExpr+0x10, 8)"
		IL_0c5f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0c64: stloc.s 15
		IL_0c66: br.s IL_0c69

		IL_0c68: nop

		IL_0c69: nop
		IL_0c6a: ldloca.s 157
		IL_0c6c: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Ray::m_Origin
		IL_0c71: ldloc.s 156
		IL_0c73: box [mscorlib]System.Int64
		IL_0c78: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0c7d: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0c82: ldloc.s 157
		IL_0c84: ldstr "&stack[0xFFFFFFFFFFFFFF68]"
		IL_0c89: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0c8e: conv.i8
		IL_0c8f: ldc.i8 -32
		IL_0c98: add
		IL_0c99: call bool [UnityEngine.PhysicsModule]UnityEngine.Physics::Raycast(valuetype [UnityEngine.CoreModule]UnityEngine.Ray, valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit&)
		IL_0c9e: stloc.s 158
		IL_0ca0: ldloc.s 158
		IL_0ca2: stloc.s 159
		IL_0ca4: ldloc.s 158
		IL_0ca6: ldc.i4.0
		IL_0ca7: ceq
		IL_0ca9: brtrue IL_07dd

		IL_0cae: nop
		IL_0caf: ldstr "&stack[0xFFFFFFFFFFFFFF68]"
		IL_0cb4: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0cb9: conv.i8
		IL_0cba: ldc.i8 -32
		IL_0cc3: add
		IL_0cc4: call instance class [UnityEngine.PhysicsModule]UnityEngine.Collider [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_collider()
		IL_0cc9: stloc.s 160
		IL_0ccb: ldloc.s 160
		IL_0ccd: stloc.s 161
		IL_0ccf: ldloc.s 161
		IL_0cd1: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Component::get_tag()
		IL_0cd6: stloc.s 162
		IL_0cd8: ldloc.s 162
		IL_0cda: ldstr "Door"
		IL_0cdf: call bool [mscorlib]System.String::Equals(string, string)
		IL_0ce4: stloc.s 163
		IL_0ce6: ldloc.s 163
		IL_0ce8: stloc.s 164
		IL_0cea: ldarg.0
		IL_0ceb: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform GameControllerScript::playerTransform
		IL_0cf0: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0cf5: stloc.s 165
		IL_0cf7: ldstr "&stack[0xFFFFFFFFFFFFFF68]"
		IL_0cfc: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0d01: conv.i8
		IL_0d02: ldc.i8 -32
		IL_0d0b: add
		IL_0d0c: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_0d11: stloc.s 166
		IL_0d13: ldloca.s 165
		IL_0d15: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0d1a: stloc.s 59
		IL_0d1c: ldloc.s 166
		IL_0d1e: stloc.s 167
		IL_0d20: ldloc.s 167
		IL_0d22: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0d27: stloc.s 168
		IL_0d29: ldloca.s 169
		IL_0d2b: ldloca.s 165
		IL_0d2d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0d32: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0d37: ldloca.s 169
		IL_0d39: ldloca.s 165
		IL_0d3b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0d40: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0d45: ldloca.s 169
		IL_0d47: ldloc.s 59
		IL_0d49: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0d4e: ldloc.s 169
		IL_0d50: ldloca.s 170
		IL_0d52: ldloca.s 168
		IL_0d54: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0d59: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0d5e: ldloca.s 170
		IL_0d60: ldloca.s 168
		IL_0d62: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0d67: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0d6c: ldloca.s 170
		IL_0d6e: ldloca.s 168
		IL_0d70: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0d75: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0d7a: ldloc.s 170
		IL_0d7c: call float32 [UnityEngine.CoreModule]UnityEngine.Vector3::Distance(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0d81: stloc.s 171
		IL_0d83: ldc.r4 10
		IL_0d88: ldloc.s 171
		IL_0d8a: clt
		IL_0d8c: brfalse.s IL_0d93

		IL_0d8e: ldc.i4.0
		IL_0d8f: stloc.s 172
		IL_0d91: br.s IL_0d98

		IL_0d93: nop
		IL_0d94: ldloc.s 163
		IL_0d96: stloc.s 172

		IL_0d98: nop
		IL_0d99: ldloc.s 171
		IL_0d9b: stloc.s 10
		IL_0d9d: ldloc.s 172
		IL_0d9f: stloc.s 173
		IL_0da1: ldloc.s 172
		IL_0da3: ldc.i4.0
		IL_0da4: ceq
		IL_0da6: brtrue IL_07dd

		IL_0dab: nop
		IL_0dac: ldstr "&stack[0xFFFFFFFFFFFFFF68]"
		IL_0db1: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0db6: conv.i8
		IL_0db7: ldc.i8 -32
		IL_0dc0: add
		IL_0dc1: call instance class [UnityEngine.PhysicsModule]UnityEngine.Collider [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_collider()
		IL_0dc6: stloc.s 174
		IL_0dc8: ldloc.s 174
		IL_0dca: stloc.s 175
		IL_0dcc: ldloc.s 175
		IL_0dce: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_0dd3: stloc.s 176
		IL_0dd5: ldloc.s 176
		IL_0dd7: stloc.s 177
		IL_0dd9: ldloc.s 177
		IL_0ddb: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::GetComponent<class DoorScript>()
		IL_0de0: stloc.s 178
		IL_0de2: ldloc.s 178
		IL_0de4: stloc.s 179
		IL_0de6: ldloc.s 179
		IL_0de8: ldfld bool DoorScript::bDoorLocked
		IL_0ded: ldc.i4.0
		IL_0dee: ceq
		IL_0df0: brtrue IL_07dd

		IL_0df5: nop
		IL_0df6: ldloc.s 179
		IL_0df8: ldc.i4.0
		IL_0df9: stfld bool DoorScript::bDoorLocked
		IL_0dfe: ldloc.s 179
		IL_0e00: callvirt instance void DoorScript::OpenDoor()
		IL_0e05: br IL_07d6

		IL_0e0a: nop
		IL_0e0b: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_0e10: stloc.s 180
		IL_0e12: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_width()
		IL_0e17: stloc.s 181
		IL_0e19: call int32 [UnityEngine.CoreModule]UnityEngine.Screen::get_height()
		IL_0e1e: stloc.s 182
		IL_0e20: ldloc.s 181
		IL_0e22: ldstr "rdx"
		IL_0e27: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0e2c: conv.i4
		IL_0e2d: sub
		IL_0e2e: ldc.i4.1
		IL_0e2f: shr
		IL_0e30: conv.r4
		IL_0e31: stloc.s 183
		IL_0e33: ldloc.s 182
		IL_0e35: ldstr "rdx"
		IL_0e3a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0e3f: conv.i4
		IL_0e40: sub
		IL_0e41: ldc.i4.1
		IL_0e42: shr
		IL_0e43: conv.r4
		IL_0e44: stloc.s 10
		IL_0e46: ldloc.s 180
		IL_0e48: ldloca.s 184
		IL_0e4a: ldloc.s 183
		IL_0e4c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0e51: ldloca.s 184
		IL_0e53: ldloc.s 10
		IL_0e55: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0e5a: ldloca.s 184
		IL_0e5c: ldc.r4 0.0
		IL_0e61: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0e66: ldloc.s 184
		IL_0e68: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Ray [UnityEngine.CoreModule]UnityEngine.Camera::ScreenPointToRay(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0e6d: stloc.s 185
		IL_0e6f: ldstr "struct_chunk(LocalExpr+0x0, 16)"
		IL_0e74: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0e79: stloc.s 186
		IL_0e7b: ldstr "struct_chunk(LocalExpr+0x10, 8)"
		IL_0e80: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0e85: stloc.s 15
		IL_0e87: br.s IL_0e8a

		IL_0e89: nop

		IL_0e8a: nop
		IL_0e8b: ldloca.s 187
		IL_0e8d: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Ray::m_Origin
		IL_0e92: ldloc.s 186
		IL_0e94: box [mscorlib]System.Int64
		IL_0e99: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0e9e: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0ea3: ldloc.s 187
		IL_0ea5: ldstr "&stack[0xFFFFFFFFFFFFFF68]"
		IL_0eaa: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0eaf: conv.i8
		IL_0eb0: ldc.i8 16
		IL_0eb9: add
		IL_0eba: call bool [UnityEngine.PhysicsModule]UnityEngine.Physics::Raycast(valuetype [UnityEngine.CoreModule]UnityEngine.Ray, valuetype [UnityEngine.PhysicsModule]UnityEngine.RaycastHit&)
		IL_0ebf: stloc.s 188
		IL_0ec1: ldloc.s 188
		IL_0ec3: stloc.s 189
		IL_0ec5: ldloc.s 188
		IL_0ec7: ldc.i4.0
		IL_0ec8: ceq
		IL_0eca: brtrue IL_07dd

		IL_0ecf: nop
		IL_0ed0: ldstr "&stack[0xFFFFFFFFFFFFFF68]"
		IL_0ed5: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0eda: conv.i8
		IL_0edb: ldc.i8 16
		IL_0ee4: add
		IL_0ee5: call instance class [UnityEngine.PhysicsModule]UnityEngine.Collider [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_collider()
		IL_0eea: stloc.s 190
		IL_0eec: ldloc.s 190
		IL_0eee: stloc.s 191
		IL_0ef0: ldloc.s 191
		IL_0ef2: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Component::get_tag()
		IL_0ef7: stloc.s 192
		IL_0ef9: ldloc.s 192
		IL_0efb: ldstr "SwingingDoor"
		IL_0f00: call bool [mscorlib]System.String::Equals(string, string)
		IL_0f05: stloc.s 193
		IL_0f07: ldloc.s 193
		IL_0f09: stloc.s 194
		IL_0f0b: ldarg.0
		IL_0f0c: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform GameControllerScript::playerTransform
		IL_0f11: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0f16: stloc.s 195
		IL_0f18: ldstr "&stack[0xFFFFFFFFFFFFFF68]"
		IL_0f1d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0f22: conv.i8
		IL_0f23: ldc.i8 16
		IL_0f2c: add
		IL_0f2d: call instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_transform()
		IL_0f32: stloc.s 196
		IL_0f34: ldloc.s 196
		IL_0f36: stloc.s 197
		IL_0f38: ldloc.s 197
		IL_0f3a: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0f3f: stloc.s 198
		IL_0f41: ldloca.s 199
		IL_0f43: ldloca.s 195
		IL_0f45: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0f4a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0f4f: ldloca.s 199
		IL_0f51: ldloca.s 195
		IL_0f53: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0f58: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0f5d: ldloca.s 199
		IL_0f5f: ldloca.s 195
		IL_0f61: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0f66: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0f6b: ldloc.s 199
		IL_0f6d: ldloca.s 200
		IL_0f6f: ldloca.s 198
		IL_0f71: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0f76: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0f7b: ldloca.s 200
		IL_0f7d: ldloca.s 198
		IL_0f7f: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0f84: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0f89: ldloca.s 200
		IL_0f8b: ldloca.s 198
		IL_0f8d: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0f92: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0f97: ldloc.s 200
		IL_0f99: call float32 [UnityEngine.CoreModule]UnityEngine.Vector3::Distance(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3, valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0f9e: stloc.s 201
		IL_0fa0: ldc.r4 10
		IL_0fa5: ldloc.s 201
		IL_0fa7: clt
		IL_0fa9: brfalse.s IL_0fb0

		IL_0fab: ldc.i4.0
		IL_0fac: stloc.s 202
		IL_0fae: br.s IL_0fb5

		IL_0fb0: nop
		IL_0fb1: ldloc.s 193
		IL_0fb3: stloc.s 202

		IL_0fb5: nop
		IL_0fb6: ldloc.s 201
		IL_0fb8: stloc.s 10
		IL_0fba: ldloc.s 202
		IL_0fbc: stloc.s 203
		IL_0fbe: ldloc.s 202
		IL_0fc0: ldc.i4.0
		IL_0fc1: ceq
		IL_0fc3: brtrue IL_07dd

		IL_0fc8: nop
		IL_0fc9: ldstr "&stack[0xFFFFFFFFFFFFFF68]"
		IL_0fce: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0fd3: conv.i8
		IL_0fd4: ldc.i8 16
		IL_0fdd: add
		IL_0fde: call instance class [UnityEngine.PhysicsModule]UnityEngine.Collider [UnityEngine.PhysicsModule]UnityEngine.RaycastHit::get_collider()
		IL_0fe3: stloc.s 204
		IL_0fe5: ldloc.s 204
		IL_0fe7: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_0fec: stloc.s 205
		IL_0fee: ldloc.s 205
		IL_0ff0: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.GameObject::GetComponent<class SwingingDoorScript>()
		IL_0ff5: stloc.s 206
		IL_0ff7: ldloc.s 206
		IL_0ff9: ldc.r4 15
		IL_0ffe: callvirt instance void SwingingDoorScript::LockDoor(float32)
		IL_1003: br IL_07d6

		IL_1008: nop
		IL_1009: ldarg.0
		IL_100a: ldfld class PlayerScript GameControllerScript::player
		IL_100f: ldfld float32 PlayerScript::maxStamina
		IL_1014: ldarg.0
		IL_1015: ldfld class PlayerScript GameControllerScript::player
		IL_101a: ldfld float32 PlayerScript::maxStamina
		IL_101f: add
		IL_1020: stloc.s 207
		IL_1022: ldarg.0
		IL_1023: ldfld class PlayerScript GameControllerScript::player
		IL_1028: ldloc.s 207
		IL_102a: stfld float32 PlayerScript::stamina
		IL_102f: ldloc.s 207
		IL_1031: stloc.s 10
		IL_1033: br IL_07d6

		IL_1038: nop
		IL_1039: ldnull
		IL_103a: throw

		IL_103b: nop
		IL_103c: ldnull
		IL_103d: throw
	} // end of method GameControllerScript::UseItem

	.method private hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator BootAnimation () cil managed 
	{
		// Method begins at RVA 0x577c
		// Header size: 12
		// Code size: 40 (0x28)
		.maxstack 2
		.locals init (
			[0] class GameControllerScript/'<BootAnimation>d__19'
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(GameControllerScript+<BootAnimation>d__19)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class GameControllerScript/'<BootAnimation>d__19'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 GameControllerScript/'<BootAnimation>d__19'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class GameControllerScript GameControllerScript/'<BootAnimation>d__19'::'<>4__this'
		IL_0026: ldloc.0
		IL_0027: ret
	} // end of method GameControllerScript::BootAnimation

	.method private hidebysig 
		instance class [mscorlib]System.Collections.IEnumerator Teleporter () cil managed 
	{
		// Method begins at RVA 0x57b0
		// Header size: 12
		// Code size: 40 (0x28)
		.maxstack 2
		.locals init (
			[0] class GameControllerScript/'<Teleporter>d__20'
		)

		IL_0000: nop
		IL_0001: ldstr "il2cpp_codegen_object_new(GameControllerScript+<Teleporter>d__20)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class GameControllerScript/'<Teleporter>d__20'>(object)
		IL_0010: stloc.0
		IL_0011: ldloc.0
		IL_0012: ldnull
		IL_0013: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0018: ldloc.0
		IL_0019: ldc.i4.0
		IL_001a: stfld int32 GameControllerScript/'<Teleporter>d__20'::'<>1__state'
		IL_001f: ldloc.0
		IL_0020: ldarg.0
		IL_0021: stfld class GameControllerScript GameControllerScript/'<Teleporter>d__20'::'<>4__this'
		IL_0026: ldloc.0
		IL_0027: ret
	} // end of method GameControllerScript::Teleporter

	.method private hidebysig 
		instance void Teleport () cil managed 
	{
		// Method begins at RVA 0x57e4
		// Header size: 12
		// Code size: 182 (0xb6)
		.maxstack 4
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[1] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[2] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class AILocationSelectorScript GameControllerScript::AILocationSelector
		IL_0007: callvirt instance void AILocationSelectorScript::GetNewTarget()
		IL_000c: ldarg.0
		IL_000d: ldfld class PlayerScript GameControllerScript::player
		IL_0012: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0017: stloc.0
		IL_0018: ldarg.0
		IL_0019: ldfld class AILocationSelectorScript GameControllerScript::AILocationSelector
		IL_001e: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_0023: stloc.1
		IL_0024: ldloc.1
		IL_0025: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_002a: stloc.2
		IL_002b: ldloc.0
		IL_002c: ldloca.s 3
		IL_002e: ldarg.0
		IL_002f: ldfld class PlayerScript GameControllerScript::player
		IL_0034: ldfld float32 PlayerScript::height
		IL_0039: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::upVector
		IL_003e: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0043: mul
		IL_0044: ldloca.s 2
		IL_0046: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_004b: add
		IL_004c: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0051: ldloca.s 3
		IL_0053: ldarg.0
		IL_0054: ldfld class PlayerScript GameControllerScript::player
		IL_0059: ldfld float32 PlayerScript::height
		IL_005e: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::upVector
		IL_0063: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0068: mul
		IL_0069: ldloca.s 2
		IL_006b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0070: add
		IL_0071: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0076: ldloca.s 3
		IL_0078: ldarg.0
		IL_0079: ldfld class PlayerScript GameControllerScript::player
		IL_007e: ldfld float32 PlayerScript::height
		IL_0083: ldsfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Vector3::upVector
		IL_0088: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_008d: mul
		IL_008e: ldloca.s 2
		IL_0090: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0095: add
		IL_0096: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_009b: ldloc.3
		IL_009c: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Transform::set_position(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_00a1: ldarg.0
		IL_00a2: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_00a7: ldarg.0
		IL_00a8: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip GameControllerScript::aud_Teleport
		IL_00ad: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_00b2: ret

		IL_00b3: nop
		IL_00b4: ldnull
		IL_00b5: throw
	} // end of method GameControllerScript::Teleport

	.method private hidebysig 
		instance void ResetItem () cil managed 
	{
		// Method begins at RVA 0x2127
		// Header size: 1
		// Code size: 63 (0x3f)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld int32[] GameControllerScript::item
		IL_0007: ldarg.0
		IL_0008: ldfld int32 GameControllerScript::itemSelected
		IL_000d: conv.i8
		IL_000e: conv.i4
		IL_000f: ldc.i4.0
		IL_0010: stelem [mscorlib]System.Int32
		IL_0015: ldarg.0
		IL_0016: ldfld class [UnityEngine.UI]UnityEngine.UI.RawImage[] GameControllerScript::itemSlot
		IL_001b: ldarg.0
		IL_001c: ldfld int32 GameControllerScript::itemSelected
		IL_0021: conv.i8
		IL_0022: conv.i4
		IL_0023: ldelem [UnityEngine.UI]UnityEngine.UI.RawImage
		IL_0028: ldarg.0
		IL_0029: ldfld class [UnityEngine.CoreModule]UnityEngine.Texture[] GameControllerScript::itemTextures
		IL_002e: ldc.i4.0
		IL_002f: ldelem [UnityEngine.CoreModule]UnityEngine.Texture
		IL_0034: callvirt instance void [UnityEngine.UI]UnityEngine.UI.RawImage::set_texture(class [UnityEngine.CoreModule]UnityEngine.Texture)
		IL_0039: nop
		IL_003a: ldnull
		IL_003b: throw

		IL_003c: nop
		IL_003d: ldnull
		IL_003e: throw
	} // end of method GameControllerScript::ResetItem

	.method public hidebysig 
		instance void LoseItem (
			int32 id
		) cil managed 
	{
		// Method begins at RVA 0x2167
		// Header size: 1
		// Code size: 53 (0x35)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld int32[] GameControllerScript::item
		IL_0007: ldarg.1
		IL_0008: conv.i8
		IL_0009: conv.i4
		IL_000a: ldc.i4.0
		IL_000b: stelem [mscorlib]System.Int32
		IL_0010: ldarg.0
		IL_0011: ldfld class [UnityEngine.UI]UnityEngine.UI.RawImage[] GameControllerScript::itemSlot
		IL_0016: ldarg.1
		IL_0017: conv.i8
		IL_0018: conv.i4
		IL_0019: ldelem [UnityEngine.UI]UnityEngine.UI.RawImage
		IL_001e: ldarg.0
		IL_001f: ldfld class [UnityEngine.CoreModule]UnityEngine.Texture[] GameControllerScript::itemTextures
		IL_0024: ldc.i4.0
		IL_0025: ldelem [UnityEngine.CoreModule]UnityEngine.Texture
		IL_002a: callvirt instance void [UnityEngine.UI]UnityEngine.UI.RawImage::set_texture(class [UnityEngine.CoreModule]UnityEngine.Texture)
		IL_002f: nop
		IL_0030: ldnull
		IL_0031: throw

		IL_0032: nop
		IL_0033: ldnull
		IL_0034: throw
	} // end of method GameControllerScript::LoseItem

	.method private hidebysig 
		instance void UpdateItemName () cil managed 
	{
		// Method begins at RVA 0x219d
		// Header size: 1
		// Code size: 7 (0x7)
		.maxstack 8

		IL_0000: nop
		IL_0001: nop
		IL_0002: ldnull
		IL_0003: throw

		IL_0004: nop
		IL_0005: ldnull
		IL_0006: throw
	} // end of method GameControllerScript::UpdateItemName

	.method public hidebysig 
		instance void ExitReached () cil managed 
	{
		// Method begins at RVA 0x58a8
		// Header size: 12
		// Code size: 278 (0x116)
		.maxstack 3
		.locals init (
			[0] int32,
			[1] int64,
			[2] int64,
			[3] int32,
			[4] valuetype [UnityEngine.CoreModule]UnityEngine.Color
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld int32 GameControllerScript::exitsReached
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld int32 GameControllerScript::exitsReached
		IL_000e: conv.i8
		IL_000f: ldc.i8 1
		IL_0018: add
		IL_0019: stloc.1
		IL_001a: ldarg.0
		IL_001b: ldloc.1
		IL_001c: conv.i4
		IL_001d: stfld int32 GameControllerScript::exitsReached
		IL_0022: ldloc.1
		IL_0023: stloc.2
		IL_0024: ldloc.0
		IL_0025: stloc.3
		IL_0026: ldloc.0
		IL_0027: ldc.i4.0
		IL_0028: ceq
		IL_002a: ldc.i4.0
		IL_002b: ceq
		IL_002d: brtrue.s IL_0090

		IL_002f: nop
		IL_0030: ldloca.s 4
		IL_0032: ldstr "global_13EAC10"
		IL_0037: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_003c: box [mscorlib]System.Int64
		IL_0041: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0046: stfld float32 [UnityEngine.CoreModule]UnityEngine.Color::r
		IL_004b: ldloc.s 4
		IL_004d: call void [UnityEngine.CoreModule]UnityEngine.RenderSettings::set_ambientLight(valuetype [UnityEngine.CoreModule]UnityEngine.Color)
		IL_0052: ldarg.0
		IL_0053: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_0058: ldarg.0
		IL_0059: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip GameControllerScript::aud_Switch
		IL_005e: ldc.r4 0.8
		IL_0063: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip, float32)
		IL_0068: ldarg.0
		IL_0069: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_006e: ldarg.0
		IL_006f: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip GameControllerScript::aud_MachineQuiet
		IL_0074: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_clip(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_0079: ldarg.0
		IL_007a: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_007f: ldc.i4.1
		IL_0080: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_loop(bool)
		IL_0085: ldarg.0
		IL_0086: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_008b: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Play()

		IL_0090: nop
		IL_0091: ldarg.0
		IL_0092: ldfld int32 GameControllerScript::exitsReached
		IL_0097: ldc.i4.2
		IL_0098: ceq
		IL_009a: ldc.i4.0
		IL_009b: ceq
		IL_009d: brtrue.s IL_00d8

		IL_009f: nop
		IL_00a0: ldarg.0
		IL_00a1: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_00a6: ldc.r4 0.8
		IL_00ab: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_volume(float32)
		IL_00b0: ldarg.0
		IL_00b1: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_00b6: ldarg.0
		IL_00b7: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip GameControllerScript::aud_MachineStart
		IL_00bc: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_clip(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_00c1: ldarg.0
		IL_00c2: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_00c7: ldc.i4.1
		IL_00c8: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_loop(bool)
		IL_00cd: ldarg.0
		IL_00ce: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_00d3: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Play()

		IL_00d8: nop
		IL_00d9: ldarg.0
		IL_00da: ldfld int32 GameControllerScript::exitsReached
		IL_00df: ldc.i4.3
		IL_00e0: ceq
		IL_00e2: ldc.i4.0
		IL_00e3: ceq
		IL_00e5: brtrue.s IL_0111

		IL_00e7: nop
		IL_00e8: ldarg.0
		IL_00e9: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_00ee: ldarg.0
		IL_00ef: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip GameControllerScript::aud_MachineRev
		IL_00f4: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_clip(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_00f9: ldarg.0
		IL_00fa: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_00ff: ldc.i4.0
		IL_0100: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_loop(bool)
		IL_0105: ldarg.0
		IL_0106: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_010b: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Play()
		IL_0110: ret

		IL_0111: nop
		IL_0112: ret

		IL_0113: nop
		IL_0114: ldnull
		IL_0115: throw
	} // end of method GameControllerScript::ExitReached

	.method public hidebysig 
		instance void Fliparoo () cil managed 
	{
		// Method begins at RVA 0x59cc
		// Header size: 12
		// Code size: 124 (0x7c)
		.maxstack 2
		.locals init (
			[0] class PlayerScript,
			[1] class [UnityEngine.CoreModule]UnityEngine.Camera,
			[2] class CameraScript
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class PlayerScript GameControllerScript::player
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldc.i4.1
		IL_000a: stfld bool GameControllerScript::flipped
		IL_000f: ldloc.0
		IL_0010: ldc.r4 6
		IL_0015: stfld float32 PlayerScript::height
		IL_001a: ldarg.0
		IL_001b: ldfld class PlayerScript GameControllerScript::player
		IL_0020: ldc.r4 180
		IL_0025: stfld float32 PlayerScript::fliparoo
		IL_002a: ldarg.0
		IL_002b: ldfld class PlayerScript GameControllerScript::player
		IL_0030: ldc.r4 -1
		IL_0035: stfld float32 PlayerScript::flipaturn
		IL_003a: call class [UnityEngine.CoreModule]UnityEngine.Camera [UnityEngine.CoreModule]UnityEngine.Camera::get_main()
		IL_003f: stloc.1
		IL_0040: ldloc.1
		IL_0041: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class CameraScript>()
		IL_0046: stloc.2
		IL_0047: ldloc.2
		IL_0048: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::offset
		IL_004d: ldc.r4 -1
		IL_0052: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0057: ldloc.2
		IL_0058: ldc.i4.0
		IL_0059: box [mscorlib]System.Int32
		IL_005e: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.Vector3>(object)
		IL_0063: stfld valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::offset
		IL_0068: ldloc.2
		IL_0069: ldflda valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 CameraScript::offset
		IL_006e: ldc.r4 0.0
		IL_0073: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0078: ret

		IL_0079: nop
		IL_007a: ldnull
		IL_007b: throw
	} // end of method GameControllerScript::Fliparoo

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x5a54
		// Header size: 12
		// Code size: 219 (0xdb)
		.maxstack 3
		.locals init (
			[0] int32[],
			[1] class [UnityEngine.UI]UnityEngine.UI.RawImage[],
			[2] string[],
			[3] class [UnityEngine.CoreModule]UnityEngine.Texture[]
		)

		IL_0000: nop
		IL_0001: ldc.i4.3
		IL_0002: newarr [mscorlib]System.Int32
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld int32[] GameControllerScript::item
		IL_000f: ldc.i4.3
		IL_0010: newarr [UnityEngine.UI]UnityEngine.UI.RawImage
		IL_0015: stloc.1
		IL_0016: ldarg.0
		IL_0017: ldloc.1
		IL_0018: stfld class [UnityEngine.UI]UnityEngine.UI.RawImage[] GameControllerScript::itemSlot
		IL_001d: ldc.i4.s 12
		IL_001f: newarr [mscorlib]System.String
		IL_0024: stloc.2
		IL_0025: ldloc.2
		IL_0026: ldc.i4.0
		IL_0027: ldstr "Nothing"
		IL_002c: stelem [mscorlib]System.String
		IL_0031: ldloc.2
		IL_0032: ldc.i4.1
		IL_0033: ldstr "Energy flavored Zesty Bar"
		IL_0038: stelem [mscorlib]System.String
		IL_003d: ldloc.2
		IL_003e: ldc.i4.2
		IL_003f: ldstr "Yellow Door Lock"
		IL_0044: stelem [mscorlib]System.String
		IL_0049: ldloc.2
		IL_004a: ldc.i4.3
		IL_004b: ldstr "Principal's Keys"
		IL_0050: stelem [mscorlib]System.String
		IL_0055: ldloc.2
		IL_0056: ldc.i4.4
		IL_0057: ldstr "BSODA"
		IL_005c: stelem [mscorlib]System.String
		IL_0061: ldloc.2
		IL_0062: ldc.i4.5
		IL_0063: ldstr "Quarter"
		IL_0068: stelem [mscorlib]System.String
		IL_006d: ldloc.2
		IL_006e: ldc.i4.6
		IL_006f: ldstr "Baldi Anti Hearing and Disorienting Tape"
		IL_0074: stelem [mscorlib]System.String
		IL_0079: ldloc.2
		IL_007a: ldc.i4.7
		IL_007b: ldstr "Alarm Clock"
		IL_0080: stelem [mscorlib]System.String
		IL_0085: ldloc.2
		IL_0086: ldc.i4.8
		IL_0087: ldstr "WD-NoSquee (Door Type)"
		IL_008c: stelem [mscorlib]System.String
		IL_0091: ldloc.2
		IL_0092: ldc.i4.s 9
		IL_0094: ldstr "Safety Scissors"
		IL_0099: stelem [mscorlib]System.String
		IL_009e: ldloc.2
		IL_009f: ldc.i4.s 10
		IL_00a1: ldstr "Big Ol' Boots"
		IL_00a6: stelem [mscorlib]System.String
		IL_00ab: ldloc.2
		IL_00ac: ldc.i4.s 11
		IL_00ae: ldstr "Teleportation Teleporter"
		IL_00b3: stelem [mscorlib]System.String
		IL_00b8: ldarg.0
		IL_00b9: ldloc.2
		IL_00ba: stfld string[] GameControllerScript::itemNames
		IL_00bf: ldc.i4.s 10
		IL_00c1: newarr [UnityEngine.CoreModule]UnityEngine.Texture
		IL_00c6: stloc.3
		IL_00c7: ldarg.0
		IL_00c8: ldloc.3
		IL_00c9: stfld class [UnityEngine.CoreModule]UnityEngine.Texture[] GameControllerScript::itemTextures
		IL_00ce: ldarg.0
		IL_00cf: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_00d4: ret

		IL_00d5: nop
		IL_00d6: ldnull
		IL_00d7: throw

		IL_00d8: nop
		IL_00d9: ldnull
		IL_00da: throw
	} // end of method GameControllerScript::.ctor

} // end of class GameControllerScript
