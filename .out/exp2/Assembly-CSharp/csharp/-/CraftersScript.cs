using UnityEngine;
using UnityEngine.AI;

// Token: 0x02000032 RID: 50
public class CraftersScript : MonoBehaviour
{
	// Token: 0x0400014A RID: 330 Offset: 0x20
	public bool db;

	// Token: 0x0400014B RID: 331 Offset: 0x21
	public bool angry;

	// Token: 0x0400014C RID: 332 Offset: 0x22
	public bool gettingAngry;

	// Token: 0x0400014D RID: 333 Offset: 0x24
	public float anger;

	// Token: 0x0400014E RID: 334 Offset: 0x28
	private float forceShowTime;

	// Token: 0x0400014F RID: 335 Offset: 0x30
	public Transform player;

	// Token: 0x04000150 RID: 336 Offset: 0x38
	public CharacterController cc;

	// Token: 0x04000151 RID: 337 Offset: 0x40
	public Transform playerCamera;

	// Token: 0x04000152 RID: 338 Offset: 0x48
	public Transform baldiTeleportPoint;

	// Token: 0x04000153 RID: 339 Offset: 0x50
	public Transform playerTeleportPoint;

	// Token: 0x04000154 RID: 340 Offset: 0x58
	public NavMeshAgent baldiAgent;

	// Token: 0x04000155 RID: 341 Offset: 0x60
	public GameObject sprite;

	// Token: 0x04000156 RID: 342 Offset: 0x68
	public GameControllerScript gc;

	// Token: 0x04000157 RID: 343 Offset: 0x70
	private NavMeshAgent agent;

	// Token: 0x04000158 RID: 344 Offset: 0x78
	public Renderer craftersRenderer;

	// Token: 0x04000159 RID: 345 Offset: 0x80
	public SpriteRenderer spriteImage;

	// Token: 0x0400015A RID: 346 Offset: 0x88
	public Sprite angrySprite;

	// Token: 0x0400015B RID: 347 Offset: 0x90
	private AudioSource audioDevice;

	// Token: 0x0400015C RID: 348 Offset: 0x98
	public AudioClip aud_Intro;

	// Token: 0x0400015D RID: 349 Offset: 0xA0
	public AudioClip aud_Loop;

	// Token: 0x060000FE RID: 254 RVA: 0x00242F50 File Offset: 0x00241550
	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		audioDevice = GetComponent<AudioSource>();
		sprite.SetActive(value: false);
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00243050 File Offset: 0x00241650
	private void Update()
	{
		float num = forceShowTime;
		if (forceShowTime > 0f)
		{
			forceShowTime = num - Time.deltaTime;
		}
		float num2 = anger;
		if (gettingAngry)
		{
			float deltaTime = Time.deltaTime;
			anger = deltaTime + num2;
			if (!angry && !(deltaTime + num2 < 1f))
			{
				AudioSource audioSource = audioDevice;
				angry = true;
				audioSource.PlayOneShot(aud_Intro);
				spriteImage.sprite = angrySprite;
			}
		}
		else if (num2 > 0f)
		{
			anger = num2 - Time.deltaTime;
		}
		if (angry)
		{
			NavMeshAgent navMeshAgent = agent;
			float speed = navMeshAgent.speed;
			navMeshAgent.speed = Time.deltaTime * 60f + speed;
			agent.SetDestination(player.position);
			if (!audioDevice.isPlaying)
			{
				audioDevice.PlayOneShot(aud_Loop);
			}
			return;
		}
		Vector3 position = transform.position;
		Vector3 destination = agent.destination;
		Vector3 vector = new Vector3(position.x - destination.x, position.y - destination.y, position.z - destination.z);
		float magnitude = vector.magnitude;
		Vector3 position2 = transform.position;
		Vector3 position3 = player.position;
		Vector3 vector2 = new Vector3(position2.x - position3.x, position2.y - position3.y, position2.z - position3.z);
		GameObject gameObject;
		int active;
		if (!(!(20f < magnitude) & !(vector2.magnitude < 60f)))
		{
			_ = forceShowTime;
			if (!(forceShowTime > 0f))
			{
				gameObject = sprite;
				active = 0;
				goto IL_0346;
			}
		}
		gameObject = sprite;
		active = 1;
		goto IL_0346;
		IL_0346:
		gameObject.SetActive((byte)active != 0);
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00242A60 File Offset: 0x00241060
	private void FixedUpdate()
	{
		_ = gc;
		Vector3 position = player.position;
		Transform transform = this.transform;
		_ = position.x;
		_ = position.y;
		_ = position.z;
		_ = transform.position;
		_ = this.transform;
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00242D60 File Offset: 0x00241360
	public void GiveLocation(Vector3 location, bool flee)
	{
		if (!angry && agent.isActiveAndEnabled)
		{
			agent.SetDestination(location);
			if (flee)
			{
				forceShowTime = 3f;
			}
		}
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00242FF0 File Offset: 0x002415F0
	private void TargetPlayer()
	{
		agent.SetDestination(player.position);
	}

	// Token: 0x06000103 RID: 259 RVA: 0x00242DE0 File Offset: 0x002413E0
	private void OnTriggerEnter(Collider other)
	{
		bool flag = other.tag == "Player";
		if (angry && flag)
		{
			cc.enabled = false;
			Transform transform = player;
			Vector3 position = playerTeleportPoint.position;
			Vector3 position3 = new Vector3(position.x, 4f, playerTeleportPoint.position.z);
			transform.position = position3;
			NavMeshAgent navMeshAgent = baldiAgent;
			Vector3 position4 = baldiTeleportPoint.position;
			Vector3 newPosition = default(Vector3);
			newPosition.x = position4.x;
			newPosition.y = position4.y;
			newPosition.y = 4f;
			newPosition.z = position4.z;
			navMeshAgent.Warp(newPosition);
			cc.enabled = true;
			gameObject.SetActive(value: false);
		}
	}
}

