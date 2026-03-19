using System;
using System.Collections;
using UnityEngine;

public enum BombSpiderMoveState
{
    Idle,
    Chasing,
    Attack    
}

public class BombSpiderController : MonoBehaviour
{
    public event Action<Transform> OnChaseStart;
    public event Action OnChaseEnd;
    
    public event Action OnAttackStart;
    public event Action OnAttackEnd;


    [Header("Attack Variables")]
    [SerializeField] float minDistanceToAttack = 1.5f;
    [SerializeField] float attackAttemptTime = 0.12f;
    [SerializeField] float attackAnimatonTime = 0.3f;
    [SerializeField] float attackCooldownTime = 2.5f;

    [Header("References")]
    [SerializeField] BombSpiderMoveState state;
    [SerializeField] DetectionRadius detectionRadius;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] ParticleSystem ps;

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

        state = BombSpiderMoveState.Chasing;

        OnChaseStart?.Invoke(targetToFollow.transform);

        float distance = Vector2.Distance(transform.position, targetToFollow.transform.position);

        if (distance <= minDistanceToAttack)
        {
            state = BombSpiderMoveState.Attack;
            OnAttackStart?.Invoke();

            StartCoroutine(AttackRoutine());
        }
    }

    void CancelDetectTarget()
    {
        state = BombSpiderMoveState.Idle;

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
            OnAttackEnd?.Invoke();
            yield break;
        }

        yield return new WaitForSeconds(attackAttemptTime);

        OnAttackStart?.Invoke();

        canAttack = false;

        yield return new WaitForSeconds(attackAnimatonTime);

        OnAttackEnd?.Invoke();

        ps.gameObject.SetActive(true);
        ps.Play();
        
        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }

    #endregion
}