using System;
using Il2CppStudio.Native;
using UnityEngine.UIElements;

namespace TMPro;

// Token: 0x0200005C RID: 92
[Serializable]
public class TMP_DigitValidator : TMP_InputValidator
{
	// Token: 0x060001B6 RID: 438 RVA: 0x00259B20 File Offset: 0x00258120
	public override char Validate(ref string text, ref int pos, char ch)
	{
		if ((uint)((int)ch + -48L) > 9)
		{
			return '\0';
		}
		NativeIntrinsics.Stmt("inc dword ptr [r8] ; value = BinaryExpr");
		return ch;
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x00259B40 File Offset: 0x00258140
	public TMP_DigitValidator()
	{
		((StyleSheet)(object)this)..ctor();
	}
}

