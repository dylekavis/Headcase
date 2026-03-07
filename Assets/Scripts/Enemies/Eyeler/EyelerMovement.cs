using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.UI;

[RequireComponent(typeof(EyelerAIController))]

public class EyelerMovement : MonoBehaviour
{
    [SerializeField] EyelerAIController eyelerAI;


    [Header("Movement Parameters")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float minAttackDistance = 1.5f;

    [Header("Idle Parameters")]
    [SerializeField] float yOscillationSpeed = 2f;
    [SerializeField, Range(0, 1)] float yOscillationHeight;

    [Header("Attack Parameters")]
    [SerializeField] float preAttackTime = 0.4f;
    [SerializeField] float xOscillationSpeed = 10f;
    [SerializeField, Range(0, 1)] float xOscillationWidth;

    float startY;
    float startX;
    float idleTimer;

    bool hasTarget;

    Vector2 targetVector;

    Rigidbody2D rb;

    Coroutine attackMovementRoutine;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        eyelerAI = GetComponent<EyelerAIController>();

        startY = transform.position.y;
        startX = transform.position.x;
    }

    void OnEnable()
    {
        eyelerAI.OnChaseStart += SetTargetToChase;
        eyelerAI.OnChaseEnd += StopUpdatingTarget;
        eyelerAI.OnAttackStart += StartAttackMovement;
    }

    void OnDisable()
    {
        eyelerAI.OnChaseStart -= SetTargetToChase;
        eyelerAI.OnChaseEnd -= StopUpdatingTarget;
        eyelerAI.OnAttackStart -= StartAttackMovement;
    }

    void Update()
    {
        if (eyelerAI.GetEyelerState == EyelerState.Attacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (eyelerAI.GetEyelerState == EyelerState.Idle)
        {
            IdleMovement();
            return;
        }
        
        if (hasTarget)
        {
            ChaseTargetPosition();
        }
    }

    void ChaseTargetPosition()
    {
        if (eyelerAI.GetEyelerState != EyelerState.Chasing) return;
        if (eyelerAI.GetEyelerState == EyelerState.Attacking) return;
        
        float distance = Vector2.Distance(transform.position, targetVector);

        if (distance > minAttackDistance)
        {
            Vector2 movePos = Vector2.MoveTowards(
                rb.position,
                targetVector,
                moveSpeed * Time.fixedDeltaTime
            );

            rb.MovePosition(movePos);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            hasTarget = false;
            return;
        }
    }

    void SetTargetToChase(Vector2 targetToChase)
    {
        Debug.Log("target set");
        targetVector = targetToChase;
        hasTarget = true;
    }

    void StopUpdatingTarget()
    {
        hasTarget = false;
        ReturnToIdle();
    }

    void IdleMovement()
    {
        idleTimer += Time.deltaTime;

        float sin = Mathf.Sin(yOscillationSpeed * idleTimer) * yOscillationHeight;

        transform.position = new Vector2(transform.position.x, startY + sin);
    }

    void ReturnToIdle()
    {
        idleTimer = 0;
        startY = transform.position.y;
        startX = transform.position.x;
    }

    void StartAttackMovement()
    {
        hasTarget = false;
    }
}
