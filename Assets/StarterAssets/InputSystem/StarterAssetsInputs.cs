using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
        public static StarterAssetsInputs Instance { get; private set; }

        [Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;


        [Header("Action Input Values")]
        public bool left;
        public bool right;
        public bool up;
        public bool down;
        public bool interact;
        public bool Inventory;
        public bool use;

        [Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

        private void Awake()
        {
            Instance = this;
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

        public void OnLeftInput(InputValue value)
        {
            LeftInput(value.isPressed);
        }

        public void OnRightInput(InputValue value)
        {
            RightInput(value.isPressed);
        }

        public void OnUpInput(InputValue value)
        {
            UpInput(value.isPressed);
        }

        public void OnDownInput(InputValue value)
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

        private void OnApplicationFocus(bool hasFocus)
		{
            SetCursorState(cursorLocked && GameManager.Instance.GetCurrentControlMode() == GameManager.ControlMode.PlayerControl);
        }

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}