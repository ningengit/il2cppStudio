using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MaterialKit;

// Token: 0x02000097 RID: 151
public class DpCanvasScaler : UIBehaviour
{
	// Token: 0x040003C2 RID: 962 Offset: 0x20
	protected float m_ReferencePixelsPerUnit = 100f;

	// Token: 0x040003C3 RID: 963 Offset: 0x0
	private const float kLogBase = 2f;

	// Token: 0x040003C4 RID: 964 Offset: 0x24
	protected float m_FallbackScreenDPI = 96f;

	// Token: 0x040003C5 RID: 965 Offset: 0x28
	protected float m_DefaultSpriteDPI = 96f;

	// Token: 0x040003C6 RID: 966 Offset: 0x2C
	protected float m_DynamicPixelsPerUnit = 1f;

	// Token: 0x040003C7 RID: 967 Offset: 0x30
	private Canvas m_Canvas;

	// Token: 0x040003C8 RID: 968 Offset: 0x38
	[NonSerialized]
	private float m_PrevScaleFactor = 1f;

	// Token: 0x040003C9 RID: 969 Offset: 0x3C
	[NonSerialized]
	private float m_PrevReferencePixelsPerUnit = 100f;

	public float referencePixelsPerUnit
	{
		// Token: 0x060002B0 RID: 688 RVA: 0x00263FB0 File Offset: 0x002625B0
		get
		{
			return m_ReferencePixelsPerUnit;
		}
		// Token: 0x060002B1 RID: 689 RVA: 0x00263FF0 File Offset: 0x002625F0
		set
		{
			m_ReferencePixelsPerUnit = value;
		}
	}

	public float fallbackScreenDPI
	{
		// Token: 0x060002B2 RID: 690 RVA: 0x00263FA0 File Offset: 0x002625A0
		get
		{
			return m_FallbackScreenDPI;
		}
		// Token: 0x060002B3 RID: 691 RVA: 0x00263FE0 File Offset: 0x002625E0
		set
		{
			m_FallbackScreenDPI = value;
		}
	}

	public float defaultSpriteDPI
	{
		// Token: 0x060002B4 RID: 692 RVA: 0x00263F80 File Offset: 0x00262580
		get
		{
			return m_DefaultSpriteDPI;
		}
		// Token: 0x060002B5 RID: 693 RVA: 0x00263FC0 File Offset: 0x002625C0
		set
		{
			m_DefaultSpriteDPI = value;
		}
	}

	public float dynamicPixelsPerUnit
	{
		// Token: 0x060002B6 RID: 694 RVA: 0x00263F90 File Offset: 0x00262590
		get
		{
			return m_DynamicPixelsPerUnit;
		}
		// Token: 0x060002B7 RID: 695 RVA: 0x00263FD0 File Offset: 0x002625D0
		set
		{
			m_DynamicPixelsPerUnit = value;
		}
	}

	// Token: 0x060002AF RID: 687 RVA: 0x00263F40 File Offset: 0x00262540
	protected DpCanvasScaler()
	{
	}

	// Token: 0x060002B8 RID: 696 RVA: 0x00263E30 File Offset: 0x00262430
	protected override void OnEnable()
	{
		OnEnable();
		m_Canvas = GetComponent<Canvas>();
		Handle();
	}

	// Token: 0x060002B9 RID: 697 RVA: 0x00263DB0 File Offset: 0x002623B0
	protected override void OnDisable()
	{
		if (1f != m_PrevScaleFactor)
		{
			m_Canvas.scaleFactor = 1f;
			m_PrevScaleFactor = 1f;
		}
		if (100f != m_PrevReferencePixelsPerUnit)
		{
			m_Canvas.referencePixelsPerUnit = 100f;
			m_PrevReferencePixelsPerUnit = 100f;
		}
		OnDisable();
	}

	// Token: 0x060002BA RID: 698 RVA: 0x00263F20 File Offset: 0x00262520
	protected virtual void Update()
	{
		Handle();
	}

	// Token: 0x060002BB RID: 699 RVA: 0x00263CF0 File Offset: 0x002622F0
	protected virtual void Handle()
	{
		if (!(m_Canvas == null) && m_Canvas.isRootCanvas)
		{
			if (m_Canvas.renderMode != RenderMode.WorldSpace)
			{
				HandleConstantPhysicalSize();
			}
			else
			{
				HandleWorldCanvas();
			}
		}
	}

	// Token: 0x060002BC RID: 700 RVA: 0x00263C80 File Offset: 0x00262280
	protected virtual void HandleWorldCanvas()
	{
		float num = m_DynamicPixelsPerUnit;
		if (m_DynamicPixelsPerUnit != m_PrevScaleFactor)
		{
			m_Canvas.scaleFactor = num;
			m_PrevScaleFactor = num;
		}
		num = m_ReferencePixelsPerUnit;
		if (m_ReferencePixelsPerUnit != m_PrevReferencePixelsPerUnit)
		{
			m_Canvas.referencePixelsPerUnit = num;
			m_PrevReferencePixelsPerUnit = num;
		}
	}

	// Token: 0x060002BD RID: 701 RVA: 0x00263BE0 File Offset: 0x002621E0
	protected virtual void HandleConstantPhysicalSize()
	{
		float dpi = Screen.dpi;
		float num = dpi;
		if (dpi == 0f)
		{
			num = m_FallbackScreenDPI;
		}
		num /= 160f;
		if (num / 160f != m_PrevScaleFactor)
		{
			m_Canvas.scaleFactor = num;
			m_PrevScaleFactor = num;
		}
		num = m_ReferencePixelsPerUnit * 160f / m_DefaultSpriteDPI;
		if (m_ReferencePixelsPerUnit * 160f / m_DefaultSpriteDPI != m_PrevReferencePixelsPerUnit)
		{
			m_Canvas.referencePixelsPerUnit = num;
			m_PrevReferencePixelsPerUnit = num;
		}
	}

	// Token: 0x060002BE RID: 702 RVA: 0x00263EE0 File Offset: 0x002624E0
	protected void SetScaleFactor(float scaleFactor)
	{
		if (scaleFactor != m_PrevScaleFactor)
		{
			m_Canvas.scaleFactor = scaleFactor;
			m_PrevScaleFactor = scaleFactor;
		}
	}

	// Token: 0x060002BF RID: 703 RVA: 0x00263EA0 File Offset: 0x002624A0
	protected void SetReferencePixelsPerUnit(float referencePixelsPerUnit)
	{
		if (referencePixelsPerUnit != m_PrevReferencePixelsPerUnit)
		{
			m_Canvas.referencePixelsPerUnit = referencePixelsPerUnit;
			m_PrevReferencePixelsPerUnit = referencePixelsPerUnit;
		}
	}
}

