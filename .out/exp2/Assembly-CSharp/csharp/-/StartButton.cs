using Il2CppStudio.Native;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000012 RID: 18
public class StartButton : MonoBehaviour
{
	// Token: 0x02000013 RID: 19
	public enum Mode
	{
		// Token: 0x04000079 RID: 121 Offset: 0x0
		Story,
		// Token: 0x0400007A RID: 122 Offset: 0x0
		Endless
	}

	// Token: 0x04000076 RID: 118 Offset: 0x20
	public Mode currentMode;

	// Token: 0x04000077 RID: 119 Offset: 0x28
	public string LoadScene;

	// Token: 0x0600005D RID: 93 RVA: 0x002502C0 File Offset: 0x0024E8C0
	public void StartGame()
	{
		if (NativeIntrinsics.Reinterpret<int>(currentMode) == 0)
		{
		}
		PlayerPrefs.SetString("CurrentMode", "endless");
		SceneManager.LoadSceneAsync(LoadScene);
	}
}

