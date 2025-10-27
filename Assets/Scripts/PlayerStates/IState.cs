using UnityEngine;

public interface IState
{
    void OnExit();
    void OnEnter();
    public IState UpdatePlayer(float moveInput, float rotateInput,
    float rotationSpeed, float movementSpeed,
    float gravity, ref CharacterController controller);
}
