using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConnectLightMiniGame : MonoBehaviour, IInteractable, IScannable
{
    [SerializeField] private Color[] colours;
    [SerializeField] private Image[] lightControllers;
    [SerializeField] private Image [] lightFillers; 
    [SerializeField] private float fillSpeed;

    [Header("Scanning")]
    [SerializeField] private ScanInfo scanInfo;

    [Header("Properties")]
    [SerializeField] private GameObject puzzleUIDisplay;
    [SerializeField] private GameObject puzzleCam;
    [SerializeField] private GameObject postProcessing;

    [Header("Events")]
    [SerializeField] private UnityEvent OnCorrectInput;
    [SerializeField] private UnityEvent OnWrongInput;
    [SerializeField] private UnityEvent OnComplected;

    [Header("Debug")]
    [SerializeField] private bool forceCorrect;
    [SerializeField] private int lastColourIndex;

    public void EnterPuzzle()
    {
        InteractionSystem.Instance.enabled = false;
        InteractionSystem.Instance.ForceScanningCloseUI();

        GameManager.Instance.SwitchControl(GameManager.ControlMode.UIControl);
        GameManager.Instance.TogglePlayerVisual(false);
        GameManager.Instance.DisableMovement();

        puzzleCam.SetActive(true);
        puzzleUIDisplay.SetActive(true);
        postProcessing.SetActive(true);

        StartMiniGame();
    }

    public void EnterExit()
    {
        puzzleCam.SetActive(false);
        postProcessing.SetActive(false);
        puzzleUIDisplay.SetActive(false);

        GameManager.Instance.TogglePlayerVisual(true);
        GameManager.Instance.EnableMovement();
        GameManager.Instance.SwitchControl(GameManager.ControlMode.PlayerControl);

        InteractionSystem.Instance.enabled = true;
    }

    public void StartMiniGame()
    {
        GameManager.Instance.SwitchControl(GameManager.ControlMode.UIControl);
        StartCoroutine(FillLights());
    }

    IEnumerator FillLights()
    {
        DisableLights();
        
        for (int i = 0; i < lightFillers.Length; i++)
        {
            lightFillers[i].fillAmount = 0;
            
            lightFillers[i].color = RandomColour();

            if (forceCorrect) lightControllers[i].color = lightFillers[i].color;

            while (lightFillers[i].fillAmount < 1)
            {
                lightFillers[i].fillAmount += Time.deltaTime * fillSpeed;
                yield return null;
            }

            if (lightFillers[i].color != lightControllers[i].color)
            {
                StartMiniGame();
                yield break;
            }
        }

        yield return new WaitForSeconds(2f);

        StartCoroutine(EmptyLights());
    }

    IEnumerator EmptyLights()
    {
        for (int i = lightFillers.Length - 1; i >= 0; i--)
        {
            lightFillers[i].fillAmount = 1;
            
            lightFillers[i].color = RandomColour();

            if (forceCorrect)
            {
                if (i == 0) lightControllers[lightControllers.Length - 1].color = lightFillers[i].color;
                else lightControllers[i - 1].color = lightFillers[i].color;
            }
            
            while (lightFillers[i].fillAmount > 0)
            {
                lightFillers[i].fillAmount -= Time.deltaTime * fillSpeed;
                yield return null;
            }

            if(i == 0)
            {
                if (lightFillers[i].color != lightControllers[lightControllers.Length - 1].color)
                {
                    StartMiniGame();
                    yield break;
                }
            }
            else
            {
                if (lightFillers[i].color != lightControllers[i - 1].color)
                {
                    StartMiniGame();
                    yield break;
                }
            }
        }

        yield return new WaitForSeconds(2f);

        GameComplected();
    }

    public void DisableLights()
    {
        for (int i = 0; i < lightFillers.Length; i++)
        {
            lightFillers[i].fillAmount = 0;
        }

        for (int i = 0; i < lightControllers.Length; i++)
        {
            lightControllers[i].color = RandomColour();
        }
    }

    public void CompareColor(Color lightFillerColor, Color lightControllerColor)
    {
        if (lightFillerColor == lightControllerColor) Pass(); else Fail();
    }

    public void Pass()
    {
        OnCorrectInput?.Invoke();
    }

    public void Fail()
    {
        OnWrongInput?.Invoke();
        StartMiniGame();
    }

    public void GameComplected()
    {
        EnterExit();
        OnComplected?.Invoke();
    }

    public void LightControllerClick(Image image)
    {
        int colourIndex = System.Array.IndexOf(colours, image.color);
        image.color = colours[++colourIndex % colours.Length];
    }

    public Color RandomColour()
    {
        int randomClourIndex = UnityEngine.Random.Range(0, colours.Length);
        while (randomClourIndex == lastColourIndex)
        {
            randomClourIndex = UnityEngine.Random.Range(0, colours.Length);
        }
        lastColourIndex = randomClourIndex;
        return colours[lastColourIndex];
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

