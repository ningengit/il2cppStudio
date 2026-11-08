using UnityEngine;
using UnityEngine.AI;

// Token: 0x0200003C RID: 60
public class SweepScript : MonoBehaviour
{
	// Token: 0x040001C8 RID: 456 Offset: 0x20
	public Transform wanderTarget;

	// Token: 0x040001C9 RID: 457 Offset: 0x28
	public AILocationSelectorScript wanderer;

	// Token: 0x040001CA RID: 458 Offset: 0x30
	public float coolDown;

	// Token: 0x040001CB RID: 459 Offset: 0x34
	public float waitTime;

	// Token: 0x040001CC RID: 460 Offset: 0x38
	public int wanders;

	// Token: 0x040001CD RID: 461 Offset: 0x3C
	public bool active;

	// Token: 0x040001CE RID: 462 Offset: 0x40
	private Vector3 origin;

	// Token: 0x040001CF RID: 463 Offset: 0x50
	public AudioClip aud_Sweep;

	// Token: 0x040001D0 RID: 464 Offset: 0x58
	public AudioClip aud_Intro;

	// Token: 0x040001D1 RID: 465 Offset: 0x60
	private NavMeshAgent agent;

	// Token: 0x040001D2 RID: 466 Offset: 0x68
	private AudioSource audioDevice;

	// Token: 0x06000132 RID: 306 RVA: 0x00250570 File Offset: 0x0024EB70
	private void Start()
	{
		NavMeshAgent component = GetComponent<NavMeshAgent>();
		agent = component;
		AudioSource component2 = GetComponent<AudioSource>();
		audioDevice = component2;
		Transform transform = this.transform;
		Vector3 position = transform.position;
		origin.x = position.x;
		origin.y = position.y;
		origin.z = position.z;
		float num = Random.Range(120f, 180f);
		waitTime = num;
	}

	// Token: 0x06000133 RID: 307 RVA: 0x00250640 File Offset: 0x0024EC40
	private void Update()
	{
		float num = coolDown;
		if (coolDown > 0f)
		{
			float deltaTime = Time.deltaTime;
			coolDown = num - deltaTime;
		}
		num = waitTime;
		if (!(waitTime > 0f))
		{
			if (!active)
			{
				active = true;
				wanders = 0;
				Wander();
				audioDevice.PlayOneShot(aud_Intro);
			}
		}
		else
		{
			float deltaTime2 = Time.deltaTime;
			waitTime = num - deltaTime2;
			num = deltaTime2;
		}
	}

	// Token: 0x06000134 RID: 308 RVA: 0x00250360 File Offset: 0x0024E960
	private void FixedUpdate()
	{
		Vector3 velocity = agent.velocity;
		Vector3 vector = default(Vector3);
		vector.x = velocity.x;
		vector.y = velocity.y;
		vector.z = velocity.z;
		float magnitude = vector.magnitude;
		int num = ((wanders < 5 && (!(0.1 < (double)magnitude) & !(0f < coolDown))) ? 1 : 0);
		int num2 = ((!(0f < coolDown)) ? 1 : 0);
		double num3 = magnitude;
		if (((active ? 1u : 0u) & (uint)num) == 0)
		{
			if (wanders >= 5)
			{
				NavMeshAgent navMeshAgent = agent;
				Vector3 destination = default(Vector3);
				destination.x = origin.x;
				destination.y = origin.y;
				destination.z = origin.z;
				bool flag = navMeshAgent.SetDestination(destination);
				float num4 = (waitTime = Random.Range(120f, 180f));
				wanders = 0;
				active = false;
				float num5 = num4;
			}
		}
		else
		{
			Wander();
		}
	}

	// Token: 0x06000135 RID: 309 RVA: 0x002506E0 File Offset: 0x0024ECE0
	private void Wander()
	{
		wanderer.GetNewTargetHallway();
		NavMeshAgent navMeshAgent = agent;
		Vector3 position = wanderTarget.position;
		Vector3 destination = default(Vector3);
		destination.x = position.x;
		destination.y = position.y;
		destination.z = position.z;
		bool flag = navMeshAgent.SetDestination(destination);
		wanders++;
		coolDown = 1f;
	}

	// Token: 0x06000136 RID: 310 RVA: 0x00250450 File Offset: 0x0024EA50
	private void GoHome()
	{
		NavMeshAgent navMeshAgent = agent;
		Vector3 destination = default(Vector3);
		destination.x = origin.x;
		destination.y = origin.y;
		destination.z = origin.z;
		bool flag = navMeshAgent.SetDestination(destination);
		float num = Random.Range(120f, 180f);
		waitTime = num;
		wanders = 0;
		active = false;
	}

	// Token: 0x06000137 RID: 311 RVA: 0x002504C0 File Offset: 0x0024EAC0
	private void OnTriggerEnter(Collider other)
	{
		string a = other.tag;
		if (!string.Equals(a, "NPC"))
		{
			string a2 = other.tag;
			bool flag = string.Equals(a2, "Player");
			bool flag2 = flag;
			if (!flag)
			{
				return;
			}
		}
		audioDevice.PlayOneShot(aud_Sweep);
	}
}

