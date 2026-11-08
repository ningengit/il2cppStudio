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
		// error: enumerator has no value
		// Token: 0x040002D3 RID: 723 Offset: 0x0
		Follow,
		// error: enumerator has no value
		// Token: 0x040002D4 RID: 724 Offset: 0x0
		Isometric,
		// error: enumerator has no value
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
	private const string event_SmoothingValue = null;

	// Token: 0x040002D1 RID: 721 Offset: 0x0
	private const string event_FollowDistance = null;

	// Token: 0x060001EA RID: 490 RVA: 0x00253BA0 File Offset: 0x002521A0
	private void Awake()
	{
		int vSyncCount = QualitySettings.vSyncCount;
		if (vSyncCount <= 0 || (int)NativeIntrinsics.Asm("*(0xFFFFFFFFFFFFFFFF + 0xE0)") == 0)
		{
		}
		Application.targetFrameRate = 60;
		RuntimePlatform platform = Application.platform;
		if (NativeIntrinsics.Reinterpret<int>(platform) != 8)
		{
			RuntimePlatform platform2 = Application.platform;
			if (NativeIntrinsics.Reinterpret<int>(platform2) != 11)
			{
				goto IL_0072;
			}
		}
		Input.simulateMouseWithTouches = false;
		goto IL_0072;
		IL_0072:
		Transform transform = this.transform;
		cameraTransform = transform;
		int movementSmoothing = (MovementSmoothing ? 1 : 0);
		previousSmoothing = (byte)movementSmoothing != 0;
	}

	// Token: 0x060001EB RID: 491 RVA: 0x00254AE0 File Offset: 0x002530E0
	private void Start()
	{
		Transform cameraTarget = CameraTarget;
		bool flag = cameraTarget == null;
		bool flag2 = flag;
		if (flag)
		{
			GameObject gameObject = new GameObject("Camera Target");
			Transform transform = gameObject.transform;
			dummyTarget = transform;
			Transform cameraTarget2 = dummyTarget;
			CameraTarget = cameraTarget2;
		}
	}

	// Token: 0x060001EC RID: 492 RVA: 0x00254650 File Offset: 0x00252C50
	private void LateUpdate()
	{
		GetPlayerInput();
		Transform cameraTarget = CameraTarget;
		bool flag = cameraTarget != null;
		if (NativeIntrinsics.Reinterpret<int>(CameraMode) != 1)
		{
			if (NativeIntrinsics.Reinterpret<int>(CameraMode) == 0)
			{
				Transform cameraTarget2 = CameraTarget;
				Vector3 position = cameraTarget2.position;
				Transform cameraTarget3 = CameraTarget;
				float num = OrbitalAngle * (MathF.PI / 180f);
				Quaternion quaternion = Quaternion.Internal_FromEulerRad(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -25L));
				Vector3 vector = NativeIntrinsics.Reinterpret<Quaternion>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + 7L) * NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -25L);
				cameraTarget = cameraTarget3;
				float x = position.x;
				float y = position.y;
				float z = position.z;
				Vector3 vector2 = cameraTarget.TransformDirection(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -25L));
				desiredPosition = NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("unpcklps xmm6,xmm7"));
				desiredPosition.z = z + vector2.z;
			}
		}
		else
		{
			Transform cameraTarget2 = CameraTarget;
			Vector3 position2 = cameraTarget2.position;
			float num2 = OrbitalAngle * (MathF.PI / 180f);
			Quaternion quaternion2 = Quaternion.Internal_FromEulerRad(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -25L));
			Vector3 vector3 = NativeIntrinsics.Reinterpret<Quaternion>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + 7L) * NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -25L);
			desiredPosition = NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("unpcklps xmm8,xmm6"));
			desiredPosition.z = position2.z + vector3.z;
		}
		Transform transform = cameraTransform;
		if (!MovementSmoothing)
		{
			float z2 = desiredPosition.z;
			long num3 = NativeIntrinsics.CallNative("sub_25497C", 0L, NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -25L, 0L, 0L);
		}
	}

	// Token: 0x060001ED RID: 493 RVA: 0x00253C80 File Offset: 0x00252280
	private unsafe void GetPlayerInput()
	{
		NativeIntrinsics.Stmt("movups [rbp+60h],xmm0 ; value = FloatExpr");
		NativeIntrinsics.Stmt("mov [rbp-60h],eax ; value = 0x0");
		NativeIntrinsics.Stmt("movups [rbp+6Ch],xmm0 ; value = FloatExpr");
		NativeIntrinsics.Stmt("mov [rbp-10h],eax ; value = 0x0");
		NativeIntrinsics.Stmt("mov [rbp+40h],eax ; value = 0x0");
		NativeIntrinsics.Stmt("movups [rbp-80h],xmm0 ; value = FloatExpr");
		NativeIntrinsics.Stmt("movups [rbp-70h],xmm0 ; value = FloatExpr");
		NativeIntrinsics.Stmt("movups [rbp+50h],xmm0 ; value = FloatExpr");
		NativeIntrinsics.Stmt("movups [rbp-50h],xmm1 ; value = FloatExpr");
		NativeIntrinsics.Stmt("movups [rbp-40h],xmm1 ; value = FloatExpr");
		NativeIntrinsics.Stmt("movups [rbp-30h],xmm1 ; value = FloatExpr");
		NativeIntrinsics.Stmt("movups [rbp-20h],xmm1 ; value = FloatExpr");
		NativeIntrinsics.Stmt("movups [rbp],xmm0 ; value = FloatExpr");
		NativeIntrinsics.Stmt("movups [rbp+10h],xmm0 ; value = FloatExpr");
		NativeIntrinsics.Stmt("movups [rbp+20h],xmm0 ; value = FloatExpr");
		NativeIntrinsics.Stmt("movups [rbp+30h],xmm0 ; value = FloatExpr");
		long num = NativeIntrinsics.Asm("&stack[0x0]") + -360L;
		if (NativeIntrinsics.Asm("global_19E3BFE") == 0)
		{
			NativeIntrinsics.Stmt("mov byte ptr [1819E3BFEh],1 ; value = 0x1");
		}
		float z = Vector3.zeroVector.z;
		moveVector.x = Vector3.zeroVector.x;
		moveVector.y = Vector3.zeroVector.y;
		moveVector.z = z;
		float axis = Input.GetAxis("Mouse ScrollWheel");
		mouseWheel = axis;
		int touchCount = Input.touchCount;
		bool keyInt = Input.GetKeyInt(NativeIntrinsics.Reinterpret<KeyCode>(304));
		float num2 = touchCount;
		if (!keyInt && !Input.GetKeyInt(NativeIntrinsics.Reinterpret<KeyCode>(303)) && !(num2 > 0f))
		{
			return;
		}
		float num3 = mouseWheel * 10f;
		mouseWheel = num3;
		if (Input.GetKeyDownInt(NativeIntrinsics.Reinterpret<KeyCode>(105)))
		{
			CameraMode = NativeIntrinsics.Reinterpret<CameraModes>(1);
		}
		if (Input.GetKeyDownInt(NativeIntrinsics.Reinterpret<KeyCode>(102)))
		{
			CameraMode = NativeIntrinsics.Reinterpret<CameraModes>(0);
		}
		if (Input.GetKeyDownInt(NativeIntrinsics.Reinterpret<KeyCode>(115)))
		{
			MovementSmoothing = !MovementSmoothing;
		}
		if (Input.GetMouseButton(1))
		{
			float axis2 = Input.GetAxis("Mouse Y");
			mouseY = axis2;
			float axis3 = Input.GetAxis("Mouse X");
			float num4 = mouseY;
			mouseX = axis3;
			float num5 = axis3;
			float num6 = num4;
			if (mouseY > 0.01f || -0.01f > num6)
			{
				float elevationAngle = MinElevationAngle;
				num6 = mouseY * MoveSensitivity;
				float num7 = ElevationAngle - mouseY * MoveSensitivity;
				if (!(MinElevationAngle > ElevationAngle - mouseY * MoveSensitivity))
				{
					elevationAngle = MaxElevationAngle;
					if (!(num7 > MaxElevationAngle))
					{
						elevationAngle = num7;
					}
				}
				ElevationAngle = elevationAngle;
			}
			if (num5 > 0.01f || -0.01f > num5)
			{
				num5 = (OrbitalAngle = num5 * MoveSensitivity + OrbitalAngle);
				if (num5 * MoveSensitivity + OrbitalAngle > 360f)
				{
					OrbitalAngle = num5 - 360f;
					num5 -= 360f;
				}
				if (0f > OrbitalAngle)
				{
					OrbitalAngle += 360f;
				}
			}
		}
		float num9;
		if (num2 == 1f)
		{
			Touch touch = Input.GetTouch(0);
			NativeIntrinsics.Stmt("movaps [rbp-80h],xmm0 ; value = StructChunkExpr");
			NativeIntrinsics.Stmt("movaps [rbp-70h],xmm1 ; value = StructChunkExpr");
			NativeIntrinsics.Stmt("mov [rbp-60h],eax ; value = FieldExpr");
			Touch touch2 = default(Touch);
			touch2.m_FingerId = touch.m_FingerId;
			touch2.m_Position.x = touch.m_Position.x;
			touch2.m_RawPosition.x = touch.m_RawPosition.x;
			TouchPhase phase = touch2.phase;
			if (NativeIntrinsics.Reinterpret<int>(phase) == 1)
			{
				Touch touch3 = Input.GetTouch(0);
				NativeIntrinsics.Stmt("movaps [rbp-80h],xmm0 ; value = StructChunkExpr");
				NativeIntrinsics.Stmt("movaps [rbp-70h],xmm1 ; value = StructChunkExpr");
				NativeIntrinsics.Stmt("mov [rbp-60h],eax ; value = FieldExpr");
				Touch touch4 = default(Touch);
				touch4.m_FingerId = touch3.m_FingerId;
				touch4.m_Position.x = touch3.m_Position.x;
				touch4.m_RawPosition.x = touch3.m_RawPosition.x;
				Vector2 deltaPosition = touch4.deltaPosition;
				NativeIntrinsics.Stmt("movsd qword ptr [rbp+170h],xmm0 ; value = LocalExpr");
				int num8 = (int)NativeIntrinsics.Asm("*(LocalExpr + 0x174)");
				Vector2 value = deltaPosition;
				if (NativeIntrinsics.Reinterpret<float>((int)NativeIntrinsics.Asm("*(LocalExpr + 0x174)")) > 0.01f || -0.01f > NativeIntrinsics.Reinterpret<float>(num8))
				{
					float elevationAngle2 = MinElevationAngle;
					float elevationAngle = ElevationAngle - (float)num8 * 0.1f;
					if (!(MinElevationAngle > ElevationAngle - (float)num8 * 0.1f))
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
					num9 = (OrbitalAngle = NativeIntrinsics.Reinterpret<float>(value) * 0.1f + OrbitalAngle);
					if (NativeIntrinsics.Reinterpret<float>(value) * 0.1f + OrbitalAngle > 360f)
					{
						OrbitalAngle = num9 - 360f;
						num9 -= 360f;
					}
					if (0f > OrbitalAngle)
					{
						OrbitalAngle += 360f;
					}
				}
			}
		}
		Transform transform2;
		if (Input.GetMouseButton(0))
		{
			Camera main = Camera.main;
			Vector3 mousePosition = Input.mousePosition;
			Camera camera = main;
			Vector3 pos = default(Vector3);
			pos.x = mousePosition.x;
			pos.y = mousePosition.y;
			pos.z = mousePosition.z;
			Ray ray = camera.ScreenPointToRay(pos);
			long num10 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
			long num11 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x10, 8)");
			Ray ray2 = default(Ray);
			ray2.m_Origin.x = NativeIntrinsics.Reinterpret<float>(num10);
			if (Physics.Raycast(ray2, out *(RaycastHit*)(num + 80L), 300f, 23552))
			{
				Transform transform = ((RaycastHit*)(num + 80L))->transform;
				Transform cameraTarget = CameraTarget;
				transform2 = transform;
				if (!(transform2 == cameraTarget))
				{
					Transform cameraTarget2 = ((RaycastHit*)(num + 80L))->transform;
					CameraTarget = cameraTarget2;
					MovementSmoothing = previousSmoothing;
				}
				OrbitalAngle = 0f;
			}
		}
		if (!Input.GetMouseButton(2))
		{
			return;
		}
		transform2 = dummyTarget;
		if (!(transform2 == null))
		{
			Transform cameraTarget = CameraTarget;
			transform2 = dummyTarget;
			if (!(transform2 != cameraTarget))
			{
				goto IL_0852;
			}
		}
		else
		{
			GameObject gameObject = new GameObject("Camera Target");
			GameObject gameObject2 = gameObject;
			Transform transform3 = gameObject2.transform;
			dummyTarget = transform3;
		}
		Transform cameraTarget3 = CameraTarget;
		Transform transform4 = dummyTarget;
		Vector3 position = cameraTarget3.position;
		Transform transform5 = transform4;
		Transform obj = transform5;
		Vector3 position2 = default(Vector3);
		position2.x = position.x;
		position2.y = position.y;
		position2.z = position.z;
		obj.position = position2;
		cameraTarget3 = CameraTarget;
		Transform transform6 = dummyTarget;
		Quaternion rotation = cameraTarget3.rotation;
		transform5 = transform6;
		transform5.rotation = rotation;
		Transform cameraTarget4 = dummyTarget;
		CameraTarget = cameraTarget4;
		previousSmoothing = MovementSmoothing;
		MovementSmoothing = false;
		goto IL_0852;
		IL_0852:
		float axis4 = Input.GetAxis("Mouse Y");
		mouseY = axis4;
		float axis5 = Input.GetAxis("Mouse X");
		Transform transform7 = cameraTransform;
		mouseX = axis5;
		num9 = axis5;
		cameraTarget3 = transform7;
		Vector3 vector = cameraTarget3.TransformDirection(num9, mouseY, 0f);
		moveVector.x = vector.x;
		moveVector.y = vector.y;
		moveVector.z = vector.z;
		long num12 = NativeIntrinsics.Asm("xorps xmm1,xmm0");
		long num13 = NativeIntrinsics.Asm("xorps xmm2,xmm0");
		long num14 = NativeIntrinsics.Asm("xorps xmm3,xmm0");
		Transform transform8 = dummyTarget;
		Vector3 translation = default(Vector3);
		translation.x = NativeIntrinsics.Reinterpret<float>(num12);
		translation.y = NativeIntrinsics.Reinterpret<float>(num13);
		translation.z = NativeIntrinsics.Reinterpret<float>(num14);
		transform8.Translate(translation, NativeIntrinsics.Reinterpret<Space>(0));
	}

	// Token: 0x060001EE RID: 494 RVA: 0x00254BB0 File Offset: 0x002531B0
	public CameraController()
	{
		float z = Vector3.zeroVector.z;
		currentVelocity.x = Vector3.zeroVector.x;
		currentVelocity.y = Vector3.zeroVector.y;
		currentVelocity.z = z;
		((Behaviour)this)..ctor();
	}
}

