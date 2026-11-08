using Il2CppStudio.Native;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x02000071 RID: 113
public class SimpleScript : MonoBehaviour
{
	// Token: 0x040002EB RID: 747 Offset: 0x20
	private TextMeshPro m_textMeshPro;

	// Token: 0x040002EC RID: 748 Offset: 0x0
	private const string label = null;

	// Token: 0x040002ED RID: 749 Offset: 0x28
	private float m_frame;

	// Token: 0x060001FC RID: 508 RVA: 0x00259150 File Offset: 0x00257750
	private void Start()
	{
		GameObject gameObject = this.gameObject;
		TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
		m_textMeshPro = textMeshPro;
		m_textMeshPro.fontSize = 48f;
		m_textMeshPro.alignment = NativeIntrinsics.Reinterpret<TextAlignmentOptions>(514);
		m_textMeshPro.enableWordWrapping = false;
	}

	// Token: 0x060001FD RID: 509 RVA: 0x00259210 File Offset: 0x00257810
	private void Update()
	{
		long num = NativeIntrinsics.CallNative("sub_20CB10", NativeIntrinsics.Reinterpret<long>(this), 0L, 0L, 0L);
		m_textMeshPro.SetText("The <#0050FF>count is: </color>{0:2}", NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("xmm0")));
		float frame = m_frame;
		float deltaTime = Time.deltaTime;
		m_frame = deltaTime + frame;
	}
}

