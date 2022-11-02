using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public AudioSource sfx;
    public AudioClip toggle, select;
    public void Toggle()
    {
        sfx.PlayOneShot(toggle);
    }
    public void Selected()
    {
        sfx.PlayOneShot(select);
    }
}
