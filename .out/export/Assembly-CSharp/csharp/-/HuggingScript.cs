using Il2CppStudio.Native;
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
		Rigidbody component = GetComponent<Rigidbody>();
		rb = component;
	}

	// Token: 0x06000117 RID: 279 RVA: 0x00249010 File Offset: 0x00247610
	private void Update()
	{
		float num = failSave;
		if (!(failSave > 0f))
		{
			inBsoda = false;
			return;
		}
		float deltaTime = Time.deltaTime;
		failSave = num - deltaTime;
	}

	// Token: 0x06000118 RID: 280 RVA: 0x00248E30 File Offset: 0x00247430
	private void FixedUpdate()
	{
		if (inBsoda)
		{
			Rigidbody rigidbody = rb;
			Vector3 velocity = default(Vector3);
			velocity.x = otherVelocity.x;
			velocity.y = otherVelocity.y;
			velocity.z = otherVelocity.z;
			rigidbody.velocity = velocity;
		}
	}

	// Token: 0x06000119 RID: 281 RVA: 0x00248E80 File Offset: 0x00247480
	private void OnTriggerStay(Collider other)
	{
		Transform transform = other.transform;
		Transform transform2 = transform;
		string a = transform2.name;
		bool flag = string.Equals(a, "1st Prize");
		bool flag2 = flag;
		if (flag)
		{
			Rigidbody rigidbody = rb;
			inBsoda = true;
			Vector3 velocity = rigidbody.velocity;
			NavMeshAgent component = other.GetComponent<NavMeshAgent>();
			NavMeshAgent navMeshAgent = component;
			Vector3 velocity2 = navMeshAgent.velocity;
			failSave = 1f;
			otherVelocity = NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("unpcklps xmm6,xmm7"));
			otherVelocity.z = velocity.z * 0.1f + velocity2.z;
		}
	}

	// Token: 0x0600011A RID: 282 RVA: 0x00248E70 File Offset: 0x00247470
	private void OnTriggerExit()
	{
		inBsoda = false;
	}
}

