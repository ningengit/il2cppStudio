using Il2CppStudio.Native;
using TMPro;
using UnityEngine;

// Token: 0x02000040 RID: 64
public class DetentionTextScript : MonoBehaviour
{
	// Token: 0x040001E2 RID: 482 Offset: 0x20
	public DoorScript door;

	// Token: 0x040001E3 RID: 483 Offset: 0x28
	private TMP_Text text;

	// Token: 0x06000142 RID: 322 RVA: 0x00243F10 File Offset: 0x00242510
	private void Start()
	{
		TMP_Text component = GetComponent<TMP_Text>();
		text = component;
	}

	// Token: 0x06000143 RID: 323 RVA: 0x00243F60 File Offset: 0x00242560
	private void Update()
	{
		DoorScript doorScript = door;
		float lockTime = doorScript.lockTime;
		TMP_Text tMP_Text = this.text;
		if (doorScript.lockTime > 0f)
		{
			float num = lockTime;
			long num2 = NativeIntrinsics.CallNative("sub_20C080", 0L, 0L, 0L, 0L);
			int num3 = default(int);
			num3.m_value = (int)NativeIntrinsics.Asm("xmm0");
			string text = num3.ToString();
			string text2 = "You have detention! \n" + text + " seconds remain!";
			string text3 = text2;
		}
	}
}

