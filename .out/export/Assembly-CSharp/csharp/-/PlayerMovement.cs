using System;
using Il2CppStudio.Native;
using UnityEngine;

// Token: 0x0200004F RID: 79
public class PlayerMovement : MonoBehaviour
{
	// Token: 0x0400023A RID: 570 Offset: 0x20
	public CharacterController cc;

	// Token: 0x0400023B RID: 571 Offset: 0x28
	public float walkSpeed;

	// Token: 0x0400023C RID: 572 Offset: 0x2C
	public float runSpeed;

	// Token: 0x0400023D RID: 573 Offset: 0x30
	public float stamina;

	// Token: 0x0400023E RID: 574 Offset: 0x34
	public float staminaDrop;

	// Token: 0x0400023F RID: 575 Offset: 0x38
	public float staminaRise;

	// Token: 0x04000240 RID: 576 Offset: 0x3C
	public float staminaMax;

	// Token: 0x04000241 RID: 577 Offset: 0x40
	private float sensitivity;

	// Token: 0x04000242 RID: 578 Offset: 0x44
	private float mouseSensitivity;

	// Token: 0x04000243 RID: 579 Offset: 0x48
	private bool running;

	// Token: 0x0600017A RID: 378 RVA: 0x00256E80 File Offset: 0x00255480
	private void Awake()
	{
		float num = PlayerPrefs.GetFloat("MouseSensitivity");
		mouseSensitivity = num;
	}

	// Token: 0x0600017B RID: 379 RVA: 0x002573C0 File Offset: 0x002559C0
	private void Start()
	{
		stamina = staminaMax;
		Time.timeScale = 1f;
	}

	// Token: 0x0600017C RID: 380 RVA: 0x002573E0 File Offset: 0x002559E0
	private void Update()
	{
		bool button = Input.GetButton("Run");
		running = button;
		Transform transform = this.transform;
		Transform transform2 = transform;
		Quaternion rotation = transform2.rotation;
		Vector3 vector = Quaternion.Internal_ToEulerRad(rotation);
		Vector3 euler = default(Vector3);
		euler.x = vector.x * 57.29578f;
		euler.y = vector.y * 57.29578f;
		euler.z = vector.z * 57.29578f;
		Vector3 vector2 = Quaternion.Internal_MakePositive(euler);
		float axis = Input.GetAxis("Mouse X");
		float num = mouseSensitivity;
		float deltaTime = Time.deltaTime;
		float timeScale = Time.timeScale;
		Vector3 euler2 = default(Vector3);
		euler2.x = ((float)NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)") + 0f) * (MathF.PI / 180f);
		euler2.y = (timeScale * (num * axis * deltaTime) + (float)(int)NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFF7C]")) * (MathF.PI / 180f);
		euler2.z = (vector2.z + 0f) * (MathF.PI / 180f);
		Quaternion rotation2 = Quaternion.Internal_FromEulerRad(euler2);
		Transform transform3 = this.transform;
		transform2 = transform3;
		transform2.rotation = rotation2;
		PlayerMove();
		CharacterController characterController = cc;
		Vector3 velocity = characterController.velocity;
		Vector3 vector3 = default(Vector3);
		vector3.x = velocity.x;
		vector3.y = velocity.y;
		vector3.y = (timeScale * (num * axis * deltaTime) + (float)(int)NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFF7C]")) * (MathF.PI / 180f);
		vector3.z = velocity.z;
		float magnitude = vector3.magnitude;
		float num2 = magnitude;
		float minMoveDistance = cc.minMoveDistance;
		num2 = minMoveDistance;
		if (!(magnitude > minMoveDistance))
		{
			num2 = staminaMax;
			if (staminaMax > stamina)
			{
				float num3 = stamina;
				float num4 = staminaRise;
				float deltaTime2 = Time.deltaTime;
				stamina = deltaTime2 * num4 + num3;
				num2 = deltaTime2 * num4 + num3;
			}
		}
		else if (running)
		{
			float num5 = stamina;
			float num6 = staminaDrop;
			float deltaTime3 = Time.deltaTime;
			stamina = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("maxss xmm7,xmm11"));
			num2 = deltaTime3 * num6;
		}
	}

	// Token: 0x0600017D RID: 381 RVA: 0x00256EC0 File Offset: 0x002554C0
	private void MouseMove()
	{
		Transform transform = this.transform;
		Quaternion rotation = transform.rotation;
		Vector3 vector = Quaternion.Internal_ToEulerRad(rotation);
		Vector3 euler = default(Vector3);
		euler.x = vector.x * 57.29578f;
		euler.y = vector.y * 57.29578f;
		euler.z = vector.z * 57.29578f;
		Vector3 vector2 = Quaternion.Internal_MakePositive(euler);
		float axis = Input.GetAxis("Mouse X");
		float num = mouseSensitivity;
		float deltaTime = Time.deltaTime;
		float timeScale = Time.timeScale;
		Vector3 euler2 = default(Vector3);
		euler2.x = ((float)NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)") + 0f) * (MathF.PI / 180f);
		euler2.y = (timeScale * (num * axis * deltaTime) + (float)(int)NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFF8C]")) * (MathF.PI / 180f);
		euler2.z = (vector2.z + 0f) * (MathF.PI / 180f);
		Quaternion rotation2 = Quaternion.Internal_FromEulerRad(euler2);
		Transform transform2 = this.transform;
		transform2.rotation = rotation2;
	}

	// Token: 0x0600017E RID: 382 RVA: 0x00257090 File Offset: 0x00255690
	private void PlayerMove()
	{
		Transform transform = this.transform;
		int num = (int)NativeIntrinsics.Asm("*(A_0 + 0x28)");
		Vector3 right = transform.right;
		float axis = Input.GetAxis("Strafe");
		Transform transform2 = this.transform;
		Vector3 forward = transform2.forward;
		float axis2 = Input.GetAxis("Forward");
		Vector3 vector = default(Vector3);
		vector.x = forward.x * axis2 + right.x * axis;
		vector.y = forward.y * axis2 + right.y * axis;
		vector.z = forward.z * axis2 + right.z * axis;
		float magnitude = vector.magnitude;
		float num2 = magnitude;
		float num3 = default(float);
		if (!(0f > magnitude) && !(num2 > 1f))
		{
			num3 = num2;
		}
		CharacterController characterController = cc;
		sensitivity = num3;
		Vector3 vector2 = default(Vector3);
		Vector3 normalized = vector2.normalized;
		float num4 = (float)num * normalized.z * sensitivity;
		float num5 = (float)num * normalized.x * sensitivity;
		float num6 = (float)num * normalized.y * sensitivity;
		float deltaTime = Time.deltaTime;
		Vector3 motion = default(Vector3);
		motion.x = num5 * deltaTime;
		motion.y = num6 * deltaTime;
		motion.z = num4 * deltaTime;
		CollisionFlags collisionFlags = characterController.Move(motion);
	}

	// Token: 0x0600017F RID: 383 RVA: 0x002572E0 File Offset: 0x002558E0
	public void StaminaUpdate()
	{
		CharacterController characterController = cc;
		Vector3 velocity = characterController.velocity;
		Vector3 vector = default(Vector3);
		vector.x = velocity.x;
		vector.y = velocity.y;
		vector.z = velocity.z;
		float magnitude = vector.magnitude;
		float num = magnitude;
		float minMoveDistance = cc.minMoveDistance;
		num = minMoveDistance;
		if (!(magnitude > minMoveDistance))
		{
			num = staminaMax;
			if (staminaMax > stamina)
			{
				float num2 = stamina;
				float num3 = staminaRise;
				float deltaTime = Time.deltaTime;
				stamina = deltaTime * num3 + num2;
				num = deltaTime * num3 + num2;
			}
		}
		else if (running)
		{
			float num4 = stamina;
			float num5 = staminaDrop;
			float deltaTime2 = Time.deltaTime;
			stamina = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("maxss xmm7,xmm0"));
		}
	}
}

