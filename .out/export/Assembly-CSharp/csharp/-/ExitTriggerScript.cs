using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000042 RID: 66
public class ExitTriggerScript : MonoBehaviour
{
	// Token: 0x040001F8 RID: 504 Offset: 0x20
	public GameControllerScript gc;

	// Token: 0x040001F9 RID: 505 Offset: 0x28
	public string ResultsScene;

	// Token: 0x040001FA RID: 506 Offset: 0x30
	public string SecretScene;

	// Token: 0x0600014E RID: 334 RVA: 0x00244B30 File Offset: 0x00243130
	private void OnTriggerEnter(Collider other)
	{
		int notebooks = gc.notebooks;
		string a = other.tag;
		bool flag = string.Equals(a, "Player");
		int num = ((notebooks >= 7 && flag) ? 1 : 0);
		int num2 = (flag ? 1 : 0);
		if (num != 0)
		{
			string sceneName = ((gc.failedNotebooks >= 7) ? SecretScene : ResultsScene);
			SceneManager.LoadScene(sceneName);
		}
	}
}

