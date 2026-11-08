using Il2CppStudio.Native;
using TMPro;
using UnityEngine;

// Token: 0x02000014 RID: 20
public class JumpRopeScript : MonoBehaviour
{
	// Token: 0x0400007B RID: 123 Offset: 0x20
	public TMP_Text jumpCount;

	// Token: 0x0400007C RID: 124 Offset: 0x28
	public Animator rope;

	// Token: 0x0400007D RID: 125 Offset: 0x30
	public CameraScript cs;

	// Token: 0x0400007E RID: 126 Offset: 0x38
	public PlayerScript ps;

	// Token: 0x0400007F RID: 127 Offset: 0x40
	public PlaytimeScript playtime;

	// Token: 0x04000080 RID: 128 Offset: 0x48
	public int jumps;

	// Token: 0x04000081 RID: 129 Offset: 0x4C
	public float jumpDelay;

	// Token: 0x04000082 RID: 130 Offset: 0x50
	public float ropePosition;

	// Token: 0x04000083 RID: 131 Offset: 0x54
	public bool ropeHit;

	// Token: 0x04000084 RID: 132 Offset: 0x55
	public bool jumpStarted;

	// Token: 0x0600005F RID: 95 RVA: 0x0024A3A0 File Offset: 0x002489A0
	private void OnEnable()
	{
		TMP_Text tMP_Text = jumpCount;
		jumpDelay = 1f;
		ropeHit = true;
		jumps = 0;
		int num = default(int);
		num.m_value = 0;
		string text = num.ToString();
		string text2 = text + "/5";
		cs.jumpHeight = 0f;
		playtime.audioDevice.PlayOneShot(playtime.aud_ReadyGo);
	}

	// Token: 0x06000060 RID: 96 RVA: 0x0024A7F0 File Offset: 0x00248DF0
	private void Update()
	{
		float num = jumpDelay;
		if (!(jumpDelay > 0f))
		{
			if (!jumpStarted)
			{
				Animator animator = rope;
				jumpStarted = true;
				ropePosition = 1f;
				animator.SetTrigger("ActivateJumpRope");
				ropeHit = false;
			}
		}
		else
		{
			float deltaTime = Time.deltaTime;
			jumpDelay = num - deltaTime;
		}
		num = ropePosition;
		if (!(ropePosition > 0f))
		{
			if (!ropeHit)
			{
				RopeHit();
			}
		}
		else
		{
			float deltaTime2 = Time.deltaTime;
			ropePosition = num - deltaTime2;
			num = deltaTime2;
		}
	}

	// Token: 0x06000061 RID: 97 RVA: 0x0024A470 File Offset: 0x00248A70
	private unsafe void RopeHit()
	{
		CameraScript cameraScript = cs;
		ropeHit = true;
		if (0.2f < cameraScript.jumpHeight)
		{
			PlaytimeScript playtimeScript = playtime;
			playtimeScript.audioDevice.Stop();
			long num = jumps;
			AudioSource audioDevice = playtime.audioDevice;
			audioDevice.PlayOneShot(playtime.aud_Numbers[(int)num]);
			jumps++;
			TMP_Text tMP_Text = jumpCount;
			string text = ((int*)(NativeIntrinsics.Reinterpret<long>(this) + 72L))->ToString();
			string text2 = text + "/5";
			string text3 = text2;
			jumpDelay = 0.5f;
			if (jumps >= 5)
			{
				playtime.audioDevice.Stop();
				playtime.audioDevice.PlayOneShot(playtime.aud_Congrats);
				ps.DeactivateJumpRope();
				jumpStarted = false;
				return;
			}
		}
		else
		{
			TMP_Text tMP_Text2 = jumpCount;
			jumps = 0;
			string text4 = ((int*)(NativeIntrinsics.Reinterpret<long>(this) + 72L))->ToString();
			string text5 = text4 + "/5";
			string text3 = text5;
			PlaytimeScript playtimeScript2 = playtime;
			jumpDelay = 2f;
			playtimeScript2.audioDevice.PlayOneShot(playtime.aud_Oops);
		}
		jumpStarted = false;
	}

	// Token: 0x06000062 RID: 98 RVA: 0x0024A6A0 File Offset: 0x00248CA0
	private unsafe void Success()
	{
		PlaytimeScript playtimeScript = playtime;
		playtimeScript.audioDevice.Stop();
		long num = jumps;
		AudioSource audioDevice = playtime.audioDevice;
		audioDevice.PlayOneShot(playtime.aud_Numbers[(int)num]);
		jumps++;
		TMP_Text tMP_Text = jumpCount;
		string text = ((int*)(NativeIntrinsics.Reinterpret<long>(this) + 72L))->ToString();
		string text2 = text + "/5";
		string text3 = text2;
		jumpDelay = 0.5f;
		if (jumps >= 5)
		{
			PlaytimeScript playtimeScript2 = playtime;
			playtimeScript2.audioDevice.Stop();
			playtimeScript2 = playtime;
			playtimeScript2.audioDevice.PlayOneShot(playtimeScript2.aud_Congrats);
			ps.DeactivateJumpRope();
		}
	}

	// Token: 0x06000063 RID: 99 RVA: 0x0024A2F0 File Offset: 0x002488F0
	private unsafe void Fail()
	{
		TMP_Text tMP_Text = jumpCount;
		jumps = 0;
		string text = ((int*)(NativeIntrinsics.Reinterpret<long>(this) + 72L))->ToString();
		string text2 = text + "/5";
		PlaytimeScript playtimeScript = playtime;
		jumpDelay = 2f;
		playtimeScript.audioDevice.PlayOneShot(playtime.aud_Oops);
	}
}

