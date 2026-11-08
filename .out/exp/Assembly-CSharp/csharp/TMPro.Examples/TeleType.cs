using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x02000074 RID: 116
public class TeleType : MonoBehaviour
{
	// Token: 0x040002F8 RID: 760 Offset: 0x20
	private string label01 = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=1>";

	// Token: 0x040002F9 RID: 761 Offset: 0x28
	private string label02 = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=2>";

	// Token: 0x040002FA RID: 762 Offset: 0x30
	private TMP_Text m_textMeshPro;

	// Token: 0x0600020A RID: 522 RVA: 0x0025FA60 File Offset: 0x0025E060
	private void Awake()
	{
		m_textMeshPro = GetComponent<TMP_Text>();
		m_textMeshPro.text = label01;
		m_textMeshPro.enableWordWrapping = true;
		m_textMeshPro.alignment = TextAlignmentOptions.Top;
	}

	// Token: 0x0600020B RID: 523 RVA: 0x0025FB00 File Offset: 0x0025E100
	private IEnumerator Start()
	{
		// Il2CppStudio: coroutine reconstructed from <Start>d__4.MoveNext (state machine class hidden).
		m_textMeshPro.ForceMeshUpdate(ignoreActiveState: false, forceTextReparsing: false);
		int totalVisibleCharacters = m_textMeshPro.textInfo.characterCount;
		int counter = 0;
		while (true)
		{
			int num2 = counter % (totalVisibleCharacters + 1);
			m_textMeshPro.maxVisibleCharacters = num2;
			if (num2 >= totalVisibleCharacters)
			{
				yield return new WaitForSeconds(1f);
				m_textMeshPro.text = label02;
				yield return new WaitForSeconds(1f);
				m_textMeshPro.text = label01;
				yield return new WaitForSeconds(1f);
			}
			counter++;
			yield return new WaitForSeconds(0.05f);
		}
	}
}

