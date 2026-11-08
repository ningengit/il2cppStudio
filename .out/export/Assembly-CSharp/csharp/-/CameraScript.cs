using Il2CppStudio.Native;
using UnityEngine;

// Token: 0x0200003E RID: 62
public class CameraScript : MonoBehaviour
{
	// Token: 0x040001D4 RID: 468 Offset: 0x20
	public GameObject player;

	// Token: 0x040001D5 RID: 469 Offset: 0x28
	public PlayerScript ps;

	// Token: 0x040001D6 RID: 470 Offset: 0x30
	public Transform baldi;

	// Token: 0x040001D7 RID: 471 Offset: 0x38
	public Vector3 BaldiOffset;

	// Token: 0x040001D8 RID: 472 Offset: 0x44
	public float initVelocity;

	// Token: 0x040001D9 RID: 473 Offset: 0x48
	public float velocity;

	// Token: 0x040001DA RID: 474 Offset: 0x4C
	public float gravity;

	// Token: 0x040001DB RID: 475 Offset: 0x50
	private int lookBehind;

	// Token: 0x040001DC RID: 476 Offset: 0x54
	public Vector3 offset;

	// Token: 0x040001DD RID: 477 Offset: 0x60
	public float jumpHeight;

	// Token: 0x040001DE RID: 478 Offset: 0x64
	public Vector3 jumpHeightV3;

	// Token: 0x0600013B RID: 315 RVA: 0x00241120 File Offset: 0x0023F720
	private void Start()
	{
		Transform transform = this.transform;
		Vector3 position = transform.position;
		Transform transform2 = player.transform;
		Vector3 position2 = transform2.position;
		offset = NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("unpcklps xmm6,xmm7"));
		offset.z = position.z - position2.z;
	}

	// Token: 0x0600013C RID: 316 RVA: 0x002411D0 File Offset: 0x0023F7D0
	private void Update()
	{
		if (!ps.jumpRope)
		{
			InputManager instance = Singleton<InputManager>.Instance;
			int num = 0 - (instance.currentKeyStates[10] ? 1 : 0);
			lookBehind = (int)NativeIntrinsics.Asm("sbb ecx,ecx") & 0xB4;
			return;
		}
		float num2 = gravity;
		float num3 = velocity;
		float deltaTime = Time.deltaTime;
		float num4 = jumpHeight;
		velocity = num3 - deltaTime * num2;
		float deltaTime2 = Time.deltaTime;
		jumpHeight = deltaTime2 * (num3 - deltaTime * num2) + num4;
		float num5 = deltaTime2 * (num3 - deltaTime * num2) + num4;
		if (!(0f < deltaTime2 * (num3 - deltaTime * num2) + num4))
		{
			jumpHeight = 0f;
			InputManager instance2 = Singleton<InputManager>.Instance;
			if (instance2.currentKeyStates[11])
			{
				velocity = initVelocity;
			}
		}
		float y = jumpHeight;
		jumpHeightV3.y = y;
		jumpHeightV3 = NativeIntrinsics.Reinterpret<Vector3>(0);
		jumpHeightV3.z = 0f;
	}

	// Token: 0x0600013D RID: 317 RVA: 0x00240890 File Offset: 0x0023EE90
	private void LateUpdate()
	{
		Transform transform = this.transform;
		long num = NativeIntrinsics.Asm("&stack[0x0]") + -95L;
		Transform transform2 = transform;
		Transform transform3 = player.transform;
		transform2 = transform3;
		Vector3 position = transform2.position;
		float num2 = offset.x + position.x;
		float num3 = offset.y + position.y;
		float num4 = offset.z + position.z;
		NativeIntrinsics.Stmt("movss dword ptr [rbp-39h],xmm0 ; value = LocalExpr");
		NativeIntrinsics.Stmt("movss dword ptr [rbp-35h],xmm1 ; value = LocalExpr");
		NativeIntrinsics.Stmt("movss dword ptr [rbp-31h],xmm2 ; value = LocalExpr");
		transform.position = NativeIntrinsics.Reinterpret<Vector3>(num + -57L);
		PlayerScript playerScript = ps;
		int num5 = ((!playerScript.gameOver) ? 1 : 0);
		Transform transform10;
		if (!(!playerScript.gameOver & !playerScript.jumpRope))
		{
			if (playerScript.gameOver)
			{
				Transform transform4 = this.transform;
				Transform transform5 = transform4;
				Transform transform6 = baldi.transform;
				transform5 = transform6;
				Vector3 position2 = transform5.position;
				Transform transform7 = baldi.transform;
				transform5 = transform7;
				Vector3 forward = transform5.forward;
				float num6 = BaldiOffset.z * forward.x + position2.x + 0f;
				float num7 = BaldiOffset.z * forward.y + position2.y + BaldiOffset.y;
				float num8 = BaldiOffset.z * forward.z + position2.z + 0f;
				NativeIntrinsics.Stmt("movss dword ptr [rbp-39h],xmm2 ; value = LocalExpr");
				NativeIntrinsics.Stmt("movss dword ptr [rbp-35h],xmm3 ; value = LocalExpr");
				NativeIntrinsics.Stmt("movss dword ptr [rbp-31h],xmm1 ; value = LocalExpr");
				transform4.position = NativeIntrinsics.Reinterpret<Vector3>(num + -57L);
				Transform transform8 = this.transform;
				transform5 = transform8;
				Vector3 position3 = baldi.position;
				Vector3 position4 = baldi.position;
				float num9 = BaldiOffset.y + position4.y;
				Vector3 position5 = baldi.position;
				NativeIntrinsics.Stmt("movss dword ptr [rbp-31h],xmm0 ; value = FieldExpr");
				NativeIntrinsics.Stmt("movss dword ptr [rbp-39h],xmm7 ; value = FieldExpr");
				NativeIntrinsics.Stmt("movss dword ptr [rbp-35h],xmm6 ; value = LocalExpr");
				transform8.LookAt(NativeIntrinsics.Reinterpret<Vector3>(num + -57L));
				return;
			}
			if (!playerScript.jumpRope)
			{
				return;
			}
			Transform transform9 = this.transform;
			transform10 = transform9;
			Transform transform11 = player.transform;
			Vector3 position6 = transform11.position;
			num2 = offset.x + position6.x + jumpHeightV3.x;
			num3 = offset.y + position6.y + jumpHeightV3.y;
			num4 = offset.z + position6.z + jumpHeightV3.z;
			NativeIntrinsics.Stmt("movss dword ptr [rbp-39h],xmm0 ; value = LocalExpr");
			NativeIntrinsics.Stmt("movss dword ptr [rbp-35h],xmm1 ; value = LocalExpr");
			NativeIntrinsics.Stmt("movss dword ptr [rbp-31h],xmm2 ; value = LocalExpr");
			transform10.position = NativeIntrinsics.Reinterpret<Vector3>(num + -57L);
			Transform transform12 = this.transform;
			transform10 = transform12;
			Transform transform13 = player.transform;
			Quaternion rotation = transform13.rotation;
			NativeIntrinsics.Stmt("movaps [rbp-29h],xmm0 ; value = StructChunkExpr");
			long num10 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
		}
		else
		{
			Transform transform14 = this.transform;
			transform10 = transform14;
			Transform transform15 = player.transform;
			Vector3 position7 = transform15.position;
			float num11 = offset.x + position7.x;
			num3 = offset.y + position7.y;
			num4 = offset.z + position7.z;
			NativeIntrinsics.Stmt("movss dword ptr [rbp-39h],xmm0 ; value = LocalExpr");
			NativeIntrinsics.Stmt("movss dword ptr [rbp-35h],xmm1 ; value = LocalExpr");
			NativeIntrinsics.Stmt("movss dword ptr [rbp-31h],xmm2 ; value = LocalExpr");
			transform10.position = NativeIntrinsics.Reinterpret<Vector3>(num + -57L);
			Transform transform16 = this.transform;
			transform10 = transform16;
			Transform transform17 = player.transform;
			Quaternion rotation2 = transform17.rotation;
			float num12 = lookBehind;
			NativeIntrinsics.Stmt("mov dword ptr [rbp-39h],0 ; value = 0x0");
			NativeIntrinsics.Stmt("mov dword ptr [rbp-31h],0 ; value = 0x0");
			NativeIntrinsics.Stmt("movss dword ptr [rbp-35h],xmm1 ; value = BinaryExpr");
			Quaternion quaternion = Quaternion.Internal_FromEulerRad(NativeIntrinsics.Reinterpret<Vector3>(num + -57L));
			num11 = quaternion.x * rotation2.y;
			num3 = quaternion.y * rotation2.x;
			num4 = quaternion.x * rotation2.x;
			float num13 = quaternion.y * rotation2.y;
			NativeIntrinsics.Stmt("movss dword ptr [rbp-29h],xmm10 ; value = BinaryExpr");
			NativeIntrinsics.Stmt("movss dword ptr [rbp-25h],xmm11 ; value = BinaryExpr");
			NativeIntrinsics.Stmt("movss dword ptr [rbp-21h],xmm12 ; value = BinaryExpr");
			NativeIntrinsics.Stmt("movss dword ptr [rbp-1Dh],xmm5 ; value = BinaryExpr");
		}
		transform10.rotation = NativeIntrinsics.Reinterpret<Quaternion>(num + -41L);
	}
}

