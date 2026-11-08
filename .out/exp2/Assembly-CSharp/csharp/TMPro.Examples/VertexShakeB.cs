using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
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
			int num2 = num;
			if (num != 0)
			{
				num2--;
				int result = default(int);
				if (num2 != 0 && num2 != 1)
				{
					return (byte)result != 0;
				}
				<>1__state = -1;
			}
			else
			{
				<>1__state = -1;
				vertexShakeB.m_TextComponent.ForceMeshUpdate(ignoreActiveState: false, forceTextReparsing: false);
				<textInfo>5__2 = vertexShakeB.m_TextComponent.m_textInfo;
				<copyOfVertices>5__3 = new Vector3[0][];
				vertexShakeB.hasTextChanged = true;
			}
			<AnimateVertexColors>d__10 <AnimateVertexColors>d__ = this;
			if (vertexShakeB.hasTextChanged)
			{
				if (<copyOfVertices>5__3.Length < <textInfo>5__2.meshInfo.Length)
				{
					<copyOfVertices>5__3 = new Vector3[<textInfo>5__2.meshInfo.Length][];
				}
				TMP_TextInfo tMP_TextInfo = <textInfo>5__2;
				int num3 = 0;
				<AnimateVertexColors>d__ = this;
				int num4 = 0;
				while (num3 < tMP_TextInfo.meshInfo.Length)
				{
					_ = <AnimateVertexColors>d__.<textInfo>5__2.meshInfo;
					NativeIntrinsics.Asm("*(LocalExpr + 0x30)");
					Vector3[] array = new Vector3[(int)NativeIntrinsics.Asm("*(LocalExpr + 0x18)")];
					<AnimateVertexColors>d__.<copyOfVertices>5__3[num4] = array;
					_ = <AnimateVertexColors>d__.<textInfo>5__2;
					num4++;
					if (<AnimateVertexColors>d__.<textInfo>5__2 == null)
					{
						break;
					}
				}
				NativeIntrinsics.Stmt("mov byte ptr [rdi+38h],0 ; value = 0x0");
				NativeIntrinsics.Asm("stack[0x18]");
			}
			TMP_TextInfo tMP_TextInfo2 = <AnimateVertexColors>d__.<textInfo>5__2;
			if (tMP_TextInfo2.characterCount != 0)
			{
				if (tMP_TextInfo2.lineCount > 0)
				{
					int num5 = 0;
					float num6 = 0.5f;
					Vector3 euler = default(Vector3);
					Vector3 vector = default(Vector3);
					Quaternion quaternion = default(Quaternion);
					Vector3 s = default(Vector3);
					Matrix4x4 matrix4x2 = default(Matrix4x4);
					Vector3 point = default(Vector3);
					Vector3 point2 = default(Vector3);
					Vector3 point3 = default(Vector3);
					Vector3 point4 = default(Vector3);
					Vector3 vector4 = default(Vector3);
					Quaternion quaternion2 = default(Quaternion);
					Vector3 s2 = default(Vector3);
					Matrix4x4 matrix4x4 = default(Matrix4x4);
					Vector3 point5 = default(Vector3);
					Vector3 point6 = default(Vector3);
					Vector3 point7 = default(Vector3);
					Vector3 point8 = default(Vector3);
					do
					{
						_ = <AnimateVertexColors>d__.<textInfo>5__2.characterInfo;
						long num7 = (int)NativeIntrinsics.Asm("*(FieldExpr + 0x34)");
						_ = <AnimateVertexColors>d__.<textInfo>5__2.characterInfo;
						long num8 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x3C)");
						float num9 = UnityEngine.Random.Range(-0.25f, 0.25f);
						euler.x = 0f;
						euler.z = num9 * ((float)Math.PI / 180f);
						Quaternion.Internal_FromEulerRad(euler);
						_ = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x128)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x11C)")) * num6;
						float num10 = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x130)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x124)")) * num6;
						float num11 = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x12C)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x120)")) * num6;
						if ((int)num7 <= (int)num8)
						{
							do
							{
								if (NativeIntrinsics.Asm("*(FieldExpr + 0x194)") != 0)
								{
									_ = <AnimateVertexColors>d__.<textInfo>5__2.meshInfo;
									_ = <AnimateVertexColors>d__.<textInfo>5__2;
									long num12 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x58)");
									NativeIntrinsics.Asm("*(LocalExpr + 0x30)");
									_ = (int)NativeIntrinsics.Asm("*(FieldExpr + 0x6C)");
									float num13 = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)")) * num6;
									float num14 = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)")) * num6;
									float num15 = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)")) * num6;
									Vector3[][] array2 = <AnimateVertexColors>d__.<copyOfVertices>5__3;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num14;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - num13;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num15;
									_ = array2[(int)num12];
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+20h],xmm0 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+24h],xmm1 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+28h],xmm2 ; value = LocalExpr");
									Vector3[][] array3 = <AnimateVertexColors>d__.<copyOfVertices>5__3;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num14;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - num13;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num15;
									_ = array3[(int)num12];
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*4+20h],xmm0 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*4+24h],xmm1 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*4+28h],xmm2 ; value = LocalExpr");
									Vector3[][] array4 = <AnimateVertexColors>d__.<copyOfVertices>5__3;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num14;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - num13;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num15;
									_ = array4[(int)num12];
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*4+20h],xmm0 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*4+24h],xmm1 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*4+28h],xmm2 ; value = LocalExpr");
									Vector3[][] array5 = <AnimateVertexColors>d__.<copyOfVertices>5__3;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num14;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - num13;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num15;
									_ = array5[(int)num12];
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*4+20h],xmm0 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*4+24h],xmm1 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*4+28h],xmm2 ; value = LocalExpr");
									float num16 = UnityEngine.Random.Range(0.95f, 1.05f);
									NativeIntrinsics.Asm("init_flag_19E3BFF");
									if ((int)NativeIntrinsics.Asm("init_flag_19E3BFF") == 0)
									{
										NativeIntrinsics.Stmt("mov [1819E3BFFh],dl ; value = 0x1");
									}
									Quaternion identityQuaternion = Quaternion.identityQuaternion;
									float z = Vector3.oneVector.z;
									NativeIntrinsics.Asm("init_flag_19E3BFF");
									if ((int)NativeIntrinsics.Asm("init_flag_19E3BFF") == 0)
									{
										NativeIntrinsics.Stmt("mov byte ptr [1819E3BFFh],1 ; value = 0x1");
									}
									float x = num16 * Vector3.oneVector.x;
									float y = num16 * Vector3.oneVector.y;
									float z2 = num16 * Vector3.oneVector.z;
									vector.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("xmm11"));
									vector.z = z;
									Vector3 pos = vector;
									quaternion.x = NativeIntrinsics.Reinterpret<float>(identityQuaternion);
									Quaternion q = quaternion;
									s = new Vector3(x, y, z2);
									Matrix4x4 matrix4x = Matrix4x4.TRS(pos, q, s);
									_ = <AnimateVertexColors>d__.<copyOfVertices>5__3[(int)num12];
									_ = <AnimateVertexColors>d__.<copyOfVertices>5__3[(int)num12];
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
									point.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)"));
									point.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)"));
									matrix4x2.MultiplyPoint3x4(point);
									NativeIntrinsics.Stmt("movsd qword ptr [r14+rdx*4+20h],xmm0 ; value = StructChunkExpr");
									NativeIntrinsics.Stmt("mov [r14+rdx*4+28h],r8d ; value = FieldExpr");
									Vector3[][] array6 = <AnimateVertexColors>d__.<copyOfVertices>5__3;
									_ = array6[(int)num12];
									_ = array6[(int)num12];
									point2.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)"));
									point2.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)"));
									Vector3 vector2 = matrix4x2.MultiplyPoint3x4(point2);
									NativeIntrinsics.Stmt("movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr");
									NativeIntrinsics.Stmt("mov [r14+rcx*4+28h],edx ; value = FieldExpr");
									array6 = <AnimateVertexColors>d__.<copyOfVertices>5__3;
									_ = vector2.z;
									_ = array6[(int)num12];
									_ = array6[(int)num12];
									point3.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)"));
									point3.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)"));
									Vector3 vector3 = matrix4x2.MultiplyPoint3x4(point3);
									NativeIntrinsics.Stmt("movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr");
									NativeIntrinsics.Stmt("mov [r14+rcx*4+28h],edx ; value = FieldExpr");
									array6 = <AnimateVertexColors>d__.<copyOfVertices>5__3;
									_ = vector3.z;
									_ = array6[(int)num12];
									_ = array6[(int)num12];
									point4.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)"));
									point4.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)"));
									matrix4x2.MultiplyPoint3x4(point4);
									NativeIntrinsics.Stmt("movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr");
									NativeIntrinsics.Stmt("mov [r14+rcx*4+28h],edx ; value = FieldExpr");
									_ = <AnimateVertexColors>d__.<copyOfVertices>5__3[(int)num12];
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+24h],xmm0 ; value = BinaryExpr");
									Vector3[][] array7 = <AnimateVertexColors>d__.<copyOfVertices>5__3;
									_ = num14 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
									_ = num15 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
									_ = array7[(int)num12];
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr");
									Vector3[][] array8 = <AnimateVertexColors>d__.<copyOfVertices>5__3;
									_ = num14 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
									_ = num15 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
									_ = array8[(int)num12];
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr");
									Vector3[][] array9 = <AnimateVertexColors>d__.<copyOfVertices>5__3;
									_ = num14 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
									_ = num15 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
									_ = array9[(int)num12];
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+20h],xmm6 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+24h],xmm7 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+28h],xmm8 ; value = BinaryExpr");
									_ = <AnimateVertexColors>d__.<copyOfVertices>5__3[(int)num12];
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+24h],xmm0 ; value = BinaryExpr");
									Vector3[][] array10 = <AnimateVertexColors>d__.<copyOfVertices>5__3;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num10;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num11;
									_ = array10[(int)num12];
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr");
									Vector3[][] array11 = <AnimateVertexColors>d__.<copyOfVertices>5__3;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num10;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num11;
									_ = array11[(int)num12];
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr");
									Vector3[][] array12 = <AnimateVertexColors>d__.<copyOfVertices>5__3;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num10;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num11;
									_ = array12[(int)num12];
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr");
									int num17 = (int)NativeIntrinsics.Asm("init_flag_19E3BFF");
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num10;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num11;
									if ((int)NativeIntrinsics.Asm("init_flag_19E3BFF") == 0)
									{
										NativeIntrinsics.Stmt("mov [1819E3BFFh],cl ; value = 0x1");
										num17 = 1;
									}
									z = Vector3.oneVector.z;
									if (num17 == 0)
									{
										NativeIntrinsics.Stmt("mov byte ptr [1819E3BFFh],1 ; value = 0x1");
									}
									float z3 = Vector3.oneVector.z;
									vector4.x = num13;
									vector4.z = z;
									Vector3 pos2 = vector4;
									quaternion2.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFE48]"));
									Quaternion q2 = quaternion2;
									s2 = new Vector3(Vector3.oneVector.x, Vector3.oneVector.y, z3);
									Matrix4x4 matrix4x3 = Matrix4x4.TRS(pos2, q2, s2);
									_ = <AnimateVertexColors>d__.<copyOfVertices>5__3[(int)num12];
									matrix4x4.m00 = matrix4x3.m00;
									matrix4x4.m10 = matrix4x3.m10;
									matrix4x4.m20 = matrix4x3.m20;
									matrix4x4.m30 = matrix4x3.m30;
									matrix4x4.m01 = matrix4x3.m01;
									matrix4x4.m11 = matrix4x3.m11;
									matrix4x4.m21 = matrix4x3.m21;
									matrix4x4.m31 = matrix4x3.m31;
									matrix4x4.m02 = matrix4x3.m02;
									matrix4x4.m12 = matrix4x3.m12;
									matrix4x4.m22 = matrix4x3.m22;
									matrix4x4.m32 = matrix4x3.m32;
									matrix4x4.m03 = matrix4x3.m03;
									matrix4x4.m13 = matrix4x3.m13;
									matrix4x4.m23 = matrix4x3.m23;
									matrix4x4.m33 = matrix4x3.m33;
									point5.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(ArrayElementExpr + 0x20)"));
									point5.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(ArrayElementExpr + 0x28)"));
									matrix4x4.MultiplyPoint3x4(point5);
									NativeIntrinsics.Stmt("movsd qword ptr [r14+rdx*4+20h],xmm0 ; value = StructChunkExpr");
									NativeIntrinsics.Stmt("mov [r14+rdx*4+28h],r8d ; value = FieldExpr");
									_ = <AnimateVertexColors>d__.<copyOfVertices>5__3[(int)num12];
									point6.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(ArrayElementExpr + 0x20)"));
									point6.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(ArrayElementExpr + 0x28)"));
									matrix4x4.MultiplyPoint3x4(point6);
									NativeIntrinsics.Stmt("movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr");
									NativeIntrinsics.Stmt("mov [r14+rcx*4+28h],edx ; value = FieldExpr");
									_ = <AnimateVertexColors>d__.<copyOfVertices>5__3[(int)num12];
									point7.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(ArrayElementExpr + 0x20)"));
									point7.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(ArrayElementExpr + 0x28)"));
									matrix4x4.MultiplyPoint3x4(point7);
									NativeIntrinsics.Stmt("movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr");
									NativeIntrinsics.Stmt("mov [r14+rcx*4+28h],edx ; value = FieldExpr");
									_ = <AnimateVertexColors>d__.<copyOfVertices>5__3[(int)num12];
									point8.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(ArrayElementExpr + 0x20)"));
									point8.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(ArrayElementExpr + 0x28)"));
									matrix4x4.MultiplyPoint3x4(point8);
									NativeIntrinsics.Stmt("movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr");
									NativeIntrinsics.Stmt("mov [r14+rcx*4+28h],edx ; value = FieldExpr");
									_ = <AnimateVertexColors>d__.<copyOfVertices>5__3[(int)num12];
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+24h],xmm0 ; value = BinaryExpr");
									_ = num11 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
									_ = num10 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
									_ = <AnimateVertexColors>d__.<copyOfVertices>5__3[(int)num12];
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr");
									_ = num11 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
									_ = num10 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
									_ = <AnimateVertexColors>d__.<copyOfVertices>5__3[(int)num12];
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr");
									_ = num11 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
									_ = num10 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
									_ = <AnimateVertexColors>d__.<copyOfVertices>5__3[(int)num12];
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr");
									_ = num11 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
									_ = num10 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
								}
							}
							while ((int)num7 + 1 <= (int)num8);
						}
						num5++;
					}
					while (num5 + 1 < (int)NativeIntrinsics.Asm("stack[0x8]"));
					NativeIntrinsics.Asm("stack[0x18]");
				}
				tMP_TextInfo2 = <AnimateVertexColors>d__.<textInfo>5__2;
				int num18 = 0;
				int num19 = 0;
				while (num18 < tMP_TextInfo2.meshInfo.Length)
				{
					_ = <AnimateVertexColors>d__.<textInfo>5__2.meshInfo;
					Vector3[][] array13 = <AnimateVertexColors>d__.<copyOfVertices>5__3;
					NativeIntrinsics.Reinterpret<Mesh>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)")).vertices = array13[num19];
					NativeIntrinsics.CallNative("indirect@2661E6", NativeIntrinsics.Asm("*(LocalExpr + 0x30)"), NativeIntrinsics.Asm("*(FieldExpr + 0x20)"), num19, NativeIntrinsics.Asm("*(*(LocalExpr + 0x0) + 0x7F0)"));
					num18 = num19 + 1;
					_ = <AnimateVertexColors>d__.<textInfo>5__2;
					num19++;
					if (<AnimateVertexColors>d__.<textInfo>5__2 == null)
					{
						break;
					}
				}
				<AnimateVertexColors>d__.<>2__current = new WaitForSeconds(0.1f);
				<AnimateVertexColors>d__.<>1__state = 2;
			}
			else
			{
				<AnimateVertexColors>d__.<>2__current = new WaitForSeconds(0.25f);
				<AnimateVertexColors>d__.<>1__state = 1;
			}
			return true;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00266320 File Offset: 0x00264920
		private void System.Collections.IEnumerator.Reset()
		{
			throw new NotSupportedException();
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
		m_TextComponent = GetComponent<TMP_Text>();
	}

	// Token: 0x06000289 RID: 649 RVA: 0x00269640 File Offset: 0x00267C40
	private void OnEnable()
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(null, (IntPtr)0));
	}

	// Token: 0x0600028A RID: 650 RVA: 0x00269570 File Offset: 0x00267B70
	private void OnDisable()
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(null, (IntPtr)0));
	}

	// Token: 0x0600028B RID: 651 RVA: 0x00269710 File Offset: 0x00267D10
	private void Start()
	{
		<AnimateVertexColors>d__10 <AnimateVertexColors>d__ = new <AnimateVertexColors>d__10(0);
		<AnimateVertexColors>d__.<>1__state = 0;
		<AnimateVertexColors>d__.<>4__this = this;
		StartCoroutine(<AnimateVertexColors>d__);
	}

	// Token: 0x0600028C RID: 652 RVA: 0x00269510 File Offset: 0x00267B10
	private void ON_TEXT_CHANGED(UnityEngine.Object obj)
	{
		if ((bool)m_TextComponent)
		{
			hasTextChanged = true;
		}
	}

	// Token: 0x0600028D RID: 653 RVA: 0x00269450 File Offset: 0x00267A50
	private IEnumerator AnimateVertexColors()
	{
		<AnimateVertexColors>d__10 <AnimateVertexColors>d__ = new <AnimateVertexColors>d__10(0);
		<AnimateVertexColors>d__.<>1__state = 0;
		<AnimateVertexColors>d__.<>4__this = this;
		return <AnimateVertexColors>d__;
	}
}

