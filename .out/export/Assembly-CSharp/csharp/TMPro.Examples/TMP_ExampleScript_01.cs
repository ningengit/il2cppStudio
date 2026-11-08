using Il2CppStudio.Native;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x0200007F RID: 127
public class TMP_ExampleScript_01 : MonoBehaviour
{
	// Token: 0x02000080 RID: 128
	public enum objectType
	{
		// error: enumerator has no value
		// Token: 0x04000343 RID: 835 Offset: 0x0
		TextMeshPro,
		// error: enumerator has no value
		// Token: 0x04000344 RID: 836 Offset: 0x0
		TextMeshProUGUI
	}

	// Token: 0x0400033D RID: 829 Offset: 0x20
	public objectType ObjectType;

	// Token: 0x0400033E RID: 830 Offset: 0x24
	public bool isStatic;

	// Token: 0x0400033F RID: 831 Offset: 0x28
	private TMP_Text m_text;

	// Token: 0x04000340 RID: 832 Offset: 0x0
	private const string k_label = null;

	// Token: 0x04000341 RID: 833 Offset: 0x30
	private int count;

	// Token: 0x0600023E RID: 574 RVA: 0x00259B50 File Offset: 0x00258150
	private void Awake()
	{
		GameObject gameObject4 = default(GameObject);
		if (NativeIntrinsics.Reinterpret<int>(ObjectType) != 0)
		{
			TextMeshProUGUI component = GetComponent<TextMeshProUGUI>();
			TextMeshProUGUI textMeshProUGUI = component;
			if ((object)component == null)
			{
				GameObject gameObject = this.gameObject;
				GameObject gameObject2 = gameObject;
				goto IL_0059;
			}
		}
		else
		{
			TextMeshPro component2 = GetComponent<TextMeshPro>();
			TextMeshPro textMeshPro = component2;
			if ((object)component2 == null)
			{
				GameObject gameObject3 = this.gameObject;
				gameObject4 = gameObject3;
				goto IL_0059;
			}
		}
		goto IL_0067;
		IL_0059:
		TextMeshPro textMeshPro2 = gameObject4.AddComponent<TextMeshPro>();
		TextMeshPro text = textMeshPro2;
		goto IL_0067;
		IL_0067:
		m_text = text;
		TMP_Text text2 = m_text;
		TMP_FontAsset font = Resources.Load<TMP_FontAsset>("Fonts & Materials/Anton SDF");
		text2.font = font;
		TMP_Text text3 = m_text;
		Material material = Resources.Load<Material>("Fonts & Materials/Anton SDF - Drop Shadow");
		m_text.fontSize = 120f;
		Vector2 preferredValues = m_text.GetPreferredValues(float.PositiveInfinity, float.PositiveInfinity);
		RectTransform rectTransform = m_text.rectTransform;
		rectTransform.sizeDelta = NativeIntrinsics.Reinterpret<Vector2>(NativeIntrinsics.Asm("unpcklps xmm6,xmm7"));
	}

	// Token: 0x0600023F RID: 575 RVA: 0x00259D80 File Offset: 0x00258380
	private void Update()
	{
		if (!isStatic)
		{
			m_text.SetText("The count is <#0080ff>{0}</color>", (int)NativeIntrinsics.Asm("imul r8d") - (((int)NativeIntrinsics.Asm("rdx") >> 6) + ((int)NativeIntrinsics.Asm("rdx") >> 6 >>> 31)) * 1000);
			count++;
		}
	}
}

