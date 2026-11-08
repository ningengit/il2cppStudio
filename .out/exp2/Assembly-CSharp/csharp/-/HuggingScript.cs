using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000037 RID: 55
public class HuggingScript : MonoBehaviour
{
	// Token: 0x04000185 RID: 389 Offset: 0x20
	private Rigidbody rb;

	// Token: 0x04000186 RID: 390 Offset: 0x28
	private Vector3 otherVelocity;

	// Token: 0x04000187 RID: 391 Offset: 0x34
	public bool inBsoda;

	// Token: 0x04000188 RID: 392 Offset: 0x38
	private float failSave;

	// Token: 0x06000116 RID: 278 RVA: 0x00248FC0 File Offset: 0x002475C0
	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	// Token: 0x06000117 RID: 279 RVA: 0x00249010 File Offset: 0x00247610
	private void Update()
	{
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

	// Token: 0x06000118 RID: 280 RVA: 0x00248E30 File Offset: 0x00247430
	private void FixedUpdate()
	{
		if (inBsoda)
		{
			Rigidbody rigidbody = rb;
			rigidbody.velocity = otherVelocity;
		}
	}

	// Token: 0x06000119 RID: 281 RVA: 0x00248E80 File Offset: 0x00247480
	private void OnTriggerStay(Collider other)
	{
		if (other.transform.name == "1st Prize")
		{
			Rigidbody rigidbody = rb;
			inBsoda = true;
			Vector3 velocity = rigidbody.velocity;
			Vector3 velocity2 = other.GetComponent<NavMeshAgent>().velocity;
			failSave = 1f;
			otherVelocity = new Vector3(velocity.x * 0.1f + velocity2.x, velocity.y * 0.1f + velocity2.y, velocity.z * 0.1f + velocity2.z);
		}
	}

	// Token: 0x0600011A RID: 282 RVA: 0x00248E70 File Offset: 0x00247470
	private void OnTriggerExit()
	{
		inBsoda = false;
	}
}

