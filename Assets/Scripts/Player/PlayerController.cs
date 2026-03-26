using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerPitDetection pitDetection;
    HealthManager hm;

    Vector2 respawnPoint;
    public bool isOnPlatform;

    void Awake()
    {
        pitDetection = GetComponentInChildren<PlayerPitDetection>();
        hm = GetComponent<HealthManager>();
    }

    void Start()
    {
        SpawnPlayerInRoom.Instance.RegisterPlayer(this.gameObject);
    }

    void OnEnable() 
    {
        pitDetection.OnPitDetected += HandlePit;
    }

    void OnDisable() 
    { 
        pitDetection.OnPitDetected -= HandlePit;
    }

    void HandlePit(Vector2 spawnPoint)
    {
        if (isOnPlatform) return;
        hm.Damage(10);
        transform.position = spawnPoint;
    }

    public void SetOnPlatform(Transform platformTransform)
    {
        if (isOnPlatform) return;
        isOnPlatform = true;
        pitDetection.gameObject.SetActive(false);
        transform.SetParent(platformTransform);
    }

    public void SetOffPlatform()
    {
        if (!isOnPlatform) return;
        isOnPlatform = false;
        transform.SetParent(null);
        pitDetection.gameObject.SetActive(true);
    }
}
