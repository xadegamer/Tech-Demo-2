using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public event EventHandler OnScanningObjectChanged;

    [SerializeField] private string saveID;

    [SerializeField] private ItemDatabase itemDatabase;

    [SerializeField] private List<Item> inventoryItems = new List<Item>();

    [SerializeField] private List<ItemData> inventoryItemData = new List<ItemData>();

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        LoadInventory();
    }

    public void LoadInventory()
    {
        inventoryItemData = JsonSaveSystem.Load<List<ItemData>>(saveID);

        if (inventoryItemData.Count == 0) return;

        foreach (ItemData itemData in inventoryItemData)
        {
            Item item = CreateItem(itemDatabase.GetItemSOByID(itemData.ID), itemData);
            inventoryItems.Add(item);
        }
    }

    public void AddItemToInventory(ItemSO itemSO)
    {
        if (itemSO.isStackable)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].itemSO == itemSO)
                {
                    if (inventoryItems[i].itemData.amount < inventoryItems[i].itemSO.maxAmount)
                    {
                        inventoryItems[i].itemData.amount++;
                        SaveInventory();
                    }
                    return;
                }
            }

            Item newItem = CreateItem(itemSO);
            inventoryItems.Add(newItem);
            inventoryItemData.Add(newItem.itemData);
        }
        else
        {
            Item newItem = CreateItem(itemSO);
            inventoryItems.Add(newItem);
            inventoryItemData.Add(newItem.itemData);
        }

        SaveInventory();
    }

    public void RemoveItemFromInventory(Item item)
    {
        if (item.itemSO.isStackable)
        {
            if (item.itemData.amount > 1)
            {
                item.itemData.amount--;
            }
            else
            {
                inventoryItems.Remove(item);
                inventoryItemData.Remove(item.itemData);
            }
        }
        else
        {
            inventoryItems.Remove(item);
            inventoryItemData.Remove(item.itemData);
        }

        SaveInventory();
    }

    public Item CreateItem(ItemSO itemSO, ItemData itemData = null)
    {
        Item item = new Item();
        item.itemSO = itemSO;
        item.itemData = (itemData == null) ? new ItemData(itemSO) : itemData;
        return item;
    }

    public void SaveInventory()
    {
        JsonSaveSystem.Save(saveID, inventoryItemData);
    }
}
