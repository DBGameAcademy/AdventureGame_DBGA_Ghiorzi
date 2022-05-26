using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [SerializeField]
    private Item.eItemType slotType;
    [SerializeField]
    private DraggableItem draggable;

    public void OnDrop(DraggableItem draggedItem)
    {
        Item tempItem = draggable.Item;
        draggable.Item = draggedItem.Item;
        
        if(tempItem != null)
            draggedItem.Item = tempItem;
        else
            draggedItem.Item = null;

        UpdateItemDisplay();
        draggedItem.ParentSlot.UpdateItemDisplay();
    }

    public void UpdateItemDisplay()
    {
        if(draggable.Item != null)
        {
            draggable.ItemImage.gameObject.SetActive(true);
            draggable.ItemImage.sprite = draggable.Item.Image;
        }
        else
        {
            draggable.ItemImage.gameObject.SetActive(false);
        }
    }
}
