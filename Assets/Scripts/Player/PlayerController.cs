using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace FinalCharacterController
{
    [DefaultExecutionOrder(-1)]
    public class PlayerController : NetworkBehaviour
    {
        #region Components
        [Header("Components")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Transform playerBody;
        [SerializeField] private CinemachineCamera cinemachineCameraPan;
        [SerializeField] private CinemachineCamera cinemachineCameraAim;
        #endregion

        #region Movement Settings
        [Header("Movement Settings")]
        public float runSpeed = 4;
        #endregion

        #region Gravity
        [Header("Gravity Settings")]
        public float gravity = -9.81f;
        private Vector3 gravityVector = Vector3.zero;
        #endregion

        #region States
        private IPlayerStates currentState;
        Dictionary<StateEnum, IPlayerStates> states;
        private IdleState idleState;
        private RunState runState;
        #endregion

        private PlayerLocomotionInput playerLocomotionInput;

        
        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
                return;

            playerCamera = Instantiate(playerCamera);
            cinemachineCameraPan = Instantiate(cinemachineCameraPan);

            cinemachineCameraPan.Follow = transform;
        }

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

            Quaternion lookRotation = Quaternion.LookRotation(movementDirection);
            playerBody.rotation = lookRotation;

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
                gravityVector.y = gravity;
                vector3.y = gravityVector.y;
            }
            else
            {
                gravityVector.y += gravity * Time.deltaTime;
                vector3.y = gravityVector.y;
            }

            return vector3;
        }
    }
}
