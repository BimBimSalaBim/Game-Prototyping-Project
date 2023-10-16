using UnityEngine;
using UnityEngine.UI;

public class ButtonFix : MonoBehaviour
{
    Image img;

    private void Awake() {
        img = GetComponent<Image>();
        img.alphaHitTestMinimumThreshold = 0.5f;
    }
}
