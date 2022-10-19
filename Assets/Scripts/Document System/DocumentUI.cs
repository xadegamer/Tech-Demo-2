using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DocumentUI : MonoBehaviour
{
    public static DocumentUI Instance { get; private set; }

    [Header("Document UI")]
    [SerializeField] private GameObject documentUI;
    [SerializeField] private TextMeshProUGUI documentTitle;
    [SerializeField] private TextMeshProUGUI documentInfo;

    [Header("Document Clear UI")]
    [SerializeField] private GameObject clearDocumentUI;
    [SerializeField] private TextMeshProUGUI clearDocumentTitle;
    [SerializeField] private TextMeshProUGUI clearDocumentInfo;

    [Header("Debug")]
    [SerializeField] private DocumentSO currentDocumentSO;
    [SerializeField] private int documentIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowDocument(DocumentSO documentSO)
    {
        documentIndex = 0;
        currentDocumentSO = documentSO;
        documentTitle.text = currentDocumentSO.documentName;
        documentInfo.text = currentDocumentSO.documentPages[documentIndex];
        documentUI.SetActive(true);
    }

    public void ShowNextDocumentPage()
    {
        documentIndex = ++documentIndex % currentDocumentSO.documentPages.Length;
        documentInfo.text = currentDocumentSO.documentPages[documentIndex];
    }

    public void ShowClearDocument()
    {
        clearDocumentUI.SetActive(true);
        documentTitle.text = documentTitle.text;
        documentInfo.text = documentInfo.text;
    }

    public void HideClearDocument()
    {
        clearDocumentUI.SetActive(false);
    }

    public void CloseDocument()
    {
        documentUI.SetActive(false);
    }
}
