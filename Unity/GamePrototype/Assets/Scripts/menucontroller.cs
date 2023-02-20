using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;  

public class menucontroller : MonoBehaviour
{

    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    public void login()
    {
        //todo
        SceneManager.LoadScene("Playground");  
    }

    public void signup()
    {
        //todo
    }


}
