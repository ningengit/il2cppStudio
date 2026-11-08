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
	private sealed class <AnimateVertexColors>d__3 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x04000381 RID: 897 Offset: 0x10
		private int <>1__state;

		// Token: 0x04000382 RID: 898 Offset: 0x18
		private object <>2__current;

		// Token: 0x04000383 RID: 899 Offset: 0x20
		public VertexColorCycler <>4__this;

		// Token: 0x04000384 RID: 900 Offset: 0x28
		private TMP_TextInfo <textInfo>5__2;

		// Token: 0x04000385 RID: 901 Offset: 0x30
		private int <currentCharacter>5__3;

		private object System.Collections.Generic.IEnumerator<System.Object>.Current => <>2__current;

		private object System.Collections.IEnumerator.Current => <>2__current;

		// Token: 0x06000268 RID: 616 RVA: 0x00250B50 File Offset: 0x0024F150
		public <AnimateVertexColors>d__3(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00250B40 File Offset: 0x0024F140
		private void System.IDisposable.Dispose()
		{
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00267DB0 File Offset: 0x002663B0
		private bool MoveNext()
		{
			VertexColorCycler vertexColorCycler = <>4__this;
			int num = <>1__state;
			if (<>1__state != 0)
			{
				num--;
				int result = default(int);
				if (num != 0 && num != 1)
				{
					return (byte)result != 0;
				}
				<>1__state = -1;
			}
			else
			{
				<>1__state = -1;
				<textInfo>5__2 = vertexColorCycler.m_TextComponent.m_textInfo;
				<currentCharacter>5__3 = 0;
				_ = (Color32)vertexColorCycler.m_TextComponent.color;
			}
			TMP_TextInfo tMP_TextInfo = <textInfo>5__2;
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
				<currentCharacter>5__3 = (<currentCharacter>5__3 + 1) % characterCount;
				<>2__current = new WaitForSeconds(0.05f);
				<>1__state = 2;
			}
			else
			{
				<>2__current = new WaitForSeconds(0.25f);
				<>1__state = 1;
			}
			return true;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00268080 File Offset: 0x00266680
		private void System.Collections.IEnumerator.Reset()
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
		<AnimateVertexColors>d__3 <AnimateVertexColors>d__ = new <AnimateVertexColors>d__3(0);
		<AnimateVertexColors>d__.<>1__state = 0;
		<AnimateVertexColors>d__.<>4__this = this;
		StartCoroutine(<AnimateVertexColors>d__);
	}

	// Token: 0x06000266 RID: 614 RVA: 0x00268C30 File Offset: 0x00267230
	private IEnumerator AnimateVertexColors()
	{
		<AnimateVertexColors>d__3 <AnimateVertexColors>d__ = new <AnimateVertexColors>d__3(0);
		<AnimateVertexColors>d__.<>1__state = 0;
		<AnimateVertexColors>d__.<>4__this = this;
		return <AnimateVertexColors>d__;
	}
}

