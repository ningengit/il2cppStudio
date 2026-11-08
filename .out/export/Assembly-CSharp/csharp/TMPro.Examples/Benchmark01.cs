using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using Mono.Xml;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x02000064 RID: 100
public class Benchmark01 : MonoBehaviour
{
	// Token: 0x02000065 RID: 101
	private sealed class <Start>d__10 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x0400029C RID: 668 Offset: 0x10
		private int <>1__state;

		// Token: 0x0400029D RID: 669 Offset: 0x18
		private object <>2__current;

		// Token: 0x0400029E RID: 670 Offset: 0x20
		public Benchmark01 <>4__this;

		// Token: 0x0400029F RID: 671 Offset: 0x28
		private int <i>5__2;

		private object System.Collections.Generic.IEnumerator<System.Object>.Current => <>2__current;

		private object System.Collections.IEnumerator.Current => <>2__current;

		// Token: 0x060001D5 RID: 469 RVA: 0x00250B50 File Offset: 0x0024F150
		public <Start>d__10(int <>1__state)
		{
			((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
			this.<>1__state = <>1__state;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00250B40 File Offset: 0x0024F140
		private void System.IDisposable.Dispose()
		{
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00261DB0 File Offset: 0x002603B0
		private bool MoveNext()
		{
			Benchmark01 benchmark = <>4__this;
			int num = <>1__state;
			if (<>1__state != 0)
			{
				num--;
				if (num - 1 != 0)
				{
					if (num == 1)
					{
						<>1__state = -1;
					}
				}
				else
				{
					int num2 = <i>5__2;
					<>1__state = -1;
					<i>5__2 = (int)(num2 + 1L);
					long num3 = num2 + 1L;
					if ((int)(num2 + 1L) <= 1000000)
					{
						goto IL_0299;
					}
					<>2__current = null;
					<>1__state = 2;
				}
				goto IL_0096;
			}
			<>1__state = -1;
			if (benchmark.BenchmarkType != 0)
			{
				if (benchmark.BenchmarkType == 1)
				{
					GameObject gameObject = benchmark.gameObject;
					TextMesh textMesh = gameObject.AddComponent<TextMesh>();
					benchmark.m_textMesh = textMesh;
					Font textMeshFont = benchmark.TextMeshFont;
					bool flag = textMeshFont != null;
					TextMesh textMesh2 = benchmark.m_textMesh;
					Font textMeshFont2 = default(Font);
					if (!flag)
					{
						Type typeFromHandle = Type.GetTypeFromHandle(NativeIntrinsics.Reinterpret<RuntimeTypeHandle>((nint)NativeIntrinsics.Asm("typeof(Font)")));
						UnityEngine.Object obj = Resources.Load("Fonts/ARIAL", typeFromHandle);
						UnityEngine.Object obj2 = obj;
						if ((object)obj != null)
						{
							UnityEngine.Object obj3 = ((NativeIntrinsics.Asm("LocalExpr->klass") != NativeIntrinsics.Asm("Font_TypeInfo")) ? null : obj2);
							UnityEngine.Object obj4 = obj3;
						}
					}
					else
					{
						textMeshFont2 = benchmark.TextMeshFont;
					}
					textMesh2.font = textMeshFont2;
					Renderer component = benchmark.m_textMesh.GetComponent<Renderer>();
					Font font = benchmark.m_textMesh.font;
					Material material = font.material;
					component.SetMaterial(material);
					benchmark.m_textMesh.fontSize = 48;
					benchmark.m_textMesh.anchor = NativeIntrinsics.Reinterpret<TextAnchor>(4);
				}
			}
			else
			{
				GameObject gameObject2 = benchmark.gameObject;
				TextMeshPro textMeshPro = gameObject2.AddComponent<TextMeshPro>();
				benchmark.m_textMeshPro = textMeshPro;
				TextMeshPro textMeshPro2 = benchmark.m_textMeshPro;
				TMP_FontAsset tMProFont = benchmark.TMProFont;
				if (tMProFont != null)
				{
					benchmark.m_textMeshPro.font = benchmark.TMProFont;
				}
				benchmark.m_textMeshPro.fontSize = 48f;
				benchmark.m_textMeshPro.alignment = NativeIntrinsics.Reinterpret<TextAlignmentOptions>(514);
				benchmark.m_textMeshPro.extraPadding = true;
				benchmark.m_textMeshPro.enableWordWrapping = false;
				Material material2 = ((TMP_Text)benchmark.m_textMeshPro).m_fontAsset.material;
				benchmark.m_material01 = material2;
				Material material3 = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - Drop Shadow");
				benchmark.m_material02 = material3;
			}
			<i>5__2 = 0;
			goto IL_0299;
			IL_0096:
			return true;
			IL_0299:
			if (benchmark.BenchmarkType != 0)
			{
				if (benchmark.BenchmarkType == 1)
				{
					TextMesh textMesh3 = benchmark.m_textMesh;
					int num4 = default(int);
					num4.m_value = (int)NativeIntrinsics.Asm("imul ecx") - (((int)NativeIntrinsics.Asm("rdx") >> 6) + ((int)NativeIntrinsics.Asm("rdx") >> 6 >>> 31)) * 1000;
					string text = num4.ToString();
					string text2 = "The <color=#0050FF>count is: </color>" + text;
					textMesh3.text = text2;
				}
			}
			else
			{
				TextMeshPro textMeshPro3 = benchmark.m_textMeshPro;
				textMeshPro3.SetText("The <#0050FF>count is: </color>{0}", (int)NativeIntrinsics.Asm("imul ecx") - (((int)NativeIntrinsics.Asm("rdx") >> 6) + ((int)NativeIntrinsics.Asm("rdx") >> 6 >>> 31)) * 1000);
				if ((int)NativeIntrinsics.Asm("imul ecx") - (((int)NativeIntrinsics.Asm("rdx") >> 6) + ((int)NativeIntrinsics.Asm("rdx") >> 6 >>> 31)) * 1000 == 999)
				{
					TextMeshPro textMeshPro2 = benchmark.m_textMeshPro;
					Material material4 = benchmark.m_material01;
					TextMeshPro textMeshPro4 = textMeshPro2;
					if (NativeIntrinsics.Reinterpret<UnityEngine.Object>((nint)NativeIntrinsics.Asm("LocalExpr->klass")) == material4)
					{
						Material material5 = benchmark.m_material02;
					}
					else
					{
						Material material5 = benchmark.m_material01;
					}
				}
			}
			<>2__current = null;
			<>1__state = 1;
			goto IL_0096;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00262800 File Offset: 0x00260E00
		private void System.Collections.IEnumerator.Reset()
		{
			long num = NativeIntrinsics.CallNative("sub_184BE0", NativeIntrinsics.Asm("rax"), 0L, 0L, 0L);
			NativeIntrinsics.Reinterpret<NotSupportedException>(num)..ctor();
			throw null;
		}
	}

	// Token: 0x04000292 RID: 658 Offset: 0x20
	public int BenchmarkType;

	// Token: 0x04000293 RID: 659 Offset: 0x28
	public TMP_FontAsset TMProFont;

	// Token: 0x04000294 RID: 660 Offset: 0x30
	public Font TextMeshFont;

	// Token: 0x04000295 RID: 661 Offset: 0x38
	private TextMeshPro m_textMeshPro;

	// Token: 0x04000296 RID: 662 Offset: 0x40
	private TextContainer m_textContainer;

	// Token: 0x04000297 RID: 663 Offset: 0x48
	private TextMesh m_textMesh;

	// Token: 0x04000298 RID: 664 Offset: 0x0
	private const string label01 = null;

	// Token: 0x04000299 RID: 665 Offset: 0x0
	private const string label02 = null;

	// Token: 0x0400029A RID: 666 Offset: 0x50
	private Material m_material01;

	// Token: 0x0400029B RID: 667 Offset: 0x58
	private Material m_material02;

	// Token: 0x060001D3 RID: 467 RVA: 0x00252940 File Offset: 0x00250F40
	private IEnumerator Start()
	{
		<Start>d__10 <Start>d__ = NativeIntrinsics.Reinterpret<<Start>d__10>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.Benchmark01+<Start>d__10)"));
		((Mono.Xml.SecurityParser)(object)<Start>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<Start>d__.<>1__state = 0;
		<Start>d__.<>4__this = this;
		return <Start>d__;
	}
}

