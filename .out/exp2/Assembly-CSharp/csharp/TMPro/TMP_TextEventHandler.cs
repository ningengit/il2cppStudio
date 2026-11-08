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
	private CharacterSelectionEvent m_OnCharacterSelection = new CharacterSelectionEvent();

	// Token: 0x04000287 RID: 647 Offset: 0x28
	private SpriteSelectionEvent m_OnSpriteSelection = new SpriteSelectionEvent();

	// Token: 0x04000288 RID: 648 Offset: 0x30
	private WordSelectionEvent m_OnWordSelection = new WordSelectionEvent();

	// Token: 0x04000289 RID: 649 Offset: 0x38
	private LineSelectionEvent m_OnLineSelection = new LineSelectionEvent();

	// Token: 0x0400028A RID: 650 Offset: 0x40
	private LinkSelectionEvent m_OnLinkSelection = new LinkSelectionEvent();

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
		m_TextComponent = gameObject.GetComponent<TMP_Text>();
		TMP_TextEventHandler tMP_TextEventHandler;
		Camera camera = default(Camera);
		if (!(m_TextComponent.GetType() == Type.GetTypeFromHandle(NativeIntrinsics.Reinterpret<RuntimeTypeHandle>((nint)NativeIntrinsics.Asm("typeof(TextMeshProUGUI)")))))
		{
			Camera main = Camera.main;
			tMP_TextEventHandler = this;
			camera = main;
		}
		else
		{
			m_Canvas = gameObject.GetComponentInParent<Canvas>();
			if (!(m_Canvas != null))
			{
				return;
			}
			if (m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				m_Camera = null;
				return;
			}
			_ = m_Canvas.worldCamera;
			tMP_TextEventHandler = this;
		}
		tMP_TextEventHandler.m_Camera = camera;
	}

	// Token: 0x060001C5 RID: 453 RVA: 0x0025B910 File Offset: 0x00259F10
	private void LateUpdate()
	{
		RectTransform rectTransform = m_TextComponent.rectTransform;
		Vector3 mousePosition = Input.mousePosition;
		Camera camera = m_Camera;
		float z = mousePosition.z;
		Vector3 position = new Vector3(mousePosition.x, mousePosition.y, z);
		if (!TMP_TextUtilities.IsIntersectingRectTransform(rectTransform, position, camera))
		{
			return;
		}
		TMP_Text textComponent = m_TextComponent;
		Vector3 mousePosition2 = Input.mousePosition;
		camera = m_Camera;
		z = mousePosition2.z;
		Vector3 position2 = new Vector3(mousePosition2.x, mousePosition2.y, z);
		int num = TMP_TextUtilities.FindIntersectingCharacter(textComponent, position2, camera, visibleOnly: true);
		long num2 = num;
		int arg;
		CharacterSelectionEvent characterSelectionEvent = default(CharacterSelectionEvent);
		int num3;
		if (num != -1 && (int)num2 != m_lastCharIndex)
		{
			_ = m_TextComponent;
			m_lastCharIndex = (int)num2;
			num3 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x2C)");
			if ((int)NativeIntrinsics.Asm("*(BinaryExpr + 0x2C)") == 0)
			{
				arg = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x20)");
				if (m_OnCharacterSelection != null)
				{
					characterSelectionEvent = m_OnCharacterSelection;
					goto IL_01c0;
				}
			}
			else if (num3 == 1)
			{
				arg = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x20)");
				if (m_OnSpriteSelection != null)
				{
					_ = m_OnSpriteSelection;
					goto IL_01c0;
				}
			}
		}
		goto IL_01cf;
		IL_01cf:
		TMP_Text textComponent2 = m_TextComponent;
		Vector3 mousePosition3 = Input.mousePosition;
		camera = m_Camera;
		Vector3 position3 = new Vector3(mousePosition3.x, mousePosition3.y, mousePosition3.z);
		int num4 = TMP_TextUtilities.FindIntersectingWord(textComponent2, position3, camera);
		num3 = num4;
		if (num4 != -1 && num3 != m_lastWordIndex)
		{
			TMP_Text textComponent3 = m_TextComponent;
			m_lastWordIndex = num3;
			_ = textComponent3.m_textInfo.wordInfo;
			TMP_WordInfo tMP_WordInfo = default(TMP_WordInfo);
			tMP_WordInfo.textComponent = NativeIntrinsics.Reinterpret<TMP_Text>(NativeIntrinsics.Asm("*(FieldExpr + 0x20)"));
			tMP_WordInfo.characterCount = (int)NativeIntrinsics.Asm("*(LocalExpr + 0x30)");
			string word = tMP_WordInfo.GetWord();
			int characterCount = tMP_WordInfo.characterCount;
			int firstCharacterIndex = tMP_WordInfo.firstCharacterIndex;
			string arg2 = word;
			if (m_OnWordSelection != null)
			{
				m_OnWordSelection.Invoke(arg2, firstCharacterIndex, characterCount);
			}
		}
		TMP_Text textComponent4 = m_TextComponent;
		Vector3 mousePosition4 = Input.mousePosition;
		Camera camera2 = m_Camera;
		float z3 = mousePosition4.z;
		Vector3 position4 = new Vector3(mousePosition4.x, mousePosition4.y, z3);
		int num5 = TMP_TextUtilities.FindIntersectingLine(textComponent4, position4, camera2);
		TMP_TextEventHandler tMP_TextEventHandler = this;
		num3 = num5;
		if (num5 != -1)
		{
			tMP_TextEventHandler = this;
			if (num3 != m_lastLineIndex)
			{
				TMP_Text textComponent5 = m_TextComponent;
				m_lastLineIndex = num3;
				_ = textComponent5.m_textInfo.lineInfo;
				char[] array = new char[(int)(NativeIntrinsics.Asm("*(BinaryExpr + 0x20)") >>> 32)];
				tMP_TextEventHandler = this;
				long num6 = NativeIntrinsics.Asm("*(BinaryExpr + 0x20)");
				NativeIntrinsics.Asm("*(BinaryExpr + 0x30)");
				if (((int)(NativeIntrinsics.Asm("*(BinaryExpr + 0x20)") >>> 32) & (int)(NativeIntrinsics.Asm("*(BinaryExpr + 0x20)") >>> 32)) > 0)
				{
					tMP_TextEventHandler = this;
					int num7 = 0;
					while (num7 < tMP_TextEventHandler.m_TextComponent.m_textInfo.characterInfo.Length)
					{
						array[num7] = (char)NativeIntrinsics.Asm("*(FieldExpr + 0x20)");
						num7++;
						if (num7 + 1 >= (int)(num6 >>> 32))
						{
							break;
						}
					}
				}
				string arg3 = ((string)null).CreateString(array);
				if (tMP_TextEventHandler.m_OnLineSelection != null)
				{
					tMP_TextEventHandler.m_OnLineSelection.Invoke(arg3, (int)NativeIntrinsics.Asm("psrldq xmm6,4"), (int)NativeIntrinsics.Asm("psrldq xmm7,4"));
				}
			}
		}
		TMP_Text textComponent6 = tMP_TextEventHandler.m_TextComponent;
		Vector3 mousePosition5 = Input.mousePosition;
		camera2 = tMP_TextEventHandler.m_Camera;
		z3 = mousePosition5.z;
		Vector3 position5 = new Vector3(mousePosition5.x, mousePosition5.y, z3);
		int num8 = TMP_TextUtilities.FindIntersectingLink(textComponent6, position5, camera2);
		long num9 = num8;
		if (num8 != -1 && (int)num9 != tMP_TextEventHandler.m_selectedLink)
		{
			TMP_Text textComponent7 = tMP_TextEventHandler.m_TextComponent;
			tMP_TextEventHandler.m_selectedLink = (int)num9;
			_ = textComponent7.m_textInfo.linkInfo;
			TMP_LinkInfo tMP_LinkInfo = default(TMP_LinkInfo);
			tMP_LinkInfo.textComponent = NativeIntrinsics.Reinterpret<TMP_Text>(NativeIntrinsics.Asm("*(FieldExpr + 0x20)"));
			tMP_LinkInfo.linkIdLength = (int)NativeIntrinsics.Asm("*(LocalExpr + 0x30)");
			tMP_LinkInfo.linkID = NativeIntrinsics.Reinterpret<char[]>(NativeIntrinsics.Asm("*(LocalExpr + 0x40)"));
			string linkID = tMP_LinkInfo.GetLinkID();
			string linkText = tMP_LinkInfo.GetLinkText();
			string arg4 = linkID;
			if (tMP_TextEventHandler.m_OnLinkSelection != null)
			{
				tMP_TextEventHandler.m_OnLinkSelection.Invoke(arg4, linkText, (int)num9);
			}
		}
		return;
		IL_01c0:
		characterSelectionEvent.Invoke((char)arg, (int)num2);
		goto IL_01cf;
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

