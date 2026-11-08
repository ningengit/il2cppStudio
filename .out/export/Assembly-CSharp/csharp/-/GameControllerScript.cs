using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using Mono.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x02000008 RID: 8
public class GameControllerScript : MonoBehaviour
{
	// Token: 0x02000009 RID: 9
	private sealed class <BootAnimation>d__19 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x0400005F RID: 95 Offset: 0x10
		private int <>1__state;

		// Token: 0x04000060 RID: 96 Offset: 0x18
		private object <>2__current;

		// Token: 0x04000061 RID: 97 Offset: 0x20
		public GameControllerScript <>4__this;

		// Token: 0x04000062 RID: 98 Offset: 0x28
		private float <time>5__2;

		// Token: 0x04000063 RID: 99 Offset: 0x2C
		private float <height>5__3;

		private object System.Collections.Generic.IEnumerator<System.Object>.Current => <>2__current;

		private object System.Collections.IEnumerator.Current => <>2__current;

		// Token: 0x06000039 RID: 57 RVA: 0x00250B50 File Offset: 0x0024F150
		public <BootAnimation>d__19(int <>1__state)
		{
			((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
			this.<>1__state = <>1__state;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00250B40 File Offset: 0x0024F140
		private void System.IDisposable.Dispose()
		{
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00250760 File Offset: 0x0024ED60
		private bool MoveNext()
		{
			GameControllerScript gameControllerScript = <>4__this;
			int num = <>1__state;
			if (<>1__state != 0)
			{
				num--;
				if (num - 1 != 0)
				{
					num--;
					if (num - 1 != 0)
					{
						if (num == 1)
						{
							<>1__state = -1;
							goto IL_01f4;
						}
						goto IL_013a;
					}
					<>1__state = -1;
					goto IL_019d;
				}
				<>1__state = -1;
			}
			else
			{
				<>1__state = -1;
				<time>5__2 = 15f;
				<height>5__3 = 375f;
				GameObject gameObject = gameControllerScript.boots.gameObject;
				gameObject.SetActive(value: true);
			}
			float num2 = <height>5__3;
			if (<height>5__3 > -375f)
			{
				float deltaTime = Time.deltaTime;
				<height>5__3 = num2 - deltaTime * 375f;
				float num3 = <time>5__2;
				float deltaTime2 = Time.deltaTime;
				<time>5__2 = num3 - deltaTime2;
				num2 = deltaTime2;
				Vector3 localPosition = gameControllerScript.boots.localPosition;
				RectTransform boots = gameControllerScript.boots;
				Vector3 localPosition2 = default(Vector3);
				localPosition2.x = localPosition.x;
				localPosition2.y = <height>5__3;
				localPosition2.z = localPosition.z;
				boots.localPosition = localPosition2;
				<>2__current = null;
				<>1__state = 1;
				goto IL_013a;
			}
			RectTransform boots2 = gameControllerScript.boots;
			Vector3 localPosition3 = boots2.localPosition;
			float x = localPosition3.x;
			RectTransform boots3 = gameControllerScript.boots;
			Vector3 localPosition4 = default(Vector3);
			localPosition4.x = x;
			localPosition4.y = -375f;
			localPosition4.z = localPosition3.z;
			boots3.localPosition = localPosition4;
			GameObject gameObject2 = gameControllerScript.boots.gameObject;
			gameObject2.SetActive(value: false);
			goto IL_019d;
			IL_019d:
			float num4 = <time>5__2;
			if (<time>5__2 > 0f)
			{
				float deltaTime3 = Time.deltaTime;
				<>2__current = null;
				<time>5__2 = num4 - deltaTime3;
				<>1__state = 2;
				goto IL_013a;
			}
			GameObject gameObject3 = gameControllerScript.boots.gameObject;
			gameObject3.SetActive(value: true);
			goto IL_01f4;
			IL_01f4:
			if (375f > <height>5__3)
			{
				float num5 = <height>5__3;
				float deltaTime4 = Time.deltaTime;
				<height>5__3 = deltaTime4 * 375f + num5;
				num2 = deltaTime4 * 375f + num5;
				Vector3 localPosition5 = gameControllerScript.boots.localPosition;
				RectTransform boots4 = gameControllerScript.boots;
				Vector3 localPosition6 = default(Vector3);
				localPosition6.x = localPosition5.x;
				localPosition6.y = <height>5__3;
				localPosition6.z = localPosition5.z;
				boots4.localPosition = localPosition6;
				<>2__current = null;
				<>1__state = 3;
			}
			else
			{
				Vector3 localPosition7 = gameControllerScript.boots.localPosition;
				RectTransform boots5 = gameControllerScript.boots;
				Vector3 localPosition8 = default(Vector3);
				localPosition8.x = localPosition7.x;
				localPosition8.y = 375f;
				localPosition8.z = localPosition7.z;
				boots5.localPosition = localPosition8;
				GameObject gameObject4 = gameControllerScript.boots.gameObject;
				gameObject4.SetActive(value: false);
			}
			goto IL_013a;
			IL_013a:
			return true;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00250B00 File Offset: 0x0024F100
		private void System.Collections.IEnumerator.Reset()
		{
			long num = NativeIntrinsics.CallNative("sub_184BE0", NativeIntrinsics.Asm("rax"), 0L, 0L, 0L);
			NativeIntrinsics.Reinterpret<NotSupportedException>(num)..ctor();
			throw null;
		}
	}

	// Token: 0x0200000A RID: 10
	private sealed class <Teleporter>d__20 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x04000064 RID: 100 Offset: 0x10
		private int <>1__state;

		// Token: 0x04000065 RID: 101 Offset: 0x18
		private object <>2__current;

		// Token: 0x04000066 RID: 102 Offset: 0x20
		public GameControllerScript <>4__this;

		// Token: 0x04000067 RID: 103 Offset: 0x28
		private int <teleports>5__2;

		// Token: 0x04000068 RID: 104 Offset: 0x2C
		private int <teleportCount>5__3;

		// Token: 0x04000069 RID: 105 Offset: 0x30
		private float <baseTime>5__4;

		// Token: 0x0400006A RID: 106 Offset: 0x34
		private float <currentTime>5__5;

		// Token: 0x0400006B RID: 107 Offset: 0x38
		private float <increaseFactor>5__6;

		private object System.Collections.Generic.IEnumerator<System.Object>.Current => <>2__current;

		private object System.Collections.IEnumerator.Current => <>2__current;

		// Token: 0x0600003F RID: 63 RVA: 0x00250B50 File Offset: 0x0024F150
		public <Teleporter>d__20(int <>1__state)
		{
			((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
			this.<>1__state = <>1__state;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00250B40 File Offset: 0x0024F140
		private void System.IDisposable.Dispose()
		{
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00251550 File Offset: 0x0024FB50
		private bool MoveNext()
		{
			int num = <>1__state;
			GameControllerScript gameControllerScript = <>4__this;
			if (<>1__state == 0)
			{
				<>1__state = -1;
				gameControllerScript.playerCharacter.enabled = false;
				gameControllerScript.playerCollider.enabled = false;
				int num2 = UnityEngine.Random.Range(12, 16);
				<teleports>5__2 = num2;
				<teleportCount>5__3 = 0;
				<baseTime>5__4 = 0.2f;
				<currentTime>5__5 = 0.2f;
				<increaseFactor>5__6 = 1.1f;
			}
			else
			{
				if (num != 1)
				{
					goto IL_0213;
				}
				<>1__state = -1;
			}
			if (<teleportCount>5__3 < <teleports>5__2)
			{
				float num3 = <currentTime>5__5;
				float deltaTime = Time.deltaTime;
				<currentTime>5__5 = num3 - deltaTime;
				float num4 = deltaTime;
				if (0f > num3 - deltaTime)
				{
					gameControllerScript.AILocationSelector.GetNewTarget();
					Transform transform = gameControllerScript.player.transform;
					Transform transform2 = gameControllerScript.AILocationSelector.transform;
					Vector3 position = transform2.position;
					float x = gameControllerScript.player.height * Vector3.upVector.x + position.x;
					float y = gameControllerScript.player.height * Vector3.upVector.y + position.y;
					Vector3 position2 = default(Vector3);
					position2.x = x;
					position2.y = y;
					position2.z = gameControllerScript.player.height * Vector3.upVector.z + position.z;
					transform.position = position2;
					gameControllerScript.audioDevice.PlayOneShot(gameControllerScript.aud_Teleport);
					float num5 = <increaseFactor>5__6 * <baseTime>5__4;
					<teleportCount>5__3++;
					<baseTime>5__4 = num5;
					<currentTime>5__5 = num5;
				}
				PlayerScript player = gameControllerScript.player;
				if (!gameControllerScript.flipped)
				{
				}
				player.height = 6f;
				<>2__current = null;
				<>1__state = 1;
			}
			else
			{
				gameControllerScript.playerCharacter.enabled = true;
				gameControllerScript.playerCollider.enabled = true;
			}
			goto IL_0213;
			IL_0213:
			return false;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00251800 File Offset: 0x0024FE00
		private void System.Collections.IEnumerator.Reset()
		{
			long num = NativeIntrinsics.CallNative("sub_184BE0", NativeIntrinsics.Asm("rax"), 0L, 0L, 0L);
			NativeIntrinsics.Reinterpret<NotSupportedException>(num)..ctor();
			throw null;
		}
	}

	// Token: 0x04000015 RID: 21 Offset: 0x20
	public CursorControllerScript cursorController;

	// Token: 0x04000016 RID: 22 Offset: 0x28
	public PlayerScript player;

	// Token: 0x04000017 RID: 23 Offset: 0x30
	public Transform playerTransform;

	// Token: 0x04000018 RID: 24 Offset: 0x38
	public CharacterController playerCharacter;

	// Token: 0x04000019 RID: 25 Offset: 0x40
	public Collider playerCollider;

	// Token: 0x0400001A RID: 26 Offset: 0x48
	public AILocationSelectorScript AILocationSelector;

	// Token: 0x0400001B RID: 27 Offset: 0x50
	public Transform cameraTransform;

	// Token: 0x0400001C RID: 28 Offset: 0x58
	public Camera playerCamera;

	// Token: 0x0400001D RID: 29 Offset: 0x60
	private int cullingMask;

	// Token: 0x0400001E RID: 30 Offset: 0x68
	public EntranceScript entrance_0;

	// Token: 0x0400001F RID: 31 Offset: 0x70
	public EntranceScript entrance_1;

	// Token: 0x04000020 RID: 32 Offset: 0x78
	public EntranceScript entrance_2;

	// Token: 0x04000021 RID: 33 Offset: 0x80
	public EntranceScript entrance_3;

	// Token: 0x04000022 RID: 34 Offset: 0x88
	public GameObject baldiTutor;

	// Token: 0x04000023 RID: 35 Offset: 0x90
	public GameObject baldi;

	// Token: 0x04000024 RID: 36 Offset: 0x98
	public BaldiScript baldiScrpt;

	// Token: 0x04000025 RID: 37 Offset: 0xA0
	public AudioClip aud_Prize;

	// Token: 0x04000026 RID: 38 Offset: 0xA8
	public AudioClip aud_PrizeMobile;

	// Token: 0x04000027 RID: 39 Offset: 0xB0
	public AudioClip aud_AllNotebooks;

	// Token: 0x04000028 RID: 40 Offset: 0xB8
	public AudioClip aud_Teleport;

	// Token: 0x04000029 RID: 41 Offset: 0xC0
	private bool flipped;

	// Token: 0x0400002A RID: 42 Offset: 0xC8
	public GameObject principal;

	// Token: 0x0400002B RID: 43 Offset: 0xD0
	public GameObject crafters;

	// Token: 0x0400002C RID: 44 Offset: 0xD8
	public GameObject playtime;

	// Token: 0x0400002D RID: 45 Offset: 0xE0
	public PlaytimeScript playtimeScript;

	// Token: 0x0400002E RID: 46 Offset: 0xE8
	public GameObject gottaSweep;

	// Token: 0x0400002F RID: 47 Offset: 0xF0
	public GameObject bully;

	// Token: 0x04000030 RID: 48 Offset: 0xF8
	public GameObject firstPrize;

	// Token: 0x04000031 RID: 49 Offset: 0x100
	public GameObject TestEnemy;

	// Token: 0x04000032 RID: 50 Offset: 0x108
	public FirstPrizeScript firstPrizeScript;

	// Token: 0x04000033 RID: 51 Offset: 0x110
	public GameObject quarter;

	// Token: 0x04000034 RID: 52 Offset: 0x118
	public AudioSource tutorBaldi;

	// Token: 0x04000035 RID: 53 Offset: 0x120
	public RectTransform boots;

	// Token: 0x04000036 RID: 54 Offset: 0x128
	public string mode;

	// Token: 0x04000037 RID: 55 Offset: 0x130
	public int notebooks;

	// Token: 0x04000038 RID: 56 Offset: 0x134
	public int failedNotebooks;

	// Token: 0x04000039 RID: 57 Offset: 0x138
	public bool spoopMode;

	// Token: 0x0400003A RID: 58 Offset: 0x139
	public bool finaleMode;

	// Token: 0x0400003B RID: 59 Offset: 0x13A
	public bool debugMode;

	// Token: 0x0400003C RID: 60 Offset: 0x13B
	public bool mouseLocked;

	// Token: 0x0400003D RID: 61 Offset: 0x13C
	public int exitsReached;

	// Token: 0x0400003E RID: 62 Offset: 0x140
	public int itemSelected;

	// Token: 0x0400003F RID: 63 Offset: 0x148
	public int[] item = new int[3];

	// Token: 0x04000040 RID: 64 Offset: 0x150
	public RawImage[] itemSlot = new RawImage[3];

	// Token: 0x04000041 RID: 65 Offset: 0x158
	private string[] itemNames = new string[12]
	{
		"Nothing", "Energy flavored Zesty Bar", "Yellow Door Lock", "Principal's Keys", "BSODA", "Quarter", "Baldi Anti Hearing and Disorienting Tape", "Alarm Clock", "WD-NoSquee (Door Type)", "Safety Scissors",
		"Big Ol' Boots", "Teleportation Teleporter"
	};

	// Token: 0x04000042 RID: 66 Offset: 0x160
	public TMP_Text itemText;

	// Token: 0x04000043 RID: 67 Offset: 0x168
	public Texture[] itemTextures = new Texture[10];

	// Token: 0x04000044 RID: 68 Offset: 0x170
	public GameObject bsodaSpray;

	// Token: 0x04000045 RID: 69 Offset: 0x178
	public GameObject alarmClock;

	// Token: 0x04000046 RID: 70 Offset: 0x180
	public TMP_Text notebookCount;

	// Token: 0x04000047 RID: 71 Offset: 0x188
	public GameObject pauseMenu;

	// Token: 0x04000048 RID: 72 Offset: 0x190
	public string ExitGameScene;

	// Token: 0x04000049 RID: 73 Offset: 0x198
	public GameObject highScoreText;

	// Token: 0x0400004A RID: 74 Offset: 0x1A0
	public GameObject warning;

	// Token: 0x0400004B RID: 75 Offset: 0x1A8
	public GameObject reticle;

	// Token: 0x0400004C RID: 76 Offset: 0x1B0
	public RectTransform itemSelect;

	// Token: 0x0400004D RID: 77 Offset: 0x1B8
	public int[] itemSelectOffset;

	// Token: 0x0400004E RID: 78 Offset: 0x1C0
	public int itemSelectPosition;

	// Token: 0x0400004F RID: 79 Offset: 0x1C4
	private bool gamePaused;

	// Token: 0x04000050 RID: 80 Offset: 0x1C5
	private bool learningActive;

	// Token: 0x04000051 RID: 81 Offset: 0x1C8
	private float gameOverDelay;

	// Token: 0x04000052 RID: 82 Offset: 0x1D0
	public string GameOverScene;

	// Token: 0x04000053 RID: 83 Offset: 0x1D8
	private AudioSource audioDevice;

	// Token: 0x04000054 RID: 84 Offset: 0x1E0
	public AudioClip aud_Soda;

	// Token: 0x04000055 RID: 85 Offset: 0x1E8
	public AudioClip aud_Spray;

	// Token: 0x04000056 RID: 86 Offset: 0x1F0
	public AudioClip aud_buzz;

	// Token: 0x04000057 RID: 87 Offset: 0x1F8
	public AudioClip aud_Hang;

	// Token: 0x04000058 RID: 88 Offset: 0x200
	public AudioClip aud_MachineQuiet;

	// Token: 0x04000059 RID: 89 Offset: 0x208
	public AudioClip aud_MachineStart;

	// Token: 0x0400005A RID: 90 Offset: 0x210
	public AudioClip aud_MachineRev;

	// Token: 0x0400005B RID: 91 Offset: 0x218
	public AudioClip aud_MachineLoop;

	// Token: 0x0400005C RID: 92 Offset: 0x220
	public AudioClip aud_Switch;

	// Token: 0x0400005D RID: 93 Offset: 0x228
	public AudioSource schoolMusic;

	// Token: 0x0400005E RID: 94 Offset: 0x230
	public AudioSource learnMusic;

	// Token: 0x0600001D RID: 29 RVA: 0x002469B0 File Offset: 0x00244FB0
	private void Start()
	{
		int num = playerCamera.cullingMask;
		cullingMask = num;
		AudioSource component = GetComponent<AudioSource>();
		audioDevice = component;
		string text = PlayerPrefs.GetString("CurrentMode");
		mode = text;
		if (string.Equals(mode, "endless"))
		{
			baldiScrpt.endless = true;
		}
		schoolMusic.Play();
		if (!learningActive)
		{
			Cursor.lockState = NativeIntrinsics.Reinterpret<CursorLockMode>(1);
			Cursor.visible = false;
			GameObject gameObject = reticle;
			mouseLocked = true;
			gameObject.SetActive(value: true);
		}
		UpdateNotebookCount();
		itemSelected = 0;
		gameOverDelay = 0.5f;
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00247010 File Offset: 0x00245610
	private void Update()
	{
		float num;
		if (learningActive)
		{
			float timeScale = Time.timeScale;
			num = timeScale;
			if (timeScale != 0f)
			{
				Time.timeScale = 0f;
			}
			goto IL_03a3;
		}
		InputManager instance = Singleton<InputManager>.Instance;
		InputManager inputManager = instance;
		if (inputManager.currentKeyStates[12] && !inputManager.previousKeyStates[12] && !player.gameOver)
		{
			if (gamePaused)
			{
				UnpauseGame();
			}
			else if (!learningActive)
			{
				Cursor.lockState = NativeIntrinsics.Reinterpret<CursorLockMode>(0);
				Cursor.visible = true;
				GameObject gameObject = reticle;
				mouseLocked = false;
				gameObject.SetActive(value: false);
				Time.timeScale = 0f;
				GameObject gameObject2 = pauseMenu;
				gamePaused = true;
				gameObject2.SetActive(value: true);
			}
		}
		bool keyDownInt = Input.GetKeyDownInt(NativeIntrinsics.Reinterpret<KeyCode>(121));
		if (!(gamePaused && keyDownInt))
		{
			bool keyDownInt2 = Input.GetKeyDownInt(NativeIntrinsics.Reinterpret<KeyCode>(110));
			if (gamePaused && keyDownInt2)
			{
				UnpauseGame();
			}
		}
		else
		{
			Time.timeScale = 1f;
			string exitGameScene = ExitGameScene;
			SceneManager.LoadScene(exitGameScene);
		}
		int num2 = (gamePaused ? 1 : 0);
		float timeScale2 = Time.timeScale;
		int num3 = num2;
		num = timeScale2;
		if (timeScale2 != 1f && num3 == 0)
		{
			Time.timeScale = 1f;
		}
		InputManager instance2 = Singleton<InputManager>.Instance;
		inputManager = instance2;
		if (inputManager.currentKeyStates[5] && !inputManager.previousKeyStates[5])
		{
			float timeScale3 = Time.timeScale;
			num = timeScale3;
			if (timeScale3 != 0f)
			{
				UseItem();
			}
		}
		float axis = Input.GetAxis("Mouse ScrollWheel");
		num = axis;
		if (axis > 0f)
		{
			float timeScale4 = Time.timeScale;
			num = timeScale4;
			if (timeScale4 != 0f)
			{
				itemSelected--;
				if (itemSelected - 1 < 0)
				{
					itemSelected = 2;
				}
				goto IL_02a5;
			}
		}
		float axis2 = Input.GetAxis("Mouse ScrollWheel");
		num = axis2;
		if (0f > axis2)
		{
			float timeScale5 = Time.timeScale;
			num = timeScale5;
			if (timeScale5 != 0f)
			{
				if (++itemSelected > 2)
				{
					itemSelected = 0;
				}
				goto IL_02a5;
			}
		}
		goto IL_0315;
		IL_02a5:
		int[] array = itemSelectOffset;
		num = array[itemSelected];
		float num4 = itemSelectPosition;
		itemSelect.anchoredPosition = NativeIntrinsics.Reinterpret<Vector2>(NativeIntrinsics.Asm("unpcklps xmm0,xmm1"));
		string[] array2 = itemNames;
		long num5 = item[itemSelected];
		string text = array2[(int)num5];
		string text2 = text;
		goto IL_0315;
		IL_03a3:
		float stamina = player.stamina;
		bool activeSelf = warning.activeSelf;
		GameObject gameObject3;
		if (!(0f > stamina) || ((activeSelf ? 1u : 0u) ^ 1u) == 0)
		{
			float stamina2 = player.stamina;
			bool activeSelf2 = warning.activeSelf;
			if (!(stamina2 > 0f) || !activeSelf2)
			{
				goto IL_0435;
			}
			gameObject3 = warning;
		}
		else
		{
			gameObject3 = warning;
		}
		gameObject3.SetActive(value: true);
		goto IL_0435;
		IL_0435:
		PlayerScript playerScript = player;
		if (playerScript.gameOver)
		{
			if (string.Equals(mode, "endless"))
			{
				int num6 = notebooks;
				int num7 = PlayerPrefs.GetInt("HighBooks");
				if (num6 > num7)
				{
					gameObject3 = highScoreText;
					if (!gameObject3.activeSelf)
					{
						gameObject3 = highScoreText;
						gameObject3.SetActive(value: true);
					}
				}
			}
			Time.timeScale = 0f;
			float num8 = gameOverDelay;
			float unscaledDeltaTime = Time.unscaledDeltaTime;
			Camera camera = playerCamera;
			gameOverDelay = num8 - unscaledDeltaTime * 0.5f;
			num = unscaledDeltaTime * 0.5f;
			camera.farClipPlane = (num8 - unscaledDeltaTime * 0.5f) * 400f;
			audioDevice.PlayOneShot(aud_buzz);
			if (!(0f < gameOverDelay))
			{
				if (string.Equals(mode, "endless"))
				{
					int num9 = notebooks;
					int num10 = PlayerPrefs.GetInt("HighBooks");
					if (num9 > num10)
					{
						PlayerPrefs.SetInt("HighBooks", notebooks);
					}
					PlayerPrefs.SetInt("CurrentBooks", notebooks);
				}
				Time.timeScale = 1f;
				string gameOverScene = GameOverScene;
				SceneManager.LoadScene(gameOverScene);
			}
		}
		if (finaleMode)
		{
			bool isPlaying = audioDevice.isPlaying;
			bool flag = isPlaying;
			if (!isPlaying && exitsReached == 3)
			{
				audioDevice.clip = aud_MachineLoop;
				audioDevice.loop = true;
				audioDevice.Play();
			}
		}
		return;
		IL_0315:
		float timeScale6 = Time.timeScale;
		num = timeScale6;
		if (timeScale6 != 0f)
		{
			InputManager instance3 = Singleton<InputManager>.Instance;
			if (!instance3.currentKeyStates[6])
			{
				InputManager instance4 = Singleton<InputManager>.Instance;
				if (!instance4.currentKeyStates[7])
				{
					InputManager instance5 = Singleton<InputManager>.Instance;
					if (!instance5.currentKeyStates[8])
					{
						goto IL_03a3;
					}
					itemSelected = 2;
				}
				else
				{
					itemSelected = 1;
				}
			}
			else
			{
				itemSelected = 0;
			}
			UpdateItemSelection();
		}
		goto IL_03a3;
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00246ED0 File Offset: 0x002454D0
	private unsafe void UpdateNotebookCount()
	{
		bool flag = string.Equals(mode, "story");
		long num = NativeIntrinsics.Reinterpret<long>(this) + 304L;
		TMP_Text tMP_Text = notebookCount;
		string text2;
		if (!flag)
		{
			string text = ((int*)num)->ToString();
			text2 = text;
		}
		else
		{
			string text3 = ((int*)num)->ToString();
			text2 = text3;
		}
		string text4 = text2 + "/7 Notebooks";
		text2 = text4;
		int num2 = notebooks;
		bool flag2 = string.Equals(mode, "story");
		int num3 = ((num2 == 7 && flag2) ? 1 : 0);
		int num4 = (flag2 ? 1 : 0);
		if (num3 != 0)
		{
			EntranceScript entranceScript = entrance_0;
			finaleMode = true;
			entranceScript.Raise();
			entrance_1.Raise();
			entrance_2.Raise();
			entrance_3.Raise();
		}
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00246060 File Offset: 0x00244660
	public void CollectNotebook()
	{
		notebooks++;
		UpdateNotebookCount();
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00246710 File Offset: 0x00244D10
	public void LockMouse()
	{
		if (!learningActive)
		{
			Cursor.lockState = NativeIntrinsics.Reinterpret<CursorLockMode>(1);
			Cursor.visible = false;
			GameObject gameObject = reticle;
			mouseLocked = true;
			gameObject.SetActive(value: true);
		}
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00246CC0 File Offset: 0x002452C0
	public void UnlockMouse()
	{
		Cursor.lockState = NativeIntrinsics.Reinterpret<CursorLockMode>(0);
		Cursor.visible = true;
		GameObject gameObject = reticle;
		mouseLocked = false;
		gameObject.SetActive(value: false);
	}

	// Token: 0x06000023 RID: 35 RVA: 0x00246850 File Offset: 0x00244E50
	public void PauseGame()
	{
		if (!learningActive)
		{
			Cursor.lockState = NativeIntrinsics.Reinterpret<CursorLockMode>(0);
			Cursor.visible = true;
			GameObject gameObject = reticle;
			mouseLocked = false;
			gameObject.SetActive(value: false);
			Time.timeScale = 0f;
			GameObject gameObject2 = pauseMenu;
			gamePaused = true;
			gameObject2.SetActive(value: true);
		}
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00246320 File Offset: 0x00244920
	public void ExitGame()
	{
		Time.timeScale = 1f;
		string exitGameScene = ExitGameScene;
		SceneManager.LoadScene(exitGameScene);
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00246D10 File Offset: 0x00245310
	public void UnpauseGame()
	{
		Time.timeScale = 1f;
		GameObject gameObject = pauseMenu;
		gamePaused = false;
		gameObject.SetActive(value: false);
		if (!learningActive)
		{
			Cursor.lockState = NativeIntrinsics.Reinterpret<CursorLockMode>(1);
			Cursor.visible = false;
			GameObject gameObject2 = reticle;
			mouseLocked = true;
			gameObject2.SetActive(value: true);
		}
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00245C70 File Offset: 0x00244270
	public void ActivateSpoopMode()
	{
		spoopMode = true;
		entrance_0.Lower();
		entrance_1.Lower();
		entrance_2.Lower();
		entrance_3.Lower();
		baldiTutor.SetActive(value: false);
		baldi.SetActive(value: true);
		principal.SetActive(value: true);
		crafters.SetActive(value: true);
		playtime.SetActive(value: true);
		gottaSweep.SetActive(value: true);
		bully.SetActive(value: true);
		firstPrize.SetActive(value: true);
		audioDevice.PlayOneShot(aud_Hang);
		learnMusic.Stop();
		schoolMusic.Stop();
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00245B70 File Offset: 0x00244170
	private void ActivateFinaleMode()
	{
		finaleMode = true;
		entrance_0.Raise();
		entrance_1.Raise();
		entrance_2.Raise();
		entrance_3.Raise();
	}

	// Token: 0x06000028 RID: 40 RVA: 0x002465A0 File Offset: 0x00244BA0
	public void GetAngry(float value)
	{
		if (!spoopMode)
		{
			ActivateSpoopMode();
		}
		BaldiScript baldiScript = baldiScrpt;
		baldiScript.baldiAnger = value + baldiScript.baldiAnger;
		if (0.5f > value + baldiScript.baldiAnger)
		{
			baldiScript.baldiAnger = 0.5f;
		}
		float num = 2f / baldiScript.baldiSpeedScale + baldiScript.baldiAnger;
		baldiScript.baldiWait = baldiScript.baldiAnger * -3f / (2f / baldiScript.baldiSpeedScale + baldiScript.baldiAnger) + 3f;
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00245BD0 File Offset: 0x002441D0
	public void ActivateLearningGame()
	{
		learningActive = true;
		Cursor.lockState = NativeIntrinsics.Reinterpret<CursorLockMode>(0);
		Cursor.visible = true;
		GameObject gameObject = reticle;
		mouseLocked = false;
		gameObject.SetActive(value: false);
		tutorBaldi.Stop();
		if (!spoopMode)
		{
			schoolMusic.Stop();
			learnMusic.Play();
		}
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00246070 File Offset: 0x00244670
	public void DeactivateLearningGame(GameObject subject)
	{
		Camera camera = playerCamera;
		camera.cullingMask = cullingMask;
		learningActive = false;
		UnityEngine.Object.Destroy(subject);
		if (!learningActive)
		{
			Cursor.lockState = NativeIntrinsics.Reinterpret<CursorLockMode>(1);
			Cursor.visible = false;
			GameObject gameObject = reticle;
			mouseLocked = true;
			gameObject.SetActive(value: true);
		}
		PlayerScript playerScript = player;
		if (100f > playerScript.stamina)
		{
			playerScript.stamina = 100f;
		}
		if (!spoopMode)
		{
			schoolMusic.Play();
			learnMusic.Stop();
		}
		if (!((notebooks == 1) & !spoopMode))
		{
			int num = notebooks;
			bool flag = string.Equals(mode, "story");
			int num2 = ((num == 7 && flag) ? 1 : 0);
			int num3 = (flag ? 1 : 0);
			if (num2 != 0)
			{
				audioDevice.PlayOneShot(aud_AllNotebooks, 0.8f);
			}
		}
		else
		{
			quarter.SetActive(value: true);
			tutorBaldi.PlayOneShot(aud_Prize);
		}
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00246630 File Offset: 0x00244C30
	private void IncreaseItemSelection()
	{
		if (++itemSelected > 2)
		{
			itemSelected = 0;
		}
		itemSelect.anchoredPosition = NativeIntrinsics.Reinterpret<Vector2>(NativeIntrinsics.Asm("unpcklps xmm0,xmm1"));
		throw null;
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00246250 File Offset: 0x00244850
	private void DecreaseItemSelection()
	{
		itemSelected--;
		if (itemSelected - 1 < 0)
		{
			itemSelected = 2;
		}
		itemSelect.anchoredPosition = NativeIntrinsics.Reinterpret<Vector2>(NativeIntrinsics.Asm("unpcklps xmm0,xmm1"));
		throw null;
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00246E10 File Offset: 0x00245410
	private void UpdateItemSelection()
	{
		itemSelect.anchoredPosition = NativeIntrinsics.Reinterpret<Vector2>(NativeIntrinsics.Asm("unpcklps xmm0,xmm1"));
		throw null;
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00245E60 File Offset: 0x00244460
	public void CollectItem(int item_ID)
	{
		int[] array = item;
		int num = array.Length;
		int[] array2 = array;
		RawImage rawImage4 = default(RawImage);
		if (array[0] != 0)
		{
			num = array.Length;
			if (array[1] != 0)
			{
				if (array[2] != 0)
				{
					array2[itemSelected] = item_ID;
					RawImage[] array3 = itemSlot;
					Texture[] array4 = itemTextures;
					RawImage rawImage = array3[itemSelected];
				}
				else
				{
					array[2] = item_ID;
					RawImage[] array5 = itemSlot;
					Texture[] array4 = itemTextures;
					RawImage rawImage2 = array5[2];
				}
			}
			else
			{
				array[1] = item_ID;
				RawImage[] array6 = itemSlot;
				Texture[] array4 = itemTextures;
				RawImage rawImage3 = array6[1];
			}
		}
		else
		{
			array[0] = item_ID;
			RawImage[] array7 = itemSlot;
			Texture[] array4 = itemTextures;
			rawImage4 = array7[0];
		}
		rawImage4.texture = NativeIntrinsics.Reinterpret<Texture>(NativeIntrinsics.Asm("*(BinaryExpr + 0x0)"));
		throw null;
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00247790 File Offset: 0x00245D90
	private unsafe void UseItem()
	{
		int[] array = item;
		long num = itemSelected;
		if (array[(int)num] == 0)
		{
			return;
		}
		int[] array2 = item;
		if (item[itemSelected] != 1)
		{
			if (array2[itemSelected] != 2)
			{
				if (array2[itemSelected] != 3)
				{
					if (array2[itemSelected] == 4)
					{
						Transform transform = playerTransform;
						GameObject gameObject = bsodaSpray;
						Vector3 position = transform.position;
						GameObject gameObject2 = gameObject;
						transform = cameraTransform;
						Quaternion rotation = transform.rotation;
						long num2 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
						long num3 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
						float z = position.z;
						GameObject original = gameObject2;
						Vector3 vector = default(Vector3);
						vector.x = NativeIntrinsics.Reinterpret<float>(num3);
						vector.z = z;
						Vector3 position2 = vector;
						Quaternion rotation2 = default(Quaternion);
						rotation2.x = NativeIntrinsics.Reinterpret<float>(num2);
						GameObject gameObject3 = UnityEngine.Object.Instantiate(original, position2, rotation2);
						ResetItem();
						player.ResetGuilt("drink", 1f);
						audioDevice.PlayOneShot(aud_Soda);
						return;
					}
					long num15 = default(long);
					if (array2[itemSelected] != 5)
					{
						long num2;
						float y;
						if (array2[itemSelected] != 6)
						{
							if (array2[itemSelected] != 7)
							{
								if (array2[itemSelected] == 8)
								{
									Camera main = Camera.main;
									int width = Screen.width;
									int height = Screen.height;
									float x = width - (int)NativeIntrinsics.Asm("rdx") >> 1;
									y = height - (int)NativeIntrinsics.Asm("rdx") >> 1;
									Vector3 pos = default(Vector3);
									pos.x = x;
									pos.y = y;
									pos.z = 0f;
									Ray ray = main.ScreenPointToRay(pos);
									long num4 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
									num2 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x10, 8)");
									Ray ray2 = default(Ray);
									ray2.m_Origin.x = NativeIntrinsics.Reinterpret<float>(num4);
									bool flag = Physics.Raycast(ray2, out *(RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFF68]") + -128L));
									bool flag2 = flag;
									if (flag)
									{
										Collider collider = ((RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFF68]") + -128L))->collider;
										Collider collider2 = collider;
										string a = collider2.tag;
										bool flag3 = string.Equals(a, "Door");
										bool flag4 = flag3;
										Vector3 position3 = playerTransform.position;
										Transform transform2 = ((RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFF68]") + -128L))->transform;
										Transform transform3 = transform2;
										Vector3 position4 = transform3.position;
										Vector3 vector2 = default(Vector3);
										vector2.x = position3.x;
										vector2.y = position3.y;
										vector2.z = position3.z;
										Vector3 a2 = vector2;
										Vector3 b = default(Vector3);
										b.x = position4.x;
										b.y = position4.y;
										b.z = position4.z;
										float num5 = Vector3.Distance(a2, b);
										int num6 = ((!(10f < num5) && flag3) ? 1 : 0);
										int num7 = num6;
										y = num5;
										if (num6 != 0)
										{
											Collider collider3 = ((RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFF68]") + -128L))->collider;
											Collider collider4 = collider3;
											GameObject gameObject4 = collider4.gameObject;
											GameObject gameObject5 = gameObject4;
											DoorScript component = gameObject5.GetComponent<DoorScript>();
											DoorScript doorScript = component;
											doorScript.silentOpens = 4;
											ResetItem();
											audioDevice.PlayOneShot(aud_Spray);
										}
									}
									return;
								}
								if (array2[itemSelected] != 9)
								{
									if (array2[itemSelected] != 10)
									{
										long num8 = itemSelected;
										if (array2[itemSelected] != 11)
										{
											return;
										}
									}
									else
									{
										PlayerScript playerScript = player;
										playerScript.ActivateBoots();
									}
									<Teleporter>d__20 <Teleporter>d__ = NativeIntrinsics.Reinterpret<<Teleporter>d__20>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(GameControllerScript+<Teleporter>d__20)"));
									((Mono.Xml.SecurityParser)(object)<Teleporter>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
									<Teleporter>d__.<>1__state = 0;
									<Teleporter>d__.<>4__this = this;
									Coroutine coroutine = StartCoroutine(<Teleporter>d__);
								}
								else
								{
									Camera main2 = Camera.main;
									int width2 = Screen.width;
									int height2 = Screen.height;
									y = height2 - (int)NativeIntrinsics.Asm("rdx") >> 1;
									int num9 = height2 - (int)NativeIntrinsics.Asm("rdx") >> 1;
									Vector3 pos2 = default(Vector3);
									pos2.x = width2 - (int)NativeIntrinsics.Asm("rdx") >> 1;
									pos2.y = y;
									pos2.z = 0f;
									Ray ray3 = main2.ScreenPointToRay(pos2);
									if (!player.jumpRope)
									{
										long num10 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
										num2 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x10, 8)");
										Ray ray4 = default(Ray);
										ray4.m_Origin.x = NativeIntrinsics.Reinterpret<float>(num10);
										bool flag5 = Physics.Raycast(ray4, out *(RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFF68]") + 64L));
										bool flag6 = flag5;
										if (!flag5)
										{
											return;
										}
										Collider collider5 = ((RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFF68]") + 64L))->collider;
										Collider collider6 = collider5;
										string a3 = collider6.name;
										bool flag7 = string.Equals(a3, "1st Prize");
										bool flag2 = flag7;
										if (!flag7)
										{
											return;
										}
										firstPrizeScript.crazyTime = 15f;
									}
									else
									{
										player.DeactivateJumpRope();
										PlaytimeScript playtimeScript = this.playtimeScript;
										this.playtimeScript.animator.SetBool("disappointed", value: true);
										playtimeScript.audioDevice.Stop();
										playtimeScript.audioDevice.PlayOneShot(playtimeScript.aud_Sad);
									}
								}
							}
							else
							{
								Transform transform = playerTransform;
								GameObject gameObject6 = alarmClock;
								Vector3 position5 = transform.position;
								GameObject gameObject2 = gameObject6;
								transform = cameraTransform;
								Quaternion rotation3 = transform.rotation;
								num2 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
								long num4 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
								float z = position5.z;
								GameObject original2 = gameObject2;
								Vector3 vector3 = default(Vector3);
								vector3.x = NativeIntrinsics.Reinterpret<float>(num4);
								vector3.z = z;
								Vector3 position6 = vector3;
								Quaternion rotation4 = default(Quaternion);
								rotation4.x = NativeIntrinsics.Reinterpret<float>(num2);
								GameObject gameObject7 = UnityEngine.Object.Instantiate(original2, position6, rotation4);
								AlarmClockScript component2 = gameObject7.GetComponent<AlarmClockScript>();
								BaldiScript baldiScript = baldiScrpt;
								component2.baldi = baldiScript;
							}
							goto IL_07d6;
						}
						Camera main3 = Camera.main;
						int width3 = Screen.width;
						int height3 = Screen.height;
						float x2 = width3 - (int)NativeIntrinsics.Asm("rdx") >> 1;
						y = height3 - (int)NativeIntrinsics.Asm("rdx") >> 1;
						Vector3 pos3 = default(Vector3);
						pos3.x = x2;
						pos3.y = y;
						pos3.z = 0f;
						Ray ray5 = main3.ScreenPointToRay(pos3);
						long num11 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
						num2 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x10, 8)");
						Ray ray6 = default(Ray);
						ray6.m_Origin.x = NativeIntrinsics.Reinterpret<float>(num11);
						bool flag8 = Physics.Raycast(ray6, out *(RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFF68]") + -80L));
						bool flag9 = flag8;
						if (!flag8)
						{
							return;
						}
						Collider collider7 = ((RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFF68]") + -80L))->collider;
						Collider collider8 = collider7;
						string a4 = collider8.name;
						bool flag10 = string.Equals(a4, "TapePlayer");
						bool flag11 = flag10;
						Vector3 position7 = playerTransform.position;
						Transform transform4 = ((RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFF68]") + -80L))->transform;
						Transform transform5 = transform4;
						Vector3 position8 = transform5.position;
						Vector3 vector4 = default(Vector3);
						vector4.x = position7.x;
						vector4.y = position7.y;
						vector4.z = position7.z;
						Vector3 a5 = vector4;
						Vector3 b2 = default(Vector3);
						b2.x = position8.x;
						b2.y = position8.y;
						b2.z = position8.z;
						float num12 = Vector3.Distance(a5, b2);
						int num13 = ((!(10f < num12) && flag10) ? 1 : 0);
						int num14 = num13;
						y = num12;
						if (num13 == 0)
						{
							return;
						}
						num15 = NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFF68]") + -80L;
					}
					else
					{
						Camera main4 = Camera.main;
						int width4 = Screen.width;
						int height4 = Screen.height;
						float x3 = width4 - (int)NativeIntrinsics.Asm("rdx") >> 1;
						float y = height4 - (int)NativeIntrinsics.Asm("rdx") >> 1;
						Vector3 pos4 = default(Vector3);
						pos4.x = x3;
						pos4.y = y;
						pos4.z = 0f;
						Ray ray7 = main4.ScreenPointToRay(pos4);
						long num3 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
						long num2 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x10, 8)");
						Ray ray8 = default(Ray);
						ray8.m_Origin.x = NativeIntrinsics.Reinterpret<float>(num3);
						RaycastHit hitInfo;
						bool flag12 = Physics.Raycast(ray8, out hitInfo);
						bool flag13 = flag12;
						if (!flag12)
						{
							return;
						}
						RaycastHit raycastHit = default(RaycastHit);
						Collider collider9 = raycastHit.collider;
						string a6 = collider9.name;
						bool flag14 = string.Equals(a6, "BSODAMachine");
						Transform transform = playerTransform;
						Vector3 position9 = transform.position;
						RaycastHit raycastHit2 = default(RaycastHit);
						Transform transform6 = raycastHit2.transform;
						float z = position9.z;
						Vector3 position10 = transform6.position;
						Vector3 vector5 = default(Vector3);
						vector5.x = position9.x;
						vector5.y = position9.y;
						vector5.z = z;
						Vector3 a7 = vector5;
						Vector3 b3 = default(Vector3);
						b3.x = position10.x;
						b3.y = position10.y;
						b3.z = position10.z;
						float num16 = Vector3.Distance(a7, b3);
						int num17 = ((!(10f < num16) && flag14) ? 1 : 0);
						y = num16;
						if (num17 != 0)
						{
							ResetItem();
							CollectItem(4);
							return;
						}
						RaycastHit raycastHit3 = default(RaycastHit);
						Collider collider10 = raycastHit3.collider;
						string a8 = collider10.name;
						bool flag15 = string.Equals(a8, "ZestyMachine");
						transform = playerTransform;
						Vector3 position11 = transform.position;
						RaycastHit raycastHit4 = default(RaycastHit);
						Transform transform7 = raycastHit4.transform;
						Vector3 position12 = transform7.position;
						Vector3 vector6 = default(Vector3);
						vector6.x = position11.x;
						vector6.y = position11.y;
						vector6.z = position11.z;
						Vector3 a9 = vector6;
						Vector3 b4 = default(Vector3);
						b4.x = position12.x;
						b4.y = position12.y;
						b4.z = position12.z;
						float num18 = Vector3.Distance(a9, b4);
						int num19 = ((!(10f < num18) && flag15) ? 1 : 0);
						y = num18;
						if (num19 != 0)
						{
							ResetItem();
							CollectItem(1);
							return;
						}
						RaycastHit raycastHit5 = default(RaycastHit);
						Collider collider11 = raycastHit5.collider;
						Collider collider12 = collider11;
						string a10 = collider12.name;
						bool flag16 = string.Equals(a10, "PayPhone");
						bool flag17 = flag16;
						Vector3 position13 = playerTransform.position;
						RaycastHit raycastHit6 = default(RaycastHit);
						Transform transform8 = raycastHit6.transform;
						Transform transform9 = transform8;
						Vector3 position14 = transform9.position;
						Vector3 vector7 = default(Vector3);
						vector7.x = position13.x;
						vector7.y = position13.y;
						vector7.z = position13.z;
						Vector3 a11 = vector7;
						Vector3 b5 = default(Vector3);
						b5.x = position14.x;
						b5.y = position14.y;
						b5.z = position14.z;
						float num20 = Vector3.Distance(a11, b5);
						int num21 = ((!(10f < num20) && flag16) ? 1 : 0);
						int num22 = num21;
						y = num20;
						if (num21 == 0)
						{
							return;
						}
					}
					Collider collider13 = ((RaycastHit*)num15)->collider;
					GameObject gameObject8 = collider13.gameObject;
					TapePlayerScript component3 = gameObject8.GetComponent<TapePlayerScript>();
					component3.Play();
				}
				else
				{
					Camera main5 = Camera.main;
					int width5 = Screen.width;
					int height5 = Screen.height;
					float x4 = width5 - (int)NativeIntrinsics.Asm("rdx") >> 1;
					float y = height5 - (int)NativeIntrinsics.Asm("rdx") >> 1;
					Vector3 pos5 = default(Vector3);
					pos5.x = x4;
					pos5.y = y;
					pos5.z = 0f;
					Ray ray9 = main5.ScreenPointToRay(pos5);
					long num23 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
					long num2 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x10, 8)");
					Ray ray10 = default(Ray);
					ray10.m_Origin.x = NativeIntrinsics.Reinterpret<float>(num23);
					bool flag18 = Physics.Raycast(ray10, out *(RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFF68]") + -32L));
					bool flag19 = flag18;
					if (!flag18)
					{
						return;
					}
					Collider collider14 = ((RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFF68]") + -32L))->collider;
					Collider collider15 = collider14;
					string a12 = collider15.tag;
					bool flag20 = string.Equals(a12, "Door");
					bool flag21 = flag20;
					Vector3 position15 = playerTransform.position;
					Transform transform10 = ((RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFF68]") + -32L))->transform;
					float z = position15.z;
					Transform transform11 = transform10;
					Vector3 position16 = transform11.position;
					Vector3 vector8 = default(Vector3);
					vector8.x = position15.x;
					vector8.y = position15.y;
					vector8.z = z;
					Vector3 a13 = vector8;
					Vector3 b6 = default(Vector3);
					b6.x = position16.x;
					b6.y = position16.y;
					b6.z = position16.z;
					float num24 = Vector3.Distance(a13, b6);
					int num25 = ((!(10f < num24) && flag20) ? 1 : 0);
					y = num24;
					int num26 = num25;
					if (num25 == 0)
					{
						return;
					}
					Collider collider16 = ((RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFF68]") + -32L))->collider;
					Collider collider17 = collider16;
					GameObject gameObject9 = collider17.gameObject;
					GameObject gameObject10 = gameObject9;
					DoorScript component4 = gameObject10.GetComponent<DoorScript>();
					DoorScript doorScript2 = component4;
					if (!doorScript2.bDoorLocked)
					{
						return;
					}
					doorScript2.bDoorLocked = false;
					doorScript2.OpenDoor();
				}
			}
			else
			{
				Camera main6 = Camera.main;
				int width6 = Screen.width;
				int height6 = Screen.height;
				float x5 = width6 - (int)NativeIntrinsics.Asm("rdx") >> 1;
				float y = height6 - (int)NativeIntrinsics.Asm("rdx") >> 1;
				Vector3 pos6 = default(Vector3);
				pos6.x = x5;
				pos6.y = y;
				pos6.z = 0f;
				Ray ray11 = main6.ScreenPointToRay(pos6);
				long num27 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
				long num2 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x10, 8)");
				Ray ray12 = default(Ray);
				ray12.m_Origin.x = NativeIntrinsics.Reinterpret<float>(num27);
				bool flag22 = Physics.Raycast(ray12, out *(RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFF68]") + 16L));
				bool flag23 = flag22;
				if (!flag22)
				{
					return;
				}
				Collider collider18 = ((RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFF68]") + 16L))->collider;
				Collider collider19 = collider18;
				string a14 = collider19.tag;
				bool flag24 = string.Equals(a14, "SwingingDoor");
				bool flag25 = flag24;
				Vector3 position17 = playerTransform.position;
				Transform transform12 = ((RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFF68]") + 16L))->transform;
				Transform transform13 = transform12;
				Vector3 position18 = transform13.position;
				Vector3 vector9 = default(Vector3);
				vector9.x = position17.x;
				vector9.y = position17.y;
				vector9.z = position17.z;
				Vector3 a15 = vector9;
				Vector3 b7 = default(Vector3);
				b7.x = position18.x;
				b7.y = position18.y;
				b7.z = position18.z;
				float num28 = Vector3.Distance(a15, b7);
				int num29 = ((!(10f < num28) && flag24) ? 1 : 0);
				y = num28;
				int num30 = num29;
				if (num29 == 0)
				{
					return;
				}
				Collider collider20 = ((RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFF68]") + 16L))->collider;
				GameObject gameObject11 = collider20.gameObject;
				SwingingDoorScript component5 = gameObject11.GetComponent<SwingingDoorScript>();
				component5.LockDoor(15f);
			}
		}
		else
		{
			float num31 = player.maxStamina + player.maxStamina;
			player.stamina = num31;
			float y = num31;
		}
		goto IL_07d6;
		IL_07d6:
		ResetItem();
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00245DF0 File Offset: 0x002443F0
	private IEnumerator BootAnimation()
	{
		<BootAnimation>d__19 <BootAnimation>d__ = NativeIntrinsics.Reinterpret<<BootAnimation>d__19>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(GameControllerScript+<BootAnimation>d__19)"));
		((Mono.Xml.SecurityParser)(object)<BootAnimation>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<BootAnimation>d__.<>1__state = 0;
		<BootAnimation>d__.<>4__this = this;
		return <BootAnimation>d__;
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00246C50 File Offset: 0x00245250
	private IEnumerator Teleporter()
	{
		<Teleporter>d__20 <Teleporter>d__ = NativeIntrinsics.Reinterpret<<Teleporter>d__20>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(GameControllerScript+<Teleporter>d__20)"));
		((Mono.Xml.SecurityParser)(object)<Teleporter>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<Teleporter>d__.<>1__state = 0;
		<Teleporter>d__.<>4__this = this;
		return <Teleporter>d__;
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00246B00 File Offset: 0x00245100
	private void Teleport()
	{
		AILocationSelector.GetNewTarget();
		Transform transform = player.transform;
		Transform transform2 = AILocationSelector.transform;
		Vector3 position = transform2.position;
		Vector3 position2 = default(Vector3);
		position2.x = player.height * Vector3.upVector.x + position.x;
		position2.y = player.height * Vector3.upVector.y + position.y;
		position2.z = player.height * Vector3.upVector.z + position.z;
		transform.position = position2;
		audioDevice.PlayOneShot(aud_Teleport);
	}

	// Token: 0x06000033 RID: 51 RVA: 0x002468D0 File Offset: 0x00244ED0
	private void ResetItem()
	{
		item[itemSelected] = 0;
		itemSlot[itemSelected].texture = itemTextures[0];
		throw null;
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00246770 File Offset: 0x00244D70
	public void LoseItem(int id)
	{
		item[id] = 0;
		itemSlot[id].texture = itemTextures[0];
		throw null;
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00246DA0 File Offset: 0x002453A0
	private void UpdateItemName()
	{
		throw null;
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00246380 File Offset: 0x00244980
	public void ExitReached()
	{
		int num = exitsReached;
		long num2 = exitsReached + 1L;
		exitsReached = (int)num2;
		long num3 = num2;
		int num4 = num;
		if (num == 0)
		{
			Color ambientLight = default(Color);
			ambientLight.r = NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("global_13EAC10"));
			RenderSettings.ambientLight = ambientLight;
			audioDevice.PlayOneShot(aud_Switch, 0.8f);
			audioDevice.clip = aud_MachineQuiet;
			audioDevice.loop = true;
			audioDevice.Play();
		}
		if (exitsReached == 2)
		{
			audioDevice.volume = 0.8f;
			audioDevice.clip = aud_MachineStart;
			audioDevice.loop = true;
			audioDevice.Play();
		}
		if (exitsReached == 3)
		{
			audioDevice.clip = aud_MachineRev;
			audioDevice.loop = false;
			audioDevice.Play();
		}
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00246500 File Offset: 0x00244B00
	public void Fliparoo()
	{
		PlayerScript playerScript = player;
		flipped = true;
		playerScript.height = 6f;
		player.fliparoo = 180f;
		player.flipaturn = -1f;
		Camera main = Camera.main;
		CameraScript component = main.GetComponent<CameraScript>();
		component.offset.y = -1f;
		component.offset = NativeIntrinsics.Reinterpret<Vector3>(0);
		component.offset.z = 0f;
	}
}

