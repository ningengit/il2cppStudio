using Il2CppStudio.Native;
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
		int num3 = num2;
		int num4 = num;
		int num5 = num2;
		if (num == 1 || num4 == 2 || num4 == 3 || num4 == 4)
		{
			normScaler.referenceResolution = NativeIntrinsics.Reinterpret<Vector2>(NativeIntrinsics.Asm("unpcklps xmm0,xmm1"));
		}
		switch (num3)
		{
		case 2:
		{
			RectTransform[] array = transforms;
			Vector3 position8 = default(Vector3);
			while (0 < array.Length)
			{
				RectTransform rectTransform2 = array[0];
				Vector3 position5 = rectTransform2.position;
				Vector3 position6 = rectTransform2.position;
				int height2 = Screen.height;
				Vector3 position7 = rectTransform2.position;
				position8.x = position5.x;
				position8.y = (float)(height2 + ((int)NativeIntrinsics.Asm("rdx") & 3) >> 2) + position6.y;
				position8.z = position7.z;
				rectTransform2.position = position8;
				num4 = 0 + 1;
			}
			break;
		}
		case 1:
		{
			RectTransform[] array = default(RectTransform[]);
			RectTransform rectTransform = array[2];
			Vector3 position4 = default(Vector3);
			while (0 < (int)NativeIntrinsics.Asm("*(LocalExpr + 0x18)"))
			{
				Vector3 position = NativeIntrinsics.Reinterpret<Transform>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)")).position;
				Vector3 position2 = NativeIntrinsics.Reinterpret<Transform>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)")).position;
				int height = Screen.height;
				Vector3 position3 = NativeIntrinsics.Reinterpret<Transform>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)")).position;
				Transform obj = NativeIntrinsics.Reinterpret<Transform>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)"));
				position4.x = position.x;
				position4.y = (float)(height + ((int)NativeIntrinsics.Asm("rdx") & 7) >> 3) + position2.y;
				position4.z = position3.z;
				obj.position = position4;
				num4 = 0 + 1;
			}
			break;
		}
		}
	}
}

