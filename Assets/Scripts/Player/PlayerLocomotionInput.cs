using UnityEngine;
using UnityEngine.InputSystem;

namespace FinalCharacterController
{
    [DefaultExecutionOrder(-2)]
    public class PlayerLocomotionInput : MonoBehaviour, PlayerControls.IPlayerLocomotionMapActions
    {
        public PlayerControls PlayerControls { get; private set; }
        public Vector2 movementInput { get; private set; }
        public bool tackleInput { get; private set; }

        private void OnEnable()
        {
            PlayerControls = new PlayerControls();
            PlayerControls.Enable();

            PlayerControls.PlayerLocomotionMap.Enable();
            PlayerControls.PlayerLocomotionMap.SetCallbacks(this);
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            movementInput = context.ReadValue<Vector2>();
        }

        public void OnTackle(InputAction.CallbackContext context)
        {
            tackleInput = context.ReadValueAsButton();
        }
    }
}
