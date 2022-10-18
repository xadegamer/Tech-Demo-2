using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ATM_UI : MonoBehaviour {

    [SerializeField] private ButtonWorldUI button1;
    [SerializeField] private ButtonWorldUI button2;
    [SerializeField] private ButtonWorldUI button3;
    [SerializeField] private ButtonWorldUI button4;
    [SerializeField] private ButtonWorldUI button5;
    [SerializeField] private ButtonWorldUI button6;
    [SerializeField] private ButtonWorldUI button7;
    [SerializeField] private ButtonWorldUI button8;
    [SerializeField] private ButtonWorldUI button9;
    [SerializeField] private TextMeshProUGUI textMesh;

    private string code;

    private void Start() {
        button1.OnPointerDown += (object send, EventArgs e) => { AddCode("1"); };
        button2.OnPointerDown += (object send, EventArgs e) => { AddCode("2"); };
        button3.OnPointerDown += (object send, EventArgs e) => { AddCode("3"); };
        button4.OnPointerDown += (object send, EventArgs e) => { AddCode("4"); };
        button5.OnPointerDown += (object send, EventArgs e) => { AddCode("5"); };
        button6.OnPointerDown += (object send, EventArgs e) => { AddCode("6"); };
        button7.OnPointerDown += (object send, EventArgs e) => { AddCode("7"); };
        button8.OnPointerDown += (object send, EventArgs e) => { AddCode("8"); };
        button9.OnPointerDown += (object send, EventArgs e) => { AddCode("9"); };

        code = "";
        textMesh.text = "";
    }

    private void AddCode(string str) {
        string newCode = "";
        for (int i = 0; i < code.Length; i++) {
            newCode += "*";
        }
        newCode += str;

        code = newCode;
        textMesh.text = code;
    }

}
