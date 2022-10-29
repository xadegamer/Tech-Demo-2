using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentPickUp : MonoBehaviour, IInteractable,IScannable
{
    [SerializeField] private DocumentSO documentSO;

    public DocumentSO GetDocument() => documentSO;

    public void Interact()
    {
        DocumentViewUI.Instance.ShowDocument(this);
    }

    public void PickDocument()
    {
        DocumentManager.Instance.AddDocument(documentSO);
        Destroy(gameObject);
    }

    public string ScanDescription()
    {
        return documentSO.documentDescription;
    }

    public string ScanName()
    {
        return documentSO.documentName;
    }

    public float ScanSize()
    {
        return documentSO.scanSize;
    }
}
