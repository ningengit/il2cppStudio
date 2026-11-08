using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000039 RID: 57
public class PlaytimeScript : MonoBehaviour
{
	// Token: 0x04000189 RID: 393 Offset: 0x20
	public bool db;

	// Token: 0x0400018A RID: 394 Offset: 0x21
	public bool playerSeen;

	// Token: 0x0400018B RID: 395 Offset: 0x22
	public bool disappointed;

	// Token: 0x0400018C RID: 396 Offset: 0x24
	public int audVal;

	// Token: 0x0400018D RID: 397 Offset: 0x28
	public Animator animator;

	// Token: 0x0400018E RID: 398 Offset: 0x30
	public Transform player;

	// Token: 0x0400018F RID: 399 Offset: 0x38
	public PlayerScript ps;

	// Token: 0x04000190 RID: 400 Offset: 0x40
	public Transform wanderTarget;

	// Token: 0x04000191 RID: 401 Offset: 0x48
	public AILocationSelectorScript wanderer;

	// Token: 0x04000192 RID: 402 Offset: 0x50
	public float coolDown;

	// Token: 0x04000193 RID: 403 Offset: 0x54
	public float playCool;

	// Token: 0x04000194 RID: 404 Offset: 0x58
	public bool playerSpotted;

	// Token: 0x04000195 RID: 405 Offset: 0x59
	public bool jumpRopeStarted;

	// Token: 0x04000196 RID: 406 Offset: 0x60
	private NavMeshAgent agent;

	// Token: 0x04000197 RID: 407 Offset: 0x68
	public AudioClip[] aud_Numbers = new AudioClip[10];

	// Token: 0x04000198 RID: 408 Offset: 0x70
	public AudioClip[] aud_Random = new AudioClip[2];

	// Token: 0x04000199 RID: 409 Offset: 0x78
	public AudioClip aud_Instrcutions;

	// Token: 0x0400019A RID: 410 Offset: 0x80
	public AudioClip aud_Oops;

	// Token: 0x0400019B RID: 411 Offset: 0x88
	public AudioClip aud_LetsPlay;

	// Token: 0x0400019C RID: 412 Offset: 0x90
	public AudioClip aud_Congrats;

	// Token: 0x0400019D RID: 413 Offset: 0x98
	public AudioClip aud_ReadyGo;

	// Token: 0x0400019E RID: 414 Offset: 0xA0
	public AudioClip aud_Sad;

	// Token: 0x0400019F RID: 415 Offset: 0xA8
	public AudioSource audioDevice;

	// Token: 0x0600011E RID: 286 RVA: 0x0024EC50 File Offset: 0x0024D250
	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		audioDevice = GetComponent<AudioSource>();
		Wander();
	}

	// Token: 0x0600011F RID: 287 RVA: 0x0024EDD0 File Offset: 0x0024D3D0
	private void Update()
	{
		float num = coolDown;
		if (coolDown > 0f)
		{
			coolDown = num - Time.deltaTime;
		}
		num = playCool;
		if (!(playCool < 0f))
		{
			playCool = num - Time.deltaTime;
		}
		else if (this.animator.GetBool("disappointed"))
		{
			Animator animator = this.animator;
			playCool = 0f;
			animator.SetBool("disappointed", value: false);
		}
	}

	// Token: 0x06000120 RID: 288 RVA: 0x0024E780 File Offset: 0x0024CD80
	private void FixedUpdate()
	{
		if (ps.jumpRope)
		{
			if (!jumpRopeStarted)
			{
				NavMeshAgent navMeshAgent = agent;
				Vector3 position = this.transform.position;
				Vector3 forward = this.transform.forward;
				_ = forward.y * 10f;
				Vector3 newPosition = new Vector3(position.x - forward.x * 10f, position.y - forward.y * 10f, position.z - forward.z * 10f);
				navMeshAgent.Warp(newPosition);
			}
			NavMeshAgent navMeshAgent2 = agent;
			jumpRopeStarted = true;
			navMeshAgent2.speed = 0f;
			playCool = 15f;
			return;
		}
		Vector3 position2 = player.position;
		Transform transform = this.transform;
		float x = position2.x;
		float y = position2.y;
		float z = position2.z;
		Vector3 position3 = transform.position;
		Transform transform2 = this.transform;
		x -= position3.x;
		y -= position3.y;
		z -= position3.z;
		Vector3 position4 = transform2.position;
		Vector3 vector = new Vector3(position4.x, position4.y, position4.z);
		Vector3 origin = vector;
		Vector3 direction = new Vector3(x, y, z);
		RaycastHit hitInfo = default(RaycastHit);
		bool flag = Physics.Raycast(origin, direction, out hitInfo, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore);
		bool flag2 = hitInfo.transform.tag == "Player";
		Vector3 position5 = this.transform.position;
		Vector3 position6 = player.position;
		Vector3 vector2 = new Vector3(position5.x - position6.x, position5.y - position6.y, position5.z - position6.z);
		bool num = (!(80f < vector2.magnitude) && flag2 && flag);
		_ = !(0f < playCool);
		if (num && (!(0f < playCool)))
		{
			playerSeen = true;
			animator.SetBool("disappointed", value: false);
			agent.SetDestination(player.position);
			agent.speed = 20f;
			coolDown = 0.2f;
			if (!playerSpotted)
			{
				AudioSource audioSource = audioDevice;
				playerSpotted = true;
				audioSource.PlayOneShot(aud_LetsPlay);
			}
		}
		else if (!playerSeen | (0f < coolDown))
		{
			Vector3 vector3 = agent.velocity;
			if (!(1f < vector3.magnitude) & !(0f < coolDown))
			{
				Wander();
				jumpRopeStarted = false;
				return;
			}
		}
		else
		{
			playerSeen = false;
			Wander();
			jumpRopeStarted = false;
			return;
		}
		jumpRopeStarted = false;
	}

	// Token: 0x06000121 RID: 289 RVA: 0x0024EEA0 File Offset: 0x0024D4A0
	private void Wander()
	{
		wanderer.GetNewTargetHallway();
		agent.SetDestination(wanderTarget.position);
		agent.speed = 15f;
		playerSpotted = false;
		int num = Mathf.RoundToInt(Random.Range(0f, 1f));
		AudioSource audioSource = audioDevice;
		audVal = num;
		if (!audioSource.isPlaying)
		{
			AudioClip[] array = aud_Random;
			audioDevice.PlayOneShot(array[audVal]);
		}
		coolDown = 1f;
	}

	// Token: 0x06000122 RID: 290 RVA: 0x0024ECE0 File Offset: 0x0024D2E0
	private void TargetPlayer()
	{
		animator.SetBool("disappointed", value: false);
		agent.SetDestination(player.position);
		agent.speed = 20f;
		coolDown = 0.2f;
		if (!playerSpotted)
		{
			AudioSource audioSource = audioDevice;
			playerSpotted = true;
			audioSource.PlayOneShot(aud_LetsPlay);
		}
	}

	// Token: 0x06000123 RID: 291 RVA: 0x0024E700 File Offset: 0x0024CD00
	public void Disappoint()
	{
		animator.SetBool("disappointed", value: true);
		audioDevice.Stop();
		audioDevice.PlayOneShot(aud_Sad);
	}
}

