using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class DocumentUI : MonoBehaviour
{
    public static DocumentUI Instance { get; private set; }

    [Header("Document UI")]
    [SerializeField] private GameObject documentUI;
    [SerializeField] private TextMeshProUGUI documentTitle;
    [SerializeField] private TextMeshProUGUI documentInfo;
    [SerializeField] private GameObject nextPageButton;

    [Header("Document Clear UI")]
    [SerializeField] private GameObject clearDocumentUI;
    [SerializeField] private TextMeshProUGUI clearDocumentTitle;
    [SerializeField] private TextMeshProUGUI clearDocumentInfo;

    [Header("Effect")]
    [SerializeField] private GameObject postProcessing;

    [Header("Debug")]
    [SerializeField] private DocumentPickUp documentPickUp;
    [SerializeField] private DocumentSO currentDocumentSO;
    [SerializeField] private int documentIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        documentUI.SetActive(false);
        clearDocumentUI.SetActive(false);
        nextPageButton.SetActive(false);
    }

    public void ShowDocument(DocumentPickUp documentPickUp)
    {
        GameManager.Instance.SwitchControl(GameManager.ControlMode.UIControl);

        postProcessing.SetActive(true);

        documentIndex = 0;

        this.documentPickUp = documentPickUp;
        currentDocumentSO = documentPickUp.GetDocument();
        documentTitle.text = currentDocumentSO.documentName;

        documentInfo.fontSize = currentDocumentSO.documentPageTextSize;
        documentInfo.text = currentDocumentSO.documentPages[documentIndex];

        nextPageButton.SetActive(currentDocumentSO.documentPages.Length > 0);

        documentUI.SetActive(true);
    }

    public void ShowDocumentNextPage()
    {
        documentIndex = ++documentIndex % currentDocumentSO.documentPages.Length;
        documentInfo.text = currentDocumentSO.documentPages[documentIndex];
    }

    public void ShowClearDocument()
    {
        clearDocumentTitle.text = documentTitle.text;

        clearDocumentInfo.fontSize = currentDocumentSO.documentPageTextSize;
        clearDocumentInfo.text = documentInfo.text;
        
        clearDocumentUI.SetActive(true);
    }

    public void HideClearDocument()
    {
        clearDocumentUI.SetActive(false);
    }

    public void CloseDocument()
    {
        documentUI.SetActive(false);
        postProcessing.SetActive(false);
        GameManager.Instance.SwitchControl(GameManager.ControlMode.PlayerControl);
    }

    public void PickDocument()
    {
        documentPickUp.PickDocument();
        CloseDocument();
    }
}
