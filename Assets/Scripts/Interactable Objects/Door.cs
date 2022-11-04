using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable, IScannable
{
    [SerializeField] private ScanInfo scanInfo;
    [SerializeField] private GameObject cam;

    private Animator animator;
    private string actionText = "Open";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public string GetInteractText()
    {
        return "Open";
    }

    public ScanInfo GetScanInfo()
    {
        return scanInfo;
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

    public void CamOn()
    {
        cam.SetActive(true);
    }

    public void CamOff()
    {
        cam.SetActive(false);
    }
}
