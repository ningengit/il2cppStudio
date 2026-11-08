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
		mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
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
		running = Input.GetButton("Run");
		Vector3 vector = Quaternion.Internal_ToEulerRad(transform.rotation);
		Vector3 euler = new Vector3(vector.x * 57.29578f, vector.y * 57.29578f, vector.z * 57.29578f);
		Vector3 vector2 = Quaternion.Internal_MakePositive(euler);
		float axis = Input.GetAxis("Mouse X");
		float num = mouseSensitivity;
		float deltaTime = Time.deltaTime;
		float timeScale = Time.timeScale;
		Vector3 euler2 = new Vector3(((float)NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)") + 0f) * ((float)Math.PI / 180f), (timeScale * (num * axis * deltaTime) + (float)(int)NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFF7C]")) * ((float)Math.PI / 180f), (vector2.z + 0f) * ((float)Math.PI / 180f));
		Quaternion rotation = Quaternion.Internal_FromEulerRad(euler2);
		transform.rotation = rotation;
		PlayerMove();
		Vector3 velocity = cc.velocity;
		Vector3 vector3 = default(Vector3);
		vector3.x = velocity.x;
		vector3.y = velocity.y;
		vector3.y = (timeScale * (num * axis * deltaTime) + (float)(int)NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFF7C]")) * ((float)Math.PI / 180f);
		vector3.z = velocity.z;
		if (!(vector3.magnitude > cc.minMoveDistance))
		{
			_ = staminaMax;
			if (staminaMax > stamina)
			{
				float num2 = stamina;
				stamina = Time.deltaTime * staminaRise + num2;
			}
		}
		else if (running)
		{
			float num4 = stamina;
			float num5 = staminaDrop;
			stamina = Math.Max(num4 - Time.deltaTime * num5, 0f);
		}
	}

	// Token: 0x0600017D RID: 381 RVA: 0x00256EC0 File Offset: 0x002554C0
	private void MouseMove()
	{
		Vector3 vector = Quaternion.Internal_ToEulerRad(transform.rotation);
		Vector3 euler = new Vector3(vector.x * 57.29578f, vector.y * 57.29578f, vector.z * 57.29578f);
		Vector3 vector2 = Quaternion.Internal_MakePositive(euler);
		float axis = Input.GetAxis("Mouse X");
		float num = mouseSensitivity;
		float deltaTime = Time.deltaTime;
		float timeScale = Time.timeScale;
		Vector3 euler2 = new Vector3(((float)NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)") + 0f) * ((float)Math.PI / 180f), (timeScale * (num * axis * deltaTime) + (float)(int)NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFF8C]")) * ((float)Math.PI / 180f), (vector2.z + 0f) * ((float)Math.PI / 180f));
		Quaternion rotation = Quaternion.Internal_FromEulerRad(euler2);
		transform.rotation = rotation;
	}

	// Token: 0x0600017E RID: 382 RVA: 0x00257090 File Offset: 0x00255690
	private void PlayerMove()
	{
		Transform transform = this.transform;
		int num = (int)NativeIntrinsics.Asm("*(A_0 + 0x28)");
		Vector3 right = transform.right;
		float axis = Input.GetAxis("Strafe");
		Vector3 forward = this.transform.forward;
		float axis2 = Input.GetAxis("Forward");
		Vector3 vector = new Vector3(forward.x * axis2 + right.x * axis, forward.y * axis2 + right.y * axis, forward.z * axis2 + right.z * axis);
		float magnitude = vector.magnitude;
		float num2 = 0f;
		float num3 = magnitude;
		if (!(0f > magnitude))
		{
			num2 = 1f;
			if (!(num3 > 1f))
			{
				num2 = num3;
			}
		}
		CharacterController characterController = cc;
		sensitivity = num2;
		Vector3 normalized = vector.normalized;
		float num4 = (float)num * normalized.z * sensitivity;
		float num5 = (float)num * normalized.x * sensitivity;
		float num6 = (float)num * normalized.y * sensitivity;
		float deltaTime = Time.deltaTime;
		Vector3 motion = new Vector3(num5 * deltaTime, num6 * deltaTime, num4 * deltaTime);
		characterController.Move(motion);
	}

	// Token: 0x0600017F RID: 383 RVA: 0x002572E0 File Offset: 0x002558E0
	public void StaminaUpdate()
	{
		Vector3 vector = cc.velocity;
		if (!(vector.magnitude > cc.minMoveDistance))
		{
			_ = staminaMax;
			if (staminaMax > stamina)
			{
				float num = stamina;
				stamina = Time.deltaTime * staminaRise + num;
			}
		}
		else if (running)
		{
			float num3 = stamina;
			float num4 = staminaDrop;
			stamina = Math.Max(num3 - Time.deltaTime * num4, 0f);
		}
	}
}

