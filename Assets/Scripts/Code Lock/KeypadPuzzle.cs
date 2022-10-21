using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class KeypadPuzzle : MonoBehaviour, IInteractable
{
    public static KeypadPuzzle Instance { get; private set; }

    public bool isActive { get; private set; }

    [SerializeField] GameObject puzzleCam;
    [SerializeField] private TextMeshPro screenText;
    [SerializeField] private string answer;
    [SerializeField] private int maxCharacter;

    [SerializeField] UnityEvent OnCorrect;

    [SerializeField] UnityEvent OnWrongCorrect;

    [Header("Effect")]
    [SerializeField] private GameObject postProcessing;

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
        InteractionSystem.Instance.ForceCloseUI();
        puzzleCam.SetActive(true);
        GameManager.Instance.DisableMovement();
        postProcessing.SetActive(true);
        isActive = true;
    }

    public void ExitPuzzle()
    {
        puzzleCam.SetActive(false);
        GameManager.Instance.EnableMovement();
        isActive = false;
        InteractionSystem.Instance.enabled = true;
        postProcessing.SetActive(false);
    }

    public void Interact()
    {
        EnterPuzzle();
    }
}
