using Il2CppStudio.Native;
using UnityEngine;

// Token: 0x0200004B RID: 75
public class PickupScript : MonoBehaviour
{
	// Token: 0x0400022C RID: 556 Offset: 0x20
	public GameControllerScript gc;

	// Token: 0x0400022D RID: 557 Offset: 0x28
	public Transform player;

	// Token: 0x0600016F RID: 367 RVA: 0x00256150 File Offset: 0x00254750
	private unsafe void Update()
	{
		InputManager instance = Singleton<InputManager>.Instance;
		InputManager inputManager = instance;
		bool actionKeyDown = inputManager.GetActionKeyDown(NativeIntrinsics.Reinterpret<InputAction>(4));
		bool flag = actionKeyDown;
		if (!actionKeyDown)
		{
			return;
		}
		float timeScale = Time.timeScale;
		float num = timeScale;
		if (timeScale == 0f)
		{
			return;
		}
		Camera main = Camera.main;
		int width = Screen.width;
		int height = Screen.height;
		float num2 = width - (int)NativeIntrinsics.Asm("rdx") >> 1;
		num = height - (int)NativeIntrinsics.Asm("rdx") >> 1;
		Ray ray = main.ScreenPointToRay(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -105L));
		long num3 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x0, 16)");
		long num4 = NativeIntrinsics.Asm("struct_chunk(LocalExpr+0x10, 8)");
		bool flag2 = Physics.Raycast(NativeIntrinsics.Reinterpret<Ray>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -89L), out *(RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -57L));
		flag = flag2;
		if (!flag2)
		{
			return;
		}
		Transform transform = ((RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -57L))->transform;
		string a = transform.name;
		bool flag3 = string.Equals(a, "Pickup_EnergyFlavoredZestyBar");
		Transform transform2 = player;
		int num5 = (flag3 ? 1 : 0);
		Vector3 position = transform2.position;
		Transform transform3 = this.transform;
		float y = position.y;
		float x = position.x;
		float z = position.z;
		Vector3 position2 = transform3.position;
		y -= position2.y;
		x -= position2.x;
		z -= position2.z;
		long num6 = NativeIntrinsics.Asm("cvtps2pd xmm1,xmm6");
		long num7 = default(long);
		if (!(0f > NativeIntrinsics.Reinterpret<float>(NativeIntrinsics.Asm("cvtps2pd xmm1,xmm6"))))
		{
			num7 = NativeIntrinsics.Asm("sqrtsd xmm0,xmm1");
		}
		else
		{
			long num8 = NativeIntrinsics.CallNative("sub_20C340", 0L, NativeIntrinsics.Reinterpret<long>(transform2), 0L, 0L);
		}
		int num9 = ((10f > (float)num7) ? num5 : 0);
		long num10 = NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -57L;
		float num11 = num7;
		GameControllerScript gameControllerScript = default(GameControllerScript);
		if (num9 == 0)
		{
			Transform transform4 = ((RaycastHit*)num10)->transform;
			string a2 = transform4.name;
			bool flag4 = string.Equals(a2, "Pickup_YellowDoorLock");
			transform2 = player;
			Vector3 position3 = transform2.position;
			Transform transform5 = this.transform;
			Vector3 position4 = transform5.position;
			float num12 = Vector3.Distance(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -89L), NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -105L));
			int num13 = ((10f > num12 && flag4) ? 1 : 0);
			num10 = NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -57L;
			num11 = num12;
			if (num13 == 0)
			{
				Transform transform6 = ((RaycastHit*)num10)->transform;
				string a3 = transform6.name;
				bool flag5 = string.Equals(a3, "Pickup_Key");
				transform2 = player;
				Vector3 position5 = transform2.position;
				Transform transform7 = this.transform;
				Vector3 position6 = transform7.position;
				float num14 = Vector3.Distance(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -105L), NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -89L));
				int num15 = ((10f > num14 && flag5) ? 1 : 0);
				num10 = NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -57L;
				num11 = num14;
				if (num15 == 0)
				{
					Transform transform8 = ((RaycastHit*)num10)->transform;
					string a4 = transform8.name;
					bool flag6 = string.Equals(a4, "Pickup_BSODA");
					transform2 = player;
					Vector3 position7 = transform2.position;
					Transform transform9 = this.transform;
					Vector3 position8 = transform9.position;
					float num16 = Vector3.Distance(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -105L), NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -89L));
					int num17 = ((10f > num16 && flag6) ? 1 : 0);
					num10 = NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -57L;
					num11 = num16;
					if (num17 == 0)
					{
						Transform transform10 = ((RaycastHit*)num10)->transform;
						string a5 = transform10.name;
						bool flag7 = string.Equals(a5, "Pickup_Quarter");
						transform2 = player;
						Vector3 position9 = transform2.position;
						Transform transform11 = this.transform;
						Vector3 position10 = transform11.position;
						float num18 = Vector3.Distance(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -105L), NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -89L));
						int num19 = ((10f > num18 && flag7) ? 1 : 0);
						num10 = NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -57L;
						num11 = num18;
						if (num19 == 0)
						{
							Transform transform12 = ((RaycastHit*)num10)->transform;
							string a6 = transform12.name;
							bool flag8 = string.Equals(a6, "Pickup_Tape");
							transform2 = player;
							Vector3 position11 = transform2.position;
							Transform transform13 = this.transform;
							Vector3 position12 = transform13.position;
							float num20 = Vector3.Distance(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -105L), NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -89L));
							int num21 = ((10f > num20 && flag8) ? 1 : 0);
							num10 = NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -57L;
							num11 = num20;
							if (num21 == 0)
							{
								Transform transform14 = ((RaycastHit*)num10)->transform;
								string a7 = transform14.name;
								bool flag9 = string.Equals(a7, "Pickup_AlarmClock");
								transform2 = player;
								Vector3 position13 = transform2.position;
								Transform transform15 = this.transform;
								Vector3 position14 = transform15.position;
								float num22 = Vector3.Distance(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -105L), NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -89L));
								int num23 = ((10f > num22 && flag9) ? 1 : 0);
								num10 = NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -57L;
								num11 = num22;
								if (num23 == 0)
								{
									Transform transform16 = ((RaycastHit*)num10)->transform;
									string a8 = transform16.name;
									bool flag10 = string.Equals(a8, "Pickup_WD-3D");
									transform2 = player;
									Vector3 position15 = transform2.position;
									Transform transform17 = this.transform;
									Vector3 position16 = transform17.position;
									float num24 = Vector3.Distance(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -105L), NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -89L));
									int num25 = ((10f > num24 && flag10) ? 1 : 0);
									num10 = NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -57L;
									num11 = num24;
									if (num25 == 0)
									{
										Transform transform18 = ((RaycastHit*)num10)->transform;
										string a9 = transform18.name;
										bool flag11 = string.Equals(a9, "Pickup_SafetyScissors");
										transform2 = player;
										Vector3 position17 = transform2.position;
										Transform transform19 = this.transform;
										Vector3 position18 = transform19.position;
										float num26 = Vector3.Distance(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -105L), NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -89L));
										int num27 = ((10f > num26 && flag11) ? 1 : 0);
										num10 = NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -57L;
										num11 = num26;
										if (num27 == 0)
										{
											Transform transform20 = ((RaycastHit*)num10)->transform;
											string a10 = transform20.name;
											bool flag12 = string.Equals(a10, "Pickup_BigBoots");
											transform2 = player;
											Vector3 position19 = transform2.position;
											Transform transform21 = this.transform;
											Vector3 position20 = transform21.position;
											float num28 = Vector3.Distance(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -105L), NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -89L));
											int num29 = ((10f > num28 && flag12) ? 1 : 0);
											num10 = NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -57L;
											num11 = num28;
											if (num29 == 0)
											{
												Transform transform22 = ((RaycastHit*)num10)->transform;
												Transform transform23 = transform22;
												string a11 = transform23.name;
												bool flag13 = string.Equals(a11, "Pickup_Teleporter");
												bool flag14 = flag13;
												Vector3 position21 = player.position;
												Transform transform24 = this.transform;
												Transform transform25 = transform24;
												Vector3 position22 = transform25.position;
												float num30 = Vector3.Distance(NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -105L), NativeIntrinsics.Reinterpret<Vector3>(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -89L));
												int num31 = ((10f > num30 && flag13) ? 1 : 0);
												num11 = num30;
												if (num31 == 0)
												{
													return;
												}
												Transform transform26 = ((RaycastHit*)(NativeIntrinsics.Asm("&stack[0xFFFFFFFFFFFFFFA1]") + -57L))->transform;
												GameObject gameObject = transform26.gameObject;
												gameObject.SetActive(value: false);
												gameControllerScript = gc;
											}
											else
											{
												Transform transform27 = ((RaycastHit*)gameControllerScript)->transform;
												GameObject gameObject2 = transform27.gameObject;
												gameObject2.SetActive(value: false);
												gameControllerScript = gc;
											}
										}
										else
										{
											Transform transform28 = ((RaycastHit*)gameControllerScript)->transform;
											GameObject gameObject3 = transform28.gameObject;
											gameObject3.SetActive(value: false);
											gameControllerScript = gc;
										}
									}
									else
									{
										Transform transform29 = ((RaycastHit*)gameControllerScript)->transform;
										GameObject gameObject4 = transform29.gameObject;
										gameObject4.SetActive(value: false);
										gameControllerScript = gc;
									}
								}
								else
								{
									Transform transform30 = ((RaycastHit*)gameControllerScript)->transform;
									GameObject gameObject5 = transform30.gameObject;
									gameObject5.SetActive(value: false);
									gameControllerScript = gc;
								}
							}
							else
							{
								Transform transform31 = ((RaycastHit*)gameControllerScript)->transform;
								GameObject gameObject6 = transform31.gameObject;
								gameObject6.SetActive(value: false);
								gameControllerScript = gc;
							}
						}
						else
						{
							Transform transform32 = ((RaycastHit*)gameControllerScript)->transform;
							GameObject gameObject7 = transform32.gameObject;
							gameObject7.SetActive(value: false);
							gameControllerScript = gc;
						}
					}
					else
					{
						Transform transform33 = ((RaycastHit*)gameControllerScript)->transform;
						GameObject gameObject8 = transform33.gameObject;
						gameObject8.SetActive(value: false);
						gameControllerScript = gc;
					}
				}
				else
				{
					Transform transform34 = ((RaycastHit*)gameControllerScript)->transform;
					GameObject gameObject9 = transform34.gameObject;
					gameObject9.SetActive(value: false);
					gameControllerScript = gc;
				}
			}
			else
			{
				Transform transform35 = ((RaycastHit*)gameControllerScript)->transform;
				GameObject gameObject10 = transform35.gameObject;
				gameObject10.SetActive(value: false);
				gameControllerScript = gc;
			}
		}
		else
		{
			Transform transform36 = ((RaycastHit*)gameControllerScript)->transform;
			GameObject gameObject11 = transform36.gameObject;
			gameObject11.SetActive(value: false);
			gameControllerScript = gc;
		}
		gameControllerScript.CollectItem(11);
	}
}

