using System;
using UnityEngine;

public class ObjectPitDetector : MonoBehaviour
{
    public event Action<bool> OnPitDetected;
    public event Action<bool> OnPitUndetected;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pit"))
        {
            OnPitDetected?.Invoke(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pit"))
        {
            OnPitUndetected?.Invoke(false);
        }
    }
}
