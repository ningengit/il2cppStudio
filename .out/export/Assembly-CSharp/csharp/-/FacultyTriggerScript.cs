using UnityEngine;

// Token: 0x02000044 RID: 68
public class FacultyTriggerScript : MonoBehaviour
{
	// Token: 0x04000210 RID: 528 Offset: 0x20
	public PlayerScript ps;

	// Token: 0x04000211 RID: 529 Offset: 0x28
	private BoxCollider hitBox;

	// Token: 0x06000157 RID: 343 RVA: 0x00255630 File Offset: 0x00253C30
	private void Start()
	{
		BoxCollider component = GetComponent<BoxCollider>();
		hitBox = component;
	}

	// Token: 0x06000158 RID: 344 RVA: 0x00255570 File Offset: 0x00253B70
	private void OnTriggerStay(Collider other)
	{
		GameObject gameObject = other.gameObject;
		GameObject gameObject2 = gameObject;
		bool flag = gameObject2.CompareTag("Player");
		bool flag2 = flag;
		if (flag)
		{
			PlayerScript playerScript = ps;
			if (!(1f < playerScript.guilt))
			{
				playerScript.guiltType = "faculty";
				playerScript.guilt = 1f;
			}
		}
	}
}

