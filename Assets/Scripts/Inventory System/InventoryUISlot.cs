using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUISlot : MonoBehaviour
{
    [SerializeField] private Item item;

    private void Awake()
    {
        item = null;
    }

    public void SetItem(Item item)
    {
        this.item = item;
    }

    public void RemoveItem()
    {
        item = null;
    }

    public Item GetItem()
    {
        return item;
    }
}
