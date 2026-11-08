using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x02000089 RID: 137
public class VertexColorCycler : MonoBehaviour
{
	// Token: 0x0200008A RID: 138
	private sealed class _AnimateVertexColors_d__3 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x04000381 RID: 897 Offset: 0x10
		// Il2CppStudio: declared private; widened because native code inlined into other types accesses it.
		internal int __1__state;

		// Token: 0x04000382 RID: 898 Offset: 0x18
		private object __2__current;

		// Token: 0x04000383 RID: 899 Offset: 0x20
		public VertexColorCycler __4__this;

		// Token: 0x04000384 RID: 900 Offset: 0x28
		private TMP_TextInfo _textInfo_5__2;

		// Token: 0x04000385 RID: 901 Offset: 0x30
		private int _currentCharacter_5__3;

		object System.Collections.Generic.IEnumerator<System.Object>.Current => __2__current;

		object System.Collections.IEnumerator.Current => __2__current;

		// Token: 0x06000268 RID: 616 RVA: 0x00250B50 File Offset: 0x0024F150
		public _AnimateVertexColors_d__3(int __1__state)
		{
			this.__1__state = __1__state;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00250B40 File Offset: 0x0024F140
		void System.IDisposable.Dispose()
		{
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00267DB0 File Offset: 0x002663B0
		bool System.Collections.IEnumerator.MoveNext()
		{
			VertexColorCycler vertexColorCycler = __4__this;
			int num = __1__state;
			if (__1__state != 0)
			{
				num--;
				int result = default(int);
				if (num != 0 && num != 1)
				{
					return (byte)result != 0;
				}
				__1__state = -1;
			}
			else
			{
				__1__state = -1;
				_textInfo_5__2 = vertexColorCycler.m_TextComponent.textInfo;
				_currentCharacter_5__3 = 0;
				_ = (Color32)vertexColorCycler.m_TextComponent.color;
			}
			TMP_TextInfo tMP_TextInfo = _textInfo_5__2;
			int characterCount = tMP_TextInfo.characterCount;
			if (tMP_TextInfo.characterCount != 0)
			{
				_ = tMP_TextInfo.meshInfo;
				_ = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x58)");
				if (NativeIntrinsics.Asm("*(BinaryExpr + 0x194)") != 0)
				{
					UnityEngine.Random.Range(0, 255);
					UnityEngine.Random.Range(0, 255);
					UnityEngine.Random.Range(0, 255);
					NativeIntrinsics.Stmt("mov [rbx+rbp*4+20h],eax ; value = LocalExpr");
					NativeIntrinsics.Stmt("mov [rbx+rcx*4+20h],eax ; value = LocalExpr");
					NativeIntrinsics.Stmt("mov [rbx+rcx*4+20h],eax ; value = LocalExpr");
					NativeIntrinsics.Stmt("mov [rbx+rcx*4+20h],eax ; value = LocalExpr");
					vertexColorCycler.m_TextComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
				}
				_currentCharacter_5__3 = (_currentCharacter_5__3 + 1) % characterCount;
				__2__current = new WaitForSeconds(0.05f);
				__1__state = 2;
			}
			else
			{
				__2__current = new WaitForSeconds(0.25f);
				__1__state = 1;
			}
			return true;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00268080 File Offset: 0x00266680
		void System.Collections.IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	// Token: 0x04000380 RID: 896 Offset: 0x20
	private TMP_Text m_TextComponent;

	// Token: 0x06000264 RID: 612 RVA: 0x00268CA0 File Offset: 0x002672A0
	private void Awake()
	{
		m_TextComponent = GetComponent<TMP_Text>();
	}

	// Token: 0x06000265 RID: 613 RVA: 0x00268CF0 File Offset: 0x002672F0
	private void Start()
	{
		_AnimateVertexColors_d__3 _AnimateVertexColors_d__ = new _AnimateVertexColors_d__3(0);
		_AnimateVertexColors_d__.__1__state = 0;
		_AnimateVertexColors_d__.__4__this = this;
		StartCoroutine(_AnimateVertexColors_d__);
	}

	// Token: 0x06000266 RID: 614 RVA: 0x00268C30 File Offset: 0x00267230
	private IEnumerator AnimateVertexColors()
	{
		_AnimateVertexColors_d__3 _AnimateVertexColors_d__ = new _AnimateVertexColors_d__3(0);
		_AnimateVertexColors_d__.__1__state = 0;
		_AnimateVertexColors_d__.__4__this = this;
		return _AnimateVertexColors_d__;
	}
}

