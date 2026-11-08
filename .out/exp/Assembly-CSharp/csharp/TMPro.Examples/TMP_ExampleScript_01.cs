using Il2CppStudio.Native;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x0200007F RID: 127
public class TMP_ExampleScript_01 : MonoBehaviour
{
	// Token: 0x02000080 RID: 128
	public enum objectType
	{
		// Token: 0x04000343 RID: 835 Offset: 0x0
		TextMeshPro,
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
	private const string k_label = "The count is <#0080ff>{0}</color>";

	// Token: 0x04000341 RID: 833 Offset: 0x30
	private int count;

	// Token: 0x0600023E RID: 574 RVA: 0x00259B50 File Offset: 0x00258150
	private void Awake()
	{
		GameObject gameObject = default(GameObject);
		if (NativeIntrinsics.Reinterpret<int>(ObjectType) != 0)
		{
			if ((object)GetComponent<TextMeshProUGUI>() == null)
			{
				_ = this.gameObject;
				goto IL_0074;
			}
		}
		else if ((object)GetComponent<TextMeshPro>() == null)
		{
			gameObject = this.gameObject;
			goto IL_0074;
		}
		goto IL_0084;
		IL_0074:
		TextMeshProUGUI text = gameObject.AddComponent<TextMeshProUGUI>();
		goto IL_0084;
		IL_0084:
		m_text = text;
		m_text.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/Anton SDF");
		m_text.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/Anton SDF - Drop Shadow");
		m_text.fontSize = 120f;
		m_text.text = "A <#0080ff>simple</color> line of text.";
		Vector2 preferredValues = m_text.GetPreferredValues(float.PositiveInfinity, float.PositiveInfinity);
		m_text.rectTransform.sizeDelta = new Vector2(NativeIntrinsics.Reinterpret<float>(preferredValues), NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("stack[0xC]")));
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

