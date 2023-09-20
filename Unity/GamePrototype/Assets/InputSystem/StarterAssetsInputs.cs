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
            bool paused;
            if (PlayerPrefs.HasKey("Paused")) {
                paused = PlayerPrefs.GetInt("Paused") == 1;
            } else {
                paused = false;
            }
            if (!paused) {
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;
                AudioListener.pause = true;
                SetCursorState(false);
                Cursor.visible = true;
                PlayerPrefs.SetInt("Paused", 1);
                PlayerPrefs.Save();
            } else {
                pauseMenu.SetActive(false);
                Time.timeScale = 1f;
                AudioListener.pause = false;
                SetCursorState(true);
                Cursor.visible = false;
                PlayerPrefs.SetInt("Paused", 0);
                PlayerPrefs.Save();
            }
        }
    }

}