using System.Collections;
using UnityEngine;

public class Interactor : MonoBehaviour {
    public Transform InteractorSource;
    public float InteractRange;

    public void CheckInteract(GameObject radialMenu) {
        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange)) {
            if (hitInfo.collider.gameObject.TryGetComponent(out IAnimal animal)) {
                animal.CommandMenu(radialMenu);
                animal.Interact();
            } else if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactable)) {
                interactable.Interact();
            }
        }
    }

    public void CheckPrimary() {
        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange)) {
            if (hitInfo.collider.gameObject.TryGetComponent(out IAnimal mineral)) {
                mineral.PrimaryInteract();
            }
        }
    }
}

public interface IInteractable {
    void Interact();
}

public interface IActionable {
    void PerformAction();
}

/*
public interface IDroppableResource {
    public void Initialize(Item item);

    public IEnumerator MoveAndCollect();
}
*/

public interface Describable {
    string Name { get; set; }
    string Description { get; set; }
}

public interface IResource : Describable {
    string Icon { get; set; }
}

public interface IAnimal : IInteractable {
    void CommandMenu(GameObject radialMenu);

    void FeedOnClick();

    void PrimaryInteract();
}

public interface IPlant : IInteractable {

}

public interface IMineral : IActionable {

}

public interface IAttackable : IActionable {

}
