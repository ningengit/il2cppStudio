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
		agent = GetComponent<NavMeshAgent>();
		audioQueue = GetComponent<AudioQueueScript>();
		audioDevice = GetComponent<AudioSource>();
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
			_ = !angry;
			if (!angry && !(deltaTime + num < 0.5f))
			{
				angry = true;
				seesRuleBreak = false;
				timeSeenRuleBreak = 0f;
				TargetPlayer();
				CorrectPlayer();
			}
		}
		float num2 = coolDown;
		if (coolDown > 0f)
		{
			coolDown = num2 - Time.deltaTime;
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
		Vector3 position = player.position;
		Transform transform = this.transform;
		float y = position.y;
		float x = position.x;
		Vector3 position2 = transform.position;
		aim = new Vector3(x - position2.x, y - position2.y, position.z - position2.z);
		Transform transform2 = this.transform;
		y -= position2.y;
		_ = x - position2.x;
		Vector3 position3 = transform2.position;
		float z = aim.z;
		float z2 = position3.z;
		Vector3 vector = new Vector3(position3.x, position3.y, z2);
		Vector3 origin = vector;
		Vector3 direction = default(Vector3);
		direction.x = y;
		direction.z = z;
		bool flag = Physics.Raycast(origin, direction, out *(RaycastHit*)(NativeIntrinsics.Reinterpret<long>(this) + 288L), float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore);
		bool flag2 = ((RaycastHit*)(NativeIntrinsics.Reinterpret<long>(this) + 288L))->transform.tag == "Player";
		bool num = (!angry && ((playerScript.guilt > 0f) & !inOffice));
		_ = playerScript.guilt;
		if (!((flag2 && flag) && num))
		{
			NavMeshAgent navMeshAgent = agent;
			seesRuleBreak = false;
			Vector3 vector2 = navMeshAgent.velocity;
			if (!(1f < vector2.magnitude) & !(0f < coolDown))
			{
				playerScript.principalBugFixer = 1;
				wanderer.GetNewTarget();
				Transform transform3 = wanderTarget;
				agent.SetDestination(transform3.position);
				if (agent.isStopped)
				{
					agent.isStopped = false;
				}
				coolDown = 1f;
				if (!(1f < UnityEngine.Random.Range(0f, 10f)))
				{
					quietAudioDevice.PlayOneShot(aud_Whistle);
				}
			}
		}
		else
		{
			seesRuleBreak = true;
		}
		Vector3 position4 = bully.position;
		Transform transform4 = this.transform;
		float y2 = position4.y;
		y = position4.x;
		Vector3 position5 = transform4.position;
		aim = new Vector3(y - position5.x, y2 - position5.y, position4.z - position5.z);
		Transform transform5 = this.transform;
		y2 -= position5.y;
		_ = y - position5.x;
		Vector3 position6 = transform5.position;
		z = aim.z;
		z2 = position6.z;
		Vector3 vector3 = new Vector3(position6.x, position6.y, z2);
		Vector3 origin2 = vector3;
		Vector3 direction2 = default(Vector3);
		direction2.x = y2;
		direction2.z = z;
		bool flag3 = Physics.Raycast(origin2, direction2, out *(RaycastHit*)(NativeIntrinsics.Reinterpret<long>(this) + 288L), float.PositiveInfinity, 769);
		bool flag4 = ((RaycastHit*)(NativeIntrinsics.Reinterpret<long>(this) + 288L))->transform.name == "Its a Bully";
		BullyScript bullyScript = this.bullyScript;
		bool num2 = (!angry && ((bullyScript.guilt > 0f) & !inOffice));
		_ = bullyScript.guilt;
		if ((flag4 && flag3) && num2 && !bullySeen)
		{
			agent.SetDestination(bully.position);
			long num3 = audioQueue.audioInQueue;
			AudioQueueScript audioQueueScript = audioQueue;
			audioQueue.audioQueue[(int)num3] = audNoBullying;
			audioQueueScript.audioInQueue++;
			bullySeen = true;
		}
	}

	// Token: 0x06000128 RID: 296 RVA: 0x0024FF30 File Offset: 0x0024E530
	private void Wander()
	{
		playerScript.principalBugFixer = 1;
		wanderer.GetNewTarget();
		Transform transform = wanderTarget;
		agent.SetDestination(transform.position);
		if (agent.isStopped)
		{
			agent.isStopped = false;
		}
		coolDown = 1f;
		if (!(1f < UnityEngine.Random.Range(0f, 10f)))
		{
			quietAudioDevice.PlayOneShot(aud_Whistle);
		}
	}

	// Token: 0x06000129 RID: 297 RVA: 0x0024FE10 File Offset: 0x0024E410
	private void TargetPlayer()
	{
		agent.SetDestination(player.position);
		coolDown = 1f;
	}

	// Token: 0x0600012A RID: 298 RVA: 0x0024FD50 File Offset: 0x0024E350
	private void TargetBully()
	{
		if (!bullySeen)
		{
			agent.SetDestination(bully.position);
			long num = audioQueue.audioInQueue;
			AudioQueueScript audioQueueScript = audioQueue;
			audioQueue.audioQueue[(int)num] = audNoBullying;
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
		if (playerScript.guiltType == "faculty")
		{
			audioQueueScript = audioQueue;
			array = audioQueueScript.audioQueue;
			num = audioQueueScript.audioInQueue;
			audioClip = audNoFaculty;
		}
		else if (playerScript.guiltType == "running")
		{
			audioQueueScript = audioQueue;
			array = audioQueueScript.audioQueue;
			num = audioQueueScript.audioInQueue;
			audioClip = audNoRunning;
		}
		else if (playerScript.guiltType == "drink")
		{
			audioQueueScript = audioQueue;
			array = audioQueueScript.audioQueue;
			num = audioQueueScript.audioInQueue;
			audioClip = audNoDrinking;
		}
		else
		{
			if (playerScript.guiltType == "escape")
			{
				audioQueue.QueueAudio(audNoEscaping);
			}
			return;
		}
		array[(int)num] = audioClip;
		audioQueueScript.audioInQueue++;
	}

	// Token: 0x0600012C RID: 300 RVA: 0x0024F860 File Offset: 0x0024DE60
	private void OnTriggerStay(Collider other)
	{
		if (other.name == "Office Trigger")
		{
			inOffice = true;
		}
		bool flag = other.tag == "Player";
		if (!(angry && flag))
		{
			return;
		}
		PlayerScript playerScript = this.playerScript;
		inOffice = true;
		playerScript.principalBugFixer = 0;
		agent.Warp(principalDetentionPoint.position);
		agent.isStopped = true;
		Transform transform = other.transform;
		Vector3 position = playerDetentionPoint.position;
		Vector3 position3 = new Vector3(position.x, 4f, playerDetentionPoint.position.z);
		transform.position = position3;
		long num = audioQueue.audioInQueue;
		AudioQueueScript audioQueueScript = audioQueue;
		audioQueue.audioQueue[(int)num] = aud_Delay;
		audioQueueScript.audioInQueue++;
		num = audioQueue.audioInQueue;
		AudioQueueScript audioQueueScript2 = audioQueue;
		AudioClip audioClip2 = audTimes[detentions];
		audioQueue.audioQueue[(int)num] = audioClip2;
		audioQueueScript2.audioInQueue++;
		num = audioQueue.audioInQueue;
		AudioQueueScript audioQueueScript3 = audioQueue;
		audioQueue.audioQueue[(int)num] = audDetention;
		audioQueueScript3.audioInQueue++;
		int num2 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 2f));
		num = audioQueue.audioInQueue;
		AudioQueueScript audioQueueScript4 = audioQueue;
		audioQueue.audioQueue[(int)num] = audScolds[num2];
		audioQueueScript4.audioInQueue++;
		int num3 = lockTime[detentions];
		DoorScript doorScript = officeDoor;
		doorScript.bDoorLocked = true;
		doorScript.lockTime = num3;
		if (this.baldiScript.isActiveAndEnabled)
		{
			BaldiScript baldiScript = this.baldiScript;
			Transform transform2 = this.transform;
			BaldiScript baldiScript2 = baldiScript;
			Vector3 position4 = transform2.position;
			if (!baldiScript2.antiHearing && !(8f < baldiScript2.currentPriority))
			{
				baldiScript2.agent.SetDestination(position4);
				baldiScript2.currentPriority = 8f;
			}
		}
		int num4 = detentions + 1;
		coolDown = 5f;
		angry = false;
		detentions = num4;
		if (num4 > 4)
		{
			detentions = 4;
		}
	}

	// Token: 0x0600012D RID: 301 RVA: 0x0024F7C0 File Offset: 0x0024DDC0
	private void OnTriggerExit(Collider other)
	{
		if (other.name == "Office Trigger")
		{
			inOffice = false;
		}
		if (other.name == "Its a Bully")
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
		base..ctor();
	}
}

