using UnityEngine;

// Token: 0x02000047 RID: 71
public class BsodaSparyScript : MonoBehaviour
{
	// Token: 0x0400021C RID: 540 Offset: 0x20
	public float speed;

	// Token: 0x0400021D RID: 541 Offset: 0x24
	private float lifeSpan;

	// Token: 0x0400021E RID: 542 Offset: 0x28
	private Rigidbody rb;

	// Token: 0x06000163 RID: 355 RVA: 0x002539C0 File Offset: 0x00251FC0
	private void Start()
	{
		Rigidbody component = GetComponent<Rigidbody>();
		rb = component;
		Rigidbody rigidbody = rb;
		Transform transform = this.transform;
		Vector3 forward = transform.forward;
		Vector3 velocity = default(Vector3);
		velocity.x = speed * forward.x;
		velocity.y = speed * forward.y;
		velocity.z = speed * forward.z;
		rigidbody.velocity = velocity;
		lifeSpan = 30f;
	}

	// Token: 0x06000164 RID: 356 RVA: 0x00253A90 File Offset: 0x00252090
	private void Update()
	{
		Rigidbody rigidbody = rb;
		Transform transform = this.transform;
		Transform transform2 = transform;
		Vector3 forward = transform2.forward;
		float z = speed * forward.z;
		float x = speed * forward.x;
		float y = speed * forward.y;
		Vector3 velocity = default(Vector3);
		velocity.x = x;
		velocity.y = y;
		velocity.z = z;
		rigidbody.velocity = velocity;
		float num = lifeSpan;
		float deltaTime = Time.deltaTime;
		lifeSpan = num - deltaTime;
		z = deltaTime;
		if (0f > num - deltaTime)
		{
			GameObject gameObject = this.gameObject;
			GameObject obj = gameObject;
			Object.Destroy(obj, 0f);
		}
	}
}

