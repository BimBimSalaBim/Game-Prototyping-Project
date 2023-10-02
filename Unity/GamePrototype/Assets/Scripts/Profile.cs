using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour
{
    public string Name;
    public int Level;
    public GameObject Character;
    //public string originText;


    public void ShowStats()
    {
        string messageOut = "";
        
        messageOut = "Profile name: " + "\n" +  Name.ToString() + "\n" + "Level: " + Level;
        
        this.GetComponent<TextMesh>().text = messageOut;
    }
    public void Start()
    {
        //this.GetComponent<TextMesh>().text = this.name;
    }
    public void ResetText()
    {
        this.GetComponent<TextMesh>().text = this.name;
    }
}

