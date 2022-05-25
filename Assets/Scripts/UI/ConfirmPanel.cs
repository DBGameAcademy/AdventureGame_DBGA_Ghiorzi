using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmPanel : MonoBehaviour
{
    private bool _isOpen = false;
    private Animator _animator;
    public void Open()
    {
        if (_isOpen)
            return;
        _isOpen = true;
        _animator.SetBool("IsOpen", _isOpen);
    }

    public void Close()
    {
        if (!_isOpen)
            return;
        _isOpen = false;
        _animator.SetBool("IsOpen", _isOpen);
    }

    private void Awake()
    {
        _isOpen = false;
        _animator = GetComponent<Animator>();
    }
}
