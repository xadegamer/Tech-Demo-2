using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class EquipmentHolder : MonoBehaviour
{
    public Item eqquipedItem;
    public GameObject itemObject;
    public IEquipment equipment;

    void Start()
    {
        InventoryUI.Instance.OnInventorySlotSelected += InventoryUI_OnInventorySlotSelected;
        
       // equipment = transform.GetChild(0).GetComponent<IEquipment>();
    }

    private void InventoryUI_OnInventorySlotSelected(object sender, Item item)
    {
        if (eqquipedItem != null) UnEquipLastItem();
        EquipNewItem(item);
    }

    public void UnEquipLastItem()
    {
        eqquipedItem = null;
        Destroy(itemObject);
        equipment = null;
    }

    public void EquipNewItem(Item item)
    {
        eqquipedItem = item;
        itemObject = Instantiate(item.itemSO.itemPrefab, transform);
        itemObject.transform.localPosition = item.itemSO.equipmentSpawnPos;
        itemObject.transform.localEulerAngles = item.itemSO.equipmentSpawnRot;
        equipment = itemObject.GetComponent<IEquipment>();
        InteractionSystem.Instance.SetAllChildrenScanningSelected(itemObject, LayerMask.NameToLayer("Equipment"));
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && equipment != null)
        {
            equipment.Use();
        }
    }
}

public interface IEquipment
{
    void Use();
}
