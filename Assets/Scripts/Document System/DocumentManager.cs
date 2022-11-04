using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentManager : MonoBehaviour
{
    public static DocumentManager Instance { get; private set; }

    [SerializeField] private List<DocumentSO> documents = new List<DocumentSO>();

    [Header("Document UI")]
    [SerializeField] private GameObject jornalUI;
    [SerializeField] private GameObject documentPrefab;
    [SerializeField] private Transform documentParent;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (jornalUI.activeInHierarchy) CloseJornal(); 
            else if (GameManager.Instance.GetCurrentControlMode() == GameManager.ControlMode.PlayerControl) OpenJornal();
        }
    }

    public void AddDocument(DocumentSO document)
    {
        documents.Add(document);
    }

    public void RemoveDocument(DocumentSO document)
    {
        documents.Remove(document);
    }

    public List<DocumentSO> GetDocuments() => documents;

    public void DisplayDocuments()
    {
        foreach (Transform transform in documentParent)
        {
            Destroy(transform.gameObject);
        }
        
        foreach (DocumentSO document in documents)
        {
            GameObject documentObject = Instantiate(documentPrefab, documentParent);
            documentObject.GetComponent<DocumentUI>().SetDocument(document);
        }
    }

    public void OpenJornal()
    {
        DisplayDocuments();
        jornalUI.SetActive(true);
        GameManager.Instance.SwitchControl(GameManager.ControlMode.UIControl);
    }

    public void CloseJornal()
    {
        jornalUI.SetActive(false);
        GameManager.Instance.SwitchControl(GameManager.ControlMode.PlayerControl);
    }

    internal void OpenDocument(DocumentSO documentSO)
    {
        jornalUI.SetActive(false);
        DocumentViewUI.Instance.DisplayDocument(documentSO, true);
    }
}
