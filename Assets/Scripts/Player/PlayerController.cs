using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace FinalCharacterController
{
    [DefaultExecutionOrder(-1)]
    public class PlayerController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Transform playerBody;

        [Header("Movement Settings")]
        public float runSpeed = 4;

        [Header("Gravity Settings")]
        public float gravity = -9.81f;

        private PlayerLocomotionInput playerLocomotionInput;

        private IPlayerStates currentState;
        #region States
        Dictionary<StateEnum, IPlayerStates> states;
        private IdleState idleState;
        private RunState runState;
        #endregion

        private void Awake()
        {
            playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            idleState = new IdleState(this);
            runState = new RunState(this);

            states = new Dictionary<StateEnum, IPlayerStates>()
            {
                {StateEnum.Idle, idleState },
                {StateEnum.Run, runState}
            };

            currentState = idleState;
        }

        private void Update()
        {
            currentState.UpdateState(ref playerLocomotionInput);
        }

        public void ChangeState(StateEnum newState)
        {
            currentState = states[newState];
        }

        public void move()
        {
            Vector3 cameraForwardXZ = new Vector3(playerCamera.transform.forward.x, 0f, playerCamera.transform.forward.z).normalized;
            Vector3 cameraRightXZ = new Vector3(playerCamera.transform.right.x, 0f, playerCamera.transform.right.z).normalized;
            Vector3 movementDirection = (cameraRightXZ * playerLocomotionInput.movementInput.x + cameraForwardXZ * playerLocomotionInput.movementInput.y).normalized;

            Vector3 inputDir = new Vector3(playerLocomotionInput.movementInput.x, 0f, playerLocomotionInput.movementInput.y);
            if (inputDir.sqrMagnitude > 0.001f)
            {
                Vector3 lookDirection = cameraRightXZ * inputDir.x + cameraForwardXZ * inputDir.z;
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                playerBody.rotation = targetRotation;
            }

            Vector3 move = movementDirection * runSpeed;

            move = applyGravity(move);

            characterController.Move(move * Time.deltaTime);
        }

        public void idle()
        {
            Vector3 vector = Vector3.zero;
            vector = applyGravity(vector);
            characterController.Move(vector * Time.deltaTime);
        }

        public Vector3 applyGravity(Vector3 vector3)
        {
            if (characterController.isGrounded)
            {
                vector3.y = -2f;
            }
            else
            {
                vector3.y = gravity;
            }

            return vector3;
        }
    }
}
