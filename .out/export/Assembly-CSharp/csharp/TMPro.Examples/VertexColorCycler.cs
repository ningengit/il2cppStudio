using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using Mono.Xml;
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
			((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
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
				if (num - 1 != 0 && num != 1)
				{
					goto IL_0158;
				}
				<>1__state = -1;
			}
			else
			{
				<>1__state = -1;
				TMP_Text textComponent = vertexColorCycler.m_TextComponent;
				TMP_TextInfo textInfo = textComponent.m_textInfo;
				<textInfo>5__2 = textInfo;
				<currentCharacter>5__3 = 0;
				textComponent = vertexColorCycler.m_TextComponent;
				Color color = default(Color);
				color.r = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(LocalExpr->klass + 0x0)"));
				Color32 color2 = color;
			}
			TMP_TextInfo tMP_TextInfo = <textInfo>5__2;
			int characterCount = tMP_TextInfo.characterCount;
			if (tMP_TextInfo.characterCount != 0)
			{
				TMP_MeshInfo[] meshInfo = tMP_TextInfo.meshInfo;
				long num2 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x58)");
				TMP_TextInfo tMP_TextInfo2 = tMP_TextInfo;
				if (NativeIntrinsics.Asm("*(BinaryExpr + 0x194)") != 0)
				{
					int num3 = UnityEngine.Random.Range(0, 255);
					int num4 = UnityEngine.Random.Range(0, 255);
					int num5 = UnityEngine.Random.Range(0, 255);
					NativeIntrinsics.Stmt("mov [rbx+rbp*4+20h],eax ; value = LocalExpr");
					NativeIntrinsics.Stmt("mov [rbx+rcx*4+20h],eax ; value = LocalExpr");
					NativeIntrinsics.Stmt("mov [rbx+rcx*4+20h],eax ; value = LocalExpr");
					NativeIntrinsics.Stmt("mov [rbx+rcx*4+20h],eax ; value = LocalExpr");
				}
				<currentCharacter>5__3 = (int)NativeIntrinsics.Asm("rdx");
				WaitForSeconds waitForSeconds = new WaitForSeconds(0.05f);
				<>2__current = waitForSeconds;
				<>1__state = 2;
			}
			else
			{
				WaitForSeconds waitForSeconds2 = new WaitForSeconds(0.25f);
				<>2__current = waitForSeconds2;
				<>1__state = 1;
			}
			goto IL_0158;
			IL_0158:
			return true;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00268080 File Offset: 0x00266680
		private void System.Collections.IEnumerator.Reset()
		{
			long num = NativeIntrinsics.CallNative("sub_184BE0", NativeIntrinsics.Asm("rax"), 0L, 0L, 0L);
			NativeIntrinsics.Reinterpret<NotSupportedException>(num)..ctor();
			throw null;
		}
	}

	// Token: 0x04000380 RID: 896 Offset: 0x20
	private TMP_Text m_TextComponent;

	// Token: 0x06000264 RID: 612 RVA: 0x00268CA0 File Offset: 0x002672A0
	private void Awake()
	{
		TMP_Text component = GetComponent<TMP_Text>();
		m_TextComponent = component;
	}

	// Token: 0x06000265 RID: 613 RVA: 0x00268CF0 File Offset: 0x002672F0
	private void Start()
	{
		<AnimateVertexColors>d__3 <AnimateVertexColors>d__ = NativeIntrinsics.Reinterpret<<AnimateVertexColors>d__3>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.VertexColorCycler+<AnimateVertexColors>d__3)"));
		((Mono.Xml.SecurityParser)(object)<AnimateVertexColors>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<AnimateVertexColors>d__.<>1__state = 0;
		<AnimateVertexColors>d__.<>4__this = this;
		Coroutine coroutine = StartCoroutine(<AnimateVertexColors>d__);
	}

	// Token: 0x06000266 RID: 614 RVA: 0x00268C30 File Offset: 0x00267230
	private IEnumerator AnimateVertexColors()
	{
		<AnimateVertexColors>d__3 <AnimateVertexColors>d__ = NativeIntrinsics.Reinterpret<<AnimateVertexColors>d__3>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.VertexColorCycler+<AnimateVertexColors>d__3)"));
		((Mono.Xml.SecurityParser)(object)<AnimateVertexColors>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<AnimateVertexColors>d__.<>1__state = 0;
		<AnimateVertexColors>d__.<>4__this = this;
		return <AnimateVertexColors>d__;
	}
}

