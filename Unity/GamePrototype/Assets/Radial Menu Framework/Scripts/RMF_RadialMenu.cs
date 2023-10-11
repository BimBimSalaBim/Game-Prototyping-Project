using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using StarterAssets;

[AddComponentMenu("Radial Menu Framework/RMF Core Script")]
public class RMF_RadialMenu : MonoBehaviour {
    public RectTransform rt;
    public bool useLazySelection = true;
    public bool useSelectionFollower = true;
    public RectTransform selectionFollowerContainer;
    public Text textLabel;
    public List<RMF_RadialMenuElement> elements = new List<RMF_RadialMenuElement>();
    public float globalOffset = 0f;
    public float currentAngle = 0f;
    public int index = 0;
    private int elementCount;
    private float angleOffset;
    private int previousActiveIndex = 0;
    private StarterAssetsInputs inputs;
    public GameObject playerObject;

    void Start() {
        inputs = playerObject.GetComponent<StarterAssetsInputs>();
    }

    void Update() {
        Vector2 lookVector = inputs.interactLook;
        float rawAngle = Mathf.Atan2(lookVector.y - rt.position.y, lookVector.x - rt.position.x) * Mathf.Rad2Deg;
        currentAngle = normalizeAngle(-rawAngle + 90 - globalOffset + (angleOffset / 2f));

        if (angleOffset != 0 && useLazySelection) {
            index = (int)(currentAngle / angleOffset);

            if (elements[index] != null) {
                // Select it.
                selectButton(index);

                // If we click or press a "submit" button, trigger UI interaction (specific Input System code depends on your Input Actions).
                if (inputs.primary) {
                    // Handle UI interaction here (e.g., button click).
                    HandleUIInteraction(elements[index]);
                }
            }
        }
    }

    private void selectButton(int i) {
        if (elements[i].active == false) {
            elements[i].highlightThisElement();
            if (previousActiveIndex != i) {
                elements[previousActiveIndex].unHighlightThisElement();
            }
        }
        previousActiveIndex = i;
    }

    private float normalizeAngle(float angle) {
        angle = angle % 360f;
        if (angle < 0)
            angle += 360;
        return angle;
    }

    private void HandleUIInteraction(RMF_RadialMenuElement element) {
        // Handle UI interaction here. For example, you can trigger the button click action.
        element.clickMeTest(); // Replace this with the appropriate method or action to interact with the UI element.
    }
}