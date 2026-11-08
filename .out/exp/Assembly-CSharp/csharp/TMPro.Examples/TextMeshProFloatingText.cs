using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x02000079 RID: 121
public class TextMeshProFloatingText : MonoBehaviour
{
	// Token: 0x0200007A RID: 122
	private sealed class _DisplayTextMeshFloatingText_d__13 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x0400031A RID: 794 Offset: 0x10
		// Il2CppStudio: declared private; widened because native code inlined into other types accesses it.
		internal int __1__state;

		// Token: 0x0400031B RID: 795 Offset: 0x18
		private object __2__current;

		// Token: 0x0400031C RID: 796 Offset: 0x20
		public TextMeshProFloatingText __4__this;

		// Token: 0x0400031D RID: 797 Offset: 0x28
		private float _CountDuration_5__2;

		// Token: 0x0400031E RID: 798 Offset: 0x2C
		private float _starting_Count_5__3;

		// Token: 0x0400031F RID: 799 Offset: 0x30
		private float _current_Count_5__4;

		// Token: 0x04000320 RID: 800 Offset: 0x34
		private Vector3 _start_pos_5__5;

		// Token: 0x04000321 RID: 801 Offset: 0x40
		private Color32 _start_color_5__6;

		// Token: 0x04000322 RID: 802 Offset: 0x44
		private float _alpha_5__7;

		// Token: 0x04000323 RID: 803 Offset: 0x48
		private float _fadeDuration_5__8;

		object System.Collections.Generic.IEnumerator<System.Object>.Current => __2__current;

		object System.Collections.IEnumerator.Current => __2__current;

		// Token: 0x0600022C RID: 556 RVA: 0x00250B50 File Offset: 0x0024F150
		public _DisplayTextMeshFloatingText_d__13(int __1__state)
		{
			this.__1__state = __1__state;
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00250B40 File Offset: 0x0024F140
		void System.IDisposable.Dispose()
		{
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00260DE0 File Offset: 0x0025F3E0
		bool System.Collections.IEnumerator.MoveNext()
		{
			int num = __1__state;
			TextMeshProFloatingText textMeshProFloatingText = __4__this;
			int num2 = num;
			TextMeshProFloatingText textMeshProFloatingText2 = textMeshProFloatingText;
			if (num != 0)
			{
				num2--;
				if (num2 != 0)
				{
					if (num2 == 1)
					{
						__1__state = -1;
						Transform floatingText_Transform = textMeshProFloatingText2.m_floatingText_Transform;
						Vector3 position = new Vector3(_start_pos_5__5.x, _start_pos_5__5.y, _start_pos_5__5.z);
						floatingText_Transform.position = position;
						textMeshProFloatingText2.StartCoroutine(textMeshProFloatingText2.DisplayTextMeshFloatingText());
					}
					return false;
				}
				__1__state = -1;
			}
			else
			{
				__1__state = -1;
				_CountDuration_5__2 = 2f;
				_current_Count_5__4 = (_starting_Count_5__3 = UnityEngine.Random.Range(5f, 20f));
				Vector3 position2 = textMeshProFloatingText2.m_floatingText_Transform.position;
				_start_pos_5__5 = position2;
				_ = position2.z;
				_start_color_5__6 = NativeIntrinsics.Reinterpret<Color32>(NativeIntrinsics.Reinterpret<int>((Color32)textMeshProFloatingText2.m_textMesh.color));
				float num3 = 3f / _starting_Count_5__3;
				_alpha_5__7 = 255f;
				_fadeDuration_5__8 = num3 * _CountDuration_5__2;
			}
			float num4 = _current_Count_5__4;
			if (_current_Count_5__4 > 0f)
			{
				float deltaTime = Time.deltaTime;
				float num5 = deltaTime / _CountDuration_5__2 * _starting_Count_5__3;
				_current_Count_5__4 = num4 - deltaTime / _CountDuration_5__2 * _starting_Count_5__3;
				if (!(3f < num5 - deltaTime / _CountDuration_5__2 * _starting_Count_5__3))
				{
					float num6 = _alpha_5__7;
					float deltaTime2 = Time.deltaTime;
					_ = deltaTime2 / _fadeDuration_5__8 * 255f;
					float num7 = num6 - deltaTime2 / _fadeDuration_5__8 * 255f;
					if ((0f > num6 - deltaTime2 / _fadeDuration_5__8 * 255f))
					{
						num7 = 0f;
					}
					else if (num7 > 255f)
					{
						num7 = 255f;
					}
					_alpha_5__7 = num7;
				}
				TextMesh textMesh = textMeshProFloatingText2.m_textMesh;
				int num8 = default(int);
				num8 = (int)_current_Count_5__4;
				textMesh.text = num8.ToString();
				TextMesh textMesh2 = textMeshProFloatingText2.m_textMesh;
				num4 = (float)_start_color_5__6.rgba / 255f;
				float num9 = (float)(int)_start_color_5__6.g / 255f;
				float b = (float)(int)_start_color_5__6.b / 255f;
				float a = (float)(int)_alpha_5__7 / 255f;
				Color color = new Color(num4, num9, b, a);
				textMesh2.color = color;
				Transform floatingText_Transform2 = textMeshProFloatingText2.m_floatingText_Transform;
				Vector3 position3 = floatingText_Transform2.position;
				float num10 = _starting_Count_5__3;
				float deltaTime3 = Time.deltaTime;
				Vector3 position4 = new Vector3(position3.x + 0f, deltaTime3 * num10 + position3.y, position3.z + 0f);
				floatingText_Transform2.position = position4;
				float z2 = textMeshProFloatingText2.lastPOS.z;
				Vector3 position5 = textMeshProFloatingText2.m_cameraTransform.position;
				Vector3 vector = default(Vector3);
				vector.x = textMeshProFloatingText2.lastPOS.x;
				vector.y = textMeshProFloatingText2.lastPOS.y;
				vector.y = num9;
				vector.z = z2;
				Vector3 v = vector;
				Vector3 v2 = default(Vector3);
				v2.x = position5.x;
				v2.y = position5.y;
				v2.y = deltaTime3 * num10 + position3.y;
				v2.z = position5.z;
				if (TMPro_ExtensionMethods.Compare(v, v2, 1000))
				{
					Transform cameraTransform = textMeshProFloatingText2.m_cameraTransform;
					Quaternion lastRotation = textMeshProFloatingText2.lastRotation;
					Quaternion rotation = cameraTransform.rotation;
					Quaternion q = default(Quaternion);
					q.x = NativeIntrinsics.Reinterpret<float>(lastRotation);
					if (TMPro_ExtensionMethods.Compare(q, rotation, 1000))
					{
						goto IL_06e5;
					}
				}
				Vector3 position6 = textMeshProFloatingText2.m_cameraTransform.position;
				Transform cameraTransform2 = textMeshProFloatingText2.m_cameraTransform;
				textMeshProFloatingText2.lastPOS = position6;
				Quaternion rotation2 = cameraTransform2.rotation;
				Transform floatingText_Transform3 = textMeshProFloatingText2.m_floatingText_Transform;
				textMeshProFloatingText2.lastRotation = rotation2;
				floatingText_Transform3.rotation = rotation2;
				Vector3 position7 = textMeshProFloatingText2.m_transform.position;
				num4 = position7.x - textMeshProFloatingText2.lastPOS.x;
				num9 = position7.z - textMeshProFloatingText2.lastPOS.z;
				Transform transform = textMeshProFloatingText2.m_transform;
				Vector3 forward = new Vector3(num4, 0f, num9);
				transform.forward = forward;
				goto IL_06e5;
			}
			__2__current = new WaitForSeconds(UnityEngine.Random.Range(0.1f, 1f));
			__1__state = 2;
			goto IL_06fc;
			IL_06e5:
			__2__current = new WaitForEndOfFrame();
			__1__state = 1;
			goto IL_06fc;
			IL_06fc:
			return true;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x002612F0 File Offset: 0x0025F8F0
		void System.Collections.IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	// Token: 0x0200007B RID: 123
	private sealed class _DisplayTextMeshProFloatingText_d__12 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x04000324 RID: 804 Offset: 0x10
		// Il2CppStudio: declared private; widened because native code inlined into other types accesses it.
		internal int __1__state;

		// Token: 0x04000325 RID: 805 Offset: 0x18
		private object __2__current;

		// Token: 0x04000326 RID: 806 Offset: 0x20
		public TextMeshProFloatingText __4__this;

		// Token: 0x04000327 RID: 807 Offset: 0x28
		private float _CountDuration_5__2;

		// Token: 0x04000328 RID: 808 Offset: 0x2C
		private float _starting_Count_5__3;

		// Token: 0x04000329 RID: 809 Offset: 0x30
		private float _current_Count_5__4;

		// Token: 0x0400032A RID: 810 Offset: 0x34
		private Vector3 _start_pos_5__5;

		// Token: 0x0400032B RID: 811 Offset: 0x40
		private Color32 _start_color_5__6;

		// Token: 0x0400032C RID: 812 Offset: 0x44
		private float _alpha_5__7;

		// Token: 0x0400032D RID: 813 Offset: 0x48
		private float _fadeDuration_5__8;

		object System.Collections.Generic.IEnumerator<System.Object>.Current => __2__current;

		object System.Collections.IEnumerator.Current => __2__current;

		// Token: 0x06000232 RID: 562 RVA: 0x00250B50 File Offset: 0x0024F150
		public _DisplayTextMeshProFloatingText_d__12(int __1__state)
		{
			this.__1__state = __1__state;
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00250B40 File Offset: 0x0024F140
		void System.IDisposable.Dispose()
		{
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00261330 File Offset: 0x0025F930
		bool System.Collections.IEnumerator.MoveNext()
		{
			int num = __1__state;
			TextMeshProFloatingText textMeshProFloatingText = __4__this;
			int num2 = num;
			TextMeshProFloatingText textMeshProFloatingText2 = textMeshProFloatingText;
			if (num != 0)
			{
				num2--;
				if (num2 != 0)
				{
					if (num2 == 1)
					{
						__1__state = -1;
						Transform floatingText_Transform = textMeshProFloatingText2.m_floatingText_Transform;
						Vector3 position = new Vector3(_start_pos_5__5.x, _start_pos_5__5.y, _start_pos_5__5.z);
						floatingText_Transform.position = position;
						textMeshProFloatingText2.StartCoroutine(textMeshProFloatingText2.DisplayTextMeshProFloatingText());
					}
					return false;
				}
				__1__state = -1;
			}
			else
			{
				__1__state = -1;
				_CountDuration_5__2 = 2f;
				_current_Count_5__4 = (_starting_Count_5__3 = UnityEngine.Random.Range(5f, 20f));
				Vector3 position2 = textMeshProFloatingText2.m_floatingText_Transform.position;
				_start_pos_5__5 = position2;
				_ = position2.z;
				_start_color_5__6 = NativeIntrinsics.Reinterpret<Color32>(NativeIntrinsics.Reinterpret<int>((Color32)textMeshProFloatingText2.m_textMeshPro.color));
				float num3 = 3f / _starting_Count_5__3;
				_alpha_5__7 = 255f;
				_fadeDuration_5__8 = num3 * _CountDuration_5__2;
			}
			float num4 = _current_Count_5__4;
			if (_current_Count_5__4 > 0f)
			{
				float deltaTime = Time.deltaTime;
				float num5 = deltaTime / _CountDuration_5__2 * _starting_Count_5__3;
				_current_Count_5__4 = num4 - deltaTime / _CountDuration_5__2 * _starting_Count_5__3;
				if (!(3f < num5 - deltaTime / _CountDuration_5__2 * _starting_Count_5__3))
				{
					float num6 = _alpha_5__7;
					float deltaTime2 = Time.deltaTime;
					_ = deltaTime2 / _fadeDuration_5__8 * 255f;
					float num7 = num6 - deltaTime2 / _fadeDuration_5__8 * 255f;
					if ((0f > num6 - deltaTime2 / _fadeDuration_5__8 * 255f))
					{
						num7 = 0f;
					}
					else if (num7 > 255f)
					{
						num7 = 255f;
					}
					_alpha_5__7 = num7;
				}
				TextMeshPro textMeshPro = textMeshProFloatingText2.m_textMeshPro;
				int num8 = default(int);
				num8 = (int)_current_Count_5__4;
				textMeshPro.text = num8.ToString();
				num4 = (float)_start_color_5__6.rgba / 255f;
				float num9 = (float)(int)_start_color_5__6.g / 255f;
				float b = (float)(int)_start_color_5__6.b / 255f;
				float a = (float)(int)_alpha_5__7 / 255f;
				TextMeshPro textMeshPro2 = textMeshProFloatingText2.m_textMeshPro;
				Color color = new Color(num4, num9, b, a);
				textMeshPro2.color = color;
				Transform floatingText_Transform2 = textMeshProFloatingText2.m_floatingText_Transform;
				Vector3 position3 = floatingText_Transform2.position;
				float num10 = _starting_Count_5__3;
				float deltaTime3 = Time.deltaTime;
				Vector3 position4 = new Vector3(position3.x + 0f, deltaTime3 * num10 + position3.y, position3.z + 0f);
				floatingText_Transform2.position = position4;
				float z2 = textMeshProFloatingText2.lastPOS.z;
				Vector3 position5 = textMeshProFloatingText2.m_cameraTransform.position;
				Vector3 vector = default(Vector3);
				vector.x = textMeshProFloatingText2.lastPOS.x;
				vector.y = textMeshProFloatingText2.lastPOS.y;
				vector.y = num9;
				vector.z = z2;
				Vector3 v = vector;
				Vector3 v2 = default(Vector3);
				v2.x = position5.x;
				v2.y = position5.y;
				v2.y = deltaTime3 * num10 + position3.y;
				v2.z = position5.z;
				if (TMPro_ExtensionMethods.Compare(v, v2, 1000))
				{
					Transform cameraTransform = textMeshProFloatingText2.m_cameraTransform;
					Quaternion lastRotation = textMeshProFloatingText2.lastRotation;
					Quaternion rotation = cameraTransform.rotation;
					Quaternion q = default(Quaternion);
					q.x = NativeIntrinsics.Reinterpret<float>(lastRotation);
					if (TMPro_ExtensionMethods.Compare(q, rotation, 1000))
					{
						goto IL_06e1;
					}
				}
				Vector3 position6 = textMeshProFloatingText2.m_cameraTransform.position;
				Transform cameraTransform2 = textMeshProFloatingText2.m_cameraTransform;
				textMeshProFloatingText2.lastPOS = position6;
				Quaternion rotation2 = cameraTransform2.rotation;
				Transform floatingText_Transform3 = textMeshProFloatingText2.m_floatingText_Transform;
				textMeshProFloatingText2.lastRotation = rotation2;
				floatingText_Transform3.rotation = rotation2;
				Vector3 position7 = textMeshProFloatingText2.m_transform.position;
				num4 = position7.x - textMeshProFloatingText2.lastPOS.x;
				num9 = position7.z - textMeshProFloatingText2.lastPOS.z;
				Transform transform = textMeshProFloatingText2.m_transform;
				Vector3 forward = new Vector3(num4, 0f, num9);
				transform.forward = forward;
				goto IL_06e1;
			}
			__2__current = new WaitForSeconds(UnityEngine.Random.Range(0.1f, 1f));
			__1__state = 2;
			goto IL_06f8;
			IL_06e1:
			__2__current = new WaitForEndOfFrame();
			__1__state = 1;
			goto IL_06f8;
			IL_06f8:
			return true;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00261850 File Offset: 0x0025FE50
		void System.Collections.IEnumerator.Reset()
		{
			throw new NotSupportedException();
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
		m_transform = transform;
		m_floatingText = new GameObject(name + " floating text");
		m_cameraTransform = Camera.main.transform;
	}

	// Token: 0x06000228 RID: 552 RVA: 0x00260150 File Offset: 0x0025E750
	private void Start()
	{
		TextMeshProFloatingText textMeshProFloatingText = default(TextMeshProFloatingText);
		IEnumerator routine = default(IEnumerator);
		if (SpawnType == 0)
		{
			m_textMeshPro = m_floatingText.AddComponent<TextMeshPro>();
			m_textMeshPro.rectTransform.sizeDelta = new Vector2(3f, 3f);
			m_floatingText_Transform = m_floatingText.transform;
			Transform transform2 = m_transform;
			Transform floatingText_Transform2 = m_floatingText_Transform;
			Vector3 position3 = transform2.position;
			float z = 0f + position3.z;
			float x = 0f + position3.x;
			float y = 15f + position3.y;
			Vector3 position4 = new Vector3(x, y, z);
			floatingText_Transform2.position = position4;
			m_textMeshPro.alignment = TextAlignmentOptions.Center;
			TextMeshPro textMeshPro = m_textMeshPro;
			int num3 = UnityEngine.Random.Range(0, 255);
			int num4 = UnityEngine.Random.Range(0, 255);
			z = (float)UnityEngine.Random.Range(0, 255) / 255f;
			Color color2 = new Color((float)num3 / 255f, (float)num4 / 255f, z, 1f);
			textMeshPro.color = color2;
			m_textMeshPro.fontSize = 24f;
			m_textMeshPro.enableKerning = false;
			m_textMeshPro.text = string.Empty;
			_DisplayTextMeshProFloatingText_d__12 _DisplayTextMeshProFloatingText_d__ = new _DisplayTextMeshProFloatingText_d__12(0);
			_DisplayTextMeshProFloatingText_d__.__1__state = 0;
			_DisplayTextMeshProFloatingText_d__.__4__this = this;
			textMeshProFloatingText = this;
		}
		else if (SpawnType == 1)
		{
			m_floatingText_Transform = m_floatingText.transform;
			Transform transform = m_transform;
			Transform floatingText_Transform = m_floatingText_Transform;
			Vector3 position = transform.position;
			float z = 0f + position.z;
			float x = 0f + position.x;
			float y = 15f + position.y;
			Vector3 position2 = new Vector3(x, y, z);
			floatingText_Transform.position = position2;
			m_textMesh = m_floatingText.AddComponent<TextMesh>();
			m_textMesh.font = Resources.Load<Font>("Fonts/ARIAL");
			m_textMesh.GetComponent<Renderer>().SetMaterial(m_textMesh.font.material);
			TextMesh textMesh = m_textMesh;
			int num = UnityEngine.Random.Range(0, 255);
			int num2 = UnityEngine.Random.Range(0, 255);
			z = (float)UnityEngine.Random.Range(0, 255) / 255f;
			Color color = new Color((float)num / 255f, (float)num2 / 255f, z, 1f);
			textMesh.color = color;
			m_textMesh.anchor = TextAnchor.LowerCenter;
			m_textMesh.fontSize = 24;
			IEnumerator enumerator = DisplayTextMeshFloatingText();
			textMeshProFloatingText = this;
			routine = enumerator;
		}
		else
		{
			return;
		}
		textMeshProFloatingText.StartCoroutine(routine);
	}

	// Token: 0x06000229 RID: 553 RVA: 0x002600E0 File Offset: 0x0025E6E0
	public IEnumerator DisplayTextMeshProFloatingText()
	{
		_DisplayTextMeshProFloatingText_d__12 _DisplayTextMeshProFloatingText_d__ = new _DisplayTextMeshProFloatingText_d__12(0);
		_DisplayTextMeshProFloatingText_d__.__1__state = 0;
		_DisplayTextMeshProFloatingText_d__.__4__this = this;
		return _DisplayTextMeshProFloatingText_d__;
	}

	// Token: 0x0600022A RID: 554 RVA: 0x00260070 File Offset: 0x0025E670
	public IEnumerator DisplayTextMeshFloatingText()
	{
		_DisplayTextMeshFloatingText_d__13 _DisplayTextMeshFloatingText_d__ = new _DisplayTextMeshFloatingText_d__13(0);
		_DisplayTextMeshFloatingText_d__.__1__state = 0;
		_DisplayTextMeshFloatingText_d__.__4__this = this;
		return _DisplayTextMeshFloatingText_d__;
	}

	// Token: 0x0600022B RID: 555 RVA: 0x00260690 File Offset: 0x0025EC90
	public TextMeshProFloatingText()
	{
		float z = Vector3.zero.z;
		lastPOS = new Vector3(Vector3.zero.x, Vector3.zero.y, z);
		lastRotation = Quaternion.identityQuaternion;
	}
}

