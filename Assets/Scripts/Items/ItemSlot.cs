using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public DraggableItem Draggable { get => draggable; set=>draggable = value; }
    public Item.eItemType SlotType { get=>slotType; }
    [SerializeField]
    private Item.eItemType slotType;
    [SerializeField]
    private DraggableItem draggable;

    public void OnDrop(DraggableItem draggedItem)
    {
        Item tempItem = Draggable.Item;
        Draggable.Item = draggedItem.Item;
        
        if(tempItem != null)
            draggedItem.Item = tempItem;
        else
            draggedItem.Item = null;

        if(slotType == Item.eItemType.Armour)
        {
            GameController.Instance.Player.EquipedArmour = Draggable.Item as ArmourItem;
        }
        if (slotType == Item.eItemType.Weapon)
        {
            GameController.Instance.Player.HeldWeapon = Draggable.Item as WeaponItem;
        }

        UpdateItemDisplay();
        draggedItem.ParentSlot.UpdateItemDisplay();
    }

    public void UpdateItemDisplay()
    {
        if(Draggable.Item != null)
        {
            //Draggable.ItemImage.gameObject.SetActive(true);
            Draggable.ItemImage.enabled = true;
            Draggable.ItemImage.sprite = Draggable.Item.Image;
        }
        else
        {
            Draggable.ItemImage.enabled = false;
            //Draggable.ItemImage.gameObject.SetActive(false);
        }
    }
}
