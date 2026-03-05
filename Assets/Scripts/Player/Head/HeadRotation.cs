using System;
using System.Collections;
using UnityEngine;

public class HeadRotation : MonoBehaviour
{
    public event Action<Vector2> OnHeadRotate;

    [SerializeField] float maxRotationTime = 2f;
    [SerializeField] float rotationSpeed = 5f;
    
    float currentSpeed;
    float elaped;
    bool isSpinning;

    void OnEnable()
    {
        StartRotation();
    }

    void Update()
    {
        if (!isSpinning) return;

        elaped += Time.deltaTime;

        float t = elaped / maxRotationTime;
        currentSpeed = Mathf.Lerp(rotationSpeed, 0, t);

        float x = Mathf.Sin(Time.time * currentSpeed);
        float y = Mathf.Cos(Time.time * currentSpeed);

        OnHeadRotate?.Invoke(new Vector2(x, y));

        if (elaped >= maxRotationTime)
        {
            isSpinning = false;
            OnHeadRotate?.Invoke(Vector2.zero);
        }
    }

    void StartRotation()
    {
        elaped = 0f;
        currentSpeed = rotationSpeed;
        isSpinning = true;
    }
}
