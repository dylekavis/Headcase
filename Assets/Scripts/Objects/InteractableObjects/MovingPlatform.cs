using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;

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
    List<GameObject> objectsOnPlatform = new();
    bool isActive;

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

        if (objectsOnPlatform != null)
        {
            foreach (var obj in objectsOnPlatform)
            {
                obj.transform.position = Vector2.Lerp(obj.transform.position, rb.position, moveSpeed * Time.deltaTime);
            }
        }

        if (Vector2.Distance(rb.position, currentTarget.position) < 0.1f)
        {
            currentTarget = (currentTarget == startPoint) ? endPoint : startPoint;

            isActive = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnPlatformEnter?.Invoke(this.transform);

            PlayerController pc = collision.GetComponent<PlayerController>();
            pc.isOnPlatform = true;

            objectsOnPlatform.Add(collision.gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("ThrowableObject"))
        {
            ThrowableObjectController toc = collision.GetComponent<ThrowableObjectController>();
            BombSpiderController bc = collision.GetComponent<BombSpiderController>();
            
            if (toc != null)
                toc.isOnPlatform = true;

            if (bc != null)
                bc.isOnPlatform = true;

            objectsOnPlatform.Add(collision.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnPlatformExit?.Invoke();

            objectsOnPlatform.Remove(collision.gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("ThrowableObject"))
        {
            ThrowableObjectController toc = collision.GetComponent<ThrowableObjectController>();
            BombSpiderController bc = collision.GetComponent<BombSpiderController>();

           if (toc != null)
                toc.isOnPlatform = false;

            if (bc != null)
                bc.isOnPlatform = false;

            objectsOnPlatform.Remove(collision.gameObject);
        }
    }

    void HandleActive() 
    { 
        isActive = true;
    }
    void CancelActive() 
    { 
        isActive = false; 
    }
}
