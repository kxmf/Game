using UnityEngine;
using UnityEngine.EventSystems;

public class PointerDebugger : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(
            $"<color=red>Клик перехвачен UI-элементом: {eventData.pointerCurrentRaycast.gameObject.name}</color>",
            eventData.pointerCurrentRaycast.gameObject
        );
    }
}
