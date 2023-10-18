using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using static Interactor;

public class GenericAnimal : MonoBehaviour, IAnimal {
    public string Name { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }

    public bool statsEnabled { get; set;}

    public GameObject canvas;
    private FoV fov;

    private Entity entity;

    private GameObject commandMenu;

    public void Start() {
        statsEnabled = false;
        canvas = this.transform.parent.Find("Canvas").gameObject;
        entity = this.GetComponent<Entity>();
        canvas.SetActive(false);
    }

    public void Update() {
        //check if command menu is open
        
        if(statsEnabled){
            
            //find the PlayerArmature and set the rotation of canvas to face the player
            GameObject MainCamera = GameObject.Find("MainCamera");
            canvas.transform.LookAt(MainCamera.transform);
        }
        else{
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



    void FeedOnClick() {
        Debug.Log("Nom Nom Nom");
    }

    void ViewStatsOnClick() {
        Debug.Log("Stats button clicked");
        //get child of panel in canvas and set the text to the stats of the animal
        GameObject panel = canvas.transform.Find("statsPanel").gameObject;
        TMPro.TextMeshProUGUI text = panel.transform.Find("CreatureType").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        fov = this.transform.parent.GetComponent<FoV>();
        entity = this.transform.parent.GetComponent<Entity>();
        //temp set all stats to 100
        int mHealth = 100;
        int mMaxHealth = 100;
        int mSpeed = 100;
        int mJumpHeight = 100;
        int mStamina = 100;
        int mStrength = 100;
        int mHunger = 100;
        int mInventory_Slots = 100;

        text.text = "Creature Name: "+ fov.getCreatureType() +
                    "\nHealth: " + mHealth +
                    "\nMax Health: " + mMaxHealth +
                    "\nSpeed: " + mSpeed +
                    "\nJump Height: " + mJumpHeight +
                    "\nStamina: " + mStamina +
                    "\nStrength: " + mStrength +
                    "\nHunger: " + mHunger +
                    "\nInventory Slots: " + mInventory_Slots;

        canvas.SetActive(true);
        statsEnabled = true;

    }



    void TameOnClick() {
        Debug.Log("Tame button clicked");
    }

    void StayOnClick() {
        Debug.Log("Stay button clicked");
    }
public List<IResource> DropResources() {
        List<IResource> resources = new List<IResource>();
        return resources;
    }

    public void CommandMenu(GameObject radialMenu) {
        commandMenu = radialMenu;
    }
}