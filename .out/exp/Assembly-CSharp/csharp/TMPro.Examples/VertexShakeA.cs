using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x0200008E RID: 142
public class VertexShakeA : MonoBehaviour
{
	// Token: 0x0200008F RID: 143
	private sealed class _AnimateVertexColors_d__11 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x0400039B RID: 923 Offset: 0x10
		// Il2CppStudio: declared private; widened because native code inlined into other types accesses it.
		internal int __1__state;

		// Token: 0x0400039C RID: 924 Offset: 0x18
		private object __2__current;

		// Token: 0x0400039D RID: 925 Offset: 0x20
		public VertexShakeA __4__this;

		// Token: 0x0400039E RID: 926 Offset: 0x28
		private TMP_TextInfo _textInfo_5__2;

		// Token: 0x0400039F RID: 927 Offset: 0x30
		private Vector3[][] _copyOfVertices_5__3;

		object System.Collections.Generic.IEnumerator<System.Object>.Current => __2__current;

		object System.Collections.IEnumerator.Current => __2__current;

		// Token: 0x06000282 RID: 642 RVA: 0x00250B50 File Offset: 0x0024F150
		public _AnimateVertexColors_d__11(int __1__state)
		{
			this.__1__state = __1__state;
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00250B40 File Offset: 0x0024F140
		void System.IDisposable.Dispose()
		{
		}

		// Token: 0x06000284 RID: 644 RVA: 0x002663A0 File Offset: 0x002649A0
		bool System.Collections.IEnumerator.MoveNext()
		{
			int num = __1__state;
			VertexShakeA vertexShakeA = __4__this;
			int num2 = num;
			if (num != 0)
			{
				num2--;
				int result = default(int);
				if (num2 != 0 && num2 != 1)
				{
					return (byte)result != 0;
				}
				__1__state = -1;
			}
			else
			{
				__1__state = -1;
				vertexShakeA.m_TextComponent.ForceMeshUpdate(ignoreActiveState: false, forceTextReparsing: false);
				_textInfo_5__2 = vertexShakeA.m_TextComponent.textInfo;
				_copyOfVertices_5__3 = new Vector3[0][];
				vertexShakeA.hasTextChanged = true;
			}
			_AnimateVertexColors_d__11 _AnimateVertexColors_d__ = this;
			if (vertexShakeA.hasTextChanged)
			{
				if (_copyOfVertices_5__3.Length < _textInfo_5__2.meshInfo.Length)
				{
					_copyOfVertices_5__3 = new Vector3[_textInfo_5__2.meshInfo.Length][];
				}
				TMP_TextInfo tMP_TextInfo = _textInfo_5__2;
				int num3 = 0;
				_AnimateVertexColors_d__ = this;
				int num4 = 0;
				while (num3 < tMP_TextInfo.meshInfo.Length)
				{
					_ = _AnimateVertexColors_d__._textInfo_5__2.meshInfo;
					NativeIntrinsics.Asm("*(LocalExpr + 0x30)");
					Vector3[] array = new Vector3[(int)NativeIntrinsics.Asm("*(LocalExpr + 0x18)")];
					_AnimateVertexColors_d__._copyOfVertices_5__3[num4] = array;
					_ = _AnimateVertexColors_d__._textInfo_5__2;
					num4++;
					if (_AnimateVertexColors_d__._textInfo_5__2 == null)
					{
						break;
					}
				}
				vertexShakeA.hasTextChanged = false;
			}
			TMP_TextInfo tMP_TextInfo2 = _AnimateVertexColors_d__._textInfo_5__2;
			if (tMP_TextInfo2.characterCount != 0)
			{
				if (tMP_TextInfo2.lineCount > 0)
				{
					float num5 = 0.5f;
					float num6 = 0.995f;
					float num7 = 0.001f;
					float num8 = 1.005f;
					Vector3 euler = default(Vector3);
					Vector3 vector = default(Vector3);
					Vector3 s = default(Vector3);
					Matrix4x4 matrix4x2 = default(Matrix4x4);
					Vector3 point = default(Vector3);
					Vector3 point2 = default(Vector3);
					Vector3 point3 = default(Vector3);
					Vector3 point4 = default(Vector3);
					do
					{
						_ = _AnimateVertexColors_d__._textInfo_5__2.characterInfo;
						long num9 = (int)NativeIntrinsics.Asm("*(FieldExpr + 0x34)");
						long num10 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x3C)");
						float num11 = UnityEngine.Random.Range(-0.25f, 0.25f);
						euler.x = 0f;
						euler.z = num11 * vertexShakeA.RotationMultiplier * ((float)Math.PI / 180f);
						Quaternion q = Quaternion.Internal_FromEulerRad(euler);
						float num12 = ((float)(int)NativeIntrinsics.Asm("*(FieldExpr + 0x128)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x11C)")) * num5;
						float num13 = ((float)(int)NativeIntrinsics.Asm("*(FieldExpr + 0x12C)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x120)")) * num5;
						float num14 = ((float)(int)NativeIntrinsics.Asm("*(FieldExpr + 0x130)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x124)")) * num5;
						if ((int)num9 <= (int)num10)
						{
							do
							{
								if (NativeIntrinsics.Asm("*(FieldExpr + 0x194)") != 0)
								{
									_ = _AnimateVertexColors_d__._textInfo_5__2.meshInfo;
									long num15 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x58)");
									Vector3[][] array2 = _AnimateVertexColors_d__._copyOfVertices_5__3;
									NativeIntrinsics.Asm("*(LocalExpr + 0x30)");
									_ = (int)NativeIntrinsics.Asm("*(FieldExpr + 0x6C)");
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num14;
									_ = array2[(int)num15];
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - num12;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num13;
									NativeIntrinsics.Stmt("movss dword ptr [r8+rcx*4+20h],xmm0 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [r8+rcx*4+24h],xmm1 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [r8+rcx*4+28h],xmm2 ; value = LocalExpr");
									Vector3[][] array3 = _AnimateVertexColors_d__._copyOfVertices_5__3;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num14;
									_ = array3[(int)num15];
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - num12;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num13;
									NativeIntrinsics.Stmt("movss dword ptr [r8+rax*4+20h],xmm0 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [r8+rax*4+24h],xmm1 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [r8+rax*4+28h],xmm2 ; value = LocalExpr");
									Vector3[][] array4 = _AnimateVertexColors_d__._copyOfVertices_5__3;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num14;
									_ = array4[(int)num15];
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - num12;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num13;
									NativeIntrinsics.Stmt("movss dword ptr [r8+rax*4+20h],xmm0 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [r8+rax*4+24h],xmm1 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [r8+rax*4+28h],xmm2 ; value = LocalExpr");
									Vector3[][] array5 = _AnimateVertexColors_d__._copyOfVertices_5__3;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num14;
									_ = array5[(int)num15];
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - num12;
									_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num13;
									NativeIntrinsics.Stmt("movss dword ptr [r8+rax*4+20h],xmm0 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [r8+rax*4+24h],xmm1 ; value = LocalExpr");
									NativeIntrinsics.Stmt("movss dword ptr [r8+rax*4+28h],xmm2 ; value = LocalExpr");
									float num16 = UnityEngine.Random.Range(num6 - vertexShakeA.ScaleMultiplier * num7, vertexShakeA.ScaleMultiplier * num7 + num8);
									int num17 = (int)NativeIntrinsics.Asm("init_flag_19E3BFF");
									float num18 = num16;
									if ((int)NativeIntrinsics.Asm("init_flag_19E3BFF") == 0)
									{
										NativeIntrinsics.Stmt("mov [1819E3BFFh],cl ; value = 0x1");
										num17 = 1;
									}
									float z = Vector3.one.z;
									if (num17 == 0)
									{
										NativeIntrinsics.Stmt("mov byte ptr [1819E3BFFh],1 ; value = 0x1");
									}
									float x = num18 * Vector3.one.x;
									vector.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("xmm6"));
									vector.z = z;
									Vector3 pos = vector;
									s = new Vector3(x, num18 * Vector3.one.y, num18 * Vector3.one.z);
									Matrix4x4 matrix4x = Matrix4x4.TRS(pos, q, s);
									_ = _AnimateVertexColors_d__._copyOfVertices_5__3[(int)num15];
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
									point.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(ArrayElementExpr + 0x20)"));
									point.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(ArrayElementExpr + 0x28)"));
									matrix4x2.MultiplyPoint3x4(point);
									NativeIntrinsics.Stmt("movsd qword ptr [r14+rdx*4+20h],xmm0 ; value = StructChunkExpr");
									NativeIntrinsics.Stmt("mov [r14+rdx*4+28h],r8d ; value = FieldExpr");
									_ = _AnimateVertexColors_d__._copyOfVertices_5__3[(int)num15];
									point2.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(ArrayElementExpr + 0x20)"));
									point2.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(ArrayElementExpr + 0x28)"));
									matrix4x2.MultiplyPoint3x4(point2);
									NativeIntrinsics.Stmt("movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr");
									NativeIntrinsics.Stmt("mov [r14+rcx*4+28h],edx ; value = FieldExpr");
									_ = _AnimateVertexColors_d__._copyOfVertices_5__3[(int)num15];
									point3.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(ArrayElementExpr + 0x20)"));
									point3.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(ArrayElementExpr + 0x28)"));
									matrix4x2.MultiplyPoint3x4(point3);
									NativeIntrinsics.Stmt("movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr");
									NativeIntrinsics.Stmt("mov [r14+rcx*4+28h],edx ; value = FieldExpr");
									_ = _AnimateVertexColors_d__._copyOfVertices_5__3[(int)num15];
									point4.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(ArrayElementExpr + 0x20)"));
									point4.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(ArrayElementExpr + 0x28)"));
									matrix4x2.MultiplyPoint3x4(point4);
									NativeIntrinsics.Stmt("movsd qword ptr [r14+rcx*4+20h],xmm0 ; value = StructChunkExpr");
									NativeIntrinsics.Stmt("mov [r14+rcx*4+28h],edx ; value = FieldExpr");
									_ = _AnimateVertexColors_d__._copyOfVertices_5__3[(int)num15];
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+24h],xmm0 ; value = BinaryExpr");
									_ = num13 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
									_ = num14 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
									_ = _AnimateVertexColors_d__._copyOfVertices_5__3[(int)num15];
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr");
									_ = num13 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
									_ = num14 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
									_ = _AnimateVertexColors_d__._copyOfVertices_5__3[(int)num15];
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr");
									_ = num13 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
									_ = num14 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
									_ = _AnimateVertexColors_d__._copyOfVertices_5__3[(int)num15];
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+20h],xmm0 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+28h],xmm1 ; value = BinaryExpr");
									NativeIntrinsics.Stmt("movss dword ptr [rcx+rax*4+24h],xmm0 ; value = BinaryExpr");
									_ = num13 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
									_ = num14 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
								}
							}
							while ((int)num9 + 1 <= (int)num10);
						}
						_ = (int)NativeIntrinsics.Asm("stack[0x8]") + 1;
					}
					while ((int)NativeIntrinsics.Asm("stack[0x8]") + 1 < (int)NativeIntrinsics.Asm("stack[0x18]"));
				}
				tMP_TextInfo2 = _AnimateVertexColors_d__._textInfo_5__2;
				int num19 = 0;
				int num20 = 0;
				while (num19 < tMP_TextInfo2.meshInfo.Length)
				{
					_ = _AnimateVertexColors_d__._textInfo_5__2.meshInfo;
					Vector3[][] array6 = _AnimateVertexColors_d__._copyOfVertices_5__3;
					NativeIntrinsics.Reinterpret<Mesh>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)")).vertices = array6[num20];
					vertexShakeA.m_TextComponent.UpdateGeometry(NativeIntrinsics.Reinterpret<Mesh>(NativeIntrinsics.Asm("*(FieldExpr + 0x20)")), num20);
					num19 = num20 + 1;
					_ = _AnimateVertexColors_d__._textInfo_5__2;
					num20++;
					if (_AnimateVertexColors_d__._textInfo_5__2 == null)
					{
						break;
					}
				}
				_AnimateVertexColors_d__.__2__current = new WaitForSeconds(0.1f);
				_AnimateVertexColors_d__.__1__state = 2;
			}
			else
			{
				_AnimateVertexColors_d__.__2__current = new WaitForSeconds(0.25f);
				_AnimateVertexColors_d__.__1__state = 1;
			}
			return true;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00267D70 File Offset: 0x00266370
		void System.Collections.IEnumerator.Reset()
		{
			throw new NotSupportedException();
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
		m_TextComponent = GetComponent<TMP_Text>();
	}

	// Token: 0x0600027C RID: 636 RVA: 0x002692D0 File Offset: 0x002678D0
	private void OnEnable()
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Add(Il2CppStudio.Native.NativeIntrinsics.Reinterpret<Action<UnityEngine.Object>>("unresolved delegate target"));
	}

	// Token: 0x0600027D RID: 637 RVA: 0x00269200 File Offset: 0x00267800
	private void OnDisable()
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(Il2CppStudio.Native.NativeIntrinsics.Reinterpret<Action<UnityEngine.Object>>("unresolved delegate target"));
	}

	// Token: 0x0600027E RID: 638 RVA: 0x002693A0 File Offset: 0x002679A0
	private void Start()
	{
		_AnimateVertexColors_d__11 _AnimateVertexColors_d__ = new _AnimateVertexColors_d__11(0);
		_AnimateVertexColors_d__.__1__state = 0;
		_AnimateVertexColors_d__.__4__this = this;
		StartCoroutine(_AnimateVertexColors_d__);
	}

	// Token: 0x0600027F RID: 639 RVA: 0x002691A0 File Offset: 0x002677A0
	private void ON_TEXT_CHANGED(UnityEngine.Object obj)
	{
		if ((bool)m_TextComponent)
		{
			hasTextChanged = true;
		}
	}

	// Token: 0x06000280 RID: 640 RVA: 0x002690E0 File Offset: 0x002676E0
	private IEnumerator AnimateVertexColors()
	{
		_AnimateVertexColors_d__11 _AnimateVertexColors_d__ = new _AnimateVertexColors_d__11(0);
		_AnimateVertexColors_d__.__1__state = 0;
		_AnimateVertexColors_d__.__4__this = this;
		return _AnimateVertexColors_d__;
	}
}

