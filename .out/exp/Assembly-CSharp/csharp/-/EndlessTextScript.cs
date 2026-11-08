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
		TMP_Text tMP_Text = this.text;
		array[0] = this.text.text;
		array[1] = "\nHigh Score: ";
		object obj = PlayerPrefs.GetInt("HighBooks");
		array[2] = obj;
		array[3] = " Notebooks";
		tMP_Text.text = string.Concat(array);
	}
}

