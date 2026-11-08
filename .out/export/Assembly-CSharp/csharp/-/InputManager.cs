using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Il2CppStudio.Native;
using Mono.Xml;
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
	private const string SavePrefix = null;

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
		InputManager inputManager = Singleton<InputManager>.Instance;
		InputManager inputManager2 = inputManager;
		GameObject obj = default(GameObject);
		if (inputManager2 != null)
		{
			InputManager inputManager3 = Singleton<InputManager>.Instance;
			inputManager2 = inputManager3;
			if (inputManager2 != this)
			{
				GameObject gameObject = this.gameObject;
				obj = gameObject;
				UnityEngine.Object.Destroy(obj);
				return;
			}
		}
		((Singleton<InputManager>)(object)obj).Awake();
		GameObject gameObject2 = ((Component)(object)obj).gameObject;
		GameObject target = gameObject2;
		UnityEngine.Object.DontDestroyOnLoad(target);
		((InputManager)(object)obj).Load();
	}

	// Token: 0x0600009B RID: 155 RVA: 0x0024A080 File Offset: 0x00248680
	private unsafe void Update()
	{
		bool[] array = currentKeyStates;
		bool[] array2 = previousKeyStates;
		previousKeyStates = array;
		currentKeyStates = array2;
		UpdateSimulatedKeys();
		do
		{
			bool flag = KeyboardMapping.TryGetValue(NativeIntrinsics.Reinterpret<InputAction>(0), out *(InputBinding*)NativeIntrinsics.Asm("&stack[0x8]"));
			bool flag2 = flag;
			if (flag)
			{
				long num = NativeIntrinsics.Asm("stack[0x8]");
				if (((int)NativeIntrinsics.Asm("stack[0x8]") & (int)NativeIntrinsics.Asm("stack[0x8]")) == 0 || !GetKeyState(NativeIntrinsics.Reinterpret<KeyCode>(num)))
				{
					int num2 = (int)NativeIntrinsics.Asm("stack[0xC]");
					if ((int)NativeIntrinsics.Asm("stack[0xC]") != 0)
					{
						bool keyState = GetKeyState(NativeIntrinsics.Reinterpret<KeyCode>(num2));
						flag2 = keyState;
					}
				}
				currentKeyStates[0] = flag2;
			}
			int num3 = 0 + 1;
		}
		while (0 + 1 < 13);
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00249E30 File Offset: 0x00248430
	private unsafe void UpdateSimulatedKeys()
	{
		int count = simulatedKeys.Count;
		int num = count;
		if (count == 0)
		{
			return;
		}
		ICollection<!0> collection = (ICollection<!0>)((Dictionary<KeyValuePair<object, object>, object>)(object)simulatedKeys).System.Collections.Generic.IDictionary<TKey,TValue>.Keys;
		List<KeyCode> list = new List<KeyCode>((IEnumerable<KeyCode>)collection);
		List<KeyCode> list2 = list;
		List<!0>.Enumerator enumerator = (List<!0>.Enumerator)((List<Color32>)(object)NativeIntrinsics.Reinterpret<List<>>((nint)NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA0]"))).GetEnumerator();
		long num2 = NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFFA0]");
		long num3 = NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFFB0]");
		List<>.Enumerator enumerator2 = default(List<>.Enumerator);
		while (((List<KeyCode>.Enumerator*)(&enumerator2))->MoveNext())
		{
			SimulatedKeyState value = simulatedKeys[NativeIntrinsics.Reinterpret<KeyCode>((int)NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFFC8]"))];
			int num4 = (int)NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFFC8]");
			simulatedKeys[NativeIntrinsics.Reinterpret<KeyCode>(num4)] = NativeIntrinsics.Reinterpret<SimulatedKeyState>(0);
			if (NativeIntrinsics.Reinterpret<int>(value) == 0)
			{
				bool flag = simulatedKeys.Remove(NativeIntrinsics.Reinterpret<KeyCode>(num4));
			}
		}
		NativeIntrinsics.Reinterpret<Mono.Xml.SecurityParser>((nint)NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFB8]")).OnStartParsing(NativeIntrinsics.Reinterpret<Mono.Xml.SmallXmlParser>((nint)NativeIntrinsics.Asm("System.Collections.Generic.List<KeyCode>::Dispose_RuntimeMethod")));
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00249D90 File Offset: 0x00248390
	public unsafe void SimulateKey(KeyCode key)
	{
		int num = NativeIntrinsics.Reinterpret<int>(key);
		if (!simulatedKeys.TryGetValue(NativeIntrinsics.Reinterpret<KeyCode>(num), out *(SimulatedKeyState*)NativeIntrinsics.Asm("&stack[0x10]")))
		{
		}
		simulatedKeys[NativeIntrinsics.Reinterpret<KeyCode>(num)] = NativeIntrinsics.Reinterpret<SimulatedKeyState>(1);
	}

	// Token: 0x0600009E RID: 158 RVA: 0x002495A0 File Offset: 0x00247BA0
	private unsafe bool GetKeyState(KeyCode key)
	{
		int num = NativeIntrinsics.Reinterpret<int>(key);
		if (simulatedKeys.TryGetValue(NativeIntrinsics.Reinterpret<KeyCode>(num), out *(SimulatedKeyState*)NativeIntrinsics.Asm("&stack[0x10]")) && NativeIntrinsics.Asm("stack[0x10]") != 0)
		{
			return true;
		}
		return Input.GetKeyInt(NativeIntrinsics.Reinterpret<KeyCode>(num));
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
		int num = NativeIntrinsics.Reinterpret<int>(action);
		bool flag = KeyboardMapping.TryGetValue(NativeIntrinsics.Reinterpret<InputAction>(num), out *(InputBinding*)NativeIntrinsics.Asm("&stack[0x8]"));
		bool flag2 = flag;
		if (flag)
		{
			KeyboardMapping[NativeIntrinsics.Reinterpret<InputAction>(num)] = NativeIntrinsics.Reinterpret<InputBinding>(0);
		}
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00249250 File Offset: 0x00247850
	public unsafe void ClearSecondaryKey(InputAction action)
	{
		int num = NativeIntrinsics.Reinterpret<int>(action);
		bool flag = KeyboardMapping.TryGetValue(NativeIntrinsics.Reinterpret<InputAction>(num), out *(InputBinding*)NativeIntrinsics.Asm("&stack[0x8]"));
		bool flag2 = flag;
		if (flag)
		{
			KeyboardMapping[NativeIntrinsics.Reinterpret<InputAction>(num)] = NativeIntrinsics.Reinterpret<InputBinding>(NativeIntrinsics.Asm("stack[0x8]"));
		}
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x002492F0 File Offset: 0x002478F0
	public unsafe string ConvertInputActionToString(InputAction action)
	{
		int num = NativeIntrinsics.Reinterpret<int>(action);
		if (KeyboardMapping.TryGetValue(NativeIntrinsics.Reinterpret<InputAction>(num), out *(InputBinding*)NativeIntrinsics.Asm("&stack[0x8]")))
		{
			long num2 = NativeIntrinsics.Asm("stack[0x8]");
			int num3 = default(int);
			if (((int)NativeIntrinsics.Asm("stack[0x8]") & (int)NativeIntrinsics.Asm("stack[0x8]")) == 0)
			{
				num3 = (int)NativeIntrinsics.Asm("stack[0xC]");
				if ((int)NativeIntrinsics.Asm("stack[0xC]") == 0)
				{
					goto IL_0093;
				}
			}
			return KeyCodeToDisplayString(NativeIntrinsics.Reinterpret<KeyCode>(num3));
		}
		goto IL_0093;
		IL_0093:
		return NativeIntrinsics.Reinterpret<Enum>((nint)NativeIntrinsics.Asm("&stack[0x20]")).ToString();
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x00249A00 File Offset: 0x00248000
	public unsafe void Modify(InputAction action, KeyCode newKey, [Optional] KeyCode secondaryKey)
	{
		int num = NativeIntrinsics.Reinterpret<int>(newKey);
		int num2 = NativeIntrinsics.Reinterpret<int>(secondaryKey);
		int num3 = NativeIntrinsics.Reinterpret<int>(action);
		if (!KeyboardMapping.TryGetValue(NativeIntrinsics.Reinterpret<InputAction>(num3), out *(InputBinding*)NativeIntrinsics.Asm("&stack[0x8]")))
		{
		}
		int num4 = (int)((num == 0) ? NativeIntrinsics.Asm("stack[0x8]") : num);
		if (num2 != 0)
		{
			int num5 = num2;
		}
		else
		{
			int num5 = (int)NativeIntrinsics.Asm("stack[0xC]");
		}
		KeyboardMapping[NativeIntrinsics.Reinterpret<InputAction>(num3)] = NativeIntrinsics.Reinterpret<InputBinding>(num4);
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x00249AC0 File Offset: 0x002480C0
	public unsafe void Save()
	{
		Dictionary<!0, !1>.Enumerator enumerator = (Dictionary<!0, !1>.Enumerator)((Dictionary<int, UnityEngine.UIElements.DynamicHeightVirtualizationController<object>.ContentHeightCacheInfo>)(object)NativeIntrinsics.Reinterpret<Dictionary<, >>((nint)NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFB0]"))).GetEnumerator();
		long num = NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFFB0]");
		long num2 = NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFFC0]");
		Dictionary<, >.Enumerator enumerator2 = default(Dictionary<, >.Enumerator);
		while (((Dictionary<InputAction, InputBinding>.Enumerator*)(&enumerator2))->MoveNext())
		{
			string text = NativeIntrinsics.Reinterpret<Enum>((nint)NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFB0]")).ToString();
			string key = "InputBinding_" + text;
			object arg = (int)NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFFE4]");
			object arg2 = (int)((ulong)(int)NativeIntrinsics.Asm("stack[0xFFFFFFFFFFFFFFE4]") >> 32);
			string value = $"{arg},{arg2}";
			PlayerPrefs.SetString(key, value);
		}
		NativeIntrinsics.Reinterpret<Mono.Xml.SecurityParser>((nint)NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFD0]")).OnStartParsing(NativeIntrinsics.Reinterpret<Mono.Xml.SmallXmlParser>((nint)NativeIntrinsics.Asm("System.Collections.Generic.Dictionary<InputAction, InputBinding>::Dispose_RuntimeMethod")));
		PlayerPrefs.Save();
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00249860 File Offset: 0x00247E60
	public unsafe void Load()
	{
		KeyboardMapping.Clear();
		InputBinding value = default(InputBinding);
		do
		{
			string text = NativeIntrinsics.Reinterpret<Enum>((nint)NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFC8]")).ToString();
			string text2 = "InputBinding_" + text;
			bool flag = PlayerPrefs.HasKey(text2);
			string key = text2;
			Dictionary<InputAction, InputBinding> dictionary;
			if (flag)
			{
				string text3 = PlayerPrefs.GetString(key);
				string text4 = text3;
				string[] array = text4.Split(',', NativeIntrinsics.Reinterpret<StringSplitOptions>(0));
				string[] array2 = array;
				string[] array3 = array;
				if (array2.Length == 2 && int.TryParse(array2[0], out *(int*)NativeIntrinsics.Asm("&stack[0x8]")) && int.TryParse(array3[1], out *(int*)NativeIntrinsics.Asm("&stack[0x18]")))
				{
					dictionary = KeyboardMapping;
					int num = (int)NativeIntrinsics.Asm("stack[0x8]");
					goto IL_00fa;
				}
			}
			Dictionary<InputAction, InputBinding> keyboardMapping = KeyboardMapping;
			InputBinding defaultBinding = GetDefaultBinding(NativeIntrinsics.Reinterpret<InputAction>(0));
			dictionary = keyboardMapping;
			value = defaultBinding;
			goto IL_00fa;
			IL_00fa:
			dictionary[NativeIntrinsics.Reinterpret<InputAction>(0)] = value;
			int num2 = 0 + 1;
		}
		while (0 + 1 < 13);
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00249CE0 File Offset: 0x002482E0
	public void SetDefaults()
	{
		KeyboardMapping.Clear();
		do
		{
			Dictionary<InputAction, InputBinding> keyboardMapping = KeyboardMapping;
			InputBinding defaultBinding = GetDefaultBinding(NativeIntrinsics.Reinterpret<InputAction>(0));
			keyboardMapping[NativeIntrinsics.Reinterpret<InputAction>(0)] = defaultBinding;
			int num = 0 + 1;
		}
		while (0 + 1 < 13);
		Save();
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00249620 File Offset: 0x00247C20
	public string KeyCodeToDisplayString(KeyCode key)
	{
		int num = NativeIntrinsics.Reinterpret<int>(key);
		if (NativeIntrinsics.Reinterpret<int>(key) > 27)
		{
			if ((uint)(num + -48L) <= 9)
			{
				return "0";
			}
			long num2 = num + -323L;
			if (((int)(num + -323L) & (int)(num + -323L)) == 0)
			{
				return "Left Mouse Button";
			}
			int num3 = (int)num2 - 1;
			if ((int)num2 - 1 == 0)
			{
				return "Right Mouse Button";
			}
			if (num3 == 1)
			{
				return "Middle Mouse Button";
			}
		}
		else
		{
			switch (num)
			{
			case 0:
				return "";
			case 27:
				return "ESC";
			}
		}
		return NativeIntrinsics.Reinterpret<Enum>((nint)NativeIntrinsics.Asm("&stack[0x410]")).ToString();
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00249480 File Offset: 0x00247A80
	private InputBinding GetDefaultBinding(InputAction action)
	{
		if ((uint)NativeIntrinsics.Reinterpret<int>(action) <= 12u)
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

