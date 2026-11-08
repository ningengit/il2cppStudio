using System;
using Il2CppStudio.Native;
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
		audioDevice = GetComponent<AudioSource>();
		waitTime = UnityEngine.Random.Range(60f, 120f);
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x002406A0 File Offset: 0x0023ECA0
	private void Update()
	{
		float num = waitTime;
		if ((waitTime > 0f))
		{
			waitTime = num - Time.deltaTime;
		}
		else if (!active)
		{
			Activate();
		}
		if (active)
		{
			float num2 = activeTime;
			float deltaTime = Time.deltaTime;
			activeTime = deltaTime + num2;
			Vector3 position = transform.position;
			Vector3 position2 = player.position;
			Vector3 vector = new Vector3(position.x - position2.x, position.y - position2.y, position.z - position2.z);
			if (!(deltaTime + num2 < 180f) & !(vector.magnitude < 120f))
			{
				Reset();
			}
		}
		num = guilt;
		if (guilt > 0f)
		{
			guilt = num - Time.deltaTime;
		}
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x0023FDD0 File Offset: 0x0023E3D0
	private void FixedUpdate()
	{
		Vector3 position = player.position;
		Transform transform = this.transform;
		float x = position.x;
		float y = position.y;
		float z = position.z;
		Vector3 position2 = transform.position;
		Transform transform2 = this.transform;
		x -= position2.x;
		y -= position2.y;
		z -= position2.z;
		Vector3 position3 = transform2.position;
		float x2 = 0f + position3.x;
		float y2 = 4f + position3.y;
		float z2 = 0f + position3.z;
		Vector3 vector = new Vector3(x2, y2, z2);
		Vector3 origin = vector;
		Vector3 direction = new Vector3(x, y, z);
		RaycastHit hitInfo;
		bool flag = Physics.Raycast(origin, direction, out hitInfo, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore);
		bool flag2 = hitInfo.transform.tag == "Player";
		Vector3 position4 = this.transform.position;
		Vector3 position5 = player.position;
		Vector3 vector2 = new Vector3(position4.x - position5.x, position4.y - position5.y, position4.z - position5.z);
		if (!(30f < vector2.magnitude) && active && flag2 && flag)
		{
			if (!spoken)
			{
				int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
				audioDevice.PlayOneShot(aud_Taunts[num]);
				spoken = true;
			}
			guilt = 10f;
		}
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x0023FB80 File Offset: 0x0023E180
	private void Activate()
	{
		wanderer.GetNewTargetHallway();
		Transform transform = this.transform;
		Vector3 position = wanderTarget.position;
		float x = 0f + position.x;
		float y = 5f + position.y;
		float z = 0f + position.z;
		Vector3 position2 = new Vector3(x, y, z);
		transform.position = position2;
		Transform transform2 = this.transform;
		float num = 20f;
		float num2 = 5f;
		Vector3 position6 = default(Vector3);
		while (true)
		{
			Vector3 position3 = transform2.position;
			Vector3 position4 = player.position;
			float num3 = position3.y - position4.y;
			float num4 = position3.x - position4.x;
			float num5 = position3.z - position4.z;
			double d = num3 * num3 + num4 * num4 + num5 * num5;
			if (!(num > (float)((0.0 > (double)(num3 * num3 + num4 * num4 + num5 * num5)) ? Math.Sqrt(d) : Math.Sqrt(d))))
			{
				break;
			}
			wanderer.GetNewTargetHallway();
			Transform transform3 = this.transform;
			Vector3 position5 = wanderTarget.position;
			float x2 = 0f + position5.x;
			float y2 = num2 + position5.y;
			z = 0f + position5.z;
			position6 = new Vector3(x2, y2, z);
			transform3.position = position6;
			transform2 = this.transform;
		}
		active = true;
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x002400F0 File Offset: 0x0023E6F0
	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.tag != "Player")
		{
			return;
		}
		bool num = (gc.item[0] == 0 && ((gc.item[2] == 0) & (gc.item[1] == 0)));
		_ = (gc.item[2] == 0) & (gc.item[1] == 0);
		if (!num)
		{
			int num2 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 2f));
			GameControllerScript gameControllerScript = gc;
			int num3 = num2;
			float maxInclusive = 2f;
			float num4 = 0f;
			double num5 = -0.5;
			double num6 = 0.5;
			double num7 = 1.0;
			double num13 = default(double);
			double num15 = default(double);
			while (gameControllerScript.item[num3] == 0)
			{
				float num8 = UnityEngine.Random.Range(0f, maxInclusive);
				float num9 = num8;
				float num10 = num8;
				NativeIntrinsics.CallNative("modf", num3, NativeIntrinsics.Asm("&stack[0x10]"), 0L, 0L);
				double num11 = num9;
				if (!((double)num9 < (double)num4))
				{
					if ((double)num10 == num6)
					{
						long num12 = NativeIntrinsics.Asm("stack[0x10]");
						if (((int)NativeIntrinsics.Asm("stack[0x10]") & 1) != 0)
						{
							num13 = (double)num12 + num7;
						}
					}
					else
					{
						num13 = Math.Floor(num11 + num6);
					}
				}
				else if (num13 == num5)
				{
					long num14 = NativeIntrinsics.Asm("stack[0x10]");
					if (((int)NativeIntrinsics.Asm("stack[0x10]") & 1) != 0)
					{
						num15 = (double)num14 - num7;
					}
				}
				else
				{
					num15 = Math.Ceiling(num11 - num6);
				}
				gameControllerScript = gc;
				num3 = (int)num15;
			}
			GameControllerScript gameControllerScript2 = gc;
			gc.item[num3] = 0;
			gameControllerScript2.itemSlot[num3].texture = gameControllerScript2.itemTextures[0];
			string text = gameControllerScript2.itemNames[gameControllerScript2.item[(int)gameControllerScript2.itemSelected]];
			_ = gameControllerScript2.itemText;
			_ = gameControllerScript2.itemNames;
			gameControllerScript2.itemText.text = text;
			int num17 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
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
		bool flag = other.transform.name == "Principal of the Thing";
		bool num = (guilt > 0f && flag);
		_ = guilt;
		if (num)
		{
			Reset();
		}
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00240570 File Offset: 0x0023EB70
	private void Reset()
	{
		Transform transform = this.transform;
		Vector3 position = this.transform.position;
		Vector3 position2 = new Vector3(position.x - 0f, position.y - 20f, position.z - 0f);
		transform.position = position2;
		waitTime = UnityEngine.Random.Range(60f, 120f);
		active = false;
		activeTime = 0f;
	}
}

