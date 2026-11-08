using System;
using Il2CppStudio.Native;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x02000087 RID: 135
public class TMP_UiFrameRateCounter : MonoBehaviour
{
	// Token: 0x02000088 RID: 136
	public enum FpsCounterAnchorPositions
	{
		// Token: 0x0400037C RID: 892 Offset: 0x0
		TopLeft,
		// Token: 0x0400037D RID: 893 Offset: 0x0
		BottomLeft,
		// Token: 0x0400037E RID: 894 Offset: 0x0
		TopRight,
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
	private const string fpsLabel = "{0:2}</color> <#8080ff>FPS \n<#FF8000>{1:2} <#8080ff>MS";

	// Token: 0x04000378 RID: 888 Offset: 0x38
	private TextMeshProUGUI m_TextMeshPro;

	// Token: 0x04000379 RID: 889 Offset: 0x40
	private RectTransform m_frameCounter_transform;

	// Token: 0x0400037A RID: 890 Offset: 0x48
	private FpsCounterAnchorPositions last_AnchorPosition;

	// Token: 0x0600025F RID: 607 RVA: 0x0025ED40 File Offset: 0x0025D340
	private void Awake()
	{
		if (enabled)
		{
			Application.targetFrameRate = -1;
			GameObject gameObject = new GameObject("Frame Counter");
			m_frameCounter_transform = gameObject.AddComponent<RectTransform>();
			m_frameCounter_transform.SetParent(transform, worldPositionStays: false);
			m_TextMeshPro = gameObject.AddComponent<TextMeshProUGUI>();
			m_TextMeshPro.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
			m_TextMeshPro.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - Overlay");
			m_TextMeshPro.enableWordWrapping = false;
			m_TextMeshPro.fontSize = 36f;
			m_TextMeshPro.isOverlay = true;
			Set_FrameCounter_Position(AnchorPosition);
			last_AnchorPosition = NativeIntrinsics.Reinterpret<FpsCounterAnchorPositions>(NativeIntrinsics.Reinterpret<int>(AnchorPosition));
		}
	}

	// Token: 0x06000260 RID: 608 RVA: 0x0025A2E0 File Offset: 0x002588E0
	private void Start()
	{
		m_LastInterval = Time.realtimeSinceStartup;
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
		float lastInterval = realtimeSinceStartup;
		if (!(realtimeSinceStartup > m_LastInterval + UpdateInterval))
		{
			return;
		}
		_ = NativeIntrinsics.Reinterpret<long>(this) + 48L;
		float num = 30f;
		Math.Max((float)m_Frames / (num - m_LastInterval), 1E-05f);
		float arg = 1000f / Math.Max((float)m_Frames / (num - m_LastInterval), 1E-05f);
		float num2 = (float)m_Frames / (num - m_LastInterval);
		if ((30f > (float)m_Frames / (num - m_LastInterval)))
		{
			htmlColorTag = "<color=yellow>";
		}
		else if (!(10f > num2))
		{
			htmlColorTag = "<color=green>";
		}
		else
		{
			htmlColorTag = "<color=red>";
		}
		m_TextMeshPro.SetText(htmlColorTag + "{0:2}</color> <#8080ff>FPS \n<#FF8000>{1:2} <#8080ff>MS", num2, arg);
		m_LastInterval = lastInterval;
		m_Frames = 0;
	}

	// Token: 0x06000262 RID: 610 RVA: 0x0025EF60 File Offset: 0x0025D560
	private void Set_FrameCounter_Position(FpsCounterAnchorPositions anchor_position)
	{
		float y = default(float);
		TMP_UiFrameRateCounter tMP_UiFrameRateCounter = default(TMP_UiFrameRateCounter);
		float x = default(float);
		RectTransform frameCounter_transform = default(RectTransform);
		float num2 = default(float);
		if ((NativeIntrinsics.Reinterpret<int>(anchor_position) & NativeIntrinsics.Reinterpret<int>(anchor_position)) != 0)
		{
			int num = NativeIntrinsics.Reinterpret<int>(anchor_position) - 1;
			if (num != 0)
			{
				num--;
				if (num != 0)
				{
					if (num != 1)
					{
						return;
					}
					else
					{
						m_TextMeshPro.alignment = TextAlignmentOptions.BottomRight;
						m_frameCounter_transform.pivot = Vector2.right;
						frameCounter_transform = m_frameCounter_transform;
						x = 0.99f;
						y = 0.01f;
						tMP_UiFrameRateCounter = this;
						num2 = 1f;
						goto IL_00e2;
					}
				}
				m_TextMeshPro.alignment = TextAlignmentOptions.TopRight;
				m_frameCounter_transform.pivot = Vector2.one;
				x = 0.99f;
				num2 = 1f;
			}
			else
			{
				m_TextMeshPro.alignment = TextAlignmentOptions.BottomLeft;
				m_frameCounter_transform.pivot = Vector2.zero;
				x = 0.01f;
				num2 = 0f;
			}
			m_frameCounter_transform.anchorMin = new Vector2(x, x);
			m_frameCounter_transform.anchorMax = new Vector2(x, x);
			frameCounter_transform = m_frameCounter_transform;
			num2 = num2;
			goto IL_0117;
		}
		m_TextMeshPro.alignment = TextAlignmentOptions.TopLeft;
		m_frameCounter_transform.pivot = Vector2.up;
		_ = m_frameCounter_transform;
		x = 0.01f;
		y = 0.99f;
		tMP_UiFrameRateCounter = this;
		num2 = 0f;
		goto IL_00e2;
		IL_00e2:
		frameCounter_transform.anchorMin = new Vector2(x, y);
		tMP_UiFrameRateCounter.m_frameCounter_transform.anchorMax = new Vector2(x, y);
		frameCounter_transform = tMP_UiFrameRateCounter.m_frameCounter_transform;
		num2 = num2;
		goto IL_0117;
		IL_0117:
		frameCounter_transform.anchoredPosition = NativeIntrinsics.Reinterpret<Vector2>(num2);
	}
}

