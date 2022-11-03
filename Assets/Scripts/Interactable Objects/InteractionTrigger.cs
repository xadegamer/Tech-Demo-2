using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionTrigger : MonoBehaviour, IInteractable, IScannable
{
    [SerializeField] private string actionText;
    [SerializeField] private ScanInfo scanInfo;
    [SerializeField] private UnityEvent<bool> OnInteract;

    private bool isInteracted = true;
    
    public string GetInteractText()
    {
        return actionText;
    }

    public ScanInfo GetScanInfo()
    {
        return scanInfo;
    }

    public void Interact()
    {
        isInteracted = !isInteracted;
        OnInteract?.Invoke(isInteracted);
    }
}
