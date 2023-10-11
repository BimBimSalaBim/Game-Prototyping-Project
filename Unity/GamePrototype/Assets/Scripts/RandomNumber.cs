using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class RandomNumber : MonoBehaviour, IInteractable {
    public void Interact(GameObject radialMenu) {
        radialMenu.SetActive(true);
    }
}
