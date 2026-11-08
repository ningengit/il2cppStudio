using System;
using UnityEngine;

// Token: 0x02000036 RID: 54
public class FirstPrizeSpriteScript : MonoBehaviour
{
	// Token: 0x0400017E RID: 382 Offset: 0x20
	public float debug;

	// Token: 0x0400017F RID: 383 Offset: 0x24
	public int angle;

	// Token: 0x04000180 RID: 384 Offset: 0x28
	public float angleF;

	// Token: 0x04000181 RID: 385 Offset: 0x30
	private SpriteRenderer sprite;

	// Token: 0x04000182 RID: 386 Offset: 0x38
	public Transform cam;

	// Token: 0x04000183 RID: 387 Offset: 0x40
	public Transform body;

	// Token: 0x04000184 RID: 388 Offset: 0x48
	public Sprite[] sprites = new Sprite[16];

	// Token: 0x06000113 RID: 275 RVA: 0x002458F0 File Offset: 0x00243EF0
	private void Start()
	{
		sprite = GetComponent<SpriteRenderer>();
	}

	// Token: 0x06000114 RID: 276 RVA: 0x00245940 File Offset: 0x00243F40
	private void Update()
	{
		Vector3 position = cam.position;
		Vector3 position2 = body.position;
		float z = position.z;
		Vector3 position3 = cam.position;
		Vector3 position4 = body.position;
		float num = (float)Math.Atan2(z - position2.z, position3.x - position4.x);
		angleF = num * 57.29578f;
		float num2 = num * 57.29578f;
		if (0f > num * 57.29578f)
		{
			angleF = num2 + 360f;
		}
		Vector3 eulerAngles = body.eulerAngles;
		Transform transform = body;
		debug = eulerAngles.y;
		float num3 = angleF;
		Vector3 eulerAngles2 = transform.eulerAngles;
		angleF = num3 + eulerAngles2.y;
		angle = Mathf.RoundToInt((num3 + eulerAngles2.y) / 22.5f);
		z = 0f;
		float num4 = -1f;
		float num5 = -16f;
		while (angle < 0 || (uint)angle >= 16u)
		{
			if (!((float)angle < z))
			{
				angle += (int)(1f * num5);
			}
			else
			{
				angle += (int)(num4 * num5);
			}
		}
		sprite.sprite = sprites[angle];
	}
}

