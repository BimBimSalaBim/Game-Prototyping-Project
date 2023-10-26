using StarterAssets;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class EquipmentController : MonoBehaviour {
    public GameObject mountedEquipment;
    private StarterAssetsInputs playerInput;
    public bool isAnimating = false;

    void Update()
    {
        var keyboard = Keyboard.current;

/*        if (keyboard.cKey.wasPressedThisFrame && !isAnimating)
        {
            StartCoroutine(RotateEquipment());
            Debug.Log("Using Equipment");
           
        }
*/

    }

    public void UseTool()
    {
        if (!isAnimating)
        {
            StartCoroutine(RotateEquipment());
            Debug.Log("Using Equipment");

        }
    }
    IEnumerator RotateEquipment()
    {
        isAnimating = true;

        Quaternion startRotation = mountedEquipment.transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, -90, 0); // 90 degrees in the x-axis

        float duration = 0.1f; // action duration
        float elapsedTime = 0;

        // Rotate to 90 degrees in the x-axis
        while (elapsedTime < duration)
        {
            mountedEquipment.transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mountedEquipment.transform.rotation = endRotation;

        elapsedTime = 0;

        // Rotate back to the original rotation
        while (elapsedTime < duration)
        {
            mountedEquipment.transform.rotation = Quaternion.Lerp(endRotation, startRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mountedEquipment.transform.rotation = startRotation;

        isAnimating = false;
    }
}
