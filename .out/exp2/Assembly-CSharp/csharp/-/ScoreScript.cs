using TMPro;
using UnityEngine;

// Token: 0x02000053 RID: 83
public class ScoreScript : MonoBehaviour
{
	// Token: 0x04000272 RID: 626 Offset: 0x20
	public GameObject scoreText;

	// Token: 0x04000273 RID: 627 Offset: 0x28
	public TMP_Text text;

	// Token: 0x0600019D RID: 413 RVA: 0x00258EF0 File Offset: 0x002574F0
	private void Start()
	{
		if (PlayerPrefs.GetString("CurrentMode") == "endless")
		{
			scoreText.SetActive(value: true);
			TMP_Text tMP_Text = text;
			int num = default(int);
			num.m_value = PlayerPrefs.GetInt("CurrentBooks");
			tMP_Text.text = "Score:\n" + num + " Notebooks";
		}
	}
}

