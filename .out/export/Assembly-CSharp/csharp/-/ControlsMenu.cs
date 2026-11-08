using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppStudio.Native;
using Mono.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000019 RID: 25
public class ControlsMenu : MonoBehaviour
{
	// Token: 0x0200001A RID: 26
	private sealed class <RebindCoroutine>d__15 : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x040000C5 RID: 197 Offset: 0x10
		private int <>1__state;

		// Token: 0x040000C6 RID: 198 Offset: 0x18
		private object <>2__current;

		// Token: 0x040000C7 RID: 199 Offset: 0x20
		public ControlsMenu <>4__this;

		// Token: 0x040000C8 RID: 200 Offset: 0x28
		public float duration;

		// Token: 0x040000C9 RID: 201 Offset: 0x30
		private string <actionName>5__2;

		// Token: 0x040000CA RID: 202 Offset: 0x38
		private float <remainingTime>5__3;

		private object System.Collections.Generic.IEnumerator<System.Object>.Current => <>2__current;

		private object System.Collections.IEnumerator.Current => <>2__current;

		// Token: 0x06000092 RID: 146 RVA: 0x00250B50 File Offset: 0x0024F150
		public <RebindCoroutine>d__15(int <>1__state)
		{
			((Mono.Xml.SecurityParser)(object)this).OnStartParsing((Mono.Xml.SmallXmlParser)null);
			this.<>1__state = <>1__state;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00250B40 File Offset: 0x0024F140
		private void System.IDisposable.Dispose()
		{
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00251050 File Offset: 0x0024F650
		private unsafe bool MoveNext()
		{
			int num = <>1__state;
			ControlsMenu controlsMenu = <>4__this;
			num--;
			num--;
			if (num - 1 != 0 && num != 1)
			{
				long num2 = NativeIntrinsics.CallNative("sub_251368", 0L, 0L, 0L, 0L);
				return (byte)num2 != 0;
			}
			float num3 = <remainingTime>5__3;
			<>1__state = -1;
			float f = num3;
			TMP_Text counterText = controlsMenu.counterText;
			string text = <actionName>5__2;
			int num4 = Mathf.CeilToInt(f);
			object arg = num4;
			string text2 = $"<size=16>{text}<size=21>\nPress a key to assign it to {text}\n\n\n\n\n\n{arg}";
			TMP_Text tMP_Text = counterText;
			string text3 = text2;
			string text4 = text3;
			long num8 = default(long);
			if (!controlsMenu.isBindingMouseInput)
			{
				bool anyKeyDown = Input.anyKeyDown;
				KeyCode[] allKeyCodes = controlsMenu.allKeyCodes;
				while (true)
				{
					KeyCode keyCode = allKeyCodes[0];
					bool keyDownInt = Input.GetKeyDownInt(NativeIntrinsics.Reinterpret<KeyCode>(NativeIntrinsics.Reinterpret<int>(allKeyCodes[0])));
					KeyCode keyCode2 = keyCode;
					if (keyDownInt && (uint)(NativeIntrinsics.Reinterpret<long>(keyCode2) + -323L) > 4)
					{
						break;
					}
					int num5 = 0 + 1;
					int num6 = 0 + 1;
				}
			}
			else
			{
				int num6 = default(int);
				while (!Input.GetMouseButtonDown(0))
				{
					num6 = 0 + 1;
					if (0 + 1 >= 7)
					{
						long num7 = NativeIntrinsics.CallNative("sub_25132C", 0L, 0L, 0L, 0L);
						return (byte)num7 != 0;
					}
				}
				num8 = num6 + 323L;
			}
			int num9;
			if (controlsMenu.isBindingMouseInput)
			{
				if ((uint)(num8 + -323L) <= 4)
				{
					InputManager inputMan = controlsMenu.inputMan;
					num9 = controlsMenu.currentActionIndex;
					if (NativeIntrinsics.Asm("global_19E3B38") == 0)
					{
						NativeIntrinsics.Stmt("mov byte ptr [1819E3B38h],1 ; value = 0x1");
					}
					Dictionary<InputAction, InputBinding> keyboardMapping = inputMan.KeyboardMapping;
					if (!keyboardMapping.TryGetValue(NativeIntrinsics.Reinterpret<InputAction>(num9), out *(InputBinding*)NativeIntrinsics.Asm("&stack[0x8]")))
					{
					}
					if (((int)num8 & (int)num8) != 0)
					{
						int num10 = (int)num8;
					}
					else
					{
						int num10 = (int)NativeIntrinsics.Asm("stack[0xC]");
					}
					keyboardMapping = inputMan.KeyboardMapping;
					goto IL_033c;
				}
				if (NativeIntrinsics.Asm("*(LocalExpr + 0x80)") != 0)
				{
					goto IL_036e;
				}
			}
			long num12 = default(long);
			if ((uint)(num8 + -323L) > 4)
			{
				long num11 = NativeIntrinsics.Asm("*(LocalExpr + 0x88)");
				num9 = (int)NativeIntrinsics.Asm("*(LocalExpr + 0x68)");
				if (NativeIntrinsics.Asm("global_19E3B38") == 0)
				{
					NativeIntrinsics.Stmt("mov byte ptr [1819E3B38h],1 ; value = 0x1");
				}
				num12 = NativeIntrinsics.Asm("*(LocalExpr + 0x20)");
				if (!((Dictionary<InputAction, InputBinding>)(object)NativeIntrinsics.Reinterpret<Dictionary<, >>(num12)).TryGetValue(NativeIntrinsics.Reinterpret<InputAction>(num9), out *(InputBinding*)NativeIntrinsics.Asm("&stack[0x8]")))
				{
				}
				if (((int)num8 & (int)num8) != 0)
				{
					int num13 = (int)num8;
				}
				else
				{
					int num13 = (int)NativeIntrinsics.Asm("stack[0x8]");
				}
				num12 = NativeIntrinsics.Asm("*(LocalExpr + 0x20)");
				goto IL_033c;
			}
			goto IL_036e;
			IL_033c:
			((Dictionary<InputAction, InputBinding>)(object)NativeIntrinsics.Reinterpret<Dictionary<, >>(num12))[NativeIntrinsics.Reinterpret<InputAction>(num9)] = NativeIntrinsics.Reinterpret<InputBinding>(NativeIntrinsics.Asm("stack[0x8]"));
			goto IL_036e;
			IL_036e:
			<remainingTime>5__3 = 0.01f;
			return false;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00251510 File Offset: 0x0024FB10
		private void System.Collections.IEnumerator.Reset()
		{
			long num = NativeIntrinsics.CallNative("sub_184BE0", NativeIntrinsics.Asm("rax"), 0L, 0L, 0L);
			NativeIntrinsics.Reinterpret<NotSupportedException>(num)..ctor();
			throw null;
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
		Type typeFromHandle = Type.GetTypeFromHandle(NativeIntrinsics.Reinterpret<RuntimeTypeHandle>((nint)NativeIntrinsics.Asm("typeof(KeyCode)")));
		Type enumType = typeFromHandle;
		Array values = Enum.GetValues(enumType);
		Array array = values;
		Array array2 = values;
		if (values == null)
		{
			allKeyCodes = (KeyCode[])array2;
			return;
		}
		KeyCode[] array3 = array as KeyCode[];
		Array array4 = array;
		KeyCode[] array5 = array3;
		allKeyCodes = array5;
		KeyCode[] array6 = array4 as KeyCode[];
		array5 = array6;
	}

	// Token: 0x0600007E RID: 126 RVA: 0x002427C0 File Offset: 0x00240DC0
	private void Start()
	{
		InputManager instance = Singleton<InputManager>.Instance;
		inputMan = instance;
		InputManager inputManager = inputMan;
		bool flag = inputManager != null;
		bool flag2 = flag;
		if (flag)
		{
			UpdateAllButtonTexts();
		}
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00241E40 File Offset: 0x00240440
	private void OnEnable()
	{
		InputManager inputManager = inputMan;
		if (inputManager == null)
		{
			InputManager instance = Singleton<InputManager>.Instance;
			inputMan = instance;
		}
		inputManager = inputMan;
		if (!(inputManager == null))
		{
			Dictionary<InputAction, InputBinding> keyboardMapping = inputMan.KeyboardMapping;
			int count = keyboardMapping.Count;
			object arg = count;
			string text = $"ControlsMenu: InputManager found. Mappings count: {arg}";
			string message = text;
			Debug.Log(message);
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
		while (0 < array.Length)
		{
			Type typeFromHandle = Type.GetTypeFromHandle(NativeIntrinsics.Reinterpret<RuntimeTypeHandle>((nint)NativeIntrinsics.Asm("typeof(InputAction)")));
			Type enumType = typeFromHandle;
			Array values = Enum.GetValues(enumType);
			int num = values.System.Collections.ICollection.Count;
			if (0 < num && inputMan.KeyboardMapping.TryGetValue(NativeIntrinsics.Reinterpret<InputAction>(0), out *(InputBinding*)NativeIntrinsics.Asm("&stack[0x8]")))
			{
				TMP_Text[] array2 = buttonTexts;
				long num2 = NativeIntrinsics.Asm("stack[0x8]");
				TMP_Text tMP_Text = array2[0];
				string text2;
				if ((uint)(NativeIntrinsics.Asm("stack[0x8]") + -323L) > 4)
				{
					string text = FormatKeyText(NativeIntrinsics.Reinterpret<KeyCode>(num2));
					text2 = text;
				}
				else
				{
					string text3 = FormatMouseText(NativeIntrinsics.Reinterpret<KeyCode>(num2));
					text2 = text3;
				}
				string text4 = text2;
				array2 = secondaryButtonTexts;
				int num3 = (int)NativeIntrinsics.Asm("stack[0xC]");
				tMP_Text = array2[0];
				if ((uint)((int)NativeIntrinsics.Asm("stack[0xC]") + -323L) > 4)
				{
					string text5 = FormatKeyText(NativeIntrinsics.Reinterpret<KeyCode>(num3));
					text2 = text5;
				}
				else
				{
					string text6 = FormatMouseText(NativeIntrinsics.Reinterpret<KeyCode>(num3));
					text2 = text6;
				}
				string text7 = text2;
			}
			int num4 = 0 + 1;
			TMP_Text[] array3 = labelTexts;
			int num5 = 0 + 1;
		}
	}

	// Token: 0x06000082 RID: 130 RVA: 0x00241AA0 File Offset: 0x002400A0
	private string FormatKeyText(KeyCode key)
	{
		int num = NativeIntrinsics.Reinterpret<int>(key);
		if (NativeIntrinsics.Reinterpret<int>(key) == 0)
		{
			return "";
		}
		if (num == 27)
		{
			return "ESC";
		}
		if ((uint)(num + -48L) <= 9)
		{
			return "0";
		}
		return NativeIntrinsics.Reinterpret<Enum>((nint)NativeIntrinsics.Asm("&stack[0x338]")).ToString();
	}

	// Token: 0x06000083 RID: 131 RVA: 0x00241C80 File Offset: 0x00240280
	private string FormatMouseText(KeyCode mouseKey)
	{
		long num = NativeIntrinsics.Reinterpret<int>(mouseKey) + -323L;
		int num2 = NativeIntrinsics.Reinterpret<int>(mouseKey);
		if (((int)(NativeIntrinsics.Reinterpret<int>(mouseKey) + -323L) & (int)(NativeIntrinsics.Reinterpret<int>(mouseKey) + -323L)) != 0)
		{
			int num3 = (int)num - 1;
			if ((int)num - 1 != 0)
			{
				if (num3 != 1)
				{
					return NativeIntrinsics.Reinterpret<Enum>((nint)NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFD8]")).ToString();
				}
				return "Middle Mouse Button";
			}
			return "Right Mouse Button";
		}
		return "Left Mouse Button";
	}

	// Token: 0x06000084 RID: 132 RVA: 0x00242400 File Offset: 0x00240A00
	public void StartRebindKeyboard(int actionIndex)
	{
		bool activeSelf = panel.activeSelf;
		bool flag = activeSelf;
		if (activeSelf)
		{
			return;
		}
		bool activeSelf2 = confirmationPanel.activeSelf;
		flag = activeSelf2;
		if (!activeSelf2)
		{
			Type typeFromHandle = Type.GetTypeFromHandle(NativeIntrinsics.Reinterpret<RuntimeTypeHandle>((nint)NativeIntrinsics.Asm("typeof(InputAction)")));
			Type enumType = typeFromHandle;
			Array values = Enum.GetValues(enumType);
			Array array = values;
			int num = array.System.Collections.ICollection.Count;
			int num2 = num;
			if (actionIndex < num)
			{
				GameObject gameObject = confirmationPanel;
				currentActionIndex = actionIndex;
				isBindingMouseInput = false;
				gameObject.SetActive(value: true);
				SetButtonsInteractable(state: false);
				long num3 = currentActionIndex;
				TMP_Text tMP_Text = confirmText;
				string text = NativeIntrinsics.Reinterpret<string>((nint)NativeIntrinsics.Asm("ArrayElementExpr->klass")) + "\n\n" + NativeIntrinsics.Reinterpret<string>((nint)NativeIntrinsics.Asm("ArrayElementExpr->klass"));
				string text2 = text;
			}
		}
	}

	// Token: 0x06000085 RID: 133 RVA: 0x002425E0 File Offset: 0x00240BE0
	public void StartRebindMouse(int actionIndex)
	{
		bool activeSelf = panel.activeSelf;
		bool flag = activeSelf;
		if (activeSelf)
		{
			return;
		}
		bool activeSelf2 = confirmationPanel.activeSelf;
		flag = activeSelf2;
		if (!activeSelf2)
		{
			Type typeFromHandle = Type.GetTypeFromHandle(NativeIntrinsics.Reinterpret<RuntimeTypeHandle>((nint)NativeIntrinsics.Asm("typeof(InputAction)")));
			Type enumType = typeFromHandle;
			Array values = Enum.GetValues(enumType);
			Array array = values;
			int num = array.System.Collections.ICollection.Count;
			int num2 = num;
			if (actionIndex < num)
			{
				GameObject gameObject = confirmationPanel;
				currentActionIndex = actionIndex;
				isBindingMouseInput = true;
				gameObject.SetActive(value: true);
				SetButtonsInteractable(state: false);
				long num3 = currentActionIndex;
				TMP_Text tMP_Text = confirmText;
				string text = NativeIntrinsics.Reinterpret<string>((nint)NativeIntrinsics.Asm("ArrayElementExpr->klass")) + "\n\n" + NativeIntrinsics.Reinterpret<string>((nint)NativeIntrinsics.Asm("ArrayElementExpr->klass"));
				string text2 = text;
			}
		}
	}

	// Token: 0x06000086 RID: 134 RVA: 0x00242390 File Offset: 0x00240990
	public void ShowResetConfirmation()
	{
		resetConfirmationPanel.SetActive(value: true);
		Button[] array = allButtons;
		while (0 < array.Length)
		{
			long num = 0L;
			array[0].interactable = false;
			int num2 = 0 + 1;
			int num3 = 0 + 1;
		}
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00242070 File Offset: 0x00240670
	public unsafe void ResetKey()
	{
		InputManager inputManager = inputMan;
		int num = currentActionIndex;
		Dictionary<InputAction, InputBinding> keyboardMapping;
		if (isBindingMouseInput)
		{
			if (inputManager.KeyboardMapping.TryGetValue(NativeIntrinsics.Reinterpret<InputAction>(num), out *(InputBinding*)NativeIntrinsics.Asm("&stack[0x8]")))
			{
				keyboardMapping = inputManager.KeyboardMapping;
				goto IL_007e;
			}
		}
		else if (inputManager.KeyboardMapping.TryGetValue(NativeIntrinsics.Reinterpret<InputAction>(num), out *(InputBinding*)NativeIntrinsics.Asm("&stack[0x8]")))
		{
			keyboardMapping = inputManager.KeyboardMapping;
			goto IL_007e;
		}
		goto IL_00a4;
		IL_007e:
		keyboardMapping[NativeIntrinsics.Reinterpret<InputAction>(num)] = NativeIntrinsics.Reinterpret<InputBinding>(NativeIntrinsics.Asm("stack[0x8]"));
		goto IL_00a4;
		IL_00a4:
		InputManager inputManager2 = inputMan;
		inputManager2.Save();
		UpdateAllButtonTexts();
		GameObject gameObject = confirmationPanel;
		gameObject.SetActive(value: false);
		if (rebindCoroutine != null)
		{
			StopCoroutine(rebindCoroutine);
		}
		Button[] array = allButtons;
		while (0 < array.Length)
		{
			array[0].interactable = true;
			int num2 = 0 + 1;
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
		<RebindCoroutine>d__15 <RebindCoroutine>d__ = NativeIntrinsics.Reinterpret<<RebindCoroutine>d__15>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(ControlsMenu+<RebindCoroutine>d__15)"));
		((Mono.Xml.SecurityParser)(object)<RebindCoroutine>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<RebindCoroutine>d__.<>1__state = 0;
		<RebindCoroutine>d__.<>4__this = this;
		<RebindCoroutine>d__.duration = 5f;
		Coroutine coroutine = StartCoroutine(<RebindCoroutine>d__);
		rebindCoroutine = coroutine;
	}

	// Token: 0x06000089 RID: 137 RVA: 0x00241950 File Offset: 0x0023FF50
	public void ConfirmReset()
	{
		resetConfirmationPanel.SetActive(value: false);
		Button[] array = allButtons;
		while (0 < array.Length)
		{
			long num = 0L;
			array[0].interactable = true;
			int num2 = 0 + 1;
			int num3 = 0 + 1;
		}
		InputManager inputManager = inputMan;
		inputManager.KeyboardMapping.Clear();
		do
		{
			Dictionary<InputAction, InputBinding> keyboardMapping = inputManager.KeyboardMapping;
			InputBinding defaultBinding = inputManager.GetDefaultBinding(NativeIntrinsics.Reinterpret<InputAction>(0));
			keyboardMapping[NativeIntrinsics.Reinterpret<InputAction>(0)] = defaultBinding;
			int num3 = 0 + 1;
		}
		while (0 + 1 < 13);
		inputManager.Save();
		UpdateAllButtonTexts();
		Button[] array2 = allButtons;
		while (0 < array2.Length)
		{
			long num4 = 0L;
			array2[0].interactable = true;
			int num5 = 0 + 1;
			int num3 = 0 + 1;
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
		while (0 < array.Length)
		{
			long num = 0L;
			array[0].interactable = true;
			int num2 = 0 + 1;
			int num3 = 0 + 1;
		}
	}

	// Token: 0x0600008B RID: 139 RVA: 0x00241800 File Offset: 0x0023FE00
	public void CancelReset()
	{
		resetConfirmationPanel.SetActive(value: false);
		Button[] array = allButtons;
		while (0 < array.Length)
		{
			long num = 0L;
			array[0].interactable = true;
			int num2 = 0 + 1;
			int num3 = 0 + 1;
		}
	}

	// Token: 0x0600008C RID: 140 RVA: 0x00241FF0 File Offset: 0x002405F0
	private IEnumerator RebindCoroutine(float duration)
	{
		<RebindCoroutine>d__15 <RebindCoroutine>d__ = NativeIntrinsics.Reinterpret<<RebindCoroutine>d__15>(NativeIntrinsics.Asm("il2cpp_codegen_object_new(ControlsMenu+<RebindCoroutine>d__15)"));
		((Mono.Xml.SecurityParser)(object)<RebindCoroutine>d__).OnStartParsing((Mono.Xml.SmallXmlParser)null);
		<RebindCoroutine>d__.<>1__state = 0;
		<RebindCoroutine>d__.<>4__this = this;
		<RebindCoroutine>d__.duration = duration;
		return <RebindCoroutine>d__;
	}

	// Token: 0x0600008D RID: 141 RVA: 0x00241D30 File Offset: 0x00240330
	private bool GetPressedInput(out KeyCode pressedKey)
	{
		NativeIntrinsics.Stmt("mov dword ptr [rdx],0 ; value = 0x0");
		if (!isBindingMouseInput)
		{
			if (Input.anyKeyDown)
			{
				KeyCode[] array = allKeyCodes;
				while (0 < array.Length)
				{
					KeyCode keyCode = array[0];
					bool keyDownInt = Input.GetKeyDownInt(NativeIntrinsics.Reinterpret<KeyCode>(NativeIntrinsics.Reinterpret<int>(array[0])));
					KeyCode keyCode2 = keyCode;
					if (!keyDownInt || (uint)(NativeIntrinsics.Reinterpret<long>(keyCode2) + -323L) <= 4)
					{
						int num = 0 + 1;
						int num2 = 0 + 1;
						continue;
					}
					NativeIntrinsics.Stmt("mov [r14],esi ; value = ConvExpr");
					return true;
				}
			}
		}
		else
		{
			do
			{
				if (!Input.GetMouseButtonDown(0))
				{
					int num2 = 0 + 1;
					continue;
				}
				NativeIntrinsics.Stmt("mov [r14],ebx ; value = BinaryExpr");
				return true;
			}
			while (0 + 1 < 7);
		}
		return false;
	}

	// Token: 0x0600008E RID: 142 RVA: 0x00241DF0 File Offset: 0x002403F0
	private bool IsMouseKey(KeyCode key)
	{
		if ((uint)(NativeIntrinsics.Reinterpret<long>(key) + -323L) <= 3)
		{
			return true;
		}
		return NativeIntrinsics.Reinterpret<int>(key) == 327;
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00242320 File Offset: 0x00240920
	private void SetButtonsInteractable(bool state)
	{
		Button[] array = allButtons;
		while (0 < array.Length)
		{
			long num = 0L;
			array[0].interactable = state;
			int num2 = 0 + 1;
			int num3 = 0 + 1;
		}
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00242220 File Offset: 0x00240820
	public void ResetToDefaults()
	{
		Button[] array = allButtons;
		while (0 < array.Length)
		{
			long num = 0L;
			array[0].interactable = true;
			int num2 = 0 + 1;
			int num3 = 0 + 1;
		}
		InputManager inputManager = inputMan;
		inputManager.KeyboardMapping.Clear();
		do
		{
			Dictionary<InputAction, InputBinding> keyboardMapping = inputManager.KeyboardMapping;
			InputBinding defaultBinding = inputManager.GetDefaultBinding(NativeIntrinsics.Reinterpret<InputAction>(0));
			keyboardMapping[NativeIntrinsics.Reinterpret<InputAction>(0)] = defaultBinding;
			int num3 = 0 + 1;
		}
		while (0 + 1 < 13);
		inputManager.Save();
		UpdateAllButtonTexts();
	}
}

