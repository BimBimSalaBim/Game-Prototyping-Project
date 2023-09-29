using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class VideoMenuPresenter {
    private Toggle fullScreenToggle;
    private DropdownField resolutionSelection;
    private List<string> resolutions = new List<string>() {
        "3840x2160",
        "2560x1440",
        "1920x1080",
        "1600x900",
        "1366x768",
        "1280x720"
    };

    private const string FullScreenKey = "FullscreenToggle";
    private const string ResolutionIndexKey = "ResolutionIndex";

    public VideoMenuPresenter(VisualElement root) {
        fullScreenToggle = root.Q<Toggle>();
        fullScreenToggle.value = true;
        fullScreenToggle.RegisterCallback<MouseUpEvent>(evt => fullScreen(fullScreenToggle.value));

        resolutionSelection = root.Q<DropdownField>();
        resolutionSelection.choices = resolutions;
        resolutionSelection.RegisterValueChangedCallback<string>(value => SetResolution(value.newValue));

        LoadSavedValues();
    }

    private void LoadSavedValues() {
        if (PlayerPrefs.HasKey(FullScreenKey)) {
            bool savedValue = PlayerPrefs.GetInt(FullScreenKey) == 1;
            fullScreenToggle.value = savedValue;
        }

        if (PlayerPrefs.HasKey(ResolutionIndexKey)) {
            int index = PlayerPrefs.GetInt(ResolutionIndexKey);
            resolutionSelection.index = index;
        }
    }

    private void SetResolution(string newResolution) {
        string[] resolution = newResolution.Split('x');
        int[] values = new int[] { int.Parse(resolution[0]), int.Parse(resolution[1]) };
        int index = resolutionSelection.index;
        PlayerPrefs.SetInt(ResolutionIndexKey, index);
        PlayerPrefs.Save();

        Screen.SetResolution(values[0], values[1], fullScreenToggle.value);
    }

    private void fullScreen(bool enabled) {
        Debug.Log(enabled);
        int fullscreenValue = enabled ? 1 : 0;
        PlayerPrefs.SetInt(FullScreenKey, fullscreenValue);
        PlayerPrefs.Save();

        Screen.fullScreen = enabled;
    }
}