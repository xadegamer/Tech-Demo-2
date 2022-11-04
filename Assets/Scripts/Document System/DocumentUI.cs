using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DocumentUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI documentNameDisplay;
    private DocumentSO documentSO;


    public void SetDocument(DocumentSO documentSO)
    {
        this.documentSO = documentSO;
        documentNameDisplay.text = documentSO.documentName;
    }

    public void OnClick()
    {
        DocumentManager.Instance.OpenDocument(documentSO);
    }
}
