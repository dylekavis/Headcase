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
    public event Action OnAttackStart;
    public event Action OnAttackEnd;

    [Header("Eyeler State")]
    [SerializeField] EyelerState state;

    [Header("Detection Parameters")]
    [SerializeField] DetectionRadius detectionRadius;

    [Header("Attacking Parameters")]
    [SerializeField] float minDistanceToAttack = 1.25f;
    [SerializeField] float attackCooldownTime = 1.5f;

    bool canAttack = true;

    Vector2 targetToChase;
    Coroutine attackCooldownRoutine;

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

        float distance = Vector2.Distance(transform.position, targetToChase);

        if (distance <= minDistanceToAttack)
        {
            canAttack = false;
            state = EyelerState.Attacking;

            OnAttackStart?.Invoke();

            attackCooldownRoutine = StartCoroutine(AttackCooldown());
        }
    }

    void SetTargetToChase(GameObject player)
    {
        targetToChase = player.transform.position;

        state = EyelerState.Chasing;
        OnChaseStart?.Invoke(targetToChase);
    }

    void CancelChase()
    {
        state = EyelerState.Idle;
        OnChaseEnd?.Invoke();
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldownTime);

        canAttack = true;
        OnAttackEnd?.Invoke();
        state = EyelerState.Idle;
    }

    public EyelerState GetEyelerState => state;
}
