using Il2CppStudio.Native;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TMPro.Examples;

// Token: 0x02000085 RID: 133
public class TMP_TextSelector_A : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x0400035E RID: 862 Offset: 0x20
	private TextMeshPro m_TextMeshPro;

	// Token: 0x0400035F RID: 863 Offset: 0x28
	private Camera m_Camera;

	// Token: 0x04000360 RID: 864 Offset: 0x30
	private bool m_isHoveringObject;

	// Token: 0x04000361 RID: 865 Offset: 0x34
	private int m_selectedLink = -1;

	// Token: 0x04000362 RID: 866 Offset: 0x38
	private int m_lastCharIndex;

	// Token: 0x04000363 RID: 867 Offset: 0x3C
	private int m_lastWordIndex = -1;

	// Token: 0x0600024F RID: 591 RVA: 0x0025C430 File Offset: 0x0025AA30
	private void Awake()
	{
		m_TextMeshPro = gameObject.GetComponent<TextMeshPro>();
		m_Camera = Camera.main;
		m_TextMeshPro.ForceMeshUpdate(ignoreActiveState: false, forceTextReparsing: false);
	}

	// Token: 0x06000250 RID: 592 RVA: 0x0025C4D0 File Offset: 0x0025AAD0
	private void LateUpdate()
	{
		TextMeshPro textMeshPro = m_TextMeshPro;
		m_isHoveringObject = false;
		RectTransform rectTransform = textMeshPro.rectTransform;
		Vector3 mousePosition = Input.mousePosition;
		Camera main = Camera.main;
		float z = mousePosition.z;
		Vector3 position = new Vector3(mousePosition.x, mousePosition.y, z);
		if (TMP_TextUtilities.IsIntersectingRectTransform(rectTransform, position, main))
		{
			m_isHoveringObject = true;
		}
		if (!m_isHoveringObject)
		{
			return;
		}
		TextMeshPro textMeshPro2 = m_TextMeshPro;
		Vector3 mousePosition2 = Input.mousePosition;
		main = Camera.main;
		z = mousePosition2.z;
		Vector3 position2 = new Vector3(mousePosition2.x, mousePosition2.y, z);
		int num = TMP_TextUtilities.FindIntersectingCharacter(textMeshPro2, position2, main, visibleOnly: true);
		long num2 = num;
		if (num != -1 && (int)num2 != m_lastCharIndex && (Input.GetKeyInt(KeyCode.LeftShift) || Input.GetKeyInt(KeyCode.RightShift)))
		{
			_ = m_TextMeshPro;
			m_lastCharIndex = (int)num2;
			Random.Range(0, 255);
			Random.Range(0, 255);
			Random.Range(0, 255);
			_ = m_TextMeshPro;
			NativeIntrinsics.Stmt("mov [rdx+rbx*4+20h],eax ; value = LocalExpr");
			NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = LocalExpr");
			NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = LocalExpr");
			NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = LocalExpr");
			_ = ((TMP_Text)m_TextMeshPro).m_textInfo.meshInfo;
			NativeIntrinsics.Reinterpret<Mesh>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)")).colors32 = NativeIntrinsics.Reinterpret<Color32[]>(NativeIntrinsics.Asm("*(FieldExpr + 0x58)"));
		}
		TextMeshPro textMeshPro3 = m_TextMeshPro;
		Vector3 mousePosition3 = Input.mousePosition;
		main = m_Camera;
		z = mousePosition3.z;
		Vector3 position3 = new Vector3(mousePosition3.x, mousePosition3.y, z);
		int num3 = TMP_TextUtilities.FindIntersectingLink(textMeshPro3, position3, main);
		int num4 = num3;
		if ((num3 == -1 && m_selectedLink != num4) || num4 != m_selectedLink)
		{
			m_selectedLink = -1;
		}
		if (num4 != -1 && num4 != m_selectedLink)
		{
			TextMeshPro textMeshPro4 = m_TextMeshPro;
			m_selectedLink = num4;
			_ = ((TMP_Text)textMeshPro4).m_textInfo.linkInfo;
			string[] array = new string[5];
			string[] array2 = array;
			array[0] = "Link ID: \"";
			TMP_LinkInfo tMP_LinkInfo = default(TMP_LinkInfo);
			tMP_LinkInfo.textComponent = NativeIntrinsics.Reinterpret<TMP_Text>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)"));
			tMP_LinkInfo.linkIdLength = (int)NativeIntrinsics.Asm("*(LocalExpr + 0x30)");
			tMP_LinkInfo.linkID = NativeIntrinsics.Reinterpret<char[]>(NativeIntrinsics.Asm("*(LocalExpr + 0x40)"));
			array2[1] = tMP_LinkInfo.GetLinkID();
			array2[2] = "\"   Link Text: \"";
			array2[3] = tMP_LinkInfo.GetLinkText();
			array2[4] = "\"";
			Debug.Log(string.Concat(array2));
			TextMeshPro textMeshPro5 = m_TextMeshPro;
			_ = Vector3.zeroVector.z;
			RectTransform rectTransform2 = textMeshPro5.rectTransform;
			Vector3 mousePosition4 = Input.mousePosition;
			float x = mousePosition4.x;
			float y = mousePosition4.y;
			RectTransformUtility.ScreenPointToWorldPointInRectangle(cam: m_Camera, rect: rectTransform2, screenPoint: new Vector2(x, y), worldPoint: out var _);
			string linkID = tMP_LinkInfo.GetLinkID();
			bool flag = linkID == "id_01";
			if (!flag)
			{
				linkID == "id_02";
			}
		}
		TextMeshPro textMeshPro6 = m_TextMeshPro;
		Vector3 mousePosition5 = Input.mousePosition;
		main = Camera.main;
		Vector3 position4 = new Vector3(mousePosition5.x, mousePosition5.y, mousePosition5.z);
		int num5 = TMP_TextUtilities.FindIntersectingWord(textMeshPro6, position4, main);
		num4 = num5;
		if (num5 == -1 || num4 == m_lastWordIndex)
		{
			return;
		}
		_ = m_TextMeshPro;
		m_lastWordIndex = num4;
		Transform transform = m_TextMeshPro.transform;
		_ = (int)NativeIntrinsics.Asm("*(FieldExpr + 0x28)");
		int num6 = (int)NativeIntrinsics.Asm("*(FieldExpr + 0x30)");
		Vector3 position5 = default(Vector3);
		position5.x = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(BinaryExpr + 0x11C)"));
		position5.z = NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(BinaryExpr + 0x124)"));
		Vector3 vector = transform.TransformPoint(position5);
		Camera main2 = Camera.main;
		Vector3 position6 = new Vector3(vector.x, vector.y, vector.z);
		main2.WorldToScreenPoint(position6);
		_ = ((TMP_Text)m_TextMeshPro).m_textInfo.meshInfo;
		Random.Range(0, 255);
		Random.Range(0, 255);
		Random.Range(0, 255);
		TMP_TextSelector_A tMP_TextSelector_A = this;
		long num7 = NativeIntrinsics.Asm("*(LocalExpr + 0x58)");
		if (num6 > 0)
		{
			tMP_TextSelector_A = this;
			int num8 = 0;
			NativeIntrinsics.Asm("stack[0x8]");
			do
			{
				NativeIntrinsics.Stmt("mov [rbx+r8*4+20h],eax ; value = LocalExpr");
				NativeIntrinsics.Stmt("mov [rbx+rcx*4+20h],eax ; value = LocalExpr");
				NativeIntrinsics.Stmt("mov [rbx+rcx*4+20h],eax ; value = LocalExpr");
				NativeIntrinsics.Stmt("mov [rbx+rcx*4+20h],eax ; value = LocalExpr");
				num8++;
			}
			while (num8 + 1 < num6);
		}
		tMP_TextSelector_A.m_TextMeshPro.mesh.colors32 = NativeIntrinsics.Reinterpret<Color32[]>(num7);
	}

	// Token: 0x06000251 RID: 593 RVA: 0x0025CD80 File Offset: 0x0025B380
	public void OnPointerEnter(PointerEventData eventData)
	{
		Debug.Log("OnPointerEnter()");
		m_isHoveringObject = true;
	}

	// Token: 0x06000252 RID: 594 RVA: 0x0025CDE0 File Offset: 0x0025B3E0
	public void OnPointerExit(PointerEventData eventData)
	{
		Debug.Log("OnPointerExit()");
		m_isHoveringObject = false;
	}
}

