using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GenericHarvestable : MonoBehaviour, IPlant {

    [SerializeField] private Item item;
    [SerializeField] private CollectibleItem collectibleItem;

    public void Start() {
        collectibleItem = GetComponent<CollectibleItem>();
        collectibleItem.Initialize(item);
    }

    public void Interact() {
        StartCoroutine(collectibleItem.MoveAndCollect());
    }
}
