using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericHarvestable : MonoBehaviour, IPlant {
    public string Name { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }

    public void Interact() {
        Debug.Log("HARVESTABLE");
        this.gameObject.SetActive(false);
    }

    public List<IResource> DropResources() {
        List<IResource> resources = new List<IResource>();
        return resources;
    }
}
