using Il2CppStudio.Native;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x0200006A RID: 106
public class Benchmark04 : MonoBehaviour
{
	// Token: 0x040002B4 RID: 692 Offset: 0x20
	public int SpawnType;

	// Token: 0x040002B5 RID: 693 Offset: 0x24
	public int MinPointSize = 12;

	// Token: 0x040002B6 RID: 694 Offset: 0x28
	public int MaxPointSize = 64;

	// Token: 0x040002B7 RID: 695 Offset: 0x2C
	public int Steps = 4;

	// Token: 0x040002B8 RID: 696 Offset: 0x30
	private Transform m_Transform;

	// Token: 0x060001E8 RID: 488 RVA: 0x00253360 File Offset: 0x00251960
	private unsafe void Start()
	{
		Transform transform = this.transform;
		m_Transform = transform;
		Camera main = Camera.main;
		int height = Screen.height;
		int num = height;
		main.orthographicSize = num - (int)NativeIntrinsics.Asm("rdx") >> 1;
		int width = Screen.width;
		int height2 = Screen.height;
		num = MinPointSize;
		float num2 = height2;
		float num3 = num - (int)NativeIntrinsics.Asm("rdx") >> 1;
		float num4 = (float)width / (float)height2;
		if (MinPointSize > MaxPointSize)
		{
			return;
		}
		long num5 = NativeIntrinsics.Asm("global_13EACA0");
		int num6 = default(int);
		Vector3 position2 = default(Vector3);
		int num7 = default(int);
		int num8 = default(int);
		do
		{
			if (SpawnType == 0)
			{
				string text = num6.ToString();
				string text2 = "Text - " + text + " Pts";
				GameObject gameObject = new GameObject(text2);
				num2 = num3 + num3;
				GameObject gameObject2 = gameObject;
				if (0f > num3 + num3)
				{
					break;
				}
				Transform transform2 = gameObject2.transform;
				Transform transform3 = transform2;
				Vector3 position = m_Transform.position;
				num2 = (float)NativeIntrinsics.Asm("xorps xmm0,xmm10") * num4 * 0.975f + position.x;
				float y = num3 * 0.975f - 0f + position.y;
				float z = 0f + position.z;
				position2.x = num2;
				position2.y = y;
				position2.z = z;
				transform2.position = position2;
				TextMeshPro textMeshPro = gameObject2.AddComponent<TextMeshPro>();
				TextMeshPro textMeshPro2 = textMeshPro;
				RectTransform rectTransform = textMeshPro2.rectTransform;
				RectTransform rectTransform2 = rectTransform;
				rectTransform2.pivot = NativeIntrinsics.Reinterpret<Vector2>(NativeIntrinsics.Asm("unpcklps xmm0,xmm12"));
				textMeshPro.enableWordWrapping = false;
				textMeshPro.extraPadding = true;
				textMeshPro.isOrthographic = true;
				textMeshPro.fontSize = ((int*)num6)->m_value;
				string text3 = num7.ToString();
				string text4 = text3 + " pts - Lorem ipsum dolor sit...";
				num8 = ((int*)num7)->m_value;
				num2 = ((int*)num7)->m_value;
				float num9 = 0f + (float)((int*)num7)->m_value;
			}
			num8 += Steps;
		}
		while (num8 + Steps <= MaxPointSize);
	}
}

