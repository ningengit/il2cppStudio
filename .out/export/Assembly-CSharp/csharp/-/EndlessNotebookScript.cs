using Il2CppStudio.Native;
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
		GameObject gameObject = GameObject.Find("Game Controller");
		GameControllerScript component = gameObject.GetComponent<GameControllerScript>();
		gc = component;
		GameObject gameObject2 = GameObject.Find("Player");
		Transform component2 = gameObject2.GetComponent<Transform>();
		player = component2;
	}

	// Token: 0x06000167 RID: 359 RVA: 0x002551A0 File Offset: 0x002537A0
	private void Update()
	{
		bool mouseButtonDown = Input.GetMouseButtonDown(0);
		Camera main = Camera.main;
		int width = Screen.width;
		int height = Screen.height;
		float x = width - (int)NativeIntrinsics.Asm("rdx") >> 1;
		float y = height - (int)NativeIntrinsics.Asm("rdx") >> 1;
		Camera camera = main;
		Vector3 pos = default(Vector3);
		pos.x = x;
		pos.y = y;
		pos.z = 0f;
		Ray ray = camera.ScreenPointToRay(pos);
		long num = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
		long num2 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x10, 8)");
		Ray ray2 = default(Ray);
		ray2.m_Origin.x = NativeIntrinsics.Reinterpret<float>(num);
		RaycastHit hitInfo;
		bool flag = Physics.Raycast(ray2, out hitInfo);
		RaycastHit raycastHit = default(RaycastHit);
		Transform transform = raycastHit.transform;
		Transform transform2 = transform;
		string a = transform2.tag;
		bool flag2 = string.Equals(a, "Notebook");
		Transform transform3 = player;
		Vector3 position = transform3.position;
		Transform transform4 = this.transform;
	}
}

