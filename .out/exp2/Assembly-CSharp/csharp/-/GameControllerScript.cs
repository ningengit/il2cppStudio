using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
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
				if (num != 0)
				{
					num--;
					if (num != 0)
					{
						if (num != 1)
						{
							goto IL_0351;
						}
						else
						{
							<>1__state = -1;
							goto IL_024d;
						}
					}
					<>1__state = -1;
					goto IL_01ce;
				}
				<>1__state = -1;
			}
			else
			{
				<>1__state = -1;
				<time>5__2 = 15f;
				<height>5__3 = 375f;
				gameControllerScript.boots.gameObject.SetActive(value: true);
			}
			float num2 = <height>5__3;
			if (<height>5__3 > -375f)
			{
				<height>5__3 = num2 - Time.deltaTime * 375f;
				<time>5__2 -= Time.deltaTime;
				Vector3 localPosition = gameControllerScript.boots.localPosition;
				RectTransform boots = gameControllerScript.boots;
				Vector3 localPosition2 = new Vector3(localPosition.x, <height>5__3, localPosition.z);
				boots.localPosition = localPosition2;
				<>2__current = null;
				<>1__state = 1;
				goto IL_015a;
			}
			Vector3 localPosition3 = gameControllerScript.boots.localPosition;
			float x = localPosition3.x;
			RectTransform boots2 = gameControllerScript.boots;
			Vector3 localPosition4 = new Vector3(x, -375f, localPosition3.z);
			boots2.localPosition = localPosition4;
			gameControllerScript.boots.gameObject.SetActive(value: false);
			goto IL_01ce;
			IL_015a:
			return true;
			IL_0351:
			return false;
			IL_01ce:
			float num3 = <time>5__2;
			if (<time>5__2 > 0f)
			{
				float deltaTime = Time.deltaTime;
				<>2__current = null;
				<time>5__2 = num3 - deltaTime;
				<>1__state = 2;
				goto IL_015a;
			}
			gameControllerScript.boots.gameObject.SetActive(value: true);
			goto IL_024d;
			IL_024d:
			if (375f > <height>5__3)
			{
				float num4 = <height>5__3;
				<height>5__3 = Time.deltaTime * 375f + num4;
				Vector3 localPosition5 = gameControllerScript.boots.localPosition;
				RectTransform boots3 = gameControllerScript.boots;
				Vector3 localPosition6 = new Vector3(localPosition5.x, <height>5__3, localPosition5.z);
				boots3.localPosition = localPosition6;
				<>2__current = null;
				<>1__state = 3;
				goto IL_015a;
			}
			Vector3 localPosition7 = gameControllerScript.boots.localPosition;
			RectTransform boots4 = gameControllerScript.boots;
			Vector3 localPosition8 = new Vector3(localPosition7.x, 375f, localPosition7.z);
			boots4.localPosition = localPosition8;
			gameControllerScript.boots.gameObject.SetActive(value: false);
			goto IL_0351;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00250B00 File Offset: 0x0024F100
		private void System.Collections.IEnumerator.Reset()
		{
			throw new NotSupportedException();
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
		cullingMask = playerCamera.cullingMask;
		audioDevice = GetComponent<AudioSource>();
		mode = PlayerPrefs.GetString("CurrentMode");
		if (mode == "endless")
		{
			baldiScrpt.endless = true;
		}
		schoolMusic.Play();
		if (!learningActive)
		{
			Cursor.lockState = CursorLockMode.Locked;
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
			num = 0f;
			if (timeScale != 0f)
			{
				Time.timeScale = 0f;
				num = 0f;
			}
			goto IL_097c;
		}
		InputManager instance = Singleton<InputManager>.Instance;
		if (instance.currentKeyStates[12] && !instance.previousKeyStates[12] && !player.gameOver)
		{
			if (gamePaused)
			{
				UnpauseGame();
			}
			else if (!learningActive)
			{
				Cursor.lockState = CursorLockMode.None;
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
		bool keyDownInt = Input.GetKeyDownInt(KeyCode.Y);
		if ((gamePaused && keyDownInt))
		{
			Time.timeScale = 1f;
			SceneManager.LoadScene(ExitGameScene);
		}
		else if (Input.GetKeyDownInt(KeyCode.N) & gamePaused)
		{
			UnpauseGame();
		}
		int num2 = (gamePaused ? 1 : 0);
		float timeScale2 = Time.timeScale;
		if (timeScale2 != 1f && num2 == 0)
		{
			Time.timeScale = 1f;
		}
		instance = Singleton<InputManager>.Instance;
		if (instance.currentKeyStates[5] && !instance.previousKeyStates[5] && Time.timeScale != 0f)
		{
			UseItem();
		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0f && Time.timeScale != 0f)
		{
			itemSelected--;
			if (itemSelected - 1 < 0)
			{
				itemSelected = 2;
			}
		}
		else if ((0f > Input.GetAxis("Mouse ScrollWheel")) && Time.timeScale != 0f)
		{
			if (++itemSelected > 2)
			{
				itemSelected = 0;
			}
		}
		else
		{
			goto IL_07a2;
		}
		float x = itemSelectOffset[itemSelected];
		itemSelect.anchoredPosition = new Vector2(x, itemSelectPosition);
		string text = itemNames[item[itemSelected]];
		itemText.text = text;
		goto IL_07a2;
		IL_07a2:
		float timeScale3 = Time.timeScale;
		num = 0f;
		if (timeScale3 != 0f)
		{
			if (Singleton<InputManager>.Instance.currentKeyStates[6])
			{
				itemSelected = 0;
			}
			else if (!Singleton<InputManager>.Instance.currentKeyStates[7])
			{
				InputManager instance2 = Singleton<InputManager>.Instance;
				num = 0f;
				if (!instance2.currentKeyStates[8])
				{
					goto IL_097c;
				}
				itemSelected = 2;
			}
			else
			{
				itemSelected = 1;
			}
			UpdateItemSelection();
			num = 0f;
		}
		goto IL_097c;
		IL_0a75:
		if (player.gameOver)
		{
			if (mode == "endless" && notebooks > PlayerPrefs.GetInt("HighBooks") && !highScoreText.activeSelf)
			{
				highScoreText.SetActive(value: true);
			}
			Time.timeScale = 0f;
			float num4 = gameOverDelay;
			float unscaledDeltaTime = Time.unscaledDeltaTime;
			Camera camera = playerCamera;
			gameOverDelay = num4 - unscaledDeltaTime * 0.5f;
			camera.farClipPlane = (num4 - unscaledDeltaTime * 0.5f) * 400f;
			audioDevice.PlayOneShot(aud_buzz);
			if (!(num < gameOverDelay))
			{
				if (mode == "endless")
				{
					if (notebooks > PlayerPrefs.GetInt("HighBooks"))
					{
						PlayerPrefs.SetInt("HighBooks", notebooks);
					}
					PlayerPrefs.SetInt("CurrentBooks", notebooks);
				}
				Time.timeScale = 1f;
				SceneManager.LoadScene(GameOverScene);
			}
		}
		if (finaleMode && !audioDevice.isPlaying && exitsReached == 3)
		{
			audioDevice.clip = aud_MachineLoop;
			audioDevice.loop = true;
			audioDevice.Play();
		}
		return;
		IL_097c:
		float stamina = player.stamina;
		bool activeSelf = warning.activeSelf;
		GameObject gameObject3;
		int active;
		if (!(num > stamina) || activeSelf)
		{
			float stamina2 = player.stamina;
			if ((stamina2 > num) && warning.activeSelf)
			{
				gameObject3 = warning;
				active = 0;
			}
			else
			{
				goto IL_0a75;
			}
		}
		else
		{
			gameObject3 = warning;
			active = 1;
		}
		gameObject3.SetActive((byte)active != 0);
		goto IL_0a75;
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00246ED0 File Offset: 0x002454D0
	private unsafe void UpdateNotebookCount()
	{
		bool flag = mode == "story";
		long num = NativeIntrinsics.Reinterpret<long>(this) + 304L;
		notebookCount.text = (flag ? ((int*)num)->ToString() : ((int*)num)->ToString()) + " Notebooks";
		int num2 = notebooks;
		bool flag2 = mode == "story";
		if (num2 == 7 && flag2)
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
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			GameObject gameObject = reticle;
			mouseLocked = true;
			gameObject.SetActive(value: true);
		}
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00246CC0 File Offset: 0x002452C0
	public void UnlockMouse()
	{
		Cursor.lockState = CursorLockMode.None;
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
			Cursor.lockState = CursorLockMode.None;
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
		SceneManager.LoadScene(ExitGameScene);
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
			Cursor.lockState = CursorLockMode.Locked;
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
		_ = 2f / baldiScript.baldiSpeedScale + baldiScript.baldiAnger;
		baldiScript.baldiWait = baldiScript.baldiAnger * -3f / (2f / baldiScript.baldiSpeedScale + baldiScript.baldiAnger) + 3f;
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00245BD0 File Offset: 0x002441D0
	public void ActivateLearningGame()
	{
		learningActive = true;
		Cursor.lockState = CursorLockMode.None;
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
		playerCamera.cullingMask = cullingMask;
		learningActive = false;
		UnityEngine.Object.Destroy(subject);
		if (!learningActive)
		{
			Cursor.lockState = CursorLockMode.Locked;
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
		if (((notebooks == 1) & !spoopMode))
		{
			quarter.SetActive(value: true);
			tutorBaldi.PlayOneShot(aud_Prize);
		}
		else if ((notebooks == 7) & (mode == "story"))
		{
			audioDevice.PlayOneShot(aud_AllNotebooks, 0.8f);
		}
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00246630 File Offset: 0x00244C30
	private void IncreaseItemSelection()
	{
		if (++itemSelected > 2)
		{
			itemSelected = 0;
		}
		itemSelect.anchoredPosition = new Vector2(itemSelectOffset[itemSelected], itemSelectPosition);
		itemText.text = itemNames[item[itemSelected]];
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00246250 File Offset: 0x00244850
	private void DecreaseItemSelection()
	{
		itemSelected--;
		if (itemSelected - 1 < 0)
		{
			itemSelected = 2;
		}
		itemSelect.anchoredPosition = new Vector2(itemSelectOffset[itemSelected], itemSelectPosition);
		itemText.text = itemNames[item[itemSelected]];
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00246E10 File Offset: 0x00245410
	private void UpdateItemSelection()
	{
		itemSelect.anchoredPosition = new Vector2(itemSelectOffset[itemSelected], itemSelectPosition);
		itemText.text = itemNames[item[itemSelected]];
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00245E60 File Offset: 0x00244460
	public void CollectItem(int item_ID)
	{
		int[] array = item;
		_ = array.Length;
		int[] array2 = array;
		RawImage rawImage = default(RawImage);
		if (array[0] != 0)
		{
			_ = array.Length;
			if (array[1] == 0)
			{
				array[1] = item_ID;
				RawImage[] array5 = itemSlot;
				_ = itemTextures;
				_ = array5[1];
			}
			else if (array[2] != 0)
			{
				array2[itemSelected] = item_ID;
				RawImage[] array3 = itemSlot;
				_ = itemTextures;
				_ = array3[itemSelected];
			}
			else
			{
				array[2] = item_ID;
				RawImage[] array4 = itemSlot;
				_ = itemTextures;
				_ = array4[2];
			}
		}
		else
		{
			array[0] = item_ID;
			RawImage[] array6 = itemSlot;
			_ = itemTextures;
			rawImage = array6[0];
		}
		rawImage.texture = NativeIntrinsics.Reinterpret<Texture>(NativeIntrinsics.Asm("*(BinaryExpr + 0x0)"));
		itemText.text = itemNames[item[itemSelected]];
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00247790 File Offset: 0x00245D90
	private unsafe void UseItem()
	{
		if (item[itemSelected] == 0)
		{
			return;
		}
		int[] array = item;
		GameControllerScript gameControllerScript;
		if (item[itemSelected] == 1)
		{
			float stamina = player.maxStamina + player.maxStamina;
			player.stamina = stamina;
			gameControllerScript = this;
		}
		else if (array[itemSelected] == 2)
		{
			Camera main6 = Camera.main;
			int width6 = Screen.width;
			int height6 = Screen.height;
			float x = width6 / 2;
			float y = height6 / 2;
			Vector3 pos6 = new Vector3(x, y, 0f);
			if (Physics.Raycast(main6.ScreenPointToRay(pos6), out var hitInfo6))
			{
				bool flag7 = hitInfo6.collider.tag == "SwingingDoor";
				if (10f < Vector3.Distance(playerTransform.position, hitInfo6.transform.position) || !flag7)
				{
					return;
				}
				hitInfo6.collider.gameObject.GetComponent<SwingingDoorScript>().LockDoor(15f);
				gameControllerScript = this;
			}
			else
			{
				return;
			}
		}
		else if (array[itemSelected] == 3)
		{
			Camera main5 = Camera.main;
			int width5 = Screen.width;
			int height5 = Screen.height;
			float x = width5 / 2;
			float y = height5 / 2;
			Vector3 pos5 = new Vector3(x, y, 0f);
			if (Physics.Raycast(main5.ScreenPointToRay(pos5), out var hitInfo5))
			{
				bool flag6 = hitInfo5.collider.tag == "Door";
				Vector3 position7 = playerTransform.position;
				Transform transform4 = hitInfo5.transform;
				float z = position7.z;
				Vector3 position8 = transform4.position;
				Vector3 a2 = new Vector3(position7.x, position7.y, z);
				if (10f < Vector3.Distance(a2, position8) || !flag6)
				{
					return;
				}
				DoorScript component = hitInfo5.collider.gameObject.GetComponent<DoorScript>();
				if (!component.bDoorLocked)
				{
					return;
				}
				component.bDoorLocked = false;
				component.OpenDoor();
				gameControllerScript = this;
			}
			else
			{
				return;
			}
		}
		else if (array[itemSelected] != 4)
		{
			if (array[itemSelected] != 5)
			{
				float x;
				float y;
				if (array[itemSelected] == 6)
				{
					Camera main3 = Camera.main;
					int width3 = Screen.width;
					int height3 = Screen.height;
					x = width3 / 2;
					y = height3 / 2;
					Vector3 pos3 = new Vector3(x, y, 0f);
					if (!Physics.Raycast(main3.ScreenPointToRay(pos3), out var hitInfo3))
					{
						return;
					}
					bool flag2 = hitInfo3.collider.name == "TapePlayer";
					if (10f < Vector3.Distance(playerTransform.position, hitInfo3.transform.position) || !flag2)
					{
						return;
					}
					gameControllerScript = this;
				}
				else
				{
					if (array[itemSelected] == 7)
					{
						Transform transform2 = playerTransform;
						GameObject gameObject3 = alarmClock;
						Vector3 position3 = transform2.position;
						GameObject gameObject2 = gameObject3;
						Quaternion rotation2 = cameraTransform.rotation;
						float z2 = position3.z;
						GameObject original2 = gameObject2;
						Vector3 position4 = new Vector3(position3.x, position3.y, z2);
						UnityEngine.Object.Instantiate(original2, position4, rotation2).GetComponent<AlarmClockScript>().baldi = baldiScrpt;
						gameControllerScript = this;
					}
					else if (array[itemSelected] != 8)
					{
						if (array[itemSelected] != 9)
						{
							if (array[itemSelected] != 10)
							{
								_ = itemSelected;
								if (array[itemSelected] != 11)
								{
									return;
								}
								gameControllerScript = this;
							}
							else
							{
								player.ActivateBoots();
								gameControllerScript = this;
							}
							<Teleporter>d__20 <Teleporter>d__ = new <Teleporter>d__20(0);
							<Teleporter>d__.<>1__state = 0;
							<Teleporter>d__.<>4__this = gameControllerScript;
							gameControllerScript.StartCoroutine(<Teleporter>d__);
						}
						else
						{
							Camera main2 = Camera.main;
							int width2 = Screen.width;
							int height2 = Screen.height;
							y = height2 / 2;
							_ = height2 / 2;
							Vector3 pos2 = new Vector3(width2 / 2, y, 0f);
							Ray ray = main2.ScreenPointToRay(pos2);
							if (player.jumpRope)
							{
								player.DeactivateJumpRope();
								PlaytimeScript playtimeScript = this.playtimeScript;
								this.playtimeScript.animator.SetBool("disappointed", value: true);
								playtimeScript.audioDevice.Stop();
								playtimeScript.audioDevice.PlayOneShot(playtimeScript.aud_Sad);
								gameControllerScript = this;
							}
							else if (Physics.Raycast(ray, out var hitInfo2) && hitInfo2.collider.name == "1st Prize")
							{
								firstPrizeScript.crazyTime = 15f;
								gameControllerScript = this;
							}
							else
							{
								return;
							}
						}
					}
					else
					{
						Camera main = Camera.main;
						int width = Screen.width;
						int height = Screen.height;
						x = width / 2;
						y = height / 2;
						Vector3 pos = new Vector3(x, y, 0f);
						if (Physics.Raycast(main.ScreenPointToRay(pos), out var hitInfo))
						{
							if ((hitInfo.collider.tag == "Door") & !(10f < Vector3.Distance(playerTransform.position, hitInfo.transform.position)))
							{
								hitInfo.collider.gameObject.GetComponent<DoorScript>().silentOpens = 4;
								ResetItem();
								audioDevice.PlayOneShot(aud_Spray);
							}
						}
						return;
					}
					goto IL_07b1;
				}
			}
			else
			{
				Camera main4 = Camera.main;
				int width4 = Screen.width;
				int height4 = Screen.height;
				float x = width4 / 2;
				float y = height4 / 2;
				Vector3 pos4 = new Vector3(x, y, 0f);
				if (Physics.Raycast(main4.ScreenPointToRay(pos4), out var hitInfo4))
				{
					bool flag3 = hitInfo4.collider.name == "BSODAMachine";
					Vector3 position5 = playerTransform.position;
					Transform transform3 = hitInfo4.transform;
					float z2 = position5.z;
					Vector3 position6 = transform3.position;
					Vector3 a = new Vector3(position5.x, position5.y, z2);
					if (!(10f < Vector3.Distance(a, position6)) && flag3)
					{
						ResetItem();
						CollectItem(4);
						return;
					}
					bool flag4 = hitInfo4.collider.name == "ZestyMachine";
					if (!(10f < Vector3.Distance(playerTransform.position, hitInfo4.transform.position)) && flag4)
					{
						ResetItem();
						CollectItem(1);
						return;
					}
					bool flag5 = hitInfo4.collider.name == "PayPhone";
					if (10f < Vector3.Distance(playerTransform.position, hitInfo4.transform.position) || !flag5)
					{
						return;
					}
					gameControllerScript = this;
				}
				else
				{
					return;
				}
			}
			PlayerScript playerScript = default(PlayerScript);
			((RaycastHit*)playerScript)->collider.gameObject.GetComponent<TapePlayerScript>().Play();
		}
		else
		{
			Transform transform = playerTransform;
			GameObject gameObject = bsodaSpray;
			Vector3 position = transform.position;
			GameObject gameObject2 = gameObject;
			Quaternion rotation = cameraTransform.rotation;
			float z = position.z;
			GameObject original = gameObject2;
			Vector3 position2 = new Vector3(position.x, position.y, z);
			UnityEngine.Object.Instantiate(original, position2, rotation);
			ResetItem();
			player.ResetGuilt("drink", 1f);
			audioDevice.PlayOneShot(aud_Soda);
			return;
		}
		goto IL_07b1;
		IL_07b1:
		gameControllerScript.ResetItem();
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00245DF0 File Offset: 0x002443F0
	private IEnumerator BootAnimation()
	{
		<BootAnimation>d__19 <BootAnimation>d__ = new <BootAnimation>d__19(0);
		<BootAnimation>d__.<>1__state = 0;
		<BootAnimation>d__.<>4__this = this;
		return <BootAnimation>d__;
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00246C50 File Offset: 0x00245250
	private IEnumerator Teleporter()
	{
		// Il2CppStudio: coroutine reconstructed from <Teleporter>d__20.MoveNext (state machine class hidden).
		playerCharacter.enabled = false;
		playerCollider.enabled = false;
		int teleports = UnityEngine.Random.Range(12, 16);
		int teleportCount = 0;
		float baseTime = 0.2f;
		float currentTime = 0.2f;
		float increaseFactor = 1.1f;
		while (teleportCount < teleports)
		{
			float num2 = currentTime;
			float deltaTime = Time.deltaTime;
			currentTime = num2 - deltaTime;
			if (0f > num2 - deltaTime)
			{
				AILocationSelector.GetNewTarget();
				Transform transform = player.transform;
				Vector3 position = AILocationSelector.transform.position;
				float x = player.height * Vector3.upVector.x + position.x;
				float y = player.height * Vector3.upVector.y + position.y;
				Vector3 position2 = new Vector3(x, y, player.height * Vector3.upVector.z + position.z);
				transform.position = position2;
				audioDevice.PlayOneShot(aud_Teleport);
				float num3 = increaseFactor * baseTime;
				teleportCount++;
				baseTime = num3;
				currentTime = num3;
			}
			player.height = (flipped ? 6f : 4f);
			yield return null;
		}
		playerCharacter.enabled = true;
		playerCollider.enabled = true;
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00246B00 File Offset: 0x00245100
	private void Teleport()
	{
		AILocationSelector.GetNewTarget();
		Transform transform = player.transform;
		Vector3 position = AILocationSelector.transform.position;
		Vector3 position2 = new Vector3(player.height * Vector3.upVector.x + position.x, player.height * Vector3.upVector.y + position.y, player.height * Vector3.upVector.z + position.z);
		transform.position = position2;
		audioDevice.PlayOneShot(aud_Teleport);
	}

	// Token: 0x06000033 RID: 51 RVA: 0x002468D0 File Offset: 0x00244ED0
	private void ResetItem()
	{
		item[itemSelected] = 0;
		itemSlot[itemSelected].texture = itemTextures[0];
		itemText.text = itemNames[item[itemSelected]];
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00246770 File Offset: 0x00244D70
	public void LoseItem(int id)
	{
		item[id] = 0;
		itemSlot[id].texture = itemTextures[0];
		itemText.text = itemNames[item[itemSelected]];
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00246DA0 File Offset: 0x002453A0
	private void UpdateItemName()
	{
		itemText.text = itemNames[item[itemSelected]];
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00246380 File Offset: 0x00244980
	public void ExitReached()
	{
		int num = exitsReached;
		exitsReached = (int)(exitsReached + 1L);
		if (num == 0)
		{
			RenderSettings.ambientLight = Color.red;
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
		CameraScript component = Camera.main.GetComponent<CameraScript>();
		component.offset = Vector3.down;
	}
}

