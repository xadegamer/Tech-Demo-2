using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScanningUI : MonoBehaviour 
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI infoText;

    private void Start() 
    {
        InteractionSystem.Instance.OnScanningObjectChanged += ScanningVision_OnScanningObjectChanged;
        ToggleObject(false);
    }

    private void ScanningVision_OnScanningObjectChanged(object sender, System.EventArgs e) {
        if (sender != null) 
        {
            gameObject.SetActive(true);
            IScannable scannableObject = ((GameObject)sender).GetComponent<IScannable>();
            nameText.text = scannableObject.ScanName();
            infoText.text = scannableObject.ScanDescription();
        } 
        else 
        {
            gameObject.SetActive(false);
        }
    }

    public void ToggleObject(bool toggle)
    {
        gameObject.SetActive(toggle);
    }
}
