using FinalCharacterController;
using UnityEngine;

public enum StateEnum
{
    Idle,
    Run,
    Tackle,
    ADS
}
public interface IPlayerStates
{
    void OnEnter();
    void OnExit();
    void UpdateState(ref PlayerLocomotionInput input);
}
