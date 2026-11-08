using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x0200006F RID: 111
public class ShaderPropAnimator : MonoBehaviour
{
	// Token: 0x040002E4 RID: 740 Offset: 0x20
	private Renderer m_Renderer;

	// Token: 0x040002E5 RID: 741 Offset: 0x28
	private Material m_Material;

	// Token: 0x040002E6 RID: 742 Offset: 0x30
	public AnimationCurve GlowCurve;

	// Token: 0x040002E7 RID: 743 Offset: 0x38
	public float m_frame;

	// Token: 0x060001F2 RID: 498 RVA: 0x00259060 File Offset: 0x00257660
	private void Awake()
	{
		m_Renderer = GetComponent<Renderer>();
		m_Material = m_Renderer.GetMaterial();
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x002590D0 File Offset: 0x002576D0
	private void Start()
	{
		StartCoroutine(AnimateProperties());
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x00258FF0 File Offset: 0x002575F0
	private IEnumerator AnimateProperties()
	{
		// Il2CppStudio: coroutine reconstructed from <AnimateProperties>d__6.MoveNext (state machine class hidden).
		m_frame = UnityEngine.Random.Range(0f, 1f);
		while (true)
		{
			float value = GlowCurve.Evaluate(m_frame);
			m_Material.SetFloatImpl(ShaderUtilities.ID_GlowPower, value);
			float frame = m_frame;
			float deltaTime = Time.deltaTime;
			m_frame = UnityEngine.Random.Range(0.2f, 0.3f) * deltaTime + frame;
			yield return new WaitForEndOfFrame();
		}
	}
}

