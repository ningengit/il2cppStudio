using UnityEngine;

// Token: 0x02000056 RID: 86
public class YouWonScript : MonoBehaviour
{
	// Token: 0x04000276 RID: 630 Offset: 0x20
	private float delay;

	// Token: 0x060001A4 RID: 420 RVA: 0x00263B60 File Offset: 0x00262160
	private void Start()
	{
		delay = 10f;
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x00263B70 File Offset: 0x00262170
	private void Update()
	{
		float num = delay;
		float deltaTime = Time.deltaTime;
		delay = num - deltaTime;
		if (!(0f < num - deltaTime))
		{
			Application.Quit();
		}
	}
}

