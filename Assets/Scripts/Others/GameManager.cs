using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField] private Image cursor;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private FirstPersonController player;
    [SerializeField] private GameObject equipmentHolder;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        
    }

    public void SwitchControl(ControlMode mode)
    {
        currentControlMode = mode;
        switch (currentControlMode)
        {
            case ControlMode.PlayerControl: PlayerControl(); break;
            case ControlMode.UIControl: UIControl(); break;
        }
    }

    public void UIControl()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        DisableMovement();
        cursor.gameObject.SetActive(false);
        InteractionSystem.Instance.enabled = false;
        InteractionSystem.Instance.ForceScanningCloseUI();
        equipmentHolder.SetActive(false);
        gameUI.SetActive(false);
    }

    public void PlayerControl()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        EnableMovement();
        cursor.gameObject.SetActive(true);
        InteractionSystem.Instance.ForceScanningCloseUI();
        InteractionSystem.Instance.enabled = true;
        equipmentHolder.SetActive(true);
        gameUI.SetActive(true);
        StarterAssetsInputs.SwitchActionMap("Player");
    }

    public void EnableMovement()
    {
        player.EnableMovement(true);
    }

    public void DisableMovement()
    {
        player.EnableMovement(false);
    }

    public ControlMode GetCurrentControlMode() => currentControlMode;
}
