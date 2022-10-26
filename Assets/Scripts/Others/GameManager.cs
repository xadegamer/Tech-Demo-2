using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum ControlMode
    {
        PlayerControl,
        UIControl   
    }
    
    [SerializeField] private ControlMode currentControlMode = ControlMode.PlayerControl;
    [SerializeField] private MeshRenderer playerRenderer;
    [SerializeField] private FirstPersonController player;

    private void Awake()
    {
        Instance = this;
    }

    public void SwitchControl(ControlMode mode)
    {
        currentControlMode = mode;
        switch (currentControlMode)
        {
            case ControlMode.PlayerControl:
                PlayerControl();
                break;
            case ControlMode.UIControl:
                UIControl();
                break;
        }
    }

    public void UIControl()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        DisableMovement();
        InteractionUI.Instance.ToggleCursor(false);
    }

    public void PlayerControl()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        EnableMovement();
        InteractionUI.Instance.ToggleCursor(true);
    }

    public void EnableMovement()
    {
        player.EnableMovement(true);
    }

    public void DisableMovement()
    {
        player.EnableMovement(false);
    }

    public void TogglePlayerVisual(bool state)
    {
        playerRenderer.enabled = state;
    }

    public ControlMode GetCurrentControlMode() => currentControlMode;
}
