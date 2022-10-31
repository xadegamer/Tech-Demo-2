using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class KeypadPuzzle : MonoBehaviour, IInteractable, IScannable
{
    public static KeypadPuzzle Instance { get; private set; }

    public bool isActive { get; private set; }

    [SerializeField] private bool useUI;

    [Header("UI")]
    [SerializeField] private GameObject screenTextFade;
    [SerializeField] private TextMeshProUGUI screenTextText;

    [Header("Mesh")]
    [SerializeField] private TextMeshPro screenText;

    [Header("Properties")]
    [SerializeField] private GameObject puzzleCam;
    [SerializeField] private string answer;
    [SerializeField] private int maxCharacter;
    [SerializeField] private Color normalColour;
    [SerializeField] private Color correctColor;
    [SerializeField] private Color wrongColour;

    [Header("Events")]
    [SerializeField] private UnityEvent OnCorrectInput;
    [SerializeField] private UnityEvent OnWrongInput;

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

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isActive)
        {
            ExitPuzzle();
        }
    }

    public void EnterKey(int key)
    {
        if (disableInput || inputString.Length >= maxCharacter) return;
        inputString += key.ToString();
        DisplayOnScreen(inputString);
    }
    public void DeleteKey()
    {
        if (!disableInput && inputString.Length > 0)
        {
            inputString = inputString.Substring(0, inputString.Length - 1);
            DisplayOnScreen(inputString);
        }
    }

    public void Clear()
    {
        if (!disableInput)
        {
            inputString = "";
            DisplayOnScreen(inputString);
        }
    }

    public void Confirm()
    {
        if (disableInput) return;

        disableInput = true;

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
        if (useUI)
        {
            screenTextText.text = input;
            screenTextFade.SetActive(input == "");
        } else screenText.text = input;
    }

    public void SetDisplayColour(Color color)
    {
        if (useUI) screenTextText.color = color; else screenText.color = color;
    }

    public void EnterPuzzle()
    {
        InteractionSystem.Instance.enabled = false;
        InteractionSystem.Instance.ForceScanningCloseUI();
        
        puzzleCam.SetActive(true);

        GameManager.Instance.SwitchControl(GameManager.ControlMode.UIControl);

        GameManager.Instance.TogglePlayerVisual(false);
        GameManager.Instance.DisableMovement();
        postProcessing.SetActive(true);
        isActive = true;
    }

    public IEnumerator CorrectCode()
    {
        OnCorrectInput.Invoke();
        SetDisplayColour(correctColor);
        yield return new WaitForSeconds(correctInputDelay);
        SetDisplayColour(normalColour);
        Clear();
        ExitPuzzle();
    }

    public IEnumerator IncorrectCode()
    {
        SetDisplayColour(wrongColour);
        yield return new WaitForSeconds(wrongInputDelay);
        disableInput = false;
        SetDisplayColour(normalColour);
        Clear();
        OnWrongInput.Invoke();
    }

    public void ExitPuzzle()
    {
        disableInput = false;
        puzzleCam.SetActive(false);     
        GameManager.Instance.TogglePlayerVisual(true);
        GameManager.Instance.EnableMovement();
        GameManager.Instance.SwitchControl(GameManager.ControlMode.PlayerControl);
        isActive = false;
        InteractionSystem.Instance.enabled = true;
        postProcessing.SetActive(false);
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
        return "Press [E] to interact";
    }
}
