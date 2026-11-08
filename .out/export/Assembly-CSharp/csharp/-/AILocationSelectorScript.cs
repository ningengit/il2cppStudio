using UnityEngine;

// Token: 0x0200002E RID: 46
public class AILocationSelectorScript : MonoBehaviour
{
	// Token: 0x0400011A RID: 282 Offset: 0x20
	public Transform[] newLocation = new Transform[29];

	// Token: 0x0400011B RID: 283 Offset: 0x28
	public AmbienceScript ambience;

	// Token: 0x0400011C RID: 284 Offset: 0x30
	private int id;

	// Token: 0x060000E4 RID: 228 RVA: 0x0023E500 File Offset: 0x0023CB00
	public void GetNewTarget()
	{
		float f = Random.Range(0f, 28f);
		int num = Mathf.RoundToInt(f);
		id = num;
		Transform transform = this.transform;
		Vector3 position = newLocation[id].position;
		Vector3 position2 = default(Vector3);
		position2.x = position.x;
		position2.y = position.y;
		position2.z = position.z;
		transform.position = position2;
		ambience.PlayAudio();
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x0023E450 File Offset: 0x0023CA50
	public void GetNewTargetHallway()
	{
		float f = Random.Range(0f, 15f);
		int num = Mathf.RoundToInt(f);
		id = num;
		Transform transform = this.transform;
		Vector3 position = newLocation[id].position;
		Vector3 position2 = default(Vector3);
		position2.x = position.x;
		position2.y = position.y;
		position2.z = position.z;
		transform.position = position2;
		ambience.PlayAudio();
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x0023E5B0 File Offset: 0x0023CBB0
	public void QuarterExclusive()
	{
		float f = Random.Range(1f, 15f);
		int num = Mathf.RoundToInt(f);
		id = num;
		Transform transform = this.transform;
		Vector3 position = newLocation[id].position;
		Vector3 position2 = default(Vector3);
		position2.x = position.x;
		position2.y = position.y;
		position2.z = position.z;
		transform.position = position2;
	}
}

