using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using Mono.Xml;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x02000092 RID: 146
public class VertexZoom : MonoBehaviour
{
	// Token: 0x02000093 RID: 147
	private sealed class <>c__DisplayClass10_0
	{
		// Token: 0x040003AF RID: 943 Offset: 0x10
		public List<float> modifiedCharScale;

		// Token: 0x040003B0 RID: 944 Offset: 0x18
		public Comparison<int> <>9__0;

		// Token: 0x0600029C RID: 668 RVA: 0x00263AB0 File Offset: 0x002620B0
		public <>c__DisplayClass10_0()
		{
			((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x002680C0 File Offset: 0x002666C0
		internal int <AnimateVertexColors>b__0(int a, int b)
		{
			float value = modifiedCharScale[a];
			float value2 = modifiedCharScale[b];
			float num = default(float);
			num.m_value = value;
			return num.CompareTo(value2);
		}
	}

	// Token: 0x02000094 RID: 148
	private sealed class <AnimateVertexColors>d__10 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x040003B1 RID: 945 Offset: 0x10
		private int <>1__state;

		// Token: 0x040003B2 RID: 946 Offset: 0x18
		private object <>2__current;

		// Token: 0x040003B3 RID: 947 Offset: 0x20
		public VertexZoom <>4__this;

		// Token: 0x040003B4 RID: 948 Offset: 0x28
		private <>c__DisplayClass10_0 <>8__1;

		// Token: 0x040003B5 RID: 949 Offset: 0x30
		private TMP_TextInfo <textInfo>5__2;

		// Token: 0x040003B6 RID: 950 Offset: 0x38
		private TMP_MeshInfo[] <cachedMeshInfoVertexData>5__3;

		// Token: 0x040003B7 RID: 951 Offset: 0x40
		private List<int> <scaleSortingOrder>5__4;

		private object System.Collections.Generic.IEnumerator<System.Object>.Current => <>2__current;

		private object System.Collections.IEnumerator.Current => <>2__current;

		// Token: 0x0600029E RID: 670 RVA: 0x00250B50 File Offset: 0x0024F150
		public <AnimateVertexColors>d__10(int <>1__state)
		{
			((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
			this.<>1__state = <>1__state;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00250B40 File Offset: 0x0024F140
		private void System.IDisposable.Dispose()
		{
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00264000 File Offset: 0x00262600
		private unsafe bool MoveNext()
		{
			int num = <>1__state;
			VertexZoom vertexZoom = <>4__this;
			VertexZoom vertexZoom2 = vertexZoom;
			int num2 = num;
			if (num != 0)
			{
				num2--;
				if (num2 - 1 != 0 && num2 != 1)
				{
					goto IL_0b3a;
				}
				<>1__state = -1;
			}
			else
			{
				<>1__state = -1;
				<>c__DisplayClass10_0 <>c__DisplayClass10_ = new <>c__DisplayClass10_0();
				((Mono.Xml.SecurityParser)(object)<>c__DisplayClass10_).OnStartParsing((Mono.Xml.SmallXmlParser)null);
				<>8__1 = <>c__DisplayClass10_;
				TMP_Text textComponent = vertexZoom2.m_TextComponent;
				TMP_TextInfo textInfo = textComponent.m_textInfo;
				<textInfo>5__2 = textInfo;
				TMP_MeshInfo[] array = <textInfo>5__2.CopyMeshInfoVertexData();
				<cachedMeshInfoVertexData>5__3 = array;
				<>c__DisplayClass10_0 <>c__DisplayClass10_2 = <>8__1;
				List<float> modifiedCharScale = new List<float>();
				<>c__DisplayClass10_2.modifiedCharScale = modifiedCharScale;
				List<int> list = new List<int>();
				<scaleSortingOrder>5__4 = list;
				vertexZoom2.hasTextChanged = true;
			}
			if (vertexZoom2.hasTextChanged)
			{
				TMP_MeshInfo[] array2 = <textInfo>5__2.CopyMeshInfoVertexData();
				<cachedMeshInfoVertexData>5__3 = array2;
				vertexZoom2.hasTextChanged = false;
			}
			int characterCount = <textInfo>5__2.characterCount;
			if (<textInfo>5__2.characterCount != 0)
			{
				List<float> modifiedCharScale2 = <>8__1.modifiedCharScale;
				NativeIntrinsics.Stmt("inc dword ptr [rcx+1Ch] ; value = BinaryExpr");
				NativeIntrinsics.Stmt("mov dword ptr [rcx+18h],0 ; value = 0x0");
				List<int> list2 = <scaleSortingOrder>5__4;
				NativeIntrinsics.Stmt("inc dword ptr [rcx+1Ch] ; value = BinaryExpr");
				NativeIntrinsics.Stmt("mov dword ptr [rcx+18h],0 ; value = 0x0");
				if (characterCount > 0)
				{
					Vector3 vector = default(Vector3);
					Vector3 s = default(Vector3);
					Vector3 point = default(Vector3);
					Vector3 point2 = default(Vector3);
					Vector3 point3 = default(Vector3);
					do
					{
						long num3 = NativeIntrinsics.CallNative("sub_32D0", NativeIntrinsics.Reinterpret<long>(<textInfo>5__2.characterInfo), NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFD78]") + 96L, 0L, 0L);
						if (NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFF4C]") != 0)
						{
							TMP_MeshInfo[] array3 = <cachedMeshInfoVertexData>5__3;
							long num4 = NativeIntrinsics.Asm("*(LocalExpr + 0x30)");
							float num5 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)")) * 0.5f;
							float num6 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)")) * 0.5f;
							float num7 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - 0f;
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+20h],xmm0 ; value = LocalExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+24h],xmm1 ; value = LocalExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+28h],xmm2 ; value = LocalExpr");
							num5 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)")) * 0.5f;
							num6 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)")) * 0.5f;
							num7 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - 0f;
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = LocalExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm1 ; value = LocalExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm2 ; value = LocalExpr");
							num5 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)")) * 0.5f;
							num6 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)")) * 0.5f;
							num7 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - 0f;
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = LocalExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm1 ; value = LocalExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm2 ; value = LocalExpr");
							num5 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)")) * 0.5f;
							num6 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)")) * 0.5f;
							num7 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - 0f;
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = LocalExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm1 ; value = LocalExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm2 ; value = LocalExpr");
							float num8 = UnityEngine.Random.Range(1f, 1.5f);
							num5 = num8;
							<>8__1.modifiedCharScale.Add(num5);
							<scaleSortingOrder>5__4.Add((int)NativeIntrinsics.Asm("*(FieldExpr + 0x18)") - 1);
							float num9 = num8 * Vector3.oneVector.x;
							vector.x = 0f;
							vector.z = 0f;
							Vector3 pos = vector;
							Quaternion q = NativeIntrinsics.Reinterpret<Quaternion>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFD78]") + -48L);
							s.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("unpcklps xmm1,xmm0"));
							s.z = num8 * Vector3.oneVector.z;
							Matrix4x4 matrix4x = Matrix4x4.TRS(pos, q, s);
							long num10 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x20, 16)");
							long num11 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x30, 16)");
							long num12 = NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFD78]") + -112L;
							point.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)"));
							point.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x28)"));
							Vector3 vector2 = ((Matrix4x4*)num12)->MultiplyPoint3x4(point);
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rdx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rdx*4+28h],r8d ; value = FieldExpr");
							float z = vector2.z;
							num10 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
							long num13 = NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFD78]") + -112L;
							point2.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)"));
							point2.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x28)"));
							Vector3 vector3 = ((Matrix4x4*)num13)->MultiplyPoint3x4(point2);
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rcx*4+28h],edx ; value = FieldExpr");
							num10 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
							long num14 = NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFD78]") + -112L;
							point3.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)"));
							point3.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x28)"));
							Vector3 vector4 = ((Matrix4x4*)num14)->MultiplyPoint3x4(point3);
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rcx*4+28h],edx ; value = FieldExpr");
							num10 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
							Vector3 vector5 = ((Matrix4x4*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFD78]") + -112L))->MultiplyPoint3x4(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFD78]") + -128L));
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rcx*4+28h],edx ; value = FieldExpr");
							num10 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+24h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+28h],xmm1 ; value = BinaryExpr");
							float num15 = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)")) * 0.5f + (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x24)");
							float num16 = 0f + (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							num15 = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)")) * 0.5f + (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x24)");
							num16 = 0f + (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							num15 = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)")) * 0.5f + (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x24)");
							num16 = 0f + (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm6 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm7 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr");
							TMP_MeshInfo[] array4 = <cachedMeshInfoVertexData>5__3;
							num15 = 0f + (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x28)");
							long num17 = NativeIntrinsics.Asm("*(LocalExpr + 0x48)");
							NativeIntrinsics.Stmt("movss dword ptr [rdx+rdi*8+20h],xmm0 ; value = *(LocalExpr + 0x20)");
							NativeIntrinsics.Stmt("movss dword ptr [rdx+rdi*8+24h],xmm1 ; value = *(LocalExpr + 0x24)");
							long num18 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 1L;
							int num19 = (int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)");
							int num20 = (int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*8+24h],xmm0 ; value = *(LocalExpr + 0x24)");
							NativeIntrinsics.Stmt("mov [rdx+rcx*8+20h],eax ; value = *(LocalExpr + 0x20)");
							num18 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 2L;
							num19 = (int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*8+24h],xmm0 ; value = *(LocalExpr + 0x24)");
							NativeIntrinsics.Stmt("mov [rdx+rcx*8+20h],eax ; value = *(LocalExpr + 0x20)");
							num18 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 3L;
							num19 = (int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*8+24h],xmm0 ; value = *(LocalExpr + 0x24)");
							NativeIntrinsics.Stmt("mov [rdx+rcx*8+20h],eax ; value = *(LocalExpr + 0x20)");
							TMP_MeshInfo[] array5 = <cachedMeshInfoVertexData>5__3;
							num19 = (int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							long num21 = NativeIntrinsics.Asm("*(LocalExpr + 0x58)");
							NativeIntrinsics.Stmt("mov [rdx+rdi*4+20h],eax ; value = *(LocalExpr + 0x20)");
							num18 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 1L;
							NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = *(LocalExpr + 0x20)");
							num18 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 2L;
							NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = *(LocalExpr + 0x20)");
							num18 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 3L;
							NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = *(LocalExpr + 0x20)");
						}
						int num22 = 0 + 1;
					}
					while (0 + 1 < characterCount);
				}
				TMP_TextInfo tMP_TextInfo = <textInfo>5__2;
				while (0 < tMP_TextInfo.meshInfo.Length)
				{
					<>c__DisplayClass10_0 <>c__DisplayClass10_3 = <>8__1;
					List<int> list3 = <scaleSortingOrder>5__4;
					Comparison<int> comparison = <>c__DisplayClass10_3.<>9__0;
					if (<>c__DisplayClass10_3.<>9__0 == null)
					{
						<>c__DisplayClass10_0 CS$<>8__locals3 = <>8__1;
						comparison = (CS$<>8__locals3.<>9__0 = delegate(int a, int b)
						{
							float value = CS$<>8__locals3.modifiedCharScale[a];
							float value2 = CS$<>8__locals3.modifiedCharScale[b];
							float num27 = default(float);
							num27.m_value = value;
							return num27.CompareTo(value2);
						});
					}
					list3.Sort(comparison);
					TMP_TextInfo tMP_TextInfo2 = <textInfo>5__2;
					TMP_MeshInfo[] meshInfo = tMP_TextInfo2.meshInfo;
					List<int> sortingOrder = <scaleSortingOrder>5__4;
					((TMP_MeshInfo*)((0 + 0 * 4L << 4) + 32L + NativeIntrinsics.Reinterpret<long>(meshInfo)))->SortGeometry(sortingOrder);
					TMP_TextInfo tMP_TextInfo3 = <textInfo>5__2;
					TMP_MeshInfo[] meshInfo2 = tMP_TextInfo3.meshInfo;
					long num23 = NativeIntrinsics.Asm("*(LocalExpr + 0x20)");
					long num24 = 0L;
					NativeIntrinsics.Reinterpret<Mesh>(num23).vertices = NativeIntrinsics.Reinterpret<Vector3[]>(NativeIntrinsics.Asm("*(FieldExpr + 0x30)"));
					TMP_TextInfo tMP_TextInfo4 = <textInfo>5__2;
					TMP_MeshInfo[] meshInfo3 = tMP_TextInfo4.meshInfo;
					num23 = NativeIntrinsics.Asm("*(LocalExpr + 0x20)");
					num24 = 0L;
					NativeIntrinsics.Reinterpret<Mesh>(num23).uv = NativeIntrinsics.Reinterpret<Vector2[]>(NativeIntrinsics.Asm("*(FieldExpr + 0x48)"));
					TMP_TextInfo tMP_TextInfo5 = <textInfo>5__2;
					TMP_MeshInfo[] meshInfo4 = tMP_TextInfo5.meshInfo;
					num23 = NativeIntrinsics.Asm("*(LocalExpr + 0x20)");
					num24 = 0L;
					NativeIntrinsics.Reinterpret<Mesh>(num23).colors32 = NativeIntrinsics.Reinterpret<Color32[]>(NativeIntrinsics.Asm("*(FieldExpr + 0x58)"));
					TMP_Text textComponent2 = vertexZoom2.m_TextComponent;
					int num25 = 0 + 1;
					TMP_TextInfo tMP_TextInfo6 = <textInfo>5__2;
					int num26 = 0 + 1;
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
			goto IL_0b3a;
			IL_0b3a:
			return true;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00266360 File Offset: 0x00264960
		private void System.Collections.IEnumerator.Reset()
		{
			long num = NativeIntrinsics.CallNative("sub_184BE0", NativeIntrinsics.Asm("rax"), 0L, 0L, 0L);
			NativeIntrinsics.Reinterpret<NotSupportedException>(num)..ctor();
			throw null;
		}
	}

	// Token: 0x040003AA RID: 938 Offset: 0x20
	public float AngleMultiplier = 1f;

	// Token: 0x040003AB RID: 939 Offset: 0x24
	public float SpeedMultiplier = 1f;

	// Token: 0x040003AC RID: 940 Offset: 0x28
	public float CurveScale = 1f;

	// Token: 0x040003AD RID: 941 Offset: 0x30
	private TMP_Text m_TextComponent;

	// Token: 0x040003AE RID: 942 Offset: 0x38
	private bool hasTextChanged;

	// Token: 0x06000295 RID: 661 RVA: 0x00269800 File Offset: 0x00267E00
	private void Awake()
	{
		TMP_Text component = GetComponent<TMP_Text>();
		m_TextComponent = component;
	}

	// Token: 0x06000296 RID: 662 RVA: 0x00269990 File Offset: 0x00267F90
	private void OnEnable()
	{
		FastAction<UnityEngine.Object> tEXT_CHANGED_EVENT = TMPro_EventManager.TEXT_CHANGED_EVENT;
		Action<UnityEngine.Object> rhs = (Action<UnityEngine.Object>)(object)new System.IOAsyncCallback(ON_TEXT_CHANGED);
		tEXT_CHANGED_EVENT.Add(rhs);
	}

	// Token: 0x06000297 RID: 663 RVA: 0x002698C0 File Offset: 0x00267EC0
	private void OnDisable()
	{
		FastAction<UnityEngine.Object> tEXT_CHANGED_EVENT = TMPro_EventManager.TEXT_CHANGED_EVENT;
		Action<UnityEngine.Object> rhs = (Action<UnityEngine.Object>)(object)new System.IOAsyncCallback(ON_TEXT_CHANGED);
		tEXT_CHANGED_EVENT.Remove(rhs);
	}

	// Token: 0x06000298 RID: 664 RVA: 0x00269A60 File Offset: 0x00268060
	private void Start()
	{
		<AnimateVertexColors>d__10 <AnimateVertexColors>d__ = NativeIntrinsics.Reinterpret<<AnimateVertexColors>d__10>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.VertexZoom+<AnimateVertexColors>d__10)"));
		((Mono.Xml.SecurityParser)(object)<AnimateVertexColors>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<AnimateVertexColors>d__.<>1__state = 0;
		<AnimateVertexColors>d__.<>4__this = this;
		Coroutine coroutine = StartCoroutine(<AnimateVertexColors>d__);
	}

	// Token: 0x06000299 RID: 665 RVA: 0x00269850 File Offset: 0x00267E50
	private void ON_TEXT_CHANGED(UnityEngine.Object obj)
	{
		TMP_Text textComponent = m_TextComponent;
		bool flag = obj == textComponent;
		bool flag2 = flag;
		if (flag)
		{
			hasTextChanged = true;
		}
	}

	// Token: 0x0600029A RID: 666 RVA: 0x00269790 File Offset: 0x00267D90
	private IEnumerator AnimateVertexColors()
	{
		<AnimateVertexColors>d__10 <AnimateVertexColors>d__ = NativeIntrinsics.Reinterpret<<AnimateVertexColors>d__10>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.VertexZoom+<AnimateVertexColors>d__10)"));
		((Mono.Xml.SecurityParser)(object)<AnimateVertexColors>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<AnimateVertexColors>d__.<>1__state = 0;
		<AnimateVertexColors>d__.<>4__this = this;
		return <AnimateVertexColors>d__;
	}
}

