using UnityEngine;
using UnityEngine.SceneManagement;

public class MyLibrary : MonoBehaviour
{
    [SerializeField] private string AbobaString = "AbobaString";
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);

    }
    [ContextMenu("DoAbobaFunction")]
    public void Aboba() {
        Debug.Log(AbobaString);
        AbobaString = "AbobaString";
    }
}
