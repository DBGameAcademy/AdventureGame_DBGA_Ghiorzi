using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicController : Singleton<CinematicController>
{
    [SerializeField]
    private Animator animator;

    public void StartCinematic()
    {
        animator.SetTrigger("StartCinematic"); 
    }

    public void EndCinematic()
    {
        animator.SetTrigger("CloseCinematic");
    }
}
