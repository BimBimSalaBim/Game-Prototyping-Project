using System.Collections;
using UnityEngine;

public class CollectibleItem : MonoBehaviour {
    [SerializeField] private float collectDistance = 1;
    [SerializeField] private float shrinkSize = 0.17f;
    [SerializeField] private float fallHeight = 0.17f;
    [SerializeField] private float fallSpeed = 0.01f;
    
    private Item item;
    private Collider collider;
    private Transform player;

    public void Initialize(Item item) {
        this.item = item;
        player = GameObject.Find("PlayerArmature").GetComponent<Transform>();
        collider = GetComponent<Collider>();
    }

    public IEnumerator MoveAndCollect() {
        Destroy(collider);
        var isChild = false;
        if (GetComponentInParent<FoV>() != null) {
            var parent = transform.parent.gameObject;
            isChild = true;
            Destroy(GetComponentInParent<FoV>());
            foreach(Transform child in parent.transform) {
                if (child != transform) {
                    Destroy(child.gameObject);
                }
            }
        }
        var oldPosition = transform.position;
        transform.position += Vector3.up;
        while (Vector3.Distance(transform.position, player.position) > collectDistance) {
            transform.position = Vector3.MoveTowards(transform.position, oldPosition + new Vector3(0, fallHeight, 0), fallSpeed);
            transform.localScale = new Vector3(shrinkSize, shrinkSize, shrinkSize);
            // maybe add a timer here that will break out, then alter code to destroy when not picked up
            yield return null;
        }

        Destroy(gameObject);
        if (isChild) {
            Destroy(transform.parent.gameObject);
        }
        Debug.Log(InventoryManager.instance.AddItem(item));
    }
}
