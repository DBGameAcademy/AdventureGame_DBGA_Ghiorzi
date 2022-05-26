using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool IsOpen { get; set; }

    [SerializeField]
    private GameObject inventoryContainer;

    private List<ItemSlot> _slots = new List<ItemSlot>();

    public void AddItemToInventory(Item item)
    {
        foreach(ItemSlot slot in _slots)
        {
            if(slot.Draggable.Item == null)
            {
                slot.Draggable.Item = item;
                break;
            }
        }
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        foreach(ItemSlot slot in _slots)
        {
            slot.UpdateItemDisplay();
        }
    }

    private void Start()
    {
        foreach(Transform t in inventoryContainer.transform)
        {
            if (t.GetComponent<ItemSlot>())
            {
                _slots.Add(t.GetComponent<ItemSlot>());
            }
        }

        AddItemToInventory(ItemController.Instance.Items.GetItemFromID("arm_01"));
        AddItemToInventory(ItemController.Instance.Items.GetItemFromID("sw_01"));
        AddItemToInventory(ItemController.Instance.Items.GetItemFromID("hpp_01"));
        AddItemToInventory(ItemController.Instance.Items.GetItemFromID("hpp_01"));
        AddItemToInventory(ItemController.Instance.Items.GetItemFromID("hpp_02"));
        AddItemToInventory(ItemController.Instance.Items.GetItemFromID("hpp_02"));
        AddItemToInventory(ItemController.Instance.Items.GetItemFromID("sw_99"));
        AddItemToInventory(ItemController.Instance.Items.GetItemFromID("arm_99"));
    }
}
