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
    public void OnPointerEnter(PointerEventData eventData)
    {
        _shop.DescriptionPanel.SetPanel("ITEM DESCRIPTION GOES HERE");
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
        _shop.DescriptionPanel.SetPanel("DESCRIPTION GOES HERE");
        _shop.DescriptionPanel.Select();
    }

    public void SetEntry(Item item)
    {
        _currentItem = item;
        itemImage.sprite = item.Image;
        textName.text = item.Name;
        itemCount = 0; // TO-DO: GET ITEM COUNT FROM INVENTORY BY ITEM ID IN DICTIONARY
        textCount.text = "BAG x" + itemCount.ToString("000");
    }

    private void Awake()
    {
        _shop = GetComponentInParent<ShopPanel>();
    }

  
}
