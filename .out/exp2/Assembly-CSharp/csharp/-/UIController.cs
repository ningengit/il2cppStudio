using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000024 RID: 36
public class UIController : MonoBehaviour
{
	// Token: 0x040000EF RID: 239 Offset: 0x20
	public CursorControllerScript cc;

	// Token: 0x040000F0 RID: 240 Offset: 0x28
	private bool joystickEnabled;

	// Token: 0x040000F1 RID: 241 Offset: 0x29
	public bool controlMouse;

	// Token: 0x040000F2 RID: 242 Offset: 0x2A
	public bool unlockOnStart;

	// Token: 0x040000F3 RID: 243 Offset: 0x2B
	public bool uiControlEnabled;

	// Token: 0x040000F4 RID: 244 Offset: 0x30
	public Selectable firstButton;

	// Token: 0x040000F5 RID: 245 Offset: 0x38
	public Selectable dummyButtonPC;

	// Token: 0x040000F6 RID: 246 Offset: 0x40
	public Selectable dummyButtonElse;

	// Token: 0x040000F7 RID: 247 Offset: 0x48
	public string buttonTag;

	public bool usingJoystick => false;

	// Token: 0x060000BD RID: 189 RVA: 0x002518D0 File Offset: 0x0024FED0
	private void Start()
	{
		if (unlockOnStart & !joystickEnabled)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}

	// Token: 0x060000BE RID: 190 RVA: 0x00251860 File Offset: 0x0024FE60
	private void OnEnable()
	{
		dummyButtonPC.Select();
		UpdateControllerType();
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00251BC0 File Offset: 0x002501C0
	private void Update()
	{
		UpdateControllerType();
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x00251860 File Offset: 0x0024FE60
	public void SwitchMenu()
	{
		dummyButtonPC.Select();
		UpdateControllerType();
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00251A50 File Offset: 0x00250050
	private void UpdateControllerType()
	{
		if (((joystickEnabled ? 1u : 0u) & 1u) != 0)
		{
			joystickEnabled = false;
			if (controlMouse)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}
		if (uiControlEnabled)
		{
			if (!joystickEnabled)
			{
				dummyButtonPC.Select();
			}
			else if ((EventSystem.current.m_CurrentSelected.tag != buttonTag) & (firstButton != null))
			{
				firstButton.Select();
				firstButton.OnSelect(null);
			}
		}
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00251910 File Offset: 0x0024FF10
	private void UIUpdate()
	{
		if (uiControlEnabled)
		{
			if (!joystickEnabled)
			{
				dummyButtonPC.Select();
			}
			else if ((EventSystem.current.m_CurrentSelected.tag != buttonTag) & (firstButton != null))
			{
				firstButton.Select();
				firstButton.OnSelect(null);
			}
		}
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x00251850 File Offset: 0x0024FE50
	public void EnableControl()
	{
		uiControlEnabled = true;
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00251840 File Offset: 0x0024FE40
	public void DisableControl()
	{
		uiControlEnabled = false;
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x002518A0 File Offset: 0x0024FEA0
	private void SelectDummy()
	{
		dummyButtonPC.Select();
	}
}

