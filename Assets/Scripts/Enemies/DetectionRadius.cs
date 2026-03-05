using System;
using UnityEngine;

public class DetectionRadius : MonoBehaviour
{
    public event Action<GameObject> OnPlayerDetected;
    public event Action OnPlayerUndetected;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnPlayerDetected?.Invoke(collision.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnPlayerUndetected?.Invoke();
        }
    }
}
