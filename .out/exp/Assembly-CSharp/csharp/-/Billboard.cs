using UnityEngine;

// Token: 0x02000004 RID: 4
public class Billboard : MonoBehaviour
{
	// Token: 0x04000007 RID: 7 Offset: 0x20
	private Camera m_Camera;

	// Token: 0x0600000A RID: 10 RVA: 0x0023FB50 File Offset: 0x0023E150
	private void Start()
	{
		m_Camera = Camera.main;
	}

	// Token: 0x0600000B RID: 11 RVA: 0x0023FA00 File Offset: 0x0023E000
	private void LateUpdate()
	{
		Transform transform = this.transform;
		Vector3 position = this.transform.position;
		Quaternion rotation = m_Camera.transform.rotation;
		Vector3 vector2 = rotation * Vector3.forward;
		Vector3 worldPosition = new Vector3(position.x + vector2.x, position.y + vector2.y, position.z + vector2.z);
		transform.LookAt(worldPosition);
	}
}

