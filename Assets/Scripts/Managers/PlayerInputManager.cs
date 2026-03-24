using System;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance;

    public event Action<Vector2> OnMove;
    public event Action OnMoveCancelled;
    public event Action OnSprint;
    public event Action OnSprintCancelled;
    public event Action<Vector2> OnThrow;
    public event Action<Vector2> OnThrowCancelled;
    public event Action<Vector2> OnPlayerLook;
    public event Action OnLookCancelled;
    public event Action OnJumpStarted;
    public event Action OnJumpCancelled;
    public event Action OnRetrieveStart;
    public event Action OnRetrieveEnd;

    [SerializeField] Camera mainCam;

    Vector2 moveInput;
    [SerializeField] Vector2 aimDirection;

    bool isMoving;

    void Awake()
    {
        UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;

        if (Instance != this && Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void RegisterCamera(Camera camera)
    {
        mainCam = camera;
    }

    public void HandleMovement(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            isMoving = true;
            moveInput = ctx.ReadValue<Vector2>();
            OnMove?.Invoke(moveInput);
        }
        else if (ctx.canceled)
        {
            isMoving = false;
            moveInput = Vector2.zero;
            OnMoveCancelled?.Invoke();
        }
    }

    public void HandleSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            OnSprint?.Invoke();   
        }
        else if (ctx.canceled)
            OnSprintCancelled?.Invoke();
    }

    public void HandleThrow(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            OnThrow?.Invoke(aimDirection);
        else if (ctx.canceled)
            OnThrowCancelled?.Invoke(aimDirection);
    }

    public void HandleLookDirection(InputAction.CallbackContext ctx)
    {
        if (isMoving) return;

        if (ctx.canceled) 
        {
            OnLookCancelled?.Invoke();
            return;
        }

        if (!ctx.performed) return;

        Vector2 lookDirection = ctx.ReadValue<Vector2>();

        if (ctx.control.device is Gamepad)
        {
            if (lookDirection.magnitude > 0.2f)
            {
                OnPlayerLook?.Invoke(lookDirection);
            }
        }
        else
        {
            Vector3 mouseScreenPos = lookDirection;
            Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(mouseScreenPos);
            Vector2 direction = (mouseWorldPos - transform.position).normalized;

            aimDirection = direction.normalized;
            OnPlayerLook?.Invoke(aimDirection);
        }
    }

    public void HandleJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            OnJumpStarted?.Invoke();
        }
        
        if(ctx.canceled)
        {
            OnJumpCancelled?.Invoke();
        }
    }

    public void HandleRetrieve(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
            OnRetrieveStart?.Invoke();
        
        if(ctx.canceled)
            OnRetrieveEnd?.Invoke();
    }
}
