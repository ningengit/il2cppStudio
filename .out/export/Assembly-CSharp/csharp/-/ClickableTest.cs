using Il2CppStudio.Native;
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
		GameObject gameObject2 = gameObject;
		bool flag = gameObject2 != null;
		bool flag2 = flag;
		if (flag)
		{
			Transform transform = gameObject2.transform;
			player = transform;
		}
	}

	// Token: 0x060000FC RID: 252 RVA: 0x002413A0 File Offset: 0x0023F9A0
	private void Update()
	{
		bool mouseButtonDown = Input.GetMouseButtonDown(0);
		Camera main = Camera.main;
		Vector3 mousePosition = Input.mousePosition;
		Camera camera = main;
		Vector3 pos = default(Vector3);
		pos.x = mousePosition.x;
		pos.y = mousePosition.y;
		pos.z = mousePosition.z;
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

