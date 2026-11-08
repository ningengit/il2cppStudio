using System;
using Il2CppStudio.Native;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x02000081 RID: 129
public class TMP_FrameRateCounter : MonoBehaviour
{
	// Token: 0x02000082 RID: 130
	public enum FpsCounterAnchorPositions
	{
		// Token: 0x04000350 RID: 848 Offset: 0x0
		TopLeft,
		// Token: 0x04000351 RID: 849 Offset: 0x0
		BottomLeft,
		// Token: 0x04000352 RID: 850 Offset: 0x0
		TopRight,
		// Token: 0x04000353 RID: 851 Offset: 0x0
		BottomRight
	}

	// Token: 0x04000345 RID: 837 Offset: 0x20
	public float UpdateInterval = 5f;

	// Token: 0x04000346 RID: 838 Offset: 0x24
	private float m_LastInterval;

	// Token: 0x04000347 RID: 839 Offset: 0x28
	private int m_Frames;

	// Token: 0x04000348 RID: 840 Offset: 0x2C
	public FpsCounterAnchorPositions AnchorPosition = NativeIntrinsics.Reinterpret<FpsCounterAnchorPositions>(2);

	// Token: 0x04000349 RID: 841 Offset: 0x30
	private string htmlColorTag;

	// Token: 0x0400034A RID: 842 Offset: 0x0
	private const string fpsLabel = "{0:2}</color> <#8080ff>FPS \n<#FF8000>{1:2} <#8080ff>MS";

	// Token: 0x0400034B RID: 843 Offset: 0x38
	private TextMeshPro m_TextMeshPro;

	// Token: 0x0400034C RID: 844 Offset: 0x40
	private Transform m_frameCounter_transform;

	// Token: 0x0400034D RID: 845 Offset: 0x48
	private Camera m_camera;

	// Token: 0x0400034E RID: 846 Offset: 0x50
	private FpsCounterAnchorPositions last_AnchorPosition;

	// Token: 0x06000241 RID: 577 RVA: 0x00259E00 File Offset: 0x00258400
	private void Awake()
	{
		if (enabled)
		{
			m_camera = Camera.main;
			Application.targetFrameRate = -1;
			GameObject gameObject = new GameObject("Frame Counter");
			m_TextMeshPro = gameObject.AddComponent<TextMeshPro>();
			m_TextMeshPro.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
			m_TextMeshPro.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - Overlay");
			m_frameCounter_transform = gameObject.transform;
			m_frameCounter_transform.parentInternal = m_camera.transform;
			Transform frameCounter_transform = m_frameCounter_transform;
			Quaternion localRotation = default(Quaternion);
			localRotation.x = NativeIntrinsics.Reinterpret<float>(Quaternion.identityQuaternion);
			frameCounter_transform.localRotation = localRotation;
			m_TextMeshPro.enableWordWrapping = false;
			m_TextMeshPro.fontSize = 24f;
			m_TextMeshPro.isOverlay = true;
			Set_FrameCounter_Position(AnchorPosition);
			last_AnchorPosition = NativeIntrinsics.Reinterpret<FpsCounterAnchorPositions>(NativeIntrinsics.Reinterpret<int>(AnchorPosition));
		}
	}

	// Token: 0x06000242 RID: 578 RVA: 0x0025A2E0 File Offset: 0x002588E0
	private void Start()
	{
		m_LastInterval = Time.realtimeSinceStartup;
		m_Frames = 0;
	}

	// Token: 0x06000243 RID: 579 RVA: 0x0025A310 File Offset: 0x00258910
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

	// Token: 0x06000244 RID: 580 RVA: 0x0025A080 File Offset: 0x00258680
	private void Set_FrameCounter_Position(FpsCounterAnchorPositions anchor_position)
	{
		int num = NativeIntrinsics.Reinterpret<int>(anchor_position);
		TextMeshPro textMeshPro = m_TextMeshPro;
		Vector4 margin = new Vector4(1f, 1f, 1f, 1f);
		textMeshPro.margin = margin;
		Camera camera;
		if (num != 0)
		{
			num--;
			if (num != 0)
			{
				switch (num - 1)
				{
				case 1:
					m_TextMeshPro.alignment = TextAlignmentOptions.BottomRight;
					m_TextMeshPro.rectTransform.pivot = Vector2.right;
					camera = m_camera;
					break;
				case 0:
					m_TextMeshPro.alignment = TextAlignmentOptions.TopRight;
					m_TextMeshPro.rectTransform.pivot = Vector2.one;
					camera = m_camera;
					break;
				default:
					return;
				}
			}
			else
			{
				m_TextMeshPro.alignment = TextAlignmentOptions.BottomLeft;
				m_TextMeshPro.rectTransform.pivot = Vector2.zero;
				camera = m_camera;
			}
		}
		else
		{
			m_TextMeshPro.alignment = TextAlignmentOptions.TopLeft;
			m_TextMeshPro.rectTransform.pivot = Vector2.up;
			camera = m_camera;
		}
		Transform frameCounter_transform = m_frameCounter_transform;
		Camera camera2 = camera;
		Vector3 position = default(Vector3);
		position.z = 100f;
		frameCounter_transform.position = camera2.ViewportToWorldPoint(position);
	}
}

