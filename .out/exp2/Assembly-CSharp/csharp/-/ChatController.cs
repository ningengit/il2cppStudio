using System;
using Il2CppStudio.Native;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000057 RID: 87
public class ChatController : MonoBehaviour
{
	// Token: 0x04000277 RID: 631 Offset: 0x20
	public TMP_InputField TMP_ChatInput;

	// Token: 0x04000278 RID: 632 Offset: 0x28
	public TMP_Text TMP_ChatOutput;

	// Token: 0x04000279 RID: 633 Offset: 0x30
	public Scrollbar ChatScrollbar;

	// Token: 0x060001A7 RID: 423 RVA: 0x00255030 File Offset: 0x00253630
	private void OnEnable()
	{
		TMP_ChatInput.m_OnSubmit.AddListener(new UnityAction<string>(null, (IntPtr)0));
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x00254F80 File Offset: 0x00253580
	private void OnDisable()
	{
		TMP_ChatInput.m_OnSubmit.RemoveListener(new UnityAction<string>(null, (IntPtr)0));
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x00254C80 File Offset: 0x00253280
	private void AddToChatOutput(string newText)
	{
		TMP_ChatInput.text = string.Empty;
		DateTime now = DateTime.Now;
		string[] array = new string[10];
		TMP_Text tMP_ChatOutput = TMP_ChatOutput;
		array[0] = TMP_ChatOutput.text;
		array[1] = "[<#FFFF80>";
		DateTime dateTime = default(DateTime);
		dateTime._dateData = NativeIntrinsics.Reinterpret<ulong>(now);
		int num = default(int);
		num.m_value = dateTime.Hour;
		array[2] = num.ToString("d2");
		array[3] = ":";
		int num2 = default(int);
		num2.m_value = dateTime.Minute;
		array[4] = num2.ToString("d2");
		array[5] = ":";
		int num3 = default(int);
		num3.m_value = dateTime.Second;
		array[6] = num3.ToString("d2");
		array[7] = "</color>] ";
		array[8] = newText;
		array[9] = "\n";
		tMP_ChatOutput.text = string.Concat(array);
		TMP_ChatInput.ActivateInputField();
		ChatScrollbar.value = 0f;
	}
}

