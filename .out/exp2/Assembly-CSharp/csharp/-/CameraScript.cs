using System;
using Il2CppStudio.Native;
using UnityEngine;

// Token: 0x0200003E RID: 62
public class CameraScript : MonoBehaviour
{
	// Token: 0x040001D4 RID: 468 Offset: 0x20
	public GameObject player;

	// Token: 0x040001D5 RID: 469 Offset: 0x28
	public PlayerScript ps;

	// Token: 0x040001D6 RID: 470 Offset: 0x30
	public Transform baldi;

	// Token: 0x040001D7 RID: 471 Offset: 0x38
	public Vector3 BaldiOffset;

	// Token: 0x040001D8 RID: 472 Offset: 0x44
	public float initVelocity;

	// Token: 0x040001D9 RID: 473 Offset: 0x48
	public float velocity;

	// Token: 0x040001DA RID: 474 Offset: 0x4C
	public float gravity;

	// Token: 0x040001DB RID: 475 Offset: 0x50
	private int lookBehind;

	// Token: 0x040001DC RID: 476 Offset: 0x54
	public Vector3 offset;

	// Token: 0x040001DD RID: 477 Offset: 0x60
	public float jumpHeight;

	// Token: 0x040001DE RID: 478 Offset: 0x64
	public Vector3 jumpHeightV3;

	// Token: 0x0600013B RID: 315 RVA: 0x00241120 File Offset: 0x0023F720
	private void Start()
	{
		Vector3 position = transform.position;
		Vector3 position2 = player.transform.position;
		offset = new Vector3(position.x - position2.x, position.y - position2.y, position.z - position2.z);
	}

	// Token: 0x0600013C RID: 316 RVA: 0x002411D0 File Offset: 0x0023F7D0
	private void Update()
	{
		if (!ps.jumpRope)
		{
			_ = 0 - (Singleton<InputManager>.Instance.currentKeyStates[10] ? 1 : 0);
			lookBehind = (int)NativeIntrinsics.Asm("sbb ecx,ecx") & 0xB4;
			return;
		}
		float num = gravity;
		float num2 = velocity;
		float deltaTime = Time.deltaTime;
		float num3 = jumpHeight;
		velocity = num2 - deltaTime * num;
		float deltaTime2 = Time.deltaTime;
		jumpHeight = deltaTime2 * (num2 - deltaTime * num) + num3;
		if (!(0f < deltaTime2 * (num2 - deltaTime * num) + num3))
		{
			jumpHeight = 0f;
			if (Singleton<InputManager>.Instance.currentKeyStates[11])
			{
				velocity = initVelocity;
			}
		}
		jumpHeightV3.y = jumpHeight;
		jumpHeightV3.x = 0f;
		jumpHeightV3.z = 0f;
	}

	// Token: 0x0600013D RID: 317 RVA: 0x00240890 File Offset: 0x0023EE90
	private void LateUpdate()
	{
		Transform transform = this.transform;
		Vector3 position = player.transform.position;
		float x = offset.x + position.x;
		float y = offset.y + position.y;
		float z = offset.z + position.z;
		Vector3 position2 = new Vector3(x, y, z);
		transform.position = position2;
		PlayerScript playerScript = ps;
		_ = !playerScript.gameOver;
		Transform transform4;
		if ((!playerScript.gameOver & !playerScript.jumpRope))
		{
			transform4 = this.transform;
			Vector3 position10 = player.transform.position;
			x = offset.x + position10.x;
			y = offset.y + position10.y;
			z = offset.z + position10.z;
			Transform obj2 = transform4;
			Vector3 position11 = new Vector3(x, y, z);
			obj2.position = position11;
			transform4 = this.transform;
			Quaternion rotation = player.transform.rotation;
			Vector3 euler = new Vector3(0f, lookBehind * ((float)Math.PI / 180f), 0f);
			Quaternion quaternion = Quaternion.Internal_FromEulerRad(euler);
			_ = quaternion.x * rotation.y;
			_ = quaternion.y * rotation.x;
			_ = quaternion.x * rotation.x;
			_ = quaternion.y * rotation.y;
		}
		else if (!playerScript.gameOver)
		{
			if (!playerScript.jumpRope)
			{
				return;
			}
			transform4 = this.transform;
			Vector3 position8 = player.transform.position;
			x = offset.x + position8.x + jumpHeightV3.x;
			y = offset.y + position8.y + jumpHeightV3.y;
			z = offset.z + position8.z + jumpHeightV3.z;
			Transform obj = transform4;
			Vector3 position9 = new Vector3(x, y, z);
			obj.position = position9;
			transform4 = this.transform;
			_ = player.transform.rotation;
		}
		else
		{
			Transform transform2 = this.transform;
			Vector3 position3 = baldi.transform.position;
			Vector3 forward = baldi.transform.forward;
			float x2 = BaldiOffset.z * forward.x + position3.x + 0f;
			float y2 = BaldiOffset.z * forward.y + position3.y + BaldiOffset.y;
			float z2 = BaldiOffset.z * forward.z + position3.z + 0f;
			Vector3 position4 = new Vector3(x2, y2, z2);
			transform2.position = position4;
			Transform transform3 = this.transform;
			Vector3 position5 = baldi.position;
			float y3 = BaldiOffset.y + baldi.position.y;
			Vector3 worldPosition = new Vector3(position5.x, y3, baldi.position.z);
			transform3.LookAt(worldPosition);
			return;
		}
		Quaternion rotation2 = default(Quaternion);
		transform4.rotation = rotation2;
	}
}

