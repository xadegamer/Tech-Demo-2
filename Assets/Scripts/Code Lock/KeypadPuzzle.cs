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

    [SerializeField] private UnityEvent OnCorrectInput;

    [SerializeField] private UnityEvent OnWrongInput;

    [Header("Effect")]
    [SerializeField] private GameObject postProcessing;
    [SerializeField] private float correctInputDelay;
    [SerializeField] private float wrongInputDelay;


    [Header("Scanning")]
    [SerializeField] private string scanName;
    [SerializeField] private string scanDescription;
    [SerializeField] private float scanSize = 0.05f;

    private bool disableInput = false;


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
        if (disableInput || screenText.text.Length >= maxCharacter) return;
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
            StartCoroutine(CorrectCode());
        }
        else
        {
            StartCoroutine(IncorrectCode());
        }
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
        disableInput = true;
        OnCorrectInput.Invoke();
        screenText.color = Color.green;
        yield return new WaitForSeconds(correctInputDelay);
        screenText.color = Color.white;
        screenText.text = "";
        disableInput = false;
        ExitPuzzle();
    }

    public IEnumerator IncorrectCode()
    {
        disableInput = true;
        screenText.color = Color.red;
        yield return new WaitForSeconds(wrongInputDelay);
        screenText.color = Color.white;
        screenText.text = "";
        disableInput = false;
        OnWrongInput.Invoke();
    }

    public void ExitPuzzle()
    {
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

    public string ScanName() => scanName;
    public string ScanDescription() => scanDescription;
    public float ScanSize() => scanSize;
}
