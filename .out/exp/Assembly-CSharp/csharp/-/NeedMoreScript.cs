using UnityEngine;

// Token: 0x0200002B RID: 43
public class NeedMoreScript : MonoBehaviour
{
	// Token: 0x04000111 RID: 273 Offset: 0x20
	public GameControllerScript gc;

	// Token: 0x04000112 RID: 274 Offset: 0x28
	public AudioSource audioDevice;

	// Token: 0x04000113 RID: 275 Offset: 0x30
	public AudioClip baldiDoor;

	// Token: 0x060000DA RID: 218 RVA: 0x0024E2E0 File Offset: 0x0024C8E0
	private void OnTriggerEnter(Collider other)
	{
		int notebooks = gc.notebooks;
		bool flag = other.tag == "Player";
		if (notebooks < 2 && flag)
		{
			audioDevice.PlayOneShot(baldiDoor, 1f);
		}
	}
}

