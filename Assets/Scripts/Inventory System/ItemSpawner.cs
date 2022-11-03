using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private ItemSO itemSO;

    void Start()
    {
       Util.SpawnItem(itemSO, transform);
    }
}
