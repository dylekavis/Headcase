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

        eyelerAI.OnAttackAttempt += AttemptAttack;
        eyelerAI.OnAttackLand += HandleAttack;
        eyelerAI.OnAttackEnd += CancelAttack;    
    }

    void OnDisable()
    {
        eyelerAI.OnChaseStart -= HandleMove;
        eyelerAI.OnChaseEnd -= CancelMove;

        eyelerAI.OnAttackAttempt -= AttemptAttack;
        eyelerAI.OnAttackLand -= HandleAttack;
        eyelerAI.OnAttackEnd -= CancelAttack;  
    }

    void HandleMove(Vector2 moveDir)
    {
        if (state == AnimationState.Attacking) return;

        state = AnimationState.Moving;

        anim.SetBool("isWalking", true);

        anim.SetFloat("AnimMoveX", moveDir.x);
        anim.SetFloat("AnimMoveY", moveDir.y);
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
        anim.SetBool("canAttack", false);
        anim.SetBool("canAttemptAttack", false);
    }

    void AttemptAttack()
    {
        state = AnimationState.Attacking;
        
        anim.SetBool("isWalking", false);
        anim.SetBool("canAttemptAttack", true);
    }

    void HandleAttack()
    {
        anim.SetBool("canAttemptAttack", false);
        anim.SetBool("canAttack", true);
    }

    void CancelAttack()
    {
        HandleIdle();
    }
}
