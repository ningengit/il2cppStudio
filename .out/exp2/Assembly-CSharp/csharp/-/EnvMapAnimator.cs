using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000058 RID: 88
public class EnvMapAnimator : MonoBehaviour
{
	// Token: 0x02000059 RID: 89
	private sealed class <Start>d__4 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x0400027D RID: 637 Offset: 0x10
		private int <>1__state;

		// Token: 0x0400027E RID: 638 Offset: 0x18
		private object <>2__current;

		// Token: 0x0400027F RID: 639 Offset: 0x20
		public EnvMapAnimator <>4__this;

		// Token: 0x04000280 RID: 640 Offset: 0x28
		private Matrix4x4 <matrix>5__2;

		private object System.Collections.Generic.IEnumerator<System.Object>.Current => <>2__current;

		private object System.Collections.IEnumerator.Current => <>2__current;

		// Token: 0x060001AE RID: 430 RVA: 0x00250B50 File Offset: 0x0024F150
		public <Start>d__4(int <>1__state)
		{
			this.<>1__state = <>1__state;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00250B40 File Offset: 0x0024F140
		private void System.IDisposable.Dispose()
		{
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00262B10 File Offset: 0x00261110
		private bool MoveNext()
		{
			_ = <>1__state;
			_ = <>4__this;
			if (<>1__state == 0)
			{
				<>1__state = -1;
				<matrix>5__2.m00 = 0f;
				<matrix>5__2.m10 = 0f;
				<matrix>5__2.m20 = 0f;
				<matrix>5__2.m30 = 0f;
				<matrix>5__2.m01 = 0f;
				<matrix>5__2.m11 = 0f;
				<matrix>5__2.m21 = 0f;
				<matrix>5__2.m31 = 0f;
				<matrix>5__2.m02 = 0f;
				<matrix>5__2.m12 = 0f;
				<matrix>5__2.m22 = 0f;
				<matrix>5__2.m32 = 0f;
				<matrix>5__2.m03 = 0f;
				<matrix>5__2.m13 = 0f;
				<matrix>5__2.m23 = 0f;
				<matrix>5__2.m33 = 0f;
			}
			else
			{
				<>1__state = -1;
			}
			_ = Time.time;
			return false;
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00262D80 File Offset: 0x00261380
		private void System.Collections.IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	// Token: 0x0400027A RID: 634 Offset: 0x20
	public Vector3 RotationSpeeds;

	// Token: 0x0400027B RID: 635 Offset: 0x30
	private TMP_Text m_textMeshPro;

	// Token: 0x0400027C RID: 636 Offset: 0x38
	private Material m_material;

	// Token: 0x060001AB RID: 427 RVA: 0x00255480 File Offset: 0x00253A80
	private void Awake()
	{
		m_textMeshPro = GetComponent<TMP_Text>();
		m_material = m_textMeshPro.fontSharedMaterial;
	}

	// Token: 0x060001AC RID: 428 RVA: 0x00255500 File Offset: 0x00253B00
	private IEnumerator Start()
	{
		<Start>d__4 <Start>d__ = new <Start>d__4(0);
		<Start>d__.<>1__state = 0;
		<Start>d__.<>4__this = this;
		return <Start>d__;
	}
}

