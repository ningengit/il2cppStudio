using Il2CppStudio.Native;
using UnityEngine;

// Token: 0x02000041 RID: 65
public class DoorScript : MonoBehaviour
{
	// Token: 0x040001E4 RID: 484 Offset: 0x20
	public float openingDistance;

	// Token: 0x040001E5 RID: 485 Offset: 0x28
	public Transform player;

	// Token: 0x040001E6 RID: 486 Offset: 0x30
	public BaldiScript baldi;

	// Token: 0x040001E7 RID: 487 Offset: 0x38
	public MeshCollider barrier;

	// Token: 0x040001E8 RID: 488 Offset: 0x40
	public MeshCollider trigger;

	// Token: 0x040001E9 RID: 489 Offset: 0x48
	public MeshCollider invisibleBarrier;

	// Token: 0x040001EA RID: 490 Offset: 0x50
	public MeshRenderer inside;

	// Token: 0x040001EB RID: 491 Offset: 0x58
	public MeshRenderer outside;

	// Token: 0x040001EC RID: 492 Offset: 0x60
	public AudioClip doorOpen;

	// Token: 0x040001ED RID: 493 Offset: 0x68
	public AudioClip doorClose;

	// Token: 0x040001EE RID: 494 Offset: 0x70
	public Material closed;

	// Token: 0x040001EF RID: 495 Offset: 0x78
	public Material open;

	// Token: 0x040001F0 RID: 496 Offset: 0x80
	public Material closedOutside;

	// Token: 0x040001F1 RID: 497 Offset: 0x88
	public Material openOutside;

	// Token: 0x040001F2 RID: 498 Offset: 0x90
	private bool bDoorOpen;

	// Token: 0x040001F3 RID: 499 Offset: 0x91
	private bool bDoorLocked;

	// Token: 0x040001F4 RID: 500 Offset: 0x94
	public int silentOpens;

	// Token: 0x040001F5 RID: 501 Offset: 0x98
	private float openTime;

	// Token: 0x040001F6 RID: 502 Offset: 0x9C
	public float lockTime;

	// Token: 0x040001F7 RID: 503 Offset: 0xA0
	private AudioSource myAudio;

	public bool DoorLocked => bDoorLocked;

	// Token: 0x06000145 RID: 325 RVA: 0x002441F0 File Offset: 0x002427F0
	private void Start()
	{
		AudioSource component = GetComponent<AudioSource>();
		myAudio = component;
	}

	// Token: 0x06000146 RID: 326 RVA: 0x00244250 File Offset: 0x00242850
	private void Update()
	{
		float num = lockTime;
		if (!(lockTime > 0f))
		{
			if (bDoorLocked)
			{
				bDoorLocked = false;
			}
		}
		else
		{
			float deltaTime = Time.deltaTime;
			lockTime = num - deltaTime;
		}
		num = openTime;
		if (openTime > 0f)
		{
			float deltaTime2 = Time.deltaTime;
			openTime = num - deltaTime2;
			num = deltaTime2;
		}
		if (bDoorOpen & !(0f < openTime))
		{
			MeshCollider meshCollider = barrier;
			meshCollider.enabled = true;
			meshCollider = invisibleBarrier;
			meshCollider.enabled = true;
			MeshRenderer meshRenderer = inside;
			bDoorOpen = false;
			MeshRenderer meshRenderer2 = meshRenderer;
			((Renderer)meshRenderer2).SetMaterial(closed);
			meshRenderer2 = outside;
			((Renderer)meshRenderer2).SetMaterial(closedOutside);
			if (silentOpens <= 0)
			{
				AudioSource audioSource = myAudio;
				audioSource.PlayOneShot(doorClose, 1f);
			}
		}
		InputManager instance = Singleton<InputManager>.Instance;
		InputManager inputManager = instance;
		bool[] currentKeyStates = inputManager.currentKeyStates;
		bool[] previousKeyStates = inputManager.previousKeyStates;
		float timeScale = Time.timeScale;
		num = timeScale;
		Camera main = Camera.main;
		int width = Screen.width;
		int height = Screen.height;
		float x = width - (int)NativeIntrinsics.Asm("rdx") >> 1;
		num = height - (int)NativeIntrinsics.Asm("rdx") >> 1;
		Camera camera = main;
		Vector3 pos = default(Vector3);
		pos.x = x;
		pos.y = num;
		pos.z = 0f;
		Ray ray = camera.ScreenPointToRay(pos);
		long num2 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
		long num3 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x10, 8)");
		Ray ray2 = default(Ray);
		ray2.m_Origin.x = NativeIntrinsics.Reinterpret<float>(num2);
		RaycastHit hitInfo;
		bool flag = Physics.Raycast(ray2, out hitInfo);
		RaycastHit raycastHit = default(RaycastHit);
		Collider collider = raycastHit.collider;
		MeshCollider meshCollider2 = trigger;
		Collider collider2 = collider;
		bool flag2 = collider2 == meshCollider2;
		Transform transform = player;
		Vector3 position = transform.position;
		Transform transform2 = this.transform;
	}

	// Token: 0x06000147 RID: 327 RVA: 0x00244130 File Offset: 0x00242730
	public void OpenDoor()
	{
		if (silentOpens <= 0 && !bDoorOpen)
		{
			myAudio.PlayOneShot(doorOpen, 1f);
		}
		barrier.enabled = false;
		invisibleBarrier.enabled = false;
		MeshRenderer meshRenderer = inside;
		bDoorOpen = true;
		((Renderer)meshRenderer).SetMaterial(open);
		((Renderer)outside).SetMaterial(openOutside);
		openTime = 3f;
	}

	// Token: 0x06000148 RID: 328 RVA: 0x002440B0 File Offset: 0x002426B0
	private void OnTriggerStay(Collider other)
	{
		int num = (bDoorLocked ? 1 : 0);
		bool flag = other.CompareTag("NPC");
		int num2 = ((num == 0 && flag) ? 1 : 0);
		int num3 = (flag ? 1 : 0);
		if (num2 != 0)
		{
			OpenDoor();
		}
	}

	// Token: 0x06000149 RID: 329 RVA: 0x002440A0 File Offset: 0x002426A0
	public void LockDoor(float time)
	{
		lockTime = time;
		bDoorLocked = true;
	}

	// Token: 0x0600014A RID: 330 RVA: 0x00244240 File Offset: 0x00242840
	public void UnlockDoor()
	{
		bDoorLocked = false;
	}

	// Token: 0x0600014C RID: 332 RVA: 0x002441E0 File Offset: 0x002427E0
	public void SilenceDoor()
	{
		silentOpens = 4;
	}
}

