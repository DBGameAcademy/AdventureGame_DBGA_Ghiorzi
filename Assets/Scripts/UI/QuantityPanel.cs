using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuantityPanel : MonoBehaviour
{
    public int CurrentPrice { get; private set; }

    [SerializeField]
    private TextMeshProUGUI priceText;
    [SerializeField]
    private TextMeshProUGUI quantityText;

    private int _basePrice = 0;
    private int _quantity = 0;

    private bool _isOpen = false;
    private Animator _animator;

    public void SetPanel(int basePrice)
    {
        if (basePrice < 0)
            return;
        _basePrice = basePrice;
        _quantity = 1;

        UpdateUI();
    }

    public void ResetPanel()
    {
        _basePrice = 0;
        _quantity = 0;
        UpdateUI();
    }

    public void AddQauntity()
    {
        if(_basePrice <=0)
            return;
        if (_quantity >= 99)
        {
            _quantity = 99;
            return;
        }
        _quantity++;

        UpdateUI();
    }

    public void RemoveQauntity()
    {
        if (_basePrice <= 0)
            return;
        if (_quantity <= 1)
        {
            _quantity = 1;
            return;
        }
        _quantity--;

        UpdateUI();
    }

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
        _animator = GetComponent<Animator>();
    }

    private void UpdateUI()
    {
        quantityText.text = "x"+_quantity.ToString("00");
        CurrentPrice = _quantity*_basePrice;
        priceText.text = "$" + CurrentPrice.ToString("0000");
    }
}
