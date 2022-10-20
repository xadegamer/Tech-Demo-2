using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory / Create Item", order = 1)]
public class ItemSO : ScriptableObjectBase
{
    public int ID;
    public bool oneAllowed;
    public bool isStackable;
    public string itemName;

    [TextArea(5, 10)]
    public string itemDescription;
    
    public Sprite itemIcon;
    public Color itemColor;
    public GameObject itemPrefab;
    public int maxAmount;

    public override int GetID()
    {
        return ID;
    }
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
}
