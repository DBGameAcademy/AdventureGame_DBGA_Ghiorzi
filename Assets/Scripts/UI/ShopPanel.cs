using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShopEntry { 
    public string ID;
    public int BasePrice;
}

public class ShopPanel : MonoBehaviour
{   
  
    public QuantityPanel QuantityPanel { get => quantityPanel; }
    public ConfirmPanel ConfirmPanel { get => confirmPanel; }
    public DescriptionPanel DescriptionPanel { get => descriptionPanel; }
    [Header("UI References")]
    [SerializeField]
    private GameObject itemEntryPrefab;
    [SerializeField]
    private GameObject itemList;
    [SerializeField]
    private QuantityPanel quantityPanel;
    [SerializeField]
    private ConfirmPanel confirmPanel;
    [SerializeField]
    private DescriptionPanel descriptionPanel;
    [Header("Shop Entries")]
    [SerializeField]
    private List<ShopEntry> shopEntries = new List<ShopEntry>();

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
        quantityPanel.Close();
        descriptionPanel.Deselect();
        confirmPanel.Close();
        _animator.SetBool("IsOpen", _isOpen);
    }

    public void AddItemEntry(string id,int basePrice)
    {
        GameObject itemEntryObj = Instantiate(itemEntryPrefab, itemList.transform);
        ItemEntry itemEntry = itemEntryObj.GetComponent<ItemEntry>();
        Item item = ItemController.Instance.Items.GetItemFromID(id);
        itemEntry.SetEntry(item,basePrice);
    }

    private void Awake()
    {
        _isOpen = false;
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        foreach(ShopEntry entry in shopEntries)
            AddItemEntry(entry.ID,entry.BasePrice);
    }
}
