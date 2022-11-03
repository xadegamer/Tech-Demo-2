using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable, IScannable
{
    [SerializeField] private ScanInfo scanInfo;

    public string GetInteractText()
    {
        throw new System.NotImplementedException();
    }

    public ScanInfo GetScanInfo()
    {
        throw new System.NotImplementedException();
    }

    public void Interact()
    {
        throw new System.NotImplementedException();
    }

    public void ToggleDoorState(bool state)
    {

    }
}
