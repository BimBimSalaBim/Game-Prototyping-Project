using StarterAssets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PortalController: MonoBehaviour
{
    public GameObject textPopUp;
    public float rotationSpeed = 5f;
    public List<Transform> destinations;
    private bool playerInRange = false;
    private bool playerPressKey = false;

    void Update()
    {
        var keyboard = Keyboard.current;


        //Alway facing player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && playerInRange)
        {
            Vector3 directionToPlayer = player.transform.position - textPopUp.transform.position;
            Quaternion rotationToPlayer = Quaternion.LookRotation(directionToPlayer);
            rotationToPlayer *= Quaternion.Euler(0, 180, 0); // Add 180 degrees rotation around the Y axis
            textPopUp.transform.rotation = Quaternion.Slerp(textPopUp.transform.rotation, rotationToPlayer, Time.deltaTime * rotationSpeed);
        }


        if (playerInRange && keyboard.fKey.wasPressedThisFrame)
        {
            Debug.Log("F Key Pressed");
            string destinationText = "";
            for (int i = 0; i < destinations.Count; i++)
            {
                Debug.Log("Press " + i + " to teleport to destination " + i);
                destinationText += $"{i + 1} for destination {i + 1}\n ";
            }
            playerPressKey = true;
            textPopUp.GetComponent<TextMesh>().text = destinationText;
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
        textPopUp.GetComponent<TextMesh>().text = "Press F to use";
        textPopUp.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player Entered Portal Trigger");
            textPopUp.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player Exited Portal Trigger");
            textPopUp.GetComponent<TextMesh>().text = "Press F to use";
            textPopUp.SetActive(false);
        }
    }
}
