using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class PlayerProfileHolder : MonoBehaviour
{
    public GameObject[] userProfiles = new GameObject[3];
    public int selectedProfile = 0;

    public void NextProfile()
    {
        Debug.Log("Button clicked.");
        userProfiles[selectedProfile].SetActive(false);
        userProfiles[selectedProfile].GetComponent<Profile>().ResetText();
        selectedProfile = (selectedProfile + 1) % userProfiles.Length;
        userProfiles[selectedProfile].SetActive(true);
        

    }

    public void PreviousProfile()
    {
        Debug.Log("Button clicked.");
        userProfiles[selectedProfile].SetActive(false);
        userProfiles[selectedProfile].GetComponent<Profile>().ResetText();
        selectedProfile--;
        if (selectedProfile < 0)
        {
            selectedProfile += userProfiles.Length;
        }
        userProfiles[selectedProfile].SetActive(true);
        
    }

    public void Sellect()
    {
        //doing some thing to the sellecting profile.
        Debug.Log("Profile sellected!");
    }
    public void View()
    {
        Debug.Log("Profile Viewer!");
        userProfiles[selectedProfile].GetComponent<Profile>().ShowStats();

    }
   
}
