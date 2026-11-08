using Il2CppStudio.Native;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x0200002F RID: 47
public class BaldiScript : MonoBehaviour
{
	// Token: 0x0400011D RID: 285 Offset: 0x20
	public bool db;

	// Token: 0x0400011E RID: 286 Offset: 0x24
	public float baseTime;

	// Token: 0x0400011F RID: 287 Offset: 0x28
	public float speed;

	// Token: 0x04000120 RID: 288 Offset: 0x2C
	public float timeToMove;

	// Token: 0x04000121 RID: 289 Offset: 0x30
	public float baldiAnger;

	// Token: 0x04000122 RID: 290 Offset: 0x34
	public float baldiTempAnger;

	// Token: 0x04000123 RID: 291 Offset: 0x38
	public float baldiWait;

	// Token: 0x04000124 RID: 292 Offset: 0x3C
	public float baldiSpeedScale;

	// Token: 0x04000125 RID: 293 Offset: 0x40
	private float moveFrames;

	// Token: 0x04000126 RID: 294 Offset: 0x44
	private float currentPriority;

	// Token: 0x04000127 RID: 295 Offset: 0x48
	public bool antiHearing;

	// Token: 0x04000128 RID: 296 Offset: 0x4C
	public float antiHearingTime;

	// Token: 0x04000129 RID: 297 Offset: 0x50
	public float vibrationDistance;

	// Token: 0x0400012A RID: 298 Offset: 0x54
	public float angerRate;

	// Token: 0x0400012B RID: 299 Offset: 0x58
	public float angerRateRate;

	// Token: 0x0400012C RID: 300 Offset: 0x5C
	public float angerFrequency;

	// Token: 0x0400012D RID: 301 Offset: 0x60
	public float timeToAnger;

	// Token: 0x0400012E RID: 302 Offset: 0x64
	public bool endless;

	// Token: 0x0400012F RID: 303 Offset: 0x68
	public Transform player;

	// Token: 0x04000130 RID: 304 Offset: 0x70
	public Transform wanderTarget;

	// Token: 0x04000131 RID: 305 Offset: 0x78
	public AILocationSelectorScript wanderer;

	// Token: 0x04000132 RID: 306 Offset: 0x80
	private AudioSource baldiAudio;

	// Token: 0x04000133 RID: 307 Offset: 0x88
	public AudioClip slap;

	// Token: 0x04000134 RID: 308 Offset: 0x90
	public Animator baldiAnimator;

	// Token: 0x04000135 RID: 309 Offset: 0x98
	public float coolDown;

	// Token: 0x04000136 RID: 310 Offset: 0x9C
	private Vector3 previous;

	// Token: 0x04000137 RID: 311 Offset: 0xA8
	private bool rumble;

	// Token: 0x04000138 RID: 312 Offset: 0xB0
	private NavMeshAgent agent;

	// Token: 0x060000E8 RID: 232 RVA: 0x0023F5D0 File Offset: 0x0023DBD0
	private void Start()
	{
		AudioSource component = GetComponent<AudioSource>();
		baldiAudio = component;
		NavMeshAgent component2 = GetComponent<NavMeshAgent>();
		agent = component2;
		timeToMove = baseTime;
		Wander();
		int num = PlayerPrefs.GetInt("Rumble");
		int num2 = num;
		if (num == 1)
		{
			rumble = (byte)num2 != 0;
		}
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x0023F710 File Offset: 0x0023DD10
	private void Update()
	{
		float num = timeToMove;
		if (!(timeToMove > 0f))
		{
			Move();
		}
		else
		{
			float deltaTime = Time.deltaTime;
			timeToMove = num - deltaTime;
		}
		num = coolDown;
		if (coolDown > 0f)
		{
			float deltaTime2 = Time.deltaTime;
			coolDown = num - deltaTime2;
		}
		num = baldiTempAnger;
		if (!(baldiTempAnger > 0f))
		{
			baldiTempAnger = 0f;
		}
		else
		{
			float deltaTime3 = Time.deltaTime;
			baldiTempAnger = num - deltaTime3 * 0.02f;
		}
		num = antiHearingTime;
		if (!(antiHearingTime > 0f))
		{
			antiHearing = false;
		}
		else
		{
			float deltaTime4 = Time.deltaTime;
			antiHearingTime = num - deltaTime4;
			num = deltaTime4;
		}
		if (!endless)
		{
			return;
		}
		num = timeToAnger;
		if (!(timeToAnger > 0f))
		{
			float num2 = angerRate + baldiAnger;
			float num3 = (timeToAnger = angerFrequency);
			baldiAnger = num2;
			float num4 = num3;
			if (0.5f > num2)
			{
				baldiAnger = 0.5f;
			}
			float num5 = angerRateRate + angerRate;
			baldiWait = baldiAnger * -3f / (2f / baldiSpeedScale + baldiAnger) + 3f;
			angerRate = num5;
		}
		else
		{
			float deltaTime5 = Time.deltaTime;
			timeToAnger = num - deltaTime5;
			num = deltaTime5;
		}
	}

	// Token: 0x060000EA RID: 234 RVA: 0x0023F010 File Offset: 0x0023D610
	private void FixedUpdate()
	{
		float num = moveFrames;
		NavMeshAgent navMeshAgent = agent;
		float num2 = default(float);
		if (moveFrames > 0f)
		{
			moveFrames = num - 1f;
			num -= 1f;
			num2 = speed;
		}
		navMeshAgent.speed = num2;
		Transform transform = player;
		Vector3 position = transform.position;
		Transform transform2 = this.transform;
		Transform transform3 = transform2;
		float x = position.x;
		float y = position.y;
		float z = position.z;
		Vector3 position2 = transform3.position;
		Transform transform4 = this.transform;
		transform3 = transform4;
	}

	// Token: 0x060000EB RID: 235 RVA: 0x0023F870 File Offset: 0x0023DE70
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
		currentPriority = 0f;
	}

	// Token: 0x060000EC RID: 236 RVA: 0x0023F690 File Offset: 0x0023DC90
	public void TargetPlayer()
	{
		NavMeshAgent navMeshAgent = agent;
		Vector3 position = player.position;
		Vector3 destination = default(Vector3);
		destination.x = position.x;
		destination.y = position.y;
		destination.z = position.z;
		bool flag = navMeshAgent.SetDestination(destination);
		coolDown = 1f;
		currentPriority = 0f;
	}

	// Token: 0x060000ED RID: 237 RVA: 0x0023F410 File Offset: 0x0023DA10
	private void Move()
	{
		Transform transform = this.transform;
		Vector3 position = transform.position;
		float num = (position.x - previous.x) * (position.x - previous.x);
		float num2 = (position.y - previous.y) * (position.y - previous.y) + (position.x - previous.x) * (position.x - previous.x) + (position.z - previous.z) * (position.z - previous.z);
		if ((0f > coolDown) & (9.9999994E-11f > (position.y - previous.y) * (position.y - previous.y) + (position.x - previous.x) * (position.x - previous.x) + (position.z - previous.z) * (position.z - previous.z)))
		{
			Wander();
		}
		float num3 = baldiWait - baldiTempAnger;
		moveFrames = 10f;
		timeToMove = num3;
		Transform transform2 = this.transform;
		Transform transform3 = transform2;
		Vector3 position2 = transform3.position;
		previous.x = position2.x;
		previous.y = position2.y;
		previous.z = position2.z;
		long num4 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
		baldiAudio.PlayOneShot(slap);
		baldiAnimator.SetTrigger("slap");
		if (rumble)
		{
			Transform transform4 = this.transform;
			transform3 = transform4;
			Vector3 position3 = transform3.position;
			Transform transform5 = player;
			Vector3 position4 = transform5.position;
		}
	}

	// Token: 0x060000EE RID: 238 RVA: 0x0023F350 File Offset: 0x0023D950
	public void GetAngry(float value)
	{
		baldiAnger = value + baldiAnger;
		if (0.5f > value + baldiAnger)
		{
			baldiAnger = 0.5f;
		}
		float num = 2f / baldiSpeedScale + baldiAnger;
		baldiWait = baldiAnger * -3f / (2f / baldiSpeedScale + baldiAnger) + 3f;
	}

	// Token: 0x060000EF RID: 239 RVA: 0x0023F3A0 File Offset: 0x0023D9A0
	public void GetTempAngry(float value)
	{
		baldiTempAnger = value + baldiTempAnger;
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x0023F3B0 File Offset: 0x0023D9B0
	public void Hear(Vector3 soundLocation, float priority)
	{
		if (!antiHearing && !(priority < currentPriority))
		{
			NavMeshAgent navMeshAgent = agent;
			Vector3 destination = default(Vector3);
			destination.x = soundLocation.x;
			destination.y = soundLocation.y;
			destination.z = soundLocation.z;
			bool flag = navMeshAgent.SetDestination(destination);
			currentPriority = priority;
			bool flag2 = flag;
		}
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x0023EFE0 File Offset: 0x0023D5E0
	public void ActivateAntiHearing(float t)
	{
		Wander();
		antiHearingTime = t;
		antiHearing = true;
	}
}

