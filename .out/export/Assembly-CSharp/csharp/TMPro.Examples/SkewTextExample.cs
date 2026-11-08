using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using Mono.Xml;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x02000072 RID: 114
public class SkewTextExample : MonoBehaviour
{
	// Token: 0x02000073 RID: 115
	private sealed class <WarpText>d__7 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x040002F2 RID: 754 Offset: 0x10
		private int <>1__state;

		// Token: 0x040002F3 RID: 755 Offset: 0x18
		private object <>2__current;

		// Token: 0x040002F4 RID: 756 Offset: 0x20
		public SkewTextExample <>4__this;

		// Token: 0x040002F5 RID: 757 Offset: 0x28
		private float <old_CurveScale>5__2;

		// Token: 0x040002F6 RID: 758 Offset: 0x2C
		private float <old_ShearValue>5__3;

		// Token: 0x040002F7 RID: 759 Offset: 0x30
		private AnimationCurve <old_curve>5__4;

		private object System.Collections.Generic.IEnumerator<System.Object>.Current => <>2__current;

		private object System.Collections.IEnumerator.Current => <>2__current;

		// Token: 0x06000204 RID: 516 RVA: 0x00250B50 File Offset: 0x0024F150
		public <WarpText>d__7(int <>1__state)
		{
			((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
			this.<>1__state = <>1__state;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00250B40 File Offset: 0x0024F140
		private void System.IDisposable.Dispose()
		{
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00262E00 File Offset: 0x00261400
		private unsafe bool MoveNext()
		{
			SkewTextExample skewTextExample = <>4__this;
			int num = <>1__state;
			NativeIntrinsics.Stmt("movups [rbp-20h],xmm0 ; value = FloatExpr");
			NativeIntrinsics.Stmt("movups [rbp-10h],xmm0 ; value = FloatExpr");
			NativeIntrinsics.Stmt("movups [rbp],xmm0 ; value = FloatExpr");
			NativeIntrinsics.Stmt("movups [rbp+10h],xmm0 ; value = FloatExpr");
			SkewTextExample skewTextExample2 = skewTextExample;
			long num2 = NativeIntrinsics.Asm("&stack[0x0]") + -424L;
			int num3 = num;
			if (num != 0)
			{
				num3--;
				if (num3 - 1 != 0 && num3 != 1)
				{
					goto IL_0a67;
				}
				<>1__state = -1;
			}
			else
			{
				<>1__state = -1;
				skewTextExample2.VertexCurve.preWrapMode = NativeIntrinsics.Reinterpret<WrapMode>(1);
				skewTextExample2.VertexCurve.postWrapMode = NativeIntrinsics.Reinterpret<WrapMode>(1);
				skewTextExample2.m_TextComponent.havePropertiesChanged = true;
				<old_CurveScale>5__2 = (skewTextExample2.CurveScale *= 10f);
				<old_ShearValue>5__3 = skewTextExample2.ShearAmount;
				AnimationCurve vertexCurve = skewTextExample2.VertexCurve;
				AnimationCurve animationCurve = new AnimationCurve();
				Keyframe[] keys = vertexCurve.keys;
				animationCurve.keys = keys;
				<old_curve>5__4 = animationCurve;
			}
			Vector3 vector = default(Vector3);
			Vector3 euler = default(Vector3);
			Vector3 vector2 = default(Vector3);
			Vector3 s = default(Vector3);
			Vector3 point = default(Vector3);
			while (true)
			{
				if (!skewTextExample2.m_TextComponent.m_havePropertiesChanged)
				{
					float num4 = <old_CurveScale>5__2;
					if (<old_CurveScale>5__2 == skewTextExample2.CurveScale)
					{
						Keyframe[] keys2 = <old_curve>5__4.keys;
						float value = ((Keyframe*)(NativeIntrinsics.Reinterpret<long>(keys2) + 60L))->value;
						num4 = value;
						Keyframe[] keys3 = skewTextExample2.VertexCurve.keys;
						float value2 = ((Keyframe*)(NativeIntrinsics.Reinterpret<long>(keys3) + 60L))->value;
						num4 = value2;
						if (value == value2)
						{
							num4 = <old_ShearValue>5__3;
							if (<old_ShearValue>5__3 == skewTextExample2.ShearAmount)
							{
								<>2__current = null;
								<>1__state = 1;
								break;
							}
						}
					}
				}
				<old_CurveScale>5__2 = skewTextExample2.CurveScale;
				AnimationCurve animationCurve2 = skewTextExample2.CopyAnimationCurve(skewTextExample2.VertexCurve);
				<old_curve>5__4 = animationCurve2;
				<old_ShearValue>5__3 = skewTextExample2.ShearAmount;
				TMP_Text textComponent = skewTextExample2.m_TextComponent;
				TMP_TextInfo textInfo = textComponent.m_textInfo;
				int characterCount = textInfo.characterCount;
				if (textInfo.characterCount == 0)
				{
					continue;
				}
				Bounds bounds = skewTextExample2.m_TextComponent.bounds;
				NativeIntrinsics.Stmt("movsd qword ptr [rbp-40h],xmm0 ; value = StructChunkExpr");
				long num5 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x10, 8)");
				long num6 = NativeIntrinsics.Asm("shufps xmm1,xmm15,0FFh");
				float num7 = (float)NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)") - (float)NativeIntrinsics.Asm("shufps xmm1,xmm15,0FFh");
				Bounds bounds2 = skewTextExample2.m_TextComponent.bounds;
				NativeIntrinsics.Stmt("movsd qword ptr [rbp-40h],xmm0 ; value = StructChunkExpr");
				NativeIntrinsics.Stmt("movaps [rbp-30h],xmm4 ; value = BinaryExpr");
				num5 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x10, 8)");
				num6 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
				float num8 = (float)NativeIntrinsics.Asm("shufps xmm4,xmm1,0FFh") + (float)NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
				if (characterCount > 0)
				{
					do
					{
						TMP_CharacterInfo[] characterInfo = textInfo.characterInfo;
						if (NativeIntrinsics.Asm("*(LocalExpr + 0x194)") != 0)
						{
							TMP_MeshInfo[] meshInfo = textInfo.meshInfo;
							long num9 = NativeIntrinsics.Asm("*(LocalExpr + 0x30)");
							long num10 = (int)NativeIntrinsics.Asm("*(FieldExpr + 0x6C)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+24h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+28h],xmm0 ; value = BinaryExpr");
							float num11 = -0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							float num12 = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)")) * 0.5f;
							int num13 = (int)NativeIntrinsics.Asm("*(FieldExpr + 0x14C)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr");
							num11 = -0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr");
							num11 = -0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr");
							TMP_CharacterInfo[] characterInfo2 = textInfo.characterInfo;
							num11 = -0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							num11 = skewTextExample2.ShearAmount * 0.01f;
							float num14 = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x14C)") - (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x138)")) * (skewTextExample2.ShearAmount * 0.01f);
							float num15 = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x12C)") - (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x14C)")) * (skewTextExample2.ShearAmount * 0.01f);
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+24h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+28h],xmm1 ; value = BinaryExpr");
							float num16 = -0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							num11 = -0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							num16 = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							num11 = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm3 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr");
							num16 = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							num11 = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							num15 += (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm2 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr");
							num16 = -0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							num11 = num8 - num7;
							num14 = (float)NativeIntrinsics.Asm("xorps xmm2,xmm11") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)");
							float num17 = skewTextExample2.VertexCurve.Evaluate((num12 - num7) / (num8 - num7));
							num11 = num17;
							float curveScale = skewTextExample2.CurveScale;
							float num18 = skewTextExample2.VertexCurve.Evaluate((num12 - num7) / (num8 - num7) + 0.0001f);
							float num19 = num18 * skewTextExample2.CurveScale - curveScale * num17;
							vector.x = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0xFFFFFFFFFFFFFFD0)") - num7) * ((num12 - num7) / (num8 - num7) + 0.0001f) + num7 - num12;
							vector.y = num19;
							vector.z = 0f;
							Vector3 normalized = vector.normalized;
							long num20 = NativeIntrinsics.CallNative("sub_20C3F0", 0L, 0L, 0L, 0L);
							num16 = (float)NativeIntrinsics.Asm("xmm0") * 57.29578f;
							float y = curveScale * num17;
							if (!(num19 - (((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0xFFFFFFFFFFFFFFD0)") - num7) * ((num12 - num7) / (num8 - num7) + 0.0001f) + num7 - num12) * 0f > 0f))
							{
								num16 = 360f - num16;
								num11 = 360f - num16;
							}
							euler.x = 0f;
							euler.z = num16 * (MathF.PI / 180f);
							Quaternion quaternion = Quaternion.Internal_FromEulerRad(euler);
							NativeIntrinsics.Stmt("movdqa [rbp+20h],xmm6 ; value = StructChunkExpr");
							vector2.x = 0f;
							vector2.y = y;
							vector2.z = 0f;
							Vector3 pos = vector2;
							Quaternion q = NativeIntrinsics.Reinterpret<Quaternion>(num2 + 32L);
							s.x = Vector3.oneVector.x;
							s.y = Vector3.oneVector.y;
							s.z = Vector3.oneVector.z;
							Matrix4x4 matrix4x = Matrix4x4.TRS(pos, q, s);
							NativeIntrinsics.Stmt("movaps [rbp-20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("movaps [rbp-10h],xmm1 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("movaps [rbp],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("movaps [rbp+10h],xmm1 ; value = StructChunkExpr");
							long num21 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x20, 16)");
							long num22 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x30, 16)");
							long num23 = num2 + -32L;
							point.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)"));
							point.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)"));
							Vector3 vector3 = ((Matrix4x4*)num23)->MultiplyPoint3x4(point);
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rdx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rdx*4+28h],r8d ; value = FieldExpr");
							float z = vector3.z;
							num21 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
							NativeIntrinsics.Stmt("movsd qword ptr [rbp-80h],xmm0 ; value = *(LocalExpr + 0x20)");
							NativeIntrinsics.Stmt("mov [rbp-78h],eax ; value = *(LocalExpr + 0x28)");
							Vector3 vector4 = ((Matrix4x4*)(num2 + -32L))->MultiplyPoint3x4(NativeIntrinsics.Reinterpret<Vector3>(num2 + -128L));
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rcx*4+28h],edx ; value = FieldExpr");
							num21 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
							NativeIntrinsics.Stmt("movsd qword ptr [rbp-70h],xmm0 ; value = *(LocalExpr + 0x20)");
							NativeIntrinsics.Stmt("mov [rbp-68h],eax ; value = *(LocalExpr + 0x28)");
							Vector3 vector5 = ((Matrix4x4*)(num2 + -32L))->MultiplyPoint3x4(NativeIntrinsics.Reinterpret<Vector3>(num2 + -112L));
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rcx*4+28h],edx ; value = FieldExpr");
							num21 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
							NativeIntrinsics.Stmt("movsd qword ptr [rbp-60h],xmm0 ; value = *(LocalExpr + 0x20)");
							NativeIntrinsics.Stmt("mov [rbp-58h],eax ; value = *(LocalExpr + 0x28)");
							Vector3 vector6 = ((Matrix4x4*)(num2 + -32L))->MultiplyPoint3x4(NativeIntrinsics.Reinterpret<Vector3>(num2 + -96L));
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rcx*4+28h],edx ; value = FieldExpr");
							num21 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+24h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+28h],xmm1 ; value = BinaryExpr");
							float num24 = (float)num13 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							float num25 = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							num24 = (float)num13 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							num25 = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							num24 = (float)num13 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							num25 = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm8 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm9 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr");
							num24 = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							long num26 = NativeIntrinsics.Asm("*(LocalExpr + 0xFFFFFFFFFFFFFFD0)");
						}
						int num27 = 0 + 1;
					}
					while (0 + 1 < characterCount);
				}
				<>2__current = null;
				<>1__state = 2;
				break;
			}
			goto IL_0a67;
			IL_0a67:
			return true;
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00263990 File Offset: 0x00261F90
		private void System.Collections.IEnumerator.Reset()
		{
			long num = NativeIntrinsics.CallNative("sub_184BE0", NativeIntrinsics.Asm("rax"), 0L, 0L, 0L);
			NativeIntrinsics.Reinterpret<NotSupportedException>(num)..ctor();
			throw null;
		}
	}

	// Token: 0x040002EE RID: 750 Offset: 0x20
	private TMP_Text m_TextComponent;

	// Token: 0x040002EF RID: 751 Offset: 0x28
	public AnimationCurve VertexCurve;

	// Token: 0x040002F0 RID: 752 Offset: 0x30
	public float CurveScale;

	// Token: 0x040002F1 RID: 753 Offset: 0x34
	public float ShearAmount;

	// Token: 0x060001FF RID: 511 RVA: 0x00259290 File Offset: 0x00257890
	private void Awake()
	{
		GameObject gameObject = this.gameObject;
		TMP_Text component = gameObject.GetComponent<TMP_Text>();
		m_TextComponent = component;
	}

	// Token: 0x06000200 RID: 512 RVA: 0x00259370 File Offset: 0x00257970
	private void Start()
	{
		<WarpText>d__7 <WarpText>d__ = NativeIntrinsics.Reinterpret<<WarpText>d__7>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.SkewTextExample+<WarpText>d__7)"));
		((Mono.Xml.SecurityParser)(object)<WarpText>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<WarpText>d__.<>1__state = 0;
		<WarpText>d__.<>4__this = this;
		Coroutine coroutine = StartCoroutine(<WarpText>d__);
	}

	// Token: 0x06000201 RID: 513 RVA: 0x002592F0 File Offset: 0x002578F0
	private AnimationCurve CopyAnimationCurve(AnimationCurve curve)
	{
		AnimationCurve animationCurve = new AnimationCurve();
		Keyframe[] keys = curve.keys;
		animationCurve.keys = keys;
		return animationCurve;
	}

	// Token: 0x06000202 RID: 514 RVA: 0x002593F0 File Offset: 0x002579F0
	private IEnumerator WarpText()
	{
		<WarpText>d__7 <WarpText>d__ = NativeIntrinsics.Reinterpret<<WarpText>d__7>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.SkewTextExample+<WarpText>d__7)"));
		((Mono.Xml.SecurityParser)(object)<WarpText>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<WarpText>d__.<>1__state = 0;
		<WarpText>d__.<>4__this = this;
		return <WarpText>d__;
	}

	// Token: 0x06000203 RID: 515 RVA: 0x00259460 File Offset: 0x00257A60
	public unsafe SkewTextExample()
	{
		Keyframe[] array = new Keyframe[5];
		*(Keyframe*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -73L) = new Keyframe(0f, 0f);
		array[0] = NativeIntrinsics.Reinterpret<Keyframe>(0f);
		NativeIntrinsics.Stmt("movups [rdi+2Ch],xmm1 ; value = FloatExpr");
		*(Keyframe*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -41L) = new Keyframe(0.25f, 2f);
		array[1] = NativeIntrinsics.Reinterpret<Keyframe>(0f);
		NativeIntrinsics.Stmt("movups [rdi+48h],xmm1 ; value = FloatExpr");
		*(Keyframe*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -9L) = new Keyframe(0.5f, 0f);
		array[2] = NativeIntrinsics.Reinterpret<Keyframe>(0f);
		NativeIntrinsics.Stmt("movups [rdi+64h],xmm1 ; value = FloatExpr");
		*(Keyframe*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + 23L) = new Keyframe(0.75f, 2f);
		array[3] = NativeIntrinsics.Reinterpret<Keyframe>(0f);
		NativeIntrinsics.Stmt("movups [rdi+80h],xmm1 ; value = FloatExpr");
		*(Keyframe*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + 55L) = new Keyframe(1f, 0f);
		array[4] = NativeIntrinsics.Reinterpret<Keyframe>(0f);
		NativeIntrinsics.Stmt("movups [rdi+9Ch],xmm1 ; value = FloatExpr");
		VertexCurve = new AnimationCurve(array);
		CurveScale = 1f;
		ShearAmount = 1f;
		((Behaviour)this)..ctor();
	}
}

