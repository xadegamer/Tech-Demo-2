using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using static DialogSO;

public class DialogManager : MonoBehaviour
{
    [System.Serializable]
    public class Dialog
    {
        public DialogSO dialogObject;
        public UnityEvent OnBegin;
        public UnityEvent OnFinsihed;
    }

    public static DialogManager Instance { get; private set; }

    [Header("Properties")]
    [SerializeField] private GameObject popUpBox;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TextMeshProUGUI dialogBoxText;
    [SerializeField] private float delay = 0.1f;

    [Header("Dialog")]
    [SerializeField] private Dialog[] dialogs;

    [Header("Debug")]
    [SerializeField] private Dialog currentDialog;
    private TextMeshProUGUI dialogText;
    private string fullText;
    private string currentText = "";

    private bool isTyping = false;
    private bool isPlayingVoiceOver = false;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        dialogBoxText.text = "";
        dialogText = dialogBoxText;

        PlayDialog(0);
    }

    public void PlayDialog(int dialogIndex)
    {
        currentDialog = dialogs[dialogIndex];
        currentDialog.OnBegin.Invoke();
        popUpBox.SetActive(true);
        StartCoroutine(DialogRoutine(currentDialog.dialogObject.textSequences));
    }

    IEnumerator PlayText()
    {
        isTyping = true;
        foreach (char letter in fullText)
        {
            currentText = dialogText.text += letter;
            yield return new WaitForSeconds(delay);
        }
        isTyping = false;
    }

    IEnumerator PlayVoiceOver(AudioClip audioClip)
    {
        isPlayingVoiceOver = true;
        audioSource.PlayOneShot(audioClip);
        yield return new WaitForSecondsRealtime(audioClip.length);
        isPlayingVoiceOver = false;
    }

    IEnumerator DialogRoutine(TextSequence[] textSequences)
    {
        for (int i = 0; i < textSequences.Length; i++)
        {
            dialogBoxText.text = "";
            fullText = textSequences[i].messages;

            isTyping = true;
            isPlayingVoiceOver = true;

            yield return new WaitForSecondsRealtime(textSequences[i].audioClip.length);

            StartCoroutine(PlayText());

            StartCoroutine(PlayVoiceOver(textSequences[i].audioClip));

            yield return new WaitUntil(() => !isTyping && !isPlayingVoiceOver);
        }

        popUpBox.SetActive(false);
        currentDialog.OnFinsihed.Invoke();
    }
    
    public void SkipDialog()
    {
        StopAllCoroutines();
        popUpBox.SetActive(false);
        currentDialog.OnFinsihed.Invoke();
    }
}