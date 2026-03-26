using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    [Header("Track Points")]
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    Transform currentTarget;

    Rigidbody2D rb;
    PlayerController currentPlayer;
    public bool isActive;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        currentTarget = endPoint;
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

        Vector2 moveTo = Vector2.MoveTowards(rb.position, currentTarget.position, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(moveTo);

        if (Vector2.Distance(rb.position, currentTarget.position) < 0.05f)
            currentTarget = currentTarget == endPoint ? startPoint : endPoint;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            currentPlayer = other.GetComponent<PlayerController>();
            if (currentPlayer != null)
                currentPlayer.SetOnPlatform(transform);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (currentPlayer != null)
            {
                StartCoroutine(DelayedExit());
            }
        }
    }

    IEnumerator DelayedExit()
    {
        yield return new WaitForFixedUpdate();
        currentPlayer.SetOffPlatform();
        currentPlayer = null;
    }

    void HandleActive() { isActive = true; }
    void CancelActive() { isActive = false; }
}
