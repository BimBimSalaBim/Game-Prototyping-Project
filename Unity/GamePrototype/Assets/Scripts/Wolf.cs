using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour, IAttackable {
    public string Name { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }

    public void PerformAction() {
        Debug.Log("Owwww");
    }

    public List<IResource> DropResources() {
        List<IResource> resources = new List<IResource>();
        return resources;
    }

    public void Initialize(Item item) {
        throw new System.NotImplementedException();
    }

    public IEnumerator MoveAndCollect() {
        throw new System.NotImplementedException();
    }
}
