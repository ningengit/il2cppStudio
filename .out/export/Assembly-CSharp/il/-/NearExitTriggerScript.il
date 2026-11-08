.class public auto ansi beforefieldinit NearExitTriggerScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Fields
	.field public class GameControllerScript gc
	.field public class EntranceScript es

	// Methods
	.method private hidebysig 
		instance void OnTriggerEnter (
			class [UnityEngine.PhysicsModule]UnityEngine.Collider other
		) cil managed 
	{
		// Method begins at RVA 0xb164
		// Header size: 12
		// Code size: 571 (0x23b)
		.maxstack 3
		.locals init (
			[0] int32,
			[1] int32,
			[2] string,
			[3] bool,
			[4] int32,
			[5] int32,
			[6] class GameControllerScript,
			[7] int32,
			[8] valuetype [UnityEngine.CoreModule]UnityEngine.Color,
			[9] bool,
			[10] bool,
			[11] class BaldiScript,
			[12] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[13] class [UnityEngine.CoreModule]UnityEngine.Transform,
			[14] class BaldiScript,
			[15] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[16] valuetype [UnityEngine.CoreModule]UnityEngine.Vector3,
			[17] bool,
			[18] bool
		)

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: ldfld class GameControllerScript NearExitTriggerScript::gc
		IL_0007: ldfld int32 GameControllerScript::exitsReached
		IL_000c: stloc.0
		IL_000d: ldarg.0
		IL_000e: ldfld class GameControllerScript NearExitTriggerScript::gc
		IL_0013: ldfld bool GameControllerScript::finaleMode
		IL_0018: stloc.1
		IL_0019: ldarg.1
		IL_001a: callvirt instance string [UnityEngine.CoreModule]UnityEngine.Component::get_tag()
		IL_001f: stloc.2
		IL_0020: ldloc.2
		IL_0021: ldstr "Player"
		IL_0026: call bool [mscorlib]System.String::Equals(string, string)
		IL_002b: stloc.3
		IL_002c: ldloc.0
		IL_002d: ldc.i4.3
		IL_002e: clt
		IL_0030: brfalse.s IL_0039

		IL_0032: ldloc.3
		IL_0033: ldloc.1
		IL_0034: and
		IL_0035: stloc.s 4
		IL_0037: br.s IL_003d

		IL_0039: nop
		IL_003a: ldc.i4.0
		IL_003b: stloc.s 4

		IL_003d: nop
		IL_003e: ldloc.s 4
		IL_0040: stloc.s 5
		IL_0042: ldloc.s 4
		IL_0044: ldc.i4.0
		IL_0045: ceq
		IL_0047: brtrue IL_0236

		IL_004c: nop
		IL_004d: ldarg.0
		IL_004e: ldfld class GameControllerScript NearExitTriggerScript::gc
		IL_0053: stloc.s 6
		IL_0055: ldloc.s 6
		IL_0057: ldfld int32 GameControllerScript::exitsReached
		IL_005c: stloc.s 7
		IL_005e: ldloc.s 6
		IL_0060: ldloc.s 6
		IL_0062: ldfld int32 GameControllerScript::exitsReached
		IL_0067: conv.i8
		IL_0068: ldc.i8 1
		IL_0071: add
		IL_0072: conv.i4
		IL_0073: stfld int32 GameControllerScript::exitsReached
		IL_0078: ldloc.s 7
		IL_007a: ldc.i4.0
		IL_007b: ceq
		IL_007d: ldc.i4.0
		IL_007e: ceq
		IL_0080: brtrue.s IL_00e9

		IL_0082: nop
		IL_0083: ldloca.s 8
		IL_0085: ldstr "global_13EAC10"
		IL_008a: call int64 Il2CppStudio.Native.NativeIntrinsics::Asm(string)
		IL_008f: box [mscorlib]System.Int64
		IL_0094: call !!0 Il2CppStudio.Native.NativeIntrinsics::Reinterpret<float32>(object)
		IL_0099: stfld float32 [UnityEngine.CoreModule]UnityEngine.Color::r
		IL_009e: ldloc.s 8
		IL_00a0: call void [UnityEngine.CoreModule]UnityEngine.RenderSettings::set_ambientLight(valuetype [UnityEngine.CoreModule]UnityEngine.Color)
		IL_00a5: ldloc.s 6
		IL_00a7: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_00ac: ldloc.s 6
		IL_00ae: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip GameControllerScript::aud_Switch
		IL_00b3: ldc.r4 0.8
		IL_00b8: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::PlayOneShot(class [UnityEngine.AudioModule]UnityEngine.AudioClip, float32)
		IL_00bd: ldloc.s 6
		IL_00bf: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_00c4: ldloc.s 6
		IL_00c6: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip GameControllerScript::aud_MachineQuiet
		IL_00cb: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_clip(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_00d0: ldloc.s 6
		IL_00d2: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_00d7: ldc.i4.1
		IL_00d8: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_loop(bool)
		IL_00dd: ldloc.s 6
		IL_00df: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_00e4: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Play()

		IL_00e9: nop
		IL_00ea: ldloc.s 6
		IL_00ec: ldfld int32 GameControllerScript::exitsReached
		IL_00f1: ldc.i4.2
		IL_00f2: ceq
		IL_00f4: ldc.i4.0
		IL_00f5: ceq
		IL_00f7: brtrue.s IL_0137

		IL_00f9: nop
		IL_00fa: ldloc.s 6
		IL_00fc: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_0101: ldc.r4 0.8
		IL_0106: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_volume(float32)
		IL_010b: ldloc.s 6
		IL_010d: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_0112: ldloc.s 6
		IL_0114: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip GameControllerScript::aud_MachineStart
		IL_0119: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_clip(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_011e: ldloc.s 6
		IL_0120: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_0125: ldc.i4.1
		IL_0126: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_loop(bool)
		IL_012b: ldloc.s 6
		IL_012d: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_0132: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Play()

		IL_0137: nop
		IL_0138: ldloc.s 6
		IL_013a: ldfld int32 GameControllerScript::exitsReached
		IL_013f: ldc.i4.3
		IL_0140: ceq
		IL_0142: ldc.i4.0
		IL_0143: ceq
		IL_0145: brtrue.s IL_0174

		IL_0147: nop
		IL_0148: ldloc.s 6
		IL_014a: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_014f: ldloc.s 6
		IL_0151: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioClip GameControllerScript::aud_MachineRev
		IL_0156: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_clip(class [UnityEngine.AudioModule]UnityEngine.AudioClip)
		IL_015b: ldloc.s 6
		IL_015d: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_0162: ldc.i4.0
		IL_0163: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::set_loop(bool)
		IL_0168: ldloc.s 6
		IL_016a: ldfld class [UnityEngine.AudioModule]UnityEngine.AudioSource GameControllerScript::audioDevice
		IL_016f: callvirt instance void [UnityEngine.AudioModule]UnityEngine.AudioSource::Play()

		IL_0174: nop
		IL_0175: ldarg.0
		IL_0176: ldfld class EntranceScript NearExitTriggerScript::es
		IL_017b: callvirt instance void EntranceScript::Lower()
		IL_0180: ldarg.0
		IL_0181: ldfld class GameControllerScript NearExitTriggerScript::gc
		IL_0186: ldfld class BaldiScript GameControllerScript::baldiScrpt
		IL_018b: callvirt instance bool [UnityEngine.CoreModule]UnityEngine.Behaviour::get_isActiveAndEnabled()
		IL_0190: stloc.s 9
		IL_0192: ldloc.s 9
		IL_0194: stloc.s 10
		IL_0196: ldloc.s 9
		IL_0198: ldc.i4.0
		IL_0199: ceq
		IL_019b: brtrue IL_0236

		IL_01a0: nop
		IL_01a1: ldarg.0
		IL_01a2: ldfld class GameControllerScript NearExitTriggerScript::gc
		IL_01a7: stloc.s 6
		IL_01a9: ldloc.s 6
		IL_01ab: ldfld class BaldiScript GameControllerScript::baldiScrpt
		IL_01b0: stloc.s 11
		IL_01b2: ldarg.0
		IL_01b3: callvirt instance class [UnityEngine.CoreModule]UnityEngine.Transform [UnityEngine.CoreModule]UnityEngine.Component::get_transform()
		IL_01b8: stloc.s 12
		IL_01ba: ldloc.s 12
		IL_01bc: stloc.s 13
		IL_01be: ldloc.s 11
		IL_01c0: stloc.s 14
		IL_01c2: ldloc.s 13
		IL_01c4: callvirt instance valuetype [UnityEngine.CoreModule]UnityEngine.Vector3 [UnityEngine.CoreModule]UnityEngine.Transform::get_position()
		IL_01c9: stloc.s 15
		IL_01cb: ldloc.s 14
		IL_01cd: ldfld bool BaldiScript::antiHearing
		IL_01d2: ldc.i4.0
		IL_01d3: ceq
		IL_01d5: ldc.i4.0
		IL_01d6: ceq
		IL_01d8: brtrue.s IL_0236

		IL_01da: nop
		IL_01db: ldc.r4 8
		IL_01e0: ldloc.s 14
		IL_01e2: ldfld float32 BaldiScript::currentPriority
		IL_01e7: clt
		IL_01e9: brtrue.s IL_0236

		IL_01eb: nop
		IL_01ec: ldloc.s 14
		IL_01ee: ldfld class [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent BaldiScript::agent
		IL_01f3: ldloca.s 16
		IL_01f5: ldloca.s 15
		IL_01f7: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_01fc: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::x
		IL_0201: ldloca.s 16
		IL_0203: ldloca.s 15
		IL_0205: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_020a: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::y
		IL_020f: ldloca.s 16
		IL_0211: ldloca.s 15
		IL_0213: ldfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_0218: stfld float32 [UnityEngine.CoreModule]UnityEngine.Vector3::z
		IL_021d: ldloc.s 16
		IL_021f: callvirt instance bool [UnityEngine.AIModule]UnityEngine.AI.NavMeshAgent::SetDestination(valuetype [UnityEngine.CoreModule]UnityEngine.Vector3)
		IL_0224: stloc.s 17
		IL_0226: ldloc.s 14
		IL_0228: ldc.r4 8
		IL_022d: stfld float32 BaldiScript::currentPriority
		IL_0232: ldloc.s 17
		IL_0234: stloc.s 18

		IL_0236: nop
		IL_0237: ret

		IL_0238: nop
		IL_0239: ldnull
		IL_023a: throw
	} // end of method NearExitTriggerScript::OnTriggerEnter

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x24a3
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method NearExitTriggerScript::.ctor

} // end of class NearExitTriggerScript
