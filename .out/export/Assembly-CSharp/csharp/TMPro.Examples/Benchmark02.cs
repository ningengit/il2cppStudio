using Il2CppStudio.Native;
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
		long num = NativeIntrinsics.Asm("global_13EAC90");
		Vector3 localScale = default(Vector3);
		Vector3 position = default(Vector3);
		Vector3 position2 = default(Vector3);
		Color color = default(Color);
		Vector3 position3 = default(Vector3);
		do
		{
			GameObject gameObject3;
			float z;
			TextMeshProFloatingText textMeshProFloatingText2;
			if (SpawnType != 0)
			{
				if (SpawnType != 1)
				{
					if (SpawnType == 2)
					{
						GameObject gameObject = new GameObject();
						Canvas canvas = gameObject.AddComponent<Canvas>();
						Camera main = Camera.main;
						canvas.worldCamera = main;
						Transform transform = gameObject.transform;
						localScale.x = 0.1f;
						localScale.y = 0.1f;
						localScale.z = 0.1f;
						transform.localScale = localScale;
						Transform transform2 = gameObject.transform;
						float x = Random.Range(-95f, 95f);
						float num2 = Random.Range(-95f, 95f);
						z = num2;
						position.x = x;
						position.y = 5f;
						position.z = z;
						transform2.position = position;
						GameObject gameObject2 = new GameObject();
						TextMeshProUGUI textMeshProUGUI = gameObject2.AddComponent<TextMeshProUGUI>();
						RectTransform rectTransform = textMeshProUGUI.rectTransform;
						Transform parent = gameObject.transform;
						rectTransform.SetParent(parent, worldPositionStays: false);
						textMeshProUGUI.alignment = NativeIntrinsics.Reinterpret<TextAlignmentOptions>(1026);
						textMeshProUGUI.fontSize = 96f;
						gameObject3 = gameObject;
						goto IL_0315;
					}
				}
				else
				{
					GameObject gameObject4 = new GameObject();
					Transform transform3 = gameObject4.transform;
					float x2 = Random.Range(-95f, 95f);
					float num3 = Random.Range(-95f, 95f);
					z = num3;
					position2.x = x2;
					position2.y = 0.25f;
					position2.z = z;
					transform3.position = position2;
					TextMesh textMesh = gameObject4.AddComponent<TextMesh>();
					Font font = Resources.Load<Font>("Fonts/ARIAL");
					Font font2 = font;
					textMesh.font = font2;
					Renderer component = textMesh.GetComponent<Renderer>();
					Font font3 = textMesh.font;
					font2 = font3;
					Material material = font2.material;
					Material material2 = material;
					component.SetMaterial(material2);
					textMesh.anchor = NativeIntrinsics.Reinterpret<TextAnchor>(7);
					textMesh.fontSize = 96;
					color.r = NativeIntrinsics.Reinterpret<float>(num);
					textMesh.color = color;
					textMesh.text = "!";
					TextMeshProFloatingText textMeshProFloatingText = gameObject4.AddComponent<TextMeshProFloatingText>();
					floatingText_Script = textMeshProFloatingText;
					textMeshProFloatingText2 = floatingText_Script;
					textMeshProFloatingText2.SpawnType = 1;
				}
				goto IL_0337;
			}
			GameObject gameObject5 = new GameObject();
			Transform transform4 = gameObject5.transform;
			float x3 = Random.Range(-95f, 95f);
			float num4 = Random.Range(-95f, 95f);
			z = num4;
			position3.x = x3;
			position3.y = 0.25f;
			position3.z = z;
			transform4.position = position3;
			TextMeshPro textMeshPro = gameObject5.AddComponent<TextMeshPro>();
			RectTransform rectTransform2 = textMeshPro.rectTransform;
			rectTransform2.pivot = NativeIntrinsics.Reinterpret<Vector2>(NativeIntrinsics.Asm("unpcklps xmm0,xmm12"));
			textMeshPro.alignment = NativeIntrinsics.Reinterpret<TextAlignmentOptions>(1026);
			textMeshPro.fontSize = 96f;
			textMeshPro.enableKerning = false;
			gameObject3 = gameObject5;
			goto IL_0315;
			IL_0337:
			int num5 = 0 + 1;
			continue;
			IL_0315:
			TextMeshProFloatingText textMeshProFloatingText3 = gameObject3.AddComponent<TextMeshProFloatingText>();
			floatingText_Script = textMeshProFloatingText3;
			textMeshProFloatingText2 = floatingText_Script;
			textMeshProFloatingText2.SpawnType = 0;
			goto IL_0337;
		}
		while (0 + 1 < NumberOfNPC);
	}
}

