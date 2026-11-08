using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x0200008B RID: 139
public class VertexJitter : MonoBehaviour
{
	// Token: 0x0200008C RID: 140
	private struct VertexAnim
	{
		// Token: 0x0400038B RID: 907 Offset: 0x0
		public float angleRange;

		// Token: 0x0400038C RID: 908 Offset: 0x4
		public float angle;

		// Token: 0x0400038D RID: 909 Offset: 0x8
		public float speed;
	}

	// Token: 0x0200008D RID: 141
	private sealed class _AnimateVertexColors_d__11 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x0400038E RID: 910 Offset: 0x10
		// Il2CppStudio: declared private; widened because native code inlined into other types accesses it.
		internal int __1__state;

		// Token: 0x0400038F RID: 911 Offset: 0x18
		private object __2__current;

		// Token: 0x04000390 RID: 912 Offset: 0x20
		public VertexJitter __4__this;

		// Token: 0x04000391 RID: 913 Offset: 0x28
		private TMP_TextInfo _textInfo_5__2;

		// Token: 0x04000392 RID: 914 Offset: 0x30
		private int _loopCount_5__3;

		// Token: 0x04000393 RID: 915 Offset: 0x38
		private VertexAnim[] _vertexAnim_5__4;

		// Token: 0x04000394 RID: 916 Offset: 0x40
		private TMP_MeshInfo[] _cachedMeshInfo_5__5;

		object System.Collections.Generic.IEnumerator<System.Object>.Current => __2__current;

		object System.Collections.IEnumerator.Current => __2__current;

		// Token: 0x06000275 RID: 629 RVA: 0x00250B50 File Offset: 0x0024F150
		public _AnimateVertexColors_d__11(int __1__state)
		{
			this.__1__state = __1__state;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00250B40 File Offset: 0x0024F140
		void System.IDisposable.Dispose()
		{
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00267130 File Offset: 0x00265730
		bool System.Collections.IEnumerator.MoveNext()
		{
			int num = __1__state;
			VertexJitter vertexJitter = __4__this;
			int num2 = num;
			int num3 = default(int);
			_AnimateVertexColors_d__11 _AnimateVertexColors_d__ = default(_AnimateVertexColors_d__11);
			float num4 = default(float);
			float num5 = default(float);
			if (num != 0)
			{
				num2--;
				int result = default(int);
				if (num2 != 0 && num2 != 1)
				{
					return (byte)result != 0;
				}
				__1__state = -1;
				num3 = 0;
				_AnimateVertexColors_d__ = this;
				num4 = 25f;
				num5 = 1f;
			}
			else
			{
				__1__state = -1;
				vertexJitter.m_TextComponent.ForceMeshUpdate(ignoreActiveState: false, forceTextReparsing: false);
				_textInfo_5__2 = vertexJitter.m_TextComponent.textInfo;
				_loopCount_5__3 = 0;
				vertexJitter.hasTextChanged = true;
				_vertexAnim_5__4 = new VertexAnim[1024];
				_AnimateVertexColors_d__ = this;
				num4 = 25f;
				float minInclusive = 10f;
				int num6 = 0;
				float maxInclusive = 3f;
				num5 = 1f;
				num3 = 0;
				do
				{
					_ = _AnimateVertexColors_d__._vertexAnim_5__4;
					UnityEngine.Random.Range(minInclusive, num4);
					NativeIntrinsics.Stmt("movss dword ptr [rdi+rcx*4+20h],xmm0 ; value = LocalExpr");
					_ = _AnimateVertexColors_d__._vertexAnim_5__4;
					UnityEngine.Random.Range(num5, maxInclusive);
					NativeIntrinsics.Stmt("movss dword ptr [rdi+rcx*4+28h],xmm0 ; value = LocalExpr");
					num6++;
				}
				while (num6 + 1 < 1024);
				_AnimateVertexColors_d__._cachedMeshInfo_5__5 = _AnimateVertexColors_d__._textInfo_5__2.CopyMeshInfoVertexData();
			}
			if ((vertexJitter.hasTextChanged ? 1 : 0) != num3)
			{
				_AnimateVertexColors_d__._cachedMeshInfo_5__5 = _AnimateVertexColors_d__._textInfo_5__2.CopyMeshInfoVertexData();
				vertexJitter.hasTextChanged = (byte)num3 != 0;
			}
			int characterCount = _AnimateVertexColors_d__._textInfo_5__2.characterCount;
			if (_AnimateVertexColors_d__._textInfo_5__2.characterCount != 0)
			{
				int num7 = num3;
				if (_AnimateVertexColors_d__._textInfo_5__2.characterCount > 0)
				{
					float num8 = 0.5f;
					float num9 = 0f;
					float num10 = 2f;
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
						NativeIntrinsics.CallNative("GetAt", NativeIntrinsics.Reinterpret<long>(_AnimateVertexColors_d__._textInfo_5__2.characterInfo), NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFDA8]"), num7, 0L);
						if (NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFF1C]") != num3)
						{
							_ = _AnimateVertexColors_d__._textInfo_5__2.characterInfo;
							_ = (int)NativeIntrinsics.Asm("*(LocalExpr + 0x58)");
							_ = _AnimateVertexColors_d__._textInfo_5__2;
							_ = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)");
							_ = _AnimateVertexColors_d__._textInfo_5__2.meshInfo;
							_ = ((float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)") + (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)")) * num8;
							float num11 = ((float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x24)") + (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x24)")) * num8;
							_ = (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x28)") - num9;
							NativeIntrinsics.Asm("*(LocalExpr + 0x30)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+24h],xmm1 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+28h],xmm2 ; value = LocalExpr");
							NativeIntrinsics.Asm("*(FieldExpr + 0x20)");
							int num12 = (int)NativeIntrinsics.Asm("*(FieldExpr + 0x28)");
							_ = (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x28)") - num9;
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm1 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm2 ; value = LocalExpr");
							_ = (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x28)") - num9;
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm1 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm2 ; value = LocalExpr");
							_ = (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x28)") - num9;
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm1 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm2 ; value = LocalExpr");
							float num13 = (float)_AnimateVertexColors_d__._loopCount_5__3 / num4 * (float)num12;
							float num14 = (float)Math.Floor((float)_AnimateVertexColors_d__._loopCount_5__3 / num4 * (float)num12 * num8);
							float minInclusive = num13 - (num14 + num14);
							if ((num9 > num13 - (num14 + num14)))
							{
								minInclusive = 0f;
							}
							else if (minInclusive > num10)
							{
								minInclusive = num10;
							}
							float num15 = num5 - Math.Abs(minInclusive - num5);
							if ((num9 > num5 - Math.Abs(minInclusive - num5)))
							{
								num15 = 0f;
							}
							else if (num15 > num5)
							{
								num15 = num5;
							}
							float num16 = UnityEngine.Random.Range(-0.25f, 0.25f);
							float num17 = UnityEngine.Random.Range(-0.25f, 0.25f);
							float num18 = vertexJitter.CurveScale * num9;
							float num19 = vertexJitter.CurveScale * num17;
							float num20 = vertexJitter.CurveScale * num16;
							float num21 = UnityEngine.Random.Range(-5f, 5f);
							euler.x = NativeIntrinsics.Reinterpret<float>(num3);
							euler.z = num21 * vertexJitter.AngleMultiplier * ((float)Math.PI / 180f);
							Quaternion q = Quaternion.Internal_FromEulerRad(euler);
							num10 = num20;
							minInclusive = num19;
							float z = num18;
							_ = (num5 - (num15 * -2f * num15 * num15 + num15 * 3f * num15)) * (float)NativeIntrinsics.Asm("xorps xmm0,[1813EACB0h]");
							vector = new Vector3(num10, minInclusive, z);
							Vector3 pos = vector;
							s = Vector3.one;
							Matrix4x4 matrix4x = Matrix4x4.TRS(pos, q, s);
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
							Vector3 vector2 = matrix4x2.MultiplyPoint3x4(point);
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rdx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rdx*4+28h],r8d ; value = FieldExpr");
							_ = vector2.z;
							point2.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)"));
							point2.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)"));
							matrix4x2.MultiplyPoint3x4(point2);
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rcx*4+28h],edx ; value = FieldExpr");
							point3.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)"));
							point3.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)"));
							matrix4x2.MultiplyPoint3x4(point3);
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rcx*4+28h],edx ; value = FieldExpr");
							point4.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)"));
							point4.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)"));
							matrix4x2.MultiplyPoint3x4(point4);
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rcx*4+28h],edx ; value = FieldExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+24h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+28h],xmm1 ; value = BinaryExpr");
							_ = num11 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							_ = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							_ = num11 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							_ = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							_ = num11 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							_ = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm7 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm8 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr");
							_ = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							_ = _AnimateVertexColors_d__._vertexAnim_5__4;
							NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+20h],xmm0 ; value = stack[0x8]");
							NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+24h],xmm10 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+28h],xmm0 ; value = stack[0x18]");
							NativeIntrinsics.Asm("stack[0x18]");
							num8 = 0.5f;
							num4 = 25f;
							num10 = 2f;
						}
						num7++;
					}
					while (num7 + 1 < characterCount);
				}
				TMP_TextInfo tMP_TextInfo = _AnimateVertexColors_d__._textInfo_5__2;
				int num22 = num3;
				int num23 = num3;
				while (num22 < tMP_TextInfo.meshInfo.Length)
				{
					_ = _AnimateVertexColors_d__._textInfo_5__2.meshInfo;
					NativeIntrinsics.Reinterpret<Mesh>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)")).vertices = NativeIntrinsics.Reinterpret<Vector3[]>(NativeIntrinsics.Asm("*(FieldExpr + 0x30)"));
					vertexJitter.m_TextComponent.UpdateGeometry(NativeIntrinsics.Reinterpret<Mesh>(NativeIntrinsics.Asm("*(FieldExpr + 0x20)")), num23);
					_ = _AnimateVertexColors_d__._textInfo_5__2;
					num23++;
					if (_AnimateVertexColors_d__._textInfo_5__2 == null)
					{
						break;
					}
				}
				_AnimateVertexColors_d__._loopCount_5__3++;
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

		// Token: 0x06000279 RID: 633 RVA: 0x00267D30 File Offset: 0x00266330
		void System.Collections.IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	// Token: 0x04000386 RID: 902 Offset: 0x20
	public float AngleMultiplier = 1f;

	// Token: 0x04000387 RID: 903 Offset: 0x24
	public float SpeedMultiplier = 1f;

	// Token: 0x04000388 RID: 904 Offset: 0x28
	public float CurveScale = 1f;

	// Token: 0x04000389 RID: 905 Offset: 0x30
	private TMP_Text m_TextComponent;

	// Token: 0x0400038A RID: 906 Offset: 0x38
	private bool hasTextChanged;

	// Token: 0x0600026E RID: 622 RVA: 0x00268DE0 File Offset: 0x002673E0
	private void Awake()
	{
		m_TextComponent = GetComponent<TMP_Text>();
	}

	// Token: 0x0600026F RID: 623 RVA: 0x00268F70 File Offset: 0x00267570
	private void OnEnable()
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Add(Il2CppStudio.Native.NativeIntrinsics.Reinterpret<Action<UnityEngine.Object>>("unresolved delegate target"));
	}

	// Token: 0x06000270 RID: 624 RVA: 0x00268EA0 File Offset: 0x002674A0
	private void OnDisable()
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(Il2CppStudio.Native.NativeIntrinsics.Reinterpret<Action<UnityEngine.Object>>("unresolved delegate target"));
	}

	// Token: 0x06000271 RID: 625 RVA: 0x00269040 File Offset: 0x00267640
	private void Start()
	{
		_AnimateVertexColors_d__11 _AnimateVertexColors_d__ = new _AnimateVertexColors_d__11(0);
		_AnimateVertexColors_d__.__1__state = 0;
		_AnimateVertexColors_d__.__4__this = this;
		StartCoroutine(_AnimateVertexColors_d__);
	}

	// Token: 0x06000272 RID: 626 RVA: 0x00268E30 File Offset: 0x00267430
	private void ON_TEXT_CHANGED(UnityEngine.Object obj)
	{
		if (obj == m_TextComponent)
		{
			hasTextChanged = true;
		}
	}

	// Token: 0x06000273 RID: 627 RVA: 0x00268D70 File Offset: 0x00267370
	private IEnumerator AnimateVertexColors()
	{
		_AnimateVertexColors_d__11 _AnimateVertexColors_d__ = new _AnimateVertexColors_d__11(0);
		_AnimateVertexColors_d__.__1__state = 0;
		_AnimateVertexColors_d__.__4__this = this;
		return _AnimateVertexColors_d__;
	}
}

