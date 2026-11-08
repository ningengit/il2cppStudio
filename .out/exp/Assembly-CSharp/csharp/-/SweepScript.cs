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
		agent = GetComponent<NavMeshAgent>();
		audioDevice = GetComponent<AudioSource>();
		origin = transform.position;
		waitTime = Random.Range(120f, 180f);
	}

	// Token: 0x06000133 RID: 307 RVA: 0x00250640 File Offset: 0x0024EC40
	private void Update()
	{
		float num = coolDown;
		if (coolDown > 0f)
		{
			coolDown = num - Time.deltaTime;
		}
		num = waitTime;
		if ((waitTime > 0f))
		{
			waitTime = num - Time.deltaTime;
		}
		else if (!active)
		{
			active = true;
			wanders = 0;
			Wander();
			audioDevice.PlayOneShot(aud_Intro);
		}
	}

	// Token: 0x06000134 RID: 308 RVA: 0x00250360 File Offset: 0x0024E960
	private void FixedUpdate()
	{
		bool num = (wanders < 5 && (!(0.1 < (double)agent.velocity.magnitude) & !(0f < coolDown)));
		_ = !(0f < coolDown);
		if (active && num)
		{
			Wander();
		}
		else if (wanders >= 5)
		{
			NavMeshAgent navMeshAgent = agent;
			Vector3 destination = origin;
			navMeshAgent.SetDestination(destination);
			waitTime = Random.Range(120f, 180f);
			wanders = 0;
			active = false;
		}
	}

	// Token: 0x06000135 RID: 309 RVA: 0x002506E0 File Offset: 0x0024ECE0
	private void Wander()
	{
		wanderer.GetNewTargetHallway();
		agent.SetDestination(wanderTarget.position);
		wanders++;
		coolDown = 1f;
	}

	// Token: 0x06000136 RID: 310 RVA: 0x00250450 File Offset: 0x0024EA50
	private void GoHome()
	{
		NavMeshAgent navMeshAgent = agent;
		Vector3 destination = origin;
		navMeshAgent.SetDestination(destination);
		waitTime = Random.Range(120f, 180f);
		wanders = 0;
		active = false;
	}

	// Token: 0x06000137 RID: 311 RVA: 0x002504C0 File Offset: 0x0024EAC0
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "NPC" || other.tag == "Player")
		{
			audioDevice.PlayOneShot(aud_Sweep);
		}
	}
}

