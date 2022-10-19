using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionSystem : MonoBehaviour
{
    public static InteractionSystem Instance { get; private set; }

    public event EventHandler OnScanningObjectChanged;
    
    [SerializeField] private Transform playerCameraTranform;
    [SerializeField] private LayerMask interactLayerMask;
    [SerializeField] private float interactDistance;

    private GameObject lastActiveScannedGameObject;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
 
        DetectObject();
    }

    public void DetectObject()
    {      
        if (Physics.Raycast(playerCameraTranform.position, playerCameraTranform.forward, out RaycastHit raycastHit, interactDistance, interactLayerMask))
        {
            if (lastActiveScannedGameObject != raycastHit.collider.gameObject)
            {
                if (lastActiveScannedGameObject != null)
                {
                    SetAllChildrenScanningSelected(lastActiveScannedGameObject, LayerMask.NameToLayer("Scannable"));
                    lastActiveScannedGameObject = null;
                }

                lastActiveScannedGameObject = raycastHit.collider.gameObject;
                SetAllChildrenScanningSelected(raycastHit.collider.gameObject, LayerMask.NameToLayer("Scanning"));
                OnScanningObjectChanged?.Invoke(lastActiveScannedGameObject, EventArgs.Empty);
            }

            if (Mouse.current.rightButton.wasPressedThisFrame && raycastHit.transform.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact();
            }
        }
        else
        {
            if (lastActiveScannedGameObject != null)
            {
                SetAllChildrenScanningSelected(lastActiveScannedGameObject, LayerMask.NameToLayer("Scannable"));
                lastActiveScannedGameObject = null;
                OnScanningObjectChanged?.Invoke(lastActiveScannedGameObject, EventArgs.Empty);
            }
        }
    }

    public void DetectInteractable()
    {
        if (Physics.Raycast(playerCameraTranform.position, playerCameraTranform.forward, out RaycastHit raycastHit, interactDistance, interactLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact();
            }
        }
    }

    private void SetAllChildrenScanningSelected(GameObject gameObject, int layer)
    {
        gameObject.layer = layer;

        foreach (Transform child in gameObject.transform)
        {
            SetAllChildrenScanningSelected(child.gameObject, layer);
        }
    }
}

public interface IInteractable
{
    public void Interact();
}

