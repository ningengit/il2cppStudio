using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000021 RID: 33
public class MouseSliderScript : MonoBehaviour
{
	// Token: 0x040000E8 RID: 232 Offset: 0x20
	public Slider slider;

	// Token: 0x060000B5 RID: 181 RVA: 0x0024DE30 File Offset: 0x0024C430
	private void Start()
	{
		if (100f > PlayerPrefs.GetFloat("MouseSensitivity"))
		{
			PlayerPrefs.SetFloat("MouseSensitivity", 200f);
		}
		slider.value = PlayerPrefs.GetFloat("MouseSensitivity");
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x0024DEC0 File Offset: 0x0024C4C0
	private void Update()
	{
		PlayerPrefs.SetFloat("MouseSensitivity", slider.value);
	}
}

