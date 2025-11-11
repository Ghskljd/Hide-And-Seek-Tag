using FinalCharacterController;
using Unity.VisualScripting;
using UnityEngine;

public class IdleState : IPlayerStates
{
    private PlayerController playerController;

    public IdleState(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void OnEnter()
    {
        //TODO
    }

    public void OnExit()
    {
        //TODO
    }

    public void UpdateState(ref PlayerLocomotionInput input)
    {
        if (input.movementInput.x != 0 || input.movementInput.y != 0)
        {
            playerController.ChangeState(StateEnum.Run);
            return;
        }

        if (input.tackleInput)
        {
            playerController.ChangeState(StateEnum.Tackle);
            return;
        }
        
        playerController.idle();
    }
}
