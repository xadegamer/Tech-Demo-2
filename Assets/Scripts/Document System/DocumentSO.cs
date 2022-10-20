using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "New Document", menuName = "Documet System / Create Document", order = 1)]
public class DocumentSO : ScriptableObjectBase
{
    public int ID;
    public string documentName;

    [TextArea(5, 30)]
    public string[] documentPages;
    
    public Sprite documentIcon;
    public Color documentColor;

    [Header("Font Setting")]
    public float documentPageTextSize;
    public TMP_FontAsset documentFont;

    public override int GetID()
    {
        return ID;
    }
}
