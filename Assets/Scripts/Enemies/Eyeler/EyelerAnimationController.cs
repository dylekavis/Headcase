using UnityEngine;

[RequireComponent(typeof(EyelerAIController))]
public class EyelerAnimationController : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] AnimationState state;

    [SerializeField] EyelerAIController eyelerAI;

    void Start()
    {
        eyelerAI = GetComponent<EyelerAIController>();    
    }

    void OnEnable()
    {
        eyelerAI.OnChaseStart += HandleMove;
        eyelerAI.OnChaseEnd += CancelMove;

        eyelerAI.OnAttackAttemptStart += AttemptAttack;
        eyelerAI.OnAttackAttemptEnd += CancelAttackAttempt;
        eyelerAI.OnAttackStart += HandleAttack;
        eyelerAI.OnAttackEnd += CancelAttack;    
    }

    void OnDisable()
    {
        eyelerAI.OnChaseStart -= HandleMove;
        eyelerAI.OnChaseEnd -= CancelMove;

        eyelerAI.OnAttackAttemptStart -= AttemptAttack;
        eyelerAI.OnAttackAttemptEnd -= CancelAttackAttempt;
        eyelerAI.OnAttackStart -= HandleAttack;
        eyelerAI.OnAttackEnd -= CancelAttack;  
    }

    void HandleMove(Transform target)
    {
        Vector2 direction = (target.transform.position - transform.position).normalized;

        anim.SetInteger("SwitchState", 1);

        anim.SetFloat("AnimMoveX", direction.x);
        anim.SetFloat("AnimMoveY", direction.y);
    }

    void CancelMove()
    {
        HandleIdle();
    }

    void HandleIdle()
    {
        anim.SetInteger("SwitchState", 0);
        anim.SetBool("isAttacking", false);
    }

    void AttemptAttack()
    {
        anim.SetInteger("SwitchState", 2);
    }

    void CancelAttackAttempt()
    {
        HandleIdle();
    }

    void HandleAttack()
    {
        anim.SetBool("isAttacking", true);
    }

    void CancelAttack()
    {
        HandleIdle();
    }
}
