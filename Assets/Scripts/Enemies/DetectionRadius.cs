using System;
using UnityEngine;

public class DetectionRadius : MonoBehaviour
{
    public event Action<GameObject> OnPlayerDetected;
    public event Action OnPlayerUndetected;

    GameObject player;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject;
            Debug.Log("Player detected");
            OnPlayerDetected?.Invoke(collision.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = null;
            Debug.Log("Player undetected");
            OnPlayerUndetected?.Invoke();
        }
    }
}
