using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000033 RID: 51
public class CraftersTriggerScript : MonoBehaviour
{
	// Token: 0x0400015E RID: 350 Offset: 0x20
	public Transform goTarget;

	// Token: 0x0400015F RID: 351 Offset: 0x28
	public Transform fleeTarget;

	// Token: 0x04000160 RID: 352 Offset: 0x30
	public CraftersScript crafters;

	// Token: 0x06000105 RID: 261 RVA: 0x00243380 File Offset: 0x00241980
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag != "Player")
		{
			return;
		}
		CraftersScript craftersScript = crafters;
		Vector3 position = goTarget.position;
		CraftersScript craftersScript2 = craftersScript;
		if (!craftersScript2.angry)
		{
			bool flag = craftersScript2.agent.isActiveAndEnabled;
			float z = position.z;
			if (flag)
			{
				NavMeshAgent agent = craftersScript2.agent;
				Vector3 destination = new Vector3(position.x, position.y, z);
				agent.SetDestination(destination);
			}
		}
	}

	// Token: 0x06000106 RID: 262 RVA: 0x00243450 File Offset: 0x00241A50
	private void OnTriggerExit(Collider other)
	{
		if (other.tag != "Player")
		{
			return;
		}
		CraftersScript craftersScript = crafters;
		Vector3 position = fleeTarget.position;
		CraftersScript craftersScript2 = craftersScript;
		if (!craftersScript2.angry)
		{
			bool flag = craftersScript2.agent.isActiveAndEnabled;
			float z = position.z;
			if (flag)
			{
				NavMeshAgent agent = craftersScript2.agent;
				Vector3 destination = new Vector3(position.x, position.y, z);
				agent.SetDestination(destination);
				craftersScript2.forceShowTime = 3f;
			}
		}
	}
}

