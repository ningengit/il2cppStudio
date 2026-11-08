using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using Mono.Globalization.Unicode;
using Mono.Xml;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x02000079 RID: 121
public class TextMeshProFloatingText : MonoBehaviour
{
	// Token: 0x0200007A RID: 122
	private sealed class <DisplayTextMeshFloatingText>d__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x0400031A RID: 794 Offset: 0x10
		private int <>1__state;

		// Token: 0x0400031B RID: 795 Offset: 0x18
		private object <>2__current;

		// Token: 0x0400031C RID: 796 Offset: 0x20
		public TextMeshProFloatingText <>4__this;

		// Token: 0x0400031D RID: 797 Offset: 0x28
		private float <CountDuration>5__2;

		// Token: 0x0400031E RID: 798 Offset: 0x2C
		private float <starting_Count>5__3;

		// Token: 0x0400031F RID: 799 Offset: 0x30
		private float <current_Count>5__4;

		// Token: 0x04000320 RID: 800 Offset: 0x34
		private Vector3 <start_pos>5__5;

		// Token: 0x04000321 RID: 801 Offset: 0x40
		private Color32 <start_color>5__6;

		// Token: 0x04000322 RID: 802 Offset: 0x44
		private float <alpha>5__7;

		// Token: 0x04000323 RID: 803 Offset: 0x48
		private float <fadeDuration>5__8;

		private object System.Collections.Generic.IEnumerator<System.Object>.Current => <>2__current;

		private object System.Collections.IEnumerator.Current => <>2__current;

		// Token: 0x0600022C RID: 556 RVA: 0x00250B50 File Offset: 0x0024F150
		public <DisplayTextMeshFloatingText>d__13(int <>1__state)
		{
			((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
			this.<>1__state = <>1__state;
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00250B40 File Offset: 0x0024F140
		private void System.IDisposable.Dispose()
		{
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00260DE0 File Offset: 0x0025F3E0
		private unsafe bool MoveNext()
		{
			int num = <>1__state;
			TextMeshProFloatingText textMeshProFloatingText = <>4__this;
			int num2 = num;
			TextMeshProFloatingText textMeshProFloatingText2 = textMeshProFloatingText;
			if (num != 0)
			{
				num2--;
				if (num2 - 1 != 0)
				{
					if (num2 == 1)
					{
						<>1__state = -1;
						Transform floatingText_Transform = textMeshProFloatingText2.m_floatingText_Transform;
						float z = <start_pos>5__5.z;
						floatingText_Transform.position = NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -41L);
						IEnumerator routine = textMeshProFloatingText2.DisplayTextMeshFloatingText();
						Coroutine coroutine = textMeshProFloatingText2.StartCoroutine(routine);
					}
					goto IL_05ad;
				}
				<>1__state = -1;
			}
			else
			{
				<>1__state = -1;
				<CountDuration>5__2 = 2f;
				<current_Count>5__4 = (<starting_Count>5__3 = UnityEngine.Random.Range(5f, 20f));
				Transform floatingText_Transform2 = textMeshProFloatingText2.m_floatingText_Transform;
				Vector3 position = floatingText_Transform2.position;
				<start_pos>5__5.x = position.x;
				<start_pos>5__5.y = position.y;
				<start_pos>5__5.z = position.z;
				float z2 = position.z;
				TextMesh textMesh = textMeshProFloatingText2.m_textMesh;
				Color color = textMesh.color;
				Color32 value = NativeIntrinsics.Reinterpret<Color>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -25L);
				<start_color>5__6 = NativeIntrinsics.Reinterpret<Color32>(NativeIntrinsics.Reinterpret<int>(value));
				float num3 = 3f / <starting_Count>5__3;
				<alpha>5__7 = 255f;
				<fadeDuration>5__8 = num3 * <CountDuration>5__2;
			}
			float num4 = <current_Count>5__4;
			if (<current_Count>5__4 > 0f)
			{
				float deltaTime = Time.deltaTime;
				float num5 = deltaTime / <CountDuration>5__2 * <starting_Count>5__3;
				<current_Count>5__4 = num4 - deltaTime / <CountDuration>5__2 * <starting_Count>5__3;
				num4 = num5;
				if (!(3f < num4 - deltaTime / <CountDuration>5__2 * <starting_Count>5__3))
				{
					float num6 = <alpha>5__7;
					float deltaTime2 = Time.deltaTime;
					num4 = deltaTime2 / <fadeDuration>5__8 * 255f;
					float num7 = num6 - deltaTime2 / <fadeDuration>5__8 * 255f;
					if (0f > num6 - deltaTime2 / <fadeDuration>5__8 * 255f || num7 > 255f)
					{
					}
					<alpha>5__7 = 0f;
				}
				TextMesh textMesh2 = textMeshProFloatingText2.m_textMesh;
				string text = ((int*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + 103L))->ToString();
				textMesh2.text = text;
				TextMesh textMesh3 = textMeshProFloatingText2.m_textMesh;
				num4 = (float)<start_color>5__6.rgba / 255f;
				float num8 = (float)(int)<start_color>5__6.g / 255f;
				float num9 = (float)(int)<start_color>5__6.b / 255f;
				float num10 = (float)(int)<alpha>5__7 / 255f;
				textMesh3.color = NativeIntrinsics.Reinterpret<Color>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -25L);
				Transform floatingText_Transform3 = textMeshProFloatingText2.m_floatingText_Transform;
				Vector3 position2 = floatingText_Transform3.position;
				float num11 = <starting_Count>5__3;
				float deltaTime3 = Time.deltaTime;
				floatingText_Transform3.position = NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -41L);
				float z3 = textMeshProFloatingText2.lastPOS.z;
				Vector3 position3 = textMeshProFloatingText2.m_cameraTransform.position;
				Transform cameraTransform;
				if (TMPro_ExtensionMethods.Compare(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -25L), NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -41L), 1000))
				{
					cameraTransform = textMeshProFloatingText2.m_cameraTransform;
					Quaternion lastRotation = textMeshProFloatingText2.lastRotation;
					Quaternion rotation = cameraTransform.rotation;
					if (TMPro_ExtensionMethods.Compare(NativeIntrinsics.Reinterpret<Quaternion>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -41L), NativeIntrinsics.Reinterpret<Quaternion>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -25L), 1000))
					{
						goto IL_0595;
					}
				}
				cameraTransform = textMeshProFloatingText2.m_cameraTransform;
				Vector3 position4 = cameraTransform.position;
				Transform cameraTransform2 = textMeshProFloatingText2.m_cameraTransform;
				textMeshProFloatingText2.lastPOS.x = position4.x;
				textMeshProFloatingText2.lastPOS.y = position4.y;
				textMeshProFloatingText2.lastPOS.z = position4.z;
				cameraTransform = cameraTransform2;
				long num12 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
				Quaternion rotation2 = cameraTransform.rotation;
				Transform floatingText_Transform4 = textMeshProFloatingText2.m_floatingText_Transform;
				textMeshProFloatingText2.lastRotation.x = rotation2.x;
				textMeshProFloatingText2.lastRotation.y = rotation2.y;
				textMeshProFloatingText2.lastRotation.z = rotation2.z;
				textMeshProFloatingText2.lastRotation.w = rotation2.w;
				num12 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
				floatingText_Transform4.rotation = NativeIntrinsics.Reinterpret<Quaternion>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -25L);
				cameraTransform = textMeshProFloatingText2.m_transform;
				Vector3 position5 = cameraTransform.position;
				float num13 = position5.x - textMeshProFloatingText2.lastPOS.x;
				num8 = position5.z - textMeshProFloatingText2.lastPOS.z;
				Transform transform = textMeshProFloatingText2.m_transform;
				transform.forward = NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -41L);
				goto IL_0595;
			}
			float seconds = UnityEngine.Random.Range(0.1f, 1f);
			WaitForSeconds waitForSeconds = new WaitForSeconds(seconds);
			<>2__current = waitForSeconds;
			<>1__state = 2;
			goto IL_05ad;
			IL_05ad:
			return true;
			IL_0595:
			WaitForEndOfFrame waitForEndOfFrame = (WaitForEndOfFrame)(object)new Mono.Globalization.Unicode.ContractionComparer();
			<>2__current = waitForEndOfFrame;
			<>1__state = 1;
			goto IL_05ad;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x002612F0 File Offset: 0x0025F8F0
		private void System.Collections.IEnumerator.Reset()
		{
			long num = NativeIntrinsics.CallNative("sub_184BE0", NativeIntrinsics.Asm("rax"), 0L, 0L, 0L);
			NativeIntrinsics.Reinterpret<NotSupportedException>(num)..ctor();
			throw null;
		}
	}

	// Token: 0x0200007B RID: 123
	private sealed class <DisplayTextMeshProFloatingText>d__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x04000324 RID: 804 Offset: 0x10
		private int <>1__state;

		// Token: 0x04000325 RID: 805 Offset: 0x18
		private object <>2__current;

		// Token: 0x04000326 RID: 806 Offset: 0x20
		public TextMeshProFloatingText <>4__this;

		// Token: 0x04000327 RID: 807 Offset: 0x28
		private float <CountDuration>5__2;

		// Token: 0x04000328 RID: 808 Offset: 0x2C
		private float <starting_Count>5__3;

		// Token: 0x04000329 RID: 809 Offset: 0x30
		private float <current_Count>5__4;

		// Token: 0x0400032A RID: 810 Offset: 0x34
		private Vector3 <start_pos>5__5;

		// Token: 0x0400032B RID: 811 Offset: 0x40
		private Color32 <start_color>5__6;

		// Token: 0x0400032C RID: 812 Offset: 0x44
		private float <alpha>5__7;

		// Token: 0x0400032D RID: 813 Offset: 0x48
		private float <fadeDuration>5__8;

		private object System.Collections.Generic.IEnumerator<System.Object>.Current => <>2__current;

		private object System.Collections.IEnumerator.Current => <>2__current;

		// Token: 0x06000232 RID: 562 RVA: 0x00250B50 File Offset: 0x0024F150
		public <DisplayTextMeshProFloatingText>d__12(int <>1__state)
		{
			((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
			this.<>1__state = <>1__state;
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00250B40 File Offset: 0x0024F140
		private void System.IDisposable.Dispose()
		{
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00261330 File Offset: 0x0025F930
		private unsafe bool MoveNext()
		{
			int num = <>1__state;
			TextMeshProFloatingText textMeshProFloatingText = <>4__this;
			int num2 = num;
			TextMeshProFloatingText textMeshProFloatingText2 = textMeshProFloatingText;
			if (num != 0)
			{
				num2--;
				if (num2 - 1 != 0)
				{
					if (num2 == 1)
					{
						<>1__state = -1;
						Transform floatingText_Transform = textMeshProFloatingText2.m_floatingText_Transform;
						float z = <start_pos>5__5.z;
						floatingText_Transform.position = NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -41L);
						IEnumerator routine = textMeshProFloatingText2.DisplayTextMeshProFloatingText();
						Coroutine coroutine = textMeshProFloatingText2.StartCoroutine(routine);
					}
					goto IL_056d;
				}
				<>1__state = -1;
			}
			else
			{
				<>1__state = -1;
				<CountDuration>5__2 = 2f;
				<current_Count>5__4 = (<starting_Count>5__3 = UnityEngine.Random.Range(5f, 20f));
				Transform floatingText_Transform2 = textMeshProFloatingText2.m_floatingText_Transform;
				Vector3 position = floatingText_Transform2.position;
				<start_pos>5__5.x = position.x;
				<start_pos>5__5.y = position.y;
				<start_pos>5__5.z = position.z;
				float z2 = position.z;
				TextMeshPro textMeshPro = textMeshProFloatingText2.m_textMeshPro;
				Color32 value = NativeIntrinsics.Reinterpret<Color>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -25L);
				<start_color>5__6 = NativeIntrinsics.Reinterpret<Color32>(NativeIntrinsics.Reinterpret<int>(value));
				float num3 = 3f / <starting_Count>5__3;
				<alpha>5__7 = 255f;
				<fadeDuration>5__8 = num3 * <CountDuration>5__2;
			}
			float num4 = <current_Count>5__4;
			if (<current_Count>5__4 > 0f)
			{
				float deltaTime = Time.deltaTime;
				float num5 = deltaTime / <CountDuration>5__2 * <starting_Count>5__3;
				<current_Count>5__4 = num4 - deltaTime / <CountDuration>5__2 * <starting_Count>5__3;
				num4 = num5;
				if (!(3f < num4 - deltaTime / <CountDuration>5__2 * <starting_Count>5__3))
				{
					float num6 = <alpha>5__7;
					float deltaTime2 = Time.deltaTime;
					num4 = deltaTime2 / <fadeDuration>5__8 * 255f;
					float num7 = num6 - deltaTime2 / <fadeDuration>5__8 * 255f;
					if (0f > num6 - deltaTime2 / <fadeDuration>5__8 * 255f || num7 > 255f)
					{
					}
					<alpha>5__7 = 0f;
				}
				TextMeshPro textMeshPro2 = textMeshProFloatingText2.m_textMeshPro;
				string text = ((int*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + 103L))->ToString();
				num4 = (float)<start_color>5__6.rgba / 255f;
				float num8 = (float)(int)<start_color>5__6.g / 255f;
				float num9 = (float)(int)<start_color>5__6.b / 255f;
				float num10 = (float)(int)<alpha>5__7 / 255f;
				Transform floatingText_Transform3 = textMeshProFloatingText2.m_floatingText_Transform;
				Vector3 position2 = floatingText_Transform3.position;
				float num11 = <starting_Count>5__3;
				float deltaTime3 = Time.deltaTime;
				floatingText_Transform3.position = NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -41L);
				float z3 = textMeshProFloatingText2.lastPOS.z;
				Vector3 position3 = textMeshProFloatingText2.m_cameraTransform.position;
				Transform cameraTransform;
				if (TMPro_ExtensionMethods.Compare(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -25L), NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -41L), 1000))
				{
					cameraTransform = textMeshProFloatingText2.m_cameraTransform;
					Quaternion lastRotation = textMeshProFloatingText2.lastRotation;
					Quaternion rotation = cameraTransform.rotation;
					if (TMPro_ExtensionMethods.Compare(NativeIntrinsics.Reinterpret<Quaternion>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -41L), NativeIntrinsics.Reinterpret<Quaternion>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -25L), 1000))
					{
						goto IL_0555;
					}
				}
				cameraTransform = textMeshProFloatingText2.m_cameraTransform;
				Vector3 position4 = cameraTransform.position;
				Transform cameraTransform2 = textMeshProFloatingText2.m_cameraTransform;
				textMeshProFloatingText2.lastPOS.x = position4.x;
				textMeshProFloatingText2.lastPOS.y = position4.y;
				textMeshProFloatingText2.lastPOS.z = position4.z;
				cameraTransform = cameraTransform2;
				long num12 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
				Quaternion rotation2 = cameraTransform.rotation;
				Transform floatingText_Transform4 = textMeshProFloatingText2.m_floatingText_Transform;
				textMeshProFloatingText2.lastRotation.x = rotation2.x;
				textMeshProFloatingText2.lastRotation.y = rotation2.y;
				textMeshProFloatingText2.lastRotation.z = rotation2.z;
				textMeshProFloatingText2.lastRotation.w = rotation2.w;
				num12 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
				floatingText_Transform4.rotation = NativeIntrinsics.Reinterpret<Quaternion>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -25L);
				cameraTransform = textMeshProFloatingText2.m_transform;
				Vector3 position5 = cameraTransform.position;
				float num13 = position5.x - textMeshProFloatingText2.lastPOS.x;
				num8 = position5.z - textMeshProFloatingText2.lastPOS.z;
				Transform transform = textMeshProFloatingText2.m_transform;
				transform.forward = NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -41L);
				goto IL_0555;
			}
			float seconds = UnityEngine.Random.Range(0.1f, 1f);
			WaitForSeconds waitForSeconds = new WaitForSeconds(seconds);
			<>2__current = waitForSeconds;
			<>1__state = 2;
			goto IL_056d;
			IL_056d:
			return true;
			IL_0555:
			WaitForEndOfFrame waitForEndOfFrame = (WaitForEndOfFrame)(object)new Mono.Globalization.Unicode.ContractionComparer();
			<>2__current = waitForEndOfFrame;
			<>1__state = 1;
			goto IL_056d;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00261850 File Offset: 0x0025FE50
		private void System.Collections.IEnumerator.Reset()
		{
			long num = NativeIntrinsics.CallNative("sub_184BE0", NativeIntrinsics.Asm("rax"), 0L, 0L, 0L);
			NativeIntrinsics.Reinterpret<NotSupportedException>(num)..ctor();
			throw null;
		}
	}

	// Token: 0x04000310 RID: 784 Offset: 0x20
	public Font TheFont;

	// Token: 0x04000311 RID: 785 Offset: 0x28
	private GameObject m_floatingText;

	// Token: 0x04000312 RID: 786 Offset: 0x30
	private TextMeshPro m_textMeshPro;

	// Token: 0x04000313 RID: 787 Offset: 0x38
	private TextMesh m_textMesh;

	// Token: 0x04000314 RID: 788 Offset: 0x40
	private Transform m_transform;

	// Token: 0x04000315 RID: 789 Offset: 0x48
	private Transform m_floatingText_Transform;

	// Token: 0x04000316 RID: 790 Offset: 0x50
	private Transform m_cameraTransform;

	// Token: 0x04000317 RID: 791 Offset: 0x58
	private Vector3 lastPOS;

	// Token: 0x04000318 RID: 792 Offset: 0x64
	private Quaternion lastRotation;

	// Token: 0x04000319 RID: 793 Offset: 0x74
	public int SpawnType;

	// Token: 0x06000227 RID: 551 RVA: 0x0025FF90 File Offset: 0x0025E590
	private void Awake()
	{
		Transform transform = this.transform;
		m_transform = transform;
		string text = name;
		string text2 = text + " floating text";
		GameObject floatingText = new GameObject(text2);
		m_floatingText = floatingText;
		Camera main = Camera.main;
		Transform cameraTransform = main.transform;
		m_cameraTransform = cameraTransform;
	}

	// Token: 0x06000228 RID: 552 RVA: 0x00260150 File Offset: 0x0025E750
	private void Start()
	{
		IEnumerator routine = default(IEnumerator);
		if (SpawnType != 0)
		{
			if (SpawnType != 1)
			{
				return;
			}
			Transform floatingText_Transform = m_floatingText.transform;
			m_floatingText_Transform = floatingText_Transform;
			Transform transform = m_transform;
			Transform floatingText_Transform2 = m_floatingText_Transform;
			Vector3 position = transform.position;
			float z = 0f + position.z;
			float x = 0f + position.x;
			float y = 15f + position.y;
			Vector3 position2 = default(Vector3);
			position2.x = x;
			position2.y = y;
			position2.z = z;
			floatingText_Transform2.position = position2;
			TextMesh textMesh = m_floatingText.AddComponent<TextMesh>();
			m_textMesh = textMesh;
			TextMesh textMesh2 = m_textMesh;
			Font font = Resources.Load<Font>("Fonts/ARIAL");
			textMesh2.font = font;
			Renderer component = m_textMesh.GetComponent<Renderer>();
			Font font2 = m_textMesh.font;
			Material material = font2.material;
			component.SetMaterial(material);
			TextMesh textMesh3 = m_textMesh;
			int num = UnityEngine.Random.Range(0, 255);
			int num2 = UnityEngine.Random.Range(0, 255);
			int num3 = UnityEngine.Random.Range(0, 255);
			int num4 = num3;
			z = (float)num3 / 255f;
			Color color = default(Color);
			color.r = (float)num / 255f;
			color.g = (float)num2 / 255f;
			color.b = z;
			color.a = 1f;
			textMesh3.color = color;
			m_textMesh.anchor = NativeIntrinsics.Reinterpret<TextAnchor>(7);
			m_textMesh.fontSize = 24;
			IEnumerator enumerator = DisplayTextMeshFloatingText();
			routine = enumerator;
		}
		else
		{
			TextMeshPro textMeshPro = m_floatingText.AddComponent<TextMeshPro>();
			m_textMeshPro = textMeshPro;
			RectTransform rectTransform = m_textMeshPro.rectTransform;
			rectTransform.sizeDelta = NativeIntrinsics.Reinterpret<Vector2>(NativeIntrinsics.Asm("unpcklps xmm0,xmm0"));
			Transform floatingText_Transform3 = m_floatingText.transform;
			m_floatingText_Transform = floatingText_Transform3;
			Transform transform2 = m_transform;
			Transform floatingText_Transform4 = m_floatingText_Transform;
			Vector3 position3 = transform2.position;
			float z = 0f + position3.z;
			float x = 0f + position3.x;
			float y = 15f + position3.y;
			Vector3 position4 = default(Vector3);
			position4.x = x;
			position4.y = y;
			position4.z = z;
			floatingText_Transform4.position = position4;
			m_textMeshPro.alignment = NativeIntrinsics.Reinterpret<TextAlignmentOptions>(514);
			TextMeshPro textMeshPro2 = m_textMeshPro;
			int num5 = UnityEngine.Random.Range(0, 255);
			int num6 = UnityEngine.Random.Range(0, 255);
			int num7 = UnityEngine.Random.Range(0, 255);
			z = (float)num7 / 255f;
			m_textMeshPro.fontSize = 24f;
			m_textMeshPro.enableKerning = false;
			<DisplayTextMeshProFloatingText>d__12 <DisplayTextMeshProFloatingText>d__ = NativeIntrinsics.Reinterpret<<DisplayTextMeshProFloatingText>d__12>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.TextMeshProFloatingText+<DisplayTextMeshProFloatingText>d__12)"));
			((Mono.Xml.SecurityParser)(object)<DisplayTextMeshProFloatingText>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
			<DisplayTextMeshProFloatingText>d__.<>1__state = 0;
			<DisplayTextMeshProFloatingText>d__.<>4__this = this;
			<DisplayTextMeshProFloatingText>d__12 <DisplayTextMeshProFloatingText>d__2 = <DisplayTextMeshProFloatingText>d__;
		}
		Coroutine coroutine = StartCoroutine(routine);
		Coroutine coroutine2 = coroutine;
	}

	// Token: 0x06000229 RID: 553 RVA: 0x002600E0 File Offset: 0x0025E6E0
	public IEnumerator DisplayTextMeshProFloatingText()
	{
		<DisplayTextMeshProFloatingText>d__12 <DisplayTextMeshProFloatingText>d__ = NativeIntrinsics.Reinterpret<<DisplayTextMeshProFloatingText>d__12>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.TextMeshProFloatingText+<DisplayTextMeshProFloatingText>d__12)"));
		((Mono.Xml.SecurityParser)(object)<DisplayTextMeshProFloatingText>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<DisplayTextMeshProFloatingText>d__.<>1__state = 0;
		<DisplayTextMeshProFloatingText>d__.<>4__this = this;
		return <DisplayTextMeshProFloatingText>d__;
	}

	// Token: 0x0600022A RID: 554 RVA: 0x00260070 File Offset: 0x0025E670
	public IEnumerator DisplayTextMeshFloatingText()
	{
		<DisplayTextMeshFloatingText>d__13 <DisplayTextMeshFloatingText>d__ = NativeIntrinsics.Reinterpret<<DisplayTextMeshFloatingText>d__13>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(TMPro.Examples.TextMeshProFloatingText+<DisplayTextMeshFloatingText>d__13)"));
		((Mono.Xml.SecurityParser)(object)<DisplayTextMeshFloatingText>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<DisplayTextMeshFloatingText>d__.<>1__state = 0;
		<DisplayTextMeshFloatingText>d__.<>4__this = this;
		return <DisplayTextMeshFloatingText>d__;
	}

	// Token: 0x0600022B RID: 555 RVA: 0x00260690 File Offset: 0x0025EC90
	public TextMeshProFloatingText()
	{
		float z = Vector3.zeroVector.z;
		lastPOS.x = Vector3.zeroVector.x;
		lastPOS.y = Vector3.zeroVector.y;
		lastPOS.z = z;
		lastRotation = Quaternion.identityQuaternion;
		((Behaviour)this)..ctor();
	}
}

