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
		TMP_InputField.SubmitEvent onSubmit = TMP_ChatInput.m_OnSubmit;
		UnityAction<string> call = (UnityAction<string>)(object)new AsyncCallback(AddToChatOutput);
		onSubmit.AddListener(call);
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x00254F80 File Offset: 0x00253580
	private void OnDisable()
	{
		TMP_InputField.SubmitEvent onSubmit = TMP_ChatInput.m_OnSubmit;
		UnityAction<string> call = (UnityAction<string>)(object)new AsyncCallback(AddToChatOutput);
		onSubmit.RemoveListener(call);
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x00254C80 File Offset: 0x00253280
	private void AddToChatOutput(string newText)
	{
		TMP_InputField tMP_ChatInput = TMP_ChatInput;
		tMP_ChatInput.text = NativeIntrinsics.Reinterpret<string>(NativeIntrinsics.Asm("*(/*typedef#-1*/object.static_fields + 0x0)"));
		DateTime now = DateTime.Now;
		string[] array = new string[10];
		TMP_Text tMP_ChatOutput = TMP_ChatOutput;
		array[0] = (string)(object)array;
		array[1] = "[<#FFFF80>";
		DateTime dateTime = default(DateTime);
		dateTime._dateData = NativeIntrinsics.Reinterpret<ulong>(now);
		int hour = dateTime.Hour;
		int num = default(int);
		num.m_value = hour;
		string text = num.ToString("d2");
		array[2] = text;
		array[3] = ":";
		DateTime dateTime2 = default(DateTime);
		dateTime2._dateData = NativeIntrinsics.Reinterpret<ulong>(now);
		int minute = dateTime2.Minute;
		int num2 = default(int);
		num2.m_value = minute;
		string text2 = num2.ToString("d2");
		array[4] = text2;
		array[5] = ":";
		DateTime dateTime3 = default(DateTime);
		dateTime3._dateData = NativeIntrinsics.Reinterpret<ulong>(now);
		int second = dateTime3.Second;
		int num3 = default(int);
		num3.m_value = second;
		string text3 = num3.ToString("d2");
		array[6] = text3;
		array[7] = "</color>] ";
		array[8] = newText;
		array[9] = "\n";
		string text4 = string.Concat(array);
		TMP_ChatInput.ActivateInputField();
		ChatScrollbar.value = 0f;
	}
}

