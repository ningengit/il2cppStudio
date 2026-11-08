using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x02000064 RID: 100
public class Benchmark01 : MonoBehaviour
{
	// Token: 0x02000065 RID: 101
	private sealed class _Start_d__10 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x0400029C RID: 668 Offset: 0x10
		// Il2CppStudio: declared private; widened because native code inlined into other types accesses it.
		internal int __1__state;

		// Token: 0x0400029D RID: 669 Offset: 0x18
		private object __2__current;

		// Token: 0x0400029E RID: 670 Offset: 0x20
		public Benchmark01 __4__this;

		// Token: 0x0400029F RID: 671 Offset: 0x28
		private int _i_5__2;

		object System.Collections.Generic.IEnumerator<System.Object>.Current => __2__current;

		object System.Collections.IEnumerator.Current => __2__current;

		// Token: 0x060001D5 RID: 469 RVA: 0x00250B50 File Offset: 0x0024F150
		public _Start_d__10(int __1__state)
		{
			this.__1__state = __1__state;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00250B40 File Offset: 0x0024F140
		void System.IDisposable.Dispose()
		{
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00261DB0 File Offset: 0x002603B0
		bool System.Collections.IEnumerator.MoveNext()
		{
			Benchmark01 benchmark = __4__this;
			int num = __1__state;
			int result = default(int);
			if (__1__state != 0)
			{
				num--;
				if (num != 0)
				{
					result = 0;
					if (num == 1)
					{
						__1__state = -1;
						result = 0;
					}
					goto IL_00cf;
				}
				int num2 = _i_5__2;
				__1__state = -1;
				_i_5__2 = (int)(num2 + 1L);
				if ((int)(num2 + 1L) > 1000000)
				{
					__2__current = null;
					__1__state = 2;
					goto IL_00cb;
				}
			}
			else
			{
				__1__state = -1;
				if (benchmark.BenchmarkType == 0)
				{
					benchmark.m_textMeshPro = benchmark.gameObject.AddComponent<TextMeshPro>();
					benchmark.m_textMeshPro.autoSizeTextContainer = true;
					if (benchmark.TMProFont != null)
					{
						benchmark.m_textMeshPro.font = benchmark.TMProFont;
					}
					benchmark.m_textMeshPro.fontSize = 48f;
					benchmark.m_textMeshPro.alignment = TextAlignmentOptions.Center;
					benchmark.m_textMeshPro.extraPadding = true;
					benchmark.m_textMeshPro.enableWordWrapping = false;
					benchmark.m_material01 = ((TMP_Text)benchmark.m_textMeshPro).m_fontAsset.material;
					benchmark.m_material02 = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - Drop Shadow");
				}
				else if (benchmark.BenchmarkType == 1)
				{
					benchmark.m_textMesh = benchmark.gameObject.AddComponent<TextMesh>();
					bool flag = benchmark.TextMeshFont != null;
					TextMesh textMesh = benchmark.m_textMesh;
					Font textMeshFont = default(Font);
					if (flag)
					{
						textMeshFont = benchmark.TextMeshFont;
					}
					else if ((object)Resources.Load("Fonts/ARIAL", Type.GetTypeFromHandle(NativeIntrinsics.Reinterpret<RuntimeTypeHandle>((nint)NativeIntrinsics.Asm("typeof(Font)")))) != null && NativeIntrinsics.Asm("LocalExpr->klass") != NativeIntrinsics.Asm("Font_TypeInfo"))
					{
					}
					textMesh.font = textMeshFont;
					benchmark.m_textMesh.GetComponent<Renderer>().SetMaterial(benchmark.m_textMesh.font.material);
					benchmark.m_textMesh.fontSize = 48;
					benchmark.m_textMesh.anchor = TextAnchor.MiddleCenter;
				}
				_i_5__2 = 0;
			}
			if (benchmark.BenchmarkType == 0)
			{
				benchmark.m_textMeshPro.SetText("The <#0050FF>count is: </color>{0}", (int)NativeIntrinsics.Asm("imul ecx") - (((int)NativeIntrinsics.Asm("rdx") >> 6) + ((int)NativeIntrinsics.Asm("rdx") >> 6 >>> 31)) * 1000);
				if ((int)NativeIntrinsics.Asm("imul ecx") - (((int)NativeIntrinsics.Asm("rdx") >> 6) + ((int)NativeIntrinsics.Asm("rdx") >> 6 >>> 31)) * 1000 == 999)
				{
					TextMeshPro textMeshPro = benchmark.m_textMeshPro;
					Material fontSharedMaterial = textMeshPro.fontSharedMaterial;
					Material material = benchmark.m_material01;
					Material material2 = fontSharedMaterial;
					TextMeshPro textMeshPro2 = textMeshPro;
					material2 = ((!(material2 == material)) ? benchmark.m_material01 : benchmark.m_material02);
					benchmark.m_textMeshPro.fontSharedMaterial = material2;
					textMeshPro2.fontSharedMaterial = material2;
				}
			}
			else if (benchmark.BenchmarkType == 1)
			{
				TextMesh textMesh2 = benchmark.m_textMesh;
				int num3 = default(int);
				num3 = (int)NativeIntrinsics.Asm("imul ecx") - (((int)NativeIntrinsics.Asm("rdx") >> 6) + ((int)NativeIntrinsics.Asm("rdx") >> 6 >>> 31)) * 1000;
				textMesh2.text = "The <color=#0050FF>count is: </color>" + num3;
			}
			__2__current = null;
			__1__state = 1;
			goto IL_00cb;
			IL_00cb:
			result = 1;
			goto IL_00cf;
			IL_00cf:
			return (byte)result != 0;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00262800 File Offset: 0x00260E00
		void System.Collections.IEnumerator.Reset()
		{
			throw new NotSupportedException();
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
	private const string label01 = "The <#0050FF>count is: </color>{0}";

	// Token: 0x04000299 RID: 665 Offset: 0x0
	private const string label02 = "The <color=#0050FF>count is: </color>";

	// Token: 0x0400029A RID: 666 Offset: 0x50
	private Material m_material01;

	// Token: 0x0400029B RID: 667 Offset: 0x58
	private Material m_material02;

	// Token: 0x060001D3 RID: 467 RVA: 0x00252940 File Offset: 0x00250F40
	private IEnumerator Start()
	{
		_Start_d__10 _Start_d__ = new _Start_d__10(0);
		_Start_d__.__1__state = 0;
		_Start_d__.__4__this = this;
		return _Start_d__;
	}
}

