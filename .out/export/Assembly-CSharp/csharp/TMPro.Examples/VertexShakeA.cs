using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using Mono.Xml;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x0200008E RID: 142
public class VertexShakeA : MonoBehaviour
{
	// Token: 0x0200008F RID: 143
	private sealed class <AnimateVertexColors>d__11 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x0400039B RID: 923 Offset: 0x10
		private int <>1__state;

		// Token: 0x0400039C RID: 924 Offset: 0x18
		private object <>2__current;

		// Token: 0x0400039D RID: 925 Offset: 0x20
		public VertexShakeA <>4__this;

		// Token: 0x0400039E RID: 926 Offset: 0x28
		private TMP_TextInfo <textInfo>5__2;

		// Token: 0x0400039F RID: 927 Offset: 0x30
		private Vector3[][] <copyOfVertices>5__3;

		private object System.Collections.Generic.IEnumerator<System.Object>.Current => <>2__current;

		private object System.Collections.IEnumerator.Current => <>2__current;

		// Token: 0x06000282 RID: 642 RVA: 0x00250B50 File Offset: 0x0024F150
		public <AnimateVertexColors>d__11(int <>1__state)
		{
			((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
			this.<>1__state = <>1__state;
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00250B40 File Offset: 0x0024F140
		private void System.IDisposable.Dispose()
		{
		}

		// Token: 0x06000284 RID: 644 RVA: 0x002663A0 File Offset: 0x002649A0
		private unsafe bool MoveNext()
		{
			int num = <>1__state;
			VertexShakeA vertexShakeA = <>4__this;
			VertexShakeA vertexShakeA2 = vertexShakeA;
			int num2 = num;
			if (num != 0)
			{
				num2--;
				if (num2 - 1 != 0 && num2 != 1)
				{
					goto IL_0a82;
				}
				<>1__state = -1;
			}
			else
			{
				<>1__state = -1;
				TMP_Text textComponent = vertexShakeA2.m_TextComponent;
				TMP_TextInfo textInfo = textComponent.m_textInfo;
				<textInfo>5__2 = textInfo;
				Vector3[][] array = new Vector3[0][];
				<copyOfVertices>5__3 = array;
				vertexShakeA2.hasTextChanged = true;
			}
			TMP_TextInfo tMP_TextInfo3;
			if (vertexShakeA2.hasTextChanged)
			{
				if (<copyOfVertices>5__3.Length < <textInfo>5__2.meshInfo.Length)
				{
					Vector3[][] array2 = new Vector3[<textInfo>5__2.meshInfo.Length][];
					<copyOfVertices>5__3 = array2;
				}
				TMP_TextInfo tMP_TextInfo = <textInfo>5__2;
				while (0 < tMP_TextInfo.meshInfo.Length)
				{
					TMP_TextInfo tMP_TextInfo2 = <textInfo>5__2;
					TMP_MeshInfo[] meshInfo = tMP_TextInfo2.meshInfo;
					long num3 = 0 + 0 * 4L + (0 + 0 * 4L);
					long num4 = NativeIntrinsics.Asm("*(LocalExpr + 0x30)");
					long num5 = 0L;
					Vector3[] array3 = new Vector3[(int)NativeIntrinsics.Asm("*(LocalExpr + 0x18)")];
					Vector3[] array4 = array3;
					Vector3[][] array5 = <copyOfVertices>5__3;
					array5[0] = array4;
					int num6 = 0 + 1;
					tMP_TextInfo3 = <textInfo>5__2;
					int num7 = 0 + 1;
					if (<textInfo>5__2 == null)
					{
						break;
					}
				}
				vertexShakeA2.hasTextChanged = false;
			}
			tMP_TextInfo3 = <textInfo>5__2;
			if (tMP_TextInfo3.characterCount != 0)
			{
				int lineCount = tMP_TextInfo3.lineCount;
				if (lineCount > 0)
				{
					Vector3 euler = default(Vector3);
					Vector3 vector = default(Vector3);
					Vector3 s = default(Vector3);
					Vector3 point = default(Vector3);
					Vector3 point2 = default(Vector3);
					do
					{
						TMP_CharacterInfo[] characterInfo = <textInfo>5__2.characterInfo;
						long num8 = (int)NativeIntrinsics.Asm("*(FieldExpr + 0x34)");
						long num9 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x3C)");
						float num10 = UnityEngine.Random.Range(-0.25f, 0.25f);
						euler.x = 0f;
						euler.z = num10 * vertexShakeA2.RotationMultiplier * (MathF.PI / 180f);
						Quaternion quaternion = Quaternion.Internal_FromEulerRad(euler);
						float num11 = ((float)(int)NativeIntrinsics.Asm("*(FieldExpr + 0x128)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x11C)")) * 0.5f;
						float num12 = ((float)(int)NativeIntrinsics.Asm("*(FieldExpr + 0x12C)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x120)")) * 0.5f;
						float num13 = ((float)(int)NativeIntrinsics.Asm("*(FieldExpr + 0x130)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x124)")) * 0.5f;
						long num14 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
						if ((int)num8 <= (int)num9)
						{
							do
							{
								if (NativeIntrinsics.Asm("*(FieldExpr + 0x194)") != 0)
								{
									TMP_MeshInfo[] meshInfo2 = <textInfo>5__2.meshInfo;
									long num3 = (int)num8 * 376L;
									long num15 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x58)");
									Vector3[][] array6 = <copyOfVertices>5__3;
									num3 = num15 + num15 * 4L + (num15 + num15 * 4L);
									long num16 = NativeIntrinsics.Asm("*(LocalExpr + 0x30)");
									num3 = (int)num8 * 376L;
									long num17 = (int)NativeIntrinsics.Asm("*(FieldExpr + 0x6C)");
									num3 = num17 + num17 * 2L;
									float num18 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num13;
									Vector3[] array7 = array6[(int)num15];
									float num19 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - num11;
									float num20 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num12;
									NativeIntrinsics.Stmt("movss dword ptr [r8+rcx*4+20h],xmm0 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [r8+rcx*4+24h],xmm1 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [r8+rcx*4+28h],xmm2 ; value = LocalExpr");
									num3 = num17 + num17 * 2L;
									Vector3[][] array8 = <copyOfVertices>5__3;
									num3 = num17 + 1L + (num17 + 1L) * 2L;
									num18 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num13;
									Vector3[] array9 = array8[(int)num15];
									num19 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - num11;
									num20 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num12;
									NativeIntrinsics.Stmt("movss dword ptr [r8+rax*4+20h],xmm0 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [r8+rax*4+24h],xmm1 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [r8+rax*4+28h],xmm2 ; value = LocalExpr");
									Vector3[][] array10 = <copyOfVertices>5__3;
									num3 = num17 + 2L + (num17 + 2L) * 2L;
									num18 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num13;
									Vector3[] array11 = array10[(int)num15];
									num19 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - num11;
									num20 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num12;
									NativeIntrinsics.Stmt("movss dword ptr [r8+rax*4+20h],xmm0 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [r8+rax*4+24h],xmm1 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [r8+rax*4+28h],xmm2 ; value = LocalExpr");
									Vector3[][] array12 = <copyOfVertices>5__3;
									num3 = num17 + 3L + (num17 + 3L) * 2L;
									num18 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num13;
									Vector3[] array13 = array12[(int)num15];
									num19 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - num11;
									num20 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num12;
									NativeIntrinsics.Stmt("movss dword ptr [r8+rax*4+20h],xmm0 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [r8+rax*4+24h],xmm1 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [r8+rax*4+28h],xmm2 ; value = LocalExpr");
									float num21 = UnityEngine.Random.Range(0.995f - vertexShakeA2.ScaleMultiplier * 0.001f, vertexShakeA2.ScaleMultiplier * 0.001f + 1.005f);
									int num22 = (int)NativeIntrinsics.Asm("global_19E3BFF");
									float num23 = num21;
									num19 = num21;
									if ((int)NativeIntrinsics.Asm("global_19E3BFF") == 0)
									{
										NativeIntrinsics.Stmt("mov [1819E3BFFh],cl ; value = 0x1");
									}
									float z = Vector3.oneVector.z;
									long num24 = NativeIntrinsics.Asm("struct_chunk(StaticFieldExpr+0x0, 8)");
									if (1 == 0)
									{
										NativeIntrinsics.Stmt("mov byte ptr [1819E3BFFh],1 ; value = 0x1");
									}
									float num25 = num23 * Vector3.oneVector.x;
									vector.x = NativeIntrinsics.Reinterpret<float>(num24);
									vector.z = z;
									Vector3 pos = vector;
									Quaternion q = NativeIntrinsics.Reinterpret<Quaternion>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFEA8]") + -32L);
									s.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("unpcklps xmm1,xmm0"));
									s.z = num23 * Vector3.oneVector.z;
									Matrix4x4 matrix4x = Matrix4x4.TRS(pos, q, s);
									Vector3[][] array14 = <copyOfVertices>5__3;
									long num26 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x20, 16)");
									long num27 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x30, 16)");
									Vector3[] array15 = array14[(int)num15];
									long num28 = NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFEA8]") + -96L;
									point.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(ArrayElementExpr + 0x20)"));
									point.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(ArrayElementExpr + 0x28)"));
									Vector3 vector2 = ((Matrix4x4*)num28)->MultiplyPoint3x4(point);
									NativeIntrinsics.Stmt("movsd qword ptr [r14+rdx*4+20h],xmm0 ; value = StructChunkExpr");
									NativeIntrinsics.Stmt("mov [r14+rdx*4+28h],r8d ; value = FieldExpr");
									Vector3[][] array16 = <copyOfVertices>5__3;
									num26 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
									Vector3[] array17 = array16[(int)num15];
									long num29 = NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFEA8]") + -96L;
									point2.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(ArrayElementExpr + 0x20)"));
									point2.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(ArrayElementExpr + 0x28)"));
									Vector3 vector3 = ((Matrix4x4*)num29)->MultiplyPoint3x4(point2);
									NativeIntrinsics.Stmt("movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr");
									NativeIntrinsics.Stmt("mov [r14+rcx*4+28h],edx ; value = FieldExpr");
									array16 = <copyOfVertices>5__3;
									num26 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
									Vector3[] array18 = array16[(int)num15];
									Vector3 vector4 = ((Matrix4x4*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFEA8]") + -96L))->MultiplyPoint3x4(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFEA8]") + -128L));
									NativeIntrinsics.Stmt("movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr");
									NativeIntrinsics.Stmt("mov [r14+rcx*4+28h],edx ; value = FieldExpr");
									array16 = <copyOfVertices>5__3;
									num26 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
									Vector3[] array19 = array16[(int)num15];
									Vector3 vector5 = ((Matrix4x4*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFEA8]") + -96L))->MultiplyPoint3x4(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFEA8]") + -112L));
									NativeIntrinsics.Stmt("movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr");
									NativeIntrinsics.Stmt("mov [r14+rcx*4+28h],edx ; value = FieldExpr");
									num26 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
									Vector3[] array20 = <copyOfVertices>5__3[(int)num15];
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+24h],xmm0 ; value = BinaryExpr");
									float num30 = num12 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
									float num31 = num13 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
									Vector3[] array21 = <copyOfVertices>5__3[(int)num15];
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr");
									num30 = num12 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
									num31 = num13 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
									Vector3[] array22 = <copyOfVertices>5__3[(int)num15];
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr");
									num30 = num12 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
									num31 = num13 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
									Vector3[] array23 = <copyOfVertices>5__3[(int)num15];
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr");
									num30 = num12 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
									num31 = num13 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
								}
								int num32 = (int)num8 + 1;
							}
							while ((int)num8 + 1 <= (int)num9);
						}
						int num33 = (int)NativeIntrinsics.Asm("stack[0x8]") + 1;
					}
					while ((int)NativeIntrinsics.Asm("stack[0x8]") + 1 < (int)NativeIntrinsics.Asm("stack[0x18]"));
				}
				tMP_TextInfo3 = <textInfo>5__2;
				while (0 < tMP_TextInfo3.meshInfo.Length)
				{
					TMP_TextInfo tMP_TextInfo4 = <textInfo>5__2;
					TMP_MeshInfo[] meshInfo3 = tMP_TextInfo4.meshInfo;
					Vector3[][] array24 = <copyOfVertices>5__3;
					long num34 = NativeIntrinsics.Asm("*(LocalExpr + 0x20)");
					long num35 = 0L;
					NativeIntrinsics.Reinterpret<Mesh>(num34).vertices = array24[0];
					TMP_Text textComponent2 = vertexShakeA2.m_TextComponent;
					int num36 = 0 + 1;
					TMP_TextInfo tMP_TextInfo5 = <textInfo>5__2;
					int num37 = 0 + 1;
					if (<textInfo>5__2 == null)
					{
						break;
					}
				}
				WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
				<>2__current = waitForSeconds;
				<>1__state = 2;
			}
			else
			{
				WaitForSeconds waitForSeconds2 = new WaitForSeconds(0.25f);
				<>2__current = waitForSeconds2;
				<>1__state = 1;
			}
			goto IL_0a82;
			IL_0a82:
			return true;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00267D70 File Offset: 0x00266370
		private void System.Collections.IEnumerator.Reset()
		{
			long num = NativeIntrinsics.CallNative("sub_184BE0", NativeIntrinsics.Asm("rax"), 0L, 0L, 0L);
			NativeIntrinsics.Reinterpret<NotSupportedException>(num)..ctor();
			throw null;
		}
	}

	// Token: 0x04000395 RID: 917 Offset: 0x20
	public float AngleMultiplier = 1f;

	// Token: 0x04000396 RID: 918 Offset: 0x24
	public float SpeedMultiplier = 1f;

	// Token: 0x04000397 RID: 919 Offset: 0x28
	public float ScaleMultiplier = 1f;

	// Token: 0x04000398 RID: 920 Offset: 0x2C
	public float RotationMultiplier = 1f;

	// Token: 0x04000399 RID: 921 Offset: 0x30
	private TMP_Text m_TextComponent;

	// Token: 0x0400039A RID: 922 Offset: 0x38
	private bool hasTextChanged;

	// Token: 0x0600027B RID: 635 RVA: 0x00269150 File Offset: 0x00267750
	private void Awake()
	{
		TMP_Text component = GetComponent<TMP_Text>();
		m_TextComponent = component;
	}

	// Token: 0x0600027C RID: 636 RVA: 0x002692D0 File Offset: 0x002678D0
	private void OnEnable()
	{
		FastAction<UnityEngine.Object> tEXT_CHANGED_EVENT = TMPro_EventManager.TEXT_CHANGED_EVENT;
		Action<UnityEngine.Object> rhs = (Action<UnityEngine.Object>)(object)new System.IOAsyncCallback(ON_TEXT_CHANGED);
		tEXT_CHANGED_EVENT.Add(rhs);
	}

	// Token: 0x0600027D RID: 637 RVA: 0x00269200 File Offset: 0x00267800
	private void OnDisable()
	{
		FastAction<UnityEngine.Object> tEXT_CHANGED_EVENT = TMPro_EventManager.TEXT_CHANGED_EVENT;
		Action<UnityEngine.Object> rhs = (Action<UnityEngine.Object>)(object)new System.IOAsyncCallback(ON_TEXT_CHANGED);
		tEXT_CHANGED_EVENT.Remove(rhs);
	}

	// Token: 0x0600027E RID: 638 RVA: 0x002693A0 File Offset: 0x002679A0
	private void Start()
	{
		<AnimateVertexColors>d__11 <AnimateVertexColors>d__ = NativeIntrinsics.Reinterpret<<AnimateVertexColors>d__11>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.VertexShakeA+<AnimateVertexColors>d__11)"));
		((Mono.Xml.SecurityParser)(object)<AnimateVertexColors>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<AnimateVertexColors>d__.<>1__state = 0;
		<AnimateVertexColors>d__.<>4__this = this;
		Coroutine coroutine = StartCoroutine(<AnimateVertexColors>d__);
	}

	// Token: 0x0600027F RID: 639 RVA: 0x002691A0 File Offset: 0x002677A0
	private void ON_TEXT_CHANGED(UnityEngine.Object obj)
	{
		TMP_Text textComponent = m_TextComponent;
		bool flag = textComponent;
		bool flag2 = flag;
		if (flag)
		{
			hasTextChanged = true;
		}
	}

	// Token: 0x06000280 RID: 640 RVA: 0x002690E0 File Offset: 0x002676E0
	private IEnumerator AnimateVertexColors()
	{
		<AnimateVertexColors>d__11 <AnimateVertexColors>d__ = NativeIntrinsics.Reinterpret<<AnimateVertexColors>d__11>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.VertexShakeA+<AnimateVertexColors>d__11)"));
		((Mono.Xml.SecurityParser)(object)<AnimateVertexColors>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<AnimateVertexColors>d__.<>1__state = 0;
		<AnimateVertexColors>d__.<>4__this = this;
		return <AnimateVertexColors>d__;
	}
}

