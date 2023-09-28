using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System;
using UnityEngine.InputSystem.Utilities;
using System.Linq;
using System.Collections.Generic;

public class ControlMenuPresenter {
    private VisualElement root;
    private InputActionAsset actionAsset;
    private InputActionMap actionMap;
    private InputActionRebindingExtensions.RebindingOperation rebindOperation;

    public ControlMenuPresenter(VisualElement root, InputActionAsset actionAsset) {
        this.root = root;
        this.actionAsset = actionAsset;

        GeneralScreen();
    }

    private void GeneralScreen() {

        VisualElement controlContainer = root.Q<VisualElement>("unity-content-container");
        var upElement = GetProperties(controlContainer, "Up");
        var downElement = GetProperties(controlContainer, "Down");
        var leftElement = GetProperties(controlContainer, "Left");
        var rightElement = GetProperties(controlContainer, "Right");

        actionMap = actionAsset.FindActionMap("Player");

        InputAction moveAction = actionMap.FindAction("Move");

        var moveBindings = moveAction.bindings;

        int[] bindingIndexes = { 1, 2, 3, 4, moveBindings.Count - 1 };


        AssignCallBack(upElement.Item2, evt => Rebind(moveAction, bindingIndexes[0], upElement.Item2.Item1),
                        evt => Rebind(moveAction, bindingIndexes[4], upElement.Item2.Item1));
        AssignCallBack(downElement.Item2, evt => Rebind(moveAction, bindingIndexes[1], downElement.Item2.Item1));
        AssignCallBack(leftElement.Item2, evt => Rebind(moveAction, bindingIndexes[2], leftElement.Item2.Item1));
        AssignCallBack(rightElement.Item2, evt => Rebind(moveAction, bindingIndexes[3], rightElement.Item2.Item1));

        UpdateButtonText(upElement.Item2.Item1, moveBindings[bindingIndexes[0]]);
        UpdateButtonText(upElement.Item2.Item2, moveBindings[bindingIndexes[4]]);
        UpdateButtonText(downElement.Item2.Item1, moveBindings[bindingIndexes[1]]);
        UpdateButtonText(leftElement.Item2.Item1, moveBindings[bindingIndexes[2]]);
        UpdateButtonText(rightElement.Item2.Item1, moveBindings[bindingIndexes[3]]);


        string[] controls = { "Jump", "Sprint", "Interact", "PrimaryClick", "SecondaryClick", "Dodge" };
        foreach (string control in controls) {
            InitializeAction(control, controlContainer);
        }
    }

    private void InitializeAction(string name, VisualElement container) {
        var element = GetProperties(container, name);
        InputAction action = actionMap.FindAction(name);
        AssignCallBack(element.Item2, evt => Rebind(action, 0, element.Item2.Item1),
                evt => Rebind(action, 1, element.Item2.Item1));
        ReadOnlyArray<InputBinding> bindings = action.bindings;
        UpdateButtons(element.Item2, bindings);
    }

    private StyleColor PrepButton(InputAction action, Button button) {
        action.Disable();
        button.text = "";
        var color = button.style.backgroundColor;
        button.style.backgroundColor = Color.gray;
        return color;
    }

    private Tuple<VisualElement, Tuple<Button, Button>> GetProperties(VisualElement container, string name) {
        VisualElement element = container.Q<VisualElement>(name);
        return Tuple.Create(element, GetButtons(element));
    }

    private Tuple<Button, Button> GetButtons(VisualElement element) {
        Button keyboard = element.Q<Button>("Keyboard");
        Button gamepad = element.Q<Button>("Gamepad");
        return Tuple.Create(keyboard, gamepad);
    }

    private void AssignCallBack(Tuple<Button, Button> buttons, EventCallback<ClickEvent> callBackOne) {
        buttons.Item1.RegisterCallback(callBackOne);
    }

    private void AssignCallBack(Tuple<Button, Button> buttons, EventCallback<ClickEvent> callBackOne, EventCallback<ClickEvent> callBackTwo) {
        buttons.Item1.RegisterCallback(callBackOne);
        buttons.Item2.RegisterCallback(callBackTwo);
    }

    private void Rebind(InputAction action, int bindingIndex, Button button) {
        PerformRebind(action, bindingIndex, button);
    }

    private void PerformRebind(InputAction action, int bindingIndex, Button button) {
        StyleColor color = PrepButton(action, button);

        rebindOperation = action
            .PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.05f)
            .WithMatchingEventsBeingSuppressed()
            .OnComplete(evt => End(action, bindingIndex, button, color));
        rebindOperation.Start();
    }
    private void UpdateButtons(Tuple<Button, Button> buttons, ReadOnlyArray<InputBinding> bindings) {
        UpdateButtonText(buttons.Item1, bindings[0]);
        UpdateButtonText(buttons.Item2, bindings[1]);
    }

    private void UpdateButtonText(Button button, InputBinding binding) {
        if (button.ClassListContains("MissingControlButton")) {
            button.ToggleInClassList("MissingControlButton");
            button.AddToClassList("ControlButton");
        }
        button.text = binding.effectivePath.Split("/")[1];
    }

    private void End(InputAction action, int bindingIndex, Button button, StyleColor color) {
        rebindOperation.Dispose();
        button.style.backgroundColor = color;
        action.Enable();
        UpdateButtonText(button, action.bindings[bindingIndex]);
    }
}