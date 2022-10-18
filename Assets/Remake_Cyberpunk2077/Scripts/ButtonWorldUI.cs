using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWorldUI : MonoBehaviour, IPointerWorldUI {

    public event EventHandler OnPointerEnter;
    public event EventHandler OnPointerExit;
    public event EventHandler OnPointerDown;


    private Image image;

    private void Awake() {
        image = GetComponent<Image>();
    }

    public void PointerEnter() {
        //image.color = Color.green;
        OnPointerEnter?.Invoke(this, EventArgs.Empty);
    }

    public void PointerExit() {
        //image.color = Color.white;
        OnPointerExit?.Invoke(this, EventArgs.Empty);
    }

    public void PointerDown() {
        //image.color = Color.blue;
        OnPointerDown?.Invoke(this, EventArgs.Empty);
    }

}
