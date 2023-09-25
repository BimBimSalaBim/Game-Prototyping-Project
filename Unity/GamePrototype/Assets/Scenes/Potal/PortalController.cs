using StarterAssets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PortalController : MonoBehaviour
{
    public List<Transform> destinations;
    private bool playerInRange = false;
    private bool playerPressKey = false;

    void Update()
    {
        var keyboard = Keyboard.current;

        if (playerInRange && keyboard.fKey.wasPressedThisFrame)
        {
            Debug.Log("F Key Pressed");
            for (int i = 0; i < destinations.Count; i++)
            {
                Debug.Log("Press " + i + " to teleport to destination " + i);
            }
            playerPressKey = true;
        }

        if (playerPressKey && playerInRange)
        {
            for (int j = 0; j < destinations.Count; j++)  // Use destinations.Count here
            {
                if (keyboard[Key.Digit1 + j].wasPressedThisFrame)
                {
                    Debug.Log(j + " Key Pressed");
                    TeleportPlayer(j);
                    playerPressKey = false;  // Reset the playerPressKey here
                    break;
                }
            }
        }
    }

    void TeleportPlayer(int destinationIndex)
    {
        if (destinationIndex >= 0 && destinationIndex < destinations.Count)
        {
            Debug.Log("Attempting to teleport to destination " + destinationIndex);
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector3 position = destinations[destinationIndex].position;
                Debug.Log("Vector 3 : " + position);
                player.GetComponent<ThirdPersonController>().Teleport(position);
            }
            else
            {
                Debug.LogError("No GameObject found with Tag 'Player'");
            }
        }
        else
        {
            Debug.LogError("Invalid Destination Index. Please enter a number between 0 and " + (destinations.Count - 1));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player Entered Portal Trigger");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player Exited Portal Trigger");
        }
    }
}
