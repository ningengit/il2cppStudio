using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000046 RID: 70
public class BsodaEffectScript : MonoBehaviour
{
	// Token: 0x04000218 RID: 536 Offset: 0x20
	private NavMeshAgent agent;

	// Token: 0x04000219 RID: 537 Offset: 0x28
	private Vector3 otherVelocity;

	// Token: 0x0400021A RID: 538 Offset: 0x34
	private bool inBsoda;

	// Token: 0x0400021B RID: 539 Offset: 0x38
	private float failSave;

	// Token: 0x0600015E RID: 350 RVA: 0x002538F0 File Offset: 0x00251EF0
	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	// Token: 0x0600015F RID: 351 RVA: 0x00253940 File Offset: 0x00251F40
	private void Update()
	{
		if (inBsoda)
		{
			NavMeshAgent navMeshAgent = agent;
			navMeshAgent.velocity = otherVelocity;
		}
		float num = failSave;
		if (!(failSave > 0f))
		{
			inBsoda = false;
		}
		else
		{
			failSave = num - Time.deltaTime;
		}
	}

	// Token: 0x06000160 RID: 352 RVA: 0x00253710 File Offset: 0x00251D10
	private void OnTriggerStay(Collider other)
	{
		BsodaEffectScript bsodaEffectScript;
		if (other.tag == "BSODA")
		{
			inBsoda = true;
			Vector3 velocity2 = other.GetComponent<Rigidbody>().velocity;
			otherVelocity = velocity2;
			bsodaEffectScript = this;
		}
		else if (other.transform.name == "Gotta Sweep")
		{
			inBsoda = true;
			Vector3 forward = transform.forward;
			float speed = agent.speed;
			Vector3 velocity = other.GetComponent<NavMeshAgent>().velocity;
			otherVelocity = new Vector3(forward.x * speed * 0.1f + velocity.x, forward.y * speed * 0.1f + velocity.y, forward.z * speed * 0.1f + velocity.z);
			bsodaEffectScript = this;
		}
		else
		{
			return;
		}
		bsodaEffectScript.failSave = 1f;
	}

	// Token: 0x06000161 RID: 353 RVA: 0x00248E70 File Offset: 0x00247470
	private void OnTriggerExit()
	{
		inBsoda = false;
	}
}

