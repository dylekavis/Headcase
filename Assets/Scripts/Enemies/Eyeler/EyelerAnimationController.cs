using UnityEngine;

public enum AnimationState
{
    Idle,
    Moving,
    Attacking
}

[RequireComponent(typeof(EyelerMovement), typeof(EyelerAttack))]
public class EyelerAnimationController : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] AnimationState state;

    [SerializeField] EyelerMovement eyelerMovement;
    [SerializeField] EyelerAttack eyelerAttack;

    void OnEnable()
    {
        eyelerMovement.OnMove += HandleMove;
        eyelerMovement.OnMoveCancelled += CancelMove;
        eyelerMovement.OnIdle += HandleIdle;

        eyelerAttack.OnAttack += HandleAttack;
        eyelerAttack.OnAttackCancelled += CancelAttack;
    }

    void OnDisable()
    {
        eyelerMovement.OnMove -= HandleMove;
        eyelerMovement.OnMoveCancelled -= CancelMove;
        eyelerMovement.OnIdle -= HandleIdle;

        eyelerAttack.OnAttack -= HandleAttack;
        eyelerAttack.OnAttackCancelled -= CancelAttack;
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
