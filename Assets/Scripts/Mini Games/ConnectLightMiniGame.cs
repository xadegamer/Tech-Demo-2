using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectLightMiniGame : MonoBehaviour
{
    [SerializeField] private Color[] colours;

    [SerializeField] private Image[] lightControllers;

    [SerializeField] private Image [] lightFillers;
    
    [SerializeField] private float fillSpeed;

    [Header("Debug")]

    [SerializeField] private bool forceCorrect;

    [SerializeField] private int lastColourIndex;

    public IEnumerator Start()
    {
        DisableLights();
        yield return StartCoroutine(FillLights());
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(EmptyLights());
    }

    IEnumerator FillLights()
    {
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

            CompareColor(lightFillers[i].color, lightControllers[i].color);
        }
    }

    IEnumerator EmptyLights()
    {
        for (int i = lightFillers.Length - 1; i >= 0; i--)
        {
            lightFillers[i].fillAmount = 1;
            
            lightFillers[i].color = RandomColour();

            if (forceCorrect) lightControllers[i].color = lightFillers[i].color;
            
            while (lightFillers[i].fillAmount > 0)
            {
                lightFillers[i].fillAmount -= Time.deltaTime * fillSpeed;
                yield return null;
            }

            CompareColor(lightFillers[i].color, lightControllers[i].color);
        }
    }

    public void DisableLights()
    {
        for (int i = 0; i < lightFillers.Length; i++)
        {
            lightFillers[i].fillAmount = 0;
        }
    }

    public void CompareColor(Color lightFillerColor, Color lightControllerColor)
    {
        if (lightFillerColor == lightControllerColor)
        {
            Debug.Log("Correct");
        }
        else
        {
            Debug.Log("Wrong");
        }
    }

    public Color RandomColour()
    {
        return colours[Random.Range(0, colours.Length)];
    }
}

