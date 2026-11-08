using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200000F RID: 15
public class BasicButtonScript : MonoBehaviour
{
	// Token: 0x04000074 RID: 116 Offset: 0x20
	private Button button;

	// Token: 0x04000075 RID: 117 Offset: 0x28
	public GameObject screen;

	// Token: 0x06000056 RID: 86 RVA: 0x0023F930 File Offset: 0x0023DF30
	private void Start()
	{
		button = GetComponent<Button>();
		button.m_OnClick.AddListener(OpenScreen);
	}

	// Token: 0x06000057 RID: 87 RVA: 0x0023F900 File Offset: 0x0023DF00
	private void OpenScreen()
	{
		screen.SetActive(value: true);
	}
}

