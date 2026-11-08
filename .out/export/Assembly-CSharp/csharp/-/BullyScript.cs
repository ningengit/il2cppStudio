using Il2CppStudio.Native;
using TMPro;
using UnityEngine;

// Token: 0x02000030 RID: 48
public class BullyScript : MonoBehaviour
{
	// Token: 0x04000139 RID: 313 Offset: 0x20
	public Transform player;

	// Token: 0x0400013A RID: 314 Offset: 0x28
	public GameControllerScript gc;

	// Token: 0x0400013B RID: 315 Offset: 0x30
	public Renderer bullyRenderer;

	// Token: 0x0400013C RID: 316 Offset: 0x38
	public Transform wanderTarget;

	// Token: 0x0400013D RID: 317 Offset: 0x40
	public AILocationSelectorScript wanderer;

	// Token: 0x0400013E RID: 318 Offset: 0x48
	public float waitTime;

	// Token: 0x0400013F RID: 319 Offset: 0x4C
	public float activeTime;

	// Token: 0x04000140 RID: 320 Offset: 0x50
	public float guilt;

	// Token: 0x04000141 RID: 321 Offset: 0x54
	public bool active;

	// Token: 0x04000142 RID: 322 Offset: 0x55
	public bool spoken;

	// Token: 0x04000143 RID: 323 Offset: 0x58
	private AudioSource audioDevice;

	// Token: 0x04000144 RID: 324 Offset: 0x60
	public AudioClip[] aud_Taunts = new AudioClip[2];

	// Token: 0x04000145 RID: 325 Offset: 0x68
	public AudioClip[] aud_Thanks = new AudioClip[2];

	// Token: 0x04000146 RID: 326 Offset: 0x70
	public AudioClip aud_Denied;

	// Token: 0x060000F3 RID: 243 RVA: 0x00240630 File Offset: 0x0023EC30
	private void Start()
	{
		AudioSource component = GetComponent<AudioSource>();
		audioDevice = component;
		float num = Random.Range(60f, 120f);
		waitTime = num;
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x002406A0 File Offset: 0x0023ECA0
	private void Update()
	{
		float num = waitTime;
		if (!(waitTime > 0f))
		{
			if (!active)
			{
				Activate();
			}
		}
		else
		{
			float deltaTime = Time.deltaTime;
			waitTime = num - deltaTime;
			num = deltaTime;
		}
		if (active)
		{
			float num2 = activeTime;
			float deltaTime2 = Time.deltaTime;
			activeTime = deltaTime2 + num2;
			Transform transform = this.transform;
			Transform transform2 = transform;
			Vector3 position = transform2.position;
			Transform transform3 = player;
			Vector3 position2 = transform3.position;
			Vector3 vector = default(Vector3);
			vector.x = position.x - position2.x;
			vector.y = position.y - position2.y;
			vector.z = position.z - position2.z;
			float magnitude = vector.magnitude;
			int num3 = ((!(deltaTime2 + num2 < 180f)) ? 1 : 0);
			num = magnitude;
			if (!(deltaTime2 + num2 < 180f) && !(magnitude < 120f))
			{
				Reset();
			}
		}
		num = guilt;
		if (guilt > 0f)
		{
			float deltaTime3 = Time.deltaTime;
			guilt = num - deltaTime3;
			num = deltaTime3;
		}
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x0023FDD0 File Offset: 0x0023E3D0
	private void FixedUpdate()
	{
		Transform transform = player;
		Vector3 position = transform.position;
		Transform transform2 = this.transform;
		float x = position.x;
		float y = position.y;
		float z = position.z;
		Vector3 position2 = transform2.position;
		Transform transform3 = this.transform;
		x -= position2.x;
		y -= position2.y;
		z -= position2.z;
		Vector3 position3 = transform3.position;
		float num = 0f + position3.x;
		float num2 = 4f + position3.y;
		float z2 = 0f + position3.z;
		Vector3 vector = default(Vector3);
		vector.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("unpcklps xmm9,xmm10"));
		vector.z = z2;
		Vector3 origin = vector;
		Vector3 direction = default(Vector3);
		direction.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("unpcklps xmm6,xmm7"));
		direction.z = z;
		RaycastHit hitInfo;
		bool flag = Physics.Raycast(origin, direction, out hitInfo, float.PositiveInfinity, 769, NativeIntrinsics.Reinterpret<QueryTriggerInteraction>(1));
		RaycastHit raycastHit = default(RaycastHit);
		Transform transform4 = raycastHit.transform;
		Transform transform5 = transform4;
		string a = transform5.tag;
		bool flag2 = string.Equals(a, "Player");
		Transform transform6 = this.transform;
		transform5 = transform6;
		Vector3 position4 = transform5.position;
		transform = player;
		Vector3 position5 = transform.position;
		Vector3 vector2 = default(Vector3);
		vector2.x = position4.x - position5.x;
		vector2.y = position4.y - position5.y;
		vector2.z = position4.z - position5.z;
		float magnitude = vector2.magnitude;
		int num3 = ((!(30f < magnitude) && active && flag2 && flag) ? 1 : 0);
		float num4 = magnitude;
		int num5 = num3;
		if (num3 != 0)
		{
			if (!spoken)
			{
				float f = Random.Range(0f, 1f);
				int num6 = Mathf.RoundToInt(f);
				int num7 = num6;
				AudioSource audioSource = audioDevice;
				audioSource.PlayOneShot(aud_Taunts[num6]);
				spoken = true;
			}
			guilt = 10f;
		}
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x0023FB80 File Offset: 0x0023E180
	private void Activate()
	{
		AILocationSelectorScript aILocationSelectorScript = wanderer;
		aILocationSelectorScript.GetNewTargetHallway();
		Transform transform = this.transform;
		Transform transform2 = transform;
		Vector3 position = wanderTarget.position;
		float x = 0f + position.x;
		float y = 5f + position.y;
		float z = 0f + position.z;
		Vector3 position2 = default(Vector3);
		position2.x = x;
		position2.y = y;
		position2.z = z;
		transform.position = position2;
		Transform transform3 = this.transform;
		transform2 = transform3;
		long num5 = default(long);
		Vector3 position6 = default(Vector3);
		while (true)
		{
			Vector3 position3 = transform2.position;
			Transform transform4 = player;
			Vector3 position4 = transform4.position;
			float num = position3.y - position4.y;
			float num2 = position3.x - position4.x;
			float num3 = position3.z - position4.z;
			long num4 = NativeIntrinsics.Asm("cvtps2pd xmm1,xmm6");
			if (!(0f > NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("cvtps2pd xmm1,xmm6"))))
			{
				num5 = NativeIntrinsics.Asm("sqrtsd xmm0,xmm1");
			}
			else
			{
				long num6 = NativeIntrinsics.CallNative("sub_20C340", 0L, NativeIntrinsics.Reinterpret<long>(transform4), 0L, 0L);
				long num7 = num6;
			}
			float num8 = num5;
			if (!(20f > (float)num5))
			{
				break;
			}
			wanderer.GetNewTargetHallway();
			Transform transform5 = this.transform;
			Transform transform6 = transform5;
			Vector3 position5 = wanderTarget.position;
			num8 = 0f + position5.x;
			float y2 = 5f + position5.y;
			z = 0f + position5.z;
			position6.x = num8;
			position6.y = y2;
			position6.z = z;
			transform5.position = position6;
			Transform transform7 = this.transform;
			transform6 = transform7;
		}
		active = true;
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x002400F0 File Offset: 0x0023E6F0
	private void OnTriggerEnter(Collider other)
	{
		Transform transform = other.transform;
		Transform transform2 = transform;
		string a = transform2.tag;
		bool flag = string.Equals(a, "Player");
		bool flag2 = flag;
		if (!flag)
		{
			return;
		}
		GameControllerScript gameControllerScript = gc;
		int[] item = gameControllerScript.item;
		int num = ((item[0] == 0 && ((gc.item[2] == 0) & (gc.item[1] == 0))) ? 1 : 0);
		int num2 = (((gc.item[2] == 0) & (gc.item[1] == 0)) ? 1 : 0);
		if (num == 0)
		{
			float f = Random.Range(0f, 2f);
			int num3 = Mathf.RoundToInt(f);
			GameControllerScript gameControllerScript2 = gc;
			num2 = num3;
			double num10 = default(double);
			double num13 = default(double);
			while (true)
			{
				item = gameControllerScript2.item;
				if (item[num2] != 0)
				{
					break;
				}
				float num4 = Random.Range(0f, 2f);
				float num5 = num4;
				float num6 = num4;
				long num7 = NativeIntrinsics.CallNative("sub_20BB40", 0L, NativeIntrinsics.Asm("&stack[0x10]"), 0L, 0L);
				double num8 = num5;
				if (!((double)num5 < 0.0))
				{
					if ((double)num6 == 0.5)
					{
						long num9 = NativeIntrinsics.Asm("stack[0x10]");
						if (((int)NativeIntrinsics.Asm("stack[0x10]") & 1) != 0)
						{
							num10 = (double)num9 + 1.0;
						}
					}
					else
					{
						long num11 = NativeIntrinsics.CallNative("sub_20C1F0", 0L, NativeIntrinsics.Reinterpret<long>(item), 0L, 0L);
					}
				}
				else if (num10 == -0.5)
				{
					long num12 = NativeIntrinsics.Asm("stack[0x10]");
					if (((int)NativeIntrinsics.Asm("stack[0x10]") & 1) != 0)
					{
						num13 = (double)num12 - 1.0;
					}
				}
				else
				{
					long num14 = NativeIntrinsics.CallNative("sub_20C080", 0L, NativeIntrinsics.Reinterpret<long>(item), 0L, 0L);
				}
				gameControllerScript2 = gc;
				num2 = (int)num13;
			}
			GameControllerScript gameControllerScript3 = gc;
			gc.item[num2] = 0;
			long num15 = num2;
			num15 = num2;
			gameControllerScript3.itemSlot[num2].texture = gameControllerScript3.itemTextures[0];
			num15 = gameControllerScript3.itemSelected;
			string text = gameControllerScript3.itemNames[gameControllerScript3.item[(int)num15]];
			TMP_Text itemText = gameControllerScript3.itemText;
			string[] itemNames = gameControllerScript3.itemNames;
			float f2 = Random.Range(0f, 1f);
			int num16 = Mathf.RoundToInt(f2);
			int num17 = num16;
			audioDevice.PlayOneShot(aud_Thanks[num17]);
			Reset();
		}
		else
		{
			audioDevice.PlayOneShot(aud_Denied);
		}
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x002404E0 File Offset: 0x0023EAE0
	private void OnTriggerStay(Collider other)
	{
		Transform transform = other.transform;
		Transform transform2 = transform;
		string a = transform2.name;
		bool flag = string.Equals(a, "Principal of the Thing");
		int num = ((guilt > 0f && flag) ? 1 : 0);
		int num2 = num;
		float num3 = guilt;
		if (num != 0)
		{
			Reset();
		}
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00240570 File Offset: 0x0023EB70
	private void Reset()
	{
		Transform transform = this.transform;
		Transform transform2 = this.transform;
		Vector3 position = transform2.position;
		Vector3 position2 = default(Vector3);
		position2.x = position.x - 0f;
		position2.y = position.y - 20f;
		position2.z = position.z - 0f;
		transform.position = position2;
		float num = Random.Range(60f, 120f);
		waitTime = num;
		active = false;
		activeTime = 0f;
	}
}

