using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace TMPro;

// Token: 0x0200005D RID: 93
[Serializable]
public class TMP_PhoneNumberValidator : TMP_InputValidator
{
	// Token: 0x060001B8 RID: 440 RVA: 0x0025A4C0 File Offset: 0x00258AC0
	public override char Validate(ref string text, ref int pos, char ch)
	{
		Debug.Log("Trying to validate...");
		return '\0';
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x00259B40 File Offset: 0x00258140
	public TMP_PhoneNumberValidator()
	{
		((StyleSheet)(object)this)..ctor();
	}
}

