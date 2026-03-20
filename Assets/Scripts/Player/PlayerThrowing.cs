using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerThrowing : MonoBehaviour
{
    public event Action OnHeadLoss;
    public event Action OnHeadPickup;

    [Header("Objects To Throw")]
    [SerializeField] GameObject headObject;
    [SerializeField] GameObject otherObject;

    [Header("Throw Parameters")]
    [SerializeField] float collisionOffsetTime = 0.2f;
    [SerializeField] float maxChargeTime = 3f;
    [SerializeField] float throwForce = 10f;
    float currentCharge;

    [Header("Do you have your head?")]
    [SerializeField] bool hasHead = true;

    [Header("Powerbar")]
    [SerializeField] Image powerbar;

    Coroutine throwCharge;

    void Start()
    {
        if (headObject != null)
        {
            HeadCollection hc = headObject.GetComponent<HeadCollection>();

            if (hc != null)
                hc.OnHeadCollect += CollectObject;
        }
    }

    void OnEnable()
    {
        PlayerInputManager.Instance.OnThrow += StartCharge;
        PlayerInputManager.Instance.OnThrowCancelled += CancelThrow;
    }

    void OnDisable()
    {
        PlayerInputManager.Instance.OnThrow -= StartCharge;
        PlayerInputManager.Instance.OnThrowCancelled -= CancelThrow;
    }

    void StartCharge()
    {
        if (throwCharge != null) return;
        if (!hasHead) return;

        throwCharge = StartCoroutine(ChargeThrow());
    }

    void CancelThrow(Vector2 aimDirection)
    {
        if (throwCharge != null)
        {
            powerbar.enabled = false;
            powerbar.fillAmount = 0;
            StopCoroutine(throwCharge);
            ThrowObject(aimDirection);
            throwCharge = null;
        }
    }

    IEnumerator ChargeThrow()
    {
        currentCharge = 0f;
        powerbar.fillAmount = 0;
        powerbar.enabled = true;

        while (currentCharge < maxChargeTime)
        {
            currentCharge += Time.deltaTime;
            powerbar.fillAmount += 0.01f;
            yield return null;
        }

        currentCharge = maxChargeTime;
        powerbar.fillAmount = maxChargeTime;
        powerbar.enabled = false;
    }

    void ThrowObject(Vector2 aimDirection)
    {
        GameObject objectToThrow = null;

        if (hasHead && otherObject == null)
        {
            objectToThrow = headObject;
            hasHead = false;
            OnHeadLoss?.Invoke();
        }
        else if (otherObject != null && hasHead)
        {
            objectToThrow = otherObject;
        }
        else return;

        IThrowable throwable = objectToThrow.GetComponent<IThrowable>();

        if (throwable == null) return;

        float chargePercent = currentCharge / maxChargeTime;
        float finalForce = chargePercent * throwForce;
        Vector2 spawnOffset = aimDirection.normalized * 0.5f;
        Vector2 spawnPos = (Vector2)transform.position + spawnOffset;

        objectToThrow.transform.parent = null;

        Rigidbody2D objRb = objectToThrow.GetComponent<Rigidbody2D>();
        objectToThrow.transform.position = spawnPos;
        
        StartCoroutine(ObjectCollisionOffset(objectToThrow));

        objectToThrow.SetActive(true);
        throwable.SetState(IThrowable.State.Thrown);

        objRb.AddForce(aimDirection.normalized * finalForce, ForceMode2D.Impulse);
        
        if (objectToThrow == otherObject)
        {
            otherObject = null;
        }
    }

    void CollectObject(GameObject objectToCollect)
    {
        if (otherObject == null)
        {
            otherObject = objectToCollect;

            otherObject.SetActive(false);
            otherObject.transform.SetParent(transform);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Head"))
        {
            headObject = collision.gameObject;
            headObject.transform.SetParent(this.transform);
            headObject.SetActive(false);
            headObject.transform.position = this.transform.position;
            hasHead = true;

            OnHeadPickup?.Invoke();

            if (headObject != null)
            {
                HeadCollection hc = headObject.GetComponent<HeadCollection>();
                
                if (hc != null)
                    hc.OnHeadCollect += CollectObject;
            }
        }
    }

    IEnumerator ObjectCollisionOffset(GameObject obj)
    {
        Collider2D objCol = obj.GetComponent<Collider2D>();

        objCol.enabled = false;

        yield return new WaitForSeconds(collisionOffsetTime);

        objCol.enabled = true;

        yield break;
    }

    public bool HasHead() => hasHead;
}
