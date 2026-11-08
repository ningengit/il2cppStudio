using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using Mono.Xml;
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
			((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
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
			num--;
			if (num - 1 != 0)
			{
				<>1__state = -1;
			}
			else
			{
				<>1__state = -1;
				<elapsed>5__2 = 0f;
				TMP_Text errorText = debugScript.errorText;
				<color>5__3 = NativeIntrinsics.Reinterpret<Color>(NativeIntrinsics.Asm("*(LocalExpr->klass + 0x0)"));
				TMP_Text errorText2 = debugScript.errorText;
				<color>5__3.a = 1f;
				TMP_Text tMP_Text = errorText2;
			}
			return false;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00251010 File Offset: 0x0024F610
		private void System.Collections.IEnumerator.Reset()
		{
			long num = NativeIntrinsics.CallNative("sub_184BE0", NativeIntrinsics.Asm("rax"), 0L, 0L, 0L);
			NativeIntrinsics.Reinterpret<NotSupportedException>(num)..ctor();
			throw null;
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
		Application.LogCallback logCallback = LogError;
		Application.LogCallback value = logCallback;
		Application.logMessageReceived += value;
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00243850 File Offset: 0x00241E50
	private void OnDisable()
	{
		Application.LogCallback logCallback = LogError;
		Application.LogCallback value = logCallback;
		Application.logMessageReceived -= value;
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00243710 File Offset: 0x00241D10
	private void LogError(string logString, string stackTrace, LogType type)
	{
		int num = NativeIntrinsics.Reinterpret<int>(type);
		if (NativeIntrinsics.Reinterpret<int>(type) == 0 || num == 4)
		{
			TMP_Text tMP_Text = errorText;
			string text = logString + "\n" + stackTrace + "\n";
			string text2 = text;
			if (!errorSource.isPlaying)
			{
				errorSource.Play();
			}
			if (logDissappearCoroutine != null)
			{
				StopCoroutine(logDissappearCoroutine);
			}
			<LogDissappear>d__3 <LogDissappear>d__ = NativeIntrinsics.Reinterpret<<LogDissappear>d__3>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(DebugScript+<LogDissappear>d__3)"));
			((Mono.Xml.SecurityParser)(object)<LogDissappear>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
			<LogDissappear>d__.<>1__state = 0;
			<LogDissappear>d__.<>4__this = this;
			Coroutine coroutine = StartCoroutine(<LogDissappear>d__);
			logDissappearCoroutine = coroutine;
		}
	}

	// Token: 0x06000013 RID: 19 RVA: 0x002436A0 File Offset: 0x00241CA0
	private IEnumerator LogDissappear()
	{
		<LogDissappear>d__3 <LogDissappear>d__ = NativeIntrinsics.Reinterpret<<LogDissappear>d__3>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(DebugScript+<LogDissappear>d__3)"));
		((Mono.Xml.SecurityParser)(object)<LogDissappear>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
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
			int targetFrameRate = framerate;
			Application.targetFrameRate = targetFrameRate;
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x002439D0 File Offset: 0x00241FD0
	public void Update()
	{
		float num = Time.deltaTime;
		float num2 = num;
		if (num != 0f)
		{
			float num3 = deltaTime;
			float num4 = Time.deltaTime;
			float num5 = (num4 - deltaTime) * 0.1f + num3;
			deltaTime = num5;
			TextMeshProUGUI textMeshProUGUI = fpsText;
			long num6 = NativeIntrinsics.CallNative("sub_20C150", 0L, 0L, 0L, 0L);
			float num7 = default(float);
			num7.m_value = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("xmm0"));
			string text = num7.ToString();
			string text2 = text + " :FPS";
			string text3 = text2;
		}
		Type typeFromHandle = Type.GetTypeFromHandle(NativeIntrinsics.Reinterpret<RuntimeTypeHandle>((nint)NativeIntrinsics.Asm("typeof(KeyCode)")));
		Type enumType = typeFromHandle;
		Array values = Enum.GetValues(enumType);
		IEnumerator enumerator = values.GetEnumerator();
		long num18 = default(long);
		long num22;
		while (true)
		{
			long arg = NativeIntrinsics.Asm("stack[0x20]");
			long num8 = NativeIntrinsics.Asm("*(LocalExpr + 0x0)");
			int num9 = (int)NativeIntrinsics.Asm("*(*(LocalExpr + 0x0) + 0x12E)");
			if (0 < (uint)NativeIntrinsics.Asm("*(*(LocalExpr + 0x0) + 0x12E)"))
			{
				long num10 = NativeIntrinsics.Asm("*(LocalExpr + 0xB0)");
				do
				{
					int num11 = 0 + 1;
				}
				while (1u < (uint)num9);
			}
			long num12 = NativeIntrinsics.CallNative("sub_17A5F0", arg, 0L, 0L, 0L);
			long num13 = num12;
			long num14 = NativeIntrinsics.Asm("*(LocalExpr + 0x8)");
			if (((int)num13 & (int)num13) == 0)
			{
				break;
			}
			arg = NativeIntrinsics.Asm("stack[0x20]");
			num8 = NativeIntrinsics.Asm("*(LocalExpr + 0x0)");
			int num15 = (int)NativeIntrinsics.Asm("*(*(LocalExpr + 0x0) + 0x12E)");
			if (0 < (uint)NativeIntrinsics.Asm("*(*(LocalExpr + 0x0) + 0x12E)"))
			{
				long num10 = NativeIntrinsics.Asm("*(LocalExpr + 0xB0)");
				do
				{
					int num11 = 0 + 1;
				}
				while (1u < (uint)num15);
			}
			long num16 = NativeIntrinsics.CallNative("sub_17A5F0", arg, 0L, 1L, 0L);
			num13 = num16;
			long num17 = arg;
			num18 = NativeIntrinsics.Asm("*(LocalExpr + 0x8)");
			num17 = num13;
			num18 = NativeIntrinsics.Asm("*(LocalExpr + 0x0)");
			long num19 = NativeIntrinsics.CallNative("il2cpp_object_unbox", num17, num18, 0L, 0L);
			bool keyDownInt = Input.GetKeyDownInt(NativeIntrinsics.Reinterpret<KeyCode>((int)NativeIntrinsics.Asm("*(LocalExpr + 0x0)")));
			int num20 = (int)NativeIntrinsics.Asm("*(LocalExpr + 0x0)");
			if (keyDownInt)
			{
				TextMeshProUGUI textMeshProUGUI2 = keyText;
				string text4 = NativeIntrinsics.Reinterpret<Enum>((nint)NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFD0]")).ToString();
				string text5 = text4 + " :Input";
				string value = text5;
				long num21 = NativeIntrinsics.CallNative("sub_23E000", NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFC0]"), NativeIntrinsics.Reinterpret<long>(value), 0L, 0L);
				num22 = num21;
				return;
			}
		}
		long num23 = NativeIntrinsics.CallNative("sub_23E000", NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFC0]"), num18, 0L, 0L);
		num22 = num23;
	}
}

