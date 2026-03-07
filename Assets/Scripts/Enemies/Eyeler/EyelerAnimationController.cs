using UnityEngine;

public enum AnimationState
{
    Idle,
    Moving,
    Attacking
}

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

        eyelerAI.OnAttackStart += HandleAttack;
        eyelerAI.OnAttackEnd += CancelAttack;    
    }

    void OnDisable()
    {
        eyelerAI.OnChaseStart -= HandleMove;
        eyelerAI.OnChaseEnd -= CancelMove;

        eyelerAI.OnAttackStart -= HandleAttack;
        eyelerAI.OnAttackEnd -= CancelAttack;  
    }

    void HandleMove(Vector2 moveDir)
    {
        if (state == AnimationState.Attacking) return;

        Vector2 normalized = moveDir.normalized;

        state = AnimationState.Moving;

        anim.SetBool("isWalking", true);

        anim.SetFloat("AnimMoveX", normalized.x);
        anim.SetFloat("AnimMoveY", normalized.y);
    }

    void CancelMove()
    {
        if (state != AnimationState.Attacking)
            HandleIdle();
    }

    void HandleIdle()
    {
        state = AnimationState.Idle;

        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", false);
    }

    void HandleAttack()
    {
        state = AnimationState.Attacking;
        
        anim.SetBool("isWalking", false);
        anim.SetBool("isAttacking", true);
    }

    void CancelAttack()
    {
        HandleIdle();
    }
}
