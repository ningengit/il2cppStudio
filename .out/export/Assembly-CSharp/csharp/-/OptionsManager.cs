using Il2CppStudio.Native;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000022 RID: 34
public class OptionsManager : MonoBehaviour
{
	// Token: 0x040000E9 RID: 233 Offset: 0x20
	public Slider slider;

	// Token: 0x040000EA RID: 234 Offset: 0x28
	public Toggle rumble;

	// Token: 0x040000EB RID: 235 Offset: 0x30
	public Toggle analog;

	// Token: 0x060000B8 RID: 184 RVA: 0x0024E410 File Offset: 0x0024CA10
	private void Start()
	{
		if (!PlayerPrefs.HasKey("OptionsSet"))
		{
			PlayerPrefs.SetInt("OptionsSet", 1);
			return;
		}
		Slider slider = this.slider;
		float num = PlayerPrefs.GetFloat("MouseSensitivity");
		int num2 = PlayerPrefs.GetInt("Rumble");
		rumble.isOn = num2 == 1;
		int num3 = PlayerPrefs.GetInt("AnalogMove");
		if (num3 != 1)
		{
			NativeIntrinsics.Reinterpret<Toggle>(NativeIntrinsics.Asm("rcx")).isOn = false;
		}
		else
		{
			NativeIntrinsics.Reinterpret<Toggle>(NativeIntrinsics.Asm("rcx")).isOn = true;
		}
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x0024E530 File Offset: 0x0024CB30
	private void Update()
	{
		Slider slider = this.slider;
		PlayerPrefs.SetFloat("MouseSensitivity", NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("xmm0")));
		if (!rumble.m_IsOn)
		{
		}
		PlayerPrefs.SetInt("Rumble", 1);
		if (!analog.m_IsOn)
		{
			PlayerPrefs.SetInt(NativeIntrinsics.Reinterpret<string>(NativeIntrinsics.Asm("rcx")), 0);
		}
		else
		{
			PlayerPrefs.SetInt(NativeIntrinsics.Reinterpret<string>(NativeIntrinsics.Asm("rcx")), 1);
		}
	}
}

