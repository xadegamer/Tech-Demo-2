using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentPickUp : MonoBehaviour, IInteractable,IScannable
{
    [SerializeField] private DocumentSO documentSO;

    private ScanInfo scanInfo;

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

    public ScanInfo GetScanInfo()
    {
        scanInfo = new ScanInfo();
        scanInfo.scanName = documentSO.documentName;
        scanInfo.scanDescription = documentSO.documentDescription;
        scanInfo.scanSize = documentSO.scanSize;
        return scanInfo;
    }

    public string GetInteractText()
    {
        return "to read";
    }
}
