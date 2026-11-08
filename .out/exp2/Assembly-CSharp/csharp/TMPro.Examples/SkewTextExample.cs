using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
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
			SkewTextExample skewTextExample2 = skewTextExample;
			int num2 = num;
			int result;
			if (num != 0)
			{
				num2--;
				if (num2 != 0 && num2 != 1)
				{
					result = 0;
					goto IL_0aaa;
				}
				<>1__state = -1;
			}
			else
			{
				<>1__state = -1;
				skewTextExample2.VertexCurve.preWrapMode = WrapMode.Once;
				skewTextExample2.VertexCurve.postWrapMode = WrapMode.Once;
				skewTextExample2.m_TextComponent.havePropertiesChanged = true;
				<old_CurveScale>5__2 = (skewTextExample2.CurveScale *= 10f);
				<old_ShearValue>5__3 = skewTextExample2.ShearAmount;
				AnimationCurve vertexCurve = skewTextExample2.VertexCurve;
				AnimationCurve animationCurve = new AnimationCurve();
				animationCurve.keys = vertexCurve.keys;
				<old_curve>5__4 = animationCurve;
			}
			Vector3 vector = default(Vector3);
			Vector3 euler = default(Vector3);
			Vector3 vector2 = default(Vector3);
			Vector3 s = default(Vector3);
			Matrix4x4 matrix4x2 = default(Matrix4x4);
			Vector3 point = default(Vector3);
			Vector3 point2 = default(Vector3);
			Vector3 point3 = default(Vector3);
			Vector3 point4 = default(Vector3);
			while (true)
			{
				if (!skewTextExample2.m_TextComponent.m_havePropertiesChanged)
				{
					_ = <old_CurveScale>5__2;
					if (<old_CurveScale>5__2 == skewTextExample2.CurveScale && ((Keyframe*)(NativeIntrinsics.Reinterpret<long>(<old_curve>5__4.keys) + 60L))->value == ((Keyframe*)(NativeIntrinsics.Reinterpret<long>(skewTextExample2.VertexCurve.keys) + 60L))->value)
					{
						_ = <old_ShearValue>5__3;
						if (<old_ShearValue>5__3 == skewTextExample2.ShearAmount)
						{
							<>2__current = null;
							<>1__state = 1;
							break;
						}
					}
				}
				<old_CurveScale>5__2 = skewTextExample2.CurveScale;
				<old_curve>5__4 = skewTextExample2.CopyAnimationCurve(skewTextExample2.VertexCurve);
				<old_ShearValue>5__3 = skewTextExample2.ShearAmount;
				skewTextExample2.m_TextComponent.ForceMeshUpdate(ignoreActiveState: false, forceTextReparsing: false);
				TMP_TextInfo textInfo = skewTextExample2.m_TextComponent.m_textInfo;
				int characterCount = textInfo.characterCount;
				if (textInfo.characterCount == 0)
				{
					continue;
				}
				_ = skewTextExample2.m_TextComponent.bounds;
				NativeIntrinsics.Asm("shufps xmm1,xmm15,0FFh");
				float num3 = (float)NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)") - (float)NativeIntrinsics.Asm("shufps xmm1,xmm15,0FFh");
				_ = skewTextExample2.m_TextComponent.bounds;
				float num4 = (float)NativeIntrinsics.Asm("shufps xmm4,xmm1,0FFh") + (float)NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
				if (characterCount > 0)
				{
					int num5 = 0;
					float num6 = -0f;
					float num7 = 0f;
					do
					{
						_ = textInfo.characterInfo;
						if (NativeIntrinsics.Asm("*(LocalExpr + 0x194)") != 0)
						{
							_ = textInfo.meshInfo;
							NativeIntrinsics.Asm("*(LocalExpr + 0x30)");
							_ = (int)NativeIntrinsics.Asm("*(FieldExpr + 0x6C)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+24h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+28h],xmm0 ; value = BinaryExpr");
							_ = num6 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							float num8 = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)")) * 0.5f;
							int num9 = (int)NativeIntrinsics.Asm("*(FieldExpr + 0x14C)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr");
							_ = num6 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr");
							_ = num6 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr");
							_ = textInfo.characterInfo;
							_ = num6 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							_ = skewTextExample2.ShearAmount * 0.01f;
							_ = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x14C)") - (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x138)")) * (skewTextExample2.ShearAmount * 0.01f);
							float num10 = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x12C)") - (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x14C)")) * (skewTextExample2.ShearAmount * 0.01f);
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+24h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+28h],xmm1 ; value = BinaryExpr");
							_ = num6 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							_ = num6 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							_ = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							_ = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm3 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr");
							_ = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							_ = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							_ = num10 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm2 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr");
							_ = num6 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							_ = (float)NativeIntrinsics.Asm("xorps xmm2,xmm11") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)");
							float num11 = skewTextExample2.VertexCurve.Evaluate((num8 - num3) / (num4 - num3));
							float curveScale = skewTextExample2.CurveScale;
							float num12 = skewTextExample2.VertexCurve.Evaluate((num8 - num3) / (num4 - num3) + 0.0001f) * skewTextExample2.CurveScale - curveScale * num11;
							vector = new Vector3(((float)(int)NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFE28]") - num3) * ((num8 - num3) / (num4 - num3) + 0.0001f) + num3 - num8, num12, 0f);
							Vector3 normalized = vector.normalized;
							float num13 = (float)Math.Acos(normalized.y * num7 + normalized.x + normalized.z * num7) * 57.29578f;
							float y = curveScale * num11;
							if (!(num12 - (((float)(int)NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFE28]") - num3) * ((num8 - num3) / (num4 - num3) + 0.0001f) + num3 - num8) * num7 > num7))
							{
								num13 = 360f - num13;
							}
							euler.x = 0f;
							euler.z = num13 * ((float)Math.PI / 180f);
							Quaternion q = Quaternion.Internal_FromEulerRad(euler);
							vector2 = new Vector3(0f, y, 0f);
							Vector3 pos = vector2;
							s = new Vector3(Vector3.oneVector.x, Vector3.oneVector.y, Vector3.oneVector.z);
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
							Vector3 vector3 = matrix4x2.MultiplyPoint3x4(point);
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rdx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rdx*4+28h],r8d ; value = FieldExpr");
							_ = vector3.z;
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
							_ = (float)num9 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							_ = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							_ = (float)num9 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							_ = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							_ = (float)num9 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							_ = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm8 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm9 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr");
							_ = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFE28]");
						}
						num5++;
					}
					while (num5 + 1 < characterCount);
				}
				skewTextExample2.m_TextComponent.UpdateVertexData();
				<>2__current = null;
				<>1__state = 2;
				break;
			}
			result = 1;
			goto IL_0aaa;
			IL_0aaa:
			return (byte)result != 0;
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00263990 File Offset: 0x00261F90
		private void System.Collections.IEnumerator.Reset()
		{
			throw new NotSupportedException();
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
		m_TextComponent = gameObject.GetComponent<TMP_Text>();
	}

	// Token: 0x06000200 RID: 512 RVA: 0x00259370 File Offset: 0x00257970
	private void Start()
	{
		<WarpText>d__7 <WarpText>d__ = new <WarpText>d__7(0);
		<WarpText>d__.<>1__state = 0;
		<WarpText>d__.<>4__this = this;
		StartCoroutine(<WarpText>d__);
	}

	// Token: 0x06000201 RID: 513 RVA: 0x002592F0 File Offset: 0x002578F0
	private AnimationCurve CopyAnimationCurve(AnimationCurve curve)
	{
		AnimationCurve animationCurve = new AnimationCurve();
		animationCurve.keys = curve.keys;
		return animationCurve;
	}

	// Token: 0x06000202 RID: 514 RVA: 0x002593F0 File Offset: 0x002579F0
	private IEnumerator WarpText()
	{
		<WarpText>d__7 <WarpText>d__ = new <WarpText>d__7(0);
		<WarpText>d__.<>1__state = 0;
		<WarpText>d__.<>4__this = this;
		return <WarpText>d__;
	}

	// Token: 0x06000203 RID: 515 RVA: 0x00259460 File Offset: 0x00257A60
	public SkewTextExample()
	{
		Keyframe[] array = new Keyframe[5];
		Keyframe keyframe = default(Keyframe);
		keyframe.m_Time = 0f;
		keyframe.m_OutTangent = 0f;
		array[0] = NativeIntrinsics.Reinterpret<Keyframe>(new Keyframe(0f, 0f).m_Time);
		NativeIntrinsics.Stmt("movups [rdi+2Ch],xmm1 ; value = FieldExpr");
		Keyframe keyframe2 = default(Keyframe);
		keyframe2.m_Time = 0f;
		keyframe2.m_OutTangent = 0f;
		array[1] = NativeIntrinsics.Reinterpret<Keyframe>(new Keyframe(0.25f, 2f).m_Time);
		NativeIntrinsics.Stmt("movups [rdi+48h],xmm1 ; value = FieldExpr");
		Keyframe keyframe3 = default(Keyframe);
		keyframe3.m_Time = 0f;
		keyframe3.m_OutTangent = 0f;
		array[2] = NativeIntrinsics.Reinterpret<Keyframe>(new Keyframe(0.5f, 0f).m_Time);
		NativeIntrinsics.Stmt("movups [rdi+64h],xmm1 ; value = FieldExpr");
		Keyframe keyframe4 = default(Keyframe);
		keyframe4.m_Time = 0f;
		keyframe4.m_OutTangent = 0f;
		array[3] = NativeIntrinsics.Reinterpret<Keyframe>(new Keyframe(0.75f, 2f).m_Time);
		NativeIntrinsics.Stmt("movups [rdi+80h],xmm1 ; value = FieldExpr");
		Keyframe keyframe5 = default(Keyframe);
		keyframe5.m_Time = 0f;
		keyframe5.m_OutTangent = 0f;
		array[4] = NativeIntrinsics.Reinterpret<Keyframe>(new Keyframe(1f, 0f).m_Time);
		NativeIntrinsics.Stmt("movups [rdi+9Ch],xmm1 ; value = FieldExpr");
		VertexCurve = new AnimationCurve(array);
		CurveScale = 1f;
		ShearAmount = 1f;
		base..ctor();
	}
}

