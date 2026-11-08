using System;
using Il2CppStudio.Native;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TMPro.Examples;

// Token: 0x02000086 RID: 134
public class TMP_TextSelector_B : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler
{
	// Token: 0x04000364 RID: 868 Offset: 0x20
	public RectTransform TextPopup_Prefab_01;

	// Token: 0x04000365 RID: 869 Offset: 0x28
	private RectTransform m_TextPopup_RectTransform;

	// Token: 0x04000366 RID: 870 Offset: 0x30
	private TextMeshProUGUI m_TextPopup_TMPComponent;

	// Token: 0x04000367 RID: 871 Offset: 0x0
	private const string k_LinkText = "You have selected link <#ffff00>";

	// Token: 0x04000368 RID: 872 Offset: 0x0
	private const string k_WordText = "Word Index: <#ffff00>";

	// Token: 0x04000369 RID: 873 Offset: 0x38
	private TextMeshProUGUI m_TextMeshPro;

	// Token: 0x0400036A RID: 874 Offset: 0x40
	private Canvas m_Canvas;

	// Token: 0x0400036B RID: 875 Offset: 0x48
	private Camera m_Camera;

	// Token: 0x0400036C RID: 876 Offset: 0x50
	private bool isHoveringObject;

	// Token: 0x0400036D RID: 877 Offset: 0x54
	private int m_selectedWord = -1;

	// Token: 0x0400036E RID: 878 Offset: 0x58
	private int m_selectedLink;

	// Token: 0x0400036F RID: 879 Offset: 0x5C
	private int m_lastIndex = -1;

	// Token: 0x04000370 RID: 880 Offset: 0x60
	private Matrix4x4 m_matrix;

	// Token: 0x04000371 RID: 881 Offset: 0xA0
	private TMP_MeshInfo[] m_cachedMeshInfoVertexData;

	// Token: 0x06000254 RID: 596 RVA: 0x0025CE60 File Offset: 0x0025B460
	private void Awake()
	{
		m_TextMeshPro = gameObject.GetComponent<TextMeshProUGUI>();
		m_Canvas = gameObject.GetComponentInParent<Canvas>();
		int num = default(int);
		if (m_Canvas.renderMode != RenderMode.ScreenSpaceOverlay)
		{
			_ = m_Canvas.worldCamera;
		}
		else
		{
			num = 0;
		}
		m_Camera = NativeIntrinsics.Reinterpret<Camera>(num);
		m_TextPopup_RectTransform = UnityEngine.Object.Instantiate(TextPopup_Prefab_01);
		m_TextPopup_RectTransform.SetParent(m_Canvas.transform, worldPositionStays: false);
		m_TextPopup_TMPComponent = m_TextPopup_RectTransform.GetComponentInChildren<TextMeshProUGUI>();
		m_TextPopup_RectTransform.gameObject.SetActive(value: false);
	}

	// Token: 0x06000255 RID: 597 RVA: 0x0025E1C0 File Offset: 0x0025C7C0
	private void OnEnable()
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(null, (IntPtr)0));
	}

	// Token: 0x06000256 RID: 598 RVA: 0x0025E0F0 File Offset: 0x0025C6F0
	private void OnDisable()
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(null, (IntPtr)0));
	}

	// Token: 0x06000257 RID: 599 RVA: 0x0025E040 File Offset: 0x0025C640
	private void ON_TEXT_CHANGED(UnityEngine.Object obj)
	{
		if (obj == m_TextMeshPro)
		{
			m_cachedMeshInfoVertexData = ((TMP_Text)m_TextMeshPro).m_textInfo.CopyMeshInfoVertexData();
		}
	}

	// Token: 0x06000258 RID: 600 RVA: 0x0025D020 File Offset: 0x0025B620
	private void LateUpdate()
	{
		if (!isHoveringObject)
		{
			RestoreCachedVertexAttributes(m_lastIndex);
			m_lastIndex = -1;
			return;
		}
		TextMeshProUGUI textMeshPro = m_TextMeshPro;
		Vector3 mousePosition = Input.mousePosition;
		Camera camera = m_Camera;
		TextMeshProUGUI text = textMeshPro;
		Vector3 position = new Vector3(mousePosition.x, mousePosition.y, mousePosition.z);
		int num = TMP_TextUtilities.FindIntersectingCharacter(text, position, camera, visibleOnly: true);
		long num2 = num;
		RestoreCachedVertexAttributes(m_lastIndex);
		m_lastIndex = -1;
		if (!Input.GetKeyInt(KeyCode.LeftShift))
		{
			Input.GetKeyInt(KeyCode.RightShift);
		}
		TextMeshProUGUI textMeshPro2 = m_TextMeshPro;
		m_lastIndex = (int)num2;
		TextMeshProUGUI textMeshProUGUI = textMeshPro2;
		_ = ((TMP_Text)textMeshProUGUI).m_textInfo.characterInfo;
		_ = ((TMP_Text)textMeshProUGUI).m_textInfo.meshInfo;
		_ = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x58)");
		NativeIntrinsics.Asm("*(LocalExpr + 0x30)");
		_ = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)");
		_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)")) * 0.5f;
		_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)")) * 0.5f;
		_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - 0f;
		float num3 = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)")) * 0.5f;
		float num4 = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)")) * 0.5f;
		NativeIntrinsics.Stmt("movss dword ptr [rdi+rcx*4+20h],xmm0 ; value = LocalExpr");
		NativeIntrinsics.Stmt("movss dword ptr [rdi+rcx*4+24h],xmm1 ; value = LocalExpr");
		NativeIntrinsics.Stmt("movss dword ptr [rdi+rcx*4+28h],xmm2 ; value = LocalExpr");
		_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - num3;
		_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num4;
		_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - 0f;
		NativeIntrinsics.Stmt("movss dword ptr [rdi+rax*4+20h],xmm0 ; value = LocalExpr");
		NativeIntrinsics.Stmt("movss dword ptr [rdi+rax*4+24h],xmm1 ; value = LocalExpr");
		NativeIntrinsics.Stmt("movss dword ptr [rdi+rax*4+28h],xmm2 ; value = LocalExpr");
		_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - num3;
		_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num4;
		_ = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - 0f;
		NativeIntrinsics.Stmt("movss dword ptr [rdi+rax*4+20h],xmm0 ; value = LocalExpr");
		NativeIntrinsics.Stmt("movss dword ptr [rdi+rax*4+24h],xmm1 ; value = LocalExpr");
		NativeIntrinsics.Stmt("movss dword ptr [rdi+rax*4+28h],xmm2 ; value = LocalExpr");
	}

	// Token: 0x06000259 RID: 601 RVA: 0x0025E290 File Offset: 0x0025C890
	public void OnPointerEnter(PointerEventData eventData)
	{
		isHoveringObject = true;
	}

	// Token: 0x0600025A RID: 602 RVA: 0x0025E2A0 File Offset: 0x0025C8A0
	public void OnPointerExit(PointerEventData eventData)
	{
		isHoveringObject = false;
	}

	// Token: 0x0600025B RID: 603 RVA: 0x00250B40 File Offset: 0x0024F140
	public void OnPointerClick(PointerEventData eventData)
	{
	}

	// Token: 0x0600025C RID: 604 RVA: 0x00250B40 File Offset: 0x0024F140
	public void OnPointerUp(PointerEventData eventData)
	{
	}

	// Token: 0x0600025D RID: 605 RVA: 0x0025E2B0 File Offset: 0x0025C8B0
	private void RestoreCachedVertexAttributes(int index)
	{
		long num = index;
		if (index != -1)
		{
			TMP_TextInfo textInfo = ((TMP_Text)m_TextMeshPro).m_textInfo;
			_ = textInfo.characterCount - 1;
			if ((int)num <= textInfo.characterCount - 1)
			{
				_ = ((TMP_Text)m_TextMeshPro).m_textInfo.characterInfo;
				NativeIntrinsics.Stmt("movsd qword ptr [r11+rdx*4+20h],xmm0 ; value = *(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("mov [r11+rdx*4+28h],r8d ; value = *(*(FieldExpr + 0x30) + 0x28)");
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("movsd qword ptr [r11+rcx*4+20h],xmm0 ; value = *(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("mov [r11+rcx*4+28h],edx ; value = *(*(FieldExpr + 0x30) + 0x28)");
				_ = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 1L + ((int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 1L) * 2L;
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("movsd qword ptr [r11+rcx*4+20h],xmm0 ; value = *(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("mov [r11+rcx*4+28h],edx ; value = *(*(FieldExpr + 0x30) + 0x28)");
				_ = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 2L + ((int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 2L) * 2L;
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("movsd qword ptr [r11+rcx*4+20h],xmm0 ; value = *(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("mov [r11+rcx*4+28h],edx ; value = *(*(FieldExpr + 0x30) + 0x28)");
				_ = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 3L + ((int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 3L) * 2L;
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("mov [rdx+r9*4+20h],eax ; value = *(*(FieldExpr + 0x58) + 0x20)");
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x58) + 0x20)");
				NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = *(*(FieldExpr + 0x58) + 0x20)");
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x58) + 0x20)");
				NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = *(*(FieldExpr + 0x58) + 0x20)");
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x58) + 0x20)");
				NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = *(*(FieldExpr + 0x58) + 0x20)");
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x58) + 0x20)");
				_ = m_TextMeshPro;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+r9*8+20h],xmm0 ; value = *(*(FieldExpr + 0x48) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+r9*8+24h],xmm1 ; value = *(*(FieldExpr + 0x48) + 0x24)");
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x48) + 0x20)");
				_ = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 1L;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x48) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x48) + 0x24)");
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x48) + 0x20)");
				_ = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 2L;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x48) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x48) + 0x24)");
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x48) + 0x20)");
				_ = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 3L;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x48) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x48) + 0x24)");
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x48) + 0x20)");
				_ = m_TextMeshPro;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+r9*8+20h],xmm0 ; value = *(*(FieldExpr + 0x50) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+r9*8+24h],xmm1 ; value = *(*(FieldExpr + 0x50) + 0x24)");
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x50) + 0x20)");
				_ = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 1L;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x50) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x50) + 0x24)");
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x50) + 0x20)");
				_ = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 2L;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x50) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x50) + 0x24)");
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x50) + 0x20)");
				_ = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 3L;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x50) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x50) + 0x24)");
				_ = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + (((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") >> 31) & 3) >> 2;
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x50) + 0x20)");
				NativeIntrinsics.Stmt("movsd qword ptr [r11+rdx*4+20h],xmm0 ; value = *(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("mov [r11+rdx*4+28h],r8d ; value = *(*(FieldExpr + 0x30) + 0x28)");
				_ = (int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + (((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") >> 31) & 3) >> 2) * 4L + -4L);
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("movsd qword ptr [r11+rcx*4+20h],xmm0 ; value = *(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("mov [r11+rcx*4+28h],edx ; value = *(*(FieldExpr + 0x30) + 0x28)");
				_ = (int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + (((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") >> 31) & 3) >> 2) * 4L + -4L) + 1L + ((int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + (((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") >> 31) & 3) >> 2) * 4L + -4L) + 1L) * 2L;
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("movsd qword ptr [r11+rcx*4+20h],xmm0 ; value = *(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("mov [r11+rcx*4+28h],edx ; value = *(*(FieldExpr + 0x30) + 0x28)");
				_ = (int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + (((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") >> 31) & 3) >> 2) * 4L + -4L) + 2L + ((int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + (((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") >> 31) & 3) >> 2) * 4L + -4L) + 2L) * 2L;
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("movsd qword ptr [r11+rcx*4+20h],xmm0 ; value = *(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("mov [r11+rcx*4+28h],edx ; value = *(*(FieldExpr + 0x30) + 0x28)");
				_ = (int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + (((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") >> 31) & 3) >> 2) * 4L + -4L) + 3L + ((int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + (((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") >> 31) & 3) >> 2) * 4L + -4L) + 3L) * 2L;
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)");
				_ = m_TextMeshPro;
				NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = *(*(FieldExpr + 0x58) + 0x20)");
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x58) + 0x20)");
				NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = *(*(FieldExpr + 0x58) + 0x20)");
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x58) + 0x20)");
				NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = *(*(FieldExpr + 0x58) + 0x20)");
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x58) + 0x20)");
				NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = *(*(FieldExpr + 0x58) + 0x20)");
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x58) + 0x20)");
				_ = m_TextMeshPro;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*8+20h],xmm0 ; value = *(*(FieldExpr + 0x48) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*8+24h],xmm1 ; value = *(*(FieldExpr + 0x48) + 0x24)");
				_ = (int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + (((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") >> 31) & 3) >> 2) * 4L + -4L);
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x48) + 0x20)");
				_ = (int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + (((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") >> 31) & 3) >> 2) * 4L + -4L) + 1L;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x48) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x48) + 0x24)");
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x48) + 0x20)");
				_ = (int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + (((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") >> 31) & 3) >> 2) * 4L + -4L) + 2L;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x48) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x48) + 0x24)");
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x48) + 0x20)");
				_ = (int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + (((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") >> 31) & 3) >> 2) * 4L + -4L) + 3L;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x48) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x48) + 0x24)");
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x48) + 0x20)");
				_ = m_TextMeshPro;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*8+20h],xmm0 ; value = *(*(FieldExpr + 0x50) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*8+24h],xmm1 ; value = *(*(FieldExpr + 0x50) + 0x24)");
				_ = (int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + (((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") >> 31) & 3) >> 2) * 4L + -4L);
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x50) + 0x20)");
				_ = (int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + (((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") >> 31) & 3) >> 2) * 4L + -4L) + 1L;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x50) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x50) + 0x24)");
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x50) + 0x20)");
				_ = (int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + (((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") >> 31) & 3) >> 2) * 4L + -4L) + 2L;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x50) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x50) + 0x24)");
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x50) + 0x20)");
				_ = (int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + (((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") >> 31) & 3) >> 2) * 4L + -4L) + 3L;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x50) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x50) + 0x24)");
				NativeIntrinsics.Asm("*(*(FieldExpr + 0x50) + 0x20)");
				m_TextMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
			}
		}
	}
}

