using UnityEngine;

// Token: 0x02000038 RID: 56
public class MoveTest : MonoBehaviour
{
	// Token: 0x0600011C RID: 284 RVA: 0x0024DF60 File Offset: 0x0024C560
	private void Update()
	{
		Transform transform = this.transform;
		Transform transform2 = this.transform;
		Vector3 position = transform2.position;
		Vector3 position2 = default(Vector3);
		position2.x = 0.1f + position.x;
		position2.y = 0f + position.y;
		position2.z = 0f + position.z;
		transform.position = position2;
	}
}

