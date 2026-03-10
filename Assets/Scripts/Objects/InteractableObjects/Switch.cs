using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] float animationRoutineTime = 0.2f;
    [SerializeField] float timeToDeactivate = 120f;

    [Header("Connections")]
    [SerializeField] Switch connectedSwitch;
    [SerializeField] GameObject connectedObjectToSwitch;

    [Header("References")]
    [SerializeField] Animator anim;

    bool isActive;
    bool isSentry;

    Coroutine deactiveRoutine;

    public bool GetActiveState() => isActive;
    public bool GetSentryState() =>isSentry;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;
        if (collision.gameObject.CompareTag("Enemy")) return;

        if (connectedSwitch == null)
        {
            TriggerSingleState();
        }
        else
        {
            TriggerMultipleState();
        }
    }

    void TriggerSingleState()
    {
        Activate();
    }

    void TriggerMultipleState()
    {
        if (connectedSwitch.GetSentryState())
        {
            Activate();
            connectedSwitch.Activate();
        }
        else
        {
            SetSentry();
        }
    }

    public void Activate()
    {
        if (isActive) return;

        isActive = true;
        isSentry = false;

        anim.SetInteger("SwitchState", 2);

        ;

        StartCoroutine(ActiveModeDelay());
    }

    void SetSentry()
    {
        if (isSentry) return;

        isActive = false;
        isSentry = true;

        anim.SetInteger("SwitchState", 1);
        StartCoroutine(SentryModeDelay());
    }

    IEnumerator ActiveModeDelay()
    {
        yield return new WaitForSeconds(0.2f);

        anim.SetBool("isActive", true);

        if (deactiveRoutine != null)
            StopCoroutine(deactiveRoutine);
        
        deactiveRoutine = StartCoroutine(BackToBase());
    }

    IEnumerator SentryModeDelay()
    {
        yield return new WaitForSeconds(0.2f);

        anim.SetBool("isSentry", true);

        if (deactiveRoutine != null)
            StopCoroutine(deactiveRoutine);
        
        deactiveRoutine = StartCoroutine(BackToBase());
    }

    IEnumerator BackToBase()
    {
        yield return new WaitForSeconds(timeToDeactivate);

        isActive = false;
        isSentry = false;

        anim.SetBool("isActive", false);
        anim.SetBool("isSentry", false);

        anim.SetInteger("SwitchState", 0);
    }
}
