using FinalCharacterController;
using UnityEngine;

public class RunState : IPlayerStates
{
    private PlayerController playerController;

    public RunState(PlayerController playerController)
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
        if (input.movementInput.x == 0 && input.movementInput.y == 0)
        {
            playerController.ChangeState(StateEnum.Idle);
            return;
        }
        
        playerController.move();
    }
}
