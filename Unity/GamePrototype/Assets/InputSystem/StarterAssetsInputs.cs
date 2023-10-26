using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Editor;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using System;
#endif

namespace StarterAssets {
    public class StarterAssetsInputs : MonoBehaviour {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public Vector2 interactLook;
        public bool jump;
        public bool sprint;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        [Header("PauseMenu")]
        public GameObject pauseMenu;
        private PlayerInput input;
        public event Action OnDeletePressed;

        [Header("RadialMenu")]
        public GameObject radialMenu;

        [Header("Inventory")]
        public bool openInventory = false;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        public void OnMove(InputValue value) {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value) {
            var direction = value.Get<Vector2>();
            if (cursorInputForLook) {
                LookInput(direction);
            } else {
                InteractLookInput(direction);
            }
        }

        public void OnJump(InputValue value) {
            JumpInput(value.isPressed);
        }

        public void OnSprint(InputValue value) {
            SprintInput(value.isPressed);
        }

        public void OnPause() {
            if (input == null) {
                input = GetComponent<PlayerInput>();
            }
            if (input.currentActionMap.name == "Player") {
                PauseInput();
            } else if (input.currentActionMap.name == "Menu") {
                ResumeInput();
            }
            
        }

        public void OnInteract(InputValue value) {
            GameObject gameObject = GameObject.Find("MainCamera");
            if (gameObject != null && value.isPressed) {
                gameObject.GetComponent<Interactor>().CheckInteract(radialMenu);
                if (radialMenu.activeSelf == true) {
                    cursorInputForLook = false;
                    SetCursorState(false);
                    UnityEngine.Cursor.visible = true;
                }
            } else {
                radialMenu.SetActive(false);
                cursorInputForLook = true;
                SetCursorState(true);
                UnityEngine.Cursor.visible = false;
            }
        }

        public void OnPrimaryClick() {
            GameObject gameObject = GameObject.Find("MainCamera");
            if (gameObject != null) {
                gameObject.GetComponent<Interactor>().CheckPrimary();
            }
        }

        public void  OnUseTool()
        {
            //Debug.Log("Tool in use!");
            GameObject equiqmentController = GameObject.Find("EquiqmentController");
            GameObject mineralController = GameObject.Find("Mineral");
            if (equiqmentController != null && mineralController != null)
            {

                equiqmentController.GetComponent<EquipmentController>().UseTool();
                if (mineralController.GetComponent<MineralController>()._isPlayerInRange == true)
                {
                    mineralController.GetComponent<MineralController>().SpawnGem();
                }
                
                Debug.Log("Tool in use!");
            }
        }
        public void OnSecondaryClick() {
            Debug.Log("SecondaryClick");
        }

        public void OnDodge() {
            Debug.Log("Dodge");
        }

        public void OnCrouch() {
            Debug.Log("Crouch");
        }

        public void OnInventory() {
            GameObject inventory = GameObject.Find("Inventory");
            if (inventory != null) {
                openInventory = !openInventory;
                inventory.transform.GetChild(0).gameObject.SetActive(openInventory);
                cursorInputForLook = !openInventory;
                SetCursorState(!openInventory);
                UnityEngine.Cursor.visible = openInventory;
                Time.timeScale = 1f * (openInventory ? 0.0f : 1.0f);
                AudioListener.pause = openInventory;
            }
        }

        public void OnDelete() {
            UIDocument menu = pauseMenu.GetComponent<UIDocument>();
            VisualElement optionsMenu = menu.rootVisualElement.Q<VisualElement>("OptionsMenu");
            VisualElement controlMenu = optionsMenu.Q<VisualElement>("ControlMenu");
            if (controlMenu.style.display.ToString() != "None") {
                InputActionMap actionMap = input.actions.FindActionMap("Player");
                if(actionMap != null) {
                    actionMap.RemoveAllBindingOverrides();
                    if(OnDeletePressed != null) {
                        OnDeletePressed();
                    }
                }
            }
        }
#endif

        public void MoveInput(Vector2 newMoveDirection) {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection) {
            look = newLookDirection;
        }
        public void InteractLookInput(Vector2 newLookDirection) {
            interactLook = newLookDirection;
        }

        public void JumpInput(bool newJumpState) {
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState) {
            sprint = newSprintState;
        }

        private void OnApplicationFocus(bool hasFocus) {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState) {
            UnityEngine.Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }

        public void PauseInput() {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            AudioListener.pause = true;
            SetCursorState(false);
            UnityEngine.Cursor.visible = true;
            cursorInputForLook = false;
            if (input != null) {
                input.SwitchCurrentActionMap("Menu");
            }
        }

        public void ResumeInput() {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            AudioListener.pause = false;
            SetCursorState(true);
            UnityEngine.Cursor.visible = false;
            cursorInputForLook = true;
            if (input != null) {
                input.SwitchCurrentActionMap("Player");
            }
        }
    }

}