using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DrawMainMenu : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("Starting game...");
        SceneManager.LoadScene("Intro");
    }
    
    public void EnterSettings()
    {
        //TODO: settings menu
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
