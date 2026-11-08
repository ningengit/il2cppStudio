.class public auto ansi beforefieldinit SecretScript
	extends [UnityEngine.CoreModule]UnityEngine.MonoBehaviour
{
	// Methods
	.method public hidebysig 
		instance void LoadScene () cil managed 
	{
		// Method begins at RVA 0x2226
		// Header size: 1
		// Code size: 16 (0x10)
		.maxstack 8

		IL_0000: nop
		IL_0001: br.s IL_0004

		IL_0003: nop

		IL_0004: nop
		IL_0005: ldstr "Secret"
		IL_000a: call void [UnityEngine.CoreModule]UnityEngine.SceneManagement.SceneManager::LoadScene(string)
		IL_000f: ret
	} // end of method SecretScript::LoadScene

	.method public hidebysig specialname rtspecialname 
		instance void .ctor () cil managed 
	{
		// Method begins at RVA 0x2237
		// Header size: 1
		// Code size: 8 (0x8)
		.maxstack 8

		IL_0000: nop
		IL_0001: ldarg.0
		IL_0002: callvirt instance void [UnityEngine.CoreModule]UnityEngine.Behaviour::.ctor()
		IL_0007: ret
	} // end of method SecretScript::.ctor

} // end of class SecretScript
