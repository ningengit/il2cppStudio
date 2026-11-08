using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x02000095 RID: 149
public class WarpTextExample : MonoBehaviour
{
	// Token: 0x02000096 RID: 150
	private sealed class <WarpText>d__8 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x040003BD RID: 957 Offset: 0x10
		private int <>1__state;

		// Token: 0x040003BE RID: 958 Offset: 0x18
		private object <>2__current;

		// Token: 0x040003BF RID: 959 Offset: 0x20
		public WarpTextExample <>4__this;

		// Token: 0x040003C0 RID: 960 Offset: 0x28
		private float <old_CurveScale>5__2;

		// Token: 0x040003C1 RID: 961 Offset: 0x30
		private AnimationCurve <old_curve>5__3;

		private object System.Collections.Generic.IEnumerator<System.Object>.Current => <>2__current;

		private object System.Collections.IEnumerator.Current => <>2__current;

		// Token: 0x060002A9 RID: 681 RVA: 0x00250B50 File Offset: 0x0024F150
		public <WarpText>d__8(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00250B40 File Offset: 0x0024F140
		private void System.IDisposable.Dispose()
		{
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00268160 File Offset: 0x00266760
		private unsafe bool MoveNext()
		{
			int num = <>1__state;
			WarpTextExample warpTextExample = <>4__this;
			int num2 = num;
			int result;
			if (num != 0)
			{
				num2--;
				if (num2 != 0 && num2 != 1)
				{
					result = 0;
					goto IL_08ed;
				}
				<>1__state = -1;
			}
			else
			{
				<>1__state = -1;
				warpTextExample.VertexCurve.preWrapMode = WrapMode.Once;
				warpTextExample.VertexCurve.postWrapMode = WrapMode.Once;
				warpTextExample.m_TextComponent.havePropertiesChanged = true;
				<old_CurveScale>5__2 = (warpTextExample.CurveScale *= 10f);
				AnimationCurve vertexCurve = warpTextExample.VertexCurve;
				AnimationCurve animationCurve = new AnimationCurve();
				animationCurve.keys = vertexCurve.keys;
				<old_curve>5__3 = animationCurve;
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
				if (!warpTextExample.m_TextComponent.m_havePropertiesChanged)
				{
					_ = <old_CurveScale>5__2;
					if (<old_CurveScale>5__2 == warpTextExample.CurveScale && ((Keyframe*)(NativeIntrinsics.Reinterpret<long>(<old_curve>5__3.keys) + 60L))->value == ((Keyframe*)(NativeIntrinsics.Reinterpret<long>(warpTextExample.VertexCurve.keys) + 60L))->value)
					{
						<>2__current = null;
						<>1__state = 1;
						break;
					}
				}
				<old_CurveScale>5__2 = warpTextExample.CurveScale;
				<old_curve>5__3 = warpTextExample.CopyAnimationCurve(warpTextExample.VertexCurve);
				warpTextExample.m_TextComponent.ForceMeshUpdate(ignoreActiveState: false, forceTextReparsing: false);
				TMP_TextInfo textInfo = warpTextExample.m_TextComponent.m_textInfo;
				int characterCount = textInfo.characterCount;
				if (textInfo.characterCount == 0)
				{
					continue;
				}
				_ = warpTextExample.m_TextComponent.bounds;
				NativeIntrinsics.Asm("shufps xmm1,xmm15,0FFh");
				float num3 = (float)NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)") - (float)NativeIntrinsics.Asm("shufps xmm1,xmm15,0FFh");
				_ = warpTextExample.m_TextComponent.bounds;
				float num4 = (float)NativeIntrinsics.Asm("shufps xmm2,xmm1,0FFh") + (float)NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
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
							float num10 = warpTextExample.VertexCurve.Evaluate((num8 - num3) / (num4 - num3));
							float curveScale = warpTextExample.CurveScale;
							float num11 = warpTextExample.VertexCurve.Evaluate((num8 - num3) / (num4 - num3) + 0.0001f) * warpTextExample.CurveScale - curveScale * num10;
							vector = new Vector3(((float)(int)NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFE48]") - num3) * ((num8 - num3) / (num4 - num3) + 0.0001f) + num3 - num8, num11, 0f);
							Vector3 normalized = vector.normalized;
							float num12 = (float)Math.Acos(normalized.y * num7 + normalized.x + normalized.z * num7) * 57.29578f;
							float y = curveScale * num10;
							if (!(num11 - (((float)(int)NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFE48]") - num3) * ((num8 - num3) / (num4 - num3) + 0.0001f) + num3 - num8) * num7 > num7))
							{
								num12 = 360f - num12;
							}
							euler.x = 0f;
							euler.z = num12 * ((float)Math.PI / 180f);
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
							NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFE48]");
						}
						num5++;
					}
					while (num5 + 1 < characterCount);
				}
				warpTextExample.m_TextComponent.UpdateVertexData();
				<>2__current = new WaitForSeconds(0.025f);
				<>1__state = 2;
				break;
			}
			result = 1;
			goto IL_08ed;
			IL_08ed:
			return (byte)result != 0;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00268BF0 File Offset: 0x002671F0
		private void System.Collections.IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	// Token: 0x040003B8 RID: 952 Offset: 0x20
	private TMP_Text m_TextComponent;

	// Token: 0x040003B9 RID: 953 Offset: 0x28
	public AnimationCurve VertexCurve;

	// Token: 0x040003BA RID: 954 Offset: 0x30
	public float AngleMultiplier;

	// Token: 0x040003BB RID: 955 Offset: 0x34
	public float SpeedMultiplier;

	// Token: 0x040003BC RID: 956 Offset: 0x38
	public float CurveScale;

	// Token: 0x060002A4 RID: 676 RVA: 0x00269AE0 File Offset: 0x002680E0
	private void Awake()
	{
		m_TextComponent = gameObject.GetComponent<TMP_Text>();
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x00269BC0 File Offset: 0x002681C0
	private void Start()
	{
		<WarpText>d__8 <WarpText>d__ = new <WarpText>d__8(0);
		<WarpText>d__.<>1__state = 0;
		<WarpText>d__.<>4__this = this;
		StartCoroutine(<WarpText>d__);
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x00269B40 File Offset: 0x00268140
	private AnimationCurve CopyAnimationCurve(AnimationCurve curve)
	{
		AnimationCurve animationCurve = new AnimationCurve();
		animationCurve.keys = curve.keys;
		return animationCurve;
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x00269C40 File Offset: 0x00268240
	private IEnumerator WarpText()
	{
		<WarpText>d__8 <WarpText>d__ = new <WarpText>d__8(0);
		<WarpText>d__.<>1__state = 0;
		<WarpText>d__.<>4__this = this;
		return <WarpText>d__;
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x00269CB0 File Offset: 0x002682B0
	public WarpTextExample()
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
		AngleMultiplier = 1f;
		SpeedMultiplier = 1f;
		CurveScale = 1f;
		base..ctor();
	}
}

