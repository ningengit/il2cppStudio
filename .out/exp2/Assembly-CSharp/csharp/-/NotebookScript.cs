using System;
using UnityEngine;

// Token: 0x02000049 RID: 73
public class NotebookScript : MonoBehaviour
{
	// Token: 0x04000223 RID: 547 Offset: 0x20
	public float openingDistance;

	// Token: 0x04000224 RID: 548 Offset: 0x28
	public GameControllerScript gc;

	// Token: 0x04000225 RID: 549 Offset: 0x30
	public BaldiScript bsc;

	// Token: 0x04000226 RID: 550 Offset: 0x38
	public float respawnTime;

	// Token: 0x04000227 RID: 551 Offset: 0x3C
	public bool up;

	// Token: 0x04000228 RID: 552 Offset: 0x40
	public Transform player;

	// Token: 0x04000229 RID: 553 Offset: 0x48
	public GameObject learningGame;

	// Token: 0x0400022A RID: 554 Offset: 0x50
	public AudioSource audioDevice;

	// Token: 0x06000169 RID: 361 RVA: 0x00255700 File Offset: 0x00253D00
	private void Start()
	{
		up = true;
	}

	// Token: 0x0600016A RID: 362 RVA: 0x00255710 File Offset: 0x00253D10
	private void Update()
	{
		if (gc.mode == "endless")
		{
			_ = respawnTime;
			if ((respawnTime > 0f))
			{
				Vector3 position4 = this.transform.position;
				Vector3 position5 = player.position;
				Vector3 vector = new Vector3(position4.x - position5.x, position4.y - position5.y, position4.z - position5.z);
				if (vector.magnitude > 60f)
				{
					respawnTime -= Time.deltaTime;
				}
			}
			else if (!up)
			{
				Transform transform = this.transform;
				Vector3 position = this.transform.position;
				Vector3 position3 = new Vector3(position.x, 4f, this.transform.position.z);
				transform.position = position3;
				AudioSource audioSource = audioDevice;
				up = true;
				audioSource.Play();
			}
		}
		if (!Singleton<InputManager>.Instance.GetActionKey(InputAction.Interact) || Time.timeScale == 0f)
		{
			return;
		}
		Camera main = Camera.main;
		int width = Screen.width;
		int height = Screen.height;
		float x = width / 2;
		float y = height / 2;
		Vector3 pos = new Vector3(x, y, 0f);
		if (Physics.Raycast(main.ScreenPointToRay(pos), out var hitInfo))
		{
			bool flag = hitInfo.transform.tag == "Notebook";
			Transform transform2 = player;
			int num = (flag ? 1 : 0);
			Vector3 position6 = transform2.position;
			Transform transform3 = this.transform;
			x = position6.y;
			float x2 = position6.x;
			float z = position6.z;
			Vector3 position7 = transform3.position;
			x -= position7.y;
			x2 -= position7.x;
			z -= position7.z;
			double d = x * x + x2 * x2 + z * z;
			double num2 = ((0.0 > (double)(x * x + x2 * x2 + z * z)) ? Math.Sqrt(d) : Math.Sqrt(d));
			int num3 = ((openingDistance > (float)num2) ? num : 0);
			_ = openingDistance;
			if (num3 != 0)
			{
				Transform transform4 = this.transform;
				Transform transform5 = this.transform;
				Transform transform6 = transform4;
				Vector3 position8 = transform5.position;
				Vector3 position10 = new Vector3(position8.x, -20f, this.transform.position.z);
				transform6.position = position10;
				GameControllerScript gameControllerScript = gc;
				up = false;
				respawnTime = 120f;
				gameControllerScript.CollectNotebook();
				GameObject gameObject = UnityEngine.Object.Instantiate(learningGame);
				gameObject.GetComponent<MathGameScript>().gc = gc;
				gameObject.GetComponent<MathGameScript>().baldiScript = bsc;
				MathGameScript component = gameObject.GetComponent<MathGameScript>();
				component.playerPosition = player.position;
			}
		}
	}
}

