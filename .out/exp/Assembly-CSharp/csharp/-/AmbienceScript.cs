using UnityEngine;

// Token: 0x02000002 RID: 2
public class AmbienceScript : MonoBehaviour
{
	// Token: 0x04000001 RID: 1 Offset: 0x20
	public Transform aiLocation;

	// Token: 0x04000002 RID: 2 Offset: 0x28
	public AudioClip[] sounds;

	// Token: 0x04000003 RID: 3 Offset: 0x30
	public AudioSource audioDevice;

	// Token: 0x06000001 RID: 1 RVA: 0x0023EAF0 File Offset: 0x0023D0F0
	public void PlayAudio()
	{
		int num = Mathf.RoundToInt(Random.Range(0f, 49f));
		bool isPlaying = audioDevice.isPlaying;
		if (num == 0 && !isPlaying)
		{
			transform.position = aiLocation.position;
			int num2 = Mathf.RoundToInt(Random.Range(0f, sounds.Length - 1));
			audioDevice.PlayOneShot(sounds[num2]);
		}
	}
}

