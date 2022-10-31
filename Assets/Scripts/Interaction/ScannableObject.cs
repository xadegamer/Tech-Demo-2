using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannableObject : MonoBehaviour , IScannable
{
    [SerializeField] private ScanInfo scanInfo;

    public ScanInfo GetScanInfo()
    {
        return scanInfo;
    }
}