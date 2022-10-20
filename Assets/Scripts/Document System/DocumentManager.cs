using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentManager : MonoBehaviour
{
    public static DocumentManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddDocument(DocumentSO document)
    {
        // Add document to list
    }
}
