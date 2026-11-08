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
		float f = Random.Range(0f, 49f);
		int num = Mathf.RoundToInt(f);
		int num2 = num;
		bool isPlaying = audioDevice.isPlaying;
		int num3 = ((num == 0) ? ((isPlaying ? 1 : 0) ^ 1) : 0);
		bool flag = isPlaying;
		if (num3 != 0)
		{
			Transform transform = this.transform;
			Transform transform2 = transform;
			Vector3 position = aiLocation.position;
			Vector3 position2 = default(Vector3);
			position2.x = position.x;
			position2.y = position.y;
			position2.z = position.z;
			transform.position = position2;
			AudioClip[] array = sounds;
			float f2 = Random.Range(0f, array.Length - 1);
			int num4 = Mathf.RoundToInt(f2);
			int num5 = num4;
			audioDevice.PlayOneShot(sounds[num5]);
		}
	}
}

