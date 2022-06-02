using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanel : MonoBehaviour
{
    public bool IsOpen { get; private set; }
    private Animator _animator;

    public void Open()
    {
        if (IsOpen)
            return;
        _animator.SetTrigger("Open");
        IsOpen = true;
    }

    public void Close()
    {
        if (!IsOpen)
            return;
        _animator.SetTrigger("Close");
        IsOpen=false;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
}
