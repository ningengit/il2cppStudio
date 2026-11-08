using Il2CppStudio.Native;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x0200007D RID: 125
public class TMPro_InstructionOverlay : MonoBehaviour
{
	// Token: 0x0200007E RID: 126
	public enum FpsCounterAnchorPositions
	{
		// Token: 0x04000339 RID: 825 Offset: 0x0
		TopLeft,
		// Token: 0x0400033A RID: 826 Offset: 0x0
		BottomLeft,
		// Token: 0x0400033B RID: 827 Offset: 0x0
		TopRight,
		// Token: 0x0400033C RID: 828 Offset: 0x0
		BottomRight
	}

	// Token: 0x04000332 RID: 818 Offset: 0x20
	public FpsCounterAnchorPositions AnchorPosition = NativeIntrinsics.Reinterpret<FpsCounterAnchorPositions>(1);

	// Token: 0x04000333 RID: 819 Offset: 0x0
	private const string instructions = "Camera Control - <#ffff00>Shift + RMB\n</color>Zoom - <#ffff00>Mouse wheel.";

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
		if (enabled)
		{
			m_camera = Camera.main;
			GameObject gameObject = new GameObject("Frame Counter");
			m_frameCounter_transform = gameObject.transform;
			m_frameCounter_transform.parent = m_camera.transform;
			Transform frameCounter_transform = m_frameCounter_transform;
			Quaternion localRotation = default(Quaternion);
			localRotation.x = NativeIntrinsics.Reinterpret<float>(Quaternion.identityQuaternion);
			frameCounter_transform.localRotation = localRotation;
			m_TextMeshPro = gameObject.AddComponent<TextMeshPro>();
			m_TextMeshPro.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
			m_TextMeshPro.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - Overlay");
			m_TextMeshPro.fontSize = 30f;
			m_TextMeshPro.isOverlay = true;
			m_textContainer = gameObject.GetComponent<TextContainer>();
			Set_FrameCounter_Position(AnchorPosition);
			m_TextMeshPro.text = "Camera Control - <#ffff00>Shift + RMB\n</color>Zoom - <#ffff00>Mouse wheel.";
		}
	}

	// Token: 0x0600023C RID: 572 RVA: 0x0025F5E0 File Offset: 0x0025DBE0
	private void Set_FrameCounter_Position(FpsCounterAnchorPositions anchor_position)
	{
		Camera camera;
		if ((NativeIntrinsics.Reinterpret<int>(anchor_position) & NativeIntrinsics.Reinterpret<int>(anchor_position)) != 0)
		{
			int num = NativeIntrinsics.Reinterpret<int>(anchor_position) - 1;
			if (num != 0)
			{
				switch (num - 1)
				{
				case 1:
					m_textContainer.anchorPosition = TextContainerAnchors.BottomRight;
					camera = m_camera;
					break;
				case 0:
					m_textContainer.anchorPosition = TextContainerAnchors.TopRight;
					camera = m_camera;
					break;
				default:
					return;
				}
			}
			else
			{
				m_textContainer.anchorPosition = TextContainerAnchors.BottomLeft;
				camera = m_camera;
			}
		}
		else
		{
			m_textContainer.anchorPosition = TextContainerAnchors.TopLeft;
			camera = m_camera;
		}
		Transform frameCounter_transform = m_frameCounter_transform;
		Camera camera2 = camera;
		Vector3 position = default(Vector3);
		position.z = 100f;
		frameCounter_transform.position = camera2.ViewportToWorldPoint(position);
	}
}

