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
    private VisualElement root;
    public PauseMenuPresenter(VisualElement root) {
        this.root = root;
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
        Time.timeScale = 1f;
        AudioListener.volume = 1f;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = false;
        GameObject gameObject = GameObject.Find("PauseMenu");
        PlayerPrefs.SetInt("Paused", 0);
        PlayerPrefs.Save();
        gameObject.SetActive(false);
    }
    private void Save() {
        //todo
    }
    private void MainMenu() {
        Time.timeScale = 1f;
        AudioListener.volume = 1f;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
        SceneManager.LoadScene("Menu");
        PlayerPrefs.DeleteKey("Paused");
    }
    private void ExitGame() {
        Application.Quit();
    }
}
