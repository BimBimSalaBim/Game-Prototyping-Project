using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
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
        public bool paused = false;
        public GameObject pauseMenu;

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

        public void OnPause(InputValue value) {
            PauseInput();
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
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }

        public void PauseInput() {
            paused = !paused;
            if (paused) {
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;
                AudioListener.volume = 0f;
                SetCursorState(false);
                Cursor.visible = true;
            } else {
                pauseMenu.SetActive(false);
                Time.timeScale = 1f;
                AudioListener.volume = 1f;
                SetCursorState(true);
                Cursor.visible = false;
            }
        }
    }

}