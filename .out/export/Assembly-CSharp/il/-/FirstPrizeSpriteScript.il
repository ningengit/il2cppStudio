.class public auto ansi beforefieldinit FirstPrizeSpriteScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public float32 debug
	.field public int32 angle
	.field public float32 angleF
	.field private class [UnityEngine.CoreModule]UnityEngine.SpriteRenderer sprite
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform cam
	.field public class [UnityEngine.CoreModule]UnityEngine.Transform body
	.field public class [UnityEngine.CoreModule]UnityEngine.Sprite[] sprites

	// Methods
	.method private hidebysig 
		instance void Start () cil managed 
	{
		// Method begins at RVA 0xdcd0
		// Header size: 12
		// Code size: 16 (0x10)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.SpriteRenderer
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance !!0 [UnityEngine.CoreModule]UnityEngine.Component::GetComponent<class [UnityEngine.CoreModule]UnityEngine.SpriteRenderer>()
		IL_0007: stloc.0
		IL_0008: ldarg.0
		IL_0009: ldloc.0
		IL_000a: stfld class [UnityEngine.CoreModule]UnityEngine.SpriteRenderer FirstPrizeSpriteScript::sprite
		IL_000f: ret
	} // end of method FirstPrizeSpriteScript::Start

	.method private hidebysig 
		instance void Update () cil managed 
	{
		// Method begins at RVA 0xdcec
		// Header size: 12
		// Code size: 437 (0x1b5)
		.maxstack 5
		.locals init (
			[0] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[1] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[2] float32,
			[3] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[4] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[5] int64,
			[6] float32,
			[7] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[8] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[9] float32,
			[10] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[11] int32
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform FirstPrizeSpriteScript::cam
		IL_0007: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_000c: stloc.0
		IL_000d: ldarg.0
		IL_000e: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform FirstPrizeSpriteScript::body
		IL_0013: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0018: stloc.1
		IL_0019: ldloca.s 0
		IL_001b: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0020: stloc.2
		IL_0021: ldarg.0
		IL_0022: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform FirstPrizeSpriteScript::cam
		IL_0027: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_002c: stloc.3
		IL_002d: ldarg.0
		IL_002e: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform FirstPrizeSpriteScript::body
		IL_0033: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_0038: stloc.s 4
		IL_003a: ldstr "sub_20BC10"
		IL_003f: ldc.i8 0
		IL_0048: ldc.i8 0
		IL_0051: ldc.i8 0
		IL_005a: ldc.i8 0
		IL_0063: call int64 Il2CppStudio.Native.NativeIntrinsics::CallNative(string, int64, int64, int64, int64)
		IL_0068: stloc.s 5
		IL_006a: ldarg.0
		IL_006b: ldstr "xmm0"
		IL_0070: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_0075: conv.r4
		IL_0076: ldc.r4 57.29578
		IL_007b: mul
		IL_007c: stfld float32 FirstPrizeSpriteScript::angleF
		IL_0081: ldstr "xmm0"
		IL_0086: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_008b: conv.r4
		IL_008c: ldc.r4 57.29578
		IL_0091: mul
		IL_0092: stloc.s 6
		IL_0094: ldc.r4 0.0
		IL_0099: ldstr "xmm0"
		IL_009e: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_00a3: conv.r4
		IL_00a4: ldc.r4 57.29578
		IL_00a9: mul
		IL_00aa: cgt
		IL_00ac: ldc.i4.0
		IL_00ad: ceq
		IL_00af: brtrue.s IL_00ca

		IL_00b1: nop
		IL_00b2: ldarg.0
		IL_00b3: ldloc.s 6
		IL_00b5: ldc.r4 360
		IL_00ba: add
		IL_00bb: stfld float32 FirstPrizeSpriteScript::angleF
		IL_00c0: ldloc.s 6
		IL_00c2: ldc.r4 360
		IL_00c7: add
		IL_00c8: stloc.s 6

		IL_00ca: nop
		IL_00cb: ldarg.0
		IL_00cc: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform FirstPrizeSpriteScript::body
		IL_00d1: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_eulerAngles()
		IL_00d6: stloc.s 7
		IL_00d8: ldarg.0
		IL_00d9: ldfld class [UnityEngine.CoreModule]UnityEngine.Transform FirstPrizeSpriteScript::body
		IL_00de: stloc.s 8
		IL_00e0: ldarg.0
		IL_00e1: ldloca.s 7
		IL_00e3: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_00e8: stfld float32 FirstPrizeSpriteScript::debug
		IL_00ed: ldarg.0
		IL_00ee: ldfld float32 FirstPrizeSpriteScript::angleF
		IL_00f3: stloc.s 9
		IL_00f5: ldloc.s 8
		IL_00f7: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_eulerAngles()
		IL_00fc: stloc.s 10
		IL_00fe: ldarg.0
		IL_00ff: ldloc.s 9
		IL_0101: ldloca.s 10
		IL_0103: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0108: add
		IL_0109: stfld float32 FirstPrizeSpriteScript::angleF
		IL_010e: ldloc.s 9
		IL_0110: ldloca.s 10
		IL_0112: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_0117: add
		IL_0118: ldc.r4 22.5
		IL_011d: div
		IL_011e: call int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
		IL_0123: stloc.s 11
		IL_0125: ldarg.0
		IL_0126: ldloc.s 11
		IL_0128: stfld int32 FirstPrizeSpriteScript::angle
		// loop start (head: IL_012d)
			IL_012d: nop
			IL_012e: ldarg.0
			IL_012f: ldfld int32 FirstPrizeSpriteScript::angle
			IL_0134: ldc.i4.0
			IL_0135: clt
			IL_0137: brtrue.s IL_0146

			IL_0139: nop
			IL_013a: ldarg.0
			IL_013b: ldfld int32 FirstPrizeSpriteScript::angle
			IL_0140: ldc.i4.s 16
			IL_0142: clt.un
			IL_0144: brtrue.s IL_018f

			IL_0146: nop
			IL_0147: ldarg.0
			IL_0148: ldfld int32 FirstPrizeSpriteScript::angle
			IL_014d: conv.r4
			IL_014e: ldc.r4 0.0
			IL_0153: clt
			IL_0155: brtrue.s IL_0173

			IL_0157: nop
			IL_0158: ldarg.0
			IL_0159: ldarg.0
			IL_015a: ldfld int32 FirstPrizeSpriteScript::angle
			IL_015f: ldc.r4 1
			IL_0164: ldc.r4 -16
			IL_0169: mul
			IL_016a: conv.i4
			IL_016b: add
			IL_016c: stfld int32 FirstPrizeSpriteScript::angle
			IL_0171: br.s IL_012d

			IL_0173: nop
			IL_0174: ldarg.0
			IL_0175: ldarg.0
			IL_0176: ldfld int32 FirstPrizeSpriteScript::angle
			IL_017b: ldc.r4 -1
			IL_0180: ldc.r4 -16
			IL_0185: mul
			IL_0186: conv.i4
			IL_0187: add
			IL_0188: stfld int32 FirstPrizeSpriteScript::angle
			IL_018d: br.s IL_012d
		// end loop

		IL_018f: nop
		IL_0190: ldarg.0
		IL_0191: ldfld class [UnityEngine.CoreModule]UnityEngine.SpriteRenderer FirstPrizeSpriteScript::sprite
		IL_0196: ldarg.0
		IL_0197: ldfld class [UnityEngine.CoreModule]UnityEngine.Sprite[] FirstPrizeSpriteScript::sprites
		IL_019c: ldarg.0
		IL_019d: ldfld int32 FirstPrizeSpriteScript::angle
		IL_01a2: conv.i8
		IL_01a3: conv.i4
		IL_01a4: ldelem [UnityEngine.CoreModule]UnityEngine.Sprite
		IL_01a9: callvirt instance void [UnityEngine.CoreModule]UnityEngine.SpriteRenderer::set_sprite(class [UnityEngine.CoreModule]UnityEngine.Sprite)
		IL_01ae: ret

		IL_01af: nop
		IL_01b0: ldnull
		IL_01b1: throw

		IL_01b2: nop
		IL_01b3: ldnull
		IL_01b4: throw
	} // end of method FirstPrizeSpriteScript::Update

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0xdeb0
		// Header size: 12
		// Code size: 23 (0x17)
		.maxstack 2
		.locals init (
			[0] class [UnityEngine.CoreModule]UnityEngine.Sprite[]
		)

		IL_0000: nop
		IL_0001: ldc.i4.s 16
		IL_0003: newarr [UnityEngine.CoreModule]UnityEngine.Sprite
		IL_0008: stloc.0
		IL_0009: ldarg.0
		IL_000a: ldloc.0
		IL_000b: stfld class [UnityEngine.CoreModule]UnityEngine.Sprite[] FirstPrizeSpriteScript::sprites
		IL_0010: ldarg.0
		IL_0011: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0016: ret
	} // end of method FirstPrizeSpriteScript::.ctor

} // end of class FirstPrizeSpriteScript
