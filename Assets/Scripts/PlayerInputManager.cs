using System;
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
    public event Action OnThrow;
    public event Action OnThrowCancelled;

    Vector2 moveInput;

    void Awake()
    {
        UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;

        if (Instance != this && Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void HandleMovement(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            moveInput = ctx.ReadValue<Vector2>();
            OnMove?.Invoke(moveInput);
        }
        else if (ctx.canceled)
        {
            moveInput = Vector2.zero;
            OnMoveCancelled?.Invoke();
        }
    }

    public void HandleDash(InputAction.CallbackContext ctx)
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
            OnThrow?.Invoke();
        else if (ctx.canceled)
            OnThrowCancelled?.Invoke();
    }
}
