using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour, IInteractable, IScannable
{
    [SerializeField] private ItemSO itemSO;
    [SerializeField] private float rotationSpeed = 1f;
    
    private GameObject item;
    
    void Start()
    {
        if (itemSO != null)
        {
            SetUp(itemSO);
        }
    }

    public void SetUp(ItemSO itemSO)
    {
        item = Instantiate(itemSO.itemPrefab, transform);
        InteractionSystem.Instance.SetAllChildrenScanningSelected(item, LayerMask.NameToLayer("Scannable"), true);
    }

    private void Update()
    {
        item.transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }

    public void Interact()
    {
        InventoryManager.Instance.AddItemToInventory(itemSO);
        InteractionSystem.Instance.ForceScanningCloseUI();
        Destroy(gameObject);
    }

    public string ScanName()=>  itemSO.itemName;

    public string ScanDescription() => itemSO.itemDescription;

    public float ScanSize() => itemSO.scanSize;
}
