using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DrawIntro : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("IntroTextAnimation");
        StartCoroutine(WaitTillEndOfScene());
    }

    IEnumerator WaitTillEndOfScene()
    {
        yield return new WaitForSeconds(80f);
        SceneManager.LoadScene("Game");
    }
    
    void Update()
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            animator.enabled = false;
            SceneManager.LoadScene("Game");
        }
    }
}
