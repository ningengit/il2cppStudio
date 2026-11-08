using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000019 RID: 25
public class ControlsMenu : MonoBehaviour
{
	// Token: 0x0200001A RID: 26
	private sealed class _RebindCoroutine_d__15 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x040000C5 RID: 197 Offset: 0x10
		// Il2CppStudio: declared private; widened because native code inlined into other types accesses it.
		internal int __1__state;

		// Token: 0x040000C6 RID: 198 Offset: 0x18
		private object __2__current;

		// Token: 0x040000C7 RID: 199 Offset: 0x20
		public ControlsMenu __4__this;

		// Token: 0x040000C8 RID: 200 Offset: 0x28
		public float duration;

		// Token: 0x040000C9 RID: 201 Offset: 0x30
		private string _actionName_5__2;

		// Token: 0x040000CA RID: 202 Offset: 0x38
		private float _remainingTime_5__3;

		object System.Collections.Generic.IEnumerator<System.Object>.Current => __2__current;

		object System.Collections.IEnumerator.Current => __2__current;

		// Token: 0x06000092 RID: 146 RVA: 0x00250B50 File Offset: 0x0024F150
		public _RebindCoroutine_d__15(int __1__state)
		{
			this.__1__state = __1__state;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00250B40 File Offset: 0x0024F140
		void System.IDisposable.Dispose()
		{
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00251050 File Offset: 0x0024F650
		unsafe bool System.Collections.IEnumerator.MoveNext()
		{
			int num = __1__state;
			ControlsMenu controlsMenu = __4__this;
			num = num - 1 - 1;
			if (num != 0 && num != 1)
			{
				return (byte)NativeIntrinsics.CallNative("sub_251368", 0L, 0L, 0L, 0L) != 0;
			}
			float num2 = _remainingTime_5__3;
			__1__state = -1;
			float f = num2;
			TMP_Text counterText = controlsMenu.counterText;
			string text = _actionName_5__2;
			counterText.text = $"<size=16>{text}<size=21>\nPress a key to assign it to {text}\n\n\n\n\n\n{Mathf.CeilToInt(f)}";
			int num4 = default(int);
			_RebindCoroutine_d__15 _RebindCoroutine_d__ = default(_RebindCoroutine_d__15);
			long num5 = default(long);
			if (!controlsMenu.isBindingMouseInput)
			{
				_ = Input.anyKeyDown;
				KeyCode[] allKeyCodes = controlsMenu.allKeyCodes;
				int num3 = 0;
				num4 = 0;
				_RebindCoroutine_d__ = this;
				while (true)
				{
					KeyCode keyCode = allKeyCodes[num3];
					bool keyDownInt = Input.GetKeyDownInt(keyCode);
					if (keyDownInt && (uint)((long)keyCode + -323L) > 4)
					{
						break;
					}
					num3++;
				}
			}
			else
			{
				int num3 = 0;
				num4 = 0;
				_RebindCoroutine_d__ = this;
				while (!Input.GetMouseButtonDown(num3))
				{
					num3++;
					if (num3 + 1 >= 7)
					{
						return (byte)NativeIntrinsics.CallNative("sub_25132C", 0L, 0L, 0L, 0L) != 0;
					}
				}
				num5 = num3 + 323L;
			}
			int key = default(int);
			if ((controlsMenu.isBindingMouseInput ? 1 : 0) != num4)
			{
				if (!((uint)(num5 + -323L) <= 4))
				{
					if (NativeIntrinsics.Asm("*(LocalExpr + 0x80)") != num4)
					{
						goto IL_0377;
					}
				}
				else
				{
					InputManager inputMan = controlsMenu.inputMan;
					key = controlsMenu.currentActionIndex;
					if (!inputMan.KeyboardMapping.TryGetValue((InputAction)key, out *(InputBinding*)NativeIntrinsics.Asm("&stack[0x8]")))
					{
					}
					if (((int)num5 & (int)num5) == 0)
					{
						NativeIntrinsics.Asm("stack[0xC]");
					}
					_ = inputMan.KeyboardMapping;
					goto IL_034f;
				}
			}
			long num6 = default(long);
			if ((uint)(num5 + -323L) > 4)
			{
				NativeIntrinsics.Asm("*(LocalExpr + 0x88)");
				key = (int)NativeIntrinsics.Asm("*(LocalExpr + 0x68)");
				if (!((Dictionary<InputAction, InputBinding>)(object)NativeIntrinsics.Reinterpret<Dictionary<object, object>>(NativeIntrinsics.Asm("*(LocalExpr + 0x20)"))).TryGetValue((InputAction)key, out *(InputBinding*)NativeIntrinsics.Asm("&stack[0x8]")))
				{
				}
				if (((int)num5 & (int)num5) == 0)
				{
					NativeIntrinsics.Asm("stack[0x8]");
				}
				num6 = NativeIntrinsics.Asm("*(LocalExpr + 0x20)");
				goto IL_034f;
			}
			goto IL_0377;
			IL_0377:
			_RebindCoroutine_d__._remainingTime_5__3 = 0.01f;
			return false;
			IL_034f:
			((Dictionary<InputAction, InputBinding>)(object)NativeIntrinsics.Reinterpret<Dictionary<object, object>>(num6))[(InputAction)key] = NativeIntrinsics.Reinterpret<InputBinding>(NativeIntrinsics.Asm("stack[0x8]"));
			goto IL_0377;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00251510 File Offset: 0x0024FB10
		void System.Collections.IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	// Token: 0x040000B7 RID: 183 Offset: 0x20
	private Button[] allButtons;

	// Token: 0x040000B8 RID: 184 Offset: 0x28
	private TMP_Text[] buttonTexts;

	// Token: 0x040000B9 RID: 185 Offset: 0x30
	private TMP_Text[] secondaryButtonTexts;

	// Token: 0x040000BA RID: 186 Offset: 0x38
	private TMP_Text[] labelTexts;

	// Token: 0x040000BB RID: 187 Offset: 0x40
	private GameObject panel;

	// Token: 0x040000BC RID: 188 Offset: 0x48
	private GameObject confirmationPanel;

	// Token: 0x040000BD RID: 189 Offset: 0x50
	private GameObject resetConfirmationPanel;

	// Token: 0x040000BE RID: 190 Offset: 0x58
	private TMP_Text counterText;

	// Token: 0x040000BF RID: 191 Offset: 0x60
	private TMP_Text confirmText;

	// Token: 0x040000C0 RID: 192 Offset: 0x68
	private int currentActionIndex;

	// Token: 0x040000C1 RID: 193 Offset: 0x70
	private Coroutine rebindCoroutine;

	// Token: 0x040000C2 RID: 194 Offset: 0x78
	private KeyCode[] allKeyCodes;

	// Token: 0x040000C3 RID: 195 Offset: 0x80
	private bool isBindingMouseInput;

	// Token: 0x040000C4 RID: 196 Offset: 0x88
	private InputManager inputMan;

	// Token: 0x0600007D RID: 125 RVA: 0x00241660 File Offset: 0x0023FC60
	private void Awake()
	{
		Array values = Enum.GetValues(Type.GetTypeFromHandle(NativeIntrinsics.Reinterpret<RuntimeTypeHandle>((nint)NativeIntrinsics.Asm("typeof(KeyCode)"))));
		Array array = values;
		if (values == null)
		{
			allKeyCodes = (KeyCode[])values;
		}
		else
		{
			allKeyCodes = array as KeyCode[];
		}
	}

	// Token: 0x0600007E RID: 126 RVA: 0x002427C0 File Offset: 0x00240DC0
	private void Start()
	{
		inputMan = Singleton<InputManager>.Instance;
		if (inputMan != null)
		{
			UpdateAllButtonTexts();
		}
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00241E40 File Offset: 0x00240440
	private void OnEnable()
	{
		if (inputMan == null)
		{
			inputMan = Singleton<InputManager>.Instance;
		}
		if (!(inputMan == null))
		{
			Debug.Log($"ControlsMenu: InputManager found. Mappings count: {inputMan.KeyboardMapping.Count}");
			UpdateAllButtonTexts();
		}
		else
		{
			Debug.LogError("ControlsMenu: InputManager not found! Is it in the scene?");
		}
	}

	// Token: 0x06000080 RID: 128 RVA: 0x00241E10 File Offset: 0x00240410
	private void OnDisable()
	{
		inputMan.Save();
	}

	// Token: 0x06000081 RID: 129 RVA: 0x00242860 File Offset: 0x00240E60
	public unsafe void UpdateAllButtonTexts()
	{
		TMP_Text[] array = labelTexts;
		int num = 0;
		int num2 = 0;
		while (num < array.Length)
		{
			if (num2 < ((System.Collections.ICollection)Enum.GetValues(Type.GetTypeFromHandle(NativeIntrinsics.Reinterpret<RuntimeTypeHandle>((nint)NativeIntrinsics.Asm("typeof(InputAction)"))))).Count && inputMan.KeyboardMapping.TryGetValue((InputAction)num2, out *(InputBinding*)NativeIntrinsics.Asm("&stack[0x8]")))
			{
				TMP_Text[] array2 = buttonTexts;
				ControlsMenu controlsMenu = this;
				long num3 = NativeIntrinsics.Asm("stack[0x8]");
				array2[num2].text = (((uint)(NativeIntrinsics.Asm("stack[0x8]") + -323L) <= 4) ? controlsMenu.FormatMouseText((KeyCode)num3) : controlsMenu.FormatKeyText((KeyCode)num3));
				TMP_Text[] array3 = secondaryButtonTexts;
				ControlsMenu controlsMenu2 = this;
				int num4 = (int)NativeIntrinsics.Asm("stack[0xC]");
				array3[num2].text = (((uint)((int)NativeIntrinsics.Asm("stack[0xC]") + -323L) <= 4) ? controlsMenu2.FormatMouseText((KeyCode)num4) : controlsMenu2.FormatKeyText((KeyCode)num4));
			}
			_ = labelTexts;
			num2++;
		}
	}

	// Token: 0x06000082 RID: 130 RVA: 0x00241AA0 File Offset: 0x002400A0
	private string FormatKeyText(KeyCode key)
	{
		switch (key)
		{
		case KeyCode.None:
			return "";
		case KeyCode.Escape:
			return "ESC";
		default:
			if ((uint)((long)key + -48L) <= 9)
			{
				return "0";
			}
			return NativeIntrinsics.Reinterpret<Enum>((nint)NativeIntrinsics.Asm("&stack[0x338]")).ToString();
		}
	}

	// Token: 0x06000083 RID: 131 RVA: 0x00241C80 File Offset: 0x00240280
	private string FormatMouseText(KeyCode mouseKey)
	{
		long num = (long)mouseKey + -323L;
		if (((int)((long)mouseKey + -323L) & (int)((long)mouseKey + -323L)) != 0)
		{
			return ((int)num - 1) switch
			{
				1 => "Middle Mouse Button", 
				0 => "Right Mouse Button", 
				_ => NativeIntrinsics.Reinterpret<Enum>((nint)NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFD8]")).ToString(), 
			};
		}
		return "Left Mouse Button";
	}

	// Token: 0x06000084 RID: 132 RVA: 0x00242400 File Offset: 0x00240A00
	public void StartRebindKeyboard(int actionIndex)
	{
		if (!panel.activeSelf && !confirmationPanel.activeSelf && actionIndex < ((System.Collections.ICollection)Enum.GetValues(Type.GetTypeFromHandle(NativeIntrinsics.Reinterpret<RuntimeTypeHandle>((nint)NativeIntrinsics.Asm("typeof(InputAction)"))))).Count)
		{
			GameObject gameObject = confirmationPanel;
			currentActionIndex = actionIndex;
			isBindingMouseInput = false;
			gameObject.SetActive(value: true);
			SetButtonsInteractable(state: false);
			confirmText.text = labelTexts[(int)currentActionIndex].text + "\n\n" + buttonTexts[currentActionIndex].text;
		}
	}

	// Token: 0x06000085 RID: 133 RVA: 0x002425E0 File Offset: 0x00240BE0
	public void StartRebindMouse(int actionIndex)
	{
		if (!panel.activeSelf && !confirmationPanel.activeSelf && actionIndex < ((System.Collections.ICollection)Enum.GetValues(Type.GetTypeFromHandle(NativeIntrinsics.Reinterpret<RuntimeTypeHandle>((nint)NativeIntrinsics.Asm("typeof(InputAction)"))))).Count)
		{
			GameObject gameObject = confirmationPanel;
			currentActionIndex = actionIndex;
			isBindingMouseInput = true;
			gameObject.SetActive(value: true);
			SetButtonsInteractable(state: false);
			confirmText.text = labelTexts[(int)currentActionIndex].text + "\n\n" + secondaryButtonTexts[currentActionIndex].text;
		}
	}

	// Token: 0x06000086 RID: 134 RVA: 0x00242390 File Offset: 0x00240990
	public void ShowResetConfirmation()
	{
		resetConfirmationPanel.SetActive(value: true);
		Button[] array = allButtons;
		int num = 0;
		int num2 = 0;
		while (num < array.Length)
		{
			array[num2].interactable = false;
			num2++;
		}
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00242070 File Offset: 0x00240670
	public unsafe void ResetKey()
	{
		InputManager inputManager = inputMan;
		int key = currentActionIndex;
		Dictionary<InputAction, InputBinding> keyboardMapping = default(Dictionary<InputAction, InputBinding>);
		if (isBindingMouseInput)
		{
			if (inputManager.KeyboardMapping.TryGetValue((InputAction)key, out *(InputBinding*)NativeIntrinsics.Asm("&stack[0x8]")))
			{
				keyboardMapping = inputManager.KeyboardMapping;
				goto IL_009c;
			}
		}
		else if (inputManager.KeyboardMapping.TryGetValue((InputAction)key, out *(InputBinding*)NativeIntrinsics.Asm("&stack[0x8]")))
		{
			keyboardMapping = inputManager.KeyboardMapping;
			goto IL_009c;
		}
		goto IL_00bd;
		IL_009c:
		keyboardMapping[(InputAction)key] = NativeIntrinsics.Reinterpret<InputBinding>(NativeIntrinsics.Asm("stack[0x8]"));
		goto IL_00bd;
		IL_00bd:
		inputMan.Save();
		UpdateAllButtonTexts();
		confirmationPanel.SetActive(value: false);
		if (rebindCoroutine != null)
		{
			StopCoroutine(rebindCoroutine);
		}
		Button[] array = allButtons;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].interactable = true;
		}
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00241870 File Offset: 0x0023FE70
	public void ConfirmRebind()
	{
		confirmationPanel.SetActive(value: false);
		panel.SetActive(value: true);
		if (rebindCoroutine != null)
		{
			StopCoroutine(rebindCoroutine);
		}
		_RebindCoroutine_d__15 _RebindCoroutine_d__ = new _RebindCoroutine_d__15(0);
		_RebindCoroutine_d__.__1__state = 0;
		_RebindCoroutine_d__.__4__this = this;
		_RebindCoroutine_d__.duration = 5f;
		rebindCoroutine = StartCoroutine(_RebindCoroutine_d__);
	}

	// Token: 0x06000089 RID: 137 RVA: 0x00241950 File Offset: 0x0023FF50
	public void ConfirmReset()
	{
		resetConfirmationPanel.SetActive(value: false);
		Button[] array = allButtons;
		int num = 0;
		int num2 = 0;
		while (num < array.Length)
		{
			array[num2].interactable = true;
			num2++;
		}
		InputManager inputManager = inputMan;
		inputManager.KeyboardMapping.Clear();
		num2 = 0;
		do
		{
			inputManager.KeyboardMapping[(InputAction)num2] = inputManager.GetDefaultBinding((InputAction)num2);
			num2++;
		}
		while (num2 + 1 < 13);
		inputManager.Save();
		UpdateAllButtonTexts();
		Button[] array2 = allButtons;
		int num3 = 0;
		num2 = 0;
		while (num3 < array2.Length)
		{
			array2[num2].interactable = true;
			num2++;
		}
	}

	// Token: 0x0600008A RID: 138 RVA: 0x00241770 File Offset: 0x0023FD70
	public void CancelRebind()
	{
		confirmationPanel.SetActive(value: false);
		if (rebindCoroutine != null)
		{
			StopCoroutine(rebindCoroutine);
		}
		Button[] array = allButtons;
		int num = 0;
		int num2 = 0;
		while (num < array.Length)
		{
			array[num2].interactable = true;
			num2++;
		}
	}

	// Token: 0x0600008B RID: 139 RVA: 0x00241800 File Offset: 0x0023FE00
	public void CancelReset()
	{
		resetConfirmationPanel.SetActive(value: false);
		Button[] array = allButtons;
		int num = 0;
		int num2 = 0;
		while (num < array.Length)
		{
			array[num2].interactable = true;
			num2++;
		}
	}

	// Token: 0x0600008C RID: 140 RVA: 0x00241FF0 File Offset: 0x002405F0
	private IEnumerator RebindCoroutine(float duration)
	{
		_RebindCoroutine_d__15 _RebindCoroutine_d__ = new _RebindCoroutine_d__15(0);
		_RebindCoroutine_d__.__1__state = 0;
		_RebindCoroutine_d__.__4__this = this;
		_RebindCoroutine_d__.duration = duration;
		return _RebindCoroutine_d__;
	}

	// Token: 0x0600008D RID: 141 RVA: 0x00241D30 File Offset: 0x00240330
	private bool GetPressedInput(out KeyCode pressedKey)
	{
		pressedKey = default(KeyCode);
		NativeIntrinsics.Stmt("mov dword ptr [rdx],0 ; value = 0x0");
		if (isBindingMouseInput)
		{
			int num2 = 0;
			do
			{
				if (!Input.GetMouseButtonDown(num2))
				{
					num2++;
					continue;
				}
				NativeIntrinsics.Stmt("mov [r14],ebx ; value = BinaryExpr");
				return true;
			}
			while (num2 + 1 < 7);
		}
		else if (Input.anyKeyDown)
		{
			KeyCode[] array = allKeyCodes;
			int num = 0;
			int num2 = 0;
			while (num < array.Length)
			{
				KeyCode keyCode = array[num2];
				bool keyDownInt = Input.GetKeyDownInt(keyCode);
				if (!keyDownInt || (uint)((long)keyCode + -323L) <= 4)
				{
					num = num2 + 1;
					num2++;
					continue;
				}
				NativeIntrinsics.Stmt("mov [r14],esi ; value = LocalExpr");
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600008E RID: 142 RVA: 0x00241DF0 File Offset: 0x002403F0
	private bool IsMouseKey(KeyCode key)
	{
		if ((uint)((long)key + -323L) <= 3)
		{
			return true;
		}
		return key == KeyCode.Mouse4;
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00242320 File Offset: 0x00240920
	private void SetButtonsInteractable(bool state)
	{
		Button[] array = allButtons;
		int num = 0;
		int num2 = 0;
		while (num < array.Length)
		{
			array[num2].interactable = state;
			num2++;
		}
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00242220 File Offset: 0x00240820
	public void ResetToDefaults()
	{
		Button[] array = allButtons;
		int num = 0;
		int num2 = 0;
		while (num < array.Length)
		{
			array[num2].interactable = true;
			num2++;
		}
		InputManager inputManager = inputMan;
		inputManager.KeyboardMapping.Clear();
		num2 = 0;
		do
		{
			inputManager.KeyboardMapping[(InputAction)num2] = inputManager.GetDefaultBinding((InputAction)num2);
			num2++;
		}
		while (num2 + 1 < 13);
		inputManager.Save();
		UpdateAllButtonTexts();
	}
}

