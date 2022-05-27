using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    public DraggableItem Draggable { get => draggable; set=>draggable = value; }
    public Item.eItemType SlotType { get=>slotType; }
    [SerializeField]
    private Item.eItemType slotType;
    [SerializeField]
    private DraggableItem draggable;

    private Inventory _inventory;

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
            if (slotType == Item.eItemType.Armour)
            {
                GameController.Instance.Player.EquipedArmour = null;
            }
            else if (slotType == Item.eItemType.Weapon)
            {
                GameController.Instance.Player.HeldWeapon = null;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (draggable.Item == null || draggable.Item.Type != Item.eItemType.Consumable)
                return;

            ConsumableItem consumableItem = draggable.Item as ConsumableItem;
            consumableItem.Use();

            _inventory.RemoveItemFromInventory(draggable.Item);
            draggable.Item = null;
            UpdateItemDisplay();
        }
    }

    private void Awake()
    {
        _inventory = GetComponentInParent<Inventory>();
    }
}
