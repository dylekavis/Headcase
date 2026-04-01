using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{
    public event Action<Transform> OnPlatformEnter;
    public event Action OnPlatformExit;

    [SerializeField] float moveSpeed;

    [Header("Track Points")]
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] Transform currentTarget;
    [SerializeField] float waitTime = 2f;
    [SerializeField] float objSpeedOffset = 1.5f;

    [Header("Connections")]
    [SerializeField] SwitchManager switchManager;
    [SerializeField] PressurePlate plate;

    Rigidbody2D rb;
    List<GameObject> objectsOnPlatform = new();
    bool isActive;
    bool switchActive;

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
            switchManager.OnAllActivate += HandleSwitchActive;
            switchManager.OnAllDeactivate += CancelSwitchActive;
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
            switchManager.OnAllActivate -= HandleSwitchActive;
            switchManager.OnAllDeactivate -= CancelSwitchActive;
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
                Rigidbody2D objRb = obj.GetComponent<Rigidbody2D>();
                objRb.MovePosition(Vector2.MoveTowards(obj.transform.position, rb.position, moveSpeed * objSpeedOffset * Time.deltaTime));
            }
        }

        if (Vector2.Distance(rb.position, currentTarget.position) < 0.1f)
        {
            currentTarget = (currentTarget == startPoint) ? endPoint : startPoint;
            isActive = false;
            StartCoroutine(PlatformWait());
            
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OnPlatformEnter?.Invoke(this.transform);
            collision.transform.SetParent(this.transform);

            PlayerController pc = collision.GetComponent<PlayerController>();
            pc.isOnPlatform = true;

            objectsOnPlatform.Add(collision.gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("ThrowableObject"))
        {
            ThrowableObjectController toc = collision.GetComponent<ThrowableObjectController>();
            BombSpiderController bc = collision.GetComponent<BombSpiderController>();

            collision.transform.SetParent(this.transform);
            
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

            collision.transform.SetParent(null);

            objectsOnPlatform.Remove(collision.gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("ThrowableObject"))
        {
            ThrowableObjectController toc = collision.GetComponent<ThrowableObjectController>();
            BombSpiderController bc = collision.GetComponent<BombSpiderController>();

            collision.transform.SetParent(null);

           if (toc != null)
                toc.isOnPlatform = false;

            if (bc != null)
                bc.isOnPlatform = false;

            objectsOnPlatform.Remove(collision.gameObject);
        }
    }

    IEnumerator PlatformWait()
    {
        yield return new WaitForSeconds(waitTime);

        if (!switchActive)
        {
            isActive = false;
            yield break;
        }

        isActive = false;
    }

    void HandleActive() 
    { 
        isActive = true;
        switchActive = true;
    }
    void CancelActive() 
    { 
        isActive = false; 
    }

    void HandleSwitchActive() 
    { 
        switchActive = true;
        isActive = true;
    }
    void CancelSwitchActive() 
    { 
        switchActive = false; 
    }
}
