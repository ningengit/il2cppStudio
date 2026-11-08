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
		AudioSource component = GetComponent<AudioSource>();
		audioDevice = component;
	}

	// Token: 0x06000174 RID: 372 RVA: 0x0025F730 File Offset: 0x0025DD30
	public void Play()
	{
		sprite.sprite = closedSprite;
		float f = Random.Range(0f, 4f);
		int num = Mathf.RoundToInt(f);
		AudioClip[] array = recordings;
		audVal = num;
		int num2 = num;
		AudioClip[] array2 = array;
		audioDevice.PlayOneShot(array2[num2]);
		bool flag = baldi.isActiveAndEnabled;
		bool flag2 = flag;
		if (flag)
		{
			BaldiScript baldiScript = baldi;
			Transform transform = this.transform;
			Transform transform2 = transform;
			Vector3 position = transform2.position;
			Vector3 soundLocation = default(Vector3);
			soundLocation.x = position.x;
			soundLocation.y = position.y;
			soundLocation.z = position.z;
			baldiScript.Hear(soundLocation, 4f);
		}
	}
}

