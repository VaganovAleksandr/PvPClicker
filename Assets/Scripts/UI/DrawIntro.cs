using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DrawIntro : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
        StartCoroutine(WaitTillEndOfScene());
    }

    IEnumerator WaitTillEndOfScene()
    {
        yield return new WaitForSeconds(80f);
        SceneManager.LoadScene("Game");
    }
    
    void Update()
    {
        var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            _animator.enabled = false;
            SceneManager.LoadScene("Game");
        }
    }
}
