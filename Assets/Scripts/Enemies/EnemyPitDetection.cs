using System;
using System.Collections;
using UnityEngine;

public class EnemyPitDetection : MonoBehaviour
{
    public event Action OnPitDetected;
    public event Action OnPitUndetected;

    [SerializeField] float pitfallAnimTime;
    [SerializeField] BombSpiderController bombSpiderController;

    bool isOverPit;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pit"))
        {
            Debug.Log($"{name} detected the pit");
            isOverPit = true;

            StartCoroutine(PitFall());
            return;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pit"))
        {
            isOverPit = false;
            OnPitUndetected?.Invoke();
        }
    }

    IEnumerator PitFall()
    {
        yield return new WaitForSeconds(pitfallAnimTime);

        if (bombSpiderController.isOnPlatform) yield break;

        OnPitDetected?.Invoke();
    }
}
