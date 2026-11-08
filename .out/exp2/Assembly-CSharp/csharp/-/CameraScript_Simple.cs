using System;
using UnityEngine;

// Token: 0x0200003F RID: 63
public class CameraScript_Simple : MonoBehaviour
{
	// Token: 0x040001DF RID: 479 Offset: 0x20
	public GameObject player;

	// Token: 0x040001E0 RID: 480 Offset: 0x28
	private int lookBehind;

	// Token: 0x040001E1 RID: 481 Offset: 0x2C
	private Vector3 offset;

	// Token: 0x0600013F RID: 319 RVA: 0x00241070 File Offset: 0x0023F670
	private void Start()
	{
		Vector3 position = transform.position;
		Vector3 position2 = player.transform.position;
		offset = new Vector3(position.x - position2.x, position.y - position2.y, position.z - position2.z);
	}

	// Token: 0x06000140 RID: 320 RVA: 0x00240DF0 File Offset: 0x0023F3F0
	private void LateUpdate()
	{
		Transform transform = this.transform;
		Vector3 position = player.transform.position;
		Vector3 position2 = new Vector3(offset.x + position.x, offset.y + position.y, offset.z + position.z);
		transform.position = position2;
		Transform transform2 = this.transform;
		Quaternion rotation = player.transform.rotation;
		Vector3 euler = new Vector3(0f, (float)lookBehind * ((float)Math.PI / 180f), 0f);
		Quaternion quaternion = Quaternion.Internal_FromEulerRad(euler);
		Quaternion rotation2 = new Quaternion(quaternion.x * rotation.w + quaternion.w * rotation.x + quaternion.z * rotation.y - quaternion.y * rotation.z, quaternion.w * rotation.y + quaternion.y * rotation.w + quaternion.x * rotation.z - quaternion.z * rotation.x, quaternion.w * rotation.z + quaternion.z * rotation.w + quaternion.y * rotation.x - quaternion.x * rotation.y, quaternion.w * rotation.w - quaternion.x * rotation.x - quaternion.y * rotation.y - quaternion.z * rotation.z);
		transform2.rotation = rotation2;
	}
}

