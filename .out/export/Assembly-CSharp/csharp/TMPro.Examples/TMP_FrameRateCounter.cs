using Il2CppStudio.Native;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x02000081 RID: 129
public class TMP_FrameRateCounter : MonoBehaviour
{
	// Token: 0x02000082 RID: 130
	public enum FpsCounterAnchorPositions
	{
		// error: enumerator has no value
		// Token: 0x04000350 RID: 848 Offset: 0x0
		TopLeft,
		// error: enumerator has no value
		// Token: 0x04000351 RID: 849 Offset: 0x0
		BottomLeft,
		// error: enumerator has no value
		// Token: 0x04000352 RID: 850 Offset: 0x0
		TopRight,
		// error: enumerator has no value
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
	private const string fpsLabel = null;

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
		bool flag = enabled;
		bool flag2 = flag;
		if (flag)
		{
			Camera main = Camera.main;
			m_camera = main;
			Application.targetFrameRate = -1;
			GameObject gameObject = new GameObject("Frame Counter");
			TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
			m_TextMeshPro = textMeshPro;
			TextMeshPro textMeshPro2 = m_TextMeshPro;
			TMP_FontAsset tMP_FontAsset = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
			TMP_FontAsset font = tMP_FontAsset;
			textMeshPro2.font = font;
			TextMeshPro textMeshPro3 = m_TextMeshPro;
			Material material = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - Overlay");
			Material material2 = material;
			Transform frameCounter_transform = gameObject.transform;
			m_frameCounter_transform = frameCounter_transform;
			Transform frameCounter_transform2 = m_frameCounter_transform;
			Transform transform = m_camera.transform;
			Transform parentInternal = transform;
			frameCounter_transform2.parentInternal = parentInternal;
			Transform frameCounter_transform3 = m_frameCounter_transform;
			Quaternion localRotation = default(Quaternion);
			localRotation.x = NativeIntrinsics.Reinterpret<float>(Quaternion.identityQuaternion);
			frameCounter_transform3.localRotation = localRotation;
			m_TextMeshPro.enableWordWrapping = false;
			m_TextMeshPro.fontSize = 24f;
			m_TextMeshPro.isOverlay = true;
			Set_FrameCounter_Position(AnchorPosition);
			FpsCounterAnchorPositions anchorPosition = AnchorPosition;
			last_AnchorPosition = NativeIntrinsics.Reinterpret<FpsCounterAnchorPositions>(NativeIntrinsics.Reinterpret<int>(anchorPosition));
			FpsCounterAnchorPositions fpsCounterAnchorPositions = anchorPosition;
		}
	}

	// Token: 0x06000242 RID: 578 RVA: 0x0025A2E0 File Offset: 0x002588E0
	private void Start()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		m_LastInterval = realtimeSinceStartup;
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
		TextMeshPro textMeshPro = m_TextMeshPro;
		string text = htmlColorTag + "{0:2}</color> <#8080ff>FPS \n<#FF8000>{1:2} <#8080ff>MS";
		string sourceText = text;
		textMeshPro.SetText(sourceText, num4, arg);
		m_LastInterval = lastInterval;
		m_Frames = 0;
	}

	// Token: 0x06000244 RID: 580 RVA: 0x0025A080 File Offset: 0x00258680
	private void Set_FrameCounter_Position(FpsCounterAnchorPositions anchor_position)
	{
		int num = NativeIntrinsics.Reinterpret<int>(anchor_position);
		long num2 = NativeIntrinsics.Asm("global_13EACA0");
		Camera camera;
		if (num != 0)
		{
			num--;
			if (num - 1 != 0)
			{
				num--;
				if (num - 1 != 0)
				{
					if (num != 1)
					{
						return;
					}
					m_TextMeshPro.alignment = NativeIntrinsics.Reinterpret<TextAlignmentOptions>(1028);
					RectTransform rectTransform = m_TextMeshPro.rectTransform;
					rectTransform.pivot = NativeIntrinsics.Reinterpret<Vector2>(NativeIntrinsics.Asm("unpcklps xmm0,xmm1"));
					camera = m_camera;
				}
				else
				{
					m_TextMeshPro.alignment = NativeIntrinsics.Reinterpret<TextAlignmentOptions>(260);
					RectTransform rectTransform2 = m_TextMeshPro.rectTransform;
					rectTransform2.pivot = NativeIntrinsics.Reinterpret<Vector2>(NativeIntrinsics.Asm("unpcklps xmm0,xmm0"));
					camera = m_camera;
				}
			}
			else
			{
				m_TextMeshPro.alignment = NativeIntrinsics.Reinterpret<TextAlignmentOptions>(1025);
				RectTransform rectTransform3 = m_TextMeshPro.rectTransform;
				rectTransform3.pivot = NativeIntrinsics.Reinterpret<Vector2>(NativeIntrinsics.Asm("unpcklps xmm0,xmm0"));
				camera = m_camera;
			}
		}
		else
		{
			m_TextMeshPro.alignment = NativeIntrinsics.Reinterpret<TextAlignmentOptions>(257);
			RectTransform rectTransform4 = m_TextMeshPro.rectTransform;
			rectTransform4.pivot = NativeIntrinsics.Reinterpret<Vector2>(NativeIntrinsics.Asm("unpcklps xmm0,xmm1"));
			camera = m_camera;
		}
		Transform frameCounter_transform = m_frameCounter_transform;
		Vector3 vector = camera.ViewportToWorldPoint(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFF8]") + -32L));
		frameCounter_transform.position = NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFF8]") + -32L);
	}
}

