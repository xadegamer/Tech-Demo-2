using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScanningUI : MonoBehaviour 
{
    [Header("Scanning UI")]
    [SerializeField] private GameObject ui;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private int characterWarpLimit;

    [Header("Interaction UI")]
    [SerializeField] private GameObject interactionUI;
    [SerializeField] private TextMeshProUGUI actionText;

    private void Start() 
    {
        InteractionSystem.Instance.OnScanningObjectChanged += ScanningVision_OnScanningObjectChanged;
        ToggleObject(false);
        interactionUI.SetActive(false);
    }

    private void ScanningVision_OnScanningObjectChanged(object sender, System.EventArgs e) {
        if (sender != null) 
        {
            ui.SetActive(true);
            IScannable scannableObject = ((GameObject)sender).GetComponent<IScannable>();
            nameText.text = scannableObject.GetScanInfo().scanName;
            infoText.text = scannableObject.GetScanInfo().scanDescription;

            int headerLenth = nameText.text.Length;
            int contentLenght = infoText.text.Length;
            layoutElement.enabled = headerLenth > characterWarpLimit || contentLenght > characterWarpLimit;

            if (((GameObject)sender).TryGetComponent(out IInteractable interactableObject))
            {
                actionText.text = "Press" + "<color=yellow> [E] </color>" + interactableObject.GetInteractText();
                interactionUI.SetActive(true);
            }
        } 
        else 
        {
            interactionUI.SetActive(false);
            ui.SetActive(false);
        }
    }

    public void ToggleObject(bool toggle)
    {
        ui.SetActive(toggle);
    }

    public void ToggleInteractImage(bool toggle)
    {
        interactionUI.SetActive(toggle);
    }
}
