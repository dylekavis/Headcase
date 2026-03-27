using System;
using System.Collections;
using UnityEngine;

public class PlayerPitDetection : MonoBehaviour
{
    public event Action<Vector2> OnPitDetected;
    public event Action OnPitUndetected;

    public event Action<Vector2> OnRespawnCreated;

    [SerializeField] float coyoteTime = 0.4f;
    [SerializeField] float triggerOffsetTime = 0.01f;
    [SerializeField] LayerMask pitLayer;
 
    Vector2 respawnPoint;
    PlayerMovement pm;
    PlayerController pc;

    void Start()
    {
        pm = GetComponentInParent<PlayerMovement>();
        pc = GetComponentInParent<PlayerController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.activeInHierarchy) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            gameObject.SetActive(false);
        }
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pit"))
        {
            if (pc.isOnPlatform) return;

            Debug.Log($"{name} detected the pit");

            Vector2 moveDir = pm.GetMoveInput.normalized;

            respawnPoint = (Vector2)transform.position - moveDir * 1.5f;

            StartCoroutine(PitOffset());
            return;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Pit"))
        {
            OnPitUndetected?.Invoke();
            StopAllCoroutines();
        }
    }

    IEnumerator PitOffset()
    {
        yield return new WaitForSeconds(coyoteTime);

        if (pc.isOnPlatform) yield break;

        OnPitDetected?.Invoke(respawnPoint);
    }
}
