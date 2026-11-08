using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using Mono.Xml;
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
			((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
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
			WarpTextExample warpTextExample2 = warpTextExample;
			int num2 = num;
			if (num != 0)
			{
				num2--;
				if (num2 - 1 != 0 && num2 != 1)
				{
					goto IL_081f;
				}
				<>1__state = -1;
			}
			else
			{
				<>1__state = -1;
				AnimationCurve vertexCurve = warpTextExample2.VertexCurve;
				vertexCurve.preWrapMode = NativeIntrinsics.Reinterpret<WrapMode>(1);
				vertexCurve = warpTextExample2.VertexCurve;
				vertexCurve.postWrapMode = NativeIntrinsics.Reinterpret<WrapMode>(1);
				TMP_Text textComponent = warpTextExample2.m_TextComponent;
				textComponent.havePropertiesChanged = true;
				<old_CurveScale>5__2 = (warpTextExample2.CurveScale *= 10f);
				AnimationCurve vertexCurve2 = warpTextExample2.VertexCurve;
				AnimationCurve animationCurve = new AnimationCurve();
				Keyframe[] keys = vertexCurve2.keys;
				animationCurve.keys = keys;
				<old_curve>5__3 = animationCurve;
			}
			Vector3 vector = default(Vector3);
			Vector3 euler = default(Vector3);
			Vector3 vector2 = default(Vector3);
			Vector3 s = default(Vector3);
			Vector3 point = default(Vector3);
			while (true)
			{
				if (!warpTextExample2.m_TextComponent.m_havePropertiesChanged)
				{
					float num3 = <old_CurveScale>5__2;
					if (<old_CurveScale>5__2 == warpTextExample2.CurveScale)
					{
						Keyframe[] keys2 = <old_curve>5__3.keys;
						float value = ((Keyframe*)(NativeIntrinsics.Reinterpret<long>(keys2) + 60L))->value;
						num3 = value;
						Keyframe[] keys3 = warpTextExample2.VertexCurve.keys;
						float value2 = ((Keyframe*)(NativeIntrinsics.Reinterpret<long>(keys3) + 60L))->value;
						num3 = value2;
						if (value == value2)
						{
							<>2__current = null;
							<>1__state = 1;
							break;
						}
					}
				}
				<old_CurveScale>5__2 = warpTextExample2.CurveScale;
				AnimationCurve animationCurve2 = warpTextExample2.CopyAnimationCurve(warpTextExample2.VertexCurve);
				<old_curve>5__3 = animationCurve2;
				TMP_Text textComponent2 = warpTextExample2.m_TextComponent;
				TMP_TextInfo textInfo = textComponent2.m_textInfo;
				int characterCount = textInfo.characterCount;
				if (textInfo.characterCount == 0)
				{
					continue;
				}
				Bounds bounds = warpTextExample2.m_TextComponent.bounds;
				long num4 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x10, 8)");
				long num5 = NativeIntrinsics.Asm("shufps xmm1,xmm15,0FFh");
				float num6 = (float)NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)") - (float)NativeIntrinsics.Asm("shufps xmm1,xmm15,0FFh");
				Bounds bounds2 = warpTextExample2.m_TextComponent.bounds;
				num4 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x10, 8)");
				num5 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
				float num7 = (float)NativeIntrinsics.Asm("shufps xmm2,xmm1,0FFh") + (float)NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
				if (characterCount > 0)
				{
					do
					{
						TMP_CharacterInfo[] characterInfo = textInfo.characterInfo;
						if (NativeIntrinsics.Asm("*(LocalExpr + 0x194)") != 0)
						{
							TMP_MeshInfo[] meshInfo = textInfo.meshInfo;
							long num8 = NativeIntrinsics.Asm("*(LocalExpr + 0x30)");
							long num9 = (int)NativeIntrinsics.Asm("*(FieldExpr + 0x6C)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+24h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+28h],xmm0 ; value = BinaryExpr");
							float num10 = -0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							float num11 = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)")) * 0.5f;
							int num12 = (int)NativeIntrinsics.Asm("*(FieldExpr + 0x14C)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr");
							num10 = -0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr");
							num10 = -0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr");
							num10 = num7 - num6;
							float num13 = warpTextExample2.VertexCurve.Evaluate((num11 - num6) / (num7 - num6));
							num10 = num13;
							float curveScale = warpTextExample2.CurveScale;
							float num14 = warpTextExample2.VertexCurve.Evaluate((num11 - num6) / (num7 - num6) + 0.0001f);
							float num15 = num14 * warpTextExample2.CurveScale - curveScale * num13;
							vector.x = ((float)(int)NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFE48]") - num6) * ((num11 - num6) / (num7 - num6) + 0.0001f) + num6 - num11;
							vector.y = num15;
							vector.z = 0f;
							Vector3 normalized = vector.normalized;
							long num16 = NativeIntrinsics.CallNative("sub_20C3F0", 0L, 0L, 0L, 0L);
							float num17 = (float)NativeIntrinsics.Asm("xmm0") * 57.29578f;
							float y = curveScale * num13;
							if (!(num15 - (((float)(int)NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFE48]") - num6) * ((num11 - num6) / (num7 - num6) + 0.0001f) + num6 - num11) * 0f > 0f))
							{
								num17 = 360f - num17;
								num10 = 360f - num17;
							}
							euler.x = 0f;
							euler.z = num17 * (MathF.PI / 180f);
							Quaternion quaternion = Quaternion.Internal_FromEulerRad(euler);
							vector2.x = 0f;
							vector2.y = y;
							vector2.z = 0f;
							Vector3 pos = vector2;
							Quaternion q = NativeIntrinsics.Reinterpret<Quaternion>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFE78]") + 32L);
							s.x = Vector3.oneVector.x;
							s.y = Vector3.oneVector.y;
							s.z = Vector3.oneVector.z;
							Matrix4x4 matrix4x = Matrix4x4.TRS(pos, q, s);
							long num18 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x20, 16)");
							long num19 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x30, 16)");
							long num20 = NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFE78]") + -32L;
							point.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)"));
							point.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)"));
							Vector3 vector3 = ((Matrix4x4*)num20)->MultiplyPoint3x4(point);
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rdx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rdx*4+28h],r8d ; value = FieldExpr");
							float z = vector3.z;
							num18 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
							Vector3 vector4 = ((Matrix4x4*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFE78]") + -32L))->MultiplyPoint3x4(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFE78]") + -128L));
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rcx*4+28h],edx ; value = FieldExpr");
							num18 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
							Vector3 vector5 = ((Matrix4x4*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFE78]") + -32L))->MultiplyPoint3x4(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFE78]") + -112L));
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rcx*4+28h],edx ; value = FieldExpr");
							num18 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
							Vector3 vector6 = ((Matrix4x4*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFE78]") + -32L))->MultiplyPoint3x4(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFE78]") + -96L));
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rcx*4+28h],edx ; value = FieldExpr");
							num18 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+24h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+28h],xmm1 ; value = BinaryExpr");
							float num21 = (float)num12 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							float num22 = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							num21 = (float)num12 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							num22 = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							num21 = (float)num12 + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							num22 = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm8 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm9 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr");
							num21 = 0f + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)");
							long num23 = NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFE48]");
						}
						int num24 = 0 + 1;
					}
					while (0 + 1 < characterCount);
				}
				WaitForSeconds waitForSeconds = new WaitForSeconds(0.025f);
				<>2__current = waitForSeconds;
				<>1__state = 2;
				break;
			}
			goto IL_081f;
			IL_081f:
			return true;
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00268BF0 File Offset: 0x002671F0
		private void System.Collections.IEnumerator.Reset()
		{
			long num = NativeIntrinsics.CallNative("sub_184BE0", NativeIntrinsics.Asm("rax"), 0L, 0L, 0L);
			NativeIntrinsics.Reinterpret<NotSupportedException>(num)..ctor();
			throw null;
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
		GameObject gameObject = this.gameObject;
		TMP_Text component = gameObject.GetComponent<TMP_Text>();
		m_TextComponent = component;
	}

	// Token: 0x060002A5 RID: 677 RVA: 0x00269BC0 File Offset: 0x002681C0
	private void Start()
	{
		<WarpText>d__8 <WarpText>d__ = NativeIntrinsics.Reinterpret<<WarpText>d__8>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.WarpTextExample+<WarpText>d__8)"));
		((Mono.Xml.SecurityParser)(object)<WarpText>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<WarpText>d__.<>1__state = 0;
		<WarpText>d__.<>4__this = this;
		Coroutine coroutine = StartCoroutine(<WarpText>d__);
	}

	// Token: 0x060002A6 RID: 678 RVA: 0x00269B40 File Offset: 0x00268140
	private AnimationCurve CopyAnimationCurve(AnimationCurve curve)
	{
		AnimationCurve animationCurve = new AnimationCurve();
		Keyframe[] keys = curve.keys;
		animationCurve.keys = keys;
		return animationCurve;
	}

	// Token: 0x060002A7 RID: 679 RVA: 0x00269C40 File Offset: 0x00268240
	private IEnumerator WarpText()
	{
		<WarpText>d__8 <WarpText>d__ = NativeIntrinsics.Reinterpret<<WarpText>d__8>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.WarpTextExample+<WarpText>d__8)"));
		((Mono.Xml.SecurityParser)(object)<WarpText>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<WarpText>d__.<>1__state = 0;
		<WarpText>d__.<>4__this = this;
		return <WarpText>d__;
	}

	// Token: 0x060002A8 RID: 680 RVA: 0x00269CB0 File Offset: 0x002682B0
	public unsafe WarpTextExample()
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
		AngleMultiplier = 1f;
		SpeedMultiplier = 1f;
		CurveScale = 1f;
		((Behaviour)this)..ctor();
	}
}

