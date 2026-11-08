using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000017 RID: 23
public class MenuController : MonoBehaviour
{
	// Token: 0x040000B1 RID: 177 Offset: 0x20
	public UIController uc;

	// Token: 0x040000B2 RID: 178 Offset: 0x28
	public Selectable firstButton;

	// Token: 0x040000B3 RID: 179 Offset: 0x30
	public Selectable dummyButtonPC;

	// Token: 0x040000B4 RID: 180 Offset: 0x38
	public Selectable dummyButtonElse;

	// Token: 0x040000B5 RID: 181 Offset: 0x40
	public GameObject back;

	// Token: 0x06000078 RID: 120 RVA: 0x0024DDA0 File Offset: 0x0024C3A0
	public void OnEnable()
	{
		UIController uIController = uc;
		Selectable selectable = firstButton;
		uIController.firstButton = selectable;
		Selectable selectable2 = dummyButtonPC;
		UIController uIController2 = uc;
		uIController2.dummyButtonPC = selectable2;
		UIController uIController3 = uc;
		Selectable selectable3 = dummyButtonElse;
		uIController3.dummyButtonElse = selectable3;
		UIController uIController4 = uc;
		uIController4.UpdateControllerType();
	}
}

