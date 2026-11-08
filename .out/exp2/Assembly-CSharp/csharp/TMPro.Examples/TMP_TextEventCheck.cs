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
		if (TextEventHandler != null)
		{
			TextEventHandler.m_OnCharacterSelection.AddListener(OnCharacterSelection);
			TextEventHandler.m_OnSpriteSelection.AddListener(OnSpriteSelection);
			TextEventHandler.m_OnWordSelection.AddListener((UnityAction<string, int, int>)(object)new UnityAction<object, int, int>(OnWordSelection));
			TextEventHandler.m_OnLineSelection.AddListener((UnityAction<string, int, int>)(object)new UnityAction<object, int, int>(OnLineSelection));
			TextEventHandler.m_OnLinkSelection.AddListener((UnityAction<string, string, int>)(object)new UnityAction<object, object, int>(OnLinkSelection));
		}
	}

	// Token: 0x06000247 RID: 583 RVA: 0x0025ABD0 File Offset: 0x002591D0
	private void OnDisable()
	{
		if (TextEventHandler != null)
		{
			((UnityEvent<bool>)(object)TextEventHandler.m_OnCharacterSelection).RemoveListener((UnityAction<bool>)(object)new UnityAction<char, int>(OnCharacterSelection));
			((UnityEvent<bool>)(object)TextEventHandler.m_OnSpriteSelection).RemoveListener((UnityAction<bool>)(object)new UnityAction<char, int>(OnSpriteSelection));
			((UnityEvent<bool>)(object)TextEventHandler.m_OnWordSelection).RemoveListener((UnityAction<bool>)(object)new UnityAction<object, int, int>(OnWordSelection));
			((UnityEvent<bool>)(object)TextEventHandler.m_OnLineSelection).RemoveListener((UnityAction<bool>)(object)new UnityAction<object, int, int>(OnLineSelection));
			((UnityEvent<bool>)(object)TextEventHandler.m_OnLinkSelection).RemoveListener((UnityAction<bool>)(object)new UnityAction<object, object, int>(OnLinkSelection));
		}
	}

	// Token: 0x06000248 RID: 584 RVA: 0x0025AA50 File Offset: 0x00259050
	private void OnCharacterSelection(char c, int index)
	{
		string[] array = new string[5];
		string[] array2 = array;
		array[0] = "Character [";
		char c2 = default(char);
		array2[1] = c2.ToString();
		array2[2] = "] at Index: ";
		int num = default(int);
		array2[3] = num.ToString();
		array2[4] = " has been selected.";
		Debug.Log(string.Concat(array2));
	}

	// Token: 0x06000249 RID: 585 RVA: 0x0025B430 File Offset: 0x00259A30
	private void OnSpriteSelection(char c, int index)
	{
		string[] array = new string[5];
		string[] array2 = array;
		array[0] = "Sprite [";
		char c2 = default(char);
		array2[1] = c2.ToString();
		array2[2] = "] at Index: ";
		int num = default(int);
		array2[3] = num.ToString();
		array2[4] = " has been selected.";
		Debug.Log(string.Concat(array2));
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
		array2[3] = num.ToString();
		array2[4] = " and length of ";
		int num2 = default(int);
		num2.m_value = length;
		array2[5] = num2.ToString();
		array2[6] = " has been selected.";
		Debug.Log(string.Concat(array2));
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
		array2[3] = num.ToString();
		array2[4] = " and length of ";
		int num2 = default(int);
		num2.m_value = length;
		array2[5] = num2.ToString();
		array2[6] = " has been selected.";
		Debug.Log(string.Concat(array2));
	}

	// Token: 0x0600024C RID: 588 RVA: 0x0025B280 File Offset: 0x00259880
	private void OnLinkSelection(string linkID, string linkText, int linkIndex)
	{
		string[] array = new string[7];
		string[] array2 = array;
		array[0] = "Link Index: ";
		int num = default(int);
		num.m_value = linkIndex;
		array2[1] = num.ToString();
		array2[2] = " with ID [";
		array2[3] = linkID;
		array2[4] = "] and Text \"";
		array2[5] = linkText;
		array2[6] = "\" has been selected.";
		Debug.Log(string.Concat(array2));
	}
}

