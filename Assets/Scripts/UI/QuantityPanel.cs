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

    private int _basePrice;
    private int _quantity = 1;

    public void SetPanel(int basePrice)
    {
        if (basePrice < 0)
            return;
        _basePrice = basePrice;
        _quantity = 1;

        UpdateUI();
    }

    public void AddQauntity()
    {
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
        if(_quantity <= 1)
        {
            _quantity = 1;
            return;
        }
        _quantity--;

        UpdateUI();
    }

    private void UpdateUI()
    {
        quantityText.text = "x"+_quantity.ToString("00");
        CurrentPrice = _quantity*_basePrice;
        priceText.text = "$" + CurrentPrice.ToString("0000");
    }
}
