using System;
using System.Collections;
using UnityEngine;

public class HeadRotation : MonoBehaviour
{
    public event Action<Vector2> OnHeadRotate;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip spinningClip;

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

    
        if (!audioSource.isPlaying)
        {
            float randomPitch = UnityEngine.Random.Range(0.9f, 1.1f);
            audioSource.pitch = randomPitch;
            audioSource.clip = spinningClip;
            audioSource.Play();
            audioSource.loop = true;
        }

        if (elaped >= maxRotationTime)
        {
            audioSource.Stop();
            audioSource.loop = false;
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
