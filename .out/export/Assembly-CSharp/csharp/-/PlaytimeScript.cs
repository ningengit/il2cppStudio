using Il2CppStudio.Native;
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
		NavMeshAgent component = GetComponent<NavMeshAgent>();
		agent = component;
		AudioSource component2 = GetComponent<AudioSource>();
		audioDevice = component2;
		Wander();
	}

	// Token: 0x0600011F RID: 287 RVA: 0x0024EDD0 File Offset: 0x0024D3D0
	private void Update()
	{
		float num = coolDown;
		if (coolDown > 0f)
		{
			float deltaTime = Time.deltaTime;
			coolDown = num - deltaTime;
		}
		num = playCool;
		if (playCool < 0f)
		{
			bool flag = this.animator.GetBool("disappointed");
			bool flag2 = flag;
			if (flag)
			{
				Animator animator = this.animator;
				playCool = 0f;
				animator.SetBool("disappointed", value: false);
			}
		}
		else
		{
			float deltaTime2 = Time.deltaTime;
			playCool = num - deltaTime2;
			num = deltaTime2;
		}
	}

	// Token: 0x06000120 RID: 288 RVA: 0x0024E780 File Offset: 0x0024CD80
	private unsafe void FixedUpdate()
	{
		PlayerScript playerScript = ps;
		float num;
		if (playerScript.jumpRope)
		{
			if (!jumpRopeStarted)
			{
				NavMeshAgent navMeshAgent = agent;
				Transform transform = this.transform;
				Vector3 position = transform.position;
				Transform transform2 = this.transform;
				num = transform2.forward.y * 10f;
				bool flag = navMeshAgent.Warp(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -57L));
			}
			NavMeshAgent navMeshAgent2 = agent;
			jumpRopeStarted = true;
			navMeshAgent2.speed = 0f;
			playCool = 15f;
			return;
		}
		Transform transform3 = player;
		Vector3 position2 = transform3.position;
		Transform transform4 = this.transform;
		float x = position2.x;
		float y = position2.y;
		float z = position2.z;
		Vector3 position3 = transform4.position;
		Transform transform5 = this.transform;
		x -= position3.x;
		y -= position3.y;
		z -= position3.z;
		Vector3 position4 = transform5.position;
		long num2 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
		float z2 = position4.z;
		bool flag2 = Physics.Raycast(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -41L), NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -57L), out *(RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -25L), float.PositiveInfinity, 769, NativeIntrinsics.Reinterpret<QueryTriggerInteraction>(1));
		Transform transform6 = ((RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -25L))->transform;
		Transform transform7 = transform6;
		long num3 = NativeIntrinsics.Asm("unpcklps xmm6,xmm7");
		string a = transform7.tag;
		bool flag3 = string.Equals(a, "Player");
		Transform transform8 = this.transform;
		transform7 = transform8;
		Vector3 position5 = transform7.position;
		transform3 = player;
		Vector3 position6 = transform3.position;
		float magnitude = ((Vector3*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -57L))->magnitude;
		int num4 = ((!(80f < magnitude) && flag3 && flag2) ? 1 : 0);
		num = magnitude;
		int num5 = ((!(0f < playCool)) ? 1 : 0);
		int num6 = default(int);
		if (((uint)num4 & ((!(0f < playCool)) ? 1u : 0u)) == 0)
		{
			if (playerSeen & !(0f < coolDown))
			{
				playerSeen = (byte)num6 != 0;
				Wander();
				jumpRopeStarted = false;
				return;
			}
			Vector3 velocity = agent.velocity;
			float magnitude2 = ((Vector3*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -57L))->magnitude;
			num = magnitude2;
			num6 = ((!(1f < magnitude2)) ? 1 : 0);
			if (!(1f < magnitude2) & !(0f < coolDown))
			{
				Wander();
				jumpRopeStarted = false;
				return;
			}
		}
		else
		{
			playerSeen = true;
			if (NativeIntrinsics.Asm("global_19E3B6E") == num6)
			{
				NativeIntrinsics.Stmt("mov byte ptr [1819E3B6Eh],1 ; value = 0x1");
			}
			animator.SetBool("disappointed", value: false);
			NavMeshAgent navMeshAgent3 = agent;
			Vector3 position7 = player.position;
			bool flag4 = navMeshAgent3.SetDestination(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -41L));
			bool flag5 = flag4;
			agent.speed = 20f;
			coolDown = 0.2f;
			if (!playerSpotted)
			{
				AudioSource audioSource = audioDevice;
				playerSpotted = true;
				audioSource.PlayOneShot(aud_LetsPlay);
			}
		}
		jumpRopeStarted = false;
	}

	// Token: 0x06000121 RID: 289 RVA: 0x0024EEA0 File Offset: 0x0024D4A0
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
		bool flag2 = flag;
		agent.speed = 15f;
		playerSpotted = false;
		float f = Random.Range(0f, 1f);
		int num = Mathf.RoundToInt(f);
		AudioSource audioSource = audioDevice;
		audVal = num;
		int num2 = num;
		bool isPlaying = audioSource.isPlaying;
		bool flag3 = isPlaying;
		if (!isPlaying)
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
		NavMeshAgent navMeshAgent = agent;
		Vector3 position = player.position;
		Vector3 destination = default(Vector3);
		destination.x = position.x;
		destination.y = position.y;
		destination.z = position.z;
		bool flag = navMeshAgent.SetDestination(destination);
		bool flag2 = flag;
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

