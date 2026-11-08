using UnityEngine;
using UnityEngine.Events;

namespace TMPro.Examples;

// Token: 0x02000083 RID: 131
public class TMP_TextEventCheck : MonoBehaviour
{
	// Token: 0x04000354 RID: 852 Offset: 0x20
	public TMP_TextEventHandler TextEventHandler;

	// Token: 0x06000246 RID: 582 RVA: 0x0025AE50 File Offset: 0x00259450
	private void OnEnable()
	{
		TMP_TextEventHandler textEventHandler = TextEventHandler;
		bool flag = textEventHandler != null;
		bool flag2 = flag;
		if (flag)
		{
			TMP_TextEventHandler textEventHandler2 = TextEventHandler;
			TMP_TextEventHandler.CharacterSelectionEvent onCharacterSelection = textEventHandler2.m_OnCharacterSelection;
			UnityAction<char, int> call = OnCharacterSelection;
			onCharacterSelection.AddListener(call);
			textEventHandler2 = TextEventHandler;
			TMP_TextEventHandler.SpriteSelectionEvent onSpriteSelection = textEventHandler2.m_OnSpriteSelection;
			UnityAction<char, int> call2 = OnSpriteSelection;
			onSpriteSelection.AddListener(call2);
			textEventHandler2 = TextEventHandler;
			TMP_TextEventHandler.WordSelectionEvent onWordSelection = textEventHandler2.m_OnWordSelection;
			UnityAction<string, int, int> call3 = (UnityAction<string, int, int>)(object)new UnityAction<object, int, int>(OnWordSelection);
			onWordSelection.AddListener(call3);
			textEventHandler2 = TextEventHandler;
			TMP_TextEventHandler.LineSelectionEvent onLineSelection = textEventHandler2.m_OnLineSelection;
			UnityAction<string, int, int> call4 = (UnityAction<string, int, int>)(object)new UnityAction<object, int, int>(OnLineSelection);
			onLineSelection.AddListener(call4);
			textEventHandler2 = TextEventHandler;
			TMP_TextEventHandler.LinkSelectionEvent onLinkSelection = textEventHandler2.m_OnLinkSelection;
			UnityAction<string, string, int> call5 = (UnityAction<string, string, int>)(object)new UnityAction<object, object, int>(OnLinkSelection);
			onLinkSelection.AddListener(call5);
		}
	}

	// Token: 0x06000247 RID: 583 RVA: 0x0025ABD0 File Offset: 0x002591D0
	private void OnDisable()
	{
		TMP_TextEventHandler textEventHandler = TextEventHandler;
		bool flag = textEventHandler != null;
		bool flag2 = flag;
		if (flag)
		{
			TMP_TextEventHandler textEventHandler2 = TextEventHandler;
			TMP_TextEventHandler.CharacterSelectionEvent onCharacterSelection = textEventHandler2.m_OnCharacterSelection;
			UnityAction<char, int> call = OnCharacterSelection;
			((UnityEvent<bool>)(object)onCharacterSelection).RemoveListener((UnityAction<bool>)(object)call);
			textEventHandler2 = TextEventHandler;
			TMP_TextEventHandler.SpriteSelectionEvent onSpriteSelection = textEventHandler2.m_OnSpriteSelection;
			UnityAction<char, int> call2 = OnSpriteSelection;
			((UnityEvent<bool>)(object)onSpriteSelection).RemoveListener((UnityAction<bool>)(object)call2);
			textEventHandler2 = TextEventHandler;
			TMP_TextEventHandler.WordSelectionEvent onWordSelection = textEventHandler2.m_OnWordSelection;
			UnityAction<string, int, int> call3 = (UnityAction<string, int, int>)(object)new UnityAction<object, int, int>(OnWordSelection);
			((UnityEvent<bool>)(object)onWordSelection).RemoveListener((UnityAction<bool>)(object)call3);
			textEventHandler2 = TextEventHandler;
			TMP_TextEventHandler.LineSelectionEvent onLineSelection = textEventHandler2.m_OnLineSelection;
			UnityAction<string, int, int> call4 = (UnityAction<string, int, int>)(object)new UnityAction<object, int, int>(OnLineSelection);
			((UnityEvent<bool>)(object)onLineSelection).RemoveListener((UnityAction<bool>)(object)call4);
			textEventHandler2 = TextEventHandler;
			TMP_TextEventHandler.LinkSelectionEvent onLinkSelection = textEventHandler2.m_OnLinkSelection;
			UnityAction<string, string, int> call5 = (UnityAction<string, string, int>)(object)new UnityAction<object, object, int>(OnLinkSelection);
			((UnityEvent<bool>)(object)onLinkSelection).RemoveListener((UnityAction<bool>)(object)call5);
		}
	}

	// Token: 0x06000248 RID: 584 RVA: 0x0025AA50 File Offset: 0x00259050
	private void OnCharacterSelection(char c, int index)
	{
		string[] array = new string[5];
		string[] array2 = array;
		array[0] = "Character [";
		char c2 = default(char);
		string text = c2.ToString();
		array2[1] = text;
		array2[2] = "] at Index: ";
		int num = default(int);
		string text2 = num.ToString();
		array2[3] = text2;
		array2[4] = " has been selected.";
		string text3 = string.Concat(array2);
		string message = text3;
		Debug.Log(message);
	}

	// Token: 0x06000249 RID: 585 RVA: 0x0025B430 File Offset: 0x00259A30
	private void OnSpriteSelection(char c, int index)
	{
		string[] array = new string[5];
		string[] array2 = array;
		array[0] = "Sprite [";
		char c2 = default(char);
		string text = c2.ToString();
		array2[1] = text;
		array2[2] = "] at Index: ";
		int num = default(int);
		string text2 = num.ToString();
		array2[3] = text2;
		array2[4] = " has been selected.";
		string text3 = string.Concat(array2);
		string message = text3;
		Debug.Log(message);
	}

	// Token: 0x0600024A RID: 586 RVA: 0x0025B5B0 File Offset: 0x00259BB0
	private void OnWordSelection(string word, int firstCharacterIndex, int length)
	{
		string[] array = new string[7];
		string[] array2 = array;
		array[0] = "Word [";
		array2[1] = word;
		array2[2] = "] with first character index of ";
		int num = default(int);
		num.m_value = firstCharacterIndex;
		string text = num.ToString();
		array2[3] = text;
		array2[4] = " and length of ";
		int num2 = default(int);
		num2.m_value = length;
		string text2 = num2.ToString();
		array2[5] = text2;
		array2[6] = " has been selected.";
		string text3 = string.Concat(array2);
		string message = text3;
		Debug.Log(message);
	}

	// Token: 0x0600024B RID: 587 RVA: 0x0025B0D0 File Offset: 0x002596D0
	private void OnLineSelection(string lineText, int firstCharacterIndex, int length)
	{
		string[] array = new string[7];
		string[] array2 = array;
		array[0] = "Line [";
		array2[1] = lineText;
		array2[2] = "] with first character index of ";
		int num = default(int);
		num.m_value = firstCharacterIndex;
		string text = num.ToString();
		array2[3] = text;
		array2[4] = " and length of ";
		int num2 = default(int);
		num2.m_value = length;
		string text2 = num2.ToString();
		array2[5] = text2;
		array2[6] = " has been selected.";
		string text3 = string.Concat(array2);
		string message = text3;
		Debug.Log(message);
	}

	// Token: 0x0600024C RID: 588 RVA: 0x0025B280 File Offset: 0x00259880
	private void OnLinkSelection(string linkID, string linkText, int linkIndex)
	{
		string[] array = new string[7];
		string[] array2 = array;
		array[0] = "Link Index: ";
		int num = default(int);
		num.m_value = linkIndex;
		string text = num.ToString();
		array2[1] = text;
		array2[2] = " with ID [";
		array2[3] = linkID;
		array2[4] = "] and Text \"";
		array2[5] = linkText;
		array2[6] = "\" has been selected.";
		string text2 = string.Concat(array2);
		string message = text2;
		Debug.Log(message);
	}
}

