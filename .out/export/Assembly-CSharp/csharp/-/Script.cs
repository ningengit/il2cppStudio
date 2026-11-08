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
		int num = (isPlaying ? 1 : 0) ^ 1;
		if (((played ? 1u : 0u) & ((isPlaying ? 1u : 0u) ^ 1u)) != 0)
		{
			Application.Quit();
			Debug.Log("The game is closed.");
		}
	}

	// Token: 0x06000130 RID: 304 RVA: 0x00250120 File Offset: 0x0024E720
	private void OnTriggerEnter(Collider other)
	{
		string a = other.name;
		bool flag = string.Equals(a, "Player");
		int num = ((!played && flag) ? 1 : 0);
		int num2 = num;
		if (num != 0)
		{
			audioDevice.Play();
			played = true;
		}
	}
}

