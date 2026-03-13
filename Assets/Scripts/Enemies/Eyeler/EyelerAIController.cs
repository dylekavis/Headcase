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
    public event Action<Transform> OnChaseStart;
    public event Action OnChaseEnd;
    
    public event Action OnAttackAttemptStart;
    public event Action OnAttackAttemptEnd;
    public event Action OnAttackStart;
    public event Action OnAttackEnd;


    [Header("Attack Variables")]
    [SerializeField] float minDistanceToAttack = 1.5f;
    [SerializeField] float attackAttemptTime = 0.12f;
    [SerializeField] float attackAnimatonTime = 0.3f;
    [SerializeField] float attackCooldownTime = 2.5f;

    [Header("References")]
    [SerializeField] EyelerState state;
    [SerializeField] DetectionRadius detectionRadius;

    //Unsearialized references
    GameObject targetToFollow;
    bool canAttack = true;

    public float GetAttackDistance() => minDistanceToAttack;
    
    void OnEnable()
    {
        detectionRadius.OnPlayerDetected += HandleDetectTarget;
        detectionRadius.OnPlayerUndetected += CancelDetectTarget;
    }

    void OnDisable()
    {
        detectionRadius.OnPlayerDetected -= HandleDetectTarget;
        detectionRadius.OnPlayerUndetected -= CancelDetectTarget;
    }

#region Target Detection
    void HandleDetectTarget(GameObject target)
    {
        targetToFollow = target;

        state = EyelerState.Chasing;

        OnChaseStart?.Invoke(targetToFollow.transform);

        float distance = Vector2.Distance(transform.position, targetToFollow.transform.position);

        if (distance <= minDistanceToAttack)
        {
            state = EyelerState.Attacking;
            OnAttackAttemptStart?.Invoke();

            StartCoroutine(AttackRoutine());
        }
    }

    void CancelDetectTarget()
    {
        state = EyelerState.Idle;

        targetToFollow = null;

        OnChaseEnd?.Invoke();
    }

#endregion

#region Attacking

    IEnumerator AttackRoutine()
    {
        if (targetToFollow == null) yield break;

        float distance = Vector2.Distance(transform.position, targetToFollow.transform.position);

        if (distance > minDistanceToAttack)
        {
            OnAttackAttemptEnd?.Invoke();
            yield break;
        }

        yield return new WaitForSeconds(attackAttemptTime);

        OnAttackStart?.Invoke();

        canAttack = false;

        yield return new WaitForSeconds(attackAnimatonTime);

        OnAttackEnd?.Invoke();

        yield return new WaitForSeconds(attackCooldownTime);

        ResetAttack();
    }
    
    void ResetAttack()
    {
        canAttack = true;
        StopCoroutine(AttackRoutine());

        state = EyelerState.Idle;
    }

#endregion
}
