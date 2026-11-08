using Il2CppStudio.Native;
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
		NavMeshAgent component = GetComponent<NavMeshAgent>();
		agent = component;
		coolDown = 1f;
		Wander();
	}

	// Token: 0x0600010B RID: 267 RVA: 0x002452B0 File Offset: 0x002438B0
	private unsafe void Update()
	{
		float num = coolDown;
		long num2 = NativeIntrinsics.Asm("&stack[0x0]") + -95L;
		float num3 = num;
		if (num > 0f)
		{
			float deltaTime = Time.deltaTime;
			coolDown = num3 - deltaTime;
		}
		num3 = autoBrakeCool;
		if (!(autoBrakeCool > 0f))
		{
			agent.autoBraking = true;
		}
		else
		{
			float deltaTime2 = Time.deltaTime;
			autoBrakeCool = num3 - deltaTime2;
			num3 = deltaTime2;
		}
		Transform transform = this.transform;
		Vector3 eulerAngles = transform.eulerAngles;
		NavMeshAgent navMeshAgent = agent;
		Vector3 steeringTarget = navMeshAgent.steeringTarget;
		Transform transform2 = this.transform;
		float x = steeringTarget.x;
		Vector3 position = transform2.position;
		navMeshAgent = agent;
		Vector3 steeringTarget2 = navMeshAgent.steeringTarget;
		Transform transform3 = this.transform;
		float z = steeringTarget2.z;
		Vector3 position2 = transform3.position;
		long num4 = NativeIntrinsics.CallNative("sub_20BC10", 0L, 0L, 0L, 0L);
		long num5 = NativeIntrinsics.CallNative("sub_20C2B0", 0L, 0L, 0L, 0L);
		num3 = (float)NativeIntrinsics.Asm("xmm0") * 360f;
		z = (float)NativeIntrinsics.Asm("xmm0") * 57.29578f - eulerAngles.y - (float)NativeIntrinsics.Asm("xmm0") * 360f;
		if (0f > (float)NativeIntrinsics.Asm("xmm0") * 57.29578f - eulerAngles.y - (float)NativeIntrinsics.Asm("xmm0") * 360f || z > 360f)
		{
		}
		if (0f > 180f)
		{
			z = 0f - 360f;
		}
		angleDiff = z;
		if (0f < crazyTime)
		{
			agent.speed = 0f;
			Transform transform4 = this.transform;
			float deltaTime3 = Time.deltaTime;
			num3 = deltaTime3 * 180f;
			NativeIntrinsics.Stmt("movss dword ptr [rbp-15h],xmm0 ; value = LocalExpr");
			NativeIntrinsics.Stmt("mov dword ptr [rbp-19h],0 ; value = 0x0");
			NativeIntrinsics.Stmt("mov dword ptr [rbp-11h],0 ; value = 0x0");
			transform4.Rotate(NativeIntrinsics.Reinterpret<Vector3>(num2 + -25L));
			float num6 = crazyTime;
			float deltaTime4 = Time.deltaTime;
			crazyTime = num6 - deltaTime4;
			num3 = deltaTime4;
		}
		else
		{
			NavMeshAgent navMeshAgent2 = default(NavMeshAgent);
			float num8 = default(float);
			if (!(5f > NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("andps xmm6,[1813EAC20h]"))))
			{
				Transform transform5 = this.transform;
				num3 = angleDiff;
				float num7 = turnSpeed;
				Transform transform6 = transform5;
				if (!(angleDiff < 0f))
				{
				}
				float deltaTime5 = Time.deltaTime;
				num3 = deltaTime5 * (-1f * num7);
				NativeIntrinsics.Stmt("movss dword ptr [rbp-15h],xmm0 ; value = LocalExpr");
				NativeIntrinsics.Stmt("mov dword ptr [rbp-19h],0 ; value = 0x0");
				NativeIntrinsics.Stmt("mov dword ptr [rbp-11h],0 ; value = 0x0");
				transform6.Rotate(NativeIntrinsics.Reinterpret<Vector3>(num2 + -25L));
				navMeshAgent2 = agent;
			}
			else
			{
				Transform transform7 = navMeshAgent2.transform;
				navMeshAgent = agent;
				Vector3 steeringTarget3 = navMeshAgent.steeringTarget;
				Transform transform8 = this.transform;
				Vector3 position3 = transform8.position;
				navMeshAgent = agent;
				Vector3 steeringTarget4 = navMeshAgent.steeringTarget;
				NativeIntrinsics.Stmt("movss dword ptr [rbp-11h],xmm0 ; value = FieldExpr");
				NativeIntrinsics.Stmt("movss dword ptr [rbp-19h],xmm7 ; value = FieldExpr");
				NativeIntrinsics.Stmt("movss dword ptr [rbp-15h],xmm6 ; value = FieldExpr");
				transform7.LookAt(NativeIntrinsics.Reinterpret<Vector3>(num2 + -25L));
				navMeshAgent2 = agent;
				num8 = currentSpeed;
			}
			navMeshAgent2.speed = num8;
		}
		AudioSource source = motorAudio;
		Vector3 velocity = agent.velocity;
		NativeIntrinsics.Stmt("movsd qword ptr [rbp-19h],xmm0 ; value = StructChunkExpr");
		NativeIntrinsics.Stmt("mov [rbp-11h],eax ; value = FieldExpr");
		float magnitude = ((Vector3*)(num2 + -25L))->magnitude;
		float timeScale = Time.timeScale;
		AudioSource.SetPitch(source, (magnitude + 1f) * timeScale);
	}

	// Token: 0x0600010C RID: 268 RVA: 0x00244C10 File Offset: 0x00243210
	private unsafe void FixedUpdate()
	{
		Transform transform = player;
		Transform transform2 = transform;
		Vector3 position = transform2.position;
		Transform transform3 = this.transform;
		float x = position.x;
		float y = position.y;
		float z = position.z;
		Vector3 position2 = transform3.position;
		Transform transform4 = this.transform;
		x -= position2.x;
		y -= position2.y;
		z -= position2.z;
		Vector3 position3 = transform4.position;
		long num = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
		float z2 = position3.z;
		bool flag = Physics.Raycast(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -73L), NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -89L), out *(RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -57L), float.PositiveInfinity, 769, NativeIntrinsics.Reinterpret<QueryTriggerInteraction>(1));
		Transform transform5 = ((RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -57L))->transform;
		string a = transform5.tag;
		bool flag2 = string.Equals(a, "Player");
		int num2 = (playerSeen ? 1 : 0);
		AudioSource audioSource = default(AudioSource);
		if (!(flag && flag2))
		{
			currentSpeed = normSpeed;
			if (((uint)num2 & ((!(0f < coolDown)) ? 1u : 0u)) == 0)
			{
				Vector3 velocity = agent.velocity;
				float magnitude = ((Vector3*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -89L))->magnitude;
				float num3 = coolDown;
				Transform transform6 = this.transform;
				Transform transform7 = transform6;
				Vector3 position4 = transform7.position;
				Vector3 destination = agent.destination;
				float magnitude2 = ((Vector3*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -89L))->magnitude;
				int num4 = ((!(1f < magnitude) && 5f > magnitude2 && !(0f < num3)) ? 1 : 0);
				float num5 = magnitude2;
				if (num4 == 0)
				{
					return;
				}
			}
			else
			{
				audioSource = audioDevice;
				if (!audioSource.isPlaying)
				{
					float f = Random.Range(0f, 1f);
					int num6 = Mathf.RoundToInt(f);
					AudioClip[] array = aud_Lost;
					audioDevice.PlayOneShot(array[num6]);
				}
				playerSeen = false;
			}
			Wander();
			return;
		}
		if ((object)audioSource == null)
		{
			audioSource = audioDevice;
			if (!audioSource.isPlaying)
			{
				float f2 = Random.Range(0f, 1f);
				int num7 = Mathf.RoundToInt(f2);
				audioDevice.PlayOneShot(aud_Found[num7]);
			}
		}
		Transform transform8 = player;
		playerSeen = true;
		NavMeshAgent navMeshAgent = agent;
		Vector3 position5 = transform8.position;
		bool flag3 = navMeshAgent.SetDestination(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -73L));
		float num8 = (currentSpeed = runSpeed);
		coolDown = 0.5f;
		float num9 = num8;
	}

	// Token: 0x0600010D RID: 269 RVA: 0x002456E0 File Offset: 0x00243CE0
	private void Wander()
	{
		AILocationSelectorScript aILocationSelectorScript = wanderer;
		aILocationSelectorScript.GetNewTargetHallway();
		Transform transform = wanderTarget;
		NavMeshAgent navMeshAgent = agent;
		Vector3 position = transform.position;
		Vector3 destination = default(Vector3);
		destination.x = position.x;
		destination.y = position.y;
		destination.z = position.z;
		bool flag = navMeshAgent.SetDestination(destination);
		hugAnnounced = false;
		float f = Random.Range(0f, 9f);
		int num = Mathf.RoundToInt(f);
		int num2 = num;
		AudioSource audioSource = audioDevice;
		bool isPlaying = audioSource.isPlaying;
		int num3 = ((!(0f < coolDown)) ? ((isPlaying ? 1 : 0) ^ 1) : 0);
		num2 = ((num == 0) ? 1 : 0);
		int num4 = num3;
		if ((((num == 0) ? 1u : 0u) & (uint)num3) != 0)
		{
			float f2 = Random.Range(0f, 1f);
			int num5 = Mathf.RoundToInt(f2);
			num2 = num5;
			AudioSource audioSource2 = audioDevice;
			audioSource2.PlayOneShot(aud_Random[num5]);
		}
		coolDown = 1f;
	}

	// Token: 0x0600010E RID: 270 RVA: 0x00245240 File Offset: 0x00243840
	private void TargetPlayer()
	{
		NavMeshAgent navMeshAgent = agent;
		Vector3 position = player.position;
		Vector3 destination = default(Vector3);
		destination.x = position.x;
		destination.y = position.y;
		destination.z = position.z;
		bool flag = navMeshAgent.SetDestination(destination);
		coolDown = 0.5f;
	}

	// Token: 0x0600010F RID: 271 RVA: 0x00245070 File Offset: 0x00243670
	private void OnTriggerEnter(Collider other)
	{
		string a = other.tag;
		bool flag = string.Equals(a, "Player");
		bool flag2 = flag;
		if (flag)
		{
			AudioSource audioSource = audioDevice;
			bool isPlaying = audioSource.isPlaying;
			int num = ((!hugAnnounced) ? ((isPlaying ? 1 : 0) ^ 1) : 0);
			int num2 = (isPlaying ? 1 : 0) ^ 1;
			if (num != 0)
			{
				float f = Random.Range(0f, 1f);
				int num3 = Mathf.RoundToInt(f);
				AudioClip[] array = aud_Hug;
				audioDevice.PlayOneShot(array[num3]);
				hugAnnounced = true;
			}
			agent.autoBraking = false;
		}
	}

	// Token: 0x06000110 RID: 272 RVA: 0x00245170 File Offset: 0x00243770
	private void OnTriggerExit(Collider other)
	{
		string a = other.tag;
		bool flag = string.Equals(a, "Player");
		bool flag2 = flag;
		if (flag)
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

