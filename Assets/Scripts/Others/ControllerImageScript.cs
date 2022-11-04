using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControllerImageScript : MonoBehaviour
{
    private Image buttonImage;

    // refs to your sprites
    public Sprite gamepadImage;
    public Sprite keyboardImage;

    void Awake()
    {
        buttonImage = GetComponent<Image>();
        PlayerInput input = FindObjectOfType<PlayerInput>();
        updateButtonImage(input.currentControlScheme);
    }

    private void Start()
    {
        InputUser.onChange += onInputDeviceChange;

    }

    void OnEnable()
    {
       // InputUser.onChange += onInputDeviceChange;
    }

    void OnDisable()
    {
      //  InputUser.onChange -= onInputDeviceChange;
    }

    void onInputDeviceChange(InputUser user, InputUserChange change, InputDevice device)
    {
        if (change == InputUserChange.ControlSchemeChanged)
        {
            updateButtonImage(user.controlScheme.Value.name);
        }
    }

    void updateButtonImage(string schemeName)
    {
        // assuming you have only 2 schemes: keyboard and gamepad
        if (schemeName.Equals("Gamepad"))
        {
            buttonImage.sprite = gamepadImage;
        }
        else
        {
            buttonImage.sprite = keyboardImage;
        }
    }
}
