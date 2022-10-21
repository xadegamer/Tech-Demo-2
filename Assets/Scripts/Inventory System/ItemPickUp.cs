using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemPickUp : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO itemSO;
    [SerializeField] private float rotationSpeed = 1f;
    
    private ScannableObject scannableObject;
    private GameObject item;

    private void Awake()
    {
        scannableObject = GetComponent<ScannableObject>();
    }
    
    void Start()
    {
        scannableObject.scanName = itemSO.itemName;
        scannableObject.scanDescription = itemSO.itemDescription;
        item =  Instantiate(itemSO.itemPrefab, transform);
        InteractionSystem.Instance.SetAllChildrenScanningSelected(item, LayerMask.NameToLayer("Scannable"), true);
    }

    private void Update()
    {
        item.transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }

    public void Interact()
    {
        CheckDistanceToPlayer();

        return;


        InventoryManager.Instance.AddItemToInventory(itemSO);
        InteractionSystem.Instance.ForceCloseUI();
        Destroy(gameObject);
    }

    public void CheckDistanceToPlayer()
    {
        Debug.Log(Vector3.Distance(transform.position, InteractionSystem.Instance.transform.position));
    }
}
