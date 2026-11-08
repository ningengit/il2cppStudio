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
		rb = GetComponent<Rigidbody>();
		Rigidbody rigidbody = rb;
		Vector3 forward = transform.forward;
		Vector3 velocity = new Vector3(speed * forward.x, speed * forward.y, speed * forward.z);
		rigidbody.velocity = velocity;
		lifeSpan = 30f;
	}

	// Token: 0x06000164 RID: 356 RVA: 0x00253A90 File Offset: 0x00252090
	private void Update()
	{
		Rigidbody rigidbody = rb;
		Vector3 forward = transform.forward;
		float z = speed * forward.z;
		float x = speed * forward.x;
		float y = speed * forward.y;
		Vector3 velocity = new Vector3(x, y, z);
		rigidbody.velocity = velocity;
		float num = lifeSpan;
		float deltaTime = Time.deltaTime;
		lifeSpan = num - deltaTime;
		if (0f > num - deltaTime)
		{
			Object.Destroy(gameObject, 0f);
		}
	}
}

