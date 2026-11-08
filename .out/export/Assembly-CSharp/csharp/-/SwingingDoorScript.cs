using UnityEngine;

// Token: 0x02000043 RID: 67
public class SwingingDoorScript : MonoBehaviour
{
	// Token: 0x040001FB RID: 507 Offset: 0x20
	public GameControllerScript gc;

	// Token: 0x040001FC RID: 508 Offset: 0x28
	public BaldiScript baldi;

	// Token: 0x040001FD RID: 509 Offset: 0x30
	public MeshCollider barrier;

	// Token: 0x040001FE RID: 510 Offset: 0x38
	public GameObject obstacle;

	// Token: 0x040001FF RID: 511 Offset: 0x40
	public MeshCollider trigger;

	// Token: 0x04000200 RID: 512 Offset: 0x48
	public MeshRenderer inside;

	// Token: 0x04000201 RID: 513 Offset: 0x50
	public MeshRenderer outside;

	// Token: 0x04000202 RID: 514 Offset: 0x58
	public Material closed;

	// Token: 0x04000203 RID: 515 Offset: 0x60
	public Material open;

	// Token: 0x04000204 RID: 516 Offset: 0x68
	public Material locked;

	// Token: 0x04000205 RID: 517 Offset: 0x70
	public Material closedOutside;

	// Token: 0x04000206 RID: 518 Offset: 0x78
	public Material openOutside;

	// Token: 0x04000207 RID: 519 Offset: 0x80
	public Material lockedOutside;

	// Token: 0x04000208 RID: 520 Offset: 0x88
	public AudioClip doorOpen;

	// Token: 0x04000209 RID: 521 Offset: 0x90
	public AudioClip baldiDoor;

	// Token: 0x0400020A RID: 522 Offset: 0x98
	private float openTime;

	// Token: 0x0400020B RID: 523 Offset: 0x9C
	private float lockTime;

	// Token: 0x0400020C RID: 524 Offset: 0xA0
	public bool bDoorOpen;

	// Token: 0x0400020D RID: 525 Offset: 0xA1
	public bool bDoorLocked;

	// Token: 0x0400020E RID: 526 Offset: 0xA2
	private bool requirementMet;

	// Token: 0x0400020F RID: 527 Offset: 0xA8
	private AudioSource myAudio;

	// Token: 0x06000150 RID: 336 RVA: 0x00259930 File Offset: 0x00257F30
	private void Start()
	{
		AudioSource component = GetComponent<AudioSource>();
		myAudio = component;
		bDoorLocked = true;
	}

	// Token: 0x06000151 RID: 337 RVA: 0x00259A00 File Offset: 0x00258000
	private void Update()
	{
		int num = ((gc.notebooks >= 2) ? 1 : 0);
		if (!requirementMet & (gc.notebooks >= 2))
		{
			requirementMet = true;
			UnlockDoor();
		}
		float num2 = openTime;
		if (openTime > 0f)
		{
			float deltaTime = Time.deltaTime;
			openTime = num2 - deltaTime;
		}
		num2 = lockTime;
		if (!(lockTime > 0f))
		{
			if (bDoorLocked & requirementMet)
			{
				UnlockDoor();
			}
		}
		else
		{
			float deltaTime2 = Time.deltaTime;
			lockTime = num2 - deltaTime2;
			num2 = deltaTime2;
		}
		int num3 = ((!bDoorLocked) ? 1 : 0);
		if (bDoorOpen & (!(0f < openTime) & !bDoorLocked))
		{
			MeshRenderer meshRenderer = inside;
			bDoorOpen = false;
			((Renderer)meshRenderer).SetMaterial(closed);
			((Renderer)outside).SetMaterial(closedOutside);
		}
	}

	// Token: 0x06000152 RID: 338 RVA: 0x002598D0 File Offset: 0x00257ED0
	private void OnTriggerStay(Collider other)
	{
		if (!bDoorLocked)
		{
			bDoorOpen = true;
			((Renderer)inside).SetMaterial(open);
			((Renderer)outside).SetMaterial(openOutside);
			openTime = 2f;
		}
	}

	// Token: 0x06000153 RID: 339 RVA: 0x00259780 File Offset: 0x00257D80
	private void OnTriggerEnter(Collider other)
	{
		int notebooks = gc.notebooks;
		string a = other.tag;
		bool flag = string.Equals(a, "Player");
		int num = ((notebooks < 2 && flag) ? 1 : 0);
		int num2 = (flag ? 1 : 0);
		int num3 = num;
		if (num != 0 || (bDoorLocked ? 1 : 0) != num3)
		{
			return;
		}
		myAudio.PlayOneShot(doorOpen, 1f);
		string a2 = other.tag;
		bool flag2 = string.Equals(a2, "Player");
		bool flag3 = flag2;
		if (flag2)
		{
			bool flag4 = baldi.isActiveAndEnabled;
			flag3 = flag4;
			if (flag4)
			{
				BaldiScript baldiScript = baldi;
				Transform transform = this.transform;
				Transform transform2 = transform;
				Vector3 position = transform2.position;
				Vector3 soundLocation = default(Vector3);
				soundLocation.x = position.x;
				soundLocation.y = position.y;
				soundLocation.z = position.z;
				baldiScript.Hear(soundLocation, 1f);
			}
		}
	}

	// Token: 0x06000154 RID: 340 RVA: 0x002596F0 File Offset: 0x00257CF0
	public void LockDoor(float time)
	{
		barrier.enabled = true;
		obstacle.SetActive(value: true);
		MeshRenderer meshRenderer = inside;
		lockTime = time;
		bDoorLocked = true;
		((Renderer)meshRenderer).SetMaterial(locked);
		((Renderer)outside).SetMaterial(lockedOutside);
	}

	// Token: 0x06000155 RID: 341 RVA: 0x00259990 File Offset: 0x00257F90
	private void UnlockDoor()
	{
		barrier.enabled = false;
		obstacle.SetActive(value: false);
		MeshRenderer meshRenderer = inside;
		bDoorLocked = false;
		((Renderer)meshRenderer).SetMaterial(closed);
		((Renderer)outside).SetMaterial(closedOutside);
	}
}

