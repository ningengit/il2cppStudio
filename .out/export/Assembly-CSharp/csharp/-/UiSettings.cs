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
		if (sAuto.m_IsOn || sXLarge.m_IsOn || sLarge.m_IsOn || sMed.m_IsOn || sSmall.m_IsOn)
		{
			PlayerPrefs.SetInt("UiSize", 0);
		}
		if (!hLow.m_IsOn)
		{
			if (!hMed.m_IsOn)
			{
				Toggle toggle = hHigh;
				if (toggle.m_IsOn)
				{
					PlayerPrefs.SetInt("UiHeight", 2);
				}
			}
			else
			{
				PlayerPrefs.SetInt("UiHeight", 1);
			}
		}
		else
		{
			PlayerPrefs.SetInt("UiHeight", 0);
		}
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00251E60 File Offset: 0x00250460
	public void RestoreState()
	{
		int num = PlayerPrefs.GetInt("UiSize");
		size = num;
		int num2 = (height = PlayerPrefs.GetInt("UiHeight"));
		Toggle toggle;
		if (size != 0)
		{
			if (size != 1)
			{
				if (size != 2)
				{
					if (size != 3)
					{
						if (size != 4)
						{
							goto IL_009d;
						}
						toggle = sSmall;
					}
					else
					{
						toggle = sMed;
					}
				}
				else
				{
					toggle = sLarge;
				}
			}
			else
			{
				toggle = sXLarge;
			}
		}
		else
		{
			toggle = sAuto;
		}
		toggle.isOn = true;
		goto IL_009d;
		IL_009d:
		if (height != 0)
		{
			if (height != 1)
			{
				if (height != 2)
				{
					return;
				}
				toggle = hHigh;
			}
			else
			{
				toggle = hMed;
			}
		}
		else
		{
			toggle = hLow;
		}
		toggle.isOn = true;
	}
}

