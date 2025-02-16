using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public GameObject topCover;
    public GameObject inventory;
    private Vector3 originalRotation;
    public GameObject textPopUp;


    private void Start()
    {
        // Cache the original rotation of the topCover
        originalRotation = topCover.transform.localEulerAngles;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Entered Chest Trigger");
            textPopUp.SetActive(true);
            Vector3 newRotation = new Vector3(-40, topCover.transform.localEulerAngles.y, topCover.transform.localEulerAngles.z);
            topCover.transform.localEulerAngles = newRotation;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object that exited the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Close the chest by setting the rotation back to the original value
            textPopUp.SetActive(false);
            topCover.transform.localEulerAngles = originalRotation;
        }
    }
    public GameObject getInventory()
    {
        return inventory;
    }
    public void setInventory(GameObject inputInventory)
    {
        
    }
}

