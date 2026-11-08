using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using Mono.Globalization.Unicode;
using Mono.Xml;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x0200006F RID: 111
public class ShaderPropAnimator : MonoBehaviour
{
	// Token: 0x02000070 RID: 112
	private sealed class <AnimateProperties>d__6 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x040002E8 RID: 744 Offset: 0x10
		private int <>1__state;

		// Token: 0x040002E9 RID: 745 Offset: 0x18
		private object <>2__current;

		// Token: 0x040002EA RID: 746 Offset: 0x20
		public ShaderPropAnimator <>4__this;

		private object System.Collections.Generic.IEnumerator<System.Object>.Current => <>2__current;

		private object System.Collections.IEnumerator.Current => <>2__current;

		// Token: 0x060001F6 RID: 502 RVA: 0x00250B50 File Offset: 0x0024F150
		public <AnimateProperties>d__6(int <>1__state)
		{
			((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
			this.<>1__state = <>1__state;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00250B40 File Offset: 0x0024F140
		private void System.IDisposable.Dispose()
		{
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00260B50 File Offset: 0x0025F150
		private bool MoveNext()
		{
			int num = <>1__state;
			ShaderPropAnimator shaderPropAnimator = <>4__this;
			float frame;
			if (<>1__state == 0)
			{
				<>1__state = -1;
				float num2 = UnityEngine.Random.Range(0f, 1f);
				frame = num2;
				shaderPropAnimator.m_frame = frame;
			}
			else
			{
				if (num != 1)
				{
					goto IL_00d1;
				}
				<>1__state = -1;
			}
			float num3 = shaderPropAnimator.GlowCurve.Evaluate(shaderPropAnimator.m_frame);
			Material material = shaderPropAnimator.m_Material;
			float value = num3;
			frame = num3;
			material.SetFloatImpl(ShaderUtilities.ID_GlowPower, value);
			float frame2 = shaderPropAnimator.m_frame;
			float deltaTime = Time.deltaTime;
			float num4 = UnityEngine.Random.Range(0.2f, 0.3f);
			shaderPropAnimator.m_frame = num4 * deltaTime + frame2;
			WaitForEndOfFrame waitForEndOfFrame = (WaitForEndOfFrame)(object)new Mono.Globalization.Unicode.ContractionComparer();
			<>2__current = waitForEndOfFrame;
			<>1__state = 1;
			goto IL_00d1;
			IL_00d1:
			return false;
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00260CD0 File Offset: 0x0025F2D0
		private void System.Collections.IEnumerator.Reset()
		{
			long num = NativeIntrinsics.CallNative("sub_184BE0", NativeIntrinsics.Asm("rax"), 0L, 0L, 0L);
			NativeIntrinsics.Reinterpret<NotSupportedException>(num)..ctor();
			throw null;
		}
	}

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
		Renderer component = GetComponent<Renderer>();
		m_Renderer = component;
		Material material = m_Renderer.GetMaterial();
		m_Material = material;
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x002590D0 File Offset: 0x002576D0
	private void Start()
	{
		<AnimateProperties>d__6 <AnimateProperties>d__ = NativeIntrinsics.Reinterpret<<AnimateProperties>d__6>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.ShaderPropAnimator+<AnimateProperties>d__6)"));
		((Mono.Xml.SecurityParser)(object)<AnimateProperties>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<AnimateProperties>d__.<>1__state = 0;
		<AnimateProperties>d__.<>4__this = this;
		Coroutine coroutine = StartCoroutine(<AnimateProperties>d__);
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x00258FF0 File Offset: 0x002575F0
	private IEnumerator AnimateProperties()
	{
		<AnimateProperties>d__6 <AnimateProperties>d__ = NativeIntrinsics.Reinterpret<<AnimateProperties>d__6>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.ShaderPropAnimator+<AnimateProperties>d__6)"));
		((Mono.Xml.SecurityParser)(object)<AnimateProperties>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<AnimateProperties>d__.<>1__state = 0;
		<AnimateProperties>d__.<>4__this = this;
		return <AnimateProperties>d__;
	}
}

