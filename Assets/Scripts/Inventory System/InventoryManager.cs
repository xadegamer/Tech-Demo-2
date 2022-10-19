using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

[Serializable]
public class InventoryData 
{
  public  List<ItemData> ItemDataList = new List<ItemData>();
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public event EventHandler OnObjectAdded;

    public event EventHandler OnObjectRemoved;

    [SerializeField] private string saveID;

    [SerializeField] private ItemDatabase itemDatabase;

    [SerializeField] private List<Item> currentInventoryItems = new List<Item>();

    [SerializeField] private InventoryData inventoryData;

    private void Awake()
    {
        Instance = this;
        LoadInventory();
    }
    
    public void LoadInventory()
    {
        inventoryData = JsonSaveSystem.Load<InventoryData>(saveID);
        if (inventoryData == null) inventoryData = new InventoryData();
        if (inventoryData.ItemDataList.Count == 0) return;

        foreach (ItemData itemData in inventoryData.ItemDataList)
        {
            Item item = CreateItem(itemDatabase.GetItemSOByID(itemData.ID), itemData);
            currentInventoryItems.Add(item);
        }
    }

    public void AddItemToInventory(ItemSO itemSO)
    {
        Item newItem = null;
        
        if (itemSO.isStackable)
        {
            for (int i = 0; i < currentInventoryItems.Count; i++)
            {
                if (currentInventoryItems[i].itemSO == itemSO)
                {
                    if (currentInventoryItems[i].itemData.amount < currentInventoryItems[i].itemSO.maxAmount)
                    {
                        currentInventoryItems[i].itemData.amount++;
                        SaveInventory();
                        OnObjectAdded.Invoke(currentInventoryItems[i], EventArgs.Empty);
                    }
                    return;
                }
            }

            newItem = CreateItem(itemSO);
            currentInventoryItems.Add(newItem);
            inventoryData.ItemDataList.Add(newItem.itemData);
        }
        else
        {
            newItem = CreateItem(itemSO);
            currentInventoryItems.Add(newItem);
            inventoryData.ItemDataList.Add(newItem.itemData);
        }

        SaveInventory();

        OnObjectAdded.Invoke(newItem, EventArgs.Empty);
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
                currentInventoryItems.Remove(item);
                inventoryData.ItemDataList.Remove(item.itemData);
            }
        }
        else
        {
            currentInventoryItems.Remove(item);
            inventoryData.ItemDataList.Remove(item.itemData);
        }

        SaveInventory();
        OnObjectRemoved.Invoke(item, EventArgs.Empty);
    }

    public Item CreateItem(ItemSO itemSO, ItemData itemData = null)
    {
        Item item = new Item();
        item.itemSO = itemSO;
        item.itemData = (itemData == null) ? new ItemData(itemSO) : itemData;
        return item;
    }

    public List<Item> GetInventoryItems()
    {
        return currentInventoryItems;
    }

    public void SaveInventory()
    {
        JsonSaveSystem.Save(saveID, inventoryData);
    }
}
