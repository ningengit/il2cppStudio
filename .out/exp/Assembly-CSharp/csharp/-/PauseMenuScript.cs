using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000018 RID: 24
public class PauseMenuScript : MonoBehaviour
{
	// Token: 0x040000B6 RID: 182 Offset: 0x20
	public GameControllerScript gc;

	private bool usingJoystick => false;

	// Token: 0x0600007A RID: 122 RVA: 0x0024E610 File Offset: 0x0024CC10
	private void Update()
	{
		_ = EventSystem.current.m_CurrentSelected == null;
		GameControllerScript gameControllerScript = gc;
		if (gameControllerScript.mouseLocked)
		{
			gameControllerScript.UnlockMouse();
		}
	}
}

