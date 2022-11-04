using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpMessage : MonoBehaviour
{
    public static PopUpMessage Instance { get; private set; }

    public enum messageType { Normal, Error}

    [SerializeField] private Color normalColor;
    [SerializeField] private Color errorColor;
    [SerializeField] GameObject popUpUI;
    [SerializeField] TextMeshProUGUI messageDisplay;
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private int characterWarpLimit;
    [SerializeField] AudioClip notifySound;

    Coroutine coroutine;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowMessage(string message, messageType messageType)
    {
        if (coroutine != null)
        {
            popUpUI.SetActive(false);
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(DisplayRoutine(message, messageType));
    }

    public IEnumerator DisplayRoutine(string message, messageType messageType)
    {
        switch (messageType)
        {
            case messageType.Normal: messageDisplay.color = normalColor; break;
            case messageType.Error: messageDisplay.color = errorColor; break;
        }
        messageDisplay.text = message;
        int headerLenth = messageDisplay.text.Length;
        layoutElement.enabled = headerLenth > characterWarpLimit;
        popUpUI.SetActive(true);
        AudioHandler.Instance.PlaySfx(notifySound, true);

        yield return new WaitForSeconds(2f);
        popUpUI.SetActive(false);
    }
}
