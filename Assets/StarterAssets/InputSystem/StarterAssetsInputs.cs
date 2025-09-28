using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    public class StarterAssetsInputs : NetworkBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        public void OnMove(InputAction.CallbackContext context)
        {
            Debug.Log("Is Owner:" + IsOwner);
            if (!IsOwner) return;
            MoveInput(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;
            if (cursorInputForLook)
                LookInput(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;
            JumpInput(context.ReadValueAsButton());
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (!IsOwner) return;
            SprintInput(context.ReadValueAsButton());
        }

        public void MoveInput(Vector2 newMoveDirection) => move = newMoveDirection;
        public void LookInput(Vector2 newLookDirection) => look = newLookDirection;
        public void JumpInput(bool newJumpState) => jump = newJumpState;
        public void SprintInput(bool newSprintState) => sprint = newSprintState;

        private void OnApplicationFocus(bool hasFocus) =>
            SetCursorState(cursorLocked);

        private void SetCursorState(bool newState) =>
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
