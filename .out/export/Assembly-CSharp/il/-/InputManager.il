.class public auto ansi beforefieldinit InputManager
	extends class Singleton`1<class InputManager>
{
	// Fields
	.field public class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> KeyboardMapping
	.field private class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode, valuetype SimulatedKeyState> simulatedKeys
	.field private bool[] currentKeyStates
	.field private bool[] previousKeyStates
	.field private static literal string SavePrefix

	// Methods
	.method public hidebysig specialname 
		instance class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> get_Mappings () cil managed 
	{
		// Method begins at RVA 0x9994
		// Header size: 12
		// Code size: 49 (0x31)
		.maxstack 2
		.locals init (
			[0] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
		IL_0007: ldnull
		IL_0008: ceq
		IL_000a: brtrue.s IL_0022

		IL_000c: nop
		IL_000d: ldarg.0
		IL_000e: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
		IL_0013: callvirt instance int32 class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::get_Count()
		IL_0018: stloc.0
		IL_0019: ldloc.0
		IL_001a: ldc.i4.0
		IL_001b: ceq
		IL_001d: ldc.i4.0
		IL_001e: ceq
		IL_0020: brtrue.s IL_0029

		IL_0022: nop
		IL_0023: ldarg.0
		IL_0024: callvirt instance void InputManager::Load()

		IL_0029: nop
		IL_002a: ldarg.0
		IL_002b: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
		IL_0030: ret
	} // end of method InputManager::get_Mappings

	.method family hidebysig virtual 
		instance void Awake () cil managed 
	{
		// Method begins at RVA 0x99d4
		// Header size: 12
		// Code size: 121 (0x79)
		.maxstack 2
		.locals init (
			[0] class InputManager,
			[1] class InputManager,
			[2] bool,
			[3] class InputManager,
			[4] bool,
			[5] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[6] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[7] class [UnityEngine.CoreModule]UnityEngine.GameObject,
			[8] class [UnityEngine.CoreModule]UnityEngine.GameObject
		)

		IL_0000: nop
		IL_0001: call !0 class Singleton`1<class InputManager>::get_Instance()
		IL_0006: stloc.0
		IL_0007: ldloc.0
		IL_0008: stloc.1
		IL_0009: br.s IL_000c

		IL_000b: nop

		IL_000c: nop
		IL_000d: ldloc.1
		IL_000e: ldnull
		IL_000f: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Inequality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0014: stloc.2
		IL_0015: ldloc.2
		IL_0016: ldc.i4.0
		IL_0017: ceq
		IL_0019: brtrue.s IL_0051

		IL_001b: nop
		IL_001c: call !0 class Singleton`1<class InputManager>::get_Instance()
		IL_0021: stloc.3
		IL_0022: ldloc.3
		IL_0023: stloc.1
		IL_0024: br.s IL_0027

		IL_0026: nop

		IL_0027: nop
		IL_0028: ldloc.1
		IL_0029: ldarg.0
		IL_002a: call bool [UnityEngine.CoreModule]UnityEngine.Object::op_Inequality(class [UnityEngine.CoreModule]UnityEngine.Object, class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_002f: stloc.s 4
		IL_0031: ldloc.s 4
		IL_0033: ldc.i4.0
		IL_0034: ceq
		IL_0036: brtrue.s IL_0051

		IL_0038: nop
		IL_0039: ldarg.0
		IL_003a: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_003f: stloc.s 5
		IL_0041: ldloc.s 5
		IL_0043: stloc.s 6
		IL_0045: br.s IL_0048

		IL_0047: nop

		IL_0048: nop
		IL_0049: ldloc.s 6
		IL_004b: call void [UnityEngine.CoreModule]UnityEngine.Object::Destroy(class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0050: ret

		IL_0051: nop
		IL_0052: ldloc.s 6
		IL_0054: callvirt instance void class Singleton`1<class InputManager>::Awake()
		IL_0059: ldloc.s 6
		IL_005b: callvirt instance class [UnityEngine.CoreModule]UnityEngine.GameObject [UnityEngine.CoreModule]UnityEngine.Component::get_gameObject()
		IL_0060: stloc.s 7
		IL_0062: ldloc.s 7
		IL_0064: stloc.s 8
		IL_0066: br.s IL_0069

		IL_0068: nop

		IL_0069: nop
		IL_006a: ldloc.s 8
		IL_006c: call void [UnityEngine.CoreModule]UnityEngine.Object::DontDestroyOnLoad(class [UnityEngine.CoreModule]UnityEngine.Object)
		IL_0071: ldloc.s 6
		IL_0073: callvirt instance void InputManager::Load()
		IL_0078: ret
	} // end of method InputManager::Awake

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0x9a5c
		// Header size: 12
		// Code size: 252 (0xfc)
		.maxstack 3
		.locals init (
			[0] bool[],
			[1] bool[],
			[2] bool,
			[3] bool,
			[4] int64,
			[5] bool,
			[6] int32,
			[7] bool,
			[8] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld bool[] InputManager::currentKeyStates
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldfld bool[] InputManager::previousKeyStates
		IL_000e: stloc.1
		IL_000f: ldarg.0
		IL_0010: ldloc.0
		IL_0011: stfld bool[] InputManager::previousKeyStates
		IL_0016: ldarg.0
		IL_0017: ldloc.1
		IL_0018: stfld bool[] InputManager::currentKeyStates
		IL_001d: ldarg.0
		IL_001e: callvirt instance void InputManager::UpdateSimulatedKeys()
		// loop start (head: IL_0023)
			IL_0023: nop
			IL_0024: ldarg.0
			IL_0025: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
			IL_002a: ldc.i4.0
			IL_002b: box [mscorlib]System.Int32
			IL_0030: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
			IL_0035: ldstr "&stack[0x8]"
			IL_003a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_003f: callvirt instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::TryGetValue(!0, !1&)
			IL_0044: stloc.2
			IL_0045: ldloc.2
			IL_0046: stloc.3
			IL_0047: ldloc.2
			IL_0048: ldc.i4.0
			IL_0049: ceq
			IL_004b: brtrue IL_00e2

			IL_0050: nop
			IL_0051: ldstr "stack[0x8]"
			IL_0056: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_005b: stloc.s 4
			IL_005d: ldstr "stack[0x8]"
			IL_0062: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0067: conv.i4
			IL_0068: ldstr "stack[0x8]"
			IL_006d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0072: conv.i4
			IL_0073: and
			IL_0074: ldc.i4.0
			IL_0075: ceq
			IL_0077: brtrue.s IL_0098

			IL_0079: nop
			IL_007a: ldarg.0
			IL_007b: ldloc.s 4
			IL_007d: box [mscorlib]System.Int64
			IL_0082: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
			IL_0087: callvirt instance bool InputManager::GetKeyState(valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode)
			IL_008c: stloc.s 5
			IL_008e: ldloc.s 5
			IL_0090: ldc.i4.0
			IL_0091: ceq
			IL_0093: brtrue.s IL_0098

			IL_0095: nop
			IL_0096: br.s IL_00d4

			IL_0098: nop
			IL_0099: ldstr "stack[0xC]"
			IL_009e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_00a3: conv.i4
			IL_00a4: stloc.s 6
			IL_00a6: ldstr "stack[0xC]"
			IL_00ab: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_00b0: conv.i4
			IL_00b1: ldc.i4.0
			IL_00b2: ceq
			IL_00b4: ldc.i4.0
			IL_00b5: ceq
			IL_00b7: brtrue.s IL_00bc

			IL_00b9: nop
			IL_00ba: br.s IL_00d4

			IL_00bc: nop
			IL_00bd: ldarg.0
			IL_00be: ldloc.s 6
			IL_00c0: box [mscorlib]System.Int32
			IL_00c5: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
			IL_00ca: callvirt instance bool InputManager::GetKeyState(valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode)
			IL_00cf: stloc.s 7
			IL_00d1: ldloc.s 7
			IL_00d3: stloc.3

			IL_00d4: nop
			IL_00d5: ldarg.0
			IL_00d6: ldfld bool[] InputManager::currentKeyStates
			IL_00db: ldc.i4.0
			IL_00dc: ldloc.3
			IL_00dd: stelem [mscorlib]System.Boolean

			IL_00e2: nop
			IL_00e3: ldc.i4.0
			IL_00e4: ldc.i4.1
			IL_00e5: add
			IL_00e6: stloc.s 8
			IL_00e8: ldc.i4.0
			IL_00e9: ldc.i4.1
			IL_00ea: add
			IL_00eb: ldc.i4.s 13
			IL_00ed: clt
			IL_00ef: brtrue IL_0023
		// end loop
		IL_00f4: nop
		IL_00f5: ret

		IL_00f6: nop
		IL_00f7: ldnull
		IL_00f8: throw

		IL_00f9: nop
		IL_00fa: ldnull
		IL_00fb: throw
	} // end of method InputManager::Update

	.method private hidebysig 
		instance void UpdateSimulatedKeys () cil managed 
	{
		// Method begins at RVA 0x9b64
		// Header size: 12
		// Code size: 408 (0x198)
		.maxstack 3
		.locals init (
			[0] int32,
			[1] int32,
			[2] class [mscorlib]System.Collections.Generic.ICollection`1<!0>,
			[3] class [mscorlib]System.Collections.Generic.List`1<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>,
			[4] class [mscorlib]System.Collections.Generic.List`1<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>,
			[5] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<!0>,
			[6] int64,
			[7] int64,
			[8] valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator,
			[9] bool,
			[10] valuetype SimulatedKeyState,
			[11] int32,
			[12] bool,
			[13] int64
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode, valuetype SimulatedKeyState> InputManager::simulatedKeys
		IL_0007: callvirt instance int32 class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode, valuetype SimulatedKeyState>::get_Count()
		IL_000c: stloc.0
		IL_000d: ldloc.0
		IL_000e: stloc.1
		IL_000f: ldloc.0
		IL_0010: ldc.i4.0
		IL_0011: ceq
		IL_0013: brtrue IL_0159

		IL_0018: nop
		IL_0019: ldarg.0
		IL_001a: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode, valuetype SimulatedKeyState> InputManager::simulatedKeys
		IL_001f: callvirt instance class [mscorlib]System.Collections.Generic.ICollection`1<!0> class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype [mscorlib]System.Collections.Generic.KeyValuePair`2<object, object>, object>::'System.Collections.Generic.IDictionary<TKey,TValue>.get_Keys'()
		IL_0024: stloc.2
		IL_0025: ldloc.2
		IL_0026: newobj instance void class [mscorlib]System.Collections.Generic.List`1<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>::.ctor(class [mscorlib]System.Collections.Generic.IEnumerable`1<!0>)
		IL_002b: stloc.3
		IL_002c: ldloc.3
		IL_002d: stloc.s 4
		IL_002f: ldstr "&stack[0xFFFFFFFFFFFFFFA0]"
		IL_0034: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0039: box [mscorlib]System.IntPtr
		IL_003e: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [mscorlib]System.Collections.Generic.List`1>(object)
		IL_0043: callvirt instance valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<!0> class [mscorlib]System.Collections.Generic.List`1<valuetype [UnityEngine.CoreModule]UnityEngine.Color32>::GetEnumerator()
		IL_0048: stloc.s 5
		IL_004a: ldstr "stack[0xFFFFFFFFFFFFFFA0]"
		IL_004f: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0054: stloc.s 6
		IL_0056: ldstr "stack[0xFFFFFFFFFFFFFFB0]"
		IL_005b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0060: stloc.s 7
		// loop start (head: IL_0062)
			IL_0062: nop
			IL_0063: ldloca.s 8
			IL_0065: call instance bool valuetype [mscorlib]System.Collections.Generic.List`1/Enumerator<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>::MoveNext()
			IL_006a: stloc.s 9
			IL_006c: ldloc.s 9
			IL_006e: ldc.i4.0
			IL_006f: ceq
			IL_0071: brtrue IL_00fb

			IL_0076: nop
			IL_0077: ldarg.0
			IL_0078: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode, valuetype SimulatedKeyState> InputManager::simulatedKeys
			IL_007d: ldstr "stack[0xFFFFFFFFFFFFFFC8]"
			IL_0082: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0087: conv.i4
			IL_0088: box [mscorlib]System.Int32
			IL_008d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
			IL_0092: callvirt instance !1 class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode, valuetype SimulatedKeyState>::get_Item(!0)
			IL_0097: stloc.s 10
			IL_0099: ldstr "stack[0xFFFFFFFFFFFFFFC8]"
			IL_009e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_00a3: conv.i4
			IL_00a4: stloc.s 11
			IL_00a6: ldarg.0
			IL_00a7: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode, valuetype SimulatedKeyState> InputManager::simulatedKeys
			IL_00ac: ldloc.s 11
			IL_00ae: box [mscorlib]System.Int32
			IL_00b3: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
			IL_00b8: ldc.i4.0
			IL_00b9: box [mscorlib]System.Int32
			IL_00be: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype SimulatedKeyState>(object)
			IL_00c3: callvirt instance void class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode, valuetype SimulatedKeyState>::set_Item(!0, !1)
			IL_00c8: ldloc.s 10
			IL_00ca: box [mscorlib]System.Object
			IL_00cf: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
			IL_00d4: ldc.i4.0
			IL_00d5: ceq
			IL_00d7: ldc.i4.0
			IL_00d8: ceq
			IL_00da: brtrue.s IL_0062

			IL_00dc: nop
			IL_00dd: ldarg.0
			IL_00de: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode, valuetype SimulatedKeyState> InputManager::simulatedKeys
			IL_00e3: ldloc.s 11
			IL_00e5: box [mscorlib]System.Int32
			IL_00ea: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
			IL_00ef: callvirt instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode, valuetype SimulatedKeyState>::Remove(!0)
			IL_00f4: stloc.s 12
			IL_00f6: br IL_0062
		// end loop

		IL_00fb: nop
		IL_00fc: ldstr "&stack[0xFFFFFFFFFFFFFFB8]"
		IL_0101: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0106: box [mscorlib]System.IntPtr
		IL_010b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [mscorlib]Mono.Xml.SecurityParser>(object)
		IL_0110: ldstr "System.Collections.Generic.List<KeyCode>::Dispose_RuntimeMethod"
		IL_0115: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_011a: box [mscorlib]System.IntPtr
		IL_011f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [mscorlib]Mono.Xml.SmallXmlParser>(object)
		IL_0124: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0129: br.s IL_0159

		IL_012b: nop
		IL_012c: ldstr "stack[0xFFFFFFFFFFFFFFA8]"
		IL_0131: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0136: box [mscorlib]System.Int64
		IL_013b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [mscorlib]Mono.Xml.SecurityParser>(object)
		IL_0140: ldstr "System.Collections.Generic.List<KeyCode>::Dispose_RuntimeMethod"
		IL_0145: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_014a: box [mscorlib]System.IntPtr
		IL_014f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [mscorlib]Mono.Xml.SmallXmlParser>(object)
		IL_0154: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)

		IL_0159: nop
		IL_015a: ret

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
		IL_0165: ldstr "sub_167180"
		IL_016a: ldc.i8 0
		IL_0173: ldc.i8 0
		IL_017c: ldc.i8 0
		IL_0185: ldc.i8 0
		IL_018e: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0193: stloc.s 13
		IL_0195: nop
		IL_0196: ldnull
		IL_0197: throw
	} // end of method InputManager::UpdateSimulatedKeys

	.method public hidebysig 
		instance void SimulateKey (
			valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode key
		) cil managed 
	{
		// Method begins at RVA 0x9d08
		// Header size: 12
		// Code size: 94 (0x5e)
		.maxstack 3
		.locals init (
			[0] int32,
			[1] bool
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: box [mscorlib]System.Object
		IL_0007: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_000c: stloc.0
		IL_000d: ldarg.0
		IL_000e: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode, valuetype SimulatedKeyState> InputManager::simulatedKeys
		IL_0013: ldloc.0
		IL_0014: box [mscorlib]System.Int32
		IL_0019: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
		IL_001e: ldstr "&stack[0x10]"
		IL_0023: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0028: callvirt instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode, valuetype SimulatedKeyState>::TryGetValue(!0, !1&)
		IL_002d: stloc.1
		IL_002e: ldloc.1
		IL_002f: ldc.i4.0
		IL_0030: ceq
		IL_0032: ldc.i4.0
		IL_0033: ceq
		IL_0035: brtrue.s IL_0038

		IL_0037: nop

		IL_0038: nop
		IL_0039: ldarg.0
		IL_003a: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode, valuetype SimulatedKeyState> InputManager::simulatedKeys
		IL_003f: ldloc.0
		IL_0040: box [mscorlib]System.Int32
		IL_0045: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
		IL_004a: ldc.i4.1
		IL_004b: box [mscorlib]System.Int32
		IL_0050: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype SimulatedKeyState>(object)
		IL_0055: callvirt instance void class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode, valuetype SimulatedKeyState>::set_Item(!0, !1)
		IL_005a: ret

		IL_005b: nop
		IL_005c: ldnull
		IL_005d: throw
	} // end of method InputManager::SimulateKey

	.method private hidebysig 
		instance bool GetKeyState (
			valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode key
		) cil managed 
	{
		// Method begins at RVA 0x9d74
		// Header size: 12
		// Code size: 95 (0x5f)
		.maxstack 3
		.locals init (
			[0] int32,
			[1] bool,
			[2] bool
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: box [mscorlib]System.Object
		IL_0007: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_000c: stloc.0
		IL_000d: ldarg.0
		IL_000e: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode, valuetype SimulatedKeyState> InputManager::simulatedKeys
		IL_0013: ldloc.0
		IL_0014: box [mscorlib]System.Int32
		IL_0019: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
		IL_001e: ldstr "&stack[0x10]"
		IL_0023: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0028: callvirt instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode, valuetype SimulatedKeyState>::TryGetValue(!0, !1&)
		IL_002d: stloc.1
		IL_002e: ldloc.1
		IL_002f: ldc.i4.0
		IL_0030: ceq
		IL_0032: brtrue.s IL_0048

		IL_0034: nop
		IL_0035: ldstr "stack[0x10]"
		IL_003a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_003f: ldc.i4.0
		IL_0040: conv.i8
		IL_0041: ceq
		IL_0043: brtrue.s IL_0048

		IL_0045: nop
		IL_0046: ldc.i4.1
		IL_0047: ret

		IL_0048: nop
		IL_0049: ldloc.0
		IL_004a: box [mscorlib]System.Int32
		IL_004f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
		IL_0054: call bool [UnityEngine.InputLegacyModule]UnityEngine.Input::GetKeyInt(valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode)
		IL_0059: stloc.2
		IL_005a: ldloc.2
		IL_005b: ret

		IL_005c: nop
		IL_005d: ldnull
		IL_005e: throw
	} // end of method InputManager::GetKeyState

	.method public hidebysig 
		instance bool GetActionKey (
			valuetype InputAction action
		) cil managed 
	{
		// Method begins at RVA 0x2302
		// Header size: 1
		// Code size: 25 (0x19)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldstr "*(ConvExpr + 0x20)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: ldc.i4.0
		IL_000c: conv.i8
		IL_000d: ceq
		IL_000f: ldc.i4.0
		IL_0010: ceq
		IL_0012: ret

		IL_0013: nop
		IL_0014: ldnull
		IL_0015: throw

		IL_0016: nop
		IL_0017: ldnull
		IL_0018: throw
	} // end of method InputManager::GetActionKey

	.method public hidebysig 
		instance bool GetActionKeyDown (
			valuetype InputAction action
		) cil managed 
	{
		// Method begins at RVA 0x231c
		// Header size: 1
		// Code size: 45 (0x2d)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldstr "*(ConvExpr + 0x20)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: ldc.i4.0
		IL_000c: conv.i8
		IL_000d: ceq
		IL_000f: ldc.i4.0
		IL_0010: ceq
		IL_0012: brtrue.s IL_0017

		IL_0014: nop
		IL_0015: ldc.i4.0
		IL_0016: ret

		IL_0017: nop
		IL_0018: ldstr "*(ConvExpr + 0x20)"
		IL_001d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0022: ldc.i4.0
		IL_0023: conv.i8
		IL_0024: ceq
		IL_0026: ret

		IL_0027: nop
		IL_0028: ldnull
		IL_0029: throw

		IL_002a: nop
		IL_002b: ldnull
		IL_002c: throw
	} // end of method InputManager::GetActionKeyDown

	.method public hidebysig 
		instance bool GetActionKeyUp (
			valuetype InputAction action
		) cil managed 
	{
		// Method begins at RVA 0x234a
		// Header size: 1
		// Code size: 45 (0x2d)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldstr "*(ConvExpr + 0x20)"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: ldc.i4.0
		IL_000c: conv.i8
		IL_000d: ceq
		IL_000f: brtrue.s IL_0014

		IL_0011: nop
		IL_0012: ldc.i4.0
		IL_0013: ret

		IL_0014: nop
		IL_0015: ldstr "*(ConvExpr + 0x20)"
		IL_001a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_001f: ldc.i4.0
		IL_0020: conv.i8
		IL_0021: ceq
		IL_0023: ldc.i4.0
		IL_0024: ceq
		IL_0026: ret

		IL_0027: nop
		IL_0028: ldnull
		IL_0029: throw

		IL_002a: nop
		IL_002b: ldnull
		IL_002c: throw
	} // end of method InputManager::GetActionKeyUp

	.method public hidebysig 
		instance void ClearPrimaryKey (
			valuetype InputAction action
		) cil managed 
	{
		// Method begins at RVA 0x9de0
		// Header size: 12
		// Code size: 93 (0x5d)
		.maxstack 3
		.locals init (
			[0] int32,
			[1] bool,
			[2] bool
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: box [mscorlib]System.Object
		IL_0007: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_000c: stloc.0
		IL_000d: ldarg.0
		IL_000e: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
		IL_0013: ldloc.0
		IL_0014: box [mscorlib]System.Int32
		IL_0019: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
		IL_001e: ldstr "&stack[0x8]"
		IL_0023: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0028: callvirt instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::TryGetValue(!0, !1&)
		IL_002d: stloc.1
		IL_002e: ldloc.1
		IL_002f: stloc.2
		IL_0030: ldloc.1
		IL_0031: ldc.i4.0
		IL_0032: ceq
		IL_0034: brtrue.s IL_0058

		IL_0036: nop
		IL_0037: ldarg.0
		IL_0038: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
		IL_003d: ldloc.0
		IL_003e: box [mscorlib]System.Int32
		IL_0043: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
		IL_0048: ldc.i4.0
		IL_0049: box [mscorlib]System.Int32
		IL_004e: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputBinding>(object)
		IL_0053: callvirt instance void class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::set_Item(!0, !1)

		IL_0058: nop
		IL_0059: ret

		IL_005a: nop
		IL_005b: ldnull
		IL_005c: throw
	} // end of method InputManager::ClearPrimaryKey

	.method public hidebysig 
		instance void ClearSecondaryKey (
			valuetype InputAction action
		) cil managed 
	{
		// Method begins at RVA 0x9e4c
		// Header size: 12
		// Code size: 102 (0x66)
		.maxstack 3
		.locals init (
			[0] int32,
			[1] bool,
			[2] bool
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: box [mscorlib]System.Object
		IL_0007: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_000c: stloc.0
		IL_000d: ldarg.0
		IL_000e: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
		IL_0013: ldloc.0
		IL_0014: box [mscorlib]System.Int32
		IL_0019: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
		IL_001e: ldstr "&stack[0x8]"
		IL_0023: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0028: callvirt instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::TryGetValue(!0, !1&)
		IL_002d: stloc.1
		IL_002e: ldloc.1
		IL_002f: stloc.2
		IL_0030: ldloc.1
		IL_0031: ldc.i4.0
		IL_0032: ceq
		IL_0034: brtrue.s IL_0061

		IL_0036: nop
		IL_0037: ldarg.0
		IL_0038: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
		IL_003d: ldloc.0
		IL_003e: box [mscorlib]System.Int32
		IL_0043: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
		IL_0048: ldstr "stack[0x8]"
		IL_004d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0052: box [mscorlib]System.Int64
		IL_0057: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputBinding>(object)
		IL_005c: callvirt instance void class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::set_Item(!0, !1)

		IL_0061: nop
		IL_0062: ret

		IL_0063: nop
		IL_0064: ldnull
		IL_0065: throw
	} // end of method InputManager::ClearSecondaryKey

	.method public hidebysig 
		instance string ConvertInputActionToString (
			valuetype InputAction action
		) cil managed 
	{
		// Method begins at RVA 0x9ec0
		// Header size: 12
		// Code size: 181 (0xb5)
		.maxstack 3
		.locals init (
			[0] int32,
			[1] bool,
			[2] int64,
			[3] int32,
			[4] string,
			[5] string
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: box [mscorlib]System.Object
		IL_0007: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_000c: stloc.0
		IL_000d: ldarg.0
		IL_000e: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
		IL_0013: ldloc.0
		IL_0014: box [mscorlib]System.Int32
		IL_0019: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
		IL_001e: ldstr "&stack[0x8]"
		IL_0023: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0028: callvirt instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::TryGetValue(!0, !1&)
		IL_002d: stloc.1
		IL_002e: ldloc.1
		IL_002f: ldc.i4.0
		IL_0030: ceq
		IL_0032: brtrue.s IL_0093

		IL_0034: nop
		IL_0035: ldstr "stack[0x8]"
		IL_003a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_003f: stloc.2
		IL_0040: ldstr "stack[0x8]"
		IL_0045: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_004a: conv.i4
		IL_004b: ldstr "stack[0x8]"
		IL_0050: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0055: conv.i4
		IL_0056: and
		IL_0057: ldc.i4.0
		IL_0058: ceq
		IL_005a: ldc.i4.0
		IL_005b: ceq
		IL_005d: brtrue.s IL_007c

		IL_005f: nop
		IL_0060: ldstr "stack[0xC]"
		IL_0065: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_006a: conv.i4
		IL_006b: stloc.3
		IL_006c: ldstr "stack[0xC]"
		IL_0071: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0076: conv.i4
		IL_0077: ldc.i4.0
		IL_0078: ceq
		IL_007a: brtrue.s IL_0093

		IL_007c: nop
		IL_007d: ldarg.0
		IL_007e: ldloc.3
		IL_007f: box [mscorlib]System.Int32
		IL_0084: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode>(object)
		IL_0089: callvirt instance string InputManager::KeyCodeToDisplayString(valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode)
		IL_008e: stloc.s 4
		IL_0090: ldloc.s 4
		IL_0092: ret

		IL_0093: nop
		IL_0094: ldstr "&stack[0x20]"
		IL_0099: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_009e: box [mscorlib]System.IntPtr
		IL_00a3: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [mscorlib]System.Enum>(object)
		IL_00a8: callvirt instance string [mscorlib]System.Enum::ToString()
		IL_00ad: stloc.s 5
		IL_00af: ldloc.s 5
		IL_00b1: ret

		IL_00b2: nop
		IL_00b3: ldnull
		IL_00b4: throw
	} // end of method InputManager::ConvertInputActionToString

	.method public hidebysig 
		instance void Modify (
			valuetype InputAction action,
			valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode newKey,
			[opt] valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode secondaryKey
		) cil managed 
	{
		// Method begins at RVA 0x9f84
		// Header size: 12
		// Code size: 177 (0xb1)
		.maxstack 3
		.locals init (
			[0] int32,
			[1] int32,
			[2] int32,
			[3] bool,
			[4] int32,
			[5] int32
		)

		IL_0000: nop
		IL_0001: ldarg.2
		IL_0002: box [mscorlib]System.Object
		IL_0007: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_000c: stloc.0
		IL_000d: ldarg.3
		IL_000e: box [mscorlib]System.Object
		IL_0013: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0018: stloc.1
		IL_0019: ldarg.1
		IL_001a: box [mscorlib]System.Object
		IL_001f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0024: stloc.2
		IL_0025: ldarg.0
		IL_0026: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
		IL_002b: ldloc.2
		IL_002c: box [mscorlib]System.Int32
		IL_0031: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
		IL_0036: ldstr "&stack[0x8]"
		IL_003b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0040: callvirt instance bool class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::TryGetValue(!0, !1&)
		IL_0045: stloc.3
		IL_0046: ldloc.3
		IL_0047: ldc.i4.0
		IL_0048: ceq
		IL_004a: ldc.i4.0
		IL_004b: ceq
		IL_004d: brtrue.s IL_0050

		IL_004f: nop

		IL_0050: nop
		IL_0051: ldloc.0
		IL_0052: ldc.i4.0
		IL_0053: ceq
		IL_0055: ldc.i4.0
		IL_0056: ceq
		IL_0058: brfalse.s IL_005f

		IL_005a: ldloc.0
		IL_005b: stloc.s 4
		IL_005d: br.s IL_006d

		IL_005f: nop
		IL_0060: ldstr "stack[0x8]"
		IL_0065: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_006a: conv.i4
		IL_006b: stloc.s 4

		IL_006d: nop
		IL_006e: ldloc.1
		IL_006f: ldc.i4.0
		IL_0070: ceq
		IL_0072: ldc.i4.0
		IL_0073: ceq
		IL_0075: brfalse.s IL_007c

		IL_0077: ldloc.1
		IL_0078: stloc.s 5
		IL_007a: br.s IL_008a

		IL_007c: nop
		IL_007d: ldstr "stack[0xC]"
		IL_0082: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0087: conv.i4
		IL_0088: stloc.s 5

		IL_008a: nop
		IL_008b: ldarg.0
		IL_008c: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
		IL_0091: ldloc.2
		IL_0092: box [mscorlib]System.Int32
		IL_0097: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
		IL_009c: ldloc.s 4
		IL_009e: box [mscorlib]System.Int32
		IL_00a3: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputBinding>(object)
		IL_00a8: callvirt instance void class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::set_Item(!0, !1)
		IL_00ad: ret

		IL_00ae: nop
		IL_00af: ldnull
		IL_00b0: throw
	} // end of method InputManager::Modify

	.method public hidebysig 
		instance void Save () cil managed 
	{
		// Method begins at RVA 0xa044
		// Header size: 12
		// Code size: 305 (0x131)
		.maxstack 3
		.locals init (
			[0] valuetype [mscorlib]System.Collections.Generic.Dictionary`2/Enumerator<!0, !1>,
			[1] int64,
			[2] int64,
			[3] valuetype [mscorlib]System.Collections.Generic.Dictionary`2/Enumerator,
			[4] bool,
			[5] string,
			[6] string,
			[7] object,
			[8] object,
			[9] string,
			[10] int64
		)

		IL_0000: nop
		IL_0001: ldstr "&stack[0xFFFFFFFFFFFFFFB0]"
		IL_0006: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_000b: box [mscorlib]System.IntPtr
		IL_0010: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [mscorlib]System.Collections.Generic.Dictionary`2>(object)
		IL_0015: callvirt instance valuetype [mscorlib]System.Collections.Generic.Dictionary`2/Enumerator<!0, !1> class [mscorlib]System.Collections.Generic.Dictionary`2<int32, valuetype [UnityEngine.UIElementsModule]UnityEngine.UIElements.DynamicHeightVirtualizationController`1/ContentHeightCacheInfo<object>>::GetEnumerator()
		IL_001a: stloc.0
		IL_001b: ldstr "stack[0xFFFFFFFFFFFFFFB0]"
		IL_0020: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0025: stloc.1
		IL_0026: ldstr "stack[0xFFFFFFFFFFFFFFC0]"
		IL_002b: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0030: stloc.2
		// loop start (head: IL_0031)
			IL_0031: nop
			IL_0032: ldloca.s 3
			IL_0034: call instance bool valuetype [mscorlib]System.Collections.Generic.Dictionary`2/Enumerator<valuetype InputAction, valuetype InputBinding>::MoveNext()
			IL_0039: stloc.s 4
			IL_003b: ldloc.s 4
			IL_003d: ldc.i4.0
			IL_003e: ceq
			IL_0040: brtrue.s IL_00ba

			IL_0042: nop
			IL_0043: ldstr "&stack[0xFFFFFFFFFFFFFFB0]"
			IL_0048: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_004d: box [mscorlib]System.IntPtr
			IL_0052: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [mscorlib]System.Enum>(object)
			IL_0057: callvirt instance string [mscorlib]System.Enum::ToString()
			IL_005c: stloc.s 5
			IL_005e: ldstr "InputBinding_"
			IL_0063: ldloc.s 5
			IL_0065: call string [mscorlib]System.String::Concat(string, string)
			IL_006a: stloc.s 6
			IL_006c: ldstr "stack[0xFFFFFFFFFFFFFFE4]"
			IL_0071: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0076: conv.i4
			IL_0077: box [mscorlib]System.Int32
			IL_007c: stloc.s 7
			IL_007e: ldstr "stack[0xFFFFFFFFFFFFFFE4]"
			IL_0083: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0088: conv.i4
			IL_0089: conv.i8
			IL_008a: ldc.i8 32
			IL_0093: shr.un
			IL_0094: conv.i4
			IL_0095: box [mscorlib]System.Int32
			IL_009a: stloc.s 8
			IL_009c: ldstr "{0},{1}"
			IL_00a1: ldloc.s 7
			IL_00a3: ldloc.s 8
			IL_00a5: call string [mscorlib]System.String::Format(string, object, object)
			IL_00aa: stloc.s 9
			IL_00ac: ldloc.s 6
			IL_00ae: ldloc.s 9
			IL_00b0: call void [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::SetString(string, string)
			IL_00b5: br IL_0031
		// end loop

		IL_00ba: nop
		IL_00bb: ldstr "&stack[0xFFFFFFFFFFFFFFD0]"
		IL_00c0: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00c5: box [mscorlib]System.IntPtr
		IL_00ca: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [mscorlib]Mono.Xml.SecurityParser>(object)
		IL_00cf: ldstr "System.Collections.Generic.Dictionary<InputAction, InputBinding>::Dispose_RuntimeMethod"
		IL_00d4: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00d9: box [mscorlib]System.IntPtr
		IL_00de: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [mscorlib]Mono.Xml.SmallXmlParser>(object)
		IL_00e3: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_00e8: br.s IL_0124

		IL_00ea: nop
		IL_00eb: ldstr "stack[0xFFFFFFFFFFFFFFA8]"
		IL_00f0: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00f5: box [mscorlib]System.Int64
		IL_00fa: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [mscorlib]Mono.Xml.SecurityParser>(object)
		IL_00ff: ldstr "System.Collections.Generic.Dictionary<InputAction, InputBinding>::Dispose_RuntimeMethod"
		IL_0104: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0109: box [mscorlib]System.IntPtr
		IL_010e: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [mscorlib]Mono.Xml.SmallXmlParser>(object)
		IL_0113: callvirt instance void [mscorlib]Mono.Xml.SecurityParser::OnStartParsing(class [mscorlib]Mono.Xml.SmallXmlParser)
		IL_0118: ldstr "stack[0xFFFFFFFFFFFFFFA0]"
		IL_011d: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0122: stloc.s 10

		IL_0124: nop
		IL_0125: call void [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::Save()
		IL_012a: ret

		IL_012b: nop
		IL_012c: ldnull
		IL_012d: throw

		IL_012e: nop
		IL_012f: ldnull
		IL_0130: throw
	} // end of method InputManager::Save

	.method public hidebysig 
		instance void Load () cil managed 
	{
		// Method begins at RVA 0xa184
		// Header size: 12
		// Code size: 296 (0x128)
		.maxstack 3
		.locals init (
			[0] string,
			[1] string,
			[2] bool,
			[3] string,
			[4] string,
			[5] string,
			[6] string[],
			[7] string[],
			[8] string[],
			[9] bool,
			[10] bool,
			[11] class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>,
			[12] int32,
			[13] class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>,
			[14] valuetype InputBinding,
			[15] valuetype InputBinding,
			[16] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
		IL_0007: callvirt instance void class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::Clear()
		// loop start (head: IL_000c)
			IL_000c: nop
			IL_000d: ldstr "&stack[0xFFFFFFFFFFFFFFC8]"
			IL_0012: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_0017: box [mscorlib]System.IntPtr
			IL_001c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [mscorlib]System.Enum>(object)
			IL_0021: callvirt instance string [mscorlib]System.Enum::ToString()
			IL_0026: stloc.0
			IL_0027: ldstr "InputBinding_"
			IL_002c: ldloc.0
			IL_002d: call string [mscorlib]System.String::Concat(string, string)
			IL_0032: stloc.1
			IL_0033: ldloc.1
			IL_0034: call bool [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::HasKey(string)
			IL_0039: stloc.2
			IL_003a: ldloc.1
			IL_003b: stloc.3
			IL_003c: ldloc.2
			IL_003d: ldc.i4.0
			IL_003e: ceq
			IL_0040: brtrue IL_00d6

			IL_0045: nop
			IL_0046: ldloc.3
			IL_0047: call string [UnityEngine.CoreModule]UnityEngine.PlayerPrefs::GetString(string)
			IL_004c: stloc.s 4
			IL_004e: ldloc.s 4
			IL_0050: stloc.s 5
			IL_0052: ldloc.s 5
			IL_0054: ldc.i4.s 44
			IL_0056: ldc.i4.0
			IL_0057: box [mscorlib]System.Int32
			IL_005c: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype [mscorlib]System.StringSplitOptions>(object)
			IL_0061: callvirt instance string[] [mscorlib]System.String::Split(char, valuetype [mscorlib]System.StringSplitOptions)
			IL_0066: stloc.s 6
			IL_0068: ldloc.s 6
			IL_006a: stloc.s 7
			IL_006c: ldloc.s 6
			IL_006e: stloc.s 8
			IL_0070: ldloc.s 7
			IL_0072: ldlen
			IL_0073: conv.i4
			IL_0074: ldc.i4.2
			IL_0075: ceq
			IL_0077: ldc.i4.0
			IL_0078: ceq
			IL_007a: brtrue.s IL_00d6

			IL_007c: nop
			IL_007d: ldloc.s 7
			IL_007f: ldc.i4.0
			IL_0080: ldelem [mscorlib]System.String
			IL_0085: ldstr "&stack[0x8]"
			IL_008a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_008f: call bool [mscorlib]System.Int32::TryParse(string, int32&)
			IL_0094: stloc.s 9
			IL_0096: ldloc.s 9
			IL_0098: ldc.i4.0
			IL_0099: ceq
			IL_009b: brtrue.s IL_00d6

			IL_009d: nop
			IL_009e: ldloc.s 8
			IL_00a0: ldc.i4.1
			IL_00a1: ldelem [mscorlib]System.String
			IL_00a6: ldstr "&stack[0x18]"
			IL_00ab: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_00b0: call bool [mscorlib]System.Int32::TryParse(string, int32&)
			IL_00b5: stloc.s 10
			IL_00b7: ldloc.s 10
			IL_00b9: ldc.i4.0
			IL_00ba: ceq
			IL_00bc: brtrue.s IL_00d6

			IL_00be: nop
			IL_00bf: ldarg.0
			IL_00c0: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
			IL_00c5: stloc.s 11
			IL_00c7: ldstr "stack[0x8]"
			IL_00cc: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
			IL_00d1: conv.i4
			IL_00d2: stloc.s 12
			IL_00d4: br.s IL_00fa

			IL_00d6: nop
			IL_00d7: ldarg.0
			IL_00d8: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
			IL_00dd: stloc.s 13
			IL_00df: ldarg.0
			IL_00e0: ldc.i4.0
			IL_00e1: box [mscorlib]System.Int32
			IL_00e6: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
			IL_00eb: callvirt instance valuetype InputBinding InputManager::GetDefaultBinding(valuetype InputAction)
			IL_00f0: stloc.s 14
			IL_00f2: ldloc.s 13
			IL_00f4: stloc.s 11
			IL_00f6: ldloc.s 14
			IL_00f8: stloc.s 15

			IL_00fa: nop
			IL_00fb: ldloc.s 11
			IL_00fd: ldc.i4.0
			IL_00fe: box [mscorlib]System.Int32
			IL_0103: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
			IL_0108: ldloc.s 15
			IL_010a: callvirt instance void class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::set_Item(!0, !1)
			IL_010f: ldc.i4.0
			IL_0110: ldc.i4.1
			IL_0111: add
			IL_0112: stloc.s 16
			IL_0114: ldc.i4.0
			IL_0115: ldc.i4.1
			IL_0116: add
			IL_0117: ldc.i4.s 13
			IL_0119: clt
			IL_011b: brtrue IL_000c
		// end loop
		IL_0120: nop
		IL_0121: ret

		IL_0122: nop
		IL_0123: ldnull
		IL_0124: throw

		IL_0125: nop
		IL_0126: ldnull
		IL_0127: throw
	} // end of method InputManager::Load

	.method public hidebysig 
		instance void SetDefaults () cil managed 
	{
		// Method begins at RVA 0xa2b8
		// Header size: 12
		// Code size: 80 (0x50)
		.maxstack 3
		.locals init (
			[0] class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>,
			[1] valuetype InputBinding,
			[2] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
		IL_0007: callvirt instance void class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::Clear()
		// loop start (head: IL_000c)
			IL_000c: nop
			IL_000d: ldarg.0
			IL_000e: ldfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
			IL_0013: stloc.0
			IL_0014: ldarg.0
			IL_0015: ldc.i4.0
			IL_0016: box [mscorlib]System.Int32
			IL_001b: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
			IL_0020: callvirt instance valuetype InputBinding InputManager::GetDefaultBinding(valuetype InputAction)
			IL_0025: stloc.1
			IL_0026: ldloc.0
			IL_0027: ldc.i4.0
			IL_0028: box [mscorlib]System.Int32
			IL_002d: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputAction>(object)
			IL_0032: ldloc.1
			IL_0033: callvirt instance void class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::set_Item(!0, !1)
			IL_0038: ldc.i4.0
			IL_0039: ldc.i4.1
			IL_003a: add
			IL_003b: stloc.2
			IL_003c: ldc.i4.0
			IL_003d: ldc.i4.1
			IL_003e: add
			IL_003f: ldc.i4.s 13
			IL_0041: clt
			IL_0043: brtrue.s IL_000c
		// end loop
		IL_0045: nop
		IL_0046: ldarg.0
		IL_0047: callvirt instance void InputManager::Save()
		IL_004c: ret

		IL_004d: nop
		IL_004e: ldnull
		IL_004f: throw
	} // end of method InputManager::SetDefaults

	.method public hidebysig 
		instance string KeyCodeToDisplayString (
			valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode key
		) cil managed 
	{
		// Method begins at RVA 0xa314
		// Header size: 12
		// Code size: 296 (0x128)
		.maxstack 3
		.locals init (
			[0] int32,
			[1] int64,
			[2] int32,
			[3] string
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: box [mscorlib]System.Object
		IL_0007: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_000c: stloc.0
		IL_000d: ldarg.1
		IL_000e: box [mscorlib]System.Object
		IL_0013: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_0018: ldc.i4.s 27
		IL_001a: cgt
		IL_001c: ldc.i4.0
		IL_001d: ceq
		IL_001f: brtrue IL_00db

		IL_0024: nop
		IL_0025: ldloc.0
		IL_0026: conv.i8
		IL_0027: ldc.i8 -48
		IL_0030: add
		IL_0031: conv.i4
		IL_0032: ldc.i4.s 9
		IL_0034: cgt.un
		IL_0036: brtrue.s IL_007f

		IL_0038: nop
		IL_0039: nop
		IL_003a: ldstr "0"
		IL_003f: ret

		IL_0040: nop
		IL_0041: ldstr "1"
		IL_0046: ret

		IL_0047: nop
		IL_0048: ldstr "2"
		IL_004d: ret

		IL_004e: nop
		IL_004f: ldstr "3"
		IL_0054: ret

		IL_0055: nop
		IL_0056: ldstr "4"
		IL_005b: ret

		IL_005c: nop
		IL_005d: ldstr "5"
		IL_0062: ret

		IL_0063: nop
		IL_0064: ldstr "6"
		IL_0069: ret

		IL_006a: nop
		IL_006b: ldstr "7"
		IL_0070: ret

		IL_0071: nop
		IL_0072: ldstr "8"
		IL_0077: ret

		IL_0078: nop
		IL_0079: ldstr "9"
		IL_007e: ret

		IL_007f: nop
		IL_0080: ldloc.0
		IL_0081: conv.i8
		IL_0082: ldc.i8 -323
		IL_008b: add
		IL_008c: stloc.1
		IL_008d: ldloc.0
		IL_008e: conv.i8
		IL_008f: ldc.i8 -323
		IL_0098: add
		IL_0099: conv.i4
		IL_009a: ldloc.0
		IL_009b: conv.i8
		IL_009c: ldc.i8 -323
		IL_00a5: add
		IL_00a6: conv.i4
		IL_00a7: and
		IL_00a8: ldc.i4.0
		IL_00a9: ceq
		IL_00ab: brtrue.s IL_00d4

		IL_00ad: nop
		IL_00ae: ldloc.1
		IL_00af: conv.i4
		IL_00b0: ldc.i4.1
		IL_00b1: sub
		IL_00b2: stloc.2
		IL_00b3: ldloc.1
		IL_00b4: conv.i4
		IL_00b5: ldc.i4.1
		IL_00b6: sub
		IL_00b7: ldc.i4.0
		IL_00b8: ceq
		IL_00ba: brtrue.s IL_00cd

		IL_00bc: nop
		IL_00bd: ldloc.2
		IL_00be: ldc.i4.1
		IL_00bf: ceq
		IL_00c1: ldc.i4.0
		IL_00c2: ceq
		IL_00c4: brtrue.s IL_00fe

		IL_00c6: nop
		IL_00c7: ldstr "Middle Mouse Button"
		IL_00cc: ret

		IL_00cd: nop
		IL_00ce: ldstr "Right Mouse Button"
		IL_00d3: ret

		IL_00d4: nop
		IL_00d5: ldstr "Left Mouse Button"
		IL_00da: ret

		IL_00db: nop
		IL_00dc: ldloc.0
		IL_00dd: ldc.i4.0
		IL_00de: ceq
		IL_00e0: ldc.i4.0
		IL_00e1: ceq
		IL_00e3: brtrue.s IL_00ec

		IL_00e5: nop
		IL_00e6: ldstr ""
		IL_00eb: ret

		IL_00ec: nop
		IL_00ed: ldloc.0
		IL_00ee: ldc.i4.s 27
		IL_00f0: ceq
		IL_00f2: ldc.i4.0
		IL_00f3: ceq
		IL_00f5: brtrue.s IL_00fe

		IL_00f7: nop
		IL_00f8: ldstr "ESC"
		IL_00fd: ret

		IL_00fe: nop
		IL_00ff: ldstr "&stack[0x410]"
		IL_0104: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0109: box [mscorlib]System.IntPtr
		IL_010e: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<class [mscorlib]System.Enum>(object)
		IL_0113: callvirt instance string [mscorlib]System.Enum::ToString()
		IL_0118: stloc.3
		IL_0119: ldloc.3
		IL_011a: ret

		IL_011b: nop
		IL_011c: ldstr "add [rbp-6DFFDB69h],al ; value = BinaryExpr"
		IL_0121: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0126: ldnull
		IL_0127: ret
	} // end of method InputManager::KeyCodeToDisplayString

	.method private hidebysig 
		instance valuetype InputBinding GetDefaultBinding (
			valuetype InputAction action
		) cil managed 
	{
		// Method begins at RVA 0xa448
		// Header size: 12
		// Code size: 294 (0x126)
		.maxstack 2
		.locals init (
			[0] valuetype InputBinding
		)

		IL_0000: nop
		IL_0001: ldarg.1
		IL_0002: box [mscorlib]System.Object
		IL_0007: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<int32>(object)
		IL_000c: ldc.i4.s 12
		IL_000e: cgt.un
		IL_0010: brtrue IL_0102

		IL_0015: nop
		IL_0016: nop
		IL_0017: ldc.i8 97
		IL_0020: box [mscorlib]System.Int64
		IL_0025: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputBinding>(object)
		IL_002a: ret

		IL_002b: nop
		IL_002c: ldc.i8 100
		IL_0035: box [mscorlib]System.Int64
		IL_003a: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputBinding>(object)
		IL_003f: ret

		IL_0040: nop
		IL_0041: ldc.i8 119
		IL_004a: box [mscorlib]System.Int64
		IL_004f: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputBinding>(object)
		IL_0054: ret

		IL_0055: nop
		IL_0056: ldc.i8 115
		IL_005f: box [mscorlib]System.Int64
		IL_0064: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputBinding>(object)
		IL_0069: ret

		IL_006a: nop
		IL_006b: ldc.i4.0
		IL_006c: box [mscorlib]System.Int32
		IL_0071: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputBinding>(object)
		IL_0076: ret

		IL_0077: nop
		IL_0078: ldc.i4.0
		IL_0079: box [mscorlib]System.Int32
		IL_007e: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputBinding>(object)
		IL_0083: ret

		IL_0084: nop
		IL_0085: ldc.i8 49
		IL_008e: box [mscorlib]System.Int64
		IL_0093: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputBinding>(object)
		IL_0098: ret

		IL_0099: nop
		IL_009a: ldc.i8 50
		IL_00a3: box [mscorlib]System.Int64
		IL_00a8: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputBinding>(object)
		IL_00ad: ret

		IL_00ae: nop
		IL_00af: ldc.i8 51
		IL_00b8: box [mscorlib]System.Int64
		IL_00bd: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputBinding>(object)
		IL_00c2: ret

		IL_00c3: nop
		IL_00c4: ldc.i8 304
		IL_00cd: box [mscorlib]System.Int64
		IL_00d2: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputBinding>(object)
		IL_00d7: ret

		IL_00d8: nop
		IL_00d9: ldc.i8 32
		IL_00e2: box [mscorlib]System.Int64
		IL_00e7: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputBinding>(object)
		IL_00ec: ret

		IL_00ed: nop
		IL_00ee: ldc.i8 27
		IL_00f7: box [mscorlib]System.Int64
		IL_00fc: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputBinding>(object)
		IL_0101: ret

		IL_0102: nop
		IL_0103: ldc.i4.0
		IL_0104: box [mscorlib]System.Int32
		IL_0109: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<valuetype InputBinding>(object)
		IL_010e: ret

		IL_010f: nop
		IL_0110: ldstr "add [rbx],al ; value = BinaryExpr"
		IL_0115: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_011a: ldstr "xor [rbp-6AC0FFDCh],dl ; value = BinaryExpr"
		IL_011f: call void Il2CppStudio.Native.NativeIntrinsics::Stmt(string)
		IL_0124: ldloc.0
		IL_0125: ret
	} // end of method InputManager::GetDefaultBinding

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0xa57c
		// Header size: 12
		// Code size: 64 (0x40)
		.maxstack 2
		.locals init (
			[0] class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>,
			[1] class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode, valuetype SimulatedKeyState>,
			[2] bool[],
			[3] bool[]
		)

		IL_0000: nop
		IL_0001: newobj instance void class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding>::.ctor()
		IL_0006: stloc.0
		IL_0007: ldarg.0
		IL_0008: ldloc.0
		IL_0009: stfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::KeyboardMapping
		IL_000e: newobj instance void class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode, valuetype SimulatedKeyState>::.ctor()
		IL_0013: stloc.1
		IL_0014: ldarg.0
		IL_0015: ldloc.1
		IL_0016: stfld class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype [UnityEngine.CoreModule]UnityEngine.KeyCode, valuetype SimulatedKeyState> InputManager::simulatedKeys
		IL_001b: ldc.i4.s 13
		IL_001d: newarr [mscorlib]System.Boolean
		IL_0022: stloc.2
		IL_0023: ldarg.0
		IL_0024: ldloc.2
		IL_0025: stfld bool[] InputManager::currentKeyStates
		IL_002a: ldc.i4.s 13
		IL_002c: newarr [mscorlib]System.Boolean
		IL_0031: stloc.3
		IL_0032: ldarg.0
		IL_0033: ldloc.3
		IL_0034: stfld bool[] InputManager::previousKeyStates
		IL_0039: ldarg.0
		IL_003a: callvirt instance void [Unity.TextMeshPro]TMPro.TMP_Dropdown/DropdownItem::.ctor()
		IL_003f: ret
	} // end of method InputManager::.ctor

	// Properties
	.property instance class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> Mappings()
	{
		.get instance class [mscorlib]System.Collections.Generic.Dictionary`2<valuetype InputAction, valuetype InputBinding> InputManager::get_Mappings()
	}

} // end of class InputManager
