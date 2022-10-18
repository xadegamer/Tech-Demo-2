using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScanningVision : MonoBehaviour {

    public event EventHandler OnScanningObjectChanged;

    private const int SCANNING_LAYER = 11;
    private const int SCANNINGSELECTED_LAYER = 12;


    [SerializeField] private Camera playerCamera = null;
    [SerializeField] private Camera cameraOverlay1 = null;
    [SerializeField] private Camera cameraOverlay2 = null;
    [SerializeField] private Volume postProcessingVolume = null;
    [SerializeField] private UniversalAdditionalCameraData additionalCameraData = null;
    [SerializeField] private LayerMask layerMask = default(LayerMask);

    private bool isActive;
    private GameObject lastActiveScannedGameObject;

    private void Start() {
        SetIsActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            SetIsActive(!isActive);
        }

        if (lastActiveScannedGameObject != null) {
            SetAllChildrenScanningSelected(lastActiveScannedGameObject, SCANNING_LAYER);
            lastActiveScannedGameObject = null;
            OnScanningObjectChanged?.Invoke(lastActiveScannedGameObject, EventArgs.Empty);
        }

        if (isActive) {
            if (Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit, 100f, layerMask)) {
                if (raycastHit.collider.gameObject.TryGetComponent<ScannableObject>(out ScannableObject scannableObject)) {
                    // This object can be scanned
                    SetAllChildrenScanningSelected(raycastHit.collider.gameObject, SCANNINGSELECTED_LAYER);

                    lastActiveScannedGameObject = raycastHit.collider.gameObject;
                    OnScanningObjectChanged?.Invoke(lastActiveScannedGameObject, EventArgs.Empty);
                }
            }
        }
    }

    private void SetAllChildrenScanningSelected(GameObject gameObject, int layer) {
        gameObject.layer = layer;

        foreach (Transform child in gameObject.transform) {
            SetAllChildrenScanningSelected(child.gameObject, layer);
        }
    }

    private void SetIsActive(bool isActive) {
        this.isActive = isActive;

        if (isActive) {
            cameraOverlay1.gameObject.SetActive(true);
            cameraOverlay2.gameObject.SetActive(true);
            postProcessingVolume.gameObject.SetActive(true);
            additionalCameraData.SetRenderer(3);
        } else {
            cameraOverlay1.gameObject.SetActive(false);
            cameraOverlay2.gameObject.SetActive(false);
            postProcessingVolume.gameObject.SetActive(false);
            additionalCameraData.SetRenderer(0);
        }
    }

}
