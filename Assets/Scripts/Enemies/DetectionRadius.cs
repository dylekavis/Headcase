using System;
using UnityEngine;

public class DetectionRadius : MonoBehaviour
{
    public event Action<GameObject> OnPlayerDetected;
    public event Action OnPlayerUndetected;

    GameObject target;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            target = collision.gameObject;
            Debug.Log("Player detected");
            OnPlayerDetected?.Invoke(collision.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            target = null;
            Debug.Log("Player undetected");
            OnPlayerUndetected?.Invoke();
        }
    }
}
