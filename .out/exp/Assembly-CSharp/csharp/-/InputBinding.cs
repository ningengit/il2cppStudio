using System;
using System.Runtime.InteropServices;
using Il2CppStudio.Native;
using UnityEngine;

// Token: 0x0200001C RID: 28
[Serializable]
public struct InputBinding
{
	// Token: 0x040000DA RID: 218 Offset: 0x0
	public KeyCode primaryKey;

	// Token: 0x040000DB RID: 219 Offset: 0x4
	public KeyCode secondaryKey;

	// Token: 0x06000098 RID: 152 RVA: 0x00249060 File Offset: 0x00247660
	public unsafe InputBinding(KeyCode primary, [Optional] KeyCode secondary)
	{
		((InputBinding*)NativeIntrinsics.Asm("&ThisStructExpr"))->primaryKey = primary;
		((InputBinding*)NativeIntrinsics.Asm("&ThisStructExpr"))->secondaryKey = secondary;
	}
}

