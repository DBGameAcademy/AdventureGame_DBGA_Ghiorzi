using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    private bool IsOpen = false;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Open()
    {
        if (IsOpen)
            return;
        _animator.SetBool("IsOpen",true);
        IsOpen = true;
    }

    public void Close()
    {
        if (!IsOpen)
            return;
        _animator.SetBool("IsOpen", false);
        IsOpen = false;
    }
}
