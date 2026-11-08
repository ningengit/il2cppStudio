using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using Mono.Xml;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x02000076 RID: 118
public class TextConsoleSimulator : MonoBehaviour
{
	// Token: 0x02000077 RID: 119
	private sealed class <RevealCharacters>d__7 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x04000302 RID: 770 Offset: 0x10
		private int <>1__state;

		// Token: 0x04000303 RID: 771 Offset: 0x18
		private object <>2__current;

		// Token: 0x04000304 RID: 772 Offset: 0x20
		public TMP_Text textComponent;

		// Token: 0x04000305 RID: 773 Offset: 0x28
		public TextConsoleSimulator <>4__this;

		// Token: 0x04000306 RID: 774 Offset: 0x30
		private TMP_TextInfo <textInfo>5__2;

		// Token: 0x04000307 RID: 775 Offset: 0x38
		private int <totalVisibleCharacters>5__3;

		// Token: 0x04000308 RID: 776 Offset: 0x3C
		private int <visibleCount>5__4;

		private object System.Collections.Generic.IEnumerator<System.Object>.Current => <>2__current;

		private object System.Collections.IEnumerator.Current => <>2__current;

		// Token: 0x0600021B RID: 539 RVA: 0x00250B50 File Offset: 0x0024F150
		public <RevealCharacters>d__7(int <>1__state)
		{
			((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
			this.<>1__state = <>1__state;
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00250B40 File Offset: 0x0024F140
		private void System.IDisposable.Dispose()
		{
		}

		// Token: 0x0600021D RID: 541 RVA: 0x002619A0 File Offset: 0x0025FFA0
		private bool MoveNext()
		{
			TextConsoleSimulator textConsoleSimulator = <>4__this;
			int num = <>1__state;
			if (<>1__state != 0)
			{
				num--;
				if (num - 1 == 0)
				{
					<>1__state = -1;
					<visibleCount>5__4 = 0;
					goto IL_00c0;
				}
				if (num != 1)
				{
					return false;
				}
				<>1__state = -1;
			}
			else
			{
				TMP_Text tMP_Text = textComponent;
				<>1__state = -1;
				TMP_Text tMP_Text2 = textComponent;
				TMP_TextInfo textInfo = tMP_Text2.m_textInfo;
				<textInfo>5__2 = textInfo;
				<totalVisibleCharacters>5__3 = <textInfo>5__2.characterCount;
				<visibleCount>5__4 = 0;
			}
			if (textConsoleSimulator.hasTextChanged)
			{
				<totalVisibleCharacters>5__3 = <textInfo>5__2.characterCount;
				textConsoleSimulator.hasTextChanged = false;
			}
			if (<visibleCount>5__4 > <totalVisibleCharacters>5__3)
			{
				WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
				<>2__current = waitForSeconds;
				<>1__state = 1;
				return true;
			}
			goto IL_00c0;
			IL_00c0:
			textComponent.maxVisibleCharacters = <visibleCount>5__4;
			<visibleCount>5__4++;
			<>2__current = null;
			<>1__state = 2;
			return true;
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00261B30 File Offset: 0x00260130
		private void System.Collections.IEnumerator.Reset()
		{
			long num = NativeIntrinsics.CallNative("sub_184BE0", NativeIntrinsics.Asm("rax"), 0L, 0L, 0L);
			NativeIntrinsics.Reinterpret<NotSupportedException>(num)..ctor();
			throw null;
		}
	}

	// Token: 0x02000078 RID: 120
	private sealed class <RevealWords>d__8 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x04000309 RID: 777 Offset: 0x10
		private int <>1__state;

		// Token: 0x0400030A RID: 778 Offset: 0x18
		private object <>2__current;

		// Token: 0x0400030B RID: 779 Offset: 0x20
		public TMP_Text textComponent;

		// Token: 0x0400030C RID: 780 Offset: 0x28
		private int <totalWordCount>5__2;

		// Token: 0x0400030D RID: 781 Offset: 0x2C
		private int <totalVisibleCharacters>5__3;

		// Token: 0x0400030E RID: 782 Offset: 0x30
		private int <counter>5__4;

		// Token: 0x0400030F RID: 783 Offset: 0x34
		private int <visibleCount>5__5;

		private object System.Collections.Generic.IEnumerator<System.Object>.Current => <>2__current;

		private object System.Collections.IEnumerator.Current => <>2__current;

		// Token: 0x06000221 RID: 545 RVA: 0x00250B50 File Offset: 0x0024F150
		public <RevealWords>d__8(int <>1__state)
		{
			((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
			this.<>1__state = <>1__state;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00250B40 File Offset: 0x0024F140
		private void System.IDisposable.Dispose()
		{
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00261B70 File Offset: 0x00260170
		private bool MoveNext()
		{
			int num = <>1__state;
			if (<>1__state != 0)
			{
				num--;
				if (num - 1 == 0)
				{
					<>1__state = -1;
					goto IL_0121;
				}
				if (num != 1)
				{
					return false;
				}
				<>1__state = -1;
			}
			else
			{
				TMP_Text tMP_Text = textComponent;
				<>1__state = -1;
				<totalWordCount>5__2 = textComponent.m_textInfo.wordCount;
				<totalVisibleCharacters>5__3 = textComponent.m_textInfo.characterCount;
				<counter>5__4 = 0;
			}
			if (((int)NativeIntrinsics.Asm("rdx") & (int)NativeIntrinsics.Asm("rdx")) != 0)
			{
				if ((int)NativeIntrinsics.Asm("method") >= <totalWordCount>5__2)
				{
					if ((int)NativeIntrinsics.Asm("method") == <totalWordCount>5__2)
					{
						<visibleCount>5__5 = <totalVisibleCharacters>5__3;
					}
				}
				else
				{
					<visibleCount>5__5 = (int)NativeIntrinsics.Asm("*(FieldExpr + 0x2C)") + 1;
				}
			}
			else
			{
				<visibleCount>5__5 = 0;
			}
			textComponent.maxVisibleCharacters = <visibleCount>5__5;
			if (<visibleCount>5__5 >= <totalVisibleCharacters>5__3)
			{
				WaitForSeconds waitForSeconds = new WaitForSeconds(1f);
				<>2__current = waitForSeconds;
				<>1__state = 1;
				return true;
			}
			goto IL_0121;
			IL_0121:
			<counter>5__4++;
			WaitForSeconds waitForSeconds2 = new WaitForSeconds(0.1f);
			<>2__current = waitForSeconds2;
			<>1__state = 2;
			return true;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00261D70 File Offset: 0x00260370
		private void System.Collections.IEnumerator.Reset()
		{
			long num = NativeIntrinsics.CallNative("sub_184BE0", NativeIntrinsics.Asm("rax"), 0L, 0L, 0L);
			NativeIntrinsics.Reinterpret<NotSupportedException>(num)..ctor();
			throw null;
		}
	}

	// Token: 0x04000300 RID: 768 Offset: 0x20
	private TMP_Text m_TextComponent;

	// Token: 0x04000301 RID: 769 Offset: 0x28
	private bool hasTextChanged;

	// Token: 0x06000213 RID: 531 RVA: 0x0025FBF0 File Offset: 0x0025E1F0
	private void Awake()
	{
		GameObject gameObject = this.gameObject;
		TMP_Text component = gameObject.GetComponent<TMP_Text>();
		m_TextComponent = component;
	}

	// Token: 0x06000214 RID: 532 RVA: 0x0025FF00 File Offset: 0x0025E500
	private void Start()
	{
		TMP_Text textComponent = m_TextComponent;
		<RevealCharacters>d__7 <RevealCharacters>d__ = NativeIntrinsics.Reinterpret<<RevealCharacters>d__7>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.TextConsoleSimulator+<RevealCharacters>d__7)"));
		((Mono.Xml.SecurityParser)(object)<RevealCharacters>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<RevealCharacters>d__.<>1__state = 0;
		<RevealCharacters>d__.<>4__this = this;
		<RevealCharacters>d__.textComponent = textComponent;
		Coroutine coroutine = StartCoroutine(<RevealCharacters>d__);
	}

	// Token: 0x06000215 RID: 533 RVA: 0x0025FD30 File Offset: 0x0025E330
	private void OnEnable()
	{
		FastAction<UnityEngine.Object> tEXT_CHANGED_EVENT = TMPro_EventManager.TEXT_CHANGED_EVENT;
		Action<UnityEngine.Object> rhs = (Action<UnityEngine.Object>)(object)new System.IOAsyncCallback(ON_TEXT_CHANGED);
		tEXT_CHANGED_EVENT.Add(rhs);
	}

	// Token: 0x06000216 RID: 534 RVA: 0x0025FC60 File Offset: 0x0025E260
	private void OnDisable()
	{
		FastAction<UnityEngine.Object> tEXT_CHANGED_EVENT = TMPro_EventManager.TEXT_CHANGED_EVENT;
		Action<UnityEngine.Object> rhs = (Action<UnityEngine.Object>)(object)new System.IOAsyncCallback(ON_TEXT_CHANGED);
		tEXT_CHANGED_EVENT.Remove(rhs);
	}

	// Token: 0x06000217 RID: 535 RVA: 0x0025FC50 File Offset: 0x0025E250
	private void ON_TEXT_CHANGED(UnityEngine.Object obj)
	{
		hasTextChanged = true;
	}

	// Token: 0x06000218 RID: 536 RVA: 0x0025FE00 File Offset: 0x0025E400
	private IEnumerator RevealCharacters(TMP_Text textComponent)
	{
		<RevealCharacters>d__7 <RevealCharacters>d__ = NativeIntrinsics.Reinterpret<<RevealCharacters>d__7>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.TextConsoleSimulator+<RevealCharacters>d__7)"));
		((Mono.Xml.SecurityParser)(object)<RevealCharacters>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<RevealCharacters>d__.<>1__state = 0;
		<RevealCharacters>d__.<>4__this = this;
		<RevealCharacters>d__.textComponent = textComponent;
		return <RevealCharacters>d__;
	}

	// Token: 0x06000219 RID: 537 RVA: 0x0025FE90 File Offset: 0x0025E490
	private IEnumerator RevealWords(TMP_Text textComponent)
	{
		<RevealWords>d__8 <RevealWords>d__ = NativeIntrinsics.Reinterpret<<RevealWords>d__8>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.TextConsoleSimulator+<RevealWords>d__8)"));
		((Mono.Xml.SecurityParser)(object)<RevealWords>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<RevealWords>d__.<>1__state = 0;
		<RevealWords>d__.textComponent = textComponent;
		return <RevealWords>d__;
	}
}

