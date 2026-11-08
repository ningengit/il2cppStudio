using UnityEngine;

// Token: 0x0200000B RID: 11
public class PlatformSpecificMenu : MonoBehaviour
{
	// Token: 0x0400006C RID: 108 Offset: 0x20
	public GameObject pC;

	// Token: 0x0400006D RID: 109 Offset: 0x28
	public GameObject mobile;

	// Token: 0x06000045 RID: 69 RVA: 0x0024E6D0 File Offset: 0x0024CCD0
	private void Start()
	{
		pC.SetActive(value: true);
	}
}

