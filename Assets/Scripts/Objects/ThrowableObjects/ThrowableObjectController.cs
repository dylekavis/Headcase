using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class ThrowableObjectController : MonoBehaviour
{
    [SerializeField] Animator animator;
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
        isAbovePit = pit;
        animator.SetInteger("SwitchState", 0);
        StopAllCoroutines();
    }

    IEnumerator PitTimeOffsetRoutine()
    {
        yield return new WaitForSeconds(pitThrowOffsetTime);

        if (isAbovePit)
        {
            animator.SetInteger("SwitchState", 1);
            yield return new WaitForSeconds(0.1f);
            gameObject.SetActive(false);
            
            ThrowableObject to = GetComponent<ThrowableObject>();
            to.GetSpawnPool().ActiveCount -= 1;
        }
        else
        {
            yield break;
        }
    }
}
