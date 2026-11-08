using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
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

		// Token: 0x0600029D RID: 669 RVA: 0x002680C0 File Offset: 0x002666C0
		internal int <AnimateVertexColors>b__0(int a, int b)
		{
			float value = modifiedCharScale.get_Item(a);
			float value2 = modifiedCharScale.get_Item(b);
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
				<>8__1 = new <>c__DisplayClass10_0();
				vertexZoom.m_TextComponent.ForceMeshUpdate(ignoreActiveState: false, forceTextReparsing: false);
				<textInfo>5__2 = vertexZoom.m_TextComponent.m_textInfo;
				<cachedMeshInfoVertexData>5__3 = <textInfo>5__2.CopyMeshInfoVertexData();
				<>8__1.modifiedCharScale = new List<float>();
				<scaleSortingOrder>5__4 = new List<int>();
				vertexZoom.hasTextChanged = true;
			}
			if (vertexZoom.hasTextChanged)
			{
				<cachedMeshInfoVertexData>5__3 = <textInfo>5__2.CopyMeshInfoVertexData();
				vertexZoom.hasTextChanged = false;
			}
			int characterCount = <textInfo>5__2.characterCount;
			if (<textInfo>5__2.characterCount != 0)
			{
				_ = <>8__1.modifiedCharScale;
				NativeIntrinsics.Stmt("inc dword ptr [rcx+1Ch] ; value = BinaryExpr");
				NativeIntrinsics.Stmt("mov dword ptr [rcx+18h],0 ; value = 0x0");
				_ = <scaleSortingOrder>5__4;
				NativeIntrinsics.Stmt("inc dword ptr [rcx+1Ch] ; value = BinaryExpr");
				NativeIntrinsics.Stmt("mov dword ptr [rcx+18h],0 ; value = 0x0");
				<AnimateVertexColors>d__10 <AnimateVertexColors>d__ = this;
				if (characterCount > 0)
				{
					<AnimateVertexColors>d__ = this;
					int num3 = 0;
					float num4 = 0.5f;
					float num5 = 0f;
					float minInclusive = 1f;
					float maxInclusive = 1.5f;
					Vector3 vector = default(Vector3);
					Quaternion quaternion = default(Quaternion);
					Vector3 s = default(Vector3);
					Matrix4x4 matrix4x2 = default(Matrix4x4);
					Vector3 point = default(Vector3);
					Vector3 point2 = default(Vector3);
					Vector3 point3 = default(Vector3);
					Vector3 point4 = default(Vector3);
					do
					{
						NativeIntrinsics.CallNative("GetAt", NativeIntrinsics.Reinterpret<long>(<AnimateVertexColors>d__.<textInfo>5__2.characterInfo), NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFDD8]"), num3, 0L);
						if (NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFF4C]") != 0)
						{
							_ = <AnimateVertexColors>d__.<cachedMeshInfoVertexData>5__3;
							NativeIntrinsics.Asm("*(LocalExpr + 0x30)");
							_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)")) * num4;
							_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)")) * num4;
							_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num5;
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+20h],xmm0 ; value = LocalExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+24h],xmm1 ; value = LocalExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+28h],xmm2 ; value = LocalExpr");
							_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)")) * num4;
							_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)")) * num4;
							_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num5;
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = LocalExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm1 ; value = LocalExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm2 ; value = LocalExpr");
							_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)")) * num4;
							_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)")) * num4;
							_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num5;
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = LocalExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm1 ; value = LocalExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm2 ; value = LocalExpr");
							_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)")) * num4;
							_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)")) * num4;
							_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - num5;
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = LocalExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm1 ; value = LocalExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm2 ; value = LocalExpr");
							float num6 = UnityEngine.Random.Range(minInclusive, maxInclusive);
							<AnimateVertexColors>d__.<>8__1.modifiedCharScale.Add(num6);
							<AnimateVertexColors>d__.<scaleSortingOrder>5__4.Add((int)NativeIntrinsics.Asm("*(FieldExpr + 0x18)") - 1);
							float x = num6 * Vector3.oneVector.x;
							vector.x = 0f;
							vector.z = 0f;
							Vector3 pos = vector;
							quaternion.x = NativeIntrinsics.Reinterpret<float>(Quaternion.identityQuaternion);
							Quaternion q = quaternion;
							s = new Vector3(x, num6 * Vector3.oneVector.y, num6 * Vector3.oneVector.z);
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
							point.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)"));
							point.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x28)"));
							Vector3 vector2 = matrix4x2.MultiplyPoint3x4(point);
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rdx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rdx*4+28h],r8d ; value = FieldExpr");
							_ = vector2.z;
							point2.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)"));
							point2.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x28)"));
							matrix4x2.MultiplyPoint3x4(point2);
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rcx*4+28h],edx ; value = FieldExpr");
							point3.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)"));
							point3.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x28)"));
							matrix4x2.MultiplyPoint3x4(point3);
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rcx*4+28h],edx ; value = FieldExpr");
							point4.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)"));
							point4.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x28)"));
							matrix4x2.MultiplyPoint3x4(point4);
							NativeIntrinsics.Stmt("movsd qword ptr [rbx+rcx*4+20h],xmm0 ; value = StructChunkExpr");
							NativeIntrinsics.Stmt("mov [rbx+rcx*4+28h],edx ; value = FieldExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+24h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rcx*4+28h],xmm1 ; value = BinaryExpr");
							_ = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)")) * num4 + (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x24)");
							_ = 0f + (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							_ = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)")) * num4 + (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x24)");
							_ = 0f + (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm0 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm1 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm0 ; value = BinaryExpr");
							_ = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)")) * num4 + (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x24)");
							_ = 0f + (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x28)");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+20h],xmm6 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+24h],xmm7 ; value = BinaryExpr");
							NativeIntrinsics.Stmt("movss dword ptr [rbx+rax*4+28h],xmm0 ; value = BinaryExpr");
							_ = <AnimateVertexColors>d__.<cachedMeshInfoVertexData>5__3;
							_ = 0f + (float)(int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x28)");
							NativeIntrinsics.Asm("*(LocalExpr + 0x48)");
							NativeIntrinsics.Stmt("movss dword ptr [rdx+rdi*8+20h],xmm0 ; value = *(LocalExpr + 0x20)");
							NativeIntrinsics.Stmt("movss dword ptr [rdx+rdi*8+24h],xmm1 ; value = *(LocalExpr + 0x24)");
							_ = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 1L;
							NativeIntrinsics.Asm("*(LocalExpr + 0x20)");
							NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*8+24h],xmm0 ; value = *(LocalExpr + 0x24)");
							NativeIntrinsics.Stmt("mov [rdx+rcx*8+20h],eax ; value = *(LocalExpr + 0x20)");
							_ = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 2L;
							NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*8+24h],xmm0 ; value = *(LocalExpr + 0x24)");
							NativeIntrinsics.Stmt("mov [rdx+rcx*8+20h],eax ; value = *(LocalExpr + 0x20)");
							_ = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 3L;
							NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*8+24h],xmm0 ; value = *(LocalExpr + 0x24)");
							NativeIntrinsics.Stmt("mov [rdx+rcx*8+20h],eax ; value = *(LocalExpr + 0x20)");
							_ = <AnimateVertexColors>d__.<cachedMeshInfoVertexData>5__3;
							NativeIntrinsics.Asm("*(LocalExpr + 0x24)");
							NativeIntrinsics.Asm("*(LocalExpr + 0x58)");
							NativeIntrinsics.Stmt("mov [rdx+rdi*4+20h],eax ; value = *(LocalExpr + 0x20)");
							_ = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 1L;
							NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = *(LocalExpr + 0x20)");
							_ = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 2L;
							NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = *(LocalExpr + 0x20)");
							_ = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 3L;
							NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = *(LocalExpr + 0x20)");
						}
						num3++;
					}
					while (num3 + 1 < characterCount);
				}
				TMP_TextInfo tMP_TextInfo = <AnimateVertexColors>d__.<textInfo>5__2;
				int num7 = 0;
				int num8 = 0;
				while (num7 < tMP_TextInfo.meshInfo.Length)
				{
					<>c__DisplayClass10_0 <>c__DisplayClass10_ = <AnimateVertexColors>d__.<>8__1;
					List<int> list = <AnimateVertexColors>d__.<scaleSortingOrder>5__4;
					Comparison<int> comparison = <>c__DisplayClass10_.<>9__0;
					if (<>c__DisplayClass10_.<>9__0 == null)
					{
						<>c__DisplayClass10_0 CS$<>8__locals3 = <AnimateVertexColors>d__.<>8__1;
						comparison = (CS$<>8__locals3.<>9__0 = delegate(int a, int b)
						{
							float value = CS$<>8__locals3.modifiedCharScale.get_Item(a);
							float value2 = CS$<>8__locals3.modifiedCharScale.get_Item(b);
							float num9 = default(float);
							num9.m_value = value;
							return num9.CompareTo(value2);
						});
					}
					list.Sort(comparison);
					TMP_MeshInfo[] meshInfo = <AnimateVertexColors>d__.<textInfo>5__2.meshInfo;
					List<int> sortingOrder = <AnimateVertexColors>d__.<scaleSortingOrder>5__4;
					((TMP_MeshInfo*)((num8 + num8 * 4L << 4) + 32L + NativeIntrinsics.Reinterpret<long>(meshInfo)))->SortGeometry(sortingOrder);
					_ = <AnimateVertexColors>d__.<textInfo>5__2.meshInfo;
					NativeIntrinsics.Reinterpret<Mesh>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)")).vertices = NativeIntrinsics.Reinterpret<Vector3[]>(NativeIntrinsics.Asm("*(FieldExpr + 0x30)"));
					_ = <AnimateVertexColors>d__.<textInfo>5__2.meshInfo;
					NativeIntrinsics.Reinterpret<Mesh>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)")).uv = NativeIntrinsics.Reinterpret<Vector2[]>(NativeIntrinsics.Asm("*(FieldExpr + 0x48)"));
					_ = <AnimateVertexColors>d__.<textInfo>5__2.meshInfo;
					NativeIntrinsics.Reinterpret<Mesh>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)")).colors32 = NativeIntrinsics.Reinterpret<Color32[]>(NativeIntrinsics.Asm("*(FieldExpr + 0x58)"));
					vertexZoom.m_TextComponent.UpdateGeometry(NativeIntrinsics.Reinterpret<Mesh>(NativeIntrinsics.Asm("*(FieldExpr + 0x20)")), num8);
					_ = <AnimateVertexColors>d__.<textInfo>5__2;
					num8++;
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
				<>2__current = new WaitForSeconds(0.25f);
				<>1__state = 1;
			}
			return true;
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00266360 File Offset: 0x00264960
		private void System.Collections.IEnumerator.Reset()
		{
			throw new NotSupportedException();
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
		m_TextComponent = GetComponent<TMP_Text>();
	}

	// Token: 0x06000296 RID: 662 RVA: 0x00269990 File Offset: 0x00267F90
	private void OnEnable()
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(null, (IntPtr)0));
	}

	// Token: 0x06000297 RID: 663 RVA: 0x002698C0 File Offset: 0x00267EC0
	private void OnDisable()
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(null, (IntPtr)0));
	}

	// Token: 0x06000298 RID: 664 RVA: 0x00269A60 File Offset: 0x00268060
	private void Start()
	{
		<AnimateVertexColors>d__10 <AnimateVertexColors>d__ = new <AnimateVertexColors>d__10(0);
		<AnimateVertexColors>d__.<>1__state = 0;
		<AnimateVertexColors>d__.<>4__this = this;
		StartCoroutine(<AnimateVertexColors>d__);
	}

	// Token: 0x06000299 RID: 665 RVA: 0x00269850 File Offset: 0x00267E50
	private void ON_TEXT_CHANGED(UnityEngine.Object obj)
	{
		if (obj == m_TextComponent)
		{
			hasTextChanged = true;
		}
	}

	// Token: 0x0600029A RID: 666 RVA: 0x00269790 File Offset: 0x00267D90
	private IEnumerator AnimateVertexColors()
	{
		<AnimateVertexColors>d__10 <AnimateVertexColors>d__ = new <AnimateVertexColors>d__10(0);
		<AnimateVertexColors>d__.<>1__state = 0;
		<AnimateVertexColors>d__.<>4__this = this;
		return <AnimateVertexColors>d__;
	}
}

