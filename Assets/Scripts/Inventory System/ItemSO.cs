using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory / Create Item", order = 1)]
public class ItemSO : ScriptableObject
{
    public int ID;
    public bool isStackable;
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public Color itemColor;
    public GameObject itemPrefab;
    public int maxAmount;
}

[System.Serializable]
public class Item
{
    public ItemSO itemSO;
    public ItemData itemData;
}

[System.Serializable]
public class ItemData
{
    public int ID;
    public int amount;

    public ItemData(ItemSO itemSO)
    {
        ID = itemSO.ID;
        amount = 1;
    }

    public int GetValue()
    {
        return amount;
    }

    public void AddValue(int increment)
    {
        amount += increment;
    }

    public void RemoveValue(int increment)
    {
        amount -= increment;
    }
}
