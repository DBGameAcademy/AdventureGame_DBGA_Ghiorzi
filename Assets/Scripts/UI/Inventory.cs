using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int FreeSpace { get; private set; }
    public bool IsOpen { get; set; }

    [SerializeField]
    private GameObject inventoryContainer;

    private List<ItemSlot> _slots = new List<ItemSlot>();
    private Dictionary<string, int> countDictionary = new Dictionary<string, int>();

    public void AddItemToInventory(Item item)
    {
        foreach(ItemSlot slot in _slots)
        {
            if(slot.Draggable.Item == null)
            {
                slot.Draggable.Item = item;
                if(countDictionary.TryGetValue(item.ID, out int count))
                {
                    count++;
                    countDictionary[item.ID] = count;
                }
                else
                {
                    count = 1;
                    countDictionary.Add(item.ID, count);
                }
                break;
            }
        }
        UpdateDisplay();
        CheckFreeSpace();
    }

    public void RemoveItemFromInventory(Item item)
    {
        if (item == null)
            return;
        if (!countDictionary.ContainsKey(item.ID))
            return;
        if(countDictionary[item.ID] <= 1)
        {
            countDictionary.Remove(item.ID);
        }
        else
        {
            countDictionary[item.ID]--;
        }
        item = null;
        CheckFreeSpace();
    }

    public void CheckFreeSpace()
    {
        int freeCount = 0;
        foreach (ItemSlot slot in _slots)
        {
            if (slot.IsEmpty)
                freeCount++;
        }
        FreeSpace = freeCount;
    }

    public void UpdateDisplay()
    { 
        foreach(ItemSlot slot in _slots)
        {
            slot.UpdateItemDisplay();
        }
    }

    public int GetItemCount(string id)
    {
        if(!countDictionary.ContainsKey(id))
            return 0;
        return countDictionary[id];
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
