using Il2CppStudio.Native;
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
				float x = Random.Range(-95f, 95f);
				float num2 = Random.Range(-95f, 95f);
				float z = num2;
				position.x = x;
				position.y = 0.5f;
				position.z = z;
				transform.position = position;
				TextMesh textMesh = gameObject.AddComponent<TextMesh>();
				TextMesh textMesh2 = textMesh;
				Renderer component = textMesh2.GetComponent<Renderer>();
				Renderer renderer = component;
				Material material = TheFont.material;
				Material material2 = material;
				component.SetMaterial(material2);
				textMesh.font = TheFont;
				textMesh.anchor = NativeIntrinsics.Reinterpret<TextAnchor>(7);
				textMesh.fontSize = 96;
				color.r = NativeIntrinsics.Reinterpret<float>(num);
				textMesh.color = color;
				textMesh.text = "!";
				TextMeshProFloatingText textMeshProFloatingText = gameObject.AddComponent<TextMeshProFloatingText>();
				floatingText_Script = textMeshProFloatingText;
				TextMeshProFloatingText textMeshProFloatingText2 = floatingText_Script;
				textMeshProFloatingText2.SpawnType = 1;
			}
			else
			{
				GameObject gameObject2 = new GameObject();
				Transform transform2 = gameObject2.transform;
				float x2 = Random.Range(-95f, 95f);
				float num3 = Random.Range(-95f, 95f);
				float z = num3;
				position2.x = x2;
				position2.y = 0.5f;
				position2.z = z;
				transform2.position = position2;
				TextMeshPro textMeshPro = gameObject2.AddComponent<TextMeshPro>();
				TextMeshPro textMeshPro2 = textMeshPro;
				textMeshPro2.fontSize = 96f;
				TextMeshProFloatingText textMeshProFloatingText3 = gameObject2.AddComponent<TextMeshProFloatingText>();
				floatingText_Script = textMeshProFloatingText3;
				TextMeshProFloatingText textMeshProFloatingText4 = floatingText_Script;
				textMeshProFloatingText4.SpawnType = 0;
			}
			int num4 = 0 + 1;
		}
		while (0 + 1 < NumberOfNPC);
	}
}

