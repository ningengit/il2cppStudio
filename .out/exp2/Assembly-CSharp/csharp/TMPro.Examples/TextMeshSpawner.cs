using UnityEngine;

namespace TMPro.Examples;

// Token: 0x0200007C RID: 124
public class TextMeshSpawner : MonoBehaviour
{
	// Token: 0x0400032E RID: 814 Offset: 0x20
	public int SpawnType;

	// Token: 0x0400032F RID: 815 Offset: 0x24
	public int NumberOfNPC = 12;

	// Token: 0x04000330 RID: 816 Offset: 0x28
	public Font TheFont;

	// Token: 0x04000331 RID: 817 Offset: 0x30
	private TextMeshProFloatingText floatingText_Script;

	// Token: 0x06000238 RID: 568 RVA: 0x00250B40 File Offset: 0x0024F140
	private void Awake()
	{
	}

	// Token: 0x06000239 RID: 569 RVA: 0x00260720 File Offset: 0x0025ED20
	private void Start()
	{
		if (NumberOfNPC <= 0)
		{
			return;
		}
		int num = 0;
		float maxInclusive = 95f;
		float minInclusive = -95f;
		float fontSize = 96f;
		float r = 1f;
		int num2 = 0;
		Vector3 position = default(Vector3);
		Color color = default(Color);
		Vector3 position2 = default(Vector3);
		Color color2 = default(Color);
		do
		{
			if (SpawnType != num)
			{
				GameObject gameObject = new GameObject();
				Transform transform = gameObject.transform;
				float x = Random.Range(minInclusive, maxInclusive);
				float z = Random.Range(minInclusive, maxInclusive);
				position = new Vector3(x, 0.5f, z);
				transform.position = position;
				TextMesh textMesh = gameObject.AddComponent<TextMesh>();
				textMesh.GetComponent<Renderer>().SetMaterial(TheFont.material);
				textMesh.font = TheFont;
				textMesh.anchor = TextAnchor.LowerCenter;
				textMesh.fontSize = 96;
				color.r = r;
				textMesh.color = color;
				textMesh.text = "!";
				floatingText_Script = gameObject.AddComponent<TextMeshProFloatingText>();
				floatingText_Script.SpawnType = 1;
			}
			else
			{
				GameObject gameObject2 = new GameObject();
				Transform transform2 = gameObject2.transform;
				float x2 = Random.Range(minInclusive, maxInclusive);
				float z = Random.Range(minInclusive, maxInclusive);
				position2 = new Vector3(x2, 0.5f, z);
				transform2.position = position2;
				TextMeshPro textMeshPro = gameObject2.AddComponent<TextMeshPro>();
				textMeshPro.fontSize = fontSize;
				textMeshPro.text = "!";
				color2.r = r;
				textMeshPro.color = color2;
				floatingText_Script = gameObject2.AddComponent<TextMeshProFloatingText>();
				floatingText_Script.SpawnType = num;
			}
			num2++;
		}
		while (num2 + 1 < NumberOfNPC);
	}
}

