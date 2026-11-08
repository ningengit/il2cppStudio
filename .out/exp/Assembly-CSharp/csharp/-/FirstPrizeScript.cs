using System;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000035 RID: 53
public class FirstPrizeScript : MonoBehaviour
{
	// Token: 0x04000163 RID: 355 Offset: 0x20
	public float debug;

	// Token: 0x04000164 RID: 356 Offset: 0x24
	public float turnSpeed;

	// Token: 0x04000165 RID: 357 Offset: 0x28
	public float str;

	// Token: 0x04000166 RID: 358 Offset: 0x2C
	public float angleDiff;

	// Token: 0x04000167 RID: 359 Offset: 0x30
	public float normSpeed;

	// Token: 0x04000168 RID: 360 Offset: 0x34
	public float runSpeed;

	// Token: 0x04000169 RID: 361 Offset: 0x38
	public float currentSpeed;

	// Token: 0x0400016A RID: 362 Offset: 0x3C
	public float acceleration;

	// Token: 0x0400016B RID: 363 Offset: 0x40
	public float speed;

	// Token: 0x0400016C RID: 364 Offset: 0x44
	public float autoBrakeCool;

	// Token: 0x0400016D RID: 365 Offset: 0x48
	public float crazyTime;

	// Token: 0x0400016E RID: 366 Offset: 0x4C
	public Quaternion targetRotation;

	// Token: 0x0400016F RID: 367 Offset: 0x5C
	public float coolDown;

	// Token: 0x04000170 RID: 368 Offset: 0x60
	private float prevSpeed;

	// Token: 0x04000171 RID: 369 Offset: 0x64
	public bool playerSeen;

	// Token: 0x04000172 RID: 370 Offset: 0x65
	public bool hugAnnounced;

	// Token: 0x04000173 RID: 371 Offset: 0x68
	public AILocationSelectorScript wanderer;

	// Token: 0x04000174 RID: 372 Offset: 0x70
	public Transform player;

	// Token: 0x04000175 RID: 373 Offset: 0x78
	public Transform wanderTarget;

	// Token: 0x04000176 RID: 374 Offset: 0x80
	public AudioClip[] aud_Found = new AudioClip[2];

	// Token: 0x04000177 RID: 375 Offset: 0x88
	public AudioClip[] aud_Lost = new AudioClip[2];

	// Token: 0x04000178 RID: 376 Offset: 0x90
	public AudioClip[] aud_Hug = new AudioClip[2];

	// Token: 0x04000179 RID: 377 Offset: 0x98
	public AudioClip[] aud_Random = new AudioClip[2];

	// Token: 0x0400017A RID: 378 Offset: 0xA0
	public AudioClip audBang;

	// Token: 0x0400017B RID: 379 Offset: 0xA8
	public AudioSource audioDevice;

	// Token: 0x0400017C RID: 380 Offset: 0xB0
	public AudioSource motorAudio;

	// Token: 0x0400017D RID: 381 Offset: 0xB8
	private NavMeshAgent agent;

	// Token: 0x0600010A RID: 266 RVA: 0x002451E0 File Offset: 0x002437E0
	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		coolDown = 1f;
		Wander();
	}

	// Token: 0x0600010B RID: 267 RVA: 0x002452B0 File Offset: 0x002438B0
	private void Update()
	{
		float num = coolDown;
		float num2 = num;
		if (num > 0f)
		{
			coolDown = num2 - Time.deltaTime;
		}
		num2 = autoBrakeCool;
		if (!(autoBrakeCool > 0f))
		{
			agent.autoBraking = true;
		}
		else
		{
			autoBrakeCool = num2 - Time.deltaTime;
		}
		Vector3 eulerAngles = this.transform.eulerAngles;
		Vector3 steeringTarget = agent.steeringTarget;
		Transform transform = this.transform;
		float x = steeringTarget.x;
		Vector3 position = transform.position;
		Vector3 steeringTarget2 = agent.steeringTarget;
		Transform transform2 = this.transform;
		float z = steeringTarget2.z;
		Vector3 position2 = transform2.position;
		float num3 = (float)Math.Atan2(x - position.x, z - position2.z);
		float num4 = (float)Math.Floor((num3 * 57.29578f - eulerAngles.y) / 360f);
		z = num3 * 57.29578f - eulerAngles.y - num4 * 360f;
		if ((0f > num3 * 57.29578f - eulerAngles.y - num4 * 360f))
		{
			z = 0f;
		}
		else if (z > 360f)
		{
			z = 360f;
		}
		if (z > 180f)
		{
			z -= 360f;
		}
		angleDiff = z;
		if (0f < crazyTime)
		{
			agent.speed = 0f;
			Transform transform3 = this.transform;
			num2 = Time.deltaTime * 180f;
			Vector3 eulers = new Vector3(0f, num2, 0f);
			transform3.Rotate(eulers);
			crazyTime -= Time.deltaTime;
		}
		else
		{
			NavMeshAgent navMeshAgent = default(NavMeshAgent);
			float num6 = default(float);
			if (!(5f > Math.Abs(z)))
			{
				Transform transform4 = this.transform;
				_ = angleDiff;
				float num5 = turnSpeed;
				Transform transform5 = transform4;
				z = ((angleDiff < 0f) ? (-1f) : 1f);
				num2 = Time.deltaTime * (z * num5);
				Vector3 eulers2 = new Vector3(0f, num2, 0f);
				transform5.Rotate(eulers2);
				navMeshAgent = agent;
				num6 = 0f;
			}
			else
			{
				Transform transform6 = this.transform;
				Vector3 steeringTarget3 = agent.steeringTarget;
				Vector3 position3 = this.transform.position;
				Vector3 worldPosition = new Vector3(steeringTarget3.x, position3.y, agent.steeringTarget.z);
				transform6.LookAt(worldPosition);
				navMeshAgent = agent;
				num6 = currentSpeed;
			}
			navMeshAgent.speed = num6;
		}
		AudioSource source = motorAudio;
		Vector3 vector = agent.velocity;
		AudioSource.SetPitch(source, (vector.magnitude + 1f) * Time.timeScale);
	}

	// Token: 0x0600010C RID: 268 RVA: 0x00244C10 File Offset: 0x00243210
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
		RaycastHit hitInfo = default(RaycastHit);
		bool flag = Physics.Raycast(origin, direction, out hitInfo, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore);
		bool flag2 = hitInfo.transform.tag == "Player";
		bool num = playerSeen;
		if (!(flag && flag2))
		{
			currentSpeed = normSpeed;
			FirstPrizeScript firstPrizeScript = default(FirstPrizeScript);
			if (!(num && (!(0f < coolDown))))
			{
				float magnitude = agent.velocity.magnitude;
				float num2 = coolDown;
				Vector3 position4 = this.transform.position;
				Vector3 destination = agent.destination;
				Vector3 vector3 = new Vector3(position4.x - destination.x, position4.y - destination.y, position4.z - destination.z);
				if (!(1f < magnitude) && (5f > vector3.magnitude && !(0f < num2)))
				{
					firstPrizeScript = this;
				}
				else
				{
					return;
				}
			}
			else
			{
				if (!audioDevice.isPlaying)
				{
					int num3 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
					AudioClip[] array = aud_Lost;
					audioDevice.PlayOneShot(array[num3]);
				}
				playerSeen = false;
				firstPrizeScript = this;
			}
			firstPrizeScript.Wander();
		}
		else
		{
			AudioSource audioSource = default(AudioSource);
			if ((object)audioSource == null && !audioDevice.isPlaying)
			{
				int num4 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
				audioDevice.PlayOneShot(aud_Found[num4]);
			}
			Transform transform3 = player;
			playerSeen = true;
			agent.SetDestination(transform3.position);
			currentSpeed = runSpeed;
			coolDown = 0.5f;
		}
	}

	// Token: 0x0600010D RID: 269 RVA: 0x002456E0 File Offset: 0x00243CE0
	private void Wander()
	{
		wanderer.GetNewTargetHallway();
		Transform transform = wanderTarget;
		agent.SetDestination(transform.position);
		hugAnnounced = false;
		int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 9f));
		bool isPlaying = audioDevice.isPlaying;
		if ((num == 0) && ((!(0f < coolDown)) && !isPlaying))
		{
			int num2 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
			audioDevice.PlayOneShot(aud_Random[num2]);
		}
		coolDown = 1f;
	}

	// Token: 0x0600010E RID: 270 RVA: 0x00245240 File Offset: 0x00243840
	private void TargetPlayer()
	{
		agent.SetDestination(player.position);
		coolDown = 0.5f;
	}

	// Token: 0x0600010F RID: 271 RVA: 0x00245070 File Offset: 0x00243670
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			bool isPlaying = audioDevice.isPlaying;
			if (!hugAnnounced && !isPlaying)
			{
				int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
				AudioClip[] array = aud_Hug;
				audioDevice.PlayOneShot(array[num]);
				hugAnnounced = true;
			}
			agent.autoBraking = false;
		}
	}

	// Token: 0x06000110 RID: 272 RVA: 0x00245170 File Offset: 0x00243770
	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			autoBrakeCool = 1f;
		}
	}

	// Token: 0x06000111 RID: 273 RVA: 0x00245060 File Offset: 0x00243660
	public void GoCrazy()
	{
		crazyTime = 15f;
	}
}

