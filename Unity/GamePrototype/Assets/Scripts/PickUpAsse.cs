using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAsset : MonoBehaviour
{
    // Start is called before the first frame update

    public List<GameObject> _InventoryList;
    void Start()
    {
        _InventoryList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toInventory(GameObject asset)
    {
        _InventoryList.Add(asset);
        asset.SetActive(false);
    }

}
