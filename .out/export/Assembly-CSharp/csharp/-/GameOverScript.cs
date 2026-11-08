using Il2CppStudio.Native;
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
		Image component = GetComponent<Image>();
		image = component;
		AudioSource component2 = GetComponent<AudioSource>();
		audioDevice = component2;
		delay = 5f;
		if (!(98f > (chance = Random.Range(1f, 99f))))
		{
			image.sprite = rare;
			return;
		}
		float f = Random.Range(0f, 4f);
		int num = Mathf.RoundToInt(f);
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
			Transform transform = image.transform;
			Transform transform2 = transform;
			Vector3 localScale = default(Vector3);
			localScale.x = 5f;
			localScale.y = 5f;
			localScale.z = 1f;
			transform2.localScale = localScale;
			long num2 = NativeIntrinsics.Asm("global_13EAC10");
			bool isPlaying = audioDevice.isPlaying;
			bool flag = isPlaying;
			if (!isPlaying)
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
			string loadScene = LoadScene;
			SceneManager.LoadScene(loadScene);
		}
	}
}

