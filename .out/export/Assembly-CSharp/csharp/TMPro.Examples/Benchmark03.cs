using Il2CppStudio.Native;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x02000069 RID: 105
public class Benchmark03 : MonoBehaviour
{
	// Token: 0x040002B1 RID: 689 Offset: 0x20
	public int SpawnType;

	// Token: 0x040002B2 RID: 690 Offset: 0x24
	public int NumberOfNPC = 12;

	// Token: 0x040002B3 RID: 691 Offset: 0x28
	public Font TheFont;

	// Token: 0x060001E5 RID: 485 RVA: 0x00250B40 File Offset: 0x0024F140
	private void Awake()
	{
	}

	// Token: 0x060001E6 RID: 486 RVA: 0x002530C0 File Offset: 0x002516C0
	private void Start()
	{
		if (NumberOfNPC <= 0)
		{
			return;
		}
		long num = NativeIntrinsics.Asm("global_13EAC90");
		Vector3 position = default(Vector3);
		Color color = default(Color);
		Vector3 position2 = default(Vector3);
		do
		{
			if (SpawnType != 0)
			{
				GameObject gameObject = new GameObject();
				Transform transform = gameObject.transform;
				Transform transform2 = transform;
				position.x = 0f;
				position.z = 0f;
				transform2.position = position;
				TextMesh textMesh = gameObject.AddComponent<TextMesh>();
				TextMesh textMesh2 = textMesh;
				Renderer component = textMesh2.GetComponent<Renderer>();
				Renderer renderer = component;
				Material material = TheFont.material;
				Material material2 = material;
				component.SetMaterial(material2);
				textMesh.font = TheFont;
				textMesh.anchor = NativeIntrinsics.Reinterpret<TextAnchor>(4);
				textMesh.fontSize = 96;
				color.r = NativeIntrinsics.Reinterpret<float>(num);
				textMesh.color = color;
				textMesh.text = "@";
			}
			else
			{
				GameObject gameObject2 = new GameObject();
				Transform transform3 = gameObject2.transform;
				Transform transform4 = transform3;
				position2.x = 0f;
				position2.z = 0f;
				transform4.position = position2;
				TextMeshPro textMeshPro = gameObject2.AddComponent<TextMeshPro>();
				TextMeshPro textMeshPro2 = textMeshPro;
				textMeshPro2.alignment = NativeIntrinsics.Reinterpret<TextAlignmentOptions>(514);
				textMeshPro.fontSize = 96f;
			}
			int num2 = 0 + 1;
		}
		while (0 + 1 < NumberOfNPC);
	}
}

