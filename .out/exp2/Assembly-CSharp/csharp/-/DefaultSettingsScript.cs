using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200001F RID: 31
public class DefaultSettingsScript : MonoBehaviour
{
	// Token: 0x040000E3 RID: 227 Offset: 0x20
	public GameObject options;

	// Token: 0x040000E4 RID: 228 Offset: 0x28
	public Canvas canvas;

	// Token: 0x060000AC RID: 172 RVA: 0x00243E30 File Offset: 0x00242430
	private void Start()
	{
		if (!PlayerPrefs.HasKey("OptionsSet"))
		{
			options.SetActive(value: true);
			StartCoroutine(CloseOptions());
			canvas.enabled = false;
		}
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00243DC0 File Offset: 0x002423C0
	public IEnumerator CloseOptions()
	{
		// Il2CppStudio: coroutine reconstructed from <CloseOptions>d__1.MoveNext (state machine class hidden).
		yield return new WaitForEndOfFrame();
		canvas.enabled = true;
		options.SetActive(value: false);
	}
}

