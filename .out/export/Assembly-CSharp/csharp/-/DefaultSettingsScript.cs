using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using Mono.Globalization.Unicode;
using Mono.Xml;
using UnityEngine;

// Token: 0x0200001F RID: 31
public class DefaultSettingsScript : MonoBehaviour
{
	// Token: 0x02000020 RID: 32
	private sealed class <CloseOptions>d__1 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x040000E5 RID: 229 Offset: 0x10
		private int <>1__state;

		// Token: 0x040000E6 RID: 230 Offset: 0x18
		private object <>2__current;

		// Token: 0x040000E7 RID: 231 Offset: 0x20
		public DefaultSettingsScript <>4__this;

		private object System.Collections.Generic.IEnumerator<System.Object>.Current => <>2__current;

		private object System.Collections.IEnumerator.Current => <>2__current;

		// Token: 0x060000AF RID: 175 RVA: 0x00250B50 File Offset: 0x0024F150
		public <CloseOptions>d__1(int <>1__state)
		{
			((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
			this.<>1__state = <>1__state;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00250B40 File Offset: 0x0024F140
		private void System.IDisposable.Dispose()
		{
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00250CE0 File Offset: 0x0024F2E0
		private bool MoveNext()
		{
			int num = <>1__state;
			if (<>1__state == 0)
			{
				<>1__state = -1;
				WaitForEndOfFrame waitForEndOfFrame = (WaitForEndOfFrame)(object)new Mono.Globalization.Unicode.ContractionComparer();
				<>2__current = waitForEndOfFrame;
				<>1__state = 1;
				return true;
			}
			if (num == 1)
			{
				DefaultSettingsScript defaultSettingsScript = <>4__this;
				<>1__state = -1;
				defaultSettingsScript.canvas.enabled = (byte)num != 0;
				defaultSettingsScript.options.SetActive(value: false);
			}
			return false;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00250DB0 File Offset: 0x0024F3B0
		private void System.Collections.IEnumerator.Reset()
		{
			long num = NativeIntrinsics.CallNative("sub_184BE0", NativeIntrinsics.Asm("rax"), 0L, 0L, 0L);
			NativeIntrinsics.Reinterpret<NotSupportedException>(num)..ctor();
			throw null;
		}
	}

	// Token: 0x040000E3 RID: 227 Offset: 0x20
	public GameObject options;

	// Token: 0x040000E4 RID: 228 Offset: 0x28
	public Canvas canvas;

	// Token: 0x060000AC RID: 172 RVA: 0x00243E30 File Offset: 0x00242430
	private void Start()
	{
		bool flag = PlayerPrefs.HasKey("OptionsSet");
		bool flag2 = flag;
		if (!flag)
		{
			options.SetActive(value: true);
			<CloseOptions>d__1 <CloseOptions>d__ = NativeIntrinsics.Reinterpret<<CloseOptions>d__1>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(DefaultSettingsScript+<CloseOptions>d__1)"));
			((Mono.Xml.SecurityParser)(object)<CloseOptions>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
			<CloseOptions>d__.<>1__state = 0;
			<CloseOptions>d__.<>4__this = this;
			Coroutine coroutine = StartCoroutine(<CloseOptions>d__);
			canvas.enabled = false;
		}
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00243DC0 File Offset: 0x002423C0
	public IEnumerator CloseOptions()
	{
		<CloseOptions>d__1 <CloseOptions>d__ = NativeIntrinsics.Reinterpret<<CloseOptions>d__1>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(DefaultSettingsScript+<CloseOptions>d__1)"));
		((Mono.Xml.SecurityParser)(object)<CloseOptions>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<CloseOptions>d__.<>1__state = 0;
		<CloseOptions>d__.<>4__this = this;
		return <CloseOptions>d__;
	}
}

