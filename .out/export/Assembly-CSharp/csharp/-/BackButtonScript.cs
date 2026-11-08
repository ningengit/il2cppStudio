using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200000E RID: 14
public class BackButtonScript : MonoBehaviour
{
	// Token: 0x04000072 RID: 114 Offset: 0x20
	private Button button;

	// Token: 0x04000073 RID: 115 Offset: 0x28
	public GameObject screen;

	// Token: 0x06000053 RID: 83 RVA: 0x0023EF10 File Offset: 0x0023D510
	private void Start()
	{
		Button component = GetComponent<Button>();
		button = component;
		Button.ButtonClickedEvent onClick = button.m_OnClick;
		UnityAction call = (UnityAction)(object)new Action(CloseScreen);
		onClick.AddListener(call);
	}

	// Token: 0x06000054 RID: 84 RVA: 0x0023EEE0 File Offset: 0x0023D4E0
	private void CloseScreen()
	{
		screen.SetActive(value: false);
	}
}

