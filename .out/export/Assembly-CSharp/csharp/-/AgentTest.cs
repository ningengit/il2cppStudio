using Il2CppStudio.Native;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x0200002D RID: 45
public class AgentTest : MonoBehaviour
{
	// Token: 0x04000114 RID: 276 Offset: 0x20
	public bool db;

	// Token: 0x04000115 RID: 277 Offset: 0x28
	public Transform player;

	// Token: 0x04000116 RID: 278 Offset: 0x30
	public Transform wanderTarget;

	// Token: 0x04000117 RID: 279 Offset: 0x38
	public AILocationSelectorScript wanderer;

	// Token: 0x04000118 RID: 280 Offset: 0x40
	public float coolDown;

	// Token: 0x04000119 RID: 281 Offset: 0x48
	private NavMeshAgent agent;

	// Token: 0x060000DE RID: 222 RVA: 0x0023E950 File Offset: 0x0023CF50
	private void Start()
	{
		NavMeshAgent component = GetComponent<NavMeshAgent>();
		agent = component;
		Wander();
	}

	// Token: 0x060000DF RID: 223 RVA: 0x0023EA20 File Offset: 0x0023D020
	private void Update()
	{
		float num = coolDown;
		if (coolDown > 0f)
		{
			float deltaTime = Time.deltaTime;
			coolDown = num - deltaTime;
			float num2 = deltaTime;
		}
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x0023E6C0 File Offset: 0x0023CCC0
	private void FixedUpdate()
	{
		Transform transform = player;
		Vector3 position = transform.position;
		Transform transform2 = this.transform;
		float x = position.x;
		float y = position.y;
		float z = position.z;
		Vector3 position2 = transform2.position;
		Transform transform3 = this.transform;
		x -= position2.x;
		y -= position2.y;
		z -= position2.z;
		Vector3 position3 = transform3.position;
		long num = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
		float z2 = position3.z;
		Vector3 vector = default(Vector3);
		vector.x = NativeIntrinsics.Reinterpret<float>(num);
		vector.z = z2;
		Vector3 origin = vector;
		Vector3 direction = default(Vector3);
		direction.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("unpcklps xmm6,xmm7"));
		direction.z = z;
		RaycastHit hitInfo;
		bool flag = Physics.Raycast(origin, direction, out hitInfo, float.PositiveInfinity, 3, NativeIntrinsics.Reinterpret<QueryTriggerInteraction>(1));
		RaycastHit raycastHit = default(RaycastHit);
		Transform transform4 = raycastHit.transform;
		int num2 = (flag ? 1 : 0);
		string a = transform4.tag;
		bool flag2 = string.Equals(a, "Player");
		NavMeshAgent navMeshAgent = agent;
		if (((uint)num2 & (flag2 ? 1u : 0u)) == 0)
		{
			db = false;
			Vector3 velocity = navMeshAgent.velocity;
			Vector3 vector2 = default(Vector3);
			vector2.x = velocity.x;
			vector2.y = velocity.y;
			vector2.z = velocity.z;
			float magnitude = vector2.magnitude;
			int num3 = ((!(1f < magnitude)) ? 1 : 0);
			float num4 = magnitude;
			if (!(1f < magnitude) & !(0f < coolDown))
			{
				Wander();
			}
		}
		else
		{
			Transform transform5 = player;
			db = true;
			Vector3 position4 = transform5.position;
			Vector3 destination = default(Vector3);
			destination.x = position4.x;
			destination.y = position4.y;
			destination.z = position4.z;
			bool flag3 = navMeshAgent.SetDestination(destination);
			coolDown = 1f;
			bool flag4 = flag3;
		}
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x0023EA60 File Offset: 0x0023D060
	private void Wander()
	{
		wanderer.GetNewTarget();
		NavMeshAgent navMeshAgent = agent;
		Vector3 position = wanderTarget.position;
		Vector3 destination = default(Vector3);
		destination.x = position.x;
		destination.y = position.y;
		destination.z = position.z;
		bool flag = navMeshAgent.SetDestination(destination);
		coolDown = 1f;
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x0023E9B0 File Offset: 0x0023CFB0
	private void TargetPlayer()
	{
		NavMeshAgent navMeshAgent = agent;
		Vector3 position = player.position;
		Vector3 destination = default(Vector3);
		destination.x = position.x;
		destination.y = position.y;
		destination.z = position.z;
		bool flag = navMeshAgent.SetDestination(destination);
		coolDown = 1f;
	}
}

