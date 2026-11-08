using Il2CppStudio.Native;
using UnityEngine;

// Token: 0x02000005 RID: 5
public class CursorControllerScript : MonoBehaviour
{
	// Token: 0x0600000D RID: 13 RVA: 0x00243530 File Offset: 0x00241B30
	public void LockCursor()
	{
		Cursor.lockState = NativeIntrinsics.Reinterpret<CursorLockMode>(1);
		Cursor.visible = false;
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00243550 File Offset: 0x00241B50
	public void UnlockCursor()
	{
		Cursor.lockState = NativeIntrinsics.Reinterpret<CursorLockMode>(0);
		Cursor.visible = true;
	}
}

