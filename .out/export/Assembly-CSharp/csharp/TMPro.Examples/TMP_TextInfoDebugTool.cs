using UnityEngine;

namespace TMPro.Examples;

// Token: 0x02000084 RID: 132
public class TMP_TextInfoDebugTool : MonoBehaviour
{
	// Token: 0x04000355 RID: 853 Offset: 0x20
	public bool ShowCharacters;

	// Token: 0x04000356 RID: 854 Offset: 0x21
	public bool ShowWords;

	// Token: 0x04000357 RID: 855 Offset: 0x22
	public bool ShowLinks;

	// Token: 0x04000358 RID: 856 Offset: 0x23
	public bool ShowLines;

	// Token: 0x04000359 RID: 857 Offset: 0x24
	public bool ShowMeshBounds;

	// Token: 0x0400035A RID: 858 Offset: 0x25
	public bool ShowTextBounds;

	// Token: 0x0400035B RID: 859 Offset: 0x28
	public string ObjectStats;

	// Token: 0x0400035C RID: 860 Offset: 0x30
	private TMP_Text m_TextComponent;

	// Token: 0x0400035D RID: 861 Offset: 0x38
	private Transform m_Transform;
}

