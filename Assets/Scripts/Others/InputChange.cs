using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputChange : MonoBehaviour
{
    UnityEngine.InputSystem.PlayerInput _controls;
    public static ControlDeviceType currentControlDevice;

    public enum ControlDeviceType
    {
        KeyboardAndMouse,
        Gamepad,
    }

    void Start()
    {
        _controls = GetComponent<UnityEngine.InputSystem.PlayerInput>();
         _controls.onControlsChanged += OnControlsChanged;
    }
    private void OnControlsChanged(UnityEngine.InputSystem.PlayerInput obj)
    {
        switch (obj.currentControlScheme)
        {
            case "KeyboardAndMouse":
                if (currentControlDevice != ControlDeviceType.KeyboardAndMouse) currentControlDevice = ControlDeviceType.KeyboardAndMouse;
                break;
            case "Gamepad":
                if (currentControlDevice != ControlDeviceType.Gamepad) currentControlDevice = ControlDeviceType.Gamepad;
                break;
        }
    }

    #region Input Detection
    private void GameManagerRegisterInput()
    {
        //Binds onDeviceChange event to InputDeviceChanged
        InputSystem.onDeviceChange += InputDeviceChanged;
    }

    public void InputDeviceChanged(InputDevice device, InputDeviceChange change)
    {
        Debug.Log("Device: " + device + " changed: " + change);

        switch (change)
        {
            //New device added
            case InputDeviceChange.Added:
                Debug.Log("New device added");

                //Checks if is Playstation Controller
                if (device.description.manufacturer == "Sony Interactive Entertainment")
                {
                    //Sets UI scheme
                    Debug.Log("Playstation Controller Detected");

                }
                //Else, assumes Xbox controller
                //device.description.manufacturer for Xbox returns empty string
                else
                {
                    Debug.Log("Xbox Controller Detected");

                }
                break;

            //Device disconnected
            case InputDeviceChange.Disconnected:

                Debug.Log("Device disconnected");
                break;

            //Familiar device connected
            case InputDeviceChange.Reconnected:

                Debug.Log("Device reconnected");

                //Checks if is Playstation Controller
                if (device.description.manufacturer == "Sony Interactive Entertainment")
                {
                    //Sets UI scheme
                    Debug.Log("Playstation Controller Detected");
                    ;
                }
                //Else, assumes Xbox controller
                //device.description.manufacturer for Xbox returns empty string
                else
                {
                    Debug.Log("Xbox Controller Detected");

                }
                break;

            //Else
            default:
                break;
        }
    }

    private void UIImageSchemeInitialSet()
    {
        //Disables all devices currently read by InputSystem
        for (int rep = 0; rep < InputSystem.devices.Count - 1; rep++)
        {
            InputSystem.RemoveDevice(InputSystem.devices[rep]);
        }

        if (InputSystem.devices[0] == null) return;

        //Checks the first slot of the InputSystem devices list for controller type
        if (InputSystem.devices[0].description.manufacturer == "Sony Interactive Entertainment")
        {
            //Sets UI scheme to PS
            Debug.Log("Playstation Controller Detected");

        }
        else
        {
            //Sets UI scheme to XB
            Debug.Log("Xbox Controller Detected");

        }
    }
    #endregion

}
