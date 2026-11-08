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
		id = Mathf.RoundToInt(Random.Range(0f, 28f));
		transform.position = newLocation[id].position;
		ambience.PlayAudio();
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x0023E450 File Offset: 0x0023CA50
	public void GetNewTargetHallway()
	{
		id = Mathf.RoundToInt(Random.Range(0f, 15f));
		transform.position = newLocation[id].position;
		ambience.PlayAudio();
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x0023E5B0 File Offset: 0x0023CBB0
	public void QuarterExclusive()
	{
		id = Mathf.RoundToInt(Random.Range(1f, 15f));
		transform.position = newLocation[id].position;
	}
}

