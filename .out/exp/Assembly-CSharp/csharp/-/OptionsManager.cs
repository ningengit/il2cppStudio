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
		slider.value = PlayerPrefs.GetFloat("MouseSensitivity");
		int num = PlayerPrefs.GetInt("Rumble");
		rumble.isOn = num == 1;
		if (PlayerPrefs.GetInt("AnalogMove") != 1)
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
		PlayerPrefs.SetFloat("MouseSensitivity", slider.value);
		PlayerPrefs.SetInt("Rumble", rumble.isOn ? 1 : 0);
		if (!analog.isOn)
		{
			PlayerPrefs.SetInt(NativeIntrinsics.Reinterpret<string>(NativeIntrinsics.Asm("rcx")), 0);
		}
		else
		{
			PlayerPrefs.SetInt(NativeIntrinsics.Reinterpret<string>(NativeIntrinsics.Asm("rcx")), 1);
		}
	}
}

