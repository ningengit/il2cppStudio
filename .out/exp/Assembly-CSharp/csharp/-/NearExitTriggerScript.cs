using UnityEngine;

// Token: 0x0200002A RID: 42
public class NearExitTriggerScript : MonoBehaviour
{
	// Token: 0x0400010F RID: 271 Offset: 0x20
	public GameControllerScript gc;

	// Token: 0x04000110 RID: 272 Offset: 0x28
	public EntranceScript es;

	// Token: 0x060000D8 RID: 216 RVA: 0x0024DFF0 File Offset: 0x0024C5F0
	private void OnTriggerEnter(Collider other)
	{
		int exitsReached = gc.exitsReached;
		bool finaleMode = gc.finaleMode;
		bool flag = other.tag == "Player";
		if (exitsReached >= 3 || !(flag && finaleMode))
		{
			return;
		}
		GameControllerScript gameControllerScript = gc;
		int exitsReached2 = gameControllerScript.exitsReached;
		gameControllerScript.exitsReached = (int)(gameControllerScript.exitsReached + 1L);
		if (exitsReached2 == 0)
		{
			RenderSettings.ambientLight = Color.red;
			gameControllerScript.audioDevice.PlayOneShot(gameControllerScript.aud_Switch, 0.8f);
			gameControllerScript.audioDevice.clip = gameControllerScript.aud_MachineQuiet;
			gameControllerScript.audioDevice.loop = true;
			gameControllerScript.audioDevice.Play();
		}
		if (gameControllerScript.exitsReached == 2)
		{
			gameControllerScript.audioDevice.volume = 0.8f;
			gameControllerScript.audioDevice.clip = gameControllerScript.aud_MachineStart;
			gameControllerScript.audioDevice.loop = true;
			gameControllerScript.audioDevice.Play();
		}
		if (gameControllerScript.exitsReached == 3)
		{
			gameControllerScript.audioDevice.clip = gameControllerScript.aud_MachineRev;
			gameControllerScript.audioDevice.loop = false;
			gameControllerScript.audioDevice.Play();
		}
		es.Lower();
		if (gc.baldiScrpt.isActiveAndEnabled)
		{
			BaldiScript baldiScrpt = gc.baldiScrpt;
			Transform transform = this.transform;
			BaldiScript baldiScript = baldiScrpt;
			Vector3 position = transform.position;
			if (!baldiScript.antiHearing && !(8f < baldiScript.currentPriority))
			{
				baldiScript.agent.SetDestination(position);
				baldiScript.currentPriority = 8f;
			}
		}
	}
}

