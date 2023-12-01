using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodResource : MonoBehaviour
{
    public int _value;
    public bool _isReadyToPickUp = false;
    public Vector3 _targetPosition;
    public Item item;
    //public GameObject _inventoryLocation;
    // Start is called before the first frame update
    void Start()
    {
        //_targetPosition = _inventoryLocation.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void pickUpAsset()
    {
        MoveToTargetPosition();
        Debug.Log("Ship to Inventory");
        InventoryManager.instance.AddItem(item);
    }

    public void MoveToTargetPosition()
    {
        transform.position = _targetPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isReadyToPickUp = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isReadyToPickUp = false;
        }
    }

    public void setValue(int value) { this._value = value; }
    public int getValue() { return _value; }
}
