using System;
using System.Collections;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    public event Action OnPitFall;
    public event Action OnPitCancel;

    [SerializeField] ObjectPitDetector pitDetector;
    [SerializeField] float pitThrowOffsetTime = 0.75f;

    bool isAbovePit;

    void OnEnable()
    {
        pitDetector.OnPitDetected += HandlePitDetection;
        pitDetector.OnPitUndetected += CancelPitDetection;
    }

    void OnDisable()
    {
        pitDetector.OnPitDetected -= HandlePitDetection;
        pitDetector.OnPitUndetected -= CancelPitDetection;
    }

    void HandlePitDetection(bool pit)
    {
        isAbovePit = pit;
        StartCoroutine(PitTimeOffsetRoutine());
    }
    
    void CancelPitDetection(bool pit)
    {
        OnPitCancel?.Invoke();
        isAbovePit = pit;
        StopAllCoroutines();
    }

    IEnumerator PitTimeOffsetRoutine()
    {
        yield return new WaitForSeconds(pitThrowOffsetTime);

        if (isAbovePit)
        {
            OnPitFall?.Invoke();
            yield return new WaitForSeconds(0.1f);
            gameObject.SetActive(false);
        }
        else
        {
            yield break;
        }
    }
}
