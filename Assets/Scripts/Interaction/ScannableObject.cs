using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannableObject : MonoBehaviour , IScannable
{
    public string scanName;
    public string scanDescription;
    public float scanSize = 0.05f;

    public string ScanName() => scanName;
    public string ScanDescription() => scanDescription;
    public float ScanSize() => scanSize;
}