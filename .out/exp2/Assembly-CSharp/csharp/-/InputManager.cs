using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Il2CppStudio.Native;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

// Token: 0x0200001E RID: 30
public class InputManager : Singleton<InputManager>
{
	// Token: 0x040000DE RID: 222 Offset: 0x20
	public Dictionary<InputAction, InputBinding> KeyboardMapping = new Dictionary<InputAction, InputBinding>();

	// Token: 0x040000DF RID: 223 Offset: 0x28
	private Dictionary<KeyCode, SimulatedKeyState> simulatedKeys = new Dictionary<KeyCode, SimulatedKeyState>();

	// Token: 0x040000E0 RID: 224 Offset: 0x30
	private bool[] currentKeyStates = new bool[13];

	// Token: 0x040000E1 RID: 225 Offset: 0x38
	private bool[] previousKeyStates = new bool[13];

	// Token: 0x040000E2 RID: 226 Offset: 0x0
	private const string SavePrefix = "InputBinding_";

	public Dictionary<InputAction, InputBinding> Mappings
	{
		// Token: 0x06000099 RID: 153 RVA: 0x0024A290 File Offset: 0x00248890
		get
		{
			if (KeyboardMapping == null || KeyboardMapping.Count == 0)
			{
				Load();
			}
			return KeyboardMapping;
		}
	}

	// Token: 0x0600009A RID: 154 RVA: 0x00249070 File Offset: 0x00247670
	protected override void Awake()
	{
		if (Singleton<InputManager>.Instance != null && Singleton<InputManager>.Instance != this)
		{
			UnityEngine.Object.Destroy(gameObject);
			return;
		}
		Awake();
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		Load();
	}

	// Token: 0x0600009B RID: 155 RVA: 0x0024A080 File Offset: 0x00248680
	private unsafe void Update()
	{
		bool[] array = currentKeyStates;
		bool[] array2 = previousKeyStates;
		previousKeyStates = array;
		currentKeyStates = array2;
		UpdateSimulatedKeys();
		int num = 0;
		bool keyState = default(bool);
		do
		{
			if (KeyboardMapping.TryGetValue((InputAction)num, out *(InputBinding*)NativeIntrinsics.Asm("&stack[0x8]")))
			{
				long num2 = NativeIntrinsics.Asm("stack[0x8]");
				if (((int)NativeIntrinsics.Asm("stack[0x8]") & (int)NativeIntrinsics.Asm("stack[0x8]")) == 0 || !GetKeyState((KeyCode)num2))
				{
					int key = (int)NativeIntrinsics.Asm("stack[0xC]");
					if ((int)NativeIntrinsics.Asm("stack[0xC]") != 0)
					{
						keyState = GetKeyState((KeyCode)key);
					}
				}
				currentKeyStates[num] = keyState;
			}
			num++;
		}
		while (num + 1 < 13);
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00249E30 File Offset: 0x00248430
	private unsafe void UpdateSimulatedKeys()
	{
		if (simulatedKeys.Count == 0)
		{
			return;
		}
		List<KeyCode> list = new List<KeyCode>((IEnumerable<KeyCode>)((Dictionary<KeyValuePair<object, object>, object>)(object)simulatedKeys).System.Collections.Generic.IDictionary<TKey,TValue>.Keys);
		((List<Color32>)(object)NativeIntrinsics.Reinterpret<List<>>((nint)NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA0]"))).GetEnumerator();
		NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFFA0]");
		NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFFB0]");
		List<>.Enumerator enumerator = default(List<>.Enumerator);
		while (((List<KeyCode>.Enumerator*)(&enumerator))->MoveNext())
		{
			SimulatedKeyState value = simulatedKeys.get_Item((KeyCode)NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFFC8]"));
			int key = (int)NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFFC8]");
			simulatedKeys.set_Item((KeyCode)key, NativeIntrinsics.Reinterpret<SimulatedKeyState>(0));
			if (NativeIntrinsics.Reinterpret<int>(value) == 0)
			{
				simulatedKeys.Remove((KeyCode)key);
			}
		}
		list..ctor();
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00249D90 File Offset: 0x00248390
	public unsafe void SimulateKey(KeyCode key)
	{
		if (!simulatedKeys.TryGetValue(key, out *(SimulatedKeyState*)NativeIntrinsics.Asm("&stack[0x10]")))
		{
		}
		simulatedKeys.set_Item(key, NativeIntrinsics.Reinterpret<SimulatedKeyState>(1));
	}

	// Token: 0x0600009E RID: 158 RVA: 0x002495A0 File Offset: 0x00247BA0
	private unsafe bool GetKeyState(KeyCode key)
	{
		if (simulatedKeys.TryGetValue(key, out *(SimulatedKeyState*)NativeIntrinsics.Asm("&stack[0x10]")) && NativeIntrinsics.Asm("stack[0x10]") != 0)
		{
			return true;
		}
		return Input.GetKeyInt(key);
	}

	// Token: 0x0600009F RID: 159 RVA: 0x00249450 File Offset: 0x00247A50
	public bool GetActionKey(InputAction action)
	{
		return NativeIntrinsics.Asm("*(ConvExpr + 0x20)") != 0;
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x002493B0 File Offset: 0x002479B0
	public bool GetActionKeyDown(InputAction action)
	{
		if (NativeIntrinsics.Asm("*(ConvExpr + 0x20)") == 0)
		{
			return false;
		}
		return NativeIntrinsics.Asm("*(ConvExpr + 0x20)") == 0;
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x00249400 File Offset: 0x00247A00
	public bool GetActionKeyUp(InputAction action)
	{
		if (NativeIntrinsics.Asm("*(ConvExpr + 0x20)") != 0)
		{
			return false;
		}
		return NativeIntrinsics.Asm("*(ConvExpr + 0x20)") != 0;
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x002491B0 File Offset: 0x002477B0
	public unsafe void ClearPrimaryKey(InputAction action)
	{
		if (KeyboardMapping.TryGetValue(action, out *(InputBinding*)NativeIntrinsics.Asm("&stack[0x8]")))
		{
			KeyboardMapping.set_Item(action, NativeIntrinsics.Reinterpret<InputBinding>(0));
		}
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00249250 File Offset: 0x00247850
	public unsafe void ClearSecondaryKey(InputAction action)
	{
		if (KeyboardMapping.TryGetValue(action, out *(InputBinding*)NativeIntrinsics.Asm("&stack[0x8]")))
		{
			KeyboardMapping.set_Item(action, NativeIntrinsics.Reinterpret<InputBinding>(NativeIntrinsics.Asm("stack[0x8]")));
		}
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x002492F0 File Offset: 0x002478F0
	public unsafe string ConvertInputActionToString(InputAction action)
	{
		if (KeyboardMapping.TryGetValue(action, out *(InputBinding*)NativeIntrinsics.Asm("&stack[0x8]")))
		{
			NativeIntrinsics.Asm("stack[0x8]");
			int key = default(int);
			if (((int)NativeIntrinsics.Asm("stack[0x8]") & (int)NativeIntrinsics.Asm("stack[0x8]")) == 0)
			{
				key = (int)NativeIntrinsics.Asm("stack[0xC]");
				if ((int)NativeIntrinsics.Asm("stack[0xC]") == 0)
				{
					goto IL_0091;
				}
			}
			return KeyCodeToDisplayString((KeyCode)key);
		}
		goto IL_0091;
		IL_0091:
		return NativeIntrinsics.Reinterpret<Enum>((nint)NativeIntrinsics.Asm("&stack[0x20]")).ToString();
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x00249A00 File Offset: 0x00248000
	public unsafe void Modify(InputAction action, KeyCode newKey, [Optional] KeyCode secondaryKey)
	{
		if (!KeyboardMapping.TryGetValue(action, out *(InputBinding*)NativeIntrinsics.Asm("&stack[0x8]")))
		{
		}
		KeyCode keyCode = ((newKey == KeyCode.None) ? ((KeyCode)NativeIntrinsics.Asm("stack[0x8]")) : newKey);
		if (secondaryKey == KeyCode.None)
		{
			NativeIntrinsics.Asm("stack[0xC]");
		}
		KeyboardMapping.set_Item(action, NativeIntrinsics.Reinterpret<InputBinding>((int)keyCode));
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00249AC0 File Offset: 0x002480C0
	public unsafe void Save()
	{
		((Dictionary<int, UnityEngine.UIElements.DynamicHeightVirtualizationController<object>.ContentHeightCacheInfo>)(object)NativeIntrinsics.Reinterpret<Dictionary<, >>((nint)NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFB0]"))).GetEnumerator();
		NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFFB0]");
		NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFFC0]");
		Dictionary<, >.Enumerator enumerator = default(Dictionary<, >.Enumerator);
		while (((Dictionary<InputAction, InputBinding>.Enumerator*)(&enumerator))->MoveNext())
		{
			PlayerPrefs.SetString("InputBinding_" + NativeIntrinsics.Reinterpret<Enum>((nint)NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFB0]")).ToString(), string.Format("{0},{1}", (int)NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFFE4]"), (int)((ulong)(int)NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFFE4]") >> 32)));
		}
		((object)this)..ctor();
		PlayerPrefs.Save();
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00249860 File Offset: 0x00247E60
	public unsafe void Load()
	{
		KeyboardMapping.Clear();
		int num = 0;
		InputBinding value = default(InputBinding);
		do
		{
			string text = "InputBinding_" + NativeIntrinsics.Reinterpret<Enum>((nint)NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFC8]")).ToString();
			bool flag = PlayerPrefs.HasKey(text);
			string key = text;
			Dictionary<InputAction, InputBinding> dictionary;
			if (flag)
			{
				string[] array = PlayerPrefs.GetString(key).Split(',', StringSplitOptions.None);
				string[] array2 = array;
				string[] array3 = array;
				if (array2.Length == 2 && int.TryParse(array2[0], out *(int*)NativeIntrinsics.Asm("&stack[0x8]")) && int.TryParse(array3[1], out *(int*)NativeIntrinsics.Asm("&stack[0x18]")))
				{
					dictionary = KeyboardMapping;
					NativeIntrinsics.Asm("stack[0x8]");
					goto IL_00f2;
				}
			}
			Dictionary<InputAction, InputBinding> keyboardMapping = KeyboardMapping;
			InputBinding defaultBinding = GetDefaultBinding((InputAction)num);
			dictionary = keyboardMapping;
			value = defaultBinding;
			goto IL_00f2;
			IL_00f2:
			dictionary.set_Item((InputAction)num, value);
			num++;
		}
		while (num + 1 < 13);
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00249CE0 File Offset: 0x002482E0
	public void SetDefaults()
	{
		KeyboardMapping.Clear();
		int num = 0;
		do
		{
			KeyboardMapping.set_Item((InputAction)num, GetDefaultBinding((InputAction)num));
			num++;
		}
		while (num + 1 < 13);
		Save();
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00249620 File Offset: 0x00247C20
	public string KeyCodeToDisplayString(KeyCode key)
	{
		if (!(key > KeyCode.Escape))
		{
			switch (key)
			{
			case KeyCode.None:
				return "";
			case KeyCode.Escape:
				return "ESC";
			}
		}
		else if (!((uint)((long)key + -48L) <= 9))
		{
			long num = (long)key + -323L;
			if (((int)((long)key + -323L) & (int)((long)key + -323L)) == 0)
			{
				return "Left Mouse Button";
			}
			switch ((int)num - 1)
			{
			case 1:
				return "Middle Mouse Button";
			case 0:
				return "Right Mouse Button";
			}
		}
		else
		{
			return "0";
		}
		return NativeIntrinsics.Reinterpret<Enum>((nint)NativeIntrinsics.Asm("&stack[0x410]")).ToString();
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00249480 File Offset: 0x00247A80
	private InputBinding GetDefaultBinding(InputAction action)
	{
		if ((uint)action <= 12u)
		{
			return NativeIntrinsics.Reinterpret<InputBinding>(97L);
		}
		return NativeIntrinsics.Reinterpret<InputBinding>(0);
	}

	// Token: 0x060000AB RID: 171 RVA: 0x0024A170 File Offset: 0x00248770
	public InputManager()
	{
		((TMP_Dropdown.DropdownItem)(object)this)..ctor();
	}
}

