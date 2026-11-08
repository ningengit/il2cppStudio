using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x02000076 RID: 118
public class TextConsoleSimulator : MonoBehaviour
{
	// Token: 0x04000300 RID: 768 Offset: 0x20
	private TMP_Text m_TextComponent;

	// Token: 0x04000301 RID: 769 Offset: 0x28
	private bool hasTextChanged;

	// Token: 0x06000213 RID: 531 RVA: 0x0025FBF0 File Offset: 0x0025E1F0
	private void Awake()
	{
		m_TextComponent = gameObject.GetComponent<TMP_Text>();
	}

	// Token: 0x06000214 RID: 532 RVA: 0x0025FF00 File Offset: 0x0025E500
	private void Start()
	{
		TMP_Text textComponent = m_TextComponent;
		StartCoroutine(RevealCharacters(textComponent));
	}

	// Token: 0x06000215 RID: 533 RVA: 0x0025FD30 File Offset: 0x0025E330
	private void OnEnable()
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Add(Il2CppStudio.Native.NativeIntrinsics.Reinterpret<Action<UnityEngine.Object>>("unresolved delegate target"));
	}

	// Token: 0x06000216 RID: 534 RVA: 0x0025FC60 File Offset: 0x0025E260
	private void OnDisable()
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(Il2CppStudio.Native.NativeIntrinsics.Reinterpret<Action<UnityEngine.Object>>("unresolved delegate target"));
	}

	// Token: 0x06000217 RID: 535 RVA: 0x0025FC50 File Offset: 0x0025E250
	private void ON_TEXT_CHANGED(UnityEngine.Object obj)
	{
		hasTextChanged = true;
	}

	// Token: 0x06000218 RID: 536 RVA: 0x0025FE00 File Offset: 0x0025E400
	private IEnumerator RevealCharacters(TMP_Text textComponent)
	{
		// Il2CppStudio: coroutine reconstructed from <RevealCharacters>d__7.MoveNext (state machine class hidden).
		textComponent.ForceMeshUpdate(ignoreActiveState: false, forceTextReparsing: false);
		TMP_TextInfo textInfo = textComponent.textInfo;
		int totalVisibleCharacters = textInfo.characterCount;
		int visibleCount = 0;
		while (true)
		{
			if (hasTextChanged)
			{
				totalVisibleCharacters = textInfo.characterCount;
				hasTextChanged = false;
			}
			if (visibleCount > totalVisibleCharacters)
			{
				yield return new WaitForSeconds(1f);
				visibleCount = 0;
			}
			textComponent.maxVisibleCharacters = visibleCount;
			visibleCount++;
			yield return null;
		}
	}

	// Token: 0x06000219 RID: 537 RVA: 0x0025FE90 File Offset: 0x0025E490
	private IEnumerator RevealWords(TMP_Text textComponent)
	{
		// Il2CppStudio: coroutine reconstructed from <RevealWords>d__8.MoveNext (state machine class hidden).
		int visibleCount = default(int);
		textComponent.ForceMeshUpdate(ignoreActiveState: false, forceTextReparsing: false);
		int totalWordCount = textComponent.textInfo.wordCount;
		int totalVisibleCharacters = textComponent.textInfo.characterCount;
		int counter = 0;
		while (true)
		{
			int num2 = counter % (totalWordCount + 1);
			if (counter % (totalWordCount + 1) == 0)
			{
				visibleCount = 0;
			}
			else if (!(num2 >= totalWordCount))
			{
				visibleCount = (int)NativeIntrinsics.Asm("*(FieldExpr + 0x2C)") + 1;
			}
			else if (num2 == totalWordCount)
			{
				visibleCount = totalVisibleCharacters;
			}
			textComponent.maxVisibleCharacters = visibleCount;
			if (visibleCount >= totalVisibleCharacters)
			{
				yield return new WaitForSeconds(1f);
			}
			counter++;
			yield return new WaitForSeconds(0.1f);
		}
	}
}

