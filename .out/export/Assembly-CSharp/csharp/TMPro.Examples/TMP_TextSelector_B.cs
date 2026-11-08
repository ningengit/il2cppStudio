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
	private const string k_LinkText = null;

	// Token: 0x04000368 RID: 872 Offset: 0x0
	private const string k_WordText = null;

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
		GameObject gameObject = this.gameObject;
		TextMeshProUGUI component = gameObject.GetComponent<TextMeshProUGUI>();
		m_TextMeshPro = component;
		GameObject gameObject2 = this.gameObject;
		Canvas componentInParent = gameObject2.GetComponentInParent<Canvas>();
		m_Canvas = componentInParent;
		RenderMode renderMode = m_Canvas.renderMode;
		if ((NativeIntrinsics.Reinterpret<int>(renderMode) & NativeIntrinsics.Reinterpret<int>(renderMode)) != 0)
		{
			Canvas canvas = m_Canvas;
			Camera worldCamera = canvas.worldCamera;
			Camera camera = worldCamera;
			Camera camera2 = worldCamera;
		}
		m_Camera = null;
		RectTransform textPopup_Prefab_ = TextPopup_Prefab_01;
		RectTransform textPopup_RectTransform = UnityEngine.Object.Instantiate(textPopup_Prefab_);
		m_TextPopup_RectTransform = textPopup_RectTransform;
		RectTransform textPopup_RectTransform2 = m_TextPopup_RectTransform;
		Transform parent = m_Canvas.transform;
		textPopup_RectTransform2.SetParent(parent, worldPositionStays: false);
		TextMeshProUGUI componentInChildren = m_TextPopup_RectTransform.GetComponentInChildren<TextMeshProUGUI>();
		m_TextPopup_TMPComponent = componentInChildren;
		GameObject gameObject3 = m_TextPopup_RectTransform.gameObject;
		gameObject3.SetActive(value: false);
	}

	// Token: 0x06000255 RID: 597 RVA: 0x0025E1C0 File Offset: 0x0025C7C0
	private void OnEnable()
	{
		FastAction<UnityEngine.Object> tEXT_CHANGED_EVENT = TMPro_EventManager.TEXT_CHANGED_EVENT;
		Action<UnityEngine.Object> rhs = (Action<UnityEngine.Object>)(object)new System.IOAsyncCallback(ON_TEXT_CHANGED);
		tEXT_CHANGED_EVENT.Add(rhs);
	}

	// Token: 0x06000256 RID: 598 RVA: 0x0025E0F0 File Offset: 0x0025C6F0
	private void OnDisable()
	{
		FastAction<UnityEngine.Object> tEXT_CHANGED_EVENT = TMPro_EventManager.TEXT_CHANGED_EVENT;
		Action<UnityEngine.Object> rhs = (Action<UnityEngine.Object>)(object)new System.IOAsyncCallback(ON_TEXT_CHANGED);
		tEXT_CHANGED_EVENT.Remove(rhs);
	}

	// Token: 0x06000257 RID: 599 RVA: 0x0025E040 File Offset: 0x0025C640
	private void ON_TEXT_CHANGED(UnityEngine.Object obj)
	{
		TextMeshProUGUI textMeshPro = m_TextMeshPro;
		bool flag = obj == textMeshPro;
		bool flag2 = flag;
		if (flag)
		{
			TextMeshProUGUI textMeshPro2 = m_TextMeshPro;
			TMP_MeshInfo[] cachedMeshInfoVertexData = ((TMP_Text)textMeshPro2).m_textInfo.CopyMeshInfoVertexData();
			m_cachedMeshInfoVertexData = cachedMeshInfoVertexData;
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
		long num = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
		float z = mousePosition.z;
		Vector3 position = default(Vector3);
		position.x = NativeIntrinsics.Reinterpret<float>(num);
		position.z = z;
		int num2 = TMP_TextUtilities.FindIntersectingCharacter(text, position, camera, visibleOnly: true);
		long num3 = num2;
		if (num2 != -1)
		{
		}
		RestoreCachedVertexAttributes(m_lastIndex);
		m_lastIndex = -1;
		if (!Input.GetKeyInt(NativeIntrinsics.Reinterpret<KeyCode>(304)))
		{
			bool keyInt = Input.GetKeyInt(NativeIntrinsics.Reinterpret<KeyCode>(303));
		}
		TextMeshProUGUI textMeshPro2 = m_TextMeshPro;
		m_lastIndex = (int)num3;
		TextMeshProUGUI textMeshProUGUI = textMeshPro2;
		TMP_TextInfo textInfo = ((TMP_Text)textMeshProUGUI).m_textInfo;
		TMP_CharacterInfo[] characterInfo = textInfo.characterInfo;
		TextMeshProUGUI textMeshProUGUI2 = textMeshProUGUI;
		TMP_MeshInfo[] meshInfo = ((TMP_Text)textMeshProUGUI).m_textInfo.meshInfo;
		long num4 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x58)");
		long num5 = NativeIntrinsics.Asm("*(LocalExpr + 0x30)");
		num3 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)");
		float num6 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)")) * 0.5f;
		float num7 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)")) * 0.5f;
		float num8 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - 0f;
		float num9 = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)")) * 0.5f;
		float num10 = ((float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") + (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)")) * 0.5f;
		NativeIntrinsics.Stmt("movss dword ptr [rdi+rcx*4+20h],xmm0 ; value = LocalExpr");
		NativeIntrinsics.Stmt("movss dword ptr [rdi+rcx*4+24h],xmm1 ; value = LocalExpr");
		NativeIntrinsics.Stmt("movss dword ptr [rdi+rcx*4+28h],xmm2 ; value = LocalExpr");
		long num11 = num3 + 1L;
		num6 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - num9;
		num7 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num10;
		num8 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - 0f;
		NativeIntrinsics.Stmt("movss dword ptr [rdi+rax*4+20h],xmm0 ; value = LocalExpr");
		NativeIntrinsics.Stmt("movss dword ptr [rdi+rax*4+24h],xmm1 ; value = LocalExpr");
		NativeIntrinsics.Stmt("movss dword ptr [rdi+rax*4+28h],xmm2 ; value = LocalExpr");
		num11 = num3 + 2L;
		num6 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x20)") - num9;
		num7 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x24)") - num10;
		num8 = (float)(int)NativeIntrinsics.Asm("*(LocalExpr + 0x28)") - 0f;
		NativeIntrinsics.Stmt("movss dword ptr [rdi+rax*4+20h],xmm0 ; value = LocalExpr");
		NativeIntrinsics.Stmt("movss dword ptr [rdi+rax*4+24h],xmm1 ; value = LocalExpr");
		NativeIntrinsics.Stmt("movss dword ptr [rdi+rax*4+28h],xmm2 ; value = LocalExpr");
		num11 = num3 + 3L;
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
			TextMeshProUGUI textMeshPro = m_TextMeshPro;
			TMP_TextInfo textInfo = ((TMP_Text)textMeshPro).m_textInfo;
			int num2 = textInfo.characterCount - 1;
			if ((int)num <= textInfo.characterCount - 1)
			{
				TextMeshProUGUI textMeshPro2 = m_TextMeshPro;
				TMP_CharacterInfo[] characterInfo = ((TMP_Text)textMeshPro2).m_textInfo.characterInfo;
				NativeIntrinsics.Stmt("movsd qword ptr [r11+rdx*4+20h],xmm0 ; value = *(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("mov [r11+rdx*4+28h],r8d ; value = *(*(FieldExpr + 0x30) + 0x28)");
				long num3 = NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("movsd qword ptr [r11+rcx*4+20h],xmm0 ; value = *(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("mov [r11+rcx*4+28h],edx ; value = *(*(FieldExpr + 0x30) + 0x28)");
				long num4 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 1L + ((int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 1L) * 2L;
				num3 = NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("movsd qword ptr [r11+rcx*4+20h],xmm0 ; value = *(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("mov [r11+rcx*4+28h],edx ; value = *(*(FieldExpr + 0x30) + 0x28)");
				num4 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 2L + ((int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 2L) * 2L;
				num3 = NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("movsd qword ptr [r11+rcx*4+20h],xmm0 ; value = *(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("mov [r11+rcx*4+28h],edx ; value = *(*(FieldExpr + 0x30) + 0x28)");
				num4 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 3L + ((int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 3L) * 2L;
				num3 = NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("mov [rdx+r9*4+20h],eax ; value = *(*(FieldExpr + 0x58) + 0x20)");
				int num5 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x58) + 0x20)");
				NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = *(*(FieldExpr + 0x58) + 0x20)");
				num5 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x58) + 0x20)");
				NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = *(*(FieldExpr + 0x58) + 0x20)");
				num5 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x58) + 0x20)");
				NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = *(*(FieldExpr + 0x58) + 0x20)");
				num5 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x58) + 0x20)");
				TextMeshProUGUI textMeshPro3 = m_TextMeshPro;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+r9*8+20h],xmm0 ; value = *(*(FieldExpr + 0x48) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+r9*8+24h],xmm1 ; value = *(*(FieldExpr + 0x48) + 0x24)");
				int num6 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x48) + 0x20)");
				long num7 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 1L;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x48) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x48) + 0x24)");
				num6 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x48) + 0x20)");
				num7 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 2L;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x48) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x48) + 0x24)");
				num6 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x48) + 0x20)");
				num7 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 3L;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x48) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x48) + 0x24)");
				num6 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x48) + 0x20)");
				TextMeshProUGUI textMeshPro4 = m_TextMeshPro;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+r9*8+20h],xmm0 ; value = *(*(FieldExpr + 0x50) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+r9*8+24h],xmm1 ; value = *(*(FieldExpr + 0x50) + 0x24)");
				num6 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x50) + 0x20)");
				long num8 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 1L;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x50) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x50) + 0x24)");
				num6 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x50) + 0x20)");
				num8 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 2L;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x50) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x50) + 0x24)");
				num6 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x50) + 0x20)");
				num8 = (int)NativeIntrinsics.Asm("*(BinaryExpr + 0x6C)") + 3L;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x50) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x50) + 0x24)");
				int num9 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + ((int)NativeIntrinsics.Asm("*(FieldExpr + 0x50)") & 3) >> 2;
				num6 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x50) + 0x20)");
				NativeIntrinsics.Stmt("movsd qword ptr [r11+rdx*4+20h],xmm0 ; value = *(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("mov [r11+rdx*4+28h],r8d ; value = *(*(FieldExpr + 0x30) + 0x28)");
				long num10 = (int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + ((int)NativeIntrinsics.Asm("*(FieldExpr + 0x50)") & 3) >> 2) * 4L + -4L);
				long num11 = NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("movsd qword ptr [r11+rcx*4+20h],xmm0 ; value = *(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("mov [r11+rcx*4+28h],edx ; value = *(*(FieldExpr + 0x30) + 0x28)");
				num10 = (int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + ((int)NativeIntrinsics.Asm("*(FieldExpr + 0x50)") & 3) >> 2) * 4L + -4L) + 1L + ((int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + ((int)NativeIntrinsics.Asm("*(FieldExpr + 0x50)") & 3) >> 2) * 4L + -4L) + 1L) * 2L;
				num11 = NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("movsd qword ptr [r11+rcx*4+20h],xmm0 ; value = *(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("mov [r11+rcx*4+28h],edx ; value = *(*(FieldExpr + 0x30) + 0x28)");
				num10 = (int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + ((int)NativeIntrinsics.Asm("*(FieldExpr + 0x50)") & 3) >> 2) * 4L + -4L) + 2L + ((int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + ((int)NativeIntrinsics.Asm("*(FieldExpr + 0x50)") & 3) >> 2) * 4L + -4L) + 2L) * 2L;
				num11 = NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("movsd qword ptr [r11+rcx*4+20h],xmm0 ; value = *(*(FieldExpr + 0x30) + 0x20)");
				NativeIntrinsics.Stmt("mov [r11+rcx*4+28h],edx ; value = *(*(FieldExpr + 0x30) + 0x28)");
				num10 = (int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + ((int)NativeIntrinsics.Asm("*(FieldExpr + 0x50)") & 3) >> 2) * 4L + -4L) + 3L + ((int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + ((int)NativeIntrinsics.Asm("*(FieldExpr + 0x50)") & 3) >> 2) * 4L + -4L) + 3L) * 2L;
				num11 = NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x20)");
				TextMeshProUGUI textMeshPro5 = m_TextMeshPro;
				NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = *(*(FieldExpr + 0x58) + 0x20)");
				int num12 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x58) + 0x20)");
				NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = *(*(FieldExpr + 0x58) + 0x20)");
				num12 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x58) + 0x20)");
				NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = *(*(FieldExpr + 0x58) + 0x20)");
				num12 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x58) + 0x20)");
				NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = *(*(FieldExpr + 0x58) + 0x20)");
				num12 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x58) + 0x20)");
				TextMeshProUGUI textMeshPro6 = m_TextMeshPro;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*8+20h],xmm0 ; value = *(*(FieldExpr + 0x48) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*8+24h],xmm1 ; value = *(*(FieldExpr + 0x48) + 0x24)");
				long num13 = (int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + ((int)NativeIntrinsics.Asm("*(FieldExpr + 0x50)") & 3) >> 2) * 4L + -4L);
				int num14 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x48) + 0x20)");
				num13 = (int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + ((int)NativeIntrinsics.Asm("*(FieldExpr + 0x50)") & 3) >> 2) * 4L + -4L) + 1L;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x48) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x48) + 0x24)");
				num14 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x48) + 0x20)");
				num13 = (int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + ((int)NativeIntrinsics.Asm("*(FieldExpr + 0x50)") & 3) >> 2) * 4L + -4L) + 2L;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x48) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x48) + 0x24)");
				num14 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x48) + 0x20)");
				num13 = (int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + ((int)NativeIntrinsics.Asm("*(FieldExpr + 0x50)") & 3) >> 2) * 4L + -4L) + 3L;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x48) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x48) + 0x24)");
				num14 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x48) + 0x20)");
				TextMeshProUGUI textMeshPro7 = m_TextMeshPro;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*8+20h],xmm0 ; value = *(*(FieldExpr + 0x50) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rcx*8+24h],xmm1 ; value = *(*(FieldExpr + 0x50) + 0x24)");
				long num15 = (int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + ((int)NativeIntrinsics.Asm("*(FieldExpr + 0x50)") & 3) >> 2) * 4L + -4L);
				num14 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x50) + 0x20)");
				num15 = (int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + ((int)NativeIntrinsics.Asm("*(FieldExpr + 0x50)") & 3) >> 2) * 4L + -4L) + 1L;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x50) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x50) + 0x24)");
				num14 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x50) + 0x20)");
				num15 = (int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + ((int)NativeIntrinsics.Asm("*(FieldExpr + 0x50)") & 3) >> 2) * 4L + -4L) + 2L;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x50) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x50) + 0x24)");
				num14 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x50) + 0x20)");
				num15 = (int)(0L + ((int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x30) + 0x18)") + ((int)NativeIntrinsics.Asm("*(FieldExpr + 0x50)") & 3) >> 2) * 4L + -4L) + 3L;
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+20h],xmm0 ; value = *(*(FieldExpr + 0x50) + 0x20)");
				NativeIntrinsics.Stmt("movss dword ptr [rdx+rax*8+24h],xmm1 ; value = *(*(FieldExpr + 0x50) + 0x24)");
				num14 = (int)NativeIntrinsics.Asm("*(*(FieldExpr + 0x50) + 0x20)");
			}
		}
	}
}

