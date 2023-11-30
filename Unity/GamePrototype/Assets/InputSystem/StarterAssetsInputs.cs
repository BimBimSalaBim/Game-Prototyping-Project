using Unity.VisualScripting;
using UnityEngine;
// using UnityEngine.InputSystem.Editor;
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

        [Header("Inventory")]
        public bool interactMenuOpen = false;

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
                    interactMenuOpen = true;
                }
            } else {
                interactMenuOpen = false;
                radialMenu.SetActive(false);
                cursorInputForLook = true;
                SetCursorState(true);
                UnityEngine.Cursor.visible = false;
            }

            GameObject player = GameObject.FindWithTag("Player");
            GameObject gemStone = GameObject.FindWithTag("GemStone");
            GameObject wood = GameObject.FindWithTag("Wood");
            if (player != null && gemStone != null) {
                player.GetComponent<PickUpAsset>().toInventory(gemStone);
                gemStone.GetComponent<MineralStone>().pickUpAsset();
                Debug.Log("Pick Up Asset");
            }
            if (player != null && wood != null) {
                player.GetComponent<PickUpAsset>().toInventory(wood);
                wood.GetComponent<WoodResource>().pickUpAsset();
                Debug.Log("Pick Up Asset");
            }
        }

        public void OnPrimaryClick() {
            GameObject player = GameObject.FindWithTag("Player");
            GameObject gameObject = GameObject.Find("MainCamera");
            if (interactMenuOpen) {
                return;
            }
            if (gameObject != null) {
                gameObject.GetComponent<Interactor>().CheckPrimary();
            }
            if (InventoryManager.instance.CheckSelectedItem().Item2 == ItemType.Tool) {
                GameObject equiqmentController = GameObject.FindWithTag("EquiqmentController");
               
                
                GameObject[] mineralFields = GameObject.FindGameObjectsWithTag("MineralField");
                GameObject[] woodFields = GameObject.FindGameObjectsWithTag("WoodField");
                GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
                equiqmentController.GetComponent<EquipmentController>().UseTool();

                GameObject closestMineralField = null;
                GameObject closestWoodField = null;
                GameObject closestTree = null;
                float minDistance = float.MaxValue;

                foreach (GameObject tree in trees)
                {
                    float distance = Vector3.Distance(player.transform.position, tree.transform.position);
                    if (distance < minDistance)
                    {
                        closestTree = tree;
                        minDistance = distance;
                    }
                }
                foreach (GameObject mineralField in mineralFields)
                {
                    float distance = Vector3.Distance(player.transform.position, mineralField.transform.position);
                    if (distance < minDistance)
                    {
                        closestMineralField = mineralField;
                        minDistance = distance;
                    }
                }
                foreach (GameObject woodField in woodFields)
                {
                    float distance = Vector3.Distance(player.transform.position, woodField.transform.position);
                    if (distance < minDistance)
                    {
                        closestWoodField = woodField;
                        minDistance = distance;
                    }
                }


                if (equiqmentController != null && closestMineralField != null && closestMineralField.GetComponent<MineralController>()._isPlayerInRange == true) {
                    if (InventoryManager.instance.CheckSelectedItem().Item2 != ItemType.Tool || InventoryManager.instance.CheckSelectedItem().Item3 != ActionType.Mine) {
                        Debug.Log("Wrong tool");
                        return;
                    }
                    closestMineralField.GetComponent<MineralController>().SpawnGem();
                    Debug.Log("Tool in use to make mineral!");
                }
                if (equiqmentController != null && closestWoodField != null && closestWoodField.GetComponent<WoodMiinerController>()._isPlayerInRange == true) {
                    if (InventoryManager.instance.CheckSelectedItem().Item2 != ItemType.Tool || InventoryManager.instance.CheckSelectedItem().Item3 != ActionType.Cut) {
                        Debug.Log("Wrong tool");
                        Debug.Log(InventoryManager.instance.CheckSelectedItem().Item3);
                        return;
                    }
                    closestWoodField.GetComponent<WoodMiinerController>().SpawnWood();
                    Debug.Log("Tool in use to make wood!");
                }
                try{
                    if (equiqmentController != null && closestTree != null && closestTree.GetComponent<TreeCutDown>()._isPlayerInRange == true)
                    {
                        if (InventoryManager.instance.CheckSelectedItem().Item2 != ItemType.Tool || InventoryManager.instance.CheckSelectedItem().Item3 != ActionType.Cut)
                        {
                            Debug.Log("Cutting Down Tree");
                            Debug.Log(InventoryManager.instance.CheckSelectedItem().Item3);
                            return;
                        }
                        closestTree.GetComponent<TreeCutDown>().CutTree();
                        Debug.Log("Tool in use!");
                    }
                    else
                    {
                        return;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("No Tree");
                }
                
            }
        }
        public void OnSecondaryClick() {
            //OnUseTool();
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
                inventory.transform.GetChild(1).gameObject.SetActive(openInventory);
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