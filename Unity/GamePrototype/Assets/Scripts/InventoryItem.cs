
using TMPro;
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

    //Start should be removed once we have a item pickup system working
    //should be connected to InventoryManager that will then call InitializeItem
    public void Start() {
        InitializeItem(item);
    }

    public void InitializeItem(Item newItem) {
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }

    public void RefreshCount() {
        countText.text = count.ToString();
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
