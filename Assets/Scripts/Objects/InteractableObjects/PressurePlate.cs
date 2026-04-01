using System;
using UnityEngine;

public enum PlateState
{
    Idle,
    Active
}

public class PressurePlate : MonoBehaviour
{
    public event Action OnPlateActivate;
    public event Action OnPlateDeactivate;

    [SerializeField] Animator animator;
    [SerializeField] PlateState state;
    
    bool isActive;

    public bool GetActiveState() => isActive;

    [SerializeField] GameObject objOnPlate;

    public void SetState(PlateState state)
    {
        this.state = state;

        animator.SetInteger("SwitchState", (int)state);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DetectionRadius")) return;

        Debug.Log($"{collision.gameObject.name} stepped on this plate {name}");

        objOnPlate = collision.gameObject;

        if (state == PlateState.Idle)
        {
            SetState(PlateState.Active);
            OnPlateActivate?.Invoke();
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject != objOnPlate && objOnPlate != null) return;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DetectionRadius")) return;

        Debug.Log($"{collision.gameObject.name} stepped off this plate {name}");

        objOnPlate = null;

        if (state == PlateState.Active)
        {
            SetState(PlateState.Idle);
            OnPlateDeactivate?.Invoke();
        }
    }
}
