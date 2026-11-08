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
		agent = GetComponent<NavMeshAgent>();
		Wander();
	}

	// Token: 0x060000DF RID: 223 RVA: 0x0023EA20 File Offset: 0x0023D020
	private void Update()
	{
		float num = coolDown;
		if (coolDown > 0f)
		{
			coolDown = num - Time.deltaTime;
		}
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x0023E6C0 File Offset: 0x0023CCC0
	private void FixedUpdate()
	{
		Vector3 position = player.position;
		Transform transform = this.transform;
		float x = position.x;
		float y = position.y;
		float z = position.z;
		Vector3 position2 = transform.position;
		Transform transform2 = this.transform;
		x -= position2.x;
		y -= position2.y;
		z -= position2.z;
		Vector3 position3 = transform2.position;
		Vector3 vector = new Vector3(position3.x, position3.y, position3.z);
		Vector3 origin = vector;
		Vector3 direction = new Vector3(x, y, z);
		RaycastHit hitInfo;
		bool flag = Physics.Raycast(origin, direction, out hitInfo, float.PositiveInfinity, 3, QueryTriggerInteraction.Ignore);
		Transform transform3 = hitInfo.transform;
		bool num = flag;
		bool flag2 = transform3.tag == "Player";
		NavMeshAgent navMeshAgent = agent;
		if (!(num && flag2))
		{
			db = false;
			Vector3 vector2 = navMeshAgent.velocity;
			if (!(1f < vector2.magnitude) & !(0f < coolDown))
			{
				Wander();
			}
		}
		else
		{
			Transform transform4 = player;
			db = true;
			navMeshAgent.SetDestination(transform4.position);
			coolDown = 1f;
		}
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x0023EA60 File Offset: 0x0023D060
	private void Wander()
	{
		wanderer.GetNewTarget();
		agent.SetDestination(wanderTarget.position);
		coolDown = 1f;
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x0023E9B0 File Offset: 0x0023CFB0
	private void TargetPlayer()
	{
		agent.SetDestination(player.position);
		coolDown = 1f;
	}
}

