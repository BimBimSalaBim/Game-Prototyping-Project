using StarterAssets;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class EquipmentController : MonoBehaviour {
    public GameObject toolContainer;
    public GameObject mountedEquipment;
    public bool isAnimating = false;
    public int lastSlot = -1;
    public Item lastItem = null;
    public GameObject player;

    void Start() {
        player = GameObject.Find("PlayerArmature");
        EquipSelectedTool();
    }

    void Update() {
        if (InventoryManager.instance.selectedSlot != lastSlot || !InventoryManager.instance.selectedItemChange(lastItem)) {
            Debug.Log("Change");
            if (InventoryManager.instance.CheckSelectedItem().Item2 == ItemType.Tool) {
                Item item = InventoryManager.instance.GetSelectedItem(false);
                EquipSelectedTool(item);
                lastSlot = InventoryManager.instance.selectedSlot;
                lastItem = item;
            } else {
                UnequipTool();
            }

            lastItem = InventoryManager.instance.GetSelectedItem(false);
            lastSlot = InventoryManager.instance.selectedSlot;
        }


        var keyboard = Keyboard.current;

        // Add the rotation coroutine call here if needed

        /*if (keyboard.cKey.wasPressedThisFrame && !isAnimating)
        {
            StartCoroutine(RotateEquipment());
            Debug.Log("Using Equipment");

        }
        */
    }

    void EquipSelectedTool(Item item = null) {
        if (item == null) {
            item = InventoryManager.instance.GetSelectedItem(false);
        }

        if (item && item.model) {
            if (mountedEquipment) {
                Destroy(mountedEquipment);
            }

            GameObject copy = Instantiate(item.model);
            copy.name = item.model.name;
            copy.transform.SetParent(toolContainer.transform);
            copy.transform.localPosition = toolContainer.transform.localPosition - new Vector3(-0.5f, 0.7f, 0.4f);
            copy.transform.rotation = player.transform.rotation * Quaternion.Euler(1f, 1f, 90f);
            mountedEquipment = copy;
        }
    }

    void UnequipTool() {
        if (mountedEquipment) {
            Destroy(mountedEquipment);
            mountedEquipment = null;
        }
    }

    public void UseTool()
    {
        if (!isAnimating)
        {
            if(mountedEquipment.tag == "Sword") 
            {
                int rand = Random.Range(0, 3); // Get a random number 0, 1, or 2
                switch (rand)
                {
                    case 0:
                        StartCoroutine(RotateEquipmentForSwordV0());
                        Debug.Log("Using Sword V-" + rand);
                        break;
                    case 1:
                        StartCoroutine(RotateEquipmentForSwordV1());
                        Debug.Log("Using Sword V-" + rand);
                        break;
                    case 2:
                        StartCoroutine(RotateEquipmentForSwordV2());
                        Debug.Log("Using Sword V-" + rand);
                        break;
                }
            }
            else {
                StartCoroutine(RotateEquipment());
            }
            
            Debug.Log("Using Equipment");

        }
    }


    IEnumerator RotateEquipmentForSwordV0()
    {
        isAnimating = true;
        Quaternion startLocalRotation = mountedEquipment.transform.localRotation;

        Quaternion endRotation = startLocalRotation * Quaternion.Euler(0, 90, 0);

        float duration = 0.1f;
        float elapsedTime = 0;


        while (elapsedTime < duration)
        {
            Quaternion newLocalRotation = Quaternion.Lerp(startLocalRotation, endRotation, elapsedTime / duration);
            mountedEquipment.transform.localRotation = newLocalRotation;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mountedEquipment.transform.localRotation = endRotation;

        elapsedTime = 0;
        while (elapsedTime < duration)
        {
            Quaternion newLocalRotation = Quaternion.Lerp(endRotation, startLocalRotation, elapsedTime / duration);
            mountedEquipment.transform.localRotation = newLocalRotation;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mountedEquipment.transform.localRotation = startLocalRotation;

        isAnimating = false;
    }

    IEnumerator RotateEquipmentForSwordV1()
    {
        isAnimating = true;
        Quaternion startLocalRotation = mountedEquipment.transform.localRotation;

        Quaternion endRotation = startLocalRotation * Quaternion.Euler(120, 90, 0);

        float duration = 0.1f;
        float elapsedTime = 0;


        while (elapsedTime < duration)
        {
            Quaternion newLocalRotation = Quaternion.Lerp(startLocalRotation, endRotation, elapsedTime / duration);
            mountedEquipment.transform.localRotation = newLocalRotation;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mountedEquipment.transform.localRotation = endRotation;

        elapsedTime = 0;
        while (elapsedTime < duration)
        {
            Quaternion newLocalRotation = Quaternion.Lerp(endRotation, startLocalRotation, elapsedTime / duration);
            mountedEquipment.transform.localRotation = newLocalRotation;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mountedEquipment.transform.localRotation = startLocalRotation;

        isAnimating = false;
    }

    IEnumerator RotateEquipmentForSwordV2()
    {
        isAnimating = true;
        Quaternion startLocalRotation = mountedEquipment.transform.localRotation;

        Quaternion endRotation = startLocalRotation * Quaternion.Euler(-120, 90, 0);

        float duration = 0.1f;
        float elapsedTime = 0;


        while (elapsedTime < duration)
        {
            Quaternion newLocalRotation = Quaternion.Lerp(startLocalRotation, endRotation, elapsedTime / duration);
            mountedEquipment.transform.localRotation = newLocalRotation;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mountedEquipment.transform.localRotation = endRotation;

        elapsedTime = 0;
        while (elapsedTime < duration)
        {
            Quaternion newLocalRotation = Quaternion.Lerp(endRotation, startLocalRotation, elapsedTime / duration);
            mountedEquipment.transform.localRotation = newLocalRotation;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mountedEquipment.transform.localRotation = startLocalRotation;

        isAnimating = false;
    }



    IEnumerator RotateEquipment() {
        isAnimating = true;
        Quaternion startLocalRotation = mountedEquipment.transform.localRotation;

        Quaternion endRotation = startLocalRotation * Quaternion.Euler(0, -90, 0);

        float duration = 0.1f; 
        float elapsedTime = 0;

        
        while (elapsedTime < duration) {
            Quaternion newLocalRotation = Quaternion.Lerp(startLocalRotation, endRotation, elapsedTime / duration);
            mountedEquipment.transform.localRotation = newLocalRotation;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mountedEquipment.transform.localRotation = endRotation;

        elapsedTime = 0;
        while (elapsedTime < duration) {
            Quaternion newLocalRotation = Quaternion.Lerp(endRotation, startLocalRotation, elapsedTime / duration);
            mountedEquipment.transform.localRotation = newLocalRotation;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        mountedEquipment.transform.localRotation = startLocalRotation;

        isAnimating = false;
    }
}
