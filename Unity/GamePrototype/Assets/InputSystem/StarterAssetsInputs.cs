using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
#endif

namespace StarterAssets {
    public class StarterAssetsInputs : MonoBehaviour {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
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

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        public void OnMove(InputValue value) {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value) {
            if (cursorInputForLook) {
                LookInput(value.Get<Vector2>());
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

        public void OnInteract() {
            Debug.Log("Interact");
        }

        public void OnPrimaryClick() {
            Debug.Log("PrimaryClick");
        }

        public void OnSecondaryClick() {
            Debug.Log("SecondaryClick");
        }

        public void OnDodge() {
            Debug.Log("Dodge");
        }

        public void OnDelete() {
            UIDocument menu = pauseMenu.GetComponent<UIDocument>();
            VisualElement optionsMenu = menu.rootVisualElement.Q<VisualElement>("OptionsMenu");
            VisualElement controlMenu = optionsMenu.Q<VisualElement>("ControlMenu");
            if (controlMenu.style.display.ToString() != "None") {
                var actions = input.actions;
                foreach(var action in actions) {
                    if(action.actionMap.name == "Player") {
                        action.RemoveAllBindingOverrides();
                    }
                }
                Debug.Log("Delete");
            }
        }
#endif

        public void MoveInput(Vector2 newMoveDirection) {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection) {
            look = newLookDirection;
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