using System;
using Il2CppStudio.Native;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TMPro;

// Token: 0x0200005E RID: 94
public class TMP_TextEventHandler : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x0200005F RID: 95
	[Serializable]
	public class CharacterSelectionEvent : UnityEvent<char, int>
	{
		// Token: 0x060001CE RID: 462 RVA: 0x00254C40 File Offset: 0x00253240
		public CharacterSelectionEvent()
		{
			((UnityEvent<bool>)(object)this)..ctor();
		}
	}

	// Token: 0x02000060 RID: 96
	[Serializable]
	public class SpriteSelectionEvent : UnityEvent<char, int>
	{
		// Token: 0x060001CF RID: 463 RVA: 0x002596B0 File Offset: 0x00257CB0
		public SpriteSelectionEvent()
		{
			((UnityEvent<bool>)(object)this)..ctor();
		}
	}

	// Token: 0x02000061 RID: 97
	[Serializable]
	public class WordSelectionEvent : UnityEvent<string, int, int>
	{
		// Token: 0x060001D0 RID: 464 RVA: 0x00263B20 File Offset: 0x00262120
		public WordSelectionEvent()
		{
			((UnityEvent<bool>)(object)this)..ctor();
		}
	}

	// Token: 0x02000062 RID: 98
	[Serializable]
	public class LineSelectionEvent : UnityEvent<string, int, int>
	{
		// Token: 0x060001D1 RID: 465 RVA: 0x00255680 File Offset: 0x00253C80
		public LineSelectionEvent()
		{
			((UnityEvent<bool>)(object)this)..ctor();
		}
	}

	// Token: 0x02000063 RID: 99
	[Serializable]
	public class LinkSelectionEvent : UnityEvent<string, string, int>
	{
		// Token: 0x060001D2 RID: 466 RVA: 0x002556C0 File Offset: 0x00253CC0
		public LinkSelectionEvent()
		{
			((UnityEvent<bool>)(object)this)..ctor();
		}
	}

	// Token: 0x04000286 RID: 646 Offset: 0x20
	private CharacterSelectionEvent m_OnCharacterSelection = (CharacterSelectionEvent)(object)new UnityEvent<bool>();

	// Token: 0x04000287 RID: 647 Offset: 0x28
	private SpriteSelectionEvent m_OnSpriteSelection = (SpriteSelectionEvent)(object)new UnityEvent<bool>();

	// Token: 0x04000288 RID: 648 Offset: 0x30
	private WordSelectionEvent m_OnWordSelection = (WordSelectionEvent)(object)new UnityEvent<bool>();

	// Token: 0x04000289 RID: 649 Offset: 0x38
	private LineSelectionEvent m_OnLineSelection = (LineSelectionEvent)(object)new UnityEvent<bool>();

	// Token: 0x0400028A RID: 650 Offset: 0x40
	private LinkSelectionEvent m_OnLinkSelection = (LinkSelectionEvent)(object)new UnityEvent<bool>();

	// Token: 0x0400028B RID: 651 Offset: 0x48
	private TMP_Text m_TextComponent;

	// Token: 0x0400028C RID: 652 Offset: 0x50
	private Camera m_Camera;

	// Token: 0x0400028D RID: 653 Offset: 0x58
	private Canvas m_Canvas;

	// Token: 0x0400028E RID: 654 Offset: 0x60
	private int m_selectedLink = -1;

	// Token: 0x0400028F RID: 655 Offset: 0x64
	private int m_lastCharIndex;

	// Token: 0x04000290 RID: 656 Offset: 0x68
	private int m_lastWordIndex = -1;

	// Token: 0x04000291 RID: 657 Offset: 0x6C
	private int m_lastLineIndex;

	public CharacterSelectionEvent onCharacterSelection
	{
		// Token: 0x060001BA RID: 442 RVA: 0x0025C390 File Offset: 0x0025A990
		get
		{
			return m_OnCharacterSelection;
		}
		// Token: 0x060001BB RID: 443 RVA: 0x0025C3E0 File Offset: 0x0025A9E0
		set
		{
			m_OnCharacterSelection = value;
		}
	}

	public SpriteSelectionEvent onSpriteSelection
	{
		// Token: 0x060001BC RID: 444 RVA: 0x0025C3C0 File Offset: 0x0025A9C0
		get
		{
			return m_OnSpriteSelection;
		}
		// Token: 0x060001BD RID: 445 RVA: 0x0025C410 File Offset: 0x0025AA10
		set
		{
			m_OnSpriteSelection = value;
		}
	}

	public WordSelectionEvent onWordSelection
	{
		// Token: 0x060001BE RID: 446 RVA: 0x0025C3D0 File Offset: 0x0025A9D0
		get
		{
			return m_OnWordSelection;
		}
		// Token: 0x060001BF RID: 447 RVA: 0x0025C420 File Offset: 0x0025AA20
		set
		{
			m_OnWordSelection = value;
		}
	}

	public LineSelectionEvent onLineSelection
	{
		// Token: 0x060001C0 RID: 448 RVA: 0x0025C3A0 File Offset: 0x0025A9A0
		get
		{
			return m_OnLineSelection;
		}
		// Token: 0x060001C1 RID: 449 RVA: 0x0025C3F0 File Offset: 0x0025A9F0
		set
		{
			m_OnLineSelection = value;
		}
	}

	public LinkSelectionEvent onLinkSelection
	{
		// Token: 0x060001C2 RID: 450 RVA: 0x0025C3B0 File Offset: 0x0025A9B0
		get
		{
			return m_OnLinkSelection;
		}
		// Token: 0x060001C3 RID: 451 RVA: 0x0025C400 File Offset: 0x0025AA00
		set
		{
			m_OnLinkSelection = value;
		}
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x0025B760 File Offset: 0x00259D60
	private void Awake()
	{
		GameObject gameObject = this.gameObject;
		TMP_Text component = gameObject.GetComponent<TMP_Text>();
		m_TextComponent = component;
		TMP_Text textComponent = m_TextComponent;
		Type type = textComponent.GetType();
		Type value = type;
		Type typeFromHandle = Type.GetTypeFromHandle(NativeIntrinsics.Reinterpret<RuntimeTypeHandle>((nint)NativeIntrinsics.Asm("typeof(TextMeshProUGUI)")));
		Camera camera = default(Camera);
		if (!(NativeIntrinsics.Reinterpret<TimeSpan>(value) == NativeIntrinsics.Reinterpret<TimeSpan>(typeFromHandle)))
		{
			Camera main = Camera.main;
			camera = main;
		}
		else
		{
			GameObject gameObject2 = this.gameObject;
			Canvas componentInParent = gameObject2.GetComponentInParent<Canvas>();
			m_Canvas = componentInParent;
			Canvas canvas = m_Canvas;
			bool flag = canvas != null;
			bool flag2 = flag;
			if (!flag)
			{
				return;
			}
			RenderMode renderMode = m_Canvas.renderMode;
			if ((NativeIntrinsics.Reinterpret<int>(renderMode) & NativeIntrinsics.Reinterpret<int>(renderMode)) == 0)
			{
				m_Camera = null;
				return;
			}
			Camera worldCamera = m_Canvas.worldCamera;
			Camera camera2 = worldCamera;
		}
		m_Camera = camera;
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x0025B910 File Offset: 0x00259F10
	private unsafe void LateUpdate()
	{
		TMP_Text textComponent = m_TextComponent;
		TMP_Text tMP_Text = textComponent;
		RectTransform rectTransform = tMP_Text.rectTransform;
		Vector3 mousePosition = Input.mousePosition;
		long num = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
		Camera camera = m_Camera;
		float z = mousePosition.z;
		RectTransform rectTransform2 = rectTransform;
		bool flag = TMP_TextUtilities.IsIntersectingRectTransform(rectTransform2, NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -121L), camera);
		bool flag2 = flag;
		if (!flag)
		{
			return;
		}
		TMP_Text textComponent2 = m_TextComponent;
		Vector3 mousePosition2 = Input.mousePosition;
		num = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
		camera = m_Camera;
		z = mousePosition2.z;
		TMP_Text text = textComponent2;
		int num2 = TMP_TextUtilities.FindIntersectingCharacter(text, NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -121L), camera, visibleOnly: true);
		long num3 = num2;
		int arg;
		CharacterSelectionEvent characterSelectionEvent = default(CharacterSelectionEvent);
		int num4;
		if (num2 != -1 && (int)num3 != m_lastCharIndex)
		{
			TMP_Text textComponent3 = m_TextComponent;
			m_lastCharIndex = (int)num3;
			TMP_Text tMP_Text2 = textComponent3;
			num4 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x2C)");
			if ((int)NativeIntrinsics.Asm("*(BinaryExpr + 0x2C)") != 0)
			{
				if (num4 == 1)
				{
					arg = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x20)");
					if (m_OnSpriteSelection != null)
					{
						SpriteSelectionEvent spriteSelectionEvent = m_OnSpriteSelection;
						goto IL_0179;
					}
				}
			}
			else
			{
				arg = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x20)");
				if (m_OnCharacterSelection != null)
				{
					characterSelectionEvent = m_OnCharacterSelection;
					goto IL_0179;
				}
			}
		}
		goto IL_0186;
		IL_0186:
		TMP_Text textComponent4 = m_TextComponent;
		Vector3 mousePosition3 = Input.mousePosition;
		num = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
		camera = m_Camera;
		float z2 = mousePosition3.z;
		TMP_Text text2 = textComponent4;
		int num5 = TMP_TextUtilities.FindIntersectingWord(text2, NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -121L), camera);
		num4 = num5;
		if (num5 != -1 && num4 != m_lastWordIndex)
		{
			TMP_Text textComponent5 = m_TextComponent;
			m_lastWordIndex = num4;
			TMP_WordInfo[] wordInfo = textComponent5.m_textInfo.wordInfo;
			string word = ((TMP_WordInfo*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -105L))->GetWord();
			long num6 = NativeIntrinsics.Asm("*(LocalExpr + 0x30)");
			int arg2 = (int)NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFF40]");
			string arg3 = word;
			if (m_OnWordSelection != null)
			{
				m_OnWordSelection.Invoke(arg3, arg2, (int)num6);
			}
		}
		TMP_Text textComponent6 = m_TextComponent;
		Vector3 mousePosition4 = Input.mousePosition;
		num = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
		Camera camera2 = m_Camera;
		float z3 = mousePosition4.z;
		TMP_Text text3 = textComponent6;
		int num7 = TMP_TextUtilities.FindIntersectingLine(text3, NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -121L), camera2);
		num4 = num7;
		if (num7 != -1 && num4 != m_lastLineIndex)
		{
			TMP_Text textComponent7 = m_TextComponent;
			m_lastLineIndex = num4;
			TMP_TextInfo textInfo = textComponent7.m_textInfo;
			TMP_LineInfo[] lineInfo = textInfo.lineInfo;
			char[] array = new char[(int)(NativeIntrinsics.Asm("*(BinaryExpr + 0x20)") >>> 32)];
			char[] array2 = array;
			long num8 = NativeIntrinsics.Asm("*(BinaryExpr + 0x20)");
			num = NativeIntrinsics.Asm("*(BinaryExpr + 0x30)");
			if (((int)(NativeIntrinsics.Asm("*(BinaryExpr + 0x20)") >>> 32) & (int)(NativeIntrinsics.Asm("*(BinaryExpr + 0x20)") >>> 32)) > 0)
			{
				while (0 < m_TextComponent.m_textInfo.characterInfo.Length)
				{
					array2[0] = (char)NativeIntrinsics.Asm("*(FieldExpr + 0x20)");
					int num9 = 0 + 1;
					if (0 + 1 >= (int)(num8 >>> 32))
					{
						break;
					}
				}
			}
			string text4 = ((string)null).CreateString(array2);
			string arg4 = text4;
			if (m_OnLineSelection != null)
			{
				m_OnLineSelection.Invoke(arg4, (int)NativeIntrinsics.Asm("psrldq xmm6,4"), (int)NativeIntrinsics.Asm("psrldq xmm7,4"));
			}
		}
		TMP_Text textComponent8 = m_TextComponent;
		Vector3 mousePosition5 = Input.mousePosition;
		num = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
		camera2 = m_Camera;
		z3 = mousePosition5.z;
		TMP_Text text5 = textComponent8;
		int num10 = TMP_TextUtilities.FindIntersectingLink(text5, NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -121L), camera2);
		num4 = num10;
		long num11 = num10;
		if (num10 != -1 && (int)num11 != m_selectedLink)
		{
			TMP_Text textComponent9 = m_TextComponent;
			m_selectedLink = (int)num11;
			TMP_Text tMP_Text3 = textComponent9;
			TMP_LinkInfo[] linkInfo = tMP_Text3.m_textInfo.linkInfo;
			string linkID = ((TMP_LinkInfo*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -81L))->GetLinkID();
			string linkText = ((TMP_LinkInfo*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -81L))->GetLinkText();
			string text6 = linkText;
			string arg5 = linkText;
			string arg6 = linkID;
			if (m_OnLinkSelection != null)
			{
				m_OnLinkSelection.Invoke(arg6, arg5, (int)num11);
			}
		}
		return;
		IL_0179:
		characterSelectionEvent.Invoke((char)arg, (int)num3);
		goto IL_0186;
	}

	// Token: 0x060001C6 RID: 454 RVA: 0x00250B40 File Offset: 0x0024F140
	public void OnPointerEnter(PointerEventData eventData)
	{
	}

	// Token: 0x060001C7 RID: 455 RVA: 0x00250B40 File Offset: 0x0024F140
	public void OnPointerExit(PointerEventData eventData)
	{
	}

	// Token: 0x060001C8 RID: 456 RVA: 0x0025BFA0 File Offset: 0x0025A5A0
	private void SendOnCharacterSelection(char character, int characterIndex)
	{
		if (m_OnCharacterSelection != null)
		{
			m_OnCharacterSelection.Invoke(character, characterIndex);
		}
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x0025C0D0 File Offset: 0x0025A6D0
	private void SendOnSpriteSelection(char character, int characterIndex)
	{
		if (m_OnSpriteSelection != null)
		{
			m_OnSpriteSelection.Invoke(character, characterIndex);
		}
	}

	// Token: 0x060001CA RID: 458 RVA: 0x0025C140 File Offset: 0x0025A740
	private void SendOnWordSelection(string word, int charIndex, int length)
	{
		if (m_OnWordSelection != null)
		{
			m_OnWordSelection.Invoke(word, charIndex, length);
		}
	}

	// Token: 0x060001CB RID: 459 RVA: 0x0025C010 File Offset: 0x0025A610
	private void SendOnLineSelection(string line, int charIndex, int length)
	{
		if (m_OnLineSelection != null)
		{
			m_OnLineSelection.Invoke(line, charIndex, length);
		}
	}

	// Token: 0x060001CC RID: 460 RVA: 0x0025C070 File Offset: 0x0025A670
	private void SendOnLinkSelection(string linkID, string linkText, int linkIndex)
	{
		if (m_OnLinkSelection != null)
		{
			m_OnLinkSelection.Invoke(linkID, linkText, linkIndex);
		}
	}
}

