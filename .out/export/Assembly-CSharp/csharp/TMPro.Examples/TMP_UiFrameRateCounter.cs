using Il2CppStudio.Native;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x02000087 RID: 135
public class TMP_UiFrameRateCounter : MonoBehaviour
{
	// Token: 0x02000088 RID: 136
	public enum FpsCounterAnchorPositions
	{
		// error: enumerator has no value
		// Token: 0x0400037C RID: 892 Offset: 0x0
		TopLeft,
		// error: enumerator has no value
		// Token: 0x0400037D RID: 893 Offset: 0x0
		BottomLeft,
		// error: enumerator has no value
		// Token: 0x0400037E RID: 894 Offset: 0x0
		TopRight,
		// error: enumerator has no value
		// Token: 0x0400037F RID: 895 Offset: 0x0
		BottomRight
	}

	// Token: 0x04000372 RID: 882 Offset: 0x20
	public float UpdateInterval = 5f;

	// Token: 0x04000373 RID: 883 Offset: 0x24
	private float m_LastInterval;

	// Token: 0x04000374 RID: 884 Offset: 0x28
	private int m_Frames;

	// Token: 0x04000375 RID: 885 Offset: 0x2C
	public FpsCounterAnchorPositions AnchorPosition = NativeIntrinsics.Reinterpret<FpsCounterAnchorPositions>(2);

	// Token: 0x04000376 RID: 886 Offset: 0x30
	private string htmlColorTag;

	// Token: 0x04000377 RID: 887 Offset: 0x0
	private const string fpsLabel = null;

	// Token: 0x04000378 RID: 888 Offset: 0x38
	private TextMeshProUGUI m_TextMeshPro;

	// Token: 0x04000379 RID: 889 Offset: 0x40
	private RectTransform m_frameCounter_transform;

	// Token: 0x0400037A RID: 890 Offset: 0x48
	private FpsCounterAnchorPositions last_AnchorPosition;

	// Token: 0x0600025F RID: 607 RVA: 0x0025ED40 File Offset: 0x0025D340
	private void Awake()
	{
		bool flag = enabled;
		bool flag2 = flag;
		if (flag)
		{
			Application.targetFrameRate = -1;
			GameObject gameObject = new GameObject("Frame Counter");
			RectTransform frameCounter_transform = gameObject.AddComponent<RectTransform>();
			m_frameCounter_transform = frameCounter_transform;
			RectTransform frameCounter_transform2 = m_frameCounter_transform;
			Transform transform = this.transform;
			Transform parent = transform;
			frameCounter_transform2.SetParent(parent, worldPositionStays: false);
			TextMeshProUGUI textMeshPro = gameObject.AddComponent<TextMeshProUGUI>();
			m_TextMeshPro = textMeshPro;
			TextMeshProUGUI textMeshPro2 = m_TextMeshPro;
			TMP_FontAsset tMP_FontAsset = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
			TMP_FontAsset font = tMP_FontAsset;
			textMeshPro2.font = font;
			TextMeshProUGUI textMeshPro3 = m_TextMeshPro;
			Material material = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - Overlay");
			Material material2 = material;
			m_TextMeshPro.enableWordWrapping = false;
			m_TextMeshPro.fontSize = 36f;
			m_TextMeshPro.isOverlay = true;
			Set_FrameCounter_Position(AnchorPosition);
			FpsCounterAnchorPositions anchorPosition = AnchorPosition;
			last_AnchorPosition = NativeIntrinsics.Reinterpret<FpsCounterAnchorPositions>(NativeIntrinsics.Reinterpret<int>(anchorPosition));
			FpsCounterAnchorPositions fpsCounterAnchorPositions = anchorPosition;
		}
	}

	// Token: 0x06000260 RID: 608 RVA: 0x0025A2E0 File Offset: 0x002588E0
	private void Start()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		m_LastInterval = realtimeSinceStartup;
		m_Frames = 0;
	}

	// Token: 0x06000261 RID: 609 RVA: 0x0025F1B0 File Offset: 0x0025D7B0
	private void Update()
	{
		if (NativeIntrinsics.Reinterpret<int>(AnchorPosition) != NativeIntrinsics.Reinterpret<int>(last_AnchorPosition))
		{
			Set_FrameCounter_Position(AnchorPosition);
		}
		FpsCounterAnchorPositions anchorPosition = AnchorPosition;
		m_Frames++;
		last_AnchorPosition = NativeIntrinsics.Reinterpret<FpsCounterAnchorPositions>(NativeIntrinsics.Reinterpret<int>(anchorPosition));
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		float num = realtimeSinceStartup;
		float lastInterval = realtimeSinceStartup;
		if (!(realtimeSinceStartup > m_LastInterval + UpdateInterval))
		{
			return;
		}
		long num2 = NativeIntrinsics.Reinterpret<long>(this) + 48L;
		long num3 = NativeIntrinsics.Asm("maxss xmm1,dword ptr [1813EAC30h]");
		float arg = 1000f / (float)NativeIntrinsics.Asm("maxss xmm1,dword ptr [1813EAC30h]");
		float num4 = (float)m_Frames / (num - m_LastInterval);
		if (!(30f > (float)m_Frames / (num - m_LastInterval)))
		{
			if (!(10f > num4))
			{
				htmlColorTag = "<color=green>";
			}
			else
			{
				htmlColorTag = "<color=red>";
			}
		}
		else
		{
			htmlColorTag = "<color=yellow>";
		}
		TextMeshProUGUI textMeshPro = m_TextMeshPro;
		string text = htmlColorTag + "{0:2}</color> <#8080ff>FPS \n<#FF8000>{1:2} <#8080ff>MS";
		string sourceText = text;
		textMeshPro.SetText(sourceText, num4, arg);
		m_LastInterval = lastInterval;
		m_Frames = 0;
	}

	// Token: 0x06000262 RID: 610 RVA: 0x0025EF60 File Offset: 0x0025D560
	private void Set_FrameCounter_Position(FpsCounterAnchorPositions anchor_position)
	{
		long num2;
		RectTransform frameCounter_transform = default(RectTransform);
		if ((NativeIntrinsics.Reinterpret<int>(anchor_position) & NativeIntrinsics.Reinterpret<int>(anchor_position)) != 0)
		{
			int num = NativeIntrinsics.Reinterpret<int>(anchor_position) - 1;
			if (NativeIntrinsics.Reinterpret<int>(anchor_position) - 1 != 0)
			{
				num--;
				if (num - 1 != 0)
				{
					if (num == 1)
					{
						TextMeshProUGUI textMeshPro = m_TextMeshPro;
						textMeshPro.alignment = NativeIntrinsics.Reinterpret<TextAlignmentOptions>(1028);
						frameCounter_transform = m_frameCounter_transform;
						frameCounter_transform.pivot = NativeIntrinsics.Reinterpret<Vector2>(NativeIntrinsics.Asm("unpcklps xmm0,xmm8"));
						frameCounter_transform = m_frameCounter_transform;
						goto IL_00a6;
					}
					return;
				}
				NativeIntrinsics.Reinterpret<TMP_Text>(NativeIntrinsics.Asm("*(LocalExpr + 0x38)")).alignment = NativeIntrinsics.Reinterpret<TextAlignmentOptions>(260);
				m_frameCounter_transform.pivot = NativeIntrinsics.Reinterpret<Vector2>(NativeIntrinsics.Asm("unpcklps xmm0,xmm6"));
			}
			else
			{
				NativeIntrinsics.Reinterpret<TMP_Text>(NativeIntrinsics.Asm("*(LocalExpr + 0x38)")).alignment = NativeIntrinsics.Reinterpret<TextAlignmentOptions>(1025);
				m_frameCounter_transform.pivot = NativeIntrinsics.Reinterpret<Vector2>(NativeIntrinsics.Asm("unpcklps xmm0,xmm6"));
			}
			frameCounter_transform = m_frameCounter_transform;
			frameCounter_transform.anchorMin = NativeIntrinsics.Reinterpret<Vector2>(NativeIntrinsics.Asm("unpcklps xmm0,xmm7"));
			frameCounter_transform = m_frameCounter_transform;
			frameCounter_transform.anchorMax = NativeIntrinsics.Reinterpret<Vector2>(NativeIntrinsics.Asm("unpcklps xmm7,xmm7"));
			frameCounter_transform = m_frameCounter_transform;
			num2 = NativeIntrinsics.Asm("unpcklps xmm6,xmm6");
			goto IL_00f4;
		}
		long num3 = NativeIntrinsics.Asm("*(LocalExpr + 0x38)");
		NativeIntrinsics.Reinterpret<TMP_Text>(num3).alignment = NativeIntrinsics.Reinterpret<TextAlignmentOptions>(257);
		RectTransform frameCounter_transform2 = m_frameCounter_transform;
		frameCounter_transform2.pivot = NativeIntrinsics.Reinterpret<Vector2>(NativeIntrinsics.Asm("unpcklps xmm0,xmm8"));
		frameCounter_transform2 = m_frameCounter_transform;
		goto IL_00a6;
		IL_00a6:
		frameCounter_transform.anchorMin = NativeIntrinsics.Reinterpret<Vector2>(NativeIntrinsics.Asm("unpcklps xmm0,xmm9"));
		frameCounter_transform = m_frameCounter_transform;
		frameCounter_transform.anchorMax = NativeIntrinsics.Reinterpret<Vector2>(NativeIntrinsics.Asm("unpcklps xmm7,xmm9"));
		frameCounter_transform = m_frameCounter_transform;
		num2 = NativeIntrinsics.Asm("unpcklps xmm6,xmm8");
		goto IL_00f4;
		IL_00f4:
		frameCounter_transform.anchoredPosition = NativeIntrinsics.Reinterpret<Vector2>(num2);
	}
}

