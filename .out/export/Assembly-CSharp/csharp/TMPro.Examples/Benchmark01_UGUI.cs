using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using Mono.Xml;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro.Examples;

// Token: 0x02000066 RID: 102
public class Benchmark01_UGUI : MonoBehaviour
{
	// Token: 0x02000067 RID: 103
	private sealed class <Start>d__10 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x040002AA RID: 682 Offset: 0x10
		private int <>1__state;

		// Token: 0x040002AB RID: 683 Offset: 0x18
		private object <>2__current;

		// Token: 0x040002AC RID: 684 Offset: 0x20
		public Benchmark01_UGUI <>4__this;

		// Token: 0x040002AD RID: 685 Offset: 0x28
		private int <i>5__2;

		private object System.Collections.Generic.IEnumerator<System.Object>.Current => <>2__current;

		private object System.Collections.IEnumerator.Current => <>2__current;

		// Token: 0x060001DD RID: 477 RVA: 0x00250B50 File Offset: 0x0024F150
		public <Start>d__10(int <>1__state)
		{
			((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
			this.<>1__state = <>1__state;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00250B40 File Offset: 0x0024F140
		private void System.IDisposable.Dispose()
		{
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00262350 File Offset: 0x00260950
		private bool MoveNext()
		{
			Benchmark01_UGUI benchmark01_UGUI = <>4__this;
			int num = <>1__state;
			if (<>1__state != 0)
			{
				num--;
				if (num - 1 != 0)
				{
					if (num == 1)
					{
						<>1__state = -1;
						return false;
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
						goto IL_01e8;
					}
					<>2__current = null;
					<>1__state = 2;
				}
				return true;
			}
			<>1__state = -1;
			if (benchmark01_UGUI.BenchmarkType != 0)
			{
				if (benchmark01_UGUI.BenchmarkType == 1)
				{
					GameObject gameObject = benchmark01_UGUI.gameObject;
					Text textMesh = gameObject.AddComponent<Text>();
					benchmark01_UGUI.m_textMesh = textMesh;
					Font textMeshFont = benchmark01_UGUI.TextMeshFont;
					if (textMeshFont != null)
					{
						benchmark01_UGUI.m_textMesh.font = benchmark01_UGUI.TextMeshFont;
					}
					benchmark01_UGUI.m_textMesh.fontSize = 48;
					benchmark01_UGUI.m_textMesh.alignment = NativeIntrinsics.Reinterpret<TextAnchor>(4);
				}
			}
			else
			{
				GameObject gameObject2 = benchmark01_UGUI.gameObject;
				TextMeshProUGUI textMeshPro = gameObject2.AddComponent<TextMeshProUGUI>();
				benchmark01_UGUI.m_textMeshPro = textMeshPro;
				TMP_FontAsset tMProFont = benchmark01_UGUI.TMProFont;
				if (tMProFont != null)
				{
					benchmark01_UGUI.m_textMeshPro.font = benchmark01_UGUI.TMProFont;
				}
				benchmark01_UGUI.m_textMeshPro.fontSize = 48f;
				benchmark01_UGUI.m_textMeshPro.alignment = NativeIntrinsics.Reinterpret<TextAlignmentOptions>(514);
				benchmark01_UGUI.m_textMeshPro.extraPadding = true;
				Material material = ((TMP_Text)benchmark01_UGUI.m_textMeshPro).m_fontAsset.material;
				benchmark01_UGUI.m_material01 = material;
				Material material2 = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - BEVEL");
				benchmark01_UGUI.m_material02 = material2;
			}
			<i>5__2 = 0;
			goto IL_01e8;
			IL_01e8:
			if (benchmark01_UGUI.BenchmarkType != 0)
			{
				if (benchmark01_UGUI.BenchmarkType == 1)
				{
					Text textMesh2 = benchmark01_UGUI.m_textMesh;
					int num4 = default(int);
					num4.m_value = (int)NativeIntrinsics.Asm("imul ecx") - (((int)NativeIntrinsics.Asm("rdx") >> 6) + ((int)NativeIntrinsics.Asm("rdx") >> 6 >>> 31)) * 1000;
					string text = num4.ToString();
					string text2 = "The <color=#0050FF>count is: </color>" + text;
				}
			}
			else
			{
				TextMeshProUGUI textMeshPro2 = benchmark01_UGUI.m_textMeshPro;
				int num5 = default(int);
				num5.m_value = (int)NativeIntrinsics.Asm("imul ecx") - (((int)NativeIntrinsics.Asm("rdx") >> 6) + ((int)NativeIntrinsics.Asm("rdx") >> 6 >>> 31)) * 1000;
				string text3 = num5.ToString();
				string value = "The <#0050FF>count is: </color>" + text3;
				if ((int)NativeIntrinsics.Asm("imul ecx") - ((NativeIntrinsics.Reinterpret<int>(value) >> 6) + (NativeIntrinsics.Reinterpret<int>(value) >> 6 >>> 31)) * 1000 == 999)
				{
					TextMeshProUGUI textMeshPro3 = benchmark01_UGUI.m_textMeshPro;
					Material material3 = benchmark01_UGUI.m_material01;
					TextMeshProUGUI textMeshProUGUI = textMeshPro3;
					if (NativeIntrinsics.Reinterpret<UnityEngine.Object>((nint)NativeIntrinsics.Asm("LocalExpr->klass")) == material3)
					{
						Material material4 = benchmark01_UGUI.m_material02;
					}
					else
					{
						long num6 = NativeIntrinsics.Asm("*(LocalExpr + 0x50)");
					}
				}
			}
			<>2__current = null;
			<>1__state = 1;
			return true;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00262840 File Offset: 0x00260E40
		private void System.Collections.IEnumerator.Reset()
		{
			long num = NativeIntrinsics.CallNative("sub_184BE0", NativeIntrinsics.Asm("rax"), 0L, 0L, 0L);
			NativeIntrinsics.Reinterpret<NotSupportedException>(num)..ctor();
			throw null;
		}
	}

	// Token: 0x040002A0 RID: 672 Offset: 0x20
	public int BenchmarkType;

	// Token: 0x040002A1 RID: 673 Offset: 0x28
	public Canvas canvas;

	// Token: 0x040002A2 RID: 674 Offset: 0x30
	public TMP_FontAsset TMProFont;

	// Token: 0x040002A3 RID: 675 Offset: 0x38
	public Font TextMeshFont;

	// Token: 0x040002A4 RID: 676 Offset: 0x40
	private TextMeshProUGUI m_textMeshPro;

	// Token: 0x040002A5 RID: 677 Offset: 0x48
	private Text m_textMesh;

	// Token: 0x040002A6 RID: 678 Offset: 0x0
	private const string label01 = null;

	// Token: 0x040002A7 RID: 679 Offset: 0x0
	private const string label02 = null;

	// Token: 0x040002A8 RID: 680 Offset: 0x50
	private Material m_material01;

	// Token: 0x040002A9 RID: 681 Offset: 0x58
	private Material m_material02;

	// Token: 0x060001DB RID: 475 RVA: 0x002529B0 File Offset: 0x00250FB0
	private IEnumerator Start()
	{
		<Start>d__10 <Start>d__ = NativeIntrinsics.Reinterpret<<Start>d__10>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.Benchmark01_UGUI+<Start>d__10)"));
		((Mono.Xml.SecurityParser)(object)<Start>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<Start>d__.<>1__state = 0;
		<Start>d__.<>4__this = this;
		return <Start>d__;
	}
}

