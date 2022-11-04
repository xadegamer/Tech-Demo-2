using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
        public static StarterAssetsInputs Instance { get; private set; }

        private PlayerInput _playerInput;

        [Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
        public bool interact;
        public bool Inventory;
        public bool jornal;

        [Header("Inventory Input Values")]
        public bool use;
        public bool equip;
        public bool TurnRight;
        public bool TurnLeft;
        public bool Drop;

        [Header("Keypad Puzzle Input Values")]
        public bool left;
        public bool right;
        public bool up;
        public bool down;
        public bool enter;
        public bool exit;


        [Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        public static void SwitchActionMap(string newActionMap)
        {
          Instance. _playerInput.SwitchCurrentActionMap(newActionMap);
            Debug.Log("Switched to " + Instance._playerInput.currentActionMap);
        }

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

        public void OnLeft(InputValue value)
        {
            LeftInput(value.isPressed);
        }

        public void OnRight(InputValue value)
        {
            RightInput(value.isPressed);
        }

        public void OnUp(InputValue value)
        {
            UpInput(value.isPressed);
        }

        public void OnDown(InputValue value)
        {
            DownInput(value.isPressed);
        }

        public void OnInteract(InputValue value)
        {
            InteractInput(value.isPressed);
        }

        public void OnInventory(InputValue value)
        {
            InventoryInput(value.isPressed);
        }

        public void OnUse(InputValue value)
        {
            UseInput(value.isPressed);
        }

        public void OnEquip(InputValue value)
        {
            EquipInput(value.isPressed);
        }

        public void OnEnter(InputValue value)
        {
            OnEnterInput(value.isPressed);
        }

        public void OnExit(InputValue value)
        {
            OnExitInput(value.isPressed);
        }

        public void OnTurnRight(InputValue value)
        {
            TurnRightInput(value.isPressed);
        }

        public void OnTurnLeft(InputValue value)
        {
            TurnLeftInput(value.isPressed);
        }

        public void OnDrop(InputValue value)
        {
            DropInput(value.isPressed);
        }

        public void OnJornal(InputValue value)
        {
            JornalInput(value.isPressed);
        }

#endif


        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

        public void LeftInput(bool newSprintState)
        {
            left = newSprintState;
        }

        public void RightInput(bool newSprintState)
        {
            right = newSprintState;
        }

        public void UpInput(bool newSprintState)
        {
            up = newSprintState;
        }

        public void DownInput(bool newSprintState)
        {
            down = newSprintState;
        }

        public void InteractInput(bool newSprintState)
        {
            interact = newSprintState;
        }

        public void InventoryInput(bool newSprintState)
        {
            Inventory = newSprintState;
        }

        public void UseInput(bool newSprintState)
        {
            use = newSprintState;
        }

        public void EquipInput(bool newSprintState)
        {
            equip = newSprintState;
        }

        public void OnEnterInput(bool newSprintState)
        {
            enter = newSprintState;
        }

        public void OnExitInput(bool newSprintState)
        {
            exit = newSprintState;
        }

        private void TurnRightInput(bool isPressed)
        {
            TurnRight = isPressed;
        }

        private void TurnLeftInput(bool isPressed)
        {
            TurnLeft = isPressed;
        }

        private void DropInput(bool isPressed)
        {
            Drop = isPressed;
        }

        private void JornalInput(bool isPressed)
        {
            jornal = isPressed;
        }

        private void OnApplicationFocus(bool hasFocus)
		{
            SetCursorState(cursorLocked && GameManager.Instance?.GetCurrentControlMode() == GameManager.ControlMode.PlayerControl);
        }

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}