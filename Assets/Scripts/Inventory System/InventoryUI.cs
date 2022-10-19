using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [Header("Properties")]
    [SerializeField] private float wheelSpinSpeed;
    [SerializeField] private float wheelRadius;
    [SerializeField] private float currentSlotRotationSpeed = 1f;
    [SerializeField] private Transform inventoryWheel;
    [SerializeField] private InventoryUISlot itemSlotPrefab;

    [Header("Info")]
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemInfo;

    [Header("Debug")]
    [SerializeField] List<InventoryUISlot> itemSlotsList = new List<InventoryUISlot>();
    [SerializeField] private float angleToRotate;
    [SerializeField] private int slotInFront = 0;
    [SerializeField] private InventoryUISlot currentSlot;
    [SerializeField] bool isTurning = false;
    
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InventoryManager.Instance.OnObjectAdded += InventoryManager_OnObjectAdded;
        InventoryManager.Instance.OnObjectRemoved += InventoryManager_OnObjectRemoved;

        SetUp();
    }

    private void InventoryManager_OnObjectAdded(object sender, EventArgs e)
    {
       // itemSlotsList.Remove(GetInventorySlotWithItem(sender as Item));   
    }

    public void OpenInventory()
    {

    }

    public void CloseInventory()
    {

    }

    private void InventoryManager_OnObjectRemoved(object sender, EventArgs e)
    {
        InventoryUISlot inventoryUISlot = GetInventorySlotWithItem(sender as Item);

        itemSlotsList.Remove(GetInventorySlotWithItem(sender as Item));
        
        Destroy(inventoryUISlot.gameObject);

        ResetWheel();

        if (itemSlotsList.Count == 0) return;

        slotInFront = 0;
        currentSlot = itemSlotsList[0];
        if (currentSlot.GetItem() == null) return;

        itemName.text = currentSlot.GetItem().itemSO.itemName;
        itemInfo.text = currentSlot.GetItem().itemSO.itemDescription;
    }

    public void SetUp()
    {
        SpawnItemSlotInWheel(InventoryManager.Instance.GetInventoryItems().Count);
        DisplayCurrentItemInfo();
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
            
            Instantiate(InventoryManager.Instance.GetInventoryItems()[i].itemSO.itemPrefab, inventoryUISlot.transform);
            inventoryUISlot.SetItem(InventoryManager.Instance.GetInventoryItems()[i]);
        }
    }

    public void DisplayCurrentItemInfo()
    {
        currentSlot = itemSlotsList[slotInFront];
        if (currentSlot.GetItem() == null) return;

        itemName.text = currentSlot.GetItem().itemSO.itemName;
        itemInfo.text = currentSlot.GetItem().itemSO.itemDescription;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !isTurning)
        {
            RotateLeft();
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow) && !isTurning)
        {
            RotateRight();
        }

        if(!isTurning) currentSlot.transform.Rotate(Vector3.right * currentSlotRotationSpeed * Time.deltaTime);
    }

    public void RotateRight()
    {
        if (itemSlotsList.Count == 1) return;
        slotInFront = ++slotInFront % itemSlotsList.Count;
        StartCoroutine(RotateInventoryWheel(angleToRotate, wheelSpinSpeed));
    }

    public void RotateLeft()
    {
        if (slotInFront == 0) slotInFront = itemSlotsList.Count;
        slotInFront = --slotInFront % itemSlotsList.Count;
        StartCoroutine(RotateInventoryWheel(-angleToRotate, wheelSpinSpeed));
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