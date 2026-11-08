using Il2CppStudio.Native;
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
		string a = other.tag;
		bool flag = string.Equals(a, "Player");
		bool flag2 = flag;
		if (!flag)
		{
			return;
		}
		CraftersScript craftersScript = crafters;
		Vector3 position = goTarget.position;
		CraftersScript craftersScript2 = craftersScript;
		if (!craftersScript2.angry)
		{
			bool flag3 = craftersScript2.agent.isActiveAndEnabled;
			flag2 = flag3;
			long num = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
			float z = position.z;
			if (flag3)
			{
				NavMeshAgent agent = craftersScript2.agent;
				Vector3 destination = default(Vector3);
				destination.x = NativeIntrinsics.Reinterpret<float>(num);
				destination.z = z;
				bool flag4 = agent.SetDestination(destination);
				flag2 = flag4;
			}
		}
	}

	// Token: 0x06000106 RID: 262 RVA: 0x00243450 File Offset: 0x00241A50
	private void OnTriggerExit(Collider other)
	{
		string a = other.tag;
		bool flag = string.Equals(a, "Player");
		bool flag2 = flag;
		if (!flag)
		{
			return;
		}
		CraftersScript craftersScript = crafters;
		Vector3 position = fleeTarget.position;
		CraftersScript craftersScript2 = craftersScript;
		if (!craftersScript2.angry)
		{
			bool flag3 = craftersScript2.agent.isActiveAndEnabled;
			flag2 = flag3;
			long num = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
			float z = position.z;
			if (flag3)
			{
				NavMeshAgent agent = craftersScript2.agent;
				Vector3 destination = default(Vector3);
				destination.x = NativeIntrinsics.Reinterpret<float>(num);
				destination.z = z;
				bool flag4 = agent.SetDestination(destination);
				craftersScript2.forceShowTime = 3f;
				flag2 = flag4;
			}
		}
	}
}

