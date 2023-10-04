using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;

public class PlayerProfileHolder : MonoBehaviour
{
    public GameObject[] userProfiles = new GameObject[3];
    public int selectedProfile = 0;
    public GameObject Canvas;

    public void NextProfile()
    {
        Debug.Log("Button clicked.");
        userProfiles[selectedProfile].SetActive(false);
        userProfiles[selectedProfile].GetComponent<Profile>().ResetText();
        selectedProfile = (selectedProfile + 1) % userProfiles.Length;
        userProfiles[selectedProfile].SetActive(true);
        userProfiles[selectedProfile].GetComponent<Profile>().Character.SetActive(false);
        //userProfiles[selectedProfile].GetComponent<Profile>().Demo();


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
        userProfiles[selectedProfile].GetComponent<Profile>().Character.SetActive(false);
        //userProfiles[selectedProfile].GetComponent<Profile>().Demo();

    }

    public void Sellect()
    {
        //doing some thing to the sellecting profile.
        Debug.Log("Profile sellected!");
        userProfiles[selectedProfile].GetComponent<Profile>().Character.SetActive(true);
        userProfiles[selectedProfile].GetComponent<Profile>().Character.GetComponent<PlayerInput>().enabled = true;
        userProfiles[selectedProfile].GetComponent<Profile>().Character.GetComponent<ThirdPersonController>().isInDemoMode = false;
        userProfiles[selectedProfile].GetComponent<MeshRenderer>().enabled = false;
        Canvas.SetActive(false);

    }
    public void View()
    {
        Debug.Log("Profile Viewer!");
        userProfiles[selectedProfile].GetComponent<Profile>().ShowStats();
        userProfiles[selectedProfile].GetComponent<Profile>().Demo();

    }
   
}
