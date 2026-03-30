using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class ObjectPitDetector : MonoBehaviour
{
    public event Action<bool> OnPitDetected;
    public event Action<bool> OnPitUndetected;

    [SerializeField] float coyoteTime = 0.25f;
    [SerializeField] ThrowableObjectController toc;
    
    bool isOverPit;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pit"))
        {
            isOverPit = true;
            StartCoroutine(PitOffset());
            return;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pit"))
        {
            isOverPit = false;
            OnPitUndetected?.Invoke(false);
        }
    }

    IEnumerator PitOffset()
    {
        yield return new WaitForSeconds(coyoteTime);

        if (toc.isOnPlatform) yield break;
        if (!isOverPit) yield break;

        OnPitDetected?.Invoke(true);
    }
}
