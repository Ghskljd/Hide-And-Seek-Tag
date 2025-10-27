using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class IdleState : IState
{
    private CharacterStateMachine StateMachine;
    public IdleState(CharacterStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }
    public void OnEnter()
    {
        Debug.Log("Idle State: On Enter");
    }

    public void OnExit()
    {
        Debug.Log("Idle State: On Exit");
    }

    public IState UpdatePlayer(float moveInput, float rotateInput,
    float rotationSpeed, float movementSpeed,
    float gravity, ref CharacterController controller)
    {
        if(Math.Abs(moveInput) > 0.1f || Math.Abs(rotateInput) > 0.1f)
        {
            return StateMachine.moveState;
        }
        return this;
    }
}
