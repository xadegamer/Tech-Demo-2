using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }

    [Header("Properties")]
    [SerializeField] private float rotSpeed = 250;
    [SerializeField] private float currentSlotRotationSpeed = 1f;
    [SerializeField] private InventoryUISlot[] itemSlots;
    [SerializeField] private Transform itemRotator;

    [Header("Info")]
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemInfo;

    [Header("Debug")]
    [SerializeField] private int slotInFront = 0;
    [SerializeField] private InventoryUISlot currentSlot;
    [SerializeField] bool isTurning = false;
    
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SpawnInventoryUI();
    }

    public void SpawnInventoryUI()
    {
        for (int i = 0; i < InventoryManager.Instance.GetInventoryItems().Count; i++)
        {
            Instantiate(InventoryManager.Instance.GetInventoryItems()[i].itemSO.itemPrefab, itemSlots[i].transform);
            itemSlots[i].SetItem(InventoryManager.Instance.GetInventoryItems()[i]);

            if (i == itemSlots.Length) return;
        }

        DisplayCurrentItemInfo();
    }

    public void DisplayCurrentItemInfo()
    {
        currentSlot = itemSlots[slotInFront];
        if (currentSlot.GetItem().itemSO == null) return;

        itemName.text = currentSlot.GetItem().itemSO.itemName;
        itemInfo.text = currentSlot.GetItem().itemSO.itemDescription;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !isTurning  && itemSlots.Length > 0)
        {
            slotInFront = ++slotInFront % itemSlots.Length;
  
            StartCoroutine(RotateMe(Vector3.up * 90, rotSpeed));
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow) && !isTurning && itemSlots.Length > 0)
        {
            if (slotInFront == 0) slotInFront = itemSlots.Length;

            slotInFront = --slotInFront % itemSlots.Length;

            StartCoroutine(RotateMe(Vector3.up * -90, rotSpeed));
        }

        if(!isTurning) currentSlot.transform.Rotate(Vector3.right * currentSlotRotationSpeed * Time.deltaTime);
    }

    IEnumerator RotateMe(Vector3 byAngles, float inTime)
    {
        isTurning = true;
        var fromAngle = itemRotator.rotation;
        var toAngle = Quaternion.Euler(itemRotator.eulerAngles + byAngles);
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            itemRotator.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }

        DisplayCurrentItemInfo();

        isTurning = false;
    }
}
