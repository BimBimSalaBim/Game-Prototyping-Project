using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public GameObject topCover;
    private Vector3 originalRotation;


    private void Start()
    {
        // Cache the original rotation of the topCover
        originalRotation = topCover.transform.localEulerAngles;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the player
        // assuming the player has a tag "Player".
        if (other.CompareTag("Player"))
        {
            // Open the chest by setting the x rotation to 30 degrees
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
            topCover.transform.localEulerAngles = originalRotation;
        }
    }
}

