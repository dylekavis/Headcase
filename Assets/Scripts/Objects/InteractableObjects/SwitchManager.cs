using System;
using System.Collections;
using UnityEngine;

public class SwitchManager : MonoBehaviour
{
    public static SwitchManager Instance;

    public event Action OnAllActivate;
    public event Action OnAllDeactivate;

    [SerializeField] float deactivateTime = 15f;

    [SerializeField] Switch[] connectedSwitches;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] selectionClips;
    [SerializeField] AudioClip allSelected;
    [SerializeField] AudioClip countDown;

    [SerializeField] float elapsed;
 
    int sentryCount;

    void Awake()
    {
        if (Instance != this && Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    public void RegisterSentry()
    {
        sentryCount++;

        int randomClip = UnityEngine.Random.Range(0, selectionClips.Length);

        for(int i = 0; i < selectionClips.Length; i++)
        {
            if (randomClip == i)
            {
                if (audioSource.isPlaying) return;
                audioSource.PlayOneShot(selectionClips[i]);
            }
        }

        if (sentryCount == connectedSwitches.Length)
            ActivateAll();
    }

    void ActivateAll()
    {
        foreach (var connect in connectedSwitches)
        {
            if (connect != null)
            {
                connect.SetState(SwitchState.Active);
            }
        }

        OnAllActivate?.Invoke();

        audioSource.PlayOneShot(allSelected);

        StartCoroutine(DeactivateAll());
    }

    IEnumerator DeactivateAll()
    {
        yield return new WaitUntil(() => !audioSource.isPlaying);

        elapsed = 0;
        
        while (elapsed < deactivateTime)
        {
            elapsed += 1f;
            audioSource.PlayOneShot(countDown);
            yield return new WaitForSecondsRealtime(1f);
        }

        foreach (var connect in connectedSwitches)
        {
            connect.SetState(SwitchState.Base);
        }

        OnAllDeactivate?.Invoke();
        
        sentryCount = 0;

        yield break;
    }
}
