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
		// error: enumerator has no value
		// Token: 0x040002E1 RID: 737 Offset: 0x0
		Rotation,
		// error: enumerator has no value
		// Token: 0x040002E2 RID: 738 Offset: 0x0
		BackAndForth,
		// error: enumerator has no value
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
		Transform transform = this.transform;
		m_transform = transform;
		Transform transform2 = m_transform;
		Quaternion rotation = transform2.rotation;
		Vector3 vector = Quaternion.Internal_ToEulerRad(rotation);
		Vector3 euler = default(Vector3);
		euler.x = vector.x * 57.29578f;
		euler.y = vector.y * 57.29578f;
		euler.z = vector.z * 57.29578f;
		Vector3 vector2 = Quaternion.Internal_MakePositive(euler);
		Transform transform3 = m_transform;
		m_initial_Rotation.x = vector2.x;
		m_initial_Rotation.y = vector2.y;
		m_initial_Rotation.z = vector2.z;
		long num = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
		float z = vector2.z;
		transform2 = transform3;
		Vector3 position = transform2.position;
		m_initial_Position.x = position.x;
		m_initial_Position.y = position.y;
		m_initial_Position.z = position.z;
		Light component = GetComponent<Light>();
		Light light = component;
		if (light != null)
		{
			Color color = light.color;
			float r = color.r;
			float g = color.g;
			float b = color.b;
			float a = color.a;
		}
		Color color2 = default(Color);
		color2.r = 0f;
		color2.g = 0f;
		color2.b = 0f;
		color2.a = 1f;
		Color32 value = color2;
		m_lightColor = NativeIntrinsics.Reinterpret<Color32>(NativeIntrinsics.Reinterpret<int>(value));
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
				long num = NativeIntrinsics.CallNative("sub_20C8F0", 0L, 0L, 0L, 0L);
				long num2 = num;
				float x = (float)NativeIntrinsics.Asm("xmm0") * 15f + m_initial_Position.x;
				Vector3 position = default(Vector3);
				position.x = x;
				position.y = 0f + m_initial_Position.y;
				position.z = 10f + m_initial_Position.z;
				transform.position = position;
				Vector3 position2 = m_transform.position;
				frames++;
				m_prevPOS.x = position2.x;
				m_prevPOS.y = position2.y;
				m_prevPOS.z = position2.z;
				long num3 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
			}
			else
			{
				float deltaTime2 = Time.deltaTime;
				Transform transform2 = m_transform;
				m_time = deltaTime2 * spinSpeed + time;
				float num4 = RotationRange;
				long num5 = NativeIntrinsics.CallNative("sub_20D1D0", 0L, 0L, 0L, 0L);
				Vector3 euler = default(Vector3);
				euler.x = m_initial_Rotation.x * (MathF.PI / 180f);
				euler.y = ((float)NativeIntrinsics.Asm("xmm0") * num4 + m_initial_Rotation.y) * (MathF.PI / 180f);
				euler.z = m_initial_Rotation.z * (MathF.PI / 180f);
				Quaternion rotation = Quaternion.Internal_FromEulerRad(euler);
				transform2.rotation = rotation;
			}
		}
		else
		{
			Transform transform3 = m_transform;
			float deltaTime3 = Time.deltaTime;
			float num6 = deltaTime3;
			transform3.Rotate(0f, num6 * spinSpeed, 0f);
		}
	}
}

