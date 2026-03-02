using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class PlayerThrowing : MonoBehaviour
{
    [SerializeField] GameObject headObject;
    [SerializeField] GameObject otherObject;
    [SerializeField] float maxChargeTime = 3f;
    [SerializeField] float throwForce = 10f;
    
    float currentCharge;

    bool hasHead = true;

    Coroutine throwCharge;

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

        throwCharge = StartCoroutine(ChargeThrow());
    }

    void CancelThrow()
    {
        if (throwCharge != null)
        {
            StopCoroutine(throwCharge);
            ThrowObject();
            throwCharge = null;
        }
    }

    IEnumerator ChargeThrow()
    {
        currentCharge = 0f;

        while (currentCharge < maxChargeTime)
        {
            currentCharge += Time.deltaTime;
            yield return null;
        }

        currentCharge = maxChargeTime;
    }

    void ThrowObject()
    {
        GameObject objectToThrow = null;

        if (hasHead)
        {
            objectToThrow = headObject;
            hasHead = false;
        }
        else if (otherObject != null)
        {
            objectToThrow = otherObject;
        }
        else return;

        float chargePercent = currentCharge / maxChargeTime;
        float finalForce = chargePercent * throwForce;

        Vector2 direction = GetComponent<PlayerMovement>().GetMoveInput.normalized;

        GameObject thrown = Instantiate(objectToThrow, transform.position, Quaternion.identity);
        Rigidbody2D thrownRb = thrown.GetComponent<Rigidbody2D>();

        thrownRb.AddForce(direction * finalForce, ForceMode2D.Impulse);
    }
}
