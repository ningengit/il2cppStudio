using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Token: 0x02000029 RID: 41
public class MouseoverScript : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
	// Token: 0x0400010D RID: 269 Offset: 0x20
	public UnityEvent mouseOver;

	// Token: 0x0400010E RID: 270 Offset: 0x28
	public UnityEvent mouseLeave;

	// Token: 0x060000D3 RID: 211 RVA: 0x0024DF40 File Offset: 0x0024C540
	public void OnSelect(BaseEventData eventData)
	{
		mouseOver.Invoke();
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x0024DF40 File Offset: 0x0024C540
	public void OnPointerEnter(PointerEventData eventData)
	{
		mouseOver.Invoke();
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x0024DF20 File Offset: 0x0024C520
	public void OnDeselect(BaseEventData eventData)
	{
		mouseLeave.Invoke();
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x0024DF20 File Offset: 0x0024C520
	public void OnPointerExit(PointerEventData eventData)
	{
		mouseLeave.Invoke();
	}
}

