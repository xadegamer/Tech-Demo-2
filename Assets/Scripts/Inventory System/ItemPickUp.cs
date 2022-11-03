using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour, IInteractable, IScannable
{
    [SerializeField] private ItemSO itemSO;
    [SerializeField] private AudioClip itemPickUpSound;
    private ScanInfo scanInfo;

    void Start()
    {
        InteractionSystem.Instance.SetAllChildrenScanningSelected(gameObject, LayerMask.NameToLayer("Scannable"), true);
    }
    
    public void SetUp(ItemSO itemSO)
    {
        this.itemSO = itemSO;
        InteractionSystem.Instance.SetAllChildrenScanningSelected(gameObject, LayerMask.NameToLayer("Scannable"), true);
    }

    public void Interact()
    {
        AudioHandler.Instance.PlaySfx(itemPickUpSound, true);
        InventoryManager.Instance.AddItemToInventory(itemSO);
        InteractionSystem.Instance.ForceScanningCloseUI();
        Destroy(gameObject);
    }

    public string ScanName()=>  itemSO.itemName;

    public string ScanDescription() => itemSO.itemDescription;

    public float ScanSize() => itemSO.scanSize;

    public ScanInfo GetScanInfo()
    {
        scanInfo = new ScanInfo();
        scanInfo.scanName = itemSO.itemName;
        scanInfo.scanDescription = itemSO.itemDescription;
        scanInfo.scanSize = itemSO.scanSize;
        return scanInfo;
    }

    public string GetInteractText()
    {
        return "to pick up";
    }
}
