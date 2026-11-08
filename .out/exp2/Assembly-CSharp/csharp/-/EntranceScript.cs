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
		Vector3 position = this.transform.position;
		float x = position.x - 0f;
		float y = position.y - 10f;
		float z = position.z - 0f;
		Vector3 position2 = new Vector3(x, y, z);
		transform.position = position2;
		if (gc.finaleMode)
		{
			((Renderer)wall).SetMaterial(map);
		}
	}

	// Token: 0x060000CE RID: 206 RVA: 0x00244A60 File Offset: 0x00243060
	public void Raise()
	{
		Transform transform = this.transform;
		Vector3 position = this.transform.position;
		Vector3 position2 = new Vector3(0f + position.x, 10f + position.y, 0f + position.z);
		transform.position = position2;
	}
}

