using Il2CppStudio.Native;
using UnityEngine;

// Token: 0x02000045 RID: 69
public class AlarmClockScript : MonoBehaviour
{
	// Token: 0x04000212 RID: 530 Offset: 0x20
	public float timeLeft;

	// Token: 0x04000213 RID: 531 Offset: 0x24
	private float lifeSpan;

	// Token: 0x04000214 RID: 532 Offset: 0x28
	private bool rang;

	// Token: 0x04000215 RID: 533 Offset: 0x30
	public BaldiScript baldi;

	// Token: 0x04000216 RID: 534 Offset: 0x38
	public AudioClip ring;

	// Token: 0x04000217 RID: 535 Offset: 0x40
	public AudioSource audioDevice;

	// Token: 0x0600015A RID: 346 RVA: 0x002527A0 File Offset: 0x00250DA0
	private void Start()
	{
		timeLeft = 30f;
		lifeSpan = 35f;
	}

	// Token: 0x0600015B RID: 347 RVA: 0x002527B0 File Offset: 0x00250DB0
	private void Update()
	{
		float num = timeLeft;
		if (timeLeft < 0f)
		{
			if (!rang)
			{
				BaldiScript baldiScript = baldi;
				rang = true;
				if (baldiScript.isActiveAndEnabled)
				{
					BaldiScript baldiScript2 = baldi;
					Transform transform = this.transform;
					Vector3 position = transform.position;
					Vector3 soundLocation = default(Vector3);
					soundLocation.x = position.x;
					soundLocation.y = position.y;
					soundLocation.z = position.z;
					baldiScript2.Hear(soundLocation, 8f);
				}
				audioDevice.clip = ring;
				audioDevice.loop = false;
				audioDevice.Play();
			}
		}
		else
		{
			float deltaTime = Time.deltaTime;
			timeLeft = num - deltaTime;
		}
		num = lifeSpan;
		if (lifeSpan < 0f)
		{
			GameObject gameObject = this.gameObject;
			GameObject obj = gameObject;
			Object.Destroy(obj, 0f);
		}
		else
		{
			float deltaTime2 = Time.deltaTime;
			NativeIntrinsics.Stmt("movss dword ptr [rbx+24h],xmm6 ; value = BinaryExpr");
			num = deltaTime2;
		}
	}

	// Token: 0x0600015C RID: 348 RVA: 0x002526D0 File Offset: 0x00250CD0
	private void Alarm()
	{
		rang = true;
		if (baldi.isActiveAndEnabled)
		{
			BaldiScript baldiScript = baldi;
			Transform transform = this.transform;
			Vector3 position = transform.position;
			Vector3 soundLocation = default(Vector3);
			soundLocation.x = position.x;
			soundLocation.y = position.y;
			soundLocation.z = position.z;
			baldiScript.Hear(soundLocation, 8f);
		}
		audioDevice.clip = ring;
		audioDevice.loop = false;
		audioDevice.Play();
	}
}

