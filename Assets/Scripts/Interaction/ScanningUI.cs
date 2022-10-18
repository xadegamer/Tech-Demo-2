using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScanningUI : MonoBehaviour {

    [SerializeField] private InteractionSystem interactionSystem;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI infoText;

    private void Start() 
    {
        interactionSystem.OnScanningObjectChanged += ScanningVision_OnScanningObjectChanged;
        Hide();
    }

    private void ScanningVision_OnScanningObjectChanged(object sender, System.EventArgs e) {
        if (sender != null) 
        {
            Show();
            ScannableObject scannableObject = ((GameObject)sender).GetComponent<ScannableObject>();
            nameText.text = scannableObject.scanName;
            infoText.text = scannableObject.scanAffiliation;
        } 
        else 
        {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

}
