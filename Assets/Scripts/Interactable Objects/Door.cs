using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable, IScannable
{
    [SerializeField] private ScanInfo scanInfo;
    
    private Animator animator;
    private bool isOpen = false;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

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

    public void OpenDoor()
    {
        animator.Play("Open");
    }

    public void CloseDoor()
    {
        animator.Play("Close");
    }
}
