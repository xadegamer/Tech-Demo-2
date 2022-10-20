using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentPickUp : MonoBehaviour, IInteractable
{
    [SerializeField] private DocumentSO documentSO;

    public DocumentSO GetDocument() => documentSO;

    public void Interact()
    {
        DocumentUI.Instance.ShowDocument(this);
    }

    public void PickDocument()
    {
        //DocumentManager.Instance.AddDocument(documentSO);
        Destroy(gameObject);
    }
}
