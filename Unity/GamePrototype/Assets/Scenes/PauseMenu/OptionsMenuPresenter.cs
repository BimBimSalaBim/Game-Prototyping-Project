using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OptionsMenuPresenter
{
    public Action OpenPauseMenu { set => backButton.clicked += value; }

    private Button backButton;

    public OptionsMenuPresenter(VisualElement root) {
        backButton = root.Q<Button>("Back");
    } 
}
