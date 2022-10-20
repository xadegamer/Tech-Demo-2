using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    [SerializeField] private int ID;
    [SerializeField] private ItemSO[] itemSO;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            for (int i = 0; i < itemSO.Length; i++)
            {
                InventoryManager.Instance.AddItemToInventory(itemSO[i]);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            InventoryManager.Instance.RemoveItemFromInventory(InventoryManager.Instance.GetInventoryItems()[0]);
        }
    }
}
