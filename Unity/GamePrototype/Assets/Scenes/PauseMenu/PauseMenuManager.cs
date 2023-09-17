using UnityEngine;
using UnityEngine.UIElements;
using StarterAssets;
using UnityEngine.SceneManagement;
using System;

public class PauseMenuManager : MonoBehaviour {
    [SerializeField] UIDocument uiDoc;
    private VisualElement pauseMenu;
    private VisualElement optionsControlMenu;

    private void OnEnable() {
        VisualElement root = uiDoc.rootVisualElement;
        pauseMenu = root.Q("PauseMenu");
        optionsControlMenu = root.Q("OptionsControlMenu");

        SetupPauseMenu();
        SetupOptionsMenu();
    }

    private void SetupPauseMenu() {
        PauseMenuPresenter menuPresenter = new PauseMenuPresenter(pauseMenu);
        menuPresenter.OpenOptions = () => {
            pauseMenu.style.display = DisplayStyle.None;
            optionsControlMenu.style.display = DisplayStyle.Flex;
        };
    }

    private void SetupOptionsMenu() {
        OptionsMenuPresenter optionPresenter = new OptionsMenuPresenter(optionsControlMenu);
        optionPresenter.OpenPauseMenu = () => {
            pauseMenu.style.display = DisplayStyle.Flex;
            optionsControlMenu.style.display = DisplayStyle.None;
        };
    }
}
