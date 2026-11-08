using TMPro;
using UnityEngine;

// Token: 0x02000054 RID: 84
public class TextUnderliner : MonoBehaviour
{
	// Token: 0x04000274 RID: 628 Offset: 0x20
	public TMP_Text text;

	// Token: 0x0600019F RID: 415 RVA: 0x00260AF0 File Offset: 0x0025F0F0
	public void Underline()
	{
		text.fontStyle = FontStyles.Underline;
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x00260B20 File Offset: 0x0025F120
	public void Ununderline()
	{
		text.fontStyle = FontStyles.Normal;
	}
}

