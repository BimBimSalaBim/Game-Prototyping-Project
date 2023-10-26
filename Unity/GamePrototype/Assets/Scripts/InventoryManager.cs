using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour {
    
    public static InventoryManager instance;

    public Item[] startItems;
    public int maxStackItems = 64;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    int selectedSlot = -1;

    public void Awake() {
        instance = this;
        foreach (Item item in startItems) {
            AddItem(item);
        }
    }

    private void Start() {
        ChangeSelectedSlot(0);

    }

    private void Update() {
        Keyboard keyboard = Keyboard.current;
        if (keyboard.digit1Key.isPressed) {
            ChangeSelectedSlot(0);
        } else if (keyboard.digit2Key.isPressed) {
            ChangeSelectedSlot(1);
        } else if (keyboard.digit3Key.isPressed) {
            ChangeSelectedSlot(2);
        } else if (keyboard.digit4Key.isPressed) {
            ChangeSelectedSlot(3);
        } else if (keyboard.digit5Key.isPressed) {
            ChangeSelectedSlot(4);
        } else if (keyboard.digit6Key.isPressed) {
            ChangeSelectedSlot(5);
        } else if (keyboard.digit7Key.isPressed) {
            ChangeSelectedSlot(6);
        }
    }

    void ChangeSelectedSlot(int newValue) {
        if (selectedSlot >= 0) {
            inventorySlots[selectedSlot].Deselect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
    }
    public bool AddItem(Item item) {
        for (int i = 0; i < inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null 
                    && itemInSlot.item == item 
                    && itemInSlot.count < maxStackItems
                    && itemInSlot.item.stackable) {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }

        for (int i = 0; i < inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null) {
                FillSlot(item, slot);
                return true;
            }
        }
        return false;
    }

    public void FillSlot(Item item, InventorySlot slot) {
        GameObject newItemGO = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }

    public Item GetSelectedItem(bool use) {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null) { 
            Item item = itemInSlot.item;
            if (use == true) {
                itemInSlot.count--;
                if (itemInSlot.count <= 0) {
                    Destroy(itemInSlot.gameObject);
                } else {
                    itemInSlot.RefreshCount();
                }
            }
        }
        return null;
    }
}
