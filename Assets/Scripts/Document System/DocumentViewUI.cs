using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DocumentViewUI : MonoBehaviour
{
    public static DocumentViewUI Instance { get; private set; }

    [Header("Document UI")]
    [SerializeField] private GameObject documentUI;
    [SerializeField] private TextMeshProUGUI documentTitle;
    [SerializeField] private TextMeshProUGUI documentInfo;
    [SerializeField] private TextMeshProUGUI documentPage;
    [SerializeField] private GameObject nextPageButton;
    [SerializeField] private GameObject takeButton;

    [Header("Document Clear UI")]
    [SerializeField] private GameObject clearDocumentUI;
    [SerializeField] private TextMeshProUGUI clearDocumentTitle;
    [SerializeField] private TextMeshProUGUI clearDocumentInfo;

    [Header("Effect")]
    [SerializeField] private GameObject postProcessing;
    [SerializeField] private AudioClip openDocumentSound;

    [Header("Debug")]
    [SerializeField] private DocumentPickUp documentPickUp;
    [SerializeField] private DocumentSO currentDocumentSO;
    [SerializeField] private int documentIndex = 0;

    private bool inJornal = false;

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

    public void ShowNewDocument(DocumentPickUp documentPickUp)
    {
        this.documentPickUp = documentPickUp;
        DisplayDocument(documentPickUp.GetDocument());
    }


    public void DisplayDocument(DocumentSO documentSO, bool inJornal = false)
    {
        documentIndex = 0;
        this.inJornal = inJornal;
        currentDocumentSO = documentSO;
        documentTitle.text = currentDocumentSO.documentName;
        documentPage.text = $" Page: {documentIndex + 1}/{currentDocumentSO.documentPages.Length}";
        documentInfo.fontSize = currentDocumentSO.documentPageTextSize;
        documentInfo.text = currentDocumentSO.documentPages[documentIndex];
        nextPageButton.SetActive(currentDocumentSO.documentPages.Length > 0);
        takeButton.SetActive(!inJornal);

        GameManager.Instance.SwitchControl(GameManager.ControlMode.UIControl);
        postProcessing.SetActive(true);
        documentUI.SetActive(true);

        if (openDocumentSound != null) AudioHandler.Instance.PlaySfx(openDocumentSound, true);
    }


    public void ShowDocumentNextPage()
    {
        documentIndex = ++documentIndex % currentDocumentSO.documentPages.Length;
        documentInfo.text = currentDocumentSO.documentPages[documentIndex];
        documentPage.text = $" Page: {documentIndex + 1}/{currentDocumentSO.documentPages.Length}";
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

        if (inJornal) DocumentManager.Instance.OpenJornal();
        else GameManager.Instance.SwitchControl(GameManager.ControlMode.PlayerControl);
    }

    public void PickDocument()
    {
        PopUpMessage.Instance.ShowMessage("You picked up a document, Check Jornal", PopUpMessage.messageType.Normal);
        documentPickUp.PickDocument();
        CloseDocument();
    }
}
