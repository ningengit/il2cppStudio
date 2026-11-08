using Il2CppStudio.Native;
using UnityEngine;

// Token: 0x0200004A RID: 74
public class PickupAnimationScript : MonoBehaviour
{
	// Token: 0x0400022B RID: 555 Offset: 0x20
	private Transform itemPosition;

	// Token: 0x0600016C RID: 364 RVA: 0x00256090 File Offset: 0x00254690
	private void Start()
	{
		Transform component = GetComponent<Transform>();
		itemPosition = component;
	}

	// Token: 0x0600016D RID: 365 RVA: 0x002560E0 File Offset: 0x002546E0
	private void Update()
	{
		Transform transform = itemPosition;
		int frameCount = Time.frameCount;
		long num = NativeIntrinsics.CallNative("sub_20D1D0", 0L, 0L, 0L, 0L);
		Vector3 localPosition = default(Vector3);
		localPosition.x = 0f;
		localPosition.y = (float)NativeIntrinsics.Asm("xmm0") * 0.5f + 1f;
		localPosition.z = 0f;
		transform.localPosition = localPosition;
	}
}

