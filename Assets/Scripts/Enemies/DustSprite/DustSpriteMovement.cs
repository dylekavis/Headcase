using System.Collections;
using UnityEngine;

public enum DustSpriteMoveState
{
    Idle,
    Chasing,
    Attack    
}

public class DustSpriteMovement : MonoBehaviour
{
    [SerializeField] DustSpriteMoveState state;

    [SerializeField] float moveSpeed;
    [SerializeField] float amplitude;
    [SerializeField] float frequency;

    [SerializeField] DetectionRadius detectionRadius;

    [SerializeField] float minDistanceToAttack;
    [SerializeField] float attackWindUpTime;
    [SerializeField] float attackSpeedModifier;

    float startY;
    bool isAttacking;
    Rigidbody2D rb;
    GameObject targetToChase;
    Coroutine attackRoutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        detectionRadius = GetComponentInChildren<DetectionRadius>();
    }

    void Update()
    {   
        if (state == DustSpriteMoveState.Idle)
        {
            float sin = Mathf.Sin(frequency * Time.time) * amplitude;
            transform.position += new Vector3(0, sin);
        }
        else if (state == DustSpriteMoveState.Chasing && targetToChase != null)
        {
            Vector2 moveDir = Vector2.MoveTowards(rb.position, targetToChase.transform.position, moveSpeed * Time.deltaTime);
            rb.MovePosition(moveDir);

            float distance = Vector2.Distance(rb.position, targetToChase.transform.position);
            if (distance < minDistanceToAttack)
            {
                state = DustSpriteMoveState.Attack;
                attackRoutine = StartCoroutine(AttackRoutine());
            }
        }
    }

    void OnEnable()
    {
        detectionRadius.OnPlayerDetected += AssignTarget;
        detectionRadius.OnPlayerUndetected += CancelTarget;
    }

    void AssignTarget(GameObject target)
    {
        state = DustSpriteMoveState.Chasing;
        targetToChase = target;
    }

    void CancelTarget()
    {
        targetToChase = null;
        
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }
    }

    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(attackWindUpTime);

        if (targetToChase == null) yield break;

        isAttacking = true;
        Vector2 originalPos = rb.position;

        while (isAttacking && targetToChase != null)
        {
            Vector2 attackDir = Vector2.MoveTowards(rb.position, targetToChase.transform.position, moveSpeed * attackSpeedModifier* Time.deltaTime);
            rb.MovePosition(attackDir);
            isAttacking = false;
            yield return null;
        }

        Vector2 moveToOrigin = Vector2.MoveTowards(rb.position, originalPos, moveSpeed * Time.deltaTime);
        rb.MovePosition(moveToOrigin);

        attackRoutine = null;
    }
}
