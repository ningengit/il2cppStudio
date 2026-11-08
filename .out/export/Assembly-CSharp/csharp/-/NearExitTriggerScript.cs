using Il2CppStudio.Native;
using UnityEngine;
using UnityEngine.AI;

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
		int finaleMode = (gc.finaleMode ? 1 : 0);
		string a = other.tag;
		bool flag = string.Equals(a, "Player");
		int num = ((exitsReached < 3) ? ((flag ? 1 : 0) & finaleMode) : 0);
		int num2 = num;
		if (num == 0)
		{
			return;
		}
		GameControllerScript gameControllerScript = gc;
		int exitsReached2 = gameControllerScript.exitsReached;
		gameControllerScript.exitsReached = (int)(gameControllerScript.exitsReached + 1L);
		if (exitsReached2 == 0)
		{
			Color ambientLight = default(Color);
			ambientLight.r = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("global_13EAC10"));
			RenderSettings.ambientLight = ambientLight;
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
		bool flag2 = gc.baldiScrpt.isActiveAndEnabled;
		bool flag3 = flag2;
		if (flag2)
		{
			gameControllerScript = gc;
			BaldiScript baldiScrpt = gameControllerScript.baldiScrpt;
			Transform transform = this.transform;
			Transform transform2 = transform;
			BaldiScript baldiScript = baldiScrpt;
			Vector3 position = transform2.position;
			if (!baldiScript.antiHearing && !(8f < baldiScript.currentPriority))
			{
				NavMeshAgent agent = baldiScript.agent;
				Vector3 destination = default(Vector3);
				destination.x = position.x;
				destination.y = position.y;
				destination.z = position.z;
				bool flag4 = agent.SetDestination(destination);
				baldiScript.currentPriority = 8f;
				bool flag5 = flag4;
			}
		}
	}
}

