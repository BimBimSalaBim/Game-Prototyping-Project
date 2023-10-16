using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RandomNumber : MonoBehaviour {
    public void Interact(GameObject radialMenu) {
        var buttons = new List<Button>();
        foreach ( var item in radialMenu.GetComponentsInChildren<Button>()) {
            buttons.Add(item);
        }
        if (buttons.Count >= 4) {
            buttons[0].GetComponentInChildren<Text>().text = "Feed";
            buttons[1].GetComponentInChildren<Text>().text = "Pet";
            buttons[2].GetComponentInChildren<Text>().text = "Tame";
            buttons[3].GetComponentInChildren<Text>().text = "Stay";
        }
        Debug.Log(buttons.Count);
        radialMenu.SetActive(true);
    }
}
