using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GenericHarvestable : MonoBehaviour, IPlant {

    [SerializeField] private float collectDistance = 1;
    [SerializeField] private Item item;
    [SerializeField] private float shrinkSize = 0.17f;
    [SerializeField] private float fallHeight = 0.17f;
    [SerializeField] private float fallSpeed = 0.01f;
    [SerializeField] private GameObject shrinkObject = null;
    private Collider collider;
    private Transform player;

    public void Start() {
        Initialize(item);
    }

    public void Initialize(Item item) {
        this.item = item;
        player = GameObject.Find("PlayerArmature").GetComponent<Transform>();
        collider = GetComponent<Collider>();
    }
    public void Interact() {
        StartCoroutine(MoveAndCollect());
    }

    public IEnumerator MoveAndCollect() {
        Destroy(collider);
        var oldPosition = transform.position;
        transform.position += Vector3.up;
        while (Vector3.Distance(transform.position, player.position) > collectDistance) {
            transform.position = Vector3.MoveTowards(transform.position, oldPosition + new Vector3(0, fallHeight, 0), fallSpeed);
            transform.localScale = new Vector3(shrinkSize, shrinkSize, shrinkSize);
            //maybe add timer here that will break out and then alter code to destroy when not picked up
            yield return 0;
        }

        Destroy(gameObject);
        Debug.Log(InventoryManager.instance.AddItem(item));
    }
}
