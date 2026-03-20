using Unity.VisualScripting;
using UnityEngine;

public class BombSpiderMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] ThrowableEnemy throwable;

    Transform targetToChase;

    Rigidbody2D rb;
    BombSpiderController controller;

    float variableMoveSpeed;

    bool isThrown => throwable.GetState() == IThrowable.State.Thrown;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<BombSpiderController>();
        variableMoveSpeed = moveSpeed;
    }

    void OnEnable()
    {
        controller.OnChaseStart += HandleChase;
        controller.OnChaseEnd += CancelChase;
        throwable.OnThrown += CancelChase;
    }

    void OnDisable()
    {
        controller.OnChaseStart -= HandleChase;
        controller.OnChaseEnd -= CancelChase;
        throwable.OnThrown -= CancelChase;
    }

    void FixedUpdate()
    {
        if (isThrown) return;

        variableMoveSpeed = moveSpeed;

        if (targetToChase != null)
        {
            float distance = Vector2.Distance(transform.position, targetToChase.position);

            if (distance > controller.GetAttackDistance())
            {
                Vector2 moveDir = Vector2.MoveTowards(rb.position, targetToChase.position, moveSpeed * Time.fixedDeltaTime);

                rb.MovePosition(moveDir);
            }
            
            if (distance < controller.GetAttackDistance())
            {
                CancelChase();
            }
        }
    }

    void HandleChase(Transform target)
    {
        targetToChase = target;
    }

    void CancelChase()
    {
        targetToChase = null;
        variableMoveSpeed = 0;
    }
}