using Il2CppStudio.Native;
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
		NavMeshAgent component = GetComponent<NavMeshAgent>();
		agent = component;
	}

	// Token: 0x0600015F RID: 351 RVA: 0x00253940 File Offset: 0x00251F40
	private void Update()
	{
		if (inBsoda)
		{
			NavMeshAgent navMeshAgent = agent;
			Vector3 velocity = default(Vector3);
			velocity.x = otherVelocity.x;
			velocity.y = otherVelocity.y;
			velocity.z = otherVelocity.z;
			navMeshAgent.velocity = velocity;
		}
		float num = failSave;
		if (!(failSave > 0f))
		{
			inBsoda = false;
			return;
		}
		float deltaTime = Time.deltaTime;
		failSave = num - deltaTime;
	}

	// Token: 0x06000160 RID: 352 RVA: 0x00253710 File Offset: 0x00251D10
	private void OnTriggerStay(Collider other)
	{
		string a = other.tag;
		if (!string.Equals(a, "BSODA"))
		{
			Transform transform = other.transform;
			Transform transform2 = transform;
			string a2 = transform2.name;
			bool flag = string.Equals(a2, "Gotta Sweep");
			bool flag2 = flag;
			if (!flag)
			{
				return;
			}
			inBsoda = true;
			Transform transform3 = this.transform;
			Transform transform4 = transform3;
			Vector3 forward = transform4.forward;
			float speed = agent.speed;
			NavMeshAgent component = other.GetComponent<NavMeshAgent>();
			NavMeshAgent navMeshAgent = component;
			Vector3 velocity = navMeshAgent.velocity;
			otherVelocity = NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("unpcklps xmm6,xmm7"));
			otherVelocity.z = forward.z * speed * 0.1f + velocity.z;
		}
		else
		{
			inBsoda = true;
			Rigidbody component2 = other.GetComponent<Rigidbody>();
			Rigidbody rigidbody = component2;
			Vector3 velocity2 = rigidbody.velocity;
			otherVelocity.x = velocity2.x;
			otherVelocity.y = velocity2.y;
			otherVelocity.z = velocity2.z;
			long num = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
		}
		failSave = 1f;
	}

	// Token: 0x06000161 RID: 353 RVA: 0x00248E70 File Offset: 0x00247470
	private void OnTriggerExit()
	{
		inBsoda = false;
	}
}

