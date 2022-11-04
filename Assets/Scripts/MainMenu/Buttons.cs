using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    [SerializeField] private AudioClip toggle, select;
    public void Toggle()
    {
        AudioHandler.Instance.PlaySfx(toggle, true);
    }
    public void Selected()
    {
        AudioHandler.Instance.PlaySfx(select, true);
    }
}
