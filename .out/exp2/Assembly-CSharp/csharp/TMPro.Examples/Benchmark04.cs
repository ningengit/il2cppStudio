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
		m_Transform = this.transform;
		Camera.main.orthographicSize = Screen.height / 2;
		int width = Screen.width;
		int height = Screen.height;
		float num = MinPointSize / 2;
		float num2 = (float)width / (float)height;
		if (MinPointSize > MaxPointSize)
		{
			return;
		}
		float num3 = 0f;
		float num4 = 0.975f;
		float y = 0.5f;
		float r = 1f;
		int num5 = default(int);
		Vector3 position2 = default(Vector3);
		Color color = default(Color);
		int num6 = default(int);
		do
		{
			if (SpawnType == 0)
			{
				GameObject gameObject = new GameObject("Text - " + num5 + " Pts");
				if (!(num3 > num + num))
				{
					Transform transform = gameObject.transform;
					Vector3 position = m_Transform.position;
					float x = (float)NativeIntrinsics.Asm("xorps xmm0,xmm10") * num2 * num4 + position.x;
					float y2 = num * num4 - num3 + position.y;
					float z = 0f + position.z;
					position2 = new Vector3(x, y2, z);
					transform.position = position2;
					TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
					textMeshPro.rectTransform.pivot = new Vector2(0f, y);
					textMeshPro.enableWordWrapping = false;
					textMeshPro.extraPadding = true;
					textMeshPro.isOrthographic = true;
					textMeshPro.fontSize = ((int*)num5)->m_value;
					textMeshPro.text = num5 + " pts - Lorem ipsum dolor sit...";
					color.r = r;
					textMeshPro.color = color;
					num6 = ((int*)num5)->m_value;
					_ = (float)((int*)num5)->m_value;
					num3 += (float)((int*)num5)->m_value;
				}
				else
				{
					break;
				}
			}
			num6 += Steps;
		}
		while (num6 + Steps <= MaxPointSize);
	}
}

