using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200003D RID: 61
public class OnAwakeTrigger : MonoBehaviour
{
	// Token: 0x040001D3 RID: 467 Offset: 0x20
	public UnityEvent OnEnableEvent;

	// Token: 0x06000139 RID: 313 RVA: 0x0024DF40 File Offset: 0x0024C540
	private void OnEnable()
	{
		OnEnableEvent.Invoke();
	}
}

