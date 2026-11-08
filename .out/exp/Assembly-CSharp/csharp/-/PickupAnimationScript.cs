using System;
using UnityEngine;

// Token: 0x0200004A RID: 74
public class PickupAnimationScript : MonoBehaviour
{
	// Token: 0x0400022B RID: 555 Offset: 0x20
	private Transform itemPosition;

	// Token: 0x0600016C RID: 364 RVA: 0x00256090 File Offset: 0x00254690
	private void Start()
	{
		itemPosition = GetComponent<Transform>();
	}

	// Token: 0x0600016D RID: 365 RVA: 0x002560E0 File Offset: 0x002546E0
	private void Update()
	{
		Transform transform = itemPosition;
		float num = (float)Math.Sin((float)Time.frameCount * ((float)Math.PI / 180f));
		Vector3 localPosition = new Vector3(0f, num * 0.5f + 1f, 0f);
		transform.localPosition = localPosition;
	}
}

