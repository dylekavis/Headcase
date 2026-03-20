using System.Collections;
using System.Linq;
using UnityEngine;

public enum PlayerMovementState
{
    Idle,
    Walking,
    Sprinting
}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip[] stepFX;

    [SerializeField] PlayerMovementState state;
    [SerializeField] Rigidbody2D rigidBody;
    [SerializeField] float moveSpeed;
    [SerializeField] float sprintSpeedModifier;

    PlayerPitDetection pitDetection;

    Vector2 moveInput;
    Vector2 forward;

    bool isMoving;
    bool isSprinting;

    void Awake()
    {
        pitDetection = GetComponentInChildren<PlayerPitDetection>();
    }

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

        pitDetection.OnPitDetected += HandlePit;
    }

    void OnDisable()
    {
        PlayerInputManager.Instance.OnMove -= HandleMovement;
        PlayerInputManager.Instance.OnMoveCancelled -= CancelMovement;

        PlayerInputManager.Instance.OnSprint -= HandleSprint;
        PlayerInputManager.Instance.OnSprintCancelled -= CancelSprint;

        pitDetection.OnPitDetected -= HandlePit;
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
        isMoving = true;
        StartCoroutine(StepSFXGenerator());
    }

    void CancelMovement()
    {
        moveInput = Vector2.zero;
        isMoving = false;
    }

    void HandleSprint()
    {
        if (moveInput == Vector2.zero) return;
        state = PlayerMovementState.Sprinting;

        isSprinting = true;
    }

    void CancelSprint()
    {
        if (moveInput == Vector2.zero)
            state = PlayerMovementState.Idle;
        else
            state = PlayerMovementState.Walking;

        isSprinting = false;
    }

    void HandlePit()
    {
        transform.position += (Vector3)moveInput.normalized;

        StartCoroutine(PitMovementDelay());
    }

    IEnumerator PitMovementDelay()
    {
        yield return new WaitForSeconds(0.4f);

        CancelMovement();
    }

    IEnumerator StepSFXGenerator()
    {
        while (isMoving)
        {
            if (source.isPlaying) yield break;
            
            int randomStep = Random.Range(0, stepFX.Length);
            float randomPitch = Random.Range(0.9f, 1.1f);

            for (int i = 0; i < stepFX.Length; i++)
            {
                if (randomStep == i)
                {
                    source.pitch = randomPitch;
                    source.PlayOneShot(stepFX[i]);
                    yield return new WaitUntil(() => !source.isPlaying);
                }
            }

            if (isMoving && !isSprinting)
                yield return new WaitForSeconds(0.1f);
            else   
                yield return new WaitForSeconds(0.05f);
        }
    }

    public Vector2 GetMoveInput => moveInput;

}
