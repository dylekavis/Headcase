using System;
using System.Collections;
using UnityEngine;

public enum EyelerState
{
    Idle,
    Chasing,
    Attacking
}

public class EyelerAIController : MonoBehaviour
{
    public event Action<Vector2> OnChaseStart;
    public event Action OnChaseEnd;
    public event Action OnAttackAttempt;
    public event Action OnAttackLand;
    public event Action OnAttackEnd;

    [Header("Eyeler State")]
    [SerializeField] EyelerState state;

    [Header("Detection Parameters")]
    [SerializeField] DetectionRadius detectionRadius;
    [SerializeField] float detectionResetTime = 2f;

    [Header("Attacking Parameters")]
    [SerializeField] float minDistanceToAttemptAttack = 1.25f;
    [SerializeField] float attackAttemptTime = 0.12f;
    [SerializeField] float attackAnimationTime = 0.3f;
    [SerializeField] float attackCooldownTime = 2f;

    bool canAttack = true;

    Transform target;
    Coroutine attackAttempt;

    void OnEnable()
    {
        detectionRadius.OnPlayerDetected += SetTargetToChase;
        detectionRadius.OnPlayerUndetected += CancelChase;
    }

    void OnDisable()
    {
        detectionRadius.OnPlayerDetected -= SetTargetToChase;
        detectionRadius.OnPlayerUndetected -= CancelChase;
    }

    void Update()
    {
        if (state != EyelerState.Chasing && state != EyelerState.Attacking)
            return;

        if (target == null) return;
        
        float distance = Vector2.Distance(transform.position, target.position);

        if (distance <= minDistanceToAttemptAttack && canAttack && attackAttempt == null)
        {
            state = EyelerState.Attacking;

            attackAttempt = StartCoroutine(AttackRoutine());

            OnAttackAttempt?.Invoke();
        }
    }

    void SetTargetToChase(GameObject player)
    {
        target = player.transform;

        state = EyelerState.Chasing;

        Vector2 dir = (target.position - transform.position).normalized;

        OnChaseStart?.Invoke(dir);
    }

    void CancelChase()
    {
        target = null;

        state = EyelerState.Idle;
    
        if (attackAttempt != null)
        {
            StopCoroutine(attackAttempt);
            ResetAttack();
        }

        OnChaseEnd?.Invoke();
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(attackAttemptTime);

        if (target == null)
        {
            ResetAttack();
            yield break;
        }

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance > minDistanceToAttemptAttack)
        {
            ResetAttack();
            yield break;
        }

        OnAttackLand?.Invoke();

        yield return new WaitForSeconds(attackAnimationTime);

        canAttack = false;

        yield return new WaitForSeconds(attackCooldownTime);

        ResetAttack();
    }

    void ResetAttack()
    {
        attackAttempt = null;
        canAttack = true;

        state = target != null ? EyelerState.Chasing : EyelerState.Idle;

        OnAttackEnd?.Invoke();
    }

    public EyelerState GetEyelerState => state;
    public float MinAttackDistance => minDistanceToAttemptAttack;
    public Transform Target => target;
}
