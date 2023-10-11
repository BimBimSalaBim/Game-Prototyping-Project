using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;

[AddComponentMenu("Radial Menu Framework/RMF Element")]
public class RMF_RadialMenuElement : MonoBehaviour {
    [HideInInspector]
    public RectTransform rt;
    [HideInInspector]
    public RMF_RadialMenu parentRM;

    [Tooltip("Each radial element needs a button. This is generally a child one level below this primary radial element game object.")]
    public Selectable selectable; // Replace 'Button' with 'Selectable'

    [Tooltip("This is the text label that will appear in the center of the radial menu when this option is moused over. Best to keep it short.")]
    public string label;

    [HideInInspector]
    public float angleMin, angleMax;

    [HideInInspector]
    public float angleOffset;

    [HideInInspector]
    public bool active = false;

    [HideInInspector]
    public int assignedIndex = 0;

    private CanvasGroup cg;

    void Awake() {
        rt = gameObject.GetComponent<RectTransform>();

        if (gameObject.GetComponent<CanvasGroup>() == null)
            cg = gameObject.AddComponent<CanvasGroup>();
        else
            cg = gameObject.GetComponent<CanvasGroup>();

        if (rt == null)
            Debug.LogError("Radial Menu: Rect Transform for radial element " + gameObject.name + " could not be found. Please ensure this is an object parented to a canvas.");

        if (selectable == null)
            Debug.LogError("Radial Menu: No Selectable attached to " + gameObject.name + "!");
    }

    void Start() {
        parentRM = GetComponentInParent<RMF_RadialMenu>();
        cg.blocksRaycasts = !parentRM.useLazySelection; // Blocks raycasts if using lazy selection.
    }

    // Used by the parent radial menu to set up all the appropriate angles. Affects master Z rotation and the active angles for lazy selection.
    public void setAllAngles(float offset, float baseOffset) {
        angleOffset = offset;
        angleMin = offset - (baseOffset / 2f);
        angleMax = offset + (baseOffset / 2f);
    }

    // Highlights this button using the new Input System events.
    public void highlightThisElement() {
        // Use new Input System events to highlight the element.
        if (selectable != null) {
            selectable.Select();
            active = true;
        }

        setParentMenuLabel(label);
    }

    // Sets the label of the parent menu. Is set to public so you can call this elsewhere if you need to show a special label for something.
    public void setParentMenuLabel(string l) {
        if (parentRM.textLabel != null)
            parentRM.textLabel.text = l;
    }

    // Unhighlights the button and resets the menu's label if lazy selection is off.
    public void unHighlightThisElement() {
        // Use new Input System events to unhighlight the element.
        if (selectable != null) {
            selectable.OnDeselect(null);
            active = false;
        }

        if (!parentRM.useLazySelection)
            setParentMenuLabel(" ");
    }

    // Just a quick test you can run to ensure things are working properly.
    public void clickMeTest() {
        Debug.Log(assignedIndex);
    }
}