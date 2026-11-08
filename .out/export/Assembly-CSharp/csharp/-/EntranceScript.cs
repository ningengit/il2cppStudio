using UnityEngine;

// Token: 0x02000027 RID: 39
public class EntranceScript : MonoBehaviour
{
	// Token: 0x04000103 RID: 259 Offset: 0x20
	public GameControllerScript gc;

	// Token: 0x04000104 RID: 260 Offset: 0x28
	public Material map;

	// Token: 0x04000105 RID: 261 Offset: 0x30
	public MeshRenderer wall;

	// Token: 0x060000CD RID: 205 RVA: 0x002449A0 File Offset: 0x00242FA0
	public void Lower()
	{
		Transform transform = this.transform;
		Transform transform2 = this.transform;
		Transform transform3 = transform2;
		Vector3 position = transform3.position;
		float x = position.x - 0f;
		float y = position.y - 10f;
		float z = position.z - 0f;
		Vector3 position2 = default(Vector3);
		position2.x = x;
		position2.y = y;
		position2.z = z;
		transform.position = position2;
		GameControllerScript gameControllerScript = gc;
		if (gameControllerScript.finaleMode)
		{
			((Renderer)wall).SetMaterial(map);
		}
	}

	// Token: 0x060000CE RID: 206 RVA: 0x00244A60 File Offset: 0x00243060
	public void Raise()
	{
		Transform transform = this.transform;
		Transform transform2 = this.transform;
		Vector3 position = transform2.position;
		Vector3 position2 = default(Vector3);
		position2.x = 0f + position.x;
		position2.y = 10f + position.y;
		position2.z = 0f + position.z;
		transform.position = position2;
	}
}

