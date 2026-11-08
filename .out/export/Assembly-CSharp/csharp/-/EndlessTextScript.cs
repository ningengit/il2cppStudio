using Il2CppStudio.Native;
using TMPro;
using UnityEngine;

// Token: 0x02000026 RID: 38
public class EndlessTextScript : MonoBehaviour
{
	// Token: 0x04000102 RID: 258 Offset: 0x20
	public TMP_Text text;

	// Token: 0x060000CB RID: 203 RVA: 0x00244760 File Offset: 0x00242D60
	private void Start()
	{
		object[] array = new object[4];
		object[] array2 = array;
		TMP_Text tMP_Text = this.text;
		object[] array3 = array;
		object[] array4 = array3;
		if (array3 != null)
		{
			long num = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(array3), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
		}
		array2[0] = array4;
		if ("\nHigh Score: " != null)
		{
			long num2 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("\nHigh Score: "), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
			long num3 = num2;
		}
		array2[1] = "\nHigh Score: ";
		int num4 = PlayerPrefs.GetInt("HighBooks");
		object obj = num4;
		object obj2 = obj;
		object value = obj;
		if (obj != null)
		{
			long num5 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
		}
		array2[2] = obj2;
		if (" Notebooks" != null)
		{
			long num6 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(" Notebooks"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
			long num7 = num6;
		}
		array2[3] = " Notebooks";
		string text = string.Concat(array2);
	}
}

