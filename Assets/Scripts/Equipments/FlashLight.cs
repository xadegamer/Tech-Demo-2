using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlashLight : MonoBehaviour, IEquipment
{
    [Header("FlashLight Settings")]
    [SerializeField] private float brightness;
    
    [Header("FlashLight Events")]
    [SerializeField] private UnityEvent OnFlashLightOn;
    [SerializeField] private UnityEvent OnFlashLightOff;

    [Header("FlashLight Ref")]
    [SerializeField] private Light lightSourse;

    [Header("Debug")]
    [SerializeField] private bool isOn = false;
    [SerializeField] private bool failSafe = false;
    
    void Start()
    {
        lightSourse.enabled = isOn;
    }
    
    public void Use()
    {
        if (failSafe) return;
        isOn = !isOn;
        lightSourse.enabled = isOn;

        if (isOn)
        {
            failSafe = false;
            OnFlashLightOn?.Invoke();
            StartCoroutine(FailSafe());
        }
        else OnFlashLightOff?.Invoke();
    }

    IEnumerator FailSafe()
    {
        yield return new WaitForSeconds(0.25f);
        failSafe = false;
    }
}
