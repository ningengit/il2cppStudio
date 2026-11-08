using Il2CppStudio.Native;
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
	private unsafe void Update()
	{
		GameControllerScript gameControllerScript = gc;
		float num;
		if (string.Equals(gameControllerScript.mode, "endless"))
		{
			num = respawnTime;
			if (!(respawnTime > 0f))
			{
				if (!up)
				{
					Transform transform = this.transform;
					Transform transform2 = this.transform;
					Vector3 position = transform2.position;
					Transform transform3 = this.transform;
					Vector3 position2 = transform3.position;
					transform.position = NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -105L);
					AudioSource audioSource = audioDevice;
					up = true;
					audioSource.Play();
				}
			}
			else
			{
				Transform transform4 = this.transform;
				Vector3 position3 = transform4.position;
				Transform transform5 = player;
				Vector3 position4 = transform5.position;
				float magnitude = ((Vector3*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -105L))->magnitude;
				num = magnitude;
				if (magnitude > 60f)
				{
					float num2 = respawnTime;
					float deltaTime = Time.deltaTime;
					respawnTime = num2 - deltaTime;
					num = deltaTime;
				}
			}
		}
		InputManager instance = Singleton<InputManager>.Instance;
		InputManager inputManager = instance;
		bool actionKey = inputManager.GetActionKey(NativeIntrinsics.Reinterpret<InputAction>(4));
		bool flag = actionKey;
		if (!actionKey)
		{
			return;
		}
		float timeScale = Time.timeScale;
		num = timeScale;
		if (timeScale == 0f)
		{
			return;
		}
		Camera main = Camera.main;
		int width = Screen.width;
		int height = Screen.height;
		float num3 = width - (int)NativeIntrinsics.Asm("rdx") >> 1;
		num = height - (int)NativeIntrinsics.Asm("rdx") >> 1;
		Ray ray = main.ScreenPointToRay(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -105L));
		long num4 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
		long num5 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x10, 8)");
		bool flag2 = Physics.Raycast(NativeIntrinsics.Reinterpret<Ray>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -89L), out *(RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -57L));
		flag = flag2;
		if (flag2)
		{
			Transform transform6 = ((RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -57L))->transform;
			Transform transform7 = transform6;
			string a = transform7.tag;
			bool flag3 = string.Equals(a, "Notebook");
			Transform transform5 = player;
			int num6 = (flag3 ? 1 : 0);
			bool flag4 = flag3;
			Vector3 position5 = transform5.position;
			Transform transform8 = this.transform;
			float y = position5.y;
			float x = position5.x;
			float z = position5.z;
			Transform transform9 = transform8;
			Vector3 position6 = transform9.position;
			y -= position6.y;
			x -= position6.x;
			z -= position6.z;
			long num7 = NativeIntrinsics.Asm("cvtps2pd xmm1,xmm6");
			long num8 = default(long);
			if (!(0f > NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("cvtps2pd xmm1,xmm6"))))
			{
				num8 = NativeIntrinsics.Asm("sqrtsd xmm0,xmm1");
			}
			else
			{
				long num9 = NativeIntrinsics.CallNative("sub_20C340", 0L, NativeIntrinsics.Reinterpret<long>(transform5), 0L, 0L);
				long num10 = num9;
			}
			int num11 = ((openingDistance > (float)num8) ? num6 : 0);
			float num12 = openingDistance;
			float num13 = num8;
			if (num11 != 0)
			{
				Transform transform10 = this.transform;
				Transform transform11 = this.transform;
				Transform transform12 = transform10;
				Vector3 position7 = transform11.position;
				Transform transform13 = this.transform;
				Vector3 position8 = transform13.position;
				transform12.position = NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -105L);
				GameControllerScript gameControllerScript2 = gc;
				up = false;
				respawnTime = 120f;
				GameControllerScript gameControllerScript3 = gameControllerScript2;
				gameControllerScript3.CollectNotebook();
				GameObject original = learningGame;
				GameObject gameObject = Object.Instantiate(original);
				GameObject gameObject2 = gameObject;
				MathGameScript component = gameObject2.GetComponent<MathGameScript>();
				MathGameScript mathGameScript = component;
				GameControllerScript gameControllerScript4 = gc;
				mathGameScript.gc = gameControllerScript4;
				MathGameScript component2 = gameObject.GetComponent<MathGameScript>();
				mathGameScript = component2;
				BaldiScript baldiScript = bsc;
				mathGameScript.baldiScript = baldiScript;
				MathGameScript component3 = gameObject.GetComponent<MathGameScript>();
				mathGameScript = component3;
				Vector3 position9 = player.position;
				component3.playerPosition.x = position9.x;
				component3.playerPosition.y = position9.y;
				component3.playerPosition.z = position9.z;
				long num14 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 8)");
			}
		}
	}
}

