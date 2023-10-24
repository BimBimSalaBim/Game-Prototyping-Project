using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System;
using UnityEngine.InputSystem.Utilities;
using StarterAssets;
using System.Collections.Generic;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;
using System.Text.RegularExpressions;

public class ControlMenuPresenter {
    private InputActionMap actionMap;
    private VisualElement controlContainer;
    private RebindingOperation rebindOperation;
    private Dictionary<int, string> effectivePaths;

    public ControlMenuPresenter(VisualElement root, InputActionAsset actionAsset) {
        GameObject gameObject = GameObject.Find("PlayerArmature");
        if (gameObject != null) {
            StarterAssetsInputs input = gameObject.GetComponent<StarterAssetsInputs>();
            input.OnDeletePressed += () => refreshButtons();
        }
        controlContainer = root.Q<VisualElement>("unity-content-container");
        actionMap = actionAsset.FindActionMap("Player");
        effectivePaths = new Dictionary<int, string>();
        updateEffectivePaths();
        GeneralScreen();
    }

    private void addToControls(string labelText, InputBinding firstBinding, InputBinding secondBinding, InputAction action, 
        int firstIndex, int secondIndex, bool hasSecondBinding) {
        VisualElement newRow = new();
        newRow.AddToClassList("ControlRow");

        Label label = new() {
            text = Regex.Replace(char.ToUpper(labelText[0]) + labelText[1..], "([a-z])([A-Z])", "$1 $2")
        };
        label.AddToClassList("ControlLabel");
        label.AddToClassList("controlLabelText");
        newRow.Add(label);
        
        Button keyboardButton = new() {
            text = firstBinding.effectivePath.Split("/")[1]
        };
        keyboardButton.AddToClassList("NavButton");
        keyboardButton.AddToClassList("ControlButton");
        keyboardButton.AddToClassList("ControlButtonFormat");
        newRow.Add(keyboardButton);

        Button gamepadButton = new() {
            text = hasSecondBinding ? secondBinding.effectivePath.Split("/")[1] : ""
        };
        if (hasSecondBinding) {
            gamepadButton.AddToClassList("NavButton");
            gamepadButton.AddToClassList("ControlButton");
        } else {
            gamepadButton.AddToClassList("MissingControlButton");
        }
        gamepadButton.AddToClassList("ControlButtonFormat");
        newRow.Add(gamepadButton);

        if(hasSecondBinding) {
            AssignCallBack(keyboardButton, gamepadButton, evt => PerformRebind(action, firstIndex, keyboardButton, 
                firstBinding.effectivePath.Contains("Mouse")),
                evt => PerformRebind(action, secondIndex, gamepadButton, 
                firstBinding.effectivePath.Contains("Mouse")));
        } else {
            AssignCallBack(keyboardButton, evt => PerformRebind(action, firstIndex, keyboardButton,
                firstBinding.effectivePath.Contains("Mouse")));
        }

        controlContainer.Add(newRow);
    }

    private void GeneralScreen() {
        VisualElement gameControls = controlContainer[1];
        controlContainer.RemoveAt(1);

        List<List<InputBinding>> bindingPairs = new();
        foreach (var action in actionMap.actions) {
            ReadOnlyArray<InputBinding> bindings = action.bindings;
            if (bindings.Count > 2) {
                bindingPairs.Add((new List<InputBinding> { bindings[1], bindings[^1] }));
                addToControls(bindings[1].name, bindings[1], bindings[^1], action, 1, bindings.Count-1, true);
                for (int j = 2; j < bindings.Count - 1; j++) {
                    bindingPairs.Add(new List<InputBinding> { bindings[j] });
                    addToControls(bindings[j].name, bindings[j], new InputBinding(), action, j, -1, false);
                }
            } else {
                if (action.name.Equals("Pause") || action.name.Equals("Look")) continue;
                bindingPairs.Add(new List<InputBinding> { bindings[0], bindings[1] });
                addToControls(action.name, bindings[0], bindings[1], action, 0, 1, true);
            }
        }

        controlContainer.Insert(9, gameControls);
    }

    private void refreshButtons() {
        VisualElement movementLabel = controlContainer[0];
        VisualElement gameControls = controlContainer[9];
        controlContainer.Clear();
        controlContainer.Add(movementLabel);
        controlContainer.Add(gameControls);
        actionMap.RemoveAllBindingOverrides();
        GeneralScreen();
        updateEffectivePaths();
    }

    private void updateEffectivePaths() {
        for(int i = 0; i < actionMap.bindings.Count; i++) {
            effectivePaths[i] = actionMap.bindings[i].effectivePath;
        }
    }

    private void AssignCallBack(Button button, EventCallback<ClickEvent> callBackOne) {
        button.RegisterCallback(callBackOne);
    }

    private void AssignCallBack(Button buttonOne, Button buttonTwo, EventCallback<ClickEvent> callBackOne, EventCallback<ClickEvent> callBackTwo) {
        buttonOne.RegisterCallback(callBackOne);
        buttonTwo.RegisterCallback(callBackTwo);
    }

    private void PerformRebind(InputAction action, int bindingIndex, Button button, bool mouse) {
        button.ToggleInClassList("MissingControlButton");
        button.ToggleInClassList("ControlButton");
        action.Disable();

        if (rebindOperation != null) {
            rebindOperation.Cancel();
            rebindOperation = null;
            Debug.Log(action.bindings[bindingIndex]);
        }

        rebindOperation = action
            .PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding(mouse ? "<Keyboard>" : "<Mouse>")
            .WithControlsExcluding("<Pointer>/delta")
            .WithControlsExcluding("<Pointer>/position")
            .WithControlsExcluding("<Touchscreen>/touch*/position")
            .WithControlsExcluding("<Touchscreen>/touch*/delta")
            .WithControlsExcluding("<Keyboard>/backspace")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnCancel(evt => Cancel(action, bindingIndex, button))
            .OnMatchWaitForAnother(0.05f)
            .WithMatchingEventsBeingSuppressed()
            .OnComplete(evt => End(action, bindingIndex, button));
        rebindOperation.Start();
    }

    private void Cancel(InputAction action, int bindingIndex, Button button) {
        action.Enable();
        button.ToggleInClassList("MissingControlButton");
        button.ToggleInClassList("ControlButton");
        UpdateButtonText(button, action.bindings[bindingIndex]);
        rebindOperation?.Dispose();
        rebindOperation = null;
    }

    private void UpdateButtonText(Button button, InputBinding binding) {
        button.ToggleInClassList("MissingControlButton");
        button.ToggleInClassList("ControlButton");
        button.text = binding.effectivePath.Split("/")[1];
    }

    private void End(InputAction action, int bindingIndex, Button button) {
        rebindOperation.Dispose();
        if (action.bindings[bindingIndex].effectivePath == "<Keyboard>/anyKey") {
            action.RemoveBindingOverride(bindingIndex);
        }
        action.Enable();
        var index = checkDuplicates(action, bindingIndex);
        effectivePaths[index] = action.bindings[bindingIndex].effectivePath;
        UpdateButtonText(button, action.bindings[bindingIndex]);
    }

    private int checkDuplicates(InputAction action, int bindingIndex) {
        string currentEffectivePath = action.bindings[bindingIndex].effectivePath;
        int index = 0;
        for(int i = 0; i < actionMap.bindings.Count; i++) {
            if (actionMap.bindings[i].path == action.bindings[bindingIndex].path) {
                index = i;
                break;
            }
        }
        foreach (var kvp in effectivePaths) {
            if (kvp.Key != index && kvp.Value == currentEffectivePath) {
                action.RemoveBindingOverride(bindingIndex);
            }
        }
        return index;
    }
}