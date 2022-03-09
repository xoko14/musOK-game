using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void GoToPlay(){
        SceneManager.LoadScene(1);
    }
    public void GoToSettings(){
        SceneManager.LoadScene(4);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
