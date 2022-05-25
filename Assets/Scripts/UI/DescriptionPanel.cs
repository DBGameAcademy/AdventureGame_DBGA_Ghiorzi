using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DescriptionPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI descriptionText;

    private Animator _animator;
    private bool _isOpen = false;
    private bool _isSelected = false;

    public void SetPanel(string description)
    {
        if (_isOpen)
            return;
        descriptionText.text = description;
    }

    public void Select()
    {
        if (_isSelected)
            return;
        _isSelected = true;
        Open();
    }

    public void Deselect()
    {
        if(!_isSelected)
            return ;    
        _isSelected = false;
        Close();
    }

    public void Open()
    {
        if (_isOpen)
            return;
        _isOpen = true;
        _animator.SetBool("IsOpen",_isOpen);
    }

    public void Close()
    {
        if ((!_isOpen) || _isSelected)
            return;
        _isOpen=false;
        _animator.SetBool("IsOpen", _isOpen);
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _isOpen = false;
        _isSelected = false;
    }

}
