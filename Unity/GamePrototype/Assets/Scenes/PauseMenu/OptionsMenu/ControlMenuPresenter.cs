using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System;
using UnityEngine.InputSystem.Utilities;
using StarterAssets;
using System.Collections.Generic;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class ControlMenuPresenter {
    private InputActionMap actionMap;
    private InputAction moveAction;
    private VisualElement controlContainer;
    private RebindingOperation rebindOperation;
    private Tuple<Button, Button> upElement;
    private Tuple<Button, Button> downElement;
    private Tuple<Button, Button> leftElement;
    private Tuple<Button, Button> rightElement;
    private Dictionary<string, bool> controls = new() {
        { "Jump", false },
        { "Sprint", false },
        { "Interact", false },
        { "PrimaryClick", true },
        { "SecondaryClick", true },
        { "Dodge", false }
    };

    private Dictionary<int, string> effectivePaths;

    public ControlMenuPresenter(VisualElement root, InputActionAsset actionAsset) {
        GameObject gameObject = GameObject.Find("PlayerArmature");
        if (gameObject != null) {
            StarterAssetsInputs input = gameObject.GetComponent<StarterAssetsInputs>();
            input.OnDeletePressed += () => refreshButtons();
        }
        controlContainer = root.Q<VisualElement>("unity-content-container");
        upElement = GetProperties(controlContainer, "Up");
        downElement = GetProperties(controlContainer, "Down");
        leftElement = GetProperties(controlContainer, "Left");
        rightElement = GetProperties(controlContainer, "Right");
        actionMap = actionAsset.FindActionMap("Player");
        moveAction = actionMap.FindAction("Move");
        effectivePaths = new Dictionary<int, string>();
        updateEffectivePaths();

        GeneralScreen();
    }

    private void GeneralScreen() {
        var moveBindings = moveAction.bindings;
        int[] bindingIndexes = { 1, 2, 3, 4, moveBindings.Count - 1 };

        AssignCallBack(upElement, evt => PerformRebind(moveAction, bindingIndexes[0], upElement.Item1),
                        evt => PerformRebind(moveAction, bindingIndexes[4], upElement.Item2));
        AssignCallBack(downElement, evt => PerformRebind(moveAction, bindingIndexes[1], downElement.Item1));
        AssignCallBack(leftElement, evt => PerformRebind(moveAction, bindingIndexes[2], leftElement.Item1));
        AssignCallBack(rightElement, evt => PerformRebind(moveAction, bindingIndexes[3], rightElement.Item1));

        updateComposite(moveBindings, bindingIndexes);
        updateNonCompositeControls(controls, true);
    }

    private void refreshButtons() {
        var moveBindings = moveAction.bindings;
        int[] bindingIndexes = { 1, 2, 3, 4, moveBindings.Count - 1 };

        updateComposite(moveBindings, bindingIndexes);
        updateNonCompositeControls(controls, false);
        updateEffectivePaths();
    }

    private void InitializeAction(KeyValuePair<string, bool> pair, VisualElement container, bool assignCallBacks) {
        var element = GetProperties(container, pair.Key);
        InputAction action = actionMap.FindAction(pair.Key);
        if(assignCallBacks) {
            AssignCallBack(element, evt => PerformRebind(action, 0, element.Item1, pair.Value),
                evt => PerformRebind(action, 1, element.Item2, pair.Value));
        }
        ReadOnlyArray<InputBinding> bindings = action.bindings;
        UpdateButtons(element, bindings);
    }

    private void updateComposite(ReadOnlyArray<InputBinding> moveBindings, int[] bindingIndexes) {
        UpdateButtonText(upElement.Item1, moveBindings[bindingIndexes[0]]);
        UpdateButtonText(upElement.Item2, moveBindings[bindingIndexes[4]]);
        UpdateButtonText(downElement.Item1, moveBindings[bindingIndexes[1]]);
        UpdateButtonText(leftElement.Item1, moveBindings[bindingIndexes[2]]);
        UpdateButtonText(rightElement.Item1, moveBindings[bindingIndexes[3]]);
    }

    private void updateEffectivePaths() {
        for(int i = 0; i < actionMap.bindings.Count; i++) {
            effectivePaths[i] = actionMap.bindings[i].effectivePath;
        }
    }

    private void updateNonCompositeControls(Dictionary<string, bool> controls, bool assignCallBacks) {
        foreach (var control in controls) {
            InitializeAction(control, controlContainer, assignCallBacks);
        }
    }

    private StyleColor PrepButton(InputAction action, Button button) {
        action.Disable();
        button.text = "";
        var color = button.style.backgroundColor;
        button.style.backgroundColor = Color.gray;
        return color;
    }

    private Tuple<Button, Button> GetProperties(VisualElement container, string name) {
        return GetButtons(container.Q<VisualElement>(name));
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

    private void PerformRebind(InputAction action, int bindingIndex, Button button, bool mouse=false) {
        StyleColor color = PrepButton(action, button);

        if (rebindOperation != null) {
            rebindOperation.Cancel();
            rebindOperation = null;
            action.Disable();
        }

        rebindOperation = action
            .PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding(mouse ? "<Keyboard>" : "<Mouse>")
            .WithControlsExcluding("<Pointer>/delta")
            .WithControlsExcluding("<Pointer>/position")
            .WithControlsExcluding("<Touchscreen>/touch*/position")
            .WithControlsExcluding("<Touchscreen>/touch*/delta")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnCancel(evt => Cancel(action, bindingIndex, color, button))
            .OnMatchWaitForAnother(0.05f)
            .WithMatchingEventsBeingSuppressed()
            .OnComplete(evt => End(action, bindingIndex, button, color));
        rebindOperation.Start();
    }

    private void Cancel(InputAction action, int bindingIndex, StyleColor color, Button button) {
        action.Enable();
        button.style.backgroundColor = color;
        UpdateButtonText(button, action.bindings[bindingIndex]);
        rebindOperation?.Dispose();
        rebindOperation = null;
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