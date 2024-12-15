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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            animator.enabled = false;
            SceneManager.LoadScene("Game");
        }
    }
}
