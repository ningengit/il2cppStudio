using Il2CppStudio.Native;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x0200007D RID: 125
public class TMPro_InstructionOverlay : MonoBehaviour
{
	// Token: 0x0200007E RID: 126
	public enum FpsCounterAnchorPositions
	{
		// error: enumerator has no value
		// Token: 0x04000339 RID: 825 Offset: 0x0
		TopLeft,
		// error: enumerator has no value
		// Token: 0x0400033A RID: 826 Offset: 0x0
		BottomLeft,
		// error: enumerator has no value
		// Token: 0x0400033B RID: 827 Offset: 0x0
		TopRight,
		// error: enumerator has no value
		// Token: 0x0400033C RID: 828 Offset: 0x0
		BottomRight
	}

	// Token: 0x04000332 RID: 818 Offset: 0x20
	public FpsCounterAnchorPositions AnchorPosition = NativeIntrinsics.Reinterpret<FpsCounterAnchorPositions>(1);

	// Token: 0x04000333 RID: 819 Offset: 0x0
	private const string instructions = null;

	// Token: 0x04000334 RID: 820 Offset: 0x28
	private TextMeshPro m_TextMeshPro;

	// Token: 0x04000335 RID: 821 Offset: 0x30
	private TextContainer m_textContainer;

	// Token: 0x04000336 RID: 822 Offset: 0x38
	private Transform m_frameCounter_transform;

	// Token: 0x04000337 RID: 823 Offset: 0x40
	private Camera m_camera;

	// Token: 0x0600023B RID: 571 RVA: 0x0025F340 File Offset: 0x0025D940
	private void Awake()
	{
		bool flag = enabled;
		bool flag2 = flag;
		if (flag)
		{
			Camera main = Camera.main;
			m_camera = main;
			GameObject gameObject = new GameObject("Frame Counter");
			Transform frameCounter_transform = gameObject.transform;
			m_frameCounter_transform = frameCounter_transform;
			Transform frameCounter_transform2 = m_frameCounter_transform;
			Transform transform = m_camera.transform;
			Transform parent = transform;
			frameCounter_transform2.parent = parent;
			Transform frameCounter_transform3 = m_frameCounter_transform;
			Quaternion localRotation = default(Quaternion);
			localRotation.x = NativeIntrinsics.Reinterpret<float>(Quaternion.identityQuaternion);
			frameCounter_transform3.localRotation = localRotation;
			TextMeshPro textMeshPro = gameObject.AddComponent<TextMeshPro>();
			m_TextMeshPro = textMeshPro;
			TextMeshPro textMeshPro2 = m_TextMeshPro;
			TMP_FontAsset tMP_FontAsset = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
			TMP_FontAsset font = tMP_FontAsset;
			textMeshPro2.font = font;
			TextMeshPro textMeshPro3 = m_TextMeshPro;
			Material material = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - Overlay");
			Material material2 = material;
			m_TextMeshPro.fontSize = 30f;
			m_TextMeshPro.isOverlay = true;
			TextContainer component = gameObject.GetComponent<TextContainer>();
			m_textContainer = component;
			Set_FrameCounter_Position(AnchorPosition);
		}
	}

	// Token: 0x0600023C RID: 572 RVA: 0x0025F5E0 File Offset: 0x0025DBE0
	private void Set_FrameCounter_Position(FpsCounterAnchorPositions anchor_position)
	{
		Camera camera;
		if ((NativeIntrinsics.Reinterpret<int>(anchor_position) & NativeIntrinsics.Reinterpret<int>(anchor_position)) != 0)
		{
			int num = NativeIntrinsics.Reinterpret<int>(anchor_position) - 1;
			if (NativeIntrinsics.Reinterpret<int>(anchor_position) - 1 != 0)
			{
				num--;
				if (num - 1 != 0)
				{
					if (num != 1)
					{
						return;
					}
					m_textContainer.anchorPosition = NativeIntrinsics.Reinterpret<TextContainerAnchors>(8);
					camera = m_camera;
				}
				else
				{
					m_textContainer.anchorPosition = NativeIntrinsics.Reinterpret<TextContainerAnchors>(2);
					camera = m_camera;
				}
			}
			else
			{
				m_textContainer.anchorPosition = NativeIntrinsics.Reinterpret<TextContainerAnchors>(6);
				camera = m_camera;
			}
		}
		else
		{
			m_textContainer.anchorPosition = NativeIntrinsics.Reinterpret<TextContainerAnchors>(0);
			camera = m_camera;
		}
		Transform frameCounter_transform = m_frameCounter_transform;
		Vector3 vector = camera.ViewportToWorldPoint(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFF8]") + -32L));
		frameCounter_transform.position = NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFF8]") + -32L);
	}
}

