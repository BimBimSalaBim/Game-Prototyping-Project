using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System;
using UnityEngine.InputSystem.Utilities;
using System.Collections.Generic;

public class ControlMenuPresenter {
    private int screen;
    private VisualElement root;
    private InputActionAsset actionAsset;
    private VisualElement generalContainer;
    private VisualElement interfaceContainer;
    private VisualElement sideBar;
    private Button generalButton;
    private Button interfaceButton;

    public ControlMenuPresenter(VisualElement root, InputActionAsset actionAsset) {
        //create video following tutorial
        //resume button
        this.root = root;
        this.actionAsset = actionAsset;
        generalContainer = root.Q<VisualElement>("GeneralControlOptions");
        interfaceContainer = root.Q<VisualElement>("InterfaceControlOptions");
        sideBar = root.Q<VisualElement>("SideBar");
        generalButton = sideBar.Q<Button>("General");
        interfaceButton = sideBar.Q<Button>("Interface");

        generalButton.clicked += GeneralScreen;
        interfaceButton.clicked += InterfaceScreen;
        GeneralScreen();
    }

    private void InterfaceScreen() {
        generalContainer.style.display = DisplayStyle.None;
        interfaceContainer.style.display = DisplayStyle.Flex;
    }

    private void GeneralScreen() {
        generalContainer.style.display = DisplayStyle.Flex;
        interfaceContainer.style.display = DisplayStyle.None;
        VisualElement controlContainer = root.Q<VisualElement>("unity-content-container");
        var upElement = getProperties(controlContainer, "Up");
        var downElement = getProperties(controlContainer, "Down");
        var leftElement = getProperties(controlContainer, "Left");
        var rightElement = getProperties(controlContainer, "Right");
        var jumpElement = getProperties(controlContainer, "Jump");
        var sprintElement = getProperties(controlContainer, "Sprint");

        InputActionMap actionMap = actionAsset.FindActionMap("Player");

        InputAction moveAction = actionAsset.FindAction("Move");
        InputAction jumpAction = actionAsset.FindAction("Jump");
        InputAction sprintAction = actionAsset.FindAction("Sprint");

        var moveBindings = moveAction.bindings;
        var jumpBindings = jumpAction.bindings;
        var sprintBindings = sprintAction.bindings;

        assignCallBack(upElement.Item2,
                        evt => RebindComposite(moveAction, upElement.Item2.Item1, "up"),
                        evt => RebindComposite(moveAction, upElement.Item2.Item1, "up", true));
        assignCallBack(downElement.Item2,
                        evt => RebindComposite(moveAction, downElement.Item2.Item1, "down"));
        assignCallBack(leftElement.Item2,
                        evt => RebindComposite(moveAction, leftElement.Item2.Item1, "left"));
        assignCallBack(rightElement.Item2,
                        evt => RebindComposite(moveAction, rightElement.Item2.Item1, "right"));
        assignCallBack(jumpElement.Item2,
                        evt => rebind(jumpAction, 0, jumpElement.Item2.Item1),
                        evt => rebind(jumpAction, 1, jumpElement.Item2.Item1));
        assignCallBack(sprintElement.Item2,
                        evt => rebind(sprintAction, 0, sprintElement.Item2.Item1),
                        evt => rebind(sprintAction, 1, sprintElement.Item2.Item1));

        updateButtons(jumpElement.Item2, jumpBindings);
        updateButtons(sprintElement.Item2, sprintBindings);
    }
    private void RebindComposite(InputAction action, Button button, string name, bool gamepad=false) {
        action.Disable();
        button.text = "";
        var color = button.style.backgroundColor;
        button.style.backgroundColor = Color.gray;

        int compositeBindingIndex = !gamepad? FindCompositeBindingIndex(action, name) : action.bindings.Count-1;

        if (compositeBindingIndex >= 0) {
            InputActionRebindingExtensions.RebindingOperation rebindOperation = action
                .PerformInteractiveRebinding(compositeBindingIndex)
                .WithControlsExcluding("Mouse")
                .WithCancelingThrough("<Keyboard>/escape")
                .OnMatchWaitForAnother(0.05f)
                .WithMatchingEventsBeingSuppressed();

            rebindOperation.OnComplete(evt => {
                button.style.backgroundColor = color;
                action.Enable();
                updateButtonText(button, action.bindings[compositeBindingIndex]);
            });
            rebindOperation.Start();
        }
    }

    private int FindCompositeBindingIndex(InputAction action, string name) {
        for (int i = 0; i < action.bindings.Count; i++) {
            var binding = action.bindings[i];
            if (binding.name == name) {
                Debug.Log(binding.name);
                Debug.Log(binding.ToString());
                return i;
            }
        }
        return -1; 
    }

    private (VisualElement, (Button, Button)) getProperties(VisualElement container, string name) {
        VisualElement element = container.Q<VisualElement>(name);
        return (element, (getButtons(element)));
    }

    private (Button, Button) getButtons(VisualElement element) {
        Button keyboard = element.Q<Button>("Keyboard");
        Button gamepad = element.Q<Button>("Gamepad");
        return (keyboard, gamepad);
    }

    private void assignCallBack((Button, Button) buttons,
                                EventCallback<ClickEvent> callBackOne) {
        buttons.Item1.RegisterCallback(callBackOne);
    }

    private void assignCallBack((Button, Button) buttons,
                                EventCallback<ClickEvent> callBackOne,
                                EventCallback<ClickEvent> callBackTwo) {
        buttons.Item1.RegisterCallback(callBackOne);
        buttons.Item2.RegisterCallback(callBackTwo);
    }

    private List<string> getKeyBinds(InputAction action, bool composite) {
        ReadOnlyArray<InputBinding> bindings = action.bindings;
        List<string> keys = new List<string>();
        foreach (var binding in bindings) {
            keys.Add(binding.path);
            keys.Add(binding.name);
        }
        return keys;
    }

    private void rebind(InputAction action, int bindingIndex, Button button) {
        performRebind(action, bindingIndex, button);
        var bindings = action.bindings;
        updateButtonText(button, bindings[bindingIndex]);
    }

    private void performRebind(InputAction action, int bindingIndex, Button button) {
        ReadOnlyArray<InputBinding> bindings = action.bindings;
        action.Disable();
        button.text = "";
        var color = button.style.backgroundColor;
        button.style.backgroundColor = Color.gray;
        InputActionRebindingExtensions.RebindingOperation rebind = action.PerformInteractiveRebinding(bindingIndex)
        .WithControlsExcluding("Mouse")
        .WithCancelingThrough("<Keyboard>/escape")
        .OnMatchWaitForAnother(0.05f)
        .WithMatchingEventsBeingSuppressed()
        .OnComplete(evt => End(action, bindings, bindingIndex, button, color));
        rebind.Start();
    }

    private void updateButtons((Button, Button) buttons, ReadOnlyArray<InputBinding> bindings) {
        updateButtonText(buttons.Item1, bindings[0]);
        updateButtonText(buttons.Item2, bindings[1]);
    }

    private void updateButtonText(Button button, InputBinding binding) {
        button.text = binding.path.Split("/")[1];
    }

    private void End(InputAction action, ReadOnlyArray<InputBinding> bindings, int bindingIndex, Button button, StyleColor color) {
        button.style.backgroundColor = color;
        action.Enable();
    }
}