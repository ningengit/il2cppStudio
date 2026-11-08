using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000055 RID: 85
public class WarningScreenScript : MonoBehaviour
{
	// Token: 0x04000275 RID: 629 Offset: 0x20
	public string LoadScene;

	// Token: 0x060001A2 RID: 418 RVA: 0x00263AC0 File Offset: 0x002620C0
	private void Update()
	{
		if (Input.anyKeyDown)
		{
			SceneManager.LoadScene(LoadScene);
		}
	}
}

