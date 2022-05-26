using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : Singleton<ItemController> 
{
    public ItemCollection Items { get => items; }
    [SerializeField]
    private ItemCollection items;
}
