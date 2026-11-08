using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using Mono.Xml;
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
	private sealed class <AnimateVertexColors>d__11 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x0400038E RID: 910 Offset: 0x10
		private int <>1__state;

		// Token: 0x0400038F RID: 911 Offset: 0x18
		private object <>2__current;

		// Token: 0x04000390 RID: 912 Offset: 0x20
		public VertexJitter <>4__this;

		// Token: 0x04000391 RID: 913 Offset: 0x28
		private TMP_TextInfo <textInfo>5__2;

		// Token: 0x04000392 RID: 914 Offset: 0x30
		private int <loopCount>5__3;

		// Token: 0x04000393 RID: 915 Offset: 0x38
		private VertexAnim[] <vertexAnim>5__4;

		// Token: 0x04000394 RID: 916 Offset: 0x40
		private TMP_MeshInfo[] <cachedMeshInfo>5__5;

		private object System.Collections.Generic.IEnumerator<System.Object>.Current => <>2__current;

		private object System.Collections.IEnumerator.Current => <>2__current;

		// Token: 0x06000275 RID: 629 RVA: 0x00250B50 File Offset: 0x0024F150
		public <AnimateVertexColors>d__11(int <>1__state)
		{
			((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
			this.<>1__state = <>1__state;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00250B40 File Offset: 0x0024F140
		private void System.IDisposable.Dispose()
		{
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00267130 File Offset: 0x00265730
		private unsafe bool MoveNext()
		{
			int num = <>1__state;
			VertexJitter vertexJitter = <>4__this;
			NativeIntrinsics.Stmt("movups [rbp-60h],xmm0 ; value = FloatExpr");
			NativeIntrinsics.Stmt("movups [rbp-50h],xmm0 ; value = FloatExpr");
			NativeIntrinsics.Stmt("movups [rbp-40h],xmm0 ; value = FloatExpr");
			NativeIntrinsics.Stmt("movups [rbp-30h],xmm0 ; value = FloatExpr");
			VertexJitter vertexJitter2 = vertexJitter;
			long num2 = NativeIntrinsics.Asm("&stack[0x0]") + -728L;
			int num3 = num;
			if (num != 0)
			{
				num3--;
				if (num3 - 1 != 0 && num3 != 1)
				{
					goto IL_0aca;
				}
				<>1__state = -1;
			}
			else
			{
				<>1__state = -1;
				TMP_Text textComponent = vertexJitter2.m_TextComponent;
				TMP_Text textComponent2 = vertexJitter2.m_TextComponent;
				TMP_TextInfo textInfo = textComponent2.m_textInfo;
				<textInfo>5__2 = textInfo;
				<loopCount>5__3 = 0;
				vertexJitter2.hasTextChanged = true;
				VertexAnim[] array = new VertexAnim[1024];
				<vertexAnim>5__4 = array;
				do
				{
					VertexAnim[] array2 = <vertexAnim>5__4;
					float num4 = UnityEngine.Random.Range(10f, 25f);
					float num5 = num4;
					NativeIntrinsics.Stmt("movss dword ptr [rdi+rcx*4+20h],xmm0 ; value = LocalExpr");
					long num6 = 0 + 0 * 2L;
					VertexAnim[] array3 = <vertexAnim>5__4;
					float num7 = UnityEngine.Random.Range(1f, 3f);
					num5 = num7;
					NativeIntrinsics.Stmt("movss dword ptr [rdi+rcx*4+28h],xmm0 ; value = LocalExpr");
					num6 = 0 + 0 * 2L;
					int num8 = 0 + 1;
				}
				while (0 + 1 < 1024);
				TMP_MeshInfo[] array4 = <textInfo>5__2.CopyMeshInfoVertexData();
				<cachedMeshInfo>5__5 = array4;
			}
			if (vertexJitter2.hasTextChanged)
			{
				TMP_MeshInfo[] array5 = <textInfo>5__2.CopyMeshInfoVertexData();
				<cachedMeshInfo>5__5 = array5;
				vertexJitter2.hasTextChanged = false;
			}
			int characterCount = <textInfo>5__2.characterCount;
			if (<textInfo>5__2.characterCount != 0)
			{
				if (<textInfo>5__2.characterCount > 0)
				{
					Vector3 euler = default(Vector3);
					Vector3 vector = default(Vector3);
					Vector3 s = default(Vector3);
					Vector3 point = default(Vector3);
					Vector3 point2 = default(Vector3);
					do
					{
						long num9 = NativeIntrinsics.CallNative("sub_32D0", NativeIntrinsics.Reinterpret<long>(<textInfo>5__2.characterInfo), num2 + 128L, 0L, 0L);
						if (NativeIntrinsics.Asm("*(LocalExpr + 0x1F4)") != 0)
						{
							TMP_TextInfo tMP_TextInfo = <textInfo>5__2;
							TMP_CharacterInfo[] characterInfo = tMP_TextInfo.characterInfo;
							long num6 = 0 * 376L;
							long num10 = (int)NativeIntrinsics.Asm("*(LocalExpr + 0x58)");
							num6 = num10 + num10 * 4L + (num10 + num10 * 4L);
							num6 = 0 * 376L;
							TMP_TextInfo tMP_TextInfo2 = <textInfo>5__2;
							long num11 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)");
							num6 = num11 + 2L + (num11 + 2L) * 2L;
							long arg = num11 + num11 * 2L;
							TMP_MeshInfo[] meshInfo = <textInfo>5__2.meshInfo;
							float num12 = ((float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)") + (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)")) * 0.5f;
							float num13 = ((float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x24)") + (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x24)")) * 0.5f;
							num6 = num11 + num11 * 2L;
							float num14 = (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x28)") - 0f;
							long num15 = NativeIntrinsics.Asm("*(LocalExpr + 0x30)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+24h],xmm1 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+28h],xmm2 ; value = LocalExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbp+2E0h],xmm12 ; value = *(FieldExpr + 0x20)");
							NativeIntrinsics.Stmt("movss dword ptr [rbp+2F0h],xmm3 ; value = *(FieldExpr + 0x28)");
							int num16 = (int)NativeIntrinsics.Asm("*(FieldExpr + 0x20)");
							num6 = num11 + num11 * 2L;
							int num17 = (int)NativeIntrinsics.Asm("*(FieldExpr + 0x28)");
							num6 = num11 + 1L + (num11 + 1L) * 2L;
							num14 = (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x28)") - 0f;
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm1 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm2 ; value = LocalExpr");
							num6 = num11 + 2L + (num11 + 2L) * 2L;
							num14 = (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x28)") - 0f;
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm1 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm2 ; value = LocalExpr");
							num6 = num11 + 3L + (num11 + 3L) * 2L;
							num14 = (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x28)") - 0f;
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm1 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm2 ; value = LocalExpr");
							float num18 = (float)<loopCount>5__3 / 25f * (float)num17;
							long num19 = NativeIntrinsics.CallNative("sub_20C2B0", num6, arg, NativeIntrinsics.Asm("*(FieldExpr + 0x30)"), num10);
							float num20 = num18 - ((float)NativeIntrinsics.Asm("xmm0") + (float)NativeIntrinsics.Asm("xmm0"));
							if (0f > num18 - ((float)NativeIntrinsics.Asm("xmm0") + (float)NativeIntrinsics.Asm("xmm0")) || num20 > 2f)
							{
							}
							float num21 = 1f - (float)NativeIntrinsics.Asm("andps xmm6,[1813EAC20h]");
							if (0f > 1f - (float)NativeIntrinsics.Asm("andps xmm6,[1813EAC20h]") || num21 > 1f)
							{
							}
							float num22 = UnityEngine.Random.Range(-0.25f, 0.25f);
							float num23 = UnityEngine.Random.Range(-0.25f, 0.25f);
							float num24 = vertexJitter2.CurveScale * 0f;
							float num25 = vertexJitter2.CurveScale * num23;
							float num26 = vertexJitter2.CurveScale * num22;
							float num27 = UnityEngine.Random.Range(-5f, 5f);
							euler.x = 0f;
							euler.z = num27 * vertexJitter2.AngleMultiplier * (MathF.PI / 180f);
							Quaternion quaternion = Quaternion.Internal_FromEulerRad(euler);
							float x = num26;
							num20 = num25;
							float z = num24;
							long num28 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
							float num29 = (1f - (0f * -2f * 0f * 0f + 0f * 3f * 0f)) * (float)NativeIntrinsics.Asm("xorps xmm0,[1813EACB0h]");
							float num30 = (0f * -2f * 0f * 0f + 0f * 3f * 0f) * (float)num16;
							if (NativeIntrinsics.Asm("global_19E3BFF") == 0)
							{
								NativeIntrinsics.Stmt("mov byte ptr [1819E3BFFh],1 ; value = 0x1");
							}
							NativeIntrinsics.Stmt("movdqa [rbp-20h],xmm14 ; value = LocalExpr");
							vector.x = x;
							vector.y = num20;
							vector.z = z;
							Vector3 pos = vector;
							Quaternion q = NativeIntrinsics.Reinterpret<Quaternion>(num2 + -32L);
							s.x = Vector3.oneVector.x;
							s.y = Vector3.oneVector.y;
							s.z = Vector3.oneVector.z;
							Matrix4x4 matrix4x = Matrix4x4.TRS(pos, q, s);
							NativeIntrinsics.Stmt("movaps [rbp-60h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("movaps [rbp-50h],xmm1 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("movaps [rbp-40h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("movaps [rbp-30h],xmm1 ; value = StructChunkExpr");
							long num31 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x20, 16)");
							long num32 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x30, 16)");
							long num33 = num2 + -96L;
							point.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)"));
							point.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)"));
							Vector3 vector2 = ((Matrix4x4*)num33)->MultiplyPoint3x4(point);
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rdx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rdx*4+28h],r8d ; value = FieldExpr");
							float z2 = vector2.z;
							num31 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
							long num34 = num2 + -96L;
							point2.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)"));
							point2.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)"));
							Vector3 vector3 = ((Matrix4x4*)num34)->MultiplyPoint3x4(point2);
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rcx*4+28h],edx ; value = FieldExpr");
							num31 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
							NativeIntrinsics.Stmt("movsd qword ptr [rbp-80h],xmm0 ; value = *(LocalExpr + 0x20)");
							NativeIntrinsics.Stmt("mov [rbp-78h],eax ; value = *(LocalExpr + 0x28)");
							Vector3 vector4 = ((Matrix4x4*)(num2 + -96L))->MultiplyPoint3x4(NativeIntrinsics.Reinterpret<Vector3>(num2 + -128L));
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rcx*4+28h],edx ; value = FieldExpr");
							num31 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
							NativeIntrinsics.Stmt("movsd qword ptr [rbp-70h],xmm0 ; value = *(LocalExpr + 0x20)");
							NativeIntrinsics.Stmt("mov [rbp-68h],eax ; value = *(LocalExpr + 0x28)");
							Vector3 vector5 = ((Matrix4x4*)(num2 + -96L))->MultiplyPoint3x4(NativeIntrinsics.Reinterpret<Vector3>(num2 + -112L));
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rcx*4+28h],edx ; value = FieldExpr");
							num31 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+24h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+28h],xmm1 ; value = BinaryExpr");
							float num35 = num13 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							float num36 = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							num35 = num13 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							num36 = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							num35 = num13 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							num36 = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm7 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm8 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr");
							num35 = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							VertexAnim[] array6 = <vertexAnim>5__4;
							NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+20h],xmm0 ; value = *(LocalExpr + 0x2E0)");
							NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+24h],xmm10 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*4+28h],xmm0 ; value = *(LocalExpr + 0x2F0)");
							int num37 = (int)NativeIntrinsics.Asm("*(LocalExpr + 0x2F0)");
						}
						int num38 = 0 + 1;
					}
					while (0 + 1 < characterCount);
				}
				TMP_TextInfo tMP_TextInfo3 = <textInfo>5__2;
				while (0 < tMP_TextInfo3.meshInfo.Length)
				{
					TMP_TextInfo tMP_TextInfo4 = <textInfo>5__2;
					TMP_MeshInfo[] meshInfo2 = tMP_TextInfo4.meshInfo;
					long num6 = NativeIntrinsics.Asm("*(LocalExpr + 0x20)");
					long num39 = 0L;
					NativeIntrinsics.Reinterpret<Mesh>(num6).vertices = NativeIntrinsics.Reinterpret<Vector3[]>(NativeIntrinsics.Asm("*(FieldExpr + 0x30)"));
					TMP_Text textComponent3 = vertexJitter2.m_TextComponent;
					int num40 = 0 + 1;
					TMP_TextInfo tMP_TextInfo5 = <textInfo>5__2;
					int num41 = 0 + 1;
					if (<textInfo>5__2 == null)
					{
						break;
					}
				}
				<loopCount>5__3++;
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
			goto IL_0aca;
			IL_0aca:
			return true;
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00267D30 File Offset: 0x00266330
		private void System.Collections.IEnumerator.Reset()
		{
			long num = NativeIntrinsics.CallNative("sub_184BE0", NativeIntrinsics.Asm("rax"), 0L, 0L, 0L);
			NativeIntrinsics.Reinterpret<NotSupportedException>(num)..ctor();
			throw null;
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
		TMP_Text component = GetComponent<TMP_Text>();
		m_TextComponent = component;
	}

	// Token: 0x0600026F RID: 623 RVA: 0x00268F70 File Offset: 0x00267570
	private void OnEnable()
	{
		FastAction<UnityEngine.Object> tEXT_CHANGED_EVENT = TMPro_EventManager.TEXT_CHANGED_EVENT;
		Action<UnityEngine.Object> rhs = (Action<UnityEngine.Object>)(object)new System.IOAsyncCallback(ON_TEXT_CHANGED);
		tEXT_CHANGED_EVENT.Add(rhs);
	}

	// Token: 0x06000270 RID: 624 RVA: 0x00268EA0 File Offset: 0x002674A0
	private void OnDisable()
	{
		FastAction<UnityEngine.Object> tEXT_CHANGED_EVENT = TMPro_EventManager.TEXT_CHANGED_EVENT;
		Action<UnityEngine.Object> rhs = (Action<UnityEngine.Object>)(object)new System.IOAsyncCallback(ON_TEXT_CHANGED);
		tEXT_CHANGED_EVENT.Remove(rhs);
	}

	// Token: 0x06000271 RID: 625 RVA: 0x00269040 File Offset: 0x00267640
	private void Start()
	{
		<AnimateVertexColors>d__11 <AnimateVertexColors>d__ = NativeIntrinsics.Reinterpret<<AnimateVertexColors>d__11>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.VertexJitter+<AnimateVertexColors>d__11)"));
		((Mono.Xml.SecurityParser)(object)<AnimateVertexColors>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<AnimateVertexColors>d__.<>1__state = 0;
		<AnimateVertexColors>d__.<>4__this = this;
		Coroutine coroutine = StartCoroutine(<AnimateVertexColors>d__);
	}

	// Token: 0x06000272 RID: 626 RVA: 0x00268E30 File Offset: 0x00267430
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

	// Token: 0x06000273 RID: 627 RVA: 0x00268D70 File Offset: 0x00267370
	private IEnumerator AnimateVertexColors()
	{
		<AnimateVertexColors>d__11 <AnimateVertexColors>d__ = NativeIntrinsics.Reinterpret<<AnimateVertexColors>d__11>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.VertexJitter+<AnimateVertexColors>d__11)"));
		((Mono.Xml.SecurityParser)(object)<AnimateVertexColors>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<AnimateVertexColors>d__.<>1__state = 0;
		<AnimateVertexColors>d__.<>4__this = this;
		return <AnimateVertexColors>d__;
	}
}

