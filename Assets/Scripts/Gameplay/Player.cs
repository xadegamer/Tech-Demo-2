using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform itemSpawnPos;

    [SerializeField] private int health;
    [SerializeField] private int stamina;
    [SerializeField] private int hunger;

    private void Start()
    {
        InventoryUI.Instance.OnItemDropped += InventoryUI_OnItemDropped;
    }

    public void AddHealth()
    {
        
    }

    private void InventoryUI_OnItemDropped(object sender, Item e)
    {
        Util.SpawnItem(e.itemSO, itemSpawnPos, false);
    }
}
