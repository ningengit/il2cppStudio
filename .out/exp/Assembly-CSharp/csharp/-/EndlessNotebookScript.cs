using UnityEngine;

// Token: 0x02000048 RID: 72
public class EndlessNotebookScript : MonoBehaviour
{
	// Token: 0x0400021F RID: 543 Offset: 0x20
	public float openingDistance;

	// Token: 0x04000220 RID: 544 Offset: 0x28
	public GameControllerScript gc;

	// Token: 0x04000221 RID: 545 Offset: 0x30
	public Transform player;

	// Token: 0x04000222 RID: 546 Offset: 0x38
	public GameObject learningGame;

	// Token: 0x06000166 RID: 358 RVA: 0x002550E0 File Offset: 0x002536E0
	private void Start()
	{
		gc = GameObject.Find("Game Controller").GetComponent<GameControllerScript>();
		player = GameObject.Find("Player").GetComponent<Transform>();
	}

	// Token: 0x06000167 RID: 359 RVA: 0x002551A0 File Offset: 0x002537A0
	private void Update()
	{
		Input.GetMouseButtonDown(0);
		Camera main = Camera.main;
		int width = Screen.width;
		int height = Screen.height;
		float x = width / 2;
		float y = height / 2;
		Vector3 pos = new Vector3(x, y, 0f);
		Physics.Raycast(main.ScreenPointToRay(pos), out var hitInfo);
		_ = hitInfo.transform.tag == "Notebook";
		_ = player.position;
		_ = transform;
	}
}

