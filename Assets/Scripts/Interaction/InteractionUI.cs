using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI Instance { get; private set; }

    [SerializeField] private Image interactionImage;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
    }

    public void ToggleImage(bool toggle)
    {
        gameObject.SetActive(toggle);
    }

    public void SetBarValue(float value)
    {
        interactionImage.fillAmount = value;
    }

    public Image GetInteractBar() => interactionImage;
}
