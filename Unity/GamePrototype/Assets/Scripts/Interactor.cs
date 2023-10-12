using System.Collections.Generic;
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
            if (hitInfo.collider.gameObject.TryGetComponent(out IMineral mineral)) {
                mineral.PerformAction();
            } else if (hitInfo.collider.gameObject.TryGetComponent(out IMonster monster)) {
                monster.PerformAction();
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

public interface IDroppableResource {
    List<IResource> DropResources();
}

public interface Describable {
    string Name { get; set; }
    string Description { get; set; }
}

public interface IResource : Describable {
    string Icon { get; set; }
}

public interface IAnimal : IInteractable, IDroppableResource {
    void CommandMenu(GameObject radialMenu);
}

public interface IPlant : IInteractable, IDroppableResource {

}

public interface IMineral : IActionable, IDroppableResource {

}

public interface IMonster : IActionable, IDroppableResource {

}
