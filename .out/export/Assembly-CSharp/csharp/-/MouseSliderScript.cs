using Il2CppStudio.Native;
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
		float num = PlayerPrefs.GetFloat("MouseSensitivity");
		float num2 = num;
		if (100f > num)
		{
			PlayerPrefs.SetFloat("MouseSensitivity", 200f);
		}
		Slider slider = this.slider;
		float num3 = PlayerPrefs.GetFloat("MouseSensitivity");
		throw null;
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x0024DEC0 File Offset: 0x0024C4C0
	private void Update()
	{
		PlayerPrefs.SetFloat("MouseSensitivity", NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("xmm0")));
	}
}

