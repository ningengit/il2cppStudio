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
		float fontSize = 96f;
		float r = 1f;
		int num = 0;
		Vector3 position = default(Vector3);
		Color color = default(Color);
		Vector3 position2 = default(Vector3);
		Color color2 = default(Color);
		do
		{
			if (SpawnType != 0)
			{
				GameObject gameObject = new GameObject();
				Transform obj = gameObject.transform;
				position.x = 0f;
				position.z = 0f;
				obj.position = position;
				TextMesh textMesh = gameObject.AddComponent<TextMesh>();
				textMesh.GetComponent<Renderer>().SetMaterial(TheFont.material);
				textMesh.font = TheFont;
				textMesh.anchor = TextAnchor.MiddleCenter;
				textMesh.fontSize = 96;
				color.r = r;
				textMesh.color = color;
				textMesh.text = "@";
			}
			else
			{
				GameObject gameObject2 = new GameObject();
				Transform obj2 = gameObject2.transform;
				position2.x = 0f;
				position2.z = 0f;
				obj2.position = position2;
				TextMeshPro textMeshPro = gameObject2.AddComponent<TextMeshPro>();
				textMeshPro.alignment = TextAlignmentOptions.Center;
				textMeshPro.fontSize = fontSize;
				textMeshPro.text = "@";
				color2.r = r;
				textMeshPro.color = color2;
			}
			num++;
		}
		while (num + 1 < NumberOfNPC);
	}
}

