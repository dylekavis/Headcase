using UnityEngine;

public class BombSpiderAnimationController : MonoBehaviour
{
    [SerializeField] Animator anim;

    [SerializeField] BombSpiderController controller;

    void Start()
    {
        controller = GetComponent<BombSpiderController>();    
    }

    void OnEnable()
    {
        controller.OnChaseStart += HandleMove;
        controller.OnChaseEnd += CancelMove;

        controller.OnAttackStart += HandleAttack;
        controller.OnAttackEnd += CancelAttack;    
    }

    void OnDisable()
    {
        controller.OnChaseStart -= HandleMove;
        controller.OnChaseEnd -= CancelMove;

        controller.OnAttackStart -= HandleAttack;
        controller.OnAttackEnd -= CancelAttack;  
    }

    void HandleMove(Transform target)
    {
        anim.SetInteger("SwitchState", 1);
    }

    void CancelMove()
    {
        HandleIdle();
    }

    void HandleIdle()
    {
        anim.SetInteger("SwitchState", 0);
    }

    void HandleAttack()
    {
        anim.SetInteger("SwitchState", 2);
    }

    void CancelAttack()
    {
        HandleIdle();
    }
}
