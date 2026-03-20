using System;
using System.Collections;
using UnityEngine;

public class EnemyPitDetection : MonoBehaviour
{
    public event Action OnPitDetected;
    public event Action OnPitFall;

    [SerializeField] float pitfallAnimTime;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pit"))
        {
            OnPitDetected?.Invoke();
            Debug.Log($"{name} detected the pit");

            StartCoroutine(PitFall());
        }
    }

    IEnumerator PitFall()
    {
        yield return new WaitForSeconds(pitfallAnimTime);

        OnPitFall?.Invoke();
    }
}
