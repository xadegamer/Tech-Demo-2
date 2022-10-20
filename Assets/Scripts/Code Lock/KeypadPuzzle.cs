using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEngine.Rendering.DebugUI;

public class KeypadPuzzle : MonoBehaviour
{
    public static KeypadPuzzle Instance { get; private set; }

    [SerializeField] private TextMeshPro screenText;
    [SerializeField] private string answer;

    private void Awake()
    {
        Instance = this;
    }

    public void EnterKey(int key)
    {
        screenText.text += key.ToString();
    }
    public void DeleteKey()
    {
        if(screenText.text.Length > 0) screenText.text = screenText.text.Substring(0, screenText.text.Length - 1);
    }

    public void Clear()
    {
        screenText.text = "";
    }

    public void Confirm()
    {
        if (screenText.text == answer)
        {
            Debug.Log("Correct Code");
        }
        else
        {
            Debug.Log("Incorrect Code");
        }
    }
}
