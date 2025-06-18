using UnityEngine;
using UnityEngine.EventSystems;

public class PointerDebugger : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(
            $"Клик перехвачен UI-элементом: {eventData.pointerCurrentRaycast.gameObject.name}",
            eventData.pointerCurrentRaycast.gameObject
        );
    }
}
