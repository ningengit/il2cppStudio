using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

// Token: 0x02000050 RID: 80
public class PlayerScript : MonoBehaviour
{
	// Token: 0x04000244 RID: 580 Offset: 0x20
	public GameControllerScript gc;

	// Token: 0x04000245 RID: 581 Offset: 0x28
	public BaldiScript baldi;

	// Token: 0x04000246 RID: 582 Offset: 0x30
	public DoorScript door;

	// Token: 0x04000247 RID: 583 Offset: 0x38
	public PlaytimeScript playtime;

	// Token: 0x04000248 RID: 584 Offset: 0x40
	public bool gameOver;

	// Token: 0x04000249 RID: 585 Offset: 0x41
	public bool jumpRope;

	// Token: 0x0400024A RID: 586 Offset: 0x42
	public bool sweeping;

	// Token: 0x0400024B RID: 587 Offset: 0x43
	public bool hugging;

	// Token: 0x0400024C RID: 588 Offset: 0x44
	public bool bootsActive;

	// Token: 0x0400024D RID: 589 Offset: 0x48
	public int principalBugFixer;

	// Token: 0x0400024E RID: 590 Offset: 0x4C
	public float sweepingFailsave;

	// Token: 0x0400024F RID: 591 Offset: 0x50
	public float fliparoo;

	// Token: 0x04000250 RID: 592 Offset: 0x54
	public float flipaturn;

	// Token: 0x04000251 RID: 593 Offset: 0x58
	private Quaternion playerRotation;

	// Token: 0x04000252 RID: 594 Offset: 0x68
	public Vector3 frozenPosition;

	// Token: 0x04000253 RID: 595 Offset: 0x74
	private bool sensitivityActive;

	// Token: 0x04000254 RID: 596 Offset: 0x78
	private float sensitivity;

	// Token: 0x04000255 RID: 597 Offset: 0x7C
	public float mouseSensitivity;

	// Token: 0x04000256 RID: 598 Offset: 0x80
	public float walkSpeed;

	// Token: 0x04000257 RID: 599 Offset: 0x84
	public float runSpeed;

	// Token: 0x04000258 RID: 600 Offset: 0x88
	public float slowSpeed;

	// Token: 0x04000259 RID: 601 Offset: 0x8C
	public float maxStamina;

	// Token: 0x0400025A RID: 602 Offset: 0x90
	public float staminaRate;

	// Token: 0x0400025B RID: 603 Offset: 0x94
	public float guilt;

	// Token: 0x0400025C RID: 604 Offset: 0x98
	public float initGuilt;

	// Token: 0x0400025D RID: 605 Offset: 0x9C
	private Vector3 moveDirection;

	// Token: 0x0400025E RID: 606 Offset: 0xA8
	private float playerSpeed;

	// Token: 0x0400025F RID: 607 Offset: 0xAC
	public float stamina;

	// Token: 0x04000260 RID: 608 Offset: 0xB0
	public CharacterController cc;

	// Token: 0x04000261 RID: 609 Offset: 0xB8
	public NavMeshAgent gottaSweep;

	// Token: 0x04000262 RID: 610 Offset: 0xC0
	public NavMeshAgent firstPrize;

	// Token: 0x04000263 RID: 611 Offset: 0xC8
	public Transform firstPrizeTransform;

	// Token: 0x04000264 RID: 612 Offset: 0xD0
	public Slider staminaBar;

	// Token: 0x04000265 RID: 613 Offset: 0xD8
	public float db;

	// Token: 0x04000266 RID: 614 Offset: 0xE0
	public string guiltType;

	// Token: 0x04000267 RID: 615 Offset: 0xE8
	public GameObject jumpRopeScreen;

	// Token: 0x04000268 RID: 616 Offset: 0xF0
	public float height;

	// Token: 0x04000269 RID: 617 Offset: 0xF8
	public Material blackSky;

	// Token: 0x0400026A RID: 618 Offset: 0x100
	public Canvas hud;

	// Token: 0x06000181 RID: 385 RVA: 0x002589A0 File Offset: 0x00256FA0
	private void Start()
	{
		int num = PlayerPrefs.GetInt("AnalogMove");
		if (num == 1)
		{
			sensitivityActive = (byte)num != 0;
		}
		Vector3 position = transform.position;
		float num3 = maxStamina;
		height = position.y;
		stamina = num3;
		playerRotation = transform.rotation;
		mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity");
		principalBugFixer = 1;
		flipaturn = 1f;
	}

	// Token: 0x06000182 RID: 386 RVA: 0x00258A70 File Offset: 0x00257070
	private void Update()
	{
		Transform transform = this.transform;
		Vector3 position = this.transform.position;
		float y = height;
		Vector3 position3 = new Vector3(position.x, y, this.transform.position.z);
		transform.position = position3;
		MouseMove();
		PlayerMove();
		Vector3 velocity = cc.velocity;
		Vector3 vector = default(Vector3);
		vector.x = velocity.x;
		vector.y = velocity.y;
		vector.y = y;
		vector.z = velocity.z;
		float num4 = default(float);
		if (!(vector.magnitude > 0.1f))
		{
			_ = maxStamina;
			if (maxStamina > stamina)
			{
				float num = stamina;
				stamina = Time.deltaTime * staminaRate + num;
			}
		}
		else
		{
			bool actionKey = Singleton<InputManager>.Instance.GetActionKey(InputAction.Run);
			bool num3 = (stamina > 0f && actionKey);
			num4 = stamina;
			if (num3)
			{
				stamina = num4 - Time.deltaTime * staminaRate;
			}
			if ((0f > stamina) & (stamina > -5f))
			{
				stamina = -5f;
			}
		}
		float value = stamina / maxStamina * 100f;
		staminaBar.value = value;
		num4 = guilt;
		if (guilt > 0f)
		{
			guilt = num4 - Time.deltaTime;
		}
		Vector3 vector2 = cc.velocity;
		if (vector2.magnitude > 0f)
		{
			gc.LockMouse();
		}
		bool num6 = jumpRope;
		Vector3 position4 = this.transform.position;
		Vector3 vector3 = new Vector3(position4.x - frozenPosition.x, position4.y - frozenPosition.y, position4.z - frozenPosition.z);
		if (num6 && (!(vector3.magnitude < 1f)))
		{
			jumpRopeScreen.SetActive(value: false);
			jumpRope = false;
		}
		num4 = sweepingFailsave;
		if (!(sweepingFailsave > 0f))
		{
			sweeping = false;
		}
		else
		{
			sweepingFailsave = num4 - Time.deltaTime;
		}
	}

	// Token: 0x06000183 RID: 387 RVA: 0x002578C0 File Offset: 0x00255EC0
	private void MouseMove()
	{
		Quaternion rotation = default(Quaternion);
		rotation.x = NativeIntrinsics.Reinterpret<float>(playerRotation);
		Vector3 vector = Quaternion.Internal_ToEulerRad(rotation);
		Vector3 euler = new Vector3(vector.x * 57.29578f, vector.y * 57.29578f, vector.z * 57.29578f);
		Vector3 vector2 = Quaternion.Internal_MakePositive(euler);
		Quaternion rotation2 = default(Quaternion);
		rotation2.x = NativeIntrinsics.Reinterpret<float>(playerRotation);
		Vector3 vector3 = Quaternion.Internal_ToEulerRad(rotation2);
		Vector3 euler2 = new Vector3(vector3.x * 57.29578f, vector3.y * 57.29578f, vector3.z * 57.29578f);
		Vector3 vector4 = Quaternion.Internal_MakePositive(euler2);
		float z = fliparoo * ((float)Math.PI / 180f);
		Vector3 euler3 = new Vector3(vector2.x * ((float)Math.PI / 180f), vector4.y * ((float)Math.PI / 180f), z);
		Quaternion rotation3 = Quaternion.Internal_FromEulerRad(euler3);
		playerRotation = rotation3;
		Vector3 vector5 = Quaternion.Internal_ToEulerRad(rotation3);
		Vector3 euler4 = new Vector3(vector5.x * 57.29578f, vector5.y * 57.29578f, vector5.z * 57.29578f);
		Vector3 vector6 = Quaternion.Internal_MakePositive(euler4);
		float z2 = Vector3.up.z;
		float axis = Input.GetAxis("Mouse X");
		float num = mouseSensitivity;
		float timeScale = Time.timeScale;
		float z3 = (z2 * axis * num * timeScale * flipaturn + vector6.z) * ((float)Math.PI / 180f);
		_ = flipaturn;
		float y = (vector5.y * 57.29578f * axis * num * timeScale * flipaturn + (float)(int)NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFF5C]")) * ((float)Math.PI / 180f);
		Vector3 euler5 = new Vector3(((float)NativeIntrinsics.Asm("struct_chunk(StaticFieldExpr+0x0, 8)") * axis * num * timeScale * flipaturn + (float)NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)")) * ((float)Math.PI / 180f), y, z3);
		Quaternion quaternion = Quaternion.Internal_FromEulerRad(euler5);
		playerRotation = quaternion;
		Transform transform = this.transform;
		Quaternion value = playerRotation;
		Quaternion rotation4 = default(Quaternion);
		rotation4.x = NativeIntrinsics.Reinterpret<float>(value);
		transform.rotation = rotation4;
	}

	// Token: 0x06000184 RID: 388 RVA: 0x00258010 File Offset: 0x00256610
	private void PlayerMove()
	{
		int num = (int)NativeIntrinsics.Asm("init_flag_19E3BFE");
		if ((int)NativeIntrinsics.Asm("init_flag_19E3BFE") == 0)
		{
			NativeIntrinsics.Stmt("mov [1819E3BFEh],cl ; value = 0x1");
			num = 1;
		}
		_ = Vector3.zero.z;
		if (num == 0)
		{
			NativeIntrinsics.Stmt("mov byte ptr [1819E3BFEh],1 ; value = 0x1");
		}
		float z = Vector3.zero.z;
		InputManager instance = Singleton<InputManager>.Instance;
		float num2 = z;
		if (instance.GetActionKey(InputAction.MoveForward))
		{
			Vector3 forward = transform.forward;
			_ = forward.x;
			_ = forward.y;
			_ = forward.z;
		}
		else
		{
			Vector3 forward = default(Vector3);
			_ = forward.y;
		}
		long num3 = default(long);
		long num4 = default(long);
		long num5 = default(long);
		if (Singleton<InputManager>.Instance.GetActionKey(InputAction.MoveBackward))
		{
			_ = transform.forward;
			NativeIntrinsics.Asm("xorps xmm0,xmm6");
			num3 = NativeIntrinsics.Asm("xorps xmm0,xmm6");
			num4 = NativeIntrinsics.Asm("xorps xmm10,xmm6");
			num5 = NativeIntrinsics.Asm("xorps xmm11,xmm6");
		}
		float y = default(float);
		float num6 = default(float);
		if (Singleton<InputManager>.Instance.GetActionKey(InputAction.MoveLeft))
		{
			_ = transform.right;
			NativeIntrinsics.Asm("xorps xmm7,xmm6");
			NativeIntrinsics.Asm("xorps xmm8,xmm6");
			NativeIntrinsics.Asm("xorps xmm9,xmm6");
		}
		else
		{
			Vector3 vector = default(Vector3);
			y = vector.y;
			num6 = num2;
		}
		float x = default(float);
		if (Singleton<InputManager>.Instance.GetActionKey(InputAction.MoveRight))
		{
			Vector3 right = transform.right;
			x = right.x;
			y = right.y;
			num6 = right.z;
		}
		_ = stamina;
		float y2 = default(float);
		float z2 = default(float);
		if (!(stamina > 0f))
		{
			playerSpeed = walkSpeed;
			if (sensitivityActive)
			{
				y2 = (float)num4 + y;
				z2 = (float)num5 + num6;
				goto IL_0370;
			}
			sensitivity = 1f;
		}
		else if (!Singleton<InputManager>.Instance.GetActionKey(InputAction.Run))
		{
			playerSpeed = walkSpeed;
			if (sensitivityActive)
			{
				y2 = y + (float)num4;
				z2 = num6 + (float)num5;
				goto IL_0370;
			}
			sensitivity = 1f;
		}
		else
		{
			CharacterController characterController = cc;
			playerSpeed = runSpeed;
			sensitivity = 1f;
			if (characterController.velocity.magnitude > 0.1f && (!hugging & !sweeping) && !(0.1f < guilt))
			{
				guiltType = "running";
				guilt = 0.1f;
			}
		}
		goto IL_054e;
		IL_0370:
		Vector3 vector3 = default(Vector3);
		vector3.y = y2;
		vector3.z = z2;
		float magnitude = vector3.magnitude;
		y2 = magnitude;
		if (!(0f > magnitude))
		{
			z2 = 1f;
			if (!(y2 > 1f))
			{
				sensitivity = y2;
				goto IL_054e;
			}
		}
		else
		{
			z2 = 0f;
		}
		sensitivity = z2;
		goto IL_054e;
		IL_054e:
		float num7 = playerSpeed;
		playerSpeed = Time.deltaTime * num7;
		Vector3 vector4 = new Vector3(x + (float)num3, y + (float)num4, num6 + (float)num5);
		Vector3 normalized = vector4.normalized;
		bool num8 = (!jumpRope && (!hugging & !sweeping));
		_ = sensitivity;
		float z3 = playerSpeed * normalized.z * sensitivity;
		_ = playerSpeed * normalized.y * sensitivity;
		moveDirection = new Vector3(playerSpeed * normalized.x * sensitivity, playerSpeed * normalized.y * sensitivity, z3);
		_ = playerSpeed * normalized.x * sensitivity;
		if (!num8)
		{
			if (sweeping && !bootsActive)
			{
				Vector3 velocity2 = gottaSweep.velocity;
				float deltaTime = Time.deltaTime;
				float z4 = moveDirection.z * 0.3f + velocity2.z * deltaTime;
				_ = moveDirection.y * 0.3f + velocity2.y * deltaTime;
				moveDirection = new Vector3(moveDirection.x * 0.3f + velocity2.x * deltaTime, moveDirection.y * 0.3f + velocity2.y * deltaTime, z4);
				_ = moveDirection.x * 0.3f + velocity2.x * deltaTime;
			}
			else if (hugging && !bootsActive)
			{
				Vector3 velocity3 = firstPrize.velocity;
				float deltaTime2 = Time.deltaTime;
				Vector3 position = firstPrizeTransform.position;
				float num9 = height;
				Vector3 position2 = firstPrizeTransform.position;
				int num10 = Mathf.RoundToInt(firstPrizeTransform.forward.x);
				int num11 = Mathf.RoundToInt(firstPrizeTransform.forward.z);
				Vector3 position3 = transform.position;
				float z5 = ((float)num11 * 3f + position2.z - position3.z + velocity3.z * 1.2f * deltaTime2) * (float)principalBugFixer;
				moveDirection = new Vector3(((float)num10 * 3f + position.x - position3.x + velocity3.x * 1.2f * deltaTime2) * (float)principalBugFixer, (num9 + 0f - position3.y + velocity3.y * 1.2f * deltaTime2) * (float)principalBugFixer, z5);
			}
			else if (jumpRope)
			{
				moveDirection = Vector3.zero;
			}
		}
		CharacterController characterController2 = cc;
		Vector3 motion = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z);
		characterController2.Move(motion);
	}

	// Token: 0x06000185 RID: 389 RVA: 0x00258800 File Offset: 0x00256E00
	private void StaminaCheck()
	{
		Vector3 vector = cc.velocity;
		if (!(vector.magnitude > 0.1f))
		{
			_ = maxStamina;
			if (maxStamina > stamina)
			{
				float num = stamina;
				stamina = Time.deltaTime * staminaRate + num;
			}
		}
		else
		{
			bool actionKey = Singleton<InputManager>.Instance.GetActionKey(InputAction.Run);
			bool num3 = (stamina > 0f && actionKey);
			float num4 = stamina;
			if (num3)
			{
				stamina = num4 - Time.deltaTime * staminaRate;
			}
			if ((0f > stamina) & (stamina > -5f))
			{
				stamina = -5f;
			}
		}
		staminaBar.value = stamina / maxStamina * 100f;
	}

	// Token: 0x06000186 RID: 390 RVA: 0x00257BC0 File Offset: 0x002561C0
	private void OnTriggerEnter(Collider other)
	{
		bool flag = other.transform.name == "Baldi";
		if (gc.debugMode || !flag)
		{
			bool flag2 = other.transform.name == "Playtime";
			if ((!jumpRope && flag2) & !(0f < playtime.playCool))
			{
				jumpRopeScreen.SetActive(value: true);
				jumpRope = true;
				frozenPosition = transform.position;
			}
		}
		else
		{
			Material skybox = blackSky;
			gameOver = true;
			RenderSettings.skybox = skybox;
			StartCoroutine(KeepTheHudOff());
		}
	}

	// Token: 0x06000187 RID: 391 RVA: 0x00257850 File Offset: 0x00255E50
	public IEnumerator KeepTheHudOff()
	{
		// Il2CppStudio: coroutine reconstructed from <KeepTheHudOff>d__6.MoveNext (state machine class hidden).
		while (gameOver)
		{
			hud.enabled = false;
			jumpRopeScreen.SetActive(value: false);
			yield return new WaitForEndOfFrame();
		}
	}

	// Token: 0x06000188 RID: 392 RVA: 0x00257EE0 File Offset: 0x002564E0
	private void OnTriggerStay(Collider other)
	{
		if (other.transform.name == "Gotta Sweep")
		{
			sweeping = true;
			sweepingFailsave = 1f;
		}
		else if ((other.transform.name == "1st Prize") & (firstPrize.velocity.magnitude > 5f))
		{
			hugging = true;
			sweepingFailsave = 1f;
		}
	}

	// Token: 0x06000189 RID: 393 RVA: 0x00257D80 File Offset: 0x00256380
	private void OnTriggerExit(Collider other)
	{
		if (other.transform.name != "Office Trigger")
		{
			if (other.transform.name == "Gotta Sweep")
			{
				sweeping = false;
			}
			else if (other.transform.name == "1st Prize")
			{
				hugging = false;
			}
			return;
		}
		DoorScript doorScript = door;
		if (!(doorScript.lockTime < guilt))
		{
			guiltType = "escape";
			guilt = doorScript.lockTime;
		}
	}

	// Token: 0x0600018A RID: 394 RVA: 0x002587D0 File Offset: 0x00256DD0
	public void ResetGuilt(string type, float amount)
	{
		if (!(amount < guilt))
		{
			guilt = amount;
			guiltType = type;
		}
	}

	// Token: 0x0600018B RID: 395 RVA: 0x00257810 File Offset: 0x00255E10
	private void GuiltCheck()
	{
		float num = guilt;
		if (guilt > 0f)
		{
			guilt = num - Time.deltaTime;
		}
	}

	// Token: 0x0600018C RID: 396 RVA: 0x00257710 File Offset: 0x00255D10
	public void ActivateJumpRope()
	{
		jumpRopeScreen.SetActive(value: true);
		jumpRope = true;
		frozenPosition = transform.position;
	}

	// Token: 0x0600018D RID: 397 RVA: 0x002577E0 File Offset: 0x00255DE0
	public void DeactivateJumpRope()
	{
		jumpRopeScreen.SetActive(value: false);
		jumpRope = false;
	}

	// Token: 0x0600018E RID: 398 RVA: 0x00257690 File Offset: 0x00255C90
	public void ActivateBoots()
	{
		bootsActive = true;
		StartCoroutine(BootTimer());
	}

	// Token: 0x0600018F RID: 399 RVA: 0x00257770 File Offset: 0x00255D70
	private IEnumerator BootTimer()
	{
		// Il2CppStudio: coroutine reconstructed from <BootTimer>d__14.MoveNext (state machine class hidden).
		float time = 15f;
		while (time > 0f)
		{
			time -= Time.deltaTime;
			yield return null;
		}
		bootsActive = false;
	}
}

