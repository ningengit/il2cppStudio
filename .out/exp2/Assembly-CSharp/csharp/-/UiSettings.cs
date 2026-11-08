using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000025 RID: 37
public class UiSettings : MonoBehaviour
{
	// Token: 0x040000F8 RID: 248 Offset: 0x20
	public Toggle sAuto;

	// Token: 0x040000F9 RID: 249 Offset: 0x28
	public Toggle sXLarge;

	// Token: 0x040000FA RID: 250 Offset: 0x30
	public Toggle sLarge;

	// Token: 0x040000FB RID: 251 Offset: 0x38
	public Toggle sMed;

	// Token: 0x040000FC RID: 252 Offset: 0x40
	public Toggle sSmall;

	// Token: 0x040000FD RID: 253 Offset: 0x48
	public Toggle hLow;

	// Token: 0x040000FE RID: 254 Offset: 0x50
	public Toggle hMed;

	// Token: 0x040000FF RID: 255 Offset: 0x58
	public Toggle hHigh;

	// Token: 0x04000100 RID: 256 Offset: 0x60
	private int size;

	// Token: 0x04000101 RID: 257 Offset: 0x64
	private int height;

	// Token: 0x060000C8 RID: 200 RVA: 0x00251F40 File Offset: 0x00250540
	public void UpdateState()
	{
		int value;
		if (sAuto.m_IsOn)
		{
			value = 0;
		}
		else if (sXLarge.m_IsOn)
		{
			value = 1;
		}
		else if (sLarge.m_IsOn)
		{
			value = 2;
		}
		else if (sMed.m_IsOn)
		{
			value = 3;
		}
		else if (sSmall.m_IsOn)
		{
			value = 4;
		}
		else
		{
			goto IL_00b1;
		}
		PlayerPrefs.SetInt("UiSize", value);
		goto IL_00b1;
		IL_00b1:
		if (hLow.m_IsOn)
		{
			PlayerPrefs.SetInt("UiHeight", 0);
		}
		else if (hMed.m_IsOn)
		{
			PlayerPrefs.SetInt("UiHeight", 1);
		}
		else if (hHigh.m_IsOn)
		{
			PlayerPrefs.SetInt("UiHeight", 2);
		}
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00251E60 File Offset: 0x00250460
	public void RestoreState()
	{
		size = PlayerPrefs.GetInt("UiSize");
		height = PlayerPrefs.GetInt("UiHeight");
		Toggle toggle;
		if (size == 0)
		{
			toggle = sAuto;
		}
		else if (size == 1)
		{
			toggle = sXLarge;
		}
		else if (size == 2)
		{
			toggle = sLarge;
		}
		else if (size == 3)
		{
			toggle = sMed;
		}
		else if (size == 4)
		{
			toggle = sSmall;
		}
		else
		{
			goto IL_00c5;
		}
		toggle.isOn = true;
		goto IL_00c5;
		IL_00c5:
		if (height == 0)
		{
			toggle = hLow;
		}
		else if (height == 1)
		{
			toggle = hMed;
		}
		else if (height == 2)
		{
			toggle = hHigh;
		}
		else
		{
			return;
		}
		toggle.isOn = true;
	}
}

