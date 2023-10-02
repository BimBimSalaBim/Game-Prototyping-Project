using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour {
    [SerializeField] UIDocument uiDoc;
    [SerializeField] InputActionAsset actionAsset;
    private VisualElement pauseMenu;
    private VisualElement optionsMenu;

    private void OnEnable() {
        VisualElement root = uiDoc.rootVisualElement;
        pauseMenu = root.Q("PauseMenu");
        optionsMenu = root.Q("OptionsMenu");

        SetupPauseMenu();
        SetupOptionsMenu();
    }

    private void SetupPauseMenu() {
        PauseMenuPresenter menuPresenter = new PauseMenuPresenter(pauseMenu);
        menuPresenter.OpenOptions = () => {
            pauseMenu.style.display = DisplayStyle.None;
            optionsMenu.style.display = DisplayStyle.Flex;
        };
    }

    private void SetupOptionsMenu() {
        OptionsMenuPresenter optionPresenter = new OptionsMenuPresenter(optionsMenu, actionAsset);
        optionPresenter.OpenPauseMenu = () => {
            pauseMenu.style.display = DisplayStyle.Flex;
            optionsMenu.style.display = DisplayStyle.None;
        };
    }
}
