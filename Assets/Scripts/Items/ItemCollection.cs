using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="New Item Collection", menuName = "Items/Create Item Collection")]
public class ItemCollection : ScriptableObject
{
    public List<Item> Items = new List<Item>();

    public Item GetItem(Type itemType)
    {
        foreach(Item item in Items)
        {
            if(item.GetType() == itemType)
            {
                return item;
            }
        }
        Debug.LogError("No Items for type " + itemType);
        return null;
    }
}
