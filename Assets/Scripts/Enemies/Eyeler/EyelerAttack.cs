using System;
using System.Collections;
using UnityEngine;

public class EyelerAttack : MonoBehaviour
{
    public event Action OnAttack;
    public event Action OnAttackCancelled;

    [SerializeField] float attackDistance = 1.5f;
    [SerializeField] float attackCooldownTime = 3f;

    [SerializeField] EyelerMovement movement;
    [SerializeField] DetectionRadius detectionRadius;
    [SerializeField] Collider2D attackCollider;

    Vector2 lastKnownPlayerPos;
    Coroutine attackCooldown;

    bool canAttack = true;
    bool isAttacking;

    void OnEnable()
    {
        detectionRadius.OnPlayerDetected += SetTarget;
    }

    void OnDisable()
    {
        detectionRadius.OnPlayerDetected -= SetTarget;
    }

    void SetTarget(GameObject player)
    {
        lastKnownPlayerPos = player.transform.position;
    }

    void Update()
    {
        if (!canAttack || isAttacking) return;

        if (movement.IsMoving) return;

        float distance = Vector2.Distance(transform.position, lastKnownPlayerPos);

        if (distance <= attackDistance && !isAttacking)
        {
            Attack();
        }
    }

    void Attack()
    {
        canAttack = false;
        attackCollider.enabled = true;
        isAttacking = true;

        OnAttack?.Invoke();

        attackCooldown = StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldownTime);

        OnAttackCancelled?.Invoke();
        attackCollider.enabled = false;
        isAttacking = false;

        yield return new WaitForSeconds(0.2f);
        canAttack = true;
    }
}
