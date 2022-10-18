using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Item DataBase", menuName = "Inventory / Create Item DataBase", order = 1)]
public class ItemDatabase : SerializedScriptableObject
{
    [OnValueChanged("AddItemAndID")]
    public ItemSO[] items;

    public Dictionary<int, ItemSO> itemDatabase = new Dictionary<int, ItemSO>();

    public void AddItemAndID()
    {
        itemDatabase.Clear();

        for (int i = 0; i < items.Length; i++)
        {
            ItemSO item;

            if (itemDatabase.TryGetValue(items[i].ID, out item))
            {
                Debug.LogError("Item with ID " + items[i].ID + " already exist in the database.");
            }
            else
            {
                itemDatabase.Add(items[i].ID, items[i]);
            }
        }
    }
    
    public ItemSO GetItemSOByID(int id)
    {
        if (itemDatabase.ContainsKey(id))
        {
            return itemDatabase[id];
        }
        else
        {
            Debug.LogError("Item with ID " + id + " does not exist in the database.");
            return null;
        }
    }
}
