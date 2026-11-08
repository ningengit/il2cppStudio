using UnityEngine;

// Token: 0x02000038 RID: 56
public class MoveTest : MonoBehaviour
{
	// Token: 0x0600011C RID: 284 RVA: 0x0024DF60 File Offset: 0x0024C560
	private void Update()
	{
		Transform transform = this.transform;
		Vector3 position = this.transform.position;
		Vector3 position2 = new Vector3(0.1f + position.x, 0f + position.y, 0f + position.z);
		transform.position = position2;
	}
}

