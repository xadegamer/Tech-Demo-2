using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Keypad : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro;
    [SerializeField] private int key;
    [SerializeField] private KeyType keyType;
    [SerializeField] private Transform visual;

    public enum KeyType
    {
        Number,
        Clear,
        Delete,
        Confirm
    }

    public void SetUp(Vector3 scale, KeyType keyType, int key = 0)
    {
        this.keyType = keyType;
        this.key = key;

        switch (keyType)
        {
            case KeyType.Number:
                textMeshPro.text = key.ToString();
                break;
            case KeyType.Clear:
                textMeshPro.text = keyType.ToString();
                break;
            case KeyType.Delete:
                textMeshPro.text = keyType.ToString();
                break;
            case KeyType.Confirm:
                textMeshPro.text = keyType.ToString();
                break;
        }

        visual.localScale = scale;
    }
    
    public void OnPressed()
    {
        switch (keyType)
        {
            case KeyType.Number:
                KeypadPuzzle.Instance.EnterKey(key);
                break;
            case KeyType.Clear:
                KeypadPuzzle.Instance.Clear();
                break;
            case KeyType.Delete:
                KeypadPuzzle.Instance.DeleteKey();
                break;
            case KeyType.Confirm:
                KeypadPuzzle.Instance.Confirm();
                break;
        }
    }

    public void OnSelected(Material material)
    {
        visual.GetComponent<MeshRenderer>().material = material;
    }
}
