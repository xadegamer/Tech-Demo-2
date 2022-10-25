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

    [Header("Scanning")]
    [SerializeField] private float scanningDistance;
    [SerializeField] private Material highlightMaterial;

    [Header("Interacting")]
    [SerializeField] private float interactHoldDuration;
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
        if (Physics.Raycast(playerCameraTranform.position, playerCameraTranform.forward, out RaycastHit raycastHit, scanningDistance))
        {
            CheckForScannableObject(raycastHit);
            DetectInteractable(raycastHit);
        }
        else
        {
            DeScanLastObject();
        }
    }

    public void CheckForScannableObject(RaycastHit raycastHit)
    {
        if (raycastHit.collider.TryGetComponent(out IScannable scannableObject))
        {
            if (lastActiveScannedGameObject != raycastHit.collider.gameObject)
            {
                if (lastActiveScannedGameObject != null)
                {
                    SetAllChildrenScanningSelected(lastActiveScannedGameObject, LayerMask.NameToLayer("Scannable"));
                    lastActiveScannedGameObject = null;
                }

                highlightMaterial.SetFloat("_Multiply_Value", raycastHit.collider.GetComponent<IScannable>().ScanSize());
                lastActiveScannedGameObject = raycastHit.collider.gameObject;
                SetAllChildrenScanningSelected(raycastHit.collider.gameObject, LayerMask.NameToLayer("Scanning"));
                OnScanningObjectChanged?.Invoke(lastActiveScannedGameObject, EventArgs.Empty);
            }
        }
        else DeScanLastObject();
    }
    

    public void DeScanLastObject()
    {
        if (lastActiveScannedGameObject != null)
        {
            SetAllChildrenScanningSelected(lastActiveScannedGameObject, LayerMask.NameToLayer("Scannable"));
            lastActiveScannedGameObject = null;
            OnScanningObjectChanged?.Invoke(lastActiveScannedGameObject, EventArgs.Empty);
        }
    }

    public void DetectInteractable(RaycastHit raycastHit)
    {
        if (Mouse.current.rightButton.wasPressedThisFrame && raycastHit.transform.TryGetComponent(out IInteractable interactable))
        {
            if (Vector3.Distance(transform.position, raycastHit.transform.position) < interactDistance)
            {
                interactable.Interact();
            }
            else
            {
                Debug.Log("Too far away to interact");
            }
        }


        //if (Vector3.Distance(transform.position, raycastHit.transform.position) < interactDistance)
        //{
        //    if (Input.GetKey(KeyCode.E) && raycastHit.transform.TryGetComponent(out IInteractable interactable))
        //    {
        //        if (InteractionUI.Instance.GetInteractBar().fillAmount < 1)
        //        {
        //            InteractionUI.Instance.GetInteractBar().fillAmount += 1 / interactHoldDuration * Time.deltaTime;
        //            if (InteractionUI.Instance.GetInteractBar().fillAmount == 1) interactable.Interact();
        //        }
        //    }
        //}
    }

    public void ForceScanningCloseUI()
    {
        if (lastActiveScannedGameObject != null) SetAllChildrenScanningSelected(lastActiveScannedGameObject, LayerMask.NameToLayer("Scannable"));
        lastActiveScannedGameObject = null;
        OnScanningObjectChanged?.Invoke(null, EventArgs.Empty);
    }

    public void SetAllChildrenScanningSelected(GameObject gameObject, int layer, bool force = false)
    {
        if(force || gameObject.layer == LayerMask.NameToLayer("Scanning") || gameObject.layer == LayerMask.NameToLayer("Scannable"))
        {
            gameObject.layer = layer;
        }
        
        foreach (Transform child in gameObject.transform)
        {
            SetAllChildrenScanningSelected(child.gameObject, layer, force);
        }
    }
}

public interface IInteractable
{
    public void Interact();
}

public interface IScannable
{
    public string ScanName();
    public string ScanDescription();
    public float ScanSize();
}
