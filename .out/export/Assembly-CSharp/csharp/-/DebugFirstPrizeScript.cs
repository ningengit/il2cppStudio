using UnityEngine;

// Token: 0x02000034 RID: 52
public class DebugFirstPrizeScript : MonoBehaviour
{
	// Token: 0x04000161 RID: 353 Offset: 0x20
	public Transform player;

	// Token: 0x04000162 RID: 354 Offset: 0x28
	public Transform first;

	// Token: 0x06000108 RID: 264 RVA: 0x00243570 File Offset: 0x00241B70
	private void Update()
	{
		Transform transform = this.transform;
		Vector3 position = first.position;
		int num = Mathf.RoundToInt(first.forward.x);
		int num2 = Mathf.RoundToInt(first.forward.z);
		Vector3 position2 = default(Vector3);
		position2.x = (float)num * 3f + position.x;
		position2.y = position.y + 0f;
		position2.z = (float)num2 * 3f + position.z;
		transform.position = position2;
	}
}

