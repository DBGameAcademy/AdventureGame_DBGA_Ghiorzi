using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IUpdateSelectedHandler
{
    [SerializeField]
    private TextMeshProUGUI textName;
    [SerializeField]
    private TextMeshProUGUI textCount;
    [SerializeField]
    private Image itemImage;

    private ShopPanel _shop;
    private int itemCount = 0;
    private Item _currentItem;
    private int _basePrice;

    private bool _isQuantitySet = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _shop.DescriptionPanel.SetPanel(_currentItem.Description);
        _shop.DescriptionPanel.Open();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _shop.DescriptionPanel.Close();
    }

    public void OnSelect(BaseEventData eventData)
    {
        _shop.DescriptionPanel.Select();
        _shop.QuantityPanel.Open();
        if (!_isQuantitySet)
        {
            _shop.QuantityPanel.SetPanel(_basePrice);
            _isQuantitySet = true;
        }
        _shop.ConfirmPanel.Open();
    }
    
    public void OnDeselect(BaseEventData eventData)
    {
        _shop.DescriptionPanel.Deselect();
        _shop.QuantityPanel.Close();
        _shop.ConfirmPanel.Close();  
    }

    public void OnUpdateSelected(BaseEventData eventData)
    {
        _shop.DescriptionPanel.Deselect();
        _shop.DescriptionPanel.SetPanel(_currentItem.Description);
        _shop.DescriptionPanel.Select();
    }

    public void SetEntry(Item item, int basePrice)
    {
        _currentItem = item;
        itemImage.sprite = item.Image;
        textName.text = item.Name;
        _basePrice = basePrice;
    }

    private void Awake()
    {
        _shop = GetComponentInParent<ShopPanel>();
    }

    private void Update()
    {
        UpdateCountForItem();
    }

    private void UpdateCountForItem()
    {
        if (_currentItem == null)
            return;
        itemCount = UIController.Instance.Inventory.GetItemCount(_currentItem.ID);
        textCount.text = "BAG x" + itemCount.ToString("000");

        if (EventSystem.current.currentSelectedGameObject != null 
            && EventSystem.current.currentSelectedGameObject.GetComponent<ItemEntry>()
            && (!EventSystem.current.currentSelectedGameObject.Equals(this.gameObject)))
        {
            if (_isQuantitySet)
            {
                _isQuantitySet = false;
            }
        }
    }
}
