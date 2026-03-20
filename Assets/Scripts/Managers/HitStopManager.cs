using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class HitStopManager : MonoBehaviour
{
    public static HitStopManager Instance;

    void Awake()
    {
        if (Instance != this && Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void Stop(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(HitStop(duration));
    }

    IEnumerator HitStop(float duration)
    {
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;

    }
}
