using System;
using UnityEngine;


public class EyelerMovement : MonoBehaviour
{
    public event Action<Vector2> OnMove;
    public event Action OnMoveCancelled;
    public event Action OnAttackStart;

    public event Action OnIdle;

    [Header("Movement Parameters")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float attackDistance = 1.5f;
    [SerializeField] float oscillationSpeed = 5f;
    [SerializeField, Range(0, 1)] float oscillationHeight;

    [Header("References")]
    [SerializeField] EyelerAttack eyelerAttack;
    [SerializeField] DetectionRadius detectionRadius;
    [SerializeField] Rigidbody2D rb;

    Vector2 lastKnownPlayerPos;

    float idleStartY;
    float idleTimer;

    bool hasTarget;
    bool isIdle;
    bool isMoving;
    bool isAttacking;

    void Start()
    {
        idleStartY = transform.position.y;
    }

    void OnEnable()
    {
        detectionRadius.OnPlayerDetected += SetTarget;
        detectionRadius.OnPlayerUndetected += StopUpdatingTarget;

        eyelerAttack.OnAttackCancelled += ReturnToIdle;
    }

    void OnDisable()
    {
        detectionRadius.OnPlayerDetected -= SetTarget;
        detectionRadius.OnPlayerUndetected -= StopUpdatingTarget;

        eyelerAttack.OnAttackCancelled -= ReturnToIdle;
    }

    void Update()
    {
        if (isIdle)
        {
            IdleMovement();
            return;
        }

        if (hasTarget)
        {
            MoveToLastKnownPosition();
        }
    }

    void SetTarget(GameObject player)
    {
        lastKnownPlayerPos = player.transform.position;
        hasTarget = true;
    }

    void StopUpdatingTarget()
    {
        hasTarget = true;
    }

    void IdleMovement()
    {
        idleTimer += Time.deltaTime;

        float sin = Mathf.Sin(oscillationSpeed * idleTimer) * oscillationHeight;

        transform.position = new Vector2(transform.position.x, idleStartY + sin);
    }

    void MoveToLastKnownPosition()
    {
        isIdle = false;

        if (isAttacking) return;

        Vector2 moveDir = (lastKnownPlayerPos - (Vector2)transform.position).normalized;

        float distance = Vector2.Distance(transform.position, lastKnownPlayerPos);

        if (distance > attackDistance)
        {
            isMoving = true;

            OnMove?.Invoke(moveDir);
            
            Vector2 newPos = Vector2.MoveTowards(
                rb.position,
                lastKnownPlayerPos,
                moveSpeed * Time.deltaTime
            );

            rb.MovePosition(newPos);
        }
        else
        {
            isMoving = false;
            isAttacking = true;
            OnAttackStart?.Invoke();
        }
    }

    void ReturnToIdle()
    {
        isIdle = true;
        isAttacking = false;
        idleTimer = 0;
        idleStartY = transform.position.y;
    }

    public bool IsMoving => isMoving;
}
