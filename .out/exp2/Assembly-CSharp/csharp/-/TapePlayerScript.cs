using UnityEngine;

// Token: 0x0200004E RID: 78
public class TapePlayerScript : MonoBehaviour
{
	// Token: 0x04000236 RID: 566 Offset: 0x20
	public Sprite closedSprite;

	// Token: 0x04000237 RID: 567 Offset: 0x28
	public SpriteRenderer sprite;

	// Token: 0x04000238 RID: 568 Offset: 0x30
	public BaldiScript baldi;

	// Token: 0x04000239 RID: 569 Offset: 0x38
	private AudioSource audioDevice;

	// Token: 0x06000176 RID: 374 RVA: 0x0025F950 File Offset: 0x0025DF50
	private void Start()
	{
		audioDevice = GetComponent<AudioSource>();
	}

	// Token: 0x06000177 RID: 375 RVA: 0x0025F9A0 File Offset: 0x0025DFA0
	private void Update()
	{
		bool isPlaying = audioDevice.isPlaying;
		if (!(Time.timeScale == 0f && isPlaying))
		{
			float timeScale = Time.timeScale;
			BaldiScript baldiScript = baldi;
			float num = timeScale;
			_ = baldiScript.antiHearingTime;
			if ((num > 0f) & (baldiScript.antiHearingTime > 0f))
			{
				audioDevice.UnPause();
			}
		}
		else
		{
			audioDevice.Pause();
		}
	}

	// Token: 0x06000178 RID: 376 RVA: 0x0025F8E0 File Offset: 0x0025DEE0
	public void Play()
	{
		sprite.sprite = closedSprite;
		audioDevice.Play();
		if (baldi.isActiveAndEnabled)
		{
			baldi.ActivateAntiHearing(30f);
		}
	}
}

