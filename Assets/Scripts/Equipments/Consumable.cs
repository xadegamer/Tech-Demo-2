using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour, IEquipment
{
    public enum Type { Health, Stamina, Food, Water }

    [SerializeField] private Type type;
    [SerializeField] private GameObject usedObject;
    [SerializeField] private AudioClip usedSound;

    private Item item;
    
    public void Use()
    {
        switch (type)
        {
            case Type.Health:

                break;
            case Type.Stamina:

                break;
            case Type.Food:

                break;
            case Type.Water:
                break;
        }


        AudioHandler.Instance.PlaySfx(usedSound, true);
        
        Instantiate(usedObject, transform.position, Quaternion.identity);
        
        item =  GetComponentInParent<EquipmentHolder>().GetItem();
        PopUpMessage.Instance.ShowMessage("Consumed " + item.itemSO.itemName, PopUpMessage.messageType.Normal);
        InventoryManager.Instance.RemoveItemFromInventory(item);
    }
}
