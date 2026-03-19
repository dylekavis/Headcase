using UnityEngine;

public class BombSpiderMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 15f;

    Transform targetToChase;

    Rigidbody2D rb;
    BombSpiderController controller;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<BombSpiderController>();
    }

    void OnEnable()
    {
        controller.OnChaseStart += HandleChase;
        controller.OnChaseEnd += CancelChase;
    }

    void OnDisable()
    {
        controller.OnChaseStart -= HandleChase;
        controller.OnChaseEnd -= CancelChase;
    }

    void Update()
    {
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
    }
}