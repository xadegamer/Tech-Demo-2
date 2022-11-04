using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using StarterAssets;

public class KeypadPuzzle : MonoBehaviour, IInteractable, IScannable
{
    public static KeypadPuzzle Instance { get; private set; }

    public bool isActive { get; private set; }
    
    [Header("UI")]
    [SerializeField] private bool useUI;
    [SerializeField] private Image textBackground;
    [SerializeField] private TextMeshProUGUI screenTextText;

    [Header("Grid")]
    [SerializeField] private GameObject gridInstructionUI;
    [SerializeField] private TextMeshPro screenText;
    [SerializeField] private GameObject screenObject;

    [Header("Properties")]
    [SerializeField] private GameObject puzzleUI;
    [SerializeField] private GameObject screenTextFade;
    [SerializeField] private GameObject puzzleCam;
    [SerializeField] private string answer;
    [SerializeField] private int maxCharacter;
    [SerializeField] private Color normalColour;
    [SerializeField] private Color correctColor;
    [SerializeField] private Color wrongColour;

    [Header("Events")]
    [SerializeField] private UnityEvent OnKeyPressed;
    [SerializeField] private UnityEvent OnCorrectInput;
    [SerializeField] private UnityEvent OnWrongInput;
    [SerializeField] private UnityEvent OnFirstAttempt;

    [Header("Effect")]
    [SerializeField] private GameObject postProcessing;
    [SerializeField] private float correctInputDelay;
    [SerializeField] private float wrongInputDelay;

    [Header("Scanning")]
    [SerializeField] private ScanInfo scanInfo;

    [Header("Interactiong")]
    [SerializeField] private string actionText;

    private bool disableInput = false;
    private string inputString = "";
    private bool complected = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (StarterAssetsInputs.Instance.exit && isActive)
        {
            StarterAssetsInputs.Instance.exit = false;
            ExitPuzzle();
        }
    }

    public void EnterPuzzle()
    {
        StarterAssetsInputs.SwitchActionMap("Keypad Grid Puzzle");
        puzzleCam.SetActive(true);

        GameManager.Instance.SwitchControl(GameManager.ControlMode.UIControl);
        postProcessing.SetActive(true);
        isActive = true;

        puzzleUI.SetActive(true);
        gridInstructionUI.SetActive(!useUI);

        if (!useUI)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public IEnumerator CorrectCode()
    {
        OnCorrectInput.Invoke();
        SetDisplayColour(correctColor);
        yield return new WaitForSeconds(correctInputDelay);
        SetDisplayColour(normalColour);
        disableInput = false;
        Clear();

        if (!complected)
        {
            complected = true;
            OnFirstAttempt.Invoke();
            yield return new WaitForSeconds(2);
            ExitPuzzle();
        }
        else ExitPuzzle();
    }

    public IEnumerator IncorrectCode()
    {
        OnWrongInput.Invoke();
        SetDisplayColour(wrongColour);
        yield return new WaitForSeconds(wrongInputDelay);
        disableInput = false;
        SetDisplayColour(normalColour);
        Clear();
    }

    public void ExitPuzzle()
    {
        disableInput = false;
        puzzleCam.SetActive(false);
        GameManager.Instance.SwitchControl(GameManager.ControlMode.PlayerControl);
        isActive = false;
        puzzleUI.SetActive(false);
        postProcessing.SetActive(false);
    }

    public void EnterKey(int key)
    {
        if (disableInput || inputString.Length >= maxCharacter) return;
        inputString += key.ToString();
        DisplayOnScreen(inputString);
        OnKeyPressed?.Invoke(); 
    }
    public void DeleteKey()
    {
        if (!disableInput && inputString.Length > 0)
        {
            inputString = inputString.Substring(0, inputString.Length - 1);
            DisplayOnScreen(inputString);
            OnKeyPressed?.Invoke();
        }
    }

    public void Clear()
    {
        if (!disableInput)
        {
            inputString = "";
            DisplayOnScreen(inputString);
            OnKeyPressed?.Invoke();
        }
    }

    public void Confirm()
    {
        if (disableInput) return;

        disableInput = true;

        OnKeyPressed?.Invoke();

        if (inputString == answer)
        {
            StartCoroutine(CorrectCode());
        }
        else
        {
            StartCoroutine(IncorrectCode());
        }
    }

    public void DisplayOnScreen(string input)
    {
        screenTextFade.SetActive(input == "");
        
        if (useUI) screenTextText.text = input;
        else screenText.text = input;
    }

    public void SetDisplayColour(Color color)
    {
        if (useUI) textBackground.color = color; else screenObject.GetComponent<MeshRenderer>().material.color = color;
    }


    public void Interact()
    {
        EnterPuzzle();
    }

    public ScanInfo GetScanInfo()
    {
       return scanInfo;
    }

    public string GetInteractText()
    {
        return "to interact";
    }
}
