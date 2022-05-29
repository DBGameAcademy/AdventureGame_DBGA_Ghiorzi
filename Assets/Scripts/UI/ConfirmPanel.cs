using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmPanel : MonoBehaviour
{
    [SerializeField]
    private QuantityPanel quantityPanel;

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

    public void Confirm()
    {
        // TO-DO MONEY CHECK LOGIC HERE -> FALSE: ERROR MESSAGE OR WHATEVER AND THEN RETURN

        // TO-DO FREE SPACE CHECK -> FALSE: ERROR MESSAGE OR WHATEVER AND THEN RETURN

        // Add items to inventory
        for (int i = 0; i<quantityPanel.Quantity; i++)
        {
            UIController.Instance.Inventory.AddItemToInventory(quantityPanel.Item);
        }
    }

    private void Awake()
    {
        _isOpen = false;
        _animator = GetComponent<Animator>();
    }
}
