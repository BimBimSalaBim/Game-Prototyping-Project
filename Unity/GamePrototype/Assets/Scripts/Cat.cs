using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using static Interactor;

public class Cat : MonoBehaviour, IAnimal {
    public string Name { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }

    private GameObject commandMenu;

    public void Interact() {
        var buttons = new List<Button>();
        foreach (var item in commandMenu.GetComponentsInChildren<Button>()) {
            buttons.Add(item);
        }
        if (buttons.Count >= 4) {
            buttons[0].GetComponentInChildren<Text>().text = "Feed";
            buttons[1].GetComponentInChildren<Text>().text = "Pet";
            buttons[2].GetComponentInChildren<Text>().text = "Tame";
            buttons[3].GetComponentInChildren<Text>().text = "Stay";
        }
        commandMenu.SetActive(true);
    }

    public List<IResource> DropResources() {
        List<IResource> resources = new List<IResource>();
        return resources;
    }

    public void CommandMenu(GameObject radialMenu) {
        commandMenu = radialMenu;
    }
}