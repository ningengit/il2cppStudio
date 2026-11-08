using System;
using Il2CppStudio.Native;
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
		Transform transform = this.transform;
		Vector3 position = transform.position;
		Transform transform2 = player.transform;
		Vector3 position2 = transform2.position;
		offset = NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("unpcklps xmm6,xmm7"));
		offset.z = position.z - position2.z;
	}

	// Token: 0x06000140 RID: 320 RVA: 0x00240DF0 File Offset: 0x0023F3F0
	private void LateUpdate()
	{
		Transform transform = this.transform;
		Transform transform2 = player.transform;
		Vector3 position = transform2.position;
		Vector3 position2 = default(Vector3);
		position2.x = offset.x + position.x;
		position2.y = offset.y + position.y;
		position2.z = offset.z + position.z;
		transform.position = position2;
		Transform transform3 = this.transform;
		Transform transform4 = player.transform;
		Quaternion rotation = transform4.rotation;
		Vector3 euler = default(Vector3);
		euler.x = 0f;
		euler.y = (float)lookBehind * (MathF.PI / 180f);
		euler.z = 0f;
		Quaternion quaternion = Quaternion.Internal_FromEulerRad(euler);
		Quaternion rotation2 = default(Quaternion);
		rotation2.x = quaternion.x * rotation.w + quaternion.w * rotation.x + quaternion.z * rotation.y - quaternion.y * rotation.z;
		rotation2.y = quaternion.w * rotation.y + quaternion.y * rotation.w + quaternion.x * rotation.z - quaternion.z * rotation.x;
		rotation2.z = quaternion.w * rotation.z + quaternion.z * rotation.w + quaternion.y * rotation.x - quaternion.x * rotation.y;
		rotation2.w = quaternion.w * rotation.w - quaternion.x * rotation.x - quaternion.y * rotation.y - quaternion.z * rotation.z;
		transform3.rotation = rotation2;
	}
}

