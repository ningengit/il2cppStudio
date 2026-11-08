using UnityEngine;

// Token: 0x02000031 RID: 49
public class ClickableTest : MonoBehaviour
{
	// Token: 0x04000147 RID: 327 Offset: 0x20
	private Transform player;

	// Token: 0x04000148 RID: 328 Offset: 0x28
	public AudioSource test;

	// Token: 0x04000149 RID: 329 Offset: 0x30
	public AudioClip testHit;

	// Token: 0x060000FB RID: 251 RVA: 0x00241300 File Offset: 0x0023F900
	private void Start()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
		if (gameObject != null)
		{
			player = gameObject.transform;
		}
	}

	// Token: 0x060000FC RID: 252 RVA: 0x002413A0 File Offset: 0x0023F9A0
	private void Update()
	{
		Input.GetMouseButtonDown(0);
		Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo);
		_ = hitInfo.transform.tag == "Notebook";
		_ = player.position;
		_ = transform;
	}
}

