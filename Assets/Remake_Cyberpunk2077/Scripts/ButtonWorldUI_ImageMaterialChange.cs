using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonWorldUI_ImageMaterialChange : MonoBehaviour {

    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material overMaterial;

    private Image image;

    private void Awake() {
        image = GetComponent<Image>();

    }

    private void Start() {
        ButtonWorldUI buttonWorldUI = GetComponent<ButtonWorldUI>();
        buttonWorldUI.OnPointerEnter += (object sender, EventArgs e) => { image.material = overMaterial; };
        buttonWorldUI.OnPointerExit += (object sender, EventArgs e) => { image.material = normalMaterial; };

        image.material = normalMaterial;
    }

}
