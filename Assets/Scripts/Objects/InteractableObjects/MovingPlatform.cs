using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    [Header("Track Points")]
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    Transform currentTarget;

    Rigidbody2D rb;

    public bool isActive;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        currentTarget = endPoint;

        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
    }

    void OnEnable()
    {
        SwitchManager.Instance.OnAllActivate += HandleActive;
        SwitchManager.Instance.OnAllDeactivate += CancelActive;
    }

    void OnDisable()
    {
        SwitchManager.Instance.OnAllActivate -= HandleActive;
        SwitchManager.Instance.OnAllDeactivate -= CancelActive;
    }

    void FixedUpdate()
    {
        if (!isActive)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 moveTo = Vector2.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);
        rb.MovePosition(moveTo);

        if (Vector2.Distance(transform.position, currentTarget.position) < 0.05f)
        {
            currentTarget = currentTarget == endPoint ? startPoint : endPoint;
        }
    }

    void HandleActive()
    {
        isActive = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    void CancelActive()
    {
        isActive = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }
}
