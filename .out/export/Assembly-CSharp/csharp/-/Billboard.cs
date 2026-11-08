using UnityEngine;

// Token: 0x02000004 RID: 4
public class Billboard : MonoBehaviour
{
	// Token: 0x04000007 RID: 7 Offset: 0x20
	private Camera m_Camera;

	// Token: 0x0600000A RID: 10 RVA: 0x0023FB50 File Offset: 0x0023E150
	private void Start()
	{
		Camera main = Camera.main;
		m_Camera = main;
	}

	// Token: 0x0600000B RID: 11 RVA: 0x0023FA00 File Offset: 0x0023E000
	private void LateUpdate()
	{
		Transform transform = this.transform;
		Transform transform2 = this.transform;
		Vector3 position = transform2.position;
		Transform transform3 = m_Camera.transform;
		Quaternion rotation = transform3.rotation;
		Vector3 vector = default(Vector3);
		vector.x = Vector3.forwardVector.x;
		vector.y = Vector3.forwardVector.y;
		vector.z = Vector3.forwardVector.z;
		Vector3 vector2 = rotation * vector;
		Vector3 worldPosition = default(Vector3);
		worldPosition.x = position.x + vector2.x;
		worldPosition.y = position.y + vector2.y;
		worldPosition.z = position.z + vector2.z;
		transform.LookAt(worldPosition);
	}
}

