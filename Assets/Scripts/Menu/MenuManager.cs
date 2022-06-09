using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField]
    private Animator loadingPanelAnimator;
    [SerializeField]
    private Animator menuUIAnimator;
    [SerializeField]
    private Animator tutorialAnimator;

    public void ExitApplicaiton()
    {
        Application.Quit();
    }

    public void ShowTutorial()
    {
        tutorialAnimator.SetTrigger("Appear");
    }

    public void HideTutorial()
    {
        tutorialAnimator.SetTrigger("Disappear");
    }

    public void ShowMenu()
    {
        menuUIAnimator.SetTrigger("Appear");
    }

    public void HideMenu()
    {
        menuUIAnimator.SetTrigger("Disappear");
    }

    private void Start()
    {
        StartCoroutine(COWaitForLoading(8.0f));
    }

    private IEnumerator COWaitForLoading(float delay)
    {
        yield return new WaitForSeconds(delay);
        loadingPanelAnimator.SetTrigger("Close");
    }

    public void OpenLoading()
    {
        loadingPanelAnimator.SetTrigger("Open");
    }

    public void LoadScene(string name)
    {
        StartCoroutine(COOpenNextSceneWithLoading(name));
    }

    private IEnumerator COOpenNextSceneWithLoading(string sceneName)
    {
        loadingPanelAnimator.SetTrigger("Open");
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(COLoadSceneAsync(sceneName));
    }

    private IEnumerator COLoadSceneAsync(string sceneName)
    {
        yield return null;
        SceneManager.LoadScene(sceneName);
    }
}
