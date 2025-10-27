using System.Numerics;
using Unity.Netcode;
using UnityEngine;

public class CharacterStateMachine : NetworkBehaviour
{
    #region Fields
    [SerializeField] public CharacterController controller;
    [Header("Movement Settings")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float moveSpeed;
    [Header("Gravity Settings")]
    [SerializeField] private float gravity;
    private float moveInput;
    private float rotateInput;
    #endregion

    #region States
    private IState currentState;
    public IdleState idleState;
    public MoveState moveState;
    #endregion
    void Awake()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();

        idleState = new IdleState(this);
        moveState = new MoveState(this, transform);
        currentState = idleState;
        moveInput = 0;
        rotateInput = 0;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState.OnEnter();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        moveInput = Input.GetAxis("Vertical");
        rotateInput = Input.GetAxis("Horizontal");

        IState newState = currentState.UpdatePlayer(moveInput, rotateInput, rotationSpeed, moveSpeed, gravity, ref controller);
        if (newState.GetType() != currentState.GetType())
        {
            currentState.OnExit();
            currentState = newState;
            currentState.OnEnter();
        }
    }
}
