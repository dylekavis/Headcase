using UnityEngine;
using System;
using Unity.VisualScripting;

public class MovingPlatform : MonoBehaviour
{
    public event Action<Transform> OnPlatformEnter;
    public event Action OnPlatformExit;

    [SerializeField] float moveSpeed;

    [Header("Track Points")]
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] Transform currentTarget;

    [Header("Connections")]
    [SerializeField] SwitchManager switchManager;
    [SerializeField] PressurePlate plate;

    Rigidbody2D rb;
    PlayerController pc;
    bool isActive;
    bool activatedBySwitch;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        currentTarget = endPoint;
    }

    void OnEnable()
    {
        if (switchManager != null)
        {
            switchManager.OnAllActivate += HandleActive;
            switchManager.OnAllDeactivate += CancelActive;
        }

        if (plate != null)
        {
            plate.OnPlateActivate += HandleActive;
            plate.OnPlateDeactivate += CancelActive;
        }
    }

    void OnDisable()
    {
        if (switchManager != null)
        {
            switchManager.OnAllActivate -= HandleActive;
            switchManager.OnAllDeactivate -= CancelActive;
        }

        if (plate != null)
        {
            plate.OnPlateActivate -= HandleActive;
            plate.OnPlateDeactivate -= CancelActive;
        }
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

        if (pc != null && pc.isOnPlatform)
            pc.gameObject.transform.position = Vector2.Lerp(pc.transform.position, rb.position, moveSpeed * Time.deltaTime);

        if (Vector2.Distance(rb.position, currentTarget.position) < 0.1f)
        {
            if (currentTarget == startPoint)
            {
                currentTarget = endPoint;
            }
            else
            {
                currentTarget = startPoint;
            }

            if (activatedBySwitch)
            {
                isActive = false;
                activatedBySwitch = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnPlatformEnter?.Invoke(this.transform);

            pc = collision.GetComponent<PlayerController>();
            pc.isOnPlatform = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnPlatformExit?.Invoke();
        }
    }

    void HandleActive() 
    { 
        isActive = true; 
        if (plate == null) activatedBySwitch = true;
    }
    void CancelActive() 
    { 
        isActive = false; 
    }
}
