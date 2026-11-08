using System;
using Il2CppStudio.Native;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x0200006D RID: 109
public class ObjectSpin : MonoBehaviour
{
	// Token: 0x0200006E RID: 110
	public enum MotionType
	{
		// Token: 0x040002E1 RID: 737 Offset: 0x0
		Rotation,
		// Token: 0x040002E2 RID: 738 Offset: 0x0
		BackAndForth,
		// Token: 0x040002E3 RID: 739 Offset: 0x0
		Translation
	}

	// Token: 0x040002D6 RID: 726 Offset: 0x20
	public float SpinSpeed = 5f;

	// Token: 0x040002D7 RID: 727 Offset: 0x24
	public int RotationRange = 15;

	// Token: 0x040002D8 RID: 728 Offset: 0x28
	private Transform m_transform;

	// Token: 0x040002D9 RID: 729 Offset: 0x30
	private float m_time;

	// Token: 0x040002DA RID: 730 Offset: 0x34
	private Vector3 m_prevPOS;

	// Token: 0x040002DB RID: 731 Offset: 0x40
	private Vector3 m_initial_Rotation;

	// Token: 0x040002DC RID: 732 Offset: 0x4C
	private Vector3 m_initial_Position;

	// Token: 0x040002DD RID: 733 Offset: 0x58
	private Color32 m_lightColor;

	// Token: 0x040002DE RID: 734 Offset: 0x5C
	private int frames;

	// Token: 0x040002DF RID: 735 Offset: 0x60
	public MotionType Motion;

	// Token: 0x060001EF RID: 495 RVA: 0x00255D00 File Offset: 0x00254300
	private void Awake()
	{
		m_transform = this.transform;
		Vector3 vector = Quaternion.Internal_ToEulerRad(m_transform.rotation);
		Vector3 euler = new Vector3(vector.x * 57.29578f, vector.y * 57.29578f, vector.z * 57.29578f);
		Vector3 vector2 = Quaternion.Internal_MakePositive(euler);
		Transform transform = m_transform;
		m_initial_Rotation = vector2;
		_ = vector2.z;
		m_initial_Position = transform.position;
		Light component = GetComponent<Light>();
		float r;
		float g;
		float b;
		float a;
		if (component != null)
		{
			Color color = component.color;
			r = color.r;
			g = color.g;
			b = color.b;
			a = color.a;
		}
		else
		{
			r = 0f;
			g = 0f;
			b = 0f;
			a = 1f;
		}
		Color color2 = new Color(r, g, b, a);
		m_lightColor = NativeIntrinsics.Reinterpret<Color32>(NativeIntrinsics.Reinterpret<int>((Color32)color2));
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x00255EB0 File Offset: 0x002544B0
	private void Update()
	{
		float spinSpeed = SpinSpeed;
		if (NativeIntrinsics.Reinterpret<int>(Motion) != 0)
		{
			float time = m_time;
			if (NativeIntrinsics.Reinterpret<int>(Motion) != 1)
			{
				float deltaTime = Time.deltaTime;
				Transform transform = m_transform;
				m_time = deltaTime * spinSpeed + time;
				float x = (float)Math.Cos((deltaTime * spinSpeed + time) * 0.95f) * 15f + m_initial_Position.x;
				Vector3 position = new Vector3(x, 0f + m_initial_Position.y, 10f + m_initial_Position.z);
				transform.position = position;
				Vector3 position2 = m_transform.position;
				frames++;
				m_prevPOS = position2;
			}
			else
			{
				float deltaTime2 = Time.deltaTime;
				Transform transform2 = m_transform;
				m_time = deltaTime2 * spinSpeed + time;
				float num = RotationRange;
				float num2 = (float)Math.Sin(deltaTime2 * spinSpeed + time);
				Vector3 euler = new Vector3(m_initial_Rotation.x * ((float)Math.PI / 180f), (num2 * num + m_initial_Rotation.y) * ((float)Math.PI / 180f), m_initial_Rotation.z * ((float)Math.PI / 180f));
				transform2.rotation = Quaternion.Internal_FromEulerRad(euler);
			}
		}
		else
		{
			m_transform.Rotate(0f, Time.deltaTime * spinSpeed, 0f);
		}
	}
}

