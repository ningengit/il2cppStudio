using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using Mono.Globalization.Unicode;
using Mono.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000015 RID: 21
public class MathGameScript : MonoBehaviour
{
	// Token: 0x02000016 RID: 22
	private sealed class <CheatText>d__11 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x040000AD RID: 173 Offset: 0x10
		private int <>1__state;

		// Token: 0x040000AE RID: 174 Offset: 0x18
		private object <>2__current;

		// Token: 0x040000AF RID: 175 Offset: 0x20
		public MathGameScript <>4__this;

		// Token: 0x040000B0 RID: 176 Offset: 0x28
		public string text;

		private object System.Collections.Generic.IEnumerator<System.Object>.Current => <>2__current;

		private object System.Collections.IEnumerator.Current => <>2__current;

		// Token: 0x06000072 RID: 114 RVA: 0x00250B50 File Offset: 0x0024F150
		public <CheatText>d__11(int <>1__state)
		{
			((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
			this.<>1__state = <>1__state;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00250B40 File Offset: 0x0024F140
		private void System.IDisposable.Dispose()
		{
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00250B80 File Offset: 0x0024F180
		private bool MoveNext()
		{
			if ((uint)<>1__state <= 1u)
			{
				MathGameScript mathGameScript = <>4__this;
				<>1__state = -1;
				WaitForEndOfFrame waitForEndOfFrame = (WaitForEndOfFrame)(object)new Mono.Globalization.Unicode.ContractionComparer();
				<>2__current = waitForEndOfFrame;
				<>1__state = 1;
				return true;
			}
			return false;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00250CA0 File Offset: 0x0024F2A0
		private void System.Collections.IEnumerator.Reset()
		{
			long num = NativeIntrinsics.CallNative("sub_184BE0", NativeIntrinsics.Asm("rax"), 0L, 0L, 0L);
			NativeIntrinsics.Reinterpret<NotSupportedException>(num)..ctor();
			throw null;
		}
	}

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
		Cursor.lockState = NativeIntrinsics.Reinterpret<CursorLockMode>(0);
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
			long num = audioInQueue;
			AudioClip[] array = audioQueue;
			AudioClip audioClip = bal_intro;
			array[(int)num] = audioClip;
			long num2 = audioInQueue + 1L;
			long num3 = audioInQueue;
			AudioClip[] array2 = audioQueue;
			audioInQueue = (int)num2;
			num = num2;
			AudioClip audioClip2 = bal_howto;
			NativeIntrinsics.Stmt("mov [rcx+rax*8+28h],rdx ; value = LocalExpr");
			audioInQueue++;
		}
		NewProblem();
		GameControllerScript gameControllerScript2 = gc;
		if (gameControllerScript2.spoopMode)
		{
			Transform obj = baldiFeedTransform;
			Vector3 position = default(Vector3);
			position.x = -1000f;
			position.y = -1000f;
			position.z = 0f;
			obj.position = position;
		}
	}

	// Token: 0x06000066 RID: 102 RVA: 0x0024D860 File Offset: 0x0024BE60
	private void Update()
	{
		Animator animator;
		if (baldiAudio.isPlaying)
		{
			animator = baldiFeed;
			int num = 1;
		}
		else
		{
			if ((audioInQueue > 0) & !gc.spoopMode)
			{
				baldiAudio.PlayOneShot(audioQueue[0]);
				if (audioInQueue > 1)
				{
					do
					{
						AudioClip audioClip = audioQueue[1];
						AudioClip[] array = audioQueue;
						array[0] = audioClip;
						int num2 = 1 + 1;
					}
					while (1 + 1 < audioInQueue);
				}
				audioInQueue--;
			}
			animator = baldiFeed;
		}
		animator.SetBool("talking", value: false);
		bool keyDown = Input.GetKeyDown("return");
		bool flag = keyDown;
		int num3 = default(int);
		if (!keyDown)
		{
			bool keyDown2 = Input.GetKeyDown("enter");
			num3 = (keyDown2 ? 1 : 0);
			flag = keyDown2;
		}
		if (((questionInProgress ? 1u : 0u) & (uint)num3) != 0)
		{
			questionInProgress = false;
			CheckAnswer();
		}
		if (problem <= 3)
		{
			return;
		}
		float num4 = endDelay;
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		endDelay = num4 - unscaledDeltaTime;
		if (0f < num4 - unscaledDeltaTime)
		{
			return;
		}
		GC.Collect();
		int num5 = problemsWrong;
		bool flag2 = string.Equals(gc.mode, "endless");
		if (num5 <= 0 && flag2)
		{
			BaldiScript baldiScript = this.baldiScript;
			baldiScript.baldiAnger -= 1f;
			if (0.5f > baldiScript.baldiAnger - 1f)
			{
				baldiScript.baldiAnger = 0.5f;
			}
			float num6 = 2f / baldiScript.baldiSpeedScale + baldiScript.baldiAnger;
			float num7 = (baldiScript.baldiWait = baldiScript.baldiAnger * -3f / (2f / baldiScript.baldiSpeedScale + baldiScript.baldiAnger) + 3f);
			float num8 = num6;
			float num9 = num7;
		}
		GameControllerScript gameControllerScript = gc;
		GameObject gameObject = this.gameObject;
		GameObject subject = gameObject;
		gameControllerScript.DeactivateLearningGame(subject);
	}

	// Token: 0x06000067 RID: 103 RVA: 0x0024AFF0 File Offset: 0x002495F0
	private void NewProblem()
	{
		playerAnswer.text = NativeIntrinsics.Reinterpret<string>(NativeIntrinsics.Asm("*(/*typedef#-1*/object.static_fields + 0x0)"));
		TMP_InputField tMP_InputField = playerAnswer;
		problem++;
		tMP_InputField.ActivateInputField();
		if (problem > 3)
		{
			GameControllerScript gameControllerScript = gc;
			endDelay = 5f;
			GameControllerScript gameControllerScript2 = gameControllerScript;
			if (!gameControllerScript2.spoopMode)
			{
				return;
			}
			bool flag = string.Equals(gameControllerScript2.mode, "endless");
			int num = ((problemsWrong <= 0 && flag) ? 1 : 0);
			int num2 = (flag ? 1 : 0);
			if (num == 0)
			{
				GameControllerScript gameControllerScript3 = gc;
				bool flag2 = string.Equals(gameControllerScript3.mode, "story");
				if (problemsWrong < 3 || !flag2)
				{
					float f = UnityEngine.Random.Range(0f, 1f);
					int num3 = Mathf.RoundToInt(f);
					int num4 = num3;
					return;
				}
				bool flag3 = this.baldiScript.isActiveAndEnabled;
				bool flag4 = flag3;
				if (flag3)
				{
					BaldiScript baldiScript = this.baldiScript;
					if (!baldiScript.antiHearing && !(7f < baldiScript.currentPriority))
					{
						NavMeshAgent agent = baldiScript.agent;
						float z = playerPosition.z;
						bool flag5 = agent.SetDestination(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFF0]") + -72L));
						baldiScript.currentPriority = 7f;
						flag4 = flag5;
					}
				}
				gc.failedNotebooks++;
			}
			else
			{
				float f2 = UnityEngine.Random.Range(0f, 1f);
				int num5 = Mathf.RoundToInt(f2);
				int num6 = num5;
			}
			return;
		}
		AudioClip[] array = bal_problems;
		AudioClip audioClip = array[(int)(problem - 1L)];
		AudioClip[] array2 = audioQueue;
		long num7 = audioInQueue;
		array2[(int)num7] = audioClip;
		GameControllerScript gameControllerScript4 = gc;
		audioInQueue++;
		bool flag6 = string.Equals(gameControllerScript4.mode, "story");
		int num8 = (flag6 ? 1 : 0);
		int num9 = default(int);
		if (problem > 2)
		{
			num9 = ((gc.notebooks <= 1) ? 1 : 0);
		}
		TMP_Text tMP_Text;
		if ((num9 & num8) == 0)
		{
			GameControllerScript gameControllerScript5 = gc;
			bool flag7 = string.Equals(gameControllerScript5.mode, "endless");
			int num10 = (flag7 ? 1 : 0);
			int num11 = default(int);
			if (problem > 2)
			{
				num11 = ((gc.notebooks != 2) ? 1 : 0);
			}
			if ((num11 & num10) == 0)
			{
				impossibleMode = true;
				float num12 = UnityEngine.Random.Range(1f, 9999f);
				num1 = num12;
				float num13 = UnityEngine.Random.Range(1f, 9999f);
				this.num2 = num13;
				float num14 = UnityEngine.Random.Range(1f, 9999f);
				this.num3 = num14;
				int num15 = UnityEngine.Random.Range(0, 1);
				int num16 = Mathf.RoundToInt(num15);
				AudioClip sound = bal_screech;
				sign = num16;
				QueueAudio(sound);
				AudioClip sound2;
				if (sign != 0)
				{
					if (sign != 1)
					{
						goto IL_0a71;
					}
					object[] array3 = new object[9];
					object[] array4 = array3;
					object[] array5 = array3;
					tMP_Text = questionText;
					if ("SOLVE MATH Q" != null)
					{
						long num17 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("SOLVE MATH Q"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
						long num18 = num17;
					}
					array4[0] = "SOLVE MATH Q";
					object obj = (int)NativeIntrinsics.Asm("*(BinaryExpr)");
					object obj2 = obj;
					object value = obj;
					if (obj != null)
					{
						long num19 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
					}
					array4[1] = obj2;
					if (": \n (" != null)
					{
						long num20 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(": \n ("), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
						long num21 = num20;
					}
					array4[2] = ": \n (";
					float num22 = num1;
					object obj3 = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(BinaryExpr)"));
					obj2 = obj3;
					object value2 = obj3;
					if (obj3 != null)
					{
						long num23 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value2), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
					}
					array4[3] = obj2;
					if ("/" != null)
					{
						long num24 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("/"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
						long num25 = num24;
					}
					array4[4] = "/";
					float num26 = this.num2;
					object obj4 = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(BinaryExpr)"));
					obj2 = obj4;
					object value3 = obj4;
					if (obj4 != null)
					{
						long num27 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value3), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
					}
					array4[5] = obj2;
					if (")+" != null)
					{
						long num28 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(")+"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
						long num29 = num28;
					}
					array4[6] = ")+";
					float num30 = this.num3;
					object obj5 = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(BinaryExpr)"));
					obj2 = obj5;
					object value4 = obj5;
					if (obj5 != null)
					{
						long num31 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value4), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
					}
					array4[7] = obj2;
					if ("=" != null)
					{
						long num32 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("="), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
						long num33 = num32;
					}
					array4[8] = "=";
					string text = string.Concat(array4);
					QueueAudio(bal_divided);
					QueueAudio(bal_screech);
					sound2 = bal_plus;
				}
				else
				{
					object[] array6 = new object[9];
					object[] array4 = array6;
					object[] array7 = array6;
					tMP_Text = questionText;
					if ("SOLVE MATH Q" != null)
					{
						long num34 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("SOLVE MATH Q"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
						long num35 = num34;
					}
					array4[0] = "SOLVE MATH Q";
					object obj6 = (int)NativeIntrinsics.Asm("*(BinaryExpr)");
					object obj2 = obj6;
					object value5 = obj6;
					if (obj6 != null)
					{
						long num36 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value5), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
					}
					array4[1] = obj2;
					if (": \n" != null)
					{
						long num37 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(": \n"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
						long num38 = num37;
					}
					array4[2] = ": \n";
					float num39 = num1;
					object obj7 = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(BinaryExpr)"));
					obj2 = obj7;
					object value6 = obj7;
					if (obj7 != null)
					{
						long num40 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value6), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
					}
					array4[3] = obj2;
					if ("+(" != null)
					{
						long num41 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("+("), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
						long num42 = num41;
					}
					array4[4] = "+(";
					float num43 = this.num2;
					object obj8 = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(BinaryExpr)"));
					obj2 = obj8;
					object value7 = obj8;
					if (obj8 != null)
					{
						long num44 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value7), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
					}
					array4[5] = obj2;
					if ("X" != null)
					{
						long num45 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("X"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
						long num46 = num45;
					}
					array4[6] = "X";
					float num47 = this.num3;
					object obj9 = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(BinaryExpr)"));
					obj2 = obj9;
					object value8 = obj9;
					if (obj9 != null)
					{
						long num48 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value8), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
					}
					array4[7] = obj2;
					if ("=" != null)
					{
						long num49 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("="), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
						long num50 = num49;
					}
					array4[8] = "=";
					string text2 = string.Concat(array4);
					QueueAudio(bal_plus);
					QueueAudio(bal_screech);
					sound2 = bal_times;
				}
				QueueAudio(sound2);
				QueueAudio(bal_screech);
				goto IL_0a71;
			}
		}
		float f3 = UnityEngine.Random.Range(0f, 9f);
		int num51 = Mathf.RoundToInt(f3);
		num1 = num51;
		float f4 = UnityEngine.Random.Range(0f, 9f);
		int num52 = Mathf.RoundToInt(f4);
		this.num2 = num52;
		float f5 = UnityEngine.Random.Range(0f, 1f);
		int num53 = Mathf.RoundToInt(f5);
		AudioClip[] array8 = bal_numbers;
		float f6 = num1;
		sign = num53;
		int num54 = Mathf.RoundToInt(f6);
		AudioClip[] array9 = audioQueue;
		long num55 = audioInQueue;
		AudioClip audioClip2 = array8[num54];
		array9[(int)num55] = audioClip2;
		audioInQueue++;
		if (sign != 0)
		{
			if (sign == 1)
			{
				float num56 = num1 - this.num2;
				TMP_Text tMP_Text2 = questionText;
				solution = num56;
				object[] array10 = new object[7];
				object[] array4 = array10;
				object[] array11 = array10;
				tMP_Text = tMP_Text2;
				if ("SOLVE MATH Q" != null)
				{
					long num57 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("SOLVE MATH Q"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), num55, 0L);
					long num58 = num57;
				}
				array4[0] = "SOLVE MATH Q";
				object obj10 = (int)NativeIntrinsics.Asm("*(BinaryExpr)");
				object obj2 = obj10;
				object value9 = obj10;
				if (obj10 != null)
				{
					long num59 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value9), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), num55, 0L);
				}
				array4[1] = obj2;
				if (": \n \n" != null)
				{
					long num60 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(": \n \n"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), num55, 0L);
					long num61 = num60;
				}
				array4[2] = ": \n \n";
				float num62 = num1;
				object obj11 = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(BinaryExpr)"));
				obj2 = obj11;
				object value10 = obj11;
				if (obj11 != null)
				{
					long num63 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value10), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), num55, 0L);
				}
				array4[3] = obj2;
				if ("-" != null)
				{
					long num64 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("-"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), num55, 0L);
					long num65 = num64;
				}
				array4[4] = "-";
				float num66 = this.num2;
				object obj12 = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(BinaryExpr)"));
				obj2 = obj12;
				object value11 = obj12;
				if (obj12 != null)
				{
					long num67 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value11), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), num55, 0L);
				}
				array4[5] = obj2;
				if ("=" != null)
				{
					long num68 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("="), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), num55, 0L);
					long num69 = num68;
				}
				array4[6] = "=";
				string text3 = string.Concat(array4);
				QueueAudio(bal_minus);
			}
		}
		else
		{
			float num70 = this.num2 + num1;
			TMP_Text tMP_Text3 = questionText;
			solution = num70;
			object[] array12 = new object[7];
			object[] array4 = array12;
			object[] array13 = array12;
			tMP_Text = tMP_Text3;
			if ("SOLVE MATH Q" != null)
			{
				long num71 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("SOLVE MATH Q"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), num55, 0L);
				long num72 = num71;
			}
			array4[0] = "SOLVE MATH Q";
			object obj13 = (int)NativeIntrinsics.Asm("*(BinaryExpr)");
			object obj2 = obj13;
			object value12 = obj13;
			if (obj13 != null)
			{
				long num73 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value12), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), num55, 0L);
			}
			array4[1] = obj2;
			if (": \n \n" != null)
			{
				long num74 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(": \n \n"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), num55, 0L);
				long num75 = num74;
			}
			array4[2] = ": \n \n";
			float num76 = num1;
			object obj14 = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(BinaryExpr)"));
			obj2 = obj14;
			object value13 = obj14;
			if (obj14 != null)
			{
				long num77 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value13), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), num55, 0L);
			}
			array4[3] = obj2;
			if ("+" != null)
			{
				long num78 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("+"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), num55, 0L);
				long num79 = num78;
			}
			array4[4] = "+";
			float num80 = this.num2;
			object obj15 = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(BinaryExpr)"));
			obj2 = obj15;
			object value14 = obj15;
			if (obj15 != null)
			{
				long num81 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value14), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), num55, 0L);
			}
			array4[5] = obj2;
			if ("=" != null)
			{
				long num82 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("="), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), num55, 0L);
				long num83 = num82;
			}
			array4[6] = "=";
			string text4 = string.Concat(array4);
			array9 = audioQueue;
			num55 = audioInQueue;
			AudioClip audioClip3 = bal_plus;
			array9[(int)num55] = audioClip3;
			audioInQueue++;
		}
		AudioClip[] array14 = bal_numbers;
		int num84 = Mathf.RoundToInt(this.num2);
		int num85 = num84;
		long num86 = audioInQueue;
		AudioClip[] array15 = audioQueue;
		AudioClip audioClip4 = array14[num85];
		array15[(int)num86] = audioClip4;
		long num87 = audioInQueue;
		long num88 = audioInQueue + 1L;
		AudioClip[] array16 = audioQueue;
		audioInQueue = (int)num88;
		long num89 = num87;
		AudioClip audioClip5 = bal_equals;
		NativeIntrinsics.Stmt("mov [rcx+rax*8+28h],rdx ; value = LocalExpr");
		audioInQueue++;
		questionInProgress = true;
		return;
		IL_0a71:
		float num90 = UnityEngine.Random.Range(1f, 9999f);
		num1 = num90;
		float num91 = UnityEngine.Random.Range(1f, 9999f);
		this.num2 = num91;
		float num92 = UnityEngine.Random.Range(1f, 9999f);
		this.num3 = num92;
		int num93 = UnityEngine.Random.Range(0, 1);
		int num94 = (sign = Mathf.RoundToInt(num93));
		int num95 = num94;
		string text8 = default(string);
		if (num94 != 0)
		{
			if (num95 != 1)
			{
				goto IL_11ac;
			}
			object[] array17 = new object[9];
			object[] array4 = array17;
			object[] array18 = array17;
			tMP_Text = questionText2;
			if ("SOLVE MATH Q" != null)
			{
				long num96 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("SOLVE MATH Q"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
				long num97 = num96;
			}
			array4[0] = "SOLVE MATH Q";
			object obj16 = (int)NativeIntrinsics.Asm("*(BinaryExpr)");
			object obj2 = obj16;
			object value15 = obj16;
			if (obj16 != null)
			{
				long num98 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value15), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
			}
			array4[1] = obj2;
			if (": \n (" != null)
			{
				long num99 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(": \n ("), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
				long num100 = num99;
			}
			array4[2] = ": \n (";
			float num101 = num1;
			object obj17 = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(BinaryExpr)"));
			obj2 = obj17;
			object value16 = obj17;
			if (obj17 != null)
			{
				long num102 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value16), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
			}
			array4[3] = obj2;
			if ("/" != null)
			{
				long num103 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("/"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
				long num104 = num103;
			}
			array4[4] = "/";
			float num105 = this.num2;
			object obj18 = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(BinaryExpr)"));
			obj2 = obj18;
			object value17 = obj18;
			if (obj18 != null)
			{
				long num106 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value17), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
			}
			array4[5] = obj2;
			if (")+" != null)
			{
				long num107 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(")+"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
				long num108 = num107;
			}
			array4[6] = ")+";
			float num109 = this.num3;
			object obj19 = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(BinaryExpr)"));
			obj2 = obj19;
			object value18 = obj19;
			if (obj19 != null)
			{
				long num110 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value18), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
			}
			array4[7] = obj2;
			if ("=" != null)
			{
				long num111 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("="), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
				long num112 = num111;
			}
			array4[8] = "=";
			string text5 = string.Concat(array4);
			string text6 = text5;
		}
		else
		{
			object[] array19 = new object[9];
			object[] array4 = array19;
			object[] array20 = array19;
			tMP_Text = questionText2;
			if ("SOLVE MATH Q" != null)
			{
				long num113 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("SOLVE MATH Q"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
				long num114 = num113;
			}
			array4[0] = "SOLVE MATH Q";
			object obj20 = (int)NativeIntrinsics.Asm("*(BinaryExpr)");
			object obj2 = obj20;
			object value19 = obj20;
			if (obj20 != null)
			{
				long num115 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value19), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
			}
			array4[1] = obj2;
			if (": \n" != null)
			{
				long num116 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(": \n"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
				long num117 = num116;
			}
			array4[2] = ": \n";
			float num118 = num1;
			object obj21 = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(BinaryExpr)"));
			obj2 = obj21;
			object value20 = obj21;
			if (obj21 != null)
			{
				long num119 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value20), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
			}
			array4[3] = obj2;
			if ("+(" != null)
			{
				long num120 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("+("), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
				long num121 = num120;
			}
			array4[4] = "+(";
			float num122 = this.num2;
			object obj22 = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(BinaryExpr)"));
			obj2 = obj22;
			object value21 = obj22;
			if (obj22 != null)
			{
				long num123 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value21), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
			}
			array4[5] = obj2;
			if ("X" != null)
			{
				long num124 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("X"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
				long num125 = num124;
			}
			array4[6] = "X";
			float num126 = this.num3;
			object obj23 = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(BinaryExpr)"));
			obj2 = obj23;
			object value22 = obj23;
			if (obj23 != null)
			{
				long num127 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value22), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
			}
			array4[7] = obj2;
			if ("=" != null)
			{
				long num128 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("="), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
				long num129 = num128;
			}
			array4[8] = "=";
			string text7 = string.Concat(array4);
			text8 = text7;
		}
		TMP_Text tMP_Text4 = tMP_Text;
		string text9 = text8;
		goto IL_11ac;
		IL_11ac:
		float num130 = UnityEngine.Random.Range(1f, 9999f);
		num1 = num130;
		float num131 = UnityEngine.Random.Range(1f, 9999f);
		this.num2 = num131;
		float num132 = UnityEngine.Random.Range(1f, 9999f);
		this.num3 = num132;
		int num133 = UnityEngine.Random.Range(0, 1);
		int num134 = (sign = Mathf.RoundToInt(num133));
		int num135 = num134;
		if (num134 != 0)
		{
			if (num135 == 1)
			{
				object[] array21 = new object[9];
				object[] array4 = array21;
				object[] array22 = array21;
				tMP_Text = questionText3;
				if ("SOLVE MATH Q" != null)
				{
					long num136 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("SOLVE MATH Q"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
					long num137 = num136;
				}
				array4[0] = "SOLVE MATH Q";
				object obj24 = (int)NativeIntrinsics.Asm("*(BinaryExpr)");
				object obj2 = obj24;
				object value23 = obj24;
				if (obj24 != null)
				{
					long num138 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value23), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
				}
				array4[1] = obj2;
				if (": \n (" != null)
				{
					long num139 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(": \n ("), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
					long num140 = num139;
				}
				array4[2] = ": \n (";
				float num141 = num1;
				object obj25 = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(BinaryExpr)"));
				obj2 = obj25;
				object value24 = obj25;
				if (obj25 != null)
				{
					long num142 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value24), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
				}
				array4[3] = obj2;
				if ("/" != null)
				{
					long num143 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("/"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
					long num144 = num143;
				}
				array4[4] = "/";
				float num145 = this.num2;
				object obj26 = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(BinaryExpr)"));
				obj2 = obj26;
				object value25 = obj26;
				if (obj26 != null)
				{
					long num146 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value25), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
				}
				array4[5] = obj2;
				if (")+" != null)
				{
					long num147 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(")+"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
					long num148 = num147;
				}
				array4[6] = ")+";
				float num149 = this.num3;
				object obj27 = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(BinaryExpr)"));
				obj2 = obj27;
				object value26 = obj27;
				if (obj27 != null)
				{
					long num150 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value26), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
				}
				array4[7] = obj2;
				if ("=" != null)
				{
					long num151 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("="), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
					long num152 = num151;
				}
				array4[8] = "=";
				string text10 = string.Concat(array4);
				string text11 = text10;
			}
		}
		else
		{
			object[] array23 = new object[9];
			object[] array4 = array23;
			object[] array24 = array23;
			tMP_Text = questionText3;
			if ("SOLVE MATH Q" != null)
			{
				long num153 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("SOLVE MATH Q"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
				long num154 = num153;
			}
			array4[0] = "SOLVE MATH Q";
			object obj28 = (int)NativeIntrinsics.Asm("*(BinaryExpr)");
			object obj2 = obj28;
			object value27 = obj28;
			if (obj28 != null)
			{
				long num155 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value27), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
			}
			array4[1] = obj2;
			if (": \n" != null)
			{
				long num156 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(": \n"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
				long num157 = num156;
			}
			array4[2] = ": \n";
			float num158 = num1;
			object obj29 = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(BinaryExpr)"));
			obj2 = obj29;
			object value28 = obj29;
			if (obj29 != null)
			{
				long num159 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value28), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
			}
			array4[3] = obj2;
			if ("+(" != null)
			{
				long num160 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("+("), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
				long num161 = num160;
			}
			array4[4] = "+(";
			float num162 = this.num2;
			object obj30 = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(BinaryExpr)"));
			obj2 = obj30;
			object value29 = obj30;
			if (obj30 != null)
			{
				long num163 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value29), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
			}
			array4[5] = obj2;
			if ("X" != null)
			{
				long num164 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("X"), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
				long num165 = num164;
			}
			array4[6] = "X";
			float num166 = this.num3;
			object obj31 = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("*(BinaryExpr)"));
			obj2 = obj31;
			object value30 = obj31;
			if (obj31 != null)
			{
				long num167 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>(value30), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
			}
			array4[7] = obj2;
			if ("=" != null)
			{
				long num168 = NativeIntrinsics.CallNative("sub_1A4130", NativeIntrinsics.Reinterpret<long>("="), NativeIntrinsics.Asm("*(LocalExpr->klass + 0x40)"), 0L, 0L);
				long num169 = num168;
			}
			array4[8] = "=";
			string text12 = string.Concat(array4);
			string text13 = text12;
		}
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
		if (!string.Equals(playerAnswer.m_Text, "31718"))
		{
			bool flag = string.Equals(playerAnswer.m_Text, "53045009");
			bool flag2 = flag;
			if (flag)
			{
				IEnumerator routine = CheatText("USE THESE TO STICK TO THE CEILING!");
				Coroutine coroutine = StartCoroutine(routine);
				GameControllerScript gameControllerScript = gc;
				if (NativeIntrinsics.Asm("global_19E3B11") == 0)
				{
					NativeIntrinsics.Stmt("mov byte ptr [1819E3B11h],1 ; value = 0x1");
				}
				PlayerScript player = gameControllerScript.player;
				gameControllerScript.flipped = true;
				PlayerScript playerScript = player;
				playerScript.height = 6f;
				playerScript = gameControllerScript.player;
				playerScript.fliparoo = 180f;
				playerScript = gameControllerScript.player;
				playerScript.flipaturn = -1f;
				Camera main = Camera.main;
				Camera camera = main;
				CameraScript component = camera.GetComponent<CameraScript>();
				CameraScript cameraScript = component;
				cameraScript.offset = NativeIntrinsics.Reinterpret<Vector3>(0);
				cameraScript.offset.y = -1f;
				cameraScript.offset.z = 0f;
			}
		}
		else
		{
			IEnumerator routine2 = CheatText("THIS IS WHERE IT ALL BEGAN");
			Coroutine coroutine2 = StartCoroutine(routine2);
			AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("TestRoom");
			AsyncOperation asyncOperation2 = asyncOperation;
		}
		if (problem > 3)
		{
			return;
		}
		string text = playerAnswer.m_Text;
		string b = ((float*)(NativeIntrinsics.Reinterpret<long>(this) + 268L))->ToString();
		bool flag3 = string.Equals(text, b);
		int num = ((!impossibleMode && flag3) ? 1 : 0);
		RawImage[] array = results;
		if (num == 0)
		{
			problemsWrong++;
			array[(int)(problem - 1L)].texture = incorrect;
			if (!gc.spoopMode)
			{
				baldiFeed.SetTrigger("angry");
				gc.ActivateSpoopMode();
			}
			GameControllerScript gameControllerScript2 = gc;
			bool flag4 = string.Equals(gameControllerScript2.mode, "story");
			BaldiScript baldiScript = this.baldiScript;
			if (!flag4)
			{
				baldiScript.baldiAnger += 1f;
				if (0.5f > baldiScript.baldiAnger + 1f)
				{
					baldiScript.baldiAnger = 0.5f;
				}
				float num2 = 2f / baldiScript.baldiSpeedScale + baldiScript.baldiAnger;
				float num3 = (baldiScript.baldiWait = baldiScript.baldiAnger * -3f / (2f / baldiScript.baldiSpeedScale + baldiScript.baldiAnger) + 3f);
				float num4 = num2;
				float num5 = num3;
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
		}
		else
		{
			array[(int)(problem - 1L)].texture = correct;
			baldiAudio.Stop();
			audioInQueue = 0;
			float f = UnityEngine.Random.Range(0f, 4f);
			int num6 = Mathf.RoundToInt(f);
			long num7 = audioInQueue;
			AudioClip[] array2 = audioQueue;
			AudioClip audioClip = bal_praises[num6];
			array2[(int)num7] = audioClip;
			audioInQueue++;
		}
		NewProblem();
	}

	// Token: 0x0600006A RID: 106 RVA: 0x0024D5C0 File Offset: 0x0024BBC0
	private void QueueAudio(AudioClip sound)
	{
		AudioClip[] array = audioQueue;
		long num = audioInQueue;
		array[(int)num] = sound;
		audioInQueue++;
	}

	// Token: 0x0600006B RID: 107 RVA: 0x0024D500 File Offset: 0x0024BB00
	private void PlayQueue()
	{
		baldiAudio.PlayOneShot(audioQueue[0]);
		if (audioInQueue > 1)
		{
			do
			{
				AudioClip audioClip = audioQueue[1];
				AudioClip[] array = audioQueue;
				array[0] = audioClip;
				int num = 1 + 1;
			}
			while (1 + 1 < audioInQueue);
		}
		audioInQueue--;
	}

	// Token: 0x0600006C RID: 108 RVA: 0x0024D7D0 File Offset: 0x0024BDD0
	private void UnqueueAudio()
	{
		if (audioInQueue > 1)
		{
			do
			{
				AudioClip[] array = audioQueue;
				AudioClip audioClip = array[1];
				array[0] = audioClip;
				int num = 1 + 1;
			}
			while (1 + 1 < audioInQueue);
		}
		audioInQueue--;
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
		bool flag = string.Equals(gc.mode, "endless");
		if (num <= 0 && flag)
		{
			BaldiScript baldiScript = this.baldiScript;
			baldiScript.baldiAnger -= 1f;
			if (0.5f > baldiScript.baldiAnger - 1f)
			{
				baldiScript.baldiAnger = 0.5f;
			}
			float num2 = 2f / baldiScript.baldiSpeedScale + baldiScript.baldiAnger;
			float num3 = (baldiScript.baldiWait = baldiScript.baldiAnger * -3f / (2f / baldiScript.baldiSpeedScale + baldiScript.baldiAnger) + 3f);
			float num4 = num2;
			float num5 = num3;
		}
		GameControllerScript gameControllerScript = gc;
		GameObject subject = gameObject;
		gameControllerScript.DeactivateLearningGame(subject);
	}

	// Token: 0x0600006F RID: 111 RVA: 0x0024A8C0 File Offset: 0x00248EC0
	public void ButtonPress(int value)
	{
		TMP_InputField tMP_InputField = playerAnswer;
		if (!(value >= 0 && value <= 9))
		{
			if (value != -1)
			{
				tMP_InputField.text = NativeIntrinsics.Reinterpret<string>(NativeIntrinsics.Asm("*(/*typedef#-1*/object.static_fields + 0x0)"));
				return;
			}
			string text = tMP_InputField.m_Text + "-";
			tMP_InputField.text = text;
		}
		else
		{
			string text2 = tMP_InputField.m_Text;
			int num = default(int);
			string text3 = num.ToString();
			string text4 = text2 + text3;
			tMP_InputField.text = text4;
		}
	}

	// Token: 0x06000070 RID: 112 RVA: 0x0024A9D0 File Offset: 0x00248FD0
	private IEnumerator CheatText(string text)
	{
		<CheatText>d__11 <CheatText>d__ = NativeIntrinsics.Reinterpret<<CheatText>d__11>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(MathGameScript+<CheatText>d__11)"));
		((Mono.Xml.SecurityParser)(object)<CheatText>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<CheatText>d__.<>1__state = 0;
		<CheatText>d__.<>4__this = this;
		<CheatText>d__.text = text;
		return <CheatText>d__;
	}
}

