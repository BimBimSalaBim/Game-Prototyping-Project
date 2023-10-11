using UnityEngine;

interface IInteractable {
    public void Interact(GameObject radialMenu);
}

public class Interactor : MonoBehaviour
{
    public Transform InteractorSource;
    public float InteractRange; 

    public void checkRayCast(GameObject radialMenu) {
        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange)) {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj)) {
                interactObj.Interact(radialMenu);

            }
        }
    }
}
