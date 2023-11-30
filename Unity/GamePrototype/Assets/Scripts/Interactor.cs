using System.Collections;
using UnityEngine;

public class Interactor : MonoBehaviour {
    public Transform InteractorSource;
    public float InteractRange;
    public float SphereRadius; 
 public float CastHeight;
    void OnDrawGizmos() {
        if (InteractorSource == null) {
            return;
        }

        Vector3 castPosition = InteractorSource.position + Vector3.up * CastHeight;

        Gizmos.color = Color.red;
        Vector3 direction = InteractorSource.forward * InteractRange;
        Gizmos.DrawRay(castPosition, direction);
        Gizmos.DrawWireSphere(castPosition + direction, SphereRadius);
    }
    public void CheckInteract(GameObject radialMenu) {
        Vector3 castPosition = InteractorSource.position + Vector3.up * CastHeight;
        if (Physics.SphereCast(castPosition, SphereRadius, InteractorSource.forward, out RaycastHit hitInfo, InteractRange)) {
            if (hitInfo.collider.gameObject.TryGetComponent(out IAnimal animal)) {
                animal.CommandMenu(radialMenu);
                animal.Interact();
            } else if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactable)) {
                interactable.Interact();
            }
        }
    }
    public void CheckPrimary() {
        Vector3 castPosition = InteractorSource.position + Vector3.up * CastHeight;
        RaycastHit[] hits = Physics.SphereCastAll(castPosition, SphereRadius, InteractorSource.forward, InteractRange);
        foreach (RaycastHit hitInfo in hits) {
            if (hitInfo.collider.gameObject.TryGetComponent(out IAnimal animal)) {
                animal.PrimaryInteract();
            }
        }
    }

    public bool InRange() {
        Vector3 castPosition = InteractorSource.position + Vector3.up * CastHeight;
        RaycastHit[] hits = Physics.SphereCastAll(castPosition, SphereRadius, InteractorSource.forward, InteractRange);
        foreach (RaycastHit hitInfo in hits) {
            if (hitInfo.collider.gameObject.TryGetComponent(out IAnimal animal)) {
                return true; // Returns true if any IAnimal is within range
            }
        }
        return false; // Returns false if no IAnimals are within range
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
