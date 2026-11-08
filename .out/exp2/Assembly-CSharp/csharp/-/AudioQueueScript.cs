using UnityEngine;

// Token: 0x02000003 RID: 3
public class AudioQueueScript : MonoBehaviour
{
	// Token: 0x04000004 RID: 4 Offset: 0x20
	private AudioSource audioDevice;

	// Token: 0x04000005 RID: 5 Offset: 0x28
	private int audioInQueue;

	// Token: 0x04000006 RID: 6 Offset: 0x30
	private AudioClip[] audioQueue = new AudioClip[100];

	// Token: 0x06000003 RID: 3 RVA: 0x0023ED00 File Offset: 0x0023D300
	private void Start()
	{
		audioDevice = GetComponent<AudioSource>();
	}

	// Token: 0x06000004 RID: 4 RVA: 0x0023EDC0 File Offset: 0x0023D3C0
	private void Update()
	{
		if (audioDevice.isPlaying || audioInQueue <= 0)
		{
			return;
		}
		audioDevice.PlayOneShot(audioQueue[0]);
		AudioQueueScript audioQueueScript = this;
		if (audioInQueue > 1)
		{
			audioQueueScript = this;
			int num = 1;
			do
			{
				AudioClip audioClip = audioQueueScript.audioQueue[num];
				audioQueueScript.audioQueue[(int)(num - 1L)] = audioClip;
				num++;
			}
			while (num + 1 < audioQueueScript.audioInQueue);
		}
		audioQueueScript.audioInQueue--;
	}

	// Token: 0x06000005 RID: 5 RVA: 0x0023ECB0 File Offset: 0x0023D2B0
	public void QueueAudio(AudioClip sound)
	{
		audioQueue[audioInQueue] = sound;
		audioInQueue++;
	}

	// Token: 0x06000006 RID: 6 RVA: 0x0023EC10 File Offset: 0x0023D210
	private void PlayQueue()
	{
		audioDevice.PlayOneShot(audioQueue[0]);
		AudioQueueScript audioQueueScript = this;
		if (audioInQueue > 1)
		{
			audioQueueScript = this;
			int num = 1;
			do
			{
				AudioClip audioClip = audioQueueScript.audioQueue[num];
				audioQueueScript.audioQueue[(int)(num - 1L)] = audioClip;
				num++;
			}
			while (num + 1 < audioQueueScript.audioInQueue);
		}
		audioQueueScript.audioInQueue--;
	}

	// Token: 0x06000007 RID: 7 RVA: 0x0023ED50 File Offset: 0x0023D350
	private void UnqueueAudio()
	{
		AudioQueueScript audioQueueScript = this;
		if (audioInQueue > 1)
		{
			audioQueueScript = this;
			int num = 1;
			do
			{
				AudioClip[] array = audioQueueScript.audioQueue;
				array[(int)(num - 1L)] = array[num];
				num++;
			}
			while (num + 1 < audioQueueScript.audioInQueue);
		}
		audioQueueScript.audioInQueue--;
	}

	// Token: 0x06000008 RID: 8 RVA: 0x0023EC00 File Offset: 0x0023D200
	public void ClearAudioQueue()
	{
		audioInQueue = 0;
	}
}

