using UnityEngine;

// Token: 0x0200004D RID: 77
public class TapePlayerRecordingsScript : MonoBehaviour
{
	// Token: 0x04000230 RID: 560 Offset: 0x20
	public Sprite closedSprite;

	// Token: 0x04000231 RID: 561 Offset: 0x28
	public SpriteRenderer sprite;

	// Token: 0x04000232 RID: 562 Offset: 0x30
	private int audVal;

	// Token: 0x04000233 RID: 563 Offset: 0x38
	public AudioClip[] recordings = new AudioClip[5];

	// Token: 0x04000234 RID: 564 Offset: 0x40
	public BaldiScript baldi;

	// Token: 0x04000235 RID: 565 Offset: 0x48
	private AudioSource audioDevice;

	// Token: 0x06000173 RID: 371 RVA: 0x0025F830 File Offset: 0x0025DE30
	private void Start()
	{
		audioDevice = GetComponent<AudioSource>();
	}

	// Token: 0x06000174 RID: 372 RVA: 0x0025F730 File Offset: 0x0025DD30
	public void Play()
	{
		sprite.sprite = closedSprite;
		int num = Mathf.RoundToInt(Random.Range(0f, 4f));
		AudioClip[] array = recordings;
		audVal = num;
		audioDevice.PlayOneShot(array[num]);
		if (baldi.isActiveAndEnabled)
		{
			baldi.Hear(transform.position, 4f);
		}
	}
}

