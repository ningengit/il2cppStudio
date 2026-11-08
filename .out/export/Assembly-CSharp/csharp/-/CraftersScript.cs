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
		NavMeshAgent component = GetComponent<NavMeshAgent>();
		agent = component;
		AudioSource component2 = GetComponent<AudioSource>();
		audioDevice = component2;
		sprite.SetActive(value: false);
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00243050 File Offset: 0x00241650
	private void Update()
	{
		float num = forceShowTime;
		if (forceShowTime > 0f)
		{
			float deltaTime = Time.deltaTime;
			forceShowTime = num - deltaTime;
			num = deltaTime;
		}
		float num2 = anger;
		if (!gettingAngry)
		{
			if (num2 > 0f)
			{
				float deltaTime2 = Time.deltaTime;
				anger = num2 - deltaTime2;
				num = deltaTime2;
			}
		}
		else
		{
			float deltaTime3 = Time.deltaTime;
			anger = deltaTime3 + num2;
			num = deltaTime3 + num2;
			if (!angry && !(deltaTime3 + num2 < 1f))
			{
				AudioSource audioSource = audioDevice;
				angry = true;
				audioSource.PlayOneShot(aud_Intro);
				spriteImage.sprite = angrySprite;
			}
		}
		if (angry)
		{
			NavMeshAgent navMeshAgent = agent;
			float speed = navMeshAgent.speed;
			float deltaTime4 = Time.deltaTime;
			navMeshAgent.speed = deltaTime4 * 60f + speed;
			NavMeshAgent navMeshAgent2 = agent;
			Vector3 position = player.position;
			Vector3 destination = default(Vector3);
			destination.x = position.x;
			destination.y = position.y;
			destination.z = position.z;
			bool flag = navMeshAgent2.SetDestination(destination);
			bool flag2 = flag;
			bool isPlaying = audioDevice.isPlaying;
			flag2 = isPlaying;
			if (!isPlaying)
			{
				audioDevice.PlayOneShot(aud_Loop);
			}
			return;
		}
		Transform transform = this.transform;
		Vector3 position2 = transform.position;
		Vector3 destination2 = agent.destination;
		Vector3 vector = default(Vector3);
		vector.x = position2.x - destination2.x;
		vector.y = position2.y - destination2.y;
		vector.z = position2.z - destination2.z;
		float magnitude = vector.magnitude;
		Transform transform2 = this.transform;
		Vector3 position3 = transform2.position;
		Vector3 position4 = player.position;
		Vector3 vector2 = default(Vector3);
		vector2.x = position3.x - position4.x;
		vector2.y = position3.y - position4.y;
		vector2.z = position3.z - position4.z;
		float magnitude2 = vector2.magnitude;
		GameObject gameObject;
		if (!(!(20f < magnitude) && !(magnitude2 < 60f)))
		{
			num = forceShowTime;
			if (!(forceShowTime > 0f))
			{
				gameObject = sprite;
				goto IL_0297;
			}
		}
		gameObject = sprite;
		goto IL_0297;
		IL_0297:
		gameObject.SetActive(value: false);
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00242A60 File Offset: 0x00241060
	private void FixedUpdate()
	{
		GameControllerScript gameControllerScript = gc;
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

	// Token: 0x06000101 RID: 257 RVA: 0x00242D60 File Offset: 0x00241360
	public void GiveLocation(Vector3 location, bool flee)
	{
		if (angry)
		{
			return;
		}
		bool flag = agent.isActiveAndEnabled;
		bool flag2 = flag;
		if (flag)
		{
			NavMeshAgent navMeshAgent = agent;
			Vector3 destination = default(Vector3);
			destination.x = location.x;
			destination.y = location.y;
			destination.z = location.z;
			bool flag3 = navMeshAgent.SetDestination(destination);
			flag2 = flag3;
			if (flee)
			{
				forceShowTime = 3f;
			}
		}
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00242FF0 File Offset: 0x002415F0
	private void TargetPlayer()
	{
		NavMeshAgent navMeshAgent = agent;
		Vector3 position = player.position;
		Vector3 destination = default(Vector3);
		destination.x = position.x;
		destination.y = position.y;
		destination.z = position.z;
		bool flag = navMeshAgent.SetDestination(destination);
	}

	// Token: 0x06000103 RID: 259 RVA: 0x00242DE0 File Offset: 0x002413E0
	private void OnTriggerEnter(Collider other)
	{
		string a = other.tag;
		bool flag = string.Equals(a, "Player");
		bool flag2 = flag;
		if (angry && flag)
		{
			cc.enabled = false;
			Transform transform = player;
			Vector3 position = playerTeleportPoint.position;
			Vector3 position2 = playerTeleportPoint.position;
			Vector3 position3 = default(Vector3);
			position3.x = position.x;
			position3.y = 4f;
			position3.z = position2.z;
			transform.position = position3;
			NavMeshAgent navMeshAgent = baldiAgent;
			Vector3 position4 = baldiTeleportPoint.position;
			Vector3 newPosition = default(Vector3);
			newPosition.x = position4.x;
			newPosition.y = position4.y;
			newPosition.y = 4f;
			newPosition.z = position4.z;
			bool flag3 = navMeshAgent.Warp(newPosition);
			flag2 = flag3;
			cc.enabled = true;
			GameObject gameObject = this.gameObject;
			GameObject gameObject2 = gameObject;
			gameObject2.SetActive(value: false);
		}
	}
}

