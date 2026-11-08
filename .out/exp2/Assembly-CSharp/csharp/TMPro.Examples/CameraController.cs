using System;
using Il2CppStudio.Native;
using UnityEngine;

namespace TMPro.Examples;

// Token: 0x0200006B RID: 107
public class CameraController : MonoBehaviour
{
	// Token: 0x0200006C RID: 108
	public enum CameraModes
	{
		// Token: 0x040002D3 RID: 723 Offset: 0x0
		Follow,
		// Token: 0x040002D4 RID: 724 Offset: 0x0
		Isometric,
		// Token: 0x040002D5 RID: 725 Offset: 0x0
		Free
	}

	// Token: 0x040002B9 RID: 697 Offset: 0x20
	private Transform cameraTransform;

	// Token: 0x040002BA RID: 698 Offset: 0x28
	private Transform dummyTarget;

	// Token: 0x040002BB RID: 699 Offset: 0x30
	public Transform CameraTarget;

	// Token: 0x040002BC RID: 700 Offset: 0x38
	public float FollowDistance = 30f;

	// Token: 0x040002BD RID: 701 Offset: 0x3C
	public float MaxFollowDistance = 100f;

	// Token: 0x040002BE RID: 702 Offset: 0x40
	public float MinFollowDistance = 2f;

	// Token: 0x040002BF RID: 703 Offset: 0x44
	public float ElevationAngle = 30f;

	// Token: 0x040002C0 RID: 704 Offset: 0x48
	public float MaxElevationAngle = 85f;

	// Token: 0x040002C1 RID: 705 Offset: 0x4C
	public float MinElevationAngle;

	// Token: 0x040002C2 RID: 706 Offset: 0x50
	public float OrbitalAngle;

	// Token: 0x040002C3 RID: 707 Offset: 0x54
	public CameraModes CameraMode;

	// Token: 0x040002C4 RID: 708 Offset: 0x58
	public bool MovementSmoothing = true;

	// Token: 0x040002C5 RID: 709 Offset: 0x59
	public bool RotationSmoothing;

	// Token: 0x040002C6 RID: 710 Offset: 0x5A
	private bool previousSmoothing;

	// Token: 0x040002C7 RID: 711 Offset: 0x5C
	public float MovementSmoothingValue = 25f;

	// Token: 0x040002C8 RID: 712 Offset: 0x60
	public float RotationSmoothingValue = 5f;

	// Token: 0x040002C9 RID: 713 Offset: 0x64
	public float MoveSensitivity = 2f;

	// Token: 0x040002CA RID: 714 Offset: 0x68
	private Vector3 currentVelocity;

	// Token: 0x040002CB RID: 715 Offset: 0x74
	private Vector3 desiredPosition;

	// Token: 0x040002CC RID: 716 Offset: 0x80
	private float mouseX;

	// Token: 0x040002CD RID: 717 Offset: 0x84
	private float mouseY;

	// Token: 0x040002CE RID: 718 Offset: 0x88
	private Vector3 moveVector;

	// Token: 0x040002CF RID: 719 Offset: 0x94
	private float mouseWheel;

	// Token: 0x040002D0 RID: 720 Offset: 0x0
	private const string event_SmoothingValue = "Slider - Smoothing Value";

	// Token: 0x040002D1 RID: 721 Offset: 0x0
	private const string event_FollowDistance = "Slider - Camera Zoom";

	// Token: 0x060001EA RID: 490 RVA: 0x00253BA0 File Offset: 0x002521A0
	private void Awake()
	{
		Application.targetFrameRate = ((QualitySettings.vSyncCount > 0) ? 60 : (-1));
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		{
			Input.simulateMouseWithTouches = false;
		}
		cameraTransform = transform;
		previousSmoothing = MovementSmoothing;
	}

	// Token: 0x060001EB RID: 491 RVA: 0x00254AE0 File Offset: 0x002530E0
	private void Start()
	{
		if (CameraTarget == null)
		{
			dummyTarget = new GameObject("Camera Target").transform;
			CameraTarget = dummyTarget;
		}
	}

	// Token: 0x060001EC RID: 492 RVA: 0x00254650 File Offset: 0x00252C50
	private void LateUpdate()
	{
		GetPlayerInput();
		_ = CameraTarget != null;
		if (NativeIntrinsics.Reinterpret<int>(CameraMode) == 1)
		{
			Vector3 position2 = CameraTarget.position;
			float y3 = OrbitalAngle * ((float)Math.PI / 180f);
			Vector3 euler2 = new Vector3(ElevationAngle * ((float)Math.PI / 180f), y3, 0f);
			Quaternion quaternion2 = Quaternion.Internal_FromEulerRad(euler2);
			Vector3 vector3 = new Vector3(0f, y3, NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("xorps xmm0,[1813EACB0h]")));
			Vector3 vector4 = quaternion2 * vector3;
			desiredPosition = new Vector3(position2.x + vector4.x, position2.y + vector4.y, position2.z + vector4.z);
		}
		else if (NativeIntrinsics.Reinterpret<int>(CameraMode) == 0)
		{
			Vector3 position = CameraTarget.position;
			Transform cameraTarget = CameraTarget;
			float y = OrbitalAngle * ((float)Math.PI / 180f);
			Vector3 euler = new Vector3(ElevationAngle * ((float)Math.PI / 180f), y, 0f);
			Quaternion quaternion = Quaternion.Internal_FromEulerRad(euler);
			Vector3 vector = new Vector3(0f, y, NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("xorps xmm0,[1813EACB0h]")));
			Vector3 direction = quaternion * vector;
			Transform transform = cameraTarget;
			float x = position.x;
			float y2 = position.y;
			float z = position.z;
			Vector3 vector2 = transform.TransformDirection(direction);
			desiredPosition = new Vector3(x + vector2.x, y2 + vector2.y, z + vector2.z);
		}
		_ = cameraTransform;
		if (!MovementSmoothing)
		{
			_ = desiredPosition.z;
			NativeIntrinsics.CallNative("sub_25497C", 0L, NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFF88]"), 0L, 0L);
		}
	}

	// Token: 0x060001ED RID: 493 RVA: 0x00253C80 File Offset: 0x00252280
	private void GetPlayerInput()
	{
		moveVector = new Vector3(Vector3.zeroVector.x, Vector3.zeroVector.y, Vector3.zeroVector.z);
		mouseWheel = Input.GetAxis("Mouse ScrollWheel");
		int touchCount = Input.touchCount;
		bool keyInt = Input.GetKeyInt(KeyCode.LeftShift);
		float num = touchCount;
		if (!keyInt && !Input.GetKeyInt(KeyCode.RightShift) && !(num > 0f))
		{
			return;
		}
		mouseWheel *= 10f;
		if (Input.GetKeyDownInt(KeyCode.I))
		{
			CameraMode = NativeIntrinsics.Reinterpret<CameraModes>(1);
		}
		if (Input.GetKeyDownInt(KeyCode.F))
		{
			CameraMode = NativeIntrinsics.Reinterpret<CameraModes>(0);
		}
		if (Input.GetKeyDownInt(KeyCode.S))
		{
			MovementSmoothing = !MovementSmoothing;
		}
		if (Input.GetMouseButton(1))
		{
			mouseY = Input.GetAxis("Mouse Y");
			float axis = Input.GetAxis("Mouse X");
			float num2 = mouseY;
			mouseX = axis;
			float num3 = axis;
			if (mouseY > 0.01f || -0.01f > num2)
			{
				float elevationAngle = MinElevationAngle;
				_ = mouseY * MoveSensitivity;
				float num5 = ElevationAngle - mouseY * MoveSensitivity;
				if (!(MinElevationAngle > ElevationAngle - mouseY * MoveSensitivity))
				{
					elevationAngle = MaxElevationAngle;
					if (!(num5 > MaxElevationAngle))
					{
						elevationAngle = num5;
					}
				}
				ElevationAngle = elevationAngle;
			}
			if (num3 > 0.01f || -0.01f > num3)
			{
				num3 = (OrbitalAngle = num3 * MoveSensitivity + OrbitalAngle);
				if (num3 * MoveSensitivity + OrbitalAngle > 360f)
				{
					OrbitalAngle = num3 - 360f;
				}
				if (0f > OrbitalAngle)
				{
					OrbitalAngle += 360f;
				}
			}
		}
		if (num == 1f)
		{
			Touch touch = Input.GetTouch(0);
			Touch touch2 = default(Touch);
			touch2.m_FingerId = touch.m_FingerId;
			touch2.m_Position.x = touch.m_Position.x;
			touch2.m_RawPosition.x = touch.m_RawPosition.x;
			touch2.m_TapCount = touch.m_TapCount;
			touch2.m_Phase = touch.m_Phase;
			touch2.m_Type = touch.m_Type;
			touch2.m_Pressure = touch.m_Pressure;
			touch2.m_maximumPossiblePressure = touch.m_maximumPossiblePressure;
			touch2.m_Radius = touch.m_Radius;
			touch2.m_RadiusVariance = touch.m_RadiusVariance;
			touch2.m_AltitudeAngle = touch.m_AltitudeAngle;
			touch2.m_AzimuthAngle = touch.m_AzimuthAngle;
			if (touch2.phase == TouchPhase.Moved)
			{
				Touch touch3 = Input.GetTouch(0);
				Touch touch4 = default(Touch);
				touch4.m_FingerId = touch3.m_FingerId;
				touch4.m_Position.x = touch3.m_Position.x;
				touch4.m_RawPosition.x = touch3.m_RawPosition.x;
				touch4.m_TapCount = touch3.m_TapCount;
				touch4.m_Phase = touch3.m_Phase;
				touch4.m_Type = touch3.m_Type;
				touch4.m_Pressure = touch3.m_Pressure;
				touch4.m_maximumPossiblePressure = touch3.m_maximumPossiblePressure;
				touch4.m_Radius = touch3.m_Radius;
				touch4.m_RadiusVariance = touch3.m_RadiusVariance;
				touch4.m_AltitudeAngle = touch3.m_AltitudeAngle;
				touch4.m_AzimuthAngle = touch3.m_AzimuthAngle;
				Vector2 deltaPosition = touch4.deltaPosition;
				int num6 = (int)NativeIntrinsics.Asm("stack[0xC]");
				Vector2 value = deltaPosition;
				if (NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("stack[0xC]")) > 0.01f || -0.01f > NativeIntrinsics.Reinterpret<float>(num6))
				{
					float elevationAngle2 = MinElevationAngle;
					float elevationAngle = ElevationAngle - (float)num6 * 0.1f;
					if (!(MinElevationAngle > ElevationAngle - (float)num6 * 0.1f))
					{
						elevationAngle2 = MaxElevationAngle;
						if (!(elevationAngle > MaxElevationAngle))
						{
							elevationAngle2 = elevationAngle;
						}
					}
					ElevationAngle = elevationAngle2;
				}
				if (NativeIntrinsics.Reinterpret<float>(value) > 0.01f || -0.01f > NativeIntrinsics.Reinterpret<float>(value))
				{
					float num7 = (OrbitalAngle = NativeIntrinsics.Reinterpret<float>(value) * 0.1f + OrbitalAngle);
					if (NativeIntrinsics.Reinterpret<float>(value) * 0.1f + OrbitalAngle > 360f)
					{
						OrbitalAngle = num7 - 360f;
					}
					if (0f > OrbitalAngle)
					{
						OrbitalAngle += 360f;
					}
				}
			}
		}
		if (Input.GetMouseButton(0) && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hitInfo, 300f, 23552))
		{
			if (!(hitInfo.transform == CameraTarget))
			{
				CameraTarget = hitInfo.transform;
				MovementSmoothing = previousSmoothing;
			}
			OrbitalAngle = 0f;
		}
		if (!Input.GetMouseButton(2))
		{
			return;
		}
		if ((dummyTarget == null))
		{
			dummyTarget = new GameObject("Camera Target").transform;
		}
		else if (!(dummyTarget != CameraTarget))
		{
			goto IL_1121;
		}
		Transform cameraTarget2 = CameraTarget;
		dummyTarget.position = cameraTarget2.position;
		cameraTarget2 = CameraTarget;
		dummyTarget.rotation = cameraTarget2.rotation;
		CameraTarget = dummyTarget;
		previousSmoothing = MovementSmoothing;
		MovementSmoothing = false;
		goto IL_1121;
		IL_1121:
		mouseY = Input.GetAxis("Mouse Y");
		float axis2 = Input.GetAxis("Mouse X");
		Transform transform = cameraTransform;
		mouseX = axis2;
		Vector3 vector = transform.TransformDirection(axis2, mouseY, 0f);
		moveVector = vector;
		long num8 = NativeIntrinsics.Asm("xorps xmm1,xmm0");
		long num9 = NativeIntrinsics.Asm("xorps xmm2,xmm0");
		long num10 = NativeIntrinsics.Asm("xorps xmm3,xmm0");
		Transform obj = dummyTarget;
		Vector3 translation = new Vector3(NativeIntrinsics.Reinterpret<float>(num8), NativeIntrinsics.Reinterpret<float>(num9), NativeIntrinsics.Reinterpret<float>(num10));
		obj.Translate(translation, Space.World);
	}

	// Token: 0x060001EE RID: 494 RVA: 0x00254BB0 File Offset: 0x002531B0
	public CameraController()
	{
		float z = Vector3.zeroVector.z;
		currentVelocity = new Vector3(Vector3.zeroVector.x, Vector3.zeroVector.y, z);
		base..ctor();
	}
}

