using System;
using System.Data;
using UnityEngine;

public class MoveState : IState
{
    private CharacterStateMachine StateMachine;
    private Transform transform;
    private float verticalVelocity = 0;
    public MoveState(CharacterStateMachine stateMachine, Transform transform)
    {
        StateMachine = stateMachine;
        this.transform = transform;
    }
    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public IState UpdatePlayer(float moveInput, float rotateInput, float rotationSpeed, float movementSpeed, float gravity, ref CharacterController controller)
    {
        if (Math.Abs(rotateInput) < 0.1f && Math.Abs(moveInput) < 0.1f)
        {
            return StateMachine.idleState;
        }
        
        //rotate
        float turn = rotateInput * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, turn, 0f);

        //move
        Vector3 move = transform.forward * moveInput * movementSpeed;

        //gravity
        if (controller.isGrounded)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity;
        }

        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);

        return StateMachine.moveState;
    }

}
