using UnityEngine;

public enum PlayerMovementState
{
    Idle,
    Walking,
    Sprinting
}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] PlayerMovementState state;
    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] float moveSpeed;
    [SerializeField] float sprintSpeedModifier;

    Vector2 moveInput;
    Vector2 forward;

    void FixedUpdate()
    {
        UpdateMovementState();
        AppplyMovement();
    }

    void OnEnable()
    {
        PlayerInputManager.Instance.OnMove += HandleMovement;
        PlayerInputManager.Instance.OnMoveCancelled += CancelMovement;

        PlayerInputManager.Instance.OnSprint += HandleSprint;
        PlayerInputManager.Instance.OnSprintCancelled += CancelSprint;
    }

    void OnDisable()
    {
        PlayerInputManager.Instance.OnMove -= HandleMovement;
        PlayerInputManager.Instance.OnMoveCancelled -= CancelMovement;

        PlayerInputManager.Instance.OnSprint -= HandleSprint;
        PlayerInputManager.Instance.OnSprintCancelled -= CancelSprint;
    }

    void UpdateMovementState()
    {
        if(moveInput == Vector2.zero)
        {
            forward = moveInput.normalized;
            state = PlayerMovementState.Idle;
            return;
        }

        if (state == PlayerMovementState.Sprinting) return;
        state = PlayerMovementState.Walking;
    }

    void AppplyMovement()
    {
        switch(state)
        {
            case PlayerMovementState.Idle:
                rigidBody.linearVelocity = Vector2.zero;
                break;
            case PlayerMovementState.Walking:
                rigidBody.linearVelocity = moveInput * moveSpeed;
                break;
            case PlayerMovementState.Sprinting:
                rigidBody.linearVelocity = moveInput * moveSpeed * sprintSpeedModifier;
                break;
        }
    }

    void HandleMovement(Vector2 input)
    {
        moveInput = input;
    }

    void CancelMovement()
    {
        moveInput = Vector2.zero;
    }

    void HandleSprint()
    {
        if (moveInput == Vector2.zero) return;
        state = PlayerMovementState.Sprinting;
    }

    void CancelSprint()
    {
        if (moveInput == Vector2.zero)
            state = PlayerMovementState.Idle;
        else
            state = PlayerMovementState.Walking;
    }

    public Vector2 GetMoveInput => moveInput;

}
