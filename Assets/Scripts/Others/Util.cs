using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static void SpawnItem(ItemSO itemSO, Transform transform, bool isParent = true)
    {
        GameObject item = GameObject.Instantiate(itemSO.itemPrefab, transform);
        item.transform.localScale = new Vector3(itemSO.pickupScale, itemSO.pickupScale, itemSO.pickupScale);
        item.GetComponent<Collider>().isTrigger = false;
        item.AddComponent<ItemPickUp>().SetUp(itemSO);
        item.AddComponent<Rigidbody>();
        if (!isParent) item.transform.SetParent(null);
    }
}
