using System;
using System.Runtime.CompilerServices;
using Il2CppStudio.Native;
using UnityEngine;
using UnityEngine.AI;

// Token: 0x0200003A RID: 58
public class PrincipalScript : MonoBehaviour
{
	// Token: 0x040001A0 RID: 416 Offset: 0x20
	public bool seesRuleBreak;

	// Token: 0x040001A1 RID: 417 Offset: 0x28
	public Transform player;

	// Token: 0x040001A2 RID: 418 Offset: 0x30
	public Transform bully;

	// Token: 0x040001A3 RID: 419 Offset: 0x38
	public bool bullySeen;

	// Token: 0x040001A4 RID: 420 Offset: 0x40
	public PlayerScript playerScript;

	// Token: 0x040001A5 RID: 421 Offset: 0x48
	public BullyScript bullyScript;

	// Token: 0x040001A6 RID: 422 Offset: 0x50
	public BaldiScript baldiScript;

	// Token: 0x040001A7 RID: 423 Offset: 0x58
	public Transform wanderTarget;

	// Token: 0x040001A8 RID: 424 Offset: 0x60
	public AILocationSelectorScript wanderer;

	// Token: 0x040001A9 RID: 425 Offset: 0x68
	public DoorScript officeDoor;

	// Token: 0x040001AA RID: 426 Offset: 0x70
	public Transform principalDetentionPoint;

	// Token: 0x040001AB RID: 427 Offset: 0x78
	public Transform playerDetentionPoint;

	// Token: 0x040001AC RID: 428 Offset: 0x80
	public float coolDown;

	// Token: 0x040001AD RID: 429 Offset: 0x84
	public float timeSeenRuleBreak;

	// Token: 0x040001AE RID: 430 Offset: 0x88
	public bool angry;

	// Token: 0x040001AF RID: 431 Offset: 0x89
	public bool inOffice;

	// Token: 0x040001B0 RID: 432 Offset: 0x8C
	private int detentions;

	// Token: 0x040001B1 RID: 433 Offset: 0x90
	private int[] lockTime;

	// Token: 0x040001B2 RID: 434 Offset: 0x98
	public AudioClip[] audTimes;

	// Token: 0x040001B3 RID: 435 Offset: 0xA0
	public AudioClip[] audScolds;

	// Token: 0x040001B4 RID: 436 Offset: 0xA8
	public AudioClip audDetention;

	// Token: 0x040001B5 RID: 437 Offset: 0xB0
	public AudioClip audNoDrinking;

	// Token: 0x040001B6 RID: 438 Offset: 0xB8
	public AudioClip audNoEating;

	// Token: 0x040001B7 RID: 439 Offset: 0xC0
	public AudioClip audNoBullying;

	// Token: 0x040001B8 RID: 440 Offset: 0xC8
	public AudioClip audNoFaculty;

	// Token: 0x040001B9 RID: 441 Offset: 0xD0
	public AudioClip audNoLockers;

	// Token: 0x040001BA RID: 442 Offset: 0xD8
	public AudioClip audNoRunning;

	// Token: 0x040001BB RID: 443 Offset: 0xE0
	public AudioClip audNoStabbing;

	// Token: 0x040001BC RID: 444 Offset: 0xE8
	public AudioClip audNoEscaping;

	// Token: 0x040001BD RID: 445 Offset: 0xF0
	public AudioClip aud_Whistle;

	// Token: 0x040001BE RID: 446 Offset: 0xF8
	public AudioClip aud_Delay;

	// Token: 0x040001BF RID: 447 Offset: 0x100
	private NavMeshAgent agent;

	// Token: 0x040001C0 RID: 448 Offset: 0x108
	private AudioQueueScript audioQueue;

	// Token: 0x040001C1 RID: 449 Offset: 0x110
	private AudioSource audioDevice;

	// Token: 0x040001C2 RID: 450 Offset: 0x118
	public AudioSource quietAudioDevice;

	// Token: 0x040001C3 RID: 451 Offset: 0x120
	private RaycastHit hit;

	// Token: 0x040001C4 RID: 452 Offset: 0x14C
	private Vector3 aim;

	// Token: 0x040001C5 RID: 453 Offset: 0x158
	public CharacterController cc;

	// Token: 0x06000125 RID: 293 RVA: 0x0024FC90 File Offset: 0x0024E290
	private void Start()
	{
		NavMeshAgent component = GetComponent<NavMeshAgent>();
		agent = component;
		AudioQueueScript component2 = GetComponent<AudioQueueScript>();
		audioQueue = component2;
		AudioSource component3 = GetComponent<AudioSource>();
		audioDevice = component3;
	}

	// Token: 0x06000126 RID: 294 RVA: 0x0024FE80 File Offset: 0x0024E480
	private void Update()
	{
		if (!seesRuleBreak)
		{
			timeSeenRuleBreak = 0f;
		}
		else
		{
			float num = timeSeenRuleBreak;
			float deltaTime = Time.deltaTime;
			timeSeenRuleBreak = deltaTime + num;
			int num2 = ((!angry) ? 1 : 0);
			float num3 = deltaTime + num;
			if (!angry && !(deltaTime + num < 0.5f))
			{
				angry = true;
				seesRuleBreak = false;
				timeSeenRuleBreak = 0f;
				TargetPlayer();
				CorrectPlayer();
			}
		}
		float num4 = coolDown;
		if (coolDown > 0f)
		{
			float deltaTime2 = Time.deltaTime;
			coolDown = num4 - deltaTime2;
			float num3 = deltaTime2;
		}
	}

	// Token: 0x06000127 RID: 295 RVA: 0x0024F210 File Offset: 0x0024D810
	private unsafe void FixedUpdate()
	{
		if (angry)
		{
			TargetPlayer();
			return;
		}
		Transform transform = player;
		Vector3 position = transform.position;
		Transform transform2 = this.transform;
		float y = position.y;
		float x = position.x;
		Vector3 position2 = transform2.position;
		aim = NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("unpcklps xmm6,xmm7"));
		aim.z = position.z - position2.z;
		Transform transform3 = this.transform;
		y -= position2.y;
		long num = NativeIntrinsics.Asm("unpcklps xmm6,xmm7");
		Vector3 position3 = transform3.position;
		long num2 = NativeIntrinsics.Asm("struct_chunk(FieldExpr+0x0, 8)");
		float z = aim.z;
		num = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
		float z2 = position3.z;
		bool flag = Physics.Raycast(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + 7L), NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -9L), out *(RaycastHit*)(NativeIntrinsics.Reinterpret<long>(this) + 288L), float.PositiveInfinity, 769, NativeIntrinsics.Reinterpret<QueryTriggerInteraction>(1));
		Transform transform4 = ((RaycastHit*)(NativeIntrinsics.Reinterpret<long>(this) + 288L))->transform;
		string a = transform4.tag;
		bool flag2 = string.Equals(a, "Player");
		int num3 = ((!angry && ((playerScript.guilt > 0f) & !inOffice)) ? 1 : 0);
		float guilt = playerScript.guilt;
		if ((((flag2 && flag) ? 1u : 0u) & (uint)num3) == 0)
		{
			NavMeshAgent navMeshAgent = agent;
			seesRuleBreak = false;
			Vector3 velocity = navMeshAgent.velocity;
			float magnitude = ((Vector3*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + 7L))->magnitude;
			guilt = magnitude;
			if (!(1f < magnitude) & !(0f < coolDown))
			{
				playerScript.principalBugFixer = 1;
				AILocationSelectorScript aILocationSelectorScript = wanderer;
				aILocationSelectorScript.GetNewTarget();
				transform = wanderTarget;
				NavMeshAgent navMeshAgent2 = agent;
				Vector3 position4 = transform.position;
				bool flag3 = navMeshAgent2.SetDestination(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + 7L));
				NavMeshAgent navMeshAgent3 = agent;
				if (navMeshAgent3.isStopped)
				{
					navMeshAgent3 = agent;
					navMeshAgent3.isStopped = false;
				}
				coolDown = 1f;
				float num4 = UnityEngine.Random.Range(0f, 10f);
				guilt = num4;
				if (!(1f < num4))
				{
					quietAudioDevice.PlayOneShot(aud_Whistle);
				}
			}
		}
		else
		{
			seesRuleBreak = true;
		}
		transform = bully;
		Vector3 position5 = transform.position;
		Transform transform5 = this.transform;
		float y2 = position5.y;
		float x2 = position5.x;
		Vector3 position6 = transform5.position;
		aim = NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("unpcklps xmm7,xmm8"));
		aim.z = position5.z - position6.z;
		Transform transform6 = this.transform;
		y2 -= position6.y;
		long num5 = NativeIntrinsics.Asm("unpcklps xmm7,xmm8");
		Vector3 position7 = transform6.position;
		long num6 = NativeIntrinsics.Asm("struct_chunk(FieldExpr+0x0, 8)");
		z = aim.z;
		num5 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
		z2 = position7.z;
		bool flag4 = Physics.Raycast(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -9L), NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + 7L), out *(RaycastHit*)(NativeIntrinsics.Reinterpret<long>(this) + 288L), float.PositiveInfinity, 769);
		Transform transform7 = ((RaycastHit*)(NativeIntrinsics.Reinterpret<long>(this) + 288L))->transform;
		Transform transform8 = transform7;
		string a2 = transform8.name;
		bool flag5 = string.Equals(a2, "Its a Bully");
		BullyScript bullyScript = this.bullyScript;
		int num7 = ((!angry && ((bullyScript.guilt > 0f) & !inOffice)) ? 1 : 0);
		int num8 = num7;
		guilt = bullyScript.guilt;
		if ((((flag5 && flag4) ? 1u : 0u) & (uint)num7) != 0 && !bullySeen)
		{
			NavMeshAgent navMeshAgent4 = agent;
			Vector3 position8 = bully.position;
			bool flag6 = navMeshAgent4.SetDestination(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + 7L));
			bool flag7 = flag6;
			long num9 = audioQueue.audioInQueue;
			AudioQueueScript audioQueueScript = audioQueue;
			AudioClip[] array = audioQueue.audioQueue;
			AudioClip audioClip = audNoBullying;
			array[(int)num9] = audioClip;
			audioQueueScript.audioInQueue++;
			bullySeen = true;
		}
	}

	// Token: 0x06000128 RID: 296 RVA: 0x0024FF30 File Offset: 0x0024E530
	private void Wander()
	{
		playerScript.principalBugFixer = 1;
		AILocationSelectorScript aILocationSelectorScript = wanderer;
		aILocationSelectorScript.GetNewTarget();
		Transform transform = wanderTarget;
		NavMeshAgent navMeshAgent = agent;
		Vector3 position = transform.position;
		Vector3 destination = default(Vector3);
		destination.x = position.x;
		destination.y = position.y;
		destination.z = position.z;
		bool flag = navMeshAgent.SetDestination(destination);
		NavMeshAgent navMeshAgent2 = agent;
		if (navMeshAgent2.isStopped)
		{
			navMeshAgent2 = agent;
			navMeshAgent2.isStopped = false;
		}
		coolDown = 1f;
		float num = UnityEngine.Random.Range(0f, 10f);
		float num2 = num;
		if (!(1f < num))
		{
			quietAudioDevice.PlayOneShot(aud_Whistle);
		}
	}

	// Token: 0x06000129 RID: 297 RVA: 0x0024FE10 File Offset: 0x0024E410
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

	// Token: 0x0600012A RID: 298 RVA: 0x0024FD50 File Offset: 0x0024E350
	private void TargetBully()
	{
		if (!bullySeen)
		{
			NavMeshAgent navMeshAgent = agent;
			Vector3 position = bully.position;
			Vector3 destination = default(Vector3);
			destination.x = position.x;
			destination.y = position.y;
			destination.z = position.z;
			bool flag = navMeshAgent.SetDestination(destination);
			bool flag2 = flag;
			long num = audioQueue.audioInQueue;
			AudioQueueScript audioQueueScript = audioQueue;
			AudioClip[] array = audioQueue.audioQueue;
			AudioClip audioClip = audNoBullying;
			array[(int)num] = audioClip;
			audioQueueScript.audioInQueue++;
			bullySeen = true;
		}
	}

	// Token: 0x0600012B RID: 299 RVA: 0x0024F030 File Offset: 0x0024D630
	private void CorrectPlayer()
	{
		audioQueue.audioInQueue = 0;
		AudioQueueScript audioQueueScript;
		AudioClip[] array;
		long num;
		AudioClip audioClip;
		if (!string.Equals(playerScript.guiltType, "faculty"))
		{
			if (!string.Equals(playerScript.guiltType, "running"))
			{
				if (!string.Equals(playerScript.guiltType, "drink"))
				{
					bool flag = string.Equals(playerScript.guiltType, "escape");
					bool flag2 = flag;
					if (flag)
					{
						audioQueue.QueueAudio(audNoEscaping);
					}
					return;
				}
				audioQueueScript = audioQueue;
				array = audioQueueScript.audioQueue;
				num = audioQueueScript.audioInQueue;
				audioClip = audNoDrinking;
			}
			else
			{
				audioQueueScript = audioQueue;
				array = audioQueueScript.audioQueue;
				num = audioQueueScript.audioInQueue;
				audioClip = audNoRunning;
			}
		}
		else
		{
			audioQueueScript = audioQueue;
			array = audioQueueScript.audioQueue;
			num = audioQueueScript.audioInQueue;
			audioClip = audNoFaculty;
		}
		array[(int)num] = audioClip;
		audioQueueScript.audioInQueue++;
	}

	// Token: 0x0600012C RID: 300 RVA: 0x0024F860 File Offset: 0x0024DE60
	private void OnTriggerStay(Collider other)
	{
		string a = other.name;
		if (string.Equals(a, "Office Trigger"))
		{
			inOffice = true;
		}
		string a2 = other.tag;
		bool flag = string.Equals(a2, "Player");
		bool flag2 = flag;
		if (!(angry && flag))
		{
			return;
		}
		PlayerScript playerScript = this.playerScript;
		inOffice = true;
		playerScript.principalBugFixer = 0;
		NavMeshAgent navMeshAgent = agent;
		Vector3 position = principalDetentionPoint.position;
		Vector3 newPosition = default(Vector3);
		newPosition.x = position.x;
		newPosition.y = position.y;
		newPosition.z = position.z;
		bool flag3 = navMeshAgent.Warp(newPosition);
		agent.isStopped = true;
		Transform transform = other.transform;
		Vector3 position2 = playerDetentionPoint.position;
		Vector3 position3 = playerDetentionPoint.position;
		Vector3 position4 = default(Vector3);
		position4.x = position2.x;
		position4.y = 4f;
		position4.z = position3.z;
		transform.position = position4;
		long num = audioQueue.audioInQueue;
		AudioQueueScript audioQueueScript = audioQueue;
		AudioClip audioClip = aud_Delay;
		AudioClip[] array = audioQueue.audioQueue;
		array[(int)num] = audioClip;
		audioQueueScript.audioInQueue++;
		num = audioQueue.audioInQueue;
		AudioQueueScript audioQueueScript2 = audioQueue;
		AudioClip audioClip2 = audTimes[detentions];
		AudioClip[] array2 = audioQueue.audioQueue;
		array2[(int)num] = audioClip2;
		audioQueueScript2.audioInQueue++;
		num = audioQueue.audioInQueue;
		AudioQueueScript audioQueueScript3 = audioQueue;
		AudioClip audioClip3 = audDetention;
		AudioClip[] array3 = audioQueue.audioQueue;
		array3[(int)num] = audioClip3;
		audioQueueScript3.audioInQueue++;
		float f = UnityEngine.Random.Range(0f, 2f);
		int num2 = Mathf.RoundToInt(f);
		num = audioQueue.audioInQueue;
		AudioQueueScript audioQueueScript4 = audioQueue;
		AudioClip[] array4 = audioQueue.audioQueue;
		AudioClip audioClip4 = audScolds[num2];
		array4[(int)num] = audioClip4;
		audioQueueScript4.audioInQueue++;
		int num3 = lockTime[detentions];
		DoorScript doorScript = officeDoor;
		doorScript.bDoorLocked = true;
		doorScript.lockTime = num3;
		float num4 = num3;
		if (this.baldiScript.isActiveAndEnabled)
		{
			BaldiScript baldiScript = this.baldiScript;
			Transform transform2 = this.transform;
			BaldiScript baldiScript2 = baldiScript;
			Transform transform3 = transform2;
			Vector3 position5 = transform3.position;
			if (!baldiScript2.antiHearing && !(8f < baldiScript2.currentPriority))
			{
				NavMeshAgent navMeshAgent2 = baldiScript2.agent;
				Vector3 destination = default(Vector3);
				destination.x = position5.x;
				destination.y = position5.y;
				destination.z = position5.z;
				bool flag4 = navMeshAgent2.SetDestination(destination);
				baldiScript2.currentPriority = 8f;
			}
		}
		int num5 = detentions + 1;
		coolDown = 5f;
		angry = false;
		detentions = num5;
		int num6 = num5;
		if (num5 > 4)
		{
			detentions = 4;
		}
	}

	// Token: 0x0600012D RID: 301 RVA: 0x0024F7C0 File Offset: 0x0024DDC0
	private void OnTriggerExit(Collider other)
	{
		string a = other.name;
		if (string.Equals(a, "Office Trigger"))
		{
			inOffice = false;
		}
		string a2 = other.name;
		bool flag = string.Equals(a2, "Its a Bully");
		bool flag2 = flag;
		if (flag)
		{
			bullySeen = false;
		}
	}

	// Token: 0x0600012E RID: 302 RVA: 0x00250040 File Offset: 0x0024E640
	public PrincipalScript()
	{
		int[] array = new int[5];
		RuntimeHelpers.InitializeArray(array, NativeIntrinsics.Reinterpret<RuntimeFieldHandle>((nint)NativeIntrinsics.Asm("<PrivateImplementationDetails>::6BE9D134EF6D584CC78B34C0D3A332D6443042F2AA77BB4B36BD6E2F86CA7069_FieldInfo")));
		lockTime = array;
		audTimes = new AudioClip[5];
		audScolds = new AudioClip[3];
		((Behaviour)this)..ctor();
	}
}

