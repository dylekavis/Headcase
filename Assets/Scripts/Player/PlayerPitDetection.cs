using System;
using System.Collections;
using UnityEngine;

public class PlayerPitDetection : MonoBehaviour
{
    public event Action<Vector2> OnPitDetected;
    public event Action OnPitUndetected;

    public event Action<Vector2> OnRespawnCreated;

    Vector2 respawnPoint;
    PlayerMovement pm;

    void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pit"))
        {
            Debug.Log($"{name} detected the pit");

            Vector2 moveDir = pm.GetMoveInput.normalized;

            respawnPoint = (Vector2)transform.position - moveDir * 1.5f;
            OnPitDetected?.Invoke(respawnPoint);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Pit"))
        {
            OnPitUndetected?.Invoke();
        }
    }
}
