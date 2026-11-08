using Il2CppStudio.Native;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200002C RID: 44
public class NoKeyboardScript : InputField
{
	// Token: 0x060000DC RID: 220 RVA: 0x0024E390 File Offset: 0x0024C990
	protected override void Start()
	{
		keyboardType = NativeIntrinsics.Reinterpret<TouchScreenKeyboardType>(-1);
	}
}

