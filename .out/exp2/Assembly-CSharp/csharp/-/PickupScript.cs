using System;
using UnityEngine;

// Token: 0x0200004B RID: 75
public class PickupScript : MonoBehaviour
{
	// Token: 0x0400022C RID: 556 Offset: 0x20
	public GameControllerScript gc;

	// Token: 0x0400022D RID: 557 Offset: 0x28
	public Transform player;

	// Token: 0x0600016F RID: 367 RVA: 0x00256150 File Offset: 0x00254750
	private void Update()
	{
		if (!Singleton<InputManager>.Instance.GetActionKeyDown(InputAction.Interact) || Time.timeScale == 0f)
		{
			return;
		}
		Camera main = Camera.main;
		int width = Screen.width;
		int height = Screen.height;
		float x = width / 2;
		float y = height / 2;
		Vector3 pos = new Vector3(x, y, 0f);
		if (!Physics.Raycast(main.ScreenPointToRay(pos), out var hitInfo))
		{
			return;
		}
		bool flag = hitInfo.transform.name == "Pickup_EnergyFlavoredZestyBar";
		Transform transform = player;
		bool num = flag;
		Vector3 position = transform.position;
		Transform transform2 = this.transform;
		x = position.y;
		float x2 = position.x;
		float z = position.z;
		Vector3 position2 = transform2.position;
		x -= position2.y;
		x2 -= position2.x;
		z -= position2.z;
		double d = x * x + x2 * x2 + z * z;
		GameControllerScript gameControllerScript;
		int item_ID;
		if ((10f > (float)((0.0 > (double)(x * x + x2 * x2 + z * z)) ? Math.Sqrt(d) : Math.Sqrt(d))) && num)
		{
			hitInfo.transform.gameObject.SetActive(value: false);
			gameControllerScript = gc;
			item_ID = 1;
		}
		else if ((hitInfo.transform.name == "Pickup_YellowDoorLock") & (10f > Vector3.Distance(player.position, this.transform.position)))
		{
			hitInfo.transform.gameObject.SetActive(value: false);
			gameControllerScript = gc;
			item_ID = 2;
		}
		else if ((hitInfo.transform.name == "Pickup_Key") & (10f > Vector3.Distance(player.position, this.transform.position)))
		{
			hitInfo.transform.gameObject.SetActive(value: false);
			gameControllerScript = gc;
			item_ID = 3;
		}
		else if ((hitInfo.transform.name == "Pickup_BSODA") & (10f > Vector3.Distance(player.position, this.transform.position)))
		{
			hitInfo.transform.gameObject.SetActive(value: false);
			gameControllerScript = gc;
			item_ID = 4;
		}
		else if ((hitInfo.transform.name == "Pickup_Quarter") & (10f > Vector3.Distance(player.position, this.transform.position)))
		{
			hitInfo.transform.gameObject.SetActive(value: false);
			gameControllerScript = gc;
			item_ID = 5;
		}
		else if ((hitInfo.transform.name == "Pickup_Tape") & (10f > Vector3.Distance(player.position, this.transform.position)))
		{
			hitInfo.transform.gameObject.SetActive(value: false);
			gameControllerScript = gc;
			item_ID = 6;
		}
		else if ((hitInfo.transform.name == "Pickup_AlarmClock") & (10f > Vector3.Distance(player.position, this.transform.position)))
		{
			hitInfo.transform.gameObject.SetActive(value: false);
			gameControllerScript = gc;
			item_ID = 7;
		}
		else if ((hitInfo.transform.name == "Pickup_WD-3D") & (10f > Vector3.Distance(player.position, this.transform.position)))
		{
			hitInfo.transform.gameObject.SetActive(value: false);
			gameControllerScript = gc;
			item_ID = 8;
		}
		else if ((hitInfo.transform.name == "Pickup_SafetyScissors") & (10f > Vector3.Distance(player.position, this.transform.position)))
		{
			hitInfo.transform.gameObject.SetActive(value: false);
			gameControllerScript = gc;
			item_ID = 9;
		}
		else if ((hitInfo.transform.name == "Pickup_BigBoots") & (10f > Vector3.Distance(player.position, this.transform.position)))
		{
			hitInfo.transform.gameObject.SetActive(value: false);
			gameControllerScript = gc;
			item_ID = 10;
		}
		else if ((hitInfo.transform.name == "Pickup_Teleporter") & (10f > Vector3.Distance(player.position, this.transform.position)))
		{
			hitInfo.transform.gameObject.SetActive(value: false);
			gameControllerScript = gc;
			item_ID = 11;
		}
		else
		{
			return;
		}
		gameControllerScript.CollectItem(item_ID);
	}
}

