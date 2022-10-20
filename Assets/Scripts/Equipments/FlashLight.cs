using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public bool isOn = false;
    public GameObject lightSourse;
    public AudioSource clickSound;
    public bool failSafe = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input. GetMouseButtonDown (0))
        {
            if(isOn == false && failSafe == false)
            {
                failSafe = true;
                lightSourse.SetActive(true);
                //  clickSound.Play();
                isOn = true;
                StartCoroutine(FailSafe());
            }
            if (isOn == true && failSafe == false)
            {
                failSafe = true;
                lightSourse.SetActive(false);
                //  clickSound.Play();
                isOn = false;
                StartCoroutine(FailSafe());
            }
        }
    }

    IEnumerator FailSafe()
    {
        yield return new WaitForSeconds(0.25f);
        failSafe = false;
    }
}
