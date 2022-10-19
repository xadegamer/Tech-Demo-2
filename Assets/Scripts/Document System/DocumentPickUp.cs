using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentPickUp : MonoBehaviour, IInteractable
{
    [SerializeField] private DocumentSO documentSO;
    
    public void Interact()
    {
        DocumentUI.Instance.ShowDocument(documentSO);
    }
}
