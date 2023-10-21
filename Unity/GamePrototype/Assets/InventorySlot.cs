using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData) {
        Debug.Log("DROP");
        if (transform.childCount == 0) {
            inventoryItem inventoryItem = eventData.pointerDrag.GetComponent<inventoryItem>();
            inventoryItem.parentAfterDrag = transform;
        }
    }
}
