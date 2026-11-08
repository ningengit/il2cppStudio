using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
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
				int result;
				if (num != 0)
				{
					result = 0;
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
					if ((int)(num2 + 1L) <= 1000000)
					{
						goto IL_028a;
					}
					<>2__current = null;
					<>1__state = 2;
					result = 1;
				}
				return (byte)result != 0;
			}
			<>1__state = -1;
			if (benchmark01_UGUI.BenchmarkType == 0)
			{
				benchmark01_UGUI.m_textMeshPro = benchmark01_UGUI.gameObject.AddComponent<TextMeshProUGUI>();
				if (benchmark01_UGUI.TMProFont != null)
				{
					benchmark01_UGUI.m_textMeshPro.font = benchmark01_UGUI.TMProFont;
				}
				benchmark01_UGUI.m_textMeshPro.fontSize = 48f;
				benchmark01_UGUI.m_textMeshPro.alignment = TextAlignmentOptions.Center;
				benchmark01_UGUI.m_textMeshPro.extraPadding = true;
				benchmark01_UGUI.m_material01 = ((TMP_Text)benchmark01_UGUI.m_textMeshPro).m_fontAsset.material;
				benchmark01_UGUI.m_material02 = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - BEVEL");
			}
			else if (benchmark01_UGUI.BenchmarkType == 1)
			{
				benchmark01_UGUI.m_textMesh = benchmark01_UGUI.gameObject.AddComponent<Text>();
				if (benchmark01_UGUI.TextMeshFont != null)
				{
					benchmark01_UGUI.m_textMesh.font = benchmark01_UGUI.TextMeshFont;
				}
				benchmark01_UGUI.m_textMesh.fontSize = 48;
				benchmark01_UGUI.m_textMesh.alignment = TextAnchor.MiddleCenter;
			}
			<i>5__2 = 0;
			goto IL_028a;
			IL_028a:
			if (benchmark01_UGUI.BenchmarkType == 0)
			{
				TextMeshProUGUI textMeshPro = benchmark01_UGUI.m_textMeshPro;
				int num4 = default(int);
				num4.m_value = (int)NativeIntrinsics.Asm("imul ecx") - (((int)NativeIntrinsics.Asm("rdx") >> 6) + ((int)NativeIntrinsics.Asm("rdx") >> 6 >>> 31)) * 1000;
				textMeshPro.text = "The <#0050FF>count is: </color>" + num4;
				if ((int)NativeIntrinsics.Asm("imul ecx") - (((int)NativeIntrinsics.Asm("rdx") >> 6) + ((int)NativeIntrinsics.Asm("rdx") >> 6 >>> 31)) * 1000 == 999)
				{
					TextMeshProUGUI textMeshPro2 = benchmark01_UGUI.m_textMeshPro;
					Material fontSharedMaterial = textMeshPro2.fontSharedMaterial;
					Material material = benchmark01_UGUI.m_material01;
					Material material2 = fontSharedMaterial;
					TextMeshProUGUI textMeshProUGUI = textMeshPro2;
					long num5 = default(long);
					if (material2 == material)
					{
						_ = benchmark01_UGUI.m_material02;
					}
					else
					{
						num5 = NativeIntrinsics.Asm("*(LocalExpr + 0x50)");
					}
					benchmark01_UGUI.m_textMeshPro.fontSharedMaterial = NativeIntrinsics.Reinterpret<Material>(num5);
					textMeshProUGUI.fontSharedMaterial = NativeIntrinsics.Reinterpret<Material>(num5);
				}
			}
			else if (benchmark01_UGUI.BenchmarkType == 1)
			{
				Text textMesh = benchmark01_UGUI.m_textMesh;
				int num3 = default(int);
				num3.m_value = (int)NativeIntrinsics.Asm("imul ecx") - (((int)NativeIntrinsics.Asm("rdx") >> 6) + ((int)NativeIntrinsics.Asm("rdx") >> 6 >>> 31)) * 1000;
				textMesh.text = "The <color=#0050FF>count is: </color>" + num3;
			}
			<>2__current = null;
			<>1__state = 1;
			return true;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00262840 File Offset: 0x00260E40
		private void System.Collections.IEnumerator.Reset()
		{
			throw new NotSupportedException();
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
	private const string label01 = "The <#0050FF>count is: </color>";

	// Token: 0x040002A7 RID: 679 Offset: 0x0
	private const string label02 = "The <color=#0050FF>count is: </color>";

	// Token: 0x040002A8 RID: 680 Offset: 0x50
	private Material m_material01;

	// Token: 0x040002A9 RID: 681 Offset: 0x58
	private Material m_material02;

	// Token: 0x060001DB RID: 475 RVA: 0x002529B0 File Offset: 0x00250FB0
	private IEnumerator Start()
	{
		<Start>d__10 <Start>d__ = new <Start>d__10(0);
		<Start>d__.<>1__state = 0;
		<Start>d__.<>4__this = this;
		return <Start>d__;
	}
}

