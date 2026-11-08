using UnityEngine;

// Token: 0x0200003B RID: 59
public class Script : MonoBehaviour
{
	// Token: 0x040001C6 RID: 454 Offset: 0x20
	public AudioSource audioDevice;

	// Token: 0x040001C7 RID: 455 Offset: 0x28
	private bool played;

	// Token: 0x0600012F RID: 303 RVA: 0x002501B0 File Offset: 0x0024E7B0
	private void Update()
	{
		bool isPlaying = audioDevice.isPlaying;
		if (played && !isPlaying)
		{
			Application.Quit();
			Debug.Log("The game is closed.");
		}
	}

	// Token: 0x06000130 RID: 304 RVA: 0x00250120 File Offset: 0x0024E720
	private void OnTriggerEnter(Collider other)
	{
		bool flag = other.name == "Player";
		if (!played && flag)
		{
			audioDevice.Play();
			played = true;
		}
	}
}

