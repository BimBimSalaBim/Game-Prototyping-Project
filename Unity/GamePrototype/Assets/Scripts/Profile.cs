using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Profile : MonoBehaviour
{
    public string Name;
    public int Level;
    public GameObject Character;
    public void ShowStats()
    {
        string messageOut = "";
        messageOut = "Profile name: " + "\n" +  Name.ToString() + "\n" + "Level: " + Level;
        GetComponent<TextMesh>().text = messageOut;
    }
}

