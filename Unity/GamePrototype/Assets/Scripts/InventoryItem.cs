
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class InventoryItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
    [Header("UI")]
    public Image image;
    public Text countText;
    public Item item;


    [HideInInspector]
    public Transform parentAfterDrag;

    [HideInInspector]
    public int count = 1;

    public void InitializeItem(Item newItem) {
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }

    public void RefreshCount() {
        countText.text = count.ToString();
        countText.color = Color.black;
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData) {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Mouse.current.position.ReadValue(); 
    }

    public void OnEndDrag(PointerEventData eventData) {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }
}
