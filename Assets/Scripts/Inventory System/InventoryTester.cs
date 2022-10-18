using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    public ItemSO[] itemSOs;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            for (int i = 0; i < itemSOs.Length; i++)
            {
                InventoryManager.Instance.CreateItem(itemSOs[i]);
            }
        }
    }
}
