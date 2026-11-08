using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000028 RID: 40
public class GameOverScript : MonoBehaviour
{
	// Token: 0x04000106 RID: 262 Offset: 0x20
	private Image image;

	// Token: 0x04000107 RID: 263 Offset: 0x28
	private float delay;

	// Token: 0x04000108 RID: 264 Offset: 0x30
	public Sprite[] images = new Sprite[5];

	// Token: 0x04000109 RID: 265 Offset: 0x38
	public Sprite rare;

	// Token: 0x0400010A RID: 266 Offset: 0x40
	private float chance;

	// Token: 0x0400010B RID: 267 Offset: 0x48
	private AudioSource audioDevice;

	// Token: 0x0400010C RID: 268 Offset: 0x50
	public string LoadScene;

	// Token: 0x060000D0 RID: 208 RVA: 0x00248B50 File Offset: 0x00247150
	private void Start()
	{
		image = GetComponent<Image>();
		audioDevice = GetComponent<AudioSource>();
		delay = 5f;
		if (!(98f > (chance = Random.Range(1f, 99f))))
		{
			image.sprite = rare;
			return;
		}
		int num = Mathf.RoundToInt(Random.Range(0f, 4f));
		image.sprite = images[num];
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x00248C60 File Offset: 0x00247260
	private void Update()
	{
		float num = delay;
		float deltaTime = Time.deltaTime;
		delay = num - deltaTime;
		if (0f < num - deltaTime)
		{
			return;
		}
		if (!(98f > chance))
		{
			Transform obj = image.transform;
			Vector3 localScale = new Vector3(5f, 5f, 1f);
			obj.localScale = localScale;
			Image obj2 = image;
			obj2.color = Color.red;
			if (!audioDevice.isPlaying)
			{
				audioDevice.Play();
			}
			if (!(-5f < delay))
			{
				Application.Quit();
			}
		}
		else
		{
			SceneManager.LoadScene(LoadScene);
		}
	}
}

