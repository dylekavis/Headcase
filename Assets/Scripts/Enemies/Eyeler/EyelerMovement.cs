using UnityEngine;

[RequireComponent(typeof(EyelerAIController))]
public class EyelerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 15f;

    Transform targetToChase;

    Rigidbody2D rb;
    EyelerAIController eyelerAI;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        eyelerAI = GetComponent<EyelerAIController>();
    }

    void OnEnable()
    {
        eyelerAI.OnChaseStart += HandleChase;
        eyelerAI.OnChaseEnd += CancelChase;
    }

    void OnDisable()
    {
        eyelerAI.OnChaseStart -= HandleChase;
        eyelerAI.OnChaseEnd -= CancelChase;
    }

    void Update()
    {
        if (targetToChase != null)
        {
            float distance = Vector2.Distance(transform.position, targetToChase.position);

            if (distance > eyelerAI.GetAttackDistance())
            {
                Vector2 moveDir = Vector2.MoveTowards(rb.position, targetToChase.position, moveSpeed * Time.fixedDeltaTime);

                rb.MovePosition(moveDir);
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
