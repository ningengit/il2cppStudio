using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using TMPro;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class DebugScript : MonoBehaviour
{
	// Token: 0x02000007 RID: 7
	private sealed class <LogDissappear>d__3 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x04000010 RID: 16 Offset: 0x10
		private int <>1__state;

		// Token: 0x04000011 RID: 17 Offset: 0x18
		private object <>2__current;

		// Token: 0x04000012 RID: 18 Offset: 0x20
		public DebugScript <>4__this;

		// Token: 0x04000013 RID: 19 Offset: 0x28
		private float <elapsed>5__2;

		// Token: 0x04000014 RID: 20 Offset: 0x2C
		private Color <color>5__3;

		private object System.Collections.Generic.IEnumerator<System.Object>.Current => <>2__current;

		private object System.Collections.IEnumerator.Current => <>2__current;

		// Token: 0x06000017 RID: 23 RVA: 0x00250B50 File Offset: 0x0024F150
		public <LogDissappear>d__3(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00250B40 File Offset: 0x0024F140
		private void System.IDisposable.Dispose()
		{
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00250DF0 File Offset: 0x0024F3F0
		private bool MoveNext()
		{
			int num = <>1__state;
			DebugScript debugScript = <>4__this;
			if (num - 1 != 0)
			{
				<>1__state = -1;
			}
			else
			{
				<>1__state = -1;
				<elapsed>5__2 = 0f;
				<color>5__3 = debugScript.errorText.color;
				TMP_Text errorText = debugScript.errorText;
				<color>5__3.a = 1f;
				Color color2 = default(Color);
				color2.r = NativeIntrinsics.Reinterpret<float>(<color>5__3);
				errorText.color = color2;
			}
			return false;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00251010 File Offset: 0x0024F610
		private void System.Collections.IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	// Token: 0x04000008 RID: 8 Offset: 0x20
	public bool limitFramerate;

	// Token: 0x04000009 RID: 9 Offset: 0x24
	public int framerate;

	// Token: 0x0400000A RID: 10 Offset: 0x28
	public TextMeshProUGUI fpsText;

	// Token: 0x0400000B RID: 11 Offset: 0x30
	public float deltaTime;

	// Token: 0x0400000C RID: 12 Offset: 0x38
	private TextMeshProUGUI keyText;

	// Token: 0x0400000D RID: 13 Offset: 0x40
	private TMP_Text errorText;

	// Token: 0x0400000E RID: 14 Offset: 0x48
	private AudioSource errorSource;

	// Token: 0x0400000F RID: 15 Offset: 0x50
	private Coroutine logDissappearCoroutine;

	// Token: 0x06000010 RID: 16 RVA: 0x002438E0 File Offset: 0x00241EE0
	private void OnEnable()
	{
		Application.logMessageReceived += LogError;
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00243850 File Offset: 0x00241E50
	private void OnDisable()
	{
		Application.logMessageReceived -= LogError;
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00243710 File Offset: 0x00241D10
	private void LogError(string logString, string stackTrace, LogType type)
	{
		if (type == LogType.Error || type == LogType.Exception)
		{
			errorText.text = logString + "\n" + stackTrace + "\n";
			if (!errorSource.isPlaying)
			{
				errorSource.Play();
			}
			if (logDissappearCoroutine != null)
			{
				StopCoroutine(logDissappearCoroutine);
			}
			<LogDissappear>d__3 <LogDissappear>d__ = new <LogDissappear>d__3(0);
			<LogDissappear>d__.<>1__state = 0;
			<LogDissappear>d__.<>4__this = this;
			logDissappearCoroutine = StartCoroutine(<LogDissappear>d__);
		}
	}

	// Token: 0x06000013 RID: 19 RVA: 0x002436A0 File Offset: 0x00241CA0
	private IEnumerator LogDissappear()
	{
		<LogDissappear>d__3 <LogDissappear>d__ = new <LogDissappear>d__3(0);
		<LogDissappear>d__.<>1__state = 0;
		<LogDissappear>d__.<>4__this = this;
		return <LogDissappear>d__;
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00243970 File Offset: 0x00241F70
	private void Start()
	{
		if (limitFramerate)
		{
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = framerate;
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x002439D0 File Offset: 0x00241FD0
	public void Update()
	{
		if (Time.deltaTime != 0f)
		{
			float num = deltaTime;
			float num2 = (deltaTime = (Time.deltaTime - deltaTime) * 0.1f + num);
			TextMeshProUGUI textMeshProUGUI = fpsText;
			float num3 = default(float);
			num3.m_value = (float)Math.Ceiling(1f / num2);
			textMeshProUGUI.text = num3 + " :FPS";
		}
		Enum.GetValues(Type.GetTypeFromHandle(NativeIntrinsics.Reinterpret<RuntimeTypeHandle>((nint)NativeIntrinsics.Asm("typeof(KeyCode)")))).GetEnumerator();
		long arg2 = default(long);
		long arg4 = default(long);
		while (true)
		{
			long arg = NativeIntrinsics.Asm("stack[0x20]");
			NativeIntrinsics.Asm("*(LocalExpr + 0x0)");
			int num4 = (int)NativeIntrinsics.Asm("*(*(LocalExpr + 0x0) + 0x12E)");
			if (0 < (uint)NativeIntrinsics.Asm("*(*(LocalExpr + 0x0) + 0x12E)"))
			{
				int num5 = 0;
				arg2 = NativeIntrinsics.Asm("*(LocalExpr + 0xB0)");
				do
				{
					num5++;
				}
				while ((uint)(num5 + 1) < (uint)num4);
			}
			NativeIntrinsics.CallNative("GetInterfaceInvokeDataFromVTableSlowPath", arg, 0L, 0L, 0L);
			long num6 = NativeIntrinsics.CallNative("indirect@243BFD", arg, NativeIntrinsics.Asm("*(LocalExpr + 0x8)"), arg2, 0L);
			if (((int)num6 & (int)num6) == 0)
			{
				break;
			}
			arg = NativeIntrinsics.Asm("stack[0x20]");
			NativeIntrinsics.Asm("*(LocalExpr + 0x0)");
			num4 = (int)NativeIntrinsics.Asm("*(*(LocalExpr + 0x0) + 0x12E)");
			if (0 < (uint)NativeIntrinsics.Asm("*(*(LocalExpr + 0x0) + 0x12E)"))
			{
				int num5 = 0;
				arg2 = NativeIntrinsics.Asm("*(LocalExpr + 0xB0)");
				do
				{
					num5++;
				}
				while ((uint)(num5 + 1) < (uint)num4);
			}
			NativeIntrinsics.CallNative("GetInterfaceInvokeDataFromVTableSlowPath", arg, 0L, 1L, 0L);
			long arg3 = NativeIntrinsics.CallNative("indirect@243C70", arg, NativeIntrinsics.Asm("*(LocalExpr + 0x8)"), 0L, 0L);
			arg4 = NativeIntrinsics.Asm("*(LocalExpr + 0x0)");
			NativeIntrinsics.CallNative("il2cpp_object_unbox", arg3, arg4, 0L, 0L);
			bool keyDownInt = Input.GetKeyDownInt((KeyCode)NativeIntrinsics.Asm("*(LocalExpr + 0x0)"));
			NativeIntrinsics.Asm("*(LocalExpr + 0x0)");
			if (keyDownInt)
			{
				keyText.text = NativeIntrinsics.Reinterpret<Enum>((nint)NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFD0]")).ToString() + " :Input";
				NativeIntrinsics.CallNative("sub_23E000", NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFC0]"), 0L, 0L, 0L);
				return;
			}
		}
		NativeIntrinsics.CallNative("sub_23E000", NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFC0]"), arg4, arg2, 0L);
	}
}

