using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using StarterAssets;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [Header("Properties")]
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private float wheelSpinSpeed;
    [SerializeField] private float wheelRadius;
    [SerializeField] private float currentSlotRotationSpeed = 1f;
    [SerializeField] private Transform inventoryWheel;
    [SerializeField] private InventoryUISlot itemSlotPrefab;

    [Header("Info")]
    [SerializeField] private GameObject infoUI;
    [SerializeField] private GameObject noItemUi;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemInfo;
    [SerializeField] private TextMeshProUGUI itemCountInfo;
    [SerializeField] private GameObject equipButton;
    [SerializeField] private GameObject unEquipButton;


    [Header("Events")]
    public EventHandler<Item> OnInventorySlotSelected;
    public EventHandler<Item> OnItemDropped;

    [Header("Debug")]
    [SerializeField] List<InventoryUISlot> itemSlotsList = new List<InventoryUISlot>();
    [SerializeField] private float angleToRotate;
    [SerializeField] private int slotInFront = 0;
    [SerializeField] private InventoryUISlot currentSlot;
    [SerializeField] bool isTurning = false;
    [SerializeField] bool isActivated = false;
    
    private Item selectedItem = null;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InventoryManager.Instance.OnObjectAdded += InventoryManager_OnObjectAdded;
        InventoryManager.Instance.OnObjectRemoved += InventoryManager_OnObjectRemoved;

        //SetUp();
    }

    void Update()
    {
        if (StarterAssetsInputs.Instance.Inventory)
        {
            StarterAssetsInputs.Instance.Inventory = false;
            if (inventoryUI.activeInHierarchy) CloseInventory();
            else if (GameManager.Instance.GetCurrentControlMode() == GameManager.ControlMode.PlayerControl) OpenInventory();
        }
        
        if (StarterAssetsInputs.Instance.left)
        {
            StarterAssetsInputs.Instance.left = false;
            RotateLeft();
        }

        if (StarterAssetsInputs.Instance.right)
        {
            StarterAssetsInputs.Instance.right = false;
            RotateRight();
        }

        if (StarterAssetsInputs.Instance.equip)
        {
            StarterAssetsInputs.Instance.equip = false;
            EquipItem(true);
        }

        if (!isTurning && currentSlot != null) currentSlot.transform.Rotate(Vector3.right * currentSlotRotationSpeed * Time.deltaTime);
    }

    private void InventoryManager_OnObjectAdded(object sender, EventArgs e)
    {
       // itemSlotsList.Remove(GetInventorySlotWithItem(sender as Item));   
    }

    public void OpenInventory()
    {
        isActivated = true;
        SetUp();
        GameManager.Instance.SwitchControl(GameManager.ControlMode.UIControl);

        if (selectedItem != null)
        {
            currentSlot = GetInventorySlotWithItem(selectedItem);
            slotInFront = itemSlotsList.IndexOf(currentSlot);

            inventoryWheel.rotation = Quaternion.Euler(inventoryWheel.eulerAngles + Vector3.up * (angleToRotate * slotInFront));
            DisplayCurrentItemInfo();
        }
        
        inventoryUI.SetActive(true);
    }

    public void CloseInventory()
    {
        isActivated = false;
        inventoryUI.SetActive(false);
        ResetInventory();
        GameManager.Instance.SwitchControl(GameManager.ControlMode.PlayerControl);
    }

    private void InventoryManager_OnObjectRemoved(object sender, EventArgs e)
    {
        InventoryUISlot inventoryUISlot = GetInventorySlotWithItem(sender as Item);

        if (inventoryUISlot == null)  return;

        itemSlotsList.Remove(inventoryUISlot);

        Destroy(inventoryUISlot.gameObject);
    
        ResetWheel();

        slotInFront = 0;

        if (currentSlot.GetItem() == selectedItem)
        {
            selectedItem = null;
            OnInventorySlotSelected?.Invoke(this, selectedItem);
        }

        if (itemSlotsList.Count == 0)
        {
            infoUI.SetActive(false);
            noItemUi.SetActive(true);
            return;
        }

        if (itemSlotsList.Count == 1)
        {
            StartCoroutine(RotateInventoryWheel(angleToRotate, wheelSpinSpeed));
        }
        else RotateRight();
    }

    public void SetUp()
    {
        SpawnItemSlotInWheel(InventoryManager.Instance.GetInventoryItems().Count);
        DisplayCurrentItemInfo();
    }

    public void ResetInventory()
    {
        foreach (InventoryUISlot itemSlot in itemSlotsList)
        {
            Destroy(itemSlot.gameObject);
        }
        itemSlotsList.Clear();
        slotInFront = 0;
    }

    public void SpawnItemSlotInWheel(int howMany)
    {
        float angleSection = Mathf.PI * 2f / howMany;
        angleToRotate = angleSection * Mathf.Rad2Deg;
        for (int i = 0; i < howMany; i++)
        {
            float angle = i * angleSection;
            Vector3 newPos = inventoryWheel.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * wheelRadius;
            
            InventoryUISlot inventoryUISlot = Instantiate(itemSlotPrefab, inventoryWheel);
            inventoryUISlot.gameObject.name = "Slot " + i;
            inventoryUISlot.transform.position = newPos;
            itemSlotsList.Add(inventoryUISlot);

            ItemSO itemSO = InventoryManager.Instance.GetInventoryItems()[i].itemSO;

            GameObject gameObject =  Instantiate(itemSO.itemPrefab, inventoryUISlot.transform);
            gameObject.transform.localScale = new Vector3(itemSO.inventorySpawnScale, itemSO.inventorySpawnScale, itemSO.inventorySpawnScale);
            InteractionSystem.Instance.SetAllChildrenScanningSelected(gameObject, LayerMask.NameToLayer("Inventory") , true);

            inventoryUISlot.SetItem(InventoryManager.Instance.GetInventoryItems()[i]);
        }

        infoUI.SetActive(howMany > 0);
        noItemUi.SetActive(howMany == 0);
    }

    public void DisplayCurrentItemInfo()
    {
        if (itemSlotsList.Count == 0) return;
        currentSlot = itemSlotsList[slotInFront];
        if (currentSlot.GetItem() == null) return;

        itemName.text = currentSlot.GetItem().itemSO.itemName;
        itemInfo.text = currentSlot.GetItem().itemSO.itemDescription;

        if (currentSlot.GetItem().itemSO.isStackable) itemCountInfo.text = currentSlot.GetItem().itemData.amount + " / " + currentSlot.GetItem().itemSO.maxAmount;
        else itemCountInfo.text = "";

        equipButton.SetActive(currentSlot.GetItem() != selectedItem);
        unEquipButton.SetActive(currentSlot.GetItem() == selectedItem);
    }

    public void RotateRight()
    {
        if (!isActivated || isTurning || itemSlotsList.Count <= 1) return;
        slotInFront = ++slotInFront % itemSlotsList.Count;
        StartCoroutine(RotateInventoryWheel(angleToRotate, wheelSpinSpeed));
    }

    public void RotateLeft()
    {
        if (!isActivated || isTurning || itemSlotsList.Count <= 1) return;
        if (slotInFront == 0) slotInFront = itemSlotsList.Count;
        slotInFront = --slotInFront % itemSlotsList.Count;
        StartCoroutine(RotateInventoryWheel(-angleToRotate, wheelSpinSpeed));
    }

    public void EquipItem(bool toggle)
    {
        if (!isActivated) return;
        selectedItem = toggle ? currentSlot.GetItem() : null;
        OnInventorySlotSelected?.Invoke(this, selectedItem);
        CloseInventory();
    }

    public void DropItem()
    {
        OnItemDropped?.Invoke(this, currentSlot.GetItem());
        InventoryManager.Instance.RemoveItemFromInventory(currentSlot.GetItem());
        CloseInventory();
    }

    IEnumerator RotateInventoryWheel(float angle, float inTime)
    {
        isTurning = true;
        Vector3 byAngles = Vector3.up * angle;

        var fromAngle = inventoryWheel.rotation;
        var toAngle = Quaternion.Euler(inventoryWheel.eulerAngles + byAngles);
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            inventoryWheel.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }

        inventoryWheel.rotation = toAngle;
        DisplayCurrentItemInfo();
        isTurning = false;
    }

    public void ResetWheel()
    {
        float angleSection = Mathf.PI * 2f / itemSlotsList.Count;
        angleToRotate = angleSection * Mathf.Rad2Deg;
        
        for (int i = 0; i < itemSlotsList.Count; i++)
        {
            float angle = i * angleSection;
            Vector3 newPos = inventoryWheel.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * wheelRadius;
            itemSlotsList[i].transform.position = newPos;
            itemSlotsList[i].gameObject.name = "Slot " + i;
        }
    }

    public InventoryUISlot GetInventorySlotWithItem(Item item)
    {
        return itemSlotsList.Find(x => x.GetItem() == item);
    }

    public void ResetSelectedItem()
    {
        selectedItem = null;
    }

    #region Old Code
    //IEnumerator RotateRightRoutine(float rotSpeed)
    //{
    //    slotInFront = ++slotInFront % itemSlots.Length;

    //    yield return StartCoroutine(RotateInventoryWheel(90, rotSpeed));

    //    if (currentSlot.GetItem() == null)
    //    {
    //        StartCoroutine(RotateRightRoutine(rotSpeed / 2));
    //    }
    //    else isTurning = false;
    //}
    //IEnumerator RotateLeftRoutine(float rotSpeed)
    //{
    //    if (slotInFront == 0) slotInFront = itemSlots.Length;

    //    slotInFront = --slotInFront % itemSlots.Length;

    //    yield return StartCoroutine(RotateInventoryWheel(-90, rotSpeed));

    //    if (currentSlot.GetItem() == null)
    //    {
    //        StartCoroutine(RotateLeftRoutine(rotSpeed / 2));
    //    }
    //    else isTurning = false;
    //}
    #endregion
}
