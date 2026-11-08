using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using Mono.Globalization.Unicode;
using Mono.Xml;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

// Token: 0x02000050 RID: 80
public class PlayerScript : MonoBehaviour
{
	// Token: 0x02000051 RID: 81
	private sealed class <BootTimer>d__14 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x0400026B RID: 619 Offset: 0x10
		private int <>1__state;

		// Token: 0x0400026C RID: 620 Offset: 0x18
		private object <>2__current;

		// Token: 0x0400026D RID: 621 Offset: 0x20
		public PlayerScript <>4__this;

		// Token: 0x0400026E RID: 622 Offset: 0x28
		private float <time>5__2;

		private object System.Collections.Generic.IEnumerator<System.Object>.Current => <>2__current;

		private object System.Collections.IEnumerator.Current => <>2__current;

		// Token: 0x06000191 RID: 401 RVA: 0x00250B50 File Offset: 0x0024F150
		public <BootTimer>d__14(int <>1__state)
		{
			((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
			this.<>1__state = <>1__state;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00250B40 File Offset: 0x0024F140
		private void System.IDisposable.Dispose()
		{
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00260D10 File Offset: 0x0025F310
		private bool MoveNext()
		{
			int num = <>1__state;
			if (<>1__state == 0)
			{
				<time>5__2 = 15f;
			}
			else if (num != 1)
			{
				goto IL_007e;
			}
			float num2 = <time>5__2;
			<>1__state = -1;
			float num3 = num2;
			if (<time>5__2 > 0f)
			{
				float deltaTime = Time.deltaTime;
				<>2__current = null;
				<time>5__2 = num3 - deltaTime;
				<>1__state = 1;
				return true;
			}
			<>4__this.bootsActive = false;
			goto IL_007e;
			IL_007e:
			return false;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00260DA0 File Offset: 0x0025F3A0
		private void System.Collections.IEnumerator.Reset()
		{
			long num = NativeIntrinsics.CallNative("sub_184BE0", NativeIntrinsics.Asm("rax"), 0L, 0L, 0L);
			NativeIntrinsics.Reinterpret<NotSupportedException>(num)..ctor();
			throw null;
		}
	}

	// Token: 0x02000052 RID: 82
	private sealed class <KeepTheHudOff>d__6 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x0400026F RID: 623 Offset: 0x10
		private int <>1__state;

		// Token: 0x04000270 RID: 624 Offset: 0x18
		private object <>2__current;

		// Token: 0x04000271 RID: 625 Offset: 0x20
		public PlayerScript <>4__this;

		private object System.Collections.Generic.IEnumerator<System.Object>.Current => <>2__current;

		private object System.Collections.IEnumerator.Current => <>2__current;

		// Token: 0x06000197 RID: 407 RVA: 0x00250B50 File Offset: 0x0024F150
		public <KeepTheHudOff>d__6(int <>1__state)
		{
			((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
			this.<>1__state = <>1__state;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00250B40 File Offset: 0x0024F140
		private void System.IDisposable.Dispose()
		{
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00261890 File Offset: 0x0025FE90
		private bool MoveNext()
		{
			if ((uint)<>1__state <= 1u)
			{
				PlayerScript playerScript = <>4__this;
				<>1__state = -1;
				PlayerScript playerScript2 = playerScript;
				if (playerScript2.gameOver)
				{
					playerScript2.hud.enabled = false;
					playerScript2.jumpRopeScreen.SetActive(value: false);
					WaitForEndOfFrame waitForEndOfFrame = (WaitForEndOfFrame)(object)new Mono.Globalization.Unicode.ContractionComparer();
					<>2__current = waitForEndOfFrame;
					<>1__state = 1;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00261960 File Offset: 0x0025FF60
		private void System.Collections.IEnumerator.Reset()
		{
			long num = NativeIntrinsics.CallNative("sub_184BE0", NativeIntrinsics.Asm("rax"), 0L, 0L, 0L);
			NativeIntrinsics.Reinterpret<NotSupportedException>(num)..ctor();
			throw null;
		}
	}

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
		int num2 = num;
		if (num == 1)
		{
			sensitivityActive = (byte)num2 != 0;
		}
		Transform transform = this.transform;
		Vector3 position = transform.position;
		float num3 = maxStamina;
		height = position.y;
		stamina = num3;
		Transform transform2 = this.transform;
		Quaternion rotation = transform2.rotation;
		playerRotation.x = rotation.x;
		playerRotation.y = rotation.y;
		playerRotation.z = rotation.z;
		playerRotation.w = rotation.w;
		float num4 = PlayerPrefs.GetFloat("MouseSensitivity");
		mouseSensitivity = num4;
		principalBugFixer = 1;
		flipaturn = 1f;
	}

	// Token: 0x06000182 RID: 386 RVA: 0x00258A70 File Offset: 0x00257070
	private void Update()
	{
		Transform transform = this.transform;
		Transform transform2 = this.transform;
		Vector3 position = transform2.position;
		float y = height;
		Transform transform3 = this.transform;
		Vector3 position2 = transform3.position;
		Vector3 position3 = default(Vector3);
		position3.x = position.x;
		position3.y = y;
		position3.z = position2.z;
		transform.position = position3;
		MouseMove();
		PlayerMove();
		CharacterController characterController = cc;
		Vector3 velocity = characterController.velocity;
		Vector3 vector = default(Vector3);
		vector.x = velocity.x;
		vector.y = velocity.y;
		vector.y = y;
		vector.z = velocity.z;
		float magnitude = vector.magnitude;
		float num = magnitude;
		if (!(magnitude > 0.1f))
		{
			num = maxStamina;
			if (maxStamina > stamina)
			{
				float num2 = stamina;
				float num3 = staminaRate;
				float deltaTime = Time.deltaTime;
				stamina = deltaTime * num3 + num2;
			}
		}
		else
		{
			InputManager instance = Singleton<InputManager>.Instance;
			bool actionKey = instance.GetActionKey(NativeIntrinsics.Reinterpret<InputAction>(9));
			int num4 = ((stamina > 0f && actionKey) ? 1 : 0);
			num = stamina;
			if (num4 != 0)
			{
				float num5 = staminaRate;
				float deltaTime2 = Time.deltaTime;
				stamina = num - deltaTime2 * num5;
			}
			if ((0f > stamina) & (stamina > -5f))
			{
				stamina = -5f;
			}
		}
		float num6 = stamina / maxStamina * 100f;
		num = guilt;
		float num7 = num6;
		if (guilt > 0f)
		{
			float deltaTime3 = Time.deltaTime;
			guilt = num - deltaTime3;
			num = deltaTime3;
		}
		Vector3 velocity2 = cc.velocity;
		Vector3 vector2 = default(Vector3);
		vector2.x = velocity2.x;
		vector2.y = velocity2.y;
		vector2.z = velocity2.z;
		float magnitude2 = vector2.magnitude;
		num = magnitude2;
		if (magnitude2 > 0f)
		{
			gc.LockMouse();
		}
		int num8 = (jumpRope ? 1 : 0);
		Transform transform4 = this.transform;
		Transform transform5 = transform4;
		Vector3 position4 = transform5.position;
		Vector3 vector3 = default(Vector3);
		vector3.x = position4.x - frozenPosition.x;
		vector3.y = position4.y - frozenPosition.y;
		vector3.z = position4.z - frozenPosition.z;
		float magnitude3 = vector3.magnitude;
		int num9 = ((!(magnitude3 < 1f)) ? 1 : 0);
		num = magnitude3;
		if (((uint)num8 & ((!(magnitude3 < 1f)) ? 1u : 0u)) != 0)
		{
			jumpRopeScreen.SetActive(value: false);
			jumpRope = false;
		}
		num = sweepingFailsave;
		if (!(sweepingFailsave > 0f))
		{
			sweeping = false;
			return;
		}
		float deltaTime4 = Time.deltaTime;
		sweepingFailsave = num - deltaTime4;
		num = deltaTime4;
	}

	// Token: 0x06000183 RID: 387 RVA: 0x002578C0 File Offset: 0x00255EC0
	private void MouseMove()
	{
		Quaternion quaternion = playerRotation;
		NativeIntrinsics.Stmt("movaps [rbp-39h],xmm0 ; value = LocalExpr");
		Vector3 vector = Quaternion.Internal_ToEulerRad(NativeIntrinsics.Reinterpret<Quaternion>(NativeIntrinsics.Asm("&stack[0x0]") + -95L + -57L));
		NativeIntrinsics.Stmt("movss dword ptr [rbp-59h],xmm0 ; value = BinaryExpr");
		NativeIntrinsics.Stmt("movss dword ptr [rbp-55h],xmm0 ; value = BinaryExpr");
		NativeIntrinsics.Stmt("movss dword ptr [rbp-51h],xmm1 ; value = BinaryExpr");
		Vector3 vector2 = Quaternion.Internal_MakePositive(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0x0]") + -95L + -89L));
		Quaternion quaternion2 = playerRotation;
		NativeIntrinsics.Stmt("movaps [rbp-39h],xmm0 ; value = LocalExpr");
		Vector3 vector3 = Quaternion.Internal_ToEulerRad(NativeIntrinsics.Reinterpret<Quaternion>(NativeIntrinsics.Asm("&stack[0x0]") + -95L + -57L));
		NativeIntrinsics.Stmt("movss dword ptr [rbp-59h],xmm0 ; value = BinaryExpr");
		NativeIntrinsics.Stmt("movss dword ptr [rbp-55h],xmm0 ; value = BinaryExpr");
		NativeIntrinsics.Stmt("movss dword ptr [rbp-51h],xmm1 ; value = BinaryExpr");
		Vector3 vector4 = Quaternion.Internal_MakePositive(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0x0]") + -95L + -89L));
		float num = fliparoo * (MathF.PI / 180f);
		NativeIntrinsics.Stmt("movss dword ptr [rbp-55h],xmm0 ; value = BinaryExpr");
		NativeIntrinsics.Stmt("movss dword ptr [rbp-59h],xmm6 ; value = BinaryExpr");
		NativeIntrinsics.Stmt("movss dword ptr [rbp-51h],xmm1 ; value = LocalExpr");
		Quaternion quaternion3 = Quaternion.Internal_FromEulerRad(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0x0]") + -95L + -89L));
		playerRotation.x = quaternion3.x;
		playerRotation.y = quaternion3.y;
		playerRotation.z = quaternion3.z;
		playerRotation.w = quaternion3.w;
		NativeIntrinsics.Stmt("movdqa [rbp-39h],xmm0 ; value = StructChunkExpr");
		Vector3 vector5 = Quaternion.Internal_ToEulerRad(NativeIntrinsics.Reinterpret<Quaternion>(NativeIntrinsics.Asm("&stack[0x0]") + -95L + -57L));
		NativeIntrinsics.Stmt("movss dword ptr [rbp-59h],xmm0 ; value = BinaryExpr");
		NativeIntrinsics.Stmt("movss dword ptr [rbp-55h],xmm0 ; value = BinaryExpr");
		NativeIntrinsics.Stmt("movss dword ptr [rbp-51h],xmm1 ; value = BinaryExpr");
		Vector3 vector6 = Quaternion.Internal_MakePositive(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0x0]") + -95L + -89L));
		NativeIntrinsics.Stmt("movsd qword ptr [rbp-49h],xmm12 ; value = StructChunkExpr");
		NativeIntrinsics.Stmt("movsd qword ptr [rbp-59h],xmm6 ; value = StructChunkExpr");
		float z = Vector3.upVector.z;
		float axis = Input.GetAxis("Mouse X");
		float num2 = mouseSensitivity;
		float timeScale = Time.timeScale;
		float num3 = (z * axis * num2 * timeScale * flipaturn + vector6.z) * (MathF.PI / 180f);
		float num4 = flipaturn;
		float num5 = ((float)(int)NativeIntrinsics.Asm("*(BinaryExpr + 0xFFFFFFFFFFFFFFAB)") * axis * num2 * timeScale * flipaturn + (float)(int)NativeIntrinsics.Asm("*(BinaryExpr + 0xFFFFFFFFFFFFFFBB)")) * (MathF.PI / 180f);
		NativeIntrinsics.Stmt("movss dword ptr [rbp-59h],xmm6 ; value = BinaryExpr");
		NativeIntrinsics.Stmt("movss dword ptr [rbp-55h],xmm9 ; value = LocalExpr");
		NativeIntrinsics.Stmt("movss dword ptr [rbp-51h],xmm10 ; value = LocalExpr");
		Quaternion quaternion4 = Quaternion.Internal_FromEulerRad(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0x0]") + -95L + -89L));
		playerRotation.x = quaternion4.x;
		playerRotation.y = quaternion4.y;
		playerRotation.z = quaternion4.z;
		playerRotation.w = quaternion4.w;
		Transform transform = this.transform;
		Quaternion quaternion5 = playerRotation;
		NativeIntrinsics.Stmt("movaps [rbp-39h],xmm0 ; value = LocalExpr");
		transform.rotation = NativeIntrinsics.Reinterpret<Quaternion>(NativeIntrinsics.Asm("&stack[0x0]") + -95L + -57L);
	}

	// Token: 0x06000184 RID: 388 RVA: 0x00258010 File Offset: 0x00256610
	private unsafe void PlayerMove()
	{
		int num = (int)NativeIntrinsics.Asm("global_19E3BFE");
		if ((int)NativeIntrinsics.Asm("global_19E3BFE") == 0)
		{
			NativeIntrinsics.Stmt("mov [1819E3BFEh],cl ; value = 0x1");
		}
		float z = Vector3.zeroVector.z;
		float num2 = z;
		long num3 = NativeIntrinsics.Asm("struct_chunk(StaticFieldExpr+0x0, 8)");
		if (1 == 0)
		{
			NativeIntrinsics.Stmt("mov byte ptr [1819E3BFEh],1 ; value = 0x1");
		}
		float z2 = Vector3.zeroVector.z;
		InputManager instance = Singleton<InputManager>.Instance;
		float num4 = z2;
		long num5 = NativeIntrinsics.Asm("struct_chunk(StaticFieldExpr+0x0, 8)");
		if (instance.GetActionKey(NativeIntrinsics.Reinterpret<InputAction>(2)))
		{
			Transform transform = this.transform;
			Vector3 forward = transform.forward;
			float x = forward.x;
			float y = forward.y;
			float z3 = forward.z;
		}
		else
		{
			int num6 = (int)NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFF2C]");
			float z3 = num2;
		}
		InputManager instance2 = Singleton<InputManager>.Instance;
		long num9 = default(long);
		long num10 = default(long);
		if (instance2.GetActionKey(NativeIntrinsics.Reinterpret<InputAction>(3)))
		{
			Transform transform2 = this.transform;
			Vector3 forward2 = transform2.forward;
			long num7 = NativeIntrinsics.Asm("xorps xmm0,xmm6");
			long num8 = NativeIntrinsics.Asm("xorps xmm0,xmm6");
			num9 = NativeIntrinsics.Asm("xorps xmm10,xmm6");
			num10 = NativeIntrinsics.Asm("xorps xmm11,xmm6");
		}
		InputManager instance3 = Singleton<InputManager>.Instance;
		float num14 = default(float);
		if (instance3.GetActionKey(NativeIntrinsics.Reinterpret<InputAction>(0)))
		{
			Transform transform3 = this.transform;
			Vector3 right = transform3.right;
			num5 = NativeIntrinsics.Asm("xorps xmm7,xmm6");
			long num11 = NativeIntrinsics.Asm("xorps xmm8,xmm6");
			long num12 = NativeIntrinsics.Asm("xorps xmm9,xmm6");
		}
		else
		{
			int num13 = (int)NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFF3C]");
			num14 = num4;
		}
		InputManager instance4 = Singleton<InputManager>.Instance;
		float y2 = default(float);
		if (instance4.GetActionKey(NativeIntrinsics.Reinterpret<InputAction>(1)))
		{
			Transform transform4 = this.transform;
			Vector3 right2 = transform4.right;
			float x2 = right2.x;
			y2 = right2.y;
			num14 = right2.z;
		}
		float num15 = stamina;
		float num16;
		if (!(stamina > 0f))
		{
			playerSpeed = walkSpeed;
			if (sensitivityActive)
			{
				num15 = (float)num9 + y2;
				num16 = (float)num10 + num14;
				goto IL_02b7;
			}
			sensitivity = 1f;
		}
		else
		{
			InputManager instance5 = Singleton<InputManager>.Instance;
			if (!instance5.GetActionKey(NativeIntrinsics.Reinterpret<InputAction>(9)))
			{
				playerSpeed = walkSpeed;
				if (sensitivityActive)
				{
					num15 = y2 + (float)num9;
					num16 = num14 + (float)num10;
					goto IL_02b7;
				}
				sensitivity = 1f;
			}
			else
			{
				CharacterController characterController = cc;
				playerSpeed = runSpeed;
				sensitivity = 1f;
				CharacterController characterController2 = characterController;
				Vector3 velocity = characterController2.velocity;
				float magnitude = ((Vector3*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -121L))->magnitude;
				int num17 = ((magnitude > 0.1f && (!hugging & !sweeping)) ? 1 : 0);
				num15 = magnitude;
				if (num17 != 0 && !(0.1f < guilt))
				{
					guiltType = "running";
					guilt = 0.1f;
				}
			}
		}
		goto IL_03b5;
		IL_02b7:
		float magnitude2 = ((Vector3*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -121L))->magnitude;
		num15 = magnitude2;
		if (!(0f > magnitude2) && !(num15 > 1f))
		{
			sensitivity = num15;
			num16 = num15;
		}
		else
		{
			sensitivity = 0f;
		}
		goto IL_03b5;
		IL_03b5:
		float num18 = playerSpeed;
		float deltaTime = Time.deltaTime;
		playerSpeed = deltaTime * num18;
		Vector3 normalized = ((Vector3*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -121L))->normalized;
		int num19 = ((!jumpRope && (!hugging & !sweeping)) ? 1 : 0);
		float num20 = sensitivity;
		float num21 = playerSpeed * normalized.z * sensitivity;
		float num22 = playerSpeed * normalized.y * sensitivity;
		moveDirection = NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("unpcklps xmm2,xmm0"));
		moveDirection.z = num21;
		num16 = num20;
		long num23 = NativeIntrinsics.Asm("unpcklps xmm2,xmm0");
		float num24 = num21;
		num15 = num22;
		if (num19 == 0)
		{
			if (sweeping && !bootsActive)
			{
				Vector3 velocity2 = gottaSweep.velocity;
				float deltaTime2 = Time.deltaTime;
				float z4 = moveDirection.z * 0.3f + velocity2.z * deltaTime2;
				float num25 = moveDirection.y * 0.3f + velocity2.y * deltaTime2;
				moveDirection = NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("unpcklps xmm3,xmm1"));
				moveDirection.z = z4;
				num16 = num25;
				long num26 = NativeIntrinsics.Asm("unpcklps xmm3,xmm1");
			}
			else if (hugging && !bootsActive)
			{
				Vector3 velocity3 = firstPrize.velocity;
				float deltaTime3 = Time.deltaTime;
				Vector3 position = firstPrizeTransform.position;
				float num27 = height;
				Vector3 position2 = firstPrizeTransform.position;
				int num28 = Mathf.RoundToInt(firstPrizeTransform.forward.x);
				int num29 = Mathf.RoundToInt(firstPrizeTransform.forward.z);
				Transform transform5 = this.transform;
				Vector3 position3 = transform5.position;
				float z5 = ((float)num29 * 3f + position2.z - position3.z + velocity3.z * 1.2f * deltaTime3) * (float)principalBugFixer;
				moveDirection = NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("unpcklps xmm7,xmm9"));
				moveDirection.z = z5;
			}
			else if (jumpRope)
			{
				moveDirection = NativeIntrinsics.Reinterpret<Vector3>(0);
				moveDirection.z = 0f;
			}
		}
		CharacterController characterController3 = cc;
		float z6 = moveDirection.z;
		CollisionFlags collisionFlags = characterController3.Move(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -121L));
	}

	// Token: 0x06000185 RID: 389 RVA: 0x00258800 File Offset: 0x00256E00
	private void StaminaCheck()
	{
		CharacterController characterController = cc;
		Vector3 velocity = characterController.velocity;
		Vector3 vector = default(Vector3);
		vector.x = velocity.x;
		vector.y = velocity.y;
		vector.z = velocity.z;
		float magnitude = vector.magnitude;
		float num = magnitude;
		if (!(magnitude > 0.1f))
		{
			num = maxStamina;
			if (maxStamina > stamina)
			{
				float num2 = stamina;
				float num3 = staminaRate;
				float deltaTime = Time.deltaTime;
				stamina = deltaTime * num3 + num2;
			}
		}
		else
		{
			InputManager instance = Singleton<InputManager>.Instance;
			bool actionKey = instance.GetActionKey(NativeIntrinsics.Reinterpret<InputAction>(9));
			int num4 = ((stamina > 0f && actionKey) ? 1 : 0);
			num = stamina;
			if (num4 != 0)
			{
				float num5 = staminaRate;
				float deltaTime2 = Time.deltaTime;
				stamina = num - deltaTime2 * num5;
			}
			if ((0f > stamina) & (stamina > -5f))
			{
				stamina = -5f;
			}
		}
		throw null;
	}

	// Token: 0x06000186 RID: 390 RVA: 0x00257BC0 File Offset: 0x002561C0
	private void OnTriggerEnter(Collider other)
	{
		Transform transform = other.transform;
		string a = transform.name;
		bool flag = string.Equals(a, "Baldi");
		GameControllerScript gameControllerScript = gc;
		if (gameControllerScript.debugMode || !flag)
		{
			Transform transform2 = other.transform;
			Transform transform3 = transform2;
			string a2 = transform3.name;
			bool flag2 = string.Equals(a2, "Playtime");
			bool flag3 = flag2;
			int num = ((!jumpRope && flag3) ? 1 : 0);
			int num2 = (flag3 ? 1 : 0);
			if (((uint)num & ((!(0f < playtime.playCool)) ? 1u : 0u)) != 0)
			{
				jumpRopeScreen.SetActive(value: true);
				jumpRope = true;
				Transform transform4 = this.transform;
				Transform transform5 = transform4;
				Vector3 position = transform5.position;
				frozenPosition.x = position.x;
				frozenPosition.y = position.y;
				frozenPosition.z = position.z;
				long num3 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
			}
		}
		else
		{
			Material skybox = blackSky;
			gameOver = true;
			RenderSettings.skybox = skybox;
			<KeepTheHudOff>d__6 <KeepTheHudOff>d__ = NativeIntrinsics.Reinterpret<<KeepTheHudOff>d__6>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(PlayerScript+<KeepTheHudOff>d__6)"));
			((Mono.Xml.SecurityParser)(object)<KeepTheHudOff>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
			<KeepTheHudOff>d__.<>1__state = 0;
			<KeepTheHudOff>d__.<>4__this = this;
			Coroutine coroutine = StartCoroutine(<KeepTheHudOff>d__);
		}
	}

	// Token: 0x06000187 RID: 391 RVA: 0x00257850 File Offset: 0x00255E50
	public IEnumerator KeepTheHudOff()
	{
		<KeepTheHudOff>d__6 <KeepTheHudOff>d__ = NativeIntrinsics.Reinterpret<<KeepTheHudOff>d__6>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(PlayerScript+<KeepTheHudOff>d__6)"));
		((Mono.Xml.SecurityParser)(object)<KeepTheHudOff>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<KeepTheHudOff>d__.<>1__state = 0;
		<KeepTheHudOff>d__.<>4__this = this;
		return <KeepTheHudOff>d__;
	}

	// Token: 0x06000188 RID: 392 RVA: 0x00257EE0 File Offset: 0x002564E0
	private void OnTriggerStay(Collider other)
	{
		Transform transform = other.transform;
		Transform transform2 = transform;
		string a = transform2.name;
		bool flag = string.Equals(a, "Gotta Sweep");
		bool flag2 = flag;
		if (!flag)
		{
			Transform transform3 = other.transform;
			Transform transform4 = transform3;
			string a2 = transform4.name;
			bool flag3 = string.Equals(a2, "1st Prize");
			bool flag4 = flag3;
			Vector3 velocity = firstPrize.velocity;
			Vector3 vector = default(Vector3);
			vector.x = velocity.x;
			vector.y = velocity.y;
			vector.z = velocity.z;
			float magnitude = vector.magnitude;
			int num = ((magnitude > 5f && flag3) ? 1 : 0);
			float num2 = magnitude;
			if (num != 0)
			{
				hugging = true;
				sweepingFailsave = 1f;
			}
		}
		else
		{
			sweeping = true;
			sweepingFailsave = 1f;
		}
	}

	// Token: 0x06000189 RID: 393 RVA: 0x00257D80 File Offset: 0x00256380
	private void OnTriggerExit(Collider other)
	{
		Transform transform = other.transform;
		string a = transform.name;
		if (!string.Equals(a, "Office Trigger"))
		{
			Transform transform2 = other.transform;
			Transform transform3 = transform2;
			string a2 = transform3.name;
			bool flag = string.Equals(a2, "Gotta Sweep");
			bool flag2 = flag;
			if (!flag)
			{
				Transform transform4 = other.transform;
				Transform transform5 = transform4;
				string a3 = transform5.name;
				bool flag3 = string.Equals(a3, "1st Prize");
				bool flag4 = flag3;
				if (flag3)
				{
					hugging = false;
				}
			}
			else
			{
				sweeping = false;
			}
		}
		else
		{
			DoorScript doorScript = door;
			float lockTime = doorScript.lockTime;
			if (!(doorScript.lockTime < guilt))
			{
				guiltType = "escape";
				guilt = lockTime;
			}
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
			float deltaTime = Time.deltaTime;
			guilt = num - deltaTime;
			float num2 = deltaTime;
		}
	}

	// Token: 0x0600018C RID: 396 RVA: 0x00257710 File Offset: 0x00255D10
	public void ActivateJumpRope()
	{
		jumpRopeScreen.SetActive(value: true);
		jumpRope = true;
		Transform transform = this.transform;
		Vector3 position = transform.position;
		frozenPosition.x = position.x;
		frozenPosition.y = position.y;
		frozenPosition.z = position.z;
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
		<BootTimer>d__14 <BootTimer>d__ = NativeIntrinsics.Reinterpret<<BootTimer>d__14>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(PlayerScript+<BootTimer>d__14)"));
		((Mono.Xml.SecurityParser)(object)<BootTimer>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<BootTimer>d__.<>1__state = 0;
		<BootTimer>d__.<>4__this = this;
		Coroutine coroutine = StartCoroutine(<BootTimer>d__);
	}

	// Token: 0x0600018F RID: 399 RVA: 0x00257770 File Offset: 0x00255D70
	private IEnumerator BootTimer()
	{
		<BootTimer>d__14 <BootTimer>d__ = NativeIntrinsics.Reinterpret<<BootTimer>d__14>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(PlayerScript+<BootTimer>d__14)"));
		((Mono.Xml.SecurityParser)(object)<BootTimer>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<BootTimer>d__.<>1__state = 0;
		<BootTimer>d__.<>4__this = this;
		return <BootTimer>d__;
	}
}

