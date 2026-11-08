using MaterialKit;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000023 RID: 35
public class UiManager : MonoBehaviour
{
	// Token: 0x040000EC RID: 236 Offset: 0x20
	public CanvasScaler normScaler;

	// Token: 0x040000ED RID: 237 Offset: 0x28
	public DpCanvasScaler dpiScaler;

	// Token: 0x040000EE RID: 238 Offset: 0x30
	public RectTransform[] transforms;

	// Token: 0x060000BB RID: 187 RVA: 0x00251BD0 File Offset: 0x002501D0
	private void Start()
	{
		int num = PlayerPrefs.GetInt("UiSize");
		int num2 = PlayerPrefs.GetInt("UiHeight");
		int num3 = num;
		float x = default(float);
		float y = default(float);
		if (num == 1)
		{
			x = 640f;
			y = 480f;
		}
		else if (num3 == 2)
		{
			x = 800f;
			y = 600f;
		}
		else if (num3 == 3)
		{
			x = 900f;
			y = 720f;
		}
		else if (num3 == 4)
		{
			x = 1024f;
			y = 720f;
		}
		else
		{
			goto IL_00b3;
		}
		normScaler.referenceResolution = new Vector2(x, y);
		goto IL_00b3;
		IL_00b3:
		switch (num2)
		{
		case 2:
		{
			RectTransform[] array = transforms;
			Vector3 position8 = default(Vector3);
			foreach (RectTransform rectTransform2 in array)
			{
				Vector3 position5 = rectTransform2.position;
				Vector3 position6 = rectTransform2.position;
				int height2 = Screen.height;
				position8 = new Vector3(position5.x, (float)(height2 + ((height2 >> 31) & 3) >> 2) + position6.y, rectTransform2.position.z);
				rectTransform2.position = position8;
			}
			break;
		}
		case 1:
		{
			RectTransform[] array = transforms;
			Vector3 position4 = default(Vector3);
			foreach (RectTransform rectTransform in array)
			{
				Vector3 position = rectTransform.position;
				Vector3 position2 = rectTransform.position;
				int height = Screen.height;
				position4 = new Vector3(position.x, (float)(height + ((height >> 31) & 7) >> 3) + position2.y, rectTransform.position.z);
				rectTransform.position = position4;
			}
			break;
		}
		}
	}
}

