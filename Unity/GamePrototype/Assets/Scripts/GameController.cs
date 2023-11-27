using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    // create a timing system to save the game data every 5 seconds
    private float timer = 0.0f;
    private float saveInterval = 15.0f;
    private bool isSaving = false;
    public GameObject savingIcon;
    private GameObject clone;
    private GameObject player;


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.timer < this.saveInterval)
        {
            this.timer += Time.deltaTime;
        }
        else
        {
            this.timer = 0.0f;
            if(!this.isSaving)
            {
                if(player == null)
                {
                    player = GameObject.Find("PlayerArmature");
                }
                this.isSaving = true;
                // clone = InstantiateSavingIcon();
                StartCoroutine(SaveGame());
            }
        }
        
    }

    IEnumerator SaveGame()
    {
        // save the game data to the database
        Debug.Log("Saving Game");
        // Debug.Log(ThirdPersonController.sSingleton);
        // Database.Instance.setPosition(ThirdPersonController.sSingleton.transform.position);
        try{
            if(player != null)
            {
                Database.Instance.setPosition(player.transform.position);
            }
        }
        catch(System.Exception e)
        {
            Debug.Log(e);
        }

        yield return new WaitForSeconds(25);
        this.isSaving = false;
        Destroy(clone);
    }

    private GameObject InstantiateSavingIcon()
    {
        // Find Canvas dynamically
        Canvas canvas = FindObjectOfType<Canvas>();

        GameObject clone = Instantiate(savingIcon, new Vector3(0, 0, 0), Quaternion.identity, canvas.transform);
        Vector3 newPosition = new Vector3(40, 35, 0);
        clone.GetComponent<RectTransform>().localPosition = newPosition;
        clone.transform.SetAsLastSibling();

        // Place the icon at bottom-left corner of the Canvas
        clone.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        return clone;
    }
}