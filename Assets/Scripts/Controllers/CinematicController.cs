using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicController : Singleton<CinematicController>
{
    public bool IsPlaying { get; private set; }

    [SerializeField]
    private Animator animator;

    public void StartCinematic()
    {
        animator.ResetTrigger("CloseCinematic");
        animator.SetTrigger("StartCinematic"); 
        IsPlaying = true;
    }

    public void EndCinematic()
    {
        animator.ResetTrigger("StartCinematic");
        animator.SetTrigger("CloseCinematic");
        IsPlaying = false;
    }

    public void StartBattleCinematic()
    {
        animator.ResetTrigger("CloseCinematic");
        animator.SetTrigger("StartCinematic");
    }

    public void EndBattleCinematic()
    {
        animator.ResetTrigger("StartCinematic");
        animator.SetTrigger("CloseCinematic");
    }
}
