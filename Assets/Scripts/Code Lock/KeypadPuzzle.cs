using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class KeypadPuzzle : MonoBehaviour, IInteractable, IScannable
{
    public static KeypadPuzzle Instance { get; private set; }

    public bool isActive { get; private set; }

    [SerializeField] private GameObject puzzleCam;
    [SerializeField] private TextMeshPro screenText;
    [SerializeField] private string answer;
    [SerializeField] private int maxCharacter;

    [SerializeField] private UnityEvent OnCorrect;

    [SerializeField] private UnityEvent OnWrongCorrect;

    [Header("Effect")]
    [SerializeField] private GameObject postProcessing;

    [Header("Scanning")]
    [SerializeField] private string scanName;
    [SerializeField] private string scanDescription;
    [SerializeField] private float scanSize = 0.05f;

    private void Awake()
    {
        Instance = this;
    }

    public void EnterKey(int key)
    {
        if (screenText.text.Length >= maxCharacter) return;
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
            ExitPuzzle();
        }
    }

    public void EnterPuzzle()
    {
        InteractionSystem.Instance.enabled = false;
        InteractionSystem.Instance.ForceScanningCloseUI();
        puzzleCam.SetActive(true);
        GameManager.Instance.TogglePlayerVisual(false);
        GameManager.Instance.DisableMovement();
        postProcessing.SetActive(true);
        isActive = true;
    }

    public void ExitPuzzle()
    {
        puzzleCam.SetActive(false);
        GameManager.Instance.TogglePlayerVisual(true);
        GameManager.Instance.EnableMovement();
        isActive = false;
        InteractionSystem.Instance.enabled = true;
        postProcessing.SetActive(false);
    }

    public void Interact()
    {
        EnterPuzzle();
    }

    public string ScanName() => scanName;
    public string ScanDescription() => scanDescription;
    public float ScanSize() => scanSize;
}
