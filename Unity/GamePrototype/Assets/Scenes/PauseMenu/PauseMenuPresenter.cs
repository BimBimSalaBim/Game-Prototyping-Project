using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenuPresenter
{
    public Action OpenOptions { set => optionsButton.clicked += value; }

    private Button resume;
    private Button save;
    private Button optionsButton;
    private Button mainMenu;
    private Button exitGame;
 
    public PauseMenuPresenter(VisualElement root) {
        resume = root.Q<Button>("Resume");
        save = root.Q<Button>("Save");
        optionsButton = root.Q<Button>("Options");
        mainMenu = root.Q<Button>("MainMenu");
        exitGame = root.Q<Button>("ExitGame");

        resume.clicked += Resume;
        save.clicked += Save;
        mainMenu.clicked += MainMenu;
        exitGame.clicked += ExitGame;
    }

    private void Resume() {
        //Todo:
        //Functions
        //Creating Audio, Video, & Other Scenes
        //Making Buttons Work
        //Making Button Remapping Work
        Debug.Log("Click 0");
    }
    private void Save() {
        Debug.Log("Clicked 1");
    }
    private void MainMenu() {
        Time.timeScale = 1f;
        AudioListener.volume = 1f;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        SceneManager.LoadScene("Menu");
        Debug.Log("Click 2");
    }
    private void ExitGame() {
        Application.Quit();
    }
}
