using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private ScriptableObjectDatabase itemDatabase;

    [SerializeField] private List<Item> currentInventoryItems = new List<Item>();

    [SerializeField] private InventoryData inventoryData;

    private void Awake()
    {
        Instance = this;

        inventoryData = new InventoryData();

       // LoadInventory();
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

    public bool AddItemToInventory(ItemSO itemSO)
    {
        if (itemSO.oneAllowed && currentInventoryItems.Exists(item => item.itemSO == itemSO))
        {
            Debug.Log("Item already in inventory");
            return false;
        }

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
                        OnObjectAdded?.Invoke(currentInventoryItems[i], EventArgs.Empty);
                        PopUpMessage.Instance.ShowMessage(currentInventoryItems[i].itemSO.itemName + " added to Inventory", PopUpMessage.messageType.Normal);
                        return true;
                    }

                    PopUpMessage.Instance.ShowMessage("Item full in Inventory", PopUpMessage.messageType.Error);
                    return false;
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

        PopUpMessage.Instance.ShowMessage(newItem.itemSO.itemName + " added to Inventory", PopUpMessage.messageType.Normal);

        SaveInventory();

        OnObjectAdded?.Invoke(newItem, EventArgs.Empty);

        return true;
    }

    public void RemoveItemFromInventory(Item item)
    {
        if (item.itemSO.isStackable)
        {
            item.itemData.amount--;
            
            if (item.itemData.amount <= 0)
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
        OnObjectRemoved?.Invoke(item, EventArgs.Empty);
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
     //   JsonSaveSystem.Save(saveID, inventoryData);
    }
}
