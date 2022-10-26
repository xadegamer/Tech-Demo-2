using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentManager : MonoBehaviour
{
    public static DocumentManager Instance { get; private set; }

    [SerializeField] private List<DocumentSO> documents = new List<DocumentSO>();

    private void Awake()
    {
        Instance = this;
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
}
