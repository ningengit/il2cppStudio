using UnityEngine;

namespace TMPro.Examples;

// Token: 0x02000068 RID: 104
public class Benchmark02 : MonoBehaviour
{
	// Token: 0x040002AE RID: 686 Offset: 0x20
	public int SpawnType;

	// Token: 0x040002AF RID: 687 Offset: 0x24
	public int NumberOfNPC = 12;

	// Token: 0x040002B0 RID: 688 Offset: 0x28
	private TextMeshProFloatingText floatingText_Script;

	// Token: 0x060001E3 RID: 483 RVA: 0x00252A20 File Offset: 0x00251020
	private void Start()
	{
		if (NumberOfNPC <= 0)
		{
			return;
		}
		int num = 0;
		float maxInclusive = 95f;
		float minInclusive = -95f;
		float x = 0.5f;
		float y = 0f;
		float fontSize = 96f;
		float r = 1f;
		int num2 = 0;
		Vector3 localScale = default(Vector3);
		Vector3 position = default(Vector3);
		Color color = default(Color);
		Vector3 position2 = default(Vector3);
		Color color2 = default(Color);
		Vector3 position3 = default(Vector3);
		Color color3 = default(Color);
		do
		{
			GameObject gameObject2 = default(GameObject);
			float z = default(float);
			if (SpawnType != num)
			{
				if (SpawnType == 1)
				{
					GameObject gameObject3 = new GameObject();
					Transform transform2 = gameObject3.transform;
					float x3 = Random.Range(minInclusive, maxInclusive);
					z = Random.Range(minInclusive, maxInclusive);
					position2 = new Vector3(x3, 0.25f, z);
					transform2.position = position2;
					TextMesh textMesh = gameObject3.AddComponent<TextMesh>();
					textMesh.font = Resources.Load<Font>("Fonts/ARIAL");
					textMesh.GetComponent<Renderer>().SetMaterial(textMesh.font.material);
					textMesh.anchor = TextAnchor.LowerCenter;
					textMesh.fontSize = 96;
					color2.r = r;
					textMesh.color = color2;
					textMesh.text = "!";
					floatingText_Script = gameObject3.AddComponent<TextMeshProFloatingText>();
					floatingText_Script.SpawnType = 1;
				}
				else if (SpawnType == 2)
				{
					GameObject gameObject = new GameObject();
					gameObject.AddComponent<Canvas>().worldCamera = Camera.main;
					Transform obj = gameObject.transform;
					localScale = new Vector3(0.1f, 0.1f, 0.1f);
					obj.localScale = localScale;
					Transform transform = gameObject.transform;
					float x2 = Random.Range(minInclusive, maxInclusive);
					z = Random.Range(minInclusive, maxInclusive);
					position = new Vector3(x2, 5f, z);
					transform.position = position;
					TextMeshProUGUI textMeshProUGUI = new GameObject().AddComponent<TextMeshProUGUI>();
					textMeshProUGUI.rectTransform.SetParent(gameObject.transform, worldPositionStays: false);
					color.r = r;
					textMeshProUGUI.color = color;
					textMeshProUGUI.alignment = TextAlignmentOptions.Bottom;
					textMeshProUGUI.fontSize = fontSize;
					textMeshProUGUI.text = "!";
					gameObject2 = gameObject;
					goto IL_0323;
				}
				goto IL_0345;
			}
			GameObject gameObject4 = new GameObject();
			Transform transform3 = gameObject4.transform;
			float x4 = Random.Range(minInclusive, maxInclusive);
			z = Random.Range(minInclusive, maxInclusive);
			position3 = new Vector3(x4, 0.25f, z);
			transform3.position = position3;
			TextMeshPro textMeshPro = gameObject4.AddComponent<TextMeshPro>();
			textMeshPro.autoSizeTextContainer = true;
			textMeshPro.rectTransform.pivot = new Vector2(x, y);
			textMeshPro.alignment = TextAlignmentOptions.Bottom;
			textMeshPro.fontSize = fontSize;
			textMeshPro.enableKerning = false;
			color3.r = r;
			textMeshPro.color = color3;
			textMeshPro.text = "!";
			gameObject2 = gameObject4;
			goto IL_0323;
			IL_0345:
			num2++;
			continue;
			IL_0323:
			floatingText_Script = gameObject2.AddComponent<TextMeshProFloatingText>();
			floatingText_Script.SpawnType = num;
			goto IL_0345;
		}
		while (num2 + 1 < NumberOfNPC);
	}
}

