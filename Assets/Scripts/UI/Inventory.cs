using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool IsOpen { get; set; }

    [SerializeField]
    private GameObject inventoryContainer;

    private List<ItemSlot> _slots = new List<ItemSlot> ();

    private void Start()
    {
        foreach(Transform t in inventoryContainer.transform)
        {
            if (t.GetComponent<ItemSlot>())
            {
                _slots.Add(t.GetComponent<ItemSlot>());
            }
        }
    }

    private void Update()
    { 
    }
}
