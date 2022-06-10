using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPanel : MonoBehaviour
{
    public bool IsOpen { get; private set; }
    
    private Animator _animator;

    public void OpenCloseMenu()
    {
        if (!IsOpen)
        {
            // Open
            IsOpen = true;
            _animator.SetBool("IsOpen", IsOpen);
        }
        else
        {
            // Close
            IsOpen = false;
            _animator.SetBool("IsOpen", IsOpen);
        }
    }

    public void GoToMenu()
    {
        StartCoroutine(COWaitToLoad());
    }

    private IEnumerator COWaitToLoad()
    {
        UIController.Instance.ShowLoading();
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void Awake()
    {
        IsOpen = false;
        _animator = GetComponent<Animator>();
    }

}
