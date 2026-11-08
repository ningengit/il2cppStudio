using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using Mono.Xml;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x02000090 RID: 144
public class VertexShakeB : MonoBehaviour
{
	// Token: 0x02000091 RID: 145
	private sealed class <AnimateVertexColors>d__10 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x040003A5 RID: 933 Offset: 0x10
		private int <>1__state;

		// Token: 0x040003A6 RID: 934 Offset: 0x18
		private object <>2__current;

		// Token: 0x040003A7 RID: 935 Offset: 0x20
		public VertexShakeB <>4__this;

		// Token: 0x040003A8 RID: 936 Offset: 0x28
		private TMP_TextInfo <textInfo>5__2;

		// Token: 0x040003A9 RID: 937 Offset: 0x30
		private Vector3[][] <copyOfVertices>5__3;

		private object System.Collections.Generic.IEnumerator<System.Object>.Current => <>2__current;

		private object System.Collections.IEnumerator.Current => <>2__current;

		// Token: 0x0600028F RID: 655 RVA: 0x00250B50 File Offset: 0x0024F150
		public <AnimateVertexColors>d__10(int <>1__state)
		{
			((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
			this.<>1__state = <>1__state;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00250B40 File Offset: 0x0024F140
		private void System.IDisposable.Dispose()
		{
		}

		// Token: 0x06000291 RID: 657 RVA: 0x00264F00 File Offset: 0x00263500
		private bool MoveNext()
		{
			int num = <>1__state;
			VertexShakeB vertexShakeB = <>4__this;
			VertexShakeB vertexShakeB2 = vertexShakeB;
			int num2 = num;
			if (num != 0)
			{
				num2--;
				if (num2 - 1 != 0 && num2 != 1)
				{
					goto IL_1666;
				}
				<>1__state = -1;
			}
			else
			{
				<>1__state = -1;
				TMP_Text textComponent = vertexShakeB2.m_TextComponent;
				TMP_TextInfo textInfo = textComponent.m_textInfo;
				<textInfo>5__2 = textInfo;
				Vector3[][] array = new Vector3[0][];
				<copyOfVertices>5__3 = array;
				vertexShakeB2.hasTextChanged = true;
			}
			TMP_TextInfo tMP_TextInfo3;
			if (vertexShakeB2.hasTextChanged)
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
				NativeIntrinsics.Stmt("mov byte ptr [rdi+38h],0 ; value = 0x0");
				long num8 = NativeIntrinsics.Asm("stack[0x18]");
			}
			tMP_TextInfo3 = <textInfo>5__2;
			if (tMP_TextInfo3.characterCount != 0)
			{
				int lineCount = tMP_TextInfo3.lineCount;
				if (lineCount > 0)
				{
					Vector3 euler = default(Vector3);
					Matrix4x4 matrix4x2 = default(Matrix4x4);
					Matrix4x4 matrix4x3 = default(Matrix4x4);
					Matrix4x4 matrix4x4 = default(Matrix4x4);
					Matrix4x4 matrix4x5 = default(Matrix4x4);
					Matrix4x4 matrix4x7 = default(Matrix4x4);
					Vector3 point = default(Vector3);
					Matrix4x4 matrix4x8 = default(Matrix4x4);
					Matrix4x4 matrix4x9 = default(Matrix4x4);
					Matrix4x4 matrix4x10 = default(Matrix4x4);
					long num8;
					do
					{
						TMP_CharacterInfo[] characterInfo = <textInfo>5__2.characterInfo;
						long num9 = (int)NativeIntrinsics.Asm("*(FieldExpr + 0x34)");
						TMP_CharacterInfo[] characterInfo2 = <textInfo>5__2.characterInfo;
						long num10 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x3C)");
						float num11 = UnityEngine.Random.Range(-0.25f, 0.25f);
						euler.x = 0f;
						euler.z = num11 * (MathF.PI / 180f);
						Quaternion quaternion = Quaternion.Internal_FromEulerRad(euler);
						long num12 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
						float num13 = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x128)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x11C)")) * 0.5f;
						float num14 = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x130)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x124)")) * 0.5f;
						float num15 = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x12C)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x120)")) * 0.5f;
						if ((int)num9 <= (int)num10)
						{
							do
							{
								if (NativeIntrinsics.Asm("*(FieldExpr + 0x194)") != 0)
								{
									TMP_MeshInfo[] meshInfo2 = <textInfo>5__2.meshInfo;
									TMP_TextInfo tMP_TextInfo4 = <textInfo>5__2;
									long num16 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x58)");
									TMP_TextInfo tMP_TextInfo5 = tMP_TextInfo4;
									long num17 = NativeIntrinsics.Asm("*(LocalExpr + 0x30)");
									num8 = (int)NativeIntrinsics.Asm("*(FieldExpr + 0x6C)");
									float num18 = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)")) * 0.5f;
									float num19 = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)")) * 0.5f;
									float num20 = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)")) * 0.5f;
									Vector3[][] array6 = <copyOfVertices>5__3;
									float num21 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num19;
									float num22 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - num18;
									float num23 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num20;
									Vector3[] array7 = array6[(int)num16];
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+20h],xmm0 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+24h],xmm1 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+28h],xmm2 ; value = LocalExpr");
									Vector3[][] array8 = <copyOfVertices>5__3;
									num21 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num19;
									num22 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - num18;
									num23 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num20;
									Vector3[] array9 = array8[(int)num16];
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*4+20h],xmm0 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*4+24h],xmm1 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*4+28h],xmm2 ; value = LocalExpr");
									Vector3[][] array10 = <copyOfVertices>5__3;
									num21 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num19;
									num22 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - num18;
									num23 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num20;
									Vector3[] array11 = array10[(int)num16];
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*4+20h],xmm0 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*4+24h],xmm1 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*4+28h],xmm2 ; value = LocalExpr");
									Vector3[][] array12 = <copyOfVertices>5__3;
									num21 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num19;
									num22 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - num18;
									num23 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num20;
									Vector3[] array13 = array12[(int)num16];
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*4+20h],xmm0 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*4+24h],xmm1 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*4+28h],xmm2 ; value = LocalExpr");
									float num24 = UnityEngine.Random.Range(0.95f, 1.05f);
									float num25 = num24;
									num22 = num24;
									int num26 = (int)NativeIntrinsics.Asm("global_19E3BFF");
									if ((int)NativeIntrinsics.Asm("global_19E3BFF") == 0)
									{
										NativeIntrinsics.Stmt("mov [1819E3BFFh],dl ; value = 0x1");
									}
									Quaternion identityQuaternion = Quaternion.identityQuaternion;
									float z = Vector3.oneVector.z;
									long num27 = NativeIntrinsics.Asm("struct_chunk(StaticFieldExpr+0x0, 8)");
									num26 = (int)NativeIntrinsics.Asm("global_19E3BFF");
									if ((int)NativeIntrinsics.Asm("global_19E3BFF") == 0)
									{
										NativeIntrinsics.Stmt("mov byte ptr [1819E3BFFh],1 ; value = 0x1");
									}
									float num28 = num25 * Vector3.oneVector.x;
									float num29 = num25 * Vector3.oneVector.y;
									float num30 = num25 * Vector3.oneVector.z;
									Matrix4x4 matrix4x = Matrix4x4.TRS(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFDF8]") + -112L), NativeIntrinsics.Reinterpret<Quaternion>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFDF8]") + 64L), NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFDF8]") + -128L));
									long num31 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x30, 16)");
									long num32 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x20, 16)");
									Vector3[] array14 = <copyOfVertices>5__3[(int)num16];
									Vector3[] array15 = <copyOfVertices>5__3[(int)num16];
									matrix4x2.m00 = matrix4x.m00;
									matrix4x2.m10 = matrix4x.m10;
									matrix4x2.m20 = matrix4x.m20;
									matrix4x2.m30 = matrix4x.m30;
									matrix4x2.m01 = matrix4x.m01;
									matrix4x2.m11 = matrix4x.m11;
									matrix4x2.m21 = matrix4x.m21;
									matrix4x2.m31 = matrix4x.m31;
									matrix4x2.m02 = matrix4x.m02;
									matrix4x2.m12 = matrix4x.m12;
									matrix4x2.m22 = matrix4x.m22;
									matrix4x2.m32 = matrix4x.m32;
									matrix4x2.m03 = matrix4x.m03;
									matrix4x2.m13 = matrix4x.m13;
									matrix4x2.m23 = matrix4x.m23;
									matrix4x2.m33 = matrix4x.m33;
									Vector3 vector = matrix4x2.MultiplyPoint3x4(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFDF8]") + -96L));
									NativeIntrinsics.Stmt("movsd qword ptr [r14+rdx*4+20h],xmm0 ; value = StructChunkExpr");
									NativeIntrinsics.Stmt("mov [r14+rdx*4+28h],r8d ; value = FieldExpr");
									Vector3[][] array16 = <copyOfVertices>5__3;
									num32 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
									long num33 = num8 + num8 * 2L;
									Vector3[] array17 = array16[(int)num16];
									Vector3[] array18 = array16[(int)num16];
									matrix4x3.m00 = matrix4x.m00;
									matrix4x3.m10 = matrix4x.m10;
									matrix4x3.m20 = matrix4x.m20;
									matrix4x3.m30 = matrix4x.m30;
									matrix4x3.m01 = matrix4x.m01;
									matrix4x3.m11 = matrix4x.m11;
									matrix4x3.m21 = matrix4x.m21;
									matrix4x3.m31 = matrix4x.m31;
									matrix4x3.m02 = matrix4x.m02;
									matrix4x3.m12 = matrix4x.m12;
									matrix4x3.m22 = matrix4x.m22;
									matrix4x3.m32 = matrix4x.m32;
									matrix4x3.m03 = matrix4x.m03;
									matrix4x3.m13 = matrix4x.m13;
									matrix4x3.m23 = matrix4x.m23;
									matrix4x3.m33 = matrix4x.m33;
									Vector3 vector2 = matrix4x3.MultiplyPoint3x4(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFDF8]") + -80L));
									long num3 = num8 + 1L;
									NativeIntrinsics.Stmt("movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr");
									NativeIntrinsics.Stmt("mov [r14+rcx*4+28h],edx ; value = FieldExpr");
									num3 += num3 * 2L;
									array16 = <copyOfVertices>5__3;
									num32 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
									float z2 = vector2.z;
									Vector3[] array19 = array16[(int)num16];
									Vector3[] array20 = array16[(int)num16];
									matrix4x4.m00 = matrix4x.m00;
									matrix4x4.m10 = matrix4x.m10;
									matrix4x4.m20 = matrix4x.m20;
									matrix4x4.m30 = matrix4x.m30;
									matrix4x4.m01 = matrix4x.m01;
									matrix4x4.m11 = matrix4x.m11;
									matrix4x4.m21 = matrix4x.m21;
									matrix4x4.m31 = matrix4x.m31;
									matrix4x4.m02 = matrix4x.m02;
									matrix4x4.m12 = matrix4x.m12;
									matrix4x4.m22 = matrix4x.m22;
									matrix4x4.m32 = matrix4x.m32;
									matrix4x4.m03 = matrix4x.m03;
									matrix4x4.m13 = matrix4x.m13;
									matrix4x4.m23 = matrix4x.m23;
									matrix4x4.m33 = matrix4x.m33;
									Vector3 vector3 = matrix4x4.MultiplyPoint3x4(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFDF8]") + -64L));
									num3 = num8 + 2L;
									NativeIntrinsics.Stmt("movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr");
									NativeIntrinsics.Stmt("mov [r14+rcx*4+28h],edx ; value = FieldExpr");
									num3 += num3 * 2L;
									array16 = <copyOfVertices>5__3;
									num32 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
									float z3 = vector3.z;
									Vector3[] array21 = array16[(int)num16];
									Vector3[] array22 = array16[(int)num16];
									matrix4x5.m00 = matrix4x.m00;
									matrix4x5.m10 = matrix4x.m10;
									matrix4x5.m20 = matrix4x.m20;
									matrix4x5.m30 = matrix4x.m30;
									matrix4x5.m01 = matrix4x.m01;
									matrix4x5.m11 = matrix4x.m11;
									matrix4x5.m21 = matrix4x.m21;
									matrix4x5.m31 = matrix4x.m31;
									matrix4x5.m02 = matrix4x.m02;
									matrix4x5.m12 = matrix4x.m12;
									matrix4x5.m22 = matrix4x.m22;
									matrix4x5.m32 = matrix4x.m32;
									matrix4x5.m03 = matrix4x.m03;
									matrix4x5.m13 = matrix4x.m13;
									matrix4x5.m23 = matrix4x.m23;
									matrix4x5.m33 = matrix4x.m33;
									Vector3 vector4 = matrix4x5.MultiplyPoint3x4(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFDF8]") + -48L));
									num3 = num8 + 3L;
									NativeIntrinsics.Stmt("movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr");
									NativeIntrinsics.Stmt("mov [r14+rcx*4+28h],edx ; value = FieldExpr");
									num3 += num3 * 2L;
									num32 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
									Vector3[][] array23 = <copyOfVertices>5__3;
									Vector3[] array24 = array23[(int)num16];
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+24h],xmm0 ; value = BinaryExpr");
									Vector3[][] array25 = <copyOfVertices>5__3;
									float num34 = num19 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
									float num35 = num20 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
									Vector3[] array26 = array25[(int)num16];
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr");
									Vector3[][] array27 = <copyOfVertices>5__3;
									num34 = num19 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
									num35 = num20 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
									Vector3[] array28 = array27[(int)num16];
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr");
									Vector3[][] array29 = <copyOfVertices>5__3;
									num34 = num19 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
									num35 = num20 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
									Vector3[] array30 = array29[(int)num16];
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+20h],xmm6 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+24h],xmm7 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+28h],xmm8 ; value = BinaryExpr");
									Vector3[][] array31 = <copyOfVertices>5__3;
									Vector3[] array32 = array31[(int)num16];
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+24h],xmm0 ; value = BinaryExpr");
									Vector3[][] array33 = <copyOfVertices>5__3;
									num34 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num14;
									num35 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num15;
									Vector3[] array34 = array33[(int)num16];
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr");
									Vector3[][] array35 = <copyOfVertices>5__3;
									num34 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num14;
									num35 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num15;
									Vector3[] array36 = array35[(int)num16];
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr");
									Vector3[][] array37 = <copyOfVertices>5__3;
									num34 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num14;
									num35 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num15;
									Vector3[] array38 = array37[(int)num16];
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr");
									int num36 = (int)NativeIntrinsics.Asm("global_19E3BFF");
									num34 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num14;
									num35 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num15;
									if ((int)NativeIntrinsics.Asm("global_19E3BFF") == 0)
									{
										NativeIntrinsics.Stmt("mov [1819E3BFFh],cl ; value = 0x1");
									}
									long num37 = NativeIntrinsics.Asm("struct_chunk(StaticFieldExpr+0x0, 8)");
									z = Vector3.oneVector.z;
									if (1 == 0)
									{
										NativeIntrinsics.Stmt("mov byte ptr [1819E3BFFh],1 ; value = 0x1");
									}
									float z4 = Vector3.oneVector.z;
									Matrix4x4 matrix4x6 = Matrix4x4.TRS(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFDF8]") + -16L), NativeIntrinsics.Reinterpret<Quaternion>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFDF8]") + 64L), NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFDF8]") + -32L));
									long num38 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x20, 16)");
									long num39 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x30, 16)");
									Vector3[] array39 = <copyOfVertices>5__3[(int)num16];
									matrix4x7.m00 = matrix4x6.m00;
									matrix4x7.m10 = matrix4x6.m10;
									matrix4x7.m20 = matrix4x6.m20;
									matrix4x7.m30 = matrix4x6.m30;
									matrix4x7.m01 = matrix4x6.m01;
									matrix4x7.m11 = matrix4x6.m11;
									matrix4x7.m21 = matrix4x6.m21;
									matrix4x7.m31 = matrix4x6.m31;
									matrix4x7.m02 = matrix4x6.m02;
									matrix4x7.m12 = matrix4x6.m12;
									matrix4x7.m22 = matrix4x6.m22;
									matrix4x7.m32 = matrix4x6.m32;
									matrix4x7.m03 = matrix4x6.m03;
									matrix4x7.m13 = matrix4x6.m13;
									matrix4x7.m23 = matrix4x6.m23;
									matrix4x7.m33 = matrix4x6.m33;
									point.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(ArrayElementExpr + 0x20)"));
									point.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(ArrayElementExpr + 0x28)"));
									Vector3 vector5 = matrix4x7.MultiplyPoint3x4(point);
									NativeIntrinsics.Stmt("movsd qword ptr [r14+rdx*4+20h],xmm0 ; value = StructChunkExpr");
									NativeIntrinsics.Stmt("mov [r14+rdx*4+28h],r8d ; value = FieldExpr");
									array16 = <copyOfVertices>5__3;
									num38 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
									Vector3[] array40 = array16[(int)num16];
									matrix4x8.m00 = matrix4x6.m00;
									matrix4x8.m10 = matrix4x6.m10;
									matrix4x8.m20 = matrix4x6.m20;
									matrix4x8.m30 = matrix4x6.m30;
									matrix4x8.m01 = matrix4x6.m01;
									matrix4x8.m11 = matrix4x6.m11;
									matrix4x8.m21 = matrix4x6.m21;
									matrix4x8.m31 = matrix4x6.m31;
									matrix4x8.m02 = matrix4x6.m02;
									matrix4x8.m12 = matrix4x6.m12;
									matrix4x8.m22 = matrix4x6.m22;
									matrix4x8.m32 = matrix4x6.m32;
									matrix4x8.m03 = matrix4x6.m03;
									matrix4x8.m13 = matrix4x6.m13;
									matrix4x8.m23 = matrix4x6.m23;
									matrix4x8.m33 = matrix4x6.m33;
									Vector3 vector6 = matrix4x8.MultiplyPoint3x4(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFDF8]") + 16L));
									NativeIntrinsics.Stmt("movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr");
									NativeIntrinsics.Stmt("mov [r14+rcx*4+28h],edx ; value = FieldExpr");
									array16 = <copyOfVertices>5__3;
									num38 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
									Vector3[] array41 = array16[(int)num16];
									matrix4x9.m00 = matrix4x6.m00;
									matrix4x9.m10 = matrix4x6.m10;
									matrix4x9.m20 = matrix4x6.m20;
									matrix4x9.m30 = matrix4x6.m30;
									matrix4x9.m01 = matrix4x6.m01;
									matrix4x9.m11 = matrix4x6.m11;
									matrix4x9.m21 = matrix4x6.m21;
									matrix4x9.m31 = matrix4x6.m31;
									matrix4x9.m02 = matrix4x6.m02;
									matrix4x9.m12 = matrix4x6.m12;
									matrix4x9.m22 = matrix4x6.m22;
									matrix4x9.m32 = matrix4x6.m32;
									matrix4x9.m03 = matrix4x6.m03;
									matrix4x9.m13 = matrix4x6.m13;
									matrix4x9.m23 = matrix4x6.m23;
									matrix4x9.m33 = matrix4x6.m33;
									Vector3 vector7 = matrix4x9.MultiplyPoint3x4(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFDF8]") + 32L));
									NativeIntrinsics.Stmt("movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr");
									NativeIntrinsics.Stmt("mov [r14+rcx*4+28h],edx ; value = FieldExpr");
									array16 = <copyOfVertices>5__3;
									num38 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
									Vector3[] array42 = array16[(int)num16];
									matrix4x10.m00 = matrix4x6.m00;
									matrix4x10.m10 = matrix4x6.m10;
									matrix4x10.m20 = matrix4x6.m20;
									matrix4x10.m30 = matrix4x6.m30;
									matrix4x10.m01 = matrix4x6.m01;
									matrix4x10.m11 = matrix4x6.m11;
									matrix4x10.m21 = matrix4x6.m21;
									matrix4x10.m31 = matrix4x6.m31;
									matrix4x10.m02 = matrix4x6.m02;
									matrix4x10.m12 = matrix4x6.m12;
									matrix4x10.m22 = matrix4x6.m22;
									matrix4x10.m32 = matrix4x6.m32;
									matrix4x10.m03 = matrix4x6.m03;
									matrix4x10.m13 = matrix4x6.m13;
									matrix4x10.m23 = matrix4x6.m23;
									matrix4x10.m33 = matrix4x6.m33;
									Vector3 vector8 = matrix4x10.MultiplyPoint3x4(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFDF8]") + 48L));
									NativeIntrinsics.Stmt("movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr");
									NativeIntrinsics.Stmt("mov [r14+rcx*4+28h],edx ; value = FieldExpr");
									num38 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
									Vector3[] array43 = <copyOfVertices>5__3[(int)num16];
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+24h],xmm0 ; value = BinaryExpr");
									float num40 = num15 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
									float num41 = num14 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
									Vector3[] array44 = <copyOfVertices>5__3[(int)num16];
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr");
									num40 = num15 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
									num41 = num14 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
									Vector3[] array45 = <copyOfVertices>5__3[(int)num16];
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr");
									num40 = num15 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
									num41 = num14 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
									Vector3[] array46 = <copyOfVertices>5__3[(int)num16];
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr");
									num40 = num15 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
									num41 = num14 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
								}
								int num42 = (int)num9 + 1;
							}
							while ((int)num9 + 1 <= (int)num10);
						}
						int num43 = 0 + 1;
					}
					while (0 + 1 < (int)NativeIntrinsics.Asm("stack[0x8]"));
					num8 = NativeIntrinsics.Asm("stack[0x18]");
				}
				tMP_TextInfo3 = <textInfo>5__2;
				while (0 < tMP_TextInfo3.meshInfo.Length)
				{
					TMP_TextInfo tMP_TextInfo6 = <textInfo>5__2;
					TMP_MeshInfo[] meshInfo3 = tMP_TextInfo6.meshInfo;
					Vector3[][] array47 = <copyOfVertices>5__3;
					long num44 = NativeIntrinsics.Asm("*(LocalExpr + 0x20)");
					long num45 = 0L;
					NativeIntrinsics.Reinterpret<Mesh>(num44).vertices = array47[0];
					num44 = NativeIntrinsics.Asm("*(LocalExpr + 0x30)");
					int num46 = 0 + 1;
					long num47 = NativeIntrinsics.Asm("*(*(LocalExpr + 0x0) + 0x7F0)");
					TMP_TextInfo tMP_TextInfo7 = <textInfo>5__2;
					int num48 = 0 + 1;
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
			goto IL_1666;
			IL_1666:
			return true;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00266320 File Offset: 0x00264920
		private void System.Collections.IEnumerator.Reset()
		{
			long num = NativeIntrinsics.CallNative("sub_184BE0", NativeIntrinsics.Asm("rax"), 0L, 0L, 0L);
			NativeIntrinsics.Reinterpret<NotSupportedException>(num)..ctor();
			throw null;
		}
	}

	// Token: 0x040003A0 RID: 928 Offset: 0x20
	public float AngleMultiplier = 1f;

	// Token: 0x040003A1 RID: 929 Offset: 0x24
	public float SpeedMultiplier = 1f;

	// Token: 0x040003A2 RID: 930 Offset: 0x28
	public float CurveScale = 1f;

	// Token: 0x040003A3 RID: 931 Offset: 0x30
	private TMP_Text m_TextComponent;

	// Token: 0x040003A4 RID: 932 Offset: 0x38
	private bool hasTextChanged;

	// Token: 0x06000288 RID: 648 RVA: 0x002694C0 File Offset: 0x00267AC0
	private void Awake()
	{
		TMP_Text component = GetComponent<TMP_Text>();
		m_TextComponent = component;
	}

	// Token: 0x06000289 RID: 649 RVA: 0x00269640 File Offset: 0x00267C40
	private void OnEnable()
	{
		FastAction<UnityEngine.Object> tEXT_CHANGED_EVENT = TMPro_EventManager.TEXT_CHANGED_EVENT;
		Action<UnityEngine.Object> rhs = (Action<UnityEngine.Object>)(object)new System.IOAsyncCallback(ON_TEXT_CHANGED);
		tEXT_CHANGED_EVENT.Add(rhs);
	}

	// Token: 0x0600028A RID: 650 RVA: 0x00269570 File Offset: 0x00267B70
	private void OnDisable()
	{
		FastAction<UnityEngine.Object> tEXT_CHANGED_EVENT = TMPro_EventManager.TEXT_CHANGED_EVENT;
		Action<UnityEngine.Object> rhs = (Action<UnityEngine.Object>)(object)new System.IOAsyncCallback(ON_TEXT_CHANGED);
		tEXT_CHANGED_EVENT.Remove(rhs);
	}

	// Token: 0x0600028B RID: 651 RVA: 0x00269710 File Offset: 0x00267D10
	private void Start()
	{
		<AnimateVertexColors>d__10 <AnimateVertexColors>d__ = NativeIntrinsics.Reinterpret<<AnimateVertexColors>d__10>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.VertexShakeB+<AnimateVertexColors>d__10)"));
		((Mono.Xml.SecurityParser)(object)<AnimateVertexColors>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<AnimateVertexColors>d__.<>1__state = 0;
		<AnimateVertexColors>d__.<>4__this = this;
		Coroutine coroutine = StartCoroutine(<AnimateVertexColors>d__);
	}

	// Token: 0x0600028C RID: 652 RVA: 0x00269510 File Offset: 0x00267B10
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

	// Token: 0x0600028D RID: 653 RVA: 0x00269450 File Offset: 0x00267A50
	private IEnumerator AnimateVertexColors()
	{
		<AnimateVertexColors>d__10 <AnimateVertexColors>d__ = NativeIntrinsics.Reinterpret<<AnimateVertexColors>d__10>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.VertexShakeB+<AnimateVertexColors>d__10)"));
		((Mono.Xml.SecurityParser)(object)<AnimateVertexColors>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<AnimateVertexColors>d__.<>1__state = 0;
		<AnimateVertexColors>d__.<>4__this = this;
		return <AnimateVertexColors>d__;
	}
}

