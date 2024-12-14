using UnityEngine;
using UnityEngine.SceneManagement;

public class DrawMainMenu : MonoBehaviour
{
    public void StartGame()
    {
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
