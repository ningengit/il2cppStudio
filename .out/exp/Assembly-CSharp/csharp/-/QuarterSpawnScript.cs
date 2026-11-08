using UnityEngine;

// Token: 0x0200004C RID: 76
public class QuarterSpawnScript : MonoBehaviour
{
	// Token: 0x0400022E RID: 558 Offset: 0x20
	public AILocationSelectorScript wanderer;

	// Token: 0x0400022F RID: 559 Offset: 0x28
	public Transform location;

	// Token: 0x06000171 RID: 369 RVA: 0x00258DE0 File Offset: 0x002573E0
	private void Start()
	{
		wanderer.QuarterExclusive();
		Transform transform = this.transform;
		Vector3 position = location.position;
		Vector3 position2 = new Vector3(Vector3.up.x * 4f + position.x, Vector3.up.y * 4f + position.y, Vector3.up.z * 4f + position.z);
		transform.position = position2;
	}
}

