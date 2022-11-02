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

    Animator a;

    void Start()
    {
        InventoryUI.Instance.OnInventorySlotSelected += InventoryUI_OnInventorySlotSelected;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && equipment != null)
        {
            equipment.Use();
        }
    }

    private void InventoryUI_OnInventorySlotSelected(object sender, Item item)
    {
        if (eqquipedItem != null) UnEquipLastItem();

        if (item != null) EquipNewItem(item);
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
    
    private void MovementAnimation()
    {
        //Checking Movement
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            a.SetFloat("Mag", 1);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                a.SetBool("Running", true);
            }
            else
            {
                a.SetBool("Running", false);
            }
        }
        else
        {
            a.SetBool("Running", false);
            a.SetFloat("Mag", 0);
        }
    }
}

public interface IEquipment
{
    void Use();
}
