using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    [SerializeField]
    private Animator loadingPanelAnimator;

    private void Start()
    {
        StartCoroutine(COWaitForLoading(8.0f));
    }

    private IEnumerator COWaitForLoading(float delay)
    {
        yield return new WaitForSeconds(delay);
        loadingPanelAnimator.SetTrigger("Close");
    }
}
