using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StarterAssets
{
public class DeathMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public void RestartGame()
    {
        // reload the scene
        player.GetComponent<StarterAssetsInputs>().ResumeInput();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    
}
}