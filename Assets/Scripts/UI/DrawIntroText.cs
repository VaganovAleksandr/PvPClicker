using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DrawIntroText : MonoBehaviour
{
    void Start()
    {
        Debug.Log("In start!");
        StartCoroutine(ShowIntro());
    }
    public IEnumerator ShowIntro()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("Showing intro...");
        SceneManager.LoadScene("IntroText");
    }
}
