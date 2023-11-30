using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using static Interactor;
using System.Collections;
using Unity.VisualScripting;


public class GenericAnimal : MonoBehaviour, IAnimal {

    [SerializeField] private Item item;
    [SerializeField] private CollectibleItem collectibleItem;

    public string Name { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }

    public bool statsEnabled { get; set; }

    public GameObject canvas;
    public ActionType feedType = ActionType.FeedHerbivore;
    private FoV fov;

    private Entity entity;

    private GameObject commandMenu;


    public void Start() {
        statsEnabled = false;
        Transform lParent = transform;
        while(lParent.parent != null)
        {
            lParent = lParent.parent;
        }
        Transform lCanvas = lParent.Find("StatCanvas");
        if(lCanvas != null)
        {
            canvas = lCanvas.gameObject;
        }
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
        entity = lParent.GetComponent<Entity>();
        fov = lParent.GetComponent<FoV>();
        collectibleItem = GetComponent<CollectibleItem>();
        if (collectibleItem != null)
        {
            collectibleItem.Initialize(item);
        }
    }

    public void Update() {
        //check if command menu is open

        if (statsEnabled) {

            //find the PlayerArmature and set the rotation of canvas to face the player
            if (canvas != null) {
                GameObject MainCamera = GameObject.Find("MainCamera");
                canvas.transform.LookAt(MainCamera.transform);
            }
        } else if(canvas != null) {
            canvas.SetActive(false);
        }
    }
    public void Interact() {

        var buttons = new List<Button>();
        foreach (var item in commandMenu.GetComponentsInChildren<Button>()) {
            buttons.Add(item);
        }
        if (buttons.Count >= 4) {
            buttons[0].GetComponentInChildren<Text>().text = "Feed";
            buttons[0].onClick.AddListener(FeedOnClick);

            buttons[1].GetComponentInChildren<Text>().text = "View Stats";
            buttons[1].onClick.AddListener(ViewStatsOnClick);

            buttons[2].GetComponentInChildren<Text>().text = "Tame";
            buttons[2].onClick.AddListener(TameOnClick);

            buttons[3].GetComponentInChildren<Text>().text = "Stay";
            buttons[3].onClick.AddListener(StayOnClick);
        }

        commandMenu.SetActive(true);
    }


    public void ActivateCollectibleItem()
    {
        if (collectibleItem != null)
        {
            Debug.Log("Dead");
            // StartCoroutine(collectibleItem.MoveAndCollect());
            entity.mSpeed = 0;
            entity.mHealth = 0;
            try{
                fov.gameObject.GetComponent<Animator>().enabled = false;
            }
            catch{
                Debug.Log("No animator");
            }
            fov.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            fov.enabled = false;
        }
    }
    void ViewStatsOnClick() {
        Debug.Log("Stats button clicked");
        //get child of panel in canvas and set the text to the stats of the animal
        GameObject panel = canvas.transform.Find("statsPanel").gameObject;
        TMPro.TextMeshProUGUI text = panel.transform.Find("CreatureType").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        
        

        text.text = "Creature Name: " + fov.GetCreatureType() +
                    "\nHealth: " + entity.mHealth +
                    "\nMax Health: " + entity.mMaxHealth +
                    "\nSpeed: " + entity.mSpeed +
                    "\nJump Height: " + entity.mJumpHeight +
                    "\nStamina: " + entity.mStamina +
                    "\nStrength: " + entity.mStrength +
                    "\nHunger: " + entity.mHunger +
                    "\nInventory Slots: " + entity.mInventory_Slots;
        canvas.SetActive(true);
        statsEnabled = true;
        StartCoroutine(UpdateTest(text));

    }

     IEnumerator UpdateTest(TMPro.TextMeshProUGUI text){
        while (statsEnabled != false){
        Debug.Log("Updating stats");
        
        yield return new WaitForSeconds(1);
        text.text = "Creature Name: " + fov.GetCreatureType() +
                            "\nHealth: " + entity.mHealth +
                            "\nMax Health: " + entity.mMaxHealth +
                            "\nSpeed: " + entity.mSpeed +
                            "\nJump Height: " + entity.mJumpHeight +
                            "\nStamina: " + entity.mStamina +
                            "\nStrength: " + entity.mStrength +
                            "\nHunger: " + entity.mHunger +
                            "\nInventory Slots: " + entity.mInventory_Slots;
        }
     }



    void TameOnClick() {
        Debug.Log("Tame button clicked");
    }

    void StayOnClick() {
        Debug.Log("Stay button clicked");
    }

    public void CommandMenu(GameObject radialMenu) {
        commandMenu = radialMenu;
    }

    public void FeedOnClick() {
        var (name, type, actionType) = InventoryManager.instance.CheckSelectedItem();
        if (type == ItemType.Food && actionType.Equals(actionType)) {
            InventoryManager.instance.GetSelectedItem(true);
            Debug.Log("Nom nom nom");
            Debug.Log(string.Format("Ate a {0}", name));
            //change entity stats 
            if (entity.mHunger + 10 > entity.mMaxHealth) {
                entity.mHunger = entity.mMaxHealth;
            } else{
            entity.mHunger += 10;
            }
        }
    }

    public void PrimaryInteract() {
        Debug.Log("Oww");
        fov.TakeDamage(10, transform.position);
        fov.Agitate(GameObject.Find("PlayerArmature"));
        if (entity.mHealth <= 0) {
            Debug.Log("Dead");
            StartCoroutine(collectibleItem.MoveAndCollect());
        }
    }
}