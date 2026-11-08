using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000015 RID: 21
public class MathGameScript : MonoBehaviour
{
	// Token: 0x04000085 RID: 133 Offset: 0x20
	public GameControllerScript gc;

	// Token: 0x04000086 RID: 134 Offset: 0x28
	public BaldiScript baldiScript;

	// Token: 0x04000087 RID: 135 Offset: 0x30
	public Vector3 playerPosition;

	// Token: 0x04000088 RID: 136 Offset: 0x40
	public GameObject mathGame;

	// Token: 0x04000089 RID: 137 Offset: 0x48
	public RawImage[] results = new RawImage[3];

	// Token: 0x0400008A RID: 138 Offset: 0x50
	public Texture correct;

	// Token: 0x0400008B RID: 139 Offset: 0x58
	public Texture incorrect;

	// Token: 0x0400008C RID: 140 Offset: 0x60
	public TMP_InputField playerAnswer;

	// Token: 0x0400008D RID: 141 Offset: 0x68
	public TMP_Text questionText;

	// Token: 0x0400008E RID: 142 Offset: 0x70
	public TMP_Text questionText2;

	// Token: 0x0400008F RID: 143 Offset: 0x78
	public TMP_Text questionText3;

	// Token: 0x04000090 RID: 144 Offset: 0x80
	public Animator baldiFeed;

	// Token: 0x04000091 RID: 145 Offset: 0x88
	public Transform baldiFeedTransform;

	// Token: 0x04000092 RID: 146 Offset: 0x90
	public AudioClip bal_plus;

	// Token: 0x04000093 RID: 147 Offset: 0x98
	public AudioClip bal_minus;

	// Token: 0x04000094 RID: 148 Offset: 0xA0
	public AudioClip bal_times;

	// Token: 0x04000095 RID: 149 Offset: 0xA8
	public AudioClip bal_divided;

	// Token: 0x04000096 RID: 150 Offset: 0xB0
	public AudioClip bal_equals;

	// Token: 0x04000097 RID: 151 Offset: 0xB8
	public AudioClip bal_howto;

	// Token: 0x04000098 RID: 152 Offset: 0xC0
	public AudioClip bal_intro;

	// Token: 0x04000099 RID: 153 Offset: 0xC8
	public AudioClip bal_screech;

	// Token: 0x0400009A RID: 154 Offset: 0xD0
	public AudioClip[] bal_numbers = new AudioClip[10];

	// Token: 0x0400009B RID: 155 Offset: 0xD8
	public AudioClip[] bal_praises = new AudioClip[5];

	// Token: 0x0400009C RID: 156 Offset: 0xE0
	public AudioClip[] bal_problems = new AudioClip[3];

	// Token: 0x0400009D RID: 157 Offset: 0xE8
	public Button firstButton;

	// Token: 0x0400009E RID: 158 Offset: 0xF0
	private float endDelay;

	// Token: 0x0400009F RID: 159 Offset: 0xF4
	private int problem;

	// Token: 0x040000A0 RID: 160 Offset: 0xF8
	private int audioInQueue;

	// Token: 0x040000A1 RID: 161 Offset: 0xFC
	private float num1;

	// Token: 0x040000A2 RID: 162 Offset: 0x100
	private float num2;

	// Token: 0x040000A3 RID: 163 Offset: 0x104
	private float num3;

	// Token: 0x040000A4 RID: 164 Offset: 0x108
	private int sign;

	// Token: 0x040000A5 RID: 165 Offset: 0x10C
	private float solution;

	// Token: 0x040000A6 RID: 166 Offset: 0x110
	private string[] hintText = new string[2] { "I GET ANGRIER FOR EVERY PROBLEM YOU GET WRONG", "I HEAR EVERY DOOR YOU OPEN" };

	// Token: 0x040000A7 RID: 167 Offset: 0x118
	private string[] endlessHintText = new string[2] { "That's more like it...", "Keep up the good work or see me after class..." };

	// Token: 0x040000A8 RID: 168 Offset: 0x120
	private bool questionInProgress;

	// Token: 0x040000A9 RID: 169 Offset: 0x121
	private bool impossibleMode;

	// Token: 0x040000AA RID: 170 Offset: 0x124
	private int problemsWrong;

	// Token: 0x040000AB RID: 171 Offset: 0x128
	private AudioClip[] audioQueue = new AudioClip[20];

	// Token: 0x040000AC RID: 172 Offset: 0x130
	public AudioSource baldiAudio;

	// Token: 0x06000065 RID: 101 RVA: 0x0024D610 File Offset: 0x0024BC10
	private void Start()
	{
		GameControllerScript gameControllerScript = gc;
		gameControllerScript.learningActive = true;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		GameObject reticle = gameControllerScript.reticle;
		gameControllerScript.mouseLocked = false;
		reticle.SetActive(value: false);
		gameControllerScript.tutorBaldi.Stop();
		if (!gameControllerScript.spoopMode)
		{
			gameControllerScript.schoolMusic.Stop();
			gameControllerScript.learnMusic.Play();
		}
		if (gc.notebooks == 1)
		{
			audioQueue[(int)audioInQueue] = bal_intro;
			long num2 = audioInQueue + 1L;
			_ = audioInQueue;
			_ = audioQueue;
			audioInQueue = (int)num2;
			_ = bal_howto;
			NativeIntrinsics.Stmt("mov [rcx+rax*8+28h],rdx ; value = LocalExpr");
			audioInQueue++;
		}
		NewProblem();
		if (gc.spoopMode)
		{
			Transform obj = baldiFeedTransform;
			Vector3 position = new Vector3(-1000f, -1000f, 0f);
			obj.position = position;
		}
	}

	// Token: 0x06000066 RID: 102 RVA: 0x0024D860 File Offset: 0x0024BE60
	private void Update()
	{
		Animator animator = default(Animator);
		int value = default(int);
		MathGameScript mathGameScript = default(MathGameScript);
		int num = default(int);
		if (baldiAudio.isPlaying)
		{
			animator = baldiFeed;
			value = 1;
			mathGameScript = this;
			num = 1;
		}
		else
		{
			mathGameScript = this;
			num = 1;
			if ((audioInQueue > 0) & !gc.spoopMode)
			{
				baldiAudio.PlayOneShot(audioQueue[0]);
				mathGameScript = this;
				num = 1;
				if (audioInQueue > 1)
				{
					mathGameScript = this;
					int num2 = 1;
					num = 1;
					do
					{
						AudioClip audioClip = mathGameScript.audioQueue[num2];
						mathGameScript.audioQueue[(int)(num2 - 1L)] = audioClip;
						num2++;
					}
					while (num2 + 1 < mathGameScript.audioInQueue);
				}
				mathGameScript.audioInQueue--;
			}
			animator = mathGameScript.baldiFeed;
			value = 0;
		}
		animator.SetBool("talking", (byte)value != 0);
		if (!Input.GetKeyDown("return"))
		{
			num = (Input.GetKeyDown("enter") ? 1 : 0);
		}
		if (((mathGameScript.questionInProgress ? 1u : 0u) & (uint)num) != 0)
		{
			mathGameScript.questionInProgress = false;
			mathGameScript.CheckAnswer();
		}
		if (mathGameScript.problem <= 3)
		{
			return;
		}
		float num3 = mathGameScript.endDelay;
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		mathGameScript.endDelay = num3 - unscaledDeltaTime;
		if (0f < num3 - unscaledDeltaTime)
		{
			return;
		}
		GC.Collect();
		int num4 = mathGameScript.problemsWrong;
		bool flag = mathGameScript.gc.mode == "endless";
		if (num4 <= 0 && flag)
		{
			BaldiScript baldiScript = mathGameScript.baldiScript;
			baldiScript.baldiAnger -= 1f;
			if (0.5f > baldiScript.baldiAnger - 1f)
			{
				baldiScript.baldiAnger = 0.5f;
			}
			_ = 2f / baldiScript.baldiSpeedScale + baldiScript.baldiAnger;
			baldiScript.baldiWait = baldiScript.baldiAnger * -3f / (2f / baldiScript.baldiSpeedScale + baldiScript.baldiAnger) + 3f;
		}
		mathGameScript.gc.DeactivateLearningGame(mathGameScript.gameObject);
	}

	// Token: 0x06000067 RID: 103 RVA: 0x0024AFF0 File Offset: 0x002495F0
	private void NewProblem()
	{
		playerAnswer.text = string.Empty;
		TMP_InputField tMP_InputField = playerAnswer;
		problem++;
		tMP_InputField.ActivateInputField();
		if (problem > 3)
		{
			GameControllerScript gameControllerScript = gc;
			endDelay = 5f;
			GameControllerScript gameControllerScript2 = gameControllerScript;
			if (gameControllerScript2.spoopMode)
			{
				bool flag = gameControllerScript2.mode == "endless";
				if (!(problemsWrong > 0) && flag)
				{
					Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
				}
				else if (!(gc.mode == "story") | !(problemsWrong >= 3))
				{
					int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
					questionText.text = hintText[num];
					questionText2.text = string.Empty;
				}
				else
				{
					questionText.text = "I HEAR MATH THAT BAD";
					questionText2.text = string.Empty;
					questionText3.text = string.Empty;
					if (this.baldiScript.isActiveAndEnabled)
					{
						BaldiScript baldiScript = this.baldiScript;
						if (!baldiScript.antiHearing && !(7f < baldiScript.currentPriority))
						{
							NavMeshAgent agent = baldiScript.agent;
							Vector3 destination = new Vector3(playerPosition.x, playerPosition.y, playerPosition.z);
							agent.SetDestination(destination);
							baldiScript.currentPriority = 7f;
						}
					}
					gc.failedNotebooks++;
					return;
				}
				questionText3.text = NativeIntrinsics.Reinterpret<string>(NativeIntrinsics.Asm("rdx"));
			}
			else
			{
				questionText.text = "WOW! YOU EXIST!";
			}
			return;
		}
		AudioClip audioClip = bal_problems[(int)(problem - 1L)];
		audioQueue[audioInQueue] = audioClip;
		GameControllerScript gameControllerScript3 = gc;
		audioInQueue++;
		bool num2 = gameControllerScript3.mode == "story";
		TMP_Text tMP_Text = default(TMP_Text);
		if (!((problem <= 2 || gc.notebooks <= 1) && num2))
		{
			bool num3 = gc.mode == "endless";
			if (!((problem <= 2 || gc.notebooks != 2) && num3))
			{
				impossibleMode = true;
				num1 = UnityEngine.Random.Range(1f, 9999f);
				this.num2 = UnityEngine.Random.Range(1f, 9999f);
				this.num3 = UnityEngine.Random.Range(1f, 9999f);
				int num4 = Mathf.RoundToInt(UnityEngine.Random.Range(0, 1));
				AudioClip sound = bal_screech;
				sign = num4;
				QueueAudio(sound);
				AudioClip sound2 = default(AudioClip);
				if (sign == 0)
				{
					object[] array = new object[9];
					tMP_Text = questionText;
					array[0] = "SOLVE MATH Q";
					object obj2 = problem;
					array[1] = obj2;
					array[2] = ": \n";
					obj2 = num1;
					array[3] = obj2;
					array[4] = "+(";
					obj2 = this.num2;
					array[5] = obj2;
					array[6] = "X";
					obj2 = this.num3;
					array[7] = obj2;
					array[8] = "=";
					tMP_Text.text = string.Concat(array);
					QueueAudio(bal_plus);
					QueueAudio(bal_screech);
					sound2 = bal_times;
				}
				else if (sign == 1)
				{
					object[] array = new object[9];
					tMP_Text = questionText;
					array[0] = "SOLVE MATH Q";
					object obj2 = problem;
					array[1] = obj2;
					array[2] = ": \n (";
					obj2 = num1;
					array[3] = obj2;
					array[4] = "/";
					obj2 = this.num2;
					array[5] = obj2;
					array[6] = ")+";
					obj2 = this.num3;
					array[7] = obj2;
					array[8] = "=";
					tMP_Text.text = string.Concat(array);
					QueueAudio(bal_divided);
					QueueAudio(bal_screech);
					sound2 = bal_plus;
				}
				else
				{
					goto IL_0f20;
				}
				QueueAudio(sound2);
				QueueAudio(bal_screech);
				goto IL_0f20;
			}
		}
		num1 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 9f));
		this.num2 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 9f));
		int num5 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));
		AudioClip[] array2 = bal_numbers;
		float f = num1;
		sign = num5;
		int num6 = Mathf.RoundToInt(f);
		audioQueue[audioInQueue] = array2[num6];
		audioInQueue++;
		if (sign == 0)
		{
			float num8 = this.num2 + num1;
			TMP_Text tMP_Text3 = questionText;
			solution = num8;
			object[] array = new object[7];
			tMP_Text = tMP_Text3;
			array[0] = "SOLVE MATH Q";
			object obj2 = problem;
			array[1] = obj2;
			array[2] = ": \n \n";
			obj2 = num1;
			array[3] = obj2;
			array[4] = "+";
			obj2 = this.num2;
			array[5] = obj2;
			array[6] = "=";
			tMP_Text.text = string.Concat(array);
			audioQueue[audioInQueue] = bal_plus;
			audioInQueue++;
		}
		else if (sign == 1)
		{
			float num7 = num1 - this.num2;
			TMP_Text tMP_Text2 = questionText;
			solution = num7;
			object[] array = new object[7];
			tMP_Text = tMP_Text2;
			array[0] = "SOLVE MATH Q";
			object obj2 = problem;
			array[1] = obj2;
			array[2] = ": \n \n";
			obj2 = num1;
			array[3] = obj2;
			array[4] = "-";
			obj2 = this.num2;
			array[5] = obj2;
			array[6] = "=";
			tMP_Text.text = string.Concat(array);
			QueueAudio(bal_minus);
		}
		AudioClip audioClip2 = bal_numbers[Mathf.RoundToInt(this.num2)];
		audioQueue[audioInQueue] = audioClip2;
		_ = audioQueue;
		long num9 = audioInQueue + 1L;
		_ = audioInQueue;
		audioInQueue = (int)num9;
		_ = bal_equals;
		NativeIntrinsics.Stmt("mov [rcx+rax*8+28h],rdx ; value = LocalExpr");
		audioInQueue++;
		questionInProgress = true;
		return;
		IL_0f20:
		num1 = UnityEngine.Random.Range(1f, 9999f);
		this.num2 = UnityEngine.Random.Range(1f, 9999f);
		this.num3 = UnityEngine.Random.Range(1f, 9999f);
		int num10 = (sign = Mathf.RoundToInt(UnityEngine.Random.Range(0, 1)));
		int num11 = num10;
		string text = default(string);
		if (num10 == 0)
		{
			object[] array = new object[9];
			tMP_Text = questionText2;
			array[0] = "SOLVE MATH Q";
			object obj2 = problem;
			array[1] = obj2;
			array[2] = ": \n";
			obj2 = num1;
			array[3] = obj2;
			array[4] = "+(";
			obj2 = this.num2;
			array[5] = obj2;
			array[6] = "X";
			obj2 = this.num3;
			array[7] = obj2;
			array[8] = "=";
			text = string.Concat(array);
		}
		else if (num11 == 1)
		{
			object[] array = new object[9];
			tMP_Text = questionText2;
			array[0] = "SOLVE MATH Q";
			object obj2 = problem;
			array[1] = obj2;
			array[2] = ": \n (";
			obj2 = num1;
			array[3] = obj2;
			array[4] = "/";
			obj2 = this.num2;
			array[5] = obj2;
			array[6] = ")+";
			obj2 = this.num3;
			array[7] = obj2;
			array[8] = "=";
			string.Concat(array);
		}
		else
		{
			goto IL_1a00;
		}
		tMP_Text.text = text;
		goto IL_1a00;
		IL_1a00:
		num1 = UnityEngine.Random.Range(1f, 9999f);
		this.num2 = UnityEngine.Random.Range(1f, 9999f);
		this.num3 = UnityEngine.Random.Range(1f, 9999f);
		int num12 = (sign = Mathf.RoundToInt(UnityEngine.Random.Range(0, 1)));
		int num13 = num12;
		string text2 = default(string);
		if (num12 == 0)
		{
			object[] array = new object[9];
			tMP_Text = questionText3;
			array[0] = "SOLVE MATH Q";
			object obj2 = problem;
			array[1] = obj2;
			array[2] = ": \n";
			obj2 = num1;
			array[3] = obj2;
			array[4] = "+(";
			obj2 = this.num2;
			array[5] = obj2;
			array[6] = "X";
			obj2 = this.num3;
			array[7] = obj2;
			array[8] = "=";
			text2 = string.Concat(array);
		}
		else if (num13 == 1)
		{
			object[] array = new object[9];
			tMP_Text = questionText3;
			array[0] = "SOLVE MATH Q";
			object obj2 = problem;
			array[1] = obj2;
			array[2] = ": \n (";
			obj2 = num1;
			array[3] = obj2;
			array[4] = "/";
			obj2 = this.num2;
			array[5] = obj2;
			array[6] = ")+";
			obj2 = this.num3;
			array[7] = obj2;
			array[8] = "=";
			string.Concat(array);
		}
		else
		{
			goto IL_1f5a;
		}
		tMP_Text.text = text2;
		goto IL_1f5a;
		IL_1f5a:
		QueueAudio(bal_equals);
		questionInProgress = true;
	}

	// Token: 0x06000068 RID: 104 RVA: 0x0024D4F0 File Offset: 0x0024BAF0
	public void OKButton()
	{
		CheckAnswer();
	}

	// Token: 0x06000069 RID: 105 RVA: 0x0024AA60 File Offset: 0x00249060
	public unsafe void CheckAnswer()
	{
		if (playerAnswer.text == "31718")
		{
			StartCoroutine(CheatText("THIS IS WHERE IT ALL BEGAN"));
			SceneManager.LoadSceneAsync("TestRoom");
		}
		else if (playerAnswer.text == "53045009")
		{
			StartCoroutine(CheatText("USE THESE TO STICK TO THE CEILING!"));
			GameControllerScript gameControllerScript = gc;
			PlayerScript player = gameControllerScript.player;
			gameControllerScript.flipped = true;
			player.height = 6f;
			gameControllerScript.player.fliparoo = 180f;
			gameControllerScript.player.flipaturn = -1f;
			CameraScript component = Camera.main.GetComponent<CameraScript>();
			component.offset = Vector3.down;
		}
		if (problem > 3)
		{
			return;
		}
		bool flag = playerAnswer.text == ((float*)(NativeIntrinsics.Reinterpret<long>(this) + 268L))->ToString();
		bool num = (!impossibleMode && flag);
		RawImage[] array = results;
		MathGameScript mathGameScript = default(MathGameScript);
		if (!num)
		{
			problemsWrong++;
			array[(int)(problem - 1L)].texture = incorrect;
			if (!gc.spoopMode)
			{
				baldiFeed.SetTrigger("angry");
				gc.ActivateSpoopMode();
			}
			bool flag2 = gc.mode == "story";
			BaldiScript baldiScript = this.baldiScript;
			if (!flag2)
			{
				baldiScript.baldiAnger += 1f;
				if (0.5f > baldiScript.baldiAnger + 1f)
				{
					baldiScript.baldiAnger = 0.5f;
				}
				_ = 2f / baldiScript.baldiSpeedScale + baldiScript.baldiAnger;
				baldiScript.baldiWait = baldiScript.baldiAnger * -3f / (2f / baldiScript.baldiSpeedScale + baldiScript.baldiAnger) + 3f;
			}
			else if (problem != 3)
			{
				baldiScript.baldiTempAnger += 0.25f;
			}
			else
			{
				baldiScript.GetAngry(1f);
			}
			AudioSource audioSource = baldiAudio;
			audioInQueue = 0;
			audioSource.Stop();
			mathGameScript = this;
		}
		else
		{
			array[(int)(problem - 1L)].texture = correct;
			baldiAudio.Stop();
			audioInQueue = 0;
			int num2 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 4f));
			audioQueue[(int)audioInQueue] = bal_praises[num2];
			audioInQueue++;
			mathGameScript = this;
		}
		mathGameScript.NewProblem();
	}

	// Token: 0x0600006A RID: 106 RVA: 0x0024D5C0 File Offset: 0x0024BBC0
	private void QueueAudio(AudioClip sound)
	{
		audioQueue[audioInQueue] = sound;
		audioInQueue++;
	}

	// Token: 0x0600006B RID: 107 RVA: 0x0024D500 File Offset: 0x0024BB00
	private void PlayQueue()
	{
		baldiAudio.PlayOneShot(audioQueue[0]);
		MathGameScript mathGameScript = this;
		if (audioInQueue > 1)
		{
			mathGameScript = this;
			int num = 1;
			do
			{
				AudioClip audioClip = mathGameScript.audioQueue[num];
				mathGameScript.audioQueue[(int)(num - 1L)] = audioClip;
				num++;
			}
			while (num + 1 < mathGameScript.audioInQueue);
		}
		mathGameScript.audioInQueue--;
	}

	// Token: 0x0600006C RID: 108 RVA: 0x0024D7D0 File Offset: 0x0024BDD0
	private void UnqueueAudio()
	{
		MathGameScript mathGameScript = this;
		if (audioInQueue > 1)
		{
			mathGameScript = this;
			int num = 1;
			do
			{
				AudioClip[] array = mathGameScript.audioQueue;
				array[(int)(num - 1L)] = array[num];
				num++;
			}
			while (num + 1 < mathGameScript.audioInQueue);
		}
		mathGameScript.audioInQueue--;
	}

	// Token: 0x0600006D RID: 109 RVA: 0x0024AEE0 File Offset: 0x002494E0
	private void ClearAudioQueue()
	{
		audioInQueue = 0;
	}

	// Token: 0x0600006E RID: 110 RVA: 0x0024AEF0 File Offset: 0x002494F0
	private void ExitGame()
	{
		int num = problemsWrong;
		bool flag = gc.mode == "endless";
		if (num <= 0 && flag)
		{
			BaldiScript baldiScript = this.baldiScript;
			baldiScript.baldiAnger -= 1f;
			if (0.5f > baldiScript.baldiAnger - 1f)
			{
				baldiScript.baldiAnger = 0.5f;
			}
			_ = 2f / baldiScript.baldiSpeedScale + baldiScript.baldiAnger;
			baldiScript.baldiWait = baldiScript.baldiAnger * -3f / (2f / baldiScript.baldiSpeedScale + baldiScript.baldiAnger) + 3f;
		}
		gc.DeactivateLearningGame(gameObject);
	}

	// Token: 0x0600006F RID: 111 RVA: 0x0024A8C0 File Offset: 0x00248EC0
	public void ButtonPress(int value)
	{
		TMP_InputField tMP_InputField = playerAnswer;
		if ((value >= 0 && value <= 9))
		{
			int num = default(int);
			tMP_InputField.text = tMP_InputField.text + num;
		}
		else if (value != -1)
		{
			tMP_InputField.text = string.Empty;
		}
		else
		{
			tMP_InputField.text = tMP_InputField.text + "-";
		}
	}

	// Token: 0x06000070 RID: 112 RVA: 0x0024A9D0 File Offset: 0x00248FD0
	private IEnumerator CheatText(string text)
	{
		// Il2CppStudio: coroutine reconstructed from <CheatText>d__11.MoveNext (state machine class hidden).
		while (true)
		{
			questionText.text = text;
			questionText2.text = string.Empty;
			questionText3.text = string.Empty;
			yield return new WaitForEndOfFrame();
		}
	}
}

