using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class OptionsMenuPresenter
{
    public Action OpenPauseMenu { set => backButton.clicked += value; }

    private VisualElement audioMenu;
    private VisualElement videoMenu;
    private VisualElement controlMenu;
    private VisualElement gamePlayMenu;

    private Button backButton;
    private Button audio;
    private Button video;
    private Button control;
    private Button gameplay;

    private InputActionAsset actionAsset;

    public OptionsMenuPresenter(VisualElement root, InputActionAsset actionAsset) {
        audioMenu = root.Q("AudioMenu");
        videoMenu = root.Q("VideoMenu");
        controlMenu = root.Q("ControlMenu");
        gamePlayMenu = root.Q("GamePlayMenu");

        backButton = root.Q<Button>("Back");
        audio = root.Q<Button>("Audio");
        video = root.Q<Button>("Video");
        control = root.Q<Button>("Control");
        gameplay = root.Q<Button>("GamePlay");

        backButton.clicked += Audio;
        audio.clicked += Audio;
        video.clicked += Video;
        control.clicked += Control;
        gameplay.clicked += GamePlay;

        this.actionAsset = actionAsset;
        Audio();
    } 

    private void Audio() {
        AudioMenuPresenter menuPresenter = new AudioMenuPresenter(audioMenu);
        audioMenu.style.display = DisplayStyle.Flex;
        videoMenu.style.display = DisplayStyle.None;
        controlMenu.style.display = DisplayStyle.None;
        gamePlayMenu.style.display = DisplayStyle.None;
    }
    private void Video() {
        VideoMenuPresenter menuPresenter = new VideoMenuPresenter(videoMenu);
        audioMenu.style.display = DisplayStyle.None;
        videoMenu.style.display = DisplayStyle.Flex;
        controlMenu.style.display = DisplayStyle.None;
        gamePlayMenu.style.display = DisplayStyle.None;
    }
    private void Control() {
        ControlMenuPresenter menuPresenter = new ControlMenuPresenter(controlMenu, actionAsset);
        audioMenu.style.display = DisplayStyle.None;
        videoMenu.style.display = DisplayStyle.None;
        controlMenu.style.display = DisplayStyle.Flex;
        gamePlayMenu.style.display = DisplayStyle.None;
    }
    private void GamePlay() {
        GamePlayMenuPresenter menuPresenter = new GamePlayMenuPresenter(gamePlayMenu);
        audioMenu.style.display = DisplayStyle.None;
        videoMenu.style.display = DisplayStyle.None;
        controlMenu.style.display = DisplayStyle.None;
        gamePlayMenu.style.display = DisplayStyle.Flex;
    }
    private void SetFullScreen(bool enabled) {

    }
}
