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
		GameObject gameObject = this.gameObject;
		TextMeshPro component = gameObject.GetComponent<TextMeshPro>();
		m_TextMeshPro = component;
		Camera main = Camera.main;
		m_Camera = main;
		throw null;
	}

	// Token: 0x06000250 RID: 592 RVA: 0x0025C4D0 File Offset: 0x0025AAD0
	private unsafe void LateUpdate()
	{
		TextMeshPro textMeshPro = m_TextMeshPro;
		m_isHoveringObject = false;
		TextMeshPro textMeshPro2 = textMeshPro;
		RectTransform rectTransform = textMeshPro2.rectTransform;
		Vector3 mousePosition = Input.mousePosition;
		Camera main = Camera.main;
		long num = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
		Camera camera = main;
		float z = mousePosition.z;
		RectTransform rectTransform2 = rectTransform;
		bool flag = TMP_TextUtilities.IsIntersectingRectTransform(rectTransform2, NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -41L), camera);
		bool flag2 = flag;
		if (flag)
		{
			m_isHoveringObject = true;
		}
		if (!m_isHoveringObject)
		{
			return;
		}
		TextMeshPro textMeshPro3 = m_TextMeshPro;
		Vector3 mousePosition2 = Input.mousePosition;
		Camera main2 = Camera.main;
		num = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
		camera = main2;
		z = mousePosition2.z;
		TextMeshPro text = textMeshPro3;
		int num2 = TMP_TextUtilities.FindIntersectingCharacter(text, NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -41L), camera, visibleOnly: true);
		long num3 = num2;
		if (num2 != -1 && (int)num3 != m_lastCharIndex && (Input.GetKeyInt(NativeIntrinsics.Reinterpret<KeyCode>(304)) || Input.GetKeyInt(NativeIntrinsics.Reinterpret<KeyCode>(303))))
		{
			TextMeshPro textMeshPro4 = m_TextMeshPro;
			m_lastCharIndex = (int)num3;
			int num4 = Random.Range(0, 255);
			int num5 = Random.Range(0, 255);
			int num6 = Random.Range(0, 255);
			TextMeshPro textMeshPro5 = m_TextMeshPro;
			NativeIntrinsics.Stmt("mov [rdx+rbx*4+20h],eax ; value = LocalExpr");
			NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = LocalExpr");
			NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = LocalExpr");
			NativeIntrinsics.Stmt("mov [rdx+rcx*4+20h],eax ; value = LocalExpr");
			TextMeshPro textMeshPro6 = m_TextMeshPro;
			TMP_TextInfo textInfo = ((TMP_Text)textMeshPro6).m_textInfo;
			TMP_MeshInfo[] meshInfo = textInfo.meshInfo;
			NativeIntrinsics.Reinterpret<Mesh>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)")).colors32 = NativeIntrinsics.Reinterpret<Color32[]>(NativeIntrinsics.Asm("*(FieldExpr + 0x58)"));
		}
		TextMeshPro textMeshPro7 = m_TextMeshPro;
		Vector3 mousePosition3 = Input.mousePosition;
		num = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
		camera = m_Camera;
		z = mousePosition3.z;
		TextMeshPro text2 = textMeshPro7;
		int num7 = TMP_TextUtilities.FindIntersectingLink(text2, NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -41L), camera);
		int num8 = num7;
		if ((num7 == -1 && m_selectedLink != num8) || num8 != m_selectedLink)
		{
			m_selectedLink = -1;
		}
		if (num8 != -1 && num8 != m_selectedLink)
		{
			TextMeshPro textMeshPro8 = m_TextMeshPro;
			m_selectedLink = num8;
			textMeshPro2 = textMeshPro8;
			TMP_TextInfo textInfo2 = ((TMP_Text)textMeshPro2).m_textInfo;
			TMP_LinkInfo[] linkInfo = textInfo2.linkInfo;
			string[] array = new string[5];
			string[] array2 = array;
			array[0] = "Link ID: \"";
			string linkID = ((TMP_LinkInfo*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -9L))->GetLinkID();
			array2[1] = linkID;
			array2[2] = "\"   Link Text: \"";
			string linkText = ((TMP_LinkInfo*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -9L))->GetLinkText();
			array2[3] = linkText;
			array2[4] = "\"";
			string text3 = string.Concat(array2);
			string message = text3;
			Debug.Log(message);
			TextMeshPro textMeshPro9 = m_TextMeshPro;
			float z2 = Vector3.zeroVector.z;
			long num9 = NativeIntrinsics.Asm("struct_chunk(StaticFieldExpr+0x0, 8)");
			textMeshPro2 = textMeshPro9;
			RectTransform rectTransform3 = textMeshPro2.rectTransform;
			Vector3 mousePosition4 = Input.mousePosition;
			float x = mousePosition4.x;
			float y = mousePosition4.y;
			Camera camera2 = m_Camera;
			RectTransform rect = rectTransform3;
			bool flag3 = RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, NativeIntrinsics.Reinterpret<Vector2>(NativeIntrinsics.Asm("unpcklps xmm6,xmm7")), camera2, out *(Vector3*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -57L));
			string linkID2 = ((TMP_LinkInfo*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -9L))->GetLinkID();
			bool flag4 = string.Equals(linkID2, "id_01");
			string a = linkID2;
			if (!flag4)
			{
				bool flag5 = string.Equals(a, "id_02");
			}
		}
		TextMeshPro textMeshPro10 = m_TextMeshPro;
		Vector3 mousePosition5 = Input.mousePosition;
		Camera main3 = Camera.main;
		long num10 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
		camera = main3;
		float z3 = mousePosition5.z;
		TextMeshPro text4 = textMeshPro10;
		int num11 = TMP_TextUtilities.FindIntersectingWord(text4, NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -41L), camera);
		num8 = num11;
		if (num11 == -1 || num8 == m_lastWordIndex)
		{
			return;
		}
		TextMeshPro textMeshPro11 = m_TextMeshPro;
		m_lastWordIndex = num8;
		Transform transform = m_TextMeshPro.transform;
		long num12 = (int)NativeIntrinsics.Asm("*(FieldExpr + 0x28)");
		int num13 = (int)NativeIntrinsics.Asm("*(FieldExpr + 0x30)");
		Vector3 vector = transform.TransformPoint(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -41L));
		Camera main4 = Camera.main;
		float z4 = vector.z;
		Vector3 vector2 = main4.WorldToScreenPoint(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -41L));
		TextMeshPro textMeshPro12 = m_TextMeshPro;
		TMP_TextInfo textInfo3 = ((TMP_Text)textMeshPro12).m_textInfo;
		TMP_MeshInfo[] meshInfo2 = textInfo3.meshInfo;
		int num14 = Random.Range(0, 255);
		int num15 = Random.Range(0, 255);
		int num16 = Random.Range(0, 255);
		long num17 = NativeIntrinsics.Asm("*(LocalExpr + 0x58)");
		if (num13 > 0)
		{
			num8 = (int)NativeIntrinsics.Asm("stack[0x8]");
			do
			{
				NativeIntrinsics.Stmt("mov [rbx+r8*4+20h],eax ; value = LocalExpr");
				NativeIntrinsics.Stmt("mov [rbx+rcx*4+20h],eax ; value = LocalExpr");
				NativeIntrinsics.Stmt("mov [rbx+rcx*4+20h],eax ; value = LocalExpr");
				NativeIntrinsics.Stmt("mov [rbx+rcx*4+20h],eax ; value = LocalExpr");
				int num18 = 0 + 1;
			}
			while (0 + 1 < num13);
		}
		NativeIntrinsics.Reinterpret<Mesh>((nint)NativeIntrinsics.Asm("FieldExpr->klass")).colors32 = NativeIntrinsics.Reinterpret<Color32[]>(num17);
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

