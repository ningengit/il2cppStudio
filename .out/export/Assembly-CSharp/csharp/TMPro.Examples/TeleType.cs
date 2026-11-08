using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using Mono.Xml;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x02000074 RID: 116
public class TeleType : MonoBehaviour
{
	// Token: 0x02000075 RID: 117
	private sealed class <Start>d__4 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x040002FB RID: 763 Offset: 0x10
		private int <>1__state;

		// Token: 0x040002FC RID: 764 Offset: 0x18
		private object <>2__current;

		// Token: 0x040002FD RID: 765 Offset: 0x20
		public TeleType <>4__this;

		// Token: 0x040002FE RID: 766 Offset: 0x28
		private int <totalVisibleCharacters>5__2;

		// Token: 0x040002FF RID: 767 Offset: 0x2C
		private int <counter>5__3;

		private object System.Collections.Generic.IEnumerator<System.Object>.Current => <>2__current;

		private object System.Collections.IEnumerator.Current => <>2__current;

		// Token: 0x0600020D RID: 525 RVA: 0x00250B50 File Offset: 0x0024F150
		public <Start>d__4(int <>1__state)
		{
			((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
			this.<>1__state = <>1__state;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x00250B40 File Offset: 0x0024F140
		private void System.IDisposable.Dispose()
		{
		}

		// Token: 0x0600020F RID: 527 RVA: 0x00262880 File Offset: 0x00260E80
		private bool MoveNext()
		{
			TeleType teleType = <>4__this;
			int num = <>1__state;
			if (<>1__state != 0)
			{
				num--;
				if (num - 1 == 0)
				{
					<>1__state = -1;
					string label = teleType.label02;
					WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
					<>2__current = waitForSeconds;
					<>1__state = 2;
					return true;
				}
				num--;
				if (num - 1 == 0)
				{
					<>1__state = -1;
					string label2 = teleType.label01;
					WaitForSeconds waitForSeconds2 = new WaitForSeconds(1f);
					<>2__current = waitForSeconds2;
					<>1__state = 3;
					return true;
				}
				num--;
				if (num - 1 == 0)
				{
					<>1__state = -1;
					goto IL_0113;
				}
				if (num != 1)
				{
					return false;
				}
				<>1__state = -1;
			}
			else
			{
				<>1__state = -1;
				<totalVisibleCharacters>5__2 = teleType.m_textMeshPro.m_textInfo.characterCount;
				<counter>5__3 = 0;
			}
			teleType.m_textMeshPro.maxVisibleCharacters = (int)NativeIntrinsics.Asm("rdx");
			if ((int)NativeIntrinsics.Asm("rdx") >= <totalVisibleCharacters>5__2)
			{
				WaitForSeconds waitForSeconds3 = new WaitForSeconds(1f);
				<>2__current = waitForSeconds3;
				<>1__state = 1;
				return true;
			}
			goto IL_0113;
			IL_0113:
			<counter>5__3++;
			WaitForSeconds waitForSeconds4 = new WaitForSeconds(0.05f);
			<>2__current = waitForSeconds4;
			<>1__state = 4;
			return true;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00262DC0 File Offset: 0x002613C0
		private void System.Collections.IEnumerator.Reset()
		{
			long num = NativeIntrinsics.CallNative("sub_184BE0", NativeIntrinsics.Asm("rax"), 0L, 0L, 0L);
			NativeIntrinsics.Reinterpret<NotSupportedException>(num)..ctor();
			throw null;
		}
	}

	// Token: 0x040002F8 RID: 760 Offset: 0x20
	private string label01 = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=1>";

	// Token: 0x040002F9 RID: 761 Offset: 0x28
	private string label02 = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=2>";

	// Token: 0x040002FA RID: 762 Offset: 0x30
	private TMP_Text m_textMeshPro;

	// Token: 0x0600020A RID: 522 RVA: 0x0025FA60 File Offset: 0x0025E060
	private void Awake()
	{
		TMP_Text component = GetComponent<TMP_Text>();
		m_textMeshPro = component;
		m_textMeshPro.enableWordWrapping = true;
		m_textMeshPro.alignment = NativeIntrinsics.Reinterpret<TextAlignmentOptions>(258);
	}

	// Token: 0x0600020B RID: 523 RVA: 0x0025FB00 File Offset: 0x0025E100
	private IEnumerator Start()
	{
		<Start>d__4 <Start>d__ = NativeIntrinsics.Reinterpret<<Start>d__4>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.TeleType+<Start>d__4)"));
		((Mono.Xml.SecurityParser)(object)<Start>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<Start>d__.<>1__state = 0;
		<Start>d__.<>4__this = this;
		return <Start>d__;
	}
}

